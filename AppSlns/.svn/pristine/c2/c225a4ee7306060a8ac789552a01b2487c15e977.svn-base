using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Entity.SharedDataEntity;
using CoreWeb.Shell;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageUniversalCategory : BaseWebPage, IManageUniversalCategoryView
    {
        #region Variables

        private ManageUniversalCategoryPresenter _presenter = new ManageUniversalCategoryPresenter();

        #endregion

        #region Properties

        public ManageUniversalCategoryPresenter Presenter
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

        public IManageUniversalCategoryView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IManageUniversalCategoryView.UniversalCategoryID
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

        Boolean IManageUniversalCategoryView.IsAddMode
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

        String IManageUniversalCategoryView.CategoryName
        {
            get
            {
                return txtCategoryName.Text;
            }
            set
            {
                txtCategoryName.Text = value;
            }
        }

        //String IManageUniversalCategoryView.CategoryLabel
        //{
        //    get
        //    {
        //        return txtCategoryLabel.Text;
        //    }
        //    set
        //    {
        //        txtCategoryLabel.Text = value;
        //    }
        //}

        //String IManageUniversalCategoryView.Description
        //{
        //    get
        //    {
        //        return rdEditorDescription.Content;
        //    }
        //    set
        //    {
        //        rdEditorDescription.Content = value;
        //    }
        //}

        List<UniversalCategory> IManageUniversalCategoryView.lstUniversalCategory { get; set; }

        Int32 IManageUniversalCategoryView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }


        List<UniversalItem> IManageUniversalCategoryView.lstUniversalItems { get; set; }

        String IManageUniversalCategoryView.Messgae
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

        #region Page Event
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

        #region Category Grid Event
        protected void grdUniversalCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetUniversalCategoryDetails();
                grdUniversalCategory.DataSource = CurrentViewContext.lstUniversalCategory;
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
        protected void grdUniversalCategory_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    CurrentViewContext.UniversalCategoryID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UC_ID"]);
                    if (!Presenter.IsValidToDeleteCategory())
                    {
                        base.ShowInfoMessage("You can not delete Universal Category because it is in use.");
                        return;
                    }
                    if (Presenter.DeleteUniversalCategorydata())
                    {
                        base.ShowSuccessMessage("Universal Category Deleted successfully");
                        RebindControls();
                        CurrentViewContext.UniversalCategoryID = AppConsts.NONE;
                        CurrentViewContext.Messgae = "Universal Category Deleted successfully";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Unable to Delete Universal Category, please try again");
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

        #region Item Grid Evevnt

        protected void grdUniItems_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    Int32 currentItemID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UI_ID"]);

                    if (!Presenter.IsValidToDeleteItem(currentItemID))
                    {
                        base.ShowInfoMessage("You can not delete Universal Item because it is in use.");
                        return;
                    }
                    if (Presenter.DeleteUniversalItemByID(currentItemID))
                    {
                        base.ShowSuccessMessage("Universal Item Deleted successfully");
                        RebindControls();
                        CurrentViewContext.Messgae = "Universal Item Deleted successfully";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Unable to Item Universal Category, please try again");
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

        protected void grdUniItems_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.UniversalCategoryID > AppConsts.NONE && !CurrentViewContext.IsAddMode)
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

        #region Button Events
        protected void fsucCmdBarSaveCategory_SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.IsValidCategoryName())
                {
                    base.ShowInfoMessage(CurrentViewContext.CategoryName + " name already exists.");
                    return;
                }
                if (Presenter.SaveUpdateUniversalCategory())
                {
                    RebindControls();
                    if (CurrentViewContext.UniversalCategoryID > AppConsts.NONE)
                    {
                        base.ShowSuccessMessage("Universal Category Updated successfully.");
                        CurrentViewContext.Messgae = "Universal Category Updated successfully.";
                    }
                    else
                    {
                        base.ShowSuccessMessage("Universal Category Added successfully.");
                        CurrentViewContext.Messgae = "Universal Category Added successfully.";
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
                    base.ShowErrorInfoMessage("Unable to add Universal Category, please try again.");
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

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            try
            {
                dvAddCategory.Style["display"] = "block";
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
                    dvAddCategory.Style["display"] = "none";
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
                dvAddCategory.Style["display"] = "block";
                txtCategoryName.Enabled = true;
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
        }

        private void BindControls()
        {
            if (CurrentViewContext.IsAddMode)
            {
                dvUniCatDetails.Style["display"] = "block";
                dvSaveCancelBtn.Style["display"] = "block";
                dvAddNewCategory.Style["display"] = "block";
                dvEditBtn.Style["display"] = "none";
                dvUniItemDetails.Style["display"] = "none";
                txtCategoryName.Enabled = true;
                lblHeader.Text = "Add Universal Category";
                //txtCategoryLabel.Enabled = true;
                //rdEditorDescription.Enabled = true;
            }
            else
            {
                Presenter.GetUniversalCategoryDataByID();
                dvUniCatDetails.Style["display"] = "none";
                dvSaveCancelBtn.Style["display"] = "none";
                dvAddNewCategory.Style["display"] = "none";
                dvEditBtn.Style["display"] = "block";
                dvAddCategory.Style["display"] = "block";
                dvUniItemDetails.Style["display"] = "block";
                txtCategoryName.Enabled = false;
                lblHeader.Text = "Universal Category Information";
                //txtCategoryLabel.Enabled = false;
                //rdEditorDescription.Enabled = false;
            }
        }

        private void RebindControls()
        {
            CurrentViewContext.CategoryName = string.Empty;
            //CurrentViewContext.CategoryLabel = string.Empty;
            //CurrentViewContext.Description = string.Empty;
            grdUniversalCategory.Rebind();
            grdUniItems.Rebind();
        }

        #endregion
    }
}