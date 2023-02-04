using System.Collections.Generic;
using System.Linq;
using MoonGale.Runtime.Levels.Nodes;
using NaughtyAttributes;
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

        [SerializeField]
        private Transform nodeParent;

        [SerializeField]
        private int tilesPerTick = 5;

        private int currentTilesCount = 0;

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

        private void ConnectNeighbors(IReadOnlyList<Node> nodes)
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                ConnectNeighbors(nodes, nodes[i]);
            }
        }

        private void ConnectNeighbors(IReadOnlyList<Node> nodes, Node node)
        {
            var currentPosition = node.Position;
            var queryRadius = levelSettings.QueryRadius;

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

#if UNITY_EDITOR
        // ReSharper disable once UnusedMember.Local
        [Button("Propagate")]
        private void PropagateOneStepEditor()
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning("Only works in play-mode");
                return;
            }

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
#endif

        private void PerformBreadthFirstSearch(Node root)
        {
            if (root.NodeObject is not RootNodeObject)
            {
                return;
            }

            var queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0 && currentTilesCount < tilesPerTick)
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
                    if (currentTilesCount >= tilesPerTick)
                    {
                        break;
                    }
                }
            }
        }

        private static IEnumerable<Node> Shuffle(IEnumerable<Node> nodes)
        {
            return nodes.OrderBy(node => Random.value);
        }
    }
}
