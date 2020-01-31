#region System Specific
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#region Application Specific
using DAL.Interfaces;
using INTSOF.Utils;
using Entity.ClientEntity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Data;
#endregion




namespace DAL.Repository
{
    public class ClientSecurityRepository : ClientBaseRepository, IClientSecurityRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;
        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public ClientSecurityRepository(Int32 tenantId, String connectinString = "")
            : base(tenantId, connectinString)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        #region Department
        /// <summary>
        /// Retrieve a list of institution Type tenant.
        /// </summary>
        /// <returns>list of institution Type tenant</returns>
        public List<Tenant> getClientTenant(Int32 defaultTenantId)
        {
            String TenantTypeCodeForInstitution = TenantType.Institution.GetStringValue();
            Int32 tenantTypeIdForInstitution = _ClientDBContext.lkpTenantTypes.FirstOrDefault(condition => condition.TenantTypeCode == TenantTypeCodeForInstitution && condition.IsActive).TenantTypeID;
            return _ClientDBContext.Tenants.Where(condition => condition.TenantTypeID == tenantTypeIdForInstitution && condition.TenantID != defaultTenantId && !condition.IsDeleted).ToList();
        }

        /// <summary>
        /// Save Department in Client DB
        /// </summary>
        /// <param name="departmentObject"></param>
        /// <returns></returns>
        public Boolean SaveClientDepartment(Organization departmentObject)
        {
            _ClientDBContext.Organizations.AddObject(departmentObject);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Get Client Organization Data.
        /// </summary>
        /// <param name="departmentObject"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public Organization GetClientOrganizationById(Int32 organizationId)
        {
            return _ClientDBContext.Organizations.Where(cond => cond.OrganizationID == organizationId && !cond.IsDeleted).FirstOrDefault();
        }

        /// <summary>
        /// Update organization object
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public Boolean UpdateOrganizationObject()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }


        #endregion

        #region Program

        public Boolean DeleteProgram()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        public DeptProgramMapping GetDepProgramMapping(Int32 depProgramMappingId)
        {
            return _ClientDBContext.DeptProgramMappings.Where(cond => cond.DPM_ID == depProgramMappingId && !cond.DPM_IsDeleted).FirstOrDefault();
        }

        public IQueryable<DeptProgramPaymentOption> GetMappedDepProgramPaymentOption(Int32 depProgramMappingId)
        {
            return _ClientDBContext.DeptProgramPaymentOptions.Where(cond => cond.DPPO_DeptProgramMappingID == depProgramMappingId && !cond.DPPO_IsDeleted);
        }

        #endregion

