
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IDamageable
    {
        bool Damage(float damage, float limit=0);

        bool IsDead { get; }

        public event Action OnDead;
        public event Action OnChange;
    }
}
