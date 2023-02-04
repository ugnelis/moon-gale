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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying == false)
            {
                DrawNodeRadius();
            }

            DrawNodeConnections(Color.red);
        }

        private void OnDrawGizmos()
        {
            DrawNodeSize();
            var color = Color.white;
            color.a = 0.5f;
            DrawNodeConnections(color);
        }

        private void DrawNodeRadius()
        {
            var position = transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, levelSettings.QueryRadius);

            var color = Color.yellow;
            color.a = 0.1f;
            Gizmos.color = color;
            Gizmos.DrawSphere(position, levelSettings.QueryRadius);
        }

        private void DrawNodeSize()
        {
            var position = transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(position, levelSettings.BlockSize * Vector3.one);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(position, 0.25f);
        }

        private void DrawNodeConnections(Color color)
        {
            Gizmos.color = color;
            var position = transform.position;
            foreach (var neighbor in Neighbors)
            {
                Gizmos.DrawLine(position, neighbor.Position);
            }
        }
#endif

        public void AddNeighbor(Node node)
        {
            neighbors.Add(node);
        }

        public void ClearNeighbors()
        {
            neighbors.Clear();
        }
    }
}
