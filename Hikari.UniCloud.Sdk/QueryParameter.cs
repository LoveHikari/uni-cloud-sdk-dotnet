namespace Hikari.UniCloud.Sdk
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class QueryParameter
    {
        /// <summary>
        /// 条件
        /// </summary>
        public string? Where { get; set; }
        /// <summary>
        /// 前几条
        /// </summary>
        public int Limit { get; set; } = 100000;
        /// <summary>
        /// 跳过几条
        /// </summary>
        public int Skip { get; set; } = 0;
        /// <summary>
        /// 查询字段
        /// </summary>
        public string? Field { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string? OrderBy { get; set; }
    }
}