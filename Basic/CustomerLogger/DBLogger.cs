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
                context.Database.EnsureCreated();
                var log = new LogModel("Error",state.ToString(),exception.GetType().ToString(),exception.StackTrace)
                {
                    EventId = eventId.Id
                };
                context.Set<LogModel>().Add(log);
                context.SaveChanges();
                //context.Set<LogModel>().Add()
            }
        }
    }
}
