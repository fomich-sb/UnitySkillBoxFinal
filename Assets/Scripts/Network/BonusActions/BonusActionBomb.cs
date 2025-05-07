using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public class BonusActionBomb : NetworkBehaviour, IBonusAction
    {
        [SerializeField] private float BombDamage = 20;
        [SerializeField] private float BombRadius = 10;
        [SerializeField] private LayerMask EnemyLayerMask;
        [SerializeField] private ParticleSystem Effect;
        [SerializeField] private AK.Wwise.Event wwiseEvent;

        public bool Action(NetworkObject playerNO)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, BombRadius, EnemyLayerMask);
            foreach (Collider hit in colliders)
            {
                NetworkHealth networkHealth = hit.GetComponent<NetworkHealth>();
                if (networkHealth != null)
                {
                    float damageValue = (BombRadius - Vector3.Distance(hit.transform.position, transform.position)) / BombRadius * BombDamage;
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy && enemy.IsBoss)
                        damageValue = Mathf.Min(damageValue, networkHealth.HealthValue-1);
                    networkHealth.Damage(damageValue);
                }
            }

            RPC_Effect();
            Invoke(nameof(Despawn), 0.5f);
            return true;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_Effect()
        {
            Effect.Play();

            if (wwiseEvent != null)
                wwiseEvent.Post(gameObject);
        }

        private void Despawn()
        {
            GetComponent<NetworkBonus>().Despawn();
        }
    }
}
