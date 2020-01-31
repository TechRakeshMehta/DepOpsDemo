using System;
using Microsoft.Practices.ObjectBuilder;

namespace CoreWeb.Messaging.Views
{
	public partial class MessageTypeUserGroup : System.Web.UI.UserControl, IMessageTypeUserGroupView
	{
		private MessageTypeUserGroupPresenter _presenter=new MessageTypeUserGroupPresenter();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();
		}

		
		public MessageTypeUserGroupPresenter Presenter
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

