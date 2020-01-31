using System;
using System.Collections.Generic;
using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using System.Linq;
using INTSOF.Utils;
using Entity;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public class AgencyUserAuditHistoryPresenter : Presenter<IAgencyUserAuditHistoryView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
        }

        #region Public Method
        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<Entity.Tenant> lstTemp = SecurityManager.GetTenants(SortByName, false, clientCode);
            lstTemp.Insert(0, new Entity.Tenant { TenantID = 0, TenantName = "--Select--" });
            View.lstTenant = lstTemp;
        }
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        private Int32 ClientId
        {
            get
            {
                if (IsDefaultTenant)
                    return View.SelectedTenantId;
                return View.TenantId;
            }
        }

        public void GetAgencyUserAuditHistory()
        {
            try
            {
                if (ClientId != 0 && ClientId.IsNotNull())
                {
                    View.ListAgencyUserAuditHistory = ProfileSharingManager.AgencyUserAuditHistory(View.SelectedTenantId,View.SelectedAgencyID, View.RotationName, View.ApplicantName, View.UpdatedByName, View.UpdatedDate, View.GridCustomPaging);
                    if (View.ListAgencyUserAuditHistory.IsNotNull() && View.ListAgencyUserAuditHistory.Count > 0)
                    {
                        if (View.ListAgencyUserAuditHistory[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ListAgencyUserAuditHistory[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
                else
                {
                    View.ListAgencyUserAuditHistory = new List<AgencyUserAuditHistoryContract>();
                }
            }
            catch (Exception e)
            {
                View.ListAgencyUserAuditHistory = new List<AgencyUserAuditHistoryContract>();
                throw e;
            }
        }

        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        #endregion
    }
}
