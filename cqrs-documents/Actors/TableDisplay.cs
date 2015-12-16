using System;
using cqrs_documents.Events;

namespace cqrs_documents.Actors
{
    class TableDisplay : IHandle<OrderPlaced>, IHandle<OrderCooked>, IHandle<OrderPriced>, IHandle<OrderPaid>
    {
        public void Handle(OrderPlaced message)
        {
            Console.WriteLine(message.CorrelationId);
            Console.WriteLine(message.GetType());
            Console.WriteLine(message.Order);
        }

        public void Handle(OrderCooked message)
        {
            Console.WriteLine(message.CorrelationId);
            Console.WriteLine(message.GetType());
            Console.WriteLine(message.Order);
        }

        public void Handle(OrderPriced message)
        {
            Console.WriteLine(message.CorrelationId);
            Console.WriteLine(message.GetType());
            Console.WriteLine(message.Order);
        }

        public void Handle(OrderPaid message)
        {

            Console.WriteLine(message.CorrelationId);
            Console.WriteLine(message.GetType());
            Console.WriteLine(message.Order);
        }
    }
}