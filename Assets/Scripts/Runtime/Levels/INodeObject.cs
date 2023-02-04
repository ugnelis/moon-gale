using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal interface INodeObject
    {
        public Vector3 Position { get; }

        public INode Owner { get; set; }
    }
}
