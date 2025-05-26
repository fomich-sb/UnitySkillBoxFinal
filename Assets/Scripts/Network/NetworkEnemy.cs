using Fusion;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

namespace SkillBoxFinal
{
    public class NetworkEnemy : NetworkBehaviour, INetworkEnemy
    {
        [SerializeField] private float _interpolationSpeed = 5f;

        [Networked, HideInInspector] public Vector3 NetworkedPosition { get; set; }
        [Networked, HideInInspector] public Quaternion NetworkedRotation { get; set; }
        private NavMeshAgent navMeshAgent;
        private Vector3 _renderPosition;
        private Quaternion _renderRotation;

        public static event Action<Vector3> OnDespawnAny;
        private IEnemyFactory _enemyFactory;
        public GameObject Prefab { get; set; }


        public void Init(Vector3 pos, GameObject prefab)
        {
            if (Object.HasStateAuthority)
            {
                NetworkedPosition = pos;
                Prefab = prefab;
            }
        }

        public override void Spawned()
        {
            transform.position = NetworkedPosition;
            _renderPosition = transform.position;
            if (Object.HasStateAuthority)
                navMeshAgent = GetComponent<NavMeshAgent>();
            _enemyFactory = FindFirstObjectByType<NetworkEnemyFactory>();
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority)
            {
                NetworkedPosition = navMeshAgent.nextPosition;
                if(navMeshAgent.velocity != Vector3.zero)
                    NetworkedRotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
            }
        }
        public override void Render()
        {
            if (Object.HasStateAuthority)
            {
                // Корректировка позиции агента для сервера
                navMeshAgent.nextPosition = transform.position;
            }
            else
            {
                // Плавная интерполяция для визуализации
                _renderPosition = Vector3.Lerp(
                    _renderPosition,
                    NetworkedPosition,
                    _interpolationSpeed * Runner.DeltaTime
                );

                _renderRotation = Quaternion.Slerp(
                    _renderRotation,
                    NetworkedRotation,
                    _interpolationSpeed * Runner.DeltaTime
                );

                transform.SetPositionAndRotation(_renderPosition, _renderRotation);

            }
        }

        public void Despawn()
        {
            OnDespawnAny?.Invoke(gameObject.transform.position);
            if (TryGetComponent(out NetworkObject NO)) {
                _enemyFactory.RecycleEnemy(NO);
            }
        }

        public void ReInit(Vector3 pos)
        {
            NetworkedPosition = pos;
            RPC_ReInit(pos);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_ReInit(Vector3 pos)
        {
            transform.position = pos;
            _renderPosition = transform.position;
            if (TryGetComponent(out IEnemy e))
            {
                e.ReInit();
            }
        }
    }
}
