using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.Model
{
    public class LogModel
    {
        protected LogModel()
        {

        }
        public LogModel(string message):this("Message",message)
        {

        }
        public LogModel(string logType,string message):this(logType,message,null,null)
        {

        }
        public LogModel(string logType,string message,string exceptionType,string stackTrace)
        {
            this.LogType = logType;
            this.Message = message;
            this.ExceptionType = exceptionType;
            this.StackTrace = stackTrace;
        }
        public int LogId { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
        public DateTime LogTime { get; set; }
        public int EventId { get; set; }
    }
}
