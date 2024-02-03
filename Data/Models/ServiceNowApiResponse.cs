using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DispoDataAssistant.Data.Models
{
    public class ServiceNowApiResponse
    {
        public ServiceNowApiResponse() { }

        public ServiceNowApiResponse(bool isSuccessful, List<ServiceNowAsset> assets)
        {
            this.IsSuccessful = isSuccessful;
            this.Assets = assets;
        }

        public bool IsSuccessful { get; set; }

        [JsonPropertyName("result")]
        public List<ServiceNowAsset> Assets { get; set; } = [];
    }
}
