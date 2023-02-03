using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal sealed class LevelArea : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private Texture2D map;

        [Header("Block Sizes")]
        [Min(1)]
        [SerializeField]
        private int blockWidth = 1;

        [Min(1)]
        [SerializeField]
        private int blockHeight = 1;

        [Header("Prefabs")]
        [SerializeField]
        private LevelElement solidBlockPrefab;

        [SerializeField]
        private LevelElement airBlockPrefab;

        private void Start()
        {
            // var mapWidth = map.width;
            // var mapHeight = map.height;
            //
            // var pixels = map.GetPixels();
            // var cols = map.width;
            // var rows = map.height;
            // var row = 0;
            // for (var colIndex = 0; colIndex < cols; colIndex++)
            // {
            //     var pixel = pixels[row + colIndex];
            // }
        }
    }
}
