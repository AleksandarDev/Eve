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
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ClientState", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Contracts.Services")]
    [System.SerializableAttribute()]
    public partial class ClientState : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RelayServiceReference.IRelayService", CallbackContract=typeof(EveControl.RelayServiceReference.IRelayServiceCallback))]
    public interface IRelayService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRelayService/Subscribe")]
        void Subscribe();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRelayService/Subscribe")]
        System.Threading.Tasks.Task SubscribeAsync();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRelayService/Unsibscribe")]
        void Unsibscribe();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IRelayService/Unsibscribe")]
        System.Threading.Tasks.Task UnsibscribeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayService/UpdateClientState", ReplyAction="http://tempuri.org/IRelayService/UpdateClientStateResponse")]
        bool UpdateClientState(EveControl.RelayServiceReference.ClientState state);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayService/UpdateClientState", ReplyAction="http://tempuri.org/IRelayService/UpdateClientStateResponse")]
        System.Threading.Tasks.Task<bool> UpdateClientStateAsync(EveControl.RelayServiceReference.ClientState state);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayService/Ping", ReplyAction="http://tempuri.org/IRelayService/PingResponse")]
        string Ping(string yourName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayService/Ping", ReplyAction="http://tempuri.org/IRelayService/PingResponse")]
        System.Threading.Tasks.Task<string> PingAsync(string yourName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRelayServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayService/PingRequest", ReplyAction="http://tempuri.org/IRelayService/PingRequestResponse")]
        string PingRequest(string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRelayServiceChannel : EveControl.RelayServiceReference.IRelayService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RelayServiceClient : System.ServiceModel.DuplexClientBase<EveControl.RelayServiceReference.IRelayService>, EveControl.RelayServiceReference.IRelayService {
        
        public RelayServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public RelayServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public RelayServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public RelayServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public RelayServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void Subscribe() {
            base.Channel.Subscribe();
        }
        
        public System.Threading.Tasks.Task SubscribeAsync() {
            return base.Channel.SubscribeAsync();
        }
        
        public void Unsibscribe() {
            base.Channel.Unsibscribe();
        }
        
        public System.Threading.Tasks.Task UnsibscribeAsync() {
            return base.Channel.UnsibscribeAsync();
        }
        
        public bool UpdateClientState(EveControl.RelayServiceReference.ClientState state) {
            return base.Channel.UpdateClientState(state);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateClientStateAsync(EveControl.RelayServiceReference.ClientState state) {
            return base.Channel.UpdateClientStateAsync(state);
        }
        
        public string Ping(string yourName) {
            return base.Channel.Ping(yourName);
        }
        
        public System.Threading.Tasks.Task<string> PingAsync(string yourName) {
            return base.Channel.PingAsync(yourName);
        }
    }
}
