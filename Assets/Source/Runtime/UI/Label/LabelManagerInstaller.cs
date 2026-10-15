using UnityEngine;
using Zenject;

namespace Sakura.Zoo.UI.Label
{
    [CreateAssetMenu(fileName = "LabelManagerInstaller", menuName = "Sakura/Installer/LabelManager")]
    public sealed class LabelManagerInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private LabelManager.Settings _settings;

        public override void InstallBindings()
        {
            BindManager();
        }

        private void BindManager()
        {
            Container
                .BindInterfacesAndSelfTo<LabelManager>()
                .AsSingle()
                .WithArguments(_settings)
                .NonLazy();
        }
    }
}