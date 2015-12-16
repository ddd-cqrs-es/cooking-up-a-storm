using System;
using cqrs_documents.Commands;
using cqrs_documents.Events;

namespace cqrs_documents.Actors
{
    class Cashier : IHandle<TakePayment>
    {
        private readonly Bus _bus;

        public Cashier(Bus bus)
        {
            _bus = bus;
        }

        public void Handle(TakePayment message)
        {
            var order = message.Order;

            Console.WriteLine($"Cashier handles payment for table {order.tableNumber}");

            order.paid = true;

            _bus.Publish(new OrderPaid(order) { CorrelationId = message.CorrelationId, CausationId = message.MessageId });
        }
    }
}