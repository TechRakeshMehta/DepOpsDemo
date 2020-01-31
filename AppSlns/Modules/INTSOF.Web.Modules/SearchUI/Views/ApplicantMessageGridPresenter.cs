using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.Search.Views
{
    public class ApplicantMessageGridPresenter : Presenter<IApplicantMessageGridView>
    {
        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public ComplianceInstitutionHierarchyPresenter([CreateNew] IComplianceOperationsController controller)
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

        public void GetQueue()//String queueName, Int32 folderId, String folderCode, Int32 queueType, Int32 queueOwnerId, Int32 userGroup)
        {
            List<ApplicantMessageDetails> a = new List<ApplicantMessageDetails>();
            QueueManager queueManager = new QueueManager();
            IEnumerable<EntityObject> messages = queueManager.GetQueue(QueueConstants.MESSAGEQUEUE, View.OrganizationUserId);
            //test=test.Skip(10).Take(5).ToList();
            View.MessageDetailData = GetADBMessageDetails(messages, "MSGINB");
        }

        private List<ApplicantMessageDetails> GetADBMessageDetails(IEnumerable<EntityObject> messages, String folderCode)
        {
            List<ApplicantMessageDetails> messageDetails = new List<ApplicantMessageDetails>();
            String messageSizeNode = String.Empty;
            View.GridTotalCount = messages.Count();
            Int32 recordToSkip = (View.CurrentPageIndex - 1) * View.PageSize;
            messages = messages.Skip(recordToSkip).Take(View.PageSize);
            foreach (EntityObject msg in messages)
            {
                ADBMessageToList item = (ADBMessageToList)msg;
                if (!messageDetails.Exists(m => m.MessageId.Equals(item.ADBMessageID)))
                {

                    ApplicantMessageDetails message = new ApplicantMessageDetails();
                    message.MessageId = Convert.ToString(item.ADBMessage.ADBMessageID);
                    message.Subject = item.ADBMessage.Subject;
                    message.ReceivedDate = Convert.ToDateTime(item.ADBMessage.ReceiveDate);
                    message.CommunicationType = item.ADBMessage.lkpCommunicationType == null ? string.Empty : item.ADBMessage.lkpCommunicationType.Name;

                    message.CommunicationTypeCode = item.ADBMessage.lkpCommunicationType == null ? string.Empty : item.ADBMessage.lkpCommunicationType.Code;
                    messageSizeNode = item.ADBMessage.Message.GetNodeContent("MessageSize");
                    if (!item.ADBMessage.From.Equals(DBNull.Value))
                    {
                        String eMail = item.ADBMessage.Message.GetNodeContent("From");
                        message.FromMessage = eMail.IsNullOrEmpty() ? String.Empty : eMail;
                    }
                    messageDetails.Add(message);
                }
            }

            return messageDetails;
        }
    }
}
