using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceCategoryContract
    {
        public Int32 ComplianceCategoryId { get; set; }
        public String CategoryName { get; set; }
        public String CategoryLabel { get; set; }
        public String ScreenLabel { get; set; }
        public String ExplanatoryNotes { get; set; }
        public String Description { get; set; }
        public Boolean Active { get; set; }
        public Int32 TenantID { get; set; }

        /// <summary>
        /// Datetime for the start of the compliance category in client screen.
        /// </summary>
        public DateTime? EffectiveFrom { get; set; }

        /// <summary>
        /// Package Id for the Master compliance categories
        /// </summary>
        public Int32 AssignToPackageId { get; set; }

        /// <summary>
        /// Package id for the Client compliance categories
        /// </summary>
        public Int32 PackageId { get; set; }
        /// <summary>
        /// Get and set Display Order
        /// </summary>
        public Int32 DisplayOrder { get; set; }

        public Boolean ComplianceRequired { get; set; }

        /// <summary>
        /// Property to store the SampleDocFormURL
        /// </summary>
        public String SampleDocFormURL { get; set; }

        /// <summary>
        /// Datetime for the start of the compliance category in client screen.
        /// </summary>
        public DateTime? CmplncRqdStartDate { get; set; }

        /// <summary>
        /// Datetime for the start of the compliance category in client screen.
        /// </summary>
        public DateTime? CmplncRqdEndDate { get; set; }

        public Int32 CPC_ID { get; set; }

        //UAT-2725
        public Boolean TriggerOtherCategoryRules { get; set; }

        //UAT-3161
        public String MoreInfoText { get; set; }

        public List<DocumentUrlContract> DocumentUrls {get; set;}


        public Boolean SendItemDoconApproval { get; set; } //UAT-3805

    }
}
