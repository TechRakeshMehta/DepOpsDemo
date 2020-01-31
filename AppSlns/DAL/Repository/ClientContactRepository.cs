using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entity.SharedDataEntity;
using INTSOF.Utils;
using Entity;


namespace DAL.Repository
{
    public class ClientContactRepository : BaseRepository, IClientContactRepository
    {
        #region Variables
        private ADB_SharedDataEntities _sharedDataDBContext;
        #endregion

        #region Default Constructor to initilize DB Context

        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public ClientContactRepository()
        {
            _sharedDataDBContext = base.SharedDataDBContext;
        }
        #endregion

        #region ManageClientContact Screen (Instructor/Perceptor)

        /// <summary>
        /// Get list of Client Contacts (Instructor/Preceptors) based on Tenant ID and account activated filter.
        /// </summary>
        /// <param name="TenantID">Tenant ID of logged in Client Admin or Selected Tenant for Admin.</param>
        /// <returns>List of ClientContacts</returns>
        List<ClientContact> IClientContactRepository.GetClientContactSearchData(Int32 TenantID, Int32 IsActive)
        {
            //if (IsActive == 1)
            //    return _sharedDataDBContext.ClientContacts.Where(x => x.CC_TenantID == TenantID && !x.CC_IsDeleted && x.CC_UserID.HasValue).ToList(); //User account is activated.
            //else
            //    return _sharedDataDBContext.ClientContacts.Where(x => x.CC_TenantID == TenantID && !x.CC_IsDeleted && !x.CC_UserID.HasValue).ToList(); //User account is not activated.

            //UAT-4160// code commented 

            List<ClientContact> lstClientContact = new List<ClientContact>();

            if (IsActive == 1)
                lstClientContact = _sharedDataDBContext.ClientContacts.Where(x => x.CC_TenantID == TenantID && !x.CC_IsDeleted && x.CC_UserID.HasValue).ToList(); //User account is activated.
            else
                lstClientContact = _sharedDataDBContext.ClientContacts.Where(x => x.CC_TenantID == TenantID && !x.CC_IsDeleted && !x.CC_UserID.HasValue).ToList(); //User account is not activated.
            return GetClientContactInfo(lstClientContact);
        }

        /// <summary>
        /// Get list of Client Contacts (Instructor/Preceptors) based on Tenant ID.
        /// </summary>
        /// <param name="TenantID">Tenant ID of logged in Client Admin or Selected Tenant for Admin.</param>
        /// <returns>List of ClientContacts</returns>
        List<ClientContact> IClientContactRepository.GetClientContacts(Int32 TenantID)
        {
            // return _sharedDataDBContext.ClientContacts.Where(x => x.CC_TenantID == TenantID && !x.CC_IsDeleted).ToList();
            List<ClientContact> lstClientContact = _sharedDataDBContext.ClientContacts.Where(x => x.CC_TenantID == TenantID && !x.CC_IsDeleted).ToList();
            return GetClientContactInfo(lstClientContact);

            //String InstOrgUserType = OrganizationUserType.Instructor.GetStringValue();
            //String PrecOrgUserType = OrganizationUserType.Preceptor.GetStringValue();



            //List<Guid?> lstOrgUserIDs = lstClientContact.Where(cc => cc.CC_UserID != null).Select(sel => sel.CC_UserID).ToList();

            //List<Entity.OrganizationUser> lstOrgUserIds = this._dbSecurityContext.OrganizationUsers.Where(con => con.IsDeleted != true && con.IsSharedUser == true && lstOrgUserIDs.Contains(con.UserID)).ToList();
            //foreach (ClientContact clientContact in lstClientContact)
            //{
            //    Boolean IsReg = false;
            //    Entity.OrganizationUser orgUser = lstOrgUserIds.Where(cc => cc.UserID == clientContact.CC_UserID).FirstOrDefault();// this._dbSecurityContext.OrganizationUsers.Where(con => con.IsDeleted != true && con.IsSharedUser == true && clientContact.CC_UserID == con.UserID).FirstOrDefault();
            //    if (!orgUser.IsNullOrEmpty())
            //    {
            //        IsReg = orgUser.OrganizationUserTypeMappings.Any(con => con.OTM_IsDeleted != true && (con.lkpOrgUserType.OrgUserTypeCode == InstOrgUserType || con.lkpOrgUserType.OrgUserTypeCode == PrecOrgUserType));
            //    }
            //    clientContact.IsRegistered = IsReg;
            //}
            //return lstClientContact;
        }

