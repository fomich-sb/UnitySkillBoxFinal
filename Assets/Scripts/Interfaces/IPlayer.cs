
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IPlayer
    {
        public event Action OnDead;
        public bool MyPlayer { get; set; }
        public bool Active { get; set; }

        public bool IsDead { get; set; }
    }
}
