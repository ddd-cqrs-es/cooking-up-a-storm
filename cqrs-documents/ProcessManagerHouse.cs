using System;
using System.Collections.Concurrent;
using cqrs_documents.Events;

namespace cqrs_documents
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
            _bus.SubscribeByCorrelationId<OrderPlaced>(process, message.CorrelationId);

            _persistence[message.CorrelationId] = (IProcessManager)process;
        }

        public void Handle(OrderPaid message)
        {
            IProcessManager pm;
            _persistence.TryRemove(message.CorrelationId, out pm);
        }
    }
}