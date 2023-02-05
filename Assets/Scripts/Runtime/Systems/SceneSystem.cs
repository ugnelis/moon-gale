using MoonGale.Core;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonGale.Runtime.Systems
{
    internal sealed class SceneSystem : MonoBehaviour, ISystem, ISceneSystem
    {
        [Scene]
        [SerializeField]
        private int menuSceneBuildIndex;

        [Scene]
        [SerializeField]
        private int mainSceneBuildIndex;

        private int currentSceneBuildIndex;

        public static bool IsSceneLoading { get; private set; }

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        public void LoadMainScene()
        {
            IsSceneLoading = true;
            SceneManager.LoadScene(mainSceneBuildIndex);

            currentSceneBuildIndex = mainSceneBuildIndex;
            PublishMainSceneLoadedMessage();
        }

        public void LoadMenuScene()
        {
            IsSceneLoading = true;
            SceneManager.LoadScene(menuSceneBuildIndex);

            currentSceneBuildIndex = menuSceneBuildIndex;
            PublishMenuSceneLoadedMessage();
        }

        private static void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            IsSceneLoading = false;
        }

        private static void PublishMainSceneLoadedMessage()
        {
            GameManager.Publish(new MainSceneLoadedMessage());
        }

        private static void PublishMenuSceneLoadedMessage()
        {
            GameManager.Publish(new MenuSceneLoadedMessage());
        }
    }
}
