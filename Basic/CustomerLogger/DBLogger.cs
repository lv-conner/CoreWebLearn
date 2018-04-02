using Basic.Context;
using Basic.Model;
using Basic.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.CustomerLogger
{
    public class DBLogger : ILogger
    {
        private string categoryName;
        private DBLoggerOptions options;

        public DBLogger()
        {

        }

        public DBLogger(string categoryName, DBLoggerOptions options)
        {
            this.categoryName = categoryName;
            this.options = options;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            using (var context = new LoggerContext(options.LoggerConnectionString))
            {
                
                //context.Set<LogModel>().Add()
            }
        }
    }
}
