using System;
using System.Collections.Generic;

namespace DispoDataAssistant.Data.Models
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

            DeviceTypes =
            [
                "CPU",
                "Laptop",
                "Printer",
                "Monitor",
                "Server",
                "Other",
                "Tablet"
            ];

            DeviceModels =
            [
                "600",
                "Z440",
                "1040",
                "840",
                "800",
                "400"
            ];

            DeviceManufacturers =
            [
                "HP",
                "Dell",
                "Samsug",
                "Zebra",
                "Fujitsu",
                "Apple",
                "Microsoft",
                "IBM",
                "Cisco"
            ];

            PickupLocations =
            [
                "777",
                "AL",
                "NCRC",
                "NIB",
                "UHS",
                "TC",
                "THSL",
            ];
        }
    }
}
