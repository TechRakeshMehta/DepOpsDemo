#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public class PackagePricePresenter : Presenter<IPackagePriceView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// To get Price Adjustment List
        /// </summary>
        public void GetPriceAdjustment()
        {
            var priceAdjustmentList = ComplianceSetupManager.GetPriceAdjustmentList(View.TenantId);
            priceAdjustmentList.Insert(0, new PriceAdjustment { PriceAdjustmentID = 0, Label = "--SELECT--" });
            View.ListPriceAdjustment = priceAdjustmentList;
        }

        /// <summary>
        /// To get Price Adjustment Data by ID and tenantId
        /// </summary>
        public void GetPriceAdjustmentData()
        {
            View.ListDeptProgramPackagePriceAdjustment = ComplianceSetupManager.GetPriceAdjustmentData(View.ID, View.TenantId, View.TreeNodeType);
        }

        /// <summary>
        /// To get all Prices
        /// </summary>
        public void GetPrice()
        {
            if (View.TenantId > 0)  //View.ID > 0 && 
            {
                var priceContract = ComplianceSetupManager.GetPrice(View.ID, View.TenantId, View.TreeNodeType, View.ParentID, View.MappingID, View.ItmSubsID, View.ItmCatID, View.ItemID);
                if (priceContract.IsNotNull() && !priceContract.IsPriceNull)
                {
                    View.Price = priceContract.Price;
                    View.TotalPrice = priceContract.TotalPrice;
                    View.RushOrderAdditionalPrice = priceContract.RushOrderAdditionalPrice;
                }
            }
        }

        /// <summary>
        /// To check if Price is Disabled
        /// </summary>
        public void EnableDisablePriceByPriceModel()
        {
            View.IsPriceDisabled = ComplianceSetupManager.CheckIsPriceDisabled(View.ParentID, View.ParentSubscriptionID, View.MappingID, View.TenantId, View.TreeNodeType);
        }

        /// <summary>
        /// To check if Price is Disabled
        /// </summary>
        public void ShowMessage()
        {
            View.IsShowMessage = ComplianceSetupManager.ShowMessage(View.ParentID, View.ParentSubscriptionID, View.MappingID, View.TenantId, View.TreeNodeType);
        }

        /// <summary>
        /// To save Price and Price Adjustment Detail data
        /// </summary>
        public void SavePriceAdjustmentDetail()
        {
            var priceContract = ComplianceSetupManager.SavePriceAdjustmentDetail(View.ID, View.ParentID, View.MappingID, View.ParentSubscriptionID, View.ComplianceCategoryID, View.Price, View.RushOrderAdditionalPrice, View.SelectedPriceAdjustmentID, View.PriceAdjustmentValue, View.CurrentLoggedInUserId, View.TenantId, View.TreeNodeType);
            if (priceContract.IsNotNull())
            {
                if (View.ID == AppConsts.NONE && priceContract.NewID > AppConsts.NONE)
                {
                    View.ID = priceContract.NewID;
                }
                if (View.ParentID == AppConsts.NONE && priceContract.NewParentID > AppConsts.NONE)
                {
                    View.ParentID = priceContract.NewParentID;
                }
            }
        }

        /// <summary>
        /// To update Price Adjustment Detail
        /// </summary>
        public void UpdatePriceAdjustmentDetail()
        {
            ComplianceSetupManager.UpdatePriceAdjustmentDetail(View.ViewContract.ID, View.ID, View.ViewContract.PriceAdjustmentID, View.ViewContract.PriceAdjustmentValue, View.CurrentLoggedInUserId, View.TenantId, View.TreeNodeType);
        }

        /// <summary>
        /// To delete Price Adjustment Detail
        /// </summary>
        /// <returns></returns>
        public Boolean DeletePriceAdjustmentData()
        {
            return ComplianceSetupManager.DeletePriceAdjustmentData(View.ViewContract.ID, View.ID, View.CurrentLoggedInUserId, View.TenantId, View.TreeNodeType);
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (GetTenantId() == SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the Tenant Id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }
    }
}




