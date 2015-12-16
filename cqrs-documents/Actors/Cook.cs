using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using cqrs_documents.Commands;
using cqrs_documents.Events;

namespace cqrs_documents.Actors
{
    internal class Cook : IHandle<CookFood>
    {
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

        public Cook(string name, int delay, Bus bus)
        {
            _name = name;
            _delay = delay;
            _bus = bus;
        }

        public void Handle(CookFood message)
        {
            var order = message.Order;
            Console.WriteLine($"Cook ({_name}) handles order for table {order.tableNumber}");

            order.timeToCook = _delay;
            foreach (var lineItem in order.lineItems)
            {
                var ingredients = _recipes[lineItem.text];
                order.ingredients.AddRange(ingredients);
            }
            Thread.Sleep(_delay);

            _bus.Publish(new OrderCooked(order) { CorrelationId = message.CorrelationId, CausationId = message.MessageId });
        }
    }
}