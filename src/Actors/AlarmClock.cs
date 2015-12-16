using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Restaurant.Events;

namespace Restaurant.Actors
{
    class AlarmClock : IHandle<DelayedPublish>, IStartable
    {
        private readonly IBus _bus;

        private readonly ConcurrentBag<DelayedPublish> _delayed
            = new ConcurrentBag<DelayedPublish>();

        public AlarmClock(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(DelayedPublish message)
        {
            _delayed.Add(message);
        }

        public string Name { get; }
        public int Count { get; }

        public void StartListening()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    foreach (var delayedPublish in _delayed)
                    {
                        if (delayedPublish.At < DateTime.Now)
                        {
                            _bus.Publish(delayedPublish.Message);
                        }
                    }
                    Thread.Sleep(1);
                }
            },
                TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}