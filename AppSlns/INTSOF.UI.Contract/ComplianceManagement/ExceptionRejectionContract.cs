using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ExceptionRejectionContract
    {
        public String UserFullName { get; set; }
        public String ComplianceItemName { get; set; }
        public String PackageName { get; set; }
        public String InstituteName { get; set; }
        public String ApplicationUrl { get; set; }
        public String NodeHierarchy { get; set; }
        public String CategoryName { get; set; }
        public Int32 ApplicantID { get; set; }
        public String RejectionReason { get; set; }
        public String ApplicantName { get; set; }
        public Int32 HierarchyNodeID { get; set; }
        public Int32? ItemID { get; set; }
        public String CategoryExplanatoryNotes { get; set; }
        public String CategoryMoreInfoURL { get; set; }
    }
}
