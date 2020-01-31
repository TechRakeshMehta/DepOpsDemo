#region Namespaces

#region SystemDefined

using System;
using INTSOF.SharedObjects;
using System.Collections.Generic;
using System.Linq;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using System.Text;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public class ApplicantPortfolioOrderHistoryPresenter : Presenter<IApplicantPortfolioOrderHistoryView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetOrderDetailList()
        {
            if (View.SourceScreen.ToLower().Equals("applicantportfoliosearch"))
            {
                View.ListOrderDetail = ComplianceDataManager.GetOrderDetailListByOrgUserID(View.TenantID, View.OrganizationUserId);
            }
            else if (SecurityManager.DefaultTenantID != View.TenantID)
            {
                View.ListOrderDetail = ComplianceDataManager.GetOrderDetailList(View.TenantID, View.OrganizationUserId, new List<String>()).ToList();
            }
        }

        /// <summary>
        /// UAT-1855
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        public String SetArchiveStateText(vwOrderDetail orderDetail)
        {
            StringBuilder arcvalStateText = new StringBuilder();
            String archiveStatus = String.Empty;
            List<lkpArchiveState> lkpArchiveSatetList = LookupManager.GetLookUpData<Entity.ClientEntity.lkpArchiveState>(View.TenantID).Where(x => !x.AS_IsDeleted).ToList();

            if (orderDetail.IsNotNull())
            {
                if (orderDetail.PackageSubscriptionArchiveCode.IsNotNull())
                {
                    archiveStatus = lkpArchiveSatetList.Where(x => x.AS_Code == orderDetail.PackageSubscriptionArchiveCode).FirstOrDefault().AS_Name;
                    arcvalStateText.Append("Tracking Archive State: ").Append(archiveStatus);
                    if (orderDetail.BKgArchiveCode.IsNotNull())
                    {
                        arcvalStateText.Append("<br/>");
                    }
                }
                if (orderDetail.BKgArchiveCode.IsNotNull())
                {
                    archiveStatus = lkpArchiveSatetList.Where(x => x.AS_Code == orderDetail.BKgArchiveCode).FirstOrDefault().AS_Name;
                    arcvalStateText.Append("Screening Archive State: ").Append(archiveStatus);
                }
            }
            return arcvalStateText.ToString();
        }

        public void GetCancelledPackageByOrderID()
        {
            if (View.TenantID > AppConsts.NONE && View.OrderID > AppConsts.NONE)
            {
                List<CancelledBkgCompliancePackageContract> lstPackages = ComplianceDataManager.GetCancelledPackageByOrderID(View.TenantID, View.OrderID);

                if (!lstPackages.IsNullOrEmpty())
                {
                    List<CancelledBkgCompliancePackageContract> lstBkgPackages = lstPackages.Where(x => !x.IsCompliancePackage).ToList();
                    List<CancelledBkgCompliancePackageContract> lstCompPackages = lstPackages.Where(x => x.IsCompliancePackage).ToList();
                    StringBuilder tmpHtml = new StringBuilder();
                    tmpHtml.Append("<div class=\"doc-wrapper\" style=\"padding:10px;background-color:#f0f0f0;overflow:auto;max-height:400px;\">");

                    foreach (CancelledBkgCompliancePackageContract item in lstCompPackages)
                    {
                        tmpHtml.Append("<h2>Tracking Package</h2>");
                        String cancellationDate = item.CancellationReqDate.IsNullOrEmpty() ? String.Empty : " [" + item.CancellationReqDate.Value.Date.ToShortDateString() + "] ";
                        tmpHtml.Append("<p>" + item.PackageName + cancellationDate + "</p>");
                    }
                    if (!lstBkgPackages.IsNullOrEmpty())
                    {
                        tmpHtml.Append("<h2>Screening Packages</h2>");
                        foreach (CancelledBkgCompliancePackageContract item in lstBkgPackages)
                        {
                            tmpHtml.Append("<p>" + item.PackageName + "</p>");
                        }
                    }

                    tmpHtml.Append("</div>");
                    View.CancelledPackages = tmpHtml.ToString();
                }

            }
        }
    }
}




