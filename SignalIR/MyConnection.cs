using Microsoft.AspNetCore.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalIR
{
    public class MyConnection : ConnectionHandler
    {
        public override Task OnConnectedAsync(ConnectionContext connection)
        {
            throw new NotImplementedException();
        }
    }
}
