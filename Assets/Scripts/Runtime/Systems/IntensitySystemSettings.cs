using MoonGale.Runtime.Utilities;
using UnityEngine;

namespace MoonGale.Runtime.Systems
{
    [CreateAssetMenu(
        menuName = Constants.CreateAssetMenuName + "/Intensity System Settings",
        fileName = "New " + nameof(IntensitySystemSettings)
    )]
    internal sealed class IntensitySystemSettings : ScriptableObject
    {
        [Min(0)]
        [SerializeField]
        private int spawnRateIncreaseIntervalSeconds = 30;

        [Min(0)]
        [SerializeField]
        private int minSpawnRate = 1;

        [Min(0)]
        [SerializeField]
        private int spawnRateIncreaseAmount = 1;

        public int SpawnRateIncreaseIntervalSeconds => spawnRateIncreaseIntervalSeconds;

        public int MinSpawnRate => minSpawnRate;

        public int SpawnRateIncreaseAmount => spawnRateIncreaseAmount;
    }
}
