using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal sealed class NodeGraph : MonoBehaviour
    {
        [SerializeField]
        private List<Node> nodes;

        public IReadOnlyList<Node> Nodes => nodes;

        public void AddNode(Node node)
        {
            nodes.Add(node);
        }

#if UNITY_EDITOR
        [Button("Connect Graph")]
        private void ConnectGraphEditor()
        {
        }
#endif
    }
}
