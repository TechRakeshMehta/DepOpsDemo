using System;
using Microsoft.Practices.ObjectBuilder;

namespace CoreWeb.ComplianceOperations.Views
{
	public partial class AssigneeExceptionItemSearch : System.Web.UI.UserControl, IAssigneeExceptionItemSearchView
	{
		private AssigneeExceptionItemSearchPresenter _presenter=new AssigneeExceptionItemSearchPresenter();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();
		}

		
		public AssigneeExceptionItemSearchPresenter Presenter
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

