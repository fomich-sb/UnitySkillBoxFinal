
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IHealthArmorDisplay
    {
        public bool MyPlayer { get; set; }
        public void Display();
        public void PlayShootHitAnimation(Vector3 position);
    }
}
