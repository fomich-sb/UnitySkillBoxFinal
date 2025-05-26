using Fusion;
using UnityEngine;
using Zenject;

namespace SkillBoxFinal
{
    public class BonusActionHealth : MonoBehaviour, IBonusAction
    {
        [SerializeField] private float Value = 100;
        [SerializeField] private AK.Wwise.Event wwiseEvent;
        [Inject] private IBonusFactory _bonusFactory;

        public bool Action(NetworkObject playerNO)
        {
            if (playerNO.TryGetComponent(out IHealthSystem h))
            {
                if (h.AddHealth(Value))
                {
                    RPC_Effect();
                    _bonusFactory.RecycleBonus(GetComponent<NetworkObject>());
                    return true;
                }
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
