using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public class BonusActionArmor : NetworkBehaviour, IBonusAction
    {
        [SerializeField] private float Value = 100;
        [SerializeField] private AK.Wwise.Event wwiseEvent;

        public bool Action(NetworkObject playerNO)
        {
            if (playerNO.TryGetComponent(out NetworkHealth h))
            {
                if (h.ArmorValue < 100)
                {
                    h.AddArmor(Value);
                    RPC_Effect();
                    GetComponent<NetworkBonus>().Despawn();
                    return true;
                }
            }
            return false;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_Effect()
        {
            if (wwiseEvent != null)
            {
                wwiseEvent.Post(gameObject);
            }
        }

    }
}
