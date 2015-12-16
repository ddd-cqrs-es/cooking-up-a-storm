using cqrs_documents.Commands;
using cqrs_documents.Events;

namespace cqrs_documents.Actors
{
    class Router : IHandle<OrderPlaced>, IHandle<OrderCooked>, IHandle<OrderPriced>
    {
        private readonly Bus _bus;

        public Router(Bus bus)
        {
            _bus = bus;
        }

        public void Handle(OrderPlaced message)
        {
            var cookFood = new CookFood(message.Order) {CorrelationId = message.CorrelationId};
            _bus.Publish(cookFood);
        }

        public void Handle(OrderCooked message)
        {
            _bus.Publish(new PriceOrder(message.Order)
            {
                CorrelationId = message.CorrelationId,
                CausationId = message.MessageId
            });
        }

        public void Handle(OrderPriced message)
        {
            _bus.Publish(new TakePayment(message.Order)
            {
                CorrelationId = message.CorrelationId,
                CausationId = message.MessageId
            });
        }
    }
}