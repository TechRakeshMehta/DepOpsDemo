#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

#endregion

#region UserDefined

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ManagePriceAjustmentPresenter : Presenter<IManagePriceAjustmentView>
    {
        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties
        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {
            View.ListTenants = ComplianceDataManager.getClientTenant();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetPriceAdjustmentList()
        {
            View.ListPriceAdjustment = ComplianceSetupManager.GetAllPriceAdjustment(View.SelectedTenantID);
        }

        public Boolean IsPriceAdjustmentExist(String label, Int32? priceAdjustmentId = null)
        {
            View.ListPriceAdjustment = ComplianceSetupManager.GetAllPriceAdjustment(View.SelectedTenantID);
            if (priceAdjustmentId != null)
            {
                if (View.ListPriceAdjustment.Any(x => x.Label.ToLower() == label.ToLower() && x.PriceAdjustmentID != priceAdjustmentId))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (View.ListPriceAdjustment.Any(x => x.Label.ToLower() == label.ToLower() && !x.IsDeleted))
                {
                    return false;
                }
                return true;
            }
        }

        public Boolean SavePriceAdjustment()
        {
            if (IsPriceAdjustmentExist(View.Label, null))
            {
                PriceAdjustment priceAdjustment = new PriceAdjustment();
                priceAdjustment.Label = View.Label;
                priceAdjustment.Description = View.Description;
                priceAdjustment.IsDeleted = false;
                priceAdjustment.CreatedByID = View.CurrentUserId;
                priceAdjustment.CreatedOn = DateTime.Now;
                if (ComplianceSetupManager.SavePriceAdjustment(View.SelectedTenantID, priceAdjustment))
                {
                    View.SuccessMessage = "Price adjustment saved successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occured.Please try again.";
                    return false;
                }
            }
            else
            {
                View.ErrorMessage = "Price adjustment already exist.";
                return false;
            }
        }

        public Boolean UpdatePriceAdjustment()
        {
            if (IsPriceAdjustmentExist(View.Label, View.PriceAdjustmentId))
            {
                PriceAdjustment priceAdjustment = ComplianceSetupManager.GetPriceAdjustmentById(View.SelectedTenantID, View.PriceAdjustmentId);
                priceAdjustment.Label = View.Label;
                priceAdjustment.Description = View.Description;
                priceAdjustment.IsDeleted = false;
                priceAdjustment.ModifiedByID = View.CurrentUserId;
                priceAdjustment.ModifiedOn = DateTime.Now;
                if (ComplianceSetupManager.UpdatePriceAdjustment(View.SelectedTenantID))
                {
                    View.SuccessMessage = "Price adjustment updated successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occured.Please try again.";
                    return false;
                }
            }
            else
            {
                View.ErrorMessage = "Price adjustment already exist.";
                return false;
            }
        }

        public Boolean DeletePriceAdjustment()
        {
            if (!ComplianceSetupManager.IsPriceAdjustmentMapped(View.SelectedTenantID, View.PriceAdjustmentId))
            {
                PriceAdjustment priceAdjustment = ComplianceSetupManager.GetPriceAdjustmentById(View.SelectedTenantID, View.PriceAdjustmentId);
                priceAdjustment.IsDeleted = true;
                priceAdjustment.ModifiedByID = View.CurrentUserId;
                priceAdjustment.ModifiedOn = DateTime.Now;
                //priceAdjustment.Label = priceAdjustment.Label + Guid.NewGuid();
                if (ComplianceSetupManager.UpdatePriceAdjustment(View.SelectedTenantID))
                {
                    View.SuccessMessage = "Price adjustment deleted successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occured.Please try again.";
                    return false;
                }
            }
            else
            {
                View.ErrorMessage = "Price adjustment is in use and cannot be deleted.";
                return false;
            }
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            Int32 currentUserTenantId = GetTenantId();
            //Checked if logged user is admin or not.
            if (currentUserTenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                View.SelectedTenantID = currentUserTenantId;
                return false;
            }
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}




