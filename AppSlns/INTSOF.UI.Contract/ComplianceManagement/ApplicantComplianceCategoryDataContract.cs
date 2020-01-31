using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ApplicantComplianceCategoryDataContract
    {
        public String Notes { get; set; }
        public Int32 ReviewStatusTypeId { get; set; }
        public Int32 ComplianceCategoryId { get; set; }
        public Int32 PackageSubscriptionId { get; set; }
        public String ReviewStatusTypeCode { get; set; }
        public Int32 ApplicantComplianceCategoryId { get; set; }
    }
}
