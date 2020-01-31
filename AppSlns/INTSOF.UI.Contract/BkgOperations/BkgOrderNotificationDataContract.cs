using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class BkgOrderNotificationDataContract
    {
        public Int32 OrderID { get; set; }
        public Int32 HierarchyNodeID { get; set; }
        public Int32 BkgOrderID { get; set; }
        public Int32 ServiceID { get; set; }
        public Int32 ServiceAtachedFormID { get; set; }
        public String ServiceAtachedFormName { get; set; }
        public Boolean SendAutomatically { get; set; }
        public Int32 BkgOrderPackageSvcID { get; set; }
        public Int32 ServiceFormMappingID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PrimaryEmailAddress { get; set; }
        public Int32? SystemDocumentID { get; set; }
        public String DocumentName { get; set; }
        public String DocumentPath { get; set; }
        public Guid? RefrenceID { get; set; }
        public Int32? DocumentSize { get; set; }
        public String ServiceName { get; set; }
        public Int32 ServiceGroupID { get; set; }
        public Int32 OrderPackageServiceGroupID { get; set; }
        public String ServiceGroupName { get; set; }
        public Int32 PackageServiceGroupID { get; set; }
        public String PackageName { get; set; }
        public String OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public String NodeHierarchy { get; set; }
        public String ApplicantProfileAddress { get; set; }
        public Boolean IsEmployment { get; set; }
        public String OrderNumber { get; set; }
        public Boolean IsAdminOrder { get; set; }
        public Boolean IsOrderFlag { get; set; }//UAT-3453
    }
}
