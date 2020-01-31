using System;
using System.Collections.Generic;
using Business.RepoManagers;
using INTSOF.Service.Core;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceInterface.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.ServiceModel;
using INTSOF.Utils;
using System.Linq;

namespace INTSOF.Service.Modules.ClinicalRotation
{
    public class ClientContact : BaseService, IClientContact
    {

        #region COMMON
        ServiceResponse<List<TenantDetailContract>> IClientContact.GetTenants(ServiceRequest<Boolean, String> data)
        {
            ServiceResponse<List<TenantDetailContract>> commonResponse = new ServiceResponse<List<TenantDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetTenants(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        #endregion

        #region MANAGE CLIENT CONTACTS


        /// <summary>
        /// Get Client Contacts (Instructor/Preceptor) on based on Tenant ID and account activated filter.
        /// UAT-4153 Add "Account Activated" (can be Yes/No) info in Support Portal and Manage Preceptor results grids.
        /// </summary>
        /// <param name="data">SelectedTenantID</param>
        /// <returns></returns>
        ServiceResponse<List<ClientContactContract>> IClientContact.GetClientContactSearchData(ServiceRequest<Int32,Int32> data)
        {
            ServiceResponse<List<ClientContactContract>> commonResponse = new ServiceResponse<List<ClientContactContract>>();
            try
            {
                commonResponse.Result = ClientContactManager.GetClientContactSearchData(data.Parameter1,data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get Client Contacts (Instructor/Preceptor)
        /// </summary>
        /// <param name="data">SelectedTenantID</param>
        /// <returns></returns>
        ServiceResponse<List<ClientContactContract>> IClientContact.GetClientContacts(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<ClientContactContract>> commonResponse = new ServiceResponse<List<ClientContactContract>>();
            try
            {
                commonResponse.Result = ClientContactManager.GetClientContacts(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get Client Uploaded Documents
        /// </summary>
        /// <param name="data">ClientContactID</param>
        /// <returns></returns>
        ServiceResponse<List<SharedSystemDocumentContract>> IClientContact.GetClientDocuments(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<SharedSystemDocumentContract>> commonResponse = new ServiceResponse<List<SharedSystemDocumentContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClientContactManager.GetClientDocuments(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<ClientContactTypeContract>> IClientContact.GetClientContactTypeList()
        {
            ServiceResponse<List<ClientContactTypeContract>> commonResponse = new ServiceResponse<List<ClientContactTypeContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClientContactManager.GetClientContactType();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<WeekDayContract>> IClientContact.GetWeekDaysList()
        {
            ServiceResponse<List<WeekDayContract>> commonResponse = new ServiceResponse<List<WeekDayContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClientContactManager.GetWeekDaysList();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<SharedSystemDocTypeContract>> IClientContact.GetDocumentTypeList()
        {
            ServiceResponse<List<SharedSystemDocTypeContract>> commonResponse = new ServiceResponse<List<SharedSystemDocTypeContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClientContactManager.GetDocumentTypeList();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<ClientContactAvailibiltyContract>> IClientContact.GetClientContactAvailibilty(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<ClientContactAvailibiltyContract>> commonResponse = new ServiceResponse<List<ClientContactAvailibiltyContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClientContactManager.GetClientContactAvailibilty(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClientContact.SaveClientContact(ServiceRequest<ClientContactContract, List<SharedSystemDocumentContract>, AppSettingContract> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClientContactManager.SaveClientContact(data.Parameter1, data.Parameter1.ListClientContactAvailibiltyContract, data.Parameter2, activeUser.OrganizationUserId, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClientContact.DeleteClientContact(ServiceRequest<Int32, Int32> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClientContactManager.DeleteClientContact(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClientContact.UpdateClientContact(ServiceRequest<ClientContactContract, List<SharedSystemDocumentContract>> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClientContactManager.UpdateClientContact(data.Parameter1, data.Parameter1.ListClientContactAvailibiltyContract, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Guid?> IClientContact.GetExistingUserID(ServiceRequest<String> data)
        {
            ServiceResponse<Guid?> commonResponse = new ServiceResponse<Guid?>();
            try
            {
                commonResponse.Result = ClientContactManager.GetOrganizationUsersByEmail(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClientContact.IsEmailAlreadyExistForTenant(ServiceRequest<String, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClientContactManager.IsEmailAlreadyExistForTenant(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }


        ServiceResponse<Boolean> IClientContact.IsClientContactAllowedToDelete(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ClientContactManager.IsClientContactAllowedToDelete(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClientContact.ResendInstructorActivationMail(ServiceRequest<ClientContactContract> data)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = ClientContactManager.ResendInstructorActivationMail(data.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        #endregion

        #region CLIENT CONTACT PROFILE
        ServiceResponse<ClientContactContract> IClientContact.GetClientContactByEmail(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<ClientContactContract> commonResponse = new ServiceResponse<ClientContactContract>();
            try
            {
                commonResponse.Result = ClientContactManager.GetClientContactByEmail(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }


        ServiceResponse<List<Int32>> IClientContact.GetClientContactTenantsIDByEmail(ServiceRequest<String> data)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                commonResponse.Result = ClientContactManager.GetClientContactTenantsIDByEmail(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClientContact.SaveUploadedDocument(ServiceRequest<SharedSystemDocumentContract, Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClientContactManager.SaveUploadedDocument(data.Parameter1, data.Parameter2, data.Parameter3, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClientContact.DeleteUploadedDocument(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClientContactManager.DeleteUploadedDocument(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<OrganizationUserContract> IClientContact.GetUserData()
        {
            ServiceResponse<OrganizationUserContract> commonResponse = new ServiceResponse<OrganizationUserContract>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClientContactManager.GetUserData(activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IClientContact.UpdateClientContactOrganisationUser(ServiceRequest<OrganizationUserContract, int> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ClientContactManager.UpdateClientContactOrganisationUser(data.Parameter1, data.Parameter2, activeUser.UserID);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<ClientContactSyllabusDocumentContract>> IClientContact.GetClientContactRotationDocuments(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<List<ClientContactSyllabusDocumentContract>> commonResponse = new ServiceResponse<List<ClientContactSyllabusDocumentContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetClientContactRotationDocuments(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2042, Phase 4 (24): Preceptor Clinical Rotations
        ServiceResponse<ClientContactSyllabusDocumentContract> IClientContact.GetClientContactRotationDocumentsByID(ServiceRequest<Int32, Int32> serviceRequest)
        {
            ServiceResponse<ClientContactSyllabusDocumentContract> commonResponse = new ServiceResponse<ClientContactSyllabusDocumentContract>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                Int32 clientContactID = ClientContactManager.GetClientContactByUserID(serviceRequest.Parameter1, activeUser.UserID);
                commonResponse.Result = ClinicalRotationManager.GetClientContactRotationDocumentsByID(serviceRequest.Parameter1, clientContactID, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClientContactSvcError(ex);
                throw;
            }
        }
        
        #endregion
    }
}
