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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://FormFox.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://FormFox.com", IsNullable = false)]
    public partial class ResultOrderStatus
    {

        private string sendingFacilityField;

        private string referenceTestIDField;

        private string collectionStatusField;

        private string collectionStatusNotesField;

        /// <remarks/>
        public string SendingFacility
        {
            get
            {
                return this.sendingFacilityField;
            }
            set
            {
                this.sendingFacilityField = value;
            }
        }

        /// <remarks/>
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
        public string CollectionStatus
        {
            get
            {
                return this.collectionStatusField;
            }
            set
            {
                this.collectionStatusField = value;
            }
        }

        /// <remarks/>
        public string CollectionStatusNotes
        {
            get
            {
                return this.collectionStatusNotesField;
            }
            set
            {
                this.collectionStatusNotesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://FormFox.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://FormFox.com", IsNullable = false)]
    public partial class NewDataSet
    {

        private ResultOrderStatus[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ResultOrderStatus")]
        public ResultOrderStatus[] Items
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
}