using System.Collections.Generic;
using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal sealed class Node : MonoBehaviour
    {
        [SerializeField]
        private LevelSettings levelSettings;

        [SerializeField]
        private NodeObject nodeObject;

        [SerializeField]
        private List<Node> neighbors;

        public IReadOnlyList<Node> Neighbors => neighbors;

        public Vector3 Position => transform.position;

        public NodeObject NodeObject
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

        private void Awake()
        {
            if (nodeObject == false)
            {
                return;
            }

            nodeObject.Owner = this;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;

            var position = transform.position;
            foreach (var neighbor in Neighbors)
            {
                Gizmos.DrawLine(position, neighbor.Position);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            var position = transform.position;
            Gizmos.DrawWireCube(position, levelSettings.BlockSize);
            Gizmos.DrawSphere(position, 0.1f);
        }

        public void AddNeighbor(Node node)
        {
            neighbors.Add(node);
        }
    }
}
