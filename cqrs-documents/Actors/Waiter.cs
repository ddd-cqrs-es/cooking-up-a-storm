using System;
using System.Collections.Generic;

namespace cqrs_documents.Actors
{
    internal class Waiter
    {
        private readonly List<string> _dishes = new List<string>();
        private readonly IHandleOrder _handler;
        private readonly IMenuService _menuService;
        private readonly Bus _bus;

        public Waiter(IHandleOrder handler, IMenuService menuService, Bus bus)
        {
            _handler = handler;
            _menuService = menuService;
            _bus = bus;
        }

        public void PlaceOrder(int sequence, params string[] descriptions)
        {
            Console.WriteLine($"Waiter places order for table {sequence}");

            var order = new Order {expiry = DateTimeOffset.UtcNow.AddSeconds(2), tableNumber = sequence};

            foreach (var description in descriptions)
            {
                order.lineItems.Add(new LineItem() {text = description});
            }

            _bus.Publish(Bus.OrderPlaced, order);
        }
    }

    internal class Dish
    {
        public Dish(string description, double price)
        {
            Description = description;
            Price = price;
        }

        public string Description { get; private set; }
        public double Price { get; private set; }
    }
}