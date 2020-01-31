using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using INTSOF.AuthNet.Business;

namespace INTSOF.AuthNet.Business
{
    public class PaymentManager : BaseAuthNetManager
    {

        //string relay_url = ConfigurationManager.AppSettings["Authorize.RelayURL"];
        //private string relay_url = Settings.RelayURL;
        private string _relay_url = String.Empty;

        //string amount = ConfigurationManager.AppSettings["HoldAmount"];
        private string amount = "0";

        //string description =  ConfigurationManager.AppSettings["Heading"];
        private string description = Settings.Heading;

        private String _isTestRequest = Settings.IsTestRequest;
        string loginURL;
        string type;

        public PaymentManager()
        {
            if (base.IsAuthNetInTestMode == true)
            {
                loginURL = base.AuthNetTestURL;
                description = "Sample Transaction - " + description;
            }
            else
                loginURL = base.AuthNetLiveURL;
        }

        public PaymentFormBuilder SubmitCard(String invoiceNumber, String amount, String relayUrl, Dictionary<String,String> billingInfo)
        {
            //ReportingGateway reportingGateway = new ReportingGateway(base.ApiLogin, base.ApiTransKey);
            _relay_url = relayUrl;
            string sIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(sIPAddress))
            {
                sIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }

            Random random = new Random();
            string sequence = (random.Next(0, 1000)).ToString();
            string timeStamp = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();

            if (!String.IsNullOrEmpty(amount)) //Test Mode
            {
                amount = Convert.ToString(amount);
            }

            string fingerprint = HMAC_MD5(base.ApiTransKey, base.ApiLogin + "^" + sequence + "^" + timeStamp + "^" + amount + "^");
            type = "POST";

            PaymentFormBuilder paymentFormBuilder = new PaymentFormBuilder();

            paymentFormBuilder.FormName = "form1";
            paymentFormBuilder.Method = type;
            paymentFormBuilder.Url = loginURL;
            paymentFormBuilder.Add("x_login", base.ApiLogin);
            paymentFormBuilder.Add("x_amount", amount);
            paymentFormBuilder.Add("x_description", (description));
            paymentFormBuilder.Add("x_invoice_num", (invoiceNumber));
            paymentFormBuilder.Add("x_fp_sequence", sequence);
            paymentFormBuilder.Add("x_fp_timestamp", timeStamp);
            paymentFormBuilder.Add("x_fp_hash", fingerprint);
            paymentFormBuilder.Add("x_type", "AUTH_ONLY");
            paymentFormBuilder.Add("x_version", "3.1");
            paymentFormBuilder.Add("x_email_customer", "FALSE");
            paymentFormBuilder.Add("x_RequestIP", sIPAddress);

            //Billing Information
            paymentFormBuilder.Add("x_first_name", billingInfo["x_first_name"]);
            paymentFormBuilder.Add("x_last_name", billingInfo["x_last_name"]);
            paymentFormBuilder.Add("x_company", billingInfo["x_company"]);
            paymentFormBuilder.Add("x_address", billingInfo["x_address"]);
            paymentFormBuilder.Add("x_city", billingInfo["x_city"]);
            paymentFormBuilder.Add("x_state", billingInfo["x_state"]);
            paymentFormBuilder.Add("x_zip", billingInfo["x_zip"]);
            paymentFormBuilder.Add("x_country", billingInfo["x_country"]);
            paymentFormBuilder.Add("x_email", billingInfo["x_email"]);
            paymentFormBuilder.Add("x_phone", billingInfo["x_phone"]);
            paymentFormBuilder.Add("x_fax",billingInfo["x_fax"]);
            
            //Set payment method to Credit Card only. Remove eCheck that is Bank payment method. 
            paymentFormBuilder.Add(ApiFields.Method, "CC");
            //paymentFormBuilder.Add("x_test_request", "FALSE");

            /*
            if (!country.IsNull())
            {
                paymentFormBuilder.Add("x_country", (country.CountryName.IsNull() ? "" : country.CountryName));
            }

            paymentFormBuilder.Add("x_phone", (customerProfile.Phone.IsNull() ? "" : customerProfile.Phone));
            */
            //if (base.IsAuthNetInTestMode == false)
            //{
            //    paymentFormBuilder.Add("x_email", "");
            //}
            //else
            //{
            //    //paymentFormBuilder.Add("x_email", ConfigurationManager.AppSettings["TestEmailAddress"]);
            //    paymentFormBuilder.Add("x_email", Settings.TestEmailAddress);

            //    //paymentFormBuilder.Add("postback_url", Extensions.GetApplicationDomainPath());
            //}

            if (!String.IsNullOrEmpty(amount))
            {
                //paymentFormBuilder.Add("x_test_request", "TRUE");
                paymentFormBuilder.Add("x_test_request", _isTestRequest);
            }

            paymentFormBuilder.Add("x_show_form", "PAYMENT_FORM");
            paymentFormBuilder.Add("x_relay_response", "TRUE");
            paymentFormBuilder.Add("x_relay_url", _relay_url);
            return paymentFormBuilder;
           
        }
      
