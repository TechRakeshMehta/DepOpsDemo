#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXEmailService.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using System.Text;
using System.Collections.Generic;
#endregion

#region Application Specific
using System.Linq;
using INTSOF.Logger.factory;
using INTSOF.Logger;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Web.Configuration;
using System.ServiceModel.Channels;
using System.IO;
using INTSOF.Utils.EmailService;

#endregion

#endregion

namespace INTSOF.Utils
{

    public static class SysXEmailService
    {



        private static ILogger _logger = _logger = SysXLoggerFactory.GetInstance().GetLogger();

        //public SysXEmailService()
        //{
        //    _logger = SysXLoggerFactory.GetInstance().GetLogger();
        //}

        #region Public Methods
        /// <summary>
        /// Use to send a Email
        /// </summary>
        /// <param name="bodyText">Content of the email</param>
        /// <param name="subject">Subject of mail</param>
        /// <param name="toAddress">Email address of receiver</param>
        //public static bool SendMail(String bodyText, String subject, String toAddress)
        //{
        //    SmtpClient smtpClient = new SmtpClient();
        //    MailMessage message = new MailMessage();
        //    Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        //    System.Net.Configuration.MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as System.Net.Configuration.MailSettingsSectionGroup;

        //    try
        //    {
        //        MailAddress fromAddress = new MailAddress(Convert.ToString(mailSettings.Smtp.From), Convert.ToString(ConfigurationManager.AppSettings["mailSenderName"]));
        //        smtpClient.Host = Convert.ToString(mailSettings.Smtp.Network.Host);
        //        smtpClient.Port = Convert.ToInt32(mailSettings.Smtp.Network.Port);
        //        message.From = fromAddress;
        //        message.To.Add(toAddress);
        //        message.Subject = subject;
        //        message.IsBodyHtml = true;
        //        message.Body = bodyText;   
        //        smtpClient.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        /// <summary>
        /// Send Mail Contents to Mail Service
        /// </summary>
        /// <param name="Content">Dictonary object for Contents [Key,Pair] as defined in templates</param>
        /// <param name="subject">subject of mail</param>
        /// <param name="toAddress">To Addresses</param>
        /// <param name="emailType">Type of Email Module same as Template name</param>
        /// <param name="emailPriority">Priority of Email</param>
        /// <returns></returns>
        public static bool SendMail(Dictionary<string, string> Content, String subject, String toAddress, String emailType, String emailPriority)
        {
            bool bMailStatus = false;

            try
            {
                //Converted EmailService Call to Simple WCF Service Call.
                //Send Mail
                //blnMailStatus = (Boolean)CallEmailService(subject, toAddress, emailPriority
                //    , Content, emailType, null, "IEmailService", "SendMail");
                EmailService.EmailServiceClient emailService = new EmailService.EmailServiceClient();
                EmailService.EMailMessage msg = new EmailService.EMailMessage();
                msg.Content = Content;
                msg.Subject = subject;
                msg.ToAddresses = toAddress;
                msg.EmailType = emailType;
                msg.EmailPriority = emailPriority;
                bMailStatus = emailService.SendMail(msg);


            }
            catch (Exception ex)
            {
                return false;
            }

            return bMailStatus;
        }

        /// <summary>
        /// Send Mail Contents to Mail Service
        /// </summary>
        /// <param name="Content">Dictonary object for Contents [Key,Pair] as defined in templates</param>
        /// <param name="subject">subject of mail</param>
        /// <param name="toAddress">To Addresses</param>
        /// <param name="emailType">Type of Email Module same as Template name</param>
        /// <param name="emailPriority">Priority of Email</param>
        /// <returns></returns>
        public static bool SendSystemMail(Dictionary<string, string> content, String subject, String toAddress)
        {
            bool bMailStatus = false;

            try
            {
                String _EmailBody = content.GetValue("EmailBody") as String;
                String _CCAddresses = content.GetValue("CCAddresses") as String;
                String _BCCAddresses = content.GetValue("BCCAddresses") as String;
                String fileName = content.ContainsKey("AttachmentFileName") ? content.GetValue("AttachmentFileName").ToString() : String.Empty;

                //Converted EmailService Call to Simple WCF Service Call.
                //Send Mail
                //bMailStatus = (Boolean)CallEmailService(subject, toAddress, String.Empty, content, String.Empty, null, "IEmailService", "SendSystemMail");
                EmailService.EmailServiceClient emailService = new EmailService.EmailServiceClient();
                EmailService.EMailMessage msg = new EmailService.EMailMessage();
                //msg.Content = content;

                msg.Subject = subject;
                msg.ToAddresses = toAddress;
                msg.EmailBody = _EmailBody;
                msg.CCAddresses = _CCAddresses;
                msg.BCCAddresses = _BCCAddresses;
                msg.EmailType = string.Empty;
                msg.EmailPriority = string.Empty;
                bMailStatus = emailService.SendSystemMail(msg);
            }
            catch (Exception ex)
            {
                return false;
            }

            return bMailStatus;
        }

