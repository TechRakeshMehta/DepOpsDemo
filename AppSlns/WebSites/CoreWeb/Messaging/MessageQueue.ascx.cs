using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using Entity;
using INTSOF.Utils;
using System.Data.Linq;
using System.Linq;
using Telerik.Web.UI;
using Business.RepoManagers;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.Messaging.Views
{
    public partial class MessageQueue : BaseUserControl, IMessageQueueView
    {
        private MessageQueuePresenter _presenter=new MessageQueuePresenter();
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Messaging/Pages/Messaging.aspx");   
        }
        
        public MessageQueuePresenter Presenter
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
    }
}

