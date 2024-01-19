namespace DispoDataAssistant.Data.Models.Settings;

public class Integration
{
    public int Id { get; set; }
    public string Title { get; set; } = "Integrations";
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public virtual Settings? Settings { get; set; }
}
