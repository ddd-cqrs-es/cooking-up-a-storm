using System;
using cqrs_documents.Commands;
using cqrs_documents.Events;

namespace cqrs_documents
{
    internal class LondonProcess : IProcessManager, IHandle<OrderPlaced>, IHandle<OrderCooked>, IHandle<OrderPriced>
    {
        private readonly IBus _bus;
        private bool _foodCooked;

        public LondonProcess(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(OrderPlaced message) 
        {
            var cookFood = new CookFood(message.Order)
            {
                CorrelationId = message.CorrelationId,
                CausationId = message.MessageId,
                expiry = DateTimeOffset.Now.AddMilliseconds(5)
            };

            var timedOut = new CookOrderTimedOut(message.Order)
            {

                CorrelationId = message.CorrelationId,
                CausationId = message.MessageId
            };

            _bus.Publish(cookFood);
            _bus.Publish(new DelayedPublish(timedOut, DateTime.Now.AddMilliseconds(100))
            {
                CorrelationId = message.CorrelationId
            });
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

        public void Handle(CookOrderTimedOut message)
        {
            if (_foodCooked) return;
            var order = message.Order;
            if (message.Count >= 2)
            {
                _bus.Publish(new CookFailed(order));
            }

            ++message.Count;
            Console.WriteLine($"Retrying cook for table: {message.Order.tableNumber}");
            _bus.Publish(new CookFood(order) {expiry = DateTimeOffset.Now.AddMilliseconds(50)});
            _bus.Publish(new DelayedPublish(message, DateTime.Now.AddMilliseconds(100)));
        }
    }
}