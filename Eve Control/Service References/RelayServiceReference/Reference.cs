﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EveControl.RelayServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RelayServiceReference.IClientRelayService", CallbackContract=typeof(EveControl.RelayServiceReference.IClientRelayServiceCallback))]
    public interface IClientRelayService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/Subscribe", ReplyAction="http://tempuri.org/IClientRelayService/SubscribeResponse")]
        bool Subscribe(Eve.API.Services.Common.ServiceClient clientData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/Subscribe", ReplyAction="http://tempuri.org/IClientRelayService/SubscribeResponse")]
        System.Threading.Tasks.Task<bool> SubscribeAsync(Eve.API.Services.Common.ServiceClient clientData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/Unsibscribe", ReplyAction="http://tempuri.org/IClientRelayService/UnsibscribeResponse")]
        bool Unsibscribe(Eve.API.Services.Common.ServiceClient clientData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/Unsibscribe", ReplyAction="http://tempuri.org/IClientRelayService/UnsibscribeResponse")]
        System.Threading.Tasks.Task<bool> UnsibscribeAsync(Eve.API.Services.Common.ServiceClient clientData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/ClientPing", ReplyAction="http://tempuri.org/IClientRelayService/ClientPingResponse")]
        string ClientPing(string yourName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/ClientPing", ReplyAction="http://tempuri.org/IClientRelayService/ClientPingResponse")]
        System.Threading.Tasks.Task<string> ClientPingAsync(string yourName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IClientRelayServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SignIn", ReplyAction="http://tempuri.org/IClientRelayService/SignInResponse")]
        bool SignIn(Eve.API.Services.Common.ServiceUser user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SignOut", ReplyAction="http://tempuri.org/IClientRelayService/SignOutResponse")]
        bool SignOut(Eve.API.Services.Common.ServiceUser user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/GetAvailableClients", ReplyAction="http://tempuri.org/IClientRelayService/GetAvailableClientsResponse")]
        Eve.API.Services.Common.ServiceClient[] GetAvailableClients();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SendTrackPadMessage", ReplyAction="http://tempuri.org/IClientRelayService/SendTrackPadMessageResponse")]
        bool SendTrackPadMessage(Eve.API.Services.Common.ServiceRequestDetails details, Eve.API.Services.Common.Modules.Touch.TrackPadMessage message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SendButtonMessage", ReplyAction="http://tempuri.org/IClientRelayService/SendButtonMessageResponse")]
        bool SendButtonMessage(Eve.API.Services.Common.ServiceRequestDetails details, Eve.API.Services.Common.Modules.Touch.ButtonMessage message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SetZoom", ReplyAction="http://tempuri.org/IClientRelayService/SetZoomResponse")]
        bool SetZoom(Eve.API.Services.Common.ServiceRequestDetails details, int zoomValue);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IClientRelayServiceChannel : EveControl.RelayServiceReference.IClientRelayService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ClientRelayServiceClient : System.ServiceModel.DuplexClientBase<EveControl.RelayServiceReference.IClientRelayService>, EveControl.RelayServiceReference.IClientRelayService {
        
        public ClientRelayServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ClientRelayServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ClientRelayServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ClientRelayServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ClientRelayServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public bool Subscribe(Eve.API.Services.Common.ServiceClient clientData) {
            return base.Channel.Subscribe(clientData);
        }
        
        public System.Threading.Tasks.Task<bool> SubscribeAsync(Eve.API.Services.Common.ServiceClient clientData) {
            return base.Channel.SubscribeAsync(clientData);
        }
        
        public bool Unsibscribe(Eve.API.Services.Common.ServiceClient clientData) {
            return base.Channel.Unsibscribe(clientData);
        }
        
        public System.Threading.Tasks.Task<bool> UnsibscribeAsync(Eve.API.Services.Common.ServiceClient clientData) {
            return base.Channel.UnsibscribeAsync(clientData);
        }
        
        public string ClientPing(string yourName) {
            return base.Channel.ClientPing(yourName);
        }
        
        public System.Threading.Tasks.Task<string> ClientPingAsync(string yourName) {
            return base.Channel.ClientPingAsync(yourName);
        }
    }
}
