using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class AdminDataItemSearch : BaseUserControl, IAdminDataItemSearchView
	{
		private AdminDataItemSearchPresenter _presenter=new AdminDataItemSearchPresenter();

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Item Data Search";
                base.SetPageTitle("Item Data Search");

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            ucSearchControl.WorkQueue = WorkQueueType.DataItemSearch;
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}            
			Presenter.OnViewLoaded();
            BasePage page = (base.Page) as BasePage;
            page.SetModuleTitle("Manage Search");
		}

		
		public AdminDataItemSearchPresenter Presenter
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

