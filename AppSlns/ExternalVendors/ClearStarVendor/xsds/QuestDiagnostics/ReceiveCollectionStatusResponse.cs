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
}

namespace ExternalVendors.ClearStarVendor.xsds.QuestDiagnostics
{
}

namespace ExternalVendors.ClearStarVendor.xsds.QuestDiagnostics
{
}

namespace ExternalVendors.ClearStarVendor.xsds.QuestDiagnostics
{
}

namespace ExternalVendors.xsds.QuestDiagnostics
{
}

namespace ExternalVendors.xsds.QuestDiagnostics
{
}

namespace ExternalVendors.xsds.QuestDiagnostics
{
}

namespace ExternalVendors.xsds.QuestDiagnostics
{
}

namespace ExternalVendors.xsds.QuestDiagnostics
{
}

namespace ExternalVendors.xsds.QuestDiagnostics
{
    // 
    // This source code was auto-generated by xsd, Version=4.0.30319.1.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ReceiveCollectionStatusResults
    {

        private string updateStatusField;

        private ReceiveCollectionStatusResultsErrorSummary[] errorSummaryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UpdateStatus
        {
            get
            {
                return this.updateStatusField;
            }
            set
            {
                this.updateStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ErrorSummary", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ReceiveCollectionStatusResultsErrorSummary[] ErrorSummary
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
    public partial class ReceiveCollectionStatusResultsErrorSummary
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