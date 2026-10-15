using Cysharp.Threading.Tasks;
using Sakura.Zoo.Animal.Movement;
using System;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sakura.Zoo.Animal.Behaviour.Prey
{
    public sealed class FrogMovementController : IMovementController
    {
        private readonly Params _params;
        private readonly ITransformProvider _transformProvider;
        private readonly IRigidbodyProvider _rigidbodyProvider;

        private bool _isStarted;

        public FrogMovementController(Params @params, ITransformProvider transformProvider, IRigidbodyProvider rigidbodyProvider)
        {
            _params = @params;
            _transformProvider = transformProvider;
            _rigidbodyProvider = rigidbodyProvider;
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

                while (!ct.IsCancellationRequested)
                {
                    await UniTask.WaitForSeconds(_params.Timeout, cancellationToken: ct);

                    await Jump(transform, ct);
                    //JumpRb();
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

        private async UniTask Jump(Transform transform, CancellationToken ct)
        {
            var angle = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            var startPosition = transform.position;
            var targetPosition = startPosition + (_params.Distance * transform.forward);

            var currentTime = 0f;
            while (currentTime < _params.Duration)
            {
                currentTime += Time.deltaTime;

                var progress = currentTime / _params.Duration;
                var height = _params.GetHeight(progress);

                var position = Vector3.Lerp(startPosition, targetPosition, progress) + (height * transform.up);
                transform.position = position;

                await UniTask.NextFrame(ct);
            }

            transform.position = targetPosition;
        }

        private void JumpRb()
        {
            if (_params == null)
                return;

            var transform = _transformProvider?.Transform;
            var rigidBody = _rigidbodyProvider?.Rigidbody;
            if (!transform || !rigidBody)
                return;

            var angle = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            rigidBody.linearVelocity = Vector3.zero;

            var direction = (2f * transform.forward) + (2.5f * transform.up);
            rigidBody.AddForce(direction, ForceMode.Impulse);
        }

        [Serializable]
        public sealed class Params
        {
            [Range(0.5f, 5f)]
            [SerializeField] private float _timeout = 2f;
            [Range(0f, 2f)]
            [SerializeField] private float _distance = 1f;
            [Range(0f, 1f)]
            [SerializeField] private float _duration = 0.5f;

            [SerializeField] private AnimationCurve _trajectory = new(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));

            public float GetHeight(float progress)
            {
                return _trajectory.Evaluate(progress);
            }

            public float Timeout => _timeout;

            public float Distance => _distance;

            public float Duration => _duration;
        }
    }
}