using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DispoDataAssistant.Data.Models.ServiceNow
{
    public class LifecycleItem
    {
        [JsonPropertyName("sys_id")]
        public string? SysId { get; set; }
        [JsonPropertyName("sys_updated_by")]
        public string? UpdatedBy { get; set; }
        [JsonPropertyName("sys_created_on")]
        public string? CreatedOn { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("sys_mod_count")]
        public string? ModCount { get; set; }
        [JsonPropertyName("sys_updated_on")]
        public string? UpdatedOn { get; set; }
        [JsonPropertyName("sys_tags")]
        public string? Tags { get; set; }
        [JsonPropertyName("sys_created_by")]
        public string? CreatedBy { get; set; }
    }
}
