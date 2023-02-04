using MoonGale.Core;
using MoonGale.Runtime.Player;
using MoonGale.Runtime.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonGale.Runtime.UI
{
    internal sealed class GameplayUIController : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField]
        private CanvasGroup statsCanvasGroup;

        [SerializeField]
        private CanvasGroup gameOverCanvasGroup;

        [Header("Text Mesh Pro")]
        [SerializeField]
        private TMP_Text gameOverText;

        [Header("Buttons")]
        [SerializeField]
        private Button restartGameButton;

        [SerializeField]
        private Button mainMenuGameButton;

        [Header("Text")]
        [SerializeField]
        private string playerDeathMessage = "YOU DIED!";

        private ISceneSystem sceneSystem;

        private void Awake()
        {
            sceneSystem = GameManager.GetSystem<ISceneSystem>();
        }

        private void Start()
        {
            ActiveCanvasGroup(statsCanvasGroup);
            DeactivateCanvasGroup(gameOverCanvasGroup);
        }

        private void OnEnable()
        {
            GameManager.AddListener<MainSceneLoadedMessage>(OnMainSceneLoadedMessage);
            GameManager.AddListener<PlayerDeathMessage>(OnPlayerDeathMessage);
            mainMenuGameButton.onClick.AddListener(OnMainMenuButtonClicked);
            restartGameButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<MainSceneLoadedMessage>(OnMainSceneLoadedMessage);
            GameManager.RemoveListener<MainSceneLoadedMessage>(OnMainSceneLoadedMessage);
            GameManager.RemoveListener<PlayerDeathMessage>(OnPlayerDeathMessage);
            mainMenuGameButton.onClick.RemoveListener(OnMainMenuButtonClicked);
            restartGameButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        private void OnMainSceneLoadedMessage(MainSceneLoadedMessage message)
        {
            ActiveCanvasGroup(statsCanvasGroup);
            DeactivateCanvasGroup(gameOverCanvasGroup);
        }

        private void OnPlayerDeathMessage(PlayerDeathMessage message)
        {
            gameOverText.text = playerDeathMessage;
            DeactivateCanvasGroup(statsCanvasGroup);
            ActiveCanvasGroup(gameOverCanvasGroup);
        }

        private void OnMainMenuButtonClicked()
        {
            sceneSystem.LoadMenuScene();
        }

        private void OnRestartButtonClicked()
        {
            sceneSystem.ReloadScene();
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
