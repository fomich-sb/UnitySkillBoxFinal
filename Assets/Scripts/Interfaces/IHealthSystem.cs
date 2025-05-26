
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IHealthSystem
    {
        public float Value { get; set; }
        public void Init(float healthValue = 100);
        bool AddHealth(float amount);
        public event Action OnChange;
        public void ReInit(float healthValue);
    }
}
