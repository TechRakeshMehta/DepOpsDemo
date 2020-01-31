using System;
using Microsoft.Practices.ObjectBuilder;
using CoreWeb.Shell;
using INTSOF.Utils;


namespace CoreWeb.ComplianceOperations.Views
{
	public partial class AssigneeDataItemSearch : BaseUserControl, IAssigneeDataItemSearchView
	{
		private AssigneeDataItemSearchPresenter _presenter=new AssigneeDataItemSearchPresenter();

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
            ucSearchControl.AssignedToUserId = CurrentLoggedInUserId;
            ucSearchControl.WorkQueue = WorkQueueType.AssigneeDataItemSearch;

			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();
            BasePage page = (base.Page) as BasePage;
            page.SetModuleTitle("Manage Search");
		}

		
		public AssigneeDataItemSearchPresenter Presenter
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

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

		// TODO: Forward events to the presenter and show state to the user.
		// For examples of this, see the View-Presenter (with Application Controller) QuickStart:
		//	

	}
}

