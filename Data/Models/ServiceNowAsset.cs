using DispoDataAssistant.Data.Models.ServiceNow;
using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
namespace DispoDataAssistant.Data.Models;

public partial class ServiceNowAsset : INotifyPropertyChanged
{
    public int Id { get; set; }

    [JsonPropertyName("sys_id")]
    public string? SysId { get; set; }

    [JsonPropertyName("parent")]
    public string? Parent { get; set; }

    public virtual TabModel Tab { get; set; } = new TabModel();

    public int TabId { get; set; }

    
    private string? _assetTag;

    [JsonPropertyName("asset_tag")]
    public string? AssetTag
    {
        get => _assetTag;
        set
        {
            if (_assetTag != value)
            {
                _assetTag = value;
                OnPropertyChanged(nameof(AssetTag));
            }
        }
    }

    [JsonPropertyName("model.manufacturer.name")]
    public string? Manufacturer { get; set; }

    [JsonPropertyName("model.name")]
    public string? Model { get; set; }

    [JsonPropertyName("model_category.name")]
    public string? Category { get; set; }

    [JsonPropertyName("serial_number")]
    public string? SerialNumber { get; set; }

    private string? _lifeCycleStage;
    private string? _lifeCycleStatus;
    private string? _state;
    private string? _substate;
    private string? _lastUpdated;

    [JsonPropertyName("life_cycle_stage.name")]
    public string? LifeCycleStage
    {
        get => _lifeCycleStage;
        set
        {
            if (_lifeCycleStage != value)
            {
                _lifeCycleStage = value;
                OnPropertyChanged(nameof(LifeCycleStage));
            }
        }
    }
    [JsonPropertyName("life_cycle_stage_status.name")]
    public string? LifeCycleStatus
    {
        get => _lifeCycleStatus;
        set
        {
            if (_lifeCycleStatus != value)
            {
                _lifeCycleStatus = value;
                OnPropertyChanged(nameof(LifeCycleStatus));
            }
        }
    }
    [JsonPropertyName("install_status")]
    public string? State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }
    }
    [JsonPropertyName("substatus")]
    public string? Substate
    {
        get => _substate;
        set
        {
            if (_substate != value)
            {
                _substate = value;
                OnPropertyChanged(nameof(Substate));
            }
        }
    }
    [JsonPropertyName("sys_updated_on")]
    public string? LastUpdated
    {
        get => _lastUpdated;
        set
        {
            if (_lastUpdated != value)
            {
                _lastUpdated = value;
                OnPropertyChanged(nameof(LastUpdated));
            }
        }
    }

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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public bool IsRetired(RetireDevicePayload payload)
    {
        if (payload == null)
        {
            throw new ArgumentNullException(nameof(payload));
        }

        return State == payload.InstallStatus &&
               Substate == payload.Substatus &&
               LifeCycleStage == payload.LifecycleStage &&
               LifeCycleStatus == payload.LifecycleStatus;

    }

    public bool IsComplete()
    {
        return !(string.IsNullOrEmpty(SysId) ||
                 string.IsNullOrEmpty(Manufacturer) ||
                 string.IsNullOrEmpty(Model) ||
                 string.IsNullOrEmpty(Category) ||
                 string.IsNullOrEmpty(SerialNumber) ||
                 string.IsNullOrEmpty(LifeCycleStage) ||
                 string.IsNullOrEmpty(LifeCycleStatus) ||
                 string.IsNullOrEmpty(State) ||
                 string.IsNullOrEmpty(LastUpdated));
    }

    public static ServiceNowAsset Empty()
    {
        return new ServiceNowAsset();
    }
}

