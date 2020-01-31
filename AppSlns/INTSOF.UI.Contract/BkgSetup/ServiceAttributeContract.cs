using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class ServiceAttributeContract
    {
        public Int32 ServiceAttributeID { get; set; }
        public Int32 ServiceAttributeDatatypeID { get; set; }
        public Guid? CopiedFromCode { get; set; }
        public Guid Code { get; set; }
        public String Name { get; set; }
        public String AttributeLabel { get; set; }
        public String Description { get; set; }
        public Int32? MaximumCharacters { get; set; }
        public Int32? MinimumCharacters { get; set; }
        public Int32? MaximumNumericvalue { get; set; }
        public Int32? MinimumNumericvalue { get; set; }
        public DateTime? MaximumDatevalue { get; set; }
        public DateTime? MinimumDatevalue { get; set; }
        //public Boolean IsRequired { get; set; }
        public Boolean IsEditable { get; set; }
        public Boolean IsSystemPreConfigured { get; set; }
        //public String RequiredvalidationMsg { get; set; }
        public Boolean IsDeleted { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsRequired { get; set; }
        public Boolean IsDisplay { get; set; }
        public Int32 CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; }
        public Int32? ModifiedByID { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Int32 TenantID { get; set; }
        public Int32 ServiceAttributeMappingId { get; set; }
        public Boolean UpdateAllData { get; set; }
        public String ValidationExpression { get; set; }
        public String ValidationMessage { get; set; }
        public Boolean IsHiddenFromUI { get; set; }

        public System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> lstMasterServiceAttributeOption { get; set; }
        public Entity.BkgSvcAttribute TranslateToMasterEntity()
        {
            return new Entity.BkgSvcAttribute()
            {
                BSA_ID = this.ServiceAttributeID,
                BSA_DataTypeID = this.ServiceAttributeDatatypeID,
                BSA_CopiedFromCode = this.CopiedFromCode,
                BSA_Code = this.Code,
                BSA_Name = this.Name,
                BSA_Label = this.AttributeLabel,
                BSA_Description = this.Description,
                BSA_MaxLength = this.MaximumCharacters,
                BSA_MinLength = this.MinimumCharacters,
                BSA_MaxDateValue=this.MaximumDatevalue,
                BSA_MinDateValue=this.MinimumDatevalue,
                BSA_MaxIntValue=this.MaximumNumericvalue,
                BSA_MinIntValue=this.MinimumNumericvalue,
                BSA_IsDeleted = this.IsDeleted,
                BSA_IsEditable=this.IsEditable,
                BSA_IsRequired=this.IsRequired,
                //BSA_ReqValidationMessage=this.RequiredvalidationMsg,
                BSA_IsSystemPreConfiguredq=this.IsSystemPreConfigured,
                BSA_Active = this.IsActive,
                BSA_CreatedById = this.CreatedByID,
                BSA_CreatedDate = this.CreatedOn,
                BSA_ModifiedBy = this.ModifiedByID,
                BSA_ModifiedDate = this.ModifiedOn,
                BkgSvcAttributeOptions = this.lstMasterServiceAttributeOption
            };
        }

        public System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption> lstClientServiceAttributeOption { get; set; }
        public BkgSvcAttribute TranslateToClientEntity()
        {
            return new BkgSvcAttribute()
            {
                BSA_ID = this.ServiceAttributeID,
                BSA_DataTypeID = this.ServiceAttributeDatatypeID,
                BSA_CopiedFromCode = this.CopiedFromCode,
                BSA_Code = this.Code,
                BSA_Name = this.Name,
                BSA_Label = this.AttributeLabel,
                BSA_Description = this.Description,
                BSA_MaxLength = this.MaximumCharacters,
                BSA_MinLength = this.MinimumCharacters,
                BSA_MaxDateValue = this.MaximumDatevalue,
                BSA_MinDateValue = this.MinimumDatevalue,
                BSA_MaxIntValue = this.MaximumNumericvalue,
                BSA_MinIntValue = this.MinimumNumericvalue,
                BSA_IsDeleted = this.IsDeleted,
                BSA_IsEditable = this.IsEditable,
                //BSA_IsRequired = this.IsRequired,
                //BSA_ReqValidationMessage = this.RequiredvalidationMsg,
                BSA_IsSystemPreConfiguredq = this.IsSystemPreConfigured,
                BSA_Active = this.IsActive,
                BSA_CreatedById = this.CreatedByID,
                BSA_CreatedDate = this.CreatedOn,
                BSA_ModifiedBy = this.ModifiedByID,
                BSA_ModifiedDate = this.ModifiedOn,
                BkgSvcAttributeOptions = this.lstClientServiceAttributeOption
            };
        }
    }
}
