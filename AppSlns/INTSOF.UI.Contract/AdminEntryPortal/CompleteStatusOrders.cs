using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.AdminEntryPortal
{
    public class CompleteStatusOrders
    {
        public int OrderID { get; set; }
        public int BkgOrderID { get; set; }
        //public int BkgOrderPackageID { get; set; }
        //public int BkgOrderPkgServGrpID { get; set; }
        public string BkgServGrpStatusType { get; set; }
        public string BkgServGrpStatusCode { get; set; }
        public int OrganizationUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string InstitutionHierarchyName { get; set; }

        public int? SelectedNodeId { get; set; }

        public bool IsAdminOrder { get; set; }

        public bool IsEmployment  { get; set; }

        public string OrderNumber { get; set; }

        public int? HierarchyNodeId { get; set; }

    }
}
