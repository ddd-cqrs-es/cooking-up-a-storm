using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using cqrs_documents.Events;

namespace cqrs_documents
{
    public interface IStartable
    {
        string Name { get; }
        int Count { get; }

        void StartListening();

        void Stop();
    }

    public class ThreadedHandler<T> : IStartable, IHandle<T> where T: Message
    {
        private readonly IHandle<T> _handler;
        private readonly ConcurrentQueue<T> _messageQueue = new ConcurrentQueue<T>();
        private bool _stop;

        public ThreadedHandler(string name, IHandle<T> handler)
        {
            Name = name;
            _handler = handler;
        }

        public void Handle(T message)
        {
            _messageQueue.Enqueue(message);
        }

        public string Name { get; }
        public int Count => _messageQueue.Count;

        public void StartListening()
        {
            Task.Factory.StartNew(() =>
            {
                while (!_stop)
                {
                    T message;
                    if (!_messageQueue.TryDequeue(out message))
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    _handler.Handle(message);
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