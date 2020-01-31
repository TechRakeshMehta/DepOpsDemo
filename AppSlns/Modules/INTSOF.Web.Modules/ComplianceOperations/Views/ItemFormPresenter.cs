using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Linq;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ItemFormPresenter : Presenter<IItemFormView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public ItemFormPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public void GetComplianceItemForControls()
        {
            View.CurrentViewContext.ClientComplianceItem = ComplianceDataManager.GetDataEntryComplianceItem(View.CurrentViewContext.ItemId, View.TenantId);
        }

        public Tuple<Boolean, Int32> CheckItemPayment()
        {
            if (View.CurrentViewContext.ApplicantItemData != null)
            {
                if (View.CurrentViewContext.ApplicantItemData.ApplicantComplianceItemID > 0)
                    return ComplianceDataManager.CheckItemPayment(View.TenantId, View.CurrentViewContext.ApplicantItemData.ApplicantComplianceItemID, View.CurrentViewContext.ItemId, false);
            }
            return new Tuple<bool, int>(false, 0);
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetApplicantData()
        {
            if (View.CurrentViewContext.ItemId > 0)
                View.CurrentViewContext.ApplicantItemData = ComplianceDataManager.GetApplicantData(View.CurrentViewContext.PackageId, View.CurrentViewContext.ComplianceCategoryId, View.ItemId, View.CurrentViewContext.CurrentLoggedInUserId, View.CurrentViewContext.TenantId);
        }
        // TODO: Handle other view events and set state in the view

        #region UAT-1607: Student Data Entry Screen changes
        public void GetItemSeriesDataForControls()
        {
            ItemSery itemSeriesData = ComplianceDataManager.GetItemSeriesByID(View.TenantId, View.CurrentViewContext.ItemId);
            View.CurrentViewContext.ClientComplianceItem = ComplianceDataManager.AssignItemSeriesDataToCompItemContract(itemSeriesData);
        }
        #endregion

        #region UAT-4067
        //public void GetAllowedFileExtensions()
        //{
        //    var lstAllowedFileExtensions = ComplianceDataManager.GetAllowedFileExtensionsByNodeIDs(View.CurrentViewContext.TenantId, View.SelectedNodeIds);
        //    if (!lstAllowedFileExtensions.IsNullOrEmpty())
        //    {
        //        View.AllowedExtensions = String.Join(",", lstAllowedFileExtensions.Select(x => x.Name).ToList());
        //        if (!String.IsNullOrEmpty(View.AllowedExtensions))
        //            View.IsAllowedFileExtensionEnable = true;
        //    }
        //}

        #endregion
    }
}




