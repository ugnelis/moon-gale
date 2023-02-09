using MoonGale.Core;
using UnityEngine;

namespace MoonGale.Runtime.Player
{
    internal sealed class PlayerHoleController : MonoBehaviour
    {
        [SerializeField]
        private PlayerSettings playerSettings;

        [SerializeField]
        private PlayerActor playerActor;

        private float enteredDurationSeconds;
        private bool isEntered;

        private void OnEnable()
        {
            GameManager.AddListener<PlayerDeathMessage>(OnPlayerDeath);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<PlayerDeathMessage>(OnPlayerDeath);
        }

        private void Update()
        {
            if (isEntered == false)
            {
                return;
            }

            if (playerActor.IsDashing)
            {
                // If player is dashing, give them a second chance.
                return;
            }

            enteredDurationSeconds += Time.deltaTime;

            if (playerSettings.HoleMaxHoverDurationSeconds <= enteredDurationSeconds)
            {
                PlayerActor.Kill();
                enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
             enteredDurationSeconds = 0f;
            isEntered = true;
        }

        private void OnTriggerExit(Collider other)
        {
            enteredDurationSeconds = 0f;
            isEntered = false;
        }

        private void OnPlayerDeath(PlayerDeathMessage message)
        {
            enabled = false;
        }
    }
}
