#region Copyright
// **************************************************************************************************
// Copyright 2011 Intersoft Data Labs.
// 
//      All rights are reserved.  Reproduction or transmission in whole or in part, in any form or
//    by any means, electronic, mechanical or otherwise, is prohibited without the prior written
//    consent of the copyright owner.
// *************************************************************************************************
#endregion

#region Using Namespace



#endregion

namespace INTSOF.Utils.UI
{
   using System;

   using System.Configuration;

   using System.Net.Mail;
   using System.Web.Configuration;
   using System.Web.Hosting;
   using System.Net.Configuration;

   public static class SysXEmailService
    {
        /// <summary>
        /// Use to send a Email
        /// </summary>
        /// <param name="bodyText">Content of the email</param>
        /// <param name="subject">Subject of mail</param>
        /// <param name="toAddress">Email address of receiver</param>

        public static bool SendMail(string bodyText, string subject, string toAddress)
        {
            //SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
            
           //MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            try
            {
                //MailAddress fromAddress = new MailAddress(Convert.ToString(mailSettings.Smtp.From), Convert.ToString(ConfigurationManager.AppSettings["mailSenderName"]));
                //smtpClient.Host = Convert.ToString(mailSettings.Smtp.Network.Host);
                //smtpClient.Port = Convert.ToInt32(mailSettings.Smtp.Network.Port);
                //message.From = fromAddress;
                message.To.Add(toAddress);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = bodyText;
                Intsof.SMTPService.SMTPService smtpService = new Intsof.SMTPService.SMTPService();
                smtpService.SendMail(message);
                //smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
