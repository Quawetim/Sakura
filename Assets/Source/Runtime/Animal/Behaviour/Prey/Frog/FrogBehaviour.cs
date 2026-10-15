using Sakura.Zoo.Animal.Movement;

namespace Sakura.Zoo.Animal.Behaviour.Prey
{
    public sealed class FrogBehaviour : AnimalBehaviour, IPreyBehaviour
    {
        public FrogBehaviour(IMovementController movementController) : base(movementController)
        {
        }
    }
}