namespace Restaurant.Events
{
    internal class CookFailed : Message
    {
        public readonly Order Order;

        public CookFailed(Order order)
        {
            Order = order;
        }
    }
}