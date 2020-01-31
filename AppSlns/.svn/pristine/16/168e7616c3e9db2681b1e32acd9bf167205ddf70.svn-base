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


namespace CoreWeb.Messaging.Views
{
    public partial class MessageViewer : System.Web.UI.Page, IMessageViewerView
    {
        private MessageViewerPresenter _presenter = new MessageViewerPresenter();
        Boolean _isDashboardMessage;

        public Int32 SytemCommunicationUserId
        {
            get
            {
                AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.SYSTEM_COMMUNICATION_USER_ID);
                Int32 systemCommunicationUserId = AppConsts.SYSTEM_COMMUNICATION_USER_VALUE;
                if (appConfiguration.IsNotNull())
                {
                    systemCommunicationUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }
                return systemCommunicationUserId;
            }
        }

        public Int32 BackgroundProcessUserId
        {
            get
            {
                AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = AppConsts.SYSTEM_COMMUNICATION_USER_VALUE;
                if (appConfiguration.IsNotNull())
                {
                    backgroundProcessUserId = Convert.ToInt32(appConfiguration.AC_Value);
                }
                return backgroundProcessUserId;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            this.Title = String.Format("From: {0}, Sent On:{1}", Request[AppConsts.FROM_ID_QUERY_STRING], Request[AppConsts.DATE_FORMAt_QUERY_STRING]);

            if (Request[AppConsts.MESSAGE_ID_QUERY_STRING].IsNullOrEmpty() || Request[AppConsts.IS_HIGH_IMPORTANCE_QUERY_STRING].IsNullOrEmpty())
            {
                return;
            }
            if (!Request.QueryString[AppConsts.IS_DASHBOARD_MESSAGE_QUERY_STRING].IsNullOrEmpty())
            {
                _isDashboardMessage = true;
            }

            BindMessage();
        }

        /// <summary>
        /// Bind the selected message details using Message.aspx in Pane & attached document
        /// </summary>
        private void BindMessage()
        {
            String _communicationType = String.Empty;
            String myscript = "var currentUserID = " + SysXWebSiteUtils.SessionService.OrganizationUserId + "; var queueType = " + Convert.ToString(Session[AppConsts.SESSION_QUEUE_TYPE_KEY] ?? "1")
                + "; var backgroundProcessUserId = " + BackgroundProcessUserId.ToString() + "; var systemCommunicationUserId = " + SytemCommunicationUserId.ToString()
                + ";var communicationType='" + Convert.ToString(Request[AppConsts.COMMUNICATION_TYPE_QUERY_STRING])
                + "';var fromID = '" + Convert.ToString(Request[AppConsts.FROM_ID_QUERY_STRING]) + "';";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", myscript, true);

            _communicationType = Convert.ToString(Request[AppConsts.COMMUNICATION_TYPE_QUERY_STRING]);
            Guid currentMessageId = new Guid(Request[AppConsts.MESSAGE_ID_QUERY_STRING]);
            hdnMessageId.Value = Convert.ToString(currentMessageId);

            Boolean isHighImportanceMessage = Convert.ToBoolean(Request[AppConsts.IS_HIGH_IMPORTANCE_QUERY_STRING]) ? true : false;
            ((RadToolBarButton)WclToolBar1.FindItemByValue("HighImportance")).Visible = isHighImportanceMessage;

            WclPane3.ContentUrl = "Message.aspx?" + AppConsts.MESSAGE_ID_QUERY_STRING + "=" + currentMessageId + "&isDashboardMessage=" + _isDashboardMessage;

            RadToolBarSplitButton rsbtnAttachments = WclToolBar1.Items.FindItemByValue("Attachment") as RadToolBarSplitButton;
            List<ADBMessageDocument> attachmentDocument = MessageManager.GetMessageAttachment(currentMessageId);

            if (!attachmentDocument.IsNullOrEmpty())
            {
                foreach (var item in attachmentDocument)
                {
                    String iconURL = "~/App_Themes/Default/images/icons/filetypes/" + MessageManager.GetAttachmentIcon(item.OriginalDocumentName.Substring(item.OriginalDocumentName.LastIndexOf(".")));
                    Boolean FileExists = CommonFileManager.DoesDocExist((item.DocumentName));
                    rsbtnAttachments.Buttons.Add(new RadToolBarButton { Text = item.OriginalDocumentName, ImageUrl = iconURL, ImagePosition = ToolBarImagePosition.Left, Enabled = FileExists, NavigateUrl = "AttachmentDownload.aspx?" + AppConsts.FILE_NAME_QUERY_STRING + "=" + item.DocumentName + "&" + AppConsts.ORIGINAL_FILE_NAME_QUERY_STRING + "=" + item.OriginalDocumentName, ToolTip = !FileExists ? "System could not download attachment as it is not available" : String.Empty });
                }
            }
            else
                rsbtnAttachments.Visible = false;

            SetToolBarForDashboardMessage(isHighImportanceMessage, rsbtnAttachments);
        }

        private void SetToolBarForDashboardMessage(Boolean isHighImportanceMessage, RadToolBarSplitButton rsbtnAttachments)
        {
            if (_isDashboardMessage == true)
            {
                //Bug # 4810 and 4812: Removed non functional buttons from pop up opened from Dashboard.
                //Bug # 4876: Display High Importance and/ or attachments with message opened from Dashboard.
                if (isHighImportanceMessage == true || rsbtnAttachments.Visible == true)
                {
                    WclToolBar1.Items.FindItemByText("Print").Visible = false;
                    WclToolBar1.Items.FindItemByText("PrintSeparator").Visible = false;
                    WclToolBar1.Items.FindItemByText("Reply").Visible = false;
                    WclToolBar1.Items.FindItemByText("Reply All").Visible = false;
                    WclToolBar1.Items.FindItemByText("Forward").Visible = false;
                    WclToolBar1.Items.FindItemByText("Delete").Visible = false;
                    WclToolBar1.Items.FindItemByText("Separator").Visible = false;
                }
                else
                {
                    WclToolBar1.Visible = false;
                }
            }
            else
            {
                WclToolBar1.Items.FindItemByText("PrintSeparator").Text = null;
                WclToolBar1.Items.FindItemByText("Separator").Text = null;
            }
        }


        public MessageViewerPresenter Presenter
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

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

    }
}

