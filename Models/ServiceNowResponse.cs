using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DispoDataAssistant.Models
{
    public class ServiceNowResponse
    {
        [JsonPropertyName("result")]
        public List<ServiceNowAsset>? Assets { get; set; }
    }
}
