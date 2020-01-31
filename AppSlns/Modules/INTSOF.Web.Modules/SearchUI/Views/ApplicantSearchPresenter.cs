using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.Search.Views
{
    public class ApplicantSearchPresenter : Presenter<IApplicantSearchView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private ISearchController _controller;
        // public ApplicantSearchPresenter([CreateNew] ISearchController controller)
        // {
        // 		_controller = controller;
        // }

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
            //if (IsDefaultTenant)
            //{
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
            //}
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
        public void GetAdminProgramStudy()
        {
            //Entity.Organization organization = SecurityManager.GetOrganizationForTenant(ClientId);
            //if (organization == null)
            //    View.lstAdminProgramStudy = new List<Entity.AdminProgramStudy>();
            //else
            //    View.lstAdminProgramStudy = SecurityManager.GetAllPrograms(organization.OrganizationID).ToList();

            //View.lstAdminProgramStudy = SecurityManager.GetAllProgramsForTenantID(ClientId).ToList();

        }

        /// <summary>
        /// To get Admin Program Study
        /// </summary>
        public void GetAllInstituteNodePrograms()
        {
            if (ClientId == 0)
                View.lstInstituteNodePrgrams = new List<InstitutionNode>();
            else
                View.lstInstituteNodePrgrams = ComplianceDataManager.GetAllInstituteNodePrograms(ClientId, NodeType.Program.GetStringValue()).ToList();
        }

        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch()
        {
            if (ClientId == 0)
            {
                View.ApplicantSearchData = new List<ApplicantSearchDataContract>();
                View.VirtualPageCount = 0;
                View.CurrentPageIndex = 1;
                //                View.ApplicantSearchData = new List<vwComplianceApplicantSearch>();
            }
            else
            {
                INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract searchDataContract = new INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract();
                searchDataContract.ClientID = ClientId;
                searchDataContract.ApplicantFirstName = View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = View.ApplicantLastName;
                searchDataContract.DateOfBirth = View.DateOfBirth;
                searchDataContract.EmailAddress = View.EmailAddress;
                searchDataContract.OrganizationUserId = View.ApplicantUserId;
                //searchDataContract.ApplicantSSN = View.ApplicantSSN;
                searchDataContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.ApplicantSSN);
                searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                if (View.SelectedArchiveStateCode.IsNotNull())
                {
                    searchDataContract.LstArchiveState = View.SelectedArchiveStateCode;
                    searchDataContract.ArchieveStateIDForItemSearch = GetArchiveStateId();
                }
                try
                {
                    View.GridCustomPaging.DefaultSortExpression = QueueConstants.APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS;
                    //View.GridCustomPaging.SecondarySortExpression = QueueConstants.APPLICANT_SEARCH_SECONDARY_SORTING_FIELDS;

                    if (View.TenantId == SecurityManager.DefaultTenantID)
                    {
                        searchDataContract.IsRestricted = false;
                        View.ApplicantSearchData = ComplianceDataManager.GetApplicantListDataValues(searchDataContract, View.GridCustomPaging);
                        //View.ApplicantSearchData = ComplianceDataManager.PerformSearch<vwComplianceApplicantSearch>(searchDataContract, View.GridCustomPaging).ToList();
                    }
                    else
                    {
                        searchDataContract.IsRestricted = true;
                        View.ApplicantSearchData = ComplianceDataManager.GetApplicantListDataValues(searchDataContract, View.GridCustomPaging);

                        //                        var applicantSearchRestricted = ComplianceDataManager.PerformSearch<vwComplianceApplicantSearchRestricted>(searchDataContract, View.GridCustomPaging).ToList();

                        //View.ApplicantSearchData = applicantSearchRestricted
                        //    .Select(x => new vwComplianceApplicantSearch
                        //    {
                        //        ApplicantFirstName = x.ApplicantFirstName,
                        //        ApplicantLastName = x.ApplicantLastName,
                        //        ApplicantSSN = x.ApplicantSSN,
                        //        ClientID = x.ClientID,
                        //        DateOfBirth = x.DateOfBirth,
                        //        EmailAddress = x.EmailAddress,
                        //        OrganizationUserId = x.OrganizationUserId,
                        //        TenantName = x.TenantName

                        //    })
                        //    .ToList();
                    }

                    View.VirtualPageCount = View.GridCustomPaging.VirtualPageCount;
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                catch (Exception e)
                {
                    View.ApplicantSearchData = new List<ApplicantSearchDataContract>();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Getting Formmated SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }


        public void GetSSNSetting()
        {
            View.IsSSNDisabled = (View.SelectedTenantId > 0 && View.SelectedTenantId != SecurityManager.DefaultTenantID) ? ComplianceDataManager.GetSSNSetting(View.SelectedTenantId, Setting.DISABLE_SSN.GetStringValue()) : false;
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

        #region UAT-977
        private String GetArchiveStateId()
        {
            return ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId).FirstOrDefault(x => x.AS_Code.Equals(View.SelectedArchiveStateCode.FirstOrDefault())).AS_ID.ToString();
        }

        /// <summary>
        /// Gets the list of Archive State.
        /// </summary>
        public void GetArchiveStateList()
        {
            View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId);
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

        #endregion
    }
}




