using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using CoreWeb.Shell.Views;


namespace CoreWeb.PlacementMatching.Views
{
    public partial class PlacementMatchingDefault : BasePage, IDefaultView
    {
        private DefaultViewPresenter _presenter = new DefaultViewPresenter();
        
        public DefaultViewPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            try
            {
                base.dynamicPlaceHolder = phDynamic;
                base.OnInitComplete(e);
                SetModuleTitle("Placement Matching");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}