using MoonGale.Core;
using MoonGale.Runtime.Systems;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MoonGale.Runtime.Player
{
    internal sealed class PlayerActor : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] PlayerSettings settings;

        [SerializeField]
        private MovementController movementController;

        [SerializeField]
        private DashController dashController;

        [SerializeField]
        private AttackController attackController;

        [SerializeField]
        private AttackController strongAttackController;

        [SerializeField]
        private DebuffController debuffController;

        [Header("Inputs")]
        [SerializeField]
        private InputActionReference moveInputActionReference;

        [SerializeField]
        private InputActionReference dashInputActionReference;

        [SerializeField]
        private InputActionReference attackInputActionReference;

        [SerializeField]
        private InputActionReference strongAttackInputActionReference;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onPlayerDeath;

        private IScoreSystem scoreSystem;

        private void Awake()
        {
            scoreSystem = GameManager.GetSystem<IScoreSystem>();
            strongAttackController.AttackDurationSeconds = settings.StrongAttackDurationSeconds;
            attackController.AttackDurationSeconds = settings.AttackDurationSeconds;
        }

        private void Start()
        {
            scoreSystem.StartTimer();
        }

        private void Update()
        {
            movementController.MovementSpeedMultiplier = debuffController.DebuffMoveSpeedMultiplier;
        }

        private void OnEnable()
        {
            GameManager.AddListener<PlayerDeathMessage>(OnPlayerDeath);

            moveInputActionReference.action.performed += OnMoveInputActionPerformed;
            dashInputActionReference.action.performed += OnDashInputActionPerformed;
            moveInputActionReference.action.canceled += OnMoveInputActionCanceled;
            attackInputActionReference.action.performed += OnAttackInputActionPerformed;
            debuffController.OnDebuffDurationExceeded += OnDebuffDurationExceeded;

            strongAttackInputActionReference.action.performed += OnStrongAttackInputActionPerformed;
        }

        private void OnPlayerDeath(PlayerDeathMessage message)
        {
            movementController.enabled = false;
            attackController.enabled = false;
            strongAttackController.enabled = false;
            debuffController.enabled = false;
            dashController.enabled = false;
            scoreSystem.StopTimer();
            onPlayerDeath.Invoke();
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<PlayerDeathMessage>(OnPlayerDeath);

            moveInputActionReference.action.performed -= OnMoveInputActionPerformed;
            moveInputActionReference.action.canceled -= OnMoveInputActionCanceled;
            dashInputActionReference.action.performed -= OnDashInputActionPerformed;
            attackInputActionReference.action.performed -= OnAttackInputActionPerformed;
            debuffController.OnDebuffDurationExceeded -= OnDebuffDurationExceeded;

            strongAttackInputActionReference.action.performed -= OnStrongAttackInputActionPerformed;

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

        private void OnDashInputActionPerformed(InputAction.CallbackContext context)
        {
            dashController.Dash();
        }

        private void OnAttackInputActionPerformed(InputAction.CallbackContext context)
        {
            if(!attackController.IsAttacking)
                attackController.Attack();
        }

        private void OnStrongAttackInputActionPerformed(InputAction.CallbackContext context)
        {
            if(!attackController.IsAttacking)
                strongAttackController.Attack();
        }

        private void OnDebuffDurationExceeded()
        {
            GameOver();
        }

        [Button("Kill Player")]
        private static void GameOver()
        {
            GameManager.Publish(new PlayerDeathMessage());
        }
    }
}
