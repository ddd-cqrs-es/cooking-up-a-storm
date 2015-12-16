namespace Restaurant.Events
{
    internal class OrderPriced : Message
    {
        public readonly Order Order;

        public OrderPriced(Order order)
        {
            Order = order;
        }
    }
}