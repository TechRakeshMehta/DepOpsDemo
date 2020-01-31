#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;

#endregion

#region UserDefined

using Entity.ClientEntity;
using CoreWeb.Shell;
using INTSOF.Utils;
using INTSOF.AuthNet.Business;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Logger;
using System.Text;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class OnlinePaymentSubmission : System.Web.UI.Page, IOnlinePaymentSubmissionView
    {
        #region Variables

        #region Private Variables

        private OnlinePaymentSubmissionPresenter _presenter = new OnlinePaymentSubmissionPresenter();
        private ApplicantOrderCart _applicantOrderCart;

        #endregion

        #region Public Variables

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

        public IOnlinePaymentSubmissionView CurrentViewContext
        {
            get { return this; }
        }


        public OnlinePaymentSubmissionPresenter Presenter
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
        #endregion

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
                    {
                        return;
                    }

                    SetBillingInformation();
                    Presenter.OnViewInitialized();
                }

                if (!IsPostBack && CurrentViewContext.OnlinePaymentTransactionDetails.IsNotNull())
                {
                    GetSettings();
                    SetApplicationOrderCart();
                    ErrorLog logFile = new ErrorLog("Data is sent from OnlinePaymentSubmission page for InvoiceNumber " + CurrentViewContext.InvoiceNumber + ".");
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorLog logFile = new ErrorLog("Problem in sending data from OnlinePaymentSubmission page" + ex);
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

        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (_applicantOrderCart.IsNull())
            {
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return _applicantOrderCart;
        }

        private Boolean IsBiilingInfoSameAsAccountInfo()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            if (_applicantOrderCart.IsBiilingInfoSameAsAccountInfo)
            {
                return true;
            }
            return false;
        }

        private void GetSettings()
        {
            String _relayUrl = Convert.ToString(ConfigurationManager.AppSettings["AuthNetRetUrl"]);
            Dictionary<String, String> billingInfo = new Dictionary<String, String>()
                    {
                        {"x_first_name", CurrentViewContext.FirstName},
                        {"x_last_name", CurrentViewContext.LastName},
                        {"x_company",CurrentViewContext.Company},
                        {"x_address",CurrentViewContext.Address},
                        {"x_city",CurrentViewContext.City},
                        {"x_state",CurrentViewContext.State},
                        {"x_zip",CurrentViewContext.Zip},
                        {"x_country",CurrentViewContext.Country},
                        {"x_email", CurrentViewContext.Email},
                        {"x_phone",CurrentViewContext.Phone},
                        {"x_fax",CurrentViewContext.Fax}
                    };

            INTSOF.AuthNet.Business.PaymentManager paymentManager = new INTSOF.AuthNet.Business.PaymentManager();
            INTSOF.AuthNet.Business.PaymentFormBuilder pfb = paymentManager.SubmitCard(CurrentViewContext.OnlinePaymentTransactionDetails.Invoice_num, Convert.ToString(CurrentViewContext.OnlinePaymentTransactionDetails.Amount), _relayUrl, billingInfo);
            form1.Action = pfb.Url;


            for (int i = 0; i < pfb.Inputs.Keys.Count; i++)
            {
                String keyName = pfb.Inputs.Keys[i].ToString();
                String keyValue = pfb.Inputs.Get(keyName);

                switch (keyName)
                {
                    case "x_login":
                        x_login.Value = keyValue;
                        break;
                    case "x_amount":
                        x_amount.Value = keyValue;
                        break;
                    case "x_description":
                        x_description.Value = keyValue;
                        break;
                    case "x_invoice_num":
                        x_invoice_num.Value = keyValue;
                        break;
                    case "x_fp_sequence":
                        x_fp_sequence.Value = keyValue;
                        break;
                    case "x_fp_timestamp":
                        x_fp_timestamp.Value = keyValue;
                        break;
                    case "x_fp_hash":
                        x_fp_hash.Value = keyValue;
                        break;
                    case "x_test_request":
                        x_test_request.Value = keyValue;
                        break;
                    case ApiFields.Method:
                        x_method.Value = keyValue;
                        break;

                    //BILLING INFO
                    case "x_first_name":
                        x_first_name.Value = keyValue;
                        break;
                    case "x_last_name":
                        x_last_name.Value = keyValue;
                        break;
                    case "x_company":
                        x_company.Value = keyValue;
                        break;
                    case "x_address":
                        x_address.Value = keyValue;
                        break;
                    case "x_city":
                        x_city.Value = keyValue;
                        break;
                    case "x_state":
                        x_state.Value = keyValue;
                        break;
                    case "x_zip":
                        x_zip.Value = keyValue;
                        break;
                    case "x_country":
                        x_country.Value = keyValue;
                        break;
                    case "x_email":
                        x_email.Value = keyValue;
                        break;
                    case "x_phone":
                        x_phone.Value = keyValue;
                        break;
                    case "x_fax":
                        x_fax.Value = keyValue;
                        break;
                }
            }
        }

        private void SetApplicationOrderCart()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            RedirectIfIncorrectOrderStage(_applicantOrderCart);
            _applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlinePaymentSubmission);
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ONLINE_PAYMENT_SUBMITTED, _applicantOrderCart);

            StringBuilder _sbInfo = new StringBuilder();
            //var logger = CoreWeb.Shell.SysXWebSiteUtils.LoggerService.GetLogger();

            //LogInfo(logger, "Entered the OnlinePaymentSubmission.aspx for OrganizationUserID : " + applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserID
            //    + " AND Invoice Number : " + CurrentViewContext.InvoiceNumber);
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
                Response.Redirect(CurrentViewContext.NextPagePath);
            }
        }


        //private static void LogInfo(ILogger logger, String message)
        //{
        //    logger.Info(message);
        //}

        /// <summary>
        /// Method to set Billing Information fro organization user.
        /// </summary>
        private void SetBillingInformation()
        {
            if (IsBiilingInfoSameAsAccountInfo())
            {
                _applicantOrderCart = GetApplicantOrderCart();

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
            }
            else
            {
                CurrentViewContext.FirstName = String.Empty;
                CurrentViewContext.LastName = String.Empty;
                CurrentViewContext.Phone = String.Empty;
                CurrentViewContext.Email = String.Empty;
                CurrentViewContext.Address = String.Empty;
                CurrentViewContext.City = String.Empty;
                CurrentViewContext.State = String.Empty;
                CurrentViewContext.Zip = String.Empty;
                CurrentViewContext.Country = String.Empty;
                CurrentViewContext.Fax = String.Empty;
                CurrentViewContext.Company = String.Empty;
            }
        }

    }

        #endregion

        #endregion
}

