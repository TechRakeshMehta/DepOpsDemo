using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System.Web.UI.WebControls;
using System.Linq;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using Telerik.Web.UI;
using System.Text;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class CustomAttributeControl : BaseUserControl, ICustomAttributeControlView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private CustomAttributeControlPresenter _presenter = new CustomAttributeControlPresenter();
        private Int32 _tenantId;

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            if (this.ControlDisplayMode == DisplayMode.Labels)
            {
                CreateLabels();
                pnlLabelsMode.Visible = true;
                pnlControlsMode.Visible = false;
                pnlReadOnlyLabels.Visible = false;
            }
            else if (this.ControlDisplayMode == DisplayMode.Controls)
            {
                CreateDynamicControls();
                pnlLabelsMode.Visible = false;
                pnlControlsMode.Visible = true;
                pnlReadOnlyLabels.Visible = false;
            }
            else if (this.ControlDisplayMode == DisplayMode.ReadOnlyLabels)
            {
                CreateReadOnlyLabels();
                pnlReadOnlyLabels.Visible = true;
                pnlLabelsMode.Visible = false;
                pnlControlsMode.Visible = false;
            }
        }

        #endregion

        #region   Methods

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate the dynamic attribute control
        /// </summary>
        private void CreateDynamicControls()
        {
            String dataTypeCode = CurrentViewContext.TypeCustomtAttribute.CADataTypeCode.ToLower().Trim();
            String _caValue = CurrentViewContext.TypeCustomtAttribute.CAValue;
            Int64 _mappingId = CurrentViewContext.TypeCustomtAttribute.CAMId.HasValue ? CurrentViewContext.TypeCustomtAttribute.CAMId.Value : AppConsts.NONE;

            //Boolean _isRequired = Convert.ToBoolean(CurrentViewContext.TypeCustomtAttribute.IsRequired); //commented for UAT 4829
            //UAT 4829 start
            Boolean _isRequired;
            if (this.NeedTocheckCustomAttributeEditableSetting && this.IsReadOnly)
            {
                _isRequired = false;
            }
            else
            {
                _isRequired = Convert.ToBoolean(CurrentViewContext.TypeCustomtAttribute.IsRequired);
            }
            //UAT 4829 end

            //UAT-1068
            String regexPattern = CurrentViewContext.TypeCustomtAttribute.RegularExpression;
            String regexErrorMsg = CurrentViewContext.TypeCustomtAttribute.RegExpErrorMsg;

            String _caType = lblLabel.Text = String.IsNullOrEmpty(CurrentViewContext.TypeCustomtAttribute.CALabel)
             ? CurrentViewContext.TypeCustomtAttribute.CAName : CurrentViewContext.TypeCustomtAttribute.CALabel;

            if (_isRequired)
            {
                required.Visible = true;
            }

            hdfIdentity.Value = Convert.ToString(CurrentViewContext.TypeCustomtAttribute.CAMId);

            if (AttributeValues.IsNotNull() && AttributeValues.Count() > 0)
            {
                TypeCustomAttributes typeCustomAttributes = AttributeValues.Where(av => av.CAMId == _mappingId).FirstOrDefault();
                if (typeCustomAttributes.IsNotNull())
                    _caValue = typeCustomAttributes.CAValue;
            }

            if (dataTypeCode == CustomAttributeDatatype.Date.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dPicker = new WclDatePicker();
                dPicker.DateInput.DateFormat = "MM-dd-yyyy";
                dPicker.ID = "dp_" + _mappingId;
                dPicker.DateInput.EmptyMessage = "Select a date";
                dPicker.MinDate = Convert.ToDateTime("01-01-1900");
                dPicker.Enabled = !this.IsReadOnly;
                divControlMode.Controls.Add(dPicker);

                if (_isRequired)
                {
                    ApplyRequiredField(_mappingId, dPicker.ID, _caType);
                }

                if (regexPattern.IsNotNull())
                {
                    ApplyRegularExpressionValidator(_mappingId, dPicker.ID, _caType, regexPattern, regexErrorMsg);
                }

                try
                {
                    if (CurrentViewContext.TypeCustomtAttribute.IsNotNull() && !String.IsNullOrEmpty(_caValue))
                    {
                        dPicker.SelectedDate = Convert.ToDateTime(_caValue);
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }
            }
            else if (dataTypeCode == CustomAttributeDatatype.Text.GetStringValue().ToLower().Trim())
            {
                WclTextBox txtTextType = new WclTextBox();

                txtTextType.ID = "txtTextType_" + _mappingId;
                if ((CurrentViewContext.TypeCustomtAttribute.CAId == this.PeopleSoftId || (!(CurrentViewContext.TypeCustomtAttribute.RelatedCustomAttributeId.IsNullOrEmpty()) && (CurrentViewContext.TypeCustomtAttribute.RelatedCustomAttributeId == this.PeopleSoftId))) && this.IsIntegrationClientOrganisationUser) //3133
                {
                    txtTextType.Enabled = this.IsReadOnly;
                }
                else
                {
                    txtTextType.Enabled = !this.IsReadOnly;
                }
                if (CurrentViewContext.TypeCustomtAttribute.MaxLength.IsNull() || CurrentViewContext.TypeCustomtAttribute.MaxLength <= 0)
                    txtTextType.MaxLength = 50;
                else
                    txtTextType.MaxLength = Convert.ToInt32(CurrentViewContext.TypeCustomtAttribute.MaxLength);

                divControlMode.Controls.Add(txtTextType);
                if (_isRequired)
                    ApplyRequiredField(_mappingId, txtTextType.ID, _caType);

                if (regexPattern.IsNotNull())
                {
                    ApplyRegularExpressionValidator(_mappingId, txtTextType.ID, _caType, regexPattern, regexErrorMsg);
                }

                try
                {

                    if (CurrentViewContext.CurrentViewContext.TypeCustomtAttribute.IsNotNull())
                    {
                        txtTextType.Text = _caValue;
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

            }
            else if (dataTypeCode == CustomAttributeDatatype.Numeric.GetStringValue().ToLower().Trim())
            {
                WclNumericTextBox txtNumeric = new WclNumericTextBox();
                txtNumeric.ID = "txtNumericType_" + _mappingId;
                txtNumeric.Enabled = !this.IsReadOnly;
                divControlMode.Controls.Add(txtNumeric);
                if (_isRequired)
                {
                    ApplyRequiredField(_mappingId, txtNumeric.ID, _caType);
                }

                if (regexPattern.IsNotNull())
                {
                    ApplyRegularExpressionValidator(_mappingId, txtNumeric.ID, _caType, regexPattern, regexErrorMsg);
                }

                try
                {
                    if (CurrentViewContext.TypeCustomtAttribute.IsNotNull() && !String.IsNullOrEmpty(_caValue))
                    {
                        txtNumeric.Text = _caValue;
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }
            }

            else if (dataTypeCode == CustomAttributeDatatype.Boolean.GetStringValue().ToLower().Trim())
            {
                RadioButtonList rbtnList = new RadioButtonList();
                rbtnList.RepeatDirection = RepeatDirection.Horizontal;

                rbtnList.Items.Add(new ListItem { Text = "Yes", Value = "1" });
                rbtnList.Items.Add(new ListItem { Text = "No", Value = "2" });
                rbtnList.ID = "rbtnListType_" + _mappingId;
                rbtnList.Enabled = !this.IsReadOnly;

                divControlMode.Controls.Add(rbtnList);

                if (regexPattern.IsNotNull())
                {
                    ApplyRegularExpressionValidator(_mappingId, rbtnList.ID, _caType, regexPattern, regexErrorMsg);
                }

                if (_isRequired)
                {
                    ApplyRequiredField(_mappingId, rbtnList.ID, _caType);
                }
                else
                {
                    if (rbtnList.Items.FindByText("NA").IsNull())
                        rbtnList.Items.Add(new ListItem { Text = "NA", Value = "0", Selected = true });
                }
                //if (CurrentViewContext.TypeCustomtAttribute.IsRequired.IsNull() || _isRequired)
                //    rbtnList.Items.RemoveAt(0);
                try
                {
                    if (CurrentViewContext.TypeCustomtAttribute.IsNotNull() && !String.IsNullOrEmpty(_caValue))
                    {
                        if (String.IsNullOrEmpty(_caValue))
                            rbtnList.SelectedValue = "0";
                        else
                            rbtnList.SelectedValue = _caValue;
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }
            }
            //UAT 1438: Enhancement to allow students to select a User Group. 
            else if (dataTypeCode == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim())
            {
                WclComboBox dropDownList = new WclComboBox();
                dropDownList.ID = "cmb_" + _mappingId;
                dropDownList.Enabled = !this.IsReadOnly;
                dropDownList.EmptyMessage = "-- SELECT --";
                dropDownList.DataTextField = "UG_Name";
                dropDownList.DataValueField = "UG_ID";
                dropDownList.CheckBoxes = true;
                dropDownList.MaxHeight = AppConsts.TWOHUNDREDTWENTYTWO;
                dropDownList.EnableCheckAllItemsCheckBox = true;
                //dropDownList.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
                dropDownList.EnableTextSelection = true;
                //dropDownList.OnClientKeyPressing = "openCmbBoxOnTab";
                //  Presenter.GetAllUserGroup();
                if (!lstUserGroups.IsNullOrEmpty())
                {
                    dropDownList.DataSource = lstUserGroups.ToList();
                    dropDownList.DataBind();
                }

                divControlMode.Controls.Add(dropDownList);

                if (_isRequired)
                {
                    ApplyRequiredField(_mappingId, dropDownList.ID, _caType);
                }

                try
                {
                    if (dropDownList.Items.IsNotNull() && IsUserGroupSlctdValuesdisabled)
                    {
                        dropDownList.EnableCheckAllItemsCheckBox = false;
                    }
                    //Presenter.GetUserGroupsForUser();
                    if (lstUserGroupsForUser.IsNotNull())
                    {
                        if (dropDownList.Items.IsNotNull())
                        {
                            foreach (RadComboBoxItem item in dropDownList.Items)
                            {
                                if (lstUserGroupsForUser.Select(col => col.UG_ID).Contains(Convert.ToInt32(item.Value)))
                                {
                                    item.Checked = true;
                                    #region UAT-3455
                                    if (IsUserGroupSlctdValuesdisabled)
                                    {
                                        item.Enabled = false;

                                        if (IsApplicantProfileScreen)
                                        {
                                            if (!lstPreviousSelectedUserGroupIds.IsNullOrEmpty() && lstPreviousSelectedUserGroupIds.Contains(Convert.ToInt32(item.Value)))
                                            {
                                                item.Checked = true;
                                                item.Enabled = false;
                                            }
                                            else
                                            {
                                                item.Checked = true;
                                                item.Enabled = true;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }
            }

            if (CurrentViewContext.TypeCustomtAttribute.IsNotNull()
                //&& !String.IsNullOrEmpty(CurrentViewContext.TypeCustomtAttribute.CAValue)
                && CurrentViewContext.TypeCustomtAttribute.CAVId.IsNotNull())
            {
                hdfCAVId.Value = Convert.ToString(CurrentViewContext.TypeCustomtAttribute.CAVId);
            }
            if (CurrentViewContext.TypeCustomtAttribute.IsNotNull())
            {
                hdfCAId.Value = Convert.ToString(CurrentViewContext.TypeCustomtAttribute.CAId);
            }



        }

        /// <summary>
        /// Generate the dynamic attribute labels for special CSS of Order confirmation page
        /// </summary>
        private void CreateLabels()
        {
            String _caValue = CurrentViewContext.TypeCustomtAttribute.CAValue;
            Int32 _camId = CurrentViewContext.TypeCustomtAttribute.CAMId.Value;
            String _caType = lblLabelMode.Text = String.IsNullOrEmpty(CurrentViewContext.TypeCustomtAttribute.CALabel)
           ? CurrentViewContext.TypeCustomtAttribute.CAName : CurrentViewContext.TypeCustomtAttribute.CALabel;
            //lblLabelMode.Text += ":";
            String _caDataTypeCode = CurrentViewContext.TypeCustomtAttribute.CADataTypeCode;
            try
            {
                //UAT 1438: Enhancement to allow students to select a User Group.
                if (_caDataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim())
                {
                    //Presenter.GetUserGroupsForUser();
                    if (!CurrentViewContext.lstUserGroupsForUser.IsNullOrEmpty())
                    {
                        lblAttributeValueLabelMode.Text = GenerateDelemittedUserGroupCustomAttribute();
                    }
                }
                else if (_caDataTypeCode.ToLower().Trim() != CustomAttributeDatatype.Boolean.GetStringValue().ToLower().Trim())
                {
                    lblAttributeValueLabelMode.Text = String.IsNullOrEmpty(_caValue) ? String.Empty : _caValue;
                }

                else
                {
                    if (String.IsNullOrEmpty(_caValue) || _caValue == "0")
                        lblAttributeValueLabelMode.Text = "NA";
                    else if (_caValue == "1")
                        lblAttributeValueLabelMode.Text = "Yes";
                    else
                        lblAttributeValueLabelMode.Text = "No";
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }
        }

        /// <summary>
        /// Generate the dynamic attribute for Read only mode with ronly CSS for labels
        /// </summary>
        private void CreateReadOnlyLabels()
        {
            String _caValue = CurrentViewContext.TypeCustomtAttribute.CAValue;
            Int32 _camId = CurrentViewContext.TypeCustomtAttribute.CAMId.Value;
            String _caType = lblReadOnly.Text = String.IsNullOrEmpty(CurrentViewContext.TypeCustomtAttribute.CALabel)
           ? CurrentViewContext.TypeCustomtAttribute.CAName : CurrentViewContext.TypeCustomtAttribute.CALabel;

            String _caDataTypeCode = CurrentViewContext.TypeCustomtAttribute.CADataTypeCode;
            try
            {
                //UAT 1438: Enhancement to allow students to select a User Group.
                if (_caDataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim())
                {
                    //Presenter.GetUserGroupsForUser();
                    if (!CurrentViewContext.lstUserGroupsForUser.IsNullOrEmpty())
                    {
                        lblAttributeValueReadOnlyMode.Text = GenerateDelemittedUserGroupCustomAttribute();
                    }
                }
                else if (_caDataTypeCode.ToLower().Trim() != CustomAttributeDatatype.Boolean.GetStringValue().ToLower().Trim())
                {
                    lblAttributeValueReadOnlyMode.Text = String.IsNullOrEmpty(_caValue) ? String.Empty : _caValue;
                }
                else
                {
                    if (String.IsNullOrEmpty(_caValue) || _caValue == "0")
                        lblAttributeValueReadOnlyMode.Text = "NA";
                    else if (_caValue == "1")
                        lblAttributeValueReadOnlyMode.Text = "Yes";
                    else
                        lblAttributeValueReadOnlyMode.Text = "No";
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }
        }

        /// <summary>
        /// UAT 1438: Enhancement to allow students to select a User Group.
        /// </summary>
        /// <returns></returns>
        private string GenerateDelemittedUserGroupCustomAttribute()
        {
            Boolean flag = false;
            StringBuilder sbUserGroup = new StringBuilder();
            foreach (UserGroup userGroup in CurrentViewContext.lstUserGroupsForUser)
            {
                if (flag)
                    sbUserGroup.Append(", ");

                sbUserGroup.Append(userGroup.UG_Name);
                flag = true;
            }

            return sbUserGroup.ToString();
        }

        private void ApplyRequiredField(Int64 mappingId, String controlId, String errorMessage)
        {
            RequiredFieldValidator rfValidator = new RequiredFieldValidator();
            rfValidator.ID = "rf_" + controlId;
            rfValidator.Display = ValidatorDisplay.Dynamic;
            rfValidator.ControlToValidate = controlId;
            rfValidator.ErrorMessage = errorMessage + " is required.";
            rfValidator.CssClass = "errmsg";

            if (!String.IsNullOrEmpty(this.ValidationGroup))
                rfValidator.ValidationGroup = this.ValidationGroup;

            System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();
            div.Attributes.Add("class", "vldx");
            div.Controls.Add(rfValidator);
            divControlMode.Controls.Add(div);
        }

        private void ApplyRegularExpressionValidator(Int64 mappingId, String controlId, String errorMessage, String regexPattern, String regexErrorMsg)
        {
            RegularExpressionValidator revValidator = new RegularExpressionValidator();
            revValidator.ID = "rev_" + controlId;
            revValidator.Display = ValidatorDisplay.Dynamic;
            revValidator.ControlToValidate = controlId;
            revValidator.ErrorMessage = regexErrorMsg;
            revValidator.CssClass = "errmsg";
            revValidator.ValidationExpression = regexPattern;

            if (!String.IsNullOrEmpty(this.ValidationGroup))
                revValidator.ValidationGroup = this.ValidationGroup;

            System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();
            div.Attributes.Add("class", "vldx");
            div.Controls.Add(revValidator);
            divControlMode.Controls.Add(div);
        }

        #endregion

        #endregion

        #region  Properties

        #region Public Properties


        public CustomAttributeControlPresenter Presenter
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

        public TypeCustomAttributes TypeCustomtAttribute
        {
            get;
            set;
        }

        /// <summary>
        /// Property to enable disable the controls
        /// </summary>
        public Boolean IsReadOnly { get; set; }

        /// <summary>
        /// Property to display the labels or actual controls
        /// </summary>
        public DisplayMode ControlDisplayMode { get; set; }


        public ICustomAttributeControlView CurrentViewContext
        {
            get { return this; }
        }
        public Int32 SelectedRecordId
        {
            get;
            set;
        }

        public String ValidationGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Receive the attribute values and set to the controls, in Edit profile, in order flow
        /// </summary>
        public List<TypeCustomAttributes> AttributeValues
        {
            get;
            set;
        }

        #region UAT 1438: Enhancement to allow students to select a User Group.

        public IQueryable<UserGroup> lstUserGroups
        {
            get;
            set;
        }

        public IList<UserGroup> lstUserGroupsForUser
        {
            get;
            set;
        }

        public Int32 TenantID
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 OrganizationUserId
        {
            get;
            set;
        }

        public Boolean ShowReadOnlyUserGroupCustomAttribute
        {
            get;
            set;
        }

        #region UAT-3133
        public Boolean IsIntegrationClientOrganisationUser
        {
            get;
            set;
        }

        public Int32 PeopleSoftId
        {
            get;
            set;
        }
        #endregion

        #endregion

        //UAT-3455
        public Boolean IsUserGroupSlctdValuesdisabled
        {
            get;
            set;
        }
        public Boolean IsApplicantProfileScreen
        {
            get;
            set;
        }

        public IList<UserGroup> lstUsrGrpSavedValues
        {
            get;
            set;
        }

        public List<Int32> lstPreviousSelectedUserGroupIds
        {
            get
            {
                if (!ViewState["lstPreviousSelectedUserGroupIds"].IsNullOrEmpty())
                    return (List<Int32>)ViewState["lstPreviousSelectedUserGroupIds"];
                return null;
            }
            set
            {
                ViewState["lstPreviousSelectedUserGroupIds"] = value;
            }
        }

        //UAT 4829
        public Boolean NeedTocheckCustomAttributeEditableSetting
        {
            get;
            set;
        }
        #endregion

        #region Private Properties

        #endregion

        #endregion



    }
}

