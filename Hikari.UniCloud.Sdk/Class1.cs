using Hikari.Common;
using Hikari.Common.Net.Http;
using Hikari.Common.Security;

namespace ConsoleApp1;

public class Class1
{
    private readonly string _spaceId;
    private readonly string _clientSecret;
    public Class1(string spaceId, string clientSecret)
    {
        this._spaceId = spaceId;
        this._clientSecret = clientSecret;
    }
    public string Sign(IDictionary<string, object> dic, string key)
    {
        var s = dic.OrderBy(x => x.Key).Select(x => x.Key + "=" + x.Value);
        var str = string.Join('&', s);
        var md5Value = new SecureHelper(str).HmacMd5(key).DigestHex();
        return md5Value;
    }

    public async Task<string> GetAccessToken()
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

    public async Task<string> A(string collectionName, string field = "", int skip = 0, int limit = 0)
    {
        var accessToken = await GetAccessToken();
        var fun = """
                        {
            	"functionTarget": "DCloud-clientDB",
            	"functionArgs": {
            		"command": {
            			"$db": [{
            				"$method": "collection",
            				"$param": ["$collectionName"]
            			}, $field {
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
        string fieldStr = "";
        if (!string.IsNullOrWhiteSpace(field))
        {
            fieldStr = "{\"$method\":\"field\",\"$param\":[" + field + "]},";
        }

        string limitStr = "";
        if (limit > 0)
        {
            limitStr = "{\"$method\":\"limit\",\"$param\":[" + limit + "]},";
        }
        fun = fun.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");
        fun = fun.Replace("$collectionName", collectionName).Replace("$skip", skip.ToString()).Replace("$field", fieldStr).Replace("$limit", limitStr);
        var data = new Dictionary<string, object>()
        {
            { "method", "serverless.function.runtime.invoke" },
            { "params",  fun},
            { "spaceId", this._spaceId },
            { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() },
            {"token", accessToken}
        };

        string url = "https://api.bspapp.com/client";

        HttpClientHelper httpClientHelper = new HttpClientHelper();
        var headerItem = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "x-serverless-sign", Sign(data, this._clientSecret) },
                {"x-basement-token", data["token"].ToString()}
            }
            ;
        var html = await httpClientHelper.PostAsync(url, data, headerItem: headerItem);
        //var jo = System.Text.Json.JsonDocument.Parse(html);
        //bool success = jo.RootElement.GetProperty("success").GetBoolean();
        //if (success)
        //{
        //    return jo.RootElement.GetProperty("data").GetProperty("accessToken").GetString() ?? "";
        //}

        return "";
    }
}