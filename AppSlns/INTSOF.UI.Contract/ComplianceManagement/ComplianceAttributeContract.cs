using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceAttributeContract
    {
        public Int32 ComplianceAttributeID { get; set; }
        public Int32 ComplianceAttributeTypeID { get; set; }
        public Int32 ComplianceAttributeDatatypeID { get; set; }
        public Int32? ComplianceAttributeGroupID { get; set; }
        public Int32? ComplianceViewDocumentID { get; set; }
        public Guid? CopiedFromCode { get; set; }
        public Guid Code { get; set; }
        public String Name { get; set; }
        public String ScreenLabel { get; set; }
        public String ExplanatoryNotes { get; set; }
        public String AttributeLabel { get; set; }
        public String Description { get; set; }
        public Int32? MaximumCharacters { get; set; }
        public Boolean IsDeleted { get; set; }
        public Boolean IsActive { get; set; }
        public Int32 CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int32? ModifiedByID { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Int32 TenantID { get; set; }
        public System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption> lstComplianceAttributeOption { get; set; }
        public System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceItemAttribute> lstComplianceItemAttribute { get; set; }
        public Boolean IsCreatedByAdmin { get; set; }
        public String InstructionText { get; set; }
        public Int32? ItemAttributeMappingID { get; set; }
        public Int32? AssignmentHierarchyID { get; set; }
        public Int32? PackageID { get; set; }
        public Int32? CatagoryID { get; set; }
        public Int32? loggedInUserId { get; set; }

        //UAT-2023:Reconciliation: Addition of attribute-level setting to enable/disable trigger for reconciliation queue.
        public Boolean IsTriggerReconciliation { get; set; }

        public Boolean IsSendForintegration { get; set; }

        public ComplianceAttribute TranslateToEntity()
        {
            ComplianceAttribute newAttribute = new ComplianceAttribute()
            {
                ComplianceAttributeID = this.ComplianceAttributeID,
                ComplianceAttributeTypeID = this.ComplianceAttributeTypeID,
                ComplianceAttributeDatatypeID = this.ComplianceAttributeDatatypeID,
                ComplianceAttributeGroupID = this.ComplianceAttributeGroupID,
                CopiedFromCode = this.CopiedFromCode,
                Code = this.Code,
                Name = this.Name,
                ScreenLabel = this.ScreenLabel,
                AttributeLabel = this.AttributeLabel,
                Description = this.Description,
                MaximumCharacters = this.MaximumCharacters,
                IsDeleted = this.IsDeleted,
                IsActive = this.IsActive,
                CreatedByID = this.CreatedByID,
                CreatedOn = this.CreatedOn,
                ModifiedByID = this.ModifiedByID,
                ModifiedOn = this.ModifiedOn,
                TenantID = this.TenantID,
                ComplianceAttributeOptions = this.lstComplianceAttributeOption,
                ComplianceItemAttributes = this.lstComplianceItemAttribute,
                IsCreatedByAdmin = this.IsCreatedByAdmin,
                IsTriggersReconciliation = this.IsTriggerReconciliation,
                ///Commit4383
                IsSendForintegration = this.IsSendForintegration
            };
            if (this.ComplianceViewDocumentID > AppConsts.NONE)
            {
                newAttribute.ComplianceAttributeDocuments.Add(new ComplianceAttributeDocument()
                {
                    CAD_IsDeleted = false,
                    CAD_CreatedBy = this.CreatedByID,
                    CAD_CreatedOn = DateTime.Now,
                    CAD_DocumentID = this.ComplianceViewDocumentID.Value,
                });
            }

            //Added in UAT-4558
            if (!this.lstFileUploadAttrDocIds.IsNullOrEmpty() && this.lstFileUploadAttrDocIds.Count > AppConsts.NONE)
            {
                this.lstFileUploadAttrDocIds.ForEach(docId =>
                {
                    newAttribute.ComplianceAttributeDocMappings.Add(new ComplianceAttributeDocMapping()
                    {
                        CADM_IsDeleted = false,
                        CADM_CreatedBy = this.CreatedByID,
                        CADM_CreatedOn = DateTime.Now,
                        CADM_SystemDocumentID = docId,
                    });
                });
            }
            //END

            return newAttribute;
        }

        public List<Int32> lstFileUploadAttrDocIds { get; set; }
    }
}
