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
        [Tooltip("How fast the player moves")]
        [Min(0f)]
        [SerializeField]
        private float maxMoveSpeed = 0.2f;

        [Tooltip("How fast the player accelerates")]
        [Min(0f)]
        [SerializeField]
        private float moveAcceleration = 2f;

        [Tooltip("How fast the player accelerates on dash")]
        [Min(0f)]
        [SerializeField]
        private float dashAcceleration = 4f;

        [Tooltip("How much time takes dash movement")]
        [Min(0f)]
        [SerializeField]
        private float dashDurationSeconds = 2f;

        [Tooltip("How fast the player acceleration stops on releasing input")]
        [Min(0f)]
        [SerializeField]
        private float stopAcceleration = 1f;

        [Header("Animation")]
        [Tooltip("Speed used to translate pivots (lerping)")]
        [Min(0f)]
        [SerializeField]
        private float pivotLookSpeed = 10f;

        [Header("Attacks")]
        [Tooltip(
            "Duration of the attack (player is blocked from doing another). This also impacts " +
            "events"
        )]
        [Min(0f)]
        [SerializeField]
        private float attackDurationSeconds = 0.3f;

        [Tooltip("Cooldown until the player can perform the next attack")]
        [Min(0f)]
        [SerializeField]
        private float attackCooldownSeconds = 0.3f;

        [Min(0f)]
        [SerializeField]
        private float strongAttackDurationSeconds = 0.3f;

        [Tooltip("Cooldown until the player can perform the next attack")]
        [Min(0f)]
        [SerializeField]
        private float strongAttackCooldownSeconds = 0.3f;

        [Header("Debuffs")]
        [Tooltip(
            "How much time must pass for the player to receive the max penalty for being " +
            "debuffed (e.g., die from roots)"
        )]
        [Min(0f)]
        [SerializeField]
        private float maxDebuffDurationSeconds = 3f;

        [Tooltip("Maximum power of the Vignette effect based on debuff strength")]
        [Range(0f, 1f)]
        [SerializeField]
        private float maxDebuffVignette = 1f;

        [Tooltip("Minimum multiplier when slowing the player (less = slower player over time)")]
        [Range(0f, 1f)]
        [SerializeField]
        private float minDebuffMoveSpeedMultiplier = 0.5f;

        [Tooltip("Minimum value of post exposure on debuff strength")]
        [Range(-5f, 5f)]
        [SerializeField]
        private float minDebuffPostExposure = -1f;

        public float MaxMoveSpeed => maxMoveSpeed;

        public float MoveAcceleration => moveAcceleration;

        public float DashAcceleration => dashAcceleration;

        public float DashDurationSeconds => dashDurationSeconds;

        public float StopAcceleration => stopAcceleration;

        public float AttackDurationSeconds => attackDurationSeconds;

        public float AttackCooldownSeconds => attackCooldownSeconds;

        public float MaxDebuffDurationSeconds => maxDebuffDurationSeconds;

        public float MaxDebuffVignette => maxDebuffVignette;

        public float MinDebuffPostExposure => minDebuffPostExposure;

        public float PivotLookSpeed => pivotLookSpeed;

        public float MinDebuffMoveSpeedMultiplier => minDebuffMoveSpeedMultiplier;

        public float StrongAttackDurationSeconds => strongAttackDurationSeconds;

        public float StrongAttackCooldownSeconds => strongAttackCooldownSeconds;
    }
}
