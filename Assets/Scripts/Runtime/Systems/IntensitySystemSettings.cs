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
        [Tooltip("How often does intensity increase?")]
        [Min(0)]
        [SerializeField]
        private int intensityIncreaseIntervalSeconds = 20;

        [Tooltip("Starting/min intensity")]
        [Min(0)]
        [SerializeField]
        private int minIntensity = 1;

        [Tooltip("By how much does the intensity increase (multiplier)?")]
        [Min(0)]
        [SerializeField]
        private int intensityMultiplier = 1;

        public int IntensityIncreaseIntervalSeconds => intensityIncreaseIntervalSeconds;

        public int MinIntensity => minIntensity;

        public int IntensityMultiplier => intensityMultiplier;
    }
}
