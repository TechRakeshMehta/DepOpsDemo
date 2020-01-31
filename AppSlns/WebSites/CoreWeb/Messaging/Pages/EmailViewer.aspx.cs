using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using Business.RepoManagers;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Text;
using System.Collections.Generic;
using Entity;
using Telerik.Web.UI;
using CoreWeb.Shell;
using CoreWeb.DashBoard.Messaging.Views;

namespace CoreWeb.Messaging.Views
{
    public partial class EmailViewer : System.Web.UI.Page, IEmailViewerView
    {
        private EmailViewPresenter _presenter = new EmailViewPresenter();
        public IEmailViewerView CurrentViewContext
        {
            get { return this; }
        }
        #region Property
        public Int32 SystemCommunicationId
        {
            get;
            set;
        }

        public List<Int32> SystemCommunicationDeliveryIds
        {
            get;
            set;
        }

        public List<SystemCommunicationDelivery> lstCommunicationDelivery
        {
            get;
            set;
        }

        public String DetailedContent
        {
            set
            {
                litMessageContent.Text = value;
            }
        }
        public int CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        #endregion


        #region PageEvents
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString[AppConsts.SYSTEM_COMMUNICATION_ID_QUERY_STRING]))
            {
                CurrentViewContext.SystemCommunicationId = Convert.ToInt32(Request.QueryString[AppConsts.SYSTEM_COMMUNICATION_ID_QUERY_STRING]);
            }
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                this.Title = String.Format("Dispatched on : {0}", Request[AppConsts.DATE_FORMAt_QUERY_STRING]);
                Presenter.OnViewLoaded();
                GetAttachments();
                Presenter.GetSystemNotificationDetails();
            }
        }
        #endregion

        public void GetAttachments()
        {
            RadToolBarSplitButton rsbtnAttachments = WclToolBar1.Items.FindItemByValue("Attachment") as RadToolBarSplitButton;
            List<SystemCommunicationAttachment> attachmentDocument = MessageManager.GetSystemCommunicationAttachment(CurrentViewContext.SystemCommunicationId);

            if (!attachmentDocument.IsNullOrEmpty())
            {
                foreach (var item in attachmentDocument)
                {
                    String iconURL = "~/App_Themes/Default/images/icons/filetypes/" + MessageManager.GetAttachmentIcon(item.SCA_OriginalDocumentName.Substring(item.SCA_OriginalDocumentName.LastIndexOf(".")));
                    Boolean FileExists = CommonFileManager.DoesDocExist((item.SCA_DocumentPath));
                    rsbtnAttachments.Buttons.Add(new RadToolBarButton { Text = item.SCA_OriginalDocumentName, ImageUrl = iconURL, ImagePosition = ToolBarImagePosition.Left, Enabled = FileExists, NavigateUrl = "AttachmentDownload.aspx?" + AppConsts.FILE_NAME_QUERY_STRING + "=" + item.SCA_DocumentPath + "&" + AppConsts.ORIGINAL_FILE_NAME_QUERY_STRING + "=" + item.SCA_OriginalDocumentName });
                }
            }
            else
                rsbtnAttachments.Visible = false;
        }

        public EmailViewPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        protected void WclToolBar1_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            if (!(e.Item as RadToolBarButton).IsNullOrEmpty())
            {
                String commandName = (e.Item as RadToolBarButton).CommandName;
                if (commandName.ToLower() == "resend")
                {
                    List<SystemCommunicationAttachment> attachmentDocument = MessageManager.GetSystemCommunicationAttachment(CurrentViewContext.SystemCommunicationId);
                    if (!attachmentDocument.IsNullOrEmpty() && !CommonFileManager.DoesDocExist((Presenter.GetFilePath())))
                    {
                        lblSuccess.ShowMessage("This email cannot be resent as attachment is no longer available.", MessageType.Information);
                        return;
                    }

                    CurrentViewContext.SystemCommunicationDeliveryIds = new List<int>();
                    Presenter.GetSysCommDeliveryDetails();
                    Presenter.QueueReSendingEmails();
                    lblSuccess.ShowMessage("Email has been resent successfully.", MessageType.SuccessMessage);

                }
            }
        }
    }
}