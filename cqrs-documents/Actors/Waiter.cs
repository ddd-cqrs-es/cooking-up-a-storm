using System;
using System.Collections.Generic;
using cqrs_documents.Events;

namespace cqrs_documents.Actors
{
    internal class Waiter
    {
        private readonly Bus _bus;
        private readonly List<string> _dishes = new List<string>();
        private readonly IMenuService _menuService;

        public Waiter(IMenuService menuService, Bus bus)
        {
            _menuService = menuService;
            _bus = bus;
        }

        public void PlaceOrder(int sequence, params string[] descriptions)
        {
            Console.WriteLine($"Waiter places order for table {sequence}");

            var order = new Order {tableNumber = sequence};

            foreach (var description in descriptions)
            {
                order.lineItems.Add(new LineItem {text = description});
            }

            var correlationId = Guid.NewGuid();
            var printer = new TableDisplay();

            _bus.Subscribe<OrderPlaced>(printer, correlationId);
            _bus.Subscribe<OrderCooked>(printer, correlationId);
            _bus.Subscribe<OrderPriced>(printer, correlationId);
            _bus.Subscribe<OrderPaid>(printer, correlationId);
            
            _bus.Publish(new OrderPlaced(order)
            {
                CorrelationId = correlationId,
                expiry = DateTimeOffset.UtcNow.AddSeconds(2)
            });
        }
    }
}