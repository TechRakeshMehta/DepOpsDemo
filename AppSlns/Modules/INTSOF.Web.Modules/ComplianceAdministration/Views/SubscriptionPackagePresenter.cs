using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;


namespace CoreWeb.ComplianceAdministration.Views
{
    public class SubscriptionPackagePresenter : Presenter<ISubscriptionPackageView>
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
        /// To get Subscription Options List
        /// </summary>
        public void GetSubscriptionOptions()
        {
            View.ListSubscriptionOption = ComplianceSetupManager.GetSubscriptionOptionsList(View.TenantId);
        }

        /// <summary>
        /// To get Price Model List
        /// </summary>
        public void GetPriceModel()
        {
            View.ListPriceModel = ComplianceSetupManager.GetPriceModelList(View.TenantId);
        }

        /// <summary>
        /// To get selected Subscription Options
        /// </summary>
        public void GetSelectedSubscriptionOptions()
        {
            List<Int32> selectedSubscriptionOptionIds = new List<Int32>();
            var deptProgramPackageSubscription = ComplianceSetupManager.GetDeptProgramPackageSubscriptionByProgPackageId(View.DeptProgramPackageID, View.TenantId);

            if (deptProgramPackageSubscription.IsNotNull())
            {
                deptProgramPackageSubscription.ForEach(x => selectedSubscriptionOptionIds.Add(x.DPPS_SubscriptionID));
                if (!deptProgramPackageSubscription.Any())
                {
                    selectedSubscriptionOptionIds.Add(View.ListSubscriptionOption.Find(x => x.Code == new Guid(SubscriptionOptions.CustomMonthly.GetStringValue())).SubscriptionOptionID);
                }
                View.SelectedSubscriptionOptions = selectedSubscriptionOptionIds;
            }
        }

        /// <summary>
        /// To set selected Price Model Priority
        /// </summary>
        public void GetSelectedPriceModelPriority()
        {
            //var deptProgramPackages = ComplianceSetupManager.GetDeptProgramPackageByPackageId(View.PackageId, View.TenantId);
            var deptProgramPackages = ComplianceSetupManager.GetDeptProgramPackageByID(View.DeptProgramPackageID, View.TenantId);

            if (deptProgramPackages.IsNotNull())
            {
                if (deptProgramPackages.DPP_PriceModelID.IsNotNull())
                {
                    View.SelectedPriceModel = deptProgramPackages.DPP_PriceModelID.Value;
                    View.SavedPriceModelId = deptProgramPackages.DPP_PriceModelID.Value;
                }
                if (deptProgramPackages.DPP_Priority.IsNotNull())
                {
                    View.Priority = deptProgramPackages.DPP_Priority.Value;
                }
            }
        }

        /// <summary>
        /// To save Program Package Subscription Mapping
        /// </summary>
        public void SaveProgramPackageSubscriptionMapping(Boolean availability)
        {
            if (!ComplianceSetupManager.SaveProgramPackageSubscriptionMapping(View.DeptProgramPackageID, View.SelectedSubscriptionOptions, View.SelectedPriceModel, View.SavedPriceModelId, View.Priority, View.CurrentLoggedInUserId, View.lstSelectedOptions, View.PaymentApprovalID, View.TenantId))
                View.ErrorMessage = "Subscription(s) can not be saved due to some error. Please try again!";
            SetCompliancePkgAvailabilityForOrder(availability);
            ComplianceSetupManager.SetAutoRenewInvoiceOrderForPackage(View.DeptProgramPackageID, View.TenantId, View.CurrentLoggedInUserId, View.IsAutoRenewInvoiceOrder);
        }

        /// <summary>
        /// To Set Compliance Package Availability For Order
        /// </summary>
        public void SetCompliancePkgAvailabilityForOrder(Boolean availability)
        {
            ComplianceSetupManager.SetCompliancePkgAvailabilityForOrder(View.DeptProgramPackageID, View.CurrentLoggedInUserId, View.TenantId, availability);
        }



        public Boolean BindAvailabilityForOrder()
        {
            return ComplianceSetupManager.IsCompliancePkgAvailableForOrder(View.DeptProgramPackageID, View.TenantId);
        }

        public void BindAutoRenewInvoiceOrderForPackage()
        {
            View.IsAutoRenewInvoiceOrder = ComplianceSetupManager.IsAutoRenewInvoiceOrderForPackage(View.DeptProgramPackageID, View.TenantId);
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




