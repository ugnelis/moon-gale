using MoonGale.Core;
using UnityEngine;

namespace MoonGale.Runtime.Systems
{
    internal sealed class ScoreSystem : MonoBehaviour, ISystem, IScoreSystem
    {
        private static readonly string PlayerPrefsKey = typeof(ScoreSystem).FullName;
        private bool isTimerStarted;

        public float SurvivedTimeSeconds { get; private set; }

        private void Update()
        {
            if (isTimerStarted == false)
            {
                return;
            }

            SurvivedTimeSeconds += Time.deltaTime;
        }

        public void StartTimer()
        {
            SurvivedTimeSeconds = 0f;
            isTimerStarted = true;
        }

        public void StopTimer()
        {
            SaveScore(new ScoreData(SurvivedTimeSeconds));
            isTimerStarted = false;
        }

        public void SaveScore(ScoreData data)
        {
            if (TryGetBestScore(out var bestScore) == false)
            {
                var initialJson = JsonUtility.ToJson(data);
                PlayerPrefs.SetString(PlayerPrefsKey, initialJson);
                return;
            }

            if (data.SurvivedTimeSeconds < bestScore.SurvivedTimeSeconds)
            {
                return;
            }

            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(PlayerPrefsKey, json);
        }

        public bool TryGetBestScore(out ScoreData data)
        {
            var json = PlayerPrefs.GetString(PlayerPrefsKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                data = default;
                return false;
            }

            var scoreData = JsonUtility.FromJson<ScoreData>(json);
            if (scoreData == null)
            {
                data = default;
                return false;
            }

            data = scoreData;
            return true;
        }
    }
}
