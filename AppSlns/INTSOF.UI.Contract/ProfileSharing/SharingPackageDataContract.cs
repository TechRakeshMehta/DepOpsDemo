using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    /// <summary>
    /// Represents the Type of packages & their categories/service groups selected by the Admin for Profile sharing 
    /// </summary>
    [Serializable]
    public class SharingPackageDataContract
    {
        /// <summary>
        /// Represents the PackageId
        /// </summary>
        public Int32 PackageId { get; set; }

        /// <summary>
        /// Represents the CatgegoryIds/Service GroupIds SELECTED for packages
        /// </summary>
        public List<Int32> lstSelectedCategoryGrpIds { get; set; }

        /// <summary>
        /// Represents the CatgegoryIds/Service GroupIds NOT SELECTED for packages
        /// </summary>
        public List<Int32> lstExcludedCategoryGrpIds { get; set; }

        /// <summary>
        /// Represents the Type of Package i.e. Compliance, Background or Requirement. 
        /// Uses the 'SystemPackageTypes' Enum Codes
        /// </summary>
        public String PackageType { get; set; }

        /// <summary>
        /// Will be TRUE if, NO category or service group was selected 
        /// </summary>
        public Boolean IsCompletelyExcluded { get; set; }

        /// <summary>
        /// Will be TRUE if even a single category or service group was selected 
        /// </summary>
        public Boolean IsPartiallyExcluded { get; set; }

        /// <summary>
        /// Represents the ProfileSharingInvitation
        /// </summary>
        public Int32 PSIGroupId { get; set; }
    }

    /// <summary>
    /// Represents the Data based on which the packages are to be selected.
    /// </summary>
    [Serializable]
    public class SharingPackageSelectedDataContract
    {
        /// <summary>
        /// Represents the TenantId
        /// </summary>
        public Int32 TenantId { get; set; }

        /// <summary>
        /// Represents the RotationId
        /// </summary>
        public Int32 RotationId { get; set; }

        /// <summary>
        /// Represents the CSV of the selected applicants
        /// </summary>
        public String lstSelectedApplicants { get; set; }
    }
}
