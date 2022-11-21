using System.Text.Json;
using Hikari.Common;
using Hikari.Common.Net.Http;
using Hikari.Common.Security;
using Hikari.Common.Text.Json;

namespace Hikari.UniCloud.Sdk
{
    /// <summary>
    /// 访问云数据基类
    /// </summary>
    public class DataBaseClient
    {
        private readonly string _spaceId;
        private readonly string _clientSecret;
        private readonly HttpClientHelper _httpClient;
        private readonly string _url;
        private string _accessToken;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="spaceId"></param>
        /// <param name="clientSecret"></param>
        public DataBaseClient(string spaceId, string clientSecret)
        {
            this._spaceId = spaceId;
            this._clientSecret = clientSecret;
            this._url = "https://api.bspapp.com/client";
            _httpClient = new HttpClientHelper();
        }

        /// <summary>
        /// 小程序云数据库查询方法
        /// </summary>
        /// <param name="collectionName">数据库名</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<T>> QueryAsync<T>(string collectionName, QueryParameter query) where T : class
        {
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
                {"token", _accessToken}
            };

            string url = "https://api.bspapp.com/client";

            var headerItem = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "x-serverless-sign", Sign(data, this._clientSecret) },
                {"x-basement-token", data["token"].ToString()}
            };
            _httpClient.SetHeaderItem(headerItem);
            var json = await _httpClient.PostAsync(url, data);
            var jo = System.Text.Json.JsonDocument.Parse(json);
            bool success = jo.RootElement.GetProperty("success").GetBoolean();
            var resData = new List<T>();
            if (success)
            {
                var res = jo.RootElement.GetProperty("data").GetProperty("data").EnumerateArray();
                resData = res.Deserialize<T>();
            }


            return resData;
        }

        /// <summary>
        /// 小程序云数据库添加方法
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="paramData"></param>
        /// <returns></returns>
        public async Task<string> AddAsync(string collectionName, IDictionary<string, object> paramData)
        {
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
            fun = fun.Replace("$collectionName", collectionName);
            string data = JsonSerializer.Serialize(paramData);
            fun = fun.Replace("$data", data);


            var p = new Dictionary<string, object>()
            {
                { "method", "serverless.function.runtime.invoke" },
                { "params",  fun},
                { "spaceId", this._spaceId },
                { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() },
                {"token", _accessToken}
            };

            string url = "https://api.bspapp.com/client";

            var headerItem = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" },
                    { "x-serverless-sign", Sign(p, this._clientSecret) },
                    {"x-basement-token", p["token"].ToString()}
                };
            _httpClient.SetHeaderItem(headerItem);
            var json = await _httpClient.PostAsync(url, p);
            var jo = System.Text.Json.JsonDocument.Parse(json);
            bool success = jo.RootElement.GetProperty("success").GetBoolean();
            var id = "";
            if (success)
            {
                id = jo.RootElement.GetProperty("data").GetProperty("id").GetString();

            }

            return id;
        }

        /// <summary>
        /// 小程序云数据库添加方法
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="paramData"></param>
        /// <returns></returns>
        public async Task<List<string>> AddListAsync(string collectionName, List<IDictionary<string, object>> paramData)
        {

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
            fun = fun.Replace("$collectionName", collectionName);
            string data = JsonSerializer.Serialize(paramData);
            fun = fun.Replace("$data", data);

            var p = new Dictionary<string, object>()
            {
                { "method", "serverless.function.runtime.invoke" },
                { "params",  fun},
                { "spaceId", this._spaceId },
                { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() },
                {"token", _accessToken}
            };

            string url = "https://api.bspapp.com/client";

            var headerItem = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" },
                    { "x-serverless-sign", Sign(p, this._clientSecret) },
                    {"x-basement-token", p["token"].ToString()}
                };
            _httpClient.SetHeaderItem(headerItem);
            var json = await _httpClient.PostAsync(url, p);
            var jo = System.Text.Json.JsonDocument.Parse(json);
            bool success = jo.RootElement.GetProperty("success").GetBoolean();
            var ids = new List<string>();
            if (success)
            {
                var res = jo.RootElement.GetProperty("data").GetProperty("ids").EnumerateArray();
                foreach (JsonElement je in res)
                {
                    ids.Add(je.GetString());
                }
            }

            return ids;
        }

        /// <summary>
        /// 小程序云数据库更新方法
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="where"></param>
        /// <param name="paramData"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(string collectionName, string where, IDictionary<string, object> paramData)
        {
            var fun = """
                        {
            	"functionTarget": "DCloud-clientDB",
            	"functionArgs": {
            		"command": {
            			"$db": [{
            				"$method": "collection",
            				"$param": ["$collectionName"]
            			}, $where {
            	            "$method": "update",
            	            "$param": [$data]
                        }]
            		},
            		"uniIdToken": ""
            	}
            }
            """;
            string whereStr = "";
            if (!string.IsNullOrWhiteSpace(where))
            {
                whereStr = "{\"$method\":\"where\",\"$param\":[" + where + "]},";
            }
            fun = fun.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");
            fun = fun.Replace("$collectionName", collectionName);
            fun = fun.Replace("$where", whereStr);

            string data = JsonSerializer.Serialize(paramData);
            fun = fun.Replace("$data", data);


            var p = new Dictionary<string, object>()
            {
                { "method", "serverless.function.runtime.invoke" },
                { "params",  fun},
                { "spaceId", this._spaceId },
                { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() },
                {"token", _accessToken}
            };

            string url = "https://api.bspapp.com/client";

            var headerItem = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" },
                    { "x-serverless-sign", Sign(p, this._clientSecret) },
                    {"x-basement-token", p["token"].ToString()}
                };
            _httpClient.SetHeaderItem(headerItem);
            var json = await _httpClient.PostAsync(url, p);
            var jo = System.Text.Json.JsonDocument.Parse(json);
            bool success = jo.RootElement.GetProperty("success").GetBoolean();
            var id = "";
            if (success)
            {
                id = jo.RootElement.GetProperty("data").GetProperty("id").GetString();

            }

            return true;
        }

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
        /// 上传文件
        /// </summary>
        /// <param name="file">文件内容</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public async Task<string> UploadAsync(byte[] file, string fileName)
        {
            var fileInfo = await CreatFileNameAsync(fileName);
            await UploadFileAsync(file, fileInfo);
            bool b = await CheckFileAsync(fileInfo.Id);
            return b ? $"https://{fileInfo.CdnDomain}/{fileInfo.OssPath}" : "";
        }

        private async Task<FileInfoResponse> CreatFileNameAsync(string fileName)
        {
            IDictionary<string, object> options = new Dictionary<string, object>(){
                {"method", "serverless.file.resource.generateProximalSign"},
                {"params","{\"env\":\"public\",\"filename\":\""+fileName+"\"}"},
                {"spaceId", this._spaceId},
                { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() },
                {"token", _accessToken}
            };

            var headerItem = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "x-serverless-sign", Sign(options, this._clientSecret) },
                {"x-basement-token", options["token"].ToString()}
            };
            _httpClient.SetHeaderItem(headerItem);

            var json = await _httpClient.PostAsync(_url, options);
            var jo = System.Text.Json.JsonDocument.Parse(json);
            bool success = jo.RootElement.GetProperty("success").GetBoolean();
            var fileInfo = new FileInfoResponse();
            if (success)
            {
                var res = jo.RootElement.GetProperty("data");
                fileInfo = res.Deserialize<FileInfoResponse>();
            }
            return fileInfo;
        }
        private async Task UploadFileAsync(byte[] file, FileInfoResponse fileInfo)
        {
            string url = "https://" + fileInfo.Host + "/";
            IDictionary<string, object> options = new Dictionary<string, object>(){
                {"Cache-Control", "max-age=2592000"},
                {"Content-Disposition", "attachment"},
                {"OSSAccessKeyId", fileInfo.AccessKeyId},
                {"Signature", fileInfo.Signature},
                {"host", fileInfo.Host},
                {"id", fileInfo.Id},
                {"key", fileInfo.OssPath},
                {"policy", fileInfo.Policy},
                {"success_action_status", "200"},
                {"file", file}
            };
            IDictionary<string, string> headerItem = new Dictionary<string, string>()
            {
                {"Content-Type", "multipart/form-data"},
                {"X-OSS-server-side-encrpytion", "AES256" },
            };
            _httpClient.SetHeaderItem(headerItem);
            await _httpClient.PostAsync(url, options);
        }
        private async Task<bool> CheckFileAsync(string id)
        {
            IDictionary<string, object> options = new Dictionary<string, object>(){
                {"method", "serverless.file.resource.report"},
                {"params", "{\"id\":\""+id+"\"}"},
                {"spaceId", _spaceId},
                { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() },
                {"token", _accessToken}
            };
            var headerItem = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "x-serverless-sign", Sign(options, this._clientSecret) },
                { "x-basement-token", options["token"].ToString()! }
            };
            _httpClient.SetHeaderItem(headerItem);
            var html = await _httpClient.PostAsync(_url, options);
            var jo = System.Text.Json.JsonDocument.Parse(html);
            return jo.RootElement.GetProperty("success").GetBoolean();
        }
        /// <summary>
        /// 获取接口调用凭证
        /// </summary>
        /// <remarks>参考自：https://github.com/79W/uni-cloud-storage</remarks>
        /// <returns></returns>
        public async Task GetAccessTokenAsync()
        {
            var data = new Dictionary<string, object>()
            {
                { "method", "serverless.auth.user.anonymousAuthorize" },
                { "params", "{}" },
                { "spaceId", this._spaceId },
                { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() }
            };

            string url = "https://api.bspapp.com/client";


            var headerItem = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" },
                    { "x-serverless-sign", Sign(data, this._clientSecret) },
                };
            _httpClient.SetHeaderItem(headerItem);
            var html = await _httpClient.PostAsync(url, data);
            var jo = System.Text.Json.JsonDocument.Parse(html);
            bool success = jo.RootElement.GetProperty("success").GetBoolean();
            var accessToken = "";
            if (success)
            {
                accessToken = jo.RootElement.GetProperty("data").GetProperty("accessToken").GetString() ?? "";
            }

            _accessToken = accessToken;
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