namespace CandyLicense.Api.Data.Entities;

public class License
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Owner { get; set; }
    public DateTime? Expires { get; set; }
}