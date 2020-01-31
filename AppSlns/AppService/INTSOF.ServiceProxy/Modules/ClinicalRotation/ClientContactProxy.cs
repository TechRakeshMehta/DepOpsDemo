using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceInterface.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Core;
using INTSOF.Utils.Enums;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace INTSOF.ServiceProxy.Modules.ClinicalRotation
{
    public class ClientContactProxy : BaseServiceProxy<IClientContact>
    {
        IClientContact _clientContactServiceChannel;

        public ClientContactProxy()
            : base(ServiceUrlEnum.ClientContactSvcUrl.GetStringValue())
        {
            _clientContactServiceChannel = base.ServiceChannel;
        }

        public ServiceResponse<List<TenantDetailContract>> GetTenants(ServiceRequest<Boolean, String> data)
        {
            return _clientContactServiceChannel.GetTenants(data);
        }

        #region MANAGE CLIENT CONTACT

        public ServiceResponse<List<ClientContactContract>> GetClientContactSearchData(ServiceRequest<Int32,Int32> data)
        {
            return _clientContactServiceChannel.GetClientContactSearchData(data);
        }

        public ServiceResponse<List<ClientContactContract>> GetClientContacts(ServiceRequest<Int32> data)
        {
            return _clientContactServiceChannel.GetClientContacts(data);
        }

        public ServiceResponse<List<SharedSystemDocumentContract>> GetClientDocuments(ServiceRequest<Int32> data)
        {
            return _clientContactServiceChannel.GetClientDocuments(data);
        }

        public ServiceResponse<List<ClientContactTypeContract>> GetClientContactTypeList()
        {
            return _clientContactServiceChannel.GetClientContactTypeList();
        }

        public ServiceResponse<List<WeekDayContract>> GetWeekDaysList()
        {
            return _clientContactServiceChannel.GetWeekDaysList();
        }

        public ServiceResponse<List<SharedSystemDocTypeContract>> GetDocumentTypeList()
        {
            return _clientContactServiceChannel.GetDocumentTypeList();
        }

        public ServiceResponse<List<ClientContactAvailibiltyContract>> GetClientContactAvailibilty(ServiceRequest<Int32> data)
        {
            return _clientContactServiceChannel.GetClientContactAvailibilty(data);
        }

        public ServiceResponse<Boolean> SaveClientContact(ServiceRequest<ClientContactContract, List<SharedSystemDocumentContract>, AppSettingContract> data)
        {
            return _clientContactServiceChannel.SaveClientContact(data);
        }

        public ServiceResponse<Boolean> DeleteClientContact(ServiceRequest<Int32, Int32> data)
        {
            return _clientContactServiceChannel.DeleteClientContact(data);
        }

        public ServiceResponse<Boolean> UpdateClientContact(ServiceRequest<ClientContactContract, List<SharedSystemDocumentContract>> data)
        {
            return _clientContactServiceChannel.UpdateClientContact(data);
        }

        public ServiceResponse<Guid?> GetExistingUserID(ServiceRequest<String> data)
        {
            return _clientContactServiceChannel.GetExistingUserID(data);
        }

        public ServiceResponse<Boolean> IsEmailAlreadyExistForTenant(ServiceRequest<String, Int32> data)
        {
            return _clientContactServiceChannel.IsEmailAlreadyExistForTenant(data);
        }

        public ServiceResponse<Boolean> IsClientContactAllowedToDelete(ServiceRequest<Int32, Int32> data)
        {
            return _clientContactServiceChannel.IsClientContactAllowedToDelete(data);
        }

        public ServiceResponse<Boolean> ResendInstructorActivationMail(ServiceRequest<ClientContactContract> data)
        {
            return _clientContactServiceChannel.ResendInstructorActivationMail(data);
        }


        #endregion

        #region CLIENT CONTACT PROFILE
        public ServiceResponse<ClientContactContract> GetClientContactByEmail(ServiceRequest<Int32, String> data)
        {
            return _clientContactServiceChannel.GetClientContactByEmail(data);
        }

        public ServiceResponse<List<Int32>> GetClientContactTenantsIDByEmail(ServiceRequest<String> data)
        {
            return _clientContactServiceChannel.GetClientContactTenantsIDByEmail(data);
        }

        public ServiceResponse<Boolean> SaveUploadedDocument(ServiceRequest<SharedSystemDocumentContract, Int32, Int32> data)
        {
            return _clientContactServiceChannel.SaveUploadedDocument(data);
        }

        public ServiceResponse<Boolean> DeleteUploadedDocument(ServiceRequest<Int32, Int32> data)
        {
            return _clientContactServiceChannel.DeleteUploadedDocument(data);
        }

        public ServiceResponse<OrganizationUserContract> GetUserData()
        {
            return _clientContactServiceChannel.GetUserData();
        }

        public ServiceResponse<Boolean> UpdateClientContactOrganisationUser(ServiceRequest<OrganizationUserContract, Int32> data)
        {
            return _clientContactServiceChannel.UpdateClientContactOrganisationUser(data);
        }

        public ServiceResponse<List<ClientContactSyllabusDocumentContract>> GetClientContactRotationDocuments(ServiceRequest<Int32, Int32> data)
        {
            return _clientContactServiceChannel.GetClientContactRotationDocuments(data);
        }
        #endregion

        #region UAT-2042, Phase 4 (24): Preceptor Clinical Rotations
        public ServiceResponse<ClientContactSyllabusDocumentContract> GetClientContactRotationDocumentsByID(ServiceRequest<Int32, Int32> serviceRequest)
        {
            return _clientContactServiceChannel.GetClientContactRotationDocumentsByID(serviceRequest);
        } 
        #endregion

    }
}
