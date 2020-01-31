using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class RushOrderConfirmation : BaseUserControl, IRushOrderConfirmationView
    {
        #region Variables

        #region Private Variables

        private RushOrderConfirmationPresenter _presenter = new RushOrderConfirmationPresenter();
        private Int32 _tenantId;
        private OrganizationUserProfile _orgUserProfile;
        private OrganizationUserProfile _organizationUserProfile;
        private ApplicantOrderCart _applicantOrderCart;

        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public RushOrderConfirmationPresenter Presenter
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

        public Int32 TenantId
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 OrderId
        {
            get
            {
                if (ViewState["OrderId"].IsNull())
                {
                    ViewState["OrderId"] = Session["OrderId"];
                }
                return Convert.ToInt32(ViewState["OrderId"]);
            }
        }

        public Int32 DeptProgramPackageSubscriptionId
        {
            get { return (Int32)(ViewState["DeptProgramPackageSubscriptionId"]); }
            set { ViewState["DeptProgramPackageSubscriptionId"] = value; }
        }

        public Int32 SubscriptionId
        {
            get
            {
                if (ViewState["SubscriptionId"].IsNull())
                {
                    ViewState["SubscriptionId"] = Session["SubscriptionId"];
                }
                return Convert.ToInt32(ViewState["SubscriptionId"]);
            }
        }

        public String ClientMachineIP
        {
            get
            {
                if (ViewState["ClientMachineIP"].IsNull())
                {
                    ViewState["ClientMachineIP"] = Session["ClientMachineIP"];
                    Session.Remove("ClientMachineIP");
                }
                return Convert.ToString(ViewState["ClientMachineIP"]);
            }
        }

        public IRushOrderConfirmationView CurrentViewContext
        {
            get { return this; }
        }

        public DeptProgramPackageSubscription SelectedPackageDetails
        {
            get;
            set;
        }

        public Entity.OrganizationUser OrganizationUser
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

        public Int32 OrderPaymentTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the hierarhcy which was selected by applicant, durint actual order placement
        /// i.e. hierarchy by SelectedNodeId
        /// </summary>
        String IRushOrderConfirmationView.SelectedNodeHierarchy
        {
            set
            {
                lblInstitutionHierarchy.Text = value;
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Page_Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Dictionary<String, String> queryString = new Dictionary<string, string>();
                queryString.ToDecryptedQueryString(Request.QueryString["args"]);

                //queryString.Count() will be 2 in case of the source was the Normal order creation. For online payment it will be 1
                if (queryString.Count() > 1 && !queryString["error"].IsNullOrEmpty())
                {
                    base.ShowErrorMessage("An error occured while placing the Rush Order.");
                    pnl.Visible = false;
                }
                else
                {
                    try
                    {
                        Presenter.GetTenantId();
                        pnl.Visible = true;

                        SetApplicationOrderCart();
                        Presenter.OnViewInitialized();
                        BindControls();

                        if (Presenter.IsOrderPaymentDone(lblOrderId.Text))
                        {
                            base.ShowSuccessMessage("Your Rush Order has been successfully placed.");
                        }
                        else
                        {
                            base.ShowInfoMessage("Your payment is not completed for this Rush Order.");
                        }

                        if (!CurrentViewContext.OrderPaymentTypeId.IsNullOrEmpty())
                        {
                            String paymentInstruction = Presenter.GetPaymentInstruction(CurrentViewContext.OrderPaymentTypeId);
                            if (paymentInstruction.IsNotNull())
                            {
                                divPaymentInstruction.Visible = true;
                                litPaymentInstruction.Text = paymentInstruction;
                            }
                            else
                            {
                                divPaymentInstruction.Visible = false;
                            }
                        }

                        base.SetPageTitle(" (Step 2 of 2)");
                        (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle("Rush Order");
                    }
                    catch (Exception)
                    {
                        pnl.Visible = false;
                        base.ShowErrorMessage("An error occured while loading the Rush Order Details.");
                    }
                }
            }
            Presenter.OnViewLoaded();
            cbbuttons.SubmitButton.ToolTip = "Click to return to the dashboard";
        }

        /// <summary>
        /// Redirect to applicant's Dashboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSubmit_Click(object sender, EventArgs e)
        {
            Session.Remove("OrderId");
            Session.Remove("SubscriptionId");
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            String url = String.Format(AppConsts.DASHBOARD_URL);
            Response.Redirect(url);
        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            BindSubscriptionDetail();
            BindPersonalDetail();
        }

        /// <summary>
        /// To bind subscription detail data
        /// </summary>
        private void BindSubscriptionDetail()
        {
            Int32 sub = 0;
            lblOrderId.Text = Convert.ToString(CurrentViewContext.OrderId);
            //Commented lblIPAddress: UAT-1059:Remove I.P. address and mask social security number from order summary
            //lblIPAddress.Text = CurrentViewContext.ClientMachineIP;
            lblInstitutionHierarchy.Text = CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.DeptProgramMapping.DPM_Label;
            //lblPackage.Text = CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.CompliancePackage.PackageName;
            lblPackage.Text = (String.IsNullOrEmpty(CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.CompliancePackage.PackageLabel) ?
                                CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.CompliancePackage.PackageName :
                                CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.CompliancePackage.PackageLabel).HtmlEncode();

            if (CurrentViewContext.SelectedPackageDetails.DPPS_RushOrderAdditionalPrice.HasValue)
            {
                lblRushOrderPrice.Text = "$ " + Convert.ToString(decimal.Round(CurrentViewContext.SelectedPackageDetails.DPPS_RushOrderAdditionalPrice.Value, 2));
            }
            if (CurrentViewContext.SelectedPackageDetails.SubscriptionOption.Year != null)
            {
                sub = (CurrentViewContext.SelectedPackageDetails.SubscriptionOption.Year ?? 0) * 12;
            }
            if (CurrentViewContext.SelectedPackageDetails.SubscriptionOption.Month != null)
            {
                sub += (CurrentViewContext.SelectedPackageDetails.SubscriptionOption.Month ?? 0);
            }
            lblSubscription.Text = sub.ToString();
        }

        /// <summary>
        /// To bind personal detail data
        /// </summary>
        private void BindPersonalDetail()
        {
            lblFirstName.Text = CurrentViewContext.OrganizationUser.FirstName;
            lblLastName.Text = CurrentViewContext.OrganizationUser.LastName;
            if (CurrentViewContext.OrganizationUser.DOB.HasValue)
            {
                //UAT 1190
                lblDateOfBirth.Text = Presenter.GetMaskDOB(CurrentViewContext.OrganizationUser.DOB.Value.ToShortDateString()); //CurrentViewContext.OrganizationUser.DOB.Value.ToShortDateString();
            }
            lblEmail.Text = CurrentViewContext.OrganizationUser.PrimaryEmailAddress;
            lblPhone.Text = Presenter.GetFormattedPhoneNumber(CurrentViewContext.OrganizationUser.PhoneNumber);
            //CurrentViewContext.DeptProgramPackageSubscriptionId = CurrentViewContext.SelectedPackageDetails.DPPS_ID;
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

        private void SetApplicationOrderCart()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            RedirectIfIncorrectOrderStage(_applicantOrderCart);
            _applicantOrderCart.AddOrderStageTrackID(OrderStages.RushOrderConfirmation);
        }

        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (_applicantOrderCart.IsNull())
            {
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return _applicantOrderCart;
        }

        #endregion

        #endregion 
    }
}

