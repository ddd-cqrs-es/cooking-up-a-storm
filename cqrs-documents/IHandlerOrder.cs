namespace cqrs_documents
{
    public interface IHandlerOrder
    {
        void Handle(Order order);
    }
}