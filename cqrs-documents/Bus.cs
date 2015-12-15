using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using cqrs_documents.Events;

namespace cqrs_documents
{
    internal class Bus
    {
        private readonly ConcurrentDictionary<Type, IList<object>> _handlers =
            new ConcurrentDictionary<Type, IList<object>>();
    
        public void Publish<T>(T message)
        {
            if (!_handlers.ContainsKey(typeof(T)))
                return;

            foreach (dynamic handler in _handlers[typeof(T)])
            {
                handler.Handle(message);
            }
        }

        public void Subscribe<T>(IHandle<T> handler) where T : Message
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type))
                _handlers[type] = new List<object>();

            _handlers[type].Add(handler);
        }
    }
}