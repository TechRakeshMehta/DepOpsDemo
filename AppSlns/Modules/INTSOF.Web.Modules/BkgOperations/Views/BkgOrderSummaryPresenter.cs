using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderSummaryPresenter : Presenter<IBkgOrderSummaryView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            GetGranularPermissionForDOBandSSN();

        }

        public OrderDetailClientAdmin GetOrderDetailsInfo()
        {
            return BackgroundProcessOrderManager.GetOrderDetailsInfo(View.SelectedTenantId, View.OrderID);

        }

        public Boolean IsEdsServiceExitForOrder()
        {
            String eDrugScrnServiceTypeCode = BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue();
            return BackgroundProcessOrderManager.IsEdsServiceExitForOrder(View.SelectedTenantId, View.OrderID, eDrugScrnServiceTypeCode);
        }


        public String GetFormatttedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
            }
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

        #endregion

        #region UAT-844
        /// <summary>
        /// Get Order package service group data by OPSG_ID
        /// </summary>
        public void GetOrderPackageServiceGroupData()
        {
            if (View.orderPkgSvcGroupID.IsNotNull())
            {
                BkgOrderPackageSvcGroup bkgOrderPackageSvcGroupObj = BackgroundProcessOrderManager.GetOrderPackageServiceGroupData(View.SelectedTenantId, View.orderPkgSvcGroupID);
                if (bkgOrderPackageSvcGroupObj.IsNotNull())
                {
                    View.bkgOrderPackageSvcGroup = bkgOrderPackageSvcGroupObj;
                }
                else
                {
                    View.bkgOrderPackageSvcGroup = new BkgOrderPackageSvcGroup();
                }
            }
        }

        /// <summary>
        /// Get Service Group Review Status list to bind combo
        /// </summary>
        public void GetServiceGroupReviewStatusList()
        {
            View.lstServiceGroupReviewStatus = BackgroundProcessOrderManager.GetServiceGroupReviewStatusList(View.SelectedTenantId)
                        .Where(x => !x.BSGRS_ReviewCode.Equals(BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue())).ToList();
            // Removing Auto Review Completed status from the dropdown
        }

        #endregion
    }
}
