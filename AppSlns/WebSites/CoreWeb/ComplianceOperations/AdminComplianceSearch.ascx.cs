using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
	public partial class AdminComplianceSearch : BaseUserControl, IAdminComplianceSearchView
	{
		private AdminComplianceSearchPresenter _presenter=new AdminComplianceSearchPresenter();


        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Compliance Search";
                base.SetPageTitle("Compliance Search");

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
		}

		
		public AdminComplianceSearchPresenter Presenter
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

