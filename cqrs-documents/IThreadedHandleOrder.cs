using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace cqrs_documents
{
    public interface IStartable
    {
        string Name { get; }
        int Count { get; }

        void StartListening();

        void Stop();
    }

    public class ThreadedHandleOrder : IStartable, IHandleOrder
    {
        private readonly IHandleOrder _handler;
        private readonly ConcurrentQueue<Order> _orderQueue = new ConcurrentQueue<Order>();
        private bool _stop;

        public ThreadedHandleOrder(string name, IHandleOrder handler)
        {
            Name = name;
            _handler = handler;
        }

        public void Handle(Order order)
        {
            _orderQueue.Enqueue(order);
        }

        public string Name { get; }
        public int Count => _orderQueue.Count;

        public void StartListening()
        {
            Task.Factory.StartNew(() =>
            {
                while (!_stop)
                {
                    Order order;
                    if (!_orderQueue.TryDequeue(out order))
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    _handler.Handle(order);
                }
            },
                TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            _stop = true;
        }
    }
}