using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using System.Data;

namespace Business.RepoManagers
{
    public static class ApplicantClinicalRotationManager
    {
        #region Variables

        #region public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static ApplicantClinicalRotationManager()
        {
            BALUtils.ClassModule = "ApplicantClinicalRotationManager";
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
        public static Boolean IsApplicantClinicalRotationMember(Int32 tenantID, Int32 applicantOrgUserID)
        {
            try
            {
                return BALUtils.GetApplicantClinicalRotationRepoInstance(tenantID).IsApplicantClinicalRotationMember(applicantOrgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ClinicalRotationDetailContract> GetApplicantClinicalRotations(Int32 applicantOrgUserId, ClinicalRotationDetailContract searchDataContract, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetApplicantClinicalRotationRepoInstance(tenantID).GetApplicantClinicalRotations(applicantOrgUserId, searchDataContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// Get Applicant Clinical Rotation Details
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="applicantOrgUserId"></param>
        /// <param name="loggedInOrgUserID"></param>
        /// <returns></returns>
        public static List<ClinicalRotationDetailContract> GetApplicantClinicalRotationDetails(Int32 tenantID, Int32 applicantOrgUserId, Int32? loggedInOrgUserID)
        {
            try
            {
                return BALUtils.GetApplicantClinicalRotationRepoInstance(tenantID).GetApplicantClinicalRotationDetails(applicantOrgUserId, loggedInOrgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// IsApplicantDropped
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="applicantOrgUserId"></param>
        /// <param name="clinicalRotationID"></param>
        /// <returns></returns>
        public static Boolean IsApplicantDropped(Int32 tenantID, Int32 applicantOrgUserId, Int32 clinicalRotationID)
        {
            try
            {
                return BALUtils.GetApplicantClinicalRotationRepoInstance(tenantID).IsApplicantDropped(applicantOrgUserId, clinicalRotationID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
        #region UAT-4248
        public static List<ClinicalRotationDetailContract> GetInstructorClinicalRotationDetails(Int32 tenantID, Int32 instructorOrgUserId, Int32? loggedInOrgUserID)
        {
            try
            {
                return BALUtils.GetApplicantClinicalRotationRepoInstance(tenantID).GetInstructorClinicalRotationDetails(instructorOrgUserId, loggedInOrgUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        #endregion
        #region UAT-4313
        public static List<ClientContactNotesContract> GetClientContactNotes(Int32 tenantID, Int32 clientContactId)
        {
            try
            {
                return BALUtils.GetApplicantClinicalRotationRepoInstance(tenantID).GetClientContactNotes(clientContactId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        
     public static Boolean SaveClientContactNotes(ClientContactNotesContract ccNotesContract, Int32 currentLoggedInUserId, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetApplicantClinicalRotationRepoInstance(tenantID).SaveClientContactNotes(ccNotesContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }


             public static Boolean DeleteClientContactNotes(Int32 clientContactNoteId, Int32 currentLoggedInUserId, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetApplicantClinicalRotationRepoInstance(tenantID).DeleteClientContactNotes(clientContactNoteId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        #endregion
    }
    }
