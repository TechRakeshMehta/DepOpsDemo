﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;
namespace ExternalVendors.ClearStarVendor.xsds.QuestDiagnostics
{
    // 
    // This source code was auto-generated by xsd, Version=4.0.30319.1.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.formfox.com/ffordersvc")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.formfox.com/ffordersvc", IsNullable = false)]
    public partial class RemoveOrderResult
    {

        private RemoveOrderResultRemoveTestResult[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RemoveTestResult", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RemoveOrderResultRemoveTestResult[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RemoveOrderResultRemoveTestResult
    {

        private string referenceTestIDField;

        private string requestStatusField;

        private RemoveOrderResultRemoveTestResultErrorSummary[] errorSummaryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ReferenceTestID
        {
            get
            {
                return this.referenceTestIDField;
            }
            set
            {
                this.referenceTestIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string RequestStatus
        {
            get
            {
                return this.requestStatusField;
            }
            set
            {
                this.requestStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ErrorSummary", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RemoveOrderResultRemoveTestResultErrorSummary[] ErrorSummary
        {
            get
            {
                return this.errorSummaryField;
            }
            set
            {
                this.errorSummaryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RemoveOrderResultRemoveTestResultErrorSummary
    {

        private string errorDetailField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ErrorDetail
        {
            get
            {
                return this.errorDetailField;
            }
            set
            {
                this.errorDetailField = value;
            }
        }
    }

}