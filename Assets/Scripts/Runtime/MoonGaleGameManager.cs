﻿using MoonGale.Core;
using MoonGale.Runtime.Systems;
using UnityEngine;

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

        [Header("Controllers")]
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

            controllerParentTransform.gameObject.SetActive(true);
            systemsParentTransform.gameObject.SetActive(true);
        }
    }
}
