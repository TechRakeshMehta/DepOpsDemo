using System;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using System.Linq;
using INTSOF.Utils;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace CoreWeb.ProfileSharing.Views
{
    public class AttestationReportsWithoutSignaturePresenter : Presenter<IAttestationReportsWithoutSignature>
    {

        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        public void GetAttestationReportWithoutSignature()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.CurrentLoggedInUserID;
            var _serviceResponse = _clinicalRotationProxy.GetAttestationReportsWithoutSignature(serviceRequest);
            View.LstAttestationDocumentsContract = _serviceResponse.Result.ToList();
        }



        public void GetAttestationDocumentsToExport(Int32 invitationGroupId, Boolean IsPDF)
        {
            ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>> serviceRequest = new ServiceRequest<Dictionary<String, Int32>, List<Tuple<Int32, Int32, Int32>>>();
            serviceRequest.Parameter1 = new Dictionary<String, Int32> { { AppConsts.PROFILE_SHARING_INVITATION_GROUP_ID, invitationGroupId }, { AppConsts.ONLY_UNSIGNED_EXCEL, IsPDF ? 0 : 1 } };
            serviceRequest.Parameter2 = new List<Tuple<Int32, Int32, Int32>>();
            View.LstInvitationDocumentContract = _clinicalRotationProxy.GetAttestationDocumentsToExport(serviceRequest).Result;
            //String ConsolidatedWithoutSign = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED_WITHOUGHT_SIGN.GetStringValue();  
        }

        public void GetSharingInfoByInvitationGrpID(string tenantID, string invitationGroupId)
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = tenantID.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(tenantID);
            serviceRequest.Parameter2 = invitationGroupId.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(invitationGroupId);
            View.StatusMessage = _clinicalRotationProxy.GetSharingInfoByInvitationGrpID(serviceRequest).Result;
        }
    }
}
