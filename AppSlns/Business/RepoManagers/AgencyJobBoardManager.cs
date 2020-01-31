using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.RepoManagers
{
    public static class AgencyJobBoardManager
    {

        public static List<AgencyJobContract> GetAgencyJobTemplate(Int32 organizationUserID)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).GetAgencyJobTemplate(organizationUserID);
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

        public static List<DefinedRequirementContract> GetJobFieldType()
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).GetJobFieldType();
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

        public static Boolean SaveAgencyJobTemplate(AgencyJobContract agencyJob, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).SaveAgencyJobTemplate(agencyJob, currentLoggedInUserId);
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

        public static Boolean DeleteAgencyJobTemplate(AgencyJobContract agencyJobTemplate, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).DeleteAgencyJobTemplate(agencyJobTemplate, currentLoggedInUserId);
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

        public static Int32 GetAgencyHierarchyId(Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).GetAgencyHierarchyId(organizationUserId);
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

        public static List<AgencyJobContract> GetAgencyJobPosting(Int32 organizationUserID)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).GetAgencyJobPosting(organizationUserID);
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

        public static Boolean SaveAgencyJobPosting(AgencyJobContract agencyJob, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).SaveAgencyJobPosting(agencyJob, currentLoggedInUserId);
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

        public static Boolean DeleteAgencyJobPosting(AgencyJobContract agencyJobPosting, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).DeleteAgencyJobPosting(agencyJobPosting, currentLoggedInUserId);
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

        public static AgencyJobContract GetTemplateDetailsByID(Int32 selectedTemplateId)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).GetTemplateDetailsByID(selectedTemplateId);
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

        public static AgencyLogoContract GetAgencyLogo(Int32 agencyHierarchyID)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).GetAgencyLogo(agencyHierarchyID);
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

        public static Boolean SaveUpdateAgencyLogo(AgencyLogoContract agencyLogoContract, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).SaveUpdateAgencyLogo(agencyLogoContract, currentLoggedInUserID);
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

        public static Boolean ArchiveJobPosts(List<Int32> agencyJobIds, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).ArchiveJobPosts(agencyJobIds, currentLoggedInUserID);
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

        public static Boolean ClearLogo(Int32 agencyHierarchyID, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).ClearLogo(agencyHierarchyID, currentLoggedInUserID);
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

        public static AgencyJobContract GetSelectedJobPostDetails(Int32 currentAgencyJobID)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).GetSelectedJobPostDetails(currentAgencyJobID);
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

        public static List<AgencyJobContract> GetViewAgencyJobPosting(JobSearchContract jobSearchContract, CustomPagingArgsContract grdCustomContract)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).GetViewAgencyJobPosting(jobSearchContract, grdCustomContract);
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


        public static Boolean SaveClientSystemDocument(Int32 agencyHierarchyID, List<RequirementApprovalNotificationDocumentContract> lstRequirementApprovalNotificationDocumentContract)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).SaveClientSystemDocument(agencyHierarchyID, lstRequirementApprovalNotificationDocumentContract);
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

        public static RequirementApprovalNotificationDocumentContract GetClientSystemDocumentBasedOnDocumentType(Int32 agencyHierarchyID, String requirementApprovalNotificationDocumentTypeCode)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).GetClientSystemDocumentBasedOnDocumentType(agencyHierarchyID, requirementApprovalNotificationDocumentTypeCode);
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

        public static Boolean DeleteClientSystemDocumentBasedOnDocType(Int32 agencyHierarchyID, Int32? agencyID, Int32 currentLoggedInUserID, String requirementApprovalNotificationDocumentTypeCode)
        {
            try
            {
                return BALUtils.GetAgencyJobBoardRepoInstance(AppConsts.NONE).DeleteClientSystemDocumentBasedOnDocType(agencyHierarchyID, agencyID, currentLoggedInUserID, requirementApprovalNotificationDocumentTypeCode);
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
    }
}
