using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    [ExecuteInEditMode] // Need to execute OnDestroy.
    internal sealed class Node : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private LevelSettings levelSettings;

        [Header("Graph")]
        [SerializeField]
        private NodeObject nodeObject;

        [SerializeField]
        private Level ownerLevel;

        [SerializeField]
        private NodeGraph owner;

        [SerializeField]
        private List<Node> neighbors;

        [Header("Debug")]
        [SerializeField]
        private Color nodeSizeColor = Color.green;

        public IEnumerable<Node> Neighbors => neighbors.Where(neighbor => neighbor);

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

        public Level OwnerLevel
        {
            get => ownerLevel;
            set => ownerLevel = value;
        }

        public NodeGraph Owner
        {
            get => owner;
            set => owner = value;
        }

        private bool isQuitting;

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

        private void OnEnable()
        {
            Application.quitting += OnQuitting;
        }

        private void OnDisable()
        {
            Application.quitting -= OnQuitting;
        }

        private void OnQuitting()
        {
            isQuitting = true;
        }

        private void DrawNodeRadius()
        {
            var position = transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, levelSettings.GetQueryRadius(this));

            var color = Color.yellow;
            color.a = 0.1f;
            Gizmos.color = color;
            Gizmos.DrawSphere(position, levelSettings.GetQueryRadius(this));
        }

        private void DrawNodeSize()
        {
            var position = transform.position;

            Gizmos.color = nodeSizeColor;
            Gizmos.DrawWireCube(position, levelSettings.GetBlockSize(this) * Vector3.one);

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

        private void OnDestroy()
        {
            if (isQuitting)
            {
                return;
            }

            if (ownerLevel && Application.isPlaying)
            {
                ownerLevel.ReplaceNode(this);
            }

            if (owner)
            {
                owner.RemoveNode(this);
            }
        }

        public void AddNeighbor(Node node)
        {
            if (neighbors.Contains(node))
            {
                return;
            }

            neighbors.Add(node);
        }

        public void RemoveNeighbor(Node node)
        {
            neighbors.Remove(node);
        }

        public void ReplaceNeighbor(Node oldNode, Node newNode)
        {
            var index = neighbors.IndexOf(oldNode);

            if (index == -1)
            {
                return;
            }

            neighbors[index] = newNode;
        }

        public void SetNeighbors(IEnumerable<Node> newNeighbors)
        {
            neighbors.AddRange(newNeighbors);
        }

        public void ClearNeighbors()
        {
            neighbors.Clear();
        }

        public void DestroyNode()
        {
            Destroy(gameObject);
        }
    }
}
