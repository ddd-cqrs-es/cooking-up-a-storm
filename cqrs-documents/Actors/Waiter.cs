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

            _bus.Publish(new OrderPlaced(order) {expiry = DateTimeOffset.UtcNow.AddSeconds(2)});
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