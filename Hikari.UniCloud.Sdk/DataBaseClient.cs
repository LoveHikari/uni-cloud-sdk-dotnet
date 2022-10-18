using Hikari.Common;
using Hikari.Common.Net.Http;
using Hikari.Common.Security;
using System.Linq;
using System.Text.Json;

namespace ConsoleApp1.UniCloud
{
    /// <summary>
    /// 访问云数据基类
    /// </summary>
    public class DataBaseClient
    {
        private readonly string _spaceId;
        private readonly string _clientSecret;
        private readonly HttpClientHelper _httpClient;
        public DataBaseClient(string spaceId, string clientSecret)
        {
            this._spaceId = spaceId;
            this._clientSecret = clientSecret;
            _httpClient = new HttpClientHelper();
        }

        /// <summary>
        /// 小程序云数据库查询方法
        /// </summary>
        /// <param name="collectionName">数据库名</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync<T>(string collectionName, QueryParameter query)
        {
            var accessToken = await GetAccessTokenAsync();
            var fun = """
                        {
            	"functionTarget": "DCloud-clientDB",
            	"functionArgs": {
            		"command": {
            			"$db": [{
            				"$method": "collection",
            				"$param": ["$collectionName"]
            			}, $where $field {
            				"$method": "skip",
            				"$param": [$skip]
            			}, $limit {
            	            "$method": "get",
            	            "$param": [{"getCount":true}]
                        }]
            		},
            		"uniIdToken": ""
            	}
            }
            """;
            string whereStr = "";
            if (!string.IsNullOrWhiteSpace(query.Where))
            {
                whereStr = "{\"$method\":\"where\",\"$param\":[" + query.Where + "]},";
            }
            string fieldStr = "";
            if (!string.IsNullOrWhiteSpace(query.Field))
            {
                fieldStr = "{\"$method\":\"field\",\"$param\":[" + query.Field + "]},";
            }

            string limitStr = "";
            if (query.Limit > 0)
            {
                limitStr = "{\"$method\":\"limit\",\"$param\":[" + query.Limit + "]},";
            }
            fun = fun.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");
            fun = fun.Replace("$collectionName", collectionName).Replace("$where", whereStr).Replace("$skip", query.Skip.ToString()).Replace("$field", fieldStr).Replace("$limit", limitStr);
            var data = new Dictionary<string, object>()
            {
                { "method", "serverless.function.runtime.invoke" },
                { "params",  fun},
                { "spaceId", this._spaceId },
                { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() },
                {"token", accessToken}
            };

            string url = "https://api.bspapp.com/client";

            var headerItem = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "x-serverless-sign", Sign(data, this._clientSecret) },
                {"x-basement-token", data["token"].ToString()}
            }
                ;
            var json = await _httpClient.PostAsync(url, data, headerItem: headerItem);
            var jo = System.Text.Json.JsonDocument.Parse(json);
            bool success = jo.RootElement.GetProperty("success").GetBoolean();
            var resData = new List<T>();
            if (success)
            {
                var res = jo.RootElement.GetProperty("data").GetProperty("data").EnumerateArray();

                foreach (JsonElement je in res)
                {
                    resData.Add(JsonSerializer.Deserialize<T>(je));
                }
            }


