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

namespace ExternalVendors.ClearstarClearMD {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="DonorCCFSoap", Namespace="http://tempuri.org")]
    public partial class DonorCCF : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetDonorCCFOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetCCFPDFOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetFormPDFOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTestingAuthorityListOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTestReasonListOperationCompleted;
        
        private System.Threading.SendOrPostCallback CopyRegistrationOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendEmailForNewRegistrationOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetSitesOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public DonorCCF() {
            this.Url = global::ExternalVendors.Properties.Settings.Default.ExternalVendors_ClearstarClearMD_DonorCCF;
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
        public event GetDonorCCFCompletedEventHandler GetDonorCCFCompleted;
        
        /// <remarks/>
        public event GetCCFPDFCompletedEventHandler GetCCFPDFCompleted;
        
        /// <remarks/>
        public event GetFormPDFCompletedEventHandler GetFormPDFCompleted;
        
        /// <remarks/>
        public event GetTestingAuthorityListCompletedEventHandler GetTestingAuthorityListCompleted;
        
        /// <remarks/>
        public event GetTestReasonListCompletedEventHandler GetTestReasonListCompleted;
        
        /// <remarks/>
        public event CopyRegistrationCompletedEventHandler CopyRegistrationCompleted;
        
        /// <remarks/>
        public event SendEmailForNewRegistrationCompletedEventHandler SendEmailForNewRegistrationCompleted;
        
        /// <remarks/>
        public event GetSitesCompletedEventHandler GetSitesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetDonorCCF", RequestNamespace="http://tempuri.org", ResponseNamespace="http://tempuri.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode GetDonorCCF(int BOID, string CustID, string username, string password, string DonorID) {
            object[] results = this.Invoke("GetDonorCCF", new object[] {
                        BOID,
                        CustID,
                        username,
                        password,
                        DonorID});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void GetDonorCCFAsync(int BOID, string CustID, string username, string password, string DonorID) {
            this.GetDonorCCFAsync(BOID, CustID, username, password, DonorID, null);
        }
        
        /// <remarks/>
        public void GetDonorCCFAsync(int BOID, string CustID, string username, string password, string DonorID, object userState) {
            if ((this.GetDonorCCFOperationCompleted == null)) {
                this.GetDonorCCFOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetDonorCCFOperationCompleted);
            }
            this.InvokeAsync("GetDonorCCF", new object[] {
                        BOID,
                        CustID,
                        username,
                        password,
                        DonorID}, this.GetDonorCCFOperationCompleted, userState);
        }
        
        private void OnGetDonorCCFOperationCompleted(object arg) {
            if ((this.GetDonorCCFCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetDonorCCFCompleted(this, new GetDonorCCFCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetCCFPDF", RequestNamespace="http://tempuri.org", ResponseNamespace="http://tempuri.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode GetCCFPDF(int BOID, string CustID, string username, string password, string RegistrationID) {
            object[] results = this.Invoke("GetCCFPDF", new object[] {
                        BOID,
                        CustID,
                        username,
                        password,
                        RegistrationID});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void GetCCFPDFAsync(int BOID, string CustID, string username, string password, string RegistrationID) {
            this.GetCCFPDFAsync(BOID, CustID, username, password, RegistrationID, null);
        }
        
        /// <remarks/>
        public void GetCCFPDFAsync(int BOID, string CustID, string username, string password, string RegistrationID, object userState) {
            if ((this.GetCCFPDFOperationCompleted == null)) {
                this.GetCCFPDFOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCCFPDFOperationCompleted);
            }
            this.InvokeAsync("GetCCFPDF", new object[] {
                        BOID,
                        CustID,
                        username,
                        password,
                        RegistrationID}, this.GetCCFPDFOperationCompleted, userState);
        }
        
        private void OnGetCCFPDFOperationCompleted(object arg) {
            if ((this.GetCCFPDFCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCCFPDFCompleted(this, new GetCCFPDFCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetFormPDF", RequestNamespace="http://tempuri.org", ResponseNamespace="http://tempuri.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode GetFormPDF(int BOID, string CustID, string username, string password, int type, string testID, string callbackUrl) {
            object[] results = this.Invoke("GetFormPDF", new object[] {
                        BOID,
                        CustID,
                        username,
                        password,
                        type,
                        testID,
                        callbackUrl});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void GetFormPDFAsync(int BOID, string CustID, string username, string password, int type, string testID, string callbackUrl) {
            this.GetFormPDFAsync(BOID, CustID, username, password, type, testID, callbackUrl, null);
        }
        
        /// <remarks/>
        public void GetFormPDFAsync(int BOID, string CustID, string username, string password, int type, string testID, string callbackUrl, object userState) {
            if ((this.GetFormPDFOperationCompleted == null)) {
                this.GetFormPDFOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetFormPDFOperationCompleted);
            }
            this.InvokeAsync("GetFormPDF", new object[] {
                        BOID,
                        CustID,
                        username,
                        password,
                        type,
                        testID,
                        callbackUrl}, this.GetFormPDFOperationCompleted, userState);
        }
        
        private void OnGetFormPDFOperationCompleted(object arg) {
            if ((this.GetFormPDFCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetFormPDFCompleted(this, new GetFormPDFCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetTestingAuthorityList", RequestNamespace="http://tempuri.org", ResponseNamespace="http://tempuri.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public TestingAuthority[] GetTestingAuthorityList(int BOID, string username, string password) {
            object[] results = this.Invoke("GetTestingAuthorityList", new object[] {
                        BOID,
                        username,
                        password});
            return ((TestingAuthority[])(results[0]));
        }
        
        /// <remarks/>
        public void GetTestingAuthorityListAsync(int BOID, string username, string password) {
            this.GetTestingAuthorityListAsync(BOID, username, password, null);
        }
        
        /// <remarks/>
        public void GetTestingAuthorityListAsync(int BOID, string username, string password, object userState) {
            if ((this.GetTestingAuthorityListOperationCompleted == null)) {
                this.GetTestingAuthorityListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTestingAuthorityListOperationCompleted);
            }
            this.InvokeAsync("GetTestingAuthorityList", new object[] {
                        BOID,
                        username,
                        password}, this.GetTestingAuthorityListOperationCompleted, userState);
        }
        
        private void OnGetTestingAuthorityListOperationCompleted(object arg) {
            if ((this.GetTestingAuthorityListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTestingAuthorityListCompleted(this, new GetTestingAuthorityListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetTestReasonList", RequestNamespace="http://tempuri.org", ResponseNamespace="http://tempuri.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode GetTestReasonList(int BOID, string username, string password) {
            object[] results = this.Invoke("GetTestReasonList", new object[] {
                        BOID,
                        username,
                        password});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void GetTestReasonListAsync(int BOID, string username, string password) {
            this.GetTestReasonListAsync(BOID, username, password, null);
        }
        
        /// <remarks/>
        public void GetTestReasonListAsync(int BOID, string username, string password, object userState) {
            if ((this.GetTestReasonListOperationCompleted == null)) {
                this.GetTestReasonListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTestReasonListOperationCompleted);
            }
            this.InvokeAsync("GetTestReasonList", new object[] {
                        BOID,
                        username,
                        password}, this.GetTestReasonListOperationCompleted, userState);
        }
        
        private void OnGetTestReasonListOperationCompleted(object arg) {
            if ((this.GetTestReasonListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTestReasonListCompleted(this, new GetTestReasonListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CopyRegistration", RequestNamespace="http://tempuri.org", ResponseNamespace="http://tempuri.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode CopyRegistration(int BOID, string username, string password, System.DateTime utcExpiration, string registrationID) {
            object[] results = this.Invoke("CopyRegistration", new object[] {
                        BOID,
                        username,
                        password,
                        utcExpiration,
                        registrationID});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void CopyRegistrationAsync(int BOID, string username, string password, System.DateTime utcExpiration, string registrationID) {
            this.CopyRegistrationAsync(BOID, username, password, utcExpiration, registrationID, null);
        }
        
        /// <remarks/>
        public void CopyRegistrationAsync(int BOID, string username, string password, System.DateTime utcExpiration, string registrationID, object userState) {
            if ((this.CopyRegistrationOperationCompleted == null)) {
                this.CopyRegistrationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCopyRegistrationOperationCompleted);
            }
            this.InvokeAsync("CopyRegistration", new object[] {
                        BOID,
                        username,
                        password,
                        utcExpiration,
                        registrationID}, this.CopyRegistrationOperationCompleted, userState);
        }
        
        private void OnCopyRegistrationOperationCompleted(object arg) {
            if ((this.CopyRegistrationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CopyRegistrationCompleted(this, new CopyRegistrationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendEmailForNewRegistration", RequestNamespace="http://tempuri.org", ResponseNamespace="http://tempuri.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode SendEmailForNewRegistration(string username, string password, string customerID, string registrationID) {
            object[] results = this.Invoke("SendEmailForNewRegistration", new object[] {
                        username,
                        password,
                        customerID,
                        registrationID});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void SendEmailForNewRegistrationAsync(string username, string password, string customerID, string registrationID) {
            this.SendEmailForNewRegistrationAsync(username, password, customerID, registrationID, null);
        }
        
        /// <remarks/>
        public void SendEmailForNewRegistrationAsync(string username, string password, string customerID, string registrationID, object userState) {
            if ((this.SendEmailForNewRegistrationOperationCompleted == null)) {
                this.SendEmailForNewRegistrationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendEmailForNewRegistrationOperationCompleted);
            }
            this.InvokeAsync("SendEmailForNewRegistration", new object[] {
                        username,
                        password,
                        customerID,
                        registrationID}, this.SendEmailForNewRegistrationOperationCompleted, userState);
        }
        
        private void OnSendEmailForNewRegistrationOperationCompleted(object arg) {
            if ((this.SendEmailForNewRegistrationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendEmailForNewRegistrationCompleted(this, new SendEmailForNewRegistrationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetSites", RequestNamespace="http://tempuri.org", ResponseNamespace="http://tempuri.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode GetSites(string UserName, string Password, int BOID, string CustomerID, string Services, string zipCode) {
            object[] results = this.Invoke("GetSites", new object[] {
                        UserName,
                        Password,
                        BOID,
                        CustomerID,
                        Services,
                        zipCode});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void GetSitesAsync(string UserName, string Password, int BOID, string CustomerID, string Services, string zipCode) {
            this.GetSitesAsync(UserName, Password, BOID, CustomerID, Services, zipCode, null);
        }
        
        /// <remarks/>
        public void GetSitesAsync(string UserName, string Password, int BOID, string CustomerID, string Services, string zipCode, object userState) {
            if ((this.GetSitesOperationCompleted == null)) {
                this.GetSitesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetSitesOperationCompleted);
            }
            this.InvokeAsync("GetSites", new object[] {
                        UserName,
                        Password,
                        BOID,
                        CustomerID,
                        Services,
                        zipCode}, this.GetSitesOperationCompleted, userState);
        }
        
        private void OnGetSitesOperationCompleted(object arg) {
            if ((this.GetSitesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetSitesCompleted(this, new GetSitesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3062.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org")]
    public partial class TestingAuthority {
        
        private string codeField;
        
        private string descriptionField;
        
        /// <remarks/>
        public string Code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void GetDonorCCFCompletedEventHandler(object sender, GetDonorCCFCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetDonorCCFCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetDonorCCFCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void GetCCFPDFCompletedEventHandler(object sender, GetCCFPDFCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCCFPDFCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCCFPDFCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void GetFormPDFCompletedEventHandler(object sender, GetFormPDFCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetFormPDFCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetFormPDFCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void GetTestingAuthorityListCompletedEventHandler(object sender, GetTestingAuthorityListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTestingAuthorityListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTestingAuthorityListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TestingAuthority[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TestingAuthority[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void GetTestReasonListCompletedEventHandler(object sender, GetTestReasonListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTestReasonListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTestReasonListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void CopyRegistrationCompletedEventHandler(object sender, CopyRegistrationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CopyRegistrationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CopyRegistrationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void SendEmailForNewRegistrationCompletedEventHandler(object sender, SendEmailForNewRegistrationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendEmailForNewRegistrationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendEmailForNewRegistrationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    public delegate void GetSitesCompletedEventHandler(object sender, GetSitesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3062.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetSitesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetSitesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591