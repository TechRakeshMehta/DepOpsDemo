using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class AdminEntryCustomFormHtml : BaseUserControl, IAdminEntryCustomFormHtmlView
    {
        #region Variables

        #region Private Variables

        private AdminEntryCustomFormHtmlPresenter _presenter = new AdminEntryCustomFormHtmlPresenter();
        private ApplicantOrderCart applicantOrderCart = null;
        Control mainPanelForFor = null;

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
        //UAT-2447:
        private Dictionary<Int32, String> _lstIntPhoneNumberExtraData = new Dictionary<Int32, String>();
        private Boolean HasState { get; set; }
        private Boolean HasCountry { get; set; }

        //UAT-2062:System to determine and add additional searches in supplement (SSN Trace)
        private Boolean _IsPopulateAliasForAddiSearch = false;
        private Boolean _IsPopulateStateAndCountyForAddiSearch = false;

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

        private static List<string> countrySortPreference;

        #endregion

        #region Public Properties

        public AdminEntryCustomFormHtmlPresenter Presenter
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

        public List<SystemSpecificLanguageText> SystemSpecificLanguageTextList
        {
            get
            {
                if (ViewState["SystemSpecificLanguageText"].IsNullOrEmpty())
                    return new List<SystemSpecificLanguageText>();
                return (List<SystemSpecificLanguageText>)ViewState["SystemSpecificLanguageText"];
            }
            set
            {
                ViewState["SystemSpecificLanguageText"] = value;
            }

        }

        public IAdminEntryCustomFormHtmlView CurrentViewContext
        {
            get
            {
                return this;
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
        Dictionary<Int32, String> lstGroupIdNames
        {
            get;
            set;
        }

        #region UAT-2062:System to determine and add additional searches in supplement (SSN Trace)
        public List<SupplementAdditionalSearchContract> LstSupplementAdditionalSearchDataForName { get; set; }
        public List<SupplementAdditionalSearchContract> LstSupplementAdditionalSearchDataForLocation { get; set; }
        #endregion
        //UAT-2063:Combine the screens to add new Alias and add new locations
        public Int32 AutoFocusGroupID { get; set; }

        #region UAT-2885
        public Boolean IsAdminOrderScreen
        {
            get
            {
                if (ViewState["IsAdminOrderScreen"].IsNullOrEmpty())
                    return false;
                return Convert.ToBoolean(ViewState["IsAdminOrderScreen"]);
            }
            set
            {
                ViewState["IsAdminOrderScreen"] = value;
            }
        }
        #endregion

        public Boolean IsLocationServiceTenant
        {
            get
            {
                return hdnIsLocationServiceTenant.Value.IsNullOrEmpty() ? false : Convert.ToBoolean(hdnIsLocationServiceTenant.Value);
            }
            set
            {
                hdnIsLocationServiceTenant.Value = Convert.ToString(value);
            }
        }

        public Dictionary<Int32, String> lstAutoFillCascadingAttributes
        {
            get
            {
                if (ViewState["lstAutoFillCascadingAttributes"].IsNullOrEmpty())
                    return new Dictionary<Int32, String>();
                return (Dictionary<Int32, String>)ViewState["lstAutoFillCascadingAttributes"];
            }
            set
            {
                ViewState["lstAutoFillCascadingAttributes"] = value;
            }
        }

        public Dictionary<String, String> lstControlIds
        {
            get
            {
                if (ViewState["lstControlIds"].IsNullOrEmpty())
                    return new Dictionary<String, String>();
                return (Dictionary<String, String>)ViewState["lstControlIds"];
            }
            set
            {
                ViewState["lstControlIds"] = value;
            }
        }
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {            
            GetHtmLCreated();
            if (IsReadOnly)
            {
                if (ShowEditDetailButton)
                {
                    fsucCmdBar1.Visible = false;                  
                }
                else
                {
                    fsucCmdBar1.Visible = false;                    
                }
                if (IsAdminOrderScreen)
                {
                    cmdbarEdit.OnClientClicked = "ShowProgressBar";
                }
            }
            //else if ((occurence < 2))
            //{
            //    CommandBar.Style.Add("display", "none");
            //}
            //Added (MaximumOccurence != AppConsts.MINUS_ONE) only for supplement order data because in supplement order only we set Maximum occurrence to minus one-
            //If maximum occurrence is null [UAT-1571:WB: increase the count of Alias and Location inputs to 25. I still don’t want this to be open ended.]
            else if ((occurence < 2) || (MaximumOccurence != AppConsts.MINUS_ONE && ((MaximumOccurence == CurrentInstanceIdForGroup - 1)
                                         || !(MaximumOccurence > CurrentInstanceIdForGroup - 1) || (MaximumOccurence == 0 && !occurence.IsNullOrEmpty() && occurence < 1)
                                         || IsOrderConfirmation)))
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
            //UAT-2074 : When I hit “Add New” button, I shouldn’t have to click into State (for location) or Alias boxes.
            if (IsSupplementalOrder)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "doFocusTextBox();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "doFocusComboBox();", true);
            }
        }

        private String GetHREFForTheEditButton()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {                                                                      
                                                                    { AppConsts.CHILD, ChildControls.AdminEntryCustomFormLoad},
                                                                    {"CustomFormId",hdnCurrentFormId.Value}

                                                                 };
            return String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
        }

        protected void CmdBarEditCustomForm_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            IsEdit = true;
            applicantOrderCart.EDrugScreeningRegistrationId = null;
            if (!IsAdminOrderScreen)
            {
                queryString = new Dictionary<String, String>
                                                                 {                                                                      
                                                                    { AppConsts.CHILD, ChildControls.AdminEntryCustomFormLoad},
                                                                    {"CustomFormId",hdnCurrentFormId.Value}

                                                                 };
                Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            }
            else
            {
                String url = String.Format("AdminEntryPortal/Pages/AdminEntryCustomFormPage.aspx?SelectedTenantId=" + Convert.ToString(tenantId) + "&IsAdminEditMode=false&IsNewCustomForm=false&CustomFormId=" + Convert.ToString(hdnCurrentFormId.Value) + "&IsPrevious=1");                
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenCustomForm('" + url + "');", true);
            }

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
                if (lstHiddenInstanceOfGroup.IsNull() || IsSupplementalOrder || (lstHiddenInstanceOfGroup.IsNotNull() && !lstHiddenInstanceOfGroup.Contains(i + 1)))
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
                    //UAT-2063:Combine the screens to add new Alias and add new locations
                    Boolean isAutoFocusRequired = AutoFocusGroupID == AppConsts.NONE ? true : (AutoFocusGroupID == groupId) ? true : false;
                    //UAT-2074 : When I hit “Add New” button, I shouldn’t have to click into State (for location) or Alias boxes.
                    Boolean isAutoFocus = (IsEdit || _IsPopulateStateAndCountyForAddiSearch || _IsPopulateAliasForAddiSearch || !isAutoFocusRequired) ? false : true;
                    if (i + 1 == instanceCount && IsSupplementalOrder)
                        GenerateSectionControlsForAGroup(groupId, isAutoFocus, i + 1);
                    else
                        GenerateSectionControlsForAGroup(groupId, false, i + 1);
                    fsucCmdBar1.SaveButton.CssClass = groupId.ToString();
                    CurrentInstanceIdForGroup++;
                    //UAT-2065: Additional searches should go in one grid for each type (not one for each additional search) with ability to delete.
                    if (mainPanelForFor.IsNotNull() && IsSupplementalOrder && lstHiddenInstanceOfGroup.IsNotNull() && lstHiddenInstanceOfGroup.Contains(i + 1))
                    {
                        var pnlInner = mainPanelForFor.FindControl("pnlInner_" + groupId + "_" + (i + 1));
                        if (pnlInner.IsNotNull())
                            pnlInner.Visible = false;
                    }
                }
            }
            //UAT-2447
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowHidePhoneOnLoad();", true);
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
                //UAT-2447
                _lstIntPhoneNumberExtraData = new Dictionary<Int32, String>();
            }
            else
            {
                _lstGroupIdNames = lstBackgroundOrderData.FirstOrDefault(x => x.BkgSvcAttributeGroupId == groupId && x.CustomFormId == CustomFormId && x.InstanceId == (instanceId + 1)).CustomFormData;
                //UAT-2447
                _lstIntPhoneNumberExtraData = lstBackgroundOrderData.FirstOrDefault(x => x.BkgSvcAttributeGroupId == groupId && x.CustomFormId == CustomFormId && x.InstanceId == (instanceId + 1)).CustomFormIntPhoneNumExtraData;
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
                //UAT-2062:System to determine and add additional searches in supplement (SSN Trace)
                String attGroupMappingCode_AliasName = ("56258C54-C2BC-4514-94E1-2EF2EFFFDBF5").ToLower();
                String attGroupMappingCode_State = ("CAEAC9FA-FFF4-4F7A-A644-96967F399362").ToLower();
                String attGroupMappingCode_County = ("C00AEFB5-37DF-44F7-A050-D2C9581909DE").ToLower();
                if (!LstSupplementAdditionalSearchDataForName.IsNullOrEmpty())
                {
                    //SupplementAdditionalSearchContract addSearchNameData = LstSupplementAdditionalSearchDataForName.FirstOrDefault(x => x.UsedByInstanceID == AppConsts.NONE);
                    SupplementAdditionalSearchContract addSearchNameData = LstSupplementAdditionalSearchDataForName[instanceId];
                    var attributes = lstCustomFormAttributes.FirstOrDefault(x => x.AttributeGroupId == groupId && x.AttributeGroupMappingCode == attGroupMappingCode_AliasName);
                    if (!attributes.IsNullOrEmpty() && !addSearchNameData.IsNullOrEmpty())
                    {
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        //String aliasName = addSearchNameData.FirstName + " " + addSearchNameData.MiddleName + " " + addSearchNameData.LastName;
                        //UAT-2309:Remove middle names only from supplement searches. We should only send the first and last names to clearstar with “——-“ 
                        //in the middle name field always. only one version of each first/last combo should be displayed and sent for supplement search.
                        String aliasName = addSearchNameData.FirstName + " " + addSearchNameData.LastName;
                        _lstGroupIdNames.Add(attributes.AtrributeGroupMappingId, aliasName);
                        //addSearchNameData.UsedByInstanceID = instanceId + 1;
                        _IsPopulateAliasForAddiSearch = true;
                    }

                }
                if (!LstSupplementAdditionalSearchDataForLocation.IsNullOrEmpty())
                {
                    //SupplementAdditionalSearchContract addSearchLocationData = LstSupplementAdditionalSearchDataForLocation.FirstOrDefault(x => x.UsedByInstanceID == AppConsts.NONE);
                    SupplementAdditionalSearchContract addSearchLocationData = LstSupplementAdditionalSearchDataForLocation[instanceId];
                    var attributeState = lstCustomFormAttributes.FirstOrDefault(x => x.AttributeGroupId == groupId && x.AttributeGroupMappingCode == attGroupMappingCode_State);
                    var attributeCounty = lstCustomFormAttributes.FirstOrDefault(x => x.AttributeGroupId == groupId && x.AttributeGroupMappingCode == attGroupMappingCode_County);
                    if (!attributeState.IsNullOrEmpty() && !attributeCounty.IsNullOrEmpty() && !addSearchLocationData.IsNullOrEmpty())
                    {
                        _lstGroupIdNames.Add(attributeState.AtrributeGroupMappingId, addSearchLocationData.StateName);
                        _lstGroupIdNames.Add(attributeCounty.AtrributeGroupMappingId, addSearchLocationData.CountyName);
                        //addSearchLocationData.UsedByInstanceID = instanceId + 1;
                        _IsPopulateStateAndCountyForAddiSearch = true;
                    }
                }
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
        private void GenerateSectionControlsForAGroup(Int32 groupId, Boolean IsAutoFoucs, Int32 CurrentInstanceId = 1)
        {
            var groupdata = lstCustomFormAttributes.FirstOrDefault(x => x.AttributeGroupId == groupId);
            if (lstCustomFormAttributes.Any(x => x.AttributeGroupId == groupId && x.IsDisplay))
            {
                SectionTitle = groupdata.SectionTitle.IsNullOrEmpty() ? "" : groupdata.SectionTitle;
                CustomHtml = groupdata.CustomHtml.IsNullOrEmpty() ? "" : groupdata.CustomHtml;
                InstructionText = groupdata.InstructionText.IsNullOrEmpty() ? "" : groupdata.InstructionText;
                DisplayColumns = groupdata.DisplayColumns.IsNullOrEmpty() ? 3 : groupdata.DisplayColumns;
                //occurence = groupdata.Occurence.IsNullOrEmpty() ? 1 : groupdata.Occurence;
                //Control mainPanelForFor = null;
                if (IsOrderConfirmation)
                {
                    mainPanelForFor = RenderSectionHtmlForOrderConfirmation(groupId, CurrentInstanceId);
                }
                else
                {
                    //UAT-2065: Additional searches should go in one grid for each type (not one for each additional search) with ability to delete.
                    if (IsSupplementalOrder)
                    {
                        if (CurrentInstanceId == AppConsts.ONE || mainPanelForFor.IsNull())
                            mainPanelForFor = RenderHeaderSectionHtml(groupId, CurrentInstanceId);
                    }
                    else
                    {
                        mainPanelForFor = RenderHeaderSectionHtml(groupId, CurrentInstanceId);
                    }
                }

                //Call To method to create the dynamic Html for a particular group
                GenerateFormForColumDisplay(groupId, DisplayColumns, mainPanelForFor, CurrentInstanceId, IsAutoFoucs);

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
            HtmlGenericControl section = new HtmlGenericControl("div");
            section.Attributes.Add("class", "section");

            //12/08/2014 Add "mhdr" class to Div rather than H1 to add edit button to div.
            HtmlGenericControl mhdrDiv = new HtmlGenericControl("div");
            mhdrDiv.Attributes.Add("class", "mhdr");
            mhdrDiv.Attributes.Add("style", "position: relative; bottom: 2px;");
            mhdrDiv.Attributes.Add("id", "divCustomFormMhdr" + hdnInstanceId.Value + hdnCurrentFormId.Value + hdnGroupId.Value + "");
            HtmlGenericControl headerTag = new HtmlGenericControl("h1");
            headerTag.Attributes.Add("style", "font-size: 14px; padding-bottom: 2px;");
            Label headerLable = new Label();
            headerLable.ID = "lblHeader_" + groupId.ToString() + "_" + currentInstanceId.ToString();
            //UAT-2065: Additional searches should go in one grid for each type (not one for each additional search) with ability to delete.
            if (IsSupplementalOrder || InstanceId == 1)
                headerLable.Text = SectionTitle;
            else
                headerLable.Text = SectionTitle + " - " + CurrentInstanceIdForGroup.ToString();
            headerLable.CssClass = currentInstanceId.ToString();
            headerTag.Controls.Add(headerLable);
            //UAT-2065: Additional searches should go in one grid for each type (not one for each additional search) with ability to delete.
            if (!IsSupplementalOrder)
                SetDeleteButton(headerTag, currentInstanceId);
            mhdrDiv.Controls.Add(headerTag);

            //12/08/2014 Create Button Dynamically to show within the Accordion Mhdr Div
            if (ShowEditDetailButton && (ViewState["IsEditButtonExist"].IsNullOrEmpty() || ViewState["IsEditButtonExist"].ToString().ToLower() != "true"))
            {
                ViewState["IsEditButtonExist"] = true;
                HtmlGenericControl editBtnDiv = new HtmlGenericControl("div");
                editBtnDiv.Attributes.Add("style", "right: 20px; position: absolute; z-index: 99999999999; bottom: 20px;");
                editBtnDiv.Attributes.Add("class", "RadButton_Outlook");

                WclButton editBtn = new WclButton();
                editBtn.ID = "editBtn" + hdnInstanceId.Value + hdnCurrentFormId.Value + hdnGroupId.Value + "";
                //editBtn.Text = "Edit " + SectionTitle + "";
                editBtn.Text = Resources.Language.EDIT + " " + SectionTitle + "";
                editBtn.AutoPostBack = true;
                editBtn.ButtonType = Telerik.Web.UI.RadButtonType.StandardButton;
                editBtn.Attributes.Add("onclick", "stopColapseCustomForm('" + hdnInstanceId.Value + hdnCurrentFormId.Value + hdnGroupId.Value + "', '" + cmdbarEdit.ClientID + "');");
                editBtn.Attributes.Add("style", "float:right");
                editBtnDiv.Controls.Add(editBtn);
                mhdrDiv.Controls.Add(editBtnDiv);
            }

            section.Controls.Add(mhdrDiv);
            HtmlGenericControl content = new HtmlGenericControl("div");
            content.Attributes.Add("class", "content");

            HtmlGenericControl Container = new HtmlGenericControl("div");
            Container.Attributes.Add("class", "sxform auto");
            //Panel in which all the other controls are loaded
            Panel formPanel = new Panel();
            formPanel.ID = "pnl_" + currentInstanceId.ToString() + "_" + groupId;
            formPanel.CssClass = "sxpnl";
            Container.Controls.Add(formPanel);
            //UAT-2065: Additional searches should go in one grid for each type (not one for each additional search) with ability to delete.

            //comment if not required
            if (IsSupplementalOrder)
            {
                if ((CustomHtml != "" || InstructionText != "") && !IsReadOnly)
                {
                    HtmlGenericControl customHtmlDiv = new HtmlGenericControl("div");
                    customHtmlDiv.Attributes.Add("class", "customli");
                    customHtmlDiv.InnerHtml = CustomHtml;
                    content.Controls.Add(customHtmlDiv);
                    HtmlGenericControl customInstructionTextDiv = new HtmlGenericControl("div");
                    customInstructionTextDiv.Attributes.Add("class", "customli");
                    customInstructionTextDiv.InnerHtml = InstructionText;
                    content.Controls.Add(customInstructionTextDiv);
                }
            }
            else
            {
                if (currentInstanceId == 1 && (CustomHtml != "" || InstructionText != "") && !IsReadOnly)
                {
                    HtmlGenericControl customHtmlDiv = new HtmlGenericControl("div");
                    customHtmlDiv.Attributes.Add("class", "customli");
                    customHtmlDiv.InnerHtml = CustomHtml;
                    content.Controls.Add(customHtmlDiv);
                    HtmlGenericControl customInstructionTextDiv = new HtmlGenericControl("div");
                    customInstructionTextDiv.Attributes.Add("class", "customli");
                    customInstructionTextDiv.InnerHtml = InstructionText;
                    content.Controls.Add(customInstructionTextDiv);
                }
            }

            content.Controls.Add(Container);
            section.Controls.Add(content);
            mainContainer.Controls.Add(section);
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
            //customHtmlHeader.Attributes.Add("class", "customHeader");
            customHtmlHeader.Style.Add("color", "rgb(140, 25, 33)");
            customHtmlHeader.Style.Add("font-family", "Helvetia, Arial, sans-serif");
            customHtmlHeader.Style.Add("font-size", "100%");
            customHtmlHeader.Style.Add("font-weight", "700");
            customHtmlHeader.Style.Add("line-height", "150%");
            customHtmlHeader.Style.Add("word-spacing", "2px");

            if (InstanceId == 1)
            {
                customHtmlHeader.InnerHtml = SectionTitle;
            }
            else
            {
                customHtmlHeader.InnerHtml = SectionTitle + "-" + currentInstanceId.ToString();
            }
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
        private void GenerateFormForColumDisplay(Int32 groupId, Int32 columnNumber, Control mainPanelForFor, Int32 currentInstanceId, Boolean IsAutoFoucs)
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

                foreach (AttributesForCustomFormContract attributesForCustomForm in lstAttributesForGroupId.Where(cond => lstAutoFillCascadingAttributes.Keys.Contains(cond.AttributeId)).ToList())
                {
                    if (lstAutoFillCascadingAttributes.ContainsKey(attributesForCustomForm.AttributeId) && _lstGroupIdNames.ContainsKey(attributesForCustomForm.AtrributeGroupMappingId))
                    {
                        _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId] = lstAutoFillCascadingAttributes[attributesForCustomForm.AttributeId];
                    }
                }
            }
            else //if (IsSupplementalOrder)
            {
                SetSpecialDataTypeDataForSupplemental(lstAttributesForGroupId);
            }
            //else
            //{
            //    SetSpecialDataTypeDataForSupplemental(lstAttributesForGroupId);
            //}

            //UAT-2065: Additional searches should go in one grid for each type (not one for each additional search) with ability to delete.
            Panel pnlRendercustomFormMe = new Panel();
            pnlRendercustomFormMe.ID = "pnlInner_" + groupId + "_" + currentInstanceId;


            if (LanguageTranslateUtils.GetCurrentLanguageFromSession().LanguageCode == Languages.SPANISH.GetStringValue())
            {
                SystemSpecificLanguageTextList = Presenter.GetSystemSpecificTranslatedText();
            }

            foreach (var attributes in lstAttributesForGroupId)
            {
                if (!attributes.IsDisplay)
                {
                    continue;
                }
                if (controlInColumn == null)
                {
                    //Generate a new row
                    controlInColumn = GenerateColumnView(columnNumber);
                }

                //if (attributes.IsDisplay)
                //{
                //    i++;
                //}
                if (!attributes.IsHiddenFromUI)
                {
                    //If the number of control generated = number of column 
                    //per row then add to the form n create new row.
                    if (i == columnNumber)
                    {
                        AddNextLineDiv(controlInColumn, currentInstanceId);
                        if (IsSupplementalOrder)
                        {
                            pnlRendercustomFormMe.Controls.Add(controlInColumn);
                            mainPanelForFor.Controls.Add(pnlRendercustomFormMe);
                        }
                        else
                            mainPanelForFor.Controls.Add(controlInColumn);
                        i = 0;
                        controlInColumn = GenerateColumnView(columnNumber);
                    }
                    controlInColumn = GenerateControl(attributes, controlInColumn, currentInstanceId, IsAutoFoucs);
                    i++;
                }
                else
                {
                    HtmlGenericControl controlDiv = CreateControlForTheForm(attributes, currentInstanceId, IsAutoFoucs);
                    controlDiv.Style.Add("display", "none");
                    controlInColumn.Controls.Add(controlDiv);
                }
                //if (attributes.IsDisplay)
                //{
                //    //If the number of control generated = number of column 
                //    //per row then add to the form n create new row.
                //    if (i == columnNumber)
                //    {
                //        AddNextLineDiv(controlInColumn, currentInstanceId);
                //        if (IsSupplementalOrder)
                //        {
                //            pnlRendercustomFormMe.Controls.Add(controlInColumn);
                //            mainPanelForFor.Controls.Add(pnlRendercustomFormMe);
                //        }
                //        else
                //            mainPanelForFor.Controls.Add(controlInColumn);
                //        i = 0;
                //    }
                //    if (i == 0)
                //    {
                //        //Generate a new row
                //        controlInColumn = GenerateColumnView(columnNumber);
                //    }
                //    controlInColumn = GenerateControl(attributes, controlInColumn, currentInstanceId, IsAutoFoucs);
                //    i++;
                //}
            }
            if (i != 0)
            {
                AddNextLineDiv(controlInColumn, currentInstanceId);
                //UAT:2065
                if (IsSupplementalOrder)
                {
                    pnlRendercustomFormMe.Controls.Add(controlInColumn);
                    mainPanelForFor.Controls.Add(pnlRendercustomFormMe);
                }
                else
                    mainPanelForFor.Controls.Add(controlInColumn);
            }

        }

        private void SetDeleteButton(HtmlGenericControl headerTag, Int32 currentInstanceId)
        {
            if (IsReadOnly && IsSupplementalOrder)
            {
                HtmlGenericControl delete = new HtmlGenericControl("span");
                delete.Attributes.Add("onclick", "if (!confirm('Are you sure you want to remove this instance?')){return false;} else {HideReadOnlyCurrentInstance(this);return false;}");
                delete.InnerHtml = "Delete";
                delete.Style.Add("float", "right");
                delete.Style.Add("color", "#bd6a38");
                delete.Style.Add("padding-right", "6px");
                delete.Style.Add("font-weight", "bold");
                delete.Style.Add("cursor", "pointer");
                delete.Attributes.Add("id", groupId.ToString() + "_" + currentInstanceId.ToString());
                headerTag.Controls.Add(delete);
            }
            else if (!IsReadOnly && !IsOrderConfirmation)
            {
                if (IsSupplementalOrder)
                {
                    HtmlGenericControl delete = new HtmlGenericControl("span");
                    delete.Attributes.Add("onclick", "if (!confirm('Are you sure you want to remove this instance?')){return false;} else {HideCurrentInstance(this);return false;}");
                    delete.InnerHtml = "Delete";
                    delete.Style.Add("float", "right");
                    delete.Style.Add("color", "#bd6a38");
                    delete.Style.Add("padding-right", "6px");
                    delete.Style.Add("font-weight", "bold");
                    delete.Style.Add("cursor", "pointer");
                    delete.Attributes.Add("id", groupId.ToString() + "_" + currentInstanceId.ToString());
                    headerTag.Controls.Add(delete);
                }
                else
                {
                    //Boolean hideDeleteButton = false;
                    //if (currentInstanceId == AppConsts.ONE)
                    //{
                    //    hideDeleteButton = !MinimumOccurence.IsNullOrEmpty() && MinimumOccurence == AppConsts.NONE
                    //     && lstCustomFormAttributes.Where(xx => xx.AttriButeGroupName.Equals("Employment History") || xx.AttriButeGroupName.Equals("Education History")).ToList().Count > AppConsts.NONE;
                    //}
                    Boolean showDeleteButton = true;
                    if (currentInstanceId == AppConsts.ONE && lstCustomFormAttributes.Where(x => x.AttributeGroupId == groupId && !x.BkgSvcAttributeGroupCode.IsNull()
                        && (x.BkgSvcAttributeGroupCode.Equals(ServiceAttributeGroup.EMPLOYMENT_HISTORY.GetStringValue().ToLower())
                        || x.BkgSvcAttributeGroupCode.Equals(ServiceAttributeGroup.EDUCATION_HISTORY.GetStringValue().ToLower()))).ToList().Count > AppConsts.NONE)
                    {
                        showDeleteButton = false;
                    }

                    //if (currentInstanceId > MinimumOccurence && !hideDeleteButton)
                    if (currentInstanceId > MinimumOccurence && showDeleteButton)
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
            HtmlGenericControl twoColumn = new HtmlGenericControl("div");
            twoColumn.Attributes.Add("class", "sxro sx" + Convert.ToString(columnNumber) + "co");
            return twoColumn;

        }

        /// <summary>
        /// Add relevant space between two rows.
        /// </summary>
        /// <param name="parentControl"></param>
        /// <param name="currentInstanceId"></param>
        /// <returns>parentControl</returns>
        private HtmlGenericControl AddNextLineDiv(HtmlGenericControl parentControl, Int32 currentInstanceId)
        {
            String className = "sxroend";
            if (IsOrderConfirmation)
                //className = "newLine";
                className = "both";
            HtmlGenericControl nextLineDiv = new HtmlGenericControl("div");
            //nextLineDiv.Attributes.Add("class", className);
            nextLineDiv.Style.Add("clear", className);
            //UAT-2065: Additional searches should go in one grid for each type (not one for each additional search) with ability to delete.
            if (IsSupplementalOrder)
                SetDeleteButton(parentControl, currentInstanceId);
            parentControl.Controls.Add(nextLineDiv);
            return parentControl;
        }

        /// <summary>
        /// Main function that creates a control nas per their data type.
        /// </summary>
        /// <param name="attributesForCustomForm">Attribute data to be created.</param>
        /// <param name="parentControl"></param>
        /// <returns></returns>
        private HtmlGenericControl GenerateControl(AttributesForCustomFormContract attributesForCustomForm, HtmlGenericControl parentControl, Int32 currentInstanceId, Boolean IsAutoFoucs)
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
            //if (!attributesForCustomForm.IsHiddenFromUI) 
            //    lableDiv.Style.Add("display", "none");
            parentControl.Controls.Add(lableDiv);
            HtmlGenericControl controlDiv = CreateControlForTheForm(attributesForCustomForm, currentInstanceId, IsAutoFoucs);
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

            string _labelText = attributesForCustomForm.AttributeName;
            if (SystemSpecificLanguageTextList.IsNotNull() && SystemSpecificLanguageTextList.Count() > AppConsts.NONE
                && SystemSpecificLanguageTextList.Any(col => col.SELT_EntityId == attributesForCustomForm.AttributeId))
            {
                _labelText = SystemSpecificLanguageTextList.Where(cond => cond.SELT_EntityId == attributesForCustomForm.AttributeId).FirstOrDefault().SELT_TranslationText;
            }

            //attributeLable.Text = attributesForCustomForm.AttributeName;
            attributeLable.Text = _labelText;
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
        private HtmlGenericControl CreateControlForTheForm(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, Boolean IsAutoFoucs)
        {
            HtmlGenericControl controlDiv = new HtmlGenericControl("div");
            if (attributesForCustomForm.AttributeCode.ToUpper().Equals("4E12CF9A-A4AF-4A38-B4BB-47E6059259F4"))
            {
                controlDiv.Attributes.Add("class", "sxlm myControl");
            }
            else
            {
                controlDiv.Attributes.Add("class", "sxlm");
            }
            switch (attributesForCustomForm.AttributeTypeCode)
            {
                case "AAAB":
                    if (attributesForCustomForm.AttributeCode.ToUpper().Equals("4E12CF9A-A4AF-4A38-B4BB-47E6059259F4"))
                    {
                        HtmlGenericControl PhoneDiv = new HtmlGenericControl("div");
                        PhoneDiv.ID = "dvPhone_" + currentInstanceId;
                        PhoneDiv.Attributes.Add("class", "dvPhone");

                        HtmlGenericControl dvMaskedText = new HtmlGenericControl("div");
                        dvMaskedText.ID = "dvMaskedText_" + currentInstanceId;

                        WclMaskedTextBox mastTextBox = new WclMaskedTextBox();
                        mastTextBox.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;

                        mastTextBox.Style.Add("display", "block");
                        mastTextBox.Mask = "(###)-###-####";
                        mastTextBox.CssClass = "grayoutClass";
                        if (!attributesForCustomForm.MaximumValue.IsNullOrEmpty())
                            mastTextBox.MaxLength = Convert.ToInt32(attributesForCustomForm.MaximumValue);
                        if (IsEdit)
                        {
                            var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, mastTextBox, attributesForCustomForm);
                            mastTextBox = dataSet as WclMaskedTextBox;
                        }
                        dvMaskedText.Controls.Add(mastTextBox);
                        PhoneDiv.Controls.Add(dvMaskedText);

                        if (attributesForCustomForm.IsRequired)
                        {
                            PhoneDiv.Controls.Add(SetrequiredFieldValidator(mastTextBox.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId));
                        }

                        //UAT 3573 Adding empty Span for future validation
                        HtmlGenericControl requiredFieldsDiv = new HtmlGenericControl("div");
                        requiredFieldsDiv.Attributes.Add("class", "vldx ExtraValidation");
                        requiredFieldsDiv.Attributes.Add("display", "none");
                        HtmlGenericControl span = new HtmlGenericControl();
                        span.ID = "RequiredValidator" + "_" + attributesForCustomForm.AttributeName.Replace(" ", "") + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        //span.Attributes.Add("class", "ExtraValidation");
                        span.Style.Add("color", "red");
                        requiredFieldsDiv.Controls.Add(span);
                        PhoneDiv.Controls.Add(requiredFieldsDiv);

                        //UAT-2447
                        HtmlGenericControl dvPlainText = new HtmlGenericControl("div");
                        dvPlainText.ID = "dvPlainText_" + currentInstanceId;

                        WclTextBox txtPhoneNumber = new WclTextBox();
                        txtPhoneNumber.ID = "txtPlain_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        txtPhoneNumber.Style.Add("display", "block");
                        txtPhoneNumber.CssClass = "grayoutClass";
                        //txtPhoneNumber.ClientEvents.OnMouseOver = "onmouseover";
                        //txtPhoneNumber.ClientEvents.OnMouseOut = "onmouseover";
                        //txtPhoneNumber.ClientEvents.OnFocus = "onmouseover";
                        //txtPhoneNumber.ClientEvents.OnBlur = "onmouseover";

                        txtPhoneNumber.MaxLength = 15;
                        dvPlainText.Controls.Add(txtPhoneNumber);
                        PhoneDiv.Controls.Add(dvPlainText);
                        // dvPlainTextBox.Controls.Add(txtPhoneNumber);

                        String PlainTextBoxAttributeType = attributesForCustomForm.AttributeType + "_Plain";
                        if (attributesForCustomForm.IsRequired)
                        {
                            PhoneDiv.Controls.Add(SetrequiredFieldValidator(txtPhoneNumber.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, PlainTextBoxAttributeType, currentInstanceId));
                        }

                        //UAT 3573 Adding empty Span for future validation
                        HtmlGenericControl requiredFieldsNewDiv = new HtmlGenericControl("div");
                        requiredFieldsDiv.Attributes.Add("class", "vldx ExtraValidation");
                        requiredFieldsDiv.Attributes.Add("display", "none");
                        HtmlGenericControl span2 = new HtmlGenericControl();
                        span2.ID = "RequiredValidator" + "_" + attributesForCustomForm.AttributeName.Replace(" ", "") + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        // span2.Attributes.Add("class", "ExtraValidation");
                        span.Style.Add("color", "red");
                        requiredFieldsNewDiv.Controls.Add(span2);
                        PhoneDiv.Controls.Add(requiredFieldsDiv);

                        String revPlainTextBoxID = "rfv_" + PlainTextBoxAttributeType.RemoveWhitespace() + "_" + currentInstanceId.ToString() + "_" + attributesForCustomForm.AtrributeGroupMappingId.ToString();
                        String revMaskedPhoneID = "rfv_" + attributesForCustomForm.AttributeType.RemoveWhitespace() + "_" + currentInstanceId.ToString() + "_" + attributesForCustomForm.AtrributeGroupMappingId.ToString();

                        RegularExpressionValidator regPlainText = new RegularExpressionValidator();
                        regPlainText.ID = "reg_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        regPlainText.Display = ValidatorDisplay.Dynamic;
                        regPlainText.ControlToValidate = txtPhoneNumber.ID;
                        regPlainText.ErrorMessage = "Invalid phone number. This field only contains +, - and numbers.";
                        String ValidationExp = @"(\d?)+(([-+]+?\d+)?)+([-+]?)+";
                        regPlainText.ValidationExpression = ValidationExp;
                        regPlainText.CssClass = "errmsg";
                        PhoneDiv.Controls.Add(regPlainText);

                        WclCheckBox chkPhoneNumber = new WclCheckBox();
                        chkPhoneNumber.ID = "chk_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        chkPhoneNumber.ToolTip = "Check this box if you do not have an US Number.";
                        chkPhoneNumber.Style.Add("display", "block");
                        chkPhoneNumber.Attributes.Add("class", "ChckBox grayoutClass");
                        chkPhoneNumber.Attributes.Add("onclick", "MaskedUnmaskedPhone('" + chkPhoneNumber.ID + "','" + dvMaskedText.ID + "','" + dvPlainText.ID + "','" + revPlainTextBoxID + "','" + revMaskedPhoneID + "','" + regPlainText.ID + "');");

                        if (IsEdit && !_lstIntPhoneNumberExtraData.IsNullOrEmpty()
                            && _lstIntPhoneNumberExtraData.ContainsKey(attributesForCustomForm.AtrributeGroupMappingId))
                        {
                            chkPhoneNumber.Checked = Convert.ToBoolean(_lstIntPhoneNumberExtraData[attributesForCustomForm.AtrributeGroupMappingId]);
                        }

                        PhoneDiv.Controls.Add(chkPhoneNumber);

                        if (IsEdit)
                        {
                            var dataSet = SetTheValueForControlInEditMode("PlainTextPhone", txtPhoneNumber, attributesForCustomForm);
                            txtPhoneNumber = dataSet as WclTextBox;
                        }

                        controlDiv.Controls.Add(PhoneDiv);
                    }
                    //UAT-2448
                    else if (attributesForCustomForm.AttributeCode.ToUpper().Equals("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211"))
                    {
                        WclTextBox textBox = new WclTextBox();
                        textBox.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        textBox.Style.Add("display", "block");
                        textBox.CssClass = "grayoutClass";
                        //UAT-2074 : When I hit “Add New” button, I shouldn’t have to click into State (for location) or Alias boxes.
                        if (IsAutoFoucs)
                        {
                            textBox.Attributes.Add("doFocustxt", "focus");
                        }
                        if (!attributesForCustomForm.MaximumValue.IsNullOrEmpty())
                            textBox.MaxLength = Convert.ToInt32(attributesForCustomForm.MaximumValue);
                        //UAT-2062: System to determine and add additional searches in supplement (SSN Trace)
                        if (IsEdit || _IsPopulateAliasForAddiSearch)
                        {
                            var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, textBox, attributesForCustomForm);
                            textBox = dataSet as WclTextBox;
                        }
                        controlDiv.Controls.Add(textBox);
                        if (attributesForCustomForm.IsRequired)
                        {
                            controlDiv.Controls.Add(SetrequiredFieldValidator(textBox.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId));
                        }

                        //UAT 3573 Adding empty Span for future validation
                        HtmlGenericControl requiredFieldsDiv = new HtmlGenericControl("div");
                        requiredFieldsDiv.Attributes.Add("class", "vldx ExtraValidation");
                        requiredFieldsDiv.Attributes.Add("display", "none");
                        HtmlGenericControl span = new HtmlGenericControl();
                        span.ID = "RequiredValidator" + "_" + attributesForCustomForm.AttributeName.Replace(" ", "") + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        //span.Attributes.Add("class", "ExtraValidation");
                        span.Style.Add("color", "red");
                        requiredFieldsDiv.Controls.Add(span);
                        controlDiv.Controls.Add(requiredFieldsDiv);

                        //UAT 3541
                        if (!attributesForCustomForm.ValidateExpression.IsNullOrEmpty())
                        {
                            controlDiv.Controls.Add(SetRegularExpressionFieldValidator(textBox.ID, attributesForCustomForm.ValidateExpression, attributesForCustomForm.ValidationMessage, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId));
                        }
                        HtmlAnchor htmlanchor = new HtmlAnchor();
                        htmlanchor.HRef = "javascript:void(0)";
                        htmlanchor.Attributes.Add("onclick", "openPopUp();");
                        //htmlanchor.Title = "Help"; //tooltip
                        htmlanchor.InnerText = "Help";
                        controlDiv.Controls.Add(htmlanchor);

                    }
                    else
                    {
                        WclTextBox textBox = new WclTextBox();
                        textBox.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        AddControlID(attributesForCustomForm.AttributeId.ToString() + "_" + currentInstanceId, textBox.ID);
                        textBox.Style.Add("display", "block");
                        textBox.CssClass = "grayoutClass";
                        textBox.Attributes.Add("class", "grayoutClass");
                        //UAT-2074 : When I hit “Add New” button, I shouldn’t have to click into State (for location) or Alias boxes.
                        if (IsAutoFoucs)
                        {
                            textBox.Attributes.Add("doFocustxt", "focus");
                        }
                        if (!attributesForCustomForm.MaximumValue.IsNullOrEmpty())
                            textBox.MaxLength = Convert.ToInt32(attributesForCustomForm.MaximumValue);
                        //UAT-2062: System to determine and add additional searches in supplement (SSN Trace)
                        if (IsEdit || _IsPopulateAliasForAddiSearch)
                        {
                            var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, textBox, attributesForCustomForm);
                            textBox = dataSet as WclTextBox;
                        }
                        controlDiv.Controls.Add(textBox);
                        if (attributesForCustomForm.IsRequired)
                        {
                            controlDiv.Controls.Add(SetrequiredFieldValidator(textBox.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId, AttributeId: attributesForCustomForm.AttributeId));
                        }

                        //UAT 3573 Adding empty Span for future validation
                        HtmlGenericControl requiredFieldsDiv = new HtmlGenericControl("div");
                        requiredFieldsDiv.Attributes.Add("class", "vldx ExtraValidation");
                        requiredFieldsDiv.Attributes.Add("display", "none");
                        HtmlGenericControl span = new HtmlGenericControl();
                        span.ID = "RequiredValidator" + "_" + attributesForCustomForm.AttributeName.Replace(" ", "") + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                        // span.Attributes.Add("class", "ExtraValidation");
                        span.Style.Add("color", "red");
                        requiredFieldsDiv.Controls.Add(span);
                        controlDiv.Controls.Add(requiredFieldsDiv);

                        //UAT 3541
                        if (!attributesForCustomForm.ValidateExpression.IsNullOrEmpty())
                        {
                            controlDiv.Controls.Add(SetRegularExpressionFieldValidator(textBox.ID, attributesForCustomForm.ValidateExpression, attributesForCustomForm.ValidationMessage, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId));
                        }
                    }
                    break;
                case "AAAC":
                    WclNumericTextBox txtNumeric = new WclNumericTextBox();
                    txtNumeric.ID = "txtNumericType_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    AddControlID(attributesForCustomForm.AttributeId.ToString() + "_" + currentInstanceId, txtNumeric.ID);
                    txtNumeric.Style.Add("display", "block");
                    txtNumeric.CssClass = "grayoutClass";
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

                    //UAT 3573 Adding empty Span for future validation
                    HtmlGenericControl numericRequiredFieldsDiv = new HtmlGenericControl("div");
                    numericRequiredFieldsDiv.Attributes.Add("class", "vldx ExtraValidation");
                    numericRequiredFieldsDiv.Attributes.Add("display", "none");
                    HtmlGenericControl numericSpan = new HtmlGenericControl();
                    numericSpan.ID = "RequiredValidator" + "_" + attributesForCustomForm.AttributeName.Replace(" ", "") + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    // numericSpan.Attributes.Add("class", "ExtraValidation");
                    numericSpan.Style.Add("color", "red");
                    numericRequiredFieldsDiv.Controls.Add(numericSpan);
                    controlDiv.Controls.Add(numericRequiredFieldsDiv);

                    break;
                case "AAAA":
                    WclDatePicker dPicker = new WclDatePicker();
                    dPicker.DateInput.DateFormat = "MM-dd-yyyy";
                    dPicker.CssClass = "grayoutClass";
                    dPicker.ID = "dp_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    AddControlID(attributesForCustomForm.AttributeId.ToString() + "_" + currentInstanceId, dPicker.ID);
                    dPicker.DateInput.EmptyMessage = Resources.Language.SELECTDATE;  //"Select a date";
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

                    //UAT 3573 Adding empty Span for future validation
                    HtmlGenericControl dateRequiredFieldsDiv = new HtmlGenericControl("div");
                    dateRequiredFieldsDiv.Attributes.Add("class", "vldx ExtraValidation");
                    dateRequiredFieldsDiv.Attributes.Add("display", "none");
                    HtmlGenericControl dateSpan = new HtmlGenericControl();
                    dateSpan.ID = "RequiredValidator" + "_" + attributesForCustomForm.AttributeName.Replace(" ", "") + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    dateSpan.Style.Add("color", "red");
                    dateRequiredFieldsDiv.Controls.Add(dateSpan);
                    controlDiv.Controls.Add(dateRequiredFieldsDiv);

                    //UAT-2216: Remove "End Date" from current employer (not previous employers) on Employment Verification.
                    //Attribute code for "Employment End Date" : "41BFF8A4-EC01-42C9-B6A6-778BAC34D488" that is fixed for that attribute.
                    if (String.Compare(attributesForCustomForm.AttributeCode, "7A0F4CC0-5416-48D1-AC9F-62A98F0D8606", true) == AppConsts.NONE)
                    {
                        HiddenField hdnField = CreateHiddenField(currentInstanceId, attributesForCustomForm.AttributeGroupId);
                        hdnField.Value = Convert.ToString(attributesForCustomForm.AttributeId) + "_" + Convert.ToString(attributesForCustomForm.AtrributeGroupMappingId);
                        controlDiv.Controls.Add(hdnField);
                    }

                    break;
                case "AAAD":
                    WclComboBox dropDownList = new WclComboBox();
                    dropDownList.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    AddControlID(attributesForCustomForm.AttributeId.ToString() + "_" + currentInstanceId, dropDownList.ID);
                    dropDownList.Style.Add("display", "block");
                    dropDownList.DataTextField = "EBSAO_OptionText";
                    dropDownList.DataValueField = "EBSAO_OptionText";
                    dropDownList.EmptyMessage = Resources.Language.SELECTWITHHYPENS;
                    dropDownList.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
                    dropDownList.EnableTextSelection = true;
                    dropDownList.MarkFirstMatch = true;
                    dropDownList.CssClass = "grayoutClass";
                    dropDownList.OnClientBlur = "onLocationBlur";
                    dropDownList.OnClientKeyPressing = "openCmbBoxOnTab";
                    if (attributesForCustomForm.IsDecisionField)
                    {
                        dropDownList.OnClientSelectedIndexChanged = "GetDecisionForTheFields";
                        //UAT-2866
                        hdnDecisionFieldId.Value = attributesForCustomForm.AttributeType + "," + attributesForCustomForm.AttributeGroupId + "," + attributesForCustomForm.AtrributeGroupMappingId;
                    }

                    //UAT-2216: Remove "End Date" from current employer (not previous employers) on Employment Verification.
                    //Attribute code for "Is Current Employer" : "41BFF8A4-EC01-42C9-B6A6-778BAC34D488" that is fixed for that attribute.
                    if (String.Compare(attributesForCustomForm.AttributeCode, "41BFF8A4-EC01-42C9-B6A6-778BAC34D488", true) == AppConsts.NONE)
                    {
                        dropDownList.OnClientSelectedIndexChanged = "GetDecisionForEmploymentEndDate";
                    }

                    dropDownList.DataSource = Presenter.GetOptionValues(attributesForCustomForm.AttributeId);
                    dropDownList.DataBind();

                    if (IsEdit)
                    {
                        var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, dropDownList, attributesForCustomForm);
                        dropDownList = dataSet as WclComboBox;
                    }
                    //UAT-2866
                    if (attributesForCustomForm.IsDecisionField)
                    {
                        var ValueToSelect = dropDownList.Items.Where(cond => cond.Value.ToLower().Trim() == "false" || cond.Value.ToLower().Trim() == "no").FirstOrDefault();
                        if (!ValueToSelect.IsNullOrEmpty() && dropDownList.SelectedValue.IsNullOrEmpty())
                        {
                            ValueToSelect.Selected = true;
                        }
                    }

                    controlDiv.Controls.Add(dropDownList);
                    if (attributesForCustomForm.IsRequired)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownList.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, attributesForCustomForm.AttributeType, currentInstanceId, AttributeId: attributesForCustomForm.AttributeId));
                    }

                    //UAT 3573 Adding empty Span for future validation
                    HtmlGenericControl dropdownRequiredFieldsDiv = new HtmlGenericControl("div");
                    dropdownRequiredFieldsDiv.Attributes.Add("class", "vldx ExtraValidation");
                    dropdownRequiredFieldsDiv.Attributes.Add("display", "none");
                    HtmlGenericControl dropdownSpan = new HtmlGenericControl();
                    dropdownSpan.ID = "RequiredValidator" + "_" + attributesForCustomForm.AttributeName.Replace(" ", "") + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    dropdownSpan.Style.Add("color", "red");
                    dropdownRequiredFieldsDiv.Controls.Add(dropdownSpan);
                    controlDiv.Controls.Add(dropdownRequiredFieldsDiv);
                    break;
                case "AAAE":
                    WclComboBox dropDownListCountry = new WclComboBox();
                    dropDownListCountry.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    AddControlID(attributesForCustomForm.AttributeId.ToString() + "_" + currentInstanceId, dropDownListCountry.ID);
                    dropDownListCountry.DataTextField = "CompleteName";
                    dropDownListCountry.DataValueField = "FullName";
                    dropDownListCountry.CssClass = "JCountry";
                    dropDownListCountry.EmptyMessage = Resources.Language.SELECTWITHHYPENS;
                    dropDownListCountry.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
                    dropDownListCountry.OnClientKeyPressing = "openCmbBoxOnTab";
                    dropDownListCountry.Style.Add("display", "block");
                    dropDownListCountry.CssClass = "grayoutClass";
                    dropDownListCountry.OnClientSelectedIndexChanged = "GetDataForDropDown";
                    dropDownListCountry.OnClientBlur = "ClearDropDownIfNoValueSelected";
                    //UAT-3663-- Added OnClientDropDownClosing event
                    dropDownListCountry.OnClientDropDownClosing = "clientDropdownClosing";

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
                    SetTheStateControl(attributesForCustomForm, currentInstanceId, controlDiv, IsAutoFoucs);
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
                    AddControlID(attributesForCustomForm.AttributeId.ToString() + "_" + currentInstanceId, dropDownListCountryCitizen.ID);
                    dropDownListCountryCitizen.DataTextField = "CompleteName";
                    dropDownListCountryCitizen.DataValueField = "FullName";
                    dropDownListCountryCitizen.CssClass = "JCountry";
                    dropDownListCountryCitizen.EmptyMessage = Resources.Language.SELECTWITHHYPENS;
                    dropDownListCountryCitizen.Style.Add("display", "block");
                    dropDownListCountryCitizen.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
                    dropDownListCountryCitizen.OnClientKeyPressing = "openCmbBoxOnTab";
                    dropDownListCountryCitizen.CssClass = "grayoutClass";
                    dropDownListCountryCitizen.OnClientBlur = "ClearDropDownIfNoValueSelected";
                    //UAT-3663-- Added OnClientDropDownClosing event
                    dropDownListCountryCitizen.OnClientDropDownClosing = "clientDropdownClosing";
                    //Changes w.r.t. UAT 627 - Fill in "United States" as default selection for Country dropdowns. 
                    var listOfCountries = Presenter.GetListOFCountries();
                    if (listOfCountries.IsNotNull())
                    {
                        dropDownListCountryCitizen.DataSource = listOfCountries;
                        dropDownListCountryCitizen.DataBind();
                        if (!IsLocationServiceTenant)
                        {
                            var CountryUSA = listOfCountries.FirstOrDefault(x => x.CountryID == AppConsts.COUNTRY_USA_ID);
                            if (CountryUSA.IsNotNull())
                            {
                                dropDownListCountryCitizen.SelectedValue = CountryUSA.FullName;
                            }
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
                //UAT 3521
                case "AAAK":
                    WclComboBox cascadingDropDownList = new WclComboBox();
                    cascadingDropDownList.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + Convert.ToString(tenantId) + "_" + attributesForCustomForm.AttributeId +
                        "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId; ;
                    AddControlID(attributesForCustomForm.AttributeId.ToString() + "_" + currentInstanceId, cascadingDropDownList.ID);
                    cascadingDropDownList.DataTextField = "";
                    cascadingDropDownList.DataValueField = "";
                    cascadingDropDownList.Enabled = true;
                    cascadingDropDownList.EmptyMessage = Resources.Language.SELECTWITHHYPENS;
                    cascadingDropDownList.Style.Add("display", "block");
                    cascadingDropDownList.Filter = Telerik.Web.UI.RadComboBoxFilter.Contains;
                    cascadingDropDownList.OnClientKeyPressing = "openCmbBoxOnTab";
                    cascadingDropDownList.OnClientSelectedIndexChanged = "GetDataForCascadingDropDown";
                    cascadingDropDownList.OnClientDropDownClosing = "CascadingDropdownClosing";
                    cascadingDropDownList.OnClientBlur = "ClearDropDownIfNoValueSelected";
                    //cascadingDropDownList.IsCaseSensitive = false;

                    string SearchByID = "";

                    //if (attributesForCustomForm.Name.Equals("Place Of Birth CBI (State)")) //UAT_3821
                    //{
                    //    SearchByID = AppConsts.BIRTH_COUNTRY_USA;
                    //}

                    var listOfAttributeData = Presenter.GetDataForCascadingAttr(tenantId, attributesForCustomForm.AttributeId, attributesForCustomForm.AttributeGroupId, SearchByID);

                    if (listOfAttributeData.IsNotNull())
                    {
                        if (countrySortPreference == null)
                        {
                            countrySortPreference = new List<string>();
                            var preferencesKey = WebConfigurationManager.AppSettings["countrySortPreferenceForCascadingAttributes"];
                            if (!string.IsNullOrWhiteSpace(preferencesKey))
                            {
                                countrySortPreference = preferencesKey.Split(',').ToList();
                                countrySortPreference = countrySortPreference.Select(x => x.ToLowerInvariant()).ToList();
                            }
                        }

                        if (listOfAttributeData.Any(att => countrySortPreference.Any(c => c == att.ToLowerInvariant())))
                        {
                            listOfAttributeData = listOfAttributeData.OrderByDescending(pob => Enumerable.Reverse(countrySortPreference).ToList().IndexOf(pob.ToLowerInvariant())).ToList();
                        }

                        cascadingDropDownList.DataSource = listOfAttributeData;
                        cascadingDropDownList.DataBind();

                        if (!IsEdit && !lstAutoFillCascadingAttributes.IsNullOrEmpty() && lstAutoFillCascadingAttributes.ContainsKey(attributesForCustomForm.AttributeId))
                        {
                            cascadingDropDownList.SelectedValue = lstAutoFillCascadingAttributes[attributesForCustomForm.AttributeId];
                            _lstGroupIdNames.Add(attributesForCustomForm.AtrributeGroupMappingId, cascadingDropDownList.SelectedValue);
                            cascadingDropDownList.Enabled = false;
                        }
                    }
                    if (IsEdit || _lstGroupIdNames.ContainsKey(attributesForCustomForm.ParentAttributeGroupMappingId))
                    {
                        if (attributesForCustomForm.ParentAttributeGroupMappingId != AppConsts.NONE && _lstGroupIdNames.ContainsKey(attributesForCustomForm.ParentAttributeGroupMappingId))
                        {
                            listOfAttributeData = Presenter.GetDataForCascadingAttr(tenantId, attributesForCustomForm.AttributeId, attributesForCustomForm.AttributeGroupId, _lstGroupIdNames[attributesForCustomForm.ParentAttributeGroupMappingId]);
                        }

                        cascadingDropDownList.DataSource = listOfAttributeData;
                        cascadingDropDownList.DataBind();

                        if (!IsEdit && !lstAutoFillCascadingAttributes.IsNullOrEmpty() && lstAutoFillCascadingAttributes.ContainsKey(attributesForCustomForm.AttributeId))
                        {
                            var selectedValue = listOfAttributeData.FirstOrDefault(op => op.ToLowerInvariant() == lstAutoFillCascadingAttributes[attributesForCustomForm.AttributeId].ToLowerInvariant());
                            cascadingDropDownList.SelectedValue = string.IsNullOrWhiteSpace(selectedValue) ? lstAutoFillCascadingAttributes[attributesForCustomForm.AttributeId] : selectedValue;
                            _lstGroupIdNames.Add(attributesForCustomForm.AtrributeGroupMappingId, cascadingDropDownList.SelectedValue);
                        }
                        if (!lstAutoFillCascadingAttributes.IsNullOrEmpty() && lstAutoFillCascadingAttributes.ContainsKey(attributesForCustomForm.AttributeId))
                        {
                            cascadingDropDownList.Enabled = false;
                        }
                        var dataSet = SetTheValueForControlInEditMode(attributesForCustomForm.AttributeType, cascadingDropDownList, attributesForCustomForm);
                        if (!listOfAttributeData.IsNullOrEmpty() && listOfAttributeData.Count == AppConsts.ONE)
                        {
                            if (!IsEdit && cascadingDropDownList.SelectedIndex == -1)
                            {
                                _lstGroupIdNames.Add(attributesForCustomForm.AtrributeGroupMappingId, cascadingDropDownList.SelectedValue);
                            }
                            cascadingDropDownList.SelectedIndex = 0;
                            cascadingDropDownList.Enabled = false;
                        }
                        cascadingDropDownList = dataSet as WclComboBox;
                    }
                    controlDiv.Controls.Add(cascadingDropDownList);
                    if (attributesForCustomForm.IsRequired)
                    {
                        controlDiv.Controls.Add(SetrequiredFieldValidator(cascadingDropDownList.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "drop" + attributesForCustomForm.AttributeType, currentInstanceId, AttributeId: attributesForCustomForm.AttributeId));
                    }

                    //UAT 3573 Adding empty Span for future validation
                    HtmlGenericControl cascadingRequiredFieldsDiv = new HtmlGenericControl("div");
                    cascadingRequiredFieldsDiv.Attributes.Add("class", "vldx ExtraValidation");
                    cascadingRequiredFieldsDiv.Attributes.Add("display", "none");
                    HtmlGenericControl cascadingSpan = new HtmlGenericControl();
                    cascadingSpan.ID = "RequiredValidator" + "_" + attributesForCustomForm.AttributeName.Replace(" ", "") + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
                    cascadingSpan.Style.Add("color", "red");
                    cascadingRequiredFieldsDiv.Controls.Add(cascadingSpan);
                    controlDiv.Controls.Add(cascadingRequiredFieldsDiv);
                    break;
                default:

                    break;
            }
            return controlDiv;
        }

        private void SetTheStateControl(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, HtmlGenericControl controlDiv, Boolean IsAutoFoucs)
        {
            WclComboBox dropDownListState = new WclComboBox();
            dropDownListState.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            dropDownListState.CssClass = "JState grayoutClass";
            dropDownListState.OnClientSelectedIndexChanged = "GetDataForDropDown";
            dropDownListState.Enabled = true;
            dropDownListState.Style.Add("display", "block");
            dropDownListState.EmptyMessage = Resources.Language.SELECTWITHHYPENS;
            dropDownListState.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
            dropDownListState.OnClientKeyPressing = "openCmbBoxOnTab";

            dropDownListState.EnableTextSelection = true;
            dropDownListState.MarkFirstMatch = true;
            dropDownListState.OnClientBlur = "onLocationBlur";

            WclTextBox textBoxState = new WclTextBox();
            textBoxState.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            textBoxState.Style.Add("display", "block");
            textBoxState.CssClass = "txtState classTxt grayoutClass";
            //UAT-2062:System to determine and add additional searches in supplement (SSN Trace)
            if ((IsEdit || _IsPopulateStateAndCountyForAddiSearch) && _lstGroupIdNames.Count > 0)
            {
                StateName = String.Empty;
                if (IsLocationServiceTenant && CountryName != "UNITED STATES")
                {
                    dropDownListState.Enabled = false;
                    SetHiddenTextBoxValues(attributesForCustomForm, currentInstanceId);
                }
                else
                {
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
                        if (_IsPopulateStateAndCountyForAddiSearch)
                        {
                            dropDownListState.OnClientSelectedIndexChanged = "GetDataForDropDownForSupplemental";
                        }
                    }
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
                if (IsSupplementalOrder)
                {
                    dropDownListState.OnClientSelectedIndexChanged = "GetDataForDropDownForSupplemental";
                }
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

            //UAT-2074 : When I hit “Add New” button, I shouldn’t have to click into State (for location) or Alias boxes.
            if (IsAutoFoucs)
            {
                dropDownListState.Attributes.Add("doFocusCmb", "focus");
            }
            //dropDownListState.Attributes.Add("style", "focus:true");
            //dropDownListState.Focus();

            controlDiv.Controls.Add(dropDownListState);
            controlDiv.Controls.Add(textBoxState);
            if (attributesForCustomForm.IsRequired)
            {
                if (IsLocationServiceTenant && CountryName != "UNITED STATES")
                {
                    controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownListState.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "drop" + attributesForCustomForm.AttributeType, currentInstanceId, false));
                }
                else
                {
                    controlDiv.Controls.Add(SetrequiredFieldValidator(dropDownListState.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "drop" + attributesForCustomForm.AttributeType, currentInstanceId));
                }
                controlDiv.Controls.Add(SetrequiredFieldValidator(textBoxState.ID, attributesForCustomForm.AttributeName, attributesForCustomForm.AtrributeGroupMappingId, "txt" + attributesForCustomForm.AttributeType, currentInstanceId, false));
            }
        }

        private void SetTheCityControl(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, HtmlGenericControl controlDiv)
        {
            WclComboBox dropDownListCity = new WclComboBox();
            dropDownListCity.ID = "dropDown_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            dropDownListCity.CssClass = "JCity grayoutClass";
            dropDownListCity.Style.Add("display", "block");
            dropDownListCity.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
            dropDownListCity.OnClientKeyPressing = "openCmbBoxOnTab";
            if (IsSupplementalOrder)
                dropDownListCity.OnClientSelectedIndexChanged = "GetDataForDropDownForSupplemental";
            else
                dropDownListCity.OnClientSelectedIndexChanged = "GetDataForDropDown";
            dropDownListCity.Enabled = true;
            dropDownListCity.EmptyMessage = Resources.Language.SELECTWITHHYPENS;

            dropDownListCity.EnableTextSelection = true;
            dropDownListCity.MarkFirstMatch = true;
            dropDownListCity.OnClientBlur = "onLocationBlur";

            WclTextBox textBoxCity = new WclTextBox();
            textBoxCity.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            textBoxCity.Style.Add("display", "block");
            textBoxCity.CssClass = "txtCity classTxt grayoutClass";
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
            dropDownListZip.CssClass = "JZipCode grayoutClass";
            dropDownListZip.Enabled = true;
            dropDownListZip.Style.Add("display", "block");
            dropDownListZip.EmptyMessage = Resources.Language.SELECTWITHHYPENS;
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
            textBoxZip.CssClass = "txtZipCode classTxt grayoutClass";
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
            dropDownListCounty.CssClass = "JCounty grayoutClass";
            dropDownListCounty.Style.Add("display", "block");
            dropDownListCounty.Enabled = true;
            dropDownListCounty.EmptyMessage = Resources.Language.SELECTWITHHYPENS;
            dropDownListCounty.Filter = Telerik.Web.UI.RadComboBoxFilter.StartsWith;
            dropDownListCounty.OnClientKeyPressing = "openCmbBoxOnTab";

            dropDownListCounty.EnableTextSelection = true;
            dropDownListCounty.MarkFirstMatch = true;
            dropDownListCounty.OnClientBlur = "onLocationBlur";

            WclTextBox textBoxCounty = new WclTextBox();
            textBoxCounty.ID = "txt_" + attributesForCustomForm.AttributeType + "_" + currentInstanceId + "_" + attributesForCustomForm.AttributeGroupId + "_" + attributesForCustomForm.AtrributeGroupMappingId;
            textBoxCounty.Style.Add("display", "block");
            textBoxCounty.CssClass = "txtCounty classTxt grayoutClass";
            //UAT-2062:System to determine and add additional searches in supplement (SSN Trace)
            if ((IsEdit || _IsPopulateStateAndCountyForAddiSearch) && _lstGroupIdNames.Count > 0)
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
                    if (_IsPopulateStateAndCountyForAddiSearch)
                    {
                        lstCounty = Presenter.GetListOfCountyByState(StateName, CountryName);
                    }
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
                //UAT-2447
                case "PlainTextPhone":
                    if (attributesForCustomForm.AttributeCode.ToUpper().Equals("4E12CF9A-A4AF-4A38-B4BB-47E6059259F4"))
                    {
                        WclTextBox PlainText = control as WclTextBox;
                        if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                            PlainText.Text = String.Empty;
                        else
                            PlainText.Text = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
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
                //UAT 3521
                case "Cascading":
                    WclComboBox CascadingDropDown = control as WclComboBox;
                    if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && !_lstGroupIdNames.ContainsKey(attributesForCustomForm.AtrributeGroupMappingId)))
                    {
                        CascadingDropDown.SelectedValue = String.Empty;
                    }
                    else
                    {
                        CascadingDropDown.SelectedValue = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                    }
                    break;
                case "Stand Alone Country":
                    if (IsLocationServiceTenant)
                    {
                        WclComboBox citizenDropDown = control as WclComboBox;
                        if (_lstGroupIdNames.Count == 0 || (_lstGroupIdNames.Count > 0 && _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId].IsNullOrEmpty()))
                        {
                            citizenDropDown.SelectedValue = String.Empty;
                        }
                        else
                        {
                            citizenDropDown.SelectedValue = _lstGroupIdNames[attributesForCustomForm.AtrributeGroupMappingId];
                        }
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
            HtmlGenericControl lableDiv = new HtmlGenericControl("div");
            lableDiv.Attributes.Add("class", "sxlb");
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes.Add("class", "cptn");

            string _labelText = attributesForCustomForm.AttributeName;
            if (SystemSpecificLanguageTextList.IsNotNull() && SystemSpecificLanguageTextList.Count() > AppConsts.NONE
                && SystemSpecificLanguageTextList.Any(col => col.SELT_EntityId == attributesForCustomForm.AttributeId))
            {
                _labelText = SystemSpecificLanguageTextList.Where(cond => cond.SELT_EntityId == attributesForCustomForm.AttributeId).FirstOrDefault().SELT_TranslationText;
            }

            //span.InnerHtml = attributesForCustomForm.AttributeName;
            span.InnerHtml = _labelText;

            lableDiv.Controls.Add(span);
            HtmlGenericControl controlDiv = new HtmlGenericControl("div");
            controlDiv.Attributes.Add("class", "sxlm");
            Label controlLable = new Label();
            controlLable.ID = "lbl" + attributesForCustomForm.AttributeType + "_" + attributesForCustomForm.AttributeGroupId + "_" + currentInstanceId.ToString() + "_" + attributesForCustomForm.AttributeId;
            controlLable.CssClass = "ronly";
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
            controlDiv.Controls.Add(controlLable);
            parentControl.Controls.Add(lableDiv);
            parentControl.Controls.Add(controlDiv);
            return parentControl;
        }

        private HtmlGenericControl CreateControlForOrderConfirmation(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, HtmlGenericControl parentControl)
        {
            HtmlGenericControl mainDiv = new HtmlGenericControl("div");
            //mainDiv.Attributes.Add("class", "customDivStyle");
            mainDiv.Style.Add("width", "30%"); //vertical-align: top; float: left; display: inline-block
            mainDiv.Style.Add("vertical-align", "top");
            mainDiv.Style.Add("float", "left");
            mainDiv.Style.Add("display", "inline-block");
            HtmlGenericControl lableSpan = new HtmlGenericControl("span");

            //lableSpan.InnerHtml = attributesForCustomForm.AttributeName + ":&nbsp;";

            string _labelText = attributesForCustomForm.AttributeName;
            if (SystemSpecificLanguageTextList.IsNotNull() && SystemSpecificLanguageTextList.Count() > AppConsts.NONE
                && SystemSpecificLanguageTextList.Any(col => col.SELT_EntityId == attributesForCustomForm.AttributeId))
            {
                _labelText = SystemSpecificLanguageTextList.Where(cond => cond.SELT_EntityId == attributesForCustomForm.AttributeId).FirstOrDefault().SELT_TranslationText;
            }

            lableSpan.InnerHtml = _labelText + ":&nbsp;";

            mainDiv.Controls.Add(lableSpan);
            HtmlGenericControl dataSpan = new HtmlGenericControl("span");
            //dataSpan.Attributes.Add("class", "boldFont");
            dataSpan.Style.Add("font-weight", "bold");
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
        private HtmlGenericControl SetrequiredFieldValidator(String controlTovalidate, String attributeName, Int32 AtrributeGroupMappingId, String AttributeType, Int32 instanceId, Boolean enable = true, Int32 AttributeId = 0)
        {
            if (SystemSpecificLanguageTextList.IsNotNull() && SystemSpecificLanguageTextList.Count() > AppConsts.NONE &&
                AttributeId > AppConsts.NONE && SystemSpecificLanguageTextList.Any(col => col.SELT_EntityId == AttributeId))
            {
                attributeName = SystemSpecificLanguageTextList.Where(cond => cond.SELT_EntityId == AttributeId).FirstOrDefault().SELT_TranslationText;
            }

            HtmlGenericControl requiredFieldsDiv = new HtmlGenericControl("div");
            requiredFieldsDiv.Attributes.Add("class", "vldx");
            //groupId
            RequiredFieldValidator requiredField = new RequiredFieldValidator();
            requiredField.ID = "rfv_" + AttributeType.RemoveWhitespace() + "_" + instanceId.ToString() + "_" + AtrributeGroupMappingId.ToString();
            requiredField.ControlToValidate = controlTovalidate;
            requiredField.ErrorMessage = Resources.Language.PLEASEENTERTEXT + " " + attributeName;
            requiredField.ValidationGroup = "submitForm" + groupId.ToString();
            requiredField.CssClass = "errmsg";
            requiredField.Enabled = enable;
            requiredField.Display = ValidatorDisplay.Dynamic;// "Dynamic";
            requiredFieldsDiv.Controls.Add(requiredField);
            return requiredFieldsDiv;
        }

        /// <summary>
        /// Add a RegularExpression Validator.
        /// </summary>
        /// <param name="controlTovalidate">Control on which RegularExpression is to be applied</param>
        /// <param name="AttributeType"></param>
        /// <returns></returns>
        private HtmlGenericControl SetRegularExpressionFieldValidator(String controlTovalidate, String validationExpression, String validationMessage, Int32 AtrributeGroupMappingId, String AttributeType, Int32 instanceId, Boolean enable = true)
        {
            HtmlGenericControl regularExpressionDiv = new HtmlGenericControl("div");
            regularExpressionDiv.Attributes.Add("class", "vldx");
            //groupId 
            RegularExpressionValidator regularExpression = new RegularExpressionValidator();
            regularExpression.ID = "rev_" + AttributeType.RemoveWhitespace() + "_" + instanceId.ToString() + "_" + AtrributeGroupMappingId.ToString();
            regularExpression.ControlToValidate = controlTovalidate;
            regularExpression.ErrorMessage = validationMessage;
            regularExpression.ValidationGroup = "submitForm" + groupId.ToString();
            regularExpression.CssClass = "errmsg";
            regularExpression.Enabled = enable;
            regularExpression.Display = ValidatorDisplay.Dynamic;
            regularExpression.ValidationExpression = validationExpression;
            regularExpressionDiv.Controls.Add(regularExpression);
            return regularExpressionDiv;
        }

        /// <summary>
        /// Recursive function to find the control dynamically added
        /// </summary>
        /// <param name="control">Initial control</param>
        /// <param name="attributeDetails">sdictionary with the value of attribute and it i d.</param>
        private void FindControlRecursiveForCustomForms(Control control, Dictionary<Int32, String> attributeDetails, Dictionary<Int32, String> attributeExtraDetails = null)
        {

            //As the WclDatePicker and the WclComboBox is a collection of controls, so trhis check avoid going futher in the control and gets the actual value.
            if (control != null && control.Controls.Count > 0 && !(control.GetType().Name.ToString().Equals("WclDatePicker") || control.GetType().Name.ToString().Equals("WclComboBox")))
            {
                foreach (Control cntrl in control.Controls)
                {
                    FindControlRecursiveForCustomForms(cntrl, attributeDetails, attributeExtraDetails);
                }
            }
            //For more cobntrol just add the OR condition.
            else if (control.GetType().Name.ToString().Equals("WclTextBox") || control.GetType().Name.ToString().Equals("WclNumericTextBox")
                || control.GetType().Name.ToString().Equals("WclDatePicker") || control.GetType().Name.ToString().Equals("WclMaskedTextBox") ||
                control.GetType().Name.ToString().Equals("WclComboBox"))
            {
                SetValue(control, attributeDetails, attributeExtraDetails);
            }
        }

        /// <summary>
        /// Gets the value of the control depending
        /// on their type and set it to dictionary.
        /// </summary>
        /// <param name="control">Control for which we have to get the value</param>
        /// <param name="attributeDetails"></param>
        private void SetValue(Control control, Dictionary<Int32, String> attributeDetails, Dictionary<Int32, String> attributeExtraDetails = null)
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
                        //UAT-2447:
                        Boolean isIntPhoneNumber = true;
                        if (!attributeExtraDetails.IsNullOrEmpty() && attributeExtraDetails.ContainsKey(attributeMappingId))
                        {
                            isIntPhoneNumber = Convert.ToBoolean(attributeExtraDetails[attributeMappingId]);
                        }

                        if (attributeMappingId != 0 && !attributeDetails.ContainsKey(attributeMappingId) && isIntPhoneNumber)
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

                    //UAT-2447:
                    Boolean isIntPhoneNumberChecked = false;
                    if (!attributeExtraDetails.IsNullOrEmpty() && attributeExtraDetails.ContainsKey(attributeMappingId))
                    {
                        isIntPhoneNumberChecked = Convert.ToBoolean(attributeExtraDetails[attributeMappingId]);
                    }
                    if (attributeMappingId != 0 && !isIntPhoneNumberChecked)
                    {
                        attributeDetails.Add(attributeMappingId, maskedTextBox.Text);
                    }
                    break;
                case "WclCheckBox":
                    WclCheckBox isInternationalCheckbox = control as WclCheckBox;

                    attributeMappingId = GetAttributeMappingId(isInternationalCheckbox.ID);
                    if (attributeMappingId != 0)
                    {
                        attributeDetails.Add(attributeMappingId, Convert.ToString(isInternationalCheckbox.Checked));
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
            HtmlGenericControl divMain = new HtmlGenericControl("div");
            divMain.Attributes.Add("class", "sxro sx3co");
            HtmlGenericControl divEDSDocumentLink = new HtmlGenericControl("div");
            divEDSDocumentLink.ID = "divEDSDocumentLink";
            divEDSDocumentLink.Visible = true;
            divMain.Controls.Add(divEDSDocumentLink);
            HtmlGenericControl divLbl = new HtmlGenericControl("div");
            divLbl.Attributes.Add("class", "sxlb");
            HtmlGenericControl divSxLm = new HtmlGenericControl("div");
            divSxLm.Attributes.Add("class", "sxlm");
            HtmlGenericControl divSxrow = new HtmlGenericControl("div");
            divSxrow.Attributes.Add("class", "sxroend");
            Label lblEdsDocument = new Label();
            lblEdsDocument.ID = "lblEdsDocument";
            lblEdsDocument.Text = "Authorization Form";
            divLbl.Controls.Add(lblEdsDocument);
            HyperLink lnkEDSDocument = new HyperLink();
            lnkEDSDocument = pnlRendercustomForm.FindControl("lnkEDSDocumentTemp") as HyperLink;
            lnkEDSDocument.Visible = true;
            divSxLm.Controls.Add(lnkEDSDocument);
            divEDSDocumentLink.Controls.Add(divLbl);
            divEDSDocumentLink.Controls.Add(divSxLm);
            divMain.Controls.Add(divSxrow);
            hdfOrderID.Value = Convert.ToString(OrderId);
            hdfDocumentType.Value = "EDS_AuthorizationForm";
            hdfTenantId.Value = Convert.ToString(tenantId);
            mainPanelForFor.Controls.Add(divMain);
        }

        #region UAT-586

        #endregion

        #region UAT-2447

        private void RecursivInternationalExtraDetail(Control control, Dictionary<Int32, String> attributeExtraDetails)
        {
            //Checkbox Control
            if (control != null && control.Controls.Count > 0 && !(control.GetType().Name.ToString().Equals("WclCheckBox")))
            {
                foreach (Control cntrl in control.Controls)
                {
                    RecursivInternationalExtraDetail(cntrl, attributeExtraDetails);
                }
            }
            else if (control.GetType().Name.ToString().Equals("WclCheckBox"))
            {
                SetValue(control, attributeExtraDetails);
            }
        }
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
                            backgroundOrderData.CustomFormIntPhoneNumExtraData = new Dictionary<int, string>();
                            //UAT-2447
                            RecursivInternationalExtraDetail(pnlRendercustomFormMe, backgroundOrderData.CustomFormIntPhoneNumExtraData);
                            FindControlRecursiveForCustomForms(pnlRendercustomFormMe, backgroundOrderData.CustomFormData, backgroundOrderData.CustomFormIntPhoneNumExtraData);
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
                        //UAT-2065: Additional searches should go in one grid for each type (not one for each additional search) with ability to delete.
                        Panel pnlRendercustomFormMe = null;
                        if (IsSupplementalOrder)
                            pnlRendercustomFormMe = pnlRendercustomForm.FindControl("pnlInner_" + hdnGroupId.Value + "_" + grp) as Panel;
                        else
                            pnlRendercustomFormMe = pnlRendercustomForm.FindControl("pnl_" + grp + "_" + hdnGroupId.Value) as Panel;

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

        //UAT-2216
        private HiddenField CreateHiddenField(Int32 instanceId, Int32 attributeGroupId)
        {
            HiddenField hdnField = new HiddenField();
            hdnField.ID = "hdn_" + instanceId + "_" + attributeGroupId;
            return hdnField;
        }

        /// <summary>
        /// Add the control ID CBI
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void AddControlID(String key, String value)
        {
            if (!lstControlIds.ContainsKey(key))
            {
                var ControlIds = lstControlIds;
                ControlIds.Add(key, value);
                lstControlIds = ControlIds;
            }
        }
    }
}