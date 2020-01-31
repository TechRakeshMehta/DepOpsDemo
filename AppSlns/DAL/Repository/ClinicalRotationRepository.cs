using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;


namespace DAL.Repository
{
    public class ClinicalRotationRepository : ClientBaseRepository, IClinicalRotationRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public ClinicalRotationRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }



        #region Clinical Rotation Details

        /// <summary>
        /// Get Applicant Clinical Rotation search data
        /// </summary>
        /// <param name="searchDataContract"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        List<ApplicantDataListContract> IClinicalRotationRepository.GetApplicantClinicalRotationSearch(Int32 clinicalRotationId
                                                                                                       , ClinicalRotationSearchContract searchDataContract
                                                                                                       , CustomPagingArgsContract gridCustomPaging)
        {
            List<ApplicantDataListContract> applicantDataContractList = new List<ApplicantDataListContract>();
            string orderBy = QueueConstants.APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS;
            string ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@FirstName", searchDataContract.ApplicantFirstName),
                             new SqlParameter("@LastName", searchDataContract.ApplicantLastName),
                             new SqlParameter("@EmailAddress", searchDataContract.EmailAddress),
                             new SqlParameter("@SSN", searchDataContract.ApplicantSSN),
                             new SqlParameter("@DOB", searchDataContract.DateOfBirth),
                             new SqlParameter("@FilterUserGroupID", searchDataContract.FilterUserGroupID),
                             new SqlParameter("@OrderBy", orderBy),
                             new SqlParameter("@OrderDirection", ordDirection),
                             new SqlParameter("@PageIndex", gridCustomPaging.CurrentPageIndex),
                             new SqlParameter("@PageSize", gridCustomPaging.PageSize),
                             new SqlParameter("@LoggedInOrgUserID", searchDataContract.LoggedInUserId),
                             new SqlParameter("@LoggedInOrgUserTenantID", searchDataContract.LoggedInUserTenantId),
                             new SqlParameter("@ClinicalRotationID", clinicalRotationId),
                             new SqlParameter("@DPMIds", searchDataContract.SelectedDPMIds)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClinicalRotationDetailSearch", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ApplicantDataListContract applicantData = new ApplicantDataListContract();
                            applicantData.OrganizationUserId = dr["OrganizationUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OrganizationUserId"]);
                            applicantData.ApplicantFirstName = Convert.ToString(dr["ApplicantFirstName"]);
                            applicantData.ApplicantLastName = Convert.ToString(dr["ApplicantLastName"]);
                            applicantData.DateOfBirth = dr["DateOfBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DateOfBirth"]);
                            applicantData.EmailAddress = Convert.ToString(dr["EmailAddress"]);
                            applicantData.SSN = Convert.ToString(dr["SSN"]);
                            applicantData.TenantID = dr["TenantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TenantID"]);
                            applicantData.InstituteName = Convert.ToString(dr["InstituteName"]);
                            applicantData.UserGroups = Convert.ToString(dr["UserGroups"]);
                            applicantData.TotalCount = dr["TotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TotalCount"]);

                            applicantDataContractList.Add(applicantData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return applicantDataContractList;
        }

        #region UAT-3139: Client Admin Auto-archive rotations 1 year following the rotation end date.
        List<Int32> IClinicalRotationRepository.SetRotationsToArchive(Int32 chunkSize, Int32 systemUserId)
        {
            //var lstRotationToArchiveContract = new List<ClinicalRotationDetailContract>();
            var lstRotationIdsToArchive = new List<Int32>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@chunkSize", chunkSize),
                           new SqlParameter("@userId", systemUserId)
                        };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_RotationToArchive", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //rotationToArchiveContract.RotationID = dr["RotationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RotationID"]);
                            //lstRotationToArchiveContract.Add(rotationToArchiveContract);
                            Int32 rotationId;
                            rotationId = dr["RotationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RotationID"]);
                            lstRotationIdsToArchive.Add(rotationId);
                        }
                    }
                }
            }
            return lstRotationIdsToArchive;
        }
        #endregion


        /// <summary>
        /// Get Clinical Rotation by Clinical Rotation Id
        /// </summary>
        /// <param name="clinicalRotationId"></param>
        /// <returns></returns>
        ClinicalRotationDetailContract IClinicalRotationRepository.GetClinicalRotationById(Int32 clinicalRotationId, Int32? agencyID)
        {
            ClinicalRotationDetailContract clinicalRotationDetailContract = new ClinicalRotationDetailContract();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ClinicalRotationID", clinicalRotationId),
                             new SqlParameter("@AgencyID", agencyID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClinicalRotationDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clinicalRotationDetailContract.RotationID = dr["ClinicalRotationId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ClinicalRotationId"]);
                            clinicalRotationDetailContract.Department = Convert.ToString(dr["Department"]);
                            clinicalRotationDetailContract.Program = Convert.ToString(dr["Program"]);
                            clinicalRotationDetailContract.Course = Convert.ToString(dr["Course"]);
                            clinicalRotationDetailContract.UnitFloorLoc = Convert.ToString(dr["UnitFloorLoc"]);
                            clinicalRotationDetailContract.RecommendedHours = dr["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfHours"]));
                            clinicalRotationDetailContract.Shift = Convert.ToString(dr["RotationShift"]);
                            clinicalRotationDetailContract.RotationName = Convert.ToString(dr["RotationName"]);
                            clinicalRotationDetailContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            clinicalRotationDetailContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            clinicalRotationDetailContract.StartTime = dr["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(dr["StartTime"]);
                            clinicalRotationDetailContract.EndTime = dr["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(dr["EndTime"]);
                            clinicalRotationDetailContract.ComplioID = dr["ComplioID"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["ComplioID"]);
                            clinicalRotationDetailContract.Time = dr["Times"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Times"]);
                            clinicalRotationDetailContract.AgencyIDs = Convert.ToString(dr["AgencyIDs"]);
                            clinicalRotationDetailContract.AgencyName = Convert.ToString(dr["AgencyName"]);
                            clinicalRotationDetailContract.AgencyNameSpltdWithBreak = Convert.ToString(dr["AgencyNameSpltdWithBreak"]);
                            clinicalRotationDetailContract.DaysName = dr["Days"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Days"]);
                            clinicalRotationDetailContract.ContactNames = Convert.ToString(dr["ClientContactName"]);
                            clinicalRotationDetailContract.Term = Convert.ToString(dr["Term"]);
                            clinicalRotationDetailContract.TypeSpecialty = Convert.ToString(dr["TypeSpecialty"]);
                            //UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
                            clinicalRotationDetailContract.Students = dr["NoOfStudents"] == DBNull.Value ? (Int32?)null : (Int32?)(Convert.ToInt32(dr["NoOfStudents"]));
                            clinicalRotationDetailContract.IsEditableByAgencyUser = dr["IsEditableByAgencyUser"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsEditableByAgencyUser"]);
                            clinicalRotationDetailContract.IsEditableByClientAdmin = dr["IsEditableByClientAdmin"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsEditableByClientAdmin"]);
                            clinicalRotationDetailContract.CreatedDate = dr["RotationCreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RotationCreatedDate"]);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

            }
            return clinicalRotationDetailContract;
        }

        ///UAT-2040
        /// <summary>
        /// Get Clinical Rotation by Clinical Rotation Id
        /// </summary>
        /// <param name="clinicalRotationId"></param>
        /// <returns></returns>
        List<ClinicalRotationDetailContract> IClinicalRotationRepository.GetClinicalRotationByIds(String clinicalRotationIds)
        {
            List<ClinicalRotationDetailContract> lstClinicalRotationDetails = new List<ClinicalRotationDetailContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ClinicalRotationIDs", clinicalRotationIds)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClinicalRotationDetailsByIDs", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationDetailContract clinicalRotationDetailContract = new ClinicalRotationDetailContract();

                            clinicalRotationDetailContract.RotationID = dr["ClinicalRotationId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ClinicalRotationId"]);
                            clinicalRotationDetailContract.Department = Convert.ToString(dr["Department"]);
                            clinicalRotationDetailContract.Program = Convert.ToString(dr["Program"]);
                            clinicalRotationDetailContract.Course = Convert.ToString(dr["Course"]);
                            clinicalRotationDetailContract.UnitFloorLoc = Convert.ToString(dr["UnitFloorLoc"]);
                            clinicalRotationDetailContract.RecommendedHours = dr["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfHours"]));
                            clinicalRotationDetailContract.Shift = Convert.ToString(dr["RotationShift"]);
                            clinicalRotationDetailContract.RotationName = Convert.ToString(dr["RotationName"]);
                            clinicalRotationDetailContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            clinicalRotationDetailContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            clinicalRotationDetailContract.StartTime = dr["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(dr["StartTime"]);
                            clinicalRotationDetailContract.EndTime = dr["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(dr["EndTime"]);
                            clinicalRotationDetailContract.ComplioID = dr["ComplioID"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["ComplioID"]);
                            clinicalRotationDetailContract.Time = dr["Times"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Times"]);
                            //clinicalRotationDetailContract.AgencyID = dr["AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            clinicalRotationDetailContract.AgencyName = Convert.ToString(dr["AgencyName"]);
                            clinicalRotationDetailContract.AgencyNameSpltdWithBreak = Convert.ToString(dr["AgencyNameSpltdWithBreak"]);
                            clinicalRotationDetailContract.DaysName = dr["Days"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Days"]);
                            clinicalRotationDetailContract.ContactNames = Convert.ToString(dr["ClientContactName"]);
                            clinicalRotationDetailContract.Term = Convert.ToString(dr["Term"]);
                            clinicalRotationDetailContract.TypeSpecialty = Convert.ToString(dr["TypeSpecialty"]);
                            //UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
                            clinicalRotationDetailContract.Students = dr["NoOfStudents"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfStudents"]));
                            //UAT 3041
                            clinicalRotationDetailContract.IsEditableByClientAdmin = dr["IsEditableByClientAdmin"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsEditableByClientAdmin"]);
                            clinicalRotationDetailContract.IsEditableByAgencyUser = dr["IsEditableByAgencyUser"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsEditableByAgencyUser"]);
                            lstClinicalRotationDetails.Add(clinicalRotationDetailContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

            }
            return lstClinicalRotationDetails;
        }

        string IClinicalRotationRepository.GetClinicalRotationNotificationCustomAttributes(Int32 clinicalRotationId)
        {
            var customAttributes = _dbContext.CustomAttributeValues.Where(cav => cav.CAV_RecordID == clinicalRotationId
            && cav.CustomAttributeMapping.CustomAttribute.CA_IncludeInNotification)
            .Select(cav => new
            {
                AttributeName = cav.CustomAttributeMapping.CustomAttribute.CA_AttributeName,
                AttributeLabel = cav.CustomAttributeMapping.CustomAttribute.CA_AttributeLabel,
                AttributeValue = cav.CAV_AttributeValue
            }).ToList();

            var notificationCustomAttributes = string.Join(" ",
                customAttributes.Select(ca =>
                "<li><b>" + (string.IsNullOrWhiteSpace(ca.AttributeLabel) ? ca.AttributeName : ca.AttributeLabel)
                + " </b>: " + ca.AttributeValue + "</li>").ToArray());

            return string.IsNullOrWhiteSpace(notificationCustomAttributes) ? "" : string.Format("<div style='line-height:21px'><ul>{0}</ul></div>", notificationCustomAttributes);
        }

        /// <summary>
        /// Get Clinical Rotation members data
        /// </summary>
        /// <param name="clinicalRotationId"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        List<RotationMemberDetailContract> IClinicalRotationRepository.GetClinicalRotationMembers(Int32 clinicalRotationId, Int32 agencyID, Int32 currentLoggedInUserId)
        {
            List<RotationMemberDetailContract> rotationMemberDetailContractList = new List<RotationMemberDetailContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ClinicalRotationID", clinicalRotationId),
                             new SqlParameter("@AgencyID", agencyID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClinicalRotationMembers", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RotationMemberDetailContract rotationMemberDetailContract = new RotationMemberDetailContract();
                            rotationMemberDetailContract.ClinicalRotationMemberId = dr["ClinicalRotationMemberId"].GetType().Name == "DBNull"
                                                                                    ? 0 : Convert.ToInt32(dr["ClinicalRotationMemberId"]);
                            rotationMemberDetailContract.SchoolCompliance = dr["SchoolCompliance"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SchoolCompliance"]);
                            rotationMemberDetailContract.AgencyCompliance = dr["AgencyCompliance"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyCompliance"]);

                            rotationMemberDetailContract.SchoolCompliancePackageID = dr["SchoolCompliancePackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SchoolCompliancePackageID"]);
                            rotationMemberDetailContract.SchoolPackageSubscriptionID = dr["SchoolPackageSubscriptionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SchoolPackageSubscriptionID"]);

                            rotationMemberDetailContract.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementPackageID"]);
                            rotationMemberDetailContract.RequirementSubscriptionId = dr["RequirementSubscriptionId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementSubscriptionId"]);

                            //UAT-2544:
                            rotationMemberDetailContract.IsDropped = dr["IsDropped"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsDropped"]);
                            //UAT-3350
                            rotationMemberDetailContract.IsInstructor = dr["IsInstructor"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsInstructor"]);
                            rotationMemberDetailContract.RotationMemberDetail = new ApplicantDataListContract
                            {
                                OrganizationUserId = dr["OrganizationUserId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserId"]),
                                ApplicantFirstName = Convert.ToString(dr["ApplicantFirstName"]),
                                ApplicantLastName = Convert.ToString(dr["ApplicantLastName"]),
                                DateOfBirth = dr["DateOfBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DateOfBirth"]),
                                EmailAddress = Convert.ToString(dr["EmailAddress"]),
                                SSN = Convert.ToString(dr["SSN"]),
                                UserGroups = Convert.ToString(dr["UserGroups"]),
                                PhoneNumber = Convert.ToString(dr["PhoneNumber"]), //UAT-3552
                                CustomAttributes = Convert.ToString(dr["CustomAttributes"]),
                                //To be checked when UAT-2090 occurs.
                                Notes = Convert.ToString(dr["Notes"]),
                                ShareStatus = Convert.ToString(dr["ShareStatus"]),
                                InvitationReviewStatus = Convert.ToString(dr["IsInvitationApproved"]), //UAT-2443
                                StudentDroppedDate = dr["StudentDroppedDate"] == DBNull.Value || dr["StudentDroppedDate"].IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(dr["StudentDroppedDate"]) //UAT-4460
                            };
                            rotationMemberDetailContractList.Add(rotationMemberDetailContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return rotationMemberDetailContractList;
        }

        /// <summary>
        /// To add applicants to Clinical Rotation
        /// </summary>
        /// <param name="clinicalRotationID"></param>
        /// <param name="requirementNotCompliantPackStatusID"></param>
        /// <param name="rotationSubscriptionTypeID"></param>
        /// <param name="organizationUserIds"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        Boolean IClinicalRotationRepository.AddApplicantsToRotation(Int32 clinicalRotationID, Int32 requirementNotCompliantPackStatusID, Int32 rotationSubscriptionTypeID, Dictionary<Int32, Boolean> organizationUserIds, Int32 currentLoggedInUserId, Int32 reqPkgTypeId, Int16 statusId)
        {
            try
            {
                organizationUserIds.ForEach(cond =>
                    {
                        ClinicalRotationMember clinicalRotationMember = new ClinicalRotationMember();
                        clinicalRotationMember.CRM_ClinicalRotationID = clinicalRotationID;
                        clinicalRotationMember.CRM_ApplicantOrgUserID = cond.Key;
                        clinicalRotationMember.CRM_IsDeleted = false;
                        clinicalRotationMember.CRM_CreatedByID = currentLoggedInUserId;
                        clinicalRotationMember.CRM_CreatedOn = DateTime.Now;
                        _dbContext.ClinicalRotationMembers.AddObject(clinicalRotationMember);
                    });
                //Add Requirement Package Subscription for applicants
                List<RequirementPackageSubscription> lstRequirementPackageSubscription = AddRequirementPackageSubscription(clinicalRotationID, requirementNotCompliantPackStatusID, rotationSubscriptionTypeID, organizationUserIds, currentLoggedInUserId, reqPkgTypeId);
                //UAT-4460
                ClinicalRotation clinicalRotation = _dbContext.ClinicalRotations.Where(where => where.CR_ID == clinicalRotationID).FirstOrDefault();
                if (clinicalRotation.CR_IsDropped)
                {
                    clinicalRotation.CR_IsDropped = false;
                    clinicalRotation.CR_DroppedOn = null;
                }
                //END UAT-4460
                if (_dbContext.SaveChanges() > 0)
                {
                    //UAt-2165: Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                    String packageSubscriptionIdsXML = "<PackageSubscriptionIDs>";
                    foreach (RequirementPackageSubscription requirementPackageSubscription in lstRequirementPackageSubscription)
                    {
                        packageSubscriptionIdsXML += "<ID>" + requirementPackageSubscription.RPS_ID + "</ID>";
                    }
                    packageSubscriptionIdsXML += "</PackageSubscriptionIDs>";
                    CreateOptionalCategorySetAproved(packageSubscriptionIdsXML, currentLoggedInUserId);
                    //UAT-2603
                    List<Int32> lstReqSubsIds = lstRequirementPackageSubscription.Select(cond => cond.RPS_ID).ToList();
                    AddDataToRotDataMovement(lstReqSubsIds, currentLoggedInUserId, statusId);
                    return true;
                }
                return false;
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

        /// <summary>
        /// Remove applicants from rotation
        /// </summary>
        /// <param name="clinicalRotationMemberIDs"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        RotationStudentDropped IClinicalRotationRepository.RemoveApplicantsFromRotation(Dictionary<Int32, Boolean> clinicalRotationMemberIDs, Int32 currentLoggedInUserId, Int32 reqPkgTypeId
                                                                         , List<Int32> approvedMemberIdsToRemove, Int32 tenantId, Int16 dataMovementDueStatusId, Int16 dataMovementNotRequiredStatusId)
        {
            try
            {
                //UAT-3222
                RotationStudentDropped rotationStudentDropped = new RotationStudentDropped();

                //UAT-2544:
                List<ClinicalRotationMember> lstClinicalRotationMemberAllData = _dbContext.ClinicalRotationMembers.Where(cond => (clinicalRotationMemberIDs.Keys.Contains(cond.CRM_ID) || approvedMemberIdsToRemove.Contains(cond.CRM_ID))
                                                                                                                                  && !cond.CRM_IsDeleted).ToList();
                List<ClinicalRotationMember> lstClinicalRotationMember = lstClinicalRotationMemberAllData.Where(cnd => !approvedMemberIdsToRemove.Contains(cnd.CRM_ID)).ToList();
                List<ClinicalRotationMember> lstClinicalRotationMemberToSetDropped = lstClinicalRotationMemberAllData.Where(cnd => approvedMemberIdsToRemove.Contains(cnd.CRM_ID)).ToList();
                Int32? inviteeOrgId = null;
                if (!lstClinicalRotationMember.IsNullOrEmpty())
                {
                    Int32 clinicalRotationID = lstClinicalRotationMember.FirstOrDefault().CRM_ClinicalRotationID;
                    List<Int32> organizationUserIds = lstClinicalRotationMember.Select(x => x.CRM_ApplicantOrgUserID).ToList();
                    //Remove Requirement Package Subscription for applicants
                    List<Int32> lstRemoveReqPkgSubId = RemoveRequirementPackageSubscription(clinicalRotationID, organizationUserIds, currentLoggedInUserId, reqPkgTypeId);

                    #region UAT-3623
                    //var flatTableDatatoDelete = SharedDataDBContext.FlatRequirementVerificationDetailDatas.Where(frvdd => organizationUserIds.Contains((int)frvdd.FRVDD_ApplicantId) && frvdd.FRVDD_TenantId == tenantId && frvdd.FRVDD_RotationId == clinicalRotationID && frvdd.FRVDD_RotationStartDate > DateTime.Today);
                    var flatTableDatatoDelete = SharedDataDBContext.FlatRequirementVerificationDetailDatas.Where(frvdd => organizationUserIds.Contains((int)frvdd.FRVDD_ApplicantId) && frvdd.FRVDD_TenantId == tenantId && frvdd.FRVDD_RotationId == clinicalRotationID);
                    flatTableDatatoDelete.ForEach(frvdd =>
                    {
                        frvdd.FRVDD_IsActive = false;
                        frvdd.FRVDD_IsDeleted = true;
                        frvdd.FRVDD_ModifiedBy = currentLoggedInUserId;
                        frvdd.FRVDD_ModifiedOn = DateTime.Now;
                    });
                    SharedDataDBContext.SaveChanges();
                    #endregion

                    lstClinicalRotationMember.ForEach(cond =>
                    {
                        cond.CRM_IsDeleted = true;
                        cond.CRM_ModifiedByID = currentLoggedInUserId;
                        cond.CRM_ModifiedOn = DateTime.Now;
                    });
                    //UAT-4460
                    if (lstClinicalRotationMemberToSetDropped.IsNullOrEmpty())
                    {
                        List<Int32> lstCRMIds = lstClinicalRotationMember.Select(sel => sel.CRM_ID).ToList();
                        Boolean IsAllMembersDropped = _dbContext.ClinicalRotationMembers.Where(cond => cond.CRM_ClinicalRotationID == clinicalRotationID && !cond.CRM_IsDeleted && !(lstCRMIds.Contains(cond.CRM_ID))).All(where => where.CRM_IsDropped) ? true : false;
                        if (IsAllMembersDropped)
                        {
                            ClinicalRotation clinicalRotationToUpdate = _dbContext.ClinicalRotations.Where(cond => cond.CR_ID == clinicalRotationID).FirstOrDefault();
                            if (!clinicalRotationToUpdate.IsNullOrEmpty())
                            {
                                clinicalRotationToUpdate.CR_IsDropped = true;
                                clinicalRotationToUpdate.CR_DroppedOn = DateTime.Now;
                                clinicalRotationToUpdate.CR_ModifiedByID = currentLoggedInUserId;
                                clinicalRotationToUpdate.CR_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                    //End UAT
                    //UAT-2603
                    if (!lstRemoveReqPkgSubId.IsNullOrEmpty())
                    {
                        UpdateDataMovementStatus(lstRemoveReqPkgSubId, dataMovementDueStatusId, dataMovementNotRequiredStatusId, currentLoggedInUserId);
                    }
                }
                //UAT-2544:
                if (!lstClinicalRotationMemberToSetDropped.IsNullOrEmpty())
                {
                    Int32 clinicalRotationID = lstClinicalRotationMemberToSetDropped.FirstOrDefault().CRM_ClinicalRotationID;

                    List<Int32> lstProfileSharingInvitationGroupIDs = SharedDataDBContext.ProfileSharingInvitationGroups.Where(cond => cond.PSIG_ClinicalRotationID == clinicalRotationID && !cond.PSIG_IsDeleted).Select(sel => sel.PSIG_ID).ToList();

                    lstClinicalRotationMemberToSetDropped.ForEach(cond =>
                     {
                         cond.CRM_IsDropped = true;
                         cond.CRM_DroppedOn = DateTime.Now;
                         cond.CRM_ModifiedByID = currentLoggedInUserId;
                         cond.CRM_ModifiedOn = DateTime.Now;
                         //Start UAT-4460
                         Int32 appOrguserID = cond.CRM_ApplicantOrgUserID;
                         String invitationReviewDroppedCode = SharedUserInvitationReviewStatus.Dropped.GetStringValue();
                         //Entity.SharedDataEntity.ProfileSharingInvitation profileSharingInvitation 
                         if (!lstProfileSharingInvitationGroupIDs.IsNullOrEmpty())
                         {
                             String sharedUserInvitationStatusCode = SharedUserInvitationReviewStatus.Dropped.GetStringValue();
                             List<Int32> lstPSI_IDs = SharedDataDBContext.ProfileSharingInvitations.Where(cond2 => !cond2.PSI_IsDeleted && lstProfileSharingInvitationGroupIDs.Contains(cond2.PSI_ProfileSharingInvitationGroupID.Value) && cond2.PSI_ApplicantOrgUserID == appOrguserID).Select(sel => sel.PSI_ID).ToList();
                             if (!lstPSI_IDs.IsNullOrEmpty())
                             {
                                 var lstsharedUserInvitationReview = SharedDataDBContext.SharedUserInvitationReviews.Where(cond2 => !cond2.SUIR_IsDeleted && lstPSI_IDs.Contains(cond2.SUIR_ProfileSharingInvitationID)).ToList();
                                 inviteeOrgId = SharedDataDBContext.ProfileSharingInvitations.Where(cond2 => !cond2.PSI_IsDeleted && cond2.PSI_ID == lstPSI_IDs.FirstOrDefault()).Select(sel => sel.PSI_InviteeOrgUserID).FirstOrDefault();
                                 Int32 droppedRotationReviewID = SharedDataDBContext.lkpSharedUserInvitationReviewStatus.Where(cond2 => !cond2.SUIRS_IsDeleted && cond2.SUIRS_Code == invitationReviewDroppedCode).Select(sel => sel.SUIRS_ID).FirstOrDefault();
                                 if (!lstsharedUserInvitationReview.IsNullOrEmpty())
                                 {
                                     foreach (var index in lstsharedUserInvitationReview)
                                     {
                                         index.SUIR_ReviewByID = currentLoggedInUserId;
                                         index.SUIR_ReviewStatusID = droppedRotationReviewID;
                                         index.SUIR_ModifiedByID = currentLoggedInUserId;
                                         index.SUIR_ModifiedOn = DateTime.Now;
                                     }
                                 }
                                 SharedDataDBContext.SaveChanges();
                             }
                         }
                         //End UAT
                     });
                }

                if (_dbContext.SaveChanges() > 0)
                {
                    //UAT-2544:Approved Rotation Student Sharing Functionality
                    if (!lstClinicalRotationMemberToSetDropped.IsNullOrEmpty())
                    {
                        Int32 clinicalRotationID = lstClinicalRotationMemberToSetDropped.FirstOrDefault().CRM_ClinicalRotationID;
                        Boolean isAllMembersAreDropped = !_dbContext.ClinicalRotationMembers.Any(x => x.CRM_ClinicalRotationID == clinicalRotationID && !x.CRM_IsDeleted && !x.CRM_IsDropped);
                        if (isAllMembersAreDropped)
                        {
                            List<Entity.SharedDataEntity.ProfileSharingInvitationGroup> lstProfileSharingInvitationGroup = SharedDataDBContext.ProfileSharingInvitationGroups.Where(cond => cond.PSIG_ClinicalRotationID == clinicalRotationID && !cond.PSIG_IsDeleted).ToList(); //UAT-4460

                            ClinicalRotation clinicalRotationToUpdate = _dbContext.ClinicalRotations.FirstOrDefault(cond => cond.CR_ID == clinicalRotationID && !cond.CR_IsDeleted);
                            if (!clinicalRotationToUpdate.IsNullOrEmpty())
                            {
                                clinicalRotationToUpdate.CR_IsDropped = true;
                                clinicalRotationToUpdate.CR_DroppedOn = DateTime.Now;
                                clinicalRotationToUpdate.CR_ModifiedByID = currentLoggedInUserId;
                                clinicalRotationToUpdate.CR_ModifiedOn = DateTime.Now;

                            }

                            //UAT-4460

                            List<SharedUserRotationReview> lstSharedUserRotationReview = _dbContext.SharedUserRotationReviews.Where(cond => !cond.SURR_IsDeleted && cond.SURR_ClinicalRotaionID == clinicalRotationID).ToList();
                            String rotationDroppedReviewCode = SharedUserRotationReviewStatus.Dropped.GetStringValue();
                            Int32 rotationDroppedReviewID = _dbContext.lkpSharedUserRotationReviewStatus.Where(cond => cond.SURRS_Code == rotationDroppedReviewCode && !cond.SURRS_IsDeleted).Select(col => col.SURRS_ID).FirstOrDefault();
                            if (!lstSharedUserRotationReview.IsNullOrEmpty())
                            {
                                foreach (var sharedUserRotationReview in lstSharedUserRotationReview)
                                {
                                    sharedUserRotationReview.SURR_RotationReviewStatusID = rotationDroppedReviewID;
                                    sharedUserRotationReview.SURR_ReviewByID = currentLoggedInUserId;
                                    sharedUserRotationReview.SURR_ModifiedByID = currentLoggedInUserId;
                                    sharedUserRotationReview.SURR_ModifiedOn = DateTime.Now;
                                }
                            }
                            _dbContext.SaveChanges();


                            //END UAT
                        }
                        List<Int32> clinicalMemberIds = lstClinicalRotationMemberToSetDropped.Select(x => x.CRM_ID).ToList();
                        CreateShanpshotForDroppedStudent(clinicalRotationID, clinicalMemberIds, currentLoggedInUserId, tenantId);

                        //UAT-3222
                        rotationStudentDropped = GenerateDataForStudentDroppedFromRotation(clinicalRotationID, clinicalMemberIds, currentLoggedInUserId, tenantId);
                        rotationStudentDropped.InviteeOrgId = inviteeOrgId.Value.IsNullOrEmpty() ? 0 : inviteeOrgId.Value;
                        // This is done because 'IsRemovedApplicantsFromRotation' returns to method
                        // 'IsRemovedApplicantsFromRotation' is set to true only when any applicant is dropped or sharing is already done 
                        // Bug ID: 24887
                        rotationStudentDropped.IsRemovedApplicantsFromRotation = true;
                    }                     
                     
                    //return true;
                }
                else
                {
                    //return false;
                    rotationStudentDropped.IsRemovedApplicantsFromRotation = false;
                }
                return rotationStudentDropped;
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

        /// <summary>
        /// Add Requirement Package Subscription for applicants
        /// </summary>
        /// <param name="clinicalRotationID"></param>
        /// <param name="organizationUserIds"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public List<RequirementPackageSubscription> AddRequirementPackageSubscription(Int32 clinicalRotationID, Int32 requirementNotCompliantPackStatusID, Int32 rotationSubscriptionTypeID, Dictionary<Int32, Boolean> organizationUserIds, Int32 currentLoggedInUserId, Int32 reqPkgTypeId)
        {
            try
            {
                ClinicalRotationRequirementPackage clinicalRotationRequirementPackage = GetRotationRequirementPackageByRotationId(clinicalRotationID, reqPkgTypeId);

                List<RequirementPackageSubscription> lstRequirementPackageSubscription = new List<RequirementPackageSubscription>();
                if (clinicalRotationRequirementPackage.IsNotNull())
                {
                    organizationUserIds.ForEach(cond =>
                    {
                        //Add records in RequirementPackageSubscription table
                        RequirementPackageSubscription requirementPackageSubscription = new RequirementPackageSubscription();
                        requirementPackageSubscription.RPS_RequirementPackageID = clinicalRotationRequirementPackage.CRRP_RequirementPackageID;
                        requirementPackageSubscription.RPS_RequirementSubscriptionTypeID = rotationSubscriptionTypeID;
                        requirementPackageSubscription.RPS_ApplicantOrgUserID = cond.Key;
                        requirementPackageSubscription.RPS_RequirementPackageStatusID = requirementNotCompliantPackStatusID;
                        requirementPackageSubscription.RPS_IsDeleted = false;
                        requirementPackageSubscription.RPS_CreatedByID = currentLoggedInUserId;
                        requirementPackageSubscription.RPS_CreatedOn = DateTime.Now;

                        //Add records in ClinicalRotationSubscription table
                        ClinicalRotationSubscription clinicalRotationSubscription = new ClinicalRotationSubscription();
                        clinicalRotationSubscription.CRS_ClinicalRotationRequirementPackageID = clinicalRotationRequirementPackage.CRRP_ID;
                        clinicalRotationSubscription.CRS_IsDeleted = false;
                        clinicalRotationSubscription.CRS_CreatedByID = currentLoggedInUserId;
                        clinicalRotationSubscription.CRS_CreatedOn = DateTime.Now;

                        requirementPackageSubscription.ClinicalRotationSubscriptions.Add(clinicalRotationSubscription);
                        _dbContext.RequirementPackageSubscriptions.AddObject(requirementPackageSubscription);
                        lstRequirementPackageSubscription.Add(requirementPackageSubscription);
                    });

                }
                return lstRequirementPackageSubscription;
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

        /// <summary>
        /// Remove Requirement Package Subscription for applicants
        /// </summary>
        /// <param name="clinicalRotationID"></param>
        /// <param name="organizationUserIds"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        private List<Int32> RemoveRequirementPackageSubscription(Int32 clinicalRotationID, List<Int32> organizationUserIds, Int32 currentLoggedInUserId, Int32 reqPkgTypeId)
        {
            try
            {
                List<Int32> lstRemoveReqPkgSubsId = new List<int>();
                ClinicalRotationRequirementPackage clinicalRotationRequirementPackage = GetRotationRequirementPackageByRotationId(clinicalRotationID, reqPkgTypeId);
                if (clinicalRotationRequirementPackage.IsNotNull())
                {
                    List<ClinicalRotationSubscription> lstClinicalRotationSubscription = GetClinicalRotationSubscriptionsByRotationReqPackId(clinicalRotationRequirementPackage.CRRP_ID);
                    if (lstClinicalRotationSubscription.IsNotNull())
                    {
                        List<Int32> lstRequirementPackageSubscriptionID = lstClinicalRotationSubscription.Select(x => x.CRS_RequirementPackageSubscriptionID).ToList();
                        List<RequirementPackageSubscription> lstRequirementPackageSubscription = _dbContext.RequirementPackageSubscriptions.Where(con => organizationUserIds.Contains(con.RPS_ApplicantOrgUserID)
                            && lstRequirementPackageSubscriptionID.Contains(con.RPS_ID) && !con.RPS_IsDeleted).ToList();
                        lstRequirementPackageSubscription.ForEach(con =>
                        {

                            Int32 anotherClinicalReqSubscriptionsCount = _dbContext.ClinicalRotationSubscriptions
                                  .Where(cond => cond.CRS_RequirementPackageSubscriptionID == con.RPS_ID
                                          && cond.CRS_ClinicalRotationRequirementPackageID != clinicalRotationRequirementPackage.CRRP_ID
                                          && !cond.CRS_IsDeleted).Count();

                            if (anotherClinicalReqSubscriptionsCount == 0)
                            {
                                con.RPS_IsDeleted = true;
                                con.RPS_ModifiedByID = currentLoggedInUserId;
                                con.RPS_ModifiedOn = DateTime.Now;

                                lstRemoveReqPkgSubsId.Add(con.RPS_ID);
                            }

                            lstClinicalRotationSubscription.Where(x => con.RPS_ID == x.CRS_RequirementPackageSubscriptionID).ForEach(y =>
                            {
                                y.CRS_IsDeleted = true;
                                y.CRS_ModifiedByID = currentLoggedInUserId;
                                y.CRS_ModifiedOn = DateTime.Now;
                            });
                        });


                    }
                }
                return lstRemoveReqPkgSubsId;
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

        /// <summary>
        /// Add requirement package to rotation
        /// </summary>
        /// <param name="clinicalRotationID"></param>
        /// <param name="requirementPackageID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        Boolean IClinicalRotationRepository.AddPackageToRotation(Int32 clinicalRotationID, Int32 requirementPackageID, Int32 currentLoggedInUserId, Int32 reqPkgTypeId)
        {
            try
            {
                ClinicalRotationRequirementPackage clinicalRotationRequirementPackage = GetRotationRequirementPackageByRotationId(clinicalRotationID, reqPkgTypeId);
                if (clinicalRotationRequirementPackage.IsNotNull() && clinicalRotationRequirementPackage.CRRP_ID > 0)
                {
                    clinicalRotationRequirementPackage.CRRP_RequirementPackageID = requirementPackageID;
                    clinicalRotationRequirementPackage.CRRP_ModifiedByID = currentLoggedInUserId;
                    clinicalRotationRequirementPackage.CRRP_ModifiedOn = DateTime.Now;
                }
                else
                {
                    ClinicalRotationRequirementPackage rotationRequirementPackage = new ClinicalRotationRequirementPackage();
                    rotationRequirementPackage.CRRP_ClinicalRotationID = clinicalRotationID;
                    rotationRequirementPackage.CRRP_RequirementPackageID = requirementPackageID;
                    rotationRequirementPackage.CRRP_IsDeleted = false;
                    rotationRequirementPackage.CRRP_CreatedByID = currentLoggedInUserId;
                    rotationRequirementPackage.CRRP_CreatedOn = DateTime.Now;
                    _dbContext.ClinicalRotationRequirementPackages.AddObject(rotationRequirementPackage);
                }

                if (_dbContext.SaveChanges() > 0)
                    return true;
                return false;
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

        /// <summary>
        /// Get clinical rotation requirement package by clinical rotation ID
        /// </summary>
        /// <param name="clinicalRotationID"></param>
        /// <returns></returns>
        public ClinicalRotationRequirementPackage GetRotationRequirementPackageByRotationId(Int32 clinicalRotationID, Int32 reqPkgTypeId)
        {
            return _dbContext.ClinicalRotationRequirementPackages.FirstOrDefault(x => x.CRRP_ClinicalRotationID == clinicalRotationID
                                                                                    && !x.CRRP_IsDeleted && x.RequirementPackage.RP_RequirementPackageTypeID == reqPkgTypeId);
        }

        /// <summary>
        /// Get clinical rotation subscriptions by clinical rotation requirement package ID
        /// </summary>
        /// <param name="clinicalRotationReqPackID"></param>
        /// <returns></returns>
        public List<ClinicalRotationSubscription> GetClinicalRotationSubscriptionsByRotationReqPackId(Int32 clinicalRotationReqPackID)
        {
            return _dbContext.ClinicalRotationSubscriptions.Include("RequirementPackageSubscription")
                    .Where(x => x.CRS_ClinicalRotationRequirementPackageID == clinicalRotationReqPackID && !x.CRS_IsDeleted).ToList();
        }

        /// <summary>
        /// Get clinical rotation member by clinical rotation ID
        /// </summary>
        /// <param name="clinicalRotationID"></param>
        /// <returns></returns>
        ClinicalRotationMember IClinicalRotationRepository.GetClinicalRotationMemberByRotationId(Int32 clinicalRotationID)
        {
            return _dbContext.ClinicalRotationMembers.FirstOrDefault(x => x.CRM_ClinicalRotationID == clinicalRotationID && !x.CRM_IsDeleted);
        }

        /// <summary>
        /// Get Clinical Rotation requirement package by ClinicalRotationID
        /// </summary>
        /// <param name="clinicalRotationId"></param>
        /// <returns></returns>
        ClinicalRotationRequirementPackageContract IClinicalRotationRepository.GetRotationRequirementPackage(Int32 clinicalRotationId, String pkgTypeCode)
        {
            ClinicalRotationRequirementPackage clinicalRotationRequirementPackage = _dbContext.ClinicalRotationRequirementPackages
                                                    .FirstOrDefault(x => x.CRRP_ClinicalRotationID == clinicalRotationId
                                                     && !x.CRRP_IsDeleted && x.RequirementPackage.lkpRequirementPackageType.RPT_Code == pkgTypeCode);
            ClinicalRotationRequirementPackageContract rotationRequirementPackageContract = new ClinicalRotationRequirementPackageContract();
            if (clinicalRotationRequirementPackage.IsNotNull())
            {
                rotationRequirementPackageContract.ClinicalRotationRequirementPackageID = clinicalRotationRequirementPackage.CRRP_ID;
                rotationRequirementPackageContract.ClinicalRotationID = clinicalRotationRequirementPackage.CRRP_ClinicalRotationID;
                rotationRequirementPackageContract.RequirementPackageID = clinicalRotationRequirementPackage.CRRP_RequirementPackageID;
                if (clinicalRotationRequirementPackage.RequirementPackage.IsNotNull())
                {
                    rotationRequirementPackageContract.RequirementPackageName = clinicalRotationRequirementPackage.RequirementPackage.RP_PackageName;
                    rotationRequirementPackageContract.RequirementPackageCode = clinicalRotationRequirementPackage.RequirementPackage.RP_Code.Value;
                    rotationRequirementPackageContract.IsCopied = clinicalRotationRequirementPackage.RequirementPackage.RP_IsCopied.HasValue ?
                                                                                clinicalRotationRequirementPackage.RequirementPackage.RP_IsCopied.Value : false;
                    rotationRequirementPackageContract.IsActive = clinicalRotationRequirementPackage.RequirementPackage.RP_IsActive;
                    rotationRequirementPackageContract.RequirementPackageLabel = clinicalRotationRequirementPackage.RequirementPackage.RP_PackageLabel;
                    rotationRequirementPackageContract.IsArchived = clinicalRotationRequirementPackage.RequirementPackage.RP_IsArchived;
                }
            }
            return rotationRequirementPackageContract;
        }

        #region UAT-1843: Phase 2 5: Combining User group mapping, archive and rotation assignment screens

        /// <summary>
        /// Get Clinical Rotation Mapping Data
        /// </summary>
        /// <param name="customPagingArgsContract"></param>
        /// <param name="applicantUserIds"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public List<ClinicalRotationDetailContract> GetClinicalRotationMappingData(CustomPagingArgsContract customPagingArgsContract, String applicantUserIds, Int32? currentUserId)
        {
            try
            {
                List<ClinicalRotationDetailContract> clinicalRotDetailList = new List<ClinicalRotationDetailContract>();

                EntityConnection connection = _dbContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ApplicantOrgUserIDs", applicantUserIds),
                             new SqlParameter("@LoggedInOrgUserID", currentUserId),
                             new SqlParameter("@customFilteringXML", customPagingArgsContract.XML)
                        };

                    SqlCommand command = new SqlCommand("usp_GetRotationDetailMapping", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(sqlParameterCollection);


                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = command;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    int virtualPageCount = 0;
                    int currentPageIndex = 0;

                    if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 1)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            currentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                            virtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                        }

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                ClinicalRotationDetailContract clinicalRotDetail = new ClinicalRotationDetailContract();

                                clinicalRotDetail.RotationID = Convert.ToInt32(dr["RotationID"]);
                                //clinicalRotDetail.AgencyID = Convert.ToInt32(dr["AgencyID"]);
                                clinicalRotDetail.ComplioID = dr["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplioID"]);
                                clinicalRotDetail.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                                clinicalRotDetail.RotationName = dr["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationName"]);
                                clinicalRotDetail.Department = dr["Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Department"]);
                                clinicalRotDetail.Program = dr["Program"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Program"]);
                                clinicalRotDetail.Course = dr["Course"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Course"]);
                                clinicalRotDetail.UnitFloorLoc = dr["UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UnitFloorLoc"]);
                                clinicalRotDetail.Shift = dr["Shift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Shift"]);
                                clinicalRotDetail.Term = dr["Term"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Term"]);
                                clinicalRotDetail.RecommendedHours = dr["RecommendedHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["RecommendedHours"]));
                                clinicalRotDetail.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                                clinicalRotDetail.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                                clinicalRotDetail.StartTime = dr["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["StartTime"]);
                                clinicalRotDetail.EndTime = dr["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["EndTime"]);
                                clinicalRotDetail.Time = dr["Time"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Time"]);
                                clinicalRotDetail.DaysName = dr["DaysName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DaysName"]);
                                clinicalRotDetail.ContactNames = dr["ContactName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContactName"]);
                                clinicalRotDetail.DaysBefore = dr["DaysBefore"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["DaysBefore"]);
                                clinicalRotDetail.Frequency = dr["Frequency"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Frequency"]);
                                clinicalRotDetail.TypeSpecialty = dr["TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TypeSpecialty"]);
                                clinicalRotDetail.Students = dr["Students"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["Students"]));
                                clinicalRotDetail.HierarchyNodes = dr["HierarchyNodes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodes"]);
                                clinicalRotDetail.RotationReviewStatusName = dr["RotationReviewStatusName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationReviewStatusName"]);
                                clinicalRotDetail.IsEditableByClientAdmin = dr["IsEditableByClientAdmin"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsEditableByClientAdmin"]);
                                clinicalRotDetail.TotalRecordCount = virtualPageCount; //dr["TotalCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TotalCount"]);
                                clinicalRotDetail.CreatedDate = dr["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CreatedDate"]); //UAT-3490
                                clinicalRotDetailList.Add(clinicalRotDetail);
                            }
                        }
                    }
                }
                return clinicalRotDetailList;
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
        public Boolean CheckRotationHierarchyNodeSetting(Int32 clinicalRotationID, Int32 orgUserId, out Boolean IsApplicantTakeSpecialPackage)
        {

            Int32 NodeExemptedInRotaionID = GetRotationNodeExemptSetting(clinicalRotationID);
            IsApplicantTakeSpecialPackage = false;
            if (NodeExemptedInRotaionID > AppConsts.NONE && NodeExemptedInRotaionID == 1)
            {
                var deptProgramMapping = _dbContext.ClinicalRotationHierarchyMappings.Where(cond => cond.CRHM_ClinicalRotationID == clinicalRotationID && !cond.CRHM_IsDeleted && !cond.DeptProgramMapping.DPM_IsDeleted).Select(cond => cond.DeptProgramMapping).FirstOrDefault();
                Boolean IsUserExempted = false;
                Boolean result = false;
                List<Int32> nodeIds = GetChildNodeIDsByDPMId(deptProgramMapping.DPM_ID);
                IsUserExempted = _dbContext.BkgExemptedHierarchyNodes.Any(x => nodeIds.Contains(x.EHN_HierarchyNodeID) && x.EHN_OrganizationUserID == orgUserId);

                if (!IsUserExempted)
                {
                    String paidOrderStatus = ApplicantOrderStatus.Paid.GetStringValue();
                    List<BkgOrder> bkgOrders = _dbContext.BkgOrders.Where(cond => !cond.OrganizationUserProfile.IsDeleted
                                        && cond.OrganizationUserProfile.OrganizationUserID == orgUserId && !cond.BOR_IsDeleted
                                        && (cond.BOR_ArchiveStateID == 1 || cond.BOR_ArchiveStateID == null) && (cond.BOR_IsArchived == false)
                                          && cond.Order.OrderPaymentDetails.All(opd => opd.lkpOrderStatu.Code == paidOrderStatus && cond.Order.SelectedNodeID.HasValue && nodeIds.Contains(cond.Order.SelectedNodeID.Value))
                                            ).ToList();

                    if (bkgOrders.IsNotNull() && bkgOrders.Count > AppConsts.NONE)
                    {
                        foreach (BkgOrder item in bkgOrders)
                        {
                            result = item.BkgOrderPackages.Any(x => !x.BkgPackageHierarchyMapping.BPHM_IsDeleted && x.BkgPackageHierarchyMapping.BPHM_IsActive == true && !x.BkgPackageHierarchyMapping.BackgroundPackage.BPA_IsDeleted && x.BkgPackageHierarchyMapping.BackgroundPackage.BPA_IsActive == true && x.BkgPackageHierarchyMapping.BackgroundPackage.BPA_IsReqToQualifyInRotation == true);
                            if (result)
                            {
                                IsApplicantTakeSpecialPackage = true;
                                return false;
                            }
                        }
                        if (!IsApplicantTakeSpecialPackage)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public Int32 GetRotationNodeExemptSetting(Int32 clinicalRotationID)
        {
            Int32 NodeExemptedInRotaionID = AppConsts.NONE;
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetRotationNodeExemptSetting", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClinicalRotationID", clinicalRotationID);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        NodeExemptedInRotaionID = Convert.ToInt32(ds.Tables[0].Rows[0]["NodeExemptedInRotaionID"]);
                    }
                }
            }
            return NodeExemptedInRotaionID;
        }

        public List<Int32> GetChildNodeIDsByDPMId(Int32 dpmId)
        {

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            List<Int32> NodeIds = new List<Int32>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetChildNodeIDsByDPMId", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DPM_ID", dpmId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            NodeIds.Add(Convert.ToInt32(dr["NodeID"]));
                        }
                    }
                }
            }
            return NodeIds;
        }
        /// <summary>
        /// Assign Rotations to applicant users
        /// </summary>
        /// <param name="rotationIds"></param>
        /// <param name="applicantUserIds"></param>
        /// <param name="currentUserId"></param>
        /// <param name="requirementNotCompliantPackStatusID"></param>
        /// <param name="rotationSubscriptionTypeID"></param>
        /// <param name="reqPkgTypeId"></param>
        /// <returns></returns>
        public Boolean AssignRotationsToUsers(List<Int32> rotationIds, List<Int32> applicantUserIds, Int32 currentUserId,
                                                Int32 requirementNotCompliantPackStatusID, Int32 rotationSubscriptionTypeID, Int32 reqPkgTypeId, Int16 statusId, out List<Tuple<Int32, Int32>> applicantList, out Boolean IsApplicantTakeSpecialPackage)
        {
            try
            {
                Boolean flag = false;
                String paidOrderStatus = ApplicantOrderStatus.Paid.GetStringValue();
                applicantList = new List<Tuple<Int32, Int32>>();


                IsApplicantTakeSpecialPackage = false;
                List<Int32> orgUserIDsWithOrder = _dbContext.Orders.Where(cond => !cond.OrganizationUserProfile.IsDeleted
                                    && applicantUserIds.Contains(cond.OrganizationUserProfile.OrganizationUserID) && !cond.IsDeleted
                                        && cond.OrderPaymentDetails.Any(opd => opd.lkpOrderStatu.Code == paidOrderStatus))
                                       .Select(col => col.OrganizationUserProfile.OrganizationUserID).Distinct().ToList();

                var clinicalRotationMembers = _dbContext.ClinicalRotationMembers.Where(x => orgUserIDsWithOrder.Contains(x.CRM_ApplicantOrgUserID)
                                                    && rotationIds.Contains(x.CRM_ClinicalRotationID) && !x.CRM_IsDeleted).ToList();

                List<RequirementPackageSubscription> lstRequirementPackageSubscription = new List<RequirementPackageSubscription>();

                foreach (var rotationId in rotationIds)
                {
                    Dictionary<Int32, Boolean> organizationUserIds = new Dictionary<Int32, Boolean>();
                    foreach (var userId in orgUserIDsWithOrder)
                    {

                        Boolean IsUserAddInRotation = CheckRotationHierarchyNodeSetting(rotationId, userId, out IsApplicantTakeSpecialPackage);

                        if (!IsUserAddInRotation)
                        {
                            if (!clinicalRotationMembers.Any(x => x.CRM_ClinicalRotationID == rotationId && x.CRM_ApplicantOrgUserID == userId))
                            {
                                ClinicalRotationMember clinicalRotationMember = new ClinicalRotationMember();
                                clinicalRotationMember.CRM_ClinicalRotationID = rotationId;
                                clinicalRotationMember.CRM_ApplicantOrgUserID = userId;
                                clinicalRotationMember.CRM_IsDeleted = false;
                                clinicalRotationMember.CRM_CreatedByID = currentUserId;
                                clinicalRotationMember.CRM_CreatedOn = DateTime.Now;
                                _dbContext.ClinicalRotationMembers.AddObject(clinicalRotationMember);
                                organizationUserIds.Add(userId, false);

                                flag = true;
                            }

                        }
                        else
                        {
                            applicantList.Add(new Tuple<int, int>(userId, rotationId));
                        }

                    }
                    if (!organizationUserIds.IsNullOrEmpty())
                    {
                        //Add Requirement Package Subscription for applicants
                        lstRequirementPackageSubscription.AddRange(AddRequirementPackageSubscription(rotationId, requirementNotCompliantPackStatusID, rotationSubscriptionTypeID, organizationUserIds, currentUserId, reqPkgTypeId));
                    }
                }

                if (_dbContext.SaveChanges() > 0)
                {
                    //UAt-2165: Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                    String packageSubscriptionIdsXML = "<PackageSubscriptionIDs>";
                    foreach (RequirementPackageSubscription requirementPackageSubscription in lstRequirementPackageSubscription)
                    {
                        packageSubscriptionIdsXML += "<ID>" + requirementPackageSubscription.RPS_ID + "</ID>";
                    }
                    packageSubscriptionIdsXML += "</PackageSubscriptionIDs>";
                    CreateOptionalCategorySetAproved(packageSubscriptionIdsXML, currentUserId);
                    //UAT-2603
                    List<Int32> lstreqPkgSubscriptionIds = lstRequirementPackageSubscription.Select(sel => sel.RPS_ID).ToList();
                    AddDataToRotDataMovement(lstreqPkgSubscriptionIds, currentUserId, statusId);
                    return true;
                }
                return false;


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

        /// <summary>
        /// Unassign Rotations of applicants
        /// </summary>
        /// <param name="rotationIds"></param>
        /// <param name="applicantUserIds"></param>
        /// <param name="currentUserId"></param>
        /// <param name="reqPkgTypeId"></param>
        /// <returns></returns>
        public List<RotationStudentDropped> UnassignRotations(List<Int32> rotationIds, List<Int32> applicantUserIds, Int32 currentUserId, Int32 reqPkgTypeId, Int32 tenantId, Int16 dataMovementDueStatusId, Int16 dataMovementNotRequiredStatusId)
        {
            try
            {
                List<RotationStudentDropped> lstRotationStudentDropped = new List<RotationStudentDropped>();//UAT-3222

                var clinicalRotationMembers = _dbContext.ClinicalRotationMembers.Where(x => applicantUserIds.Contains(x.CRM_ApplicantOrgUserID)
                                                       && rotationIds.Contains(x.CRM_ClinicalRotationID)
                                                       && !x.CRM_IsDeleted).ToList();


                var clinicalRotations = _dbContext.ClinicalRotations.Where(cond => rotationIds.Contains(cond.CR_ID)
                                                                        && !cond.CR_IsDeleted).ToList();

                List<Int32> lstReqPkgSubsIds = new List<Int32>();
                foreach (var rotationId in rotationIds)
                {
                    //RotationStudentDropped rotationStudentDropped = new RotationStudentDropped();//UAT-3222

                    List<Int32> organizationUserIds = new List<Int32>();
                    List<Int32> CRM_IDs = new List<Int32>();
                    //List<Int32> lstClinicalRotationMemberToSetDropped = new List<Int32>();//UAT-3222

                    var rotation = clinicalRotations.FirstOrDefault(cond => cond.CR_ID == rotationId);
                    bool isLiveRotation = DateTime.Now.Date >= rotation.CR_StartDate && (rotation.CR_EndDate.HasValue && DateTime.Now.Date <= rotation.CR_EndDate.Value);

                    foreach (var clinicalRotationMember in clinicalRotationMembers.Where(x => x.CRM_ClinicalRotationID == rotationId))
                    {
                        bool needToRemoveApplicantFromRotation = false;

                        //if Rotation is approved & applicant is approved 
                        if (isLiveRotation)
                        {
                            bool isApproved = new ProfileSharingRepository().IsApplicantApprovedForRotation(tenantId, rotationId, clinicalRotationMember.CRM_ApplicantOrgUserID);

                            if (isApproved)
                            {
                                CRM_IDs.Add(clinicalRotationMember.CRM_ID);
                                clinicalRotationMember.CRM_IsDropped = true;
                                clinicalRotationMember.CRM_DroppedOn = DateTime.Now;
                            }
                            else
                                needToRemoveApplicantFromRotation = true;
                        }
                        else
                        {
                            needToRemoveApplicantFromRotation = true;
                        }

                        if (needToRemoveApplicantFromRotation)
                        {
                            clinicalRotationMember.CRM_IsDeleted = true;
                            organizationUserIds.Add(clinicalRotationMember.CRM_ApplicantOrgUserID);
                        }

                        clinicalRotationMember.CRM_ModifiedByID = currentUserId;
                        clinicalRotationMember.CRM_ModifiedOn = DateTime.Now;
                    }

                    //Remove Requirement Package Subscription for applicants
                    lstReqPkgSubsIds.AddRange(RemoveRequirementPackageSubscription(rotationId, organizationUserIds, currentUserId, reqPkgTypeId));

                    if (!CRM_IDs.IsNullOrEmpty())
                    {
                        lstRotationStudentDropped.Add(GenerateDataForStudentDroppedFromRotation(rotationId, CRM_IDs, currentUserId, tenantId));//UAt-3222
                        CreateShanpshotForDroppedStudent(rotationId, CRM_IDs, currentUserId, tenantId);
                    }
                }


                if (_dbContext.SaveChanges() > 0)
                {
                    //UAT-3222
                    lstRotationStudentDropped.ForEach(x =>
                    {
                        x.IsRemovedApplicantsFromRotation = true;
                    });

                    //UAT-2603
                    UpdateDataMovementStatus(lstReqPkgSubsIds, dataMovementDueStatusId, dataMovementNotRequiredStatusId, currentUserId);
                    //return true;
                }
                else
                {
                    //UAT-3222
                    lstRotationStudentDropped.ForEach(x =>
                    {
                        x.IsRemovedApplicantsFromRotation = false;
                    });
                }
                return lstRotationStudentDropped;
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

        public void DropRotaionIfRequired(List<Int32> clinicalRotationIds, Int32 currentLoggedInUserId)
        {
            try
            {
                foreach (var clinicalRotationId in clinicalRotationIds)
                {
                    var rotationMembers = _dbContext.ClinicalRotationMembers.Where(x => x.CRM_ClinicalRotationID == clinicalRotationId && !x.CRM_IsDeleted).ToList();
                    if (rotationMembers.Count() > 0 && rotationMembers.Where(x => !x.CRM_IsDropped).Count() == 0)
                    {
                        var clinicalRotation = _dbContext.ClinicalRotations.Where(x => x.CR_ID == clinicalRotationId && !x.CR_IsDeleted).FirstOrDefault();
                        clinicalRotation.CR_IsDropped = true;
                        clinicalRotation.CR_DroppedOn = DateTime.Now;
                        clinicalRotation.CR_ModifiedByID = currentLoggedInUserId;
                        clinicalRotation.CR_ModifiedOn = DateTime.Now;

                        List<SharedUserRotationReview> lstSharedUserRotationReview = _dbContext.SharedUserRotationReviews.Where(cond => !cond.SURR_IsDeleted && cond.SURR_ClinicalRotaionID == clinicalRotationId).ToList();
                        String rotationDroppedReviewCode = SharedUserRotationReviewStatus.Dropped.GetStringValue();
                        Int32 rotationDroppedReviewID = _dbContext.lkpSharedUserRotationReviewStatus.Where(cond => cond.SURRS_Code == rotationDroppedReviewCode && !cond.SURRS_IsDeleted).Select(col => col.SURRS_ID).FirstOrDefault();
                        if (!lstSharedUserRotationReview.IsNullOrEmpty())
                        {
                            foreach (var sharedUserRotationReview in lstSharedUserRotationReview)
                            {
                                sharedUserRotationReview.SURR_RotationReviewStatusID = rotationDroppedReviewID;
                                sharedUserRotationReview.SURR_ReviewByID = currentLoggedInUserId;
                                sharedUserRotationReview.SURR_ModifiedByID = currentLoggedInUserId;
                                sharedUserRotationReview.SURR_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                }
                _dbContext.SaveChanges();
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

        #endregion

        #region Manage Rotations
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        List<ClinicalRotationDetailContract> IClinicalRotationRepository.GetClinicalRotationQueueData(ClinicalRotationDetailContract clinicalRotationDetailContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<ClinicalRotationDetailContract> clinicalRotDetailList = new List<ClinicalRotationDetailContract>();
            String orderBy = "StartDate";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {

                            new SqlParameter("@AgencyIDs", clinicalRotationDetailContract.AgencyIDs),
                            new SqlParameter("@ComplioID", clinicalRotationDetailContract.ComplioID),
                            new SqlParameter("@RotationName", clinicalRotationDetailContract.RotationName),
                            new SqlParameter("@Department", clinicalRotationDetailContract.Department),
                            new SqlParameter("@Program", clinicalRotationDetailContract.Program),
                            new SqlParameter("@Course", clinicalRotationDetailContract.Course),
                            new SqlParameter("@Term", clinicalRotationDetailContract.Term),
                            new SqlParameter("@UnitFloorLoc", clinicalRotationDetailContract.UnitFloorLoc),
                            new SqlParameter("@NoOfHours", clinicalRotationDetailContract.RecommendedHours),
                            //UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
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
                            new SqlParameter("@CustomAtrributesData", clinicalRotationDetailContract.CustomAttributes),
                            new SqlParameter("@ArchieveStatusID", clinicalRotationDetailContract.ArchieveStatusId),
                            new SqlParameter("@pendingReviewStatusIDs",clinicalRotationDetailContract.RotationReviewStatusIdList),
                            new SqlParameter("@DPMIDs",clinicalRotationDetailContract.DPMIds) //UAT-2979: Add the Institution Hierarchy link as a way to filter out existing rotations on the Manage Rotation Search?
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClinicalRotationSearch", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationDetailContract clinicalRotDetail = new ClinicalRotationDetailContract();

                            clinicalRotDetail.RotationID = Convert.ToInt32(dr["RotationID"]);
                            clinicalRotDetail.AgencyID = Convert.ToInt32(dr["AgencyID"]);
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
                            clinicalRotDetail.ContactNames = dr["ContactName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContactName"]);
                            clinicalRotDetail.SyllabusFileName = dr["SyllabusFileName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SyllabusFileName"]);
                            clinicalRotDetail.SyllabusFilePath = dr["SyllabusFilePath"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SyllabusFilePath"]);
                            clinicalRotDetail.TotalRecordCount = dr["TotalCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TotalCount"]);
                            clinicalRotDetail.DaysBefore = dr["DaysBefore"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["DaysBefore"]);
                            clinicalRotDetail.Frequency = dr["Frequency"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Frequency"]);
                            clinicalRotDetail.TypeSpecialty = dr["TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TypeSpecialty"]);
                            clinicalRotDetail.Students = dr["NoOfStudents"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfStudents"]));
                            clinicalRotDetail.HierarchyNodeIDList = dr["HierarchyNodeIDList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodeIDList"]);
                            clinicalRotDetail.HierarchyNodes = dr["HierarchyNodes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodes"]);
                            clinicalRotDetail.CustomAttributes = dr["CustomAttributeList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CustomAttributeList"]);
                            //UAT-2289
                            clinicalRotDetail.DeadlineDate = dr["DeadlineDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DeadlineDate"]);
                            clinicalRotDetail.AgencyHierarchyID = dr["AgencyHierarchyID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyHierarchyID"]);
                            clinicalRotDetail.RootNodeID = dr["RootNodeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RootNodeID"]);
                            clinicalRotDetail.RotationReviewStatusName = dr["RotationReviewName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationReviewName"]);
                            clinicalRotDetail.AgencyIDs = dr["AgencyIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyIds"]);
                            clinicalRotDetail.IsAllowNotification = dr["IsAllowNotification"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsAllowNotification"]);
                            clinicalRotDetail.IsEditableByClientAdmin = dr["IsEditableByClientAdmin"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsEditableByClientAdmin"]);
                            clinicalRotDetail.IsEditableByAgencyUser = dr["IsEditableByAgencyUser"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsEditableByAgencyUser"]);
                            clinicalRotDetail.CreatedDate = dr["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CreatedDate"]); //UAT_3490
                            clinicalRotDetail.CreatedBy = dr["CreatedBy"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CreatedBy"]);
                            clinicalRotDetail.DroppedDate = dr["DroppedDate"] == DBNull.Value || dr["DroppedDate"].IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(dr["DroppedDate"]);
                            clinicalRotDetailList.Add(clinicalRotDetail);
                        }
                    }
                }

                return clinicalRotDetailList;
            }
        }

        RotationsMappedToAgenciesContract IClinicalRotationRepository.GetRotationsMappedToAgencies(String rotationIDs)
        {
            RotationsMappedToAgenciesContract rotationsMappedToAgenciesDetails = new RotationsMappedToAgenciesContract();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {

                            new SqlParameter("@RotationIDs", rotationIDs)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_CheckSelectedRotationAgencies", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            rotationsMappedToAgenciesDetails = new RotationsMappedToAgenciesContract();
                            rotationsMappedToAgenciesDetails.IsRotationAgencyCountMatched = Convert.ToBoolean(dr["IsRotationAgencyCountMatched"]);
                            rotationsMappedToAgenciesDetails.AgencyIDs = Convert.ToString(dr["AgencyIDs"]);
                        }
                    }
                }
            }
            return rotationsMappedToAgenciesDetails;
        }

        //UAT:4395
        Dictionary<Int32, Boolean> IClinicalRotationRepository.GetExistingorganizationUserIds(Int32 ClincalRotationId, Int32? currentLogggedUserId)
        {
            try
            {
                Dictionary<Int32, Boolean> ReturnExistingorganizationUserId = null;
                EntityConnection connection = _dbContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("Usp_GetOrgUsersIdsForClone", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LoggedInOrgUserID", currentLogggedUserId);
                    command.Parameters.AddWithValue("@ClinicalRotationID", ClincalRotationId);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = command;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ReturnExistingorganizationUserId = new Dictionary<int, bool>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ReturnExistingorganizationUserId.Add(Convert.ToInt32(ds.Tables[0].Rows[i]["OrganizationUserId"]), true);
                        }
                    }
                    return ReturnExistingorganizationUserId;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        ///  Method to save clinical rotation
        /// </summary>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Int32 IClinicalRotationRepository.SaveClinicalRotation(ClinicalRotationDetailContract clinicalRotationDetailContract, List<CustomAttribteContract> customAttributeListToSave, Int32 currentUserId, Int32 syllabusDocumentTypeID, Int32 profileSynchSourceTypeID, Int32 additionalDocumentsTypeId = 0)
        {
            ClinicalRotation rotationToAdd = new ClinicalRotation();


            AddRotation(clinicalRotationDetailContract, rotationToAdd, currentUserId);

            #region UAT-2666
            SaveUpdateRotationFieldUpdateByAgency(rotationToAdd, clinicalRotationDetailContract, currentUserId, false);
            #endregion

            AddRotationAgencyMapping(clinicalRotationDetailContract, rotationToAdd, currentUserId);

            AddRotationDays(clinicalRotationDetailContract, rotationToAdd, currentUserId);

            AddRotationContacts(clinicalRotationDetailContract, rotationToAdd, currentUserId);

            AddRotationHierarchy(clinicalRotationDetailContract, rotationToAdd, currentUserId);


            ClientDBContext.ClinicalRotations.AddObject(rotationToAdd);

            if (ClientDBContext.SaveChanges() > 0)
            {
                //if (!clinicalRotationDetailContract.SyllabusFileName.IsNullOrEmpty())
                //{
                AddSyllabusDocument(clinicalRotationDetailContract, currentUserId, rotationToAdd.CR_ID, syllabusDocumentTypeID, additionalDocumentsTypeId);
                //}

                //Update complioId in rotation table.
                ClinicalRotation rotationToUpdate = GetRotationById(rotationToAdd.CR_ID);
                //rotationToUpdate.CR_ComplioID = String.Format("RT-{0}-{1}", clinicalRotationDetailContract.TenantID, rotationToAdd.CR_ID);
                rotationToUpdate.CR_ComplioID = String.Format("CID-{0}-{1}", clinicalRotationDetailContract.TenantID, rotationToAdd.CR_ID); //UAT-3210
                foreach (CustomAttribteContract customAttributeToSave in customAttributeListToSave)
                {
                    AddCustomAttributeMapping(rotationToAdd.CR_ID, customAttributeToSave, currentUserId);
                }
                if (ClientDBContext.SaveChanges() > 0)
                //save data into shared db.
                {
                    //UAT 3961
                    //if (clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID == null || clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID > AppConsts.NONE)
                    //    UpdateClinicalRotationTypeSpecialty(rotationToAdd.CR_ID, clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID, currentUserId);

                    List<Int32> contactsToBeMapped = new List<Int32>();
                    if (!clinicalRotationDetailContract.ContactIdList.IsNullOrEmpty())
                    {
                        contactsToBeMapped = clinicalRotationDetailContract.ContactIdList.Split(',').Select(int.Parse).ToList();
                        foreach (Int32 contact in contactsToBeMapped)
                        {
                            Entity.SharedDataEntity.ClinicalRotationClientContactMapping newContactMapping = new Entity.SharedDataEntity.ClinicalRotationClientContactMapping();
                            newContactMapping.CRCCM_ClientContactID = contact;
                            newContactMapping.CRCCM_TenantID = clinicalRotationDetailContract.TenantID;
                            newContactMapping.CRCCM_ClinicalRotationID = rotationToAdd.CR_ID;
                            newContactMapping.CRCCM_IsDeleted = false;
                            newContactMapping.CRCCM_CreatedByID = currentUserId;
                            newContactMapping.CRCCM_CreatedOn = DateTime.Now;
                            SharedDataDBContext.ClinicalRotationClientContactMappings.AddObject(newContactMapping);
                        }
                        SharedDataDBContext.SaveChanges();
                        //Method to synchronize ClientContact Profiles
                        SynchronizeClientContactProfiles(clinicalRotationDetailContract, currentUserId, profileSynchSourceTypeID, clinicalRotationDetailContract.TenantID); //UAT-1361 - Client COntact Profile Synching
                    }
                    return rotationToAdd.CR_ID;
                }
            }

            return AppConsts.NONE;
        }



        /// <summary>
        /// Method to update clinical rotation
        /// </summary>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        RotationDetailFieldChanges IClinicalRotationRepository.UpdateClinicalRotation(ClinicalRotationDetailContract clinicalRotationDetailContract, List<CustomAttribteContract> customAttributeListToUpdate, Int32 currentUserId, Int32 syllabusDocumentTypeID, Int32 profileSynchSourceTypeID
                                                                                                , Int32 reqPkgTypeId, Int32 rotationSubscriptionTypeID, Int32 requirementNotCompliantPackStatusID, Int16 dataMovementDueStatusId, Int16 dataMovementNotRequiredStatusId, int additionalDocumentTypeId)
        {
            RotationDetailFieldChanges rotationDetailFieldChanges = new RotationDetailFieldChanges();//UAT-3108

            ClinicalRotation rotationToBeUpdated = GetRotationById(clinicalRotationDetailContract.RotationID);
            Boolean isEndDateUpdated = rotationToBeUpdated.CR_EndDate != clinicalRotationDetailContract.EndDate;
            if (rotationToBeUpdated.IsNotNull())
            {
                #region UAT-3108
                rotationDetailFieldChanges = GenerateDataRotationFieldChanges(rotationToBeUpdated, clinicalRotationDetailContract, currentUserId, clinicalRotationDetailContract.TenantID, false, customAttributeListToUpdate, additionalDocumentTypeId);
                #endregion

                //Start UAT-4228
                rotationDetailFieldChanges.IsStartDateUpdated = rotationToBeUpdated.CR_StartDate != clinicalRotationDetailContract.StartDate;
                //END UAT-4228

                UpdateClinicalRotation(clinicalRotationDetailContract, rotationToBeUpdated, currentUserId);

                #region UAT-2666
                SaveUpdateRotationFieldUpdateByAgency(rotationToBeUpdated, clinicalRotationDetailContract, currentUserId, false);
                #endregion

                ClinicalRotationAgency rotationAgencyToBeUpdated = rotationToBeUpdated.ClinicalRotationAgencies.FirstOrDefault();
                rotationAgencyToBeUpdated.CRA_ModifiedByID = currentUserId;
                rotationAgencyToBeUpdated.CRA_ModifiedOn = DateTime.Now;

                AddUpdateRotationDays(clinicalRotationDetailContract, rotationToBeUpdated, currentUserId);
                AddUpdateRotationHierarchy(clinicalRotationDetailContract, rotationToBeUpdated, currentUserId);
                Boolean ifAnySharedCtctDeleted = AddUpdateRotationContacts(clinicalRotationDetailContract, rotationToBeUpdated, currentUserId, dataMovementDueStatusId, dataMovementNotRequiredStatusId);

                AddUpdateRotationCustomAttributes(customAttributeListToUpdate, clinicalRotationDetailContract.RotationID, currentUserId);

                var existingRotationDocumentList = rotationToBeUpdated.ClinicalRotationDocuments.Where(cond => !cond.CRD_IsDeleted);
                if (!existingRotationDocumentList.IsNullOrEmpty() && clinicalRotationDetailContract.IfSyllabusFileRemoved)
                {
                    ClinicalRotationDocument existingClinicalRotationDocument = existingRotationDocumentList.FirstOrDefault(cond => !cond.CRD_IsDeleted);
                    existingClinicalRotationDocument.CRD_IsDeleted = true;
                    existingClinicalRotationDocument.CRD_ModifiedByID = currentUserId;
                    existingClinicalRotationDocument.CRD_ModifiedOn = DateTime.Now;

                    ClientSystemDocument existingClientSystemDocument = existingClinicalRotationDocument.ClientSystemDocument;
                    existingClientSystemDocument.CSD_IsDeleted = true;
                    existingClientSystemDocument.CSD_ModifiedByID = currentUserId;
                    existingClientSystemDocument.CSD_ModifiedOn = DateTime.Now;
                }

                AddSyllabusDocument(clinicalRotationDetailContract, currentUserId, clinicalRotationDetailContract.RotationID, syllabusDocumentTypeID, additionalDocumentTypeId);


                //UAT :3878-Return a rotation back to Pending Review status on the agency side if 1) the school edits the rotation end date AND 2) at least one student's profile has been shared?
                if (isEndDateUpdated)
                {
                    Boolean result = IsAnyMemberInRotationIsApprvdOrNotApprvd(clinicalRotationDetailContract.RotationID, clinicalRotationDetailContract.TenantID); //UAT-4561 --tenantId added for UAT-4743

                    RevertRotationStatusToPending(clinicalRotationDetailContract.RotationID, currentUserId, clinicalRotationDetailContract.TenantID);
                    //Above calling commentd and below calling added in UAT-4561 // Again below is commented as implementation is changed.
                    //Boolean isStatusChangeSuccessfully = RevertRotationStatusToPending(clinicalRotationDetailContract.RotationID, currentUserId, clinicalRotationDetailContract.TenantID);
                    //Start UAT-4561

                    if (result)
                    {
                        rotationDetailFieldChanges.IsNeedToSendEndDateMail = true;
                    }
                    //END UAT-4561
                }
                if (ClientDBContext.SaveChanges() > 0)
                {
                    if (ifAnySharedCtctDeleted)
                        SharedDataDBContext.SaveChanges();

                    if (!clinicalRotationDetailContract.ContactIdList.IsNullOrEmpty())
                    {
                        SynchronizeClientContactProfiles(clinicalRotationDetailContract, currentUserId, profileSynchSourceTypeID, clinicalRotationDetailContract.TenantID); //UAT-1361 - Client COntact Profile Synching 

                    }
                    //for getting list which are newly added in rotation.
                    List<Int32> lstClientContactContractIds = clinicalRotationDetailContract.ContactsToSendEmail.Select(cond => cond.ClientContactID).ToList();
                    if (lstClientContactContractIds.Count > AppConsts.NONE)
                    {
                        List<RequirementPackageSubscription> lstRequirementPackageSubscriptionToBeAdded = CreateRotationSubscriptionForClientContact(lstClientContactContractIds, clinicalRotationDetailContract.RotationID, reqPkgTypeId, rotationSubscriptionTypeID, requirementNotCompliantPackStatusID, currentUserId);
                        if (ClientDBContext.SaveChanges() > AppConsts.NONE)
                        {
                            String packageSubscriptionIdsXML = "<PackageSubscriptionIDs>";
                            foreach (RequirementPackageSubscription requirementPackageSubscription in lstRequirementPackageSubscriptionToBeAdded)
                            {
                                packageSubscriptionIdsXML += "<ID>" + requirementPackageSubscription.RPS_ID + "</ID>";
                            }
                            packageSubscriptionIdsXML += "</PackageSubscriptionIDs>";
                            CreateOptionalCategorySetAproved(packageSubscriptionIdsXML, currentUserId);
                            //UAT-2603
                            List<Int32> lstreqPkgSubscriptionIds = lstRequirementPackageSubscriptionToBeAdded.Select(sel => sel.RPS_ID).ToList();
                            AddDataToRotDataMovement(lstreqPkgSubscriptionIds, currentUserId, dataMovementDueStatusId);
                        }
                    }
                    //return true;
                    rotationDetailFieldChanges.IsClinicalRotationUpdatedSuccessfully = true;
                }
                else
                {
                    rotationDetailFieldChanges.IsClinicalRotationUpdatedSuccessfully = false;
                }
                //UAT 3961
                //    if (clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID == null || clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID > AppConsts.NONE)
                //        UpdateClinicalRotationTypeSpecialty(rotationToBeUpdated.CR_ID, clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID, currentUserId);
            }
            //return false;
            return rotationDetailFieldChanges;

        }

        //public void RevertRotationStatusToPending(Int32 rotationId, Int32 userId, Int32 tenantID) 
        //Changed the return type of method in UAT-4651
        public Boolean RevertRotationStatusToPending(Int32 rotationId, Int32 userId, Int32 tenantID, Int32? agencyID = 0)
        {
            Boolean isStatusUpdated = false;
            Int32 pendingReviewStatusID = _dbContext.lkpSharedUserRotationReviewStatus.FirstOrDefault(s => s.SURRS_Code == "AAAA" && !s.SURRS_IsDeleted).SURRS_ID;
            Int32 invitationReviewStatusID = SharedDataDBContext.lkpSharedUserInvitationReviewStatus.FirstOrDefault(s => s.SUIRS_Code == "AAAA" && !s.SUIRS_IsDeleted).SUIRS_ID;

            //UAT-4673
            // if (_dbContext.SharedUserRotationReviews.Where(a => a.SURR_ClinicalRotaionID == rotationId && !a.SURR_IsDeleted && a.SURR_RotationReviewStatusID != pendingReviewStatusID).Any())
            // {
            //End UAT-4673

            var ProfileSharingInvitationGroupsList = SharedDataDBContext.ProfileSharingInvitationGroups.Where(cnd => cnd.PSIG_ClinicalRotationID == rotationId
                                                    && cnd.PSIG_TenantID == tenantID && !cnd.PSIG_IsDeleted).ToList();

            List<Int32> lstClinicalRotationDroppedMembers = _dbContext.ClinicalRotationMembers.Where(x => x.CRM_ClinicalRotationID == rotationId && !x.CRM_IsDeleted
                                                                      && x.CRM_IsDropped).Select(cond => cond.CRM_ApplicantOrgUserID)
                                                                                           .ToList();

            var invitationIdsTemp = ProfileSharingInvitationGroupsList.Select(slct => slct.ProfileSharingInvitations.Where(cond =>
                                                                                 !cond.PSI_IsDeleted

                                                                              && cond.lkpInvitationStatu.Code != "AAAG"
                                                                              && cond.lkpInvitationStatu.Code != "AAAD"
                                                                              && cond.lkpInvitationStatu.Code != "AAAE"
                                                                              && !lstClinicalRotationDroppedMembers.Contains(cond.PSI_ApplicantOrgUserID)).Select(x => x.PSI_ID)).ToList();
            List<Int32> lstinvitationIds = new List<Int32>();

            invitationIdsTemp.ForEach(x =>
            {
                lstinvitationIds.AddRange(x);
            });
            //UAT-4673
            if (!lstinvitationIds.IsNullOrEmpty() && lstinvitationIds.Count > AppConsts.NONE)
            {
                if (SharedDataDBContext.SharedUserInvitationReviews.Where(con => !con.SUIR_IsDeleted && lstinvitationIds.Contains(con.SUIR_ProfileSharingInvitationID)
                                             && con.SUIR_ReviewStatusID != invitationReviewStatusID).Any())
                {
                    var invitationIds = string.Join(",", lstinvitationIds);
                    //End UAT-4673
                    EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
                    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                    {
                        SqlCommand command = new SqlCommand("usp_UpdateUserRotationReviewStatus", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@InvitationIDs", invitationIds);
                        command.Parameters.AddWithValue("@ClinicalRotationID", rotationId);
                        command.Parameters.AddWithValue("@InvitationReviewStatusID", invitationReviewStatusID);
                        command.Parameters.AddWithValue("@RotationReviewStatusID", pendingReviewStatusID);
                        command.Parameters.AddWithValue("@ModifiedByID", userId);
                        command.Parameters.AddWithValue("@tenantID", tenantID);
                        command.Parameters.AddWithValue("@agencyID", agencyID);
                        command.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        command.ExecuteNonQuery();
                        con.Close();

                        // Added in UAT-4561
                        //if (isRotPrevStatusApproved)
                        isStatusUpdated = true;
                    }
                }
            }

            //UAT-4561
            return isStatusUpdated;
            //UAT-4673
            //Int32 approvedRotationStatusID = _dbContext.lkpSharedUserRotationReviewStatus.Where(s => s.SURRS_Code == "AAAB" && !s.SURRS_IsDeleted).FirstOrDefault().SURRS_ID; // Added in UAT-4561
            //Int32 approvedInvitationStatusID = SharedDataDBContext.lkpSharedUserInvitationReviewStatus.Where(con => con.SUIRS_Code == "AAAB" && !con.SUIRS_IsDeleted).FirstOrDefault().SUIRS_ID;
            //Boolean isRotPrevStatusApproved = SharedDataDBContext.SharedUserInvitationReviews
            //                            .Where(con => !con.SUIR_IsDeleted && lstinvitationIds.Contains(con.SUIR_ProfileSharingInvitationID))
            //                            .All(x => x.SUIR_ReviewStatusID == approvedInvitationStatusID)

            //             && _dbContext.SharedUserRotationReviews.Where(a => a.SURR_ClinicalRotaionID == rotationId && !a.SURR_IsDeleted
            //                                                        && (a.SURR_OrganizationUserID.HasValue ? !lstClinicalRotationDroppedMembers.Contains(a.SURR_OrganizationUserID.Value)
            //                                                        : true)).All(x => x.SURR_RotationReviewStatusID == approvedRotationStatusID);

            //Boolean isRotPrevStatusApproved = SharedDataDBContext.SharedUserInvitationReviews
            //                            .Where(con => !con.SUIR_IsDeleted && lstinvitationIds.Contains(con.SUIR_ProfileSharingInvitationID))
            //                            .All(x => x.SUIR_ReviewStatusID == approvedInvitationStatusID);

            //Boolean isRotPrevStatusApproved = _dbContext.SharedUserRotationReviews.Where(a => a.SURR_ClinicalRotaionID == rotationId && !a.SURR_IsDeleted).All(x => x.SURR_RotationReviewStatusID == approvedRotationStatusID);
            //}
            //End UAT-4673
        }

        /// <summary>
        /// Method to delete clinical rotation
        /// </summary>
        /// <returns></returns>
        Boolean IClinicalRotationRepository.DeleteClinicalRotation(Int32 clinicalRotationId, Int32 currentUserId)
        {
            ClinicalRotation rotationToBeDeleted = GetRotationById(clinicalRotationId);
            if (rotationToBeDeleted.IsNotNull())
            {
                rotationToBeDeleted.CR_IsDeleted = true;
                rotationToBeDeleted.CR_ModifiedOn = DateTime.Now;
                rotationToBeDeleted.CR_ModifiedByID = currentUserId;

                ClinicalRotationAgency rotationAgencyToBeDeleted = rotationToBeDeleted.ClinicalRotationAgencies.FirstOrDefault();
                rotationAgencyToBeDeleted.CRA_IsDeleted = true;
                rotationAgencyToBeDeleted.CRA_ModifiedByID = currentUserId;
                rotationAgencyToBeDeleted.CRA_ModifiedOn = DateTime.Now;

                List<ClinicalRotationDay> daysListToBeDeleted = rotationToBeDeleted.ClinicalRotationDays.Where(cond => !cond.CRD_IsDeleted).ToList();
                foreach (ClinicalRotationDay dayToBeDeleted in daysListToBeDeleted)
                {
                    dayToBeDeleted.CRD_IsDeleted = true;
                    dayToBeDeleted.CRD_ModifiedByID = currentUserId;
                    dayToBeDeleted.CRD_ModifiedOn = DateTime.Now;
                }

                List<ClinicalRotationClientContact> contactListToBeDeleted = rotationToBeDeleted.ClinicalRotationClientContacts.Where(cond => !cond.CRCC_IsDeleted).ToList();
                //delete existing contact list
                foreach (ClinicalRotationClientContact contactToBeDeleted in contactListToBeDeleted)
                {
                    contactToBeDeleted.CRCC_IsDeleted = true;
                    contactToBeDeleted.CRCC_ModifiedByID = currentUserId;
                    contactToBeDeleted.CRCC_ModifiedOn = DateTime.Now;

                }

                //delete CustomAttributeMapping list
                List<CustomAttribute> attributeListToUpdate = GetCustomAttributeListByUseType(CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue());
                if (!attributeListToUpdate.IsNullOrEmpty())
                {
                    List<Int32> attributeIdsToUpdate = attributeListToUpdate.Select(cond => cond.CA_CustomAttributeID).ToList();
                    List<CustomAttributeMapping> existingCustomAttributeMappingInDb = GetCustomAttributeMappings(clinicalRotationId, attributeIdsToUpdate);
                    foreach (CustomAttributeMapping customAttributeMappingToUpdate in existingCustomAttributeMappingInDb)
                    {
                        customAttributeMappingToUpdate.CAM_IsDeleted = true;
                        customAttributeMappingToUpdate.CAM_ModifiedByID = currentUserId;
                        customAttributeMappingToUpdate.CAM_ModifiedOn = DateTime.Now;
                        CustomAttributeValue customAttributeValueToUpdate = customAttributeMappingToUpdate.CustomAttributeValues.FirstOrDefault(cond => !cond.CAV_IsDeleted);
                        customAttributeValueToUpdate.CAV_IsDeleted = true;
                        customAttributeValueToUpdate.CAV_ModifiedByID = currentUserId;
                        customAttributeValueToUpdate.CAV_ModifiedOn = DateTime.Now;
                    }
                }
                if (ClientDBContext.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }


        #region UAT-2545
        /// <summary>
        /// Method to delete clinical rotation
        /// </summary>
        /// <returns></returns>
        Boolean IClinicalRotationRepository.ArchiveClinicalRotation(List<Int32> clinicalRotationIds, Int32 currentUserId)
        {
            var rotationList = ClientDBContext.ClinicalRotations.Where(cond => clinicalRotationIds.Contains(cond.CR_ID)).ToList();
            rotationList.ForEach(update => update.CR_IsArchived = true);

            if (ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region UAT-3138
        Boolean IClinicalRotationRepository.UnArchiveClinicalRotation(List<Int32> clinicalRotationIds, Int32 currentUserId)
        {
            var rotationList = ClientDBContext.ClinicalRotations.Where(cond => clinicalRotationIds.Contains(cond.CR_ID) && !cond.CR_IsDeleted).ToList();
            rotationList.ForEach(update => update.CR_IsArchived = false);

            if (ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }

            return false;
        }
        #endregion

        List<CustomAttribteContract> IClinicalRotationRepository.GetCustomAttributeMappingList(String useTypeCode, Int32? rotationID)
        {
            List<CustomAttribute> customAttributeList = GetCustomAttributeListByUseType(useTypeCode);

            List<CustomAttribteContract> customAttributeMappingList = new List<CustomAttribteContract>();
            List<CustomAttributeMapping> existingCustomAttributeMappingList = new List<CustomAttributeMapping>();
            if (!rotationID.IsNullOrEmpty())
            {
                List<Int32> attrIds = customAttributeList.Select(sel => sel.CA_CustomAttributeID).ToList();
                existingCustomAttributeMappingList = GetCustomAttributeMappings(rotationID.Value, attrIds);
            }

            foreach (CustomAttribute customAttribute in customAttributeList)
            {
                CustomAttribteContract customAttribteContract = new CustomAttribteContract();
                customAttribteContract.CustomAttributeId = customAttribute.CA_CustomAttributeID;
                customAttribteContract.CustomAttributeName = customAttribute.CA_AttributeName;
                customAttribteContract.CustomAttributeLabel = customAttribute.CA_AttributeLabel;
                customAttribteContract.CustomAttributeDataTypeCode = customAttribute.lkpCustomAttributeDataType.Code;
                customAttribteContract.CustomAttributeIsRequired = customAttribute.CA_IsRequired;
                customAttribteContract.MaxLength = customAttribute.CA_StringLength;
                CustomAttributeMapping existingCustomAttributeMapping = existingCustomAttributeMappingList.FirstOrDefault(
                    cond => cond.CAM_CustomAttributeID == customAttribute.CA_CustomAttributeID);
                if (!existingCustomAttributeMapping.IsNullOrEmpty())
                {
                    customAttribteContract.CustomAttrMappingId = existingCustomAttributeMapping.CAM_CustomAttributeMappingID;
                    customAttribteContract.CustomAttributeValue = existingCustomAttributeMapping.CustomAttributeValues.FirstOrDefault(cond => !cond.CAV_IsDeleted).CAV_AttributeValue;
                }
                customAttributeMappingList.Add(customAttribteContract);
            }
            return customAttributeMappingList;
        }

        /// <summary>
        /// Get the ApplicantId's in the current Rotation
        /// </summary>
        /// <param name="rotationId"></param>
        /// <returns></returns>
        List<Int32> IClinicalRotationRepository.GetRotationApplicantIds(Int32 rotationId)
        {
            return _dbContext.ClinicalRotationMembers.Where(crm => crm.CRM_ClinicalRotationID == rotationId && !crm.CRM_IsDeleted).Select(crm => crm.CRM_ApplicantOrgUserID).ToList();
        }

        /// <summary>
        /// Method to Get Instructor/Preceptor Data, including the backgrond and compliance shared info type codes.
        /// </summary>
        /// <param name="rotationId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        List<ClientContactProfileSharingData> IClinicalRotationRepository.GetRotationClientContacts(Int32 rotationId, Int32 tenantId)
        {
            return _dbContext.GetClientContactData(tenantId, rotationId).ToList();
        }

        #endregion

        #region Private Methods

        private ClinicalRotation GetRotationById(Int32 clinicalRotationId)
        {
            return ClientDBContext.ClinicalRotations.FirstOrDefault(cond => cond.CR_ID == clinicalRotationId);
        }

        private List<CustomAttributeMapping> GetCustomAttributeMappings(Int32 rotationID, List<Int32> attrIdsList)
        {
            List<CustomAttributeMapping> existingCustomAttributeMappingInDb = new List<CustomAttributeMapping>();
            if (!attrIdsList.IsNullOrEmpty())
            {
                existingCustomAttributeMappingInDb = ClientDBContext.CustomAttributeMappings.Include("CustomAttributeValues")
                                                                                              .Where(con => con.CAM_RecordID == rotationID
                                                                                                && attrIdsList.Contains(con.CAM_CustomAttributeID)
                                                                                                && !con.CAM_IsDeleted).ToList();
            }
            return existingCustomAttributeMappingInDb;
        }

        /// <summary>
        /// Get mapping list of custom attribute with node 
        /// </summary>
        /// <param name="customAttributeNodeId">customAttributeNodeId</param>
        /// <returns>IQueryable</returns>
        public List<CustomAttribute> GetCustomAttributeListByUseType(String attributeUseTypeCode)
        {
            return _dbContext.CustomAttributes.Where(cond => !cond.CA_IsDeleted && cond.CA_IsActive
                                                                && cond.lkpCustomAttributeUseType.Code == attributeUseTypeCode)
                                                                                                .ToList();
        }

        private static void AddRotation(ClinicalRotationDetailContract clinicalRotationDetailContract, ClinicalRotation rotationToAdd, Int32 currentUserId)
        {
            rotationToAdd.CR_Department = clinicalRotationDetailContract.Department;
            rotationToAdd.CR_Program = clinicalRotationDetailContract.Program;
            rotationToAdd.CR_Course = clinicalRotationDetailContract.Course;
            rotationToAdd.CR_UnitFloorLoc = clinicalRotationDetailContract.UnitFloorLoc;
            // UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
            rotationToAdd.CR_NoOfStudents = clinicalRotationDetailContract.Students;
            rotationToAdd.CR_NoOfHours = clinicalRotationDetailContract.RecommendedHours;
            rotationToAdd.CR_RotationShift = clinicalRotationDetailContract.Shift;
            //UAT 1355 Addition of Term field to the Create rotation, rotation details screens
            rotationToAdd.CR_Term = clinicalRotationDetailContract.Term;
            rotationToAdd.CR_RotationName = clinicalRotationDetailContract.RotationName;
            rotationToAdd.CR_StartTime = clinicalRotationDetailContract.StartTime;
            rotationToAdd.CR_EndTime = clinicalRotationDetailContract.EndTime;
            rotationToAdd.CR_StartDate = clinicalRotationDetailContract.StartDate;
            rotationToAdd.CR_EndDate = clinicalRotationDetailContract.EndDate;
            rotationToAdd.CR_IsDeleted = false;
            rotationToAdd.CR_CreatedOn = DateTime.Now;
            rotationToAdd.CR_CreatedByID = currentUserId;
            //UAT-1414 notification to go out prior to student's start date for clinical rotation
            rotationToAdd.CR_DaysBefore = clinicalRotationDetailContract.DaysBefore;
            rotationToAdd.CR_Frequency = clinicalRotationDetailContract.Frequency;
            //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
            rotationToAdd.CR_TypeSpecialty = clinicalRotationDetailContract.TypeSpecialty;
            //UAT-2289 : Rotation - New Field - Compliance Deadline date
            rotationToAdd.CR_DeadlineDate = clinicalRotationDetailContract.DeadlineDate;
            //UAT-2905: Notification email when applicant has submitted an item into requirements verification queue.
            rotationToAdd.CR_IsAllowNotification = clinicalRotationDetailContract.IsAllowNotification;

            //UAT-3041: New flag/setting for if rotation details should be editable by client admins/agency users or not
            rotationToAdd.CR_IsEditableByAgencyUser = clinicalRotationDetailContract.IsEditableByAgencyUser;
            rotationToAdd.CR_IsEditableByClientAdmin = clinicalRotationDetailContract.IsEditableByClientAdmin;
            //UAT 3961
            rotationToAdd.CR_TypeSpecialtyID = clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID;
            //UAT-4150
            rotationToAdd.CR_IsSchoolSendingInstructor = clinicalRotationDetailContract.IsSchoolSendingInstructor;
            //UAT - 4466
            if (clinicalRotationDetailContract.CloneRotationId > 0)
                rotationToAdd.CR_CloneRotationID = clinicalRotationDetailContract.CloneRotationId;

        }

        private static void AddRotationAgencyMapping(ClinicalRotationDetailContract clinicalRotationDetailContract, ClinicalRotation rotationToAdd, Int32 currentUserId)
        {
            //ClinicalRotationAgency newRotationAgency = new ClinicalRotationAgency();
            //newRotationAgency.CRA_AgencyID = clinicalRotationDetailContract.AgencyID;
            //newRotationAgency.CRA_IsDeleted = false;
            //newRotationAgency.CRA_CreatedByID = currentUserId;
            //newRotationAgency.CRA_CreatedOn = DateTime.Now;
            //rotationToAdd.ClinicalRotationAgencies.Add(newRotationAgency);

            ClinicalRotationAgency newRotationAgency = new ClinicalRotationAgency();
            String[] agencyIDs = clinicalRotationDetailContract.AgencyIdList.Split(',');
            foreach (string agencyid in agencyIDs)
            {
                newRotationAgency = new ClinicalRotationAgency();
                newRotationAgency.CRA_AgencyID = Convert.ToInt32(agencyid);
                newRotationAgency.CRA_IsDeleted = false;
                newRotationAgency.CRA_CreatedByID = currentUserId;
                newRotationAgency.CRA_CreatedOn = DateTime.Now;
                rotationToAdd.ClinicalRotationAgencies.Add(newRotationAgency);
            }
        }

        private static void AddRotationContacts(ClinicalRotationDetailContract clinicalRotationDetailContract, ClinicalRotation rotationToAdd, Int32 currentUserId)
        {
            List<Int32> contactsToBeMapped = new List<Int32>();
            if (!clinicalRotationDetailContract.ContactIdList.IsNullOrEmpty())
            {
                contactsToBeMapped = clinicalRotationDetailContract.ContactIdList.Split(',').Select(int.Parse).ToList();
                foreach (Int32 contact in contactsToBeMapped)
                {
                    ClinicalRotationClientContact newContact = new ClinicalRotationClientContact();
                    newContact.CRCC_ClientContactID = contact;
                    newContact.CRCC_IsDeleted = false;
                    newContact.CRCC_CreatedByID = currentUserId;
                    newContact.CRCC_CreatedOn = DateTime.Now;
                    rotationToAdd.ClinicalRotationClientContacts.Add(newContact);
                }
            }
        }

        private static void AddRotationDays(ClinicalRotationDetailContract clinicalRotationDetailContract, ClinicalRotation rotationToAdd, Int32 currentUserId)
        {
            List<Int32> daysToBeMapped = new List<Int32>();
            if (!clinicalRotationDetailContract.DaysIdList.IsNullOrEmpty())
            {
                daysToBeMapped = clinicalRotationDetailContract.DaysIdList.Split(',').Select(int.Parse).ToList();
                foreach (Int32 day in daysToBeMapped)
                {
                    ClinicalRotationDay newDay = new ClinicalRotationDay();
                    newDay.CRD_WeekDayID = day;
                    newDay.CRD_IsDeleted = false;
                    newDay.CRD_CreatedByID = currentUserId;
                    newDay.CRD_CreatedOn = DateTime.Now;
                    rotationToAdd.ClinicalRotationDays.Add(newDay);
                }
            }
        }

        private static void AddRotationHierarchy(ClinicalRotationDetailContract clinicalRotationDetailContract, ClinicalRotation rotationToAdd, Int32 currentUserId)
        {
            List<Int32> nodesToBeMapped = new List<Int32>();
            if (!clinicalRotationDetailContract.HierarchyNodeIDList.IsNullOrEmpty())
            {
                nodesToBeMapped = clinicalRotationDetailContract.HierarchyNodeIDList.Split(',').Select(int.Parse).ToList();
                foreach (Int32 node in nodesToBeMapped)
                {
                    ClinicalRotationHierarchyMapping newClinicalRotationHierarchyMapping = new ClinicalRotationHierarchyMapping();
                    newClinicalRotationHierarchyMapping.CRHM_HierarchyNodeID = node;
                    newClinicalRotationHierarchyMapping.CRHM_IsDeleted = false;
                    newClinicalRotationHierarchyMapping.CRHM_CreatedBy = currentUserId;
                    newClinicalRotationHierarchyMapping.CRHM_CreatedOn = DateTime.Now;
                    rotationToAdd.ClinicalRotationHierarchyMappings.Add(newClinicalRotationHierarchyMapping);
                }
            }
        }

        private void AddCustomAttributeMapping(Int32 rotationTID, CustomAttribteContract customAttributeToSave, Int32 currentUserId)
        {
            CustomAttributeMapping customAttributeMappingToAdd = new CustomAttributeMapping();
            customAttributeMappingToAdd.CAM_CustomAttributeID = customAttributeToSave.CustomAttributeId;
            customAttributeMappingToAdd.CAM_RecordID = rotationTID;
            customAttributeMappingToAdd.CAM_IsRequired = customAttributeToSave.CustomAttributeIsRequired;
            customAttributeMappingToAdd.CAM_IsDeleted = false;
            customAttributeMappingToAdd.CAM_CreatedByID = currentUserId;
            customAttributeMappingToAdd.CAM_CreatedOn = DateTime.Now;

            CustomAttributeValue customAttributeValueToAdd = new CustomAttributeValue();
            customAttributeValueToAdd.CAV_RecordID = rotationTID;
            customAttributeValueToAdd.CAV_AttributeValue = customAttributeToSave.CustomAttributeValue;
            customAttributeValueToAdd.CAV_IsDeleted = false;
            customAttributeValueToAdd.CAV_CreatedByID = currentUserId;
            customAttributeValueToAdd.CAV_CreatedOn = DateTime.Now;
            customAttributeMappingToAdd.CustomAttributeValues.Add(customAttributeValueToAdd);
            ClientDBContext.CustomAttributeMappings.AddObject(customAttributeMappingToAdd);
        }

        private static void UpdateClinicalRotation(ClinicalRotationDetailContract clinicalRotationDetailContract, ClinicalRotation rotationToBeUpdated, Int32 currentUserId)
        {
            rotationToBeUpdated.CR_RotationName = clinicalRotationDetailContract.RotationName;
            rotationToBeUpdated.CR_Department = clinicalRotationDetailContract.Department;
            rotationToBeUpdated.CR_Program = clinicalRotationDetailContract.Program;
            rotationToBeUpdated.CR_Course = clinicalRotationDetailContract.Course;
            rotationToBeUpdated.CR_UnitFloorLoc = clinicalRotationDetailContract.UnitFloorLoc;
            //UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
            rotationToBeUpdated.CR_NoOfStudents = clinicalRotationDetailContract.Students;
            rotationToBeUpdated.CR_NoOfHours = clinicalRotationDetailContract.RecommendedHours;
            rotationToBeUpdated.CR_RotationShift = clinicalRotationDetailContract.Shift;
            //UAT 1355 Addition of Term field to the Create rotation, rotation details screens
            rotationToBeUpdated.CR_Term = clinicalRotationDetailContract.Term;
            rotationToBeUpdated.CR_StartTime = clinicalRotationDetailContract.StartTime;
            rotationToBeUpdated.CR_EndTime = clinicalRotationDetailContract.EndTime;
            rotationToBeUpdated.CR_StartDate = clinicalRotationDetailContract.StartDate;
            rotationToBeUpdated.CR_EndDate = clinicalRotationDetailContract.EndDate;
            rotationToBeUpdated.CR_IsDeleted = false;
            rotationToBeUpdated.CR_ModifiedOn = DateTime.Now;
            rotationToBeUpdated.CR_ModifiedByID = currentUserId;
            rotationToBeUpdated.CR_Frequency = clinicalRotationDetailContract.Frequency;
            rotationToBeUpdated.CR_DaysBefore = clinicalRotationDetailContract.DaysBefore;
            //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
            rotationToBeUpdated.CR_TypeSpecialty = clinicalRotationDetailContract.TypeSpecialty;
            //UAT-2289 : Rotation - New Field - Compliance Deadline date
            rotationToBeUpdated.CR_DeadlineDate = clinicalRotationDetailContract.DeadlineDate;
            //UAT-2905: Notification email when applicant has submitted an item into requirements verification queue.
            rotationToBeUpdated.CR_IsAllowNotification = clinicalRotationDetailContract.IsAllowNotification;

            //UAT-3041: New flag/setting for if rotation details should be editable by client admins/agency users or not
            rotationToBeUpdated.CR_IsEditableByAgencyUser = clinicalRotationDetailContract.IsEditableByAgencyUser;
            rotationToBeUpdated.CR_IsEditableByClientAdmin = clinicalRotationDetailContract.IsEditableByClientAdmin;
            //UAT 3961
            rotationToBeUpdated.CR_TypeSpecialtyID = clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID;
            //UAT-4150
            rotationToBeUpdated.CR_IsSchoolSendingInstructor = clinicalRotationDetailContract.IsSchoolSendingInstructor;
        }

        private static void AddUpdateRotationDays(ClinicalRotationDetailContract clinicalRotationDetailContract, ClinicalRotation rotationToBeUpdated, Int32 currentUserId)
        {
            List<ClinicalRotationDay> existingMappedDays = rotationToBeUpdated.ClinicalRotationDays.Where(cond => !cond.CRD_IsDeleted).ToList();
            List<Int32> daysToBeMapped = new List<Int32>();

            if (!clinicalRotationDetailContract.DaysIdList.IsNullOrEmpty())
                daysToBeMapped = clinicalRotationDetailContract.DaysIdList.Split(',').Select(int.Parse).ToList();

            //Check whether existing MappedDays exist in current list if not exist then delete it
            foreach (ClinicalRotationDay existingDay in existingMappedDays)
            {
                if (!(daysToBeMapped.Contains(existingDay.CRD_WeekDayID.Value)))
                {
                    existingDay.CRD_IsDeleted = true;
                    existingDay.CRD_ModifiedByID = currentUserId;
                    existingDay.CRD_ModifiedOn = DateTime.Now;
                }
            }

            //Check whether selected day exist in db if not exist then insert it
            foreach (Int32 day in daysToBeMapped)
            {
                if (!(existingMappedDays.Any(obj => obj.CRD_WeekDayID == day && obj.CRD_IsDeleted == false)))
                {
                    ClinicalRotationDay newDay = new ClinicalRotationDay();
                    newDay.CRD_WeekDayID = day;
                    newDay.CRD_IsDeleted = false;
                    newDay.CRD_CreatedByID = currentUserId;
                    newDay.CRD_CreatedOn = DateTime.Now;
                    rotationToBeUpdated.ClinicalRotationDays.Add(newDay);
                }
            }
        }

        private static void AddUpdateRotationHierarchy(ClinicalRotationDetailContract clinicalRotationDetailContract, ClinicalRotation rotationToBeUpdated, Int32 currentUserId)
        {
            List<ClinicalRotationHierarchyMapping> existingClinicalRotationHierarchyMappings = rotationToBeUpdated.ClinicalRotationHierarchyMappings.Where(cond => !cond.CRHM_IsDeleted).ToList();
            List<Int32> nodesToBeMapped = new List<Int32>();

            if (!clinicalRotationDetailContract.HierarchyNodeIDList.IsNullOrEmpty())
                nodesToBeMapped = clinicalRotationDetailContract.HierarchyNodeIDList.Split(',').Select(int.Parse).ToList();

            //Check whether existing node exist in current list if not exist then delete it
            foreach (ClinicalRotationHierarchyMapping existingNode in existingClinicalRotationHierarchyMappings)
            {
                if (!(nodesToBeMapped.Contains(existingNode.CRHM_HierarchyNodeID)))
                {
                    existingNode.CRHM_IsDeleted = true;
                    existingNode.CRHM_ModifiedBy = currentUserId;
                    existingNode.CRHM_ModifiedOn = DateTime.Now;
                }
            }

            //Check whether selected node exist in db if not exist then insert it
            foreach (Int32 node in nodesToBeMapped)
            {
                if (!(existingClinicalRotationHierarchyMappings.Any(obj => obj.CRHM_HierarchyNodeID == node && obj.CRHM_IsDeleted == false)))
                {
                    ClinicalRotationHierarchyMapping newClinicalRotationHierarchyMapping = new ClinicalRotationHierarchyMapping();
                    newClinicalRotationHierarchyMapping.CRHM_HierarchyNodeID = node;
                    newClinicalRotationHierarchyMapping.CRHM_IsDeleted = false;
                    newClinicalRotationHierarchyMapping.CRHM_CreatedBy = currentUserId;
                    newClinicalRotationHierarchyMapping.CRHM_CreatedOn = DateTime.Now;
                    rotationToBeUpdated.ClinicalRotationHierarchyMappings.Add(newClinicalRotationHierarchyMapping);
                }
            }
        }

        private Boolean AddUpdateRotationContacts(ClinicalRotationDetailContract clinicalRotationDetailContract, ClinicalRotation rotationToBeUpdated, Int32 currentUserId, Int16 dataMovementDueStatusId, Int16 dataMovementNotRequiredStatusId)
        {
            List<ClinicalRotationClientContact> existingContactList = rotationToBeUpdated.ClinicalRotationClientContacts.Where(cond => !cond.CRCC_IsDeleted).ToList();
            Boolean ifAnySharedCtctUpdated = false;
            List<Int32> contactsToBeMapped = new List<Int32>();
            List<Int32> contactsToBeAdded = new List<Int32>();
            List<Int32> contactsToBeDeleted = new List<Int32>();
            List<Int32> aleadyMappedContacts = existingContactList.Select(sel => sel.CRCC_ClientContactID.Value).ToList();
            if (!clinicalRotationDetailContract.ContactIdList.IsNullOrEmpty())
            {
                contactsToBeMapped = clinicalRotationDetailContract.ContactIdList.Split(',').Select(int.Parse).ToList();
                contactsToBeAdded = contactsToBeMapped.Except(aleadyMappedContacts).ToList();
            }

            //Check whether existing contact exist in current list if not exist then delete it
            foreach (ClinicalRotationClientContact existingContact in existingContactList)
            {
                if (!(contactsToBeMapped.Contains(existingContact.CRCC_ClientContactID.Value)))
                {
                    contactsToBeDeleted.Add(existingContact.CRCC_ClientContactID.Value);
                    existingContact.CRCC_IsDeleted = true;
                    existingContact.CRCC_ModifiedByID = currentUserId;
                    existingContact.CRCC_ModifiedOn = DateTime.Now;
                }
            }
            //Remove requirementSubscription for deleted contacts.
            if (contactsToBeDeleted.Count > AppConsts.NONE)
            {
                ifAnySharedCtctUpdated = RemoveRotationSubscriptionForClientContacts(clinicalRotationDetailContract, currentUserId, contactsToBeDeleted, dataMovementDueStatusId, dataMovementNotRequiredStatusId);
            }
            //Check whether selected contact exist in db if not exist then insert it
            foreach (Int32 contact in contactsToBeMapped)
            {
                if (!(existingContactList.Any(obj => obj.CRCC_ClientContactID == contact && obj.CRCC_IsDeleted == false)))
                {
                    ClinicalRotationClientContact newContact = new ClinicalRotationClientContact();
                    newContact.CRCC_ClientContactID = contact;
                    newContact.CRCC_IsDeleted = false;
                    newContact.CRCC_CreatedByID = currentUserId;
                    newContact.CRCC_CreatedOn = DateTime.Now;
                    rotationToBeUpdated.ClinicalRotationClientContacts.Add(newContact);
                }
            }

            foreach (Int32 contact in contactsToBeAdded)
            {
                Entity.SharedDataEntity.ClinicalRotationClientContactMapping newContactMapping = new Entity.SharedDataEntity.ClinicalRotationClientContactMapping();
                newContactMapping.CRCCM_ClientContactID = contact;
                newContactMapping.CRCCM_TenantID = clinicalRotationDetailContract.TenantID;
                newContactMapping.CRCCM_ClinicalRotationID = clinicalRotationDetailContract.RotationID;
                newContactMapping.CRCCM_IsDeleted = false;
                newContactMapping.CRCCM_CreatedByID = currentUserId;
                newContactMapping.CRCCM_CreatedOn = DateTime.Now;
                SharedDataDBContext.ClinicalRotationClientContactMappings.AddObject(newContactMapping);
                ifAnySharedCtctUpdated = true;
            }
            //Set the contact to send Email.
            clinicalRotationDetailContract.ContactsToSendEmail = clinicalRotationDetailContract.ContactsToSendEmail.Where(x => contactsToBeAdded.Contains(x.ClientContactID)).ToList();
            return ifAnySharedCtctUpdated;
        }

        private void AddUpdateRotationCustomAttributes(List<CustomAttribteContract> customAttributeListToUpdate, Int32 rotationId, Int32 currentUserId)
        {
            List<Int32> attributeIdsToUpdate = customAttributeListToUpdate.Select(cond => cond.CustomAttributeId).ToList();
            List<CustomAttributeMapping> existingCustomAttributeMappingInDb = GetCustomAttributeMappings(rotationId, attributeIdsToUpdate);
            foreach (CustomAttribteContract customAttributeToSave in customAttributeListToUpdate)
            {
                CustomAttributeMapping customAttributeMappingToUpdate = existingCustomAttributeMappingInDb.FirstOrDefault(cond =>
                                                                        cond.CAM_CustomAttributeMappingID == customAttributeToSave.CustomAttrMappingId);

                if (customAttributeMappingToUpdate.IsNotNull())
                {
                    customAttributeMappingToUpdate.CAM_ModifiedByID = currentUserId;
                    customAttributeMappingToUpdate.CAM_ModifiedOn = DateTime.Now;

                    CustomAttributeValue customAttributeValueToUpdate = customAttributeMappingToUpdate.CustomAttributeValues.FirstOrDefault(cond => !cond.CAV_IsDeleted);
                    customAttributeValueToUpdate.CAV_AttributeValue = customAttributeToSave.CustomAttributeValue;
                    customAttributeValueToUpdate.CAV_ModifiedByID = currentUserId;
                    customAttributeValueToUpdate.CAV_ModifiedOn = DateTime.Now;
                }
                else
                {
                    AddCustomAttributeMapping(rotationId, customAttributeToSave, currentUserId);
                }
            }
        }

        //UAT:4395

        private void AddClinicalRotationMemeber(ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentUserId, Int32 clinicalRotationID)
        {
            var GetRecordsExistsRotationMember = ClientDBContext.ClinicalRotationMembers.Where(x => x.CRM_ClinicalRotationID == clinicalRotationDetailContract.CloneRotationId && x.CRM_IsDeleted == false && x.CRM_IsDropped == false).ToList();
            foreach (var item in GetRecordsExistsRotationMember)
            {
                ClinicalRotationMember ObjClinicalRotationMember = new ClinicalRotationMember { CRM_ApplicantOrgUserID = item.CRM_ApplicantOrgUserID, CRM_ClinicalRotationID = clinicalRotationID, CRM_CreatedByID = currentUserId, CRM_CreatedOn = DateTime.Now, CRM_IsDropped = false };
                ClientDBContext.ClinicalRotationMembers.AddObject(ObjClinicalRotationMember);
            }
            ClientDBContext.SaveChanges();
        }

        private void AddSyllabusDocument(ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentUserId, Int32 clinicalRotationID, Int32 syllabusDocumentTypeID, int additionalDocumentsTypeId = 0)
        {
            if (!clinicalRotationDetailContract.SyllabusFileName.IsNullOrEmpty())
            {
                // Get the records which is allready save..
                var SyllabusFileName = ClientDBContext.ClinicalRotationDocuments.Join(ClientDBContext.ClientSystemDocuments, x => x.CRD_ClientSystemDocumentID, CD => CD.CSD_ID,
                (x, CD) => new { CSD = x, CRD = CD }).Where(x => x.CSD.CRD_ClinicalRotationID == clinicalRotationID && x.CSD.CRD_IsDeleted == false && x.CRD.CSD_DocumentTypeID == syllabusDocumentTypeID).FirstOrDefault();

                if (SyllabusFileName != null)
                {
                    SyllabusFileName.CSD.CRD_ModifiedByID = currentUserId;
                    SyllabusFileName.CSD.CRD_ModifiedOn = DateTime.Now;
                    SyllabusFileName.CSD.CRD_IsDeleted = true;
                    ClientSystemDocument ObjClientSystemDocuments = _dbContext.ClientSystemDocuments.Where(x => x.CSD_ID == SyllabusFileName.CSD.CRD_ClientSystemDocumentID && x.CSD_IsDeleted == false).FirstOrDefault();
                    ObjClientSystemDocuments.CSD_IsDeleted = true;
                    ObjClientSystemDocuments.CSD_ModifiedByID = currentUserId;
                    ObjClientSystemDocuments.CSD_ModifiedOn = DateTime.Now;
                }
                ClientSystemDocument newSyllabusDocument = new ClientSystemDocument();
                newSyllabusDocument.CSD_FileName = clinicalRotationDetailContract.SyllabusFileName;
                newSyllabusDocument.CSD_DocumentPath = clinicalRotationDetailContract.SyllabusFilePath;
                newSyllabusDocument.CSD_Description = "SyllabusDocument";
                newSyllabusDocument.CSD_DocumentTypeID = syllabusDocumentTypeID;
                newSyllabusDocument.CSD_CreatedByID = currentUserId;
                newSyllabusDocument.CSD_CreatedOn = DateTime.Now;
                ClinicalRotationDocument newSyllabusDocumentmapping = new ClinicalRotationDocument();
                newSyllabusDocumentmapping.CRD_ClinicalRotationID = clinicalRotationID;
                newSyllabusDocumentmapping.CRD_IsDeleted = false;
                newSyllabusDocumentmapping.CRD_CreatedByID = currentUserId;
                newSyllabusDocumentmapping.CRD_CreatedOn = DateTime.Now;
                newSyllabusDocument.ClinicalRotationDocuments.Add(newSyllabusDocumentmapping);
                ClientDBContext.ClientSystemDocuments.AddObject(newSyllabusDocument);
            }
            #region UAT: 4062

            ///Remove Ids
            ///Get all list data which is save already.
            var ListOfRotationDocuments = ClientDBContext.ClinicalRotationDocuments.Join(ClientDBContext.ClientSystemDocuments, x => x.CRD_ClientSystemDocumentID, CD => CD.CSD_ID,
                (x, CD) => new { CSD = x, CRD = CD }).Where(x => x.CSD.CRD_ClinicalRotationID == clinicalRotationID && x.CSD.CRD_IsDeleted == false && x.CRD.CSD_DocumentTypeID == additionalDocumentsTypeId).ToList();

            if (ListOfRotationDocuments != null && ListOfRotationDocuments.Count > AppConsts.NONE)
            {
                string[] GetDocumentUpdateIds = clinicalRotationDetailContract.ClinicalRotationDocumentUpdatedIds.Split(',');

                foreach (var item in ListOfRotationDocuments)
                {
                    if (!GetDocumentUpdateIds.Contains(item.CSD.CRD_ClientSystemDocumentID.ToString()))
                    {
                        item.CSD.CRD_ModifiedByID = currentUserId;
                        item.CSD.CRD_ModifiedOn = DateTime.Now;
                        item.CSD.CRD_IsDeleted = true;
                        ClientSystemDocument ObjClientSystemDocuments = _dbContext.ClientSystemDocuments.Where(x => x.CSD_ID == item.CSD.CRD_ClientSystemDocumentID && x.CSD_IsDeleted == false).FirstOrDefault();
                        if (ObjClientSystemDocuments != null)
                        {
                            ObjClientSystemDocuments.CSD_IsDeleted = true;
                            ObjClientSystemDocuments.CSD_ModifiedByID = currentUserId;
                            ObjClientSystemDocuments.CSD_ModifiedOn = DateTime.Now;
                        }
                    }

                }
            }

            // New Addition records add..
            if (clinicalRotationDetailContract.listOfMultipleDocument != null && clinicalRotationDetailContract.listOfMultipleDocument.Count > AppConsts.NONE && additionalDocumentsTypeId > AppConsts.NONE)
            {
                foreach (MultipleAdditionalDocumentsContract item in clinicalRotationDetailContract.listOfMultipleDocument)
                {
                    ClientSystemDocument ObjnewSyllabusDocument = new ClientSystemDocument();
                    ObjnewSyllabusDocument.CSD_FileName = item.AdditionalDocumentFileName;
                    ObjnewSyllabusDocument.CSD_DocumentPath = item.AdditionalDocumentFilePath;
                    ObjnewSyllabusDocument.CSD_Description = "AdditionalDocument";
                    ObjnewSyllabusDocument.CSD_Size = item.AdditionalDocumentFileSize;
                    ObjnewSyllabusDocument.CSD_DocumentTypeID = additionalDocumentsTypeId;
                    ObjnewSyllabusDocument.CSD_CreatedByID = currentUserId;
                    ObjnewSyllabusDocument.CSD_CreatedOn = DateTime.Now;
                    ClinicalRotationDocument ObjnewSyllabusDocumentmapping = new ClinicalRotationDocument();
                    ObjnewSyllabusDocumentmapping.CRD_ClinicalRotationID = clinicalRotationID;
                    ObjnewSyllabusDocumentmapping.CRD_IsDeleted = false;
                    ObjnewSyllabusDocumentmapping.CRD_CreatedByID = currentUserId;
                    ObjnewSyllabusDocumentmapping.CRD_CreatedOn = DateTime.Now;
                    ObjnewSyllabusDocument.ClinicalRotationDocuments.Add(ObjnewSyllabusDocumentmapping);
                    ClientDBContext.ClientSystemDocuments.AddObject(ObjnewSyllabusDocument);
                }

            }

            #region Clone of Rotation....
            if (ListOfRotationDocuments.Count == AppConsts.NONE && !clinicalRotationDetailContract.ClinicalRotationDocumentUpdatedIds.IsNullOrEmpty() && additionalDocumentsTypeId > AppConsts.NONE)
            {
                string[] GetDocumentUpdateIds = clinicalRotationDetailContract.ClinicalRotationDocumentUpdatedIds.Split(',');
                foreach (var item in GetDocumentUpdateIds)
                {
                    if (!item.IsNullOrEmpty())
                    {
                        int ClientSystemDocumentID = Convert.ToInt32(item);
                        var ObjOfData = ClientDBContext.ClinicalRotationDocuments.Join(ClientDBContext.ClientSystemDocuments, x => x.CRD_ClientSystemDocumentID, CD => CD.CSD_ID,
                        (x, CD) => new { CSD = x, CRD = CD }).Where(x => x.CSD.CRD_ClientSystemDocumentID == ClientSystemDocumentID && x.CSD.CRD_IsDeleted == false && x.CRD.CSD_DocumentTypeID == additionalDocumentsTypeId).FirstOrDefault();
                        if (ObjOfData.CRD != null)
                        {
                            ClientSystemDocument ObjnewSyllabusDocument = new ClientSystemDocument();
                            ObjnewSyllabusDocument.CSD_FileName = ObjOfData.CRD.CSD_FileName;
                            ObjnewSyllabusDocument.CSD_DocumentPath = ObjOfData.CRD.CSD_DocumentPath;
                            ObjnewSyllabusDocument.CSD_Description = "AdditionalDocument";
                            ObjnewSyllabusDocument.CSD_Size = ObjOfData.CRD.CSD_Size;
                            ObjnewSyllabusDocument.CSD_DocumentTypeID = additionalDocumentsTypeId;
                            ObjnewSyllabusDocument.CSD_CreatedByID = currentUserId;
                            ObjnewSyllabusDocument.CSD_CreatedOn = DateTime.Now;
                            ClinicalRotationDocument ObjnewSyllabusDocumentmapping = new ClinicalRotationDocument();
                            ObjnewSyllabusDocumentmapping.CRD_ClinicalRotationID = clinicalRotationID;
                            ObjnewSyllabusDocumentmapping.CRD_IsDeleted = false;
                            ObjnewSyllabusDocumentmapping.CRD_CreatedByID = currentUserId;
                            ObjnewSyllabusDocumentmapping.CRD_CreatedOn = DateTime.Now;
                            ObjnewSyllabusDocument.ClinicalRotationDocuments.Add(ObjnewSyllabusDocumentmapping);
                            ClientDBContext.ClientSystemDocuments.AddObject(ObjnewSyllabusDocument);
                        }
                    }
                }
            }
            #endregion
            #endregion
        }
        #endregion

        #region UAT 1304 : Instructor/Preceptor screens and functionality
        /// <summary>
        /// Returns the list of ClientSystemDocuments for all the rotations attached to Client Contact.
        /// </summary>
        /// <param name="clientContactID"></param>
        /// <param name="tenantId"></param>
        /// <returns>list of ClientSystemDocuments</returns>
        List<ClientContactSyllabusDocumentContract> IClinicalRotationRepository.GetClientContactRotationDocuments(Int32 clientContactID)
        {
            List<ClientContactSyllabusDocumentContract> clientContactSyllabusDocumentList = new List<ClientContactSyllabusDocumentContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetClientContactSyllabusDocs", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@clientContactID", clientContactID);

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@clientContactID", clientContactID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClientContactSyllabusDocs", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClientContactSyllabusDocumentContract clientContactSyllabusDocument = new ClientContactSyllabusDocumentContract();
                            clientContactSyllabusDocument.DocumentID = dr["DocumentID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["DocumentID"]);
                            clientContactSyllabusDocument.ComplioID = dr["ComplioID"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["ComplioID"]);
                            clientContactSyllabusDocument.RotationName = dr["RotationName"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["RotationName"]);
                            clientContactSyllabusDocument.Department = dr["Department"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Department"]);
                            clientContactSyllabusDocument.Program = dr["Program"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Program"]);
                            clientContactSyllabusDocument.Course = dr["Course"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Course"]);
                            clientContactSyllabusDocument.FileName = dr["FileName"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["FileName"]);
                            clientContactSyllabusDocument.ClinicalRotationID = dr["ClinicalRotationID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ClinicalRotationID"]);
                            clientContactSyllabusDocumentList.Add(clientContactSyllabusDocument);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return clientContactSyllabusDocumentList;
        }
        #endregion

        #region UAT 1302 As an admin (client or ADB), I should be able to create preceptors/instructors
        /// <summary>
        /// Check whether there is ClientRotation and ClientContact mapping exist or not 
        /// </summary>
        /// <param name="clientContactID"></param>
        /// <returns></returns>
        Boolean IClinicalRotationRepository.IsClientRotationClientContactMappingExist(Int32 clientContactID)
        {
            return _dbContext.ClinicalRotationClientContacts.Any(cond => cond.CRCC_ClientContactID == clientContactID && !cond.CRCC_IsDeleted);
        }
        #endregion

        #region Manage Invitations and Rotations for Shared User

        /// <summary>
        /// Get Clinical Rotations by Clinical Rotation IDs
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="clinicalRotationXML"></param>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        List<ClinicalRotationDetailContract> IClinicalRotationRepository.GetClinicalRotationsByIDs(Int32 currentLoggedInUserId, String clinicalRotationXML, ClinicalRotationDetailContract clinicalRotationDetailContract, CustomPagingArgsContract customPagingArgsContract)
        {
            String spName = String.Empty;
            if (clinicalRotationDetailContract.IsInstructor)
                spName = "usp_GetClinicalRotationsByIDsForIP";
            else
                spName = "usp_GetClinicalRotationsByIDs";

            List<ClinicalRotationDetailContract> clinicalRotationDetailList = new List<ClinicalRotationDetailContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@xmldata", clinicalRotationXML),
                             new SqlParameter("@searchDataXML", clinicalRotationDetailContract.XML),
                             new SqlParameter("@LoggedInUserId", currentLoggedInUserId)
                             //new SqlParameter("@customFilteringXML", customPagingArgsContract.XML)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader col = base.ExecuteSQLDataReader(con, spName, sqlParameterCollection))
                {
                    while (col.Read())
                    {
                        ClinicalRotationDetailContract clinicalRotationDetail = new ClinicalRotationDetailContract();

                        //clinicalRotationDetail.ProfileSharingInvGroupID = col["ProfileSharingInvGroupID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ProfileSharingInvGroupID"]);
                        clinicalRotationDetail.RotationID = col["ClinicalRotationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ClinicalRotationID"]);
                        clinicalRotationDetail.ComplioID = col["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComplioID"]);
                        clinicalRotationDetail.RotationName = col["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(col["RotationName"]);
                        clinicalRotationDetail.Department = col["Department"] == DBNull.Value ? String.Empty : Convert.ToString(col["Department"]);
                        clinicalRotationDetail.Program = col["Program"] == DBNull.Value ? String.Empty : Convert.ToString(col["Program"]);
                        clinicalRotationDetail.Course = col["Course"] == DBNull.Value ? String.Empty : Convert.ToString(col["Course"]);
                        clinicalRotationDetail.StartDate = col["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["StartDate"]);
                        clinicalRotationDetail.EndDate = col["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["EndDate"]);
                        clinicalRotationDetail.UnitFloorLoc = col["UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(col["UnitFloorLoc"]);
                        clinicalRotationDetail.RecommendedHours = col["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(col["NoOfHours"]));
                        clinicalRotationDetail.Shift = col["RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(col["RotationShift"]);
                        clinicalRotationDetail.StartTime = col["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(col["StartTime"]);
                        clinicalRotationDetail.EndTime = col["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(col["EndTime"]);
                        clinicalRotationDetail.Time = col["Times"] == DBNull.Value ? String.Empty : Convert.ToString(col["Times"]);
                        //clinicalRotationDetail.AgencyID = col["AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(col["AgencyID"]);
                        clinicalRotationDetail.AgencyName = col["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(col["AgencyName"]);
                        clinicalRotationDetail.DaysName = col["Days"].GetType().Name == "DBNull" ? null : Convert.ToString(col["Days"]);
                        clinicalRotationDetail.Term = col["Term"] == DBNull.Value ? String.Empty : Convert.ToString(col["Term"]);
                        clinicalRotationDetail.TotalRecordCount = col["TotalCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TotalCount"]);
                        clinicalRotationDetail.IsInstructorPreceptorPkgAvailable = col["IsInstructorPreceptorPkgAvailable"] == DBNull.Value ? false : Convert.ToBoolean(col["IsInstructorPreceptorPkgAvailable"]);
                        //if (!clinicalRotationDetailContract.IsInstructor)
                        //{
                        clinicalRotationDetail.RotationReviewStatusName = col["SharedUserRotationReviewStatusName"] == DBNull.Value ? String.Empty : Convert.ToString(col["SharedUserRotationReviewStatusName"]);
                        clinicalRotationDetail.RotationReviewID = col["SharedUserRotationReviewID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["SharedUserRotationReviewID"]);
                        //}
                        //clinicalRotationDetail.PkgSubscriptionId = col["RequirementPkgSubscriptionId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RequirementPkgSubscriptionId"]);
                        clinicalRotationDetail.TypeSpecialty = col["TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(col["TypeSpecialty"]);
                        //UAT-1769Addition of "# of Students" field on rotation creation and rotation details for all except students
                        clinicalRotationDetail.Students = col["NoOfStudents"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(col["NoOfStudents"]));
                        clinicalRotationDetail.IpReqPkgStatus = col["IpReqPkgStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["IpReqPkgStatus"]);
                        clinicalRotationDetail.CustomAttributes = col["CustomAttributeList"] == DBNull.Value ? String.Empty : Convert.ToString(col["CustomAttributeList"]); //UAT-3594
                        clinicalRotationDetailList.Add(clinicalRotationDetail);
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return clinicalRotationDetailList;
        }

        #endregion

        #region UAT-1701, New Agency User Search
        /// <summary>
        /// Get Clinical Rotations by Clinical Rotation IDs
        /// </summary>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="clinicalRotationXML"></param>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        List<ClinicalRotationDetailContract> IClinicalRotationRepository.GetClinicalRotationsWithStudentByIDs(Int32 currentLoggedInUserId, String clinicalRotationXML, ClinicalRotationDetailContract clinicalRotationDetailContract, String customAttributeXML)
        {
            List<ClinicalRotationDetailContract> clinicalRotationDetailList = new List<ClinicalRotationDetailContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@xmldata", clinicalRotationXML),
                             new SqlParameter("@searchDataXML", clinicalRotationDetailContract.XML),
                             new SqlParameter("@LoggedInUserId", currentLoggedInUserId),
                             new SqlParameter("@DroppedStatus", AppConsts.APPLICANT_DROPPED_STATUS),
                             new SqlParameter("@customAttributeXML", customAttributeXML) //UAT-3165
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader col = base.ExecuteSQLDataReader(con, "usp_GetClinicalRotationsWithStudentsByIDs", sqlParameterCollection))
                {
                    while (col.Read())
                    {
                        ClinicalRotationDetailContract clinicalRotationDetail = new ClinicalRotationDetailContract();

                        //clinicalRotationDetail.ProfileSharingInvGroupID = col["ProfileSharingInvGroupID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ProfileSharingInvGroupID"]);
                        clinicalRotationDetail.RotationID = col["ClinicalRotationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ClinicalRotationID"]);
                        clinicalRotationDetail.ComplioID = col["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComplioID"]);
                        clinicalRotationDetail.RotationName = col["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(col["RotationName"]);
                        clinicalRotationDetail.Department = col["Department"] == DBNull.Value ? String.Empty : Convert.ToString(col["Department"]);
                        clinicalRotationDetail.Program = col["Program"] == DBNull.Value ? String.Empty : Convert.ToString(col["Program"]);
                        clinicalRotationDetail.Course = col["Course"] == DBNull.Value ? String.Empty : Convert.ToString(col["Course"]);
                        clinicalRotationDetail.StartDate = col["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["StartDate"]);
                        clinicalRotationDetail.EndDate = col["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["EndDate"]);
                        clinicalRotationDetail.UnitFloorLoc = col["UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(col["UnitFloorLoc"]);
                        clinicalRotationDetail.RecommendedHours = col["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(col["NoOfHours"]));
                        clinicalRotationDetail.Shift = col["RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(col["RotationShift"]);
                        clinicalRotationDetail.StartTime = col["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(col["StartTime"]);
                        clinicalRotationDetail.EndTime = col["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(col["EndTime"]);
                        clinicalRotationDetail.Time = col["Times"] == DBNull.Value ? String.Empty : Convert.ToString(col["Times"]);
                        clinicalRotationDetail.AgencyID = col["AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(col["AgencyID"]);
                        clinicalRotationDetail.AgencyName = col["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(col["AgencyName"]);
                        clinicalRotationDetail.DaysName = col["Days"].GetType().Name == "DBNull" ? null : Convert.ToString(col["Days"]);
                        clinicalRotationDetail.Term = col["Term"] == DBNull.Value ? String.Empty : Convert.ToString(col["Term"]);
                        clinicalRotationDetail.TotalRecordCount = col["TotalCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TotalCount"]);
                        clinicalRotationDetail.IsInstructorPreceptorPkgAvailable = col["IsInstructorPreceptorPkgAvailable"] == DBNull.Value ? false : Convert.ToBoolean(col["IsInstructorPreceptorPkgAvailable"]);
                        clinicalRotationDetail.SharedUserInvitationReviewID = col["SharedUserInvitationReviewID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["SharedUserInvitationReviewID"]);
                        clinicalRotationDetail.SharedUserInvitationReviewStatusName = col["SharedUserInvitationReviewStatusName"] == DBNull.Value ? String.Empty : Convert.ToString(col["SharedUserInvitationReviewStatusName"]);
                        //clinicalRotationDetail.RotationReviewID = col["SharedUserRotationReviewID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["SharedUserRotationReviewID"]);
                        //clinicalRotationDetail.RotationReviewStatusName = col["SharedUserRotationReviewStatusName"] == DBNull.Value ? String.Empty : Convert.ToString(col["SharedUserRotationReviewStatusName"]);
                        //clinicalRotationDetail.PkgSubscriptionId = col["RequirementPkgSubscriptionId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RequirementPkgSubscriptionId"]);
                        clinicalRotationDetail.TypeSpecialty = col["TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(col["TypeSpecialty"]);
                        //UAT-1769Addition of "# of Students" field on rotation creation and rotation details for all except students
                        clinicalRotationDetail.Students = col["NoOfStudents"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(col["NoOfStudents"]));
                        clinicalRotationDetail.FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]);
                        clinicalRotationDetail.LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]);
                        //UAT-3977
                        clinicalRotationDetail.IsInstructorShare = col["IsInstructorShare"] == DBNull.Value ? false : Convert.ToBoolean(col["IsInstructorShare"]);
                        clinicalRotationDetail.ApplicantRequirementPkgStatus = col["ApplicantRequirementPkgStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantRequirementPkgStatus"]);

                        clinicalRotationDetail.CustomAttributes = col["CustomAttributeList"] == DBNull.Value ? String.Empty : Convert.ToString(col["CustomAttributeList"]);//UAT-3165
                        clinicalRotationDetail.ProfileSharingInvGroupID = col["PSIG_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(col["PSIG_ID"]);//UAT-4399
                        clinicalRotationDetail.DroppedDate = col["DroppedDate"] == DBNull.Value || col["DroppedDate"].IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(col["DroppedDate"]);
                        bool existChk = clinicalRotationDetailList.Exists(s => s == clinicalRotationDetail);
                        bool existChk1 = clinicalRotationDetailList.Exists(s => s.RotationID == clinicalRotationDetail.RotationID);
                        clinicalRotationDetailList.Add(clinicalRotationDetail);
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return clinicalRotationDetailList.ToList();
        }

        #endregion

        #region Rotation Student Detail for Shared User

        List<ApplicantDataListContract> IClinicalRotationRepository.GetRotationStudentsDetail(CustomPagingArgsContract customPagingArgsContract, String applicantUserIDsXML, RotationStudentDetailContract searchDataContract)
        {
            List<ApplicantDataListContract> applicantDataList = new List<ApplicantDataListContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@xmldata", applicantUserIDsXML),
                             new SqlParameter("@customFilteringXml", customPagingArgsContract.CreateXml()),
                             new SqlParameter("@FirstName", searchDataContract.ApplicantFirstName),
                             new SqlParameter("@LastName", searchDataContract.ApplicantLastName),
                             new SqlParameter("@EmailAddress", searchDataContract.ApplicantEmail),
                             new SqlParameter("@SSN", searchDataContract.ApplicantSSN),
                             new SqlParameter("@DOB", searchDataContract.DateOfBirth),
                             new SqlParameter("@RotationId", searchDataContract.ClinicalRotationID),
                             new SqlParameter("@DroppedStatus", AppConsts.APPLICANT_DROPPED_STATUS),
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationStudentDetail", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ApplicantDataListContract applicantDataListContract = new ApplicantDataListContract();
                            applicantDataListContract.OrganizationUserId = dr["OrganizationUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OrganizationUserId"]);
                            applicantDataListContract.ApplicantFirstName = Convert.ToString(dr["ApplicantFirstName"]);
                            applicantDataListContract.ApplicantMiddleName = Convert.ToString(dr["ApplicantFirstName"]);
                            applicantDataListContract.ApplicantLastName = Convert.ToString(dr["ApplicantLastName"]);
                            applicantDataListContract.DateOfBirth = dr["DateOfBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DateOfBirth"]);
                            applicantDataListContract.EmailAddress = Convert.ToString(dr["EmailAddress"]);
                            applicantDataListContract.SSN = Convert.ToString(dr["SSN"]);
                            applicantDataListContract.TotalCount = dr["TotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TotalCount"]);
                            applicantDataListContract.InvitationDate = dr["InvitationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["InvitationDate"]);
                            applicantDataListContract.ProfileSharingInvID = dr["ProfileSharingInvID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ProfileSharingInvID"]);
                            applicantDataListContract.PhoneNumber = Convert.ToString(dr["PhoneNumber"]);
                            applicantDataListContract.ExpirationDate = dr["ExpirationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationDate"]);
                            applicantDataListContract.ViewsRemaining = dr["ViewsRemaining"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ViewsRemaining"]);
                            applicantDataListContract.IsInvitationVisible = dr["IsInvitationVisible"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsInvitationVisible"]);
                            applicantDataListContract.ComplianceStatus = dr["ComplianceStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplianceStatus"]);
                            applicantDataListContract.IsApplicant = dr["IsApplicant"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsApplicant"]);
                            applicantDataListContract.InstitutionHierarchy = dr["InstitutionHierarchy"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InstitutionHierarchy"]);
                            applicantDataListContract.InvitationReviewStatus = dr["InvitationReviewStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InvitationReviewStatus"]);
                            applicantDataListContract.InvitationReviewStatusCode = dr["InvitationReviewStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InvitationReviewStatusCode"]);
                            applicantDataListContract.IsDropped = dr["IsDropped"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsDropped"]);
                            applicantDataListContract.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]); //UAT-2705
                            applicantDataListContract.InstructorName = dr["InstructorName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InstructorName"]); //UAT-2705
                            applicantDataListContract.RotationStartDate = dr["RotationStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RotationStartDate"]); //UAT-2705
                            applicantDataListContract.RotationEndDate = dr["RotationEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RotationEndDate"]); //UAT-2705
                            applicantDataListContract.StudentDroppedDate = dr["StudentDroppedDate"] == DBNull.Value || dr["StudentDroppedDate"].IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(dr["StudentDroppedDate"]); //UAT-4460
                            #region UAT-2923
                            applicantDataListContract.RotationSharedByUserName = dr["RotationSharedByUserName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationSharedByUserName"]);
                            applicantDataListContract.RotationSharedByUserEmailId = dr["RotationSharedByUserEmailId"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationSharedByUserEmailId"]);
                            #endregion

                            //UAT-3421
                            applicantDataListContract.RotationSharedByUserOrgUserId = dr["RotationSharedByUserOrgUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RotationSharedByUserOrgUserId"]);

                            applicantDataList.Add(applicantDataListContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

            }
            return applicantDataList;
        }

        List<ApplicantDataListContract> IClinicalRotationRepository.GetInstructorRotationStudentsDetail(CustomPagingArgsContract customPagingArgsContract, RotationStudentDetailContract searchDataContract)
        {
            List<ApplicantDataListContract> applicantDataList = new List<ApplicantDataListContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@customFilteringXml", customPagingArgsContract.CreateXml()),
                             new SqlParameter("@FirstName", searchDataContract.ApplicantFirstName),
                             new SqlParameter("@LastName", searchDataContract.ApplicantLastName),
                             new SqlParameter("@EmailAddress", searchDataContract.ApplicantEmail),
                             new SqlParameter("@SSN", searchDataContract.ApplicantSSN),
                             new SqlParameter("@DOB", searchDataContract.DateOfBirth),
                             new SqlParameter("@RotationId", searchDataContract.ClinicalRotationID),
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationStudentDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ApplicantDataListContract applicantDataListContract = new ApplicantDataListContract();
                            applicantDataListContract.OrganizationUserId = dr["OrganizationUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OrganizationUserId"]);
                            applicantDataListContract.ApplicantFirstName = Convert.ToString(dr["ApplicantFirstName"]);
                            applicantDataListContract.ApplicantMiddleName = Convert.ToString(dr["ApplicantFirstName"]);
                            applicantDataListContract.ApplicantLastName = Convert.ToString(dr["ApplicantLastName"]);
                            applicantDataListContract.DateOfBirth = dr["DateOfBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DateOfBirth"]);
                            applicantDataListContract.EmailAddress = Convert.ToString(dr["EmailAddress"]);
                            applicantDataListContract.SSN = Convert.ToString(dr["SSN"]);
                            applicantDataListContract.TotalCount = dr["TotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TotalCount"]);
                            applicantDataListContract.PhoneNumber = Convert.ToString(dr["PhoneNumber"]);
                            applicantDataListContract.ComplianceStatus = dr["ComplianceStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplianceStatus"]);
                            applicantDataListContract.InstitutionHierarchy = dr["InstitutionHierarchy"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InstitutionHierarchy"]);
                            //UAT-4148
                            applicantDataListContract.ReqPkgSubscriptionId = dr["ReqPkgSubscriptionId"].GetType().Name == "DBNull"
                                                                                    ? 0 : Convert.ToInt32(dr["ReqPkgSubscriptionId"]);
                            applicantDataListContract.ClinicalRotaionId = dr["ClinicalRotaionId"].GetType().Name == "DBNull"
                                                                                   ? 0 : Convert.ToInt32(dr["ClinicalRotaionId"]);
                            //UAT-4200
                            applicantDataListContract.RotationSharedByUserName = dr["RotationSharedByUserName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationSharedByUserName"]);
                            applicantDataListContract.RotationSharedByUserEmailId = dr["RotationSharedByUserEmailId"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationSharedByUserEmailId"]);
                            applicantDataListContract.RotationSharedByUserOrgUserId = dr["RotationSharedByUserOrgUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RotationSharedByUserOrgUserId"]);

                            applicantDataList.Add(applicantDataListContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return applicantDataList;
        }
        //END
        /// <summary>
        /// Get Rotation Student details for Instructor Preceptor
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="userID"></param>
        /// <param name="searchContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        //List<RotationMemberSearchDetailContract> IClinicalRotationRepository.GetInstrctrPreceptrRotationStudents(String tenantID, Guid userID, RotationMemberSearchDetailContract searchContract, CustomPagingArgsContract customPagingArgsContract)
        //{
        //    List<RotationMemberSearchDetailContract> rotationDetailList = new List<RotationMemberSearchDetailContract>();
        //    EntityConnection connection = _dbContext.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {
        //        SqlParameter[] sqlParameterCollection = new SqlParameter[]
        //                {
        //                     new SqlParameter("@searchDataXML", searchContract.XML),
        //                     new SqlParameter("@customFilteringXML", customPagingArgsContract.XML),
        //                     new SqlParameter("@UserID", userID),
        //                     new SqlParameter("@TenantID", tenantID)
        //                };

        //        base.OpenSQLDataReaderConnection(con);
        //        using (SqlDataReader col = base.ExecuteSQLDataReader(con, "usp_GetRotationStudentSearchData", sqlParameterCollection))
        //        {
        //            while (col.Read())
        //            {
        //                RotationMemberSearchDetailContract rotationDetail = new RotationMemberSearchDetailContract();

        //                rotationDetail.RotationID = col["ClinicalRotationID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ClinicalRotationID"]);
        //                rotationDetail.ComplioID = col["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComplioID"]);
        //                rotationDetail.RotationName = col["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(col["RotationName"]);
        //                rotationDetail.Department = col["Department"] == DBNull.Value ? String.Empty : Convert.ToString(col["Department"]);
        //                rotationDetail.Program = col["Program"] == DBNull.Value ? String.Empty : Convert.ToString(col["Program"]);
        //                rotationDetail.Course = col["Course"] == DBNull.Value ? String.Empty : Convert.ToString(col["Course"]);
        //                rotationDetail.StartDate = col["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["StartDate"]);
        //                rotationDetail.EndDate = col["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["EndDate"]);
        //                rotationDetail.UnitFloorLoc = col["UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(col["UnitFloorLoc"]);
        //                rotationDetail.RecommendedHours = col["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(col["NoOfHours"]));
        //                rotationDetail.Shift = col["RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(col["RotationShift"]);
        //                rotationDetail.StartTime = col["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(col["StartTime"]);
        //                rotationDetail.EndTime = col["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(col["EndTime"]);
        //                rotationDetail.Time = col["Times"] == DBNull.Value ? String.Empty : Convert.ToString(col["Times"]);
        //                rotationDetail.AgencyName = col["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(col["AgencyName"]);
        //                rotationDetail.DaysName = col["Days"].GetType().Name == "DBNull" ? null : Convert.ToString(col["Days"]);
        //                rotationDetail.Term = col["Term"] == DBNull.Value ? String.Empty : Convert.ToString(col["Term"]);
        //                rotationDetail.TypeSpecialty = col["TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(col["TypeSpecialty"]);
        //                rotationDetail.ApplicantFirstName = col["ApplicantFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantFirstName"]);
        //                rotationDetail.ApplicantLastName = col["ApplicantLastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantLastName"]);
        //                rotationDetail.ApplicantSSN = col["ApplicantSSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantSSN"]);
        //                rotationDetail.DateOfBirth = col["DateOfBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["DateOfBirth"]);
        //                rotationDetail.TotalRecordCount = col["TotalCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TotalCount"]);

        //                //UAT-4013
        //                rotationDetail.SlctdAgencyID = col["AgencyID"] == DBNull.Value ? String.Empty : Convert.ToString(col["AgencyID"]);
        //                rotationDetail.OrganizationUserID = col["ApplicantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ApplicantID"]);
        //                rotationDetailList.Add(rotationDetail);
        //            }
        //        }

        //        base.CloseSQLDataReaderConnection(con);
        //    }
        //    return rotationDetailList;
        //}

        #endregion

        /// <summary>
        /// Get the UserDetails by OrganizationUserId
        /// </summary>
        /// <param name="orgUserId"></param>
        /// <returns></returns>
        usp_GetUserDetails_Result IClinicalRotationRepository.GetUserData(Int32 orgUserId)
        {
            return _dbContext.GetUserDetails(Convert.ToInt32(orgUserId)).First();
        }

        public List<RequirementPackageSubscription> CreateRotationSubscriptionForClientContact(List<Int32> clientContactIds, Int32 clinicalRotationID, Int32 reqPkgTypeId, Int32 rotationSubscriptionTypeID,
                                                                                                               Int32 requirementNotCompliantPackStatusID, Int32 currentLoggedInUserId)
        {
            if (clientContactIds.IsNullOrEmpty())
            {
                List<ClinicalRotationClientContact> rotationContact = _dbContext.ClinicalRotationClientContacts.Where(cond => cond.CRCC_ClinicalRotationID == clinicalRotationID
                                                                                                                        && !cond.CRCC_IsDeleted).ToList();
                if (!rotationContact.IsNullOrEmpty())
                    clientContactIds = rotationContact.Select(cond => cond.CRCC_ClientContactID.Value).ToList();
            }
            List<Entity.SharedDataEntity.ClientContactProfileSynchronization> lstAlreadySyncContacts = SharedDataDBContext.ClientContactProfileSynchronizations.Where(cond => clientContactIds.Contains(cond.CCPS_ClientContactID)
                                                                                                             && cond.CCPS_OrgUserID != null
                                                                                                             && !cond.CCPS_IsDeleted).ToList();
            ClinicalRotationRequirementPackage clinicalRotationRequirementPackage = GetRotationRequirementPackageByRotationId(clinicalRotationID, reqPkgTypeId);

            List<RequirementPackageSubscription> lstRequirementPackageSubscription = new List<RequirementPackageSubscription>();
            if (clinicalRotationRequirementPackage.IsNotNull())
            {

                lstAlreadySyncContacts.ForEach(cond =>
                {
                    //Commented in UAT-4960
                    //var existingSubscription = _dbContext.ClinicalRotationSubscriptions
                    //   .Where(crs =>
                    //       crs.ClinicalRotationRequirementPackage.RequirementPackage.RP_ID == clinicalRotationRequirementPackage.CRRP_RequirementPackageID
                    //       && !crs.ClinicalRotationRequirementPackage.CRRP_IsDeleted
                    //       && !crs.ClinicalRotationRequirementPackage.ClinicalRotation.CR_IsDeleted
                    //       && !crs.RequirementPackageSubscription.RPS_IsDeleted
                    //       && !crs.CRS_IsDeleted
                    //       && crs.RequirementPackageSubscription.RPS_ApplicantOrgUserID == cond.CCPS_OrgUserID.Value
                    //           ).OrderByDescending(col => col.RequirementPackageSubscription.RPS_ID)
                    //           .Select(crs => crs.RequirementPackageSubscription).FirstOrDefault(); 
                    //Commented in UAT-4960
                    // if (existingSubscription.IsNullOrEmpty())
                    // {
                    //Add records in RequirementPackageSubscription table
                    Boolean isSubscriptionExistForSameRotation = _dbContext.ClinicalRotationSubscriptions.Any(crs => !crs.CRS_IsDeleted && !crs.ClinicalRotationRequirementPackage.CRRP_IsDeleted
                             && crs.ClinicalRotationRequirementPackage.CRRP_ClinicalRotationID == clinicalRotationID
                             && crs.ClinicalRotationRequirementPackage.CRRP_RequirementPackageID == clinicalRotationRequirementPackage.CRRP_RequirementPackageID
                             && !crs.RequirementPackageSubscription.RPS_IsDeleted && crs.RequirementPackageSubscription.RPS_ApplicantOrgUserID == cond.CCPS_OrgUserID.Value);

                    if (!isSubscriptionExistForSameRotation)
                    {
                        RequirementPackageSubscription requirementPackageSubscription = new RequirementPackageSubscription();
                        requirementPackageSubscription.RPS_RequirementPackageID = clinicalRotationRequirementPackage.CRRP_RequirementPackageID;
                        requirementPackageSubscription.RPS_RequirementSubscriptionTypeID = rotationSubscriptionTypeID;
                        requirementPackageSubscription.RPS_ApplicantOrgUserID = cond.CCPS_OrgUserID.Value;
                        requirementPackageSubscription.RPS_RequirementPackageStatusID = requirementNotCompliantPackStatusID;
                        requirementPackageSubscription.RPS_IsDeleted = false;
                        requirementPackageSubscription.RPS_CreatedByID = currentLoggedInUserId;
                        requirementPackageSubscription.RPS_CreatedOn = DateTime.Now;

                        //Add records in ClinicalRotationSubscription table
                        ClinicalRotationSubscription clinicalRotationSubscription = new ClinicalRotationSubscription();
                        clinicalRotationSubscription.CRS_ClinicalRotationRequirementPackageID = clinicalRotationRequirementPackage.CRRP_ID;
                        clinicalRotationSubscription.CRS_IsDeleted = false;
                        clinicalRotationSubscription.CRS_CreatedByID = currentLoggedInUserId;
                        clinicalRotationSubscription.CRS_CreatedOn = DateTime.Now;

                        requirementPackageSubscription.ClinicalRotationSubscriptions.Add(clinicalRotationSubscription);
                        _dbContext.RequirementPackageSubscriptions.AddObject(requirementPackageSubscription);
                        lstRequirementPackageSubscription.Add(requirementPackageSubscription);
                    }
                    //Commented in UAT-4960
                    // }
                    //else
                    //{
                    //    var existingClinicalRotationSubscription = _dbContext.ClinicalRotationSubscriptions
                    //                                                .Where(crs => !crs.CRS_IsDeleted
                    //                                                        && crs.CRS_ClinicalRotationRequirementPackageID == clinicalRotationRequirementPackage.CRRP_ID
                    //                                                        && crs.CRS_RequirementPackageSubscriptionID == existingSubscription.RPS_ID).FirstOrDefault();


                    //    if (existingClinicalRotationSubscription.IsNullOrEmpty())
                    //    {
                    //        //Add records in ClinicalRotationSubscription table
                    //        ClinicalRotationSubscription clinicalRotationSubscription = new ClinicalRotationSubscription();
                    //        clinicalRotationSubscription.CRS_ClinicalRotationRequirementPackageID = clinicalRotationRequirementPackage.CRRP_ID;
                    //        clinicalRotationSubscription.CRS_IsDeleted = false;
                    //        clinicalRotationSubscription.CRS_CreatedByID = currentLoggedInUserId;
                    //        clinicalRotationSubscription.CRS_CreatedOn = DateTime.Now;
                    //        clinicalRotationSubscription.CRS_RequirementPackageSubscriptionID = existingSubscription.RPS_ID;
                    //        _dbContext.ClinicalRotationSubscriptions.AddObject(clinicalRotationSubscription);
                    //    }
                    //}
                });
            }
            return lstRequirementPackageSubscription;
        }

        void IClinicalRotationRepository.UpdateRotationSubscriptionForClientContact(Int32 clinicalRotationID, Int32 currentLoggedInUserId, Int32 oldPkgId, Int32 newPkgId, Int32 rotationSubscriptionTypeID,
                                                                                                               Int32 requirementNotCompliantPackStatusID, Int16 dataMovementDueStatusId)
        {
            try
            {
                ClinicalRotationRequirementPackage clinicalRotationRequirementPackage = _dbContext.ClinicalRotationRequirementPackages.FirstOrDefault(x => x.CRRP_ClinicalRotationID == clinicalRotationID
                                                                                    && !x.CRRP_IsDeleted && x.CRRP_RequirementPackageID == newPkgId);

                List<RequirementPackageSubscription> lstRequirementPackageSubscriptionToBeAdded = new List<RequirementPackageSubscription>();
                if (clinicalRotationRequirementPackage.IsNotNull())
                {
                    List<ClinicalRotationSubscription> lstClinicalRotationSubscription = GetClinicalRotationSubscriptionsByRotationReqPackId(clinicalRotationRequirementPackage.CRRP_ID);
                    if (lstClinicalRotationSubscription.IsNotNull())
                    {
                        List<Int32> lstRequirementPackageSubscriptionID = lstClinicalRotationSubscription.Select(x => x.CRS_RequirementPackageSubscriptionID).ToList();
                        List<RequirementPackageSubscription> lstRequirementPackageSubscription = _dbContext.RequirementPackageSubscriptions.Where(con => lstRequirementPackageSubscriptionID.Contains(con.RPS_ID) && !con.RPS_IsDeleted).ToList();
                        lstRequirementPackageSubscription.ForEach(con =>
                        {
                            //Commented IN UAT-4960
                            //            var existingSubscription = _dbContext.ClinicalRotationSubscriptions
                            //                                        .Where(crs =>
                            //                                           !crs.ClinicalRotationRequirementPackage.CRRP_IsDeleted
                            //                                            && !crs.ClinicalRotationRequirementPackage.ClinicalRotation.CR_IsDeleted
                            //                                            && !crs.RequirementPackageSubscription.RPS_IsDeleted
                            //                                            && crs.RequirementPackageSubscription.RequirementPackage.RP_ID == newPkgId
                            //                                            && !crs.CRS_IsDeleted
                            //                                            && crs.RequirementPackageSubscription.RPS_ApplicantOrgUserID == con.RPS_ApplicantOrgUserID
                            //                                                ).OrderByDescending(col => col.RequirementPackageSubscription.RPS_ID)
                            //                                                .Select(crs => crs.RequirementPackageSubscription).FirstOrDefault();

                            //            if (existingSubscription.IsNullOrEmpty())
                            //            {
                            //Add records in RequirementPackageSubscription table
                            RequirementPackageSubscription requirementPackageSubscription = new RequirementPackageSubscription();
                            requirementPackageSubscription.RPS_RequirementPackageID = clinicalRotationRequirementPackage.CRRP_RequirementPackageID;
                            requirementPackageSubscription.RPS_RequirementSubscriptionTypeID = rotationSubscriptionTypeID;
                            requirementPackageSubscription.RPS_ApplicantOrgUserID = con.RPS_ApplicantOrgUserID;
                            requirementPackageSubscription.RPS_RequirementPackageStatusID = requirementNotCompliantPackStatusID;
                            requirementPackageSubscription.RPS_IsDeleted = false;
                            requirementPackageSubscription.RPS_CreatedByID = currentLoggedInUserId;
                            requirementPackageSubscription.RPS_CreatedOn = DateTime.Now;

                            //Add records in ClinicalRotationSubscription table
                            ClinicalRotationSubscription clinicalRotationSubscription = new ClinicalRotationSubscription();
                            clinicalRotationSubscription.CRS_ClinicalRotationRequirementPackageID = clinicalRotationRequirementPackage.CRRP_ID;
                            clinicalRotationSubscription.CRS_IsDeleted = false;
                            clinicalRotationSubscription.CRS_CreatedByID = currentLoggedInUserId;
                            clinicalRotationSubscription.CRS_CreatedOn = DateTime.Now;

                            requirementPackageSubscription.ClinicalRotationSubscriptions.Add(clinicalRotationSubscription);
                            _dbContext.RequirementPackageSubscriptions.AddObject(requirementPackageSubscription);

                            lstRequirementPackageSubscriptionToBeAdded.Add(requirementPackageSubscription);
                            lstClinicalRotationSubscription.Where(x => con.RPS_ID == x.CRS_RequirementPackageSubscriptionID).ForEach(y =>
                            {
                                y.CRS_ModifiedByID = currentLoggedInUserId;
                                y.CRS_ModifiedOn = DateTime.Now;
                                y.CRS_IsDeleted = true;
                            });

                            //Below Code is commented in UAT-4960
                            //            }
                            //            else
                            //            {
                            //                //con.RPS_RequirementPackageID = newPkgId;
                            //                //con.RPS_ModifiedByID = currentLoggedInUserId;
                            //                //con.RPS_ModifiedOn = DateTime.Now;

                            //                lstClinicalRotationSubscription.Where(x => con.RPS_ID == x.CRS_RequirementPackageSubscriptionID).ForEach(y =>
                            //                {
                            //                    y.CRS_ModifiedByID = currentLoggedInUserId;
                            //                    y.CRS_ModifiedOn = DateTime.Now;
                            //                    y.CRS_RequirementPackageSubscriptionID = existingSubscription.RPS_ID;
                            //                });
                            //            }


                            var anotherClinicalReqSubscriptionsCount = _dbContext.ClinicalRotationSubscriptions
                                   .Where(cond => cond.CRS_RequirementPackageSubscriptionID == con.RPS_ID
                                           && cond.CRS_ClinicalRotationRequirementPackageID != clinicalRotationRequirementPackage.CRRP_ID
                                           && !cond.CRS_IsDeleted).Count();

                            if (anotherClinicalReqSubscriptionsCount == 0)
                            {
                                con.RPS_IsDeleted = true;
                                con.RPS_ModifiedByID = currentLoggedInUserId;
                                con.RPS_ModifiedOn = DateTime.Now;
                            }


                        });
                        if (_dbContext.SaveChanges() > AppConsts.NONE)
                        {
                            String packageSubscriptionIdsXML = "<PackageSubscriptionIDs>";
                            foreach (RequirementPackageSubscription requirementPackageSubscription in lstRequirementPackageSubscriptionToBeAdded)
                            {
                                packageSubscriptionIdsXML += "<ID>" + requirementPackageSubscription.RPS_ID + "</ID>";
                            }
                            packageSubscriptionIdsXML += "</PackageSubscriptionIDs>";
                            CreateOptionalCategorySetAproved(packageSubscriptionIdsXML, currentLoggedInUserId);
                            //UAT-2603
                            List<Int32> lstreqPkgSubscriptionIds = lstRequirementPackageSubscriptionToBeAdded.Select(sel => sel.RPS_ID).ToList();
                            AddDataToRotDataMovement(lstreqPkgSubscriptionIds, currentLoggedInUserId, dataMovementDueStatusId);
                        }
                    }
                }
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
        private Boolean RemoveRotationSubscriptionForClientContacts(ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentUserId, List<Int32> contactsToBeDeleted, Int16 dataMovementDueStatusId, Int16 dataMovementNotRequiredStatusId)
        {
            Boolean ifAnySharedCtctUpdated = false;
            List<Entity.SharedDataEntity.ClientContactProfileSynchronization> lstAlreadySyncContacts = SharedDataDBContext.ClientContactProfileSynchronizations.
                                                                                                             Where(cond => contactsToBeDeleted.Contains(cond.CCPS_ClientContactID)
                                                                                                             && cond.CCPS_IsProfileSynched && cond.CCPS_OrgUserID != null
                                                                                                             && !cond.CCPS_IsDeleted).ToList();

            List<Entity.SharedDataEntity.ClinicalRotationClientContactMapping> lstMappedContacts = SharedDataDBContext.ClinicalRotationClientContactMappings.
                                                                                                             Where(cond => contactsToBeDeleted.Contains(cond.CRCCM_ClientContactID)
                                                                                                                 && cond.CRCCM_ClinicalRotationID == clinicalRotationDetailContract.RotationID
                                                                                                             && !cond.CRCCM_IsDeleted).ToList();
            if (lstMappedContacts.Count > AppConsts.NONE)
            {
                foreach (Entity.SharedDataEntity.ClinicalRotationClientContactMapping cntcts in lstMappedContacts)
                {
                    cntcts.CRCCM_IsDeleted = true;
                    cntcts.CRCCM_ModifiedByID = currentUserId;
                    cntcts.CRCCM_ModifiedOn = DateTime.Now;
                }
                List<Int32> orgUserIds = lstAlreadySyncContacts.Select(cond => cond.CCPS_OrgUserID.Value).ToList();
                String reqPkgTypeCode = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
                Int32 reqPkgTypeId = _dbContext.lkpRequirementPackageTypes.Where(x => !x.RPT_IsDeleted
                                                                                        && x.RPT_Code == reqPkgTypeCode)
                                                                                           .FirstOrDefault().RPT_ID;
                List<Int32> lstReqPkgSubsIds = RemoveRequirementPackageSubscription(clinicalRotationDetailContract.RotationID, orgUserIds, currentUserId, reqPkgTypeId);
                //UAt-2603
                UpdateDataMovementStatus(lstReqPkgSubsIds, dataMovementDueStatusId, dataMovementNotRequiredStatusId, currentUserId);
                ifAnySharedCtctUpdated = true;
            }
            return ifAnySharedCtctUpdated;
        }

        Boolean IClinicalRotationRepository.SaveContextIntoDataBase()
        {
            if (_dbContext.SaveChanges() > 0)
            {

                return true;
            }
            return false;
        }

        Boolean IClinicalRotationRepository.IfAnyContactIsMappedToRotation(Int32 rotationId, Int32 tenantId)
        {
            return SharedDataDBContext.ClinicalRotationClientContactMappings.Any(cond => cond.CRCCM_ClinicalRotationID == rotationId
                                                                                      && cond.CRCCM_TenantID == tenantId && !cond.CRCCM_IsDeleted);

        }

        Boolean IClinicalRotationRepository.IfAnyContactHasEnteredDataForRotation(Int32 packageId, Int32 clinicalRotationID)
        {
            ClinicalRotationRequirementPackage clinicalRotationRequirementPackage = _dbContext.ClinicalRotationRequirementPackages.FirstOrDefault(x => x.CRRP_ClinicalRotationID == clinicalRotationID
                                                                                    && !x.CRRP_IsDeleted && x.CRRP_RequirementPackageID == packageId);
            if (clinicalRotationRequirementPackage.IsNotNull())
            {
                List<ClinicalRotationSubscription> lstClinicalRotationSubscription = GetClinicalRotationSubscriptionsByRotationReqPackId(clinicalRotationRequirementPackage.CRRP_ID);
                if (lstClinicalRotationSubscription.IsNotNull())
                {
                    List<Int32> lstRequirementPackageSubscriptionID = lstClinicalRotationSubscription.Select(x => x.CRS_RequirementPackageSubscriptionID).ToList();
                    return _dbContext.RequirementPackageSubscriptions.Any(con => lstRequirementPackageSubscriptionID.Contains(con.RPS_ID)
                                                                                                                  && con.ApplicantRequirementCategoryDatas.Where(arcd => !arcd.ARCD_IsDeleted).Count() > AppConsts.NONE
                                                                                                                  && !con.RPS_IsDeleted);


                }

            }
            return false;
        }


        List<Entity.SharedDataEntity.ClinicalRotationClientContactMapping> IClinicalRotationRepository.lstRotationMappedToContacts(Int32 contactId, Int32 tenantId)
        {
            return SharedDataDBContext.ClinicalRotationClientContactMappings.Where(cond => cond.CRCCM_ClientContactID == contactId
                                                                                      && cond.CRCCM_TenantID == tenantId && !cond.CRCCM_IsDeleted).ToList();

        }

        /// <summary>
        /// Method to Synchronize ClientContact Profiles
        /// </summary>
        /// <param name="clinicalRotationDetailContract"></param>
        public List<ClientContactContract> SynchronizeClientContactProfiles(ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentUserId, Int32 profileSynchSourceTypeID, Int32 tenantID)
        {
            #region UAT-1361 ClientContact Profile Synchronization
            String sourceOrgUserIDs = String.Empty;
            if (clinicalRotationDetailContract.IsNotNull())
            {
                List<Int32> allClientContactIDs = clinicalRotationDetailContract.ContactIdList.Split(',').Select(int.Parse).ToList();
                List<Int32> lstSourceOrgUserIDs = GetSourceOrgUserIDs(allClientContactIDs);
                sourceOrgUserIDs = String.Join(",", lstSourceOrgUserIDs);
            }
            else
            {
                sourceOrgUserIDs = Convert.ToString(currentUserId);
            }

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@SourceOrgUserIDs",sourceOrgUserIDs),
                             new SqlParameter("@TenantID",tenantID)
                        };

                base.OpenSQLDataReaderConnection(con);
                List<ClientContactContract> lstClientContactContract = new List<ClientContactContract>();
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_SynchroniseClientContactProfile", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClientContactContract clientContactContract = new ClientContactContract();
                            clientContactContract.TenantID = dr["TenantID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TenantID"]);
                            clientContactContract.UserID = dr["UserID"].GetType().Name == "DBNull" ? new Guid() : (Guid)(dr["UserID"]);
                            clientContactContract.OrgUserId = dr["OrganizationUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OrganizationUserID"]);
                            lstClientContactContract.Add(clientContactContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);

                List<Entity.SharedDataEntity.ClientContact> lstClientContact = SharedDataDBContext.ClientContacts.Where(x => x.CC_UserID != null && !x.CC_IsDeleted).ToList();
                List<Int32> lstclientContactID = new List<int>();
                foreach (var cntct in lstClientContactContract)
                {
                    Int32 clientContactID = lstClientContact.Where(cond => cond.CC_UserID == cntct.UserID && cond.CC_TenantID == cntct.TenantID)
                                                             .Select(col => col.CC_ID)
                                                             .FirstOrDefault();
                    cntct.ClientContactID = clientContactID;
                    lstclientContactID.Add(clientContactID);
                }


                List<Entity.SharedDataEntity.ClientContactProfileSynchronization> lstCCPS = SharedDataDBContext.ClientContactProfileSynchronizations
                                                                  .Where(cond => lstclientContactID.Contains(cond.CCPS_ClientContactID) && !cond.CCPS_IsDeleted).ToList();
                foreach (Entity.SharedDataEntity.ClientContactProfileSynchronization ccps in lstCCPS)
                {
                    ccps.CCPS_OrgUserID = lstClientContactContract.FirstOrDefault(cond => cond.ClientContactID == ccps.CCPS_ClientContactID).OrgUserId;
                    ccps.CCPS_IsProfileSynched = true;
                    ccps.CCPS_ModifiedByID = currentUserId;
                    ccps.CCPS_ModifiedOn = DateTime.Now;
                    ccps.CCPS_ProfileSynchSourceTypeID = profileSynchSourceTypeID;
                }
                SharedDataDBContext.SaveChanges();
                return lstClientContactContract;
                //var dtMergedOrganizationUser = new DataTable();
                //dtMergedOrganizationUser.Load(dr);
                // List<Entity.SharedDataEntity.ClientContact> lstClientContact = SharedDataDBContext.ClientContacts.Where(x => x.CC_UserID != null && !x.CC_IsDeleted).ToList();

                //foreach (DataRow row in dtMergedOrganizationUser.Rows)
                //{
                //    //Int32 clientContactID = lstClientContact.Where(cond => cond.CC_UserID == (Guid)row["UserID"] && cond.CC_TenantID == (Int32)row["TenantID"])
                //    //                                         .Select(col => col.CC_ID)
                //    //                                         .FirstOrDefault();
                //    //Entity.SharedDataEntity.ClientContactProfileSynchronization ccps = SharedDataDBContext.ClientContactProfileSynchronizations
                //    //                                            .Where(cond => cond.CCPS_ClientContactID == clientContactID && !cond.CCPS_IsDeleted).FirstOrDefault();

                //    //if (!ccps.IsNullOrEmpty())
                //    //{
                //    //    ccps.CCPS_OrgUserID = (Int32)row["OrganizationUserID"];
                //    //    ccps.CCPS_IsProfileSynched = true;
                //    //    ccps.CCPS_ModifiedByID = currentUserId;
                //    //    ccps.CCPS_ModifiedOn = DateTime.Now;
                //    //    ccps.CCPS_ProfileSynchSourceTypeID = profileSynchSourceTypeID;
                //    //}
                //}
                //SharedDataDBContext.SaveChanges();
            }

            #endregion
        }

        /// <summary>
        /// Method to get OrganizationUserID list for which client contact profile synching have to be done
        /// </summary>
        /// <param name="allClientContactIDs"></param>
        /// <returns></returns>
        private List<Int32> GetSourceOrgUserIDs(List<Int32> allClientContactIDs)
        {
            List<Guid?> lstUserID = base.SharedDataDBContext.ClientContactProfileSynchronizations.Where(cond => allClientContactIDs.Contains(cond.CCPS_ClientContactID)
                && cond.CCPS_IsProfileSynched == false
                && !cond.CCPS_IsDeleted
                && cond.CCPS_OrgUserID == null
                && cond.ClientContact.CC_UserID != null).Select(col => col.ClientContact.CC_UserID).ToList(); //&& cond.CCPS_OrgUserID.HasValue

            SecurityRepository securityRepo = new SecurityRepository();
            return securityRepo.GetOrganizationUserIdsByUserIds(lstUserID);

        }

        #region UAT-1362:As an Instructor/Preceptor I should be able to enter data for my rotation requirements package

        public Int32 GetRequirementSubscriptionIdByClinicalRotID(Int32 clinicalRotationID, Int32 rotReqSubTypeID, Int32 inscPrecptorPkgID, Int32 orgUserId)
        {

            Int32 reqPackageSubID = AppConsts.NONE;
            var lstClinicalRotSubscription = _dbContext.ClinicalRotationRequirementPackages.Where(x => x.CRRP_ClinicalRotationID == clinicalRotationID
                                                                                 && !x.CRRP_IsDeleted && x.RequirementPackage.RP_RequirementPackageTypeID == inscPrecptorPkgID
                                                                                 && !x.RequirementPackage.RP_IsDeleted).Select(slct => slct.ClinicalRotationSubscriptions).ToList();
            if (!lstClinicalRotSubscription.IsNullOrEmpty())
            {
                foreach (var clRotSub in lstClinicalRotSubscription)
                {
                    var rePkgSub = clRotSub.Where(x => !x.CRS_IsDeleted && x.RequirementPackageSubscription.RPS_IsDeleted == false
                                                        && x.RequirementPackageSubscription.RPS_ApplicantOrgUserID == orgUserId
                                                        && x.RequirementPackageSubscription.RPS_RequirementSubscriptionTypeID == rotReqSubTypeID).FirstOrDefault();
                    if (!rePkgSub.IsNullOrEmpty())
                    {
                        reqPackageSubID = rePkgSub.RequirementPackageSubscription.RPS_ID;
                        break;
                    }
                }
            }
            return reqPackageSubID;
        }
        #endregion

        /// <summary>
        /// Check whether the selected Agency is associated with any clinical rotation in any Tenant 
        /// </summary>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        Boolean IClinicalRotationRepository.IsAgencyAssociated(int agencyId)
        {
            return _dbContext.ClinicalRotationAgencies.Where(cra => cra.CRA_AgencyID == agencyId && !cra.CRA_IsDeleted && !cra.ClinicalRotation.CR_IsDeleted).Any();
        }

        #region UAT-1405:should be a way to search all students and/ or instructors that were in a rotation during a given date range
        List<ApplicantDocumentContract> IClinicalRotationRepository.GetApplicantDocumentToExport(String appRotationXMl)
        {
            List<ApplicantDocumentContract> appDocDataList = new List<ApplicantDocumentContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@xmldata", appRotationXMl)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetApplicantRequirementDocumentToExport", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ApplicantDocumentContract applicantDocData = new ApplicantDocumentContract();
                            applicantDocData.DocumentPath = dr["DocumentPath"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DocumentPath"]);
                            applicantDocData.FileName = dr["FileName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FileName"]);
                            applicantDocData.ApplicantName = dr["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                            applicantDocData.ApplicantDocumentId = dr["ApplicantDocumentID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantDocumentID"]);
                            applicantDocData.ApplicantId = dr["ApplicantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantID"]);
                            appDocDataList.Add(applicantDocData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return appDocDataList;
        }

        Boolean IClinicalRotationRepository.IsAgenycHierarchyAvailable(String agencyHierarchyIds)
        {
            Dictionary<Int32, Boolean> dic = new Dictionary<Int32, Boolean>();
            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@agencyHierarchyIds", agencyHierarchyIds)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyNodeAvailablity", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            dic.Add(Convert.ToInt32(dr["HierarchyID"]), Convert.ToBoolean(dr["IsAvailable"]));
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            if (dic.ContainsValue(false))
                return false;
            return true;
        }

        List<RotationMemberSearchDetailContract> IClinicalRotationRepository.GetRotationMemberSearchData(RotationMemberSearchDetailContract clinicalRotationDetailContract
                                                                                , CustomPagingArgsContract customPagingArgsContract)
        {
            List<RotationMemberSearchDetailContract> rotationMemberSearchDataList = new List<RotationMemberSearchDetailContract>();
            String orderBy = "RotationID";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {

                            new SqlParameter("@AgencyID", (clinicalRotationDetailContract.AgencyID == AppConsts.NONE?null:clinicalRotationDetailContract.AgencyID)),
                            new SqlParameter("@SelectedUserGroupID", (clinicalRotationDetailContract.SelectedUserGroupID == AppConsts.NONE?(Int32?)null:clinicalRotationDetailContract.SelectedUserGroupID)),
                            new SqlParameter("@FirstName", clinicalRotationDetailContract.FirstName),
                            new SqlParameter("@LastName", clinicalRotationDetailContract.LastName),
                            new SqlParameter("@ComplioID", clinicalRotationDetailContract.ComplioID),
                            new SqlParameter("@RotationName", clinicalRotationDetailContract.RotationName),
                            new SqlParameter("@Department", clinicalRotationDetailContract.Department),
                            new SqlParameter("@Program", clinicalRotationDetailContract.Program),
                            new SqlParameter("@Course", clinicalRotationDetailContract.Course),
                            new SqlParameter("@Term", clinicalRotationDetailContract.Term),
                            new SqlParameter("@UnitFloorLoc", clinicalRotationDetailContract.UnitFloorLoc),
                            new SqlParameter("@NoOfHours", clinicalRotationDetailContract.RecommendedHours),
                            //UAT-1769
                            new SqlParameter("@NoOfStudents",clinicalRotationDetailContract.Students),
                            new SqlParameter("@RotationShift", clinicalRotationDetailContract.Shift),
                            new SqlParameter("@StartTime", clinicalRotationDetailContract.StartTime),
                            new SqlParameter("@EndTime", clinicalRotationDetailContract.EndTime),
                            new SqlParameter("@StartDate", clinicalRotationDetailContract.StartDate),
                            new SqlParameter("@EndDate", clinicalRotationDetailContract.EndDate),
                            new SqlParameter("@DaysList", clinicalRotationDetailContract.DaysIdList),
                            new SqlParameter("@ContactList", clinicalRotationDetailContract.ContactIdList),
                            new SqlParameter("@TypeSpecialty", clinicalRotationDetailContract.TypeSpecialty),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize),
                            new SqlParameter("@ArchieveStatusId", clinicalRotationDetailContract.ArchieveStatusId),
                            new SqlParameter("@LoggedInOrgUserID", clinicalRotationDetailContract.CurrentLoggedInClientUserID),//UAT-3549
                            //UAT-3749
                            new SqlParameter("@SelectedUserTypeCode",clinicalRotationDetailContract.SelectedUserTypeCode)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationMemberSearchData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RotationMemberSearchDetailContract rotationMemberSearchData = new RotationMemberSearchDetailContract();

                            rotationMemberSearchData.RotationID = Convert.ToInt32(dr["RotationID"]);
                            rotationMemberSearchData.AgencyID = dr["AgencyID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyID"]);
                            rotationMemberSearchData.OrganizationUserID = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                            rotationMemberSearchData.ComplioID = dr["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplioID"]);
                            rotationMemberSearchData.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            rotationMemberSearchData.FirstName = dr["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FirstName"]);
                            rotationMemberSearchData.LastName = dr["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["LastName"]);
                            rotationMemberSearchData.RotationName = dr["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationName"]);
                            rotationMemberSearchData.Department = dr["Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Department"]);
                            rotationMemberSearchData.Program = dr["Program"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Program"]);
                            rotationMemberSearchData.Course = dr["Course"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Course"]);
                            rotationMemberSearchData.UnitFloorLoc = dr["UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UnitFloorLoc"]);
                            rotationMemberSearchData.Shift = dr["RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationShift"]);
                            rotationMemberSearchData.RecommendedHours = dr["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfHours"]));
                            rotationMemberSearchData.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            rotationMemberSearchData.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            rotationMemberSearchData.StartTime = dr["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["StartTime"]);
                            rotationMemberSearchData.EndTime = dr["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["EndTime"]);
                            rotationMemberSearchData.Time = dr["Times"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Times"]);
                            rotationMemberSearchData.DaysIdList = dr["DaysList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DaysList"]);
                            rotationMemberSearchData.DaysName = dr["DaysName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DaysName"]);
                            rotationMemberSearchData.ContactIdList = dr["ContactList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContactList"]);
                            rotationMemberSearchData.ContactNames = dr["ContactName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContactName"]);
                            rotationMemberSearchData.IsPackageExistsInRotation = dr["IsPackageExistsInRotation"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsPackageExistsInRotation"]);
                            rotationMemberSearchData.SyllabusFileName = dr["SyllabusFileName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SyllabusFileName"]);
                            rotationMemberSearchData.SyllabusFilePath = dr["SyllabusFilePath"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SyllabusFilePath"]);
                            rotationMemberSearchData.TotalRecordCount = dr["TotalCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TotalCount"]);
                            rotationMemberSearchData.IsApplicant = dr["IsApplicant"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsApplicant"]);
                            rotationMemberSearchData.Term = dr["Term"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Term"]);
                            rotationMemberSearchData.TypeSpecialty = dr["TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TypeSpecialty"]);
                            //UAT-1769
                            rotationMemberSearchData.Students = dr["NoOfStudents"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfStudents"]));
                            rotationMemberSearchData.UserGroup = dr["UserGroup"] == DBNull.Value ? String.Empty : Convert.ToString((dr["UserGroup"]));
                            //UAT-3749
                            rotationMemberSearchData.UserType = dr["UserType"] == DBNull.Value ? String.Empty : Convert.ToString((dr["UserType"]));
                            rotationMemberSearchData.AgencyComplianceStatus = dr["AgencyComplianceStatus"] == DBNull.Value ? String.Empty : Convert.ToString((dr["AgencyComplianceStatus"]));
                            rotationMemberSearchDataList.Add(rotationMemberSearchData);
                        }
                    }
                }

                return rotationMemberSearchDataList;
            }
        }

        public Int32 GetSubscriptionIdByRotIDAndUserID(Int32 clinicalRotationID, Int32 reSubscriptionPkgTypeId, Int32 rotReqSubTypeID, Int32 orgUserId)
        {

            Int32 reqPackageSubID = AppConsts.NONE;
            var lstClinicalRotSubscription = _dbContext.ClinicalRotationRequirementPackages.Where(x => x.CRRP_ClinicalRotationID == clinicalRotationID
                                                                                 && !x.CRRP_IsDeleted).Select(slct => slct.ClinicalRotationSubscriptions).ToList();
            if (!lstClinicalRotSubscription.IsNullOrEmpty())
            {
                foreach (var clRotSub in lstClinicalRotSubscription)
                {
                    var rePkgSub = clRotSub.Where(x => !x.CRS_IsDeleted && x.RequirementPackageSubscription.RPS_IsDeleted == false
                                                        && x.RequirementPackageSubscription.RPS_ApplicantOrgUserID == orgUserId
                                                        && x.RequirementPackageSubscription.RequirementPackage.RP_RequirementPackageTypeID == reSubscriptionPkgTypeId
                                                        && x.RequirementPackageSubscription.RPS_RequirementSubscriptionTypeID == rotReqSubTypeID).FirstOrDefault();
                    if (!rePkgSub.IsNullOrEmpty())
                    {
                        reqPackageSubID = rePkgSub.RequirementPackageSubscription.RPS_ID;
                        break;
                    }
                }
            }
            return reqPackageSubID;
        }

        #endregion


        /// <summary>
        /// Get clinical rotation member by clinical rotation ID
        /// </summary>
        /// <param name="clinicalRotationID"></param>
        /// <returns></returns>
        IEnumerable<ClinicalRotationMember> IClinicalRotationRepository.GetRotationMemberListByRotationId(Int32 clinicalRotationID)
        {
            return _dbContext.ClinicalRotationMembers.Where(x => x.CRM_ClinicalRotationID == clinicalRotationID && !x.CRM_IsDeleted);
        }

        #region UAT 1409 : The Agency User should be able to filter their search by those rotations that are active or completed (expired) AND by pending revieew or reviewed.
        Boolean IClinicalRotationRepository.SaveUpdateRotationReviewStatus(List<SharedUserRotationReview> lstSharedUserRotationReview, Boolean isUpdateMode)
        {
            if (!isUpdateMode)
            {
                //Insert mode
                foreach (SharedUserRotationReview sharedUserRotationReview in lstSharedUserRotationReview)
                {
                    _dbContext.AddToSharedUserRotationReviews(sharedUserRotationReview);
                }
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        List<SharedUserRotationReview> IClinicalRotationRepository.GetShareduserRotationReviewStatusByIds(List<Int32> lstSharedUserRotationReviewIDs)
        {
            return _dbContext.SharedUserRotationReviews.Where(cond => lstSharedUserRotationReviewIDs.Contains(cond.SURR_ID) && !cond.SURR_IsDeleted).ToList();
        }

        #endregion


        #region UAT-1414:Notification to go out prior to student's start date for clinical rotation.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subEventId"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        List<ClinicalRotationMemberDetail> IClinicalRotationRepository.GetRotationMemberDetailForNagMail(Int32 subEventId, Int32 chunkSize)
        {
            List<ClinicalRotationMemberDetail> lstClinicalRotationMemberDetail = new List<ClinicalRotationMemberDetail>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@chunkSize", chunkSize),
                             new SqlParameter("@eventId", subEventId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationMemberDetailForNagMail", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationMemberDetail clinicalRotDetail = new ClinicalRotationMemberDetail();

                            clinicalRotDetail.RotationID = Convert.ToInt32(dr["CR_ID"]);
                            clinicalRotDetail.RotationName = dr["CR_RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_RotationName"]);
                            clinicalRotDetail.Department = dr["CR_Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_Department"]);
                            clinicalRotDetail.Program = dr["CR_Program"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_Program"]);
                            clinicalRotDetail.Course = dr["CR_Course"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_Course"]);
                            clinicalRotDetail.UnitFloorLoc = dr["CR_UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_UnitFloorLoc"]);
                            clinicalRotDetail.Shift = dr["CR_RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_RotationShift"]);
                            clinicalRotDetail.RecommendedHours = dr["CR_NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["CR_NoOfHours"]));
                            clinicalRotDetail.StartDate = dr["CR_StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CR_StartDate"]);
                            clinicalRotDetail.EndDate = dr["CR_EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CR_EndDate"]);
                            clinicalRotDetail.OrganizationUserId = Convert.ToInt32(dr["CRM_ApplicantOrgUserID"]);
                            clinicalRotDetail.ApplicantName = dr["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                            clinicalRotDetail.PrimaryEmailaddress = dr["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PrimaryEmailAddress"]);
                            clinicalRotDetail.ComplioID = dr["CR_ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_ComplioID"]);
                            clinicalRotDetail.Term = dr["CR_Term"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_Term"]);
                            clinicalRotDetail.Time = dr["CR_StartTime"] == DBNull.Value ? String.Empty : String.Format("{0}-{1}", dr["CR_StartTime"], dr["CR_EndTime"]);
                            clinicalRotDetail.DaysName = dr["DaysName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DaysName"]);
                            clinicalRotDetail.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            clinicalRotDetail.TypeSpecialty = dr["CR_TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_TypeSpecialty"]);
                            lstClinicalRotationMemberDetail.Add(clinicalRotDetail);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
                #region UAT-3254
                lstClinicalRotationMemberDetail.ForEach(x =>
                {
                    List<Int32> lstHierarchyNodes = _dbContext.ClinicalRotationHierarchyMappings.Where(cond => cond.CRHM_ClinicalRotationID == x.RotationID && !cond.CRHM_IsDeleted).Select(sel => sel.CRHM_HierarchyNodeID).ToList();
                    if (!lstHierarchyNodes.IsNullOrEmpty())
                    {
                        x.RotationHirarchyIds = String.Join(",", lstHierarchyNodes);
                    }
                });
                #endregion
                return lstClinicalRotationMemberDetail;
            }
        }
        #endregion

        #region UAT 1799 : Rotations should not be shared if student(s) are not compliant for rotation package requirements
        /// <summary>
        /// Returns boolean value 0 if Rotation should not be shared if student(s) are not compliant for rotation package requirements.
        /// </summary>
        /// <param name="RotationID"></pa
        /// <returns>list of ClientSystemDocuments</returns>
        Boolean IClinicalRotationRepository.GetRequirementPackageStatusByRotationID(Int32 RotationID, String RotationMemberIds)
        {
            // BkgOrderDetailCustomFormDataContract customFormDataContract = null;
            Boolean RotationEligibilityForSharing = false;
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetRequirementPackageStatusByRotationID", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RotationID", RotationID);
                command.Parameters.AddWithValue("@SelectedRotaionMembers", RotationMemberIds);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    // customFormDataContract = new BkgOrderDetailCustomFormDataContract();
                    //customFormDataContract.lstCustomFormAttributes = SetAttributesForCustomForm(ds.Tables[0]);
                    RotationEligibilityForSharing = Convert.ToBoolean(ds.Tables[0].Rows[0]["RotationEligibilityForSharing"]);
                }
            }
            return RotationEligibilityForSharing;
        }
        #endregion

        #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        Boolean IClinicalRotationRepository.SaveUpdateUserRotationReviewStatus(int RotationID, Int32 currentLoggedInUserId, Int32 inviteeOrgUserId, Int32 reviewStatusId, Int32 agencyID, Int32? lastReviewedByID, Boolean isRotationReviewStatusUpdatingWhileRS, Int32 approvedReviewStatusID, Boolean isAdminLoggedInAsAgencyUser = false)
        {
            SharedUserRotationReview sharedUserRotationReviewData = _dbContext.SharedUserRotationReviews
                                                                        .FirstOrDefault(cnd => cnd.SURR_ClinicalRotaionID == RotationID
                                                                                            && cnd.SURR_OrganizationUserID == inviteeOrgUserId
                                                                                            && cnd.SURR_AgencyID == agencyID
                                                        && !cnd.SURR_IsDeleted);


            //Int32 notDroppedApplicantCount = _dbContext.ClinicalRotationMembers.Count(cond => cond.CRM_ClinicalRotationID == RotationID
            //                                                                                        && !cond.CRM_IsDropped
            //                                                                                        && !cond.CRM_IsDeleted);

            var rotationMembers = _dbContext.ClinicalRotationMembers.Where(cond => cond.CRM_ClinicalRotationID == RotationID
                                                                                                    && !cond.CRM_IsDeleted).ToList();

            var clinicalRotation = _dbContext.ClinicalRotations.Where(cond => cond.CR_ID == RotationID
                                                                                   && !cond.CR_IsDeleted
                                                                       ).FirstOrDefault();


            if (!clinicalRotation.IsNullOrEmpty() && !rotationMembers.IsNullOrEmpty())
            {
                //if (notDroppedApplicantCount == AppConsts.NONE)
                //    clinicalRotation.CR_IsDropped = true;
                //else
                //    clinicalRotation.CR_IsDropped = false;

                if (rotationMembers.Count(cnd => !cnd.CRM_IsDropped) == AppConsts.NONE)
                {
                    clinicalRotation.CR_IsDropped = true;
                    clinicalRotation.CR_DroppedOn = DateTime.Now;
                    //UAT-4460
                    String invitationReviewDroppedCode = SharedUserInvitationReviewStatus.Dropped.GetStringValue();
                    reviewStatusId = SharedDataDBContext.lkpSharedUserInvitationReviewStatus.Where(cond2 => !cond2.SUIRS_IsDeleted && cond2.SUIRS_Code == invitationReviewDroppedCode).Select(sel => sel.SUIRS_ID).FirstOrDefault();
                }
                else
                    clinicalRotation.CR_IsDropped = false;

                clinicalRotation.CR_ModifiedByID = currentLoggedInUserId;
                clinicalRotation.CR_ModifiedOn = DateTime.Now;
            }

            if (sharedUserRotationReviewData.IsNullOrEmpty())
            {

                SharedUserRotationReview sharedUserRotationReview = new SharedUserRotationReview();
                sharedUserRotationReview.SURR_ClinicalRotaionID = RotationID;
                sharedUserRotationReview.SURR_OrganizationUserID = inviteeOrgUserId;
                sharedUserRotationReview.SURR_RotationReviewStatusID = reviewStatusId;
                sharedUserRotationReview.SURR_AgencyID = agencyID;
                sharedUserRotationReview.SURR_IsDeleted = false;
                sharedUserRotationReview.SURR_CreatedOn = DateTime.Now;
                sharedUserRotationReview.SURR_CreatedByID = currentLoggedInUserId;

                if (inviteeOrgUserId == currentLoggedInUserId)
                    sharedUserRotationReview.SURR_ReviewByID = inviteeOrgUserId;

                if (isRotationReviewStatusUpdatingWhileRS)
                    sharedUserRotationReview.SURR_ReviewByID = lastReviewedByID;

                _dbContext.SharedUserRotationReviews.AddObject(sharedUserRotationReview);
            }
            else
            {
                sharedUserRotationReviewData.SURR_RotationReviewStatusID = reviewStatusId;
                sharedUserRotationReviewData.SURR_ModifiedOn = DateTime.Now;
                sharedUserRotationReviewData.SURR_ModifiedByID = currentLoggedInUserId;
                if (isAdminLoggedInAsAgencyUser || inviteeOrgUserId == currentLoggedInUserId)
                    sharedUserRotationReviewData.SURR_ReviewByID = inviteeOrgUserId;

                if (isRotationReviewStatusUpdatingWhileRS)
                    sharedUserRotationReviewData.SURR_ReviewByID = lastReviewedByID;
            }
            if (_dbContext.SaveChanges() > 0)
                return true;
            return false;
        }
        #endregion

        #region UAT-1843
        /// <summary>
        /// Fetch Rotation member details on the basis of Organization User ID
        /// </summary>
        /// <param name="orgUserIds"></param>
        /// <returns></returns>
        List<ClinicalRotationMemberDetail> IClinicalRotationRepository.GetRotationDetailsByOrgUserIds(String orgUserIds, Int32? clinicalRotationID)
        {
            List<ClinicalRotationMemberDetail> lstClinicalRotationMemberDetail = new List<ClinicalRotationMemberDetail>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@orgUserIds", orgUserIds),
                            new SqlParameter("@clinicalRotationID", clinicalRotationID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationDetailsByOrgUserIds", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationMemberDetail clinicalRotDetail = new ClinicalRotationMemberDetail();

                            clinicalRotDetail.RotationID = Convert.ToInt32(dr["CR_ID"]);
                            clinicalRotDetail.RotationName = dr["CR_RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_RotationName"]);
                            clinicalRotDetail.Department = dr["CR_Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_Department"]);
                            clinicalRotDetail.Program = dr["CR_Program"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_Program"]);
                            clinicalRotDetail.Course = dr["CR_Course"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_Course"]);
                            clinicalRotDetail.UnitFloorLoc = dr["CR_UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_UnitFloorLoc"]);
                            clinicalRotDetail.Shift = dr["CR_RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_RotationShift"]);
                            clinicalRotDetail.RecommendedHours = dr["CR_NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["CR_NoOfHours"]));
                            clinicalRotDetail.StartDate = dr["CR_StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CR_StartDate"]);
                            clinicalRotDetail.EndDate = dr["CR_EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CR_EndDate"]);
                            clinicalRotDetail.OrganizationUserId = Convert.ToInt32(dr["CRM_ApplicantOrgUserID"]);
                            clinicalRotDetail.ApplicantName = dr["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                            clinicalRotDetail.PrimaryEmailaddress = dr["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PrimaryEmailAddress"]);
                            clinicalRotDetail.ComplioID = dr["CR_ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_ComplioID"]);
                            clinicalRotDetail.Term = dr["CR_Term"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_Term"]);
                            clinicalRotDetail.Time = dr["CR_StartTime"] == DBNull.Value ? String.Empty : String.Format("{0}-{1}", dr["CR_StartTime"], dr["CR_EndTime"]);
                            clinicalRotDetail.DaysName = dr["DaysName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DaysName"]);
                            clinicalRotDetail.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            clinicalRotDetail.TypeSpecialty = dr["CR_TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CR_TypeSpecialty"]);
                            clinicalRotDetail.RotationMemberId = dr["CRM_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CRM_ID"]);
                            //UAT-2290 : 
                            clinicalRotDetail.DeadlineDate = dr["CR_DeadlineDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CR_DeadlineDate"]);
                            lstClinicalRotationMemberDetail.Add(clinicalRotDetail);
                        }
                    }
                }

                #region UAT-3254
                lstClinicalRotationMemberDetail.ForEach(x =>
                {
                    List<Int32> lstHierarchyNodes = _dbContext.ClinicalRotationHierarchyMappings.Where(cond => cond.CRHM_ClinicalRotationID == x.RotationID && !cond.CRHM_IsDeleted).Select(sel => sel.CRHM_HierarchyNodeID).ToList();
                    if (!lstHierarchyNodes.IsNullOrEmpty())
                    {
                        x.RotationHirarchyIds = String.Join(",", lstHierarchyNodes);
                    }
                });
                #endregion

                base.CloseSQLDataReaderConnection(con);
                return lstClinicalRotationMemberDetail;
            }
        }
        /// <summary>
        /// update clinicalRotationMember table if Nag mail is dielvered
        /// </summary>
        /// <param name="RotationMemberId"></param>
        /// <returns></returns>
        Boolean IClinicalRotationRepository.UpdateClinicalRotationMenberForNagMail(Int32 RotationMemberId, Int32 CurentLoggedInUserId)
        {
            ClinicalRotationMember rotationMemberDeatil = _dbContext.ClinicalRotationMembers.Where(cond => cond.CRM_ID == RotationMemberId && !cond.CRM_IsDeleted).FirstOrDefault();

            if (rotationMemberDeatil.IsNotNull())
            {
                rotationMemberDeatil.CRM_ModifiedByID = CurentLoggedInUserId;
                rotationMemberDeatil.CRM_ModifiedOn = DateTime.Now;
                rotationMemberDeatil.CRM_ScheduleNotificationDate = DateTime.Now;
            }

            if (_dbContext.SaveChanges() > 0)
                return true;
            return false;
        }
        #endregion

        #region UAT-2034:
        Tuple<Boolean, List<ClientContactContract>, List<RotationDetailFieldChanges>> IClinicalRotationRepository.SaveUpdateClinicalRotationAssignments(List<Int32> clinicalRotIDs, ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentUserId
                                                      , Int32 profileSynchSourceTypeID, String rotationAssignType, Int32 syllabusDocumentTypeID, Int32 packageID
                                                      , Int32 rotationSubscriptionTypeID, Int32 requirementNotCompliantPackStatusID, Int16 dataMovementDueStatusId)
        {
            List<ClinicalRotation> lstClinicalRotationDB = _dbContext.ClinicalRotations.Where(cnd => clinicalRotIDs.Contains(cnd.CR_ID) && !cnd.CR_IsDeleted).ToList();
            Boolean isDataSaved = false;
            List<ClientContactContract> lstClientContact = new List<ClientContactContract>();
            List<RotationDetailFieldChanges> lstRotationDetailFieldChanges = new List<RotationDetailFieldChanges>();//UAT-3180
            if (String.Compare(rotationAssignType, RotationAssignmentType.UPLOAD_SYLLABUS.GetStringValue(), true) == 0)
            {
                clinicalRotIDs.ForEach(rotationID =>
                {
                    String oldSyllabusDocumentName = String.Empty;//UAT-3108
                    ClinicalRotation rotationToBeUpdated = lstClinicalRotationDB.FirstOrDefault(x => x.CR_ID == rotationID && !x.CR_IsDeleted);
                    var existingRotationDocumentList = rotationToBeUpdated.ClinicalRotationDocuments.Where(cond => !cond.CRD_IsDeleted);
                    if (!existingRotationDocumentList.IsNullOrEmpty())
                    {
                        ClinicalRotationDocument existingClinicalRotationDocument = existingRotationDocumentList.FirstOrDefault(cond => !cond.CRD_IsDeleted);
                        existingClinicalRotationDocument.CRD_IsDeleted = true;
                        existingClinicalRotationDocument.CRD_ModifiedByID = currentUserId;
                        existingClinicalRotationDocument.CRD_ModifiedOn = DateTime.Now;

                        ClientSystemDocument existingClientSystemDocument = existingClinicalRotationDocument.ClientSystemDocument;
                        existingClientSystemDocument.CSD_IsDeleted = true;
                        existingClientSystemDocument.CSD_ModifiedByID = currentUserId;
                        existingClientSystemDocument.CSD_ModifiedOn = DateTime.Now;
                        oldSyllabusDocumentName = existingClientSystemDocument.CSD_FileName.IsNullOrEmpty() ? String.Empty : Convert.ToString(existingClientSystemDocument.CSD_FileName);//UAT-3108
                    }
                    if (!clinicalRotationDetailContract.SyllabusFileName.IsNullOrEmpty())
                    {
                        AddSyllabusDocument(clinicalRotationDetailContract, currentUserId, rotationID, syllabusDocumentTypeID);
                    }
                    //UAT-3108
                    lstRotationDetailFieldChanges.Add(GenerateDataForRotationSyllDocUpdation(rotationID, oldSyllabusDocumentName, clinicalRotationDetailContract.SyllabusFileName, currentUserId, clinicalRotationDetailContract.TenantID));

                });
                if (_dbContext.SaveChanges() > 0)
                    isDataSaved = true;
            }
            else if (String.Compare(rotationAssignType, RotationAssignmentType.ASSIGN_PRECEPTOR.GetStringValue(), true) == 0)
            {
                lstRotationDetailFieldChanges = GenerateDataForRotationAssignPreceptor(clinicalRotIDs, clinicalRotationDetailContract.ContactIdList, currentUserId, clinicalRotationDetailContract.TenantID);

                Tuple<Boolean, List<ClientContactContract>> tuple = BulkAssignmentOfRotationData(clinicalRotationDetailContract.TenantID, AppConsts.NONE, currentUserId, clinicalRotIDs
                                                                                                , RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue()
                                                                                                , rotationAssignType
                                                                                                , GenerateContactXML(clinicalRotIDs, clinicalRotationDetailContract));
                isDataSaved = tuple.Item1;
                lstClientContact = tuple.Item2;
                if (isDataSaved)
                {
                    List<Int32> lstIds = new List<Int32>();
                    lstIds = lstClientContact.Select(x => x.ClientContactID).ToList();
                    String concatedClientContactIDs = String.Join(",", lstIds);
                    clinicalRotationDetailContract.ContactIdList = concatedClientContactIDs;
                    if (!clinicalRotationDetailContract.ContactIdList.IsNullOrEmpty())
                    {
                        SynchronizeClientContactProfiles(clinicalRotationDetailContract, currentUserId, profileSynchSourceTypeID, clinicalRotationDetailContract.TenantID);
                        CreateBulkRotationSubscriptionForClientContact(lstIds, clinicalRotIDs, rotationSubscriptionTypeID, requirementNotCompliantPackStatusID, currentUserId, dataMovementDueStatusId);
                    }
                }
            }
            else if (String.Compare(rotationAssignType, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0)
            {
                Tuple<Boolean, List<ClientContactContract>> tuple = BulkAssignmentOfRotationData(clinicalRotationDetailContract.TenantID, packageID, currentUserId, clinicalRotIDs
                                                                                                 , RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue()
                                                                                                 , rotationAssignType, null);
                isDataSaved = tuple.Item1;
            }
            else if (String.Compare(rotationAssignType, RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue(), true) == 0)
            {
                Tuple<Boolean, List<ClientContactContract>> tuple = BulkAssignmentOfRotationData(clinicalRotationDetailContract.TenantID, packageID, currentUserId, clinicalRotIDs
                                                                                                 , RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue()
                                                                                                 , rotationAssignType, null);
                isDataSaved = tuple.Item1;
            }
            return new Tuple<Boolean, List<ClientContactContract>, List<RotationDetailFieldChanges>>(isDataSaved, lstClientContact, lstRotationDetailFieldChanges);
        }


        Tuple<Boolean, List<ClientContactContract>> BulkAssignmentOfRotationData(Int32 tenantId, Int32 packageId, Int32 currentLoggedInUserID, List<Int32> rotationIds, String packageType
                                                                                 , String actionType, String rotationContactXML = null)
        {
            String rotationIdConcat = String.Empty;
            Boolean isDataSaved = false;
            if (rotationIds.IsNotNull())
            {
                rotationIdConcat = String.Join(",", rotationIds);
            }
            List<ClientContactContract> lstClientContactContract = new List<ClientContactContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_BulkAssignmentOfPackageAndContactToRotation", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageID", packageId);
                command.Parameters.AddWithValue("@currentLoggedInUserId", currentLoggedInUserID);
                command.Parameters.AddWithValue("@RotationIDs", rotationIdConcat);
                command.Parameters.AddWithValue("@PackageType", packageType);
                command.Parameters.AddWithValue("@ActionType", actionType);
                command.Parameters.AddWithValue("@RotationContactXML", rotationContactXML);
                command.Parameters.AddWithValue("@TenantID", tenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        isDataSaved = Convert.ToBoolean(ds.Tables[0].Rows[0][0]);
                    }
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        IEnumerable<DataRow> rows = ds.Tables[1].AsEnumerable();
                        lstClientContactContract = rows.Select(col =>
                              new ClientContactContract
                              {
                                  ClientContactID = col["ClientContactID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(col["ClientContactID"]),
                                  Name = Convert.ToString(col["CC_Name"]),
                                  Email = Convert.ToString(col["CC_Email"]),
                                  ClientContactTypeID = Convert.ToInt32(col["CC_ClientContactTypeID"]),
                                  TokenID = Guid.Parse(Convert.ToString(col["CC_TokenID"])),
                              }).DistinctBy(dst => dst.ClientContactID).ToList();
                    }
                }
            }
            return new Tuple<Boolean, List<ClientContactContract>>(isDataSaved, lstClientContactContract);
        }

        private String GenerateContactXML(List<Int32> rotationIds, ClinicalRotationDetailContract clinicalRotationDetailContract)
        {
            String contactXML = String.Empty;
            if (!clinicalRotationDetailContract.IsNullOrEmpty() && !clinicalRotationDetailContract.ContactIdList.IsNullOrEmpty())
            {
                List<Int32> contactIds = new List<Int32>();
                contactIds = clinicalRotationDetailContract.ContactIdList.Split(',').Select(int.Parse).ToList();
                contactXML += "<RotationContactData>";
                rotationIds.ForEach(rotID =>
                {
                    contactIds.ForEach(contactID =>
                    {
                        contactXML += "<ContactData>";
                        contactXML += "<RotationID>" + rotID + "</RotationID>";
                        contactXML += "<ClientContactID>" + contactID + "</ClientContactID>";
                        contactXML += "</ContactData>";

                    });


                });
                contactXML += "</RotationContactData>";
            }
            return contactXML;
        }

        Boolean IClinicalRotationRepository.IsDataEnteredForAnyRotation(Int32 tenantId, String rotationIds, String packageType)
        {
            Boolean isDataEntered = false;
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_IsDataEnteredForAnyRotation", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RotationIDs", rotationIds);
                command.Parameters.AddWithValue("@PackageType", packageType);
                command.Parameters.AddWithValue("@TenantID", tenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        isDataEntered = Convert.ToInt32(ds.Tables[0].Rows[0]["RotCount"]) > AppConsts.NONE ? true : false;
                    }
                }
            }
            return isDataEntered;
        }

        Boolean IClinicalRotationRepository.IsPreceptorAssignedForAllRotations(List<Int32> rotationIds)
        {
            Boolean isPreceptorAssignedForAllRotations = false;
            List<Int32> clientContactNotMappedRotIDs = new List<Int32>();
            var clientContactMappedRotationIDs = _dbContext.ClinicalRotationClientContacts.Where(cnd => rotationIds.Contains(cnd.CRCC_ClinicalRotationID) && !cnd.CRCC_IsDeleted).Select(x => x.CRCC_ClinicalRotationID).Distinct().ToList();
            if (!clientContactMappedRotationIDs.IsNullOrEmpty())
            {
                clientContactNotMappedRotIDs = rotationIds.Except(clientContactMappedRotationIDs).ToList();
                if (clientContactNotMappedRotIDs.IsNullOrEmpty())
                {
                    isPreceptorAssignedForAllRotations = true;
                }
            }
            return isPreceptorAssignedForAllRotations;
        }
        List<InstructorAvailabilityContract> IClinicalRotationRepository.CheckInstAvailabilityByRotationIds(String rotationIds)
        {
            List<InstructorAvailabilityContract> lstInstructorAvailabilityDetails = new List<InstructorAvailabilityContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {   new SqlParameter("@RotationIds", rotationIds)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetInstAvailabilityByRotationIds", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            InstructorAvailabilityContract instructorAvailabilityDetails = new InstructorAvailabilityContract();
                            instructorAvailabilityDetails.InsAvailibility = dr["InsAvailibility"] == DBNull.Value ? false : Convert.ToBoolean(dr["InsAvailibility"]);
                            instructorAvailabilityDetails.RotationID = dr["RotationID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RotationID"]);
                            instructorAvailabilityDetails.IsSchoolSendingInstructor = dr["IsSchoolSendingInstructor"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsSchoolSendingInstructor"]);
                            instructorAvailabilityDetails.ComplioID = dr["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplioID"]);
                            lstInstructorAvailabilityDetails.Add(instructorAvailabilityDetails);
                        }
                    }
                }
            }
            return lstInstructorAvailabilityDetails;
        }

        public void CreateBulkRotationSubscriptionForClientContact(List<Int32> clientContactIds, List<Int32> clinicalRotationIDs, Int32 rotationSubscriptionTypeID,
                                                                   Int32 requirementNotCompliantPackStatusID, Int32 currentLoggedInUserId, Int16 dataMovementDueStatusId)
        {
            if (!clientContactIds.IsNullOrEmpty())
            {
                List<Entity.SharedDataEntity.ClientContactProfileSynchronization> lstAlreadySyncContacts = SharedDataDBContext.ClientContactProfileSynchronizations.Where(cond => clientContactIds.Contains(cond.CCPS_ClientContactID)
                                                                                                                 && cond.CCPS_OrgUserID != null
                                                                                                                 && !cond.CCPS_IsDeleted).ToList();
                List<ClinicalRotationRequirementPackage> clinicalRotationRequirementPackageList = _dbContext.ClinicalRotationRequirementPackages.Where(x => clinicalRotationIDs.Contains(x.CRRP_ClinicalRotationID)
                                                                                    && !x.CRRP_IsDeleted && x.RequirementPackage.lkpRequirementPackageType.RPT_Code == "AAAB").ToList();
                if (clinicalRotationRequirementPackageList.IsNotNull())
                {
                    List<RequirementPackageSubscription> lstRequirementPackageSubscriptionToBeAdded = new List<RequirementPackageSubscription>();
                    clinicalRotationRequirementPackageList.ForEach(CRRP =>
                    {
                        lstAlreadySyncContacts.ForEach(cond =>
                        {
                            //Changes for UAT-2040
                            //Commented in UAT-4960
                            //     var existingSubscription = _dbContext.ClinicalRotationSubscriptions
                            //.Where(crs =>
                            //    crs.ClinicalRotationRequirementPackage.RequirementPackage.RP_ID == CRRP.CRRP_RequirementPackageID
                            //    && !crs.ClinicalRotationRequirementPackage.CRRP_IsDeleted
                            //    && !crs.ClinicalRotationRequirementPackage.ClinicalRotation.CR_IsDeleted
                            //    && !crs.RequirementPackageSubscription.RPS_IsDeleted
                            //    && !crs.CRS_IsDeleted
                            //    && crs.RequirementPackageSubscription.RPS_ApplicantOrgUserID == cond.CCPS_OrgUserID.Value
                            //        ).OrderByDescending(col => col.RequirementPackageSubscription.RPS_ID)
                            //        .Select(crs => crs.RequirementPackageSubscription).FirstOrDefault();
                            //Commented in UAT-4960
                            //if (existingSubscription.IsNullOrEmpty())
                            //{

                            Boolean isSubscriptionExistForSameRotation = _dbContext.ClinicalRotationSubscriptions.Any(crs => !crs.CRS_IsDeleted && !crs.ClinicalRotationRequirementPackage.CRRP_IsDeleted
                             && crs.ClinicalRotationRequirementPackage.CRRP_ClinicalRotationID == CRRP.CRRP_ClinicalRotationID
                             && crs.ClinicalRotationRequirementPackage.CRRP_RequirementPackageID == CRRP.CRRP_RequirementPackageID
                             && !crs.RequirementPackageSubscription.RPS_IsDeleted && crs.RequirementPackageSubscription.RPS_ApplicantOrgUserID == cond.CCPS_OrgUserID.Value);

                            if (!isSubscriptionExistForSameRotation)
                            {
                                //Add records in RequirementPackageSubscription table
                                RequirementPackageSubscription requirementPackageSubscription = new RequirementPackageSubscription();
                                requirementPackageSubscription.RPS_RequirementPackageID = CRRP.CRRP_RequirementPackageID;
                                requirementPackageSubscription.RPS_RequirementSubscriptionTypeID = rotationSubscriptionTypeID;
                                requirementPackageSubscription.RPS_ApplicantOrgUserID = cond.CCPS_OrgUserID.Value;
                                requirementPackageSubscription.RPS_RequirementPackageStatusID = requirementNotCompliantPackStatusID;
                                requirementPackageSubscription.RPS_IsDeleted = false;
                                requirementPackageSubscription.RPS_CreatedByID = currentLoggedInUserId;
                                requirementPackageSubscription.RPS_CreatedOn = DateTime.Now;

                                //Add records in ClinicalRotationSubscription table
                                ClinicalRotationSubscription clinicalRotationSubscription = new ClinicalRotationSubscription();
                                clinicalRotationSubscription.CRS_ClinicalRotationRequirementPackageID = CRRP.CRRP_ID;
                                clinicalRotationSubscription.CRS_IsDeleted = false;
                                clinicalRotationSubscription.CRS_CreatedByID = currentLoggedInUserId;
                                clinicalRotationSubscription.CRS_CreatedOn = DateTime.Now;

                                requirementPackageSubscription.ClinicalRotationSubscriptions.Add(clinicalRotationSubscription);
                                _dbContext.RequirementPackageSubscriptions.AddObject(requirementPackageSubscription);
                                lstRequirementPackageSubscriptionToBeAdded.Add(requirementPackageSubscription);
                            }
                            //Commented in UAT-4960
                            //}
                            //else
                            //{
                            //    var existingClinicalRotationSubscription = _dbContext.ClinicalRotationSubscriptions
                            //                                                .Where(crs => !crs.CRS_IsDeleted
                            //                                                        && crs.CRS_ClinicalRotationRequirementPackageID == CRRP.CRRP_ID
                            //                                                        && crs.CRS_RequirementPackageSubscriptionID == existingSubscription.RPS_ID).FirstOrDefault();


                            //    if (existingClinicalRotationSubscription.IsNullOrEmpty())
                            //    {
                            //        //Add records in ClinicalRotationSubscription table
                            //        ClinicalRotationSubscription clinicalRotationSubscription = new ClinicalRotationSubscription();
                            //        clinicalRotationSubscription.CRS_ClinicalRotationRequirementPackageID = CRRP.CRRP_ID;
                            //        clinicalRotationSubscription.CRS_IsDeleted = false;
                            //        clinicalRotationSubscription.CRS_CreatedByID = currentLoggedInUserId;
                            //        clinicalRotationSubscription.CRS_CreatedOn = DateTime.Now;
                            //        clinicalRotationSubscription.CRS_RequirementPackageSubscriptionID = existingSubscription.RPS_ID;
                            //        _dbContext.ClinicalRotationSubscriptions.AddObject(clinicalRotationSubscription);
                            //    }
                            //}


                            ////Add records in RequirementPackageSubscription table
                            //RequirementPackageSubscription requirementPackageSubscription = new RequirementPackageSubscription();
                            //requirementPackageSubscription.RPS_RequirementPackageID = CRRP.CRRP_RequirementPackageID;
                            //requirementPackageSubscription.RPS_RequirementSubscriptionTypeID = rotationSubscriptionTypeID;
                            //requirementPackageSubscription.RPS_ApplicantOrgUserID = cond.CCPS_OrgUserID.Value;
                            //requirementPackageSubscription.RPS_RequirementPackageStatusID = requirementNotCompliantPackStatusID;
                            //requirementPackageSubscription.RPS_IsDeleted = false;
                            //requirementPackageSubscription.RPS_CreatedByID = currentLoggedInUserId;
                            //requirementPackageSubscription.RPS_CreatedOn = DateTime.Now;

                            ////Add records in ClinicalRotationSubscription table
                            //ClinicalRotationSubscription clinicalRotationSubscription = new ClinicalRotationSubscription();
                            //clinicalRotationSubscription.CRS_ClinicalRotationRequirementPackageID = CRRP.CRRP_ID;
                            //clinicalRotationSubscription.CRS_IsDeleted = false;
                            //clinicalRotationSubscription.CRS_CreatedByID = currentLoggedInUserId;
                            //clinicalRotationSubscription.CRS_CreatedOn = DateTime.Now;

                            //requirementPackageSubscription.ClinicalRotationSubscriptions.Add(clinicalRotationSubscription);
                            //_dbContext.RequirementPackageSubscriptions.AddObject(requirementPackageSubscription);
                        });
                    });

                    if (_dbContext.SaveChanges() > AppConsts.NONE)
                    {
                        String packageSubscriptionIdsXML = "<PackageSubscriptionIDs>";
                        foreach (RequirementPackageSubscription requirementPackageSubscription in lstRequirementPackageSubscriptionToBeAdded)
                        {
                            packageSubscriptionIdsXML += "<ID>" + requirementPackageSubscription.RPS_ID + "</ID>";
                        }
                        packageSubscriptionIdsXML += "</PackageSubscriptionIDs>";
                        CreateOptionalCategorySetAproved(packageSubscriptionIdsXML, currentLoggedInUserId);
                        //UAT-2603
                        List<Int32> reqPkgSubscriptionsIds = lstRequirementPackageSubscriptionToBeAdded.Select(sel => sel.RPS_ID).ToList();
                        AddDataToRotDataMovement(reqPkgSubscriptionsIds, currentLoggedInUserId, dataMovementDueStatusId);
                    }

                }
            }
        }
        #endregion

        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        Dictionary<Int32, Boolean> IClinicalRotationRepository.GetComplianceRequiredRotCatForPackage(Int32 reqPackageId)
        {
            Dictionary<Int32, Boolean> lstComplianceRqdForReqCategories = new Dictionary<Int32, Boolean>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ReqPackageId", reqPackageId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationComplianceRqdForPackage", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstComplianceRqdForReqCategories.Add(Convert.ToInt32(dr["RequirementCategoryID"]), Convert.ToBoolean(dr["ComplianceRequired"]));
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstComplianceRqdForReqCategories;
        }
        #endregion

        #region UAT-2165: Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        public void CreateOptionalCategorySetAproved(String packageSubscriptionIdsXML, Int32 currentUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_OptionalCategorySetAproved", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageSubscriptionIDs", packageSubscriptionIdsXML);
                command.Parameters.AddWithValue("@SystemUserID", currentUserId);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }
        #endregion

        public void CreateOptionalCategorySetAproved(List<RequirementPackageSubscription> lstRequirementPackageSubscriptionToBeAdded, Int32 currentUserId)
        {
            String packageSubscriptionIdsXML = "<PackageSubscriptionIDs>";
            foreach (RequirementPackageSubscription requirementPackageSubscription in lstRequirementPackageSubscriptionToBeAdded)
            {
                packageSubscriptionIdsXML += "<ID>" + requirementPackageSubscription.RPS_ID + "</ID>";
            }
            packageSubscriptionIdsXML += "</PackageSubscriptionIDs>";
            CreateOptionalCategorySetAproved(packageSubscriptionIdsXML, currentUserId);
        }

        #region UAT 2477
        List<ClinicalRotationDetailContract> IClinicalRotationRepository.GetRotationPackageAndAgencyData(int rotationID, int tenantID)
        {
            List<ClinicalRotationDetailContract> lstClinicalRotDetail = new List<ClinicalRotationDetailContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {   new SqlParameter("@RotationID", rotationID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationPackageAndAgencyData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationDetailContract clinicalRotDetail = new ClinicalRotationDetailContract();
                            clinicalRotDetail.RotationID = dr["CR_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CR_ID"]);
                            clinicalRotDetail.AgencyID = dr["CRA_AgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CRA_AgencyID"]);
                            clinicalRotDetail.RotationName = dr["CRA_AgencyID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CRA_AgencyID"]);
                            clinicalRotDetail.RequirementPackageID = dr["CRRP_RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CRRP_RequirementPackageID"]);
                            clinicalRotDetail.IsComplianceRequiredforRotation = dr["Compliance Required for Rotation"] == DBNull.Value ? "no" : Convert.ToString(dr["Compliance Required for Rotation"]);
                            clinicalRotDetail.IsComplianceRequiredforInstructorPreceptorRotationPkgs = dr["Compliance Required for Instructor/Preceptor Rotation Package"] == DBNull.Value ? "no" : Convert.ToString(dr["Compliance Required for Instructor/Preceptor Rotation Package"]);
                            clinicalRotDetail.RequirementPackageTypeCode = dr["RequirementPackageTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageTypeCode"]);
                            lstClinicalRotDetail.Add(clinicalRotDetail);
                        }
                    }
                }
                return lstClinicalRotDetail;
            }
        }
        #endregion

        #region UAT-2514
        Dictionary<Boolean, DateTime> IClinicalRotationRepository.IsRotationEndDateRangeNeedToManage(Int32 clinicalRotationID)
        {
            Dictionary<Boolean, DateTime> result = new Dictionary<Boolean, DateTime>();
            List<RequirementPackage> requirementPackage = _dbContext.ClinicalRotationRequirementPackages.Where(cond => cond.CRRP_ClinicalRotationID == clinicalRotationID
                                                                    && !cond.CRRP_IsDeleted && !cond.ClinicalRotation.CR_IsDeleted && !cond.RequirementPackage.RP_IsDeleted
                                                                    && cond.RequirementPackage.RP_IsNewPackage).Select(sel => sel.RequirementPackage).ToList();
            if (!requirementPackage.IsNullOrEmpty())
            {
                result.Add(true, requirementPackage.Max(x => x.RP_EffectiveStartDate.Value));
            }
            return result;
        }
        #endregion

        public List<ClinicalRotationMember> GetDroppedRotationMembersByRotationID(Int32 clinicalRotationID)
        {
            return _dbContext.ClinicalRotationMembers.Where(x => x.CRM_ClinicalRotationID == clinicalRotationID
                                                                    && !x.CRM_IsDeleted
                                                                    && x.CRM_IsDropped).ToList();
        }

        #region UAT-2424
        /// <summary>
        /// Get All Clinical Rotations of a Tenant
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        List<ClinicalRotation> IClinicalRotationRepository.GetAllClinicalRotations()
        {
            return _dbContext.ClinicalRotations.Where(cond => !cond.CR_IsDeleted).ToList();
        }

        ClinicalRotationRequirementPackage IClinicalRotationRepository.GetRotationPackageByRotationId(Int32 clinicalRotationID, String reqPkgTypeCode)
        {
            return _dbContext.ClinicalRotationRequirementPackages.FirstOrDefault(x => x.CRRP_ClinicalRotationID == clinicalRotationID
                                                                                    && !x.CRRP_IsDeleted && x.RequirementPackage.lkpRequirementPackageType.RPT_Code == reqPkgTypeCode);

        }

        List<ClinicalRotationDetailContract> IClinicalRotationRepository.GetAllClinicalRotationsForLoggedInUser(Int32 currentUserId, bool isAdminLoggedIn)
        {
            List<ClinicalRotationDetailContract> clinicalRotationDetailContract = new List<ClinicalRotationDetailContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@userID", currentUserId),
                            new SqlParameter("@isAdbAdminLoggedIn", isAdminLoggedIn)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAllClinicalRotationsForLoggedInUser", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clinicalRotationDetailContract.Add(new ClinicalRotationDetailContract
                            {
                                RotationID = Convert.ToInt32(dr["CR_ID"]),
                                RotationName = dr["CR_RotationName"].ToString(),
                                ComplioID = dr["CR_ComplioID"].ToString()
                            });
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }

            return clinicalRotationDetailContract;
        }

        #region UAT: 4062
        public List<MultipleAdditionalDocumentsContract> GetAdditionalDocumnetDetails(Int32 clinicalRotationId, int additionalDocumentTypeId)
        {
            try
            {

                List<MultipleAdditionalDocumentsContract> listMultipleAdditionalDocumentsContract = new List<MultipleAdditionalDocumentsContract>();
                var temp = from CD in _dbContext.ClinicalRotationDocuments
                           join CSD in _dbContext.ClientSystemDocuments on CD.CRD_ClientSystemDocumentID equals CSD.CSD_ID
                           where CD.CRD_ClinicalRotationID == clinicalRotationId && CD.CRD_IsDeleted == false && CSD.CSD_IsDeleted == false && CSD.CSD_DocumentTypeID == additionalDocumentTypeId
                           select new { FileName = CSD.CSD_FileName, DocumentID = CD.CRD_ClientSystemDocumentID, FilePath = CSD.CSD_DocumentPath };

                foreach (var item in temp)
                {
                    listMultipleAdditionalDocumentsContract.Add(new MultipleAdditionalDocumentsContract
                    {
                        AdditionalDocumentFileName = item.FileName,
                        AdditionalDocumentFilePath = item.FilePath,
                        AdditionalDocumentID = item.DocumentID
                    });

                }
                return listMultipleAdditionalDocumentsContract;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        /// <summary>
        /// Get Rotation Details by Rotation ID
        /// </summary>
        /// <param name="clinicalRotationId"></param>
        /// <returns></returns>
        ClinicalRotationDetailContract IClinicalRotationRepository.GetClinicalRotationDetailsById(Int32 clinicalRotationId, int syllabusDocumentTypeID)
        {
            ClinicalRotationDetailContract clinicalRotationDetailContract = new ClinicalRotationDetailContract();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ClinicalRotationID", clinicalRotationId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClinicalRotationDetailsByRotationID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clinicalRotationDetailContract.RotationID = dr["ClinicalRotationId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ClinicalRotationId"]);
                            clinicalRotationDetailContract.Department = Convert.ToString(dr["Department"]);
                            clinicalRotationDetailContract.Program = Convert.ToString(dr["Program"]);
                            clinicalRotationDetailContract.Course = Convert.ToString(dr["Course"]);
                            clinicalRotationDetailContract.UnitFloorLoc = Convert.ToString(dr["UnitFloorLoc"]);
                            clinicalRotationDetailContract.RecommendedHours = dr["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfHours"]));
                            clinicalRotationDetailContract.Shift = Convert.ToString(dr["RotationShift"]);
                            clinicalRotationDetailContract.RotationName = Convert.ToString(dr["RotationName"]);
                            clinicalRotationDetailContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            clinicalRotationDetailContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            clinicalRotationDetailContract.StartTime = dr["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(dr["StartTime"]);
                            clinicalRotationDetailContract.EndTime = dr["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(dr["EndTime"]);
                            clinicalRotationDetailContract.ComplioID = dr["ComplioID"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["ComplioID"]);
                            clinicalRotationDetailContract.AgencyID = dr["AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            clinicalRotationDetailContract.AgencyName = Convert.ToString(dr["AgencyName"]);
                            clinicalRotationDetailContract.Term = Convert.ToString(dr["Term"]);
                            clinicalRotationDetailContract.TypeSpecialty = Convert.ToString(dr["TypeSpecialty"]);
                            clinicalRotationDetailContract.Students = dr["NoOfStudents"] == DBNull.Value ? (Int32?)null : (Int32?)(Convert.ToInt32(dr["NoOfStudents"]));
                            clinicalRotationDetailContract.DaysIdList = dr["DaysIDs"].GetType().Name == "DBNull" ? string.Empty : Convert.ToString(dr["DaysIDs"]);
                            clinicalRotationDetailContract.ContactIdList = dr["ClientContactIDs"].GetType().Name == "DBNull" ? string.Empty : Convert.ToString(dr["ClientContactIDs"]);
                            clinicalRotationDetailContract.DeadlineDate = dr["DeadlineDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DeadlineDate"]);
                            clinicalRotationDetailContract.DaysBefore = dr["DaysBefore"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["DaysBefore"]);
                            clinicalRotationDetailContract.Frequency = Convert.ToString(dr["Frequency"]);
                            clinicalRotationDetailContract.HierarchyNodeIDList = dr["HierarchyNodeIDList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodeIDList"]);
                            clinicalRotationDetailContract.HierarchyNodes = dr["HierarchyNodes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodes"]);
                            clinicalRotationDetailContract.CustomAttributes = dr["CustomAttributeList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CustomAttributeList"]);
                            clinicalRotationDetailContract.SyllabusFileName = dr["SyllabusFileName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SyllabusFileName"]);
                            clinicalRotationDetailContract.SyllabusFilePath = dr["SyllabusFilePath"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SyllabusFilePath"]);
                            clinicalRotationDetailContract.AgencyHierarchyID = dr["AgencyHierarchyID"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AgencyHierarchyID"]);
                            clinicalRotationDetailContract.RootNodeID = dr["RootNodeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RootNodeID"]);
                            clinicalRotationDetailContract.AgencyIDs = Convert.ToString(dr["AgencyIDs"]);
                            clinicalRotationDetailContract.AgencyHierarchyIDs = Convert.ToString(dr["AgencyHierarchyIDs"]);
                            clinicalRotationDetailContract.IsAllowNotification = dr["IsAllowNotification"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsAllowNotification"]);
                            clinicalRotationDetailContract.IsEditableByAgencyUser = dr["IsEditableByAgencyUser"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsEditableByAgencyUser"]);
                            clinicalRotationDetailContract.IsEditableByClientAdmin = dr["IsEditableByClientAdmin"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsEditableByClientAdmin"]);
                            clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID = dr["AgnecyHierarchyRootNodeSettingMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgnecyHierarchyRootNodeSettingMappingID"]);
                            //UAT-4150
                            clinicalRotationDetailContract.IsSchoolSendingInstructor = dr["IsSchoolSendingInstructor"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsSchoolSendingInstructor"]);

                            if (clinicalRotationDetailContract.listOfMultipleDocument.IsNullOrEmpty())
                            {

                                clinicalRotationDetailContract.listOfMultipleDocument = GetAdditionalDocumnetDetails(clinicalRotationId, syllabusDocumentTypeID);
                            }
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

            }
            return clinicalRotationDetailContract;
        }

        #endregion


        #region UAT-2544:
        private void CreateShanpshotForDroppedStudent(Int32 clinicalRotationId, List<Int32> clinicalMemberIdList, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            String clinicalMemberIds = String.Join(",", clinicalMemberIdList);

            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_CreateRequirementSnapshotOnMemberDropped", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClinicalRotationID", clinicalRotationId);
                command.Parameters.AddWithValue("@ClinicalRotationMemberIDs", clinicalMemberIds);
                command.Parameters.AddWithValue("@CurrentLoggedInUserD", currentLoggedInUserId);
                command.Parameters.AddWithValue("@TenantID", tenantId);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public Boolean IsApplicantDroppedFromRotation(Int32 clinicalRotationId, Int32 RPS_ID, Int32 currentLoggedInUserId)
        {
            try
            {
                List<ClinicalRotationDetailContract> clinicalRotDetailList = new List<ClinicalRotationDetailContract>();

                EntityConnection connection = _dbContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ClinicalRotationID", clinicalRotationId),
                             new SqlParameter("@RequirementPackageSubscriptionID", RPS_ID),
                             new SqlParameter("@LoggedInOrgUserID", currentLoggedInUserId)
                        };

                    SqlCommand command = new SqlCommand("usp_IsStudentDroppedFromRotation", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(sqlParameterCollection);


                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = command;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return Convert.ToBoolean(ds.Tables[0].Rows[0]["IsApplicantDropped"]);
                        }
                    }
                }
                return false;
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

        public Boolean NeedToChangeInvitationStatusAsPending(Int32 clinicalRotationId, List<Int32> invitationIDs, Int32 studentid, Int32 currentLoggedInUserId)
        {
            try
            {
                String invitationIDsXML = String.Join(",", invitationIDs);

                EntityConnection connection = _dbContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ClinicalRotationID", clinicalRotationId),
                             new SqlParameter("@InvitationIDs", invitationIDsXML),
                             new SqlParameter("@OrganizationUserID", studentid),
                             new SqlParameter("@LoggedInOrgUserID", currentLoggedInUserId)
                        };

                    SqlCommand command = new SqlCommand("usp_NeedToChangeStatusAsPendingReview", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(sqlParameterCollection);


                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = command;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return Convert.ToBoolean(ds.Tables[0].Rows[0]["NeedToChangeStatusAsPending"]);
                        }
                    }
                }
                return false;
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

        #region UAT-2554

        Boolean IClinicalRotationRepository.IsPreceptorRequiredForAgency(Int32 agencyID, Int32 agencyPrmsnTypeID)
        {
            return new ProfileSharingRepository().IsPreceptorRequiredForAgency(agencyID, agencyPrmsnTypeID);
        }
        #endregion

        #region UAT-2313

        void IClinicalRotationRepository.SyncRotationAgencyAndClientContacts(String rotationIds, Int32 tenantId, Int32 organizationUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_SyncRotationAgencyAndClientContacts", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClinicalRoatiaonIDList", rotationIds);
                command.Parameters.AddWithValue("@TenantID", tenantId);
                command.Parameters.AddWithValue("@IsAgencyToBeSynced", true);
                command.Parameters.AddWithValue("@IsClientContactToBeSynced", true);
                command.Parameters.AddWithValue("@OrganizationUserID", organizationUserId);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }

        }

        #endregion

        #region UAT-2666
        RotationDetailFieldChanges IClinicalRotationRepository.UpdateClinicalRotationByAgency(ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentLoggedInUserId, Boolean IsSharedUser, Int32 tenantId)
        {
            RotationDetailFieldChanges rotationDetailFieldChanges = new RotationDetailFieldChanges();
            ClinicalRotation clinicalRotationDetail = _dbContext.ClinicalRotations.Where(cmd => cmd.CR_ID == clinicalRotationDetailContract.RotationID && !cmd.CR_IsDeleted).FirstOrDefault();

            if (!clinicalRotationDetail.IsNullOrEmpty())
            {
                SaveUpdateRotationFieldUpdateByAgency(clinicalRotationDetail, clinicalRotationDetailContract, currentLoggedInUserId, IsSharedUser);

                #region UAT-3108
                rotationDetailFieldChanges = GenerateDataRotationFieldChanges(clinicalRotationDetail, clinicalRotationDetailContract, currentLoggedInUserId, tenantId, true, null);
                #endregion

                //UAT-4673 
                if (IsSharedUser)
                {
                    Boolean isEndDateUpdated = clinicalRotationDetailContract.EndDate != clinicalRotationDetail.CR_EndDate;
                    if (isEndDateUpdated)
                    {
                        RevertRotationStatusToPending(clinicalRotationDetailContract.RotationID, currentLoggedInUserId, tenantId, clinicalRotationDetailContract.AgencyID);
                    }

                    //UAT-4428
                    rotationDetailFieldChanges.IsStartDateUpdated = clinicalRotationDetailContract.StartDate != clinicalRotationDetail.CR_StartDate;
                    //END
                }
                //End UAT-4673

                clinicalRotationDetail.CR_Department = clinicalRotationDetailContract.Department;
                clinicalRotationDetail.CR_Course = clinicalRotationDetailContract.Course;
                clinicalRotationDetail.CR_RotationName = clinicalRotationDetailContract.RotationName;
                clinicalRotationDetail.CR_Program = clinicalRotationDetailContract.Program;
                clinicalRotationDetail.CR_RotationShift = clinicalRotationDetailContract.Shift;
                clinicalRotationDetail.CR_StartDate = clinicalRotationDetailContract.StartDate;
                clinicalRotationDetail.CR_EndDate = clinicalRotationDetailContract.EndDate;
                clinicalRotationDetail.CR_Term = clinicalRotationDetailContract.Term;
                clinicalRotationDetail.CR_TypeSpecialty = clinicalRotationDetailContract.TypeSpecialty;
                clinicalRotationDetail.CR_UnitFloorLoc = clinicalRotationDetailContract.UnitFloorLoc;
                clinicalRotationDetail.CR_NoOfHours = clinicalRotationDetailContract.RecommendedHours;
                clinicalRotationDetail.CR_NoOfStudents = clinicalRotationDetailContract.Students;
                clinicalRotationDetail.CR_ModifiedOn = DateTime.Now;
                clinicalRotationDetail.CR_ModifiedByID = currentLoggedInUserId;
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
            {
                rotationDetailFieldChanges.IsClinicalRotationUpdatedSuccessfully = true;
            }
            else
            {
                rotationDetailFieldChanges.IsClinicalRotationUpdatedSuccessfully = false;
            }
            return rotationDetailFieldChanges;

            //    return true;
            //return false;
        }

        /// <summary>
        /// Save Update RotationFieldUpdateByAgency Table
        /// </summary>
        /// <param name="clinicalRotation"></param>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="IsSharedUser"></param>
        private void SaveUpdateRotationFieldUpdateByAgency(ClinicalRotation clinicalRotation, ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentLoggedInUserId, Boolean IsSharedUser)
        {
            RotationFieldUpdatedByAgency rotationFieldupdatedByAgencyDetails = clinicalRotation.RotationFieldUpdatedByAgencies
                                                                                        .Where(cmd => cmd.RFUA_ClinicalRotationID == clinicalRotation.CR_ID
                                                                                                   && !cmd.RFUA_IsDeleted).FirstOrDefault();
            if (rotationFieldupdatedByAgencyDetails.IsNullOrEmpty())
            {
                rotationFieldupdatedByAgencyDetails = new RotationFieldUpdatedByAgency();

                rotationFieldupdatedByAgencyDetails.RFUA_ClinicalRotationID = clinicalRotation.CR_ID;

                rotationFieldupdatedByAgencyDetails.RFUA_IsDepartmentUpdated = !String.Equals(clinicalRotationDetailContract.Department, clinicalRotation.CR_Department.Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsCourseUpdated = !String.Equals(clinicalRotationDetailContract.Course, clinicalRotation.CR_Course.Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsNoOfHoursUpdated = !String.Equals(Convert.ToString(clinicalRotationDetailContract.RecommendedHours), Convert.ToString(clinicalRotation.CR_NoOfHours).Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsNoOfStudentsUpdated = !String.Equals(Convert.ToString(clinicalRotationDetailContract.Students), Convert.ToString(clinicalRotation.CR_NoOfStudents).Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsProgramUpdated = !String.Equals(clinicalRotationDetailContract.Program, clinicalRotation.CR_Program.Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsRotationNameUpadted = !String.Equals(clinicalRotationDetailContract.RotationName, clinicalRotation.CR_RotationName.Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsRotationShiftUpdated = !String.Equals(clinicalRotationDetailContract.Shift, clinicalRotation.CR_RotationShift.Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsTermUpdated = !String.Equals(clinicalRotationDetailContract.Term, clinicalRotation.CR_Term.Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsTypeSpecialtyUpdated = !String.Equals(clinicalRotationDetailContract.TypeSpecialty, clinicalRotation.CR_TypeSpecialty.Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsUnitFloorLocUpdated = !String.Equals(clinicalRotationDetailContract.UnitFloorLoc, clinicalRotation.CR_UnitFloorLoc.Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsStartDateUpdated = !String.Equals(clinicalRotationDetailContract.StartDate.Value.ToString(), clinicalRotation.CR_StartDate.Value.ToString().Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsEndDateUpdated = !String.Equals(clinicalRotationDetailContract.EndDate.Value.ToString(), clinicalRotation.CR_EndDate.Value.ToString().Trim());
                rotationFieldupdatedByAgencyDetails.RFUA_IsDeleted = false;
                rotationFieldupdatedByAgencyDetails.RFUA_CreatedByID = currentLoggedInUserId;
                rotationFieldupdatedByAgencyDetails.RFUA_CreatedOn = DateTime.Now;
                clinicalRotation.RotationFieldUpdatedByAgencies.Add(rotationFieldupdatedByAgencyDetails);
            }
            else
            {
                rotationFieldupdatedByAgencyDetails.RFUA_ClinicalRotationID = clinicalRotation.CR_ID;

                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsStartDateUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsStartDateUpdated = !String.Equals(clinicalRotationDetailContract.StartDate.Value.ToString(), clinicalRotation.CR_StartDate.Value.ToString().Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsEndDateUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsEndDateUpdated = !String.Equals(clinicalRotationDetailContract.EndDate.Value.ToString(), clinicalRotation.CR_EndDate.Value.ToString().Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsDepartmentUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsDepartmentUpdated = !String.Equals(clinicalRotationDetailContract.Department, clinicalRotation.CR_Department.Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsCourseUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsCourseUpdated = !String.Equals(clinicalRotationDetailContract.Course, clinicalRotation.CR_Course.Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsNoOfHoursUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsNoOfHoursUpdated = !String.Equals(Convert.ToString(clinicalRotationDetailContract.RecommendedHours), Convert.ToString(clinicalRotation.CR_NoOfHours).Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsNoOfStudentsUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsNoOfStudentsUpdated = !String.Equals(Convert.ToString(clinicalRotationDetailContract.Students), Convert.ToString(clinicalRotation.CR_NoOfStudents).Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsProgramUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsProgramUpdated = !String.Equals(clinicalRotationDetailContract.Program, clinicalRotation.CR_Program.Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsRotationNameUpadted))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsRotationNameUpadted = !String.Equals(clinicalRotationDetailContract.RotationName, clinicalRotation.CR_RotationName.Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsRotationShiftUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsRotationShiftUpdated = !String.Equals(clinicalRotationDetailContract.Shift, clinicalRotation.CR_RotationShift.Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsTermUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsTermUpdated = !String.Equals(clinicalRotationDetailContract.Term, clinicalRotation.CR_Term.Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsTypeSpecialtyUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsTypeSpecialtyUpdated = !String.Equals(clinicalRotationDetailContract.TypeSpecialty, clinicalRotation.CR_TypeSpecialty.Trim());
                }
                if (!Boolean.Equals(IsSharedUser, rotationFieldupdatedByAgencyDetails.RFUA_IsUnitFloorLocUpdated))
                {
                    rotationFieldupdatedByAgencyDetails.RFUA_IsUnitFloorLocUpdated = !String.Equals(clinicalRotationDetailContract.UnitFloorLoc, clinicalRotation.CR_UnitFloorLoc.Trim());
                }

                rotationFieldupdatedByAgencyDetails.RFUA_ModifiedByID = currentLoggedInUserId;
                rotationFieldupdatedByAgencyDetails.RFUA_ModifiedOn = DateTime.Now;
            }
        }

        /// <summary>
        /// Remove RotationFieldUpdatedByAgency by ClinicalRotationID
        /// </summary>
        /// <param name="RotationId"></param>
        /// <param name="CurrentLoggedInUserId"></param>
        private void DeleteRotationFieldUpdatedByAgency(Int32 RotationId, Int32 CurrentLoggedInUserId)
        {
            _dbContext.RotationFieldUpdatedByAgencies.Where(cmd => cmd.RFUA_ClinicalRotationID == RotationId && !cmd.RFUA_IsDeleted).ForEach(x =>
            {
                x.RFUA_IsDeleted = true;
                x.RFUA_ModifiedByID = CurrentLoggedInUserId;
                x.RFUA_ModifiedOn = DateTime.Now;
            });
        }

        List<RotationFieldUpdatedByAgency> IClinicalRotationRepository.GetRotationFieldUpdateByAgencyDetails(List<Int32> lstClinicalRotationIds)
        {
            return _dbContext.RotationFieldUpdatedByAgencies.Where(cond => lstClinicalRotationIds.Contains(cond.RFUA_ClinicalRotationID) && !cond.RFUA_IsDeleted).ToList();
        }
        #endregion

        #region UAT-2603

        public Boolean AddDataToRotDataMovement(List<Int32> reqPkgSubscriptions, Int32 currentUserId, Int16 statusId)
        {
            try
            {
                foreach (Int32 rpsId in reqPkgSubscriptions)
                {
                    RotationDataMovement rotationDataMovement = new RotationDataMovement();
                    rotationDataMovement.RDM_RequirementPackageSubscriptionID = rpsId;
                    rotationDataMovement.RDM_StatusID = statusId;
                    rotationDataMovement.RDM_CreatedBy = currentUserId;
                    rotationDataMovement.RDM_CreatedOn = DateTime.Now;
                    rotationDataMovement.RDM_IsDeleted = false;
                    _dbContext.RotationDataMovements.AddObject(rotationDataMovement);
                }

                if (_dbContext.SaveChanges() > 0)
                    return true;
                else
                    return false;
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


        public Boolean UpdateDataMovementStatus(List<Int32> lstReqPkgSubsIds, Int16 dataMovementDueStatusId, Int16 dataMovementNotRequiredStatusId, Int32 currentUserId)
        {
            try
            {
                var rotationDataMovement = _dbContext.RotationDataMovements
                                                .Where(cond => lstReqPkgSubsIds.Contains(cond.RDM_RequirementPackageSubscriptionID)
                                                    && cond.RDM_StatusID == dataMovementDueStatusId
                                                    && cond.RDM_IsDeleted == false
                                                ).ToList();

                foreach (var rotationDataMovementObj in rotationDataMovement)
                {
                    rotationDataMovementObj.RDM_StatusID = dataMovementNotRequiredStatusId;
                    rotationDataMovementObj.RDM_ModifiedOn = DateTime.Now;
                    rotationDataMovementObj.RDM_ModifiedBy = currentUserId;
                    rotationDataMovementObj.RDM_ExecutionResponse = "RPS has been deleted.";
                }

                if (_dbContext.SaveChanges() > 0)
                    return true;
                else
                    return false;
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

        public List<Int32> GetRPSIdsWithDataMovementDueStatus(Int32 chunkSize, Int32 statusId)
        {
            try
            {
                //UAT-3530
                List<Int32> lstTargetIds = new List<Int32>();
                EntityConnection connection = _dbContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@StatusID", statusId),
                             new SqlParameter("@ChunkSize", chunkSize)
                        };

                    base.OpenSQLDataReaderConnection(con);

                    using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRPSIdsWithDataMovementDueStatus", sqlParameterCollection))
                    {
                        Int32 targetID;
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                targetID = dr["RequirementPackageSubscriptionID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementPackageSubscriptionID"]);
                                lstTargetIds.Add(targetID);
                            }
                        }
                    }
                    base.CloseSQLDataReaderConnection(con);
                }
                return lstTargetIds;
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

        public DataTable PerformRotationDataMovement(String lstRPSIdsWithDueStatus, Int32 currentUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_PerformRotationDataMovement", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TargetRequirementSubscriptionIds", lstRPSIdsWithDueStatus);
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentUserId);
                command.CommandTimeout = 120;
                if (con.State == ConnectionState.Closed)
                    con.Open();

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
        #endregion

        #region UAT-2712

        List<AgencyHierarchyRotationFieldOptionContract> IClinicalRotationRepository.GetAgencyHierarchyRotationFieldOptionSetting(String hierarchyID)
        {
            List<AgencyHierarchyRotationFieldOptionContract> resultList = new List<AgencyHierarchyRotationFieldOptionContract>();
            AgencyHierarchyRotationFieldOptionContract result;
            Nullable<Boolean> DefaultSetting = null;

            EntityConnection connection = SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyHierarchyID", null),
                    new SqlParameter("@AgencyIDs", hierarchyID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyRotationFieldOptionSettings", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = new AgencyHierarchyRotationFieldOptionContract();
                            result.AgencyHierarchyID = dr["AHRFO_AgencyHierarchyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AHRFO_AgencyHierarchyID"]);
                            result.AHRFO_IsCourse_Required = dr["AHRFO_IsCourse_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsCourse_Required"]);
                            result.AHRFO_IsDaysBefore_Required = dr["AHRFO_IsDaysBefore_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsDaysBefore_Required"]);
                            result.AHRFO_IsDeadlineDate_Required = dr["AHRFO_IsDeadlineDate_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsDeadlineDate_Required"]);
                            result.AHRFO_IsDepartment_Required = dr["AHRFO_IsDepartment_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsDepartment_Required"]);
                            result.AHRFO_IsEndTime_Required = dr["AHRFO_IsEndTime_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsEndTime_Required"]);
                            result.AHRFO_IsFrequency_Required = dr["AHRFO_IsFrequency_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsFrequency_Required"]);
                            result.AHRFO_IsIP_Required = dr["AHRFO_IsIP_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsIP_Required"]);
                            result.AHRFO_IsNoOfHours_Required = dr["AHRFO_IsNoOfHours_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsNoOfHours_Required"]);
                            result.AHRFO_IsNoOfStudents_Required = dr["AHRFO_IsNoOfStudents_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsNoOfStudents_Required"]);
                            result.AHRFO_IsProgram_Required = dr["AHRFO_IsProgram_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsProgram_Required"]);
                            result.AHRFO_IsRotationName_Required = dr["AHRFO_IsRotationName_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsRotationName_Required"]);
                            result.AHRFO_IsRotationShift_Required = dr["AHRFO_IsRotationShift_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsRotationShift_Required"]);
                            result.AHRFO_IsRotDays_Required = dr["AHRFO_IsRotDays_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsRotDays_Required"]);
                            result.AHRFO_IsStartTime_Required = dr["AHRFO_IsStartTime_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsStartTime_Required"]);
                            result.AHRFO_IsSyllabusDocument_Required = dr["AHRFO_IsSyllabusDocument_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsSyllabusDocument_Required"]);
                            result.AHRFO_IsTerm_Required = dr["AHRFO_IsTerm_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsTerm_Required"]);
                            result.AHRFO_IsTypeSpecialty_Required = dr["AHRFO_IsTypeSpecialty_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsTypeSpecialty_Required"]);
                            result.AHRFO_IsUnitFloorLoc_Required = dr["AHRFO_IsUnitFloorLoc_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsUnitFloorLoc_Required"]);
                            result.AHRFO_IsAdditionalDocuments_Required = dr["AHRFO_IsAdditionalDocuments_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsAdditionalDocuments_Required"]);
                            resultList.Add(result);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }

            #region Set-Permissions
            //if (result.AgencyHierarchyID < AppConsts.ONE)
            //{
            //    //Setting not existed in DB
            //    result.AgencyHierarchyID = hierarchyID;
            //    result.AHRFO_IsCourse_Required = DefaultSetting;
            //    result.AHRFO_IsDaysBefore_Required = DefaultSetting;
            //    result.AHRFO_IsDeadlineDate_Required = DefaultSetting;
            //    result.AHRFO_IsDepartment_Required = DefaultSetting;
            //    result.AHRFO_IsEndTime_Required = DefaultSetting;
            //    result.AHRFO_IsFrequency_Required = DefaultSetting;
            //    result.AHRFO_IsIP_Required = DefaultSetting;
            //    result.AHRFO_IsNoOfHours_Required = DefaultSetting;
            //    result.AHRFO_IsNoOfStudents_Required = DefaultSetting;
            //    result.AHRFO_IsProgram_Required = DefaultSetting;
            //    result.AHRFO_IsRotationName_Required = DefaultSetting;
            //    result.AHRFO_IsRotationShift_Required = DefaultSetting;
            //    result.AHRFO_IsRotDays_Required = DefaultSetting;
            //    result.AHRFO_IsStartTime_Required = DefaultSetting;
            //    result.AHRFO_IsSyllabusDocument_Required = DefaultSetting;
            //    result.AHRFO_IsTerm_Required = DefaultSetting;
            //    result.AHRFO_IsTypeSpecialty_Required = DefaultSetting;
            //    result.AHRFO_IsUnitFloorLoc_Required = DefaultSetting;
            //}

            #endregion
            return resultList;
        }
        #endregion

        List<ClinicalRotationAgencyContract> IClinicalRotationRepository.GetAgenciesMappedWithRotation(Int32 clinicalRotationID)
        {
            List<ClinicalRotationAgencyContract> lstAgency = new List<ClinicalRotationAgencyContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@ClinicalRotationID", clinicalRotationID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetMappedAgenciesWithClinicalRotation", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationAgencyContract clinicalRotationAgencyContract = new ClinicalRotationAgencyContract();
                            clinicalRotationAgencyContract.AgencyID = dr["AgencyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            clinicalRotationAgencyContract.AgencyName = dr["AgencyName"].GetType().Name == "DBNull" ? string.Empty : Convert.ToString(dr["AgencyName"]);
                            lstAgency.Add(clinicalRotationAgencyContract);
                        }
                    }
                }
            }

            return lstAgency;
        }

        #region UAT-2513
        Boolean IClinicalRotationRepository.SaveBatchRotationUploadDetails(List<BatchRotationUploadContract> clinicalRotationDetailContractList, String fileName, Int32 currentLoggedInID)
        {
            Boolean rotationListSavedStatus = false;

            var bulkRotationUpload = new BulkRotationUpload();
            bulkRotationUpload.BRU_CreatedBy = currentLoggedInID;
            bulkRotationUpload.BRU_CreatedOn = DateTime.Now;
            bulkRotationUpload.BRU_FileName = fileName;
            bulkRotationUpload.BRU_IsDeleted = false;

            foreach (var row in clinicalRotationDetailContractList)
            {
                BulkRotationUploadDetail bulkRotationUploadDetail = new BulkRotationUploadDetail();
                bulkRotationUploadDetail.BRUD_Agency = row.Agency;
                bulkRotationUploadDetail.BRUD_BatchRotationUploadStatusID = AppConsts.ONE;
                bulkRotationUploadDetail.BRUD_ComplioID = null;
                bulkRotationUploadDetail.BRUD_Course = row.Course;
                bulkRotationUploadDetail.BRUD_CreatedBy = currentLoggedInID;
                bulkRotationUploadDetail.BRUD_CreatedOn = DateTime.Now;
                bulkRotationUploadDetail.BRUD_Days = row.Days;
                bulkRotationUploadDetail.BRUD_Hours = row.Recommended_Hours;
                bulkRotationUploadDetail.BRUD_Department = row.Department;
                bulkRotationUploadDetail.BRUD_EndDate = Convert.ToDateTime(row.EndDate);
                bulkRotationUploadDetail.BRUD_InstitutionNodeID = Convert.ToInt32(row.InstitutionNodeID);
                bulkRotationUploadDetail.BRUD_Instructor_Preceptor = row.Instructor_Preceptor;
                bulkRotationUploadDetail.BRUD_IsDeleted = false;
                bulkRotationUploadDetail.BRUD_NoOfStudents = row.Students;
                bulkRotationUploadDetail.BRUD_Program = row.Program;
                bulkRotationUploadDetail.BRUD_RotationName = row.Rotation_Name;
                bulkRotationUploadDetail.BRUD_RotationReviewStatus = row.Rotation_Review_Status;
                bulkRotationUploadDetail.BRUD_RotationShift = row.Shift;
                bulkRotationUploadDetail.BRUD_StartDate = Convert.ToDateTime(row.StartDate);
                bulkRotationUploadDetail.BRUD_Term = row.Term;
                bulkRotationUploadDetail.BRUD_Time = row.Time;
                bulkRotationUploadDetail.BRUD_TypeSpecialty = row.Type_Specialty;
                bulkRotationUploadDetail.BRUD_UnitFloorLoc = row.Unit_Floor;
                bulkRotationUpload.BulkRotationUploadDetails.Add(bulkRotationUploadDetail);
            }

            _dbContext.BulkRotationUploads.AddObject(bulkRotationUpload);
            _dbContext.SaveChanges();
            rotationListSavedStatus = true;

            return rotationListSavedStatus;
        }

        List<BatchRotationUploadContract> IClinicalRotationRepository.GetBatchRotationList(BatchRotationUploadContract searchContract, CustomPagingArgsContract gridCustomPaging)
        {
            List<BatchRotationUploadContract> BatchRotationUploadContractList = new List<BatchRotationUploadContract>();
            String orderBy = "RotationName";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@RotationName", searchContract.Rotation_Name),
                            new SqlParameter("@StartDate", searchContract.StartDate),
                            new SqlParameter("@EndDate", searchContract.EndDate),
                            new SqlParameter("@UploadStatusCode", searchContract.Upload_Status),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", gridCustomPaging.CurrentPageIndex),
                            new SqlParameter("@PageSize", gridCustomPaging.PageSize),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetBatchRotationList", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BatchRotationUploadContract batchRotationUploadContract = new BatchRotationUploadContract();
                            batchRotationUploadContract.Agency = dr["BRUD_Agency"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_Agency"]);
                            batchRotationUploadContract.Course = dr["BRUD_Course"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_Course"]);
                            batchRotationUploadContract.Days = dr["BRUD_Days"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_Days"]);
                            batchRotationUploadContract.Department = dr["BRUD_Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_Department"]);
                            batchRotationUploadContract.EndDate = dr["BRUD_EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["BRUD_EndDate"]);
                            batchRotationUploadContract.InstitutionNodeID = Convert.ToInt32(dr["BRUD_InstitutionNodeID"]);
                            batchRotationUploadContract.Instructor_Preceptor = dr["BRUD_Instructor_Preceptor"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_Instructor_Preceptor"]);
                            batchRotationUploadContract.Program = dr["BRUD_Program"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_Program"]);
                            batchRotationUploadContract.Recommended_Hours = dr["BRUD_Hours"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_Hours"]);
                            batchRotationUploadContract.Rotation_Name = dr["BRUD_RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_RotationName"]);
                            batchRotationUploadContract.Rotation_Review_Status = dr["BRUS_Name"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUS_Name"]);
                            batchRotationUploadContract.Shift = dr["BRUD_RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_RotationShift"]);
                            batchRotationUploadContract.StartDate = dr["BRUD_StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["BRUD_StartDate"]);
                            batchRotationUploadContract.Students = dr["BRUD_NoOfStudents"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_NoOfStudents"]);
                            batchRotationUploadContract.Term = dr["BRUD_Term"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_Term"]);
                            batchRotationUploadContract.Time = dr["BRUD_Time"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_Time"]);
                            batchRotationUploadContract.Type_Specialty = dr["BRUD_TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_TypeSpecialty"]);
                            batchRotationUploadContract.Unit_Floor = dr["BRUD_UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_UnitFloorLoc"]);
                            batchRotationUploadContract.TotalCount = dr["TotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TotalCount"]);
                            batchRotationUploadContract.BatchRotationErrorMessage = dr["BRUD_ErrorMessage"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BRUD_ErrorMessage"]);
                            batchRotationUploadContract.CreatedOn = dr["BRUD_CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["BRUD_CreatedOn"]);
                            BatchRotationUploadContractList.Add(batchRotationUploadContract);
                        }
                    }
                }
                return BatchRotationUploadContractList;
            }
        }

        List<Int32> IClinicalRotationRepository.GetBatchRotationListForTimer(Int32 chunksize)
        {
            return _dbContext.BulkRotationUploadDetails.Where(cond => cond.BRUD_BatchRotationUploadStatusID == AppConsts.ONE && !cond.BRUD_IsDeleted).Select(sel => sel.BRUD_ID).OrderBy(ord => ord).Take(chunksize).ToList();
        }

        List<ClinicalRotationDetailContract> IClinicalRotationRepository.CreateClinicalRotationFromBatchRotationUploadDetails(List<Int32> lstBatchRotationUploadDetailId, Int32 currentLoggedInID)
        {
            List<ClinicalRotationDetailContract> lstClinicalRotationDetailContract = new List<ClinicalRotationDetailContract>();//UAT-2973

            String batchRotationUploadDetailIds = String.Join(",", lstBatchRotationUploadDetailId);

            EntityConnection connection = _dbContext.Connection as EntityConnection;

            #region Commented Code regarding UAT-2973
            //using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            //{

            //    SqlCommand command = new SqlCommand("usp_BatchRotationCreation", con);
            //    command.CommandType = CommandType.StoredProcedure;
            //    command.Parameters.AddWithValue("@BatchRotationUploadDetailsIDs", batchRotationUploadDetailIds);
            //    command.Parameters.AddWithValue("@ModifiedID", currentLoggedInID);

            //    if (con.State == ConnectionState.Closed)
            //        con.Open();

            //    command.ExecuteNonQuery();
            //    con.Close();
            //}
            //return true; 
            #endregion

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@BatchRotationUploadDetailsIDs", batchRotationUploadDetailIds),
                            new SqlParameter("@ModifiedID", currentLoggedInID),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_BatchRotationCreation", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationDetailContract clinicalRotation = new ClinicalRotationDetailContract();
                            clinicalRotation.RotationID = dr["RotationId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RotationId"]);
                            clinicalRotation.AgencyIdList = dr["RotationAgencyIDs"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationAgencyIDs"]);
                            clinicalRotation.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            clinicalRotation.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            lstClinicalRotationDetailContract.Add(clinicalRotation);
                        }
                    }
                }
                return lstClinicalRotationDetailContract;
            }
        }
        #endregion


        #region [UAT-2679]

        List<RequirementPackageContract> IClinicalRotationRepository.GetRequirementPackage(Int32 packageTypeId)
        {

            return _dbContext.RequirementPackages
                        .Where(cond => !cond.RP_IsDeleted && cond.RP_IsActive && cond.RP_RequirementPackageTypeID == packageTypeId)
                        .Select(s => new RequirementPackageContract
                        {
                            RequirementPackageName = string.IsNullOrEmpty(s.RP_PackageLabel) ? s.RP_PackageName : s.RP_PackageLabel,
                            RequirementPackageID = s.RP_ID,
                        })
                        .OrderBy(o => o.RequirementPackageName)
                        .ToList();
        }

        List<RequirementCategoryContract> IClinicalRotationRepository.GetRequirementCategory(List<Int32> lstRequirementpackage)
        {
            return _dbContext.RequirementPackageCategories
                        .Where(cond => !cond.RPC_IsDeleted && (lstRequirementpackage.Count == 0 || (lstRequirementpackage.Contains(cond.RPC_RequirementPackageID))))
                        .Select(cond => new RequirementCategoryContract
                        {
                            RequirementCategoryName = string.IsNullOrEmpty(cond.RequirementCategory.RC_CategoryLabel) ? cond.RequirementCategory.RC_CategoryName : cond.RequirementCategory.RC_CategoryLabel
                            ,
                            RequirementCategoryID = cond.RequirementCategory.RC_ID
                        })
                        .OrderBy(o => o.RequirementCategoryName)
                        .Distinct()
                        .ToList();
        }

        List<RequirementItemContract> IClinicalRotationRepository.GetRequirementItem(List<Int32> lstRequirementcategory)
        {
            return _dbContext.RequirementCategoryItems
                        .Where(cond => !cond.RCI_IsDeleted && lstRequirementcategory.Contains(cond.RCI_RequirementCategoryID))
                        .Select(cond => new RequirementItemContract
                        {
                            RequirementItemName = string.IsNullOrEmpty(cond.RequirementItem.RI_ItemLabel) ? cond.RequirementItem.RI_ItemName : cond.RequirementItem.RI_ItemLabel
                            ,
                            RequirementItemID = cond.RequirementItem.RI_ID
                        })
                        .OrderBy(o => o.RequirementItemName)
                        .Distinct()
                        .ToList();
        }



        List<ApplicantRequirementDataAuditContract> IClinicalRotationRepository.GetApplicantRequirementDataAudit(ApplicantRequirementDataAuditSearchContract searchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<ApplicantRequirementDataAuditContract> lstApplicantRequirementDataAudit = new List<ApplicantRequirementDataAuditContract>();

            String orderBy = "TimeStampValue";
            String ordDirection = "DESC";

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@ApplicantFirstName", searchContract.ApplicantFirstName),
                            new SqlParameter("@ApplicantLastName", searchContract.ApplicantLastName),
                            new SqlParameter("@ComplioID",searchContract.ComplioId), //UAT-3117
                            new SqlParameter("@AdminFirstName", searchContract.AdminFirstName),
                            new SqlParameter("@AdminLastName", searchContract.AdminLastName),
                            new SqlParameter("@PackageIds", searchContract.PackageIDs),
                            new SqlParameter("@CategoryIds", searchContract.CategoryIDs),
                            new SqlParameter("@ItemID", searchContract.ItemID),
                            new SqlParameter("@FromDate", searchContract.FromDate),
                            new SqlParameter("@ToDate", searchContract.ToDate),
                            new SqlParameter("@UserGroupId", searchContract.FilterUserGroupID),
                            new SqlParameter("@ApplicantRequirementPackageTypeID", searchContract.PackageTypeID), //UAT-4019
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex ),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize),

                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_ApplicantRequirementDataAudit", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ApplicantRequirementDataAuditContract applicantRequirementDataAuditContract = new ApplicantRequirementDataAuditContract();
                            applicantRequirementDataAuditContract.ApplicantRequirementDataAuditID = Convert.ToInt32(dr["ApplicantRequirementDataAuditID"]);
                            applicantRequirementDataAuditContract.ApplicantName = dr["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                            applicantRequirementDataAuditContract.PackageName = dr["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageName"]);
                            applicantRequirementDataAuditContract.CategoryName = dr["CategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CategoryName"]);
                            applicantRequirementDataAuditContract.ItemName = dr["CategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ItemName"]);
                            applicantRequirementDataAuditContract.ChangeValue = dr["ChangeValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ChangeValue"]);
                            applicantRequirementDataAuditContract.ChangeBY = dr["ChangeBY"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ChangeBY"]);
                            applicantRequirementDataAuditContract.ChangeBYInitials = dr["ChangeBYInitials"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ChangeBYInitials"]);
                            applicantRequirementDataAuditContract.TimeStampValue = dr["TimeStampValue"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(dr["TimeStampValue"]);
                            applicantRequirementDataAuditContract.TotalCount = dr["TotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TotalCount"]);
                            //UAT-3117
                            applicantRequirementDataAuditContract.ComplioId = dr["ComplioId"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplioId"]);
                            lstApplicantRequirementDataAudit.Add(applicantRequirementDataAuditContract);
                        }
                    }
                }
                return lstApplicantRequirementDataAudit;
            }
        }

        #endregion

        #region UAT-2907
        List<Entity.OrganizationUser> IClinicalRotationRepository.GetOrganizationUserFromIds(List<Int32> orgUserIds)
        {
            SecurityRepository securityRepo = new SecurityRepository();
            return securityRepo.GetOrganizationUserByIds(orgUserIds);
        }
        #endregion

        #region [UAT-3045]
        ProfileSharingExpiryContract IClinicalRotationRepository.GetNonComplianceCategoryList(Int32 clinicalRotationID, String delimittedOrgUserIDs, String selectedCategoriesXml)
        {
            ProfileSharingExpiryContract result = new ProfileSharingExpiryContract();
            List<NonComplianceCategoryContract> NonComplianceCategoryContractList = new List<NonComplianceCategoryContract>();
            List<RequirementExpiryCategoryContract> RequirementExpiryCategoryContractList = new List<RequirementExpiryCategoryContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetNonComplianceCategoriesByApplicantIds", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClinicalRotationID", clinicalRotationID);
                command.Parameters.AddWithValue("@DelimittedOrgUserIDs", delimittedOrgUserIDs);
                command.Parameters.AddWithValue("@SelectedCategoriesXml", selectedCategoriesXml);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //Tracking data
                    IEnumerable<DataRow> TrackingRows = ds.Tables[0].AsEnumerable();
                    if (TrackingRows.Count() > 0)
                    {
                        foreach (var item in TrackingRows)
                        {
                            NonComplianceCategoryContract nonComplianceCategoryContract = new NonComplianceCategoryContract();
                            nonComplianceCategoryContract.ApplicantName = item["ApplicantName"].GetType().Name == "DBNull" ? string.Empty : Convert.ToString(item["ApplicantName"]);
                            nonComplianceCategoryContract.Category = item["Category"].GetType().Name == "DBNull" ? string.Empty : Convert.ToString(item["Category"]);
                            NonComplianceCategoryContractList.Add(nonComplianceCategoryContract);
                        }
                    }
                }
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    //Rotation data
                    IEnumerable<DataRow> RequirementRows = ds.Tables[1].AsEnumerable();
                    if (RequirementRows.Count() > 0)
                    {
                        foreach (var item in RequirementRows)
                        {
                            RequirementExpiryCategoryContract requirementExpiryCategoryContract = new RequirementExpiryCategoryContract();
                            requirementExpiryCategoryContract.RotationApplicantName = item["RotationApplicantName"].GetType().Name == "DBNull" ? string.Empty : Convert.ToString(item["RotationApplicantName"]);
                            requirementExpiryCategoryContract.RotationCategory = item["RotationCategory"].GetType().Name == "DBNull" ? string.Empty : Convert.ToString(item["RotationCategory"]);
                            requirementExpiryCategoryContract.RotationItem = item["RotationItem"].GetType().Name == "DBNull" ? string.Empty : Convert.ToString(item["RotationItem"]);
                            requirementExpiryCategoryContract.RotationExpirationDate = item["RotationExpirationDate"].GetType().Name == "DBNull" ? string.Empty : Convert.ToDateTime(item["RotationExpirationDate"]).ToShortDateString();
                            RequirementExpiryCategoryContractList.Add(requirementExpiryCategoryContract);
                        }
                    }
                }

            }

            result.NonComplianceCategoryContractList = NonComplianceCategoryContractList;
            result.RequirementExpiryCategoryContractList = RequirementExpiryCategoryContractList;
            return result;
        }
        #endregion


        DataTable IClinicalRotationRepository.GetTargetReqPackageSubscriptionIDsForSync(Int32 requirementSubscriptionID, Int32 requirementCategoryID)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetTargetReqPackageSubscriptionIDsForSync", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SourcePackageSubscriptionID", requirementSubscriptionID);
                command.Parameters.AddWithValue("@RequirementCategoryID", requirementCategoryID);

                if (con.State == ConnectionState.Closed)
                    con.Open();

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

        DataTable IClinicalRotationRepository.PerformRotationLiveDataMovement(Int32 requirementSubscriptionID, Int32 requirementCategoryID, Int32 currentUserID)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_PerformRotationLiveDataMovement", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SourcePackageSubscriptionID", requirementSubscriptionID);
                command.Parameters.AddWithValue("@RequirementCategoryID", requirementCategoryID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentUserID);

                if (con.State == ConnectionState.Closed)
                    con.Open();

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


        #region UAT-3197, As an Agency User, I should be able to retrieve the syllabus
        /// <summary>
        /// Returns the list of Syllabus Documents for all the rotations.
        /// </summary>
        /// <param name="clientContactID"></param>
        /// <param name="tenantId"></param>
        /// <returns>list of ClientSystemDocuments</returns>
        List<ClientContactSyllabusDocumentContract> IClinicalRotationRepository.GetClinicalRotationSyllabusDocumentsByID(Int32 clinicalRotationID, Int32 additionalDocumentTypeId)
        {



            List<ClientContactSyllabusDocumentContract> clientContactSyllabusDocumentList = new List<ClientContactSyllabusDocumentContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetClinicalRotationSyllabusDocs", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@clinicalRotationID", clinicalRotationID);

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@clinicalRotationID", clinicalRotationID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClinicalRotationSyllabusDocs", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClientContactSyllabusDocumentContract clientContactSyllabusDocument = new ClientContactSyllabusDocumentContract();
                            clientContactSyllabusDocument.DocumentID = dr["DocumentID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["DocumentID"]);
                            clientContactSyllabusDocument.ComplioID = dr["ComplioID"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["ComplioID"]);
                            clientContactSyllabusDocument.RotationName = dr["RotationName"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["RotationName"]);
                            clientContactSyllabusDocument.Department = dr["Department"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Department"]);
                            clientContactSyllabusDocument.Program = dr["Program"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Program"]);
                            clientContactSyllabusDocument.Course = dr["Course"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Course"]);
                            clientContactSyllabusDocument.FileName = dr["FileName"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["FileName"]);
                            clientContactSyllabusDocument.ClinicalRotationID = dr["ClinicalRotationID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ClinicalRotationID"]);
                            if (clientContactSyllabusDocument.listMultipleDocuments.IsNullOrEmpty())
                            {

                                clientContactSyllabusDocument.listMultipleDocuments = GetAdditionalDocumnetDetails(clinicalRotationID, additionalDocumentTypeId);
                            }
                            clientContactSyllabusDocumentList.Add(clientContactSyllabusDocument);

                        }
                    }
                    else
                    {
                        ClientContactSyllabusDocumentContract clientContactSyllabusDocument = new ClientContactSyllabusDocumentContract();
                        if (clientContactSyllabusDocument.listMultipleDocuments.IsNullOrEmpty())
                        {

                            clientContactSyllabusDocument.listMultipleDocuments = GetAdditionalDocumnetDetails(clinicalRotationID, additionalDocumentTypeId);
                        }
                        clientContactSyllabusDocumentList.Add(clientContactSyllabusDocument);
                    }
                }

                base.CloseSQLDataReaderConnection(con);

            }
            return clientContactSyllabusDocumentList;
        }
        #endregion


        #region UAT-3108

        private RotationDetailFieldChanges GenerateDataRotationFieldChanges(ClinicalRotation clinicalRotation, ClinicalRotationDetailContract clinicalRotationDetailContract, Int32 currentLoggedInUserId, Int32 tenantId, Boolean IsUpdatedbyAgencyUser, List<CustomAttribteContract> customAttributeListToUpdate = null, int additionalDocumentTypesId = 0)
        {


            RotationDetailFieldChanges rotationDetailFieldChanges = new RotationDetailFieldChanges();
            System.Text.StringBuilder _sbRotationUpdatedDetails = new System.Text.StringBuilder();
            _sbRotationUpdatedDetails.Append("<h3>Rotation Updated Detail(s):</h3>");
            _sbRotationUpdatedDetails.Append("<div style='line-height:15px'>");
            _sbRotationUpdatedDetails.Append("<ul style='list-style-type: disc'>");

            if (String.Compare(clinicalRotation.CR_RotationName.IsNull() ? String.Empty : clinicalRotation.CR_RotationName, clinicalRotationDetailContract.RotationName.IsNull() ? String.Empty : clinicalRotationDetailContract.RotationName) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;
                if (clinicalRotation.CR_RotationName.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Rotation ID/Name: </b> -NA- " + clinicalRotationDetailContract.RotationName + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Rotation ID/Name: </b>" + "<strike>" + clinicalRotation.CR_RotationName + "</strike>" + " " + clinicalRotationDetailContract.RotationName + "</li>");
                }

            }
            if (String.Compare(clinicalRotation.CR_TypeSpecialty.IsNull() ? String.Empty : clinicalRotation.CR_TypeSpecialty, clinicalRotationDetailContract.TypeSpecialty.IsNull() ? String.Empty : clinicalRotationDetailContract.TypeSpecialty) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;
                if (clinicalRotation.CR_TypeSpecialty.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Type/Specialty: </b> -NA- " + clinicalRotationDetailContract.TypeSpecialty + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Type/Specialty: </b>" + "<strike>" + clinicalRotation.CR_TypeSpecialty + "</strike>" + " " + clinicalRotationDetailContract.TypeSpecialty + "</li>");
                }
            }
            if (String.Compare(clinicalRotation.CR_Department.IsNull() ? String.Empty : clinicalRotation.CR_Department, clinicalRotationDetailContract.Department.IsNull() ? String.Empty : clinicalRotationDetailContract.Department) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;
                if (clinicalRotation.CR_Department.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Department: </b> -NA- " + clinicalRotationDetailContract.Department + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Department: </b>" + "<strike>" + clinicalRotation.CR_Department + "</strike>" + " " + clinicalRotationDetailContract.Department + "</li>");
                }
            }
            if (String.Compare(clinicalRotation.CR_Program.IsNull() ? String.Empty : clinicalRotation.CR_Program, clinicalRotationDetailContract.Program.IsNull() ? String.Empty : clinicalRotationDetailContract.Program) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;
                if (clinicalRotation.CR_Program.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Program: </b> -NA- " + clinicalRotationDetailContract.Program + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Program: </b>" + "<strike>" + clinicalRotation.CR_Program + "</strike>" + " " + clinicalRotationDetailContract.Program + "</li>");
                }
            }
            if (String.Compare(clinicalRotation.CR_Course.IsNull() ? String.Empty : clinicalRotation.CR_Course, clinicalRotationDetailContract.Course.IsNull() ? String.Empty : clinicalRotationDetailContract.Course) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;
                if (clinicalRotation.CR_Course.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Course: </b> -NA- " + clinicalRotationDetailContract.Course + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Course: </b>" + "<strike>" + clinicalRotation.CR_Course + "</strike>" + " " + clinicalRotationDetailContract.Course + "</li>");
                }
            }
            if (String.Compare(clinicalRotation.CR_Term.IsNull() ? String.Empty : clinicalRotation.CR_Term, clinicalRotationDetailContract.Term.IsNull() ? String.Empty : clinicalRotationDetailContract.Term) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;
                if (clinicalRotation.CR_Term.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Term: </b> -NA- " + clinicalRotationDetailContract.Term + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Term: </b>" + "<strike>" + clinicalRotation.CR_Term + "</strike>" + " " + clinicalRotationDetailContract.Term + "</li>");
                }
            }

            if (String.Compare(clinicalRotation.CR_StartDate.ToString().IsNull() ? String.Empty : clinicalRotation.CR_StartDate.ToString(), clinicalRotationDetailContract.StartDate.ToString().IsNull() ? String.Empty : clinicalRotationDetailContract.StartDate.ToString()) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;
                if (clinicalRotation.CR_StartDate.ToString().IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Start Date: </b> -NA- " + clinicalRotationDetailContract.StartDate.Value.ToString("MM/dd/yyyy") + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Start Date: </b>" + "<strike>" + clinicalRotation.CR_StartDate.Value.ToString("MM/dd/yyyy") + "</strike>" + " " + clinicalRotationDetailContract.StartDate.Value.ToString("MM/dd/yyyy") + "</li>");
                }
            }

            if (String.Compare(clinicalRotation.CR_EndDate.ToString().IsNull() ? String.Empty : clinicalRotation.CR_EndDate.ToString(), clinicalRotationDetailContract.EndDate.ToString().IsNull() ? String.Empty : clinicalRotationDetailContract.EndDate.Value.ToString()) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;
                if (clinicalRotation.CR_StartDate.ToString().IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "End Date: </b> -NA- " + clinicalRotationDetailContract.EndDate.Value.ToString("MM/dd/yyyy") + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "End Date: </b>" + "<strike>" + clinicalRotation.CR_EndDate.Value.ToString("MM/dd/yyyy") + "</strike>" + " " + clinicalRotationDetailContract.EndDate.Value.ToString("MM/dd/yyyy") + "</li>");
                }
            }

            if (String.Compare(clinicalRotation.CR_UnitFloorLoc.IsNull() ? String.Empty : clinicalRotation.CR_UnitFloorLoc, clinicalRotationDetailContract.UnitFloorLoc.IsNull() ? String.Empty : clinicalRotationDetailContract.UnitFloorLoc) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;

                if (clinicalRotation.CR_UnitFloorLoc.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Unit/Floor or Location: </b> -NA- " + clinicalRotationDetailContract.UnitFloorLoc + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Unit/Floor or Location: </b>" + "<strike>" + clinicalRotation.CR_UnitFloorLoc + "</strike>" + " " + clinicalRotationDetailContract.UnitFloorLoc + "</li>");
                }
            }
            if ((clinicalRotation.CR_NoOfStudents.IsNullOrEmpty() ? null : clinicalRotation.CR_NoOfStudents) != (clinicalRotationDetailContract.Students.IsNullOrEmpty() ? null : clinicalRotationDetailContract.Students))
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;

                if (clinicalRotation.CR_NoOfStudents.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "# of Students: </b> -NA- " + Convert.ToString(clinicalRotationDetailContract.Students) + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "# of Students: </b>" + "<strike>" + Convert.ToString(clinicalRotation.CR_NoOfStudents) + "</strike>" + " " + Convert.ToString(clinicalRotationDetailContract.Students) + "</li>");
                }

            }
            //if (!clinicalRotation.CR_NoOfHours.Equals(clinicalRotationDetailContract.RecommendedHours))

            if ((clinicalRotation.CR_NoOfHours.IsNullOrEmpty() ? null : clinicalRotation.CR_NoOfHours) != (clinicalRotationDetailContract.RecommendedHours.IsNullOrEmpty() ? null : clinicalRotationDetailContract.RecommendedHours))
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;

                if (clinicalRotation.CR_NoOfHours.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "# of Recommended Hours: </b> -NA- " + Convert.ToString(clinicalRotationDetailContract.RecommendedHours) + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "# of Recommended Hours: </b>" + "<strike>" + Convert.ToString(clinicalRotation.CR_NoOfHours) + "</strike>" + " " + Convert.ToString(clinicalRotationDetailContract.RecommendedHours) + "</li>");
                }

            }
            if (!IsUpdatedbyAgencyUser)
            {
                List<String> lstExistingWeekDays = new List<String>();
                String existingWeekDays = String.Empty;
                List<String> lstNewDays = new List<String>();
                String newWeekDays = String.Empty;

                if (!clinicalRotation.ClinicalRotationDays.IsNullOrEmpty())
                {
                    List<Int32> lstExistingWeekDaysIDs = clinicalRotation.ClinicalRotationDays.Where(con => !con.CRD_IsDeleted && !con.CRD_WeekDayID.IsNullOrEmpty()).Select(sel => sel.CRD_WeekDayID.Value).ToList();
                    if (!lstExistingWeekDaysIDs.IsNullOrEmpty())
                    {
                        lstExistingWeekDays = ClientDBContext.lkpWeekDays.Where(con => lstExistingWeekDaysIDs.Contains(con.WD_ID) && !con.WD_IsDeleted).OrderBy(c => c.WD_ID).Select(sel => sel.WD_Name).ToList();
                        if (!lstExistingWeekDays.IsNullOrEmpty())
                        {
                            existingWeekDays = String.Join(", ", lstExistingWeekDays);
                        }
                    }
                }
                if (!clinicalRotationDetailContract.DaysIdList.IsNullOrEmpty())
                {
                    List<Int32> lstNewWeekDaysIDs = clinicalRotationDetailContract.DaysIdList.Split(',').Select(int.Parse).ToList();
                    if (!lstNewWeekDaysIDs.IsNullOrEmpty())
                    {
                        lstNewDays = ClientDBContext.lkpWeekDays.Where(con => lstNewWeekDaysIDs.Contains(con.WD_ID) && !con.WD_IsDeleted).OrderBy(c => c.WD_ID).Select(sel => sel.WD_Name).ToList();
                        if (!lstNewDays.IsNullOrEmpty())
                        {
                            newWeekDays = String.Join(", ", lstNewDays);
                        }
                    }
                }
                if ((lstExistingWeekDays.IsNullOrEmpty() && lstExistingWeekDays.Count == AppConsts.NONE) && (!lstNewDays.IsNullOrEmpty() || lstNewDays.Count > AppConsts.NONE))
                {
                    //In this case existing days are empty and updated new days in rotation.
                    rotationDetailFieldChanges.NeedToSendEmail = true;
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Days: </b> -NA- " + newWeekDays + "</li>");

                }
                else if ((!lstExistingWeekDays.IsNullOrEmpty() && lstExistingWeekDays.Count > AppConsts.NONE) && (lstNewDays.IsNullOrEmpty() && lstNewDays.Count == AppConsts.NONE))
                {
                    //In this case existing days are not empty and updated new days are empty in rotation.
                    rotationDetailFieldChanges.NeedToSendEmail = true;
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Days: </b>" + "<strike>" + existingWeekDays + "</strike>" + " " + newWeekDays + "</li>");

                }
                else if ((!lstExistingWeekDays.IsNullOrEmpty() && lstExistingWeekDays.Count > AppConsts.NONE) && (!lstNewDays.IsNullOrEmpty() && lstNewDays.Count > AppConsts.NONE))
                {
                    if (String.Compare(existingWeekDays, newWeekDays) != 0)
                    {
                        rotationDetailFieldChanges.NeedToSendEmail = true;
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Days: </b>" + "<strike>" + existingWeekDays + "</strike>" + " " + newWeekDays + "</li>");
                    }
                }
            }

            if (String.Compare(clinicalRotation.CR_RotationShift.IsNull() ? String.Empty : clinicalRotation.CR_RotationShift, clinicalRotationDetailContract.Shift.IsNull() ? String.Empty : clinicalRotationDetailContract.Shift) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;
                if (clinicalRotation.CR_RotationShift.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Shift: </b> -NA- " + Convert.ToString(clinicalRotationDetailContract.Shift) + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Shift: </b>" + "<strike>" + Convert.ToString(clinicalRotation.CR_RotationShift) + "</strike>" + " " + Convert.ToString(clinicalRotationDetailContract.Shift) + "</li>");
                }
            }

            //INSTRUCTOR PECEPTOR CHECK
            if (!IsUpdatedbyAgencyUser)
            {
                List<String> lstExistingClientContacts = new List<String>();
                String existingClientContacts = String.Empty;
                List<String> lstNewClientContacts = new List<String>();
                String newClientContacts = String.Empty;

                if (!clinicalRotation.ClinicalRotationClientContacts.IsNullOrEmpty())
                {
                    List<Int32> lstExistingClientContactsIDs = clinicalRotation.ClinicalRotationClientContacts.Where(con => !con.CRCC_IsDeleted && !con.CRCC_ClientContactID.IsNullOrEmpty()).Select(sel => sel.CRCC_ClientContactID.Value).ToList();
                    if (!lstExistingClientContactsIDs.IsNullOrEmpty())
                    {
                        lstExistingClientContacts = SharedDataDBContext.ClientContacts.Where(con => lstExistingClientContactsIDs.Contains(con.CC_ID) && !con.CC_IsDeleted).OrderBy(o => o.CC_ID).Select(sel => sel.CC_Name).ToList();
                        if (!lstExistingClientContacts.IsNullOrEmpty())
                        {
                            existingClientContacts = String.Join(", ", lstExistingClientContacts);
                        }
                    }
                }
                if (!clinicalRotationDetailContract.ContactIdList.IsNullOrEmpty())
                {
                    List<Int32> lstNewClientContactsIDs = clinicalRotationDetailContract.ContactIdList.Split(',').Select(int.Parse).ToList();
                    if (!lstNewClientContactsIDs.IsNullOrEmpty())
                    {
                        lstNewClientContacts = SharedDataDBContext.ClientContacts.Where(con => lstNewClientContactsIDs.Contains(con.CC_ID) && !con.CC_IsDeleted).OrderBy(o => o.CC_ID).Select(sel => sel.CC_Name).ToList();
                        if (!lstNewClientContacts.IsNullOrEmpty())
                        {
                            newClientContacts = String.Join(", ", lstNewClientContacts);
                        }
                    }
                }
                if ((lstExistingClientContacts.IsNullOrEmpty() || lstExistingClientContacts.Count == AppConsts.NONE) && (!lstNewClientContacts.IsNullOrEmpty() || lstNewClientContacts.Count > AppConsts.NONE))
                {
                    //In this case existing Instructor/Preceptor are empty and updated new days in rotation.
                    rotationDetailFieldChanges.NeedToSendEmail = true;
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Instructor/Preceptor: </b> -NA- " + newClientContacts + "</li>");

                }
                else if ((!lstExistingClientContacts.IsNullOrEmpty() && lstExistingClientContacts.Count > AppConsts.NONE) && (lstNewClientContacts.IsNullOrEmpty() && lstNewClientContacts.Count == AppConsts.NONE))
                {
                    //In this case existing Instructor/Preceptor are not empty and updated instructor preceptor are empty in rotation.
                    rotationDetailFieldChanges.NeedToSendEmail = true;
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Instructor/Preceptor: </b>" + "<strike>" + existingClientContacts + "</strike>" + " " + newClientContacts + "</li>");

                }
                else if ((!lstExistingClientContacts.IsNullOrEmpty() && lstExistingClientContacts.Count > AppConsts.NONE) && (!lstNewClientContacts.IsNullOrEmpty() && lstNewClientContacts.Count > AppConsts.NONE))
                {
                    if (String.Compare(existingClientContacts, newClientContacts) != 0)
                    {
                        rotationDetailFieldChanges.NeedToSendEmail = true;
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Instructor/Preceptor: </b>" + "<strike>" + existingClientContacts + "</strike>" + " " + newClientContacts + "</li>");
                    }
                }

                if ((clinicalRotation.CR_StartTime.IsNullOrEmpty() ? null : clinicalRotation.CR_StartTime) != (clinicalRotationDetailContract.StartTime.IsNullOrEmpty() ? null : clinicalRotationDetailContract.StartTime))
                {
                    rotationDetailFieldChanges.NeedToSendEmail = true;
                    if (clinicalRotation.CR_StartTime.IsNullOrEmpty())
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Start Time: </b> -NA- " + Convert.ToString(clinicalRotationDetailContract.StartTime) + "</li>");
                    }
                    else
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Start Time: </b>" + "<strike>" + Convert.ToString(clinicalRotation.CR_StartTime) + "</strike>" + " " + Convert.ToString(clinicalRotationDetailContract.StartTime) + "</li>");
                    }
                }
                if ((clinicalRotation.CR_EndTime.IsNullOrEmpty() ? null : clinicalRotation.CR_EndTime) != (clinicalRotationDetailContract.EndTime.IsNullOrEmpty() ? null : clinicalRotationDetailContract.EndTime))
                {
                    rotationDetailFieldChanges.NeedToSendEmail = true;

                    if (clinicalRotation.CR_EndTime.IsNullOrEmpty())
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "End Time: </b> -NA- " + Convert.ToString(clinicalRotationDetailContract.EndTime) + "</li>");
                    }
                    else
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "End Time: </b>" + "<strike>" + Convert.ToString(clinicalRotation.CR_EndTime) + "</strike>" + " " + Convert.ToString(clinicalRotationDetailContract.EndTime) + "</li>");
                    }
                }
                if ((clinicalRotation.CR_StartDate.IsNullOrEmpty() ? null : clinicalRotation.CR_StartDate) != (clinicalRotationDetailContract.StartDate.IsNullOrEmpty() ? null : clinicalRotationDetailContract.StartDate))
                {
                    rotationDetailFieldChanges.NeedToSendEmail = true;

                    if (clinicalRotation.CR_StartDate.IsNullOrEmpty())
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Start Date: </b> -NA- " + Convert.ToDateTime(clinicalRotationDetailContract.StartDate).ToString("MM/dd/yyyy") + "</li>");
                    }
                    else
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Start Date: </b>" + "<strike>" + Convert.ToDateTime(clinicalRotation.CR_StartDate).ToString("MM/dd/yyyy") + "</strike>" + " " + Convert.ToDateTime(clinicalRotationDetailContract.StartDate).ToString("MM/dd/yyyy") + "</li>");
                    }
                }
                if ((clinicalRotation.CR_EndDate.IsNullOrEmpty() ? null : clinicalRotation.CR_EndDate) != (clinicalRotationDetailContract.EndDate.IsNullOrEmpty() ? null : clinicalRotationDetailContract.EndDate))
                {
                    rotationDetailFieldChanges.NeedToSendEmail = true;

                    if (clinicalRotation.CR_EndDate.IsNullOrEmpty())
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "End Date: </b> -NA- " + Convert.ToDateTime(clinicalRotationDetailContract.EndDate).ToString("MM/dd/yyyy") + "</li>");
                    }
                    else
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "End Date: </b>" + "<strike>" + Convert.ToDateTime(clinicalRotation.CR_EndDate).ToString("MM/dd/yyyy") + "</strike>" + " " + Convert.ToDateTime(clinicalRotationDetailContract.EndDate).ToString("MM/dd/yyyy") + "</li>");
                    }
                }

                //Syllabus document
                String oldSyllabusDocumentName = String.Empty;
                String newSyllabusDocumentName = Convert.ToString(clinicalRotationDetailContract.SyllabusFileName.IsNullOrEmpty() ? String.Empty : clinicalRotationDetailContract.SyllabusFileName);

                if (!clinicalRotation.ClinicalRotationDocuments.IsNullOrEmpty())
                {
                    oldSyllabusDocumentName = clinicalRotation.ClinicalRotationDocuments.Where(con => !con.CRD_IsDeleted).Select(sel => sel.ClientSystemDocument.CSD_FileName).FirstOrDefault();
                }
                if (!newSyllabusDocumentName.IsNullOrEmpty() && oldSyllabusDocumentName != newSyllabusDocumentName)
                {
                    rotationDetailFieldChanges.NeedToSendEmail = true;
                    if (!oldSyllabusDocumentName.IsNullOrEmpty())
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Syllabus Document: </b>" + "<strike>" + oldSyllabusDocumentName + "</strike>" + " " + newSyllabusDocumentName + "</li>");
                    }
                    else
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Syllabus Document: </b> -NA- " + newSyllabusDocumentName + "</li>");
                    }
                }
                if ((clinicalRotation.CR_DeadlineDate.IsNullOrEmpty() ? null : clinicalRotation.CR_DeadlineDate) != (clinicalRotationDetailContract.DeadlineDate.IsNullOrEmpty() ? null : clinicalRotationDetailContract.DeadlineDate))
                {
                    rotationDetailFieldChanges.NeedToSendEmail = true;

                    if (clinicalRotation.CR_DeadlineDate.IsNullOrEmpty())
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Deadline Date: </b> -NA- " + Convert.ToDateTime(clinicalRotationDetailContract.DeadlineDate).ToString("MM/dd/yyyy") + "</li>");
                    }
                    else if (clinicalRotationDetailContract.DeadlineDate.IsNullOrEmpty())
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Deadline Date: </b>" + "<strike>" + Convert.ToDateTime(clinicalRotation.CR_DeadlineDate).ToString("MM/dd/yyyy") + "</strike>" + " " + String.Empty + "</li>");
                    }
                    else
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Deadline Date: </b>" + "<strike>" + Convert.ToDateTime(clinicalRotation.CR_DeadlineDate).ToString("MM/dd/yyyy") + "</strike>" + " " + Convert.ToDateTime(clinicalRotationDetailContract.DeadlineDate).ToString("MM/dd/yyyy") + "</li>");
                    }
                }
                if (clinicalRotation.CR_IsEditableByClientAdmin.Value.IsNullOrEmpty() ? false : clinicalRotation.CR_IsEditableByClientAdmin.Value != clinicalRotationDetailContract.IsEditableByClientAdmin)
                {
                    rotationDetailFieldChanges.NeedToSendEmail = true;

                    if (clinicalRotation.CR_IsEditableByClientAdmin.Value.IsNullOrEmpty())
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Editable By Client Admin: </b> -NA- " + Convert.ToString(clinicalRotationDetailContract.IsEditableByClientAdmin ? "Yes" : "No") + "</li>");
                    }
                    else
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Editable By Client Admin: </b>" + "<strike>" + Convert.ToString(clinicalRotation.CR_IsEditableByClientAdmin.Value ? "Yes" : "No") + "</strike>" + " " + Convert.ToString(clinicalRotationDetailContract.IsEditableByClientAdmin ? "Yes" : "No") + "</li>");
                    }
                }
                if (clinicalRotation.CR_IsEditableByAgencyUser.Value != clinicalRotationDetailContract.IsEditableByAgencyUser)
                {
                    rotationDetailFieldChanges.NeedToSendEmail = true;

                    if (clinicalRotation.CR_IsEditableByAgencyUser.Value.IsNullOrEmpty())
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Editable By Agency User: </b> -NA- " + Convert.ToString(clinicalRotationDetailContract.IsEditableByAgencyUser ? "Yes" : "No") + "</li>");
                    }
                    else
                    {
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Editable By Agency User: </b>" + "<strike>" + Convert.ToString(clinicalRotation.CR_IsEditableByAgencyUser.Value ? "Yes" : "No") + "</strike>" + " " + Convert.ToString(clinicalRotationDetailContract.IsEditableByAgencyUser ? "Yes" : "No") + "</li>");
                    }
                }

                // 4062 Document Update Name throgh Email...




                if (clinicalRotationDetailContract.listOfMultipleDocument != null && clinicalRotationDetailContract.listOfMultipleDocument.Count() > AppConsts.NONE)
                {
                    rotationDetailFieldChanges.NeedToSendEmail = true;
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Added Additional Document(s): </b> </li>");
                    foreach (var item in clinicalRotationDetailContract.listOfMultipleDocument)
                    {
                        _sbRotationUpdatedDetails.Append("<p style='margin-left:275px'>" + item.AdditionalDocumentFileName + "</p>");
                    }
                }
                var listOfAdditionalDocumentData = GetAdditionalDocumnetDetails(clinicalRotationDetailContract.RotationID, additionalDocumentTypesId);

                string ExistsIds = clinicalRotationDetailContract.ClinicalRotationDocumentUpdatedIds;


                if (listOfAdditionalDocumentData != null && listOfAdditionalDocumentData.Count > AppConsts.NONE)
                {

                    string[] CollectionOfIds = ExistsIds.Split(',');

                    bool IsRemoveAdd = false;
                    foreach (var item in listOfAdditionalDocumentData)
                    {
                        if (!CollectionOfIds.Contains(item.AdditionalDocumentID.ToString()))
                        {
                            if (IsRemoveAdd == false)
                            {
                                _sbRotationUpdatedDetails.Append("<li><b>" + "Removed Additional Document(s): </b> </li>");
                                IsRemoveAdd = true;

                            }
                            _sbRotationUpdatedDetails.Append("<p style='margin-left:275px'><strike>" + item.AdditionalDocumentFileName + "</strike></p>");
                        }
                    }

                }

            }
            _sbRotationUpdatedDetails.Append("</ul>");
            _sbRotationUpdatedDetails.Append("</div>");

            //Nag notification settings
            //Boolean isNagNotificationSettingsHeadingAdded = false;
            //if ((clinicalRotation.CR_DaysBefore.IsNullOrEmpty() ? null : clinicalRotation.CR_DaysBefore) != (clinicalRotationDetailContract.DaysBefore.IsNullOrEmpty() ? null : clinicalRotationDetailContract.DaysBefore))
            //{
            //    rotationDetailFieldChanges.NeedToSendEmail = true;
            //    if (!isNagNotificationSettingsHeadingAdded)
            //    {
            //        _sbRotationUpdatedDetails.Append("<h2>Nag Notification Settings:</h2>");
            //        _sbRotationUpdatedDetails.Append("<div style='line-height:15px'>");
            //        _sbRotationUpdatedDetails.Append("<ul style='list-style-type: disc'>");
            //        isNagNotificationSettingsHeadingAdded = true;
            //    }

            //    if (clinicalRotation.CR_DaysBefore.IsNullOrEmpty())
            //    {
            //        _sbRotationUpdatedDetails.Append("<li><b>" + "Days Before: </b> -NA- " + Convert.ToString(clinicalRotationDetailContract.DaysBefore) + "</li>");
            //    }
            //    else
            //    {
            //        _sbRotationUpdatedDetails.Append("<li><b>" + "Days Before: </b>" + "<strike>" + Convert.ToString(clinicalRotation.CR_DaysBefore) + "</strike>" + " " + Convert.ToString(clinicalRotationDetailContract.DaysBefore) + "</li>");
            //    }
            //}
            //if ((clinicalRotation.CR_Frequency.IsNullOrEmpty() ? null : clinicalRotation.CR_DaysBefore) != (clinicalRotationDetailContract.Frequency.IsNullOrEmpty() ? null : clinicalRotationDetailContract.DaysBefore))
            //{
            //    rotationDetailFieldChanges.NeedToSendEmail = true;
            //    if (!isNagNotificationSettingsHeadingAdded)
            //    {
            //        _sbRotationUpdatedDetails.Append("<h2>Nag Notification Settings:</h2>");
            //        _sbRotationUpdatedDetails.Append("<div style='line-height:15px'>");
            //        _sbRotationUpdatedDetails.Append("<ul style='list-style-type: disc'>");
            //        isNagNotificationSettingsHeadingAdded = true;
            //    }

            //    if (clinicalRotation.CR_Frequency.IsNullOrEmpty())
            //    {
            //        _sbRotationUpdatedDetails.Append("<li><b>" + "Frequency: </b> -NA- " + Convert.ToString(clinicalRotationDetailContract.Frequency) + "</li>");
            //    }
            //    else
            //    {
            //        _sbRotationUpdatedDetails.Append("<li><b>" + "Frequency: </b>" + "<strike>" + Convert.ToString(clinicalRotation.CR_Frequency) + "</strike>" + " " + Convert.ToString(clinicalRotationDetailContract.Frequency) + "</li>");
            //    }
            //}
            //if (isNagNotificationSettingsHeadingAdded)
            //{
            //    _sbRotationUpdatedDetails.Append("</ul>");
            //    _sbRotationUpdatedDetails.Append("</div>");
            //}

            //custom attributes
            if (!customAttributeListToUpdate.IsNullOrEmpty())
            {
                Boolean isCustomAttributesHeadingAdded = false;
                List<Int32> attributeIdsToUpdate = customAttributeListToUpdate.Select(cond => cond.CustomAttributeId).ToList();
                List<CustomAttributeMapping> existingCustomAttributeMappingInDb = GetCustomAttributeMappings(clinicalRotation.CR_ID, attributeIdsToUpdate);
                foreach (CustomAttribteContract customAttributeToSave in customAttributeListToUpdate)
                {
                    CustomAttributeMapping customAttributeMappingToUpdate = existingCustomAttributeMappingInDb.FirstOrDefault(cond =>
                                                                       cond.CAM_CustomAttributeMappingID == customAttributeToSave.CustomAttrMappingId);

                    if (customAttributeMappingToUpdate.IsNotNull())
                    {
                        CustomAttributeValue customAttributeValueToUpdate = customAttributeMappingToUpdate.CustomAttributeValues.FirstOrDefault(cond => !cond.CAV_IsDeleted);
                        if (String.Compare(customAttributeValueToUpdate.CAV_AttributeValue.IsNull() ? String.Empty : customAttributeValueToUpdate.CAV_AttributeValue, customAttributeToSave.CustomAttributeValue.IsNull() ? String.Empty : customAttributeToSave.CustomAttributeValue) != 0)
                        {
                            rotationDetailFieldChanges.NeedToSendEmail = true;
                            if (!isCustomAttributesHeadingAdded)
                            {
                                _sbRotationUpdatedDetails.Append("<h3>Other Detail(s):</h3>");
                                _sbRotationUpdatedDetails.Append("<div style='line-height:15px'>");
                                _sbRotationUpdatedDetails.Append("<ul style='list-style-type: disc'>");
                                isCustomAttributesHeadingAdded = true;
                            }
                            String customAttributeLabel = customAttributeToSave.CustomAttributeLabel.IsNullOrEmpty() ? customAttributeToSave.CustomAttributeName : customAttributeToSave.CustomAttributeLabel;

                            if (customAttributeValueToUpdate.CAV_AttributeValue.IsNullOrEmpty())
                            {
                                _sbRotationUpdatedDetails.Append("<li><b>" + customAttributeLabel + " : </b> -NA- " + Convert.ToString(customAttributeToSave.CustomAttributeValue) + "</li>");
                            }
                            else
                            {
                                _sbRotationUpdatedDetails.Append("<li><b>" + customAttributeLabel + " : </b>" + "<strike>" + Convert.ToString(customAttributeValueToUpdate.CAV_AttributeValue) + "</strike>" + " " + Convert.ToString(customAttributeToSave.CustomAttributeValue) + "</li>");
                            }
                        }
                    }
                }
                if (isCustomAttributesHeadingAdded)
                {
                    _sbRotationUpdatedDetails.Append("</ul>");
                    _sbRotationUpdatedDetails.Append("</div>");
                }
            }
            rotationDetailFieldChanges.RotationFieldChanges = Convert.ToString(_sbRotationUpdatedDetails);
            if (rotationDetailFieldChanges.NeedToSendEmail)
            {
                rotationDetailFieldChanges.RotationID = clinicalRotation.CR_ID;
                rotationDetailFieldChanges.ComplioID = clinicalRotation.CR_ComplioID;
                rotationDetailFieldChanges.TenantID = tenantId;
                rotationDetailFieldChanges.TenantName = ClientDBContext.Tenants.Where(con => con.TenantID == tenantId && !con.IsDeleted).Select(sel => sel.TenantName).FirstOrDefault();
                List<Int32> lstHierarchyNodes = ClientDBContext.ClinicalRotationHierarchyMappings.Where(con => con.CRHM_ClinicalRotationID == clinicalRotation.CR_ID && !con.CRHM_IsDeleted).Select(sel => sel.CRHM_HierarchyNodeID).ToList();
                if (!lstHierarchyNodes.IsNullOrEmpty())
                {
                    rotationDetailFieldChanges.HierarchyNodeIDs = String.Join(",", lstHierarchyNodes);
                }
                if (!currentLoggedInUserId.IsNullOrEmpty() && currentLoggedInUserId > AppConsts.NONE)
                {
                    Entity.OrganizationUser orgUser = SecurityContext.OrganizationUsers.Where(con => con.OrganizationUserID == currentLoggedInUserId && !con.IsDeleted).FirstOrDefault();
                    if (!orgUser.IsNullOrEmpty())
                    {
                        rotationDetailFieldChanges.ModifiedByName = orgUser.FirstName + " " + orgUser.LastName;
                    }
                    rotationDetailFieldChanges.CurrentLoggedInUserId = currentLoggedInUserId;

                }
            }
            return rotationDetailFieldChanges;
        }

        private RotationDetailFieldChanges GenerateDataForRotationSyllDocUpdation(Int32 rotationID, String oldDocumentName, String newDocumentName, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            RotationDetailFieldChanges rotationDetailFieldChanges = new RotationDetailFieldChanges();
            System.Text.StringBuilder _sbRotationUpdatedDetails = new System.Text.StringBuilder();
            _sbRotationUpdatedDetails.Append("<h3>Rotation Updated Detail(s):</h3>");
            _sbRotationUpdatedDetails.Append("<div style='line-height:15px'>");
            _sbRotationUpdatedDetails.Append("<ul style='list-style-type: disc'>");


            if (String.Compare(oldDocumentName.IsNull() ? String.Empty : oldDocumentName, newDocumentName.IsNull() ? String.Empty : newDocumentName) != 0)
            {
                rotationDetailFieldChanges.NeedToSendEmail = true;
                if (!oldDocumentName.IsNullOrEmpty())
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Syllabus Document: </b>" + "<strike>" + oldDocumentName + "</strike>" + " " + newDocumentName + "</li>");
                }
                else
                {
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Syllabus Document: </b> -NA- " + newDocumentName + "</li>");
                }
            }
            _sbRotationUpdatedDetails.Append("</ul>");
            _sbRotationUpdatedDetails.Append("</div>");

            rotationDetailFieldChanges.RotationFieldChanges = Convert.ToString(_sbRotationUpdatedDetails);
            if (rotationDetailFieldChanges.NeedToSendEmail)
            {
                rotationDetailFieldChanges.RotationID = rotationID;
                rotationDetailFieldChanges.ComplioID = ClientDBContext.ClinicalRotations.Where(con => con.CR_ID == rotationID && !con.CR_IsDeleted).Select(sel => sel.CR_ComplioID).FirstOrDefault();
                rotationDetailFieldChanges.TenantID = tenantId;
                rotationDetailFieldChanges.TenantName = ClientDBContext.Tenants.Where(con => con.TenantID == tenantId && !con.IsDeleted).Select(sel => sel.TenantName).FirstOrDefault();
                List<Int32> lstHierarchyNodes = ClientDBContext.ClinicalRotationHierarchyMappings.Where(con => con.CRHM_ClinicalRotationID == rotationID && !con.CRHM_IsDeleted).Select(sel => sel.CRHM_HierarchyNodeID).ToList();
                if (!lstHierarchyNodes.IsNullOrEmpty())
                {
                    rotationDetailFieldChanges.HierarchyNodeIDs = String.Join(",", lstHierarchyNodes);
                }
                if (!currentLoggedInUserId.IsNullOrEmpty() && currentLoggedInUserId > AppConsts.NONE)
                {
                    Entity.OrganizationUser orgUser = SecurityContext.OrganizationUsers.Where(con => con.OrganizationUserID == currentLoggedInUserId && !con.IsDeleted).FirstOrDefault();
                    if (!orgUser.IsNullOrEmpty())
                    {
                        rotationDetailFieldChanges.ModifiedByName = orgUser.FirstName + " " + orgUser.LastName;
                    }
                    rotationDetailFieldChanges.CurrentLoggedInUserId = currentLoggedInUserId;
                }
            }
            return rotationDetailFieldChanges;
        }

        private List<RotationDetailFieldChanges> GenerateDataForRotationAssignPreceptor(List<Int32> lstRotationIds, String clientContactIds, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            List<RotationDetailFieldChanges> lstRotationDetailFieldChanges = new List<RotationDetailFieldChanges>();
            if (lstRotationIds.IsNullOrEmpty() && lstRotationIds.Count == AppConsts.NONE)
                return lstRotationDetailFieldChanges;

            List<String> lstNewClientContacts = new List<String>();
            String newClientContacts = String.Empty;
            String ModifiedByName = String.Empty;

            if (!clientContactIds.IsNullOrEmpty())
            {
                List<Int32> lstNewClientContactsIDs = clientContactIds.Split(',').Select(int.Parse).ToList();
                if (!lstNewClientContactsIDs.IsNullOrEmpty())
                {
                    lstNewClientContacts = SharedDataDBContext.ClientContacts.Where(con => lstNewClientContactsIDs.Contains(con.CC_ID) && !con.CC_IsDeleted).OrderBy(o => o.CC_ID).Select(sel => sel.CC_Name).ToList();
                    if (!lstNewClientContacts.IsNullOrEmpty())
                    {
                        newClientContacts = String.Join(", ", lstNewClientContacts);
                    }
                }
            }
            if (!currentLoggedInUserId.IsNullOrEmpty() && currentLoggedInUserId > AppConsts.NONE)
            {
                Entity.OrganizationUser orgUser = SecurityContext.OrganizationUsers.Where(con => con.OrganizationUserID == currentLoggedInUserId && !con.IsDeleted).FirstOrDefault();
                if (!orgUser.IsNullOrEmpty())
                {
                    ModifiedByName = orgUser.FirstName + " " + orgUser.LastName;
                }
            }

            // List<Int32> lstRotationIds = rotationIds.Split(',').Select(int.Parse).ToList();

            foreach (Int32 rotationID in lstRotationIds)
            {
                RotationDetailFieldChanges rotationDetailFieldChanges = new RotationDetailFieldChanges();
                System.Text.StringBuilder _sbRotationUpdatedDetails = new System.Text.StringBuilder();
                _sbRotationUpdatedDetails.Append("<h3>Rotation Updated Detail(s):</h3>");
                _sbRotationUpdatedDetails.Append("<div style='line-height:15px'>");
                _sbRotationUpdatedDetails.Append("<ul style='list-style-type: disc'>");


                ClinicalRotation clinicalRotation = ClientDBContext.ClinicalRotations.Where(con => con.CR_ID == rotationID && !con.CR_IsDeleted).FirstOrDefault();

                List<String> lstExistingClientContacts = new List<String>();
                String existingClientContacts = String.Empty;

                if (!clinicalRotation.ClinicalRotationClientContacts.IsNullOrEmpty())
                {
                    List<Int32> lstExistingClientContactsIDs = clinicalRotation.ClinicalRotationClientContacts.Where(con => !con.CRCC_IsDeleted && !con.CRCC_ClientContactID.IsNullOrEmpty()).Select(sel => sel.CRCC_ClientContactID.Value).ToList();
                    if (!lstExistingClientContactsIDs.IsNullOrEmpty())
                    {
                        lstExistingClientContacts = SharedDataDBContext.ClientContacts.Where(con => lstExistingClientContactsIDs.Contains(con.CC_ID) && !con.CC_IsDeleted).OrderBy(o => o.CC_ID).Select(sel => sel.CC_Name).ToList();
                        if (!lstExistingClientContacts.IsNullOrEmpty())
                        {
                            existingClientContacts = String.Join(", ", lstExistingClientContacts);
                        }
                    }
                }

                if ((lstExistingClientContacts.IsNullOrEmpty() || lstExistingClientContacts.Count == AppConsts.NONE) && (!lstNewClientContacts.IsNullOrEmpty() || lstNewClientContacts.Count > AppConsts.NONE))
                {
                    //In this case existing Instructor/Preceptor are empty and updated new days in rotation.
                    rotationDetailFieldChanges.NeedToSendEmail = true;
                    _sbRotationUpdatedDetails.Append("<li><b>" + "Instructor/Preceptor: </b> -NA- " + newClientContacts + "</li>");

                }
                else if ((!lstExistingClientContacts.IsNullOrEmpty() && lstExistingClientContacts.Count > AppConsts.NONE) && !lstNewClientContacts.IsNullOrEmpty() && lstNewClientContacts.Count > AppConsts.NONE)
                {
                    if (String.Compare(existingClientContacts, newClientContacts) != 0)
                    {
                        rotationDetailFieldChanges.NeedToSendEmail = true;
                        _sbRotationUpdatedDetails.Append("<li><b>" + "Instructor/Preceptor: </b>" + "<strike>" + existingClientContacts + "</strike>" + " " + newClientContacts + "</li>");
                    }
                }

                _sbRotationUpdatedDetails.Append("</ul>");
                _sbRotationUpdatedDetails.Append("</div>");

                rotationDetailFieldChanges.RotationFieldChanges = Convert.ToString(_sbRotationUpdatedDetails);
                if (rotationDetailFieldChanges.NeedToSendEmail)
                {
                    rotationDetailFieldChanges.RotationID = rotationID;
                    rotationDetailFieldChanges.ComplioID = clinicalRotation.CR_ComplioID;
                    rotationDetailFieldChanges.TenantID = tenantId;
                    rotationDetailFieldChanges.TenantName = ClientDBContext.Tenants.Where(con => con.TenantID == tenantId && !con.IsDeleted).Select(sel => sel.TenantName).FirstOrDefault();
                    List<Int32> lstHierarchyNodes = ClientDBContext.ClinicalRotationHierarchyMappings.Where(con => con.CRHM_ClinicalRotationID == rotationID && !con.CRHM_IsDeleted).Select(sel => sel.CRHM_HierarchyNodeID).ToList();
                    if (!lstHierarchyNodes.IsNullOrEmpty())
                    {
                        rotationDetailFieldChanges.HierarchyNodeIDs = String.Join(",", lstHierarchyNodes);
                    }
                    rotationDetailFieldChanges.ModifiedByName = ModifiedByName;

                    rotationDetailFieldChanges.CurrentLoggedInUserId = currentLoggedInUserId;
                }
                lstRotationDetailFieldChanges.Add(rotationDetailFieldChanges);
            }
            return lstRotationDetailFieldChanges;
        }

        #endregion

        List<ApplicantDataListContract> IClinicalRotationRepository.GetRotationMembersForRotationDocs(ClinicalRotationSearchContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            List<ApplicantDataListContract> applicantDataContractList = new List<ApplicantDataListContract>();

            string orderBy = "ApplicantFirstName";
            string ordDirection = "ASC";

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "DESC";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@FirstName", searchDataContract.ApplicantFirstName),
                             new SqlParameter("@LastName", searchDataContract.ApplicantLastName),
                             new SqlParameter("@EmailAddress", searchDataContract.EmailAddress),
                             new SqlParameter("@SSN", searchDataContract.ApplicantSSN),
                             new SqlParameter("@DOB", searchDataContract.DateOfBirth),
                             new SqlParameter("@OrderBy", orderBy),
                             new SqlParameter("@OrderDirection", ordDirection),
                             new SqlParameter("@PageIndex", gridCustomPaging.CurrentPageIndex),
                             new SqlParameter("@PageSize", gridCustomPaging.PageSize),
                             new SqlParameter("@ClinicalRotationID", searchDataContract.ClinicalRotationID),
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationMembersForRotationDocs", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ApplicantDataListContract applicantData = new ApplicantDataListContract();
                            applicantData.OrganizationUserId = dr["OrganizationUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OrganizationUserID"]);
                            applicantData.ApplicantFirstName = Convert.ToString(dr["FirstName"]);
                            applicantData.ApplicantLastName = Convert.ToString(dr["LastName"]);
                            applicantData.DateOfBirth = dr["DOB"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DOB"]);
                            applicantData.EmailAddress = Convert.ToString(dr["PrimaryEmailAddress"]);
                            applicantData.SSN = Convert.ToString(dr["SSN"]);
                            applicantData.TotalCount = dr["TotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TotalCount"]);
                            applicantDataContractList.Add(applicantData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return applicantDataContractList;
        }

        List<RequirementCategoryContract> IClinicalRotationRepository.GetReqPkgCatByRotationID(Int32 clinicalRotationID)
        {
            List<RequirementCategoryContract> lstReqCat = new List<RequirementCategoryContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ClinicalRotationID", clinicalRotationID),
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetReqPkgCatByRotationID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementCategoryContract reqCat = new RequirementCategoryContract();
                            reqCat.RequirementCategoryID = dr["ReqCatID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ReqCatID"]);
                            reqCat.RequirementCategoryName = Convert.ToString(dr["ReqCatDisplayName"]);
                            lstReqCat.Add(reqCat);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstReqCat;
        }

        List<RotationDocumentContact> IClinicalRotationRepository.GetApplicantDocsByReqCatID(string reqCatIDs, string applicantIds, CustomPagingArgsContract gridCustomPaging)
        {
            List<RotationDocumentContact> lstRotationDocument = new List<RotationDocumentContact>();

            string orderBy = "FirstName";
            string ordDirection = "ASC";

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "DESC";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ReqCatIDs", reqCatIDs),
                             new SqlParameter("@ApplicantIds", applicantIds),
                             new SqlParameter("@OrderBy", orderBy),
                             new SqlParameter("@OrderDirection", ordDirection),
                             new SqlParameter("@PageIndex", gridCustomPaging.CurrentPageIndex),
                             new SqlParameter("@PageSize", gridCustomPaging.PageSize),
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAppDocsByReqCatID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RotationDocumentContact rotDoc = new RotationDocumentContact();
                            rotDoc.OrganizationUserID = dr["OrganizationUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OrganizationUserID"]);
                            rotDoc.FirstName = Convert.ToString(dr["FirstName"]);
                            rotDoc.LastName = Convert.ToString(dr["LastName"]);
                            rotDoc.ReqCatName = Convert.ToString(dr["ReqCatDisplayName"]);
                            rotDoc.ReqCatID = dr["ReqCatID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ReqCatID"]);
                            rotDoc.ApplicantDocumentID = dr["ApplicantDocumentID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ApplicantDocumentID"]);
                            rotDoc.TotalCount = dr["TotalCount"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["TotalCount"]);
                            rotDoc.DocumentPath = Convert.ToString(dr["DocumentPath"]);
                            rotDoc.DocumentName = Convert.ToString(dr["FileName"]);
                            lstRotationDocument.Add(rotDoc);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstRotationDocument;
        }


        List<RotationDocumentContact> IClinicalRotationRepository.GetApplicantDocumentsByDocIDs(string applicantDocIds, string reqCatIds)
        {
            List<RotationDocumentContact> lstRotationDocument = new List<RotationDocumentContact>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@AppDocIds", applicantDocIds)
                             ,new SqlParameter("@ReqCatIds", reqCatIds)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetApplicantDocumentsByDocIDs", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RotationDocumentContact rotDoc = new RotationDocumentContact();
                            rotDoc.OrganizationUserID = dr["OrganizationUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OrganizationUserID"]);
                            rotDoc.ApplicantFullName = Convert.ToString(dr["ApplicantFullName"]);
                            rotDoc.ReqCatName = Convert.ToString(dr["ReqCatDisplayName"]);
                            rotDoc.ApplicantDocumentID = dr["ApplicantDocumentID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ApplicantDocumentID"]);
                            rotDoc.DocumentPath = Convert.ToString(dr["DocumentPath"]);
                            rotDoc.DocumentName = Convert.ToString(dr["FileName"]);
                            lstRotationDocument.Add(rotDoc);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstRotationDocument;
        }

        #region UAT-3222
        private RotationStudentDropped GenerateDataForStudentDroppedFromRotation(Int32 clinicalRotationID, List<Int32> clinicalMemberIds, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            try
            {
                RotationStudentDropped rotationStudentDropped = new RotationStudentDropped();
                rotationStudentDropped.RotationID = clinicalRotationID;
                rotationStudentDropped.TenantID = tenantId;
                rotationStudentDropped.CurrentLoggedInUserId = currentLoggedInUserId;
                rotationStudentDropped.ComplioID = ClientDBContext.ClinicalRotations.Where(con => con.CR_ID == clinicalRotationID && !con.CR_IsDeleted).Select(sel => sel.CR_ComplioID).FirstOrDefault();
                rotationStudentDropped.TenantName = ClientDBContext.Tenants.Where(con => con.TenantID == tenantId && !con.IsDeleted).Select(sel => sel.TenantName).FirstOrDefault();
                rotationStudentDropped.AgencyId = ClientDBContext.ClinicalRotationAgencies.Where(con => !con.CRA_IsDeleted && con.CRA_ClinicalRotationID == clinicalRotationID).Select(sel => sel.CRA_AgencyID).FirstOrDefault();
                List<Int32> lstHierarchyNodes = ClientDBContext.ClinicalRotationHierarchyMappings.Where(con => con.CRHM_ClinicalRotationID == clinicalRotationID && !con.CRHM_IsDeleted).Select(sel => sel.CRHM_HierarchyNodeID).ToList();
                if (!lstHierarchyNodes.IsNullOrEmpty())
                {
                    rotationStudentDropped.HierarchyNodeIDs = String.Join(",", lstHierarchyNodes);
                }

                String removedApplicantNames = String.Empty;

                List<OrganizationUser> lstOrganizationUser = ClientDBContext.ClinicalRotationMembers.Where(con => clinicalMemberIds.Contains(con.CRM_ID) && !con.CRM_IsDeleted).Select(sel => sel.OrganizationUser).ToList();

                if (!lstOrganizationUser.IsNullOrEmpty())
                {
                    //lstOrganizationUser = lstOrganizationUser.Where(con => !con.IsDeleted).ToList();

                    foreach (OrganizationUser item in lstOrganizationUser)
                    {
                        removedApplicantNames += item.FirstName + " " + item.LastName + ", ";
                    }
                    removedApplicantNames = removedApplicantNames.Remove(removedApplicantNames.Length - 2, 2);
                }
                rotationStudentDropped.RemovedApplicantNames = removedApplicantNames;

                return rotationStudentDropped;
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


        #region UAT-3315
        List<ApplicantDocumentContract> IClinicalRotationRepository.GetSelectedBadgeDocumentsToExport(String studentIds, Int32 loggedInUserIds)
        {
            List<ApplicantDocumentContract> lstApplicantBadgeDocument = new List<ApplicantDocumentContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@OrganisationUserIds", studentIds)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetUserBadgeFormDocuments", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ApplicantDocumentContract applicantBadgeDoc = new ApplicantDocumentContract();
                            applicantBadgeDoc.ApplicantId = dr["OrganizationUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OrganizationUserID"]);
                            applicantBadgeDoc.DocumentPath = Convert.ToString(dr["DocumentPath"]);
                            applicantBadgeDoc.FileName = Convert.ToString(dr["FileName"]);
                            applicantBadgeDoc.ApplicantDocumentId = dr["ApplicantDocumentID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ApplicantDocumentID"]);
                            applicantBadgeDoc.DocumentPath = Convert.ToString(dr["DocumentPath"]);
                            applicantBadgeDoc.ApplicantName = Convert.ToString(dr["ApplicantName"]);
                            lstApplicantBadgeDocument.Add(applicantBadgeDoc);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstApplicantBadgeDocument;
        }
        #endregion

        #region UAT-3364
        Int32 IClinicalRotationRepository.GetRotationCreatorByRotationID(Int32 rotationID)
        {
            return _dbContext.ClinicalRotations.Where(d => d.CR_ID == rotationID).Select(s => s.CR_CreatedByID).FirstOrDefault();
        }
        #endregion

        #region UAT-3458
        List<RequirementExpiringItemListContract> IClinicalRotationRepository.GetRequirementItemsAboutToExpire(Int32 requirementPackageSubscriptionId)
        {
            List<RequirementExpiringItemListContract> lstRequirementExpiringItemListContract = new List<RequirementExpiringItemListContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@RequirementPackageSubscriptionId", requirementPackageSubscriptionId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementItemsAboutToExpire", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementExpiringItemListContract requirementExpiringItemListContract = new RequirementExpiringItemListContract();
                            requirementExpiringItemListContract.ARID_RequirementItemID = dr["ARID_RequirementItemID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ARID_RequirementItemID"]);
                            requirementExpiringItemListContract.ItemRequirementStatus = Convert.ToString(dr["ItemRequirementStatus"]);
                            requirementExpiringItemListContract.ShowUpdateDelete = Convert.ToBoolean(dr["ShowUpdateDelete"]);
                            lstRequirementExpiringItemListContract.Add(requirementExpiringItemListContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstRequirementExpiringItemListContract;
        }
        #endregion

        #region UAT-3485
        List<RequirementItemsAboutToExpireContract> IClinicalRotationRepository.GetExpiringRequirementItems(Int32 subEventId, Int32 chunkSize)
        {
            List<RequirementItemsAboutToExpireContract> lstRequirementItemsAboutToExpireContract = new List<RequirementItemsAboutToExpireContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@SubEventId", subEventId),
                             new SqlParameter("@ChunkSize", chunkSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetReqItemsAboutToExpireNotificationData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementItemsAboutToExpireContract requirementItemsAboutToExpireContract = new RequirementItemsAboutToExpireContract();
                            requirementItemsAboutToExpireContract.UserFirstName = dr["FirstName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["FirstName"]);
                            requirementItemsAboutToExpireContract.UserLastName = dr["LastName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["LastName"]);
                            requirementItemsAboutToExpireContract.PrimaryEmailaddress = dr["EmailID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["EmailID"]);
                            requirementItemsAboutToExpireContract.RequirementItemName = dr["ItemName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ItemName"]);
                            requirementItemsAboutToExpireContract.ItemExpirationDate = dr["ItemExpirationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ItemExpirationDate"]);
                            requirementItemsAboutToExpireContract.RequirementItemID = dr["RequirementItemID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementItemID"]);
                            requirementItemsAboutToExpireContract.ApplicantRequirementItemID = dr["ApplicantRequirementItemID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ApplicantRequirementItemID"]);
                            requirementItemsAboutToExpireContract.OrgUserId = dr["OrgUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OrgUserID"]);
                            requirementItemsAboutToExpireContract.RotationHierachyIds = dr["RotationHierachyIds"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationHierachyIds"]);
                            requirementItemsAboutToExpireContract.ComplioID = dr["ComplioID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ComplioID"]); //UAT:4619
                            lstRequirementItemsAboutToExpireContract.Add(requirementItemsAboutToExpireContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstRequirementItemsAboutToExpireContract;
        }

        #endregion


        #region UAT-3137
        List<RequirementCategoriesBeforeGoingToBeRequiredContract> IClinicalRotationRepository.GetRequirementCategoriesBeforeGoingToBeRequired(Int32 subEventId, Int32 chunkSize)
        {
            List<RequirementCategoriesBeforeGoingToBeRequiredContract> lstRequirementCategoriesBeforeFallOutComplianceContract = new List<RequirementCategoriesBeforeGoingToBeRequiredContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@SubEventId", subEventId),
                             new SqlParameter("@ChunkSize", chunkSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotCategoriesBeforeGoingToBeRequiredNotificationData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementCategoriesBeforeGoingToBeRequiredContract requirementCategoriesBeforeFallOutComplianceContract = new RequirementCategoriesBeforeGoingToBeRequiredContract();
                            requirementCategoriesBeforeFallOutComplianceContract.UserFirstName = dr["FirstName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["FirstName"]);
                            requirementCategoriesBeforeFallOutComplianceContract.UserLastName = dr["LastName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["LastName"]);
                            requirementCategoriesBeforeFallOutComplianceContract.PrimaryEmailaddress = dr["EmailID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["EmailID"]);
                            requirementCategoriesBeforeFallOutComplianceContract.RequirementCategoryName = dr["CategoryName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["CategoryName"]);
                            requirementCategoriesBeforeFallOutComplianceContract.CategoryRequiredDate = dr["CategoryRequiredDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CategoryRequiredDate"]);
                            requirementCategoriesBeforeFallOutComplianceContract.RequirementCategoryID = dr["RequirementCategoryID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementCategoryID"]);
                            requirementCategoriesBeforeFallOutComplianceContract.ApplicantRequirementCategoryID = dr["ApplicantRequirementCategoryID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["ApplicantRequirementCategoryID"]);
                            requirementCategoriesBeforeFallOutComplianceContract.OrgUserId = dr["OrgUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OrgUserID"]);
                            requirementCategoriesBeforeFallOutComplianceContract.RotationHierachyIds = dr["RotationHierachyIds"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationHierachyIds"]);
                            requirementCategoriesBeforeFallOutComplianceContract.ComplioID = dr["ComplioID"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["ComplioID"]);
                            requirementCategoriesBeforeFallOutComplianceContract.RotationName = dr["RotationName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RotationName"]);
                            lstRequirementCategoriesBeforeFallOutComplianceContract.Add(requirementCategoriesBeforeFallOutComplianceContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstRequirementCategoriesBeforeFallOutComplianceContract;
        }

        #endregion



        void IClinicalRotationRepository.AutomaticallyArchiveRotation()
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_AutomaticallyArchivedRotation", con);
                command.CommandType = CommandType.StoredProcedure;
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }

        }

        #region UAT-4147
        List<ClinicalRotationMembersContract> IClinicalRotationRepository.IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(string rotationIDs, int tenantID, string selectedOrgUserIDs, string selectedClientContactIDs)
        {
            List<ClinicalRotationMembersContract> lstClinicalRotationMembersContract = new List<ClinicalRotationMembersContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@ClinicalRotationIDs", rotationIDs)
                            ,new SqlParameter("@OrganizationUserIDs", selectedOrgUserIDs)
                            ,new SqlParameter("@ClientContactIDs", selectedClientContactIDs)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_FetchUsersAlreadyExistsInRotation", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationMembersContract objClinicalRotationMembersContract = new ClinicalRotationMembersContract();
                            objClinicalRotationMembersContract.RotationID = Convert.ToInt32(dr["RotationID"]);
                            objClinicalRotationMembersContract.RotationName = Convert.ToString(dr["RotationName"]);
                            objClinicalRotationMembersContract.ComplioID = Convert.ToString(dr["ComplioID"]);
                            objClinicalRotationMembersContract.IsApplicant = Convert.ToBoolean(dr["IsApplicant"]);
                            objClinicalRotationMembersContract.UserName = Convert.ToString(dr["UserName"]);
                            lstClinicalRotationMembersContract.Add(objClinicalRotationMembersContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

                return lstClinicalRotationMembersContract;
            }
        }
        #endregion

        #region UAT-4323
        List<ClinicalRotationDetailContract> IClinicalRotationRepository.GetApplicantDetailsForSelectedRotations(string rotationIDs, int tenantID)
        {
            List<ClinicalRotationDetailContract> lstClinicalRotationDetailContract = new List<ClinicalRotationDetailContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@ClinicalRotationIDs", rotationIDs)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetApplicantDetailsForSelectedRotations", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClinicalRotationDetailContract objClinicalRotationMembersContract = new ClinicalRotationDetailContract();
                            objClinicalRotationMembersContract.RotationID = Convert.ToInt32(dr["RotationID"]);
                            objClinicalRotationMembersContract.RotationName = Convert.ToString(dr["RotationName"]);
                            objClinicalRotationMembersContract.ComplioID = Convert.ToString(dr["ComplioID"]);
                            objClinicalRotationMembersContract.Students = dr["NoOfStudents"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfStudents"]));
                            objClinicalRotationMembersContract.ApplicantCount = Convert.ToInt32(dr["ApplicantCount"]);
                            lstClinicalRotationDetailContract.Add(objClinicalRotationMembersContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

                return lstClinicalRotationDetailContract;
            }
        }
        #endregion

        #region UAT-4561
        private Boolean IsAnyMemberInRotationIsApprvdOrNotApprvd(Int32 clinicalRotationId, Int32 tenantId)
        {
            Int32 approvedInvitationStatusID = SharedDataDBContext.lkpSharedUserInvitationReviewStatus.Where(con => con.SUIRS_Code == "AAAB" && !con.SUIRS_IsDeleted).FirstOrDefault().SUIRS_ID;
            Int32 notApprovedInvitationStatusID = SharedDataDBContext.lkpSharedUserInvitationReviewStatus.Where(con => con.SUIRS_Code == "AAAC" && !con.SUIRS_IsDeleted).FirstOrDefault().SUIRS_ID;

            List<Int32> lstPsids = new List<int>();
            SharedDataDBContext.ProfileSharingInvitationGroups.Where(con => !con.PSIG_IsDeleted && con.PSIG_ClinicalRotationID == clinicalRotationId && con.PSIG_TenantID == tenantId)
                                 .OrderByDescending(x => x.PSIG_ID).ToList().ForEach(x =>
                     {
                         lstPsids.AddRange(x.ProfileSharingInvitations.Where(c => !c.PSI_IsDeleted).Select(psi => psi.PSI_ID).ToList());
                     });

            if (!lstPsids.IsNullOrEmpty() && lstPsids.Count > AppConsts.NONE)
            {

                return SharedDataDBContext.SharedUserInvitationReviews.Where(con => !con.SUIR_IsDeleted && lstPsids.Contains(con.SUIR_ProfileSharingInvitationID)
                                            && (con.SUIR_ReviewStatusID == approvedInvitationStatusID || con.SUIR_ReviewStatusID == notApprovedInvitationStatusID)).Any();
            }
            return false;
        }

        #endregion

        #region MyRegion

        List<ClinicalRotationRequirementPackage> IClinicalRotationRepository.GetReqPackagesByRotId(Int32 rotationId)
        {
            return _dbContext.ClinicalRotationRequirementPackages.Where(c => !c.CRRP_IsDeleted
                                                                      && c.CRRP_ClinicalRotationID == rotationId).ToList();
        }



        #endregion
    }
}
