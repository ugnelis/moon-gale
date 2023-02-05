using System;
using System.Collections.Generic;
using MoonGale.Core;
using MoonGale.Runtime.Player;
using MoonGale.Runtime.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MoonGale.Runtime.UI
{
    internal sealed class GameplayUIController : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField]
        private CanvasGroup statsCanvasGroup;

        [SerializeField]
        private CanvasGroup pauseCanvasGroup;

        [SerializeField]
        private CanvasGroup gameOverCanvasGroup;

        [Header("Inputs")]
        [SerializeField]
        private InputActionReference pauseInputActionReference;

        [Header("Text Mesh Pro")]
        [SerializeField]
        private TMP_Text gameOverText;

        [SerializeField]
        private TMP_Text gameOverSurvivedTimeText;

        [SerializeField]
        private TMP_Text gameOverBestSurvivedTimeText;

        [SerializeField]
        private TMP_Text survivedTimeText;

        [SerializeField]
        private TMP_Text intensityLevelText;

        [Header("Buttons")]
        [SerializeField]
        private List<Button> restartGameButtons;

        [SerializeField]
        private List<Button> mainMenuGameButtons;

        [SerializeField]
        private Button pauseFocusButton;

        [SerializeField]
        private Button gameOverFocusButton;

        [Header("Text")]
        [SerializeField]
        private string timerTextFormat = "mm':'ss";

        [SerializeField]
        private string playerDeathMessage = "Game Over!";

        [SerializeField]
        private string newBestTimeSuffix = " (New Best)";

        [Header("Events")]
        [SerializeField]
        private UnityEvent onGameResumed;

        [SerializeField]
        private UnityEvent onGamePaused;

        private bool isGameOver;
        private bool isPaused;

        private IIntensitySystem intensitySystem;
        private IScoreSystem scoreSystem;
        private ISceneSystem sceneSystem;

        private void Awake()
        {
            intensitySystem = GameManager.GetSystem<IIntensitySystem>();
            scoreSystem = GameManager.GetSystem<IScoreSystem>();
            sceneSystem = GameManager.GetSystem<ISceneSystem>();
        }

        private void Start()
        {
            ActiveCanvasGroup(statsCanvasGroup);
            DeactivateCanvasGroup(pauseCanvasGroup);
            DeactivateCanvasGroup(gameOverCanvasGroup);
        }

        private void Update()
        {
            survivedTimeText.text = GetSurvivedTimeText();
            intensityLevelText.text = intensitySystem.IntensityLevel.ToString();
        }

        private void OnEnable()
        {
            pauseInputActionReference.action.performed += OnPausePerformed;
            GameManager.AddListener<PlayerDeathMessage>(OnPlayerDeathMessage);

            foreach (var button in mainMenuGameButtons)
            {
                button.onClick.AddListener(OnMainMenuButtonClicked);
            }

            foreach (var button in restartGameButtons)
            {
                button.onClick.AddListener(OnRestartButtonClicked);
            }
        }

        private void OnDisable()
        {
            pauseInputActionReference.action.performed -= OnPausePerformed;
            GameManager.RemoveListener<PlayerDeathMessage>(OnPlayerDeathMessage);

            foreach (var button in mainMenuGameButtons)
            {
                button.onClick.RemoveListener(OnMainMenuButtonClicked);
            }

            foreach (var button in restartGameButtons)
            {
                button.onClick.RemoveListener(OnRestartButtonClicked);
            }

            Resume();
            isGameOver = false;
        }

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            if (isGameOver)
            {
                return;
            }

            if (isPaused)
            {
                EventSystem.current.SetSelectedGameObject(pauseFocusButton.gameObject);
                DeactivateCanvasGroup(pauseCanvasGroup);
                Resume();
                onGameResumed?.Invoke();
            }
            else
            {
                ActiveCanvasGroup(pauseCanvasGroup);
                Pause();
                onGamePaused?.Invoke();
            }
        }

        private void Resume()
        {
            Time.timeScale = 1f;
            isPaused = false;
        }

        private void Pause()
        {
            Time.timeScale = 0f;
            isPaused = true;
        }

        private void OnPlayerDeathMessage(PlayerDeathMessage message)
        {
            gameOverText.text = playerDeathMessage;
            DeactivateCanvasGroup(statsCanvasGroup);
            ActiveCanvasGroup(gameOverCanvasGroup);

            // TODO: doesn't work, only triggers the second time
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameOverFocusButton.gameObject);

            var survivedTime = GetSurvivedTimeText();
            gameOverSurvivedTimeText.text = survivedTime;

            if (scoreSystem.TryGetBestScore(out var bestScore))
            {
                var bestSurvivedTime = GetSurvivedTimeText(bestScore.SurvivedTimeSeconds);
                if (Mathf.Approximately(bestScore.SurvivedTimeSeconds, scoreSystem.SurvivedTimeSeconds))
                {
                    gameOverBestSurvivedTimeText.text = bestSurvivedTime + newBestTimeSuffix;
                }
                else
                {
                    gameOverBestSurvivedTimeText.text = bestSurvivedTime;
                }
            }

            Resume();
        }

        private void OnMainMenuButtonClicked()
        {
            sceneSystem.LoadMenuScene();
        }

        private void OnRestartButtonClicked()
        {
            sceneSystem.LoadMainScene();
        }

        private static void ActiveCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        private static void DeactivateCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        private string GetSurvivedTimeText()
        {
            return GetSurvivedTimeText(scoreSystem.SurvivedTimeSeconds);
        }

        private string GetSurvivedTimeText(float seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            var timeText = timeSpan.ToString(timerTextFormat);

            return timeText;
        }
    }
}
