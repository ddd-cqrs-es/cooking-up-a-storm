namespace cqrs_documents.Events
{
    internal class OrderPaid : Message
    {
        public readonly Order Order;

        public OrderPaid(Order order)
        {
            Order = order;
        }
    }
}