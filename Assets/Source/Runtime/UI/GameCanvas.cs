using Sakura.Zoo.Animal.Behaviour.Predator;
using Sakura.Zoo.Animal.Behaviour.Prey;
using Sakura.Zoo.Manager;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Sakura.Zoo.UI
{
    public sealed class GameCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _deadPreysCounter;
        [SerializeField] private TextMeshProUGUI _deadPredatorsCounter;

        private IAnimalManager _animalManager;

        [Inject]
        private void Construct(IAnimalManager animalManager)
        {
            _animalManager = animalManager;
        }

        private void Start()
        {
            _deadPreysCounter.SetText("000");
            _deadPredatorsCounter.SetText("000");
        }

        private void OnEnable()
        {
            _animalManager.onAnimalDeath += OnAnimalDeath;
        }

        private void OnDisable()
        {
            _animalManager.onAnimalDeath -= OnAnimalDeath;
        }

        private void OnAnimalDeath(Type behaviourType)
        {
            var deaths = _animalManager.GetDeaths(behaviourType);

            if (typeof(IPreyBehaviour).IsAssignableFrom(behaviourType))
            {
                _deadPreysCounter.SetText(deaths.ToString("D3"));
                return;
            }

            if (typeof(IPredatorBehaviour).IsAssignableFrom(behaviourType))
            {
                _deadPredatorsCounter.SetText(deaths.ToString("D3"));
                return;
            }
        }
    }
}