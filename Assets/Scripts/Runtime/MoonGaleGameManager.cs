using System;
using MoonGale.Core;
using MoonGale.Runtime.Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoonGale.Runtime
{
    internal sealed class MoonGaleGameManager : GameManager
    {
        [Header("Systems")]
        [SerializeField]
        private InputManagerSystem inputManagerSystem;

        [SerializeField]
        private SceneSystem sceneSystem;

        [SerializeField]
        private ScoreSystem scoreSystem;

        [SerializeField]
        private IntensitySystem intensitySystem;

        [Header("Management")]
        [SerializeField]
        private Transform controllerParentTransform;

        [SerializeField]
        private Transform systemsParentTransform;

        protected override string GetApplicationName()
        {
            return nameof(MoonGaleGameManager);
        }

        protected override void OnInitialized()
        {
            AddSystem<InputManagerSystem, IInputManagerSystem>(inputManagerSystem);
            AddSystem<SceneSystem, ISceneSystem>(sceneSystem);
            AddSystem<ScoreSystem, IScoreSystem>(scoreSystem);
            AddSystem<IntensitySystem, IIntensitySystem>(intensitySystem);

            controllerParentTransform.gameObject.SetActive(true);
            systemsParentTransform.gameObject.SetActive(true);

            var seed = DateTime.Now.Millisecond;
            Random.InitState(seed);
            Debug.Log($"Initialized game (random seed {seed})");
        }
    }
}
