#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.Services;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.UI;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.FingerPrintSetup;
using Business.RepoManagers;



#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class OrderHistory : BaseUserControl, IOrderHistoryView

    {
        #region Variables

        #region Private Variables

        private List<Int32> _lstPossibleRushOrders;
        private OrderHistoryPresenter _presenter = new OrderHistoryPresenter();
        private Int32 _currentUserTenantId;
        private String _viewType;
        private Boolean _isFalsePostBack = false;
        private Boolean _isScreeningOnly = false;
        private String _pageType;
        private Boolean _visiblity;
        protected String ImagePath = "~/images/small";
        private Guid LCSAttCode = new Guid("1ADA97AE-9100-4BE6-B829-C914B7FA8750");//Driver's License State
        private Guid LCNAttCode = new Guid("515BEF57-9072-4D2A-A97A-0C248BB045F9");////License Number
        private Guid MotherNameAttrCode = new Guid("3DA8912A-6337-4B8F-93C4-88BFC3032D2D");////Mother's Maiden Name
        private Guid IdentificationNumberAttrCode = new Guid("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211");////Identification Number
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties

        // UAT-1683 Add the Archive button and Manage Un-Archive to the Screening side
        Boolean IOrderHistoryView.IsArchivedSubscription { get; set; }

        #endregion

        #region Public Properties

        public OrderHistoryPresenter Presenter
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

        public Dictionary<int, bool> EDSExistStatus
        {
            get;
            set;
        }

        public List<Tuple<Int32, Boolean>> LstReciptDocumentStatus
        {
            get;
            set;
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 CurrentUserTenantId
        {
            //get
            //{
            //    if (_currentUserTenantId == 0)
            //    {
            //        _currentUserTenantId = Convert.ToInt32(ViewState["TenantID"]);
            //    }
            //    return _currentUserTenantId;

            //}
            //set
            //{
            //    ViewState["TenantID"] = value;
            //}

            get
            {
                if (_currentUserTenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _currentUserTenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _currentUserTenantId;
            }
            set { _currentUserTenantId = value; }
        }
        public Boolean IsLocationServiceTenant
        {
            get
            {
                if (ViewState["IsLocationServiceTenant"] != null)
                    return Convert.ToBoolean(ViewState["IsLocationServiceTenant"]);
                return false;
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
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

        public Boolean IsBkgOrderWithAppointment
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
        public List<vwOrderDetail> ListOrderDetail
        {
            get;
            set;
        }

        /// <summary>
        /// Get Set Property to show hide link for place rush order on the basis of client setting.
        /// </summary>
        public Boolean ShowRushOrder
        {
            get
            {
                return (Boolean)(ViewState["ShowRushOrder"] ?? "0");
            }
            set
            {
                ViewState["ShowRushOrder"] = value;
            }
        }

        public Boolean Visiblity
        {
            get
            {
                return _visiblity;
            }
            set
            {
                _visiblity = value;
            }
        }

        public String ControlUseType
        {
            get;
            set;
        }

        public Boolean IsFalsePostBack
        {
            get
            {
                return _isFalsePostBack;
            }
            set
            {
                _isFalsePostBack = value;
            }
        }

        public Boolean IsScreeningOnly
        {
            get
            {
                return _isScreeningOnly;
            }
            set
            {
                _isScreeningOnly = value;
            }
        }

        public Boolean HavingPackageOtherthanScreening
        {
            get;
            set;
        }

        public String PageType
        {
            get
            {
                return _pageType;
            }
            set
            {
                _pageType = value;
            }
        }

        public List<ServiceFormContract> lstServiceForm
        {
            get;
            set;
        }

        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        public Int32 OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return CurrentUserId;
            }
        }
        #region UAT-1648
        public List<BkgOrderDetailCustomFormUserData> lstDataForCustomForm { get; set; }
        public List<Int32> BopIds { get; set; }
        #endregion

        public List<AttributeFieldsOfSelectedPackages> lstAttrMVRGrp
        {
            get
            {
                if (ViewState["lstMvrAttGrp"] != null)
                    return (List<AttributeFieldsOfSelectedPackages>)ViewState["lstMvrAttGrp"];
                return null;
            }
            set
            {
                ViewState["lstMvrAttGrp"] = value;
            }
        }

        public List<AttributeFieldsOfSelectedPackages> LstInternationCriminalSrchAttributes
        {
            get
            {
                if (ViewState["LstInternationCriminalSrchAttributes"] != null)
                    return (List<AttributeFieldsOfSelectedPackages>)ViewState["LstInternationCriminalSrchAttributes"];
                return null;
            }
            set
            {
                ViewState["LstInternationCriminalSrchAttributes"] = value;
            }
        }

        public String MenuId
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                //change done for applicant dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    base.OnInit(e);
                    if (ControlUseType != AppConsts.DASHBOARD)
                    {
                        base.Title = "Order History";
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



        /// <summary>
        /// Loads the page ManageAssignmentProperties.aspx.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //change done for applicant dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                    Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                    Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
                    if (!this.IsPostBack)
                    {
                        //CurrentUserTenantId = Presenter.GetTenantId();
                        Presenter.OnViewInitialized();
                        Presenter.IsLocationServiceTenant();//UAT 3573
                        if (ControlUseType != AppConsts.DASHBOARD)
                        {
                            base.SetPageTitle("Order History");
                        }
                    }
                    if (IsFalsePostBack || (this.hdnPostbacksource.Text != "OH" && this.hdnPostbacksource.Text != "OSH" && this.ControlUseType == AppConsts.DASHBOARD))
                    {
                        Presenter.IsLocationServiceTenant();
                        grdOrderHistory.Rebind();
                    }
                    hfTenantId.Value = CurrentUserTenantId.ToString();
                    HandleGridColumns();
                    if (!IsLocationServiceTenant && HavingPackageOtherthanScreening)
                    {
                        btnGotoHome.Visible = true;
                    }
                }
                //hfCurrentUserID.Value = CurrentUserId.ToString();
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

        #region Grid Events

        protected void grdOrderHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                //change done for applicant dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    Presenter.GetOrderDetailList();
                    Presenter.ShowRushOrder();

                    _lstPossibleRushOrders = Presenter.GerPossibleRushOrderIds(ListOrderDetail);

                    //UAT 3723
                    var SubmittedToCBI = FingerPrintSetUpManager.GetAllAppointmentStatus().ToList().Where(t => t.AS_Code == FingerPrintAppointmentStatus.SUBMITTED_TO_CBI.GetStringValue()).FirstOrDefault().AS_Name;
                    if (IsLocationServiceTenant)
                    {
                        foreach (var item in ListOrderDetail)
                        {
                            if (item.AppointmentStatusCode != null && item.AppointmentStatusCode != string.Empty)
                            {
                                if (item.AppointmentStatusCode != FingerPrintAppointmentStatus.ACTIVE.GetStringValue() && item.AppointmentStatusCode != FingerPrintAppointmentStatus.COMPLETED.GetStringValue())
                                {
                                    item.BkgOrderStatus = item.AppointmentStatus;
                                    if (item.AppointmentStatusCode == FingerPrintAppointmentStatus.TECHNICAL_REVIEW.GetStringValue())
                                    {
                                        item.BkgOrderStatus = SubmittedToCBI;
                                    }
                                    item.BkgOrderStatusID = item.AppointmentStatusID;
                                    item.BkgOrderStatusCode = item.AppointmentStatusCode;
                                }
                            }
                        }

                    }
                    grdOrderHistory.DataSource = ListOrderDetail;
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

        protected void grdOrderHistory_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                Boolean isPackageChangeSubscription = false;
                Boolean isPackageRenewSubscription = false;
                Boolean isPackageRepurchaseSubscription = false;
                LinkButton lnkRescheduleAppointment = ((LinkButton)e.Item.FindControl("lnkRescheduleAppointment"));
                LinkButton lnkModifyShipping = ((LinkButton)e.Item.FindControl("lnkModifyShipping"));
              
                
                List<OrderPaymentDetail> tempOrderPaymentDetail = new List<OrderPaymentDetail>();

                if (IsLocationServiceTenant)
                {
                    if (lnkRescheduleAppointment.IsNotNull())
                    {
                        lnkRescheduleAppointment.Visible = true;
                    }
                    if (lnkModifyShipping.IsNotNull())
                    {
                        lnkModifyShipping.Visible = false;
                    }
                }
                if (e.Item is GridDataItem)
                {
                    var dataItem = (GridDataItem)e.Item;
                    var AmountVal = DataBinder.Eval(dataItem.DataItem, "AMOUNT").IsNullOrEmpty() ? 0 : DataBinder.Eval(dataItem.DataItem, "AMOUNT");
                    dataItem["AMOUNT"].Text = string.Format((new System.Globalization.CultureInfo(LanguageCultures.ENGLISH_CULTURE.GetStringValue())).NumberFormat, "{0:C}", AmountVal);
                    if (IsLocationServiceTenant)
                    {
                        var paymentTypeCode = dataItem.GetDataKeyValue("PaymentTypeCode").ToString();
                        var orderStatusCode = dataItem.GetDataKeyValue("OrderStatusCode").ToString();
                        if (paymentTypeCode.Contains(PaymentOptions.Credit_Card.GetStringValue())
                         && orderStatusCode.Contains(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()))
                        {
                            dataItem["OrderId"].Text = "";
                        }
                    }
                }

                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    String orderId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"].ToString();
                    //UAT-1683 Add the Archive button and Manage Un-Archive to the Screening side
                    LinkButton SendUnArchiveRequest = ((LinkButton)e.Item.FindControl("lnkbtnUnArchiveRequest"));
                    HiddenField hdnIsFromNewOrderClick = ((HiddenField)e.Item.FindControl("hdnIfNeworderClick"));
                    Label lblUnArchiverequestSent = ((Label)e.Item.FindControl("lblUnArchiverequestSent"));
                    //SendUnArchiveRequest.ToolTip = "Click here to send request for Un-Archive";
                    SendUnArchiveRequest.ToolTip = Resources.Language.UNACHVSENDREQ;
                    SendUnArchiveRequest.Visible = false;
                    lblUnArchiverequestSent.Visible = false;

                    LinkButton cancelColumn = ((LinkButton)e.Item.FindControl("btnCancel"));
                    if (orderId.IsNotNull())
                    {
                        vwOrderDetail orderDetail = ListOrderDetail.FirstOrDefault(obj => obj.OrderId == Convert.ToInt32(orderId));
                        //List<Int32> orderIdList = ListOrderDetail.Where(cnd => cnd.PreviousOrderID == Convert.ToInt32(orderId)).Select(x => x.OrderId).ToList();
                        //orderIdList.Add(Convert.ToInt32(orderId));
                        String orderStatusCode = orderDetail.OrderStatusCode;
                        String pendPaymentAppCode = ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue();
                        String orderPaidStatusCode = ApplicantOrderStatus.Paid.GetStringValue();
                        String payOptInvcWithAppCode = PaymentOptions.InvoiceWithApproval.GetStringValue();
                        String payOptInvcWithOutAppCode = PaymentOptions.InvoiceWithOutApproval.GetStringValue();
                     //UAT-916
                        tempOrderPaymentDetail = Presenter.GetAllPaymentDetailsOfOrderByOrderID(Convert.ToInt32(orderId));
                        Boolean ReturnToSender = false, IfAddSvcNewStatus = false;
                        if (IsLocationServiceTenant)
                        {
                            string CABSAppointmentStatus = Presenter.GetCABSAppointmentStatus(Convert.ToInt32(orderId));
                            
                            if (CABSAppointmentStatus != FingerPrintAppointmentStatus.SUBMITTED_TO_CBI.GetStringValue() && CABSAppointmentStatus != FingerPrintAppointmentStatus.CANCELLED.GetStringValue())
                            {
                                List<String> str = Presenter.GetServiceStatus(Convert.ToInt32(orderId), CurrentUserId);
                                if (str.Any() && str.IsNotNull())
                                {
                                    foreach (string item in str)
                                    {
                                        if (item == CABSServiceStatus.RETURNED_TO_SENDER.GetStringValue())
                                        {
                                            ReturnToSender = true;
                                        }
                                        if (item == CABSServiceStatus.NEW.GetStringValue())
                                        {
                                            IfAddSvcNewStatus = true;
                                            hdnIsFromNewOrderClick.Value = Convert.ToString(true);
                                        }
                                    }
                                }
                            }

                        }
                       
                        if (!IsScreeningOnly)
                        {
                            /*UAT-916
                             * if ((orderStatusCode.Equals(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()) ||
                                orderStatusCode.Equals(ApplicantOrderStatus.Paid.GetStringValue())))*/
                            if (tempOrderPaymentDetail.Any(cnd => cnd.lkpOrderStatu != null && (cnd.lkpOrderStatu.Code == pendPaymentAppCode || cnd.lkpOrderStatu.Code == orderPaidStatusCode)))
                            {
                                String rushOrderStatusCode = orderDetail.RushOrderStatusCode;

                                if ((rushOrderStatusCode.IsNullOrEmpty() || (rushOrderStatusCode.IsNotNull()
                                    &&
                                    (
                                       rushOrderStatusCode.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue())
                                        ||
                                       rushOrderStatusCode.Equals(ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue())
                                    )
                                    )) && ShowRushOrder && orderDetail.OrderPackageTypeCode != "AAAB")
                                {
                                    LinkButton linkButton = ((LinkButton)e.Item.FindControl("lbtnPlaceRushOrder"));
                                    linkButton.Visible = true;
                                }
                                if (rushOrderStatusCode.IsNotNull()
                                    && !rushOrderStatusCode.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue())
                                    && !rushOrderStatusCode.Equals(ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue()))
                                {
                                    Label label = ((Label)e.Item.FindControl("lblRushOrderPlaced"));
                                    label.Visible = true;
                                }
                                //if (!Presenter.IsPackageSubscribedForOrderIds(orderIdList) && orderStatusCode != ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue())
                                //{
                                //    LinkButton linkButton = ((LinkButton)e.Item.FindControl("lbtnPlaceRushOrder"));
                                //    linkButton.Visible = false;
                                //}
                            }

                            //UAT 265 Check for Change Subscription:
                            if (Presenter.IsPackageChangeSubscription(Convert.ToInt32(orderId)))
                            {
                                isPackageChangeSubscription = true;
                            }
                            ////Check for Renew Subscrition
                            if (Presenter.IsPackageRenewSubscription(Convert.ToInt32(orderId)))
                            {
                                isPackageRenewSubscription = true;
                            }

                            #region [UAT-977:Additional work towards archive ability]

                            if (Presenter.IsPackageRepurchasedSubscription(Convert.ToInt32(orderId)))
                            {
                                isPackageRepurchaseSubscription = true;
                            }
                            if (IsSubscriptionArchive(orderDetail.PackageSubscriptionArchiveCode))
                            {
                                LinkButton lnkBtnPlaceRushOrder = ((LinkButton)e.Item.FindControl("lbtnPlaceRushOrder"));
                                // ((e.Item as GridDataItem)["CancelColumn"].Controls[0] as LinkButton).Visible = false;
                                cancelColumn.Visible = false;
                                lnkBtnPlaceRushOrder.Visible = false;
                            }

                            #endregion
                            //Added acheck of repurchase subscription that are Archived and expired. 
                            if ((isPackageChangeSubscription || isPackageRenewSubscription || isPackageRepurchaseSubscription))
                            {
                                LinkButton linkButton = ((LinkButton)e.Item.FindControl("lbtnPlaceRushOrder"));
                                if (linkButton.IsNotNull())
                                {
                                    linkButton.Visible = false;
                                }
                            }

                            if (orderDetail.PackageSubscriptionID.IsNull()) //UAT-2387 --Make the cancel link of previous order visible false
                            {
                                cancelColumn.Visible = false;
                            }
                        }
                        if ((orderDetail.Amount) == (Decimal)AppConsts.NONE || orderDetail.Amount == null)
                        {
                            ((GridDataItem)e.Item)["PaymentType"].Text = String.Empty;
                        }


                        //Set amount empty for order having payment type invoice
                        //if (orderDetail.IsInvoiceOnly.IsNotNull() && Convert.ToBoolean(orderDetail.IsInvoiceOnly) == true)
                        //UAT-916 :WB: As an application admin, I should be able to define payment options at the package level in addition to the node level
                        //UAT-3850: Additional location service check.
                        if (!IsLocationServiceTenant.IsNullOrEmpty() && !IsLocationServiceTenant &&
                            tempOrderPaymentDetail.IsNotNull() && tempOrderPaymentDetail.Any(x => x.lkpPaymentOption != null && ((!IsLocationServiceTenant && x.lkpPaymentOption.Code == payOptInvcWithAppCode) || x.lkpPaymentOption.Code == payOptInvcWithOutAppCode)))
                        {
                            ((GridDataItem)e.Item)["Amount"].Text = String.Empty;
                        }
                        // Bug 5512 - Hide 'Place Rush Order' link for Orders having Rush Order price as zero
                        if (!_lstPossibleRushOrders.Contains(Convert.ToInt32(orderId)))
                        {
                            LinkButton lnkbtnRushOrderButton = ((LinkButton)e.Item.FindControl("lbtnPlaceRushOrder"));
                            lnkbtnRushOrderButton.Visible = false;
                        }

                        AppointmentSlotContract AppointSlotContract = Presenter.GetBkgOrderWithAppointmentData(Convert.ToInt32(orderId));
                        Presenter.IsBkgOrderWithAppointment(Convert.ToInt32(orderId));
                        List<OrderPaymentDetail> orderPaymentDetailList = Presenter.GetAllPaymentDetailsOfOrderByOrderID(Convert.ToInt32(orderId));
                        var MaxLocScheduleAllowedDays = Presenter.GetLocTenMaxAllowedDays();

                        if (IsLocationServiceTenant)
                        {
                            if (IsBkgOrderWithAppointment)
                            {
                                if (!AppointSlotContract.IsNullOrEmpty() && !AppointSlotContract.IsOutOfStateAppointment)
                                {
                                    if ((orderPaymentDetailList.IsNotNull() && orderPaymentDetailList.All(cond => cond.lkpOrderStatu != null && (cond.lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue()))))
                                    {
                                        if (lnkRescheduleAppointment.IsNotNull())
                                        {
                                            lnkRescheduleAppointment.Visible = false;
                                        }
                                    }

                                    else
                                    {
                                        var hideReschedlue = FingerPrintDataManager.HideReschedule(AppointSlotContract, CurrentUserTenantId);
                                        if ((AppointSlotContract.SlotDate != null && AppointSlotContract.SlotDate.Value.AddDays(Convert.ToInt32(MaxLocScheduleAllowedDays)).Date < DateTime.Now.Date) || hideReschedlue == true)
                                        {
                                            if (lnkRescheduleAppointment.IsNotNull())
                                            {
                                                lnkRescheduleAppointment.Visible = false;
                                            }
                                        }
                                    }


                                }
                                else
                                {
                                    if (lnkRescheduleAppointment.IsNotNull())
                                    {
                                        lnkRescheduleAppointment.Visible = false;
                                    }

                                }
                            }
                            else
                            {
                                if (lnkRescheduleAppointment.IsNotNull())
                                {
                                    lnkRescheduleAppointment.Visible = false;
                                }

                            }
                            if (IfAddSvcNewStatus || ReturnToSender)
                            {
                                if (lnkModifyShipping.IsNotNull())
                                {
                                    lnkModifyShipping.Visible = true;
                                }
                            }
                           
                        }
                        else
                        {
                            if (lnkRescheduleAppointment.IsNotNull())
                            {
                                lnkRescheduleAppointment.Visible = false;
                            }
                            if (lnkModifyShipping.IsNotNull())
                            {
                                lnkModifyShipping.Visible = false;
                            }
                        }

                        #region Mobility Changes
                        if (orderDetail.SubscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.MobilitySwitched)
                        {
                            LinkButton lnkBtnPlaceRushOrder = ((LinkButton)e.Item.FindControl("lbtnPlaceRushOrder"));
                            Label labelPlaceOrder = ((Label)e.Item.FindControl("lblRushOrderPlaced"));
                            //  ((e.Item as GridDataItem)["CancelColumn"].Controls[0] as LinkButton).Visible = false;
                            cancelColumn.Visible = false;
                            lnkBtnPlaceRushOrder.Visible = false;
                            labelPlaceOrder.Visible = false;
                        }
                        #endregion


                        //Order Completion Report Link
                        if (orderDetail.ShowOrderCompletion == AppConsts.ONE)
                        {
                            grdOrderHistory.Columns.FindByUniqueName("ViewCompletionReport").Visible = true;
                            LinkButton lbtnOrderCompletion = ((LinkButton)e.Item.FindControl("lbtnOrderCompletion"));
                            lbtnOrderCompletion.Visible = true;
                        }

                        if (orderDetail.OrderPackageTypeCode == "AAAB")
                        {
                            LinkButton lnkBtnPlaceRushOrder = ((LinkButton)e.Item.FindControl("lbtnPlaceRushOrder"));
                            Label labelPlaceOrder = ((Label)e.Item.FindControl("lblRushOrderPlaced"));
                            lnkBtnPlaceRushOrder.Visible = false;
                            labelPlaceOrder.Visible = false;
                        }
                        #region UAT-2444

                        if (orderDetail.OrderPackageTypeCode == "AAAA")
                        {
                            cancelColumn.OnClientClick = "return setCancelConfirmationText('" + Resources.Language.CNCLCONFIRMATION + "')";
                            //cancelColumn.OnClientClick = "return setCancelConfirmationText('" + AppConsts.CONFIRMATION_TEXT_FOR_COMPLIANCE_PACKAGE + "')";
                        }
                        else if (orderDetail.OrderPackageTypeCode == "AAAC")
                        {
                            //cancelColumn.OnClientClick = "return setCancelConfirmationText('" + AppConsts.CONFIRMATION_TEXT_FOR_COMPLIANCE_AND_BACKGROUND_PACKAGE + "')";
                            cancelColumn.OnClientClick = "return setCancelConfirmationText('" + Resources.Language.CONFRMFORBKGCHK + "')";
                        }
                        //  cancelColumn.OnClientClick = "setCancelConfirmationText('" + confirmationText + "')";
                        #endregion

                        #region UAT-1269
                        // LinkButton cancelColumn = (e.Item as GridDataItem)["CancelColumn"].Controls[0] as LinkButton;
                        if (cancelColumn.IsNotNull())
                        {
                            cancelColumn.Text = "Cancel Tracking Package";
                            if (orderDetail.OrderPackageTypeCode == "AAAB" || orderStatusCode!="OSPAD" ||
                                orderDetail.PartialOrderCancellationTypeCode == PartialOrderCancellationType.COMPLIANCE_PACKAGE.GetStringValue()
                                || orderDetail.PartialOrderCancellationTypeCode == PartialOrderCancellationType.COMPLIANCE_BACKGROUND_PACKAGES.GetStringValue())
                            {
                                cancelColumn.Visible = false;
                            }
                        }
                        #endregion

                        #region UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"

                        if (!tempOrderPaymentDetail.IsNullOrEmpty() && Presenter.IsOrderAvailableForCompletingProcess(tempOrderPaymentDetail))
                        {
                            LinkButton lnkBtnCompleteOrder = ((LinkButton)e.Item.FindControl("lbtnCompleteOrder"));
                            lnkBtnCompleteOrder.Visible = true;
                        }
                        #endregion

                        if (!tempOrderPaymentDetail.IsNullOrEmpty() && Presenter.IsOrderAvailableForCompletingCompleteModifyshipping(tempOrderPaymentDetail))
                        {
                            LinkButton lnkBtnCompleteModifyshipping = ((LinkButton)e.Item.FindControl("lbtnCompleteModifyshipping"));
                            lnkBtnCompleteModifyshipping.Visible = true;
                            lnkModifyShipping.Visible = false;
                        }
                    }

                    Int32 orderIdForEDS = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"]);

                    #region UAT-1033 Add link to download E Drug authorization form (Electronic Service Form) to screening tab

                    // Set the visibility of the View EDS Form link if Order has EDS service.
                    if (IsScreeningOnly)
                    {
                        //UAT-1683
                        vwOrderDetail BkgorderDetail = ListOrderDetail.FirstOrDefault(obj => obj.OrderId == Convert.ToInt32(orderId));

                        if (EDSExistStatus.ContainsKey(orderIdForEDS))
                        {
                            //HtmlAnchor anchor = (HtmlAnchor)e.Item.FindControl("ancDownldEDS");
                            LinkButton lnk = (LinkButton)e.Item.FindControl("lnkDownldEDS");
                            if (lnk.IsNotNull())
                            {
                                lnk.Visible = EDSExistStatus[orderIdForEDS];
                                if (lnk.Visible)
                                {
                                    grdOrderHistory.Columns.FindByUniqueName("ViewEDSLink").Visible = true;
                                    ApplicantDocument applicantDoc = Presenter.GetEDSDocument(orderIdForEDS);
                                    if (applicantDoc.IsNotNull())
                                    {
                                        string url = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}", applicantDoc.ApplicantDocumentID, CurrentUserTenantId);
                                        lnk.OnClientClick = "DownloadForm('" + url + "')";
                                        // anchor.HRef = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}", applicantDoc.ApplicantDocumentID, CurrentUserTenantId);
                                    }
                                    else
                                    {
                                        lnk.Visible = false;
                                    }
                                }
                            }
                            //UAT-1683 Add the Archive button and Manage Un-Archive to the Screening side
                            if (IsSubscriptionArchive(BkgorderDetail.BKgArchiveCode))
                            {
                                if (Presenter.IsActiveUnArchiveRequestForBkgOrderSubscriptionId(BkgorderDetail.OrderId))
                                {
                                    lblUnArchiverequestSent.Visible = true;
                                }
                                else
                                {
                                    SendUnArchiveRequest.Visible = true;
                                }
                            }
                        }

                        //Service Form
                        Repeater rptServiceForms = (Repeater)e.Item.FindControl("rptServiceForms");
                        if (rptServiceForms != null)
                        {
                            if (lstServiceForm.IsNotNull() && lstServiceForm.Count > AppConsts.NONE)
                            {
                                grdOrderHistory.Columns.FindByUniqueName("ServiceForms").Visible = true;
                                rptServiceForms.DataSource = lstServiceForm.Where(cond => cond.OrderID == orderIdForEDS).ToList();
                                rptServiceForms.DataBind();
                            }
                        }

                    }
                    #endregion

                    LinkButton lnkOrderSummary = (LinkButton)e.Item.FindControl("lbtnOrderSummary");
                    if (lnkOrderSummary.IsNotNull())
                    {
                        if (LstReciptDocumentStatus.IsNotNull())
                        {
                            lnkOrderSummary.Visible = LstReciptDocumentStatus.Where(x => x.Item1 == orderIdForEDS).Select(x => x.Item2).FirstOrDefault();
                            if (lnkOrderSummary.Visible == true)
                            {
                                grdOrderHistory.Columns.FindByUniqueName("PrintReciept").Visible = true;
                            }
                        }
                    }
                }


                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    // String orderId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"].ToString();
                    //List<Int32> orderIdList = new List<Int32>();
                    //if (orderId.IsNotNull())
                    //{
                    //    orderIdList = ListOrderDetail.Where(cnd => cnd.PreviousOrderID == Convert.ToInt32(orderId)).Select(x => x.OrderId).ToList();
                    //    orderIdList.Add(Convert.ToInt32(orderId));
                    //}
                    GridDataItem dataItem = e.Item as GridDataItem;
                    Int32 daysLeftToExpire = 0;
                    String orderStatusCode = Convert.ToString(dataItem["OrderStatusCode"].Text);
                    Int32.TryParse(dataItem["DaysLeftToExpire"].Text, out daysLeftToExpire);
                    LinkButton cancelColumn = ((LinkButton)e.Item.FindControl("btnCancel"));
                    /*UAT-916
                     * if (orderStatusCode == ApplicantOrderStatus.Cancellation_Requested.GetStringValue() ||
                        orderStatusCode == ApplicantOrderStatus.Payment_Rejected.GetStringValue() ||
                        orderStatusCode == ApplicantOrderStatus.Cancelled.GetStringValue() || daysLeftToExpire < 0)*/
                    if ((tempOrderPaymentDetail.FirstOrDefault().lkpOrderStatu.IsNotNull() && (tempOrderPaymentDetail.FirstOrDefault().lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue() ||
                        tempOrderPaymentDetail.FirstOrDefault().lkpOrderStatu.Code == ApplicantOrderStatus.Cancelled.GetStringValue())) ||
                        !tempOrderPaymentDetail.Any(cnd => cnd.lkpOrderStatu.Code != ApplicantOrderStatus.Payment_Rejected.GetStringValue()) || daysLeftToExpire < 0)
                    {
                        //  LinkButton cancelColumn = dataItem["CancelColumn"].Controls[0] as LinkButton;
                        cancelColumn.Visible = false;
                    }
                    //else if (!Presenter.IsPackageSubscribedForOrderIds(orderIdList) && orderStatusCode != ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue())
                    //{
                    //    LinkButton cancelColumn = dataItem["CancelColumn"].Controls[0] as LinkButton;
                    //    cancelColumn.Visible = false;
                    //}

                    //Added acheck of repurchase subscription that are Archived and expired. 

                    if (isPackageChangeSubscription || isPackageRenewSubscription || isPackageRepurchaseSubscription)
                    {
                        //  LinkButton cancelColumn = dataItem["CancelColumn"].Controls[0] as LinkButton;
                        if (cancelColumn.IsNotNull())
                        {
                            cancelColumn.Visible = false;
                        }
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

        protected void grdOrderHistory_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Cancel"))
                {
                    Int32 orderId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"]);

                    //UAT-2021: Auto Approve cancellation requests.
                    //if (Presenter.SendOrderCancelRequest(orderId))
                    //{
                    //    Presenter.SendOrderCancellationNotification(orderId);
                    //}
                    if (Presenter.ApproveCancellations(orderId))
                    {
                        Presenter.SendOrderCancellationApprovalNotification(orderId);
                        ShowMessage("Tracking Package cancelled successfully.", MessageType.SuccessMessage);
                    }
                }
                if (e.CommandName.Equals("PlaceRushOrder"))
                {
                    Int32 subscriptionOptionID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SubscriptionOptionID"]);
                    Int32 orderId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"]);
                    SetApplicationOrderCart(orderId, ControlUseType);
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "OrderId", Convert.ToString(orderId) },
                                                                    { "SubscriptionOptionID",Convert.ToString(subscriptionOptionID)},
                                                                    { "Child", ChildControls.RushOrderReview}
                                                                 };
                    String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
                if (e.CommandName.Equals("ViewDetail"))
                {
                    
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String orderId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"].ToString();
                    String orderNumber = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderNumber"].ToString();
                    Int32 subscriptionOptionID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SubscriptionOptionID"]);
                    Order currentOrder = Presenter.GetOrderByOrderId(Convert.ToInt32(orderId));

                    //SetApplicationOrderCart(Convert.ToInt32(orderId), ControlUseType);
                    bool IsFingerPrintSvc = false, IsPassportPhotoSvc = false;
                    if (IsLocationServiceTenant)
                    {
                       
                        //SetSessionDataToCompleteOrder(Convert.ToInt32(orderId), ControlUseType, subscriptionOptionID, currentOrder);
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.DISCLAIMER_ACCEPTED, true);
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED, true);
                        Session.Remove(ResourceConst.APPLICANT_ORDER_LOCATION_CART);

                        //CABS - to check if order has any additional services(Fingerprint card or passport photo)
                        XmlDocument xmlDoc = new XmlDocument();
                        string BkgOrderServiceDetailsxml = Presenter.GetBkgOrderServiceDetails(Convert.ToInt32(orderId));
                        if (!BkgOrderServiceDetailsxml.IsNullOrEmpty())
                        {
                            xmlDoc.LoadXml(BkgOrderServiceDetailsxml);
                            XmlNodeList elemlist = xmlDoc.GetElementsByTagName("ServiceType");
                            IsFingerPrintSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.FingerPrint_Card.GetStringValue());
                            IsPassportPhotoSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.Passport_Photo.GetStringValue());
                            hdfFingerPrint.Value = Convert.ToString(IsFingerPrintSvc);
                            hdfPassport.Value = Convert.ToString(IsPassportPhotoSvc);
                            hdfOrderID.Value = Convert.ToString(orderId);
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "openOrderPayment()", true);
                            // System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "openOrderPayment();", true);
                        }
                    }

                    if (ControlUseType == AppConsts.DASHBOARD)
                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString( CurrentUserTenantId) },
                                                                    { "Child", ChildControls.OrderPaymentDetails},
                                                                    { "OrderId", orderId},
                                                                    {"ShowApproveRejectButtons",false.ToString()},
                                                                    {"Parent", AppConsts.DASHBOARD_URL},
                                                                    {AppConsts.ORDER_NUMBER, orderNumber},
                                                                    {"hdfFingerPrint",hdfFingerPrint.Value },
                                                                    {"hdfPassport",hdfPassport.Value }
                                                                 };
                    else
                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString( CurrentUserTenantId) },
                                                                    { "Child", ChildControls.OrderPaymentDetails},
                                                                    { "OrderId", orderId},
                                                                    {"ShowApproveRejectButtons",false.ToString()},
                                                                    {AppConsts.ORDER_NUMBER, orderNumber},
                                                                    {"hdfFingerPrint",hdfFingerPrint.Value },
                                                                    {"hdfPassport",hdfPassport.Value }
                                                                 };
                    string url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }

                if (e.CommandName.Equals("Reschedule Appointment"))
                {
                    Int32 subscriptionOptionID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SubscriptionOptionID"]);
                    Int32 orderId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"]);
                    Order currentOrder = Presenter.GetOrderByOrderId(orderId);
                    bool IsFingerPrintSvc = false, IsPassportPhotoSvc = false;
                    if (IsLocationServiceTenant)
                    {
                        // SetSessionDataToCompleteOrder(orderId, ControlUseType, subscriptionOptionID, currentOrder);
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.DISCLAIMER_ACCEPTED, true);
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED, true);
                        Session.Remove(ResourceConst.APPLICANT_ORDER_LOCATION_CART);

                        //CABS - to check if order has any additional services(Fingerprint card or passport photo)
                        XmlDocument xmlDoc = new XmlDocument();
                        string BkgOrderServiceDetailsxml = Presenter.GetBkgOrderServiceDetails(currentOrder.OrderID);
                        if (!BkgOrderServiceDetailsxml.IsNullOrEmpty())
                        {
                            xmlDoc.LoadXml(BkgOrderServiceDetailsxml);
                            XmlNodeList elemlist = xmlDoc.GetElementsByTagName("ServiceType");
                            IsFingerPrintSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.FingerPrint_Card.GetStringValue());
                            IsPassportPhotoSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.Passport_Photo.GetStringValue());
                            hdfFingerPrint.Value = Convert.ToString(IsFingerPrintSvc);
                            hdfPassport.Value = Convert.ToString(IsPassportPhotoSvc);
                            hdfOrderID.Value = Convert.ToString(orderId);
                        }
                        Dictionary<String, String> queryString = new Dictionary<String, String>();
                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"TenantId", Convert.ToString( CurrentUserTenantId) },
                                                                    {"OrderId", hdfOrderID.Value},
                                                                    {"hdfFingerPrint",hdfFingerPrint.Value },
                                                                    {"hdfPassport",hdfPassport.Value },
                                                                    {"Parent", AppConsts.DASHBOARD_URL},
                                                                    {"SubscriptionOptionID", subscriptionOptionID.ToString()}
                                                                 };
                        String url = String.Format("~/ComplianceOperations/Pages/OrderPaymentDetails.aspx?args={0}", queryString.ToEncryptedQueryString());
                        Response.Redirect(url, true);
                        //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "openModifyShippingPopup('" + orderId + "');", true);                         
                        // String url = String.Format(@"~\\ComplianceOperations\\UserControl\\ModifyShippingInfo.ascx?OrderID={0}&tenantId={1}", orderId, CurrentUserTenantId);                   

                    }
                }

                #region UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                if (e.CommandName.Equals("CompleteOrder"))
                {
                    Int32 subscriptionOptionID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SubscriptionOptionID"]);
                    Int32 orderId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"]);
                    Order currentOrder = Presenter.GetOrderByOrderId(orderId);
                    bool IsFingerPrintSvc = false, IsPassportPhotoSvc = false;
                    if (IsLocationServiceTenant)
                    {
                        SetSessionDataToCompleteOrder(orderId, ControlUseType, subscriptionOptionID, currentOrder);
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.DISCLAIMER_ACCEPTED, true);
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED, true);
                        Session.Remove(ResourceConst.APPLICANT_ORDER_LOCATION_CART);

                        //CABS - to check if order has any additional services(Fingerprint card or passport photo)
                        XmlDocument xmlDoc = new XmlDocument();
                        string BkgOrderServiceDetailsxml = Presenter.GetBkgOrderServiceDetails(currentOrder.OrderID);
                        if (!BkgOrderServiceDetailsxml.IsNullOrEmpty())
                        {
                            xmlDoc.LoadXml(BkgOrderServiceDetailsxml);
                            XmlNodeList elemlist = xmlDoc.GetElementsByTagName("ServiceType");
                            IsFingerPrintSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.FingerPrint_Card.GetStringValue());
                            IsPassportPhotoSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.Passport_Photo.GetStringValue());
                        }
                        Dictionary<String, String> queryString = new Dictionary<String, String>();
                        queryString = new Dictionary<String, String>
                                    {
                                        { AppConsts.CHILD, ChildControls.FINGER_PRINTDATA_CONTROL},
                                        {"TenantId", CurrentUserTenantId.ToString()},
                                        {"IsFromOrderHistoryScreen",true.ToString()},
                                        {"IsFingerPrintSvcSelected",IsFingerPrintSvc.ToString()},
                                        {"IsPassportPhotoSvcSelected",IsPassportPhotoSvc.ToString()},
                                        {"OrderId",orderId.ToString() }
                                    };
                        String Url = String.Format("~/FingerPrintSetUp/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                        Response.Redirect(Url, true);
                    }
                    else
                    {
                        INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract itemPaymentContract = Presenter.GetItempaymentDetailsByOrderId(orderId);
                        //UAT-3083
                        if (!currentOrder.IsNullOrEmpty() && itemPaymentContract.ItemID > AppConsts.NONE)
                        {
                            if (itemPaymentContract.TotalPrice > AppConsts.NONE && itemPaymentContract.ItemID > AppConsts.NONE && itemPaymentContract.PkgSubscriptionId > AppConsts.NONE && itemPaymentContract.orderID > AppConsts.NONE && !String.IsNullOrEmpty(itemPaymentContract.invoiceNumber) && itemPaymentContract.OrganizationUserProfileID > AppConsts.NONE)
                            {
                                itemPaymentContract.TenantID = CurrentUserTenantId;
                                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_PARKING_CART, itemPaymentContract);
                                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "IsOrderCreated", AppConsts.TRUE }
                                                                 };
                                String url = String.Format("~/ComplianceOperations/Pages/ItemPaymentPopup.aspx?args={0}", queryString.ToEncryptedQueryString());
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenItemPaymentFormOrderHistory('" + url + "');", true);
                            }
                        }
                        else
                        {
                            SetSessionDataToCompleteOrder(orderId, ControlUseType, subscriptionOptionID, currentOrder);
                            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.DISCLAIMER_ACCEPTED, true);
                            //UAT-1560
                            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED, true);
                            Dictionary<String, String> queryString = new Dictionary<String, String>();
                            queryString = new Dictionary<String, String>
                                                         {
                                                            { AppConsts.CHILD,  ChildControls.ApplicantOrderReview}
                                                         };
                            String url = String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                            Response.Redirect(url, true);
                        }
                    }
                }
                #endregion
                #region UAT-1683 Add the Archive button and Manage Un-Archive to the Screening side

                if (e.CommandName.Equals("UnArchiveRequest"))
                {
                    Int32 orderId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"].ToString());
                    if (orderId > AppConsts.NONE)
                    {
                        if (Presenter.IsActiveUnArchiveRequestForBkgOrderSubscriptionId(orderId))
                            base.ShowInfoMessage(Resources.Language.UNACHVREQALRDYSENT);
                        //base.ShowInfoMessage("Un-archive request for this Background Order is already sent.");
                        else
                        {
                            if (Presenter.SaveBkgOrderUnArchiveRequest(orderId))
                            {
                                grdOrderHistory.Rebind();
                                base.ShowInfoMessage(Resources.Language.UNACHVREQSENTSUCCESSFULLY);
                                //base.ShowInfoMessage("Un-archive request is successfully sent to admin.");
                            }
                            else
                                base.ShowInfoMessage(Resources.Language.SOMEERROCCURD);
                            //base.ShowInfoMessage("Some error has occurred. Please try again.");
                        }
                    }
                }
                #endregion

                #region Modify Shipping
                if (e.CommandName.Equals("ModifyShipping"))
                {
                    Int32 subscriptionOptionID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SubscriptionOptionID"]);
                    Int32 orderId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"]);
                    Order currentOrder = Presenter.GetOrderByOrderId(orderId);
                    bool ifNeworderClick = Convert.ToBoolean((e.Item.FindControl("hdnIfNeworderClick") as HiddenField).Value);
                    bool IsFingerPrintSvc, IsPassportPhotoSvc;
                    if (IsLocationServiceTenant)
                    {
                        SetSessionDataToModifyShipping(orderId, ControlUseType, subscriptionOptionID, currentOrder);
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.DISCLAIMER_ACCEPTED, true);
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED, true);
                        Session.Remove(ResourceConst.APPLICANT_ORDER_LOCATION_CART);

                        //CABS - to check if order has any additional services(Fingerprint card or passport photo)
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(Presenter.GetBkgOrderServiceDetails(currentOrder.OrderID));
                        XmlNodeList elemlist = xmlDoc.GetElementsByTagName("ServiceType");
                        IsFingerPrintSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.FingerPrint_Card.GetStringValue());
                        IsPassportPhotoSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.Passport_Photo.GetStringValue());
                    }
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                                         {
                                                            { AppConsts.CHILD,  @"~/ComplianceOperations/UserControl/ModifyShippingInfo.ascx"},
                                                             {"OrderID", orderId.ToString()},
                                        {"tenantId",CurrentUserTenantId.ToString()},
                                        {"IsFromNewOrderClick",ifNeworderClick.ToString()},
                                        {"PageType", "ModifyShipping"}
                                                         };
                    String url = String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "openModifyShippingPopup('" + orderId + "');", true);                         
                    // String url = String.Format(@"~\\ComplianceOperations\\UserControl\\ModifyShippingInfo.ascx?OrderID={0}&tenantId={1}", orderId, CurrentUserTenantId);                   
                    Response.Redirect(url, true);
                }
                if (e.CommandName.Equals("CompleteModifyshipping"))
                {
                    Int32 subscriptionOptionID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SubscriptionOptionID"]);
                    Int32 orderId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"]);
                    Order currentOrder = Presenter.GetOrderByOrderId(orderId);
                    bool ifNeworderClick = Convert.ToBoolean((e.Item.FindControl("hdnIfNeworderClick") as HiddenField).Value);
                    bool IsFingerPrintSvc, IsPassportPhotoSvc;bool IsCompleteYourPayment=false;
                    var serviceStatus=Presenter.GetServiceStatus(orderId, CurrentUserId);

                    if (serviceStatus != null && serviceStatus[0] == CABSServiceStatus.RETURNED_TO_SENDER.GetStringValue())
                        IsCompleteYourPayment = true;
                    if (IsLocationServiceTenant)
                    {
                        SetSessionDataToModifyShipping(orderId, ControlUseType, subscriptionOptionID, currentOrder);
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.DISCLAIMER_ACCEPTED, true);
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED, true);
                        Session.Remove(ResourceConst.APPLICANT_ORDER_LOCATION_CART);

                        //CABS - to check if order has any additional services(Fingerprint card or passport photo)
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(Presenter.GetBkgOrderServiceDetails(currentOrder.OrderID));
                        XmlNodeList elemlist = xmlDoc.GetElementsByTagName("ServiceType");
                        IsFingerPrintSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.FingerPrint_Card.GetStringValue());
                        IsPassportPhotoSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.Passport_Photo.GetStringValue());
                    }
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                                         {
                                                             { AppConsts.CHILD,  @"~/ComplianceOperations/UserControl/ModifyShippingInfo.ascx"},
                                                             {"OrderID", orderId.ToString()},
                                                             {"tenantId",CurrentUserTenantId.ToString()},
                                                             {"IsFromNewOrderClick",ifNeworderClick.ToString()},
                                                             {"PageType", "ModifyShipping"},
                                                             {"ModifyShippingPayment", true.ToString()},
                                                             {"IsCompleteYourPayment",IsCompleteYourPayment.ToString()}
                        

                                                         };
                    String url = String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "openModifyShippingPopup('" + orderId + "');", true);                         
                    // String url = String.Format(@"~\\ComplianceOperations\\UserControl\\ModifyShippingInfo.ascx?OrderID={0}&tenantId={1}", orderId, CurrentUserTenantId);                   
                    Response.Redirect(url, true);
                }
                #endregion
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
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        protected void grdOrderHistory_PreRender(object sender, EventArgs e)
        {
            grdOrderHistory.MasterTableView.PageSize = 50;
        }
        #endregion

        #region Button Events

        protected void btnUpdateOrderDetails_Click(object sender, EventArgs e)
        {
            try
            {
                grdOrderHistory.Rebind();
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

        protected void btnGotoHome_Click(object sender, EventArgs e)
        {
            Response.Redirect(AppConsts.APPLICANT_MAIN_PAGE_NAME);
        }
        #endregion

        #region DropDown Events



        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        private static void SetApplicationOrderCart(Int32 orderId, String controlUseType)
        {
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;

            if (applicantOrderCart == null)
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                applicantOrderCart.lstApplicantOrder.Add(new ApplicantOrder() { });
            }
            applicantOrderCart.lstApplicantOrder[0].OrderId = orderId;
            applicantOrderCart.AddOrderStageTrackID(OrderStages.OrderHistory);
            applicantOrderCart.lstApplicantOrder[0].OrderId = orderId;
            applicantOrderCart.ParentControlType = controlUseType;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
        }

        #region UAT-977: Additional work towards archive ability
        /// <summary>
        /// Method to return subscription archive status.
        /// </summary>
        /// <param name="ArchiveStatusCode">Archive Code of Subscription</param>
        private Boolean IsSubscriptionArchive(String ArchiveStatusCode)
        {
            Boolean IsSubscriptionArchived = false;
            IsSubscriptionArchived = ArchiveStatusCode.IsNullOrEmpty() ? false : ArchiveStatusCode.Equals(ArchiveState.Archived.GetStringValue()) ? true : false;
            return IsSubscriptionArchived;

        }
        #endregion

        protected void rptServiceForms_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnk = (LinkButton)e.Item.FindControl("lnkDownldSvcFrm");
                if (lnk.IsNotNull())
                {
                    ServiceFormContract serviceFormContract = (ServiceFormContract)e.Item.DataItem;
                    lnk.Text = serviceFormContract.ServiceFormName;
                    string url = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?systemDocumentId={0}&systemDocumentType={1}", serviceFormContract.SystemDocumentID, "DownloadServiceForm");
                    lnk.OnClientClick = "DownloadServiceForm('" + url + "')";

                    ImageButton imgPDF = (ImageButton)e.Item.FindControl("imgPDF");
                    if (imgPDF.IsNotNull())
                    {
                        imgPDF.OnClientClick = "DownloadServiceForm('" + url + "')";
                    }
                }
            }
        }

        #region UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"

        /// <summary>
        /// Method to set Order Data in Applicant Order cart for Completing "SentForOnlinePayment" order.
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <param name="controlUseType">controlUseType</param>
        /// <param name="subscriptionOptionId">subscriptionOptionId</param>
        /// <param name="currentOrder">currentOrder</param>
        private void SetSessionDataToCompleteOrder(Int32 orderId, String controlUseType, Int32 subscriptionOptionId, Order currentOrder)
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
            Session.Remove(AppConsts.DISCLOSURE_ACCEPTED);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;

            if (applicantOrderCart == null)
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                applicantOrderCart.lstApplicantOrder.Add(new ApplicantOrder() { });
            }
            applicantOrderCart.lstApplicantOrder[0].OrderId = orderId;
            applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclaimer);
            //applicantOrderCart.lstApplicantOrder[0].OrderId = orderId;
            applicantOrderCart.ParentControlType = controlUseType;
            applicantOrderCart.IsReadOnly = true;
            if (!currentOrder.IsNullOrEmpty())
            {
                Boolean isCompliancePkgIncluded = false;
                Boolean isBkgPackageIncluded = false;
                Boolean ifInvoiceIsOnlyPaymentOptions = false;
                List<OrderPaymentDetail> sentForOnlinePaymentDetailList = Presenter.GetOPDOfSentForOnlinePaymentStatus(currentOrder);
                isCompliancePkgIncluded = Presenter.IsCompliancePackageIncluded(sentForOnlinePaymentDetailList);
                isBkgPackageIncluded = Presenter.IsBackgroundPackageIncluded(sentForOnlinePaymentDetailList);
                ifInvoiceIsOnlyPaymentOptions = Presenter.IfInvoiceIsOnlyPaymentOptions(currentOrder.SelectedNodeID.Value);
                String compPkgTypeCode = String.Empty;


                applicantOrderCart.SelectedHierarchyNodeID = currentOrder.SelectedNodeID;

                if (!currentOrder.DeptProgramPackage.IsNullOrEmpty() && isCompliancePkgIncluded)
                {
                    if (!currentOrder.DeptProgramPackage.CompliancePackage.IsNullOrEmpty()
                      && !currentOrder.DeptProgramPackage.CompliancePackage.lkpCompliancePackageType.IsNullOrEmpty())
                    {
                        compPkgTypeCode = currentOrder.DeptProgramPackage.CompliancePackage.lkpCompliancePackageType.CPT_Code;
                    }
                    applicantOrderCart.CurrentCompliancePackageTypeInContext = compPkgTypeCode;

                    Int32? _duration = AppConsts.NONE;

                    if (!currentOrder.DeptProgramPackage.DeptProgramMapping.InstitutionNode.IsNullOrEmpty())
                    {
                        _duration = currentOrder.DeptProgramPackage.DeptProgramMapping.InstitutionNode.IN_Duration;
                    }

                    applicantOrderCart.ProgramDuration = _duration;
                    applicantOrderCart.IsCompliancePackageSelected = isCompliancePkgIncluded;
                    applicantOrderCart.DPP_Id = currentOrder.DeptProgramPackage.DPP_ID;
                    applicantOrderCart.CompliancePackageID = currentOrder.DeptProgramPackage.DPP_CompliancePackageID;
                    Decimal _actualPrice = 0;
                    Decimal _netPrice = 0;
                    Decimal _settlementAmount = 0; //UAT-4806

                    DeptProgramPackageSubscription _selectedDpps = currentOrder.DeptProgramPackage.DeptProgramPackageSubscriptions.Where(dpps => !dpps.DPPS_IsDeleted
                                                                                                      && dpps.DPPS_SubscriptionID == subscriptionOptionId).First();
                    GetPricing(_selectedDpps, out _netPrice, out _actualPrice);

                    //if (CurrentViewContext.PreviousOrderId > 0 && applicantOrderCart.OrderRequestType.IsNotNull()
                    //    && applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue())
                    //{
                    //    applicantOrderCart.Amount = Convert.ToString(_netPrice);
                    //    applicantOrderCart.GrandTotal = _netPrice;
                    //}
                    //else
                    //{
                    //Change 15/02/2015
                    //applicantOrderCart.Amount = Convert.ToString(_actualPrice);
                    //applicantOrderCart.GrandTotal = _netPrice;
                    applicantOrderCart.Amount = Convert.ToString(currentOrder.TotalPrice);
                    //applicantOrderCart.GrandTotal = currentOrder.TotalPrice;  //Commented in UAT-4806
                    //Added in //UAT-4086 :- Issue when first time CC payment fails, second attempt to complete the transaction doesn't consider settlement amount.
                    _settlementAmount = !currentOrder.OriginalSettlementPrice.IsNullOrEmpty() && currentOrder.OriginalSettlementPrice.Value > AppConsts.NONE ? Convert.ToDecimal(currentOrder.OriginalSettlementPrice.Value) : AppConsts.NONE;
                    applicantOrderCart.GrandTotal = currentOrder.TotalPrice - _settlementAmount;
                    applicantOrderCart.SettleAmount = _settlementAmount;
                    //END

                    //// In case of Change Program, it will be filled from that screen 
                    //applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();
                    //}

                    applicantOrderCart.CurrentPackagePrice = _actualPrice;


                    applicantOrderCart.DPPS_ID = _selectedDpps.DPPS_ID;
                }
                else
                {
                    if (applicantOrderCart.IsNotNull())
                    {
                        applicantOrderCart.CompliancePackages = new Dictionary<string, OrderCartCompliancePackage>();
                    }
                }

                if (isBkgPackageIncluded)
                {
                    List<BkgOrderPackage> bkgOrderPkgLst = new List<BkgOrderPackage>();
                    bkgOrderPkgLst = Presenter.GetBkgOrderPackageDetail(sentForOnlinePaymentDetailList);

                    AddBackgroundPackageDataToSession(applicantOrderCart, bkgOrderPkgLst, currentOrder.OrderID);
                    GenerateCustomFormData(applicantOrderCart, currentOrder.OrderID);
                    if (IsLocationServiceTenant)
                    {
                        AddOderLineItems(applicantOrderCart, orderId);
                        GetShiippingAddress(applicantOrderCart,orderId);
                    }
                        
                }
                else if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
                {
                    applicantOrderCart.lstApplicantOrder[0].lstPackages = null;
                }
                applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel = ifInvoiceIsOnlyPaymentOptions;
                SetApplicantProfileDataInSession(applicantOrderCart, currentOrder);

                applicantOrderCart.OrderRequestType = OrderRequestType.CompleteOrderByApplicant.GetStringValue();
                if (IsLocationServiceTenant)
                {
                    if (applicantOrderCart.FingerPrintData.IsNull())
                        applicantOrderCart.FingerPrintData = new FingerPrintAppointmentContract();                    
                    applicantOrderCart.FingerPrintData.CBIUniqueID = lstDataForCustomForm.Where(cond => cond.AttributName == "CBIUniqueID").Select(sel => sel.Value).FirstOrDefault();
                    
                }
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
            }
        }


        //Added new code
        private static List<BkgOrderDetailCustomFormUserData> GetAttributesCustomFormIdOrderId(Int32 masterOrderId, Boolean isIncludeMvrData, List<Int32> lstBopIds, Int32 tenantId)
        {
            String bopIds = String.Join(",", lstBopIds);

            BkgOrderDetailCustomFormDataContract bkgOrderDetailCustomFormDataContract = BackgroundProcessOrderManager.GetBkgORDCustomFormAttrDataForCompletingOrder
                                                                                        (tenantId, masterOrderId, bopIds, isIncludeMvrData);
            if (bkgOrderDetailCustomFormDataContract.IsNotNull())
            {                
                if (bkgOrderDetailCustomFormDataContract.lstDataForCustomForm.IsNotNull())
                    return bkgOrderDetailCustomFormDataContract.lstDataForCustomForm;
                else
                    return new List<BkgOrderDetailCustomFormUserData>();
            }
            return new List<BkgOrderDetailCustomFormUserData>();
        }

        //Added new code
        private static List<AttributeFieldsOfSelectedPackages> GetAttributeFieldsOfSelectedPackages(String packageIds, Int32 tenantId)
        {
            List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages> lstAttributeFields = BackgroundProcessOrderManager.GetAttributeFieldsOfSelectedPackages(packageIds, tenantId);
            if (!lstAttributeFields.IsNullOrEmpty())
            {
                return lstAttributeFields.Where(cond => (cond.BSA_Code.ToUpper().Equals("1ADA97AE-9100-4BE6-B829-C914B7FA8750")
                                                                        || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))
                                                                       && cond.AttributeGrpCode.ToUpper().Equals("CF76960D-2120-46FE-9E03-01C218F8A336")).ToList();
            }
            else
            {
                return new List<AttributeFieldsOfSelectedPackages>();
            }
        }

        private void GetShiippingAddress(ApplicantOrderCart applicantOrderCart, Int32 orderId)
        {
            PreviousAddressContract mailingAddress = new PreviousAddressContract();
            XmlDocument xmlDoc = new XmlDocument();


            string BkgOrderServiceDetailsxml = Presenter.GetBkgOrderServiceDetails(orderId);
            if (!BkgOrderServiceDetailsxml.IsNullOrEmpty())
            {
                xmlDoc.LoadXml(BkgOrderServiceDetailsxml);
                XmlNodeList elemlist = xmlDoc.GetElementsByTagName("BkgpkgData");
                var MailingPrice = "";
                var MailingOptionName = ""; 

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

                                MailingOptionName = Presenter.GetShippingLineItemName(node.ChildNodes[i].InnerText);
                                
                            }

                        }
                        if (mailingAddress.CountryId != 0)
                        {
                            mailingAddress.Country = Presenter.GetCountryByCountryId(mailingAddress.CountryId);
                        }
                        if (MailingPrice != "" && MailingOptionName!="")
                        {
                            mailingAddress.MailingOptionPrice = MailingOptionName+"("+MailingPrice+")";
                        }
                    }
                }
            }
            if(mailingAddress.MailingAddressHandleId!=null)
            applicantOrderCart.MailingAddress = mailingAddress;
        }



        private void AddOderLineItems(ApplicantOrderCart applicantOrderCart, Int32 orderId)
        {
            List<OrderLineItem> lineItems = new List<OrderLineItem>();

            //CABS - to check if order has any additional services(Fingerprint card or passport photo)
            List<OrderDetailContract> orderDetailContracts = new List<OrderDetailContract>();
           
           
                XmlDocument xmlDoc = new XmlDocument();


                string BkgOrderServiceDetailsxml = Presenter.GetBkgOrderServiceDetails(orderId);
                if (!BkgOrderServiceDetailsxml.IsNullOrEmpty())
                {
                    xmlDoc.LoadXml(BkgOrderServiceDetailsxml);
                    XmlNodeList elemlist = xmlDoc.GetElementsByTagName("BkgpkgData");

                    foreach (XmlNode node in elemlist)
                    {
                        //List<lkpPaymentOption> lstPaymentOptions = new List<lkpPaymentOption>();
                        // List<BkgPackagePaymentOption> lstBkgPackagePaymentOptions = bop.BkgPackageHierarchyMapping.BkgPackagePaymentOptions.Where(cond => !cond.BPPO_IsDeleted).ToList();
                        OrderLineItem orderLineItem = new OrderLineItem();
                        if (node.HasChildNodes)
                        {

                            for (var i = 0; i < node.ChildNodes.Count; i++)
                            {
                                if (node.ChildNodes[i].Name == "ServiceType")
                                {

                                    orderLineItem.OrderName = Presenter.GetPackageNameForCompleteOrder(orderId, node.ChildNodes[i].InnerText, false);
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
                            lineItems.Add(orderLineItem);
                            OrderLineItem shippingLineItem = new OrderLineItem();
                        
                            for (var j = 0; j < node.ChildNodes.Count; j++)
                            {
                                if (node.ChildNodes[j].Name == "MailingOptionId")
                                {
                                    shippingLineItem.OrderName = "Shipping Fee (" + Presenter.GetShippingLineItemName(node.ChildNodes[j].InnerText) + ")";
                                }
                                if (node.ChildNodes[j].Name == "MailingOptionPrice")
                                {
                                    shippingLineItem.Price = null;
                                    shippingLineItem.Amount = Decimal.Parse(node.ChildNodes[j].InnerText);
                                    shippingLineItem.Quantity = null;
                                }
                            }
                            if ((shippingLineItem.OrderName != null))
                                lineItems.Add(shippingLineItem);
                        
                        
                    }

                    }


                }



                applicantOrderCart.lstOrderLineItems = lineItems.ToList();

            
            
                

        }





        /// <summary>
        /// Add the data of Background Packages to applicantCart
        /// </summary>
        private void AddBackgroundPackageDataToSession(ApplicantOrderCart applicantOrderCart, List<BkgOrderPackage> bkgOrderPkgLst,int orderId)
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
                    IsInvoiceOnlyAtPackageLevel = IsInvoiceOnlyAtPackageLevel  ,
                   
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

                                PackageId = Presenter.GetPackageNameForCompleteOrder(orderId, node.ChildNodes[i].InnerText,true);
                                ServiceCode = node.ChildNodes[i].InnerText;

                            }

                            if (node.ChildNodes[i].Name == "NumberOfCopies")
                            {

                                {
                                    ordrLinItm.Quantity = Int32.Parse(node.ChildNodes[i].InnerText);
                                }
                            }

                            if (node.ChildNodes[i].Name == "FCAdditionalPrice")
                            {

                                {
                                    ordrLinItm.FCAdditionalPrice = Decimal.Parse(node.ChildNodes[i].InnerText);
                                }
                            }

                            if (node.ChildNodes[i].Name == "PPCopiesCount")
                            {

                                {
                                    ordrLinItm.PPQuantity = Int32.Parse(node.ChildNodes[i].InnerText);
                                }
                            }

                            if (node.ChildNodes[i].Name == "PPAdditionalPrice")
                            {

                                {
                                    ordrLinItm.PPAdditionalPrice = Decimal.Parse(node.ChildNodes[i].InnerText);
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
                                if (ordrLinItm.FCAdditionalPrice.HasValue)
                                    pkg.FCAdditionalPrice = (decimal)(ordrLinItm.FCAdditionalPrice);
                                if (ordrLinItm.PPQuantity.HasValue)
                                    pkg.PPCopiesCount = (int)(ordrLinItm.PPQuantity);
                                if (ordrLinItm.PPAdditionalPrice.HasValue)
                                    pkg.PPAdditionalPrice = (decimal)(ordrLinItm.PPAdditionalPrice);
                            }

                        });



                    }


                }
            }

            
            applicantOrderCart.lstApplicantOrder[0].lstPackages = _lstBackgroundPackages;
        }

        /// <summary>
        /// Method to Set Applicant Profile data in ApplicantOrderCart.
        /// </summary>
        /// <param name="applicantOrderCart">applicantOrderCart</param>
        /// <param name="currentOrder">currentOrder</param>
        private void SetApplicantProfileDataInSession(ApplicantOrderCart applicantOrderCart, Order currentOrder)
        {

            Entity.ClientEntity.OrganizationUserProfile organizationUserProfile = new Entity.ClientEntity.OrganizationUserProfile();
            organizationUserProfile.FirstName = currentOrder.OrganizationUserProfile.FirstName;
            organizationUserProfile.LastName = currentOrder.OrganizationUserProfile.LastName;
            organizationUserProfile.MiddleName = currentOrder.OrganizationUserProfile.MiddleName;
            organizationUserProfile.Gender = currentOrder.OrganizationUserProfile.Gender;
            organizationUserProfile.DOB = currentOrder.OrganizationUserProfile.DOB;
            //organizationUserProfile.PrimaryEmailAddress = txtPrimaryEmail.Text;
            //
            organizationUserProfile.PrimaryEmailAddress = currentOrder.OrganizationUserProfile.PrimaryEmailAddress;
            organizationUserProfile.SecondaryEmailAddress = currentOrder.OrganizationUserProfile.SecondaryEmailAddress;
            organizationUserProfile.SecondaryPhone = currentOrder.OrganizationUserProfile.SecondaryPhone;
            organizationUserProfile.SSN = Presenter.GetDecryptedSSN(currentOrder.OrganizationUserProfile.OrganizationUserProfileID);
            organizationUserProfile.OrganizationUserProfileID = currentOrder.OrganizationUserProfile.OrganizationUserProfileID;
            organizationUserProfile.PhoneNumber = currentOrder.OrganizationUserProfile.PhoneNumber;
            organizationUserProfile.OrganizationUserID = currentOrder.OrganizationUserProfile.OrganizationUserID;
            organizationUserProfile.AddressHandleID = currentOrder.OrganizationUserProfile.AddressHandleID;
            organizationUserProfile.IsActive = true;
            //UAT 4243
            if (IsLocationServiceTenant)
            {
                organizationUserProfile.UserTypeID = currentOrder.OrganizationUserProfile.UserTypeID;
            }
            organizationUserProfile.AddressHandle = new AddressHandle
            {
                AddressHandleID = currentOrder.OrganizationUserProfile.AddressHandle.AddressHandleID
            };

            var Addresses = currentOrder.OrganizationUserProfile.AddressHandle.Addresses.FirstOrDefault();
            organizationUserProfile.AddressHandle.Addresses = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Address>();
            organizationUserProfile.AddressHandle.Addresses.Add(new Address
            {
                AddressHandleID = currentOrder.OrganizationUserProfile.AddressHandle.AddressHandleID,
                Address1 = Addresses.Address1,
                Address2 = Addresses.Address2,
                ZipCodeID = Addresses.ZipCodeID
            });

            if (applicantOrderCart != null)
            {
                String clientMachineIP = currentOrder.OrderMachineIP;
                Boolean isUserGroupCustomAttributeExist = false;
                isUserGroupCustomAttributeExist = Presenter.IsUserGroupCustomAttributeExist(CustomAttributeUseTypeContext.Hierarchy.GetStringValue(), currentOrder.SelectedNodeID.Value);
                applicantOrderCart.AddOrganizationUserProfile(organizationUserProfile, false, clientMachineIP);

                applicantOrderCart.IsUserGroupCustomAttributeExist = isUserGroupCustomAttributeExist;
                if (isUserGroupCustomAttributeExist)
                {
                    //TPDO:25/01/2016
                    applicantOrderCart.lstCustomAttributeUserGroupIDs = new List<Int32>();
                    //applicantOrderCart.lstCustomAttributeUserGroupIDs = (caOtherDetails).GetUserGroupCustomAttributeValues();
                }

                //TPDO:25/01/2016
                //applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport = chkSendBkgReport.Checked;
                applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState = false;

                #region UAT-1578 : Addition of SMS notification
                applicantOrderCart.lstApplicantOrder[0].IsReceiveTextNotification = false;
                applicantOrderCart.lstApplicantOrder[0].PhoneNumber = String.Empty;
                #endregion

                //Get Residential history for current order
                List<ResidentialHistoryProfile> lstResidentialHistory = new List<ResidentialHistoryProfile>();
                lstResidentialHistory = currentOrder.OrganizationUserProfile.ResidentialHistoryProfiles.Where(resHis => !resHis.RHIP_IsDeleted).ToList();

                List<PersonAliasProfile> lstPersonAlias = currentOrder.OrganizationUserProfile.PersonAliasProfiles.Where(alias => !alias.PAP_IsDeleted).ToList();

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
                //currentAddress.CountryId = addressLookup.CountryId;
                if (currentAddressDB.Address.ZipCodeID.Value > 0)
                {
                    currentAddress.CountyName = addressLookup.CountyName;
                }
                currentAddress.ResidenceStartDate = currentAddressDB.RHIP_ResidenceStartDate;
                currentAddress.ResidenceEndDate = currentAddressDB.RHIP_ResidenceEndDate;
                currentAddress.isCurrent = currentAddressDB.RHIP_IsCurrentAddress;
                currentAddress.ResHistorySeqOrdID = currentAddressDB.RHIP_SequenceOrder.IsNullOrEmpty() ? AppConsts.ONE : currentAddressDB.RHIP_SequenceOrder.Value;
                applicantOrderCart.lstPrevAddresses = new List<PreviousAddressContract>();
                applicantOrderCart.lstPrevAddresses.Add(currentAddress);
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
                    //applicantOrderCart.lstPrevAddresses = new List<PreviousAddressContract>();
                    applicantOrderCart.lstPrevAddresses.Add(prevAddress);
                    applicantOrderCart.IsResidentialHistoryVisible = true;
                }

                applicantOrderCart.lstPersonAlias = new List<PersonAliasContract>();

                Int32 sequenceNo = AppConsts.NONE;

                lstPersonAlias.ForEach(alias =>
                {
                    sequenceNo += 1;
                    PersonAliasContract personAlias = new PersonAliasContract();
                    personAlias.FirstName = alias.PAP_FirstName;
                    personAlias.LastName = alias.PAP_LastName;
                    personAlias.ID = alias.PAP_ID;
                    personAlias.AliasSequenceId = sequenceNo;
                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                    personAlias.MiddleName = alias.PAP_MiddleName;

                    applicantOrderCart.lstPersonAlias.Add(personAlias);
                });

                applicantOrderCart.IsAccountUpdated = false;
            }
        }

        /// <summary>
        /// Method to generate custom form data.
        /// </summary>
        /// <param name="applicantOrderCart">applicantOrderCart</param>
        /// <param name="orderId">orderId</param>
        private void GenerateCustomFormData(ApplicantOrderCart applicantOrderCart, Int32 orderId)
        {
            List<Int32> lstGroupIds = new List<Int32>();
            List<Int32> lstInstanceIds = null;
            Dictionary<Int32, String> customFormData = null;
            List<BkgOrderDetailCustomFormUserData> lstRefinedData = null;
            List<BackgroundOrderData> lstBackGroundOrderData = new List<BackgroundOrderData>();
            //Get MVR related Attributes.
            Presenter.GetAttributeFieldsOfSelectedPackages(GetPackageIdString(applicantOrderCart));
            Boolean isIncludeMvrData = false;
            if (!lstAttrMVRGrp.IsNullOrEmpty() && lstAttrMVRGrp.Select(x => x.AttributeGrpId).FirstOrDefault() > 0)
            {
                isIncludeMvrData = true;
            }
            Presenter.GetAttributesCustomFormIdOrderId(orderId, isIncludeMvrData);
            List<Int32> lstCustomFormIds = lstDataForCustomForm.Where(cmd => cmd.CustomFormID != AppConsts.NONE).DistinctBy(x => x.CustomFormID).Select(x => x.CustomFormID).ToList();
            //Int32 mvrBkgSvcAttributeGroupId = Convert.ToInt32(lstDataForCustomForm.Where(x=>x.)).FirstOrDefault());
            //applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState = true;
            if (!lstCustomFormIds.IsNullOrEmpty())
            {
                foreach (Int32 customFormID in lstCustomFormIds)
                {
                    List<BkgOrderDetailCustomFormUserData> lstDistinctCustomFormData = new List<BkgOrderDetailCustomFormUserData>();
                    lstDistinctCustomFormData = lstDataForCustomForm.Where(cst => cst.CustomFormID == customFormID).ToList();
                    lstGroupIds = lstDistinctCustomFormData.DistinctBy(x => x.AttributeGroupID).Select(x => x.AttributeGroupID).ToList();
                    for (Int32 grpId = 0; grpId < lstGroupIds.Count; grpId++)
                    {
                        lstInstanceIds = lstDistinctCustomFormData.Where(cond => cond.AttributeGroupID == lstGroupIds[grpId]).DistinctBy(x => x.InstanceID).Select(x => x.InstanceID).ToList();
                        for (Int32 instId = 0; instId < lstInstanceIds.Count; instId++)
                        {
                            customFormData = new Dictionary<Int32, String>();
                            BackgroundOrderData backgroundOrderData = new BackgroundOrderData();
                            lstRefinedData = lstDistinctCustomFormData.Where(cond => cond.InstanceID == lstInstanceIds[instId] && cond.AttributeGroupID == lstGroupIds[grpId]).ToList();

                            backgroundOrderData.InstanceId = lstInstanceIds[instId];
                            backgroundOrderData.BkgSvcAttributeGroupId = lstGroupIds[grpId];
                            backgroundOrderData.CustomFormId = customFormID;
                            foreach (var element in lstRefinedData)
                            {
                                if (!customFormData.ContainsKey(element.AttributeGroupMappingID))
                                    customFormData.Add(element.AttributeGroupMappingID, element.Value);
                            }
                            backgroundOrderData.CustomFormData = customFormData;
                            lstBackGroundOrderData.Add(backgroundOrderData);
                        }
                    }
                }

                #region Mvr Field Set in The Session

                if (isIncludeMvrData)
                {
                    Int32 mvrBkgSvcAttributeGroupId = Convert.ToInt32(lstAttrMVRGrp.Select(x => x.AttributeGrpId).FirstOrDefault());
                    applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState = true;

                    BackgroundOrderData backgroundOrderDataMVR = new BackgroundOrderData();
                    backgroundOrderDataMVR.InstanceId = AppConsts.ONE;
                    //backgroundOrderDataMVR.CustomFormId = (_presenter.GetCustomFormIDBYCode() > 0) ? _presenter.GetCustomFormIDBYCode() : AppConsts.NONE;
                    backgroundOrderDataMVR.CustomFormId = AppConsts.ONE;
                    backgroundOrderDataMVR.BkgSvcAttributeGroupId = Convert.ToInt32(lstAttrMVRGrp.Select(x => x.AttributeGrpId).FirstOrDefault());
                    backgroundOrderDataMVR.CustomFormData = new Dictionary<Int32, String>();
                    Int32 mappingID = 0;
                    mappingID = lstAttrMVRGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID;
                    var MVRDataLiscenceNumber = lstDataForCustomForm.FirstOrDefault(cmd => cmd.CustomFormID == AppConsts.ONE && cmd.AttributeGroupMappingID == mappingID);
                    backgroundOrderDataMVR.CustomFormData.Add(mappingID, MVRDataLiscenceNumber.IsNullOrEmpty() ? String.Empty : MVRDataLiscenceNumber.Value);
                    applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID = mappingID;
                    mappingID = lstAttrMVRGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID;

                    var MVRDataState = lstDataForCustomForm.FirstOrDefault(cmd => cmd.CustomFormID == AppConsts.ONE && cmd.AttributeGroupMappingID == mappingID);
                    backgroundOrderDataMVR.CustomFormData.Add(mappingID, MVRDataState.IsNullOrEmpty() ? String.Empty : MVRDataState.Value);
                    applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID = mappingID;

                    lstBackGroundOrderData.Insert(AppConsts.NONE, backgroundOrderDataMVR);
                }
                #endregion
                applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData = lstBackGroundOrderData;
            }
        }

        /// <summary>
        /// Method to Return Package Ids in String
        /// </summary>
        /// <param name="applicantOrderCart">applicantOrderCart</param>
        /// <returns>String</returns>
        private string GetPackageIdString(ApplicantOrderCart applicantOrderCart)
        {
            String packages = String.Empty;
            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                applicantOrderCart.lstApplicantOrder[0].lstPackages.ForEach(x => packages += Convert.ToString(x.BPAId) + ",");
                //packages = "4";
                if (packages.EndsWith(","))
                    packages = packages.Substring(0, packages.Length - 1);
            }
            return packages;
        }

        private void GetPricing(DeptProgramPackageSubscription deptProgramPackageSubscription, out Decimal netPrice, out Decimal actualPrice)
        {
            netPrice = 0;
            actualPrice = deptProgramPackageSubscription.DPPS_TotalPrice == null ? 0 : deptProgramPackageSubscription.DPPS_TotalPrice.Value;
            netPrice = actualPrice;
        }


        #endregion

        #region As an applicant, I should be able to modify shipping address for an order 

        /// <summary>
        /// Method to set Order Data in Applicant Order cart to modify shipping address.
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <param name="controlUseType">controlUseType</param>
        /// <param name="subscriptionOptionId">subscriptionOptionId</param>
        /// <param name="currentOrder">currentOrder</param>
        private void SetSessionDataToModifyShipping(Int32 orderId, String controlUseType, Int32 subscriptionOptionId, Order currentOrder)
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
            Session.Remove(AppConsts.DISCLOSURE_ACCEPTED);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;

            if (applicantOrderCart == null)
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                applicantOrderCart.lstApplicantOrder.Add(new ApplicantOrder() { });
            }
            applicantOrderCart.lstApplicantOrder[0].OrderId = orderId;
            applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclaimer);
            applicantOrderCart.lstApplicantOrder[0].OrderId = orderId;
            applicantOrderCart.ParentControlType = controlUseType;
            applicantOrderCart.IsReadOnly = true;
            if (!currentOrder.IsNullOrEmpty())
            {
                Boolean isCompliancePkgIncluded = false;
                Boolean isBkgPackageIncluded = false;
                Boolean ifInvoiceIsOnlyPaymentOptions = false;
                List<OrderPaymentDetail> PaymentDetailList = Presenter.GetOrderPaymentDetails(currentOrder);
                isCompliancePkgIncluded = Presenter.IsCompliancePackageIncluded(PaymentDetailList);
                isBkgPackageIncluded = Presenter.IsBackgroundPackageIncluded(PaymentDetailList);
                ifInvoiceIsOnlyPaymentOptions = Presenter.IfInvoiceIsOnlyPaymentOptions(currentOrder.SelectedNodeID.Value);
                String compPkgTypeCode = String.Empty;
                applicantOrderCart.SelectedHierarchyNodeID = currentOrder.SelectedNodeID;

                if (!currentOrder.DeptProgramPackage.IsNullOrEmpty() && isCompliancePkgIncluded)
                {
                    if (!currentOrder.DeptProgramPackage.CompliancePackage.IsNullOrEmpty()
                      && !currentOrder.DeptProgramPackage.CompliancePackage.lkpCompliancePackageType.IsNullOrEmpty())
                    {
                        compPkgTypeCode = currentOrder.DeptProgramPackage.CompliancePackage.lkpCompliancePackageType.CPT_Code;
                    }
                    applicantOrderCart.CurrentCompliancePackageTypeInContext = compPkgTypeCode;

                    Int32? _duration = AppConsts.NONE;

                    if (!currentOrder.DeptProgramPackage.DeptProgramMapping.InstitutionNode.IsNullOrEmpty())
                    {
                        _duration = currentOrder.DeptProgramPackage.DeptProgramMapping.InstitutionNode.IN_Duration;
                    }

                    applicantOrderCart.ProgramDuration = _duration;
                    applicantOrderCart.IsCompliancePackageSelected = isCompliancePkgIncluded;
                    applicantOrderCart.DPP_Id = currentOrder.DeptProgramPackage.DPP_ID;
                    applicantOrderCart.CompliancePackageID = currentOrder.DeptProgramPackage.DPP_CompliancePackageID;
                    Decimal _actualPrice = 0;
                    Decimal _netPrice = 0;
                    Decimal _settlementAmount = 0; //UAT-4806

                    DeptProgramPackageSubscription _selectedDpps = currentOrder.DeptProgramPackage.DeptProgramPackageSubscriptions.Where(dpps => !dpps.DPPS_IsDeleted
                                                                                                      && dpps.DPPS_SubscriptionID == subscriptionOptionId).First();
                    GetPricing(_selectedDpps, out _netPrice, out _actualPrice);

                    //if (CurrentViewContext.PreviousOrderId > 0 && applicantOrderCart.OrderRequestType.IsNotNull()
                    //    && applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue())
                    //{
                    //    applicantOrderCart.Amount = Convert.ToString(_netPrice);
                    //    applicantOrderCart.GrandTotal = _netPrice;
                    //}
                    //else
                    //{
                    //Change 15/02/2015
                    //applicantOrderCart.Amount = Convert.ToString(_actualPrice);
                    //applicantOrderCart.GrandTotal = _netPrice;
                    applicantOrderCart.Amount = Convert.ToString(currentOrder.TotalPrice);
                    //applicantOrderCart.GrandTotal = currentOrder.TotalPrice;  //Commented in UAT-4806
                    //Added in //UAT-4086 :- Issue when first time CC payment fails, second attempt to complete the transaction doesn't consider settlement amount.
                    _settlementAmount = !currentOrder.OriginalSettlementPrice.IsNullOrEmpty() && currentOrder.OriginalSettlementPrice.Value > AppConsts.NONE ? Convert.ToDecimal(currentOrder.OriginalSettlementPrice.Value) : AppConsts.NONE;
                    applicantOrderCart.GrandTotal = currentOrder.TotalPrice - _settlementAmount;
                    applicantOrderCart.SettleAmount = _settlementAmount;
                    //END

                    //// In case of Change Program, it will be filled from that screen 
                    //applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();
                    //}

                    applicantOrderCart.CurrentPackagePrice = _actualPrice;


                    applicantOrderCart.DPPS_ID = _selectedDpps.DPPS_ID;
                }
                else
                {
                    if (applicantOrderCart.IsNotNull())
                    {
                        applicantOrderCart.CompliancePackages = new Dictionary<string, OrderCartCompliancePackage>();
                    }
                }

                if (isBkgPackageIncluded)
                {
                    List<BkgOrderPackage> bkgOrderPkgLst = new List<BkgOrderPackage>();
                    bkgOrderPkgLst = Presenter.GetBkgOrderPackageDetail(PaymentDetailList);

                    AddBackgroundPackageDataToSession(applicantOrderCart, bkgOrderPkgLst,currentOrder.OrderID);
                    GenerateCustomFormData(applicantOrderCart, currentOrder.OrderID);
                    //if (IsLocationServiceTenant)
                    //{
                    //    AddOderLineItems(applicantOrderCart, orderId);
                    //    GetShiippingAddress(applicantOrderCart, orderId);
                    //}
                }
                else if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
                {
                    applicantOrderCart.lstApplicantOrder[0].lstPackages = null;
                }
                applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel = ifInvoiceIsOnlyPaymentOptions;
                SetApplicantProfileDataInSession(applicantOrderCart, currentOrder);

                applicantOrderCart.OrderRequestType = OrderRequestType.ModifyShipping.GetStringValue();
                applicantOrderCart.IsLocationServiceTenant = IsLocationServiceTenant;
                if (IsLocationServiceTenant)
                {
                    if (applicantOrderCart.FingerPrintData.IsNull())
                        applicantOrderCart.FingerPrintData = new FingerPrintAppointmentContract();
                    applicantOrderCart.FingerPrintData.CBIUniqueID = lstDataForCustomForm.Where(cond => cond.AttributName == "CBIUniqueID").Select(sel => sel.Value).FirstOrDefault();
                }
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
            }
        }

        #endregion

        private void ShowMessage(String strMessage, MessageType msgType)
        {
            String msgClass = "info";
            switch (msgType)
            {
                case MessageType.Error:
                    msgClass = "error";
                    break;
                case MessageType.Information:
                    msgClass = "info";
                    break;
                case MessageType.SuccessMessage:
                    msgClass = "sucs";
                    break;
                default:
                    msgClass = "info";
                    break;
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlertMessageWithTitle"
                                                , "$page.showAlertMessageWithTitle('" + strMessage + "','" + msgClass + "',true);", true);
        }

        private void HandleGridColumns()
        {
            if (!MenuId.IsNullOrEmpty() && MenuId == "BKG")
            {
                grdOrderHistory.Columns.FindByUniqueName("BkgOrderStatus").Display = true;
            }
            else
            {
                grdOrderHistory.Columns.FindByUniqueName("BkgOrderStatus").Display = false;
            }

            //if (IsLocationServiceTenant)
            //{
            //    grdOrderHistory.Columns.FindByUniqueName("CompleteOrder").Display = false;
            //}
        }
        #endregion

        #endregion
    }
}

