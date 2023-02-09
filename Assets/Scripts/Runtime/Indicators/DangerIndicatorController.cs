using System.Collections.Generic;
using MoonGale.Core;
using MoonGale.Runtime.Levels.Nodes;
using UnityEngine;
using UnityEngine.Events;

namespace MoonGale.Runtime.Indicators
{
    internal sealed class DangerIndicatorController : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        private DangerIndicator dangerIndicatorPrefab;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onIndicatorTriggerred;

        [SerializeField]
        private UnityEvent onIndicatorCleared;

        private DestinationNodeObject destinationNodeObject;
        private float initialStartWidth;
        private bool isIndicatorShown;

        private readonly IDictionary<DangerSensor, DangerIndicator> indicatorsBySensors =
            new Dictionary<DangerSensor, DangerIndicator>();

        private void OnEnable()
        {
            GameManager.AddListener<IndicatorTriggeredMessage>(OnIndicatorTriggered);
            GameManager.AddListener<IndicatorClearedMessage>(OnIndicatorCleared);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<IndicatorTriggeredMessage>(OnIndicatorTriggered);
            GameManager.RemoveListener<IndicatorClearedMessage>(OnIndicatorCleared);
        }

        private void OnIndicatorTriggered(IndicatorTriggeredMessage message)
        {
            var sensor = message.Sensor;
            if (indicatorsBySensors.ContainsKey(sensor))
            {
                return;
            }

            var indicator = Instantiate(dangerIndicatorPrefab, transform);
            indicator.ShowIndicator(sensor.PivotTransform);
            indicatorsBySensors[sensor] = indicator;
            onIndicatorTriggerred.Invoke();
        }

        private void OnIndicatorCleared(IndicatorClearedMessage message)
        {
            var sensor = message.Sensor;
            if (indicatorsBySensors.TryGetValue(sensor, out var indicator) == false)
            {
                return;
            }

            indicatorsBySensors.Remove(sensor);
            indicator.DestroyIndicator();
            onIndicatorCleared.Invoke();
        }
    }
}
