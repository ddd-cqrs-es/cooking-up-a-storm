using System.Collections.Generic;

namespace Restaurant
{
    public class Order
    {
        public int tableNumber { get; set; }
        public List<string> ingredients { get; set; } = new List<string>();
        public List<LineItem> lineItems { get; set; } = new List<LineItem>();
        public double subTotal { get; set; }
        public double tax { get; set; }
        public double total { get; set; }
        public bool paid { get; set; }
        public int timeToCook { get; set; }
        public string paymentMethod { get; set; }
        public bool dodgyCustomer { get; set; }
    }
}