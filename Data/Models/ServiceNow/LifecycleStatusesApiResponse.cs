using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DispoDataAssistant.Data.Models.ServiceNow
{
    public class LifecycleStatusesApiResponse
    {
        [JsonPropertyName("result")]
        public List<LifecycleItem>? LifecycleStatuses { get; set; }
    }
}
