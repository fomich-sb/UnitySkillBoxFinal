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

        private NetworkBulletController networkBulletController;
        private float _lastAttackTime = 0;
        private Enemy _enemy;

        private void Start()
        {
            _enemy = GetComponent<Enemy>();
            networkBulletController = FindFirstObjectByType<NetworkBulletController>();
        }

        private void Update()
        {
            if (_enemy.IsDead || _enemy.TargetPlayerPlayer.IsDead) return;

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
