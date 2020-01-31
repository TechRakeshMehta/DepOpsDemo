using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace INTSOF.ServiceInterface.Modules.ClinicalRotation
{
    [ServiceContract]
    public interface IClientContact
    {
        #region COMMON
        [OperationContract]
        ServiceResponse<List<TenantDetailContract>> GetTenants(ServiceRequest<Boolean, String> data);
        #endregion

        #region MANAGE CLIENT CONTACT
        [OperationContract]
        ServiceResponse<List<ClientContactContract>> GetClientContacts(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<List<ClientContactContract>> GetClientContactSearchData(ServiceRequest<Int32,Int32> data);

        [OperationContract]
        ServiceResponse<List<SharedSystemDocumentContract>> GetClientDocuments(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<List<ClientContactTypeContract>> GetClientContactTypeList();

        [OperationContract]
        ServiceResponse<List<WeekDayContract>> GetWeekDaysList();

        [OperationContract]
        ServiceResponse<List<SharedSystemDocTypeContract>> GetDocumentTypeList();

        [OperationContract]
        ServiceResponse<List<ClientContactAvailibiltyContract>> GetClientContactAvailibilty(ServiceRequest<Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> SaveClientContact(ServiceRequest<ClientContactContract, List<SharedSystemDocumentContract>, AppSettingContract> data);

        [OperationContract]
        ServiceResponse<Boolean> DeleteClientContact(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> UpdateClientContact(ServiceRequest<ClientContactContract, List<SharedSystemDocumentContract>> data);

        [OperationContract]
        ServiceResponse<Guid?> GetExistingUserID(ServiceRequest<String> data);

        [OperationContract]
        ServiceResponse<Boolean> IsEmailAlreadyExistForTenant(ServiceRequest<String, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> IsClientContactAllowedToDelete(ServiceRequest<Int32, Int32> data);


        [OperationContract]
        ServiceResponse<Boolean> ResendInstructorActivationMail(ServiceRequest<ClientContactContract> data);

        #endregion

        #region CLIENT CONTACT PROFILE

        [OperationContract]
        ServiceResponse<ClientContactContract> GetClientContactByEmail(ServiceRequest<Int32, String> data);

        [OperationContract]
        ServiceResponse<List<Int32>> GetClientContactTenantsIDByEmail(ServiceRequest<String> data);

        [OperationContract]
        ServiceResponse<Boolean> SaveUploadedDocument(ServiceRequest<SharedSystemDocumentContract, Int32, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> DeleteUploadedDocument(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<OrganizationUserContract> GetUserData();

        [OperationContract]
        ServiceResponse<Boolean> UpdateClientContactOrganisationUser(ServiceRequest<OrganizationUserContract, Int32> data);

        [OperationContract]
        ServiceResponse<List<ClientContactSyllabusDocumentContract>> GetClientContactRotationDocuments(ServiceRequest<Int32, Int32> data);
        #endregion

        #region UAT-2042, Phase 4 (24): Preceptor Clinical Rotations
        [OperationContract]
        ServiceResponse<ClientContactSyllabusDocumentContract> GetClientContactRotationDocumentsByID(ServiceRequest<Int32,Int32> serviceRequest);

        #endregion

    }
}
