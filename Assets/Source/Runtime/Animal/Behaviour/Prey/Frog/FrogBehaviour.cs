using Sakura.Zoo.Animal.Movement;
using System;

namespace Sakura.Zoo.Animal.Behaviour.Prey
{
    public sealed class FrogBehaviour : AnimalBehaviour, IPreyBehaviour
    {
        public FrogBehaviour(IMovementController movementController) : base(movementController)
        {
        }

        public override Type InterfaceType => typeof(IPreyBehaviour);
    }
}