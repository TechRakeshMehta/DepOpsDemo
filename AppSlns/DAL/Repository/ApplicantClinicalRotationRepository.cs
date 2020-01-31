using DAL.Interfaces;
using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using System.Data.Entity.Core.EntityClient;
using INTSOF.Utils;

namespace DAL.Repository
{
    public class ApplicantClinicalRotationRepository : ClientBaseRepository, IApplicantClinicalRotationRepository
    {
        #region Variables

        #region public Variables

        #endregion

        #region Private Variables

        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public ApplicantClinicalRotationRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        #endregion

        #region Properties

        #region public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Check whether an applicant is a member of any Rotation or not
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="applicantOrgUserID"></param>
        /// <returns>Returns True if an applicant is a member of any Rotation else False</returns>
        Boolean IApplicantClinicalRotationRepository.IsApplicantClinicalRotationMember(Int32 applicantOrgUserID)
        {
            return _dbContext.ClinicalRotationMembers.Any(cond => cond.CRM_ApplicantOrgUserID == applicantOrgUserID && cond.CRM_IsDeleted == false);
        }


        #region Clinical Rotation

        /// <summary>
        /// Get Applicant Rotations for Listing
        /// </summary>
        /// <param name="applicantOrgUserId"></param>
        /// <param name="searchDataContract"></param>
        /// <returns></returns>
        List<ClinicalRotationDetailContract> IApplicantClinicalRotationRepository.GetApplicantClinicalRotations(Int32 applicantOrgUserId, ClinicalRotationDetailContract searchDataContract)
        {
            var _lstClinicalRotationDetailContract = new List<ClinicalRotationDetailContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ApplicantOrgUserId", applicantOrgUserId),
                           new SqlParameter("@Department", searchDataContract.Department),
                           new SqlParameter("@Program", searchDataContract.Program),
                           new SqlParameter("@Course", searchDataContract.Course),
                           new SqlParameter("@AgencyId", searchDataContract.AgencyID),
                           new SqlParameter("@ClientContactIds", searchDataContract.ContactIdList),
                           new SqlParameter("@StatusTypeCode", searchDataContract.StatusTypeCode)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetApplicantRotations", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var crDetailContract = new ClinicalRotationDetailContract();
                            crDetailContract.PkgSubscriptionId = dr["SubscriptionId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["SubscriptionId"]);
                            crDetailContract.RotationID = Convert.ToInt32(dr["RotationId"]);
                            crDetailContract.RotationName = dr["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationName"]);
                            crDetailContract.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            crDetailContract.Department = dr["Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Department"]);
                            crDetailContract.Program = dr["Program"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Program"]);
                            crDetailContract.Course = dr["Course"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Course"]);
                            crDetailContract.UnitFloorLoc = dr["UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UnitFloorLoc"]);
                            crDetailContract.RecommendedHours = dr["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfHours"]));
                            crDetailContract.DaysName = dr["DaysName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DaysName"]);
                            crDetailContract.Shift = dr["RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationShift"]);
                            crDetailContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            crDetailContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            crDetailContract.StartTime = dr["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["StartTime"]);
                            crDetailContract.EndTime = dr["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["EndTime"]);
                            crDetailContract.ContactNames = dr["ContactName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContactName"]);
                            crDetailContract.RequirementPackageStatusDesc = dr["RequirementPackageStatusDesc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageStatusDesc"]);
                            crDetailContract.RequirementPackageStatusCode = dr["RequirementPackageStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageStatusCode"]);
                            crDetailContract.ComplioID = dr["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplioID"]); //UAT:4619
                            crDetailContract.ReviewStatus = dr["SharedReviewStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SharedReviewStatus"]);
                            _lstClinicalRotationDetailContract.Add(crDetailContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstClinicalRotationDetailContract;
        }

        /// <summary>
        /// Get Applicant Clinical Rotation Details
        /// </summary>
        /// <param name="applicantOrgUserId"></param>
        /// <param name="loggedInOrgUserID"></param>
        /// <returns></returns>
        List<ClinicalRotationDetailContract> IApplicantClinicalRotationRepository.GetApplicantClinicalRotationDetails(Int32 applicantOrgUserId, Int32? loggedInOrgUserID)
        {
            var _lstClinicalRotationDetailContract = new List<ClinicalRotationDetailContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ApplicantOrgUserId", applicantOrgUserId),
                           new SqlParameter("@LoggedInOrgUserID", loggedInOrgUserID),
                           new SqlParameter("@DroppedStatus", AppConsts.APPLICANT_DROPPED_STATUS)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetApplicantRotationDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var crDetailContract = new ClinicalRotationDetailContract();

                            crDetailContract.PkgSubscriptionId = dr["SubscriptionId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["SubscriptionId"]);
                            crDetailContract.RotationID = Convert.ToInt32(dr["RotationId"]);
                            crDetailContract.RotationName = dr["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationName"]);
                            crDetailContract.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            crDetailContract.Department = dr["Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Department"]);
                            crDetailContract.Program = dr["Program"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Program"]);
                            crDetailContract.Course = dr["Course"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Course"]);
                            crDetailContract.UnitFloorLoc = dr["UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UnitFloorLoc"]);
                            crDetailContract.RecommendedHours = dr["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfHours"]));
                            crDetailContract.DaysName = dr["DaysName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DaysName"]);
                            crDetailContract.Shift = dr["RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationShift"]);
                            crDetailContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            crDetailContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            crDetailContract.StartTime = dr["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["StartTime"]);
                            crDetailContract.EndTime = dr["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["EndTime"]);
                            crDetailContract.ContactNames = dr["ContactName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContactName"]);
                            crDetailContract.RequirementPackageStatusDesc = dr["RequirementPackageStatusName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageStatusName"]);
                            crDetailContract.RequirementPackageStatusCode = dr["RequirementPackageStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageStatusCode"]);
                            crDetailContract.ComplioID = dr["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplioID"]);
                            crDetailContract.TypeSpecialty = dr["TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TypeSpecialty"]);
                            crDetailContract.Term = dr["Term"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Term"]);
                            crDetailContract.Students = dr["NoOfStudents"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfStudents"]));
                            crDetailContract.HierarchyNodes = dr["HierarchyNodes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodes"]);
                            crDetailContract.SharedUserInvitationReviewStatusName = dr["SharedUserInvitationReviewStatusName"] == DBNull.Value ? "Pending Share" : Convert.ToString(dr["SharedUserInvitationReviewStatusName"]);
                            _lstClinicalRotationDetailContract.Add(crDetailContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstClinicalRotationDetailContract;
        }

        #region "UAT-2544"

        public Boolean IsApplicantDropped(int ApplicantOrgUserID, int clinicalRotationID)
        {
            var result = _dbContext.ClinicalRotationMembers.Where(x => x.CRM_ApplicantOrgUserID == ApplicantOrgUserID && x.CRM_ClinicalRotationID == clinicalRotationID && !x.CRM_IsDeleted).FirstOrDefault();
            if (result != null)
                return result.CRM_IsDropped;
            else
                return false;
        }

        #endregion

        #endregion

        #endregion

        #region Private Methods

        #endregion

        #endregion

        #region UAT-4248
        List<ClinicalRotationDetailContract> IApplicantClinicalRotationRepository.GetInstructorClinicalRotationDetails(Int32 instructorOrgUserId, Int32? loggedInOrgUserID)
        {
            var _lstClinicalRotationDetailContract = new List<ClinicalRotationDetailContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@InstructorOrgUserId", instructorOrgUserId),
                           new SqlParameter("@LoggedInOrgUserID", loggedInOrgUserID),
                           new SqlParameter("@DroppedStatus", AppConsts.APPLICANT_DROPPED_STATUS)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetInstructorRotationDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var crDetailContract = new ClinicalRotationDetailContract();

                            crDetailContract.PkgSubscriptionId = dr["SubscriptionId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["SubscriptionId"]);
                            crDetailContract.RotationID = Convert.ToInt32(dr["RotationId"]);
                            crDetailContract.RotationName = dr["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationName"]);
                            crDetailContract.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            crDetailContract.Department = dr["Department"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Department"]);
                            crDetailContract.Program = dr["Program"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Program"]);
                            crDetailContract.Course = dr["Course"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Course"]);
                            crDetailContract.UnitFloorLoc = dr["UnitFloorLoc"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UnitFloorLoc"]);
                            crDetailContract.RecommendedHours = dr["NoOfHours"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfHours"]));
                            crDetailContract.DaysName = dr["DaysName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DaysName"]);
                            crDetailContract.Shift = dr["RotationShift"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationShift"]);
                            crDetailContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            crDetailContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            crDetailContract.StartTime = dr["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["StartTime"]);
                            crDetailContract.EndTime = dr["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan)(dr["EndTime"]);
                            crDetailContract.ContactNames = dr["ContactName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContactName"]);
                            crDetailContract.RequirementPackageStatusDesc = dr["RequirementPackageStatusName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageStatusName"]);
                            crDetailContract.RequirementPackageStatusCode = dr["RequirementPackageStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageStatusCode"]);
                            crDetailContract.ComplioID = dr["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplioID"]);
                            crDetailContract.TypeSpecialty = dr["TypeSpecialty"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TypeSpecialty"]);
                            crDetailContract.Term = dr["Term"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Term"]);
                            crDetailContract.Students = dr["NoOfStudents"] == DBNull.Value ? (float?)null : (float?)(Convert.ToDouble(dr["NoOfStudents"]));
                            crDetailContract.HierarchyNodes = dr["HierarchyNodes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodes"]);
                            crDetailContract.SharedUserInvitationReviewStatusName = dr["SharedUserInvitationReviewStatusName"] == DBNull.Value ? "Pending Share" : Convert.ToString(dr["SharedUserInvitationReviewStatusName"]);
                            _lstClinicalRotationDetailContract.Add(crDetailContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstClinicalRotationDetailContract;
        }
        #endregion
        #region UAT-4313
        List<ClientContactNotesContract> IApplicantClinicalRotationRepository.GetClientContactNotes(Int32 clientContactId)
        {
            var _lstClientContactNotesContract = new List<ClientContactNotesContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ClientContactId", clientContactId),
                          // new SqlParameter("@LoggedInOrgUserID", loggedInOrgUserID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetClientContactNotes", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var ccNotesContract = new ClientContactNotesContract();

                            ccNotesContract.NoteId = Convert.ToInt32(dr["NoteId"]);
                            ccNotesContract.Notes = dr["Notes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Notes"]);
                            ccNotesContract.NotesCreatedBy = dr["NotesCreatedBy"] == DBNull.Value ? String.Empty : Convert.ToString(dr["NotesCreatedBy"]);
                            ccNotesContract.NoteCreatedOn = Convert.ToDateTime(dr["NoteCreatedOn"]);
                            _lstClientContactNotesContract.Add(ccNotesContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstClientContactNotesContract;
        }
        Boolean IApplicantClinicalRotationRepository.SaveClientContactNotes(ClientContactNotesContract ccNotesContract, Int32 currentLoggedInUserId)
        {
            if (!ccNotesContract.IsNullOrEmpty())
            {

                ClientContactNote clientContactNotes = new ClientContactNote();
                if (!ccNotesContract.NoteId.IsNullOrEmpty() && ccNotesContract.NoteId > AppConsts.NONE)
                {
                    //Update Client Contact Notes
                    clientContactNotes = _dbContext.ClientContactNotes.Where(cond => !cond.CCN_IsDeleted && cond.CCN_ID == ccNotesContract.NoteId && cond.CCN_ClientContactID == ccNotesContract.ClientContactId).FirstOrDefault();
                    clientContactNotes.CCN_Notes = ccNotesContract.Notes;
                    clientContactNotes.CCN_ModifiedBy = currentLoggedInUserId;
                    clientContactNotes.CCN_ModifiedOn = DateTime.Now;
                }
                else
                {
                    //Save Client Contact Notes
                    clientContactNotes.CCN_Notes = ccNotesContract.Notes;
                    clientContactNotes.CCN_CreatedBy = currentLoggedInUserId;
                    clientContactNotes.CCN_CreatedOn = DateTime.Now;
                    clientContactNotes.CCN_ClientContactID = ccNotesContract.ClientContactId;
                    clientContactNotes.CCN_IsDeleted = false;

                    _dbContext.ClientContactNotes.AddObject(clientContactNotes);
                }
                if (_dbContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }

            return false;
        }
        Boolean IApplicantClinicalRotationRepository.DeleteClientContactNotes(Int32 clientCOntactNoteId, Int32 currentLoggedInUserId)
        {
            if (!clientCOntactNoteId.IsNullOrEmpty())
            {

                ClientContactNote clientContactNotes = new ClientContactNote();
              
                    //Delete Client Contact Notes
                    clientContactNotes = _dbContext.ClientContactNotes.Where(cond => !cond.CCN_IsDeleted && cond.CCN_ID == clientCOntactNoteId ).FirstOrDefault();
                    clientContactNotes.CCN_IsDeleted = true;
                    clientContactNotes.CCN_ModifiedBy = currentLoggedInUserId;
                clientContactNotes.CCN_ModifiedOn = DateTime.Now;
              
                if (_dbContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region UAT-4657

        void IApplicantClinicalRotationRepository.VersioningRequirementPackages(Int32 currentUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_RequirementPackageVersioning", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SystemUserID", currentUserId);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }

        void IApplicantClinicalRotationRepository.ProcessRequirementCategoryDisassociation(Int32 requirementCategoryDisassociationTenantMappingId, Int32 currentUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_RequirementCategoryDisassociation", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RCDTMId", requirementCategoryDisassociationTenantMappingId);
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentUserId);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }

        #endregion
    }
}
