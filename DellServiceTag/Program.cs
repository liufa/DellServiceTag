using DellServiceTagData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DellServiceTag
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var getDellAsset = new ApiCaller().GetDellAsset("asdasd", "asdadadasd");
                new DatabaseWritter().CreateDellAsset(new DellAsset
                {
                    CountryLookupCode = "CountryLookupCode",
                    CustomerNumber = "CustomerNumber",
                    IsDuplicate = false,
                    ItemClassCode = "ItemClassCode",
                    LocalChannel = "LocalChannel",
                    MachineDescription = "MachineDescription",
                    OrderNumber = "OrderNumber",
                    ParentServiceTag = "ParentServiceTag",
                    ServiceTag = "ServiceTag",
                    ShipDate = "ShipDate"
                });
            }
            catch (Exception e) {
                File.AppendAllLines("log.txt", new[] { Environment.NewLine, DateTime.Now.ToString(), e.Message, e.StackTrace });
            }
        }
    }
}
