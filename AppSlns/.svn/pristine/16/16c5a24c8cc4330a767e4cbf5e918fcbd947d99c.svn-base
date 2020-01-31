#region Namespaces

#region System Defined Namespaces

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#region User Defined Namespaces

using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.SystemSetUp;
using Entity;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.Reports.Views;
using System.Text;


#endregion

#endregion

namespace CoreWeb.CommonOperations.Views
{
    public partial class SavedReportsDetailsPage : BaseWebPage, ISavedReportsDetailsPageView
    {
        #region Variables

        #region Private Variables

        private SavedReportsDetailsPagePresenter _presenter = new SavedReportsDetailsPagePresenter();
        private String _viewType;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public SavedReportsDetailsPagePresenter Presenter
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

        public ISavedReportsDetailsPageView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String ISavedReportsDetailsPageView.ErrorMessage { get; set; }

        Int32 ISavedReportsDetailsPageView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 ISavedReportsDetailsPageView.SelectedFavParamID
        {
            get { return (Int32)(ViewState["SelectedFavParamID"]); }
            set { ViewState["SelectedFavParamID"] = value; }
        }

        ReportFavouriteParameter ISavedReportsDetailsPageView.SelectedFavouriteParameter
        {
            get { return (ReportFavouriteParameter)(ViewState["SelectedFavouriteParameter"]); }
            set { ViewState["SelectedFavouriteParameter"] = value; }
        }

        Int32 ISavedReportsDetailsPageView.SelectedTenantID
        {
            get
            {
                String institutionParameterCode = ReportParameters.INSTITUTE.GetStringValue();
                FavParamReportParamMapping favParamMapping = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                .Where(cond => cond.lkpReportParameter.RP_Code == institutionParameterCode)
                                    .FirstOrDefault();
                if (favParamMapping.IsNullOrEmpty())
                {
                    return AppConsts.NONE;
                }
                return Convert.ToInt32(CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                .Where(cond => cond.lkpReportParameter.RP_Code == institutionParameterCode)
                                    .FirstOrDefault().FPRPM_Value);
            }
        }

        String ISavedReportsDetailsPageView.SelectedTenantIDs
        {
            get
            {
                String institutionParameterCode = ReportParameters.INSTITUTE.GetStringValue();
                FavParamReportParamMapping favParamMapping = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                .Where(cond => cond.lkpReportParameter.RP_Code == institutionParameterCode)
                                    .FirstOrDefault();
                if (favParamMapping.IsNullOrEmpty())
                {
                    return String.Empty;
                }
                return CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                .Where(cond => cond.lkpReportParameter.RP_Code == institutionParameterCode)
                                    .FirstOrDefault().FPRPM_Value;
            }
        }

        String ISavedReportsDetailsPageView.SelectedNodeIds
        {
            get
            {
                String hierachyParameterCode = ReportParameters.HIERARCHY.GetStringValue();
                FavParamReportParamMapping favParamMapping = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                               .Where(cond => cond.lkpReportParameter.RP_Code == hierachyParameterCode)
                                   .FirstOrDefault();
                if (favParamMapping.IsNullOrEmpty())
                {
                    return String.Empty;
                }
                return favParamMapping.FPRPM_Value;
            }
        }

        String ISavedReportsDetailsPageView.SelectedCategoryIds
        {
            get
            {
                String categoryParameterCode = ReportParameters.CATEGORY.GetStringValue();
                FavParamReportParamMapping favParamMapping = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                .Where(cond => cond.lkpReportParameter.RP_Code == categoryParameterCode)
                                    .FirstOrDefault();
                if (favParamMapping.IsNullOrEmpty())
                {
                    return String.Empty;
                }
                return favParamMapping.FPRPM_Value;
            }
        }

