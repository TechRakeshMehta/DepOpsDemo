using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public class AdminEntryApplicantLandingPresenter : Presenter<IAdminEntryApplicantLandingView>
    {
        /// <summary>
        /// Gets the total number of Custom forms available for the selected background packages
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public Int32 GetCustomFormsCount(String packageId)
        {
            List<CustomFormDataContract> lstCustomForm = BackgroundProcessOrderManager.GetCustomFormsForThePackage(View.TenantId, packageId);
            if (!lstCustomForm.IsNullOrEmpty())
                return lstCustomForm.DistinctBy(cstFrm => cstFrm.customFormId).Count();

            return AppConsts.NONE;
        }
        public Int32 GetInstitutionDPMID()
        {
            return ComplianceDataManager.GetInstitutionDPMID(View.TenantId);
        }
        public Int32 GetLastNodeInstitutionId(Int32 selectedDPMId)
        {
            return ComplianceDataManager.GetLastNodeInstitutionId(selectedDPMId, View.TenantId);
        }
        public void GetAdditionalDocuments(List<BackgroundPackagesContract> lstBkgPackages, Int32 selectedHierarchyId, Dictionary<string, OrderCartCompliancePackage> CompliancePackages, Boolean isCompliancePackageSelected)
        {
            var packagesList = lstBkgPackages;
            List<Int32> BkgPackages = new List<Int32>();
            List<Int32> compPackageIdList = new List<Int32>();
            Boolean isAdditionalDocumentExist = false;
            if (!packagesList.IsNullOrEmpty())
            {
                foreach (var item in packagesList)
                {
                    BkgPackages.Add(item.BPAId);
                }
            }

            //Get Compliance package Ids.

            if (isCompliancePackageSelected)
            {
                CompliancePackages.ForEach(cnd =>
                {
                    if (!cnd.Value.IsNullOrEmpty())
                    {
                        compPackageIdList.Add(cnd.Value.CompliancePackageID);
                    }
                });
            }
            List<Entity.SystemDocument> additionalDocument = BackgroundSetupManager.GetAdditionalDocuments(BkgPackages, compPackageIdList, selectedHierarchyId, View.TenantId);
            if (!additionalDocument.IsNullOrEmpty())
            {
                isAdditionalDocumentExist = true;
            }
            View.IsAdditionalDocumentExist = isAdditionalDocumentExist;
        }
        public Int32 GetDefaultNodeId(Int32 tenantId)
        {
            return ComplianceDataManager.GetDefaultNodeId(tenantId);
        }
        public void InitializedData()
        {
            GetTenantName();
            GetOrderNode();
            IsTokenExpired();
        }

        public void GetTenantName()
        {
            if (View.TenantId > AppConsts.NONE)
            {
                View.TenantName = SecurityManager.GetTenantName(View.TenantId, AppConsts.NONE, AppConsts.NONE);
            }
        }

        public void GetOrderNode()
        {
            Entity.ClientEntity.Order orderDetail = ComplianceDataManager.GetOrderDetailsByOrderId(View.TenantId, View.OrderId);
            var doesBkgPkgExists = false;

            if (!orderDetail.IsNullOrEmpty())
                doesBkgPkgExists = orderDetail.BkgOrders.Where(x => x.BOR_MasterOrderID == View.OrderId).Any(y => y.BkgOrderPackages.ToList()
                                              .Any(b => !b.BkgPackageHierarchyMapping.BackgroundPackage.IsNullOrEmpty() && b.BkgPackageHierarchyMapping.BackgroundPackage.BPA_ID > AppConsts.NONE));

            if (!orderDetail.IsNullOrEmpty() && doesBkgPkgExists)
            {
                View.NodeName = orderDetail.DeptProgramMapping.InstitutionNode.IN_Name;
            }
            else
            {
                View.IsLinkExpiredOrOrderDeleted = true;
            }
        }

        public void GetApplicantCartData()
        {
            View.applicantOrderCartData = new ApplicantOrderCart();
            if (View.TenantId > 0 && View.OrderId > 0)
            {
                View.applicantOrderCartData = AdminEntryPortalManager.GetApplicantCartData(View.TenantId, View.OrderId);
            }
        }

        public void IsTokenExpired()
        {
            Boolean isTokenExpired = SecurityManager.IsTokenExpired(View.TenantId, View.OrderId, View.TokenKey);
            if (isTokenExpired)
                View.IsLinkExpiredOrOrderDeleted = true;
        }

        public String GetApplicantInviteContent()
        {
            return AdminEntryPortalManager.GetApplicantInviteContent(View.TenantId, View.OrderId);
        }
    }
}
