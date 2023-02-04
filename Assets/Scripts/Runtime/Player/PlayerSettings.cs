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

        public float MaxMoveSpeed => maxMoveSpeed;

        public float MoveAcceleration => moveAcceleration;

        public float StopAcceleration => stopAcceleration;
    }
}
