﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;
namespace ExternalVendors.ClearStarVendor.xsds.ClearstarGateway
{
}

namespace ExternalVendors.ClearStarVendor.xsds.ClearstarGateway
{
}

namespace ExternalVendors.ClearStarVendor.xsds.ClearstarGateway
{
}

namespace ExternalVendors.ClearStarVendor.xsds.ClearstarGateway
{
}

namespace ExternalVendors.xsds.ClearstarGateway
{
}

namespace ExternalVendors.xsds.ClearstarGateway
{
}

namespace ExternalVendors.xsds.ClearstarGateway
{
}

namespace ExternalVendors.xsds.ClearstarGateway
{
}

namespace ExternalVendors.xsds.ClearstarGateway
{
}

namespace ExternalVendors.xsds.ClearstarGateway
{
}

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class GetInvoices {
    
    private object[] itemsField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ErrorStatus", typeof(GetInvoicesErrorStatus), Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    [System.Xml.Serialization.XmlElementAttribute("Invoice", typeof(GetInvoicesInvoice), Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public object[] Items {
        get {
            return this.itemsField;
        }
        set {
            this.itemsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class GetInvoicesErrorStatus {
    
    private string codeField;
    
    private string typeField;
    
    private string messageField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Code {
        get {
            return this.codeField;
        }
        set {
            this.codeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Type {
        get {
            return this.typeField;
        }
        set {
            this.typeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Message {
        get {
            return this.messageField;
        }
        set {
            this.messageField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class GetInvoicesInvoice {
    
    private string iInvoiceNoField;
    
    private string sInvoiceDateField;
    
    private string iProfilesField;
    
    private string iOrdersField;
    
    private string sInvPeriodBeginField;
    
    private string sInvPeriodEndField;
    
    private string sFeesField;
    
    private string sAdjustmentsField;
    
    private string sSubTotalField;
    
    private string sTaxField;
    
    private string sTotalField;
    
    private string sBalanceField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string iInvoiceNo {
        get {
            return this.iInvoiceNoField;
        }
        set {
            this.iInvoiceNoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string sInvoiceDate {
        get {
            return this.sInvoiceDateField;
        }
        set {
            this.sInvoiceDateField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string iProfiles {
        get {
            return this.iProfilesField;
        }
        set {
            this.iProfilesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string iOrders {
        get {
            return this.iOrdersField;
        }
        set {
            this.iOrdersField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string sInvPeriodBegin {
        get {
            return this.sInvPeriodBeginField;
        }
        set {
            this.sInvPeriodBeginField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string sInvPeriodEnd {
        get {
            return this.sInvPeriodEndField;
        }
        set {
            this.sInvPeriodEndField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string sFees {
        get {
            return this.sFeesField;
        }
        set {
            this.sFeesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string sAdjustments {
        get {
            return this.sAdjustmentsField;
        }
        set {
            this.sAdjustmentsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string sSubTotal {
        get {
            return this.sSubTotalField;
        }
        set {
            this.sSubTotalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string sTax {
        get {
            return this.sTaxField;
        }
        set {
            this.sTaxField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string sTotal {
        get {
            return this.sTotalField;
        }
        set {
            this.sTotalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string sBalance {
        get {
            return this.sBalanceField;
        }
        set {
            this.sBalanceField = value;
        }
    }
}