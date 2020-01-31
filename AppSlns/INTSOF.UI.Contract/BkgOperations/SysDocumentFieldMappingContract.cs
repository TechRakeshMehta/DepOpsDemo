using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class SysDocumentFieldMappingContract
    {
        public Int32 ID { get; set; }
        public String FieldName { get; set; }
        public String FieldValue { get; set; }
        public String DocumentPath { get; set; }
        public Int32? SpecialFieldTypeID { get; set; }
        public String SpecialFieldTypeName { get; set; }
        public String SpecialFieldTypeCode { get; set; }
        public Int32? SystemDocOldID { get; set; }
        public Boolean? SendToStudent { get; set; }
        public SystemDocBkgSvcMapping SystemDocBkgSvcMapping { get; set; }
    }
}
