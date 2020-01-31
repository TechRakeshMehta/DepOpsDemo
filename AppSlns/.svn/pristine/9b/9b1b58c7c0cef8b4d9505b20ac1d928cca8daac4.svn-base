using CoreWeb.FingerPrintSetUp.Views;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class AdminEntryApplicantLandingScreen : BaseUserControl, IAdminEntryApplicantLandingView
    {
        #region Variables

        #region Private Variables

        private ApplicantOrderCart applicantOrderCart;
        private Int32 _defaultNodeId = 0;
        private AdminEntryApplicantLandingPresenter _presenter = new AdminEntryApplicantLandingPresenter();

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties
        public AdminEntryApplicantLandingPresenter Presenter
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

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                //return 134;
                return base.CurrentUserId;
            }
        }

        public IAdminEntryApplicantLandingView CurrentViewContext
        {
            get { return this; }
        }
        public Int32 TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNullOrEmpty())
                    return (Int32)(ViewState["TenantId"]);
                return 0;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }
        public Int32 OrderId
        {
            get
            {
                if (!ViewState["OrderId"].IsNullOrEmpty())
                    return (Int32)ViewState["OrderId"];
                return AppConsts.NONE;
            }
            set
            {
                ViewState["OrderId"] = value;
            }
        }

        /// <summary>
        /// Institution Id of the last node selected in the pending order screen. Used to get the associated Custom attributes for this institution.
        /// </summary>
        public Int32 NodeId
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["NodeId"])))
                    return (Int32)ViewState["NodeId"];
                return 0;
            }
            set
            {
                ViewState["NodeId"] = value;
            }
        }
        public Boolean IsAdditionalDocumentExist { get; set; }
        public ApplicantOrderCart applicantOrderCartData
        {
            get
            {
                if (!ViewState["applicantOrderCartData"].IsNullOrEmpty())
                {
                    return ViewState["applicantOrderCartData"] as ApplicantOrderCart;
                }
                return new ApplicantOrderCart();
            }
            set
            {
                ViewState["applicantOrderCartData"] = value;
            }
        }
        public String TenantName
        {
            get
            {
                if (!ViewState["TenantName"].IsNullOrEmpty())
                {
                    return ViewState["TenantName"].ToString();
                }
                return String.Empty;
            }
            set
            {
                ViewState["TenantName"] = value;
            }
        }

        String IAdminEntryApplicantLandingView.NodeName
        {
            get
            {
                if (!ViewState["NodeName"].IsNullOrEmpty())
                {
                    return ViewState["NodeName"].ToString();
                }
                return String.Empty;
            }
            set
            {
                ViewState["NodeName"] = value;
            }
        }

        Boolean IAdminEntryApplicantLandingView.IsLinkExpiredOrOrderDeleted
        {
            get; set;
        }

        String IAdminEntryApplicantLandingView.TokenKey
        {
            get; set;
        }
        #endregion

        #endregion

        #region Events

        #region Page Events
        /// <summary>
        /// set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Complete Your Order";
                base.SetPageTitle("Complete Your Order");
                base.OnInit(e);
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                String NodeName = String.Empty;
                if (!Page.IsPostBack)
                {
                    CaptureQueryString();
                    CurrentViewContext.TokenKey = Convert.ToString(SysXWebSiteUtils.SessionService.GetCustomData("TokenKey"));
                    Presenter.InitializedData();
                    //if (!CurrentViewContext.applicantOrderCartData.IsNullOrEmpty() && !CurrentViewContext.applicantOrderCartData.lstApplicantOrder.IsNullOrEmpty() && CurrentViewContext.applicantOrderCartData.lstApplicantOrder.Count > 0)
                    //{
                    //    //NodeName = CurrentViewContext.applicantOrderCartData.lstApplicantOrder[0].lstPackages.Count > 0 ? CurrentViewContext.applicantOrderCartData.lstApplicantOrder[0].lstPackages[0].HierarchyNodeName : String.Empty;
                    //    NodeName = CurrentViewContext.applicantOrderCartData.HierarchyNodeName.IsNullOrEmpty()?  String.Empty: CurrentViewContext.applicantOrderCartData.HierarchyNodeName;
                    //}
                }
                if (!CurrentViewContext.IsLinkExpiredOrOrderDeleted)
                {
                    String content=Presenter.GetApplicantInviteContent();
                    if (content.IsNullOrEmpty())
                    {
                        lblAdminEntryWlcmMsg.Text = "<p>Welcome to the American DataBank Background Screening Portal.</p><p> " + CurrentViewContext.TenantName + " - " + CurrentViewContext.NodeName
                                            + " has requested that you complete a background check. To enter your information, please follow the steps indicated in the following pages.</p><p> Please note, if you close this screen, information you have already entered may not be saved.";
                    }
                    else
                    {
                        lblAdminEntryWlcmMsg.Visible = false;
                        dvAdminEntryWlcmMsg.Visible = true;
                        dvAdminEntryWlcmMsg.Text = content.ToString();
                    }
                    btnBegin.Enabled = true;
                    btnBegin.Visible = true;
                    btnclose.Enabled = false;
                    btnclose.Visible = false;
                }
                else
                {
                    lblAdminEntryWlcmMsg.Text = "Invite is no longer live.";
                    btnBegin.Enabled = false;
                    btnBegin.Visible = false;
                    btnclose.Enabled = true;
                    btnclose.Visible = true;
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

        #region Button Events
        protected void btnBegin_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.GetApplicantCartData();
                StartOrder();
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.ADMIN_ENTRY_APPLICANT_INFORMATION},
                                                                    {"OrderId" ,  applicantOrderCart.OrderId.ToString()}
                                                                 };
                string url = String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
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

        protected void btnclose_Click(object sender, EventArgs e)
        {
            try
            {
                SysXWebSiteUtils.SessionService.ClearSession(true);
                FormsAuthentication.RedirectToLoginPage();
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

        #endregion

        #region Methods

        #region private Methods
        //Method to start order process and add applicant order cart.
        private void StartOrder()
        {
            applicantOrderCart = applicantOrderCartData;
            if (applicantOrderCart == null)
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.GetApplicantOrder();
            }
            applicantOrderCart.OrderId = CurrentViewContext.OrderId;
            applicantOrderCart.TenantId = CurrentViewContext.TenantId;
            //if (applicantOrderCart.IsNotNull() && (CurrentViewContext.FingerPrintData.IsLocationServiceTenant))
            //{
            //    if (applicantOrderCart.FingerPrintData.IsNull())
            //        applicantOrderCart.FingerPrintData = new FingerPrintAppointmentContract();
            //    applicantOrderCart.FingerPrintData.LocationId = CurrentViewContext.FingerPrintData.LocationId;
            //    applicantOrderCart.FingerPrintData.LocationName = CurrentViewContext.FingerPrintData.LocationName;
            //    applicantOrderCart.FingerPrintData.LocationAddress = CurrentViewContext.FingerPrintData.LocationAddress;
            //    applicantOrderCart.FingerPrintData.LocationDescription = CurrentViewContext.FingerPrintData.LocationDescription;
            //    applicantOrderCart.FingerPrintData.IsEventCode = CurrentViewContext.FingerPrintData.IsEventCode;
            //    applicantOrderCart.FingerPrintData.StartTime = CurrentViewContext.FingerPrintData.StartTime;
            //    applicantOrderCart.FingerPrintData.EndTime = CurrentViewContext.FingerPrintData.EndTime;
            //    applicantOrderCart.FingerPrintData.SlotID = CurrentViewContext.FingerPrintData.SlotID;
            //    applicantOrderCart.FingerPrintData.SlotDate = CurrentViewContext.FingerPrintData.SlotDate;
            //    applicantOrderCart.FingerPrintData.EventName = CurrentViewContext.FingerPrintData.EventName;
            //    applicantOrderCart.FingerPrintData.EventDescription = CurrentViewContext.FingerPrintData.EventDescription;
            //    applicantOrderCart.IsLocationServiceTenant = true;
            //    applicantOrderCart.FingerPrintData.CBIUniqueID = CurrentViewContext.FingerPrintData.CBIUniqueID.Trim();
            //    applicantOrderCart.FingerPrintData.IsSSNRequired = CurrentViewContext.FingerPrintData.IsSSNRequired;
            //    applicantOrderCart.FingerPrintData.lstAutoFilledAttributes = CurrentViewContext.FingerPrintData.lstAutoFilledAttributes;
            //    applicantOrderCart.FingerPrintData.BillingCode = CurrentViewContext.FingerPrintData.BillingCode;
            //    applicantOrderCart.FingerPrintData.IsLegalNameChange = CurrentViewContext.FingerPrintData.IsLegalNameChange;
            //    //UAT-3850
            //    applicantOrderCart.FingerPrintData.BillingCodeAmount = CurrentViewContext.FingerPrintData.BillingCodeAmount;
            //    applicantOrderCart.IncrementOrderStepCount();
            //}

            //if (String.IsNullOrEmpty(applicantOrderCart.PendingOrderNavigationFrom))
            //{
            //    applicantOrderCart.PendingOrderNavigationFrom = GetNavigationFrom();
            //}

            if (!String.IsNullOrEmpty(applicantOrderCart.EDrugScreeningRegistrationId))
            {
                applicantOrderCart.EDrugScreeningRegistrationId = null;
            }

            if (applicantOrderCart.IsNotNull())
            {
                applicantOrderCart.alNodeIds = new ArrayList();
                applicantOrderCart.alNodeIds.Add(applicantOrderCart.SelectedHierarchyNodeID);
                applicantOrderCart.DefaultNodeId = null;
            }
            //SetSelectedHierarchyData();

            //var _isBundleSelected = false;

            //if (applicantOrderCart.IsNotNull() && applicantOrderCart.OrderRequestType != OrderRequestType.ChangeSubscription.GetStringValue() && !_isBundleSelected)
            //{
            //    AddBackgroundPackageDataToSession();
            //}


            applicantOrderCart.AddOrderStageTrackID(OrderStages.PendingOrder);

            // If No package is selected, then stop navigation
            if (!applicantOrderCart.IsCompliancePackageSelected && applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                base.ShowInfoMessage(Resources.Language.PLSSLCTPCKGS);
                return;
            }


            Int32 _customFormSteps = AppConsts.NONE;

            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                _customFormSteps = GetTotalCustomForms(applicantOrderCart.lstApplicantOrder[0].lstPackages.ToList());
            }

            //UAT 3521 Add two steps in existing flow because appointment and location screens are added
            //if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            //{
            //    if (CurrentViewContext.FingerPrintData.IsEventCode)
            //    {
            //        applicantOrderCart.SetTotalOrderSteps(AppConsts.SIX + _customFormSteps);
            //    }
            //    else if (CurrentViewContext.FingerPrintData.IsOutOfState)
            //    {
            //        applicantOrderCart.SetTotalOrderSteps(AppConsts.SIX + _customFormSteps);
            //    }
            //    else
            //        applicantOrderCart.SetTotalOrderSteps(AppConsts.SEVEN + _customFormSteps);
            //}
            //else
            //{
            // For placing 'Rush Order for existing order' and 'Renew subscription', different screens are used
            //Set Total Order steps to Seven because a new Required Documentation screen is added in order flow [UAT-1560]
            applicantOrderCart.SetTotalOrderSteps(AppConsts.FOUR + _customFormSteps);


            // }

            //applicantOrderCart.IncrementOrderStepCount();
            applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.ONE;
            applicantOrderCart.lstApplicantOrder[0].OrderId = applicantOrderCart.OrderId;
            var selectedHierarchyNodeId = (applicantOrderCart.SelectedHierarchyNodeID.HasValue && applicantOrderCart.SelectedHierarchyNodeID.Value > AppConsts.NONE) ? applicantOrderCart.SelectedHierarchyNodeID.Value : Presenter.GetInstitutionDPMID();
            applicantOrderCart.NodeId = CurrentViewContext.NodeId = Presenter.GetLastNodeInstitutionId(selectedHierarchyNodeId);
            applicantOrderCart.SelectedHierarchyNodeID = selectedHierarchyNodeId > AppConsts.NONE ? selectedHierarchyNodeId : AppConsts.NONE;

            Int32 selectedNodeId;

            selectedNodeId = Convert.ToInt32(selectedHierarchyNodeId);


            //IfInvoiceOnlyPymnOptn = Presenter.IfInvoiceIsOnlyPaymentOptions(selectedNodeId);

            // For now we only have "Invoice without approval" so the value of "IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel" must be true
            applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel = true;

            #region UAT-1560: WB: We should be able to add documents that need to be signed to the order process
            Presenter.GetAdditionalDocuments(applicantOrderCart.lstApplicantOrder[0].lstPackages, applicantOrderCart.SelectedHierarchyNodeID.Value, applicantOrderCart.CompliancePackages, applicantOrderCart.IsCompliancePackageSelected);
            applicantOrderCart.IsAdditionalDocumentExist = IsAdditionalDocumentExist;

            if (IsAdditionalDocumentExist)
            {
                applicantOrderCart.SetTotalOrderSteps(AppConsts.FIVE + _customFormSteps);
            }
            applicantOrderCart.IsAdminEntryPortalOrder = true;
            applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();
            #endregion
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
        }

        /// <summary>
        /// Gets the total custom forms for the selected Background packages selected,
        /// to add the Total count to Session
        /// </summary>
        private Int32 GetTotalCustomForms(List<BackgroundPackagesContract> lstBkgPackages)
        {
            String _packageIds = String.Empty;
            if (!lstBkgPackages.IsNullOrEmpty())
            {
                lstBkgPackages.ForEach(pkgId => _packageIds += Convert.ToString(pkgId.BPAId) + ",");

                if (_packageIds.EndsWith(","))
                    _packageIds = _packageIds.Substring(0, _packageIds.Length - 1);
            }

            if (!String.IsNullOrEmpty(_packageIds))
                return Presenter.GetCustomFormsCount(_packageIds);

            return AppConsts.NONE;
        }

        /// <summary>
        /// Method to get the args in Query string.
        /// </summary>
        private void CaptureQueryString()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("OrderId"))
                {
                    CurrentViewContext.OrderId = Convert.ToInt32(args["OrderId"]);
                }
                if (args.ContainsKey("TenantId"))
                {
                    CurrentViewContext.TenantId = Convert.ToInt32(args["TenantId"]);
                }
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}
