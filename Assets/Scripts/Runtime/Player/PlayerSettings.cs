using MoonGale.Runtime.Utilities;
using UnityEngine;

namespace MoonGale.Runtime.Player
{
    [CreateAssetMenu(
        menuName = Constants.CreateAssetMenuName + "/Player Settings",
        fileName = "New " + nameof(PlayerSettings)
    )]
    internal sealed class PlayerSettings : ScriptableObject
    {
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

        [Header("Animation")]
        [Min(0f)]
        [SerializeField]
        private float pivotLookSpeed = 10f;

        [Header("Attacks")]
        [Min(0f)]
        [SerializeField]
        private float attackDurationSeconds = 0.3f;

        [Min(0f)]
        [SerializeField]
        private float attackCooldownSeconds = 0.3f;

        [SerializeField]
        private LayerMask attackLayerMask;

        [Header("Debuffs")]
        [Min(0f)]
        [SerializeField]
        private float maxDebuffDurationSeconds = 3f;

        [Range(0f, 1f)]
        [SerializeField]
        private float maxDebuffVignette = 1f;

        [Range(-5f, 5f)]
        [SerializeField]
        private float minDebuffPostExposure = -1f;

        public float MaxMoveSpeed => maxMoveSpeed;

        public float MoveAcceleration => moveAcceleration;

        public float StopAcceleration => stopAcceleration;

        public float AttackDurationSeconds => attackDurationSeconds;

        public float AttackCooldownSeconds => attackCooldownSeconds;

        public LayerMask AttackLayerMask => attackLayerMask;

        public float MaxDebuffDurationSeconds => maxDebuffDurationSeconds;

        public float MaxDebuffVignette => maxDebuffVignette;

        public float MinDebuffPostExposure => minDebuffPostExposure;

        public float PivotLookSpeed => pivotLookSpeed;
    }
}
