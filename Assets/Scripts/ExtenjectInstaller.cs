using Zenject;

namespace SkillBoxFinal
{
    public class ExtenjectInstaller : MonoInstaller
    {
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
        }
    }
}