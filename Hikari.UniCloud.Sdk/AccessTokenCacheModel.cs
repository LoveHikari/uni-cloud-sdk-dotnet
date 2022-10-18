using System.Text.Json.Serialization;

namespace ConsoleApp1.UniCloud
{
    public class AccessTokenCacheModel
    {
        /// <summary>
        /// Token值
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiresTime { get; set; } = DateTime.Now.AddSeconds(7200);
    }
}