using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementFieldContract
    {
        //[DataMember]
        //public Boolean? IsEditableByApplicant { get; set; }
        [DataMember]
        public Boolean IsCalculatedAttribute { get; set; }
        [DataMember]
        public Int32 RequirementPackageID { get; set; }
        //[DataMember]
        //public Dictionary<String, Boolean> SelectedEditableBy { get; set; }
        [DataMember]
        public Int32 RequirementFieldID { get; set; }

        [DataMember]
        public Int32 RequirementItemFieldID { get; set; }
        //[DataMember]
        //public Boolean IsCustomSetting { get; set; }
        [DataMember]
        public String RequirementFieldName { get; set; }

        [DataMember]
        public Nullable<Int32> RequirementFieldMaxLength { get; set; }

        [DataMember]
        public String RequirementFieldDescription { get; set; }
        [DataMember]
        public String RequirementFieldDataTypeCode { get; set; }
        [DataMember]
        public String RequirementFieldLabel { get; set; }

        [DataMember]
        public Boolean IsFieldRequired { get; set; }

        [DataMember]
        public Guid RequirementFieldCode { get; set; }

        [DataMember]
        public Int32 RequirementItemID { get; set; }

        /// <summary>
        /// It denotes whether this field is the one in which data is being entered currently.
        /// </summary>
        [DataMember]
        public Boolean IsCurrentField { get; set; }
        [DataMember]
        public Boolean IsUpdated { get; set; }
        [DataMember]
        public Boolean IsNewField { get; set; }
        [DataMember]
        public Boolean IsDeleted { get; set; }
        [DataMember]
        public RequirementFieldDataContract RequirementFieldData { get; set; }
        [DataMember]
        public Int32 FieldObjectTreeID { get; set; }

        /// <summary>
        /// UAT-2164, Agency User - Granular Permissions
        /// </summary>
        [DataMember]
        public Boolean IsBackgroundDocument { get; set; }

        [DataMember]
        public bool IsFieldRuleNotDefined { get; set; }

        [DataMember]
        public UniversalAttributeContract UniversalAttributeData { get; set; }


        [DataMember]
        public String RequirementFieldFixedRuleTypeCode { get; set; }

        //UAT-2366
        [DataMember]
        public Int32? UiRequirementItemID { get; set; }

        [DataMember]
        public Int32? UiRequirementFieldID { get; set; }

        [DataMember]
        public String RequirementFieldUIRuleTypeCode { get; set; }

        [DataMember]
        public String UiRuleErrorMessage { get; set; }

        #region UAT-2213
        //UAT-2213:New Rotation Package Process: Master Setup
        [DataMember]
        public Boolean IsNewPackage { get; set; }
        [DataMember]
        public Int32 RequirementCategoryID { get; set; }
        #endregion

        #region UAT-2788
        [DataMember]
        public Int32? AttributeTypeID { get; set; }
        [DataMember]
        public String AttributeTypeCode { get; set; }
        #endregion

        #region UAT-3078
        [DataMember]
        public Int32 RequirementFieldDisplayOrder { get; set; }
        #endregion

        #region UAT-3176
        [DataMember]
        public Int32 RequirementFieldAttributeGroupId { get; set; }
        #endregion

        #region UAT-4380
        [DataMember]
        public Dictionary<String, Boolean> SelectedEditableBy { get; set; }
        [DataMember]
        public Boolean IsCustomSetting { get; set; }

        [DataMember]
        public RequirementObjectPropertiesContract RequirementObjectProperties { get; set; }
        [DataMember]
        public Boolean? IsEditableByApplicant { get; set; }
        #endregion
    }


    [Serializable]
    [DataContract]
    public class RequirementFieldType
    {
        [DataMember]
        public Int32? ComplianceAttributeTypeID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String Code { get; set; }
    }
    [Serializable]
    [DataContract]
    public class RequirementAttributeGroups
    {
        [DataMember]
        public Int32? RequirementAttributeGroupID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public Guid Code { get; set; }

        [DataMember]
        public String Label { get; set; }
    }
    [Serializable]
    [DataContract]
    public class RequirementDocumentAcroFieldType
    {
        [DataMember]
        public Int32 DocumentAcroFieldTypeID { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String Code { get; set; }
    }
}




