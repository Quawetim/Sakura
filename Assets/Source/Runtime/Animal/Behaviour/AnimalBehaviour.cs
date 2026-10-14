using UnityEngine;

namespace Sakura.Zoo.Animal.Behaviour
{
    public abstract class AnimalBehaviour : IAnimalBehaviour
    {
        private readonly ITransformProvider _transformProvider;

        protected AnimalBehaviour(ITransformProvider transformProvider)
        {
            _transformProvider = transformProvider;
        }

        public abstract void Update(float deltaTime);

        protected void SetPosition(Vector3 position)
        {
            if (!_transformProvider?.Transform)
                return;

            _transformProvider.Transform.position = position;
        }

        protected void SetRotation(Quaternion rotation)
        {
            if (!_transformProvider?.Transform)
                return;

            _transformProvider.Transform.rotation = rotation;
        }
    }
}