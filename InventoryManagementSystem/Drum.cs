using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryManagementSystem
{
    class Drum : Product, IInstrument
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public DrumType DrumType { get; set; }

        public Drum(string name, string brand, string model, decimal price, int quantity, DrumType drumType) : base(name, price, quantity)
        {
            Brand = brand;
            Model = model;
            DrumType = drumType;
            Type = "Drum";
        }

        [JsonConstructor]
        public Drum(Guid id, string name, string brand, string model, decimal price, int quantity, DrumType drumType) : base(id, name, price, quantity)
        {
            Brand = brand;
            Model = model;
            DrumType = drumType;
            Type = "Drum";
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Brand: {Brand}, Model: {Model}, Price: {Price:C}, Quantity: {Quantity}, DrumType: {DrumType}";
        }
    }
}