            return resData;
        }

        /// <summary>
        /// 小程序云数据库添加方法
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResponseBase> AddAsync(string collectionName, AddParameter param)
        {
            var accessToken = await GetAccessTokenAsync();
            var fun = """
                        {
            	"functionTarget": "DCloud-clientDB",
            	"functionArgs": {
            		"command": {
            			"$db": [{
            				"$method": "collection",
            				"$param": ["$collectionName"]
            			}, {
            	            "$method": "add",
            	            "$param": [$data]
                        }]
            		},
            		"uniIdToken": ""
            	}
            }
            """;
            fun = fun.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");
            string data = JsonSerializer.Serialize(param.Data);
            fun = fun.Replace("$data", data);


            var p = new Dictionary<string, object>()
            {
                { "method", "serverless.function.runtime.invoke" },
                { "params",  fun},
                { "spaceId", this._spaceId },
                { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() },
                {"token", accessToken}
            };

            string url = "https://api.bspapp.com/client";

            var headerItem = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" },
                    { "x-serverless-sign", Sign(p, this._clientSecret) },
                    {"x-basement-token", p["token"].ToString()}
                }
                ;
            var json = await _httpClient.PostAsync(url, p, headerItem: headerItem);
            var jo = System.Text.Json.JsonDocument.Parse(json);
            bool success = jo.RootElement.GetProperty("success").GetBoolean();
            var resData = new List<T>();
            if (success)
            {
                var res = jo.RootElement.GetProperty("data").GetProperty("data").EnumerateArray();

            }

            return resultData;
        }

        ///// <summary>
        ///// 小程序云数据库更新方法
        ///// </summary>
        ///// <param name="dateBaseName"></param>
        ///// <param name="update"></param>
        ///// <returns></returns>
        //public async Task<ResponseBase> UpdateAsync(UpdateParameter update)
        //{
        //    //小程序云数据库查询接口API地址
        //    string url = $"https://api.weixin.qq.com/tcb/databaseupdate?access_token={AccessTokenInit()}";
        //    //小程序云数据库查询接口参数
        //    UpdateParameter queryClass = new UpdateParameter();

        //    string updateString = $"db.collection(\"{update.TableName}\").where({update.Where}).update({{data: {update.Data}}})";

        //    var queryBase = new Dictionary<string, object>()
        //    {
        //        {"env", _env},
        //        {"query", updateString.Replace("\n", "").Replace("\r", "")}
        //    };
        //    string json = await _httpClient.PostAsync(url, queryBase);
        //    ResponseBase resultData = System.Text.Json.JsonSerializer.Deserialize<ResponseBase>(json);
        //    return resultData;
        //}

        ///// <summary>
        ///// 小程序云数据库删除方法
        ///// </summary>
        ///// <param name="dateBaseName"></param>
        ///// <param name="update"></param>
        ///// <returns>删除记录数量</returns>
        //public async Task<int> Delete(DeleteParameter param)
        //{
        //    //小程序云数据库查询接口API地址
        //    string url = $"https://api.weixin.qq.com/tcb/databasedelete?access_token={AccessTokenInit()}";

        //    string updateString = $"db.collection(\"{param.TableName}\").where({param.Where}).remove()";

        //    var queryBase = new Dictionary<string, object>()
        //    {
        //        {"env", _env},
        //        {"query", updateString.Replace("\n", "").Replace("\r", "")}
        //    };
        //    string json = await _httpClient.PostAsync(url, queryBase);
        //    ResponseBase resultData = System.Text.Json.JsonSerializer.Deserialize<ResponseBase>(json);
        //    return resultData.Deleted;
        //}



        /// <summary>
        /// 获取接口调用凭证
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAccessTokenAsync()
        {
            var data = new Dictionary<string, object>()
            {
                { "method", "serverless.auth.user.anonymousAuthorize" },
                { "params", "{}" },
                { "spaceId", this._spaceId },
                { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() }
            };

            string url = "https://api.bspapp.com/client";

            HttpClientHelper httpClientHelper = new HttpClientHelper();
            var headerItem = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" },
                    { "x-serverless-sign", Sign(data, this._clientSecret) },
                }
                ;
            var html = await httpClientHelper.PostAsync(url, data, headerItem: headerItem);
            var jo = System.Text.Json.JsonDocument.Parse(html);
            bool success = jo.RootElement.GetProperty("success").GetBoolean();
            if (success)
            {
                return jo.RootElement.GetProperty("data").GetProperty("accessToken").GetString() ?? "";
            }

            return "";
        }

        private string Sign(IDictionary<string, object> dic, string key)
        {
            var s = dic.OrderBy(x => x.Key).Select(x => x.Key + "=" + x.Value);
            var str = string.Join('&', s);
            var md5Value = new SecureHelper(str).HmacMd5(key).DigestHex();
            return md5Value;
        }
    }
}