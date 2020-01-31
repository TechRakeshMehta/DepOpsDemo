using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class BkgOrderSearchQueueContract
    {
        //UAT-1795 : Add D&A download button on Background Order Queue search. 
        public String FileName { get; set; }
        public Int32 ApplicantDocumentID { get; set; }
        public String DocumentPath { get; set; }
        public Int32 ApplicantID { get; set; }
        public Int32 OrderID { get; set; }
        public String OrderNumber { get; set; }
    }

    [Serializable]
    public class GranularPermission
    {
        //UAT-4522
        public String EntityCode {get;set;}
        public String PermissionCode {get;set;}
        public Int32 HierarchyID { get; set; }
        public Int32 MasterDpmId { get; set; }

    }
}
