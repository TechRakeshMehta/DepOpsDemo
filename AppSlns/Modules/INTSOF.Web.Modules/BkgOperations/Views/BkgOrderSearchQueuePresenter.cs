#region NameSpace
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Configuration;
#endregion
namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderSearchQueuePresenter : Presenter<IBkgOrderSearchQueueView>
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

            GetGranularPermissionForClientAdmins();//UAT-806 and UAT-1075
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return Business.RepoManagers.SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Gets the list of Payment status.
        /// </summary>
        public void GetOrderStatusList()
        {
            View.lstPaymentStatus = ComplianceDataManager.GetOrderStatusList(View.SelectedTenantId);
        }

        /// <summary>
        /// Gets the list of Master Backround Service.
        /// </summary>
        public void GetBackroundServiceList()
        {
            //View.lstBackroundServices = BackgroundSetupManager.GetMasterServices();
            View.lstBackroundServices = BackgroundSetupManager.GetTenantServices(View.SelectedTenantId);
        }

        /// <summary>
        /// Gets the data from table ApplicantComplianceDataItems.
        /// </summary>
        public void GetBkgOrderQueueData()
        {
            // if (!(searchBkgOrdeContract.ClientID == Business.RepoManagers.SecurityManager.DefaultTenantID && View.SelectedTenantId == AppConsts.NONE))
            if (View.IsAdminUser && View.SelectedTenantId > AppConsts.NONE || (!View.IsAdminUser && View.SelectedTenantId > AppConsts.NONE))
            {
                BkgOrderSearchContract searchBkgOrdeContract = new BkgOrderSearchContract();
                //searchItemDataContract.LstStatusCode = new List<String>();

                searchBkgOrdeContract = GetSearchContract();

                try
                {
                    View.GridCustomPaging.DefaultSortExpression = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
                    View.GridCustomPaging.SecondarySortExpression = QueueConstants.ORDER_QUEUE_SECONDARY_SORTING_FIELDS;

                    BackroundOrderSearchContract backroundOrderSearchContract = StoredProcedureManagers.GetBackroundOrderSearch(View.GridCustomPaging, searchBkgOrdeContract, searchBkgOrdeContract.ClientID);
                    View.lstBackroundOrderSearchContract = backroundOrderSearchContract;
                    View.lstBackroundOrder = backroundOrderSearchContract.BackroundOrder;
                    if (View.lstBackroundOrder.IsNotNull() && View.lstBackroundOrder.Count > 0)
                    {
                        if (View.lstBackroundOrder[0].TotalCount > 0)
                        {
                            View.VirtualPageCount = View.lstBackroundOrder[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualPageCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                    //UAT-1456 related changes.
                    View.GridCustomPaging.VirtualPageCount = View.VirtualPageCount;
                    View.GridCustomPaging.CurrentPageIndex = View.CurrentPageIndex;
                    searchBkgOrdeContract.GridCustomPagingArguments = View.GridCustomPaging;
                    View.SetBkgOrderSearchContract = searchBkgOrdeContract;
                }
                catch (Exception e)
                {
                    View.lstBackroundOrderSearch = null;
                    throw e;
                }
            }
        }

        private BkgOrderSearchContract GetSearchContract()
        {
            BkgOrderSearchContract searchBkgOrdeContract = new BkgOrderSearchContract();

            searchBkgOrdeContract.ClientID = View.TenantId;

            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if (searchBkgOrdeContract.ClientID == Business.RepoManagers.SecurityManager.DefaultTenantID && View.SelectedTenantId != AppConsts.NONE)
            {
                searchBkgOrdeContract.ClientID = View.SelectedTenantId;
            }

            if (View.OrderFromDate.IsNotNull() && View.OrderToDate.IsNotNull())
            {
                searchBkgOrdeContract.OrderFromDate = View.OrderFromDate;
                searchBkgOrdeContract.OrderToDate = View.OrderToDate;
            }
            if (View.PaidFromDate.IsNotNull() && View.PaidToDate.IsNotNull())
            {
                searchBkgOrdeContract.PaidFromDate = View.PaidFromDate;
                searchBkgOrdeContract.PaidToDate = View.PaidToDate;
            }
            if (View.OrderCompletedFromDate.IsNotNull() && View.OrderCompletedToDate.IsNotNull())
            {
                searchBkgOrdeContract.OrderCompletedFromDate = View.OrderCompletedFromDate;
                searchBkgOrdeContract.OrderCompletedToDate = View.OrderCompletedToDate;
            }
            if (View.OrderPaymentStatusID.IsNotNull() && View.OrderPaymentStatusID > AppConsts.NONE)
            {
                searchBkgOrdeContract.OrderPaymentStatusID = View.OrderPaymentStatusID;
            }
            if (View.OrderStatusTypeID.IsNotNull() && View.OrderStatusTypeID > AppConsts.NONE)
            {
                searchBkgOrdeContract.OrderStatusTypeID = View.OrderStatusTypeID;
            }
            if (View.ServiceID.IsNotNull() && View.ServiceID > AppConsts.NONE)
            {
                searchBkgOrdeContract.ServiceID = View.ServiceID;
            }
            if (View.ServiceFormStatusID.IsNotNull() && View.ServiceFormStatusID > AppConsts.NONE)
            {
                searchBkgOrdeContract.ServiceFormStatusID = View.ServiceFormStatusID;
            }
            //if (!View.SSN.IsNullOrEmpty())
            //{
            //    searchBkgOrdeContract.ApplicantSSN = View.SSN;
            //}
            searchBkgOrdeContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSN);
            if (View.DOB.IsNotNull())
            {
                searchBkgOrdeContract.DateOfBirth = View.DOB;
            }
            if (View.InstitutionStatusColorID.IsNotNull() && View.InstitutionStatusColorID > AppConsts.NONE)
            {
                searchBkgOrdeContract.InstitutionStatusColorID = View.InstitutionStatusColorID;
            }
            if (View.ServiceGroupId > AppConsts.NONE)
            {
                searchBkgOrdeContract.ServiceGroupId = View.ServiceGroupId;
            }
            else
            {
                searchBkgOrdeContract.ServiceGroupId = null;
            }
            if (View.OrderClientStatusID > AppConsts.NONE)
            {
                searchBkgOrdeContract.OrderClientStatusID = View.OrderClientStatusID;
            }
            else
            {
                searchBkgOrdeContract.OrderClientStatusID = null;
            }
            //If client admin than set the IsServiceGroupRequired true
            if (!View.IsAdminUser)
            {
                searchBkgOrdeContract.IsServiceGroupRequired = true;
            }
            else
            {
                searchBkgOrdeContract.IsServiceGroupRequired = false;
            }
            if (View.IsArchive.IsNotNull())
            {
                searchBkgOrdeContract.IsArchive = View.IsArchive;
            }

            if (View.IsPaymentStatusChecked.IsNotNull())
            {
                searchBkgOrdeContract.IsPaymentStatusChecked = View.IsPaymentStatusChecked;
            }
            if (View.IsClientStatusChecked.IsNotNull())
            {
                searchBkgOrdeContract.IsClientStatusChecked = View.IsClientStatusChecked;
            }

            //UAT-1732: Change Yes/No checkbox for Is flagged to radio buttons with "Flagged", "Not Flagged", and "all"
            //if (View.IsFlagged.IsNotNull())
            //{
            //    searchBkgOrdeContract.IsFlagged = View.IsFlagged;
            //}
            //if (View.IsFlaggedChecked.IsNotNull())
            //{
            //    searchBkgOrdeContract.IsFlaggedChecked = View.IsFlaggedChecked;
            //}
            //ZERO means Not Flagged, ONE means Flagged, TWO mean All Flagged
            if (!View.SelectedFlagged.IsNullOrEmpty())
            {
                if (View.SelectedFlagged == AppConsts.ZERO)
                {
                    searchBkgOrdeContract.IsFlagged = false;
                }
                else if (View.SelectedFlagged == AppConsts.STR_ONE)
                {
                    searchBkgOrdeContract.IsFlagged = true;
                }
                else
                {
                    searchBkgOrdeContract.IsFlagged = null;
                }
                searchBkgOrdeContract.SelectedFlagged = View.SelectedFlagged;
            }

            if (View.IsArchiveChecked.IsNotNull())
            {
                searchBkgOrdeContract.IsArchiveChecked = View.IsArchiveChecked;
            }
            if (View.CurrentLoggedInUserId.IsNotNull() && View.CurrentLoggedInUserId > AppConsts.NONE)
            {
                searchBkgOrdeContract.LoggedInUserId = View.IsAdminUser ? (Int32?)null : View.CurrentLoggedInUserId;
            }
            if (View.UserGroupID.IsNotNull() && View.UserGroupID > AppConsts.NONE)
            {
                searchBkgOrdeContract.UserGroupID = View.UserGroupID;
            }

            //if (View.TargetHierarchyNodeId.IsNotNull() && View.TargetHierarchyNodeId > AppConsts.NONE)
            //{
            //    searchBkgOrdeContract.DeptProgramMappingID = View.TargetHierarchyNodeId;
            //}
            //searchBkgOrdeContract.DeptProgramMappingIDs = View.TargetHierarchyNodeIds;
            //if (!View.TargetHierarchyNodeIds.IsNullOrEmpty())
            //{
            //    searchBkgOrdeContract.DeptProgramMappingIDs = View.TargetHierarchyNodeIds;
            //}
            //else
            //{
            //    searchBkgOrdeContract.DeptProgramMappingIDs = null;
            //}
            searchBkgOrdeContract.ApplicantFirstName = String.IsNullOrEmpty(View.FirstNameSearch) ? null : View.FirstNameSearch;
            searchBkgOrdeContract.ApplicantLastName = String.IsNullOrEmpty(View.LastNameSearch) ? null : View.LastNameSearch;
            if (!View.OrderNumberSearch.IsNullOrEmpty())
            {
                searchBkgOrdeContract.OrderNumber = View.OrderNumberSearch;
            }
            if (!View.SelectedArchiveStateCode.IsNullOrWhiteSpace())
            {
                searchBkgOrdeContract.SelectedArchieveStateId = View.SelectedArchiveStateCode;
            }
            //UAT-1723
            if (!View.DPM_IDs.IsNullOrEmpty())
            {
                searchBkgOrdeContract.DeptProgramMappingIDs = View.DPM_IDs;
            }
            else
            {
                searchBkgOrdeContract.DeptProgramMappingIDs = null;
            }
            searchBkgOrdeContract.NodeIds = View.NodeIds.IsNullOrEmpty() ? null : View.NodeIds;
            searchBkgOrdeContract.CustomFields = String.IsNullOrEmpty(View.CustomFields) ? null : View.CustomFields;
            searchBkgOrdeContract.NodeLabel = View.NodeLabel;
            return searchBkgOrdeContract;
        }

        public void GetAllOrderIds()
        {
            BkgOrderSearchContract searchBkgOrdeContract = new BkgOrderSearchContract();
            searchBkgOrdeContract = GetSearchContract();

            View.GridCustomPaging.DefaultSortExpression = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
            View.GridCustomPaging.SecondarySortExpression = QueueConstants.ORDER_QUEUE_SECONDARY_SORTING_FIELDS;
            View.GridCustomPaging.PageSize = View.GridCustomPaging.VirtualPageCount;

            BackroundOrderSearchContract backroundOrderSearchContract = StoredProcedureManagers.GetBackroundOrderSearch(View.GridCustomPaging, searchBkgOrdeContract, searchBkgOrdeContract.ClientID);

            if (!backroundOrderSearchContract.IsNullOrEmpty()
                && !backroundOrderSearchContract.BackroundOrder.IsNullOrEmpty()
                && backroundOrderSearchContract.BackroundOrder.Count > 0)
            {
                //StringBuilder sb = new StringBuilder(string.Empty);

                View.SelectedOrderIds = string.Join(",", backroundOrderSearchContract.BackroundOrder.Select(cond => cond.OrderID).Distinct().ToList());

                //foreach (BackroundOrderContract item in backroundOrderSearchContract.BackroundOrder)
                //{
                //    sb.Append(string.Join(",", item.OrderID.ToString()));
                //}
                //View.SelectedOrderIds = sb.ToString();
            }
        }

        /// <summary>
        /// check the order is dispatched to clear star or not
        /// </summary>
        /// <param name="bkgOrderId">bkgOrderId</param>
        /// <returns>ExternalVendorBkgOrderDetail</returns>
        public Entity.ClientEntity.ExternalVendorBkgOrderDetail GetExternalVendorBkgOrderDetail(Int32 bkgOrderId)
        {
            return BackgroundProcessOrderManager.GetExternalVendorBkgOrderDetail(View.SelectedTenantId, bkgOrderId);
        }
        /// <summary>
        /// Get Institution color flag 
        /// </summary>
        /// <returns>List<Entity.ClientEntity.InstitutionOrderFlag></returns>
        public List<Entity.ClientEntity.InstitutionOrderFlag> GetInstitutionStatusColor()
        {
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                var lstInstitutionOrderFlags = BackgroundProcessOrderManager.GetInstitutionStatusColor(View.SelectedTenantId);

                //UAT-2178: Color flag column should only show when color flag is enabled for a tenant.
                if (!View.IsAdminUser && !lstInstitutionOrderFlags.IsNullOrEmpty())
                {
                    View.IsInstitutionHasOrderFlag = true;
                }

                return View.InstitutionOrderFlagList = lstInstitutionOrderFlags;
            }
            return null;
        }
        /// <summary>
        /// Get all service group list 
        /// </summary>
        /// <returns> List<Entity.ClientEntity.BkgSvcGroup></returns>
        public List<Entity.ClientEntity.BkgSvcGroup> GetBkgServiceGroup()
        {
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                return View.lstBkgSvcGroup = BackgroundProcessOrderManager.GetBackroundServiceGroup(View.SelectedTenantId);
            }
            return null;
        }
        /// <summary>
        /// Get client status based on the tenant id
        /// </summary>
        /// <returns>List<Entity.ClientEntity.BkgOrderClientStatu></returns>
        public List<Entity.ClientEntity.BkgOrderClientStatu> GetBkgOrderClientStatus()
        {
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                return View.lstOrderClientStatus = BackgroundProcessOrderManager.GetBkgOrderClientStatus(View.SelectedTenantId);
            }
            return null;
        }
        /// <summary>
        /// Get service group based on the order id
        /// </summary>
        /// <param name="orderId">orderId</param>
        public void GetServiceGroupListByOrderId(Int32 orderId)
        {
            View.lstBackroundServiceGroup = View.lstBackroundOrderSearchContract.BackroundServiceGroup.Where(obj => obj.OrderID == orderId).ToList();
        }

        /// <summary>
        /// Get services based on the service group Id
        /// </summary>
        /// <param name="bkgOrderPackageServiceGroupId">bkgOrderPackageServiceGroupId</param>
        public void GetServicesListByServiceGroupId(Int32 bkgOrderPackageServiceGroupId)
        {
            View.lstBackroundServicesContract = View.lstBackroundOrderSearchContract.BackroundServices.Where(obj => obj.BkgOrderPackageSvcGroupID == bkgOrderPackageServiceGroupId).ToList();
        }
        /// <summary>
        /// Get Institution color flag based on the institution status color Id
        /// </summary>
        /// <param name="institutionStatusColorId">institutionStatusColorId</param>
        /// <returns></returns>
        public Entity.ClientEntity.InstitutionOrderFlag GetOrderInstitutionStatusColor(Int32 institutionStatusColorId)
        {
            if (View.SelectedTenantId > AppConsts.NONE && institutionStatusColorId > AppConsts.NONE)
            {
                return BackgroundProcessOrderManager.GetOrderInstitutionStatusColor(View.SelectedTenantId, institutionStatusColorId);
            }
            return null;
        }
        /// <summary>
        /// Get client status based on the order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>orderId</returns>
        public Int32 GetClientStatusByOrderId(Int32 orderId)
        {
            if (View.SelectedTenantId > AppConsts.NONE && orderId > AppConsts.NONE)
            {
                return BackgroundProcessOrderManager.GetClientStatusByOrderId(View.SelectedTenantId, orderId);
            }
            return 0;
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public void GetOrderStatusType()
        {
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                View.lstOrderStatusType = BackgroundProcessOrderManager.GetOrderRequestType(View.SelectedTenantId);
            }
        }
        /// <summary>
        /// Get service form status 
        /// </summary>
        public void GetServiceFormStatus()
        {
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                View.lstServiceFormStatus = BackgroundProcessOrderManager.GetServiceFormStatus(View.SelectedTenantId);
            }
        }
        /// <summary>
        /// Get background order detail based on the masterOrderId
        /// </summary>
        /// <param name="masterOrderId"></param>
        /// <returns></returns>
        public Entity.ClientEntity.BkgOrder GetBkgOrderDetail(Int32 masterOrderId)
        {
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                return BackgroundProcessOrderManager.GetBkgOrderDetail(View.SelectedTenantId, masterOrderId);
            }
            return null;
        }
        /// <summary>
        /// Update archive status with respect to the order
        /// </summary>
        /// <param name="masterOrderId"></param>
        /// <param name="archiveStatus"></param>
        /// <param name="eventDetailNotes"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Boolean UpdateBkgOrderArchiveStatus(Int32 masterOrderId, Boolean archiveStatus, String eventDetailNotes, Int32 loggedInUserId)
        {
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                return BackgroundProcessOrderManager.UpdateBkgOrderArchiveStatus(View.SelectedTenantId, masterOrderId, archiveStatus, eventDetailNotes, loggedInUserId);
            }
            return false;
        }

        /// <summary>
        /// getting FormattedSSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }

        public void GetOrganisationUserByOrder(String[] orderIDs)
        {
            Dictionary<Int32, String> selectedUsers = new Dictionary<Int32, String>();

            foreach (String orderID in orderIDs)
            {
                Entity.ClientEntity.OrganizationUser orgUser = null;
                orgUser = BackgroundProcessOrderManager.GetOrganisationUserByOrderId(View.SelectedTenantId, Convert.ToInt32(orderID));
                Int32 orgUserID = orgUser.OrganizationUserID;
                String orgUserName = orgUser.FirstName + " " + orgUser.LastName;
                if (!selectedUsers.ContainsKey(orgUserID))
                {
                    selectedUsers.Add(orgUserID, orgUserName);
                }
            }
            View.AssignOrganisationUserIDs = selectedUsers;
        }

        #region UAT-806 Creation of granular permissions for Client Admin users AND UAT-1075 WB:Admin Granular permissions for color flag and Result PDF


        public void GetGranularPermissionForClientAdmins()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (Business.RepoManagers.SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                //UAT-806
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }

                //UAT-1075
                if (dicPermissions.ContainsKey(EnumSystemEntity.BKG_ORDER_COLOR_FLAG.GetStringValue()) && dicPermissions[EnumSystemEntity.BKG_ORDER_COLOR_FLAG.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsBkgColorFlagDisable = true;
                }
                //UAT-1996
                else if (dicPermissions.ContainsKey(EnumSystemEntity.BKG_ORDER_COLOR_FLAG.GetStringValue())
                       && String.Compare(dicPermissions[EnumSystemEntity.BKG_ORDER_COLOR_FLAG.GetStringValue()], EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue()
                                         ) == AppConsts.NONE)
                {
                    View.IsBkgColorFlagFullPermission = true;
                }
                //UAT-1996:
                if (!dicPermissions.ContainsKey(EnumSystemEntity.BKG_ORDER_COLOR_FLAG.GetStringValue()))
                {
                    View.IsBkgColorFlagFullPermission = true;
                }
                //if (dicPermissions.ContainsKey(EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()) && dicPermissions[EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                //{
                //    View.IsBkgResultReportDisable = true;
                //}
                if (dicPermissions.ContainsKey(EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()))
                {
                    View.LstBkgOrderResultPermissions = dicPermissions[EnumSystemEntity.BKG_ORDER_RESULT_REPORT.GetStringValue()].Split(',').ToList();
                }

                //UAT-3010:-  Granular Permission for Client Admin Users to Archive.
                if (dicPermissions.ContainsKey(EnumSystemEntity.ARCHIVE_ABILITY.GetStringValue()))
                {
                    View.ArchivePermissionCode = dicPermissions[EnumSystemEntity.ARCHIVE_ABILITY.GetStringValue()];
                }
            }
            //UAT-1996
            else
            {
                View.IsBkgColorFlagFullPermission = true;
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
        #endregion

        /// <summary>
        /// UAT 1417
        /// </summary>
        public void GetAllUserGroups()
        {
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                //UAT-2284: User Group permisson/access and availability by node
                Int32? currentUserId = View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID ? (Int32?)null : View.CurrentLoggedInUserId;
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(View.SelectedTenantId, currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }

        public void GetBkgOrderNoteSetting()
        {
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                View.IsBkgOrderNoteEnabled = (ComplianceDataManager.GetBkgOrderNoteSetting(View.SelectedTenantId, Setting.ENABLE_BACKGROUND_ORDER_NOTES.GetStringValue()));
            }
        }

        //UAT:4522
        public void GetClientAdminGranularPermission()
        {

            if (View.CurrentLoggedInUserId > AppConsts.NONE && !View.IsAdminUser)
            {
                View.GranularPermission = ComplianceDataManager.GeNewtGranularPermission(View.TenantId, View.CurrentLoggedInUserId);
            }
        }

        public String ArchiveBkgOrder(List<Int32> selectedOrderIDs)
        {
            return StoredProcedureManagers.ArchieveBkgOrderIds(selectedOrderIDs, View.SelectedTenantId, View.CurrentLoggedInUserId);
        }

        #region UTA-4085
        public String UnArchiveBkgOrder(List<Int32> selectedOrderIDs)
        {
            return StoredProcedureManagers.UnArchieveBkgOrderIds(selectedOrderIDs, View.SelectedTenantId, View.CurrentLoggedInUserId);
        }
        #endregion

        /// <summary>
        /// Gets the list of Archive State.
        /// </summary>
        public void GetArchiveStateList()
        {
            View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId);
        }

        /// <summary>
        /// UAT 1740: Move 604 notification from the time of login to when an admin attempts for view an employment result report. 
        /// </summary>
        /// <returns></returns>
        public Boolean IsEDFormPreviouslyAccepted()
        {
            Double employmentDisclosureIntervalHours = AppConsts.NONE;
            if (!ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"].IsNullOrEmpty())
            {
                employmentDisclosureIntervalHours = Convert.ToDouble(ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"]);
            }
            return Business.RepoManagers.SecurityManager.IsEDFormPreviouslyAccepted(View.CurrentLoggedInUserId, employmentDisclosureIntervalHours);
        }

        #region UAT-1795 : Add D&A download button on Background Order Queue search.
        public void GetAllDnADocument()
        {
            if (!View.lstSeletedOrderIds.IsNullOrEmpty() && View.SelectedTenantId > AppConsts.NONE)
            {
                View.DocumentListToExport = BackgroundProcessOrderManager.GetAllDnADocument(View.SelectedTenantId, View.lstSeletedOrderIds);
            }
        }
        #endregion
    }
}
