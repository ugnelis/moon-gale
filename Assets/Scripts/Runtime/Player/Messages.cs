using MoonGale.Core;

namespace MoonGale.Runtime.Player
{
    internal sealed class PlayerDeathMessage : IMessage
    {
    }

    internal sealed class PlayerStrongAttackMessage : IMessage
    {
        public float NextAttackTimeSeconds { get; }

        public PlayerStrongAttackMessage(float nextAttackTimeSeconds)
        {
            NextAttackTimeSeconds = nextAttackTimeSeconds;
        }
    }

    internal sealed class PlayerWeakAttackMessage : IMessage
    {
        public float NextAttackTimeSeconds { get; }

        public PlayerWeakAttackMessage(float nextAttackTimeSeconds)
        {
            NextAttackTimeSeconds = nextAttackTimeSeconds;
        }
    }

    internal sealed class PlayerDashMessage : IMessage
    {
        public float NextDashTimeSeconds { get; }

        public PlayerDashMessage(float nextDashTimeSeconds)
        {
            NextDashTimeSeconds = nextDashTimeSeconds;
        }
    }
}
