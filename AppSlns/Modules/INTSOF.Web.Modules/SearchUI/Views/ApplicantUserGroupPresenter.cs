using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils.Consts;

namespace CoreWeb.Search.Views
{
    public class ApplicantUserGroupPresenter : Presenter<IApplicantUserGroupView>
    {


        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
            GetGranularPermissionForDOBandSSN();
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        /// <summary>
        /// To get Tenant Id
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// To get Client Id
        /// </summary>
        private Int32 ClientId
        {
            get
            {
                if (IsDefaultTenant)
                    return View.SelectedTenantId;
                return View.TenantId;
            }
        }

        /// <summary>
        /// To get Admin Program Study
        /// </summary>
        public void GetAllUserGroups()
        {
            if (ClientId == 0)
                View.lstUserGroup = new List<UserGroup>();
            else
            {
                //UAT-2284: User Group permisson/access and availability by node
                Int32? currentUserId = IsDefaultTenant ? (Int32?)null : View.CurrentLoggedInUserId;
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(ClientId, currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }

        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch()
        {
            if (ClientId == 0)
            {
                View.ApplicantSearchData = new List<ApplicantDataList>();
            }
            else if (View.MatchUserGroupId == 0)
            {
                View.ApplicantSearchData = new List<ApplicantDataList>();
                View.InfoMessage = "Please select a user group.";
            }
            else
            {
                SearchItemDataContract searchDataContract = new INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                if (View.OrganizationUserID > SysXDBConsts.NONE)
                {
                    searchDataContract.OrganizationUserId = View.OrganizationUserID;
                }
                searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
                //searchDataContract.ApplicantSSN = String.IsNullOrEmpty(View.SSN) ? null : View.SSN;
                searchDataContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSN);
                searchDataContract.DateOfBirth = View.DateOfBirth;
                //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
                //if (View.DPM_ID > SysXDBConsts.NONE)
                //{
                //    searchDataContract.DPM_Id = View.DPM_ID;
                //}
                if (!View.DPM_IDs.IsNullOrEmpty())
                {
                    searchDataContract.SelectedDPMIds = View.DPM_IDs;
                }
                searchDataContract.CustomFields = String.IsNullOrEmpty(View.CustomFields) ? null : View.CustomFields;
                if (View.MatchUserGroupId != SysXDBConsts.NONE)
                {
                    searchDataContract.MatchUserGroupID = View.MatchUserGroupId;
                }

                #region UAT-2535
                if (View.LstSelectedUserGrpIDs.Count > AppConsts.NONE)
                {
                    searchDataContract.MatchedSelectedUserGroupIDs = View.LstSelectedUserGrpIDs;
                }
                #endregion

                if (View.FilterUserGroupId != SysXDBConsts.NONE)
                {
                    if (View.IsResult != 0)
                    {
                        searchDataContract.FilterUserGroupID = View.FilterUserGroupId;
                        searchDataContract.IsUserGroupAssigned = View.IsUserGroupAssigned;
                    }
                    //searchDataContract.IsUserGroupAssigned = null;
                }

                if (View.TenantId != SecurityManager.DefaultTenantID)
                {
                    searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                }

                searchDataContract.LoggedInUserTenantId = View.TenantId;

                #region UAT-1088
                if (!View.OrderCreatedFrom.IsNullOrEmpty() && View.OrderCreatedFrom != DateTime.MinValue)
                {
                    searchDataContract.OrderCreatedFrom = View.OrderCreatedFrom;
                }
                else
                {
                    searchDataContract.OrderCreatedFrom = null;
                }
                if (!View.OrderCreatedTo.IsNullOrEmpty() && View.OrderCreatedTo != DateTime.MinValue)
                {
                    searchDataContract.OrderCreatedTo = View.OrderCreatedTo;
                }
                else
                {
                    searchDataContract.OrderCreatedTo = null;
                }
                #endregion

                if (View.SelectedArchiveStateCode.IsNotNull())
                {
                    searchDataContract.LstArchiveState = View.SelectedArchiveStateCode;
                    searchDataContract.ArchieveStateId = GetXMLString(GetArchiveStateId());
                }
                try
                {
                    View.GridCustomPaging.SortExpression = View.SortColumnName;
                    View.GridCustomPaging.SortDirectionDescending = View.SortColumnNameType;
                    View.ApplicantSearchData = ComplianceDataManager.GetApplicantPortfolioSearch(ClientId, searchDataContract, View.GridCustomPaging);
                    if (View.ApplicantSearchData.IsNotNull() && View.ApplicantSearchData.Count > 0)
                    {
                        if (View.ApplicantSearchData[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ApplicantSearchData[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                        if (View.CurrentPageIndex == 1 && View.AssignOrganizationUserIds.Count == 0)
                        {
                            SetAssignedUsersDic();
                        }
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                    }
                }
                catch (Exception e)
                {
                    View.ApplicantSearchData = null;
                    throw e;
                }
            }
        }

        public void SetAssignedUsersDic()
        {
            List<Int32> lstUserGroupMapping = ComplianceDataManager.GetUsersMappedUserGroup(ClientId, View.MatchUserGroupId);
            if (lstUserGroupMapping.IsNotNull() && lstUserGroupMapping.Count > 0)
            {
                if (View.OrganizationUserID > SysXDBConsts.NONE)
                {
                    lstUserGroupMapping = lstUserGroupMapping.Where(cond => cond == View.OrganizationUserID).ToList();
                }
                Dictionary<Int32, Boolean> lstOrganizationUserIds = new Dictionary<Int32, Boolean>();
                foreach (var item in lstUserGroupMapping)
                {
                    lstOrganizationUserIds.Add(item, true);
                }
                View.AssignOrganizationUserIds = lstOrganizationUserIds;
            }
        }

        public Boolean AssignUserGroupToUsers(Dictionary<Int32, Boolean> selectedItems)
        {
            if (ClientId == 0)
            {
                View.InfoMessage = "Please select an institute to assign applicants to user group.";
                return false;
            }
            else if (View.MatchUserGroupId == 0)
            {
                View.InfoMessage = "Please select user group to assign applicants to user group.";
                return false;
            }
            else if (selectedItems.Count <= 0)
            {
                View.InfoMessage = "Please select at least one applicants to be assigned.";
                return false;
            }
            else
            {
                if (ComplianceDataManager.AssignUserGroupToUsers(ClientId, selectedItems, View.MatchUserGroupId, View.CurrentLoggedInUserId))
                {
                    View.SuccessMessage = "User group mapping saved successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred. Please try again.";
                    return false;
                }
            }
        }

        /// <summary>
        /// Getting Formatted SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetFormattedSSN(String unformattedSSN)
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

        /// <summary>
        /// Gets the list of Archive State.
        /// </summary>
        public void GetArchiveStateList()
        {
            if (View.SelectedTenantId ==AppConsts.NONE)
            {
                View.lstArchiveState = new List<lkpArchiveState>();
            }
            else
            {
                View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId);
            }
        }

        private String GetXMLString(List<Int32> listOfIds)
        {
            if (listOfIds.IsNotNull() && listOfIds.Count > 0)
            {
                StringBuilder IdString = new StringBuilder();
                foreach (Int32 id in listOfIds)
                {
                    IdString.Append("<Root><Value>" + id.ToString() + "</Value></Root>");
                }

                return IdString.ToString();
            }
            return null;
        }

        private List<Int32> GetArchiveStateId()
        {
            return new List<Int32>{
            ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId).FirstOrDefault(x => x.AS_Code.Equals(View.SelectedArchiveStateCode.FirstOrDefault())).AS_ID};

        }
    }
}




