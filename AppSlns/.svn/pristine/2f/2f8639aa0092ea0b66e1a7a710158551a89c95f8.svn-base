using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgSetup.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.UserControl.Views
{
    public partial class VendorAccountSettings : BaseUserControl, IVendorAccountSettingsView
    {
        #region VARIABLES

        private VendorAccountSettingsPresenter _presenter = new VendorAccountSettingsPresenter();
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _tenantid;
        private String _viewType;

        #endregion

        #region PRESENTER
        public VendorAccountSettingsPresenter Presenter
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

        #region PROPERTIES

        public Int32 DefaultTenantId
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

        public Int32 TenantId
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

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public Boolean IsAdminLoggedIn
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

        public IList<Entity.ExternalVendorAccount> VendorsAccountsList
        {
            get;
            set;
        }

        public Int32 VendorId
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

        public String AccountNumber
        {
            get;
            set;
        }

        public String AccountName
        {
            get;
            set;
        }

        public Int32 EvaID
        {
            get;
            set;
        }

        #endregion

        #region PAGE EVENTS

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.Title = "Vendor Account Setting";
                base.SetPageTitle("Vendor Account Setting");
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
                ApplyActionLevelPermission(ActionCollection, "Vendor Account Setting");
                Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    encryptedQueryString.ToDecryptedQueryString(Request.QueryString["args"]);
                }
                VendorId = encryptedQueryString.ContainsKey("VendorID") ?
                    Int32.Parse(encryptedQueryString["VendorID"]) : AppConsts.NONE;
            }
        }
        #endregion

        #region GRID EVENTS

        protected void grdVendorAccountSettings_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.FetchVendorAccSettings();
                grdVendorAccountSettings.DataSource = VendorsAccountsList;
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

        protected void grdVendorAccountSettings_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                String status = null;
                AccountNumber = (e.Item.FindControl("txtAccNumber") as WclTextBox).Text.Trim();
                AccountName = (e.Item.FindControl("txtAccName") as WclTextBox).Text.Trim();
                status = Presenter.SaveVendorsAccountDetail();
                if (status.IsNotNull())
                {
                    if (status.Trim().ToLower().Equals(("Account is successfully created.").Trim().ToLower()))
                        base.ShowSuccessMessage(status);

                    else
                        base.ShowErrorInfoMessage(status);
                }
                else
                {
                    base.ShowErrorInfoMessage("Vendor Account insertion failed.");
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

        protected void grdVendorAccountSettings_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                String status = null;
                EvaID = (Int32)(e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EVA_ID"];
                AccountNumber = (e.Item.FindControl("txtAccNumber") as WclTextBox).Text.Trim();
                AccountName = (e.Item.FindControl("txtAccName") as WclTextBox).Text.Trim();
                status = Presenter.UpdateVendorsAccountDetail();
                if (status.IsNotNull())
                {
                    if (status.Trim().ToLower().Equals(("Account is successfully updated.").Trim().ToLower()))
                        base.ShowSuccessMessage(status);

                    else
                        base.ShowErrorInfoMessage(status);
                }
                else
                {
                    base.ShowErrorInfoMessage("Vendor Account updation failed.");
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

        protected void grdVendorAccountSettings_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Boolean isDeleted = true;
                EvaID = (Int32)(e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EVA_ID"];
                isDeleted = Presenter.DeleteVendorsAccountDetail();
                if (isDeleted)
                {
                    base.ShowSuccessMessage("Vendor Account deleted successfully.");
                }
                else
                {
                    base.ShowErrorInfoMessage("Vendor Account deletion failed.");
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
                objClsFeatureAction.CustomActionId = "Add";
                objClsFeatureAction.CustomActionLabel = "Add";
                objClsFeatureAction.ScreenName = "Vendor Account Setting";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Delete";
                objClsFeatureAction.CustomActionLabel = "Delete";
                objClsFeatureAction.ScreenName = "Vendor Account Setting";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Edit";
                objClsFeatureAction.CustomActionLabel = "Edit";
                objClsFeatureAction.ScreenName = "Vendor Account Setting";
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
                                if (x.FeatureAction.CustomActionId == "Add")
                                {
                                    grdVendorAccountSettings.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdVendorAccountSettings.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdVendorAccountSettings.MasterTableView.GetColumn("EditCommandColumn").Display = false;
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