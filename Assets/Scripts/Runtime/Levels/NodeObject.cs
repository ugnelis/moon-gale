using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal abstract class NodeObject : MonoBehaviour, INodeObject
    {
        public Vector3 Position => transform.position;

        public INode Owner { get; set; }
    }
}
