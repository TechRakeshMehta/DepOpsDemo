#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Net;
using System.Web;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

#endregion

#region UserDefined

using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.Shell.Views
{
	public partial class RedirectIPNhandler : System.Web.UI.Page, IRedirectIPNhandlerView
	{
        #region Variables

        #region Private Variables

        private RedirectIPNhandlerPresenter _presenter=new RedirectIPNhandlerPresenter();

        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        
        public RedirectIPNhandlerPresenter Presenter
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

        public List<Entity.PaymentIntegrationSetting> PaymentIntegrationSettings
        {
            get;
            set;
        }

        public String IPNPostData
        {
            get;
            set;
        }

        public String IPNTransactionStatus
        {
            get;
            set;
        }

        public Dictionary<String, String> IPNPostDataKeyValue
        {
            get;
            set;
        }

        public Int32 TenantID
        {
            get;
            set;
        }

        public List<Int32> OrderIDs
        {
            get;
            set;
        }

        public IRedirectIPNhandlerView CurrentViewContext
        {
            get { return this; }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Presenter.OnViewInitialized();

                if (CurrentViewContext.PaymentIntegrationSettings.IsNotNull())
                {
                    GetIPNResponse();
                }
            }
            catch (Exception ex)
            {
                ErrorLog errorLog = new ErrorLog("Exception on RedirectIPNhandler page.", ex);
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

        private void GetIPNResponse()
        {
            String paypalPostUrl = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("PaypalPostUrl")).NameValue;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalPostUrl);

            //Set values for the request back
            req.Method = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("Method")).NameValue;
            req.ContentType = CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("ContentType")).NameValue;
            Byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            String strRequest = Encoding.ASCII.GetString(param);
            IPNPostData = strRequest;
            strRequest += "&" + CurrentViewContext.PaymentIntegrationSettings.FirstOrDefault(x => x.NameKey.Equals("Command-NotifyAndValidate")).NameValue; ;
            req.ContentLength = strRequest.Length;

            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();

            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            String strResponse = IPNTransactionStatus = streamIn.ReadToEnd();
            streamIn.Close();

            ErrorLog errorLogs = new ErrorLog("IPN hit RedirectIPNhandler page." + Environment.NewLine + Environment.NewLine + IPNPostData + Environment.NewLine +
                Environment.NewLine);

            //Saves the IPN Response and if IPN response attempt is more than 4 times then, set the status = 200 means no more IPN request is required.
            Boolean isIPNResponseRetryMoreThanFour = Presenter.SaveIPNResponse();
            Response.Clear();

            //Checks the IPS Response Status.
            if (strResponse == "VERIFIED")
            {
                Presenter.CheckAndUpdateOrderByIPN();
                ErrorLog errorLog = new ErrorLog("Data is verified on RedirectIPNhandler page.");
                Response.StatusCode = 200;
            }
            else if (strResponse == "INVALID")
            {
                Response.StatusCode = isIPNResponseRetryMoreThanFour == true ? 200 : 400;
                ErrorLog errorLog = new ErrorLog("Data is invalid on RedirectIPNhandler page.");
            }
            else
            {
                Response.StatusCode = isIPNResponseRetryMoreThanFour == true ? 200 : 400;
                //log response/ipn data for manual investigation
                ErrorLog errorLog = new ErrorLog("Data is neither verified nor invalid on RedirectIPNhandler page.");
            }
        }

        #endregion

        #endregion

    }
}

