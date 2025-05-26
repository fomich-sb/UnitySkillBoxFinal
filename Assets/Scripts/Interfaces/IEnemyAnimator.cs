
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IEnemyAnimator
    {
        public void SetMove();
        public void SetAttack();
        public void OnDead();
        public void ReInit();
    }
}
