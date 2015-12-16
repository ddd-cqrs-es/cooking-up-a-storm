using System;
using Restaurant.Commands;
using Restaurant.Events;

namespace Restaurant.Actors
{
    class AssistantManager : IHandle<PriceOrder>
    {
        private readonly IMenuService _service;
        private readonly Bus _bus;

        public AssistantManager(IMenuService service, Bus bus)
        {
            _service = service;
            _bus = bus;
        }

        public void Handle(PriceOrder message)
        {
            Console.WriteLine($"Assistant manager handles order for table {message.Order.tableNumber}");

            var total = 0;
            var order = message.Order;

            foreach (var lineItem in order.lineItems)
            {
                total += _service.GetPrice(lineItem.text);
            }

            order.tax = total*.2;
            order.total = total + order.tax;

            _bus.Publish(new OrderPriced(order) {CorrelationId = message.CorrelationId, CausationId = message.MessageId});
        }
    }
}