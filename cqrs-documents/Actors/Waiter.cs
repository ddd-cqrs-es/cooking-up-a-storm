using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace cqrs_documents.Actors
{
    class Waiter
    {
        private readonly IHandlerOrder _handler;
        private readonly List<string> _dishes = new List<string>(   );
        private IDictionary<short, Order> _map = new ConcurrentDictionary<short, Order>(); 

        public Waiter(IHandlerOrder handler)
        {
            _handler = handler;
        }

        public void PlaceOrder(short tableNumber, params string[] descriptions)
        {
            if (_map.ContainsKey(tableNumber))
            {
                foreach (var description in descriptions)
                {
                    if (_dishes.Contains(description))
                    {
                        _map[tableNumber].AddItem(description);
                    }
                    else
                    {
                        throw new Exception("frown, rub fingers together");
                    }
                   
                }
            }
            _handler.Handle(_map[tableNumber]);
        }
    }

    class Dish
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
