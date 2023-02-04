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

        public void ReloadScene()
        {
            SceneManager.LoadScene(currentSceneBuildIndex);
            PublishSceneLoadedMessage();
        }

        public void LoadMainScene()
        {
            SceneManager.LoadScene(mainSceneBuildIndex);
            currentSceneBuildIndex = mainSceneBuildIndex;
            PublishSceneLoadedMessage();
        }

        public void LoadMenuScene()
        {
            SceneManager.LoadScene(menuSceneBuildIndex);
            currentSceneBuildIndex = menuSceneBuildIndex;
            PublishSceneLoadedMessage();
        }

        private static void PublishSceneLoadedMessage()
        {
            GameManager.Publish(new MainSceneLoadedMessage());
        }
    }
}
