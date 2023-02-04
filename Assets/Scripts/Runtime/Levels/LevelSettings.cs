using MoonGale.Runtime.Levels_Old;
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

        [SerializeField]
        private Vector3 blockSize = new(5f, 5f, 5f);

        [Header("Prefabs")]
        [SerializeField]
        private SolidBlockLevelElement solidBlockPrefab;

        public Texture2D Map => map;

        public Vector3 BlockSize => blockSize;

        public SolidBlockLevelElement SolidBlockPrefab => solidBlockPrefab;
    }
}
