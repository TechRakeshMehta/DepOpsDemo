using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using CoreWeb.Shell;
using Telerik.Web.UI;
using Entity;
using INTSOF.Utils;

namespace CoreWeb.Messaging.Views
{
    public partial class TransferRules : System.Web.UI.Page, ITransferRulesView
    {
        private TransferRulesPresenter _presenter=new TransferRulesPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }

        
        public TransferRulesPresenter Presenter
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

