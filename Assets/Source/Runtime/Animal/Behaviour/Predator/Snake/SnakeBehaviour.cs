using Sakura.Zoo.Animal.Movement;

namespace Sakura.Zoo.Animal.Behaviour.Predator
{
    public sealed class SnakeBehaviour : AnimalBehaviour, IPredatorBehaviour
    {
        public SnakeBehaviour(IMovementController movementController) : base(movementController)
        {
        }
    }
}