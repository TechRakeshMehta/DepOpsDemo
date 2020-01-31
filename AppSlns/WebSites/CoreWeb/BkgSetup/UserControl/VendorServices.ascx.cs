using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgSetup.Views;
using Telerik.Web.UI;
using INTSOF.Utils;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;

namespace CoreWeb.BkgSetup.UserControl.Views
{
    public partial class VendorServices : BaseUserControl, IVendorServicesView
    {
        #region VARIABLES
        private VendorServicesPresenter _presenter = new VendorServicesPresenter();
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _tenantid;
        private String _viewType;
        #endregion

        #region PROPERTIES
        public int DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        public int TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    _tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        public int CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// CurrentViewContext
        /// </summary>
        IVendorServicesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public bool IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.HasValue ? _isAdminLoggedIn.Value : true;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        public int VendorID
        {
            get
            {
                return Convert.ToInt32(ViewState["VendorId"]);
            }
            set
            {
                ViewState["VendorId"] = value;
            }
        }

        public IList<Entity.ExternalBkgSvc> ExternalBkgServices
        {
            get;
            set;
        }

        public IList<Entity.ExternalBkgSvcAttribute> ExternalBkgServiceAttributes
        {
            get;
            set;
        }
        #endregion

        #region PRESENTER
        public VendorServicesPresenter Presenter
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

        #region EVENTS

        #region PAGE EVENTS

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.Title = "Vendor Services";
                base.SetPageTitle("Vendor Services");
                base.OnInit(e);
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

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
               
                Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    encryptedQueryString.ToDecryptedQueryString(Request.QueryString["args"]);
                }
                VendorID = encryptedQueryString.ContainsKey("VendorID") ?
                    Int32.Parse(encryptedQueryString["VendorID"]) : AppConsts.NONE;
                hdnVendorId.Value = VendorID.ToString();
                if (VendorID == AppConsts.ONE)
                {
                    btnImportServices.Visible = true;
                    //divImportSvc.Visible = true;
                }
                ApplyActionLevelPermission(ActionCollection, "Vendor Services");
            }
        }
        #endregion

        #region GRID EVENTS

        protected void grdExternalBkgSvc_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.FetchExternalBkgService();
                grdExternalBkgSvc.DataSource = ExternalBkgServices;
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

        protected void grdExternalBkgSvc_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            try
            {
                GridDataItem gridDataItem = (GridDataItem)e.DetailTableView.ParentItem;
                Int32 EBS_ID = Convert.ToInt32(Convert.ToString(gridDataItem.GetDataKeyValue("EBS_ID")));
                Presenter.RetrievingServiceAttributes(EBS_ID);
                grdExternalBkgSvc.MasterTableView.DetailTables[AppConsts.NONE].DataSource = CurrentViewContext.ExternalBkgServiceAttributes;
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

        protected void CmdBarCancel_Click(Object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.VendorAccount}
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }
        #endregion

        #region ACTION PERMISSION
        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

        }

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Import Services";
                objClsFeatureAction.CustomActionLabel = "Import Services";
                objClsFeatureAction.ScreenName = "Vendor Services";
                actionCollection.Add(objClsFeatureAction);

                return actionCollection;
            }
        }

        private void ApplyPermisions()
        {
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Import Services")
                                {
                                    btnImportServices.Visible = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }
        #endregion

    }
}