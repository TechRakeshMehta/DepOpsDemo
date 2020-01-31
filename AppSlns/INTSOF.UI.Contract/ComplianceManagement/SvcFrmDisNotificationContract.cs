using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class SvcFrmDisNotificationContract
    {
        public Int32 OrderID { get; set; }
        public Int32 HierarchyNodeID { get; set; }
        public Int32 BkgOrderID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PrimaryEmailAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public String BackgroundServiceName { get; set; }
        public String ServiceFormName { get; set; }
        public DateTime DispatchedDate { get; set; }
        public String PackageName { get; set; }
        public String NodeHierarchy { get; set; }
        public Int32 OrderServiceFormID { get; set; }
        public Int32 ServiceFormID { get; set; }
        public String ServiceGroupName { get; set; }
        public String OrderStatus { get; set; }
        public Boolean IsManual { get; set; }
        public String OrderNumber { get; set; }
        //UAT-2446:
        public Int32? SystemDocumentID { get; set; }
        public String DocumentName { get; set; }
        public String DocumentPath { get; set; }
        public Guid? RefrenceID { get; set; }
        public Int32? DocumentSize { get; set; }
    }
        
    public class ServiceFormContract
    {
        public String ServiceFormName { get; set; }
        public String ServiceName { get; set; }
        public String DocumentPath { get; set; }
        public Int32 SystemDocumentID { get; set; }
        public Int32 OrderID { get; set; }
    }
}
