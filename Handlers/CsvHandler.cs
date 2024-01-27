using DispoDataAssistant.Data.Models;
using System;
using System.Collections.Generic;

namespace DispoDataAssistant.Handlers;

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

    public string BuildCsvHeader(string delimiter)
    {
        return string.Join(delimiter, _csvHeader) + Environment.NewLine;
    }

    public string ConvertAssetToCsvLine(ServiceNowAsset asset, string delimiter)
    {
        List<string> _details =
        [
            "1",
            "ECOF",
            "0",
            asset.SerialNumber?.ToUpper() ?? "",
            asset.Category?.ToUpper() ?? "",
            asset.AssetTag?.ToUpper() ?? "",
            asset.Manufacturer?.ToUpper() ?? "",
            asset.Model?.ToUpper() ?? "",
            "Not Sanitized",
            "s"
        ];
        string csvLine = string.Join(delimiter, _details);

        return csvLine;
    }
}
