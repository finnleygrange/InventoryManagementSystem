using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryManagementSystem
{
    class Guitar : Product, IInstrument
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int NumberOfStrings { get; set; }

        public Guitar(string name, string brand, string model, decimal price, int quantity, int numberOfStrings) : base(name, price, quantity)
        {
            Brand = brand;
            Model = model;
            NumberOfStrings = numberOfStrings;
            Type = "Guitar";
        }

        [JsonConstructor]
        public Guitar(Guid id, string name, string brand, string model, decimal price, int quantity, int numberOfStrings) : base(id, name, price, quantity)
        {
            Brand = brand;
            Model = model;
            NumberOfStrings = numberOfStrings;
            Type = "Guitar";
        }


        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Brand: {Brand}, Model: {Model}, Price: {Price:C}, Quantity: {Quantity}, NumberOfStrings: {NumberOfStrings}";
        }
    }
}
