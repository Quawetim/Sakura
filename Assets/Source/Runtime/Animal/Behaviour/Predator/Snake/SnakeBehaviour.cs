using Sakura.Zoo.Animal.Movement;
using System;

namespace Sakura.Zoo.Animal.Behaviour.Predator
{
    public sealed class SnakeBehaviour : AnimalBehaviour, IPredatorBehaviour
    {
        public SnakeBehaviour(IMovementController movementController) : base(movementController)
        {
        }

        public override Type InterfaceType => typeof(IPredatorBehaviour);
    }
}