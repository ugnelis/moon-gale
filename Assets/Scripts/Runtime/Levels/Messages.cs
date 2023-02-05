using MoonGale.Core;
using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal sealed class DangerStartedMessage : IMessage
    {
        public Transform Target { get; }

        public DangerStartedMessage(Transform target)
        {
            Target = target;
        }
    }

    internal sealed class DangerStoppedMessage : IMessage
    {
        public Transform Target { get; }

        public DangerStoppedMessage(Transform target)
        {
            Target = target;
        }
    }
}
