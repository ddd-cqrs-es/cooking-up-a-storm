using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cqrs_documents
{
    public class JsonOrder
    {
        private readonly JObject _json;
        public override string ToString()
        {
            return _json.ToString(Formatting.Indented);
        }

        public JsonOrder(string json)
        {
            _json = JObject.Parse(json);
        }

        private int _tableNumber;
        private double _subTotal;
        private double _tax;
        private double _total;
        private bool _paid;
        private int _timeToCook;
        private string _paymentMethod;

        private T GetValue<T>()
        {
            var stacktrace = new StackTrace();
            var methodName = stacktrace.GetFrame(1).GetMethod().Name
                .Replace("get_", string.Empty)
                .Replace("set_", string.Empty);

            return _json[methodName].Value<T>();
        }
        private void SetValue<T>(T value)
        {
            var stacktrace = new StackTrace();
            var methodName = stacktrace.GetFrame(1).GetMethod().Name
                .Replace("get_", string.Empty)
                .Replace("set_", string.Empty);

            _json[methodName]= JToken.FromObject( value);
        }

        public short tableNumber
        {
            get { return GetValue<short>(); }
            set { _tableNumber = value; }
        }



        public List<string> ingredients { get; set; }
        public List<LineItem> lineItem { get; set; }

        public double subTotal
        {
            get { return GetValue<double>(); }
            set { _subTotal = value; }
        }

        public double tax
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public double total
        {
            get { return GetValue<double>(); }
            set { _total = value; }
        }

        public bool paid
        {
            get { return GetValue<bool>(); }
            set { _paid = value; }
        }

        public int timeToCook
        {
            get { return GetValue<int>(); }
            set { _timeToCook = value; }
        }

        public string paymentMethod
        {
            get { return GetValue<string>(); }
            set { _paymentMethod = value; }
        }

        public void AddItem(string description)
        {
            
        }

        public IEnumerable<LineItem> GetLineItems()
        {
            return Enumerable.Empty<LineItem>();
        }

        public void AddIngredients(List<Tuple<string, double>> list)
        {
            throw new NotImplementedException();
        }
    }
}