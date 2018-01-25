using CoreWebLearn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebLearn.serviceinterface
{
    public interface ILog
    {
        Task LogMessage(Message message);
    }
}
