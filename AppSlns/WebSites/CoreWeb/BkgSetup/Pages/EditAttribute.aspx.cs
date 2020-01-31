using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using CoreWeb.Shell.Views;
using System.Text.RegularExpressions;

namespace CoreWeb.BkgSetup.Views
{
    public partial class EditAttribute : BaseWebPage, IEditAttributeView
    {
        #region Variables

        #region Private variables
        private EditAttributePresenter _presenter = new EditAttributePresenter();
        Boolean _editLocally = false;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!Request.QueryString["tenantId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["tenantId"]);
                    }
                    Presenter.GetAttributeDataType();
                    BindDataToForm();
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

        private void BindDataToForm()
        {
            SetFormData();
            SetFormMode(false);
            ResetButtons(true);
            ApplyActionLevelPermission(ActionCollection, "Manage Package Service SetUp");
        }

        #endregion

        #region DropDown Events

        protected void cmbDataType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ResetButtons(false);
                ShowHideContentArea(cmbDataType.SelectedItem.Text, true);
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
        protected void fsucCmdBarCat_SaveClick(object sender, EventArgs e)
        {
            try
            {
                _editLocally = (bool)ViewState["_editLocally"];
                ServiceAttributeContract serviceAttributeContract = new ServiceAttributeContract();
                serviceAttributeContract.ServiceAttributeID = CurrentViewContext.AttributeId;
                serviceAttributeContract.Name = txtAttributeName.Text;
                serviceAttributeContract.AttributeLabel = txtAttributeLabel.Text;
                serviceAttributeContract.Description = txtAttributeDescription.Text;
                serviceAttributeContract.IsActive = chkActive.Checked;
                //serviceAttributeContract.IsRequired = chkRequired.Checked;
                serviceAttributeContract.ServiceAttributeDatatypeID = Convert.ToInt32(cmbDataType.SelectedValue);

                serviceAttributeContract.ValidationMessage = txtErrorMessage.Text.Trim();
                serviceAttributeContract.ValidationExpression = txtRegularExpression.Text.Trim();

                //serviceAttributeContract

                if (!IsValidOptionFormat(txtOptOptions.Text))
                {
                    base.ShowErrorMessage("Please enter valid options format i.e. Positive=1,Negative=2.");
                    return;
                }

                serviceAttributeContract.lstClientServiceAttributeOption = GetServiceAttributeOption(txtOptOptions.Text);
                if (String.IsNullOrEmpty(ntxtMaxChars.Text))
                    serviceAttributeContract.MaximumCharacters = null;
                else
                    serviceAttributeContract.MaximumCharacters = Convert.ToInt32(ntxtMaxChars.Text);
                if (String.IsNullOrEmpty(nTxtMinLength.Text))
                    serviceAttributeContract.MinimumCharacters = null;
                else
                    serviceAttributeContract.MinimumCharacters = Convert.ToInt32(nTxtMinLength.Text);
                if (String.IsNullOrEmpty(nTxtMaxIntegerValue.Text))
                    serviceAttributeContract.MaximumNumericvalue = null;
                else
                    serviceAttributeContract.MaximumNumericvalue = Convert.ToInt32(nTxtMaxIntegerValue.Text);
                if (String.IsNullOrEmpty(nTxtMinimunIntegerValue.Text))
                    serviceAttributeContract.MinimumNumericvalue = null;
                else
                    serviceAttributeContract.MinimumNumericvalue = Convert.ToInt32(nTxtMinimunIntegerValue.Text);
                if (dpkrMaxDateValue.SelectedDate == null)
                    serviceAttributeContract.MaximumDatevalue = null;
                else
                    serviceAttributeContract.MaximumDatevalue = dpkrMaxDateValue.SelectedDate;
                if (dpkrMinDateValue.SelectedDate == null)
                    serviceAttributeContract.MinimumDatevalue = null;
                else
                    serviceAttributeContract.MinimumDatevalue = dpkrMinDateValue.SelectedDate;

                serviceAttributeContract.ModifiedByID = CurrentViewContext.CurrentLoggedInUserId;
                serviceAttributeContract.ModifiedOn = DateTime.Now;
                serviceAttributeContract.TenantID = CurrentViewContext.TenantId;
                serviceAttributeContract.IsRequired = chkIsRequired.Checked;
                serviceAttributeContract.IsDisplay = chkIsDisplay.Checked;
                serviceAttributeContract.IsHiddenFromUI = chkIsHiddenFromUI.Checked;
                serviceAttributeContract.IsEditable = Convert.ToBoolean(hdnIsEditable.Value);
                serviceAttributeContract.UpdateAllData = Convert.ToBoolean(hdnIsCompleteEdit.Value);
                serviceAttributeContract.ServiceAttributeMappingId = Convert.ToInt32(hdnServiceAttributeMappingID.Value);
                if (Presenter.UpdateServiceAttribute(serviceAttributeContract, _editLocally))
                {
                    base.ShowSuccessMessage("Service Attribute updated successfully.");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }

                ResetButtons(true);
                SetFormMode(false);
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
        protected void fsucCmdBarCat_CancelClick(object sender, EventArgs e)
        {
            try
            {
                BindDataToForm();
                //ResetButtons(true);
                //SetFormMode(false);
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

        /// <summary>
        /// Event to edit Attribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarEditAttribute_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.IsEditable == "True")
                {
                    if (CurrentViewContext.IsSystemConfigured == "False")
                    {
                        SetFormMode(true);
                    }
                    else
                    {
                        SetFormMode(false);
                        SetFormModeForIsSystemConfigered();
                    }
                    ViewState["_editLocally"] = false;
                    divSaveButton.Visible = true;
                }
                else { }
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

        protected void btnEditLocally_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.IsEditable == "True")
                {
                    SetFormMode(false);
                    SetFormModeForIsSystemConfigered();
                    txtAttributeLabel.Enabled = true;
                    divSaveButton.Visible = true;
                    ViewState["_editLocally"] = true;
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

        #region Properties

        #region Public Properties
        public EditAttributePresenter Presenter
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

        Int32 IEditAttributeView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 IEditAttributeView.TenantId
        {
            get
            {
                return (Int32)(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }
        Int32 IEditAttributeView.AttributeId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "ATT"));
            }
        }

        private Int32 PackageID
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "PKG"));
            }
        }

        private Int32 ServiceGroup
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "SVCG"));
            }
        }

        private Int32 ServiceID
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "SVC"));
            }
        }

        private Int32 AttributeGroup
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "ATTG"));
            }
        }

        String IEditAttributeView.IsSystemConfigured
        {
            get
            {
                return hdnIsSystemConfigured.Value;
            }

        }

        String IEditAttributeView.IsCompleteEdit
        {
            get
            {
                return hdnIsCompleteEdit.Value;
            }

        }

        String IEditAttributeView.IsEditable
        {
            get
            {
                return hdnIsEditable.Value;
            }

        }

        List<lkpSvcAttributeDataType> IEditAttributeView.listAttributeDataType
        {
            set
            {
                cmbDataType.DataSource = value;
                cmbDataType.DataBind();
            }
        }

        public ManageServiceAttributeData ManageServiceAttributeData
        {
            get;
            set;
        }

        public ServiceAttributeParameter serviceAttributeParameter
        {
            get;
            set;
        }

        public String NodeId
        {
            get
            {
                if (!Request.QueryString["nodeId"].IsNullOrEmpty())
                {
                    return Request.QueryString["nodeId"];
                }
                return String.Empty;
            }

        }
        #region Current View Context
        private IEditAttributeView CurrentViewContext
        {
            get { return this; }
        }
        #endregion
        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Private Methods
        private void ShowHideContentArea(string complianceAttributeDatatype, bool clearData = false)
        {
            if (clearData)
            {
                txtOptOptions.Text = string.Empty;
                ntxtMaxChars.Text = string.Empty;
                nTxtMinLength.Text = string.Empty;
                nTxtMaxIntegerValue.Text = string.Empty;
                nTxtMinimunIntegerValue.Text = string.Empty;
                dpkrMaxDateValue.SelectedDate = null;
                dpkrMinDateValue.SelectedDate = null;
            }
            //divOption.Style["display"] = "none";
            //divCharacters.Style["display"] = "none";
            //divDateType.Style["display"] = "none";
            //divIntegerType.Style["display"] = "none";

            divOption.Visible = false;
            divCharacters.Visible = false;
            divDateType.Visible = false;
            divIntegerType.Visible = false;
            divRegexValidate.Visible = false;

            //Disable Validators
            //rfvOptions.Enabled = false;
            //rfvMinimumLength.Enabled = false;
            //rfvMaximumLength.Enabled = false;
            //rfvMaxDate.Enabled = false;
            //rfvMinDate.Enabled = false;
            //rfvMaxIntegerValue.Enabled = false;
            //rfvMinimumIntegerValue.Enabled = false;
            if (divOption != null && complianceAttributeDatatype.Equals("Option"))
            {
                divOption.Visible = true;
            }

            if (divCharacters != null && complianceAttributeDatatype.Equals("Text"))
            {
                divCharacters.Visible = true;
                divRegexValidate.Visible = true;
                //rfvMinimumLength.Enabled = true;
                //rfvMaximumLength.Enabled = true;
            }

            if (divCharacters != null && complianceAttributeDatatype.Equals("Date"))
            {
                divDateType.Visible = true;
                //rfvMaxDate.Enabled = true;
                //rfvMinDate.Enabled = true;
            }
            if (divCharacters != null && complianceAttributeDatatype.Equals("Numeric"))
            {
                //divIntegerType.Style["display"] = "block";
                //rfvMaxIntegerValue.Enabled = true;
                //rfvMinimumIntegerValue.Enabled = true;
                divIntegerType.Visible = true;
            }
        }

        private void ResetButtons(Boolean isReset)
        {
            if (isReset)
            {
                divSaveButton.Visible = false;

            }
            else
            {
                divSaveButton.Visible = true;
            }
        }

        private void SetFormMode(Boolean isEnabled)
        {

            txtAttributeName.Enabled = isEnabled;
            txtAttributeDescription.Enabled = isEnabled;
            txtAttributeLabel.Enabled = isEnabled;
            chkActive.IsActiveEnable = isEnabled;
            //chkRequired.Enabled = isEnabled;
            //txtReqValMessage.Enabled = isEnabled;
            cmbDataType.Enabled = isEnabled;
            txtOptOptions.Enabled = isEnabled;
            ntxtMaxChars.Enabled = isEnabled;
            nTxtMinLength.Enabled = isEnabled;
            dpkrMaxDateValue.Enabled = isEnabled;
            dpkrMinDateValue.Enabled = isEnabled;
            nTxtMaxIntegerValue.Enabled = isEnabled;
            nTxtMaxIntegerValue.Enabled = isEnabled;
            nTxtMinimunIntegerValue.Enabled = isEnabled;
            chkIsRequired.Enabled = isEnabled;
            chkIsDisplay.Enabled = isEnabled;
            chkIsHiddenFromUI.Enabled = isEnabled;
            txtRegularExpression.Enabled = isEnabled;
            txtErrorMessage.Enabled = isEnabled;
            txtInputToValidate.Enabled = isEnabled;
            btnValidateRegExp.Enabled = isEnabled;
            if (!isEnabled)
            {
                lblValidStatus.Text = "";
                txtInputToValidate.Text = "";
            }
        }

        private void SetFormModeForIsSystemConfigered()
        {
            chkIsRequired.Enabled = true;
            chkIsDisplay.Enabled = true;
            chkIsHiddenFromUI.Enabled = true;
            txtRegularExpression.Enabled = true;
            txtErrorMessage.Enabled = true;
            txtInputToValidate.Enabled = true;
            btnValidateRegExp.Enabled = true;
        }

        private void SetFormData()
        {
            ServiceAttributeParameter serviceAttribute = new ServiceAttributeParameter();
            serviceAttribute.PackageId = PackageID;
            serviceAttribute.ServiceGroupId = ServiceGroup;
            serviceAttribute.ServiceId = ServiceID;
            serviceAttribute.AttributeGroupId = AttributeGroup;
            serviceAttribute.AttributeId = CurrentViewContext.AttributeId;
            serviceAttributeParameter = serviceAttribute;
            Presenter.GetBkgSvcAttributeDataForEdit();
            if (ManageServiceAttributeData.IsNotNull())
            {
                txtAttributeName.Text = ManageServiceAttributeData.Name;
                txtAttributeDescription.Text = ManageServiceAttributeData.Description;
                txtAttributeLabel.Text = ManageServiceAttributeData.Lable;
                // chkRequired.Checked = attributeData.BSA_IsRequired;
                //if (chkRequired.Checked)
                //{
                //    divIsRequired.Style["display"] = "none";
                //}
                //txtReqValMessage.Text = attributeData.BSA_ReqValidationMessage;
                chkActive.Checked = ManageServiceAttributeData.Active;
                txtOptOptions.Text = ManageServiceAttributeData.OptionValues;
                if (ManageServiceAttributeData.MaxLength != null && ManageServiceAttributeData.MaxLength != 0)
                    ntxtMaxChars.Text = Convert.ToString(ManageServiceAttributeData.MaxLength);
                if (ManageServiceAttributeData.MinLength != null) //&& ManageServiceAttributeData.MinLength != 0)
                    nTxtMinLength.Text = Convert.ToString(ManageServiceAttributeData.MinLength);
                if (ManageServiceAttributeData.MaxIntValue != null && ManageServiceAttributeData.MaxIntValue != 0)
                    nTxtMaxIntegerValue.Text = Convert.ToString(ManageServiceAttributeData.MaxIntValue);
                if (ManageServiceAttributeData.MinIntValue != null) //&& ManageServiceAttributeData.MinIntValue != 0)
                    nTxtMinimunIntegerValue.Text = Convert.ToString(ManageServiceAttributeData.MinIntValue);
                if (!ManageServiceAttributeData.MaxDateValue.IsNullOrEmpty())
                {
                    dpkrMaxDateValue.MaxDate = Convert.ToDateTime(ManageServiceAttributeData.MaxDateValue).Date;
                    dpkrMaxDateValue.SelectedDate = Convert.ToDateTime(ManageServiceAttributeData.MaxDateValue);
                }
                if (!ManageServiceAttributeData.MinDateValue.IsNullOrEmpty())
                {
                    dpkrMinDateValue.MinDate = Convert.ToDateTime(ManageServiceAttributeData.MinDateValue).Date;
                    dpkrMinDateValue.SelectedDate = Convert.ToDateTime(ManageServiceAttributeData.MinDateValue);
                }
                if (ManageServiceAttributeData.DataTypeID.IsNotNull() && ManageServiceAttributeData.DataTypeID != 0 && cmbDataType.Items.Count > 0)
                    cmbDataType.SelectedValue = Convert.ToString(ManageServiceAttributeData.DataTypeID);

                txtRegularExpression.Text = ManageServiceAttributeData.ValidationExpression;
                txtErrorMessage.Text = ManageServiceAttributeData.ValidationMessage;

                hdnIsSystemConfigured.Value = Convert.ToString(ManageServiceAttributeData.IsSystmComfigered);
                hdnIsCompleteEdit.Value = Convert.ToString(ManageServiceAttributeData.ShowAllDataInEditForm);
                hdnIsEditable.Value = Convert.ToString(ManageServiceAttributeData.IsEditable);
                hdnServiceAttributeMappingID.Value = Convert.ToString(ManageServiceAttributeData.ServiceIdToBeUpdated);
                //Hide EditLocally button for three attribute groups i.e Personal Alias,Residential history and personal information.
                lnkbtnEditLocally.Visible = IsEditLocallyButtonVisible(ManageServiceAttributeData.AttributeGroupCode);
                chkIsRequired.Checked = ManageServiceAttributeData.IsRequired;
                chkIsDisplay.Checked = ManageServiceAttributeData.IsDisplay;
                chkIsHiddenFromUI.Checked = ManageServiceAttributeData.IsHiddenFromUI;
                if (!ManageServiceAttributeData.IsEditable)
                {
                    SetFormMode(false);
                    divSaveButton.Visible = false;
                    spnAttributeTitle.InnerText = "Attribute Details";
                    divEditButton.Visible = false;
                    lnkbtnEditLocally.Visible = false;
                }
                ShowHideContentArea(ManageServiceAttributeData.AttributeDataType);
            }
        }

        private Boolean IsValidOptionFormat(String options)
        {
            if (!String.IsNullOrEmpty(options))
            {
                string[] arrayOfOptions = options.Split(',');
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

        private System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption> GetServiceAttributeOption(String options)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption> lstServiceAttributeOption = null;
            if (String.IsNullOrEmpty(options))
                return lstServiceAttributeOption;

            string[] arrayOfOptions = options.Split(',');
            if (arrayOfOptions.Length > 0)
            {
                for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                {
                    string[] option = arrayOfOptions[counter].Split('=');
                    if (option.Length.Equals(2))
                    {
                        if (lstServiceAttributeOption == null)
                            lstServiceAttributeOption = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption>();

                        lstServiceAttributeOption.Add(new BkgSvcAttributeOption()
                        {
                            EBSAO_OptionText = option[0],
                            EBSAO_OptionValue = option[1],
                            EBSAO_CreatedByID = CurrentViewContext.CurrentLoggedInUserId,
                            EBSAO_CreatedOn = DateTime.Now,
                            EBSAO_IsActive = true,
                            EBSAO_IsDeleted = false

                        });
                    }
                }
            }
            return lstServiceAttributeOption;
        }

        private Boolean IsEditLocallyButtonVisible(String attributeGroupCode)
        {
            if (attributeGroupCode.ToUpper() == AppConsts.MVR_ATTRIBUTE_GROUP_CODE || attributeGroupCode.ToUpper() == AppConsts.RESIDENTIAL_HISTORY_ATTRIBUTE_GROUP_CODE || attributeGroupCode.ToUpper() == AppConsts.PERSONAL_INFORMATION_ATTRIBUTE_GROUP_CODE || attributeGroupCode.ToUpper() == AppConsts.PERSONAL_ALIAS_ATTRIBUTE_GROUP_CODE)
                return false;
            return true;
        }
        #endregion

        #region Public Methods

        #endregion

        #endregion

        #region Apply Permissions


        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
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
                                if (x.FeatureAction.CustomActionId == "Edit Attribute")
                                {
                                    btnEdit.Enabled = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Edit Attribute")
                                {
                                    btnEdit.Visible = false;
                                }
                                break;
                            }
                    }

                });
            }
        }


        #endregion

        protected void btnValidateRegExp_Click(object sender, EventArgs e)
        {            
            if (txtRegularExpression.Text.IsNullOrEmpty())
            {
                lblValidStatus.Text = "Please enter Regular Expression to Validate.";
                lblValidStatus.ForeColor = System.Drawing.Color.Red;
            }
            else if (txtInputToValidate.Text.IsNullOrEmpty())
            {
                lblValidStatus.Text = "Please enter Input Text to Validate.";
                lblValidStatus.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (Regex.IsMatch(txtInputToValidate.Text, txtRegularExpression.Text))
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
    }
}