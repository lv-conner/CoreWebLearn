using CoreWebLearn.Model;
using CoreWebLearn.serviceinterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebLearn.service
{
    public class Log : ILog
    {
        public async Task  LogMessage(Message message)
        {
            //Console.WriteLine(message);
            await Task.Run(() => Console.WriteLine(message));
        }
    }
}
