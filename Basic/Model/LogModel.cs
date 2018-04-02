using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.Model
{
    public class LogModel
    {
        public Guid LogId { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
    }
}
