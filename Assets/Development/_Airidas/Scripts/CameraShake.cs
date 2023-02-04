using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace MoonGale
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera cinemachineCamera;
        float shakeTimer;
        CinemachineBasicMultiChannelPerlin perlin;

        [SerializeField] float intensity;
        [SerializeField] float time;

        private void Start()
        {
            perlin = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void ShakeCamera()
        {
            perlin.m_AmplitudeGain = intensity;
            shakeTimer = time;
        }

        // Update is called once per frame
        void Update()
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0f)
                {
                    perlin.m_AmplitudeGain = 0.05f;
                }
            }

        }
    }
}
