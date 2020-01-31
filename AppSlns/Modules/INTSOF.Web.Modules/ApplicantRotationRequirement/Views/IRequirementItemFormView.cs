using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public interface IRequirementItemFormView
    {
        /// <summary>
        /// Id of the Master compliance Item (Client database), present in the ApplicantComplianceItem Entity. 
        /// </summary>
        Int32 ItemId { get; set; }

        Int32 TenantId { get; set; }
        String ItemName { get; set; }
        Boolean ReadOnly { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 RequirementCategoryId { get; set; }
        Int32 RequirementPkgSubscriptionId { get; set; }
        IRequirementItemFormView CurrentViewContext { get; }
        Int32 ApplicantRequirementItemId { get; set; }
        Dictionary<Int32, Int32> AttributeDocuments { get; set; }

        ApplicantRequirementFieldDataContract ViewContract { get; }
        RequirementItemContract RequirementItem { get; set; }
        ApplicantRequirementItemDataContract RequirementItemData { get; set; }
        List<RequirementFieldContract> RequirementItemFields { get; set; }
        List<RequirementObjectTreeContract> LstRequirementObjTreeProperty { get; set; }
        Int32 RequirementPackageId { get; set; }
    }
}
