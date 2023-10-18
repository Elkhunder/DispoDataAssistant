using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Models
{
    public class DeviceInformation
    {
        public List<string>? DeviceTypes { get; set; }
        public List<string>? DeviceModels { get; set; }
        public List<string>? DeviceManufacturers { get; set; }
        public List<string>? PickupLocations { get; set; }
        public DeviceInformation()
        {
            Console.WriteLine("Device Information: Instance Created");

            DeviceTypes = new List<string>
            {
                "CPU",
                "Laptop",
                "Printer",
                "Monitor",
                "Server",
                "Other",
                "Tablet"
            };

            DeviceModels = new List<string>
            {
                "600",
                "Z440",
                "1040",
                "840",
                "800",
                "400"
            };

            DeviceManufacturers = new List<string>
            {
                "HP",
                "Dell",
                "Samsug",
                "Zebra",
                "Fujitsu",
                "Apple",
                "Microsoft",
                "IBM",
                "Cisco"
            };

            PickupLocations = new List<string>
            {
                "777",
                "AL",
                "NCRC",
                "NIB",
                "UHS",
                "TC",
                "THSL",
            };
        }
    }
}
