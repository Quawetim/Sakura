namespace Sakura.Zoo.Animal.Behaviour.Prey
{
    public sealed class FrogBehaviour : AnimalBehaviour, IPreyBehaviour
    {
        public FrogBehaviour(ITransformProvider transformProvider) : base(transformProvider)
        {
        }

        public override void Update(float deltaTime)
        {
        }
    }
}