using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SkillBoxFinal
{
    public class NetworkPlayerFactory : NetworkFactoryBase, IPlayerFactory
    {
        public NetworkObject CreatePlayer(GameObject prefab, Vector3 position, PlayerRef playerRef, String playerName)
        {
            return InstantiateNetworkObject(prefab, position, Quaternion.identity, playerRef, no =>
            {
                no.GetComponent<INetworkPlayer>().Name = playerName;
                no.GetComponent<IPlayerAttack>().IsServer = true;
            });
        }

        public void RecyclePlayer(NetworkObject player)
        {
            Recycle(player);
        }
    }
}
