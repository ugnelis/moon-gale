using MoonGale.Runtime.Utilities;
using NaughtyAttributes;
using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    [CreateAssetMenu(
        menuName = Constants.CreateAssetMenuName + "/Level Settings",
        fileName = "New " + nameof(LevelSettings)
    )]
    internal sealed class LevelSettings : ScriptableObject
    {
        [Header("General")]
        [ShowAssetPreview]
        [SerializeField]
        private Texture2D map;

        [Header("Block Sizes")]
        [Min(1)]
        [SerializeField]
        private int blockWidth = 5;

        [Min(1)]
        [SerializeField]
        private int blockHeight = 5;

        [Header("Prefabs")]
        [SerializeField]
        private LevelElement solidBlockPrefab;

        [SerializeField]
        private LevelElement airBlockPrefab;

        public Texture2D Map => map;

        public int BlockWidth => blockWidth;

        public int BlockHeight => blockHeight;

        public LevelElement SolidBlockPrefab => solidBlockPrefab;

        public LevelElement AirBlockPrefab => airBlockPrefab;
    }
}
