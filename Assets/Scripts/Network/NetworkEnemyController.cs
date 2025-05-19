using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkEnemyController : NetworkBehaviour
    {
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private int generatePeriod = 5;

        private Vector3[] enemySpawnPointPositions;
        private float nextGenerateTime = 0;
        private Dictionary<NetworkPlayer, int> bossGenerated = new();
        private Dictionary<NetworkPlayer, NetworkObject> bossNO = new();

        [Inject] private readonly GameController gameController;


        public override void FixedUpdateNetwork()
        {
            if (!Runner.IsServer) return;

            if (nextGenerateTime < Time.time)
            {
                Generate();
            }
        }

        public void SetEnemySpawnPoints(Transform[] _enemySpawnPoints) {

            enemySpawnPointPositions = new Vector3[_enemySpawnPoints.Length];
            for(int i=0; i<_enemySpawnPoints.Length; i++)
                enemySpawnPointPositions[i] = _enemySpawnPoints[i].position;
        }

        private void Generate()
        {
            if (gameController.ActiveNetworkPlayers.Count == 0) return;

            int enemyVolumeSum = 0;
            foreach (var networkPlayer in gameController.ActiveNetworkPlayers)
            {
                nextGenerateTime = Time.time + generatePeriod;

                enemyVolumeSum = 10 + networkPlayer.Level * 3;
                while (enemyVolumeSum > 0)
                {
                    int enemyVolume = Random.Range(networkPlayer.Level, 10+networkPlayer.Level);
                    if (enemyVolumeSum < enemyVolume)
                        enemyVolume = enemyVolumeSum;

                    int enemyPrefabIndex = Random.Range(0, enemyPrefabs.Length);
                    GameObject prefab = enemyPrefabs[enemyPrefabIndex];

                    Spawn(prefab, enemyVolume, networkPlayer);

                    enemyVolumeSum -= enemyVolume;
                }
                if (!bossGenerated.ContainsKey(networkPlayer))
                {
                    bossGenerated.Add(networkPlayer, 0);
                    bossNO.Add(networkPlayer, null);
                }
                if (!bossNO[networkPlayer] || bossGenerated[networkPlayer] < networkPlayer.Level)
                {
                    NetworkObject enemyNO = Spawn(bossPrefab, 30 + bossGenerated[networkPlayer] * 5, networkPlayer);
                    bossGenerated[networkPlayer]++;
                    bossNO[networkPlayer] = enemyNO;
                }
            }

        }

        private NetworkObject Spawn(GameObject prefab, int enemyVolume, NetworkPlayer networkPlayer)
        {
            int spawnIndex = Random.Range(0, enemySpawnPointPositions.Length);
            Vector3 spawnPosition = enemySpawnPointPositions[spawnIndex];
            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
                spawnPosition = hit.position;
            else
                Debug.Log("Enemy не касается NavMesh");

            NetworkObject enemyObj = Runner.Spawn(
                prefab.GetComponent<NetworkObject>(),
                spawnPosition,
                Quaternion.identity,
                onBeforeSpawned: (runner, obj) =>
                {
                    obj.GetComponent<NetworkHealth>().Init(enemyVolume);
                    obj.GetComponent<Enemy>().Init(networkPlayer.gameObject);
                    obj.GetComponent<NetworkEnemy>().Init(spawnPosition);
                    obj.GetComponent<NavMeshAgent>().enabled = true;
                    obj.GetComponent<EnemyMove>().enabled = true;
                }
            );
            return enemyObj;
        }
    }
}
