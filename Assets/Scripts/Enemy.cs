using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

namespace SkillBoxFinal
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private GameObject InfoPanel;
        [SerializeField] private AK.Wwise.Event wwiseEventDead;
        public int Score=1;
        public bool IsBoss = false;

        [HideInInspector] public bool IsDead = false;
        [HideInInspector] public NetworkEnemy networkEnemy;
        [HideInInspector] public Animator animator;
        [HideInInspector] public Transform targetPlayerTransform;
        [HideInInspector] public NetworkHealth targetPlayerNetworkHealth;
        [HideInInspector] public Player TargetPlayerPlayer;
        [HideInInspector] public float targetPlayerDistance;
        private GameObject targetPlayer;
        private Camera _mainCamera;

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

        public void Init(GameObject _targetPlayer)
        {
            TargetPlayer = _targetPlayer;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            if(TryGetComponent(out NetworkHealth networkHealth))
                networkHealth.OnDead += Dead;
            animator = GetComponent<Animator>();
            networkEnemy = GetComponent<NetworkEnemy>();
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
            targetPlayerNetworkHealth = targetPlayer.GetComponent<NetworkHealth>();
            TargetPlayerPlayer = targetPlayer.GetComponent<Player>();
        }

        public void Dead()
        {
            if (wwiseEventDead != null)
                wwiseEventDead.Post(gameObject);

            IsDead = true;
            if(animator)
                animator.SetBool("dead", true);

            if (!networkEnemy) return;
            Invoke("Despawn", 3f);

        }

        private void Despawn()
        {
            networkEnemy.Despawn();
        }
    }
}
