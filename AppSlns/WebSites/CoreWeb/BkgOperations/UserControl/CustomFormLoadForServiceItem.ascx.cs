using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using System.Xml;
using System.Xml.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.Configuration;

namespace CoreWeb.BkgOperations.Views
{
    public partial class CustomFormLoadForServiceItem : BaseUserControl, ICustomFormLoadForServiceItemView
    {
        #region Variables

        #region Private Variables

        //New Change 21072016
        SupplementOrderCart supplementOrderCart = null;

        /// <summary>
        /// This Property used to denied set of "Supplement order cart" in session on page unload event 
        /// when user click on cancel supplement button (in that case user redirect back to the previous screen so no need to set session on Page_Unload) 
        /// </summary>
        private Boolean _isRequiredToSetDataInSession = true;

        private Int32 _tenantId;
        private CustomFormLoadForServiceItemPresenter _presenter = new CustomFormLoadForServiceItemPresenter();
        private Int32 _customFormId;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public CustomFormLoadForServiceItemPresenter Presenter
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

        public Boolean IsEdit { get; set; }

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

        public List<SupplementServiceItemCustomForm> lstSupplementServiceCustomForm { get; set; }

        public Int32 CurrentServiceID
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

        public Int32 NextCustomForm { get; set; }

        public Int32 NextServiceId { get; set; }

