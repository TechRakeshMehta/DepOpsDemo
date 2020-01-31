using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;

namespace CoreWeb.Messaging.Views
{
	public partial class CommunicationNotificationPreview :System.Web.UI.Page, ICommunicationNotificationPreviewView
	{
        private CommunicationNotificationPreviewPresenter _presenter=new CommunicationNotificationPreviewPresenter();

        public Int32 SystemCommunicationId
        {
            get;
            set;
        }

        public String Subject
        {
            set
            {
                litSubject.Text = value;
            }
        }

        public String DetailedContent
        {
            set
            {
                litMessageContent.Text = value;
            }
        }

        public ICommunicationNotificationPreviewView CurrentContext
        {
            get { return this; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            if (!String.IsNullOrEmpty(Request.QueryString[AppConsts.SYSTEM_COMMUNICATION_ID_QUERY_STRING]))
            {
                CurrentContext.SystemCommunicationId = Convert.ToInt32(Request.QueryString[AppConsts.SYSTEM_COMMUNICATION_ID_QUERY_STRING]);
                Presenter.GetSystemNotificationDetails();
            }
        }

        
        public CommunicationNotificationPreviewPresenter Presenter
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

