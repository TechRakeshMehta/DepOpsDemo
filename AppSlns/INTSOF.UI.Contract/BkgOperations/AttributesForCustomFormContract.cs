using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    [DataContract]
    public class AttributesForCustomFormContract
    {
        [DataMember]
        public Int32 PackageID { get; set; }
        [DataMember]
        public Int32 AtrributeGroupMappingId { get; set; }
        [DataMember]
        public Int32 ParentAttributeGroupMappingId { get; set; }  //UAT 3521 
        [DataMember]
        public String ValidateExpression { get; set; }  //UAT 3521
        [DataMember]
        public String ValidationMessage { get; set; }  //UAT 3521
        [DataMember]
        public Int32 AttributeGroupId { get; set; }
        [DataMember]
        public String AttriButeGroupName { get; set; }
        [DataMember]
        public Int32 AttributeId { get; set; }
        [DataMember]
        public String AttributeName { get; set; }
        [DataMember]
        public String AttributeType { get; set; }
        [DataMember]
        public Boolean IsDisplay { get; set; }
        [DataMember]
        public Boolean IsRequired { get; set; }
        [DataMember]
        // public Int32 CustomFormId { get; set; }
        public String SectionTitle { get; set; }
        [DataMember]
        public Int32 CustomFieldsDisplaySequence { get; set; }
        [DataMember]
        public String CustomHtml { get; set; }
        [DataMember]
        public String AttributeTypeCode { get; set; }
        [DataMember]
        public Int32 DisplayColumns { get; set; }
        [DataMember]
        public Int32 Sequence { get; set; }
        [DataMember]
        public Int32 Occurence { get; set; }
        [DataMember]
        public Int32 MinimumOccurence { get; set; }
        [DataMember]
        public Int32 MaximumOccurence { get; set; }
        [DataMember]
        public String AttributeGroupMappingCode { get; set; }
        [DataMember]
        public String MaximumValue { get; set; }
        [DataMember]
        public String MinimumValue { get; set; }
        [DataMember]
        public String InstructionText { get; set; }
        [DataMember]
        public String AttributeCode { get; set; }
        [DataMember]
        public Boolean IsDecisionField { get; set; }
        [DataMember]
        #region UAT-586 WB: AMS: When adding supplemental services to an order, need to be able to see what the applicant has already entered (location, alias, etc.)
        public String AttributeDataValue { get; set; }
        [DataMember]
        #endregion
        public Int32 InstanceID { get; set; }
        [DataMember]
        public Boolean IsHiddenFromUI { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String BkgSvcAttributeGroupCode { get; set; }

        [DataMember]
        public String ServiceTypeCode { get; set; }

        [DataMember]
        public String DisplayName { get; set; }
    }
}
