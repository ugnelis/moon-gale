using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MoonGale.Sample
{
    internal sealed class SamplePlayerController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private Rigidbody rigidBody;

        [SerializeField]
        private Camera mainCamera;

        [Header("Movement")]
        [Min(0f)]
        [SerializeField]
        private float moveForce = 5f;

        [Min(0f)]
        [SerializeField]
        private float jumpForce = 200f;

        [Header("Inputs")]
        [SerializeField]
        private InputActionReference moveInputAction;

        [SerializeField]
        private InputActionReference jumpInputAction;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onJumped;

        private Vector3 absoluteMoveDirection;

        private void OnEnable()
        {
            moveInputAction.action.performed += OnMovePerformed;
            moveInputAction.action.canceled += OnMoveCanceled;
            jumpInputAction.action.performed += OnJumpPerformed;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            moveInputAction.action.performed -= OnMovePerformed;
            moveInputAction.action.canceled -= OnMoveCanceled;
            jumpInputAction.action.performed -= OnJumpPerformed;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void FixedUpdate()
        {
            if (absoluteMoveDirection == Vector3.zero)
            {
                return;
            }

            var cameraDirection = mainCamera.transform.forward;
            var projectedCameraDirection = Vector3.ProjectOnPlane(cameraDirection, Vector3.up);
            var normalizedCameraDirection = projectedCameraDirection.normalized;
            var cameraRotation = Quaternion.LookRotation(normalizedCameraDirection);
            var relativeMoveDirection = cameraRotation * absoluteMoveDirection;
            var relativeForce = relativeMoveDirection * moveForce;

            rigidBody.AddForce(relativeForce);
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            var moveAxis = context.ReadValue<Vector2>();
            absoluteMoveDirection.x = moveAxis.x;
            absoluteMoveDirection.z = moveAxis.y;
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            absoluteMoveDirection = Vector3.zero;
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            rigidBody.AddForce(Vector3.up * jumpForce);
            onJumped.Invoke();
        }
    }
}
