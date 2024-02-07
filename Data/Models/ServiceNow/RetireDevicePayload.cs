using System.Text.Json.Serialization;

namespace DispoDataAssistant.Data.Models.ServiceNow;

public class RetireDevicePayload
{
    [JsonPropertyName("install_status")]
    public string InstallStatus { get; set; }

    [JsonPropertyName("substatus")]
    public string Substatus { get; set; }

    [JsonPropertyName("life_cycle_stage")]
    public string LifecycleStage { get; set; }

    [JsonPropertyName("life_cycle_stage_status")]
    public string LifecycleStatus { get; set; }

    [JsonPropertyName("parent")]
    public string? Parent { get; set; }
}
