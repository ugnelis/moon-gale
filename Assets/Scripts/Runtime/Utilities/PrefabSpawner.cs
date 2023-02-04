using UnityEngine;

namespace MoonGale.Runtime.Utilities
{
    internal sealed class PrefabSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;

        public void Spawn()
        {
            Instantiate(prefab);
        }
    }
}
