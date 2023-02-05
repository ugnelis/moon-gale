using System;
using System.Collections;
using MoonGale.Core;
using MoonGale.Runtime.UI;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonGale.Runtime.Systems
{
    internal sealed class SceneSystem : MonoBehaviour, ISystem, ISceneSystem
    {
        [Header("Scenes")]
        [Scene]
        [SerializeField]
        private int menuSceneBuildIndex;

        [Scene]
        [SerializeField]
        private int mainSceneBuildIndex;

        [Header("Transitions")]
        [SerializeField]
        private FadeCanvasUIController fadeCanvas;

        public static bool IsSceneLoading
        {
            get => isSceneLoading;
            private set
            {
                var oldValue = isSceneLoading;
                var newValue = value;

                isSceneLoading = value;

                if (oldValue == newValue)
                {
                    return;
                }

                if (value)
                {
                    GameManager.Publish(new SceneLoadStartedMessage());
                }
                else
                {
                    GameManager.Publish(new SceneLoadStoppedMessage());
                }
            }
        }

        private static bool isSceneLoading;

        public void LoadMainScene()
        {
            StartLoadScene(mainSceneBuildIndex, PublishMainSceneLoadedMessage);
        }

        public void LoadMenuScene()
        {
            StartLoadScene(menuSceneBuildIndex, PublishMenuSceneLoadedMessage);
        }

        private void StartLoadScene(int index, Action onLoaded)
        {
            StartCoroutine(LoadSceneRoutine(index, onLoaded));
        }

        private IEnumerator LoadSceneRoutine(int index, Action onLoaded)
        {
            if (IsSceneLoading)
            {
                yield break;
            }

            IsSceneLoading = true;

            yield return fadeCanvas.ShowCanvas();
            yield return SceneManager.LoadSceneAsync(index);
            yield return fadeCanvas.HideCanvas();

            onLoaded?.Invoke();

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
