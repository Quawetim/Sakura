using Cysharp.Threading.Tasks;
using Sakura.Zoo.Animal.Movement;
using System;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sakura.Zoo.Animal.Behaviour.Prey
{
    public sealed class SnakeMovementController : IMovementController
    {
        private readonly Params _params;
        private readonly ITransformProvider _transformProvider;

        private bool _isStarted;
        private float _rotationTimeout;

        public SnakeMovementController(Params @params, ITransformProvider transformProvider)
        {
            _params = @params;
            _transformProvider = transformProvider;
        }

        public void Start(CancellationToken ct)
        {
            if (_isStarted)
                return;

            StartAsync(ct).Forget();
        }

        private async UniTask StartAsync(CancellationToken ct)
        {
            if (_params == null || ct.IsCancellationRequested)
                return;

            var transform = _transformProvider?.Transform;
            if (!transform)
                return;

            try
            {
                _isStarted = true;
                _rotationTimeout = _params.RotationTimeout;

                while (!ct.IsCancellationRequested)
                {
                    await Move(transform, ct);
                }
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                _isStarted = false;
            }
        }

        private async UniTask Move(Transform transform, CancellationToken ct)
        {
            if (_rotationTimeout < 0)
            {
                var chance = Random.value < _params.RotationChance;

                if (chance)
                {
                    var angle = Random.Range(0f, 360f);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    _rotationTimeout = _params.RotationTimeout;
                }
            }

            _rotationTimeout -= Time.deltaTime;
            transform.position += _params.Speed * Time.deltaTime * transform.forward;

            await UniTask.NextFrame(ct);
        }

        [Serializable]
        public sealed class Params
        {
            [Range(0.5f, 5f)]
            [SerializeField] private float _speed = 0.8f;

            [Range(1f, 5f)]
            [SerializeField] private float _rotationTimeout = 3f;

            [Range(0f, 0.1f)]
            [SerializeField] private float _rotationChance = 0.05f;

            public float Speed => _speed;

            public float RotationTimeout => _rotationTimeout;

            public float RotationChance => _rotationChance;
        }
    }
}