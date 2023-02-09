using System;
using System.Collections.Generic;
using MoonGale.Runtime.Levels;
using UnityEngine;
using UnityEngine.Events;

namespace MoonGale.Runtime.Player
{
    internal sealed class AttackController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField]
        private UnityEvent onAttackStarted;

        public event Action<float> OnAttacked;

        private readonly List<Node> attackCandidates = new();
        private float nextAttackTimeSeconds;

        public float AttackCooldownSeconds { get; set; }

        public bool IsAttacking { get; private set; }

        private void OnDisable()
        {
            attackCandidates.Clear();
            nextAttackTimeSeconds = 0f;
            IsAttacking = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            var sourceRootNodeObject = other.GetComponentInParent<Node>();
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
            var sourceRootNodeObject = other.GetComponentInParent<Node>();
            if (sourceRootNodeObject == false)
            {
                return;
            }

            attackCandidates.Remove(sourceRootNodeObject);
        }

        public void Attack()
        {
            if (IsAttacking || Time.time < nextAttackTimeSeconds)
            {
                return;
            }

            nextAttackTimeSeconds = Time.time + AttackCooldownSeconds;
            IsAttacking = true;

            onAttackStarted.Invoke();
            OnAttacked?.Invoke(nextAttackTimeSeconds);

            foreach (var node in attackCandidates)
            {
                if (node)
                    node.DestroyNode();
            }

            attackCandidates.Clear();
            IsAttacking = false;
        }
    }
}
