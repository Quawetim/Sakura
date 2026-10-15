using BadPractice.Cursoriam.Extension;
using BadPractice.Cursoriam.Helper.Pool;
using Sakura.Zoo.Animal;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sakura.Zoo.Manager
{
    public sealed class AnimalManager : ITickable, IDisposable
    {
        private readonly Settings _settings;
        private readonly SceneSettings _sceneSettings;
        private readonly IGameObjectPool _gameObjectPool;
        private readonly List<AnimalEntity> _spawnedAnimals = new(10);
        private readonly Dictionary<Type, int> _deathsCounter = new(2);
        private float _spawnTimeout;
        private Action<Type> _onAnimalDeath;

        public AnimalManager(Settings settings, SceneSettings sceneSettings, IGameObjectPool gameObjectPool)
        {
            _settings = settings;
            _sceneSettings = sceneSettings;
            _gameObjectPool = gameObjectPool;
        }

        public void Dispose()
        {
            foreach (var animal in _spawnedAnimals)
            {
                DespawnAnimal(animal, false);
            }

            _spawnedAnimals.Clear();
            _deathsCounter.Clear();

            _onAnimalDeath = null;
        }

        public void Tick()
        {
            _spawnTimeout -= Time.deltaTime;

            if (_spawnTimeout < 0f)
            {
                SpawnAnimal();
                _spawnTimeout = _settings.SpawnTimeout;
            }
        }

        public int GetDeaths(Type type)
        {
            return _deathsCounter.GetValueOrDefault(type, 0);
        }

        private void SpawnAnimal()
        {
            if (_settings?.AnimalPrefabs == null || _sceneSettings?.SpawnPoints == null)
                return;

            if (_settings.AnimalPrefabs.Length == 0 || _sceneSettings.SpawnPoints.Length == 0)
                return;

            var animalIndex = Random.Range(0, _settings.AnimalPrefabs.Length);
            var prefab = _settings.AnimalPrefabs[animalIndex].gameObject;

            var spawnPointIndex = Random.Range(0, _sceneSettings.SpawnPoints.Length);
            var spawnPoint = _sceneSettings.SpawnPoints[spawnPointIndex];

            var animal = _gameObjectPool.Spawn<AnimalEntity, GameObject>(prefab, spawnPoint, _sceneSettings.AnimalsParent);
            _spawnedAnimals.Add(animal);

            animal.onDeath += OnAnimalDeath;
        }

        private void DespawnAnimal(AnimalEntity animal, bool clear = true)
        {
            if (!animal)
                return;

            animal.onDeath -= OnAnimalDeath;
            _gameObjectPool.Despawn(animal);

            if (!clear)
                return;

            _spawnedAnimals.Remove(animal);
        }

        private void OnAnimalDeath(AnimalEntity animal)
        {
            DespawnAnimal(animal);

            var behaviourType = animal.BehaviourType;
            var deaths = _deathsCounter.GetValueOrDefault(behaviourType, 0) + 1;
            _deathsCounter.AddOrUpdate(behaviourType, deaths);

            _onAnimalDeath?.Invoke(behaviourType);
        }

        public event Action<Type> onAnimalDeath
        {
            add => _onAnimalDeath += value;
            remove => _onAnimalDeath -= value;
        }

        [Serializable]
        public sealed class Settings
        {
            [Range(1f, 3f)]
            [SerializeField] private float _spawnTimeout = 1.5f;
            [SerializeField] private AnimalEntity[] _animalPrefabs = { };

            public float SpawnTimeout => _spawnTimeout;

            public AnimalEntity[] AnimalPrefabs => _animalPrefabs;
        }

        [Serializable]
        public sealed class SceneSettings
        {
            [SerializeField] private GameObject[] _spawnPoints = { };
            [SerializeField] private Transform _animalsParent;

            public GameObject[] SpawnPoints => _spawnPoints;

            public Transform AnimalsParent => _animalsParent;
        }
    }
}