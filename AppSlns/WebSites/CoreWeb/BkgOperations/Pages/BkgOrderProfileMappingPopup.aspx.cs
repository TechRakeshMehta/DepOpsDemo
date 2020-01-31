using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderProfileMappingPopup : BaseWebPage, IBkgOrderProfileMappingPopupView
    {
        #region Variables

        #region Private Variables

        BkgOrderProfileMappingPopupPresenter _presenter = new BkgOrderProfileMappingPopupPresenter();

        #endregion

        #region Public Variables

        public IBkgOrderProfileMappingPopupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public BkgOrderProfileMappingPopupPresenter Presenter
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

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        Int32 IBkgOrderProfileMappingPopupView.TenantID
        {
            get
            {
                if (!ViewState["TenantID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["TenantID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantID"] = value;
                ucBkgOrderProfileMapping.TenantID = value;
            }
        }

        Boolean IBkgOrderProfileMappingPopupView.IsLinkProfile
        {
            get
            {
                if (!ViewState["IsLinkProfile"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsLinkProfile"]);
                return false;
            }
            set
            {
                ViewState["IsLinkProfile"] = value;
                ucBkgOrderProfileMapping.IsLinkProfile = value;
            }
        }

        Int32 IBkgOrderProfileMappingPopupView.OrderID
        {
            get
            {
                if (!ViewState["OrderID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["OrderID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["OrderID"] = value;
                ucBkgOrderProfileMapping.OrderID = value;
            }
        }

        Int32 IBkgOrderProfileMappingPopupView.PackageServiceLineItemID
        {
            get
            {
                if (!ViewState["PackageServiceLineItemID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["PackageServiceLineItemID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PackageServiceLineItemID"] = value;
                ucBkgOrderProfileMapping.PackageServiceLineItemID = value;
            }
        }

        #endregion

        #region Public Properties

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CaptureQueryStringAndSetParameters();
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

        #region Methods

        #region Private Methods

        private void CaptureQueryStringAndSetParameters()
        {

            if (!Request.QueryString["IsLinkProfile"].IsNullOrEmpty())
            {
                CurrentViewContext.IsLinkProfile = Convert.ToBoolean(Request.QueryString["IsLinkProfile"]);
            }
            
            if (!Request.QueryString["PackageServiceLineItemID"].IsNullOrEmpty())
            {
                CurrentViewContext.PackageServiceLineItemID = Convert.ToInt32(Request.QueryString["PackageServiceLineItemID"]);
            }

            CurrentViewContext.TenantID = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            CurrentViewContext.OrderID = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion





    }
}