using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects.DataClasses;
using INTSOF.Utils;
using System.Text;
using System.Collections.Generic;
using Entity;
using Business.RepoManagers;
using CoreWeb.Shell;


namespace CoreWeb.Messaging.Views
{
    public partial class Message : BaseWebPage, IMessageView
    {
        private MessagePresenter _presenter=new MessagePresenter();
        Boolean _isDashBoardMessage = false;
        Guid _currentMessageId;
        Int32 _currentUserId;
        Int32 _queueType;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            if (Request[AppConsts.MESSAGE_ID_QUERY_STRING].IsNullOrEmpty())
            {
                return;
            }

            if (!Request[AppConsts.IS_DASHBOARD_MESSAGE_QUERY_STRING].IsNullOrEmpty())
            {
                _isDashBoardMessage = true;
            }

            _currentMessageId = new Guid(Request[AppConsts.MESSAGE_ID_QUERY_STRING]);
            _currentUserId = SysXWebSiteUtils.SessionService.OrganizationUserId;
            _queueType = Convert.ToInt32(Session[AppConsts.SESSION_QUEUE_TYPE_KEY]);
            GetMessageDetails();
        }

        /// <summary>
        /// Get the details of the selected message
        /// </summary>
        private void GetMessageDetails()
        {
            EntityObject message = null;

            if (_isDashBoardMessage)
                message = MessageManager.GetMessageContents(Convert.ToInt32(Session[AppConsts.SESSION_QUEUE_TYPE_KEY]), _currentMessageId, _currentUserId, _isDashBoardMessage);
            else
                message = MessageManager.GetMessageContents(Convert.ToInt32(Session[AppConsts.SESSION_QUEUE_TYPE_KEY]), _currentMessageId, _currentUserId);

            String str = String.Empty;
            String toListName = String.Empty;
            StringBuilder messageContent = new StringBuilder();

            Int32 messageType = AppConsts.NONE;
            Int32 tenantType = AppConsts.ONE;

            Dictionary<String, String> dicAttachments = new Dictionary<String, String>();
            MessageDetail.MakeReplyMessageHistoryContent(message, new List<Int32>(), new List<Int32>(), out str, Convert.ToInt32(Request[AppConsts.CURRENT_USER_ID_QUERY_STRING]), MessagingAction.Detail, messageContent, out tenantType, out messageType, out toListName, out dicAttachments, true);
            //lblMessageDetails.Text = Convert.ToString(messageContent);
            messageContent.Replace("<em>", "<i>");
            messageContent.Replace("</em>", "</i>");
            messageContent.Replace("<strong>", "<b>");
            messageContent.Replace("</strong>", "</b>");
            //lblMessageDetails.Text = Server.HtmlDecode(Convert.ToString(messageContent));
            lblMessageDetails.Text = Convert.ToString(messageContent);

        }

        
        public MessagePresenter Presenter
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

