using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Zenject.SpaceFighter;

namespace SkillBoxFinal
{

    public class EnemyAttackDistance : MonoBehaviour
    {
        [SerializeField] private float distance = 6f;
        [SerializeField] private float period = 2f;
        [SerializeField] private Transform bulletSpawnTransform;

        [Inject] private NetworkBulletController networkBulletController;
        private float _lastAttackTime = 0;
        private IEnemy _enemy;

        private void Start()
        {
            _enemy = GetComponent<IEnemy>();
        }

        private void Update()
        {
            if (_enemy.IsDead || _enemy.TargetPlayerPlayer is null || _enemy.TargetPlayerPlayer.IsDead) return;

            if (_enemy.TargetPlayer && _enemy.targetPlayerDistance < distance && Time.time - _lastAttackTime > period)
            {
                networkBulletController.Shoot(
                    bulletSpawnTransform.position, 
                    _enemy.targetPlayerTransform.position
                );
                _lastAttackTime = Time.time;
            }
        }
    }
}
