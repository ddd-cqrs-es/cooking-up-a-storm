using System;
using NUnit.Framework;
using Restaurant.Events;

namespace Restaurant.Specifications
{
    [TestFixture]
    public class when_dodgy_customer_places_order
    {
        [Test]
        public void should_have_ttl_in_future()
        {
            var mockBus = new MockBus();
            var dodgyProcess = new DodgyProcess(mockBus);

            dodgyProcess.Handle(new OrderPaid(new Order()));
            var publishedMessage = mockBus.Messages[0];
            var tolerance = TimeSpan.FromHours(1);
            var ttl = ((IHaveTtl) publishedMessage).expiry - DateTimeOffset.Now;

            Assert.That(ttl, Is.GreaterThanOrEqualTo(tolerance));
        }
    }
}