using CoreWeb.Shell;
using Entity.SharedDataEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageUniversalItem : BaseWebPage, IManageUniversalItemView
    {
        #region Variables
        private ManageUniversalItemPresenter _presenter = new ManageUniversalItemPresenter();
        #endregion

        #region Properties

        public ManageUniversalItemPresenter Presenter
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

        public IManageUniversalItemView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<UniversalItem> IManageUniversalItemView.lstUniversalItems { get; set; }

        List<UniversalAttribute> IManageUniversalItemView.lstUniversalAttribute { get; set; }

        String IManageUniversalItemView.ItemName
        {
            get
            {
                return txtItemName.Text;
            }
            set
            {
                txtItemName.Text = value;
            }
        }

        Int32 IManageUniversalItemView.UniversalItemID
        {
            get
            {
                return Convert.ToInt32(ViewState["UniversalItemID"]);
            }
            set
            {
                ViewState["UniversalItemID"] = value;
            }
        }

        Boolean IManageUniversalItemView.IsAddMode
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAddMode"]);
            }
            set
            {
                ViewState["IsAddMode"] = value;
            }
        }

        Int32 IManageUniversalItemView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IManageUniversalItemView.UniversalCategoryID
        {
            get
            {
                return Convert.ToInt32(ViewState["UniversalCategoryID"]);
            }
            set
            {
                ViewState["UniversalCategoryID"] = value;
            }
        }

        String IManageUniversalItemView.Messgae
        {
            get
            {
                return Convert.ToString(Session["Message"]);
            }
            set
            {
                Session["Message"] = value;
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
                    Presenter.OnViewLoaded();
                    CaptureQueryStringData();
                    BindControls();
                    if (!CurrentViewContext.Messgae.IsNullOrEmpty())
                    {
                        base.ShowSuccessMessage(CurrentViewContext.Messgae);
                        Session["Message"] = null;
                    }
                }
                Presenter.OnViewInitialized();
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

        #region Item Grid Events
        protected void grdUniItems_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    CurrentViewContext.UniversalItemID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UI_ID"]);
                    if (!Presenter.IsValidToDeleteItem())
                    {
                        base.ShowInfoMessage("You can not delete Universal Item because it is in use.");
                        return;
                    }
                    if (Presenter.DeleteUniversalItemByID())
                    {
                        base.ShowSuccessMessage("Universal Item Deleted successfully.");
                        RebindControls();
                        CurrentViewContext.UniversalItemID = AppConsts.NONE;
                        CurrentViewContext.Messgae = "Universal Item Deleted successfully.";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Unable to Item Universal Category, please try again.");
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

        protected void grdUniItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.UniversalCategoryID > AppConsts.NONE)
                {
                    Presenter.GetUniversalItemsByCatID();
                    grdUniItems.DataSource = CurrentViewContext.lstUniversalItems;
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

        #region Attribute Grid Events
        protected void grdUniversalAttribute_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    Int32 currentAttributeID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UA_ID"]);
                    if(Presenter.DeleteUniversalAttributeByID(currentAttributeID))
                    {
                        base.ShowSuccessMessage("Universal Attribute Deleted successfully.");
                        RebindControls();
                        CurrentViewContext.Messgae = "Universal Attribute Deleted successfully";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Unable to Attribute Universal Category, please try again");
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

        protected void grdUniversalAttribute_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.UniversalCategoryID > AppConsts.NONE && CurrentViewContext.UniversalItemID > AppConsts.NONE)
                {
                    Presenter.GetUniversalAttributesDetails();
                    grdUniversalAttribute.DataSource = CurrentViewContext.lstUniversalAttribute;
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

        #region Button Events
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                dvAddItem.Style["display"] = "block";
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

        protected void fsucCmdBarSaveCategory_SaveClick(object sender, EventArgs e)
        {
            try
            {
                //if (Presenter.IsValidItemName())
                //{
                //    base.ShowInfoMessage(CurrentViewContext.ItemName + " name already exists.");
                //    return;
                //}
                if (Presenter.SaveUpdateUniversalItem())
                {
                    RebindControls();
                    if (CurrentViewContext.UniversalItemID > AppConsts.NONE)
                    {
                        base.ShowSuccessMessage("Universal Item Updated successfully.");
                        CurrentViewContext.Messgae = "Universal Item Updated successfully.";
                    }
                    else
                    {
                        base.ShowSuccessMessage("Universal Item Added successfully.");
                        CurrentViewContext.Messgae = "Universal Item Added successfully.";
                    }
                    dvAddItem.Style["display"] = "none";
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
                    base.ShowErrorInfoMessage("Universal Item not Added.");
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

        protected void fsucCmdBarSaveCategory_CancelClick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.IsAddMode)
                {
                    dvAddItem.Style["display"] = "none";
                    RebindControls();
                }
                else
                {
                    BindControls();
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                dvAddItem.Style["display"] = "block";
                txtItemName.Enabled = true;
                dvSaveCancelBtn.Style["display"] = "block";
                dvEditBtn.Style["display"] = "none";
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

        /// <summary>
        /// 
        /// </summary>
        private void CaptureQueryStringData()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();

            if (!Request.QueryString["IsAddMode"].IsNullOrEmpty())
            {
                CurrentViewContext.IsAddMode = Convert.ToBoolean(Request.QueryString["IsAddMode"]);
            }
            if (!Request.QueryString["UniversalCategoryID"].IsNullOrEmpty())
            {
                CurrentViewContext.UniversalCategoryID = Convert.ToInt32(Request.QueryString["UniversalCategoryID"]);
            }
            if (!Request.QueryString["UniversalItemID"].IsNullOrEmpty())
            {
                CurrentViewContext.UniversalItemID = Convert.ToInt32(Request.QueryString["UniversalItemID"]);
            }
        }

        private void BindControls()
        {
            if (CurrentViewContext.IsAddMode)
            {
                dvAddNewItem.Style["display"] = "block";
                dvSaveCancelBtn.Style["display"] = "block";
                dvUniAtrDetails.Style["display"] = "none";
                dvUniItemDetails.Style["display"] = "block";
                dvEditBtn.Style["display"] = "none";
                txtItemName.Enabled = true;
                lblHeader.Text = "Add Universal Item";
            }
            else
            {
                Presenter.GetUniversalItemDetailsByID();
                dvAddNewItem.Style["display"] = "none";
                dvSaveCancelBtn.Style["display"] = "none";
                dvUniAtrDetails.Style["display"] = "block";
                dvUniItemDetails.Style["display"] = "none";
                dvAddItem.Style["display"] = "block";
                dvEditBtn.Style["display"] = "block";
                txtItemName.Enabled = false;
                lblHeader.Text = "Universal Item Information";
            }
        }

        private void RebindControls()
        {
            CurrentViewContext.ItemName = string.Empty;
            grdUniItems.Rebind();
            grdUniversalAttribute.Rebind();
        }
        #endregion
    }
}