using System;
using cqrs_documents.Events;
using NUnit.Framework;

namespace cqrs_documents.Specifications
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

    [TestFixture]
    public class when_document_created
    {
        private readonly string _json = @"{
    tableNumber : 12,
    ingredients : ['foo','bar','baz'],
    lineItem : [
        {
            text : 'razor blade pizza',
            qty : 4,
            price : 9.99
        }
    ],
    subTotal : 9.99,
    tax : 1.99,
    total : 11.98,
    paid : false,
    timeToCook: 300,
    paymentMethod : 'card' 
}";

        [Test]
        public void ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => new JsonOrder(_json));
        }

        [Test]
        public void ShouldReadTax()
        {
            var doc = new JsonOrder(_json);

            var tax = doc.tax;

            Assert.That(tax, Is.EqualTo(1.99));
        }

        [Test]
        public void ShouldSetTax()
        {
            var doc = new JsonOrder(_json) {tax = 10};

            Assert.That(doc.tax, Is.EqualTo(10));
        }

        [Test]
        public void ShouldReadPaymentMethod()
        {
            var doc = new JsonOrder(_json);

            var paymentMethod = doc.paymentMethod;

            Assert.That(paymentMethod, Is.EqualTo("card"));
        }

        [Test]
        public void ShouldReadIngredients()
        {
        }
    }
}