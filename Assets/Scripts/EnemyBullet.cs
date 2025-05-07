using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace SkillBoxFinal
{
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float damageValue = 5f;
        [SerializeField] private float explosionRadius = 2f;
        [SerializeField] private ParticleSystem Effect;
        [SerializeField] private LayerMask _playerLayerMask;
        [SerializeField] private AK.Wwise.Event wwiseEvent;
        [SerializeField] private AK.Wwise.Event wwiseEventExplode;

        private bool hasExploded = false;
        private NetworkEnemyBullet _networkEnemyBullet;


        private void Start()
        {
            _networkEnemyBullet = GetComponent<NetworkEnemyBullet>();

            if (wwiseEvent != null)
                wwiseEvent.Post(gameObject);
        }

        private void Update()
        {
            if (hasExploded) return;

            transform.position = Vector3.MoveTowards(
                transform.position,
                _networkEnemyBullet.TargetPosition,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, _networkEnemyBullet.TargetPosition) < 0.1f)
            {
                Explode();
            }
        }

        void Explode()
        {
            if (hasExploded) return;

            wwiseEventExplode?.Post(gameObject);

            hasExploded = true;
            Effect.Play();
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, _playerLayerMask);
            foreach (Collider hit in colliders)
            {
                if (hit.TryGetComponent<NetworkHealth>(out var networkHealth))
                {
                    networkHealth.Damage((explosionRadius - Vector3.Distance(hit.transform.position, transform.position))/explosionRadius * damageValue);
                }
            }
            Invoke(nameof(Despawn), 0.1f);
        }

        private void Despawn()
        {
            _networkEnemyBullet.Despawn();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasExploded) return;
            Explode();
        }
    }
}
