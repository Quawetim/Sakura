namespace Sakura.Zoo.Animal.Behaviour.Predator
{
    public sealed class SnakeBehaviour : AnimalBehaviour, IPredatorBehaviour
    {
        public SnakeBehaviour(ITransformProvider transformProvider) : base(transformProvider)
        {
        }

        public override void Update(float deltaTime)
        {
        }
    }
}