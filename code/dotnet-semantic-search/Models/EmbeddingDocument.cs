using Newtonsoft.Json;

namespace MicrosoftExtensionsAiSample.Models;

public class EmbeddingDocument
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    [JsonProperty("inputText")]
    public string InputText { get; set; } = string.Empty;

    [JsonProperty("vector")]
    public float[] Vector { get; set; } = Array.Empty<float>();

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonProperty("_ts")]
    public long Timestamp { get; set; }
}