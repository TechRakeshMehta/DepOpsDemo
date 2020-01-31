using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ReconciliationApplicantPanelPresenter : Presenter<IReconciliationApplicantPanelView>
    {
        public override void OnViewLoaded()
        {

        }

        /// <summary>
        /// call when View Initialized
        /// </summary>
        public override void OnViewInitialized()
        {
            GetOrgUserData();
            GetApplicantComplianceData();
        }

        private void GetOrgUserData()
        {
            View.OrganizationUserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(View.CurrentApplicantId_Global);
        }


        public String GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }
        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unMaskedSSN);
        }
        public void GetApplicantComplianceData()
        {
            vwApplicantComplianceItemData ApplicantComplianceItemDataList;
            String status = "";
            Entity.CustomPagingArgs customPagingArgs = new Entity.CustomPagingArgs();
            String itemComplianceStatusCode = String.Empty;
            String reviewerTypeCode = String.Empty;
            Int32 clientId = View.OrganizationUserData.Organization.Tenant.TenantID;
            if (clientId == SecurityManager.DefaultTenantID)
            {
                status += "'" + ApplicantItemComplianceStatus.Pending_Review.GetStringValue() + "'";
                reviewerTypeCode = LkpReviewerType.Admin;
                if (View.TenantId_Global != 0)
                {
                    clientId = View.TenantId_Global;
                }
            }

            if (!(IsDefaultTenant && View.TenantId_Global == 0))
            {
                ApplicantComplianceItemDataList = ComplianceDataManager.GetApplicantVerificationDetails(View.SelectedPackageSubscriptionID_Global, View.TenantId_Global);
                View.CurrentCompliancePackageId_Global = ApplicantComplianceItemDataList.PackageID;
                View.CurrentApplicantId_Global = ApplicantComplianceItemDataList.ApplicantID;
                View.SubscriptionExpirationDate = ApplicantComplianceItemDataList.ExpirationDate;
                View.CurrentCompliancePackageStatus = ApplicantComplianceItemDataList.ComplianceStatusName;
                View.CurrentCompliancePackageStatusCode = ApplicantComplianceItemDataList.ComplianceStatusCode;
                View.packageName = ApplicantComplianceItemDataList.PackageName;
                View.CurrentPackageBredCrum = ApplicantComplianceItemDataList.InstitutionHierarchy;
                if (View.OrganizationUserData != null && View.OrganizationUserData.OrganizationUserID != View.CurrentApplicantId_Global)
                {
                    View.OrganizationUserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(View.CurrentApplicantId_Global);
                    View.UserGroupDataList = ComplianceDataManager.GetUserGroupsForUser(View.TenantId_Global, View.CurrentApplicantId_Global);
                }
                View.IsRushOrder = (ApplicantComplianceItemDataList.RushOrderStatus == null) ? false : true;
                View.SelectedOrderId = ApplicantComplianceItemDataList.OrderID;

                List<OrderPkgPaymentDetail> lstOrderPkgPaymentDetail = GetOrderPkgPaymentDetail();
                String compOrderPkgTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
                String compRushOrderPkgTypeCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
                String orderPkgTypeToGet = View.IsRushOrder ? compRushOrderPkgTypeCode : compOrderPkgTypeCode;
                OrderPaymentDetail complianceOrderPaymentDetail = new OrderPaymentDetail();
                if (!lstOrderPkgPaymentDetail.IsNullOrEmpty())
                {
                    complianceOrderPaymentDetail = lstOrderPkgPaymentDetail.FirstOrDefault(x => x.lkpOrderPackageType.OPT_Code == orderPkgTypeToGet).OrderPaymentDetail;
                }

                if (!complianceOrderPaymentDetail.IsNullOrEmpty())
                {
                    View.OrderApprovalDate = complianceOrderPaymentDetail.OPD_ApprovalDate.HasValue ? complianceOrderPaymentDetail.OPD_ApprovalDate.Value : (DateTime?)null;
                }

                View.lstNextPrevReconiciliationItem = ComplianceDataManager.GetReconciledItemsList(View.SelectedInstitutionIds, View.CurrentCompItemRecDataID);
                HttpContext.Current.Session["CurrentReconciliationIds"] = View.lstNextPrevReconiciliationItem;
                HttpContext.Current.Session["CurrentCompItemRecDataID"] = View.CurrentCompItemRecDataID;

            }
        }

        private List<OrderPkgPaymentDetail> GetOrderPkgPaymentDetail()
        {
            if (View.SelectedOrderId > AppConsts.NONE)
            {
                return ComplianceDataManager.GetOrderPkgPaymentDetailsByOrderID(View.TenantId_Global, View.SelectedOrderId);
            }
            return new List<OrderPkgPaymentDetail>();
        }



        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Entity.Tenant GetTenant(Int32 tenantID)
        {
            if (View.OrganizationUserData != null && View.OrganizationUserData.OrganizationUserID != tenantID)
                return SecurityManager.GetOrganizationUser(tenantID).Organization.Tenant;
            return View.OrganizationUserData.Organization.Tenant;
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.LoggedInUser.TenantID;
            }
        }

        public Boolean IsThirdPartyTenant
        {
            get
            {
                return SecurityManager.IsTenantThirdPartyType(View.LoggedInUser.TenantID, TenantType.Compliance_Reviewer.GetStringValue());
            }
        }

        public string GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }


        /// <summary>
        /// Get the Current applicant data, including his address.
        /// </summary>
        public void GetApplicantData()
        {
            View.ApplicantData = ComplianceDataManager.GetUserData(View.CurrentApplicantId_Global, View.TenantId_Global);
        }


        public String GetApplicantSSN()
        {
            return SecurityManager.GetFormattedString(View.OrganizationUserData.OrganizationUserID, false);
        }
    }
}
