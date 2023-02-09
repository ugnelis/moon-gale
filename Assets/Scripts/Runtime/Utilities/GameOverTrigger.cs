using System.Collections;
using MoonGale.Core;
using MoonGale.Runtime.Player;
using UnityEngine;
using UnityEngine.Events;

namespace MoonGale.Runtime.Utilities
{
    internal sealed class GameOverTrigger : MonoBehaviour
    {
        [Min(0f)]
        [SerializeField]
        private float delaySeconds;

        [SerializeField]
        private UnityEvent onGameOver;

        private bool isTriggered;

        private void OnEnable()
        {
            GameManager.AddListener<PlayerDeathMessage>(OnGameOver);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<PlayerDeathMessage>(OnGameOver);
        }

        private void OnGameOver(PlayerDeathMessage message)
        {
            if (isTriggered)
            {
                return;
            }

            isTriggered = true;
            StartCoroutine(TriggerRoutine());
        }

        private IEnumerator TriggerRoutine()
        {
            if (delaySeconds > 0f)
            {
                yield return new WaitForSeconds(delaySeconds);
            }

            onGameOver.Invoke();
        }
    }
}
