using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class VerificationQueueContract
    {
        public Int32? UserGroupId { get; set; }

        public Int32? PackageId { get; set; }
        public Int32? CategoryId { get; set; }
        public Int32? ItemId { get; set; }
        public String AssignToUserName { get; set; }

        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }

        public String statusCodesXML { get; set; }

        public String ReviewerType { get; set; }
        public Boolean? ShowIncompleteItems { get; set; }
        public Boolean? ShowRushOrders { get; set; }
        public Boolean IsSuperAdmin { get; set; }
        public Int32 ReviewerId { get; set; }

        public String OrderBy { get; set; }
        public Boolean? OrderDirection { get; set; }
        public Int32? PageSize { get; set; }
        public Int32? PageIndex { get; set; }
        public Int32? CurrentLoggedInUserId { get; set; }
    }

    [Serializable]
    public class PkgSubscriptionIDList
    {
        public Int32 PackageSubscriptionID { get; set; }
        public Int32 applicantcomplianceitemid { get; set; }
        public Int32 applicantcompliancecategoryid { get; set; }
        public Int32 complianceitemid { get; set; }
    }
}
