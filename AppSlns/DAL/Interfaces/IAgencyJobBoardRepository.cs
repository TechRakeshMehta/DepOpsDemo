using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAgencyJobBoardRepository
    {
        List<AgencyJobContract> GetAgencyJobTemplate(Int32 OrganizationUserID);

        List<DefinedRequirementContract> GetJobFieldType(); //UAT-3071

        Boolean SaveAgencyJobTemplate(AgencyJobContract AgencyJob, Int32 CurrentLoggedInUserID);

        Boolean DeleteAgencyJobTemplate(AgencyJobContract AgencyJob, Int32 CurrentLoggedInUserID);

        Int32 GetAgencyHierarchyId(Int32 OrganizationUserID);

        List<AgencyJobContract> GetAgencyJobPosting(Int32 OrganizationUserID);

        Boolean SaveAgencyJobPosting(AgencyJobContract AgencyJob, Int32 CurrentLoggedInUserID);

        Boolean DeleteAgencyJobPosting(AgencyJobContract AgencyJobPosting, Int32 CurrentLoggedInUserId);

        AgencyJobContract GetTemplateDetailsByID(Int32 SelectedTemplateID);

        AgencyLogoContract GetAgencyLogo(Int32 agencyHierarchyID);

        Boolean SaveUpdateAgencyLogo(AgencyLogoContract agencyLogoContract, Int32 currentLoggedInUserID);

        Boolean ArchiveJobPosts(List<Int32> AgencyJobIds, Int32 CurrentLoggedInUserID);

        AgencyJobContract GetSelectedJobPostDetails(Int32 currentAgencyJobID);

        Boolean ClearLogo(Int32 AgencyHierarchyID, Int32 CurrentLoggedInUserID);

        #region Job Board
        List<AgencyJobContract> GetViewAgencyJobPosting(JobSearchContract jobSearchContract, CustomPagingArgsContract grdCustomContract);
        #endregion

        Boolean SaveClientSystemDocument(Int32 agencyHierarchyID, List<RequirementApprovalNotificationDocumentContract> lstRequirementApprovalNotificationDocumentContract);

        RequirementApprovalNotificationDocumentContract GetClientSystemDocumentBasedOnDocumentType(Int32 agencyHierarchyID, String requirementApprovalNotificationDocumentTypeCode);

        Boolean DeleteClientSystemDocumentBasedOnDocType(Int32 agencyHierarchyID, Int32? agencyID, Int32 currentLoggedInUserID, String requirementApprovalNotificationDocumentTypeCode);
    }
}
