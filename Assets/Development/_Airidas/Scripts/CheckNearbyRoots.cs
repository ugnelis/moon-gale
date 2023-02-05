using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace MoonGale
{
    public class CheckNearbyRoots : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer.Equals(6))
            {
                other.gameObject.GetComponentInChildren<VisualEffect>().Play();
                other.gameObject.GetComponent<Animator>().Play("Blob_GettingActive");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer.Equals(6))
            {
                other.gameObject.GetComponentInChildren<VisualEffect>().Stop();
                other.gameObject.GetComponent<Animator>().Play("Blob_GettingInactive");
            }
        }
    }
}
