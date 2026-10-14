using Sakura.Zoo.Animal.Behaviour;
using UnityEngine;
using Zenject;

namespace Sakura.Zoo.Animal
{
    public class AnimalEntity : MonoBehaviour, ITransformProvider
    {
        private IAnimalBehaviour _behaviour;

        [Inject]
        private void Construct(IAnimalBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        private void Update()
        {
            _behaviour.Update(Time.deltaTime);
        }

        public Transform Transform => (this && transform) ? transform : null;
    }
}