using System;

namespace cqrs_documents.Events
{
    internal class CookOrderTimedOut : Message, IHaveTtl
    {
        public readonly Order Order;

        public CookOrderTimedOut(Order order)
        {
            Order = order;
        }

        public DateTimeOffset expiry { get; set; }
        public int Count { get; set; }
    }
}