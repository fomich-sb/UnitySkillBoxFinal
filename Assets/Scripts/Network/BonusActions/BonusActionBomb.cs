using Fusion;
using UnityEngine;
using Zenject;

namespace SkillBoxFinal
{
    public class BonusActionBomb : NetworkBehaviour, IBonusAction
    {
        [SerializeField] private float BombDamage = 20;
        [SerializeField] private float BombRadius = 10;
        [SerializeField] private LayerMask EnemyLayerMask;
        [SerializeField] private ParticleSystem Effect;
        [SerializeField] private AK.Wwise.Event wwiseEvent;
        [Inject] private IBonusFactory _bonusFactory;

        public bool Action(NetworkObject playerNO)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, BombRadius, EnemyLayerMask);
            foreach (Collider hit in colliders)
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float damageValue = (BombRadius - Vector3.Distance(hit.transform.position, transform.position)) / BombRadius * BombDamage;
                    IEnemy enemy = hit.GetComponent<IEnemy>();
                    if (enemy is not null && enemy.IsBoss)
                        damageable.Damage(damageValue, 1);
                    else
                        damageable.Damage(damageValue);
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
            _bonusFactory.RecycleBonus(GetComponent<NetworkObject>());
        }
    }
}
