using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementItemDataLoader
    {
        /// <summary>
        /// Represents the Category Level Data i.e. All items
        /// </summary>
        List<RequirementVerificationDetailContract> lstCategoryData
        {
            get;
            set;
        }

        /// <summary>
        /// Reporesents the TenantID of the Applicant
        /// </summary>
        Int32 TenantId { get; set; }

        /// <summary>
        /// Represents the CategoryId
        /// </summary>
        Int32 CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// prefix for the Item Control
        /// </summary>
        String ItemControlIdPrefix
        {
            get;
        }

        /// <summary>
        /// List for 'lkpRequirementItemStatus' entity
        /// </summary>
        List<RequirementItemStatusContract> lstReqItemStatusTypes
        {
            get;
            set;
        }

        /// <summary>
        /// ApplicantRequirementCategoryDataID
        /// </summary>
        Int32 ApplReqCatDataId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the screen from which the screen was opened
        /// </summary>
        String ControlUseType
        {
            get;
            set;
        }

        //UAT-2224: Admin access to upload/associate documents on rotation package items.
        Int32 SelectedApplicantId_Global { get; set; }
        Int32 CurrentRequirementPackageSubscriptionID_Global { get; set; }
        List<ApplicantDocumentContract> lstApplicantDocument { get; set; }
        ApplicantRequirementItemDataContract RequirementItemData { get; set; }

        //UAT 2371
        String EntityPermissionName
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn { get; set; }
        Boolean IsClientAdminLoggedIn { get; set; }
        Int32 CurrentTenantId_Global { get; set; }

    }
}
