using System;

namespace cqrs_documents.Events
{
    internal class OrderPlaced : Message, IHaveTtl
    {
        public readonly Order Order;

        public OrderPlaced(Order order)
        {
            Order = order;
        }

        public DateTimeOffset expiry { get; set; }
    }
}