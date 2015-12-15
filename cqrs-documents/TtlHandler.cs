using System;
using cqrs_documents.Events;

namespace cqrs_documents
{
    class TtlHandler<T> : IHandle<T> where T : Message
    {
        private readonly IHandle<T> _handler;

        public TtlHandler(IHandle<T> handler)
        {
            _handler = handler;
        }

        public void Handle(T message)
        {
            var ttl = message as IHaveTtl;

            if (ttl == null) return;

            if (ttl.expiry > DateTimeOffset.UtcNow)
                _handler.Handle(message);
            else
            {
                var orderPlaced = message as OrderPlaced;
                if (orderPlaced == null) return;
                Console.WriteLine($"Table {orderPlaced.Order.tableNumber} have left the restaurant");
            }
        }
    }
}