using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public interface ISharingPackageView
    {
        /// <summary>
        /// List of ApplicantId's selected for sharing
        /// </summary>
        String lstApplicantIds
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the Current View Context
        /// </summary>
        ISharingPackageView CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Represents the Current Rotation Id  
        /// </summary>
        Int32 RotationId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the Current TenantId
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// List of Compliance Packages
        /// </summary>
        List<ProfileSharingPackages> lstCompliancePackages
        {
            get;
            set;
        }

        /// <summary>
        /// List of Compliance Packages
        /// </summary>
        List<ProfileSharingPackages> lstBkgPackages
        {
            get;
            set;
        }

        /// <summary>
        /// List of Requirement Packages
        /// </summary>
        List<ProfileSharingRequirementPackage> lstRequirementPackages
        {
            get;
            set;
        }
    }
}
