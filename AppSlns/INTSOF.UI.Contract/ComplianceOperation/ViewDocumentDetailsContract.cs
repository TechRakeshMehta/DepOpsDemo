using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class ViewDocumentDetailsContract
    {
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
        public string FullName { get; set; }
        public bool IsSignatureRequired { get; set; }
        public bool IsApplicantDoc { get; set; }

        public Int32 AddedViewDocId { get; set; }
       
    }
}
