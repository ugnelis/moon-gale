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
        [Header("Neighbors")]
        [Tooltip("Radius used to find neighbor nodes on destination nodes (override)")]
        [Min(0f)]
        [SerializeField]
        private float destinationNodeQueryRadius = 12f;

        [Tooltip("Radius used to find neighbor nodes on all nodes (default)")]
        [Min(0f)]
        [SerializeField]
        private float queryRadius = 10f;

        [Header("Prefabs")]
        [Tooltip("Prefab used to spawn air tiles")]
        [Min(0f)]
        [SerializeField]
        private Node airNodePrefab;

        [Tooltip("Prefab used to spawn roots")]
        [Min(0f)]
        [SerializeField]
        private Node rootNodePrefab;

        [Header("Root Spawn Rates")]
        [Tooltip("How often to spawn roots")]
        [Min(0f)]
        [SerializeField]
        private float rootSpawnIntervalSeconds = 1f;

        [Tooltip(
            "Curve which determines the amount of roots spawned each tick. The x axis denotes " +
            "intensity level, the y axis denotes the amount of roots spawned at that level"
        )]
        [Min(0f)]
        [SerializeField]
        private AnimationCurve rootIntensityLevelCurve;

        [Header("Debug")]
        [Tooltip("Size of the destination node debug visuals")]
        [Min(0f)]
        [SerializeField]
        private float destinationNodeBlockSize = 20f;

        [Tooltip("Size of grid tile (mostly used for debug visuals)")]
        [Min(0f)]
        [SerializeField]
        private float blockSize = 5f;

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

        public Node AirNodePrefab => airNodePrefab;

        public Node RootNodePrefab => rootNodePrefab;

        public float RootSpawnIntervalSeconds => rootSpawnIntervalSeconds;

        public AnimationCurve RootIntensityLevelCurve => rootIntensityLevelCurve;
    }
}
