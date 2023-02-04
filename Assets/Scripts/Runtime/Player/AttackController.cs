using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MoonGale.Runtime.Player
{
    internal sealed class AttackController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private PlayerSettings playerSettings;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onAttackStarted;

        [SerializeField]
        private UnityEvent onAttackStopped;

        private float nextAttackTimeSeconds;
        private bool isAttacking;

        private void OnDisable()
        {
            nextAttackTimeSeconds = 0f;
            isAttacking = false;
        }

        public void Attack()
        {
            if (isAttacking || nextAttackTimeSeconds > Time.time)
            {
                return;
            }

            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            nextAttackTimeSeconds = Time.time + playerSettings.AttackCooldownSeconds;
            isAttacking = true;

            onAttackStarted.Invoke();

            yield return new WaitForSeconds(playerSettings.AttackDurationSeconds);

            isAttacking = false;
            onAttackStopped.Invoke();
        }
    }
}
