using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementApplicantDetailPanel
    {
        Entity.Tenant LoggedInUser { get; }
        Int32 CurrentLoggedInUserId { get; }
        List<INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass> ApplicantComplianceCategoryDataList { get; set; }
        Int32 SelectedTenantID { get; set; }
        Int32 ClinicalRotationID { get; set; }
        ClinicalRotationDetailContract RotationDeatils { get; set; }
        OrganizationUserContract ApplicantData { get; set; }
        Int32 CurrentApplicantID { get; set; }
        List<ReqPkgSubscriptionIDList> lstReqPkgsubscriptionIdList { get; set; }
        Entity.OrganizationUser OrganizationUserData { get; set; }
        Boolean IsDOBDisable { get; set; }
        String SSNPermissionCode { get; set; }
        Int32 SelectedPackageSubscriptionID { get; set; }
        Int32 ReqPkgSubsciptionID { get; set; }
        List<RequirementVerificationDetailContract> lstReqPkgSubData { get; set; }
        Int32 PrevReqPackageSubscriptionID { get; set; }
        Int32 NextReqPackageSubscriptionID { get; set; }
        Int32 CategoryID { get; set; }
        String ControlUseType { get; set; }
        Int32 AffectedItemsCount { get; set; }
        String EntityPermissionName { get; set; }

        Int32 RequirementItemId { get; set; }
        Int32 ApplicantRequirementItemId { get; set; }
        Int32 SubPageIndex { get; set; }
        Int32 SubTotalPages { get; set; }
        Int32 SelectedPackageSubscriptionID_Global { get; set; }
        Int32 ItemDataId_Global { get; set; }
        Int32 RequirementPackageTypeId { get; set; }

        Int32 PrevAppReqItemID { get; set; }
        Int32 PrevRotationID { get; set; }
        Int32 NextRotationID { get; set; }
        Int32 NextApplicantID { get; set; }
        Int32 PrevApplicantID { get; set; }
        Int32 NextAppReqItemID { get; set; }
        Int32 PrevCategoryID { get; set; }
        Int32 NextCategoryID { get; set; }
        Int32 PrevTenantID { get; set; }
        Int32 NextTenantID { get; set; }

        Int32 TenantID { get; }
        String PendingItemNames { get; set; }
    }
}
