﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </auto-generated>
//------------------------------------------------------------------------------

namespace zzjserver
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://qhsoft.org/", ConfigurationName="zzjserver.ServiceSoap")]
    public interface ServiceSoap
    {
        
        // CODEGEN: 正在生成消息协定，因为消息 my_nameRequest 具有多个标头
        [System.ServiceModel.OperationContractAttribute(Action="http://qhsoft.org/my_name", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        zzjserver.my_nameResponse my_name(zzjserver.my_nameRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://qhsoft.org/my_name", ReplyAction="*")]
        System.Threading.Tasks.Task<zzjserver.my_nameResponse> my_nameAsync(zzjserver.my_nameRequest request);
        
        // CODEGEN: 正在生成消息协定，因为消息 BusinessYYJK_SECRETRequest 具有多个标头
        [System.ServiceModel.OperationContractAttribute(Action="http://qhsoft.org/BusinessYYJK_SECRET", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        zzjserver.BusinessYYJK_SECRETResponse BusinessYYJK_SECRET(zzjserver.BusinessYYJK_SECRETRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://qhsoft.org/BusinessYYJK_SECRET", ReplyAction="*")]
        System.Threading.Tasks.Task<zzjserver.BusinessYYJK_SECRETResponse> BusinessYYJK_SECRETAsync(zzjserver.BusinessYYJK_SECRETRequest request);
        
        // CODEGEN: 正在生成消息协定，因为消息 BusinessZZJRequest 具有多个标头
        [System.ServiceModel.OperationContractAttribute(Action="http://qhsoft.org/BusinessZZJ", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        zzjserver.BusinessZZJResponse BusinessZZJ(zzjserver.BusinessZZJRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://qhsoft.org/BusinessZZJ", ReplyAction="*")]
        System.Threading.Tasks.Task<zzjserver.BusinessZZJResponse> BusinessZZJAsync(zzjserver.BusinessZZJRequest request);
        
        // CODEGEN: 正在生成消息协定，因为消息 WXZFBTFRequest 具有多个标头
        [System.ServiceModel.OperationContractAttribute(Action="http://qhsoft.org/WXZFBTF", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        zzjserver.WXZFBTFResponse WXZFBTF(zzjserver.WXZFBTFRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://qhsoft.org/WXZFBTF", ReplyAction="*")]
        System.Threading.Tasks.Task<zzjserver.WXZFBTFResponse> WXZFBTFAsync(zzjserver.WXZFBTFRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://qhsoft.org/")]
    public partial class MySoapHeader
    {
        
        private string userNameField;
        
        private string passWordField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string PassWord
        {
            get
            {
                return this.passWordField;
            }
            set
            {
                this.passWordField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="my_name", WrapperNamespace="http://qhsoft.org/", IsWrapped=true)]
    public partial class my_nameRequest
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://qhsoft.org/")]
        public zzjserver.MySoapHeader MySoapHeader;
        
        public my_nameRequest()
        {
        }
        
        public my_nameRequest(zzjserver.MySoapHeader MySoapHeader)
        {
            this.MySoapHeader = MySoapHeader;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="my_nameResponse", WrapperNamespace="http://qhsoft.org/", IsWrapped=true)]
    public partial class my_nameResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=0)]
        public string my_nameResult;
        
        public my_nameResponse()
        {
        }
        
        public my_nameResponse(string my_nameResult)
        {
            this.my_nameResult = my_nameResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="BusinessYYJK_SECRET", WrapperNamespace="http://qhsoft.org/", IsWrapped=true)]
    public partial class BusinessYYJK_SECRETRequest
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://qhsoft.org/")]
        public zzjserver.MySoapHeader MySoapHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=0)]
        public string xmlStr;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=1)]
        public string user_id;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=2)]
        public string signature;
        
        public BusinessYYJK_SECRETRequest()
        {
        }
        
        public BusinessYYJK_SECRETRequest(zzjserver.MySoapHeader MySoapHeader, string xmlStr, string user_id, string signature)
        {
            this.MySoapHeader = MySoapHeader;
            this.xmlStr = xmlStr;
            this.user_id = user_id;
            this.signature = signature;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="BusinessYYJK_SECRETResponse", WrapperNamespace="http://qhsoft.org/", IsWrapped=true)]
    public partial class BusinessYYJK_SECRETResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=0)]
        public string BusinessYYJK_SECRETResult;
        
        public BusinessYYJK_SECRETResponse()
        {
        }
        
        public BusinessYYJK_SECRETResponse(string BusinessYYJK_SECRETResult)
        {
            this.BusinessYYJK_SECRETResult = BusinessYYJK_SECRETResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="BusinessZZJ", WrapperNamespace="http://qhsoft.org/", IsWrapped=true)]
    public partial class BusinessZZJRequest
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://qhsoft.org/")]
        public zzjserver.MySoapHeader MySoapHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=0)]
        public string xmlStr;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=1)]
        public string user_id;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=2)]
        public string signature;
        
        public BusinessZZJRequest()
        {
        }
        
        public BusinessZZJRequest(zzjserver.MySoapHeader MySoapHeader, string xmlStr, string user_id, string signature)
        {
            this.MySoapHeader = MySoapHeader;
            this.xmlStr = xmlStr;
            this.user_id = user_id;
            this.signature = signature;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="BusinessZZJResponse", WrapperNamespace="http://qhsoft.org/", IsWrapped=true)]
    public partial class BusinessZZJResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=0)]
        public string BusinessZZJResult;
        
        public BusinessZZJResponse()
        {
        }
        
        public BusinessZZJResponse(string BusinessZZJResult)
        {
            this.BusinessZZJResult = BusinessZZJResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="WXZFBTF", WrapperNamespace="http://qhsoft.org/", IsWrapped=true)]
    public partial class WXZFBTFRequest
    {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://qhsoft.org/")]
        public zzjserver.MySoapHeader MySoapHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=0)]
        public string xmlStr;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=1)]
        public string user_id;
        
        public WXZFBTFRequest()
        {
        }
        
        public WXZFBTFRequest(zzjserver.MySoapHeader MySoapHeader, string xmlStr, string user_id)
        {
            this.MySoapHeader = MySoapHeader;
            this.xmlStr = xmlStr;
            this.user_id = user_id;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="WXZFBTFResponse", WrapperNamespace="http://qhsoft.org/", IsWrapped=true)]
    public partial class WXZFBTFResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qhsoft.org/", Order=0)]
        public string WXZFBTFResult;
        
        public WXZFBTFResponse()
        {
        }
        
        public WXZFBTFResponse(string WXZFBTFResult)
        {
            this.WXZFBTFResult = WXZFBTFResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface ServiceSoapChannel : zzjserver.ServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class ServiceSoapClient : System.ServiceModel.ClientBase<zzjserver.ServiceSoap>, zzjserver.ServiceSoap
    {
        
        /// <summary>
        /// 实现此分部方法，配置服务终结点。
        /// </summary>
        /// <param name="serviceEndpoint">要配置的终结点</param>
        /// <param name="clientCredentials">客户端凭据</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public ServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(ServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), ServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        zzjserver.my_nameResponse zzjserver.ServiceSoap.my_name(zzjserver.my_nameRequest request)
        {
            return base.Channel.my_name(request);
        }
        
        public string my_name(zzjserver.MySoapHeader MySoapHeader)
        {
            zzjserver.my_nameRequest inValue = new zzjserver.my_nameRequest();
            inValue.MySoapHeader = MySoapHeader;
            zzjserver.my_nameResponse retVal = ((zzjserver.ServiceSoap)(this)).my_name(inValue);
            return retVal.my_nameResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<zzjserver.my_nameResponse> zzjserver.ServiceSoap.my_nameAsync(zzjserver.my_nameRequest request)
        {
            return base.Channel.my_nameAsync(request);
        }
        
        public System.Threading.Tasks.Task<zzjserver.my_nameResponse> my_nameAsync(zzjserver.MySoapHeader MySoapHeader)
        {
            zzjserver.my_nameRequest inValue = new zzjserver.my_nameRequest();
            inValue.MySoapHeader = MySoapHeader;
            return ((zzjserver.ServiceSoap)(this)).my_nameAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        zzjserver.BusinessYYJK_SECRETResponse zzjserver.ServiceSoap.BusinessYYJK_SECRET(zzjserver.BusinessYYJK_SECRETRequest request)
        {
            return base.Channel.BusinessYYJK_SECRET(request);
        }
        
        public string BusinessYYJK_SECRET(zzjserver.MySoapHeader MySoapHeader, string xmlStr, string user_id, string signature)
        {
            zzjserver.BusinessYYJK_SECRETRequest inValue = new zzjserver.BusinessYYJK_SECRETRequest();
            inValue.MySoapHeader = MySoapHeader;
            inValue.xmlStr = xmlStr;
            inValue.user_id = user_id;
            inValue.signature = signature;
            zzjserver.BusinessYYJK_SECRETResponse retVal = ((zzjserver.ServiceSoap)(this)).BusinessYYJK_SECRET(inValue);
            return retVal.BusinessYYJK_SECRETResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<zzjserver.BusinessYYJK_SECRETResponse> zzjserver.ServiceSoap.BusinessYYJK_SECRETAsync(zzjserver.BusinessYYJK_SECRETRequest request)
        {
            return base.Channel.BusinessYYJK_SECRETAsync(request);
        }
        
        public System.Threading.Tasks.Task<zzjserver.BusinessYYJK_SECRETResponse> BusinessYYJK_SECRETAsync(zzjserver.MySoapHeader MySoapHeader, string xmlStr, string user_id, string signature)
        {
            zzjserver.BusinessYYJK_SECRETRequest inValue = new zzjserver.BusinessYYJK_SECRETRequest();
            inValue.MySoapHeader = MySoapHeader;
            inValue.xmlStr = xmlStr;
            inValue.user_id = user_id;
            inValue.signature = signature;
            return ((zzjserver.ServiceSoap)(this)).BusinessYYJK_SECRETAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        zzjserver.BusinessZZJResponse zzjserver.ServiceSoap.BusinessZZJ(zzjserver.BusinessZZJRequest request)
        {
            return base.Channel.BusinessZZJ(request);
        }
        
        public string BusinessZZJ(zzjserver.MySoapHeader MySoapHeader, string xmlStr, string user_id, string signature)
        {
            zzjserver.BusinessZZJRequest inValue = new zzjserver.BusinessZZJRequest();
            inValue.MySoapHeader = MySoapHeader;
            inValue.xmlStr = xmlStr;
            inValue.user_id = user_id;
            inValue.signature = signature;
            zzjserver.BusinessZZJResponse retVal = ((zzjserver.ServiceSoap)(this)).BusinessZZJ(inValue);
            return retVal.BusinessZZJResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<zzjserver.BusinessZZJResponse> zzjserver.ServiceSoap.BusinessZZJAsync(zzjserver.BusinessZZJRequest request)
        {
            return base.Channel.BusinessZZJAsync(request);
        }
        
        public System.Threading.Tasks.Task<zzjserver.BusinessZZJResponse> BusinessZZJAsync(zzjserver.MySoapHeader MySoapHeader, string xmlStr, string user_id, string signature)
        {
            zzjserver.BusinessZZJRequest inValue = new zzjserver.BusinessZZJRequest();
            inValue.MySoapHeader = MySoapHeader;
            inValue.xmlStr = xmlStr;
            inValue.user_id = user_id;
            inValue.signature = signature;
            return ((zzjserver.ServiceSoap)(this)).BusinessZZJAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        zzjserver.WXZFBTFResponse zzjserver.ServiceSoap.WXZFBTF(zzjserver.WXZFBTFRequest request)
        {
            return base.Channel.WXZFBTF(request);
        }
        
        public string WXZFBTF(zzjserver.MySoapHeader MySoapHeader, string xmlStr, string user_id)
        {
            zzjserver.WXZFBTFRequest inValue = new zzjserver.WXZFBTFRequest();
            inValue.MySoapHeader = MySoapHeader;
            inValue.xmlStr = xmlStr;
            inValue.user_id = user_id;
            zzjserver.WXZFBTFResponse retVal = ((zzjserver.ServiceSoap)(this)).WXZFBTF(inValue);
            return retVal.WXZFBTFResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<zzjserver.WXZFBTFResponse> zzjserver.ServiceSoap.WXZFBTFAsync(zzjserver.WXZFBTFRequest request)
        {
            return base.Channel.WXZFBTFAsync(request);
        }
        
        public System.Threading.Tasks.Task<zzjserver.WXZFBTFResponse> WXZFBTFAsync(zzjserver.MySoapHeader MySoapHeader, string xmlStr, string user_id)
        {
            zzjserver.WXZFBTFRequest inValue = new zzjserver.WXZFBTFRequest();
            inValue.MySoapHeader = MySoapHeader;
            inValue.xmlStr = xmlStr;
            inValue.user_id = user_id;
            return ((zzjserver.ServiceSoap)(this)).WXZFBTFAsync(inValue);
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
            if ((endpointConfiguration == EndpointConfiguration.ServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.ServiceSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
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
            if ((endpointConfiguration == EndpointConfiguration.ServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://192.168.3.46:9115/Service.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.ServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://192.168.3.46:9115/Service.asmx");
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            ServiceSoap,
            
            ServiceSoap12,
        }
    }
}