using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceAttributeDocMappings
    {
        public Int32 PackageID { get; set; }
        public Int32 CategoryID { get; set; }
        public Int32 ComplianceCategoryStatusId { get; set; }
        public Int32 ItemID { get; set; }
        public Int32 ComplianceItemStatusId { get; set; }

        public Int32 AttributeID { get; set; }

        public List<Int32> lstComplianceAttrSysDocID { get; set; }

        public List<Int32> lstApplicantDocID { get; set; }
    }
}
