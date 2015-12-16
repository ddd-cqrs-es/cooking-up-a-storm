namespace cqrs_documents
{
    public interface IStartable
    {
        string Name { get; }
        int Count { get; }

        void StartListening();

        void Stop();
    }
}