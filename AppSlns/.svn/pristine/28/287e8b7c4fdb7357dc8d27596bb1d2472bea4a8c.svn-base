using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class RenewalOrderOptions : BaseUserControl, IRenewalOrderOptions
    {

        #region Private Variables
        private RenewalOrderOptionsPresenter _presenter = new RenewalOrderOptionsPresenter();
        private String _viewType = null;
        #endregion

        #region PROPERTIES
        #region Presenter

        public RenewalOrderOptionsPresenter Presenter
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
        #endregion

        Int32 IRenewalOrderOptions.OrderID
        {
            get
            {
                return Convert.ToInt32(ViewState["OrderID"]);
            }
            set
            {
                ViewState["OrderID"] = value;
            }
        }

        Dictionary<String, String> IRenewalOrderOptions.DicOrderDetails
        {
            get
            {
                return (Dictionary<String, String>)ViewState["DicOrderDetails"];
            }
            set
            {
                ViewState["DicOrderDetails"] = value;
            }
        }

        Int32 IRenewalOrderOptions.CurrentUserTenantID
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentUserTenantID"]);
            }
            set
            {
                ViewState["CurrentUserTenantID"] = value;
            }
        }

        IRenewalOrderOptions CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion


        #region EVENTS

        /// <summary>
        /// Page Init Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            //base.OnInit(e);
            //base.Title = "Renewal Order Options";
        }

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                base.SetPageTitle("Renewal Order Options");
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                Dictionary<String, String> queryString = new Dictionary<String, String>();

                //Decrypt the OrderId from Query String.
                if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                {
                    queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                }

                //Checks if the OrderId is present in Query String.
                if (queryString.ContainsKey("OrderId"))
                {
                    //Assigns the OrderId to property OrderId.
                    if (!queryString["OrderId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.OrderID = Convert.ToInt32(queryString["OrderId"]);
                    }
                }

                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    CurrentViewContext.CurrentUserTenantID = user.TenantId.HasValue ? user.TenantId.Value : 0;
                }
            }
            SetCommandbarButton();
        }

        /// <summary>
        /// Proceed Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString;

                if (rbRenewalOrderOptions.SelectedValue == "renew")
                {
                    queryString = new Dictionary<String, String>
                                     { 
                                        { "OrderId", Convert.ToString(CurrentViewContext.OrderID) },
                                        { "Child",ChildControls.RenewalOrder} 
                                     };
                    String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
                else
                {
                    if (ChangeSubscription() == true)
                    {
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString(CurrentViewContext.CurrentUserTenantID) }, 
                                                                    { "Child", ChildControls.ApplicantPendingOrder},
                                                                    { AppConsts.PENDING_ORDER_NAVIGATION_FROM, PendingOrderNavigationFrom.ApplicantChangeSubscription.GetStringValue()}
                                                                 };
                        String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        Response.Redirect(url, true);
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
        /// Cancel Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
                Session.Remove(AppConsts.DISCLOSURE_ACCEPTED);
                //UAT-1560
                Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
                String url = String.Format(AppConsts.DASHBOARD_URL);
                Response.Redirect(url);
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

        #region Private Events
        private void SetCommandbarButton()
        {
            cmdBar.SubmitButtonText = AppConsts.NEXT_BUTTON_TEXT;            
            cmdBar.SaveButton.CssClass = "cancelposition";
        }
        #endregion

        #endregion

        private Boolean ChangeSubscription()
        {
            CurrentViewContext.DicOrderDetails = Presenter.GetOrderDetails();
            if (CurrentViewContext.DicOrderDetails != null && CurrentViewContext.DicOrderDetails.Count > 0)
            {
                if (CurrentViewContext.DicOrderDetails.ContainsKey("ExpiryDate") && CurrentViewContext.DicOrderDetails.ContainsKey("PackagePrice") && CurrentViewContext.DicOrderDetails.ContainsKey("TotalMonthsInPackage"))
                {
                    //-------------------------------------------------------------
                    //Below Code commented for UAT-1188 Change request
                    //-------------------------------------------------------------
                    //DateTime ExpiryDate = Convert.ToDateTime(CurrentViewContext.DicOrderDetails["ExpiryDate"]);

                    //Decimal packagePrice = Convert.ToDecimal(CurrentViewContext.DicOrderDetails["PackagePrice"]);
                    //Int32 totalMonthsInPackage = Convert.ToInt32(CurrentViewContext.DicOrderDetails["TotalMonthsInPackage"]);

                    //if (ExpiryDate.Date > DateTime.Now.Date)
                    //{
                    //    int remainingMonth = 0;
                    //    if (ExpiryDate.Day < DateTime.Now.Day)
                    //        remainingMonth = ((ExpiryDate.Year - DateTime.Now.Year) * 12) + ExpiryDate.Month - DateTime.Now.Month - 1;
                    //    else
                    //        remainingMonth = ((ExpiryDate.Year - DateTime.Now.Year) * 12) + ExpiryDate.Month - DateTime.Now.Month;
                    //    if (remainingMonth > 0)
                    //    {
                    //        ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                    //        if (applicantOrderCart == null)
                    //            applicantOrderCart = new ApplicantOrderCart();
                    //        applicantOrderCart.PrevOrderId = CurrentViewContext.OrderID;

                    //        Decimal settlementAmount = (remainingMonth * ((packagePrice) / (totalMonthsInPackage)));
                    //        applicantOrderCart.SettleAmount = Decimal.Round(settlementAmount, 2, MidpointRounding.AwayFromZero);
                    //        //Convert.ToDecimal(String.Format("${0}", settlementAmount.ToString("0.00")));
                    //        applicantOrderCart.OrderRequestType = OrderRequestType.ChangeSubscription.GetStringValue();
                    //        //change done for Applicant Dashboard Redesign.
                    //        applicantOrderCart.ParentControlType = AppConsts.DASHBOARD; 
                    //        SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
                    //        return true;
                    //    }
                    //    else
                    //        base.ShowInfoMessage("Your pending subscription period is less than one month, so it could not be adjusted.");
                    //}
                    //else
                    //    base.ShowInfoMessage("Your selected subscription is already expired.");


                    DateTime ExpiryDate = Convert.ToDateTime(CurrentViewContext.DicOrderDetails["ExpiryDate"]);

                    ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                    if (applicantOrderCart == null)
                        applicantOrderCart = new ApplicantOrderCart();
                    applicantOrderCart.PrevOrderId = CurrentViewContext.OrderID;
                    applicantOrderCart.OrderRequestType = OrderRequestType.ChangeSubscription.GetStringValue();
                    applicantOrderCart.ParentControlType = AppConsts.DASHBOARD;
                    applicantOrderCart.SettleAmount = AppConsts.NONE;

                    Decimal packagePrice = Convert.ToDecimal(CurrentViewContext.DicOrderDetails["PackagePrice"]);
                    Int32 totalMonthsInPackage = Convert.ToInt32(CurrentViewContext.DicOrderDetails["TotalMonthsInPackage"]);

                    if (ExpiryDate.Date > DateTime.Now.Date)
                    {
                        int remainingMonth = 0;
                        if (ExpiryDate.Day < DateTime.Now.Day)
                            remainingMonth = ((ExpiryDate.Year - DateTime.Now.Year) * 12) + ExpiryDate.Month - DateTime.Now.Month - 1;
                        else
                            remainingMonth = ((ExpiryDate.Year - DateTime.Now.Year) * 12) + ExpiryDate.Month - DateTime.Now.Month;
                        if (remainingMonth > 0)
                        {
                            Decimal settlementAmount = (remainingMonth * ((packagePrice) / (totalMonthsInPackage)));
                            applicantOrderCart.SettleAmount = Decimal.Round(settlementAmount, 2, MidpointRounding.AwayFromZero);
                        }
                    }
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
                    return true;
                }
            }
            return false;
        }

        protected void cmdBar_SubmitClick()
        {

        }
    }
}