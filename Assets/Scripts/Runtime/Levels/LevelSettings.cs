using MoonGale.Runtime.Levels.Nodes;
using MoonGale.Runtime.Utilities;
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
        [Min(0f)]
        [SerializeField]
        private float destinationNodeQueryRadius = 10f;

        [Min(0f)]
        [SerializeField]
        private float destinationNodeBlockSize = 20f;

        [Min(0f)]
        [SerializeField]
        private float queryRadius = 10f;

        [Min(0f)]
        [SerializeField]
        private float blockSize = 5f;

        [SerializeField]
        private LayerMask nodeLayerMask;

        [Header("Prefab")]
        [Min(0f)]
        [SerializeField]
        private Node airNodePrefab;

        [Min(0f)]
        [SerializeField]
        private Node rootNodePrefab;

        public float GetQueryRadius(Node node)
        {
            if (node.NodeObject is DestinationNodeObject)
            {
                return destinationNodeQueryRadius;
            }

            return queryRadius;
        }

        public float GetBlockSize(Node node)
        {
            if (node.NodeObject is DestinationNodeObject)
            {
                return destinationNodeBlockSize;
            }

            return blockSize;
        }

        public LayerMask NodeLayerMask => nodeLayerMask;

        public Node AirNodePrefab => airNodePrefab;

        public Node RootNodePrefab => rootNodePrefab;
    }
}
