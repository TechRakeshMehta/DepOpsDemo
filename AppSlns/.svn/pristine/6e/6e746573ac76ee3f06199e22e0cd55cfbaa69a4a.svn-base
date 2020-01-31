using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.SharedDataEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageUniversalAttribute : BaseWebPage, IManageUniversalAttributeView
    {
        #region Variables
        private ManageUniversalAttributePresenter _presenter = new ManageUniversalAttributePresenter();
        #endregion

        #region Portperties

        public ManageUniversalAttributePresenter Presenter
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

        public IManageUniversalAttributeView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IManageUniversalAttributeView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IManageUniversalAttributeView.UniversalItemID
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

        Boolean IManageUniversalAttributeView.IsAddMode
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

        Int32 IManageUniversalAttributeView.UniversalAttributeID
        {
            get
            {
                return Convert.ToInt32(ViewState["UniversalAttributeID"]);
            }
            set
            {
                ViewState["UniversalAttributeID"] = value;
            }
        }

        String IManageUniversalAttributeView.AttributeName
        {
            get
            {
                return txtAttributeName.Text;
            }
            set
            {
                txtAttributeName.Text = value;
            }
        }

        String IManageUniversalAttributeView.AttributeDataTypeID
        {
            get
            {
                return ddlDataType.SelectedValue;
            }
            set
            {
                ddlDataType.SelectedValue = value;
            }
        }

        String IManageUniversalAttributeView.OptionDataTypeValue
        {
            get
            {
                return txtOption.Text;
            }
            set
            {
                txtOption.Text = value;
            }
        }

        List<UniversalAttribute> IManageUniversalAttributeView.lstUniversalAttribute { get; set; }

        List<lkpUniversalAttributeDataType> IManageUniversalAttributeView.lstAttributeDatatype 
        {
            get {
                return (ViewState["lstAttributeDatatype"] as List<lkpUniversalAttributeDataType>);
            }
            set {
                ViewState["lstAttributeDatatype"] = value.Where(cond => cond.LUADT_Code != UniversalAttributeDataTypeEnum.VIEW_DOCUMENT.GetStringValue()).ToList();
                ddlDataType.DataSource = value.Where(cond=>cond.LUADT_Code != UniversalAttributeDataTypeEnum.VIEW_DOCUMENT.GetStringValue()).ToList();
                ddlDataType.DataBind();
                ddlDataType.Items.Insert(0, new RadComboBoxItem("--SELECT--"));
            }
        }

        String IManageUniversalAttributeView.Messgae
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
                    Presenter.OnViewInitialized();
                    CaptureQueryStringData();
                    BindControls();
                    if (!CurrentViewContext.Messgae.IsNullOrEmpty())
                    {
                        base.ShowSuccessMessage(CurrentViewContext.Messgae);
                        Session["Message"] = null;
                    }
                }
                Presenter.OnViewLoaded();
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

        #region Attribute Grid
        protected void grdUniversalAttribute_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    CurrentViewContext.UniversalAttributeID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UA_ID"]);
                    if (Presenter.DeleteUniversalAttributeByID())
                    {
                        base.ShowSuccessMessage("Universal Attribute deleted successfully.");
                        RebindControls();
                        CurrentViewContext.UniversalAttributeID = AppConsts.NONE;
                        CurrentViewContext.Messgae = "Universal Attribute deleted successfully.";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Unable to delete Universal Attribute, please try again.");
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
            if (CurrentViewContext.UniversalItemID > AppConsts.NONE)
            {
                Presenter.GetUniversalAttributesDetails();
                grdUniversalAttribute.DataSource = CurrentViewContext.lstUniversalAttribute;
            }
        }
        #endregion

        #region Button Events
        protected void btnAddUniAtr_Click(object sender, EventArgs e)
        {
            try
            {
                dvAddAttribute.Style["display"] = "block";
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
                    dvAddAttribute.Style["display"] = "none";
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

        protected void fsucCmdBarSaveCategory_SaveClick(object sender, EventArgs e)
        {
            try
            {
                //if (Presenter.IsValidAttributeName())
                //{
                //    base.ShowInfoMessage(CurrentViewContext.AttributeName + " name already exists.");
                //    return;
                //}
                if (!Presenter.IsValidOptionFormat(txtOption.Text))
                {
                    base.ShowInfoMessage("Please enter valid options format i.e. Positive=1,Negative=2.");
                    return;
                }
                if (Presenter.SaveUpdateAttributeDetails())
                {
                    RebindControls();
                    if (CurrentViewContext.UniversalAttributeID > AppConsts.NONE)
                    {
                        base.ShowSuccessMessage("Universal Attribute Updated successfully.");
                        CurrentViewContext.Messgae = "Universal Attribute Updated successfully.";
                    }
                    else
                    {
                        base.ShowSuccessMessage("Universal Attribute Added successfully.");
                        CurrentViewContext.Messgae = "Universal Attribute Added successfully.";
                    }
                    dvAddAttribute.Style["display"] = "none";
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
                    base.ShowErrorInfoMessage("Unable to add Universal Attribute, please try again.");
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
                dvAddAttribute.Style["display"] = "block";
                txtAttributeName.Enabled = true;
                ddlDataType.Enabled = false;
                if (ddlDataType.SelectedValue == UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue())
                {
                    dvOptionType.Style["display"] = "block";
                    txtOption.Enabled = true;
                    rfvOption.Enabled = true;
                }
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

        #region Drop Down Event
        protected void ddlDataType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (ddlDataType.SelectedValue == UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue())
                {
                    dvOptionType.Style["display"] = "block";
                    rfvOption.Enabled = true;
                }
                else
                {
                    dvOptionType.Style["display"] = "none";
                    rfvOption.Enabled = false;
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
            if (!Request.QueryString["UniversalAttributeID"].IsNullOrEmpty())
            {
                CurrentViewContext.UniversalAttributeID = Convert.ToInt32(Request.QueryString["UniversalAttributeID"]);
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
                dvAddNewAttribute.Style["display"] = "block";
                dvSaveCancelBtn.Style["display"] = "block";
                dvUniAtrDetails.Style["display"] = "block";
                dvEditBtn.Style["display"] = "none";
                txtAttributeName.Enabled = true;
                ddlDataType.Enabled = true;
                txtOption.Enabled = true;
                lblHeader.Text = "Add Universal Attribute";
            }
            else
            {
                Presenter.GetUniversalAttributeData();
                dvAddNewAttribute.Style["display"] = "none";
                dvSaveCancelBtn.Style["display"] = "none";
                dvUniAtrDetails.Style["display"] = "none";
                dvEditBtn.Style["display"] = "block";
                dvAddAttribute.Style["display"] = "block";
                txtAttributeName.Enabled = false;
                if (ddlDataType.SelectedValue == UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue())
                {
                    dvOptionType.Style["display"] = "block";
                }
                else
                {
                    dvOptionType.Style["display"] = "none";
                }
                txtOption.Enabled = false;
                ddlDataType.Enabled = false;
                lblHeader.Text = "Universal Attribute Information";
            }
        }

        private void RebindControls()
        {
            CurrentViewContext.AttributeName = string.Empty;
            CurrentViewContext.AttributeDataTypeID = string.Empty;
            CurrentViewContext.OptionDataTypeValue = string.Empty;
            grdUniversalAttribute.Rebind();
        }
        #endregion
    }
}