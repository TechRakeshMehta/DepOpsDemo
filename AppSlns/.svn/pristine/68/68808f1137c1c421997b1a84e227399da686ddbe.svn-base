using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using Entity.ClientEntity;
using System.Collections.Generic;
using System.Linq;
using INTSOF.Utils;
using System.Web.UI;
using System.Collections.Specialized;
using System.Web;
using System.Threading;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class RushOrderReview : BaseUserControl, IRushOrderReviewView
    {
        #region Variables

        #region Private Variables

        private RushOrderReviewPresenter _presenter = new RushOrderReviewPresenter();
        private Int32 _tenantId;
        private OrganizationUserProfile _orgUserProfile;
        private OrganizationUserProfile _organizationUserProfile;
        private ApplicantOrderCart _applicantOrderCart;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public RushOrderReviewPresenter Presenter
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

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                    _tenantId = 3;
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        public IRushOrderReviewView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 OrderId
        {
            get { return (Int32)(ViewState["OrderId"]); }
            set { ViewState["OrderId"] = value; }
        }

        public Int32 DeptProgramPackageSubscriptionId
        {
            get { return (Int32)(ViewState["DeptProgramPackageSubscriptionId"]); }
            set { ViewState["DeptProgramPackageSubscriptionId"] = value; }
        }

        public Int32 SubscriptionId
        {
            get { return (Int32)(ViewState["SubscriptionId"]); }
            set { ViewState["SubscriptionId"] = value; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public String PaymentModeCode
        {
            get;
            set;
        }

        public DeptProgramPackageSubscription SelectedPackageDetails
        {
            get;
            set;
        }

        public List<lkpPaymentOption> lstPaymentOptions
        {
            get;
            set;
        }

        public Entity.OrganizationUser OrganizationUser
        {
            get;
            set;
        }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        public String NextPagePath
        {
            get;
            set;
        }   
        
        /// <summary>
        /// Represents the hierarhcy which was selected by applicant, durint actual order placement
        /// i.e. hierarchy by SelectedNodeId
        /// </summary>
        String IRushOrderReviewView.SelectedNodeHierarchy
        {
            set
            {
                lblInstitutionHierarchy.Text = value;
            }
        }
        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.SetPageTitle(" (Step 1 of 2)");

                if (!this.IsPostBack)
                {
                    //_presenter.GetTenantId();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    //Decrypt Query String
                    queryString.ToDecryptedQueryString(Request.QueryString["args"]);
                    //Checks if the OrderId is present in Query String.
                    if (queryString.ContainsKey("OrderId"))
                    {
                        //Assigns the OrderId to property OrderId.
                        if (!queryString["OrderId"].IsNullOrEmpty())
                        {
                            CurrentViewContext.OrderId = Convert.ToInt32(queryString["OrderId"]);
                            Session["OrderId"] = Convert.ToInt32(queryString["OrderId"]);
                        }
                    }
                    if (queryString.ContainsKey("SubscriptionOptionID"))
                    {
                        //Assigns the SubscriptionId to property SubscriptionId.
                        if (!queryString["SubscriptionOptionID"].IsNullOrEmpty())
                        {
                            CurrentViewContext.SubscriptionId = Convert.ToInt32(queryString["SubscriptionOptionID"]);
                            Session["SubscriptionId"] = Convert.ToInt32(queryString["SubscriptionOptionID"]);
                        }
                    }
                    if (Session["ClientMachineIP"].IsNullOrEmpty())
                    {
                        Session["ClientMachineIP"] = Request.UserHostAddress;
                    }
                    Presenter.OnViewInitialized();
                    BindPaymentOptions();
                    ShowHidePaymentInstructionSection();
                }

                CheckRushOrder();
                Presenter.OnViewLoaded();

                if (!IsDisclaimerAccepted())
                {
                    //RedirectToDisclaimerPage();
                }
                BindControls();
                ShowHideBillingDetailsCheckboxControl();

                cmdbarSubmit.SubmitButton.ToolTip = "Click to return to the dashboard";
                cmdbarSubmit.ClearButton.ToolTip = "Submit and pay for your order";

                (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle("Rush Order");
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


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                /* ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                Dictionary<String, String> queryString;
                if (applicantOrderCart.PrevOrderId == AppConsts.NONE)
                {
                    Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                    Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { AppConsts.CHILD,  ChildControls.ApplicantPendingOrder}
                                                                 };
                    Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
                else
                {
                    Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                    Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
                    queryString = new Dictionary<String, String>()
                                                         { 
                                                            {"OrderId",applicantOrderCart.PrevOrderId.ToString()},
                                                            { "Child",  ChildControls.RenewalOrder}
                                                         };
                    //Response.Redirect(String.Format("~/Main/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                    Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                } */

                String url = String.Format(AppConsts.DASHBOARD_URL);
                Response.Redirect(url);
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Order _applicantOrder = new Order();
            // _applicantOrder.PaymentOptionID = Convert.ToInt32(cmbPaymentModes.SelectedValue);
            // _orgUserProfile.OrganizationUserID = CurrentLoggedInUserId;

            //// Presenter.SubmitApplicantOrder(_orgUserProfile, _applicantOrder, Convert.ToInt32(cmbPaymentModes.SelectedValue));

            //// if (CurrentViewContext.GeneratedOrderId > 0 && CurrentViewContext.PaymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower())
            // {
            //     SubmitOnlinePayment();
            // }
        }

        protected void cmdbarSubmit_CancelClick(object sender, EventArgs e)
        {
            try
            {
                String controlUseType = _applicantOrderCart.ParentControlType;
                Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
                Session.Remove(AppConsts.DISCLOSURE_ACCEPTED);
                //UAT-1560
                Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
                if (controlUseType == AppConsts.DASHBOARD)
                    Response.Redirect(AppConsts.DASHBOARD_URL, true);
                else
                {
                    Dictionary<String, String> queryString;
                    queryString = new Dictionary<String, String> { { AppConsts.CHILD, ChildControls.OrderHistory } };
                    Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
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

        #region Methods

        #region Public Methods

        public Boolean IsDisclaimerAccepted()
        {
            if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.DISCLAIMER_ACCEPTED).IsNotNull() && (Boolean)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.DISCLAIMER_ACCEPTED))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RedirectToDisclaimerPage()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {AppConsts.CHILD,  ChildControls.ApplicantDisclaimerPage}
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

        }

        public void RedirectToRushOrderConfirmation()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { AppConsts.CHILD,  ChildControls.ApplicantRushOrderConfirmPage }
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

        }


        #endregion

        #region Private Methods

        private void BindControls()
        {
            BindPackageDetails();
            //BindPaymentOptions();
        }

        private void BindPackageDetails()
        {
            // lblDepartment.Text = CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.DeptProgramMapping.Organization.OrganizationName; ;
            // lblProgram.Text = CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.DeptProgramMapping.AdminProgramStudy.ProgramStudy;

            //UAT 1067: Hierarchy for orders (background and screening) should display as the full 
            // hierarchy sleected during the order, not the node the package lives on.
            //lblInstitutionHierarchy.Text = CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.DeptProgramMapping.DPM_Label;

            lblPackage.Text = (String.IsNullOrEmpty(CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.CompliancePackage.PackageLabel)
                ? CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.CompliancePackage.PackageName
                : CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.CompliancePackage.PackageLabel).HtmlEncode();

            lblPackageDescription.Text = CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.CompliancePackage.Description.HtmlEncode();
            lblOrderNumber.Text = Convert.ToString(CurrentViewContext.OrderId);
            lblFirstName.Text = CurrentViewContext.OrganizationUser.FirstName.HtmlEncode();
            lblLastName.Text = CurrentViewContext.OrganizationUser.LastName.HtmlEncode();

            if (CurrentViewContext.OrganizationUser.DOB.HasValue)
            {
                lblDateOfBirth.Text = CurrentViewContext.OrganizationUser.DOB.Value.ToShortDateString();
            }
            lblEmail.Text = CurrentViewContext.OrganizationUser.PrimaryEmailAddress.HtmlEncode();
            lblPhone.Text = Presenter.GetFormattedPhoneNumber(CurrentViewContext.OrganizationUser.PhoneNumber);


            if (CurrentViewContext.SelectedPackageDetails.DPPS_RushOrderAdditionalPrice.HasValue)
            {
                lblRushOrderPrice.Text = "$ " + Convert.ToString(decimal.Round(CurrentViewContext.SelectedPackageDetails.DPPS_RushOrderAdditionalPrice.Value, 2));
            }
            CurrentViewContext.DeptProgramPackageSubscriptionId = CurrentViewContext.DeptProgramPackageSubscriptionId = CurrentViewContext.SelectedPackageDetails.DPPS_ID;
            hdnDeptPrgPackageSubscriptionId.Value = Convert.ToString(CurrentViewContext.DeptProgramPackageSubscriptionId);
        }

        private void BindPaymentOptions()
        {
            //ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;

            //_orgUserProfile = new OrganizationUserProfile();
            //foreach (var applicantOrder in applicantOrderCart.lstApplicantOrder)
            //{
            //    _orgUserProfile = applicantOrder.OrganizationUserProfile;
            //CurrentViewContext.DPPSId = CurrentViewContext.DPPSId = applicantOrder.DPPS_Id.FirstOrDefault();
            //    CurrentViewContext.UpdateOriginalData = applicantOrder.UpdatePersonalDetails;
            //}

            Presenter.GetPaymentOptions();
            cmbPaymentOptions.DataSource = CurrentViewContext.lstPaymentOptions;
            cmbPaymentOptions.DataBind();
        }

        private void SubmitOnlinePayment()
        {
            Response.Redirect("~/ComplianceOperations/Pages/OnlinePayment.aspx");
        }

        private void CheckOnlinePayment()
        {
            try
            {
                ErrorLog logFile = new ErrorLog("Data is sent from Rush Order Review page.");
                RedirectToRushOrderConfirmation();
            }
            catch (Exception ex)
            {
                ErrorLog logFile = new ErrorLog("Problem in sending data from Rush Order Review page" + ex);
            }
        }

        private void CheckRushOrder()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            RedirectIfIncorrectOrderStage(_applicantOrderCart);

            if (_applicantOrderCart.lstApplicantOrder[0].OrderId != AppConsts.NONE)
            {
                Presenter.GetTenantId();
                Order order = Presenter.GetOrderById(_applicantOrderCart.lstApplicantOrder[0].OrderId);

                if (order.IsNotNull())
                {
                    // UAT 1067: Hierarchy for orders (background and screening) should display as the full hierarchy
                    // sleected during the order, not the node the package lives on.
                    if (order.DeptProgramMapping1.IsNotNull())
                    {
                        CurrentViewContext.SelectedNodeHierarchy = order.DeptProgramMapping1.DPM_Label;
                    }

                    CheckRushOrderStatus(order);
                }
                else
                {
                    RedirectToOrderHistory();
                }
            }
        }

        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (_applicantOrderCart.IsNull())
            {
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return _applicantOrderCart;
        }

        private void CheckRushOrderStatus(Order order)
        {
            Int32? rushOrderStatusID = order.RushOrderStatusID;

            if (rushOrderStatusID.IsNotNull() && !order.lkpOrderStatu1.Code.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()))
            {
                CheckOnlinePayment();
            }
        }

        /// <summary>
        /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void RedirectIfIncorrectOrderStage(ApplicantOrderCart applicantOrderCart)
        {
            if (applicantOrderCart.IsNull() || _applicantOrderCart.lstApplicantOrder[0].OrderId == AppConsts.NONE)
            {
                RedirectToOrderHistory();
            }
            Presenter.GetNextPagePathByOrderStageID(applicantOrderCart);

            //Redirect to next page path if Order Status track is not correct.
            if (CurrentViewContext.NextPagePath.IsNotNull())
            {
                Response.Redirect(CurrentViewContext.NextPagePath);
            }
            else
            {
                applicantOrderCart.AddOrderStageTrackID(OrderStages.RushOrderReview);
            }
        }

        private void RedirectToOrderHistory()
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { AppConsts.CHILD,  ChildControls.OrderHistory }
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }

        /// <summary>
        /// Method to Show/hide "Billing Details Info" checkbox control
        /// </summary>
        private void ShowHideBillingDetailsCheckboxControl()
        {
            //UAT-261

            //Commented corresponding to UAT - 780
            //if (!cmbPaymentOptions.SelectedValue.IsNullOrEmpty())
            //{
            //    if (Presenter.GetPaymentCodeByID(Convert.ToInt32(cmbPaymentOptions.SelectedValue)).ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower())
            //    {
            //        chkBillingAddress.Visible = true;
            //        dvBillingAddress.Visible = true;
            //    }
            //    else
            //    {
            //        chkBillingAddress.Visible = false;
            //        dvBillingAddress.Visible = false;
            //    }
            //}
        }

        #endregion

        protected void cmbPaymentOptions_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ShowHideBillingDetailsCheckboxControl();
            ShowHidePaymentInstructionSection();
        }

        #endregion


        private void ShowHidePaymentInstructionSection()
        {
            if (!cmbPaymentOptions.SelectedValue.IsNullOrEmpty())
            {
                String paymentInstruction = Presenter.GetPaymentInstruction(Convert.ToInt32(cmbPaymentOptions.SelectedValue));
                if (paymentInstruction.IsNotNull())
                {
                    divPaymentInstruction.Visible = true;
                    litPaymentInstruction.Text = paymentInstruction;
                }
                else
                {
                    divPaymentInstruction.Visible = false;
                }
            }
        }
    }
}

