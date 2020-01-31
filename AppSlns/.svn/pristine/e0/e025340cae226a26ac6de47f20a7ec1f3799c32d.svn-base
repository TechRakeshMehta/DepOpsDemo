using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.BkgOperations;
using System.Data.Linq;
using INTSOF.Utils;
using System.Web.UI.HtmlControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Data.Entity.Core.Objects.DataClasses;
using INTSOF.UI.Contract.BkgSetup;
using CoreWeb.Shell;
using Entity.ClientEntity;

namespace CoreWeb.BkgOperations.Views
{
    public partial class NewCustomFormHtlm : BaseUserControl, ICustomFormHtlmView
    {
        #region Variables

        #region Private Variables

        private CustomFormHtlmPresenter _presenter = new CustomFormHtlmPresenter();
        private ApplicantOrderCart applicantOrderCart = null;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        private String CountryName { get; set; }
        private String StateName { get; set; }
        private String CityName { get; set; }
        private String ZipCodeName { get; set; }
        private List<String> lstSpecialDataTypes = new List<string> { "Country", "City", "State", "County", "Zip Code" };
        private Dictionary<String, Int32> specialDataType = new Dictionary<String, Int32>();
        private List<Entity.State> lstState { get; set; }
        private List<Entity.City> lstCity { get; set; }
        private List<Entity.ZipCode> lstZipCode { get; set; }
        private List<Entity.County> lstCounty { get; set; }
        private Dictionary<Int32, String> _lstGroupIdNames = new Dictionary<Int32, String>();
        private Boolean HasState { get; set; }
        private Boolean HasCountry { get; set; }

        private Int32 MinimumOccurence
        {
            get
            {
                return lstCustomFormAttributes.OrderByDescending(x => x.MinimumOccurence).IsNullOrEmpty() ? 0 : lstCustomFormAttributes.OrderByDescending(x => x.MinimumOccurence).FirstOrDefault(x => x.AttributeGroupId == groupId).MinimumOccurence;
            }
        }

        private Int32 MaximumOccurence
        {
            get
            {
                return lstCustomFormAttributes.OrderByDescending(x => x.MaximumOccurence).IsNullOrEmpty() ? 0 : lstCustomFormAttributes.OrderByDescending(x => x.MaximumOccurence).FirstOrDefault(x => x.AttributeGroupId == groupId).MaximumOccurence;
            }
        }

        #endregion

        #region Public Properties

        public CustomFormHtlmPresenter Presenter
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

        public Boolean IsReadOnly
        { get; set; }

        public Int32 DisplayColumns { get; set; }
        public String SectionTitle { get; set; }
        public String CustomHtml { get; set; }
        public String InstructionText { get; set; }
        public Int32 tenantId { get; set; }
        public Int32 groupId
        {
            get
            {
                return Convert.ToInt32(hdnGroupId.Value);
            }
            set
            {
                hdnGroupId.Value = value.ToString();
            }
        }

        public Int32 ServiceID
        {
            get
            {
                return hdnServiceId.Value.IsNullOrEmpty() ? 0 : Convert.ToInt32(hdnServiceId.Value);
            }
            set
            {
                hdnServiceId.Value = Convert.ToString(value);
            }
        }

        public Int32 PackageServiceId
        {
            get
            {
                return hdnPackageServiceId.Value.IsNullOrEmpty() ? 0 : Convert.ToInt32(hdnPackageServiceId.Value);
            }
            set
            {
                hdnPackageServiceId.Value = Convert.ToString(value);
            }
        }

