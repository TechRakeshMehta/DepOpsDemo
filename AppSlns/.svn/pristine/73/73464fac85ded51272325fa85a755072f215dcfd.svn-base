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
using CoreWeb.Messaging.Views;
using Microsoft.Practices.ObjectBuilder;
using System.Web.Services;
using Telerik.Web.UI;
using System.Collections.Generic;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity;

using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Text;
using System.Web.Configuration;

namespace CoreWeb.Messaging.Views
{
    public partial class MessagingDefault : BasePage, IDefaultView
    {
        private DefaultViewPresenter _presenter=new DefaultViewPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
               Presenter.OnViewInitialized();
            }
            //this._presenter.OnViewLoaded();
            //base.HideTitleBars();
            //// base.SetModuleTitle("Messaging"); 

            //if (this.Master == null)
            //{
            //    return;
            //}

            //CoreWeb.Shell.MasterPages.Page master = this.Master as CoreWeb.Shell.MasterPages.Page;
            //if (master != null)
            //{
            //    master.HideTitleBars();
            //}
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.dynamicPlaceHolder = phDynamic;
            if (!Request.QueryString[AppConsts.CHILD].IsNull())
            {
                base.ControlName = Request.QueryString[AppConsts.CHILD];
            }
            base.OnInitComplete(e);
            base.SetModuleTitle("Communication Center");
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
    }
}
