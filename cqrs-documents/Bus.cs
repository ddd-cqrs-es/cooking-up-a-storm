using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace cqrs_documents
{
    class MockBus : IBus
    {
        public List<Message> Messages { get; } = new List<Message>();

        public void Publish<T>(T message) where T : Message
        {
            Messages.Add(message);
        }

        public void SubscribeByCorrelationId<T>(IHandle<T> handler, Guid correlationId) where T : Message
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>(IHandle<T> handler) where T : Message
        {
            throw new NotImplementedException();
        }
    }

    internal class Bus : IBus
    {
        private readonly ConcurrentDictionary<string, IList<object>> _handlers =
            new ConcurrentDictionary<string, IList<object>>();

        public void Publish<T>(T message) where T : Message
        {
            PublishByType(message);
            PublishByCorrelationId(message);
        }

        private void PublishByCorrelationId<T>(T message) where T : Message
        {
            var correlationId = message.CorrelationId.ToString();

            if (!_handlers.ContainsKey(correlationId))
                return;

            foreach (dynamic handler in _handlers[correlationId])
            {
                try
                {
                    handler.Handle(message);
                }
                catch
                {
                }
            }
        }

        private void PublishByType<T>(T message)
        {
            var typeName = typeof (T).FullName;

            if (!_handlers.ContainsKey(typeName))
                return;

            foreach (dynamic handler in _handlers[typeName])
            {
                handler.Handle(message);
            }
        }

        public void SubscribeByCorrelationId<T>(IHandle<T> handler, Guid correlationId) where T : Message
        {
            var key = correlationId.ToString();
            if (!_handlers.ContainsKey(key))
            {
                _handlers[key] = new List<object>();
            }

            _handlers[key].Add(handler);
        }

        public void Subscribe<T>(IHandle<T> handler) where T : Message
        {
            var typeName = typeof (T).FullName;
            if (!_handlers.ContainsKey(typeName))
            {
                _handlers[typeName] = new List<object>();
            }

            _handlers[typeName].Add(handler);
        }
    }
}