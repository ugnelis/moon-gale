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

        [Min(0f)]
        [SerializeField]
        private float queryRadius = 10f;

        [Min(0f)]
        [SerializeField]
        private float blockSize = 5f;

        [Header("Prefabs")]
        [SerializeField]
        private SolidBlockLevelElement solidBlockPrefab;

        public Texture2D Map => map;

        public float QueryRadius => queryRadius;

        public float BlockSize => blockSize;

        public SolidBlockLevelElement SolidBlockPrefab => solidBlockPrefab;
    }
}
