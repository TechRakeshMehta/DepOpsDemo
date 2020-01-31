using CoreWeb.Shell;
using ExternalVendors.ClearStarVendor;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderProfileMapping : BaseUserControl, IBkgOrderProfileMappingView
    {

        #region Variables

        private BkgOrderProfileMappingPresenter _presenter = new BkgOrderProfileMappingPresenter();

        public delegate void ShowMessage();
        public event ShowMessage eventShowMessage;

        #endregion

        #region Properties

        #region Private Properties

        List<VendorProfileSvcLineItemContract> IBkgOrderProfileMappingView.lstLineItemsData
        {
            get
            {
                if (!ViewState["lstLineItemsData"].IsNullOrEmpty())
                    return (List<VendorProfileSvcLineItemContract>)ViewState["lstLineItemsData"];
                return new List<VendorProfileSvcLineItemContract>();
            }
            set
            {
                ViewState["lstLineItemsData"] = value;
            }
        }

        VendorProfileSvcLineItemContract IBkgOrderProfileMappingView.LineItemProfileData
        {
            get
            {
                if (!ViewState["LineItemProfileData"].IsNullOrEmpty())
                    return (VendorProfileSvcLineItemContract)ViewState["LineItemProfileData"];
                return new VendorProfileSvcLineItemContract();
            }
            set
            {
                ViewState["LineItemProfileData"] = value;
            }
        }

        List<Entity.ExternalVendor> IBkgOrderProfileMappingView.lstExtVendors
        {
            get
            {
                if (!ViewState["lstExtVendors"].IsNullOrEmpty())
                    return (List<Entity.ExternalVendor>)ViewState["lstExtVendors"];
                return new List<Entity.ExternalVendor>();
            }
            set
            {
                ViewState["lstExtVendors"] = value;
            }
        }

        Int32 IBkgOrderProfileMappingView.SelectedVendorID
        {
            get
            {
                if (!CurrentViewContext.lstExtVendors.IsNullOrEmpty())
                    return CurrentViewContext.lstExtVendors.Where(con => !con.EVE_IsDeleted && con.EVE_Code == "AAAA").FirstOrDefault().EVE_ID;
                return AppConsts.NONE;
                //if (!ViewState["SelectedVendorID"].IsNullOrEmpty())
                //    return Convert.ToInt32(ViewState["SelectedVendorID"]);
                //return AppConsts.NONE;
            }
            //set
            //{
            //    ViewState["SelectedVendorID"] = value;
            //}
        }

        Dictionary<Int32, String> IBkgOrderProfileMappingView.dicBkgPackages
        {
            get
            {
                if (!ViewState["dicBkgPackages"].IsNullOrEmpty())
                    return (Dictionary<Int32, String>)ViewState["dicBkgPackages"];
                return new Dictionary<Int32, String>();
            }
            set
            {
                ViewState["dicBkgPackages"] = value;
            }
        }


        Int32 IBkgOrderProfileMappingView.SelectedPackageID
        {
            get
            {
                if (!ViewState["SelectedPackageID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedPackageID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedPackageID"] = value;
            }
        }

        Dictionary<Int32, String> IBkgOrderProfileMappingView.dicBkgSvcGroups
        {
            get
            {
                if (!ViewState["dicBkgSvcGroups"].IsNullOrEmpty())
                    return (Dictionary<Int32, String>)ViewState["dicBkgSvcGroups"];
                return new Dictionary<Int32, String>();
            }
            set
            {
                ViewState["dicBkgSvcGroups"] = value;
            }
        }

        Int32 IBkgOrderProfileMappingView.SelectedSvcGroupID
        {
            get
            {
                if (!ViewState["SelectedSvcGroupID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedSvcGroupID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedSvcGroupID"] = value;
            }
        }

        Dictionary<Int32, String> IBkgOrderProfileMappingView.dicBkgServices
        {
            get
            {
                if (!ViewState["dicBkgServices"].IsNullOrEmpty())
                    return (Dictionary<Int32, String>)ViewState["dicBkgServices"];
                return new Dictionary<Int32, String>();
            }
            set
            {
                ViewState["dicBkgServices"] = value;
            }
        }

        Int32 IBkgOrderProfileMappingView.SelectedServiceID
        {
            get
            {
                if (!ViewState["SelectedServiceID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedServiceID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedServiceID"] = value;
            }
        }

        String IBkgOrderProfileMappingView.VendorProfileID
        {
            get;
            set;
        }

        String IBkgOrderProfileMappingView.VendorLineItemOrderID
        {
            get;
            set;
        }

        List<Entity.ClientEntity.lkpOrderLineItemResultStatu> IBkgOrderProfileMappingView.lstLineItemStatus
        {
            get
            {
                if (!ViewState["lstLineItemStatus"].IsNullOrEmpty())
                    return (List<Entity.ClientEntity.lkpOrderLineItemResultStatu>)ViewState["lstLineItemStatus"];
                return new List<Entity.ClientEntity.lkpOrderLineItemResultStatu>();
            }
            set
            {
                ViewState["lstLineItemStatus"] = value;
            }
        }

        Int32 IBkgOrderProfileMappingView.SelectedLineItemStatusID
        {
            get
            {
                if (!ViewState["SelectedLineItemStatusID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedLineItemStatusID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedLineItemStatusID"] = value;
            }
        }

        Int32 IBkgOrderProfileMappingView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        List<VendorProfileSvcLineItemContract> IBkgOrderProfileMappingView.lstCreatedVendorProfileSvcLineItem
        {
            get
            {
                if (!ViewState["lstCreatedVendorProfileSvcLineItem"].IsNullOrEmpty())
                    return (List<VendorProfileSvcLineItemContract>)ViewState["lstCreatedVendorProfileSvcLineItem"];
                return new List<VendorProfileSvcLineItemContract>();
            }
            set
            {
                ViewState["lstCreatedVendorProfileSvcLineItem"] = value;
            }
        }

        Boolean IBkgOrderProfileMappingView.IsProfileIDExists
        {
            get
            {
                if (!ViewState["IsProfileIDExists"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsProfileIDExists"]);
                return false;
            }
            set
            {
                ViewState["IsProfileIDExists"] = value;
            }
        }

        Boolean IBkgOrderProfileMappingView.IsVendorOrderIDExists
        {
            get
            {
                if (!ViewState["IsVendorOrderIDExists"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsVendorOrderIDExists"]);
                return false;
            }
            set
            {
                ViewState["IsVendorOrderIDExists"] = value;
            }
        }
        #endregion

        #region Public Properties

        public IBkgOrderProfileMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public BkgOrderProfileMappingPresenter Presenter
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

        public Int32 TenantID
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
            }
        }


        public Boolean IsLinkProfile
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
            }
        }

        public Int32 OrderID
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
            }
        }


        public Int32 PackageServiceLineItemID
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
            }
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
                    ManagePanelVisibity();
                    Presenter.GetLineItemsDataforOrderID();

                    if (!CurrentViewContext.PackageServiceLineItemID.IsNullOrEmpty() && CurrentViewContext.PackageServiceLineItemID > AppConsts.NONE)
                    {
                        Presenter.GetLineItemData();
                        BindExternalVendor(cmbExternalVendor);
                        BindPackage(cmbPackage);
                        BindOtherData(txtMappedVendorProfileID, txtMappedVendorOrderID);
                        BindLineItemStatus(cmbLineItemStatus);
                    }
                    //ManageControls();
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

        protected void grdExternalVendorProfileMapping_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);

                    WclComboBox cmbExternalVendor = editform.FindControl("cmbExternalVendor") as WclComboBox;
                    WclComboBox cmbPackage = editform.FindControl("cmbPackage") as WclComboBox;
                    WclComboBox cmbLineItemStatus = editform.FindControl("cmbLineItemStatus") as WclComboBox;
                    WclTextBox txtMappedVendorProfileIDGrd = editform.FindControl("txtMappedVendorProfileIDGrd") as WclTextBox;
                    WclTextBox txtVendorOrderIDGrd = editform.FindControl("txtVendorOrderIDGrd") as WclTextBox;

                    BindExternalVendor(cmbExternalVendor);
                    BindPackage(cmbPackage);
                    BindOtherData(txtMappedVendorProfileIDGrd, txtVendorOrderIDGrd);
                    BindLineItemStatus(cmbLineItemStatus);
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

        protected void grdExternalVendorProfileMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetSvcLineItemsCreated();
                grdExternalVendorProfileMapping.DataSource = CurrentViewContext.lstCreatedVendorProfileSvcLineItem;
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

        protected void grdExternalVendorProfileMapping_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName)
                {
                    //to save new service line and profile mapping.
                    WclComboBox cmbExternalVendor = e.Item.FindControl("cmbExternalVendor") as WclComboBox;
                    WclComboBox cmbPackage = e.Item.FindControl("cmbPackage") as WclComboBox;
                    WclComboBox cmbServiceGroup = e.Item.FindControl("cmbServiceGroup") as WclComboBox;
                    WclComboBox cmbServices = e.Item.FindControl("cmbServices") as WclComboBox;
                    WclComboBox cmbLineItemStatus = e.Item.FindControl("cmbLineItemStatus") as WclComboBox;

                    WclTextBox txtMappedVendorProfileIDGrd = e.Item.FindControl("txtMappedVendorProfileIDGrd") as WclTextBox;
                    WclTextBox txtVendorOrderIDGrd = e.Item.FindControl("txtVendorOrderIDGrd") as WclTextBox;

                    //if (!cmbExternalVendor.IsNullOrEmpty())
                    //    CurrentViewContext.SelectedVendorID = cmbExternalVendor.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbExternalVendor.SelectedValue);

                    if (!cmbPackage.IsNullOrEmpty())
                        CurrentViewContext.SelectedPackageID = cmbPackage.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbPackage.SelectedValue);

                    if (!cmbServiceGroup.IsNullOrEmpty())
                        CurrentViewContext.SelectedSvcGroupID = cmbServiceGroup.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbServiceGroup.SelectedValue);

                    if (!cmbServices.IsNullOrEmpty())
                        CurrentViewContext.SelectedServiceID = cmbServices.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbServices.SelectedValue);

                    if (!cmbLineItemStatus.IsNullOrEmpty())
                        CurrentViewContext.SelectedLineItemStatusID = cmbLineItemStatus.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbLineItemStatus.SelectedValue);

                    if (!txtMappedVendorProfileIDGrd.IsNullOrEmpty())
                    {
                        CurrentViewContext.VendorProfileID = String.IsNullOrEmpty(txtMappedVendorProfileIDGrd.Text) ? String.Empty : txtMappedVendorProfileIDGrd.Text.Trim();
                    }
                    if (!txtVendorOrderIDGrd.IsNullOrEmpty())
                    {
                        CurrentViewContext.VendorLineItemOrderID = String.IsNullOrEmpty(txtVendorOrderIDGrd.Text) ? String.Empty : txtVendorOrderIDGrd.Text.Trim();
                    }

                    String _resultMsg = String.Empty;
                    ClearStar cs = new ClearStar();
                    String accountNumber = String.Empty;
                    if (!CurrentViewContext.SelectedVendorID.IsNullOrEmpty() && CurrentViewContext.SelectedVendorID > AppConsts.NONE)
                    {
                        Entity.ExternalVendorAccount extVendorAccount = new Entity.ExternalVendorAccount();
                        extVendorAccount = Presenter.GetExternalVendorAccount();

                        if (!extVendorAccount.IsNullOrEmpty())
                            accountNumber = extVendorAccount.EVA_AccountNumber;
                    }

                    _resultMsg = cs.IsProfileAndOrderExist(accountNumber, CurrentViewContext.VendorProfileID, CurrentViewContext.VendorLineItemOrderID);

                    if (!String.IsNullOrEmpty(_resultMsg))
                    {
                        lblSuccess.ShowMessage(_resultMsg, MessageType.Error);
                        lblSuccess.Visible = true;
                        return;
                    }

                    if (Presenter.SaveProfileMapping())
                    {
                        //e.Canceled = false;
                        //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenProfileMappingPopup('" + pkgSivcLineItemID + "','" + IsLinkProfile + "');", true);
                        hdnIsSavedSuccessfully.Value = true.ToString();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);

                        // base.ShowSuccessMessage("Service line item is saved successfully.");
                    }
                    //else
                    //{
                    //    e.Canceled = true;
                    //    base.ShowErrorMessage("Some error has occured. Please try again.");
                    //}
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

        #region Dropdown Events

        protected void cmbPackage_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox cmbPackageGrd = sender as WclComboBox;
                if (!cmbPackageGrd.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(cmbPackageGrd.SelectedValue) > AppConsts.NONE)
                {
                    CurrentViewContext.SelectedPackageID = Convert.ToInt32(cmbPackageGrd.SelectedValue);
                }

                if (!cmbPackageGrd.IsNullOrEmpty())
                {
                    WclComboBox cmbServiceGroupGrd = cmbPackageGrd.Parent.NamingContainer.FindControl("cmbServiceGroup") as WclComboBox;
                    BindServiceGroup(cmbServiceGroupGrd);
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

        protected void cmbServiceGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox cmbServiceGroupGrd = sender as WclComboBox;
                if (!cmbServiceGroupGrd.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(cmbServiceGroupGrd.SelectedValue) > AppConsts.NONE)
                {
                    CurrentViewContext.SelectedSvcGroupID = Convert.ToInt32(cmbServiceGroupGrd.SelectedValue);
                }

                if (!cmbServiceGroupGrd.IsNullOrEmpty())
                {
                    WclComboBox cmbServicesGrd = cmbServiceGroupGrd.Parent.NamingContainer.FindControl("cmbServices") as WclComboBox;
                    BindServices(cmbServicesGrd);
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

        protected void cmb_DataBound(object sender, EventArgs e)
        {
            try
            {
                WclComboBox cmb = sender as WclComboBox;
                cmb.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
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

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
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

        protected void fsucCmdBarButton_SaveClick(object sender, EventArgs e)
        {
            try
            {
                //call Sp to update profile,lineItemorderID and LineItemStatusID.

                if (String.Equals(CurrentViewContext.VendorProfileID, txtMappedVendorProfileID.Text)
                    && String.Equals(CurrentViewContext.VendorLineItemOrderID, txtMappedVendorOrderID.Text)
                    && CurrentViewContext.SelectedLineItemStatusID == Convert.ToInt32(cmbLineItemStatus.SelectedValue))
                {
                    lblSuccess.ShowMessage("There is no change in any data. No Update is required.", MessageType.Error);
                    lblSuccess.Visible = true;
                    return;
                }

                if (!String.Equals(CurrentViewContext.VendorProfileID, txtMappedVendorProfileID.Text))
                {
                    CurrentViewContext.VendorProfileID = txtMappedVendorProfileID.Text.Trim();
                }
                if (!String.Equals(CurrentViewContext.VendorLineItemOrderID, txtMappedVendorOrderID.Text))
                {
                    CurrentViewContext.VendorLineItemOrderID = txtMappedVendorOrderID.Text.Trim();
                }

                String _resultMsg = String.Empty;
                ClearStar cs = new ClearStar();
                String accountNumber = String.Empty;
                if (!CurrentViewContext.SelectedVendorID.IsNullOrEmpty() && CurrentViewContext.SelectedVendorID > AppConsts.NONE)
                {
                    Entity.ExternalVendorAccount extVendorAccount = new Entity.ExternalVendorAccount();
                    extVendorAccount = Presenter.GetExternalVendorAccount();

                    if (!extVendorAccount.IsNullOrEmpty())
                        accountNumber = extVendorAccount.EVA_AccountNumber;
                }
                _resultMsg = cs.IsProfileAndOrderExist(accountNumber, CurrentViewContext.VendorProfileID, CurrentViewContext.VendorLineItemOrderID);

                if (!String.IsNullOrEmpty(_resultMsg))
                {
                    lblSuccess.ShowMessage(_resultMsg, MessageType.Error);
                    lblSuccess.Visible = true;
                    return;
                }

                if (CurrentViewContext.SelectedLineItemStatusID != Convert.ToInt32(cmbLineItemStatus.SelectedValue))
                    CurrentViewContext.SelectedLineItemStatusID = Convert.ToInt32(cmbLineItemStatus.SelectedValue);

                //CurrentViewContext.SelectedVendorID = cmbExternalVendor.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbExternalVendor.SelectedValue);
                CurrentViewContext.SelectedPackageID = cmbPackage.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbPackage.SelectedValue);
                CurrentViewContext.SelectedSvcGroupID = cmbServiceGroup.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbServiceGroup.SelectedValue);
                CurrentViewContext.SelectedServiceID = cmbServices.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbServices.SelectedValue);

                if (Presenter.SaveProfileMapping())
                {
                    //base.ShowSuccessMessage("Service line item is updated successfully.");
                    hdnIsUpdatesSuccessfully.Value = true.ToString();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
                }
                //else
                //{
                //    base.ShowErrorMessage("Some error has occured. Please try again.");
                //}
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

        private void ManagePanelVisibity()
        {
            if (CurrentViewContext.IsLinkProfile)
            {
                pnlLinkProfile.Visible = true;
                pnlAddNewLineItemMapping.Visible = false;
            }
            else
            {
                pnlLinkProfile.Visible = false;
                pnlAddNewLineItemMapping.Visible = true;
            }
        }

        private void ManageControls()
        {
            if (CurrentViewContext.IsLinkProfile)
            {
                cmbExternalVendor.Enabled = false;
                cmbPackage.Enabled = false;
                cmbServiceGroup.Enabled = false;
                cmbServices.Enabled = false;
            }
            else
            {
                cmbExternalVendor.Enabled = true;
                cmbPackage.Enabled = true;
                cmbServiceGroup.Enabled = true;
                cmbServices.Enabled = true;
            }
        }

        private void BindExternalVendor(WclComboBox cmb)
        {
            Presenter.GetExternalVendor();
            cmb.DataSource = CurrentViewContext.lstExtVendors;
            cmb.DataBind();
            if (CurrentViewContext.IsLinkProfile && !CurrentViewContext.PackageServiceLineItemID.IsNullOrEmpty() && CurrentViewContext.PackageServiceLineItemID > AppConsts.NONE && !CurrentViewContext.LineItemProfileData.IsNullOrEmpty())
            {
                //   CurrentViewContext.SelectedVendorID = CurrentViewContext.LineItemProfileData.ExtVendorID;
                cmb.SelectedValue = Convert.ToString(CurrentViewContext.SelectedVendorID);
                cmb.Enabled = !String.IsNullOrEmpty(cmb.SelectedValue) && Convert.ToInt32(cmb.SelectedValue) > AppConsts.NONE ? false : true;
            }
        }

        private void BindPackage(WclComboBox cmb)
        {
            Presenter.GetBkgOrderPackage();
            cmb.DataSource = CurrentViewContext.dicBkgPackages;
            cmb.DataBind();

            if (CurrentViewContext.IsLinkProfile && !CurrentViewContext.PackageServiceLineItemID.IsNullOrEmpty() && CurrentViewContext.PackageServiceLineItemID > AppConsts.NONE && !CurrentViewContext.LineItemProfileData.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedPackageID = CurrentViewContext.LineItemProfileData.BackgroundPackageID;
                cmb.SelectedValue = Convert.ToString(CurrentViewContext.SelectedPackageID);
                cmb.Enabled = !String.IsNullOrEmpty(cmb.SelectedValue) && Convert.ToInt32(cmb.SelectedValue) > AppConsts.NONE ? false : true;
                BindServiceGroup(cmbServiceGroup);
            }
        }

        private void BindServiceGroup(WclComboBox cmb)
        {
            Presenter.GetPakageServiceGroup();
            cmb.DataSource = CurrentViewContext.dicBkgSvcGroups;
            cmb.DataBind();
            if (CurrentViewContext.IsLinkProfile && !CurrentViewContext.PackageServiceLineItemID.IsNullOrEmpty() && CurrentViewContext.PackageServiceLineItemID > AppConsts.NONE && !CurrentViewContext.LineItemProfileData.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedSvcGroupID = CurrentViewContext.LineItemProfileData.ServiceGroupID;
                cmb.SelectedValue = Convert.ToString(CurrentViewContext.SelectedSvcGroupID);
                cmb.Enabled = !String.IsNullOrEmpty(cmb.SelectedValue) && Convert.ToInt32(cmb.SelectedValue) > AppConsts.NONE ? false : true;
                BindServices(cmbServices);
            }
        }

        private void BindServices(WclComboBox cmb)
        {
            Presenter.GetServices();
            cmb.DataSource = CurrentViewContext.dicBkgServices;
            cmb.DataBind();

            if (CurrentViewContext.IsLinkProfile && !CurrentViewContext.PackageServiceLineItemID.IsNullOrEmpty() && CurrentViewContext.PackageServiceLineItemID > AppConsts.NONE && !CurrentViewContext.LineItemProfileData.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedServiceID = CurrentViewContext.LineItemProfileData.ServiceID;
                cmb.SelectedValue = Convert.ToString(CurrentViewContext.SelectedServiceID);
                cmb.Enabled = !String.IsNullOrEmpty(cmb.SelectedValue) && Convert.ToInt32(cmb.SelectedValue) > AppConsts.NONE ? false : true;
            }
        }

        private void BindLineItemStatus(WclComboBox cmb)
        {
            Presenter.GetLineItemStatus();
            cmb.DataSource = CurrentViewContext.lstLineItemStatus;
            cmb.DataBind();
            if (CurrentViewContext.IsLinkProfile && !CurrentViewContext.PackageServiceLineItemID.IsNullOrEmpty() && CurrentViewContext.PackageServiceLineItemID > AppConsts.NONE && !CurrentViewContext.LineItemProfileData.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedLineItemStatusID = CurrentViewContext.LineItemProfileData.SvcLineItemStatusID;
                cmb.SelectedValue = Convert.ToString(CurrentViewContext.SelectedLineItemStatusID);
            }
        }


        private void BindOtherData(WclTextBox txtVendorProfileID, WclTextBox txtVendorOrderID)
        {
            if (CurrentViewContext.IsLinkProfile && !CurrentViewContext.PackageServiceLineItemID.IsNullOrEmpty() && CurrentViewContext.PackageServiceLineItemID > AppConsts.NONE && !CurrentViewContext.LineItemProfileData.IsNullOrEmpty())
            {
                CurrentViewContext.VendorProfileID = CurrentViewContext.LineItemProfileData.VendorProfileID;
                txtVendorProfileID.Text = CurrentViewContext.VendorProfileID;
                CurrentViewContext.VendorLineItemOrderID = CurrentViewContext.LineItemProfileData.VendorLineItemOrderID;
                txtVendorOrderID.Text = CurrentViewContext.VendorLineItemOrderID;
            }
            else
            {
                txtVendorProfileID.Text = String.Empty;
                txtVendorOrderID.Text = String.Empty;
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}