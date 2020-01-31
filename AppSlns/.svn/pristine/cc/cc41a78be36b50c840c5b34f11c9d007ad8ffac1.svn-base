using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System.Text;
using System.Web.UI.HtmlControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.Globalization;
using Entity.ClientEntity;

namespace CoreWeb.BkgOperations.Views
{
    public partial class CustomFormLoad : BaseUserControl, ICustomFormLoadView
    {
        #region Variables

        #region Private Variables

        private CustomFormLoadPresenter _presenter = new CustomFormLoadPresenter();

        #endregion

        #region Public Variables

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        #endregion

        #endregion

        #region Properties

        #region Private Properties
        private Dictionary<String, String> _qr;// to be accessed only through property queryString

        private Dictionary<String, String> queryString
        {
            get
            {
                if (_qr.IsNullOrEmpty())
                {
                    _qr = new Dictionary<String, String>();
                    //Decrypt the TenantId from Query String.
                    if (!Request.QueryString["args"].IsNull())
                    {
                        _qr.ToDecryptedQueryString(Request.QueryString["args"]);
                    }
                }
                return _qr;
            }
        }
        private CustomFormPageMode CustomFormMode
        {
            get
            {
                if (IsEdit && !IsPostBack)
                    return CustomFormPageMode.ReviewEditClicked;
                if (queryString.ContainsKey("IsPrevious") && !IsPostBack)
                    return CustomFormPageMode.PreviousClicked;
                if (queryString.ContainsKey("NextCustomForm") && !IsPostBack)
                    return CustomFormPageMode.NextClicked;
                if (PageModeType.Equals("IsPrevious") && !IsPostBack && IsAdminOrderScreen)
                    return CustomFormPageMode.PreviousClicked;
                if (PageModeType.Equals("NextCustomForm") && !IsPostBack && IsAdminOrderScreen)
                    return CustomFormPageMode.NextClicked;
                return CustomFormPageMode.Default;
            }
        }
        private ApplicantOrderCart applicantOrderCart = new ApplicantOrderCart();

        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (applicantOrderCart.IsNullOrEmpty())
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return applicantOrderCart;
        }

        #endregion

