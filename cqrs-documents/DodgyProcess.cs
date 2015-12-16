using System;
using cqrs_documents.Commands;
using cqrs_documents.Events;

namespace cqrs_documents
{
    internal class DodgyProcess : IProcessManager, IHandle<OrderPlaced>, IHandle<OrderPriced>, IHandle<OrderPaid>
    {
        private readonly IBus _bus;

        public DodgyProcess(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(OrderPlaced message)
        {
            _bus.Publish(new PriceOrder(message.Order)
            {
                CorrelationId = message.CorrelationId,
                CausationId = message.MessageId
            });
        }

        public void Handle(OrderPaid message)
        {
            _bus.Publish(new CookFood(message.Order)
            {
                CorrelationId = message.CorrelationId,
                CausationId = message.MessageId,
                expiry = DateTimeOffset.UtcNow.AddHours(2)
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