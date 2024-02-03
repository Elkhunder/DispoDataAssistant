using System;
using System.Text.Json.Serialization;
namespace DispoDataAssistant.Data.Models;

public class ServiceNowAsset
{
    public int Id { get; set; }
    [JsonPropertyName("sys_id")]
    public string? SysId { get; set; }

    [JsonPropertyName("parent")]
    public string? Parent { get; set; }

    public virtual TabModel Tab { get; set; } = new TabModel();

    public int TabId { get; set; }

    [JsonPropertyName("asset_tag")]
    public string? AssetTag { get; set; }

    [JsonPropertyName("model.manufacturer.name")]
    public string? Manufacturer { get; set; }

    [JsonPropertyName("model.name")]
    public string? Model { get; set; }

    [JsonPropertyName("model_category.name")]
    public string? Category { get; set; }

    [JsonPropertyName("serial_number")]
    public string? SerialNumber { get; set; }

    [JsonPropertyName("life_cycle_stage.name")]
    public string? LifeCycleStage { get; set; }

    [JsonPropertyName("life_cycle_stage_status.name")]
    public string? LifeCycleStatus { get; set; }

    [JsonPropertyName("install_status")]
    public string? State { get; set; }

    [JsonPropertyName("substatus")]
    public string? Substate { get; set; }

    [JsonPropertyName("sys_updated_on")]
    public string? LastUpdated { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not null && obj is ServiceNowAsset other)
        {
            return SysId == other.SysId
                && AssetTag == other.AssetTag
                && SerialNumber == other.SerialNumber;
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(SysId);
        hash.Add(AssetTag);
        hash.Add(SerialNumber);
        return hash.ToHashCode();
    }
}

