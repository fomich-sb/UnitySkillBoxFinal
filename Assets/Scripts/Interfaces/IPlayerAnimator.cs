
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IPlayerAnimator
    {
        public void UpdateStatus(Vector3 direction);
        public void OnDead();
    }
}
