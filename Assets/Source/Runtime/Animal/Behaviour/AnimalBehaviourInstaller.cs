using Zenject;

namespace Sakura.Zoo.Animal.Behaviour
{
    public abstract class AnimalBehaviourInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAnimal();
            BindBehaviour();
        }

        private void BindAnimal()
        {
            Container
                .BindInterfacesAndSelfTo<AnimalEntity>()
                .FromMethod(_ => gameObject.GetComponent<AnimalEntity>())
                .AsSingle();
        }

        protected abstract void BindBehaviour();
    }
}