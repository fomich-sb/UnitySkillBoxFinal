using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public class NetworkBulletController : NetworkBehaviour
    {
        [SerializeField] private NetworkObject _bulletPrefab;

        public void Despawn(NetworkObject no)
        {
            Runner.Despawn(no);
        }

        public void Shoot(Vector3 spawnPosition, Vector3 targetPosition)
        {
            NetworkObject bulletObj = Runner.Spawn(
                _bulletPrefab,
                spawnPosition,
                Quaternion.identity,
                onBeforeSpawned: (runner, obj) =>
                {
                    obj.GetComponent<NetworkEnemyBullet>().Init(spawnPosition, targetPosition);
                }
            );
        }
    }
}
