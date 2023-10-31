using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace DispoDataAssistant.Models
{
    public class ServiceNowResponse
    {
        [JsonPropertyName("result")]
        public List<ServiceNowAsset> Assets { get; set; }
    }
    public class ServiceNowAsset
    {
        [JsonPropertyName("asset_tag")]
        public string AssetTag { get; set; }

        [JsonPropertyName("model.manufacturer.name")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("model.name")]
        public string Model { get; set; }

        [JsonPropertyName("model_category.name")]
        public string Category { get; set; }

        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; }

    }
}