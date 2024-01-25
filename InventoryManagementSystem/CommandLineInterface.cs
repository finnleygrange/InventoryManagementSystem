using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem
{
    class CommandLineInterface
    {
        const string version = "1.0.0";
        Inventory inventory;
        FileManager fileManager;

        public CommandLineInterface(Inventory inventory)
        {
            this.inventory = inventory;
            fileManager = new FileManager(this.inventory);
        }

        public void ExecuteCommand(string[] args)
        {
            if (args.Length == 0)
            {
                PrintError("Error: No argument provided.");
                return;
            }

            string command = args[0].ToLower();
            switch (command)
            {
                case "-h":
                case "--help":
                    if (args.Length == 1)
                    {
                        PrintHelp();
                    }
                    else
                    {
                        PrintError("Error: Unexpected arguments for -h or --help.");
                    }
                    break;
                case "-v":
                case "--version":
                    if (args.Length == 1)
                    {
                        PrintVersion();
                    }
                    else
                    {
                        PrintError("Error: Unexpected arguments for -v or --version.");
                    }
                    break;
                case "-a":
                case "--add":
                    if (args.Length < 2)
                    {
                        PrintError("Error: Unexpected arguments for -a or --add.");
                        return;
                    }

                    string productType = args[1].ToLower();
                    string[] properties = args.Skip(2).ToArray();

                    switch (productType)
                    {
                        case "guitar":
                            if (ValidateProduct(properties, ProductType.Guitar))
                            {
                                AddProduct(properties, ProductType.Guitar);
                                PrintSuccess("Successfully added guitar to the inventory.");
                            }
                            break;
                        case "drum":
                            if (ValidateProduct(properties, ProductType.Drum))
                            {
                                AddProduct(properties, ProductType.Drum);
                                PrintSuccess("Successfully added drum to the inventory.");
                            }
                            break;
                        default:
                            PrintError("Invalid Product Type");
                            break;
                    }
                    break;
                case "-r":
                case "--remove":
                    if (args.Length == 2)
                    {
                        if (Guid.TryParse(args[1], out Guid id))
                        {
                            if (inventory.RemoveProduct(id))
                            {
                                PrintSuccess($"Successfully removed product {id} from the inventory.");
                            }
                            else
                            {
                                PrintError($"Error: Product with ID {id} not found in the inventory.");
                            }
                        }
                        else
                        {
                            PrintError("Error: Invalid format for product ID. Please provide a valid GUID.");
                        }


                    }
                    else
                    {
                        PrintError("Error: Unexpected arguments for -r or --remove.");
                    }
                    break;
                case "-s":
                case "--search":
                    break;
                case "-l":
                case "--list":
                    if (args.Length == 1)
                    {
                        DisplayInventory(inventory.GetProducts());
                    }

                    else if (args.Length == 2)
                    {
                        switch (args[1].ToLower())
                        {
                            case "guitar":
                                DisplayInventory(inventory.GetProducts(), ProductType.Guitar);
                                break;
                            case "drum":
                                DisplayInventory(inventory.GetProducts(), ProductType.Drum);
                                break;
                            default:
                                PrintError("Invalid Product Type");
                                break;
                        }
                    }
                    else
                    {
                        PrintError("Error: Unexpected arguments for -l or --list.");
                    }
                    break;
                case "-i":
                case "--import":
                    if (args.Length == 2)
                    {
                        string path = args[1];
                        fileManager.LoadFromFileJson(path);
                        PrintSuccess($"Successfully imported inventory from '{path}'.");
                    }
                    else
                    {
                        PrintError("Error: Unexpected arguments for -i or --import.");
                    }
                    break;
                case "-e":
                case "--export":
                    if (args.Length == 3)
                    {
                        string fileName = args[1];
                        string path = args[2];
                        fileManager.SaveToFileJson(fileName, path);
                        PrintSuccess($"Successfully exported inventory to '{path}'.");

                    }
                    else
                    {
                        PrintError("Error: Unexpected arguments for -e or --export.");
                    }
                    break;
                default:
                    Console.WriteLine("-h, --help for help.");
                    break;
            }
        }

        private bool AddProduct(string[] properties, ProductType type)
        {
            if (type == ProductType.Guitar)
            {
                const int expectedProperties = 6;
                if (properties.Length != 6)
                {
                    PrintError($"Error: Invalid number of properties for guitar. Expected {expectedProperties}.");
                    return false;
                }

                string name = properties[0];
                string brand = properties[1];
                string model = properties[2];
                decimal price = Convert.ToDecimal(properties[3]);
                int quantity = Convert.ToInt32(properties[4]);
                int numberOfStrings = Convert.ToInt32(properties[5]);


                Guitar guitar = new Guitar(name, brand, model, price, quantity, numberOfStrings);
                inventory.AddProduct(guitar);


                return true;
            }

            if (type == ProductType.Drum)
            {
                const int expectedProperties = 6;
                if (properties.Length != 6)
                {
                    PrintError($"Error: Invalid number of properties for drum. Expected {expectedProperties}.");
                    return false;
                }

                string name = properties[0];
                string brand = properties[1];
                string model = properties[2];
                decimal price = Convert.ToDecimal(properties[3]);
                int quantity = Convert.ToInt32(properties[4]);
                DrumType drumType = (DrumType)Enum.Parse(typeof(DrumType), char.ToUpper(properties[5][0]) + properties[5].Substring(1));


                Drum drum = new Drum(name, brand, model, price, quantity, drumType);
                inventory.AddProduct(drum);


                return true;
            }

            return false;
        }

        private bool ValidateProduct(string[] args, ProductType type)
        {
            try
            {
                if (type == ProductType.Guitar && args.Length != 6)
                {
                    PrintError("Error: Unexpected amount of properties for product type guitar. Expected 6.");
                    return false;
                }

                if (type == ProductType.Drum && args.Length != 6)
                {
                    PrintError("Error: Unexpected amount of properties for product type drum. Expected 6.");
                    return false;
                }



                string productName = args[0];
                if (!ValidateName(productName))
                {
                    return false;
                }

                decimal productPrice = Convert.ToDecimal(args[3]);
                if (!ValidatePrice(productPrice))
                {
                    return false;
                }

                int productQuantity = Convert.ToInt32(args[4]);
                if (!ValidateQuantity(productQuantity))
                {
                    return false;
                }
            }
            catch (FormatException)
            {
                PrintError("Error: Unexpected format error. Please check your input.");
                return false;
            }
            catch (OverflowException)
            {
                PrintError("Error: Overflow exception. Please enter a valid integer value within the allowed range.");
                return false;
            }
            return true;
        }

        private bool ValidateQuantity(int productQuantity)
        {
            try
            {
                const int maxQuantity = 100;

                if (productQuantity < 0)
                {
                    PrintError("Error: Product quantity cannot be negative.");
                    return false;
                }

                if (productQuantity > maxQuantity)
                {
                    PrintError($"Error: Product quantity cannot exceed the maximum of ({maxQuantity}).");
                    return false;
                }
            }
            catch (FormatException)
            {
                PrintError("Error: Invalid format for quantity. Please enter a valid integer value.");
                return false;
            }

            return true;
        }

        private bool ValidatePrice(decimal productPrice)
        {
            try
            {
                const int maxPrice = 10000;

                if (productPrice < 0)
                {
                    PrintError("Error: Product price cannot be negative.");
                    return false;
                }

                if (productPrice > maxPrice)
                {
                    PrintError($"Error: Product price cannot exceed £{maxPrice}.");
                    return false;
                }
            }
            catch (FormatException)
            {
                PrintError("Error: Invalid format for price. Please enter a valid decimal value.");
                return false;
            }
            return true;
        }

        private bool ValidateName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                PrintError("Error: Product name cannot be empty");
                return false;
            }

            const int maxChars = 50;
            if (productName.Length > maxChars)
            {
                PrintError($"Error: Product name cannot exceed {maxChars} characters.");
                return false;
            }

            return true;
        }

        private void DisplayInventory(List<Product> products)
        {
            foreach (Product product in products)
            {
                Console.WriteLine(product);
            }
        }

        private void DisplayInventory(List<Product> products, ProductType typeToDisplay)
        {
            foreach (Product product in products)
            {
                switch (typeToDisplay)
                {
                    case ProductType.Guitar:
                        if (product is Guitar)
                        {
                            Guitar guitar = (Guitar)product;
                            Console.WriteLine(guitar);
                        }
                        break;
                    case ProductType.Drum:
                        if (product is Drum)
                        {
                            Drum drum = (Drum)product;
                            Console.WriteLine(drum);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void PrintHelp()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"\n===================================   HELP   ===================================\n" +
                $"-h, --help  >>> Show help message with usage instructions and available options.\n" +
                "-v , --version >>> Display the version of the MusicStoreInventory tool.\n" +
                "-a, --add >>> Add a new item to the inventory.\n" +
                "-r, --remove >>> Remove an item from the inventory.\n" +
                "-s, --search >>> Search for items in the inventory.\n" +
                "-l, --list >>> Display a list of items in the inventory.\n" +
                "-i, --import >>> Import data into the inventory system.\n" +
                "-e, --export >>> Export the current inventory data to a file.\n" +
                "================================================================================"
                );
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void PrintError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMessage);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void PrintSuccess(string successMessage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(successMessage);
            Console.ForegroundColor = ConsoleColor.White;
        }


        private void PrintVersion()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"\n==========   VERSION   ==========\n" +
                $"InventoryManagementSystem version {version}\n" +
                $"=================================");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
