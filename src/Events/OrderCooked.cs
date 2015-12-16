namespace Restaurant.Events
{
    internal class OrderCooked : Message
    {
        public readonly Order Order;

        public OrderCooked(Order order)
        {
            Order = order;
        }
    }
}