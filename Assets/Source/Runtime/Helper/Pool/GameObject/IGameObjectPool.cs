using UnityEngine;
using Zenject;

namespace BadPractice.Cursoriam.Helper.Pool
{
    public interface IGameObjectPool
    {
        public void Setup(GameObject prefab, MemoryPoolSettings poolSettings = null, bool canRecreate = false);

        public T Spawn<T>(GameObject prefab, Transform parent = null, MemoryPoolSettings poolSettings = null)
            where T : Component, ICursoriamPoolable;

        T Spawn<T, TParam>(GameObject prefab, TParam param, Transform parent = null, MemoryPoolSettings poolSettings = null)
            where T : Component, ICursoriamPoolable<TParam>;

        public GameObject Spawn(GameObject prefab, Transform parent = null, MemoryPoolSettings poolSettings = null);

        public void Despawn<T>(T component) where T : Component, ICursoriamPoolableBase;

        public void Despawn(GameObject gameObject);
    }
}