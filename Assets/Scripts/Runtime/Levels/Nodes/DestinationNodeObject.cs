using System;
using UnityEngine;

namespace MoonGale.Runtime.Levels.Nodes
{
    internal sealed class DestinationNodeObject : NodeObject
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            var position = transform.position;
            Gizmos.DrawRay(position, Vector3.up * 10f);
        }
    }
}
