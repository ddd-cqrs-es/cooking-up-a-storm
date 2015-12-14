using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace cqrs_documents.Actors
{
    class AssistantManager : IHandlerOrder
    {
        private readonly IHandlerOrder _handler;
        private readonly IMenuService _service;

        public AssistantManager(IHandlerOrder handler, IMenuService service)
        {
            _handler = handler;
            _service = service;
        }


        public void Handle(Order order)
        {
            var total = 0;
            foreach (var lineItem in order.GetLineItems())
            {
                total += _service.GetPrice(lineItem.Description);
            }

            order.tax = total*.2;
            order.total = total + order.tax;

            _handler.Handle(order);
        }
    }

    internal interface IMenuService
    {
        int GetPrice(string description);
    }

    class Cook : IHandlerOrder
    {
        private readonly IHandlerOrder _handler;

        private readonly IDictionary<string, List<Tuple<string, double>>> _recipes =
            new ConcurrentDictionary<string, List<Tuple<string, double>>>();

        public Cook(IHandlerOrder handler)
        {
            _handler = handler;
        }

        public void Handle(Order order)
        {
            order.timeToCook = new Random().Next(500);
            foreach (var lineItem in order.GetLineItems())
            {
                order.AddIngredients(_recipes[lineItem.Description]);
            }
            Thread.Sleep(500);
            _handler.Handle(order);
        }
    }
}