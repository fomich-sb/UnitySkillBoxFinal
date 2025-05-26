using Fusion;
using UnityEngine;
using Zenject;

namespace SkillBoxFinal
{
    public class BonusActionHighDamage : MonoBehaviour, IBonusAction
    {
        [SerializeField] private int Value = 30;
        [SerializeField] private AK.Wwise.Event wwiseEvent;
        [Inject] private IBonusFactory _bonusFactory;

        public bool Action(NetworkObject playerNO)
        {
            if (playerNO.TryGetComponent(out IHighDamageBulletsSystem hdba))
            {
                hdba.HighDamageBullets += Value;
                RPC_Effect();
                _bonusFactory.RecycleBonus(GetComponent<NetworkObject>());
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
