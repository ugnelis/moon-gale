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
        private UnityEvent onDisabled;

        [SerializeField]
        private UnityEvent onDestroyed;

        private bool isQuitting;

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

        private void OnQuitting()
        {
            isQuitting = true;
        }

        private void OnEnable()
        {
            Application.quitting += OnQuitting;
            onEnabled?.Invoke();
        }

        private void OnDisable()
        {
            Application.quitting -= OnQuitting;
            onDisabled?.Invoke();
        }

        private void OnDestroy()
        {
            if (isQuitting)
            {
                return;
            }

            onDestroyed?.Invoke();
        }
    }
}
