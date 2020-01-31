#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.Services;
using System.Linq;
using System.Web.UI.WebControls;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;
using CoreWeb.Shell.Views;
using System.Text.RegularExpressions;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageCustomAttributes : BaseUserControl, IManageCustomAttributesView
    {
        #region Variables

        #region Private Variables

        private ManageCustomAttributesPresenter _presenter = new ManageCustomAttributesPresenter();
        private CustomAttributeContract currentViewContract = null;

        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Presenter


        public ManageCustomAttributesPresenter Presenter
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

        #endregion

        #region Private Properties


        #endregion

        #region public Properties

        public List<Tenant> lstTenant
        {
            get;
            set;
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (ViewState["ClientTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ClientTenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ClientTenantID"] = value;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IQueryable<lkpCustomAttributeDataType> lstCustomAttDataType
        {
            get;
            set;
        }

        public IList<lkpCustomAttributeDataType> lstCustomAttDataTypeList
        {
            get;
            set;
        }

        public IQueryable<lkpCustomAttributeUseType> lstCustomAttUseType
        {
            get;
            set;
        }

        public IList<lkpCustomAttributeUseType> lstCustomAttUseTypeList
        {
            get;
            set;
        }

        public IQueryable<CustomAttribute> lstCustomAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IManageCustomAttributesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        private CustomAttributeContract CurrentViewContract
        {
            get
            {
                if (currentViewContract == null)
                    currentViewContract = new CustomAttributeContract();
                return currentViewContract;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public String SuccessMessage
        {
            get;
            set;
        }

        public String InfoMessage
        {
            get;
            set;
        }

        public List<CustomAttribute> lstProfileCustomAttributes
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Custom Attributes";
                base.SetPageTitle("Custom Attributes");
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
                if (Presenter.IsAdminLoggedIn())
                {
                    Presenter.OnViewLoaded();
                    ddlTenant.DataSource = lstTenant;
                    ddlTenant.DataBind();
                }
            }
            if (Presenter.IsAdminLoggedIn())
            {
                if (ddlTenant.SelectedValue == String.Empty || Convert.ToInt32(ddlTenant.SelectedValue) == AppConsts.NONE)
                {
                    dvNode.Visible = false;
                }
            }
            else
            {
                pnlTenant.Visible = false;
            }
        }

        #endregion

        #region DropDown Events

        /// <summary>
        /// Binds the subscription options as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    dvNode.Visible = true;
                    grdCustomAttributes.Rebind();
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

        protected void cmbDataType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbItemType = sender as WclComboBox;

            System.Web.UI.WebControls.Panel panel = (System.Web.UI.WebControls.Panel)cmbItemType.Parent;

            //UAT 1438
            WclComboBox cmbUseType = panel.FindControl("cmbUseType") as WclComboBox;
            HandleUseTypeComboBox(cmbItemType, cmbUseType);

            if (panel != null)
            {
                ShowHideContentArea(panel, true);

                Int32 selectedUseTypeId = Convert.ToInt32(cmbUseType.SelectedValue);
                String selectedUseTypeCode = CurrentViewContext.lstCustomAttUseType.FirstOrDefault(cond => cond.CustomAttributeUseTypeID == selectedUseTypeId).Code;

                if (selectedUseTypeCode == CustomAttributeUseTypeContext.Hierarchy.GetStringValue())
                {
                    WclComboBox cmbRelatedAttribute = panel.FindControl("cmbRelatedAttribute") as WclComboBox;
                    Presenter.GetProfileCustomAttributesList(Convert.ToInt32(cmbItemType.SelectedValue));
                    cmbRelatedAttribute.DataSource = lstProfileCustomAttributes;
                    cmbRelatedAttribute.DataBind();
                    cmbRelatedAttribute.Items.Insert(0, new RadComboBoxItem("--Select--"));
                }
            }
            dvNode.Visible = true;
        }



        protected void cmbUseType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbItemType = sender as WclComboBox;

            System.Web.UI.WebControls.Panel panel = (System.Web.UI.WebControls.Panel)cmbItemType.Parent;

            WclComboBox cmbDataType = panel.FindControl("cmbDataType") as WclComboBox;
            HandleDataTypeComboBox(cmbItemType, cmbDataType);

            if (panel != null)
            {
                ShowHideContentArea(panel, true);

                Int32 selectedUseTypeId = Convert.ToInt32(cmbItemType.SelectedValue);
                String selectedUseTypeCode = CurrentViewContext.lstCustomAttUseType.FirstOrDefault(cond => cond.CustomAttributeUseTypeID == selectedUseTypeId).Code;

                if (selectedUseTypeCode == CustomAttributeUseTypeContext.Hierarchy.GetStringValue())
                {
                    WclComboBox cmbRelatedAttribute = panel.FindControl("cmbRelatedAttribute") as WclComboBox;
                    Presenter.GetProfileCustomAttributesList(Convert.ToInt32(cmbDataType.SelectedValue));
                    cmbRelatedAttribute.DataSource = lstProfileCustomAttributes;
                    cmbRelatedAttribute.DataBind();
                    cmbRelatedAttribute.Items.Insert(0, new RadComboBoxItem("--Select--"));
                }
            }
        }



        #endregion

        #region Grid Events

        protected void grdCustomAttributes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetCustomAttributesList();
                grdCustomAttributes.DataSource = CurrentViewContext.lstCustomAttributes;
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

        protected void grdCustomAttributes_ItemCreated(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {
                GridEditFormItem editform = (e.Item as GridEditFormItem);

                WclComboBox cmbDataType = editform.FindControl("cmbDataType") as WclComboBox;
                WclComboBox cmbUseType = editform.FindControl("cmbUseType") as WclComboBox;
                RadioButtonList rblIsRequired = editform.FindControl("rblIsRequired") as RadioButtonList;
                WclComboBox cmbRelatedAttribute = editform.FindControl("cmbRelatedAttribute") as WclComboBox;
                //UAT-3751
                IsActiveToggle chkShowPendingCmpProfile = editform.FindControl("chkShowPendingCmpProfile") as IsActiveToggle;


                Presenter.GetCustomAttDataTypelist();
                //UAT 1438
                CurrentViewContext.lstCustomAttDataTypeList = new List<lkpCustomAttributeDataType>(CurrentViewContext.lstCustomAttDataType);
                cmbDataType.DataSource = CurrentViewContext.lstCustomAttDataType;
                cmbDataType.DataBind();
                cmbDataType.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbDataType_SelectedIndexChanged);

                Presenter.GetCustomAttrUseTypeList();
                //UAT 1438
                CurrentViewContext.lstCustomAttUseTypeList = new List<lkpCustomAttributeUseType>(CurrentViewContext.lstCustomAttUseType);
                cmbUseType.DataSource = CurrentViewContext.lstCustomAttUseTypeList; //CurrentViewContext.lstCustomAttUseType;
                cmbUseType.DataBind();
                cmbUseType.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbUseType_SelectedIndexChanged);



                CustomAttribute attribute = e.Item.DataItem as CustomAttribute;
                if (attribute != null)
                {
                    cmbDataType.SelectedValue = attribute.CA_CustomAttributeDataTypeID.ToString();
                    cmbUseType.SelectedValue = attribute.CA_CustomAttributeUseTypeID.ToString();

                    System.Web.UI.WebControls.Panel panel = editform.FindControl("pnlAttr") as System.Web.UI.WebControls.Panel;
                    cmbUseType.Enabled = false;

                    HandleDataTypeComboBox(cmbUseType, cmbDataType);

                    ShowHideContentArea(panel);
                    if (attribute.CA_IsRequired.IsNotNull())
                    {
                        rblIsRequired.SelectedValue = attribute.CA_IsRequired.ToString();
                    }
                    chkShowPendingCmpProfile.Checked = attribute.CA_ShowInPendingComProfilesGrid;

                }

                //Presenter.GetProfileCustomAttributesList(Convert.ToInt32(cmbDataType.SelectedValue));
                //cmbRelatedAttribute.DataSource = lstProfileCustomAttributes;
                //cmbRelatedAttribute.DataBind();
                //cmbRelatedAttribute.Items.Insert(0, new RadComboBoxItem("--Select--"));
                //if (attribute != null)
                //{
                //    cmbRelatedAttribute.SelectedValue = attribute.CA_RelatedCustomAttributeId.HasValue ? attribute.CA_RelatedCustomAttributeId.ToString() : String.Empty;
                //}
            }
        }

        protected void grdCustomAttributes_ItemCommand(object sender, GridCommandEventArgs e)
        {
            dvNode.Visible = true;
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                   || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdCustomAttributes);
            }

            //#region Export functionality
            //// Implemented the export functionlaity for both admin and client admin and show and hide the columns accordingly
            //if (e.CommandName.IsNullOrEmpty())
            //{
            //    if (e.Item is GridCommandItem)
            //    {
            //        foreach (GridDataItem item in grdCustomAttributes.MasterTableView.Items)
            //        {
            //            item["IsActiveExport"].Text = (item.FindControl("IsActive") as Label).Text;
            //        }
            //    }
            //    WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
            //    if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
            //    {
            //        grdCustomAttributes.MasterTableView.GetColumn("IsActiveExport").Display = true;
            //        //grdCustomAttributes.MasterTableView.GetColumn("IsActive").Display = false;
            //        //grdCustomAttributes.NonExportingColumns = "IsActive";
            //    }
            //    else
            //    {
            //        grdCustomAttributes.MasterTableView.GetColumn("IsActiveExport").Display = false;
            //        //grdCustomAttributes.MasterTableView.GetColumn("IsActive").Display = true;
            //    }
            //}
            //if (e.CommandName == "Cancel")
            //{
            //    grdCustomAttributes.MasterTableView.GetColumn("IsActiveExport").Display = false;
            //    grdCustomAttributes.MasterTableView.GetColumn("IsActive").Display = true;
            //}
            //if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
            //    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            //{
            //    base.ConfigureExport(grdCustomAttributes);
            //}

            //#endregion
        }

        protected void grdCustomAttributes_InsertCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;

            CurrentViewContract.CA_CustomAttributeUseTypeID = Convert.ToInt32((e.Item.FindControl("cmbUseType") as WclComboBox).SelectedValue);
            CurrentViewContract.CA_CustomAttributeDataTypeID = Convert.ToInt32((e.Item.FindControl("cmbDataType") as WclComboBox).SelectedValue);
            CurrentViewContract.CA_AttributeName = (e.Item.FindControl("txtAttrName") as WclTextBox).Text.Trim();
            CurrentViewContract.CA_AttributeLabel = (e.Item.FindControl("txtAttrLabel") as WclTextBox).Text.Trim();
            CurrentViewContract.CA_Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
            if (string.IsNullOrEmpty((e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox).Text))
                CurrentViewContract.CA_StringLength = null;
            else
                CurrentViewContract.CA_StringLength = Convert.ToInt32((e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox).Text);
            CurrentViewContract.CA_IsActive = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);
            CurrentViewContract.CA_IsDeleted = false;
            CurrentViewContract.CA_CreatedByID = CurrentUserId;
            CurrentViewContract.CA_CreatedOn = DateTime.Now;
            CurrentViewContract.CA_ModifiedByID = null;
            CurrentViewContract.CA_ModifiedOn = null;
            //UAT- 1068 Ability to configure specific custom field formats
            if (string.IsNullOrEmpty((e.Item.FindControl("txtRegularExp") as WclTextBox).Text))
                CurrentViewContract.CA_RegularExpression = null;
            else
                CurrentViewContract.CA_RegularExpression = (e.Item.FindControl("txtRegularExp") as WclTextBox).Text.Trim();

            if (string.IsNullOrEmpty((e.Item.FindControl("txtRegExpErrorMsg") as WclTextBox).Text))
                CurrentViewContract.CA_RegExpErrorMsg = null;
            else
                CurrentViewContract.CA_RegExpErrorMsg = (e.Item.FindControl("txtRegExpErrorMsg") as WclTextBox).Text.Trim();
            Int32 selectedUseTypeId = Convert.ToInt32((e.Item.FindControl("cmbUseType") as WclComboBox).SelectedValue);
            String selectedUseTypeCode = CurrentViewContext.lstCustomAttUseType.FirstOrDefault(cond => cond.CustomAttributeUseTypeID == selectedUseTypeId).Code;
            if (selectedUseTypeCode == CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue()
                || selectedUseTypeCode == CustomAttributeUseTypeContext.Profile.GetStringValue())
            {
                CurrentViewContract.CA_IsRequired = Convert.ToBoolean((e.Item.FindControl("rblIsRequired") as RadioButtonList).SelectedValue);
            }
            if (selectedUseTypeCode == CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue())
            {
                //UAT-3751
                CurrentViewContract.CA_ShowInPendingComProfilesGrid = (e.Item.FindControl("chkShowPendingCmpProfile") as IsActiveToggle).Checked;
                //UAT-3964
                CurrentViewContract.CA_IncludeInNotification = (e.Item.FindControl("chkIncludeInNotification") as IsActiveToggle).Checked;
            }


            if (selectedUseTypeCode == CustomAttributeUseTypeContext.Hierarchy.GetStringValue())
            {
                String relatedCustomAttribute = (e.Item.FindControl("cmbRelatedAttribute") as WclComboBox).SelectedValue;
                CurrentViewContract.CA_RelatedCustomAttributeID = relatedCustomAttribute.IsNullOrEmpty() ? (int?)null : Convert.ToInt32(relatedCustomAttribute);
                CurrentViewContract.CA_DisplayInSearchFilter = Convert.ToBoolean((e.Item.FindControl("chkDisplayInSearchFilter") as IsActiveToggle).Checked);

            }

            Presenter.AddCustomAttribute(CurrentViewContract);
            if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
            {
                base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
            }
            if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
            {
                e.Canceled = true;
                base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
            }
            if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
            {
                e.Canceled = true;
                base.ShowInfoMessage(CurrentViewContext.InfoMessage);
            }
        }

        protected void grdCustomAttributes_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            CurrentViewContract.CA_CustomAttributeID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("CA_CustomAttributeID"));
            CurrentViewContract.CA_CustomAttributeUseTypeID = Convert.ToInt32((e.Item.FindControl("cmbUseType") as WclComboBox).SelectedValue);
            CurrentViewContract.CA_CustomAttributeDataTypeID = Convert.ToInt32((e.Item.FindControl("cmbDataType") as WclComboBox).SelectedValue);
            CurrentViewContract.CA_AttributeName = (e.Item.FindControl("txtAttrName") as WclTextBox).Text.Trim();
            CurrentViewContract.CA_AttributeLabel = (e.Item.FindControl("txtAttrLabel") as WclTextBox).Text.Trim();
            CurrentViewContract.CA_Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
            if (string.IsNullOrEmpty((e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox).Text))
                CurrentViewContract.CA_StringLength = null;
            else
                CurrentViewContract.CA_StringLength = Convert.ToInt32((e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox).Text);
            CurrentViewContract.CA_IsActive = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);
            CurrentViewContract.CA_ModifiedByID = CurrentUserId;
            CurrentViewContract.CA_ModifiedOn = DateTime.Now;

            //UAT- 1068 Ability to configure specific custom field formats
            if (string.IsNullOrEmpty((e.Item.FindControl("txtRegularExp") as WclTextBox).Text))
                CurrentViewContract.CA_RegularExpression = null;
            else
                CurrentViewContract.CA_RegularExpression = (e.Item.FindControl("txtRegularExp") as WclTextBox).Text.Trim();

            if (string.IsNullOrEmpty((e.Item.FindControl("txtRegExpErrorMsg") as WclTextBox).Text))
                CurrentViewContract.CA_RegExpErrorMsg = null;
            else
                CurrentViewContract.CA_RegExpErrorMsg = (e.Item.FindControl("txtRegExpErrorMsg") as WclTextBox).Text.Trim();

            Int32 selectedUseTypeId = Convert.ToInt32((e.Item.FindControl("cmbUseType") as WclComboBox).SelectedValue);
            String selectedUseTypeCode = CurrentViewContext.lstCustomAttUseType.FirstOrDefault(cond => cond.CustomAttributeUseTypeID == selectedUseTypeId).Code;
            if (selectedUseTypeCode == CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue()
                || selectedUseTypeCode == CustomAttributeUseTypeContext.Profile.GetStringValue())
                CurrentViewContract.CA_IsRequired = Convert.ToBoolean((e.Item.FindControl("rblIsRequired") as RadioButtonList).SelectedValue);

             if (selectedUseTypeCode == CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue())
            {
                //UAT-3751
                CurrentViewContract.CA_ShowInPendingComProfilesGrid = (e.Item.FindControl("chkShowPendingCmpProfile") as IsActiveToggle).Checked;
                CurrentViewContract.CA_IncludeInNotification = (e.Item.FindControl("chkIncludeInNotification") as IsActiveToggle).Checked;
            }

            if (selectedUseTypeCode == CustomAttributeUseTypeContext.Hierarchy.GetStringValue())
            {
                String relatedCustomAttribute = (e.Item.FindControl("cmbRelatedAttribute") as WclComboBox).SelectedValue;
                CurrentViewContract.CA_RelatedCustomAttributeID = relatedCustomAttribute.IsNullOrEmpty() ? (int?)null : Convert.ToInt32(relatedCustomAttribute);
                CurrentViewContract.CA_DisplayInSearchFilter = Convert.ToBoolean((e.Item.FindControl("chkDisplayInSearchFilter") as IsActiveToggle).Checked);
            }
            Presenter.UpdateCustomAttribute(CurrentViewContract);
            if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
            {
                base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
            }
            if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
            {
                e.Canceled = true;
                base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
            }
            if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
            {
                e.Canceled = true;
                base.ShowInfoMessage(CurrentViewContext.InfoMessage);
            }
        }

        protected void grdCustomAttributes_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            Int32 customAttributeID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("CA_CustomAttributeID"));
            Presenter.DeleteCustomAttribute(customAttributeID);
            if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
            {
                base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
            }
            if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
            {
                e.Canceled = true;
                base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
            }
            if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
            {
                e.Canceled = true;
                base.ShowInfoMessage(CurrentViewContext.InfoMessage);
            }
        }

        /// <summary>
        /// Grid ItemDataBound Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCustomAttributes_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = (GridDataItem)e.Item;
                        dataItem["IsActive"].Text = Convert.ToBoolean(dataItem["IsActive"].Text) == true ? Convert.ToString("Yes") : Convert.ToString("No");
                    }
                }
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox cmbDataType = editform.FindControl("cmbDataType") as WclComboBox;
                    WclComboBox cmbRelatedAttribute = editform.FindControl("cmbRelatedAttribute") as WclComboBox;
                    Presenter.GetProfileCustomAttributesList(Convert.ToInt32(cmbDataType.SelectedValue));
                    cmbRelatedAttribute.DataSource = lstProfileCustomAttributes;
                    cmbRelatedAttribute.DataBind();
                    cmbRelatedAttribute.Items.Insert(0, new RadComboBoxItem("--Select--"));

                    CustomAttribute attribute = e.Item.DataItem as CustomAttribute;
                    if (attribute != null)
                    {
                        cmbRelatedAttribute.SelectedValue = attribute.CA_RelatedCustomAttributeId.HasValue ? attribute.CA_RelatedCustomAttributeId.ToString() : String.Empty;
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

        #region Button Events
        protected void btnValidateRegExp_Click(object sender, EventArgs e)
        {
            WclButton btnvalidateRegExp = sender as WclButton;
            System.Web.UI.HtmlControls.HtmlGenericControl dvValidateRegExp = (System.Web.UI.HtmlControls.HtmlGenericControl)btnvalidateRegExp.Parent;

            System.Web.UI.WebControls.Panel panel = (System.Web.UI.WebControls.Panel)dvValidateRegExp.Parent;
            System.Web.UI.HtmlControls.HtmlGenericControl divCharacters = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("divCharacters");

            Label lblValidStatus = dvValidateRegExp.FindControl("lblValidStatus") as Label;
            WclTextBox txtRegularExp = divCharacters.FindControl("txtRegularExp") as WclTextBox;
            WclTextBox txtInputString = divCharacters.FindControl("txtValString") as WclTextBox;
            if (txtRegularExp.Text.IsNullOrEmpty())
            {
                lblValidStatus.Text = "Please enter Regular Expression to Validate.";
                lblValidStatus.ForeColor = System.Drawing.Color.Red;
            }
            else if (txtInputString.Text.IsNullOrEmpty())
            {
                lblValidStatus.Text = "Please enter Input Text to Validate.";
                lblValidStatus.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (Regex.IsMatch(txtInputString.Text, txtRegularExp.Text))
                {
                    lblValidStatus.Text = "Success";
                    lblValidStatus.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblValidStatus.Text = "Failed";
                    lblValidStatus.ForeColor = System.Drawing.Color.Red;
                }
            }

        }
        #endregion

        #endregion

        #region Methods

        private void ShowHideContentArea(System.Web.UI.WebControls.Panel panel, bool clearData = false)
        {
            HtmlGenericControl divCharacters = (HtmlGenericControl)panel.FindControl("divCharacters");
            HtmlGenericControl dvValidateRegExp = (HtmlGenericControl)panel.FindControl("dvValidateRegExp");
            HtmlGenericControl dvValidater = (HtmlGenericControl)panel.FindControl("dvValidater");
            HtmlGenericControl dvIsRequired = (HtmlGenericControl)panel.FindControl("dvIsRequired");
            WclComboBox cmbAttributeDatatype = (WclComboBox)panel.FindControl("cmbDataType");
            WclComboBox cmbAttributeUsetype = (WclComboBox)panel.FindControl("cmbUseType");
            String customAttributeDatatype = cmbAttributeDatatype.SelectedItem.Text;
            Int32 customAttributeTypeId = Convert.ToInt32(cmbAttributeUsetype.SelectedValue);
            String customAttributeUsetype = CurrentViewContext.lstCustomAttUseType.FirstOrDefault(cond => cond.CustomAttributeUseTypeID == customAttributeTypeId).Code;

            HtmlGenericControl dvRelatedAttribute = (HtmlGenericControl)panel.FindControl("dvRelatedAttribute");
            WclComboBox cmbRelatedAttribute = (WclComboBox)panel.FindControl("cmbRelatedAttribute");
            HtmlGenericControl divIsDisplayInSearchFilter = (HtmlGenericControl)panel.FindControl("divIsDisplayInSearchFilter");

            //UAT-3751
            HtmlGenericControl dvPendingCmpProfile = (HtmlGenericControl)panel.FindControl("dvPendingCmpProfile");
            HtmlGenericControl dvMaxCharRegExpression = (HtmlGenericControl)panel.FindControl("dvMaxCharRegExpression");

            if (divCharacters != null)
            {
                if (clearData)
                {
                    WclNumericTextBox ntxtTextMaxChars = panel.FindControl("ntxtTextMaxChars") as WclNumericTextBox;
                    WclTextBox txtRegularExp = panel.FindControl("txtRegularExp") as WclTextBox;
                    WclTextBox txtRegExpErrorMsg = panel.FindControl("txtRegExpErrorMsg") as WclTextBox;
                    Label lblValidStatus = panel.FindControl("lblValidStatus") as Label;
                    WclTextBox txtInputString = panel.FindControl("txtValString") as WclTextBox;

                    ntxtTextMaxChars.Text = string.Empty;
                    txtRegularExp.Text = string.Empty;
                    txtRegExpErrorMsg.Text = string.Empty;
                    lblValidStatus.Text = string.Empty;
                    txtInputString.Text = string.Empty;
                }
                divCharacters.Visible = false;
                if (dvValidateRegExp.IsNotNull())
                {
                    dvValidateRegExp.Visible = false;
                    dvValidater.Visible = false;
                }
            }

            if (divCharacters != null && customAttributeDatatype.Equals("Text"))
            {
                divCharacters.Visible = true;
                if (dvValidateRegExp.IsNotNull())
                {
                    dvValidateRegExp.Visible = true;
                    dvValidater.Visible = true;
                }
            }

            if ((divCharacters != null && divCharacters.Visible) || dvValidater.Visible || dvValidateRegExp.Visible)
            {
                dvMaxCharRegExpression.Visible = true;
            }
            else
            {
                dvMaxCharRegExpression.Visible = false;
            }

            if (customAttributeUsetype == CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue())
            {
                dvIsRequired.Visible = true;
                dvValidateRegExp.Visible = false;
                dvValidater.Visible = false;
                dvRelatedAttribute.Visible = false;
                cmbRelatedAttribute.Items.Clear();
                divIsDisplayInSearchFilter.Visible = false;
                dvPendingCmpProfile.Visible = true;
            }
            else if (customAttributeUsetype == CustomAttributeUseTypeContext.Profile.GetStringValue())
            {
                dvIsRequired.Visible = true;
                dvRelatedAttribute.Visible = false;
                cmbRelatedAttribute.Items.Clear();
                divIsDisplayInSearchFilter.Visible = false;
                dvPendingCmpProfile.Visible = false;
            }
            else
            {
                dvIsRequired.Visible = false;
                dvRelatedAttribute.Visible = true;
                divIsDisplayInSearchFilter.Visible = true;
                dvPendingCmpProfile.Visible = false;
            }
        }

        private void HandleUseTypeComboBox(WclComboBox cmbDataType, WclComboBox cmbUseType)
        {
            Int32 userGroupDatatypeID = lstCustomAttDataType.Where(cond => cond.Code == CustomAttributeDatatype.User_Group.GetStringValue()).Select(col => col.CustomAttributeDataTypeID).FirstOrDefault();
            if (cmbDataType.SelectedValue == Convert.ToString(userGroupDatatypeID))
            {
                //Removes the Clinical Rotation from DropDown.
                CurrentViewContext.lstCustomAttUseTypeList.RemoveAll(x => x.Code == CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue());
            }
            else
            {
                if (!CurrentViewContext.lstCustomAttUseTypeList.Any(x => x.Code == CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue()))
                {
                    lkpCustomAttributeUseType attrUseTypeToAdd = CurrentViewContext.lstCustomAttUseType.Where(x => x.Code == CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue()).FirstOrDefault();
                    CurrentViewContext.lstCustomAttUseTypeList.Add(attrUseTypeToAdd);
                }
            }

            String prevSelectedValue = cmbUseType.SelectedValue;
            cmbUseType.DataSource = CurrentViewContext.lstCustomAttUseTypeList;
            cmbUseType.DataBind();
            cmbUseType.SelectedValue = prevSelectedValue;
        }

        private void HandleDataTypeComboBox(WclComboBox cmbUseType, WclComboBox cmbDataType)
        {
            Int32 userGroupUseTypeID = lstCustomAttUseType.Where(cond => cond.Code == CustomAttributeUseTypeContext.Clinical_Rotation.GetStringValue()).Select(col => col.CustomAttributeUseTypeID).FirstOrDefault();

            Int32 profileUseTypeID = lstCustomAttUseType.Where(cond => cond.Code == CustomAttributeUseTypeContext.Profile.GetStringValue()).Select(col => col.CustomAttributeUseTypeID).FirstOrDefault();
            if (cmbUseType.SelectedValue == Convert.ToString(userGroupUseTypeID)
                || cmbUseType.SelectedValue == Convert.ToString(profileUseTypeID))
            {
                //Removes the User Group from DropDown.
                CurrentViewContext.lstCustomAttDataTypeList.RemoveAll(x => x.Code == CustomAttributeDatatype.User_Group.GetStringValue());
            }
            else
            {
                if (!CurrentViewContext.lstCustomAttDataTypeList.Any(x => x.Code == CustomAttributeDatatype.User_Group.GetStringValue()))
                {
                    lkpCustomAttributeDataType attrDatatypeToAdd = CurrentViewContext.lstCustomAttDataType.Where(x => x.Code == CustomAttributeDatatype.User_Group.GetStringValue()).FirstOrDefault();
                    CurrentViewContext.lstCustomAttDataTypeList.Add(attrDatatypeToAdd);
                }
            }
            //Get previous selected value
            String prevSelectedValue = cmbDataType.SelectedValue;
            cmbDataType.DataSource = CurrentViewContext.lstCustomAttDataTypeList;
            cmbDataType.DataBind();
            cmbDataType.SelectedValue = prevSelectedValue;
        }

        #endregion

    }
}

