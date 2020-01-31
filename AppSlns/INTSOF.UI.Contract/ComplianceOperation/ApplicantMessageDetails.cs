using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class ApplicantMessageDetails
    {
        public String MessageId { get; set; }
        public String Subject { get; set; }
        public String FromMessage { get; set; }
        public DateTime ReceivedDate { get; set; }
        public String CommunicationType { get; set; }
        public String CommunicationTypeCode { get; set; }
        public Int32 FolderId { get; set; }
    }
}
