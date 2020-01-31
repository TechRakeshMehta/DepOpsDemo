using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementVerificationDetail
    {
        /// <summary>
        /// Selected TenantId
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Selected ApplicantId
        /// </summary>
        Int32 SelectedApplicantId
        {
            get;
            set;
        }

        /// <summary>
        /// Selected RotationId
        /// </summary>
        Int32 ClinicalRotationId
        {
            get;
            set;
        }

        /// <summary>
        /// RequiremnetPackageSubscriptionID i.e. RPS_ID
        /// </summary>
        Int32 RPSId
        {
            get;
            set;
        }

        IRequirementVerificationDetail CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Data of the Applicant.
        /// </summary>
        OrganizationUserContract ApplicantData
        {
            get;
            set;
        }

        /// <summary>
        /// Complete data of the Verification details screen, including the data entered by applicant
        /// </summary>
        List<RequirementVerificationDetailContract> lstVerificationDetailData
        {
            get;
            set;
        }


        /// <summary>
        /// List for 'lkpRequirementItemStatus' entity
        /// </summary>
        List<RequirementItemStatusContract> lstReqItemStatusTypes
        {
            get;
            set;
        }

        String CategoryControlIdPrefix
        {
            get;
        }

        /// <summary>
        /// Contains the complete data of the verification details, which needs to be saved/updated
        /// </summary>
        RequirementVerificationData DataToSave
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId { get; }
    }
}
