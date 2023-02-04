using System.Collections;
using System.Collections.Generic;
using MoonGale.Runtime.Levels.Nodes;
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

        private readonly List<RootNodeObject> attackCandidates = new();
        private float nextAttackTimeSeconds;
        private bool isAttacking;

        private void OnDisable()
        {
            attackCandidates.Clear();
            nextAttackTimeSeconds = 0f;
            isAttacking = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            var sourceRootNodeObject = other.GetComponentInParent<RootNodeObject>();
            if (sourceRootNodeObject == false)
            {
                return;
            }

            if (attackCandidates.Contains(sourceRootNodeObject))
            {
                return;
            }

            attackCandidates.Add(sourceRootNodeObject);
        }

        private void OnTriggerExit(Collider other)
        {
            var sourceRootNodeObject = other.GetComponentInParent<RootNodeObject>();
            if (sourceRootNodeObject == false)
            {
                return;
            }

            attackCandidates.Remove(sourceRootNodeObject);
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
            attackCandidates.Clear();

            yield return new WaitForSeconds(playerSettings.AttackDurationSeconds);

            isAttacking = false;
            onAttackStopped.Invoke();
        }
    }
}
