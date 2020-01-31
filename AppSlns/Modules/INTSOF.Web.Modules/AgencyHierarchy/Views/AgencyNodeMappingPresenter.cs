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
    public class AgencyNodeMappingPresenter : Presenter<IAgencyNodeMappingView>
    {

        private AgencyHierarchyProxy _agencyHierarchyProxy
        {
            get
            {
                return new AgencyHierarchyProxy();
            }
        }

        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        public Boolean SaveAgencyHierarchyAgencyMapping()
        {
            ServiceRequest<AgencyNodeMappingContract> serviceRequest = new ServiceRequest<AgencyNodeMappingContract>();
            AgencyNodeMappingContract agencyNodeMappingContract = View.AgencyNodeMappingContract;
            agencyNodeMappingContract.CurrentLoggedInUserID = View.CurrentLoggedInUserID;
            serviceRequest.Parameter = agencyNodeMappingContract;
            var _response = _agencyHierarchyProxy.SaveAgencyHierarchyAgencyMapping(serviceRequest);

            //UAT- 2631 Digestion Process      
            if (_response.Result)
            {
                AgencyHierarchyManager.CallDigestionProcess(agencyNodeMappingContract.AgencyHierarchyId.ToString(), AppConsts.CHANGE_TYPE_ALL, View.CurrentLoggedInUserID);
            }
            return _response.Result;
        }

        public Boolean DeleteAgencyHierarchyAgencyMapping()
        {
            ServiceRequest<AgencyNodeMappingContract> serviceRequest = new ServiceRequest<AgencyNodeMappingContract>();
            AgencyNodeMappingContract agencyNodeMappingContract = View.AgencyNodeMappingContract;
            agencyNodeMappingContract.CurrentLoggedInUserID = View.CurrentLoggedInUserID;
            serviceRequest.Parameter = agencyNodeMappingContract;
            var _response = _agencyHierarchyProxy.DeleteAgencyHierarchyAgencyMapping(serviceRequest); 
            //UAT- 2631 Digestion Process      
            if (_response.Result)
            {
                AgencyHierarchyManager.CallDigestionProcess(agencyNodeMappingContract.AgencyHierarchyId.ToString(), AppConsts.CHANGE_TYPE_ALL, View.CurrentLoggedInUserID);
            }
            return _response.Result;
        }

        public void GetAllAgency(AgencyNodeMappingContract agencyNodeMappingContract)
        {
            List<AgencyNodeMappingContract> LstAgencies = new List<AgencyNodeMappingContract>();
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.AgencyHierarchyID;
            LstAgencies = _agencyHierarchyProxy.GetAgencies(serviceRequest).Result;
            if (!agencyNodeMappingContract.IsNullOrEmpty() && agencyNodeMappingContract.AgencyID > 0)
            {
                View.lstAgencyHierarchyAgencies.Remove(View.lstAgencyHierarchyAgencies.Where(cond => cond.AgencyID == agencyNodeMappingContract.AgencyID).FirstOrDefault());
            }
            View.lstAgencies = LstAgencies.Where(cond => !View.lstAgencyHierarchyAgencies.Select(sel => sel.AgencyID).Contains(cond.AgencyID)).ToList();
        }

        public void GetAgencyHirarchyAgencies()
        {
            List<AgencyNodeMappingContract> lstAgencyHierarchyAgencies = new List<AgencyNodeMappingContract>();
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.AgencyHierarchyID;
            lstAgencyHierarchyAgencies = _agencyHierarchyProxy.GetAgencyHierarchyAgencies(serviceRequest).Result;
            View.lstAgencyHierarchyAgencies = lstAgencyHierarchyAgencies;
        }

        public void CheckForLeafNode()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.AgencyHierarchyID;
            var _response = _agencyHierarchyProxy.IsAgencyHierarchyLeafNode(serviceRequest);
            View.IsAgencyHierarchyLeafNode = _response.Result;
        }

        public bool SaveAttestationForm()
        {
            if (View.ToSaveUploadedDocuments.IsNotNull() && View.ToSaveUploadedDocuments.Count > AppConsts.NONE)
            {
                ServiceRequest<Int32, List<RequirementApprovalNotificationDocumentContract>> serviceRequest = new ServiceRequest<Int32, List<RequirementApprovalNotificationDocumentContract>>();
                serviceRequest.Parameter1 = View.AgencyHierarchyID;
                serviceRequest.Parameter2 = View.ToSaveUploadedDocuments;
                var _response = _agencyHierarchyProxy.SaveClientSystemDocument(serviceRequest);
            }

            ServiceRequest<AgencyHierarchySettingContract> serviceRequest1 = new ServiceRequest<AgencyHierarchySettingContract>();
            serviceRequest1.Parameter = View.AgencyHierarchySettingContract;
            Boolean result = _agencyHierarchyProxy.SaveAgencyHierarchySetting(serviceRequest1).Result;

            if (result)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyID.ToString(), AppConsts.ATTESTATION_FORM, View.CurrentLoggedInUserID);
            }

            return result;
        }

        public bool DeleteAttestationFormDocuments(Int32 agencyID)
        {
            ServiceRequest<Int32, Int32?, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32?, Int32, String>();
            serviceRequest.Parameter1 = View.AgencyHierarchyID; 
            serviceRequest.Parameter2 = agencyID;
            serviceRequest.Parameter3 = View.CurrentLoggedInUserID;
            serviceRequest.Parameter4 =DocumentType.ATTESTATION_FORM.GetStringValue();
            var _response = _agencyHierarchyProxy.DeleteClientSystemDocumentBasedOnDocType(serviceRequest);

            if (_response.Result)
            {
                AgencyHierarchyManager.CallDigestionProcess(View.AgencyHierarchyID.ToString(), AppConsts.ATTESTATION_FORM, View.CurrentLoggedInUserID);
            }

            return _response.Result;
        }

        public bool DeleteAgencyHierarchySetting(Int32 agencyID)
        {
            ServiceRequest<Int32, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32, String>();
            serviceRequest.Parameter1 = agencyID;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserID;
            serviceRequest.Parameter3 = AgencyHierarchySettingType.SPECIFIC_ATTESTATION_FORM.GetStringValue();
            var _response = _agencyHierarchyProxy.DeleteAgencyHierarchySetting(serviceRequest);
            return _response.Result;
        }
    }
}
