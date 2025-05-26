
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IPlayerAttack
    {
        public int ShootCnt { get; set; }
        public int ShootGoodCnt { get; set; }
        public bool Attack { get; set; }
        public bool IsServer { get; set; }
        public void Shoot();
    }
}
