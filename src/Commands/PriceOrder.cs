namespace Restaurant.Commands
{
    internal class PriceOrder : Message
    {
        public readonly Order Order;

        public PriceOrder(Order order)
        {
            Order = order;
        }
    }
}