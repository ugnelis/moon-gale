using System.Collections.Generic;
using MoonGale.Core;
using MoonGale.Runtime.Levels;
using MoonGale.Runtime.Levels.Nodes;
using UnityEngine;

namespace MoonGale.Runtime.Indicators
{
    internal sealed class DangerSensor : MonoBehaviour
    {
        [SerializeField]
        private Transform pivotTransform;

        private readonly List<Node> enteredNodes = new();

        public Transform PivotTransform => pivotTransform;

        private void FixedUpdate()
        {
            var isContainsNodes = enteredNodes.Count > 0;
            for (var index = enteredNodes.Count - 1; index >= 0; index--)
            {
                var enteredNode = enteredNodes[index];
                if (enteredNode == false)
                {
                    enteredNodes.Remove(enteredNode);
                }
            }

            if (isContainsNodes && enteredNodes.Count == 0)
            {
                OnDangerStopped();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var node = other.GetComponentInParent<Node>();
            if (node == false)
            {
                return;
            }

            if (node.NodeObject is RootNodeObject)
            {
                if (enteredNodes.Count == 0)
                {
                    OnDangerStarted();
                }

                enteredNodes.Add(node);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var node = other.GetComponentInParent<Node>();
            if (node == false)
            {
                return;
            }

            enteredNodes.Remove(node);

            if (enteredNodes.Count == 0)
            {
                OnDangerStopped();
            }
        }

        private void OnDangerStarted()
        {
            GameManager.Publish(new IndicatorTriggeredMessage(this));
        }

        private void OnDangerStopped()
        {
            GameManager.Publish(new IndicatorClearedMessage(this));
        }
    }
}
