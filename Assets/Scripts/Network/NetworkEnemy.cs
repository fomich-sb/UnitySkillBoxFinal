using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkEnemy : NetworkBehaviour
    {
        [SerializeField] private float _interpolationSpeed = 5f;

        [Networked, HideInInspector] public Vector3 NetworkedPosition { get; set; }
        [Networked, HideInInspector] public Quaternion NetworkedRotation { get; set; }
        private NavMeshAgent navMeshAgent;
        private Vector3 _renderPosition;
        private Quaternion _renderRotation;
        private NetworkBonusController networkBonusController;


        public void Init(Vector3 pos)
        {
            if (Object.HasStateAuthority)
                NetworkedPosition = pos;

        }

        public override void Spawned()
        {
            transform.position = NetworkedPosition;
            _renderPosition = transform.position;
            if (Object.HasStateAuthority)
            {
                navMeshAgent = GetComponent<NavMeshAgent>();
            }
            networkBonusController = FindFirstObjectByType<NetworkBonusController>();
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
            if(TryGetComponent(out NetworkObject NO)) {
                networkBonusController?.CheckNeedSpawn(NO.transform.position);
            }
            Runner.Despawn(NO);
        }
    }
}
