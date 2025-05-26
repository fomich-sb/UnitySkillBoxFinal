
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IArmorSystem
    {
        public float Value { get; set; }
        public float ReduceDamage(float damage);
        public bool AddArmor(float value);

        public event Action OnChange;
    }
}
