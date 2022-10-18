using System.Text.Json.Serialization;

namespace ConsoleApp1;

public class Class2
{
    [JsonPropertyName("_id")]
    public string Id { get; set; }
}