using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class BkgServiceGroupMailContract
    {
        public Int32 PackageServiceGroupID { get; set; }
        public Int32 BkgOrderID { get; set; }
        public Int32 BkgSVCGrpID { get; set; }
        public Boolean IsCompleted { get; set; }
        public String ServiceGroupName { get; set; }
        public String PrimaryEmailaddress { get; set; }
        public String OrderNumber { get; set; }
        public String ApplicantName { get; set; }
        public Int32 OrganizationUserId  { get; set; }
        public Int32 SelectedNodeID { get; set; }
    }
}
