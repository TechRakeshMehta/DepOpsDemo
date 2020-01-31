using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.DashBoard.Messaging.Views
{
    public class EmailViewPresenter : Presenter<IEmailViewerView>
    {
        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMessagingController _controller;
        // public MessageViewerPresenter([CreateNew] IMessagingController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        // TODO: Handle other view events and set state in the view
        public void GetSystemNotificationDetails()
        {
            SystemCommunication systemCommunication = CommunicationManager.GetSystemNotificationDetails(View.SystemCommunicationId);
            StringBuilder strContent = new StringBuilder();              
            GetSysCommDeliveryDetails();
            String toUsers = String.Empty;
            String ccUsers = String.Empty;
            String dispatchedDate = String.Empty;
            if (View.lstCommunicationDelivery != null)
            {
                toUsers = string.Join(",", View.lstCommunicationDelivery.Where(x => x.IsCC == false || x.IsCC.IsNullOrEmpty() && (x.IsBCC.IsNullOrEmpty() || x.IsBCC == false)).Select(x => x.RecieverEmailID).ToList());
                ccUsers = string.Join(",", View.lstCommunicationDelivery.Where(x => x.IsCC == true && x.IsBCC == false && x.IsBCC.IsNullOrEmpty()).Select(x => x.RecieverEmailID).ToList());

                strContent.Append("<div class='header'><div class='subject'>" + SysXUtils.GetXmlDecodedString(systemCommunication.Subject) + "</div><div class='senders'>" + systemCommunication.SenderEmailID + "</div>");
                strContent.Append("<div class='date'><span>Sent:&nbsp;</span>" + View.lstCommunicationDelivery.Where(x=>x.IsDispatched).Select(x=>x.DispatchedDate).FirstOrDefault() + "</div>");
                strContent.Append("<div class='receivers'><span>To:&nbsp;</span>" + toUsers + "</div>");
                strContent.Append("<div class='copies'><span>CC:&nbsp;</span>" + ccUsers + "</div></div>");
            }
            strContent.Append(systemCommunication.Content);
            View.DetailedContent = strContent.ToString();            
        }
        public void QueueReSendingEmails()
        {
            CommunicationManager.QueueReSendingEmails(View.SystemCommunicationDeliveryIds, View.CurrentUserId,true);
        }

        public void GetSysCommDeliveryDetails()
        {
          View.lstCommunicationDelivery = CommunicationManager.GetSysCommDeliveryIds(View.SystemCommunicationId).ToList();          
          View.SystemCommunicationDeliveryIds = View.lstCommunicationDelivery.Select(x => x.SystemCommunicationDeliveryID).ToList();
        }

        public String GetFilePath()
        {
            List<SystemCommunicationAttachment> attachmentDocument = MessageManager.GetSystemCommunicationAttachment(View.SystemCommunicationId);
            if (!attachmentDocument.IsNullOrEmpty())
            {
                return attachmentDocument.Select(x => x.SCA_DocumentPath).FirstOrDefault();                
            }
            return String.Empty;
        }      
    }
}
