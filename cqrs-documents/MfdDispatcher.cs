using System.Collections.Generic;
using System.Threading;

namespace cqrs_documents
{
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