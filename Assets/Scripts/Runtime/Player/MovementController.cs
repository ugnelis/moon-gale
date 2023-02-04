using UnityEngine;

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
        private Camera mainCamera;

        public Vector3 AbsoluteMoveDirection { get; set; }

        public bool IsMoveInputActive { get; set; }

        // ReSharper disable once ConvertToAutoProperty
        public float CurrentMoveSpeed
        {
            get => currentMoveSpeed;
            set => currentMoveSpeed = value;
        }

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
            var relativeMotion = GetRelativeMotion(mainCameraTransform, AbsoluteMoveDirection);
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

        private void FixedUpdate()
        {
            UpdateCurrentSpeed();
            UpdateMovement();
        }

        private void UpdateCurrentSpeed()
        {
            var newMoveSpeed = IsMoveInputActive
                ? CurrentMoveSpeed + playerSettings.MoveAcceleration * Time.deltaTime
                : CurrentMoveSpeed - playerSettings.StopAcceleration * Time.deltaTime;

            var clampedMoveSpeed = Mathf.Clamp(
                newMoveSpeed,
                min: 0f,
                max: playerSettings.MaxMoveSpeed
            );

            CurrentMoveSpeed = clampedMoveSpeed;
        }

        private void UpdateMovement()
        {
            if (CurrentMoveSpeed <= 0)
            {
                return;
            }

            var mainCameraTransform = mainCamera.transform;
            var relativeMotion = GetRelativeMotion(mainCameraTransform, AbsoluteMoveDirection);

            characterController.Move(relativeMotion);
        }

        private Vector3 GetRelativeMotion(Transform forwardTransform, Vector3 absoluteDirection)
        {
            var direction = forwardTransform.forward;
            var relativeProjectedDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
            var relativeDirection = relativeProjectedDirection.normalized;
            var relativeRotation = Quaternion.LookRotation(relativeDirection);
            var relativeMotion = relativeRotation * absoluteDirection * CurrentMoveSpeed;

            return relativeMotion;
        }
    }
}
