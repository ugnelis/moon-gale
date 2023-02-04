using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal sealed class Level : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private LevelSettings levelSettings;

        [SerializeField]
        private NodeGraph graph;
    }
}