        //UAT-4160:- method to check if registered or not


        private List<ClientContact> GetClientContactInfo(List<ClientContact> lstClientContact)
        {
            if (!lstClientContact.IsNullOrEmpty())
            {
                String InstOrgUserType = OrganizationUserType.Instructor.GetStringValue();
                String PrecOrgUserType = OrganizationUserType.Preceptor.GetStringValue();
                List<Guid?> lstOrgUserIDs = lstClientContact.Where(cc => cc.CC_UserID != null).Select(sel => sel.CC_UserID).ToList();

                List<Entity.OrganizationUser> lstOrgUserIds = base.Context.OrganizationUsers.Where(con => con.IsDeleted != true && con.IsSharedUser == true && lstOrgUserIDs.Contains(con.UserID)).ToList();
                foreach (ClientContact clientContact in lstClientContact)
                {
                    Boolean IsReg = false;
                    Entity.OrganizationUser orgUser = lstOrgUserIds.Where(cc => cc.UserID == clientContact.CC_UserID).FirstOrDefault();// this._dbSecurityContext.OrganizationUsers.Where(con => con.IsDeleted != true && con.IsSharedUser == true && clientContact.CC_UserID == con.UserID).FirstOrDefault();
                    if (!orgUser.IsNullOrEmpty())
                    {
                        IsReg = orgUser.OrganizationUserTypeMappings.Any(con => con.OTM_IsDeleted != true && (con.lkpOrgUserType.OrgUserTypeCode == InstOrgUserType || con.lkpOrgUserType.OrgUserTypeCode == PrecOrgUserType));
                    }
                    clientContact.IsRegistered = IsReg;
                }
                return lstClientContact;
            }
            return new List<ClientContact>();
        }


        Boolean IClientContactRepository.SaveClientContact(ClientContact clientContact)
        {
            _sharedDataDBContext.AddToClientContacts(clientContact);
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
            //return true;
        }

