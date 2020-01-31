using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class InvoiceGroupContract
    {
        public Int32 InvoiceGroupID { get; set; }
        public String InvoiceGroupName { get; set; }
        public String InvoiceGroupDescription { get; set; }
        public String InstitutionIDs { get; set; }
        public String InstitutionNames { get; set; }
        public String InstitutionHierarchyIDs { get; set; }
        public String InstitutionHierarchyLabels { get; set; }
        public String ReportColumnIDs { get; set; }
        public String ReportColumnNames { get; set; }
        public String InvoiceGroupReportColumnIDs { get; set; }

    }

    [Serializable]
    public class InvoiceGroupNodes
    {
        public Int32 TenantID { get; set; }
        public String NodeLabel { get; set; }
        public String NodeValue { get; set; }
        public Boolean Checked { get; set; }
        public String ParentNodeValue { get; set; }
    }

}
