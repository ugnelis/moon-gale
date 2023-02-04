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
            nodes.AddRange(newNodes);
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
