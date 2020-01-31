using System;
using Microsoft.Practices.ObjectBuilder;

namespace CoreWeb.Messaging.MasterPages
{
	public partial class ModuleMaster : System.Web.UI.MasterPage, IModuleMasterView
	{
		private ModuleMasterPresenter _presenter=new ModuleMasterPresenter();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();
		}

		
		public ModuleMasterPresenter Presenter
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
	}
}
