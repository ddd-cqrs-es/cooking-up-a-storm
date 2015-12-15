using System;

namespace cqrs_documents.Actors
{
    class AssistantManager : IHandleOrder
    {
        private readonly IHandleOrder _handler;
        private readonly IMenuService _service;

        public AssistantManager(IHandleOrder handler, IMenuService service)
        {
            _handler = handler;
            _service = service;
        }

        public void Handle(Order order)
        {
            Console.WriteLine($"Assistant manager handles order for table {order.tableNumber}");

            var total = 0;

            foreach (var lineItem in order.lineItems)
            {
                total += _service.GetPrice(lineItem.text);
            }

            order.tax = total*.2;
            order.total = total + order.tax;

            _handler.Handle(order);
        }
    }
}