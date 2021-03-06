﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceClient", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Common")]
    [System.SerializableAttribute()]
    public partial class ServiceClient : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AliasField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IDField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Alias {
            get {
                return this.AliasField;
            }
            set {
                if ((object.ReferenceEquals(this.AliasField, value) != true)) {
                    this.AliasField = value;
                    this.RaisePropertyChanged("Alias");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ID {
            get {
                return this.IDField;
            }
            set {
                if ((object.ReferenceEquals(this.IDField, value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceUser", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Common")]
    [System.SerializableAttribute()]
    public partial class ServiceUser : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordHashField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TokenField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PasswordHash {
            get {
                return this.PasswordHashField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordHashField, value) != true)) {
                    this.PasswordHashField = value;
                    this.RaisePropertyChanged("PasswordHash");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Token {
            get {
                return this.TokenField;
            }
            set {
                if ((object.ReferenceEquals(this.TokenField, value) != true)) {
                    this.TokenField = value;
                    this.RaisePropertyChanged("Token");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserName {
            get {
                return this.UserNameField;
            }
            set {
                if ((object.ReferenceEquals(this.UserNameField, value) != true)) {
                    this.UserNameField = value;
                    this.RaisePropertyChanged("UserName");
                }
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TrackPadMessage", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Common.Modules.Touch")]
    [System.SerializableAttribute()]
    public partial class TrackPadMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double AngleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private EveControl.RelayServiceReference.TrackPadMessage.TrackPadCommands CommandField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private EveControl.RelayServiceReference.Orientation DirectionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double DistanceRatioField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double XField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double YField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Angle {
            get {
                return this.AngleField;
            }
            set {
                if ((this.AngleField.Equals(value) != true)) {
                    this.AngleField = value;
                    this.RaisePropertyChanged("Angle");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public EveControl.RelayServiceReference.TrackPadMessage.TrackPadCommands Command {
            get {
                return this.CommandField;
            }
            set {
                if ((this.CommandField.Equals(value) != true)) {
                    this.CommandField = value;
                    this.RaisePropertyChanged("Command");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public EveControl.RelayServiceReference.Orientation Direction {
            get {
                return this.DirectionField;
            }
            set {
                if ((this.DirectionField.Equals(value) != true)) {
                    this.DirectionField = value;
                    this.RaisePropertyChanged("Direction");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double DistanceRatio {
            get {
                return this.DistanceRatioField;
            }
            set {
                if ((this.DistanceRatioField.Equals(value) != true)) {
                    this.DistanceRatioField = value;
                    this.RaisePropertyChanged("DistanceRatio");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double X {
            get {
                return this.XField;
            }
            set {
                if ((this.XField.Equals(value) != true)) {
                    this.XField = value;
                    this.RaisePropertyChanged("X");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Y {
            get {
                return this.YField;
            }
            set {
                if ((this.YField.Equals(value) != true)) {
                    this.YField = value;
                    this.RaisePropertyChanged("Y");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
        [System.Runtime.Serialization.DataContractAttribute(Name="TrackPadMessage.TrackPadCommands", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Common.Modules.Touch")]
        public enum TrackPadCommands : int {
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Tap = 0,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Hold = 1,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            DoubleTap = 2,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Flick = 3,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            DragStarted = 4,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            DragDelta = 5,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            DragCompleted = 6,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            PinchStarted = 7,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            PinchDelta = 8,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            PinchCompleted = 9,
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.FlagsAttribute()]
    [System.Runtime.Serialization.DataContractAttribute(Name="Orientation", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Common.Modules")]
    public enum Orientation : uint {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Horizontal = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Vertical = 1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ButtonMessage", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Common.Modules.Touch")]
    [System.SerializableAttribute()]
    public partial class ButtonMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private EveControl.RelayServiceReference.ButtonMessage.Buttons ButtonField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private EveControl.RelayServiceReference.ButtonMessage.ButtonCommands CommandField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public EveControl.RelayServiceReference.ButtonMessage.Buttons Button {
            get {
                return this.ButtonField;
            }
            set {
                if ((this.ButtonField.Equals(value) != true)) {
                    this.ButtonField = value;
                    this.RaisePropertyChanged("Button");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public EveControl.RelayServiceReference.ButtonMessage.ButtonCommands Command {
            get {
                return this.CommandField;
            }
            set {
                if ((this.CommandField.Equals(value) != true)) {
                    this.CommandField = value;
                    this.RaisePropertyChanged("Command");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
        [System.FlagsAttribute()]
        [System.Runtime.Serialization.DataContractAttribute(Name="ButtonMessage.Buttons", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Common.Modules.Touch")]
        public enum Buttons : uint {
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Left = 1,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Right = 2,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Middle = 3,
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
        [System.FlagsAttribute()]
        [System.Runtime.Serialization.DataContractAttribute(Name="ButtonMessage.ButtonCommands", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Common.Modules.Touch")]
        public enum ButtonCommands : uint {
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Tap = 1,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Hold = 2,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            DoubleTap = 4,
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Light", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Common.Modules.Lights")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(EveControl.RelayServiceReference.AmbientalLight))]
    public partial class Light : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AliasField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool StateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Alias {
            get {
                return this.AliasField;
            }
            set {
                if ((object.ReferenceEquals(this.AliasField, value) != true)) {
                    this.AliasField = value;
                    this.RaisePropertyChanged("Alias");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool State {
            get {
                return this.StateField;
            }
            set {
                if ((this.StateField.Equals(value) != true)) {
                    this.StateField = value;
                    this.RaisePropertyChanged("State");
                }
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AmbientalLight", Namespace="http://schemas.datacontract.org/2004/07/Eve.API.Services.Common.Modules.Ambiental" +
        "")]
    [System.SerializableAttribute()]
    public partial class AmbientalLight : EveControl.RelayServiceReference.Light {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte AValueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte BValueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte GValueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte RValueField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte AValue {
            get {
                return this.AValueField;
            }
            set {
                if ((this.AValueField.Equals(value) != true)) {
                    this.AValueField = value;
                    this.RaisePropertyChanged("AValue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte BValue {
            get {
                return this.BValueField;
            }
            set {
                if ((this.BValueField.Equals(value) != true)) {
                    this.BValueField = value;
                    this.RaisePropertyChanged("BValue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte GValue {
            get {
                return this.GValueField;
            }
            set {
                if ((this.GValueField.Equals(value) != true)) {
                    this.GValueField = value;
                    this.RaisePropertyChanged("GValue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte RValue {
            get {
                return this.RValueField;
            }
            set {
                if ((this.RValueField.Equals(value) != true)) {
                    this.RValueField = value;
                    this.RaisePropertyChanged("RValue");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RelayServiceReference.IClientRelayService", CallbackContract=typeof(EveControl.RelayServiceReference.IClientRelayServiceCallback))]
    public interface IClientRelayService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/Subscribe", ReplyAction="http://tempuri.org/IClientRelayService/SubscribeResponse")]
        bool Subscribe(EveControl.RelayServiceReference.ServiceClient clientData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/Subscribe", ReplyAction="http://tempuri.org/IClientRelayService/SubscribeResponse")]
        System.Threading.Tasks.Task<bool> SubscribeAsync(EveControl.RelayServiceReference.ServiceClient clientData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/Unsibscribe", ReplyAction="http://tempuri.org/IClientRelayService/UnsibscribeResponse")]
        bool Unsibscribe(EveControl.RelayServiceReference.ServiceClient clientData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/Unsibscribe", ReplyAction="http://tempuri.org/IClientRelayService/UnsibscribeResponse")]
        System.Threading.Tasks.Task<bool> UnsibscribeAsync(EveControl.RelayServiceReference.ServiceClient clientData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/ClientPing", ReplyAction="http://tempuri.org/IClientRelayService/ClientPingResponse")]
        string ClientPing(string yourName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/ClientPing", ReplyAction="http://tempuri.org/IClientRelayService/ClientPingResponse")]
        System.Threading.Tasks.Task<string> ClientPingAsync(string yourName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IClientRelayServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SignIn", ReplyAction="http://tempuri.org/IClientRelayService/SignInResponse")]
        bool SignIn(EveControl.RelayServiceReference.ServiceUser user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SignOut", ReplyAction="http://tempuri.org/IClientRelayService/SignOutResponse")]
        bool SignOut(EveControl.RelayServiceReference.ServiceUser user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/GetAvailableClients", ReplyAction="http://tempuri.org/IClientRelayService/GetAvailableClientsResponse")]
        EveControl.RelayServiceReference.ServiceClient[] GetAvailableClients();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SendTrackPadMessage", ReplyAction="http://tempuri.org/IClientRelayService/SendTrackPadMessageResponse")]
        bool SendTrackPadMessage(string client, EveControl.RelayServiceReference.TrackPadMessage message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SendButtonMessage", ReplyAction="http://tempuri.org/IClientRelayService/SendButtonMessageResponse")]
        bool SendButtonMessage(string client, EveControl.RelayServiceReference.ButtonMessage message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SetZoom", ReplyAction="http://tempuri.org/IClientRelayService/SetZoomResponse")]
        bool SetZoom(string client, int zoomValue);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/GetLights", ReplyAction="http://tempuri.org/IClientRelayService/GetLightsResponse")]
        EveControl.RelayServiceReference.Light[] GetLights(string client);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SetLightState", ReplyAction="http://tempuri.org/IClientRelayService/SetLightStateResponse")]
        bool SetLightState(string client, int id, bool state);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/GetAmbientalLights", ReplyAction="http://tempuri.org/IClientRelayService/GetAmbientalLightsResponse")]
        EveControl.RelayServiceReference.AmbientalLight[] GetAmbientalLights(string client);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SetAmbientalLightState", ReplyAction="http://tempuri.org/IClientRelayService/SetAmbientalLightStateResponse")]
        bool SetAmbientalLightState(string client, int id, bool state);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IClientRelayService/SetAmbientalLightColor", ReplyAction="http://tempuri.org/IClientRelayService/SetAmbientalLightColorResponse")]
        bool SetAmbientalLightColor(string client, int id, byte r, byte g, byte b, byte a);
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
        
        public bool Subscribe(EveControl.RelayServiceReference.ServiceClient clientData) {
            return base.Channel.Subscribe(clientData);
        }
        
        public System.Threading.Tasks.Task<bool> SubscribeAsync(EveControl.RelayServiceReference.ServiceClient clientData) {
            return base.Channel.SubscribeAsync(clientData);
        }
        
        public bool Unsibscribe(EveControl.RelayServiceReference.ServiceClient clientData) {
            return base.Channel.Unsibscribe(clientData);
        }
        
        public System.Threading.Tasks.Task<bool> UnsibscribeAsync(EveControl.RelayServiceReference.ServiceClient clientData) {
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
