using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgServiceItemCustomForm : BaseUserControl, IBkgServiceItemCustomFormView
    {

        #region Variables

        #region Private Variables

        private SessionForSupplementServiceCustomForm supplementServiceCustomForm = new SessionForSupplementServiceCustomForm();
        BkgServiceItemCustomFormPresenter _presenter = new BkgServiceItemCustomFormPresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public BkgServiceItemCustomFormPresenter Presenter
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

        /// <summary>
        /// OrderPackageServiceGroupId i.e. PK of ams.BkgorderPackageSvcGroup table,
        /// Helps to fetch the Supplement services for the selected Service Group
        /// </summary>
        public Int32 OrderPkgSvcGroupId
        {
            get
            {
                return ViewState[AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID].IsNotNull() ? Convert.ToInt32(ViewState[AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID]) : AppConsts.NONE;
            }
            set
            {
                ViewState[AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID] = value;
            }
        }

        /// <summary>
        ///Screen Name
        /// </summary>
        private String ParentScreen
        {
            get
            {
                return ViewState["ParentScreen"].IsNotNull() ? Convert.ToString(ViewState["ParentScreen"]) : String.Empty;
            }
            set
            {
                ViewState["ParentScreen"] = value;
            }
        }


        public List<SupplementServicesInformation> lstSupplementServiceList { get; set; }
        public List<SupplementServiceItemInformation> lstSupplementServiceItemList { get; set; }

        public Int32 MasterOrderID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID);
                return 0;
            }
        }

        public String MasterOrderNumber
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_NUMBER).IsNotNull())
                    return (String)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_NUMBER);
                return String.Empty;
            }
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID);
                return 0;
            }
        }

        public IBkgServiceItemCustomFormView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Get or Set Selected Applicant mobility Status ids.
        /// </summary>
        //public List<Int32> SelectedServiceItem
        //{
        //    get
        //    {
        //        List<Int32> selectedIds = new List<Int32>();
        //        for (Int32 i = 0; i < cmbServiceItem.Items.Count; i++)
        //        {
        //            if (cmbServiceItem.Items[i].Checked)
        //            {
        //                selectedIds.Add(Convert.ToInt32(cmbServiceItem.Items[i].Value));
        //            }
        //        }
        //        return selectedIds;
        //    }
        //    set
        //    {
        //        for (Int32 i = 0; i < cmbServiceItem.Items.Count; i++)
        //        {
        //            cmbServiceItem.Items[i].Checked = value.Contains(Convert.ToInt32(cmbServiceItem.Items[i].Value));
        //        }
        //    }

        //}

        //public List<Int32> SetServiceItemToSession
        //{
        //    set
        //    {
        //        supplementServiceCustomForm = (SessionForSupplementServiceCustomForm)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_SERVICE_CUSTOM_FORM);
        //        supplementServiceCustomForm = new SessionForSupplementServiceCustomForm();
        //        supplementServiceCustomForm.lstServiceItemId = value;
        //        SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_SERVICE_CUSTOM_FORM, supplementServiceCustomForm);
        //    }
        //}
        #endregion

        #region Private Properties

        List<Int32> IBkgServiceItemCustomFormView.SelectedSupplementServices
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < cmbServices.Items.Count; i++)
                {
                    if (cmbServices.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(cmbServices.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < cmbServices.Items.Count; i++)
                {
                    cmbServices.Items[i].Checked = value.Contains(Convert.ToInt32(cmbServices.Items[i].Value));
                }
            }
        }

        List<Int32> IBkgServiceItemCustomFormView.SetSupplementServicesToSession
        {
            set
            {
                supplementServiceCustomForm = (SessionForSupplementServiceCustomForm)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_SERVICE_CUSTOM_FORM);
                supplementServiceCustomForm = new SessionForSupplementServiceCustomForm();
                supplementServiceCustomForm.LstSupplementServiceId = value;
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_SERVICE_CUSTOM_FORM, supplementServiceCustomForm);
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetQueryString();
                BindServices();
                //BindSupplementServiceResults();

                ucApplicantData.MasterOrderId = this.MasterOrderID;
                ucApplicantData.TenantId = this.SelectedTenantID;
            }
            fsucCmdBar1.CancelButton.ValidationGroup = "grpServc";
            fsucCmdBar1.SaveButton.CausesValidation = false;
            base.SetPageTitle("Supplement Order");
            base.Title = "Supplement Order";

            ucApplicantDetails.MasterOrderId = this.MasterOrderID;
            ucApplicantDetails.TenantId = this.SelectedTenantID;
            ucApplicantDetails.MasterOrderNumber = this.MasterOrderNumber;
        }
        #endregion

        #region Drop down events

        //protected void cmbServices_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    Int32 serviceId = Convert.ToInt32(cmbServices.SelectedValue);
        //    Presenter.GetSupplementServiceItem(serviceId);
        //    cmbServiceItem.DataSource = lstSupplementServiceItemList;
        //    cmbServiceItem.DataTextField = "ServiceItemName";
        //    cmbServiceItem.DataValueField = "ServiceItemId";
        //    cmbServiceItem.DataBind();
        //}

        #endregion

        #region Button Events

        protected void CmdBarSave_Click(object sender, EventArgs e)
        {
            //SetServiceItemToSession = SelectedServiceItem;
            CurrentViewContext.SetSupplementServicesToSession = CurrentViewContext.SelectedSupplementServices;

            //UAT-2062:System to determine and add additional searches in supplement (SSN Trace)
            supplementServiceCustomForm = (SessionForSupplementServiceCustomForm)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_SERVICE_CUSTOM_FORM);
            //supplementServiceCustomForm.LstSupplementAdditionalSearchDataForName = ucApplicantData.lstMatchedNameForAdditionalSearch;
            //supplementServiceCustomForm.LstSupplementAdditionalSearchDataForLocation = ucApplicantData.lstMatchedLocationForAdditionalSearch;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_SERVICE_CUSTOM_FORM, supplementServiceCustomForm);
            Dictionary<String, String> queryString;

            if (this.OrderPkgSvcGroupId > 0) // Handle the case of 'Cancel' clicked by admin
            {
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                   { AppConsts.CHILD,  ChildControls.ServiceItemCustomForm},
                                                                   { AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, Convert.ToString(this.OrderPkgSvcGroupId)},
                                                                   { AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, AppConsts.BKG_ORDER_REVIEW_QUEUE}
                                                                 };
            }
            else
            {

                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { AppConsts.CHILD,  ChildControls.ServiceItemCustomForm}
                                                                 };
            }
            Response.Redirect(String.Format("~/BkgOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()), true);
        }

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            String _viewType = String.Empty;
            Dictionary<String, String> queryString = new Dictionary<String, String>();

            if (this.OrderPkgSvcGroupId > 0) // Handle the case of 'Cancel' clicked by admin
            {
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantID) },                                                      
                                                                    { "OrderId", Convert.ToString(MasterOrderID)},
                                                                    { AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, Convert.ToString(this.OrderPkgSvcGroupId)},
                                                                     { AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, AppConsts.BKG_ORDER_REVIEW_QUEUE}
                                                                 };
            }
            else
            {
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantID) },                                                      
                                                                    { "OrderId", Convert.ToString(MasterOrderID)}
                                                                 };
            }
            Response.Redirect(String.Format("~/BkgOperations/Pages/BkgOrderDetailPage.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString()), true);
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        private void BindServices()
        {
            Presenter.GetSupplementServices();
            cmbServices.DataSource = lstSupplementServiceList;
            cmbServices.DataTextField = "ServiceName";
            //cmbServices.DataValueField = "ServiceId";
            cmbServices.DataValueField = "PackageServiceId";
            cmbServices.DataBind();
            foreach (RadComboBoxItem item in cmbServices.Items)
            {
                item.Checked = true;
            }
        }

        private void BindSupplementServiceResults()
        {
            //SourceServiceDetailForSupplement sourceService = Presenter.CheckSourceServicesForSupplement();
            //if (sourceService.IfSSNServiceExist)
            //{
            //    String resultText = sourceService.SSNServiceResult;
            //    dvSSNPanel.Style.Add("display", "block");
            //    if (String.IsNullOrEmpty(resultText))
            //    {
            //        resultText = "Empty result. Perhaps not sent yet to Clearstar...";
            //    }
            //    int myLastCharPosition = resultText.IndexOf("This product is a locater index");
            //    if (myLastCharPosition > 0)
            //    {
            //        resultText = resultText.Substring(0, myLastCharPosition);
            //    }
            //    string[] mySplitSSNResults = Regex.Split(resultText, @"_+");
            //    List<KeyValuePair<string, string>> ssnDS = new List<KeyValuePair<string, string>>();
            //    int counter = 1;
            //    foreach (string s in mySplitSSNResults)
            //    {
            //        Regex.Replace(s, @"^\s+", "");
            //        Regex.Replace(s, @"\s+$", "");
            //        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(s, counter.ToString());
            //        ssnDS.Add(kvp); counter++;
            //    }
            //    radGridOrderSSNResults.DataSource = ssnDS;
            //    radGridOrderSSNResults.ClientSettings.Scrolling.AllowScroll = true;
            //    radGridOrderSSNResults.ClientSettings.Scrolling.ScrollHeight = 180;
            //    radGridOrderSSNResults.DataBind();
            //    radGridOrderSSNResults.Visible = true;
            //    //ssnDataSource = ssnDS;
            //}
            //if (sourceService.IfNationalCriminalServiceExist)
            //{
            //    dvNtnlCrimnalSrch.Style.Add("display", "block");
            //    String resultText = sourceService.NationalCriminalServiceResult;
            //    if (String.IsNullOrEmpty(resultText))
            //    {
            //        resultText = "Empty result. Perhaps not sent yet to Clearstar...";
            //    }
            //    int myNoDataFoundPosition = resultText.IndexOf("No offenses found for this name");
            //    if (myNoDataFoundPosition > 0)
            //    {
            //        resultText = "No offenses found for this name.";
            //    }
            //    else
            //    {
            //        int myFirstCharPosition = resultText.IndexOf("*************************************************************************************************************");
            //        int myLastCharPosition = resultText.IndexOf("Sources SearchedAL - Alabama Sex Offender");
            //        if (myLastCharPosition > 0)
            //        {
            //            resultText = resultText.Substring(myFirstCharPosition, myLastCharPosition - myFirstCharPosition);
            //            myFirstCharPosition = resultText.IndexOf(" ");
            //            myLastCharPosition = resultText.LastIndexOf("*************************************************************************************************************");
            //            resultText = resultText.Substring(myFirstCharPosition, myLastCharPosition - myFirstCharPosition);
            //            resultText = resultText.Trim();
            //        }
            //    }
            //    resultText = resultText.Replace("*************************************************************************************************************", "_");
            //    string[] mySplitNationwideResults = Regex.Split(resultText, @"_+");
            //    List<KeyValuePair<string, string>> nationwideDS = new List<KeyValuePair<string, string>>();
            //    int counter = 1;
            //    foreach (string s in mySplitNationwideResults)
            //    {
            //        Regex.Replace(s, @"^\s+", "");
            //        Regex.Replace(s, @"\s+$", "");
            //        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(s, counter.ToString());
            //        nationwideDS.Add(kvp); counter++;
            //    }
            //    radGridOrderNationwideCriminalResults.DataSource = nationwideDS;
            //    radGridOrderNationwideCriminalResults.ClientSettings.Scrolling.AllowScroll = true;
            //    radGridOrderNationwideCriminalResults.ClientSettings.Scrolling.ScrollHeight = 180;
            //    radGridOrderNationwideCriminalResults.DataBind();
            //    radGridOrderNationwideCriminalResults.Visible = true;
            //    //nationwideDataSource = nationwideDS;
            //}
        }

        /// <summary>
        /// Gets the Query string data and sets in the properties
        /// </summary>
        private void GetQueryString()
        {
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNotNull())
            {
                Dictionary<String, String> qryString = new Dictionary<String, String>();
                qryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);

                if (qryString.ContainsKey(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID))
                {
                    this.OrderPkgSvcGroupId = Convert.ToInt32(qryString[AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID]);
                }
                if (qryString.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME))
                {
                    this.ParentScreen = Convert.ToString(qryString[AppConsts.QUERYSTRING_PARENT_SCREEN_NAME]);
                }
            }
        }

        #endregion

        #endregion

    }
}