        /// <summary>
        /// OrderPackageServiceGroupId i.e. PK of ams.BkgorderPackageSvcGroup table,
        /// Helps to fetch the Supplement services for the selected Service Group
        /// </summary>
        private Int32 OrderPkgSvcGroupId
        {
            get
            {
                return ViewState["OPSGId"].IsNotNull() ? Convert.ToInt32(ViewState["OPSGId"]) : AppConsts.NONE;
            }
            set
            {
                ViewState["OPSGId"] = value;
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

        public List<SupplementServiceItemCustomForm> lstSupplementServiceCustomFormList
        {
            //New Change 21072016
            get
            {
                List<SupplementServiceItemCustomForm> lstSupplementServiceCustomForm = new List<SupplementServiceItemCustomForm>();
                if (!ViewState["lstSupplementServiceCustomForm"].IsNullOrEmpty())
                {
                    lstSupplementServiceCustomForm = (List<SupplementServiceItemCustomForm>)ViewState["lstSupplementServiceCustomForm"];
                }
                return lstSupplementServiceCustomForm;
            }
            set
            {
                ViewState["lstSupplementServiceCustomForm"] = value;
            }
        }

        List<Int32> ICustomFormLoadForServiceItemView.LstDistinctCustomFormId
        {
            get
            {   //New Change 21072016
                List<Int32> lstDistinctCustomFormId = new List<Int32>();
                if (!ViewState["LstDistinctCustomFormId"].IsNullOrEmpty())
                {
                    lstDistinctCustomFormId = (List<Int32>)ViewState["LstDistinctCustomFormId"];
                }
                return lstDistinctCustomFormId;
            }
            set
            {
                //New Change 21072016
                ViewState["LstDistinctCustomFormId"] = value;
            }
        }

        //public List<Int32> SelectedServiceItem
        //{
        //    get
        //    {
        //        supplementServiceCustomForm = (SessionForSupplementServiceCustomForm)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_SERVICE_CUSTOM_FORM);
        //        if (supplementServiceCustomForm.lstServiceItemId.IsNullOrEmpty())
        //        {
        //            return new List<Int32>();
        //        }
        //        return supplementServiceCustomForm.lstServiceItemId;
        //    }
        //}

        //UAT-2116: Move "Select Services" to the next page and remove its current screen
        //List<Int32> ICustomFormLoadForServiceItemView.SelectedSupplementServices
        //{
        //    get
        //    {
        //        supplementServiceCustomForm = (SessionForSupplementServiceCustomForm)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_SERVICE_CUSTOM_FORM);
        //        if (supplementServiceCustomForm.LstSupplementServiceId.IsNullOrEmpty())
        //        {
        //            return new List<Int32>();
        //        }
        //        return supplementServiceCustomForm.LstSupplementServiceId;
        //    }
        //}

        public ICustomFormLoadForServiceItemView CurrentViewContext
        {
            get { return this; }
        }

        public List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }

        /// <summary>
        /// Get or Set Selected Applicant mobility Status ids.
        /// </summary>

        public Int32 tenantId { get; set; }
        public Int32 OrderId { get; set; }
        public List<SupplementServicesInformation> lstSupplementServiceList { get; set; }
        public List<SupplementServiceItemInformation> lstSupplementServiceItemList { get; set; }
        #region UAT-586 WB: AMS: When adding supplemental services to an order, need to be able to see what the applicant has already entered (location, alias, etc.)
        public List<AttributesForCustomFormContract> lstPreExitingSupplementAttributes
        {
            get;
            set;
        }
        #endregion


        //UAT-2116: Move "Select Services" to the next page and remove its current screen
        List<Int32> ICustomFormLoadForServiceItemView.SelectedSupplementServices
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                if (!cmbServices.CheckedItems.IsNullOrEmpty())
                {
                    selectedIds = cmbServices.CheckedItems.Select(slct => Int32.Parse(slct.Value)).ToList();
                }
                //New Change 21072016
                hdnPreviousSelectedServices.Value = String.Join(",", selectedIds);
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
        //New Change 21072016
        //public List<Int32> SetSupplementServicesToSession
        //{

        //    set
        //    {
        //        //supplementServiceCustomForm = (SessionForSupplementServiceCustomForm)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_SERVICE_CUSTOM_FORM);
        //        //if (supplementServiceCustomForm.IsNull())
        //        //    supplementServiceCustomForm = new SessionForSupplementServiceCustomForm();
        //        //supplementServiceCustomForm.LstSupplementServiceId = value;
        //        //SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_SERVICE_CUSTOM_FORM, supplementServiceCustomForm);
        //        //UAT-2063:Combine the screens to add new Alias and add new locations
        //        hdnPreviousSelectedServices.Value = String.Join(",", value);
        //    }
        //}

        public List<SupplementAdditionalSearchContract> lstMatchedNameForAdditionalSearch
        {
            get;

            set;

        }

        public List<SupplementAdditionalSearchContract> lstMatchedLocationForAdditionalSearch
        {
            get;

            set;

        }

        //UAT-2249
        Int32 ICustomFormLoadForServiceItemView.OrderPackageSvcGroupID
        {
            get;
            set;
        }
        Boolean ICustomFormLoadForServiceItemView.IsOtherServiceGroupsAreCompleted { get; set; }
        Boolean ICustomFormLoadForServiceItemView.IsSuccessIndicatorApplicable { get; set; }
        Boolean ICustomFormLoadForServiceItemView.IsAllExistingSearchesAreClear { get; set; }
        #endregion

        #region Private Properties

        private SupplementOrderCart GetSupplementOrderCart()
        {
            if (supplementOrderCart.IsNullOrEmpty())
            {
                supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
            }
            return supplementOrderCart;
        }

        private String SetServiceItemHeading
        {
            set
            {
                lblServiceItemHeading.Text = value;
            }
        }

        //UAT-2249
        private String ResultMessage { get; set; }
        private String ResultMessageType { get; set; }
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.SetPageTitle("Supplement Order");

                SetTheQueryParameter();
                //UAT-2116: Move "Select Services" to the next page and remove its current screen
                fsucCmdBar1.CancelButton.ValidationGroup = "grpServc";
                if (!IsPostBack)
                {
                    BindServices();
                }

                ucApplicantDetails.MasterOrderId = this.MasterOrderID;
                ucApplicantDetails.TenantId = this.SelectedTenantID;
                ucApplicantDetails.MasterOrderNumber = this.MasterOrderNumber;
                //UAT-2114: Dont show additional searches if line items will not be created.
                ucApplicantData.ParentScreenName = AppConsts.CUSTOM_FORM_FOR_SERVICE_ITEM_SUPPLEMENT;

                if (!IsPostBack)
                {
                    ucApplicantData.MasterOrderId = this.MasterOrderID;
                    ucApplicantData.TenantId = this.SelectedTenantID;
                    //UAT-2062:
                    ucApplicantData.IsShowUnIdentifiedSSNResultMessage = true;
                    SetSupplOrderDataInSessionToFilterLocationSearch();
                    //UAT-2063:Combine the screens to add new Alias and add new locations
                    SetFilteredAdditionalSearchDataInSession();
                }

                //UAT-2063:Combine the screens to add new Alias and add new locations
                if (!CurrentViewContext.SelectedSupplementServices.IsNullOrEmpty())
                {
                    //LoadCustomForm();
                    LoadAllCustomForms();
                }
            }
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

        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                if (_isRequiredToSetDataInSession)
                {
                    supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
                    if (supplementOrderCart.IsNull())
                    {
                        supplementOrderCart = new SupplementOrderCart();
                    }
                    supplementOrderCart.LstSupplementServiceId = CurrentViewContext.SelectedSupplementServices;
                    supplementOrderCart.lstCustomFormLst = lstSupplementServiceCustomFormList;
                    supplementOrderCart.OrdPkgSvcGroupId = this.OrderPkgSvcGroupId;
                    supplementOrderCart.ParentScreen = this.ParentScreen;
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART, supplementOrderCart);
                }
            }
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

        #region Button Events

        protected void CmdBarRestart_Click(object sender, EventArgs e)
        {
            try
            {
                //New Change 21072016
                supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
                this.OrderPkgSvcGroupId = supplementOrderCart.IsNull() ? this.OrderPkgSvcGroupId : supplementOrderCart.OrdPkgSvcGroupId;
                this.ParentScreen = supplementOrderCart.IsNull() ? this.ParentScreen : supplementOrderCart.ParentScreen;
                supplementOrderCart = null;
                _isRequiredToSetDataInSession = false;
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART, supplementOrderCart);
                String _viewType = String.Empty;
                Dictionary<String, String> queryString = new Dictionary<String, String>();

                if (this.OrderPkgSvcGroupId > AppConsts.NONE)
                {
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantID) },                                                      
                                                                    { "OrderId", Convert.ToString(MasterOrderID)},
                                                                    { AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, Convert.ToString(this.OrderPkgSvcGroupId)},
                                                                    { AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, this.ParentScreen}
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

        protected void CmdBarSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveData();
                //ToDo related to UAT-2249
                //LoadNextForm();
                SaveSupplementOrder();
            }
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

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Its sets the query parameters.
        /// </summary>
        private void SetTheQueryParameter()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();

            //Decrypt the TenantId from Query String.
            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }
            if (queryString.ContainsKey("NextCustomForm"))
            {
                NextCustomForm = Convert.ToInt32(queryString["NextCustomForm"]);
            }
            if (queryString.ContainsKey("NextServiceId"))
            {
                NextServiceId = Convert.ToInt32(queryString["NextServiceId"]);
            }

            if (queryString.ContainsKey(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID))
            {
                this.OrderPkgSvcGroupId = Convert.ToInt32(queryString[AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID]);
            }
            if (queryString.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME))
            {
                this.ParentScreen = Convert.ToString(queryString[AppConsts.QUERYSTRING_PARENT_SCREEN_NAME]);
            }
        }

        /// <summary>
        /// load the custom forms for the service items.
        /// </summary>
        private void LoadCustomForm()
        {
            //UAT-2116: Move "Select Services" to the next page and remove its current screen
            //Uncomment below code that was commented regarding UAT-2116, This functionality handled in "LoadAllCustromForm" method for the implementation of UAT-2063
            if (lstSupplementServiceCustomFormList.IsNullOrEmpty())
            {
                //Presenter.GetListOfCustomFormsForSelectedItem();
                Presenter.GetListOfCustomFormsForSelectedServices();
            }
            //Commented This code regarding UAT-2063.
            //Presenter.GetListOfCustomFormsForSelectedServices();

            if (!lstSupplementServiceCustomFormList.IsNullOrEmpty())
            {
                SupplementServiceItemCustomForm serviceItemCustomForm = new SupplementServiceItemCustomForm();
                if (NextCustomForm == 0 && NextServiceId == 0)
                {
                    serviceItemCustomForm = lstSupplementServiceCustomFormList.FirstOrDefault();
                }
                else
                {
                    //serviceItemCustomForm = lstSupplementServiceCustomFormList.FirstOrDefault(x => x.CustomFormID == NextCustomForm && x.ServiceItemID == NextServiceId);
                    serviceItemCustomForm = lstSupplementServiceCustomFormList.FirstOrDefault(x => x.CustomFormID == NextCustomForm);
                }
                if (!serviceItemCustomForm.IsNullOrEmpty())
                {
                    //SetServiceItemHeading = serviceItemCustomForm.ServiceItemName;
                    CurrentFormId = serviceItemCustomForm.CustomFormID;
                    CurrentServiceID = serviceItemCustomForm.ServiceItemID;
                    Presenter.GetListOfAttributesForSelectedItem(CurrentFormId, CurrentServiceID);
                    if (!lstCustomFormAttributes.IsNullOrEmpty() && lstCustomFormAttributes.Count > 0 && lstCustomFormAttributes.Any(x => x.IsDisplay))
                    {
                        serviceItemCustomForm.lstCustomFormAttributes = lstCustomFormAttributes;
                        List<Int32> groupIds = lstCustomFormAttributes.DistinctBy(x => x.AttributeGroupId).OrderBy(x => x.Sequence).Select(x => x.AttributeGroupId).ToList();
                        lstSupplementServiceCustomForm = new List<SupplementServiceItemCustomForm>();
                        for (int i = 0; i < groupIds.Count; i++)
                        {
                            //if it is elecetroni9c drug screening group
                            if (lstCustomFormAttributes.Any(x => x.IsDisplay && x.AttributeGroupId == groupIds[i]))
                            {
                                //UAT-2062
                                Dictionary<String, Boolean> dicAdditionalSearchDataType = SetInstanceIDForAdditionalSearchData(serviceItemCustomForm, groupIds[i]);
                                SupplementServiceItemCustomForm newCustomFormData = GetNewInstanceForTheForm(serviceItemCustomForm, groupIds[i]);

                                //UAT-2062
                                SetAdditionalSearchDataInFormData(newCustomFormData, dicAdditionalSearchDataType);
                                lstSupplementServiceCustomForm.Add(newCustomFormData);
                            }
                            //SetPreExistingSupplementData(groupIds[i], serviceItemCustomForm.ServiceId, serviceItemCustomForm.ServiceItemID);
                        }

                        BindCustomForms(lstSupplementServiceCustomForm);
                    }
                }
            }


        }

        /// <summary>
        /// This Function gets the new instance of the form 
        /// </summary>
        /// <param name="customFormDataContract">Previous instance</param>
        /// <param name="i">Instance Number</param>
        /// <returns>New instance</returns>
        private SupplementServiceItemCustomForm GetNewInstanceForTheForm(SupplementServiceItemCustomForm supplementDataContract, int groupId)
        {
            SupplementServiceItemCustomForm newCustomFormData = new SupplementServiceItemCustomForm();
            newCustomFormData.lstCustomFormAttributes = supplementDataContract.lstCustomFormAttributes;
            newCustomFormData.CustomFormID = supplementDataContract.CustomFormID;
            newCustomFormData.ServiceItemID = supplementDataContract.ServiceItemID;
            newCustomFormData.ServiceId = supplementDataContract.ServiceId;
            newCustomFormData.PackageServiceId = supplementDataContract.PackageServiceId;
            newCustomFormData.groupId = groupId;
            newCustomFormData.instanceId = GetInstanceIdForTheGroup(groupId);
            return newCustomFormData;
        }

        /// <summary>
        /// It loads the user control depending upon the 
        /// number of instance to be added
        /// </summary>
        /// <param name="lstDataSourceRepeater"></param>
        private void BindCustomForms(List<SupplementServiceItemCustomForm> lstDataSourceRepeater)
        {
            //Check for Electronic Drug Screening
            {
                foreach (var grp in lstDataSourceRepeater)
                {
                    AddGroups(grp);
                }
            }
            //Check for electronic drug screening
            //load it
        }

        /// <summary>
        /// It loads the user control depending upon the 
        /// number of instance to be added
        /// </summary>
        /// <param name="grp"></param>
        private void AddGroups(SupplementServiceItemCustomForm grp)
        {
            CustomFormHtlm _customForm = Page.LoadControl("~/BkgOperations/UserControl/CustomFormHtlm.ascx") as CustomFormHtlm;
            _customForm.lstCustomFormAttributes = grp.lstCustomFormAttributes;
            _customForm.groupId = grp.groupId;
            _customForm.IsSupplementalOrder = true;
            _customForm.InstanceId = grp.instanceId;
            _customForm.ServiceID = grp.ServiceId;
            _customForm.PackageServiceId = grp.PackageServiceId;
            _customForm.CustomFormId = CurrentFormId;
            _customForm.tenantId = SelectedTenantID;
            _customForm.IsEdit = IsEdit;
            _customForm.lstHiddenInstanceOfGroup = GetListOfHiddenInstance(grp.groupId);
            _customForm.ServiceItemID = grp.ServiceItemID;
            _customForm.ShowPreExistingSupplementData = true;
            //_customForm.lstBackgroundOrderData = lstBkgGrdOrderForCurrentForm;
            //UAT-2062:
            _customForm.LstSupplementAdditionalSearchDataForName = grp.LstSupplementAdditionalSearchDataForName;
            _customForm.LstSupplementAdditionalSearchDataForLocation = grp.LstSupplementAdditionalSearchDataForLocation;
            //UAT-2063:Combine the screens to add new Alias and add new locations
            //Set the current form Attribute group id to identify the form "AddNew" button is clicked.
            _customForm.AutoFocusGroupID = hdnAutofocusGroupID.Value.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(hdnAutofocusGroupID.Value);
            pnlLoader.Controls.Add(_customForm);
        }

        /// <summary>
        /// It loads the next custom form in queue
        /// </summary>
        private void LoadNextForm()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            SupplementServiceItemCustomForm data = null;
            Int32 nextCustomFormId = 0;
            String redirectUrl = String.Empty;
            Int32 index = 0;
            Int32 customFormIndex = 0;
            if (!CurrentViewContext.LstDistinctCustomFormId.IsNullOrEmpty())
            {
                customFormIndex = CurrentViewContext.LstDistinctCustomFormId.FindIndex(x => x == CurrentFormId);
                if (customFormIndex < CurrentViewContext.LstDistinctCustomFormId.Count - 1)
                {
                    data = new SupplementServiceItemCustomForm();
                    data = lstSupplementServiceCustomFormList.Where(cond => cond.CustomFormID == CurrentViewContext.LstDistinctCustomFormId[customFormIndex + 1]).FirstOrDefault();

                }
            }
            //if (!lstSupplementServiceCustomFormList.IsNullOrEmpty())
            //{
            //    index = lstSupplementServiceCustomFormList.FindIndex(x => x.CustomFormID == CurrentFormId && x.ServiceItemID == CurrentServiceID);
            //    if (index < lstSupplementServiceCustomFormList.Count - 1)
            //    {
            //        data = new SupplementServiceItemCustomForm();
            //        data = lstSupplementServiceCustomFormList[index + 1];
            //    }
            //}

            if (!data.IsNullOrEmpty())
            {
                nextCustomFormId = data.CustomFormID;
                //UAT-2063:Combine the screens to add new Alias and add new locations
                NextCustomForm = nextCustomFormId;
                NextServiceId = data.ServiceItemID;
                //ToDo: For Comment UAT-2063
                //queryString = new Dictionary<String, String>
                //                                                 { 
                //                                                    { AppConsts.CHILD, ChildControls.ServiceItemCustomForm},
                //                                                    { "NextCustomForm", Convert.ToString(nextCustomFormId)},
                //                                                    {"NextServiceId",Convert.ToString(data.ServiceItemID)},
                //                                                    { "CustomFormId", Convert.ToString(nextCustomFormId)},
                //                                                    {"IsEdit",IsEdit.ToString()}
                //                                                 };
                //redirectUrl = "~/BkgOperations/Default.aspx?args={0}";

            }
            else
            {
                //ToDo related to UAT-2249
                //queryString = new Dictionary<String, String> 
                //                                                { 
                //                                                    //{ "Child", ChildControls.ApplicantProfile}
                //                                                       { AppConsts.CHILD, @"~/BkgOperations/UserControl/OrderDetailForServiceItem.ascx" } 
                //                                                };
                //redirectUrl = "~/BkgOperations/Default.aspx?args={0}";
                //UAT-2063:Combine the screens to add new Alias and add new locations
                //Response.Redirect(String.Format(redirectUrl, queryString.ToEncryptedQueryString()));
            }
            //ToDo:For CommentUAT-2063
            //Response.Redirect(String.Format(redirectUrl, queryString.ToEncryptedQueryString()));
        }

        private List<Int32> GetListOfHiddenInstance(Int32 groupId)
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

        private void SaveData()
        {
            supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
            if (supplementOrderCart.IsNullOrEmpty())
            {
                supplementOrderCart = new SupplementOrderCart();
            }
            Int32 controlCount = pnlLoader.Controls.Count;
            if (controlCount > 0)
            {
                supplementOrderCart.MasterOrderId = MasterOrderID;
                for (Int32 i = 0; i < controlCount; i++)
                {
                    if (pnlLoader.Controls[i] is CustomFormHtlm)
                    {
                        CustomFormHtlm customFormHtlm = pnlLoader.Controls[i] as CustomFormHtlm;
                        List<SupplementOrderData> lstSupplementOrderData = customFormHtlm.GetDataForServiceItem();// drugScreenDataCntrl.GetEDrugData();
                        //foreach (SupplementOrderData supplementOrderData in lstSupplementOrderData)
                        //{
                        //    //check for each form data in lstSupplementOrderData => if count of value >= 2 and any value is empty then return false and don't load next form
                        //    if (!supplementOrderData.FormData.IsNullOrEmpty() && supplementOrderData.FormData.Count == AppConsts.TWO)
                        //    {
                        //        if (supplementOrderData.FormData.Any(data => data.Value != null && data.Value != "") && supplementOrderData.FormData.Any(data=>data.Value == null || data.Value == ""))
                        //        {
                        //            return false;
                        //        }
                        //    }
                        //}
                        if (supplementOrderCart.lstSupplementOrderData.IsNullOrEmpty())
                        {
                            supplementOrderCart.lstSupplementOrderData = new List<SupplementOrderData>();
                        }
                        supplementOrderCart.lstSupplementOrderData.AddRange(lstSupplementOrderData);
                    }
                }
            }
            // Already have the values in Session, when the Second custom forms is loaded. So do not update from variable 
            // As query string will be NULL in that case.  
            supplementOrderCart.OrdPkgSvcGroupId = this.OrderPkgSvcGroupId != AppConsts.NONE ? this.OrderPkgSvcGroupId : supplementOrderCart.OrdPkgSvcGroupId;
            supplementOrderCart.ParentScreen = !this.ParentScreen.IsNullOrEmpty() ? this.ParentScreen : supplementOrderCart.ParentScreen;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART, supplementOrderCart);
        }

        #region UAT-2114: Dont show additional searches if line items will not be created.

        /// <summary>
        /// Bind Services
        /// </summary>
        private void BindServices()
        {
            Presenter.GetSupplementServices();
            cmbServices.DataSource = lstSupplementServiceList;
            cmbServices.DataTextField = "ServiceName";
            cmbServices.DataValueField = "PackageServiceId";
            cmbServices.DataBind();
            List<Int32> lstPreviouslySelectedServices = new List<Int32>();
            //New Change 21072016
            GetSupplementOrderCart();
            if (!supplementOrderCart.IsNullOrEmpty())
            {
                lstPreviouslySelectedServices = supplementOrderCart.LstSupplementServiceId;
            }
            foreach (RadComboBoxItem item in cmbServices.Items)
            {
                if (!lstPreviouslySelectedServices.IsNullOrEmpty())
                {
                    if (lstPreviouslySelectedServices.Contains(Convert.ToInt32(item.Value)))
                    {
                        item.Checked = true;
                    }
                }
                else
                {
                    item.Checked = true;
                }
            }
            //CurrentViewContext.SetSupplementServicesToSession = CurrentViewContext.SelectedSupplementServices;
        }

        /// <summary>
        /// Save Data in Session
        /// </summary>
        private void SetSupplOrderDataInSessionToFilterLocationSearch()
        {
            ucApplicantData.BindOtherServiceResults();
            SupplementOrderCart supplementOrderCartTemp = new SupplementOrderCart();
           
            List<SupplementOrderData> lstSupplementOrderData = GetSupplementOrderDataToFilterLocationSearch();
            if (supplementOrderCartTemp.lstSupplementOrderData.IsNullOrEmpty())
            {
                supplementOrderCartTemp.lstSupplementOrderData = new List<SupplementOrderData>();
            }
            supplementOrderCartTemp.lstSupplementOrderData.AddRange(lstSupplementOrderData);

            // Already have the values in Session, when the Second custom forms is loaded. So do not update from variable 
            // As query string will be NULL in that case.  
            supplementOrderCartTemp.OrdPkgSvcGroupId = this.OrderPkgSvcGroupId;
            supplementOrderCartTemp.ParentScreen =  this.ParentScreen;

            //New Change 21072016
            supplementOrderCartTemp.LstSupplementServiceId = CurrentViewContext.SelectedSupplementServices;
            //New Change 21072016
            ucApplicantData.lstSupplementServiceCustomFormList = lstSupplementServiceCustomFormList;
            ucApplicantData.supplementOrderCartTemp = supplementOrderCartTemp;
        }

        /// <summary>
        /// Load Additional Searches For Custom Form
        /// </summary>
        /// <returns></returns>
        private List<SupplementOrderData> GetSupplementOrderDataToFilterLocationSearch()
        {
            List<SupplementOrderData> lstSupplementOrderData = new List<SupplementOrderData>();
            //UAT-2116: Move "Select Services" to the next page and remove its current screen
            //Uncomment below code that was commented regarding UAT-2116, This functionality handled in "LoadAllCustromForm" method for the implementation of UAT-2063
            if (lstSupplementServiceCustomFormList.IsNullOrEmpty())
            {
                //Presenter.GetListOfCustomFormsForSelectedItem();
                Presenter.GetListOfCustomFormsForSelectedServices();
            }
            if (!lstSupplementServiceCustomFormList.IsNullOrEmpty())
            {
                foreach (var serviceItemCustomForm in lstSupplementServiceCustomFormList)
                {
                    CurrentFormId = serviceItemCustomForm.CustomFormID;
                    CurrentServiceID = serviceItemCustomForm.ServiceItemID;
                    Presenter.GetListOfAttributesForSelectedItem(CurrentFormId, CurrentServiceID);
                    if (!lstCustomFormAttributes.IsNullOrEmpty() && lstCustomFormAttributes.Count > 0 && lstCustomFormAttributes.Any(x => x.IsDisplay))
                    {
                        serviceItemCustomForm.lstCustomFormAttributes = lstCustomFormAttributes;
                        List<Int32> groupIds = lstCustomFormAttributes.DistinctBy(x => x.AttributeGroupId).OrderBy(x => x.Sequence).Select(x => x.AttributeGroupId).ToList();
                        lstSupplementServiceCustomForm = new List<SupplementServiceItemCustomForm>();
                        for (int i = 0; i < groupIds.Count; i++)
                        {
                            //if it is location atrribute group then get supplement order form data to filter Service Item Additional Search
                            if (lstCustomFormAttributes.Any(x => x.IsDisplay && x.AttributeGroupId == groupIds[i]) && lstCustomFormAttributes.Any(x => x.AtrributeGroupMappingId == ucApplicantData.AttributeGroupMappingIdForState))
                            {
                                lstSupplementOrderData = GetLocationSearchData(ucApplicantData.lstMatchedLocationForAdditionalSearch, ucApplicantData.AttributeGroupMappingIdForState,
                                    ucApplicantData.AttributeGroupMappingIdForCounty, serviceItemCustomForm.ServiceId, serviceItemCustomForm.PackageServiceId, groupIds[i]);
                                return lstSupplementOrderData;
                            }
                        }
                    }
                }
            }
            return lstSupplementOrderData;
        }

        /// <summary>
        /// Get Data For Service Item Additional Search
        /// </summary>
        /// <param name="lstMatchedLocationForAdditionalSearch"></param>
        /// <param name="attributeGroupMappingIdForState"></param>
        /// <param name="attributeGroupMappingIdForCounty"></param>
        /// <param name="serviceID"></param>
        /// <param name="packageServiceId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private List<SupplementOrderData> GetLocationSearchData(List<SupplementAdditionalSearchContract> lstMatchedLocationForAdditionalSearch, Int32 attributeGroupMappingIdForState,
                                                                                Int32 attributeGroupMappingIdForCounty, Int32 serviceID, Int32 packageServiceId, Int32 groupId)
        {
            List<SupplementOrderData> lstResult = new List<SupplementOrderData>();
            if (lstCustomFormAttributes.Any(x => x.AtrributeGroupMappingId == attributeGroupMappingIdForState))
            {
                if (lstMatchedLocationForAdditionalSearch.Count() > 0)
                {
                    Int32 count = 1;
                    foreach (var matchedLocationForAdditionalSearch in lstMatchedLocationForAdditionalSearch)
                    {
                        SupplementOrderData supplementOrderData = new SupplementOrderData();
                        supplementOrderData.InstanceId = count;
                        supplementOrderData.BkgServiceId = serviceID;
                        supplementOrderData.PackageServiceId = packageServiceId;
                        supplementOrderData.CustomFormId = CurrentFormId;
                        supplementOrderData.PackageSvcItemId = CurrentServiceID;
                        supplementOrderData.BkgSvcAttributeGroupId = groupId;
                        supplementOrderData.FormData = new Dictionary<int, string>();
                        SetLocationFormData(matchedLocationForAdditionalSearch, attributeGroupMappingIdForState, attributeGroupMappingIdForCounty, supplementOrderData.FormData);

                        lstResult.Add(supplementOrderData);
                        count++;

                    }
                }
            }
            return lstResult;
        }

        /// <summary>
        /// Create Form Data
        /// </summary>
        /// <param name="matchedLocationForAdditionalSearch"></param>
        /// <param name="attributeGroupMappingIdForState"></param>
        /// <param name="attributeGroupMappingIdForCounty"></param>
        /// <param name="formDataDetails"></param>
        private void SetLocationFormData(SupplementAdditionalSearchContract matchedLocationForAdditionalSearch, Int32 attributeGroupMappingIdForState,
                                    Int32 attributeGroupMappingIdForCounty, Dictionary<Int32, String> formDataDetails)
        {
            if (matchedLocationForAdditionalSearch.IsLocationUsedForSearch)
            {
                if (!matchedLocationForAdditionalSearch.StateName.IsNullOrEmpty())
                    formDataDetails.Add(attributeGroupMappingIdForState, matchedLocationForAdditionalSearch.StateName);
                if (!matchedLocationForAdditionalSearch.CountyName.IsNullOrEmpty())
                    formDataDetails.Add(attributeGroupMappingIdForCounty, matchedLocationForAdditionalSearch.CountyName);
            }
        }

        #endregion

        #endregion


        #region UAT-586 WB: AMS: When adding supplemental services to an order, need to be able to see what the applicant has already entered (location, alias, etc.)
        private void SetPreExistingSupplementData(Int32 groupId, Int32 serviceId, Int32 serviceItemId)
        {
            HtmlGenericControl divPreExistngData = null;//Only for supplement order data.
            divPreExistngData = GenerateFormForSupplementPreExistingData(groupId, AppConsts.THREE, AppConsts.NONE, serviceId, serviceItemId);
            RenderHeaderSectionHtml(groupId, AppConsts.NONE, divPreExistngData);
        }
        /// <summary>
        /// This creates the header section 
        /// for the title of the group
        /// </summary>
        /// <returns>Panell in which the remaining controls are dynamically loaded.</returns>
        private void RenderHeaderSectionHtml(Int32 groupId, Int32 currentInstanceId, HtmlGenericControl divPreExistngData)
        {
            if (divPreExistngData.IsNotNull())
            {
                HtmlGenericControl mainContainer = new HtmlGenericControl("div");
                mainContainer.ID = "mainDiv_" + groupId.ToString() + "_" + currentInstanceId.ToString();
                HtmlGenericControl section = new HtmlGenericControl("div");
                section.Attributes.Add("class", "section");

                HtmlGenericControl mhdrDiv = new HtmlGenericControl("div");
                mhdrDiv.Attributes.Add("class", "mhdr");
                mhdrDiv.Attributes.Add("style", "position: relative; bottom: 2px;");
                //mhdrDiv.Attributes.Add("id", "divCustomFormMhdr" + formId + "");
                HtmlGenericControl content = new HtmlGenericControl("div");
                content.Attributes.Add("class", "content");
                //UAT-586 WB: AMS: When adding supplemental services to an order, need to be able to see what the applicant has already entered (location, alias, etc.)
                HtmlGenericControl headerTagSupplement = new HtmlGenericControl("h1");
                headerTagSupplement.Attributes.Add("style", "font-size: 14px; padding-bottom: 2px;");
                Label headerLableSupplement = new Label();
                headerLableSupplement.ID = "lblHeaderSupplement_" + groupId.ToString() + "_" + currentInstanceId.ToString();
                headerLableSupplement.Text = "Pre Existing Data";
                headerLableSupplement.CssClass = currentInstanceId.ToString();
                headerTagSupplement.Controls.Add(headerLableSupplement);
                mhdrDiv.Controls.Add(headerTagSupplement);

                section.Controls.Add(mhdrDiv);
                HtmlGenericControl ContainerSupplement = new HtmlGenericControl("div");
                ContainerSupplement.Attributes.Add("class", "sxform auto");
                HtmlGenericControl cointainerDiv = new HtmlGenericControl("div");
                cointainerDiv.Attributes.Add("style", "padding-bottom: 10px;");
                //Panel in which all the other controls are loaded
                Panel formPanelSupplementData = new Panel();
                formPanelSupplementData.ID = "pnlSupplementData_" + currentInstanceId.ToString() + "_" + groupId;
                formPanelSupplementData.CssClass = "sxpnl";
                formPanelSupplementData.Controls.Add(divPreExistngData);
                ContainerSupplement.Controls.Add(formPanelSupplementData);
                cointainerDiv.Controls.Add(ContainerSupplement);
                content.Controls.Add(cointainerDiv);
                //section.Controls.Add(contentSupplement);

                section.Controls.Add(content);
                mainContainer.Controls.Add(section);
                pnlExistingData.Controls.Add(mainContainer);
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
        /// Add relevant space between tweo row.
        /// </summary>
        /// <param name="parentControl"></param>
        /// <returns>parentControl</returns>
        private HtmlGenericControl AddNextLineDiv(HtmlGenericControl parentControl)
        {
            String className = "sxroend";
            HtmlGenericControl nextLineDiv = new HtmlGenericControl("div");

            nextLineDiv.Attributes.Add("class", className);
            parentControl.Controls.Add(nextLineDiv);
            return parentControl;
        }

        /// <summary>
        /// This creates the dynamic HTML as per the group id and the number of column in each row
        /// </summary>
        /// <param name="groupId">Group ID</param>
        /// <param name="columnNumber">No of column per row</param>
        /// <param name="mainPanelForFor">Main panel in wwhich the HTML is loaded.</param>
        private HtmlGenericControl GenerateFormForSupplementPreExistingData(Int32 groupId, Int32 columnNumber, Int32 currentInstanceId, Int32 serviceId, Int32 serviceItemId)
        {
            int i = 0;
            Int32 attributeCount = 1;
            HtmlGenericControl controlInColumn = null;
            HtmlGenericControl divPreExistngData = null;
            Presenter.GetAttributeDataListForPreExistingSupplement(groupId, MasterOrderID, serviceItemId, serviceId);
            if (lstPreExitingSupplementAttributes.IsNotNull() && lstPreExitingSupplementAttributes.Count > 0)
            {
                divPreExistngData = new HtmlGenericControl("div");

                foreach (var attributes in lstPreExitingSupplementAttributes)
                {
                    //If the number of control generated = number of column 
                    //per row then add to the form n create new row.
                    if (i == columnNumber)
                    {
                        AddNextLineDiv(controlInColumn);
                        divPreExistngData.Controls.Add(controlInColumn);
                        i = 0;
                    }
                    if (i == 0)
                    {
                        //Generate a new row
                        controlInColumn = GenerateColumnView(columnNumber);
                    }
                    controlInColumn = CreateControlForSupplementPreExistData(attributes, currentInstanceId, controlInColumn, attributeCount);
                    i++;
                    attributeCount++;
                }
                if (i != 0)
                {
                    AddNextLineDiv(controlInColumn);
                    divPreExistngData.Controls.Add(controlInColumn);
                }
            }
            return divPreExistngData;
        }

        /// <summary>
        /// It Create html for the read only mode
        /// </summary>
        /// <param name="attributesForCustomForm"></param>
        /// <param name="currentInstanceId"></param>
        /// <returns></returns>
        private HtmlGenericControl CreateControlForSupplementPreExistData(AttributesForCustomFormContract attributesForCustomForm, Int32 currentInstanceId, HtmlGenericControl parentControl, Int32 attributeCount)
        {
            HtmlGenericControl lableDiv = new HtmlGenericControl("div");
            lableDiv.Attributes.Add("class", "sxlb");
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes.Add("class", "cptn");
            span.InnerHtml = attributesForCustomForm.AttributeName;
            lableDiv.Controls.Add(span);
            HtmlGenericControl controlDiv = new HtmlGenericControl("div");
            controlDiv.Attributes.Add("class", "sxlm");
            Label controlLable = new Label();
            controlLable.ID = "lblSupplement" + attributesForCustomForm.AttributeType + "_" + attributesForCustomForm.AttributeGroupId + "_" + currentInstanceId.ToString() + "_" + attributesForCustomForm.AttributeId + "_" + attributeCount;
            controlLable.CssClass = "ronly";
            if (attributesForCustomForm.AttributeDataValue.IsNullOrEmpty())
                controlLable.Text = String.Empty;
            else
                controlLable.Text = attributesForCustomForm.AttributeDataValue;
            controlDiv.Controls.Add(controlLable);
            parentControl.Controls.Add(lableDiv);
            parentControl.Controls.Add(controlDiv);
            return parentControl;
        }
        #endregion
        #endregion

        #region UAT-2062:
        private Dictionary<String, Boolean> SetInstanceIDForAdditionalSearchData(SupplementServiceItemCustomForm supplementDataContract, Int32 groupId)
        {
            String attGroupMappingCode_AliasName = ("56258C54-C2BC-4514-94E1-2EF2EFFFDBF5").ToLower();
            String attGroupMappingCode_State = ("CAEAC9FA-FFF4-4F7A-A644-96967F399362").ToLower();
            Dictionary<String, Boolean> groupTypeData = new Dictionary<String, Boolean>();
            if (!supplementDataContract.IsNullOrEmpty() && !supplementDataContract.lstCustomFormAttributes.IsNullOrEmpty())
            {
                //New Change 21072016
                if (supplementDataContract.lstCustomFormAttributes.Any(x => x.AttributeGroupMappingCode == attGroupMappingCode_AliasName)
                    //New Change 21072016
                    && !lstMatchedNameForAdditionalSearch.IsNullOrEmpty()
                    && !IsGroupIdAlreadyCreated(groupId)
                    )
                {
                    //New Change 21072016
                    hdnGroupidandIntanceNumber.Value = groupId + "_" + lstMatchedNameForAdditionalSearch.Count;
                    groupTypeData.Add("IsNameSearchGroup", true);
                }
                if (supplementDataContract.lstCustomFormAttributes.Any(x => x.AttributeGroupMappingCode == attGroupMappingCode_State)
                    //New Change 21072016
                    && !lstMatchedLocationForAdditionalSearch.IsNullOrEmpty()
                    && !IsGroupIdAlreadyCreated(groupId)
                    )
                {
                    //New Change 21072016
                    hdnGroupidandIntanceNumber.Value = !hdnGroupidandIntanceNumber.Value.IsNullOrEmpty() ? hdnGroupidandIntanceNumber.Value + ":" + groupId + "_" + lstMatchedLocationForAdditionalSearch.Count
                                                                                                          : groupId + "_" + lstMatchedLocationForAdditionalSearch.Count;
                    groupTypeData.Add("IsLocationSearchGroup", true);
                }
            }
            return groupTypeData;
        }


        private void SetAdditionalSearchDataInFormData(SupplementServiceItemCustomForm supplementDataContract, Dictionary<String, Boolean> dicAdditionalSearchDataTypes)
        {
            if (!supplementDataContract.IsNullOrEmpty() && !dicAdditionalSearchDataTypes.IsNullOrEmpty())
            {
                //New Change 21072016
                if (dicAdditionalSearchDataTypes.Keys.Contains("IsNameSearchGroup") && dicAdditionalSearchDataTypes["IsNameSearchGroup"])
                {
                    //New Change 21072016
                    supplementDataContract.LstSupplementAdditionalSearchDataForName = lstMatchedNameForAdditionalSearch;
                }

                if (dicAdditionalSearchDataTypes.Keys.Contains("IsLocationSearchGroup") && dicAdditionalSearchDataTypes["IsLocationSearchGroup"])
                {
                    //New Change 21072016
                    supplementDataContract.LstSupplementAdditionalSearchDataForLocation = lstMatchedLocationForAdditionalSearch;
                }
            }
        }

        /// <summary>
        /// This function return is group controls is created or not for the particular group.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private Boolean IsGroupIdAlreadyCreated(Int32 groupId)
        {
            Boolean isGroupIdAlreadyCreated = false;
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
                            isGroupIdAlreadyCreated = true;
                        }
                    }
                }
            }

            return isGroupIdAlreadyCreated;
        }
        #endregion

        #region UAT-2063:Combine the screens to add new Alias and add new locations
        /// <summary>
        /// Method to load "Alias" and "Location" custom form on same screen.
        /// </summary>
        private void LoadAllCustomForms()
        {
            Int32 totalCustomForms = AppConsts.NONE;
            if (lstSupplementServiceCustomFormList.IsNullOrEmpty())
            {
                Presenter.GetListOfCustomFormsForSelectedServices();
                pnlLoader.Controls.Clear();
            }
            if (!lstSupplementServiceCustomFormList.IsNullOrEmpty())
            {
                List<Int32> DistinctForm = lstSupplementServiceCustomFormList.DistinctBy(x => x.CustomFormID).Select(slct => slct.CustomFormID).ToList();
                totalCustomForms = DistinctForm.Count();
            }
            for (int i = AppConsts.NONE; i < totalCustomForms; i++)
            {
                if (i == AppConsts.NONE)
                {
                    LoadCustomForm();
                }
                else
                {
                    LoadNextForm();
                    LoadCustomForm();
                }
            }
        }
        /// <summary>
        /// Method to set automatic fill additional search data location and alias in session for further used in custom form.
        /// </summary>
        private void SetFilteredAdditionalSearchDataInSession()
        {
            //To get the auto fields values for alias search data and location details.
            //ucApplicantData.BindOtherServiceResults(true);
            ucApplicantData.GetFilteredLocationSearches();
            //New Change 21072016
            lstMatchedNameForAdditionalSearch = ucApplicantData.lstMatchedNameForAdditionalSearch;
            lstMatchedLocationForAdditionalSearch = ucApplicantData.lstMatchedLocationForAdditionalSearch;
        }

        private void ReloadPage()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { AppConsts.CHILD, ChildControls.ServiceItemCustomForm},
                                                                    { AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, Convert.ToString(this.OrderPkgSvcGroupId)},
                                                                     { AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, this.ParentScreen},
                                                                    
                                                                 };
            String redirectUrl = "~/BkgOperations/Default.aspx?args={0}";

            Response.Redirect(String.Format(redirectUrl, queryString.ToEncryptedQueryString()));
        }
        protected void btnReload_Click(object sender, EventArgs e)
        {
            try
            {
                //New Change 21072016
                GetSupplementOrderCart();
                if (!supplementOrderCart.IsNullOrEmpty())
                {
                    supplementOrderCart.lstCustomFormLst = new List<SupplementServiceItemCustomForm>();
                }
                //CurrentViewContext.SetSupplementServicesToSession = CurrentViewContext.SelectedSupplementServices;
                ReloadPage();
            }
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

        #region UAT-2249
        private void SaveSupplementOrder()
        {
            String msgText = String.Empty;
            String msgType = String.Empty;
            try
            {
                supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
                
                //UAT-2200:Should send back to queue if everything in the service group is complete and clear and other service group(s) are in progress
                CurrentViewContext.OrderPackageSvcGroupID = supplementOrderCart.OrdPkgSvcGroupId;

                List<Entity.ClientEntity.BkgAttributeGroupMapping> lstBkgAttributeGroupMapping = Presenter.GetAllBkgAttributeGroupMapping(this.SelectedTenantID);
                Guid stateAliasGuid = new Guid("CAEAC9FA-FFF4-4F7A-A644-96967F399362");
                Guid countryGuid = new Guid("37B6B708-C691-4568-B604-6F70F24BC839");
                Guid countyGuid = new Guid("C00AEFB5-37DF-44F7-A050-D2C9581909DE");
                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                Guid aliasNameGuid = new Guid("56258C54-C2BC-4514-94E1-2EF2EFFFDBF5");
                Int32 attributeGroupMappingIdForState = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == stateAliasGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
                Int32 attributeGroupMappingIdForCountry = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == countryGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
                Int32 attributeGroupMappingIdForCounty = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == countyGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
                Int32 attributeGroupMappingIdForAlias = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == aliasNameGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;

                XmlDocument _doc = new XmlDocument();
                XmlElement _rootNode = (XmlElement)_doc.AppendChild(_doc.CreateElement("BkgOrderAddition"));

                List<Int32> _lstPackageServiceIds = lstSupplementServiceCustomFormList
                                                .DistinctBy(svc => svc.PackageServiceId)
                                                .Select(svcId => svcId.PackageServiceId).ToList();

                _rootNode.AppendChild(_doc.CreateElement("MasterOrderID")).InnerText = Convert.ToString(supplementOrderCart.MasterOrderId);

                XmlNode _packageServiceNode = _rootNode.AppendChild(_doc.CreateElement("PackageService"));
                foreach (var _packageServiceId in _lstPackageServiceIds)
                {
                    _packageServiceNode.AppendChild(_doc.CreateElement("PackageSvcID")).InnerText = Convert.ToString(_packageServiceId);
                }

                List<SupplementServiceItemCustomForm> lstCustomForms = new List<SupplementServiceItemCustomForm>();
                lstCustomForms = lstSupplementServiceCustomFormList.DistinctBy(x => x.CustomFormID).Select(col => col).ToList();
                foreach (SupplementServiceItemCustomForm customForm in lstCustomForms)
                {
                    List<SupplementOrderData> _lstSupplementOrderData = supplementOrderCart.lstSupplementOrderData
                                                                        .Where(sod => sod.BkgServiceId == customForm.ServiceId)
                                                                        .ToList();
                    List<SupplementOrderData> _lstBkgAttributeDataGrps = _lstSupplementOrderData
                                                                            .Where(svc => svc.PackageSvcItemId == customForm.ServiceItemID
                                                                                  && svc.BkgServiceId == customForm.ServiceId)
                                                                            .ToList();
                    foreach (var _attrGrp in _lstBkgAttributeDataGrps)
                    {
                        XmlNode _attDataGrpNode = _rootNode.AppendChild(_doc.CreateElement("BkgSvcAttributeDataGroup"));
                        _attDataGrpNode.AppendChild(_doc.CreateElement("AttributeGroupID")).InnerText = Convert.ToString(_attrGrp.BkgSvcAttributeGroupId);
                        _attDataGrpNode.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(_attrGrp.InstanceId);

                        if (_attrGrp.FormData.Any(cond => cond.Key == attributeGroupMappingIdForState))//if not personal alias type mapping id then add mapping id for country
                        {
                            XmlNode expChild_Country = _attDataGrpNode.AppendChild(_doc.CreateElement("BkgSvcAttributeData"));
                            expChild_Country.AppendChild(_doc.CreateElement("BkgAttributeGroupMappingID")).InnerText = Convert.ToString(attributeGroupMappingIdForCountry);
                            expChild_Country.AppendChild(_doc.CreateElement("Value")).InnerText = Convert.ToString("UNITED STATES");
                        }
                        //UAT-2100:
                        Boolean isArmedForceStateExist = IsArmedForcesStateExist(_attrGrp, attributeGroupMappingIdForState, attributeGroupMappingIdForCounty);
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        SetAliasMiddleNameIfNotExist(_attrGrp, attributeGroupMappingIdForAlias);
                        foreach (var _attrData in _attrGrp.FormData)
                        {
                            XmlNode expChild = _attDataGrpNode.AppendChild(_doc.CreateElement("BkgSvcAttributeData"));
                            expChild.AppendChild(_doc.CreateElement("BkgAttributeGroupMappingID")).InnerText = Convert.ToString(_attrData.Key);
                            expChild.AppendChild(_doc.CreateElement("Value")).InnerText = isArmedForceStateExist ? String.Empty : Convert.ToString(_attrData.Value);
                        }
                    }
                }
                if (_lstPackageServiceIds.IsNullOrEmpty())
                {
                    //New Change 21072016
                    if (!supplementOrderCart.LstSupplementServiceId.IsNullOrEmpty())
                    {
                        _lstPackageServiceIds = supplementOrderCart.LstSupplementServiceId;
                    }
                }
                Boolean isAnyLineItemCreated = _presenter.GenerateSupplementOrder(this.SelectedTenantID, base.CurrentUserId, this.MasterOrderID, _doc.OuterXml, supplementOrderCart.OrdPkgSvcGroupId, _lstPackageServiceIds);
                if (isAnyLineItemCreated)
                {
                    base.ShowSuccessMessage("Supplement order has been placed successfully.");
                    fsucCmdBar1.SaveButton.Text = "Back to order details";
                    //UAT-2066:"Continue" button click from the supplement review screen should return the user to the order review queue
                    ResultMessage = "Supplement order has been placed successfully.";
                    ResultMessageType = "success";
                    RedirectToOrderReviewQueue();
                }
                else
                {
                    msgText = "No external services have been created for this order supplement. Please change review status of service group to Review Completed or try to supplement again with valid input.";
                    msgType = "info";
                    //UAT-2117:"Continue" button behavior
                    CheckAndUpdateBackgroundOrder(msgText);
                }
                fsucCmdBar1.CancelButton.Enabled = false;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
                msgText = ex.Message;
                msgType = "error";
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
                msgText = ex.Message;
                msgType = "error";
            }
            finally
            {
                if (msgType == "info")
                    base.ShowInfoMessage(msgText);
                else if (msgType == "error")
                    base.ShowErrorMessage(msgText);
            }
        }
        private Boolean IsArmedForcesStateExist(SupplementOrderData suppOrderData, Int32 attributeGroupMappingIdForState, Int32 attributeGroupMappingIdForCounty)
        {
            if (!suppOrderData.FormData.IsNullOrEmpty() && suppOrderData.FormData.ContainsKey(attributeGroupMappingIdForState) && suppOrderData.FormData.Any(cnd => cnd.Value.ToLower().Contains(("Armed Forces").ToLower())))
            {
                return true;
            }
            return false;
        }
        private void SetAliasMiddleNameIfNotExist(SupplementOrderData suppOrderData, Int32 attributeGroupMappingIdForAlias)
        {
            if (!suppOrderData.FormData.IsNullOrEmpty() && suppOrderData.FormData.ContainsKey(attributeGroupMappingIdForAlias))
            {
                String aliasName = suppOrderData.FormData[attributeGroupMappingIdForAlias];
                String[] splitedAliasNames = aliasName.IsNullOrEmpty() ? null : aliasName.Trim().Split(' ');
                if (!splitedAliasNames.IsNullOrEmpty())
                {
                    String firstName = splitedAliasNames.First().Trim();
                    //UAT-2309:Remove middle names only from supplement searches. We should only send the first and last names to clearstar with “——-“ 
                    //in the middle name field always. only one version of each first/last combo should be displayed and sent for supplement search.
                    String middleName = String.Empty; //String.Join(" ", splitedAliasNames.Where((cond, index) => index < splitedAliasNames.Length - 1 && index > AppConsts.NONE));
                    String lastName = splitedAliasNames.Length > AppConsts.ONE ? splitedAliasNames.LastOrDefault().Trim() : String.Empty;
                    String NoMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY].IsNull() ? String.Empty : WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
                    if (middleName.IsNullOrEmpty())
                    {
                        middleName = middleName.IsNullOrEmpty() ? NoMiddleNameText : middleName;
                        aliasName = firstName + " " + middleName + " " + lastName;
                        suppOrderData.FormData[attributeGroupMappingIdForAlias] = aliasName;
                    }
                }
            }
        }
        /// <summary>
        /// Method to check and apply success indicator and update the order status.
        /// </summary>
        private void CheckAndUpdateBackgroundOrder(String errorMsg)
        {
            Boolean saveStatus = Presenter.CheckOrderToUpdate(this.MasterOrderID, this.CurrentUserId);
            //UAT-2200:Should send back to queue if everything in the service group is complete and clear and other service group(s) are in progress
            if (CurrentViewContext.IsOtherServiceGroupsAreCompleted)
            {
                if (CurrentViewContext.IsSuccessIndicatorApplicable && CurrentViewContext.IsAllExistingSearchesAreClear)
                {
                    ResultMessage = saveStatus ? "Order has been completed and success flag is applied on order because no external services have been created for this supplement order."
                                               : "Some error has occurred.Please try again.";
                    ResultMessageType = saveStatus ? "info" : "error";
                    RedirectToOrderReviewQueue();
                }
                else if (!CurrentViewContext.IsSuccessIndicatorApplicable || !CurrentViewContext.IsAllExistingSearchesAreClear)
                {
                    RedirectToVendorServicesScreen();
                }
            }
            else
            {
                ResultMessage = errorMsg;
                ResultMessageType = "info";
                RedirectToOrderReviewQueue();
            }
        }
        private void RedirectToOrderReviewQueue()
        {
            //New Change 21072016
            supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
            var _opsgId = supplementOrderCart.OrdPkgSvcGroupId;
            var _parentScreen = supplementOrderCart.ParentScreen;
            supplementOrderCart = null;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART, supplementOrderCart);
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {"Child",ChildControls.BackgroundOrderReviewQueue},
                                                                    { "ShowSuppSuccessMessage","true"},
                                                                    { "MessageType",ResultMessageType},
                                                                    { "Message",ResultMessage},
                                                                 };

            String url = String.Format("~/BkgOperations/Default.aspx?ucid={0}&args={1}", new Guid(), queryString.ToEncryptedQueryString());
            Response.Redirect(url);
        }
        private void RedirectToVendorServicesScreen()
        {
            //New Change 21072016
            supplementOrderCart = (SupplementOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART);
            var _opsgId = supplementOrderCart.OrdPkgSvcGroupId;
            //var _parentScreen = supplementOrderCart.ParentScreen;
            supplementOrderCart = null;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART, supplementOrderCart);
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantID) },
                                                                    { "OrderId", Convert.ToString(this.MasterOrderID)},
                                                                    { AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, Convert.ToString(_opsgId)},
                                                                    { AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, AppConsts.BKG_ORDER_REVIEW_QUEUE},
                                                                     { AppConsts.ORDER_NUMBER, this.MasterOrderNumber},
                                                                     { AppConsts.MENU_ID, AppConsts.MINUS_TWO.ToString()},
                                                                     { AppConsts.SOURCE_SCREEN_NAME, AppConsts.ORDER_DETAIL_FOR_SERVICE_ITEM_SUPPLEMENT},
                                                                 };

            String url = String.Format("~/BkgOperations/Pages/BkgOrderDetailPage.aspx?ucid={0}&args={1}", new Guid(), queryString.ToEncryptedQueryString());
            Response.Redirect(url);
        }
        #endregion
    }
}