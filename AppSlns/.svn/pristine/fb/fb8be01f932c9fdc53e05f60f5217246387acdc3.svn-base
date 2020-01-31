#region NameSpace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;

#endregion

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderNote : BaseWebPage, IBkgOrderNoteView
    {
        #region Private variables
        private BkgOrderNotePresenter _presenter = new BkgOrderNotePresenter();
        #endregion

        #region Public Properties

        public BkgOrderNotePresenter Presenter
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

        public IBkgOrderNoteView CurrentViewContext
        {
            get { return this; }
        }

        Int32 IBkgOrderNoteView.OrderID
        {
            get
            {
                if (ViewState["OrderID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["OrderID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["OrderID"] = value;
            }
        }

        Int32 IBkgOrderNoteView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IBkgOrderNoteView.TenantID
        {
            get
            {
                if (ViewState["TenantId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }


        String IBkgOrderNoteView.Notes
        {
            get
            {
                if (!txtNotes.IsNullOrEmpty())
                {
                    return txtNotes.Text;
                }
                return String.Empty;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    txtNotes.Text = value;
                }
            }
        }

        List<BkgOrderQueueNotesContract> IBkgOrderNoteView.LstNotes { get; set; }

        #endregion

        #region Events

        #region page Load Event

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!Page.IsPostBack)
                {
                    _presenter.OnViewInitialized();
                    if (!Request.QueryString["OrderID"].IsNull())
                    {
                        CurrentViewContext.OrderID = Convert.ToInt32(Request.QueryString["OrderID"]);
                    }

                    if (!Request.QueryString["TenantId"].IsNull())
                    {
                        CurrentViewContext.TenantID = Convert.ToInt32(Request.QueryString["TenantId"]);
                    }
                }
                Page.Title = "Background Order Note";
                _presenter.OnViewLoaded();
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

        #endregion

        #region Button Events
        /// <summary>
        /// Save Client Status 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucFeatureClientStatus_SaveClick(object sender, EventArgs e)
        {
            try
            {
                //if (!CurrentViewContext.Notes.IsNullOrEmpty())
                //{
                if (Presenter.SaveBkgOrderNote())
                {
                    this.ShowSuccessMessage("Note saved successfully");
                    txtNotes.Text = String.Empty;
                    grdNotes.Rebind();
                }
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

        #endregion

        #region Grid Events

        protected void grdNotes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetBkgOrderNote();
                grdNotes.DataSource = CurrentViewContext.LstNotes.IsNullOrEmpty() ? new List<BkgOrderQueueNotesContract>() : CurrentViewContext.LstNotes;
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

        #endregion

        #endregion

        #region Private Method


        #endregion
    }
}