using System.Collections.Generic;
using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal sealed class NodeGraph : MonoBehaviour
    {
        [SerializeField]
        private List<Node> nodes;

        public IReadOnlyList<Node> Nodes => nodes;

        public void AddNodes(IEnumerable<Node> newNodes)
        {
            foreach (var newNode in newNodes)
            {
                AddNode(newNode);
            }
        }

        public void AddNode(Node newNode)
        {
            newNode.Owner = this;
            nodes.Add(newNode);
        }

        public void RemoveNode(Node node)
        {
            foreach (var otherNode in nodes)
            {
                otherNode.RemoveNeighbor(node);
            }

            nodes.Remove(node);
        }

        public void ClearNodes()
        {
            foreach (var node in nodes)
            {
                node.ClearNeighbors();
            }

            nodes.Clear();
        }
    }
}
