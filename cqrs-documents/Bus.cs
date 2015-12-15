using System.Collections.Concurrent;
using System.Collections.Generic;

namespace cqrs_documents
{
    internal class Bus
    {
        public const string BillCalculated = "BillCalculated";
        public const string OrderPaid = "OrderPaid";
        public const string FoodCooked = "FoodCooked";
        public const string OrderPlaced = "OrderPlaced";

        private readonly ConcurrentDictionary<string, IList<IHandleOrder>> _handlers =
            new ConcurrentDictionary<string, IList<IHandleOrder>>();
    
        public void Publish(string operation, Order order)
        {
            foreach (var handler in _handlers[operation])
            {
                handler.Handle(order);
            }
        }

        public void Subscribe(IHandleOrder handler, string operation)
        {
            if (!_handlers.ContainsKey(operation))
                _handlers[operation] = new List<IHandleOrder>();

            _handlers[operation].Add(handler);
        }
    }
}