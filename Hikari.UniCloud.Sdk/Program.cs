// See https://aka.ms/new-console-template for more information

using System;
using System.Text.RegularExpressions;
using ConsoleApp1;
using ConsoleApp1.UniCloud;
using Hikari.Common;
using Hikari.Common.Net.Http;

var idc = new Dictionary<string, object>()
{
    { "method", "serverless.auth.user.anonymousAuthorize" },
    { "params", "{}" },
    { "spaceId", "5af01932-9c54-47e2-b57e-f150aec0d075" },
    { "timestamp", DateTime.Now.ToUnixTimeMilliseconds() }
};

var c = new DataBaseClient("5af01932-9c54-47e2-b57e-f150aec0d075", "uQIxQIH3RTws2kD1olrleQ==");
var v = await c.QueryAsync<Class2>("opendb-app-list");

Console.WriteLine("Hello, World!");
Console.ReadKey();

