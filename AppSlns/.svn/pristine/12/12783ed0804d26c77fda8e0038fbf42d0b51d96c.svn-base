#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;

#endregion

#region UserDefined

using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using NLog;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class PaypalPaymentSubmission : System.Web.UI.Page, IPaypalPaymentSubmissionView
    {
        #region Variables

        #region Private Variables

        private PaypalPaymentSubmissionPresenter _presenter = new PaypalPaymentSubmissionPresenter();

        /// <summary>
        /// Logger instance to log the Order flow steps
        /// </summary>
        private static Logger _orderFlowlogger;

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public PaypalPaymentSubmissionPresenter Presenter
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

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
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

        public IPaypalPaymentSubmissionView CurrentViewContext
        {
            get { return this; }
        }

        public List<Entity.PaymentIntegrationSetting> PaymentIntegrationSettings
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

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Dictionary<String, String> args = new Dictionary<String, String>();

                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        CurrentViewContext.InvoiceNumber = args.GetValue("invnum");
                    }
                    else
                        return;

                    Presenter.OnViewInitialized();
                    LogOrderFlowSteps("PaypalPaymentSubmission.aspx - STEP 1: Payment gateway settings fetched successfully, Page loaded for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);
                }

                if (!IsPostBack && CurrentViewContext.OnlinePaymentTransactionDetails.IsNotNull() && CurrentViewContext.PaymentIntegrationSettings.IsNotNull())
                {
                    SetApplicationOrderCart();
                    GetSettings();
                    LogOrderFlowSteps("PaypalPaymentSubmission.aspx - STEP 2: Payment gateway settings assigned to the html controls, for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);
                    //ErrorLog logFile = new ErrorLog("Data is sent from PaypalPaymentSubmission page for InvoiceNumber " + CurrentViewContext.InvoiceNumber + ".");
                }
                else
                {
                    LogOrderFlowSteps("PaypalPaymentSubmission.aspx - STEP 2: Payment gateway settings NOT assigned to the html controls, for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber
                        + "; CurrentViewContext.OnlinePaymentTransactionDetails:" + (CurrentViewContext.OnlinePaymentTransactionDetails.IsNotNull() ? "Not Null" : "Null")
                        + "; CurrentViewContext.PaymentIntegrationSettings:" + (CurrentViewContext.PaymentIntegrationSettings.IsNotNull() ? "Not Null" : "Null"));
                    return;
                }
            }
            catch (Exception ex)
            {
                LogOrderFlowSteps("PaypalPaymentSubmission.aspx - STEP 1: Exception on Page_Load for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber + ". Exception is: " + Convert.ToString(ex));
                //ErrorLog logFile = new ErrorLog("Problem in sending data from PaypalPaymentSubmission page" + ex);
            }
        }

        #endregion

        #region Grid Events



        #endregion

        #region Button Events



        #endregion

        #region DropDown Events



        #endregion

        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods

        private void GetSettings()
        {
            var _amount = CurrentViewContext.OnlinePaymentTransactionDetails
                     .OrderPaymentDetails
                     .Where(x => x.lkpPaymentOption.Code == PaymentOptions.Paypal.GetStringValue())
                     .Sum(opd => opd.OPD_Amount);

            frmPaypalPaymentSubmission.Action = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("PaypalPostUrl")).NameValue;
            cmd.Value = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("Command-BuyNow")).NameValue;
            business.Value = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("Business-MerchantEmailID")).NameValue;
            // amount.Value = Convert.ToString(decimal.Round(CurrentViewContext.OnlinePaymentTransactionDetails.Amount ?? 0, 2));
            amount.Value = Convert.ToString(decimal.Round(_amount ?? 0, 2));
            invoice.Value = CurrentViewContext.OnlinePaymentTransactionDetails.Invoice_num;
            //SuccessURL.Value = "http://182.71.23.117/Rediect/MainPage1.aspx";
            //FailedURL.Value = "http://182.71.23.117/Rediect/MainPage2.aspx";
            @return.Value = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("ReturnUrl")).NameValue;
            notify_url.Value = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("NotifyUrl")).NameValue;
            rm.Value = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("ReturnMethod")).NameValue;
        }

        private void SetApplicationOrderCart()
        {
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            RedirectIfIncorrectOrderStage(applicantOrderCart);
            applicantOrderCart.AddOrderStageTrackID(OrderStages.PaypalPaymentSubmission);
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
                LogOrderFlowSteps("PaypalPaymentSubmission.aspx - 'RedirectIfIncorrectOrderStage()' executed, Redirecting to " + CurrentViewContext.NextPagePath + " for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);
                Response.Redirect(CurrentViewContext.NextPagePath);
            }
        }

        /// <summary>
        /// Log the Order flow related information in different steps
        /// </summary>
        /// <param name="logMessage"></param>
        private static void LogOrderFlowSteps(String logMessage)
        {
            if (_orderFlowlogger == null)
            {
                _orderFlowlogger = LogManager.GetLogger(NLogLoggerTypes.ORDER_FLOW_LOGGER.GetStringValue());
            }
            _orderFlowlogger.Info(logMessage);
        }

        #endregion

        #endregion
    }
}



