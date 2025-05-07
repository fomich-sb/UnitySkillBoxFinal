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
        private Enemy _enemy;

        private void Start()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void Update()
        {
            if (_enemy.IsDead || _enemy.TargetPlayerPlayer.IsDead) return;

            if (_enemy.TargetPlayer && _enemy.targetPlayerDistance < distance && Time.time - _lastAttackTime > period)
            {
                _enemy.targetPlayerNetworkHealth.Damage(damageValue);
                _lastAttackTime = Time.time;

                if (wwiseEvent != null)
                    wwiseEvent.Post(gameObject);
            }

            if (_enemy.TargetPlayer && _enemy.targetPlayerDistance < distance)
            {
                _enemy.animator.SetBool("move", false);
                _enemy.animator.SetBool("attack", true);
            }
            else
            {
                _enemy.animator.SetBool("move", true);
                _enemy.animator.SetBool("attack", false);
            }
        }
    }
}
