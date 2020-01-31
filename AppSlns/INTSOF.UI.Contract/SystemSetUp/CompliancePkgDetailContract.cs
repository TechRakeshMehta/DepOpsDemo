using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SystemSetUp
{
    [Serializable]
    public class CompliancePkgDetailContract
    {
        public String NodeLabel { get; set; }
        public String PackageName { get; set; }
        public List<ReviewerTypeDetail> ReviewerTypeDetails { get; set; }
        public List<SubscriptionOptionDetail> SubscriptionOptionDetails { get; set; }
    }

    [Serializable]
    public class ReviewerTypeDetail
    {
        public Int32 AssignmentHierarchyID { get; set; }
        public Int32? ParentAssignmentHierarchyId { get; set; }
        public String ObjectName { get; set; }
        public String ReviewerType { get; set; }
        public String ObjectTypeCode { get; set; }
    }

    [Serializable]
    public class SubscriptionOptionDetail
    {
        public String SubscriptionOptionLabel { get; set; }
        public String Price { get; set; }
    }

    [Serializable]
    public class InstituteHierarchyTreeDataContract
    {      
        public Int32 TreeNodeTypeID { get; set; }
        public String NodeID { get; set; }
        public String ParentNodeID { get; set; }
        public Int32 Level { get; set; }
        public Int32 DataID { get; set; }
        public String Value { get; set; }
        public Int32? ParentDataID { get; set; }
        public String UICode { get; set; }
        public Boolean IsLabel { get; set; }
        public String NodeCode { get; set; }
        public String ParentNodeCode { get; set; }
        public Boolean Associated { get; set; }
        public Int32 MappingID { get; set; }
        public Int32 EntityID { get; set; }
        public String PermissionCode { get; set; }
        //public Boolean Used { get; set; }
        public String PermissionName { get; set; }
        public String ProfilePermissionCode { get; set; }
        public String ProfilePermissionName { get; set; }
        public String VerificationPermissionCode { get; set; }
        public String VerificationPermissionName { get; set; }
        public String OrderPermissionCode { get; set; }
        public String OrderPermissionName { get; set; }
        public Int32? DPM_DisplayOrder { get; set; }
        public Boolean? IsPackageAvailableForOrder { get; set; }
        public Boolean? IsPackageBundleAvailableForOrder { get; set; }
        
    }

    #region UAT-3873

    [Serializable]
    public class NodePackagesDetails
    {
        public Int32 PackageId { get; set; }
        public String PackageName { get; set; }
        public String PackageType { get; set; }
        public String IsBundlePackage { get; set; }
    }

    #endregion
}
