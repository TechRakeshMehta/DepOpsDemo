using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;

namespace CoreWeb.Messaging.Views
{
    public class CommunicationArchiveSummaryPresenter : Presenter<ICommunicationArchiveSummaryView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMessagingController _controller;
        // public CommunicationSummaryPresenter([CreateNew] IMessagingController controller)
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

        public void GetCommunicationSummaryArchive()
        {
            //Created By Pawan Kapoor
            //new code for UAT-333 
            if (View.SearchContract == null)
            {
                View.CommunicationSummaryList = new List<CommunicationTemplateContract>();
                View.VirtualRecordCount = AppConsts.NONE;
                View.CurrentPageIndex = 1;
            }
            else
            {
                SearchCommunicationTemplateContract searchItems = View.SearchContract;
                try
                {
                    View.GridCustomPaging.DefaultSortExpression = "SystemCommunicationDeliveryID";
                    View.GridCustomPaging.SecondarySortExpression = "SystemCommunicationID";
                    List<CommunicationTemplateContract> lstCommunicationTemplate = CommunicationManager.GetCommunicationSummarysearchArchive(searchItems, View.GridCustomPaging);
                    View.CommunicationSummaryList = lstCommunicationTemplate;
                    if (lstCommunicationTemplate.IsNullOrEmpty())
                    {
                        View.VirtualRecordCount = AppConsts.NONE;
                    }
                    else
                    {
                        // View.VirtualRecordCount = lstCommunicationTemplate[0].TotalRecordCount;
                        View.VirtualRecordCount = View.GridCustomPaging.VirtualPageCount;
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }

                }
                catch (Exception e)
                {
                    View.CommunicationSummaryList = new List<CommunicationTemplateContract>();
                    throw e;
                }

                //code before UAT-1427: WB: 

                //Entity.CustomPagingArgs customPagingArgs = new Entity.CustomPagingArgs();
                //View.GridCustomPaging.DefaultSortExpression = QueueConstants.COM_SUMMARY_DEFAULT_SORTING_FIELDS;
                //View.GridCustomPaging.SecondarySortExpression = QueueConstants.COM_SUMMARY_SECONDARY_SORTING_FIELDS;

                //IQueryable<CommunicationTemplateContract> communicationSummaryList = CommunicationManager.GetCommunicationSummary();
                //List<CommunicationTemplateContract> temp = communicationSummaryList.ToList();

                //temp.ForEach(cond =>
                //{
                //    cond.DispatchDate = Convert.ToDateTime(cond.DispatchDate).Date;

                //});

                //View.CommunicationSummaryList = customPagingArgs.ApplyFilterOrSort(temp.AsQueryable(), View.GridCustomPaging);
                //View.VirtualPageCount = View.GridCustomPaging.VirtualPageCount;
                //View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
        }

        public void QueueReSendingEmails()
        {
            CommunicationManager.QueueReSendingEmails(View.SystemCommunicationDeliveryIds, View.CurrentUserId);
        }
    }
}




