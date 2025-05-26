using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace SkillBoxFinal
{
    public class EnemyAnimator : MonoBehaviour, IEnemyAnimator
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            if (TryGetComponent(out IEnemy enemy))
                enemy.OnDead += OnDead;
        }

        public void SetMove()
        {
            animator?.SetBool("move", true);
            animator?.SetBool("attack", false);
        }

        public void SetAttack()
        {
            animator?.SetBool("move", false);
            animator?.SetBool("attack", true);
        }

        public void OnDead()
        {
            animator?.SetBool("dead", true);
        }

        public void ReInit()
        {
            animator?.SetBool("dead", false);
        }
    }
}
