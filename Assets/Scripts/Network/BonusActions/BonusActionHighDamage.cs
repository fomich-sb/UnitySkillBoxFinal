using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public class BonusActionHighDamage : MonoBehaviour, IBonusAction
    {
        [SerializeField] private int Value = 30;
        [SerializeField] private AK.Wwise.Event wwiseEvent;

        public bool Action(NetworkObject playerNO)
        {
            if (playerNO.TryGetComponent(out NetworkPlayer np))
            {
                np.HighDamageBullets += Value;
                RPC_Effect();
                GetComponent<NetworkBonus>().Despawn();
                return true;
            }
            return false;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_Effect()
        {
            if (wwiseEvent != null)
                wwiseEvent.Post(gameObject);
        }
    }
}
