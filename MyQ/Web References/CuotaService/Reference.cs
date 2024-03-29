﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace MyQ.CuotaService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="InetCuotasWSBinding", Namespace="urn:InetCuotasWS")]
    public partial class InetCuotasWSService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ObtenerCuotaOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public InetCuotasWSService() {
            this.Url = global::MyQ.Properties.Settings.Default.MyQ_CuotaService_InetCuotasWSService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ObtenerCuotaCompletedEventHandler ObtenerCuotaCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:InetCuotasWSAction", RequestNamespace="urn:InetCuotasWS", ResponseNamespace="urn:InetCuotasWS")]
        [return: System.Xml.Serialization.SoapElementAttribute("ObtenerCuotaReturn")]
        public Usuario ObtenerCuota(string usuario, string clave, string dominio) {
            object[] results = this.Invoke("ObtenerCuota", new object[] {
                        usuario,
                        clave,
                        dominio});
            return ((Usuario)(results[0]));
        }
        
        /// <remarks/>
        public void ObtenerCuotaAsync(string usuario, string clave, string dominio) {
            this.ObtenerCuotaAsync(usuario, clave, dominio, null);
        }
        
        /// <remarks/>
        public void ObtenerCuotaAsync(string usuario, string clave, string dominio, object userState) {
            if ((this.ObtenerCuotaOperationCompleted == null)) {
                this.ObtenerCuotaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerCuotaOperationCompleted);
            }
            this.InvokeAsync("ObtenerCuota", new object[] {
                        usuario,
                        clave,
                        dominio}, this.ObtenerCuotaOperationCompleted, userState);
        }
        
        private void OnObtenerCuotaOperationCompleted(object arg) {
            if ((this.ObtenerCuotaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ObtenerCuotaCompleted(this, new ObtenerCuotaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="urn:InetCuotasWS")]
    public partial class Usuario {
        
        private float cuotaField;
        
        private object cuota_usadaField;
        
        private string nivel_navegacionField;
        
        private string usuarioField;
        
        /// <remarks/>
        public float cuota {
            get {
                return this.cuotaField;
            }
            set {
                this.cuotaField = value;
            }
        }
        
        /// <remarks/>
        public object cuota_usada {
            get {
                return this.cuota_usadaField;
            }
            set {
                this.cuota_usadaField = value;
            }
        }
        
        /// <remarks/>
        public string nivel_navegacion {
            get {
                return this.nivel_navegacionField;
            }
            set {
                this.nivel_navegacionField = value;
            }
        }
        
        /// <remarks/>
        public string usuario {
            get {
                return this.usuarioField;
            }
            set {
                this.usuarioField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void ObtenerCuotaCompletedEventHandler(object sender, ObtenerCuotaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ObtenerCuotaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ObtenerCuotaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Usuario Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Usuario)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591