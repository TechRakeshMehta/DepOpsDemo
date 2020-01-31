using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Interfaces
{
    public interface IClientSecurityRepository
    {
        #region Department
        /// <summary>
        /// Retrieve a list of institution Type tenant.
        /// </summary>
        /// <returns>list of institution Type tenant</returns>
        List<Tenant> getClientTenant(Int32 defaultTenantId);

        /// <summary>
        /// Save Department in Client DB
        /// </summary>
        /// <param name="departmentObject"></param>
        /// <returns></returns>
        Boolean SaveClientDepartment(Organization departmentObject);

        /// <summary>
        /// Get Client Organization Data.
        /// </summary>
        /// <param name="departmentObject"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Organization GetClientOrganizationById(Int32 organizationId);

        /// <summary>
        /// Update organization object
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Boolean UpdateOrganizationObject();

        #endregion

        #region Manage Program

        DeptProgramMapping GetDepProgramMapping(Int32 depProgramMappingId);
        IQueryable<DeptProgramPaymentOption> GetMappedDepProgramPaymentOption(Int32 depProgramMappingId);
        Boolean DeleteProgram();

        #endregion

        #region Manage Grade

        #endregion

        #region OrganizationUser
        OrganizationUser AddOrganizationUser(OrganizationUser organizationUser);

        OrganizationUserProfile AddOrganizationUserProfile(OrganizationUserProfile orgUserProfile);

        OrganizationUser GetOrganisationUser(Entity.OrganizationUser organizationUser);

        Boolean UpdateOrganizationData(OrganizationUser organizationUser);
        #endregion

        #region EditProfile
        void AddAddressHandle(Guid addressHandleId);
        void AddAddress(Dictionary<String, Object> dicAddressData, Guid addressHandleId, Int32 currentUserId, Entity.Address addressNew, Entity.AddressExt addressExtNew);
        #endregion

        #region Manage Tenant DB Default Entry

        Boolean UpdateDefaultEntryForNewClient(Int32 tenantId);

        #endregion

        /// <summary>
        /// Retrieve tenant name.
        /// </summary>
        /// <param name="tenantId">InstitutionID</param>
        /// <returns>TenantName</returns>
        String GetTenantName(Int32 tenantId);

        /// <summary>
        /// UAT 1834: NYU Migration 2 of 3: Applicant Complete Order Process.
        /// </summary>
        /// <param name="emailAddress"></param>
        void UpdateBulkOrderUploadForOrgUser(Int32 applicantID, String emailAddress);
        
        //UAT-2257
        void CopyClientAdminPermissions(Int32 tenantID, Guid CopyFromUserId, Guid CopyToUserId, Int32 CurrentLoggedInUserId, Int32 CopyToOrgUserId);
    }
}
