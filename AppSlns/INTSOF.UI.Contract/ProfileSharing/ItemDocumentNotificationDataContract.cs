using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class ItemDocumentNotificationDataContract
    {
        public String PackageName { get; set; }
        public String RotationName { get; set; }
        public DateTime? RotationStartDate { get; set; }
        public DateTime? RotationEndDate { get; set; }
        public Int32? RotationID { get; set; }
        public Int32 PackageSubscriptionID { get; set; }
        public String DocumentPath { get; set; }
        public String DocumentName { get; set; }
        public Int32 ApplicantDocumentID { get; set; }
        public Int32 CategoryDataID { get; set; }
        public String AgencyAdminName { get; set; }
        public String AgencyAdminEmail { get; set; }
        public Int32 AgencyOrgUserID { get; set; }
        public Int32 AgencyUserID { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public String ApplicantEmail { get; set; }
        public Int32 ApplicantOrgUserID { get; set; }
        public String HierarchyIDs { get; set; }
        public String RequestTypeCode { get; set; }
        public String CategoryName { get; set; }
        public Int32 DocumentSize { get; set; }
    }
}
