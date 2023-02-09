using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoonGale.Core
{
    internal sealed class MessageListener<TMessage> : MessageListener where TMessage : IMessage
    {
        private readonly IList<Action<TMessage>> listeners = new List<Action<TMessage>>();

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
            for (var index = listeners.Count - 1; index >= 0; index--)
            {
                var listener = listeners[index];
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
