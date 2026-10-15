using Sakura.Zoo.Animal.Behaviour.Prey;
using UnityEngine;

namespace Sakura.Zoo.Animal.Behaviour.Predator
{
    public sealed class SnakeBehaviourInstaller : AnimalBehaviourInstaller
    {
        [Space]
        [SerializeField] private SnakeMovementController.Params _movementParams;

        protected override void BindBehaviour()
        {
            Container
                .BindInterfacesAndSelfTo<SnakeBehaviour>()
                .AsSingle()
                .NonLazy();
        }

        protected override void BindMovementController()
        {
            Container
                .BindInterfacesAndSelfTo<SnakeMovementController>()
                .AsSingle()
                .WithArguments(_movementParams)
                .NonLazy();
        }
    }
}