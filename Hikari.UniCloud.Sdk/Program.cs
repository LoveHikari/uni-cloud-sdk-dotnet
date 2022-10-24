// See https://aka.ms/new-console-template for more information

using System;
using System.Text.RegularExpressions;
using Hikari.Common;
using Hikari.Common.Net.Http;
using Hikari.UniCloud.Sdk;

var idc = new Dictionary<string, object>()
{
    { "method", "serverless.auth.user.anonymousAuthorize" },
    { "params", "{}" },
    { "spaceId", "5af01932-9c54-47e2-b57e-f150aec0d075" },
    { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() }
};

var c = new DataBaseClient("5af01932-9c54-47e2-b57e-f150aec0d075", "uQIxQIH3RTws2kD1olrleQ==");

IDictionary<string, object> dic = new Dictionary<string, object>()
{
    { "name", "1111" },
    { "type", "0" },
};
List<IDictionary<string, object>> dd = new List<IDictionary<string, object>>();

dd.Add(dic);
dd.Add(dic);


var v = await c.AddListAsync("opendb-app-list", dd);

Console.WriteLine("Hello, World!");
Console.ReadKey();

