using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonGale
{
    public class AnimationUpDown : MonoBehaviour
    {

        [SerializeField] [Range(0, 1)] float speed = 0.4f;
        [SerializeField] [Range(0, 100)] float range = 0.8f;
        Vector3 pos;

        private void Awake()
        {
            pos = transform.position;
        }
        void Update()
        {
            loop();
        }

        void loop()
        {
            float yPos = Mathf.PingPong(Time.time * speed, 1) * range;
            transform.position = new Vector3(pos.x, pos.y + yPos, pos.z);
        }
    }
}

