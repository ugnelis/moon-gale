using System;
using FMODUnity;
using MoonGale.Core;
using UnityEngine;

namespace MoonGale.Runtime.Systems
{
    internal sealed class IntensitySystem : MonoBehaviour, ISystem, IIntensitySystem
    {
        [SerializeField]
        private IntensitySystemSettings settings;

        [SerializeField]
        private StudioGlobalParameterTrigger globalParameterTrigger;

        private IScoreSystem scoreSystem;

        public int IntensityLevel { get; private set; }

        private int previousIntensityLevel;

        private void Awake()
        {
            scoreSystem = GameManager.GetSystem<IScoreSystem>();
        }

        private void OnEnable()
        {
            GameManager.AddListener<MainSceneLoadedMessage>(OnMainSceneLoaded);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<MainSceneLoadedMessage>(OnMainSceneLoaded);
        }

        private void Start()
        {
            UpdateSpawnRate();
        }

        private void Update()
        {
            UpdateSpawnRate();
        }

        private void OnMainSceneLoaded(MainSceneLoadedMessage message)
        {
            UpdateSpawnRate();
        }

        private void UpdateSpawnRate()
        {
            var survivedTimeSeconds = scoreSystem.SurvivedTimeSeconds;
            var multiplier = (int) (survivedTimeSeconds / settings.IntensityIncreaseIntervalSeconds) + 1;

            var oldIntensity = IntensityLevel;
            var newIntensity = multiplier * settings.IntensityMultiplier;

            IntensityLevel = Math.Max(settings.MinIntensity, newIntensity);

            if (oldIntensity != newIntensity)
            {
                OnIntensityLevelChanged(oldIntensity, newIntensity);
            }
        }

        private void OnIntensityLevelChanged(int oldIntensity, int newIntensity)
        {
            globalParameterTrigger.Value = newIntensity;
            globalParameterTrigger.TriggerParameters();
        }
    }
}
