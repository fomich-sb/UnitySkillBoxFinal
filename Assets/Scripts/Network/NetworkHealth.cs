using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public class NetworkHealth : NetworkBehaviour
    {
        [Networked, OnChangedRender(nameof(OnHealthChanged))] public float HealthValue { get; set; }
        [Networked, OnChangedRender(nameof(OnHealthChanged))] public float ArmorValue { get; set; }

        [HideInInspector] public delegate void OnDeadContainer();
        [HideInInspector] public event OnDeadContainer OnDead;
        private Health _health;
        private bool _dead=false;

        public void Init(float healthValue = 100, float armorValue = 0)
        {
            HealthValue = healthValue;
            ArmorValue = armorValue;
        }

        public override void Spawned()
        {
            _health = GetComponent<Health>();
            _health.Display(HealthValue, ArmorValue);
        }

        private void OnHealthChanged()
        {
            Display();
            if (HealthValue <= 0)
            {
                _dead = true;
                OnDead();
            }
        }

        public bool Damage(float damage)
        {
            if (_dead) return false;
            damage = Mathf.Abs(damage);
            float damageRest = damage;
            if (ArmorValue > 0)
            {
                damageRest -= Mathf.Min(ArmorValue, damage);
                ArmorValue -= Mathf.Min(ArmorValue, damage);
            }

            if (HealthValue <= damageRest)
            {
                HealthValue = 0;
                return true;
            }
            else
                HealthValue -= damageRest;

            return false;
        }

        public void AddHealth(float value)
        {
            if (_dead) return;
            HealthValue = Mathf.Min(100, HealthValue + value);
        }

        public void AddArmor(float value)
        {
            if (_dead) return;
            ArmorValue = Mathf.Min(100, ArmorValue + value);
        }

        public void Display()
        {
            _health.Display(HealthValue, ArmorValue);
        }
    }
}
