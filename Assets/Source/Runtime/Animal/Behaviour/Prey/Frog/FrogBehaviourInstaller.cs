namespace Sakura.Zoo.Animal.Behaviour.Prey
{
    public sealed class FrogBehaviourInstaller : AnimalBehaviourInstaller
    {
        protected override void BindBehaviour()
        {
            Container
                .BindInterfacesAndSelfTo<FrogBehaviour>()
                .AsSingle()
                .NonLazy();
        }
    }
}