using System.Collections.Generic;

namespace DispoDataAssistant.Data.Models.Settings;

public class Settings
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public virtual ICollection<Integration>? Integrations { get; set; }
    public virtual ICollection<General>? General { get; set; }
}
