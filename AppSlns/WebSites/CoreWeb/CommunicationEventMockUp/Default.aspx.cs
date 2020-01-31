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
using CoreWeb.CommunicationEventMockUp.Views;
using Microsoft.Practices.ObjectBuilder;

namespace CoreWeb.CommunicationEventMockUp.Views
{
    public partial class CommunicationEventMockUpDefault : BasePage, IDefaultView
	{
		private DefaultViewPresenter _presenter=new DefaultViewPresenter();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();
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
        /// Page OnInitComplete event.
        /// </summary>
        /// <param name="e">Event</param>
        protected override void OnInitComplete(EventArgs e)
        {
            try
            {
                base.dynamicPlaceHolder = phDynamic;
                base.OnInitComplete(e);
                SetModuleTitle("Communication");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}
