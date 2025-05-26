using Fusion;
using UnityEngine;
using Zenject;
using static UnityEditor.PlayerSettings;

namespace SkillBoxFinal
{
    public class NetworkBulletController : NetworkBehaviour
    {
        [SerializeField] private NetworkObject _bulletPrefab;
        [Inject] private IBulletFactory _bulletFactory;

        public void Despawn(NetworkObject no)
        {
            _bulletFactory.RecycleBullet(no);
            //Runner.Despawn(no);
        }

        public void Shoot(Vector3 spawnPosition, Vector3 targetPosition)
        {
            _bulletFactory.CreateBullet(_bulletPrefab.gameObject, spawnPosition, targetPosition);
        }
    }
}
