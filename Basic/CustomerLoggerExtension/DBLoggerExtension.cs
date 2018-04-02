using Basic.CustomerLoggerProvider;
using Basic.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.CustomerLoggerExtension
{
    public static class DBLoggerExtension
    {
        public static ILoggerFactory AddDBLogger(this ILoggerFactory factory, DBLoggerOptions options)
        {
            factory.AddProvider(new DBLoggerProvider(options));
            return factory;
        }
    }
}
