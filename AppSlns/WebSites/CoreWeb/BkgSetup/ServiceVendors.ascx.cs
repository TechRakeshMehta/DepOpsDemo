using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ServiceVendors : BaseUserControl, IServiceVendorsView
    {
        #region VARIABLES

        private ServiceVendorsPresenter _presenter = new ServiceVendorsPresenter();
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _tenantid;
        private String _viewType;

        #endregion

        #region PRESENTER
        public ServiceVendorsPresenter Presenter
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

        public VendorsDetailsContract VendorsDetailsContract
        {
            get;
            set;
        }

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

        public List<Entity.Tenant> TenantsList
        {
            get;
            set;
        }

        public int CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
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

        public IList<Entity.ExternalVendor> ServiceVendorsList
        {
            get;
            set;
        }

        public IServiceVendorsView CurrentViewContext
        {
            get { return this; }
        }

        public String ErrorMessage
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
                base.OnInit(e);
                base.Title = "Service Vendors";
                base.SetPageTitle("Service Vendors");
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
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    ApplyActionLevelPermission(ActionCollection, "Service Vendors");
                }
                base.SetPageTitle("Service Vendors");
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

        #region GRID EVENTS
        protected void grdServiceVendors_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.FetchServiceVendors();
                grdServiceVendors.DataSource = ServiceVendorsList;
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

        protected void grdServiceVendors_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Boolean isUpdated = true;
                Int32 serviceVendorsID = (Int32)(e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EVE_ID"]; //Convert.ToInt32(e..GetDataKeyValue("BOCS_ID"));
                VendorsDetailsContract vendorsDetailsContract = new VendorsDetailsContract();
                vendorsDetailsContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                vendorsDetailsContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                vendorsDetailsContract.ContactName = (e.Item.FindControl("txtContactName") as WclTextBox).Text.Trim();
                vendorsDetailsContract.ContactPhone = (e.Item.FindControl("txtContactPhone") as WclMaskedTextBox).Text.Trim();
                vendorsDetailsContract.ContactEmail = (e.Item.FindControl("txtContactEmail") as WclTextBox).Text.Trim();
                isUpdated = Presenter.UpdateServiceVendors(vendorsDetailsContract, serviceVendorsID);
                if (isUpdated)
                {
                    base.ShowSuccessMessage("Service Vendor updated successfully.");
                }
                else
                {
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exists.", vendorsDetailsContract.Name), MessageType.Error);
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Service Vendor updation failed.");
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

        protected void grdServiceVendors_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Boolean isDeleted = true;
                Int32 serviceVendorsID = (Int32)(e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EVE_ID"]; //Convert.ToInt32(e..GetDataKeyValue("BOCS_ID"));
                isDeleted = Presenter.DeleteServiceVendors(serviceVendorsID);
                if (isDeleted)
                {
                    base.ShowSuccessMessage("Service Vendor deleted successfully.");
                }
                else
                {
                    base.ShowErrorInfoMessage("Service Vendor deletion failed.");
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

        protected void grdServiceVendors_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Boolean isInserted = true;
                VendorsDetailsContract vendorsDetailsContract = new VendorsDetailsContract();
                vendorsDetailsContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                vendorsDetailsContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                vendorsDetailsContract.ContactName = (e.Item.FindControl("txtContactName") as WclTextBox).Text.Trim();
                vendorsDetailsContract.ContactPhone = (e.Item.FindControl("txtContactPhone") as WclMaskedTextBox).Text.Trim();
                vendorsDetailsContract.ContactEmail = (e.Item.FindControl("txtContactEmail") as WclTextBox).Text.Trim();
                isInserted = Presenter.SaveServiceVendors(vendorsDetailsContract);
                if (isInserted)
                {
                    base.ShowSuccessMessage("Service Vendor inserted successfully.");
                }
                else
                {
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exists.", vendorsDetailsContract.Name), MessageType.Error);
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Service Vendor insertion failed.");
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
                objClsFeatureAction.ScreenName = "Service Vendors";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Delete";
                objClsFeatureAction.CustomActionLabel = "Delete";
                objClsFeatureAction.ScreenName = "Service Vendors";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Edit";
                objClsFeatureAction.CustomActionLabel = "Edit";
                objClsFeatureAction.ScreenName = "Service Vendors";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Manage Account";
                objClsFeatureAction.CustomActionLabel = "Manage Account";
                objClsFeatureAction.ScreenName = "Service Vendors";
                actionCollection.Add(objClsFeatureAction);

                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Vendor Services";
                objClsFeatureAction.CustomActionLabel = "Vendor Services";
                objClsFeatureAction.ScreenName = "Service Vendors";
                actionCollection.Add(objClsFeatureAction);

                return actionCollection;
            }
        }

        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(@"~/BkgSetup/UserControl/VendorAccountSettings.ascx");
                childScreenPathCollection.Add(@"~/BkgSetup/UserControl/VendorServices.ascx");
                return childScreenPathCollection;
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
                                    grdServiceVendors.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdServiceVendors.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdServiceVendors.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Manage Account")
                                {
                                    grdServiceVendors.MasterTableView.GetColumn("ManageAccount").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Vendor Services")
                                {
                                    grdServiceVendors.MasterTableView.GetColumn("VendorServices").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }
        #endregion

        protected void grdServiceVendors_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ManageAccount")
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                String VendorID = (e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EVE_ID"].ToString();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.VendorAccountSetting},
                                                                    { "VendorID",Convert.ToString(VendorID)},
                                                                    
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }

            if (e.CommandName == "VendorServices")
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                String VendorID = (e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EVE_ID"].ToString();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.VendorServices},
                                                                    { "VendorID",Convert.ToString(VendorID)},
                                                                    
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }

        }

        protected void grdServiceVendors_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    if (e.Item.OwnerTableView.Columns.FindByUniqueNameSafe("EVE_ContactPhone") != null)
                    {
                        dataItem["EVE_ContactPhone"].Text = Presenter.GetFormattedPhoneNumber(Convert.ToString(dataItem["EVE_ContactPhone"].Text));
                    }
                }
            }
        }
    }
}