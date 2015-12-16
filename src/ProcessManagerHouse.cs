using System;
using System.Collections.Concurrent;
using Restaurant.Events;

namespace Restaurant
{
    class ProcessManagerHouse : IHandle<OrderPlaced>, IHandle<OrderPaid>
    {
        private readonly Bus _bus;

        readonly ConcurrentDictionary<Guid, IProcessManager> _persistence =
            new ConcurrentDictionary<Guid, IProcessManager>();

        public ProcessManagerHouse(Bus bus)
        {
            _bus = bus;
        }

        public void Handle(OrderPlaced message)
        {
            IHandle<OrderPlaced> process;
            if (message.Order.dodgyCustomer)
            {
                process = new DodgyProcess(_bus);
            }
            else
            {
                process = new LondonProcess(_bus);
            }
            _bus.SubscribeByCorrelationId(process, message.CorrelationId);

            _persistence[message.CorrelationId] = (IProcessManager) process;
        }

        public void Handle(OrderPaid message)
        {
            IProcessManager pm;
            _persistence.TryRemove(message.CorrelationId, out pm);
        }
    }
}