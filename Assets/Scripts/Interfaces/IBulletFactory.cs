
using Fusion;
using UnityEngine;

namespace SkillBoxFinal
{
    public interface IBulletFactory
    {
        NetworkObject CreateBullet(GameObject prefab, Vector3 position, Vector3 targetPosition);

        void RecycleBullet(INetworkEnemyBullet bullet);
        void RecycleBullet(NetworkObject bullet);
    }
}
