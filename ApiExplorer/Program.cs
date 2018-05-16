using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ApiExplorer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var  configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "ASPNETCORE_")
            .AddEnvironmentVariables()
            .Build();
            var applicationName = configuration["applicationName"];
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static IWebHost BuildWebHostVersion(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .PreferHostingUrls(false)
                .CaptureStartupErrors(false)
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        await context.Response.WriteAsync("hello");
                    });
                })
                .ConfigureLogging((context, loggerbuilder) =>
                {
                    loggerbuilder.AddConsole();
                    loggerbuilder.AddDebug();
                })
                .ConfigureServices(services =>
                {
                    services.AddMvc();
                })
            .Build();
        }
    }
}
