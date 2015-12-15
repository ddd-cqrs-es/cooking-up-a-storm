namespace cqrs_documents.Commands
{
    internal class CookFood : Message
    {
        public readonly Order Order;

        public CookFood(Order order)
        {
            Order = order;
        }
    }
}