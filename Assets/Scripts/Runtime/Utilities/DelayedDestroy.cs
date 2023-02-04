using UnityEngine;

namespace MoonGale.Runtime.Utilities
{
    internal sealed class DelayedDestroy : MonoBehaviour
    {
        [Min(0f)]
        [SerializeField]
        private float delaySeconds = 6f;

        private void Start()
        {
            Destroy(gameObject, delaySeconds);
        }
    }
}
