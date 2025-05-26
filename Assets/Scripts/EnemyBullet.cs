using Fusion;
using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace SkillBoxFinal
{
    public class EnemyBullet : MonoBehaviour, IEnemyBullet
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float damageValue = 5f;
        [SerializeField] private float explosionRadius = 2f;
        [SerializeField] private ParticleSystem Effect;
        [SerializeField] private LayerMask _playerLayerMask;
        [SerializeField] private AK.Wwise.Event wwiseEvent;
        [SerializeField] private AK.Wwise.Event wwiseEventExplode;

        [HideInInspector] public Vector3 TargetPosition { get; set; }
        private bool hasExploded = false;

        public event Action OnDespawn;


        private void Start()
        {
            if (wwiseEvent != null)
                wwiseEvent.Post(gameObject);
        }

        private void Update()
        {
            if (hasExploded) return;

            transform.position = Vector3.MoveTowards(
                transform.position,
                TargetPosition,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, TargetPosition) < 0.1f)
            {
                Explode();
            }
        }

        private void Explode()
        {
            if (hasExploded) return;

            wwiseEventExplode?.Post(gameObject);

            hasExploded = true;
            Effect.Play();
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, _playerLayerMask);
            foreach (Collider hit in colliders)
            {
                if (hit.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.Damage((explosionRadius - Vector3.Distance(hit.transform.position, transform.position))/explosionRadius * damageValue);
                }
            }
            Invoke(nameof(Despawn), 0.1f);
        }

        private void Despawn()
        {
            gameObject.SetActive(false);
            OnDespawn?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasExploded) return;
            Explode();
        }

        public void Init(Vector3 pos, Vector3 targetPosition)
        {
            hasExploded = false;
            transform.position = pos;
            TargetPosition = targetPosition;
            gameObject.SetActive(true);
        }
    }
}
