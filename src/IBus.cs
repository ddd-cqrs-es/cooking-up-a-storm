using System;

namespace Restaurant
{
    internal interface IBus
    {
        void Publish<T>(T message) where T : Message;
        void SubscribeByCorrelationId<T>(IHandle<T> handler, Guid correlationId) where T : Message;
        void Subscribe<T>(IHandle<T> handler) where T : Message;
    }
}