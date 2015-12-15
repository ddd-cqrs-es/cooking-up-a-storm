using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using cqrs_documents.Actors;

namespace cqrs_documents
{
    internal class Program
    {
        private static bool _stop;

        private static void Main(string[] args)
        {
            IList<IStartable> startables = new List<IStartable>();

            var bus = new Bus();

            var cashier = new ThreadedHandleOrder("Cashier", new Cashier(bus));
            var assistantManager = new ThreadedHandleOrder("Assistant Manager", new AssistantManager(cashier, new MenuService(), bus));
            var cook1 = new ThreadedHandleOrder("Cook (Chewie)", new TtlHandler(new Cook("Chewie", assistantManager, 123, bus)));
            var cook2 = new ThreadedHandleOrder("Cook (Luke)", new TtlHandler(new Cook("Luke", assistantManager, 456, bus)));
            var cook3 = new ThreadedHandleOrder("Cook (Darth)", new TtlHandler(new Cook("Darth", assistantManager,217, bus)));
            var mfdDispatcher = new MfdDispatcher(new[] {cook1, cook2, cook3});
            var kitchen = new ThreadedHandleOrder("Kitchen", mfdDispatcher);
            var waiter = new Waiter(kitchen, new MenuService(), bus);

            bus.Subscribe(cashier, Bus.BillCalculated);
            bus.Subscribe(assistantManager, Bus.FoodCooked);
            bus.Subscribe(kitchen, Bus.OrderPlaced);

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