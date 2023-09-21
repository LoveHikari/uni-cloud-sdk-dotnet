using System.Text.Json.Serialization;

namespace TestProject2;

public class Class1
{
    [JsonPropertyName("_id")]
    public string Id { get; set; }
}