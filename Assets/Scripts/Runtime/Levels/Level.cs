using System.Collections.Generic;
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
            var queryRadius = levelSettings.QueryRadius;
            for (var i = 0; i < nodes.Count; i++)
            {
                var currentNode = nodes[i];
                var currentPosition = currentNode.Position;

                for (var j = 0; j < nodes.Count; j++)
                {
                    var neighbor = nodes[j];
                    if (neighbor == currentNode)
                    {
                        continue;
                    }

                    var neighborPosition = neighbor.Position;
                    var distance = Vector3.Distance(currentPosition, neighborPosition);

                    if (distance <= queryRadius)
                    {
                        currentNode.AddNeighbor(neighbor);
                    }
                }
            }

            graph.AddNodes(nodes);

            UnityEditor.EditorUtility.SetDirty(graph);
        }
#endif
    }
}
