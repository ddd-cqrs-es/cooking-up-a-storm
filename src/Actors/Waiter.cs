using System;
using System.Collections.Generic;
using Restaurant.Events;

namespace Restaurant.Actors
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

        public void PlaceOrder(int sequence, bool isDodgy, params string[] descriptions)
        {
            Console.WriteLine($"Waiter places order for table {sequence}");

            var order = new Order {tableNumber = sequence, dodgyCustomer = isDodgy};

            foreach (var description in descriptions)
            {
                order.lineItems.Add(new LineItem {text = description});
            }

            var correlationId = Guid.NewGuid();

            _bus.Publish(new OrderPlaced(order)
            {
                CorrelationId = correlationId,
                expiry = DateTimeOffset.UtcNow.AddSeconds(2)
            });
        }
    }
}