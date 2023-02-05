using System;
using DG.Tweening;
using MoonGale.Core;
using MoonGale.Runtime.Levels.Nodes;
using UnityEngine;
using UnityEngine.Events;

namespace MoonGale.Runtime.Levels
{
    internal sealed class DangerIndicator : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private Transform startPointTransform;

        [Header("Indicator")]
        [Min(0f)]
        [SerializeField]
        private float indicatorLength = 7f;

        [Min(0f)]
        [SerializeField]
        private float indicatorYOffset = 5f;

        [Min(0f)]
        [SerializeField]
        private float animationDurationSeconds = 0.5f;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onIndicatorHidden;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onIndicatorShown;

        private DestinationNodeObject destinationNodeObject;
        private float initialStartWidth;
        private bool isIndicatorShown;

        private void Awake()
        {
            destinationNodeObject = FindObjectOfType<DestinationNodeObject>();
            initialStartWidth = lineRenderer.startWidth;

            if (destinationNodeObject == false)
            {
                enabled = false;
                return;
            }

            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
        }

        private void OnEnable()
        {
            GameManager.AddListener<DangerStartedMessage>(OnDangerStarted);
            GameManager.AddListener<DangerStoppedMessage>(OnDangerStopped);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<DangerStartedMessage>(OnDangerStarted);
            GameManager.RemoveListener<DangerStoppedMessage>(OnDangerStopped);
        }

        private void Update()
        {
            if (isIndicatorShown == false)
            {
                return;
            }

            var startPosition = startPointTransform.position;
            var endPosition = destinationNodeObject.transform.position;

            lineRenderer.SetPosition(0, startPosition);

            var destinationPoint = Vector3.MoveTowards(startPosition, endPosition, indicatorLength);
            destinationPoint.y += indicatorYOffset;

            lineRenderer.SetPosition(1, destinationPoint);
        }

        private void OnDangerStarted(DangerStartedMessage message)
        {
            ShowIndicator();
        }

        private void OnDangerStopped(DangerStoppedMessage message)
        {
            HideIndicator();
        }

        private void ShowIndicator()
        {
            if (isIndicatorShown)
            {
                return;
            }

            lineRenderer.enabled = true;
            AnimateWidth(initialStartWidth, () => { });

            isIndicatorShown = true;
            onIndicatorShown?.Invoke();
        }

        private void HideIndicator()
        {
            if (isIndicatorShown == false)
            {
                return;
            }

            AnimateWidth(0f, () => { lineRenderer.enabled = false; });

            isIndicatorShown = false;
            onIndicatorHidden?.Invoke();
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
