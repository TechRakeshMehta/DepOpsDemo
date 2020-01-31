using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class ApplicantDocumentContract
    {
        public Int32 ApplicantDocumentID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String FileName { get; set; }
        public String Description { get; set; }
        public String MD5Hash { get; set; }
        public String DocumentTypeName { get; set; }
    }
}
