using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class SchoolNodeAssociationPresenter : Presenter<ISchoolNodeAssociationView>
    {
        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        #region Methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed the every time the view loads          
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetSchoolNodeAssociationByAgencyHierarchyID()
        {
            View.lstSchoolNodeAssociation = new List<SchoolNodeAssociationDataContract>();
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();

            List<SchoolNodeAssociationDataContract> lstSchoolNodeAssociationContract = new List<SchoolNodeAssociationDataContract>();
            serviceRequest.Parameter1 = AppConsts.NONE;
            serviceRequest.Parameter2 = View.AgencyHierarchyID;

            var _response = _agencyHierarchyProxy.GetSchoolNodeAssociationByAgencyHierarchyID(serviceRequest);

            if (!_response.IsNullOrEmpty())
            {

                foreach (var item in _response.Result.GroupBy(x => new { x.TenantID, x.AgencyHierarchyID }))
                {
                    lstSchoolNodeAssociationContract.Add(new SchoolNodeAssociationDataContract { CommaSeparatedDpmIds = String.Join(",", item.Select(x => x.DPM_ID)), CommaSeparatedDpmlabel = String.Join(",", item.Select(x => x.DPM_Label)), TenantID = item.Select(x => x.TenantID).FirstOrDefault(), TenantName = item.Select(x => x.TenantName).FirstOrDefault(), AgencyHierarchyID = item.Select(x => x.AgencyHierarchyID).FirstOrDefault(), IsAdminShare = item.Select(x => x.IsAdminShare).FirstOrDefault(), IsStudentShare = item.Select(x => x.IsStudentShare).FirstOrDefault() });
                }

                View.lstSchoolNodeAssociation = lstSchoolNodeAssociationContract;
            }
        }

        public void GetTenants(Boolean isUpdate)
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
            serviceRequest.Parameter1 = SortByName;
            serviceRequest.Parameter2 = clientCode;
            var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);

            if (!_serviceResponse.Result.IsNullOrEmpty())
            {
                var result = _serviceResponse.Result;
                if (isUpdate)
                {
                    View.lstTenant = result;
                }
                else
                    View.lstTenant = result.Where(cond => !View.lstSchoolNodeAssociation.Select(sel => sel.TenantID).Contains(cond.TenantID)).ToList();
            }
            else
            {
                View.lstTenant = new List<TenantDetailContract>();
            }
        }

        public bool SaveUpdateSchoolNodeAssociation()
        {
            ServiceRequest<Int32, SchoolNodeAssociationContract> serviceRequest = new ServiceRequest<Int32, SchoolNodeAssociationContract>();

            serviceRequest.Parameter1 = View.SelectedTenantID;
            serviceRequest.Parameter2 = View.SchoolNodeAssociationContract;

            var _response = _agencyHierarchyProxy.SaveUpdateSchoolNodeAssociation(serviceRequest);

            if (!_response.IsNullOrEmpty())
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyID.ToString(), AppConsts.CHANGE_TYPE_SCHOOL_HIERARCHY, View.CurrentUserId);
                return _response.Result;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveSchoolNodeAssociation()
        {
            ServiceRequest<Int32, SchoolNodeAssociationContract> serviceRequest = new ServiceRequest<Int32, SchoolNodeAssociationContract>();

            serviceRequest.Parameter1 = View.SelectedTenantID;
            serviceRequest.Parameter2 = View.SchoolNodeAssociationContract;

            var _response = _agencyHierarchyProxy.RemoveSchoolNodeAssociation(serviceRequest);

            if (!_response.IsNullOrEmpty())
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyID.ToString(), AppConsts.CHANGE_TYPE_SCHOOL_HIERARCHY, View.CurrentUserId);
                return _response.Result;
            }
            else
            {
                return false;
            }
        }

        public bool IsSchoolNodeAssociationExists()
        {
            ServiceRequest<Int32, Int32, Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32, Int32, Int32>();

            serviceRequest.Parameter1 = View.SelectedTenantID;
            serviceRequest.Parameter2 = View.SchoolNodeAssociationContract.AgencyHierarchyInstitutionNodeID;
            serviceRequest.Parameter3 = View.SchoolNodeAssociationContract.AgencyHierarchyID;
            serviceRequest.Parameter4 = View.SchoolNodeAssociationContract.DPM_ID;

            var _response = _agencyHierarchyProxy.IsSchoolNodeAssociationExists(serviceRequest);

            if (!_response.IsNullOrEmpty())
            {
                return _response.Result;
            }
            else
            {
                return false;
            }
        }



        #endregion
    }
}
