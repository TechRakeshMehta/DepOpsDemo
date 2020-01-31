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
using CoreWeb.ComplianceAdministration.Views;
using Microsoft.Practices.ObjectBuilder;
using System.Web.Services;
using INTSOF.Utils;
using System.Linq;
using System.Collections.Generic;
using Business.RepoManagers;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public partial class BackgroundPackageAdministrationDefault : BasePage, IDefaultView
    {
        private DefaultViewPresenter _presenter = new DefaultViewPresenter();

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
                SetModuleTitle("Background Package Setup");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
