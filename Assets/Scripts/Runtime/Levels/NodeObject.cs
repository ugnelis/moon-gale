using UnityEngine;
using UnityEngine.Events;

namespace MoonGale.Runtime.Levels
{
    internal abstract class NodeObject : MonoBehaviour
    {
        [Header("Graph")]
        [SerializeField]
        private Node owner;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onEnabled;

        [SerializeField]
        private UnityEvent onDestroyed;

        public Node Owner
        {
            get => owner;
            set => owner = value;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            owner = GetComponentInParent<Node>();
        }
#endif

        private void OnEnable()
        {
            onEnabled?.Invoke();
        }

        private void OnDisable()
        {
            onDestroyed?.Invoke();
        }
    }
}
