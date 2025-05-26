using Fusion;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Zenject.SpaceFighter;

namespace SkillBoxFinal
{
    public class NetworkBonusFactory : NetworkFactoryBase, IBonusFactory
    {
        [Inject] private INetworkObjectPool networkObjectPool;

        public NetworkObject CreateBonus(GameObject prefab, Vector3 position)
        {
            NetworkObject no = networkObjectPool.GetNetworkObject(prefab);
            if (no)
            {
                no.GetComponent<IBonus>().IsServer = true;
                no.GetComponent<INetworkBonus>().ReInit(position);
                return no;
            }

            return InstantiateNetworkObject(prefab, position, Quaternion.identity, PlayerRef.None, no =>
            {
                no.GetComponent<IBonus>().IsServer = true;
                no.GetComponent<INetworkBonus>().Init(position, prefab);
            });
        }

        public void RecycleBonus(NetworkObject bonus)
        {
            networkObjectPool.ReturnNetworkObject(bonus.GetComponent<INetworkBonus>().Prefab, bonus);
        }
    }
}
