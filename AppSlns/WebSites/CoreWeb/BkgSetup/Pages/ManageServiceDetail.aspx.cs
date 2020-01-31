using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageServiceDetail : BaseWebPage, IManageServiceDetailView
    {
        #region Private variables
        private ManageServiceDetailPresenter _presenter = new ManageServiceDetailPresenter();
        #endregion

        #region Properties

        public ManageServiceDetailPresenter Presenter
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

        public String ErrorMessage
        {
            set { base.ShowErrorMessage(value); }

        }

        public String InfoMessage
        {
            set { base.ShowInfoMessage(value); }
        }

        public String SuccessMessage
        {
            set { base.ShowSuccessMessage(value); }
        }

        public Int32 BkgPackageSvcId
        {
            get
            {
                return Convert.ToInt32(ViewState["PackageServiceItemId"]);
            }
            set
            {
                ViewState["PackageServiceItemId"] = value;
            }
        }

        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!Request.QueryString["SelectedTenantId"].IsNullOrEmpty())
                    {
                        SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    }

                    if (!Request.QueryString["Id"].IsNullOrEmpty())
                    {
                        BkgPackageSvcId = Convert.ToInt32(Request.QueryString["Id"]);
                    }
                    SetServiceItemDetailUCProperties();
                    SetRuleSetListUCProperties();
                    SetSerivceDetailUCProperties();
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

        #region Methods
        #region Private Methods

        /// <summary>
        /// Method to bind the user control properties from query String.
        /// </summary>
        private void SetServiceItemDetailUCProperties()
        {
            if (!IsPostBack)
            {
                if (!Request.QueryString["SelectedTenantId"].IsNullOrEmpty())
                {
                    ucServiceItemDetail.TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                }
                if (!Request.QueryString["ParentID"].IsNullOrEmpty())
                {
                    ucServiceItemDetail.ParentNodeId = Convert.ToInt32(Request.QueryString["ParentID"]);
                }
                if (!Request.QueryString["Id"].IsNullOrEmpty())
                {
                    ucServiceItemDetail.BkgPackageSvcId = Convert.ToInt32(Request.QueryString["Id"]);
                }
                if (!Request.QueryString["SrvcId"].IsNullOrEmpty())
                {
                    ucServiceItemDetail.BackgroundServiceId = Convert.ToInt32(Request.QueryString["SrvcId"]);
                }
            }
        }

        private void SetRuleSetListUCProperties()
        {
            ucRuleSetList.ObjectId = BkgPackageSvcId;
            ucRuleSetList.ObjectTypeId = Presenter.getObjectTypeIdByCode();
            ucRuleSetList.SelectedTenantId = SelectedTenantId;
        }

        private void SetSerivceDetailUCProperties()
        {
            ucServiceDetail.BkgPackageSrvcId = BkgPackageSvcId;
            ucServiceDetail.TenantId = SelectedTenantId;
        }
        #endregion

        #region public Methods
        #endregion
        #endregion
    }
}