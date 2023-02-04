using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal sealed class LevelActor : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private LevelSettings levelSettings;

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
