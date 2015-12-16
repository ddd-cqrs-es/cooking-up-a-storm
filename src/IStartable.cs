namespace Restaurant
{
    public interface IStartable
    {
        string Name { get; }
        int Count { get; }

        void StartListening();

        void Stop();
    }
}