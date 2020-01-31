using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class UploadedDocumentApplicantDataContract
    {
        public Int32 PackageSubscriptionID { get; set; }
        public Int32 ApplicantID { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public DateTime ApplicantDOB { get; set; }
        public String PackageName { get; set; }
        public String InstitutionHierarchy { get; set; }
        public Int32 OrderID { get; set; }
        public String OrderStatusName { get; set; }
        public Int32 TotalCount { get; set; }
        public String OrderNumber { get; set; }
    }

    [Serializable]
    public class UploadDocumentContract
    {
        public String ItemsID { get; set; }
        public String ItemsName { get; set; }
        public String CategoryId { get; set; }
        public String CategoryName { get; set; }
        public String CategoryItemsID { get; set; }
    }

}
