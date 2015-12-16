using System;

namespace cqrs_documents.Commands
{
    internal class CookFood : Message, IHaveTtl
    {
        public readonly Order Order;

        public CookFood(Order order)
        {
            Order = order;
        }

        public DateTimeOffset expiry { get; set; }
    }
}