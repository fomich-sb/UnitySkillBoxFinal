using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [Networked, HideInInspector] public string Name { get; set; }
        [Networked, OnChangedRender(nameof(OnLevelChanged)), HideInInspector] public int Level { get; set; }
        [Networked, OnChangedRender(nameof(OnScoreChanged)), HideInInspector] public int Score { get; set; }
        [Networked, OnChangedRender(nameof(OnHighDamageBulletsChanged)), HideInInspector] public int HighDamageBullets { get; set; }

        private Player player;

        override public void Spawned()
        {
            player = GetComponent<Player>();
            OnNameChanged();
            Level = 1;
            OnLevelChanged();
            HighDamageBullets = 0;
            OnHighDamageBulletsChanged();
        }

        private void OnNameChanged()
        {
            player.SetName(Name);
        }

        private void OnLevelChanged()
        {
            player.DisplayLevel(Level);
        }
        private void OnScoreChanged()
        {
            player.DisplayScore(Score);
        }
        private void OnHighDamageBulletsChanged()
        {
            player.DisplayHighDamageBullets(HighDamageBullets);
        }

        public void AddLevel()
        {
            Level++;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_RequestGameOverStat()
        {
            PlayerAttack p = GetComponent<PlayerAttack>();
            RPC_UpdateGameOverStat(p.ShootCnt, p.ShootGoodCnt);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
        private void RPC_UpdateGameOverStat(int ShootCnt, int ShootGoodCnt)
        {
            player.UpdateGameOverStat(ShootCnt, ShootGoodCnt);
        }
    }
}
