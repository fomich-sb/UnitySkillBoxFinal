using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkBonusController : NetworkBehaviour
    {
        [SerializeField] private GameObject[] bonusPrefabs;
        [Inject] private IBonusFactory _bonusFactory;
        private float[] bonusChances;

        override public void Spawned()
        {
            bonusChances = new float[bonusPrefabs.Length];
            for (int i = 0; i < bonusPrefabs.Length; i++)
                bonusChances[i] = bonusPrefabs[i].GetComponent<IBonus>().Chance;

            NetworkEnemy.OnDespawnAny += OnEnemyDespawn;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            NetworkEnemy.OnDespawnAny -= OnEnemyDespawn;
        }

        public void OnEnemyDespawn(Vector3 position)
        {
            CheckNeedSpawn(position);
        }

        public void CheckNeedSpawn(Vector3 pos)
        {
            float bonusValue = Random.value;
            float chanceSum = 0;
            for (int i = 0; i < bonusChances.Length; i++)
            {
                if (bonusValue < chanceSum + bonusChances[i])
                {
                    _bonusFactory.CreateBonus(bonusPrefabs[i], pos);
                    break;
                }
                chanceSum += bonusChances[i];
            }
        }
    }
}
