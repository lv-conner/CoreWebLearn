//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//     //
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </自动生成>
//------------------------------------------------------------------------------

namespace WcfService
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WcfService.INameService")]
    public interface INameService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INameService/Hello", ReplyAction="http://tempuri.org/INameService/HelloResponse")]
        System.Threading.Tasks.Task<string> HelloAsync(string name);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public interface INameServiceChannel : WcfService.INameService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class NameServiceClient : System.ServiceModel.ClientBase<WcfService.INameService>, WcfService.INameService
    {
        
    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public NameServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(NameServiceClient.GetBindingForEndpoint(endpointConfiguration), NameServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public NameServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(NameServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public NameServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(NameServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public NameServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<string> HelloAsync(string name)
        {
            return base.Channel.HelloAsync(name);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.NameService))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                result.Elements.Add(new System.ServiceModel.Channels.BinaryMessageEncodingBindingElement());
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.NameService1))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                result.Elements.Add(new System.ServiceModel.Channels.BinaryMessageEncodingBindingElement());
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.NameService))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:46468/NameService");
            }
            if ((endpointConfiguration == EndpointConfiguration.NameService1))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:9562/NameService");
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            NameService,
            
            NameService1,
        }
    }
}
