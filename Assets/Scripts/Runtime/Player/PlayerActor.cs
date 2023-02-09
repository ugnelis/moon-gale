using Cinemachine;
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
        [SerializeField]
        private PlayerSettings settings;

        [Header("Controllers")]
        [SerializeField]
        private CinemachineVirtualCamera cinemachineCamera;

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

        public bool IsDashing => dashController.IsDashing;

        private void Awake()
        {
            scoreSystem = GameManager.GetSystem<IScoreSystem>();
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
            strongAttackInputActionReference.action.performed += OnStrongAttackInputActionPerformed;

            debuffController.OnDebuffDurationExceeded += OnDebuffDurationExceeded;
            strongAttackController.OnAttacked += OnStrongAttacked;
            attackController.OnAttacked += OnWeakAttacked;
            dashController.OnDashed += OnDashed;
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<PlayerDeathMessage>(OnPlayerDeath);

            moveInputActionReference.action.performed -= OnMoveInputActionPerformed;
            moveInputActionReference.action.canceled -= OnMoveInputActionCanceled;
            dashInputActionReference.action.performed -= OnDashInputActionPerformed;
            attackInputActionReference.action.performed -= OnAttackInputActionPerformed;
            strongAttackInputActionReference.action.performed -= OnStrongAttackInputActionPerformed;

            debuffController.OnDebuffDurationExceeded -= OnDebuffDurationExceeded;
            strongAttackController.OnAttacked -= OnStrongAttacked;
            attackController.OnAttacked -= OnWeakAttacked;
            dashController.OnDashed -= OnDashed;

            movementController.StopMovement();
        }

        private void OnPlayerDeath(PlayerDeathMessage message)
        {
            cinemachineCamera.Follow = null;
            cinemachineCamera.LookAt = null;
            cinemachineCamera.gameObject.transform.parent = null;

            movementController.enabled = false;
            attackController.enabled = false;
            strongAttackController.enabled = false;
            debuffController.enabled = false;
            dashController.enabled = false;
            scoreSystem.StopTimer();
            onPlayerDeath.Invoke();
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
            if (strongAttackController.IsAttacking == false)
            {
                attackController.AttackCooldownSeconds = settings.AttackCooldownSeconds;
                attackController.Attack();
            }
        }

        private void OnStrongAttackInputActionPerformed(InputAction.CallbackContext context)
        {
            if (attackController.IsAttacking == false)
            {
                strongAttackController.AttackCooldownSeconds = settings.StrongAttackCooldownSeconds;
                strongAttackController.Attack();
            }
        }

        private void OnDebuffDurationExceeded()
        {
            Kill();
        }

        private static void OnStrongAttacked(float nextAttackTime)
        {
            GameManager.Publish(new PlayerStrongAttackMessage(nextAttackTime));
        }

        private static void OnWeakAttacked(float nextAttackTime)
        {
            GameManager.Publish(new PlayerWeakAttackMessage(nextAttackTime));
        }

        private static void OnDashed(float nextDashTime)
        {
            GameManager.Publish(new PlayerDashMessage(nextDashTime));
        }

        [Button("Kill Player")]
        public static void Kill()
        {
            GameManager.Publish(new PlayerDeathMessage());
        }
    }
}
