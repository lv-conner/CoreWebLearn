using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WcfService;
using static WcfService.NameServiceClient;

namespace WcfService
{
    public partial class NameServiceClient
    {
        public NameServiceClient():this(EndpointConfiguration.NameService)
        {
            
        }
        public string name;
    }
}
