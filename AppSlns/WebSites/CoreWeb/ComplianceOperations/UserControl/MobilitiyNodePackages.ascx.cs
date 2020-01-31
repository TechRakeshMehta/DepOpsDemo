using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using System.Collections.Generic;
using System.Threading;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity.ClientEntity;
using System.Linq;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class MobilitiyNodePackages : BaseUserControl, IMobilitiyNodePackagesView
    {
        public delegate void GridCommandEvent(object sender, GridCommandEventArgs e);
        public event GridCommandEvent grdCommandEvent;

        public Int32 TenantId
        {
            get;
            set;
        }

        public Int32 SelectedNodeId
        {
            get;
            set;
        }

        public Int32 SelectedNodeDPPId
        {
            get;
            set;
        }


        private MobilitiyNodePackagesPresenter _presenter= new MobilitiyNodePackagesPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }

        
        public MobilitiyNodePackagesPresenter Presenter
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

        #region Grid Events

        protected void grdMobilityPackages_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            { }
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

        protected void grdMobilityPackages_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower() == "viewdetails" && grdCommandEvent.IsNotNull())
                {
                    grdCommandEvent(sender, e);
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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

        #endregion

        #region Methods

        public Boolean BindMobilityNodePackages()
        {
            List<MobilityNodePackages> _lst = null;
            if (this.TenantId > 0)
            {
                _lst = Presenter.GetMobilityPackages(this.SelectedNodeId, this.SelectedNodeDPPId, this.TenantId);
                grdMobilityPackages.DataSource = _lst;
                grdMobilityPackages.DataBind();
            }
            if (_lst.Count() > 0 && _lst.IsNotNull())
                return true;
            else
                return false;
        }

        #endregion
    }
}

