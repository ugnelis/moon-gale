using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace MoonGale.Runtime.UI
{
    internal sealed class FadeCanvasUIController : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField]
        private CanvasGroup canvasGroup;

        [Min(0f)]
        [SerializeField]
        private float fadeDurationSeconds = 1f;

        private void Start()
        {
            StartCoroutine(HideCanvas());
        }

        public IEnumerator ShowCanvas()
        {
            return FadeRoutine(1f, () =>
            {
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            });
        }

        public IEnumerator HideCanvas()
        {
            return FadeRoutine(0f, () =>
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            });
        }

        private IEnumerator FadeRoutine(float to, Action onComplete)
        {
            yield return canvasGroup
                .DOFade(to, fadeDurationSeconds)
                .OnComplete(onComplete.Invoke)
                .SetUpdate(true)
                .WaitForCompletion();
        }
    }
}
