using System;
using System.Collections.Generic;

namespace cqrs_documents.Actors
{
    internal class Waiter
    {
        private readonly List<string> _dishes = new List<string>();
        private readonly IHandleOrder _handler;
        private readonly IMenuService _menuService;

        public Waiter(IHandleOrder handler, IMenuService menuService)
        {
            _handler = handler;
            _menuService = menuService;
        }

        public void PlaceOrder(int tableNumber, params string[] descriptions)
        {
            Console.WriteLine($"Waiter places order for table {tableNumber}");

            var order = new Order();

            foreach (var description in descriptions)
            {
                order.lineItems.Add(new LineItem() {text = description});
            }

            _handler.Handle(order);
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