        /// <summary>
        /// Send Mail Contents to Mail Service
        /// </summary>
        /// <param name="Content">Dictonary object for Contents [Key,Pair] as defined in templates</param>
        /// <param name="subject">subject of mail</param>
        /// <param name="toAddress">To Addresses</param>
        /// <param name="emailType">Type of Email Module same as Template name</param>
        /// <param name="emailPriority">Priority of Email</param>
        /// <returns></returns>
        public static bool SendSystemMailWithAttachment(Dictionary<string, string> content, String subject, String toAddress, Dictionary<String, Object> attachmentData)
        {
            try
            {
                String _EmailBody = content.GetValue("EmailBody") as String;
                String _CCAddresses = content.GetValue("CCAddresses") as String;
                String _BCCAddresses = content.GetValue("BCCAddresses") as String;
                String fileName = content.ContainsKey("AttachmentFileName") ? content.GetValue("AttachmentFileName").ToString() : String.Empty;

                //EmailService.EmailServiceClient emailService = new EmailService.EmailServiceClient();
                EmailService.EMailMessage msg = new EmailService.EMailMessage();
                msg.Subject = subject;
                msg.ToAddresses = toAddress;
                msg.EmailBody = _EmailBody;
                msg.CCAddresses = _CCAddresses;
                msg.BCCAddresses = _BCCAddresses;
                msg.EmailType = string.Empty;
                msg.EmailPriority = string.Empty;
                

                //SmtpClient smtpClient = new SmtpClient();
                _logger.Info("Inside the SendMail Method.");
                MailMessage message = new MailMessage();
                Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
                //System.Net.Configuration.MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as System.Net.Configuration.MailSettingsSectionGroup;

                try
                {
                    //MailAddress fromAddress = new MailAddress(Convert.ToString(mailSettings.Smtp.From), Convert.ToString(ConfigurationManager.AppSettings["mailSenderName"]));
                    //smtpClient.Host = Convert.ToString(mailSettings.Smtp.Network.Host);
                    //smtpClient.Port = Convert.ToInt32(mailSettings.Smtp.Network.Port);

                    //if (mailSettings.Smtp.Network.UserName != null && mailSettings.Smtp.Network.Password != null)
                    //    smtpClient.Credentials = new System.Net.NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password);


                    //message.From = fromAddress;
                    message.To.Add(msg.ToAddresses);
                    message.Subject = msg.Subject;
                    message.IsBodyHtml = true;
                    message.Priority = MailPriority.Normal;
                    message.Body = msg.EmailBody;
                    _logger.Info("Sending Email to Address - " + msg.ToAddresses);
                    if (!String.IsNullOrEmpty(msg.CCAddresses))
                    {
                        String[] ccUsers = msg.CCAddresses.Split(';').Distinct().ToArray();
                        foreach (var ccUser in ccUsers)
                        {
                            message.CC.Add(new MailAddress(ccUser));
                        }
                    }
                    if (!String.IsNullOrEmpty(msg.BCCAddresses))
                    {
                        String[] bccUsers = msg.BCCAddresses.Split(';').Distinct().ToArray();

                        foreach (var bccUser in bccUsers)
                        {
                            message.Bcc.Add(new MailAddress(bccUser));
                        }
                    }

                    if (attachmentData.IsNotNull())
                    {
                        if (!String.IsNullOrEmpty(fileName))
                        {
                            byte[] contentByte = attachmentData.GetValue("Attachment") as byte[];

                            MemoryStream stream = new MemoryStream(contentByte);
                            Attachment attachment = new Attachment(stream, fileName);
                            message.Attachments.Add(attachment);
                        }
                    }

                    Intsof.SMTPService.SMTPService smtpService = new Intsof.SMTPService.SMTPService();
                    smtpService.SendMail(message);
                    //smtpClient.Send(message);
                    _logger.Info("Email Sent.");

                }
                catch (Exception ex)
                {
                    _logger.Error("Unable to send email.", ex);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static byte[] GetPdfBytes(Dictionary<String, String> Content, String ContractName, String OperationName)
        {
            byte[] pdfBytes = null;

            try
            {
                //Send Mail
                pdfBytes = (byte[])CallEmailService(null, null, null
                    , null, null, Content, ContractName, OperationName);
            }
            catch (Exception ex)
            {
                return null;
            }

            return pdfBytes;
        }
        /// <summary>
        /// Generates the bytes of Specified url.
        /// </summary>
        /// <param name="urlToConvert"></param>
        /// <returns> byte[]</returns>
        public static byte[] GetPdfBytes(String urlToConvert)
        {
            //Converted PDFConversionService Call to Simple WCF Service Call.
            PDFConversion.PDFConversionClient pdfConversion = new PDFConversion.PDFConversionClient();
            return pdfConversion.GeneratePDF(urlToConvert);
        }

        public static bool SendMail(string content, string subject, string toAddresses, string ccAddrresses, bool isHighImportance, string documentName, string originalDocumentName)
        {
            //SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            //System.Net.Configuration.MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as System.Net.Configuration.MailSettingsSectionGroup;

            try
            {
                if (!String.IsNullOrEmpty(toAddresses))
                {
                    //MailAddress fromAddress = new MailAddress(Convert.ToString(mailSettings.Smtp.From), Convert.ToString(ConfigurationManager.AppSettings["mailSenderName"]));
                    //smtpClient.Host = Convert.ToString(mailSettings.Smtp.Network.Host);
                    //smtpClient.Port = Convert.ToInt32(mailSettings.Smtp.Network.Port);

                    //if (mailSettings.Smtp.Network.UserName != null && mailSettings.Smtp.Network.Password != null)
                    //    smtpClient.Credentials = new System.Net.NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password);


                    //message.From = fromAddress;
                    foreach (string address in toAddresses.Split(';'))
                        message.To.Add(address);


                    if (!string.IsNullOrEmpty(ccAddrresses))
                    {
                        foreach (string address in ccAddrresses.Split(';'))
                            message.CC.Add(address);
                    }

                    message.Subject = subject;
                    message.IsBodyHtml = true;
                    message.Priority = isHighImportance ? MailPriority.High : MailPriority.Normal;
                    message.Body = content;
                    if (!string.IsNullOrEmpty(documentName))
                    {
                        //string documentPath = HttpContext.Current.Server.MapPath("~/" + WebConfigurationManager.AppSettings[MessagingFolder.MESSAGE_FILE_LOCATION] + "/" + documentName);
                        //string documentPath = WebConfigurationManager.AppSettings[MessagingFolder.MESSAGE_FILE_LOCATION];
                        //if (!documentPath.EndsWith(@"\"))
                        //{
                        //    documentPath += @"\";
                        //}

                        //documentPath = documentPath + DateTime.Now.ToString("YYYYMM") + @"\";

                        //documentPath = documentPath + documentName;

                        Attachment attachment = new Attachment(documentName);
                        attachment.Name = originalDocumentName;
                        message.Attachments.Add(attachment);
                    }
                    Intsof.SMTPService.SMTPService smtpService = new Intsof.SMTPService.SMTPService();
                    smtpService.SendMail(message);
                    //smtpClient.Send(message);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to send email.", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Used to generate and send  mail for exceptions in HTML format
        /// </summary>
        /// <param name="ex"> Exception</param>
        /// <param name="subjectPrefix">Subject Prefix</param>
        /// <param name="toAddress">To Address</param>
        /// <returns></returns>
        public static bool SendExceptionMails(Exception ex)
        {
            Boolean blnMailStatus = false;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["ExceptionEmailNotification"]))
            {
                HttpContext appCtx = null;
                HttpRequest appRequest = null;
                string strSubject = string.Empty;
                string strHttpHost = string.Empty; ;
                StringBuilder stbBuilder = new StringBuilder();

                Dictionary<string, string> Content = new Dictionary<string, string>();

                appCtx = HttpContext.Current;
                appRequest = appCtx.Request;
                strHttpHost = appRequest.ServerVariables["HTTP_HOST"];
                strSubject = String.Format("{0} {1} ({2})", strHttpHost, ConfigurationManager.AppSettings["ExceptionEmail_SubPrefix"], appRequest.FilePath.ToString());

                #region Contents
                Content.Add("TimeStamp", DateTime.Now.ToString());
                Content.Add("QueryString", appRequest.QueryString.ToString());
                Content.Add("CurrentExecutionFilePath", appRequest.FilePath.ToString());
                Content.Add("FilePath", appRequest.CurrentExecutionFilePath);
                Content.Add("ApplicationPath", appRequest.ApplicationPath);
                Content.Add("Request.FilePath", appRequest.FilePath);
                Content.Add("Path", appRequest.Path);
                Content.Add("TYPE", ex.GetType().ToString());
                Content.Add("MESSAGE", ex.Message);
                Content.Add("SOURCE", ex.Source);
                Content.Add("STACKTRACE", ex.StackTrace.ToString());
                Content.Add("InnerException", ex.InnerException.IsNotNull() ? (ex.InnerException).Message : string.Empty);
                Content.Add("TARGETSITE", ex.TargetSite.ToString());

                //ex Data
                if (ex.Data.Count > 0)
                {
                    stbBuilder.Append("<table width='100%'  BorderStyle='Solid'  cellpadding='4' cellspacing='2' border='1'>");

                    foreach (string sKey in ex.Data.Keys)
                    {
                        stbBuilder.AppendFormat("<tr><td>{0}:</td><td>{1}</td></tr>", sKey, ex.Data[sKey].ToString());
                    }
                    stbBuilder.Append("</table>");

                }
                Content.Add("DATA", ex.Data.Count == 0 ? "None" : stbBuilder.ToString());

                //Context Items
                if (appCtx.Items.Count > 0)
                {
                    stbBuilder.Clear();
                    stbBuilder.Append("<table width='100%' BorderStyle='Solid' >");
                    foreach (var key in appCtx.Items.Keys)
                    {
                        if (!appCtx.Items[key].IsNull())
                        {
                            if (!appCtx.Items[key].ToString().Equals("System.Object"))

                                stbBuilder.AppendFormat("<tr><td>{0}:</td><td>{1}</td></tr>", key.ToString(), appCtx.Items[key].ToString());
                        }
                    }
                    stbBuilder.Append("</table>");

                }
                Content.Add("Context.Items", appCtx.Items.Count == 0 ? "None" : stbBuilder.ToString());

                //Form Variables
                if (appRequest.Form.Count > 0)
                {
                    stbBuilder.Clear();
                    stbBuilder.Append("<table width='100%' BorderStyle='Solid'>");
                    foreach (string s in appRequest.Form)
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            stbBuilder.AppendFormat("<tr><td>{0}:</td><td>{1}</td></tr>", s,
                             string.IsNullOrEmpty(appRequest.Form[s].ToString()) ? "None" :
                             (s.ToLower().Contains("password") ? "******" : appRequest.Form[s].ToString()));
                        }
                    }
                    stbBuilder.Append("</table>");



                }

                Content.Add("Request.Form", appRequest.Form.Count == 0 ? "None" : stbBuilder.ToString());

                //Server Variables

                if (appRequest.ServerVariables.Count > 0)
                {
                    stbBuilder.Clear();
                    stbBuilder.Append("<html><body><table width='100%' BorderStyle='Solid'>");
                    foreach (var s in appRequest.ServerVariables)
                    {

                        stbBuilder.AppendFormat("<tr><td>{0}:</td><td>{1}</td></tr>", (string)s, String.IsNullOrEmpty(appRequest.ServerVariables[(string)s].ToString()) ? "None" : appRequest.ServerVariables[(string)s].ToString());
                    }
                    stbBuilder.Append("</table></body></html>");
                }

                Content.Add("Request.ServerVariables", appRequest.ServerVariables.Count == 0 ? "None" : stbBuilder.ToString());
                Content.Add("Request.PathInfo", appRequest.PathInfo);
                Content.Add("Request.RawUrl", appRequest.PhysicalApplicationPath);
                Content.Add("Request.PhysicalPath", appRequest.PhysicalPath);
                Content.Add("Request.PhysicalApplicationPath", appRequest.RawUrl);

                #endregion

                //Initailizing Exception Message Values



                //Send Mail
                blnMailStatus = (Boolean)CallEmailService(strSubject, ConfigurationManager.AppSettings["ExceptionEmail_ToAddress"],
                    "Normal", Content, "Exception", null, "IEmailService", "SendMail").To<Boolean>();

            }
            return blnMailStatus;
        }


        #endregion

        #region Private Methods
        /// <summary>
        /// Use to send a Email
        /// </summary>
        /// <param name="eMailMessage">Content of the email</param>
        private static object CallEmailService(String Subject, String To, String EmailPriority
                    , Dictionary<String, String> Contents, String EmailType, Dictionary<String, String> Parameters = null, String ContractName = "IEmailService", String OperationName = "SendMail")
        {
            try
            {
                Uri mexAddress = null;
                if (ContractName == AppConsts.PDF_CONVERSION_SERVICE_CONTRACT_NAME && OperationName == AppConsts.PDF_CONVERSION_SERVICE_OPERATION_CONTRACT)
                {
                    mexAddress = new Uri(ConfigurationManager.AppSettings["PdfServiceUri"] + "?wsdl");
                }
                else
                {
                    mexAddress = new Uri(ConfigurationManager.AppSettings["EmailServiceUri"] + "?wsdl");
                }
                MetadataExchangeClientMode mexMode = MetadataExchangeClientMode.HttpGet;
                MetadataExchangeClient mexClient = new MetadataExchangeClient(mexAddress, mexMode);
                mexClient.ResolveMetadataReferences = true;
                MetadataSet metaSet = mexClient.GetMetadata();


                WsdlImporter importer = new WsdlImporter(metaSet);
                Collection<ContractDescription> contracts = importer.ImportAllContracts();
                ServiceEndpointCollection allEndpoints = importer.ImportAllEndpoints();


                ServiceContractGenerator generator = new ServiceContractGenerator();
                var endpointsForContracts = new Dictionary<string, IEnumerable<ServiceEndpoint>>();

                foreach (ContractDescription contract in contracts)
                {
                    generator.GenerateServiceContractType(contract);

                    endpointsForContracts[contract.Name] = allEndpoints.Where(
                        se => se.Contract.Name == contract.Name).ToList();
                }

                if (generator.Errors.Count != 0)
                    throw new Exception("There were errors during code compilation.");


                CodeGeneratorOptions options = new CodeGeneratorOptions();
                options.BracingStyle = "C";
                CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("C#");


                CompilerParameters compilerParameters = new CompilerParameters(
                    new string[] { 
                            "System.dll", "System.ServiceModel.dll", 
                            "System.Runtime.Serialization.dll"                            
                        });
                compilerParameters.GenerateInMemory = true;

                CompilerResults results = codeDomProvider.CompileAssemblyFromDom(compilerParameters, generator.TargetCompileUnit);

                if (results.Errors.Count > 0)
                {
                    throw new Exception("There were errors during  generated code compilation");
                }
                else
                {
                    Type clientProxyType = results.CompiledAssembly.GetTypes().First(
                        t => t.IsClass && t.GetInterface(ContractName) != null &&
                            t.GetInterface(typeof(ICommunicationObject).Name) != null);


                    ServiceEndpoint se = endpointsForContracts[ContractName].First();


                    //if (ContractName == AppConsts.PDF_CONVERSION_SERVICE_CONTRACT_NAME && OperationName == AppConsts.PDF_CONVERSION_SERVICE_OPERATION_CONTRACT)
                    //{
                    ((System.ServiceModel.BasicHttpBinding)(se.Binding)).MaxBufferSize = 2147483647;
                    ((System.ServiceModel.BasicHttpBinding)(se.Binding)).MaxReceivedMessageSize = 2147483647;
                    ((System.ServiceModel.BasicHttpBinding)(se.Binding)).ReaderQuotas.MaxArrayLength = 2147483647;
                    //}

                    object instance = results.CompiledAssembly.CreateInstance(
                        clientProxyType.Name,
                        false,
                        System.Reflection.BindingFlags.CreateInstance,
                        null,
                        new object[] { se.Binding, se.Address },
                        CultureInfo.CurrentCulture, null);

                    object[] operationParameters = null;
                    switch (ContractName)
                    {
                        case "IPDFConversion":
                            if (Parameters.IsNotNull())
                                operationParameters = new object[] { Parameters.GetValue("tempFileConvertorUrl") };
                            break;
                        case "IEmailService":
                            switch (OperationName)
                            {
                                case "SendSystemMail":
                                    operationParameters = GetOperationParametersForSystemEmail(Subject, To, Contents, results);
                                    break;
                                case "SendMail":
                                    operationParameters = GetOperationParametersForEmail(Subject, To, EmailPriority, Contents, EmailType, results);
                                    break;
                            }

                            break;
                        default:
                            break;
                    }

                    // Get the operation's method, invoke it, and get the return value
                    object retVal = instance.GetType().GetMethod(OperationName).
                        Invoke(instance, operationParameters);
                    return retVal;
                }
            }
            catch (Exception ex)
            {
                if (ContractName == AppConsts.PDF_CONVERSION_SERVICE_CONTRACT_NAME && OperationName == AppConsts.PDF_CONVERSION_SERVICE_OPERATION_CONTRACT)
                {
                    _logger.Error("Unable to generate pdf bytes.", ex);
                }
                else
                {
                    _logger.Error("Unable to send an email because an incorrect configuration parameter set was provided.", ex);
                }
                return false;
            }
        }

        private static object[] GetOperationParametersForEmail(String Subject, String To, String EmailPriority, Dictionary<String, String> Contents, String EmailType, CompilerResults results)
        {
            Type EMailMessageType = results.CompiledAssembly.GetTypes().First(condition => condition.Name == "EMailMessage");


            var Message = results.CompiledAssembly.CreateInstance(EMailMessageType.FullName);
            Message.GetType().GetProperty("Subject").SetValue(Message, Subject, null);
            Message.GetType().GetProperty("ToAddresses").SetValue(Message, To, null);
            Message.GetType().GetProperty("EmailPriority").SetValue(Message, EmailPriority, null);
            Message.GetType().GetProperty("Content").SetValue(Message, Contents, null);
            Message.GetType().GetProperty("EmailType").SetValue(Message, EmailType, null);

            object[] operationParameters = new object[] { Message };
            return operationParameters;
        }

        private static object[] GetOperationParametersForSystemEmail(String Subject, String To, Dictionary<String, String> Contents, CompilerResults results)
        {
            String _EmailBody = Contents.GetValue("EmailBody") as String;
            String _CCAddresses = Contents.GetValue("CCAddresses") as String;
            String _BCCAddresses = Contents.GetValue("BCCAddresses") as String;

            Type EMailMessageType = results.CompiledAssembly.GetTypes().First(condition => condition.Name == "EMailMessage");

            var Message = results.CompiledAssembly.CreateInstance(EMailMessageType.FullName);
            Message.GetType().GetProperty("Subject").SetValue(Message, Subject, null);
            Message.GetType().GetProperty("ToAddresses").SetValue(Message, To, null);
            Message.GetType().GetProperty("EmailBody").SetValue(Message, _EmailBody, null);
            Message.GetType().GetProperty("CCAddresses").SetValue(Message, _CCAddresses, null);
            Message.GetType().GetProperty("BCCAddresses").SetValue(Message, _BCCAddresses, null);

            object[] operationParameters = new object[] { Message };
            return operationParameters;
        }


        #endregion

    }
}
