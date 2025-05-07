using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public class NetworkBonus : NetworkBehaviour
    {
        [Networked] private Vector3 Position { get; set; }
        private IBonusAction _action;
        private bool isActive=true;

        public void Init(Vector3 pos)
        {
            Position = pos;
        }

        override public void Spawned()
        {
            transform.position = Position;
            _action = GetComponent<IBonusAction>();
        }

        public void Action(GameObject playerGO)
        {
            NetworkObject playerNO = playerGO.GetComponent<NetworkObject>();
            if (playerNO.HasInputAuthority)
            {
                RPC_Action(playerNO);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_Action(NetworkObject playerNO)
        {
            if (isActive && Vector3.Distance(Position, playerNO.transform.position) < 1)
            {
                if (_action.Action(playerNO))
                    isActive = false;
            }
        }

        public void Despawn()
        {
            Runner.Despawn(GetComponent<NetworkObject>());
        }
    }
}
