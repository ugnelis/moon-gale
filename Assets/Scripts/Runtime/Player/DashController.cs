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

        [Header("Events")]
        [SerializeField]
        private UnityEvent onDashStarted;

        [SerializeField]
        private UnityEvent onDashStopped;

        private float nextDashTimeSeconds;
        private bool isDashing;
        private Vector3 absoluteMoveDirection;


        private void OnDisable()
        {
            nextDashTimeSeconds = 0f;
            isDashing = false;
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        public void Dash()
        {
            if (isDashing || nextDashTimeSeconds > Time.time)
            {
                return;
            }

            absoluteMoveDirection = movementController.AbsoluteMoveDirection;

            StartCoroutine(DashRoutine());
        }

        private void UpdateMovement()
        {
            if (isDashing == false)
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
            isDashing = true;

            onDashStarted.Invoke();

            yield return new WaitForSeconds(playerSettings.DashDurationSeconds);

            isDashing = false;
            onDashStopped.Invoke();
        }
    }
}
