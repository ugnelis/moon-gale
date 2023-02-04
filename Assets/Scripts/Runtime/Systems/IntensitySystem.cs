using System;
using MoonGale.Core;
using UnityEngine;

namespace MoonGale.Runtime.Systems
{
    internal sealed class IntensitySystem : MonoBehaviour, ISystem, IIntensitySystem
    {
        [SerializeField]
        private IntensitySystemSettings settings;

        private IScoreSystem scoreSystem;

        public int SpawnRate { get; private set; }

        private void Awake()
        {
            scoreSystem = GameManager.GetSystem<IScoreSystem>();
        }

        private void Start()
        {
            UpdateSpawnRate();
        }

        private void Update()
        {
            UpdateSpawnRate();
        }

        private void UpdateSpawnRate()
        {
            var survivedTimeSeconds = (int) scoreSystem.SurvivedTimeSeconds;
            var multiplier = survivedTimeSeconds / settings.SpawnRateIncreaseIntervalSeconds;
            var spawnRate = multiplier * settings.SpawnRateIncreaseAmount;

            SpawnRate = Math.Max(settings.MinSpawnRate, spawnRate);
        }
    }
}
