using UnityEngine;
using UnityEngine.VFX;

namespace MoonGale.Development._Airidas.Scripts
{
    public class CheckNearbyRoots : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer.Equals(6) || other.gameObject.name.Equals("CoreRootObj"))
            {
                other.gameObject.GetComponentInChildren<VisualEffect>().Play();
                other.gameObject.GetComponent<Animator>().Play("Blob_GettingActive");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer.Equals(6) || other.gameObject.name.Equals("CoreRootObj"))
            {
                other.gameObject.GetComponentInChildren<VisualEffect>().Stop();
                other.gameObject.GetComponent<Animator>().Play("Blob_GettingInactive");
            }
        }
    }
}
