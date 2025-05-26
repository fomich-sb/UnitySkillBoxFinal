
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface INetworkObjectPool
    {

        public NetworkObject GetNetworkObject(GameObject prefab);

        public void ReturnNetworkObject(GameObject prefab, NetworkObject obj);

    }
}
