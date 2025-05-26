
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IEnemy
    {

        public event Action OnDead;
        public int Score { get; set; }
        public bool IsBoss { get; set; }
        public bool IsDead { get; set; }
        public IPlayer TargetPlayerPlayer { get; set; }
        public GameObject TargetPlayer { get; set; }
        public float targetPlayerDistance { get; set; }
        public IDamageable targetIDamageable { get; set; }
        public Transform targetPlayerTransform { get; set; }

        public void Init(GameObject _targetPlayer, bool isBoss);

        public void ReInit();

    }
}
