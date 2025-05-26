
using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IBonus
    {
        public float Chance { get; set; }
        public bool IsServer { get; set; }
    }
}
