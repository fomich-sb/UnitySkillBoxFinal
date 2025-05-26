using Fusion;
using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

namespace SkillBoxFinal
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private GameObject InfoPanel;
        [SerializeField] private AK.Wwise.Event wwiseEventDead;
        public int Score { get; set; } = 1;
        public bool IsBoss { get; set; } = false;

        [HideInInspector] public bool IsDead { get; set; } = false;
        [HideInInspector] public INetworkEnemy networkEnemy;
        [HideInInspector] public Transform targetPlayerTransform { get; set; }
        [HideInInspector] public IDamageable targetIDamageable { get; set; }
        [HideInInspector] public IPlayer TargetPlayerPlayer { get; set; }
        [HideInInspector] public float targetPlayerDistance { get; set; }
        private GameObject targetPlayer;
        private Camera _mainCamera;

        public event Action OnDead;

        [HideInInspector] public GameObject TargetPlayer
        {
            get
            {
                return targetPlayer;
            }
            set
            {
                targetPlayer = value;
                OnSetTarget();
            }
        }

        public void Init(GameObject _targetPlayer, bool isBoss)
        {
            TargetPlayer = _targetPlayer;
            IsBoss = isBoss;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            if(TryGetComponent(out IDamageable damageable))
                damageable.OnDead += Dead;
            networkEnemy = GetComponent<INetworkEnemy>();
        }

        private void OnDestroy()
        {
            if (TryGetComponent(out IDamageable damageable))
                damageable.OnDead -= Dead;
        }

        private void Update()
        {
            InfoPanel.transform.rotation = _mainCamera.transform.rotation;
            if (targetPlayerTransform)
                targetPlayerDistance = Vector3.Distance(targetPlayerTransform.position, transform.position);
        }

        public void OnSetTarget()
        {
            targetPlayerTransform = targetPlayer.transform;
            targetIDamageable = targetPlayer.GetComponent<IDamageable>();
            TargetPlayerPlayer = targetPlayer.GetComponent<IPlayer>();
        }

        public void Dead()
        {
            if (wwiseEventDead != null)
                wwiseEventDead.Post(gameObject);

            IsDead = true;

            OnDead?.Invoke();
            Invoke("Despawn", 3f);
        }

        private void Despawn()
        {
            networkEnemy?.Despawn();
        }

        public void ReInit()
        {
            IsDead = false;
        }
    }
}
