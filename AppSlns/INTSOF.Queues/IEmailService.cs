using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace INTSOF.Queues
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmailService" in both code and config file together.
    [ServiceContract]
    public interface IEmailService
    {
        [OperationContract]
        bool SendMail(EMailMessage MailMessage);

        [OperationContract]
        bool SendSystemMail(EMailMessage MailMessage);

    }
   
    /// <summary>
    /// Data Contract - Email Addresses,Subject, Type (Module type),Contents and Priority
    /// </summary>
    [DataContract]
    public class EMailMessage
    {
        #region Private Variables

        private string _ToAddresses = String.Empty;
        private string _Subject = String.Empty;
        private string _EmailPriority = "Normal";
        private Dictionary<string, string> _Content;
        private string _EmailType = String.Empty;
        private string _EmailBody = String.Empty;
        private string _CCAddresses = String.Empty;
        private string _BCCAddresses = String.Empty;
        #endregion

        #region Data Members

        [DataMember]
        public string ToAddresses
        {
            get { return _ToAddresses; }
            set { _ToAddresses = value; }
        }

        [DataMember]
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        [DataMember]
        public string EmailPriority
        {
            get { return _EmailPriority; }
            set { _EmailPriority = value; }
        }

        [DataMember]
        public Dictionary<string, string> Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        [DataMember]
        public string EmailType
        {
            get { return _EmailType; }
            set { _EmailType = value; }
        }

        [DataMember]
        public string CCAddresses
        {
            get { return _CCAddresses; }
            set { _CCAddresses = value; }
        }

        [DataMember]
        public string EmailBody
        {
            get { return _EmailBody; }
            set { _EmailBody = value; }
        }

        [DataMember]
        public string BCCAddresses
        {
            get { return _BCCAddresses; }
            set { _BCCAddresses = value; }
        }

        
        #endregion
    }
}
