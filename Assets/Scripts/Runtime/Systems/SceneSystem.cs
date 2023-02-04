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

        public void LoadMainScene()
        {
            SceneManager.LoadScene(mainSceneBuildIndex);
            currentSceneBuildIndex = mainSceneBuildIndex;
            PublishMainSceneLoadedMessage();
        }

        public void LoadMenuScene()
        {
            SceneManager.LoadScene(menuSceneBuildIndex);
            currentSceneBuildIndex = menuSceneBuildIndex;
            PublishMenuSceneLoadedMessage();
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
