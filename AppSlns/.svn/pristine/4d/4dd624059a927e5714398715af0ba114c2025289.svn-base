using System;
using Microsoft.Practices.ObjectBuilder;
using System.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageCompliance : BaseUserControl, IManageComplianceView
	{
		private ManageCompliancePresenter _presenter=new ManageCompliancePresenter();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();
            Response.Redirect("Pages/Setup.aspx");   
		}

		
		public ManageCompliancePresenter Presenter
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

