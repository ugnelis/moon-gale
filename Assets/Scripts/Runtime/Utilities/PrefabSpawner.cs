using MoonGale.Runtime.Systems;
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

        [Min(0f)]
        [SerializeField]
        private float destroyDelaySeconds = 6f;

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
            if (isQuitting || SceneSystem.IsSceneLoading)
            {
                return;
            }

            var effect = Instantiate(prefab, transform.position, Quaternion.identity);
            var vfx = effect.GetComponent<VisualEffect>();
            if (vfx)
            {
                vfx.Play();
            }

            Destroy(effect, destroyDelaySeconds);
        }
    }
}
