using System.Collections.Generic;

namespace MoonGale.Runtime.Levels
{
    internal sealed class NodeGraph
    {
        public IReadOnlyList<INode> Nodes => nodes;

        private readonly List<INode> nodes = new();

        public void AddNode(INode node)
        {
            nodes.Add(node);
        }
    }
}
