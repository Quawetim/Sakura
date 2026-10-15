using BadPractice.Cursoriam.Helper.Pool;
using Sakura.Zoo.Animal;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Sakura.Zoo.UI.Label
{
    public sealed class LabelManager : ILabelManager, ITickable, IDisposable
    {
        private readonly Settings _settings;
        private readonly IGameObjectPool _gameObjectPool;
        private readonly LinkedList<LabelTimer> _spawnedLabels = new();

        private readonly float _offsetY;

        public LabelManager(Settings settings, IGameObjectPool gameObjectPool)
        {
            _settings = settings;
            _gameObjectPool = gameObjectPool;

            _offsetY = _settings.Prefab.transform.localPosition.y;
        }

        public void Dispose()
        {
            foreach (var timer in _spawnedLabels)
            {
                if (!timer.Label)
                    continue;

                _gameObjectPool.Despawn(timer.Label);
            }

            _spawnedLabels.Clear();
        }

        public void Show(ITransformProvider transformProvider, float offsetZ = -0.5f)
        {
            if (!_settings?.Prefab)
                return;

            var label = _gameObjectPool.Spawn(_settings.Prefab);
            label.transform.position = transformProvider.Transform.position + (offsetZ * Vector3.forward) + (_offsetY * Vector3.up);

            var timer = new LabelTimer
            {
                Label = label,
                Timeout = _settings.Timeout,
                OffsetZ = offsetZ,
                Target = transformProvider
            };

            _spawnedLabels.AddLast(new LinkedListNode<LabelTimer>(timer));
        }

        public void Tick()
        {
            var currentItem = _spawnedLabels.First;

            while (currentItem != null)
            {
                var nextItem = currentItem.Next;
                var timer = currentItem.Value;

                if (timer.Target != null && timer.Target.Transform)
                {
                    var position = timer.Target.Transform.position;
                    timer.Label.transform.position = position + (timer.OffsetZ * Vector3.forward) + (_offsetY * Vector3.up);
                }

                timer.Timeout -= Time.deltaTime;

                if (timer.Timeout < 0f)
                {
                    _gameObjectPool.Despawn(timer.Label);
                    _spawnedLabels.Remove(currentItem);
                }

                currentItem = nextItem;
            }
        }

        private sealed class LabelTimer
        {
            public GameObject Label;
            public float Timeout;
            public float OffsetZ;
            public ITransformProvider Target;
        }

        [Serializable]
        public sealed class Settings
        {
            [Range(0.5f, 1f)]
            [SerializeField] private float _timeout = 0.5f;
            [SerializeField] private GameObject _prefab;

            public float Timeout => _timeout;

            public GameObject Prefab => _prefab;
        }
    }
}