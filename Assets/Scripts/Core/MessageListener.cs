using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoonGale.Core
{
    internal sealed class MessageListener<TMessage> : MessageListener where TMessage : IMessage
    {
        private readonly ICollection<Action<TMessage>> listeners = new List<Action<TMessage>>();

        public int ListenerCount => listeners.Count;

        public void AddListener(Action<TMessage> listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(Action<TMessage> listener)
        {
            listeners.Remove(listener);
        }

        public void Publish(TMessage message)
        {
            foreach (var listener in listeners)
            {
                try
                {
                    listener.Invoke(message);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }
    }

    internal abstract class MessageListener
    {
    }
}
