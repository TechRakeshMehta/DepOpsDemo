using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class ApplicantInstitutionHierarchyMapping
    {
        public Int32 DPM_ID { get; set; }
        public Int32 InstitutionNode_ID { get; set; }
        public String InstitutionHierarchyLabel { get; set; }
        public Int32? RecordID { get; set; }
        public Int32 OrganisationUserId { get; set; }
    }
}
