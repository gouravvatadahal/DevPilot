namespace DevPilot.BDD.C.Tests.Interfaces;

public interface IFeaturesService
{
    Task<FeaturesResult> GetFeaturesPage();
}

public class FeaturesResult
{
    public bool Success { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<string> Features { get; set; } = new();
    public string? ErrorMessage { get; set; }
} 