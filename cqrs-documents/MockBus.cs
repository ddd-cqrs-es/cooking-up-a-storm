using System;
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
}