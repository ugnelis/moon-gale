using System.Collections.Generic;
using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal class Node<TNodeObject> : Node, INode<TNodeObject> where TNodeObject : INodeObject
    {
        public TNodeObject NodeObject
        {
            get => nodeObject;
            set
            {
                nodeObject = value;

                if (nodeObject != null)
                {
                    nodeObject.Owner = this;
                }
            }
        }

        private TNodeObject nodeObject;
    }

    internal abstract class Node : MonoBehaviour, INode
    {
        public IReadOnlyList<INode> Neighbors => nodes;

        private readonly List<INode> nodes = new();

        public void AddNeighbor(INode node)
        {
            nodes.Add(node);
        }
    }
}
