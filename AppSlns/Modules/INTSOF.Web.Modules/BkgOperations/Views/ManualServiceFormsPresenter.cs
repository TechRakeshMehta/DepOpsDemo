#region NameSpace
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
#endregion
namespace CoreWeb.BkgOperations.Views
{
    public class ManualServiceFormsPresenter : Presenter<IManualServiceFormsView>
    {
        #region Views
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            if (Business.RepoManagers.SecurityManager.DefaultTenantID == View.TenantId)
            {
                View.IsAdminUser = true;

            }
            else
            {
                View.IsAdminUser = false;
            }
            View.lstTenant = ComplianceDataManager.getClientTenant();
        }

        #endregion

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return Business.RepoManagers.SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Gets the list of Master Backround Service.
        /// </summary>
        public void GetBackroundServiceList()
        {
            View.lstBackroundServices = BackgroundSetupManager.GetTenantServices(View.SelectedTenantId);
        }

        /// <summary>
        /// Gets the data from table ApplicantComplianceDataItems.
        /// </summary>
        public void GetManualServiceFormsData()
        {
            ManualServiceFormsSearchContract searchFormContract = new ManualServiceFormsSearchContract();
            //searchItemDataContract.LstStatusCode = new List<String>();

            searchFormContract.ClientID = View.TenantId;

            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if (searchFormContract.ClientID == Business.RepoManagers.SecurityManager.DefaultTenantID && View.SelectedTenantId != AppConsts.NONE)
            {
                searchFormContract.ClientID = View.SelectedTenantId;
            }

            // if (!(searchFormContract.ClientID == Business.RepoManagers.SecurityManager.DefaultTenantID && View.SelectedTenantId == AppConsts.NONE))
            if (View.IsAdminUser && View.SelectedTenantId > AppConsts.NONE || (!View.IsAdminUser && View.SelectedTenantId > AppConsts.NONE))
            {
                try
                {
                    if (View.ServiceID.IsNotNull() && View.ServiceID > AppConsts.NONE)
                    {
                        searchFormContract.ServiceID = View.ServiceID;
                    }
                    if (View.ServiceFormStatusID.IsNotNull() && View.ServiceFormStatusID > AppConsts.NONE)
                    {
                        searchFormContract.ServiceFormStatusID = View.ServiceFormStatusID;
                    }
                    if (!View.IsAdminUser && View.CurrentLoggedInUserId != AppConsts.NONE)
                    {
                        searchFormContract.LoggedInUserId = View.CurrentLoggedInUserId;
                    }
                    //searchFormContract.DeptProgramMappingID = View.DeptProgramMappingID;
                    searchFormContract.SelectedDeptProgramMappingID = View.SelectedDeptProgramMappingID;
                    searchFormContract.ApplicantFirstName = String.IsNullOrEmpty(View.FirstNameSearch) ? null : View.FirstNameSearch;
                    searchFormContract.ApplicantLastName = String.IsNullOrEmpty(View.LastNameSearch) ? null : View.LastNameSearch;
                    View.GridCustomPaging.DefaultSortExpression = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
                    View.GridCustomPaging.SecondarySortExpression = QueueConstants.ORDER_QUEUE_SECONDARY_SORTING_FIELDS;
                    searchFormContract.GridCustomPagingArguments = View.GridCustomPaging;
                    View.SetManualServiceFormsSearchContract = searchFormContract;


                    List<ManualServiceFormContract> lstManualServiceFormsContract = StoredProcedureManagers.GetManualServiceFormSearch(View.GridCustomPaging, searchFormContract, searchFormContract.ClientID);
                    //View.lstBackroundOrderSearchContract = backroundOrderSearchContract;
                    View.lstManualServiceForm = lstManualServiceFormsContract;
                    if (View.lstManualServiceForm.IsNotNull() && View.lstManualServiceForm.Count > 0)
                    {
                        if (View.lstManualServiceForm[0].TotalCount > 0)
                        {
                            View.VirtualPageCount = View.lstManualServiceForm[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualPageCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
                catch (Exception e)
                {
                    View.lstManualServiceForm = null;
                    throw e;
                }
            }
        }

        /// <summary>
        /// Get the List of ServiceFormStatus list.
        /// </summary>
        public void GetServiceFormStatusList()
        {
            List<Entity.ClientEntity.lkpServiceFormStatu> tempServiceFormStatusList = new List<Entity.ClientEntity.lkpServiceFormStatu>();
            tempServiceFormStatusList = BackgroundProcessOrderManager.GetServiceFormStatus(View.SelectedTenantId);
            tempServiceFormStatusList.Insert(0, new Entity.ClientEntity.lkpServiceFormStatu { SFS_Name = "--Select--", SFS_ID = 0 });
            View.ListServiceFormStatus = tempServiceFormStatusList;
        }

        public Boolean UpdateOrderServiceServiceFormStatus(Int32 notificationId, Int32 oldStatusId)
        {
            Boolean isUpdated = false;
            isUpdated = BackgroundProcessOrderManager.UpdateOrderNotificationBkgOrderServiceForm(notificationId, View.SelectedTenantId, View.CurrentLoggedInUserId, View.SelectedServiceFormStatusId, oldStatusId);
            //isUpdated = BackgroundSetupManager.UpdateOrderServiceServiceFormStatus(orderServiceFormId, View.SelectedServiceFormStatusId, View.CurrentLoggedInUserId, View.SelectedTenantId);
            return isUpdated;
        }

        /// <summary>
        /// Send notification when a manual service form status has been changed from send to student to in progress by agency.
        /// </summary>
        /// <param name="oldServiceFormStatusId"></param>
        /// <param name="manualServiceFormContract"></param>
        public void SendSvcFormStsChangeNotification(Int32 oldServiceFormStatusId, ManualServiceFormContract manualServiceFormContract)
        {
            BackgroundProcessOrderManager.SendSvcFormStsChangeNotification(View.SelectedTenantId, oldServiceFormStatusId, View.SelectedServiceFormStatusId, manualServiceFormContract);
        }

    }
}

