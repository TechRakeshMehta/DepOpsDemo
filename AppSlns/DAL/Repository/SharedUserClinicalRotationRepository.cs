using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ProfileSharing;

namespace DAL.Repository
{
    public class SharedUserClinicalRotationRepository : BaseRepository, ISharedUserClinicalRotationRepository
    {
        #region Variables

        #region public Variables

        #endregion

        #region Private Variables

        private ADB_SharedDataEntities _sharedDataDBContext;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize the context
        /// </summary>
        public SharedUserClinicalRotationRepository()
        {
            _sharedDataDBContext = base.SharedDataDBContext;
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get shared user clinical rotations
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        List<ClinicalRotationDetailContract> ISharedUserClinicalRotationRepository.GetSharedUserClinicalRotations(Int32 currentLoggedInUserId, Int32 tenantId, Guid userId)
        {
            List<ClinicalRotationDetailContract> clinicalRotationDetailList = new List<ClinicalRotationDetailContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@LoggedInOrgUserID", currentLoggedInUserId),
                              new SqlParameter("@TenantID", tenantId),
                              new SqlParameter("@UserID", userId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSharedUserClinicalRotations", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationDetailContract clinicalRotationDetailContract = new ClinicalRotationDetailContract();
                            clinicalRotationDetailContract.ClientContactID = dr["ClientContactID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ClientContactID"]);
                            //clinicalRotationDetailContract.ProfileSharingInvGroupID = dr["ProfileSharingInvGroupID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ProfileSharingInvGroupID"]);
                            clinicalRotationDetailContract.AgencyID = dr["AgencyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyID"]);

                            clinicalRotationDetailList.Add(clinicalRotationDetailContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return clinicalRotationDetailList;
        }


        List<Int32> ISharedUserClinicalRotationRepository.GetSharedUserTenantIDs(Guid userId, Boolean isAgencyUser, Boolean isInstructor_Preceptor)
        {
            List<Int32> sharedUserTenantIDs = new List<Int32>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@UserID", userId),
                              new SqlParameter("@IsAgencyUser", isAgencyUser),
                              new SqlParameter("@IsInstructor_Preceptor", isInstructor_Preceptor)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSharedUserTenantIDs", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Int32 tenantID = dr["TenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TenantID"]);

                            sharedUserTenantIDs.Add(tenantID);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return sharedUserTenantIDs;
        }

        List<ClinicalRotationDetailContract> ISharedUserClinicalRotationRepository.SetProfileInvitationDetailData(
                                                                List<ClinicalRotationDetailContract> clinicalRotationDetailList, Int32 tenantID, String tenantName, Int32 currentLoggedInUSerID)
        {
            if (clinicalRotationDetailList.IsNotNull() && clinicalRotationDetailList.Count > AppConsts.NONE)
            {
                List<Int32> clinicalRotationIDs = clinicalRotationDetailList.Select(slct => slct.RotationID).Distinct().ToList();
                String rotationSharingInvGroupTypeCode = ProfileSharingInvitationGroupTypes.ROTATION_SHARING_TYPE.GetStringValue();
                String revokedInvStatusTypeCode = LkpInviationStatusTypes.REVOKED.GetStringValue();
                String invScheduledInvStatusTypeCode = LkpInviationStatusTypes.INVITATION_SCHEDULED.GetStringValue();
                String dataChangeInvStatusTypeCode = LkpInviationStatusTypes.DATA_CHANGED_INVITATION_REVOKED.GetStringValue();

                List<ProfileSharingInvitationGroup> lstProfileSharingInvitationGroup = _sharedDataDBContext.ProfileSharingInvitationGroups.Where(PSIG =>
                                                                                       PSIG.PSIG_ClinicalRotationID.HasValue
                                                                                       && clinicalRotationIDs.Contains(PSIG.PSIG_ClinicalRotationID.Value)
                                                                                       && PSIG.PSIG_IsDeleted == false && PSIG.lkpProfileSharingInvitationGroupType != null
                                                                                       && PSIG.lkpProfileSharingInvitationGroupType.PSIGT_Code == rotationSharingInvGroupTypeCode
                                                                                       && PSIG.PSIG_TenantID == tenantID)
                                                                                       .Join(_sharedDataDBContext.ProfileSharingInvitations, PSIG => PSIG.PSIG_ID,
                                                                                             PSI => PSI.PSI_ProfileSharingInvitationGroupID, (PSIG, PSI)
                                                                                             => new
                                                                                             {
                                                                                                 PSI_IsDeleted = PSI.PSI_IsDeleted,
                                                                                                 PSI_InviteeOrgUserID = PSI.PSI_InviteeOrgUserID,
                                                                                                 lkpInvitationStatus = PSI.lkpInvitationStatu,
                                                                                                 ProfileSharingInvitationGrp = PSIG
                                                                                             })
                                                                                       .Where(PSI1 =>
                                                                                              !PSI1.PSI_IsDeleted
                                                                                              && PSI1.lkpInvitationStatus != null
                                                                                              && PSI1.lkpInvitationStatus.Code != revokedInvStatusTypeCode
                                                                                              && PSI1.lkpInvitationStatus.Code != dataChangeInvStatusTypeCode
                                                                                              && PSI1.lkpInvitationStatus.Code != invScheduledInvStatusTypeCode
                                                                                              && PSI1.PSI_InviteeOrgUserID == currentLoggedInUSerID
                                                                                       ).Select(slct => slct.ProfileSharingInvitationGrp).ToList();
                var aaa = lstProfileSharingInvitationGroup.Select(s => s.PSIG_ID).ToList();
                //Set Invitation group id in clinicalRotation list.
                clinicalRotationDetailList.ForEach(clRotDetail =>
                {

                    Boolean isProfileShared = lstProfileSharingInvitationGroup.Any(cond => cond.PSIG_ClinicalRotationID == clRotDetail.RotationID);
                    clRotDetail.IsProfileShared = isProfileShared;
                    clRotDetail.TenantID = tenantID;
                    clRotDetail.TenantName = tenantName;
                });

            }
            return clinicalRotationDetailList;
        }

        /// <summary>
        /// UAT -3165 Provide the Rotation Custom Fields available for the Agency User to View on the Rotation Shares grid results and Attestation(get custom attributes using Tenant Ids and Rotation ids)
        /// </summary>
        /// <param name="TenantIds"></param>
        /// <param name="RotationIds"></param>
        /// <returns></returns>

        List<ClinicalRotationCustomAttributeContract> GetRotationCustomAttributes(Int32 TenantId, String RotationIds)
        {
            List<ClinicalRotationCustomAttributeContract> lstRotationCustomAttributes = new List<ClinicalRotationCustomAttributeContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@TenantId", TenantId),
                             new SqlParameter("@RotationIds",RotationIds)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader col = base.ExecuteSQLDataReader(con, "usp_GetRotationCustomAttributes", sqlParameterCollection))
                {
                    while (col.Read())
                    {
                        ClinicalRotationCustomAttributeContract CustomAttributeDetailContract = new ClinicalRotationCustomAttributeContract();
                        CustomAttributeDetailContract.TenantID = col["TenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TenantID"]);
                        CustomAttributeDetailContract.RotationID = col["RotationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RotationID"]);
                        CustomAttributeDetailContract.CustomAttributeList = col["CustomAttributeList"] == DBNull.Value ? String.Empty : Convert.ToString(col["CustomAttributeList"]);

                        lstRotationCustomAttributes.Add(CustomAttributeDetailContract);
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstRotationCustomAttributes;
        }


