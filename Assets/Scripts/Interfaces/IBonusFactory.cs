
using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IBonusFactory
    {
        NetworkObject CreateBonus(GameObject prefab, Vector3 position);

        void RecycleBonus(NetworkObject bonus);
    }
}
