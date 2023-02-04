using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonGale.Runtime.Player
{
    internal sealed class PlayerActor : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private Camera mainCamera;

        [Header("Movement")]
        [Min(0f)]
        [SerializeField]
        private float maxMoveSpeed = 0.1f;

        [Min(0f)]
        [SerializeField]
        private float moveAcceleration = 2f;

        [Min(0f)]
        [SerializeField]
        private float stopAcceleration = 1f;

        [Header("Inputs")]
        [SerializeField]
        private InputActionReference moveInputActionReference;

        [SerializeField]
        private InputActionReference attackInputActionReference;

        private bool isMoveInputActive;
        private Vector3 absoluteMoveDirection;

        [NaughtyAttributes.ShowNonSerializedField]
        private float currentMoveSpeed;

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

        private void OnEnable()
        {
            moveInputActionReference.action.performed += OnMoveInputActionPerformed;
            moveInputActionReference.action.canceled += OnMoveInputActionCanceled;
            attackInputActionReference.action.performed += OnAttackInputActionPerformed;
        }

        private void OnDisable()
        {
            moveInputActionReference.action.performed -= OnMoveInputActionPerformed;
            moveInputActionReference.action.canceled -= OnMoveInputActionCanceled;
            attackInputActionReference.action.performed -= OnAttackInputActionPerformed;

            absoluteMoveDirection = Vector3.zero;
            currentMoveSpeed = 0f;
        }

        private void FixedUpdate()
        {
            UpdateCurrentSpeed();
            UpdateMovement();
        }

        private void UpdateCurrentSpeed()
        {
            var newMoveSpeed = isMoveInputActive
                ? currentMoveSpeed + moveAcceleration * Time.deltaTime
                : currentMoveSpeed - stopAcceleration * Time.deltaTime;

            var clampedMoveSpeed = Mathf.Clamp(newMoveSpeed, min: 0f, max: maxMoveSpeed);

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

        private Vector3 GetRelativeMotion(Transform forwardTransform, Vector3 absoluteDirection)
        {
            var direction = forwardTransform.forward;
            var relativeProjectedDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
            var relativeDirection = relativeProjectedDirection.normalized;
            var relativeRotation = Quaternion.LookRotation(relativeDirection);
            var relativeMotion = relativeRotation * absoluteDirection * currentMoveSpeed;

            return relativeMotion;
        }

        private void OnMoveInputActionPerformed(InputAction.CallbackContext context)
        {
            var axis = context.ReadValue<Vector2>();
            absoluteMoveDirection.x = axis.x;
            absoluteMoveDirection.z = axis.y;
            isMoveInputActive = true;
        }

        private void OnMoveInputActionCanceled(InputAction.CallbackContext context)
        {
            isMoveInputActive = false;
        }

        private void OnAttackInputActionPerformed(InputAction.CallbackContext context)
        {
        }
    }
}
