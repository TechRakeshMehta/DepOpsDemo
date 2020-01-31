using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;

namespace CoreWeb.CommonControls.Views
{
    public class RotationDetailsPresenter : Presenter<IRotationDetails>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private ClientContactProxy _clientContactProxy
        {
            get
            {
                return new ClientContactProxy();
            }
        }

        /// <summary>
        /// Get Clinical Rotation data to bind Rotation Detail section
        /// </summary>
        public void GetRotationDetail()
        {
            if (View.TenantId > 0 && View.ClinicalRotationId > 0 && !View.IsRestrictToLoadFresshData)
            {
                ServiceRequest<Int32, Int32?> serviceRequest = new ServiceRequest<Int32, Int32?>();
                serviceRequest.SelectedTenantId = View.TenantId;
                serviceRequest.Parameter1 = View.ClinicalRotationId;
                serviceRequest.Parameter2 = View.IsAgencyUser ? View.AgencyID : (Int32?)null;
                var _serviceResponse = _clinicalRotationProxy.GetClinicalRotationById(serviceRequest);
                View.ClinicalRotationDetails = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// To get custom attribute list
        /// </summary>
        /// <param name="rotationId"></param>
        public void GetCustomAttributeList()
        {
            if (View.TenantId > 0 && View.ClinicalRotationId > 0)
            {
                ServiceRequest<Int32, String, Int32?> serviceRequest = new ServiceRequest<Int32, String, Int32?>();
                serviceRequest.Parameter1 = View.TenantId;
                serviceRequest.Parameter2 = CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue();
                serviceRequest.Parameter3 = View.ClinicalRotationId;
                var _serviceResponse = _clinicalRotationProxy.GetCustomAttributeListMapping(serviceRequest);
                View.CustomAttributeList = _serviceResponse.Result;
            }
        }

        /// <summary>
        /// Returns Tenant Name.
        /// </summary>
        public string TenantName
        {
            get
            {
                Boolean SortByName = true;
                String clientCode = TenantType.Institution.GetStringValue();
                ServiceRequest<Boolean, String> serviceRequest = new ServiceRequest<Boolean, String>();
                serviceRequest.Parameter1 = SortByName;
                serviceRequest.Parameter2 = clientCode;
                var _serviceResponse = _clinicalRotationProxy.GetTenants(serviceRequest);
                return _serviceResponse.Result.Where(cond => cond.TenantID == View.TenantId).Select(col => col.TenantName).FirstOrDefault();
            }
        }


        #region UAT-2042, Phase 4 (24): Preceptor Clinical Rotations
        public void GetClientContactRotationDocumentsByID()
        {
            if (View.TenantId > 0 && View.ClinicalRotationId > 0)
            {
                ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
                serviceRequest.Parameter1 = View.TenantId;
                serviceRequest.Parameter2 = View.ClinicalRotationId;
                var _serviceResponse = _clientContactProxy.GetClientContactRotationDocumentsByID(serviceRequest);
                View.SyllabusDocumentContract = _serviceResponse.Result;
            }
        }
        #endregion

        #region UAT-2666
        public Boolean SaveUpdateClinicalRotation()
        {
            ServiceRequest<ClinicalRotationDetailContract, Int32, Boolean, Int32?> serviceRequest = new ServiceRequest<ClinicalRotationDetailContract, Int32, Boolean, Int32?>();
            //View.ViewContract.ContactsToSendEmail = View.ClientContactList.Where(x => View.SelectedClientContacts.Contains(x.ClientContactID)).ToList();
            //View.ClientContactList.Where(x => View.SelectedClientContacts.Contains(x.ClientContactID)).ToList();
            serviceRequest.Parameter1 = View.ClinicalRotationDetails;
            serviceRequest.Parameter2 = View.TenantId;
            serviceRequest.Parameter3 = View.IsSharedUser;
            serviceRequest.Parameter4 = View.CurrentLoggedInUserId;
            var _serviceResponse = _clinicalRotationProxy.UpdateClinicalRotationByAgency(serviceRequest);
            return _serviceResponse.Result;
        }

        public void GetRotationFieldUpdateByAgency(List<Int32> lstClinicalRotationIds)
        {
            if (lstClinicalRotationIds.Count > AppConsts.NONE)
            {
                ServiceRequest<List<Int32>, Int32> serviceRequest = new ServiceRequest<List<Int32>, Int32>();
                serviceRequest.Parameter1 = lstClinicalRotationIds;
                serviceRequest.Parameter2 = View.TenantId;
                var _serviceResponse = _clinicalRotationProxy.GetRotationFieldUpdateByAgencyDetails(serviceRequest);
                View.RotationFieldUpdaeByAgency = _serviceResponse.Result.FirstOrDefault();
            }
        }
        #endregion

        #region UAT-2712
        public AgencyHierarchyRotationFieldOptionContract GetRotationFieldRequierd()
        {
            AgencyHierarchyRotationFieldOptionContract requiredFieldSettings = new AgencyHierarchyRotationFieldOptionContract();

            requiredFieldSettings = Business.RepoManagers.ClinicalRotationManager.GetAgencyHierarchyRotationFieldOptionSetting(View.TenantId, View.ClinicalRotationDetails.AgencyIDs.ToString());

            return requiredFieldSettings;
        }

        #endregion

        #region UAT-3197,As an Agency User, I should be able to retrieve the syllabus
        public void GetClinicalRotationSyllabusDocumentsByID()
        {
            if (View.TenantId > 0 && View.ClinicalRotationId > 0)
            {
                ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
                serviceRequest.Parameter1 = View.TenantId;
                serviceRequest.Parameter2 = View.ClinicalRotationId;
                var _serviceResponse = _clinicalRotationProxy.GetClinicalRotationSyllabusDocumentsByID(serviceRequest);
                View.ClinicalRotationSyllabusDocumentContract = _serviceResponse.Result.FirstOrDefault();
            }
        }
        #endregion
    }
}
