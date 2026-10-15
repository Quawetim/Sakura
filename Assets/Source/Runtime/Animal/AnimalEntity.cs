using Sakura.Zoo.Animal.Behaviour;
using UnityEngine;
using Zenject;

namespace Sakura.Zoo.Animal
{
    [RequireComponent(typeof(Rigidbody))]
    public class AnimalEntity : MonoBehaviour, ITransformProvider, IRigidbodyProvider
    {
        [SerializeField] private Rigidbody _rigidbody;

        private IAnimalBehaviour _behaviour;

        [Inject]
        private void Construct(IAnimalBehaviour behaviour)
        {
            _behaviour = behaviour;

            if (!_rigidbody)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
        }

        private void Awake()
        {
            _behaviour.Start(destroyCancellationToken);
        }

        public Transform Transform => (this && transform) ? transform : null;

        public Rigidbody Rigidbody => _rigidbody;
    }
}