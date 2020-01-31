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

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageInvoiceGroupsPresenter : Presenter<IManageInvoiceGroupsView>
    {

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            //View.lstTenant = ComplianceDataManager.getClientTenant();
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get All Invoice Groups to bind the grid
        /// </summary>
        public void GetInvoiceGroupDetails()
        {
            View.lstInvoiceGroups = SecurityManager.GetInvoiceGroupDetails();
        }

        /// <summary>
        /// To get Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String tenantTypeCodeForClient = TenantType.Institution.GetStringValue();
            Int32 defaultTenantId = SecurityManager.DefaultTenantID;
            View.lstTenants = SecurityManager.GetTenants(SortByName, false, tenantTypeCodeForClient);
        }

        /// <summary>
        /// Get Nodes
        /// </summary>
        /// <param name="tenantID"></param>
        public void GetNodes(Int32 tenantID)
        {
            View.lstNodes = ComplianceDataManager.GetDeptProgramMappingList(tenantID);
        }

        /// <summary>
        /// Get Report Columns
        /// </summary>
        public void GetReportColumns()
        {
            View.lstReportColumns = SecurityManager.GetReportColumns();
        }

        /// <summary>
        /// Save/Update Invoice Group
        /// </summary>
        /// <param name="invoiceGroupName"></param>
        /// <param name="invoiceGroupDescription"></param>
        /// <param name="selectedNewMappedTenantIDs"></param>
        /// <param name="selectedNewMappedNodeIDs"></param>
        /// <param name="selectedNewMappedReportColumnIDs"></param>
        /// <returns></returns>
        public Boolean SaveInvoiceGroup(String invoiceGroupName, String invoiceGroupDescription, List<Int32> selectedNewMappedTenantIDs, List<String> selectedNewMappedNodeIDs,
                                        List<Int32> selectedNewMappedReportColumnIDs)
        {
            return SecurityManager.SaveUpdateInvoiceGroupInformation(View.CurrentLoggedInUserId, View.InvoiceGroupID, invoiceGroupName, invoiceGroupDescription, selectedNewMappedNodeIDs, selectedNewMappedReportColumnIDs);
        }

        /// <summary>
        /// Delete Invoice Group
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteInvoiceGroup()
        {
            return SecurityManager.DeleteInvoiceGroupInformation(View.CurrentLoggedInUserId, View.InvoiceGroupID);
        }
    }
}




