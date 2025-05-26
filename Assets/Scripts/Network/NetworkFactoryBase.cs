using Fusion;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace SkillBoxFinal
{
    public abstract class NetworkFactoryBase : NetworkBehaviour
    {
        [Inject] private DiContainer _container;

        protected NetworkObject InstantiateNetworkObject(GameObject prefab, Vector3 position, Quaternion rotation, PlayerRef playerRef, Action<NetworkObject> beforeSpawned = null)
        {
            if (Runner == null)
            {
                Debug.LogError("NetworkRunner не найден!");
                return null;
            }

            return Runner.Spawn(
                prefab.GetComponent<NetworkObject>(),
                position,
                rotation,
                playerRef,
                onBeforeSpawned: (runner, no) =>
                {
                    _container.InjectGameObject(no.gameObject);
                    beforeSpawned?.Invoke(no);
                }
            ); ;
        }

        protected void Recycle(NetworkObject bonus)
        {
            if (Runner == null)
            {
                Debug.LogError("NetworkRunner не найден2!");
                return;
            }

            Runner.Despawn(bonus);
        }
    }
}
