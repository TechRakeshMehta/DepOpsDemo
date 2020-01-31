using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceItemAttributeContract
    {
        public Int32 CIA_ID { get; set; }
        public Int32 CIA_ItemID { get; set; }
        public Int32 CIA_AttributeID { get; set; }
        public Int32 CIA_DisplayOrder { get; set; }
        public Boolean CIA_IsActive { get; set; }
        public Boolean CIA_IsDeleted { get; set; }
        public Int32 CIA_CreatedByID { get; set; }
        public DateTime CIA_CreatedOn { get; set; }
        public Int32? CIA_ModifiedByID { get; set; }
        public DateTime? CIA_ModifiedOn { get; set; }
        public Boolean CIA_IsCreatedByAdmin { get; set; }

        public ComplianceItemAttribute TranslateToEntity()
        {
            return new ComplianceItemAttribute()
            {
                CIA_ID = this.CIA_ID,
                CIA_ItemID = this.CIA_ItemID,
                CIA_AttributeID = this.CIA_AttributeID,
                CIA_DisplayOrder = this.CIA_DisplayOrder,
                CIA_IsActive = this.CIA_IsActive,
                CIA_IsDeleted = this.CIA_IsDeleted,
                CIA_CreatedByID = this.CIA_CreatedByID,
                CIA_CreatedOn = this.CIA_CreatedOn,
                CIA_ModifiedByID = this.CIA_ModifiedByID,
                CIA_ModifiedOn = this.CIA_ModifiedOn,
                CIA_IsCreatedByAdmin=this.CIA_IsCreatedByAdmin
            };
        }
    }
}
