
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IPlayerInfoDisplay
    {
        public void DisplayName(string Name);

        public void HideMyName();

        public void DisplayLevel(int level);

        public void DisplayScore(int score);

        public void DisplayHighDamageBullets(int cnt);
    }
}
