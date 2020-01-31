using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgOperations.Views;
using CoreWeb.Shell;
using CoreWeb.Shell.Views;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgPackagesOrdered : BaseUserControl, IBkgPackagesOrderedView
    {
        #region Variables

        private BkgPackagesOrderedPresenter _presenter = new BkgPackagesOrderedPresenter();

        #endregion

        #region Properties

        public IBkgPackagesOrderedView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public BkgPackagesOrderedPresenter Presenter
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

        Int32 IBkgPackagesOrderedView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            }
        }

        Int32 IBkgPackagesOrderedView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        Int32 IBkgPackagesOrderedView.OrderID
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
            }
        }

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                }
                //ucSvcLineItemMapping.eventShowDefaultOnCancel += new BkgOrderProfileMapping.ShowDefaultOnCancel(ShowDefaultOnCancel);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Page Events

        //protected void grdPackagesOrdered_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        //{
        //    grdPackagesOrdered.DataSource = Presenter.GetPackageByOrderId();
        //}

        protected void grdExternalVendorServices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<ExternalVendorServiceContract> externalVendorServiceContract = Presenter.GetExternalVendorServicesByOrderId();
            if (externalVendorServiceContract.IsNotNull())
            {
                grdExternalVendorServices.DataSource = externalVendorServiceContract;
            }
            else
            {
                grdExternalVendorServices.DataSource = String.Empty;
            }
        }

        protected void grdExternalVendorServices_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                if (dataItem["VendorStatus"].Text.Equals("Completed") || dataItem["VendorStatus"].Text.Equals("ADB-Completed"))
                {
                    dataItem["EditCommandColumn"].Controls[0].Visible = true;
                }
                else
                {
                    dataItem["EditCommandColumn"].Controls[0].Visible = false;
                }
            }

            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                //WclTextBox txtVendorServiceResults = gridEditableItem.FindControl("txtVendorServiceResults") as WclTextBox;
                TextBox txtVendorServiceResults = gridEditableItem.FindControl("txtVendorServiceResults") as TextBox;
                CheckBox IsVendorFlagged = gridEditableItem.FindControl("IsVendorFlagged") as CheckBox;
                Label lblVendorResultStatus = gridEditableItem.FindControl("lblVendorResultStatus") as Label;
                WclComboBox cmbServiceStatus = gridEditableItem.FindControl("cmbServiceStatus") as WclComboBox;

                Int32 PSLI_ID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("PSLI_ID"));
                OrderLineDetailsContract OrderLineDetailsContract = Presenter.GetBkgOrderLineItemDetails(PSLI_ID);
                ExtVendorBkgOrderLineItemDetail extVendorBkgOrderLineItemDetail = OrderLineDetailsContract.ExtVendorBkgOrderLineItemDetails;

                cmbServiceStatus.DataSource = Presenter.GetlkpOrderLineItemResultStatus();
                cmbServiceStatus.DataBind();
                cmbServiceStatus.AddFirstEmptyItem();

                if (!extVendorBkgOrderLineItemDetail.IsNullOrEmpty()) //check added in UAT-4498
                {
                    txtVendorServiceResults.Text = extVendorBkgOrderLineItemDetail.OLID_ResultText == String.Empty ? "No results for this order."
                                                                                                                   : extVendorBkgOrderLineItemDetail.OLID_ResultText;
                    if (extVendorBkgOrderLineItemDetail.OLID_FlaggedInd == true)
                    {
                        IsVendorFlagged.Checked = true;
                    }
                    
                    if (extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID.IsNotNull())
                    {
                        cmbServiceStatus.SelectedValue = extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID.ToString();
                    }
                }

                BkgOrderLineItemResultCopy bkgOrderLineItemResultCopy = OrderLineDetailsContract.BkgOrderLineItemResultCopy;
                if (bkgOrderLineItemResultCopy.IsNotNull())
                {
                    //WclTextBox txtADBServiceResults = gridEditableItem.FindControl("txtADBServiceResults") as WclTextBox;
                    TextBox txtADBServiceResults = gridEditableItem.FindControl("txtADBServiceResults") as TextBox;
                    CheckBox chkADBFlagged = gridEditableItem.FindControl("chkADBFlagged") as CheckBox;
                    txtADBServiceResults.Text = bkgOrderLineItemResultCopy.OLIR_ResultText;
                    if (bkgOrderLineItemResultCopy.OLIR_FlaggedInd == true)
                    {
                        chkADBFlagged.Checked = true;
                    }
                    if (bkgOrderLineItemResultCopy.OLIR_OrderLineItemResultStatusID.IsNotNull())
                    {
                        cmbServiceStatus.SelectedValue = bkgOrderLineItemResultCopy.OLIR_OrderLineItemResultStatusID.ToString();
                    }
                }
                CommandBar fsucCmdBarCategory = gridEditableItem.FindControl("fsucCmdBarCategory") as CommandBar;
                ApplyActionLevelPermission(fsucCmdBarCategory, "Vendor Services");
            }

        }

        protected void grdExternalVendorServices_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 PSLI_ID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("PSLI_ID"));
                // WclTextBox txtADBServiceResults = (e.Item.FindControl("txtADBServiceResults") as WclTextBox);
                TextBox txtADBServiceResults = (e.Item.FindControl("txtADBServiceResults") as TextBox);
                WclComboBox cmbServiceStatus = (e.Item.FindControl("cmbServiceStatus") as WclComboBox);
                CheckBox chkADBFlagged = (e.Item.FindControl("chkADBFlagged") as CheckBox);
                BkgOrderLineItemResultCopy bkgOrderLineItemResultCopy = new BkgOrderLineItemResultCopy();
                bkgOrderLineItemResultCopy.OLIR_BkgOrderPackageSvcLineItemID = PSLI_ID;
                bkgOrderLineItemResultCopy.OLIR_ResultText = txtADBServiceResults.Text.Trim();
                bkgOrderLineItemResultCopy.OLIR_OrderLineItemResultStatusID = Convert.ToInt32(cmbServiceStatus.SelectedValue);
                if (chkADBFlagged.Checked)
                {
                    bkgOrderLineItemResultCopy.OLIR_FlaggedInd = true;
                }
                else
                {
                    bkgOrderLineItemResultCopy.OLIR_FlaggedInd = false;
                }
                if (cmbServiceStatus.SelectedItem.Text == "Completed")
                {
                    bkgOrderLineItemResultCopy.OLIR_DateCompleted = DateTime.Now;
                }
                bkgOrderLineItemResultCopy.OLIR_CreatedByID = CurrentViewContext.CurrentLoggedInUserId;
                bkgOrderLineItemResultCopy.OLIR_CreatedOn = DateTime.Now;
                Presenter.UpdateRecordToADBCopy(bkgOrderLineItemResultCopy);

            }
        }

        #endregion

        #endregion

        #region Apply Permission
        private void ApplyActionLevelPermission(CommandBar fsucCmdBarCategory, string screenName = "")
        {
            List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions(fsucCmdBarCategory);
        }

        /// <summary>
        /// Set the permission on control based action permission 
        /// </summary>
        private void ApplyPermisions(CommandBar fsucCmdBarCategory)
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
                            {

                                if (x.FeatureAction.CustomActionId == "Save")
                                {
                                    fsucCmdBarCategory.SaveButton.Enabled = false;
                                }
                                if (x.FeatureAction.CustomActionId == "Cancel")
                                {
                                    fsucCmdBarCategory.CancelButton.Enabled = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {

                                if (x.FeatureAction.CustomActionId == "Save")
                                {
                                    fsucCmdBarCategory.SaveButton.Visible = false;
                                }
                                if (x.FeatureAction.CustomActionId == "Cancel")
                                {
                                    fsucCmdBarCategory.CancelButton.Visible = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }
        #endregion

        #region 4004

        protected void grdExternalVendorServices_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "LinkProfile")
                {
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    Int32 PSLI_ID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("PSLI_ID"));
                    Boolean IsLinkProfile = true;
                    //if (!PSLI_ID.IsNullOrEmpty() && PSLI_ID > AppConsts.NONE)
                    //{
                    //    ucSvcLineItemMapping.PackageServiceLineItemID = PSLI_ID;
                    //    ucSvcLineItemMapping.IsLinkProfile = true;
                    //    ucSvcLineItemMapping.TenantID = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
                    //    ucSvcLineItemMapping.OrderID = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
                    //}
                    if (!PSLI_ID.IsNullOrEmpty() && PSLI_ID > AppConsts.NONE)
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenProfileMappingPopup('" + Convert.ToString(PSLI_ID) + "','" + IsLinkProfile + "');", true);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void btnAddSvcLineItmMapping_Click(object sender, EventArgs e)
        {
            try
            {
                // dvDefault.Visible = false;
                //  dvSvcLineItemMapping.Visible = true;

                //ucSvcLineItemMapping.IsLinkProfile = false;
                // ucSvcLineItemMapping.TenantID = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
                // ucSvcLineItemMapping.OrderID = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
                Int32 pkgSivcLineItemID = AppConsts.NONE;
                Boolean IsLinkProfile = false;
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenProfileMappingPopup('" + pkgSivcLineItemID + "','" + IsLinkProfile + "');", true);
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

        protected void btnDoPostback_Click(object sender, EventArgs e)
        {
            try
            {
                if (!hdnIsSavedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnIsSavedSuccessfully.Value.ToLower()))
                    {
                        //base.ShowSuccessMessage("Service line item is saved successfully.");
                        lblSuccess.ShowMessage("Service line item is saved successfully.", MessageType.SuccessMessage);
                        lblSuccess.Visible = true;
                        grdExternalVendorServices.Rebind();
                    }
                    else
                    {
                        lblSuccess.ShowMessage("Some error has occurred. Please try again.", MessageType.Error);
                        lblSuccess.Visible = true;
                        //base.ShowInfoMessage();
                    }
                }
                if (!hdnIsUpdatesSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnIsUpdatesSuccessfully.Value.ToLower()))
                    {
                        //base.ShowSuccessMessage("Service line item is updated successfully.");
                        lblSuccess.ShowMessage("Service line item is updated successfully.", MessageType.SuccessMessage);
                        lblSuccess.Visible = true;
                        grdExternalVendorServices.Rebind();
                    }
                    else
                    {
                        lblSuccess.ShowMessage("Some error has occurred. Please try again.", MessageType.Error);
                        lblSuccess.Visible = true;
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

    }
}
