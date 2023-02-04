using UnityEngine;
using UnityEngine.VFX;

namespace MoonGale.Runtime.Utilities
{
    internal sealed class PrefabSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;

        public void Spawn()
        {


        }

        private void OnDisable()
        {
            GameObject effect = Instantiate(prefab, this.transform.position, Quaternion.identity);
            effect.GetComponent<VisualEffect>().Play();
            Destroy(effect, 6f);
        }
    }
}
