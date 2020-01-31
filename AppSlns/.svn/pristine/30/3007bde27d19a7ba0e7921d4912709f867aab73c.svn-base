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

#endregion

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderClientStatus : BaseWebPage, IBkgOrderClientStatusView
    {
        #region Private variables
        private BkgOrderClientStatusPresenter _presenter = new BkgOrderClientStatusPresenter();
        #endregion

        #region Public Properties

        public BkgOrderClientStatusPresenter Presenter
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

        public Int32 OrderID
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

        public Int32 ClientOrderStatusID
        {
            get
            {
                if (ViewState["ClientOrderStatusID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ClientOrderStatusID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ClientOrderStatusID"] = value;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 TenantID
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

        public Int32 SelectedClientStatus
        {
            get
            {
                if (ViewState["SelectedClientStatusId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["SelectedClientStatusId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedClientStatusId"] = value;
            }

        }

        public String Notes
        {
            get
            {
                if (!txtNotes.IsNullOrEmpty())
                {
                    return txtNotes.Text;
                }
                return String.Empty;
            }
        }

        #endregion

        #region Events
        #region page Load Event
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                _presenter.OnViewInitialized();
                if (!Request.QueryString["OrderID"].IsNull())
                {
                    OrderID = Convert.ToInt32(Request.QueryString["OrderID"]);
                }

                if (!Request.QueryString["ClientOrderStatusID"].IsNullOrEmpty())
                {
                    ClientOrderStatusID = Convert.ToInt32(Request.QueryString["ClientOrderStatusID"]);
                }
                if (!Request.QueryString["TenantId"].IsNull())
                {
                    TenantID = Convert.ToInt32(Request.QueryString["TenantId"]);
                }

                BindComboBox();
            }
            _presenter.OnViewLoaded();
        }

        #endregion

        #region Combobox Event
        /// <summary>
        /// Get selected client status 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void selected_IndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!rcOrderClientStatusType.SelectedValue.IsNullOrEmpty())
            {
                SelectedClientStatus = Convert.ToInt32(rcOrderClientStatusType.SelectedValue);
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
                if (SelectedClientStatus > 0)
                {
                    if (_presenter.UpdateOrderClientStatus(TenantID, OrderID, SelectedClientStatus, CurrentUserId))
                    {
                        if (!Notes.IsNullOrEmpty())
                        {
                            if (_presenter.SaveOrderClientStatusHistory(TenantID, OrderID, Notes, CurrentUserId))
                            {
                                grdBkgOrderClientStatus.Rebind();
                                txtNotes.Text = String.Empty;
                            }
                        }
                        this.ShowSuccessMessage("Client status saved successfully");
                    }
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
        /// <summary>
        /// Bind Grid BkgOrderClientStatus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdBkgOrderClientStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdBkgOrderClientStatus.DataSource = _presenter.GetClientOrderStatusHistory(TenantID, OrderID);
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
        /// <summary>
        /// Bind combo box
        /// </summary>
        private void BindComboBox()
        {
            try
            {
                rcOrderClientStatusType.DataSource = _presenter.GetBkgOrderClientStatus(TenantID);
                rcOrderClientStatusType.DataBind();
                rcOrderClientStatusType.Items.Insert(0, new RadComboBoxItem("--Select--"));

                if (ClientOrderStatusID > AppConsts.NONE && rcOrderClientStatusType.Items.Any(x => x.Value == Convert.ToString(ClientOrderStatusID)))
                {
                    rcOrderClientStatusType.FindItemByValue(ClientOrderStatusID.ToString()).Selected = true;
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
    }
}