        /// <summary>
        /// Get shared user agencies
        /// </summary>
        /// <param name="tenantIDs"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<AgencyDetailContract> ISharedUserClinicalRotationRepository.GetSharedUserAgencies(String userID)
        {
            List<AgencyDetailContract> agencyDetailContractList = new List<AgencyDetailContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@UserID", new Guid(userID))
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader col = base.ExecuteSQLDataReader(con, "usp_GetInstructorPreceptorAgencies", sqlParameterCollection))
                {
                    while (col.Read())
                    {
                        AgencyDetailContract agencyDetailContract = new AgencyDetailContract();
                        agencyDetailContract.AgencyID = col["AgencyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["AgencyID"]);
                        agencyDetailContract.AgencyName = col["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(col["AgencyName"]);

                        agencyDetailContractList.Add(agencyDetailContract);
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return agencyDetailContractList;
        }

        #region Rotation Student Detail for Shared User

        /// <summary>
        /// Get the List of ApplicantDataListContract, ApplicantOrgUserID and ProfileSharingInvID
        /// </summary>
        /// <param name="rotationStudentDetailContract"></param>
        /// <returns></returns>
        List<ApplicantDataListContract> ISharedUserClinicalRotationRepository.GetApplicantIDsForRotationAndInvGrp(RotationStudentDetailContract rotationStudentDetailContract)
        {
            List<ApplicantDataListContract> applicantDataList = new List<ApplicantDataListContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@clinicalRotationID", rotationStudentDetailContract.ClinicalRotationID),
                            //new SqlParameter("@invitationGrpID", rotationStudentDetailContract.ProfileSharingInvGrpID),
                            new SqlParameter("@loggedInOrgUserID", rotationStudentDetailContract.LoggedInUserID),
                            new SqlParameter("@tenantID", rotationStudentDetailContract.SelectedTenantID),
                            new SqlParameter("@isAgencyUser", rotationStudentDetailContract.IsAgencyUser),
                            new SqlParameter("@AgencyID", rotationStudentDetailContract.AgencyID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationInvitationApplicants", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ApplicantDataListContract applicantDataListContract = new ApplicantDataListContract();
                            applicantDataListContract.OrganizationUserId = dr["ApplicantOrgUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantOrgUserID"]);
                            applicantDataListContract.ProfileSharingInvID = dr["ProfileSharingInvID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ProfileSharingInvID"]);
                            applicantDataListContract.InvitationDate = dr["InvitationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["InvitationDate"]);
                            applicantDataListContract.ExpirationDate = dr["ExpirationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationDate"]);
                            applicantDataListContract.ViewsRemaining = dr["ViewsRemaining"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ViewsRemaining"]);
                            applicantDataListContract.IsInvitationVisible = dr["IsInvitationVisible"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsInvitationVisible"]);
                            applicantDataListContract.IsApplicant = dr["IsApplicant"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsApplicant"]);
                            applicantDataListContract.InvitationReviewStatus = dr["ReviewStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ReviewStatus"]);
                            applicantDataListContract.InvitationReviewStatusCode = dr["ReviewStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ReviewStatusCode"]);
                            applicantDataListContract.ReviewBy = dr["ReviewBy"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ReviewBy"]);

                            applicantDataList.Add(applicantDataListContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return applicantDataList;
        }

        Boolean ISharedUserClinicalRotationRepository.UpdateInvitationExpirationRequested(List<ApplicantDataListContract> applicantDataList, Int32 currentUserId)
        {
            List<Int32> profileSharingInvIDs = applicantDataList.Select(x => x.ProfileSharingInvID).ToList();
            List<ProfileSharingInvitation> lstProfileSharingInvitation = _sharedDataDBContext.ProfileSharingInvitations.Where(con => profileSharingInvIDs.Contains(con.PSI_ID) && !con.PSI_IsDeleted).ToList();

            if (lstProfileSharingInvitation.IsNotNull())
            {
                lstProfileSharingInvitation.ForEach(cond =>
                {
                    cond.PSI_IsExpirationRequested = true;
                    cond.PSI_AuditRequestedDate = DateTime.Now;
                    cond.PSI_ModifiedById = currentUserId;
                    cond.PSI_ModifiedOn = DateTime.Now;
                });
            }
            if (_sharedDataDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        //UAT-3425
        Boolean ISharedUserClinicalRotationRepository.UpdateInvitationExpirationRequirementShares(List<Int32> profileSharingInvIDs, Int32 currentUserId)
        {
            List<ProfileSharingInvitation> lstProfileSharingInvitation = _sharedDataDBContext.ProfileSharingInvitations.Where(con => profileSharingInvIDs.Contains(con.PSI_ID) && !con.PSI_IsDeleted).ToList();

            if (lstProfileSharingInvitation.IsNotNull())
            {
                lstProfileSharingInvitation.ForEach(cond =>
                {
                    cond.PSI_IsExpirationRequested = true;
                    cond.PSI_AuditRequestedDate = DateTime.Now;
                    cond.PSI_ModifiedById = currentUserId;
                    cond.PSI_ModifiedOn = DateTime.Now;
                });
            }
            if (_sharedDataDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        #endregion

        #region UAT-1344:Automated NPI Number association and agency creation

        List<AgencyDataContract> ISharedUserClinicalRotationRepository.SaveUpdateAgencyInBulk(String xmlData, Int32 currentLoggedInUserId)
        {
            List<AgencyDataContract> lstAgencyData = new List<AgencyDataContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@xmlData", xmlData), 
                           new SqlParameter("@CurrentLoggedInUserID", currentLoggedInUserId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_AddUpdateAgenciesByXMLData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AgencyDataContract agencyData = new AgencyDataContract();

                            agencyData.AgencyDataID = Convert.ToInt32(dr["ID"]);
                            agencyData.IsAgencyCreated = dr["IsAgencyCreated"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsAgencyCreated"]);
                            agencyData.IsAgencyUploaded = dr["IsAgencyUploaded"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsAgencyUploaded"]);
                            agencyData.AgencyAddress1 = dr["AgencyAddress1"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyAddress1"]);
                            agencyData.AgencyAddress2 = dr["AgencyAddress2"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyAddress2"]);
                            agencyData.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            agencyData.NPINumber = dr["NPINumber"] == DBNull.Value ? String.Empty : Convert.ToString(dr["NPINumber"]);
                            agencyData.ZipCode = dr["ZipCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ZipCode"]);
                            agencyData.ReplacementNPI = dr["ReplacementNPI"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ReplacementNPI"]);
                            agencyData.StateAbbreviation = dr["StateAbbreviation"] == DBNull.Value ? String.Empty : Convert.ToString(dr["StateAbbreviation"]);
                            agencyData.City = dr["City"] == DBNull.Value ? String.Empty : Convert.ToString(dr["City"]);
                            lstAgencyData.Add(agencyData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstAgencyData;
        }
        #endregion

        /// <summary>
        /// Get Invitation Expiration search data
        /// </summary>
        /// <param name="invitationSearchContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        List<ProfileSharingInvitationSearchContract> ISharedUserClinicalRotationRepository.GetInvitationExpirationSearchData(ProfileSharingInvitationSearchContract invitationSearchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<ProfileSharingInvitationSearchContract> searchContractList = new List<ProfileSharingInvitationSearchContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@xmldata", invitationSearchContract.XML),
                              new SqlParameter("@filteringSortingData", customPagingArgsContract.XML)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetInvitationExpirationSearchData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ProfileSharingInvitationSearchContract searchContract = new ProfileSharingInvitationSearchContract();
                            searchContract.InvitationID = dr["InvitationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["InvitationID"]);
                            searchContract.Name = dr["Name"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Name"]);
                            searchContract.EmailAddress = dr["EmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(dr["EmailAddress"]);
                            searchContract.TenantName = dr["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TenantName"]);
                            searchContract.ViewsRemaining = dr["ViewsRemaining"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ViewsRemaining"]);
                            searchContract.ExpirationDate = dr["ExpirationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationDate"]);
                            searchContract.InvitationDate = dr["InvitationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["InvitationDate"]);
                            searchContract.TotalRecordCount = dr["TotalCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TotalCount"]);

                            searchContractList.Add(searchContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return searchContractList;
        }

        /// <summary>
        /// Save Update Profile Expiration Criteria
        /// </summary>
        /// <param name="invitationSearchContract"></param>
        /// <param name="lstInvitationIDs"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean ISharedUserClinicalRotationRepository.SaveUpdateProfileExpirationCriteria(ProfileSharingInvitationSearchContract invitationSearchContract, List<Int32> lstInvitationIDs, Int32 currentUserId)
        {
            List<ProfileSharingInvitation> lstInvitationData = _sharedDataDBContext.ProfileSharingInvitations.Where(cond => lstInvitationIDs.Contains(cond.PSI_ID) && !cond.PSI_IsDeleted).ToList();
            lstInvitationData.ForEach(invitation =>
            {
                invitation.PSI_ExpirationDate = invitationSearchContract.ExpirationDate;
                invitation.PSI_ExpirationTypeID = invitationSearchContract.ExpirationTypeId.Value;
                invitation.PSI_MaxViews = invitationSearchContract.MaxViews;
                invitation.PSI_InviteeViewCount = invitationSearchContract.InviteeViewCount;
                invitation.PSI_IsExpirationRequested = false;
                invitation.PSI_AuditRequestedDate = null;
                invitation.PSI_ModifiedOn = DateTime.Now;
                invitation.PSI_ModifiedById = currentUserId;
            });

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion

        //Common code is created instead of this method
        //List<AttestationDocumentContract> ISharedUserClinicalRotationRepository.GetAttestationDocumentsToExport(Int32 rotationID, Int32 currentLoggedInUserID)
        //{
        //    List<AttestationDocumentContract> lstDocContract = new List<AttestationDocumentContract>();

        //    List<ProfileSharingInvitationGroup> lstProfileSharingInvitationGrp = _sharedDataDBContext.ProfileSharingInvitationGroups
        //                                                        .Where(cond => cond.PSIG_ClinicalRotationID == rotationID
        //                                                        && !cond.PSIG_IsDeleted).ToList();

        //    foreach (ProfileSharingInvitationGroup PSIG in lstProfileSharingInvitationGrp)
        //    {
        //        Boolean isAllInvitationsExpired = true;
        //        List<ProfileSharingInvitation> lstProfileSharingInvitation = PSIG.ProfileSharingInvitations
        //                                                       .Where(cond => cond.PSI_InviteeOrgUserID.HasValue
        //                                                              && cond.PSI_InviteeOrgUserID.Value == currentLoggedInUserID
        //                                                              && !cond.PSI_IsDeleted).ToList();
        //        foreach (ProfileSharingInvitation PSI in lstProfileSharingInvitation)
        //        {
        //            if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.NO_EXPIRATION_CRITERIA.GetStringValue())
        //            {
        //                isAllInvitationsExpired = false;
        //                break;
        //            }
        //            else if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue())
        //            {
        //                if ((PSI.PSI_MaxViews - PSI.PSI_InviteeViewCount) != 0)
        //                {
        //                    isAllInvitationsExpired = false;
        //                    break;
        //                }
        //            }
        //            else if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.SPECIFIC_DATE.GetStringValue())
        //            {
        //                if (PSI.PSI_ExpirationDate > DateTime.Now)
        //                {
        //                    isAllInvitationsExpired = false;
        //                    break;
        //                }
        //            }
        //        }
        //        if (!isAllInvitationsExpired)
        //        {
        //            List<Int32> lstProfileSharingInvitationIDs = lstProfileSharingInvitation.Select(col => col.PSI_ID).ToList();

        //            //InvitationDocumentMapping docMapping = _sharedDataDBContext.InvitationDocumentMappings.Where(cond => !cond.IDM_IsDeleted
        //            //                                                                && cond.IDM_ProfileSharingInvitationID.HasValue
        //            //                                                                && lstProfileSharingInvitationIDs.Contains(cond.IDM_ProfileSharingInvitationID.Value)
        //            //                                                                && !cond.InvitationDocument.IND_IsDeleted).FirstOrDefault();

        //            //method converted to list for UAT-1557: WB: Addition of Vertical Attestation reports
        //            //It is done because for each invitation 2 reports will be generated=>Horizontal and vertical
        //            List<InvitationDocumentMapping> docMappingList = _sharedDataDBContext.InvitationDocumentMappings.Where(cond => !cond.IDM_IsDeleted
        //                                                                    && cond.IDM_ProfileSharingInvitationID.HasValue
        //                                                                    && lstProfileSharingInvitationIDs.Contains(cond.IDM_ProfileSharingInvitationID.Value)
        //                                                                    && !cond.InvitationDocument.IND_IsDeleted).ToList();

        //            foreach (InvitationDocumentMapping docMapping in docMappingList)
        //            {
        //                AttestationDocumentContract docContract = new AttestationDocumentContract()
        //                 {
        //                     DocumentFilePath = docMapping.InvitationDocument.IND_DocumentFilePath,
        //                     InvitationDocumentID = docMapping.InvitationDocument.IND_ID,
        //                     InvitationDocumentMappingID = docMapping.IDM_ID,
        //                     ProfileSharingInvitationGroupID = PSIG.PSIG_ID,
        //                     ProfileSharingInvitationID = docMapping.IDM_ProfileSharingInvitationID.Value,
        //                     SharedSystemDocumentTypecode = docMapping.InvitationDocument.lkpSharedSystemDocType.SSDT_Code

        //                 };
        //                if (docMapping.InvitationDocument.lkpSharedSystemDocType.SSDT_Code == LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue())
        //                {
        //                    docContract.IsVerticalAttestation = true;
        //                }
        //                lstDocContract.Add(docContract);
        //            }
        //        }
        //    }
        //    return lstDocContract;
        //}

        #region Common Code for Getting Attestation Document
        /// <summary>
        /// Common Code for getting Attestation Document
        /// </summary>
        /// <param name="lstProfileSharingInvitationIDs"></param>
        /// <returns></returns>
        List<InvitationDocumentMapping> ISharedUserClinicalRotationRepository.GetInvitationDocMappingForInvitaitonID(List<Int32> lstProfileSharingInvitationIDs)
        {
            List<InvitationDocumentMapping> docMappingList = _sharedDataDBContext.InvitationDocumentMappings.Where(cond => !cond.IDM_IsDeleted
                                                                            && cond.IDM_ProfileSharingInvitationID.HasValue
                                                                            && lstProfileSharingInvitationIDs.Contains(cond.IDM_ProfileSharingInvitationID.Value)
                                                                            && !cond.InvitationDocument.IND_IsDeleted).ToList();
            return docMappingList;
        }

        //List<ProfileSharingInvitationGroup> ISharedUserClinicalRotationRepository.GetAttestationDocumentForRotation(Int32 rotationID)
        //{
        //    List<ProfileSharingInvitationGroup> lstProfileSharingInvitationGrp = _sharedDataDBContext.ProfileSharingInvitationGroups
        //                                                        .Where(cond => cond.PSIG_ClinicalRotationID == rotationID
        //                                                        && !cond.PSIG_IsDeleted).ToList();
        //    return lstProfileSharingInvitationGrp;
        //}

        List<ProfileSharingInvitationGroup> ISharedUserClinicalRotationRepository.GetAttestationDocumentForRotation(List<InvitationIDsDetailContract> lstRotationTenant)
        {
            List<Int32> selectedRotationIds = lstRotationTenant.Select(col => col.RotationID).Distinct().ToList();
            List<ProfileSharingInvitationGroup> lstProfileSharingInvitationGrp = new List<ProfileSharingInvitationGroup>();

            foreach (int rotationID in selectedRotationIds)
            {
                List<Int32> agencyIDs = new List<int>();
                List<Int32> tenantIds = new List<int>();

                agencyIDs = lstRotationTenant.Where(cond => cond.RotationID == rotationID).Select(s => s.AgencyID).ToList();
                tenantIds = lstRotationTenant.Where(cond => cond.RotationID == rotationID).Select(s => s.TenantID).ToList();

                var groups = _sharedDataDBContext.ProfileSharingInvitationGroups
                                                    .Where(cond => cond.PSIG_ClinicalRotationID.Value == rotationID
                                                        && agencyIDs.Contains(cond.PSIG_AgencyID.Value)
                                                        && tenantIds.Contains(cond.PSIG_TenantID.Value)
                                                    && !cond.PSIG_IsDeleted).ToList();

                lstProfileSharingInvitationGrp.AddRange(groups);
            }


            List<ProfileSharingInvitationGroup> lstProfileSharingInvitationGrpFinal = new List<ProfileSharingInvitationGroup>();
            foreach (InvitationIDsDetailContract rotationTenantID in lstRotationTenant)
            {
                lstProfileSharingInvitationGrpFinal.AddRange(lstProfileSharingInvitationGrp.Where(cond => cond.PSIG_ClinicalRotationID == rotationTenantID.RotationID
                    && cond.PSIG_TenantID == rotationTenantID.TenantID
                    ).ToList());
            }
            return lstProfileSharingInvitationGrpFinal;
        }

        List<ProfileSharingInvitationGroup> ISharedUserClinicalRotationRepository.GetAttestationDocumentForInvitationGroup(List<Int32> profileSharingInvitationGroupID)
        {
            List<ProfileSharingInvitationGroup> lstProfileSharingInvitationGrp = _sharedDataDBContext.ProfileSharingInvitationGroups
                                                                .Where(cond => profileSharingInvitationGroupID.Contains(cond.PSIG_ID) && !cond.PSIG_IsDeleted).ToList();
            return lstProfileSharingInvitationGrp;
        }

        List<ProfileSharingInvitationGroup> ISharedUserClinicalRotationRepository.GetAttestationDocForProfileSharingInvitaiton(List<Int32> lstProfileSharingInvitationId)
        {
            List<ProfileSharingInvitationGroup> lstProfileSharingInvitationGrp = _sharedDataDBContext.InvitationDocumentMappings.Where(cond => !cond.IDM_IsDeleted
                                                                            && cond.IDM_ProfileSharingInvitationID.HasValue
                                                                            && lstProfileSharingInvitationId.Contains(cond.IDM_ProfileSharingInvitationID.Value)
                                                                            && !cond.InvitationDocument.IND_IsDeleted).Select(con => con.ProfileSharingInvitationGroup).ToList();
            return lstProfileSharingInvitationGrp;
        }

        #endregion
        #region ADB Admin Applicant Data Audit History

        /// <summary>
        /// Get Applicant Data Audit History records
        /// </summary>
        /// <param name="searchDataContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        List<ApplicantDataAuditHistoryContract> ISharedUserClinicalRotationRepository.GetApplicantDataAuditHistory(SearchItemDataContract searchDataContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<ApplicantDataAuditHistoryContract> searchContractList = new List<ApplicantDataAuditHistoryContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;           
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@searchDataXML", searchDataContract.CreateXml()),
                              new SqlParameter("@customFilteringXML", customPagingArgsContract.XML)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_ApplicantDataAuditHistory", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ApplicantDataAuditHistoryContract searchContract = new ApplicantDataAuditHistoryContract();
                            searchContract.ApplicantDataAuditMultiTenantID = dr["ApplicantDataAuditMultiTenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantDataAuditMultiTenantID"]);
                            searchContract.ApplicantDataAuditID = dr["ApplicantDataAuditID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantDataAuditID"]);
                            searchContract.TenantID = dr["TenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TenantID"]);
                            searchContract.TenantName = dr["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TenantName"]);
                            searchContract.PackageName = dr["PackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PackageName"]);
                            searchContract.CategoryName = dr["CategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CategoryName"]);
                            searchContract.ItemName = dr["ItemName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ItemName"]);
                            searchContract.ApplicantName = dr["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                            searchContract.AdminName = dr["AdminName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AdminName"]);
                            searchContract.TimeStampValue = dr["TimeStampValue"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["TimeStampValue"]);
                            searchContract.ChangeValue = dr["ChangeValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ChangeValue"]);
                            searchContract.TotalCount = dr["TotalCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TotalCount"]);

                            searchContractList.Add(searchContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return searchContractList;
        }

        #endregion

        #region UAT-1846
        /// <summary>
        /// Get the Shared Types and invitation date and profilesharingGroupID
        /// </summary>
        /// <param name="CurrentLoggedInUserID"></param>
        /// <returns></returns>
        List<ClinicalRotationDetailContract> ISharedUserClinicalRotationRepository.GetAttestationReportWithoutSignature(Int32 CurrentLoggedInUserID)
        {
            List<ClinicalRotationDetailContract> lstAttestationReportDataContract = new List<ClinicalRotationDetailContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@LoggedInOrgUserId", CurrentLoggedInUserID),
                         
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAttestationReportWithoutSignature", sqlParameterCollection))
                {
                    while (dr.Read())
                    {
                        ClinicalRotationDetailContract attestationReportDataContract = new ClinicalRotationDetailContract();

                        attestationReportDataContract.RotationName = dr["SharedType"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SharedType"]);
                        attestationReportDataContract.InvitationDate = Convert.ToDateTime(dr["InvitationDate"]);
                        attestationReportDataContract.ProfileSharingInvGroupID = dr["ProfileSharingGroupID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ProfileSharingGroupID"]);
                        attestationReportDataContract.TenantID = dr["TenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TenantID"]);
                        attestationReportDataContract.IsPDF = dr["IsPDF"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsPDF"]);
                        attestationReportDataContract.ComplioID = dr["ComplioID"] == DBNull.Value ? String.Empty: Convert.ToString(dr["ComplioID"]);
                        attestationReportDataContract.TenantName = dr["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TenantName"]);
                        attestationReportDataContract.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                        attestationReportDataContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                        attestationReportDataContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                        attestationReportDataContract.SchoolRepresentativeName = dr["SchoolRepresentativeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SchoolRepresentativeName"]);
                        lstAttestationReportDataContract.Add(attestationReportDataContract);
                    }
                }
            }
            return lstAttestationReportDataContract;
        }

        ///// <summary>
        ///// Get AttestationDocumentWithout Sign To Export UAT-1846
        ///// </summary>
        ///// <param name="InvitationGroupID"></param>
        ///// <param name="currentLoggedInUserID"></param>
        ///// <returns></returns>
        //List<AttestationDocumentContract> ISharedUserClinicalRotationRepository.GetAttestationDocumentsWithoutSignToExport(Int32 InvitationGroupID, Int32 currentLoggedInUserID)
        //{
        //    List<AttestationDocumentContract> lstDocContract = new List<AttestationDocumentContract>();

        //    ProfileSharingInvitationGroup lstProfileSharingInvitationGrp = _sharedDataDBContext.ProfileSharingInvitationGroups
        //                                                        .Where(cond => cond.PSIG_ID == InvitationGroupID
        //                                                        && !cond.PSIG_IsDeleted).FirstOrDefault();

        //    if (lstProfileSharingInvitationGrp.IsNotNull())
        //    {

        //        Boolean isAllInvitationsExpired = true;
        //        List<ProfileSharingInvitation> lstProfileSharingInvitation = lstProfileSharingInvitationGrp.ProfileSharingInvitations
        //                                                       .Where(cond => cond.PSI_InviteeOrgUserID.HasValue
        //                                                              && cond.PSI_InviteeOrgUserID.Value == currentLoggedInUserID
        //                                                              && !cond.PSI_IsDeleted).ToList();
        //        foreach (ProfileSharingInvitation PSI in lstProfileSharingInvitation)
        //        {
        //            if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.NO_EXPIRATION_CRITERIA.GetStringValue())
        //            {
        //                isAllInvitationsExpired = false;
        //                break;
        //            }
        //            else if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue())
        //            {
        //                if ((PSI.PSI_MaxViews - PSI.PSI_InviteeViewCount) != 0)
        //                {
        //                    isAllInvitationsExpired = false;
        //                    break;
        //                }
        //            }
        //            else if (PSI.lkpInvitationExpirationType.Code == InvitationExpirationTypes.SPECIFIC_DATE.GetStringValue())
        //            {
        //                if (PSI.PSI_ExpirationDate > DateTime.Now)
        //                {
        //                    isAllInvitationsExpired = false;
        //                    break;
        //                }
        //            }
        //        }
        //        if (!isAllInvitationsExpired)
        //        {
        //            List<Int32> lstProfileSharingInvitationIDs = lstProfileSharingInvitation.Select(col => col.PSI_ID).ToList();
        //            String ConsolidatedWithoutSign = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED_WITHOUGHT_SIGN.GetStringValue();
        //            List<InvitationDocumentMapping> docMappingList = _sharedDataDBContext.InvitationDocumentMappings.Where(cond => !cond.IDM_IsDeleted
        //                                                                            && cond.IDM_ProfileSharingInvitationID.HasValue
        //                                                                            && lstProfileSharingInvitationIDs.Contains(cond.IDM_ProfileSharingInvitationID.Value)
        //                                                                            && !cond.InvitationDocument.IND_IsDeleted
        //                                                                            && cond.InvitationDocument.lkpSharedSystemDocType.SSDT_Code == ConsolidatedWithoutSign).ToList();

        //            foreach (InvitationDocumentMapping docMapping in docMappingList)
        //            {
        //                AttestationDocumentContract docContract = new AttestationDocumentContract()
        //                {
        //                    DocumentFilePath = docMapping.InvitationDocument.IND_DocumentFilePath,
        //                    InvitationDocumentID = docMapping.InvitationDocument.IND_ID,
        //                    InvitationDocumentMappingID = docMapping.IDM_ID,
        //                    ProfileSharingInvitationGroupID = InvitationGroupID,
        //                    ProfileSharingInvitationID = docMapping.IDM_ProfileSharingInvitationID.Value,
        //                    SharedSystemDocumentTypecode = docMapping.InvitationDocument.lkpSharedSystemDocType.SSDT_Code

        //                };
        //                lstDocContract.Add(docContract);
        //            }
        //        }
        //    }
        //    return lstDocContract;
        //}

        #endregion

        List<ClinicalRotationDetailContract> ISharedUserClinicalRotationRepository.GetSharedUserClinicalRotationPackageDetails(ClinicalRotationDetailContract clinicalRotationDetailContract,
                                                                Int32 currentLoggedInUserId)
        {
            List<ClinicalRotationDetailContract> lstClinicalRotationDetailContract = new List<ClinicalRotationDetailContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@LoggedInUserId", currentLoggedInUserId),
                              new SqlParameter("@AgencyHierarchyIDs", clinicalRotationDetailContract.AgencyHierarchyIDs),
                              new SqlParameter("@CR_StartDate", clinicalRotationDetailContract.StartDate),
                              new SqlParameter("@CR_EndDate", clinicalRotationDetailContract.EndDate),
                              new SqlParameter("@SelectedTenantIds", clinicalRotationDetailContract.TenantIdList) //UAT-3596
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementPkgSubsByIP", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationDetailContract rotationDetail = new ClinicalRotationDetailContract();
                            rotationDetail.RotationID = dr["RotationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RotationID"]);
                            //rotationDetail.AgencyID = dr["AgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            rotationDetail.ReqPkgSubsID = dr["ReqPkgSubsID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ReqPkgSubsID"]);
                            rotationDetail.RotationName = dr["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationName"]);
                            //rotationDetail.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            rotationDetail.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            rotationDetail.RequirementPackageName = dr["ReqPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ReqPackageName"]);
                            rotationDetail.RequirementPackageStatus = dr["ReqPkgStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ReqPkgStatus"]);
                            rotationDetail.StartDate = dr["CR_StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CR_StartDate"]);
                            rotationDetail.EndDate = dr["CR_EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CR_EndDate"]);
                            rotationDetail.TenantID = dr["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TenantID"]);
                            rotationDetail.ComplioID = dr["ComplioId"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplioId"]); //UAT-3594
                            rotationDetail.CustomAttributes = dr["CustomAttributeList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CustomAttributeList"]); //UAT-3594
                            lstClinicalRotationDetailContract.Add(rotationDetail);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstClinicalRotationDetailContract;
        }

        #region 2475
        //UAT:2475
        List<INTSOF.UI.Contract.ProfileSharing.InvitationIDsDetailContract> ISharedUserClinicalRotationRepository.GetProfileSharingInvitationIdByRotationID(Int32 rotationID, Int32 currentLoggedInUserID, Int32 tenantID)
        {
            List<INTSOF.UI.Contract.ProfileSharing.InvitationIDsDetailContract> ProfileSharingInvitationData = new List<INTSOF.UI.Contract.ProfileSharing.InvitationIDsDetailContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetProfileSharingInvitationIdByRotationID", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@clinicalRotationID", rotationID);
                command.Parameters.AddWithValue("@loggedInUserID", currentLoggedInUserID);
                command.Parameters.AddWithValue("@tenantID", tenantID);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ProfileSharingInvitationData.Add(new INTSOF.UI.Contract.ProfileSharing.InvitationIDsDetailContract { ProfileSharingInvitationID = Convert.ToInt32(dr["PSI_ID"]), InvitationSource = dr["SourceType"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SourceType"]), TenantID = Convert.ToInt32(dr["PSI_TenantID"]) });
                    }
                }
                return ProfileSharingInvitationData;
            }
        }

