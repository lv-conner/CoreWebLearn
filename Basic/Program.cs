using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Basic.CustomerLoggerExtension;
using System.Reflection;
using System.IO;
using Basic.Options;

namespace Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(),"appsettings.json")).Build();
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Log.txt");
            var services = new ServiceCollection()
                .AddOptions()
                .AddLogging()
                //.Configure<FileLoggerOptions>(options =>
                //{
                //    options.Path = logFilePath;
                //})
                .Configure<FileLoggerOptions>(config.GetSection("fileLoggerOptions"))
                .BuildServiceProvider();
            var logger = services.GetService<ILoggerFactory>()
                .AddConsole()
                .AddDebug()
                .AddFileLogger(services.GetService<IOptions<FileLoggerOptions>>().Value)
                .CreateLogger<Program>();
            logger.LogInformation("Hello");


            Console.ReadKey();
        }
    }
}
