using UnityEngine;
using Zenject;

namespace SkillBoxFinal
{
    public class ExtenjectInstaller : MonoInstaller
    {
        [SerializeField] private NetworkEnemyFactory _enemyFactory;
        [SerializeField] private NetworkPlayerFactory _playerFactory;
        [SerializeField] private NetworkBonusFactory _bonusFactory;
        [SerializeField] private NetworkBulletFactory _bulletFactory;

        public override void InstallBindings()
        {
            Container.Bind<Settings>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<GameController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<NetworkController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<NetworkEnemyController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<NetworkPlayerController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<InputController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<NetworkBulletController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<SoundController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<NetworkBonusController>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<UIController>()
                .FromComponentInHierarchy()
                .AsSingle();


            Container.Bind<INetworkObjectPool>()
                .FromComponentInHierarchy()
                .AsSingle();
            

            Container.Bind<IEnemyFactory>()
                .FromComponentInHierarchy(_enemyFactory)
                .AsSingle()
                .NonLazy();
            Container.Bind<IPlayerFactory>()
                .FromComponentInHierarchy(_playerFactory)
                .AsSingle()
                .NonLazy();
            Container.Bind<IBonusFactory>()
                .FromComponentInHierarchy(_bonusFactory)
                .AsSingle()
                .NonLazy();
            Container.Bind<IBulletFactory>()
                .FromComponentInHierarchy(_bulletFactory)
                .AsSingle()
                .NonLazy();
        }
        

    }
}