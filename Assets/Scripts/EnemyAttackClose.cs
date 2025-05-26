using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace SkillBoxFinal
{

    public class EnemyAttackClose : MonoBehaviour
    {
        [SerializeField] private float distance = 1f;
        [SerializeField] private float damageValue = 1f;
        [SerializeField] private float period = 0.5f;
        [SerializeField] private AK.Wwise.Event wwiseEvent;

        private float _lastAttackTime = 0;
        private IEnemy _enemy;
        private IEnemyAnimator enemyAnimator;

        private void Start()
        {
            _enemy = GetComponent<IEnemy>();
            enemyAnimator = GetComponent<IEnemyAnimator>();
        }

        private void Update()
        {
            if (_enemy.IsDead || _enemy.TargetPlayerPlayer is null || _enemy.TargetPlayerPlayer.IsDead) return;

            if (_enemy.TargetPlayer && _enemy.targetPlayerDistance < distance && Time.time - _lastAttackTime > period)
            {
                _enemy.targetIDamageable.Damage(damageValue);
                _lastAttackTime = Time.time;

                if (wwiseEvent != null)
                    wwiseEvent.Post(gameObject);
            }

            if (_enemy.TargetPlayer && _enemy.targetPlayerDistance < distance)
                enemyAnimator.SetAttack();
            else
                enemyAnimator.SetMove();
        }
    }
}
