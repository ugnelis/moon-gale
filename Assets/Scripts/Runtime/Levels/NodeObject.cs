using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal abstract class NodeObject : MonoBehaviour
    {
        public Node Owner { get; set; }
    }
}
