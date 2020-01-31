using System;
using Microsoft.Practices.ObjectBuilder;
using System.Text;
using System.Data.Entity.Core.Objects.DataClasses;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity;
using System.Collections.Generic;
using Telerik.Web.UI;

namespace CoreWeb.Messaging.Views
{
    public partial class MessageDetails : System.Web.UI.Page, IMessageDetailsView
    {
        private MessageDetailsPresenter _presenter=new MessageDetailsPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            this.Title = "Message Description";

            if (Request["messageID"].IsNullOrEmpty() && Request["queueType"].IsNullOrEmpty() && Request["currentUserId"].IsNullOrEmpty())
            {
                return;
            }
            String myscript = "var currentUserID = " + Convert.ToString(Request["currentUserId"]) + "; var queueType = " + Session["QueueType"].ToString() + ";var communicationType='" + Convert.ToString(Request["cType"]) + "';";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", myscript, true);

            Guid currentMessageId = new Guid(Request["messageID"]);
            hdnMessageId.Value = Convert.ToString(currentMessageId);
            EntityObject message = MessageManager.GetMessageContents(Convert.ToInt32(Request["queueType"]), currentMessageId);
            String str = String.Empty;
            String toListName = String.Empty;
            StringBuilder messageContent = new StringBuilder();
            Int32 messageType = AppConsts.NONE;
            Int32 tenantType = 1;

            Dictionary<String, String> dicAttachments = new Dictionary<String, String>();

            MessageDetail.MakeReplyMessageHistoryContent(message, new List<Int32>(), new List<Int32>(), out str, Convert.ToInt32(Request["currentUserId"]), MessagingAction.Detail, messageContent, out tenantType, out messageType, out toListName, out dicAttachments, true);

            RadToolBarSplitButton rsbtnAttachments = RadBtn.Items.FindItemByValue("Attachment") as RadToolBarSplitButton;

            if (dicAttachments.Count > 0)
            {
                foreach (var attachment in dicAttachments)
                {
                    String iconURL = "~/App_Themes/Default/images/" + MessageManager.GetAttachmentIcon(attachment.Key.Substring(attachment.Key.LastIndexOf(".")));
                    rsbtnAttachments.Buttons.Add(new RadToolBarButton { Text = attachment.Value, ImageUrl = iconURL, ImagePosition = ToolBarImagePosition.Left, NavigateUrl = "AttachmentDownload.aspx?fileName=" + attachment.Key + "&originalFileName=" + attachment.Value });
                }
            }
            else
                rsbtnAttachments.Visible = false;

            lblMessageDetails.Text = messageContent.ToString();
        }

        
        public MessageDetailsPresenter Presenter
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

