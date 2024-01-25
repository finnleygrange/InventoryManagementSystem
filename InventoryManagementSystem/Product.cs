using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryManagementSystem
{
    [Serializable]
    abstract class Product
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public decimal Price { get; protected set; }
        public int Quantity { get; protected set; }
        public string Type { get; set; }

        public Product(string name, decimal price, int quantity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public Product(Product other)
        {
            Id = other.Id;
            Name = other.Name;
            Price = other.Price;
            Quantity = other.Quantity;
        }

        [JsonConstructor]
        public Product(Guid id, string name, decimal price, int quantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}
