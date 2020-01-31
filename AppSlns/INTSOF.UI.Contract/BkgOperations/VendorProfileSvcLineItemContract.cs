using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    [DataContract]
    public class VendorProfileSvcLineItemContract
    {
        [DataMember]
        public Int32 OrderID { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public Int32 BkgOrderID { get; set; }
        [DataMember]
        public Int32 BkgOrderPackageID { get; set; }
        [DataMember]
        public Int32 BackgroundPackageID { get; set; }
        [DataMember]
        public String BackgroundPackageName { get; set; }
        [DataMember]
        public Int32 ServiceGroupID { get; set; }
        [DataMember]
        public String ServiceGroupName { get; set; }
        [DataMember]
        public Int32 ServiceID { get; set; }
        [DataMember]
        public String ServiceName { get; set; }
        [DataMember]
        public Int32 OrderPkgSvcGroupID { get; set; }
        [DataMember]
        public Int32 SvcLineItemStatusID { get; set; }
        [DataMember]
        public String SvcLineItemStatus { get; set; }
        [DataMember]
        public Int32 OrderPkgSvcID { get; set; }
        [DataMember]
        public Int32 PackageSvcLineItemID { get; set; }
        [DataMember]
        public Int32 ExtVendorBkgOrderDetailID { get; set; }
        [DataMember]
        public Int32 ExtVendorBkgOrderLineItemDetailID { get; set; }
        [DataMember]
        public Int32 ExtVendorID { get; set; }
        [DataMember]
        public String VendorProfileID { get; set; }
        [DataMember]
        public String VendorLineItemOrderID { get; set; }
        [DataMember]
        public Boolean IsLinkProfile { get; set; }
        [DataMember]
        public Int32 BkgHierarchyMappingID { get; set; }
        [DataMember]
        public Int32 OrganizationUserID { get; set; }
        [DataMember]
        public Int32 OrganizationUserProfileID { get; set; }
        [DataMember]
        public String ApplicantName { get; set; }
        [DataMember]
        public String PrimaryEmailAddress { get; set; }
        [DataMember]
        public Int32 SelectedNodeID { get; set; }
    }
}