        public Boolean IsEdit
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (applicantOrderCart.IsNullOrEmpty())
                {
                    return false;
                }
                return applicantOrderCart.IsEditMode;

            }
            set
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (!applicantOrderCart.IsNullOrEmpty())
                    applicantOrderCart.IsEditMode = value;
            }
        }

        public Boolean IsSupplementalOrder { get; set; }
        public Boolean ShowPreExistingSupplementData { get; set; }

        public Boolean IsOrderConfirmation { get; set; }
        public Boolean IsOrderDetailForEds { get; set; }
        public Int32 OrderId { get; set; }

        public List<BackgroundOrderData> lstBackgroundOrderData { get; set; }

        public List<SupplementOrderData> lstSupplementOrderData { get; set; }

        public List<AttributesForCustomFormContract> lstCustomFormAttributes
        {
            get
            {
                if (ViewState["lstCustomFormAttributes"].IsNullOrEmpty())
                    return new List<AttributesForCustomFormContract>();
                return (List<AttributesForCustomFormContract>)ViewState["lstCustomFormAttributes"];
            }
            set
            {
                ViewState["lstCustomFormAttributes"] = value;
            }

        }

        #region UAT-586 WB: AMS: When adding supplemental services to an order, need to be able to see what the applicant has already entered (location, alias, etc.)
        public Int32 MasterOrderID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID);
                return 0;
            }
        }
        #endregion

        public Boolean ShowEditDetailButton { get; set; }

        public Int32 InstanceId
        {
            get
            {
                return hdnInstanceId.Value.IsNullOrEmpty() ? 1 : Convert.ToInt32(hdnInstanceId.Value);
            }
            set
            {
                hdnInstanceId.Value = Convert.ToString(value);
            }
        }

        public List<Int32> lstHiddenInstanceOfGroup { get; set; }

        public Int32 ServiceItemID
        {
            get
            {
                return hdnServiceItemId.Value.IsNullOrEmpty() ? 0 : Convert.ToInt32(hdnServiceItemId.Value);
            }
            set
            {
                hdnServiceItemId.Value = Convert.ToString(value);
            }
        }

        public Int32 occurence
        {
            get
            {
                return lstCustomFormAttributes.IsNullOrEmpty() ? 0 : lstCustomFormAttributes.FirstOrDefault(x => x.AttributeGroupId == groupId).Occurence;
            }
        }

        public Int32 CurrentInstanceIdForGroup { get; set; }


        public String[] GetTextBoxIds
        {
            get
            {
                if (!hdnHiddenTextBoxIds.Value.IsNullOrEmpty())
                {
                    return hdnHiddenTextBoxIds.Value.Split(':');
                }
                return null;
            }
        }

        public String[] GetDropDownIds
        {
            get
            {
                if (!hdnHiddenDropDownIds.Value.IsNullOrEmpty())
                {
                    return hdnHiddenDropDownIds.Value.Split(':');
                }
                return null;
            }
        }

        public Int32 CustomFormId
        {
            get
            {
                return hdnCurrentFormId.Value.IsNullOrEmpty() ? 0 : Convert.ToInt32(hdnCurrentFormId.Value);
            }
            set
            {
                hdnCurrentFormId.Value = Convert.ToString(value);
            }
        }
        Dictionary<Int32, String> lstGroupIdNames { get; set; }

        public List<SystemSpecificLanguageText> SystemSpecificLanguageTextList
        {
            get
            {
                return new List<SystemSpecificLanguageText>();
            }
            set
            {
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            //GetTheListOfGroupIdsName();
            GetHtmLCreated();
            if (IsReadOnly)
            {
                if (ShowEditDetailButton)
                {
                    fsucCmdBar1.Visible = false;
                    //fsucCmdBar1.SaveButton.Style.Add("display", "none");
                    //cmdbarEdit.Text = "Edit " + SectionTitle + "";
                    //fsucCmdBar1.SubmitButtonText = "Edit " + SectionTitle + "";
                }
                else
                {
                    fsucCmdBar1.Visible = false;
                    //cmdbarEdit.Visible = false;
                }
            }
            //else if ((occurence < 2))
            //{
            //    CommandBar.Style.Add("display", "none");
            //}
            else if ((occurence < 2) || ((MaximumOccurence == CurrentInstanceIdForGroup - 1) || !(MaximumOccurence > CurrentInstanceIdForGroup - 1) || (MaximumOccurence == 0 && !occurence.IsNullOrEmpty() && occurence < 1) || IsOrderConfirmation))
            {
                CommandBar.Style.Add("display", "none");
                //fsucCmdBar1.Visible = false;
            }
            else
            {
                CommandBar.Style.Add("display", "block");
                fsucCmdBar1.SaveButton.ValidationGroup = "submitForm" + groupId;
                /* Fix for UAT 923 - Also removed the Submit button from HTML mark up
                fsucCmdBar1.SubmitButton.Style.Add("display", "none");*/

                //cmdbarEdit.Visible = false;
            }
            CommandBar.Attributes.Add("class", groupId.ToString());

            #region Code to stop Validation of the newly added Instance. 'SaveButton.CausesValidation' is enabled when the button Event is fired.

            if (IsPostBack)
            {
                String _postBackCtrlname = Page.Request.Params["__EVENTTARGET"];
                // If postback is from any Button
                if (_postBackCtrlname.IsNullOrEmpty())
                {
                    String ctrlStr = String.Empty;
                    Control _ctrl = null;
                    foreach (String _ctl in Page.Request.Form)
                    {
                        _ctrl = Page.FindControl(_ctl);
                        if (_ctrl is INTERSOFT.WEB.UI.WebControls.WclButton && _ctrl.ID.ToLower() == fsucCmdBar1.SaveButton.ID.ToLower())
                        {
                            // If the button is 'Add New' and not 'Restart Order'
                            if ((_ctrl as WclButton).Text.ToLower() == "add new")
                            {
                                fsucCmdBar1.SaveButton.CausesValidation = false;
                                break;
                            }
                        }
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Buttton Events

        protected void CmdBarRestart_Click(object sender, EventArgs e)
        {
            fsucCmdBar1.SaveButton.CausesValidation = true;
        }

        private String GetHREFForTheEditButton()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {  
                                                                    //{AppConsts.CHILD,  ChildControls.ApplicantDisclosurePage}
                                                                    { AppConsts.CHILD, ChildControls.CustomFormLoad},
                                                                    {"CustomFormId",hdnCurrentFormId.Value}

                                                                 };
            return String.Format("~/BkgOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
        }

        protected void CmdBarEditCustomForm_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            IsEdit = true;
            applicantOrderCart.EDrugScreeningRegistrationId = null;
            queryString = new Dictionary<String, String>
                                                                 {  
                                                                    //{AppConsts.CHILD,  ChildControls.ApplicantDisclosurePage}
                                                                    { AppConsts.CHILD, ChildControls.CustomFormLoad},
                                                                    {"CustomFormId",hdnCurrentFormId.Value}

                                                                 };
            Response.Redirect(String.Format("~/BkgOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

        }

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Function that initiate the creation of the dynamic HTMl
        /// </summary>
        private void GetHtmLCreated()
        {
            if ((occurence > 1) && (MinimumOccurence > Convert.ToInt32(InstanceId)))
            {
                InstanceId = MinimumOccurence;
            }
            Int32 instanceCount = Convert.ToInt32(InstanceId);

            CurrentInstanceIdForGroup = 1;
            for (Int32 i = 0; i < instanceCount; i++)
            {
                if (lstHiddenInstanceOfGroup.IsNull() || (lstHiddenInstanceOfGroup.IsNotNull() && !lstHiddenInstanceOfGroup.Contains(i + 1)))
                {
                    if (IsReadOnly || IsEdit || IsOrderConfirmation || IsSupplementalOrder)
                    {
                        lstState = null;
                        lstCity = null;
                        lstCounty = null;
                        lstZipCode = null;
                        if (IsSupplementalOrder)
                            SetThePreviousValuesPopulatedForServiceItems(i);
                        else
                            SetThePreviousValuesPopulated(i);
                    }
                    hiddenInstance.Attributes.Add("class", groupId.ToString());
                    GenerateSectionControlsForAGroup(groupId, i + 1);
                    fsucCmdBar1.SaveButton.CssClass = groupId.ToString();
                    CurrentInstanceIdForGroup++;
                }
            }
        }

        /// <summary>
        /// Set the previous values that are stored in the 
        /// session for the particular instance.
        /// </summary>
        /// <param name="instanceId"></param>
        private void SetThePreviousValuesPopulated(Int32 instanceId)
        {
            // Avoid the issue with STR
            // 1. user comes to second custom form and clicks 'Previous' to go to previous custom form
            // 2. Clicks 'Next' from the custom form loaded from step 1 a
            // 3. Finally clicks 'Previous' from the same screen, as in step 1
            if (lstBackgroundOrderData.IsNull())
                return;

            if (lstBackgroundOrderData.FirstOrDefault(x => x.BkgSvcAttributeGroupId == groupId && x.CustomFormId == CustomFormId && x.InstanceId == (instanceId + 1)).IsNullOrEmpty())
            {
                _lstGroupIdNames = new Dictionary<Int32, String>();
            }
            else
            {
                _lstGroupIdNames = lstBackgroundOrderData.FirstOrDefault(x => x.BkgSvcAttributeGroupId == groupId && x.CustomFormId == CustomFormId && x.InstanceId == (instanceId + 1)).CustomFormData;
            }
        }

        /// <summary>
        /// Set the previous values that are stored in the 
        /// session for the particular instance.
        /// </summary>
        /// <param name="instanceId"></param>
        private void SetThePreviousValuesPopulatedForServiceItems(Int32 instanceId)
        {
            if (lstSupplementOrderData.IsNullOrEmpty())
            {
                _lstGroupIdNames = new Dictionary<Int32, String>();
            }
            else if (lstSupplementOrderData.FirstOrDefault(x => x.BkgSvcAttributeGroupId == groupId
                && x.PackageSvcItemId == ServiceItemID && x.CustomFormId == CustomFormId && x.InstanceId == (instanceId + 1)).IsNullOrEmpty())
            {
                _lstGroupIdNames = new Dictionary<Int32, String>();
            }
            else
            {
                _lstGroupIdNames = lstSupplementOrderData.FirstOrDefault(x => x.BkgSvcAttributeGroupId == groupId
                                    && x.PackageSvcItemId == ServiceItemID && x.CustomFormId == CustomFormId && x.InstanceId == (instanceId + 1)).FormData;
            }
        }

        /// <summary>
        /// This function is main that creates the skeleton for look and feel for a 
        /// particular section corresponding to a particular group of attributes
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="CurrentInstanceId"></param>
        private void GenerateSectionControlsForAGroup(Int32 groupId, Int32 CurrentInstanceId = 1)
        {
            var groupdata = lstCustomFormAttributes.FirstOrDefault(x => x.AttributeGroupId == groupId);
            if (lstCustomFormAttributes.Any(x => x.AttributeGroupId == groupId && x.IsDisplay))
            {
                SectionTitle = groupdata.SectionTitle.IsNullOrEmpty() ? "" : groupdata.SectionTitle;
                CustomHtml = groupdata.CustomHtml.IsNullOrEmpty() ? "" : groupdata.CustomHtml;
                InstructionText = groupdata.InstructionText.IsNullOrEmpty() ? "" : groupdata.InstructionText;
                DisplayColumns = groupdata.DisplayColumns.IsNullOrEmpty() ? 3 : groupdata.DisplayColumns;
                //occurence = groupdata.Occurence.IsNullOrEmpty() ? 1 : groupdata.Occurence;
                Control mainPanelForFor = null;
                if (IsOrderConfirmation)
                {
                    mainPanelForFor = RenderSectionHtmlForOrderConfirmation(groupId, CurrentInstanceId);
                }
                else
                {
                    mainPanelForFor = RenderHeaderSectionHtml(groupId, CurrentInstanceId);
                }

                //Call To method to create the dynamic Html for a particular group
                GenerateFormForColumDisplay(groupId, DisplayColumns, mainPanelForFor, CurrentInstanceId);

                //Check to show the Eds Document link 
                if (IsOrderDetailForEds)
                {
                    AddEdsDocumentLinkInPanel(mainPanelForFor);
                }
            }
        }

        /// <summary>
        /// This creates the header section 
        /// for the title of the group
        /// </summary>
        /// <returns>Panell in which the remaining controls are dynamically loaded.</returns>
        private Control RenderHeaderSectionHtml(Int32 groupId, Int32 currentInstanceId)
        {

            HtmlGenericControl mainContainer = new HtmlGenericControl("div");
            mainContainer.ID = "mainDiv_" + groupId.ToString() + "_" + currentInstanceId.ToString();
            HtmlGenericControl container_fluid = new HtmlGenericControl("div");
            container_fluid.Attributes.Add("class", "container-fluid");

            //------------------------------------------START HEADER---------------------------
            //12/08/2014 Add "mhdr" class to Div rather than H1 to add edit button to div.
            HtmlGenericControl divHeaderRow = new HtmlGenericControl("div");
            divHeaderRow.Attributes.Add("class", "row");
            HtmlGenericControl divCol_12 = new HtmlGenericControl("div");
            divCol_12.Attributes.Add("class", "col-md-12");
            divCol_12.Attributes.Add("id", "divCustomFormMhdr" + hdnInstanceId.Value + hdnCurrentFormId.Value + hdnGroupId.Value + "");
            HtmlGenericControl headerTag = new HtmlGenericControl("h2");
            headerTag.Attributes.Add("class", "header-color");
            Label headerLable = new Label();
            headerLable.ID = "lblHeader_" + groupId.ToString() + "_" + currentInstanceId.ToString();
            headerLable.Text = SectionTitle + " - " + CurrentInstanceIdForGroup.ToString();
            headerLable.CssClass = currentInstanceId.ToString();
            headerTag.Controls.Add(headerLable);
            SetDeleteButton(headerTag, currentInstanceId);
            divCol_12.Controls.Add(headerTag);


            //12/08/2014 Create Button Dynamically to show within the Accordion Mhdr Div
            if (ShowEditDetailButton && (ViewState["IsEditButtonExist"].IsNullOrEmpty() || ViewState["IsEditButtonExist"].ToString().ToLower() != "true"))
            {
                ViewState["IsEditButtonExist"] = true;
                HtmlGenericControl editBtnDiv = new HtmlGenericControl("div");
                editBtnDiv.Attributes.Add("style", "right: 20px; position: absolute; z-index: 99999999999; bottom: 20px;");
                editBtnDiv.Attributes.Add("class", "RadButton_Outlook");

                WclButton editBtn = new WclButton();
                editBtn.ID = "editBtn" + hdnInstanceId.Value + hdnCurrentFormId.Value + hdnGroupId.Value + "";
                editBtn.Text = "Edit " + SectionTitle + "";
                editBtn.AutoPostBack = true;
                editBtn.ButtonType = Telerik.Web.UI.RadButtonType.StandardButton;
                editBtn.Attributes.Add("onclick", "stopColapseCustomForm('" + hdnInstanceId.Value + hdnCurrentFormId.Value + hdnGroupId.Value + "', '" + cmdbarEdit.ClientID + "');");
                editBtn.Attributes.Add("style", "float:right");
                editBtnDiv.Controls.Add(editBtn);
                divCol_12.Controls.Add(editBtnDiv);
            }
            divHeaderRow.Controls.Add(divCol_12);
            container_fluid.Controls.Add(divHeaderRow);
            //------------------------------------------END HEADER---------------------------


            //HtmlGenericControl content = new HtmlGenericControl("div");
            //content.Attributes.Add("class", "content");
            //HtmlGenericControl Container = new HtmlGenericControl("div");
            //Container.Attributes.Add("class", "sxform auto");


            //Panel in which all the other controls are loaded
            Panel formPanel = new Panel();
            formPanel.ID = "pnl_" + currentInstanceId.ToString() + "_" + groupId;
            formPanel.CssClass = "sxpnl";

            HtmlGenericControl row_bgLightGreen = new HtmlGenericControl("div");
            row_bgLightGreen.Attributes.Add("class", "row bgLightGreen");

            //Container.Controls.Add(formPanel);
            if (currentInstanceId == 1 && (CustomHtml != "" || InstructionText != "") && !IsReadOnly)
            {
                HtmlGenericControl customHtmlDiv = new HtmlGenericControl("div");
                customHtmlDiv.Attributes.Add("class", "customli");
                customHtmlDiv.InnerHtml = CustomHtml;
                row_bgLightGreen.Controls.Add(customHtmlDiv);
                HtmlGenericControl customInstructionTextDiv = new HtmlGenericControl("div");
                customInstructionTextDiv.Attributes.Add("class", "customli");
                customInstructionTextDiv.InnerHtml = InstructionText;
                row_bgLightGreen.Controls.Add(customInstructionTextDiv);
            }
            //row_bgLightGreen.Controls.Add(Container);
            formPanel.Controls.Add(row_bgLightGreen);
            container_fluid.Controls.Add(formPanel);
            mainContainer.Controls.Add(container_fluid);
            pnlRendercustomForm.Controls.Add(mainContainer);
            return formPanel;
        }

        /// <summary>
        /// Style pattern third for the 
        /// order confirmation page.
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <param name="currentInstanceId">currentInstanceId</param>
        /// <returns></returns>
        private Control RenderSectionHtmlForOrderConfirmation(Int32 groupId, Int32 currentInstanceId)
        {
            HtmlGenericControl headerLine = new HtmlGenericControl("hr");
            headerLine.Attributes.Add("class", "lineStyle");
            Panel formPanel = new Panel();
            formPanel.ID = "pnl_" + currentInstanceId.ToString() + "_" + groupId;
            HtmlGenericControl customHtmlHeader = new HtmlGenericControl("h6");
            customHtmlHeader.Attributes.Add("class", "customHeader");
            customHtmlHeader.InnerHtml = SectionTitle + "-" + currentInstanceId.ToString();
            formPanel.Controls.Add(customHtmlHeader);
            pnlRendercustomForm.Controls.Add(formPanel);
            pnlRendercustomForm.Controls.Add(headerLine);
            return formPanel;
        }

        /// <summary>
        /// This creates the dynamic HTML as per the group id and the number of column in each row
        /// </summary>
        /// <param name="groupId">Group ID</param>
        /// <param name="columnNumber">No of column per row</param>
        /// <param name="mainPanelForFor">Main panel in wwhich the HTML is loaded.</param>
        private void GenerateFormForColumDisplay(Int32 groupId, Int32 columnNumber, Control mainPanelForFor, Int32 currentInstanceId)
        {
            int i = 0;
            HtmlGenericControl controlInColumn = null;
            //Get the list of attributes corresponding to the group ID
            List<AttributesForCustomFormContract> lstAttributesForGroupId = lstCustomFormAttributes.Where(x => x.AttributeGroupId == groupId)
                                                                            .OrderBy(x => x.CustomFieldsDisplaySequence)
                                                                            .Select(x => x)
                                                                            .OrderBy(x => x.CustomFieldsDisplaySequence).ToList();
            if (IsEdit && lstAttributesForGroupId.IsNotNull() && lstAttributesForGroupId.Count > 0)
            {
                SetSpecialDataTypeData(lstAttributesForGroupId);
            }
            else //if (IsSupplementalOrder)
            {
                SetSpecialDataTypeDataForSupplemental(lstAttributesForGroupId);
            }
            //else
            //{
            //    SetSpecialDataTypeDataForSupplemental(lstAttributesForGroupId);
            //}
            foreach (var attributes in lstAttributesForGroupId)
            {
                if (attributes.IsDisplay)
                {
                    //If the number of control generated = number of column 
                    //per row then add to the form n create new row.
                    if (i == columnNumber)
                    {
                        AddNextLineDiv(controlInColumn);
                        mainPanelForFor.Controls.Add(controlInColumn);
                        i = 0;
                    }
                    if (i == 0)
                    {
                        //Generate a new row
                        controlInColumn = GenerateColumnView(columnNumber);
                    }
                    controlInColumn = GenerateControl(attributes, controlInColumn, currentInstanceId);
                    i++;
                }
            }
            if (i != 0)
            {
                AddNextLineDiv(controlInColumn);
                mainPanelForFor.Controls.Add(controlInColumn);
            }

        }

        private void SetDeleteButton(HtmlGenericControl headerTag, Int32 currentInstanceId)
        {
            if (currentInstanceId > MinimumOccurence && !IsReadOnly && !IsOrderConfirmation)
            {

                HtmlGenericControl delete = new HtmlGenericControl("span");
                delete.Attributes.Add("onclick", "if (!confirm('Are you sure you want to remove this instance?')){return false;} else {HideTheCurrentInstance(this);return false;}");
                delete.InnerHtml = "Delete";
                delete.Style.Add("float", "right");
                /* UAT 921 Fix. 'class' cannot be used as same class is used for '*' span of validators.
                   So change in decision field combobox also hides the 'Delete' button
                delete.Attributes.Add("class", groupId.ToString() + "_" + currentInstanceId.ToString()); */

                delete.Attributes.Add("id", groupId.ToString() + "_" + currentInstanceId.ToString());
                headerTag.Controls.Add(delete);
            }
        }

        /// <summary>
        /// Set the special data into
        /// the parameter at the page load in edit mode.
        /// </summary>
        /// <param name="lstAttributesForGroupId"></param>
        private void SetSpecialDataTypeData(List<AttributesForCustomFormContract> lstAttributesForGroupId)
        {
            IEnumerable<KeyValuePair<String, Int32>> listSpecial = lstAttributesForGroupId.Where(x => lstSpecialDataTypes.Contains(x.AttributeType)).DistinctBy(x => x.AttributeType).Select(col =>
                new KeyValuePair<String, Int32>
                (
                col.AttributeType,
                   col.AtrributeGroupMappingId

                )).ToList();
            if (specialDataType.Count > 0)
                specialDataType = new Dictionary<string, int>();
            foreach (var a in listSpecial)
            {
                if (!specialDataType.ContainsKey(a.Key))
                    specialDataType.Add(a.Key, a.Value);
            }
            Int32 mappingId = 0;
            if (specialDataType.Count > 0 && _lstGroupIdNames.Count > 0)
            {
                if (specialDataType.ContainsKey("Country"))
                    mappingId = specialDataType["Country"].IsNotNull() ? specialDataType["Country"] : 0;
                if (_lstGroupIdNames.ContainsKey(mappingId))
                    CountryName = _lstGroupIdNames[mappingId];
                if (specialDataType.ContainsKey("State"))
                    mappingId = specialDataType["State"].IsNotNull() ? specialDataType["State"] : 0;
                if (_lstGroupIdNames.ContainsKey(mappingId))
                    StateName = _lstGroupIdNames[mappingId];
                if (specialDataType.ContainsKey("City"))
                    mappingId = specialDataType["City"].IsNotNull() ? specialDataType["City"] : 0;
                if (_lstGroupIdNames.ContainsKey(mappingId))
                    CityName = _lstGroupIdNames[mappingId];
                if (specialDataType.ContainsKey("Zip Code"))
                    mappingId = specialDataType["Zip Code"].IsNotNull() ? specialDataType["Zip Code"] : 0;
                if (_lstGroupIdNames.ContainsKey(mappingId))
                    ZipCodeName = _lstGroupIdNames[mappingId];
            }
            else
            {
                //Code changes w.r.t. UAT 627 - Fill in "United States" as default selection for Country dropdowns.
                //CountryName = String.Empty;
                CountryName = "UNITED STATES";
                StateName = String.Empty;
                CityName = String.Empty;
                ZipCodeName = String.Empty;
            }
        }

        private void SetSpecialDataTypeDataForSupplemental(List<AttributesForCustomFormContract> lstAttributesForGroupId)
        {
            IEnumerable<KeyValuePair<String, Int32>> listSpecial = lstAttributesForGroupId.Where(x => lstSpecialDataTypes.Contains(x.AttributeType)).Select(col =>
                new KeyValuePair<String, Int32>
                (
                col.AttributeType,
                   col.AtrributeGroupMappingId

                )).ToList();
            if (specialDataType.Count > 0)
                specialDataType = new Dictionary<string, int>();

            foreach (var a in listSpecial)
            {
                if (!specialDataType.ContainsKey(a.Key))
                    specialDataType.Add(a.Key, a.Value);
            }
            if (specialDataType.Count > 0)
            {
                if (specialDataType.ContainsKey("State"))
                {
                    HasState = true;
                }
                if (specialDataType.ContainsKey("Country"))
                {
                    HasCountry = true;
                }
            }
            else
            {
                //Code changes w.r.t. UAT 627 - Fill in "United States" as default selection for Country dropdowns.
                //CountryName = String.Empty;
                CountryName = "UNITED STATES";
                StateName = String.Empty;
                CityName = String.Empty;
                ZipCodeName = String.Empty;
            }
        }

        /// <summary>
        /// Generate a new row
        /// </summary>
        /// <param name="columnNumber">Number of column per row</param>
        /// <returns></returns>
        private HtmlGenericControl GenerateColumnView(Int32 columnNumber)
        {
            HtmlGenericControl rowbgLightGreen = new HtmlGenericControl("div");
            rowbgLightGreen.Attributes.Add("class", "row bgLightGreen");
            return rowbgLightGreen;
        }



        /// <summary>
        /// Generate a new row
        /// </summary>
        /// <param name="columnNumber">Number of column per row</param>
        /// <returns></returns>
        private HtmlGenericControl GenerateColumnView_GreenRow(Int32 columnNumber)
        {
            HtmlGenericControl rowbgLightGreen = new HtmlGenericControl("div");
            rowbgLightGreen.Attributes.Add("class", "bgLightGreen");
            return rowbgLightGreen;
        }

        private HtmlGenericControl GenerateColumnNumbersView(Int32 columnNumber = 3)
        {
            HtmlGenericControl twoColumn = new HtmlGenericControl("div");
            if (columnNumber == AppConsts.THREE)
            {
                twoColumn.Attributes.Add("class", "form-group col-md-4");
            }
            else if (columnNumber == AppConsts.TWO)
            {
                twoColumn.Attributes.Add("class", "form-group col-md-6");
            }
            return twoColumn;
        }

        /// <summary>
        /// Add relevant space between tweo row.
        /// </summary>
        /// <param name="parentControl"></param>
        /// <returns>parentControl</returns>
        private HtmlGenericControl AddNextLineDiv(HtmlGenericControl parentControl)
        {
            String className = "sxroend";
            if (IsOrderConfirmation)
                className = "newLine";
            HtmlGenericControl nextLineDiv = new HtmlGenericControl("div");

            nextLineDiv.Attributes.Add("class", className);
            parentControl.Controls.Add(nextLineDiv);
            return parentControl;
        }

        /// <summary>
        /// Main function that creates a control nas per their data type.
        /// </summary>
        /// <param name="attributesForCustomForm">Attribute data to be created.</param>
        /// <param name="parentControl"></param>
        /// <returns></returns>
        private HtmlGenericControl GenerateControl(AttributesForCustomFormContract attributesForCustomForm, HtmlGenericControl parentControl, Int32 currentInstanceId)
        {
            if (IsReadOnly)
            {
                return CreateControlForReadOnlyMode(attributesForCustomForm, currentInstanceId, parentControl);
            }
            else if (IsOrderConfirmation)
            {
                return CreateControlForOrderConfirmation(attributesForCustomForm, currentInstanceId, parentControl);
            }

            HtmlGenericControl lableDiv = CreateLabelForTheControl(attributesForCustomForm, currentInstanceId);
            parentControl.Controls.Add(lableDiv);
            HtmlGenericControl controlDiv = CreateControlForTheForm(attributesForCustomForm, currentInstanceId);
            parentControl.Controls.Add(controlDiv);
            return parentControl;
        }

        /// <summary>
        /// This method create lable for the control.
        /// </summary>
        /// <param name="attributesForCustomForm"></param>
        /// <param name="currentInstanceId"></param>
        /// <returns></returns>
        private HtmlGenericControl CreateLabelForTheControl(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId)
        {
            HtmlGenericControl lableDiv = new HtmlGenericControl("div");
            lableDiv.Attributes.Add("class", "sxlb");
            Label attributeLable = new Label();
            attributeLable.ID = "lbl" + attributesForCustomForm.AttributeType + "_" + attributesForCustomForm.AttributeGroupId + "_" + currentInstanceId.ToString() + "_" + attributesForCustomForm.AttributeId;
            attributeLable.Text = attributesForCustomForm.DisplayName.IsNullOrEmpty() ? attributesForCustomForm.AttributeName : attributesForCustomForm.DisplayName;//UAT 4730
            lableDiv.Controls.Add(attributeLable);
            if (attributesForCustomForm.IsRequired)
            {
                lableDiv.Controls.Add(AddRequiredSign(currentInstanceId));
            }
            return lableDiv;
        }

        /// <summary>
        /// this method creates control corresponding to different data type.
        /// </summary>
        /// <param name="attributesForCustomForm"></param>
        /// <param name="currentInstanceId"></param>
        /// <returns></returns>
        private HtmlGenericControl CreateControlForTheForm(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId)
        {
            HtmlGenericControl controlDiv = new HtmlGenericControl("div");
            controlDiv.Attributes.Add("class", "sxlm");
            switch (attributesForCustomForm.AttributeTypeCode)
            {
                case "AAAB":
                    if (attributesForCustomForm.AttributeCode.ToUpper().Equals("4E12CF9A-A4AF-4A38-B4BB-47E6059259F4"))
                    {
                        WclMaskedTextBox mastTextBox = new WclMaskedTextBox();
                        mastTextBox.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        mastTextBox.Style.Add("display", "block");
                        mastTextBox.Mask = "(###)-###-####";
                        if (!attributesForCustomForm.MaximumValue.IsNullOrEmpty())
                            mastTextBox.MaxLength = Convert.ToInt32(attributesForCustomForm.MaximumValue);
                        if (IsEdit)
                        {
                            var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, mastTextBox, attributesForCustomForm);
                            mastTextBox = dataSet as WclMaskedTextBox;
                        }
                        controlDiv.Controls.Add(mastTextBox);
                        if (attributesForCustomForm.IsRequired)
                        {                                                                     //UAT-4730
                            controlDiv.Controls.Add(SetrequiredFieldValidator(mastTextBox.ID, attributesForCustomForm.DisplayName.IsNullOrEmpty() ? attributesForCustomForm.AttributeName : attributesForCustomForm.DisplayName, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId));
                        }
                    }
                    else
                    {
                        WclTextBox textBox = new WclTextBox();
                        textBox.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        textBox.Style.Add("display", "block");
                        if (!attributesForCustomForm.MaximumValue.IsNullOrEmpty())
                            textBox.MaxLength = Convert.ToInt32(attributesForCustomForm.MaximumValue);
                        if (IsEdit)
                        {
                            var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, textBox, attributesForCustomForm);
                            textBox = dataSet as WclTextBox;
                        }
                        controlDiv.Controls.Add(textBox);
                        if (attributesForCustomForm.IsRequired)
                        {                                                                 //UAT-4730
                            controlDiv.Controls.Add(SetrequiredFieldValidator(textBox.ID, attributesForCustomForm.DisplayName.IsNullOrEmpty() ? attributesForCustomForm.AttributeName : attributesForCustomForm.DisplayName, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId));
                        }
                    }
                    break;
                case "AAAC":
                    WclNumericTextBox txtNumeric = new WclNumericTextBox();
                    txtNumeric.ID = "txtNumericType_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    txtNumeric.Style.Add("display", "block");
                    if (!attributesForCustomForm.MaximumValue.IsNullOrEmpty())
                        txtNumeric.MaxValue = Convert.ToInt32(attributesForCustomForm.MaximumValue);
                    if (!attributesForCustomForm.MinimumValue.IsNullOrEmpty())
                        txtNumeric.MinValue = Convert.ToInt32(attributesForCustomForm.MinimumValue);
                    if (IsEdit)
                    {
                        var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, txtNumeric, attributesForCustomForm);
                        txtNumeric = dataSet as WclNumericTextBox;
                    }
                    controlDiv.Controls.Add(txtNumeric);
                    if (attributesForCustomForm.IsRequired)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(txtNumeric.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId));
                    }
                    break;
                case "AAAA":
                    WclDatePicker dPicker = new WclDatePicker();
                    dPicker.DateInput.DateFormat = "MM-dd-yyyy";
                    dPicker.ID = "dp_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    dPicker.DateInput.EmptyMessage = "Select a date";
                    dPicker.Style.Add("display", "block");
                    if (!attributesForCustomForm.MaximumValue.IsNullOrEmpty())
                        dPicker.MaxDate = Convert.ToDateTime(attributesForCustomForm.MaximumValue);
                    if (!attributesForCustomForm.MinimumValue.IsNullOrEmpty())
                        dPicker.MinDate = Convert.ToDateTime(attributesForCustomForm.MinimumValue);
                    //dPicker.MinDate = Convert.ToDateTime("01-01-1900");
                    if (IsEdit)
                    {
                        var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, dPicker, attributesForCustomForm);
                        dPicker = dataSet as WclDatePicker;
                    }
                    controlDiv.Controls.Add(dPicker);
                    if (attributesForCustomForm.IsRequired)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(dPicker.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId));
                    }
                    break;
                case "AAAD":
                    WclComboBox dropDownList = new WclComboBox();
                    dropDownList.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    dropDownList.Style.Add("display", "block");
                    dropDownList.DataTextField = "EBSAO_OptionText";
                    dropDownList.DataValueField = "EBSAO_OptionText";
                    dropDownList.EmptyMessage = "-- SELECT --";
                    dropDownList.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
                    dropDownList.EnableTextSelection = true;
                    dropDownList.MarkFirstMatch = true;
                    dropDownList.OnClientBlur = "onLocationBlur";

                    dropDownList.OnClientKeyPressing = "openCmbBoxOnTab";
                    if (attributesForCustomForm.IsDecisionField)
                    {
                        dropDownList.OnClientSelectedIndexChanged = "GetDecisionForTheFields";

                    }
                    dropDownList.DataSource = Presenter.GetOptionValues(attributesForCustomForm.AttributeId);
                    dropDownList.DataBind();
                    if (IsEdit)
                    {
                        var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, dropDownList, attributesForCustomForm);
                        dropDownList = dataSet as WclComboBox;
                    }
                    controlDiv.Controls.Add(dropDownList);
                    if (attributesForCustomForm.IsRequired)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownList.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId));
                    }
                    break;
                case "AAAE":
                    WclComboBox dropDownListCountry = new WclComboBox();
                    dropDownListCountry.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    dropDownListCountry.DataTextField = "CompleteName";
                    dropDownListCountry.DataValueField = "FullName";
                    dropDownListCountry.CssClass = "JCountry";
                    dropDownListCountry.EmptyMessage = "-- SELECT --";
                    dropDownListCountry.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
                    dropDownListCountry.OnClientKeyPressing = "openCmbBoxOnTab";
                    dropDownListCountry.Style.Add("display", "block");
                    dropDownListCountry.OnClientSelectedIndexChanged = "GetDataForDropDown";
                    //Changes w.r.t. UAT 627 - Fill in "United States" as default selection for Country dropdowns. 
                    var listOFCountries = Presenter.GetListOFCountries();
                    if (listOFCountries.IsNotNull())
                    {
                        dropDownListCountry.DataSource = listOFCountries;
                        dropDownListCountry.DataBind();
                        var CountryUSA = listOFCountries.FirstOrDefault(x => x.CountryID == AppConsts.COUNTRY_USA_ID);
                        if (CountryUSA.IsNotNull())
                        {
                            dropDownListCountry.SelectedValue = CountryUSA.FullName;
                            CountryName = CountryUSA.FullName;
                        }
                    }
                    if (IsEdit)
                    {
                        var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, dropDownListCountry, attributesForCustomForm);
                        dropDownListCountry = dataSet as WclComboBox;
                    }
                    controlDiv.Controls.Add(dropDownListCountry);
                    if (attributesForCustomForm.IsRequired)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownListCountry.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "drop" + attributesForCustomForm.AttributeType, currentInstanceId));
                    }
                    break;
                case "AAAF":
                    SetTheStateControl(attributesForCustomForm, currentInstanceId, controlDiv);
                    break;
                case "AAAH":
                    SetTheCityControl(attributesForCustomForm, currentInstanceId, controlDiv);
                    break;
                case "AAAI":
                    SetTheZipCodeControl(attributesForCustomForm, currentInstanceId, controlDiv);
                    break;
                case "AAAG":
                    SetTheCountyControl(attributesForCustomForm, currentInstanceId, controlDiv);
                    break;
                case "AAAJ":
                    WclComboBox dropDownListCountryCitizen = new WclComboBox();
                    dropDownListCountryCitizen.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    dropDownListCountryCitizen.DataTextField = "CompleteName";
                    dropDownListCountryCitizen.DataValueField = "FullName";
                    dropDownListCountryCitizen.CssClass = "JCountry";
                    dropDownListCountryCitizen.EmptyMessage = "-- SELECT --";
                    dropDownListCountryCitizen.Style.Add("display", "block");
                    dropDownListCountryCitizen.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
                    dropDownListCountryCitizen.OnClientKeyPressing = "openCmbBoxOnTab";
                    //Changes w.r.t. UAT 627 - Fill in "United States" as default selection for Country dropdowns. 
                    var listOfCountries = Presenter.GetListOFCountries();
                    if (listOfCountries.IsNotNull())
                    {
                        dropDownListCountryCitizen.DataSource = listOfCountries;
                        dropDownListCountryCitizen.DataBind();
                        var CountryUSA = listOfCountries.FirstOrDefault(x => x.CountryID == AppConsts.COUNTRY_USA_ID);
                        if (CountryUSA.IsNotNull())
                        {
                            dropDownListCountryCitizen.SelectedValue = CountryUSA.FullName;
                        }
                    }
                    if (IsEdit)
                    {
                        var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, dropDownListCountryCitizen, attributesForCustomForm);
                        dropDownListCountryCitizen = dataSet as WclComboBox;
                    }
                    controlDiv.Controls.Add(dropDownListCountryCitizen);
                    if (attributesForCustomForm.IsRequired)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownListCountryCitizen.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "drop" + attributesForCustomForm.AttributeType, currentInstanceId));
                    }
                    break;
                default:

                    break;
            }
            return controlDiv;
        }

        private void SetTheStateControl(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, HtmlGenericControl controlDiv)
        {
            WclComboBox dropDownListState = new WclComboBox();
            dropDownListState.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            dropDownListState.CssClass = "JState";
            dropDownListState.OnClientSelectedIndexChanged = "GetDataForDropDown";
            dropDownListState.Enabled = true;
            dropDownListState.Style.Add("display", "block");
            dropDownListState.EmptyMessage = "-- SELECT --";
            dropDownListState.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
            dropDownListState.OnClientKeyPressing = "openCmbBoxOnTab";

            dropDownListState.EnableTextSelection = true;
            dropDownListState.MarkFirstMatch = true;
            dropDownListState.OnClientBlur = "onLocationBlur";

            WclTextBox textBoxState = new WclTextBox();
            textBoxState.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            textBoxState.Style.Add("display", "block");
            textBoxState.CssClass = "txtState classTxt";

            if (IsEdit && _lstGroupIdNames.Count > 0)
            {
                StateName = String.Empty;
                if (_lstGroupIdNames.ContainsKey(attributesForCustomForm.AtrributeGroupMappingId) && !_lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty())
                {
                    StateName = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                }
                if (lstState.IsNull())
                {
                    CountryName = CountryName.IsNullOrEmpty() ? "UNITED STATES" : CountryName;
                    lstState = Presenter.GetListOfState(CountryName);
                }
                if (!CountryName.Equals(String.Empty) && (lstState.IsNullOrEmpty() || (!lstState.IsNullOrEmpty() && !lstState.Any(x => x.StateName.Equals(StateName)) && !StateName.Equals(String.Empty))))
                {
                    SetHiddenDropDownValues(attributesForCustomForm, currentInstanceId);
                    var dataSet = SetTheValueForControlInEditMode("Text", textBoxState, attributesForCustomForm);
                    textBoxState = dataSet as WclTextBox;
                }
                else
                {
                    dropDownListState.DataSource = lstState;
                    dropDownListState.DataTextField = "StateName";
                    dropDownListState.DataValueField = "StateName";
                    dropDownListState.DataBind();
                    var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, dropDownListState, attributesForCustomForm);
                    dropDownListState = dataSet as WclComboBox;
                    SetHiddenTextBoxValues(attributesForCustomForm, currentInstanceId);
                }

            }
            else if (!HasCountry)
            {
                if (lstState.IsNull())
                    lstState = Presenter.GetListOfState("UNITED STATES");
                dropDownListState.DataSource = lstState;
                dropDownListState.DataTextField = "StateName";
                dropDownListState.DataValueField = "StateName";
                dropDownListState.DataBind();
                dropDownListState.OnClientSelectedIndexChanged = "GetDataForDropDownForSupplemental";
            }
            //Added else condition w.r.t. UAT 627 - Fill in "United States" as default selection for Country dropdowns.
            else
            {
                if (lstState.IsNull())
                    lstState = Presenter.GetListOfState("UNITED STATES");
                dropDownListState.DataSource = lstState;
                dropDownListState.DataTextField = "StateName";
                dropDownListState.DataValueField = "StateName";
                dropDownListState.DataBind();
            }

            controlDiv.Controls.Add(dropDownListState);
            controlDiv.Controls.Add(textBoxState);
            if (attributesForCustomForm.IsRequired)
            {
                controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownListState.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "drop" + attributesForCustomForm.AttributeType, currentInstanceId));
                controlDiv.Controls.Add(SetrequiredFieldValidator(textBoxState.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "txt" + attributesForCustomForm.AttributeType, currentInstanceId, false));
            }
        }

        private void SetTheCityControl(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, HtmlGenericControl controlDiv)
        {
            WclComboBox dropDownListCity = new WclComboBox();
            dropDownListCity.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            dropDownListCity.CssClass = "JCity";
            dropDownListCity.Style.Add("display", "block");
            dropDownListCity.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
            dropDownListCity.OnClientKeyPressing = "openCmbBoxOnTab";
            if (IsSupplementalOrder)
                dropDownListCity.OnClientSelectedIndexChanged = "GetDataForDropDownForSupplemental";
            else
                dropDownListCity.OnClientSelectedIndexChanged = "GetDataForDropDown";
            dropDownListCity.Enabled = true;
            dropDownListCity.EmptyMessage = "-- SELECT --";

            dropDownListCity.EnableTextSelection = true;
            dropDownListCity.MarkFirstMatch = true;
            dropDownListCity.OnClientBlur = "onLocationBlur";

            WclTextBox textBoxCity = new WclTextBox();
            textBoxCity.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            textBoxCity.Style.Add("display", "block");
            textBoxCity.CssClass = "txtCity classTxt";
            if (IsEdit && _lstGroupIdNames.Count > 0)
            {
                CityName = String.Empty;
                if (!_lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty())
                {
                    CityName = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                }
                if (lstCity.IsNull())
                    lstCity = Presenter.GetListOfCity(StateName, CountryName);
                if (CheckForTheCityView())
                {
                    SetHiddenDropDownValues(attributesForCustomForm, currentInstanceId);
                    var dataSet = SetTheValueForControlInEditMode("Text", textBoxCity, attributesForCustomForm);
                    textBoxCity = dataSet as WclTextBox;
                }
                else
                {
                    dropDownListCity.DataSource = lstCity;
                    dropDownListCity.DataTextField = "CityName";
                    dropDownListCity.DataValueField = "CityName";
                    dropDownListCity.DataBind();
                    var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, dropDownListCity, attributesForCustomForm);
                    dropDownListCity = dataSet as WclComboBox;
                    SetHiddenTextBoxValues(attributesForCustomForm, currentInstanceId);
                }

            }
            if (!IsSupplementalOrder || (IsSupplementalOrder && HasState))
            {
                controlDiv.Controls.Add(dropDownListCity);
                controlDiv.Controls.Add(textBoxCity);

                if (attributesForCustomForm.IsRequired)
                {
                    controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownListCity.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "drop" + attributesForCustomForm.AttributeType, currentInstanceId));
                    controlDiv.Controls.Add(SetrequiredFieldValidator(textBoxCity.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "txt" + attributesForCustomForm.AttributeType, currentInstanceId, false));
                }
            }
        }

        private void SetTheZipCodeControl(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, HtmlGenericControl controlDiv)
        {
            WclComboBox dropDownListZip = new WclComboBox();
            dropDownListZip.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            dropDownListZip.CssClass = "JZipCode";
            dropDownListZip.Enabled = true;
            dropDownListZip.Style.Add("display", "block");
            dropDownListZip.EmptyMessage = "-- SELECT --";
            dropDownListZip.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
            dropDownListZip.OnClientKeyPressing = "openCmbBoxOnTab";
            if (IsSupplementalOrder)
                dropDownListZip.OnClientSelectedIndexChanged = "GetDataForDropDownForSupplemental";
            else
                dropDownListZip.OnClientSelectedIndexChanged = "GetDataForDropDown";


            dropDownListZip.EnableTextSelection = true;
            dropDownListZip.MarkFirstMatch = true;
            dropDownListZip.OnClientBlur = "onLocationBlur";

            WclTextBox textBoxZip = new WclTextBox();
            textBoxZip.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            textBoxZip.Style.Add("display", "block");
            textBoxZip.CssClass = "txtZipCode classTxt";
            if (IsEdit && _lstGroupIdNames.Count > 0)
            {
                ZipCodeName = String.Empty;
                if (!_lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty())
                {
                    ZipCodeName = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                }
                if (lstZipCode.IsNull())
                    lstZipCode = Presenter.GetListOfZipCode(StateName, CityName);
                if (CheckForTheZipCodeView())
                {
                    SetHiddenDropDownValues(attributesForCustomForm, currentInstanceId);
                    var dataSet = SetTheValueForControlInEditMode("Text", textBoxZip, attributesForCustomForm);
                    textBoxZip = dataSet as WclTextBox;
                }
                else
                {
                    dropDownListZip.DataSource = lstZipCode;
                    dropDownListZip.DataTextField = "ZipCode1";
                    dropDownListZip.DataValueField = "ZipCode1";
                    dropDownListZip.DataBind();
                    var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, dropDownListZip, attributesForCustomForm);
                    dropDownListZip = dataSet as WclComboBox;
                    SetHiddenTextBoxValues(attributesForCustomForm, currentInstanceId);
                }

            }
            if (!IsSupplementalOrder || (IsSupplementalOrder && HasState))
            {
                controlDiv.Controls.Add(dropDownListZip);
                controlDiv.Controls.Add(textBoxZip);
                if (attributesForCustomForm.IsRequired)
                {
                    controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownListZip.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "dropZipCode", currentInstanceId));
                    controlDiv.Controls.Add(SetrequiredFieldValidator(textBoxZip.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "txtZipCode", currentInstanceId, false));

                }
            }
        }

        private void SetTheCountyControl(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, HtmlGenericControl controlDiv)
        {
            WclComboBox dropDownListCounty = new WclComboBox();
            dropDownListCounty.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            dropDownListCounty.CssClass = "JCounty";
            dropDownListCounty.Style.Add("display", "block");
            dropDownListCounty.Enabled = true;
            dropDownListCounty.EmptyMessage = "-- SELECT --";
            dropDownListCounty.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
            dropDownListCounty.OnClientKeyPressing = "openCmbBoxOnTab";

            dropDownListCounty.EnableTextSelection = true;
            dropDownListCounty.MarkFirstMatch = true;
            dropDownListCounty.OnClientBlur = "onLocationBlur";

            WclTextBox textBoxCounty = new WclTextBox();
            textBoxCounty.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            textBoxCounty.Style.Add("display", "block");
            textBoxCounty.CssClass = "txtCounty classTxt";
            if (IsEdit && _lstGroupIdNames.Count > 0)
            {
                String countyName = String.Empty;
                if (!_lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty())
                {
                    countyName = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                }
                if (lstCounty.IsNull())
                    lstCounty = Presenter.GetListOfCounty(ZipCodeName, CityName);
                if (CheckForTheCountyView(countyName))
                {
                    SetHiddenDropDownValues(attributesForCustomForm, currentInstanceId);
                    var dataSet = SetTheValueForControlInEditMode("Text", textBoxCounty, attributesForCustomForm);
                    textBoxCounty = dataSet as WclTextBox;
                }
                else
                {
                    dropDownListCounty.DataSource = lstCounty;
                    dropDownListCounty.DataTextField = "CountyName";
                    dropDownListCounty.DataValueField = "CountyName";
                    dropDownListCounty.DataBind();
                    var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, dropDownListCounty, attributesForCustomForm);
                    dropDownListCounty = dataSet as WclComboBox;
                    SetHiddenTextBoxValues(attributesForCustomForm, currentInstanceId);
                }

            }
            if (!IsSupplementalOrder || (IsSupplementalOrder && HasState))
            {

                controlDiv.Controls.Add(dropDownListCounty);
                controlDiv.Controls.Add(textBoxCounty);
                if (attributesForCustomForm.IsRequired)
                {
                    controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownListCounty.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "drop" + attributesForCustomForm.AttributeType, currentInstanceId));
                    controlDiv.Controls.Add(SetrequiredFieldValidator(textBoxCounty.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "txt" + attributesForCustomForm.AttributeType, currentInstanceId, false));
                }
            }
        }

        /// <summary>
        /// Check for the text box or drop down display for city
        /// in edit mode
        /// </summary>
        /// <returns></returns>
        private Boolean CheckForTheCityView()
        {
            if (lstCity.IsNull())
                lstCity = Presenter.GetListOfCity(StateName, CountryName);
            if (lstState.IsNull())
                lstState = Presenter.GetListOfState(CountryName);
            if (CountryName.Equals(String.Empty))
                return false;
            else if (lstCity.IsNullOrEmpty() && lstState.IsNullOrEmpty())
                return true;
            else if (lstCity.IsNullOrEmpty() && !lstState.IsNullOrEmpty() && !String.IsNullOrEmpty(StateName))
                return true;
            else if (!lstCity.IsNullOrEmpty() && !lstCity.Any(x => x.CityName.Equals(CityName)) && !String.IsNullOrEmpty(CityName))
                return true;
            return false;
        }

        /// <summary>
        /// Check for the text box or drop down display for ZipCode
        /// in edit mode
        /// </summary>
        /// <returns></returns>
        private Boolean CheckForTheZipCodeView()
        {
            if (lstZipCode.IsNull())
                lstZipCode = Presenter.GetListOfZipCode(StateName, CityName);
            if (lstCity.IsNull())
                lstCity = Presenter.GetListOfCity(StateName, CountryName);
            if (lstState.IsNull())
                lstState = Presenter.GetListOfState(CountryName);
            if (String.IsNullOrEmpty(CountryName))
                return false;
            else if (lstZipCode.IsNullOrEmpty() && (lstCity.IsNullOrEmpty() && !String.IsNullOrEmpty(StateName)))
                return true;
            else if (lstZipCode.IsNullOrEmpty() && (lstCity.IsNullOrEmpty() && String.IsNullOrEmpty(StateName) && lstState.IsNullOrEmpty()))
                return true;
            else if (lstZipCode.IsNullOrEmpty() && (!lstCity.IsNullOrEmpty() && !String.IsNullOrEmpty(CityName)))
                return true;
            else if (!lstZipCode.IsNullOrEmpty() && !lstZipCode.Any(x => x.ZipCode1.Equals(ZipCodeName)) && !String.IsNullOrEmpty(ZipCodeName))
                return true;
            return false;
        }

        /// <summary>
        /// Check for the text box or drop down display for county
        /// in edit mode
        /// </summary>
        /// <returns></returns>
        private Boolean CheckForTheCountyView(String countyName)
        {
            if (lstCounty.IsNull())
                lstCounty = Presenter.GetListOfCounty(ZipCodeName, CityName);
            if (lstZipCode.IsNull())
                lstZipCode = Presenter.GetListOfZipCode(StateName, CityName);
            if (lstCity.IsNull())
                lstCity = Presenter.GetListOfCity(StateName, CountryName);
            if (lstState.IsNull())
                lstState = Presenter.GetListOfState(CountryName);
            if (CountryName.Equals(String.Empty))
                return false;
            else if (lstCounty.IsNullOrEmpty() && (!lstZipCode.IsNullOrEmpty() && !String.IsNullOrEmpty(ZipCodeName)))
                return true;
            else if (lstCounty.IsNullOrEmpty() && (lstZipCode.IsNullOrEmpty() && !String.IsNullOrEmpty(ZipCodeName)))
                return true;
            else if (lstCounty.IsNullOrEmpty() && (lstZipCode.IsNullOrEmpty() && lstCity.IsNullOrEmpty() && !String.IsNullOrEmpty(CityName)))
                return true;
            else if (lstCounty.IsNullOrEmpty() && (lstZipCode.IsNullOrEmpty() && lstCity.IsNullOrEmpty() && String.IsNullOrEmpty(CityName) && lstState.IsNullOrEmpty() && !String.IsNullOrEmpty(StateName)))
                return true;
            else if (lstCounty.IsNullOrEmpty() && (lstZipCode.IsNullOrEmpty() && lstCity.IsNullOrEmpty() && String.IsNullOrEmpty(CityName) && lstState.IsNullOrEmpty() && String.IsNullOrEmpty(StateName)))
                return true;
            else if (!lstCounty.IsNullOrEmpty() && !lstCounty.Any(x => x.CountyName.Equals(countyName)) && !String.IsNullOrEmpty(countyName))
                return true;
            return false;
        }

        /// <summary>
        /// Need to set the value in hidden field for the jquery implementation
        /// This only fcor edit mopde. It setr the value for drop downs to be hidden
        /// </summary>
        /// <param name="attributesForCustomForm"></param>
        /// <param name="currentInstanceId"></param>
        private void SetHiddenDropDownValues(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId)
        {
            String id = currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            Boolean IsValueToBeAdded = true;
            if (!hdnHiddenTextBoxIds.Value.IsNullOrEmpty())
            {
                String[] hiddenValues = hdnHiddenTextBoxIds.Value.Split(':');
                if (hiddenValues.Count() == 0 && hdnHiddenTextBoxIds.Value.Equals(id))
                {
                    IsValueToBeAdded = false;
                }
                else if (hiddenValues.Contains(id))
                {
                    IsValueToBeAdded = false;
                }

            }
            if (IsValueToBeAdded)
            {
                if (hdnHiddenDropDownIds.Value.IsNullOrEmpty())
                {
                    hdnHiddenDropDownIds.Value = id;
                }
                else
                {
                    hdnHiddenDropDownIds.Value = hdnHiddenDropDownIds.Value + ":" + id;
                }
            }
        }

        /// <summary>
        /// Need to set the value in hidden field for the jquery implementation
        /// This only fcor edit mopde. It setr the value for text boxes to be hidden
        /// </summary>
        /// <param name="attributesForCustomForm"></param>
        /// <param name="currentInstanceId"></param>
        private void SetHiddenTextBoxValues(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId)
        {
            String id = currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            Boolean IsValueToBeAdded = true;
            if (!hdnHiddenDropDownIds.Value.IsNullOrEmpty())
            {
                String[] hiddenValues = hdnHiddenDropDownIds.Value.Split(':');
                if (hiddenValues.Count() == 0 && hdnHiddenDropDownIds.Value.Equals(id))
                {
                    IsValueToBeAdded = false;
                }
                else if (hiddenValues.Contains(id))
                {
                    IsValueToBeAdded = false;
                }
            }
            if (IsValueToBeAdded)
            {
                if (hdnHiddenTextBoxIds.Value.IsNullOrEmpty())
                {
                    hdnHiddenTextBoxIds.Value = id;
                }
                else
                {
                    hdnHiddenTextBoxIds.Value = hdnHiddenTextBoxIds.Value + ":" + id;
                }
            }
        }

        /// <summary>
        /// This method set the value in control in edit mode.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="control"></param>
        /// <param name="attributesForCustomForm"></param>
        /// <returns></returns>
        private Control SetTheValueForControlInEditMode(String type, Control control, AttributesForCustomFormContract attributesForCustomForm)
        {
            switch (type)
            {
                case "Text":
                    if (attributesForCustomForm.AttributeCode.ToUpper().Equals("4E12CF9A-A4AF-4A38-B4BB-47E6059259F4"))
                    {
                        WclMaskedTextBox mastTextBox = control as WclMaskedTextBox;
                        if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                            mastTextBox.Text = String.Empty;
                        else
                            mastTextBox.Text = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                    }
                    else
                    {
                        WclTextBox textBox = control as WclTextBox;
                        if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                            textBox.Text = String.Empty;
                        else
                            textBox.Text = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                    }
                    break;
                case "Numeric":
                    WclNumericTextBox numericTextBox = new WclNumericTextBox();
                    if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                        numericTextBox.Text = String.Empty;
                    else
                        numericTextBox.Text = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                    break;
                case "Date":
                    WclDatePicker datePicker = control as WclDatePicker;
                    if ((_lstGroupIdNames.Count > 0 && !_lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                        datePicker.SelectedDate = Convert.ToDateTime(_lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId]).Date;
                    break;
                case "Option":
                case "County":
                    WclComboBox dropDownList = control as WclComboBox;
                    if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                    {
                        dropDownList.SelectedValue = String.Empty;
                    }
                    else
                    {
                        dropDownList.SelectedValue = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                    }
                    break;
                case "Country":
                    WclComboBox countryDropDownList = control as WclComboBox;
                    if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                    {
                        //Code changes w.r.t. UAT 627 - Fill in "United States" as default selection for Country dropdowns.
                        //countryDropDownList.SelectedValue = String.Empty;
                        //CountryName = String.Empty;
                        countryDropDownList.SelectedValue = "UNITED STATES";
                        CountryName = "UNITED STATES";
                    }
                    else
                    {
                        countryDropDownList.SelectedValue = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                        CountryName = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                    }
                    break;
                case "State":
                    WclComboBox stateDropDownList = control as WclComboBox;
                    if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                    {
                        stateDropDownList.SelectedValue = String.Empty;
                        StateName = String.Empty;
                    }
                    else
                    {
                        stateDropDownList.SelectedValue = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                        StateName = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                    }
                    break;
                case "City":
                    WclComboBox CityDropDownList = control as WclComboBox;
                    if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                    {
                        CityDropDownList.SelectedValue = String.Empty;
                        CityName = String.Empty;
                    }
                    else
                    {
                        CityDropDownList.SelectedValue = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                        CityName = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                    }
                    break;
                case "Zip Code":
                    WclComboBox ZipCodeDropDownList = control as WclComboBox;
                    if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                    {
                        ZipCodeDropDownList.SelectedValue = String.Empty;
                        ZipCodeName = String.Empty;
                    }
                    else
                    {
                        ZipCodeDropDownList.SelectedValue = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                        ZipCodeName = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                    }
                    break;
                default:
                    break;
            }
            return control;
        }

        /// <summary>
        /// It Create html for the read only mode
        /// </summary>
        /// <param name="attributesForCustomForm"></param>
        /// <param name="currentInstanceId"></param>
        /// <returns></returns>
        private HtmlGenericControl CreateControlForReadOnlyMode(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, HtmlGenericControl parentControl)
        {
            //HtmlGenericControl lableDiv = new HtmlGenericControl("div");
            //lableDiv.Attributes.Add("class", "sxlb");
            HtmlGenericControl controlsContainer = GenerateColumnNumbersView();

            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes.Add("class", "cptn");
            //UAT 4730
            span.InnerHtml = attributesForCustomForm.DisplayName.IsNullOrEmpty() ? attributesForCustomForm.AttributeName : attributesForCustomForm.DisplayName;
            //lableDiv.Controls.Add(span);
            //HtmlGenericControl controlDiv = new HtmlGenericControl("div");
            //controlDiv.Attributes.Add("class", "sxlm");
            Label controlLable = new Label();
            controlLable.ID = "lbl" + attributesForCustomForm.AttributeType + "_" + attributesForCustomForm.AttributeGroupId + "_" + currentInstanceId.ToString() + "_" + attributesForCustomForm.AttributeId;
            controlLable.CssClass = "form-control";//"ronly";
            if (_lstGroupIdNames.ContainsKey(attributesForCustomForm.AtrributeGroupMappingId))
            {
                if (_lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty())
                    controlLable.Text = String.Empty;
                else
                {
                    if (attributesForCustomForm.AttributeCode.ToUpper().Equals("4E12CF9A-A4AF-4A38-B4BB-47E6059259F4"))
                        controlLable.Text = Presenter.GetFormattedPhoneNumber(_lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId]);
                    else
                        controlLable.Text = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                }
            }
            else
            {
                controlLable.Text = String.Empty;
            }
            //controlDiv.Controls.Add(controlLable);
            controlsContainer.Controls.Add(span);
            controlsContainer.Controls.Add(controlLable);
            parentControl.Controls.Add(controlsContainer);
            return parentControl;
        }

        private HtmlGenericControl CreateControlForOrderConfirmation(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, HtmlGenericControl parentControl)
        {
            HtmlGenericControl mainDiv = new HtmlGenericControl("div");
            mainDiv.Attributes.Add("class", "customDivStyle");
            HtmlGenericControl lableSpan = new HtmlGenericControl("span");
            //UAT 4730
            lableSpan.InnerHtml = attributesForCustomForm.DisplayName.IsNullOrEmpty() ? attributesForCustomForm.AttributeName : attributesForCustomForm.DisplayName + ":&nbsp;";
            mainDiv.Controls.Add(lableSpan);
            HtmlGenericControl dataSpan = new HtmlGenericControl("span");
            dataSpan.Attributes.Add("class", "boldFont");
            Label dataLabel = new Label();
            dataLabel.ID = "lbl" + attributesForCustomForm.AttributeType + "_" + attributesForCustomForm.AttributeGroupId + "_" + currentInstanceId.ToString() + "_" + attributesForCustomForm.AttributeId;
            if (_lstGroupIdNames.ContainsKey(attributesForCustomForm.AtrributeGroupMappingId))
            {
                if (_lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty())
                    dataLabel.Text = String.Empty;
                else
                {
                    if (attributesForCustomForm.AttributeCode.ToUpper().Equals("4E12CF9A-A4AF-4A38-B4BB-47E6059259F4"))
                        dataLabel.Text = Presenter.GetFormattedPhoneNumber(_lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId]);
                    else
                        dataLabel.Text = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                }
            }
            else
            {
                dataLabel.Text = String.Empty;
            }
            dataSpan.Controls.Add(dataLabel);
            mainDiv.Controls.Add(dataSpan);
            parentControl.Controls.Add(mainDiv);
            return parentControl;
        }

        /// <summary>
        /// For the control that are required 
        /// is required sign is display.
        /// </summary>
        /// <returns></returns>
        private HtmlGenericControl AddRequiredSign(Int32 instanceId)
        {
            //<span class="reqd">*</span>
            HtmlGenericControl span = new HtmlGenericControl("span");
            //span.ID = groupId + "_" + InstanceId;
            span.Attributes.Add("class", "reqd" + " " + groupId + "_" + instanceId);
            span.InnerHtml = "*";
            return span;

        }

        /// <summary>
        /// Add a required field if required.
        /// </summary>
        /// <param name="controlTovalidate">Control on which the require  field is to be applied</param>
        /// <param name="AttributeType"></param>
        /// <returns></returns>
        private HtmlGenericControl SetrequiredFieldValidator(String controlTovalidate, String attributeName, Int32 AtrributeGroupMappingId, String AttributeType, Int32 instanceId, Boolean enable = true)
        {
            HtmlGenericControl requiredFieldsDiv = new HtmlGenericControl("div");
            requiredFieldsDiv.Attributes.Add("class", "vldx");
            //groupId
            RequiredFieldValidator requiredField = new RequiredFieldValidator();
            requiredField.ID = "rfv_" + AttributeType.RemoveWhitespace() + "_" + instanceId.ToString() + "_" + AtrributeGroupMappingId.ToString();
            requiredField.ControlToValidate = controlTovalidate;
            requiredField.ErrorMessage = "Please enter " + attributeName;
            requiredField.ValidationGroup = "submitForm" + groupId.ToString();
            requiredField.CssClass = "errmsg";
            requiredField.Enabled = enable;
            requiredField.Display = ValidatorDisplay.Dynamic;// "Dynamic";
            requiredFieldsDiv.Controls.Add(requiredField);
            return requiredFieldsDiv;
        }

        /// <summary>
        /// Recursive function to find the control dynamically added
        /// </summary>
        /// <param name="control">Initial control</param>
        /// <param name="attributeDetails">sdictionary with the value of attribute and it i d.</param>
        private void FindControlRecursiveForCustomForms(Control control, Dictionary<Int32, String> attributeDetails)
        {
            //As the WclDatePicker and the WclComboBox is a collection of controls, so trhis check avoid going futher in the control and gets the actual value.
            if (control != null && control.Controls.Count > 0 && !(control.GetType().Name.ToString().Equals("WclDatePicker") || control.GetType().Name.ToString().Equals("WclComboBox")))
            {
                foreach (Control cntrl in control.Controls)
                {
                    FindControlRecursiveForCustomForms(cntrl, attributeDetails);
                }
            }
            //For more cobntrol just add the OR condition.
            else if (control.GetType().Name.ToString().Equals("WclTextBox") || control.GetType().Name.ToString().Equals("WclNumericTextBox")
                || control.GetType().Name.ToString().Equals("WclDatePicker") || control.GetType().Name.ToString().Equals("WclMaskedTextBox") ||
                control.GetType().Name.ToString().Equals("WclComboBox"))
            {
                SetValue(control, attributeDetails);
            }
        }

        /// <summary>
        /// Gets the value of the control depending
        /// on their type and set it to dictionary.
        /// </summary>
        /// <param name="control">Control for which we have to get the value</param>
        /// <param name="attributeDetails"></param>
        private void SetValue(Control control, Dictionary<Int32, String> attributeDetails)
        {
            String controlName = control.GetType().Name.ToString();
            Int32 attributeMappingId = 0;
            switch (controlName)
            {
                case "WclTextBox":
                    WclTextBox textBox = control as WclTextBox;
                    String hiddenIdTextBox = GetHiddenId(textBox.ID);
                    if (GetTextBoxIds.IsNull() || (!GetTextBoxIds.IsNull() && !GetTextBoxIds.Contains(hiddenIdTextBox)))
                    {
                        attributeMappingId = GetAttributeMappingId(textBox.ID);
                        if (attributeMappingId != 0)
                        {
                            attributeDetails.Add(attributeMappingId, textBox.Text);
                        }
                    }
                    break;
                case "WclComboBox":
                    WclComboBox dropDownList = control as WclComboBox;
                    String hiddenIdDropDown = GetHiddenId(dropDownList.ID);
                    if (GetDropDownIds.IsNull() || (!GetDropDownIds.IsNull() && !GetDropDownIds.Contains(hiddenIdDropDown)))
                    {
                        attributeMappingId = GetAttributeMappingId(dropDownList.ID);
                        if (attributeMappingId != 0)
                        {
                            attributeDetails.Add(attributeMappingId, dropDownList.SelectedValue);
                        }
                    }
                    break;
                case "WclNumericTextBox":
                    WclNumericTextBox numericTextBox = control as WclNumericTextBox;
                    attributeMappingId = GetAttributeMappingId(numericTextBox.ID);
                    if (attributeMappingId != 0)
                    {
                        attributeDetails.Add(attributeMappingId, numericTextBox.Text);
                    }
                    break;
                case "WclDatePicker":
                    WclDatePicker datePicker = control as WclDatePicker;

                    attributeMappingId = GetAttributeMappingId(datePicker.ID);
                    if (attributeMappingId != 0)
                    {
                        String date = String.Empty;
                        if (!datePicker.SelectedDate.IsNullOrEmpty())
                        {
                            date = Convert.ToDateTime(datePicker.SelectedDate).ToShortDateString();
                        }
                        attributeDetails.Add(attributeMappingId, date);
                    }
                    break;
                case "WclMaskedTextBox":
                    WclMaskedTextBox maskedTextBox = control as WclMaskedTextBox;

                    attributeMappingId = GetAttributeMappingId(maskedTextBox.ID);
                    if (attributeMappingId != 0)
                    {
                        attributeDetails.Add(attributeMappingId, maskedTextBox.Text);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets the attribute nid from the control id.
        /// </summary>
        /// <param name="controlId"></param>
        /// <returns></returns>
        private Int32 GetAttributeMappingId(String controlId)
        {

            String[] data = controlId.Split('_');
            Int32 count = data.Count();
            if (count > 0)
            {
                return Convert.ToInt32(data[count - 1]);
            }
            return 0;
        }

        /// <summary>
        /// Get the id for matching the hidden controls
        /// </summary>
        /// <param name="controlId"></param>
        /// <returns></returns>
        private String GetHiddenId(String controlId)
        {
            String hiddenId = String.Empty;
            String[] arrayOFIds = controlId.Split('_');
            Int32 count = arrayOFIds.Count();
            for (Int32 i = 3; i > 0; i--)
            {
                if (hiddenId.Equals(String.Empty))
                {
                    hiddenId = arrayOFIds[count - i];
                }
                else
                {
                    hiddenId = hiddenId + "_" + arrayOFIds[count - i];
                }
            }
            return hiddenId;
        }

        private void AddEdsDocumentLinkInPanel(Control mainPanelForFor)
        {
            HtmlGenericControl divCol_12 = new HtmlGenericControl("div");
            //divCol_12.Attributes.Add("class", "col-md-12");
            HtmlGenericControl divbgLoghtGreen = new HtmlGenericControl("div");
            divbgLoghtGreen.Attributes.Add("class", "row bgLightGreen");
            HtmlGenericControl divCol_3 = new HtmlGenericControl("div");
            divCol_3.Attributes.Add("class", "form-group col-md-3");

            HtmlGenericControl divEDSDocumentLink = new HtmlGenericControl("div");
            divEDSDocumentLink.ID = "divEDSDocumentLink";
            divEDSDocumentLink.Visible = true;
            divCol_3.Controls.Add(divEDSDocumentLink);

            Label lblEdsDocument = new Label();
            lblEdsDocument.ID = "lblEdsDocument";
            lblEdsDocument.Text = "Authorization Form";
            lblEdsDocument.Attributes.Add("class", "cptn");
            HyperLink lnkEDSDocument = new HyperLink();
            lnkEDSDocument = pnlRendercustomForm.FindControl("lnkEDSDocumentTemp") as HyperLink;
            lnkEDSDocument.Visible = true;
            lnkEDSDocument.CssClass = "form-control blueText";
            divEDSDocumentLink.Controls.Add(lblEdsDocument);
            divEDSDocumentLink.Controls.Add(lnkEDSDocument);
            divCol_3.Controls.Add(divEDSDocumentLink);
            hdfOrderID.Value = Convert.ToString(OrderId);
            hdfDocumentType.Value = "EDS_AuthorizationForm";
            hdfTenantId.Value = Convert.ToString(tenantId);
            divbgLoghtGreen.Controls.Add(divCol_3);
            divCol_12.Controls.Add(divbgLoghtGreen);
            mainPanelForFor.Controls.Add(divCol_12);
        }

        #region UAT-586

        #endregion

        #endregion

        #region Public Methods
        /// <summary>
        /// Function called from the main page.
        /// Depending upon the instance it gets the value 
        /// of the control in the particular instance.
        /// </summary>
        /// <returns></returns>
        public List<BackgroundOrderData> GetData()
        {
            List<BackgroundOrderData> lstResult = new List<BackgroundOrderData>();
            if (InstanceId > 0)
            {
                Int32 count = 1;
                for (int grp = 1; grp <= InstanceId; grp++)
                {
                    if (lstHiddenInstanceOfGroup.IsNull() || (lstHiddenInstanceOfGroup.IsNotNull() && !lstHiddenInstanceOfGroup.Contains(grp)))
                    {
                        Panel pnlRendercustomFormMe = pnlRendercustomForm.FindControl("pnl_" + grp + "_" + hdnGroupId.Value) as Panel;
                        if (pnlRendercustomFormMe != null)
                        {
                            BackgroundOrderData backgroundOrderData = new BackgroundOrderData();
                            backgroundOrderData.InstanceId = count;
                            backgroundOrderData.CustomFormId = CustomFormId;
                            backgroundOrderData.BkgSvcAttributeGroupId = Convert.ToInt32(hdnGroupId.Value);
                            backgroundOrderData.CustomFormData = new Dictionary<int, string>();
                            FindControlRecursiveForCustomForms(pnlRendercustomFormMe, backgroundOrderData.CustomFormData);
                            lstResult.Add(backgroundOrderData);
                            count++;
                        }
                    }
                }
            }
            return lstResult;
        }

        /// <summary>
        /// Function called from the main page.
        /// Depending upon the instance it gets the value 
        /// of the control in the particular instance.
        /// </summary>
        /// <returns></returns>
        public List<SupplementOrderData> GetDataForServiceItem()
        {
            List<SupplementOrderData> lstResult = new List<SupplementOrderData>();
            if (InstanceId > 0)
            {
                Int32 count = 1;
                for (int grp = 1; grp <= InstanceId; grp++)
                {
                    if (lstHiddenInstanceOfGroup.IsNull() || (lstHiddenInstanceOfGroup.IsNotNull() && !lstHiddenInstanceOfGroup.Contains(grp)))
                    {
                        Panel pnlRendercustomFormMe = pnlRendercustomForm.FindControl("pnl_" + grp + "_" + hdnGroupId.Value) as Panel;
                        if (pnlRendercustomFormMe != null)
                        {
                            SupplementOrderData supplementOrderData = new SupplementOrderData();
                            supplementOrderData.InstanceId = count;
                            supplementOrderData.BkgServiceId = ServiceID;
                            supplementOrderData.PackageServiceId = PackageServiceId;
                            supplementOrderData.CustomFormId = CustomFormId;
                            supplementOrderData.PackageSvcItemId = ServiceItemID;
                            supplementOrderData.BkgSvcAttributeGroupId = Convert.ToInt32(hdnGroupId.Value);
                            supplementOrderData.FormData = new Dictionary<int, string>();
                            FindControlRecursiveForCustomForms(pnlRendercustomFormMe, supplementOrderData.FormData);
                            lstResult.Add(supplementOrderData);
                            count++;
                        }
                    }
                }
            }
            return lstResult;
        }

        #endregion

        #endregion


    }
}