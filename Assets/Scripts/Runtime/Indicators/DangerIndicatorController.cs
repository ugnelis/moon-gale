using System.Collections.Generic;
using System.Linq;
using MoonGale.Core;
using MoonGale.Runtime.Levels.Nodes;
using MoonGale.Runtime.Player;
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
            GameManager.AddListener<PlayerDeathMessage>(OnGameOver);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<IndicatorTriggeredMessage>(OnIndicatorTriggered);
            GameManager.RemoveListener<IndicatorClearedMessage>(OnIndicatorCleared);
            GameManager.RemoveListener<PlayerDeathMessage>(OnGameOver);
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

            DestroyIndicator(sensor, indicator);
        }

        private void OnGameOver(PlayerDeathMessage message)
        {
            var entries = indicatorsBySensors.ToList();
            foreach (var (sensor, indicator) in entries)
            {
                DestroyIndicator(sensor, indicator);
            }
        }

        private void DestroyIndicator(DangerSensor sensor, DangerIndicator indicator)
        {
            indicatorsBySensors.Remove(sensor);
            indicator.DestroyIndicator();
            onIndicatorCleared.Invoke();
        }
    }
}
