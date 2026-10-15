using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BadPractice.Cursoriam.Helper.Pool
{
    public sealed class GameObjectPool : IGameObjectPool, IDisposable
    {
        private readonly DiContainer _container;
        private readonly MemoryPoolSettings _poolSettings = new(1, 10, PoolExpandMethods.OneAtATime);
        private readonly Dictionary<GameObject, MemoryPool<GameObject>> _pools = new();
        private readonly Dictionary<Component, MemoryPool<GameObject>> _componentToPool = new();
        private readonly Dictionary<GameObject, Component> _componentCache = new();

        public GameObjectPool(DiContainer container)
        {
            _container = container;
        }

        public void Dispose()
        {
            foreach (var (_, pool) in _pools)
            {
                pool.Dispose();
            }

            _pools.Clear();
            _componentToPool.Clear();
            _componentCache.Clear();
        }

        public void Setup(GameObject prefab, MemoryPoolSettings poolSettings = null, bool canRecreate = false)
        {
            if (!prefab)
                return;

            poolSettings ??= _poolSettings;

            if (!_pools.TryGetValue(prefab, out var oldPool))
            {
                var newPool = CreatePool(prefab, poolSettings);
                _pools.Add(prefab, newPool);

                PrewarmPool(newPool, poolSettings.InitialSize);
                return;
            }

            if (!canRecreate)
            {
                if (poolSettings.InitialSize > oldPool.NumTotal)
                {
                    PrewarmPool(oldPool, poolSettings.InitialSize);
                }

                return;
            }

            var freshPool = CreatePool(prefab, poolSettings);
            _pools[prefab] = freshPool;

            var activeComponentsToMigrate = _componentToPool
                .Where(pair => pair.Value == oldPool)
                .Select(pair => pair.Key)
                .ToList();

            foreach (var component in activeComponentsToMigrate)
            {
                _componentToPool[component] = freshPool;
            }

            PrewarmPool(freshPool, poolSettings.InitialSize);

            if (oldPool is IDisposable disposablePool)
            {
                disposablePool.Dispose();
            }
        }

        private void PrewarmPool(MemoryPool<GameObject> pool, int targetSize)
        {
            var countToCreate = targetSize - pool.NumTotal;

            if (countToCreate <= 0)
                return;

            var temporaryList = new List<GameObject>(countToCreate);

            for (var i = 0; i < countToCreate; i++)
            {
                temporaryList.Add(pool.Spawn());
            }

            for (var i = 0; i < countToCreate; i++)
            {
                pool.Despawn(temporaryList[i]);
            }
        }

        public GameObject Spawn(GameObject prefab, Transform parent = null, MemoryPoolSettings poolSettings = null)
        {
            var component = SpawnInternal<Transform>(prefab, parent, poolSettings);
            return component ? component.gameObject : null;
        }

        public T Spawn<T>(GameObject prefab, Transform parent = null, MemoryPoolSettings poolSettings = null)
            where T : Component, ICursoriamPoolable
        {
            var component = SpawnInternal<T>(prefab, parent, poolSettings);
            component.OnSpawned();
            return component;
        }

        public T Spawn<T, TParam>(GameObject prefab, TParam param, Transform parent = null, MemoryPoolSettings poolSettings = null)
            where T : Component, ICursoriamPoolable<TParam>
        {
            var component = SpawnInternal<T>(prefab, parent, poolSettings);
            component.OnSpawned(param);
            return component;
        }

        public void Despawn(GameObject gameObject)
        {
            if (!gameObject)
                return;

            if (!gameObject.TryGetComponent<Transform>(out var transform))
                return;

            DespawnInternal(transform);
        }

        public void Despawn<T>(T component)
            where T : Component, ICursoriamPoolableBase
        {
            if (DespawnInternal(component))
            {
                if (component is ICursoriamPoolableBase poolable)
                {
                    poolable.OnDespawned();
                }
            }
        }

        private T SpawnInternal<T>(GameObject prefab, Transform parent = null, MemoryPoolSettings poolSettings = null) where T : Component
        {
            var pool = GetPool(prefab, poolSettings);
            var go = pool.Spawn();

            if (!go)
                return null;

            var targetComponent = GetComponent<T>(go);

            if (!targetComponent)
            {
                pool.Despawn(go);
                return null;
            }

            _componentToPool[targetComponent] = pool;

            go.SetActive(true);
            SetParent(go, parent);
            return targetComponent;
        }

        private bool DespawnInternal<T>(T component) where T : Component
        {
            if (!component)
                return false;

            if (!_componentToPool.Remove(component, out var pool))
                return false;

            if (!component.gameObject)
                return false;

            component.gameObject.SetActive(false);
            pool.Despawn(component.gameObject);
            return true;
        }

        private MemoryPool<GameObject> CreatePool(GameObject prefab, MemoryPoolSettings settings)
        {
            return _container.Instantiate<MemoryPool<GameObject>>(new object[]
            {
                settings,
                new GameObjectFactory(_container, prefab)
            });
        }

        private MemoryPool<GameObject> GetPool(GameObject prefab, MemoryPoolSettings poolSettings = null)
        {
            if (!prefab)
                return null;

            if (!_pools.TryGetValue(prefab, out var pool))
            {
                poolSettings ??= _poolSettings;
                pool = CreatePool(prefab, poolSettings);
                _pools.Add(prefab, pool);
            }

            return pool;
        }

        private T GetComponent<T>(GameObject go) where T : Component
        {
            if (_componentCache.TryGetValue(go, out var cachedComponent) && cachedComponent)
                return (T)cachedComponent;

            if (go.TryGetComponent<T>(out var component))
            {
                cachedComponent = component;
                _componentCache[go] = component;
            }
            else
            {
                Debug.LogError($"Prefab {go.name} doesn't contains component {typeof(T).Name}!");
                return null;
            }

            return (T)cachedComponent;
        }

        private void SetParent(GameObject go, Transform parent)
        {
            if (!parent)
                return;

            go.transform.SetParent(parent, false);
            go.transform.localPosition = Vector3.zero;
        }

        internal sealed class GameObjectFactory : IFactory<GameObject>
        {
            private readonly DiContainer _container;
            private readonly UnityEngine.Object _prefab;

            public GameObjectFactory(DiContainer container, UnityEngine.Object prefab)
            {
                _container = container;
                _prefab = prefab;
            }

            public GameObject Create()
            {
                return _container.InstantiatePrefab(_prefab);
            }
        }
    }
}