using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;

namespace DAL.Interfaces
{
    public interface IClientContactRepository
    {
        #region ManageClientContact Screen (Instructor/perceptor)
        /// <summary>
        /// Get list of Client Contacts (Instructor/Preceptors) based on Tenant ID.
        /// </summary>
        /// <param name="TenantID">Tenant ID of logged in Client Admin or Selected Tenant for Admin.</param>
        /// <returns>List of ClientContacts</returns>
        List<ClientContact> GetClientContacts(Int32 TenantID);
        List<ClientContact> GetClientContactSearchData(Int32 TenantID, Int32 IsActive);
        Boolean SaveClientContact(ClientContact clientContact);

        Boolean DeleteClientContact(Int32 clientContactID, Int32 tenantID, Int32 loggedInUserID);

        List<SharedSystemDocument> GetClientDocuments(Int32 clientContactID);

        List<ClientContactAvailibilty> GetClientContactAvailibilty(Int32 clientContactID);

        ClientContact UpdateClientContact(Int32 clientContactID);

        Boolean UpdateClientContact(ClientContact clientContact);

        Boolean IsEmailAlreadyExistForTenant(String EmailID, Int32 TenantID);
        #endregion

        #region Client Contact Profile
        List<ClientContact> GetClientContactByEmail(Int32 tenantID, String email);

        List<Int32> GetClientContactTenantsIDByEmail(String email);

        Boolean SaveUploadedDocument(SharedSystemDocument sharedSystemDocument);

        Boolean DeleteUploadedDocument(Int32 SharedsystemDocumentID, Int32 ClientContactID, Int32 LoggedInUserID);

        SharedSystemDocument GetSharedSystemDocument(Int32 sharedSystemDocID);
        #endregion


        List<Int32> GetClientContactTenantIdList(Guid clientContactTokenID);

        #region UAT-2042, Phase 4 (24): Preceptor Clinical Rotations
        Int32 GetClientContactByUserID(Int32 tenantID, String userID); 
        #endregion

        #region UAT-4160
        Boolean CheckIfClientContactsAlreadyDeleted(String emailId, Int32 tenantId);

        #endregion
    }
}
