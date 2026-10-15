using Sakura.Zoo.Animal.Movement;
using System;
using System.Threading;

namespace Sakura.Zoo.Animal.Behaviour
{
    public abstract class AnimalBehaviour : IAnimalBehaviour
    {
        private readonly IMovementController _movementController;

        protected AnimalBehaviour(IMovementController movementController)
        {
            _movementController = movementController;
        }

        public virtual void Start(CancellationToken ct)
        {
            _movementController.Start(ct);
        }

        public abstract Type InterfaceType { get; }
    }
}