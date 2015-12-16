namespace cqrs_documents.Actors
{
    internal class Dish
    {
        public Dish(string description, double price)
        {
            Description = description;
            Price = price;
        }

        public string Description { get; private set; }
        public double Price { get; private set; }
    }
}