
using Fusion;
using System;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface INetworkPlayer: IHighDamageBulletsSystem
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }
        public event Action OnInfoChanged;
        public void AddLevel();
        public void RequestGameOverStat();

    }
}
