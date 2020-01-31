using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [DataContract]
    [Serializable]
    public class ItemDataSearchContract
    {
        [DataMember]
        public Int32 CategoryID { get; set; }
        [DataMember]
        public Int32 PackageSubscriptionID { get; set; }
        [DataMember]
        public String ApplicantName { get; set; }
        [DataMember]
        public String FirstName { get; set; } //UAT-3796
        [DataMember]
        public String LastName { get; set; } //UAT-3796
        [DataMember]
        public String ItemName { get; set; }
        [DataMember]
        public String CategoryName { get; set; }
        [DataMember]
        public String PackageName { get; set; }
        [DataMember]
        public DateTime? SubmissionDate { get; set; }
        [DataMember]
        public String VerificationStatus { get; set; }
        [DataMember]
        public String SystemStatus { get; set; }
        [DataMember]
        public String AssignedUserName { get; set; }
        [DataMember]
        public String CustomAttributes { get; set; }
        [DataMember]
        public Int32 ApplicantComplianceItemID { get; set; }
        [DataMember]
        public Int32 ApplicantId { get; set; }
        [DataMember]
        public String UserGroups { get; set; }
        [DataMember]
        public Int32 PackageID { get; set; } //UAT-4136
    }
}
