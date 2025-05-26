using Fusion;
using System;
using UnityEngine;
using Zenject;

namespace SkillBoxFinal
{
    public class NetworkPlayer : NetworkBehaviour, IHighDamageBulletsSystem, INetworkPlayer
    {
        [Networked, HideInInspector] public string Name { get; set; }
        [Networked, OnChangedRender(nameof(OnLevelChanged)), HideInInspector] public int Level { get; set; }
        [Networked, OnChangedRender(nameof(OnScoreChanged)), HideInInspector] public int Score { get; set; }
        [Networked, OnChangedRender(nameof(OnHighDamageBulletsChanged)), HideInInspector] public int HighDamageBullets { get; set; }

        public event Action OnInfoChanged;

        private IPlayerInfoDisplay playerInfoDisplay;

        [Inject] private UIController uIController;

        override public void Spawned()
        {
            playerInfoDisplay = GetComponent<IPlayerInfoDisplay>();
            OnNameChanged();
            Level = 1;
            OnLevelChanged();
            HighDamageBullets = 0;
            OnHighDamageBulletsChanged();
        }

        private void OnNameChanged()
        {
            playerInfoDisplay.DisplayName(Name);
        }

        private void OnLevelChanged()
        {
            playerInfoDisplay.DisplayLevel(Level);
            OnInfoChanged?.Invoke();
        }
        private void OnScoreChanged()
        {
            playerInfoDisplay.DisplayScore(Score);
            OnInfoChanged?.Invoke();
        }
        private void OnHighDamageBulletsChanged()
        {
            playerInfoDisplay.DisplayHighDamageBullets(HighDamageBullets);
            OnInfoChanged?.Invoke();
        }

        public void AddLevel()
        {
            Level++;
        }
        public void RequestGameOverStat()
        {
            RPC_RequestGameOverStat();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_RequestGameOverStat()
        {
            IPlayerAttack p = GetComponent<IPlayerAttack>();
            RPC_UpdateGameOverStat(p.ShootCnt, p.ShootGoodCnt);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
        private void RPC_UpdateGameOverStat(int ShootCnt, int ShootGoodCnt)
        {
            uIController?.UpdateGameOverStat(ShootCnt, ShootGoodCnt);
        }
    }
}
