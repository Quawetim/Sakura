namespace Sakura.Zoo.Animal.Behaviour.Predator
{
    public sealed class SnakeBehaviourInstaller : AnimalBehaviourInstaller
    {
        protected override void BindBehaviour()
        {
            Container
                .BindInterfacesAndSelfTo<SnakeBehaviour>()
                .AsSingle()
                .NonLazy();
        }
    }
}