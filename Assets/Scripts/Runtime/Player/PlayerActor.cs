using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MoonGale.Runtime.Player
{
    internal sealed class PlayerActor : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private MovementController movementController;

        [SerializeField]
        private AttackController attackController;

        [Header("Inputs")]
        [SerializeField]
        private InputActionReference moveInputActionReference;

        [SerializeField]
        private InputActionReference attackInputActionReference;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onMovementInputStarted;

        [SerializeField]
        private UnityEvent onMovementInputStopped;

        [SerializeField]
        private UnityEvent onAttackInputStarted;

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

            movementController.AbsoluteMoveDirection = Vector3.zero;
            movementController.CurrentMoveSpeed = 0f;
        }

        private void OnMoveInputActionPerformed(InputAction.CallbackContext context)
        {
            var axis = context.ReadValue<Vector2>();
            movementController.AbsoluteMoveDirection = new Vector3(axis.x, 0f, axis.y);
            movementController.IsMoveInputActive = true;
            onMovementInputStarted?.Invoke();
        }

        private void OnMoveInputActionCanceled(InputAction.CallbackContext context)
        {
            movementController.IsMoveInputActive = false;
            onMovementInputStopped.Invoke();
        }

        private void OnAttackInputActionPerformed(InputAction.CallbackContext context)
        {
            attackController.Attack();
            onAttackInputStarted?.Invoke();
        }
    }
}
