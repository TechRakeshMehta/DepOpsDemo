using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.AuthNet.Business;
using INTSOF.AuthNet.Business.CustomerProfileWS;
using Telerik.Web.UI;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity.ClientEntity;
using System.Text;
using CoreWeb.IntsofSecurityModel;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.Services;
using Business.RepoManagers;
using INTSOF.Contracts;
using System.Data;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class CIMAccountSelection : BaseWebPage, ICIMAccountSelectionView
    {
        #region Variables

        #region Private Variables

        private CIMAccountSelectionPresenter _presenter = new CIMAccountSelectionPresenter();
        private ApplicantOrderCart _applicantOrderCart;

        /// <summary>
        /// Lock Object to avoid multiple transactions updating the 'IsPaymentResponsePending ' in Session
        /// </summary>
        private static String _orderLockObject = String.Empty;
        #endregion

        #region Public Variables

        public Boolean IsCustomerProfileExist
        {
            get;
            set;
        }

        public long CustomerProfileId
        {
            get
            {
                if (ViewState["CustomerProfileId"] != null)
                    return Convert.ToInt64(ViewState["CustomerProfileId"]);
                else
                    return 0;
            }
            set { ViewState["CustomerProfileId"] = value; }
        }


        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Guid UserId
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                return user.UserId;
            }
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public String InvoiceNumber
        {
            get;
            set;
        }

        public OnlinePaymentTransaction OnlinePaymentTransactionDetails
        {
            get;
            set;
        }
        //UAT 4537
        public List<OnlinePaymentTransaction> LstOnlinePaymentTransactionDetails
        {
            get;
            set;
        }
        public ICIMAccountSelectionView CurrentViewContext
        {
            get { return this; }
        }


        public CIMAccountSelectionPresenter Presenter
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
        /// Get and set next page path.
        /// </summary>
        public String NextPagePath
        {
            get;
            set;
        }

        public Entity.OrganizationUser OrganizationUserData { get; set; }

        public Boolean BillingInfoCheckedStatus { get; set; }

        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        public Int32 OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return CurrentLoggedInUserId;
            }
        }

        #region BILLING INFORMATION PROPERTIES

        public String OrganizationUserID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Company { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Zip { get; set; }
        public String Country { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }
        public List<Int32> OrderIds { get; set; }
        private string strOrderId { get; set; }
        #endregion

        /// <summary>
        /// Represents the OPDId for which the status is to be changed
        /// </summary>
        Int32 ICIMAccountSelectionView.OPDId
        {
            get;
            set;
        }

        Boolean ICIMAccountSelectionView.IsItemPayment
        {
            get;
            set;
        }


        long ICIMAccountSelectionView.DefaultpaymentProfileId
        {
            get;
            set;
        }
        Boolean ICIMAccountSelectionView.IsInstructorPreceptorPackage
        {
            get;
            set;
        }
        #endregion

        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CoreWeb.Shell.MasterPages.DefaultMaster masterPage = this.Master as CoreWeb.Shell.MasterPages.DefaultMaster;
                if (masterPage != null)
                {
                    masterPage.HideTitleBars(true);
                    masterPage.UseAsPopUpWindow = true;
                }
                Dictionary<String, String> args = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    CurrentViewContext.InvoiceNumber = args.GetValue("invnum");
                    strOrderId = args.GetValue("OrderId");

                    CurrentViewContext.OrderIds = Presenter.GetOrderAndTenantID(CurrentViewContext.InvoiceNumber)["OrderID"];
                    if (!(CurrentViewContext.OrderIds.IsNotNull() && CurrentViewContext.OrderIds.Count > 0))
                        CurrentViewContext.OrderIds.Add(Convert.ToInt32(args.GetValue("OrderId")));

                    //IsItemPayment
                    if (!args.GetValue("IsItemPayment").IsNullOrEmpty() && args.GetValue("IsItemPayment").FirstOrDefault() != null)
                        CurrentViewContext.IsItemPayment = Convert.ToBoolean(args.GetValue("IsItemPayment"));

                    if (args.ContainsKey("IsInstructorPreceptorPackage") && !args.GetValue("IsInstructorPreceptorPackage").IsNullOrEmpty())
                    {
                        CurrentViewContext.IsInstructorPreceptorPackage = Convert.ToBoolean(args.GetValue("IsInstructorPreceptorPackage"));
                    }
                    if (args.ContainsKey("SelectedTenantID") && !args.GetValue("SelectedTenantID").IsNullOrEmpty())
                    {
                        if (CurrentViewContext.IsInstructorPreceptorPackage)
                        {
                            CurrentViewContext.TenantId = Convert.ToInt32(args.GetValue("SelectedTenantID"));
                        }
                    }
                }
                else
                {
                    return;
                }
                if (!CurrentViewContext.IsItemPayment || CurrentViewContext.IsItemPayment.IsNullOrEmpty())
                    SetBillingInformation();
                else
                    BindBillingInformationForItemPayment();

                ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as ItemPaymentContract;
                if (CurrentViewContext.IsInstructorPreceptorPackage)
                    itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
                Presenter.OnViewInitialized();
                lblMessage.Visible = false;
                if (!IsPostBack && CurrentViewContext.OnlinePaymentTransactionDetails.IsNotNull())
                {
                    long customerProfileId = 0;
                    Entity.AuthNetCustomerProfile customerProfile = Presenter.GetCustomerProfile(UserId);
                    if (customerProfile.IsNullOrEmpty())
                    {
                        customerProfileId = CreateCustomerProfile();
                        BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 1: Method 'CreateCustomerProfile' executed successfully for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);
                    }
                    else
                    {
                        customerProfileId = Convert.ToInt64(customerProfile.CustomerProfileID);
                    }
                    CustomerProfileId = customerProfileId;
                    //ErrorLog logFile = new ErrorLog("Data is sent from CIMAccountSelection page for InvoiceNumber " + CurrentViewContext.InvoiceNumber + "."); 
                }
                hfProfileID.Value = Convert.ToString(CustomerProfileId);
                hiddenToken.Value = GetToken(CustomerProfileId);

                if (!String.IsNullOrEmpty(hiddenToken.Value))
                {
                    BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 2: Token generated successfully for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);
                }
                else
                {
                    BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 2: Token could not be generated for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);
                }

                hiddenIsTestMode.Value = Settings.IsTestRequest;
                if (!CurrentViewContext.IsItemPayment)
                {
                    base.SetPageTitle(Resources.Language.CRDTCRDPYMNT);
                    base.Title = Resources.Language.CRDTCRDPYMNT;
                }

                //UAT-3573 || CBI || CABS
                SkipSubmitClick();
                // SkipSubmitForLatestAddedCard();
            }
            catch (Exception ex)
            {
                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 1: Exception on Page_Load for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber + ". Exception is: " + Convert.ToString(ex));
                //ErrorLog logFile = new ErrorLog("Problem in sending data from CIMAccountSelection page" + ex);
            }
        }

        /// <summary>
        /// Create the customer profile
        /// </summary>
        /// <returns>The ID for the customer profile that was created</returns>
        public long CreateCustomerProfile()
        {
            long authNetCustomerProfileId = 0;
            String description = GenerateDescription();
            long customerProfileId = AuthorizeNetCreditCard.CreateCustomerProfile(CurrentLoggedInUserId, CurrentViewContext.Email, description);
            if (customerProfileId > 0)
            {
                Entity.AuthNetCustomerProfile authNetCustomerProfile = new Entity.AuthNetCustomerProfile()
                {
                    UserID = UserId,
                    //CreatedById = CurrentLoggedInUserId,
                    CreatedById = OrgUsrID,
                    CreatedDate = DateTime.Now,
                    CustomerProfileID = Convert.ToString(customerProfileId),
                    IsDeleted = false
                };
                authNetCustomerProfileId = Presenter.CreateNewAuthNetCustomerProfile(authNetCustomerProfile);
            }
            return authNetCustomerProfileId;
        }

        private string GenerateDescription()
        {
            //02/17/2015 [SG]: UAT-1023 - Complio: Update Credit Card Spreadsheet
            //return ("Complio: " + Presenter.GetTenantName() + " (Order #:  " + OrderId + ")");
            return Presenter.GetTenantName();
        }

        protected void grdPaymentProfiles_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                List<PaymentProfileDetail> lstPaymentProfileDetail = AuthorizeNetCreditCard.GetCustomerPaymentProfiles(CustomerProfileId);
                grdPaymentProfiles.DataSource = lstPaymentProfileDetail;
                if (lstPaymentProfileDetail.IsNullOrEmpty())
                {
                    btnSubmitPayment.Enabled = false;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblMessage.Visible = true;
                lblMessage.ShowMessage(ex.Message, MessageType.Error);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblMessage.Visible = true;
                lblMessage.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void grdPaymentProfiles_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                long paymentProfileId = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("CustomerPaymentProfileId"));
                if (paymentProfileId > 0)
                {
                    if (AuthorizeNetCreditCard.DeleteCustomerPaymentProfile(CustomerProfileId, paymentProfileId))
                    {
                        lblMessage.Visible = true;
                        lblMessage.ShowMessage(Resources.Language.CRDTCRDDLTSUCMSG, MessageType.SuccessMessage);
                        HandleSubmitClickOnDeletion();
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.ShowMessage(Resources.Language.CRDTCRDDLTERRMSG, MessageType.Error);
                    }
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ShowMessage(Resources.Language.CRDTCRDDLTERRMSG, MessageType.Error);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblMessage.Visible = true;
                lblMessage.ShowMessage(ex.Message, MessageType.Error);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblMessage.Visible = true;
                lblMessage.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnSubmitPayment_Click(object sender, EventArgs e)
        {
            try
            {
                long paymentProfileId = 0;

                FingerPrintAppointmentContract _fingerPrintAppointmentData = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_LOCATION_CART) as FingerPrintAppointmentContract;
                if (!_applicantOrderCart.IsNullOrEmpty() && _applicantOrderCart.OrderPaymentdetailId <= 0)
                {


                    if (_applicantOrderCart != null
                      && _applicantOrderCart.IsLocationServiceTenant && _fingerPrintAppointmentData != null)
                    {
                        if (!_fingerPrintAppointmentData.IsOutOfState && !_fingerPrintAppointmentData.IsFromArchivedOrderScreen)
                        {
                            if (FingerPrintSetUpManager.IsReservedSlotExpired(_fingerPrintAppointmentData.ReserverSlotID, SysXWebSiteUtils.SessionService.OrganizationUserId, true))
                            {
                                lblMessage.Visible = true;
                                lblMessage.ShowMessage(Resources.Language.SLOTEXPBEFOREPAYMENTALRTMSG, MessageType.Error);
                                btnSubmitPayment.Enabled = false;
                                btnGOTODASHBOARD.Visible = true;
                                return;
                            }
                        }
                    }
                }
                if (_applicantOrderCart != null
                    && _applicantOrderCart.IsLocationServiceTenant
                    && !CurrentViewContext.DefaultpaymentProfileId.IsNullOrEmpty()
                    && CurrentViewContext.DefaultpaymentProfileId > AppConsts.NONE)
                {
                    paymentProfileId = CurrentViewContext.DefaultpaymentProfileId;
                }

                foreach (GridDataItem item in grdPaymentProfiles.Items)
                {
                    RadioButton rdbCardNumber = (RadioButton)item.FindControl("rdbCardNumber");
                    if (rdbCardNumber.Checked == true)
                    {
                        paymentProfileId = Convert.ToInt64(item.GetDataKeyValue("CustomerPaymentProfileId"));
                    }
                }

                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 3: Payment submission initiated for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);
                //UAT 4537 Allow the CC payment method packages that don’t require approval to go through, even while other packages within the same order are still pending approval 
                if (_applicantOrderCart != null && _applicantOrderCart.InvoiceNumber.IsNotNull() && _applicantOrderCart.InvoiceNumber.ContainsKey(PaymentOptions.Credit_Card.GetStringValue()) && _applicantOrderCart.InvoiceNumber.ContainsKey(PaymentOptions.Credit_Card_With_Approval_Required.GetStringValue()))
                {
                    SubmitCardPaymentWithApprovalRequired(paymentProfileId);
                    return;
                }

                if (paymentProfileId > 0)
                {
                    String description = GenerateDescription();

                    var _amount = CurrentViewContext.OnlinePaymentTransactionDetails
                        .OrderPaymentDetails
                        .Where(x => x.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue())
                        .Sum(opd => opd.OPD_Amount);

                    Boolean paymentResponsePending = true;

                    #region UAt-3077
                    if (CurrentViewContext.IsItemPayment)
                    {
                        ItemPaymentSubmission(paymentProfileId);

                    }
                    #endregion

                    // UAT-1760: Lock the '_applicantOrderCart.IsPaymentResponsePending', so that 
                    // subsequent request(due to double click), cannot update the value and get's updated value from the session, updated by the first request. 
                    // Covers the following cases:
                    // 1. When first request is already submitted to authorize.net and response not received yet and second request also tries to submit request to authorize.net
                    // 2. When first request is submitted to authorize.net and response is received AND second request also tries to submit request to authorize.net, without any time-frame delay.
                    // 3. When response for first request is received from authorize.net AND second request also tries to submit request to authorize.net, with delay of any time-frame.
                    // 4. When a successfull request has been submitted and, however, applicant stays on this page, then tries to submit another request.
                    lock (_orderLockObject)
                    {
                        _applicantOrderCart = GetApplicantOrderCart();
                        if (!_applicantOrderCart.IsPaymentResponsePending)
                        {
                            _applicantOrderCart.IsPaymentResponsePending = true;
                            UpdateApplicantOrderCart(_applicantOrderCart);
                            paymentResponsePending = false;
                        }
                    }

                    // UAT-1760: This will prevent the subsequent/duplicate request for payment submission.
                    if (!paymentResponsePending)
                    {
                        //UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
                        //Get client approval required setting from SP
                        String strOrderIDs = _applicantOrderCart.AllOrderIDs;
                        if (strOrderIDs.IsNullOrEmpty())
                            strOrderIDs = Convert.ToString(_applicantOrderCart.lstApplicantOrder[0].OrderId);
                        Boolean isSchoolApprovalRequired = Presenter.IsSchoolApprovalRequired(strOrderIDs);
                        String creditCartPaymentCode = PaymentOptions.Credit_Card.GetStringValue();

                        //UAT-4537 On Change Payment Type Approval Required sholuld only check for Change Payment Package.
                        if (!_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.ChangePaymentTypeCode.IsNullOrEmpty() && !_applicantOrderCart.IsPaymentApprovalRequired)
                        {
                            isSchoolApprovalRequired = false;
                        }
                        if (isSchoolApprovalRequired)
                        {
                            SetApplicationOrderCart();
                            BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 4: Payment Approval required for Authorize.Net for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);

                            _applicantOrderCart.IsPaymentResponsePending = false;

                            List<Entity.ClientEntity.Order> orderList = Presenter.GetListofOrdersForOrderID(strOrderIDs);
                            List<Int32> ccOPDList = new List<Int32>();

                            foreach (var order in orderList)
                            {
                                //_applicantOrderCart.OrderPaymentdetailId > 0 only in case of payment type changed
                                if (_applicantOrderCart.OrderPaymentdetailId > 0)
                                {
                                    ccOPDList.Add(_applicantOrderCart.OrderPaymentdetailId);
                                }
                                else
                                {
                                    var ccOPD = order.OrderPaymentDetails.Where(opd => opd.lkpPaymentOption.Code == creditCartPaymentCode && !opd.OPD_IsDeleted).FirstOrDefault();
                                    if (ccOPD.IsNotNull())
                                        ccOPDList.Add(ccOPD.OPD_ID);
                                }
                            }

                            //Update OPD status to Pending School Approval and Payment Profile ID in "OrderPaymentDetails" table
                            //send notifications to the applicant
                            Presenter.UpdateOPDStatusAndPaymentProfileId(paymentProfileId, ccOPDList);

                            BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 5: Order Status changed to 'Pending School Approval', for InvoiceNumber(s): "
                                + CurrentViewContext.InvoiceNumber);

                            //Send order creation notification for Pending School Approval payment status
                            Presenter.SendNotificationForPendingSchoolApproval(orderList);

                            UpdateApplicantOrderCart(_applicantOrderCart);

                            RedirectToOrderConfirmation(orderList[0]);
                        }
                        else
                        {
                            CreateCustomerProfileTransactionResponseType response = AuthorizeNetCreditCard.CreateCustomerProfileTransaction(CustomerProfileId, paymentProfileId,
                                                                                                                                            CurrentLoggedInUserId, Convert.ToDecimal(_amount),
                                                                                                                                            CurrentViewContext.InvoiceNumber, description);
                            BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 4: Method 'AuthorizeNetCreditCard.CreateCustomerProfileTransaction' executed successfully"
                                    + " for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);

                            NameValueCollection transactionDetails = AuthorizeNetCreditCard.GetResponseFields(response);
                            SetApplicationOrderCart();

                            if (response.resultCode == MessageTypeEnum.Ok)
                            {
                                Session["IsNeedToSendOrderConfirmationDocument"] = true.ToString();//UAT-2970

                                Presenter.SaveTransactionDetails(CurrentViewContext.InvoiceNumber, transactionDetails);
                                // Need to keep it here so that Order is fetched after the Above Save is done.
                                Entity.ClientEntity.Order order = Presenter.GetOrderById(_applicantOrderCart.lstApplicantOrder[0].OrderId);

                                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 5: Result Code from Authorize.Net is:" + response.resultCode + ", redirecting to "
                                 + " Order Confirmation, for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);

                                RedirectToOrderConfirmation(order);
                            }
                            else
                            {
                                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 5: Result Code from Authorize.Net is:" + response.resultCode + " for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);

                                _applicantOrderCart.IsPaymentResponsePending = false;
                                Entity.ClientEntity.Order order = Presenter.GetOrderById(_applicantOrderCart.lstApplicantOrder[0].OrderId);

                                var _creditCartOptionCode = PaymentOptions.Credit_Card.GetStringValue();
                                var _ccOPD = order.OrderPaymentDetails.Where(opd => opd.lkpPaymentOption.Code == _creditCartOptionCode).First();

                                CurrentViewContext.OPDId = _ccOPD.OPD_ID;
                                Presenter.UpdateOPDStatus();

                                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 6: Order Status changed to 'Online_Payment_Not_Completed', for InvoiceNumber(s): "
                                    + CurrentViewContext.InvoiceNumber + ", with failure reason as: " + transactionDetails["x_response_reason_text"]);

                                lblMessage.Visible = true;
                                String errorMessage = Resources.Language.PYMNTFORORDNUM + " " + strOrderId + " " + Resources.Language.FAILEDDUETORSN + ": " + transactionDetails["x_response_reason_text"];
                                lblMessage.ShowMessage(errorMessage, MessageType.Error);
                                UpdateApplicantOrderCart(_applicantOrderCart);
                            }
                        }
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.ShowMessage("Duplicate payment attempt detected. If you are not redirected to order confirmation page shortly, please go to order history page to check the status of your order.", MessageType.Error);
                        BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 4: Duplicate payment attempt detected for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);
                    }
                }

                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ShowMessage(Resources.Language.PLZSELCRDTCRD, MessageType.Information);
                }
            }

            catch (SysXException ex)
            {
                base.LogError(ex);
                lblMessage.Visible = true;
                lblMessage.ShowMessage(ex.Message, MessageType.Error);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblMessage.Visible = true;
                lblMessage.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        private void SubmitCardPaymentWithApprovalRequired(long paymentProfileId)
        {
            if (paymentProfileId > 0)
            {
                String description = GenerateDescription();
                String paymentTypeCC = PaymentOptions.Credit_Card.GetStringValue();
                String paymentTypeCCWA = PaymentOptions.Credit_Card_With_Approval_Required.GetStringValue();

                var _dicInvoiceNumbers = _applicantOrderCart.InvoiceNumber.Where(x => x.Key.Contains(paymentTypeCC) || x.Key.Contains(paymentTypeCCWA));
                Boolean IsRedirectToSummaryPage = false;
                foreach (KeyValuePair<String, String> _invoicNum in _dicInvoiceNumbers.OrderByDescending(x => x.Key))
                {
                    // do something with entry.Value or entry.Key


                    var _amount = CurrentViewContext.LstOnlinePaymentTransactionDetails.FirstOrDefault(x => x.Invoice_num == _invoicNum.Value)
                   .OrderPaymentDetails
                   .Where(x => x.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue())
                   .Sum(opd => opd.OPD_Amount);

                    Boolean paymentResponsePending = true;

                    //#region UAt-3077
                    //if (CurrentViewContext.IsItemPayment)
                    //{
                    //    ItemPaymentSubmission(paymentProfileId);

                    //}
                    //#endregion

                    // UAT-1760: Lock the '_applicantOrderCart.IsPaymentResponsePending', so that 
                    // subsequent request(due to double click), cannot update the value and get's updated value from the session, updated by the first request. 
                    // Covers the following cases:
                    // 1. When first request is already submitted to authorize.net and response not received yet and second request also tries to submit request to authorize.net
                    // 2. When first request is submitted to authorize.net and response is received AND second request also tries to submit request to authorize.net, without any time-frame delay.
                    // 3. When response for first request is received from authorize.net AND second request also tries to submit request to authorize.net, with delay of any time-frame.
                    // 4. When a successfull request has been submitted and, however, applicant stays on this page, then tries to submit another request.
                    lock (_orderLockObject)
                    {
                        _applicantOrderCart = GetApplicantOrderCart();
                        if (!_applicantOrderCart.IsPaymentResponsePending)
                        {
                            _applicantOrderCart.IsPaymentResponsePending = true;
                            UpdateApplicantOrderCart(_applicantOrderCart);
                            paymentResponsePending = false;
                        }
                    }

                    // UAT-1760: This will prevent the subsequent/duplicate request for payment submission.
                    if (!paymentResponsePending)
                    {
                        //UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
                        //Get client approval required setting from SP
                        String strOrderIDs = _applicantOrderCart.AllOrderIDs;
                        if (strOrderIDs.IsNullOrEmpty())
                            strOrderIDs = Convert.ToString(_applicantOrderCart.lstApplicantOrder[0].OrderId);
                        // Boolean isSchoolApprovalRequired = Presenter.IsSchoolApprovalRequired(strOrderIDs);
                        String creditCartPaymentCode = PaymentOptions.Credit_Card.GetStringValue();

                        if (_invoicNum.Key == PaymentOptions.Credit_Card_With_Approval_Required.GetStringValue())
                        {
                            SetApplicationOrderCart();
                            BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 4: Payment Approval required for Authorize.Net for InvoiceNumber(s): " + _invoicNum.Value);

                            _applicantOrderCart.IsPaymentResponsePending = false;

                            List<Entity.ClientEntity.Order> orderList = Presenter.GetListofOrdersForOrderID(strOrderIDs);
                            List<Int32> ccOPDList = new List<Int32>();

                            foreach (var order in orderList)
                            {
                                //_applicantOrderCart.OrderPaymentdetailId > 0 only in case of payment type changed
                                if (_applicantOrderCart.OrderPaymentdetailId > 0)
                                {
                                    ccOPDList.Add(_applicantOrderCart.OrderPaymentdetailId);
                                }
                                else
                                {
                                    var ccOPD = order.OrderPaymentDetails.Where(opd => opd.lkpPaymentOption.Code == creditCartPaymentCode && !opd.OPD_IsDeleted && opd.OnlinePaymentTransaction.Invoice_num == _invoicNum.Value).FirstOrDefault();
                                    if (ccOPD.IsNotNull())
                                        ccOPDList.Add(ccOPD.OPD_ID);
                                }
                            }

                            //Update OPD status to Pending School Approval and Payment Profile ID in "OrderPaymentDetails" table
                            //send notifications to the applicant
                            Presenter.UpdateOPDStatusAndPaymentProfileId(paymentProfileId, ccOPDList);

                            BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 5: Order Status changed to 'Pending School Approval', for InvoiceNumber(s): "
                                + _invoicNum.Value);

                            //Send order creation notification for Pending School Approval payment status
                            Presenter.SendNotificationForPendingSchoolApproval(orderList);

                            UpdateApplicantOrderCart(_applicantOrderCart);
                            IsRedirectToSummaryPage = true;
                            //   RedirectToOrderConfirmation(orderList[0]);
                        }
                        else
                        {
                            CreateCustomerProfileTransactionResponseType response = AuthorizeNetCreditCard.CreateCustomerProfileTransaction(CustomerProfileId, paymentProfileId,
                                                                                                                                            CurrentLoggedInUserId, Convert.ToDecimal(_amount),
                                                                                                                                                      //   CurrentViewContext.InvoiceNumber, description);
                                                                                                                                                      _invoicNum.Value, description);
                            BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 4: Method 'AuthorizeNetCreditCard.CreateCustomerProfileTransaction' executed successfully"
                                    + " for InvoiceNumber(s): " + _invoicNum.Value);

                            NameValueCollection transactionDetails = AuthorizeNetCreditCard.GetResponseFields(response);
                            SetApplicationOrderCart();

                            if (response.resultCode == MessageTypeEnum.Ok)
                            {
                                Session["IsNeedToSendOrderConfirmationDocument"] = true.ToString();//UAT-2970

                                Presenter.SaveTransactionDetails(_invoicNum.Value, transactionDetails);
                                // Need to keep it here so that Order is fetched after the Above Save is done.
                                Entity.ClientEntity.Order order = Presenter.GetOrderById(_applicantOrderCart.lstApplicantOrder[0].OrderId);

                                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 5: Result Code from Authorize.Net is:" + response.resultCode + ", redirecting to "
                                 + " Order Confirmation, for InvoiceNumber(s): " + _invoicNum.Value);
                                IsRedirectToSummaryPage = true;
                                // RedirectToOrderConfirmation(order);
                            }
                            else
                            {
                                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 5: Result Code from Authorize.Net is:" + response.resultCode + " for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);

                                _applicantOrderCart.IsPaymentResponsePending = false;
                                Entity.ClientEntity.Order order = Presenter.GetOrderById(_applicantOrderCart.lstApplicantOrder[0].OrderId);

                                var _creditCartOptionCode = PaymentOptions.Credit_Card.GetStringValue();
                                var _ccOPD = order.OrderPaymentDetails.Where(opd => opd.lkpPaymentOption.Code == _creditCartOptionCode && opd.OnlinePaymentTransaction.Invoice_num == _invoicNum.Value).First();

                                CurrentViewContext.OPDId = _ccOPD.OPD_ID;
                                Presenter.UpdateOPDStatus();

                                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 6: Order Status changed to 'Online_Payment_Not_Completed', for InvoiceNumber(s): "
                                    + CurrentViewContext.InvoiceNumber + ", with failure reason as: " + transactionDetails["x_response_reason_text"]);

                                lblMessage.Visible = true;
                                String errorMessage = Resources.Language.PYMNTFORORDNUM + " " + strOrderId + " " + Resources.Language.FAILEDDUETORSN + ": " + transactionDetails["x_response_reason_text"];
                                lblMessage.ShowMessage(errorMessage, MessageType.Error);
                                UpdateApplicantOrderCart(_applicantOrderCart);
                            }
                        }

                    }

                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.ShowMessage("Duplicate payment attempt detected. If you are not redirected to order confirmation page shortly, please go to order history page to check the status of your order.", MessageType.Error);
                        BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 4: Duplicate payment attempt detected for InvoiceNumber(s): " + _invoicNum.Value);
                    }
                }
                if (IsRedirectToSummaryPage)
                {
                    String strOrderIDs = _applicantOrderCart.AllOrderIDs;
                    if (strOrderIDs.IsNullOrEmpty())
                        strOrderIDs = Convert.ToString(_applicantOrderCart.lstApplicantOrder[0].OrderId);

                    List<Entity.ClientEntity.Order> orderList = Presenter.GetListofOrdersForOrderID(strOrderIDs);
                    RedirectToOrderConfirmation(orderList[0]);
                }
            }

            else
            {
                lblMessage.Visible = true;
                lblMessage.ShowMessage(Resources.Language.PLZSELCRDTCRD, MessageType.Information);
            }
        }

        /// <summary>
        /// Method to set Billing Information for organization user.
        /// </summary>
        private void SetBillingInformation()
        {
            //if (IsBiilingInfoSameAsAccountInfo())
            //{
            _applicantOrderCart = GetApplicantOrderCart();

            RedirectIfIncorrectOrderStage(_applicantOrderCart);

            if (_applicantOrderCart.IsNotNull())
            {
                #region NEW ORDER || CHANGE SUBSCRIPTION
                //Fetch Account info from session(from OrganizationUserProfile) in case of NEW ORDER or CHANGE SUBSCRIPTION
                if ((_applicantOrderCart.IsAccountUpdated))
                {
                    OrganizationUserProfile orgUserProfile;
                    //PreviousAddressContract applicantAddress;

                    if (_applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.IsNotNull())
                    {
                        orgUserProfile = _applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile;

                        if (!String.IsNullOrEmpty(orgUserProfile.FirstName))
                        {
                            CurrentViewContext.FirstName = orgUserProfile.FirstName;
                        }
                        if (!String.IsNullOrEmpty(orgUserProfile.LastName))
                        {
                            CurrentViewContext.LastName = orgUserProfile.LastName;
                        }
                        if (!String.IsNullOrEmpty(orgUserProfile.PhoneNumber))
                        {
                            CurrentViewContext.Phone = orgUserProfile.PhoneNumber;
                        }
                        if (!String.IsNullOrEmpty(orgUserProfile.PrimaryEmailAddress))
                        {
                            CurrentViewContext.Email = orgUserProfile.PrimaryEmailAddress;
                        }
                    }

                    if (_applicantOrderCart.lstPrevAddresses.IsNotNull() && _applicantOrderCart.lstPrevAddresses.Count > 0)
                    {
                        foreach (PreviousAddressContract applicantAddress in _applicantOrderCart.lstPrevAddresses)
                        {
                            if (applicantAddress.IsNotNull() && applicantAddress.isCurrent)
                            {
                                if (!String.IsNullOrEmpty(applicantAddress.Address1))
                                {
                                    CurrentViewContext.Address = applicantAddress.Address1;
                                }
                                if (!String.IsNullOrEmpty(applicantAddress.Address2))
                                {
                                    CurrentViewContext.Address = " " + applicantAddress.Address2;
                                }
                                if (!String.IsNullOrEmpty(applicantAddress.CityName))
                                {
                                    CurrentViewContext.City = applicantAddress.CityName;
                                }
                                if (!String.IsNullOrEmpty(applicantAddress.StateName))
                                {
                                    CurrentViewContext.State = applicantAddress.StateName;
                                }
                                if (!String.IsNullOrEmpty(applicantAddress.Zipcode))
                                {
                                    CurrentViewContext.Zip = applicantAddress.Zipcode;
                                }
                                if (!String.IsNullOrEmpty(applicantAddress.Country))
                                {
                                    CurrentViewContext.Country = applicantAddress.Country;
                                }
                                break;
                            }
                        }
                        CurrentViewContext.Fax = String.Empty; //Currently Applicant's Fax is not stored in DB.
                        CurrentViewContext.Company = String.Empty; //Currently Applicant's company is not stored in DB.
                    }
                }

                #endregion

                #region RUSH ORDER || BALANCE PAYMENT

                //Fetch Account info from OrganizationUser table in case of RUSH ORDER or BALANCE PAYMENT
                else if (!_applicantOrderCart.IsAccountUpdated || _applicantOrderCart.IsRushOrder || _applicantOrderCart.IsBalancePayment)
                {
                    Presenter.getOrganizationUserDetails(CurrentLoggedInUserId);

                    if (CurrentViewContext.OrganizationUserData != null)
                    {
                        Entity.OrganizationUser orgUser = CurrentViewContext.OrganizationUserData;

                        if (!String.IsNullOrEmpty(orgUser.FirstName))
                        {
                            CurrentViewContext.FirstName = orgUser.FirstName;
                        }
                        if (!String.IsNullOrEmpty(orgUser.LastName))
                        {
                            CurrentViewContext.LastName = orgUser.LastName;
                        }
                        if (!String.IsNullOrEmpty(orgUser.PhoneNumber))
                        {
                            CurrentViewContext.Phone = orgUser.PhoneNumber;
                        }
                        if (!String.IsNullOrEmpty(orgUser.PrimaryEmailAddress))
                        {
                            CurrentViewContext.Email = orgUser.PrimaryEmailAddress;
                        }

                        if (CurrentViewContext.OrganizationUserData.AddressHandle != null
                               && CurrentViewContext.OrganizationUserData.AddressHandle.Addresses != null
                               && CurrentViewContext.OrganizationUserData.AddressHandle.Addresses.Count > 0)
                        {
                            Entity.Address address = CurrentViewContext.OrganizationUserData.AddressHandle.Addresses.ToList()[0];

                            if (!String.IsNullOrEmpty(address.Address1))
                            {
                                CurrentViewContext.Address = address.Address1;
                            }
                            if (!String.IsNullOrEmpty(address.Address2))
                            {
                                CurrentViewContext.Address = " " + address.Address2;
                            }
                            if (!String.IsNullOrEmpty(address.ZipCode.ZipCode1))
                            {
                                CurrentViewContext.Zip = address.ZipCode.ZipCode1;
                            }
                            if (!String.IsNullOrEmpty(address.ZipCode.City.CityName))
                            {
                                CurrentViewContext.City = address.ZipCode.City.CityName;
                            }
                            if (!String.IsNullOrEmpty(address.ZipCode.City.State.StateName))
                            {
                                CurrentViewContext.State = address.ZipCode.City.State.StateName;
                            }
                            if (!String.IsNullOrEmpty(address.ZipCode.City.State.Country.CompleteName))
                            {
                                CurrentViewContext.Country = address.ZipCode.City.State.Country.CompleteName;
                            }
                        }
                    }
                }

                #endregion
            }
            //}
            //else
            //{
            //    CurrentViewContext.FirstName = String.Empty;
            //    CurrentViewContext.LastName = String.Empty;
            //    CurrentViewContext.Phone = String.Empty;
            //    CurrentViewContext.Email = String.Empty;
            //    CurrentViewContext.Address = String.Empty;
            //    CurrentViewContext.City = String.Empty;
            //    CurrentViewContext.State = String.Empty;
            //    CurrentViewContext.Zip = String.Empty;
            //    CurrentViewContext.Country = String.Empty;
            //    CurrentViewContext.Fax = String.Empty;
            //    CurrentViewContext.Company = String.Empty;
            //}
        }

        private Boolean IsBiilingInfoSameAsAccountInfo()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            return _applicantOrderCart.IsBiilingInfoSameAsAccountInfo;
        }

        private void SetApplicationOrderCart()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            //RedirectIfIncorrectOrderStage(_applicantOrderCart);
            //_applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlinePaymentSubmission);
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ONLINE_PAYMENT_SUBMITTED, _applicantOrderCart);
            StringBuilder _sbInfo = new StringBuilder();
        }

        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (_applicantOrderCart.IsNull())
            {
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return _applicantOrderCart;
        }

        /// <summary>
        /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void RedirectIfIncorrectOrderStage(ApplicantOrderCart applicantOrderCart)
        {
            Presenter.GetNextPagePathByOrderStageID(applicantOrderCart);

            //Redirect to next page path if Order Status track is not correct.
            if (CurrentViewContext.NextPagePath.IsNotNull())
            {
                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - RedirectIfIncorrectOrderStage() executed, Redirecting to " + CurrentViewContext.NextPagePath + " for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);
                Response.Redirect(CurrentViewContext.NextPagePath);
            }
            else
            {
                applicantOrderCart.AddOrderStageTrackID(OrderStages.CIMAccountSelection);
            }
        }

        private void RedirectToOrderConfirmation(Entity.ClientEntity.Order order)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            Int32? rushOrderStatusID = order.RushOrderStatusID;
            Boolean isRushOrderForExistingOrder = order.IsRushOrderForExistingOrder.HasValue ? order.IsRushOrderForExistingOrder.Value : false;

            if (_applicantOrderCart.IsModifyShipping == true)
            {
                _applicantOrderCart.TenantId = CurrentViewContext.TenantId;
                if (_applicantOrderCart.IsPaymentReqInMdfyShpng)
                {
                    SecurityManager.UpdateOrderPayment(_applicantOrderCart, _applicantOrderCart.IsLocationServiceTenant, CurrentLoggedInUserId);
                }
                SecurityManager.UpdateApplicantAppointmentDetailExt(_applicantOrderCart, _applicantOrderCart.IsLocationServiceTenant, CurrentLoggedInUserId);
                CommunicationManager.SendModifyShippingNotification(CurrentViewContext.OrganizationUserData, CurrentViewContext.TenantId, CurrentViewContext.OrderIds.FirstOrDefault(), order.OrderNumber, _applicantOrderCart.MailingAddress);
                // Code to Save Order Payment Invoice Data in Modify Shipping order flow 
                // UAT-5031 : Start
                //bool result = Presenter.SaveOrderPaymentInvoice(CurrentViewContext.TenantId, _applicantOrderCart.OrderId, CurrentViewContext.CurrentLoggedInUserId, _applicantOrderCart.IsModifyShipping);
                if (!_applicantOrderCart.IsPaymentReqInMdfyShpng)
                {
                    bool result = ComplianceDataManager.SaveOrderPaymentInvoice(CurrentViewContext.TenantId, _applicantOrderCart.OrderId, CurrentViewContext.CurrentLoggedInUserId, _applicantOrderCart.IsModifyShipping);
                }
                    // UAT-5031 : End

                queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.CHILD,  @"~/ComplianceOperations/UserControl/ModifyShippingConfirmation.ascx"}
                                                                 };
                Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                return;
            }

            var _isRushOrderPaid = GetRushOrderOPD(order);

            //if (rushOrderStatusID.IsNotNull() && !order.lkpOrderStatu1.Code.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()) && isRushOrderForExistingOrder)
            if (rushOrderStatusID.IsNotNull() && isRushOrderForExistingOrder && _isRushOrderPaid)
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.CHILD,  ChildControls.ApplicantRushOrderConfirmPage}
                                                                 };
            }
            else
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.CHILD,  ChildControls.ApplicantOrderConfirmation}
                                                                 };
            }
            //if (_applicantOrderCart != null
            //        && _applicantOrderCart.IsLocationServiceTenant)
            //{
            //    Server.Transfer(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            //}
            //else
            //{
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            //}
        }

        private Boolean GetRushOrderOPD(Entity.ClientEntity.Order ord)
        {
            var _lstOPD = ord.OrderPaymentDetails;
            var _isRushOrderPaid = false;
            foreach (var opd in _lstOPD)
            {
                var _oppdRushOrder = opd.OrderPkgPaymentDetails.Where(oppd => oppd.OPPD_BkgOrderPackageID.IsNull()
                    && oppd.OPPD_IsDeleted == false
                      && oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue()).FirstOrDefault();

                if (_oppdRushOrder.IsNotNull() && opd.lkpOrderStatu.Code != ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()
                                               && opd.lkpOrderStatu.Code != ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue())
                {
                    _isRushOrderPaid = true;
                    break;
                }
            }
            return _isRushOrderPaid;
        }

        /// <summary>
        /// Update the Applicant Order cart with updated value of 'IsPaymentResponsePending', 
        /// so that next subsequent request gets updated value from session, when double click is used.
        /// </summary>
        /// <param name="OrderCart"></param>
        private void UpdateApplicantOrderCart(ApplicantOrderCart OrderCart)
        {
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, OrderCart);
        }

        [WebMethod]
        public static string GetToken(long customerProfileId)
        {

            if (customerProfileId <= 0)
            {
                return "";
            }

            //todo: Get URL from config file 
            //hostedProfileIFrameCommunicatorUrl = @"https://dev.adbhome.com/Resources/Mod/CIM/IframeCommunicator.html";
            String hostedProfileIFrameCommunicatorUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + "/Resources/Mod/CIM/IframeCommunicator.html";
            return AuthorizeNetCreditCard.GetToken(customerProfileId, hostedProfileIFrameCommunicatorUrl);
        }

        #region UAT-3077
        private void ItemPaymentSubmission(long paymentProfileId)
        {
            ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as ItemPaymentContract;
            if (CurrentViewContext.IsInstructorPreceptorPackage)
                itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
            //  Boolean paymentResponsePending = false;
            String strOrderIDs = strOrderId;
            Boolean isSchoolApprovalRequired = Presenter.IsSchoolApprovalRequired(strOrderIDs);
            String creditCartPaymentCode = "PTCC";
            CreateCustomerProfileTransactionResponseType response = AuthorizeNetCreditCard.CreateCustomerProfileTransaction(CustomerProfileId, paymentProfileId,
                                                                                                                              CurrentLoggedInUserId, itemPaymentContract.TotalPrice,
                                                                                                                              CurrentViewContext.InvoiceNumber, itemPaymentContract.ItemName);
            BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 4: Method 'AuthorizeNetCreditCard.CreateCustomerProfileTransaction' executed successfully"
                    + " for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);

            NameValueCollection transactionDetails = AuthorizeNetCreditCard.GetResponseFields(response);
            if (response.resultCode == MessageTypeEnum.Ok)
            {
                //Session["IsNeedToSendOrderConfirmationDocument"] = true.ToString();//UAT-2970

                Presenter.SaveTransactionDetails(CurrentViewContext.InvoiceNumber, transactionDetails);
                // Need to keep it here so that Order is fetched after the Above Save is done.
                Entity.ClientEntity.Order order = Presenter.GetOrderById(Convert.ToInt32(strOrderIDs));

                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 5: Result Code from Authorize.Net is:" + response.resultCode + ", redirecting to "
                 + " Order Confirmation, for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);

                //Approve payment item after successfully payment.
                itemPaymentContract.IsPaid = true;
                itemPaymentContract.orderID = Convert.ToInt32(strOrderIDs);
                Presenter.ApprovePaymentItem(itemPaymentContract);
                if (itemPaymentContract.IsRequirementPackage)
                {
                    RequirementVerificationManager.SyncRequirementVerificationToFlatData(Convert.ToString(itemPaymentContract.PkgSubscriptionId), CurrentViewContext.TenantId, CurrentViewContext.CurrentLoggedInUserId);
                    var rotMovementserviceResponse = ClinicalRotationManager.PerformRotationLiveDataMovement(CurrentViewContext.TenantId, itemPaymentContract.PkgSubscriptionId, itemPaymentContract.CategoryID, CurrentViewContext.CurrentLoggedInUserId); //UAT 3164

                    //UAT-3805
                    IEnumerable<DataRow> rows = rotMovementserviceResponse.AsEnumerable();
                    List<INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation.ApplicantRequirementParameterContract> rotData = new List<INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation.ApplicantRequirementParameterContract>();

                    rotData.AddRange(rows.Select(col => new INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation.ApplicantRequirementParameterContract
                    {
                        RequirementPkgSubscriptionId = col["RPSID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RPSID"]),
                        RequirementCategoryId = col["ReqCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqCategoryID"]),
                        prevCategoryStatusCode = col["OldCategoryStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["OldCategoryStatus"]),
                        NewCategoryStatusCode = col["NewCategoryStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["NewCategoryStatus"]),
                        AppRequirementItemDataID = col["ApplicantRequirementItemDataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ApplicantRequirementItemDataID"]),
                        RequirementItemId = col["ReqItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqItemID"]),
                    }).ToList());

                    rotData.DistinctBy(cond => cond.RequirementPkgSubscriptionId).ForEach(row =>
                    {
                        List<Int32> categoryIds = rotData.Where(cond => cond.RequirementPkgSubscriptionId == row.RequirementPkgSubscriptionId
                                                                        && cond.prevCategoryStatusCode != RequirementCategoryStatus.APPROVED.GetStringValue())
                                                         .Select(s => s.RequirementCategoryId).Distinct().ToList();

                        if (!categoryIds.IsNullOrEmpty())
                        {
                            INTSOF.UI.Contract.ProfileSharing.ItemDocNotificationRequestDataContract itemDocRequestData = new INTSOF.UI.Contract.ProfileSharing.ItemDocNotificationRequestDataContract();
                            List<INTSOF.UI.Contract.ProfileSharing.ItemDocNotificationRequestDataContract> itemDocNotificationRequestData = new List<INTSOF.UI.Contract.ProfileSharing.ItemDocNotificationRequestDataContract>();

                            String categoryIDs = String.Join(",", categoryIds);

                            itemDocRequestData.TenantID = CurrentViewContext.TenantId;
                            itemDocRequestData.CategoryIds = categoryIDs;
                            // itemDocRequestData.ApplicantOrgUserID = View.ApplicantId;
                            itemDocRequestData.ApprovedCategoryIds = String.Empty;
                            itemDocRequestData.RequestTypeCode = lkpUseTypeEnum.ROTATION.GetStringValue();
                            itemDocRequestData.PackageSubscriptionID = null;
                            itemDocRequestData.RPS_ID = row.RequirementPkgSubscriptionId;
                            itemDocRequestData.CurrentLoggedInUserID = CurrentViewContext.CurrentLoggedInUserId;
                            itemDocNotificationRequestData.Add(itemDocRequestData);

                            //UAT-3805
                            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                            Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                            dicParam.Add("CategoryData", itemDocNotificationRequestData);
                            ProfileSharingManager.RunParallelTaskItemDocNotificationOnCatApproval(dicParam, LoggerService, ExceptiomService);
                        }
                    });
                }
                RedirectToItemPaymentConfirmation(order);
            }
            else
            {
                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 5: Result Code from Authorize.Net is:" + response.resultCode + " for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);

                //  _applicantOrderCart.IsPaymentResponsePending = false;
                Entity.ClientEntity.Order order = Presenter.GetOrderById(Convert.ToInt32(strOrderIDs));


                var _ccOPD = order.OrderPaymentDetails.Where(opd => opd.lkpPaymentOption.Code == creditCartPaymentCode).First();

                CurrentViewContext.OPDId = _ccOPD.OPD_ID;
                Presenter.UpdateOPDStatus();

                BaseWebPage.LogOrderFlowSteps("CIMAccountSelection.aspx - STEP 6: Order Status changed to 'Online_Payment_Not_Completed', for InvoiceNumber(s): "
                    + CurrentViewContext.InvoiceNumber + ", with failure reason as: " + transactionDetails["x_response_reason_text"]);

                lblMessage.Visible = true;
                String errorMessage = Resources.Language.PYMNTFORORDNUM + " " + strOrderId + " " + Resources.Language.FAILEDDUETORSN + ": " + transactionDetails["x_response_reason_text"];
                lblMessage.ShowMessage(errorMessage, MessageType.Error);
                throw new Exception(errorMessage);
            }

        }
        private void RedirectToItemPaymentConfirmation(Entity.ClientEntity.Order order)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            if (CurrentViewContext.IsInstructorPreceptorPackage)
            {

                ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;
                itemPaymentContract.OrderNumber = order.OrderNumber;
                Session.Remove(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART);
                itemPaymentContract.IsPaid = true;
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART, itemPaymentContract);
            }
            else
            {
                ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as ItemPaymentContract;
                itemPaymentContract.OrderNumber = order.OrderNumber;
                Session.Remove(ResourceConst.APPLICANT_PARKING_CART);
                itemPaymentContract.IsPaid = true;
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_PARKING_CART, itemPaymentContract);
            }
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    //{AppConsts.CHILD,  ChildControls.ItemPaymentConfirmation},
                                                                     { "IsInstructorPreceptorPackage" ,CurrentViewContext.IsInstructorPreceptorPackage.ToString()},
                                                                     { "SelectedTenantID" ,CurrentViewContext.TenantId.ToString()}
                                                                 };
            // Response.Redirect(String.Format("~/ComplianceOperations/Pages/ItemPaymentConfirmationPopUp.aspx"));
            Response.Redirect(String.Format("~/ComplianceOperations/Pages/ItemPaymentConfirmationPopUp.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }

        private void BindBillingInformationForItemPayment()
        {
            OrganizationUserProfile _orgUserProfile = new OrganizationUserProfile();
            ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as ItemPaymentContract;
            if (CurrentViewContext.IsInstructorPreceptorPackage)
                itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.INSTRUCTOR_PRECEPTOR_PARKING_CART) as ItemPaymentContract;

            if (itemPaymentContract.TenantID > AppConsts.NONE)
            {
                CurrentViewContext.TenantId = itemPaymentContract.TenantID;
            }
            _orgUserProfile = Presenter.GetOrganizationUserProfileByOrganizationUserProfileID(itemPaymentContract.OrganizationUserProfileID);
            if (!_orgUserProfile.IsNullOrEmpty())
            {
                if (!String.IsNullOrEmpty(_orgUserProfile.FirstName))
                {
                    CurrentViewContext.FirstName = _orgUserProfile.FirstName;
                }
                if (!String.IsNullOrEmpty(_orgUserProfile.LastName))
                {
                    CurrentViewContext.LastName = _orgUserProfile.LastName;
                }
                if (!String.IsNullOrEmpty(_orgUserProfile.PhoneNumber))
                {
                    CurrentViewContext.Phone = _orgUserProfile.PhoneNumber;
                }
                if (!String.IsNullOrEmpty(_orgUserProfile.PrimaryEmailAddress))
                {
                    CurrentViewContext.Email = _orgUserProfile.PrimaryEmailAddress;
                }

            }
        }
        #endregion

        #region CBI|| CABS

        private void SkipSubmitClick()
        {
            if (_applicantOrderCart != null
                && _applicantOrderCart.IsLocationServiceTenant)
            {
                List<PaymentProfileDetail> lstNewPaymentProfileDetails = new List<PaymentProfileDetail>();
                if (!CustomerProfileId.IsNullOrEmpty())
                    lstNewPaymentProfileDetails = AuthorizeNetCreditCard.GetCustomerPaymentProfiles(CustomerProfileId);
                if (_applicantOrderCart.lstOldCustomerPaymentProfileId.IsNullOrEmpty())
                    _applicantOrderCart.lstOldCustomerPaymentProfileId = new List<long>();

                if (lstNewPaymentProfileDetails.Count > _applicantOrderCart.lstOldCustomerPaymentProfileId.Count)
                {
                    foreach (PaymentProfileDetail NewPaymentProfileDetail in lstNewPaymentProfileDetails)
                    {
                        if (!_applicantOrderCart.lstOldCustomerPaymentProfileId.Contains(NewPaymentProfileDetail.CustomerPaymentProfileId))
                        {
                            CurrentViewContext.DefaultpaymentProfileId = NewPaymentProfileDetail.CustomerPaymentProfileId;
                            btnSubmitPayment_Click(btnSubmitPayment, null);
                        }
                    }
                }
                
            }
        }

        private void HandleSubmitClickOnDeletion()
        {
            if (_applicantOrderCart != null
                && _applicantOrderCart.IsLocationServiceTenant)
            {
                List<PaymentProfileDetail> lstAvailablePaymentProfileDetails = AuthorizeNetCreditCard.GetCustomerPaymentProfiles(CustomerProfileId);
                if (lstAvailablePaymentProfileDetails.IsNullOrEmpty() || lstAvailablePaymentProfileDetails.Count == AppConsts.NONE)
                {
                    //_applicantOrderCart.IsSkipSubmitApplicable = true;
                    _applicantOrderCart.lstOldCustomerPaymentProfileId = new List<Int64>();
                }
            }
        }

        #endregion

        protected void grdPaymentProfiles_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    if (_applicantOrderCart != null
                        && _applicantOrderCart.IsLocationServiceTenant
                        && _applicantOrderCart.lstOldCustomerPaymentProfileId != null && _applicantOrderCart.lstOldCustomerPaymentProfileId.Count == AppConsts.ONE)
                    {
                        RadioButton rdbCardNumber = e.Item.FindControl("rdbCardNumber") as RadioButton;
                        HiddenField hdnCustomerPaymentProfileId = e.Item.FindControl("hdnCustomerPaymentProfileId") as HiddenField;
                        if (!String.IsNullOrEmpty(hdnCustomerPaymentProfileId.Value) && _applicantOrderCart.lstOldCustomerPaymentProfileId.Contains(Convert.ToInt64(hdnCustomerPaymentProfileId.Value)))
                        {
                            rdbCardNumber.Checked = true;
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblMessage.Visible = true;
                lblMessage.ShowMessage(ex.Message, MessageType.Error);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblMessage.Visible = true;
                lblMessage.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void GoToDashboard_Click(object sender, EventArgs e)
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            String url = String.Format(AppConsts.APPLICANT_MAIN_PAGE_NAME);

            // BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 5: 'Finish' clicked, Redirecitng to dashboard wih url: " + url + ", for OrderId(s): " + lblOrderId.Text);
            Response.Redirect(url);
        }
    }
}