        #region OrganizationUserDb Updates
        public OrganizationUser AddOrganizationUser(OrganizationUser organizationUser)
        {
            try
            {
                _ClientDBContext.AddToOrganizationUsers(organizationUser);
                _ClientDBContext.SaveChanges();
                return organizationUser;

            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        public OrganizationUserProfile AddOrganizationUserProfile(OrganizationUserProfile orgUserProfile)
        {
            try
            {
                _ClientDBContext.AddToOrganizationUserProfiles(orgUserProfile);
                _ClientDBContext.SaveChanges();
                return orgUserProfile;

            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }


        }

        public Boolean UpdateOrganizationData(OrganizationUser organizationUser)
        {
            try
            {
                _ClientDBContext.SaveChanges();
                return true;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        public OrganizationUser GetOrganisationUser(Entity.OrganizationUser orgUser)
        {
            try
            {
                return _ClientDBContext.OrganizationUsers.FirstOrDefault(x => x.OrganizationUserID == orgUser.OrganizationUserID);
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Manage Grade
        #endregion

        #region Edit Profile

        public void AddAddress(Dictionary<String, Object> dicAddressData, Guid addressHandleId, Int32 currentUserId, Entity.Address addressNew, Entity.AddressExt addressExtNew)
        {
            Address address = new Address();
            address.Address1 = Convert.ToString(dicAddressData.GetValue("address1"));
            address.Address2 = Convert.ToString(dicAddressData.GetValue("address2"));
            address.ZipCodeID = Convert.ToInt32(dicAddressData.GetValue("zipcodeid"));
            address.AddressHandleID = addressHandleId;
            address.CreatedOn = DateTime.Now;
            address.CreatedByID = currentUserId;
            address.AddressID = addressNew.AddressID;
            if (address.ZipCodeID == 0 && addressExtNew.IsNotNull())
            {
                AddressExt addressExt = new AddressExt();
                addressExt.AE_ID = addressExtNew.AE_ID;
                addressExtNew.AE_AddressID = addressExtNew.AE_AddressID;
                addressExt.AE_CountryID = addressExtNew.AE_CountryID;
                addressExt.AE_StateName = addressExtNew.AE_StateName;
                addressExt.AE_CityName = addressExtNew.AE_CityName;
                addressExt.AE_ZipCode = addressExtNew.AE_ZipCode;
                addressExt.AE_County = addressExtNew.AE_County;//UAT-3910
                address.AddressExts.Add(addressExt);
            }
            _ClientDBContext.Addresses.AddObject(address);
        }

        public void AddAddressHandle(Guid addressHandleId)
        {
            AddressHandle addressHandleNew = new AddressHandle();
            addressHandleNew.AddressHandleID = addressHandleId;
            _ClientDBContext.AddressHandles.AddObject(addressHandleNew);
        }
        #endregion

        #region Manage Tenant DB Default Entry

        public Boolean UpdateDefaultEntryForNewClient(Int32 tenantId)
        {
            _ClientDBContext.UpdateDefaultEntryForNewClient(tenantId, "");
            return true;
        }

        #endregion

        /// <summary>
        /// Retrieve tenant name.
        /// </summary>
        /// <param name="tenantId">InstitutionID</param>
        /// <returns>TenantName</returns>
        public String GetTenantName(Int32 tenantId)
        {
            String tenantName = String.Empty;
            String TenantTypeCodeForInstitution = TenantType.Institution.GetStringValue();
            Int32 tenantTypeIdForInstitution = _ClientDBContext.lkpTenantTypes.FirstOrDefault(condition => condition.TenantTypeCode == TenantTypeCodeForInstitution && condition.IsActive).TenantTypeID;
            Tenant tenant = _ClientDBContext.Tenants.Where(condition => condition.TenantTypeID == tenantTypeIdForInstitution && condition.TenantID == tenantId && !condition.IsDeleted).FirstOrDefault();
            if (tenant.IsNotNull())
            {
                tenantName = tenant.TenantName;
            }
            return tenantName;
        }

        /// <summary>
        /// UAT 1834: NYU Migration 2 of 3: Applicant Complete Order Process.
        /// </summary>
        /// <param name="emailAddress"></param>
        public void UpdateBulkOrderUploadForOrgUser(Int32 applicantID, String emailAddress)
        {
            String bulkOrderStatusFailedCode = BulkOrderStatus.OrderFailed.GetStringValue();
            Int32 bulkOrderStatusFailedID = _ClientDBContext.lkpBulkOrderStatus.Where(cond => cond.BOS_Code == bulkOrderStatusFailedCode && !cond.BOS_IsDeleted)
                                                                               .FirstOrDefault().BOS_ID;
            BulkOrderUpload bulkOrderUpload = _ClientDBContext.BulkOrderUploads.Where(cond => cond.BOU_EmailAddress.ToLower() == emailAddress.ToLower()
                                                                                && cond.BOU_BulkOrderStatusID != bulkOrderStatusFailedID
                                                                                && cond.BOU_IsActive && !cond.BOU_IsDeleted).FirstOrDefault();

            //If bulk order is exist for applicant.
            if (bulkOrderUpload.IsNotNull())
            {
                bulkOrderUpload.BOU_ApplicantID = applicantID;
                bulkOrderUpload.BOU_ModifiedByID = applicantID;
                bulkOrderUpload.BOU_ModifiedOn = DateTime.Now;

                //Update table
                _ClientDBContext.SaveChanges();
            }
        }

        public void CopyClientAdminPermissions(Int32 tenantID, Guid CopyFromUserId, Guid CopyToUserId, Int32 CurrentLoggedInUserId, Int32 CopyToOrgUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("dbo.usp_MirrorClientAdminPermission", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@CopyFromUserId", CopyFromUserId);
                _command.Parameters.AddWithValue("@CopyToUserId", CopyToUserId);
                _command.Parameters.AddWithValue("@TenantId", tenantID);
                _command.Parameters.AddWithValue("@CurrentUserId", CurrentLoggedInUserId);
                _command.Parameters.AddWithValue("@CopyToOrgUserId", CopyToOrgUserId);
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    con.Close();
                }
            }

        }
    }
}
