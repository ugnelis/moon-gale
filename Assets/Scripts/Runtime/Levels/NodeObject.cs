using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal abstract class NodeObject : MonoBehaviour
    {
        [SerializeField]
        private Node owner;

        public Node Owner
        {
            get => owner;
            set => owner = value;
        }
    }
}
