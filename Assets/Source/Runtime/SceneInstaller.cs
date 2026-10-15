using BadPractice.Cursoriam.Helper.Pool;
using Sakura.Zoo.Helper;
using Sakura.Zoo.Manager;
using UnityEngine;
using Zenject;

namespace Sakura.Zoo
{
    public sealed class SceneInstaller : MonoInstaller
    {
        [SerializeField] private AnimalManager.SceneSettings _animalManagerSettings;

        public override void InstallBindings()
        {
            BindPools();
            BindManagerSettings();
            BindScreenBoundsChecker();
        }

        private void BindPools()
        {
            Container
                .BindInterfacesAndSelfTo<GameObjectPool>()
                .AsSingle()
                .NonLazy();
        }

        private void BindManagerSettings()
        {
            Container
                .BindInstance(_animalManagerSettings)
                .AsSingle()
                .WhenInjectedInto<AnimalManager>()
                .NonLazy();
        }

        private void BindScreenBoundsChecker()
        {
            Container
                .BindInterfacesAndSelfTo<ScreenBoundsChecker>()
                .AsSingle()
                .NonLazy();
        }
    }
}