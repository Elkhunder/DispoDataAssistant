using DispoDataAssistant.Models;
using System;
using System.Collections.Generic;

namespace DispoDataAssistant.Handlers
{
    public class CsvHandler
    {
        private readonly string[] _csvHeader =
        {
            "Quantity",
            "Category",
            "Filler (Not used)",
            "Serial Number",
            "Description/Name",
            "Additional Desc/Service Tag",
            "Manufacturer",
            "Model",
            "Sanitization Request",
            "Condition",
            "Age",
            "Unit Cost"
        };

        public string BuildCsvHeader()
        {
            return string.Join("\t", _csvHeader) + Environment.NewLine;
        }

        public string ConvertDeviceDetailsToCsvLine(DeviceDetails deviceDetails)
        {
            int numberOfProperties = deviceDetails.Length;
            List<string> _details = new List<string>
            {
                "1",
                "ECOF",
                "0",
                deviceDetails.SerialNumber?.ToUpper() ?? "",
                deviceDetails.DeviceType?.ToUpper() ?? "",
                deviceDetails.AssetTag?.ToUpper() ?? "",
                deviceDetails.DeviceManufacturer?.ToUpper() ?? "",
                deviceDetails.DeviceModel?.ToUpper() ?? "",
                "Not Sanitized",
                "s"
            };
            string csvLine = string.Join("\t", _details);

            return csvLine;
        }
    }
}
