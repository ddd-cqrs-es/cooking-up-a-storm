using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using cqrs_documents.Actors;
using cqrs_documents.Commands;
using cqrs_documents.Events;

namespace cqrs_documents
{
    internal class Program
    {
        private static bool _stop;

        private static void Main(string[] args)
        {
            IList<IStartable> startables = new List<IStartable>();

            var bus = new Bus();
            var cashier = new ThreadedHandler<TakePayment>("Cashier", new Cashier(bus));
            var assistantManager = new ThreadedHandler<PriceOrder>("Assistant Manager",
                new AssistantManager(new MenuService(), bus));
            var cook1 = new ThreadedHandler<CookFood>("Cook (Chewie)",
                new TtlHandler<CookFood>(new Cook("Chewie", 123, bus)));
            var cook2 = new ThreadedHandler<CookFood>("Cook (Luke)",
                new TtlHandler<CookFood>(new Cook("Luke", 456, bus)));
            var cook3 = new ThreadedHandler<CookFood>("Cook (Darth)",
                new TtlHandler<CookFood>(new Cook("Darth", 217, bus)));
            var mfdDispatcher = new MfdDispatcher<CookFood>(new[] {cook1, cook2, cook3});
            var kitchen = new ThreadedHandler<CookFood>("Kitchen", mfdDispatcher);
            var waiter = new Waiter(new MenuService(), bus);

            bus.Subscribe(cashier);
            bus.Subscribe(assistantManager);
            bus.Subscribe(kitchen);

            var house = new ProcessManagerHouse(bus);
            bus.Subscribe<OrderPlaced>(house);
            bus.Subscribe<OrderPaid>(house);

            var alarmClock = new AlarmClock(bus);
            bus.Subscribe(alarmClock);

            startables.Add(cashier);
            startables.Add(assistantManager);
            startables.Add(cook1);
            startables.Add(cook2);
            startables.Add(cook3);
            startables.Add(kitchen);


            startables.Add(alarmClock);

            foreach (var startable in startables)
            {
                startable.StartListening();
            }

            Task.Factory.StartNew(() =>
            {
                while (!_stop)
                {
                    foreach (var startable in startables)
                    {
                        Console.WriteLine($"Queue: {startable.Name} Count: {startable.Count}");
                    }

                    Thread.Sleep(1000);
                }
            },
                TaskCreationOptions.LongRunning);

            for (var i = 0; i < 100; i++)
            {
                var isDodgy = false;//i%2 == 0;
                waiter.PlaceOrder(i, isDodgy, "Sausages", "Beans");
            }

            Console.ReadKey();
        }
    }
}