using System.Collections.Generic;
using MoonGale.Core;
using MoonGale.Runtime.Player;
using MoonGale.Runtime.Systems;
using TMPro;
using UnityEngine;
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
        private string playerDeathMessage = "YOU DIED!";

        private bool isGameOver;
        private bool isPaused;

        private ISceneSystem sceneSystem;

        private void Awake()
        {
            sceneSystem = GameManager.GetSystem<ISceneSystem>();
        }

        private void Start()
        {
            ActiveCanvasGroup(statsCanvasGroup);
            DeactivateCanvasGroup(pauseCanvasGroup);
            DeactivateCanvasGroup(gameOverCanvasGroup);
        }

        private void OnEnable()
        {
            pauseInputActionReference.action.performed += OnPausePerformed;

            GameManager.AddListener<MenuSceneLoadedMessage>(OnMenuSceneLoadedMessage);
            GameManager.AddListener<MainSceneLoadedMessage>(OnMainSceneLoadedMessage);
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

            GameManager.RemoveListener<MenuSceneLoadedMessage>(OnMenuSceneLoadedMessage);
            GameManager.RemoveListener<MainSceneLoadedMessage>(OnMainSceneLoadedMessage);
            GameManager.RemoveListener<PlayerDeathMessage>(OnPlayerDeathMessage);

            foreach (var button in mainMenuGameButtons)
            {
                button.onClick.RemoveListener(OnMainMenuButtonClicked);
            }

            foreach (var button in restartGameButtons)
            {
                button.onClick.RemoveListener(OnRestartButtonClicked);
            }

            isGameOver = false;
            isPaused = false;
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
                Time.timeScale = 1f;
                isPaused = false;
            }
            else
            {
                ActiveCanvasGroup(pauseCanvasGroup);
                Time.timeScale = 0f;
                isPaused = true;
            }
        }

        private void OnMenuSceneLoadedMessage(MenuSceneLoadedMessage message)
        {
            ActiveCanvasGroup(statsCanvasGroup);
            DeactivateCanvasGroup(pauseCanvasGroup);
            DeactivateCanvasGroup(gameOverCanvasGroup);

            Time.timeScale = 1f;
            isGameOver = false;
        }

        private void OnMainSceneLoadedMessage(MainSceneLoadedMessage message)
        {
            ActiveCanvasGroup(statsCanvasGroup);
            DeactivateCanvasGroup(pauseCanvasGroup);
            DeactivateCanvasGroup(gameOverCanvasGroup);

            Time.timeScale = 1f;
            isGameOver = false;
        }

        private void OnPlayerDeathMessage(PlayerDeathMessage message)
        {
            gameOverText.text = playerDeathMessage;
            DeactivateCanvasGroup(statsCanvasGroup);
            ActiveCanvasGroup(gameOverCanvasGroup);

            // TODO: doesn't work, only triggers the second time
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameOverFocusButton.gameObject);

            Time.timeScale = 1f;
            isGameOver = true;
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
    }
}
