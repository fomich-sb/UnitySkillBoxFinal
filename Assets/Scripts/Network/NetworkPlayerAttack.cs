using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public class NetworkPlayerAttack : NetworkBehaviour
    {
        [HideInInspector] public NetworkObject HitNetworkObject;
        private PlayerAttack playerAttack;

        override public void Spawned()
        {
            playerAttack = GetComponent<PlayerAttack>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                playerAttack.attack = data.attack;
            }
        }


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_SetHitObject(NetworkObject hitNetworkObject)
        {
            HitNetworkObject = hitNetworkObject;
            playerAttack.Shoot();
        }
    }
}
