using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SystemSetUp
{
    public class InstitutionConfigurationDetailsContract
    {
        public String SplashScreenURL { get; set; }
        public List<InstitutionConfigurationPackageDetails> PackageDetailsList { get; set; }
        public List<InstitutionConfigurationAdministratorDetails> AdministratorsDetailsList { get; set; }
    }

    public class InstitutionConfigurationPackageDetails
    {
        public String PackageName { get; set; }
        public Decimal? Fee { get; set; }
        public String PaymentMethods { get; set; }
        public Int32 PackageID { get; set; }
        public String SubscriptionOption { get; set; }
        public Boolean IsCompliancePackage { get; set; }
        public String PackageType { get; set; }
        public Boolean IsParentPackage { get; set; }
        public Int32 PackageHierarchyID { get; set; }
    }

    public class InstitutionConfigurationAdministratorDetails
    {
        public Int32 OrganizationUserId { get; set; }
        public String UserFirstName { get; set; }
        public String UserLastName { get; set; }
        public String UserName { get; set; }
        public String ComliancePermissionName { get; set; }
        public String ProfilePermissionName { get; set; }
        public String VerificationPermissionName { get; set; }
        public String OrderQueuePermissionName { get; set; }
        public String BkgPermissionName { get; set; }
        public String PackagePermissionName { get; set; } //UAT-3369
        public String EmailAddress { get; set; }
        public Boolean IsActive { get; set; }
    }

}
