namespace cqrs_documents.Commands
{
    internal class TakePayment : Message
    {
        public readonly Order Order;

        public TakePayment(Order order)
        {
            Order = order;
        }
    }
}