using System;
using UnityEngine;

namespace MoonGale.Core
{
    internal abstract class GameManager : MonoBehaviour
    {
        private const string GameManagerPrefabPath = "GameManager";
        private static GameManager instance;

        private readonly MessageManager messageManager = new();
        private readonly SystemManager systemManager = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (instance)
            {
                return;
            }

            var gameManagerPrefab = Resources.Load<GameManager>(GameManagerPrefabPath);
            var gameManager = Instantiate(gameManagerPrefab);

            gameManager.name = gameManager.GetApplicationName();

            DontDestroyOnLoad(gameManager);

            instance = gameManager;

            gameManager.OnInitialized();
        }

        /// <summary>
        /// Add a new message listener.
        /// </summary>
        public static void AddListener<TMessage>(Action<TMessage> listener) where TMessage : IMessage
        {
            instance.messageManager.AddListener(listener);
        }

        /// <summary>
        /// Remove an existing message listener.
        /// </summary>
        public static void RemoveListener<TMessage>(Action<TMessage> listener) where TMessage : IMessage
        {
            instance.messageManager.RemoveListener(listener);
        }

        /// <summary>
        /// Publish new message to the underlying systems of the game.
        /// </summary>
        public static void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            instance.messageManager.Publish(message);
        }

        /// <returns>
        /// Existing system in the game.
        /// </returns>
        public static TSystem GetSystem<TSystem>()
        {
            return instance.systemManager.GetSystem<TSystem>();
        }

        /// <returns>
        /// Name of the application (game).
        /// </returns>
        protected abstract string GetApplicationName();

        /// <summary>
        /// Called when game manager is initialized.
        /// </summary>
        protected abstract void OnInitialized();

        /// <summary>
        /// Add a new system to the game.
        /// </summary>
        protected void AddSystem<TSystem, TBindTo>(TSystem system) where TSystem : ISystem, TBindTo
        {
            systemManager.AddSystem<TSystem, TBindTo>(system);
        }
    }
}
