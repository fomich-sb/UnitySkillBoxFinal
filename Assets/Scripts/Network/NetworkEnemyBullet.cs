using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XInput;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkEnemyBullet : NetworkBehaviour
    {
        [Networked] private Vector3 Position { get; set; }
        [Networked, HideInInspector] public Vector3 TargetPosition { get; set; }

        private NetworkBulletController networkBulletController;

        public void Init(Vector3 pos, Vector3 targetPos)
        {
            Position = pos;
            TargetPosition = targetPos;
        }

        override public void Spawned()
        {
            transform.position = Position;
            networkBulletController = FindFirstObjectByType<NetworkBulletController>();
        }

        public void Despawn()
        {
            if(networkBulletController && Runner.IsServer)
                networkBulletController.Despawn(GetComponent<NetworkObject>());
        }
    }
}
