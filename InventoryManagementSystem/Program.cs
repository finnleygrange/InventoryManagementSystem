namespace InventoryManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Inventory inventory = new Inventory();
            FileManager fileManager = new FileManager(inventory);
            CommandLineInterface commandLineInterface = new CommandLineInterface(inventory);

            fileManager.LoadFromFileJson("inventory");
            commandLineInterface.ExecuteCommand(args);
            fileManager.SaveToFileJson("inventory");
        }
    }
}