
using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IEnemyFactory
    {
        NetworkObject CreateEnemy(GameObject prefab, Vector3 position, GameObject targetPlayer, int enemyVolume, bool isBoss);

        void RecycleEnemy(NetworkObject enemy);
    }
}
