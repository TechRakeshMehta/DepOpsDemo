using System;
using Microsoft.Practices.ObjectBuilder;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using System.Collections.Generic;
using INTSOF.Utils;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using CoreWeb.Shell.Views;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class SetupComplianceAttributes : BaseUserControl, ISetupComplianceAttributesView
    {
        #region Variables

        #region Private Variables

        private SetupComplianceAttributesPresenter _presenter = new SetupComplianceAttributesPresenter();
        private String _viewType;
        private Int32 _tenantid;
        private ComplianceAttributeContract currentViewContract = null;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public SetupComplianceAttributesPresenter Presenter
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

        private ComplianceAttributeContract CurrentViewContract
        {
            get
            {
                if (currentViewContract == null)
                    currentViewContract = new ComplianceAttributeContract();
                return currentViewContract;
            }
        }

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
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

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(ddlTenant.SelectedValue, out _selectedTenantId);

                    if (_selectedTenantId == AppConsts.NONE)
                        _selectedTenantId = TenantId;
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
            }
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
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

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.Value;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        public Int32 currentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public Int32 AttributeDataTypeId
        {
            get { return Convert.ToInt32(ViewState["AttributeDataTypeId"]); }
            set { ViewState["AttributeDataTypeId"] = value; }
        }

        public ISetupComplianceAttributesView CurrentViewContext
        {
            get { return this; }
        }
        /*UAT - 3032*/

        Int32 ISetupComplianceAttributesView.PreferredSelectedTenantID
        {
            get
            {
                if (!ViewState["PreferredSelectedTenantID"].IsNull())
                {
                    return (Int32)ViewState["PreferredSelectedTenantID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PreferredSelectedTenantID"] = value;
            }
        }
        /* END UAT - 3032*/

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                Dictionary<String, String> args = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                }
                base.Title = "Master Compliance Attributes";
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
                BindTenant();
                /*UAT-3032*/
                if (IsAdminLoggedIn == true)
                    GetPreferredSelectedTenant();
                /*END UAT-3032*/
            }
            SetDefaultSelectedTenantId();
            base.SetPageTitle("Master Compliance Attributes");
        }

        #endregion

        #region Grid Events

        protected void grdAttributes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdAttributes.DataSource = Presenter.GetComplianceItemAttributes();
            grdAttributes.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
        }

        protected void grdAttributes_ItemCreated(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {
                GridEditFormItem editform = (e.Item as GridEditFormItem);

                WclComboBox cmbDataType = editform.FindControl("cmbDataType") as WclComboBox;
                WclComboBox cmbAttrType = editform.FindControl("cmbAttrType") as WclComboBox;
                WclComboBox cmbAttributeGroup = editform.FindControl("cmbAttributeGroup") as WclComboBox;
                WclComboBox cmbDocument = editform.FindControl("cmbDocument") as WclComboBox;

                //WclComboBox cmbAdditionalDocument = editform.FindControl("cmbAdditionalDocument") as WclComboBox;//Added in UAT-4558
                ComplianceAttribute attribute = e.Item.DataItem as ComplianceAttribute; //Added in UAT-4558

                cmbDataType.DataSource = Presenter.GetComplianceAttributeDatatype();
                cmbDataType.DataBind();

                cmbDataType.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbDataType_SelectedIndexChanged);

                cmbAttrType.DataSource = Presenter.GetComplianceAttributeType();
                cmbAttrType.DataBind();

                cmbAttributeGroup.DataSource = Presenter.GetComplianceAttributeGroup();
                cmbAttributeGroup.DataBind();

                cmbDocument.DataSource = Presenter.GetComplianceViewDocumentSysDocs();
                cmbDocument.DataBind();

                //Added IN UAT-4558
                //cmbAdditionalDocument.DataSource = Presenter.GetFileUploadAdditionalDocs();//(attribute.lkpComplianceAttributeDatatype);
                //cmbAdditionalDocument.DataBind();
                //END

                //ComplianceAttribute attribute = e.Item.DataItem as ComplianceAttribute; //Commented in UAT-4558
                if (attribute != null)
                {
                    IsActiveToggle ChkSendForIntegration = editform.FindControl("ChkSendForIntegration") as IsActiveToggle;
                    if (ChkSendForIntegration != null)
                    {
                        ChkSendForIntegration.Checked = Convert.ToBoolean(attribute.IsSendForintegration);
                    }
                    AttributeDataTypeId = attribute.ComplianceAttributeDatatypeID;
                    cmbDataType.SelectedValue = attribute.ComplianceAttributeDatatypeID.ToString();
                    cmbAttributeGroup.SelectedValue = Convert.ToString(attribute.ComplianceAttributeGroupID);
                    if (!attribute.ComplianceAttributeDocuments.IsNullOrEmpty())
                    {
                        if (attribute.ComplianceAttributeDocuments.Any(cond => !cond.CAD_IsDeleted))
                        {
                            cmbDocument.SelectedValue = Convert.ToString(attribute.ComplianceAttributeDocuments.FirstOrDefault(cond => !cond.CAD_IsDeleted).CAD_DocumentID);
                        }
                    }

                    //added in UAT-4558
                    //if (!attribute.ComplianceAttributeDocMappings.IsNullOrEmpty())
                    //{
                    //    if (attribute.ComplianceAttributeDocMappings.Any(con => !con.CADM_IsDeleted))
                    //    {
                    //        foreach (RadComboBoxItem item in cmbAdditionalDocument.Items)
                    //        {
                    //            if (!item.IsNullOrEmpty())
                    //            {
                    //                item.Checked = true;
                    //                Int32 docId = !String.IsNullOrEmpty(item.Value) ? Convert.ToInt32(item.Value) : AppConsts.NONE;
                    //                if (attribute.ComplianceAttributeDocMappings.Any(x => x.CADM_SystemDocumentID == docId))
                    //                    item.Checked = true;
                    //            }
                    //        }

                    //        //for (Int32 i = 0; i < cmbAdditionalDocument.Items.Count; i++)
                    //        //{
                    //        //    Int32 docID = !String.IsNullOrEmpty(cmbAdditionalDocument.Items[i].Value) ? Convert.ToInt32(cmbAdditionalDocument.Items[i].Value) : AppConsts.NONE;
                    //        //    if (attribute.ComplianceAttributeDocMappings.Any(x => x.CADM_SystemDocumentID == docID))
                    //        //        cmbAdditionalDocument.Items[i].Checked = true;
                    //        //}
                    //    }
                    //}
                    //END

                    System.Web.UI.WebControls.Panel panel = editform.FindControl("pnlAttr") as System.Web.UI.WebControls.Panel;

                    ShowHideContentArea(panel, attribute.lkpComplianceAttributeDatatype.Name, selectedAttributeTypeId: attribute.ComplianceAttributeDatatypeID);
                    cmbAttrType.SelectedValue = attribute.ComplianceAttributeTypeID.ToString();
                    (e.Item.FindControl("txtExplanatoryNotes") as WclTextBox).Text = CurrentViewContract.ExplanatoryNotes;
                }
            }
        }

        protected void grdAttributes_InsertCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;

            CurrentViewContract.ComplianceAttributeTypeID = Convert.ToInt32((e.Item.FindControl("cmbAttrType") as WclComboBox).SelectedValue);
            CurrentViewContract.ComplianceAttributeDatatypeID = Convert.ToInt32((e.Item.FindControl("cmbDataType") as WclComboBox).SelectedValue);
            if ((e.Item.FindControl("cmbAttributeGroup") as WclComboBox).SelectedValue != "0")
            {
                CurrentViewContract.ComplianceAttributeGroupID = Convert.ToInt32((e.Item.FindControl("cmbAttributeGroup") as WclComboBox).SelectedValue);
            }
            if ((e.Item.FindControl("cmbDocument") as WclComboBox).SelectedValue != "0")
            {
                CurrentViewContract.ComplianceViewDocumentID = Convert.ToInt32((e.Item.FindControl("cmbDocument") as WclComboBox).SelectedValue);
            }
            CurrentViewContract.Name = (e.Item.FindControl("txtAttrName") as WclTextBox).Text.Trim();
            CurrentViewContract.AttributeLabel = (e.Item.FindControl("txtAttrLabel") as WclTextBox).Text.Trim();
            CurrentViewContract.ScreenLabel = (e.Item.FindControl("txtScreenLabel") as WclTextBox).Text.Trim();
            CurrentViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
            CurrentViewContract.IsCreatedByAdmin = IsAdminLoggedIn;

            if (string.IsNullOrEmpty((e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox).Text))
                CurrentViewContract.MaximumCharacters = null;
            else
                CurrentViewContract.MaximumCharacters = Convert.ToInt32((e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox).Text);

            CurrentViewContract.IsActive = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);

            CurrentViewContract.IsDeleted = false;

            CurrentViewContract.CreatedByID = CurrentUserId;
            CurrentViewContract.CreatedOn = DateTime.Now;
            CurrentViewContract.ModifiedByID = null;
            CurrentViewContract.ModifiedOn = null;
            currentViewContract.ExplanatoryNotes = (e.Item.FindControl("txtExplanatoryNotes") as WclTextBox).Text.Trim();
            String options = (e.Item.FindControl("txtOptOptions") as WclTextBox).Text.Trim();

            if (!IsValidOptionFormat(options))
            {
                base.ShowErrorMessage("Please enter valid options format i.e. Positive=1|Negative=2.");
                e.Canceled = true;
                return;
            }

            currentViewContract.lstComplianceAttributeOption = GetComplianceAttributeOption(options);
            currentViewContract.TenantID = SelectedTenantId;

            //UAT-2023:Reconciliation: Addition of attribute-level setting to enable/disable trigger for reconciliation queue.
            CurrentViewContract.IsTriggerReconciliation = Convert.ToBoolean((e.Item.FindControl("chkTriggerRecon") as IsActiveToggle).Checked);

            CurrentViewContract.IsSendForintegration = Convert.ToBoolean((e.Item.FindControl("ChkSendForIntegration") as IsActiveToggle).Checked);

            //Added IN UAT-4558
            CurrentViewContract.lstFileUploadAttrDocIds = new List<Int32>();
            WclComboBox cmbAdditionalDocument = e.Item.FindControl("cmbAdditionalDocument") as WclComboBox;
            if (!cmbAdditionalDocument.IsNullOrEmpty())
            {
                cmbAdditionalDocument.CheckedItems.ForEach(itm =>
                {
                    Int32 docId = Convert.ToInt32(itm.Value);
                    CurrentViewContract.lstFileUploadAttrDocIds.Add(docId);
                });
            }
            //END

            if (Presenter.AddComplianceAttribute(CurrentViewContract))
                base.ShowSuccessMessage("Compliance attribute saved successfully.");

        }

        protected void grdAttributes_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            if (DefaultTenantId != SelectedTenantId && AttributeDataTypeId != Convert.ToInt32((e.Item.FindControl("cmbDataType") as WclComboBox).SelectedValue)
                && Presenter.checkIfMappingIsDefinedForAttribute(Convert.ToInt32(gridEditableItem.GetDataKeyValue("ComplianceAttributeID"))))
            {
                base.ShowInfoMessage("Data type of attribute cannot be changed as mapping is already defined for it.");
                e.Canceled = true;
                return;
            }
            else
            {
                CurrentViewContract.ComplianceAttributeID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ComplianceAttributeID"));
                CurrentViewContract.ComplianceAttributeTypeID = Convert.ToInt32((e.Item.FindControl("cmbAttrType") as WclComboBox).SelectedValue);
                CurrentViewContract.ComplianceAttributeDatatypeID = Convert.ToInt32((e.Item.FindControl("cmbDataType") as WclComboBox).SelectedValue);
                if ((e.Item.FindControl("cmbAttributeGroup") as WclComboBox).SelectedValue != "0")
                {
                    CurrentViewContract.ComplianceAttributeGroupID = Convert.ToInt32((e.Item.FindControl("cmbAttributeGroup") as WclComboBox).SelectedValue);
                }
                if ((e.Item.FindControl("cmbDocument") as WclComboBox).SelectedValue != "0")
                {
                    CurrentViewContract.ComplianceViewDocumentID = Convert.ToInt32((e.Item.FindControl("cmbDocument") as WclComboBox).SelectedValue);
                }
                CurrentViewContract.Name = (e.Item.FindControl("txtAttrName") as WclTextBox).Text.Trim();
                // CurrentViewContract.ExplanatoryNotes = (e.Item.FindControl("txtExplanatoryNotes") as WclTextBox).Text.Trim();
                CurrentViewContract.AttributeLabel = (e.Item.FindControl("txtAttrLabel") as WclTextBox).Text.Trim();
                CurrentViewContract.ScreenLabel = (e.Item.FindControl("txtScreenLabel") as WclTextBox).Text.Trim();
                CurrentViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();

                if (string.IsNullOrEmpty((e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox).Text))
                    CurrentViewContract.MaximumCharacters = null;
                else
                    CurrentViewContract.MaximumCharacters = Convert.ToInt32((e.Item.FindControl("ntxtTextMaxChars") as WclNumericTextBox).Text);

                CurrentViewContract.IsActive = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);

                CurrentViewContract.IsDeleted = false;
                CurrentViewContract.ModifiedByID = CurrentUserId;
                CurrentViewContract.ModifiedOn = DateTime.Now;
                currentViewContract.ExplanatoryNotes = (e.Item.FindControl("txtExplanatoryNotes") as WclTextBox).Text.Trim();
                String options = (e.Item.FindControl("txtOptOptions") as WclTextBox).Text.Trim();
                if (!IsValidOptionFormat(options))
                {
                    base.ShowErrorMessage("Please enter valid options format i.e. Positive=1|Negative=2.");
                    e.Canceled = true;
                    return;
                }


                currentViewContract.lstComplianceAttributeOption = GetComplianceAttributeOption(options);
                currentViewContract.TenantID = SelectedTenantId;

                //UAT-2023:Reconciliation: Addition of attribute-level setting to enable/disable trigger for reconciliation queue.
                CurrentViewContract.IsTriggerReconciliation = Convert.ToBoolean((e.Item.FindControl("chkTriggerRecon") as IsActiveToggle).Checked);
                CurrentViewContract.IsSendForintegration = Convert.ToBoolean((e.Item.FindControl("ChkSendForIntegration") as IsActiveToggle).Checked);

                //Added IN UAT-4558
                CurrentViewContract.lstFileUploadAttrDocIds = new List<Int32>();
                WclComboBox cmbAdditionalDocument = e.Item.FindControl("cmbAdditionalDocument") as WclComboBox;
                if (!cmbAdditionalDocument.IsNullOrEmpty())
                {
                    cmbAdditionalDocument.CheckedItems.ForEach(itm =>
                    {
                        Int32 docId = Convert.ToInt32(itm.Value);
                        CurrentViewContract.lstFileUploadAttrDocIds.Add(docId);
                    });
                }
                //END

                if (Presenter.UpdateComplianceAttribute(CurrentViewContract))
                    base.ShowSuccessMessage("Compliance attribute updated successfully.");
            }
        }

        protected void grdAttributes_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            Int32 complianceItemAttributeID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ComplianceAttributeID"));
            if (Presenter.DeleteComplianceAttribute(complianceItemAttributeID, CurrentUserId))
                base.ShowSuccessMessage("Compliance attribute deleted successfully.");
            else
                base.ShowErrorInfoMessage(ErrorMessage);
        }

        protected void grdAttributes_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                CurrentViewContract.ComplianceAttributeID = Convert.ToInt32((e.Item as GridEditableItem).GetDataKeyValue("ComplianceAttributeID"));
                currentViewContract.ExplanatoryNotes = Presenter.GetLargeContent(CurrentViewContract.ComplianceAttributeID);
            }
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                   || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdAttributes);
            }
        }

        /// <summary>
        /// Called when data is bound in grid.
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdAttributes_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    //Gets the value of field "IsCreatedByAdmin" which is kept in a hidden field.
                    HiddenField hdnfIsCreatedByAdmin = e.Item.FindControl("hdnfIsCreatedByAdmin") as HiddenField;

                    //Sets "DeleteColumn" visiblity false when Attribute is created by admin.
                    if (IsAdminLoggedIn == false && hdnfIsCreatedByAdmin.Value == "True")
                    {
                        dataItem["DeleteColumn"].Controls[0].Visible = false;
                    }


                }

                //UAT-4558
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);

                    //UAT-4558
                    WclComboBox cmbAdditionalDocument = editform.FindControl("cmbAdditionalDocument") as WclComboBox;//Added in UAT-4558
                    if (!cmbAdditionalDocument.IsNullOrEmpty())
                    {
                        cmbAdditionalDocument.DataSource = Presenter.GetFileUploadAdditionalDocs();//(attribute.lkpComplianceAttributeDatatype);
                        cmbAdditionalDocument.DataBind();

                        ComplianceAttribute attribute = editform.DataItem as ComplianceAttribute;
                        if (!attribute.IsNullOrEmpty() && !attribute.ComplianceAttributeDocMappings.IsNullOrEmpty()
                              && attribute.ComplianceAttributeDocMappings.Where(con => !con.CADM_IsDeleted).ToList().Count > AppConsts.NONE)
                        {
                            foreach (RadComboBoxItem item in cmbAdditionalDocument.Items)
                            {
                                if (!item.IsNullOrEmpty())
                                {
                                    Int32 docId = !String.IsNullOrEmpty(item.Value) ? Convert.ToInt32(item.Value) : AppConsts.NONE;
                                    if (attribute.ComplianceAttributeDocMappings.Where(con => !con.CADM_IsDeleted).Any(x => x.CADM_SystemDocumentID == docId))
                                        item.Checked = true;
                                }
                            }
                        }
                    }
                    //END
                }
                //END
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

        #region DropDown Events

        protected void cmbDataType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbItemType = sender as WclComboBox;

            System.Web.UI.WebControls.Panel panel = (System.Web.UI.WebControls.Panel)cmbItemType.Parent;
            if (panel != null)
            {
                ShowHideContentArea(panel, e.Text, true, selectedAttributeTypeId: Convert.ToInt32((sender as WclComboBox).SelectedValue));
            }
        }

        /// <summary>
        /// Binds the attributes as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                grdAttributes.CurrentPageIndex = 0;
                grdAttributes.MasterTableView.SortExpressions.Clear();
                grdAttributes.MasterTableView.FilterExpression = null;

                foreach (GridColumn column in grdAttributes.MasterTableView.OwnerGrid.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                }
                grdAttributes.Rebind();
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

        private void ShowHideContentArea(System.Web.UI.WebControls.Panel panel, string complianceAttributeDatatype, bool clearData = false, Int32 selectedAttributeTypeId = 0)
        {

            System.Web.UI.HtmlControls.HtmlGenericControl divOption = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("divOption");
            System.Web.UI.HtmlControls.HtmlGenericControl divIsSendForIntegration = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("divIsSendForIntegration");
            if (divOption != null)
            {
                if (clearData)
                {
                    WclTextBox txtOptOptions = panel.FindControl("txtOptOptions") as WclTextBox;
                    txtOptOptions.Text = string.Empty;

                }
                divOption.Visible = false;
            }

            if (divIsSendForIntegration != null)
            {
                IsActiveToggle IsSendForIntegration = panel.FindControl("ChkSendForIntegration") as IsActiveToggle;
                if (IsSendForIntegration != null)
                {

                    divIsSendForIntegration.Visible = false;
                    if (!divIsSendForIntegration.Visible)
                    {
                        if (complianceAttributeDatatype.Equals("Date"))
                        {
                            divIsSendForIntegration.Visible = true;
                        }
                    }

                }
            }


            System.Web.UI.HtmlControls.HtmlGenericControl divCharacters = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("divCharacters");
            if (divCharacters != null)
            {
                if (clearData)
                {
                    WclNumericTextBox ntxtTextMaxChars = panel.FindControl("ntxtTextMaxChars") as WclNumericTextBox;
                    ntxtTextMaxChars.Text = string.Empty;
                }
                divCharacters.Visible = false;
            }

            if (divOption != null && complianceAttributeDatatype.Equals("Options"))
                divOption.Visible = true;

            if (divCharacters != null && complianceAttributeDatatype.Equals("Text"))
                divCharacters.Visible = true;

            System.Web.UI.HtmlControls.HtmlGenericControl divAttributeGroup = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("divAttributeGroup");
            //Added IN UAT-4558
            System.Web.UI.HtmlControls.HtmlGenericControl dvAdditionalDocuments = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("dvAdditionalDocuments");
            //END
            if (divAttributeGroup != null)
            {
                var _screeningDocTypeAttrId = Presenter.GetScreeningDocumentAttributeDataTypeId();

                WclComboBox cmbAttributeGroup = panel.FindControl("cmbAttributeGroup") as WclComboBox;
                if (complianceAttributeDatatype.ToLower().Equals("file upload") || complianceAttributeDatatype.ToLower().Equals("view document")
                    || selectedAttributeTypeId == _screeningDocTypeAttrId)
                {
                    cmbAttributeGroup.SelectedIndex = 0;
                    divAttributeGroup.Visible = false;
                    //Added in UAT-4558
                    if (complianceAttributeDatatype.ToLower().Equals("file upload"))
                    {
                        dvAdditionalDocuments.Visible = true;
                    }
                    else
                    {
                        dvAdditionalDocuments.Visible = false;
                    }
                    //END
                }
                else
                {
                    divAttributeGroup.Visible = true;
                    dvAdditionalDocuments.Visible = false; //Added in UAT-4558
                }
            }

            System.Web.UI.HtmlControls.HtmlGenericControl divDocuments = (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("divDocuments");
            if (divDocuments != null)
            {
                WclComboBox cmbDocument = panel.FindControl("cmbDocument") as WclComboBox;
                WclComboBox cmbAttrType = panel.FindControl("cmbAttrType") as WclComboBox;
                if (complianceAttributeDatatype.ToLower().Equals("view document"))
                {
                    List<lkpComplianceAttributeType> lstComplianceAttributeType = Presenter.GetComplianceAttributeType();
                    if (!lstComplianceAttributeType.IsNullOrEmpty())
                    {
                        cmbAttrType.DataSource = lstComplianceAttributeType.Where(cond => cond.Code == ComplianceAttributeType.Manual.GetStringValue()).ToList();
                        cmbAttrType.DataBind();
                    }
                    divDocuments.Visible = true;

                }
                else
                {
                    List<lkpComplianceAttributeType> lstComplianceAttributeType = Presenter.GetComplianceAttributeType();
                    if (!lstComplianceAttributeType.IsNullOrEmpty())
                    {
                        cmbAttrType.DataSource = lstComplianceAttributeType;
                        cmbAttrType.DataBind();
                    }
                    divDocuments.Visible = false;
                    cmbDocument.SelectedIndex = 0;
                }
            }
        }

        private Boolean IsValidOptionFormat(String options)
        {
            if (!String.IsNullOrEmpty(options))
            {
                //UAT-3486
                //string[] arrayOfOptions = options.Split(',');
                string[] arrayOfOptions = options.Split('|');
                if (arrayOfOptions.Length > 0)
                {
                    for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                    {
                        string[] option = arrayOfOptions[counter].Split('=');
                        if (!option.Length.Equals(2) || String.IsNullOrEmpty(option[1]))
                            return false;

                    }
                }
                else
                    return false;
            }
            return true;
        }

        private System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption> GetComplianceAttributeOption(String options)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption> lstComplianceAttributeOption = null;
            if (String.IsNullOrEmpty(options))
                return lstComplianceAttributeOption;

            //UAT-3486
            //string[] arrayOfOptions = options.Split(',');
            string[] arrayOfOptions = options.Split('|');
            if (arrayOfOptions.Length > 0)
            {
                for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                {
                    string[] option = arrayOfOptions[counter].Split('=');
                    if (option.Length.Equals(2))
                    {
                        if (lstComplianceAttributeOption == null)
                            lstComplianceAttributeOption = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption>();

                        lstComplianceAttributeOption.Add(new ComplianceAttributeOption()
                        {
                            OptionText = option[0],
                            OptionValue = option[1],
                            CreatedByID = CurrentUserId,
                            CreatedOn = DateTime.Now,
                            IsActive = true,
                            IsDeleted = false

                        });
                    }
                }
            }
            return lstComplianceAttributeOption;
        }

        private void BindTenant()
        {
            if (IsAdminLoggedIn == true)
            {
                Presenter.GetTenants();
                ddlTenant.DataSource = ListTenants;
                ddlTenant.DataBind();
            }
            else
            {
                pnlTenant.Visible = false;
            }
        }

        private void SetDefaultSelectedTenantId()
        {
            if (ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                ddlTenant.SelectedValue = Convert.ToString(TenantId);
                SelectedTenantId = TenantId;
            }
        }


        #region UAT-3032:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenantId.IsNullOrEmpty() || CurrentViewContext.SelectedTenantId == AppConsts.ONE)
            {
                //Presenter.GetPreferredSelectedTenant();
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
                }
            }
        }
        #endregion

        #endregion
    }
}

