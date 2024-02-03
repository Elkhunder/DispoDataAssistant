using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DispoDataAssistant.Data.Models.ServiceNow;

public class LifecycleStageApiResponse
{
    [JsonPropertyName("result")]
    public List<LifecycleItem>? LifecycleStages { get; set; }


}
