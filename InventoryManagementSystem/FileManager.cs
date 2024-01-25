using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InventoryManagementSystem
{
    class FileManager
    {
        private Inventory inventory;

        public FileManager(Inventory inventory)
        {
            this.inventory = inventory;
        }

        /*public void SaveToFileTxt(string path)
        {
            using (StreamWriter writer = new StreamWriter(path + ".txt"))
            {
                foreach (Product product in inventory.GetProducts())
                {
                    writer.WriteLine($"{product.Id},{product.Name},{product.Price},{product.Quantity}");
                }
            }
        }*/

        /*public void SaveToFileBin1(string path)
        {
            using (FileStream file = File.Open(path + ".bin1", FileMode.Create))
            {
                BinaryWriter writer = new BinaryWriter(file);

                foreach (Product product in inventory.GetProducts())
                {
                    writer.Write(product.Id.ToString());
                    writer.Write(product.Name);
                    writer.Write(product.Price);
                    writer.Write(product.Quantity);
                }
            }
        }*/

        /*public void SaveToFileBin2(string path)
        {
            using (FileStream file = File.Open(path + ".bin2", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, new List<Product>(inventory.GetProducts()));
            }
        }*/

        public void SaveToFileJson(string fileName, string filePath)
        {

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string path = Path.Combine(filePath, fileName);



            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(inventory.GetProducts(), options);
            File.WriteAllText(path + ".json", json);
        }

        public void SaveToFileJson(string fileName)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            List<Guitar> guitars = new List<Guitar>();
            List<Drum> drums = new List<Drum>();

            foreach (Product product in inventory.GetProducts())
            {
                if (product is Guitar guitar)
                {
                    guitars.Add(guitar);
                }
                else if (product is Drum drum)
                {
                    drums.Add(drum);
                }
            }

            string guitarsJson = JsonSerializer.Serialize(guitars, options);
            string drumsJson = JsonSerializer.Serialize(drums, options);

            File.WriteAllText(fileName + "_guitars.json", guitarsJson);
            File.WriteAllText(fileName + "_drums.json", drumsJson);
        }

        /*public void LoadFromFileTxt(string path)
        {
            inventory.Clear();

            using (StreamReader reader = new StreamReader(path + ".txt"))
            {
                while (!reader.EndOfStream)
                {
                    string[] parts = reader.ReadLine().Split(',');

                    if (parts.Length == 4)
                    {
                        Guid id = Guid.Parse(parts[0]);
                        string name = parts[1];
                        decimal price = decimal.Parse(parts[2]);
                        int quantity = int.Parse(parts[3]);

                        Product product = new Product(id, name, price, quantity);
                        inventory.AddProduct(product);
                    }
                }
            }
        }*/

        /*public void LoadFromFileBin1(string path)
        {
            inventory.Clear();

            using (FileStream file = File.Open(path + ".bin1", FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(file);

                while (file.Position < file.Length)
                {
                    Guid id = Guid.Parse(reader.ReadString());
                    string name = reader.ReadString();
                    decimal price = reader.ReadDecimal();
                    int quantity = reader.ReadInt32();

                    Product product = new Product(id, name, price, quantity);
                    inventory.AddProduct(product);
                }
            }
        }*/

        /*public void LoadFromFileBin2(string path)
        {
            inventory.Clear();

            using (FileStream file = File.Open(path + ".bin2", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                List<Product> products = formatter.Deserialize(file) as List<Product>;
                foreach (Product product in products)
                {
                    inventory.AddProduct(product);
                }
            }
        }*/

        public void LoadFromFileJson(string path)
        {
            inventory.Clear();

            string guitarsJson = File.ReadAllText(path + "_guitars.json");
            List<Guitar> guitars = JsonSerializer.Deserialize<List<Guitar>>(guitarsJson);

            string drumsJson = File.ReadAllText(path + "_drums.json");
            List<Drum> drums = JsonSerializer.Deserialize<List<Drum>>(drumsJson);

            foreach (Guitar guitar in guitars)
            {
                inventory.AddProduct(guitar);
            }

            foreach (Drum drum in drums)
            {
                inventory.AddProduct(drum);
            }
        }
    }
}
