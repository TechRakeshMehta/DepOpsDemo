#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using CoreWeb.Shell;
using System.Xml.Serialization;
using System.IO;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using System.Threading;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.IntsofSecurityModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.RepoManagers;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using CoreWeb.BkgOperations.Views;
using System.Collections.Specialized;
using INTSOF.AuthNet.Business.CustomerProfileWS;
using System.Web.Configuration;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.UI.Contract.Globalization;
using System.Globalization;
using INTSOF.UI.Contract.ProfileSharing;
using System.Xml;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class OrderPaymentDetails : BaseUserControl, IOrderPaymentDetailsView
    {
        #region Variables

        #region Private Variables

        private OrderPaymentDetailsPresenter _presenter = new OrderPaymentDetailsPresenter();
        private Int32 _tenantId;
        private String _viewType;
        //List<OrderDetailContract> orderDetailContracts = null;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private OrderApprovalQueueContract _gridSearchContract = null;
        private Boolean _showApprovePayment = false;
        private Boolean _showApproveCancellation = false;
        private Int32 _nextOrderId = 0;
        private OrderPaymentDetail _orderPaymentDetail = null;
        private ApplicantOrderCart _applicantOrderCart;
        private Boolean _showOfflineSettlement = false;
        private Boolean _showItemPaymentDetail_AdminOnly = true;
        #endregion

        #endregion

        #region Properties

        public OrderPaymentDetailsPresenter Presenter
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
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IOrderPaymentDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        //UAT-4537
        public DataSet lstPaymentOption
        {
            get
            {
                if (ViewState["lstPaymentOption"].IsNotNull())
                {
                    return (DataSet)ViewState["lstPaymentOption"];
                }
                return new DataSet();
            }
            set
            {
                ViewState["lstPaymentOption"] = value;
            }
        }
        //UAT-4537 
        public List<String> lstPendingApprovalPackageNames
        {
            get
            {
                if (ViewState["lstPendingApprovalPackageNames"].IsNotNull())
                {
                    return (List<String>)ViewState["lstPendingApprovalPackageNames"];
                }
                return new List<string>();
            }
            set
            {
                ViewState["lstPendingApprovalPackageNames"] = value;
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

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    //_tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        public Boolean IsApplicant
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    return user.IsApplicant;
                }
                return false;
            }

        }

        public List<OrderContract> lstOrderQueue
        {
            get;
            set;
        }

        public List<Int32> BopIds { get; set; }
        public List<OrderDetailContract> orderDetailContracts
        {
            get
            {
                if (!ViewState["orderDetailContracts"].IsNullOrEmpty())
                    return ViewState["orderDetailContracts"] as List<OrderDetailContract>;
                return new List<OrderDetailContract>();
            }
            set
            {
                ViewState["orderDetailContracts"] = value;
            }
        }

        public FingerPrintOrderKeyDataContract lstFingerPrintData
        {
            get;
            set;
        }
        public PreviousAddressContract MailingAddressData
        {
            get;
            set;
        }

        /// <summary>
        /// returns the Current Logged In User Id.
        /// </summary>
        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        /// <summary>
        /// Sets or gets the Selected Tenant Id from the select tenant dropdown.
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        /// <summary>
        /// sets or gets the Order status code.
        /// </summary>
        public String OrderStatusCode
        {
            get
            {
                if (!ViewState["OrderStatusCode"].IsNull())
                {
                    return ViewState["OrderStatusCode"].ToString();
                }
                return String.Empty;
            }
            set
            {
                ViewState["OrderStatusCode"] = value;
            }
        }


        /// <summary>
        /// Sets or gets the packae ID.
        /// </summary>
        public Int32 PackageId
        {
            get
            {
                if (!ViewState["PackageId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["PackageId"]);
                }
                return 0;
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }

        /// <summary>
        /// Sets or gets the Organization Id.
        /// </summary>
        public Int32 OrganizationUserId
        {
            get
            {
                if (!ViewState["OrganizationUserId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["OrganizationUserId"]);
                }
                return 0;
            }
            set
            {
                ViewState["OrganizationUserId"] = value;
            }
        }

        public DateTime ExpiryDate
        {
            get
            {
                if (!ViewState["ExpiryDate"].IsNull())
                {
                    return Convert.ToDateTime(ViewState["ExpiryDate"]);
                }
                return DateTime.Now;
            }
            set
            {
                ViewState["ExpiryDate"] = value;
            }
        }

        public String PaymentTypeCode
        {
            get
            {
                if (!ViewState["PaymentTypeCode"].IsNull())
                {
                    return ViewState["PaymentTypeCode"].ToString();
                }
                return String.Empty;
            }
            set
            {
                ViewState["PaymentTypeCode"] = value;
            }
        }

        public List<lkpPaymentOption> ListPaymentOptions
        {
            get;
            set;
        }

        public Int32 SelectedPaymentOptionID
        {
            get
            {
                //UAT-916
                //return Convert.ToInt32(cmbPaymentModes.SelectedValue);
                return 0;
            }
            set
            {
                //UAT-916
                //cmbPaymentModes.SelectedValue = value.ToString();
            }
        }

        public Int32 DPM_ID
        {
            get
            {
                if (!ViewState["DPM_ID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["DPM_ID"]);
                }
                return 0;
            }
            set
            {
                ViewState["DPM_ID"] = value;
            }
        }

        public List<PersonAliasContract> PersonAliasList
        {
            get
            {
                return ucPersonAlias.PersonAliasList;
            }
            set
            {
                ucPersonAlias.PersonAliasList = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (_gridCustomPaging.IsNull())
                {
                    var serializer = new XmlSerializer(typeof(CustomPagingArgsContract));
                    TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.ORDER_QUEUE_SESSION_KEY]));
                    _gridCustomPaging = (CustomPagingArgsContract)serializer.Deserialize(reader);
                }
                return _gridCustomPaging;
            }
        }

        /// <summary>
        /// get object of shared class of search contract
        /// </summary>
        //public SearchItemDataContract GridSearchContract
        //{
        //    get
        //    {
        //        if (_gridSearchContract.IsNull())
        //        {
        //            var serializer = new XmlSerializer(typeof(SearchItemDataContract));
        //            TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.SEARCH_OBJECT_SESSION_KEY]));
        //            _gridSearchContract = (SearchItemDataContract)serializer.Deserialize(reader);
        //        }
        //        return _gridSearchContract;
        //    }
        //}

        public OrderApprovalQueueContract GridSearchContract
        {
            get
            {
                if (_gridSearchContract.IsNull())
                {
                    var serializer = new XmlSerializer(typeof(OrderApprovalQueueContract));
                    TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.SEARCH_OBJECT_SESSION_KEY]));
                    _gridSearchContract = (OrderApprovalQueueContract)serializer.Deserialize(reader);
                }
                return _gridSearchContract;
            }
        }

        /// <summary>
        /// Sets or gets the next order Id for which details need to be displayed.
        /// </summary>
        public Int32 NextOrderId
        {
            get
            {
                if (_nextOrderId > 0)
                {
                    return _nextOrderId;
                }
                return 0;
            }
            set
            {
                _nextOrderId = value;
            }
        }

        /// <summary>
        /// Sets or gets the Order Id for the first time detail page was loaded from the Queue.
        /// </summary>
        public Int32 FirstOrderId
        {
            get
            {
                if (!ViewState["FirstOrderId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["FirstOrderId"]);
                }
                return 0;
            }
            set
            {
                ViewState["FirstOrderId"] = value;
            }
        }

        /// <summary>
        /// Sets or gets the property to show approve rejects buttons on the screen.
        /// </summary>
        public Boolean ShowApproveRejectButtons
        {
            get
            {
                if (!ViewState["ShowApproveRejectButtons"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["ShowApproveRejectButtons"]);
                }
                return false;
            }
            set
            {
                ViewState["ShowApproveRejectButtons"] = value;
            }
        }

        public String WorkQueuePath
        {
            get
            {
                if (ShowApproveRejectButtons)
                {
                    return IsParentBkgQueue()
                           ? ChildControls.BkgOrderQueue
                           : ChildControls.OrderQueue;
                }
                return ChildControls.OrderHistory;
            }
        }

        //UAT-4537
        /// <summary>
        /// To set the controls for approving payments.
        /// </summary>
        public Boolean ApprovePaymentSettings
        {
            set
            {
                /*
                dvPaymentRejection.Visible = value;
                txtReferenceNumber.Enabled = value;
                spRefNo.Visible = value;*/
                hdfShowApprovePaymentSetting.Value = value.ToString();
                /*
                //UAT - 685 If Order Approval Process Already Initiated then disable approve buttons
                if (value && ComplianceDataManager.GetScheduleTasksToProcess(SelectedTenantId, TaskType.INVOICEORDERBULKAPPROVE.GetStringValue()).Any(obj => obj.RecordID == OrderId))
                {
                    dvPaymentRejection.Disabled = true;
                    btnRejectPayment.ToolTip = "Order already queued for payment approval.";
                    btnRejectPayment.Enabled = false;
                    btnApprovePayment.ToolTip = "Order already queued for payment approval.";
                    btnApprovePayment.Enabled = false;
                }
                else
                {
                    dvPaymentRejection.Disabled = false;
                    btnRejectPayment.ToolTip = "";
                    btnRejectPayment.Enabled = true;
                    btnApprovePayment.ToolTip = "";
                    btnApprovePayment.Enabled = true;
                }*/
            }
        }

        //UAT-916
        ///// <summary>
        ///// Sets or gets the order payment details object.
        ///// </summary>
        //public OrderPaymentDetail OrderPaymentDetail
        //{
        //    get
        //    {
        //        if (ViewState["OrderPaymentDetail"].IsNotNull())
        //        {
        //            return ViewState["OrderPaymentDetail"] as OrderPaymentDetail;
        //        }
        //        return null;
        //    }
        //    set
        //    {
        //        ViewState["OrderPaymentDetail"] = value;
        //    }
        //}

        public String ParentControl
        {
            get
            {
                if (ViewState["ParentControl"].IsNotNull())
                {
                    return Convert.ToString(ViewState["ParentControl"]);
                }
                return null;
            }
            set
            {
                ViewState["ParentControl"] = value;
            }
        }

        public Boolean IsInvoiceOnly
        {
            get;
            set;
        }

        #region Page Controls Properties

        public Int32 OrderId
        {
            get
            {
                if (!ViewState["OrderId"].IsNull())
                {
                    return (Int32)ViewState["OrderId"];
                }
                return 0;
            }
            set
            {
                ViewState["OrderId"] = value;
            }
        }

        public String TextOrderId
        {
            set
            {
                txtOrderId.Text = value;
            }
        }

        public String OrderNumber
        {
            get
            {
                if (!ViewState["OrderNumber"].IsNull())
                {
                    return (String)ViewState["OrderNumber"];
                }
                return String.Empty;
            }
            set
            {
                ViewState["OrderNumber"] = value;
            }
        }

        public String OrderDate
        {
            set
            {
                String orderDate = value;
                txtOrderDate.Text = orderDate.ToString(CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
            }
        }

        public String SubscriptionStartDate
        {
            set
            {
                txtSubsStartDt.Text = value;
            }
        }

        public String SubscriptionExpirationDate
        {
            set
            {
                txtSubsExpirationDt.Text = value;
            }
        }

        public String TotalOrderValue
        {
            get
            {
                return txtTotalOrderValue.Text.Trim();
            }
            set
            {
                txtTotalOrderValue.Text = value;
            }
        }

        public String RushOrderPrice
        {
            get
            {
                return txtRushOrderPrice.Text.Trim();
            }
            set
            {
                dvRushOrderFields.Visible = true;
                dvRushOrder.Visible = true;
                txtRushOrderPrice.Text = value;
            }
        }

        public String DuePayment
        {
            set
            {
                dvDuePayment.Visible = true;
                txtDuePayment.Text = value;
            }
        }

        public String GrandTotalPrice
        {
            get
            {
                return txtGrandTotal.Text.Trim();
            }
            set
            {
                txtGrandTotal.Text = value;
            }
        }

        public String OrderStatus
        {
            set
            {
                //UAT-916
                //txtOrderStatus.Text = value;
            }
        }

        public String RushOrderStatus
        {
            set
            {
                dvRushOrderFields.Visible = true;
                txtRushOrderStatus.Text = value;
                dvRushOrderStatus.Visible = true;
            }
        }

        public String InstituteHierarchy
        {
            get
            {
                return txtHierarchy.Text.Trim();
            }
            set
            {
                txtHierarchy.Text = value;
            }
        }

        public String FirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        public String MiddleName
        {
            set
            {
                txtMiddleName.Text = value;
            }
        }

        public String LastName
        {
            get
            {
                return txtLastName.Text;
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        //public String Alias1
        //{
        //    set
        //    {
        //        txtAlias1.Text = value;
        //    }
        //}

        //public String Alias2
        //{
        //    set
        //    {
        //        txtAlias2.Text = value;
        //    }
        //}

        //public String Alias3
        //{
        //    set
        //    {
        //        txtAlias3.Text = value;
        //    }
        //}

        public String Gender
        {
            set
            {
                if (value.IsNullOrEmpty())
                    value = AppConsts.NOT_AVAILABLE;
                txtGender.Text = value;
            }
        }

        public String DateOfBirth
        {
            set
            {
                if (value.IsNullOrEmpty())
                    value = AppConsts.NOT_AVAILABLE;
                txtDateOfBirth.Text = value;
            }
        }

        public String SocialSecurityNumber
        {

            get
            {
                if (ViewState["SocialSecurityNumber"].IsNotNull())
                {
                    return (string)ViewState["SocialSecurityNumber"];
                }
                return string.Empty;
            }
            set
            {
                ViewState["SocialSecurityNumber"] = value; txtSSN.Text = value;
            }


            //set
            //{
            //    txtSSN.Text = value;
            //}
        }

        public String SocialSecurityNumberMaskedOnly
        {
            set
            {
                txtSSNMAsked.Text = value;
            }
        }
        public String PrimaryEmail
        {
            set
            {
                txtEmail.Text = value;
            }
        }

        public String SecondaryEmail
        {
            set
            {
                txtSecondaryEmail.Text = value;
            }
        }

        public String Phone
        {
            set
            {
                txtPhone.Text = value;
                txtPhoneUnMasking.Visible = false;
            }
        }

        public String SecondaryPhone
        {
            set
            {
                txtSecondaryPhone.Text = value;
                txtSecondaryPhoneUnMasking.Visible = false;
            }
        }

        #region UAT-2447
        public String PhoneUnMasking
        {
            set
            {
                txtPhoneUnMasking.Text = value;
                txtPhone.Visible = false;
            }
        }

        public String SecondaryPhoneUnMasking
        {
            set
            {
                txtSecondaryPhoneUnMasking.Text = value;
                txtSecondaryPhone.Visible = false;
            }
        }
        public Boolean IsRevokedAppointment
        {
            get
            {
                if (ViewState["IsRevokedAppointment"] == null)
                {
                    ViewState["IsRevokedAppointment"] = false;
                }
                return Convert.ToBoolean(ViewState["IsRevokedAppointment"]);
            }
            set
            {
                ViewState["IsRevokedAppointment"] = value;
            }
        }
        #endregion

        public String Address1
        {
            get
            {
                if (!ViewState["Address1"].IsNull())
                {
                    return (string)ViewState["Address1"];
                }

                return AppConsts.NOT_AVAILABLE;
            }
            set
            {
                ViewState["Address1"] = value; txtAddress1.Text = value;
            }



            //set
            //{
            //    if (value.IsNullOrEmpty())
            //        value = AppConsts.NOT_AVAILABLE;
            //    txtAddress1.Text = value;
            //}
        }

        public String Address2
        {
            get
            {
                if (!ViewState["Address2"].IsNull())
                {
                    return (string)ViewState["Address2"];
                }

                return AppConsts.NOT_AVAILABLE;
            }
            set
            {
                ViewState["Address2"] = value; txtAddress2.Text = value;
            }


            //set
            //{
            //    if (value.IsNullOrEmpty())
            //        value = AppConsts.NOT_AVAILABLE;
            //    txtAddress2.Text = value;
            //}
        }

        public String City
        {
            get
            {
                if (!ViewState["City"].IsNull())
                {
                    return (string)ViewState["City"];
                }

                return AppConsts.NOT_AVAILABLE;
            }
            set
            {
                ViewState["City"] = value; txtCity.Text = value;
            }



            //set
            //{
            //    if (value.IsNullOrEmpty())
            //        value = AppConsts.NOT_AVAILABLE;
            //    txtCity.Text = value;
            //}
        }

        public String State
        {
            get
            {
                if (!ViewState["State"].IsNull())
                {
                    return (string)ViewState["State"];
                }

                return AppConsts.NOT_AVAILABLE;
            }
            set
            {
                ViewState["State"] = value; txtState.Text = value;
            }
            //set
            //{
            //    if (value.IsNullOrEmpty())
            //        value = AppConsts.NOT_AVAILABLE;
            //    txtState.Text = value;
            //}
        }

        public String Zip
        {
            get
            {
                if (!ViewState["Zip"].IsNull())
                {
                    return (string)ViewState["Zip"];
                }

                return AppConsts.NOT_AVAILABLE;
            }
            set
            {
                ViewState["Zip"] = value; txtZip.Text = value;
            }


            //set
            //{
            //    if (value.IsNullOrEmpty())
            //        value = AppConsts.NOT_AVAILABLE;
            //    txtZip.Text = value;
            //}
        }

        public String Package
        {
            set
            {
                txtPackage.Text = value;
            }
        }

        public String DurationMonths
        {
            get
            {
                return txtDurationMnths.Text.Trim();
            }
            set
            {
                txtDurationMnths.Text = value;
            }
        }

        public Boolean ShowApprovePayment
        {
            get
            {
                return _showApprovePayment;
            }
            set
            {
                _showApprovePayment = value;
            }
        }

        public Boolean ShowApproveCancellation
        {
            get
            {
                return _showApproveCancellation;
            }
            set
            {
                _showApproveCancellation = value;
            }
        }

        public String PaymentType
        {
            set
            {
                //UAt-916
                //txtPaymentType.Text = value;
            }
        }

        public String ReferenceNumber
        {
            get;
            set;

        }

        public String RejectionReason
        {
            get
            {
                return txtRejectionReason.Text.Trim();
            }
            set
            {
                txtRejectionReason.Text = value;
            }
        }

        public String RejectionPaymentReason
        {
            get;
            set;
        }


        public OrganizationUserProfile OrganizationUserProfile
        {
            get
            {
                if (!ViewState["OrganizationUserProfile"].IsNull())
                {
                    return (OrganizationUserProfile)ViewState["OrganizationUserProfile"];
                }
                return new OrganizationUserProfile();
            }
            set
            {
                ViewState["OrganizationUserProfile"] = value;
            }
        }

        public Int32 DPPSId
        {
            get
            {
                if (!ViewState["DPPSId"].IsNull())
                {
                    return (Int32)ViewState["DPPSId"];
                }
                return 0;
            }
            set
            {
                ViewState["DPPSId"] = value;
            }
        }

        public Boolean ShowOfflineSettlement
        {
            get
            {
                return _showOfflineSettlement;
            }
            set
            {
                _showOfflineSettlement = value;
            }
        }

        public Boolean OfflineSettlementSettings
        {
            set
            {
                hdfShowOffLineSettlement.Value = value.ToString();
                if (OrderStatusCode.Equals(ApplicantOrderStatus.Paid.GetStringValue()))
                {
                    /* UAT-916
                    dvPaymentRejection.Visible = value;
                    txtReferenceNumber.Enabled = false;
                    spRefNo.Visible = false;
                    btnRejectPayment.Visible = false;
                    divRejectionReason.Visible = false;
                    btnApprovePayment.Visible = false;
                    btnCancelOrder.Visible = value;*/
                }
                else if (OrderStatusCode.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()))
                {
                    /* UAt-916
                    dvPaymentRejection.Visible = value;*/
                    //txtReferenceNumber.Enabled = false;
                    // rfvReferenceNumber.Enabled = false;
                    //spRefNo.Visible = false;
                    /*UAT-916
                    divRejectionReason.Visible = false;
                    btnRejectPayment.Visible = false;
                    btnCancelOrder.Visible = value;*/
                }
                else if (OrderStatusCode.Equals(ApplicantOrderStatus.Payment_Rejected.GetStringValue()))
                {
                    /*UAT-916
                    dvPaymentRejection.Visible = value;
                    txtReferenceNumber.Enabled = value;
                    spRefNo.Visible = value;
                    btnRejectPayment.Visible = false;
                    divRejectionReason.Visible = false;
                    btnCancelOrder.Visible = false;*/
                }
                else if (OrderStatusCode.Equals(ApplicantOrderStatus.Payment_Due.GetStringValue()))
                {
                    /*UAT-916
                    dvPaymentRejection.Visible = value;*/
                    //txtReferenceNumber.Enabled = value;
                    //spRefNo.Visible = value;
                    /*btnCancelOrder.Visible = value;*/
                }
                /* UAT-916
                if (OrderStatusCode.Equals(ApplicantOrderStatus.Payment_Due.GetStringValue())
                    || OrderStatusCode.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue())
                   || CurrentViewContext.PaymentTypeCode == PaymentOptions.Credit_Card.GetStringValue()
                   || CurrentViewContext.PaymentTypeCode == PaymentOptions.Paypal.GetStringValue()
                    )
                {
                    divReferenceNumber.Visible = false;
                }


                // UAT 285: Reference Number field to be optional for Invoice
                if (CurrentViewContext.PaymentTypeCode == PaymentOptions.InvoiceWithApproval.GetStringValue())
                {
                    //txtReferenceNumber.Attributes.Add("readonly", "readonly");
                    rfvReferenceNumber.Enabled = false;
                    spRefNo.Visible = false;
                }*/
            }
        }


        /// <summary>
        /// Used to get the CustomerProfileId, for Refund process
        /// </summary>
        public Guid UserId
        {
            get
            {
                return ViewState["UserId"].IsNullOrEmpty() ? new Guid() : new Guid(Convert.ToString(ViewState["UserId"]));
            }
            set
            {
                ViewState["UserId"] = value;
            }
        }

        /*UAT-916
         * /// <summary>
         /// Property to store Online TransactionId, used for Refund process 
         /// </summary>
         public String TransactionId
         {
             get
             {
                 return ViewState["TransactionId"].IsNullOrEmpty() ? String.Empty : ViewState["TransactionId"].ToString();
             }
             set
             {
                 ViewState["TransactionId"] = value;
             }
         }

         /// <summary>
         /// Property to store CCNumber, used for Refund process 
         /// </summary>
         public String CCNumber
         {
             get
             {
                 return ViewState["CCNumber"].IsNullOrEmpty() ? String.Empty : ViewState["CCNumber"].ToString();
             }
             set
             {
                 ViewState["CCNumber"] = value;
             }
         }

         /// <summary>
         /// Property to store InvoiceNumber, used for Refund process 
         /// </summary>
         public String InvoiceNumber
         {
             get
             {
                 return ViewState["InvoiceNumber"].IsNullOrEmpty() ? String.Empty : ViewState["InvoiceNumber"].ToString();
             }
             set
             {
                 ViewState["InvoiceNumber"] = value;
             }
         }*/


        #endregion

        public List<ServiceFormContract> lstServiceForm
        {
            get;
            set;
        }

        public Int32 PaymentMode_InvoiceWithoutApprovalId
        {
            get
            {
                return (Int32)(ViewState["PaymentMode_InvoiceWithoutApprovalId"] ?? "0");
            }
            set
            {
                ViewState["PaymentMode_InvoiceWithoutApprovalId"] = value;
            }
        }

        public Int32 PaymentMode_InvoicetoInstitutionId
        {
            get
            {
                return (Int32)(ViewState["PaymentMode_InvoicetoInstitutionId"] ?? "0");
            }
            set
            {
                ViewState["PaymentMode_InvoicetoInstitutionId"] = value;
            }
        }

        #region UAT-796
        public Boolean AutomaticRenewalTurnedOff
        {
            get
            {
                return Convert.ToBoolean(ViewState["AutomaticRenewalTurnedOff"]);
            }
            set
            {
                ViewState["AutomaticRenewalTurnedOff"] = value;
            }
        }
        public Boolean ShowAutoRenewalControl
        {
            get
            {
                return Convert.ToBoolean(ViewState["ShowAutoRenewalControl"]);
            }
            set
            {
                ViewState["ShowAutoRenewalControl"] = value;
            }
        }
        public Boolean DisableAutoRenewalControl
        {
            get
            {
                return Convert.ToBoolean(ViewState["DisableAutoRenewalControl"]);
            }
            set
            {
                ViewState["DisableAutoRenewalControl"] = value;
            }
        }


        #endregion
        /*
        get
           {
               if (ViewState["ServiceForms"].IsNotNull())
               {
                   return (List<ServiceFormContract>)(ViewState["ServiceForms"]);
               }
               return null;
           }
           set
           {
               ViewState["ServiceForms"] = value;
           }
        */
        #region UAT-806 Creation of granular permissions for Client Admin users

        public String SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }
        public Boolean IsDOBDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDOBDisabled"] ?? false);
            }
            set
            {
                ViewState["IsDOBDisabled"] = value;
            }
        }
        #endregion

        /// <summary>
        /// Property to differentiate whether the user has navigated from the BkgQueue or Compliance Queue
        /// </summary>
        private String ParentQueueType
        {
            get
            {
                return !String.IsNullOrEmpty(Convert.ToString(ViewState[AppConsts.PARENT_QUEUE_TYPE_VIEWSTATE]))
                    ? Convert.ToString(ViewState[AppConsts.PARENT_QUEUE_TYPE_VIEWSTATE]) : AppConsts.QUEUE_TYPE_BKGORDER_QUEUE;
            }
            set
            {
                ViewState[AppConsts.PARENT_QUEUE_TYPE_VIEWSTATE] = value;
            }
        }

        public List<BackgroundPackagesContract> lstExternalPackages
        {
            get;
            set;
        }

        public DeptProgramPackageSubscription SelectedPackageDetails
        {
            get;
            set;
        }

        public int BkgOrderID
        {
            get
            {
                if (!ViewState["BkgOrderId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["BkgOrderId"]);
                }
                return 0;
            }
            set
            {
                ViewState["BkgOrderId"] = value;
            }
        }

        public DataTable BkgPackagesList
        {
            get;
            set;
        }

        public Int32? OrderPackageType
        {
            get;
            set;
        }

        public String OrderPackageTypeCode
        {
            get;
            set;
        }

        Boolean IOrderPaymentDetailsView.IsCompliancePartialOrderCancelled
        {
            get;
            set;
        }

        String IOrderPaymentDetailsView.PartialOrderCancellationXML
        {
            get;
            set;
        }

        String IOrderPaymentDetailsView.PartialOrderCancellationTypeCode
        {
            get;
            set;
        }

        #region UAT-916 : WB: As an application admin, I should be able to define payment options at the package level in addition to the node level
        public List<OrderPkgPaymentDetail> OrderPkgPaymentDetailList
        {
            get
            {
                if (!ViewState["OrderPkgPaymentDetailList"].IsNull())
                {
                    return ViewState["OrderPkgPaymentDetailList"] as List<OrderPkgPaymentDetail>;
                }
                return new List<OrderPkgPaymentDetail>();
            }
            set
            {
                ViewState["OrderPkgPaymentDetailList"] = value;
            }
        }

        public List<OrderPaymentDetail> OrderPaymentDetailList
        {
            get
            {
                if (ViewState["OrderPaymentDetailList"].IsNotNull())
                {
                    return ViewState["OrderPaymentDetailList"] as List<OrderPaymentDetail>;
                }
                return null;
            }
            set
            {
                ViewState["OrderPaymentDetailList"] = value;
            }
        }

        OrderPaymentDetail IOrderPaymentDetailsView.CompPkgOrderPaymentDetail
        {
            get
            {
                if (ViewState["CompPkgOrderPaymentDetail"].IsNotNull())
                {
                    return ViewState["CompPkgOrderPaymentDetail"] as OrderPaymentDetail;
                }
                return null;
            }
            set
            {
                ViewState["CompPkgOrderPaymentDetail"] = value;
            }
        }

        String IOrderPaymentDetailsView.SetCompliancePkgPaymentType
        {
            set
            {
                txtCompPkgPaymentType.Text = value;
            }
        }

        public Int32 OrderPaymentDetailID
        {
            get;
            set;
        }

        public String OrderPaymentDetailStatusCode
        {
            get
            {
                if (!ViewState["OrderPaymentDetailStatusCode"].IsNull())
                {
                    return Convert.ToString(ViewState["OrderPaymentDetailStatusCode"]);
                }
                return null;
            }
            set
            {
                ViewState["OrderPaymentDetailStatusCode"] = value;
            }
            //get;
            //set;
        }

        public Boolean IsCompliancePackageInclude
        {
            get;
            set;
        }
        Decimal IOrderPaymentDetailsView.OrderPaymentAmount
        {
            get;
            set;
        }

        #endregion

        String IOrderPaymentDetailsView.PackageHeading
        {
            set
            {
                //spnPackageHeading.InnerHtml = value;
                spnPackageHeading.InnerText = value;
            }
        }

        #region UAT-1189:If payment method is the same, both tracking and screening are getting cancelled when the applicant attempts to cancel the tracking order.

        /// <summary>
        /// Property return boolean value for cancellation of compliance package due to change subscription
        /// </summary>
        Boolean IOrderPaymentDetailsView.IsCompliancePackageCancelledByChangeSubs { get; set; }
        #endregion

        //UAT-1558
        public String ArchiveStateCode
        {
            get
            {
                if (!ViewState["ArchiveStateCode"].IsNull())
                    return Convert.ToString(ViewState["ArchiveStateCode"]);
                return "";
            }
            set
            {
                ViewState["ArchiveStateCode"] = value;
            }
        }

        public String BkgArchiveStateCode
        {
            get
            {
                if (!ViewState["BkgArchiveStateCode"].IsNull())
                    return Convert.ToString(ViewState["BkgArchiveStateCode"]);
                return "";
            }
            set
            {
                ViewState["BkgArchiveStateCode"] = value;
            }
        }

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

        #region UAT-2212: Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality

        String IOrderPaymentDetailsView.NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                if (SecurityManager.IsLocationServiceTenant(TenantId))
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }
        #endregion


        //UAT-2384
        Boolean IOrderPaymentDetailsView.IsClientAdmin
        {
            get;
            set;
        }


        //UAT-2971
        public Int32 ApplicantOrgUserID
        {
            get
            {
                if (!ViewState["ApplicantOrgUserID"].IsNull())
                {
                    return (Int32)ViewState["ApplicantOrgUserID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ApplicantOrgUserID"] = value;
            }
        }

        public Boolean IsItemPaymentOrder
        {
            get
            {
                if (!ViewState["IsItemPaymentOrder"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsItemPaymentOrder"]);
                }
                return false;
            }
            set
            {
                ViewState["IsItemPaymentOrder"] = value;
            }
        }


        public OnlinePaymentTransaction OnlinePaymentTransaction
        {
            get
            {
                if (ViewState["OnlinePaymentTransaction"] != null)
                {
                    return (OnlinePaymentTransaction)ViewState["OnlinePaymentTransaction"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["OnlinePaymentTransaction"] = value;
            }
        }

        public Decimal OnlinePaymentAmount
        {
            get
            {
                if (ViewState["OnlinePaymentAmount"] == null)
                {
                    ViewState["OnlinePaymentAmount"] = 0;
                }
                return (Decimal)ViewState["OnlinePaymentAmount"];
            }
            set
            {
                ViewState["OnlinePaymentAmount"] = value;
            }
        }

        public Order OrderDetail
        {
            get
            {
                if (ViewState["OrderDetail"] == null)
                {
                    return null;
                }
                return (Order)ViewState["OrderDetail"];
            }
            set
            {
                ViewState["OrderDetail"] = value;
            }
        }
        public Boolean IsFileSentToCBI
        {
            get
            {
                if (ViewState["IsFileSentToCBI"] == null)
                {
                    return false;
                }
                return (Boolean)ViewState["IsFileSentToCBI"];
            }
            set
            {
                ViewState["IsFileSentToCBI"] = value;
            }

        }

        String IOrderPaymentDetailsView.LanguageCode
        {
            get
            {
                LanguageContract languageContract = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!languageContract.IsNullOrEmpty())
                    return languageContract.LanguageCode;
                return Languages.ENGLISH.GetStringValue();
            }
        }

        #region UAT-3632
        String IOrderPaymentDetailsView.PaymentItemName
        {
            get
            {
                if (!ViewState["PaymentItemName"].IsNull())
                    return Convert.ToString(ViewState["PaymentItemName"]);
                return String.Empty;
            }
            set
            {
                ViewState["PaymentItemName"] = value;
            }
        }

        Boolean IOrderPaymentDetailsView.IsRequirementItemPayment
        {
            get
            {
                if (!ViewState["IsRequirementItemPayment"].IsNull())
                    return Convert.ToBoolean(ViewState["IsRequirementItemPayment"]);
                return false;
            }
            set
            {
                ViewState["IsRequirementItemPayment"] = value;
            }
        }

        String IOrderPaymentDetailsView.ItemPaymentCIDOROrderID
        {
            get
            {
                if (!ViewState["ItemPaymentCIDOROrderID"].IsNull())
                    return Convert.ToString(ViewState["ItemPaymentCIDOROrderID"]);
                return String.Empty;
            }
            set
            {
                ViewState["ItemPaymentCIDOROrderID"] = value;
            }
        }

        #endregion

        //UAT-3335
        SharedUserDashboardDetailsContract IOrderPaymentDetailsView.SharedUserDetails { get; set; }

        #region UAT - 3636
        String IOrderPaymentDetailsView.TrackingPkgCancelledBy
        {
            get
            {
                if (ViewState["TrackingPkgCancelledBy"] == null)
                {
                    return null;
                }
                return Convert.ToString(ViewState["TrackingPkgCancelledBy"]);
            }
            set
            {
                ViewState["TrackingPkgCancelledBy"] = value;
            }
        }
        #endregion

        //String IOrderPaymentDetailsView.CompliancePkgCancelledBy
        //{
        //    get
        //    {
        //        if (ViewState["CompliancePkgCancelledBy"] == null)
        //        {
        //            return null;
        //        }
        //        return Convert.ToString(ViewState["CompliancePkgCancelledBy"]);
        //    }
        //    set
        //    {
        //        ViewState["CompliancePkgCancelledBy"] = value;
        //    }
        //}

        //DateTime IOrderPaymentDetailsView.CompliancePkgCancelledOn
        //{
        //    get
        //    {
        //        return Convert.ToDateTime(ViewState["CompliancePkgCancelledOn"]);
        //    }
        //    set
        //    {
        //        ViewState["CompliancePkgCancelledOn"] = value;
        //    }
        //}

        public String CompliancePkgCancelledBy
        {
            set
            {
                txtCompPackageCancelledBy.Text = value;
            }
        }

        public String CompliancePkgCancelledOn
        {
            set
            {
                txtCompPackageCancelledOn.Text = value;
            }
        }

        //public String BkgPkgCancelledBy
        //{
        //    set
        //    {
        //        txtBkgPackageCancelledBy.Text = value;
        //    }
        //}

        //public String BkgPkgCancelledOn
        //{
        //    set
        //    {
        //        txtBkgPackageCancelledOn.Text = value;
        //    }
        //}

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// OnInit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = Resources.Language.ORDRDTLS;
                base.BreadCrumbTitleKey = "Key_ORDRDTLS";
                base.SetPageTitle(Resources.Language.DETAILS);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                divSSN.Visible = false;
                divDOB.Visible = false;
            }

        }
        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;

            if (!this.IsPostBack)
            {


                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    CaptureQuerystringParameters(args);
                }
                //Release 158 CBI
                ucPersonAlias.SelectedTenantId = CurrentViewContext.TenantId > AppConsts.ONE ? CurrentViewContext.TenantId : CurrentViewContext.SelectedTenantId;
                ucPersonAlias.PageType = PersonAliasPageType.OrderPaymentDetails.GetStringValue();
                Presenter.IsLocationServiceTenant();//UAT 3573
                AddSuffix();


                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    lblAddress1.Text = Resources.Language.ADDRESS;
                    dvAddress2.Visible = false;
                }
                else
                {
                    lblAddress1.Text = Resources.Language.ADDRESS1;
                }

                Presenter.OnViewInitialized();
                DisplayApproveButtons();
                CheckOrder();
                List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                ApplyActionLevelPermission(ctrlCollection, "Order Payment Detail");
                GetServiceForms();
                DisplayAutoRenewalButton();
                // 20-05-2014: Show the Order Packages for background Packages and Compliance Packages based on OrderPackageType 
                ShowBackgroundPackages();

                //UAT-966: As an admin, I should be able to cancel individual parts of an order
                ShowPartialOrderCancellationPackages();

                ShowHideRefund();

                ///// true if file already sent to cbi 
                if (CurrentViewContext.IsLocationServiceTenant)
                    Presenter.IsFileSentToCbi();
                //UAT-3521 ||CBI||CABS
                ShowHideAppointmentInfo();

                ucScheduleLocationUpdateControl.TenantId = CurrentViewContext.SelectedTenantId;
                ucScheduleLocationUpdateControl.OrderID = CurrentViewContext.OrderId;
                //   uc_OLI_OrderPaymentDetail.OrderId = CurrentViewContext.OrderId;

                //UAT-3910
                if (CurrentViewContext.IsLocationServiceTenant && txtState.Text.IsNullOrEmpty())
                    dvState.Visible = false;
                else
                    dvState.Visible = true;

                if (IsApplicant)
                {
                    lnkbacksrch.Visible = false;
                }
            }
            ucScheduleLocationUpdateControl.enableDiableSaveButton += new FingerPrintSetUp.Views.AppointmentScheduleLocationUpdate.EnableDiableSaveButton(EnableNextButton);
            ucAppointmentRescheduler.eventEnableSaveButton += new FingerPrintSetUp.Views.AppointmentRescheduler.EnableSaveButton(EnableSaveButton);
            //ucScheduleLocationUpdateControl.
            //UAt-2971
            if (this.ParentQueueType == AppConsts.SUPPORT_PORTAL_DETAIL)
            {
                lnkGoBack.Visible = true;
                lnkbacksrch.Visible = false;
                //btnCancelOrderTmp.Visible = false;
                //btnCancelOrder.Visible = false;
                cbbuttons.Visible = false;
                cbbuttons.CancelButton.Visible = false;
                cbbuttons.ClearButton.Visible = false;
            }

            HideShowServiceLevelDetails();
            //UAT-3166
            Presenter.GetOrderPkgPaymentDetail();



            if (!Presenter.IsAdminLoggedIn() && CurrentViewContext.IsLocationServiceTenant)
            {
                //To get Mailing Address detail.
                Presenter.GetMailingDetail(CurrentViewContext.OrderId);
                //If we are not getting data from table , it should get from XML in case of complete your order
                if (((CurrentViewContext.MailingAddressData).IsNullOrEmpty()) || (CurrentViewContext.MailingAddressData.MailingOption.IsNullOrEmpty()))
                {
                    GetShiippingAddress();
                }
                //Bind mailing data.
                if (!((CurrentViewContext.MailingAddressData).IsNullOrEmpty()) && (CurrentViewContext.MailingAddressData.MailingOption.IsNotNull()))
                {
                    dvMailingAddressDetail.Visible = true;
                    lblMailingOption.Text = CurrentViewContext.MailingAddressData.MailingOption.HtmlEncode();
                    lblMailingAddress.Text = CurrentViewContext.MailingAddressData.Address1.HtmlEncode();
                    lblMailingCity.Text = CurrentViewContext.MailingAddressData.CityName.HtmlEncode();
                    lblMailingCountry.Text = CurrentViewContext.MailingAddressData.CountyName;
                    lblMailingState.Text = CurrentViewContext.MailingAddressData.StateName.HtmlEncode();
                    lblMailingZipCode.Text = CurrentViewContext.MailingAddressData.Zipcode.HtmlEncode();
                    if (CurrentViewContext.MailingAddressData.CountyName.Trim() == "CANADA" || CurrentViewContext.MailingAddressData.CountyName.Trim() == "UNITED STATES of AMERICA - STATE"
                    || CurrentViewContext.MailingAddressData.CountyName.Trim() == "MEXICO" || CurrentViewContext.MailingAddressData.CountyName.Trim() == "UNITED STATES of AMERICA")
                    {
                        lblZipOrPostalCode.Text = Resources.Language.ZIPCODE;
                    }
                    else
                    {
                        lblZipOrPostalCode.Text = Resources.Language.POSTALCODE;
                    }
                }
                //If we are not getting data from table , it should get from XML in case of complete your order
            }
            else
                dvMailingAddressDetail.Visible = false;


            HideShowServiceDetailsLink();
            /* UAT-916
            // UAT 285: Reference Number field to be optional for Invoice
            if (CurrentViewContext.PaymentTypeCode == PaymentOptions.InvoiceWithApproval.GetStringValue())
            {
                //txtReferenceNumber.Attributes.Add("readonly", "readonly");
                rfvReferenceNumber.Enabled = false;
                spRefNo.Visible = false;
            }*/
            if (CurrentViewContext.PersonAliasList.Count > 0)
            {
                dvPersonalAlias.Visible = true;
            }
            Presenter.OnViewLoaded();

            BasePage basePage = base.Page as BasePage;
            if (!basePage.IsNullOrEmpty())
            {
                basePage.SetModuleTitle(Resources.Language.ORDER);
            }

            HideShowControlsForGranularPermission();//UAT-806

            #region UAT-796

            hdnTenantID.Value = CurrentViewContext.SelectedTenantId.ToString();
            hdnOrderID.Value = CurrentViewContext.OrderId.ToString();
            hfCurrentUserID.Value = CurrentViewContext.CurrentLoggedInUserId.ToString();
            #endregion

            HideCancelOrderPermissionForClientAdmin();//UAT-2384
            //UAT-3077
            dvPackageDetails.Visible = !CurrentViewContext.IsItemPaymentOrder;


            LocationBaseSettings();
            ManageSSN();
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Approve Cancellation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApproveCancellation_OnClick(Object sender, EventArgs e)
        {
            try
            {
                //Boolean showSuccessMsg = false;
                String message = Resources.Language.REQCNCLAPRVLSUC;
                CancelOrder(message, true);
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

        /// <summary>
        /// Rejects the cancellation request. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRejectCancellation_Click(object sender, EventArgs e)
        {
            String message = Resources.Language.REQCNCLAPRVLREJSUC;
            if (Presenter.RejectCancellationRequest())
            {
                ShowSuccessMessage(message);
                Presenter.GetOrderDetailsAndSetControls();
                DisplayApproveButtons();
                Presenter.SendOrderCancellationRejectionNotification();
            }
        }

        /// <summary>
        /// Approve Payment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApprovePayment_OnClick(Object sender, EventArgs e)
        {
            try
            {
                //Boolean showSuccessMsg = false;
                String message = String.Empty;
                if (OrderStatusCode.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()))
                {
                    message = Resources.Language.REQONLNPAYAPRVLSUC;
                }
                else if (OrderStatusCode.Equals(ApplicantOrderStatus.Payment_Rejected.GetStringValue()))
                {
                    message = Resources.Language.REQPAYRJCTAPRVLSUC;
                }
                else if (OrderStatusCode.Equals(ApplicantOrderStatus.Payment_Due.GetStringValue()))
                {
                    message = Resources.Language.REQPAYDUEAPRVLSUC;
                }
                else if (OrderStatusCode.Equals(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()))
                {
                    message = Resources.Language.REQPNDNGPAYAPRVLSUC;
                }

                if (Presenter.ApprovePendingOrders())
                {
                    CopyBkgDataToCompliancePackage();
                    #region UAT-1476:When a tracking package is ordered and there was already a previous package with entered data,
                    //then there would be data movement as if there were a subscription change.
                    //UAT_issueFix 06/07/2017 Release 127
                    //CopyCompPackageDataForNewOrder();
                    #endregion
                    //Method to upadte EDS status.
                    Presenter.UpdateEDSStatus();
                    ComplianceDataManager.InsertAutomaticInvitationLog(CurrentViewContext.SelectedTenantId, CurrentViewContext.OrderId, CurrentViewContext.CurrentLoggedInUserId); //UAT-2388
                    if (!CurrentViewContext.IsLocationServiceTenant)
                        Presenter.SendOrderApprovalNotification();
                    ShowSuccessMessage(message);
                    Presenter.GetOrderDetailsAndSetControls();
                    DisplayApproveButtons();
                    //UAT-2970
                    //call SetOrderConfirmationDocForCreditCard web method which will send order confirmartion documnet and message to applicant.
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(),
                        "SetOrderConfirmationMailWithDoc('" + Convert.ToString(CurrentViewContext.TenantId) + "," + Convert.ToString(CurrentViewContext.CurrentLoggedInUserId) + "," + Convert.ToString(CurrentViewContext.OrderId) + "," + Convert.ToString(CurrentViewContext.OrderPaymentDetailID) + "');", true);

                    //UAT-4498
                    Presenter.CopyDataForDummyLineItem();
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

        /// <summary>
        /// Rejects the Approve Pending payment request. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRejectPayment_Click(object sender, EventArgs e)
        {
            try
            {
                String message = Resources.Language.REQPAYRJCTNSUC;
                if (Presenter.RejectPaymentRequest())
                {
                    ShowSuccessMessage(message);
                    Presenter.GetOrderDetailsAndSetControls();
                    DisplayApproveButtons();
                    //Presenter.SendOrderRejectionNotification();
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

        /// <summary>
        /// Move to next record in the Queue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarNext_Click(Object sender, EventArgs e)
        {
            try
            {
                Presenter.GetNextRecordData();
                if (NextOrderId == 0)
                {
                    ResetControlsOnPage();
                    RoutePageBack(false);
                }
                else
                {
                    OrderId = NextOrderId;
                    ResetControlsOnPage();
                    Presenter.GetOrderDetailsAndSetControls();
                    DisplayAutoRenewalButton();
                    ShowBackgroundPackages();

                    //UAT-966: As an admin, I should be able to cancel individual parts of an order
                    ShowPartialOrderCancellationPackages();

                    ShowHideRefund();

                    DisplayApproveButtons();
                    HideShowServiceDetailsLink();
                    /* UAT-916
                    //Bug 6191-Admin is able to approve payment without reference number for “Money Order” payment type when admin click on next button.
                    if (CurrentViewContext.PaymentTypeCode == PaymentOptions.InvoiceWithApproval.GetStringValue())
                    {
                        rfvReferenceNumber.Enabled = false;
                        spRefNo.Visible = false;
                    }
                    else
                    {
                        rfvReferenceNumber.Enabled = true;
                        spRefNo.Visible = true;
                    }*/
                    #region UAT 835 Fix - Order details screen show other incorrect alias information when using "next" button to navigate across orders.

                    if (ucPersonAlias.RebindAlias())
                        dvPersonalAlias.Visible = true;
                    else
                        dvPersonalAlias.Visible = false;

                    #endregion
                }
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


        protected void btnUpdateOrderDetails_Click(object sender, EventArgs e)
        {
            try
            {
                Telerik.Web.UI.RadGrid grd = ((this.Parent.Page).FindControl("grdOrderHistory") as Telerik.Web.UI.RadGrid);
                if (!grd.IsNullOrEmpty())
                {
                    grd.Rebind();
                }

                //grdOrderHistory.Rebind();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Cancel Alert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(Object sender, EventArgs e)
        {
            try
            {
                ResetControlsOnPage();

                if (ParentControl.IsNotNull())
                {
                    RedirectToDashboard();
                }
                else
                {
                    RoutePageBack(false);
                }
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

        /// <summary>
        /// To change payment type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkChangePaymentType_click(Object sender, EventArgs e)
        {
            try
            {
                //UAT-916 
                LinkButton lnkChangePaymentType = (LinkButton)(sender);
                ChangePaymentType(lnkChangePaymentType);
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

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            try
            {
                String message = Resources.Language.REQCNCLTNSUC;
                CancelOrder(message, false);
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

        protected void btnPartialOrderCancellation_Click(object sender, EventArgs e)
        {
            try
            {
                //List<Int32> partialCancellationBkgPackageOrderID = new List<Int32>();

                StringBuilder packageNames = new StringBuilder();
                hdnPartialOrderCancellationAmount.Value = "0";
                String partialOrderCancellationTypeCode = String.Empty;
                StringBuilder partialOrderCancellationXML = new StringBuilder();
                partialOrderCancellationXML.Append("<PartialOrderCancellationDetails>");

                Boolean isAllBkgPkgChecked = true;
                foreach (RepeaterItem item in rptBackgroundPackages.Items)
                {
                    WclButton chkPartialCancelBkgPkg = (WclButton)item.FindControl("chkPartialCancelBkgPkg");
                    Boolean ischkPartialCancelBkgPkgVisible = chkPartialCancelBkgPkg.Visible;
                    Boolean ischkPartialCancelBkgPkgChecked = chkPartialCancelBkgPkg.Checked;

                    if (ischkPartialCancelBkgPkgVisible && !ischkPartialCancelBkgPkgChecked)
                    {
                        isAllBkgPkgChecked = ischkPartialCancelBkgPkgChecked;
                    }

                    if (ischkPartialCancelBkgPkgVisible && ischkPartialCancelBkgPkgChecked)
                    {
                        String bkgOrderPackageID = ((HiddenField)item.FindControl("hdnBkgOrderPackageID")).Value;
                        //partialCancellationBkgPackageOrderID.Add(Convert.ToInt32(bkgOrderPackageID));
                        CreatePartialOrderCancellationXML(partialOrderCancellationXML, Convert.ToInt32(bkgOrderPackageID), false);
                        partialOrderCancellationTypeCode = PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue();
                        //add backgrounnd package name to packageName list here. Fetch it from txtBkgPackage 
                        String txtBkgPackage = ((WclTextBox)item.FindControl("txtBkgPackage")).Text;
                        packageNames.Append(txtBkgPackage + ", ");
                    }
                }

                Boolean partialCancelCompliancePkgChecked = chkPartialCancelCompliancePkg.Checked;

                if (chkPartialCancelCompliancePkg.Visible && partialCancelCompliancePkgChecked)
                {
                    CreatePartialOrderCancellationXML(partialOrderCancellationXML, CurrentViewContext.OrderId, true);
                    partialOrderCancellationTypeCode = partialOrderCancellationTypeCode.IsNullOrEmpty()
                                                       ? PartialOrderCancellationType.COMPLIANCE_PACKAGE.GetStringValue()
                                                       : PartialOrderCancellationType.COMPLIANCE_BACKGROUND_PACKAGES.GetStringValue();
                    //add comliance package name to packageName list here. Fetch it from txtPackage
                    packageNames.Append(txtPackage.Text + ", ");
                }
                String generatedPackageNames = Convert.ToString(packageNames);
                if (!generatedPackageNames.IsNullOrEmpty())
                {
                    generatedPackageNames = generatedPackageNames.Substring(0, generatedPackageNames.LastIndexOf(','));
                }

                if (!chkPartialCancelCompliancePkg.Visible)
                {
                    partialCancelCompliancePkgChecked = true;
                    CurrentViewContext.IsCompliancePartialOrderCancelled = true;
                }

                if (isAllBkgPkgChecked && partialCancelCompliancePkgChecked)
                {
                    base.ShowInfoMessage(Resources.Language.CANTCNCLALLPKG);
                    return;
                }

                if (String.IsNullOrEmpty(partialOrderCancellationTypeCode))
                {
                    base.ShowInfoMessage(Resources.Language.PLZSELPKGCNCLN);
                    return;
                }

                partialOrderCancellationXML.Append("</PartialOrderCancellationDetails>");

                CurrentViewContext.PartialOrderCancellationTypeCode = partialOrderCancellationTypeCode;
                CurrentViewContext.PartialOrderCancellationXML = partialOrderCancellationXML.ToString();
                Presenter.SavePartialOrderCancellation();
                Presenter.GetOrderDetailsAndSetControls();
                Presenter.SendPartialOrderCancellationNotification(generatedPackageNames);
                ShowBackgroundPackages();
                ShowPartialOrderCancellationPackages();
                ShowHideRefund();
                base.ShowSuccessMessage(Resources.Language.PRTLORDPKGCNCLSUC);
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

        /// <summary>
        /// Event for the Refund button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefund_Click(object sender, EventArgs e)
        {
            try
            {
                WclButton buttonRefund = (WclButton)(sender);
                RepeaterItem rptItem = buttonRefund.NamingContainer as RepeaterItem;
                if (rptItem.IsNotNull())
                {

                    WclNumericTextBox txtOriginalPrice = (rptItem.FindControl("txtOriginalPrice") as WclNumericTextBox);
                    WclNumericTextBox txtRefundAmount = (rptItem.FindControl("txtRefundAmount") as WclNumericTextBox);
                    WclNumericTextBox txtTotalRefund = (rptItem.FindControl("txtTotalRefund") as WclNumericTextBox);
                    WclNumericTextBox txtNetPrice = (rptItem.FindControl("txtNetPrice") as WclNumericTextBox);

                    HtmlGenericControl divEachOrderRefund = (rptItem.FindControl("divEachOrderRefund") as HtmlGenericControl);

                    HiddenField hdfOrderPaymentdetailID = (rptItem.FindControl("hdfOrderPaymentdetailID") as HiddenField);
                    HiddenField hdfOrderStatusTypeCode = (rptItem.FindControl("hdfOrderStatusTypeCode") as HiddenField);
                    HiddenField hdfTransactionId = (rptItem.FindControl("hdfTransactionId") as HiddenField);
                    HiddenField hdfCCNumber = (rptItem.FindControl("hdfCCNumber") as HiddenField);
                    HiddenField hdfInvoiceNumber = (rptItem.FindControl("hdfInvoiceNumber") as HiddenField);

                    /*UAT-916
                     * if (hdnIsPartialOrderCancellation.Value == "1" &&  Convert.ToDecimal(txtRefundAmount.Text) > Convert.ToDecimal(hdnPartialOrderCancellationAmount.Value)
                        && (CurrentViewContext.OrderStatusCode != ApplicantOrderStatus.Cancelled.GetStringValue()
                            && CurrentViewContext.OrderStatusCode != ApplicantOrderStatus.Cancellation_Requested.GetStringValue()))*/
                    if (hdnIsPartialOrderCancellation.Value == "1" && txtRefundAmount.IsNotNull() && hdfOrderStatusTypeCode.IsNotNull()
                        && Convert.ToDecimal(txtRefundAmount.Text) > Convert.ToDecimal(hdnPartialOrderCancellationAmount.Value)
                       && (hdfOrderStatusTypeCode.Value != ApplicantOrderStatus.Cancelled.GetStringValue()
                           && hdfOrderStatusTypeCode.Value != ApplicantOrderStatus.Cancellation_Requested.GetStringValue()))
                    {
                        base.ShowInfoMessage(Resources.Language.RFNDAMTCNTGRTRTOTPRC);
                        return;
                    }

                    /*UAT-916
                     * if (!CurrentViewContext.CCNumber.IsNullOrEmpty() && !CurrentViewContext.TransactionId.IsNullOrEmpty())*/
                    if (hdfCCNumber.IsNotNull() && hdfTransactionId.IsNotNull() && !hdfCCNumber.Value.IsNullOrEmpty() && !hdfTransactionId.Value.IsNullOrEmpty()
                        && hdfOrderPaymentdetailID.IsNotNull() && Convert.ToInt32(hdfOrderPaymentdetailID.Value) > AppConsts.NONE)
                    {
                        Entity.AuthNetCustomerProfile customerProfile = Presenter.GetCustomerProfile(UserId);
                        String _description = Presenter.GenerateDescription();

                        /*UAT-916
                         * INTSOF.AuthNet.Business.CustomerProfileWS.CreateCustomerProfileTransactionResponseType _response = INTSOF.AuthNet.Business.AuthorizeNetCreditCard.ProcessRefund
                                                                                                                            (CurrentViewContext.TransactionId,
                                                                                                                            Convert.ToInt64(customerProfile.CustomerProfileID),
                                                                                                                            Convert.ToDecimal(txtRefundAmount.Text),
                                                                                                                            CurrentViewContext.OrganizationUserId,
                                                                                                                            CurrentViewContext.CCNumber,
                                                                                                                            _description,
                                                                                                                            CurrentViewContext.InvoiceNumber);*/
                        INTSOF.AuthNet.Business.CustomerProfileWS.CreateCustomerProfileTransactionResponseType _response = INTSOF.AuthNet.Business.AuthorizeNetCreditCard.ProcessRefund
                                                                                                                            (hdfTransactionId.Value,
                                                                                                                            Convert.ToInt64(customerProfile.CustomerProfileID),
                                                                                                                            Convert.ToDecimal(txtRefundAmount.Text),
                                                                                                                            CurrentViewContext.OrganizationUserId,
                                                                                                                            hdfCCNumber.Value,
                                                                                                                            _description,
                                                                                                                            hdfInvoiceNumber.Value, CurrentViewContext.SelectedTenantId);

                        if (_response.resultCode == INTSOF.AuthNet.Business.CustomerProfileWS.MessageTypeEnum.Ok &&
                             !_response.directResponse.IsNullOrEmpty())
                        {
                            string[] arrRespParts = _response.directResponse.Split('|');

                            hdnPartialOrderCancellationAmount.Value = Convert.ToString(Convert.ToDecimal(hdnPartialOrderCancellationAmount.Value)
                                                                        - Convert.ToDecimal(txtRefundAmount.Text));

                            SaveRefundHistory(_response.directResponse, true, txtRefundAmount, hdfOrderPaymentdetailID.Value, arrRespParts);
                            SetNetAmount(true, txtTotalRefund, txtOriginalPrice, txtNetPrice, Convert.ToInt32(hdfOrderPaymentdetailID.Value));
                            base.ShowSuccessMessage(arrRespParts[3]);
                            txtRefundAmount.Text = String.Empty;
                        }
                        else if (_response.resultCode == INTSOF.AuthNet.Business.CustomerProfileWS.MessageTypeEnum.Error && !_response.directResponse.IsNullOrEmpty())
                        {
                            string[] arrRespParts = _response.directResponse.Split('|');
                            SaveRefundHistory(_response.directResponse, false, txtRefundAmount, hdfOrderPaymentdetailID.Value, arrRespParts);
                            base.ShowInfoMessage(arrRespParts[3]);
                        }
                        else
                        {
                            System.Text.StringBuilder _sbInfoMessage = new System.Text.StringBuilder();

                            for (int i = 0; i < _response.messages.Length; i++)
                            {
                                _sbInfoMessage.Append(_response.messages[i].text);  // To Get Message n for loop to check the [i] is not empty 
                            }
                            SaveRefundHistory(Convert.ToString(_sbInfoMessage), false, txtRefundAmount, hdfOrderPaymentdetailID.Value, null);

                            base.ShowInfoMessage(Convert.ToString(_sbInfoMessage));
                        }
                    }
                    else
                        base.ShowErrorMessage(Resources.Language.NSFCNTDATATORFND);
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

        protected void rptBackgroundPackages_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (OrderPackageTypeCode == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
                {
                    if (e.Item.ItemIndex == AppConsts.NONE)
                    {
                        Control hrline = e.Item.FindControl("hrline") as Control;
                        if (hrline.IsNotNull())
                            hrline.Visible = false;
                    }
                }
                /* UAT-916 commented this code because this code is shifted below on paymentdetails check.
                if ((!ShowApproveRejectButtons || ParentControl.IsNotNull()) && IsInvoiceOnly)
                {
                    Control divBkgPackagePrice = e.Item.FindControl("divBkgPackagePrice") as Control;
                    if (divBkgPackagePrice.IsNotNull())
                        divBkgPackagePrice.Visible = false;
                    Control divBkgPackagePriceLabel = e.Item.FindControl("divBkgPackagePriceLabel") as Control;
                    if (divBkgPackagePriceLabel.IsNotNull())
                        divBkgPackagePriceLabel.Visible = false;
                }*/

                Control dvCancelBackgroundPkg = e.Item.FindControl("dvCancelBackgroundPkg") as Control;
                Control dvBkgPackage = e.Item.FindControl("dvBkgPackage") as Control;
                Control dvBkgLocationTenant = e.Item.FindControl("dvBkgLocationTenant") as Control;

                //if (CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Cancelled.GetStringValue() ||
                //    CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Cancellation_Requested.GetStringValue())
                //{
                //    Control dvCancelBackgroundPkg = e.Item.FindControl("dvCancelBackgroundPkg") as Control;
                //    dvCancelBackgroundPkg.Visible = false;
                //}

                //Background Package Cancelled label is not displaying if there is only one cancelled compliance and bkg package
                //if (BkgPackagesList.IsNotNull() && BkgPackagesList.Rows.Count == AppConsts.ONE
                //    //&& (CurrentViewContext.IsCompliancePartialOrderCancelled || !chkPartialCancelCompliancePkg.Visible)
                //    && !String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                //    && CurrentViewContext.PartialOrderCancellationTypeCode != PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue())
                //{
                //    dvCancelBackgroundPkg.Visible = false;
                //}
                if (BkgPackagesList.IsNotNull() && BkgPackagesList.Rows.Count == AppConsts.ONE
                        && String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                        && OrderPackageTypeCode == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
                {
                    dvCancelBackgroundPkg.Visible = false;
                }
                else if (BkgPackagesList.IsNotNull() && BkgPackagesList.Rows.Count > AppConsts.NONE)
                {
                    HiddenField hdnBkgOrderPackageID = ((HiddenField)e.Item.FindControl("hdnBkgOrderPackageID"));
                    if (hdnBkgOrderPackageID.IsNotNull() && Convert.ToInt32(hdnBkgOrderPackageID.Value) > 0)
                    {
                        //Control dvCancelBackgroundPkg = e.Item.FindControl("dvCancelBackgroundPkg") as Control;
                        WclTextBox txtPartialCancelBkgPkgStatus = ((WclTextBox)e.Item.FindControl("txtPartialCancelBkgPkgStatus"));
                        Int32 bkgOrderPackageID = Convert.ToInt32(hdnBkgOrderPackageID.Value);

                        var bkgPackagesBasedOn_BOPID = BkgPackagesList.Select().Where(x => Convert.ToInt32(x["BkgOrderPackageID"]) == bkgOrderPackageID)
                                                      .Select(col => col).FirstOrDefault();
                        //Convert.ToBoolean(col["IsPartialOrderCancelled"])).FirstOrDefault();
                        WclButton partialCancelBkgPkgChecked = ((WclButton)e.Item.FindControl("chkPartialCancelBkgPkg"));
                        OrderPaymentDetail currentBopOrderPaymentDtl = null;
                        var OrderPkgDetailByBOPID = CurrentViewContext.OrderPkgPaymentDetailList.FirstOrDefault(cnd => cnd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue() && cnd.OPPD_BkgOrderPackageID.Value == bkgOrderPackageID && !cnd.OPPD_IsDeleted);
                        if (OrderPkgDetailByBOPID.IsNotNull())
                            currentBopOrderPaymentDtl = OrderPkgDetailByBOPID.OrderPaymentDetail;

                        if (bkgPackagesBasedOn_BOPID.IsNotNull() && Convert.ToBoolean(bkgPackagesBasedOn_BOPID["IsPartialOrderCancelled"]))
                        {
                            partialCancelBkgPkgChecked.Visible = false;
                            txtPartialCancelBkgPkgStatus.Text = Resources.Language.BKGPKGCNCL;
                            txtPartialCancelBkgPkgStatus.Visible = true;
                            hdnIsPartialOrderCancellation.Value = "1";
                            if (CheckIfBkgPartialCancelledPkg_IsCC(bkgOrderPackageID))
                            {
                                WclNumericTextBox bkgOrderPkgPrice = ((WclNumericTextBox)e.Item.FindControl("txtRushOrderPrice"));
                                hdnPartialOrderCancellationAmount.Value = Convert.ToString(Convert.ToDecimal(hdnPartialOrderCancellationAmount.Value)
                                                              + Convert.ToDecimal(bkgOrderPkgPrice.Text));
                            }
                        }
                        /*UAt-916 bkgOrderPackageID
                         * else if (CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Cancelled.GetStringValue() ||
                            CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Cancellation_Requested.GetStringValue())*/
                        //else if (CurrentViewContext.OrderPaymentDetailList.IsNotNull() && CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().lkpOrderStatu.IsNotNull()
                        //    && (CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue() ||
                        //    CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue()))
                        else if (currentBopOrderPaymentDtl.IsNotNull() && currentBopOrderPaymentDtl.lkpOrderStatu.IsNotNull()
                            && (currentBopOrderPaymentDtl.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue() ||
                            currentBopOrderPaymentDtl.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue()))
                        {
                            //Control dvCancelBackgroundPkg = e.Item.FindControl("dvCancelBackgroundPkg") as Control;
                            dvCancelBackgroundPkg.Visible = false;
                        }
                        else if (!ShowApproveRejectButtons && ParentControl.IsNotNull())
                        {
                            dvCancelBackgroundPkg.Visible = false;
                        }

                        var bkgPackagesList = BkgPackagesList.Select().Select(col => col).ToList();
                        if (bkgPackagesList.Where(cond => !Convert.ToBoolean(cond["IsPartialOrderCancelled"])).Count() == AppConsts.ONE
                            && !Convert.ToBoolean(bkgPackagesBasedOn_BOPID["IsPartialOrderCancelled"])
                            && (CurrentViewContext.IsCompliancePartialOrderCancelled)
                            && !String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                            && CurrentViewContext.OrderPackageTypeCode != OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
                        {
                            dvCancelBackgroundPkg.Visible = false;
                            btnPartialOrderCancellation.Visible = false;
                        }
                        else if (bkgPackagesList.Where(cond => !Convert.ToBoolean(cond["IsPartialOrderCancelled"])).Count() == AppConsts.ONE
                            && !Convert.ToBoolean(bkgPackagesBasedOn_BOPID["IsPartialOrderCancelled"])
                            && !String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                            && CurrentViewContext.PartialOrderCancellationTypeCode == PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue()
                            && CurrentViewContext.OrderPackageTypeCode == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
                        {
                            dvCancelBackgroundPkg.Visible = false;
                            btnPartialOrderCancellation.Visible = false;
                        }
                        //UAT-2217
                        if (bkgPackagesList.Where(cond => !Convert.ToBoolean(cond["IsPartialOrderCancelled"])).Count() < AppConsts.ONE)
                        {
                            rdbIsGraduatedBackground.Enabled = false;
                        }


                    }
                }

                #region UAT-916
                if (BkgPackagesList.IsNotNull() && BkgPackagesList.Rows.Count > AppConsts.NONE && CurrentViewContext.OrderPkgPaymentDetailList.IsNotNull() && CurrentViewContext.OrderPkgPaymentDetailList.Count > AppConsts.NONE)
                {
                    WclTextBox txtBkgPkgPaymentType = ((WclTextBox)e.Item.FindControl("txtBkgPkgPaymentType"));
                    WclNumericTextBox bkgOrderPkgPrice = ((WclNumericTextBox)e.Item.FindControl("txtRushOrderPrice"));
                    HiddenField hdnBkgOrderPackageIDTemp = ((HiddenField)e.Item.FindControl("hdnBkgOrderPackageID"));
                    Decimal bkgOrderPrice = AppConsts.NONE;
                    Boolean _isInvoiceOnly = false;
                    Int32 BOP_ID = 0;
                    if (bkgOrderPkgPrice.IsNotNull())
                    {
                        bkgOrderPrice = Convert.ToDecimal(bkgOrderPkgPrice.Text);
                    }
                    if (!hdnBkgOrderPackageIDTemp.IsNullOrEmpty())
                    {
                        BOP_ID = Convert.ToInt32(hdnBkgOrderPackageIDTemp.Value);
                        OrderPkgPaymentDetail tempOrderPkgPaymentDetail = CurrentViewContext.OrderPkgPaymentDetailList.FirstOrDefault(x => x.OPPD_BkgOrderPackageID == BOP_ID && !x.OPPD_IsDeleted);
                        if (tempOrderPkgPaymentDetail.IsNotNull())
                        {
                            OrderPaymentDetail tempOrderpaymentDetail = tempOrderPkgPaymentDetail.OrderPaymentDetail;
                            if (tempOrderpaymentDetail.lkpPaymentOption.IsNotNull() && bkgOrderPrice > AppConsts.NONE)
                                txtBkgPkgPaymentType.Text = tempOrderpaymentDetail.lkpPaymentOption.Name;
                            else
                                txtBkgPkgPaymentType.Text = String.Empty;
                            if (tempOrderpaymentDetail.lkpPaymentOption.IsNotNull() && (tempOrderpaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || tempOrderpaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                            {
                                _isInvoiceOnly = true;
                            }
                            //Hide Price for invoice order 
                            if (((!ShowApproveRejectButtons || ParentControl.IsNotNull()) && _isInvoiceOnly) || CurrentViewContext.IsLocationServiceTenant) //UAT 3573
                            {
                                Control divBkgPackagePrice = e.Item.FindControl("divBkgPackagePrice") as Control;
                                if (divBkgPackagePrice.IsNotNull())
                                    divBkgPackagePrice.Visible = false;
                                Control divBkgPackagePriceLabel = e.Item.FindControl("divBkgPackagePriceLabel") as Control;
                                if (divBkgPackagePriceLabel.IsNotNull())
                                    divBkgPackagePriceLabel.Visible = false;
                            }
                        }
                    }
                }
                #endregion

                //#region UAT-1558 As a Student, I should be able to mark when I have "Graduated" from a tracking and/or screening package's corresponding program
                //Control dvSaveIsGraduatedBackground = e.Item.FindControl("dvSaveIsGraduatedBackground") as Control;
                //if (IsApplicant)
                //{
                //    dvSaveIsGraduatedBackground.Visible = true;
                //}
                //else
                //{
                //    dvSaveIsGraduatedBackground.Visible = true;
                //}
                //#endregion

                #region UAT-2384
                HideCancelOrderPermissionForClientAdmin();
                if (CurrentViewContext.IsClientAdmin)
                {
                    dvCancelBackgroundPkg.Visible = false;
                }
                #endregion

                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    dvBkgLocationTenant.Visible = true;
                    dvBkgPackage.Visible = false;
                }
                else
                {
                    dvBkgLocationTenant.Visible = false;
                    dvBkgPackage.Visible = true;
                }

                #region UAT-3850

                if (!CurrentViewContext.IsLocationServiceTenant.IsNullOrEmpty() && CurrentViewContext.IsLocationServiceTenant)
                {
                    HtmlGenericControl dvBkgPkgPaymentType = e.Item.FindControl("dvBkgPkgPaymentType") as HtmlGenericControl;
                    //WclTextBox txtBkgPkgPaymentType = e.Item.FindControl("txtBkgPkgPaymentType") as WclTextBox;
                    if (!dvBkgPkgPaymentType.IsNullOrEmpty())
                        dvBkgPkgPaymentType.Visible = false;
                    //if (!txtBkgPkgPaymentType.IsNullOrEmpty())
                    //    txtBkgPkgPaymentType.Visible = false;
                }
                #endregion

                #region UAT-4490
                HtmlGenericControl divBkgCancellationDetails = e.Item.FindControl("divBkgCancellationDetails") as HtmlGenericControl;
                if (IsApplicant)
                {
                    divBkgCancellationDetails.Visible = false;
                }
                else
                {
                    divBkgCancellationDetails.Visible = true;
                }
                #endregion
            }
        }

        protected void cmbPaymentModes_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox comboBox = (WclComboBox)(sender);
            RepeaterItem item = comboBox.NamingContainer as RepeaterItem;
            WclButton btnSaveNewPaymentType = (item.FindControl("btnSaveNewPaymentType") as WclButton);
            HtmlGenericControl divSaveNewPaymentType = (item.FindControl("divSaveNewPaymentType") as HtmlGenericControl);
            HiddenField hdnOrderPaymentDetailID = (item.FindControl("hdnOrderPaymentDetailID") as HiddenField);
            HtmlGenericControl dvOrderPaymentAmount = (item.FindControl("dvOrderPaymentAmount") as HtmlGenericControl);
            HiddenField hdfOrderPackageType = (item.FindControl("hdfOrderPackageType") as HiddenField);
            Int32 tempOrderPaymentDetailID = Convert.ToInt32(hdnOrderPaymentDetailID.Value);
            List<Int32> orderPaymentBOPIDList = CurrentViewContext.OrderPkgPaymentDetailList.Where(x => x.OPPD_OrderPaymentDetailID == tempOrderPaymentDetailID && !x.OPPD_IsDeleted && x.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).Select(slct => slct.OPPD_BkgOrderPackageID.Value).ToList();
            Boolean isCompliancePackageIncluded = false;
            //UAT-4537
            _applicantOrderCart = GetApplicantOrderCart();
            if (!_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.IsLocationServiceTenant)
            {
                _applicantOrderCart.ApprovalPendingPackageName = String.Empty;
                if (!CurrentViewContext.lstPendingApprovalPackageNames.IsNullOrEmpty())
                {
                    CurrentViewContext.lstPendingApprovalPackageNames.ForEach(x =>
                    {
                        _applicantOrderCart.ApprovalPendingPackageName = _applicantOrderCart.ApprovalPendingPackageName + ", '" + x + "'";
                    });
                }
                Int32 _selectedPaymentOptionId = Convert.ToInt32(comboBox.SelectedValue);
                Boolean isCurrentPaymentApprovalReq = false;
                IEnumerable<DataRow> checkPkgLevel = CurrentViewContext.lstPaymentOption.Tables["Table"].AsEnumerable().Where(cnd => Convert.ToBoolean(cnd["IsPkgLevel"]));
                if (checkPkgLevel.Count() > 0)
                {
                    isCurrentPaymentApprovalReq = CurrentViewContext.lstPaymentOption.Tables["Table"].AsEnumerable().Where(cond => cond.Field<int>("PaymentOptionId") == _selectedPaymentOptionId).Select(sel => sel.Field<Boolean>("IsApprovalReqd")).FirstOrDefault();
                }

                if (isCurrentPaymentApprovalReq)
                {
                    String pkgName = CurrentViewContext.lstPaymentOption.Tables["Table"].AsEnumerable().Where(cond => cond.Field<int>("PaymentOptionId") == _selectedPaymentOptionId).Select(sel => sel.Field<String>("PkgName")).FirstOrDefault();
                    _applicantOrderCart.ApprovalPendingPackageName = _applicantOrderCart.ApprovalPendingPackageName + ", '" + pkgName + "'";
                    _applicantOrderCart.IsPaymentApprovalRequired = true;
                }
                else
                {
                    _applicantOrderCart.IsPaymentApprovalRequired = false;
                }
                if (!_applicantOrderCart.ApprovalPendingPackageName.IsNullOrEmpty())
                {
                    _applicantOrderCart.ApprovalPendingPackageName = _applicantOrderCart.ApprovalPendingPackageName.Remove(0, 1);
                }
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);
            }
            //END UAT
            if (btnSaveNewPaymentType.IsNotNull() && divSaveNewPaymentType.IsNotNull())
            {
                btnSaveNewPaymentType.Visible = true;
                divSaveNewPaymentType.Visible = true;
                btnSaveNewPaymentType.ToolTip = Resources.Language.SBMTNPAYYRORD;
            }
            if (hdfOrderPackageType.IsNotNull() && hdfOrderPackageType.Value == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
            {
                isCompliancePackageIncluded = true;
            }
            //UAT-916
            // if ((cmbPaymentModes.SelectedValue == PaymentMode_InvoicetoInstitutionId.ToString()) || (cmbPaymentModes.SelectedValue == PaymentMode_InvoiceWithoutApprovalId.ToString()))
            if ((comboBox.SelectedValue == PaymentMode_InvoicetoInstitutionId.ToString()) || (comboBox.SelectedValue == PaymentMode_InvoiceWithoutApprovalId.ToString()))
            {
                //txtGrandTotal.Text = String.Empty;
                //txtTotalOrderValue.Text = String.Empty;
                // txtRushOrderPrice.Text = String.Empty;
                /*UAT-916
                 * divTotalPrice.Visible = false;*/
                divTotalPrice.Visible = false;
                if (dvOrderPaymentAmount.IsNotNull())
                {
                    dvOrderPaymentAmount.Visible = false;
                }
                if (isCompliancePackageIncluded)
                {
                    dvRushOrder.Visible = false;
                    divSubscriptionFee.Visible = false;
                }
                foreach (var rptItem in rptBackgroundPackages.Items)
                {
                    HiddenField hdnBkgOrderPackageIDTemp = ((rptItem as RepeaterItem).FindControl("hdnBkgOrderPackageID") as HiddenField);
                    Int32 BOP_ID = 0;
                    if (hdnBkgOrderPackageIDTemp.IsNotNull() && !hdnBkgOrderPackageIDTemp.IsNullOrEmpty())
                    {
                        BOP_ID = Convert.ToInt32(hdnBkgOrderPackageIDTemp.Value);
                        if (orderPaymentBOPIDList.IsNotNull() && orderPaymentBOPIDList.Contains(BOP_ID))
                        {
                            Control divBkgPackagePrice = ((rptItem as RepeaterItem).FindControl("divBkgPackagePrice") as Control);
                            if (divBkgPackagePrice.IsNotNull())
                                divBkgPackagePrice.Visible = false;
                            Control divBkgPackagePriceLabel = ((rptItem as RepeaterItem).FindControl("divBkgPackagePriceLabel") as Control);
                            if (divBkgPackagePriceLabel.IsNotNull())
                                divBkgPackagePriceLabel.Visible = false;
                        }
                    }
                    /*UAT-916
                     * Control divBkgPackagePrice = ((rptItem as RepeaterItem).FindControl("divBkgPackagePrice") as Control);
                    if (divBkgPackagePrice.IsNotNull())
                    {
                        divBkgPackagePrice.Visible = false;
                    }
                    Control divBkgPackagePriceLabel = ((rptItem as RepeaterItem).FindControl("divBkgPackagePriceLabel") as Control);
                    if (divBkgPackagePriceLabel.IsNotNull())
                    {
                        divBkgPackagePriceLabel.Visible = false;
                    }*/
                }
            }
            else
            {
                divTotalPrice.Visible = true;
                if (dvOrderPaymentAmount.IsNotNull())
                {
                    dvOrderPaymentAmount.Visible = true;
                }
                if (isCompliancePackageIncluded)
                {
                    divSubscriptionFee.Visible = true;
                    if (!RushOrderPrice.IsNullOrEmpty())
                    {
                        dvRushOrder.Visible = true;
                    }
                }
                /*UAT-916
                 * divSubscriptionFee.Visible = true;
                if (!RushOrderPrice.IsNullOrEmpty())
                {
                    dvRushOrder.Visible = true;
                }*/
                foreach (var rptItem in rptBackgroundPackages.Items)
                {
                    HiddenField hdnBkgOrderPackageIDTemp = ((rptItem as RepeaterItem).FindControl("hdnBkgOrderPackageID") as HiddenField);
                    Int32 BOP_ID = 0;
                    if (hdnBkgOrderPackageIDTemp.IsNotNull() && !hdnBkgOrderPackageIDTemp.IsNullOrEmpty())
                    {
                        BOP_ID = Convert.ToInt32(hdnBkgOrderPackageIDTemp.Value);
                        if (orderPaymentBOPIDList.IsNotNull() && orderPaymentBOPIDList.Contains(BOP_ID))
                        {
                            Control divBkgPackagePrice = ((rptItem as RepeaterItem).FindControl("divBkgPackagePrice") as Control);
                            if (divBkgPackagePrice.IsNotNull())
                                divBkgPackagePrice.Visible = true;
                            Control divBkgPackagePriceLabel = ((rptItem as RepeaterItem).FindControl("divBkgPackagePriceLabel") as Control);
                            if (divBkgPackagePriceLabel.IsNotNull())
                                divBkgPackagePriceLabel.Visible = true;
                        }
                    }
                }
                /*UAt-916
                foreach (var rptItem in rptBackgroundPackages.Items)
                {
                    Control divBkgPackagePrice = ((rptItem as RepeaterItem).FindControl("divBkgPackagePrice") as Control);
                    if (divBkgPackagePrice.IsNotNull())
                    {
                        divBkgPackagePrice.Visible = true;
                    }
                    Control divBkgPackagePriceLabel = ((rptItem as RepeaterItem).FindControl("divBkgPackagePriceLabel") as Control);
                    if (divBkgPackagePriceLabel.IsNotNull())
                    {
                        divBkgPackagePriceLabel.Visible = true;
                    }
                }*/

            }

        }

        #region UAT-916
        protected void rptOrderPAymentDetail_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Boolean _tempShowOfflineSettlement = false;
                    Boolean _tempApprovePaymentSetting = false;
                    String _tempRefNumber = String.Empty;
                    _tempApprovePaymentSetting = hdfShowApprovePaymentSetting.Value.IsNullOrEmpty() ? false : Convert.ToBoolean(hdfShowApprovePaymentSetting.Value);
                    _tempShowOfflineSettlement = hdfShowOffLineSettlement.Value.IsNullOrEmpty() ? false : Convert.ToBoolean(hdfShowOffLineSettlement.Value);
                    Control dvChangePaymentType = (e.Item.FindControl("dvChangePaymentType") as Control);
                    Control divNewPaymentType = (e.Item.FindControl("divNewPaymentType") as Control);
                    Control divSaveNewPaymentType = (e.Item.FindControl("divSaveNewPaymentType") as Control);
                    WclButton btnSaveNewPaymentType = (e.Item.FindControl("btnSaveNewPaymentType") as WclButton);
                    WclComboBox cmbPaymentModes = (e.Item.FindControl("cmbPaymentModes") as WclComboBox);
                    WclTextBox txtPaymentType = (e.Item.FindControl("txtPaymentType") as WclTextBox);
                    WclTextBox txtPaymentOrderStatus = (e.Item.FindControl("txtPaymentOrderStatus") as WclTextBox);
                    WclNumericTextBox txtOrderPaymentAmount = (e.Item.FindControl("txtOrderPaymentAmount") as WclNumericTextBox);
                    WclTextBox txtReferenceNumber = (e.Item.FindControl("txtReferenceNumber") as WclTextBox);
                    HtmlGenericControl dvTrackingCnclBy = (e.Item.FindControl("dvTrackingCnclBy") as HtmlGenericControl);
                    if (CurrentViewContext.TrackingPkgCancelledBy.IsNullOrEmpty())
                    {
                        dvTrackingCnclBy.Visible = false;
                    }
                    else
                    {
                        if (Presenter.IsAdminLoggedIn())
                        {
                            dvTrackingCnclBy.Visible = true;
                            WclTextBox txtTrackCNCLBY = (e.Item.FindControl("txtTrackCNCLBY") as WclTextBox);
                            txtTrackCNCLBY.Text = CurrentViewContext.TrackingPkgCancelledBy;
                        }
                        else
                            dvTrackingCnclBy.Visible = false;
                    }

                    #region UAT-3817
                    Label lblPaymentApprovedBy = (e.Item.FindControl("lblPaymentApprovedBy") as Label);
                    Control divPaymentApprovedBy = (e.Item.FindControl("divPaymentApprovedBy") as Control);
                    Control divlblPaymentApprovedBy = (e.Item.FindControl("divlblPaymentApprovedBy") as Control);
                    #endregion

                    Control divReferenceNumber = (e.Item.FindControl("divReferenceNumber") as Control);
                    HtmlGenericControl dvPaymentRejection = (e.Item.FindControl("dvPaymentRejection") as HtmlGenericControl);
                    HtmlGenericControl dvOrderPaymentAmount = (e.Item.FindControl("dvOrderPaymentAmount") as HtmlGenericControl);
                    Control divRejectionReason = (e.Item.FindControl("divRejectionReason") as Control);
                    WclButton btnRejectPayment = (e.Item.FindControl("btnRejectPayment") as WclButton);
                    WclButton btnApprovePayment = (e.Item.FindControl("btnApprovePayment") as WclButton);
                    WclButton btnCancelOrder = (e.Item.FindControl("btnCancelOrder") as WclButton);
                    Control spRefNo = (e.Item.FindControl("spRefNo") as Control);
                    HiddenField hdnOrderPaymentDetailID = (e.Item.FindControl("hdnOrderPaymentDetailID") as HiddenField);
                    HiddenField hdfpaymentTypeCode = (e.Item.FindControl("hdfpaymentTypeCode") as HiddenField);
                    HiddenField hdfOrderPackageType = (e.Item.FindControl("hdfOrderPackageType") as HiddenField);
                    HiddenField hdfOrderStatusCode = (e.Item.FindControl("hdfOrderStatusCode") as HiddenField);
                    RequiredFieldValidator rfvReferenceNumber = (e.Item.FindControl("rfvReferenceNumber") as RequiredFieldValidator);
                    RequiredFieldValidator rfvPaymentRejection = (e.Item.FindControl("rfvPaymentRejection") as RequiredFieldValidator);
                    HtmlGenericControl ContainerDiv = (e.Item.FindControl("ContainerDiv") as HtmlGenericControl);

                    WclTextBox txtPrevOrderStatus = (e.Item.FindControl("txtPrevOrderStatus") as WclTextBox);
                    HtmlGenericControl dvPreviousOrderStatus = (e.Item.FindControl("dvPreviousOrderStatus") as HtmlGenericControl);
                    //UAT-3850
                    HtmlGenericControl spnAmount = (e.Item.FindControl("spnAmount") as HtmlGenericControl);
                    //

                    #region UAT-3632
                    HtmlGenericControl dvItemPaymentDetail = (e.Item.FindControl("dvItemPaymentDetail") as HtmlGenericControl);
                    WclTextBox txtItemName = (e.Item.FindControl("txtItemName") as WclTextBox);
                    WclTextBox txtCIDOROrderNumber = (e.Item.FindControl("txtCIDOROrderNumber") as WclTextBox);
                    Label lblCIDOrOrderLabel = (e.Item.FindControl("lblCIDOrOrderLabel") as Label);
                    #endregion

                    OrderPaymentDetail tempOrderPaymentDetail = e.Item.DataItem as OrderPaymentDetail;
                    if (tempOrderPaymentDetail.IsNotNull())
                    {
                        CurrentViewContext.OrderPaymentDetailID = Convert.ToInt32(tempOrderPaymentDetail.OPD_ID);
                        //Set Attribute value with tempOrderPaymentDetailId
                        String attributeId = String.Empty;
                        if (ContainerDiv.IsNotNull())
                        {
                            attributeId = "ContainerDiv_" + Convert.ToString(tempOrderPaymentDetail.OPD_ID);
                            ContainerDiv.Attributes.Add("class", attributeId);
                        }
                        if (btnSaveNewPaymentType.IsNotNull())
                        {
                            attributeId = Convert.ToString(tempOrderPaymentDetail.OPD_ID);
                            btnSaveNewPaymentType.Attributes.Add("OPDID", attributeId);
                        }
                        if (tempOrderPaymentDetail.lkpOrderStatu.IsNotNull() && tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Paid.GetStringValue()) &&
                            (tempOrderPaymentDetail.Order.DuePayment.IsNull() || (tempOrderPaymentDetail.Order.DuePayment.HasValue && tempOrderPaymentDetail.Order.DuePayment.Value == 0)) &&
                           ( //orderPaymentDetail.Order.lkpOrderRequestType.IsNotNull() && orderPaymentDetail.Order.lkpOrderRequestType.ORT_Code //Used LookupManager for lkp tables
                            tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue())))
                        {
                            _tempRefNumber = "Subscription changed by Admin- Previous Subscription Id: " + tempOrderPaymentDetail.Order.PreviousSubscriptionID + "";
                        }
                        else
                        {
                            _tempRefNumber = tempOrderPaymentDetail.OPD_ReferenceNo;
                        }

                        if (tempOrderPaymentDetail.IsNotNull() && tempOrderPaymentDetail.lkpOrderStatu.IsNotNull() && tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()) && !ShowApproveCancellation)
                            _tempApprovePaymentSetting = true;

                        if (txtPaymentType.IsNotNull() && tempOrderPaymentDetail.IsNotNull() && tempOrderPaymentDetail.lkpPaymentOption.IsNotNull())
                        {
                            txtPaymentType.Text = tempOrderPaymentDetail.lkpPaymentOption.Name;
                        }
                        if (txtPaymentOrderStatus.IsNotNull() && tempOrderPaymentDetail.IsNotNull() && tempOrderPaymentDetail.lkpOrderStatu.IsNotNull())
                        {

                            txtPaymentOrderStatus.Text = tempOrderPaymentDetail.lkpOrderStatu.Name;

                        }
                        if (txtPrevOrderStatus.IsNotNull() && tempOrderPaymentDetail.IsNotNull() && tempOrderPaymentDetail.lkpOrderStatu.IsNotNull() && !tempOrderPaymentDetail.OrderPaymentDetailHistories.IsNullOrEmpty())
                        {
                            //UAT-1167 WB: Payment Status update to be able to see history of orders with cancellation or cancel requested.
                            if (tempOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue())
                            {
                                List<OrderPaymentDetailHistory> lstOrderPaymentDetailHistory = tempOrderPaymentDetail.OrderPaymentDetailHistories.ToList();
                                OrderPaymentDetailHistory orderPaymentDetailHistory = lstOrderPaymentDetailHistory.Where(x => x.OPDH_OrderPaymentDetailID == tempOrderPaymentDetail.OPD_ID && x.OPDH_IsDeleted == false && x.OPDH_OrderStatusID != tempOrderPaymentDetail.OPD_OrderStatusID).OrderByDescending(x => x.OPDH_CreatedOn).FirstOrDefault();
                                txtPrevOrderStatus.Text = orderPaymentDetailHistory.lkpOrderStatu.Name;
                                if (dvPreviousOrderStatus.IsNotNull())
                                {
                                    dvPreviousOrderStatus.Visible = true;
                                }
                            }
                        }
                        if (txtOrderPaymentAmount.IsNotNull() && tempOrderPaymentDetail.IsNotNull())
                        {
                            txtOrderPaymentAmount.Text = tempOrderPaymentDetail.OPD_Amount.ToString();
                        }
                        if (hdnOrderPaymentDetailID.IsNotNull())
                        {
                            hdnOrderPaymentDetailID.Value = Convert.ToString(tempOrderPaymentDetail.OPD_ID);
                        }
                        if (rfvReferenceNumber.IsNotNull() && btnApprovePayment.IsNotNull())
                        {
                            rfvReferenceNumber.ValidationGroup = rfvReferenceNumber.ValidationGroup + Convert.ToString(tempOrderPaymentDetail.OPD_ID);
                            btnApprovePayment.ValidationGroup = btnApprovePayment.ValidationGroup + Convert.ToString(tempOrderPaymentDetail.OPD_ID);
                        }
                        if (rfvPaymentRejection.IsNotNull() && btnRejectPayment.IsNotNull())
                        {
                            rfvPaymentRejection.ValidationGroup = rfvPaymentRejection.ValidationGroup + Convert.ToString(tempOrderPaymentDetail.OPD_ID);
                            btnRejectPayment.ValidationGroup = btnRejectPayment.ValidationGroup + Convert.ToString(tempOrderPaymentDetail.OPD_ID);
                        }
                        if (hdfpaymentTypeCode.IsNotNull() && tempOrderPaymentDetail.lkpPaymentOption.IsNotNull())
                        {
                            hdfpaymentTypeCode.Value = tempOrderPaymentDetail.lkpPaymentOption.Code;
                        }
                        if (hdfOrderPackageType.IsNotNull() && CurrentViewContext.CompPkgOrderPaymentDetail.IsNotNull() && tempOrderPaymentDetail.OPD_ID == CurrentViewContext.CompPkgOrderPaymentDetail.OPD_ID)
                        {
                            hdfOrderPackageType.Value = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
                        }
                        else
                        {
                            hdfOrderPackageType.Value = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
                        }
                        if (hdfOrderStatusCode.IsNotNull() && tempOrderPaymentDetail.lkpOrderStatu.IsNotNull())
                        {
                            hdfOrderStatusCode.Value = tempOrderPaymentDetail.lkpOrderStatu.Code;
                        }
                        if (txtReferenceNumber.IsNotNull())
                        {
                            txtReferenceNumber.Text = _tempRefNumber;
                        }

                        //UAT-3817
                        if (IsApplicant)
                        {
                            divPaymentApprovedBy.Visible = false;
                            divlblPaymentApprovedBy.Visible = false;
                        }
                        else if (!tempOrderPaymentDetail.lkpPaymentOption.IsNullOrEmpty() && !tempOrderPaymentDetail.Order.OrganizationUserProfile.FirstName.IsNullOrEmpty())
                        {
                            if (tempOrderPaymentDetail.lkpPaymentOption.Code == "PTINA" || tempOrderPaymentDetail.lkpPaymentOption.Code == "PTPYP" ||
                                (tempOrderPaymentDetail.lkpPaymentOption.Code == "PTCC" && tempOrderPaymentDetail.lkpOrderStatu.Code == "OSPPA"))
                            {
                                divPaymentApprovedBy.Visible = false;
                                divlblPaymentApprovedBy.Visible = false;
                            }
                            else
                            {
                                Int32 paymentApprovedById = Convert.ToInt32(tempOrderPaymentDetail.Order.ApprovedBy);
                                lblPaymentApprovedBy.Text = Presenter.GetPaymentApprovedByUsingId(paymentApprovedById);
                                if (lblPaymentApprovedBy.Text == "")
                                {
                                    divPaymentApprovedBy.Visible = false;
                                    divlblPaymentApprovedBy.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            divPaymentApprovedBy.Visible = false;
                            divlblPaymentApprovedBy.Visible = false;
                        }


                        if (tempOrderPaymentDetail.IsNotNull() && tempOrderPaymentDetail.lkpPaymentOption.IsNotNull() && tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithApproval.GetStringValue())
                        {
                            if (rfvReferenceNumber.IsNotNull() && spRefNo.IsNotNull())
                            {
                                rfvReferenceNumber.Enabled = false;
                                spRefNo.Visible = false;
                            }
                        }
                        else if (rfvReferenceNumber.IsNotNull() && spRefNo.IsNotNull())
                        {
                            rfvReferenceNumber.Enabled = true;
                            spRefNo.Visible = true;
                        }

                        #region UAT-3632
                        Presenter.IsClientAdmin();
                        if (CurrentViewContext.IsItemPaymentOrder
                             && !CurrentViewContext.IsLocationServiceTenant
                           //&& _showItemPaymentDetail_AdminOnly 
                           //&& (Presenter.IsAdminLoggedIn() || CurrentViewContext.IsClientAdmin)
                           )
                        {
                            dvItemPaymentDetail.Style["Display"] = "block";
                            txtItemName.Text = CurrentViewContext.PaymentItemName;
                            lblCIDOrOrderLabel.Text = CurrentViewContext.IsRequirementItemPayment ? "Complio ID" : "Order-Number";
                            txtCIDOROrderNumber.Text = CurrentViewContext.ItemPaymentCIDOROrderID;
                        }

                        #endregion
                    }

                    if (tempOrderPaymentDetail.IsNotNull())
                    {
                        List<lkpPaymentOption> tempOrderPaymentOptions = null;
                        if (ParentControl.IsNotNull())
                        {
                            _tempApprovePaymentSetting = false;
                            _tempShowOfflineSettlement = false;
                            if (tempOrderPaymentDetail.lkpPaymentOption.IsNotNull() && (tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                            {
                                dvOrderPaymentAmount.Visible = false;
                            }
                            if (tempOrderPaymentDetail.lkpOrderStatu.IsNotNull() && tempOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()) //&& tempOrderPaymentDetail.OPD_ID == CurrentViewContext.CompPkgOrderPaymentDetail.OPD_ID)
                            {
                                //Check if there are more than 1 Payment type exists for that Applicant
                                if (hdfpaymentTypeCode.IsNotNull())
                                {
                                    tempOrderPaymentOptions = Presenter.GetPaymentOptionsForChangePayment(hdfpaymentTypeCode.Value);
                                }
                                if (tempOrderPaymentOptions.IsNotNull() &&
                                    tempOrderPaymentOptions.Any())
                                {
                                    dvChangePaymentType.Visible = true;
                                }
                                else
                                {
                                    dvChangePaymentType.Visible = false;
                                }
                                divNewPaymentType.Visible = false;
                                btnSaveNewPaymentType.Visible = false;
                            }
                            else
                            {
                                divNewPaymentType.Visible = false;
                                btnSaveNewPaymentType.Visible = false;
                                dvChangePaymentType.Visible = false;
                            }

                            if (tempOrderPaymentDetail.lkpPaymentOption.IsNotNull() && (tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()
                               || tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                                && hdfOrderPackageType.IsNotNull() && hdfOrderPackageType.Value == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()
                               )
                            {
                                dvRushOrder.Visible = false;
                                divSubscriptionFee.Visible = false;
                            }
                        }
                        else if (ShowApproveRejectButtons)
                        {
                            divNewPaymentType.Visible = false;
                            dvChangePaymentType.Visible = false;
                            btnSaveNewPaymentType.Visible = false;
                            divSaveNewPaymentType.Visible = false;
                        }
                        else
                        {
                            _tempApprovePaymentSetting = false;
                            _tempShowOfflineSettlement = false;
                            if (tempOrderPaymentDetail.lkpPaymentOption.IsNotNull() && (tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                            {
                                dvOrderPaymentAmount.Visible = false;
                            }
                            //UAT 299: As a student, I should be able to change the payment type for my subscription if it is in Pending Approval status
                            if (tempOrderPaymentDetail.lkpOrderStatu.IsNotNull() && tempOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()) //&& tempOrderPaymentDetail.OPD_ID == CurrentViewContext.CompPkgOrderPaymentDetail.OPD_ID)
                            {
                                //Check if there are more than 1 Payment type exists for that Applicant
                                if (hdfpaymentTypeCode.IsNotNull())
                                {
                                    tempOrderPaymentOptions = Presenter.GetPaymentOptionsForChangePayment(hdfpaymentTypeCode.Value);
                                }
                                if (tempOrderPaymentOptions.IsNotNull() &&
                                    tempOrderPaymentOptions.Any())
                                {
                                    dvChangePaymentType.Visible = true;
                                }
                                else
                                {
                                    dvChangePaymentType.Visible = false;
                                }
                                divNewPaymentType.Visible = false;
                                btnSaveNewPaymentType.Visible = false;
                            }
                            else
                            {
                                divNewPaymentType.Visible = false;
                                btnSaveNewPaymentType.Visible = false;
                                dvChangePaymentType.Visible = false;
                            }
                            if (tempOrderPaymentDetail.lkpPaymentOption.IsNotNull() && (tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()
                               || tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                                && hdfOrderPackageType.IsNotNull() && hdfOrderPackageType.Value == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()
                               )
                            {
                                dvRushOrder.Visible = false;
                                divSubscriptionFee.Visible = false;
                            }
                        }

                        dvPaymentRejection.Visible = _tempApprovePaymentSetting;
                        //UAT-1955
                        if (_tempApprovePaymentSetting)
                        {
                            txtReferenceNumber.Enabled = true;
                            txtReferenceNumber.ReadOnly = false;

                        }
                        else
                        {
                            txtReferenceNumber.Enabled = true;
                            txtReferenceNumber.ReadOnly = true;
                        }
                        //btnCancelOrder.Visible = false;
                        spRefNo.Visible = _tempApprovePaymentSetting;
                        //UAT - 685 If Order Approval Process Already Initiated then disable approve buttons
                        if (_tempApprovePaymentSetting && ComplianceDataManager.GetScheduleTasksToProcess(SelectedTenantId, TaskType.INVOICEORDERBULKAPPROVE.GetStringValue()).Any(obj => obj.RecordID == OrderId))
                        {
                            dvPaymentRejection.Disabled = true;
                            btnRejectPayment.ToolTip = Resources.Language.ORDINQUEUEFORPAYMNT;
                            btnRejectPayment.Enabled = false;
                            btnApprovePayment.ToolTip = Resources.Language.ORDINQUEUEFORPAYMNT;
                            btnApprovePayment.Enabled = false;
                        }
                        else
                        {
                            dvPaymentRejection.Disabled = false;
                            btnRejectPayment.ToolTip = "";
                            btnRejectPayment.Enabled = true;
                            btnApprovePayment.ToolTip = "";
                            btnApprovePayment.Enabled = true;
                        }

                        #region HideShowControls
                        if (tempOrderPaymentDetail.lkpOrderStatu.IsNotNull())
                        {
                            if (tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Paid.GetStringValue()))
                            {
                                dvPaymentRejection.Visible = _tempShowOfflineSettlement;
                                //UAT-1955
                                txtReferenceNumber.Enabled = true;
                                txtReferenceNumber.ReadOnly = true;

                                spRefNo.Visible = false;
                                btnRejectPayment.Visible = false;
                                divRejectionReason.Visible = false;
                                btnApprovePayment.Visible = false;
                                //btnCancelOrder.Visible = _tempShowOfflineSettlement;
                            }
                            else if (tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue())
                                || tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue()))
                            {
                                dvPaymentRejection.Visible = _tempShowOfflineSettlement;
                                divRejectionReason.Visible = false;
                                btnRejectPayment.Visible = false;
                                //btnCancelOrder.Visible = _tempShowOfflineSettlement;
                            }
                            else if (tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Payment_Rejected.GetStringValue()))
                            {
                                dvPaymentRejection.Visible = _tempShowOfflineSettlement;
                                //UAT-1955
                                if (_tempShowOfflineSettlement)
                                {
                                    txtReferenceNumber.Enabled = true;
                                    txtReferenceNumber.ReadOnly = false;
                                }
                                else
                                {
                                    txtReferenceNumber.Enabled = true;
                                    txtReferenceNumber.ReadOnly = true;
                                }
                                spRefNo.Visible = _tempShowOfflineSettlement;
                                btnRejectPayment.Visible = false;
                                divRejectionReason.Visible = false;
                                //btnCancelOrder.Visible = false;
                            }
                            else if (tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Payment_Due.GetStringValue()))
                            {
                                dvPaymentRejection.Visible = _tempShowOfflineSettlement;
                                //btnCancelOrder.Visible = _tempShowOfflineSettlement;
                            }
                            //UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
                            else if (tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Pending_School_Approval.GetStringValue()))
                            {
                                dvPaymentRejection.Visible = _tempShowOfflineSettlement;
                                btnApprovePayment.Visible = _tempShowOfflineSettlement;
                                divRejectionReason.Visible = _tempShowOfflineSettlement;
                                btnRejectPayment.Visible = _tempShowOfflineSettlement;
                            }
                            //UAT-2073- Order approve button for client admins to be allowed for invoice with approval payment status and Pending School Approval order status
                            if (!Presenter.IsAdminLoggedIn())
                            {
                                if (tempOrderPaymentDetail.lkpOrderStatu.Code != ApplicantOrderStatus.Pending_School_Approval.GetStringValue()
                                    && (tempOrderPaymentDetail.lkpPaymentOption.IsNotNull()
                                    && tempOrderPaymentDetail.lkpPaymentOption.Code != PaymentOptions.InvoiceWithApproval.GetStringValue()))
                                {
                                    dvPaymentRejection.Visible = false;

                                    //UAT-1955
                                    txtReferenceNumber.Enabled = true;
                                    txtReferenceNumber.ReadOnly = true;

                                    spRefNo.Visible = false;
                                    btnRejectPayment.Visible = false;
                                    divRejectionReason.Visible = false;
                                    btnApprovePayment.Visible = false;
                                }
                            }
                        }

                        if ((tempOrderPaymentDetail.lkpOrderStatu.IsNotNull() && (tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Payment_Due.GetStringValue())
                            || tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue())
                            || tempOrderPaymentDetail.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue())))
                           || (tempOrderPaymentDetail.lkpPaymentOption.IsNotNull() && (tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()
                           || tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.Paypal.GetStringValue()))
                            )
                        {
                            divReferenceNumber.Visible = false;
                        }
                        #endregion
                        // UAT 285: Reference Number field to be optional for Invoice
                        if (tempOrderPaymentDetail.IsNotNull() && tempOrderPaymentDetail.lkpPaymentOption.IsNotNull() && tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithApproval.GetStringValue())
                        {
                            if (rfvReferenceNumber.IsNotNull() && spRefNo.IsNotNull())
                            {
                                rfvReferenceNumber.Enabled = false;
                                spRefNo.Visible = false;
                            }
                        }
                        #region Bug# 70
                        if (tempOrderPaymentDetail.OPD_Amount.Value == AppConsts.NONE)
                        {
                            if (txtPaymentType.IsNotNull())
                                txtPaymentType.Text = String.Empty;
                            if (txtPaymentOrderStatus.IsNotNull() && tempOrderPaymentDetail.lkpOrderStatu.IsNotNull() && (tempOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue() || tempOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue()))
                                txtPaymentOrderStatus.Text = tempOrderPaymentDetail.lkpOrderStatu.Name;
                            else
                                txtPaymentOrderStatus.Text = Resources.Language.PAID;
                        }
                        #endregion

                        #region UAT-1648:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                        if (Presenter.IsCompliancePackageAlreadyPurchased(tempOrderPaymentDetail))
                        {
                            dvPaymentRejection.Visible = _tempShowOfflineSettlement;
                            //UAT-1955
                            txtReferenceNumber.Enabled = true;
                            txtReferenceNumber.ReadOnly = true;

                            spRefNo.Visible = false;
                            btnRejectPayment.Visible = false;
                            divRejectionReason.Visible = false;
                            btnApprovePayment.Visible = false;
                        }
                        #endregion

                        //UAT-3850
                        if (tempOrderPaymentDetail.OPD_OrderID > AppConsts.NONE)
                        {
                            spnAmount.InnerText = Resources.Language.AMOUNT;
                            //get billing code obj and add check here.
                            Entity.ClientEntity.OrderBillingCodeMapping orderBillingCodeMapping = new Entity.ClientEntity.OrderBillingCodeMapping();
                            orderBillingCodeMapping = Presenter.GetOrderBillingCode(tempOrderPaymentDetail.OPD_OrderID);

                            if (!orderBillingCodeMapping.IsNullOrEmpty() && !orderBillingCodeMapping.CBIBillingStatu.IsNullOrEmpty()
                                && !orderBillingCodeMapping.CBIBillingStatu.CBS_Amount.IsNullOrEmpty() && orderBillingCodeMapping.CBIBillingStatu.CBS_Amount > AppConsts.NONE)
                            {
                                dvOrderPaymentAmount.Visible = true;
                                if (tempOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                                    spnAmount.InnerText = Resources.Language.PAIDBYINST;
                                else
                                    spnAmount.InnerText = Resources.Language.BALANCEAMT;
                            }
                        }
                    }
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

        protected void rptOrderPAymentDetail_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            try
            {

                GetRepeaterRowData(e.Item);
                if (e.CommandName == "RejectPayment")
                {
                    RejectPayment();
                }
                else if (e.CommandName == "ApprovePayment")
                {
                    ApprovePayment(CurrentViewContext.OrderPaymentDetailStatusCode);
                }
                //else if (e.CommandName == "ChangePaymentTypeClick")
                //{
                //    ChangePaymentType(cmbPaymentModes, divNewPaymentType, btnSaveNewPaymentType);
                //}
                //else if (e.CommandName == "CancelOrder")
                //{
                //    CancelOrderPaymentDetail(CurrentViewContext.OrderPaymentDetailID, CurrentViewContext.IsCompliancePackageInclude);
                //}
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

        private void GetRepeaterRowData(System.Web.UI.WebControls.RepeaterItem item)
        {
            WclTextBox txtReferenceNumber = (item.FindControl("txtReferenceNumber") as WclTextBox);
            HiddenField hdnOrderPaymentDetailID = (item.FindControl("hdnOrderPaymentDetailID") as HiddenField);
            HiddenField hdfpaymentTypeCode = (item.FindControl("hdfpaymentTypeCode") as HiddenField);
            HiddenField hdfOrderPackageType = (item.FindControl("hdfOrderPackageType") as HiddenField);
            HiddenField hdfOrderStatusCode = (item.FindControl("hdfOrderStatusCode") as HiddenField);
            WclButton btnSaveNewPaymentType = (item.FindControl("btnSaveNewPaymentType") as WclButton);
            Control divNewPaymentType = (item.FindControl("divNewPaymentType") as Control);
            WclComboBox cmbPaymentModes = (item.FindControl("cmbPaymentModes") as WclComboBox);
            WclNumericTextBox txtOrderPaymentAmount = (item.FindControl("txtOrderPaymentAmount") as WclNumericTextBox);
            WclTextBox txtPaymentRejection = (item.FindControl("txtPaymentRejection") as WclTextBox);

            //UAT-4537
            _applicantOrderCart = GetApplicantOrderCart();
            if (!_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.IsLocationServiceTenant)
            {
                _applicantOrderCart.ApprovalPendingPackageName = String.Empty;
                if (!CurrentViewContext.lstPendingApprovalPackageNames.IsNullOrEmpty())
                {
                    CurrentViewContext.lstPendingApprovalPackageNames.ForEach(x =>
                    {
                        _applicantOrderCart.ApprovalPendingPackageName = _applicantOrderCart.ApprovalPendingPackageName + ", '" + x + "'";
                    });
                }
                Int32 _selectedPaymentOptionId = Convert.ToInt32(cmbPaymentModes.SelectedValue);
                Boolean isCurrentPaymentApprovalReq = false;
                IEnumerable<DataRow> checkPkgLevel = CurrentViewContext.lstPaymentOption.Tables["Table"].AsEnumerable().Where(cnd => Convert.ToBoolean(cnd["IsPkgLevel"]));
                if (checkPkgLevel.Count() > 0)
                {
                    isCurrentPaymentApprovalReq = CurrentViewContext.lstPaymentOption.Tables["Table"].AsEnumerable().Where(cond => cond.Field<int>("PaymentOptionId") == _selectedPaymentOptionId).Select(sel => sel.Field<Boolean>("IsApprovalReqd")).FirstOrDefault();
                }
                if (isCurrentPaymentApprovalReq)
                {
                    String pkgName = CurrentViewContext.lstPaymentOption.Tables["Table"].AsEnumerable().Where(cond => cond.Field<int>("PaymentOptionId") == _selectedPaymentOptionId).Select(sel => sel.Field<String>("PkgName")).FirstOrDefault();
                    _applicantOrderCart.ApprovalPendingPackageName = _applicantOrderCart.ApprovalPendingPackageName + ", '" + pkgName + "'";
                    _applicantOrderCart.IsPaymentApprovalRequired = true;
                }
                else
                {
                    _applicantOrderCart.IsPaymentApprovalRequired = false;
                }
                if (!_applicantOrderCart.ApprovalPendingPackageName.IsNullOrEmpty())
                {
                    _applicantOrderCart.ApprovalPendingPackageName = _applicantOrderCart.ApprovalPendingPackageName.Remove(0, 1);
                }
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);
            }
            //END UAT
            if (txtReferenceNumber.IsNotNull())
            {
                CurrentViewContext.ReferenceNumber = txtReferenceNumber.Text;
            }
            if (hdnOrderPaymentDetailID.IsNotNull())
            {
                CurrentViewContext.OrderPaymentDetailID = Convert.ToInt32(hdnOrderPaymentDetailID.Value);
            }
            if (hdfpaymentTypeCode.IsNotNull())
            {
                CurrentViewContext.PaymentType = hdfpaymentTypeCode.Value;
            }
            if (hdfOrderPackageType.IsNotNull() && hdfOrderPackageType.Value == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
            {
                CurrentViewContext.IsCompliancePackageInclude = true;
            }
            if (hdfOrderStatusCode.IsNotNull())
            {
                CurrentViewContext.OrderPaymentDetailStatusCode = hdfOrderStatusCode.Value;
            }
            if (txtOrderPaymentAmount.IsNotNull() && !txtOrderPaymentAmount.Text.IsNullOrEmpty())
            {
                CurrentViewContext.OrderPaymentAmount = Convert.ToDecimal(txtOrderPaymentAmount.Text);
            }
            if (txtPaymentRejection.IsNotNull())
            {
                CurrentViewContext.RejectionPaymentReason = txtPaymentRejection.Text;
            }
        }

        protected void rptRefundOrder_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {

                    WclButton btnRefund = (e.Item.FindControl("btnRefund") as WclButton);
                    WclNumericTextBox txtOriginalPrice = (e.Item.FindControl("txtOriginalPrice") as WclNumericTextBox);
                    WclNumericTextBox txtRefundAmount = (e.Item.FindControl("txtRefundAmount") as WclNumericTextBox);
                    WclNumericTextBox txtTotalRefund = (e.Item.FindControl("txtTotalRefund") as WclNumericTextBox);
                    WclNumericTextBox txtNetPrice = (e.Item.FindControl("txtNetPrice") as WclNumericTextBox);

                    HtmlGenericControl divEachOrderRefund = (e.Item.FindControl("divEachOrderRefund") as HtmlGenericControl);

                    HiddenField hdfOrderPaymentdetailID = (e.Item.FindControl("hdfOrderPaymentdetailID") as HiddenField);
                    HiddenField hdfOrderStatusTypeCode = (e.Item.FindControl("hdfOrderStatusTypeCode") as HiddenField);
                    HiddenField hdfTransactionId = (e.Item.FindControl("hdfTransactionId") as HiddenField);
                    HiddenField hdfCCNumber = (e.Item.FindControl("hdfCCNumber") as HiddenField);
                    HiddenField hdfInvoiceNumber = (e.Item.FindControl("hdfInvoiceNumber") as HiddenField);

                    RequiredFieldValidator rfvRefundAmount = (e.Item.FindControl("rfvRefundAmount") as RequiredFieldValidator);
                    CompareValidator cmpRefund = (e.Item.FindControl("cmpRefund") as CompareValidator);


                    OrderPaymentDetail creditCardOrderPaymentDetail = e.Item.DataItem as OrderPaymentDetail;
                    if (creditCardOrderPaymentDetail.IsNotNull() && creditCardOrderPaymentDetail.OnlinePaymentTransaction.IsNotNull())
                    {
                        #region Set Transaction Info and controls
                        if (hdfOrderPaymentdetailID.IsNotNull())
                        {
                            hdfOrderPaymentdetailID.Value = Convert.ToString(creditCardOrderPaymentDetail.OPD_ID);
                        }
                        if (hdfOrderStatusTypeCode.IsNotNull() && creditCardOrderPaymentDetail.lkpOrderStatu.IsNotNull())
                        {
                            hdfOrderStatusTypeCode.Value = creditCardOrderPaymentDetail.lkpOrderStatu.Code;
                        }
                        if (hdfTransactionId.IsNotNull())
                        {
                            hdfTransactionId.Value = creditCardOrderPaymentDetail.OnlinePaymentTransaction.Trans_id;
                        }
                        if (hdfCCNumber.IsNotNull())
                        {
                            hdfCCNumber.Value = creditCardOrderPaymentDetail.OnlinePaymentTransaction.CCNumber;
                        }
                        if (hdfInvoiceNumber.IsNotNull())
                        {
                            hdfInvoiceNumber.Value = creditCardOrderPaymentDetail.OnlinePaymentTransaction.Invoice_num;
                        }
                        if (rfvRefundAmount.IsNotNull() && cmpRefund.IsNotNull() && btnRefund.IsNotNull())
                        {
                            rfvRefundAmount.ValidationGroup = rfvRefundAmount.ValidationGroup + Convert.ToString(creditCardOrderPaymentDetail.OPD_ID);
                            cmpRefund.ValidationGroup = cmpRefund.ValidationGroup + Convert.ToString(creditCardOrderPaymentDetail.OPD_ID);
                            btnRefund.ValidationGroup = btnRefund.ValidationGroup + Convert.ToString(creditCardOrderPaymentDetail.OPD_ID);
                        }
                        #endregion
                        var _order = creditCardOrderPaymentDetail.Order;
                        DateTime? _dt = new DateTime();

                        // If Applicant request cancellation of order, which was not approved yet and was in 'Sent for Online payment status'
                        /*
                        //if (CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue())*/
                        if (creditCardOrderPaymentDetail.lkpOrderStatu.IsNotNull()
                                &&
                             (
                                creditCardOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()
                                 ||
                                creditCardOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue()))
                        {
                            _dt = _order.OrderDate;
                        }
                        else
                        {
                            _dt = creditCardOrderPaymentDetail.OPD_ApprovalDate;
                        }

                        TimeSpan? ts = DateTime.Now - _dt;

                        // If order time is less than 120 Days
                        // AND Order status is Cancellation Requested OR Cancelled
                        // AND Payment type is Credit Card
                        if (
                                ShowApproveRejectButtons
                                &&
                                (!_dt.IsNullOrEmpty() && !ts.IsNullOrEmpty() && ts.Value.TotalDays < 120)
                                &&
                                (
                                creditCardOrderPaymentDetail.lkpOrderStatu.IsNotNull() && (
                                    creditCardOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue()
                                    ||
                                    creditCardOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue())
                                )
                                &&
                                (
                                    creditCardOrderPaymentDetail.OPD_PaymentOptionID.IsNotNull()
                                    &&
                                    creditCardOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()
                                )
                           )
                        {
                            if (txtOriginalPrice.IsNotNull())
                            {
                                txtOriginalPrice.Text = Convert.ToString(creditCardOrderPaymentDetail.OPD_Amount);
                                SetNetAmount(false, txtTotalRefund, txtOriginalPrice, txtNetPrice, creditCardOrderPaymentDetail.OPD_ID);
                                if (divEachOrderRefund.IsNotNull())
                                    divEachOrderRefund.Visible = true;
                                hdfVisibleRefundCount.Value = Convert.ToString(Convert.ToInt32(hdfVisibleRefundCount.Value) + AppConsts.ONE);
                            }
                        }
                        else if (ShowApproveRejectButtons && (!_dt.IsNullOrEmpty() && !ts.IsNullOrEmpty() && ts.Value.TotalDays < 120)
                               && (creditCardOrderPaymentDetail.lkpOrderStatu.IsNotNull()
                               && creditCardOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue())
                               && (creditCardOrderPaymentDetail.lkpPaymentOption.IsNotNull()
                               && creditCardOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue())
                               && hdnIsPartialOrderCancellation.Value == "1")
                        {
                            if (txtOriginalPrice.IsNotNull())
                            {
                                txtOriginalPrice.Text = Convert.ToString(creditCardOrderPaymentDetail.OPD_Amount);
                                SetNetAmount(false, txtTotalRefund, txtOriginalPrice, txtNetPrice, creditCardOrderPaymentDetail.OPD_ID);
                                if (divEachOrderRefund.IsNotNull())
                                    divEachOrderRefund.Visible = true;
                                hdfVisibleRefundCount.Value = Convert.ToString(Convert.ToInt32(hdfVisibleRefundCount.Value) + AppConsts.ONE);
                            }
                        }
                        else
                        {
                            if (divEachOrderRefund.IsNotNull())
                                divEachOrderRefund.Visible = false;
                        }
                    }
                    else
                    {
                        if (divEachOrderRefund.IsNotNull())
                            divEachOrderRefund.Visible = false;
                    }

                    //UAT-2618:If a document has ever been associated with any item within a tracking subscription, the refund functionality on the order detail screen should be grayed out
                    if (Presenter.IsGrayedOutRefundFunctionality(creditCardOrderPaymentDetail.OPD_ID))
                    {
                        txtRefundAmount.Enabled = false;
                        btnRefund.Enabled = false;
                        btnRefund.ToolTip = Resources.Language.RFNDNOTALWD;
                    }
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

        #region UAT-2971
        protected void lnkGoBack_Click(object sender, EventArgs e)
        {
            if (this.ParentQueueType == AppConsts.SUPPORT_PORTAL_DETAIL)
            {
                String childcontrolPath = ChildControls.SupportPortalDetails;
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId", Convert.ToString(SelectedTenantId) },
                                                                    { "Child", childcontrolPath},
                                                                    {"OrganizationUserId",CurrentViewContext.ApplicantOrgUserID.ToString()},
                                                                    {"UserId",UserId.ToString()}
                                                                 };
                string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
        }
        #endregion
        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Save the refund response from Authorize.Net
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isSuccess"></param>
        /// <param name="arrRespParts"></param>
        private void SaveRefundHistory(String message, Boolean isSuccess, WclNumericTextBox txtRefundAmount, String orderPaymentDetailId, String[] arrRespParts)
        {
            Presenter.AddRefundHistory(
                new RefundHistory
                {
                    RH_OrderID = CurrentViewContext.OrderId,
                    RH_Amount = Convert.ToDecimal(txtRefundAmount.Text),
                    RH_CreatedByID = CurrentViewContext.CurrentLoggedInUserId,
                    RH_CreatedOn = DateTime.Now,
                    RH_TransID = arrRespParts == null ? null : arrRespParts[6],
                    RH_DirectResponse = message,
                    RH_IsSuccess = isSuccess,
                    RH_OrderPaymentDetailID = Convert.ToInt32(orderPaymentDetailId)
                });
        }

        /// <summary>
        /// To route Page Back
        /// </summary>
        /// <param name="showSuccessMessage"></param>
        private void RoutePageBack(bool showSuccessMessage, String successMessage = null)
        {

            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantId) },
                                                                    { "Child", WorkQueuePath}
                                                                 };

            if (showSuccessMessage)
            {
                queryString.Add("UpdatedStatus", successMessage);
            }
            String url = String.Empty;
            if (ShowApproveRejectButtons)
            {
                var _basePage = IsParentBkgQueue() ? "~/BkgOperations/"
                                                   : "~/ComplianceOperations/";

                url = String.Format(_basePage + "Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            }
            else
                url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

            Response.Redirect(url, true);

        }

        /// <summary>
        /// Returns if the parent was the Background or Compliance Queue
        /// </summary>
        /// <returns></returns>
        private Boolean IsParentBkgQueue()
        {
            return this.ParentQueueType == AppConsts.QUEUE_TYPE_BKGORDER_QUEUE
                                   ? true
                                   : false;
        }

        /// <summary>
        /// Shows or hides approve buttons for approving payment or cancellation as per the value set in the corresponding properties.
        /// </summary>
        private void DisplayApproveButtons()
        {
            //If screen is opened from Dashboard
            if (ParentControl.IsNotNull())
            {
                dvCancellation.Visible = false;
                ApprovePaymentSettings = false;
                OfflineSettlementSettings = false;
                dvShowBackLink.Visible = true;
                btnGoBack.Text = Resources.Language.GOBCKTODSHBRD;
                //UAT-916
                btnCancelOrder.Visible = false;
                divCancelOrder.Visible = false;
                //divNewPaymentType.Visible = false;
                //btnSaveNewPaymentType.Visible = false;
                //dvChangePaymentType.Visible = false;
                if (IsInvoiceOnly)
                {
                    //txtGrandTotal.Text = String.Empty;
                    //txtTotalOrderValue.Text = String.Empty;
                    //txtRushOrderPrice.Text = String.Empty;
                    //divTotalPrice.Visible = false;
                    dvPrice.Visible = false;
                    //dvRushOrder.Visible = false;
                    //divSubscriptionFee.Visible = false;
                }

                /* UAt-916
                 //UAT 299: As a student, I should be able to change the payment type for my subscription if it is in Pending Approval status
                 if (CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue())
                 {
                     //Check if there are more than 1 Payment type exists for that Applicant
                     Presenter.GetPaymentOptions();
                     if (CurrentViewContext.ListPaymentOptions.IsNotNull() &&
                         CurrentViewContext.ListPaymentOptions.Any())
                     {
                         dvChangePaymentType.Visible = true;
                     }
                     else
                     {
                         dvChangePaymentType.Visible = false;
                     }
                     divNewPaymentType.Visible = false;
                     btnSaveNewPaymentType.Visible = false;
                 }
                 else
                 {
                     divNewPaymentType.Visible = false;
                     btnSaveNewPaymentType.Visible = false;
                     dvChangePaymentType.Visible = false;
                 }*/
            }
            //If screen is opened from Order queue.
            else if (ShowApproveRejectButtons)
            {
                cbbuttons.Visible = true;
                if (ShowApproveCancellation)
                {
                    dvCancellation.Visible = true;
                }
                else
                {
                    dvCancellation.Visible = false;
                }
                if (ShowApprovePayment)
                {
                    ApprovePaymentSettings = true;
                }
                else
                {
                    ApprovePaymentSettings = false;
                }
                if (ShowOfflineSettlement)
                {
                    OfflineSettlementSettings = true;
                }
                else
                {
                    OfflineSettlementSettings = false;
                }
                /* Hide This code for UAT-916
                divNewPaymentType.Visible = false;
                btnSaveNewPaymentType.Visible = false;
                dvChangePaymentType.Visible = false;*/
                //dvCancelPackages.Visible = true;
                //UAT-916
                if ((CurrentViewContext.OrderPaymentDetailList.IsNotNull()
                     && CurrentViewContext.OrderPaymentDetailList.All(cond => cond.lkpOrderStatu != null && (cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue())))
                    || (!CurrentViewContext.OrderPaymentDetailList.Any(cnd => cnd.lkpOrderStatu.Code != ApplicantOrderStatus.Payment_Rejected.GetStringValue()))
                   )
                {
                    btnCancelOrder.Visible = false;
                    divCancelOrder.Visible = false;
                }
                //UAT-2387, Make the cancel button and link visible false for the previous order 
                if (CurrentViewContext.CompPkgOrderPaymentDetail.IsNotNull())
                {
                    if (CurrentViewContext.CompPkgOrderPaymentDetail.Order.IsNotNull() && CurrentViewContext.CompPkgOrderPaymentDetail.Order.PackageSubscriptions.IsNotNull()
                        && CurrentViewContext.CompPkgOrderPaymentDetail.Order.PackageSubscriptions.Count == 1 && CurrentViewContext.CompPkgOrderPaymentDetail.Order.PackageSubscriptions.FirstOrDefault().IsDeleted)
                    {
                        if (CurrentViewContext.CompPkgOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue())
                        {
                            btnCancelOrder.Visible = false;
                            divCancelOrder.Visible = false;
                            btnCancelOrderTmp.Visible = false;
                            dvCancelCompliancePkg.Visible = false;
                            btnPartialOrderCancellation.Visible = false;
                        }
                    }
                }

                //UAT-2971:- Hide the cbbuttons From Support Portal Details Screen.
                if (this.ParentQueueType == AppConsts.SUPPORT_PORTAL_DETAIL)
                {
                    lnkGoBack.Visible = true;
                    lnkbacksrch.Visible = false;
                    //btnCancelOrderTmp.Visible = false;
                    //btnCancelOrder.Visible = false;
                    cbbuttons.Visible = false;
                    cbbuttons.CancelButton.Visible = false;
                    cbbuttons.ClearButton.Visible = false;
                }

            }
            //If screen is opened from order history.
            else
            {
                dvCancellation.Visible = false;
                ApprovePaymentSettings = false;
                OfflineSettlementSettings = false;
                dvShowBackLink.Visible = true;
                //UAT-916
                btnCancelOrder.Visible = false;
                divCancelOrder.Visible = false;
                if (IsInvoiceOnly)
                {
                    //txtGrandTotal.Text = String.Empty;
                    //txtTotalOrderValue.Text = String.Empty;
                    //txtRushOrderPrice.Text = String.Empty;
                    divTotalPrice.Visible = false;
                    //dvRushOrder.Visible = false;
                    //divSubscriptionFee.Visible = false;
                }
                /*
                 //UAT 299: As a student, I should be able to change the payment type for my subscription if it is in Pending Approval status
                 if (CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue())
                 {
                     //Check if there are more than 1 Payment type exists for that Applicant
                     Presenter.GetPaymentOptions();
                     if (CurrentViewContext.ListPaymentOptions.IsNotNull() &&
                         CurrentViewContext.ListPaymentOptions.Any())
                     {
                         dvChangePaymentType.Visible = true;
                     }
                     else
                     {
                         dvChangePaymentType.Visible = false;
                     }
                     divNewPaymentType.Visible = false;
                     btnSaveNewPaymentType.Visible = false;
                 }
                 else
                 {
                     divNewPaymentType.Visible = false;
                     btnSaveNewPaymentType.Visible = false;
                     dvChangePaymentType.Visible = false;
                 }*/
            }

            //Bind Order Payment Detail Repeater
            BindOrderPaymentDetailRepeater();
        }

        /// <summary>
        /// To bind Order Payment Detail Repeater
        /// </summary>
        private void BindOrderPaymentDetailRepeater()
        {
            //UAT-916
            rptOrderPAymentDetail.DataSource = CurrentViewContext.OrderPaymentDetailList;
            rptOrderPAymentDetail.DataBind();

            List<OrderPkgPaymentDetail> lstOrderPaymentDetails = new List<OrderPkgPaymentDetail>();

            ////UAT-4537
            //CurrentViewContext.OrderPaymentDetailList.Where(cond => cond.lkpOrderStatu.Code == ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()).ForEach(x =>
            //{
            //    lstOrderPaymentDetails.AddRange(x.OrderPkgPaymentDetails);
            //});

            //CurrentViewContext.lstPaymentApprovalPendingBkgOrderIDs = lstOrderPaymentDetails.Select(x => x.OPPD_BkgOrderPackageID.Value).ToList();
            ////END UAT

            if (CurrentViewContext.OrderPaymentDetailList.IsNotNull())
            {
                var creditCardOrderPaymentDetail = getCreditCardOrderPaymentDetail();
                if (creditCardOrderPaymentDetail.IsNotNull())
                {
                    CurrentViewContext.OrderDetail = creditCardOrderPaymentDetail.Order;
                    CurrentViewContext.OnlinePaymentAmount = creditCardOrderPaymentDetail.OPD_Amount ?? 0;
                    CurrentViewContext.OnlinePaymentTransaction = creditCardOrderPaymentDetail.OnlinePaymentTransaction;
                    CurrentViewContext.UserId = creditCardOrderPaymentDetail.Order.OrganizationUserProfile.OrganizationUser.UserID;
                }
            }
        }

        ///// <summary>
        ///// To change Payment Type
        ///// </summary>
        //private void ChangePaymentType(WclComboBox cmbPaymentModes, Control divNewPaymentType, WclButton btnSaveNewPaymentType)
        //{
        //    divNewPaymentType.Visible = true;
        //    btnSaveNewPaymentType.Visible = true;
        //    BindPaymentTypes(cmbPaymentModes);

        //    _applicantOrderCart = GetApplicantOrderCart();
        //    SetSessionData();
        //    //Set Session Service
        //    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);
        //}

        private void ChangePaymentType(LinkButton changeNewPaymentType)
        {
            /*UAT-916
             * divNewPaymentType.Visible = true;
            btnSaveNewPaymentType.Visible = true;*/
            BindPaymentTypes(changeNewPaymentType);

            _applicantOrderCart = GetApplicantOrderCart();
            SetSessionData();
            //Set Session Service
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);
        }

        /// <summary>
        /// To bind Payment Types
        /// </summary>
        private void BindPaymentTypes(LinkButton changeNewPaymentType)
        {
            /*
            //Presenter.GetPaymentOptions();
            //cmbPaymentModes.DataSource = CurrentViewContext.ListPaymentOptions;
            //cmbPaymentModes.DataBind();*/
            //UAT-916
            RepeaterItem item = changeNewPaymentType.NamingContainer as RepeaterItem;
            WclButton btnSaveNewPaymentType = (item.FindControl("btnSaveNewPaymentType") as WclButton);
            HtmlGenericControl divSaveNewPaymentType = (item.FindControl("divSaveNewPaymentType") as HtmlGenericControl);
            HtmlGenericControl divNewPaymentType = (item.FindControl("divNewPaymentType") as HtmlGenericControl);
            HtmlGenericControl dvOrderPaymentAmount = (item.FindControl("dvOrderPaymentAmount") as HtmlGenericControl);
            HiddenField hdnOrderPaymentDetailID = (item.FindControl("hdnOrderPaymentDetailID") as HiddenField);
            HiddenField hdfOrderPackageType = (item.FindControl("hdfOrderPackageType") as HiddenField);
            HiddenField hdfpaymentTypeCode = (item.FindControl("hdfpaymentTypeCode") as HiddenField);
            WclComboBox cmbPaymentModes = (item.FindControl("cmbPaymentModes") as WclComboBox);
            Int32 tempOrderPaymentDetailID = 0;
            if (hdnOrderPaymentDetailID.IsNotNull() && !hdnOrderPaymentDetailID.Value.IsNullOrEmpty())
            {
                tempOrderPaymentDetailID = Convert.ToInt32(hdnOrderPaymentDetailID.Value);
            }
            List<Int32> orderPaymentBOPIDList = CurrentViewContext.OrderPkgPaymentDetailList.Where(x => x.OPPD_OrderPaymentDetailID == tempOrderPaymentDetailID && !x.OPPD_IsDeleted && x.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).Select(slct => slct.OPPD_BkgOrderPackageID.Value).ToList();
            Boolean isCompliancePackageIncluded = false;
            CurrentViewContext.OrderPaymentDetailID = tempOrderPaymentDetailID;
            if (btnSaveNewPaymentType.IsNotNull() && divSaveNewPaymentType.IsNotNull() && divNewPaymentType.IsNotNull())
            {
                btnSaveNewPaymentType.Visible = true;
                divSaveNewPaymentType.Visible = true;
                btnSaveNewPaymentType.ToolTip = Resources.Language.SBMTNPAYYRORD;
                divNewPaymentType.Visible = true;
            }
            if (hdfOrderPackageType.IsNotNull() && hdfOrderPackageType.Value == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
            {
                isCompliancePackageIncluded = true;
            }
            if (hdfpaymentTypeCode.IsNotNull())
            {
                cmbPaymentModes.DataSource = Presenter.GetPaymentOptionsForChangePayment(hdfpaymentTypeCode.Value);
                cmbPaymentModes.DataBind();
            }

            if ((cmbPaymentModes.SelectedValue == PaymentMode_InvoicetoInstitutionId.ToString()) || (cmbPaymentModes.SelectedValue == PaymentMode_InvoiceWithoutApprovalId.ToString()))
            {
                //txtGrandTotal.Text = String.Empty;
                //txtTotalOrderValue.Text = String.Empty;
                // txtRushOrderPrice.Text = String.Empty;
                /*UAT-916
                 * divTotalPrice.Visible = false;*/
                divTotalPrice.Visible = false;
                if (dvOrderPaymentAmount.IsNotNull())
                {
                    dvOrderPaymentAmount.Visible = false;
                }
                if (isCompliancePackageIncluded)
                {
                    dvRushOrder.Visible = false;
                    divSubscriptionFee.Visible = false;
                }
                foreach (var rptItem in rptBackgroundPackages.Items)
                {
                    HiddenField hdnBkgOrderPackageIDTemp = ((rptItem as RepeaterItem).FindControl("hdnBkgOrderPackageID") as HiddenField);
                    Int32 BOP_ID = 0;
                    if (hdnBkgOrderPackageIDTemp.IsNotNull() && !hdnBkgOrderPackageIDTemp.IsNullOrEmpty())
                    {
                        BOP_ID = Convert.ToInt32(hdnBkgOrderPackageIDTemp.Value);
                        if (orderPaymentBOPIDList.IsNotNull() && orderPaymentBOPIDList.Contains(BOP_ID))
                        {
                            Control divBkgPackagePrice = ((rptItem as RepeaterItem).FindControl("divBkgPackagePrice") as Control);
                            if (divBkgPackagePrice.IsNotNull())
                                divBkgPackagePrice.Visible = false;
                            Control divBkgPackagePriceLabel = ((rptItem as RepeaterItem).FindControl("divBkgPackagePriceLabel") as Control);
                            if (divBkgPackagePriceLabel.IsNotNull())
                                divBkgPackagePriceLabel.Visible = false;
                        }
                    }
                }
            }
            else
            {
                divTotalPrice.Visible = true;
                if (dvOrderPaymentAmount.IsNotNull())
                {
                    dvOrderPaymentAmount.Visible = true;
                }
                if (isCompliancePackageIncluded)
                {
                    divSubscriptionFee.Visible = true;
                    if (!RushOrderPrice.IsNullOrEmpty())
                    {
                        dvRushOrder.Visible = true;
                    }
                }
                foreach (var rptItem in rptBackgroundPackages.Items)
                {
                    HiddenField hdnBkgOrderPackageIDTemp = ((rptItem as RepeaterItem).FindControl("hdnBkgOrderPackageID") as HiddenField);
                    Int32 BOP_ID = 0;
                    if (hdnBkgOrderPackageIDTemp.IsNotNull() && !hdnBkgOrderPackageIDTemp.IsNullOrEmpty())
                    {
                        BOP_ID = Convert.ToInt32(hdnBkgOrderPackageIDTemp.Value);
                        if (orderPaymentBOPIDList.IsNotNull() && orderPaymentBOPIDList.Contains(BOP_ID))
                        {
                            Control divBkgPackagePrice = ((rptItem as RepeaterItem).FindControl("divBkgPackagePrice") as Control);
                            if (divBkgPackagePrice.IsNotNull())
                                divBkgPackagePrice.Visible = true;
                            Control divBkgPackagePriceLabel = ((rptItem as RepeaterItem).FindControl("divBkgPackagePriceLabel") as Control);
                            if (divBkgPackagePriceLabel.IsNotNull())
                                divBkgPackagePriceLabel.Visible = true;
                        }
                    }
                }
            }
            /*UAT-916
             * if ((cmbPaymentModes.SelectedValue == PaymentMode_InvoicetoInstitutionId.ToString()) || (cmbPaymentModes.SelectedValue == PaymentMode_InvoiceWithoutApprovalId.ToString()))
            {
                //txtGrandTotal.Text = String.Empty;
                //txtTotalOrderValue.Text = String.Empty;
                // txtRushOrderPrice.Text = String.Empty;
                divTotalPrice.Visible = false;
                dvRushOrder.Visible = false;
                divSubscriptionFee.Visible = false;
                foreach (var rptItem in rptBackgroundPackages.Items)
                {
                    Control divBkgPackagePrice = ((rptItem as RepeaterItem).FindControl("divBkgPackagePrice") as Control);
                    if (divBkgPackagePrice.IsNotNull())
                    {
                        divBkgPackagePrice.Visible = false;
                    }
                    Control divBkgPackagePriceLabel = ((rptItem as RepeaterItem).FindControl("divBkgPackagePriceLabel") as Control);
                    if (divBkgPackagePriceLabel.IsNotNull())
                    {
                        divBkgPackagePriceLabel.Visible = false;
                    }
                }
            }
            else
            {
                divTotalPrice.Visible = true;
                divSubscriptionFee.Visible = true;
                if (!RushOrderPrice.IsNullOrEmpty())
                {
                    dvRushOrder.Visible = true;
                }
                foreach (var rptItem in rptBackgroundPackages.Items)
                {
                    Control divBkgPackagePrice = ((rptItem as RepeaterItem).FindControl("divBkgPackagePrice") as Control);
                    if (divBkgPackagePrice.IsNotNull())
                    {
                        divBkgPackagePrice.Visible = true;
                    }
                    Control divBkgPackagePriceLabel = ((rptItem as RepeaterItem).FindControl("divBkgPackagePriceLabel") as Control);
                    if (divBkgPackagePriceLabel.IsNotNull())
                    {
                        divBkgPackagePriceLabel.Visible = true;
                    }
                }

            }*/
        }

        /// <summary>
        /// Clears all the controls and properties.
        /// </summary>
        private void ResetControlsOnPage()
        {
            TextOrderId = String.Empty;
            TotalOrderValue = String.Empty;
            OrderDate = String.Empty;
            OrderStatus = String.Empty;
            InstituteHierarchy = String.Empty;
            DurationMonths = String.Empty;
            FirstName = String.Empty;
            MiddleName = String.Empty;
            LastName = String.Empty;
            //Alias1 = String.Empty;
            //Alias2 = String.Empty;
            //Alias3 = String.Empty;
            Gender = String.Empty;
            DateOfBirth = String.Empty;
            SocialSecurityNumber = String.Empty;
            SocialSecurityNumberMaskedOnly = String.Empty;
            PrimaryEmail = String.Empty;
            SecondaryEmail = String.Empty;
            Phone = String.Empty;
            SecondaryPhone = String.Empty;
            OrganizationUserId = 0;
            Address1 = String.Empty;
            Address2 = String.Empty;
            City = String.Empty;
            State = String.Empty;
            Zip = String.Empty;
            PaymentType = String.Empty;
            Package = String.Empty;
            PackageId = 0;
            ExpiryDate = DateTime.Now;
            OrderStatusCode = String.Empty;
            ShowApproveCancellation = false;
            ShowApprovePayment = false;
            ReferenceNumber = String.Empty;
            ShowApproveCancellation = false;
            ShowApprovePayment = false;
            GrandTotalPrice = String.Empty;
            RushOrderPrice = String.Empty;
            RejectionReason = String.Empty;
            dvRushOrder.Visible = false;
            RejectionPaymentReason = String.Empty;
            RushOrderStatus = String.Empty;
            dvRushOrderStatus.Visible = false;
            dvRushOrderFields.Visible = false;
            ShowOfflineSettlement = false;

            //Reset Controls implemented in UAT-966
            hdnIsPartialOrderCancellation.Value = "0";
            hdnPartialOrderCancellationAmount.Value = "0";
            btnPartialOrderCancellation.Visible = true;
            dvCancelCompliancePkg.Visible = true;
            /*UAT-916
             * txtRefundAmount.Text = String.Empty;*/

            chkPartialCancelCompliancePkg.Checked = false;
            chkPartialCancelCompliancePkg.Visible = true;
            txtPartialCancelCompliancePkgStatus.Visible = false;
            txtPartialCancelCompliancePkgStatus.Text = String.Empty;

            /*UAT-916*/
            divCancelOrder.Visible = true;
            btnCancelOrder.Visible = true;

        }

        /// <summary>
        /// Hide Order cancellationpermission for client admin : UAT-2384
        /// </summary>
        private void HideCancelOrderPermissionForClientAdmin()
        {
            Presenter.IsClientAdmin();
            if (CurrentViewContext.IsClientAdmin)
            {
                divCancelOrder.Visible = false;
                btnCancelOrder.Visible = false;
                btnCancelOrderTmp.Visible = false;
                dvCancelCompliancePkg.Visible = false;
                btnPartialOrderCancellation.Visible = false;
            }

        }

        /// <summary>
        /// Sets the properties from the arguments recieved through querystring.
        /// </summary>
        /// <param name="args"></param>
        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("SelectedTenantId"))
            {
                SelectedTenantId = Convert.ToInt32(args["SelectedTenantId"]);
            }
            if (args.ContainsKey("OrderId"))
            {
                OrderId = Convert.ToInt32(args["OrderId"]);
                if (FirstOrderId == AppConsts.NONE)
                {
                    FirstOrderId = OrderId;
                }
            }
            if (args.ContainsKey("ShowApproveRejectButtons"))
            {
                ShowApproveRejectButtons = Convert.ToBoolean(args["ShowApproveRejectButtons"]);
            }

            if (args.ContainsKey("hdfFingerPrint"))
            {
                hdfFingerPrint.Value = args["hdfFingerPrint"];
            }

            if (args.ContainsKey("hdfPassport"))
            {
                hdfPassport.Value = args["hdfPassport"];
            }


            //Contains Parent parameter if this screen is opened from Dasboard.
            if (args.ContainsKey("Parent") && args["Parent"].IsNotNull())
            {
                btnGoBack.ToolTip = Resources.Language.CLKTORTNDSHBRD;
                ParentControl = Convert.ToString(args["Parent"]);
            }
            else
            {
                btnGoBack.ToolTip = Resources.Language.CLKRTNORDRHSTRY;
                //UAT-916
                //btnSaveNewPaymentType.ToolTip = "Submit and pay for your order";
            }

            if (args.ContainsKey(AppConsts.PARENT_QUEUE_QUERYSTRING) && !args[AppConsts.PARENT_QUEUE_QUERYSTRING].IsNullOrEmpty())
            {
                this.ParentQueueType = Convert.ToString(args[AppConsts.PARENT_QUEUE_QUERYSTRING]);
            }

            if (args.ContainsKey(AppConsts.ORDER_NUMBER))
            {
                OrderNumber = Convert.ToString(args[AppConsts.ORDER_NUMBER]);
            }

            if (args.ContainsKey("OrganizationUserId"))
            {
                ApplicantOrgUserID = Convert.ToInt32(args["OrganizationUserId"]);
            }

            if (args.ContainsKey("UserId"))
            {
                String userId = (args["UserId"]);
                UserId = new Guid(userId);

            }
        }

        /// <summary>
        /// To redirect to Dashboard
        /// </summary>
        private void RedirectToDashboard()
        {
            String url = String.Format(ParentControl);
            Response.Redirect(url);
        }

        /// <summary>
        /// To get applicant Order cart
        /// </summary>
        /// <returns></returns>
        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (_applicantOrderCart.IsNull())
            {
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return _applicantOrderCart;
        }

        /// <summary>
        /// Set data in ApplicantOrderCart session to match the data after payment.
        /// </summary>
        private void SetSessionData()
        {
            //Add the data to session.
            Order currentOrder = Presenter.GetOrderByOrderId();
            _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            if (_applicantOrderCart == null)
                _applicantOrderCart = new ApplicantOrderCart();
            String clientMachineIP = Request.UserHostAddress;
            _applicantOrderCart.AddOrganizationUserProfile(OrganizationUserProfile, false, clientMachineIP);
            _applicantOrderCart.OrderId = _applicantOrderCart.lstApplicantOrder[0].OrderId = CurrentViewContext.OrderId;
            _applicantOrderCart.lstApplicantOrder[0].OrderNumber = CurrentViewContext.OrderNumber;
            _applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.SSN = CurrentViewContext.SocialSecurityNumber;//GetDecryptedSSN(latestOrganizationUserProfile.OrganizationUserProfileID, tenantId); ;
            //Set orderpaymentdeatilID 
            _applicantOrderCart.OrderPaymentdetailId = CurrentViewContext.OrderPaymentDetailID;
            //_applicantOrderCart.lstApplicantOrder
            //set stage of the order page
            _applicantOrderCart.AddOrderStageTrackID(OrderStages.OrderPaymentDetails);
            //Set DPPS Id
            _applicantOrderCart.AddDeptProgramPackageSubscriptionId(CurrentViewContext.DPPSId);
            _applicantOrderCart.lstDepProgramMappingId = new List<Int32> { CurrentViewContext.DPM_ID };
            // _applicantOrderCart.SelectedDeptProgramId = CurrentViewContext.DPM_ID;
            _applicantOrderCart.ProgramDuration = Convert.ToInt32(CurrentViewContext.DurationMonths);
            //if (CurrentViewContext.DeptProgramPackage.IsNotNull())
            //{
            //    _applicantOrderCart.DPP_Id = CurrentViewContext.DeptProgramPackage.DPP_ID;
            //}
            if (!TotalOrderValue.IsNullOrEmpty())
            {
                //_applicantOrderCart.Amount = TotalOrderValue.Substring(1);
                _applicantOrderCart.Amount = TotalOrderValue;
            }
            if (!GrandTotalPrice.IsNullOrEmpty())
            {
                _applicantOrderCart.GrandTotal = Convert.ToDecimal(GrandTotalPrice);
                _applicantOrderCart.CurrentPackagePrice = Convert.ToDecimal(GrandTotalPrice);
            }
            _applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();

            if (!RushOrderPrice.IsNullOrEmpty())
            {
                _applicantOrderCart.IsRushOrderIncluded = true;
                _applicantOrderCart.RushOrderPrice = RushOrderPrice;
            }
            if (!_applicantOrderCart.CompliancePackages.IsNullOrEmpty())
            {
                _applicantOrderCart.CompliancePackageID = CurrentViewContext.PackageId;
            }
            _applicantOrderCart.IsLocationServiceTenant = CurrentViewContext.IsLocationServiceTenant;
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                Boolean isBkgPackageIncluded = false;
                List<OrderPaymentDetail> PaymentDetailList = Presenter.GetOrderPaymentDetails(currentOrder);
                isBkgPackageIncluded = Presenter.IsBackgroundPackageIncluded(PaymentDetailList);

                if (isBkgPackageIncluded)
                {
                    List<BkgOrderPackage> bkgOrderPkgLst = new List<BkgOrderPackage>();
                    bkgOrderPkgLst = Presenter.GetBkgOrderPackageDetail(PaymentDetailList);
                    AddBackgroundPackageDataToSession(_applicantOrderCart, bkgOrderPkgLst, currentOrder.OrderID);
                }
                //UAT - 3723
                if (orderDetailContracts.Count > 0)
                {
                    _applicantOrderCart.lstOrderLineItems = CurrentViewContext.orderDetailContracts.Select(x => new OrderLineItem
                    {
                        OrderName = x.ServiceName,
                        Price = x.Price,
                        Amount = x.Amount,
                        Quantity = x.Quantity,
                        FCAdditionalPrice = x.FCAdditionalPrice,
                        PPQuantity = x.PPQuantity,
                        PPAdditionalPrice = x.PPAdditionalPrice
                    }).ToList();
                }
                if (!CurrentViewContext.OrderPaymentDetailList.IsNullOrEmpty() && CurrentViewContext.OrderPaymentDetailList.Count > 0)
                {
                    _applicantOrderCart.FingerPrintData = new FingerPrintAppointmentContract();
                    if (!CurrentViewContext.lstFingerPrintData.IsNotNull())
                        _applicantOrderCart.FingerPrintData.CBIUniqueID = CurrentViewContext.lstFingerPrintData.IsNotNull() ? CurrentViewContext.lstFingerPrintData.CBIUniqueID : string.Empty;

                    foreach (var item in CurrentViewContext.OrderPaymentDetailList)
                    {
                        if (!item.OPD_Amount.IsNullOrEmpty())
                        {
                            _applicantOrderCart.FingerPrintData.BillingCodeAmount = Convert.ToDecimal(item.OPD_Amount);

                            Entity.ClientEntity.OrderBillingCodeMapping orderBillingCodeMapping = new Entity.ClientEntity.OrderBillingCodeMapping();
                            orderBillingCodeMapping = Presenter.GetOrderBillingCode(CurrentViewContext.OrderId);

                            if (!orderBillingCodeMapping.IsNullOrEmpty())
                            {
                                _applicantOrderCart.FingerPrintData.BillingCode = orderBillingCodeMapping.OBCM_BillingCode;
                            }
                        }

                    }

                }
            }

            //Get Residential history for current order

            List<ResidentialHistoryProfile> lstResidentialHistory = new List<ResidentialHistoryProfile>();
            lstResidentialHistory = currentOrder.OrganizationUserProfile.ResidentialHistoryProfiles.Where(resHis => !resHis.RHIP_IsDeleted).ToList();

            //List<PersonAliasProfile> lstPersonAlias = currentOrder.OrganizationUserProfile.PersonAliasProfiles.Where(alias => !alias.PAP_IsDeleted).ToList();

            ResidentialHistoryProfile currentAddressDB = lstResidentialHistory.FirstOrDefault(cnd => cnd.RHIP_IsCurrentAddress && !cnd.RHIP_IsDeleted);

            var addressLookup = Presenter.GetAddressLookupByHandlerId(Convert.ToString(currentAddressDB.Address.AddressHandleID));

            PreviousAddressContract currentAddress = new PreviousAddressContract();

            currentAddress.Address1 = currentAddressDB.Address.Address1;
            currentAddress.Address2 = currentAddressDB.Address.Address2;
            currentAddress.ZipCodeID = currentAddressDB.Address.ZipCodeID.Value;
            currentAddress.Zipcode = addressLookup.ZipCode;
            currentAddress.CityName = addressLookup.CityName;
            currentAddress.StateName = addressLookup.FullStateName;
            currentAddress.Country = addressLookup.CountryName;
            if (currentAddressDB.Address.ZipCodeID.Value > 0)
            {
                currentAddress.CountyName = addressLookup.CountyName;
            }
            currentAddress.ResidenceStartDate = currentAddressDB.RHIP_ResidenceStartDate;
            currentAddress.ResidenceEndDate = currentAddressDB.RHIP_ResidenceEndDate;
            currentAddress.isCurrent = currentAddressDB.RHIP_IsCurrentAddress;
            currentAddress.ResHistorySeqOrdID = currentAddressDB.RHIP_SequenceOrder.IsNullOrEmpty() ? AppConsts.ONE : currentAddressDB.RHIP_SequenceOrder.Value;
            _applicantOrderCart.lstPrevAddresses = new List<PreviousAddressContract>();
            _applicantOrderCart.lstPrevAddresses.Add(currentAddress);


            Int32 resHisSequenceNo = AppConsts.ONE;
            foreach (var addressHist in lstResidentialHistory.Where(add => !add.RHIP_IsDeleted && !add.RHIP_IsCurrentAddress).ToList())
            {
                resHisSequenceNo += AppConsts.ONE;
                var prevAddressLookup = Presenter.GetAddressLookupByHandlerId(Convert.ToString(addressHist.Address.AddressHandleID));

                PreviousAddressContract prevAddress = new PreviousAddressContract();
                prevAddress.Address1 = addressHist.Address.Address1;
                prevAddress.Address2 = addressHist.Address.Address2;
                prevAddress.ZipCodeID = addressHist.Address.ZipCodeID.Value;
                prevAddress.Zipcode = prevAddressLookup.ZipCode;
                prevAddress.CityName = prevAddressLookup.CityName;
                prevAddress.StateName = prevAddressLookup.FullStateName;
                prevAddress.Country = prevAddressLookup.CountryName;
                //prevAddress.CountryId = prevAddressExt.AE_CountryID;
                if (addressHist.Address.ZipCodeID.Value > 0)
                {
                    prevAddress.CountyName = prevAddressLookup.CountyName;
                }
                prevAddress.ResidenceStartDate = addressHist.RHIP_ResidenceStartDate;
                prevAddress.ResidenceEndDate = addressHist.RHIP_ResidenceEndDate;
                prevAddress.isCurrent = addressHist.RHIP_IsCurrentAddress;
                prevAddress.ResHistorySeqOrdID = addressHist.RHIP_SequenceOrder.IsNullOrEmpty() ? resHisSequenceNo : addressHist.RHIP_SequenceOrder.Value;

                _applicantOrderCart.lstPrevAddresses.Add(prevAddress);
                _applicantOrderCart.IsResidentialHistoryVisible = true;
            }




            SkipSubmitForNewSingleCard();
            //SetSelectedHierarchyData();
            //var selectedHierarchyNodeId = GetSelectedHierarchyNodeId();
            //_applicantOrderCart.NodeId = CurrentViewContext.NodeId = Presenter.GetLastNodeInstitutionId(Convert.ToInt32(selectedHierarchyNodeId));
            //_applicantOrderCart.SelectedHierarchyNodeID = selectedHierarchyNodeId != String.Empty ? Convert.ToInt32(selectedHierarchyNodeId) : AppConsts.NONE;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);
        }



        private void AddBackgroundPackageDataToSession(ApplicantOrderCart applicantOrderCart, List<BkgOrderPackage> bkgOrderPkgLst, int orderId)
        {
            List<BackgroundPackagesContract> _lstBackgroundPackages = new List<BackgroundPackagesContract>();
            string BkgOrderServiceDetailsxml = Presenter.GetBkgOrderServiceDetails(orderId);
            bkgOrderPkgLst.ForEach(bop =>
            {
                #region UAT-1867: Added this check to reolve issue Price was not dispaying in completeOrder process for bkgPackage, Added this check temporarily Need to verify again
                List<lkpPaymentOption> lstPaymentOptions = new List<lkpPaymentOption>();
                Boolean? IsInvoiceOnlyAtPackageLevel = null;
                List<BkgPackagePaymentOption> lstBkgPackagePaymentOptions = bop.BkgPackageHierarchyMapping.BkgPackagePaymentOptions.Where(cond => !cond.BPPO_IsDeleted).ToList();
                if (lstBkgPackagePaymentOptions.IsNotNull() && lstBkgPackagePaymentOptions.Count > 0)
                {
                    lstPaymentOptions = lstBkgPackagePaymentOptions.Select(col => col.lkpPaymentOption).ToList();
                }
                if (lstPaymentOptions.Count == 0)
                {
                    IsInvoiceOnlyAtPackageLevel = null;
                }
                else if (lstPaymentOptions.Count == 1)
                {
                    IsInvoiceOnlyAtPackageLevel = lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                }
                else if (lstPaymentOptions.Count == 2)
                {
                    IsInvoiceOnlyAtPackageLevel = lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                }
                else
                {
                    IsInvoiceOnlyAtPackageLevel = false;
                }
                #endregion


                _lstBackgroundPackages.Add(new BackgroundPackagesContract
                {
                    BPAId = bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_ID,
                    BPAName = String.IsNullOrEmpty(bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label) ?
                                            bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name : bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label,
                    IsExclusive = bop.BkgPackageHierarchyMapping.BPHM_IsExclusive,
                    BPHMId = bop.BkgPackageHierarchyMapping.BPHM_ID,
                    BasePrice = (bop.BOP_BasePrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_BasePrice.Value),
                    //+ (bop.BOP_TotalLineItemPrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_TotalLineItemPrice.Value) 
                    TotalBkgPackagePrice = (bop.BOP_BasePrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_BasePrice.Value)
                                                    + (bop.BOP_TotalLineItemPrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_TotalLineItemPrice.Value),
                    TotalLineItemPrice = bop.BOP_TotalLineItemPrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_TotalLineItemPrice.Value,
                    IsInvoiceOnlyAtPackageLevel = IsInvoiceOnlyAtPackageLevel

                });
            });
            XmlDocument xmlDocumnet = new XmlDocument();
            if (!BkgOrderServiceDetailsxml.IsNullOrEmpty())
            {
                xmlDocumnet.LoadXml(BkgOrderServiceDetailsxml);
                XmlNodeList elemlist = xmlDocumnet.GetElementsByTagName("BkgpkgData");

                foreach (XmlNode node in elemlist)
                {
                    var PackageId = "";
                    var ServiceCode = "";
                    OrderLineItem ordrLinItm = new OrderLineItem();
                    if (node.HasChildNodes)
                    {

                        for (var i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == "ServiceType")
                            {

                                PackageId = Presenter.GetPackageNameForCompleteOrder(orderId, node.ChildNodes[i].InnerText, true);
                                ServiceCode = node.ChildNodes[i].InnerText;

                            }

                            if (node.ChildNodes[i].Name == "NumberOfCopies")
                            {

                                {
                                    ordrLinItm.Quantity = Int32.Parse(node.ChildNodes[i].InnerText);
                                }
                            }
                        }

                        _lstBackgroundPackages.ForEach(pkg =>
                        {
                            if (pkg.BPAId == Int16.Parse(PackageId))
                            {
                                if (!ServiceCode.IsNullOrEmpty())
                                    pkg.ServiceCode = ServiceCode;
                                if (ordrLinItm.Quantity.HasValue)
                                    pkg.CopiesCount = (int)(ordrLinItm.Quantity);
                            }

                        });



                    }


                }
            }
            _applicantOrderCart.lstApplicantOrder[0].lstPackages = _lstBackgroundPackages;
        }

        private void SkipSubmitForNewSingleCard()
        {
            List<INTSOF.AuthNet.Business.PaymentProfileDetail> lstOldPaymentProfileDetails = new List<INTSOF.AuthNet.Business.PaymentProfileDetail>();
            //long customerProfileId = 0;
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            Entity.AuthNetCustomerProfile customerProfile = Presenter.GetCustomerProfile(user.UserId);
            _applicantOrderCart.lstOldCustomerPaymentProfileId = new List<Int64>();

            if (!customerProfile.IsNullOrEmpty())
            {
                lstOldPaymentProfileDetails = INTSOF.AuthNet.Business.AuthorizeNetCreditCard.GetCustomerPaymentProfiles(Convert.ToInt64(customerProfile.CustomerProfileID));
                if (!lstOldPaymentProfileDetails.IsNullOrEmpty())
                    _applicantOrderCart.lstOldCustomerPaymentProfileId = lstOldPaymentProfileDetails.Select(sel => sel.CustomerPaymentProfileId).ToList();
            }
        }

        /// <summary>
        /// To check Order
        /// </summary>
        private void CheckOrder()
        {
            _applicantOrderCart = GetApplicantOrderCart();

            if (_applicantOrderCart != null && _applicantOrderCart.lstApplicantOrder[0].OrderId != AppConsts.NONE && !_applicantOrderCart.IsAdminOrderCart)
            {
                Order order = Presenter.GetOrderById(_applicantOrderCart.lstApplicantOrder[0].OrderId);
                if (order.IsNotNull())
                {
                    RedirectIfIncorrectOrderStage(_applicantOrderCart);
                    CheckOrderStatus(order);
                }
            }
        }

        /// <summary>
        /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void RedirectIfIncorrectOrderStage(ApplicantOrderCart _applicantOrderCart)
        {
            Presenter.GetNextPagePathByOrderStageID(_applicantOrderCart);

            //Redirect to next page path if Order Status track is not correct.
            if (NextPagePath.IsNotNull())
            {
                Response.Redirect(NextPagePath);
            }
            else
            {
                _applicantOrderCart.AddOrderStageTrackID(OrderStages.OrderPaymentDetails);
            }
        }

        /// <summary>f
        /// To check Order status
        /// </summary>
        /// <param name="order"></param>
        private void CheckOrderStatus(Order order)
        {
            OrderPaymentDetail orderPaymentDetail = null;
            if (_applicantOrderCart.IsNotNull())
            {
                orderPaymentDetail = order.OrderPaymentDetails.Where(cond => cond.OPD_ID == _applicantOrderCart.OrderPaymentdetailId && cond.OPD_IsDeleted == false).FirstOrDefault();
                if (orderPaymentDetail.IsNotNull() && orderPaymentDetail.lkpOrderStatu.IsNotNull())
                {
                    String orderStatus = orderPaymentDetail.lkpOrderStatu.Code;
                    if (orderStatus == ApplicantOrderStatus.Paid.GetStringValue())
                    {
                        CheckOnlinePayment();
                    }
                }
            }
        }

        /// <summary>
        /// To check payment and redirect to Order confirmation page
        /// </summary>
        private void CheckOnlinePayment()
        {
            try
            {
                ErrorLog logFile = new ErrorLog("Data is sent from OrderPaymentDetails page.");
                RedirectToOrderConfirmation();
            }
            catch (Exception ex)
            {
                ErrorLog logFile = new ErrorLog("Problem in sending data from OrderPaymentDetails page" + ex);
            }
        }

        /// <summary>
        /// To redirect to Order confirmation page
        /// </summary>
        private void RedirectToOrderConfirmation()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  ChildControls.ApplicantOrderConfirmation }
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }

        #region Action Permission

        /// <summary>
        /// Set action level permissions
        /// </summary>
        /// <param name="ctrlCollection">ctrlCollection</param>
        /// <param name="screenName">screenName</param>
        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();
        }

        /// <summary>
        /// Set the permission on control based action permission 
        /// </summary>
        private void ApplyPermisions()
        {
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                            {

                                if (x.FeatureAction.CustomActionId == "Next")
                                {
                                    cbbuttons.ClearButton.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "ApprovePayment")
                                {
                                    //UAT-916
                                    //btnApprovePayment.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "RejectPayment")
                                {
                                    //UAT-916
                                    //btnRejectPayment.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "RejectCancellation")
                                {
                                    btnRejectCancellation.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "ApprovedCancellation")
                                {
                                    btnApproveCancellation.Enabled = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {

                                if (x.FeatureAction.CustomActionId == "Next")
                                {
                                    cbbuttons.ClearButton.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "ApprovePayment")
                                {
                                    //UAT-916
                                    //btnApprovePayment.Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "RejectPayment")
                                {
                                    //UAT-916
                                    //btnRejectPayment.Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "RejectCancellation")
                                {
                                    btnRejectCancellation.Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "ApprovedCancellation")
                                {
                                    btnApproveCancellation.Visible = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }

        #endregion

        /// <summary>
        /// Copy Data from AMS(Background) Package to Compliance Package
        /// </summary>
        private void CopyBkgDataToCompliancePackage()
        {
            /*[UAT-916 : WB: As an application admin, I should be able to define payment options at the package level in addition to the node level]
              Added CompliancePkgPaymentDetail check to CopyBkgdataToCompliancePackage if approved order payment include compliance package.*/

            if (Presenter.IsComplianceAndFreshOrder() && CurrentViewContext.IsCompliancePackageInclude)
            {
                Presenter.CopyBkgDataToCompliancePackage();
            }
        }

        /*UAT-916 ToDO
        //private void CancelOrder(String message, Int32? orderPaymentDetailID = null, Boolean? IsCompliancePackageInclude = null)
        //{
        //    if (Presenter.ApproveCancellations(orderPaymentDetailID, IsCompliancePackageInclude))
        //    {
        //        ShowSuccessMessage(message);
        //        Presenter.GetOrderDetailsAndSetControls();
        //        DisplayApproveButtons();

        //        //UAT-966: As an admin, I should be able to cancel individual parts of an order.
        //        ShowBackgroundPackages();
        //        ShowPartialOrderCancellationPackages();

        //        Presenter.SendOrderCancellationApprovalNotification();
        //    }
        //}*/

        private void CancelOrder(String message, Boolean isCancelledByApplicant)
        {
            if (Presenter.ApproveCancellations(isCancelledByApplicant))
            {
                ShowSuccessMessage(message);
                Presenter.GetOrderDetailsAndSetControls();
                DisplayApproveButtons();

                //UAT-966: As an admin, I should be able to cancel individual parts of an order.
                ShowBackgroundPackages();
                ShowPartialOrderCancellationPackages();

                Presenter.SendOrderCancellationApprovalNotification();
            }
        }

        /// <summary>
        /// Show/Hide Refund section, based on the order date, order status and payment options
        /// </summary>
        private void ShowHideRefund()
        {
            var isOrderCancelled_Cancellation_Requested = CurrentViewContext.OrderPaymentDetailList
                                                        .Any(cond => cond.lkpOrderStatu != null && (cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue()
                                                        || cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue()));

            if (isOrderCancelled_Cancellation_Requested
                || (!String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode) && hdnIsPartialOrderCancellation.Value == "1"
                && Convert.ToDecimal(hdnPartialOrderCancellationAmount.Value) > 0))
            {
                List<OrderPaymentDetail> creditCardOrderPaymentDetailList = CurrentViewContext.OrderPaymentDetailList.Where(cnd => cnd.lkpPaymentOption != null
                                                                              && cnd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue() && cnd.OPD_Amount > AppConsts.NONE).ToList();

                if (creditCardOrderPaymentDetailList.IsNotNull() && creditCardOrderPaymentDetailList.Count() > AppConsts.NONE)
                {
                    hdfTotalRefundCount.Value = Convert.ToString(creditCardOrderPaymentDetailList.Count);
                    rptRefundOrder.DataSource = creditCardOrderPaymentDetailList;
                    rptRefundOrder.DataBind();
                    if (Convert.ToInt32(hdfVisibleRefundCount.Value) > AppConsts.NONE)
                        divRefund.Visible = true;
                    else
                        divRefund.Visible = false;

                }
                else
                {
                    divRefund.Visible = false;
                }

            }
            //else if 
            //{

            //}
            else
            {
                divRefund.Visible = false;
            }



            //Hide below code to implement UAT-916
            //    //var _order = CurrentViewContext.OrderPaymentDetail.Order;
            //    var _order = creditCardOrderPaymentDetail.Order;
            //    DateTime? _dt = new DateTime();

            //    // If Applicant request cancellation of order, which was not approved yet and was in 'Sent for Online payment status'
            //    /*
            //    //if (CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue())*/
            //    if (creditCardOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue())
            //        _dt = _order.OrderDate;
            //    else
            //        _dt = _order.ApprovalDate;

            //    TimeSpan? ts = DateTime.Now - _dt;

            //    // If order time is less than 120 Days
            //    // AND Order status is Cancellation Requested OR Cancelled
            //    // AND Payment type is Credit Card
            //    if (
            //            ShowApproveRejectButtons
            //            &&
            //            (!_dt.IsNullOrEmpty() && !ts.IsNullOrEmpty() && ts.Value.TotalDays < 120)
            //            &&
            //            (
            //                creditCardOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue()
            //                ||
            //                creditCardOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue()
            //            )
            //            &&
            //            (
            //                creditCardOrderPaymentDetail.OPD_PaymentOptionID.IsNotNull()
            //                &&
            //                creditCardOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()
            //            )
            //       )
            //    {
            //        txtOriginalPrice.Text = txtGrandTotal.Text;
            //        SetNetAmount(false);
            //        divRefund.Visible = true;
            //    }
            //    /* UAT-916
            //else if (ShowApproveRejectButtons && (!_dt.IsNullOrEmpty() && !ts.IsNullOrEmpty() && ts.Value.TotalDays < 120)
            //       && CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Paid.GetStringValue()
            //       && CurrentViewContext.PaymentTypeCode == PaymentOptions.Credit_Card.GetStringValue()
            //       && hdnIsPartialOrderCancellation.Value == "1")*/
            //    else if (ShowApproveRejectButtons && (!_dt.IsNullOrEmpty() && !ts.IsNullOrEmpty() && ts.Value.TotalDays < 120)
            //           && creditCardOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue()
            //           && creditCardOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()
            //           && hdnIsPartialOrderCancellation.Value == "1")
            //    {
            //        txtOriginalPrice.Text = txtGrandTotal.Text;
            //        SetNetAmount(false);
            //        divRefund.Visible = true;
            //    }
            //    else
            //        divRefund.Visible = false;
            //}
            //else
            //{
            //    divRefund.Visible = false;
            //}
        }

        /// <summary>
        /// Set the net amount after deducting the Refund
        /// </summary>
        private void SetNetAmount(Boolean isRefundClicked, WclNumericTextBox txtTotalRefund, WclNumericTextBox txtOriginalPrice, WclNumericTextBox txtNetPrice,
                                    Int32 orderPaymentDetailID)
        {
            var lstRefunds = Presenter.GetRefundHistory();
            if (!lstRefunds.IsNullOrEmpty())
            {
                var orderPaymentDetailRefund = lstRefunds.Where(cond => cond.RH_OrderPaymentDetailID == orderPaymentDetailID);
                var _totalRefund = orderPaymentDetailRefund.IsNotNull() ? orderPaymentDetailRefund.Sum(rh => rh.RH_Amount) : AppConsts.NONE;
                txtTotalRefund.Text = Convert.ToString(_totalRefund);
                txtNetPrice.Text = Convert.ToString(Convert.ToDecimal(txtOriginalPrice.Text) - _totalRefund);

                if (hdnIsPartialOrderCancellation.Value == "1" && !isRefundClicked)
                {
                    hdnPartialOrderCancellationAmount.Value = Convert.ToString(Convert.ToDecimal(hdnPartialOrderCancellationAmount.Value) - Convert.ToDecimal(_totalRefund));
                }
            }
            else
            {
                txtNetPrice.Text = txtTotalRefund.Text = AppConsts.ZERO;
            }
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.IsDOBDisable)
            {
                divDOB.Visible = false;
            }
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                divSSNMasked.Visible = false;
            }
            else if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                divSSNMasked.Visible = true;
            }
        }
        #endregion

        private void DisplayAutoRenewalButton()
        {
            if (CurrentViewContext.ShowAutoRenewalControl && !(CurrentViewContext.OrganizationUserId == CurrentViewContext.CurrentLoggedInUserId))
            {
                btnAutoRenewal.Visible = true;
                lblAutoRenewal.Visible = true;
                dvAutoRenewal.Style.Add("display", "block");
                if (CurrentViewContext.DisableAutoRenewalControl)
                {
                    //btnAutoRenewal.Enabled = false;
                    btnAutoRenewal.Text = "OFF";
                    btnAutoRenewal.ToolTip = String.Empty;
                    btnAutoRenewal.Attributes.Add("Enabled", "false");
                    btnAutoRenewal.CssClass = "autoRenewalLinkOffButton";
                }
                else
                {
                    btnAutoRenewal.CssClass = "autoRenewalLink";
                    btnAutoRenewal.Text = CurrentViewContext.AutomaticRenewalTurnedOff ? "OFF " : "ON ";
                    btnAutoRenewal.ToolTip = CurrentViewContext.AutomaticRenewalTurnedOff ? Resources.Language.CLKATRNWLON : Resources.Language.CLKATRNWLOFF;
                }

            }
            else
            {
                dvAutoRenewal.Style.Add("display", "none");
                btnAutoRenewal.Visible = false;
                lblAutoRenewal.Visible = false;
            }
        }

        private void ShowPartialOrderCancellationPackages()
        {
            var isOrderCancelled_Cancellation_Requested = CurrentViewContext.OrderPaymentDetailList
                                                       .All(cond => cond.lkpOrderStatu != null && (cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue()
                                                       || cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue()));
            if ((!ShowApproveRejectButtons || ParentControl.IsNotNull()))
            {
                if (!String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                    && CurrentViewContext.PartialOrderCancellationTypeCode != PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue())
                {
                    txtPartialCancelCompliancePkgStatus.Visible = true;
                    if (CurrentViewContext.IsCompliancePackageCancelledByChangeSubs)
                    {
                        txtPartialCancelCompliancePkgStatus.Text = Resources.Language.CMPLNCPKGCNCLSBCRPTN;
                    }
                    else
                    {
                        txtPartialCancelCompliancePkgStatus.Text = Resources.Language.CMPLNCPKGCNCL;
                    }
                    chkPartialCancelCompliancePkg.Visible = false;
                    btnPartialOrderCancellation.Visible = false;
                    //UAT-2217
                    rdbIsGraduatedCompliance.Enabled = false;
                }
                else
                {
                    chkPartialCancelCompliancePkg.Visible = false;
                    btnPartialOrderCancellation.Visible = false;
                    dvCancelCompliancePkg.Visible = false;
                    //UAT-2217
                    if (isOrderCancelled_Cancellation_Requested)
                    {
                        rdbIsGraduatedCompliance.Enabled = false;
                        rdbIsGraduatedBackground.Enabled = false;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                    && CurrentViewContext.PartialOrderCancellationTypeCode != PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue())
                {
                    dvCancelCompliancePkg.Visible = true;
                    chkPartialCancelCompliancePkg.Checked = true;
                    chkPartialCancelCompliancePkg.Visible = false;
                    txtPartialCancelCompliancePkgStatus.Visible = true;
                    if (CurrentViewContext.IsCompliancePackageCancelledByChangeSubs)
                    {
                        txtPartialCancelCompliancePkgStatus.Text = Resources.Language.CMPLNCPKGCNCLSBCRPTN;
                    }
                    else
                    {
                        txtPartialCancelCompliancePkgStatus.Text = Resources.Language.CMPLNCPKGCNCL;
                    }
                    hdnIsPartialOrderCancellation.Value = "1";

                    if (CurrentViewContext.CompPkgOrderPaymentDetail.IsNotNull()
                        && CurrentViewContext.CompPkgOrderPaymentDetail.lkpPaymentOption.IsNotNull()
                        && CurrentViewContext.CompPkgOrderPaymentDetail.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue())
                    {
                        hdnPartialOrderCancellationAmount.Value = Convert.ToString(Convert.ToDecimal(hdnPartialOrderCancellationAmount.Value)
                                                                  + Convert.ToDecimal(txtTotalOrderValue.Text));
                    }

                    //if (CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Cancelled.GetStringValue()
                    //    || CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Cancellation_Requested.GetStringValue())
                    if (CurrentViewContext.CompPkgOrderPaymentDetail.IsNotNull()
                        && CurrentViewContext.CompPkgOrderPaymentDetail.lkpOrderStatu.IsNotNull()
                        && (CurrentViewContext.CompPkgOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue()
                        || CurrentViewContext.CompPkgOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue()))
                    {
                        if (isOrderCancelled_Cancellation_Requested)
                            btnPartialOrderCancellation.Visible = false;
                    }
                    //bool flag = false;
                    //CurrentViewContext.OrderDetail.PackageSubscription.
                    //flag = CurrentViewContext.OrderPaymentDetailList
                    //       .All(cond => cond.lkpOrderStatu != null && (cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue()
                    //       || cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue()));
                    //if (flag)

                }
                /* UAT-916
                 * else if (!String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                     && CurrentViewContext.PartialOrderCancellationTypeCode == PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue()
                     && (CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Cancelled.GetStringValue()
                         || CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Cancellation_Requested.GetStringValue()))*/
                //else if (!String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                //     && CurrentViewContext.PartialOrderCancellationTypeCode == PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue()
                //     && (CurrentViewContext.OrderPaymentDetailList.IsNotNull() && CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().lkpOrderStatu.IsNotNull()
                //             && (CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue() ||
                //             CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue())))
                else if (!String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                    && CurrentViewContext.PartialOrderCancellationTypeCode == PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue()
                    && (CurrentViewContext.CompPkgOrderPaymentDetail.IsNotNull() && CurrentViewContext.CompPkgOrderPaymentDetail.lkpOrderStatu.IsNotNull()
                    && (CurrentViewContext.CompPkgOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue() ||
                        CurrentViewContext.CompPkgOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue())))
                {
                    dvCancelCompliancePkg.Visible = false;
                    if (isOrderCancelled_Cancellation_Requested)
                        btnPartialOrderCancellation.Visible = false;
                }
                /*UAT-916
                 * else if (String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                    && (CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Cancelled.GetStringValue() ||
                    CurrentViewContext.OrderStatusCode == ApplicantOrderStatus.Cancellation_Requested.GetStringValue()))*/
                //else if (String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                //&& (CurrentViewContext.OrderPaymentDetailList.IsNotNull() && CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().lkpOrderStatu.IsNotNull()
                //             && (CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue() ||
                //             CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue())))
                else if (String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                && (CurrentViewContext.CompPkgOrderPaymentDetail.IsNotNull() && CurrentViewContext.CompPkgOrderPaymentDetail.lkpOrderStatu.IsNotNull()
                    && (CurrentViewContext.CompPkgOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue() ||
                        CurrentViewContext.CompPkgOrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue())))
                {
                    dvCancelCompliancePkg.Visible = false;
                    if (isOrderCancelled_Cancellation_Requested)
                        btnPartialOrderCancellation.Visible = false;
                }

                // else
                // {
                //    dvCancelCompliancePkg.Visible = true;
                //    btnPartialOrderCancellation.Visible = true;
                // }
            }
        }

        private Boolean CheckIfBkgPartialCancelledPkg_IsCC(Int32 bopID)
        {
            List<OrderPaymentDetail> creditCardOrderPaymentDetailList = CurrentViewContext.OrderPaymentDetailList.Where(cnd => cnd.lkpPaymentOption != null
                                                                               && cnd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue() && cnd.OPD_Amount > AppConsts.NONE).ToList();
            Boolean isBkgCancelledPkgCC = false;
            foreach (var item in creditCardOrderPaymentDetailList)
            {
                if (!isBkgCancelledPkgCC)
                {
                    isBkgCancelledPkgCC = item.OrderPkgPaymentDetails.Any(x => x.OPPD_OrderPaymentDetailID == item.OPD_ID
                                                            && item.OPD_IsDeleted == false
                                                            && x.OPPD_BkgOrderPackageID == bopID);
                }
            }
            return isBkgCancelledPkgCC;
        }

        private void ShowBackgroundPackages()
        {
            //if (OrderPackageType != AppConsts.ONE)
            if (OrderPackageTypeCode != OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
            {
                Presenter.GetCancelledBkgOrderData();
                rptBackgroundPackages.DataSource = BkgPackagesList;
                rptBackgroundPackages.DataBind();
                //UAT-4537
                CurrentViewContext.lstPendingApprovalPackageNames = !CurrentViewContext.BkgPackagesList.AsEnumerable().Where(cond => cond.Field<String>("OrderStatusCode") == "OSPSA").ToList().Select(sel => sel.Field<String>("BkgPackageLabel")).ToList().IsNullOrEmpty()
                                ? CurrentViewContext.BkgPackagesList.AsEnumerable().Where(cond => cond.Field<String>("OrderStatusCode") == "OSPSA").ToList().Select(sel => sel.Field<String>("BkgPackageLabel")).ToList() : new List<string>(); ;
                if (BkgPackagesList.IsNotNull() && BkgPackagesList.Rows.Count == AppConsts.ONE
                    && !String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                    && CurrentViewContext.PartialOrderCancellationTypeCode != PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue())
                {
                    btnPartialOrderCancellation.Visible = false;
                }
                else if (BkgPackagesList.IsNotNull() && BkgPackagesList.Rows.Count == AppConsts.ONE
                        && String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                        && OrderPackageTypeCode == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
                {
                    btnPartialOrderCancellation.Visible = false;
                }
                else if (BkgPackagesList.IsNotNull() && BkgPackagesList.Rows.Count > AppConsts.NONE)
                {
                    var bkgPackagesList = BkgPackagesList.Select().Select(col => col).ToList();
                    if (bkgPackagesList.All(col => Convert.ToBoolean(col["IsPartialOrderCancelled"]))
                        //&& (!CurrentViewContext.IsCompliancePartialOrderCancelled || chkPartialCancelCompliancePkg.Visible))
                        && !String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                        )
                    {
                        dvCancelCompliancePkg.Visible = false;
                        btnPartialOrderCancellation.Visible = false;
                    }
                    //else if ((bkgPackagesList.Where(cond => !Convert.ToBoolean(cond["IsPartialOrderCancelled"])).Count() == AppConsts.ONE)
                    //    //&& (!CurrentViewContext.IsCompliancePartialOrderCancelled && !chkPartialCancelCompliancePkg.Visible)
                    //         && !String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)
                    //        )
                    //{
                    //    dvCancelCompliancePkg.Visible = false;
                    //    btnPartialOrderCancellation.Visible = false;
                    //}
                    else if ((bkgPackagesList.Where(cond => !Convert.ToBoolean(cond["IsPartialOrderCancelled"])).Count() == AppConsts.ONE)
                        && (CurrentViewContext.IsCompliancePartialOrderCancelled))
                    {
                        btnPartialOrderCancellation.Visible = false;
                    }

                    //else if ((bkgPackagesList.Where(cond => !Convert.ToBoolean(cond["IsPartialOrderCancelled"])).Count() == AppConsts.ONE)
                    //   && (CurrentViewContext.IsCompliancePartialOrderCancelled && chkPartialCancelCompliancePkg.Visible))
                    //{
                    //    btnPartialOrderCancellation.Visible = false;
                    //}
                }
            }
            //else if (OrderPackageTypeCode != OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()
            //&& (!String.IsNullOrEmpty(CurrentViewContext.PartialOrderCancellationTypeCode)))
            //{
            //    Presenter.GetCancelledBkgOrderData();
            //    rptBackgroundPackages.DataSource = BkgPackagesList;
            //    rptBackgroundPackages.DataBind();
            //}


            //if (OrderPackageType == AppConsts.ONE)
            if (OrderPackageTypeCode == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
            {
                divCompliancePackage.Visible = true;
                divBackgroundPackage.Visible = false;
                dvCancelCompliancePkg.Visible = false;
                btnPartialOrderCancellation.Visible = false;
            }

            //if (OrderPackageType == AppConsts.TWO)
            else if (OrderPackageTypeCode == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
            {
                divCompliancePackage.Visible = false;
                divBackgroundPackage.Visible = true;
                dvCancelCompliancePkg.Visible = false;
            }

            //if (OrderPackageType == AppConsts.THREE)
            else if (OrderPackageTypeCode == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue())
            {
                divCompliancePackage.Visible = true;
                divBackgroundPackage.Visible = true;
            }

            //UAT-1558:As a Student, I should be able to mark when I have "Graduated" from a tracking and/or screening package's corresponding program
            if (IsApplicant)
            {
                //for CompliancePackage Div
                if (OrderPackageTypeCode == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue() || OrderPackageTypeCode == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
                {
                    dvSaveIsGraduatedCompliance.Visible = true;
                    divCancellationDetails.Visible = false;  //UAT 4490
                    if (ArchiveStateCode == ArchiveState.Graduated.GetStringValue() || ArchiveStateCode == ArchiveState.Archived_and_Graduated.GetStringValue())
                    {
                        // tgIsGraduatedCompliance.SelectedToggleStateIndex = 1;
                        rdbIsGraduatedCompliance.SelectedValue = "True";
                    }
                    else if (ArchiveStateCode == ArchiveState.Package_Subscription_Cancelled.GetStringValue())
                    {
                        rdbIsGraduatedCompliance.Enabled = false;
                    }
                    else if (ArchiveStateCode != ArchiveState.Package_Subscription_Cancelled.GetStringValue())
                    {
                        //tgIsGraduatedCompliance.SelectedToggleStateIndex = 0;
                        rdbIsGraduatedCompliance.SelectedValue = "False";
                    }
                }
                else
                {
                    dvSaveIsGraduatedCompliance.Visible = false;
                }
                //for BackgroundPackage Div                
                if (BkgPackagesList.IsNotNull() && BkgPackagesList.Rows.Count > AppConsts.NONE
                    && (OrderPackageTypeCode == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue()
                    || OrderPackageTypeCode == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()))
                {
                    dvSaveIsGraduatedBackground.Visible = true;
                    if (Convert.ToString(BkgPackagesList.Rows[0]["BkgArchiveStateCode"]).IsNotNull() &&
                        (CurrentViewContext.BkgArchiveStateCode == ArchiveState.Graduated.GetStringValue()
                        || CurrentViewContext.BkgArchiveStateCode == ArchiveState.Archived_and_Graduated.GetStringValue()))
                    {
                        //UAT-2217
                        //tgIsGraduatedBackground.SelectedToggleStateIndex = 1;
                        rdbIsGraduatedBackground.SelectedValue = "True";

                    }
                    else
                    {
                        //UAT-2217
                        //tgIsGraduatedBackground.SelectedToggleStateIndex = 0;
                        rdbIsGraduatedBackground.SelectedValue = "False";
                    }
                }
                else
                {
                    dvSaveIsGraduatedBackground.Visible = false;
                }
            }
        }

        private void GetServiceForms()
        {
            /*UAT-916
            if (OrderPackageTypeCode != OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue() && CurrentViewContext.OrderPaymentDetail.IsNotNull()
                && CurrentViewContext.OrderPaymentDetail.Order.IsNotNull() && CurrentViewContext.OrderPaymentDetail.Order.lkpOrderStatu.IsNotNull()
                && CurrentViewContext.OrderPaymentDetail.Order.lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue()
                && CurrentViewContext.CurrentLoggedInUserId == CurrentViewContext.OrderPaymentDetail.Order.OrganizationUserProfile.OrganizationUser.OrganizationUserID)*/
            if (CurrentViewContext.OrderPkgPaymentDetailList.IsNotNull() && CurrentViewContext.OrderPaymentDetailList.IsNotNull()
                && CurrentViewContext.OrderPkgPaymentDetailList.Any(cnd => cnd.lkpOrderPackageType.OPT_Code != OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue() && cnd.lkpOrderPackageType.OPT_Code != OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue() && cnd.OrderPaymentDetail.IsNotNull() && cnd.OrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue())
                && CurrentViewContext.CurrentLoggedInUserId == CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().Order.OrganizationUserProfile.OrganizationUser.OrganizationUserID)
            {
                Presenter.GetAutomaticServiceFormForOrder();

                if (CurrentViewContext.lstServiceForm.IsNotNull() && CurrentViewContext.lstServiceForm.Count > 0)
                {
                    divSvcFrm.Visible = true;
                    grdServiceForms.DataSource = CurrentViewContext.lstServiceForm;
                }
                else
                {
                    divSvcFrm.Visible = false;
                }
            }
            else
            {
                divSvcFrm.Visible = false;
            }
        }

        private void HideShowServiceLevelDetails()
        {

            //if (IsApplicant &&
            //    !CurrentViewContext.OrderPkgPaymentDetailList.IsNullOrEmpty() && !CurrentViewContext.OrderPaymentDetailList.IsNullOrEmpty()
            //    && CurrentViewContext.OrderPkgPaymentDetailList.Any(cnd => cnd.lkpOrderPackageType.OPT_Code != OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue() && cnd.lkpOrderPackageType.OPT_Code != OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue() && !cnd.OrderPaymentDetail.IsNullOrEmpty() && cnd.OrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue())
            //    && CurrentViewContext.CurrentLoggedInUserId == CurrentViewContext.OrderPaymentDetailList.FirstOrDefault().Order.OrganizationUserProfile.OrganizationUser.OrganizationUserID)
            if (IsApplicant)
            {
                List<ServiceLevelDetailsForOrderContract> LstServiceDetails = Presenter.GetServiceLevelDetailsForOrder(CurrentViewContext.TenantId);
                if (!LstServiceDetails.IsNullOrEmpty() && !CurrentViewContext.IsLocationServiceTenant)
                {
                    divServiceLevelDetails.Visible = true;
                    ServiceLevelDetailsForOrder userControl = LoadControl("~/BkgOperations/UserControl/ServiceLevelDetailsForOrder.ascx") as ServiceLevelDetailsForOrder;
                    userControl.OrderID = CurrentViewContext.OrderId;
                    userControl.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                    divServiceLevelDetails.Controls.Add(userControl);
                }
                else
                {
                    divServiceLevelDetails.Visible = false;
                }
            }
            else
            {
                divServiceLevelDetails.Visible = false;
            }
        }

        private void CreatePartialOrderCancellationXML(StringBuilder partialOrderCancellationXML, Int32 entityID, Boolean isCompliancePackage)
        {
            partialOrderCancellationXML.Append("<PartialOrderCancellation>");
            partialOrderCancellationXML.Append("<IsCompliancePackage>" + Convert.ToInt32(isCompliancePackage) + "</IsCompliancePackage>");
            partialOrderCancellationXML.Append("<EntityID>" + Convert.ToInt32(entityID) + "</EntityID>");
            partialOrderCancellationXML.Append("</PartialOrderCancellation>");
        }

        private void ManageSSN()
        {
            String AppSSN = txtSSN.Text.Trim();
            string AppSSnMasked = txtSSNMAsked.Text.Trim();
            if (AppSSN == AppConsts.DefaultSSN)
            {
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    divSSN.Visible = false;
                }
            }
            if (AppSSnMasked == AppConsts.DefaultSSN)
            {
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    divSSNMasked.Visible = false;
                }
            }
        }
        #endregion

        #endregion

        #region UAT-916 Methods
        private void ApprovePayment(String OrderPaymentStatusCode)
        {
            try
            {
                List<String> lstExistingpaidOrders = new List<String>();
                String strExistingOrders = CurrentViewContext.OrderId.ToString();
                if (!strExistingOrders.IsNullOrEmpty() && hdnConfirmSave.Value == "0")
                {

                    lstExistingpaidOrders = Presenter.IsOrderExistForCurrentYear(strExistingOrders);
                }

                if (lstExistingpaidOrders.Count > 0 && hdnConfirmSave.Value == "0")
                {
                    hdnCurrentOrderPaymentdetailID.Value = CurrentViewContext.OrderPaymentDetailID.ToString();
                    hdnPopUpText.Value = "This user already have an approved order(s) in last 365 days. Do you still want to continue?";
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "confirmClick();", true);
                    return;
                }
                else
                {
                    bool flagIsCardWithApproval = false;
                    bool isADBAdmin = SysXWebSiteUtils.SessionService.IsSysXAdmin;
                    lstOrderQueue = new List<OrderContract>();
                    Presenter.GetOrderDetails(Convert.ToString(CurrentViewContext.OrderId));
                    lstOrderQueue = CurrentViewContext.lstOrderQueue;
                    flagIsCardWithApproval = lstOrderQueue.FirstOrDefault().IsCardWithApproval;

                    if (flagIsCardWithApproval && hdnConfirmApproveOrderPayment.Value == "0" && isADBAdmin)
                    {
                        if (CurrentViewContext.OrderPaymentDetailID > 0)
                        {
                            hdnCurrentOrderPaymentdetailID.Value = CurrentViewContext.OrderPaymentDetailID.ToString();
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCCOrdersRequirApprovalConfirmationPopup();", true);
                        return;
                    }
                    else
                    {
                        hdnConfirmApproveOrderPayment.Value = "0";
                        System.Web.UI.WebControls.RepeaterItem rptItem = null;
                        foreach (RepeaterItem item in rptOrderPAymentDetail.Items)
                        {
                            if (hdnCurrentOrderPaymentdetailID.Value == ((item.FindControl("hdnOrderPaymentDetailID") as HiddenField)).Value)
                            {
                                rptItem = item;
                                break;
                            }
                        }

                        hdnConfirmSave.Value = "0";
                        if (!rptItem.IsNullOrEmpty())
                        {
                            GetRepeaterRowData(rptItem);
                        }
                        //Boolean showSuccessMsg = false;
                        String message = String.Empty;
                        if (OrderPaymentStatusCode.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()))
                        {
                            message = Resources.Language.REQONLNPAYAPRVLSUC;
                        }
                        else if (OrderPaymentStatusCode.Equals(ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue()))
                        {
                            message = Resources.Language.REQONLNPAYNOTCMPLTSCS;
                        }
                        else if (OrderPaymentStatusCode.Equals(ApplicantOrderStatus.Payment_Rejected.GetStringValue()))
                        {
                            message = Resources.Language.REQPAYRJCTAPRVLSUC;
                        }
                        else if (OrderPaymentStatusCode.Equals(ApplicantOrderStatus.Payment_Due.GetStringValue()))
                        {
                            message = Resources.Language.REQPAYDUEAPRVLSUC;
                        }
                        else if (OrderPaymentStatusCode.Equals(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()))
                        {
                            message = Resources.Language.REQPNDNGPAYAPRVLSUC;
                        }
                        //UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
                        else if (OrderPaymentStatusCode.Equals(ApplicantOrderStatus.Pending_School_Approval.GetStringValue()))
                        {
                            Entity.AuthNetCustomerProfile customerProfile = Presenter.GetCustomerProfile(UserId);
                            String _description = Presenter.GenerateDescription();
                            var _ccOPD = Presenter.GetOrdrPaymentDetailByID();
                            long _customerPaymentProfileID = _ccOPD.OPD_CustomerPaymentProfileID.IsNullOrEmpty() ? 0 : Convert.ToInt64(_ccOPD.OPD_CustomerPaymentProfileID);
                            String _invoiceNumber = _ccOPD.OnlinePaymentTransaction.Invoice_num;

                            CreateCustomerProfileTransactionResponseType response = INTSOF.AuthNet.Business.AuthorizeNetCreditCard.CreateCustomerProfileTransaction(Convert.ToInt64(customerProfile.CustomerProfileID), _customerPaymentProfileID,
                                                                                                                                   CurrentViewContext.OrganizationUserId, Convert.ToDecimal(_ccOPD.OPD_Amount),
                                                                                                                                   _invoiceNumber, _description);
                            NameValueCollection transactionDetails = INTSOF.AuthNet.Business.AuthorizeNetCreditCard.GetResponseFields(response);

                            if (response.resultCode == MessageTypeEnum.Ok)
                            {
                                message = Resources.Language.REQPNDNGSCLAPRVLSCS;

                                //Save Online Payment Transaction details
                                Presenter.SaveTransactionDetails(_invoiceNumber, transactionDetails);

                                String responseCode = transactionDetails["x_response_code"];
                                String responseReasonCode = transactionDetails["x_response_reason_code"];
                                String responseReasonText = transactionDetails["x_response_reason_text"].ToLower();
                                String successResponseText = Resources.Language.TRNSCTNAPRV;

                                if (responseCode == "1" && responseReasonCode == "1" && responseReasonText == successResponseText.ToLower())
                                {
                                    //Approve order and update order status and Bind controls
                                    ApproveOrderAndUpdateStatus(message);
                                }
                            }
                            else
                            {
                                Presenter.UpdateOPDStatus();
                                message = Resources.Language.PYMNTFORORDNUM + " " + CurrentViewContext.OrderId + " " + Resources.Language.FAILEDDUETORSN + ": " + transactionDetails["x_response_reason_text"];
                                base.ShowInfoMessage(message);
                            }

                            return;
                        }

                        //Approve order and update order status and Bind controls
                        ApproveOrderAndUpdateStatus(message);
                    }
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

        /// <summary>
        /// Approve order and update order status and Bind controls
        /// </summary>
        /// <param name="message"></param>
        private void ApproveOrderAndUpdateStatus(String message)
        {
            if (Presenter.ApprovePendingOrders())
            {
                CopyDataAndUpdateStatus(message);
                //UAT-3077
                if (!CurrentViewContext.IsItemPaymentOrder)
                {
                    //UAt-1759
                    Presenter.SendAdditionalDocumentToStudent();
                    if (!CurrentViewContext.IsLocationServiceTenant)
                        Presenter.SendOrderApprovalNotification();

                    //UAt-2970
                    //call SetOrderConfirmationDocForCreditCard web method which will send order confirmartion documnet and message to applicant.
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(),
                        "SetOrderConfirmationMailWithDoc('" + Convert.ToString(CurrentViewContext.SelectedTenantId) + "','" + Convert.ToString(CurrentViewContext.CurrentLoggedInUserId) + "','" + Convert.ToString(CurrentViewContext.OrderId) + "','" + Convert.ToString(CurrentViewContext.OrderPaymentDetailID) + "');", true);
                }
                //UAT-3077
                Presenter.ApprovePaymentItem();
                Presenter.GetOrderDetailsAndSetControls();
                DisplayApproveButtons();
                ShowSuccessMessage(message);


            }
        }

        /// <summary>
        /// Copy data and update status
        /// </summary>
        /// <param name="message"></param>
        private void CopyDataAndUpdateStatus(String message)
        {
            CopyBkgDataToCompliancePackage();

            //Cheack if orderpayment include EDS Service UAT-916
            #region UAT-1476:When a tracking package is ordered and there was already a previous package with entered data,
            //then there would be data movement as if there were a subscription change.
            //UAT_issueFix 06/07/2017 Release 127
            //CopyCompPackageDataForNewOrder();
            #endregion

            if (Presenter.IsOrderPaymentIncludeEDSService())
            {
                //Method to upadte EDS status.
                Presenter.UpdateEDSStatus();

            }
            //UAT-1358:Complio Notification to applicant for PrintScan
            if (Presenter.IsPrintScanServiceExistInOrder())
            {
                Presenter.SendNotificationForPrintScanService();
            }
            //UAT-1560
            Presenter.UpdateAdditionalDocStatus();

            //UAT-2073: commented below methods
            //Presenter.SendOrderApprovalNotification();
            //Presenter.GetOrderDetailsAndSetControls();
            //ShowSuccessMessage(message);
            //DisplayApproveButtons();
            ComplianceDataManager.InsertAutomaticInvitationLog(CurrentViewContext.SelectedTenantId, CurrentViewContext.OrderId, CurrentViewContext.CurrentLoggedInUserId); //UAT-2388

            //UAT-4498
            Presenter.CopyDataForDummyLineItem();

        }

        private void RejectPayment()
        {
            try
            {
                String message = Resources.Language.REQPAYRJCTNSUC;
                if (Presenter.RejectPaymentRequest())
                {
                    ShowSuccessMessage(message);
                    Presenter.GetOrderDetailsAndSetControls();
                    DisplayApproveButtons();
                    //[SG]: UAT-1031 - (Remove System notifications: Payment Rejected and Rule changed.)
                    //Presenter.SendOrderRejectionNotification();
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

        //private void CancelOrderPaymentDetail(Int32? orderPaymentDetailID = null, Boolean? IsCompliancePackageInclude = null)
        //{
        //    try
        //    {
        //        String message = "Request for Cancellation processed successfully.";
        //        CancelOrder(message, orderPaymentDetailID, IsCompliancePackageInclude);
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}
        #endregion

        #region UAT-1476:When a tracking package is ordered and there was already a previous package with entered data,
        //then there would be data movement as if there were a subscription change.

        /// <summary>
        /// Copy Data from Expired compliance Package to newly purchased compliance package 
        /// </summary>
        private void CopyCompPackageDataForNewOrder()
        {
            if (Presenter.IsComplianceAndFreshOrder() && CurrentViewContext.IsCompliancePackageInclude)
            {
                Presenter.CopyCompPackageDataForNewOrder();
            }
        }
        #endregion

        #region UAT-1558
        protected void tgIsGraduatedCompliance_Click(object sender, EventArgs e)
        {
            try
            {
                //UAT-2217:
                //Boolean isGraduated = tgIsGraduatedCompliance.SelectedToggleStateIndex == 1 ? true : false;
                //Boolean isGraduated = rdbIsGraduatedCompliance.SelectedIndex == 1 ? true : false;
                Boolean isGraduated = Convert.ToBoolean(rdbIsGraduatedCompliance.SelectedValue);

                //result will return the status of Updated of IsGraduated Value in DB
                Boolean result = Presenter.UpdateIsGraduatedCompPkg(isGraduated);
                //if result return false toggle button will not change
                if (!result)
                {
                    //UAT-2217
                    //tgIsGraduatedCompliance.SelectedToggleStateIndex = tgIsGraduatedCompliance.SelectedToggleStateIndex == 1 ? 0 : 1;
                    rdbIsGraduatedCompliance.SelectedIndex = rdbIsGraduatedCompliance.SelectedIndex == 1 ? 0 : 1;
                }
                else
                {
                    if (isGraduated)
                        base.ShowSuccessMessage(Resources.Language.TRKNGPKGGRDTSCS);
                    else
                        base.ShowSuccessMessage(Resources.Language.TRKNGPKGUNGRDTSCS);

                    Presenter.SetQueueImaging(); //UAT-2422
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

        protected void tgIsGraduatedBackground_Click(object sender, EventArgs e)
        {
            try
            {
                //Boolean isGraduated = tgIsGraduatedBackground.SelectedToggleStateIndex == 1 ? true : false;
                //UAT-2217
                Boolean isGraduated = Convert.ToBoolean(rdbIsGraduatedBackground.SelectedValue);

                //result will return the status of Updated of IsGraduated Value in DB
                Boolean result = Presenter.UpdateIsGraduatedBkgPkg(isGraduated);
                //if result return false toggle button will not change
                if (!result)
                {
                    //UAT-2217:
                    //tgIsGraduatedCompliance.SelectedToggleStateIndex = tgIsGraduatedCompliance.SelectedToggleStateIndex == 1 ? 0 : 1;
                    rdbIsGraduatedBackground.SelectedIndex = rdbIsGraduatedBackground.SelectedIndex == 1 ? 0 : 1;
                }
                else
                {
                    if (isGraduated)
                        base.ShowSuccessMessage(Resources.Language.SCRNPKGGRDTSCS);
                    else
                        base.ShowSuccessMessage(Resources.Language.SCRNPKGUNGRDTSCS);

                    Presenter.SetQueueImaging(); //UAT-2422
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

        #region UAT-3166
        private void HideShowServiceDetailsLink()
        {
            //List<ServiceLevelDetailsForOrderContract> LstServiceDetails = Presenter.GetServiceLevelDetailsForOrder(CurrentViewContext.SelectedTenantId);
            //if (!IsApplicant &&
            //    !CurrentViewContext.OrderPkgPaymentDetailList.IsNullOrEmpty() &&
            //    !CurrentViewContext.OrderPaymentDetailList.IsNullOrEmpty() &&
            //     CurrentViewContext.OrderPkgPaymentDetailList.Any(cnd => cnd.lkpOrderPackageType.OPT_Code != OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()
            //                                                          && cnd.lkpOrderPackageType.OPT_Code != OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue()
            //                                                          && !cnd.OrderPaymentDetail.IsNullOrEmpty())
            //   )
            if (!IsApplicant)
            {
                List<ServiceLevelDetailsForOrderContract> LstServiceDetails = Presenter.GetServiceLevelDetailsForOrder(CurrentViewContext.SelectedTenantId);
                if (!LstServiceDetails.IsNullOrEmpty())
                {
                    dvServiceDetails.Visible = true;

                    //UAT-3481
                    //Fixed Issue14: ADB Payment Details: When admin navigates across orders from order details screen then service detail pop up window of previous package is displayed. (UAT-3481 || Bug ID: 19598)   
                    hdnTenantID.Value = CurrentViewContext.SelectedTenantId.ToString();
                    hdnOrderID.Value = CurrentViewContext.OrderId.ToString();
                }
                else
                {
                    dvServiceDetails.Visible = false;
                }
            }
            else
            {
                dvServiceDetails.Visible = false;
            }
        }
        #endregion

        #region UAT-3521 || CBI||CABS

        #region UAT-3521 Properties

        Boolean IOrderPaymentDetailsView.IsLocationServiceTenant
        {
            get
            {
                if (!ViewState["IsLocationServiceTenant"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsLocationServiceTenant"]);
                return false;
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
            }
        }

        AppointmentSlotContract IOrderPaymentDetailsView.AppointSlotContract
        {
            get
            {
                if (!ViewState["AppointSlotContract"].IsNullOrEmpty())
                    return (AppointmentSlotContract)(ViewState["AppointSlotContract"]);
                return new AppointmentSlotContract();
            }
            set
            {
                ViewState["AppointSlotContract"] = value;
            }
        }

        Boolean IOrderPaymentDetailsView.IsBkgOrderWithAppointment
        {
            get
            {
                if (!ViewState["IsBkgOrderWithAppointment"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsBkgOrderWithAppointment"]);
                return false;
            }
            set
            {
                ViewState["IsBkgOrderWithAppointment"] = value;
            }
        }

        //Boolean IOrderPaymentDetailsView.IsOutOfStateAppointment
        //{
        //    get
        //    {
        //        if (!ViewState["IsOutOfStateAppointment"].IsNullOrEmpty())
        //            return Convert.ToBoolean(ViewState["IsOutOfStateAppointment"]);
        //        return false;
        //    }
        //    set
        //    {
        //        ViewState["IsOutOfStateAppointment"] = value;
        //    }
        //}

        Int32 IOrderPaymentDetailsView.SelectedSlotID
        {
            get
            {
                if (!ViewState["SelectedSlotID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedSlotID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedSlotID"] = value;
            }
        }

        List<Entity.lkpSuffix> IOrderPaymentDetailsView.lstSuffixes
        {
            get
            {
                if (!ViewState["lstSuffixes"].IsNullOrEmpty())
                    return (List<Entity.lkpSuffix>)ViewState["lstSuffixes"];
                return new List<Entity.lkpSuffix>();
            }
            set
            {
                ViewState["lstSuffixes"] = value;
            }
        }

        #endregion

        #region Events

        Boolean IsFingerPrintRejected()
        {
            if (CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.FINGERPRINT_FILE_ERROR.GetStringValue()
                   || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.FINGERPRINT_FILE_REJECTED.GetStringValue()
                   || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.CBI_FINGERPRINT_FILE_REJECTED.GetStringValue()
                   || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.REJECTED_BY_ABI.GetStringValue())
            {
                return true;
            }

            return false;
        }

        Boolean LocationUpdateAllowed()
        {
            if (IsFingerPrintRejected())
            {
                return true;
            }

            if (!(CurrentViewContext.AppointSlotContract.IsOutOfStateAppointment))
            {
                return true;
            }

            return false;
        }

        protected void lnkChangeAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                #region Previous implementation
                //ucScheduleLocationUpdateControl.ResetPreviousRescheduleData();
                //var changeLocation = LocationUpdateAllowed();
                //var isFingerPrintRejected = IsFingerPrintRejected();
                //if (changeLocation)
                //{
                //    divUCScheduleLocationUpdateControl.Style["display"] = "";
                //    ucScheduleLocationUpdateControl.ResetGrid();
                //    ucScheduleLocationUpdateControl.ShowHideFingerPrintOrderType(!isFingerPrintRejected);
                //}

                ////divUCScheduleLocationUpdateControl.Visible = true;

                //btnSelectAppointment.Visible = changeLocation;
                //btnSelectAppointment.Enabled = false;

                //dvUCAppointmentRescheduler.Visible = !changeLocation;
                //btnSaveAppointment.Visible = !changeLocation;
                //btnSaveAppointment.Enabled = false;

                //dvAppointmentButtons.Visible = true;
                //lnkChangeAppointment.Enabled = false;
                #endregion

                ucScheduleLocationUpdateControl.IsFingerPrintSvcSelected = Convert.ToBoolean(hdfFingerPrint.Value);
                ucScheduleLocationUpdateControl.IsPassportPhotoSvcSelected = Convert.ToBoolean(hdfPassport.Value);
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"TenantId", Convert.ToString( TenantId) },
                                                                    {"OrderId", CurrentViewContext.OrderId.ToString()},
                                                                    {"hdfFingerPrint",hdfFingerPrint.Value.ToString() },
                                                                    {"hdfPassport",hdfPassport.Value.ToString() },
                                                                    {"Parent", ParentControl}
                                                                 };
                String url = String.Format("~/ComplianceOperations/Pages/OrderPaymentDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void btnSaveAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAppointmentInfo(null);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        public void SaveAppointmentInfo(FingerPrintAppointmentContract selectedLocation)
        {
            if (CurrentViewContext.AppointSlotContract.ApplicantAppointmentId > AppConsts.NONE)
            {
                ReserveSlotContract reserveSlotContractRes = null;
                var IsOnsiteAppointment = false;
                if (selectedLocation != null && selectedLocation.IsEventCode)
                {
                    CurrentViewContext.SelectedSlotID = Convert.ToInt32(selectedLocation.SlotID);
                    reserveSlotContractRes = Presenter.ReserveSlot();
                    CurrentViewContext.AppointSlotContract.ReservedSlotID = reserveSlotContractRes.ReservedSlotID;
                    CurrentViewContext.AppointSlotContract.SlotID = Convert.ToInt32(selectedLocation.SlotID);
                    CurrentViewContext.AppointSlotContract.LocationId = selectedLocation.LocationId;
                    //CurrentViewContext.AppointSlotContract.IsOnsiteAppointment = true;
                    IsOnsiteAppointment = true;
                    CurrentViewContext.AppointSlotContract.IsEventType = true;
                    CurrentViewContext.AppointSlotContract.EventName = selectedLocation.EventName;
                    CurrentViewContext.AppointSlotContract.EventDescription = selectedLocation.EventDescription;
                }
                else
                {
                    //CurrentViewContext.AppointSlotContract.IsOnsiteAppointment = false;
                    IsOnsiteAppointment = false;
                    CurrentViewContext.AppointSlotContract.IsEventType = false;
                    CurrentViewContext.SelectedSlotID = ucAppointmentRescheduler.SlotRescheduleContract.SlotID;
                    reserveSlotContractRes = Presenter.ReserveSlot();
                    CurrentViewContext.AppointSlotContract.ReservedSlotID = reserveSlotContractRes.ReservedSlotID;
                    CurrentViewContext.AppointSlotContract.SlotID = ucAppointmentRescheduler.SlotRescheduleContract.SlotID;
                    CurrentViewContext.AppointSlotContract.SlotDate = ucAppointmentRescheduler.SlotRescheduleContract.SlotDate;
                    CurrentViewContext.AppointSlotContract.SlotStartTime = ucAppointmentRescheduler.SlotRescheduleContract.SlotStartTime;
                    CurrentViewContext.AppointSlotContract.SlotEndTime = ucAppointmentRescheduler.SlotRescheduleContract.SlotEndTime;
                    CurrentViewContext.AppointSlotContract.LocationId = ucAppointmentRescheduler.SlotRescheduleContract.LocationId;
                }

                if ((selectedLocation != null && selectedLocation.IsEventCode) ||
                    (!reserveSlotContractRes.IsNullOrEmpty() && reserveSlotContractRes.ReservedSlotID > AppConsts.NONE && reserveSlotContractRes.IsAvailable))
                {
                    //CurrentViewContext.AppointSlotContract.ReservedSlotID = reserveSlotContractRes.ReservedSlotID;
                    //CurrentViewContext.AppointSlotContract.SlotID = ucAppointmentRescheduler.SlotRescheduleContract.SlotID;
                    //CurrentViewContext.AppointSlotContract.SlotDate = ucAppointmentRescheduler.SlotRescheduleContract.SlotDate;
                    //CurrentViewContext.AppointSlotContract.SlotStartTime = ucAppointmentRescheduler.SlotRescheduleContract.SlotStartTime;
                    //CurrentViewContext.AppointSlotContract.SlotEndTime = ucAppointmentRescheduler.SlotRescheduleContract.SlotEndTime;
                    //CurrentViewContext.AppointSlotContract.LocationId = ucAppointmentRescheduler.SlotRescheduleContract.LocationId;
                    //if (Presenter.SaveRescheduledAppointment())

                    //CurrentViewContext.AppointSlotContract.ReservedSlotID = ucAppointmentRescheduler.SlotRescheduleContract.ReservedSlotID;
                    var isLocationUpdateAllowed = LocationUpdateAllowed();
                    var isFingerPrintRejected = IsFingerPrintRejected();
                    ReserveSlotContract reserveSlotContract = Presenter.SubmitApplicantAppointment(isLocationUpdateAllowed, IsOnsiteAppointment, isFingerPrintRejected);
                    if (!reserveSlotContract.IsNullOrEmpty() && reserveSlotContract.ApplicantAppointmentID > AppConsts.NONE)
                    {
                        if (!String.IsNullOrEmpty(reserveSlotContract.ErrorMsg))
                        {
                            base.ShowErrorInfoMessage(reserveSlotContract.ErrorMsg);
                        }
                        else
                        {
                            base.ShowSuccessMessage(Resources.Language.APNMNTRSCHDLSCS);
                            divUCScheduleLocationUpdateControl.Style["display"] = "none";
                            Presenter.SendAppointmentRescheduleNotification(isLocationUpdateAllowed);
                            BindAppointmentData();
                            ShowHideAppointmentInfo();
                            ResetUCAppointmentRescheduler();
                            lnkChangeAppointment.Enabled = true;
                            dvUCAppointmentRescheduler.Visible = false;
                            dvAppointmentButtons.Visible = false;

                        }
                    }
                    else
                    {
                        base.ShowInfoMessage(Resources.Language.SELSLOTNOLNGRSELSANTHR);
                    }
                }
            }
            else
            {
                base.ShowInfoMessage(Resources.Language.SELSLOTNOLNGRSELSANTHR);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                BindAppointmentData();
                lnkChangeAppointment.Enabled = true;
                ResetUCAppointmentRescheduler();
                dvUCAppointmentRescheduler.Visible = false;
                dvAppointmentButtons.Visible = false;
                divUCScheduleLocationUpdateControl.Style["display"] = "none";
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        private void ResetUCAppointmentRescheduler()
        {
            ucAppointmentRescheduler.SelectedSlotDate = (DateTime?)null;
            ucAppointmentRescheduler.BindAppointmentRescheduler();
            var rescheduleCalender = (ucAppointmentRescheduler.FindControl("dpRescheduler") as INTERSOFT.WEB.UI.WebControls.WclCalendar);
            if (!rescheduleCalender.IsNullOrEmpty())
                rescheduleCalender.SelectedDate = new DateTime();
        }

        protected void btnCancelBkgOrder_Click(object sender, EventArgs e)
        {
            try
            {
                String message = Resources.Language.REQCNCLTNSUC;
                //CancelOrder(message, true);
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    //Only for CC Payments Mode
                    var MaxLocScheduleAllowedDays = Presenter.GetLocTenMaxAllowedDays();

                    var creditCardOrderPaymentDetail = CurrentViewContext.OrderPaymentDetailList.FirstOrDefault(cnd => cnd.lkpPaymentOption != null
                                                                                   && cnd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue() && cnd.OPD_Amount > AppConsts.NONE
                                                                                   && cnd.lkpOrderStatu.Code != ApplicantOrderStatus.Modify_Shipping_Send_For_Online_Payment.GetStringValue()
                                                                                   && cnd.lkpOrderStatu.Code != ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue());
                                        
                    //Only appointment rather than Inprogress and Completed are allowed to refund.
                    if (creditCardOrderPaymentDetail.IsNotNull() && CurrentViewContext.AppointSlotContract.FingerPrintDocumentId == AppConsts.NONE
                       && (CurrentViewContext.AppointSlotContract.OrderStatusCode != FingerPrintAppointmentStatus.CANCELLED.GetStringValue()
                       && CurrentViewContext.AppointSlotContract.OrderStatusCode != FingerPrintAppointmentStatus.COMPLETED.GetStringValue())
                       && (creditCardOrderPaymentDetail.IsNotNull() )
                       && ((CurrentViewContext.AppointSlotContract.IsOutOfStateAppointment &&
                       DateTime.Now.Date <= CurrentViewContext.AppointSlotContract.OrderDate.AddDays(Convert.ToInt32(MaxLocScheduleAllowedDays)).Date)
                       || DateTime.Now.Date <= (CurrentViewContext.AppointSlotContract.SlotDate.HasValue ? CurrentViewContext.AppointSlotContract.SlotDate.Value.AddDays(Convert.ToInt32(MaxLocScheduleAllowedDays)).Date : (DateTime?)null)))
                    {
                        if (CurrentViewContext.IsLocationServiceTenant)
                        {
                            List<OrderPaymentDetail> lstCCPaymentDetails = CurrentViewContext.OrderPaymentDetailList.Where(cnd => cnd.lkpPaymentOption != null
                                                                                && cnd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()
                                                                                && cnd.lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue()
                                                                                && cnd.OPD_Amount > AppConsts.NONE).OrderByDescending(x => x.OPD_ApprovalDate).ToList();
                            bool result = false;
                            Decimal TotalrefundAmount = 0;
                            int Count = 0;
                            foreach (OrderPaymentDetail ccOrderPaymentDetail in lstCCPaymentDetails)
                            {
                                Count = Count + 1;
                                Decimal refundAmount = ccOrderPaymentDetail.OPD_Amount ?? 0;
                                TotalrefundAmount = TotalrefundAmount + refundAmount;
                                bool resultValue = RefundCreditCardAmount(refundAmount, ccOrderPaymentDetail);
                                if (Count == 1 && !resultValue)
                                {
                                    result = resultValue;
                                    break;
                                }
                                if (resultValue)
                                    result = resultValue;
                            }
                            if (result)
                            {
                                if (Presenter.CancelBkgOrder(TotalrefundAmount))
                                {

                                    base.ShowSuccessMessage(Resources.Language.PKGCNCLSCSNRFND);
                                    Presenter.GetOrderDetailsAndSetControls();
                                    DisplayApproveButtons();
                                    dvCancelOrder.Visible = false;
                                    dvRescheduleAppoinment.Visible = false;
                                }
                                else
                                {
                                    base.ShowErrorInfoMessage(Resources.Language.CRDTRDAMNTRFNDERR);
                                }
                            }
                        }
                        else
                        {
                            if (RefundCreditCardAmount(CurrentViewContext.OnlinePaymentAmount, creditCardOrderPaymentDetail))
                            {
                                if (Presenter.CancelBkgOrder(CurrentViewContext.OnlinePaymentAmount))
                                {

                                    base.ShowSuccessMessage(Resources.Language.PKGCNCLSCSNRFND);
                                    Presenter.GetOrderDetailsAndSetControls();
                                    DisplayApproveButtons();
                                    dvCancelOrder.Visible = false;
                                    // lnkChangeAppointment.Enabled = false;
                                    dvRescheduleAppoinment.Visible = false;
                                    // dvAppointmentInfo.Visible = false;
                                }
                                else
                                {
                                    base.ShowErrorInfoMessage(Resources.Language.CRDTRDAMNTRFNDERR);
                                }
                            }


                        }
                    }

                    // Non-CC Payments Mode
                    else
                    {

                        if (Presenter.CancelBkgOrder())
                        {

                            base.ShowSuccessMessage(Resources.Language.PKGCNCLSCS);
                            Presenter.GetOrderDetailsAndSetControls();
                            DisplayApproveButtons();
                            dvCancelOrder.Visible = false;
                            //  lnkChangeAppointment.Enabled = false;
                            dvRescheduleAppoinment.Visible = false;
                            //   dvAppointmentInfo.Visible = false;

                        }
                        else
                        {
                            base.ShowErrorInfoMessage(Resources.Language.SOMEERROCCUR);

                        }


                    }

                }
                else
                {

                    if (Presenter.CancelBkgOrder())
                    {
                        ShowSuccessMessage(message);
                        // Presenter.GetOrderDetailsAndSetControls();
                        //DisplayApproveButtons();
                        //ShowBackgroundPackages();
                        //ShowPartialOrderCancellationPackages();

                        //  Presenter.SendOrderCancellationApprovalNotification();
                        dvCancelOrder.Visible = false;
                        //  lnkChangeAppointment.Enabled = false;
                        dvRescheduleAppoinment.Visible = false;
                        //  dvAppointmentInfo.Visible = false;
                    }
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Methods

        private void ShowHideAppointmentInfo()
        {
            Presenter.IsBkgOrderWithAppointment();



            if (CurrentViewContext.IsLocationServiceTenant
                && CurrentViewContext.IsBkgOrderWithAppointment)
            {
                BindAppointmentData();
                if (!CurrentViewContext.AppointSlotContract.IsNullOrEmpty()
                && !CurrentViewContext.AppointSlotContract.IsOutOfStateAppointment)
                {
                    dvAppointmentInfo.Visible = true;
                    dvCancelOrder.Visible = CurrentViewContext.IsFileSentToCBI ? false : true;


                    var HideCancel = FingerPrintDataManager.HideCancel(CurrentViewContext.AppointSlotContract.OrderId, CurrentViewContext.TenantId);

                    if (HideCancel && dvCancelOrder.Visible)
                    {
                        dvCancelOrder.Visible = false;
                    }




                    dvSaveIsGraduatedBackground.Visible = false;

                    if ((CurrentViewContext.OrderPaymentDetailList.IsNotNull()
                        && CurrentViewContext.OrderPaymentDetailList.All(cond => cond.lkpOrderStatu != null && (cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue())))
                       )
                    {
                        dvRescheduleAppoinment.Visible = false;
                        dvCancelOrder.Visible = false;
                    }

                    var MaxLocScheduleAllowedDays = Presenter.GetLocTenMaxAllowedDays();
                    // if (!CurrentViewContext.AppointSlotContract.SlotDate.IsNullOrEmpty())
                    // {
                    //     if (!(CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.FINGERPRINT_FILE_REJECTED.GetStringValue()
                    //         || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.FINGERPRINT_FILE_ERROR.GetStringValue()
                    //         || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.CBI_FINGERPRINT_FILE_REJECTED.GetStringValue()
                    //         || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.REJECTED_BY_ABI.GetStringValue())
                    //         && (CurrentViewContext.AppointSlotContract.SlotDate.Value.AddDays(Convert.ToInt32(MaxLocScheduleAllowedDays)).Date < DateTime.Now.Date
                    //         || CurrentViewContext.AppointSlotContract.OrderStatusCode == OrderStatusType.INPROGRESS.GetStringValue()))
                    //     {
                    //         dvRescheduleAppoinment.Visible = false;
                    //     }
                    // }
                    //if (CurrentViewContext.AppointSlotContract.OrderStatusCode == OrderStatusType.INPROGRESS.GetStringValue() || Presenter.ServicesInProgress(CurrentViewContext.AppointSlotContract))
                    //{
                    //  dvRescheduleAppoinment.Visible = false;
                    //}

                    var hideReschedlue = FingerPrintDataManager.HideReschedule(CurrentViewContext.AppointSlotContract, CurrentViewContext.TenantId);

                    if ((CurrentViewContext.AppointSlotContract.SlotDate != null && CurrentViewContext.AppointSlotContract.SlotDate.Value.AddDays(Convert.ToInt32(MaxLocScheduleAllowedDays)).Date < DateTime.Now.Date) || hideReschedlue == true)
                    {
                        dvRescheduleAppoinment.Visible = false;
                    }
                    else if (CheckIfSlotDateCrossedMaxScheduleAllowedDays(MaxLocScheduleAllowedDays)) //UAT-4906
                    {
                        dvRescheduleAppoinment.Visible = false;
                    }

                    if (CurrentViewContext.AppointSlotContract.IsOnsiteAppointment == true)
                    {

                        //  dvRescheduleAppoinment.Visible = false;
                        btnViewLocImage.Visible = false;
                        dvLocAdd.Visible = false;

                    }
                }
                else
                {
                    if (!CurrentViewContext.AppointSlotContract.IsNullOrEmpty()
                        && CurrentViewContext.AppointSlotContract.IsOutOfStateAppointment)
                    {
                        dvOutOfStateAppointmentDetails.Visible = true;
                        dvAppointmentInfoData.Visible = false;
                        dvAppointmentInfo.Visible = true;
                        if ((CurrentViewContext.OrderPaymentDetailList.IsNotNull()
                        && (CurrentViewContext.IsFileSentToCBI || CurrentViewContext.OrderPaymentDetailList.All(cond => cond.lkpOrderStatu != null && (cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue())))))
                            dvCancelOrder.Visible = false;
                        else
                            dvCancelOrder.Visible = true;

                    }
                }
            }
            else
            {
                //if (!CurrentViewContext.AppointSlotContract.IsNullOrEmpty()
                //&& CurrentViewContext.AppointSlotContract.IsOutOfStateAppointment)
                //{
                //    dvOutOfStateAppointmentDetails.Visible = true;
                //    dvAppointmentInfoData.Visible = false;
                //    dvAppointmentInfo.Visible = true;
                //    if ((CurrentViewContext.OrderPaymentDetailList.IsNotNull()
                //    && CurrentViewContext.OrderPaymentDetailList.All(cond => cond.lkpOrderStatu != null && (cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue()))))
                //        dvCancelOrder.Visible = false;
                //    else
                //        dvCancelOrder.Visible = true;

                //}
                //else
                //{
                dvAppointmentInfo.Visible = false;
                dvCancelOrder.Visible = false;
                //}
                //dvSaveIsGraduatedBackground.Visible = CurrentViewContext.IsLocationServiceTenant ? false : true;


            }

            if (CurrentViewContext.IsLocationServiceTenant)
            {
                dvSaveIsGraduatedBackground.Visible = false;

                //UAT-4360
                Presenter.GetFingerPrintOrderKeydata();
                if (CurrentViewContext.lstFingerPrintData.IsNotNull())
                {
                    dvCBIUniqueID.Visible = true;
                    txtCBIUniqueID.Text = CurrentViewContext.lstFingerPrintData.CBIUniqueID;
                }
            }
        }

        //UAT-4906
        private bool CheckIfSlotDateCrossedMaxScheduleAllowedDays(string MaxLocScheduleAllowedDays)
        {
            return (CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.FINGERPRINT_FILE_REJECTED.GetStringValue()
                 || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.CBI_FINGERPRINT_FILE_REJECTED.GetStringValue()
                 || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.REJECTED_BY_ABI.GetStringValue())
                 && (Math.Round((DateTime.Now.Date - CurrentViewContext.AppointSlotContract.SlotDate.Value).TotalDays, 0, MidpointRounding.AwayFromZero) > Convert.ToInt32(MaxLocScheduleAllowedDays));
        }
        //UAT-4906

        private void BindAppointmentData()
        {
            Presenter.GetBkgOrderWithAppointmentData();
            if (!CurrentViewContext.AppointSlotContract.IsNullOrEmpty()
                && !CurrentViewContext.AppointSlotContract.IsOutOfStateAppointment)
            {
                TimeSpan startTime = CurrentViewContext.AppointSlotContract.SlotStartTimeTimeSpanFormat;
                TimeSpan endTime = CurrentViewContext.AppointSlotContract.SlotEndTimeTimeSpanFormat;
                if (CurrentViewContext.AppointSlotContract.SlotDate.IsNotNull())
                {
                    DateTime slotStartDateTime = CurrentViewContext.AppointSlotContract.SlotDate.Value.Add(startTime);
                    DateTime slotEndDateTime = CurrentViewContext.AppointSlotContract.SlotDate.Value.Add(endTime);
                    lblAppointmentDateTime.Text = slotStartDateTime.ToString("MM/dd/yyyy") + " (" + slotStartDateTime.ToString("hh:mm tt") + " - " + slotEndDateTime.ToString("hh:mm tt") + ") ";
                }

                txtLocationName.Text = String.IsNullOrEmpty(CurrentViewContext.AppointSlotContract.LocationName) ? String.Empty : CurrentViewContext.AppointSlotContract.LocationName;
                txtLocationAddress.Text = String.IsNullOrEmpty(CurrentViewContext.AppointSlotContract.LocationAddress) ? String.Empty : CurrentViewContext.AppointSlotContract.LocationAddress;
                lblSiteDescription.Text = (String.IsNullOrEmpty(CurrentViewContext.AppointSlotContract.LocDescription) ? String.Empty : CurrentViewContext.AppointSlotContract.LocDescription);
                //txtAppointmentStatus.Text= String.IsNullOrEmpty(CurrentViewContext.AppointSlotContract.AppointmentStatus) ? String.Empty : CurrentViewContext.AppointSlotContract.AppointmentStatus;
                //Bug-93

                //lnkChangeAppointment.Text = slotStartDateTime.ToString("dd/MM/yyyy HH:mm") + " - " + slotEndDateTime.ToString("dd/MM/yyyy HH:mm");

                //ucAppointmentRescheduler.SlotId = CurrentViewContext.AppointSlotContract.SlotID;
                ucAppointmentRescheduler.LocationId = CurrentViewContext.AppointSlotContract.LocationId;
                //ucAppointmentRescheduler.SelectedSlotDate = CurrentViewContext.AppointSlotContract.SlotDate;
                //ucAppointmentRescheduler.SelectedSlotStartTime = Convert.ToString(CurrentViewContext.AppointSlotContract.SlotStartTimeTimeSpanFormat);
                ucAppointmentRescheduler.IsOrderPaymentDetailScreen = true;
                ucAppointmentRescheduler.IsCreateOrderScreen = false;
                hdnLocId.Value = Convert.ToString(CurrentViewContext.AppointSlotContract.LocationId);
            }
        }

        public void EnableNextButton(bool status)
        {
            btnSelectAppointment.Enabled = status;
        }
        public void EnableSaveButton()
        {
            btnSaveAppointment.Visible = true;
            btnSaveAppointment.Enabled = true;
        }

        private void AddSuffix()
        {
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                Presenter.GetSuffixes();
            }
        }

        #endregion

        #endregion
        #region UAT-3601 || CBI || CABS
        private void LocationBaseSettings()
        {
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                lblOrderQueue.Text = Resources.Language.ORDSELDTLS;
                hrPackageDetailsLocationTenant.Visible = true;
                hrPackageDetails.Visible = false;
                hrAPILocationTenant.Visible = true;
                hrAPI.Visible = false;
                hrAILocationTenant.Visible = true;
                hrAI.Visible = false;
                hdnIsLocationServiceTenant.Value = CurrentViewContext.IsLocationServiceTenant.ToString().ToLower();

            }
            else
            {
                hrPackageDetailsLocationTenant.Visible = false;
                hrPackageDetails.Visible = true;
                hrAPILocationTenant.Visible = false;
                hrAPI.Visible = true;
                hrAILocationTenant.Visible = false;
                hrAI.Visible = true;
            }
            #endregion
        }
        protected void grdServiceDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                divBackgroundPackage.Visible = false;
                dvServiceTypeDetails.Visible = true;
                CurrentViewContext.orderDetailContracts = Presenter.OrderSerivceDetail(OrderId);
                var technicalReview = FingerPrintSetUpManager.GetAllAppointmentStatus().ToList().Where(t => t.AS_Code == FingerPrintAppointmentStatus.TECHNICAL_REVIEW.GetStringValue()).FirstOrDefault().AS_Name;
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    foreach (var item in CurrentViewContext.orderDetailContracts)
                    {
                        if (item.ServiceStatus != null && item.ServiceStatus == technicalReview)
                        {
                            item.ServiceStatus = FingerPrintSetUpManager.GetAllAppointmentStatus().ToList().Where(t => t.AS_Code == FingerPrintAppointmentStatus.SUBMITTED_TO_CBI.GetStringValue()).FirstOrDefault().AS_Name; ;
                        }
                    }
                }
                //In case of complete your order , there will will be no data in DB , so pick that data from stored XML
                if (orderDetailContracts.Count <= 1)
                {
                    if ((orderDetailContracts == null || orderDetailContracts.Count == 0 || (orderDetailContracts.Count > 0 && orderDetailContracts[0].ServiceCode != "AAAA")))
                    { 
                        GetDataFromStoredXML();
                    }
                }
                grdServiceDetails.DataSource = orderDetailContracts;

                if (orderDetailContracts.Count == 1)
                {
                    foreach (OrderDetailContract _item in orderDetailContracts)
                    {
                        if (_item.ServiceCode == BkgServiceType.SIMPLE.GetStringValue())
                        {
                            dvServiceTypeDetails.Visible = false;
                            grdServiceDetails.Visible = false;
                            divBackgroundPackage.Visible = true;
                        }
                    }
                }
                else if (orderDetailContracts.Count > 1)
                {
                    divBackgroundPackage.Visible = false;
                }

            }
            else
            {
                dvServiceTypeDetails.Visible = false;
            }
        }
        protected void GetDataFromStoredXML()
        {

            XmlDocument xmlDoc = new XmlDocument();

            List<OrderDetailContract> orderDetailContract = new List<OrderDetailContract>();
            string BkgOrderServiceDetailsxml = Presenter.GetBkgOrderServiceDetails(OrderId);
            if (!BkgOrderServiceDetailsxml.IsNullOrEmpty())
            {
                xmlDoc.LoadXml(BkgOrderServiceDetailsxml);
                XmlNodeList elemlist = xmlDoc.GetElementsByTagName("BkgpkgData");

                foreach (XmlNode node in elemlist)
                {
                    OrderDetailContract orderLineItem = new OrderDetailContract();
                    if (node.HasChildNodes)
                    {

                        for (var i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == "ServiceType")
                            {
                                orderLineItem.ServiceName = Presenter.GetPackageNameForCompleteOrder(OrderId, node.ChildNodes[i].InnerText, false);
                                orderLineItem.ServiceCode = node.ChildNodes[i].InnerText;
                            }
                            if (node.ChildNodes[i].Name == "BasePrice")
                            {
                                orderLineItem.Price = Decimal.Parse(node.ChildNodes[i].InnerText);
                            }
                            if (node.ChildNodes[i].Name == "TotalBkgPackagePrice")
                            {
                                orderLineItem.Amount = Decimal.Parse(node.ChildNodes[i].InnerText);
                            }
                            if (node.ChildNodes[i].Name == "NumberOfCopies")
                            {
                                if (Int32.Parse(node.ChildNodes[i].InnerText) == 0)
                                {
                                    orderLineItem.Quantity = null;
                                }
                                else
                                {
                                    orderLineItem.Quantity = Int32.Parse(node.ChildNodes[i].InnerText);
                                }
                            }
                            if (node.ChildNodes[i].Name == "FCAdditionalPrice")
                            {
                                orderLineItem.FCAdditionalPrice = Decimal.Parse(node.ChildNodes[i].InnerText);
                            }
                            if (node.ChildNodes[i].Name == "PPCopiesCount")
                            {
                                if (Int32.Parse(node.ChildNodes[i].InnerText) == 0)
                                {
                                    orderLineItem.PPQuantity = null;
                                }
                                else
                                {
                                    orderLineItem.PPQuantity = Int32.Parse(node.ChildNodes[i].InnerText);
                                }
                            }
                            if (node.ChildNodes[i].Name == "PPAdditionalPrice")
                            {
                                if (Decimal.Parse(node.ChildNodes[i].InnerText) == 0)
                                {
                                    orderLineItem.PPAdditionalPrice = null;
                                }
                                else
                                {
                                    orderLineItem.PPAdditionalPrice = Decimal.Parse(node.ChildNodes[i].InnerText);
                                }
                            }
                        }
                        if (orderLineItem.Quantity == null)
                        {
                            orderLineItem.Price = orderLineItem.Amount;
                        }
                        if (orderLineItem.Quantity != null && orderLineItem.Quantity >= 1)
                        {
                            if (orderLineItem.PPQuantity != null && orderLineItem.PPQuantity >= 1)
                            {
                                orderLineItem.Amount = ((orderLineItem.Quantity - 1) * orderLineItem.FCAdditionalPrice) + ((orderLineItem.PPQuantity - 1) * orderLineItem.PPAdditionalPrice) + orderLineItem.Price;
                            }
                            else
                            {
                                orderLineItem.Amount = ((orderLineItem.Quantity - 1) * orderLineItem.FCAdditionalPrice) + orderLineItem.Price;
                            }
                        }
                        orderDetailContract.Add(orderLineItem);
                        OrderDetailContract shippingLineItem = new OrderDetailContract();
                        for (var j = 0; j < node.ChildNodes.Count; j++)
                        {
                            if (node.ChildNodes[j].Name == "MailingOptionId")
                            {
                                shippingLineItem.ServiceName = "Shipping Fee (" + Presenter.GetShippingLineItemName(node.ChildNodes[j].InnerText) + ")";
                            }
                            if (node.ChildNodes[j].Name == "MailingOptionPrice")
                            {
                                shippingLineItem.Price = null;
                                shippingLineItem.Amount = Decimal.Parse(node.ChildNodes[j].InnerText);
                                shippingLineItem.Quantity = null;
                            }
                        }
                        if ((shippingLineItem.ServiceName != null))
                            orderDetailContract.Add(shippingLineItem);
                    }

                }


            }



            CurrentViewContext.orderDetailContracts = orderDetailContract;



        }
        private void GetShiippingAddress()
        {
            PreviousAddressContract mailingAddress = new PreviousAddressContract();
            XmlDocument xmlDoc = new XmlDocument();


            string BkgOrderServiceDetailsxml = Presenter.GetBkgOrderServiceDetails(CurrentViewContext.OrderId);
            if (!BkgOrderServiceDetailsxml.IsNullOrEmpty())
            {
                xmlDoc.LoadXml(BkgOrderServiceDetailsxml);
                XmlNodeList elemlist = xmlDoc.GetElementsByTagName("BkgpkgData");
                var MailingPrice = "";
                var MailingOptionName = "";
                var MailingOptionId = "";

                foreach (XmlNode node in elemlist)
                {
                    if (node.HasChildNodes)
                    {

                        for (var i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == "MailingAddressHandleId")
                            {


                                mailingAddress = Presenter.GetAddressThroughAddressHandleID(node.ChildNodes[i].InnerText);

                            }
                            if (node.ChildNodes[i].Name == "MailingOptionPrice")
                            {
                                MailingPrice = node.ChildNodes[i].InnerText;
                            }
                            if (node.ChildNodes[i].Name == "MailingOptionId")
                            {
                                MailingOptionId = node.ChildNodes[i].InnerText;
                                MailingOptionName = Presenter.GetShippingLineItemName(node.ChildNodes[i].InnerText);

                            }

                        }
                        if (mailingAddress.CountryId != 0)
                        {

                            mailingAddress.CountyName = Presenter.GetCountryByCountryId(mailingAddress.CountryId);
                        }
                        if (MailingPrice != "" && MailingOptionName != "")
                        {
                            mailingAddress.MailingOptionPrice = MailingOptionName + "(" + MailingPrice + ")";
                            mailingAddress.MailingOption = MailingOptionName;
                            mailingAddress.MailingOptionId = MailingOptionId;
                        }
                    }
                }
            }
            if (mailingAddress.MailingAddressHandleId != null)
                CurrentViewContext.MailingAddressData = mailingAddress;
        }
        protected void btnViewLocImage_Click(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "openImageSliderPopup();", true);
        }

        #region UAT-3734
        private Boolean RefundCreditCardAmount(decimal refundAmount, OrderPaymentDetail creditCardOrderPaymentDetail) // new param added creditCardOrderPaymentDetail
        {
            var result = false;
            OnlinePaymentTransaction creditCardOrderOnlineTransaction = new OnlinePaymentTransaction();

            if (CurrentViewContext.IsLocationServiceTenant)
            {
                creditCardOrderOnlineTransaction = creditCardOrderPaymentDetail.OnlinePaymentTransaction;
            }
            else
            {
                creditCardOrderOnlineTransaction = CurrentViewContext.OnlinePaymentTransaction;
            }


            if (creditCardOrderOnlineTransaction.IsNotNull()
                && !string.IsNullOrWhiteSpace(creditCardOrderOnlineTransaction.Trans_id)
                && !string.IsNullOrWhiteSpace(creditCardOrderOnlineTransaction.CCNumber)
                && !string.IsNullOrWhiteSpace(creditCardOrderOnlineTransaction.Invoice_num))
            {

                if (!CurrentViewContext.IsLocationServiceTenant)
                {
                    creditCardOrderPaymentDetail = getCreditCardOrderPaymentDetail();
                }

                var userId = CurrentViewContext.UserId;
                Entity.AuthNetCustomerProfile customerProfile = Presenter.GetCustomerProfile(UserId);
                var description = Presenter.GenerateDescription();

                INTSOF.AuthNet.Business.CustomerProfileWS.CreateCustomerProfileTransactionResponseType _response = INTSOF.AuthNet.Business.AuthorizeNetCreditCard.ProcessRefund
                                                                                                                                (creditCardOrderOnlineTransaction.Trans_id,
                                                                                                                                Convert.ToInt64(customerProfile.CustomerProfileID),
                                                                                                                                refundAmount,
                                                                                                                                 CurrentViewContext.OrganizationUserId,
                                                                                                                                creditCardOrderOnlineTransaction.CCNumber,
                                                                                                                                description,
                                                                                                                                creditCardOrderOnlineTransaction.Invoice_num, CurrentViewContext.SelectedTenantId);
                if (_response.resultCode == INTSOF.AuthNet.Business.CustomerProfileWS.MessageTypeEnum.Ok &&
                                 !_response.directResponse.IsNullOrEmpty())
                {
                    string[] arrRespParts = _response.directResponse.Split('|');

                    //hdnPartialOrderCancellationAmount.Value = Convert.ToString(Convert.ToDecimal(hdnPartialOrderCancellationAmount.Value)
                    //                                            - Convert.ToDecimal(txtRefundAmount.Text));

                    SaveRefundHistory(_response.directResponse, true, refundAmount, creditCardOrderPaymentDetail.OPD_ID, arrRespParts);
                    //base.ShowSuccessMessage(arrRespParts[3]);

                    result = true;
                }
                else if (_response.resultCode == INTSOF.AuthNet.Business.CustomerProfileWS.MessageTypeEnum.Error && !_response.directResponse.IsNullOrEmpty())
                {
                    string[] arrRespParts = _response.directResponse.Split('|');
                    SaveRefundHistory(_response.directResponse, false, refundAmount, creditCardOrderPaymentDetail.OPD_ID, arrRespParts);
                    if (CurrentViewContext.IsLocationServiceTenant && arrRespParts[3] == "The referenced transaction does not meet the criteria for issuing a credit.")
                    {
                        base.ShowInfoMessage(Resources.Language.TRRNSCTNCNCLHOURS);
                    }
                    else
                    {
                        base.ShowInfoMessage(arrRespParts[3]);
                    }

                }
                else
                {
                    System.Text.StringBuilder _sbInfoMessage = new System.Text.StringBuilder();

                    for (int i = 0; i < _response.messages.Length; i++)
                    {
                        _sbInfoMessage.Append(_response.messages[i].text);  // To Get Message n for loop to check the [i] is not empty 
                    }
                    SaveRefundHistory(Convert.ToString(_sbInfoMessage), false, refundAmount, creditCardOrderPaymentDetail.OPD_ID, null);

                    base.ShowInfoMessage(Convert.ToString(_sbInfoMessage));
                }
            }
            else
            {
                base.ShowErrorInfoMessage(Resources.Language.NSFCNTDATATORFND);
            }
            return result;
        }


        private void SaveRefundHistory(String message, Boolean isSuccess, decimal refundAmount, int orderPaymentDetailId, String[] arrRespParts)
        {
            Presenter.AddRefundHistory(
                new RefundHistory
                {
                    RH_OrderID = CurrentViewContext.OrderId,
                    RH_Amount = refundAmount,
                    RH_CreatedByID = CurrentViewContext.CurrentLoggedInUserId,
                    RH_CreatedOn = DateTime.Now,
                    RH_TransID = arrRespParts == null ? null : arrRespParts[6],
                    RH_DirectResponse = message,
                    RH_IsSuccess = isSuccess,
                    RH_OrderPaymentDetailID = orderPaymentDetailId
                });
        }

        private OrderPaymentDetail getCreditCardOrderPaymentDetail()
        {

            return CurrentViewContext.OrderPaymentDetailList.FirstOrDefault(cnd => cnd.lkpPaymentOption != null
                                                                                  && cnd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue() && cnd.OPD_Amount > AppConsts.NONE);



        }
        #endregion
        public void SelectAppointmentClick()
        {

            var selectedLocation = ucScheduleLocationUpdateControl.GetSelectedLocation();
            if (selectedLocation.IsNullOrEmpty() || selectedLocation.LocationId <= 0)
            {
                ShowErrorInfoMessage(Resources.Language.PLZSELLOC);
                return;
            }
            if (selectedLocation.IsEventCode)
            {
                SaveAppointmentInfo(selectedLocation);
            }
            else
            {
                divUCScheduleLocationUpdateControl.Style["display"] = "none";
                dvUCAppointmentRescheduler.Visible = true;
                ucAppointmentRescheduler.LocationId = selectedLocation.LocationId;
                btnSelectAppointment.Visible = false;
                btnSaveAppointment.Visible = true;
                //ucAppointmentRescheduler.SelectedSlotDate = CurrentViewContext.AppointSlotContract.SlotDate;
                //ucAppointmentRescheduler.SelectedSlotStartTime = Convert.ToString(CurrentViewContext.AppointSlotContract.SlotStartTimeTimeSpanFormat);
                ucAppointmentRescheduler.IsOrderPaymentDetailScreen = true;
                ucAppointmentRescheduler.IsCreateOrderScreen = false;
                ucAppointmentRescheduler.BindAppointmentRescheduler();
                hdnLocId.Value = Convert.ToString(selectedLocation.LocationId);
            }

        }
        protected void btnSelectAppointment_Click(object sender, EventArgs e)
        {
            //divUCScheduleLocationUpdateControl.Visible = false;            
            SelectAppointmentClick();
        }

        protected void btnApprovePaymentHide_Click(object sender, EventArgs e)
        {
            ApprovePayment(CurrentViewContext.OrderPaymentDetailStatusCode);
        }

        protected void lnkbacksrch_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantId) },
                                                                    { "Child", WorkQueuePath},
                                                                    {"ReadSession",true.ToString()}
                                                                 };

                String url = string.Empty;
                url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
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
    }
}


