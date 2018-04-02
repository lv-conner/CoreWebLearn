using Basic.CustomerLoggerProvider;
using Basic.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.CustomerLoggerExtension
{
    public static class CustomerLoggerExtension
    {
        public static ILoggerFactory AddFileLogger(this ILoggerFactory loggerFactory, FileLoggerOptions options)
        {
            loggerFactory.AddProvider(new FileLoggerProvider(options));
            return loggerFactory;
        }
    }
}