        Int32 ISavedReportsDetailsPageView.SelectedUserID
        {
            get
            {
                String userIDParameterCode = ReportParameters.USER_ID.GetStringValue();
                FavParamReportParamMapping favParamMapping = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                .Where(cond => cond.lkpReportParameter.RP_Code == userIDParameterCode)
                                    .FirstOrDefault();
                if (favParamMapping.IsNullOrEmpty())
                {
                    return AppConsts.NONE;
                }
                return Convert.ToInt32(favParamMapping.FPRPM_Value);
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
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                if (!this.IsPostBack)
                {
                    if (!Request.QueryString["Id"].IsNullOrEmpty())
                    {
                        CurrentViewContext.SelectedFavParamID = Convert.ToInt32(Request.QueryString["Id"]);
                    }
                    Presenter.OnViewInitialized();
                    //iframeReportViewer.Src = String.Empty;
                }
                BindParameterDetails();

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

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {

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

        private void BindParameterDetails()
        {
            Presenter.GetSelectedFavParamDetails();
            if (!CurrentViewContext.SelectedFavouriteParameter.IsNullOrEmpty())
            {
                if (!this.IsPostBack)
                {
                    hdnSelectedInstitutionID.Value = CurrentViewContext.SelectedTenantIDs;
                    hdnSelectedNodeIDs.Value = CurrentViewContext.SelectedNodeIds;
                    hdnSelectedCategoryIDs.Value = CurrentViewContext.SelectedCategoryIds;
                    lblFavParamName.Text = (CurrentViewContext.SelectedFavouriteParameter.Report.RP_Name
                                       + " > " + CurrentViewContext.SelectedFavouriteParameter.RFP_Name).HtmlEncode();
                    txtName.Text = CurrentViewContext.SelectedFavouriteParameter.RFP_Name;
                    txtDescription.Text = CurrentViewContext.SelectedFavouriteParameter.RFP_Description;
                }

                CreateDynamicControls();
            }
        }

        /// <summary>
        /// Generate the dynamic attribute
        /// </summary>
        /// <param name="clientItemAttribute">Attribute for which control is to be generated</param>
        private void CreateDynamicControls()
        {
            String emptyControlType = ControlType.EMPTY.GetStringValue();
            List<FavParamReportParamMapping> lstFavParamReportParamMapping = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                                                            .Where(cond => !cond.FPRPM_IsDeleted
                                                                            && cond.lkpReportParameter.lkpControlType.CT_Code != emptyControlType
                                                                            ).ToList();
            Int32 columnNumber = AppConsts.FOUR;

            int currentColumnNumber = 0;

            HtmlGenericControl controlInColumn = null;

            foreach (FavParamReportParamMapping parameter in lstFavParamReportParamMapping)
            {
                //If the number of control generated = number of column 
                //per row then add to the form n create new row.
                if (currentColumnNumber == columnNumber)
                {
                    AddNextLineDiv(controlInColumn);
                    pnlParamValues.Controls.Add(controlInColumn);
                    currentColumnNumber = 0;
                }
                if (currentColumnNumber == 0)
                {
                    //Generate a new row
                    controlInColumn = GenerateColumnView(columnNumber);
                }
                controlInColumn = GenerateControl(parameter, controlInColumn);
                currentColumnNumber++;
            }
            if (currentColumnNumber != 0)
            {
                AddNextLineDiv(controlInColumn);
                pnlParamValues.Controls.Add(controlInColumn);
            }
        }

        /// <summary>
        /// Generate a new row
        /// </summary>
        /// <param name="columnNumber">Number of column per row</param>
        /// <returns></returns>
        private HtmlGenericControl GenerateColumnView(Int32 columnNumber)
        {
            HtmlGenericControl twoColumn = new HtmlGenericControl("div");
            twoColumn.Attributes.Add("class", "row");
            //twoColumn.Attributes.Add("class", "row" + Convert.ToString(columnNumber) + "co");
            //twoColumn.Attributes.Add("class", "col-md-12" + Convert.ToString(columnNumber) + "co");
            return twoColumn;

        }

        /// <summary>
        /// Add relevant space between tweo row.
        /// </summary>
        /// <param name="parentControl"></param>
        /// <returns>parentControl</returns>
        private HtmlGenericControl AddNextLineDiv(HtmlGenericControl parentControl)
        {
            String className = "";

            HtmlGenericControl nextLineDiv = new HtmlGenericControl("div");

            nextLineDiv.Attributes.Add("class", className);
            parentControl.Controls.Add(nextLineDiv);
            return parentControl;
        }

        /// <summary>
        /// Main function that creates a control nas per their data type.
        /// </summary>
        /// <param name="parameter">Attribute data to be created.</param>
        /// <param name="parentControl"></param>
        /// <returns></returns>
        private HtmlGenericControl GenerateControl(FavParamReportParamMapping parameter, HtmlGenericControl parentControl)
        {
            HtmlGenericControl mainDiv = new HtmlGenericControl("div");
            mainDiv.Attributes.Add("class", "form-group col-md-3");
            HtmlGenericControl lableDiv = CreateLabelForTheControl(parameter);
            mainDiv.Controls.Add(lableDiv);
            HtmlGenericControl controlDiv = CreateControlForTheForm(parameter);
            mainDiv.Controls.Add(controlDiv);
            parentControl.Controls.Add(mainDiv);
            return parentControl;
        }

        /// <summary>
        /// This method create lable for the control.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="currentInstanceId"></param>
        /// <returns></returns>
        private HtmlGenericControl CreateLabelForTheControl(FavParamReportParamMapping parameter)
        {
            HtmlGenericControl lableDiv = new HtmlGenericControl("div");
            //lableDiv.Attributes.Add("class", "form-group col-md-3");
            Label attributeLable = new Label();
            attributeLable.ID = "lbl_" + parameter.lkpReportParameter.lkpControlType.CT_Name.RemoveWhitespace() + "_" + parameter.lkpReportParameter.RP_ID.ToString();
            attributeLable.Text = parameter.lkpReportParameter.RP_Name;
            attributeLable.CssClass = "cptn";
            lableDiv.Controls.Add(attributeLable);
            if (parameter.lkpReportParameter.RP_IsRequired.HasValue && parameter.lkpReportParameter.RP_IsRequired.Value)
            {
                lableDiv.Controls.Add(AddRequiredSign(parameter.FPRPM_ID));
            }
            return lableDiv;
        }

        /// <summary>
        /// For the control that are required 
        /// is required sign is display.
        /// </summary>
        /// <returns></returns>
        private HtmlGenericControl AddRequiredSign(Int32 mappingID)
        {
            //<span class="reqd">*</span>
            HtmlGenericControl span = new HtmlGenericControl("span");
            //span.ID = groupId + "_" + InstanceId;
            span.Attributes.Add("class", "reqd" + " " + mappingID);
            span.InnerHtml = "*";
            return span;

        }

        /// <summary>
        /// Add a required field if required.
        /// </summary>
        /// <param name="controlTovalidate">Control on which the require  field is to be applied</param>
        /// <param name="AttributeType"></param>
        /// <returns></returns>
        private HtmlGenericControl SetrequiredFieldValidator(String controlTovalidate, FavParamReportParamMapping parameter)
        {
            HtmlGenericControl requiredFieldsDiv = new HtmlGenericControl("div");
            requiredFieldsDiv.Attributes.Add("class", "vldx");
            //groupId
            RequiredFieldValidator requiredField = new RequiredFieldValidator();
            requiredField.ID = "rfv_" + parameter.lkpReportParameter.lkpControlType.CT_Name.RemoveWhitespace() + "_" + parameter.lkpReportParameter.RP_ID.ToString();
            requiredField.ControlToValidate = controlTovalidate;
            requiredField.ErrorMessage = "Please enter " + parameter.lkpReportParameter.RP_Name;
            requiredField.ValidationGroup = "grpSave";
            requiredField.CssClass = "errmsg";
            requiredField.Enabled = true;
            requiredField.Display = ValidatorDisplay.Dynamic;// "Dynamic";
            if (parameter.lkpReportParameter.lkpControlType.CT_Code == ControlType.OPTION.GetStringValue())
            {
                if (parameter.lkpReportParameter.RP_Code != ReportParameters.INSTITUTE.GetStringValue()
                    || (parameter.lkpReportParameter.RP_Code == ReportParameters.INSTITUTE.GetStringValue() &&
                   (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ACCOUNTS_WITHOUT_ORDER_REPORT.GetStringValue()
                      && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.INVOICE_ORDER.GetStringValue())
                      && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ACCOUNT_WITHOUT_PURCHASE_REPORT.GetStringValue())
                      && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.INSTITUTION_COUNT.GetStringValue())  //UAT-3052
                      && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.REQUIREMENT_ITEM_DATA_REPORT.GetStringValue()) //UAT-3052
                      && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENT_DETAILS.GetStringValue())  //UAT-3052
                      && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENT_DETAILS_WITH_USERID.GetStringValue())  //UAT-3052
                      && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.AGENCY_ADMINS_BY_DEPARTMENT.GetStringValue())  //UAT-3052
                      && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ITEM_DATA_COUNT_REPORT.GetStringValue()) //UAT-3052
                       && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.CATEGORY_DATA_REPORT.GetStringValue())
                        && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENTS_COMPLIANCE_STATUS.GetStringValue())
                         && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENTS_OVERALL_NONCOMPLIANCE_STATUS.GetStringValue())
                         && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENTS_BY_DAY_OF_THE_WEEK.GetStringValue()) //UAT 3214
                    )))
                {
                    requiredField.InitialValue = "--Select--";
                }

            }
            requiredFieldsDiv.Controls.Add(requiredField);
            return requiredFieldsDiv;
        }

        /// <summary>
        /// This method set the value in control in edit mode.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="control"></param>
        /// <param name="attributesForCustomForm"></param>
        /// <returns></returns>
        private Control SetTheValueForControlInEditMode(Control control, FavParamReportParamMapping parameter)
        {
            switch (parameter.lkpReportParameter.lkpControlType.CT_Code)
            {
                case "AAAA":
                    WclTextBox textBox = control as WclTextBox;
                    textBox.Text = parameter.FPRPM_Value.IsNullOrEmpty() ? String.Empty : parameter.FPRPM_Value;
                    break;
                case "AAAB":
                    WclNumericTextBox numericTextBox = control as WclNumericTextBox;
                    numericTextBox.Text = parameter.FPRPM_Value.IsNullOrEmpty() ? String.Empty : parameter.FPRPM_Value;
                    break;
                case "AAAD":
                    WclComboBox combo = control as WclComboBox;
                    if (!parameter.FPRPM_Value.IsNullOrEmpty())
                    {
                        if (
                            (parameter.lkpReportParameter.RP_Code == ReportParameters.INSTITUTE.GetStringValue() &&
                            (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ACCOUNTS_WITHOUT_ORDER_REPORT.GetStringValue()
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.INVOICE_ORDER.GetStringValue())
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ACCOUNT_WITHOUT_PURCHASE_REPORT.GetStringValue())
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENT_DETAILS_WITH_USERID.GetStringValue()) //UAT:4623
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.INSTITUTION_COUNT.GetStringValue())  //UAT-3052
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.REQUIREMENT_ITEM_DATA_REPORT.GetStringValue())  //UAT-3052
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENT_DETAILS.GetStringValue())   //UAT-3052
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.AGENCY_ADMINS_BY_DEPARTMENT.GetStringValue()) //UAT-3052
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ITEM_DATA_COUNT_REPORT.GetStringValue())  //UAT-3052
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.CATEGORY_DATA_REPORT.GetStringValue())
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_OVERALL_NONCOMPLIANCE_STATUS.GetStringValue())
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_COMPLIANCE_STATUS.GetStringValue())
                            || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_BY_DAY_OF_THE_WEEK.GetStringValue()) //UAT 3214
                            )) 
                            || (parameter.lkpReportParameter.RP_Code == ReportParameters.CATEGORY.GetStringValue() && (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_OVERALL_NONCOMPLIANCE_STATUS.GetStringValue()))
                            || (parameter.lkpReportParameter.RP_Code == ReportParameters.USER_TYPE.GetStringValue() || parameter.lkpReportParameter.RP_Code == ReportParameters.REVIEW_STATUS.GetStringValue())
                            )
                        {
                            List<String> values = parameter.FPRPM_Value.Split(',').ToList();
                            foreach (RadComboBoxItem item in combo.Items)
                            {
                                if (values.Contains(item.Value))
                                {
                                    item.Checked = true;
                                }
                            }
                        }
                        else
                        {
                            combo.SelectedValue = parameter.FPRPM_Value;
                        }
                    }
                    else
                    {
                        combo.SelectedIndex = AppConsts.NONE;
                    }
                    break;
                case "AAAE":
                case "AAAC":
                    WclComboBox comboMulti = control as WclComboBox;
                    if (!parameter.FPRPM_Value.IsNullOrEmpty())
                    {
                        List<String> values = parameter.FPRPM_Value.Split(',').ToList();
                        foreach (RadComboBoxItem item in comboMulti.Items)
                        {
                            if (values.Contains(item.Value))
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    break;
                case "AAAF":
                    WclDatePicker datePicker = control as WclDatePicker;
                    datePicker.SelectedDate = parameter.FPRPM_Value.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(parameter.FPRPM_Value).Date;
                    break;
                case "AAAH":
                    RadioButtonList radioButtonList = control as RadioButtonList;
                    radioButtonList.SelectedValue = parameter.FPRPM_Value.IsNullOrEmpty() ? String.Empty : Convert.ToString(parameter.FPRPM_Value);
                    break;
                default:
                    break;
            }
            return control;
        }

        /// <summary>
        /// this method creates control corresponding to different data type.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="currentInstanceId"></param>
        /// <returns></returns>
        private HtmlGenericControl CreateControlForTheForm(FavParamReportParamMapping parameter)
        {
            HtmlGenericControl controlDiv = new HtmlGenericControl("div");
            //  controlDiv.Attributes.Add("class", "");
            switch (parameter.lkpReportParameter.lkpControlType.CT_Code)
            {
                case "AAAA":
                    WclTextBox textBox = new WclTextBox();
                    textBox.ID = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                    textBox.Style.Add("display", "block");
                    textBox.CssClass = "form-control";
                    textBox.Width = new Unit("100%");

                    var dataSet = SetTheValueForControlInEditMode(textBox, parameter);
                    textBox = dataSet as WclTextBox;

                    controlDiv.Controls.Add(textBox);
                    if (parameter.lkpReportParameter.RP_IsRequired.HasValue && parameter.lkpReportParameter.RP_IsRequired.Value)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(textBox.ID, parameter));
                    }

                    break;
                case "AAAB":
                    WclNumericTextBox txtNumeric = new WclNumericTextBox();
                    txtNumeric.ID = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                    txtNumeric.Style.Add("display", "block");
                    txtNumeric.CssClass = "form-control";
                    txtNumeric.Width = new Unit("100%");
                    var dataSetNumeric = SetTheValueForControlInEditMode(txtNumeric, parameter);
                    txtNumeric = dataSetNumeric as WclNumericTextBox;
                    controlDiv.Controls.Add(txtNumeric);
                    if (parameter.lkpReportParameter.RP_IsRequired.HasValue && parameter.lkpReportParameter.RP_IsRequired.Value)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(txtNumeric.ID, parameter));
                    }
                    break;
                case "AAAD":
                    WclComboBox dropDownList = new WclComboBox();
                    dropDownList.ID = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                    dropDownList.Style.Add("display", "block");
                    dropDownList.CssClass = "form-control";
                    dropDownList.Width = new Unit("100%");
                    dropDownList.AutoSkinMode = false;
                    dropDownList.Skin = "Silk";
                    //dropDownList.EmptyMessage = "--Select--";
                    if ((parameter.lkpReportParameter.RP_Code == ReportParameters.INSTITUTE.GetStringValue()
                        && (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ACCOUNTS_WITHOUT_ORDER_REPORT.GetStringValue()
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.INVOICE_ORDER.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ACCOUNT_WITHOUT_PURCHASE_REPORT.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENT_DETAILS_WITH_USERID.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.INSTITUTION_COUNT.GetStringValue())  //UAT-3052
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.REQUIREMENT_ITEM_DATA_REPORT.GetStringValue())  //UAT-3052
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENT_DETAILS.GetStringValue())   //UAT-3052
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.AGENCY_ADMINS_BY_DEPARTMENT.GetStringValue()) //UAT-3052
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ITEM_DATA_COUNT_REPORT.GetStringValue())  //UAT-3052
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.CATEGORY_DATA_REPORT.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_COMPLIANCE_STATUS.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_OVERALL_NONCOMPLIANCE_STATUS.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_BY_DAY_OF_THE_WEEK.GetStringValue()) //UAT 3214
                        ))
                        || (parameter.lkpReportParameter.RP_Code == ReportParameters.USER_TYPE.GetStringValue() || parameter.lkpReportParameter.RP_Code == ReportParameters.REVIEW_STATUS.GetStringValue())
                        )
                    {
                        dropDownList.EnableCheckAllItemsCheckBox = true;
                        dropDownList.CheckBoxes = true;
                    }
                    var combo = BindDropDown(dropDownList, parameter);
                    var dataSetCombo = SetTheValueForControlInEditMode(combo, parameter);
                    dropDownList = dataSetCombo as WclComboBox;

                    controlDiv.Controls.Add(dropDownList);
                    if (parameter.lkpReportParameter.RP_IsRequired.HasValue && parameter.lkpReportParameter.RP_IsRequired.Value)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownList.ID, parameter));
                    }
                    break;
                case "AAAE":
                case "AAAC":
                    WclComboBox multiComboBox = new WclComboBox();
                    multiComboBox.ID = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                    multiComboBox.Style.Add("display", "block");
                    if (parameter.lkpReportParameter.lkpControlType.CT_Code == ControlType.HIERARCHY.GetStringValue())
                    {
                        multiComboBox.DropDownAutoWidth = RadComboBoxDropDownAutoWidth.Enabled;
                    }
                    multiComboBox.EmptyMessage = "--Select--";
                    multiComboBox.CheckBoxes = true;
                    multiComboBox.EnableCheckAllItemsCheckBox = true;
                    multiComboBox.AutoSkinMode = false;
                    multiComboBox.Width = new Unit("100%");
                    multiComboBox.CssClass = "form-control";
                    multiComboBox.Skin = "Silk";
                    var comboMulti = BindDropDown(multiComboBox, parameter);
                    var dataSetMultiCombo = SetTheValueForControlInEditMode(multiComboBox, parameter);
                    multiComboBox = multiComboBox as WclComboBox;

                    controlDiv.Controls.Add(multiComboBox);
                    if (parameter.lkpReportParameter.RP_IsRequired.HasValue && parameter.lkpReportParameter.RP_IsRequired.Value)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(multiComboBox.ID, parameter));
                    }
                    break;
                case "AAAF":
                    WclDatePicker dPicker = new WclDatePicker();
                    dPicker.DateInput.DateFormat = "MM-dd-yyyy";
                    dPicker.ID = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                    dPicker.DateInput.EmptyMessage = "Select a date";
                    dPicker.Style.Add("display", "block");
                    dPicker.CssClass = "form-control";
                    dPicker.Width = new Unit("100%");
                    var dataSetDate = SetTheValueForControlInEditMode(dPicker, parameter);
                    dPicker = dataSetDate as WclDatePicker;

                    controlDiv.Controls.Add(dPicker);
                    if (parameter.lkpReportParameter.RP_IsRequired.HasValue && parameter.lkpReportParameter.RP_IsRequired.Value)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(dPicker.ID, parameter));
                    }
                    break;
                case "AAAH":
                    RadioButtonList radioButtonList = new RadioButtonList();
                    radioButtonList.ID = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                    radioButtonList.Style.Add("display", "block");
                    radioButtonList.CssClass = "form-control";
                    radioButtonList.Width = new Unit("100%");
                    radioButtonList.RepeatDirection = RepeatDirection.Horizontal;
                    var rbList = BindRadioButtonList(radioButtonList);
                    radioButtonList = rbList as RadioButtonList;
                    var SetValue = SetTheValueForControlInEditMode(radioButtonList, parameter);
                    radioButtonList = SetValue as RadioButtonList;
                    controlDiv.Controls.Add(radioButtonList);
                    if (parameter.lkpReportParameter.RP_IsRequired.HasValue && parameter.lkpReportParameter.RP_IsRequired.Value)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(radioButtonList.ID, parameter));
                    }
                    break;
                default:

                    break;
            }
            return controlDiv;
        }
        //UAT-3052
        private Control BindRadioButtonList(RadioButtonList rbl)
        {
            var control = rbl as RadioButtonList;
            control.Items.Add(new ListItem("True", "True"));
            control.Items.Add(new ListItem("False", "False"));
            return control;
        }

        private Control BindDropDown(WclComboBox cmb, FavParamReportParamMapping parameter)
        {
            switch (parameter.lkpReportParameter.RP_Code)
            {
                case "Institute":
                    BindInstitute(cmb, parameter);
                    cmb.AutoPostBack = true;
                    cmb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbInstitution_SelectedIndexChanged);
                    break;

                case "OrderType":
                    BindOrderType(cmb);
                    break;

                case "SubscriptionArchiveState":
                    BindSubscriptionArchiveState(cmb, CurrentViewContext.SelectedTenantID);
                    break;

                case "OverallStatus":
                    BindOverallStatus(cmb, CurrentViewContext.SelectedTenantID);
                    break;

                case "CategoryStatus":
                    BindCategoryStatus(cmb, CurrentViewContext.SelectedTenantID);
                    break;

                case "UserGroup":
                    BindUserGroup(cmb, CurrentViewContext.SelectedTenantID);
                    break;

                case "Category":
                    BindCategory(cmb, CurrentViewContext.SelectedTenantIDs, CurrentViewContext.SelectedNodeIds, parameter);
                    cmb.AutoPostBack = true;
                    cmb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbCategory_SelectedIndexChanged);
                    break;

                case "Item":
                    BindItem(cmb, CurrentViewContext.SelectedTenantIDs, CurrentViewContext.SelectedCategoryIds, parameter);
                    break;

                case "Hierarchy":
                case "Node":

                    BindHierarchy(cmb, CurrentViewContext.SelectedTenantID);
                    cmb.AutoPostBack = true;
                    cmb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbHierarchy_SelectedIndexChanged);
                    break;

                case "Agency":  //UAT-3052
                    BindAgency(cmb, CurrentViewContext.SelectedTenantIDs, parameter);
                    break;
                case "Rotation":
                    BindRotation(cmb, CurrentViewContext.SelectedTenantIDs, parameter);
                    cmb.AutoPostBack = true;
                    cmb.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbHierarchy_SelectedIndexChanged);
                    break;
                case "AgencyHierarchy":
                    BindAgencyHierarchy(cmb, CurrentViewContext.SelectedTenantIDs, parameter);
                    break;
                case "WeekDays":
                    BindWeekDays(cmb, parameter);
                    break;
                case "SubscriptionInactiveState": //UAT-4309
                    BindSubscriptionArchiveStateForNonCompliantReport(cmb, CurrentViewContext.SelectedTenantID);
                    break;
                case "ComplianceStatus": //UAT-4263
                    BindComplianceStatus(cmb,  CurrentViewContext.SelectedTenantID, parameter);
                    break;
                case "UserType":
                    BindUserTypes(cmb);
                    break;
                case "ReviewStatus":
                    BindReviewStatus(cmb);
                    break;
                default:
                    break;

            }
            return cmb;
        }

        private void BindWeekDays(WclComboBox cmb, FavParamReportParamMapping parameter)
        {
            Dictionary<String, String> dicWeekDays = Presenter.GetWeekDaysList();
            cmb.DataTextField = "Value";
            cmb.DataValueField = "Key";
            cmb.DataSource = dicWeekDays;
            cmb.DataBind();
        }

        private void BindRotation(WclComboBox cmb, String SelectedTenantIDs, FavParamReportParamMapping parameter)
        {
            String loggedInUserEmailId = String.Empty;
            FavParamReportParamMapping favParam = new FavParamReportParamMapping();

            if (!parameter.ReportFavouriteParameter.IsNullOrEmpty())
                favParam = parameter.ReportFavouriteParameter.FavParamReportParamMappings.Where(cond => cond.lkpReportParameter.RP_Code == ReportParameters.LOGGEDIN_USER_EMAILID.GetStringValue()
                    && !cond.FPRPM_IsDeleted
                    && !cond.lkpReportParameter.RP_IsDeleted).FirstOrDefault();
            else
                favParam = new FavParamReportParamMapping();

            if (!favParam.IsNullOrEmpty())
            {
                loggedInUserEmailId = favParam.FPRPM_Value;
            }

            Dictionary<String, String> dicRotation = Presenter.GetRotationListFilterForLoggedInAgencyUserReports(SelectedTenantIDs, loggedInUserEmailId);
            cmb.DataTextField = "Value";
            cmb.DataValueField = "Key";
            cmb.DataSource = dicRotation;
            cmb.DataBind();
        }

        private void BindInstitute(WclComboBox cmb, FavParamReportParamMapping parameter)
        {
            cmb.DataTextField = "TenantName";
            cmb.DataValueField = "TenantID";

            String selectedTenantIds = String.Empty;
            List<Entity.ClientEntity.Tenant> lstTenants = new List<Entity.ClientEntity.Tenant>();

            if ((parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.INSTITUTION_COUNT.GetStringValue())  //UAT-3052
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.REQUIREMENT_ITEM_DATA_REPORT.GetStringValue())  //UAT-3052
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENT_DETAILS.GetStringValue())   //UAT-3052
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENT_DETAILS_WITH_USERID.GetStringValue())   //UAT-3052
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.AGENCY_ADMINS_BY_DEPARTMENT.GetStringValue()) //UAT-3052
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ITEM_DATA_COUNT_REPORT.GetStringValue())
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.CATEGORY_DATA_REPORT.GetStringValue())
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENTS_COMPLIANCE_STATUS.GetStringValue())
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENTS_OVERALL_NONCOMPLIANCE_STATUS.GetStringValue())
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENTS_BY_DAY_OF_THE_WEEK.GetStringValue()) //UAT 3214
                )
            {
                lstTenants = Presenter.GetTenants();
            }
            else
            {
                List<FavParamReportParamMapping> lstFavParam = new List<FavParamReportParamMapping>();
                if (!parameter.ReportFavouriteParameter.IsNullOrEmpty())
                    lstFavParam = parameter.ReportFavouriteParameter.FavParamReportParamMappings.ToList();
                else
                    lstFavParam = new List<FavParamReportParamMapping>();

                foreach (var favparm in lstFavParam)
                {
                    if (favparm.lkpReportParameter.RP_Code == ReportParameters.TENANT_ID.GetStringValue())
                        selectedTenantIds = favparm.FPRPM_Value;
                }
                lstTenants = Presenter.GetTenantsByTenantId(selectedTenantIds.Replace('~', ','));
            }

            if (lstTenants.IsNull())
            {
                cmb.DataSource = new List<Entity.ClientEntity.Tenant>();
            }
            else
            {
                cmb.DataSource = lstTenants;
            }
            cmb.DataBind();
            if (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ACCOUNTS_WITHOUT_ORDER_REPORT.GetStringValue()
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.INVOICE_ORDER.GetStringValue())
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ACCOUNT_WITHOUT_PURCHASE_REPORT.GetStringValue())
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.INSTITUTION_COUNT.GetStringValue())  //UAT-3052
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.REQUIREMENT_ITEM_DATA_REPORT.GetStringValue())  //UAT-3052
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENT_DETAILS.GetStringValue())   //UAT-3052
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENT_DETAILS_WITH_USERID.GetStringValue()) 
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.AGENCY_ADMINS_BY_DEPARTMENT.GetStringValue()) //UAT-3052
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ITEM_DATA_COUNT_REPORT.GetStringValue())  //UAT-3052
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.CATEGORY_DATA_REPORT.GetStringValue())
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENTS_COMPLIANCE_STATUS.GetStringValue())
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENTS_OVERALL_NONCOMPLIANCE_STATUS.GetStringValue())
                && (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENTS_BY_DAY_OF_THE_WEEK.GetStringValue()) //UAT 3214
                )
            {
                cmb.Items.Insert(0, new RadComboBoxItem("--Select--"));
            }
        }

        private static void BindOrderType(WclComboBox cmb)
        {
            Dictionary<String, Int32> dicOrderType = new Dictionary<String, Int32>();
            dicOrderType.Add("Tracking", AppConsts.ONE);
            dicOrderType.Add("Screening", AppConsts.TWO);
            dicOrderType.Add("Both", AppConsts.THREE);
            cmb.DataTextField = "Key";
            cmb.DataValueField = "Value";
            cmb.DataSource = dicOrderType;
            cmb.DataBind();
            cmb.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        private void BindHierarchy(WclComboBox cmb, Int32 selectedTenantID)
        {
            Dictionary<Int32, String> dicHierarchyFilter = Presenter.GetHierarchyListFilterForReport(selectedTenantID, CurrentViewContext.SelectedUserID);
            cmb.DataTextField = "Value";
            cmb.DataValueField = "Key";
            cmb.DataSource = dicHierarchyFilter;
            cmb.DataBind();
        }

        private void BindCategory(WclComboBox cmb, String selectedTenantID, String selectedNodeIds, FavParamReportParamMapping parameter)
        {
            if (parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.CATEGORY_DATA_REPORT.GetStringValue()
                && parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ROTATION_STUDENTS_OVERALL_NONCOMPLIANCE_STATUS.GetStringValue()
                && parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.CATEGORY_DATA_REPORT_WITH_COMPLIO_ID.GetStringValue()
                && parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ITEM_DATA_COUNT_REPORT.GetStringValue())
            {
                Dictionary<Int32, String> dicCategory = Presenter.GetCategoryListFilterForReport(Convert.ToInt32(selectedTenantID), selectedNodeIds);
                cmb.DataTextField = "Value";
                cmb.DataValueField = "Key";
                cmb.DataSource = dicCategory;
                cmb.DataBind();
            }
            //Get categories for both student and instructor.[UAT-4509]
            else if (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ITEM_DATA_COUNT_REPORT.GetStringValue())
            {
                String loggedInUserEmailId = String.Empty;
                FavParamReportParamMapping favParam = new FavParamReportParamMapping();

                if (!parameter.ReportFavouriteParameter.IsNullOrEmpty())
                    favParam = parameter.ReportFavouriteParameter.FavParamReportParamMappings.Where(cond => cond.lkpReportParameter.RP_Code == ReportParameters.LOGGEDIN_USER_EMAILID.GetStringValue()
                        && !cond.FPRPM_IsDeleted
                        && !cond.lkpReportParameter.RP_IsDeleted).FirstOrDefault();
                else
                    favParam = new FavParamReportParamMapping();

                if (!favParam.IsNullOrEmpty())
                {
                    loggedInUserEmailId = favParam.FPRPM_Value;
                }

                Dictionary<String, String> dicCategory = Presenter.GetAllCategoryListFilterForLoggedInAgencyUserReports(selectedTenantID, loggedInUserEmailId);
                cmb.DataTextField = "Value";
                cmb.DataValueField = "Key";
                cmb.DataSource = dicCategory;
                cmb.DataBind();
            }
            else
            {
                String loggedInUserEmailId = String.Empty;
                FavParamReportParamMapping favParam = new FavParamReportParamMapping();

                if (!parameter.ReportFavouriteParameter.IsNullOrEmpty())
                    favParam = parameter.ReportFavouriteParameter.FavParamReportParamMappings.Where(cond => cond.lkpReportParameter.RP_Code == ReportParameters.LOGGEDIN_USER_EMAILID.GetStringValue()
                        && !cond.FPRPM_IsDeleted
                        && !cond.lkpReportParameter.RP_IsDeleted).FirstOrDefault();
                else
                    favParam = new FavParamReportParamMapping();

                if (!favParam.IsNullOrEmpty())
                {
                    loggedInUserEmailId = favParam.FPRPM_Value;
                }

                Dictionary<String, String> dicCategory = Presenter.GetCategoryListFilterForLoggedInAgencyUserReports(selectedTenantID, loggedInUserEmailId);
                cmb.DataTextField = "Value";
                cmb.DataValueField = "Key";
                cmb.DataSource = dicCategory;
                cmb.DataBind();
            }
        }

        private void BindItem(WclComboBox cmb, String selectedTenantID, String selectedCategoryIds, FavParamReportParamMapping parameter)
        {
            if (!parameter.ReportFavouriteParameter.IsNullOrEmpty()
                && parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.ITEM_DATA_COUNT_REPORT.GetStringValue()
                && parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.REQUIREMENT_ITEM_DATA_REPORT.GetStringValue()
                && parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.CATEGORY_DATA_REPORT.GetStringValue()
                && parameter.ReportFavouriteParameter.Report.RP_Code != ReportType.CATEGORY_DATA_REPORT_WITH_COMPLIO_ID.GetStringValue())
            {
                Dictionary<Int32, String> dicItem = Presenter.GetItemListFilterForReport(Convert.ToInt32(selectedTenantID), selectedCategoryIds);
                cmb.DataTextField = "Value";
                cmb.DataValueField = "Key";
                cmb.DataSource = dicItem;
                cmb.DataBind();
            }
            //Get Item for both student and instructor.[UAT-4509]
            else if (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ITEM_DATA_COUNT_REPORT.GetStringValue())
            {
                String selectedTenantIds = String.Empty;
                String loggedInEmailID = String.Empty;
                List<FavParamReportParamMapping> lstFavParam = new List<FavParamReportParamMapping>();
                if (!parameter.ReportFavouriteParameter.IsNullOrEmpty())
                    lstFavParam = parameter.ReportFavouriteParameter.FavParamReportParamMappings.ToList();
                else
                    lstFavParam = new List<FavParamReportParamMapping>();

                foreach (var favparm in lstFavParam)
                {
                    if (favparm.lkpReportParameter.RP_Code == ReportParameters.LOGGEDIN_USER_EMAILID.GetStringValue())
                        loggedInEmailID = favparm.FPRPM_Value;                    
                }

                Dictionary<String, String> dicItem = Presenter.GetAllItemListForReportsByTenantIdLoggedInEmail(selectedTenantID, loggedInEmailID);
                cmb.DataTextField = "Value";
                cmb.DataValueField = "Key";
                cmb.DataSource = dicItem;
                cmb.DataBind();
            }
            else //UAT-3052
            {
                String selectedTenantIds = String.Empty;
                String loggedInEmailID = String.Empty;
                List<FavParamReportParamMapping> lstFavParam = new List<FavParamReportParamMapping>();


                if (!parameter.ReportFavouriteParameter.IsNullOrEmpty())
                    lstFavParam = parameter.ReportFavouriteParameter.FavParamReportParamMappings.ToList();
                else
                    lstFavParam = new List<FavParamReportParamMapping>();

                foreach (var favparm in lstFavParam)
                {
                    if (favparm.lkpReportParameter.RP_Code == ReportParameters.LOGGEDIN_USER_EMAILID.GetStringValue())
                        loggedInEmailID = favparm.FPRPM_Value;
                    // else if (favparm.lkpReportParameter.RP_Code == ReportParameters.INSTITUTE.GetStringValue())
                    // selectedTenantIds = favparm.FPRPM_Value;
                }
                Dictionary<String, String> dicItem = Presenter.GetItemListForReportsByTenantIdLoggedInEmail(selectedTenantID, loggedInEmailID);
                cmb.DataTextField = "Value";
                cmb.DataValueField = "Key";
                cmb.DataSource = dicItem;
                cmb.DataBind();
            }
        }

        private void BindUserGroup(WclComboBox cmb, Int32 selectedTenantID)
        {
            Dictionary<Int32, String> dicUserGrp = Presenter.GetUserGroupListFilterForReport(selectedTenantID);
            cmb.DataTextField = "Value";
            cmb.DataValueField = "Key";
            cmb.DataSource = dicUserGrp;
            cmb.DataBind();
        }

        private void BindCategoryStatus(WclComboBox cmb, Int32 selectedTenantID)
        {
            List<lkpCategoryComplianceStatu> lstCategoryComplianceStatus = Presenter.GetCategoryComplianceStatus(selectedTenantID);
            cmb.DataTextField = "Name";
            cmb.DataValueField = "CategoryComplianceStatusID";
            cmb.DataSource = lstCategoryComplianceStatus;
            cmb.DataBind();
            if (!lstCategoryComplianceStatus.IsNullOrEmpty())
            {
                cmb.Items.Insert(0, new RadComboBoxItem("All", null));
            }
        }

        private void BindOverallStatus(WclComboBox cmb, Int32 selectedTenantID)
        {
            List<lkpPackageComplianceStatu> lstPkgComplianceStatus = Presenter.GetPackageComplianceStatus(selectedTenantID);
            cmb.DataTextField = "Name";
            cmb.DataValueField = "PackageComplianceStatusID";
            cmb.DataSource = lstPkgComplianceStatus;
            cmb.DataBind();
            if (!lstPkgComplianceStatus.IsNullOrEmpty())
            {
                cmb.Items.Insert(0, new RadComboBoxItem("All", null));
            }
        }

        private void BindSubscriptionArchiveState(WclComboBox cmb, Int32 selectedTenantID)
        {
            List<lkpArchiveState> lstArchiveState = Presenter.GetArchiveStateList(selectedTenantID);
            cmb.DataTextField = "AS_Name";
            cmb.DataValueField = "AS_Code";
            cmb.DataSource = lstArchiveState;
            cmb.DataBind();
        }

        private void BindSubscriptionArchiveStateForNonCompliantReport(WclComboBox cmb, Int32 selectedTenantID) //UAT-4309
        {
            List<lkpArchiveState> lstArchiveState = Presenter.GetArchiveStateListForNonCompliantReport(selectedTenantID);
            cmb.DataTextField = "AS_Name";
            cmb.DataValueField = "AS_Code";
            cmb.DataSource = lstArchiveState;
            cmb.DataBind();
        }

        #region UAT-3052
        private void BindAgency(WclComboBox cmb, String selectedTenantID, FavParamReportParamMapping parameter)
        {
            String loggedInEmailID = String.Empty;
            List<Entity.SharedDataEntity.Agency> lstAgency = new List<Entity.SharedDataEntity.Agency>();
            if (!parameter.ReportFavouriteParameter.IsNullOrEmpty())
            {
                loggedInEmailID = parameter.ReportFavouriteParameter.FavParamReportParamMappings.Where(a => a.lkpReportParameter.RP_Code == ReportParameters.LOGGEDIN_USER_EMAILID.GetStringValue()).Select(a => a.FPRPM_Value).FirstOrDefault();

                if (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.AGENCY_ADMINS_BY_DEPARTMENT.GetStringValue())
                    lstAgency = Presenter.GetAgencyHierarchy(loggedInEmailID);
                else
                    lstAgency = Presenter.GetAgencyUsers(loggedInEmailID);
            }
            if (lstAgency.IsNull())
                cmb.DataSource = new List<Entity.SharedDataEntity.Agency>();
            else
                cmb.DataSource = lstAgency;

            cmb.DataTextField = "AG_Name";
            cmb.DataValueField = "AG_ID";
            cmb.DataBind();
        }

        private void BindAgencyHierarchy(WclComboBox cmb, String selectedTenantID, FavParamReportParamMapping parameter)
        {
            String loggedInEmailID = String.Empty;
            List<Entity.SharedDataEntity.Agency> lstAgencyHierarchy = new List<Entity.SharedDataEntity.Agency>();
            if (!parameter.ReportFavouriteParameter.IsNullOrEmpty())
            {
                loggedInEmailID = parameter.ReportFavouriteParameter.FavParamReportParamMappings.Where(a => a.lkpReportParameter.RP_Code == ReportParameters.LOGGEDIN_USER_EMAILID.GetStringValue()).Select(a => a.FPRPM_Value).FirstOrDefault();
                lstAgencyHierarchy = Presenter.GetAgencyHierarchy(loggedInEmailID);

            }
            if (lstAgencyHierarchy.IsNull())
                cmb.DataSource = new List<Entity.SharedDataEntity.Agency>();
            else
                cmb.DataSource = lstAgencyHierarchy;

            cmb.DataTextField = "AG_Name";
            cmb.DataValueField = "AG_ID";
            cmb.DataBind();
        }

        private void BindComplianceStatus(WclComboBox cmb, Int32 selectedTenantID, FavParamReportParamMapping parameter)  //UAT-4263
        {
            List<lkpPackageComplianceStatu> lstPkgComplianceStatus = Presenter.GetPackageComplianceStatus(selectedTenantID);            
            cmb.DataTextField = "Name";
            cmb.DataValueField = "PackageComplianceStatusID";
            cmb.DataSource = lstPkgComplianceStatus;
            cmb.DataBind();
        }

        private void BindUserTypes(WclComboBox cmb)
        {
            Dictionary<String, String> userTypes = Presenter.GetUserTypesForReports();
            cmb.DataTextField = "Value";
            cmb.DataValueField = "Key";
            cmb.DataSource = userTypes;
            cmb.DataBind();
        }

        private void BindReviewStatus(WclComboBox cmb)
        {
            Dictionary<String, String> reviewStatus = Presenter.GetInvitationReviewStatus();
            cmb.DataTextField = "Value";
            cmb.DataValueField = "Key";
            cmb.DataSource = reviewStatus;
            cmb.DataBind();
        }
        #endregion

        protected void cmbInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {

                //iframeReportViewer.Src = String.Empty;
                WclComboBox cmbInstitution = sender as WclComboBox;
                String selectedTenantID;
                if (cmbInstitution.SelectedValue.IsNullOrEmpty())
                {
                    selectedTenantID = "0";
                }
                else
                {
                    selectedTenantID = Convert.ToString(cmbInstitution.SelectedValue);
                }

                if (hdnSelectedInstitutionID.Value == selectedTenantID.ToString())
                {
                    return;
                }
                hdnSelectedInstitutionID.Value = Convert.ToString(selectedTenantID);
                hdnSelectedNodeIDs.Value = String.Empty;
                hdnSelectedCategoryIDs.Value = String.Empty;
                divReport.Visible = false;
                String emptyControlType = ControlType.EMPTY.GetStringValue();
                List<FavParamReportParamMapping> lstFavParamReportParamMapping = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                                                                .Where(cond => !cond.FPRPM_IsDeleted
                                                                                && cond.lkpReportParameter.lkpControlType.CT_Code != emptyControlType
                                                                                ).ToList();
                foreach (FavParamReportParamMapping parameter in lstFavParamReportParamMapping)
                {
                    String controlIDToFind = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                    Control control = (pnlParamValues as Control).FindServerControlRecursively(controlIDToFind);
                    if (!control.IsNullOrEmpty())
                    {
                        switch (parameter.lkpReportParameter.RP_Code)
                        {
                            case "SubscriptionArchiveState":
                                WclComboBox cmbSubscriptionArchiveState = control as WclComboBox;
                                cmbSubscriptionArchiveState.ClearSelection();
                                BindSubscriptionArchiveState(cmbSubscriptionArchiveState, Convert.ToInt32(selectedTenantID));
                                break;

                            case "OverallStatus":
                                WclComboBox cmbOverallStatus = control as WclComboBox;
                                cmbOverallStatus.ClearSelection();
                                BindOverallStatus(cmbOverallStatus, Convert.ToInt32(selectedTenantID));
                                break;

                            case "CategoryStatus":
                                WclComboBox cmbCategoryStatus = control as WclComboBox;
                                cmbCategoryStatus.ClearSelection();
                                BindCategoryStatus(cmbCategoryStatus, Convert.ToInt32(selectedTenantID));
                                break;

                            case "UserGroup":
                                WclComboBox cmbUserGroup = control as WclComboBox;
                                cmbUserGroup.ClearSelection();
                                BindUserGroup(cmbUserGroup, Convert.ToInt32(selectedTenantID));
                                break;

                            case "Category":
                                WclComboBox cmbCategory = control as WclComboBox;
                                cmbCategory.ClearSelection();
                                BindCategory(cmbCategory, selectedTenantID, String.Empty, parameter);
                                break;

                            case "Item":
                                WclComboBox cmbItem = control as WclComboBox;
                                cmbItem.ClearSelection();
                                BindItem(cmbItem, selectedTenantID, AppConsts.ZERO, parameter);
                                break;

                            case "Hierarchy":
                            case "Node":
                                WclComboBox cmbHierarchy = control as WclComboBox;
                                cmbHierarchy.ClearSelection();
                                BindHierarchy(cmbHierarchy, Convert.ToInt32(selectedTenantID));
                                break;
                            case "Agency":
                                WclComboBox cmbAgency = control as WclComboBox;
                                cmbAgency.ClearSelection();
                                BindAgency(cmbAgency, selectedTenantID, parameter);
                                break;
                            case "Rotation":
                                WclComboBox cmbRotation = control as WclComboBox;
                                cmbRotation.ClearSelection();
                                BindRotation(cmbRotation, selectedTenantID, parameter);
                                break;
                            case "AgencyHierarchy":
                                WclComboBox cmbAgencyHierarchy = control as WclComboBox;
                                cmbAgencyHierarchy.ClearSelection();
                                BindAgencyHierarchy(cmbAgencyHierarchy, selectedTenantID, parameter);
                                break;
                            case "SubscriptionInactiveState":   //UAT-4309                             
                                WclComboBox cmbSubscriptionArchiveStateForNonCompliantReport = control as WclComboBox;
                                cmbSubscriptionArchiveStateForNonCompliantReport.ClearSelection();
                                BindSubscriptionArchiveStateForNonCompliantReport(cmbSubscriptionArchiveStateForNonCompliantReport, Convert.ToInt32(selectedTenantID));
                                break;
                            case "ComplianceStatus": //UAT-4263
                                WclComboBox cmbComplianceStatus = control as WclComboBox;
                                cmbComplianceStatus.ClearSelection();
                                BindComplianceStatus(cmbComplianceStatus, Convert.ToInt32(selectedTenantID),parameter);
                                break;

                        }
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


        /*   protected void cmbHierarchy_SelectedIndexChanged(object sender, EventArgs e)
           {
               try
               {
                   divReport.Visible = false;
                   iframeReportViewer.Src = String.Empty;
                   WclComboBox cmbHierarchy = sender as WclComboBox;
                   String selectedNodeIds;
                   if (cmbHierarchy.CheckedItems.IsNullOrEmpty())
                   {
                       selectedNodeIds = String.Empty;
                   }
                   else
                   {
                       selectedNodeIds = String.Join(",", cmbHierarchy.CheckedItems.Select(cond => cond.Value));
                   }

                   String categoryParameterType = ReportParameters.CATEGORY.GetStringValue();

                   FavParamReportParamMapping favParamReportParamMapping_Category = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                                                                   .Where(cond => !cond.FPRPM_IsDeleted
                                                                                   && cond.lkpReportParameter.RP_Code == categoryParameterType
                                                                                   ).FirstOrDefault();
                   if (!favParamReportParamMapping_Category.IsNullOrEmpty())
                   {
                       String categoryControlIDToFind = favParamReportParamMapping_Category.lkpReportParameter.RP_Code + "_" + favParamReportParamMapping_Category.FPRPM_ID;
                       Control categoryControl = (pnlParamValues as Control).FindServerControlRecursively(categoryControlIDToFind);
                       if (!categoryControl.IsNullOrEmpty())
                       {
                           String institutionParameterCode = ReportParameters.INSTITUTE.GetStringValue();
                           FavParamReportParamMapping favParamReportParamMapping_Institution = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                                                                               .Where(cond => cond.lkpReportParameter.RP_Code == institutionParameterCode)
                                                                                                   .FirstOrDefault();
                           if (!favParamReportParamMapping_Institution.IsNullOrEmpty())
                           {
                               String institutionControlIDToFind = favParamReportParamMapping_Institution.lkpReportParameter.RP_Code
                                                                                           + "_" + favParamReportParamMapping_Institution.FPRPM_ID;
                               Control institutionControl = (pnlParamValues as Control).FindServerControlRecursively(institutionControlIDToFind);
                               Int32 selectedTenantID;
                               if (!institutionControl.IsNullOrEmpty())
                               {
                                   WclComboBox cmbInstitution = institutionControl as WclComboBox;
                                   selectedTenantID = Convert.ToInt32(cmbInstitution.SelectedValue);
                               }
                               else
                               {
                                   selectedTenantID = AppConsts.NONE;
                               }
                               WclComboBox cmbCategory = categoryControl as WclComboBox;
                               cmbCategory.ClearSelection();
                               BindCategory(cmbCategory, selectedTenantID, selectedNodeIds);
                           }
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
          * 
          * */

        protected void cmbHierarchy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                //iframeReportViewer.Src = String.Empty;
                WclComboBox cmbHierarchy = sender as WclComboBox;
                String selectedNodeIds;
                if (cmbHierarchy.CheckedItems.IsNullOrEmpty())
                {
                    selectedNodeIds = String.Empty;
                }
                else
                {
                    selectedNodeIds = String.Join(",", cmbHierarchy.CheckedItems.Select(cond => cond.Value));
                }

                if (selectedNodeIds == hdnSelectedNodeIDs.Value)
                {
                    return;
                }

                hdnSelectedNodeIDs.Value = selectedNodeIds;
                hdnSelectedCategoryIDs.Value = String.Empty;

                divReport.Visible = false;
                String emptyControlType = ControlType.EMPTY.GetStringValue();
                List<FavParamReportParamMapping> lstFavParamReportParamMapping = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                                                                .Where(cond => !cond.FPRPM_IsDeleted
                                                                                && cond.lkpReportParameter.lkpControlType.CT_Code != emptyControlType
                                                                                ).ToList();
                String institutionParameterCode = ReportParameters.INSTITUTE.GetStringValue();
                FavParamReportParamMapping favParamReportParamMapping_Institution = lstFavParamReportParamMapping
                                                                                    .Where(cond => cond.lkpReportParameter.RP_Code == institutionParameterCode)
                                                                                        .FirstOrDefault();
                if (!favParamReportParamMapping_Institution.IsNullOrEmpty())
                {
                    String institutionControlIDToFind = favParamReportParamMapping_Institution.lkpReportParameter.RP_Code
                                                                                + "_" + favParamReportParamMapping_Institution.FPRPM_ID;
                    Control institutionControl = (pnlParamValues as Control).FindServerControlRecursively(institutionControlIDToFind);
                    String selectedTenantID;
                    if (!institutionControl.IsNullOrEmpty())
                    {
                        WclComboBox cmbInstitution = institutionControl as WclComboBox;
                        selectedTenantID = Convert.ToString(cmbInstitution.SelectedValue);
                    }
                    else
                    {
                        selectedTenantID = "0";
                    }

                    foreach (FavParamReportParamMapping parameter in lstFavParamReportParamMapping)
                    {
                        String controlIDToFind = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                        Control control = (pnlParamValues as Control).FindServerControlRecursively(controlIDToFind);
                        if (!control.IsNullOrEmpty())
                        {
                            switch (parameter.lkpReportParameter.RP_Code)
                            {
                                case "Category":
                                    WclComboBox cmbCategory = control as WclComboBox;
                                    cmbCategory.ClearSelection();
                                    BindCategory(cmbCategory, selectedTenantID, selectedNodeIds, parameter);
                                    break;

                                case "Item":
                                    WclComboBox cmbItem = control as WclComboBox;
                                    cmbItem.ClearSelection();
                                    BindItem(cmbItem, selectedTenantID, AppConsts.ZERO, parameter);
                                    break;
                            }
                        }
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

        protected void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                //iframeReportViewer.Src = String.Empty;
                WclComboBox cmbCategory = sender as WclComboBox;
                String selectedCategoryIds;
                if (cmbCategory.CheckedItems.IsNullOrEmpty())
                {
                    selectedCategoryIds = String.Empty;
                }
                else
                {
                    selectedCategoryIds = String.Join(",", cmbCategory.CheckedItems.Select(cond => cond.Value));
                }

                if (hdnSelectedCategoryIDs.Value == selectedCategoryIds)
                {
                    return;
                }

                hdnSelectedCategoryIDs.Value = selectedCategoryIds;
                divReport.Visible = false;
                String emptyControlType = ControlType.EMPTY.GetStringValue();
                List<FavParamReportParamMapping> lstFavParamReportParamMapping = CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings
                                                                                .Where(cond => !cond.FPRPM_IsDeleted
                                                                                && cond.lkpReportParameter.lkpControlType.CT_Code != emptyControlType
                                                                                ).ToList();
                String institutionParameterCode = ReportParameters.INSTITUTE.GetStringValue();
                FavParamReportParamMapping favParamReportParamMapping_Institution = lstFavParamReportParamMapping
                                                                                    .Where(cond => cond.lkpReportParameter.RP_Code == institutionParameterCode)
                                                                                        .FirstOrDefault();
                if (!favParamReportParamMapping_Institution.IsNullOrEmpty())
                {
                    String institutionControlIDToFind = favParamReportParamMapping_Institution.lkpReportParameter.RP_Code
                                                                                + "_" + favParamReportParamMapping_Institution.FPRPM_ID;
                    Control institutionControl = (pnlParamValues as Control).FindServerControlRecursively(institutionControlIDToFind);
                    String selectedTenantID;
                    if (!institutionControl.IsNullOrEmpty())
                    {
                        WclComboBox cmbInstitution = institutionControl as WclComboBox;
                        selectedTenantID = Convert.ToString(cmbInstitution.SelectedValue);
                    }
                    else
                    {
                        selectedTenantID = "0";
                    }

                    foreach (FavParamReportParamMapping parameter in lstFavParamReportParamMapping)
                    {
                        String controlIDToFind = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                        Control control = (pnlParamValues as Control).FindServerControlRecursively(controlIDToFind);
                        if (!control.IsNullOrEmpty())
                        {
                            switch (parameter.lkpReportParameter.RP_Code)
                            {
                                case "Item":
                                    WclComboBox cmbItem = control as WclComboBox;
                                    cmbItem.ClearSelection();
                                    BindItem(cmbItem, selectedTenantID, selectedCategoryIds, parameter);
                                    break;
                            }
                        }
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

        /// <summary>
        /// This method set the value in control in edit mode.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="control"></param>
        /// <param name="attributesForCustomForm"></param>
        /// <returns></returns>
        private String GetTheValueForControlInEditMode(Control control, FavParamReportParamMapping parameter)
        {
            switch (parameter.lkpReportParameter.lkpControlType.CT_Code)
            {
                case "AAAA":
                    WclTextBox textBox = control as WclTextBox;
                    return textBox.Text;
                case "AAAB":
                    WclNumericTextBox numericTextBox = control as WclNumericTextBox;
                    return numericTextBox.Text;
                case "AAAD":
                    WclComboBox combo = control as WclComboBox;
                    if ((parameter.lkpReportParameter.RP_Code == ReportParameters.INSTITUTE.GetStringValue() &&
                        (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ACCOUNTS_WITHOUT_ORDER_REPORT.GetStringValue()
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.INVOICE_ORDER.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ACCOUNT_WITHOUT_PURCHASE_REPORT.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENT_DETAILS_WITH_USERID.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.INSTITUTION_COUNT.GetStringValue())  //UAT-3052
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.REQUIREMENT_ITEM_DATA_REPORT.GetStringValue())  //UAT-3052
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENT_DETAILS.GetStringValue())   //UAT-3052
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.AGENCY_ADMINS_BY_DEPARTMENT.GetStringValue()) //UAT-3052
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ITEM_DATA_COUNT_REPORT.GetStringValue())  //UAT-3052
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.CATEGORY_DATA_REPORT.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_COMPLIANCE_STATUS.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_OVERALL_NONCOMPLIANCE_STATUS.GetStringValue())
                        || (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_BY_DAY_OF_THE_WEEK.GetStringValue()) //UAT 3214
                        )) 
                        || (parameter.lkpReportParameter.RP_Code == ReportParameters.CATEGORY.GetStringValue() && (parameter.ReportFavouriteParameter.Report.RP_Code == ReportType.ROTATION_STUDENTS_OVERALL_NONCOMPLIANCE_STATUS.GetStringValue()))  
                        || (parameter.lkpReportParameter.RP_Code == ReportParameters.USER_TYPE.GetStringValue() || parameter.lkpReportParameter.RP_Code == ReportParameters.REVIEW_STATUS.GetStringValue())                        
                        )
                    {
                        if (!combo.CheckedItems.IsNullOrEmpty())
                        {
                            return String.Join(",", combo.CheckedItems.Select(c => c.Value).ToList());
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else
                    {
                        if (combo.SelectedValue.IsNullOrEmpty())
                        {
                            return null;
                        }
                        else
                        {
                            return combo.SelectedValue;
                        }

                    }
                case "AAAE":
                case "AAAC":
                    WclComboBox comboMulti = control as WclComboBox;
                    if (comboMulti.CheckedItems.IsNullOrEmpty())
                    {
                        return null;
                    }
                    else
                    {
                        return String.Join(",", comboMulti.CheckedItems.Select(c => c.Value).ToList());
                    }
                case "AAAF":
                    WclDatePicker datePicker = control as WclDatePicker;
                    if (datePicker.SelectedDate.HasValue)
                    {
                        return Convert.ToString(datePicker.SelectedDate);
                    }
                    else
                    {
                        return null;
                    }
                case "AAAH": //UAT-3052
                    RadioButtonList radiobuttonlist = control as RadioButtonList;
                    if (!radiobuttonlist.SelectedValue.IsNullOrEmpty())
                        return radiobuttonlist.SelectedValue;
                    else
                        return null;
                default:
                    return null;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Event called for updating parameter values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarParameter_SaveClick(object sender, EventArgs e)
        {
            try
            {
                divReport.Visible = false;
                Control ctr = new Control();
                #region UAT-3052
                Int32 adminLoggedID = 0;
                if (!System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    adminLoggedID = Convert.ToInt32(System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"]);
                }
                #endregion
                ReportFavouriteParameter favParam = new ReportFavouriteParameter();
                favParam.RFP_ID = CurrentViewContext.SelectedFavParamID;
                favParam.RFP_Name = txtName.Text.Trim();
                favParam.RFP_Description = txtDescription.Text.Trim();
                favParam.RFP_UserID = CurrentViewContext.CurrentLoggedInUserId;
                favParam.RFP_ModifiedByID = adminLoggedID == 0 ? CurrentViewContext.CurrentLoggedInUserId : adminLoggedID;
                favParam.RFP_ModifiedOn = DateTime.Now;
                Dictionary<Int32, String> dicUpdatedParameters = new Dictionary<Int32, String>();
                foreach (FavParamReportParamMapping parameter in CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings)
                {
                    String controlIDToFind = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                    Control control = (pnlParamValues as Control).FindServerControlRecursively(controlIDToFind);
                    if (!control.IsNullOrEmpty())
                    {
                        String value = GetTheValueForControlInEditMode(control, parameter);
                        if (!dicUpdatedParameters.ContainsKey(parameter.FPRPM_ID))
                        {
                            dicUpdatedParameters.Add(parameter.FPRPM_ID, value);
                        }
                    }
                }
                Boolean isSuccess = Presenter.UpdateFavParamReportParamMapping(dicUpdatedParameters, favParam);
                if (isSuccess)
                {
                    base.ShowSuccessMessage("Parameter values updated successfully.");
                    Presenter.GetSelectedFavParamDetails();
                    lblFavParamName.Text = CurrentViewContext.SelectedFavouriteParameter.Report.RP_Name
                                      + " > " + CurrentViewContext.SelectedFavouriteParameter.RFP_Name.HtmlEncode();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
                    base.ShowErrorMessage("Some error occured while updating parameter values. Please try again.");
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

        /// <summary>
        /// Event called on "View Report"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarParameter_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                Session["SavedReportParameters"] = null;
                divReport.Visible = true;
                iframeReportViewer.Src = String.Empty;

                Dictionary<String, String> dicUpdatedParameters = new Dictionary<String, String>();
                foreach (FavParamReportParamMapping parameter in CurrentViewContext.SelectedFavouriteParameter.FavParamReportParamMappings)
                {
                    String controlIDToFind = parameter.lkpReportParameter.RP_Code + "_" + parameter.FPRPM_ID;
                    Control control = (pnlParamValues as Control).FindServerControlRecursively(controlIDToFind);
                    if (!control.IsNullOrEmpty())
                    {
                        String value = GetTheValueForControlInEditMode(control, parameter);
                        if (!dicUpdatedParameters.ContainsKey(parameter.lkpReportParameter.RP_Code))
                        {
                            dicUpdatedParameters.Add(parameter.lkpReportParameter.RP_Code, value);
                        }
                    }
                    else if (!dicUpdatedParameters.ContainsKey(parameter.lkpReportParameter.RP_Code))
                    {
                        dicUpdatedParameters.Add(parameter.lkpReportParameter.RP_Code, parameter.FPRPM_Value);
                    }
                }


                StringBuilder sb = new StringBuilder();

                foreach (String key in dicUpdatedParameters.Keys)
                {
                    sb.Append(key)
                        .Append("|||")
                        .Append(dicUpdatedParameters[key].IsNullOrEmpty() ? "null" : dicUpdatedParameters[key])
                        .Append("$$$");
                }

                Session["SavedReportParameters"] = sb.ToString();

                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD, @"ReportViewerPage.aspx"},
                                                                    //{"Parameter",sb.ToString()},
                                                                    {"ReportCode",CurrentViewContext.SelectedFavouriteParameter.Report.RP_Code}

                                                                 };
                String redirectUrl = @"ReportViewerPage.aspx?args={0}";
                iframeReportViewer.Src = String.Format(redirectUrl, queryString.ToEncryptedQueryString());
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





        protected void RemoveBtn_Click(object sender, EventArgs e)
        {
            Presenter.DeleteFavParamReportParamMapping(CurrentViewContext.SelectedFavParamID.ToString());
            base.ShowSuccessMessage("Saved Report parameter deleted successfully.");
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
            pnlFavParameters.Visible = false;
            //pnlParamValues.Visible = false;
            fsucCmdBarParameter.Visible = false;
            div2.Attributes.Add("hidden", "true");
            lblParamValues.Visible = false;
            lblFavParamName.Visible = false;
        }
    }
}