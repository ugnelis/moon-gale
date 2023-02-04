using System;
using UnityEngine;

namespace MoonGale.Runtime.Systems
{
    [Serializable]
    internal sealed class ScoreData
    {
        [SerializeField]
        private float survivedTimeSeconds;

        public float SurvivedTimeSeconds => survivedTimeSeconds;

        public ScoreData(float survivedTimeSeconds)
        {
            this.survivedTimeSeconds = survivedTimeSeconds;
        }
    }
}
