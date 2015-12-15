using System;

namespace cqrs_documents.Actors
{
    class Cashier : IHandleOrder
    {
        public void Handle(Order order)
        {
            Console.WriteLine($"Cashier handles payment for table {order.tableNumber}");

            order.paid = true;
        }
    }
}