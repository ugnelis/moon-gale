using System.Collections.Generic;
using MoonGale.Core;
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

            // Destroy old node.
            Destroy(nodeGameObject);

            // Create new node
            var newNode = Instantiate(
                newNodePrefab,
                nodePosition,
                Quaternion.identity,
                nodeParent
            );

            ConnectNeighbors(graph.Nodes, newNode);
            graph.AddNode(newNode);
        }
    }
}
