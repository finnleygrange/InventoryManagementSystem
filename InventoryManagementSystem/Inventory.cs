using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem
{
    class Inventory
    {
        Dictionary<Guid, Product> products;

        public Inventory()
        {
            products = new Dictionary<Guid, Product>();
        }

        public Inventory(int size)
        {
            products = new Dictionary<Guid, Product>(size);
        }

        public Inventory(Inventory other)
        {
            products = new Dictionary<Guid, Product>(other.products);
        }

        public List<Product> GetProducts()
        {
            return new List<Product>(products.Values);
        }

        public bool AddProduct(Product product)
        {
            if (product == null)
            {
                return false;
            }

            products.Add(product.Id, product);
            return true;
        }

        public bool RemoveProduct(Guid productId)
        {
            if (!products.ContainsKey(productId))
            {
                return false;
            }

            products.Remove(productId);
            return true;
        }

        public bool RemoveProduct(Product product)
        {
            if (product == null || !products.ContainsKey(product.Id))
            {
                return false;
            }

            products.Remove(product.Id);
            return true;
        }
        public void Clear()
        {
            products.Clear();
        }
    }
}
