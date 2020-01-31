using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using INTSOF.AuthNet.Business.CustomerProfileWS;

namespace INTSOF.AuthNet.Business
{
    public static class AuthorizeNetCreditCard
    {
        /// <summary>
        /// To submit credit card info
        /// </summary>
        /// <param name="institutionID"></param>
        /// <param name="institutionName"></param>
        /// <param name="orderID"></param>
        /// <param name="userID"></param>
        /// <param name="amount">Charge Amount</param>
        /// <param name="cardNumber">No Dashes or Spaces</param>
        /// <param name="expDate">Format: MMYY, MM/YY, MM-YY, MMYYYY, MM/YYYY, or MM-YYYY</param>
        /// <param name="cardholderFirstName"></param>
        /// <param name="cardholderLastName"></param>
        /// <param name="cardholderAddress"></param>
        /// <param name="cardholderCity"></param>
        /// <param name="cardholderState"></param>
        /// <param name="cardholderZip"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static string SubmitCreditCardInfo(
            int institutionID,
            string institutionName,
            long orderID,
            long userID,
            decimal amount,
            string cardNumber,
            string expDate,
            string cardholderFirstName,
            string cardholderLastName,
            string cardholderAddress,
            string cardholderCity,
            string cardholderState,
            string cardholderZip,
            string username,
            string password,
            string url,
            out string reason,
            out string transactionID)
        {
            if (orderID == 0
                || amount == 0
                || userID == 0
                || institutionID == 0
                || string.IsNullOrEmpty(institutionName)
                || string.IsNullOrEmpty(cardNumber)
                || string.IsNullOrEmpty(expDate)
                || string.IsNullOrEmpty(cardholderFirstName)
                || string.IsNullOrEmpty(cardholderLastName)
                || string.IsNullOrEmpty(cardholderAddress)
                || string.IsNullOrEmpty(cardholderCity)
                || string.IsNullOrEmpty(cardholderState)
                || string.IsNullOrEmpty(cardholderZip)
                || string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(url)
                )
            {
                StringBuilder sbError = new StringBuilder();
                if (string.IsNullOrEmpty(url))
                {
                    sbError.AppendLine("Error url IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(password))
                {
                    sbError.AppendLine("Error password IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(username))
                {
                    sbError.AppendLine("Error username IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(cardholderZip))
                {
                    sbError.AppendLine("Error cardholderZip IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(cardholderState))
                {
                    sbError.AppendLine("Error cardholderState IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(cardholderCity))
                {
                    sbError.AppendLine("Error cardholderCity IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(cardholderAddress))
                {
                    sbError.AppendLine("Error cardholderAddress IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(cardholderLastName))
                {
                    sbError.AppendLine("Error cardholderLastName IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(cardholderFirstName))
                {
                    sbError.AppendLine("Error cardholderFirstName IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(expDate))
                {
                    sbError.AppendLine("Error expDate IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(cardNumber))
                {
                    sbError.AppendLine("Error cardNumber IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(institutionName))
                {
                    sbError.AppendLine("Error institutionName IsNullOrEmpty.");
                }
                if (amount == 0)
                {
                    sbError.AppendLine("Error amount=0");
                }
                if (orderID == 0)
                {
                    sbError.AppendLine("Error OrderID=0");
                }
                if (institutionID == 0)
                {
                    sbError.AppendLine("Error institutionID=0");
                }
                if (userID == 0)
                {
                    sbError.AppendLine("Error userID=0");
                }
                sbError.AppendLine("Missing required fields. Transaction not sent.");
                throw new Exception(sbError.ToString());
            }
            reason = "None";
            transactionID = string.Empty;

            string result = "";
            string strPost = "";
            StringBuilder sbPost = new StringBuilder();

            // Build the outbound request string 
            sbPost.Append("x_login=" + username);
            sbPost.Append("&x_tran_key=" + password);
            sbPost.Append("&x_method=CC");
            sbPost.Append("&x_type=AUTH_CAPTURE");
            sbPost.Append("&x_delim_data=TRUE");
            sbPost.Append("&x_delim_char=|");
            sbPost.Append("&x_relay_response=FALSE");

            // Card and purchase information
            sbPost.Append("&x_card_num=" + cardNumber);
            sbPost.Append("&x_exp_date=" + expDate);
            sbPost.Append("&x_amount=" + amount.ToString());
            sbPost.Append("&x_invoice_num=AMS-" + institutionID.ToString() + "-" + orderID.ToString());
            //sbPost.Append("&x_Description=American Databank (" + institutionName + ") - OrderID: "+orderID.ToString());
            //02/17/2015 [SG]: UAT-1023 - Complio: Update Credit Card Spreadsheet
            //sbPost.Append("&x_Description=AMS:  " + institutionName + " (Order #:  " + orderID.ToString() + ")");
            sbPost.Append("&x_Description=" + institutionName);

            // Cardholder information
            sbPost.Append("&x_cust_id=" + userID);
            sbPost.Append("&x_first_name=" + cardholderFirstName);
            sbPost.Append("&x_Last_Name=" + cardholderLastName);
            sbPost.Append("&x_Address=" + cardholderAddress);
            sbPost.Append("&x_city=" + cardholderCity);
            sbPost.Append("&x_state=" + cardholderState);
            sbPost.Append("&x_Zip=" + cardholderZip);

            // Completion of outbound request string
            sbPost.Append("&x_test_request=FLSE");
            sbPost.Append("&x_version=3.1");

            // Assign completed string to variable
            strPost = sbPost.ToString();

            //Get bytes using UTF-8 encoding, which allows for special characters during the Authorize checkout
            byte[] utfBytes = Encoding.UTF8.GetBytes(strPost);

            /* Previous request string...
            strPost = String.Format("x_login={0}&x_tran_key={1}&x_method=CC&x_type=AUTH_CAPTURE&x_amount={2}&x_delim_data=TRUE&x_delim_char=|&x_relay_response=FALSE&x_card_num={3}&x_exp_date={4}&x_test_request=FLSE&x_version=3.1",
                new object[5] { loginID, transactionKey, amount, cardNumber, expDate }); */

            //Request//
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            request.ContentLength = utfBytes.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            Stream dataStream = null;

            try
            {
                dataStream = request.GetRequestStream();
                dataStream.Write(utfBytes, 0, utfBytes.Length);
            }
            catch (Exception ex)
            {
                reason = ex.Message;
                return ex.Message;
            }
            finally
            {
                dataStream.Flush();
                dataStream.Close();
            }

            //Response//
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader sr = null;

            try
            {
                sr = new StreamReader(response.GetResponseStream());
                result = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                reason = ex.Message;
                return ex.Message;
            }
            finally
            {
                sr.Close();
            }

            if (result.Length == 0)
            {
                reason = "No result";
                return "";
            }

            string[] resultVal = result.Split('|');
            if (resultVal[0] != "1")
            {
                reason = string.Format("Code={0}, {1}", resultVal[2], resultVal[3], resultVal[4], resultVal[5]);
            }

            //Set transactionID: The transation id is at the 7 postion or index 6 on a zero based array.
            transactionID = resultVal[6];
            return resultVal[0];
        }

        #region CIM Methods

        /// <summary>
        /// Delete the customer profile
        /// </summary>
        /// <param name="profileId">The ID that should be deleted</param>
        /// <returns>True if successfull, false if not</returns>
        public static Boolean DeleteCustomerProfile(long profileId)
        {
            String parameters = "profileId:" + profileId;
            try
            {
                DeleteCustomerProfileResponseType response = SoapAPIUtilities.Service.DeleteCustomerProfile(SoapAPIUtilities.MerchantAuthentication, profileId);
                return (response.resultCode == MessageTypeEnum.Ok);
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete the customer payment profile
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="customerPaymentProfileId"></param>
        /// <returns></returns>
        public static Boolean DeleteCustomerPaymentProfile(long profileId, long customerPaymentProfileId)
        {
            string parameters = "profileId:" + profileId + "customerPaymentProfileId:" + customerPaymentProfileId;
            try
            {
                DeleteCustomerPaymentProfileResponseType response = SoapAPIUtilities.Service.DeleteCustomerPaymentProfile(SoapAPIUtilities.MerchantAuthentication, profileId, customerPaymentProfileId);
                return (response.resultCode == MessageTypeEnum.Ok);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete the customer payment profile
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="customerPaymentProfileId"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static Boolean DeleteCustomerPaymentProfile(long profileId, long customerPaymentProfileId, Int32 tenantID)
        {
            string parameters = "profileId:" + profileId + "customerPaymentProfileId:" + customerPaymentProfileId;
            try
            {
                DeleteCustomerPaymentProfileResponseType response = SoapAPIUtilities.Service.DeleteCustomerPaymentProfile(SoapAPIUtilities.MerchantAuthenticationByTenant(tenantID), profileId, customerPaymentProfileId);
                return (response.resultCode == MessageTypeEnum.Ok);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Get the profile for the specified customer with tenantID
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="tenantID"></param>
        /// <returns>The profile retrieved</returns>
        public static CustomerProfileMaskedType GetCustomerProfile(long profileId, Int32 tenantID)
        {
            GetCustomerProfileResponseType response_type = SoapAPIUtilities.Service.GetCustomerProfile(SoapAPIUtilities.MerchantAuthenticationByTenant(tenantID), profileId);
            return response_type.profile;
        }
        /// <summary>
        /// Get the profile for the specified customer
        /// </summary>
        /// <param name="profileId">The ID for the customer that we are retrieving</param>
        /// <returns>The profile retrieved</returns>
        public static CustomerProfileMaskedType GetCustomerProfile(long profileId)
        {
            GetCustomerProfileResponseType response_type = SoapAPIUtilities.Service.GetCustomerProfile(SoapAPIUtilities.MerchantAuthentication, profileId);
            return response_type.profile;
        }

        /// <summary>
        /// get customer payment profiles
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public static List<PaymentProfileDetail> GetCustomerPaymentProfiles(long profileId)
        {
            List<PaymentProfileDetail> paymentProfileDetailList = new List<PaymentProfileDetail>();
            CustomerProfileMaskedType profile = GetCustomerProfile(profileId);
            PupulatePaymentProfiles(paymentProfileDetailList, profile);
            return paymentProfileDetailList;
        }
        /// <summary>
        /// get customer payment profiles with tenantID
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<PaymentProfileDetail> GetCustomerPaymentProfiles(long profileId, Int32 tenantID)
        {
            List<PaymentProfileDetail> paymentProfileDetailList = new List<PaymentProfileDetail>();
            CustomerProfileMaskedType profile = GetCustomerProfile(profileId, tenantID);
            PupulatePaymentProfiles(paymentProfileDetailList, profile);
            return paymentProfileDetailList;
        }

        /// <summary>
        /// Return List of  Payment Cards
        /// </summary>
        /// <param name="paymentProfileDetailList"></param>
        /// <param name="profile"></param>
        private static void PupulatePaymentProfiles(List<PaymentProfileDetail> paymentProfileDetailList, CustomerProfileMaskedType profile)
        {
            if (profile != null && profile.paymentProfiles != null)
            {
                var paymentProfiles = profile.paymentProfiles.ToList();
                foreach (var paymentProfile in paymentProfiles)
                {
                    PaymentProfileDetail paymentProfileDetail = new PaymentProfileDetail();
                    long customerPaymentProfileId = paymentProfile.customerPaymentProfileId;
                    var creditCardMaskedType = ((CreditCardMaskedType)(paymentProfile.payment.Item));
                    paymentProfileDetail.CardNumber = creditCardMaskedType.cardNumber;
                    paymentProfileDetail.CardType = creditCardMaskedType.cardType;
                    paymentProfileDetail.Expirydate = creditCardMaskedType.expirationDate;
                    paymentProfileDetail.NameOnCard = paymentProfile.billTo.firstName + " " + paymentProfile.billTo.lastName;
                    paymentProfileDetail.CardTypeAndNumber = creditCardMaskedType.cardNumber;
                    if (creditCardMaskedType.cardType != null && creditCardMaskedType.cardType != String.Empty)
                    {
                        paymentProfileDetail.CardTypeAndNumber += " (" + creditCardMaskedType.cardType + ")";
                    }

                    paymentProfileDetail.CustomerPaymentProfileId = paymentProfile.customerPaymentProfileId;
                    paymentProfileDetailList.Add(paymentProfileDetail);
                }
            }
        }

        /// <summary>
        /// Create the customer profile
        /// </summary>
        /// <returns>The ID for the customer profile that was created</returns>
        public static long CreateCustomerProfile(Int32 CurrentLoggedInUserId, string emailId, string description)
        {

            long customerProfileId = 0;
            ValidateCustomerProfileDetails(CurrentLoggedInUserId, emailId, description);
            CustomerProfileType customerProfile = new CustomerProfileType();
            customerProfile.merchantCustomerId = CurrentLoggedInUserId.ToString();
            customerProfile.email = emailId;
            customerProfile.description = description;

            CreateCustomerProfileResponseType response = SoapAPIUtilities.Service.CreateCustomerProfile(SoapAPIUtilities.MerchantAuthentication, customerProfile, ValidationModeEnum.none);

            customerProfileId = response.customerProfileId;
            return customerProfileId;

        }
        public static long CreateCustomerProfile(Int32 CurrentLoggedInUserId, string emailId, string description, Int32 tenantID)
        {
            long customerProfileId;
            CustomerProfileType customerProfile;
            ValidateCustomerProfileDetails(CurrentLoggedInUserId, emailId, description);

            customerProfileId = 0;
            customerProfile = new CustomerProfileType();
            customerProfile.merchantCustomerId = CurrentLoggedInUserId.ToString();
            customerProfile.email = emailId;
            customerProfile.description = description;
            CreateCustomerProfileResponseType response = SoapAPIUtilities.Service.CreateCustomerProfile(SoapAPIUtilities.MerchantAuthenticationByTenant(tenantID), customerProfile, ValidationModeEnum.none);

            customerProfileId = response.customerProfileId;
            return customerProfileId;
        }

        private static void ValidateCustomerProfileDetails(int CurrentLoggedInUserId, string emailId, string description)
        {
            if (CurrentLoggedInUserId == 0
                            || string.IsNullOrEmpty(emailId)
                            || string.IsNullOrEmpty(description)
                            )
            {
                StringBuilder sbError = new StringBuilder();
                if (CurrentLoggedInUserId == 0)
                {
                    sbError.AppendLine("Error userId is null");
                }
                if (string.IsNullOrEmpty(emailId))
                {
                    sbError.AppendLine("Error emailId IsNullOrEmpty.");
                }
                if (string.IsNullOrEmpty(description))
                {
                    sbError.AppendLine("Error description IsNullOrEmpty.");
                }

                sbError.AppendLine("Missing required fields. Transaction not sent.");
                throw new Exception(sbError.ToString());

            }
        }

        /// <summary>
        /// To create customer profile transaction
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="customerPaymentProfileId"></param>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <param name="amount"></param>
        /// <param name="institutionId"></param>
        /// <param name="institutionName"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static CreateCustomerProfileTransactionResponseType CreateCustomerProfileTransaction(long profileId, long customerPaymentProfileId, Int32 CurrentLoggedInUserId, decimal amount,
            String invoiceNumber, string description)
        {
            if (amount == 0
               || String.IsNullOrEmpty(invoiceNumber)
               || profileId == 0
               || customerPaymentProfileId == 0
               || String.IsNullOrEmpty(description)
                || CurrentLoggedInUserId == 0
               )
            {
                StringBuilder sbError = new StringBuilder();

                if (profileId == 0)
                {
                    sbError.AppendLine("Error profileId=0");
                }
                if (customerPaymentProfileId == 0)
                {
                    sbError.AppendLine("Error customerPaymentProfileId=0");
                }
                if (CurrentLoggedInUserId == 0)
                {
                    sbError.AppendLine("Error userId is null");
                }
                if (amount == 0)
                {
                    sbError.AppendLine("Error amount=0");
                }
                if (string.IsNullOrEmpty(invoiceNumber))
                {
                    sbError.AppendLine("Error invoiceNumber IsNullOrEmpty");
                }
                if (string.IsNullOrEmpty(description))
                {
                    sbError.AppendLine("Error description IsNullOrEmpty.");
                }

                sbError.AppendLine("Missing required fields. Transaction not sent.");
                throw new Exception(sbError.ToString());

            }
            try
            {
                //// Create Auth & Capture Transaction
                //string invoiceNumber = "AMS-" + institutionId.ToString() + "-" + orderId.ToString();
                //string description = "AMS:  " + institutionName + " (Order #:  " + orderId.ToString() + ")";

                ProfileTransactionType transaction = new ProfileTransactionType();
                ProfileTransAuthCaptureType profileTransAuthCaptureType = new ProfileTransAuthCaptureType();
                profileTransAuthCaptureType.customerProfileId = profileId;
                profileTransAuthCaptureType.customerPaymentProfileId = customerPaymentProfileId;
                profileTransAuthCaptureType.amount = amount;
                OrderExType orderExType = new OrderExType();
                orderExType.invoiceNumber = invoiceNumber;
                orderExType.description = description;
                profileTransAuthCaptureType.order = orderExType;

                //profileTransAuthCaptureType.cardCode = cvv;

                transaction.Item = profileTransAuthCaptureType;

                string extraOptions = GetExtraOptions(CurrentLoggedInUserId);
                //CreateCustomerProfileTransactionResponseType response = SoapAPIUtilities.Service.CreateCustomerProfileTransaction(SoapAPIUtilities.MerchantAuthentication, transaction, "&x_test_request=FALSE&x_delim_data=TRUE&x_delim_char=|&x_encap_char=;&x_email_customer=FALSE&x_relay_response=FALSE&x_version=3.1&x_duplicate_window=30");
                //SoapAPIUtilities.Service.CreateCustomerProfileTransaction(SoapAPIUtilities.MerchantAuthentication, trans, "x_duplicate_window=0,&x_zip=40282");

                CreateCustomerProfileTransactionResponseType response = SoapAPIUtilities.Service.CreateCustomerProfileTransaction(SoapAPIUtilities.MerchantAuthentication, transaction, extraOptions);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CreateCustomerProfileTransactionResponseType CreateCustomerProfileTransaction(long profileId, long customerPaymentProfileId, Int32 CurrentLoggedInUserId, decimal amount,
            String invoiceNumber, string description,Int32 tenantID)
        {
            if (amount == 0
               || String.IsNullOrEmpty(invoiceNumber)
               || profileId == 0
               || customerPaymentProfileId == 0
               || String.IsNullOrEmpty(description)
                || CurrentLoggedInUserId == 0
               )
            {
                StringBuilder sbError = new StringBuilder();

                if (profileId == 0)
                {
                    sbError.AppendLine("Error profileId=0");
                }
                if (customerPaymentProfileId == 0)
                {
                    sbError.AppendLine("Error customerPaymentProfileId=0");
                }
                if (CurrentLoggedInUserId == 0)
                {
                    sbError.AppendLine("Error userId is null");
                }
                if (amount == 0)
                {
                    sbError.AppendLine("Error amount=0");
                }
                if (string.IsNullOrEmpty(invoiceNumber))
                {
                    sbError.AppendLine("Error invoiceNumber IsNullOrEmpty");
                }
                if (string.IsNullOrEmpty(description))
                {
                    sbError.AppendLine("Error description IsNullOrEmpty.");
                }

                sbError.AppendLine("Missing required fields. Transaction not sent.");
                throw new Exception(sbError.ToString());

            }
            try
            {
                //// Create Auth & Capture Transaction
                //string invoiceNumber = "AMS-" + institutionId.ToString() + "-" + orderId.ToString();
                //string description = "AMS:  " + institutionName + " (Order #:  " + orderId.ToString() + ")";

                ProfileTransactionType transaction = new ProfileTransactionType();
                ProfileTransAuthCaptureType profileTransAuthCaptureType = new ProfileTransAuthCaptureType();
                profileTransAuthCaptureType.customerProfileId = profileId;
                profileTransAuthCaptureType.customerPaymentProfileId = customerPaymentProfileId;
                profileTransAuthCaptureType.amount = amount;
                OrderExType orderExType = new OrderExType();
                orderExType.invoiceNumber = invoiceNumber;
                orderExType.description = description;
                profileTransAuthCaptureType.order = orderExType;

                //profileTransAuthCaptureType.cardCode = cvv;

                transaction.Item = profileTransAuthCaptureType;

                string extraOptions = GetExtraOptions(CurrentLoggedInUserId);
                //CreateCustomerProfileTransactionResponseType response = SoapAPIUtilities.Service.CreateCustomerProfileTransaction(SoapAPIUtilities.MerchantAuthentication, transaction, "&x_test_request=FALSE&x_delim_data=TRUE&x_delim_char=|&x_encap_char=;&x_email_customer=FALSE&x_relay_response=FALSE&x_version=3.1&x_duplicate_window=30");
                //SoapAPIUtilities.Service.CreateCustomerProfileTransaction(SoapAPIUtilities.MerchantAuthentication, trans, "x_duplicate_window=0,&x_zip=40282");

                CreateCustomerProfileTransactionResponseType response = SoapAPIUtilities.Service.CreateCustomerProfileTransaction(SoapAPIUtilities.MerchantAuthenticationByTenant(tenantID), transaction, extraOptions);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static NameValueCollection GetResponseFields(CreateCustomerProfileTransactionResponseType response)
        {
            string AuthTranMsg = "";
            string AuthTranCode = "";
            for (int i = 0; i < response.messages.Length; i++)
            {
                AuthTranMsg = response.messages[i].text;  // To Get Message n for loop to check the [i] is not empty 
            }
            for (int i = 0; i < response.messages.Length; i++)
            {
                AuthTranCode = response.messages[i].code;   // To Get Code n for loop to check the [i] is not empty 
            }
            var tCompResp = new Tuple<string, string, string>(AuthTranCode, AuthTranMsg, response.directResponse);
            NameValueCollection responseFieldValue = new NameValueCollection();
            if (!string.IsNullOrEmpty(tCompResp.Item3))
            {
                string[] arrRespParts = tCompResp.Item3.Split('|');
                responseFieldValue.Add("x_response_code", arrRespParts[0]);
                responseFieldValue.Add("x_response_reason_code", arrRespParts[2]);
                responseFieldValue.Add("x_response_reason_text", arrRespParts[3]);//payment_date , payment_status,
                responseFieldValue.Add("x_auth_code", arrRespParts[4]);
                responseFieldValue.Add("x_avs_code", arrRespParts[5]);
                responseFieldValue.Add("x_trans_id", arrRespParts[6]);
                // responseFieldValue.Add("InvoiceNo", arrRespParts[7]);
                responseFieldValue.Add("Description", arrRespParts[8]);
                responseFieldValue.Add("x_amount", arrRespParts[9]);
                // responseFieldValue.Add("transType", arrRespParts[10]);
                //  responseFieldValue.Add("respMethod", arrRespParts[11]);
                responseFieldValue.Add("payer_id", arrRespParts[12]);
                //  responseFieldValue.Add("respName", arrRespParts[13] + " " + arrRespParts[14]);
                //  responseFieldValue.Add("respEmailId", arrRespParts[23]);
                //   responseFieldValue.Add("respMD5Hash", arrRespParts[37]);
                responseFieldValue.Add("x_account_number", arrRespParts[50]);
                responseFieldValue.Add("x_card_type", arrRespParts[51]);
            }
            return responseFieldValue;
        }

        /// <summary>
        /// To get token
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public static string GetToken(long profileId, string hostedProfileIFrameCommunicatorUrl)
        {
            SettingType[] settingType = new SettingType[3];
            settingType[0] = new SettingType();
            settingType[0].settingName = "hostedProfilePageBorderVisible";
            settingType[0].settingValue = "false";
            settingType[1] = new SettingType();
            settingType[1].settingName = "hostedProfileIFrameCommunicatorUrl";
            settingType[1].settingValue = hostedProfileIFrameCommunicatorUrl;

            GetHostedProfilePageResponseType hostedProfilePageResponseType = SoapAPIUtilities.Service.GetHostedProfilePage(SoapAPIUtilities.MerchantAuthentication, profileId, settingType);
            return hostedProfilePageResponseType.token;
        }

        /// <summary>
        /// To get token when not using iframe
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="hostedProfileIFrameCommunicatorUrl"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public static string GetToken(long profileId, string hostedProfileIFrameCommunicatorUrl, string returnUrl)
        {
            return (GetToken(profileId, hostedProfileIFrameCommunicatorUrl, returnUrl, SoapAPIUtilities.MerchantAuthentication));
        }

        public static string GetToken(long profileId, string hostedProfileIFrameCommunicatorUrl, string returnUrl,Int32 tenantId)
        {
            return (GetToken(profileId, hostedProfileIFrameCommunicatorUrl, returnUrl, SoapAPIUtilities.MerchantAuthenticationByTenant(tenantId)));
        }

        private static string GetToken(long profileId, string hostedProfileIFrameCommunicatorUrl, string returnUrl, CustomerProfileWS.MerchantAuthenticationType merchantAuthenticationType)
        {
            SettingType[] settingType = new SettingType[3];
            settingType[0] = new SettingType();
            settingType[0].settingName = "hostedProfilePageBorderVisible";
            settingType[0].settingValue = "false";
            settingType[1] = new SettingType();
            settingType[1].settingName = "hostedProfileIFrameCommunicatorUrl";
            settingType[1].settingValue = hostedProfileIFrameCommunicatorUrl;
            settingType[2] = new SettingType();
            settingType[2].settingName = "hostedProfileReturnUrl";
            settingType[2].settingValue = returnUrl;

            GetHostedProfilePageResponseType hostedProfilePageResponseType = SoapAPIUtilities.Service.GetHostedProfilePage(merchantAuthenticationType, profileId, settingType);
            return hostedProfilePageResponseType.token;
        }

        /// <summary>
        /// get extra options
        /// </summary>
        /// <returns></returns>
        public static string GetExtraOptions(Int32 CurrentLoggedInUserId, Boolean isRefund = false)
        {
            StringBuilder sbPost = new StringBuilder();

            // Build the outbound request string 
            sbPost.Append("x_method=CC");
            if (isRefund)
                sbPost.Append("&x_type=CREDIT");
            else
                sbPost.Append("&x_type=AUTH_CAPTURE");

            sbPost.Append("&x_delim_data=TRUE");
            sbPost.Append("&x_delim_char=|");
            sbPost.Append("&x_relay_response=FALSE");

            // Cardholder information
            sbPost.Append("&x_cust_id=" + CurrentLoggedInUserId);

            // Completion of outbound request string
            sbPost.Append("&x_test_request=FALSE");
            sbPost.Append("&x_version=3.1");

            // Assign completed string to variable
            return sbPost.ToString();
        }

        public static CreateCustomerProfileTransactionResponseType ProcessRefund(String transactionId, long customerProfileId,
                                         Decimal refundAmount, Int32 organizationUserId, String ccNumber, String description, String invoiceNumber, Int32 applicantTenantID)
        {
            ProfileTransactionType transaction = new ProfileTransactionType();
            ProfileTransRefundType _refundType = new ProfileTransRefundType();
            _refundType.amount = refundAmount;
            _refundType.customerProfileId = customerProfileId;
            _refundType.transId = transactionId;
            _refundType.creditCardNumberMasked = ccNumber;

            OrderExType orderExType = new OrderExType();
            orderExType.invoiceNumber = invoiceNumber;
            orderExType.description = description;
            _refundType.order = orderExType;

            transaction.Item = _refundType;

            String extraOptions = GetExtraOptions(organizationUserId, true);
            return SoapAPIUtilities.Service.CreateCustomerProfileTransaction(SoapAPIUtilities.MerchantAuthenticationByTenant(applicantTenantID), transaction, extraOptions);
        }

        #endregion
    }
}

