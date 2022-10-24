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
        public string Where { get; set; }
        public int Limit { get; set; } = 100000;
        public int Skip { get; set; } = 0;
        public string Field { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string OrderBy { get; set; }
    }
}