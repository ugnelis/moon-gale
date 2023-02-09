using System;
using DG.Tweening;
using UnityEngine;

namespace MoonGale.Runtime.Indicators
{
    internal sealed class DangerIndicator : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private LineRenderer lineRenderer;

        [Header("Rendering")]
        [Min(0f)]
        [SerializeField]
        private float indicatorLength = 7f;

        [Min(0f)]
        [SerializeField]
        private float animationDurationSeconds = 0.5f;

        private Transform endTransform;

        private float initialStartWidth;
        private bool isShown;

        private void Awake()
        {
            initialStartWidth = lineRenderer.startWidth;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
        }

        private void Update()
        {
            if (isShown == false)
            {
                return;
            }

            var startPosition = transform.position;
            var endPosition = endTransform.position;

            lineRenderer.SetPosition(0, startPosition);

            var destinationPoint = Vector3.MoveTowards(startPosition, endPosition, indicatorLength);
            lineRenderer.SetPosition(1, destinationPoint);
        }

        public void ShowIndicator(Transform newEndTransform)
        {
            endTransform = newEndTransform;

            lineRenderer.startWidth = 0f;
            lineRenderer.enabled = true;
            isShown = true;

            AnimateWidth(initialStartWidth, () => { });
        }

        public void DestroyIndicator()
        {
            AnimateWidth(0f, () => Destroy(gameObject));
        }

        private void AnimateWidth(float resultWidth, Action action)
        {
            var startWidth = lineRenderer.startWidth;
            DOVirtual
                .Float(startWidth, resultWidth, animationDurationSeconds, value => { lineRenderer.startWidth = value; })
                .OnComplete(action.Invoke);
        }
    }
}
