using UnityEngine;
using UnityEngine.Events;

namespace MoonGale.Runtime.Utilities
{
    internal sealed class LifecylceTrigger : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onStarted;

        [Min(0f)]
        [SerializeField]
        private float delaySeconds = 0.5f;

        private void Start()
        {
            Invoke(nameof(InvokeOnStarted), delaySeconds);
        }

        private void InvokeOnStarted()
        {
            onStarted.Invoke();
        }
    }
}
