using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ApplicantCommunicationGridControlPresenter : Presenter<IApplicantCommunicationGridControlView>
    {

        public void GetUserMessages()
        {
            View.lstUserMessageDetailList = CommunicationManager.GetUserMessages(View.UserId, View.PageSize, View.CurrentPageIndex, View.DefaultSortExpression, View.IsSortDirectionDescending, View.ApplicantDashboard);
            if (View.lstUserMessageDetailList != null && View.lstUserMessageDetailList.Count > 0)
                View.VirtualPageCount = View.lstUserMessageDetailList.FirstOrDefault().TotalRecords;
        }
        #region UAT-3261: Badge Form Enhancements
        public void GetUserEmails()
        {
            View.lstUserEmailDetailList = CommunicationManager.GetUserEmails(View.UserId, View.PageSize, View.CurrentPageIndex, View.DefaultEmailSortExpression, View.IsSortDirectionDescending, View.ApplicantDashboard);
            if (View.lstUserEmailDetailList != null && View.lstUserEmailDetailList.Count > 0)
                View.VirtualPageCount = View.lstUserEmailDetailList.FirstOrDefault().TotalRecords;
        }

        public Boolean IsFileMissing()
        {
            Boolean result = false;
            View.SystemCommunicationDeliveryIds = CommunicationManager.GetSysCommDeliveryIds(View.SystemCommunicationId).Select(x => x.SystemCommunicationDeliveryID).ToList();
            List<SystemCommunicationAttachment> attachmentDocument = MessageManager.GetSystemCommunicationAttachment(View.SystemCommunicationId);
            if (!attachmentDocument.IsNullOrEmpty())
            {
                attachmentDocument.ForEach(x =>
                {
                    String docPath = x.SCA_DocumentPath;
                    if (!CommonFileManager.DoesDocExist((docPath)))
                    {
                        result = true;
                    }
                });
            }
            return result;
        }

        public void QueueReSendingEmails()
        {
            CommunicationManager.QueueReSendingEmails(View.SystemCommunicationDeliveryIds, View.UserId, true);
        }
        #endregion
    }
}
