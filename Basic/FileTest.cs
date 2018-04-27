using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using FileLogger;
namespace Basic
{
    class FileTest
    {
        public static void TestLog()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
            .AddOptions()
            .AddLogging()
            .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            factory.AddProvider(new FileLoggerProvider((s, l) => true));
            var logger = factory.CreateLogger("program");
            logger.LogInformation("hello");
        }
    }
}
