using UnityEngine;
using UnityEngine.Events;

namespace MoonGale.Runtime.Player
{
    internal sealed class MovementController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private PlayerSettings playerSettings;

        [SerializeField]
        private Transform lookPivot;

        [SerializeField]
        private Camera mainCamera;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onMovementStarted;

        [SerializeField]
        private UnityEvent onMovementStopped;

        private Vector3 targetPivotLookDirection;
        private Vector3 absoluteMoveDirection;
        private bool isMoveInputActive;

        [NaughtyAttributes.ShowNonSerializedField]
        private float currentMoveSpeed;

        public float MovementSpeedMultiplier { get; set; }

        private void OnDrawGizmos()
        {
            if (mainCamera == false)
            {
                return;
            }

            Gizmos.color = Color.red;

            var mainCameraTransform = mainCamera.transform;
            var playerPosition = transform.position;
            var relativeMotion = GetRelativeMotion(mainCameraTransform, absoluteMoveDirection);
            Gizmos.DrawRay(playerPosition, relativeMotion);
        }

        private void Awake()
        {
            if (mainCamera == false)
            {
                Debug.LogError($"{nameof(mainCamera)} is not set", this);
                enabled = false;
            }
        }

        private void OnDisable()
        {
            currentMoveSpeed = 0f;
        }

        private void FixedUpdate()
        {
            UpdateCurrentSpeed();
            UpdateMovement();
            UpdateLookDirection();
        }

        public void StartMovement(Vector3 newAbsoluteMoveDirection)
        {
            absoluteMoveDirection = newAbsoluteMoveDirection;
            isMoveInputActive = true;
            onMovementStarted?.Invoke();
        }

        public void StopMovement()
        {
            absoluteMoveDirection = Vector3.zero;
            isMoveInputActive = false;
            onMovementStopped?.Invoke();
        }

        private void UpdateCurrentSpeed()
        {
            var newMoveSpeed = isMoveInputActive
                ? currentMoveSpeed + playerSettings.MoveAcceleration * Time.deltaTime
                : currentMoveSpeed - playerSettings.StopAcceleration * Time.deltaTime;

            newMoveSpeed *= MovementSpeedMultiplier;

            var clampedMoveSpeed = Mathf.Clamp(
                newMoveSpeed,
                min: 0f,
                max: playerSettings.MaxMoveSpeed
            );

            currentMoveSpeed = clampedMoveSpeed;
        }

        private void UpdateMovement()
        {
            if (currentMoveSpeed <= 0)
            {
                return;
            }

            var mainCameraTransform = mainCamera.transform;
            var relativeMotion = GetRelativeMotion(mainCameraTransform, absoluteMoveDirection);

            characterController.Move(relativeMotion);
        }

        private void UpdateLookDirection()
        {
            if (absoluteMoveDirection == Vector3.zero)
            {
                return;
            }

            var mainCameraTransform = mainCamera.transform;
            targetPivotLookDirection = GetRelativeMoveDirection(mainCameraTransform, absoluteMoveDirection);

            var currentPivotLookDirection = lookPivot.forward;
            lookPivot.forward = Vector3.Lerp(
                currentPivotLookDirection,
                targetPivotLookDirection,
                playerSettings.PivotLookSpeed * Time.deltaTime
            );
        }

        private Vector3 GetRelativeMotion(Transform forwardTransform, Vector3 absoluteDirection)
        {
            return GetRelativeMoveDirection(forwardTransform, absoluteDirection) * currentMoveSpeed;
        }

        private static Vector3 GetRelativeMoveDirection(Transform forwardTransform, Vector3 absoluteDirection)
        {
            var direction = forwardTransform.forward;
            var relativeProjectedDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
            var relativeDirection = relativeProjectedDirection.normalized;
            var relativeRotation = Quaternion.LookRotation(relativeDirection);

            return relativeRotation * absoluteDirection;
        }
    }
}