        Boolean IClientContactRepository.DeleteClientContact(Int32 clientContactID, Int32 tenantID, Int32 loggedInUserID)
        {
            Boolean Result = false;
            ClientContact clientContact = _sharedDataDBContext.ClientContacts.Where(x => x.CC_ID == clientContactID && !x.CC_IsDeleted).FirstOrDefault();

            List<ClientContact> lstclientContacts = new List<ClientContact>();
            if (!clientContact.IsNullOrEmpty())
                lstclientContacts = _sharedDataDBContext.ClientContacts.Where(x => x.CC_Email == clientContact.CC_Email && !x.CC_IsDeleted).ToList();

            //If regitered 
            if (!clientContact.CC_UserID.IsNullOrEmpty() && lstclientContacts.Count == AppConsts.ONE)
            {
                //UAT-4160
                List<Entity.OrganizationUser> lstOrgUser = base.Context.OrganizationUsers.Where(con => con.IsDeleted != true
                                    && con.UserID == clientContact.CC_UserID.Value).ToList();
                Entity.OrganizationUser orgUser = lstOrgUser.Where(con => con.IsSharedUser == true).FirstOrDefault();
                if (!orgUser.IsNullOrEmpty())
                {
                    orgUser.OrganizationUserTypeMappings.Where(con => con.OTM_IsDeleted == false).ToList().ForEach(x =>
                    {
                        if (x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Instructor.GetStringValue() || x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Preceptor.GetStringValue())
                        {
                            x.OTM_IsDeleted = true;
                            x.OTM_ModifiedByID = loggedInUserID;
                            x.OTM_ModifiedOn = DateTime.Now;
                        }
                    });
                    if (!(orgUser.OrganizationUserTypeMappings.Where(con => con.OTM_IsDeleted == false
                        && (con.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.AgencyUser.GetStringValue()
                            || con.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.ApplicantsSharedUser.GetStringValue())).Any()))
                    {
                        orgUser.IsDeleted = true;
                        orgUser.ModifiedByID = loggedInUserID;
                        orgUser.ModifiedOn = DateTime.Now;
                        if (lstOrgUser.Count == AppConsts.ONE)
                        {
                            SecurityRepository secRepo = new SecurityRepository();
                            Result = secRepo.DeleteOrganizationUser(orgUser);
                        }
                    }
                    if (base.Context.SaveChanges() > AppConsts.NONE)
                        Result = true;
                }
            }
            else
            {
                Result = true;
            }
            if (!clientContact.IsNullOrEmpty())
            {
                //Delete Client Contact
                clientContact.CC_IsDeleted = true;
                clientContact.CC_ModifiedByID = loggedInUserID;
                clientContact.CC_ModifiedOn = DateTime.Now;

                foreach (ClientContactAvailibilty clientContactAvailibilty in clientContact.ClientContactAvailibilties)
                {
                    //Delete Client Availibility
                    clientContactAvailibilty.CCA_IsDeleted = true;
                    clientContactAvailibilty.CCA_ModifiedByID = loggedInUserID;
                    clientContactAvailibilty.CCA_ModifiedOn = DateTime.Now;
                }

                foreach (ClientContactDocument clientContactDocument in clientContact.ClientContactDocuments)
                {
                    //Delete ClientContactDocument
                    clientContactDocument.CCD_IsDeleted = true;
                    clientContactDocument.CCD_ModifiedByID = loggedInUserID;
                    clientContactDocument.CCD_ModifiedOn = DateTime.Now;
                    //Delete SharedSystemDocument.
                    clientContactDocument.SharedSystemDocument.SSD_IsDeleted = true;
                    clientContactDocument.SharedSystemDocument.SSD_ModifiedByID = loggedInUserID;
                    clientContactDocument.SharedSystemDocument.SSD_ModifiedOn = DateTime.Now;
                }
            }

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE && Result)
            {
                return true;
            }
            return false;
        }

        List<SharedSystemDocument> IClientContactRepository.GetClientDocuments(Int32 clientContactID)
        {
            return _sharedDataDBContext.ClientContactDocuments.Where(x => x.CCD_ClientContactID == clientContactID && !x.CCD_IsDeleted).Select(x => x.SharedSystemDocument).ToList();
        }

        List<ClientContactAvailibilty> IClientContactRepository.GetClientContactAvailibilty(Int32 clientContactID)
        {
            return _sharedDataDBContext.ClientContactAvailibilties.Where(x => x.CCA_ClientContactID == clientContactID && !x.CCA_IsDeleted).ToList();
        }

        ClientContact IClientContactRepository.UpdateClientContact(Int32 clientContactID)
        {
            return _sharedDataDBContext.ClientContacts.Where(x => x.CC_ID == clientContactID && !x.CC_IsDeleted).FirstOrDefault();
        }

