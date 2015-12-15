using System;
using System.Collections.Generic;
using System.Threading;

namespace cqrs_documents
{
    class TtlHandler : IHandleOrder
    {
        private readonly IHandleOrder _handler;

        public TtlHandler(IHandleOrder handler)
        {
            _handler = handler;
        }

        public void Handle(Order order)
        {
            var ttl = order as IHaveTtl;

            if (ttl == null) return;

            if (ttl.expiry > DateTimeOffset.UtcNow)
                _handler.Handle(order);
            else
            {
                Console.WriteLine($"Table {order.tableNumber} have left the restaurant");
            }
        }
    }

    class MfdDispatcher : IHandleOrder
    {
        private readonly IEnumerable<ThreadedHandleOrder> _handlers;

        public MfdDispatcher(IEnumerable<ThreadedHandleOrder> handlers)
        {
            _handlers = handlers;
        }

        public void Handle(Order order)
        {
            while (true)
            {
                foreach (var threadedHandleOrder in _handlers)
                {
                    if (threadedHandleOrder.Count < 5)
                    {
                        threadedHandleOrder.Handle(order);
                        return;
                    }
                }
                Thread.Sleep(1);   
            }
        }
    }
}