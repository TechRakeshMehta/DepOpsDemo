using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ProfileSharing
{

    public class ProfileSharingPackages
    {
        /// <summary>
        /// Id of the Package. Could be Compliance or Background
        /// </summary>
        public Int32 PackageId { get; set; }

        /// <summary>
        /// Name of the Package. Could be Compliance or Background
        /// </summary>
        public String PackageName { get; set; }

        /// <summary>
        /// SubscriptionId of the Package. Will be 0  if 'IsCompliancePkg' is True 
        /// </summary>
        public Int32 PackageSubscriptionId { get; set; }

        /// <summary>
        /// PK of the ams.bkgorderpackage table.
        /// </summary>
        public Int32 BkgOrderPkgId { get; set; }

        /// <summary>
        /// Represents whether the current is Compliance 
        /// </summary>
        public Boolean IsCompliancePkg { get; set; }

        /// <summary>
        /// Represents the Categories for the Compliance Package. Will be NULL or EMPTY if 'IsCompliancePkg' is False
        /// </summary>
        public List<ComplianceCategory> CompliancePkgCategories { get; set; }

        /// <summary>
        /// Represents the Service Groups for the Background Package. Will be NULL or EMPTY if 'IsCompliancePkg' is True
        /// </summary>
        public List<BkgSvcGroup> BkgSvcGroups { get; set; }

        /// <summary>
        /// Represnts Applicant Org UserID
        /// </summary>
        public Int32 ApplicantID { get; set; }

        /// <summary>
        /// Will be only for Service Groups having status as Compelted.
        /// </summary>
        public DateTime? SvcGrpCompletionDate { get; set; }

        /// <summary>
        /// Compliance status code of package.
        /// UAT-2232
        /// </summary>
        public String PkgComplianceStatus { get; set; }
    }

    /// <summary>
    /// Represents the Requirement Packages that can be shared in Profile Sharing. 
    /// Currently, can be 'Rotation' type.
    /// </summary>
    public class ProfileSharingRequirementPackage
    {
        /// <summary>
        /// SubscriptionId of the Requirement  Package. 
        /// </summary>
        public Int32 PackageSubscriptionId { get; set; }

        /// <summary>
        /// Id of the Requirement Package. 
        /// </summary>
        public Int32 RequirementPackageId { get; set; }

        /// <summary>
        /// Name of the Requirement Package. 
        /// </summary>
        public String RequirementPackageName { get; set; }

        /// <summary>
        /// Type of the Requirement Package. 
        /// </summary>
        public String PackageTypeCode { get; set; }

        /// <summary>
        /// Represents the Categories for the Requirement Package.  
        /// </summary>
        public List<RequirementCategory> RequirementPkgCategories { get; set; }

        /// <summary>
        /// Represnts Applicant Org UserID
        /// </summary>
        public Int32 ApplicantID { get; set; }
    }
}
