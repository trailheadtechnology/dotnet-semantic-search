using Newtonsoft.Json;

namespace MicrosoftExtensionsAiSample.Models;

public class BlogPost
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;
    
    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;
    
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
    
    [JsonProperty("categories")]
    public List<string> Categories { get; set; } = new();
    
    [JsonProperty("vector")]
    public float[] Vector { get; set; } = Array.Empty<float>();
    
    [JsonProperty("combinedText")]
    public string CombinedText { get; set; } = string.Empty;
    
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public void GenerateCombinedText()
    {
        // Combine title, content, and categories for embedding generation
        var categoriesText = Categories.Any() ? $" Categories: {string.Join(", ", Categories)}" : "";
        CombinedText = $"{Title}. {Content}{categoriesText}";
    }
}