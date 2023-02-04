using System.Collections.Generic;

namespace MoonGale.Runtime.Levels
{
    internal interface INode<TNodeObject> where TNodeObject : INodeObject
    {
        public TNodeObject NodeObject { get; set; }
    }

    internal interface INode
    {
        public IReadOnlyList<INode> Neighbors { get; }

        public void AddNeighbor(INode node);
    }
}
