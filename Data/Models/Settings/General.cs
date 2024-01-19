namespace DispoDataAssistant.Data.Models.Settings;

public class General
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Value { get; set; }
    public bool Type { get; set; }
    public virtual Settings? Settings { get; set; }
}
