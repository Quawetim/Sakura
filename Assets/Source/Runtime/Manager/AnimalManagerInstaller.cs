using UnityEngine;
using Zenject;

namespace Sakura.Zoo.Manager
{
    [CreateAssetMenu(fileName = "AnimalManagerInstaller", menuName = "Sakura/Installer/AnimalManager")]
    public sealed class AnimalManagerInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private AnimalManager.Settings _settings;

        public override void InstallBindings()
        {
            BindManager();
        }

        private void BindManager()
        {
            Container
                .BindInterfacesAndSelfTo<AnimalManager>()
                .AsSingle()
                .WithArguments(_settings)
                .NonLazy();
        }
    }
}