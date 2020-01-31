using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ApplicantComplianceAttributeDataContract
    {
        public Int32 ApplicantComplianceAttributeId { get; set; }

        public Int32 ApplicantComplianceItemId { get; set; }
        public Int32 ComplianceItemAttributeId { get; set; }
        public String AttributeValue { get; set; }
        public String AttributeTypeCode { get; set; }
        public Boolean IsFileUploadTypeAttribute { get; set; }
        public byte[] Signature { get; set; }
    }
}
