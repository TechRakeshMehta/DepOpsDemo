using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RotationDocumentsPresenter : Presenter<IRotationDocumentsView>
    {
        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        public void GetRotationMemebers()
        {
            View.RotationMembers = new List<ApplicantDataListContract>();

            if (View.TenantId == 0)
                View.RotationMembers = new List<ApplicantDataListContract>();
            else
            {
                ClinicalRotationSearchContract searchDataContract = new ClinicalRotationSearchContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
                searchDataContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSN);
                searchDataContract.DateOfBirth = View.DateOfBirth;
                searchDataContract.LoggedInUserTenantId = View.TenantId;
                searchDataContract.ClinicalRotationID = View.ClinicalRotationID;

                ServiceRequest<ClinicalRotationSearchContract, CustomPagingArgsContract> serviceRequest = new ServiceRequest<ClinicalRotationSearchContract, CustomPagingArgsContract>();
                serviceRequest.SelectedTenantId = View.TenantId;
                serviceRequest.Parameter1 = searchDataContract;
                serviceRequest.Parameter2 = View.GridCustomPaging;
                var _serviceResponse = _clientRotationProxy.GetRotationMembersForRotationDocs(serviceRequest);
                View.RotationMembers = _serviceResponse.Result;

                if (View.RotationMembers.IsNotNull() && View.RotationMembers.Count > 0)
                {
                    if (View.RotationMembers[0].TotalCount > 0)
                    {
                        View.VirtualRecordCount = View.RotationMembers[0].TotalCount;
                    }
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                else
                {
                    View.VirtualRecordCount = 0;
                    View.CurrentPageIndex = 1;
                }
            }
        }

        public String GetMaskedSSN(String unMaskedSSN)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = unMaskedSSN;
            var _serviceResponse = _clientRotationProxy.GetMaskedSSN(serviceRequest);
            return _serviceResponse.Result;
        }

        public void GetGranularPermissions()
        {
            var _serviceResponse = _clientRotationProxy.GetGranularPermissions();
            View.dicGranularPermissions = _serviceResponse.Result;
            //ONDB:15934
            GetGranularPermissionForClientAdmins();
        }

        public void GetGranularPermissionForClientAdmins()
        {
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            if (!View.dicGranularPermissions.IsNullOrEmpty())
            {
                if (View.dicGranularPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = View.dicGranularPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
            }
        }

        public String GetFormattedSSN(String unformattedSSN)
        {
            ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
            serviceRequest.Parameter = unformattedSSN;
            var _serviceResponse = _clientRotationProxy.GetFormattedSSN(serviceRequest);
            return _serviceResponse.Result;
        }

        public void GetReqPkgCatByRotationID()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.SelectedTenantId = View.TenantId;
            serviceRequest.Parameter = View.ClinicalRotationID;
            var _serviceResponse = _clientRotationProxy.GetReqPkgCatByRotationID(serviceRequest);

            if (!_serviceResponse.Result.IsNullOrEmpty())
                View.lstRequirementCategory = _serviceResponse.Result;
            else
                View.lstRequirementCategory = new List<INTSOF.ServiceDataContracts.Modules.RequirementPackage.RequirementCategoryContract>();
        }

        public void GetRotationDocuments()
        {
            View.RotationDocuments = new List<RotationDocumentContact>();

            if (View.TenantId == 0)
                View.RotationDocuments = new List<RotationDocumentContact>();
            else
            {
                string selectedReqCatIDs = string.Join(",", View.SelectedReqCatIds);
                string selectedApplicantIds = string.Join(",", View.SelectedApplicantIds.Where(c => c.Value == true).Select(s => s.Key));

                ServiceRequest<String, String, CustomPagingArgsContract> serviceRequest = new ServiceRequest<String, String, CustomPagingArgsContract>();
                serviceRequest.SelectedTenantId = View.TenantId;
                serviceRequest.Parameter1 = selectedReqCatIDs;
                serviceRequest.Parameter2 = selectedApplicantIds;
                serviceRequest.Parameter3 = View.GridCustomPagingGrdDoc;

                var _serviceResponse = _clientRotationProxy.GetApplicantDocsByReqCatID(serviceRequest);
                View.RotationDocuments = _serviceResponse.Result;

                if (View.RotationDocuments.IsNotNull() && View.RotationDocuments.Count > 0)
                {
                    if (View.RotationDocuments[0].TotalCount > 0)
                    {
                        View.VirtualRecordCountGrdDoc = View.RotationDocuments[0].TotalCount;
                    }
                    View.CurrentPageIndexGrdDoc = View.GridCustomPagingGrdDoc.CurrentPageIndex;
                }
                else
                {
                    View.VirtualRecordCountGrdDoc = 0;
                    View.CurrentPageIndexGrdDoc = 1;
                }
            }
        }

        public void GetRotationDocumentsByDocIDs(string selectedDocIds, string reqCatIds)
        {
            View.RotationDocumentsToDownload = new List<RotationDocumentContact>();

            if (View.TenantId == 0)
                View.RotationDocumentsToDownload = new List<RotationDocumentContact>();
            else
            {
                //string selectedDocIds = string.Join(",", View.SelectedDocumentIds);

                ServiceRequest<String, String> serviceRequest = new ServiceRequest<String, String>();
                serviceRequest.SelectedTenantId = View.TenantId;
                serviceRequest.Parameter1 = selectedDocIds;
                serviceRequest.Parameter2 = reqCatIds;

                var _serviceResponse = _clientRotationProxy.GetApplicantDocumentsByDocIDs(serviceRequest);
                View.RotationDocumentsToDownload = _serviceResponse.Result;
            }
        }
    }
}
