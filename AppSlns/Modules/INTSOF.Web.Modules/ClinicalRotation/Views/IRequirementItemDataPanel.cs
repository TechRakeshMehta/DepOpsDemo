using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementItemDataPanel
    {
        /// <summary>
        /// Represents the Category Level Data
        /// </summary>
        List<RequirementVerificationDetailContract> lstCategoryData
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the CategoryId
        /// </summary>
        Int32 CurrentReqCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the Requirement Package SubscriptionId
        /// </summary>
        Int32 CurrentReqSubsciptionId
        {
            get;
            set;
        }
        List<RequirementVerificationDetailContract> lstReqPkgSubData { get; set; } //UAT-4461
        /// <summary>
        /// Represents the CategoryId
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        String ControlUseType
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the complete data of the verification details, which needs to be saved/updated
        /// </summary>
        RequirementVerificationData DataToSave
        {
            get;
            set;
        }

        /// <summary>
        /// ControlID of ItemDataLoader
        /// </summary>
        String ReqItemDataLoaderControlId
        {
            get;
        }

        //UAT 2371
        String EntityPermissionName { get; set; }

        /// <summary>
        /// Represents the ID of the currently logged in user.
        /// </summary>
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// Represents the ID of the Applicant
        /// </summary>
        Int32 ApplicantId
        {
            get;
            set;
        }

        /// <summary>
        /// Current Rotation ID
        /// </summary>
        Int32 CurrentRotationId
        {
            get;
            set;
        }

        RequirementPackageSubscriptionContract RotationSubscriptionDetail
        {
            get;
            set;
        }

        //UAT-3049:-
        // Agency Id

        Int32 AgencyId
        {
            get;
            set;
        }
        List<ReqPkgSubscriptionIDList> lstApplicantDataForNavigation { get; set; } //UAT-4461
        Int32 CurrentTenantId_Global
        { get; set; }
        //UAT-4461
        ManageReqPkgSubscriptionContract NextPrevNavigationData
        {
            get;
            set;
        }
        Boolean IsCurrentARIDRecordAlreadySaved
        {
            get;
            set;
        }
        Int32 ApplicantRequirementItemDataId
        {
            get;
            set;
        }
    }
}
