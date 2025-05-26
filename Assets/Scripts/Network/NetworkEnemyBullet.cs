using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XInput;
using Zenject;
using static UnityEditor.PlayerSettings;

namespace SkillBoxFinal
{
    public class NetworkEnemyBullet : NetworkBehaviour, INetworkEnemyBullet
    {
        [Networked] private Vector3 Position { get; set; }
        [Networked, HideInInspector] public Vector3 TargetPosition { get; set; }
        public GameObject Prefab { get; set; }

        [Inject] private NetworkBulletController networkBulletController;
        private IEnemyBullet enemyBullet;

        public void Init(Vector3 pos, Vector3 targetPos, GameObject prefab)
        {
            Position = pos;
            TargetPosition = targetPos;
            Prefab = prefab;
        }

        override public void Spawned()
        {
            enemyBullet = GetComponent<IEnemyBullet>();
            enemyBullet.Init(Position, TargetPosition);
            enemyBullet.OnDespawn += Despawn;
        }

        public void Despawn()
        {
            enemyBullet.OnDespawn -= Despawn;
            if (networkBulletController && Runner.IsServer)
                networkBulletController.Despawn(GetComponent<NetworkObject>());
        }

        public void ReInit(Vector3 pos, Vector3 targetPos)
        {
            Position = pos;
            TargetPosition = targetPos;
            RPC_ReInit(pos, targetPos);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_ReInit(Vector3 pos, Vector3 targetPos)
        {
            enemyBullet.OnDespawn += Despawn;
            enemyBullet.Init(pos, targetPos);
        }
    }
}
