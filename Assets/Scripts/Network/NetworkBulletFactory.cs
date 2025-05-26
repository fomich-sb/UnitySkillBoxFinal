using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkBulletFactory : NetworkFactoryBase, IBulletFactory
    {
        [Inject] private INetworkObjectPool networkObjectPool;

        public NetworkObject CreateBullet(GameObject prefab, Vector3 position, Vector3 targetPosition)
        {
            NetworkObject no = networkObjectPool.GetNetworkObject(prefab);
            if (no)
            {
                no.GetComponent<INetworkEnemyBullet>().ReInit(position, targetPosition);
                return no;
            }

            return InstantiateNetworkObject(prefab, position, Quaternion.identity, PlayerRef.None, no =>
            {
                no.GetComponent<INetworkEnemyBullet>().Init(position, targetPosition, prefab);
            });
        }

        
        public void RecycleBullet(INetworkEnemyBullet bullet)
        {
            networkObjectPool.ReturnNetworkObject(bullet.Prefab, (bullet as MonoBehaviour).GetComponent<NetworkObject>());
        }
        public void RecycleBullet(NetworkObject bullet)
        {
            networkObjectPool.ReturnNetworkObject(bullet.GetComponent<INetworkEnemyBullet>().Prefab, bullet);
        }
    }
}
