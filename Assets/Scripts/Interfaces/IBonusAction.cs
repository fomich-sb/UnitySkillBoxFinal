
using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IBonusAction
    {
        public bool Action(NetworkObject playerGO);
    }
}
