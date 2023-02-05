using UnityEngine;
using UnityEngine.Events;

namespace MoonGale.Runtime.Levels.Nodes
{
    internal sealed class DestinationNodeObject : NodeObject
    {
        [SerializeField]
        private UnityEvent onVisited;

        private bool isVisited;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            var position = transform.position;
            Gizmos.DrawRay(position, Vector3.up * 10f);
        }

        public void Visit()
        {
            if (isVisited)
            {
                return;
            }

            onVisited?.Invoke();
        }
    }
}
