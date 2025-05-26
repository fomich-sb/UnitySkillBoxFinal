using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public class NetworkPlayerAttack : NetworkBehaviour, INetworkPlayerAttack
    {
        [HideInInspector] public NetworkObject HitNetworkObject { get; set; }
        private IPlayerAttack playerAttack;

        override public void Spawned()
        {
            playerAttack = GetComponent<IPlayerAttack>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                playerAttack.Attack = data.attack;
            }
        }

        public void SetHitObject(NetworkObject hitNetworkObject)
        {
            RPC_SetHitObject(hitNetworkObject);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_SetHitObject(NetworkObject hitNetworkObject)
        {
            HitNetworkObject = hitNetworkObject;
            playerAttack.Shoot();
        }
    }
}
