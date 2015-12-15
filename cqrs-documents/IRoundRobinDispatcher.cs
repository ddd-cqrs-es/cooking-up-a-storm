using System.Collections.Generic;

namespace cqrs_documents
{
    internal class RoundRobinDispatcher : IHandleOrder
    {
        private readonly Queue<IHandleOrder> _queue;

        public RoundRobinDispatcher(IEnumerable<IHandleOrder> handlers)
        {
            _queue = new Queue<IHandleOrder>(handlers);
        }

        public void Handle(Order order)
        {
            var handler = _queue.Dequeue();
            handler.Handle(order);
            _queue.Enqueue(handler);
        }
    }
    

}