using System.Text.Json;
using System.Text.Json.Serialization;
using Hikari.Common;

namespace TestProject2;
/// <summary>
/// 园区表
/// </summary>
public sealed class UPark
{
    #region Model

    /// <summary>
    /// id
    /// </summary>
    [JsonPropertyName("_id")]
    public string Id { get; set; }
    /// <summary>
    /// 园区名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
    /// <summary>
    /// 园区简称
    /// </summary>
    [JsonPropertyName("shortName")]
    public string ShortName { get; set; }
    /// <summary>
    /// 用地面积/亩
    /// </summary>
    [JsonPropertyName("area")]
    [JsonConverter(typeof(InfoToDecimalConverter))]
    public decimal Area { get; set; }
    /// <summary>
    /// 建筑面积/m²
    /// </summary>
    [JsonPropertyName("buildingArea")]
    [JsonConverter(typeof(InfoToDecimalConverter))]
    public decimal BuildingArea { get; set; }
    /// <summary>
    /// 租赁去化
    /// </summary>
    [JsonPropertyName("leaseRate")]
    [JsonConverter(typeof(InfoToDecimalConverter))]
    public decimal LeaseRate { get; set; }
    /// <summary>
    /// 销售去化
    /// </summary>
    [JsonPropertyName("industryRate")]
    [JsonConverter(typeof(InfoToDecimalConverter))]
    public decimal IndustryRate { get; set; }
    #endregion
    public class InfoToDecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            return jsonDoc.RootElement.GetRawText().ToDecimal();
        }

        public override void Write(
            Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}