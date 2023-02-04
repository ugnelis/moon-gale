using MoonGale.Core;
using MoonGale.Runtime.Player;
using UnityEngine;

namespace MoonGale.Runtime.Systems
{
    internal sealed class ScoreSystem : MonoBehaviour, ISystem, IScoreSystem
    {
        private static readonly string PlayerPrefsKey = typeof(ScoreSystem).FullName;
        private bool isTimerStarted;

        public float SurvivedTimeSeconds { get; private set; }

        private void OnEnable()
        {
            GameManager.AddListener<MenuSceneLoadedMessage>(OnMenuSceneLoaded);
            GameManager.AddListener<MainSceneLoadedMessage>(OnMainSceneLoaded);
            GameManager.AddListener<PlayerDeathMessage>(OnPlayerDeath);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<MenuSceneLoadedMessage>(OnMenuSceneLoaded);
            GameManager.RemoveListener<MainSceneLoadedMessage>(OnMainSceneLoaded);
            GameManager.RemoveListener<PlayerDeathMessage>(OnPlayerDeath);
        }

        private void OnMenuSceneLoaded(MenuSceneLoadedMessage message)
        {
            StopTimer();
        }

        private void OnMainSceneLoaded(MainSceneLoadedMessage message)
        {
            StartTimer();
        }

        private void OnPlayerDeath(PlayerDeathMessage message)
        {
            StopTimer();
        }

        private void Update()
        {
            if (isTimerStarted == false)
            {
                return;
            }

            SurvivedTimeSeconds += Time.deltaTime;
        }

        public void SaveScore(ScoreData data)
        {
            if (TryGetBestScore(out var bestScore) == false)
            {
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

        private void StartTimer()
        {
            SurvivedTimeSeconds = 0f;
            isTimerStarted = true;
        }

        private void StopTimer()
        {
            SaveScore(new ScoreData(SurvivedTimeSeconds));
            isTimerStarted = false;
        }
    }
}
