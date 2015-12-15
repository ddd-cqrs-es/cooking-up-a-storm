using cqrs_documents.Events;

namespace cqrs_documents
{
    public interface IHandle<in T> where T : Message
    {
        void Handle(T message);
    }
}