        public bool VoidHold(string TransID)
        {

            try
            {
                VoidRequest voidRequest = new VoidRequest(TransID);
                var gateway = new Gateway(base.ApiLogin, base.ApiTransKey);
                gateway.Send(voidRequest);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool PlaceAuth(string CCNo, string ExpireMonthYear, decimal amount, string description)
        {

            try
            {
                AuthorizationRequest authorizationRequest = new AuthorizationRequest(CCNo, ExpireMonthYear, amount, description, false);
                var gateway = new Gateway(base.ApiLogin, base.ApiTransKey);
                gateway.Send(authorizationRequest);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CaptureAuth(decimal Amount, string TransID, string authCode)
        {

            try
            {
                CaptureRequest captureRequest = new CaptureRequest(Amount, TransID, authCode);
                var gateway = new Gateway(base.ApiLogin, base.ApiTransKey);
                gateway.Send(captureRequest);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreditAuth(string TransID, decimal Amount, string CardNumber)
        {

            try
            {
                CreditRequest creditRequest = new CreditRequest(TransID, Amount, CardNumber);
                var gateway = new Gateway(base.ApiLogin, base.ApiTransKey);
                gateway.Send(creditRequest);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /*
        public bool ChargeHold(decimal HoldAmount, string sTransId, string AuthCode,ref AuthNetRequest newAuthNetRequest,ref AuthNetResponse newAuthNetResponse)
        {
            try
            {
                type = "POST";
                newAuthNetRequest = new PaymentRepository().CreateAuthNetRequest(loginURL, base.ApiLogin, type, Convert.ToDecimal(HoldAmount), "", "", "", "", true, DateTime.Now, null);
                CaptureRequest captureRequest = new CaptureRequest(HoldAmount, sTransId, AuthCode);
                var gateway = new Gateway(base.ApiLogin, base.ApiTransKey);
                IGatewayResponse response = gateway.Send(captureRequest);
                newAuthNetResponse = new PaymentRepository().CreateAuthNetResponse(newAuthNetRequest.AuthNetRequestId, response.ResponseCode, "", "", "", response.AuthorizationCode, response.TransactionID, "", "", "", "", Convert.ToDecimal(response.Amount), DateTime.Now);                
                return response.Approved;
            }
            catch 
            {
                return false;
            }
        }

        public bool RevertCharge(decimal HoldAmount, string sTransId, string CardNumber, ref AuthNetRequest newAuthNetRequest,ref AuthNetResponse newAuthNetResponse)
        {
            try
            {
                type = "POST";
                newAuthNetRequest = new PaymentRepository().CreateAuthNetRequest(loginURL, base.ApiLogin, type, Convert.ToDecimal(HoldAmount), "", "", "", "", true, DateTime.Now, null);
                CreditRequest creditRequest = new CreditRequest(sTransId, HoldAmount, CardNumber);
                var gateway = new Gateway(base.ApiLogin, base.ApiTransKey);
                IGatewayResponse response = gateway.Send(creditRequest);
                newAuthNetResponse = new PaymentRepository().CreateAuthNetResponse(newAuthNetRequest.AuthNetRequestId, response.ResponseCode, "", "", "", response.AuthorizationCode, response.TransactionID, "", "", "", "", Convert.ToDecimal(response.Amount), DateTime.Now);
                return response.Approved;
            }
            catch
            {
                return false;
            }
        }

        public AuthNetResponse GetAuthNetResponseByOrderNum(string orderNum)
        {
            return new PaymentRepository().GetAuthNetResponseByOrderNum(orderNum);
        }

        public AuthNetResponseCode GetAuthResponseErrorDescription(int responseCode, int errorReasonCode) 
        {
            return new PaymentRepository().GetAuthResponseErrorDescription(responseCode, errorReasonCode);
        }

        public AuthNetResponseRejectionCode GetAuthNetResponseRejectionDescription(int responseCodeReason, string rejectionReasonCode) 
        {
            return new PaymentRepository().GetAuthNetResponseRejectionDescription(responseCodeReason, rejectionReasonCode);
        }

        public WinningBidderTransaction GetWinningBidderTransactionByOrderNumber(string orderNumber) 
        {
            return new PaymentRepository().GetWinningBidderTransactionByOrderNumber(orderNumber);

        }

        public WinningBidderTransaction GetWinningBidderTransactionByBidderID(int WinningBidderId) 
        {
            return new PaymentRepository().GetWinningBidderTransactionByBidderID(WinningBidderId);
        }
         */
        //public bool PostAIMrequesest(IGatewayRequest gatewayRequest) 
        //{
        //    try
        //    {
        //        var gateway = new Gateway(base.ApiLogin, base.ApiTransKey);
        //        gateway.Send
        //    }
        //    catch 
        //    {
        //        return false;
        //    }
        //}
        //public void SubmitForm(CustomerProfile customerProfile, string InvoiceNumber)
        //{

        //}

        public bool ValidateMD5HashSecurity(string mD5Hash, string transId, string transAmmount)
        {
            return Crypto.IsMatch(base.AuthNetMD5Hash, base.ApiLogin, transId, Convert.ToDecimal(transAmmount), mD5Hash);
        }

        // This is a wrapper for the VB.NET's built-in HMACMD5 functionality
        // This function takes the data and key as strings and returns the hash as a hexadecimal value
        string HMAC_MD5(string key, string value)
        {
            // The first two lines take the input values and convert them from strings to Byte arrays
            byte[] HMACkey = (new System.Text.ASCIIEncoding()).GetBytes(key);
            byte[] HMACdata = (new System.Text.ASCIIEncoding()).GetBytes(value);

            // create a HMACMD5 object with the key set
            HMACMD5 myhmacMD5 = new HMACMD5(HMACkey);

            //calculate the hash (returns a byte array)
            byte[] HMAChash = myhmacMD5.ComputeHash(HMACdata);

            //loop through the byte array and add append each piece to a string to obtain a hash string
            string fingerprint = "";
            for (int i = 0; i < HMAChash.Length; i++)
            {
                fingerprint += HMAChash[i].ToString("x").PadLeft(2, '0');
            }

            return fingerprint;
        }


    }

}
