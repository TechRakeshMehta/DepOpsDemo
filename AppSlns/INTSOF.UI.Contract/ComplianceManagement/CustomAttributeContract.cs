using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class CustomAttributeContract
    {
        public Int32 CA_CustomAttributeID { get; set; }
        public String CA_AttributeName { get; set; }
        public String CA_AttributeLabel { get; set; }
        public String CA_Description { get; set; }
        public Int32 CA_CustomAttributeDataTypeID { get; set; }
        public Int32 CA_CustomAttributeUseTypeID { get; set; }
        public Int32? CA_StringLength { get; set; }
        public Boolean CA_IsDeleted { get; set; }
        public Boolean CA_IsActive { get; set; }
        public Int32 CA_CreatedByID { get; set; }
        public DateTime CA_CreatedOn { get; set; }
        public Int32? CA_ModifiedByID { get; set; }
        public DateTime? CA_ModifiedOn { get; set; }
        public String CA_RegularExpression { get; set; }
        public String CA_RegExpErrorMsg { get; set; }
        public Boolean? CA_IsRequired { get; set; }

        public Int32? CA_RelatedCustomAttributeID { get; set; }
        public Boolean? CA_DisplayInSearchFilter { get; set; }
        public Boolean CA_ShowInPendingComProfilesGrid { get; set; }
        public Boolean CA_IncludeInNotification { get; set; }
        public CustomAttribute TranslateToEntity()
        {
            return new CustomAttribute()
            {
                CA_CustomAttributeID = this.CA_CustomAttributeID,
                CA_AttributeName = this.CA_AttributeName,
                CA_AttributeLabel = this.CA_AttributeLabel,
                CA_Description = this.CA_Description,
                CA_CustomAttributeDataTypeID = this.CA_CustomAttributeDataTypeID,
                CA_CustomAttributeUseTypeID = this.CA_CustomAttributeUseTypeID,
                CA_StringLength = this.CA_StringLength,
                CA_IsDeleted = this.CA_IsDeleted,
                CA_IsActive = this.CA_IsActive,
                CA_CreatedByID = this.CA_CreatedByID,
                CA_CreatedOn = this.CA_CreatedOn,
                CA_ModifiedByID = this.CA_ModifiedByID,
                CA_ModifiedOn = this.CA_ModifiedOn,
                //UAT-1068 Ability to configure specific custom field formats
                CA_RegularExpression = this.CA_RegularExpression,
                CA_RegExpErrorMsg = this.CA_RegExpErrorMsg,
                CA_IsRequired = this.CA_IsRequired,
                CA_RelatedCustomAttributeId = this.CA_RelatedCustomAttributeID,
                CA_DisplayInSearchFilter = this.CA_DisplayInSearchFilter,
                CA_ShowInPendingComProfilesGrid =this.CA_ShowInPendingComProfilesGrid,
                CA_IncludeInNotification = this.CA_IncludeInNotification
            };
        }
    }
}
