using UnityEngine;

namespace Sakura.Zoo.Animal.Behaviour.Prey
{
    public sealed class FrogBehaviourInstaller : AnimalBehaviourInstaller
    {
        [Space]
        [SerializeField] private FrogMovementController.Params _movementParams;

        protected override void BindBehaviour()
        {
            Container
                .BindInterfacesAndSelfTo<FrogBehaviour>()
                .AsSingle()
                .NonLazy();
        }

        protected override void BindMovementController()
        {
            Container
                .BindInterfacesAndSelfTo<FrogMovementController>()
                .AsSingle()
                .WithArguments(_movementParams)
                .NonLazy();
        }
    }
}