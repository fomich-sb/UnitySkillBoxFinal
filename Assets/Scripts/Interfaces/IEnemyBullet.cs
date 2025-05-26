
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IEnemyBullet
    {
        public Vector3 TargetPosition { get; set; }
        public void Init(Vector3 pos, Vector3 targetPosition);

        public event Action OnDespawn;
    }
}
