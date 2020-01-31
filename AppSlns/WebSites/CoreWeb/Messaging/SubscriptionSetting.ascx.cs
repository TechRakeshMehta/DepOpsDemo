using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using INTSOF.Utils;
using System.Web.UI.WebControls;
using Entity;
using CoreWeb.Shell;
using System.Linq;


namespace CoreWeb.Messaging.Views
{
	public partial class SubscriptionSetting : System.Web.UI.UserControl, ISubscriptionSettingView
	{
        private SubscriptionSettingPresenter _presenter=new SubscriptionSettingPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            lblSuccess.Visible = false;
            lblSuccess.Text = String.Empty;
        }

        
        public SubscriptionSettingPresenter Presenter
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
        public int OrganizationUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IEnumerable<lkpCommunicationEvent> NotificationCommunicationEvents
        {
            set
            {
                cblNotification.DataSource = value;
                cblNotification.DataBind();
            }
        }


        public IEnumerable<lkpCommunicationEvent> ReminderCommunicationEvents
        {
            set
            {
                cblReminder.DataSource = value;
                cblReminder.DataBind();
            }
        }


        public IEnumerable<Int32> SelectedNotificationCommunicationEvents
        {
            get
            {
                return cblNotification.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => Int32.Parse(item.Value));
            }
            set
            {
                if (value != null)
                {
                    foreach (ListItem item in cblNotification.Items)
                    {
                        if (value.Contains(Int32.Parse(item.Value)))
                            item.Selected = true;
                    }
                }
            }
        }

        public IEnumerable<Int32> SelectedReminderCommunicationEvents
        {
            get
            {
                return cblReminder.Items.Cast<ListItem>().Where(item => item.Selected).Select(item => Int32.Parse(item.Value));
            }
            set
            {
                if (value != null)
                {
                    foreach (ListItem item in cblReminder.Items)
                    {
                        if (value.Contains(Int32.Parse(item.Value)))
                            item.Selected = true;
                    }
                }
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Presenter.SaveUserCommunicationSubscriptionSettings();
            lblSuccess.Visible = true;
            lblSuccess.ShowMessage("Settings saved successfully.", MessageType.SuccessMessage);
            Response.Redirect("~/Messaging/Pages/Messaging.aspx");
        }
	}
}

