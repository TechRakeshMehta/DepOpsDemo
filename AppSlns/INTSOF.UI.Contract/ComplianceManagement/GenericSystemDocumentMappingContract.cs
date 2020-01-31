using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class GenericSystemDocumentMappingContract
    {
        public Int32 SystemDocMappingID { get; set; }

        public Int32 RecordID { get; set; }

        public String RecordTypeName { get; set; }

        public Int32 SystemDocID { get; set; }

        public String DocumentFileName { get; set; }

        public Boolean? IsOperational { get; set; }

        public Boolean? SendToStudent { get; set; }
    }
}
