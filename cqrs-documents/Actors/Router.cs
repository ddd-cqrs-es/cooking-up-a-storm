using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cqrs_documents.Commands;
using cqrs_documents.Events;

namespace cqrs_documents.Actors
{
    class Router :IHandle<OrderPlaced>,IHandle<OrderCooked>,IHandle<OrderPriced>
    {
        private readonly Bus _bus;

        public Router(Bus bus)
        {
            _bus = bus;
        }

        public void Handle(OrderPlaced message)
        {
            _bus.Publish(new CookFood(message.Order));
        }

        public void Handle(OrderCooked message)
        {
            _bus.Publish(new PriceOrder(message.Order));
        }

        public void Handle(OrderPriced message)
        {
            _bus.Publish(new TakePayment(message.Order));
        }
    }
}
