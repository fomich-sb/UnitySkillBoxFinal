using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace SkillBoxFinal
{
    public class EnemyMove : MonoBehaviour
    {
        [SerializeField] private float updatePathRate = 2f;

        private NavMeshAgent navMeshAgent;
        private Enemy enemy;
        private float _lastUpdatePathTime;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            enemy = GetComponent<Enemy>();
        }

        private void Update()
        {
            if (enemy.IsDead)
            {
                navMeshAgent.isStopped = true;
                return;
            }

            if (Time.time - _lastUpdatePathTime > updatePathRate * (enemy.targetPlayerDistance > 5 ? 3 : 1) )
            {
                if (enemy.targetPlayerTransform)
                    navMeshAgent.SetDestination(enemy.targetPlayerTransform.position);
                _lastUpdatePathTime = Time.time;
            }
        }
    }
}
