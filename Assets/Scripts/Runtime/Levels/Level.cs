using System.Collections.Generic;
using MoonGale.Core;
using MoonGale.Runtime.Levels.Nodes;
using MoonGale.Runtime.Player;
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

        private void OnEnable()
        {
            GameManager.AddListener<NodeAttackedMessage>(OnNodeAttackedMessage);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<NodeAttackedMessage>(OnNodeAttackedMessage);
        }

        private void OnNodeAttackedMessage(NodeAttackedMessage message)
        {
            var node = message.Node;
            ReplaceNode(node, levelSettings.AirNodePrefab);
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
            ConnectNeighbors(nodes);
            graph.AddNodes(nodes);

            UnityEditor.EditorUtility.SetDirty(graph);
        }
#endif

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
            newNode.SetNeighbors(neighbors);

            graph.ReplaceNode(oldNode, newNode);
        }

#if UNITY_EDITOR
        // ReSharper disable once UnusedMember.Local
        [Button("Propagate 1 step")]
        private void PropagateOneStepEditor()
        {
            // var childCount = nodeParent.childCount;
            // var rootNodes = new List<Node>();

            var rootNodes = new List<Node>();

            foreach (var node in graph.Nodes)
            {
                if (node.NodeObject is RootNodeObject)
                {
                    rootNodes.Add(node);
                }
            }

            foreach (var node in rootNodes)
            {
                PerformBreadthFirstSearch(node);
            }
        }
#endif

        private void PerformBreadthFirstSearch(Node root, int steps = 1)
        {
            if (root.NodeObject is not RootNodeObject)
            {
                return;
            }

            var queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                foreach (var neighborNode in node.Neighbors)
                {
                    if (neighborNode.NodeObject is not AirNodeObject) continue;

                    ReplaceNode(neighborNode, levelSettings.RootNodePrefab);
                    queue.Enqueue(neighborNode);
                }
            }
        }
    }
}
