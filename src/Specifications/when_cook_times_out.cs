using System.Linq;
using NUnit.Framework;
using Restaurant.Commands;
using Restaurant.Events;

namespace Restaurant.Specifications
{
    [TestFixture]
    public class when_cook_times_out
    {
        [Test]
        public void should_retry_twice()
        {
            var bus = new MockBus();

            var londonProcess = new LondonProcess(bus);
            var order = new Order();
            londonProcess.Handle(new OrderPlaced(order));
            londonProcess.Handle(new CookOrderTimedOut(order));
            londonProcess.Handle(new CookOrderTimedOut(order));

            var count = bus.Messages.OfType<CookFood>().Count();

            Assert.That(count, Is.EqualTo(3));
        }
    }
}