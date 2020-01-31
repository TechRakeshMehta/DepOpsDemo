using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class BadgeFormDocumentDataContract
    {
        public Int32 BFND_ID { get; set; }
        public String RotationName { get; set; }
        public DateTime? RotationStartDate { get; set; }
        public DateTime? RotationEndDate { get; set; }
        public String Location { get; set; }
        public String Program { get; set; }
        public String FullName { get; set; }
        public DateTime? DOB { get; set; }
        public String FullAddress { get; set; }
        public String PhoneNumber { get; set; }
        public DateTime? ItemApprovalDate { get; set; }
        public DateTime? ItemSubmisssionDate { get; set; }
        public byte[] Signature { get; set; }
        public DateTime? ShareApprovalDate { get; set; }
        public String AgencyUserApprovedShare { get; set; }
        public String UniversalFieldValue { get; set; }
        public Int32 UniversalFieldID { get; set; }
        
        //UAT-4104
        public DateTime? ItemExpirationDate { get; set; }
        public String AttributeValue { get; set; }
        public Int32 AttributeID { get; set; }
    }
}