        #endregion

        #region UAT-2313
        List<ClinicalRotationDetailContract> ISharedUserClinicalRotationRepository.GetClinicalRotationDataFromFlatTable(ClinicalRotationDetailContract clinicalRotationDetailContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<ClinicalRotationDetailContract> clinicalRotDetailList = new List<ClinicalRotationDetailContract>();
            String orderBy = "RotationID";
            String ordDirection = null;
            Int32? agencyID = clinicalRotationDetailContract.AgencyID == AppConsts.NONE ? (Int32?)null : clinicalRotationDetailContract.AgencyID;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@TenantIDList", clinicalRotationDetailContract.TenantIdList),
                            new SqlParameter("@AgencyIDList", clinicalRotationDetailContract.AgencyIdList),
                            new SqlParameter("@ComplioID", clinicalRotationDetailContract.ComplioID),
                            new SqlParameter("@RotationName", clinicalRotationDetailContract.RotationName),
                            new SqlParameter("@Department", clinicalRotationDetailContract.Department),
                            new SqlParameter("@Program", clinicalRotationDetailContract.Program),
                            new SqlParameter("@Course", clinicalRotationDetailContract.Course),
                            new SqlParameter("@Term", clinicalRotationDetailContract.Term),
                            new SqlParameter("@UnitFloorLoc", clinicalRotationDetailContract.UnitFloorLoc),
                            new SqlParameter("@NoOfHours", clinicalRotationDetailContract.RecommendedHours),                            
                            new SqlParameter("@NoofStudents",clinicalRotationDetailContract.Students),
                            new SqlParameter("@RotationShift", clinicalRotationDetailContract.Shift),
                            new SqlParameter("@StartTime", clinicalRotationDetailContract.StartTime),
                            new SqlParameter("@EndTime", clinicalRotationDetailContract.EndTime),
                            new SqlParameter("@StartDate", clinicalRotationDetailContract.StartDate),
                            new SqlParameter("@EndDate", clinicalRotationDetailContract.EndDate),
                            new SqlParameter("@DaysList", clinicalRotationDetailContract.DaysIdList),
                            new SqlParameter("@ContactList", clinicalRotationDetailContract.ContactIdList),
                            new SqlParameter("@TypeSpecialty", clinicalRotationDetailContract.TypeSpecialty),
                            new SqlParameter("@LoggedInOrgUserID", clinicalRotationDetailContract.CurrentLoggedInClientUserID),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize),                          
                            new SqlParameter("@ArchieveStatusID", clinicalRotationDetailContract.ArchieveStatusId)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClinicalRotationDataFromFlatTable", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            try
                            {
                                ClinicalRotationDetailContract clinicalRotDetail = new ClinicalRotationDetailContract();

                                clinicalRotDetail.RotationID = dr["RotationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RotationID"]);
                                clinicalRotDetail.AgencyID = dr["AgencyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyID"]);
                                clinicalRotDetail.ComplioID = dr["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplioID"]);
                                clinicalRotDetail.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                                clinicalRotDetail.RotationName = dr["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationName"]);
                                clinicalRotDetail.Department = dr["Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Department"]);
                                clinicalRotDetail.Program = dr["Program"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Program"]);
                                clinicalRotDetail.Course = dr["Course"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Course"]);
                                clinicalRotDetail.UnitFloorLoc = dr["UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UnitFloorLoc"]);
                                clinicalRotDetail.Shift = dr["RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationShift"]);
                                clinicalRotDetail.Term = dr["Term"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Term"]);
                                clinicalRotDetail.RecommendedHours = dr["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfHours"]));
                                clinicalRotDetail.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                                clinicalRotDetail.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                                clinicalRotDetail.StartTime = dr["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["StartTime"]);
                                clinicalRotDetail.EndTime = dr["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["EndTime"]);
                                clinicalRotDetail.Time = dr["Times"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Times"]);
                                clinicalRotDetail.DaysIdList = dr["DaysList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DaysList"]);
                                clinicalRotDetail.DaysName = dr["DaysName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DaysName"]);
                                clinicalRotDetail.ContactIdList = dr["ContactList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContactList"]);
                                clinicalRotDetail.ContactNames = dr["ContactsName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContactsName"]);
                                //clinicalRotDetail.SyllabusFileName = dr["SyllabusFileName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SyllabusFileName"]);
                                //clinicalRotDetail.SyllabusFilePath = dr["SyllabusFilePath"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SyllabusFilePath"]);
                                clinicalRotDetail.TotalRecordCount = dr["TotalCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TotalCount"]);
                                clinicalRotDetail.DaysBefore = dr["DaysBefore"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["DaysBefore"]);//change
                                clinicalRotDetail.Frequency = dr["Frequency"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Frequency"]);
                                clinicalRotDetail.TypeSpecialty = dr["TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TypeSpecialty"]);
                                clinicalRotDetail.Students = dr["NoOfStudents"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfStudents"]));
                                //clinicalRotDetail.HierarchyNodeIDList = dr["HierarchyNodeIDList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodeIDList"]);
                                //clinicalRotDetail.HierarchyNodes = dr["HierarchyNodes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodes"]);
                                // clinicalRotDetail.CustomAttributes = dr["CustomAttributeList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CustomAttributeList"]);
                                //UAT-2289
                                clinicalRotDetail.DeadlineDate = dr["DeadlineDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DeadlineDate"]);
                                clinicalRotDetail.TenantID = dr["TenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TenantID"]);
                                clinicalRotDetail.TenantName = dr["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TenantName"]);
                                clinicalRotDetailList.Add(clinicalRotDetail);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }
                }

                return clinicalRotDetailList;
            }
        }
        List<ClientContact> ISharedUserClinicalRotationRepository.GetAllClientContacts()
        {
            return _sharedDataDBContext.ClientContacts.Where(x => !x.CC_IsDeleted).ToList();
        }
        #endregion

        #region UAT-3316
        String ISharedUserClinicalRotationRepository.GetSharedUserTemplatePermissionsCode(Int32 organizationUserID, Boolean isCompliancePermissions)
        {
            String SharedUserTemplatePermissionsCode = String.Empty;
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@LoggedInUserId", organizationUserID),
                              new SqlParameter("@IsCompliancePermission", isCompliancePermissions),
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSharedUserTemplatePermissions", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SharedUserTemplatePermissionsCode = dr["SharedUserPermisisonsCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SharedUserPermisisonsCode"]);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return SharedUserTemplatePermissionsCode;
        }
        #endregion


        /// <summary>
        /// Get shared user agencies
        /// </summary>
        /// <param name="tenantIDs"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract> ISharedUserClinicalRotationRepository.GetSharedUserAgencyHierarchyRootNodes(String userID)
        {
            List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract> agencyDetailContractList = new List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@UserID", new Guid(userID))
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader col = base.ExecuteSQLDataReader(con, "usp_GetInstructorPreceptorAgencyHierarchyRootNode", sqlParameterCollection))
                {
                    while (col.Read())
                    {
                        INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract agencyDetailContract = new INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract();
                        agencyDetailContract.NodeID = col["AgencyHierarchyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["AgencyHierarchyID"]);
                        agencyDetailContract.HierarchyLabel = col["AgencyHierarchyLabel"] == DBNull.Value ? String.Empty : Convert.ToString(col["AgencyHierarchyLabel"]);

                        agencyDetailContractList.Add(agencyDetailContract);
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return agencyDetailContractList;
        }
    }
}
