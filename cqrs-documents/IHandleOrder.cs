namespace cqrs_documents
{
    public interface IHandleOrder
    {
        void Handle(Order order);
    }
}