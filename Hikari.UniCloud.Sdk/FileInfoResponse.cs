
using System.Text.Json.Serialization;

namespace Hikari.UniCloud.Sdk;

public class FileInfoResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("cdnDomain")]
    public string CdnDomain { get; set; }

    [JsonPropertyName("signature")]
    public string Signature { get; set; }
    [JsonPropertyName("policy")]
    public string Policy { get; set; }
    [JsonPropertyName("accessKeyId")]
    public string AccessKeyId { get; set; }
    [JsonPropertyName("ossPath")]
    public string OssPath { get; set; }
    [JsonPropertyName("host")]
    public string Host { get; set; }
}