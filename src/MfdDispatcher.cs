using System.Collections.Generic;
using System.Threading;

namespace Restaurant
{
    class MfdDispatcher<T> : IHandle<T> where T : Message
    {
        private readonly IEnumerable<ThreadedHandler<T>> _handlers;

        public MfdDispatcher(IEnumerable<ThreadedHandler<T>> handlers)
        {
            _handlers = handlers;
        }

        public void Handle(T message)
        {
            while (true)
            {
                foreach (var handler in _handlers)
                {
                    if (handler.Count >= 5) continue;

                    handler.Handle(message);
                    return;
                }
                Thread.Sleep(1);
            }
        }
    }
}