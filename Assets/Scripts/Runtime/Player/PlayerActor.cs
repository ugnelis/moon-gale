using MoonGale.Core;
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

        [SerializeField]
        private DebuffController debuffController;

        [Header("Inputs")]
        [SerializeField]
        private InputActionReference moveInputActionReference;

        [SerializeField]
        private InputActionReference attackInputActionReference;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onPlayerDeath;

        private void OnEnable()
        {
            moveInputActionReference.action.performed += OnMoveInputActionPerformed;
            moveInputActionReference.action.canceled += OnMoveInputActionCanceled;
            attackInputActionReference.action.performed += OnAttackInputActionPerformed;
            debuffController.OnDebuffDurationExceeded += OnDebuffDurationExceeded;
        }

        private void OnDisable()
        {
            moveInputActionReference.action.performed -= OnMoveInputActionPerformed;
            moveInputActionReference.action.canceled -= OnMoveInputActionCanceled;
            attackInputActionReference.action.performed -= OnAttackInputActionPerformed;
            debuffController.OnDebuffDurationExceeded -= OnDebuffDurationExceeded;

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

        private void OnDebuffDurationExceeded()
        {
            movementController.enabled = false;
            attackController.enabled = false;
            debuffController.enabled = false;
            onPlayerDeath.Invoke();
            GameManager.Publish(new PlayerDeathMessage());
        }
    }
}
