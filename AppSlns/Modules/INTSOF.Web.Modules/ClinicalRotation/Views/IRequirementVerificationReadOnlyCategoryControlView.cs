using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementVerificationReadOnlyCategoryControlView
    {
        /// <summary>
        /// CategoryID
        /// </summary>
        Int32 CategoryId { get; set; }

        /// <summary>
        /// ApplicantRequirementCategoryDataID
        /// </summary>
        Int32 ApplReqCatDataId { get; set; }

        /// <summary>
        /// Represents the category level data
        /// </summary>
        List<RequirementVerificationDetailContract> lstCategoryLevelData
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the current Context
        /// </summary>
        IRequirementVerificationReadOnlyCategoryControlView CurrentViewContext
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
        /// prefix for the Item Control
        /// </summary>
        String ItemControlIdPrefix
        {
            get;
        }

        /// <summary>
        /// SelectedTenantId
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }
        //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
         String RequirementDocumentLink
        {
            get;
            set;
        }
         String CategoryExplanatoryNotes
         {
             get;
             set;
         }

        //UAT-3161
         String RequirementDocumentLinkLabel
         {
             get;
             set;
         }
    }
}
