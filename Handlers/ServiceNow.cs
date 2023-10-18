using DispoDataAssistant.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DispoDataAssistant.Handlers
{
    public class ServiceNowHandler
    {
        private HttpClient _client;
        public ServiceNowHandler()
        {
            _client = new HttpClient();

            string ServiceNowUser = "";
            string ServiceNowPass = "";

            var creds = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ServiceNowUser}:{ServiceNowPass}"));

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", creds);
        }   

        public async Task<ServiceNowAsset?> GetServiceNowAssetAsync(string assetTag)
        {
            HttpResponseMessage response = await _client.GetAsync($"https://ummeddev.service-now.com/api/now/table/alm_hardware?sysparm_query=asset_tag={assetTag}&sysparm_fields=asset_tag,model.name,model.manufacturer.name,serial_number,model_category.name");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(responseBody))
            {
                ServiceNowAsset[] assets = JsonSerializer.Deserialize<ServiceNowAsset[]>(JsonDocument.Parse(responseBody).RootElement.GetProperty("result").GetRawText());
                
                if (assets is not null && assets.Length > 0)
                {
                    return assets[0];
                }
            }
            return null;
        }
    }
}