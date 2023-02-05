using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoonGale.Runtime.Player
{
    internal sealed class DebuffController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private PlayerSettings playerSettings;

        [SerializeField]
        private Volume volume;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onStartDebuff;

        public Action OnDebuffDurationExceeded;

        public float DebuffMoveSpeedMultiplier
        {
            get
            {
                var maxDuration = playerSettings.MaxDebuffDurationSeconds;
                var duration = totalDebuffDuration;
                var ratio = duration / maxDuration;

                return Mathf.Lerp(1f, playerSettings.MinDebuffMoveSpeedMultiplier, ratio);
            }
        }

        private float DebuffVignetteIntensity
        {
            get
            {
                var maxDuration = playerSettings.MaxDebuffDurationSeconds;
                var duration = totalDebuffDuration;
                var ratio = duration / maxDuration;

                return Mathf.Lerp(initialVignetteIntensity, playerSettings.MaxDebuffVignette, ratio);
            }
        }

        private float DebuffPostExposure
        {
            get
            {
                var maxDuration = playerSettings.MaxDebuffDurationSeconds;
                var duration = totalDebuffDuration;
                var ratio = duration / maxDuration;

                return Mathf.Lerp(initialPostExposure, playerSettings.MinDebuffPostExposure, ratio);
            }
        }

        private float initialVignetteIntensity;
        private float initialPostExposure;

        private ColorAdjustments colorAdjustments;
        private Vignette vignette;

        private float totalDebuffDuration;
        private bool isOverlappedLastFrame;
        private bool isOverlappingNode;

        private void Awake()
        {
            if (volume == false)
            {
                volume = FindObjectOfType<Volume>();
            }

            if (volume)
            {
                volume.profile.TryGet(out colorAdjustments);
                volume.profile.TryGet(out vignette);
            }
        }

        private void Update()
        {
            UpdateDebuffDuration();
            UpdateEffects();
        }

        private void FixedUpdate()
        {
            isOverlappedLastFrame = isOverlappingNode;
            isOverlappingNode = false;
        }

        private void OnDisable()
        {
            isOverlappedLastFrame = false;
            isOverlappingNode = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (isOverlappedLastFrame == false)
            {
                onStartDebuff?.Invoke();
            }

            // This whole logic
            isOverlappingNode = true;
        }


        private void UpdateDebuffDuration()
        {
            if (isOverlappedLastFrame)
            {
                totalDebuffDuration += Time.deltaTime;
            }
            else
            {
                totalDebuffDuration = Mathf.Max(0f, totalDebuffDuration - Time.deltaTime);
            }

            if (playerSettings.MaxDebuffDurationSeconds <= totalDebuffDuration)
            {
                OnDebuffDurationExceeded?.Invoke();
            }
        }

        private void UpdateEffects()
        {
            if (colorAdjustments)
            {
                colorAdjustments.postExposure.value = DebuffPostExposure;
            }

            if (vignette)
            {
                vignette.intensity.value = DebuffVignetteIntensity;
            }
        }
    }
}