        #region Public Properties

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (ViewState["TenantId"] != null)
                    return (Int32)(ViewState["TenantId"]);
                return
                   Presenter.GetTenant();
            }
            set
            {
                if (ViewState["TenantId"] == null)
                    ViewState["TenantId"] = value;
            }
        }

        public String PageModeType
        {
            get
            {
                if (ViewState["PageType"] != null)
                    return Convert.ToString(ViewState["PageType"]);
                return String.Empty;
            }
            set
            {
                ViewState["PageType"] = value;
            }
        }

        public CustomFormLoadPresenter Presenter
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

        public Boolean IsEdit
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                return applicantOrderCart.IsEditMode;
            }
            set
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                applicantOrderCart.IsEditMode = value;
            }
        }

        public List<CustomFormDataContract> lstCustomForm
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (applicantOrderCart.lstCustomFormData.IsNullOrEmpty())
                {
                    return new List<CustomFormDataContract>();
                }
                return applicantOrderCart.lstCustomFormData;
            }
            set
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (applicantOrderCart.lstCustomFormData.IsNullOrEmpty())
                {
                    applicantOrderCart.lstCustomFormData = new List<CustomFormDataContract>();
                }
                applicantOrderCart.lstCustomFormData = value;
            }
        }

        public Int32 CurrentFormId
        {
            get
            {
                return hdnFormId.Value.IsNullOrEmpty() ? 0 : Convert.ToInt32(hdnFormId.Value);
            }
            set
            {
                hdnFormId.Value = Convert.ToString(value);
            }
        }

        public List<Int32> lstFormExecuted
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                return applicantOrderCart.lstFormExecuted;
            }
            set
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                applicantOrderCart.lstFormExecuted = value;
            }
        }

        public CustomFormDataContract customFormContract { get; set; }

        public List<CustomFormDataContract> lstDataSourceRepeater { get; set; }

        public List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }

        public List<BackgroundPackagesContract> lstPackages
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (applicantOrderCart.IsNotNull())
                {
                    if (applicantOrderCart.lstApplicantOrder.IsNotNull())
                    {
                        return applicantOrderCart.lstApplicantOrder[0].lstPackages;
                    }
                }
                return new List<BackgroundPackagesContract>();
            }
        }

        public List<BackgroundOrderData> lstBackgroundOrderData
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (!applicantOrderCart.IsNullOrEmpty())
                    return applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty()
                                                ? new List<BackgroundOrderData>()
                                                : applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData;
                return new List<BackgroundOrderData>();
            }
        }

        public List<BackgroundOrderData> lstBkgGrdOrderForCurrentForm
        {
            get;
            set;
        }

        public Int32 OrderCartCustomFormId
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (!applicantOrderCart.IsNullOrEmpty())
                    return applicantOrderCart.CurrentCustomFormId;
                return AppConsts.NONE;
            }
            set
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                applicantOrderCart.CurrentCustomFormId = value;
            }
        }

        public Int32 CustomFormId
        {
            get;
            set;
        }

        public List<CustomFormUserData> lstDataForCustomForm { get; set; }

        #region E DRUG SCREENING PROPERTIES
        public Int32 EDrugScreenCustomFormId
        {
            get
            {
                return hdnEDrugScreenCustomFormId.Value.IsNullOrEmpty() ? 0 : Convert.ToInt32(hdnEDrugScreenCustomFormId.Value);
            }
            set
            {
                hdnEDrugScreenCustomFormId.Value = Convert.ToString(value);
            }
        }
        public Int32 EDrugScreenAttributeGroupId
        {
            get
            {
                return hdnEDrugScreenAttributeGrupId.Value.IsNullOrEmpty() ? 0 : Convert.ToInt32(hdnEDrugScreenAttributeGrupId.Value);
            }
            set
            {
                hdnEDrugScreenAttributeGrupId.Value = Convert.ToString(value);
            }
        }
        #endregion

        #region UAT-2842
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
                hdnIsAdminCreateOrder.Value = Convert.ToString(value);
            }
        }

        public String CustomFormInstanceNumber
        {
            set
            {
                hdnGroupidandIntanceNumber.Value = value;
            }
        }

        public String CustomFormHiddenPanels
        {
            set
            {
                hdnHiddenPanels.Value = value;
            }
        }

        public String ValidationMessage { get; set; }
        #endregion

        //Globalization
        public String LanguageCode
        {
            get
            {
                LanguageContract languageContract = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!languageContract.IsNullOrEmpty())
                {
                    return languageContract.LanguageCode;
                }
                return Languages.ENGLISH.GetStringValue();
            }
        }


        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                hdnLanguageCode.Value = LanguageCode;
                base.Title = Resources.Language.ORDER;
                base.BreadCrumbTitleKey = "Key_ORDER";
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
            var orderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            SetCustomFormId(queryString);
            if (!IsPostBack)
            {
                if (!IsAdminOrderScreen)
                {
                    RedirectIfIncorrectOrderStage(orderCart);
                    fsucCmdBar1.ExtraButton.Style["display"] = "none";
                }
                else
                {
                    fsucCmdBar1.SaveButton.Style["display"] = "none";
                    fsucCmdBar1.SubmitButton.Style["display"] = "none";
                    fsucCmdBar1.ClearButton.Style["display"] = "none";
                    //UAT-2855
                    //if (orderCart.AdminOrderSteps > AppConsts.NONE)
                    //{
                    //    fsucCmdBar1.SubmitButton.Enabled = true;
                    //}
                    //else
                    //{
                    //    fsucCmdBar1.SubmitButton.Enabled = false;
                    //}
                }
                RedirectInvalidOrder(orderCart);
                SetButtonText();
            }

            if (this.CustomFormMode == CustomFormPageMode.ReviewEditClicked || this.CustomFormMode == CustomFormPageMode.PreviousClicked)
            {
                FetchDataInstanceCountFrmOrderCart();
            }

            BindCustomFormOrRedirect();
            SetScreenDisplay();
        }

        /// <summary>
        /// Page_PreRenderComplete event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                List<BackgroundOrderData> lstBackgroundOrderData = new List<BackgroundOrderData>();
                Int32 controlCount = AppConsts.NONE;
                StringBuilder xmlStringData = new StringBuilder();
                if (!IsAdminOrderScreen)
                    controlCount = pnlLoader.Controls.Count;
                else
                    controlCount = pnlLoader.Controls.Count;

                if (controlCount > 0)
                {
                    for (Int32 i = 0; i < controlCount; i++)
                    {
                        //panel for electronic drug screening.
                        if (pnlLoader.Controls[i] is DrugScreenDataControl)
                        {
                            DrugScreenDataControl drugScreenDataCntrl = pnlLoader.Controls[i] as DrugScreenDataControl;
                            lstBackgroundOrderData.AddRange(drugScreenDataCntrl.GetEDrugData());
                        }
                        else if (pnlLoader.Controls[i] is CustomFormHtlm)
                        {
                            CustomFormHtlm customFormHtlm = pnlLoader.Controls[i] as CustomFormHtlm;
                            lstBackgroundOrderData.AddRange(customFormHtlm.GetData());
                        }
                    }

                    if (applicantOrderCart.IsLocationServiceTenant)
                    {
                        xmlStringData.Append("<Attributes>");
                        foreach (BackgroundOrderData item in lstBackgroundOrderData)
                        {
                            foreach (var dic in item.CustomFormData.Where(cond => !cond.Value.IsNullOrEmpty()))
                            {
                                xmlStringData.Append("<Attribute><InstanceID>" + item.InstanceId + "</InstanceID><AttributeID>" + dic.Key + "</AttributeID><AttributeValue>" + System.Security.SecurityElement.Escape(dic.Value) + "</AttributeValue></Attribute>");
                            }
                        }
                        xmlStringData.Append("</Attributes>");
                    }

                    List<CustomFormAutoFillDataContract> lstAttributes = Presenter.GetConditionsforAttributes(xmlStringData);

                    for (Int32 i = 0; i < controlCount; i++)
                    {
                        if (pnlLoader.Controls[i] is CustomFormHtlm)
                        {
                            CustomFormHtlm customFormHtlm = pnlLoader.Controls[i] as CustomFormHtlm;
                            if (lstAttributes.Where(cond => cond.AttributeGroupID == customFormHtlm.groupId && cond.InstanceId == customFormHtlm.InstanceId).ToList().Count > 0)
                            {
                                Dictionary<String, String> controls = customFormHtlm.lstControlIds;
                                if (controls.Count > AppConsts.NONE)
                                {
                                    foreach (CustomFormAutoFillDataContract item in lstAttributes.Where(cond => cond.IsAttributeGroupHidden))
                                    {
                                        HtmlGenericControl group = customFormHtlm.FindControl("mainDiv_" + item.AttributeGroupID.ToString() + "_" + item.InstanceId.ToString()) as HtmlGenericControl;
                                        if (group.IsNotNull())
                                        {
                                            group.Visible = false;
                                            group.InnerHtml = "";
                                        }
                                    }

                                    foreach (CustomFormAutoFillDataContract item in lstAttributes.Where(cond => !string.IsNullOrWhiteSpace(cond.HeaderLabel)))
                                    {
                                        HtmlGenericControl group = customFormHtlm.FindControl("mainDiv_" + item.AttributeGroupID.ToString() + "_" + item.InstanceId.ToString()) as HtmlGenericControl;
                                        if (group.IsNotNull())
                                        {
                                            Label headerLable = group.FindControl("lblHeader_" + item.AttributeGroupID.ToString() + "_" + item.InstanceId.ToString()) as Label;
                                            if (headerLable != null)
                                            {
                                                headerLable.Text = item.HeaderLabel.HtmlEncode();
                                            }
                                            //headerLable.ID = "lblHeader_" + groupId.ToString() + "_" + currentInstanceId.ToString();
                                            //group.FindControl("")
                                            //group.Visible = false;
                                            //group.InnerHtml = "";
                                        }
                                    }

                                    foreach (CustomFormAutoFillDataContract item in lstAttributes.Where(cond => cond.IsAttributeHidden))
                                    {
                                        if (controls.ContainsKey(item.AttributeID + "_" + item.InstanceId))
                                        {
                                            Control tempcontrol = customFormHtlm.FindControl(controls.GetValue(item.AttributeID + "_" + item.InstanceId)) as Control;
                                            if (tempcontrol.GetType().Name == "WclTextBox")
                                            {
                                                (tempcontrol as WclTextBox).Text = string.Empty;
                                                (tempcontrol as WclTextBox).Enabled = false;
                                            }
                                            else if (tempcontrol.GetType().Name == "WclComboBox")
                                            {
                                                (tempcontrol as WclComboBox).Enabled = false;
                                            }
                                            else
                                            {
                                                tempcontrol.Visible = false;
                                            }
                                        }
                                    }
                                }
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



        #endregion

        #region Button events

        /// <summary>
        /// It saves the data of the current custom form in the session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSave_Click(object sender, EventArgs e)
        {
            OrderCartCustomFormId = AppConsts.NONE;
            SaveDataOfForm();
            if (applicantOrderCart.IsLocationServiceTenant && !ValidationMessage.IsNullOrEmpty())
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "hideExtraValidation();", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "showExtraValidation('" + ValidationMessage + "');", true);
                return;
            }

            LoadNextFormOrNextPage();
        }

        /// <summary>
        /// Event for Backward navigation i.e. Previous or Restart Order button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarRestart_Click(object sender, EventArgs e)
        {
            applicantOrderCart.DecrementOrderStepCount();
            Dictionary<String, String> queryString = null;

            var _formToLoad = AppConsts.NONE;
            var _executedForms = applicantOrderCart.lstFormExecuted;
            var _bkgOrderData = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData;
            var _isSingleForm = applicantOrderCart.lstCustomFormData.Count() == AppConsts.ONE ? true : false;

            var _currentFormIndex = (_isSingleForm || _executedForms.IsNullOrEmpty())
                                   ? AppConsts.NONE
                                   : _executedForms.IndexOf(this.OrderCartCustomFormId);

            // Case if there are more then one custom forms and user has already moved to second form, AFTER saving data of first Form
            // OR 
            // Case if there is ONLY ONE CUSTOM FORM and user is navigating from Disclosure form, through Previous button
            // and again press Previous button on the SINGLE Custom Form
            if (!_isSingleForm && !_bkgOrderData.IsNullOrEmpty()
                ||
               (_isSingleForm && !_bkgOrderData.IsNullOrEmpty()))
            {
                IsEdit = true;


                // Executed Count if if the user is continuously pressing PREVIOUS from CUSTOM forms and has reached the FIRST form in the list,
                // goes to Applicant profile, then comes to First page again and again presses PREVIOUS button
                if (_executedForms.IsNullOrEmpty())
                    _formToLoad = AppConsts.NONE;
                else
                {
                    // Case, if its a single Custom form OR
                    // OR
                    // Case, if the user is continuously pressing PREVIOUS from CUSTOM forms and has reached the FIRST form in the list
                    _formToLoad = _isSingleForm || (!_executedForms.IsNullOrEmpty() && _executedForms.Count() == 1)
                                ? _executedForms[AppConsts.NONE]
                                : _executedForms[_currentFormIndex - 1];
                }
            }
            else
                IsEdit = false;

            //Remove the form from the list to which the user is navigating
            if (!applicantOrderCart.lstFormExecuted.IsNullOrEmpty())
                applicantOrderCart.lstFormExecuted.Remove(this.OrderCartCustomFormId);

            // 1. Case if there is only one Single Form
            // OR
            // 2. Case if there are MULTIPLE CUSTOM FORM and user is navigating through Previous button
            // and again press Previous button on the FIRST Custom Form in the series
            // OR
            // 3. Case if there are MULTIPLE CUSTOM FORM and user is navigating through Previous button on VERY FIRST Custom Form
            if ((_isSingleForm)
                ||
                (!_isSingleForm && _currentFormIndex == 0 && !_bkgOrderData.IsNullOrEmpty())
                ||
                (!_isSingleForm && _bkgOrderData.IsNullOrEmpty()))
            {
                applicantOrderCart.AddOrderStageTrackID(OrderStages.ApplicantProfile);
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  ChildControls.ApplicantProfile}
                                                                 };
                Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            }
            else if (IsAdminOrderScreen)
            {
                applicantOrderCart.AdminOrderSteps = applicantOrderCart.AdminOrderSteps - 1;
                Response.Redirect(String.Format("~/BkgOperations/Pages/CustomFormPage.aspx?SelectedTenantId=" + Convert.ToString(TenantId) + "&CustomFormId=" + Convert.ToString(_formToLoad) + "&IsPrevious=1"));
            }
            else
            {
                //applicantOrderCart.EDrugScreeningRegistrationId = null;
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD, ChildControls.CustomFormLoad},
                                                                    {"CustomFormId",Convert.ToString(_formToLoad)},
                                                                    {"IsPrevious","1"}
                                                                 };
                Response.Redirect(String.Format("~/BkgOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            }
        }

        protected void fsucCmdBar1_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
                //UAT-1560
                Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
                Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
                Response.Redirect("~/Main/Default.aspx");
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public methods

        #endregion

        #region Private methods

        /// <summary>
        /// Read the data from the Query string and set the  'CustomFormId' properties
        /// </summary>
        private void SetCustomFormId(Dictionary<String, String> queryString)
        {
            if (queryString.ContainsKey("CustomFormId") && CustomFormId == 0)
            {
                CustomFormId = Convert.ToInt32(queryString["CustomFormId"]);
            }
            //UAT-2842
            if (queryString.ContainsKey("SelectedTenantId"))
            {
                TenantId = Convert.ToInt32(queryString["SelectedTenantId"]);
            }
            if (queryString.ContainsKey("IsAdminOrderScreen"))
            {
                IsAdminOrderScreen = Convert.ToBoolean(queryString["IsAdminOrderScreen"]);
            }
            if (!queryString.ContainsKey("IsPrevious") && !PageModeType.Equals("IsPrevious"))
            {
                if (OrderCartCustomFormId > 0)
                {
                    CustomFormId = OrderCartCustomFormId;
                }
            }
        }

        /// <summary>
        /// It loads the next custom form in queue
        /// </summary>
        private void LoadNextFormOrNextPage()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            CustomFormDataContract data = null;
            Int32 nextCustomFormId = 0;
            String redirectUrl = String.Empty;
            if (lstFormExecuted.IsNullOrEmpty() && !lstCustomForm.IsNullOrEmpty())
            {
                data = new CustomFormDataContract();
                data = lstCustomForm.FirstOrDefault(x => x.customFormId != CurrentFormId);
                lstFormExecuted = new List<int>();
                lstFormExecuted.Add(CurrentFormId);
            }
            else if (!lstCustomForm.IsNullOrEmpty())
            {
                data = new CustomFormDataContract();
                data = lstCustomForm.FirstOrDefault(x => !lstFormExecuted.Contains(x.customFormId));
            }

            OrderCartCustomFormId = AppConsts.NONE;
            if (!data.IsNullOrEmpty())
            {
                nextCustomFormId = data.customFormId;
                lstFormExecuted.Add(data.customFormId);

                if (IsAdminOrderScreen)
                {
                    applicantOrderCart.AdminOrderSteps = applicantOrderCart.AdminOrderSteps + 1;
                    Response.Redirect(String.Format("~/BkgOperations/Pages/CustomFormPage.aspx?SelectedTenantId=" + Convert.ToString(TenantId) + "&CustomFormId=" + Convert.ToString(nextCustomFormId) + "&NextCustomForm=" + Convert.ToString(nextCustomFormId) + "&IsAdminEditMode=false&IsNewCustomForm=false&IsEdit=" + IsEdit.ToString()));
                }
                else
                {
                    queryString = new Dictionary<String, String>
                                        {
                                            { AppConsts.CHILD, ChildControls.CustomFormLoad},
                                            { "NextCustomForm", Convert.ToString(nextCustomFormId)},
                                            { "CustomFormId", Convert.ToString(nextCustomFormId)},
                                            {"IsEdit",IsEdit.ToString()},
                                        };
                    redirectUrl = "~/BkgOperations/Default.aspx?args={0}";
                    Response.Redirect(String.Format(redirectUrl, queryString.ToEncryptedQueryString()));
                }
            }
            else if (IsAdminOrderScreen)
            {
                String SelectedTenantId = Convert.ToString(TenantId);
                String OrderID = Convert.ToString(applicantOrderCart.lstApplicantOrder[0].OrderId);
                queryString = new Dictionary<String, String>
                                        {
                                            { "Child", ChildControls.AdminCreateOrderDetails},
                                            { "OrderID",OrderID},
                                            {"SelectedTenantId",SelectedTenantId},
                                            {"IsCustomFormDetailsSave","True"}
                                        };
                string url = String.Format("/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                //Response.Redirect(String.Format(url));
                //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefeshPage('" + url + "');", true);
                Response.Redirect(String.Format("~/BkgOperations/Pages/CustomFormPage.aspx?SelectedTenantId=" + Convert.ToString(TenantId) + "&IsNewCustomForm=false&IsAdminEditMode=true"));
            }
            else
            {
                applicantOrderCart.AddOrderStageTrackID(OrderStages.CustomFormsCompleted);
                #region Order Flow EventType

                if (applicantOrderCart.IsLocationServiceTenant && applicantOrderCart.FingerPrintData.IsEventCode)
                {

                    if (applicantOrderCart.IsLocationServiceTenant && applicantOrderCart.FingerPrintData.IsEventCode)
                    {
                        applicantOrderCart.IncrementOrderStepCount();
                        queryString = new Dictionary<String, String>
                                                         {
                                                            { AppConsts.CHILD,  ChildControls.ApplicantOrderReview}
                                                         };
                        Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                    }

                    #endregion
                }
                else
                    if (applicantOrderCart.IsLocationServiceTenant)
                {
                    #region UAT - 4331 : change schedule appointment to step 2 of order flow
                    ////// Comment previous code and replace with new one as per the changes required for UAT -4331

                    //if (applicantOrderCart.FingerPrintData.IsOutOfState)
                    //{
                    //    queryString = new Dictionary<String, String>
                    //                                     {
                    //                                        { AppConsts.CHILD,  ChildControls.ApplicantOrderReview}
                    //                                     };
                    //    Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                    //}
                    //else
                    //{
                    //    queryString = new Dictionary<String, String>
                    //                                            {
                    //                                                //{AppConsts.CHILD,ChildControls.APPLICANT_APPOINTMENT_SCHEDULE}  //// 4331 : change schedule appointment to step 2 of order flow
                    //                                                
                    //                                            };
                    //    redirectUrl = "~/FingerPrintSetUp/Default.aspx?args={0}";
                    //}
                    queryString = new Dictionary<String, String>
                                                       {
                                                          { AppConsts.CHILD,  ChildControls.ApplicantOrderReview}
                                                       };
                    Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));


                    #endregion


                }
                else
                {
                    queryString = new Dictionary<String, String>
                                                                { 
                                                                    //{ "Child", ChildControls.ApplicantProfile}  // UAT-5184
                                                                       { AppConsts.CHILD, ChildControls.ApplicantDisclaimerPage }  // UAT-5184
                                                                };
                    redirectUrl = "~/ComplianceOperations/Default.aspx?args={0}";
                }

                IsEdit = false;
                Response.Redirect(String.Format(redirectUrl, queryString.ToEncryptedQueryString()));
            }
        }

        /// <summary>
        /// Gets the custom form loaded for the 
        /// </summary>
        private void BindCustomFormOrRedirect()
        {
            String packageIds = String.Empty;
            packageIds = GetPackageIdString();

            if (!packageIds.Equals(String.Empty) && lstCustomForm.IsNullOrEmpty())
            {
                Presenter.GetCustomFormsForThePackage(packageIds);
            }

            #region E DRUG SCREENING
            if (EDrugScreenCustomFormId.IsNullOrEmpty() || EDrugScreenCustomFormId == 0)
            {
                Presenter.GetEDrugAttributeGroupIdAndFormId();
            }
            #endregion

            //lstDataSourceRepeater = new List<CustomFormDataContract>();
            if (!lstCustomForm.IsNullOrEmpty())
            {
                lstDataSourceRepeater = new List<CustomFormDataContract>();
                CustomFormDataContract customFormDataContract = new CustomFormDataContract();
                if (CustomFormId > 0)
                {
                    customFormDataContract = lstCustomForm.FirstOrDefault(x => x.customFormId == CustomFormId);
                }
                else
                {
                    customFormDataContract = lstCustomForm.FirstOrDefault();
                }

                var CurrentFormDataInOrderCart = lstBackgroundOrderData.Where(x => x.CustomFormId == CustomFormId).Select(x => x).ToList();

                if (!CurrentFormDataInOrderCart.IsNullOrEmpty())
                {
                    lstBkgGrdOrderForCurrentForm = CurrentFormDataInOrderCart;
                }

                if (!customFormDataContract.IsNullOrEmpty())
                {
                    OrderCartCustomFormId = CurrentFormId = customFormDataContract.customFormId;
                    //Get the skeleton data to create the Html.
                    Presenter.GetAttributesForTheCustomForm(packageIds, CurrentFormId, LanguageTranslateUtils.GetCurrentLanguageFromSession().LanguageCode);
                    if (!lstCustomFormAttributes.IsNullOrEmpty() && lstCustomFormAttributes.Any(x => x.IsDisplay))
                    {
                        customFormDataContract.lstCustomFormAttributes = lstCustomFormAttributes;
                        List<Int32> groupIds = lstCustomFormAttributes.DistinctBy(x => x.AttributeGroupId).OrderBy(x => x.Sequence).Select(x => x.AttributeGroupId).ToList();
                        for (int i = 0; i < groupIds.Count; i++)
                        {
                            //if it is elecetroni9c drug screening group
                            if (lstCustomFormAttributes.Any(x => x.IsDisplay && x.AttributeGroupId == groupIds[i]))
                            {
                                CustomFormDataContract newCustomFormData = GetNewInstanceForTheForm(customFormDataContract, groupIds[i]);
                                lstDataSourceRepeater.Add(newCustomFormData);
                            }
                        }

                        //Call the method that actually loads the data 
                        BindCustomForms(lstDataSourceRepeater);
                        if (!lstBkgGrdOrderForCurrentForm.IsNullOrEmpty())
                        {
                            SetIsDecisionFields();
                            //UAT-2216:Remove "End Date" from current employer (not previous employers) on Employment Verification.
                            SetDataForEmployerField();
                        }
                    }
                    else
                    {
                        //incase there is no attributes with is display=true in the current custom form
                        LoadNextFormOrNextPage();
                    }
                }

            }
            else
            {
                LoadNextFormOrNextPage();
            }
        }

        private void SetIsDecisionFields()
        {
            List<AttributesForCustomFormContract> lstIsDecisionFields = lstCustomFormAttributes.Where(x => x.IsDecisionField).ToList();
            if (!lstIsDecisionFields.IsNullOrEmpty())
            {
                foreach (AttributesForCustomFormContract attrCustmFrm in lstIsDecisionFields)
                {
                    List<BackgroundOrderData> lstOrderDataForGroup = lstBkgGrdOrderForCurrentForm.Where(x => x.BkgSvcAttributeGroupId == attrCustmFrm.AttributeGroupId).ToList();
                    foreach (BackgroundOrderData backgroundOrderData in lstOrderDataForGroup)
                    {
                        if (backgroundOrderData.CustomFormData.ContainsKey(attrCustmFrm.AtrributeGroupMappingId) && (backgroundOrderData.CustomFormData[attrCustmFrm.AtrributeGroupMappingId].Equals("False") || backgroundOrderData.CustomFormData[attrCustmFrm.AtrributeGroupMappingId].Equals("No")))
                        {
                            SetValueForDecisionFieldsInHiddenField(attrCustmFrm.AttributeGroupId + "_" + backgroundOrderData.InstanceId);
                        }
                    }
                }
            }
        }

        private void SetValueForDecisionFieldsInHiddenField(String value)
        {
            String[] decisionValues = hdnIsedcisionField.Value.Split(',');
            if (!decisionValues.Contains(value))
            {
                hdnIsedcisionField.Value = hdnIsedcisionField.Value.IsNullOrEmpty() ? value : hdnIsedcisionField.Value + "," + value;
            }
        }

        /// <summary>
        /// Gets the ',' seperated string  of list of package Ids.
        /// </summary>
        /// <returns></returns>
        private string GetPackageIdString()
        {
            String packages = String.Empty;
            if (!lstPackages.IsNullOrEmpty())
            {
                lstPackages.ForEach(x => packages += Convert.ToString(x.BPAId) + ",");
                //packages = "4";
                if (packages.EndsWith(","))
                    packages = packages.Substring(0, packages.Length - 1);
            }
            return packages;
        }

        /// <summary>
        /// It loads the user control depending upon the 
        /// number of instance to be added
        /// </summary>
        /// <param name="lstDataSourceRepeater"></param>
        private void BindCustomForms(List<CustomFormDataContract> lstDataSourceRepeater)
        {
            foreach (var grp in lstDataSourceRepeater)
            {
                if ((EDrugScreenAttributeGroupId > 0 && EDrugScreenCustomFormId > 0) && (grp.groupId == EDrugScreenAttributeGroupId && grp.customFormId == EDrugScreenCustomFormId))
                {
                    LoadElectronicDrugScreeningForm(grp);
                    break;
                }
                else
                {
                    AddGroups(grp);
                }
            }
        }

        /// <summary>
        /// It loads the user control depending upon the 
        /// number of instance to be added
        /// </summary>
        /// <param name="grp"></param>
        private void AddGroups(CustomFormDataContract grp)
        {
            CustomFormHtlm _customForm = Page.LoadControl("~/BkgOperations/UserControl/CustomFormHtlm.ascx") as CustomFormHtlm;
            _customForm.lstCustomFormAttributes = grp.lstCustomFormAttributes;
            _customForm.groupId = grp.groupId;
            _customForm.InstanceId = grp.instanceId;
            _customForm.CustomFormId = CurrentFormId;
            _customForm.tenantId = TenantId;
            _customForm.IsEdit = IsEdit;
            _customForm.lstHiddenInstanceOfGroup = GetListOfHiddenInstance(grp.groupId);
            _customForm.lstBackgroundOrderData = lstBkgGrdOrderForCurrentForm;
            _customForm.IsLocationServiceTenant = applicantOrderCart.IsLocationServiceTenant;
            if (applicantOrderCart.IsLocationServiceTenant && applicantOrderCart.FingerPrintData.IsNotNull() && !applicantOrderCart.FingerPrintData.lstAutoFilledAttributes.IsNullOrEmpty())
             {
                    List<Int32> attributeIds = grp.lstCustomFormAttributes.Where(cnd => cnd.AttributeGroupId == grp.groupId).Select(x => x.AttributeId).ToList();
                    if (!attributeIds.IsNullOrEmpty() && applicantOrderCart.FingerPrintData.lstAutoFilledAttributes.Any(an => attributeIds.Contains(an.Key))
                        && applicantOrderCart.IscabsFreshSelected == true)
                    {
                        _customForm.IsEdit = false;
                        applicantOrderCart.IscabsFreshSelected = false;
                        SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
                    }
                    _customForm.lstAutoFillCascadingAttributes = applicantOrderCart.FingerPrintData.lstAutoFilledAttributes;
             }
            pnlLoader.Controls.Add(_customForm);

        }

        /// <summary>
        /// This Function gets the new instance of the form 
        /// </summary>
        /// <param name="customFormDataContract">Previous instance</param>
        /// <param name="i">Instance Number</param>
        /// <returns>New instance</returns>
        private CustomFormDataContract GetNewInstanceForTheForm(CustomFormDataContract customFormDataContract, int groupId)
        {
            CustomFormDataContract newCustomFormData = new CustomFormDataContract();
            newCustomFormData.lstCustomFormAttributes = customFormDataContract.lstCustomFormAttributes;
            newCustomFormData.customFormId = customFormDataContract.customFormId; ;
            newCustomFormData.groupId = groupId;
            newCustomFormData.instanceId = GetInstanceIdForTheGroup(groupId);
            return newCustomFormData;
        }

        /// <summary>
        /// This function return the current 
        /// number of instance for the particular group.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private Int32 GetInstanceIdForTheGroup(Int32 groupId)
        {
            Int32 instanceId = 1;
            if (!hdnGroupidandIntanceNumber.Value.Equals(""))
            {
                String[] groupIdAndInstance = hdnGroupidandIntanceNumber.Value.Split(':');
                if (groupIdAndInstance.Count() > 0)
                {
                    for (Int32 i = 0; i < groupIdAndInstance.Count(); i++)
                    {
                        String[] groupIdRelatedData = groupIdAndInstance[i].Split('_');
                        if (groupIdRelatedData[0] == groupId.ToString())
                        {
                            return Convert.ToInt32(groupIdRelatedData[1]);
                        }
                    }
                }
            }

            return instanceId;
        }

        public List<Int32> GetListOfHiddenInstance(Int32 groupId)
        {
            List<Int32> lstDeletedInstance = null;
            if (hdnHiddenPanels.Value.IsNullOrEmpty())
            {
                lstDeletedInstance = new List<Int32>();
                return lstDeletedInstance;
            }
            else
            {
                lstDeletedInstance = new List<Int32>();
                String[] groupInstanceIds = hdnHiddenPanels.Value.Split(':');
                if (groupInstanceIds.Count() > 0)
                {
                    for (Int32 i = 0; i < groupInstanceIds.Count(); i++)
                    {
                        String[] finalData = groupInstanceIds[i].Split('_');
                        if (finalData[0].Equals(groupId.ToString()))
                        {
                            lstDeletedInstance.Add(Convert.ToInt32(finalData[1]));
                        }
                    }

                }
                return lstDeletedInstance;
            }
        }

        /// <summary>
        /// Set the button depending on the occurence of the form
        /// </summary>
        private void SetScreenDisplay()
        {
            fsucCmdBar1.SubmitButton.ValidationGroup = "submitForm";
            fsucCmdBar1.CancelButton.ValidationGroup = "submitForm";
            if (!IsAdminOrderScreen)
            {
                base.Title = "Order";
                String _currentStep = String.Empty;

                if (IsEdit)
                {
                    Int32 previousStepNo = GetCurrentOrderStep();
                    _currentStep = "(Step " + (previousStepNo) + " of " + applicantOrderCart.GetTotalOrderSteps() + ")";
                }
                else
                    //_currentStep = "(Step " + (applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep) + " of " + applicantOrderCart.GetTotalOrderSteps() + ")";
                    _currentStep = "(" + Resources.Language.STEP + " " + (applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep) + " " + Resources.Language.OF + " " + applicantOrderCart.GetTotalOrderSteps() + ")";

                base.SetPageTitle(_currentStep);

                if (!applicantOrderCart.IsNullOrEmpty() &&
                    (applicantOrderCart.OrderRequestType == OrderRequestType.NewOrder.GetStringValue()
                    || applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue()))
                    (this.Page as CoreWeb.BkgOperations.Views.BackgroundPackageAdministrationDefault).SetModuleTitle(Resources.Language.CREATODR);
                else
                    (this.Page as CoreWeb.BkgOperations.Views.BackgroundPackageAdministrationDefault).SetModuleTitle("Renewal Order");
            }
        }

        /// <summary>
        /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void RedirectIfIncorrectOrderStage(ApplicantOrderCart applicantOrderCart)
        {
            String _nextPagePath = _presenter.GetNextPagePathByOrderStageID(applicantOrderCart);

            //Redirect to next page path if Order Status track is not correct.
            if (!String.IsNullOrEmpty(_nextPagePath) && !IsEdit)
                Response.Redirect(_nextPagePath);
            else
            {
                applicantOrderCart.AddOrderStageTrackID(OrderStages.CustomForms);
            }
        }

        /// <summary>
        /// It gets all the data entered by the user and save it to the order cart.
        /// </summary>
        private void SaveDataOfForm()
        {
            applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            Int32 controlCount = AppConsts.NONE;

            if (!IsAdminOrderScreen)
                controlCount = pnlLoader.Controls.Count;
            else
                controlCount = pnlLoader.Controls.Count;

            if (controlCount > 0)
            {
                if (applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty() && !IsEdit)
                {
                    applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData = new List<BackgroundOrderData>();
                }
                else
                {
                    //Remove the previous data in the session oin edit mode to save the fresh data.
                    applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.RemoveAll(x => x.CustomFormId == CurrentFormId);
                }

                for (Int32 i = 0; i < controlCount; i++)
                {
                    //panel for electronic drug screening.
                    if (pnlLoader.Controls[i] is DrugScreenDataControl)
                    {
                        DrugScreenDataControl drugScreenDataCntrl = pnlLoader.Controls[i] as DrugScreenDataControl;
                        List<BackgroundOrderData> lstBackgroundOrderData = drugScreenDataCntrl.GetEDrugData();
                        applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.AddRange(lstBackgroundOrderData);
                    }
                    else if (pnlLoader.Controls[i] is CustomFormHtlm)
                    {
                        CustomFormHtlm customFormHtlm = pnlLoader.Controls[i] as CustomFormHtlm;
                        List<BackgroundOrderData> lstBackgroundOrderData = customFormHtlm.GetData();
                        applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.AddRange(lstBackgroundOrderData);
                    }
                }

                //UAT 3573 validation the custom attributes
                if (applicantOrderCart.IsLocationServiceTenant)
                {
                    var languageCode = Languages.ENGLISH.GetStringValue();
                    var _currentCulture = SysXWebSiteUtils.SessionService.GetCustomData("LanguageCulture");

                    if (_currentCulture.IsNotNull())
                    {
                        languageCode = ((LanguageContract)(_currentCulture)).LanguageCode;
                    }

                    ValidationMessage = Presenter.ValidatePageData(applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData, true, languageCode);
                    StringBuilder xmlStringData = new StringBuilder();
                    xmlStringData.Append("<Attributes>");
                    foreach (BackgroundOrderData item in lstBackgroundOrderData)
                    {
                        foreach (var dic in item.CustomFormData.Where(cond => !cond.Value.IsNullOrEmpty()))
                        {
                            xmlStringData.Append("<Attribute><BkgAttributeGroupMappingID>" + dic.Key + "</BkgAttributeGroupMappingID><AttributeValue>" + System.Security.SecurityElement.Escape(dic.Value) + "</AttributeValue></Attribute>");
                        }
                    }
                    xmlStringData.Append("</Attributes>");
                    Presenter.SaveCustomFormApplicantData(xmlStringData.ToString(), CurrentUserId);
                }
            }

            //UAT 3573 validation the custom attributes
            if (ValidationMessage.IsNullOrEmpty())
            {
                applicantOrderCart.IncrementOrderStepCount();
            }
        }

        /// <summary>
        /// 1. Set's which custom forms have been already loaded and which are required to be loaded
        /// 2. Also sets he CustomGroups to be loaded and the number of instances for each group.
        /// Initially it was called setData()
        /// it sets the data in the session in edit mode.
        /// It is done in order to maintain the state of the page in the state as it was
        /// when the page was loaded for the first time.
        /// </summary>
        private void FetchDataInstanceCountFrmOrderCart()
        {
            List<Int32> lstCustomFormId = new List<Int32>();
            List<Int32> lstGroupIds = new List<Int32>();
            lstCustomFormId = lstBackgroundOrderData.DistinctBy(x => x.CustomFormId).Select(x => x.CustomFormId).ToList();
            if (lstCustomForm.IsNullOrEmpty())
            {
                lstCustomForm = lstCustomFormId.Select(x => new CustomFormDataContract
                {
                    CustomFormName = String.Empty,
                    customFormId = x,
                }).ToList();
            }
            if (!lstFormExecuted.IsNullOrEmpty())
            {
                lstFormExecuted = new List<Int32>();

                foreach (var i in lstCustomForm)
                {
                    if (i.customFormId == CustomFormId)
                    {
                        lstFormExecuted.Add(i.customFormId);
                        break;
                    }
                    lstFormExecuted.Add(i.customFormId);
                }
            }

            applicantOrderCart = GetApplicantOrderCart();
            applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = GetCurrentOrderStep();
            String finalHiddenValue = String.Empty;

            List<BackgroundOrderData> newLstBackGroundOrderData = new List<BackgroundOrderData>();
            newLstBackGroundOrderData = lstBackgroundOrderData.Where(x => x.CustomFormId == CustomFormId).Select(x => x).ToList();
            lstBkgGrdOrderForCurrentForm = newLstBackGroundOrderData;
            lstGroupIds = newLstBackGroundOrderData.DistinctBy(x => x.BkgSvcAttributeGroupId).Select(x => x.BkgSvcAttributeGroupId).ToList();
            for (Int32 grpId = 0; grpId < lstGroupIds.Count; grpId++)
            {
                Int32 instanceCount = newLstBackGroundOrderData.Where(x => x.BkgSvcAttributeGroupId == lstGroupIds[grpId] && x.CustomFormId == CustomFormId).Count();
                if (finalHiddenValue.Equals(String.Empty))
                    finalHiddenValue = lstGroupIds[grpId].ToString() + "_" + instanceCount.ToString();
                else
                    finalHiddenValue = finalHiddenValue + ":" + lstGroupIds[grpId].ToString() + "_" + instanceCount.ToString();
            }
            hdnGroupidandIntanceNumber.Value = finalHiddenValue;
        }

        private void LoadElectronicDrugScreeningForm(CustomFormDataContract grp)
        {
            DrugScreenDataControl _drugScreeningForm = Page.LoadControl("~/BkgOperations/UserControl/DrugScreenDataControl.ascx") as DrugScreenDataControl;
            _drugScreeningForm.AttributeGroupId = grp.groupId;
            _drugScreeningForm.CustomFormId = CurrentFormId;
            _drugScreeningForm.LstAttributeForCustomFormContract = grp.lstCustomFormAttributes;
            //UAT-2842:
            _drugScreeningForm.IsAdminOrderScreen = IsAdminOrderScreen;
            pnlLoader.Controls.Add(_drugScreeningForm);
            if (String.IsNullOrEmpty(applicantOrderCart.EDrugScreeningRegistrationId))
            {
                fsucCmdBar1.ClearButton.Enabled = false;
                fsucCmdBar1.ExtraButton.Enabled = false;
            }
        }



        /// <summary>
        /// Redirect the user to dashboard, if applicant order cart is empty
        /// </summary>
        /// <param name="applicantOrder"></param>
        private void RedirectInvalidOrder(ApplicantOrderCart applicantOrderCart)
        {
            if (applicantOrderCart.IsNullOrEmpty())
                Response.Redirect(AppConsts.APPLICANT_MAIN_PAGE_NAME);
        }

        /// <summary>
        /// Gets the current Order step number for the Custom form, based on its index in the list + 2 (Initial screens of Pending Order & Applicant Profile)
        /// </summary>
        /// <returns></returns>
        private Int32 GetCurrentOrderStep()
        {
            if (applicantOrderCart.IsLocationServiceTenant)
            {
                if (!applicantOrderCart.FingerPrintData.IsNullOrEmpty() && !applicantOrderCart.FingerPrintData.IsOutOfState && !applicantOrderCart.FingerPrintData.IsEventCode)
                    return lstCustomForm.IndexOf(lstCustomForm.FirstOrDefault(x => x.customFormId == CustomFormId)) + AppConsts.FIVE;
                else
                    return lstCustomForm.IndexOf(lstCustomForm.FirstOrDefault(x => x.customFormId == CustomFormId)) + AppConsts.FOUR;
            }
            else
                /// Get the index of the current custom form and add 3 (Pending Order + Applicant Profile + Current Screen)
                return lstCustomForm.IndexOf(lstCustomForm.FirstOrDefault(x => x.customFormId == CustomFormId)) + AppConsts.THREE;
        }

        /// <summary>
        /// Set the button Text for 'Previous', 'Next' 
        /// </summary>
        private void SetButtonText()
        {
            //fsucCmdBar1.SubmitButtonText = AppConsts.PREVIOUS_BUTTON_TEXT;
            //fsucCmdBar1.ClearButtonText = AppConsts.NEXT_BUTTON_TEXT;
            fsucCmdBar1.SubmitButtonText = Resources.Language.PREVIOUS;
            fsucCmdBar1.ClearButtonText = Resources.Language.NEXT;

            btnCancelOrder.Text = Resources.Language.CNCL;
        }

        //UAT-2216:Remove "End Date" from current employer (not previous employers) on Employment Verification.
        private void SetValueOfEmployerFieldInHiddenField(String value)
        {
            String[] currentEmployerDecisionValues = hdnCurrentEmployerDecisionField.Value.Split(',');
            if (!currentEmployerDecisionValues.Contains(value))
            {
                hdnCurrentEmployerDecisionField.Value = hdnCurrentEmployerDecisionField.Value.IsNullOrEmpty() ? value : hdnCurrentEmployerDecisionField.Value + "," + value;
            }
        }

        private void SetDataForEmployerField()
        {
            //UAT-2216: Remove "End Date" from current employer (not previous employers) on Employment Verification.
            //Attribute code for "Employment End Date" : "41BFF8A4-EC01-42C9-B6A6-778BAC34D488" that is fixed for that attribute.
            String attCodeEmployment = ("7A0F4CC0-5416-48D1-AC9F-62A98F0D8606").ToLower();
            String attCodeIsCurrentEmployer = ("41BFF8A4-EC01-42C9-B6A6-778BAC34D488").ToLower();

            AttributesForCustomFormContract employmentAttribute = lstCustomFormAttributes.Where(x => x.AttributeCode.ToLower() == attCodeEmployment).FirstOrDefault();
            AttributesForCustomFormContract isCurrentEmployerAttribute = lstCustomFormAttributes.Where(cnd => cnd.AttributeCode.ToLower() == attCodeIsCurrentEmployer).FirstOrDefault();

            if (!isCurrentEmployerAttribute.IsNullOrEmpty() && !employmentAttribute.IsNullOrEmpty())
            {
                List<BackgroundOrderData> lstOrderDataForGroup = lstBkgGrdOrderForCurrentForm.Where(x => x.BkgSvcAttributeGroupId == isCurrentEmployerAttribute.AttributeGroupId).DistinctBy(dst => dst.InstanceId).ToList();

                foreach (BackgroundOrderData backgroundOrderData in lstOrderDataForGroup)
                {
                    if (backgroundOrderData.CustomFormData.ContainsKey(isCurrentEmployerAttribute.AtrributeGroupMappingId) && (backgroundOrderData.CustomFormData[isCurrentEmployerAttribute.AtrributeGroupMappingId].Equals("True") || backgroundOrderData.CustomFormData[isCurrentEmployerAttribute.AtrributeGroupMappingId].Equals("Yes")))
                    {
                        SetValueOfEmployerFieldInHiddenField(backgroundOrderData.InstanceId + "_" + employmentAttribute.AttributeGroupId + "_" + employmentAttribute.AtrributeGroupMappingId + "_" + employmentAttribute.AttributeId);
                    }
                }

            }
        }
        #endregion

        //UAT-2855
        protected void fsucCmdBar1_ExtraClick(object sender, EventArgs e)
        {
            OrderCartCustomFormId = AppConsts.NONE;
            SaveDataOfForm();
            //Response.Redirect(String.Format("~/BkgOperations/Pages/CustomFormPage.aspx?SelectedTenantId=" + Convert.ToString(TenantId) + "&IsNewCustomForm=false&IsAdminEditMode=true"));
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefeshPage()", true);
        }

        #endregion
    }
}