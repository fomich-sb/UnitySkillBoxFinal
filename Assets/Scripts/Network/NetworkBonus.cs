using Fusion;
using TMPro;
using UnityEngine;

namespace SkillBoxFinal
{
    public class NetworkBonus : NetworkBehaviour, INetworkBonus
    {
        [Networked] private Vector3 Position { get; set; }
        private IBonusAction _action;
        private bool isActive=true;
        public GameObject Prefab { get; set; }

        public void Init(Vector3 pos, GameObject prefab)
        {
            Position = pos;
            Prefab = prefab;
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

        public void ReInit(Vector3 pos)
        {
            Position = pos;
            RPC_ReInit(pos);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_ReInit(Vector3 pos)
        {
            transform.position = pos;
            isActive = true;
        }
    }
}
