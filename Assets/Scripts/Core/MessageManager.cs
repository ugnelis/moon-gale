using System;
using System.Collections.Generic;

namespace MoonGale.Core
{
    internal sealed class MessageManager
    {
        private readonly IDictionary<Type, MessageListener> listeners =
            new Dictionary<Type, MessageListener>();

        public void AddListener<TMessage>(Action<TMessage> listener) where TMessage : IMessage
        {
            var listenerType = typeof(TMessage);
            if (listeners.TryGetValue(listenerType, out var existingMessageListener))
            {
                var typedMessageListener = (MessageListener<TMessage>)existingMessageListener;
                typedMessageListener.AddListener(listener);
            }
            else
            {
                var newMessageListener = new MessageListener<TMessage>();
                newMessageListener.AddListener(listener);

                listeners[listenerType] = newMessageListener;
            }
        }

        public void RemoveListener<TMessage>(Action<TMessage> listener) where TMessage : IMessage
        {
            var listenerType = typeof(TMessage);
            if (listeners.TryGetValue(listenerType, out var existingMessageListener) == false)
            {
                return;
            }

            var typedMessageListener = (MessageListener<TMessage>)existingMessageListener;
            typedMessageListener.RemoveListener(listener);

            if (typedMessageListener.ListenerCount <= 0)
            {
                listeners.Remove(listenerType);
            }
        }

        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            var listenerType = typeof(TMessage);
            if (listeners.TryGetValue(listenerType, out var existingMessageListener) == false)
            {
                return;
            }

            var typedMessageListener = (MessageListener<TMessage>)existingMessageListener;
            typedMessageListener.Publish(message);
        }
    }
}
