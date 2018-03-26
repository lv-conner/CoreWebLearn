using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;

namespace ResponseCacheLearn
{
    public class Program
    {
        public static void Main(string[] args)
        {

            new WebHostBuilder()
            .UseKestrel()
            .ConfigureServices(svcs => svcs.AddResponseCaching())
            .Configure(app => app
            .UseResponseCaching()
            .Run(async context =>
             {
                  context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                   {
                      Public = true,
                      MaxAge = TimeSpan.FromSeconds(3600)
                  };

                  string utc = context.Request.Query["utc"].FirstOrDefault() ?? "";
                  bool isUtc = string.Equals(utc, "true", StringComparison.OrdinalIgnoreCase);
                  await context.Response.WriteAsync(isUtc ? DateTime.UtcNow.ToString() : DateTime.UtcNow.ToString());
             }))
            .Build()
            .Run();
            //BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
