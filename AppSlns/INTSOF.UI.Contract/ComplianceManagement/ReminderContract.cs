using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ReminderContract
    {
        public Int32 PackageSubscriptionID { get; set; }
        public DateTime ExpiryDate { get; set; }
        //public Int32 CompliancePackageID { get; set; }
        public String PackageName { get; set; }
        public Int32 OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        //public Guid UserID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PrimaryEmailAddress { get; set; }
        //public String ProgramName { get; set; }
        //public String DepartmentName { get; set; }
        //public String InstituteName { get; set; }
        public String NodeHierarchy { get; set; }
        public Int32 HierarchyNodeID { get; set; }
        public String OrderNumber { get; set; }
        public DateTime? OrgUserActiveDate { get; set; }//UAT-3223
    }
}
