using System.Text.Json.Serialization;

namespace ConsoleApp1.UniCloud
{
    public class ResponseBase<T>
    {
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public PagerList Pager { get; set; }
        public List<T> Data { get; set; }
        /// <summary>
        /// 删除记录数量
        /// </summary>
        public int Deleted { get; set; }

        public class PagerList
        {
            public int Offset { get; set; }
            public int Limit { get; set; }
            public int Total { get; set; }
        }
    }
}