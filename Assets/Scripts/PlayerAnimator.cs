using Fusion;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject.SpaceFighter;

namespace SkillBoxFinal
{
    public class PlayerAnimator : MonoBehaviour, IPlayerAnimator
    {
        public Animator animator;

        private void Start()
        {
            if (TryGetComponent(out IPlayer player))
                player.OnDead += OnDead;
        }

        public void UpdateStatus(Vector3 direction)
        {
            if (direction == Vector3.zero)
            {
                animator.SetFloat("left", 0);
                animator.SetFloat("forward", 0);
            }
            else if (Mathf.Abs(direction.z) >= Mathf.Abs(direction.x))
            {
                animator.SetFloat("left", 0);
                animator.SetFloat("forward", direction.z > 0 ? -1 : 1);
            }
            else
            {
                animator.SetFloat("left", direction.x > 0 ? -1 : 1);
                animator.SetFloat("forward", 0);
            }
        }

        public void OnDead()
        {
            animator.SetBool("dead", true);
        }
    }
}
