using System.Text.Json.Serialization;
namespace DispoDataAssistant.Models
{
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