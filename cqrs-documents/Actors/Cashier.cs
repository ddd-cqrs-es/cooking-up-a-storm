using System;

namespace cqrs_documents.Actors
{
    class Cashier : IHandleOrder
    {
        private readonly Bus _bus;

        public Cashier(Bus bus)
        {
            _bus = bus;
        }

        public void Handle(Order order)
        {
            Console.WriteLine($"Cashier handles payment for table {order.tableNumber}");

            order.paid = true;

            _bus.Publish(Bus.OrderPaid, order);
        }
    }
}