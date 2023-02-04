using UnityEngine;
using UnityEngine.VFX;

namespace MoonGale.Runtime.Utilities
{
    internal sealed class PrefabSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private bool isSpawnOnDisable = true;

        private bool isQuitting;

        private void OnEnable()
        {
            Application.quitting += OnQuitting;
        }

        private void OnDisable()
        {
            Application.quitting -= OnQuitting;

            if (isSpawnOnDisable)
            {
                Spawn();
            }
        }

        private void OnQuitting()
        {
            isQuitting = true;
        }

        private void Spawn()
        {
            if (isQuitting)
            {
                return;
            }

            GameObject effect = Instantiate(prefab, this.transform.position, Quaternion.identity);
            effect.GetComponent<VisualEffect>().Play();
            Destroy(effect, 6f);
        }
    }
}
