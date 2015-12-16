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
            var router = new Router(bus);
            var cashier = new ThreadedHandler<TakePayment>("Cashier", new Cashier(bus));
            var assistantManager = new ThreadedHandler<PriceOrder>("Assistant Manager", new AssistantManager(new MenuService(), bus));
            var cook1 = new ThreadedHandler<CookFood>("Cook (Chewie)", new TtlHandler<CookFood>(new Cook("Chewie", 123, bus)));
            var cook2 = new ThreadedHandler<CookFood>("Cook (Luke)", new TtlHandler<CookFood>(new Cook("Luke", 456, bus)));
            var cook3 = new ThreadedHandler<CookFood>("Cook (Darth)", new TtlHandler<CookFood>(new Cook("Darth",217, bus)));
            var mfdDispatcher = new MfdDispatcher<CookFood>(new[] {cook1, cook2, cook3});
            var kitchen = new ThreadedHandler<CookFood>("Kitchen", mfdDispatcher);
            var waiter = new Waiter(new MenuService(), bus);

            bus.Subscribe(cashier);
            bus.Subscribe(assistantManager);
            bus.Subscribe(kitchen);

            bus.Subscribe<OrderPlaced>(router);
            bus.Subscribe<OrderCooked>(router);
            bus.Subscribe<OrderPriced>(router);

            startables.Add(cashier);
            startables.Add(assistantManager);
            startables.Add(cook1);
            startables.Add(cook2);
            startables.Add(cook3);
            startables.Add(kitchen);

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
                waiter.PlaceOrder(i, "Sausages", "Beans");
                Thread.Sleep(5);
            }
            Console.ReadKey();
        }
    }

    internal class MenuService : IMenuService
    {
        public int GetPrice(string description)
        {
            return 10;
        }
    }
}