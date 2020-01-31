#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections.Specialized;
using Microsoft.Practices.ObjectBuilder;
using System.Net;
using System.IO;

#endregion

#region UserDefined

using INTSOF.Utils;
using NLog;
using System.Configuration;

#endregion

#endregion

namespace CoreWeb.Shell.Views
{
    public partial class RedirectPaypal : System.Web.UI.Page, IRedirectPaypalView
    {
        #region Variables

        #region Private Variables

        private RedirectPaypalPresenter _presenter = new RedirectPaypalPresenter();

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

        public IRedirectPaypalView CurrentViewContext
        {
            get { return this; }
        }

        public List<Entity.PaymentIntegrationSetting> PaymentIntegrationSettings
        {
            get;
            set;
        }

        public String InvoiceNumber
        {
            get;
            set;
        }

        public String PaypalPDTStatus
        {
            get;
            set;
        }

        public String McGross
        {
            get;
            set;
        }

        public NameValueCollection TransactionDetails
        {
            get;
            set;
        }


        public RedirectPaypalPresenter Presenter
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

        public String ApplicationURL
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
                ClearCache();
                if (!Page.IsPostBack)
                {
                    Presenter.OnViewInitialized();

                    if (CurrentViewContext.PaymentIntegrationSettings.IsNotNull())
                    {
                        TransactionDetails = new NameValueCollection();
                        GetResponse();
                        Presenter.GetURL();
                        //if (InvoiceNumber.IsNotNull() && Application[InvoiceNumber].IsNotNull())
                        if (ApplicationURL.IsNotNull())
                        {
                            Presenter.UpdateOnlineTransactionResults();
                            LogOrderFlowSteps("PaypalPaymentSubmission.aspx - STEP 2: Method 'Presenter.GetURL()' executed, 'ApplicationURL' for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber + "is: " + ApplicationURL);
                        }
                        else
                        {
                            litResponseMessage.Text = "Invalid Request.";
                            btnProceed.Visible = false;
                            lblRedirectPaypalTitle.Visible = false;
                            LogOrderFlowSteps("PaypalPaymentSubmission.aspx - STEP 2: Method 'Presenter.GetURL()' executed, 'ApplicationURL' for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber + ", is NULL.");
                        }
                        SetReturnURL();
                    }
                }
            }
            catch (Exception ex)
            {
                LogOrderFlowSteps("RedirectPaypal.aspx - STEP 1: Exception on Page_Load for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber + ". Exception is: " + Convert.ToString(ex));
                //ErrorLog logFile = new ErrorLog("Problem in sending data from RedirectPaypal page." + ex);
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

        private void GetResponse()
        {
            String command = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("Command-NotifyAndSynch")).NameValue;
            String authenticationToken = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("PDTIdentityToken")).NameValue;
            //Transaction ID is got in query string in QA environment.
            String transactionToken = Request.QueryString.Get("tx");

            //Transaction ID is got in POST in Dev environment.
            if (transactionToken.IsNullOrEmpty())
            {
                transactionToken = Request.Form["txn_id"];
            }
            String query = "cmd=" + command + "&tx=" + transactionToken + "&at=" + authenticationToken;
            String paypalPostUrl = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("PaypalPostUrl")).NameValue;

            if (ConfigurationManager.AppSettings[AppConsts.APP_SETTING_USE_PAYPAL_TLS_1_2].IsNotNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_USE_PAYPAL_TLS_1_2]))
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalPostUrl);

            //Set values for the request back
            req.Method = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("Method")).NameValue;
            req.ContentType = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("ContentType")).NameValue;
            req.ContentLength = query.Length;

            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(query);
            streamOut.Close();
            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            String strResponse = streamIn.ReadToEnd();
            streamIn.Close();
            Dictionary<String, String> results = new Dictionary<String, String>();

            if (strResponse != "")
            {
                StringReader reader = new StringReader(strResponse);
                String line = PaypalPDTStatus = reader.ReadLine();
                GetTransactionDetails(strResponse);
                TransactionDetails.Add("PaypalPDTStatus", line);

                if (line == "SUCCESS")
                {
                    LogOrderFlowSteps("PaypalPaymentSubmission.aspx - STEP 1: " + line + " response is received from Paypal, on RedirectPaypal.aspx for Transaction ID: "
                                     + transactionToken
                                     + ", and InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);

                    //ErrorLog logFile = new ErrorLog(line + " response is got from Paypal on RedirectPaypal page for Transaction ID" + transactionToken + ".</br>" + strResponse);
                }
                else
                {
                    LogOrderFlowSteps("PaypalPaymentSubmission.aspx - STEP 1: " + line + " response is received from Paypal, on RedirectPaypal.aspx for Transaction ID: "
                                        + transactionToken
                                        + " and InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber
                                        + ". Response is: </br>" + strResponse);
                    // Log for manual investigation
                    // ErrorLog logFile = new ErrorLog(line + " response is got from Paypal on RedirectPaypal page for Transaction ID" + transactionToken + ".</br>" + strResponse);
                }
                litResponseMessage.Text = line;
            }
            else
            {
                //unknown error
                litResponseMessage.Text = "Transaction was not successful. Please restart the order.";
                LogOrderFlowSteps("PaypalPaymentSubmission.aspx - STEP 1: NO response is received from Paypal, on RedirectPaypal.aspx for Transaction ID: "
                                        + transactionToken
                                        + " and InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber);
                //ErrorLog logFile = new ErrorLog("No response is got from Paypal on RedirectPaypal page for Transaction ID" + transactionToken + ".");
            }
        }

        private void GetTransactionDetails(String strResponse)
        {
            String[] responseArray = strResponse.Split('\n');

            foreach (var response in responseArray)
            {
                String[] dataArray = response.Split('=');

                if (dataArray.Count() == 2)
                {
                    TransactionDetails.Add(dataArray[0], dataArray[1]);

                    if (dataArray[0].TextEquals("invoice"))
                    {
                        InvoiceNumber = dataArray[1];
                    }
                    else if (dataArray[0].TextEquals("mc_gross"))
                    {
                        McGross = dataArray[1];
                    }
                }
            }
        }

        private void SetReturnURL()
        {
            //String url = Convert.ToString(Application[InvoiceNumber]);
            String url = ApplicationURL;

            if (!url.ToLower().StartsWith("http"))
                url = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + url;

            //Application[InvoiceNumber] = null;
            Presenter.RemoveWebApplicationData();
            this.Form.Action = String.Concat(url, "/Default.aspx");

            LogOrderFlowSteps("PaypalPaymentSubmission.aspx - STEP 2: Method 'Presenter.RemoveWebApplicationData()' executed successfully for InvoiceNumber(s): " + CurrentViewContext.InvoiceNumber
                + ". Form Action url set to: " + this.Form.Action);

            //ErrorLog logFile = new ErrorLog("Data is sent from RedirectPaypal page for InvoiceNumber " + InvoiceNumber + ".");
        }

        private void ClearCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.Now);
            //Response.Cache.SetNoServerCaching();
            //Response.Cache.SetNoStore();


            Response.Cache.SetRevalidation(HttpCacheRevalidation.ProxyCaches);
            Response.Cache.SetExpires(DateTime.Now.AddYears(-5));
            Response.Cache.AppendCacheExtension("private");
            Response.Cache.AppendCacheExtension("no-cache=Set-Cookie");
            Response.Cache.SetProxyMaxAge(TimeSpan.Zero);

            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
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


