using MoonGale.Core;
using MoonGale.Runtime.Levels;

namespace MoonGale.Runtime.Player
{
    internal sealed class NodeAttackedMessage : IMessage
    {
        public Node Node { get; }

        public NodeAttackedMessage(Node node)
        {
            Node = node;
        }
    }
}
