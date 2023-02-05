using System.Collections.Generic;
using System.Linq;
using MoonGale.Core;
using MoonGale.Runtime.Levels.Nodes;
using MoonGale.Runtime.Player;
using MoonGale.Runtime.Systems;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace MoonGale.Runtime.Levels
{
    internal sealed class Level : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private LevelSettings levelSettings;

        [SerializeField]
        private NodeGraph graph;

        [SerializeField]
        private Transform nodeParent;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onDestinationReached;

        private IIntensitySystem intensitySystem;
        private int currentTilesCount;

        private int TilesPerTick
        {
            get
            {
                var intensityLevel = intensitySystem.IntensityLevel;
                var tilesPerTick = (int) levelSettings.RootIntensityLevelCurve.Evaluate(intensityLevel);

                return tilesPerTick;
            }
        }

        private float nextSpawnTimeSeconds;
        private bool isPlayerDead;

        private void Awake()
        {
            intensitySystem = GameManager.GetSystem<IIntensitySystem>();
        }

        private void Update()
        {
            if (isPlayerDead)
            {
                return;
            }

            if (Time.time <= nextSpawnTimeSeconds)
            {
                return;
            }

            PropagateOneStep();
            UpdateSpawnTime();
        }

#if UNITY_EDITOR
        // ReSharper disable once UnusedMember.Local
        [Button("Connect Graph")]
        private void ConnectGraphEditor()
        {
            graph.ClearNodes();

            var childCount = nodeParent.childCount;
            var nodes = new List<Node>();

            // Collect candidates
            for (var index = 0; index < childCount; index++)
            {
                var child = nodeParent.GetChild(index);

                if (child.TryGetComponent<Node>(out var node))
                {
                    nodes.Add(node);
                }
            }

            // Connect nodes
            InitializeNodes(nodes);
            ConnectNeighbors(nodes);
            graph.AddNodes(nodes);

            UnityEditor.EditorUtility.SetDirty(graph);
        }
#endif

        private void InitializeNodes(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                node.OwnerLevel = this;
            }
        }

        public void SignalDestinationReached()
        {
            onDestinationReached?.Invoke();
            GameManager.Publish(new PlayerDeathMessage());
            isPlayerDead = true;
        }

        private void ConnectNeighbors(IReadOnlyList<Node> nodes)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < nodes.Count; i++)
            {
                ConnectNeighbors(nodes, nodes[i]);
            }
        }

        private void ConnectNeighbors(IReadOnlyList<Node> nodes, Node node)
        {
            var currentPosition = node.Position;
            var queryRadius = levelSettings.GetQueryRadius(node);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var j = 0; j < nodes.Count; j++)
            {
                var neighbor = nodes[j];
                if (neighbor == node)
                {
                    continue;
                }

                var neighborPosition = neighbor.Position;
                var distance = Vector3.Distance(currentPosition, neighborPosition);

                if (distance <= queryRadius)
                {
                    node.AddNeighbor(neighbor);
                }
            }
        }

        /// <summary>
        /// Replace given <paramref name="node"/> with an appropriate counterpart.
        /// </summary>
        public void ReplaceNode(Node node)
        {
            var nodeObject = node.NodeObject;
            if (nodeObject is RootNodeObject)
            {
                ReplaceNode(node, levelSettings.AirNodePrefab);
                return;
            }

            if (nodeObject is AirNodeObject)
            {
                ReplaceNode(node, levelSettings.RootNodePrefab);
            }
        }

        private void ReplaceNode(Node oldNode, Node newNodePrefab)
        {
            var nodePosition = oldNode.Position;
            var nodeGameObject = oldNode.gameObject;
            var neighbors = oldNode.Neighbors;

            // Destroy old node.
            Destroy(nodeGameObject);

            // Create new node
            var newNode = Instantiate(
                newNodePrefab,
                nodePosition,
                Quaternion.identity,
                nodeParent
            );
            newNode.OwnerLevel = this;
            newNode.SetNeighbors(neighbors);

            graph.ReplaceNode(oldNode, newNode);
        }

        private void UpdateSpawnTime()
        {
            nextSpawnTimeSeconds = Time.time + levelSettings.RootSpawnIntervalSeconds;
        }

        [Button("Propagate")]
        private void PropagateOneStep()
        {
            if (Application.isPlaying == false)
            {
                Debug.LogWarning("Only works in play-mode");
                return;
            }

            Debug.Log(
                $"Spawning Root Nodes. " +
                $"Intensity Level: {intensitySystem.IntensityLevel}, " +
                $"Spawn Count: {TilesPerTick}",
                this
            );

            currentTilesCount = 0;

            var rootNodes = new List<Node>();
            for (var i = 0; i < graph.Nodes.Count(); i++)
            {
                var node = graph.Nodes.ElementAt(i);
                if (node.NodeObject is RootNodeObject)
                {
                    rootNodes.Add(node);
                }
            }

            var shuffledRootNodes = Shuffle(rootNodes).ToList();
            for (var i = 0; i < shuffledRootNodes.Count; i++)
            {
                PerformBreadthFirstSearch(shuffledRootNodes.ElementAt(i));
            }
        }

        private void PerformBreadthFirstSearch(Node root)
        {
            if (root.NodeObject is not RootNodeObject)
            {
                return;
            }

            var queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0 && currentTilesCount < TilesPerTick)
            {
                var node = queue.Dequeue();

                for (var i = 0; i < node.Neighbors.Count(); i++)
                {
                    var neighborNode = node.Neighbors.ElementAt(i);
                    if (neighborNode.NodeObject is not AirNodeObject)
                    {
                        continue;
                    }

                    neighborNode.DestroyNode();
                    queue.Enqueue(neighborNode);

                    currentTilesCount++;
                    if (currentTilesCount >= TilesPerTick)
                    {
                        break;
                    }
                }
            }
        }

        private static IEnumerable<Node> Shuffle(IEnumerable<Node> nodes)
        {
            return nodes.OrderBy(_ => Random.value);
        }
    }
}
