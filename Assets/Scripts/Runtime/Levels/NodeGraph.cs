using System.Collections.Generic;

namespace MoonGale.Runtime.Levels
{
    internal sealed class NodeGraph
    {
        public IReadOnlyList<Node> Nodes => nodes;

        private readonly List<Node> nodes = new();

        public void AddNode(Node node)
        {
            nodes.Add(node);
        }
    }
}
