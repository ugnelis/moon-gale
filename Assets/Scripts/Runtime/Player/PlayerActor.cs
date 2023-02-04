using UnityEngine;
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

            movementController.StopMovement();
        }

        private void OnMoveInputActionPerformed(InputAction.CallbackContext context)
        {
            var axis = context.ReadValue<Vector2>();
            var absoluteMoveDirection = new Vector3(axis.x, 0f, axis.y);
            movementController.StartMovement(absoluteMoveDirection);
        }

        private void OnMoveInputActionCanceled(InputAction.CallbackContext context)
        {
            movementController.StopMovement();
        }

        private void OnAttackInputActionPerformed(InputAction.CallbackContext context)
        {
            attackController.Attack();
        }
    }
}
