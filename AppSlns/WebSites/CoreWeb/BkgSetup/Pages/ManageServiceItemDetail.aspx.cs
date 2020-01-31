using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageServiceItemDetail : BaseWebPage, IManageServiceItemDetailView
    {
        #region Private variables
        private ManageServiceItemDetailPresenter _presenter = new ManageServiceItemDetailPresenter();
        #endregion

        #region Properties

        #region Public Properties


        public ManageServiceItemDetailPresenter Presenter
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

        public IManageServiceItemDetailView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 PackageServiceItemId
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

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!this.IsPostBack)
                {
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    Presenter.OnViewInitialized();
                    CurrentViewContext.PackageServiceItemId = Convert.ToInt32(Request.QueryString["Id"]);

                    SetServiceItemDetailUCProperties();
                    SetServiceItemEntityRecordUCProperties();
                    SetRuleSetListUCProperties();
                    SetServiceItemFeeItemUCProperties();
                }
                SetServiceItemCustomFormUCProperties();
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

        protected void btnDoPostBack_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #endregion

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
                    ucEditServiceItem.TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                }
                if (!Request.QueryString["Id"].IsNullOrEmpty())
                {
                    ucEditServiceItem.PSI_ID = Convert.ToInt32(Request.QueryString["Id"]);
                }
            }
        }
        /// <summary>
        /// Method to set service item detail user control properties.
        /// </summary>
        private void SetServiceItemEntityRecordUCProperties()
        {
            if (!IsPostBack)
            {
                ucSrvcItemEntityRecords.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                ucSrvcItemEntityRecords.PackageServiceItemId = CurrentViewContext.PackageServiceItemId;
            }
        }

        private void SetRuleSetListUCProperties()
        {
            ucRuleSetList.ObjectId = CurrentViewContext.PackageServiceItemId;
            ucRuleSetList.ObjectTypeId = Presenter.getObjectTypeIdByCode(); ;
            ucRuleSetList.SelectedTenantId = CurrentViewContext.SelectedTenantId;
        }


        /// <summary>
        /// Method to set service item Fee Item user control properties.
        /// </summary>
        private void SetServiceItemFeeItemUCProperties()
        {
            ucSvcItemFeeItem.TenantId = CurrentViewContext.SelectedTenantId;
            ucSvcItemFeeItem.PackageServiceItemID = CurrentViewContext.PackageServiceItemId;
        }

        /// <summary>
        /// Method to set service item custom form user control properties.
        /// </summary>
        private void SetServiceItemCustomFormUCProperties()
        {
            PackageServiceItem pkgSvcItem = Presenter.GetPackageServiceItemData();
            ucSvcItemCustomForms.TenantId = CurrentViewContext.SelectedTenantId;
            ucSvcItemCustomForms.PackageServiceItemID = CurrentViewContext.PackageServiceItemId;
            if (pkgSvcItem.PSI_IsSupplemental.HasValue && pkgSvcItem.PSI_IsSupplemental.Value)
            {
                dvCustomForms.Visible = true;                
            }
            else
            {
                dvCustomForms.Visible = false;
            }
        }

        #endregion

        #region public Methods
        #endregion
        #endregion
    }
}