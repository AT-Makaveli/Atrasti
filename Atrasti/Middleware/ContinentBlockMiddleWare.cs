using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Atrasti.Middleware
{
    public class ContinentBlockMiddleWare : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Path.ToString().Contains("Construction"))
            {
                if (context.Session.TryGetValue("continent", out byte[] data))
                {
                    if (Encoding.Default.GetString(data) != "Allow")
                    {
                        context.Response.Redirect("/Construction");
                    }
                }
                else
                {
                    string remoteIpAddress = context.Request.Headers["X-Forwarded-For"];
                    
                    string requestUrl =
                        $"http://ip-api.com/json/{remoteIpAddress}?fields=status,continent,country";
                    string json = new WebClient().DownloadString(requestUrl);
                    ContinentJsonResponse jsonResponse = JsonConvert.DeserializeObject<ContinentJsonResponse>(json);
                    if (jsonResponse.status == "success")
                    {
                        if (jsonResponse.continent == "Asia" || jsonResponse.country == "Russia" || remoteIpAddress == "213.158.48.209" || remoteIpAddress == "185.197.227.71" || remoteIpAddress == "94.234.51.20")
                        {
                            context.Session.Set("continent", Encoding.Default.GetBytes("Allow"));
                        }
                        else
                        {
                            context.Session.Set("continent", Encoding.Default.GetBytes("Disallow"));
                            context.Response.Redirect("/Construction");
                        }
                    }
                }
            }
            
            await next(context);
        }

        public class ContinentJsonResponse
        {
            public string status { get; set; }
            public string continent { get; set; }
            public string country { get; set; }
        }
    }
}