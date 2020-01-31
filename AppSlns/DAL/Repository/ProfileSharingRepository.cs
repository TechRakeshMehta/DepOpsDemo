using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.Data;
using System.Xml.Linq;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Data.Entity.Core.Objects.DataClasses;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System.Globalization;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Xml;
using INTSOF.ServiceDataContracts.Modules;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.UI.Contract.ClinicalRotation;

namespace DAL.Repository
{
    public class ProfileSharingRepository : BaseRepository, IProfileSharingRepository
    {
        #region Variables
        private ADB_SharedDataEntities _sharedDataDBContext;
        #endregion

        #region Default Constructor to initilize DB Context
        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public ProfileSharingRepository()
        {
            _sharedDataDBContext = base.SharedDataDBContext;
        }
        #endregion

        #region Manage Agency

        /// <summary>
        /// Get the all Agency Data for admins and Mapped agency data for Client Admins. 
        /// </summary>
        /// <returns></returns>
        List<ManageAgencyContract> IProfileSharingRepository.GetAgencyDetail(Int32 TenantID, String agencyIDs)
        {
            //return _sharedDataDBContext.GetAgencyDetails(TenantID).ToList();

            List<ManageAgencyContract> lstManageAgency = new List<ManageAgencyContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;


            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAgencyDetails", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantID", TenantID);
                command.Parameters.AddWithValue("@AgencyIDs", agencyIDs);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ManageAgencyContract requirementSharesDataContract = new ManageAgencyContract();
                        requirementSharesDataContract.Name = Convert.ToString(dr["NAME"]);
                        requirementSharesDataContract.AgencyID = Convert.ToInt32(dr["AgencyID"]);
                        requirementSharesDataContract.LABEL = Convert.ToString(dr["LABEL"]);
                        requirementSharesDataContract.NpiNumber = Convert.ToString(dr["NpiNumber"]);
                        requirementSharesDataContract.Description = Convert.ToString(dr["Description"]);
                        requirementSharesDataContract.FullAddress = Convert.ToString(dr["FullAddress"]);
                        requirementSharesDataContract.Address = Convert.ToString(dr["Address"]);
                        requirementSharesDataContract.SharingStatusCode = Convert.ToString(dr["SharingStatusCode"]);
                        //requirementSharesDataContract.AgencyHierarchyLabel = Convert.ToString(dr["AgencyHierarchyLabel"]);
                        requirementSharesDataContract.AgencyHierarchyRootNodeLabel = Convert.ToString(dr["AgencyHierarchyRootNodeLabel"]);
                        requirementSharesDataContract.AttestationReportText = Convert.ToString(dr["AttestationReportText"]);
                        requirementSharesDataContract.ZipCodeID = dr["ZipCodeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ZipCodeID"]);

                        lstManageAgency.Add(requirementSharesDataContract);
                    }
                }
                return lstManageAgency;
            }
        }

        /// <summary>
        /// Get the all Agency Data for admins and Mapped agency data for Client Admins. 
        /// </summary>
        /// <returns></returns>
        List<Agency> IProfileSharingRepository.GetAgencies(Int32 TenantID, Boolean IsAdmin, Boolean isAgencyUser, Guid userID, Boolean getNotVerfiedAgenciesAlso = false)
        {

            if (isAgencyUser)
            {
                List<Int32> loggedInUserAgencyIDs = _sharedDataDBContext.UserAgencyMappings.Where(cond => cond.AgencyUser.AGU_UserID == userID
                                                                && (cond.UAM_IsVerified || getNotVerfiedAgenciesAlso)
                                                                && !cond.UAM_IsDeleted && !cond.Agency.AG_IsDeleted && !cond.AgencyUser.AGU_IsDeleted)
                                                                .Select(cond => cond.UAM_AgencyID).ToList();
                return _sharedDataDBContext.Agencies.Where(cond => loggedInUserAgencyIDs.Contains(cond.AG_ID)).ToList();
            }
            else if (IsAdmin)
            {
                return _sharedDataDBContext.Agencies.Where(cond => !cond.AG_IsDeleted).ToList();
            }
            else
            {
                IEnumerable<Int32> lstAgencyIDs = _sharedDataDBContext.AgencyInstitutions.Where(cond => cond.AGI_TenantID == TenantID && !cond.AGI_IsDeleted).Select(x => x.AGI_AgencyID.Value);
                return _sharedDataDBContext.Agencies.Where(x => lstAgencyIDs.Contains(x.AG_ID) && !x.AG_IsDeleted).ToList();
            }
        }

        /// <summary>
        /// Save the created Agency.
        /// </summary>
        /// <returns></returns>
        Tuple<Int32, Dictionary<Int32, Int32>, Int32> IProfileSharingRepository.SaveAgencies(AgencyContract agencyData, List<Int32> lstTenantID)
        {
            Dictionary<Int32, Int32> AgencyInstitutionIds = new Dictionary<Int32, Int32>();//In this key is tenantID and value is AgencyInstitutionId
            Agency agency = new Agency();
            agency.AG_Name = agencyData.Name.IsNullOrEmpty() ? null : agencyData.Name;
            //UAT-2640:
            agency.AG_Label = agencyData.Label.IsNullOrEmpty() ? null : agencyData.Label;
            agency.AG_Description = agencyData.Description.IsNullOrEmpty() ? null : agencyData.Description;
            agency.AG_NpiNumber = agencyData.NpiNumber.IsNullOrEmpty() ? null : agencyData.NpiNumber;
            agency.AG_IsDeleted = false;
            agency.AG_CreatedByID = agencyData.LoggedInUserID;
            agency.AG_CreatedOn = DateTime.Now;
            agency.AG_CreatedByTenantID = agencyData.CreatedByTenantID;
            agency.AG_SearchStatusID = agencyData.SearchStatusID;
            //UAT 1522
            agency.AG_AttestationReportText = agencyData.AttestationReportText == String.Empty ? null : agencyData.AttestationReportText;

            //Handle Address
            agency.AgencyAddress = new AgencyAddress();
            agency.AgencyAddress.AA_Address1 = agencyData.Address.IsNullOrEmpty() ? null : agencyData.Address;

            if (agencyData.ZipCodeID == AppConsts.NONE)
                agency.AgencyAddress.AA_ZipCodeID = null;
            else
                agency.AgencyAddress.AA_ZipCodeID = agencyData.ZipCodeID;

            agency.AgencyAddress.AA_IsActive = true;
            agency.AgencyAddress.AA_IsDeleted = false;
            agency.AgencyAddress.AA_CreatedByID = agencyData.LoggedInUserID;
            agency.AgencyAddress.AA_CreatedOn = DateTime.Now;

            foreach (Int32 tenantID in lstTenantID)
            {
                AgencyInstitution agencyInstitution = new AgencyInstitution();
                agencyInstitution.AGI_TenantID = tenantID;
                agencyInstitution.AGI_IsDeleted = false;
                agencyInstitution.AGI_CreatedByID = agencyData.LoggedInUserID; ;
                agencyInstitution.AGI_CreatedOn = DateTime.Now;
                agency.AgencyInstitutions.Add(agencyInstitution);
            }

            //Handle permission
            if (!agencyData.lstAgencyPermission.IsNullOrEmpty())
            {
                //for existing records before UAT 1616,
                foreach (AgencyPermission agencyPermission in agencyData.lstAgencyPermission)
                {
                    agency.AgencyPermissions.Add(new AgencyPermission()
                    {
                        AP_PermissionTypeID = agencyPermission.AP_PermissionTypeID,
                        AP_PermissionAccessTypeID = agencyPermission.AP_PermissionAccessTypeID,
                        AP_IsDeleted = false,
                        AP_CreatedByID = agencyData.LoggedInUserID,
                        AP_CreatedOn = DateTime.Now,
                    });
                }
            }

            _sharedDataDBContext.AddToAgencies(agency);
            Int32 agencyHierarchyId = AppConsts.NONE;
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                var result = _sharedDataDBContext.AgencyInstitutions.Where(con => con.AGI_AgencyID == agency.AG_ID && !con.AGI_IsDeleted).Select(sel => new { sel.AGI_ID, sel.AGI_TenantID }).ToList();
                foreach (var item in result)
                {
                    AgencyInstitutionIds.Add(Convert.ToInt32(item.AGI_TenantID), item.AGI_ID);
                }

                //UAT-2639:
                agencyHierarchyId = SaveAgencyNodeAndHierarchyForAgencyCreation(agencyData, agency.AG_ID);
                return new Tuple<Int32, Dictionary<Int32, Int32>, Int32>(agency.AG_ID, AgencyInstitutionIds, agencyHierarchyId);
            }
            else
            {
                return new Tuple<Int32, Dictionary<Int32, Int32>, Int32>(0, AgencyInstitutionIds, agencyHierarchyId);
            }
        }

        /// <summary>
        /// Update the existing Agency.
        /// </summary>
        /// <returns></returns>
        Tuple<String, Dictionary<Int32, Int32>> IProfileSharingRepository.UpdateAgencies(AgencyContract agencyData, List<Int32> tenantIDs_Added, List<Int32> tenantIDs_Removed)
        {
            Dictionary<Int32, Int32> AgencyInstitutionIds = new Dictionary<Int32, Int32>();//In this key is tenantID and value is AgencyInstitutionId
            if (agencyData.IsNotNull())
            {
                Agency agency = _sharedDataDBContext.Agencies.Where(x => x.AG_ID == agencyData.AgencyID && !x.AG_IsDeleted).FirstOrDefault();
                agency.AG_Name = agencyData.Name.IsNullOrEmpty() ? null : agencyData.Name;
                //UAT-2640
                agency.AG_Label = agencyData.Name.IsNullOrEmpty() ? null : agencyData.Label;
                agency.AG_Description = agencyData.Description.IsNullOrEmpty() ? null : agencyData.Description;
                agency.AG_NpiNumber = String.IsNullOrEmpty(agencyData.NpiNumber) ? null : agencyData.NpiNumber;
                agency.AG_CreatedByTenantID = agencyData.CreatedByTenantID;
                agency.AG_SearchStatusID = agencyData.SearchStatusID;
                agency.AG_IsDeleted = false;
                agency.AG_ModifiedByID = agencyData.LoggedInUserID;
                agency.AG_ModifiedOn = DateTime.Now;

                //UAT 1522
                agency.AG_AttestationReportText = agencyData.AttestationReportText == String.Empty ? null : agencyData.AttestationReportText;

                if (agency.AgencyAddress.IsNotNull())
                {
                    //Handle AgencyAddress
                    agency.AgencyAddress.AA_Address1 = agencyData.Address.IsNullOrEmpty() ? null : agencyData.Address;
                    if (agencyData.ZipCodeID == AppConsts.NONE)
                        agency.AgencyAddress.AA_ZipCodeID = null;
                    else
                        agency.AgencyAddress.AA_ZipCodeID = agencyData.ZipCodeID;
                    agency.AgencyAddress.AA_IsActive = true;
                    agency.AgencyAddress.AA_IsDeleted = false;
                    agency.AgencyAddress.AA_ModifiedByID = agencyData.LoggedInUserID;
                    agency.AgencyAddress.AA_ModifiedOn = DateTime.Now;
                }
                else
                {
                    agency.AgencyAddress = new AgencyAddress();
                    agency.AgencyAddress.AA_Address1 = agencyData.Address.IsNullOrEmpty() ? null : agencyData.Address;
                    if (agencyData.ZipCodeID == AppConsts.NONE)
                        agency.AgencyAddress.AA_ZipCodeID = null;
                    else
                        agency.AgencyAddress.AA_ZipCodeID = agencyData.ZipCodeID;
                    agency.AgencyAddress.AA_IsActive = true;
                    agency.AgencyAddress.AA_IsDeleted = false;
                    agency.AgencyAddress.AA_CreatedByID = agencyData.LoggedInUserID;
                    agency.AgencyAddress.AA_CreatedOn = DateTime.Now;
                }


                if (agencyData.IsAdmin)
                {
                    //List<AgencyInstitution> lstAgencyInstitution = _sharedDataDBContext.AgencyInstitutions
                    //                                      .Where(cond => cond.AGI_AgencyID == agencyData.AgencyID && !cond.AGI_IsDeleted).ToList();

                    List<AgencyInstitution> lstAgencyInstitution = agency.AgencyInstitutions.Where(x => !x.AGI_IsDeleted).ToList();
                    List<AgencyInstitution> lstAgencyInstitutionToremoved = lstAgencyInstitution.Where(x => tenantIDs_Removed.Contains(x.AGI_TenantID.Value)).ToList();
                    //Remove the unchecked IDs
                    foreach (var agencyInstitution in lstAgencyInstitutionToremoved)
                    {
                        agencyInstitution.AGI_IsDeleted = true;
                        agencyInstitution.AGI_ModifiedByID = agencyData.LoggedInUserID;
                        agencyInstitution.AGI_ModifiedOn = DateTime.Now;
                    }

                    //For New selected TenantIDs
                    foreach (var tenantID in tenantIDs_Added)
                    {
                        AgencyInstitution agencyInstitution = new AgencyInstitution();
                        //agencyInstitution.AGI_AgencyID = agency.AG_ID;
                        agencyInstitution.AGI_TenantID = tenantID;
                        agencyInstitution.AGI_IsDeleted = false;
                        agencyInstitution.AGI_CreatedByID = agencyData.LoggedInUserID;
                        agencyInstitution.AGI_CreatedOn = DateTime.Now;

                        agency.AgencyInstitutions.Add(agencyInstitution);
                    }

                }

                //UAT-2071
                IEnumerable<AgencyPermission> lstExistingAgencyPermissions = agency.AgencyPermissions.Where(cond => !cond.AP_IsDeleted);
                if (!lstExistingAgencyPermissions.IsNullOrEmpty())
                {
                    foreach (AgencyPermission existingAgencyPermission in lstExistingAgencyPermissions)
                    {
                        AgencyPermission agencyPermission = agencyData.lstAgencyPermission.Where(cond => cond.AP_PermissionTypeID == existingAgencyPermission.AP_PermissionTypeID).FirstOrDefault();
                        if (!agencyPermission.IsNullOrEmpty())  //.Contains(existingAgencyUserPermission.AUP_PermissionTypeID))
                        {
                            existingAgencyPermission.AP_PermissionAccessTypeID = agencyPermission.AP_PermissionAccessTypeID;
                            existingAgencyPermission.AP_PermissionTypeID = agencyPermission.AP_PermissionTypeID;
                            existingAgencyPermission.AP_IsDeleted = false;
                            existingAgencyPermission.AP_ModifiedByID = agencyData.LoggedInUserID;
                            existingAgencyPermission.AP_ModifiedOn = DateTime.Now;
                        }
                        //else
                        //{
                        //    agency.AgencyPermissions.Add(new AgencyPermission()
                        //    {
                        //        AP_PermissionTypeID = agencyPermission.AP_PermissionTypeID,
                        //        AP_PermissionAccessTypeID = agencyPermission.AP_PermissionAccessTypeID,
                        //        AP_IsDeleted = false,
                        //        AP_CreatedByID = agencyData.LoggedInUserID,
                        //        AP_CreatedOn = DateTime.Now,
                        //    });
                        //}
                    }

                    //If new agency permission added on agency with existing permissions.
                    List<Int32> lstExistingPermissonTypeIDs = lstExistingAgencyPermissions.Select(c => c.AP_PermissionTypeID).ToList();
                    IEnumerable<AgencyPermission> lstAgencyPermission = agencyData.lstAgencyPermission.Where(x => !lstExistingPermissonTypeIDs.Contains(x.AP_PermissionTypeID));
                    foreach (AgencyPermission agencyPermission in lstAgencyPermission)
                    {
                        agency.AgencyPermissions.Add(new AgencyPermission()
                        {
                            AP_PermissionTypeID = agencyPermission.AP_PermissionTypeID,
                            AP_PermissionAccessTypeID = agencyPermission.AP_PermissionAccessTypeID,
                            AP_IsDeleted = false,
                            AP_CreatedByID = agencyData.LoggedInUserID,
                            AP_CreatedOn = DateTime.Now,
                        });
                    }
                }
                else if (!agencyData.lstAgencyPermission.IsNullOrEmpty())
                {
                    //for existing records before UAT 2071,
                    foreach (AgencyPermission agencyPermission in agencyData.lstAgencyPermission)
                    {
                        agency.AgencyPermissions.Add(new AgencyPermission()
                        {
                            AP_PermissionTypeID = agencyPermission.AP_PermissionTypeID,
                            AP_PermissionAccessTypeID = agencyPermission.AP_PermissionAccessTypeID,
                            AP_IsDeleted = false,
                            AP_CreatedByID = agencyData.LoggedInUserID,
                            AP_CreatedOn = DateTime.Now,
                        });
                    }
                }

                //UAT-2640
                if (!agencyData.IsAdmin && !agencyData.AgencyProfileSharePermission.IsNullOrEmpty())
                {
                    var agencyHierarchyProfilePermissionToUpdate = base.SharedDataDBContext.AgencyHierarchyAgencyProfileSharePermissions.FirstOrDefault(cnd =>
                                                                                      cnd.AHAPSP_ID == agencyData.AgencyProfileSharePermission.AgencyHierarchyProfileSharePermissionID
                                                                                      && !cnd.AHAPSP_IsDeleted);
                    if (!agencyHierarchyProfilePermissionToUpdate.IsNullOrEmpty())
                    {

                        agencyHierarchyProfilePermissionToUpdate.AHAPSP_IsAdminShare = agencyData.AgencyProfileSharePermission.IsAdmin;
                        agencyHierarchyProfilePermissionToUpdate.AHAPSP_IsStudentShare = agencyData.AgencyProfileSharePermission.IsStudent;
                        agencyHierarchyProfilePermissionToUpdate.AHAPSP_ModifiedByID = agencyData.LoggedInUserID;
                        agencyHierarchyProfilePermissionToUpdate.AHAPSP_ModifiedOn = DateTime.Now;

                        var agencyHierarchyTenantMapping = base.SharedDataDBContext.AgencyHierarchyTenantMappings.FirstOrDefault(cnd => cnd.AHTM_AgencyHierarchyID == agencyHierarchyProfilePermissionToUpdate.AHAPSP_AgencyHierarchyID
                                                                                              && cnd.AHTM_TenantID == agencyHierarchyProfilePermissionToUpdate.AHAPSP_TenantID && !cnd.AHTM_IsDeleted);
                        if (!agencyHierarchyTenantMapping.IsNullOrEmpty())
                        {
                            agencyHierarchyTenantMapping.AHTM_IsAdminShare = agencyData.AgencyProfileSharePermission.IsAdmin;
                            agencyHierarchyTenantMapping.AHTM_IsStudentShare = agencyData.AgencyProfileSharePermission.IsStudent;
                            agencyHierarchyTenantMapping.AHTM_ModifiedBy = agencyData.LoggedInUserID;
                            agencyHierarchyTenantMapping.AHTM_ModifiedOn = DateTime.Now;
                        }
                    }

                }
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                var result = _sharedDataDBContext.AgencyInstitutions.Where(con => con.AGI_AgencyID == agencyData.AgencyID && !con.AGI_IsDeleted).Select(sel => new { sel.AGI_ID, sel.AGI_TenantID }).ToList();
                foreach (var item in result)
                {
                    AgencyInstitutionIds.Add(Convert.ToInt32(item.AGI_TenantID), item.AGI_ID);
                }
                return new Tuple<String, Dictionary<Int32, Int32>>(AppConsts.AG_UPDATED_SUCCESS_MSG, AgencyInstitutionIds);
            }
            return new Tuple<String, Dictionary<Int32, Int32>>(AppConsts.AG_UPDATED_ERROR_MSG, AgencyInstitutionIds);
        }

        /// <summary>
        /// Return List of AgencyInstitutions corresponding to list of AgencyIDs
        /// </summary>
        /// <param name="lstAgencyId"></param>
        /// <returns></returns>
        List<AgencyInstitution> IProfileSharingRepository.GetAgencyInstitutionForAgencies(IEnumerable<int> lstAgencyId)
        {
            return _sharedDataDBContext.AgencyInstitutions.Where(cond => lstAgencyId.Contains(cond.AGI_AgencyID.Value) && !cond.AGI_IsDeleted).ToList();
        }

        /// <summary>
        /// Delete Agency
        /// </summary>
        /// <returns></returns>
        string IProfileSharingRepository.DeleteAgency(AgencyContract agencyData)
        {
            //If agency User is created for that agency don't allow deletion
            // UAT -1641
            if (_sharedDataDBContext.UserAgencyMappings
                .Any(con => con.UAM_AgencyID == agencyData.AgencyID
                && !con.UAM_IsDeleted && !con.AgencyUser.AGU_IsDeleted))
            {
                return AppConsts.AG_DELETED_INFO_MSG;
            }
            if (agencyData.IsAdmin)
            {
                Agency agency = _sharedDataDBContext.Agencies.Where(x => x.AG_ID == agencyData.AgencyID && !x.AG_IsDeleted).FirstOrDefault();
                agency.AG_IsDeleted = true;
                agency.AG_ModifiedByID = agencyData.LoggedInUserID;
                agency.AG_ModifiedOn = DateTime.Now;

                if (agency.AgencyAddress.IsNotNull())
                {
                    //Delete address
                    agency.AgencyAddress.AA_IsActive = false;
                    agency.AgencyAddress.AA_IsDeleted = true;
                    agency.AgencyAddress.AA_ModifiedByID = agencyData.LoggedInUserID;
                    agency.AgencyAddress.AA_ModifiedOn = DateTime.Now;
                }
                List<AgencyInstitution> lstAgencyInstitution = agency.AgencyInstitutions.Where(cond => cond.AGI_AgencyID == agencyData.AgencyID && !cond.AGI_IsDeleted).ToList();

                foreach (var agencyInstitution in lstAgencyInstitution)
                {
                    agencyInstitution.AGI_IsDeleted = true;
                    agencyInstitution.AGI_ModifiedByID = agencyData.LoggedInUserID;
                    agencyInstitution.AGI_ModifiedOn = DateTime.Now;
                }

                //UAT-2071,Configuration Rotation and Tracking packages must be fully compliant to share
                IEnumerable<AgencyPermission> agencyPermissions = agency.AgencyPermissions.Where(x => !x.AP_IsDeleted);
                foreach (AgencyPermission agencyPermission in agencyPermissions)
                {
                    agencyPermission.AP_IsDeleted = true;
                    agencyPermission.AP_ModifiedByID = agencyData.LoggedInUserID;
                    agencyPermission.AP_ModifiedOn = DateTime.Now;
                }

                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                    return AppConsts.AG_DELETED_SUCCESS_MSG;

                return AppConsts.AG_DELETED_ERROR_MSG;
            }
            else
            {
                AgencyInstitution _agencyInstitution = _sharedDataDBContext.AgencyInstitutions
                                                        .Where(cond => cond.AGI_AgencyID == agencyData.AgencyID && cond.AGI_TenantID == agencyData.TenantID && !cond.AGI_IsDeleted).FirstOrDefault();
                _agencyInstitution.AGI_IsDeleted = true;
                _agencyInstitution.AGI_ModifiedByID = agencyData.LoggedInUserID;
                _agencyInstitution.AGI_ModifiedOn = DateTime.Now;
                if (_sharedDataDBContext.SaveChanges() > 0)
                {
                    return AppConsts.AG_DELETED_SUCCESS_MSG;
                }
                else
                {
                    return AppConsts.AG_DELETED_ERROR_MSG;
                }
            }

        }

        /// <summary>
        /// Returns List of AgencyHierarchyIDs Corresponding to AgencyID
        /// </summary>
        /// <returns></returns>
        List<Int32> IProfileSharingRepository.GetAgencyHierarchyIDsByAgencyID(Int32 agencyID)
        {
            return _sharedDataDBContext.AgencyHierarchyAgencies.Where(con => con.AHA_AgencyID == agencyID && !con.AHA_IsDeleted).Select(sel => sel.AHA_AgencyHierarchyID).ToList();
        }

        /// <summary>
        /// Return List of AgencyInstitutions corresponding to AgencyUser
        /// </summary>
        /// <param name="lstAgencyId"></param>
        /// <returns></returns>
        List<AgencyInstitution> IProfileSharingRepository.GetAgencyInstitutionForAgencyuser(Int32 agencyUserID)
        {
            return _sharedDataDBContext.AgencyInstitutions.Join(_sharedDataDBContext.UserAgencyMappings,
                                                              child => child.AGI_AgencyID,
                                                              parent => parent.UAM_AgencyID,
                                                              (child, parent) => new { child = child, parent = parent })
                                                              .Where(cond => !cond.child.AGI_IsDeleted && !cond.parent.UAM_IsDeleted && cond.parent.UAM_AgencyUserID == agencyUserID)
                                                             .Select(sel => sel.child).ToList();
        }

        #endregion

        #region Get ApplicantInvitationMetaData
        List<ApplicantInvitationMetaData> IProfileSharingRepository.GetApplicantInvitationMetaData()
        {
            return _sharedDataDBContext.ApplicantInvitationMetaDatas.Where(cond => !cond.AIMD_IsDeleted).ToList();
        }
        #endregion

        #region Manage Invitations

        ///// <summary>
        ///// To get invitations documents
        ///// </summary>
        ///// <param name="lstProfileSharingInvitationID"></param>
        ///// <returns></returns>
        //DataTable IProfileSharingRepository.GetApplicantInviteDocuments(List<InvitationIDsContract> lstProfileSharingInvitationID)
        //{
        //    EntityConnection connection = _dbContext.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand("usp_GetInvitationDocuments", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@xmldata", CreateXml(lstProfileSharingInvitationID));
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        if (ds.Tables.Count > 0)
        //        {
        //            return ds.Tables[0];
        //        }
        //        return new DataTable();
        //    }
        //}

        private String CreateXml(List<InvitationIDsContract> lstProfileSharingInvitationID)
        {
            XElement xmlElements = new XElement("ProfileSharingInvitationIDs", lstProfileSharingInvitationID
                                    .Select(i => new XElement("ProfileSharingInvitationID", i.ProfileSharingInvitationID)));
            return xmlElements.ToString();
        }

        ///// <summary>
        ///// To get invitations documents
        ///// </summary>
        ///// <param name="lstSnapshotID"></param>
        ///// <returns></returns>
        //public DataTable GetClientInviteDocuments(String clientInvitationIDs)
        //{
        //    EntityConnection connection = _dbContext.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand("usp_GetImmunizationDocumentsFromSnapshot", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@InvitationIDs", clientInvitationIDs);
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        if (ds.Tables.Count > 0)
        //        {
        //            return ds.Tables[0];
        //        }
        //        return new DataTable();
        //    }
        //}

        ///// <summary>
        ///// To get applicant documents by applicant document IDs
        ///// </summary>
        ///// <param name="lstInvitationID"></param>
        ///// <returns></returns>
        //List<ApplicantDocument> IProfileSharingRepository.GetApplicantDocumentByIDs(List<Int32> lstApplicantDocumentID)
        //{
        //    return _dbContext.ApplicantDocuments.Where(x => lstApplicantDocumentID.Contains(x.ApplicantDocumentID)
        //                && x.IsDeleted == false).ToList();
        //}

        ///// <summary>
        ///// Get the List of SharedComplianceSubscription by invitation IDs
        ///// </summary>
        ///// <param name="lstInvitationId"></param>
        ///// <returns></returns>
        //List<SharedComplianceSubscription> IProfileSharingRepository.GetSharedComplianceSubscriptionByInvitationIDs(List<Int32> lstInvitationID)
        //{
        //    return _dbContext.SharedComplianceSubscriptions.Where(scs => lstInvitationID.Contains(scs.SCS_ProfileSharingInvitationID)
        //                && scs.SCS_IsDeleted == false).ToList();
        //}

        ///// <summary>
        ///// Get passport report data
        ///// </summary>
        ///// <param name="tenantID"></param>
        ///// <param name="xmlData"></param>
        ///// <returns></returns>
        //DataTable IProfileSharingRepository.GetPassportReportData(String xmlData)
        //{
        //    EntityConnection connection = _dbContext.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand("usp_GetPassportReportData", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@xmldata", xmlData);
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        if (ds.Tables.Count > 0)
        //        {
        //            return ds.Tables[0];
        //        }
        //        return new DataTable();
        //    }
        //}

        #endregion

        #region Private Methods

        #region  Applicant Invitations

        //private SharedComplianceSubscription GetSharedComplianceSubscriptionInstance(Int32 currentUserId, DateTime currentDateTime,
        //                                                                             Int32 psiId, ComplianceInvitationData complianceData)
        //{
        //    var _sharedCompliance = new SharedComplianceSubscription();
        //    _sharedCompliance.SCS_ProfileSharingInvitationID = psiId;
        //    _sharedCompliance.SCS_PackageSubscriptionID = complianceData.PkgSubId;
        //    _sharedCompliance.SCS_IsCompletePackageShared = complianceData.IsCompletePkgSelected;
        //    _sharedCompliance.SCS_CreatedBydID = currentUserId;
        //    _sharedCompliance.SCS_CreatedOn = currentDateTime;
        //    _sharedCompliance.SCS_IsDeleted = false;
        //    _sharedCompliance.SCS_SharedInfoTypeID = complianceData.ComplianceSharedInfoTypeId;
        //    _sharedCompliance.SCS_SnapshotID = complianceData.SnapShotId.IsNotNull() ? complianceData.SnapShotId : (Int32?)null;

        //    foreach (var catId in complianceData.lstCategoryIds)
        //    {
        //        GenerateSharedComplianceCategoriesInstance(currentUserId, currentDateTime, catId, _sharedCompliance);
        //    }
        //    return _sharedCompliance;
        //}

        //private void GenerateSharedComplianceCategoriesInstance(Int32 currentUserId, DateTime currentDateTime,
        //                                                               Int32 categoryId, SharedComplianceSubscription _sharedCompliance)
        //{
        //    var _sharedCategory = new SharedSubscriptionCategory();
        //    _sharedCategory.SharedComplianceSubscription = _sharedCompliance;
        //    _sharedCategory.SSC_ComplianceCategoryID = categoryId;
        //    _sharedCategory.SSC_CreatedByID = currentUserId;
        //    _sharedCategory.SSC_CreatedOn = currentDateTime;
        //    _sharedCategory.SSC_IsDeleted = false;
        //}

        //private static SharedBkgPackage GenerateSharedBkgPackageInstance(Int32 currentUserId, DateTime currentDateTime,
        //                                                                             Int32 psiId, BkgInvitationData bkgPkg)
        //{
        //    var _sharedBkgPkg = new SharedBkgPackage();
        //    _sharedBkgPkg.SBP_ProfileSharingInvitationID = psiId;
        //    _sharedBkgPkg.SBP_BkgOrderPackageID = bkgPkg.BOPId;
        //    _sharedBkgPkg.SBP_CreatedBydID = currentUserId;
        //    _sharedBkgPkg.SBP_CreatedOn = currentDateTime;
        //    _sharedBkgPkg.SBP_Isdeleted = false;
        //    //_sharedBkgPkg.SBP_SharedInfoTypeID = bkgPkg.BkgSharedInfoTypeId; ----UAT-1213

        //    foreach (var svcGrpId in bkgPkg.lstSvcGrpIds)
        //    {
        //        GenerateSharedBkgPkgSvcGroupInstance(currentUserId, currentDateTime, _sharedBkgPkg, svcGrpId);
        //    }
        //    return _sharedBkgPkg;
        //}

        //UAT-1213 - Method to generate InvitationSharedInfoMapping new instance
        //private static Entity.ClientEntity.InvitationSharedInfoMapping GenerateInvitationSharedInfoMappingInstance(Int32 currentUserId, DateTime currentDateTime,
        //                                                                              Int32 sharedInfoTypeID, Int32 sharedBkgPackageID)
        //{
        //    var _invitationSharedInfoMapping = new Entity.ClientEntity.InvitationSharedInfoMapping();
        //    _invitationSharedInfoMapping.ISIM_InvitationSharedInfoTypeID = sharedInfoTypeID;
        //    _invitationSharedInfoMapping.ISIM_SharedBkgPackageID = sharedBkgPackageID;
        //    _invitationSharedInfoMapping.ISIM_CreatedByID = currentUserId;
        //    _invitationSharedInfoMapping.ISIM_CreatedOn = currentDateTime;
        //    _invitationSharedInfoMapping.ISIM_IsDeleted = false;

        //    return _invitationSharedInfoMapping;
        //}

        //private static void GenerateSharedBkgPkgSvcGroupInstance(Int32 currentUserId, DateTime currentDateTime, SharedBkgPackage _sharedBkgPkg, int svcGrpId)
        //{
        //    var _sharedCategory = new SharedBkgPackageSvcGroup();
        //    _sharedCategory.SharedBkgPackage = _sharedBkgPkg;
        //    _sharedCategory.BPSG_BkgSvcGroupID = svcGrpId;
        //    _sharedCategory.BPSG_CreatedByID = currentUserId;
        //    _sharedCategory.BPSG_CreatedOn = currentDateTime;
        //    _sharedCategory.BPSG_IsDeleted = false;
        //}

        //private static Int32 GetSharedInfoTypeId(List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstSharedInfoTypes, String selectedCode, String masterTypeCode)
        //{
        //    return lstSharedInfoTypes.Where(sit => sit.Code == selectedCode &&
        //           sit.IsDeleted == false && sit.MasterInfoTypeCode == masterTypeCode).First().SharedInfoTypeID;
        //}

        ////UAT-1213 - Method to get BkgSharedInfoTypeIDs by BkgSharedInfoTypeCodes
        //private static List<Int32> GetBkgSharedInfoTypeIds(List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstSharedInfoTypes, List<String> lstSelectedCode, String masterTypeCode)
        //{
        //    return lstSharedInfoTypes.Where(cond => lstSelectedCode.Contains(cond.Code)
        //        && cond.IsDeleted == false && cond.MasterInfoTypeCode == masterTypeCode).Select(x => x.SharedInfoTypeID).ToList();
        //}

        #endregion

        #endregion

        #region Manage AgencyUser
        List<AgencyUser> IProfileSharingRepository.GetAgencyUser(Int32 tenantID, Boolean IsAdmin)
        {
            if (IsAdmin)
            {
                //return _sharedDataDBContext.AgencyUsers.Include("Agency").Where(cond => !cond.AGU_IsDeleted).ToList();
                return _sharedDataDBContext.AgencyUsers.Where(cond => !cond.AGU_IsDeleted).ToList();//UAT-1641
            }
            else
            {
                //UAT - 1641
                IEnumerable<Int32> lstAgencyInstIDs = _sharedDataDBContext.AgencyInstitutions.Where(cond => cond.AGI_TenantID == tenantID && !cond.AGI_IsDeleted).Select(x => x.AGI_ID);
                IEnumerable<Int32> lstAgencyUserId = _sharedDataDBContext.AgencyUserInstitutions
                                    .Where(x => lstAgencyInstIDs.Contains(x.AGUI_AgencyInstitutionID.Value) && !x.AGUI_IsDeleted && !x.UserAgencyMapping.UAM_IsDeleted)
                                    .Select(x => x.UserAgencyMapping.UAM_AgencyUserID);
                return _sharedDataDBContext.AgencyUsers.Where(x => lstAgencyUserId.Contains(x.AGU_ID) && !x.AGU_IsDeleted).ToList();
            }
        }

        /// <summary>
        /// Return the list of agency users belongs to 
        /// logged in user's agency, 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<AgencyUser> IProfileSharingRepository.GetAgencyUserForSharedUser(Guid userID)
        {
            //Int32 loggedInUserAgencyID = _sharedDataDBContext.AgencyUsers.Where(x => x.AGU_UserID == userID && !x.AGU_IsDeleted).Select(col => col.AGU_AgencyID).FirstOrDefault();
            //return _sharedDataDBContext.AgencyUsers.Where(x => x.AGU_AgencyID == loggedInUserAgencyID && !x.AGU_IsDeleted).ToList();

            List<Int32> loggedInUserAgencyIDs = _sharedDataDBContext.UserAgencyMappings.Where(cond => cond.AgencyUser.AGU_UserID == userID && cond.UAM_IsVerified
                                                                        && !cond.UAM_IsDeleted && !cond.Agency.AG_IsDeleted && !cond.AgencyUser.AGU_IsDeleted)
                                                                        .Select(cond => cond.UAM_AgencyID).ToList();
            return _sharedDataDBContext.UserAgencyMappings.Where(cond => loggedInUserAgencyIDs.Contains(cond.UAM_AgencyID)
                                                         && !cond.UAM_IsDeleted && !cond.AgencyUser.AGU_IsDeleted)
                                                         .Select(cond => cond.AgencyUser).Distinct().ToList();
        }

        List<AgencyUserContract> IProfileSharingRepository.GetAgencyUserInfo(Boolean IsAdmin, Boolean IsAgencyUser, Guid UserID, Int32 TenantID, CustomPagingArgsContract grdCustomPaging,Boolean GetNotVerfiedAgenciesAlso)
        {
            List<AgencyUserContract> lstAgencyUser = new List<AgencyUserContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@IsAdmin", IsAdmin),
                             new SqlParameter("@IsAgencyUser", IsAgencyUser),
                             new SqlParameter("@UserID", UserID),
                             new SqlParameter("@GetNotVerfiedAgenciesAlso",GetNotVerfiedAgenciesAlso),
                             new SqlParameter("@TenantId", TenantID),
                             new SqlParameter("@filteringSortingData", grdCustomPaging.XML),
                             //new SqlParameter("@VirtualCount",grdCustomPaging.VirtualPageCount)
            };
                
                base.OpenSQLDataReaderConnection(con);

                DataSet ds = GetDs(con, sqlParameterCollection);

                base.CloseSQLDataReaderConnection(con);
                List<Task> tasks = new List<Task>();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        grdCustomPaging.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[2].Rows[0]["CurrentPageIndex"]));
                        grdCustomPaging.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[2].Rows[0]["VirtualCount"]));
                    }
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            GetAgencyUserList(lstAgencyUser, ds, dr);
                        }));
                    }
                }
                Task.WaitAll(tasks.ToArray());
            }

            return lstAgencyUser.OrderBy(a => a.AGU_Name).ToList();
        }

        private DataSet GetDs(SqlConnection con, SqlParameter[] sqlParameterCollection)
        {
            return base.ExecuteSQLDataSet(con, "usp_GetAgencyUserList", sqlParameterCollection);
        }

        private void GetAgencyUserList(List<AgencyUserContract> lstAgencyUser, DataSet ds, DataRow dr)
        {
            Dictionary<string, List<int>> ListVaues = new Dictionary<string, List<int>>(); ;
            Task task = Task.Run(() =>
            {
                ListVaues = GetListValues(ds.Tables[1], dr["AGU_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_ID"]));
            });
            AgencyUserContract AgencyUserContract = new AgencyUserContract();

            AgencyUserContract.AGU_ID = dr["AGU_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_ID"]);
            AgencyUserContract.AGU_Name = dr["AGU_Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_Name"]);
            AgencyUserContract.AGU_Phone = dr["AGU_Phone"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_Phone"]);
            AgencyUserContract.AGU_Email = dr["AGU_Email"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_Email"]);


            AgencyUserContract.AGU_UserID = dr["AGU_UserID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_UserID"]);
            AgencyUserContract.AGU_ComplianceSharedInfoTypeID = dr["AGU_ComplianceSharedInfoTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_ComplianceSharedInfoTypeID"]);
            AgencyUserContract.AGU_ReqRotationSharedInfoTypeID = dr["AGU_ReqRotationSharedInfoTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_ReqRotationSharedInfoTypeID"]);
            AgencyUserContract.AGU_BkgSharedInfoTypeID = dr["AGU_BkgSharedInfoTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_BkgSharedInfoTypeID"]);
            AgencyUserContract.AGU_ComplianceSharedInfoTypeName = dr["AGU_ComplianceSharedInfoTypeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_ComplianceSharedInfoTypeName"]);
            AgencyUserContract.AGU_ReqRotationSharedInfoTypeName = dr["AGU_ReqRotationSharedInfoTypeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_ReqRotationSharedInfoTypeName"]);
            AgencyUserContract.AGU_BkgSharedInfoTypeName = dr["AGU_BkgSharedInfoTypeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_BkgSharedInfoTypeName"]);


            AgencyUserContract.AgencyUserTemplateId = dr["AGU_TemplateId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_TemplateId"]);//UAT-3316
                                                                                                                                                  //Dictionary<Int32, List<Int32>> dict = new Dictionary<Int32, List<Int32>>();





            //    foreach (XmlElement node in xmlDoc.DocumentElement)
            //    {
            //        Int32 Key = Convert.ToInt32(node.ChildNodes[0].InnerText);
            //        List<Int32> val = new List<Int32>();
            //        if (dict.TryGetValue(Key, out val))
            //        {
            //            val.Add(Convert.ToInt32(node.ChildNodes[1].InnerText));
            //            dict[Key] = val;
            //        }
            //        else
            //        {
            //            val = new List<Int32>();
            //            val.Add(Convert.ToInt32(node.ChildNodes[1].InnerText));
            //            dict.Add(Key, val);
            //        }
            //    }
            //}
            //AgencyUserContract.DicSelectedAgencyInstitutionMapping = dict;

            Dictionary<Int32, String> dict = new Dictionary<Int32, String>();
            AgencyUserContract.lstAgencyHierarchyIds = new List<Int32>();
            if (dr["SelectedAgencyHieararchyMapping"].GetType().Name != "DBNull")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Convert.ToString(dr["SelectedAgencyHieararchyMapping"]));

                foreach (XmlElement node in xmlDoc.DocumentElement)
                {
                    Int32 Key = Convert.ToInt32(node.ChildNodes[0].InnerText);
                    String val = node.ChildNodes[1].IsNullOrEmpty() ? String.Empty : Convert.ToString(node.ChildNodes[1].InnerText);
                    dict.Add(Key, val);
                    AgencyUserContract.lstAgencyHierarchyIds.Add(Key);
                }
            }
            AgencyUserContract.SelectedAgencyHieararchyMapping = dict;
            AgencyUserContract.lstApplicationInvitationMetaDataID = dr["lstApplicationInvitationMetaDataID"].GetType().Name == "DBNull" ? new List<Int32>() : Convert.ToString(dr["lstApplicationInvitationMetaDataID"]).Split(',').ConvertIntoIntList();
            AgencyUserContract.AGU_AgencyUserPermission = Convert.ToBoolean(dr["AGU_AgencyUserPermission"]);
            AgencyUserContract.AGU_RotationPackagePermission = Convert.ToBoolean(dr["AGU_RotationPackagePermission"]);
            AgencyUserContract.lstInvitationSharedInfoTypeID = dr["lstInvitationSharedInfoTypeID"].GetType().Name == "DBNull" ? new List<Int32>() : Convert.ToString(dr["lstInvitationSharedInfoTypeID"]).Split(',').ConvertIntoIntList();
            AgencyUserContract.SharedInfoTypeName = dr["SharedInfoTypeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SharedInfoTypeName"]);

            AgencyUserContract.AgencyName = dr["AgencyName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AgencyName"]);
            AgencyUserContract.IsActive = dr["IsActive"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsActive"]);
            AgencyUserContract.IsLocked = dr["IsLocked"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsLocked"]);
            AgencyUserContract.AttestationRptPermission = Convert.ToBoolean(dr["AttestationRptPermission"]);
            //UAT-2538
            //  AgencyUserContract.IsEmailNeedToSend = Convert.ToBoolean(dr["IsEmailNeedToSend"]);//Code commented for UAT-2803
            //UAT-2447
            AgencyUserContract.IsInternationalPhone = Convert.ToBoolean(dr["IsInternationalPhone"]);
            AgencyUserContract.HideAgencyPortalDetailLink = Convert.ToBoolean(dr["HideAgencyPortalDetailLink_Permission"]);//UAT-3220
            AgencyUserContract.AGU_AgencyApplicantStatus = Convert.ToBoolean(dr["AGU_AgencyApplicantStatus"]); //UAT-2548
            AgencyUserContract.IsCreatedByClientAdmin = Convert.ToBoolean(dr["IsCreatedByClientAdmin"]);
            AgencyUserContract.RequirementPkgPermission = Convert.ToBoolean(dr["AGU_RotationPackageViewPermission"]); //UAT-2706
            AgencyUserContract.AllowJobPosting = Convert.ToBoolean(dr["AGU_AllowJobPosting"]); //UAT-2427
            AgencyUserContract.SSN_Permission = Convert.ToBoolean(dr["SSN_Permission"]); //UAT-2510
            AgencyUserContract.DoNotShowNonAgencyShares = Convert.ToBoolean(dr["AGU_DoNotShowNonAgencyShares"]); //UAT-2840
            AgencyUserContract.IsRequirementSharingNonRotationNotification = dr["IsRequirementSharingNonRotationNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsRequirementSharingNonRotationNotification"]);//UAT-2803
            AgencyUserContract.IsRequirementSharingRotationNotification = dr["IsRequirementSharingRotationNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsRequirementSharingRotationNotification"]);//UAT-2803
            AgencyUserContract.IsRotationInvitationApprovalRejectionNotification = dr["IsRotationInvitationApprovalRejectionNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsRotationInvitationApprovalRejectionNotification"]);//UAT-2803
            AgencyUserContract.IsIndividualProfileSharingWithEmailNotification = dr["IsIndividualProfileSharingWithEmailNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsIndividualProfileSharingWithEmailNotification"]);//UAT-2803
            AgencyUserContract.EmailConfiguration = dr["EmailConfiguration"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["EmailConfiguration"]);//UAT-2803
            AgencyUserContract.IsProfileSharingWithEmailNotification = dr["IsProfileSharingWithEmailNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsProfileSharingWithEmailNotification"]);//UAT-2942
            AgencyUserContract.SendOutOfComplianceNotification = dr["SendOutOfComplianceNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["SendOutOfComplianceNotification"]);//UAT-2977
            AgencyUserContract.SendUpdatedApplicantRequirementNotification = dr["SendUpdatedApplicantRequirementNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["SendUpdatedApplicantRequirementNotification"]);//UAT-3059
            AgencyUserContract.SendUpdatedRotationDetailsNotification = dr["SendUpdatedRotationDetailsNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["SendUpdatedRotationDetailsNotification"]);//UAT-3108
            AgencyUserContract.SendStudentDroppedFromRotationNotification = dr["SendStudentDroppedFromRotationNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["SendStudentDroppedFromRotationNotification"]);//UAT-3222
            AgencyUserContract.SendItSystemAccessFormNotification = dr["SendITSystemAccessFormNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["SendITSystemAccessFormNotification"]);//UAT-3998
            AgencyUserContract.SendRotationEndDateChangeNotification = dr["SendRotationEndDateChangeNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["SendRotationEndDateChangeNotification"]);//UAT-4561

            task.Wait();
            AgencyUserContract.AGU_TenantName = ListVaues.Keys.FirstOrDefault(); // dr["AGU_TenantName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_TenantName"]);
            AgencyUserContract.lstSelectedAGI_ID = (List<int>)ListVaues.Values.FirstOrDefault(); //dr["lstSelectedAGI_ID"].GetType().Name == "DBNull" ? new List<Int32>() : Convert.ToString(dr["lstSelectedAGI_ID"]).Split(',').ConvertIntoIntList();

            lstAgencyUser.Add(AgencyUserContract);
        }

        public Dictionary<string, List<int>> GetListValues(DataTable dt, int id)
        {
            Dictionary<string, List<int>> dcListValues = new Dictionary<string, List<int>>();
            List<int> lstValues = new List<int>();
            StringBuilder commaSeparatedString = new StringBuilder();

            string expression;
            expression = "AgencyUserID = " + id.ToString();
            DataRow[] foundRows;
            // Use the Select method to find all rows matching the filter.
            foundRows = dt.Select(expression);

            foreach (DataRow dr in foundRows)
            {
                lstValues.Add(Convert.ToInt32(dr["AGI_ID"]));
                string agency_Name = Convert.ToString(dr["TenantName"]);
                commaSeparatedString.Append(agency_Name + ",");

            }

            string _commaSeparatedString = commaSeparatedString.ToString();
            if (_commaSeparatedString.Length > 1)
                _commaSeparatedString = _commaSeparatedString.Substring(0, _commaSeparatedString.Length - 1);
            List<string> uniques = _commaSeparatedString.Split(',').Reverse().Distinct().Reverse().ToList();
            string newStr = string.Join(",", uniques);
            dcListValues.Add(newStr, lstValues);
            return dcListValues;
        }
        /// <summary>
        /// Get Agency User by User ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        AgencyUser IProfileSharingRepository.GetAgencyUserByUserID(String userID)
        {
            return _sharedDataDBContext.AgencyUsers.FirstOrDefault(con => con.AGU_UserID == new Guid(userID) && !con.AGU_IsDeleted);
        }

        Int32 IProfileSharingRepository.SaveAgencyUser(AgencyUserContract _agencyUser, Int32 loggedInUserID, List<AgencyUserPermission> lstAgencyUserPermission)
        {
            AgencyUser _newAgencyUser = new AgencyUser();
            _newAgencyUser.AGU_Name = _agencyUser.AGU_Name;
            _newAgencyUser.AGU_Phone = _agencyUser.AGU_Phone;
            _newAgencyUser.AGU_Email = _agencyUser.AGU_Email;
            //_newAgencyUser.AGU_AgencyID = _agencyUser.AGU_AgencyID;//UAT-1641
            _newAgencyUser.AGU_ComplianceSharedInfoTypeID = _agencyUser.AGU_ComplianceSharedInfoTypeID;
            _newAgencyUser.AGU_BkgSharedInfoTypeID = _agencyUser.AGU_BkgSharedInfoTypeID;
            // _newAgencyUser.AGU_AgencyUserPermission = _agencyUser.AGU_AgencyUserPermission;
            // _newAgencyUser.AGU_RotationPackagePermission = _agencyUser.AGU_RotationPackagePermission;
            _newAgencyUser.AGU_CreatedByID = loggedInUserID;
            _newAgencyUser.AGU_ReqRotationSharedInfoTypeID = _agencyUser.AGU_ReqRotationSharedInfoTypeID;
            _newAgencyUser.AGU_CreatedOn = DateTime.Now;
            _newAgencyUser.AGU_IsCreatedBySharedUser = _agencyUser.IsCreatedBySharedUser;
            //UAT-2538
            // _newAgencyUser.AGU_IsNeedToSendEmail = _agencyUser.IsEmailNeedToSend;//Code commented for UAT-2803
            // _newAgencyUser.AGU_AgencyApplicantStatus = _agencyUser.AGU_AgencyApplicantStatus;//UAT-2548
            //UAT-2447
            _newAgencyUser.AGU_IsInternationalPhone = _agencyUser.IsInternationalPhone;
            //UAT-2641
            if (_agencyUser.CreatedByClientID > AppConsts.NONE)
            {
                _newAgencyUser.AGU_CreatedByClient = _agencyUser.CreatedByClientID;
            }

            _newAgencyUser.AGU_TemplateId = _agencyUser.AgencyUserTemplateId;
            //UAT-2706
            //  _newAgencyUser.AGU_RotationPackageViewPermission = _agencyUser.RequirementPkgPermission;

            //UAT-2427
            //  _newAgencyUser.AGU_AllowJobPosting = _agencyUser.AllowJobPosting;
            //  _newAgencyUser.AGU_DoNotShowNonAgencyShares = _agencyUser.DoNotShowNonAgencyShares;

            _sharedDataDBContext.AddToAgencyUsers(_newAgencyUser);
            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                Guid verificationCode = Guid.NewGuid();
                //foreach (Int32 agencyID in _agencyUser.LstAGU_AgencyID)
                //{
                //    UserAgencyMapping newUserAgencyMapping = new UserAgencyMapping();
                //    newUserAgencyMapping.UAM_AgencyID = agencyID;
                //    newUserAgencyMapping.UAM_AgencyUserID = _newAgencyUser.AGU_ID;
                //    newUserAgencyMapping.UAM_IsVerified = false;
                //    newUserAgencyMapping.UAM_VerificationCode = verificationCode;
                //    newUserAgencyMapping.UAM_IsDeleted = false;
                //    newUserAgencyMapping.UAM_CreatedBy = loggedInUserID;
                //    newUserAgencyMapping.UAM_CreatedOn = DateTime.Now;
                //    foreach (Int32 AGI_ID in _agencyUser.DicSelectedAgencyInstitutionMapping.GetValue(agencyID))
                //    {
                //        AgencyUserInstitution agencyUserInstitution = new AgencyUserInstitution();
                //        agencyUserInstitution.AGUI_AgencyInstitutionID = AGI_ID;
                //        agencyUserInstitution.AGUI_IsDeleted = false;
                //        agencyUserInstitution.AGUI_CreatedByID = loggedInUserID;
                //        agencyUserInstitution.AGUI_CreatedOn = DateTime.Now;
                //        newUserAgencyMapping.AgencyUserInstitutions.Add(agencyUserInstitution);
                //    }
                //    _sharedDataDBContext.AddToUserAgencyMappings(newUserAgencyMapping);
                //}

                //Add Agency User Meta Data
                foreach (var applicationInvtMetaDataIds in _agencyUser.lstApplicationInvitationMetaDataID)
                {
                    AgencyUserSharedData agencyUserSharedData = new AgencyUserSharedData();
                    agencyUserSharedData.AUSD_AgencyUserID = _newAgencyUser.AGU_ID;
                    agencyUserSharedData.AUSD_ApplicationInvitationMetaDataID = applicationInvtMetaDataIds;
                    agencyUserSharedData.AUSD_IsDeleted = false;
                    agencyUserSharedData.AUSD_CreatedByID = loggedInUserID;
                    agencyUserSharedData.AUSD_CreatedOn = DateTime.Now;

                    _sharedDataDBContext.AddToAgencyUserSharedDatas(agencyUserSharedData);
                }

                //UAT-1213: Updates to Agency User background check permissions.
                //Add Invitation Shared Info Mapping
                foreach (var invitationSharedInfoTypeID in _agencyUser.lstInvitationSharedInfoTypeID)
                {
                    Entity.SharedDataEntity.InvitationSharedInfoMapping invitationSharedInfoMapping = new Entity.SharedDataEntity.InvitationSharedInfoMapping();
                    invitationSharedInfoMapping.ISIM_AgencyUserID = _newAgencyUser.AGU_ID;
                    invitationSharedInfoMapping.ISIM_InvitationSharedInfoTypeID = invitationSharedInfoTypeID;
                    invitationSharedInfoMapping.ISIM_IsDeleted = false;
                    invitationSharedInfoMapping.ISIM_CreatedByID = loggedInUserID;
                    invitationSharedInfoMapping.ISIM_CreatedOn = DateTime.Now;
                    _sharedDataDBContext.AddToInvitationSharedInfoMappings(invitationSharedInfoMapping);
                }

                if (!lstAgencyUserPermission.IsNullOrEmpty())
                {
                    //for existing records before UAT 1616,
                    foreach (AgencyUserPermission agencyUserPermission in lstAgencyUserPermission)
                    {
                        _newAgencyUser.AgencyUserPermissions.Add(new AgencyUserPermission()
                        {
                            AUP_PermissionTypeID = agencyUserPermission.AUP_PermissionTypeID,
                            AUP_PermissionAccessTypeID = agencyUserPermission.AUP_PermissionAccessTypeID,
                            AUP_IsDeleted = false,
                            AUP_CreatedByID = loggedInUserID,
                            AUP_CreatedOn = DateTime.Now,
                            AUP_RecordTypeID = agencyUserPermission.AUP_RecordTypeID
                        });
                    }
                }

                if (_sharedDataDBContext.SaveChanges() > 0)
                {
                    return _newAgencyUser.AGU_ID;
                }
                return AppConsts.NONE;
            }
            else
            {
                return AppConsts.NONE;
            }
        }

        String IProfileSharingRepository.DeleteAgencyUser(Int32 TenantID, Int32 AUG_ID, Int32 LoggedInUserId, List<Int32> lstAgencyInstID, Boolean IsAdmin)
        {
            if (IsAdmin)
            {
                AgencyUser agencyUser = _sharedDataDBContext.AgencyUsers.Where(x => x.AGU_ID == AUG_ID && x.AGU_IsDeleted == false).FirstOrDefault();
                agencyUser.AGU_IsDeleted = true;
                agencyUser.AGU_ModifiedByID = LoggedInUserId;
                agencyUser.AGU_ModifiedOn = DateTime.Now;

                List<Int32> agencyHierarchyIds = agencyUser.AgencyHierarchyUsers.Where(cmd => !cmd.AHU_IsDeleted).Select(sel => sel.AHU_AgencyHierarchyID).ToList();

                agencyUser.AgencyHierarchyUsers.Where(cmd => !cmd.AHU_IsDeleted).ForEach(x =>
                {
                    x.AHU_IsDeleted = false;
                    x.AHU_ModifiedBy = LoggedInUserId;
                    x.AHU_ModifiedOn = DateTime.Now;
                });

                #region UAT-2637 : Agency hierarchy mapping: Automatic consolidation of user permissions
                foreach (var item in agencyHierarchyIds)
                {
                    EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
                    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        SqlCommand command = new SqlCommand("usp_ConsolidateAgencyUserPermissions", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AgencyHierarchyID", item);
                        command.Parameters.AddWithValue("@CurrentLoggedInUserID", LoggedInUserId);
                        command.Parameters.AddWithValue("@AgencyUserID", AUG_ID);
                        command.ExecuteScalar();
                        con.Close();
                    }
                }
                #endregion

                if (_sharedDataDBContext.SaveChanges() > 0)
                {
                    //IEnumerable<UserAgencyMapping> lstUserAgencyMapping = agencyUser.UserAgencyMappings
                    //                                      .Where(cond => !cond.UAM_IsDeleted);
                    ////Delete Agency User Institutions
                    //if (lstUserAgencyMapping.IsNotNull())
                    //{
                    //    foreach (var userAgencyMapping in lstUserAgencyMapping)
                    //    {
                    //        userAgencyMapping.UAM_IsDeleted = true;
                    //        userAgencyMapping.UAM_ModifiedBy = LoggedInUserId;
                    //        userAgencyMapping.UAM_ModifiedOn = DateTime.Now;
                    //        foreach (var AGUI in userAgencyMapping.AgencyUserInstitutions)
                    //        {
                    //            AGUI.AGUI_IsDeleted = true;
                    //            AGUI.AGUI_ModifiedByID = LoggedInUserId;
                    //            AGUI.AGUI_ModifiedOn = DateTime.Now;
                    //        }
                    //    }
                    //}

                    //Delete Agency User Shared Data
                    IEnumerable<AgencyUserSharedData> lstAgencyUserSharedData = agencyUser.AgencyUserSharedDatas.Where(x => x.AUSD_IsDeleted == false);
                    if (lstAgencyUserSharedData.IsNotNull())
                    {
                        foreach (var agencyUserSharedData in lstAgencyUserSharedData)
                        {
                            agencyUserSharedData.AUSD_IsDeleted = true;
                            agencyUserSharedData.AUSD_ModifiedByID = LoggedInUserId;
                            agencyUserSharedData.AUSD_ModifiedOn = DateTime.Now;
                        }
                    }

                    //UAT-1213: Updates to Agency User background check permissions.
                    //Delete Invitation Shared Info Mapping
                    IEnumerable<Entity.SharedDataEntity.InvitationSharedInfoMapping> lstInvitationSharedInfoMapping = agencyUser.InvitationSharedInfoMappings.Where(x => x.ISIM_IsDeleted == false);
                    if (lstInvitationSharedInfoMapping.IsNotNull())
                    {
                        foreach (var invitationSharedInfoMapping in lstInvitationSharedInfoMapping)
                        {
                            invitationSharedInfoMapping.ISIM_IsDeleted = true;
                            invitationSharedInfoMapping.ISIM_ModifiedByID = LoggedInUserId;
                            invitationSharedInfoMapping.ISIM_ModifiedOn = DateTime.Now;
                        }
                    }

                    //UAT 1616 WB: Agency users should not have to assign what schools have access to rotation packages

                    if (agencyUser.AGU_TemplateId == 0)
                    {
                        IEnumerable<AgencyUserPermission> agencyUserPermissions = agencyUser.AgencyUserPermissions.Where(x => !x.AUP_IsDeleted);
                        foreach (AgencyUserPermission agencyUserPermission in agencyUserPermissions)
                        {
                            agencyUserPermission.AUP_IsDeleted = true;
                            agencyUserPermission.AUP_ModifiedByID = LoggedInUserId;
                            agencyUserPermission.AUP_ModifiedOn = DateTime.Now;
                        }
                    }
                    #region 
                    //UAT-4579 

                    //else
                    //{
                    //    IEnumerable<AgencyUserPermissionTemplateMapping> agencyUserTemplatePermissions = _sharedDataDBContext.AgencyUserPermissionTemplateMappings.Where(x => x.AGUPTM_AgencyUserPerTemplateId == agencyUser.AGU_TemplateId && !x.AGUPTM_IsDeleted).ToList();
                    //    //agencyUser.AgencyUserPermissionTemplate.AgencyUserPermissionTemplateMappings.Where(x => x.AGUPTM_AgencyUserPerTemplateId == agencyUser.AGU_TemplateId && !x.AGUPTM_IsDeleted);
                    //    foreach (AgencyUserPermissionTemplateMapping agencyUserTempPerMapping in agencyUserTemplatePermissions)
                    //    {
                    //        agencyUserTempPerMapping.AGUPTM_IsDeleted = true;
                    //        agencyUserTempPerMapping.AGUPTM_ModifiedByID = LoggedInUserId;
                    //        agencyUserTempPerMapping.AGUPTM_ModifiedOn = DateTime.Now;
                    //    }
                    //}

                    #endregion
                    _sharedDataDBContext.SaveChanges();
                    DeleteAgencyHierarchyUserDetails(AUG_ID, LoggedInUserId);
                    //UAT-4160
                    DeleteOrganizationUserTypeMapping(Convert.ToString(agencyUser.AGU_UserID), LoggedInUserId);

                    return AppConsts.AGU_DELETED_SUCCESS_MSG;
                }
                else
                {
                    return AppConsts.AGU_DELETED_ERROR_MSG;
                }
            }
            else
            {
                //Remove only mapping
                Int32 agencyInstID = _sharedDataDBContext.AgencyInstitutions.Where(x => lstAgencyInstID.Contains(x.AGI_ID) && x.AGI_TenantID == TenantID && !x.AGI_IsDeleted).Select(c => c.AGI_ID).FirstOrDefault();
                //Client Admin
                AgencyUserInstitution agencyUserInstitution = _sharedDataDBContext.AgencyUserInstitutions
                 .Where(cond => cond.UserAgencyMapping.UAM_AgencyUserID == AUG_ID && cond.AGUI_AgencyInstitutionID == agencyInstID && !cond.AGUI_IsDeleted).FirstOrDefault();

                agencyUserInstitution.AGUI_IsDeleted = true;
                agencyUserInstitution.AGUI_ModifiedByID = LoggedInUserId;
                agencyUserInstitution.AGUI_ModifiedOn = DateTime.Now;

                _sharedDataDBContext.SaveChanges();
                return AppConsts.AGU_DELETED_SUCCESS_MSG;
            }
        }

        //String IProfileSharingRepository.UpdateAgencyUser(AgencyUserContract _agencyUser, Int32 LoggedInUserId, Boolean IsAdmin, List<Int32> agencyInstitutionIDs_Added, List<Int32> agencyInstitutionIDs_Removed, List<AgencyUserPermission> lstAgencyUserPermission)
        AgencyUser IProfileSharingRepository.UpdateAgencyUser(AgencyUserContract _agencyUser, Int32 LoggedInUserId, Boolean IsAdmin, List<AgencyUserPermission> lstAgencyUserPermission)
        {

            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            {
                AgencyUser agencyUser = _sharedDataDBContext.AgencyUsers.Where(x => x.AGU_ID == _agencyUser.AGU_ID && x.AGU_IsDeleted == false).FirstOrDefault();
                agencyUser.AGU_Name = _agencyUser.AGU_Name;
                agencyUser.AGU_Phone = _agencyUser.AGU_Phone;
                //agencyUser.AGU_Email = _agencyUser.AGU_Email;
                //agencyUser.AGU_AgencyID = _agencyUser.AGU_AgencyID;//UAT 1641
                agencyUser.AGU_ComplianceSharedInfoTypeID = _agencyUser.AGU_ComplianceSharedInfoTypeID;
                // agencyUser.AGU_AgencyUserPermission = _agencyUser.AGU_AgencyUserPermission;
                //agencyUser.AGU_RotationPackagePermission = _agencyUser.AGU_RotationPackagePermission;
                agencyUser.AGU_BkgSharedInfoTypeID = _agencyUser.AGU_BkgSharedInfoTypeID;
                agencyUser.AGU_ReqRotationSharedInfoTypeID = _agencyUser.AGU_ReqRotationSharedInfoTypeID;
                agencyUser.AGU_IsDeleted = false;
                agencyUser.AGU_ModifiedByID = LoggedInUserId;
                agencyUser.AGU_ModifiedOn = DateTime.Now;
                // agencyUser.AGU_IsNeedToSendEmail = _agencyUser.IsEmailNeedToSend;//Code commented for UAT-2803
                //UAT-2447
                agencyUser.AGU_IsInternationalPhone = _agencyUser.IsInternationalPhone;
                //UAT-2548
                //agencyUser.AGU_AgencyApplicantStatus = _agencyUser.AGU_AgencyApplicantStatus;
                //UAT-2706
                // agencyUser.AGU_RotationPackageViewPermission = _agencyUser.RequirementPkgPermission;
                //UAT-2427
                //agencyUser.AGU_AllowJobPosting = _agencyUser.AllowJobPosting;

                //  agencyUser.AGU_DoNotShowNonAgencyShares = _agencyUser.DoNotShowNonAgencyShares;
                agencyUser.AGU_TemplateId = _agencyUser.AgencyUserTemplateId;

                if (_sharedDataDBContext.SaveChanges() > 0)
                {
                    List<AgencyUserSharedData> lstAgencyUserSharedData = _sharedDataDBContext.AgencyUserSharedDatas.Where(x => x.AUSD_AgencyUserID == _agencyUser.AGU_ID && x.AUSD_IsDeleted == false).ToList();
                    if (lstAgencyUserSharedData.IsNotNull())
                    {
                        //Delete Old Data
                        foreach (var agencyUserSharedData in lstAgencyUserSharedData)
                        {
                            agencyUserSharedData.AUSD_IsDeleted = true;
                            agencyUserSharedData.AUSD_ModifiedByID = LoggedInUserId;
                            agencyUserSharedData.AUSD_ModifiedOn = DateTime.Now;
                        }

                        //Add New Data
                        foreach (var applicationInvtMetaDataIds in _agencyUser.lstApplicationInvitationMetaDataID)
                        {
                            AgencyUserSharedData agencyUserSharedData = new AgencyUserSharedData();
                            agencyUserSharedData.AUSD_AgencyUserID = agencyUser.AGU_ID;
                            agencyUserSharedData.AUSD_ApplicationInvitationMetaDataID = applicationInvtMetaDataIds;
                            agencyUserSharedData.AUSD_IsDeleted = false;
                            agencyUserSharedData.AUSD_CreatedByID = LoggedInUserId;
                            agencyUserSharedData.AUSD_CreatedOn = DateTime.Now;

                            _sharedDataDBContext.AddToAgencyUserSharedDatas(agencyUserSharedData);
                        }
                    }

                    //UAT-1213: Updates to Agency User background check permissions.
                    List<Entity.SharedDataEntity.InvitationSharedInfoMapping> lstInvitationSharedInfoMapping = _sharedDataDBContext.InvitationSharedInfoMappings.Where(x => x.ISIM_AgencyUserID == _agencyUser.AGU_ID && x.ISIM_IsDeleted == false).ToList();
                    if (lstInvitationSharedInfoMapping.IsNotNull())
                    {
                        //Delete Old Data
                        foreach (var invitationSharedInfoMapping in lstInvitationSharedInfoMapping)
                        {
                            invitationSharedInfoMapping.ISIM_IsDeleted = true;
                            invitationSharedInfoMapping.ISIM_ModifiedByID = LoggedInUserId;
                            invitationSharedInfoMapping.ISIM_ModifiedOn = DateTime.Now;
                        }

                        //Add New Data
                        foreach (var invitationSharedInfoTypeID in _agencyUser.lstInvitationSharedInfoTypeID)
                        {
                            Entity.SharedDataEntity.InvitationSharedInfoMapping invitationSharedInfoMapping = new Entity.SharedDataEntity.InvitationSharedInfoMapping();
                            invitationSharedInfoMapping.ISIM_AgencyUserID = agencyUser.AGU_ID;
                            invitationSharedInfoMapping.ISIM_InvitationSharedInfoTypeID = invitationSharedInfoTypeID;
                            invitationSharedInfoMapping.ISIM_IsDeleted = false;
                            invitationSharedInfoMapping.ISIM_CreatedByID = LoggedInUserId;
                            invitationSharedInfoMapping.ISIM_CreatedOn = DateTime.Now;

                            _sharedDataDBContext.AddToInvitationSharedInfoMappings(invitationSharedInfoMapping);
                        }
                    }
                    #region Commented code
                    //List<UserAgencyMapping> existingUserAgencyMappings = agencyUser.UserAgencyMappings.Where(cond => !cond.UAM_IsDeleted).ToList();

                    //Guid verificationCode;

                    //if (existingUserAgencyMappings.Any(cond => !cond.UAM_IsVerified))
                    //{
                    //    verificationCode = existingUserAgencyMappings.Where(cond => !cond.UAM_IsVerified).LastOrDefault().UAM_VerificationCode;
                    //}
                    //else
                    //{
                    //    verificationCode = Guid.NewGuid();
                    //}

                    ////UAT-2541:Agency users should not have to activate after additional agencies are added to their permission.   
                    //Boolean IsAnyMappingAlreadyVerified = false;
                    //if (existingUserAgencyMappings.Any(cond => cond.UAM_IsVerified && !cond.UAM_IsDeleted))
                    //{
                    //    IsAnyMappingAlreadyVerified = true;
                    //}

                    //foreach (Int32 agencyID in _agencyUser.LstAGU_AgencyID)
                    //{
                    //    if (!existingUserAgencyMappings.Any(cond => cond.UAM_AgencyID == agencyID))
                    //    {
                    //        UserAgencyMapping newUserAgencyMapping = new UserAgencyMapping();
                    //        newUserAgencyMapping.UAM_AgencyID = agencyID;
                    //        newUserAgencyMapping.UAM_AgencyUserID = agencyUser.AGU_ID;
                    //        newUserAgencyMapping.UAM_IsVerified = IsAnyMappingAlreadyVerified;
                    //        newUserAgencyMapping.UAM_VerificationCode = verificationCode;
                    //        newUserAgencyMapping.UAM_IsDeleted = false;
                    //        newUserAgencyMapping.UAM_CreatedBy = LoggedInUserId;
                    //        newUserAgencyMapping.UAM_CreatedOn = DateTime.Now;
                    //        if (!_agencyUser.DicSelectedAgencyInstitutionMapping.IsNullOrEmpty())
                    //        {
                    //            foreach (Int32 AGI_ID in _agencyUser.DicSelectedAgencyInstitutionMapping.GetValue(agencyID))
                    //            {
                    //                AgencyUserInstitution agencyUserInstitution = new AgencyUserInstitution();
                    //                agencyUserInstitution.AGUI_AgencyInstitutionID = AGI_ID;
                    //                agencyUserInstitution.AGUI_IsDeleted = false;
                    //                agencyUserInstitution.AGUI_CreatedByID = LoggedInUserId;
                    //                agencyUserInstitution.AGUI_CreatedOn = DateTime.Now;
                    //                newUserAgencyMapping.AgencyUserInstitutions.Add(agencyUserInstitution);
                    //            }
                    //        }
                    //        _sharedDataDBContext.AddToUserAgencyMappings(newUserAgencyMapping);
                    //    }
                    //    else
                    //    {
                    //        UserAgencyMapping existingUserAgencyMapping = existingUserAgencyMappings.Where(cond => cond.UAM_AgencyID == agencyID).FirstOrDefault();
                    //        List<Int32> mappedAgencyInstitutionIDs = _agencyUser.DicSelectedAgencyInstitutionMapping.GetValue(agencyID);


                    //        List<AgencyUserInstitution> existingAgencyUserInstitutions = existingUserAgencyMapping.AgencyUserInstitutions
                    //                                                .Where(cond => !cond.AGUI_IsDeleted).ToList();
                    //        foreach (AgencyUserInstitution existingAgencyUserInstitution in existingAgencyUserInstitutions)
                    //        {
                    //            if (mappedAgencyInstitutionIDs.Contains(existingAgencyUserInstitution.AGUI_AgencyInstitutionID.Value))
                    //            {
                    //                mappedAgencyInstitutionIDs.Remove(existingAgencyUserInstitution.AGUI_AgencyInstitutionID.Value);
                    //            }
                    //            else if (IsAdmin)
                    //            {
                    //                existingAgencyUserInstitution.AGUI_IsDeleted = true;
                    //                existingAgencyUserInstitution.AGUI_ModifiedByID = LoggedInUserId;
                    //                existingAgencyUserInstitution.AGUI_ModifiedOn = DateTime.Now;
                    //            }
                    //        }
                    //        foreach (Int32 agencyInstiuteID in mappedAgencyInstitutionIDs)
                    //        {
                    //            AgencyUserInstitution agencyUserInstitution = new AgencyUserInstitution();
                    //            agencyUserInstitution.AGUI_AgencyInstitutionID = agencyInstiuteID;
                    //            agencyUserInstitution.AGUI_IsDeleted = false;
                    //            agencyUserInstitution.AGUI_CreatedByID = LoggedInUserId;
                    //            agencyUserInstitution.AGUI_CreatedOn = DateTime.Now;
                    //            existingUserAgencyMapping.AgencyUserInstitutions.Add(agencyUserInstitution);
                    //        }
                    //    }

                    //}

                    //if (IsAdmin) //Update AgencyUserInstitutions for Admins and Shared(Agency) Users.
                    //{
                    //    List<AgencyUserInstitution> lstAgencyUserInstitution = new List<AgencyUserInstitution>();
                    //    //_sharedDataDBContext.AgencyUserInstitutions
                    //    //                                  .Where(cond => cond.AGUI_AgencyUserID == _agencyUser.AGU_ID && !cond.AGUI_IsDeleted).ToList();//rachit-1641

                    //    if (lstAgencyUserInstitution.IsNotNull())
                    //    {

                    //        var lstAgencyUserInstitution_ToRemoved = lstAgencyUserInstitution.Where(x => agencyInstitutionIDs_Removed.Contains(x.AGUI_AgencyInstitutionID.Value)).ToList();

                    //        //Remved
                    //        foreach (var agencyUserInstitution in lstAgencyUserInstitution_ToRemoved)
                    //        {
                    //            agencyUserInstitution.AGUI_IsDeleted = true;
                    //            agencyUserInstitution.AGUI_ModifiedByID = LoggedInUserId;
                    //            agencyUserInstitution.AGUI_ModifiedOn = DateTime.Now;
                    //        }

                    //        //Add New Record
                    //        foreach (Int32 agencyInstitutionID in agencyInstitutionIDs_Added)
                    //        {
                    //            AgencyUserInstitution agencyUserInstitution = new AgencyUserInstitution();
                    //            //agencyUserInstitution.AGUI_AgencyUserID = _agencyUser.AGU_ID;//rachit-1641
                    //            agencyUserInstitution.AGUI_AgencyInstitutionID = agencyInstitutionID;
                    //            agencyUserInstitution.AGUI_IsDeleted = false;
                    //            agencyUserInstitution.AGUI_CreatedByID = LoggedInUserId;
                    //            agencyUserInstitution.AGUI_CreatedOn = DateTime.Now;
                    //            _sharedDataDBContext.AddToAgencyUserInstitutions(agencyUserInstitution);
                    //        }

                    //        ////Delete Old Data
                    //        //foreach (var agencyUserInstitution in lstAgencyUserInstitution)
                    //        //{
                    //        //    agencyUserInstitution.AGUI_IsDeleted = true;
                    //        //    agencyUserInstitution.AGUI_ModifiedByID = LoggedInUserId;
                    //        //    agencyUserInstitution.AGUI_ModifiedOn = DateTime.Now;
                    //        //}

                    //        ////Add new Data
                    //        //foreach (Int32 agencyInstitutionID in _agencyUser.AGU_TenantIDLst)
                    //        //{
                    //        //    Entity.AgencyUserInstitution agencyUserInstitution = new Entity.AgencyUserInstitution();
                    //        //    agencyUserInstitution.AGUI_AgencyUserID = _agencyUser.AGU_ID;
                    //        //    agencyUserInstitution.AGUI_AgencyInstitutionID = agencyInstitutionID;
                    //        //    agencyUserInstitution.AGUI_IsDeleted = false;
                    //        //    agencyUserInstitution.AGUI_CreatedByID = LoggedInUserId;
                    //        //    agencyUserInstitution.AGUI_CreatedOn = DateTime.Now;
                    //        //    base.SecurityContext.AddToAgencyUserInstitutions(agencyUserInstitution);
                    //        //}
                    //    }
                    //}
                    #endregion

                    #region Agency User Permission
                    IEnumerable<AgencyUserPermission> existingAgencyUserPermissions = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted);

                    //Start UAT-3664 : Agency User Report Type Permissions.
                    String reportPermissionTypeCode = AgencyUserPermissionType.REPORTS_PERMISSION.GetStringValue();
                    Int32 reportPermissionTypeID = this.SharedDataDBContext.lkpAgencyUserPermissionTypes.Where(cond => !cond.AUPT_IsDeleted && cond.AUPT_Code == reportPermissionTypeCode).FirstOrDefault().AUPT_ID;

                    List<AgencyUserPermission> existingAgencyUserReportsPermissions = existingAgencyUserPermissions.Where(cond => cond.AUP_PermissionTypeID == reportPermissionTypeID).ToList();
                    List<AgencyUserPermission> lstAgencyUserReportsPermission = lstAgencyUserPermission.Where(cond => cond.AUP_PermissionTypeID == reportPermissionTypeID).ToList();

                    foreach (AgencyUserPermission agencyUserReportPermission in lstAgencyUserReportsPermission)
                    {
                        if (!existingAgencyUserReportsPermissions.IsNullOrEmpty() && existingAgencyUserReportsPermissions.Where(cond => cond.AUP_RecordTypeID == agencyUserReportPermission.AUP_RecordTypeID).Any())
                        {
                            AgencyUserPermission aup = existingAgencyUserReportsPermissions.Where(cond => cond.AUP_RecordTypeID == agencyUserReportPermission.AUP_RecordTypeID).FirstOrDefault();

                            //update
                            aup.AUP_PermissionAccessTypeID = agencyUserReportPermission.AUP_PermissionAccessTypeID;
                            aup.AUP_PermissionTypeID = agencyUserReportPermission.AUP_PermissionTypeID;
                            aup.AUP_IsDeleted = false;
                            aup.AUP_ModifiedByID = LoggedInUserId;
                            aup.AUP_ModifiedOn = DateTime.Now;
                            aup.AUP_RecordTypeID = agencyUserReportPermission.AUP_RecordTypeID;

                            existingAgencyUserReportsPermissions = existingAgencyUserReportsPermissions.Where(cond => cond.AUP_RecordTypeID != agencyUserReportPermission.AUP_RecordTypeID).ToList();
                        }
                        else
                        {
                            //Insert
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = agencyUserReportPermission.AUP_PermissionTypeID;
                            dbInsert.AUP_PermissionAccessTypeID = agencyUserReportPermission.AUP_PermissionAccessTypeID;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = LoggedInUserId;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            dbInsert.AUP_RecordTypeID = agencyUserReportPermission.AUP_RecordTypeID;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }

                    }

                    if (!existingAgencyUserReportsPermissions.IsNullOrEmpty())
                    {
                        //These are unchecked record, i.e these reports have permission access "YES".
                        String yesPermissionAccessCode = AgencyUserPermissionAccessType.YES.GetStringValue();
                        Int32 yesPermissionAccessID = this.SharedDataDBContext.lkpAgencyUserPermissionAccessTypes.Where(con => !con.AUPAT_IsDeleted && con.AUPAT_Code == yesPermissionAccessCode).FirstOrDefault().AUPAT_ID;

                        if (yesPermissionAccessID > AppConsts.NONE)
                        {
                            foreach (AgencyUserPermission existingAgencyUserReportPermission in existingAgencyUserReportsPermissions)
                            {
                                AgencyUserPermission aup = existingAgencyUserReportsPermissions.Where(cond => cond.AUP_RecordTypeID == existingAgencyUserReportPermission.AUP_RecordTypeID).FirstOrDefault();

                                //update
                                aup.AUP_PermissionAccessTypeID = yesPermissionAccessID;
                                aup.AUP_IsDeleted = false;
                                aup.AUP_ModifiedByID = LoggedInUserId;
                                aup.AUP_ModifiedOn = DateTime.Now;
                            }
                        }
                    }

                    _sharedDataDBContext.SaveChanges();

                    existingAgencyUserPermissions = existingAgencyUserPermissions.Where(cond => cond.AUP_PermissionTypeID != reportPermissionTypeID);
                    lstAgencyUserPermission.RemoveAll(c => c.AUP_PermissionTypeID == reportPermissionTypeID);

                    //END UAT-3664

                    foreach (AgencyUserPermission agencyUserPermission in lstAgencyUserPermission)
                    {
                        if (!existingAgencyUserPermissions.IsNullOrEmpty() && existingAgencyUserPermissions.Where(cond => cond.AUP_PermissionTypeID == agencyUserPermission.AUP_PermissionTypeID).Any())
                        {
                            AgencyUserPermission aup = existingAgencyUserPermissions.Where(cond => cond.AUP_PermissionTypeID == agencyUserPermission.AUP_PermissionTypeID).FirstOrDefault();

                            //update
                            aup.AUP_PermissionAccessTypeID = agencyUserPermission.AUP_PermissionAccessTypeID;
                            aup.AUP_PermissionTypeID = agencyUserPermission.AUP_PermissionTypeID;
                            aup.AUP_IsDeleted = false;
                            aup.AUP_ModifiedByID = LoggedInUserId;
                            aup.AUP_ModifiedOn = DateTime.Now;
                        }
                        else
                        {
                            //Insert
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = agencyUserPermission.AUP_PermissionTypeID;
                            dbInsert.AUP_PermissionAccessTypeID = agencyUserPermission.AUP_PermissionAccessTypeID;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = LoggedInUserId;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }
                        _sharedDataDBContext.SaveChanges();
                    }
                    #endregion

                    #region Old commented Agency User Permission code
                    ////UAT 1616 WB: Agency users should not have to assign what schools have access to rotation packages
                    //IEnumerable<AgencyUserPermission> existingAgencyUserPermissions = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted);
                    //if (!existingAgencyUserPermissions.IsNullOrEmpty())
                    //{
                    //    foreach (AgencyUserPermission existingAgencyUserPermission in existingAgencyUserPermissions)
                    //    {
                    //        AgencyUserPermission aup = lstAgencyUserPermission.Where(cond => cond.AUP_PermissionTypeID == existingAgencyUserPermission.AUP_PermissionTypeID).FirstOrDefault();
                    //        if (!aup.IsNullOrEmpty())  //.Contains(existingAgencyUserPermission.AUP_PermissionTypeID))
                    //        {
                    //            existingAgencyUserPermission.AUP_PermissionAccessTypeID = aup.AUP_PermissionAccessTypeID;
                    //            existingAgencyUserPermission.AUP_PermissionTypeID = aup.AUP_PermissionTypeID;
                    //            existingAgencyUserPermission.AUP_IsDeleted = false;
                    //            existingAgencyUserPermission.AUP_ModifiedByID = LoggedInUserId;
                    //            existingAgencyUserPermission.AUP_ModifiedOn = DateTime.Now;
                    //        }
                    //        else
                    //        {
                    //            agencyUser.AgencyUserPermissions.Add(new AgencyUserPermission()
                    //            {
                    //                AUP_PermissionTypeID = aup.AUP_PermissionTypeID,
                    //                AUP_PermissionAccessTypeID = aup.AUP_PermissionAccessTypeID,
                    //                AUP_IsDeleted = false,
                    //                AUP_CreatedByID = LoggedInUserId,
                    //                AUP_CreatedOn = DateTime.Now,
                    //            });
                    //        }
                    //    }
                    //}
                    //else if (!lstAgencyUserPermission.IsNullOrEmpty())
                    //{
                    //    //for existing records before UAT 1616,
                    //    foreach (AgencyUserPermission agencyUserPermission in lstAgencyUserPermission)
                    //    {
                    //        agencyUser.AgencyUserPermissions.Add(new AgencyUserPermission()
                    //        {
                    //            AUP_PermissionTypeID = agencyUserPermission.AUP_PermissionTypeID,
                    //            AUP_PermissionAccessTypeID = agencyUserPermission.AUP_PermissionAccessTypeID,
                    //            AUP_IsDeleted = false,
                    //            AUP_CreatedByID = LoggedInUserId,
                    //            AUP_CreatedOn = DateTime.Now,
                    //        });
                    //    }
                    //}
                    #endregion

                    scope.Complete();
                    return agencyUser;
                }
                //  To commit.
            }
            return null;
        }

        List<Int32> IProfileSharingRepository.GetAgencyUserSharedDataForAgencyUserID(int agencyUserID)
        {
            return _sharedDataDBContext.AgencyUserSharedDatas.Where(x => x.AUSD_AgencyUserID == agencyUserID && !x.AUSD_IsDeleted)
                .Select(cond => cond.AUSD_ApplicationInvitationMetaDataID.Value).ToList();
        }


        List<AgencyInstitution> IProfileSharingRepository.GetAgencyUserInstitutesForAgencyUserID(int agencyUserID)
        {
            //return _sharedDataDBContext.AgencyUserInstitutions.Where(cond => cond.AGUI_AgencyUserID == agencyUserID && !cond.AGUI_IsDeleted)
            //    .Select(x => x.AGUI_AgencyInstitutionID.Value).ToList();

            #region Changes corresponding to UAT-1641:As an Agency User, I should be able to be linked to multiple agencies

            return _sharedDataDBContext.AgencyUserInstitutions
                                            .Where(cond => cond.UserAgencyMapping.UAM_AgencyUserID == agencyUserID
                                                && !cond.AgencyInstitution.AGI_IsDeleted && !cond.AGUI_IsDeleted)
                                            .Select(col => col.AgencyInstitution)
                                            .ToList();

            #endregion
        }

        List<Entity.SharedDataEntity.InvitationSharedInfoMapping> IProfileSharingRepository.GetInvitationSharedInfoTypeByAgencyUserID(int agencyUserID)
        {
            return _sharedDataDBContext.InvitationSharedInfoMappings.Where(x => x.ISIM_AgencyUserID == agencyUserID && !x.ISIM_IsDeleted).ToList();
            //.Select(cond => cond.ISIM_InvitationSharedInfoTypeID).ToList();
        }

        AgencyUser IProfileSharingRepository.IsEmailAlreadyExistAgencyUser(string email)
        {
            return _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_Email.ToLower() == email.ToLower() && !cond.AGU_IsDeleted).FirstOrDefault();
        }

        /// <summary>
        /// Get Agency User details
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        AgencyUserContract IProfileSharingRepository.GetAgencyUserDetails(Guid userID)
        {
            AgencyUserContract agencyUserContract = new AgencyUserContract();
            AgencyUser agencyUser = _sharedDataDBContext.AgencyUsers.FirstOrDefault(con => con.AGU_UserID == userID && !con.AGU_IsDeleted);
            if (agencyUser.IsNotNull())
            {
                agencyUserContract.AGU_ID = agencyUser.AGU_ID;
                //agencyUserContract.AGU_AgencyID = agencyUser.AGU_AgencyID;//UAT-1641
                agencyUserContract.LstAGU_AgencyID = agencyUser.UserAgencyMappings.Where(con => con.UAM_IsVerified && !con.UAM_IsDeleted && !con.Agency.AG_IsDeleted)
                                                                                  .Select(col => col.UAM_AgencyID).ToList();
                agencyUserContract.AGU_AgencyUserPermission = agencyUser.AGU_AgencyUserPermission;

                //If master agency user
                if (agencyUser.AGU_AgencyUserPermission)
                {
                    agencyUserContract.AGU_ComplianceSharedInfoTypeID = agencyUser.AGU_ComplianceSharedInfoTypeID;
                    agencyUserContract.AGU_ReqRotationSharedInfoTypeID = agencyUser.AGU_ReqRotationSharedInfoTypeID;
                    //Bkg permissions
                    var lstnvitationSharedInfoMappings = agencyUser.InvitationSharedInfoMappings.Where(x => !x.ISIM_IsDeleted).ToList();
                    agencyUserContract.lstInvitationSharedInfoTypeID = lstnvitationSharedInfoMappings.Select(x => x.ISIM_InvitationSharedInfoTypeID).ToList();
                }
            }
            return agencyUserContract;
        }

        /// <summary>
        /// Update Agency User details
        /// </summary>
        /// <param name="agencyUserDetails"></param>
        /// <param name="userID"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        Boolean IProfileSharingRepository.UpdateAgencyUserDetails(AgencyUserContract agencyUserDetails, Guid userID, Int32 currentUserID)
        {
            AgencyUser agencyUser = _sharedDataDBContext.AgencyUsers.FirstOrDefault(con => con.AGU_UserID == userID && !con.AGU_IsDeleted);
            //agencyUser.AGU_Name = agencyUserDetails.AGU_Name;

            //If master agency user then update permissions
            if (agencyUser.AGU_AgencyUserPermission)
            {
                agencyUser.AGU_ComplianceSharedInfoTypeID = agencyUserDetails.AGU_ComplianceSharedInfoTypeID;
                agencyUser.AGU_ReqRotationSharedInfoTypeID = agencyUserDetails.AGU_ReqRotationSharedInfoTypeID;
                agencyUser.AGU_ModifiedByID = currentUserID;
                agencyUser.AGU_ModifiedOn = DateTime.Now;

                //Update Agency User background check permissions
                List<Entity.SharedDataEntity.InvitationSharedInfoMapping> lstInvitationSharedInfoMapping = _sharedDataDBContext.InvitationSharedInfoMappings.Where(x => x.ISIM_AgencyUserID == agencyUser.AGU_ID && x.ISIM_IsDeleted == false).ToList();
                if (lstInvitationSharedInfoMapping.IsNotNull())
                {
                    //Delete Old Data
                    foreach (var invitationSharedInfoMapping in lstInvitationSharedInfoMapping)
                    {
                        invitationSharedInfoMapping.ISIM_IsDeleted = true;
                        invitationSharedInfoMapping.ISIM_ModifiedByID = currentUserID;
                        invitationSharedInfoMapping.ISIM_ModifiedOn = DateTime.Now;
                    }

                    //Add New Data
                    foreach (var invitationSharedInfoTypeID in agencyUserDetails.lstInvitationSharedInfoTypeID)
                    {
                        Entity.SharedDataEntity.InvitationSharedInfoMapping invitationSharedInfoMapping = new Entity.SharedDataEntity.InvitationSharedInfoMapping();
                        invitationSharedInfoMapping.ISIM_AgencyUserID = agencyUser.AGU_ID;
                        invitationSharedInfoMapping.ISIM_InvitationSharedInfoTypeID = invitationSharedInfoTypeID;
                        invitationSharedInfoMapping.ISIM_IsDeleted = false;
                        invitationSharedInfoMapping.ISIM_CreatedByID = currentUserID;
                        invitationSharedInfoMapping.ISIM_CreatedOn = DateTime.Now;

                        _sharedDataDBContext.AddToInvitationSharedInfoMappings(invitationSharedInfoMapping);
                    }
                }
            }

            if (_sharedDataDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        List<String> IProfileSharingRepository.GetAgencyByAgencyUserID(Int32 agencyUserID)
        {
            return _sharedDataDBContext.UserAgencyMappings.Where(con => con.UAM_AgencyUserID == agencyUserID && !con.UAM_IsDeleted).Select(sel => sel.Agency.AG_Name).Distinct().ToList();
        }

        #endregion

        #region AGENCY SHARING
        //public DataTable GetDataForAgencySharing(SearchItemDataContract searchDataContract, CustomPagingArgsContract customPagingArgsContract)
        //{
        //    EntityConnection connection = _dbContext.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand("usp_GetDataForAgencySharing", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@dataXML", searchDataContract.CreateXml());
        //        //command.Parameters.AddWithValue("@CustomAtrributesData", searchDataContract.CustomFields);
        //        command.Parameters.AddWithValue("@customFilteringXml", customPagingArgsContract.CreateXml());
        //        SqlDataAdapter adp = new SqlDataAdapter();
        //        adp.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        adp.Fill(ds);
        //        if (ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                //customPagingArgsContract.CurrentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
        //                customPagingArgsContract.VirtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["TotalCount"]));
        //            }
        //            return ds.Tables[0];
        //        }
        //    }
        //    return new DataTable();
        //}

        //public Int32 SaveImmunizationSnapshot(Int32 currentUserID, Int32 packageSubscrptionID)
        //{
        //    return _dbContext.SaveImmunizationSnapshot(packageSubscrptionID, currentUserID).FirstOrDefault() ?? AppConsts.NONE;
        //}

        #endregion

        #region Immunization Data For Snapshot
        ///// <summary>
        ///// Get Immuniztion Data From snapshot.
        ///// </summary>  
        ///// <returns></returns>
        //DataSet IProfileSharingRepository.GetImmunizationDataFromSnapshot(Int32 snapshotId)
        //{
        //    EntityConnection connection = _dbContext.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand("usp_GetImmunizationDataFromSnapshot", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@SnapshotID", snapshotId);
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        if (ds.Tables.Count > 0)
        //        {
        //            return ds;
        //        }
        //        return new DataSet();
        //    }
        //}

        ///// <summary>
        ///// To get Applicant documents From snapshot
        ///// </summary>
        ///// <param name="sharedcategoryids"></param>
        ///// <param name="snapshotId"></param>
        ///// <returns></returns>
        //public DataTable GetApplicantDocumentsFromSnapshot(String sharedcategoryids, Int32 snapshotId)
        //{
        //    EntityConnection connection = _dbContext.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand("usp_GetApplicantDocumentsFromSnapshot", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@categoryIds", sharedcategoryids);
        //        command.Parameters.AddWithValue("@snapshotId", snapshotId);
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        if (ds.Tables.Count > 0)
        //        {
        //            return ds.Tables[0];
        //        }
        //        return new DataTable();
        //    }
        //}
        #endregion

        #region Check For Tenants Need To Be Disable
        List<int> IProfileSharingRepository.CheckTenantsNeedToDisable(int AgencyID)
        {
            //List<Int32> lstAgencyUserIDs = _sharedDataDBContext.AgencyUsers.Where(x => x.AGU_AgencyID == AgencyID && !x.AGU_IsDeleted).Select(x => x.AGU_ID).ToList();
            //List<Int32> lstAgencyInstitutionID = _sharedDataDBContext.AgencyUserInstitutions.Where(x => lstAgencyUserIDs.Contains(x.AGUI_AgencyUserID.Value) && !x.AGUI_IsDeleted).Select(x => x.AGUI_AgencyInstitutionID.Value).ToList();
            //return _sharedDataDBContext.AgencyInstitutions.Where(x => lstAgencyInstitutionID.Contains(x.AGI_ID) && !x.AGI_IsDeleted).Select(x => x.AGI_TenantID.Value).ToList();

            #region Changes corresponding to UAT-1641:As an Agency User, I should be able to be linked to multiple agencies

            //Step 1 - Get mapping ids of agency and user corresponding to selected agency

            List<Int32> lstUserAgencyMappingIds = _sharedDataDBContext.UserAgencyMappings
                                                        .Where(cond => cond.UAM_AgencyID == AgencyID && !cond.UAM_IsDeleted)
                                                        .Select(col => col.UAM_ID)
                                                        .ToList();

            //Step 2 - Get mapping ids of agency and institutions for above fetched list

            List<Int32> lstAgencyInstitutionIDs = _sharedDataDBContext.AgencyUserInstitutions
                                                            .Where(cond => lstUserAgencyMappingIds.Contains(cond.AGUI_UserAgencyMappingID) && !cond.AGUI_IsDeleted)
                                                            .Select(col => col.AGUI_AgencyInstitutionID.Value)
                                                            .ToList();

            //Step 3 - Get institution ids corresponding to above fetched list

            return _sharedDataDBContext.AgencyInstitutions
                        .Where(cond => lstAgencyInstitutionIDs.Contains(cond.AGI_ID) && !cond.AGI_IsDeleted)
                        .Select(col => col.AGI_TenantID.Value)
                        .ToList();

            #endregion

        }
        #endregion

        #region Profile Sharing Methods Copied from Security Repo

        #region Profile Sharing

        /// <summary>
        /// Gets the list of invitations that has been sent by the applicant
        /// </summary>
        /// <param name="applicantOrgUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        DataTable IProfileSharingRepository.GetApplicantInvitations(Int32 applicantOrgUserId, Int32 tenantId, Int32? isAgnecyShareForAdmin)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetApplicantInvitations", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicantOrgUserId", applicantOrgUserId);
                command.Parameters.AddWithValue("@TenantId", tenantId);
                command.Parameters.AddWithValue("@IsAgnecyShareForAdmin", isAgnecyShareForAdmin);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Method to check whether shared user has already account or not(RG)
        /// </summary>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        Entity.OrganizationUser IProfileSharingRepository.IsSharedUserExists(Guid token, Boolean isProfileSharingToken, Int32? agencyUserID)
        {
            if (agencyUserID != null && agencyUserID > AppConsts.NONE)
            {
                Guid agencyUserUserID = _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_ID == agencyUserID && !cond.AGU_IsDeleted).Select(x => x.AGU_UserID).FirstOrDefault() ?? new Guid();
                if (agencyUserUserID != Guid.Empty)
                {
                    Entity.OrganizationUser orgUser = base.Context.OrganizationUsers.Where(cond => cond.UserID == agencyUserUserID && cond.IsSharedUser == true && !cond.IsDeleted).FirstOrDefault(); //UAT-1218
                    if (!orgUser.IsNullOrEmpty())
                        return orgUser;
                }
            }
            else if (isProfileSharingToken) // Check if Invitation Token is recieved
            {
                Int32 inviteeOrgUserID = _sharedDataDBContext.ProfileSharingInvitations.Where(cond => cond.PSI_Token == token && !cond.PSI_IsDeleted).Select(x => x.PSI_InviteeOrgUserID).FirstOrDefault() ?? 0;
                if (inviteeOrgUserID > 0)
                {
                    List<Guid> lstUserIds = base.Context.OrganizationUsers.Where(cond => cond.OrganizationUserID == inviteeOrgUserID && !cond.IsDeleted).Select(col => col.UserID).ToList(); //UAT-1218
                    //Boolean isInviteeOrgUserIDOfSharedUser = base.Context.OrganizationUsers.Any(cond => lstUserIds.Contains(cond.UserID) && cond.IsSharedUser == true); //UAT-1218
                    //if (isInviteeOrgUserIDOfSharedUser)
                    //    return true;
                    Entity.OrganizationUser orgUser = base.Context.OrganizationUsers.Where(cond => lstUserIds.Contains(cond.UserID) && cond.IsSharedUser == true && !cond.IsDeleted).FirstOrDefault(); //UAT-1218
                    if (!orgUser.IsNullOrEmpty())
                        return orgUser;
                }
            }
            else //UAT-1218 When ClientContact Token is recieved
            {
                Guid clientContactUserID = _sharedDataDBContext.ClientContacts.Where(cond => cond.CC_TokenID == token && !cond.CC_IsDeleted).Select(x => x.CC_UserID).FirstOrDefault() ?? new Guid();
                if (!clientContactUserID.IsNullOrEmpty())
                {
                    //Boolean isUserIDOfSharedUser = base.Context.OrganizationUsers.Any(cond => cond.UserID == clientContactUserID && cond.IsSharedUser == true); //UAT-1218
                    //if (isUserIDOfSharedUser)
                    //    return true;
                    Entity.OrganizationUser orgUser = base.Context.OrganizationUsers.Where(cond => cond.UserID == clientContactUserID && cond.IsSharedUser == true && !cond.IsDeleted).FirstOrDefault(); //UAT-1218
                    if (!orgUser.IsNullOrEmpty())
                        return orgUser;
                }
            }
            return null;
        }

        /// <summary>
        /// Method to get Shared User Data from Invitation Sent by applicant(currently only Email)(RG)
        /// </summary>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        String IProfileSharingRepository.GetSharedUserDataFromInvitation(Guid token, Boolean isProfileSharingToken, Int32? agencyUserID)
        {
            if (agencyUserID != null && agencyUserID > AppConsts.NONE)
            {
                return _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_ID == agencyUserID && !cond.AGU_IsDeleted).FirstOrDefault().AGU_Email;
            }
            else if (isProfileSharingToken) //If Profile Sharing Token recieved
            {
                return _sharedDataDBContext.ProfileSharingInvitations.Where(cond => cond.PSI_Token == token && !cond.PSI_IsDeleted).Select(x => x.PSI_InviteeEmail).FirstOrDefault();
            }
            else//If Client Contact Token recieved
            {
                return _sharedDataDBContext.ClientContacts.Where(cond => cond.CC_TokenID == token && !cond.CC_IsDeleted).Select(x => x.CC_Email).FirstOrDefault();
            }
        }

        /// <summary>
        /// Method to Update Invitee Organization UserID in ProfileSharingInvitation and AgencyUser table
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        //Boolean IProfileSharingRepository.UpdateInviteeOrganizationUserID(Int32 orgUserID, Guid inviteToken, Int32 adminInitializedStatusID, Guid inviteeUserID, Int32? agencyUserID)
        //{
        //    if (!inviteToken.IsNullOrEmpty())
        //    {
        //        //Updating invitee OrganizationUserID in ProfileSharingInvitation for all invitation sent to that invitee.
        //        ProfileSharingInvitation tempIinvitation = _sharedDataDBContext.ProfileSharingInvitations.Where(cond => cond.PSI_Token == inviteToken && !cond.PSI_IsDeleted).FirstOrDefault();

        //        List<ProfileSharingInvitation> lstProfileSharingInvitation = _sharedDataDBContext.ProfileSharingInvitations
        //                                                                                .Where(cond => cond.PSI_InviteeEmail.ToLower() == tempIinvitation.PSI_InviteeEmail.ToLower()
        //                                                                                    && cond.PSI_InvitationStatusID != adminInitializedStatusID
        //                                                                                    && !cond.PSI_IsDeleted).ToList();
        //        foreach (var invitation in lstProfileSharingInvitation)
        //        {
        //            if (invitation.PSI_InviteeOrgUserID.IsNull())
        //            {
        //                invitation.PSI_InviteeOrgUserID = orgUserID;
        //            }
        //        }

        //        //Updating userid in AgencyUser if invitation sent by admin or client admin
        //        if (tempIinvitation.lkpInvitationSource.Code == InvitationSourceTypes.ADMIN.GetStringValue() || tempIinvitation.lkpInvitationSource.Code == InvitationSourceTypes.CLIENTADMIN.GetStringValue())
        //        {
        //            //Below line is commented because now inviteeUserID will be get from method parameter
        //            //Guid inviteeUserID = _sharedDataDBContext.OrganizationUsers.Where(cond => cond.OrganizationUserID == orgUserID).Select(x => x.UserID).FirstOrDefault();
        //            if (!inviteeUserID.IsNullOrEmpty())
        //            {
        //                //getting object of AgencyUser basis on EmailID 
        //                AgencyUser agencyUser = _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_Email == tempIinvitation.PSI_InviteeEmail && !cond.AGU_IsDeleted).FirstOrDefault();
        //                if (agencyUser.IsNotNull())
        //                {
        //                    agencyUser.AGU_UserID = inviteeUserID;
        //                    agencyUser.AGU_ModifiedByID = orgUserID;
        //                    agencyUser.AGU_ModifiedOn = DateTime.Now;
        //                }

        //                ClientContact clientContact = _sharedDataDBContext.ClientContacts.Where(cond => cond.CC_Email.ToLower() == tempIinvitation.PSI_InviteeEmail.ToLower()
        //                                                                                          && !cond.CC_IsDeleted).FirstOrDefault();
        //                if (clientContact.IsNotNull())
        //                {
        //                    clientContact.CC_UserID = inviteeUserID;
        //                    clientContact.CC_ModifiedByID = orgUserID;
        //                    clientContact.CC_ModifiedOn = DateTime.Now;
        //                }
        //            }
        //        }

        //        if (_sharedDataDBContext.SaveChanges() > 0)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    return false;
        //}


        Boolean IProfileSharingRepository.UpdateInviteeOrganizationUserID(Int32 orgUserID, Guid inviteToken, Int32 adminInitializedStatusID, Guid inviteeUserID, Int32? agencyUserID, out String profileSharingInvitationIds)
        {
            String inviteeEmail = String.Empty;
            ProfileSharingInvitation tempIinvitation = null;
            if (inviteToken != Guid.Empty)
            {
                //Updating invitee OrganizationUserID in ProfileSharingInvitation for all invitation sent to that invitee.
                tempIinvitation = _sharedDataDBContext.ProfileSharingInvitations.Where(cond => cond.PSI_Token == inviteToken && !cond.PSI_IsDeleted).FirstOrDefault();
                inviteeEmail = tempIinvitation.PSI_InviteeEmail;
            }
            else if (agencyUserID != null && agencyUserID > AppConsts.NONE)
            {
                inviteeEmail = _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_ID == agencyUserID && !cond.AGU_IsDeleted).FirstOrDefault().AGU_Email;
            }

            UpdateInviteeOrganizationUserID(orgUserID, adminInitializedStatusID, inviteeEmail, out profileSharingInvitationIds);


            if (inviteToken == Guid.Empty)
            {
                UpdateInviteeUserID(orgUserID, inviteeUserID, inviteeEmail);
            }
            else
            {
                if (tempIinvitation.lkpInvitationSource.Code == InvitationSourceTypes.ADMIN.GetStringValue() || tempIinvitation.lkpInvitationSource.Code == InvitationSourceTypes.CLIENTADMIN.GetStringValue())
                {
                    UpdateInviteeUserID(orgUserID, inviteeUserID, inviteeEmail);
                }
            }

            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        private void UpdateInviteeOrganizationUserID(Int32 orgUserID, Int32 adminInitializedStatusID, String inviteeEmail, out String profileSharingInvitationIds)
        {
            List<ProfileSharingInvitation> lstProfileSharingInvitation = _sharedDataDBContext.ProfileSharingInvitations
                                                                                    .Where(cond => cond.PSI_InviteeEmail.ToLower() == inviteeEmail.ToLower()
                                                                                        && cond.PSI_InvitationStatusID != adminInitializedStatusID
                                                                                        && !cond.PSI_IsDeleted).ToList();
            //UAT-2452
            List<Int32> lstProfileShareInvitationIds = lstProfileSharingInvitation.Select(sel => sel.PSI_ID).ToList();
            List<SharedUserInvitationReview> lstSharedUserInvitationReview = _sharedDataDBContext.SharedUserInvitationReviews
                                                                                .Where(cond => lstProfileShareInvitationIds.Contains(cond.SUIR_ProfileSharingInvitationID)
                                                                                    && !cond.SUIR_IsDeleted && cond.SUIR_OrganizationUserID == AppConsts.NONE).ToList();

            foreach (ProfileSharingInvitation invitation in lstProfileSharingInvitation)
            {
                if (invitation.PSI_InviteeOrgUserID.IsNull())
                {
                    invitation.PSI_InviteeOrgUserID = orgUserID;
                }
            }

            List<Int32> lstProfileSharingInvitationIds = _sharedDataDBContext.ProfileSharingInvitations
                                                                                    .Where(cond => cond.PSI_InviteeEmail.ToLower() == inviteeEmail.ToLower()
                                                                                        && (!cond.ProfileSharingInvitationGroup.PSIG_IsIndividualShare.Value || !cond.ProfileSharingInvitationGroup.PSIG_IsIndividualShare == null)
                                                                                        && !cond.PSI_IsDeleted).Select(d => d.PSI_ID).ToList();

            profileSharingInvitationIds = String.Join(",", lstProfileSharingInvitationIds); //UAT-3400

            lstSharedUserInvitationReview.ForEach(sharedUserInvitationReview =>
            {
                sharedUserInvitationReview.SUIR_OrganizationUserID = orgUserID;
            });
        }

        private void UpdateInviteeUserID(Int32 orgUserID, Guid inviteeUserID, String inviteeEmail)
        {
            if (!inviteeUserID.IsNullOrEmpty())
            {
                //getting object of AgencyUser basis on EmailID 
                AgencyUser agencyUser = _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_Email == inviteeEmail && !cond.AGU_IsDeleted).FirstOrDefault();
                if (agencyUser.IsNotNull())
                {
                    agencyUser.AGU_UserID = inviteeUserID;
                    agencyUser.AGU_ModifiedByID = orgUserID;
                    agencyUser.AGU_ModifiedOn = DateTime.Now;
                    List<UserAgencyMapping> lstUserAgecyMapping = agencyUser.UserAgencyMappings.Where(con => !con.UAM_IsVerified && !con.UAM_IsDeleted).ToList();
                    foreach (UserAgencyMapping agencyMapping in lstUserAgecyMapping)
                    {
                        agencyMapping.UAM_IsVerified = true;
                    }
                }

                ClientContact clientContact = _sharedDataDBContext.ClientContacts.Where(cond => cond.CC_Email.ToLower() == inviteeEmail
                                                                                          && !cond.CC_IsDeleted).FirstOrDefault();
                if (clientContact.IsNotNull())
                {
                    clientContact.CC_UserID = inviteeUserID;
                    clientContact.CC_ModifiedByID = orgUserID;
                    clientContact.CC_ModifiedOn = DateTime.Now;
                }
            }
        }


        /// <summary>
        /// Save the New Invitation and return the ID of the invitation generated.
        /// </summary>
        /// <param name="invitationDetails"></param>
        /// <returns>Tuple with InvitationID & its related Token</returns>
        Tuple<Int32, Guid> IProfileSharingRepository.SaveProfileSharingInvitation(InvitationDetailsContract invitationDetails, Int32 generatedInvitationGroupID, Int32 invGroupTypeID)
        {
            var _invitationGroup = new ProfileSharingInvitationGroup();

            if (generatedInvitationGroupID == 0) //invitation sent by applicant
            {
                _invitationGroup.PSIG_AgencyID = invitationDetails.AgencyId.IsNotNull() ? invitationDetails.AgencyId : (Int32?)null;
                //_invitationGroup.PSIG_InvitationInitiatedByID = invitationDetails.CurrentUserId;
                _invitationGroup.PSIG_InvitationInitiatedByID = invitationDetails.ApplicantId;
                _invitationGroup.PSIG_TenantID = invitationDetails.TenantID;
                _invitationGroup.PSIG_IsDeleted = false;
                //_invitationGroup.PSIG_CreatedByID = invitationDetails.CurrentUserId;
                _invitationGroup.PSIG_CreatedByID = invitationDetails.CurrentUserId;
                _invitationGroup.PSIG_CreatedOn = invitationDetails.CurrentDateTime;
                _invitationGroup.PSIG_ProfileSharingInvitationGroupTypeID = invGroupTypeID;
                _invitationGroup.PSIG_IsIndividualShare = true;

                #region Rotation Details
                if (!invitationDetails.RotationDetail.IsNullOrEmpty())
                {
                    ProfileSharingInvitationRotationDetail invitationRotationDetail = new ProfileSharingInvitationRotationDetail();
                    invitationRotationDetail.PSIRD_RotationName = invitationDetails.RotationDetail.RotationName;
                    invitationRotationDetail.PSIRD_TypeSpecialty = invitationDetails.RotationDetail.TypeSpecialty;
                    invitationRotationDetail.PSIRD_Department = invitationDetails.RotationDetail.Department;
                    invitationRotationDetail.PSIRD_Program = invitationDetails.RotationDetail.Program;
                    invitationRotationDetail.PSIRD_Course = invitationDetails.RotationDetail.Course;
                    invitationRotationDetail.PSIRD_Term = invitationDetails.RotationDetail.Term;
                    invitationRotationDetail.PSIRD_UnitFloor = invitationDetails.RotationDetail.UnitFloorLoc;
                    invitationRotationDetail.PSIRD_Shift = invitationDetails.RotationDetail.Shift;
                    invitationRotationDetail.PSIRD_StartTime = invitationDetails.RotationDetail.StartTime;
                    invitationRotationDetail.PSIRD_EndTime = invitationDetails.RotationDetail.EndTime;
                    invitationRotationDetail.PSIRD_StartDate = invitationDetails.RotationDetail.StartDate;
                    invitationRotationDetail.PSIRD_EndDate = invitationDetails.RotationDetail.EndDate;
                    invitationRotationDetail.PSIRD_IsDeleted = false;
                    invitationRotationDetail.PSIRD_CreatedByID = invitationDetails.CurrentUserId;
                    invitationRotationDetail.PSIRD_CreatedOn = invitationDetails.CurrentDateTime;

                    List<Int32> daysToBeMapped = new List<Int32>();
                    if (!invitationDetails.RotationDetail.DaysIdList.IsNullOrEmpty())
                    {
                        daysToBeMapped = invitationDetails.RotationDetail.DaysIdList.Split(',').Select(int.Parse).ToList();
                        foreach (Int32 day in daysToBeMapped)
                        {
                            ProfileSharingInvitationRotationDay newDay = new ProfileSharingInvitationRotationDay();
                            newDay.PSIRDY_WeekDayID = day;
                            newDay.PSIRDY_IsDeleted = false;
                            newDay.PSIRDY_CreatedByID = invitationDetails.CurrentUserId;
                            newDay.PSIRDY_CreatedOn = invitationDetails.CurrentDateTime;
                            invitationRotationDetail.ProfileSharingInvitationRotationDays.Add(newDay);
                        }
                    }
                    _invitationGroup.ProfileSharingInvitationRotationDetails.Add(invitationRotationDetail);
                }

                #endregion
            }

            var _invitation = new ProfileSharingInvitation();
            var _token = Guid.NewGuid();

            if (generatedInvitationGroupID == 0)//invitation sent by applicant
            {
                _invitation.ProfileSharingInvitationGroup = _invitationGroup;
            }
            else//invitation sent by admin/client admin
            {
                _invitation.PSI_ProfileSharingInvitationGroupID = generatedInvitationGroupID;
            }
            _invitation.PSI_InviteeName = invitationDetails.Name;
            _invitation.PSI_InviteeEmail = invitationDetails.EmailAddress;
            _invitation.PSI_InviteePhone = invitationDetails.Phone;
            //UAT-2447
            _invitation.PSI_IsInternationalInviteePhone = invitationDetails.IsInternationalPhone;

            _invitation.PSI_InviteeAgency = invitationDetails.Agency;
            _invitation.PSI_InvitationMessage = invitationDetails.CustomMessage;
            _invitation.PSI_ApplicantOrgUserID = invitationDetails.ApplicantId;
            _invitation.PSI_InviteeOrgUserID = invitationDetails.InviteeOrgUserId;
            _invitation.PSI_ExpirationTypeID = invitationDetails.ExpirationTypeId;
            _invitation.PSI_InvitationSourceID = invitationDetails.InvitationSourceId;
            _invitation.PSI_InvitationStatusID = invitationDetails.InvitationStatusId;
            _invitation.PSI_MaxViews = invitationDetails.MaxViews;
            _invitation.PSI_ExpirationDate = invitationDetails.ExpirationDate.IsNotNull() ? invitationDetails.ExpirationDate : (DateTime?)null;
            _invitation.PSI_InvitationDate = invitationDetails.CurrentDateTime;
            _invitation.PSI_CreatedById = invitationDetails.CurrentUserId;
            _invitation.PSI_CreatedOn = invitationDetails.CurrentDateTime;
            _invitation.PSI_IsDeleted = false;
            _invitation.PSI_Token = _token;
            _invitation.PSI_TenantID = invitationDetails.TenantID;
            _invitation.PSI_PreviousInvitationID = (invitationDetails.PreviousPSIId.IsNotNull() && invitationDetails.PreviousPSIId > AppConsts.NONE)
                ? invitationDetails.PreviousPSIId
                : (Int32?)null;

            //_invitation.PSI_InitiatedByID = invitationDetails.CurrentUserId;
            _invitation.PSI_InitiatedByID = invitationDetails.ApplicantId;

            //invitationDetails.InitiatedById.IsNotNull() && invitationDetails.InitiatedById > AppConsts.NONE
            //? invitationDetails.InitiatedById
            //: (Int32?)null;

            _invitation.PSI_AgencyUserID = invitationDetails.AgencyUserId.IsNotNull() ? invitationDetails.AgencyUserId : (Int32?)null;
            _invitation.PSI_InviteeUserTypeID = invitationDetails.InviteeUserTypeID;

            #region UAT-3470
            _invitation.PSI_InvitationArchiveStateID = invitationDetails.InvitationArchiveStateID;
            #endregion

            _sharedDataDBContext.ProfileSharingInvitations.AddObject(_invitation);

            foreach (var amdId in invitationDetails.SharedApplicantMetaDataIds)
            {
                var _applicantMetaData = new ApplicantSharedInvitationMetaData();
                _applicantMetaData.ProfileSharingInvitation = _invitation;
                _applicantMetaData.SIMD_ApplicantInvitationMetaDataID = amdId;
                _applicantMetaData.SIMD_CreatedByID = invitationDetails.CurrentUserId;
                _applicantMetaData.SIMD_CreatedOn = invitationDetails.CurrentDateTime;
                _applicantMetaData.SIMD_IsDeleted = false;
                _sharedDataDBContext.ApplicantSharedInvitationMetaDatas.AddObject(_applicantMetaData);
            }

            _sharedDataDBContext.SaveChanges();

            var _info = new Tuple<Int32, Guid>(_invitation.PSI_ID, _token);
            return _info;
        }

        /// <summary>
        /// Save the Bulk Invitations sent by admin/client admin
        /// </summary>
        /// <param name="lstInvitationDetails"></param>
        /// <param name="invitationGroup"></param>
        /// <param name="generatedInvitationGroupID"></param>
        /// <returns></returns>
        List<ProfileSharingInvitation> IProfileSharingRepository.SaveAdminInvitations(List<InvitationDetailsContract> lstInvitationDetails, ProfileSharingInvitationGroup invitationGroup)
        {
            var _lstInvitations = new List<ProfileSharingInvitation>();
            _sharedDataDBContext.ProfileSharingInvitationGroups.AddObject(invitationGroup);

            foreach (var invitation in lstInvitationDetails)
            {
                var _invitation = new ProfileSharingInvitation();
                var _token = Guid.NewGuid();


                _invitation.ProfileSharingInvitationGroup = invitationGroup;

                _invitation.PSI_InviteeName = invitation.Name;
                _invitation.PSI_InviteeEmail = invitation.EmailAddress;
                _invitation.PSI_InviteePhone = invitation.Phone;
                _invitation.PSI_InviteeAgency = invitation.Agency;
                _invitation.PSI_InvitationMessage = invitation.CustomMessage;
                _invitation.PSI_ApplicantOrgUserID = invitation.ApplicantId;
                _invitation.PSI_InviteeOrgUserID = invitation.InviteeOrgUserId;
                _invitation.PSI_ExpirationTypeID = invitation.ExpirationTypeId;
                _invitation.PSI_InvitationSourceID = invitation.InvitationSourceId;
                _invitation.PSI_InvitationStatusID = invitation.InvitationStatusId;
                _invitation.PSI_MaxViews = invitation.MaxViews;
                _invitation.PSI_ExpirationDate = invitation.ExpirationDate.IsNotNull() ? invitation.ExpirationDate : (DateTime?)null;
                _invitation.PSI_InvitationDate = invitation.CurrentDateTime;
                _invitation.PSI_CreatedById = invitation.CurrentUserId;
                _invitation.PSI_CreatedOn = invitation.CurrentDateTime;
                _invitation.PSI_IsDeleted = false;
                _invitation.PSI_Token = _token;
                _invitation.PSI_TenantID = invitation.TenantID;
                _invitation.PSI_PreviousInvitationID = (invitation.PreviousPSIId.IsNotNull() && invitation.PreviousPSIId > AppConsts.NONE)
                    ? invitation.PreviousPSIId
                    : (Int32?)null;

                _invitation.PSI_EffectiveDate = invitation.InvitationScheduleDate;

                _invitation.InvitationIdentifier = invitation.InvitationIdentifier;
                _invitation.PSI_InitiatedByID = invitation.CurrentUserId;

                _invitation.PSI_AgencyUserID = invitation.AgencyUserId.IsNotNull() ? invitation.AgencyUserId : (Int32?)null;
                _invitation.PSI_InviteeUserTypeID = invitation.InviteeUserTypeID;
                #region UAT-3470
                _invitation.PSI_InvitationArchiveStateID = invitation.InvitationArchiveStateID;
                #endregion

                _invitation.PSI_IsInstructorShare = invitation.PSI_IsInstructorShare;//UAT-3977 

                _sharedDataDBContext.ProfileSharingInvitations.AddObject(_invitation);


                if (invitation.SharedApplicantMetaDataIds.IsNotNull())
                {
                    foreach (var amdId in invitation.SharedApplicantMetaDataIds)
                    {
                        var _applicantMetaData = new ApplicantSharedInvitationMetaData();
                        _applicantMetaData.ProfileSharingInvitation = _invitation;
                        _applicantMetaData.SIMD_ApplicantInvitationMetaDataID = amdId;
                        _applicantMetaData.SIMD_CreatedByID = invitation.CurrentUserId;
                        _applicantMetaData.SIMD_CreatedOn = invitation.CurrentDateTime;
                        _applicantMetaData.SIMD_IsDeleted = false;
                        _sharedDataDBContext.ApplicantSharedInvitationMetaDatas.AddObject(_applicantMetaData);
                    }
                }

                _lstInvitations.Add(_invitation);
            }
            _sharedDataDBContext.SaveChanges();
            return _lstInvitations;
        }

        ///// <summary>
        ///// Returns whether the Shared user is being invited
        ///// </summary>
        ///// <param name="emailAddress"></param>
        ///// <returns></returns>
        //Boolean IProfileSharingRepository.IsSharedUserInvited(String emailAddress)
        //{
        //    var _memberShip = _sharedDataDBContext.aspnet_Membership.Where(amu => amu.Email.ToLower() == emailAddress.ToLower()).FirstOrDefault();
        //    if (_memberShip.IsNullOrEmpty())
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        var _orgUser = _sharedDataDBContext.OrganizationUsers.Where(ou => ou.UserID == _memberShip.UserId).First();
        //        if (_orgUser.IsSharedUser.IsNull())
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //}

        /// <summary>
        /// Gets the master details for the selected Invitation
        /// </summary>
        /// <param name="invitationId"></param>
        /// <returns></returns>
        ProfileSharingInvitation IProfileSharingRepository.GetInvitationDetails(Int32 invitationId)
        {
            return _sharedDataDBContext.ProfileSharingInvitations.Include("lkpInvitationExpirationType").Include("ProfileSharingInvitationGroup").Include("AgencyUser")
                 .Where(psi => psi.PSI_ID == invitationId && psi.PSI_IsDeleted == false).First();
        }

        /// <summary>
        /// Update the Status of the Invitation
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        void IProfileSharingRepository.UpdateInvitationStatus(Int32 statusId, Int32 invitationId, Int32 currentUserId)
        {
            var _invitationData = _sharedDataDBContext.ProfileSharingInvitations.Where(psi => psi.PSI_ID == invitationId && psi.PSI_IsDeleted == false).First();
            _invitationData.PSI_InvitationStatusID = statusId;
            _invitationData.PSI_ModifiedOn = DateTime.Now;
            _invitationData.PSI_ModifiedById = currentUserId;
            _sharedDataDBContext.SaveChanges();
        }

        /// <summary>
        /// Update the Status of the Invitation
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        void IProfileSharingRepository.UpdateBulkInvitationStatus(Int32 statusId, List<Int32> invitationId, Int32 currentUserId)
        {
            var _lstInvitations = _sharedDataDBContext.ProfileSharingInvitations.Where(psi => invitationId.Contains(psi.PSI_ID) && psi.PSI_IsDeleted == false).ToList();

            foreach (var _invitationData in _lstInvitations)
            {
                _invitationData.PSI_InvitationStatusID = statusId;
                _invitationData.PSI_ModifiedOn = DateTime.Now;
                _invitationData.PSI_ModifiedById = currentUserId;
            }

            _sharedDataDBContext.SaveChanges();
        }

        /// <summary>
        /// To get invitation data
        /// </summary>
        /// <param name="searchContract"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        DataTable IProfileSharingRepository.GetInvitationData(InvitationSearchContract searchContract, CustomPagingArgsContract gridCustomPaging)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetInvitationSearchData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@xmldata", searchContract.XML);
                command.Parameters.AddWithValue("@filteringSortingData", gridCustomPaging.XML);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }

        /// <summary>
        /// Get All Agencies for an institution
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        List<Agency> IProfileSharingRepository.GetAllAgency(Int32 institutionID)
        {
            List<Agency> lstAgency = _sharedDataDBContext.AgencyInstitutions.Where(cond => cond.AGI_TenantID == institutionID && !cond.AGI_IsDeleted).Select(x => x.Agency).ToList();
            if (lstAgency.IsNotNull())
                return lstAgency;
            return new List<Agency>();
        }

        /// <summary>
        /// Method to Get Agency User Data by Agency ID and InstitutionID
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        List<usp_GetAgencyUserData_Result> IProfileSharingRepository.GetAgencyUserData(Int32 institutionID, Int32 agencyID)
        {
            List<usp_GetAgencyUserData_Result> agencyUserData = _sharedDataDBContext.GetAgencyUserData(institutionID, agencyID).ToList();
            if (agencyUserData.IsNotNull())
            {
                return agencyUserData;
            }
            return new List<usp_GetAgencyUserData_Result>();
        }

        /// <summary>
        /// Get Invitations based upon PSI_InviteeOrgUserID
        /// </summary>
        /// <param name="inviteeOrgUserID"></param>
        /// <returns></returns>
        IEnumerable<ProfileSharingInvitation> IProfileSharingRepository.GetInvitationsByInviteeOrgUserID(Int32 inviteeOrgUserID)
        {
            return _sharedDataDBContext.ProfileSharingInvitations.Where(psi => psi.PSI_InviteeOrgUserID == inviteeOrgUserID && psi.PSI_IsDeleted == false);
        }

        /// Update the Views remaining and last viewed of the Invitation
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        Boolean IProfileSharingRepository.UpdateInvitationViewsRemaining(Int32 invitationId, Int32 currentUserId, Int32 expiredInvitationTypeId)
        {
            var _invitationData = _sharedDataDBContext.ProfileSharingInvitations.Where(psi => psi.PSI_ID == invitationId && psi.PSI_IsDeleted == false).FirstOrDefault();
            if (_invitationData.IsNotNull() && _invitationData.PSI_MaxViews > _invitationData.PSI_InviteeViewCount)
            {
                _invitationData.PSI_InviteeLastViewed = DateTime.Now;
                _invitationData.PSI_InviteeViewCount = _invitationData.PSI_InviteeViewCount + AppConsts.ONE;
                if (_invitationData.PSI_MaxViews == _invitationData.PSI_InviteeViewCount)
                    _invitationData.PSI_InvitationStatusID = expiredInvitationTypeId;
                _invitationData.PSI_ModifiedOn = DateTime.Now;
                _invitationData.PSI_ModifiedById = currentUserId;
                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;
        }

        /// Update Notes of the Invitation
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        Boolean IProfileSharingRepository.UpdateInvitationNotes(Int32 invitationId, Int32 currentUserId, String notes, List<lkpAuditChangeType> lstAuditChangeType)
        {
            var _invitationData = _sharedDataDBContext.ProfileSharingInvitations.Where(psi => psi.PSI_ID == invitationId && psi.PSI_IsDeleted == false).FirstOrDefault();

            if (_invitationData.IsNotNull())
            {
                #region UAT-2511
                List<AgencyUserAuditHistoryDataContract> lstAgencyUserAuditHistory = new List<AgencyUserAuditHistoryDataContract>();
                List<ProfileSharingInvitation> lstProfileSharingInvitation = new List<ProfileSharingInvitation>();
                lstProfileSharingInvitation.Add(_invitationData);

                lstAgencyUserAuditHistory.Add(GenerateAuditHistoryDataContract(null, invitationId, lstProfileSharingInvitation, AppConsts.NONE, currentUserId
                                                                               , AuditType.INVITATION_DETAIL.GetStringValue(), notes, lstAuditChangeType, AppConsts.NONE, String.Empty));

                SaveAgencyUserAuditHistory(lstAgencyUserAuditHistory, false);
                #endregion

                _invitationData.PSI_InviteeNotes = notes;
                _invitationData.PSI_ModifiedOn = DateTime.Now;
                _invitationData.PSI_ModifiedById = currentUserId;
                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Method to genarate New Invitation Group
        /// </summary>
        /// <param name="agencyID"></param>
        /// <param name="initiatedByID"></param>
        /// <returns></returns>
        Int32 IProfileSharingRepository.GenarateNewInvitationGroup(ProfileSharingInvitationGroup invitationGroupObj)
        {
            _sharedDataDBContext.AddToProfileSharingInvitationGroups(invitationGroupObj);
            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                return invitationGroupObj.PSIG_ID;
            }
            return 0;
        }

        DataTable IProfileSharingRepository.GetAttestationDocumentData(String clientInvitationIDs)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetAttestationDocuments", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvitationIDs", clientInvitationIDs);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }
        #region Profile Sharing Attestion Document
        InvitationDocument IProfileSharingRepository.GetInvitationDocuments(Int32 invitationId, String attestationTypeCode)
        {
            InvitationDocumentMapping invDocMappObj = _sharedDataDBContext.InvitationDocumentMappings.Where(cond => cond.IDM_ProfileSharingInvitationID == invitationId
                                                                     && cond.InvitationDocument.IND_IsDeleted == false && cond.IDM_IsDeleted == false
                                                                     && cond.InvitationDocument.lkpSharedSystemDocType.SSDT_Code == attestationTypeCode).FirstOrDefault();
            if (invDocMappObj.IsNotNull())
            {
                return invDocMappObj.InvitationDocument;
            }
            return null;
        }
        #endregion

        #region Attestation Document Code

        //Int32 IProfileSharingRepository.SaveAttestationDocument(String pdfDocPath, Int32 documentTypeId, Int32 currentLoggedInUserID)
        //{
        //    if (!String.IsNullOrEmpty(pdfDocPath))
        //    {
        //        //Below Code is commented and docuemntTypeid now will be get by parameter.
        //        //Int32 documentTypeId = _sharedDataDBContext.lkpDocumentTypes.Where(cond => cond.DT_Code == documentTypeCode).FirstOrDefault().DT_ID;
        //        InvitationDocument invitationDocument = new InvitationDocument()
        //        {
        //            IND_DocumentFilePath = pdfDocPath,
        //            IND_DocumentType = documentTypeId,
        //            IND_IsDeleted = false,
        //            IND_CreatedByID = currentLoggedInUserID,
        //            IND_CreatedOn = DateTime.Now
        //        };

        //        _sharedDataDBContext.InvitationDocuments.AddObject(invitationDocument);
        //        if (_sharedDataDBContext.SaveChanges() > 0)
        //        {
        //            return invitationDocument.IND_ID;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //    return 0;
        //}


        /// <summary>
        /// UAT-2443:Attestation Merge and multiple share behavior changes
        /// </summary>
        /// <param name="selectedRotationId"></param>
        /// <returns></returns>
        List<InvitationSharedInfoDetails> IProfileSharingRepository.GetInvitationDocumentData(Int32 selectedRotationId, Int32 tenantID, Int32 agencyID)
        {
            List<AttestationDocumentContract> AttestationDoc = new List<AttestationDocumentContract>();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_Get_AttestationDocumentDetails", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RotationID", selectedRotationId);
                command.Parameters.AddWithValue("@tenantID", tenantID);
                command.Parameters.AddWithValue("@AgencyID", agencyID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AttestationDoc.Add(new AttestationDocumentContract()
                        {
                            InvitationDocumentID = Convert.ToInt32(dr["InvitationDocumentID"]),
                            DocumentFilePath = Convert.ToString(dr["DocumentFilePath"]),
                            SharedInfoTypeCode = Convert.ToString(dr["SharedInfoTypeCode"]),
                            MasterInfoTypeCode = Convert.ToString(dr["MasterInfoTypeCode"])
                        });
                    }
                }
            }

            List<InvitationSharedInfoDetails> lstInvitationDoc = new List<InvitationSharedInfoDetails>();

            foreach (var item in AttestationDoc.DistinctBy(con => con.InvitationDocumentID))
            {
                InvitationSharedInfoDetails tempInvitationDoc = new InvitationSharedInfoDetails();
                List<AttestationDocumentContract> tempdoc = AttestationDoc.Where(con => con.InvitationDocumentID == item.InvitationDocumentID).ToList();

                List<String> TrackingPermission = tempdoc.Where(con => con.MasterInfoTypeCode == "IMM").Select(con => con.SharedInfoTypeCode).ToList();
                List<String> ScreeningPermission = tempdoc.Where(con => con.MasterInfoTypeCode == "BKG").Select(con => con.SharedInfoTypeCode).ToList();
                List<String> RotationPermission = tempdoc.Where(con => con.MasterInfoTypeCode == "REQROT").Select(con => con.SharedInfoTypeCode).ToList();

                tempInvitationDoc.ComplianceSharedInfoTypeCode = TrackingPermission.Where(con => con == "AAAC").FirstOrDefault();
                tempInvitationDoc.ReqRotSharedInfoTypeCode = RotationPermission.Where(con => con == "AAAJ").FirstOrDefault();
                tempInvitationDoc.LstBkgSharedInfoTypeCode = ScreeningPermission;
                tempInvitationDoc.InvitationDocumentID = item.InvitationDocumentID;
                tempInvitationDoc.DocumentPath = item.DocumentFilePath;
                tempInvitationDoc.IsForEveryOneAttestationForm = tempdoc.Any(con => con.MasterInfoTypeCode == "EVERYONE");
                lstInvitationDoc.Add(tempInvitationDoc);
            }
            return lstInvitationDoc;
        }

        Boolean IProfileSharingRepository.SaveInvitationDocumentMapping(List<InvitationDocumentMapping> lstInvitationDocumentMapping)
        {
            foreach (InvitationDocumentMapping newInvitationDocumentMapping in lstInvitationDocumentMapping)
            {
                _sharedDataDBContext.InvitationDocumentMappings.AddObject(newInvitationDocumentMapping);
            }
            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }



        Boolean IProfileSharingRepository.SaveInvAttestationDocWithPermissionType(List<InvAttestationDocWithPermissionType> lstInvAttestationDocWithPermissionType)
        {
            foreach (InvAttestationDocWithPermissionType newInvAttestationDocWithPermissionType in lstInvAttestationDocWithPermissionType)
            {
                if (newInvAttestationDocWithPermissionType.IADWPT_ID == AppConsts.NONE)
                    _sharedDataDBContext.InvAttestationDocWithPermissionTypes.AddObject(newInvAttestationDocWithPermissionType);
            }

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to Get Invitation Data by Invite Token
        /// </summary>
        /// <param name="inviteToken"></param>
        /// <returns></returns>
        ProfileSharingInvitation IProfileSharingRepository.GetInvitationDataByToken(Guid inviteToken)
        {
            ProfileSharingInvitation invitation = _sharedDataDBContext.ProfileSharingInvitations.Where(cond => cond.PSI_Token == inviteToken && !cond.PSI_IsDeleted).FirstOrDefault();
            if (invitation.IsNotNull())
            {
                return invitation;
            }
            return new ProfileSharingInvitation();
        }

        #endregion

        #region UAT-1201 as a client admin, I should be able to view the attestations for any profile shares that I have sent.
        List<ProfileSharingInvitationGroupContract> IProfileSharingRepository.GetAttestationDetailsData(Int32 clientID, Int32 currentUserID, Int32 adminInitializedInvitationStatus)
        {
            #region Commented code
            //List<ProfileSharingInvitationGroup> invitationGrupDetails = _sharedDataDBContext.ProfileSharingInvitationGroups.Where(cond => cond.PSIG_TenantID == clientID
            //                                                                               && cond.PSIG_InvitationInitiatedByID == currentUserID
            //                                                                               && !cond.PSIG_IsDeleted
            //                                                                               && !cond.ProfileSharingInvitations.All(y => y.PSI_InvitationStatusID == adminInitializedInvitationStatus && !y.PSI_IsDeleted)).ToList();

            //return (from s in _sharedDataDBContext.ProfileSharingInvitationGroups
            //        where s.PSIG_TenantID == clientID
            //            && s.PSIG_InvitationInitiatedByID == currentUserID
            //            && !s.PSIG_IsDeleted
            //            && !s.ProfileSharingInvitations.All(y => y.PSI_InvitationStatusID == adminInitializedInvitationStatus && !y.PSI_IsDeleted)
            //        select new ProfileSharingInvitationGroupContract
            //        {
            //            PSIG_ID = s.PSIG_ID,
            //            PSIG_AdminName = s.PSIG_AdminName,
            //            PSIG_AgencyID = s.PSIG_AgencyID,
            //            PSIG_AgencyName = s.Agency.AG_Name,
            //            PSIG_AssignedUnits = s.PSIG_AssignedUnits,
            //            PSIG_AttestationDate = s.PSIG_AttestationDate,
            //            PSIG_AttestationReportText = s.PSIG_AttestationReportText,
            //            PSIG_ClinicalFromDate = s.PSIG_ClinicalFromDate,
            //            PSIG_ClinicalRotationID = s.PSIG_ClinicalRotationID,
            //            PSIG_ClinicalToDate = s.PSIG_ClinicalToDate,
            //            PSIG_CreatedByID = s.PSIG_CreatedByID,
            //            PSIG_CreatedOn = s.PSIG_CreatedOn,
            //            PSIG_InvitationInitiatedByID = s.PSIG_InvitationInitiatedByID,
            //            PSIG_IsDeleted = s.PSIG_IsDeleted,
            //            PSIG_ModifiedByID = s.PSIG_ModifiedByID,
            //            PSIG_ModifiedOn = s.PSIG_ModifiedOn,
            //            PSIG_ProfileSharingInvitationGroupTypeID = s.PSIG_ProfileSharingInvitationGroupTypeID,
            //            PSIG_ProgramName = s.PSIG_ProgramName,
            //            PSIG_TenantID = s.PSIG_TenantID
            //        }).ToList();
            #endregion

            //UAT-1507 WB: Updates to Attestation Details Grid (UI and new column).
            var AttestationDetailsData = (from s in _sharedDataDBContext.ProfileSharingInvitationGroups
                                          from psi in _sharedDataDBContext.ProfileSharingInvitations.Where(cond => cond.PSI_ProfileSharingInvitationGroupID == s.PSIG_ID).Take(1)
                                          where s.PSIG_TenantID == clientID
                                              && s.PSIG_InvitationInitiatedByID == currentUserID
                                              && !s.PSIG_IsDeleted
                                              && !s.ProfileSharingInvitations.All(y => y.PSI_InvitationStatusID == adminInitializedInvitationStatus && !y.PSI_IsDeleted)
                                          select new ProfileSharingInvitationGroupContract
                                          {
                                              PSIG_ID = s.PSIG_ID,
                                              PSIG_AdminName = s.PSIG_AdminName,
                                              PSIG_AgencyID = s.PSIG_AgencyID,
                                              PSIG_AgencyName = s.Agency.AG_Name,
                                              PSIG_AssignedUnits = s.PSIG_AssignedUnits,
                                              PSIG_AttestationDate = s.PSIG_AttestationDate,
                                              PSIG_AttestationReportText = s.PSIG_AttestationReportText,
                                              PSIG_ClinicalFromDate = s.PSIG_ClinicalFromDate,
                                              PSIG_ClinicalRotationID = s.PSIG_ClinicalRotationID,
                                              PSIG_ClinicalToDate = s.PSIG_ClinicalToDate,
                                              PSIG_CreatedByID = s.PSIG_CreatedByID,
                                              PSIG_CreatedOn = s.PSIG_CreatedOn,
                                              PSIG_InvitationInitiatedByID = s.PSIG_InvitationInitiatedByID,
                                              PSIG_IsDeleted = s.PSIG_IsDeleted,
                                              PSIG_ModifiedByID = s.PSIG_ModifiedByID,
                                              PSIG_ModifiedOn = s.PSIG_ModifiedOn,
                                              PSIG_ProfileSharingInvitationGroupTypeID = s.PSIG_ProfileSharingInvitationGroupTypeID,
                                              PSIG_ProgramName = s.PSIG_ProgramName,
                                              PSIG_TenantID = s.PSIG_TenantID,
                                              PSI_EffectiveDate = psi.PSI_EffectiveDate
                                          });
            return AttestationDetailsData.ToList();
        }

        List<InvitationDocument> IProfileSharingRepository.GetAttestatationDocumentDetails(Int32 invitationGroupID)
        {
            List<Int32> lstInvitationDocumentIDs = _sharedDataDBContext.InvitationDocumentMappings.Where(cond => cond.IDM_ProfileSharingInvitationGroupID == invitationGroupID
                                                                                                        && cond.IDM_ProfileSharingInvitationID.HasValue
                                                                                                        && !cond.IDM_IsDeleted)
                                                                                            .Select(col => col.IDM_InvitationDocumentID).Distinct().ToList();

            if (!lstInvitationDocumentIDs.IsNullOrEmpty())
            {
                List<InvitationDocument> lstInvitationDocument = _sharedDataDBContext.InvitationDocuments.Where(cond => lstInvitationDocumentIDs.Contains(cond.IND_ID)).ToList();
                if (!lstInvitationDocument.IsNullOrEmpty())
                    return lstInvitationDocument;
                return new List<InvitationDocument>();
            }
            return new List<InvitationDocument>();
        }

        IEnumerable<InvitationDocumentMapping> IProfileSharingRepository.GetInvitationDocumentMapping(Int32 invitationGroupID)
        {
            IQueryable<InvitationDocumentMapping> invitationDocumentMappingList = _sharedDataDBContext.InvitationDocumentMappings
                                                                                                 .Where(cond => cond.IDM_ProfileSharingInvitationGroupID == invitationGroupID
                                                                                                    && !cond.IDM_IsDeleted);
            return invitationDocumentMappingList.ToList();
        }

        InvitationDocument IProfileSharingRepository.GetInvitationDocumentByDocumentID(Int32 invitationDocumentID)
        {

            InvitationDocument invitationDocument = _sharedDataDBContext.InvitationDocuments.Where(cond => cond.IND_ID == invitationDocumentID && !cond.IND_IsDeleted)
                                                                                     .FirstOrDefault();
            if (!invitationDocument.IsNullOrEmpty())
                return invitationDocument;
            return new InvitationDocument();

        }

        Boolean IProfileSharingRepository.UpdateInvitationDocumentPath(Int32 invitationDocumentId, String pathToUpdate, Int32 currentLoggedInUserId, Boolean isForEveryOne)
        {
            InvitationDocument invDocumentObj = _sharedDataDBContext.InvitationDocuments.Where(cond => cond.IND_ID == invitationDocumentId && !cond.IND_IsDeleted)
                                                                                     .FirstOrDefault();
            if (!invDocumentObj.IsNullOrEmpty())
            {
                invDocumentObj.IND_DocumentFilePath = pathToUpdate;
                invDocumentObj.IND_ModifiedByID = currentLoggedInUserId;
                invDocumentObj.IND_ModifiedOn = DateTime.Now;
                //invDocumentObj.IND_IsForEveryone = isForEveryOne;
                if (_sharedDataDBContext.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            return false;

        }

        InvitationDocument IProfileSharingRepository.GetInvitationDocumentByProfileSharingInvitationID(Int32 profilesharinginvitationID)
        {

            //InvitationDocument invitationDocument = _sharedDataDBContext.InvitationDocuments.Where(cond => cond.IND_ID == invitationDocumentID && !cond.IND_IsDeleted)
            //                                                                         .FirstOrDefault();
            //if (!invitationDocument.IsNullOrEmpty())
            //    return invitationDocument;
            //return new InvitationDocument();

            //UAT-1699 changes
            String consolidatedCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED.GetStringValue();
            String VerticalAttestationCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue();
            String AttestationCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT.GetStringValue();
            List<InvitationDocumentMapping> lstIDM = _sharedDataDBContext.InvitationDocumentMappings.Where(cond => cond.IDM_ProfileSharingInvitationID == profilesharinginvitationID && !cond.IDM_IsDeleted).ToList();

            if (!lstIDM.IsNullOrEmpty() && lstIDM.Any(cond => cond.InvitationDocument.lkpSharedSystemDocType.SSDT_Code == consolidatedCode))
            {
                return lstIDM.Where(cond => cond.InvitationDocument.lkpSharedSystemDocType.SSDT_Code == consolidatedCode).Select(cond => cond.InvitationDocument).FirstOrDefault();
            }
            else if (!lstIDM.IsNullOrEmpty() && lstIDM.Any(cond => cond.InvitationDocument.lkpSharedSystemDocType.SSDT_Code == AttestationCode))
            {
                return lstIDM.Where(cond => cond.InvitationDocument.lkpSharedSystemDocType.SSDT_Code == AttestationCode).Select(cond => cond.InvitationDocument).FirstOrDefault();
            }
            else
            {
                return lstIDM.Where(cond => cond.InvitationDocument.lkpSharedSystemDocType.SSDT_Code == VerticalAttestationCode).Select(cond => cond.InvitationDocument).FirstOrDefault();
            }
        }

        #endregion

        /// <summary>
        /// Method to udpate Invitation Viewed Status
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="invitationID"></param>
        void IProfileSharingRepository.UpdateInvitationViewedStatus(int currentUserID, int invitationID)
        {
            ProfileSharingInvitation psi = _sharedDataDBContext.ProfileSharingInvitations.Where(cond => cond.PSI_ID == invitationID && !cond.PSI_IsDeleted).FirstOrDefault();

            if (!psi.IsNullOrEmpty())
            {
                psi.PSI_IsInvitationViewed = true;
                psi.PSI_ModifiedById = currentUserID;
                psi.PSI_ModifiedOn = DateTime.Now;
                _sharedDataDBContext.SaveChanges();
            }
        }

        List<GetAttestationDocumentUserInfo_Result> IProfileSharingRepository.AttestationDocumentUserInfo(Int32 orgUserID, String documentType)
        {
            return _sharedDataDBContext.GetAttestationDocumentUserInfo(orgUserID, documentType).ToList();
        }

        #region UAT-1237 Add Agency/shared users to client user search

        DataTable IProfileSharingRepository.GetSharedUserSearchData(INTSOF.UI.Contract.SearchUI.SharedUserSearchContract sharedUserSearchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSharedUserSearchData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@dataXML", sharedUserSearchContract.CreateXml());
                command.Parameters.AddWithValue("@sortingAndPagingData", customPagingArgsContract.CreateXml());
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Method to get Shared User Invitation Details based on Shared UserID
        /// </summary>
        /// <param name="sharedUserID"></param>
        List<GetSharedUserInvitationDetails_Result> IProfileSharingRepository.GetSharedUserInvitationDetails(int sharedUserID)
        {
            List<GetSharedUserInvitationDetails_Result> invitationDetails = _sharedDataDBContext.GetSharedUserInvitationDetails(sharedUserID).ToList();

            if (!invitationDetails.IsNullOrEmpty())
            {
                return invitationDetails;
            }
            return new List<GetSharedUserInvitationDetails_Result>();
        }
        #endregion

        /// <summary>
        /// Update Profile Sharing Invitation Rotation Details
        /// </summary>
        /// <param name="invitationDetails"></param>
        void IProfileSharingRepository.UpdateProfileSharingInvRotationDetails(InvitationDetailsContract invitationDetails)
        {
            if (invitationDetails.RotationDetail.IsNotNull())
            {
                var profileSharingInvitation = _sharedDataDBContext.ProfileSharingInvitations.FirstOrDefault(x => x.PSI_ID == invitationDetails.PSIId && !x.PSI_IsDeleted);
                if (profileSharingInvitation.IsNotNull())
                {
                    var invitationRotationDetail = profileSharingInvitation.ProfileSharingInvitationGroup.ProfileSharingInvitationRotationDetails.FirstOrDefault(x => !x.PSIRD_IsDeleted);

                    if (!invitationRotationDetail.IsNullOrEmpty())
                    {
                        invitationRotationDetail.PSIRD_RotationName = invitationDetails.RotationDetail.RotationName;
                        invitationRotationDetail.PSIRD_TypeSpecialty = invitationDetails.RotationDetail.TypeSpecialty;
                        invitationRotationDetail.PSIRD_Department = invitationDetails.RotationDetail.Department;
                        invitationRotationDetail.PSIRD_Program = invitationDetails.RotationDetail.Program;
                        invitationRotationDetail.PSIRD_Course = invitationDetails.RotationDetail.Course;
                        invitationRotationDetail.PSIRD_Term = invitationDetails.RotationDetail.Term;
                        invitationRotationDetail.PSIRD_UnitFloor = invitationDetails.RotationDetail.UnitFloorLoc;
                        invitationRotationDetail.PSIRD_Shift = invitationDetails.RotationDetail.Shift;
                        invitationRotationDetail.PSIRD_StartTime = invitationDetails.RotationDetail.StartTime;
                        invitationRotationDetail.PSIRD_EndTime = invitationDetails.RotationDetail.EndTime;
                        invitationRotationDetail.PSIRD_StartDate = invitationDetails.RotationDetail.StartDate;
                        invitationRotationDetail.PSIRD_EndDate = invitationDetails.RotationDetail.EndDate;
                        invitationRotationDetail.PSIRD_ModifiedByID = invitationDetails.CurrentUserId;
                        invitationRotationDetail.PSIRD_ModifiedOn = invitationDetails.CurrentDateTime;
                        invitationRotationDetail.PSIRD_InstructorPreceptor = invitationDetails.RotationDetail.InstructorPreceptor; //UAT-3662

                        var existingMappedDays = invitationRotationDetail.ProfileSharingInvitationRotationDays.Where(cond => !cond.PSIRDY_IsDeleted).ToList();
                        List<Int32> daysToBeMapped = new List<Int32>();

                        if (!invitationDetails.RotationDetail.DaysIdList.IsNullOrEmpty())
                            daysToBeMapped = invitationDetails.RotationDetail.DaysIdList.Split(',').Select(int.Parse).ToList();

                        //Check whether existing MappedDays exist in current list if not exist then delete it
                        foreach (ProfileSharingInvitationRotationDay existingDay in existingMappedDays)
                        {
                            if (!(daysToBeMapped.Contains(existingDay.PSIRDY_WeekDayID.Value)))
                            {
                                existingDay.PSIRDY_IsDeleted = true;
                                existingDay.PSIRDY_ModifiedByID = invitationDetails.CurrentUserId;
                                existingDay.PSIRDY_ModifiedOn = invitationDetails.CurrentDateTime;
                            }
                        }
                        //Check whether selected day exist in db if not exist then insert it
                        foreach (Int32 day in daysToBeMapped)
                        {
                            if (!(existingMappedDays.Any(obj => obj.PSIRDY_WeekDayID == day && obj.PSIRDY_IsDeleted == false)))
                            {
                                ProfileSharingInvitationRotationDay newDay = new ProfileSharingInvitationRotationDay();
                                newDay.PSIRDY_WeekDayID = day;
                                newDay.PSIRDY_IsDeleted = false;
                                newDay.PSIRDY_CreatedByID = invitationDetails.CurrentUserId;
                                newDay.PSIRDY_CreatedOn = invitationDetails.CurrentDateTime;
                                invitationRotationDetail.ProfileSharingInvitationRotationDays.Add(newDay);
                            }
                        }

                    }
                    else
                    {
                        if (!invitationDetails.RotationDetail.IsNullOrEmpty())
                        {
                            ProfileSharingInvitationRotationDetail invitationRotationDetailInsert = new ProfileSharingInvitationRotationDetail();
                            invitationRotationDetailInsert.PSIRD_ProfileSharingInvitationGroupID = profileSharingInvitation.PSI_ProfileSharingInvitationGroupID;
                            invitationRotationDetailInsert.PSIRD_RotationName = invitationDetails.RotationDetail.RotationName;
                            invitationRotationDetailInsert.PSIRD_TypeSpecialty = invitationDetails.RotationDetail.TypeSpecialty;
                            invitationRotationDetailInsert.PSIRD_Department = invitationDetails.RotationDetail.Department;
                            invitationRotationDetailInsert.PSIRD_Program = invitationDetails.RotationDetail.Program;
                            invitationRotationDetailInsert.PSIRD_Course = invitationDetails.RotationDetail.Course;
                            invitationRotationDetailInsert.PSIRD_Term = invitationDetails.RotationDetail.Term;
                            invitationRotationDetailInsert.PSIRD_UnitFloor = invitationDetails.RotationDetail.UnitFloorLoc;
                            invitationRotationDetailInsert.PSIRD_Shift = invitationDetails.RotationDetail.Shift;
                            invitationRotationDetailInsert.PSIRD_StartTime = invitationDetails.RotationDetail.StartTime;
                            invitationRotationDetailInsert.PSIRD_EndTime = invitationDetails.RotationDetail.EndTime;
                            invitationRotationDetailInsert.PSIRD_StartDate = invitationDetails.RotationDetail.StartDate;
                            invitationRotationDetailInsert.PSIRD_EndDate = invitationDetails.RotationDetail.EndDate;
                            invitationRotationDetailInsert.PSIRD_IsDeleted = false;
                            invitationRotationDetailInsert.PSIRD_CreatedByID = invitationDetails.CurrentUserId;
                            invitationRotationDetailInsert.PSIRD_CreatedOn = DateTime.Now;
                            invitationRotationDetailInsert.PSIRD_InstructorPreceptor = invitationDetails.RotationDetail.InstructorPreceptor; //UAT-3662

                            List<Int32> daysToBeMapped = new List<Int32>();
                            if (!invitationDetails.RotationDetail.DaysIdList.IsNullOrEmpty())
                            {
                                daysToBeMapped = invitationDetails.RotationDetail.DaysIdList.Split(',').Select(int.Parse).ToList();
                                foreach (Int32 day in daysToBeMapped)
                                {
                                    ProfileSharingInvitationRotationDay newDay = new ProfileSharingInvitationRotationDay();
                                    newDay.PSIRDY_WeekDayID = day;
                                    newDay.PSIRDY_IsDeleted = false;
                                    newDay.PSIRDY_CreatedByID = invitationDetails.CurrentUserId;
                                    newDay.PSIRDY_CreatedOn = DateTime.Now;
                                    invitationRotationDetailInsert.ProfileSharingInvitationRotationDays.Add(newDay);
                                }
                            }
                            _sharedDataDBContext.ProfileSharingInvitationRotationDetails.AddObject(invitationRotationDetailInsert);
                        }
                    }
                    _sharedDataDBContext.SaveChanges();
                }
            }
        }

        #endregion


        #endregion

        #region UAT-1218
        void IProfileSharingRepository.UpdateClientContactUserID(Guid userID, String clientContactEmail, Int32 orgUserID)
        {
            List<ClientContact> lstClientContact = _sharedDataDBContext.ClientContacts.Where(cond => cond.CC_Email.ToLower() == clientContactEmail.ToLower()
                                                                                        && !cond.CC_IsDeleted).ToList();

            List<AgencyUser> lstAgencyUser = _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_Email.ToLower() == clientContactEmail.ToLower()
                                                                                        && !cond.AGU_IsDeleted).ToList();

            List<ProfileSharingInvitation> lstProfileSharingInvitation = _sharedDataDBContext.ProfileSharingInvitations.Where(cond => cond.PSI_InviteeEmail.ToLower() == clientContactEmail.ToLower()
                                                                                     && !cond.PSI_IsDeleted).ToList();

            //Updating ClientContact
            foreach (var clientContact in lstClientContact)
            {
                clientContact.CC_UserID = userID;
                clientContact.CC_ModifiedByID = orgUserID;
                clientContact.CC_ModifiedOn = DateTime.Now;
            }

            //Updating AgencyUser
            foreach (var agencyUser in lstAgencyUser)
            {
                agencyUser.AGU_UserID = userID;
                agencyUser.AGU_ModifiedByID = orgUserID;
                agencyUser.AGU_ModifiedOn = DateTime.Now;
            }

            //Updating ProfileSharingInvitation
            foreach (var invitation in lstProfileSharingInvitation)
            {
                if (invitation.PSI_InviteeOrgUserID == null)
                {
                    invitation.PSI_InviteeOrgUserID = orgUserID;
                    invitation.PSI_ModifiedById = orgUserID;
                    invitation.PSI_ModifiedOn = DateTime.Now;
                }
            }

            _sharedDataDBContext.SaveChanges();
        }
        #endregion

        /// <summary>
        /// Get Agency detail    
        /// </summary>
        /// <param name="agencyID"></param>
        /// <returns></returns>
        usp_GetAgencyDetailByAgencyID_Result IProfileSharingRepository.GetAgencyDetailByAgencyID(Int32 agencyID)
        {
            return _sharedDataDBContext.usp_GetAgencyDetailByAgencyID(agencyID).FirstOrDefault();
        }

        /// <summary>
        /// Search Agencies acc: to searchStatusID    
        /// </summary>
        /// <param name="searchStatusID"></param>
        /// <returns></returns>
        List<usp_SearchAgency_Result> IProfileSharingRepository.SearchAgency(string searchStatus)
        {
            return _sharedDataDBContext.usp_SearchAgency(searchStatus).ToList();
        }

        /// <summary>
        /// Save Agency Institution Mapping
        /// </summary>
        /// <param name="agencyInstitution"></param>
        /// <returns></returns>
        Tuple<String, Int32> IProfileSharingRepository.SaveAgencyInstitutionMapping(AgencyInstitution agencyInstitution)
        {
            _sharedDataDBContext.AgencyInstitutions.AddObject(agencyInstitution);
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return new Tuple<String, Int32>(AppConsts.AG_SAVED_SUCCESS_MSG, agencyInstitution.AGI_ID);
            }
            else
            {
                return new Tuple<String, Int32>(AppConsts.AG_SAVED_ERROR_MSG, AppConsts.NONE);
            }
        }

        DataTable IProfileSharingRepository.GetClientDataForAgencyAndAgencyHierarchyUsingFlatTable(String AGencyID, String Tenantid, CustomPagingArgsContract customPagingArgsContract)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSchoolRepresentativeSearchFlatTable", con);
                command.CommandType = CommandType.StoredProcedure;
                if (customPagingArgsContract.SortExpression.IsNullOrEmpty())
                    customPagingArgsContract.SortExpression = "FSRD_OrganizationUserID";
                string ordDir = customPagingArgsContract.SortDirectionDescending ? "desc" : "asc";
                command.Parameters.AddWithValue("@AgencyID", AGencyID);
                command.Parameters.AddWithValue("@TenantID", Tenantid);
                command.Parameters.AddWithValue("@OrderBy", customPagingArgsContract.SortExpression);
                command.Parameters.AddWithValue("@OrderDirection", ordDir);
                command.Parameters.AddWithValue("@PageIndex", customPagingArgsContract.CurrentPageIndex);
                command.Parameters.AddWithValue("@PageSize", customPagingArgsContract.PageSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }
        /// <summary>
        /// Check is agency associate with institution
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="agencyID"></param>
        /// <returns></returns>
        bool IProfileSharingRepository.IsAgencyAssociateWithInstitution(Int32 institutionID, Int32 agencyID)
        {
            var agencyInstitution = _sharedDataDBContext.AgencyInstitutions.FirstOrDefault(cond => cond.AGI_AgencyID == agencyID && cond.AGI_TenantID == institutionID && cond.AGI_IsDeleted != true);
            if (agencyInstitution.IsNotNull())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to cehck whether Shared User recieved any Bkg Order invitation
        /// </summary>
        /// <returns></returns>
        public Boolean CheckForBkgInvitation(Int32 orgUserID)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_CheckSharedUserBkgOrderInvitation", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrgUserID", orgUserID);
                command.Parameters.Add("@resultValue", SqlDbType.Bit);
                command.Parameters["@resultValue"].Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
                return (Boolean)command.Parameters["@resultValue"].Value;
            }
        }


        public Boolean IsNPINumberExist(string npiNumber)
        {
            return _sharedDataDBContext.Agencies.Any(cond => cond.AG_NpiNumber.ToLower() == npiNumber.ToLower() && !cond.AG_IsDeleted);
        }

        List<SheduledInvitationContract> IProfileSharingRepository.GetScheduledInvitations(Int32 chunkSize, Int32 maxRetryCount, Int32 retryTimeLag)
        {
            List<SheduledInvitationContract> lstScheduledInvContract = new List<SheduledInvitationContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ChunkSize", chunkSize),
                             new SqlParameter("@MaxRetryCount", maxRetryCount),
                             new SqlParameter("@RetryTimeLag", retryTimeLag)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetScheduledInvitations", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SheduledInvitationContract currentScheduledInvContract = new SheduledInvitationContract();
                            currentScheduledInvContract.InvitationID = dr["InvitationID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["InvitationID"]);
                            currentScheduledInvContract.InvitationGroupID = dr["InvitationGroupID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["InvitationGroupID"]);
                            currentScheduledInvContract.EffectiveDate = dr["EffectiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EffectiveDate"]);
                            currentScheduledInvContract.InvitationStatusID = dr["InvitationStatusID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["InvitationStatusID"]);
                            currentScheduledInvContract.TenantID = dr["TenantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TenantID"]);
                            currentScheduledInvContract.ApplicantID = dr["ApplicantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ApplicantID"]);
                            currentScheduledInvContract.InviteeSharedMetadataCodes = dr["InviteeSharedMetaDataCodes"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InviteeSharedMetaDataCodes"]);
                            currentScheduledInvContract.RotationID = dr["RotationID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RotationID"]);
                            currentScheduledInvContract.InviteeUserTypeCode = dr["InviteeUserTypeCode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InviteeUserTypeCode"]);
                            currentScheduledInvContract.ComplianceSharedInfoTypeCode = dr["ComplianceSharedInfoTypeCode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ComplianceSharedInfoTypeCode"]);
                            currentScheduledInvContract.RequiredSharedInfoTypeCode = dr["RequiredSharedInfoTypeCode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RequiredSharedInfoTypeCode"]);
                            currentScheduledInvContract.BkgSharedInfoTypeCode = dr["BkgSharedInfoTypeCode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["BkgSharedInfoTypeCode"]);
                            currentScheduledInvContract.InviteeName = dr["InviteeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InviteeName"]);
                            currentScheduledInvContract.InvitationToken = dr["InvitationToken"].GetType().Name == "DBNull" ? new Guid() : Guid.Parse(Convert.ToString(dr["InvitationToken"]));
                            currentScheduledInvContract.EmailAddress = dr["InviteeEmail"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InviteeEmail"]);
                            currentScheduledInvContract.SharedApplicantMetaDataIds = dr["InviteeSharedMetaDataIDs"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InviteeSharedMetaDataIDs"]);
                            currentScheduledInvContract.InviteeUserId = dr["InviteeOrgUserID"].GetType().Name == "DBNull" ? (Int32?)null : Convert.ToInt32(dr["InviteeOrgUserID"]);

                            currentScheduledInvContract.IsTenantDataSaved = Convert.ToBoolean(dr["IsTenantDataSaved"]);

                            currentScheduledInvContract.InvitationInitiatedByID = dr["InvitationInitiatedByID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["InvitationInitiatedByID"]);
                            currentScheduledInvContract.InvitationInitiatedUserFullName = dr["InvitationInitiatedUserFullName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InvitationInitiatedUserFullName"]);
                            currentScheduledInvContract.InvitationInitiatedUserEmailId = dr["InvitationInitiatedUserEmailId"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InvitationInitiatedUserEmailId"]);

                            currentScheduledInvContract.InviteeAgency = dr["InviteeAgency"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InviteeAgency"]);
                            currentScheduledInvContract.AgencyID = dr["AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            lstScheduledInvContract.Add(currentScheduledInvContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

            }
            return lstScheduledInvContract;
        }

        //void IProfileSharingRepository.SaveScheduledInvitationsSharedMetaData(List<SheduledInvitationContract> lstInvitationDetails, Int32 currentUserId, DateTime currentDateTime)
        //{
        //    lstInvitationDetails.ForEach(invitation =>
        //    {
        //        List<Int32> lstSharedApplicantMetaDataIds = invitation.SharedApplicantMetaDataIds.Split(',').Select(Int32.Parse).ToList();

        //        foreach (var amdId in lstSharedApplicantMetaDataIds)
        //        {
        //            var _applicantMetaData = new ApplicantSharedInvitationMetaData();
        //            _applicantMetaData.SIMD_ProfileSharingInvitationID = invitation.InvitationID;
        //            _applicantMetaData.SIMD_ApplicantInvitationMetaDataID = amdId;
        //            _applicantMetaData.SIMD_CreatedByID = currentUserId;
        //            _applicantMetaData.SIMD_CreatedOn = currentDateTime;
        //            _applicantMetaData.SIMD_IsDeleted = false;
        //            _sharedDataDBContext.ApplicantSharedInvitationMetaDatas.AddObject(_applicantMetaData);
        //        }
        //    });
        //    _sharedDataDBContext.SaveChanges();
        //}

        void IProfileSharingRepository.SaveScheduledInvitationsSharedMetaData(List<SheduledInvitationContract> lstInvitationDetails, Int32 currentUserId, DateTime currentDateTime)
        {
            lstInvitationDetails.ForEach(invitation =>
            {
                List<Int32> lstSharedApplicantMetaDataIds = invitation.SharedApplicantMetaDataIds.Split(',').Select(Int32.Parse).ToList();

                foreach (var amdId in lstSharedApplicantMetaDataIds)
                {
                    var _applicantMetaData = new ApplicantSharedInvitationMetaData();
                    _applicantMetaData.SIMD_ProfileSharingInvitationID = invitation.InvitationID;
                    _applicantMetaData.SIMD_ApplicantInvitationMetaDataID = amdId;
                    _applicantMetaData.SIMD_CreatedByID = currentUserId;
                    _applicantMetaData.SIMD_CreatedOn = currentDateTime;
                    _applicantMetaData.SIMD_IsDeleted = false;
                    _sharedDataDBContext.ApplicantSharedInvitationMetaDatas.AddObject(_applicantMetaData);
                }
            });
            _sharedDataDBContext.SaveChanges();
        }


        /// <summary>
        /// Update the 'PSI_IsTenantDataSaveRequired' for he Invitation groups to be 'False' so that they are not saved by th Schedling service, when retry is performed for them.
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="invitationId"></param>
        /// <param name="currentUserId"></param>
        void IProfileSharingRepository.UpdateInvitationGroupSaveStatus(List<Int32> lstInvitationIds, Int32 currentUserId)
        {
            var _lstInvitations = _sharedDataDBContext.ProfileSharingInvitations.Where(psi => lstInvitationIds.Contains(psi.PSI_ID) && psi.PSI_IsDeleted == false).ToList();

            foreach (var _invitationData in _lstInvitations)
            {
                _invitationData.PSI_IsTenantDataSaved = true;
                _invitationData.PSI_ModifiedOn = DateTime.Now;
                _invitationData.PSI_ModifiedById = currentUserId;
            }
            _sharedDataDBContext.SaveChanges();
        }

        void IProfileSharingRepository.UpdateRetryCountForFailedInvitation(Int32 currentGroupID, Int32 currentUserId)
        {
            var invGroup = _sharedDataDBContext.ProfileSharingInvitationGroups.Where(psig => psig.PSIG_ID == currentGroupID && !psig.PSIG_IsDeleted).FirstOrDefault();

            invGroup.PSIG_RetryCount = invGroup.PSIG_RetryCount.IsNull() ? AppConsts.ONE : (invGroup.PSIG_RetryCount + AppConsts.ONE);
            invGroup.PSIG_RetryDate = DateTime.Now;
            invGroup.PSIG_ModifiedByID = currentUserId;
            invGroup.PSIG_ModifiedOn = DateTime.Now;
            _sharedDataDBContext.SaveChanges();
        }

        #region UAT 1320 Client admin expire profile shares
        Boolean IProfileSharingRepository.SaveUpdateProfileExpirationCriteria(InvitationDetailsContract invitationDetailsContract, List<Int32> lstInvitationIDs)
        {
            List<ProfileSharingInvitation> lstInvitationData = _sharedDataDBContext.ProfileSharingInvitations.Where(cond => lstInvitationIDs.Contains(cond.PSI_ID) && !cond.PSI_IsDeleted).ToList();
            lstInvitationData.ForEach(invitation =>
             {
                 invitation.PSI_ExpirationDate = invitationDetailsContract.ExpirationDate;
                 invitation.PSI_ExpirationTypeID = invitationDetailsContract.ExpirationTypeId;
                 invitation.PSI_MaxViews = invitationDetailsContract.MaxViews;
                 invitation.PSI_InviteeViewCount = invitationDetailsContract.InviteeViewCount;
                 invitation.PSI_ModifiedOn = DateTime.Now;
                 invitation.PSI_ModifiedById = invitationDetailsContract.CurrentUserId;
                 //UAT-1895 Check if Audit requested Invitation is Selected then update the isExpirationRequested column
                 if (invitationDetailsContract.ClearAuditRequestFlag == true)
                 {
                     invitation.PSI_IsExpirationRequested = false;
                     invitation.PSI_AuditRequestedDate = null;
                 }
             });

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Shared User Dashboard Data

        /// <summary>
        /// Get the Shared User Dashboard Pie Chart related data
        /// </summary>
        /// <param name="inviteeOrgUserID"></param>
        /// <returns></returns>
        List<InstitutionProfileContract> IProfileSharingRepository.GetSharedStudentsPerInstitution(Int32 inviteeOrgUserID, DateTime? fromDate, DateTime? toDate)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            var _totalApplicants = AppConsts.NONE;

            var _lstPeiChartData = new List<InstitutionProfileContract>();
            var _lstTempData = new List<InstitutionProfileContract>();

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                      new SqlParameter("@InviteeOrgUserID",inviteeOrgUserID),
                      new SqlParameter("@FromDate",fromDate),
                      new SqlParameter("@ToDate",toDate)
                };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSharedStudentsPerInstitution", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            _totalApplicants += Convert.ToInt32(dr["NoOfStudents"]);

                            _lstTempData.Add
                                (new InstitutionProfileContract(
                             Convert.ToString(dr["TenantName"]),
                             Convert.ToDouble(dr["NoOfStudents"]),
                             false
                             ,
                            Convert.ToString(dr["Color"])
                            ));
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }

            if (_lstTempData.IsNullOrEmpty())
            {
                return _lstPeiChartData;
            }

            _lstTempData.ForEach(td =>
            {
                _lstPeiChartData.Add(new InstitutionProfileContract
                    (td.Name
                     , Math.Round((td.Share * 100) / _totalApplicants, 2)
                     , false
                     ,
                     td.pieColor
                     )
                    );
            });

            return _lstPeiChartData;
        }

        /// <summary>
        /// Get the Shared User Dashboard Calendar and Grid related data
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        List<DashBoardRotationDataContract> IProfileSharingRepository.GetDashBoardRotationData(Guid userId, Int32 organizationUserId)
        {
            List<DashBoardRotationDataContract> lstRotationDataContract = new List<DashBoardRotationDataContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                      new SqlParameter("@UserID",userId),
                      new SqlParameter("@LoggedInOrgUserId",organizationUserId),
                      new SqlParameter("@DroppedStatus", AppConsts.APPLICANT_DROPPED_STATUS),
                };
                base.OpenSQLDataReaderConnection(con);
                using (SqlCommand sqlCommand = new SqlCommand("usp_GetDashboardRotationData", con))
                {
                    sqlCommand.CommandTimeout = 122;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (sqlParameterCollection != null)
                    {
                        sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    }
                    using (SqlDataReader dr = sqlCommand.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                DashBoardRotationDataContract currentRotationData = new DashBoardRotationDataContract();
                                currentRotationData.ProfileSharingInvitationRotationDetailID = Convert.ToInt32(dr["PSIRD_ID"]);
                                currentRotationData.Tenant = Convert.ToString(dr["TenantName"]);
                                currentRotationData.TenantID = Convert.ToInt32(dr["TenantID"]);
                                currentRotationData.AgencyName = Convert.ToString(dr["AgencyName"]);
                                currentRotationData.RotationId = dr["RotationId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RotationId"]);
                                currentRotationData.RotationDepartment = Convert.ToString(dr["RotationDepartment"]);
                                currentRotationData.RotationProgram = Convert.ToString(dr["RotationProgram"]);
                                currentRotationData.RotationCourse = Convert.ToString(dr["RotationCourse"]);
                                currentRotationData.RotationUnitFloorLoc = Convert.ToString(dr["RotationUnitFloorLoc"]);
                                currentRotationData.RotationNoOfHrs = dr["RotationNoOfHrs"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RotationNoOfHrs"]);
                                currentRotationData.RotationStartDate = dr["RotationStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RotationStartDate"]);
                                currentRotationData.RotationEndDate = dr["RotationEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RotationEndDate"]);
                                currentRotationData.RotationReviewStatusCode = Convert.ToString(dr["RotationReviewStatusCode"]);
                                currentRotationData.RotationReviewStatus = Convert.ToString(dr["RotationReviewStatus"]);
                                currentRotationData.RotationName = Convert.ToString(dr["RotationName"]);
                                if (dr["RotationStartDate"] != DBNull.Value && dr["RotationEndDate"] != DBNull.Value)
                                    currentRotationData.RotationDate = String.Format("{0:M/d/yyyy}", dr["RotationStartDate"]) + " - " + String.Format("{0:M/d/yyyy}", dr["RotationEndDate"]);
                                currentRotationData.RotationTerm = Convert.ToString(dr["RotationTerm"]);
                                currentRotationData.RotationTypeSpecialty = Convert.ToString(dr["RotationTypeSpecialty"]);
                                currentRotationData.RotationStartTime = dr["RotationStartTime"] == DBNull.Value ? String.Empty
                                                    : DateTime.ParseExact(dr["RotationStartTime"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                                currentRotationData.RotationEndTime = dr["RotationEndTime"] == DBNull.Value ? String.Empty
                                                    : DateTime.ParseExact(dr["RotationEndTime"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                                currentRotationData.RotationComplioID = Convert.ToString(dr["RotationComplioID"]);
                                currentRotationData.RotationShift = Convert.ToString(dr["RotationShift"]);
                                currentRotationData.InvitationDate = dr["InvitationDate"].GetType().Name == "DBNull" ? (DateTime?)null : Convert.ToDateTime(dr["InvitationDate"]);
                                currentRotationData.RotationDays = Convert.ToString(dr["RotationDays"]);
                                currentRotationData.ApplicantCount = Convert.ToInt32(dr["ApplicantCount"]);
                                currentRotationData.IsShared = Convert.ToBoolean(dr["IsShared"]);
                                currentRotationData.IsInvitationExpired = Convert.ToBoolean(dr["IsInvitationExpired"]); //UAT-3424
                                currentRotationData.RotationFullDays = Convert.ToString(dr["RotationFullDays"]);
                                currentRotationData.ProfileSharingInvitationID = Convert.ToInt32(dr["ProfileSharingInvitationID"]);
                                currentRotationData.IsRotationSharing = Convert.ToBoolean(dr["IsRotationSharing"]);
                                currentRotationData.RotationStudentName = dr["RotationStudentName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationStudentName"]);
                                currentRotationData.AgencyID = dr["AgencyID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["AgencyID"]);
                                currentRotationData.IsDropped = dr["IsDropped"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsDropped"]);
                                //UAT-3751
                                currentRotationData.CustomAttributes = dr["CustomAttributes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CustomAttributes"]);
                                //UAT-3928
                                currentRotationData.IsInvShdByAppByAgencyDDl = dr["IsInvShdByAppByAgencyDDl"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsInvShdByAppByAgencyDDl"]);
                                currentRotationData.RotationDroppedDate = dr["RotationDroppedDate"].IsNullOrEmpty() || dr["RotationDroppedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RotationDroppedDate"]);
                                lstRotationDataContract.Add(currentRotationData);
                            }
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);

            }
            return lstRotationDataContract;
        }

        /// <summary>
        /// Get the Shared User basic details to be displayed on dashboard.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        SharedUserDashboardDetailsContract IProfileSharingRepository.GetSharedUserDashboardDetails(Guid userId)
        {
            var lstDashBoardContract = new List<SharedUserDashboardDetailsContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@userid",userId),
                };
                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetIPSharedUserDashboardDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SharedUserDashboardDetailsContract userDashBoardContract = new SharedUserDashboardDetailsContract();
                            userDashBoardContract.SharedUserName = Convert.ToString(dr["SharedUserName"]);
                            userDashBoardContract.SharedUserEmail = Convert.ToString(dr["SharedUserEmail"]);
                            userDashBoardContract.SharedUserPhone = Convert.ToString(dr["SharedUsePhone"]);
                            userDashBoardContract.SharedUserBackgroundPermissions = Convert.ToString(dr["SharedUserBackgroundPermissions"]);
                            userDashBoardContract.SharedUserCompliancePremissions = Convert.ToString(dr["SharedUserCompliancePermissions"]);
                            userDashBoardContract.TenantName = Convert.ToString(dr["TenantName"]);
                            lstDashBoardContract.Add(userDashBoardContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);

                var lstIsntitutions = lstDashBoardContract.Select(sud => sud.TenantName).Distinct();

                lstDashBoardContract.ForEach(dc =>
                {
                    dc.TenantName = String.Join(", ", lstIsntitutions.Select(s => s.ToString()));
                });
            }
            return lstDashBoardContract.FirstOrDefault();
        }

        AgencyUserDashboardDetailsContract IProfileSharingRepository.GetAgencyUserDashboardDetails(Guid userId)
        {
            var agencyUserDashboardDetailsContract = new AgencyUserDashboardDetailsContract();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSharedUserDashboardDetails", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userid", userId);

                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();

                da.SelectCommand = command;
                da.Fill(ds);

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    {
                        //Getting first row to get basic detail
                        DataRow row = ds.Tables[0].Rows[0];
                        agencyUserDashboardDetailsContract.AgencyUserName = Convert.ToString(row["UserName"]);
                        agencyUserDashboardDetailsContract.AgencyUserEmail = Convert.ToString(row["UserEmail"]);
                        agencyUserDashboardDetailsContract.AgencyUserCompliancePremissions = Convert.ToString(row["UserCompliancePermissions"]);
                        agencyUserDashboardDetailsContract.AgencyUserRequirementPermissions = Convert.ToString(row["UserRequirementPermissions"]);
                        agencyUserDashboardDetailsContract.AgencyUserBackgroundPermissions = Convert.ToString(row["UserBackgroundPermissions"]);
                        agencyUserDashboardDetailsContract.AgencyUserPhone = Convert.ToString(row["UserPhoneNumber"]);
                    }

                    //Getting Agency & institution Mapping
                    if (ds.Tables[1] != null && ds.Tables[1].Rows != null && ds.Tables[1].Rows.Count > 0)
                    {
                        agencyUserDashboardDetailsContract.LstAgencyUserAgencyInstitutionContract = new List<AgencyUserAgencyInstitutionContract>();
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            agencyUserDashboardDetailsContract.LstAgencyUserAgencyInstitutionContract.Add(new AgencyUserAgencyInstitutionContract()
                            {
                                AgencyId = Convert.ToInt32(dr["AgencyId"]),
                                AgencyName = Convert.ToString(dr["AgencyName"]),
                                MappedInstitutions = Convert.ToString(dr["TenantName"])
                            });
                        }
                    }

                    //Getting Root Node
                    if (ds.Tables[2] != null && ds.Tables[2].Rows != null && ds.Tables[2].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[2].Rows[0];
                        agencyUserDashboardDetailsContract.AgencyHierarchyRootNode = Convert.ToString(row["AgencyHierarchyRootNode"]);
                    }
                }
            }
            return agencyUserDashboardDetailsContract;
        }

        #endregion

        #region UAT 1530 WB: If sharing with an agency that does not have any users, client admin should have to fill out a form displaying the information of the person they would like to add.
        Boolean IProfileSharingRepository.SaveSharedUserForReview(SharedUserReviewQueue sharedUserReviewQueue)
        {
            if (sharedUserReviewQueue.IsNotNull())
            {
                _sharedDataDBContext.AddToSharedUserReviewQueues(sharedUserReviewQueue);
                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }
            return false;
        }

        Tuple<List<SharedUserReiewQueueContract>, Int32> IProfileSharingRepository.GetSharedUserReviewQueueData(SharedUserReiewQueueContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            List<SharedUserReiewQueueContract> sharedUserReiewQueueList = new List<SharedUserReiewQueueContract>();
            Int32 currentPageIndex = AppConsts.ONE;

            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@TenantId", searchDataContract.TenantID),
                             new SqlParameter("@AgencyID", searchDataContract.AgencyID),
                             new SqlParameter("@FirstName", searchDataContract.FirstName),
                             new SqlParameter("@LastName", searchDataContract.LastName),
                             new SqlParameter("@EmailAddress", searchDataContract.EmailAddress),
                             new SqlParameter("@Phone", searchDataContract.Phone),
                             new SqlParameter("@SortExpression", gridCustomPaging.SortExpression),
                             new SqlParameter("@SortDirectionDescending", gridCustomPaging.SortDirectionDescending),
                             new SqlParameter("@CurrentPageIndex", gridCustomPaging.CurrentPageIndex),
                             new SqlParameter("@PageSize", gridCustomPaging.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSharedUserReviewQueueData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SharedUserReiewQueueContract sharedUserReiewData = new SharedUserReiewQueueContract();
                            sharedUserReiewData.SURQ_ID = dr["SURQ_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SURQ_ID"]);
                            sharedUserReiewData.FirstName = Convert.ToString(dr["SURQ_FirstName"]);
                            sharedUserReiewData.LastName = Convert.ToString(dr["SURQ_LastName"]);
                            sharedUserReiewData.EmailAddress = Convert.ToString(dr["SURQ_EmailId"]);
                            sharedUserReiewData.Phone = Convert.ToString(dr["SURQ_Phone"]);
                            sharedUserReiewData.Title = dr["SURQ_Title"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SURQ_Title"]);
                            sharedUserReiewData.Notes = dr["SURQ_Note"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SURQ_Note"]);
                            sharedUserReiewData.TenantID = dr["TenantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TenantID"]);
                            sharedUserReiewData.InstituteName = dr["AG_ID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["TenantName"]);
                            sharedUserReiewData.AgencyID = dr["AG_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AG_ID"]);
                            sharedUserReiewData.AgencyName = dr["AG_Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AG_Name"]);
                            sharedUserReiewData.RequestedBy = dr["RequestedBy"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RequestedBy"]);
                            sharedUserReiewData.TotalCount = dr["TotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TotalCount"]);

                            sharedUserReiewQueueList.Add(sharedUserReiewData);
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                currentPageIndex = dr["CurrentPageIndex"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["CurrentPageIndex"]);
                            }
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }

            return new Tuple<List<SharedUserReiewQueueContract>, Int32>(sharedUserReiewQueueList, currentPageIndex);
        }

        Boolean IProfileSharingRepository.UpdateSharedUserReviewQueueStatus(List<Int32> ids, Int32 currentUserId, Int32 reviewedStatusId)
        {
            List<SharedUserReviewQueue> lstSharedUserReviewQueue = SharedDataDBContext.SharedUserReviewQueues.Where(cond => ids.Contains(cond.SURQ_ID)
                                                                                                                                && !cond.SURQ_IsDeleted).ToList();
            foreach (SharedUserReviewQueue record in lstSharedUserReviewQueue)
            {
                record.SURQ_StatusId = reviewedStatusId;
                record.SURQ_ModifiedById = currentUserId;
                record.SURQ_ModifiedOn = DateTime.Now;
            }
            if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        Boolean IProfileSharingRepository.DeleteSharedUserReviewQueueRecord(Int32 SURQ_ID, Int32 currentUserId)
        {
            SharedUserReviewQueue sharedUserReviewQueueRecord = SharedDataDBContext.SharedUserReviewQueues.Where(cond => cond.SURQ_ID == SURQ_ID
                                                                                                                                && !cond.SURQ_IsDeleted).FirstOrDefault();
            if (!sharedUserReviewQueueRecord.IsNullOrEmpty())
            {
                sharedUserReviewQueueRecord.SURQ_IsDeleted = true;
                sharedUserReviewQueueRecord.SURQ_ModifiedById = currentUserId;
                sharedUserReviewQueueRecord.SURQ_ModifiedOn = DateTime.Now;
                if (SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        public List<AgencyAttestationDetailContract> GetAttestationReportTextForAgency(String agencyIDs)
        {

            List<AgencyAttestationDetailContract> lstAgencyAttestationDetailContract = new List<AgencyAttestationDetailContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@AgencyIDs", agencyIDs)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyAttestationDetails", sqlParameterCollection))
                {
                    while (dr.Read())
                    {
                        AgencyAttestationDetailContract agencyAttestationDetailContract = new AgencyAttestationDetailContract();
                        agencyAttestationDetailContract.AgencyID = dr["AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AgencyID"]);
                        agencyAttestationDetailContract.AgencyName = dr["AgencyName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AgencyName"]);
                        agencyAttestationDetailContract.ClientSystemDocumentID = dr["ClientSystemDocumentID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ClientSystemDocumentID"]);
                        agencyAttestationDetailContract.DocPath = dr["DocPath"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["DocPath"]);
                        agencyAttestationDetailContract.DocFileName = dr["DocFileName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["DocFileName"]);
                        agencyAttestationDetailContract.AttestationFormSettingValue = dr["AttestationFormSettingValue"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["AttestationFormSettingValue"]);
                        agencyAttestationDetailContract.AttestationReportText = dr["AttestationReportText"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AttestationReportText"]);
                        lstAgencyAttestationDetailContract.Add(agencyAttestationDetailContract);
                    }
                }
            }
            return lstAgencyAttestationDetailContract;
            //List<Int32> agencyIDLst = new List<int>();
            //if (!String.IsNullOrEmpty(agencyIDs))
            //{
            //    foreach (String agencyID in agencyIDs.Split(','))
            //    {
            //        agencyIDLst.Add(Convert.ToInt32(agencyID));
            //    }
            //}
            //List<Tuple<Int32, String, String>> AtestationDataTupl = new List<Tuple<int, string, string>>();
            //var atestationData = _sharedDataDBContext.Agencies.Where(x => agencyIDLst.Contains(x.AG_ID) && !x.AG_IsDeleted).Select(col => new { AgencyID = col.AG_ID, AgencyName = col.AG_Name, AttestationText = col.AG_AttestationReportText }).ToList();
            //if (atestationData.Any())
            //{
            //    Tuple<Int32, String, String> tdata;
            //    foreach (var data in atestationData)
            //    {
            //        tdata = new Tuple<int, string, string>(data.AgencyID, data.AgencyName, data.AttestationText);
            //        AtestationDataTupl.Add(tdata);
            //    }
            //}
            // return AtestationDataTupl;
        }

        public List<ProfileSharingInvitationGroupContract> GetSharedInvitationsData(String searchContractXML, String gridCustomPagingXML)
        {
            List<ProfileSharingInvitationGroupContract> lstProfileSharingInvitationGroupContract = new List<ProfileSharingInvitationGroupContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@SearchDataXML", searchContractXML),
                             new SqlParameter("@CustomFilteringXML", gridCustomPagingXML),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSharedInvitationsData", sqlParameterCollection))
                {
                    while (dr.Read())
                    {
                        ProfileSharingInvitationGroupContract profileSharingInvitationGroupContract = new ProfileSharingInvitationGroupContract();
                        profileSharingInvitationGroupContract.PSIG_ID = dr["PSIG_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["PSIG_ID"]);
                        profileSharingInvitationGroupContract.PSIG_AdminName = dr["PSIG_AdminName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["PSIG_AdminName"]);
                        profileSharingInvitationGroupContract.PSIG_AgencyID = dr["PSIG_AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["PSIG_AgencyID"]);
                        profileSharingInvitationGroupContract.PSIG_AgencyName = dr["AG_Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AG_Name"]);
                        profileSharingInvitationGroupContract.PSIG_AssignedUnits = dr["PSIG_AssignedUnits"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["PSIG_AssignedUnits"]);
                        profileSharingInvitationGroupContract.PSIG_AttestationDate = dr["PSIG_AttestationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["PSIG_AttestationDate"]);
                        profileSharingInvitationGroupContract.PSIG_AttestationReportText = dr["PSIG_AttestationReportText"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["PSIG_AttestationReportText"]);
                        profileSharingInvitationGroupContract.PSIG_ClinicalFromDate = dr["PSIG_ClinicalFromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["PSIG_ClinicalFromDate"]);
                        profileSharingInvitationGroupContract.PSIG_ClinicalRotationID = dr["PSIG_ClinicalRotationID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["PSIG_ClinicalRotationID"]);
                        profileSharingInvitationGroupContract.PSIG_ClinicalToDate = dr["PSIG_ClinicalToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["PSIG_ClinicalToDate"]);
                        profileSharingInvitationGroupContract.PSIG_CreatedByID = dr["PSIG_CreatedByID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["PSIG_CreatedByID"]);
                        profileSharingInvitationGroupContract.PSIG_CreatedOn = Convert.ToDateTime(dr["PSIG_CreatedOn"]);
                        profileSharingInvitationGroupContract.PSIG_InvitationInitiatedByID = dr["PSIG_InvitationInitiatedByID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["PSIG_InvitationInitiatedByID"]);
                        profileSharingInvitationGroupContract.PSIG_IsDeleted = dr["PSIG_IsDeleted"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["PSIG_IsDeleted"]);
                        profileSharingInvitationGroupContract.PSIG_ModifiedByID = dr["PSIG_ModifiedByID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["PSIG_ModifiedByID"]);
                        profileSharingInvitationGroupContract.PSIG_ModifiedOn = dr["PSIG_ModifiedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["PSIG_ModifiedOn"]);
                        profileSharingInvitationGroupContract.PSIG_ProfileSharingInvitationGroupTypeID = dr["PSIG_ProfileSharingInvitationGroupTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["PSIG_ProfileSharingInvitationGroupTypeID"]);
                        profileSharingInvitationGroupContract.PSIG_ProgramName = dr["PSIG_ProgramName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["PSIG_ProgramName"]);
                        profileSharingInvitationGroupContract.PSIG_TenantID = dr["PSIG_TenantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["PSIG_TenantID"]);
                        profileSharingInvitationGroupContract.PSI_EffectiveDate = dr["PSI_EffectiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["PSI_EffectiveDate"]);
                        profileSharingInvitationGroupContract.TotalRecordCount = dr["TotalRecordCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TotalRecordCount"]);
                        profileSharingInvitationGroupContract.TenantName = dr["TenantName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["TenantName"]);
                        profileSharingInvitationGroupContract.PSI_IsInvitationViewed = dr["InvitationViewedStatus"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InvitationViewedStatus"]);
                        lstProfileSharingInvitationGroupContract.Add(profileSharingInvitationGroupContract);
                    }
                }
            }
            return lstProfileSharingInvitationGroupContract;
        }

        #region [Manage Profile Sharing Invitation Confirmation]
        public String SaveInvitationConfirmation(ProfileSharingInvitationConfirmation profileSharingInvitationConfirmation)
        {
            _sharedDataDBContext.ProfileSharingInvitationConfirmations.AddObject(profileSharingInvitationConfirmation);
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return AppConsts.AG_SAVED_SUCCESS_MSG;
            }
            else
            {
                return AppConsts.AG_SAVED_ERROR_MSG;
            }
        }

        public bool MarkIsViewedByInvitationConfirmationId(Int32 profileSharingInvitationConfirmationId, int currentUserId)
        {
            var invitationConfirmation = _sharedDataDBContext.ProfileSharingInvitationConfirmations.Where(cond => cond.PSIC_ID == profileSharingInvitationConfirmationId).SingleOrDefault();
            if (invitationConfirmation.IsNotNull())
            {
                invitationConfirmation.PSIC_IsViewed = true;
                invitationConfirmation.PSIC_ModifiedByID = currentUserId;
                invitationConfirmation.PSIC_ModifiedOn = DateTime.Now;
                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }
            return false;
        }

        public List<ProfileSharingInvitationConfirmation> GetProfileSharingInvitationConfirmations(int currentUserId)
        {
            return (_sharedDataDBContext.ProfileSharingInvitationConfirmations.
                    Where(cond => cond.PSIC_IsViewed != true && cond.PSIC_InvitationInitiatedByID == currentUserId).ToList());
        }

        public bool IsNeedToStartPolling(int currentUserId)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@OrganizationUserID",currentUserId)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_IsNeedToStartPolling", sqlParameterCollection))
                {
                    while (dr.Read())
                    {
                        return dr["StartPolling"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["StartPolling"]);
                    }
                }
            }
            return false;
        }

        #endregion

        #region UAT 1616: WB: As an agency user, I should be able to manage my agency's attestation statement.
        List<AgencyContract> IProfileSharingRepository.GetAttestationReportTextForAgencyUser(Guid userID)
        {
            //return (_sharedDataDBContext.AgencyUsers
            //        .Join(
            //                _sharedDataDBContext.UserAgencyMappings
            //                , au => au.AGU_ID
            //                , uam => uam.UAM_AgencyUserID
            //                , (au, uam) => new { au, uam }
            //               )
            //        .Join(
            //                _sharedDataDBContext.Agencies
            //                , uam_ => uam_.uam.UAM_AgencyID
            //                , ag => ag.AG_ID
            //                , (uam_, ag) => new { uam_, ag }
            //              )
            //        .Where(w => w.uam_.uam.UAM_IsVerified == true
            //                && w.uam_.uam.UAM_IsDeleted == false
            //                && w.uam_.au.AGU_IsDeleted == false
            //                && w.ag.AG_IsDeleted == false
            //                && w.uam_.au.AGU_UserID == userID)
            //        .Select(s => new AgencyContract
            //        {
            //            AgencyID = s.ag.AG_ID,
            //            Name = s.ag.AG_Name,
            //            Description = s.ag.AG_Description,
            //            AttestationReportText = s.ag.AG_AttestationReportText
            //        }).ToList());
            //&& !String.IsNullOrEmpty(w.ag.AG_AttestationReportText)

            return (_sharedDataDBContext.UserAgencyMappings.Where(con => con.AgencyUser.AGU_UserID == userID
                                                                    && !con.UAM_IsDeleted
                                                                    && !con.AgencyUser.AGU_IsDeleted
                                                                    && con.UAM_IsVerified
                                                                    && !con.UAM_IsDeleted
                                                                    && !con.Agency.AG_IsDeleted)
                                                            .Select(s => new AgencyContract
                                                            {
                                                                AgencyID = s.Agency.AG_ID,
                                                                Name = s.Agency.AG_Name,
                                                                Description = s.Agency.AG_Description,
                                                                AttestationReportText = s.Agency.AG_AttestationReportText
                                                            }).ToList());

        }

        String IProfileSharingRepository.UpdateAttestationReportTextForAgencyUser(Int32 loggedInUserID, Int32 agencyId, String attestationReportText)
        {
            Agency agency = _sharedDataDBContext.Agencies.Where(x => x.AG_ID == agencyId && !x.AG_IsDeleted).FirstOrDefault();
            if (agency.IsNotNull())
            {
                agency.AG_AttestationReportText = attestationReportText.IsNullOrEmpty() ? null : attestationReportText;
                agency.AG_IsDeleted = false;
                agency.AG_ModifiedByID = loggedInUserID;
                agency.AG_ModifiedOn = DateTime.Now;
                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return "Attestation statement updated successfully.";
                }
            }
            return String.Empty;
        }
        #endregion

        public bool SaveUpdateSharedUserInvitationReviewStatus(List<Int32> lstProfileSharingInvitationIds, Int32 currentLoggedInUserId, Int32 inviteeOrgUserId,
                                                               Int32 selectedInvitationReviewStatusId, String agencyUserNotes = null
                                                               , bool isAllCorrespondingInvitationsUpdate = false, bool needToChangeStatusAsPending = true,
                                                                 Int32 applicantId = 0, Int32 rotationId = 0, Int32 tenantID = 0, List<lkpAuditChangeType> lstAuditChangeType = null
                                                               , Boolean isAgencyScreen = false, String selectedInvitationReviewStatusCode = null, Boolean isAdminLoggedIn = false)
        {
            //For UAT-2463 , Get all the ids that should be updated when one agency user changes invitation status
            string invitationIds = string.Join(",", lstProfileSharingInvitationIds.Select(n => n.ToString()).ToArray());

            List<int> lstInvitationIdsToBeUpdated = new List<int>();

            if (isAllCorrespondingInvitationsUpdate)
            {
                lstInvitationIdsToBeUpdated = GetInvitationIdsToBeUpdated(invitationIds);
            }

            //else
            //{
            //    lstInvitationIdsToBeUpdated = lstProfileSharingInvitationIds;
            //}

            if (!lstInvitationIdsToBeUpdated.IsNullOrEmpty())
            {
                lstProfileSharingInvitationIds.AddRange(lstInvitationIdsToBeUpdated);

                lstProfileSharingInvitationIds = lstProfileSharingInvitationIds.Distinct().ToList();
            }

            List<SharedUserInvitationReview> lstSharedUserInvitationReview = _sharedDataDBContext.SharedUserInvitationReviews.Where(cond => lstProfileSharingInvitationIds.Contains(cond.SUIR_ProfileSharingInvitationID) && !cond.SUIR_IsDeleted).ToList();


            //UAT-2544:
            String reviewStatus = String.Empty;
            Int32 reviewStatusId = AppConsts.NONE;
            Int32? lastReviewedByUserID = null;
            if (!needToChangeStatusAsPending)
            {
                List<Int32> lstTempPSIIds = new List<Int32>();
                lstTempPSIIds = _sharedDataDBContext.ProfileSharingInvitations.Where(x => !lstProfileSharingInvitationIds.Contains(x.PSI_ID) && x.PSI_ApplicantOrgUserID == applicantId
                                                                                     && !x.PSI_IsDeleted && x.ProfileSharingInvitationGroup.PSIG_ClinicalRotationID == rotationId
                                                                                     && !x.ProfileSharingInvitationGroup.PSIG_IsDeleted
                                                                                     && x.ProfileSharingInvitationGroup.PSIG_TenantID == tenantID
                                                                                     && x.lkpInvitationStatu != null
                                                                                     && x.lkpInvitationStatu.Code != "AAAG"
                                                                                     && x.lkpInvitationStatu.Code != "AAAD"
                                                                                     && x.lkpInvitationStatu.Code != "AAAE"
                                                                                     ).Select(slct => slct.PSI_ID).ToList();
                List<SharedUserInvitationReview> lstTempGetPrevInvStatus = _sharedDataDBContext.SharedUserInvitationReviews.Where(x => lstTempPSIIds.Contains(x.SUIR_ProfileSharingInvitationID)
                                                                                                && !x.SUIR_IsDeleted)
                                                                                                .OrderByDescending(ord => ord.SUIR_ModifiedOn.HasValue ? ord.SUIR_ModifiedOn : ord.SUIR_CreatedOn).ToList();

                if (!lstTempGetPrevInvStatus.IsNullOrEmpty()
                 && !lstTempGetPrevInvStatus.FirstOrDefault().lkpSharedUserInvitationReviewStatu.IsNullOrEmpty())
                {
                    reviewStatus = lstTempGetPrevInvStatus.FirstOrDefault().lkpSharedUserInvitationReviewStatu.SUIRS_Code;
                    reviewStatusId = lstTempGetPrevInvStatus.FirstOrDefault().SUIR_ReviewStatusID;
                    if (String.Compare(reviewStatus, SharedUserInvitationReviewStatus.APPROVED.GetStringValue(), true) == AppConsts.NONE)
                    {
                        selectedInvitationReviewStatusId = reviewStatusId;
                        lastReviewedByUserID = lstTempGetPrevInvStatus.FirstOrDefault().SUIR_ReviewByID;
                    }
                }
            }

            //UAT-2511
            List<AgencyUserAuditHistoryDataContract> lstAgencyUserAuditHistory = new List<AgencyUserAuditHistoryDataContract>();
            List<ProfileSharingInvitation> lstProfileSharingInvitation = new List<ProfileSharingInvitation>();
            if (isAgencyScreen)
            {
                lstProfileSharingInvitation = _sharedDataDBContext.ProfileSharingInvitations.Where(x => lstProfileSharingInvitationIds.Contains(x.PSI_ID) && !x.PSI_IsDeleted).ToList();

            }

            foreach (int psiID in lstProfileSharingInvitationIds)
            {
                var sharedUserInvitationReview = lstSharedUserInvitationReview.Where(cond => cond.SUIR_ProfileSharingInvitationID == psiID).SingleOrDefault();

                //UAT-2511
                if (isAgencyScreen)
                {
                    lstAgencyUserAuditHistory.Add(GenerateAuditHistoryDataContract(sharedUserInvitationReview, psiID, lstProfileSharingInvitation, selectedInvitationReviewStatusId
                                                                                   , currentLoggedInUserId, AuditType.INVITATION.GetStringValue(), agencyUserNotes, lstAuditChangeType
                                                                                   , AppConsts.NONE, selectedInvitationReviewStatusCode));
                }
                if (!sharedUserInvitationReview.IsNullOrEmpty())
                {

                    if (isAdminLoggedIn || currentLoggedInUserId == inviteeOrgUserId)
                    {
                        sharedUserInvitationReview.SUIR_ReviewByID = !needToChangeStatusAsPending ? lastReviewedByUserID : inviteeOrgUserId;
                    }

                    sharedUserInvitationReview.SUIR_ReviewStatusID = selectedInvitationReviewStatusId;
                    sharedUserInvitationReview.SUIR_ModifiedByID = currentLoggedInUserId;
                    sharedUserInvitationReview.SUIR_ModifiedOn = DateTime.Now;
                    //UAT-1844:
                    sharedUserInvitationReview.SUIR_Notes = agencyUserNotes;

                }
                else
                {
                    SharedUserInvitationReview _sharedUserInvitationReview = new SharedUserInvitationReview();
                    _sharedUserInvitationReview.SUIR_ProfileSharingInvitationID = psiID;
                    _sharedUserInvitationReview.SUIR_ReviewStatusID = selectedInvitationReviewStatusId;
                    //_sharedUserInvitationReview.SUIR_OrganizationUserID = inviteeOrgUserId;
                    _sharedUserInvitationReview.SUIR_IsDeleted = false;
                    //_sharedUserInvitationReview.SUIR_CreatedByID = currentLoggedInUserId != inviteeOrgUserId ? inviteeOrgUserId : currentLoggedInUserId;
                    _sharedUserInvitationReview.SUIR_CreatedByID = currentLoggedInUserId;
                    _sharedUserInvitationReview.SUIR_CreatedOn = DateTime.Now;
                    //UAT-1844:
                    _sharedUserInvitationReview.SUIR_Notes = agencyUserNotes;

                    if (isAdminLoggedIn)
                        _sharedUserInvitationReview.SUIR_ReviewByID = !needToChangeStatusAsPending ? lastReviewedByUserID : inviteeOrgUserId;

                    var PSI_Details = _sharedDataDBContext.ProfileSharingInvitations.Where(s => s.PSI_ID == psiID).Select(d => new { PSI_ApplicantOrgUserID = d.PSI_ApplicantOrgUserID, ClinicalRotationID = d.ProfileSharingInvitationGroup.PSIG_ClinicalRotationID, InviteeOrgUserID = d.PSI_InviteeOrgUserID }).FirstOrDefault();

                    //Fix issue : Entry goes in SUIR when we have any un-registered agency user and we are sharing the same rotation in which rotation data is not changed
                    if (!PSI_Details.IsNullOrEmpty() && !PSI_Details.InviteeOrgUserID.IsNullOrEmpty() && PSI_Details.InviteeOrgUserID.HasValue)
                    {
                        _sharedUserInvitationReview.SUIR_OrganizationUserID = PSI_Details.InviteeOrgUserID.Value;
                    }
                    else
                    {
                        _sharedUserInvitationReview.SUIR_OrganizationUserID = AppConsts.NONE;
                    }
                    _sharedDataDBContext.SharedUserInvitationReviews.AddObject(_sharedUserInvitationReview);
                }
            }
            //UAT-2511
            if (isAgencyScreen)
            {
                SaveAgencyUserAuditHistory(lstAgencyUserAuditHistory, false);
            }

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            else
                return false;
        }




        #region UAT 1496: WB: Updates to Client admin profile expiration functionality
        public List<ProfileSharingInvitation> GetProfileSharingInvitationByIds(List<Int32> lstSelectedInvitationIds)
        {
            return _sharedDataDBContext.ProfileSharingInvitations.Where(x => lstSelectedInvitationIds.Contains(x.PSI_ID) && !x.PSI_IsDeleted).ToList();
        }

        public bool UpdateViewRemaining()
        {
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region UAT-1796 Enhance Client User Search to also display Agency Users and grid enhancements.
        DataTable IProfileSharingRepository.GetAgencyUserDetailByID(Int32 agencyUserId)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetAgencyUserInformationById", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyUserId", agencyUserId);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }

        List<Agency> IProfileSharingRepository.GetAgenciesByInstitionIDs(List<Int32> lstInstitutionIDs)
        {
            IEnumerable<Int32> lstAgencyIDs = _sharedDataDBContext.AgencyInstitutions.Where(cond => lstInstitutionIDs.Contains(cond.AGI_TenantID.Value) && !cond.AGI_IsDeleted).Select(x => x.AGI_AgencyID.Value);
            return _sharedDataDBContext.Agencies.Where(x => lstAgencyIDs.Contains(x.AG_ID) && !x.AG_IsDeleted).ToList();
        }
        #endregion

        #region UAT-1641:
        /// <summary>
        /// Method to Get Agencies mapped with institute
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        List<Agency> IProfileSharingRepository.GetInstitutionMappedAgency(List<Int32> institutionIDs, String userID)
        {
            Guid agencyUserID = new Guid(userID);
            List<Int32> userAgencyInstitutionID = new List<Int32>();
            List<AgencyUserInstitution> lstAgencyUserInst = _sharedDataDBContext.AgencyUserInstitutions.Where(x => x.UserAgencyMapping.AgencyUser.AGU_UserID == agencyUserID
                                                         && !x.UserAgencyMapping.AgencyUser.AGU_IsDeleted && !x.UserAgencyMapping.UAM_IsDeleted && !x.AGUI_IsDeleted
                                                         && x.UserAgencyMapping.UAM_IsVerified
                                                         ).ToList();

            if (!lstAgencyUserInst.IsNullOrEmpty())
            {
                userAgencyInstitutionID.AddRange(lstAgencyUserInst.Select(slct => slct.AGUI_AgencyInstitutionID.Value));
            }

            List<Agency> lstAgency = _sharedDataDBContext.AgencyInstitutions.Where(cond => institutionIDs.Contains(cond.AGI_TenantID.Value) && !cond.AGI_IsDeleted
                                                                                           && userAgencyInstitutionID.Contains(cond.AGI_ID)
                                                                                   ).Select(x => x.Agency).ToList();
            if (!lstAgency.IsNullOrEmpty())
            {
                return lstAgency.DistinctBy(dst => dst.AG_ID).OrderBy(ord => ord.AG_Name).ToList();
            }
            return new List<Agency>();
        }

        Boolean IProfileSharingRepository.AnyApplicantSharingExist(Int32 orgUserId, Int32 invitationSourceTypeID, List<Int32> institutionIDs)
        {
            return _sharedDataDBContext.ProfileSharingInvitations.Any(cond => cond.PSI_InviteeOrgUserID == orgUserId && cond.PSI_InvitationSourceID == invitationSourceTypeID
                                                                  && !cond.PSI_IsDeleted && institutionIDs.Contains(cond.PSI_TenantID));
        }
        #endregion

        Boolean IProfileSharingRepository.UpdateAgencyUserAgenciesVerificationCode(Int32 agencyUserID, String verificationCode, Entity.OrganizationUser orgUser)
        {
            Guid verificationCodeGuid = new Guid(verificationCode);
            List<UserAgencyMapping> lstUserAgencyMapping = _sharedDataDBContext.UserAgencyMappings
                            .Where(cond => !cond.UAM_IsDeleted && cond.UAM_VerificationCode == verificationCodeGuid && !cond.UAM_IsVerified)
                            .ToList();

            if (!lstUserAgencyMapping.IsNullOrEmpty() && lstUserAgencyMapping.FirstOrDefault().AgencyUser.AGU_UserID == orgUser.aspnet_Users.UserId)
            {
                foreach (UserAgencyMapping agencyMapping in lstUserAgencyMapping)
                {
                    agencyMapping.UAM_IsVerified = true;
                    agencyMapping.UAM_ModifiedBy = orgUser.OrganizationUserID;
                    agencyMapping.UAM_ModifiedOn = DateTime.Now;
                }
                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get All Agencies for an institution
        /// </summary>
        /// <param name="InstitutionID"></param>
        /// <returns></returns>
        List<Agency> IProfileSharingRepository.GetAgenciesFromAllTenants()
        {
            List<Agency> lstAgency = _sharedDataDBContext.AgencyInstitutions
                                                          .Where(cond => !cond.AGI_IsDeleted).Select(x => x.Agency)
                                                          .Distinct()
                                                          .ToList();
            if (lstAgency.IsNotNull())
                return lstAgency;
            return new List<Agency>();
        }

        #region UAT-1844:
        String IProfileSharingRepository.GetRotationSharedReviewStatus(Int32 clinicalRotationID, Int32 orgUserId, Int32 tenantId, Int32 agencyID, ref Int32? lastReviewedByID)
        {
            List<Int32> invitationIds = new List<Int32>();
            String rotationReviewStatusCode = SharedUserRotationReviewStatus.PENDING_REVIEW.GetStringValue();
            Boolean isAnyPendingReview = false;
            Boolean isAnyNotApproved = false;
            Boolean isAnyApproved = false; //UAT-4460
            Boolean isAllDropped = false;
            var ProfileSharingInvitationGroupsList = _sharedDataDBContext.ProfileSharingInvitationGroups.Where(cnd => cnd.PSIG_ClinicalRotationID == clinicalRotationID
                                                        && cnd.PSIG_TenantID == tenantId
                                                        && cnd.PSIG_AgencyID == agencyID
                                                        && !cnd.PSIG_IsDeleted).ToList();

            if (!ProfileSharingInvitationGroupsList.IsNullOrEmpty())
            {
                var invitationIdsTemp = ProfileSharingInvitationGroupsList.Select(slct => slct.ProfileSharingInvitations.Where(cond => cond.PSI_InviteeOrgUserID == orgUserId
                                                                           && !cond.PSI_IsDeleted && cond.lkpInvitationStatu.Code != "AAAG"
                    //  && (cond.PSI_IsInstructorShare == null || !cond.PSI_IsInstructorShare.Value)//UAT-3977
                                                                           && cond.lkpInvitationStatu.Code != "AAAD"
                                                                           && cond.lkpInvitationStatu.Code != "AAAE").Select(x => x.PSI_ID)).ToList();
                invitationIdsTemp.ForEach(x =>
                {
                    invitationIds.AddRange(x);
                });
                List<SharedUserInvitationReview> lstSharedUserInvitationreview = _sharedDataDBContext.SharedUserInvitationReviews.Where(cnd => invitationIds.Contains(cnd.SUIR_ProfileSharingInvitationID)
                                                                                 && !cnd.SUIR_IsDeleted && cnd.SUIR_OrganizationUserID == orgUserId).ToList();
                if (!lstSharedUserInvitationreview.IsNullOrEmpty())
                {
                    isAnyNotApproved = lstSharedUserInvitationreview.Any(x => x.lkpSharedUserInvitationReviewStatu.SUIRS_Code == "AAAC" && !x.SUIR_IsDeleted);
                    isAnyPendingReview = lstSharedUserInvitationreview.Any(x => x.lkpSharedUserInvitationReviewStatu.SUIRS_Code == "AAAA" && !x.SUIR_IsDeleted);
                    isAnyApproved = lstSharedUserInvitationreview.Any(x => x.lkpSharedUserInvitationReviewStatu.SUIRS_Code == SharedUserInvitationReviewStatus.APPROVED.GetStringValue() && !x.SUIR_IsDeleted); //UAT-4460
                    isAllDropped = lstSharedUserInvitationreview.All(x=>x.lkpSharedUserInvitationReviewStatu.SUIRS_Code == SharedUserInvitationReviewStatus.Dropped.GetStringValue() && !x.SUIR_IsDeleted); //UAT-4460)
                    lastReviewedByID = GetLastReviewedByUserID(clinicalRotationID, tenantId, agencyID, orgUserId);

                    if ((isAnyPendingReview || (lstSharedUserInvitationreview.Count != invitationIds.Distinct().Count())))
                    {
                        rotationReviewStatusCode = SharedUserRotationReviewStatus.PENDING_REVIEW.GetStringValue();
                    }
                    //UAT-4460
                    //else if (isAnyNotApproved && lstSharedUserInvitationreview.Count == invitationIds.Distinct().Count())
                    //{
                    //    rotationReviewStatusCode = SharedUserRotationReviewStatus.NOT_APPROVED.GetStringValue();
                    //}
                    else if (isAnyApproved && lstSharedUserInvitationreview.Count == invitationIds.Distinct().Count())
                    {
                        rotationReviewStatusCode = SharedUserRotationReviewStatus.APPROVED.GetStringValue();
                    }
                    else if (isAllDropped && lstSharedUserInvitationreview.Count == invitationIds.Distinct().Count())
                    {
                        rotationReviewStatusCode = SharedUserRotationReviewStatus.Dropped.GetStringValue();
                    }
                    else if (!isAnyApproved && !isAnyPendingReview && lstSharedUserInvitationreview.Count == invitationIds.Distinct().Count())
                    {
                        rotationReviewStatusCode = SharedUserRotationReviewStatus.NOT_APPROVED.GetStringValue();
                    }
                   
                    //End UAT
                }
            }
            return rotationReviewStatusCode;
        }
        #endregion

        public List<RequirementSharesDataContract> GetRequirementSharesData(String userId, Int32 currentLoggedInUserId, String tenantIds,
                ClinicalRotationDetailContract clinicalRotationSearchContract, InvitationSearchContract invitationSearchContract, CustomPagingArgsContract gridCustomPaging, String customAttributeXML)
        {
            List<RequirementSharesDataContract> lstRequirementSharesDataContract = new List<RequirementSharesDataContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@UserID",userId),
                             new SqlParameter("@LoggedInOrgUserID", currentLoggedInUserId),
                             new SqlParameter("@TenantIDs", tenantIds),
                             new SqlParameter("@CRSearchContract", clinicalRotationSearchContract.XML),
                             new SqlParameter("@InvitationXmlData", invitationSearchContract.XML),
                             new SqlParameter("@PagingSortingData", gridCustomPaging.XML),
                             new SqlParameter("@DroppedStatus", AppConsts.APPLICANT_DROPPED_STATUS),
                             new SqlParameter("@CustomAttributeXML", customAttributeXML.IsNullOrEmpty() ? null: customAttributeXML),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementSharesDataByUsingFlatTables", sqlParameterCollection))
                {
                    int rotationId = 0;
                    int totalRecords = 0;
                    while (dr.Read())
                    {
                        RequirementSharesDataContract requirementSharesDataContract = new RequirementSharesDataContract();
                        rotationId = requirementSharesDataContract.RotationID = dr["ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ID"]);
                        requirementSharesDataContract.ComplioID = dr["ComplioID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ComplioID"]);
                        requirementSharesDataContract.RotationName = dr["Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Name"]);
                        requirementSharesDataContract.Department = dr["Department"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Department"]);
                        requirementSharesDataContract.Program = dr["Program"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Program"]);
                        requirementSharesDataContract.Course = dr["Course"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Course"]);
                        requirementSharesDataContract.StartDate = dr["StartDate"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StartDate"]);
                        requirementSharesDataContract.EndDate = dr["EndDate"] == DBNull.Value ? String.Empty : Convert.ToString(dr["EndDate"]);
                        requirementSharesDataContract.AgencyID = dr["AgencyId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AgencyId"]);
                        requirementSharesDataContract.AgencyName = dr["AgencyName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AgencyName"]);
                        requirementSharesDataContract.Term = dr["Term"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Term"]);
                        requirementSharesDataContract.UnitFloorLoc = dr["UnitFloorLoc"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["UnitFloorLoc"]);
                        requirementSharesDataContract.RecommendedHours = dr["NoOfHours"].GetType().Name == String.Empty ? null : Convert.ToString(dr["NoOfHours"]);
                        requirementSharesDataContract.Students = dr["NoOfStudents"].GetType().Name == String.Empty ? null : Convert.ToString(dr["NoOfStudents"]);
                        requirementSharesDataContract.Shift = dr["RotationShift"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationShift"]);
                        requirementSharesDataContract.StartTime = dr["StartTime"] == DBNull.Value ? String.Empty
                                                : DateTime.ParseExact(dr["StartTime"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                        requirementSharesDataContract.EndTime = dr["EndTime"] == DBNull.Value ? String.Empty
                                                : DateTime.ParseExact(dr["EndTime"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture).ToString("hh:mm tt");
                        requirementSharesDataContract.TypeSpecialty = dr["TypeSpeciality"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["TypeSpeciality"]);
                        requirementSharesDataContract.Time = dr["Times"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Times"]);
                        requirementSharesDataContract.Days = dr["Days"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Days"]);
                        requirementSharesDataContract.DaysIdList = dr["DaysList"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["DaysList"]);
                        //requirementSharesDataContract.IsInstructorPreceptorPkgAvailable = dr["IsIPPkgAvailable"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsIPPkgAvailable"]);
                        //requirementSharesDataContract.RotationReviewID = dr["SharedUserRotationReviewId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SharedUserRotationReviewId"]);
                        requirementSharesDataContract.RotationReviewStatusName = dr["SharedUserReviewStatusName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SharedUserReviewStatusName"]);
                        requirementSharesDataContract.TotalRecordCount = dr["ActualTotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ActualTotalCount"]);
                        requirementSharesDataContract.InvitationID = dr["InvitationId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["InvitationId"]);
                        requirementSharesDataContract.ApplicantName = dr["ApplicantName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                        requirementSharesDataContract.EmailAddress = dr["EmailAddress"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["EmailAddress"]);
                        requirementSharesDataContract.Phone = dr["Phone"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Phone"]);
                        requirementSharesDataContract.TenantID = dr["TenantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TenantID"]);
                        requirementSharesDataContract.TenantName = dr["TenantName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["TenantName"]);
                        requirementSharesDataContract.ExpirationDate = dr["ExpirationDate"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ExpirationDate"]);
                        requirementSharesDataContract.InvitationDate = dr["InvitationDate"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InvitationDate"]);
                        requirementSharesDataContract.LastViewedDate = dr["LastViewedDate"] == DBNull.Value ? String.Empty : Convert.ToString(dr["LastViewedDate"]);
                        requirementSharesDataContract.ViewsRemaining = dr["ViewsRemaining"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ViewsRemaining"]);
                        requirementSharesDataContract.InviteTypeID = dr["InviteTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["InviteTypeID"]);
                        requirementSharesDataContract.InviteTypeCode = dr["InviteTypeCode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InviteTypeCode"]);
                        requirementSharesDataContract.InviteTypeName = dr["InviteTypeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InviteTypeName"]);
                        requirementSharesDataContract.Notes = dr["Notes"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Notes"]);
                        requirementSharesDataContract.IsInvitationVisible = dr["IsInvitationVisible"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsInvitationVisible"]);
                        requirementSharesDataContract.SharedUserInvitationReviewStatusCode = dr["SharedUserInvitationReviewStatusCode"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SharedUserInvitationReviewStatusCode"]);
                        requirementSharesDataContract.InviteeOrgUserID = dr["InviteeOrgUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["InviteeOrgUserID"]);
                        requirementSharesDataContract.IsProfileShared = dr["IsProfileShared"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsProfileShared"]);
                        requirementSharesDataContract.IsRotationSharing = dr["IsRotationSharing"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsRotationSharing"]);
                        requirementSharesDataContract.IsRotationDropped = dr["IsRotationDropped"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsRotationDropped"]);
                        requirementSharesDataContract.ProfileSharingInvGroupID = dr["ProfileSharingInvGroupID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ProfileSharingInvGroupID"]);
                        requirementSharesDataContract.RotationDroppedDate = dr["RotationDroppedDate"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationDroppedDate"]); //UAT-4460
                        if (requirementSharesDataContract.IsRotationDropped)
                            requirementSharesDataContract.ReviewBy = String.Empty;
                        else
                            requirementSharesDataContract.ReviewBy = dr["ReviewBy"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ReviewBy"]);


                        requirementSharesDataContract.RotationSharedByUserName = dr["RotationSharedByUserName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationSharedByUserName"]);
                        requirementSharesDataContract.RotationSharedByUserEmailId = dr["RotationSharedByUserEmailId"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationSharedByUserEmailId"]);

                        requirementSharesDataContract.CustomAttributes = dr["CustomAttributeList"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["CustomAttributeList"]); //UAT-3165
                        requirementSharesDataContract.IsInvSharedByAppByAgencyDDl = dr["IsInvShdByAppByAgencyDDl"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsInvShdByAppByAgencyDDl"]);
                        requirementSharesDataContract.IsIndividualShare = dr["IsIndividualShare"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsIndividualShare"]);

                        //UAT-3955
                        requirementSharesDataContract.InstrDetails = dr["InstDetails"].GetType().Name == "DBNull" ? String.Empty : System.Web.HttpUtility.HtmlDecode(Convert.ToString(dr["InstDetails"]));

                        //UAT 4137
                        /// requirementSharesDataContract.InstructorName = dr["InstructorName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InstructorName"]);

                        //END UAT 3955
                        lstRequirementSharesDataContract.Add(requirementSharesDataContract);
                    }
                }
            }
            return lstRequirementSharesDataContract;
        }

        /// <summary>
        /// UAT 1882:Phase 3(12): When a student profile shares, they should be presented with a selectable list of agencies, which have been associated with nodes they have orders with.
        /// </summary>
        /// <param name="InvitationGroupID"></param>
        /// <returns></returns>
        Boolean IProfileSharingRepository.IsIndividualShared(Int32 profileSharingInvitationID)
        {
            ProfileSharingInvitation profileSharingInvitation = _sharedDataDBContext.ProfileSharingInvitations.Where(x => x.PSI_ID == profileSharingInvitationID && !x.PSI_IsDeleted).FirstOrDefault();
            if (profileSharingInvitation.IsNotNull())
            {
                var isIndividualShared = profileSharingInvitation.ProfileSharingInvitationGroup.PSIG_IsIndividualShare;
                return isIndividualShared.IsNotNull() ? Convert.ToBoolean(isIndividualShared) : false;
            }
            return false;
        }

        Boolean IProfileSharingRepository.HasConsidatePassportPermission(Int32 agencyUserID)
        {
            var agencyUserPermissions = _sharedDataDBContext.AgencyUsers.Where(s => s.AGU_ID == agencyUserID).FirstOrDefault();
            Boolean TrackingAttestionOnly = false;
            Boolean BkgAttestionOnly = false;
            //Boolean RotationAttestionOnly = false;
            if (agencyUserPermissions.IsNotNull())
            {
                var trackingPermissions = agencyUserPermissions.lkpInvitationSharedInfoType2.Code;
                if (trackingPermissions.IsNotNull() && String.Compare(trackingPermissions, AppConsts.PROFILE_SHARING_TRACKING_PERMISSION_CODE, true) == AppConsts.NONE)
                {
                    TrackingAttestionOnly = true;
                }
                var bkgPermissions = agencyUserPermissions.InvitationSharedInfoMappings.Where(d => !d.ISIM_IsDeleted && String.Compare(d.lkpInvitationSharedInfoType.Code, AppConsts.PROFILE_SHARING_BKG_PERMISSION_CODE, true) == AppConsts.NONE).FirstOrDefault();
                if (bkgPermissions.IsNotNull())
                {
                    BkgAttestionOnly = true;
                }
                //var rotationPermissions = agencyUserPermissions.lkpInvitationSharedInfoType.Code;
                //if (rotationPermissions.IsNotNull() && String.Compare(rotationPermissions ,AppConsts.PROFILE_SHARING_ROT_PERMISSION_CODE,true) == AppConsts.NONE)
                //{
                //    RotationAttestionOnly = true;
                //}
                if (TrackingAttestionOnly && BkgAttestionOnly)// && RotationAttestionOnly)
                {
                    return false;
                }
            }
            return true;
        }

        //UAT-2090
        public Boolean SaveInvitationReviewStatusNotes(String InvitationIDs, String clinicalRotationIDs, Int32 selectedInvitationReviewStatusId, String notes, Int32 currentLoggedInUserId, Int32 organisationUserID, Boolean isIndividualReview)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_SaveUpdateUserRotationReviewStatus", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvitationIDs", InvitationIDs);
                command.Parameters.AddWithValue("@ClinicalRotationIDs", clinicalRotationIDs);
                command.Parameters.AddWithValue("@InvitationReviewStatusID", selectedInvitationReviewStatusId);
                command.Parameters.AddWithValue("@Notes", notes);
                command.Parameters.AddWithValue("@ReviewerID", organisationUserID);
                command.Parameters.AddWithValue("@IsIndividualReview", isIndividualReview);
                command.Parameters.AddWithValue("@ModifiedByID", currentLoggedInUserId);

                command.CommandType = CommandType.StoredProcedure;
                con.Open();
                command.ExecuteNonQuery();
                return true;
            }
        }
        #region UAT-2071, Configuration Rotation and Tracking packages must be fully compliant to share
        Boolean IProfileSharingRepository.IsTrackingPkgCompliantReqd(Int32 agencyID, Int32 agencyPrmsnTypeID, List<Int32> AgencyIDs)
        {
            List<AgencyPermission> agencyPermissions = _sharedDataDBContext.AgencyPermissions.Where(cond => AgencyIDs.Contains(cond.AP_AgencyID) // == agencyID
                                                                                            && cond.AP_PermissionTypeID == agencyPrmsnTypeID
                                                                                            && !cond.AP_IsDeleted).ToList();
            if (agencyPermissions.Count > AppConsts.NONE)
            {
                if (agencyPermissions.Where(s => s.lkpAgencyPermissionAccessType.APAT_Code == AgencyPermissionAccessType.YES.GetStringValue()).Any())
                {
                    return true;
                }
                return false;
            }
            return false; //Default Setting for Tracking Package :  Do not check for Compliance
        }

        Dictionary<Int32, String> IProfileSharingRepository.IsRequirementPkgCompliantReqd(String agencyIDs, Int32 agencyPrmsnTypeID)
        {
            List<Int32> agencyIDList = new List<int>();

            foreach (String agID in agencyIDs.Split(',').ToList())
            {
                agencyIDList.Add(Convert.ToInt32(agID));
            }
            List<AgencyPermission> agencyPermissions = _sharedDataDBContext.AgencyPermissions.Where(cond => agencyIDList.Contains(cond.AP_AgencyID)
                                                                                            && cond.AP_PermissionTypeID == agencyPrmsnTypeID
                                                                                            && !cond.AP_IsDeleted).ToList();
            Dictionary<Int32, String> AgencyIdsForPkgCompliantReqd = new Dictionary<int, string>();
            if (agencyPermissions.IsNotNull() && agencyPermissions.Count > 0)
            {
                foreach (AgencyPermission agencyPermission in agencyPermissions)
                {
                    if (agencyPermission.lkpAgencyPermissionAccessType.APAT_Code == AgencyPermissionAccessType.YES.GetStringValue())
                    {
                        AgencyIdsForPkgCompliantReqd.Add(agencyPermission.AP_AgencyID, agencyPermission.Agency.AG_Name);
                    }
                }
            }
            return AgencyIdsForPkgCompliantReqd; //Default Setting for RequirmentPackage : Check for Compliance
        }

        Boolean IProfileSharingRepository.IsOnlyRotationPkgShare(List<Int32> agencyIDs, Int32 agencyPrmsnTypeID)
        {
            List<AgencyPermission> agencyPermissions = _sharedDataDBContext.AgencyPermissions.Where(cond => agencyIDs.Contains(cond.AP_AgencyID) // == agencyID
                                                                                            && cond.AP_PermissionTypeID == agencyPrmsnTypeID
                                                                                            && !cond.AP_IsDeleted).ToList();
            if (agencyPermissions.Count > AppConsts.NONE)
            {
                if (agencyPermissions.Where(s => s.lkpAgencyPermissionAccessType.APAT_Code == AgencyPermissionAccessType.YES.GetStringValue()).Any())
                {
                    return true;
                }
                return false;
            }
            return true; //Default Setting
        }


        Dictionary<Int32, Int32> IProfileSharingRepository.GetAgencyPermisionByAgencyID(int agencyID)
        {
            Dictionary<Int32, Int32> dicAgencyPermissions = new Dictionary<Int32, Int32>();
            List<AgencyPermission> lstAgencyPermissions = _sharedDataDBContext.AgencyPermissions.Where(x => x.AP_AgencyID == agencyID && !x.AP_IsDeleted).ToList();
            if (!lstAgencyPermissions.IsNullOrEmpty())
            {
                foreach (AgencyPermission agencyPermission in lstAgencyPermissions)
                {
                    dicAgencyPermissions.Add(agencyPermission.AP_PermissionTypeID, agencyPermission.AP_PermissionAccessTypeID);
                }
            }
            return dicAgencyPermissions;
        }
        #endregion

        List<Tuple<Int32, Int32, Int32, List<Int32>>> IProfileSharingRepository.GetRotationSharedInvitations(List<InvitationIDsDetailContract> lstClinicalRotations, Int32 inviteeOrgUserId)
        {
            List<Tuple<Int32, Int32, Int32, List<Int32>>> lstData = new List<Tuple<Int32, Int32, Int32, List<int>>>();

            foreach (InvitationIDsDetailContract clinicalRotation in lstClinicalRotations)
            {
                var ProfileSharingInvitationGroupsList = _sharedDataDBContext.ProfileSharingInvitationGroups.Where(cnd => cnd.PSIG_ClinicalRotationID == clinicalRotation.RotationID
                                                    && cnd.PSIG_TenantID == clinicalRotation.TenantID
                                                    && cnd.PSIG_AgencyID == clinicalRotation.AgencyID
                                                      && !cnd.PSIG_IsDeleted).ToList();



                List<Int32> lstClinicalRotationDroppedMembers = new ClinicalRotationRepository(clinicalRotation.TenantID)
                                                                                           .GetDroppedRotationMembersByRotationID(clinicalRotation.RotationID)
                                                                                           .Select(cond => cond.CRM_ApplicantOrgUserID)
                                                                                           .ToList();

                if (!ProfileSharingInvitationGroupsList.IsNullOrEmpty())
                {
                    var invitationIdsTemp = ProfileSharingInvitationGroupsList.Select(slct => slct.ProfileSharingInvitations.Where(cond => cond.PSI_InviteeOrgUserID == inviteeOrgUserId
                                                                               && !cond.PSI_IsDeleted
                                                                               && cond.lkpInvitationStatu.Code != "AAAG"
                                                                               && cond.lkpInvitationStatu.Code != "AAAD"
                                                                               && cond.lkpInvitationStatu.Code != "AAAE"
                                                                               && !lstClinicalRotationDroppedMembers.Contains(cond.PSI_ApplicantOrgUserID)).Select(x => x.PSI_ID)).ToList();
                    List<Int32> invitationIds = new List<Int32>();

                    invitationIdsTemp.ForEach(x =>
                    {
                        invitationIds.AddRange(x);
                    });
                    lstData.Add(new Tuple<Int32, Int32, Int32, List<Int32>>(clinicalRotation.RotationID, clinicalRotation.TenantID, clinicalRotation.AgencyID, invitationIds));
                }
            }

            return lstData;
        }
        //UAT-2181:Enhance adding tenants to agencies with check boxes on the Manage Agencies screen to select which agencies you would like to add the selected tenant to:
        List<Int32> IProfileSharingRepository.SaveAgenciesInstitutionMapping(List<Int32> SelectedAgencyIDs, Int32 TenantID, Int32 CurrentLoggedInUserId)
        {
            List<Int32?> ExistingMapping = _sharedDataDBContext.AgencyInstitutions
                                            .Where(cmd => cmd.AGI_AgencyID.HasValue && SelectedAgencyIDs.Contains(cmd.AGI_AgencyID.Value) && cmd.AGI_TenantID == TenantID && !cmd.AGI_IsDeleted).Select(sel => sel.AGI_AgencyID).ToList();

            SelectedAgencyIDs = SelectedAgencyIDs.Where(cond => !ExistingMapping.Contains(cond)).ToList();

            foreach (Int32 id in SelectedAgencyIDs)
            {
                AgencyInstitution agencyInstitution = new AgencyInstitution();
                agencyInstitution.AGI_AgencyID = id;
                agencyInstitution.AGI_TenantID = TenantID;
                agencyInstitution.AGI_CreatedByID = CurrentLoggedInUserId;
                agencyInstitution.AGI_CreatedOn = DateTime.Now;
                agencyInstitution.AGI_IsDeleted = false;
                _sharedDataDBContext.AgencyInstitutions.AddObject(agencyInstitution);
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return SelectedAgencyIDs;
            }
            else
            {
                return new List<Int32>();
            }
        }
        #region UAT-2452
        Boolean IProfileSharingRepository.SaveAgencyUserSharedPermission(List<AgencyUserSharedProfilePermission> agencyUserSharedPermission)
        {
            agencyUserSharedPermission.ForEach(agencyUserData =>
            {
                _sharedDataDBContext.AgencyUserSharedProfilePermissions.AddObject(agencyUserData);
            });

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        #endregion


        #region UAT 2367
        List<Guid> IProfileSharingRepository.GetAgencyVerificationCode(int agencyUserID)
        {
            List<Guid> lstAgenct = _sharedDataDBContext.UserAgencyMappings.Where(a => a.UAM_AgencyUserID == agencyUserID).OrderByDescending(a => a.UAM_CreatedOn).Select(a => a.UAM_VerificationCode).Take(1).ToList();
            return lstAgenct;
        }
        AgencyUser IProfileSharingRepository.GetAgencyUserByID(int agencyUserID)
        {
            return _sharedDataDBContext.AgencyUsers.Where(a => a.AGU_ID == agencyUserID).FirstOrDefault();
        }
        #endregion

        #region UAT-2463

        private List<int> GetInvitationIdsToBeUpdated(string profileSharingInvitationIds)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            List<int> lstProfileSharingInvitationIds = new List<int>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetInvitationIdsToBeUpdated", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvitationIDs", profileSharingInvitationIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstProfileSharingInvitationIds.Add(Convert.ToInt32(dr["ProfileSharingInvitationId"]));
                    }
                }
            }
            return lstProfileSharingInvitationIds;
        }

        #endregion

        #region UAT-2538

        //Code commented for UAT-2803
        //Boolean IProfileSharingRepository.AssignUnAssignAgencyUsersToSendEmail(List<Int32> selectedAgencyUserIds, Boolean IsNeedToSendEmail, Int32 CurrentLoggedInUserId)
        //{
        //    List<AgencyUser> lstagencyUser = _sharedDataDBContext.AgencyUsers.Where(cmd => selectedAgencyUserIds.Contains(cmd.AGU_ID) && !cmd.AGU_IsDeleted).ToList();
        //    foreach (var item in lstagencyUser)
        //    {
        //        item.AGU_IsNeedToSendEmail = IsNeedToSendEmail;
        //        item.AGU_ModifiedOn = DateTime.Now;
        //        item.AGU_ModifiedByID = CurrentLoggedInUserId;
        //    }
        //    if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
        //        return true;
        //    else
        //    {
        //        return false;
        //    }
        //}


        List<Entity.OrganizationUser> IProfileSharingRepository.GetAgencyUserfromAgency(Int32 AgencyID)
        {
            #region Commented code for UAT-3316
            //List<Int32> AgencyUserIDs = _sharedDataDBContext.UserAgencyMappings.Where(cmd => cmd.UAM_AgencyID == AgencyID && cmd.UAM_IsDeleted == false).Select(sel => sel.UAM_AgencyUserID).ToList();

            ////Code commented for UAT-2803
            //// List<Guid> lstOrgUserIDs = _sharedDataDBContext.AgencyUsers.Where(cmb => AgencyUserIDs.Contains(cmb.AGU_ID) && cmb.AGU_IsDeleted == false && cmb.AGU_UserID != null && cmb.AGU_IsNeedToSendEmail).Select(sel => sel.AGU_UserID.Value).ToList();

            ////UAT-2803
            //String AgencyUserNotificationCode = AgencyUserNotificationLookup.NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION.GetStringValue();
            //Int32 AgencyUserNotificationID = _sharedDataDBContext.lkpAgencyUserNotifications.Where(con => con.AUN_Code == AgencyUserNotificationCode && !con.AUN_IsDeleted)
            //                                                                                    .Select(sel => sel.AUN_ID).FirstOrDefault();

            //List<AgencyUserNotificationMapping> lstAgencyUserNotificationMappingData = _sharedDataDBContext.AgencyUserNotificationMappings.Where(con => AgencyUserIDs.Contains(con.AUNM_AgencyUserID)
            //                                                                            && con.AUNM_NotificationTypeID == AgencyUserNotificationID && !con.AUNM_IsDeleted).ToList();

            //if (!lstAgencyUserNotificationMappingData.IsNullOrEmpty())
            //{
            //    List<Int32> lstAgencyUserWithNotificationPermissions = lstAgencyUserNotificationMappingData.Where(con => con.ANUM_IsMailToBeSend && !con.AUNM_IsDeleted).Select(sel => sel.AUNM_AgencyUserID).ToList();


            //    List<Guid> lstOrgUserIDs = _sharedDataDBContext.AgencyUsers.Where(cmb => lstAgencyUserWithNotificationPermissions.Contains(cmb.AGU_ID) && cmb.AGU_IsDeleted == false && cmb.AGU_UserID != null).Select(sel => sel.AGU_UserID.Value).ToList();

            //    return base.Context.OrganizationUsers.Where(cond => lstOrgUserIDs.Contains(cond.UserID) && cond.IsDeleted == false).ToList();
            //}
            //return new List<Entity.OrganizationUser>(); 
            #endregion

            #region UAT-3116

            List<AgencyUser> lstAgencyUser = _sharedDataDBContext.UserAgencyMappings.Where(cmd => cmd.UAM_AgencyID == AgencyID && cmd.UAM_IsDeleted == false && cmd.AgencyUser.AGU_UserID != null).Select(sel => sel.AgencyUser).ToList();

            List<Guid> lstOrgUserIDs = new List<Guid>();

            if (!lstAgencyUser.IsNullOrEmpty())
            {

                foreach (AgencyUser agencyuser in lstAgencyUser)
                {
                    String AgencyUserNotificationCode = AgencyUserNotificationLookup.NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION.GetStringValue();
                    Int32 AgencyUserNotificationID = _sharedDataDBContext.lkpAgencyUserNotifications.Where(con => con.AUN_Code == AgencyUserNotificationCode && !con.AUN_IsDeleted)
                                                                                                        .Select(sel => sel.AUN_ID).FirstOrDefault();

                    if (!agencyuser.AGU_TemplateId.IsNullOrEmpty() && agencyuser.AGU_TemplateId > AppConsts.NONE)
                    {

                        AgencyUserPerTemplateNotificationMapping agencyUserPerTemplateNotificationMapping = _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.Where(con => con.AGUPTNM_AgencyUserPerTemplateId == agencyuser.AGU_TemplateId.Value
                                              && con.AGUPTNM_NotificationTypeID == AgencyUserNotificationID && !con.AGUPTNM_IsDeleted).FirstOrDefault();

                        if (!agencyUserPerTemplateNotificationMapping.IsNullOrEmpty() && agencyUserPerTemplateNotificationMapping.AGUPTNM_IsMailToBeSend)
                            lstOrgUserIDs.Add(agencyuser.AGU_UserID.Value);

                    }
                    else if (agencyuser.AGU_TemplateId.IsNullOrEmpty() || agencyuser.AGU_TemplateId == AppConsts.NONE)
                    {
                        AgencyUserNotificationMapping agencyUserNotificationMapping = _sharedDataDBContext.AgencyUserNotificationMappings.Where(con => con.AUNM_AgencyUserID == agencyuser.AGU_ID && con.AUNM_NotificationTypeID == AgencyUserNotificationID && !con.AUNM_IsDeleted).FirstOrDefault();
                        if (!agencyUserNotificationMapping.IsNullOrEmpty() && agencyUserNotificationMapping.ANUM_IsMailToBeSend)
                            lstOrgUserIDs.Add(agencyuser.AGU_UserID.Value);
                    }
                }
            }
            if (!lstOrgUserIDs.IsNullOrEmpty())
            {
                return base.Context.OrganizationUsers.Where(cond => lstOrgUserIDs.Contains(cond.UserID) && cond.IsDeleted == false && cond.IsSharedUser == true).ToList();
            }
            return new List<Entity.OrganizationUser>();

            #endregion
        }
        Agency IProfileSharingRepository.GetAgency(Int32 AgencyID)
        {
            return _sharedDataDBContext.Agencies.Where(cond => cond.AG_ID == AgencyID && !cond.AG_IsDeleted).FirstOrDefault();
        }

        List<Int32> IProfileSharingRepository.GetAppOrgOnInvitations(List<Int32> LstInvitationIDs)
        {
            return _sharedDataDBContext.ProfileSharingInvitations.Where(cond => LstInvitationIDs.Contains(cond.PSI_ID) && !cond.PSI_IsDeleted).Select(sel => sel.PSI_ApplicantOrgUserID).ToList();
        }

        List<Entity.OrganizationUser> IProfileSharingRepository.GetClinicalRotationApplicantSharedData(Int32 RotationID, Int32 AgencyID)
        {
            //on basis of rotation and agency id we are having invitations group
            List<Int32> lstPSIG_IDs = _sharedDataDBContext.ProfileSharingInvitationGroups.Where(cond => cond.PSIG_AgencyID == AgencyID && cond.PSIG_ClinicalRotationID == RotationID && !cond.PSIG_IsDeleted).Select(sel => sel.PSIG_ID).ToList();

            //only select those invitations group which are shared. 
            List<Int32> PSIG_IDs_Shared = _sharedDataDBContext.ProfileSharingInvitationConfirmations.Where(cond => lstPSIG_IDs.Contains(cond.PSIC_ProfileSharingInvitationGroupID) && cond.PSIC_IsSuccess).Select(sel => sel.PSIC_ProfileSharingInvitationGroupID).Distinct().ToList();

            List<Int32> lstAppOrgUserID = _sharedDataDBContext.ProfileSharingInvitations.Where(con => PSIG_IDs_Shared.Contains(con.PSI_ProfileSharingInvitationGroupID.Value) && !con.PSI_IsDeleted).Select(sel => sel.PSI_ApplicantOrgUserID).Distinct().ToList();

            //List<Int32> lstAppOrgUserID = _sharedDataDBContext.ProfileSharingInvitations.Where(cond => cond.PSI_ProfileSharingInvitationGroupID == ProfileSharingID).Select(sel => sel.PSI_ApplicantOrgUserID).Distinct().ToList();
            return base.Context.OrganizationUsers.Where(cond => lstAppOrgUserID.Contains(cond.OrganizationUserID) && !cond.IsDeleted).ToList();
        }

        List<Int32> IProfileSharingRepository.GetInvitationIDsIfInvitationStatusChanged(List<Int32> lstRotationInvitations, Int32 selectedInvitationReviewStatusId)
        {
            List<Int32> lstInvitationsID = new List<Int32>();
            foreach (Int32 InvitationID in lstRotationInvitations)
            {
                SharedUserInvitationReviewHistory objSharedUserInvitationReviewHistory = _sharedDataDBContext.SharedUserInvitationReviewHistories.Where(con => con.SUIRH_ProfileSharingInvitationID == InvitationID && !con.SUIRH_IsDeleted).OrderByDescending(t => t.SUIRH_CreatedOn).FirstOrDefault();

                if (objSharedUserInvitationReviewHistory.SUIRH_NewReviewStatusID == selectedInvitationReviewStatusId && objSharedUserInvitationReviewHistory.SUIRH_PreviousReviewStatusID != selectedInvitationReviewStatusId)
                {
                    lstInvitationsID.Add(objSharedUserInvitationReviewHistory.SUIRH_ProfileSharingInvitationID);
                }
            }
            //  Int32 invitationId =objSharedUserInvitationReviewHistory.SUIRH_NewReviewStatusID== selectedInvitationReviewStatusId && SUIRH_PreviousReviewStatusID != selectedInvitationReviewStatusId
            //return _sharedDataDBContext.SharedUserInvitationReviews.Where(cond => lstRotationInvitations.Contains(cond.SUIR_ProfileSharingInvitationID) && cond.SUIR_ReviewStatusID != selectedInvitationReviewStatusId).Select(sel => sel.SUIR_ProfileSharingInvitationID).Distinct().ToList();
            // List<SharedUserInvitationReviewHistory> lstSharedUserInvitationReviewHistory = _sharedDataDBContext.SharedUserInvitationReviewHistories.Where(con => lstRotationInvitations.Contains(con.SUIRH_ProfileSharingInvitationID) && !con.SUIRH_IsDeleted).OrderByDescending(t => t.SUIRH_CreatedOn).Take.ToList();
            //   return lstSharedUserInvitationReviewHistory.Where(con => con.SUIRH_NewReviewStatusID == selectedInvitationReviewStatusId && con.SUIRH_PreviousReviewStatusID != selectedInvitationReviewStatusId).Select(sel => sel.SUIRH_ProfileSharingInvitationID).ToList();
            return lstInvitationsID;
        }
        #endregion

        public Boolean IsApplicantApprovedForRotation(Int32 tenantID, Int32 clinicalRotationID, Int32 applicantOrgUserID)
        {
            Boolean isApproved = false;
            List<Int32> invitationIds = new List<Int32>();

            var ProfileSharingInvitationGroupsList = _sharedDataDBContext.ProfileSharingInvitationGroups
                                                            .Where(cnd => cnd.PSIG_ClinicalRotationID == clinicalRotationID
                                                                            && cnd.PSIG_TenantID == tenantID
                                                                            && !cnd.PSIG_IsDeleted
                                                                   ).ToList();

            var invitationIdsTemp = ProfileSharingInvitationGroupsList.Select(slct => slct.ProfileSharingInvitations
                                                            .Where(cond => cond.PSI_ApplicantOrgUserID == applicantOrgUserID
                                                                          && !cond.PSI_IsDeleted && cond.lkpInvitationStatu.Code != "AAAG"
                                                                          && cond.lkpInvitationStatu.Code != "AAAD"
                                                                          && cond.lkpInvitationStatu.Code != "AAAE").Select(x => x.PSI_ID)
                                                                   ).ToList();

            invitationIdsTemp.ForEach(x =>
            {
                invitationIds.AddRange(x);
            });

            SharedUserInvitationReview sharedUserInvitationReview = _sharedDataDBContext.SharedUserInvitationReviews
                                                           .Where(cnd => invitationIds.Contains(cnd.SUIR_ProfileSharingInvitationID)
                                                                           && !cnd.SUIR_IsDeleted
                                                                  ).FirstOrDefault();

            if (!sharedUserInvitationReview.IsNullOrEmpty())
            {
                string approvedStatusCode = SharedUserInvitationReviewStatus.APPROVED.GetStringValue().ToLower();
                var approvedReviewStatus = _sharedDataDBContext.lkpSharedUserReviewStatus.Where(cond => cond.SURS_Code.ToLower() == approvedStatusCode).FirstOrDefault();
                Int32 approvedStatusID = !approvedReviewStatus.IsNullOrEmpty() ? approvedReviewStatus.SURS_ID : 0;

                if (approvedStatusID == sharedUserInvitationReview.SUIR_ReviewStatusID)
                {
                    isApproved = true;
                }
            }
            return isApproved;
        }

        #region UAT-2554

        public Boolean IsPreceptorRequiredForAgency(Int32 agencyID, Int32 agencyPrmsnTypeID)
        {
            AgencyPermission agencyPermission = _sharedDataDBContext.AgencyPermissions.Where(cond => cond.AP_AgencyID == agencyID
                                                                                            && cond.AP_PermissionTypeID == agencyPrmsnTypeID
                                                                                            && !cond.AP_IsDeleted).FirstOrDefault();
            if (agencyPermission.IsNotNull())
            {
                if (agencyPermission.lkpAgencyPermissionAccessType.APAT_Code == AgencyPermissionAccessType.YES.GetStringValue())
                {
                    return true;
                }
                return false;
            }
            return false; //Default Setting for Preceptor Required
        }
        #endregion
        #region UAT-2529
        Int32 IProfileSharingRepository.GetAgencyInstitutionID(Int32 agencyID, Int32 tenantID)
        {
            return _sharedDataDBContext.AgencyInstitutions.Where(con => con.AGI_AgencyID == agencyID && con.AGI_TenantID == tenantID).Select(sel => sel.AGI_ID).FirstOrDefault();
        }

        Dictionary<Int32, String> IProfileSharingRepository.GetAgencyInstitutionIdsForIndivialSharingPermission(String agencyUseremail, Int32 tenantID)
        {
            Dictionary<Int32, String> agencyInstitutionIds = new Dictionary<Int32, String>();
            Int32 agencyUserId = _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_Email.Equals(agencyUseremail) && !cond.AGU_IsDeleted).Select(f => f.AGU_ID).FirstOrDefault();
            if (!agencyUserId.IsNullOrEmpty())
            {
                var agencyIds = _sharedDataDBContext.UserAgencyMappings.Where(cond => cond.UAM_AgencyUserID == agencyUserId && !cond.UAM_IsDeleted).Select(sel => sel.UAM_AgencyID).ToList();
                if (!agencyIds.IsNullOrEmpty())
                {
                    foreach (var item in agencyIds)
                    {
                        var agencyInstitution = _sharedDataDBContext.AgencyInstitutions.Where(cond => cond.AGI_AgencyID == item && cond.AGI_TenantID == tenantID && !cond.AGI_IsDeleted).OrderByDescending(d => d.AGI_ID).Select(sel => new { AgencyInstitutionId = sel.AGI_ID, AgencyName = sel.Agency.AG_Name }).FirstOrDefault();
                        if (!agencyInstitution.IsNullOrEmpty())
                        {
                            agencyInstitutionIds.Add(agencyInstitution.AgencyInstitutionId, agencyInstitution.AgencyName);
                        }
                    }
                }
            }
            return agencyInstitutionIds;
        }
        #endregion

        #region UAT-2639:Agency hierarchy mapping: Default Hierarchy for Client Admin
        private Int32 SaveAgencyNodeAndHierarchyForAgencyCreation(AgencyContract agencyData, Int32 agencyId)
        {
            if (!agencyData.IsNullOrEmpty() && !agencyData.IsAdmin && agencyId > AppConsts.NONE)
            {
                //List<AgencyUser> lstAgencyUserCreatedByClient = base.SharedDataDBContext.AgencyUsers.Where(cnd => cnd.AGU_CreatedByClient == agencyData.CreatedByTenantID
                //                                                                                          && !cnd.AGU_IsDeleted).ToList();

                //Create Agency Node for newly created agency
                AgencyNode agencyNodeObj = new AgencyNode();
                agencyNodeObj.AN_Name = agencyData.Name;
                agencyNodeObj.AN_Label = agencyData.Name;
                agencyNodeObj.AN_Description = String.Empty;
                agencyNodeObj.AN_CreatedBy = agencyData.LoggedInUserID;
                agencyNodeObj.AN_CreatedOn = DateTime.Now;

                //Create Agency Hierarchy for newly created agency
                AgencyHierarchy agencyHierarchyObj = new AgencyHierarchy();
                //agencyHierarchyObj.AgencyNode = agencyNodeObj;
                agencyHierarchyObj.AH_Label = agencyData.Name;
                agencyHierarchyObj.AH_CreatedBy = agencyData.LoggedInUserID;
                agencyHierarchyObj.AH_CreatedOn = DateTime.Now;
                agencyHierarchyObj.AH_CreatedByClient = agencyData.CreatedByTenantID;

                #region UAt-2712 Default Root Node Rotation Field Settings
                AgencyHierarchyRotationFieldOption dbInsert = new AgencyHierarchyRotationFieldOption();
                dbInsert.AHRFO_CheckParentSetting = false;
                dbInsert.AHRFO_IsCourse_Required = true;
                dbInsert.AHRFO_IsDepartment_Required = true;
                dbInsert.AHRFO_IsProgram_Required = true;
                dbInsert.AHRFO_CreatedOn = DateTime.Now;
                dbInsert.AHRFO_CreatedByID = agencyData.LoggedInUserID;
                dbInsert.AHRFO_IsDeleted = false;
                agencyHierarchyObj.AgencyHierarchyRotationFieldOptions.Add(dbInsert);
                #endregion

                agencyNodeObj.AgencyHierarchies.Add(agencyHierarchyObj);

                //Create Agency Hierarchy for newly created agency
                AgencyHierarchyAgency agencyHierarchyAgencyObj = new AgencyHierarchyAgency();
                agencyHierarchyAgencyObj.AHA_AgencyID = agencyId;
                //agencyHierarchyAgencyObj.AgencyHierarchy = agencyHierarchyObj;
                agencyHierarchyAgencyObj.AHA_CreatedBy = agencyData.LoggedInUserID;
                agencyHierarchyAgencyObj.AHA_CreatedOn = DateTime.Now;
                agencyHierarchyObj.AgencyHierarchyAgencies.Add(agencyHierarchyAgencyObj);

                //Create Agency Hierarchu Tenant Mapping 
                AgencyHierarchyTenantMapping agHierarchyTenantMapping = new AgencyHierarchyTenantMapping();
                agHierarchyTenantMapping.AHTM_TenantID = agencyData.CreatedByTenantID;
                agHierarchyTenantMapping.AHTM_CreatedBy = agencyData.LoggedInUserID;
                agHierarchyTenantMapping.AHTM_CreatedOn = DateTime.Now;
                if (!agencyData.AgencyProfileSharePermission.IsNullOrEmpty())
                {
                    agHierarchyTenantMapping.AHTM_IsStudentShare = agencyData.AgencyProfileSharePermission.IsStudent;
                    agHierarchyTenantMapping.AHTM_IsAdminShare = agencyData.AgencyProfileSharePermission.IsAdmin;
                }
                agencyHierarchyObj.AgencyHierarchyTenantMappings.Add(agHierarchyTenantMapping);

                //if (!lstAgencyUserCreatedByClient.IsNullOrEmpty())
                //{
                //    lstAgencyUserCreatedByClient.ForEach(AGU =>
                //    {
                //        AgencyHierarchyUser agencyHierarchyUserObj = new AgencyHierarchyUser();
                //        agencyHierarchyUserObj.AHU_AgencyUserID = AGU.AGU_ID;
                //        agencyHierarchyUserObj.AHU_CreatedBy = agencyData.LoggedInUserID;
                //        agencyHierarchyUserObj.AHU_CreatedOn = DateTime.Now;
                //        agencyHierarchyUserObj.AHU_IsDeleted = false;
                //        agencyHierarchyObj.AgencyHierarchyUsers.Add(agencyHierarchyUserObj);
                //    });
                //}

                //UAT-2640:
                if (!agencyData.AgencyProfileSharePermission.IsNullOrEmpty())
                {
                    AgencyHierarchyAgencyProfileSharePermission agHrProfileSharePermission = new AgencyHierarchyAgencyProfileSharePermission();
                    agHrProfileSharePermission.AHAPSP_TenantID = agencyData.CreatedByTenantID;
                    agHrProfileSharePermission.AHAPSP_IsDeleted = false;
                    agHrProfileSharePermission.AHAPSP_IsStudentShare = agencyData.AgencyProfileSharePermission.IsStudent;
                    agHrProfileSharePermission.AHAPSP_IsAdminShare = agencyData.AgencyProfileSharePermission.IsAdmin;
                    agHrProfileSharePermission.AHAPSP_CreatedByID = agencyData.LoggedInUserID;
                    agHrProfileSharePermission.AHAPSP_CreatedOn = DateTime.Now;
                    agencyHierarchyObj.AgencyHierarchyAgencyProfileSharePermissions.Add(agHrProfileSharePermission);
                }

                _sharedDataDBContext.AgencyNodes.AddObject(agencyNodeObj);


                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return agencyHierarchyObj.AH_ID;
                }
                return AppConsts.NONE;
            }
            return AppConsts.NONE;
        }

        AgencyHierarchyAgencyProfileSharePermission IProfileSharingRepository.GetAgencyHierarchyProfileSharePermission(Int32 agencyID, Int32 TenantID)
        {
            var agencyData = base.SharedDataDBContext.AgencyHierarchyAgencies.FirstOrDefault(x => x.AHA_AgencyID == agencyID && !x.AHA_IsDeleted && x.AgencyHierarchy.AH_ParentID == null
                                                                      && x.AgencyHierarchy.AH_CreatedByClient == TenantID);

            if (!agencyData.IsNullOrEmpty())
            {
                return agencyData.AgencyHierarchy.AgencyHierarchyAgencyProfileSharePermissions.FirstOrDefault();
            }
            return new AgencyHierarchyAgencyProfileSharePermission();
        }
        #endregion

        #region UAT-2641
        Boolean IProfileSharingRepository.SaveUpdateAgencyHierarchyUserDetails(Int32 agencyUserID, List<Int32> lstAgencyHierarchyIds, Int32 currentLoggedUserID)
        {
            AgencyHierarchyUser agencyHierarchyUser = null;
            List<AgencyHierarchyUser> lstAgencyHierarchyUser = _sharedDataDBContext.AgencyHierarchyUsers.Where(cond => !cond.AHU_IsDeleted
                                                                    && cond.AHU_AgencyUserID == agencyUserID).ToList();

            //List<Int32> previousAgencyHierarchyIds = lstAgencyHierarchyUser.Where(cond => !lstAgencyHierarchyIds.Contains(cond.AHU_AgencyHierarchyID) && !cond.AHU_IsDeleted)
            //                                                .Select(sel => sel.AHU_AgencyHierarchyID).ToList();

            //Remove Un-Mapped Agency Hierarchy User mapping.
            lstAgencyHierarchyUser.Where(cond => !lstAgencyHierarchyIds.Contains(cond.AHU_AgencyHierarchyID) && !cond.AHU_IsDeleted).ForEach(x =>
            {
                x.AHU_IsDeleted = true;
                x.AHU_ModifiedBy = currentLoggedUserID;
                x.AHU_ModifiedOn = DateTime.Now;
            });

            foreach (Int32 agHierchyUserId in lstAgencyHierarchyIds)
            {
                //Add New Agency Hierarchy User Mapping.
                if (!lstAgencyHierarchyUser.Where(cond => cond.AHU_AgencyHierarchyID == agHierchyUserId).Any())
                {
                    agencyHierarchyUser = new AgencyHierarchyUser();
                    agencyHierarchyUser.AHU_AgencyHierarchyID = agHierchyUserId;
                    agencyHierarchyUser.AHU_AgencyUserID = agencyUserID;
                    agencyHierarchyUser.AHU_CreatedBy = currentLoggedUserID;
                    agencyHierarchyUser.AHU_CreatedOn = DateTime.Now;
                    agencyHierarchyUser.AHU_IsDeleted = false;

                    _sharedDataDBContext.AgencyHierarchyUsers.AddObject(agencyHierarchyUser);
                }
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                #region UAT-2637 : Agency hierarchy mapping: Automatic consolidation of user permissions
                foreach (var item in lstAgencyHierarchyIds)
                {
                    EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
                    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        SqlCommand command = new SqlCommand("usp_ConsolidateAgencyUserPermissions", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AgencyHierarchyID", item);
                        command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedUserID);
                        command.Parameters.AddWithValue("@AgencyUserID", agencyUserID);
                        command.ExecuteScalar();
                        con.Close();
                    }
                }


                //if (!previousAgencyHierarchyIds.IsNullOrEmpty())
                //{
                //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                //    {
                //        if (con.State == ConnectionState.Closed)
                //        {
                //            con.Open();
                //        }
                //        SqlCommand command = new SqlCommand("usp_ConsolidateAgencyUserPermissions", con);
                //        command.CommandType = CommandType.StoredProcedure;
                //        command.Parameters.AddWithValue("@AgencyHierarchyID", previousAgencyHierarchyIds.FirstOrDefault());
                //        command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedUserID);
                //        command.Parameters.AddWithValue("@AgencyUserID", agencyUserID);
                //        command.ExecuteScalar();
                //        con.Close();
                //    }
                //}
                #endregion
                return true;
            }
            return false;
        }

        private Boolean DeleteAgencyHierarchyUserDetails(Int32 agencyUserID, Int32 currentLoggedUserID)
        {
            List<AgencyHierarchyUser> lstAgencyHierarchyUser = _sharedDataDBContext.AgencyHierarchyUsers.Where(cond => !cond.AHU_IsDeleted
                                                                    && cond.AHU_AgencyUserID == agencyUserID).ToList();
            //Remove Un-Mapped Agency Hierarchy User mapping.
            lstAgencyHierarchyUser.ForEach(x =>
            {
                x.AHU_IsDeleted = true;
                x.AHU_ModifiedBy = currentLoggedUserID;
                x.AHU_ModifiedOn = DateTime.Now;
            });

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        #endregion
        //UAT-2640
        Boolean IProfileSharingRepository.DeleteAgencyHierarchyAgency(Int32 AgencyId, Int32 CurrentLoggedInUser, Boolean IsAdmin, Int32 TenantID)
        {

            if (!IsAdmin)
            {
                Agency agency = _sharedDataDBContext.Agencies.Where(con => con.AG_ID == AgencyId).FirstOrDefault();
                // if (agency.AG_CreatedByTenantID == CurrentLoggedInUser)
                if (agency.AG_CreatedByTenantID == TenantID)
                {
                    return DeleteHierarchy(AgencyId, CurrentLoggedInUser);
                }
                return false;
            }
            else
            {
                return DeleteHierarchy(AgencyId, CurrentLoggedInUser);
            }
        }
        //UAT-2640
        private Boolean DeleteHierarchy(Int32 AgencyId, Int32 CurrentLoggedInUser)
        {
            List<AgencyHierarchyAgency> lstAlreadyExistsMappigs = _sharedDataDBContext.AgencyHierarchyAgencies.Where(cond => cond.AHA_AgencyID == AgencyId && !cond.AHA_IsDeleted).ToList();
            if (!lstAlreadyExistsMappigs.IsNullOrEmpty())
            {
                foreach (AgencyHierarchyAgency ObjAgencyHierarchyAgency in lstAlreadyExistsMappigs)
                {
                    ObjAgencyHierarchyAgency.AHA_IsDeleted = true;
                    ObjAgencyHierarchyAgency.AHA_ModifiedBy = CurrentLoggedInUser;
                    ObjAgencyHierarchyAgency.AHA_ModifiedOn = DateTime.Now;
                }
                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }
            return true;
        }

        Dictionary<Int32, String> IProfileSharingRepository.GetAgencyHierarchyOfCurrentTenantToAddUser(Int32 tenantId)
        {
            Dictionary<Int32, String> lstAgencyHierarchy = new Dictionary<Int32, String>();
            var agencyHierarchies = _sharedDataDBContext.AgencyHierarchies.Where(cnd => cnd.AH_CreatedByClient == tenantId && !cnd.AH_IsDeleted && cnd.AH_ParentID == null
                                                                                 && cnd.AgencyHierarchyAgencies.Any(x => !x.AHA_IsDeleted && !x.Agency.AG_IsDeleted
                                                                                 && x.Agency.AG_CreatedByTenantID == tenantId)).ToList();

            if (!agencyHierarchies.IsNullOrEmpty())
            {
                agencyHierarchies.ForEach(hrchy =>
                {
                    if (!lstAgencyHierarchy.ContainsKey(hrchy.AH_ID))
                    {
                        var agencyName = hrchy.AgencyHierarchyAgencies.FirstOrDefault().Agency.AG_Name;
                        lstAgencyHierarchy.Add(hrchy.AH_ID, agencyName);
                    }
                });
            }
            return lstAgencyHierarchy;
        }

        Boolean IProfileSharingRepository.IsCurrentLoggedInUser(String CurrentLoggedInOrgUserID, Int32 SelectedAgencyUserID)
        {
            String SelectedAgencyUserOrgUserID = _sharedDataDBContext.AgencyUsers.Where(con => con.AGU_ID == SelectedAgencyUserID && !con.AGU_IsDeleted).Select(sel => sel.AGU_UserID).FirstOrDefault().ToString(); ;
            if (SelectedAgencyUserOrgUserID == CurrentLoggedInOrgUserID)
                return true;
            else
                return false;
        }

        Boolean IProfileSharingRepository.IsAgencyUserOnDifferentNode(Int32 CurrentLoggedInOrgUserID, Int32 SelectedAgencyUserID)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_CheckIfAgencyUserOnDifferentNode", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInOrgUserID", CurrentLoggedInOrgUserID);
                command.Parameters.AddWithValue("@SelectedAgencyUserID", SelectedAgencyUserID);
                command.Parameters.Add("@DisableAllNodes", SqlDbType.Bit);
                command.Parameters["@DisableAllNodes"].Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
                return (Boolean)command.Parameters["@DisableAllNodes"].Value;
            }
        }

        #region UAT-2511:Creation of Agency User Data Audit History screen
        private AgencyUserAuditHistoryDataContract GenerateAuditHistoryDataContract(SharedUserInvitationReview sharedUserInvitationReview, Int32 psi_ID
                                                                                    , List<ProfileSharingInvitation> lstProfileSharingInvitation
                                                                                    , Int32 selectedInvitationReviewStatusId, Int32 currentLoggedInUserID, String auditType
                                                                                    , String notes, List<lkpAuditChangeType> lstAuditChangeType, Int32 rotationID, String newReviewStatusCode, Int32 tenantId = 0
                                                                                    , Int32 agencyId = 0, Int32 oldRotationStatus = 0, String oldRotationNotes = null)
        {
            AgencyUserAuditHistoryDataContract auditHistoryDataContract = new AgencyUserAuditHistoryDataContract();
            List<AgencyUserAuditChangeTypeDataContract> lstAgencyUserChangeTypeData = new List<AgencyUserAuditChangeTypeDataContract>();
            //Invitation && Invitation Detail
            if ((String.Compare(auditType, AuditType.INVITATION.GetStringValue(), true) == AppConsts.NONE)
                 || (String.Compare(auditType, AuditType.INVITATION_DETAIL.GetStringValue(), true) == AppConsts.NONE)
                 || (String.Compare(auditType, AuditType.REQUEST_FOR_AUDIT_INVITATION.GetStringValue(), true) == AppConsts.NONE)
                )
            {
                var profileSharingInvitation = lstProfileSharingInvitation.FirstOrDefault(cnd => cnd.PSI_ID == psi_ID);

                auditHistoryDataContract.AgencyID = profileSharingInvitation.ProfileSharingInvitationGroup.PSIG_AgencyID.IsNullOrEmpty() ? AppConsts.NONE
                                                                             : profileSharingInvitation.ProfileSharingInvitationGroup.PSIG_AgencyID.Value;

                auditHistoryDataContract.RotationID = profileSharingInvitation.ProfileSharingInvitationGroup.PSIG_ClinicalRotationID.IsNullOrEmpty() ? AppConsts.NONE
                                                                               : profileSharingInvitation.ProfileSharingInvitationGroup.PSIG_ClinicalRotationID.Value;

                auditHistoryDataContract.TenantID = profileSharingInvitation.PSI_TenantID;
                auditHistoryDataContract.ProfileSharingInvitationID = profileSharingInvitation.PSI_ID;
                auditHistoryDataContract.CurrentLoggedInUserID = currentLoggedInUserID;

                //Capture audit history: Request for audit invitation
                if (String.Compare(auditType, AuditType.REQUEST_FOR_AUDIT_INVITATION.GetStringValue(), true) == AppConsts.NONE)
                {
                    Boolean oldRequestForAuditValue = false;
                    Boolean newRequestForAuditValue = true;
                    if (!profileSharingInvitation.IsNullOrEmpty() && profileSharingInvitation.PSI_IsExpirationRequested.HasValue)
                    {
                        oldRequestForAuditValue = profileSharingInvitation.PSI_IsExpirationRequested.Value;
                    }

                    //if (oldRequestForAuditValue != newRequestForAuditValue)
                    //{
                    lstAgencyUserChangeTypeData.Add(GenerateAuditChangeTypeDataContract(lstAuditChangeType, AuditChangeType.REQUEST_FOR_AUDIT_INVITATION.GetStringValue()
                                                                                        , oldRequestForAuditValue.ToString(), newRequestForAuditValue.ToString()));
                    //}
                }
                else
                {
                    String oldNotesValue = String.Empty;
                    if (String.Compare(auditType, AuditType.INVITATION.GetStringValue(), true) == AppConsts.NONE)
                    {
                        //Save Status
                        if (selectedInvitationReviewStatusId > AppConsts.NONE)
                        {
                            Int32 oldStatusValue = sharedUserInvitationReview.IsNullOrEmpty() ? AppConsts.NONE : sharedUserInvitationReview.SUIR_ReviewStatusID;
                            //if (selectedInvitationReviewStatusId != oldStatusValue)
                            //{
                            String oldStatusValueString = oldStatusValue.IsNullOrEmpty() ? null : oldStatusValue.ToString();
                            lstAgencyUserChangeTypeData.Add(GenerateAuditChangeTypeDataContract(lstAuditChangeType, AuditChangeType.INVITATION_STATUS.GetStringValue()
                                                            , oldStatusValueString, selectedInvitationReviewStatusId.ToString()));
                            // }
                        }

                        //Save Notes
                        if (!sharedUserInvitationReview.IsNullOrEmpty() && !sharedUserInvitationReview.SUIR_Notes.IsNullOrEmpty())
                        {
                            oldNotesValue = sharedUserInvitationReview.SUIR_Notes;
                        }
                        if (String.Compare(newReviewStatusCode, SharedUserRotationReviewStatus.NOT_APPROVED.GetStringValue(), true) == AppConsts.NONE)
                        {
                            lstAgencyUserChangeTypeData.Add(GenerateAuditChangeTypeDataContract(lstAuditChangeType, AuditChangeType.REJECTION_NOTES.GetStringValue(), oldNotesValue
                                                                                                , notes));

                        }
                    }
                    else if (String.Compare(auditType, AuditType.INVITATION_DETAIL.GetStringValue(), true) == AppConsts.NONE)
                    {
                        if (!profileSharingInvitation.IsNullOrEmpty() && !profileSharingInvitation.PSI_InviteeNotes.IsNullOrEmpty())
                        {
                            oldNotesValue = profileSharingInvitation.PSI_InviteeNotes;
                        }
                        //if (String.Compare(newReviewStatusCode, SharedUserRotationReviewStatus.NOT_APPROVED.GetStringValue(), true) == AppConsts.NONE)
                        //{
                        lstAgencyUserChangeTypeData.Add(GenerateAuditChangeTypeDataContract(lstAuditChangeType, AuditChangeType.INVITATION_NOTES.GetStringValue(), oldNotesValue
                                                                                            , notes));
                        //}
                    }
                }
                auditHistoryDataContract.ListChangeTypeData = lstAgencyUserChangeTypeData;
            }
            //Capture audit history:Rotation changes
            else if (String.Compare(auditType, AuditType.ROTATION.GetStringValue(), true) == AppConsts.NONE)
            {
                auditHistoryDataContract.AgencyID = agencyId;
                auditHistoryDataContract.RotationID = rotationID;
                auditHistoryDataContract.TenantID = tenantId;
                auditHistoryDataContract.CurrentLoggedInUserID = currentLoggedInUserID;

                //Save Status
                if (selectedInvitationReviewStatusId > AppConsts.NONE)
                {
                    //if (selectedInvitationReviewStatusId != oldRotationStatus)
                    //{

                    String oldStatusValue = oldRotationStatus.IsNullOrEmpty() ? null : oldRotationStatus.ToString();
                    lstAgencyUserChangeTypeData.Add(GenerateAuditChangeTypeDataContract(lstAuditChangeType, AuditChangeType.ROTATION_STATUS.GetStringValue()
                                                    , oldStatusValue, selectedInvitationReviewStatusId.ToString()));
                    //}
                }
                //Save Notes
                String oldNotesValue = oldRotationNotes.IsNullOrEmpty() ? String.Empty : oldRotationNotes;
                if (String.Compare(newReviewStatusCode, SharedUserRotationReviewStatus.NOT_APPROVED.GetStringValue(), true) == AppConsts.NONE)
                {
                    lstAgencyUserChangeTypeData.Add(GenerateAuditChangeTypeDataContract(lstAuditChangeType, AuditChangeType.ROTATION_REJECTION_NOTES.GetStringValue(), oldNotesValue
                                                                                        , notes));
                }


                auditHistoryDataContract.ListChangeTypeData = lstAgencyUserChangeTypeData;
            }
            return auditHistoryDataContract;
        }

        private AgencyUserAuditChangeTypeDataContract GenerateAuditChangeTypeDataContract(List<lkpAuditChangeType> lstAuditChangeType, String changeTypeCode, String oldValue
                                                                                          , String newValue)
        {
            AgencyUserAuditChangeTypeDataContract agencyUserChangeTypeData = new AgencyUserAuditChangeTypeDataContract();
            agencyUserChangeTypeData.ChangeTypeID = lstAuditChangeType.FirstOrDefault(x => x.LACT_Code == changeTypeCode && !x.LACT_IsDeleted).LACT_ID;
            agencyUserChangeTypeData.ChangeTypeCode = changeTypeCode;
            agencyUserChangeTypeData.OldValue = oldValue;
            agencyUserChangeTypeData.NewValue = newValue;

            return agencyUserChangeTypeData;
        }

        public Boolean SaveAgencyUserAuditHistory(List<AgencyUserAuditHistoryDataContract> lstAuditDataContract, Boolean isSaveChangesRequired)
        {
            lstAuditDataContract.ForEach(auditData =>
            {
                auditData.ListChangeTypeData.ForEach(changeData =>
                {
                    AgencyUserDataAuditHistory agUserDataAudit = new AgencyUserDataAuditHistory();
                    agUserDataAudit.AUDAH_AgencyID = auditData.AgencyID > AppConsts.NONE ? auditData.AgencyID : (Int32?)null;
                    agUserDataAudit.AUDAH_ClinicalRotationID = auditData.RotationID > AppConsts.NONE ? auditData.RotationID : (Int32?)null;
                    agUserDataAudit.AUDAH_ProfileSharingInvitationID = auditData.ProfileSharingInvitationID > AppConsts.NONE ? auditData.ProfileSharingInvitationID : (Int32?)null;
                    agUserDataAudit.AUDAH_OldValue = changeData.OldValue;
                    agUserDataAudit.AUDAH_NewValue = changeData.NewValue;
                    agUserDataAudit.AUDAH_ChangeTypeID = changeData.ChangeTypeID;
                    agUserDataAudit.AUDAH_InstitutionID = auditData.TenantID;
                    agUserDataAudit.AUDAH_CreatedBy = auditData.CurrentLoggedInUserID;
                    agUserDataAudit.AUDAH_CreatedOn = DateTime.Now;
                    _sharedDataDBContext.AgencyUserDataAuditHistories.AddObject(agUserDataAudit);
                });

            });

            if (isSaveChangesRequired)
            {
                if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;

        }

        Boolean IProfileSharingRepository.SaveRotationAuditHistory(Int32 rotationID, Int32 tenantID, Int32 newReviewStatusID, Int32 oldReviewStatusID, String newNotes, String oldNotes, Int32 currentLoggedInUserID
                                         , List<lkpAuditChangeType> lstAuditChangeType, Int32 agencyId, String newReviewStatusCode)
        {

            List<AgencyUserAuditHistoryDataContract> lstAgencyUserAuditHistory = new List<AgencyUserAuditHistoryDataContract>();
            lstAgencyUserAuditHistory.Add(GenerateAuditHistoryDataContract(null, AppConsts.NONE, null, newReviewStatusID, currentLoggedInUserID, AuditType.ROTATION.GetStringValue(), newNotes, lstAuditChangeType, rotationID, newReviewStatusCode, tenantID, agencyId, oldReviewStatusID));

            return SaveAgencyUserAuditHistory(lstAgencyUserAuditHistory, true);
        }

        List<AgencyUserAuditHistoryDataContract> IProfileSharingRepository.GenerateAuditHistoryDataForRerquestForAudit(List<Int32> profileSharingInvitationIds, Int32 currentLoggedInUserID
                                                                                             , List<lkpAuditChangeType> lstAuditChangeType)
        {
            List<AgencyUserAuditHistoryDataContract> lstAgencyUserAuditHistory = new List<AgencyUserAuditHistoryDataContract>();
            if (!profileSharingInvitationIds.IsNullOrEmpty())
            {
                List<ProfileSharingInvitation> lstProfileSharingInvitation = _sharedDataDBContext.ProfileSharingInvitations.Where(cnd =>
                                                                                                  profileSharingInvitationIds.Contains(cnd.PSI_ID) && !cnd.PSI_IsDeleted).ToList();

                profileSharingInvitationIds.ForEach(invitationId =>
                {
                    lstAgencyUserAuditHistory.Add(GenerateAuditHistoryDataContract(null, invitationId, lstProfileSharingInvitation, AppConsts.NONE, currentLoggedInUserID
                                                                                  , AuditType.REQUEST_FOR_AUDIT_INVITATION.GetStringValue(), String.Empty, lstAuditChangeType
                                                                                  , AppConsts.NONE, String.Empty));
                });
            }
            return lstAgencyUserAuditHistory;
        }

        DataTable IProfileSharingRepository.AgencyUserAuditHistory(Int32 institutionID, Int32 agencyID, string rotationName, string applicantName,
            string updatedByName, DateTime updatedDate, CustomPagingArgsContract customPagingcontract)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                string orderBy = null;
                string ordDirection = null;

                DateTime? updateDate;
                if (updatedDate == DateTime.MinValue)
                {
                    updateDate = null;
                }
                else
                {
                    updateDate = updatedDate;
                }

                orderBy = String.IsNullOrEmpty(customPagingcontract.SortExpression) ? orderBy : customPagingcontract.SortExpression;
                ordDirection = customPagingcontract.SortDirectionDescending == false ? String.IsNullOrEmpty(customPagingcontract.SortExpression) ? "desc" : null : "desc";


                SqlCommand command = new SqlCommand("usp_AgencyUserAuditHistory", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InstitutionID", institutionID);
                command.Parameters.AddWithValue("@AgencyID", agencyID == 0 ? (int?)null : agencyID);
                command.Parameters.AddWithValue("@RotationName", rotationName);
                command.Parameters.AddWithValue("@ApplicantName", applicantName);
                command.Parameters.AddWithValue("@UpdatedByName", updatedByName);
                command.Parameters.AddWithValue("@UpdatedDate", updateDate);
                command.Parameters.AddWithValue("@OrderBy", orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection);
                command.Parameters.AddWithValue("@PageIndex", customPagingcontract.CurrentPageIndex);
                command.Parameters.AddWithValue("@PageSize", customPagingcontract.PageSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }
        #endregion
        #region UAT 2548
        public List<TenantDetailContract> GetAgencyHierarchyMappedTenant(Int32 AgencyUserID)
        {
            List<TenantDetailContract> lstTenantDetailContract = new List<TenantDetailContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@AgencyOrgUserID",AgencyUserID)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyMappedTenant", sqlParameterCollection))
                {
                    while (dr.Read())
                    {
                        TenantDetailContract tenantDetailContract = new TenantDetailContract();
                        tenantDetailContract.TenantID = dr["TenantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TenantID"]);
                        tenantDetailContract.TenantName = dr["TenantName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["TenantName"]);
                        lstTenantDetailContract.Add(tenantDetailContract);
                    }
                }
            }
            return lstTenantDetailContract;
        }

        public List<AgencyDetailContract> GetAgencyUserMappedAgencies(Int32 AgencyOrgUserID)
        {
            List<AgencyDetailContract> lstAgencyDetailContract = new List<AgencyDetailContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@AgencyOrgUserID",AgencyOrgUserID)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyUserMappedAgencies", sqlParameterCollection))
                {
                    while (dr.Read())
                    {
                        AgencyDetailContract agencyDetailContract = new AgencyDetailContract();
                        agencyDetailContract.AgencyID = dr["AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AgencyID"]);
                        agencyDetailContract.AgencyName = dr["AgencyName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AgencyName"]);
                        lstAgencyDetailContract.Add(agencyDetailContract);
                    }
                }
            }
            return lstAgencyDetailContract;
        }
        #endregion

        #region UAT-2706
        Entity.SharedDataEntity.ClientSystemDocument IProfileSharingRepository.GetSharedClientSystemDocument(Int32 clientSystemDocId)
        {
            return _sharedDataDBContext.ClientSystemDocuments.Where(cond => cond.CSD_ID == clientSystemDocId && !cond.CSD_IsDeleted).FirstOrDefault();
        }
        #endregion

        List<Int32> IProfileSharingRepository.AnyAgencyUserExists(Int32 institutionID, String agencyIds)
        {
            // Boolean anyAgencyUserExists = false;
            List<Int32> lstAgencyIds = new List<Int32>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_AnyAgencyUserExistsForAgencies", con);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantID", institutionID);
                command.Parameters.AddWithValue("@AgencyIDs", agencyIds);
                //anyAgencyUserExists = Convert.ToBoolean(command.ExecuteScalar());
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Int32 AgencyID = dr["AgencyID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["AgencyID"]);
                        lstAgencyIds.Add(AgencyID);
                    }
                }
                con.Close();
                return lstAgencyIds;
            }
        }

        RequirementApprovalNotificationDocumentContract IProfileSharingRepository.GetAgencySystemDocument(Int32 agencyID)
        {
            RequirementApprovalNotificationDocumentContract doc = new RequirementApprovalNotificationDocumentContract();

            string docTypeCode = DocumentType.DOCUMENT_FOR_REQUIREMENT_APPROVAL_NOTIFICATION.GetStringValue();

            var agencySystemDocument = _sharedDataDBContext.AgencySystemDocuments
                                        .Where(con => con.ASD_AgencyID == agencyID
                                                    && !con.ClientSystemDocument.CSD_IsDeleted
                                                    && con.ClientSystemDocument.lkpDocumentType.DMT_Code == docTypeCode
                                                    && !con.ASD_IsDeleted).FirstOrDefault();

            if (!agencySystemDocument.IsNullOrEmpty() && agencySystemDocument.ASD_ID > 0)
            {
                doc.FileName = agencySystemDocument.ClientSystemDocument.CSD_FileName;
                doc.Size = agencySystemDocument.ClientSystemDocument.CSD_Size;
                doc.Description = agencySystemDocument.ClientSystemDocument.CSD_Description;
                doc.DocumentPath = agencySystemDocument.ClientSystemDocument.CSD_DocumentPath;
                doc.ClientSystemDocumentID = agencySystemDocument.ASD_ClientSystemDocumentID;
            }

            return doc;
        }

        #region UAT-2774
        List<SharedUserInvitationDocumentContract> IProfileSharingRepository.GetSharedUserInvitationDocumentDetails(Int32 ProfileSharingInvitationID, Int32 ApplicantOrgUserID, Boolean IsRotationSharing)
        {
            List<SharedUserInvitationDocumentContract> lstSharedUserInvitationDocs = new List<SharedUserInvitationDocumentContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSharedUserInvitationDocmentMappingDetails", con);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProfileSharingInvitationID", ProfileSharingInvitationID);
                command.Parameters.AddWithValue("@ApplicantOrgUserID", ApplicantOrgUserID);
                command.Parameters.AddWithValue("@IsRotaionSharing", IsRotationSharing);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SharedUserInvitationDocumentContract docDetails = new SharedUserInvitationDocumentContract();
                        docDetails.ApplicantOrgUserID = dr["ApplicantOrgUserID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["ApplicantOrgUserID"]);
                        docDetails.ProfileSharingInvitationGroupID = dr["ProfileSharingInvitationGroupID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["ProfileSharingInvitationGroupID"]);
                        docDetails.InvitationDocumentID = dr["InvitationDocumentID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["InvitationDocumentID"]);
                        docDetails.FileName = dr["FileName"].IsNullOrEmpty() ? String.Empty : Convert.ToString(dr["FileName"]);
                        docDetails.Description = dr["Description"].IsNullOrEmpty() ? String.Empty : Convert.ToString(dr["Description"]);
                        docDetails.MD5Hash = dr["MD5Hash"].IsNullOrEmpty() ? String.Empty : Convert.ToString(dr["MD5Hash"]);

                        lstSharedUserInvitationDocs.Add(docDetails);
                    }
                }
                con.Close();
                return lstSharedUserInvitationDocs;
            }
        }
        Boolean IProfileSharingRepository.SaveSharedUserInvitationDocumentDetails(List<SharedUserInvitationDocumentMapping> lstSharedUserInvoitationDocs)
        {
            lstSharedUserInvoitationDocs.ForEach(x =>
            {
                _sharedDataDBContext.SharedUserInvitationDocumentMappings.AddObject(x);
            });

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        Boolean IProfileSharingRepository.DeletedSharedUserInvitationDocument(Int32 InvitationDocumentID, Int32 ApplicantOrgUserID, Int32 ProfileSharingInvitationGroupID, Int32 SharedDocTypeID, Int32 CurrentLoggedInUserID)
        {
            SharedUserInvitationDocumentMapping SharedUserInviDocDetails = _sharedDataDBContext.SharedUserInvitationDocumentMappings.Where(cond => !cond.SUIDM_IsDeleted
                                    && cond.SUIDM_ProfileSharingInvitationGroupID == ProfileSharingInvitationGroupID
                                    && cond.SUIDM_ApplicantOrgUserID == ApplicantOrgUserID
                                    && !cond.InvitationDocument.IND_IsDeleted
                                    && cond.InvitationDocument.IND_ID == InvitationDocumentID
                                    && cond.InvitationDocument.IND_DocumentType == SharedDocTypeID).FirstOrDefault();

            if (!SharedUserInviDocDetails.IsNullOrEmpty())
            {
                SharedUserInviDocDetails.SUIDM_IsDeleted = true;
                SharedUserInviDocDetails.SUIDM_ModifiedBy = CurrentLoggedInUserID;
                SharedUserInviDocDetails.SUIDM_ModifiedOn = DateTime.Now;

                InvitationDocument invitationDocument = SharedUserInviDocDetails.InvitationDocument;

                if (!invitationDocument.IsNullOrEmpty())
                {
                    invitationDocument.IND_IsDeleted = true;
                    invitationDocument.IND_ModifiedByID = CurrentLoggedInUserID;
                    invitationDocument.IND_ModifiedOn = DateTime.Now;

                    if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                        return true;
                    return false;
                }
            }
            return false;
        }
        Boolean IProfileSharingRepository.IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 ApplicantOrgUserID, Int32 ProfileSharingInvitationGroupID, Int32 SharedDocTypeID)
        {
            SharedUserInvitationDocumentMapping SharedUserInviDocDetails = _sharedDataDBContext.SharedUserInvitationDocumentMappings.Where(cond => !cond.SUIDM_IsDeleted
                                    && cond.SUIDM_ProfileSharingInvitationGroupID == ProfileSharingInvitationGroupID
                                    && cond.SUIDM_ApplicantOrgUserID == ApplicantOrgUserID
                                    && cond.InvitationDocument.IND_FileName == documentName
                                    && !cond.InvitationDocument.IND_IsDeleted
                                    && cond.InvitationDocument.IND_Size == documentSize
                                    && cond.InvitationDocument.IND_DocumentType == SharedDocTypeID).FirstOrDefault();
            if (SharedUserInviDocDetails.IsNullOrEmpty())
                return false;
            return true;
        }
        #endregion

        Boolean IProfileSharingRepository.GetAgencyUserSSN_Permission(String userID)
        {
            var agencyUser = _sharedDataDBContext.AgencyUsers.Where(s => s.AGU_UserID == new Guid(userID) && !s.AGU_IsDeleted).FirstOrDefault();

            if (!agencyUser.IsNullOrEmpty() && !agencyUser.AGU_AgencyUserPermission.IsNullOrEmpty())
            {
                var agencyUserPerTemplate = _sharedDataDBContext.AgencyUserPermissionTemplates.Where(s => s.AGUPT_ID == agencyUser.AGU_TemplateId && !s.AGUPT_IsDeleted).FirstOrDefault();
                if (agencyUser.AGU_TemplateId.IsNullOrEmpty() || agencyUser.AGU_TemplateId == 0)
                {
                    var agencyUserSSN_Permissions = agencyUser.AgencyUserPermissions.Where(r => !r.AUP_IsDeleted && r.lkpAgencyUserPermissionType.AUPT_Code == AgencyUserPermissionType.GRANULAR_SSN_PERMISSION.GetStringValue()).FirstOrDefault();
                    if (!agencyUserSSN_Permissions.IsNullOrEmpty() && agencyUserSSN_Permissions.AUP_PermissionAccessTypeID == AppConsts.ONE)
                    {
                        return true;
                    }
                }
                else
                {
                    var agencyUserSSN_Permissions = agencyUserPerTemplate.AgencyUserPermissionTemplateMappings.Where(r => !r.AGUPTM_IsDeleted && r.lkpAgencyUserPermissionType.AUPT_Code == AgencyUserPermissionType.GRANULAR_SSN_PERMISSION.GetStringValue()).FirstOrDefault();
                    if (!agencyUserSSN_Permissions.IsNullOrEmpty() && agencyUserSSN_Permissions.AGUPTM_PermissionAccessTypeID == AppConsts.ONE)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        List<RequirementSharesDataContract> IProfileSharingRepository.GetApprovedRotationsAfterSinceLastLogin(Int32 applicantOrgUserID, Int32 tenantID, DateTime lastLoginDate)
        {
            List<RequirementSharesDataContract> lstRotation = new List<RequirementSharesDataContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetApprovedRotationsAfterSinceLastLogin", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicantOrgUserID", applicantOrgUserID);
                command.Parameters.AddWithValue("@LastLoginDate", lastLoginDate);
                command.Parameters.AddWithValue("@TenantID", tenantID);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RequirementSharesDataContract requirementSharesDataContract = new RequirementSharesDataContract();
                        requirementSharesDataContract.AgencyName = Convert.ToString(dr["AgencyName"]);
                        requirementSharesDataContract.RotationID = Convert.ToInt32(dr["ClinicalRotationID"]);
                        requirementSharesDataContract.RotationName = Convert.ToString(dr["RotationName"]);
                        requirementSharesDataContract.Department = Convert.ToString(dr["Department"]);
                        requirementSharesDataContract.Program = Convert.ToString(dr["Program"]);
                        requirementSharesDataContract.Course = Convert.ToString(dr["Course"]);

                        lstRotation.Add(requirementSharesDataContract);
                    }
                }
                return lstRotation;
            }
        }

        #region UAT-2784
        String IProfileSharingRepository.GetAgencySetting(Int32 agencyId, Int32 settingTypeId)
        {
            AgencySetting agencySettings = _sharedDataDBContext.AgencySettings.Where(cond => cond.AS_AgencyID == agencyId
                                        && !cond.AS_IsDeleted && cond.AS_SettingID == settingTypeId).FirstOrDefault();
            if (!agencySettings.IsNullOrEmpty())
            {
                return agencySettings.AS_SettingValue;
            }
            return String.Empty;
        }
        Boolean IProfileSharingRepository.CheckExpirationCriteriaForRotation(List<Int32> lstAgencyIds, Int32 settingTypeId)
        {
            List<AgencySetting> lstAgencySetting = _sharedDataDBContext.AgencySettings.Where(cond => lstAgencyIds.Contains(cond.AS_AgencyID)
                    && !cond.AS_IsDeleted && cond.AS_SettingID == settingTypeId).ToList();

            foreach (AgencySetting agencySetting in lstAgencySetting)
            {
                if (agencySetting.AS_SettingValue == "0")
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        Entity.OrganizationUser IProfileSharingRepository.GetAdminDetailsWhoSharedProfile(Int32 applicantOrgUserID, Int32 clinicalRotationID, Int32 tenantID, Int32 agencyID)
        {
            Entity.OrganizationUser organizationUser = new Entity.OrganizationUser();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAdminDetailsWhoSharedApplicantProfile", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicantOrgUserID", applicantOrgUserID);
                command.Parameters.AddWithValue("@ClinicalRotationID", clinicalRotationID);
                command.Parameters.AddWithValue("@TenantID", tenantID);
                command.Parameters.AddWithValue("@AgencyID", agencyID);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (!ds.IsNullOrEmpty()
                    && !ds.Tables.IsNullOrEmpty()
                    && ds.Tables.Count > 0
                    && !ds.Tables[0].Rows.IsNullOrEmpty()
                    && ds.Tables[0].Rows.Count > 0)
                {
                    var dr = ds.Tables[0].Rows[0];
                    organizationUser.FirstName = Convert.ToString(dr["FirstName"]);
                    organizationUser.LastName = Convert.ToString(dr["LastName"]);
                    organizationUser.PrimaryEmailAddress = Convert.ToString(dr["LoweredEmail"]);
                    organizationUser.OrganizationUserID = Convert.ToInt32(dr["OrganizationUserID"]);
                }
                return organizationUser;
            }
        }

        #region UAT-2803 : Enhance the agency user settings so that we can individually choose what notifications an agency user receives
        List<lkpAgencyUserNotification> IProfileSharingRepository.GetAgencyUserNotifications()
        {
            return _sharedDataDBContext.lkpAgencyUserNotifications.Where(con => con.AUN_IsDeleted == false).ToList();
        }
        Boolean IProfileSharingRepository.SaveAgencyUserNotificationMappings(Int32 agencyUserID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInOrgUserID)
        {
            foreach (var item in dicNotificationData)
            {
                AgencyUserNotificationMapping agencyUserNotificationMapping = new AgencyUserNotificationMapping();
                agencyUserNotificationMapping.AUNM_AgencyUserID = agencyUserID;
                agencyUserNotificationMapping.AUNM_NotificationTypeID = item.Key;
                agencyUserNotificationMapping.ANUM_IsMailToBeSend = item.Value;
                agencyUserNotificationMapping.AUNM_IsDeleted = false;
                agencyUserNotificationMapping.AUNM_CreatedOn = DateTime.Now;
                agencyUserNotificationMapping.AUNM_CreatedBy = CurrentLoggedInOrgUserID;
                _sharedDataDBContext.AgencyUserNotificationMappings.AddObject(agencyUserNotificationMapping);
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        void IProfileSharingRepository.DeleteAgencyUserNotificationMappings(Int32 AgencyUserID, Int32 CurrentLoggedInUserId)
        {
            List<AgencyUserNotificationMapping> lstAgencyUserNotificationMappings = _sharedDataDBContext.AgencyUserNotificationMappings.Where(con => con.AUNM_AgencyUserID == AgencyUserID && !con.AUNM_IsDeleted).ToList();
            if (!lstAgencyUserNotificationMappings.IsNullOrEmpty())
            {
                foreach (AgencyUserNotificationMapping item in lstAgencyUserNotificationMappings)
                {
                    item.AUNM_IsDeleted = true;
                    item.AUNM_ModifiedBy = CurrentLoggedInUserId;
                    item.AUNM_ModifiedOn = DateTime.Now;
                }
                _sharedDataDBContext.SaveChanges();
            }
        }
        Boolean IProfileSharingRepository.UpdateAgencyUserNotificationMappings(Int32 AgencyUserID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInUserId)
        {
            List<AgencyUserNotificationMapping> lstAgencyUserNotificationMappings = _sharedDataDBContext.AgencyUserNotificationMappings
                                                                                            .Where(con => con.AUNM_AgencyUserID == AgencyUserID && !con.AUNM_IsDeleted).ToList();
            if (!lstAgencyUserNotificationMappings.IsNullOrEmpty())
            {
                foreach (AgencyUserNotificationMapping agencyusernotMapping in lstAgencyUserNotificationMappings)
                {
                    foreach (var item in dicNotificationData)
                    {
                        if (item.Key == agencyusernotMapping.AUNM_NotificationTypeID)
                        {
                            agencyusernotMapping.ANUM_IsMailToBeSend = item.Value;
                            agencyusernotMapping.AUNM_ModifiedBy = CurrentLoggedInUserId;
                            agencyusernotMapping.AUNM_ModifiedOn = DateTime.Now;
                        }
                    }
                }

                foreach (var item in dicNotificationData)
                {
                    if (!(lstAgencyUserNotificationMappings.Exists(cond => cond.AUNM_NotificationTypeID == item.Key)))
                    {
                        AgencyUserNotificationMapping agencyUserNotificationMapping = new AgencyUserNotificationMapping();
                        agencyUserNotificationMapping.AUNM_AgencyUserID = AgencyUserID;
                        agencyUserNotificationMapping.AUNM_NotificationTypeID = item.Key;
                        agencyUserNotificationMapping.ANUM_IsMailToBeSend = item.Value;
                        agencyUserNotificationMapping.AUNM_IsDeleted = false;
                        agencyUserNotificationMapping.AUNM_CreatedOn = DateTime.Now;
                        agencyUserNotificationMapping.AUNM_CreatedBy = CurrentLoggedInUserId;
                        _sharedDataDBContext.AgencyUserNotificationMappings.AddObject(agencyUserNotificationMapping);
                    }
                }
            }
            else
            {
                foreach (var item in dicNotificationData)
                {
                    AgencyUserNotificationMapping agencyUserNotificationMapping = new AgencyUserNotificationMapping();
                    agencyUserNotificationMapping.AUNM_AgencyUserID = AgencyUserID;
                    agencyUserNotificationMapping.AUNM_NotificationTypeID = item.Key;
                    agencyUserNotificationMapping.ANUM_IsMailToBeSend = item.Value;
                    agencyUserNotificationMapping.AUNM_IsDeleted = false;
                    agencyUserNotificationMapping.AUNM_CreatedOn = DateTime.Now;
                    agencyUserNotificationMapping.AUNM_CreatedBy = CurrentLoggedInUserId;
                    //agencyUserNotificationMapping.AUNM_ModifiedBy = CurrentLoggedInUserId;
                    //agencyUserNotificationMapping.AUNM_ModifiedOn = DateTime.Now;
                    _sharedDataDBContext.AgencyUserNotificationMappings.AddObject(agencyUserNotificationMapping);
                }
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {

                return true;
            }
            else
            {
                return false;
            }

        }

        List<Int32> IProfileSharingRepository.GetAgencyUserNotificationPermission(List<Int32> lstAgencyUserIds, String AgencyUserNotificationCode)
        {

            Int32 AgencyUserNotificationID = _sharedDataDBContext.lkpAgencyUserNotifications.Where(con => con.AUN_Code == AgencyUserNotificationCode && !con.AUN_IsDeleted)
                                                                                                .Select(sel => sel.AUN_ID).FirstOrDefault();
            List<Int32> lstAgencyUsersNotPresentInAgencyUserNotifyMapping = new List<Int32>();

            foreach (var AgencyUserId in lstAgencyUserIds)
            {
                AgencyUser agencyUser = _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_ID == AgencyUserId && !cond.AGU_IsDeleted).FirstOrDefault();
                if (!agencyUser.IsNullOrEmpty() && !agencyUser.AGU_TemplateId.IsNullOrEmpty() && agencyUser.AGU_TemplateId > AppConsts.NONE)
                {
                    var agencyUserPerTemplateNotificationMapping = _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.Where(cond => !cond.AGUPTNM_IsDeleted
                                              && cond.AGUPTNM_NotificationTypeID == AgencyUserNotificationID && cond.AGUPTNM_AgencyUserPerTemplateId == agencyUser.AGU_TemplateId).FirstOrDefault();
                    if (agencyUserPerTemplateNotificationMapping.IsNullOrEmpty() ||
                        (!agencyUserPerTemplateNotificationMapping.IsNullOrEmpty() && agencyUserPerTemplateNotificationMapping.AGUPTNM_IsMailToBeSend == true))
                    {
                        lstAgencyUsersNotPresentInAgencyUserNotifyMapping.Add(agencyUser.AGU_ID);
                    }
                }
                else if (!agencyUser.IsNullOrEmpty())
                {
                    AgencyUserNotificationMapping AgencyUserNotificationMappingData = _sharedDataDBContext.AgencyUserNotificationMappings.Where(con => con.AUNM_AgencyUserID == agencyUser.AGU_ID
                                                && con.AUNM_NotificationTypeID == AgencyUserNotificationID && !con.AUNM_IsDeleted).FirstOrDefault();
                    if (AgencyUserNotificationMappingData.IsNullOrEmpty() ||
                        (!AgencyUserNotificationMappingData.IsNullOrEmpty() && AgencyUserNotificationMappingData.ANUM_IsMailToBeSend == true))
                    {
                        lstAgencyUsersNotPresentInAgencyUserNotifyMapping.Add(agencyUser.AGU_ID);
                    }
                }
            }
            #region old code
            //List<AgencyUserNotificationMapping> lstAgencyUserNotificationMappingData = _sharedDataDBContext.AgencyUserNotificationMappings.Where(con => lstAgencyUserIds.Contains(con.AUNM_AgencyUserID)
            //                                                                            && con.AUNM_NotificationTypeID == AgencyUserNotificationID && !con.AUNM_IsDeleted).ToList();

            //List<Int32> lstPresentAgencyUsers = lstAgencyUserNotificationMappingData.Select(sel => sel.AUNM_AgencyUserID).ToList();
            //List<Int32> lstAgencyUsersNotPresentInAgencyUserNotifyMapping = lstAgencyUserIds.Except(lstPresentAgencyUsers).ToList();

            //List<Int32> lstAgencyUsersHavingNotifPerm = lstAgencyUserNotificationMappingData.Where(con => con.ANUM_IsMailToBeSend == true).Select(sel => sel.AUNM_AgencyUserID).ToList();
            //if (!lstAgencyUsersNotPresentInAgencyUserNotifyMapping.IsNullOrEmpty())
            //{
            //    lstAgencyUsersNotPresentInAgencyUserNotifyMapping.AddRange(lstAgencyUsersHavingNotifPerm);
            //}
            //else
            //{
            //    lstAgencyUsersNotPresentInAgencyUserNotifyMapping = new List<Int32>();
            //    lstAgencyUsersNotPresentInAgencyUserNotifyMapping.AddRange(lstAgencyUsersHavingNotifPerm);
            //}
            #endregion
            return lstAgencyUsersNotPresentInAgencyUserNotifyMapping;
        }
        Int32 IProfileSharingRepository.GetAgencyUserNotificationPermissionThroughEmailID(String emailID, String AgencyUserNotificationCode)
        {
            #region UAT-3316
            Int32 AgencyUserNotificationID = _sharedDataDBContext.lkpAgencyUserNotifications.Where(con => con.AUN_Code == AgencyUserNotificationCode && !con.AUN_IsDeleted)
                                                                                                       .Select(sel => sel.AUN_ID).FirstOrDefault();

            AgencyUser agencyuser = _sharedDataDBContext.AgencyUsers.Where(cnd => cnd.AGU_Email == emailID.Trim() && !cnd.AGU_IsDeleted).FirstOrDefault();
            if (!agencyuser.IsNullOrEmpty())
            {
                if (!agencyuser.AGU_TemplateId.IsNullOrEmpty() && agencyuser.AGU_TemplateId > AppConsts.NONE)
                {
                    AgencyUserPerTemplateNotificationMapping agencyUserPerTemplateNotificationMapping = _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.Where(con => con.AGUPTNM_AgencyUserPerTemplateId == agencyuser.AGU_TemplateId.Value
                                                  && con.AGUPTNM_NotificationTypeID == AgencyUserNotificationID && !con.AGUPTNM_IsDeleted).FirstOrDefault();
                    if (agencyUserPerTemplateNotificationMapping.AGUPTNM_IsMailToBeSend)
                        return agencyuser.AGU_ID;

                }
                else if (agencyuser.AGU_TemplateId.IsNullOrEmpty() || agencyuser.AGU_TemplateId == AppConsts.NONE)
                {
                    AgencyUserNotificationMapping agencyUserNotificationMapping = _sharedDataDBContext.AgencyUserNotificationMappings.Where(con => con.AUNM_AgencyUserID == agencyuser.AGU_ID && con.AUNM_NotificationTypeID == AgencyUserNotificationID && !con.AUNM_IsDeleted).FirstOrDefault();
                    if (agencyUserNotificationMapping.ANUM_IsMailToBeSend)
                        return agencyuser.AGU_ID;
                }
            }
            return AppConsts.NONE;

            #endregion

            #region code commented for UAT-3316

            //Int32 agencyUserID = _sharedDataDBContext.AgencyUsers.Where(cnd => cnd.AGU_Email == emailID.Trim() && !cnd.AGU_IsDeleted).Select(sel => sel.AGU_ID).FirstOrDefault();
            //if (!agencyUserID.IsNullOrEmpty())
            //{
            //    AgencyUserNotificationMapping agencyUserNotificationMappingData = _sharedDataDBContext.AgencyUserNotificationMappings.Where(con => con.AUNM_AgencyUserID == agencyUserID
            //                                                                                 && con.AUNM_NotificationTypeID == AgencyUserNotificationID && !con.AUNM_IsDeleted).FirstOrDefault();

            //    if (!agencyUserNotificationMappingData.IsNullOrEmpty() && agencyUserNotificationMappingData.ANUM_IsMailToBeSend)
            //    {
            //        return agencyUserID;
            //    }
            //}
            //return AppConsts.NONE;
            #endregion
        }

        Boolean IProfileSharingRepository.IsOrgUserhaveNotificationPermission(Int32 orgUserID, String agencyUserNotificationCode)
        {
            Int32 AgencyUserNotificationID = _sharedDataDBContext.lkpAgencyUserNotifications.Where(con => con.AUN_Code == agencyUserNotificationCode && !con.AUN_IsDeleted)
                                                                                               .Select(sel => sel.AUN_ID).FirstOrDefault();
            Boolean isNotificationallowed = false;
            if (!orgUserID.IsNullOrEmpty())
            {
                Guid userID = base.Context.OrganizationUsers.Where(cond => cond.OrganizationUserID == orgUserID && cond.IsDeleted == false).Select(sel => sel.UserID).FirstOrDefault();
                if (!userID.IsNullOrEmpty())
                {
                    //Int32 agencyUserID = _sharedDataDBContext.AgencyUsers.Where(con => con.AGU_UserID == userID && !con.AGU_IsDeleted).Select(sel => sel.AGU_ID).FirstOrDefault();
                    AgencyUser agencyUser = _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_UserID == userID && !cond.AGU_IsDeleted).FirstOrDefault();
                    if (!agencyUser.AGU_ID.IsNullOrEmpty() && agencyUser.AGU_ID > AppConsts.NONE && !agencyUser.AGU_TemplateId.IsNullOrEmpty() && agencyUser.AGU_TemplateId > AppConsts.NONE)//UAT-4455
                    {
                        isNotificationallowed = _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.Where(con => con.AGUPTNM_AgencyUserPerTemplateId == agencyUser.AGU_TemplateId
                                                                                                                && con.AGUPTNM_NotificationTypeID == AgencyUserNotificationID && !con.AGUPTNM_IsDeleted)
                                                                                                                .Select(sel => sel.AGUPTNM_IsMailToBeSend).FirstOrDefault();
                    }
                    else if (!agencyUser.AGU_ID.IsNullOrEmpty() && agencyUser.AGU_ID > AppConsts.NONE)
                    {
                        isNotificationallowed = _sharedDataDBContext.AgencyUserNotificationMappings.Where(con => con.AUNM_AgencyUserID == agencyUser.AGU_ID
                                                                                                                && con.AUNM_NotificationTypeID == AgencyUserNotificationID && !con.AUNM_IsDeleted)
                                                                                                                .Select(sel => sel.ANUM_IsMailToBeSend).FirstOrDefault();
                    }
                }
            }
            return isNotificationallowed;
        }

        //List<Int32> IProfileSharingRepository.GetAgencyUserIDsFromRotationAgencyUser(Int32 AgencyID)
        //{
        //    return _sharedDataDBContext.UserAgencyMappings.Where(cmd => cmd.UAM_AgencyID == AgencyID && cmd.UAM_IsDeleted == false).Select(sel => sel.UAM_AgencyUserID).ToList();
        //}

        //List<Int32> IProfileSharingRepository.GetOrgUserfromAgencyUsers(List<Int32> lstAgencyIDs)
        //{
        //    List<Guid> lstOrgUserIDs = _sharedDataDBContext.AgencyUsers.Where(cmb => lstAgencyIDs.Contains(cmb.AGU_ID) && cmb.AGU_IsDeleted == false && cmb.AGU_UserID != null).Select(sel => sel.AGU_UserID.Value).ToList();

        //    return base.Context.OrganizationUsers.Where(cond => lstOrgUserIDs.Contains(cond.UserID) && cond.IsDeleted == false).Select(sel => sel.OrganizationUserID).ToList();
        //}



        #endregion

        List<String> IProfileSharingRepository.GetAgencyHierachyAgencyIds(List<Tuple<Int32, Int32>> AgencyHierachyAgencyIds)
        {
            List<Int32> AgencyIds = AgencyHierachyAgencyIds.Select(sel => sel.Item2).ToList();
            var AgencyDetails = _sharedDataDBContext.Agencies.Where(cond => AgencyIds.Contains(cond.AG_ID) && !cond.AG_IsDeleted).ToList();
            if (AgencyDetails.IsNotNull())
            {
                List<String> lstAgencyHierachyAgency = new List<String>();
                AgencyHierachyAgencyIds.ForEach(s => lstAgencyHierachyAgency.Add(String.Concat(s.Item1, "-", AgencyDetails.Where(d => d.AG_ID == s.Item2).Select(sel => sel.AG_Name).FirstOrDefault())));
                return lstAgencyHierachyAgency;
            }
            return new List<String>();
        }

        InvitationDocument IProfileSharingRepository.GetUploadedInvitationDocument(Int32 PSIG_ID, Int32 documentTypeID)
        {
            return _sharedDataDBContext.UploadedInvitationDocuments
                        .Where(cond => !cond.UID_IsDeleted
                                    && cond.UID_ProfileSharingInvitationGroupID == PSIG_ID
                                    && cond.InvitationDocument.IND_DocumentType == documentTypeID
                              )
                        .Select(s => s.InvitationDocument).FirstOrDefault();
        }


        private Int32? GetLastReviewedByUserID(Int32 clinicalRotationID, Int32 tenantID, Int32 agencyID, Int32 agencyUserID)
        {
            Int32? lastReviewedBy = null;
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand command = new SqlCommand("usp_GetReviewedByUserID", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClinicalRotationID", clinicalRotationID);
                command.Parameters.AddWithValue("@TenantId", tenantID);
                command.Parameters.AddWithValue("@AgencyUserId", agencyUserID);
                command.Parameters.AddWithValue("@AgencyID", agencyID);
                var result = command.ExecuteScalar();

                if (!result.IsNullOrEmpty() && result.GetType().Name != "DBNull")
                    lastReviewedBy = (Int32?)result;

                con.Close();
            }
            return lastReviewedBy;
        }

        #region UAT-2942


        List<ApprovedProfileSharingEmailContract> IProfileSharingRepository.GetApprovedProfileInvitationDetailsByIds(List<Int32> lstProfileSharingInvitationId, Int32 selectedInvitationReviewStatusId, Int32 applicantInvitationTypeId)
        {
            List<ApprovedProfileSharingEmailContract> result = new List<ApprovedProfileSharingEmailContract>();
            List<Int32> lstInvitationsID = new List<Int32>();
            foreach (Int32 InvitationID in lstProfileSharingInvitationId)
            {
                AgencyUserDataAuditHistory objAgencyUserDataAuditHistory = _sharedDataDBContext.AgencyUserDataAuditHistories.Where(con => con.AUDAH_ProfileSharingInvitationID.HasValue && con.AUDAH_ProfileSharingInvitationID == InvitationID && !con.AUDAH_IsDeleted).OrderByDescending(t => t.AUDAH_CreatedOn).FirstOrDefault();

                if (objAgencyUserDataAuditHistory.AUDAH_NewValue == selectedInvitationReviewStatusId.ToString() && objAgencyUserDataAuditHistory.AUDAH_OldValue != selectedInvitationReviewStatusId.ToString())
                {
                    lstInvitationsID.Add(Convert.ToInt32(objAgencyUserDataAuditHistory.AUDAH_ProfileSharingInvitationID));
                }
            }
            if (lstInvitationsID.Count > AppConsts.NONE)
            {
                List<ProfileSharingInvitation> profileSharingInvitationList = new List<ProfileSharingInvitation>();
                profileSharingInvitationList = _sharedDataDBContext.ProfileSharingInvitations.Where(cnd => lstInvitationsID.Contains(cnd.PSI_ID) &&
                    !cnd.PSI_IsDeleted && cnd.PSI_InvitationSourceID == applicantInvitationTypeId && (cnd.ProfileSharingInvitationGroup.PSIG_IsIndividualShare == null || cnd.ProfileSharingInvitationGroup.PSIG_IsIndividualShare == false)
                    && cnd.ProfileSharingInvitationGroup.PSIG_TenantID.HasValue && cnd.PSI_InviteeOrgUserID.HasValue
                    ).ToList();
                List<Int32> FinalInvitationsIDs = new List<Int32>();
                FinalInvitationsIDs = profileSharingInvitationList.Select(d => d.PSI_ID).ToList();
                List<Entity.OrganizationUser> GetAllAgencyUserList = new List<Entity.OrganizationUser>();

                #region Getting Full PSI Details
                foreach (var item in profileSharingInvitationList)
                {
                    ApprovedProfileSharingEmailContract objApprovedProfileSharingEmailContract = new ApprovedProfileSharingEmailContract();
                    objApprovedProfileSharingEmailContract.ProfileSharingInvitationID = item.PSI_ID;
                    objApprovedProfileSharingEmailContract.ProfileSharingInvitationDetails = item;

                    #region Getting Agency Details
                    if (!result.Where(s => s.AgencyDetails.AG_ID == item.ProfileSharingInvitationGroup.Agency.AG_ID).Any())
                    {
                        objApprovedProfileSharingEmailContract.AgencyDetails = item.ProfileSharingInvitationGroup.Agency;

                        #region UAT-3316 : `Getting Agency User Details

                        List<AgencyUser> lstAgencyUser = _sharedDataDBContext.UserAgencyMappings.Where(cmd => cmd.UAM_AgencyID == item.ProfileSharingInvitationGroup.PSIG_AgencyID
                            && cmd.UAM_IsDeleted == false && cmd.AgencyUser.AGU_UserID != null).Select(sel => sel.AgencyUser).ToList();

                        List<Guid> lstOrgUserIDs = new List<Guid>();

                        if (!lstAgencyUser.IsNullOrEmpty())
                        {
                            foreach (AgencyUser agencyuser in lstAgencyUser)
                            {
                                String AgencyUserNotificationCode = AgencyUserNotificationLookup.NOTIFICATION_FOR_PROFILE_SHARING_WITH_AGENCY_APPROVED.GetStringValue();
                                Int32 AgencyUserNotificationID = _sharedDataDBContext.lkpAgencyUserNotifications.Where(con => con.AUN_Code == AgencyUserNotificationCode && !con.AUN_IsDeleted)
                                                                                                                    .Select(sel => sel.AUN_ID).FirstOrDefault();
                                if (!agencyuser.AGU_TemplateId.IsNullOrEmpty() && agencyuser.AGU_TemplateId > AppConsts.NONE)
                                {

                                    AgencyUserPerTemplateNotificationMapping agencyUserPerTemplateNotificationMapping = _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.Where(con => con.AGUPTNM_AgencyUserPerTemplateId == agencyuser.AGU_TemplateId.Value
                                                          && con.AGUPTNM_NotificationTypeID == AgencyUserNotificationID && !con.AGUPTNM_IsDeleted).FirstOrDefault();

                                    if (agencyUserPerTemplateNotificationMapping.AGUPTNM_IsMailToBeSend)
                                        lstOrgUserIDs.Add(agencyuser.AGU_UserID.Value);

                                }
                                else if (agencyuser.AGU_TemplateId.IsNullOrEmpty() || agencyuser.AGU_TemplateId == AppConsts.NONE)
                                {
                                    AgencyUserNotificationMapping agencyUserNotificationMapping = _sharedDataDBContext.AgencyUserNotificationMappings.Where(con => con.AUNM_AgencyUserID == agencyuser.AGU_ID && con.AUNM_NotificationTypeID == AgencyUserNotificationID && !con.AUNM_IsDeleted).FirstOrDefault();
                                    if (agencyUserNotificationMapping.ANUM_IsMailToBeSend)
                                        lstOrgUserIDs.Add(agencyuser.AGU_UserID.Value);
                                }
                            }
                            objApprovedProfileSharingEmailContract.GetAgencyUserList = base.Context.OrganizationUsers.Where(cond => lstOrgUserIDs.Contains(cond.UserID) && cond.IsDeleted == false).ToList();
                        }


                        #endregion

                        #region `Getting Agency User Details : Code commented for UAT-3316
                        //List<Int32> AgencyUserIDs = _sharedDataDBContext.UserAgencyMappings.Where(cmd => cmd.UAM_AgencyID == item.ProfileSharingInvitationGroup.PSIG_AgencyID && !cmd.UAM_IsDeleted).Select(sel => sel.UAM_AgencyUserID).ToList();

                        //String AgencyUserNotificationCode = AgencyUserNotificationLookup.NOTIFICATION_FOR_PROFILE_SHARING_WITH_AGENCY_APPROVED.GetStringValue();
                        //Int32 AgencyUserNotificationID = _sharedDataDBContext.lkpAgencyUserNotifications.Where(con => con.AUN_Code == AgencyUserNotificationCode && !con.AUN_IsDeleted)
                        //                                                                                    .Select(sel => sel.AUN_ID).FirstOrDefault();

                        //List<AgencyUserNotificationMapping> lstAgencyUserNotificationMappingData = _sharedDataDBContext.AgencyUserNotificationMappings.Where(con => AgencyUserIDs.Contains(con.AUNM_AgencyUserID)
                        //                                                                            && con.AUNM_NotificationTypeID == AgencyUserNotificationID && !con.AUNM_IsDeleted).ToList();

                        //if (!lstAgencyUserNotificationMappingData.IsNullOrEmpty())
                        //{
                        //    List<Int32> lstAgencyUserWithNotificationPermissions = lstAgencyUserNotificationMappingData.Where(con => con.ANUM_IsMailToBeSend && !con.AUNM_IsDeleted).Select(sel => sel.AUNM_AgencyUserID).ToList();


                        //    List<Guid> lstOrgUserIDs = _sharedDataDBContext.AgencyUsers.Where(cmb => lstAgencyUserWithNotificationPermissions.Contains(cmb.AGU_ID) && cmb.AGU_IsDeleted == false && cmb.AGU_UserID != null).Select(sel => sel.AGU_UserID.Value).ToList();

                        //    objApprovedProfileSharingEmailContract.GetAgencyUserList = base.Context.OrganizationUsers.Where(cond => lstOrgUserIDs.Contains(cond.UserID) && cond.IsDeleted == false).ToList();
                        //}
                        #endregion
                    }
                    else
                    {
                        var res = result.Where(s => s.AgencyDetails.AG_ID == item.ProfileSharingInvitationGroup.Agency.AG_ID).Select(sel => new { AgencyDetails = sel.AgencyDetails, AgencyUserList = sel.GetAgencyUserList }).FirstOrDefault();
                        objApprovedProfileSharingEmailContract.AgencyDetails = res.AgencyDetails;

                        #region `Getting Agency User Details
                        objApprovedProfileSharingEmailContract.GetAgencyUserList = res.AgencyUserList;
                        #endregion
                    }
                    #endregion

                    #region Get Applicant Details
                    objApprovedProfileSharingEmailContract.ApplicantDetails = base.Context.OrganizationUsers.Where(cond => cond.OrganizationUserID == item.PSI_ApplicantOrgUserID && cond.IsDeleted == false).FirstOrDefault();
                    #endregion

                    #region Tenant Details
                    objApprovedProfileSharingEmailContract.TenantID = Convert.ToInt32(item.ProfileSharingInvitationGroup.PSIG_TenantID);
                    objApprovedProfileSharingEmailContract.TenantName = item.ProfileSharingInvitationGroup.TenantName;
                    #endregion

                    #region Getting Rotation Details
                    var ProfileSharingInvitationRotationDetails = item.ProfileSharingInvitationGroup.ProfileSharingInvitationRotationDetails.Where(cnd => !cnd.PSIRD_IsDeleted).FirstOrDefault();
                    if (!ProfileSharingInvitationRotationDetails.IsNullOrEmpty())
                    {
                        ClinicalRotation rotDetails = new ClinicalRotation()
                        {
                            CR_Department = !String.IsNullOrEmpty(ProfileSharingInvitationRotationDetails.PSIRD_Department) ? ProfileSharingInvitationRotationDetails.PSIRD_Department : String.Empty,
                            CR_StartDate = ProfileSharingInvitationRotationDetails.PSIRD_StartDate,
                            CR_EndDate = ProfileSharingInvitationRotationDetails.PSIRD_EndDate,
                        };
                        objApprovedProfileSharingEmailContract.RotationDetails = rotDetails;
                    }
                    #endregion

                    result.Add(objApprovedProfileSharingEmailContract);
                }
                #endregion
            }
            return result;
        }
        #endregion


        #region UAT-2943
        Int32 IProfileSharingRepository.GetReviewStatusIDByProfileSharingInvitationID(Int32 invitationId)
        {
            var _invitationData = _sharedDataDBContext.SharedUserInvitationReviews.Where(suir => suir.SUIR_ProfileSharingInvitationID == invitationId && !suir.SUIR_IsDeleted).FirstOrDefault();

            if (_invitationData.IsNotNull())
            {
                return _invitationData.SUIR_ReviewStatusID;
            }
            String PendingReviewStatusCode = SharedUserRotationReviewStatus.PENDING_REVIEW.GetStringValue();
            return _sharedDataDBContext.lkpSharedUserInvitationReviewStatus.Where(s => s.SUIRS_Code == PendingReviewStatusCode).FirstOrDefault().SUIRS_ID; //Pending Review By Default
        }
        #endregion


        DataSet IProfileSharingRepository.GetAgencyRotationMapping()
        {
            List<AgencyRotationMapping> lstAgencyRotationMapping = new List<AgencyRotationMapping>();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAgencyRotationMapping", con);
                command.CommandType = CommandType.StoredProcedure;

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                con.Close();
                return ds;
            }
        }

        Boolean IProfileSharingRepository.SaveAgencyNotification(Int32 subEventID, String entityName, Int32 entityID, DateTime dataFetchedFromDate, DateTime dataFetchedFromToDate, Int32 createdByID)
        {
            AgencyNotification agencyNotification = new AgencyNotification();
            agencyNotification.AN_EntityID = entityID;
            agencyNotification.AN_EntityName = entityName;
            agencyNotification.AN_DataFetchedFromDate = dataFetchedFromDate;
            agencyNotification.AN_DataFetchedToDate = dataFetchedFromToDate;
            agencyNotification.AN_SubEventTypeID = subEventID;
            agencyNotification.AN_IsDeleted = false;
            agencyNotification.AN_CreatedOn = DateTime.Now;
            agencyNotification.AN_CreatedBy = createdByID;
            _sharedDataDBContext.AgencyNotifications.AddObject(agencyNotification);

            if (_sharedDataDBContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        DateTime? IProfileSharingRepository.GetLastNotificationSentDate(Int32 subEventID, Int32 entityID)
        {
            AgencyNotification agencyNotification = this.SharedDataDBContext.AgencyNotifications
                                                        .Where(cond => cond.AN_SubEventTypeID == subEventID && cond.AN_EntityID == entityID && !cond.AN_IsDeleted)
                                                        .OrderByDescending(cond => cond.AN_DataFetchedToDate)
                                                        .FirstOrDefault();

            if (!agencyNotification.IsNullOrEmpty())
            {
                return agencyNotification.AN_DataFetchedToDate;
            }
            return null;
        }


        List<Tuple<int, List<int>>> IProfileSharingRepository.FilterInvitationIdsByTenant(List<int> lstPSI)
        {
            List<Tuple<int, List<int>>> lstData = new List<Tuple<int, List<int>>>();
            var lstInvitations = this.SharedDataDBContext
                                                        .ProfileSharingInvitations
                                                        .Where(cond => lstPSI.Contains(cond.PSI_ID) && !cond.PSI_IsDeleted)
                                                        .Select(s => new { s.PSI_ID, s.PSI_TenantID })
                                                        .GroupBy(f => f.PSI_TenantID)
                                                        .ToList();


            if (!lstInvitations.IsNullOrEmpty())
            {
                foreach (var item in lstInvitations)
                {
                    lstData.Add(new Tuple<int, List<int>>(item.Key, item.Select(d => d.PSI_ID).ToList()));
                }
            }

            return lstData;
        }

        Int32 IProfileSharingRepository.GetInvitationReviewStatusIDByStatusCode(string reviewStatusCode)
        {
            var reviewStatus = _sharedDataDBContext.lkpSharedUserInvitationReviewStatus.Where(cond => cond.SUIRS_Code == reviewStatusCode && !cond.SUIRS_IsDeleted).FirstOrDefault();

            if (!reviewStatus.IsNullOrEmpty())
                return reviewStatus.SUIRS_ID;
            else
                return AppConsts.NONE;
        }


        List<Int32> IProfileSharingRepository.FilterInvitationIdsByReviewStatusID(List<Int32> lstInvitations, Int32 reviewStatusID)
        {
            List<Int32> lstFilteredInvitations = new List<int>();
            lstFilteredInvitations = _sharedDataDBContext.SharedUserInvitationReviews
                                            .Where(cond => !cond.SUIR_IsDeleted
                                                            && lstInvitations.Contains(cond.SUIR_ProfileSharingInvitationID)
                                                            && cond.SUIR_ReviewStatusID == reviewStatusID
                                                            )
                                            .Select(s => s.SUIR_ProfileSharingInvitationID).ToList();

            return lstFilteredInvitations;
        }

        #region UAT-3108
        List<AgencyUserInfoContract> IProfileSharingRepository.GetAgencyUserListInRotationBasedOnPermission(Int32 RotationID, String NotificationTypeCode, Int32 tenantID)
        {
            List<AgencyUserInfoContract> lstAgencyUserInfoContract = new List<AgencyUserInfoContract>();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAgencyUserListInRotationBasedOnPermission", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RotationID", RotationID);
                command.Parameters.AddWithValue("@NotificationTypeCode", NotificationTypeCode);
                command.Parameters.AddWithValue("@TenantID", tenantID);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AgencyUserInfoContract agencyUserInfoContract = new AgencyUserInfoContract();
                        agencyUserInfoContract.AgencyID = dr["AgencyID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["AgencyID"]);
                        agencyUserInfoContract.OrgUserID = dr["OrganizationUserID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["OrganizationUserID"]);
                        agencyUserInfoContract.AgencyUserID = dr["AgencyUserID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["AgencyUserID"]);
                        agencyUserInfoContract.FirstName = dr["FirstName"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["FirstName"]);
                        agencyUserInfoContract.LastName = dr["LastName"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["LastName"]);
                        agencyUserInfoContract.FullName = dr["AgencyUserName"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["AgencyUserName"]);
                        agencyUserInfoContract.PrimaryEmailAddress = dr["PrimaryEmailAddress"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["PrimaryEmailAddress"]);
                        lstAgencyUserInfoContract.Add(agencyUserInfoContract);
                    }
                }
                return lstAgencyUserInfoContract;
            }
        }

        #endregion

        #region UAT 3102
        Boolean IProfileSharingRepository.UpdateAgencyUserEmailAddress(Int32 agencyUserId, String emailId, Int32 loggedInUserId)
        {
            Boolean IsRecordUpdated = false;
            List<ProfileSharingInvitation> lstFilteredInvitationApplicantSharedUserData = new List<ProfileSharingInvitation>();
            Guid agencyUserUserID = _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_ID == agencyUserId && !cond.AGU_IsDeleted).Select(x => x.AGU_UserID).FirstOrDefault() ?? new Guid();
            if (agencyUserUserID != Guid.Empty)
            {
                Entity.OrganizationUser orgUser = base.Context.OrganizationUsers.Where(cond => cond.UserID == agencyUserUserID && cond.IsSharedUser == true).FirstOrDefault();
                //OrganizationUser redirected to new email address
                if (orgUser != null)
                {
                    orgUser.PrimaryEmailAddress = emailId;
                    orgUser.ModifiedOn = DateTime.Now;
                    orgUser.ModifiedByID = loggedInUserId;
                }

                Entity.aspnet_Membership aspMembership = base.Context.aspnet_Membership.Where(cond => cond.UserId == agencyUserUserID).FirstOrDefault();
                //aspMembership redirected to new email address
                if (aspMembership != null)
                {
                    aspMembership.Email = emailId;
                    aspMembership.LoweredEmail = aspMembership.Email.ToLower();
                }
                //Get ProfileSharingInvitations for Applicant SharedUser
                String applicantSharedUserCode = OrganizationUserType.ApplicantsSharedUser.GetStringValue();
                lstFilteredInvitationApplicantSharedUserData = _sharedDataDBContext.ProfileSharingInvitations
                                     .Where(cond => !cond.PSI_IsDeleted
                                                    && cond.PSI_InviteeOrgUserID.HasValue && cond.PSI_InviteeOrgUserID.Value == orgUser.OrganizationUserID && cond.lkpOrgUserType.OrgUserTypeCode == applicantSharedUserCode).ToList();

                if (base.Context.SaveChanges() > 0)
                    IsRecordUpdated = true;
            }

            AgencyUser agencyUser = _sharedDataDBContext.AgencyUsers.Where(x => x.AGU_ID == agencyUserId && x.AGU_IsDeleted == false).FirstOrDefault();
            //AgencyUser redirected to new email address
            if (agencyUser != null)
            {
                agencyUser.AGU_Email = emailId;
                agencyUser.AGU_ModifiedByID = loggedInUserId;
                agencyUser.AGU_ModifiedOn = DateTime.Now;
            }
            //Get ProfileSharingInvitations for Agency User
            String agencyUserCode = OrganizationUserType.AgencyUser.GetStringValue();
            List<ProfileSharingInvitation> lstFilteredInvitationAgencyUserData = _sharedDataDBContext.ProfileSharingInvitations
                                            .Where(cond => !cond.PSI_IsDeleted
                                                            && cond.PSI_AgencyUserID == agencyUserId && cond.lkpOrgUserType.OrgUserTypeCode == agencyUserCode).ToList();

            lstFilteredInvitationAgencyUserData = lstFilteredInvitationAgencyUserData.Union(lstFilteredInvitationApplicantSharedUserData).ToList();

            if (lstFilteredInvitationAgencyUserData != null && lstFilteredInvitationAgencyUserData.Count > 0)
            {
                //All the existing shares will be redirected to new email address
                foreach (ProfileSharingInvitation item in lstFilteredInvitationAgencyUserData)
                {
                    item.PSI_InviteeEmail = emailId;
                    item.PSI_ModifiedById = loggedInUserId;
                    item.PSI_ModifiedOn = DateTime.Now;
                }
            }

            if (_sharedDataDBContext.SaveChanges() > 0)
                IsRecordUpdated = true;

            return IsRecordUpdated;
        }
        #endregion

        #region UAT-3338
        ProfileSharingInvitationGroup IProfileSharingRepository.GetProfileSharingGroupData(Int32 agencyId, Int32 clinicalRotationId)
        {
            return _sharedDataDBContext.ProfileSharingInvitationGroups.Where(con => con.PSIG_AgencyID == agencyId && con.PSIG_ClinicalRotationID == clinicalRotationId && !con.PSIG_IsDeleted).OrderByDescending(x => x.PSIG_ID).FirstOrDefault();
        }

        #endregion

        #region  UAT-3400
        Boolean IProfileSharingRepository.InsertSharedUserRotReviewForNonRegUser(String profileSharingInvIds, Int32 organizationUserId)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_InsertSharedUserRotationReviewData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvitationIDs", profileSharingInvIds);
                command.Parameters.AddWithValue("@organizationUserId", organizationUserId);
                command.CommandType = CommandType.StoredProcedure;
                con.Open();
                command.ExecuteNonQuery();
                return true;
            }
        }
        #endregion

        #region UAT 3294
        List<Entity.SharedDataEntity.AgencyUser> IProfileSharingRepository.GetAgencyUserByAgencIds(List<Int32> agencyHiearchyIDs)
        {
            if ((agencyHiearchyIDs.IsNotNull() && agencyHiearchyIDs.Count > 0))
            {
                List<Int32> agencyIdLst = _sharedDataDBContext.AgencyHierarchyAgencies.Where(cmd => agencyHiearchyIDs.Contains(cmd.AHA_AgencyHierarchyID) && cmd.AHA_IsDeleted == false).Select(sel => sel.AHA_AgencyID).Distinct().ToList();
                if (agencyIdLst.IsNotNull() && agencyIdLst.Count > 0)
                {
                    List<Int32> AgencyUserIDs = _sharedDataDBContext.UserAgencyMappings.Where(cmd => agencyIdLst.Contains(cmd.UAM_AgencyID) && cmd.UAM_IsDeleted == false).Select(sel => sel.UAM_AgencyUserID).Distinct().ToList();

                    if (AgencyUserIDs.IsNotNull() && AgencyUserIDs.Count > 0)
                    {
                        return _sharedDataDBContext.AgencyUsers.Where(cmb => AgencyUserIDs.Contains(cmb.AGU_ID) && cmb.AGU_IsDeleted == false && cmb.AGU_UserID != null).ToList();
                    }
                }
            }
            return new List<Entity.SharedDataEntity.AgencyUser>();
        }
        Boolean IProfileSharingRepository.IsApplicantSendInvitationToAgencyUser(Guid agencyUserId)
        {
            String applicantsSharedUsercode = OrganizationUserType.ApplicantsSharedUser.GetStringValue();
            base.Context.lkpOrgUserTypes.Where(c => c.IsActive && c.OrgUserTypeCode == applicantsSharedUsercode);

            AgencyUser agencyUsers = _sharedDataDBContext.AgencyUsers.Where(cmd => cmd.AGU_UserID == agencyUserId).FirstOrDefault();
            if (agencyUsers.IsNotNull())
            {
                return _sharedDataDBContext.ProfileSharingInvitations.Where(cmd => cmd.PSI_InviteeEmail.ToLower() == agencyUsers.AGU_Email.ToLower() && agencyUsers.AGU_UserID == agencyUserId && cmd.PSI_IsDeleted == false && cmd.ProfileSharingInvitationGroup.PSIG_IsDeleted == false && cmd.ProfileSharingInvitationGroup.PSIG_IsIndividualShare == true).Any();
            }
            return false;
        }
        Boolean IProfileSharingRepository.MoveApplicantEmailShareToAgencyUser(Guid fromAgencyUserID, Int32 tenantID, Guid toAgencyUserID, Int32 currentLoggedInUserId)
        {
            Boolean IsShareMovedToAgencyUser = false;
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand command = new SqlCommand("usp_MoveApplicantEmailShareToAgencyUser", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FromAgencyUserID", fromAgencyUserID);
                command.Parameters.AddWithValue("@ToAgencyUserID", toAgencyUserID);
                command.Parameters.AddWithValue("@TenantId", tenantID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentLoggedInUserId);
                var result = command.ExecuteScalar();

                if (!result.IsNullOrEmpty())
                    IsShareMovedToAgencyUser = (Boolean)result;

                con.Close();
            }
            return IsShareMovedToAgencyUser;
        }
        #endregion

        #region UAT-3360
        ClientContact IProfileSharingRepository.IsInstructorPreceptorUser(string email)
        {
            return _sharedDataDBContext.ClientContacts.Where(c => c.CC_Email.ToLower() == email.ToLower() && !c.CC_IsDeleted).FirstOrDefault();
        }
        Boolean IProfileSharingRepository.IsAgencyUserExist(List<String> userEmails)
        {
            Boolean IsAgencyUserExist = false;
            var result = _sharedDataDBContext.AgencyUsers.Where(f => userEmails.Contains(f.AGU_Email) && !f.AGU_IsDeleted).ToList();
            if (result.Count > AppConsts.NONE)
                return true;
            return IsAgencyUserExist;
        }

        #endregion

        #region UAT-3316
        Tuple<List<AgencyUserPermissionTemplateContract>, Int32, Int32, List<AgencyUserPermissionTemplateMappingContract>, List<AgencyUserPermissionTemplateNotificationsContract>> IProfileSharingRepository.GetlstAgencyUserPermissionTemplate(AgencyUserPermissionTemplateContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            List<AgencyUserPermissionTemplateContract> agencyUsrPerTemplateList = new List<AgencyUserPermissionTemplateContract>();
            List<AgencyUserPermissionTemplateMappingContract> agencyTemplatemapping = new List<AgencyUserPermissionTemplateMappingContract>();
            List<AgencyUserPermissionTemplateNotificationsContract> agencyTemplateNotification = new List<AgencyUserPermissionTemplateNotificationsContract>();

            Int32 currentPageIndex = AppConsts.ONE;
            Int32 virtualCount = AppConsts.ONE;
            String sortExpression = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? "AGUPT_Name" : gridCustomPaging.SortExpression;
            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetAgencyUserPermissionTemplatesData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TemplateName", searchDataContract.AGUPT_Name);
                command.Parameters.AddWithValue("@TemplateDescription", searchDataContract.AGUPT_Description);
                command.Parameters.AddWithValue("@SortExpression", sortExpression);
                command.Parameters.AddWithValue("@SortDirectionDescending", gridCustomPaging.SortDirectionDescending);
                command.Parameters.AddWithValue("@CurrentPageIndex", gridCustomPaging.CurrentPageIndex);
                command.Parameters.AddWithValue("@PageSize", gridCustomPaging.PageSize);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AgencyUserPermissionTemplateContract requirementSharesDataContract = new AgencyUserPermissionTemplateContract();
                            requirementSharesDataContract.AGUPT_ID = dr["AGUPT_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGUPT_ID"]);
                            requirementSharesDataContract.AGUPT_Name = Convert.ToString(dr["AGUPT_Name"]);
                            requirementSharesDataContract.AGUPT_Description = Convert.ToString(dr["AGUPT_Description"]);
                            requirementSharesDataContract.AGUPT_ComplianceSharedInfoTypeID = dr["AGUPT_ComplianceSharedInfoTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGUPT_ComplianceSharedInfoTypeID"]);
                            requirementSharesDataContract.AGUPT_ReqRotationSharedInfoTypeID = dr["AGUPT_ReqRotationSharedInfoTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGUPT_ReqRotationSharedInfoTypeID"]);
                            requirementSharesDataContract.lstApplicationInvitationMetaDataID = dr["lstApplicationInvitationMetaDataID"].GetType().Name == "DBNull" ? new List<Int32>() : Convert.ToString(dr["lstApplicationInvitationMetaDataID"]).Split(',').ConvertIntoIntList();
                            requirementSharesDataContract.lstInvitationSharedInfoTypeID = dr["lstInvitationSharedInfoTypeID"].GetType().Name == "DBNull" ? new List<Int32>() : Convert.ToString(dr["lstInvitationSharedInfoTypeID"]).Split(',').ConvertIntoIntList();
                            agencyUsrPerTemplateList.Add(requirementSharesDataContract);
                        }

                    }
                    if (ds.Tables.Count > 1)
                    {
                        currentPageIndex = Convert.ToInt32(ds.Tables[1].Rows[0]["CurrentPageIndex"]);
                        virtualCount = Convert.ToInt32(ds.Tables[1].Rows[0]["VirtualCount"]);
                    }
                    if (ds.Tables.Count > 2)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {

                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                AgencyUserPermissionTemplateMappingContract requirementSharesDataContract = new AgencyUserPermissionTemplateMappingContract();
                                requirementSharesDataContract.AGUPT_ID = dr["AGUPT_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGUPT_ID"]);
                                requirementSharesDataContract.AGUPTM_PermissionTypeID = dr["AGUPTM_PermissionTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGUPTM_PermissionTypeID"]);
                                requirementSharesDataContract.AGUPTM_PermissionAccessTypeID = dr["AGUPTM_PermissionAccessTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGUPTM_PermissionAccessTypeID"]);
                                requirementSharesDataContract.AUPT_Name = Convert.ToString(dr["AUPT_Name"]);
                                requirementSharesDataContract.AUPT_Code = Convert.ToString(dr["AUPT_Code"]);
                                agencyTemplatemapping.Add(requirementSharesDataContract);
                            }

                        }
                    }
                    if (ds.Tables.Count > 3)
                    {
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[3].Rows)
                            {

                                AgencyUserPermissionTemplateNotificationsContract requirementSharesDataContract = new AgencyUserPermissionTemplateNotificationsContract();
                                requirementSharesDataContract.AGUPT_ID = dr["AGUPT_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGUPT_ID"]);
                                requirementSharesDataContract.AUN_Code = Convert.ToString(dr["AUN_Code"]);
                                requirementSharesDataContract.AUN_Name = Convert.ToString(dr["AUN_Name"]);
                                requirementSharesDataContract.AGUPTNM_IsMailToBeSend = Convert.ToBoolean(dr["AGUPTNM_IsMailToBeSend"]);
                                agencyTemplateNotification.Add(requirementSharesDataContract);
                            }
                        }
                    }
                }
            }
            return new Tuple<List<AgencyUserPermissionTemplateContract>, Int32, Int32, List<AgencyUserPermissionTemplateMappingContract>, List<AgencyUserPermissionTemplateNotificationsContract>>(agencyUsrPerTemplateList, currentPageIndex, virtualCount, agencyTemplatemapping, agencyTemplateNotification);
        }

        Int32 IProfileSharingRepository.SaveAgencyUserPerTemplate(AgencyUserPermissionTemplateContract _agencyUserPerTemplate, Int32 loggedInUserID, List<AgencyUserPermissionTemplateMapping> lstAgencyUserPerTempMapping)
        {
            AgencyUserPermissionTemplate _newagencyUserPerTemplate = new AgencyUserPermissionTemplate();
            _newagencyUserPerTemplate.AGUPT_Name = _agencyUserPerTemplate.AGUPT_Name;
            _newagencyUserPerTemplate.AGUPT_Description = _agencyUserPerTemplate.AGUPT_Description;
            _newagencyUserPerTemplate.AGUPT_ComplianceSharedInfoTypeID = _agencyUserPerTemplate.AGUPT_ComplianceSharedInfoTypeID;
            _newagencyUserPerTemplate.AGUPT_ReqRotationSharedInfoTypeID = _agencyUserPerTemplate.AGUPT_ReqRotationSharedInfoTypeID;
            _newagencyUserPerTemplate.AGUPT_IsDeleted = _agencyUserPerTemplate.AGUPT_IsDeleted;
            _newagencyUserPerTemplate.AGUPT_CreatedBy = loggedInUserID;
            _newagencyUserPerTemplate.AGUPT_CreatedOn = DateTime.Now;


            _sharedDataDBContext.AddToAgencyUserPermissionTemplates(_newagencyUserPerTemplate);
            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                Guid verificationCode = Guid.NewGuid();

                foreach (var applicationInvtMetaDataIds in _agencyUserPerTemplate.lstApplicationInvitationMetaDataID)
                {
                    AgencyUserPerTemplateSharedData agencyUserPerTempSharedData = new AgencyUserPerTemplateSharedData();
                    agencyUserPerTempSharedData.AUPTSD_AgencyUserTemplateID = _newagencyUserPerTemplate.AGUPT_ID;
                    agencyUserPerTempSharedData.AUPTSD_ApplicationInvitationMetaDataID = applicationInvtMetaDataIds;
                    agencyUserPerTempSharedData.AUPTSD_IsDeleted = false;
                    agencyUserPerTempSharedData.AUPTSD_CreatedByID = loggedInUserID;
                    agencyUserPerTempSharedData.AUPTSD_CreatedOn = DateTime.Now;

                    _sharedDataDBContext.AddToAgencyUserPerTemplateSharedDatas(agencyUserPerTempSharedData);
                }

                //Add Invitation Shared Info Mapping
                foreach (var invitationSharedInfoTypeID in _agencyUserPerTemplate.lstInvitationSharedInfoTypeID)
                {
                    Entity.SharedDataEntity.TemplateInvitationSharedInfoMapping invitationSharedInfoMapping = new Entity.SharedDataEntity.TemplateInvitationSharedInfoMapping();
                    invitationSharedInfoMapping.TISIM_AgencyUserPermissionTemplateID = _newagencyUserPerTemplate.AGUPT_ID;
                    invitationSharedInfoMapping.TISIM_InvitationSharedInfoTypeID = invitationSharedInfoTypeID;
                    invitationSharedInfoMapping.TISIM_IsDeleted = false;
                    invitationSharedInfoMapping.TISIM_CreatedByID = loggedInUserID;
                    invitationSharedInfoMapping.TISIM_CreatedOn = DateTime.Now;
                    _sharedDataDBContext.AddToTemplateInvitationSharedInfoMappings(invitationSharedInfoMapping);
                }

                if (!lstAgencyUserPerTempMapping.IsNullOrEmpty())
                {
                    //for existing records
                    foreach (AgencyUserPermissionTemplateMapping agencyUserPerTempMap in lstAgencyUserPerTempMapping)
                    {
                        _newagencyUserPerTemplate.AgencyUserPermissionTemplateMappings.Add(new AgencyUserPermissionTemplateMapping()
                        {
                            AGUPTM_AgencyUserPerTemplateId = _newagencyUserPerTemplate.AGUPT_ID,
                            AGUPTM_PermissionTypeID = agencyUserPerTempMap.AGUPTM_PermissionTypeID,
                            AGUPTM_PermissionAccessTypeID = agencyUserPerTempMap.AGUPTM_PermissionAccessTypeID,
                            AGUPTM_IsDeleted = false,
                            AGUPTM_CreatedByID = loggedInUserID,
                            AGUPTM_CreatedOn = DateTime.Now,
                            AGUPTM_RecordTypeID = agencyUserPerTempMap.AGUPTM_RecordTypeID,
                        });
                    }
                }

                if (_sharedDataDBContext.SaveChanges() > 0)
                {
                    return _newagencyUserPerTemplate.AGUPT_ID;
                }
                return AppConsts.NONE;
            }
            else
            {
                return AppConsts.NONE;
            }
        }
        Boolean IProfileSharingRepository.SaveAgencyUserTemplateNotificationMappings(Int32 agencyUserTemplateID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInOrgUserID)
        {
            foreach (var item in dicNotificationData)
            {

                AgencyUserPerTemplateNotificationMapping agencyUserPerTemplateNotificationMapping = new AgencyUserPerTemplateNotificationMapping();
                agencyUserPerTemplateNotificationMapping.AGUPTNM_AgencyUserPerTemplateId = agencyUserTemplateID;
                agencyUserPerTemplateNotificationMapping.AGUPTNM_NotificationTypeID = item.Key;
                agencyUserPerTemplateNotificationMapping.AGUPTNM_IsMailToBeSend = item.Value;
                agencyUserPerTemplateNotificationMapping.AGUPTNM_IsDeleted = false;
                agencyUserPerTemplateNotificationMapping.AGUPTNM_CreatedOn = DateTime.Now;
                agencyUserPerTemplateNotificationMapping.AGUPTNM_CreatedBy = CurrentLoggedInOrgUserID;
                _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.AddObject(agencyUserPerTemplateNotificationMapping);
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        String IProfileSharingRepository.DeleteAgencyUserPermissionTemplate(Int32 TenantID, Int32 AGUPT_ID, Int32 LoggedInUserId, Boolean IsAdmin)
        {
            // if (IsAdmin)
            //{
            AgencyUserPermissionTemplate agencyUser = _sharedDataDBContext.AgencyUserPermissionTemplates.Where(x => x.AGUPT_ID == AGUPT_ID && x.AGUPT_IsDeleted == false).FirstOrDefault();
            agencyUser.AGUPT_IsDeleted = true;
            agencyUser.AGUPT_ModifiedBy = LoggedInUserId;
            agencyUser.AGUPT_ModifiedOn = DateTime.Now;



            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                //UAT 1616 WB: Agency users should not have to assign what schools have access to rotation packages
                IEnumerable<AgencyUserPermissionTemplateMapping> lstAgencyUserPerTempMapping = agencyUser.AgencyUserPermissionTemplateMappings.Where(x => !x.AGUPTM_IsDeleted);
                foreach (AgencyUserPermissionTemplateMapping agencyUserPerTempMapping in lstAgencyUserPerTempMapping)
                {
                    agencyUserPerTempMapping.AGUPTM_IsDeleted = true;
                    agencyUserPerTempMapping.AGUPTM_ModifiedByID = LoggedInUserId;
                    agencyUserPerTempMapping.AGUPTM_ModifiedOn = DateTime.Now;
                }

                _sharedDataDBContext.SaveChanges();
                //DeleteAgencyHierarchyUserDetails(AUG_ID, LoggedInUserId);
                return AppConsts.AGUPT_DELETED_SUCCESS_MSG;
            }
            else
            {
                return AppConsts.AGUPT_DELETED_ERROR_MSG;
            }
            //}
            //else
            //{
            //Remove only mapping
            //Int32 agencyInstID = _sharedDataDBContext.AgencyInstitutions.Where(x => lstAgencyInstID.Contains(x.AGI_ID) && x.AGI_TenantID == TenantID && !x.AGI_IsDeleted).Select(c => c.AGI_ID).FirstOrDefault();
            ////Client Admin
            //AgencyUserInstitution agencyUserInstitution = _sharedDataDBContext.AgencyUserInstitutions
            // .Where(cond => cond.UserAgencyMapping.UAM_AgencyUserID == AUG_ID && cond.AGUI_AgencyInstitutionID == agencyInstID && !cond.AGUI_IsDeleted).FirstOrDefault();

            //agencyUserInstitution.AGUI_IsDeleted = true;
            //agencyUserInstitution.AGUI_ModifiedByID = LoggedInUserId;
            //agencyUserInstitution.AGUI_ModifiedOn = DateTime.Now;

            //_sharedDataDBContext.SaveChanges();
            // return AppConsts.AGU_DELETED_SUCCESS_MSG;
            // }
        }
        void IProfileSharingRepository.DeleteAgencyUserPerTemplateNotificationMappings(Int32 AgencyUserTemplateID, Int32 CurrentLoggedInUserId)
        {
            List<AgencyUserPerTemplateNotificationMapping> lstAgencyUserPreTempNotificationMappings = _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.Where(con => con.AGUPTNM_AgencyUserPerTemplateId == AgencyUserTemplateID && !con.AGUPTNM_IsDeleted).ToList();
            if (!lstAgencyUserPreTempNotificationMappings.IsNullOrEmpty())
            {
                foreach (AgencyUserPerTemplateNotificationMapping item in lstAgencyUserPreTempNotificationMappings)
                {
                    item.AGUPTNM_IsDeleted = true;
                    item.AGUPTNM_ModifiedBy = CurrentLoggedInUserId;
                    item.AGUPTNM_ModifiedOn = DateTime.Now;
                }
                _sharedDataDBContext.SaveChanges();
            }
        }

        AgencyUserPermissionTemplate IProfileSharingRepository.UpdateAgencyUserPermissionTemplate(AgencyUserPermissionTemplateContract _agencyUserPerTemplate, Int32 LoggedInUserId, Boolean IsAdmin, List<AgencyUserPermissionTemplateMapping> lstAgencyUserPermissionTemplate)
        {

            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            {
                AgencyUserPermissionTemplate agencyUserPerTemplate = _sharedDataDBContext.AgencyUserPermissionTemplates.Where(x => x.AGUPT_ID == _agencyUserPerTemplate.AGUPT_ID && x.AGUPT_IsDeleted == false).FirstOrDefault();
                agencyUserPerTemplate.AGUPT_Name = _agencyUserPerTemplate.AGUPT_Name;
                agencyUserPerTemplate.AGUPT_Description = _agencyUserPerTemplate.AGUPT_Description;

                agencyUserPerTemplate.AGUPT_IsDeleted = false;
                agencyUserPerTemplate.AGUPT_ModifiedBy = LoggedInUserId;
                agencyUserPerTemplate.AGUPT_ModifiedOn = DateTime.Now;
                agencyUserPerTemplate.AGUPT_ComplianceSharedInfoTypeID = _agencyUserPerTemplate.AGUPT_ComplianceSharedInfoTypeID;
                agencyUserPerTemplate.AGUPT_ReqRotationSharedInfoTypeID = _agencyUserPerTemplate.AGUPT_ReqRotationSharedInfoTypeID;

                if (_sharedDataDBContext.SaveChanges() > 0)
                {

                    List<AgencyUserPerTemplateSharedData> lstAgencyUserSharedData = _sharedDataDBContext.AgencyUserPerTemplateSharedDatas.Where(x => x.AUPTSD_AgencyUserTemplateID == _agencyUserPerTemplate.AGUPT_ID && x.AUPTSD_IsDeleted == false).ToList();
                    if (lstAgencyUserSharedData.IsNotNull())
                    {
                        //Delete Old Data
                        foreach (var agencyUserSharedData in lstAgencyUserSharedData)
                        {
                            agencyUserSharedData.AUPTSD_IsDeleted = true;
                            agencyUserSharedData.AUPTSD_ModifiedByID = LoggedInUserId;
                            agencyUserSharedData.AUPTSD_ModifiedOn = DateTime.Now;
                        }

                        //Add New Data
                        foreach (var applicationInvtMetaDataIds in _agencyUserPerTemplate.lstApplicationInvitationMetaDataID)
                        {
                            AgencyUserPerTemplateSharedData agencyUserSharedData = new AgencyUserPerTemplateSharedData();
                            agencyUserSharedData.AUPTSD_AgencyUserTemplateID = agencyUserPerTemplate.AGUPT_ID;
                            agencyUserSharedData.AUPTSD_ApplicationInvitationMetaDataID = applicationInvtMetaDataIds;
                            agencyUserSharedData.AUPTSD_IsDeleted = false;
                            agencyUserSharedData.AUPTSD_CreatedByID = LoggedInUserId;
                            agencyUserSharedData.AUPTSD_CreatedOn = DateTime.Now;

                            _sharedDataDBContext.AddToAgencyUserPerTemplateSharedDatas(agencyUserSharedData);
                        }
                    }

                    List<Entity.SharedDataEntity.TemplateInvitationSharedInfoMapping> lstInvitationSharedInfoMapping = _sharedDataDBContext.TemplateInvitationSharedInfoMappings.Where(x => x.TISIM_AgencyUserPermissionTemplateID == _agencyUserPerTemplate.AGUPT_ID && x.TISIM_IsDeleted == false).ToList();
                    if (lstInvitationSharedInfoMapping.IsNotNull())
                    {
                        //Delete Old Data
                        foreach (var invitationSharedInfoMapping in lstInvitationSharedInfoMapping)
                        {
                            invitationSharedInfoMapping.TISIM_IsDeleted = true;
                            invitationSharedInfoMapping.TISIM_ModifiedByID = LoggedInUserId;
                            invitationSharedInfoMapping.TISIM_ModifiedOn = DateTime.Now;
                        }

                        //Add New Data
                        foreach (var invitationSharedInfoTypeID in _agencyUserPerTemplate.lstInvitationSharedInfoTypeID)
                        {
                            Entity.SharedDataEntity.TemplateInvitationSharedInfoMapping invitationSharedInfoMapping = new Entity.SharedDataEntity.TemplateInvitationSharedInfoMapping();
                            invitationSharedInfoMapping.TISIM_AgencyUserPermissionTemplateID = agencyUserPerTemplate.AGUPT_ID;
                            invitationSharedInfoMapping.TISIM_InvitationSharedInfoTypeID = invitationSharedInfoTypeID;
                            invitationSharedInfoMapping.TISIM_IsDeleted = false;
                            invitationSharedInfoMapping.TISIM_CreatedByID = LoggedInUserId;
                            invitationSharedInfoMapping.TISIM_CreatedOn = DateTime.Now;

                            _sharedDataDBContext.AddToTemplateInvitationSharedInfoMappings(invitationSharedInfoMapping);
                        }
                    }


                    #region Agency User Permission
                    IEnumerable<AgencyUserPermissionTemplateMapping> existingAgencyUserPermissions = agencyUserPerTemplate.AgencyUserPermissionTemplateMappings.Where(cond => !cond.AGUPTM_IsDeleted);

                    //Start UAT-3664 : Agency User Report Type Permissions.
                    String reportPermissionTypeCode = AgencyUserPermissionType.REPORTS_PERMISSION.GetStringValue();
                    Int32 reportPermissionTypeID = this.SharedDataDBContext.lkpAgencyUserPermissionTypes.Where(cond => !cond.AUPT_IsDeleted && cond.AUPT_Code == reportPermissionTypeCode).FirstOrDefault().AUPT_ID;

                    List<AgencyUserPermissionTemplateMapping> existingAgencyUserReportsPermissions = existingAgencyUserPermissions.Where(cond => cond.AGUPTM_PermissionTypeID == reportPermissionTypeID).ToList();
                    List<AgencyUserPermissionTemplateMapping> lstAgencyUserReportsPermission = lstAgencyUserPermissionTemplate.Where(cond => cond.AGUPTM_PermissionTypeID == reportPermissionTypeID).ToList();

                    foreach (AgencyUserPermissionTemplateMapping agencyUserReportPermission in lstAgencyUserReportsPermission)
                    {
                        if (!existingAgencyUserReportsPermissions.IsNullOrEmpty() && existingAgencyUserReportsPermissions.Where(cond => cond.AGUPTM_RecordTypeID == agencyUserReportPermission.AGUPTM_RecordTypeID).Any())
                        {
                            AgencyUserPermissionTemplateMapping aup = existingAgencyUserReportsPermissions.Where(cond => cond.AGUPTM_RecordTypeID == agencyUserReportPermission.AGUPTM_RecordTypeID).FirstOrDefault();

                            //update
                            aup.AGUPTM_PermissionAccessTypeID = agencyUserReportPermission.AGUPTM_PermissionAccessTypeID;
                            aup.AGUPTM_PermissionTypeID = agencyUserReportPermission.AGUPTM_PermissionTypeID;
                            aup.AGUPTM_IsDeleted = false;
                            aup.AGUPTM_ModifiedByID = LoggedInUserId;
                            aup.AGUPTM_ModifiedOn = DateTime.Now;
                            aup.AGUPTM_RecordTypeID = agencyUserReportPermission.AGUPTM_RecordTypeID;

                            existingAgencyUserReportsPermissions = existingAgencyUserReportsPermissions.Where(cond => cond.AGUPTM_RecordTypeID != agencyUserReportPermission.AGUPTM_RecordTypeID).ToList();
                        }
                        else
                        {
                            //Insert
                            AgencyUserPermissionTemplateMapping dbInsert = new AgencyUserPermissionTemplateMapping();
                            dbInsert.AGUPTM_PermissionTypeID = agencyUserReportPermission.AGUPTM_PermissionTypeID;
                            dbInsert.AGUPTM_PermissionAccessTypeID = agencyUserReportPermission.AGUPTM_PermissionAccessTypeID;
                            dbInsert.AGUPTM_IsDeleted = false;
                            dbInsert.AGUPTM_CreatedByID = LoggedInUserId;
                            dbInsert.AGUPTM_CreatedOn = DateTime.Now;
                            dbInsert.AGUPTM_RecordTypeID = agencyUserReportPermission.AGUPTM_RecordTypeID;
                            agencyUserPerTemplate.AgencyUserPermissionTemplateMappings.Add(dbInsert);
                        }

                    }

                    if (!existingAgencyUserReportsPermissions.IsNullOrEmpty())
                    {
                        //These are unchecked record, i.e these reports have permission access "YES".
                        String yesPermissionAccessCode = AgencyUserPermissionAccessType.YES.GetStringValue();
                        Int32 yesPermissionAccessID = this.SharedDataDBContext.lkpAgencyUserPermissionAccessTypes.Where(con => !con.AUPAT_IsDeleted && con.AUPAT_Code == yesPermissionAccessCode).FirstOrDefault().AUPAT_ID;

                        if (yesPermissionAccessID > AppConsts.NONE)
                        {
                            foreach (AgencyUserPermissionTemplateMapping existingAgencyUserReportPermission in existingAgencyUserReportsPermissions)
                            {
                                AgencyUserPermissionTemplateMapping aup = existingAgencyUserReportsPermissions.Where(cond => cond.AGUPTM_RecordTypeID == existingAgencyUserReportPermission.AGUPTM_RecordTypeID).FirstOrDefault();

                                //update
                                aup.AGUPTM_PermissionAccessTypeID = yesPermissionAccessID;
                                aup.AGUPTM_IsDeleted = false;
                                aup.AGUPTM_ModifiedByID = LoggedInUserId;
                                aup.AGUPTM_ModifiedOn = DateTime.Now;
                            }
                        }
                    }

                    _sharedDataDBContext.SaveChanges();

                    existingAgencyUserPermissions = existingAgencyUserPermissions.Where(cond => cond.AGUPTM_PermissionTypeID != reportPermissionTypeID);
                    lstAgencyUserPermissionTemplate.RemoveAll(c => c.AGUPTM_PermissionTypeID == reportPermissionTypeID);

                    //END UAT-3664


                    foreach (AgencyUserPermissionTemplateMapping agencyUserPermission in lstAgencyUserPermissionTemplate)
                    {

                        if (!existingAgencyUserPermissions.IsNullOrEmpty() && existingAgencyUserPermissions.Where(cond => cond.AGUPTM_PermissionTypeID == agencyUserPermission.AGUPTM_PermissionTypeID).Any())
                        {
                            AgencyUserPermissionTemplateMapping aup = existingAgencyUserPermissions.Where(cond => cond.AGUPTM_PermissionTypeID == agencyUserPermission.AGUPTM_PermissionTypeID).FirstOrDefault();

                            //update
                            aup.AGUPTM_PermissionAccessTypeID = agencyUserPermission.AGUPTM_PermissionAccessTypeID;
                            aup.AGUPTM_PermissionTypeID = agencyUserPermission.AGUPTM_PermissionTypeID;
                            aup.AGUPTM_IsDeleted = false;
                            aup.AGUPTM_ModifiedByID = LoggedInUserId;
                            aup.AGUPTM_ModifiedOn = DateTime.Now;
                        }
                        else
                        {
                            //Insert
                            AgencyUserPermissionTemplateMapping dbInsert = new AgencyUserPermissionTemplateMapping();
                            dbInsert.AGUPTM_PermissionTypeID = agencyUserPermission.AGUPTM_PermissionTypeID;
                            dbInsert.AGUPTM_PermissionAccessTypeID = agencyUserPermission.AGUPTM_PermissionAccessTypeID;
                            dbInsert.AGUPTM_IsDeleted = false;
                            dbInsert.AGUPTM_CreatedByID = LoggedInUserId;
                            dbInsert.AGUPTM_CreatedOn = DateTime.Now;
                            agencyUserPerTemplate.AgencyUserPermissionTemplateMappings.Add(dbInsert);
                        }
                        _sharedDataDBContext.SaveChanges();
                    }
                    #endregion

                    scope.Complete();
                    return agencyUserPerTemplate;
                }
                //  To commit.
            }
            return null;
        }

        Boolean IProfileSharingRepository.UpdateAgencyUserPerTemplateNotificationMappings(Int32 AgencyUserTemplateID, Dictionary<Int32, Boolean> dicNotificationData, Int32 CurrentLoggedInUserId)
        {
            List<AgencyUserPerTemplateNotificationMapping> lstAgencyUserPerTemplateNotificationMappings = _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings
                                                                                            .Where(con => con.AGUPTNM_AgencyUserPerTemplateId == AgencyUserTemplateID && !con.AGUPTNM_IsDeleted).ToList();
            if (!lstAgencyUserPerTemplateNotificationMappings.IsNullOrEmpty())
            {
                foreach (AgencyUserPerTemplateNotificationMapping agencyusernotMapping in lstAgencyUserPerTemplateNotificationMappings)
                {
                    foreach (var item in dicNotificationData)
                    {
                        if (item.Key == agencyusernotMapping.AGUPTNM_NotificationTypeID)
                        {
                            agencyusernotMapping.AGUPTNM_IsMailToBeSend = item.Value;
                            agencyusernotMapping.AGUPTNM_ModifiedBy = CurrentLoggedInUserId;
                            agencyusernotMapping.AGUPTNM_ModifiedOn = DateTime.Now;
                        }
                    }
                }

                foreach (var item in dicNotificationData)
                {
                    if (!(lstAgencyUserPerTemplateNotificationMappings.Exists(cond => cond.AGUPTNM_NotificationTypeID == item.Key)))
                    {
                        AgencyUserPerTemplateNotificationMapping agencyUserNotificationMapping = new AgencyUserPerTemplateNotificationMapping();
                        agencyUserNotificationMapping.AGUPTNM_AgencyUserPerTemplateId = AgencyUserTemplateID;
                        agencyUserNotificationMapping.AGUPTNM_NotificationTypeID = item.Key;
                        agencyUserNotificationMapping.AGUPTNM_IsMailToBeSend = item.Value;
                        agencyUserNotificationMapping.AGUPTNM_IsDeleted = false;
                        agencyUserNotificationMapping.AGUPTNM_CreatedOn = DateTime.Now;
                        agencyUserNotificationMapping.AGUPTNM_CreatedBy = CurrentLoggedInUserId;
                        _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.AddObject(agencyUserNotificationMapping);
                    }
                }
            }
            else
            {
                foreach (var item in dicNotificationData)
                {
                    AgencyUserPerTemplateNotificationMapping agencyUserNotificationMapping = new AgencyUserPerTemplateNotificationMapping();
                    agencyUserNotificationMapping.AGUPTNM_AgencyUserPerTemplateId = AgencyUserTemplateID;
                    agencyUserNotificationMapping.AGUPTNM_NotificationTypeID = item.Key;
                    agencyUserNotificationMapping.AGUPTNM_IsMailToBeSend = item.Value;
                    agencyUserNotificationMapping.AGUPTNM_IsDeleted = false;
                    agencyUserNotificationMapping.AGUPTNM_CreatedOn = DateTime.Now;
                    agencyUserNotificationMapping.AGUPTNM_CreatedBy = CurrentLoggedInUserId;
                    _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.AddObject(agencyUserNotificationMapping);
                }
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        List<AgencyUserPermissionTemplate> IProfileSharingRepository.GetAgencyUserPermissionTemplates()
        {
            return _sharedDataDBContext.AgencyUserPermissionTemplates.Where(x => !x.AGUPT_IsDeleted).ToList();
        }

        void IProfileSharingRepository.DeleteAgencyUserTemplateNotificationMappings(Int32 AgencyUserTemplateId, Int32 CurrentLoggedInUserId)
        {
            List<AgencyUserPerTemplateNotificationMapping> lstAgencyUserPerTemplateNotificationMappings = _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.Where(con => con.AGUPTNM_AgencyUserPerTemplateId == AgencyUserTemplateId && !con.AGUPTNM_IsDeleted).ToList();
            if (!lstAgencyUserPerTemplateNotificationMappings.IsNullOrEmpty())
            {
                foreach (AgencyUserPerTemplateNotificationMapping item in lstAgencyUserPerTemplateNotificationMappings)
                {
                    item.AGUPTNM_IsDeleted = true;
                    item.AGUPTNM_ModifiedBy = CurrentLoggedInUserId;
                    item.AGUPTNM_ModifiedOn = DateTime.Now;
                }
                _sharedDataDBContext.SaveChanges();
            }
        }

        List<AgencyUserPermissionTemplateMapping> IProfileSharingRepository.GetAgencyUsrPerTemplateMappings(Int32 permisisonTemplateId)
        {
            return _sharedDataDBContext.AgencyUserPermissionTemplateMappings.Where(x => x.AGUPTM_AgencyUserPerTemplateId == permisisonTemplateId && !x.AGUPTM_IsDeleted).ToList();
        }
        List<AgencyUserPerTemplateNotificationMapping> IProfileSharingRepository.GetAgencyUsrPerTemplateNotificationsMappings(Int32 permisisonTemplateId)
        {
            return _sharedDataDBContext.AgencyUserPerTemplateNotificationMappings.Where(x => x.AGUPTNM_AgencyUserPerTemplateId == permisisonTemplateId && !x.AGUPTNM_IsDeleted).ToList();
        }

        List<lkpAgencyUserPermissionType> IProfileSharingRepository.GetAgencyUserPermissionTypes()
        {
            return _sharedDataDBContext.lkpAgencyUserPermissionTypes.Where(con => con.AUPT_IsDeleted == false).ToList();
        }

        List<Int32> IProfileSharingRepository.GetInvitationSharedInfoTypeID(Int32 templateID)
        {
            List<Int32> lstInvSharedInfoIds = new List<Int32>();
            List<TemplateInvitationSharedInfoMapping> lstInvSharedInfo = _sharedDataDBContext.TemplateInvitationSharedInfoMappings.Where(con => con.TISIM_AgencyUserPermissionTemplateID == templateID && !con.TISIM_IsDeleted).ToList();

            foreach (var itm in lstInvSharedInfo)
            {
                lstInvSharedInfoIds.Add(itm.TISIM_InvitationSharedInfoTypeID);
            }
            return lstInvSharedInfoIds;
        }
        List<Int32?> IProfileSharingRepository.GetApplicationInvitationMetaDataID(Int32 templateID)
        {
            List<Int32?> lstInvMetaDataIDs = new List<Int32?>();
            List<AgencyUserPerTemplateSharedData> lstSharedMetaData = _sharedDataDBContext.AgencyUserPerTemplateSharedDatas.Where(con => con.AUPTSD_AgencyUserTemplateID == templateID && !con.AUPTSD_IsDeleted).ToList();

            foreach (var itm in lstSharedMetaData)
            {
                lstInvMetaDataIDs.Add(itm.AUPTSD_ApplicationInvitationMetaDataID);
            }
            return lstInvMetaDataIDs;
        }

        List<AgencyUserPermission> IProfileSharingRepository.GetAgencyUsrPermisisonMappings(Int32 userId)
        {
            return _sharedDataDBContext.AgencyUserPermissions.Where(x => x.AUP_AgencyUserID == userId && !x.AUP_IsDeleted).ToList();
        }
        #endregion

        #region UAT-3470
        Boolean IProfileSharingRepository.SaveUpdateInvitationArchiveState(String InvitationIds, String rotationContract, Int32 CurrentLoggedInUser, Boolean IsPerformArchiveOperation)
        {
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand("usp_ArchiveInvitation", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvitationIds", InvitationIds);
                command.Parameters.AddWithValue("@RotationContractXML", rotationContract);
                command.Parameters.AddWithValue("@CurrentLoggedInUser", CurrentLoggedInUser);
                command.Parameters.AddWithValue("@IsPerformArchiveOperation", IsPerformArchiveOperation);
                command.ExecuteScalar();
                con.Close();
            }
            return true;
        }
        #endregion

        #region UAT-3353
        List<SharedUserInvitationDocumentContract> IProfileSharingRepository.GetSharedUserRotationInvitationDocumentDetails(Int32 tenantID, Int32 clinicalRotationID, Int32 agencyID)
        {
            List<SharedUserInvitationDocumentContract> lstSharedUserInvitationDocs = new List<SharedUserInvitationDocumentContract>();
            var documentList = _sharedDataDBContext.SharedUserRotationInvitationDocumentMappings.Where(d => !d.SURIDM_IsDeleted && d.SURIDM_TenantID == tenantID && d.SURIDM_ClinicalRotationID == clinicalRotationID && d.SURIDM_AgencyID == agencyID).ToList();
            if (!documentList.IsNullOrEmpty())
            {
                documentList.ForEach(s => lstSharedUserInvitationDocs.Add(new SharedUserInvitationDocumentContract()
                {
                    AgencyID = s.SURIDM_AgencyID,
                    ClinicalRotationID = s.SURIDM_ClinicalRotationID,
                    Description = s.InvitationDocument.IND_Description,
                    InvitationDocumentID = s.SURIDM_InvitationDocumentID,
                    ProfileSharingInvitationGroupID = s.SURIDM_ProfileSharingInvitationGroupID.Value,
                    FileName = s.InvitationDocument.IND_FileName,
                    MD5Hash = s.InvitationDocument.IND_DocMD5Hash
                }));
            }
            return lstSharedUserInvitationDocs;
        }
        Boolean IProfileSharingRepository.SaveSharedUserRotationInvitationDocumentDetails(List<SharedUserRotationInvitationDocumentMapping> lstSharedUserRotationInvitationDocumentMapping)
        {
            lstSharedUserRotationInvitationDocumentMapping.ForEach(x =>
            {
                _sharedDataDBContext.SharedUserRotationInvitationDocumentMappings.AddObject(x);
            });

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        Boolean IProfileSharingRepository.DeletedSharedUserRotationInvitationDocument(Int32 invitationDocumentID, Int32 tenantID, Int32 clinicalRotationID, Int32 agencyID, Int32 currentLoggedInUserID)
        {
            SharedUserRotationInvitationDocumentMapping SharedUserRotationInviDocDetails = _sharedDataDBContext.SharedUserRotationInvitationDocumentMappings.Where(cond => !cond.SURIDM_IsDeleted
                                    && cond.SURIDM_ClinicalRotationID == clinicalRotationID
                                    && cond.SURIDM_TenantID == tenantID
                                    && !cond.InvitationDocument.IND_IsDeleted
                                    && cond.InvitationDocument.IND_ID == invitationDocumentID)
                                    .FirstOrDefault();

            if (!SharedUserRotationInviDocDetails.IsNullOrEmpty())
            {
                SharedUserRotationInviDocDetails.SURIDM_IsDeleted = true;
                SharedUserRotationInviDocDetails.SURIDM_ModifiedBy = currentLoggedInUserID;
                SharedUserRotationInviDocDetails.SURIDM_ModifiedOn = DateTime.Now;

                InvitationDocument invitationDocument = SharedUserRotationInviDocDetails.InvitationDocument;

                if (!invitationDocument.IsNullOrEmpty())
                {
                    invitationDocument.IND_IsDeleted = true;
                    invitationDocument.IND_ModifiedByID = currentLoggedInUserID;
                    invitationDocument.IND_ModifiedOn = DateTime.Now;

                    if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                        return true;
                    return false;
                }
            }
            return false;
        }
        Boolean IProfileSharingRepository.IsRotationInvitationDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 tenantID, Int32 clinicalRotationID, Int32 SharedDocTypeID)
        {
            SharedUserRotationInvitationDocumentMapping SharedUserRotationInviDocDetails = _sharedDataDBContext.SharedUserRotationInvitationDocumentMappings.Where(cond => !cond.SURIDM_IsDeleted
                                    && cond.SURIDM_TenantID == tenantID
                                    && cond.SURIDM_ClinicalRotationID == clinicalRotationID
                                    && cond.InvitationDocument.IND_FileName == documentName
                                    && !cond.InvitationDocument.IND_IsDeleted
                                    && cond.InvitationDocument.IND_Size == documentSize
                                    && cond.InvitationDocument.IND_DocumentType == SharedDocTypeID).FirstOrDefault();
            if (SharedUserRotationInviDocDetails.IsNullOrEmpty())
                return false;
            return true;
        }

        Int32 IProfileSharingRepository.GetProfileSharingGroupIDByClinicalRotationID(Int32 tenantID, Int32 clinicalRotationID)
        {
            var profileSharingGroupDetails = _sharedDataDBContext.ProfileSharingInvitationGroups.Where(d => d.PSIG_ClinicalRotationID == clinicalRotationID && d.PSIG_TenantID == tenantID && !d.PSIG_IsDeleted).Select(d => d.PSIG_ID).OrderByDescending(f => f).FirstOrDefault();
            if (!profileSharingGroupDetails.IsNullOrEmpty())
                return profileSharingGroupDetails;
            else
                return AppConsts.NONE;
        }
        #endregion

        #region UAT- 3606

        List<Entity.OrganizationUser> IProfileSharingRepository.GetProfileSharingInvitationApplicantSharedData(List<Int32> lstPSIList)
        {
            List<Int32> lstAppOrgUserID = _sharedDataDBContext.ProfileSharingInvitations.Where(con => lstPSIList.Contains(con.PSI_ID) && !con.PSI_IsDeleted).Select(sel => sel.PSI_ApplicantOrgUserID).Distinct().ToList();

            return base.Context.OrganizationUsers.Where(cond => lstAppOrgUserID.Contains(cond.OrganizationUserID) && !cond.IsDeleted).ToList();
        }
        #endregion

        #region UAT-3715

        List<String> IProfileSharingRepository.GetSharedUserBkgPermissions(Int32 CurrentLoggedInUserId)
        {
            List<String> lstSharedUserBkgPermissions = new List<String>();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSharedUserBkgPermissions", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInUserId", CurrentLoggedInUserId);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstSharedUserBkgPermissions.Add(dr["Codes"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["Codes"]));
                    }
                }
                return lstSharedUserBkgPermissions;
            }
        }

        List<InvitationAttestationDocumentDataWithAgencyUserPermissions> IProfileSharingRepository.GetAttestationDocumentDetailsWithPermissionType(Int32 selectedRotationId, Int32 tenantID, Int32 agencyID)
        {
            List<InvitationAttestationDocumentDataWithAgencyUserPermissions> lstAttestationDoc = new List<InvitationAttestationDocumentDataWithAgencyUserPermissions>();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAttestationDocumentDetailsWithPermissionType", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RotationID", selectedRotationId);
                command.Parameters.AddWithValue("@tenantID", tenantID);
                command.Parameters.AddWithValue("@AgencyID", agencyID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstAttestationDoc.Add(new InvitationAttestationDocumentDataWithAgencyUserPermissions()
                        {
                            InvitationDocumentID = Convert.ToInt32(dr["InvitationDocumentID"]),
                            DocumentPath = Convert.ToString(dr["DocumentFilePath"]),
                            AgencyUserPermissionCode = Convert.ToString(dr["AgencyUserPermissionCode"])
                        });
                    }
                }
            }

            return lstAttestationDoc;
        }

        InvitationSharedInfoDetails IProfileSharingRepository.GetSharedUserCurrentPermission(Int32? agencyUserId, Int32? agencyUserTemplateId)
        {
            InvitationSharedInfoDetails invitationSharedInfoDetails = new InvitationSharedInfoDetails();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSharedUserCurrentPermission", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyUserId", agencyUserId);
                command.Parameters.AddWithValue("@AgencyUserTemplateId", agencyUserTemplateId);

                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();

                da.SelectCommand = command;
                da.Fill(ds);

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        invitationSharedInfoDetails.ComplianceSharedInfoTypeCode = Convert.ToString(row["ComplianceSharedInfoCode"]);
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows != null && ds.Tables[1].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[1].Rows[0];
                        invitationSharedInfoDetails.ReqRotSharedInfoTypeCode = Convert.ToString(row["RequirementSharedInfoCode"]);
                    }

                    if (ds.Tables[2] != null && ds.Tables[2].Rows != null && ds.Tables[2].Rows.Count > 0)
                    {
                        invitationSharedInfoDetails.LstBkgSharedInfoTypeCode = new List<String>();
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            invitationSharedInfoDetails.LstBkgSharedInfoTypeCode.Add(Convert.ToString(dr["BackgroundSharedInfoCode"]));
                        }
                    }
                }
            }
            return invitationSharedInfoDetails;
        }

        Boolean IProfileSharingRepository.UpdateDocMappingForInvAttestation(Int32? agencyUserId, Int32? templateId, String rotationSpecificPermissionTypeCode, String profileSpecificPermissionTypeCode, Int32 CurrentLoggedInUserID)
        {
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand("usp_UpdateDocMappingForInvAttestation", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyUserId", agencyUserId);
                command.Parameters.AddWithValue("@TemplateId", templateId);
                command.Parameters.AddWithValue("@RotationSpecificPermissionTypeCode", rotationSpecificPermissionTypeCode);
                command.Parameters.AddWithValue("@ProfileSpecificPermissionTypeCode", profileSpecificPermissionTypeCode);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", CurrentLoggedInUserID);
                command.ExecuteScalar();
                con.Close();
            }
            return true;
        }

        #endregion

        #region UAT-3719
        void IProfileSharingRepository.SaveAgencyUserPermissionAuditDetails(Int32? AgencyUserId, Int32? AgencyUserTemplateID, Int32 CurrentLoggedInUserID)
        {
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand("usp_SaveAgencyUserPermissionAuditData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyUserID", AgencyUserId);
                command.Parameters.AddWithValue("@AgencyUserTemplateID", AgencyUserTemplateID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", CurrentLoggedInUserID);
                command.ExecuteScalar();
                con.Close();
            }
        }
        #endregion


        Boolean IProfileSharingRepository.UpdateSharedInfoTypeInComplAndReqSubs(Int32? agencyUserId, Int32? agencyUserTemplateId, Int32 currentLoggedInUserID)
        {

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand("usp_UpdateSharedInfoTypeInComplAndReqSubs", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyUserID", agencyUserId);
                command.Parameters.AddWithValue("@AgencyUserTemplateID", agencyUserTemplateId);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserID);
                command.ExecuteScalar();
                con.Close();
            }
            return true;
        }

        List<SyncRequirementPackageObject> IProfileSharingRepository.GetReqPackageObjectList(List<Int32> lstReqSyncObjectIds, Int32 chnageTypeID_ReqCategory)
        {

            return base.SharedDataDBContext.SyncRequirementPackageObjects.Where(cnd => lstReqSyncObjectIds.Contains(cnd.SRPO_ID) && !cnd.SRPO_IsDeleted
                                                                                && cnd.SRPO_ChangeTypeID == chnageTypeID_ReqCategory).ToList();

        }

        public List<RequirementSharesDataContract> GetRequirementNonComplaintSharesData(String userId, Int32 currentLoggedInUserId, String tenantIds,
        ClinicalRotationDetailContract clinicalRotationSearchContract, CustomPagingArgsContract gridCustomPaging)
        {
            List<RequirementSharesDataContract> lstRequirementNonCompliantSharesDataContract = new List<RequirementSharesDataContract>();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetNonCompliantRequirementShares", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@LoggedInOrgUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@TenantIDs", tenantIds);
                command.Parameters.AddWithValue("@CRSearchContract", clinicalRotationSearchContract.XML);
                command.Parameters.AddWithValue("@PagingSortingData", gridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (!ds.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstRequirementNonCompliantSharesDataContract.Add(new RequirementSharesDataContract()
                        {
                            RotationID = dr["RotationID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RotationID"]),
                            ComplioID = dr["ComplioID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ComplioID"]),
                            RotationName = dr["Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["Name"]),
                            AgencyName = dr["AgencyName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AgencyName"]),
                            TotalRecordCount = dr["ActualTotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ActualTotalCount"]),
                            ApplicantFirstName = dr["ApplicantFirstName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ApplicantFirstName"]),
                            ApplicantLastName = dr["ApplicantLastName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ApplicantLastName"]),
                            TenantID = dr["TenantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TenantID"]),
                            AgencyID = dr["AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AgencyID"]),
                            TenantName = dr["InstitutionName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["InstitutionName"]),
                            IsProfileShared = dr["IsProfileShared"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsProfileShared"]),
                            IsRotationSharing = dr["IsRotationSharing"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsRotationSharing"]),
                            RotationSharedByUserName = dr["RotationSharedByUserName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationSharedByUserName"]),
                            RotationSharedByUserEmailId = dr["RotationSharedByUserEmailId"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationSharedByUserEmailId"]),
                            RotationSharedByUserPhoneNumber = dr["RotationSharedByUserPhoneNumber"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationSharedByUserPhoneNumber"]),
                            RotationSharedByUserID = dr["RotationSharedByUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RotationSharedByUserID"]),
                        });
                    }
                }
            }

            return lstRequirementNonCompliantSharesDataContract;
        }

        #region UAT-3977
        ClientContact IProfileSharingRepository.GetClientContact(Guid userID)
        {
            return _sharedDataDBContext.ClientContacts.Where(con => con.CC_UserID.HasValue && con.CC_UserID == userID && !con.CC_IsDeleted).FirstOrDefault();
        }

        #endregion

        #region UAT-3664

        List<AgencyUserReportPermissionContract> IProfileSharingRepository.GetAgencyUserReportPermissions(Int32 agencyUserId)
        {
            List<AgencyUserReportPermissionContract> lstAgencyUserReportPermissionContract = new List<AgencyUserReportPermissionContract>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@AgencyUserID", agencyUserId.ToString()),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyUserReportPermissions", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AgencyUserReportPermissionContract agencyUserReportPermissionContract = new AgencyUserReportPermissionContract();
                            agencyUserReportPermissionContract.AgencyUserID = dr["AgencyUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyUserID"]);
                            agencyUserReportPermissionContract.PermissionTypeID = dr["PermissionTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["PermissionTypeID"]);
                            agencyUserReportPermissionContract.PermissionAccessTypeID = dr["PermissionAccessTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["PermissionAccessTypeID"]);
                            agencyUserReportPermissionContract.PermissionTypeCode = dr["PermissionTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PermissionTypeCode"]);
                            agencyUserReportPermissionContract.PermissionAccessTypeCode = dr["PermissionAccessTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PermissionAccessTypeCode"]);
                            agencyUserReportPermissionContract.AgencyUserReportID = dr["AgencyUserReportID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyUserReportID"]);
                            agencyUserReportPermissionContract.AgencyUserReportCode = dr["AgencyUserReportCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyUserReportCode"]);
                            agencyUserReportPermissionContract.AgencyUserReportFolderPath = dr["AgencyUserReportFolderPath"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyUserReportFolderPath"]);
                            agencyUserReportPermissionContract.AgencyUserReportModule = dr["AgencyUserReportModule"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyUserReportModule"]);
                            agencyUserReportPermissionContract.ReportName = dr["ReportName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ReportName"]);

                            lstAgencyUserReportPermissionContract.Add(agencyUserReportPermissionContract);
                        }
                    }
                }
                return lstAgencyUserReportPermissionContract;
            }
        }

        List<AgencyUserPermissionTemplateMapping> IProfileSharingRepository.GetAgencyUserTemplateReportPermissions(Int32 templateId)
        {
            String reportPermissionTypeCode = AgencyUserPermissionType.REPORTS_PERMISSION.GetStringValue();
            return this.SharedDataDBContext.AgencyUserPermissionTemplateMappings.Where(cond => cond.AGUPTM_AgencyUserPerTemplateId == templateId && cond.lkpAgencyUserPermissionType.AUPT_Code == reportPermissionTypeCode).ToList();
        }
        #endregion

        #region UAT-4160
        private Boolean DeleteOrganizationUserTypeMapping(String agencyUserId, Int32 LoggedInUserId)
        {

            Boolean Result = false;
            if (!agencyUserId.IsNullOrEmpty())
            {
                Guid userId = new Guid(agencyUserId);

                //UAT-4160
                List<Entity.OrganizationUser> lstOrgUser = base.Context.OrganizationUsers.Where(con => con.IsDeleted != true
                                    && con.UserID == userId).ToList();
                Entity.OrganizationUser orgUser = lstOrgUser.Where(con => con.IsSharedUser == true).FirstOrDefault();
                if (!orgUser.IsNullOrEmpty())
                {
                    orgUser.OrganizationUserTypeMappings.Where(con => con.OTM_IsDeleted == false).ToList().ForEach(x =>
                    {
                        if (x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.AgencyUser.GetStringValue())// || x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Preceptor.GetStringValue())
                        {
                            x.OTM_IsDeleted = true;
                            x.OTM_ModifiedByID = LoggedInUserId;
                            x.OTM_ModifiedOn = DateTime.Now;
                        }
                    });
                    if (!(orgUser.OrganizationUserTypeMappings.Where(con => con.OTM_IsDeleted == false
                        && (con.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.ApplicantsSharedUser.GetStringValue()
                            || con.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Instructor.GetStringValue()
                            || con.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Preceptor.GetStringValue())).Any()))
                    {
                        orgUser.IsDeleted = true;
                        orgUser.ModifiedByID = LoggedInUserId;
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

                //Int32 organizationuserId = Context.OrganizationUsers.Where(con => con.UserID == userId && !con.IsDeleted && con.IsSharedUser == true).FirstOrDefault().OrganizationUserID;
                //String agencyUserTypeCode = OrganizationUserType.AgencyUser.GetStringValue();

                //Entity.OrganizationUserTypeMapping organizationUserTypeMapping = Context.OrganizationUserTypeMappings.Where(con => con.OTM_OrgUserID == organizationuserId && con.OTM_IsDeleted != true
                //                                                                                                        && con.lkpOrgUserType.OrgUserTypeCode == agencyUserTypeCode).FirstOrDefault();

                //if (!organizationUserTypeMapping.IsNullOrEmpty())
                //{
                //    organizationUserTypeMapping.OTM_IsDeleted = true;
                //    organizationUserTypeMapping.OTM_ModifiedByID = LoggedInUserId;
                //    organizationUserTypeMapping.OTM_ModifiedOn = DateTime.Now;
                //    if (Context.SaveChanges() > AppConsts.NONE)
                //        return true;

                //}
            }
            return Result;
        }
        #endregion

        #region UAT 4398
        List<Entity.SharedDataEntity.AgencyUser> IProfileSharingRepository.GetAgencyUserListByAgencIds(List<Int32> agencyIDs)
        {
            if ((agencyIDs.IsNotNull() && agencyIDs.Count > 0))
            {
                List<Int32> AgencyUserIDs = _sharedDataDBContext.UserAgencyMappings.Where(cmd => agencyIDs.Contains(cmd.UAM_AgencyID) && cmd.UAM_IsDeleted == false).Select(sel => sel.UAM_AgencyUserID).Distinct().ToList();

                if (AgencyUserIDs.IsNotNull() && AgencyUserIDs.Count > 0)
                {
                    return _sharedDataDBContext.AgencyUsers.Where(cmb => AgencyUserIDs.Contains(cmb.AGU_ID) && cmb.AGU_IsDeleted == false && cmb.AGU_UserID != null).ToList();
                }
            }
            return new List<Entity.SharedDataEntity.AgencyUser>();
        }

        Int32 IProfileSharingRepository.GetAgencyUserOrganizationUserId(Guid? agencyUserID)
        {
            int organizationUserId = 0;
            if (agencyUserID != Guid.Empty)
            {
                return base.Context.OrganizationUsers.First(cond => cond.UserID == agencyUserID && cond.IsDeleted == false).OrganizationUserID;
            }
            return organizationUserId;
        }
        #endregion

        //UAT-4658
        Boolean IProfileSharingRepository.IsAgencyUserPresent(Int32 templateId)
        {
            return this.SharedDataDBContext.AgencyUsers.Where(x => x.AGU_TemplateId == templateId && x.AGU_IsDeleted == false).Any();
        }
        //End UAT-4658
    }
}