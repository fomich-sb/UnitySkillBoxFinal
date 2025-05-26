using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkEnemyFactory : NetworkFactoryBase, IEnemyFactory
    {
        [Inject] private INetworkObjectPool networkObjectPool;

        public NetworkObject CreateEnemy(GameObject prefab, Vector3 position, GameObject targetPlayer, int enemyVolume, bool isBoss)
        {
            NetworkObject no = networkObjectPool.GetNetworkObject(prefab);
            if (no)
            {
                if (no.TryGetComponent<IHealthSystem>(out var health))
                    health.ReInit(enemyVolume);

                if (no.TryGetComponent<IEnemy>(out var enemyComponent))
                    enemyComponent.Init(targetPlayer, isBoss);

                no.GetComponent<INetworkEnemy>().ReInit(position);

                if (no.TryGetComponent<IEnemyAnimator>(out var enemyAnimator))
                    enemyAnimator.ReInit();
                return no;
            }

            return InstantiateNetworkObject(prefab, position, Quaternion.identity, PlayerRef.None, no =>
            {
                if (no.TryGetComponent<IHealthSystem>(out var health))
                    health.Init(enemyVolume);

                if (no.TryGetComponent<IEnemy>(out var enemyComponent))
                    enemyComponent.Init(targetPlayer, isBoss);

                no.GetComponent<INetworkEnemy>().Init(position, prefab);
                no.GetComponent<NavMeshAgent>().enabled = true;
                no.GetComponent<EnemyMove>().enabled = true;
            });

        }

        public void RecycleEnemy(NetworkObject enemy)
        {
            networkObjectPool.ReturnNetworkObject(enemy.GetComponent<INetworkEnemy>().Prefab, enemy);
        }
    }
}
