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

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Threading;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class RenewalOrder : BaseUserControl, IRenewalOrderView
    {
        #region Private Variables
        private RenewalOrderPresenter _presenter = new RenewalOrderPresenter();
        private String _viewType = null;
        #endregion

        #region Properties

        #region Presenter

        public RenewalOrderPresenter Presenter
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

        #region public Properties

        public Int32 TenantId
        {
            get
            {
                if (ViewState["TenantId"] == null)
                    ViewState["TenantId"] = Presenter.GetTenant();
                return (Int32)(ViewState["TenantId"]);
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public IRenewalOrderView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 OrderId
        {
            get
            {
                if (ViewState["OrderId"] != null)
                    return (Int32)ViewState["OrderId"];
                return 0;
            }
            set
            {
                ViewState["OrderId"] = value;
            }
        }

        public Boolean IsDashbordNavigation
        {
            get
            {
                if (ViewState["IsDashbordNavigation"] != null)
                    return (Boolean)ViewState["IsDashbordNavigation"];
                return false;
            }
            set
            {
                ViewState["IsDashbordNavigation"] = value;
            }
        }

        public String FirstName
        {
            //get;
            set
            {
                //txtFirstName.Text = value;
                lblFirstname.Text = value;
            }
        }

        public String LastName
        {
            //get;
            set
            {
                //txtLastName.Text = value;
                lblLastName.Text = value;
            }
        }

        public String InstitutionHierarchy
        {
            //get;
            set
            {
                //lblInstitutionHierarchy.Text = value;
                lblInstitutionHierarchy.Text = value;
            }
        }

        public String PackageName
        {
            //get;
            set
            {
                //txtPackage.Text = value;
                lblPackage.Text = value;
            }
        }

        public Boolean ViewDetails
        {
            get;
            set;
        }

        public String PackageDetail
        {
            //get;
            set
            {
                //txtPackageDetail.Text = value;
                lblPackageDetail.Text = value;
            }
        }

        public Int32? RenewalDuration
        {
            get;
            set;
        }

        public Int32? ProgramDuration
        {
            get
            {
                if (ViewState["ProgramDuration"] != null)
                    return (Int32)ViewState["ProgramDuration"];
                return 0;
            }
            set
            {
                ViewState["ProgramDuration"] = value;
            }
        }

        public Order OrderDetail
        {
            get
            {
                if (ViewState["OrderDetail"] != null)
                    return (Order)ViewState["OrderDetail"];
                return null;
            }
            set
            {
                ViewState["OrderDetail"] = value;
            }
        }
        public Decimal? Price
        {
            get;
            set;
        }

        public Decimal? RushOrderPrice
        {
            get;
            set;
        }

        public Decimal? TotalPrice
        {
            get;
            set;
        }

        public Int32 DPPS_Id
        {
            get
            {
                if (ViewState["DPPS_Id"] != null)
                    return (Int32)ViewState["DPPS_Id"];
                return 0;
            }
            set
            {
                ViewState["DPPS_Id"] = value;
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

        public Int32 NodeId
        {
            get
            {
                if (ViewState["NodeId"] != null)
                    return (Int32)ViewState["NodeId"];
                return 0;
            }
            set
            {
                ViewState["NodeId"] = value;
            }
        }


        public Int32 SelectedDeptProgramId
        {
            get
            {
                if (ViewState["SelectedDeptProgramId"] != null)
                    return (Int32)ViewState["SelectedDeptProgramId"];
                return 0;
            }
            set
            {
                ViewState["SelectedDeptProgramId"] = value;
            }
        }

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

        public Int32 Dpp_Id
        {
            get
            {
                return (Int32)(ViewState["Dpp_Id"] ?? "0");
            }
            set
            {
                ViewState["Dpp_Id"] = value;
            }
        }

        public Int32 MaximumAllowedDuration
        {
            get 
            {
                if (!ViewState["MaximumAllowedDuration"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["MaximumAllowedDuration"]);
                return 0;
            }
            set
            {
                ViewState["MaximumAllowedDuration"] = value;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.Title = "Renewal Order";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                if (!this.IsPostBack)
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
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
                            CurrentViewContext.OrderId = Convert.ToInt32(queryString["OrderId"]);
                            if (applicantOrderCart != null)
                                applicantOrderCart.PrevOrderId = CurrentViewContext.OrderId;
                        }
                    }

                    if (queryString.ContainsKey("ParentControl"))
                    {
                        //Assigns the OrderId to property OrderId.
                        if (!queryString["ParentControl"].IsNullOrEmpty())
                        {
                            if (applicantOrderCart != null)
                                applicantOrderCart.ParentControlType = Convert.ToString(queryString["OrderId"]);
                        }
                    }

                    if (queryString.ContainsKey("IsDashbordNavigation"))
                    {
                        //Assigns the OrderId to property OrderId.
                        if (!queryString["IsDashbordNavigation"].IsNullOrEmpty())
                        {
                            IsDashbordNavigation = Convert.ToBoolean(queryString["IsDashbordNavigation"]);
                        }
                    }

                    Presenter.GetOrderDetail();

                    //start UAT-3641
                    Presenter.getMaximumAllowedDuration(CurrentViewContext.OrderDetail.DeptProgramPackageID.Value);
                    //rngvRenewalDuration.Type = ValidationDataType.Integer;
                    //rngvRenewalDuration.MinimumValue = AppConsts.ONE.ToString();
                    //rngvRenewalDuration.MaximumValue = CurrentViewContext.MaximumAllowedDuration.ToString();
                    //rngvRenewalDuration.Text = "No. of Months should not be greater than " + CurrentViewContext.MaximumAllowedDuration + " .";
                    //end UAT-3641
  
                    ShowHideViewDetailsButton();
                    SetControls(applicantOrderCart);
                    Presenter.OnViewInitialized();
                    cmdbarRenewalOrder.ExtraButton.ValidationGroup = "grpRenewalOrder";
                }
                //Production Issue: The value '' of the MaximumValue property of 'rngvRenewalDuration' cannot be converted to type 'Integer'.
                rngvRenewalDuration.Type = ValidationDataType.Integer;
                rngvRenewalDuration.MinimumValue = AppConsts.ONE.ToString();
                rngvRenewalDuration.MaximumValue = CurrentViewContext.MaximumAllowedDuration.ToString();
                rngvRenewalDuration.Text = "No. of Months should not be greater than " + CurrentViewContext.MaximumAllowedDuration + " .";
                //end

                Presenter.OnViewLoaded();

                base.SetPageTitle("(Step 1)");
                cmdbarRenewalOrder.SaveButton.CausesValidation = false;

                //Hide the rush order service check box on the basis of client setting for Rush Order.
                if (ShowRushOrder)
                {
                    dvRushOrder.Visible = true;
                }
                else
                {
                    dvRushOrder.Visible = false;
                }

                //To set tooltip of buttons
                cmdbarRenewalOrder.SubmitButton.ToolTip = "View detailed requirements associated with this subscription package";
                cmdbarRenewalOrder.ExtraButton.ToolTip = "Continue to next step";
                cmdbarRenewalOrder.ClearButton.ToolTip = "Click to return to the dashboard";

                (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle("Renewal Order");
                cmdbarRenewalOrder.SaveButton.CssClass = "cancelposition";
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

        #region Button Events
        protected void cmdbarContinueOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProgramDuration <= 0 || Convert.ToInt32(txtRenewalDuration.Text) <= ProgramDuration)
                {
                    ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                    if (applicantOrderCart == null)
                        applicantOrderCart = new ApplicantOrderCart();
                    applicantOrderCart.AddDeptProgramPackageSubscriptionId(DPPS_Id);
                    applicantOrderCart.PrevOrderId = CurrentViewContext.OrderId;
                    applicantOrderCart.OrderRequestType = OrderRequestType.RenewalOrder.GetStringValue();
                    applicantOrderCart.RenewalDuration = Convert.ToInt32(txtRenewalDuration.Text);
                    applicantOrderCart.Amount = txtPrice.Text;
                    applicantOrderCart.CurrentPackagePrice = Convert.ToDecimal(txtPrice.Text);
                    applicantOrderCart.RushOrderPrice = txtRushOrderPrice.Text;
                    applicantOrderCart.GrandTotal = Convert.ToDecimal(txtPrice.Text);
                    applicantOrderCart.AddOrderStageTrackID(OrderStages.RenewalOrder);
                    applicantOrderCart.NodeId = CurrentViewContext.NodeId;
                    //applicantOrderCart.SelectedDeptProgramId = CurrentViewContext.SelectedDeptProgramId;

                    // UAT 1067 - Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on.
                    //applicantOrderCart.SelectedHierarchyNodeID = CurrentViewContext.OrderDetail.HierarchyNodeID;
                    applicantOrderCart.SelectedHierarchyNodeID = CurrentViewContext.OrderDetail.SelectedNodeID;

                    applicantOrderCart.CompliancePackageID = OrderDetail.DeptProgramPackage.DPP_CompliancePackageID;
                    applicantOrderCart.DPPS_ID = DPPS_Id;
                    applicantOrderCart.DPP_Id = CurrentViewContext.Dpp_Id;
                    applicantOrderCart.IsCompliancePackageSelected = true;
                    //applicantOrderCart.OrderPaymentdetailId = Presenter.GetComplianceOPDId();
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.SEVEN);
                    applicantOrderCart.IncrementOrderStepCount();
                    //UAT-1560
                    applicantOrderCart.IsAdditionalDocumentExist = false;

                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.ApplicantProfile}
                                                                 };
                    Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

                }
                else
                {
                    base.ShowInfoMessage("Renewal duration can not be greater than remaining program duration " + ProgramDuration + " Month(s).");
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
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void cmdbarGoToDashboard_Click(object sender, EventArgs e)
        {
            try
            {
                ReturnToDashBoard();
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

        protected void cmdBarViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (applicantOrderCart == null)
                    applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.AddDeptProgramPackageSubscriptionId(DPPS_Id);
                applicantOrderCart.PrevOrderId = CurrentViewContext.OrderId;
                applicantOrderCart.RenewalDuration = String.IsNullOrEmpty(txtRenewalDuration.Text) ? AppConsts.NONE : Convert.ToInt32(txtRenewalDuration.Text);
                applicantOrderCart.Amount = txtPrice.Text;
                applicantOrderCart.GrandTotal = String.IsNullOrEmpty(txtRenewalDuration.Text) ? AppConsts.NONE : Convert.ToDecimal(txtPrice.Text);
                applicantOrderCart.OrderRequestType = OrderRequestType.RenewalOrder.GetStringValue();
                applicantOrderCart.RushOrderPrice = txtRushOrderPrice.Text;
                applicantOrderCart.IsCompliancePackageSelected = true;
                applicantOrderCart.AddOrderStageTrackID(OrderStages.RenewalOrder);
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                     { 
                                                                        { "Child", ChildControls.ViewPackageDetail},
                                                                        { "TenantId", TenantId.ToString()},
                                                                        { "PackageId", OrderDetail.DeptProgramPackage.DPP_CompliancePackageID.ToString()}
                                                                     };
                Response.Redirect(String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString()));
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

        protected void cmdbarCancelOrder_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
                Session.Remove(AppConsts.DISCLOSURE_ACCEPTED);
                //UAT-1560
                Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
                //if (IsDashbordNavigation)
                //{
                ReturnToDashBoard();
                //}
                //else
                //{
                //    Dictionary<String, String> queryString = new Dictionary<String, String> { { AppConsts.CHILD, ChildControls.PackageSubscription } };
                //    Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                //}
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
        #endregion

        #endregion

        #region Methods
        #region Private Methods
        private void ReturnToDashBoard()
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            String url = String.Format(AppConsts.DASHBOARD_URL);
            Response.Redirect(url);
        }

        /// <summary>
        /// Method to set renewal duration,price and hidden total price controls.
        /// </summary>
        private void SetControls(ApplicantOrderCart applicantOrderCart)
        {
            if (applicantOrderCart == null)
            {
                txtRenewalDuration.Text = CurrentViewContext.RenewalDuration.ToString();
                txtPrice.Text = CurrentViewContext.Price.ToString();
                txtRushOrderPrice.Text = CurrentViewContext.RushOrderPrice.ToString();
            }
            else
            {
                RedirectIfIncorrectOrderStage(applicantOrderCart);
                txtRenewalDuration.Text = applicantOrderCart.RenewalDuration.ToString();
                txtPrice.Text = applicantOrderCart.GrandTotal.ToString();
                txtRushOrderPrice.Text = applicantOrderCart.RushOrderPrice.ToString();
            }
            hdnTotalPrice.Value = CurrentViewContext.TotalPrice.ToString();

        }


        /// <summary>
        /// Method to Show/Hide View Details link.
        /// </summary>
        private void ShowHideViewDetailsButton()
        {
            if (ViewDetails)
            {
                cmdbarRenewalOrder.DisplayButtons = CommandBarButtons.Save | CommandBarButtons.Extra | CommandBarButtons.Clear | CommandBarButtons.Submit;
            }
            else
            {
                cmdbarRenewalOrder.DisplayButtons = CommandBarButtons.Save | CommandBarButtons.Extra | CommandBarButtons.Clear;
            }
        }

        /// <summary>
        /// Method to reset the controls.
        /// </summary>
        public void ResetControls()
        {
            //txtDepartment.Text = String.Empty;
            //txtFirstName.Text = String.Empty;
            //txtLastName.Text = String.Empty;
            //txtPackage.Text = String.Empty;
            //txtPackageDetail.Text = String.Empty;
            //txtProgram.Text = String.Empty;
            lblPackageDetail.Text = String.Empty;
            lblFirstname.Text = String.Empty;
            lblLastName.Text = String.Empty;
            lblPackage.Text = String.Empty;
            lblPackageDetail.Text = String.Empty;
            // lblProgram.Text = String.Empty;
            //lblDepartment.Text = String.Empty;
            lblInstitutionHierarchy.Text = String.Empty;
            txtPrice.Text = String.Empty;
            txtRushOrderPrice.Text = String.Empty;
            hdnTotalPrice.Value = String.Empty;
            cmdbarRenewalOrder.SubmitButton.Visible = false;
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
            else
            {
                applicantOrderCart.AddOrderStageTrackID(OrderStages.RenewalOrder);
            }
        }

        #endregion

        #region public Methods
        #endregion
        #endregion

    }
}

