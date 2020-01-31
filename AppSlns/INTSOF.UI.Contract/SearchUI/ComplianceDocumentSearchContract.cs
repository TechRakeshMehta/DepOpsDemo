using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SearchUI
{
    [Serializable]
    public class ComplianceDocumentSearchContract
    {
        public String ApplicantName { get; set; }
        public String FileName { get; set; }
        public String ItemName { get; set; }
        public Int32 ApplicantDocumentID { get; set; }
        public Int32 ApplicantID { get; set; }
        public Int32 TotalCount { get; set; }
        public String DocumentPath { get; set; }
        public Int32 ID { get; set; }

        //UAT-1560 : We should be able to add documents that need to be signed to the order process
        //Using for additional document search screen
        public String FirstName { get; set; }
        public String LastName { get; set; }

        //UAT-3725
        public DateTime? SubmissionDate { get; set; }

    }
}
