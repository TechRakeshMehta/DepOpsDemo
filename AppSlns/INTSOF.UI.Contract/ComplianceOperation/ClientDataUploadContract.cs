using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class ClientDataUploadContract
    {
        public Int32 ClientDataUploadID { get; set; }
        public String StoreProcedureName { get; set; }
        public String ClientRequestFormatCode { get; set; }
        public String ClientRequestFormatName { get; set; }
        public String WebServiceURL { get; set; }
        public String AssemblyLocation { get; set; }
        public String ClassFullName { get; set; }
        public Int32 Frequency { get; set; }
        public String AuthenticationRequestURL { get; set; }
        public Int32 HierarchyNodeID { get; set; }
        public String Code { get; set; }
        public int LoopCounter { get; set; }
        public Int32 TenantId { get; set; }
    }
}
