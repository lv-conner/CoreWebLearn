using Basic.CustomerLogger;
using Basic.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.CustomerLoggerProvider
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private FileLoggerOptions options;
        public FileLoggerProvider(FileLoggerOptions options)
        {
            this.options = options;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new Basic.CustomerLogger.FileLogger(options.Path);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
