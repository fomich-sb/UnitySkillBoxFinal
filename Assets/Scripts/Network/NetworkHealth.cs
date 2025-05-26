using Fusion;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace SkillBoxFinal
{
    public class NetworkHealth : NetworkBehaviour, IDamageable, IHealthSystem
    {
        [Networked, OnChangedRender(nameof(OnHealthChanged))] public float Value { get; set; } = 100f;

        public bool IsDead { get; private set; } = false;

        private readonly float max = 100f;
        private IArmorSystem armorable;

        public event Action OnChange;
        public event Action OnDead;

        override public void Spawned()
        {
            armorable = GetComponent<IArmorSystem>();
            OnChange?.Invoke();
        }

        public void Init(float healthValue = 100)
        {
            Value = healthValue;
        } 

        private void OnHealthChanged()
        {
            OnChange?.Invoke();
            if (Value <= 0)
            {
                IsDead = true;
                OnDead?.Invoke();
            }
        }

        public bool Damage(float damage, float limit = 0)
        {
            if (IsDead) return false;
            damage = Mathf.Abs(damage);
            float damageRest = damage;
            if (armorable != null)
                damageRest = armorable.ReduceDamage(damageRest);

            damageRest = Mathf.Min(damageRest, Value - limit);

            if (Value <= damageRest)
            {
                Value = 0;
                return true;
            }
            else
                Value -= damageRest;

            return false;
        }

        public bool AddHealth(float value)
        {
            if (IsDead || Value >= max) return false;
            Value = Mathf.Min(max, Value + value);
            return true;
        }

        public void ReInit(float healthValue)
        {
            Value = healthValue;
            IsDead = false;
            RPC_ReInit();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_ReInit()
        {
            IsDead = false;
        }
    }
}
