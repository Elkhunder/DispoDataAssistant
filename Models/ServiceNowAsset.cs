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
        [JsonPropertyName("sys_id")]
        public string? Id { get; set; }

        [JsonPropertyName("asset_tag")]
        public string? AssetTag { get; set; }

        [JsonPropertyName("manufacturer.name")]
        public string? Manufacturer { get; set; }

        [JsonPropertyName("model_id.name")]
        public string? Model { get; set; }

        [JsonPropertyName("subcategory")]
        public string? Category { get; set; }

        [JsonPropertyName("serial_number")]
        public string? SerialNumber { get; set; }

        [JsonPropertyName("operational_status")]
        public string? OperationalStatus { get; set; }

        [JsonPropertyName("install_status")]
        public string? InstallStatus { get; set; }

        [JsonPropertyName("sys_updated_on")]
        public string? LastUpdated { get; set; }

    }
}