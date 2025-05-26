using Fusion;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SkillBoxFinal
{
    public class NetworkArmor : NetworkBehaviour, IArmorSystem
    {
        [Networked, OnChangedRender(nameof(OnValueChanged))] public float Value { get; set; } = 0;
        private readonly float max = 100f;

        public event Action OnChange;

        public float ReduceDamage(float damage)
        {
            float damageToArmor = Mathf.Min(Value, Mathf.Abs(damage));
            Value -= damageToArmor;
            return damage - damageToArmor;
        }

        public bool AddArmor(float value)
        {
            if (Value >= max) return false;
            Value = Mathf.Min(max, Value + value);
            return true;
        }

        private void OnValueChanged()
        {
            OnChange?.Invoke();
        }
    }
}
