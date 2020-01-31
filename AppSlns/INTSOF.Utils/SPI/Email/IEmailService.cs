#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IEmailService.cs
// Purpose:   
//

#endregion
#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Activation;
using System.Collections;
using System.Configuration;

#endregion
#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils.SPI.Email
{
    [ServiceContract]

    public interface IEmailService
    {
        [OperationContract]
        bool SendMail(EMailMessage MailMessage);

        [OperationContract]
        bool SendSystemMail(EMailMessage EMailMessage);

        [OperationContract]
        [WebGet(UriTemplate = "/LatestAlert", ResponseFormat = WebMessageFormat.Json)]
        Alert GetLatestAlertData();

    }
    [DataContract]
    public class Alert
    {
        Int32 queueNo = 0;
        string description = "";

        [DataMember]
        public Int32 QueueNo
        {
            get { return queueNo; }
            set { queueNo = value; }
        }
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
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
