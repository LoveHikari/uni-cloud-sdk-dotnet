using System.Text.Json;
using System.Text.Json.Serialization;
using Hikari.Common;

namespace TestProject2;

/// <summary>
/// 建筑表
/// </summary>
public sealed class UBuilding
{
    #region Model
    /// <summary>
    /// id
    /// </summary>
    [JsonPropertyName("_id")]
    public string Id { get; set; }
    /// <summary>
    /// 建筑面积
    /// </summary>
    [JsonPropertyName("area_building")]
    [JsonConverter(typeof(InfoToDecimalConverter))]
    public decimal AreaBuilding { get; set; }
    /// <summary>
    /// 0自持，1出租，2出售
    /// </summary>
    [JsonPropertyName("status")]
    [JsonConverter(typeof(InfoToIntConverter))]
    public int Status { get; set; }
    /// <summary>
    /// 建筑状态，0无，1意向，2预订，3入驻
    /// </summary>
    [JsonPropertyName("rentSate")]
    public int RentSate { get; set; }
    /// <summary>
    /// 父id
    /// </summary>
    [JsonPropertyName("parentId")]
    public string? ParentId { get; set; }
    /// <summary>
    /// 园区id
    /// </summary>
    [JsonPropertyName("parkId")]
    public string ParkId { get; set; }
    /// <summary>
    /// 建筑分类，0无，1建筑空间，2室外空间，3虚拟空间
    /// </summary>
    [JsonPropertyName("sort")]
    [JsonConverter(typeof(InfoToIntConverter))]
    public int Sort { get; set; }

    #endregion Model
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
    public class InfoToIntConverter : JsonConverter<int>
    {
        public override int Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            return jsonDoc.RootElement.GetRawText().ToInt32();
        }

        public override void Write(
            Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

