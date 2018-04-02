using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Basic.CustomerLogger
{
    public class FileLogger : ILogger
    {
        private string filePath;
        public FileLogger(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                throw new Exception(filePath + "can not be null");
            }
            this.filePath = filePath;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //File.AppendText(filePath);
            File.AppendAllText(filePath, state.ToString());
            //Console.WriteLine(state.ToString() + "\t");
        }
    }
}
