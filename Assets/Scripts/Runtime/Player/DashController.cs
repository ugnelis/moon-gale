using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MoonGale.Runtime.Player
{
    internal sealed class DashController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private MovementController movementController;

        [SerializeField]
        private PlayerSettings playerSettings;

        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private Transform lookPivot;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onDashStarted;

        [SerializeField]
        private UnityEvent onDashStopped;

        private float nextDashTimeSeconds;
        private Vector3 absoluteMoveDirection;
        private bool hasCollided;

        public event Action<float> OnDashed;

        public bool IsDashing { get; private set; }

        private void OnDisable()
        {
            nextDashTimeSeconds = 0f;
            IsDashing = false;
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        public void Dash()
        {
            if (IsDashing || nextDashTimeSeconds > Time.time)
            {
                return;
            }

            absoluteMoveDirection = movementController.AbsoluteMoveDirection;

            StartCoroutine(DashRoutine());
        }

        public void DashCanceled()
        {
            hasCollided = true;
        }

        private void UpdateMovement()
        {
            if (IsDashing == false || hasCollided)
            {
                return;
            }

            var mainCameraTransform = mainCamera.transform;
            var relativeMotion = GetRelativeMotion(mainCameraTransform, absoluteMoveDirection);

            characterController.Move(relativeMotion);
        }

        private Vector3 GetRelativeMotion(Transform forwardTransform, Vector3 absoluteDirection)
        {
            return GetRelativeMoveDirection(forwardTransform, absoluteDirection) * playerSettings.DashSpeed;
        }

        private static Vector3 GetRelativeMoveDirection(Transform forwardTransform, Vector3 absoluteDirection)
        {
            var direction = forwardTransform.forward;
            var relativeProjectedDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
            var relativeDirection = relativeProjectedDirection.normalized;
            var relativeRotation = Quaternion.LookRotation(relativeDirection);

            return relativeRotation * absoluteDirection;
        }

        private IEnumerator DashRoutine()
        {
            nextDashTimeSeconds = Time.time + playerSettings.DashCooldownSeconds;
            IsDashing = true;

            onDashStarted.Invoke();
            OnDashed?.Invoke(nextDashTimeSeconds);

            yield return new WaitForSeconds(playerSettings.DashDurationSeconds);

            IsDashing = false;
            hasCollided = false;
            onDashStopped.Invoke();
        }
    }
}
