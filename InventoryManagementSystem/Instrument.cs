using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem
{
    interface IInstrument
    {
        string Brand { get; set; }
        string Model { get; set; }
    }
}
