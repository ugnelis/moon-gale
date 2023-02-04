using MoonGale.Core;
using MoonGale.Runtime.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace MoonGale.Runtime.UI
{
    internal sealed class MainMenuUIController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField]
        private Button startGameButton;

        [SerializeField]
        private Button exitGameButton;

        private ISceneSystem sceneSystem;

        private void Awake()
        {
            sceneSystem = GameManager.GetSystem<ISceneSystem>();
        }

        private void OnEnable()
        {
            startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            exitGameButton.onClick.AddListener(OnExitGameButtonClicked);
        }

        private void OnDisable()
        {
            startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
            exitGameButton.onClick.RemoveListener(OnExitGameButtonClicked);
        }

        private void OnStartGameButtonClicked()
        {
            sceneSystem.LoadMainScene();
        }

        private static void OnExitGameButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
