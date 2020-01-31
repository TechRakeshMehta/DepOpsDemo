using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CoreWeb.Student.Views;
using Microsoft.Practices.ObjectBuilder;

namespace CoreWeb.Student.Views
{
    public partial class StudentDefault : BasePage, IDefaultView
	{
		private DefaultViewPresenter _presenter=new DefaultViewPresenter();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();
            base.SetModuleTitle("Compliance Management");
		}

		
		public DefaultViewPresenter Presenter
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


        /// <summary>
        /// Raises the initialize complete event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInitComplete(EventArgs e)
        {
            base.dynamicPlaceHolder = this.plcDynamic;
            base.OnInitComplete(e);
        }

	}
}
