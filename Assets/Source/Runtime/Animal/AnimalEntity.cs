using BadPractice.Cursoriam.Helper.Pool;
using Sakura.Zoo.Animal.Behaviour;
using Sakura.Zoo.Animal.Behaviour.Prey;
using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sakura.Zoo.Animal
{
    [RequireComponent(typeof(Rigidbody))]
    public class AnimalEntity : MonoBehaviour, ITransformProvider, IRigidbodyProvider, ICursoriamPoolable<GameObject>
    {
        [SerializeField] private Rigidbody _rigidbody;

        private IAnimalBehaviour _behaviour;
        private bool _isDead;
        private event Action<AnimalEntity> _onDeath;

        [Inject]
        private void Construct(IAnimalBehaviour behaviour)
        {
            _behaviour = behaviour;

            if (!_rigidbody)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
        }

        private void OnDestroy()
        {
            _onDeath = null;
        }

        public void Die()
        {
            _isDead = true;
            _onDeath?.Invoke(this);
        }

        public bool HasBehaviour<TBehaviour>()
            where TBehaviour : IAnimalBehaviour
        {
            return _behaviour is TBehaviour;
        }

        public void OnSpawned(GameObject spawnPoint)
        {
            _isDead = false;

            transform.position = spawnPoint.transform.position;
            transform.forward = spawnPoint.transform.forward;

            _behaviour.Start(destroyCancellationToken);
        }

        public void OnDespawned()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            _onDeath = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isDead || HasBehaviour<IPreyBehaviour>())
                return;

            if (!collision.gameObject.TryGetComponent<AnimalEntity>(out var animal))
                return;

            if (animal.HasBehaviour<IPreyBehaviour>())
            {
                animal.Die();
                return;
            }

            var chance = Random.value < 0.5;
            if (chance)
            {
                animal.Die();
                return;
            }

            Die();
        }

        public Transform Transform => (this && transform) ? transform : null;

        public Rigidbody Rigidbody => _rigidbody;

        public Type BehaviourType => _behaviour.InterfaceType;

        public event Action<AnimalEntity> onDeath
        {
            add => _onDeath += value;
            remove => _onDeath -= value;
        }
    }
}