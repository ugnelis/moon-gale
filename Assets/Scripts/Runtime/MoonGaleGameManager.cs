using MoonGale.Core;
using MoonGale.Runtime.Systems;
using UnityEngine;

namespace MoonGale.Runtime
{
    internal sealed class MoonGaleGameManager : GameManager
    {
        [Header("Systems")]
        [SerializeField]
        private InputManagerSystem inputManagerSystem;

        protected override string GetApplicationName()
        {
            return nameof(MoonGaleGameManager);
        }

        protected override void OnInitialized()
        {
            AddSystem<InputManagerSystem, IInputManagerSystem>(inputManagerSystem);
        }
    }
}
