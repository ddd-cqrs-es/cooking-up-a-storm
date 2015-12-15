using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace cqrs_documents.Actors
{
    internal class Cook : IHandleOrder
    {
        private readonly IHandleOrder _handler;
        private readonly string _name;
        private readonly int _delay;
        private readonly Bus _bus;

        //private readonly IDictionary<string, List<Tuple<string, double>>> _recipes =
        //    new ConcurrentDictionary<string, List<Tuple<string, double>>>();
        private readonly IDictionary<string, List<string>> _recipes = new Dictionary<string, List<string>>()
        {
            {"Beans", new List<string>() {"tin can of beans"}},
            {"Sausages", new List<string>() {"piggies"}},
        };

        public Cook(string name, IHandleOrder handler, int delay, Bus bus)
        {
            _name = name;
            _handler = handler;
            _delay = delay;
            _bus = bus;
        }

        public void Handle(Order order)
        {
            Console.WriteLine($"Cook ({_name}) handles order for table {order.tableNumber}");

            order.timeToCook = _delay;
            foreach (var lineItem in order.lineItems)
            {
                var ingredients = _recipes[lineItem.text];
                order.ingredients.AddRange(ingredients);
            }
            Thread.Sleep(_delay);

            _bus.Publish(Bus.FoodCooked, order);
        }
    }
}