        Boolean IClientContactRepository.UpdateClientContact(ClientContact clientContact)
        {
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        Boolean IClientContactRepository.IsEmailAlreadyExistForTenant(string EmailID, int TenantID)
        {
            return _sharedDataDBContext.ClientContacts.Any(cond => cond.CC_Email == EmailID && cond.CC_TenantID == TenantID && !cond.CC_IsDeleted);
        }

        #endregion

        #region Client Contact Profile
        List<ClientContact> IClientContactRepository.GetClientContactByEmail(int tenantID, string email)
        {
            //There should be only one record for each email. Implement Check (to do.)
            return _sharedDataDBContext.ClientContacts.Where(cond => cond.CC_Email == email && cond.CC_TenantID == tenantID && !cond.CC_IsDeleted).ToList();
        }

        List<Int32> IClientContactRepository.GetClientContactTenantsIDByEmail(String email)
        {
            Guid userid = new Guid();
            userid = base.Context.aspnet_Membership.Where(cond => cond.LoweredEmail == email.ToLower()).Select(cond => cond.UserId).FirstOrDefault(); // UAT-4333
            return _sharedDataDBContext.ClientContacts.Where(cond => cond.CC_UserID == userid && !cond.CC_IsDeleted).Select(sel => sel.CC_TenantID).Distinct().ToList();
        //    return _sharedDataDBContext.ClientContacts.Where(cond => cond.CC_Email == email && !cond.CC_IsDeleted).Select(sel => sel.CC_TenantID).Distinct().ToList(); UAT-4333
        }

        Boolean IClientContactRepository.SaveUploadedDocument(SharedSystemDocument sharedSystemDocument)
        {
            _sharedDataDBContext.AddToSharedSystemDocuments(sharedSystemDocument);
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        Boolean IClientContactRepository.DeleteUploadedDocument(Int32 SharedsystemDocumentID, Int32 ClientContactID, Int32 LoggedInUserID)
        {
            SharedSystemDocument sharedSystemDocument = _sharedDataDBContext.SharedSystemDocuments.Where(cond => cond.SSD_ID == SharedsystemDocumentID && !cond.SSD_IsDeleted).FirstOrDefault();
            if (!sharedSystemDocument.IsNullOrEmpty())
            {
                sharedSystemDocument.SSD_IsDeleted = true;
                sharedSystemDocument.SSD_ModifiedByID = LoggedInUserID;
                sharedSystemDocument.SSD_ModifiedOn = DateTime.Now;

                foreach (ClientContactDocument clientContactDocument in sharedSystemDocument.ClientContactDocuments)
                {
                    clientContactDocument.CCD_IsDeleted = true;
                    clientContactDocument.CCD_ModifiedByID = LoggedInUserID;
                    clientContactDocument.CCD_ModifiedOn = DateTime.Now;
                }
                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }
            return false;
        }

        SharedSystemDocument IClientContactRepository.GetSharedSystemDocument(Int32 sharedSystemDocID)
        {
            return _sharedDataDBContext.SharedSystemDocuments.Where(x => x.SSD_ID == sharedSystemDocID && !x.SSD_IsDeleted).FirstOrDefault();
        }

        #endregion

        #region UAT-1361 Client Contact Profile Synching

        /// <summary>
        /// Method to get Client Contact Tenant IDs from [ClinicalRotationClientContactMapping] 
        /// </summary>
        /// <returns></returns>
        List<Int32> IClientContactRepository.GetClientContactTenantIdList(Guid clientContactTokenID)
        {
            return SharedDataDBContext.ClinicalRotationClientContactMappings.Where(cond => !cond.CRCCM_IsDeleted && cond.ClientContact.CC_TokenID == clientContactTokenID).Select(col => col.CRCCM_TenantID).ToList();
        }
        #endregion


        #region UAT-2042, Phase 4 (24): Preceptor Clinical Rotations
        Int32 IClientContactRepository.GetClientContactByUserID(Int32 tenantID, String userID)
        {
            Guid orgUserID = Guid.Parse(userID);
            ClientContact clientContact = _sharedDataDBContext.ClientContacts.Where(cond => cond.CC_UserID == orgUserID && cond.CC_TenantID == tenantID && !cond.CC_IsDeleted).FirstOrDefault();
            if (clientContact.IsNotNull())
            {
                return clientContact.CC_ID;
            }
            return AppConsts.NONE;
        }
        #endregion

        #region UAT-4160
        Boolean IClientContactRepository.CheckIfClientContactsAlreadyDeleted(String emailId, Int32 tenantId)
        {
            List<ClientContact> lstClientContact = _sharedDataDBContext.ClientContacts.Where(con => con.CC_Email == emailId && !con.CC_IsDeleted && con.CC_TenantID != tenantId).ToList();
            if (!lstClientContact.IsNullOrEmpty() && lstClientContact.Count > AppConsts.NONE)
                return false;
            return true;
        }
        #endregion
    }
}
