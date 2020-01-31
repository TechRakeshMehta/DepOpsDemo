using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RequirementDataAuditHistoryPresenter : Presenter<IRequirementDataAuditHistory>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
        }

        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
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

        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<Entity.Tenant> lstTemp = SecurityManager.GetTenants(SortByName, false, clientCode);
            lstTemp.Insert(0, new Entity.Tenant { TenantID = 0, TenantName = "--Select--" });
            View.lstTenant = lstTemp;
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void GetRequirementPackage()
        {
            if (View.SelectedTenantId != AppConsts.NONE)
            {
                ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
                serviceRequest.Parameter1 = View.SelectedTenantId;
                serviceRequest.Parameter2 = View.SelectedReqPackageTypeID;
                var _serviceResponse = _clientRotationProxy.GetRequirementPackage(serviceRequest);

                if (!_serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstRequirementPackage = _serviceResponse.Result;
                }
                else
                {
                    View.lstRequirementPackage = new List<RequirementPackageContract>();
                }
            }
            else
            {
                View.lstRequirementPackage = new List<RequirementPackageContract>();
            }

        }

        #region UAT-4019
        public void GetRequirementPackageTypes()
        {
            if (View.SelectedTenantId != AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.Parameter = View.SelectedTenantId;
                var _serviceResponse = _clientRotationProxy.GetSharedRequirementPackageTypes();
                //View.lstRequirementPackageType = _serviceResponse.Result;
                if (!_serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstRequirementPackageType = _serviceResponse.Result;
                }
                else
                {
                    View.lstRequirementPackageType = new List<RequirementPackageTypeContract>();
                }
            }
            else
            {
                View.lstRequirementPackageType = new List<RequirementPackageTypeContract>();
            }
        }
        #endregion

        public void GetRequirementCategory()
        {
            if (View.SelectedTenantId != AppConsts.NONE)
            {
                ServiceRequest<Int32, List<Int32>> serviceRequest = new ServiceRequest<Int32, List<Int32>>();
                serviceRequest.Parameter1 = View.SelectedTenantId;
                serviceRequest.Parameter2 = View.SelectedPackageIds.IsNullOrEmpty() ? new List<Int32>() : View.SelectedPackageIds;

                var _serviceResponse = _clientRotationProxy.GetRequirementCategory(serviceRequest);

                if (!_serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstRequirementCategory = _serviceResponse.Result;
                }
                else
                {
                    View.lstRequirementCategory = _serviceResponse.Result;
                }
            }
            else
            {
                View.lstRequirementCategory = new List<RequirementCategoryContract>();
            }

        }

        public void GetRequirementItem()
        {
            if (View.SelectedTenantId != AppConsts.NONE)
            {
                ServiceRequest<Int32, List<Int32>> serviceRequest = new ServiceRequest<Int32, List<Int32>>();
                serviceRequest.Parameter1 = View.SelectedTenantId;
                serviceRequest.Parameter2 = View.SelectedCategoryIds.IsNullOrEmpty() ? new List<Int32>() : View.SelectedCategoryIds;

                var _serviceResponse = _clientRotationProxy.GetRequirementItem(serviceRequest);

                if (!_serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstRequirementItems = _serviceResponse.Result;
                }
                else
                {
                    View.lstRequirementItems = new List<RequirementItemContract>();
                }
            }
            else
            {
                View.lstRequirementItems = new List<RequirementItemContract>();
            }

        }

        public void GetAllUserGroups()
        {
            List<Entity.ClientEntity.UserGroup> tempList = new List<Entity.ClientEntity.UserGroup>();

            if (ClientId != AppConsts.NONE)
            {
                tempList = ComplianceSetupManager.GetAllUserGroup(ClientId).OrderBy(x => x.UG_Name).ToList(); //UAT- sort dropdowns by Name
                tempList.Insert(0, new Entity.ClientEntity.UserGroup { UG_ID = 0, UG_Name = "--Select--" });
                View.lstUserGroup = tempList;
            }
            else
            {
                tempList.Insert(0, new Entity.ClientEntity.UserGroup { UG_ID = 0, UG_Name = "--Select--" });
                View.lstUserGroup = tempList;
            }
        }

        public void GetDataAuditHistory()
        {
            if (ClientId != 0 && ClientId.IsNotNull())
            {
                ApplicantRequirementDataAuditSearchContract searchDataContract = new ApplicantRequirementDataAuditSearchContract();

                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.FirstName) ? null : View.FirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.LastName) ? null : View.LastName;
                searchDataContract.AdminFirstName = String.IsNullOrEmpty(View.AdminFirstName) ? null : View.AdminFirstName;
                searchDataContract.AdminLastName = String.IsNullOrEmpty(View.AdminLastName) ? null : View.AdminLastName;
                searchDataContract.ComplioId = String.IsNullOrEmpty(View.ComplioId) ? null : View.ComplioId;  //UAT-3117

                if (View.SelectedUserGroupId != AppConsts.NONE && View.SelectedUserGroupId.IsNotNull())
                    searchDataContract.FilterUserGroupID = View.SelectedUserGroupId;

                if (!View.SelectedCategoryIds.IsNullOrEmpty())
                    searchDataContract.CategoryIDs = String.Join(",", View.SelectedCategoryIds);

                if (!View.SelectedPackageIds.IsNullOrEmpty())
                    searchDataContract.PackageIDs = String.Join(",", View.SelectedPackageIds);

                searchDataContract.ItemID = View.SelectedItemID;

                if (!View.TimeStampFromDate.IsNullOrEmpty() && View.TimeStampFromDate != DateTime.MinValue)
                    searchDataContract.FromDate = View.TimeStampFromDate;
                else
                    searchDataContract.FromDate = null;

                if (!View.TimeStampToDate.IsNullOrEmpty() && View.TimeStampToDate != DateTime.MinValue)
                    searchDataContract.ToDate = View.TimeStampToDate.AddDays(1).AddSeconds(-1);
                else
                    searchDataContract.ToDate = null;

                searchDataContract.PackageTypeID = View.SelectedReqPackageTypeID; //UAT-4019

                ServiceRequest<Int32, ApplicantRequirementDataAuditSearchContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<Int32, ApplicantRequirementDataAuditSearchContract, CustomPagingArgsContract>();
                serviceRequest.Parameter1 = View.SelectedTenantId;
                serviceRequest.Parameter2 = searchDataContract;
                serviceRequest.Parameter3 = View.GridCustomPaging;

                var _serviceResponse = _clientRotationProxy.GetApplicantRequirementDataAudit(serviceRequest);

                foreach (var item in _serviceResponse.Result)
                {
                    item.ChangeValue = item.ChangeValue?.Replace("###", "<br/>");
                }

                if (!_serviceResponse.Result.IsNullOrEmpty())
                {
                    View.ApplicantRequirementDataAuditList = _serviceResponse.Result;

                    if (View.ApplicantRequirementDataAuditList.Count > 0)
                    {
                        if (View.ApplicantRequirementDataAuditList[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ApplicantRequirementDataAuditList[0].TotalCount;
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
                    View.ApplicantRequirementDataAuditList = new List<ApplicantRequirementDataAuditContract>();
                }
            }
            else
            {
                View.ApplicantRequirementDataAuditList = new List<ApplicantRequirementDataAuditContract>();
            }
        }
    }
}
