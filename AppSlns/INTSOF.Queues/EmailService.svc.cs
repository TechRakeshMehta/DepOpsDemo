using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Xsl;
using INTSOF.Logger;
using INTSOF.Logger.factory;

namespace INTSOF.Queues
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmailService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmailService.svc or EmailService.svc.cs at the Solution Explorer and start debugging.
    public class EmailService : IEmailService
    {
        private ILogger _logger;

        public EmailService()
        {
            _logger = SysXLoggerFactory.GetInstance().GetLogger();
            _logger.Info("EmailService constructor called.");
        }

        #region Public Methods
        /// <summary>
        /// Send SMTP Mails
        /// </summary>
        /// <param name="EMailMessage"> Email Message Object </param>
        /// <returns></returns>
        public bool SendMail(EMailMessage EMailMessage)
        {
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
                message.To.Add(EMailMessage.ToAddresses);
                message.Subject = EMailMessage.Subject;
                message.IsBodyHtml = true;
                message.Priority = GetEmailPriority(EMailMessage.EmailPriority);
                message.Body = PrepareEmailContent(EMailMessage.Content, EMailMessage.EmailType);
                _logger.Info("Sending Email to Address - "+EMailMessage.ToAddresses);

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

        /// <summary>
        /// Send SMTP Mails
        /// </summary>
        /// <param name="EMailMessage"> Email Message Object </param>
        /// <returns></returns>
        public bool SendSystemMail(EMailMessage EMailMessage)
        {
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
                message.To.Add(EMailMessage.ToAddresses);
                message.Subject = EMailMessage.Subject;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.Normal;
                message.Body = EMailMessage.EmailBody;
                _logger.Info("Sending Email to Address - " + EMailMessage.ToAddresses);
                if (!String.IsNullOrEmpty(EMailMessage.CCAddresses))
                {
                    String[] ccUsers = EMailMessage.CCAddresses.Split(';').Distinct().ToArray();
                    foreach (var ccUser in ccUsers)
                    {
                        message.CC.Add(new MailAddress(ccUser));
                    }
                }
                if (!String.IsNullOrEmpty(EMailMessage.BCCAddresses))
                {
                    String[] bccUsers = EMailMessage.BCCAddresses.Split(';').Distinct().ToArray();

                    foreach (var bccUser in bccUsers)
                    {
                        message.Bcc.Add(new MailAddress(bccUser));
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
        #endregion

        #region Private Methods

        /// <summary>
        /// Set EMail Priority 
        /// </summary>
        /// <param name="emailPriority"></param>
        /// <returns>MailPriority</returns>
        private MailPriority GetEmailPriority(string emailPriority)
        {
            if (emailPriority.Equals("Normal"))
                return MailPriority.Normal;
            if (emailPriority.Equals("High"))
                return MailPriority.High;
            if (emailPriority.Equals("Low"))
                return MailPriority.Low;
            return MailPriority.Normal;
        }

        /// <summary>
        /// Create HTML Message content
        /// </summary>
        /// <param name="Content">Contents to be placed in XML</param>
        /// <param name="emailType">Name of Module Calling Service,Should be same as template files names</param>
        /// <returns>HTML Message Contents</returns>
        private string PrepareEmailContent(Dictionary<string, string> Content, string emailType)
        {

            try
            {
                String emailTemplatePath = HostingEnvironment.ApplicationPhysicalPath + "EmailTemplates\\" + emailType.ToString() + ".xml";
                String transformFilePath = HostingEnvironment.ApplicationPhysicalPath + "EmailTemplates\\" + emailType.ToString() + ".xslt";

                return ExecuteEmailTransform(emailTemplatePath, transformFilePath, Content);

            }
            catch (XsltException xsltEx)
            {

                return xsltEx.Message + "--" + xsltEx.InnerException.Message; //Call Ezxception module <TBD>
            }
        }

        /// <summary>
        /// XSLT transformations
        /// </summary>
        /// <param name="emailTemplatePath">Path of xml file</param>
        /// <param name="transformFilePath">Path of xslt file</param>
        /// <param name="emailContent">Contents /param>
        /// <returns>Transformed Message </returns>
        private string ExecuteEmailTransform(string emailTemplatePath, string transformFilePath, Dictionary<string, string> emailContent)
        {
            MemoryStream mTransformResults = new MemoryStream();

            try
            {
                XmlDocument emailDoc = new XmlDocument();
                emailDoc.Load(emailTemplatePath);

                XmlNamespaceManager emailDocNamespaces = new XmlNamespaceManager(emailDoc.NameTable);
                emailDocNamespaces.AddNamespace("mstns", "http://tempuri.org/ADBEmails.xsd");


                foreach (var item in emailContent)
                {
                    XmlNode node = emailDoc.SelectSingleNode("//mstns:" + item.Key.ToString(), emailDocNamespaces);
                    node.InnerText = item.Value;

                }

                // Prepare the XSLT argument list
                XsltArgumentList emailTransformArgList = new XsltArgumentList();
                foreach (var item in emailContent)
                {
                    emailTransformArgList.AddParam(item.Key, String.Empty, item.Value);

                }

                // Apply the Xml transform and get the email body
                XslCompiledTransform emailTransform = new XslCompiledTransform();
                emailTransform.Load(transformFilePath);

                emailTransform.Transform(emailDoc, emailTransformArgList, mTransformResults);

                //Convert the transform results to a string
                string results = ReadTransformResults(mTransformResults);
                mTransformResults.Close();
                return results;
            }
            catch (Exception transformEx)
            {
                mTransformResults.Close();
                String errorMessage = transformEx.Message;
                if (transformEx.InnerException != null)
                {
                    errorMessage += transformEx.InnerException.Message;
                }
                _logger.Error("Exception Is:", transformEx);
                _logger.Error("Error is:" + errorMessage);
                return transformEx.Message;
            }
        }
        /// <summary>
        /// Convert XSLT Transformed contents from Memory Stream to string Value
        /// </summary>
        /// <param name="resultsStream">Memory stream</param>
        /// <returns>string data</returns>
        private string ReadTransformResults(MemoryStream resultsStream)
        {

            if (resultsStream == null || resultsStream.Length == 0)
                return String.Empty;

            resultsStream.Flush();
            resultsStream.Position = 0;
            StreamReader sr = new StreamReader(resultsStream);
            string results = sr.ReadToEnd();
            return results;

        }

        #endregion
    }
}
