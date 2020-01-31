using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace EmailDispatcherService
{
    public class Email
    {
        public MailAddress From { get; set; }
        public List<MailAddress> ToAddresses { get; set; }        
        public String Subject { get; set; }
        public String Body { get; set; }
        public MailPriority Priority { get; set; }
        public List<MailAddress> CcAddresses { get; set; }
        public List<MailAddress> BccAddresses { get; set; }
        public Dictionary<Int32, DocumentInfo> MailAttachments { get; set; }
    }

    public class DocumentInfo
    {
        public String DocumentPath { get; set; }
        public String DocumentName { get; set; }
        public String AttachmentTypeCode { get; set; }
    }
}
