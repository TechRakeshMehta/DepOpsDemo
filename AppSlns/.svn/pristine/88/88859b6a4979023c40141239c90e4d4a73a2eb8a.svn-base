#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using System.Text;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Threading;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.Globalization;
using System.Globalization;
using INTSOF.UI.Contract.BkgSetup;
#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class PendingOrder : BaseUserControl, IPendingOrderView
    {
        #region Variables

        #region Private Variables

        private ApplicantOrderCart applicantOrderCart;
        private PendingOrderPresenter _presenter = new PendingOrderPresenter();
        private String _viewType = null;
        private Int32 _defaultNodeId = 0;

        List<BundleData> _lstBundlePackageData = new List<BundleData>();
        List<BundleData> lstExclusiveBundlePackageData = new List<BundleData>();
        List<BundleData> lstNonExclusiveBundlePackageData = new List<BundleData>();
        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties

        /// <summary>
        /// Used for 'ViewDetails' redirect management
        /// </summary>
        private String CompliancePackageType { get; set; }

        #endregion

        #region Public Properties

        public PendingOrderPresenter Presenter
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

        List<ServiceFeeItemRecordContract> IPendingOrderView.lstAdditionalServiceFeeOption
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get
            {
                if (ViewState["TenantId"] != null)
                    return (Int32)(ViewState["TenantId"]);
                return
                   Presenter.GetTenant();
            }
            set
            {
                if (ViewState["TenantId"] == null)
                    ViewState["TenantId"] = value;
            }
        }

        /// <summary>
        /// Maintains the Request type, even after the page postback
        /// </summary>
        public Boolean IsChangeSubscriptionRequest
        {
            get
            {
                if (ViewState["IsChangeSubscriptionRequest"].IsNotNull())
                    return (Boolean)(ViewState["IsChangeSubscriptionRequest"]);
                return false;
            }
            set
            {
                if (ViewState["IsChangeSubscriptionRequest"].IsNullOrEmpty())
                    ViewState["IsChangeSubscriptionRequest"] = value;
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

        public IPendingOrderView CurrentViewContext
        {
            get { return this; }
        }

        public List<Program> ProgramsList
        {
            set
            {
                //chkListProgram.DataSource = value;
                //chkListProgram.DataBind();
                //cmbProgram.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            }
        }

        public List<DeptProgramPackageSubscription> lstDeptProgramPackageSubscription
        {
            get;
            set;
        }

        public List<Entity.Organization> Departments
        {
            set
            {
                //cmbDepartment.DataSource = value;
                //cmbDepartment.DataBind();
                //cmbDepartment.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            }
        }

        public List<Int32> SelectedDepProgramMappingId
        {
            get;
            set;
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

        public Int32 SelectedDepartmentId
        {
            get;
            set;
        }

        public List<DeptProgramPackage> DeptProgramPackages
        {
            get;
            set;
        }

        public DeptProgramPackageSubscription SelectedDeptProgramPackageSubscription
        {
            get
            {
                Presenter.GetDeptProgramPackageSubscription();
                WclComboBox cntrlDdlSubscriptions = (WclComboBox)FindControl("ddlSubscriptions" + controlSuffix);
                if (CurrentViewContext.lstDeptProgramPackageSubscription != null)
                    return CurrentViewContext.lstDeptProgramPackageSubscription
                        .FirstOrDefault(x => x.SubscriptionOption.SubscriptionOptionID.ToString() == cntrlDdlSubscriptions.SelectedValue);
                return null;
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

        public String InstitutionName
        {
            get;
            set;
        }

        public List<DeptProgramMapping> lstHierarchy
        {
            get;
            set;
        }

        public Int32 SelectedNodeId
        {
            get;
            set;
        }

        //public List<Int32> SelectedHierarchyNodeIds
        public Dictionary<Int32, Int32> SelectedHierarchyNodeIds
        {
            get;
            set;
        }

        public List<Int32> SelectedProgramIds
        {
            get;
            set;
        }

        public Boolean IsPackageSubscribe
        {
            get;
            set;
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


        public Boolean IsExecPasscodeMatched
        {
            get;
            set;
        }

        public Boolean IsNonExecPasscodeMatched
        {
            get;
            set;
        }

        public Int32 PackagePasscodeId
        {
            get;
            set;
        }


        public Int32 ChangeSubscriptionSourceNodeId
        {
            get
            {
                return ViewState["ChangeSubscriptionSourceNodeId"].IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ViewState["ChangeSubscriptionSourceNodeId"]);
            }
            set
            {
                ViewState["ChangeSubscriptionSourceNodeId"] = value;
            }
        }

        public String ChangeSubscriptionTargetNodeId
        {
            get;
            set;
        }

        public Int32 ChangeSubscriptionSourceNodeDPPId
        {
            get
            {
                return ViewState["ChangeSubscriptionSourceNodeDPPId"].IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ViewState["ChangeSubscriptionSourceNodeDPPId"]);
            }
            set
            {
                ViewState["ChangeSubscriptionSourceNodeDPPId"] = value;
            }
        }

        public Int32 ChangeSubscriptionCompliancePackageTypeId
        {
            get
            {
                return ViewState["ChangeSubscriptionCompliancePackageTypeId"].IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ViewState["ChangeSubscriptionCompliancePackageTypeId"]);
            }
            set
            {
                ViewState["ChangeSubscriptionCompliancePackageTypeId"] = value;
            }
        }

        //public List<CurrentNodePredecessors> lstCurrentNodeHierarchy
        //{
        //    get
        //    {
        //        return ViewState["lstCurrentNodeHierarchy"].IsNullOrEmpty() ? new List<CurrentNodePredecessors>()
        //            : (ViewState["lstCurrentNodeHierarchy"] as List<CurrentNodePredecessors>);
        //    }
        //    set
        //    {
        //        ViewState["lstCurrentNodeHierarchy"] = value;
        //    }
        //}

        public Boolean IfInvoiceOnlyPymnOptn
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IfInvoiceOnlyPymnOptn"])))
                    return (Boolean)ViewState["IfInvoiceOnlyPymnOptn"];
                return false;
            }
            set
            {
                ViewState["IfInvoiceOnlyPymnOptn"] = value;
            }
        }

        /// <summary>
        ///  hierarchy node from which package is selected.(in complio n ams complio is preferred , in case of ams last selected node is preferred.)
        /// </summary>
        public Int32 SelectedHierarchyNodeId
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["SelectedPackageNodeId"])))
                    return (Int32)ViewState["SelectedPackageNodeId"];
                return 0;
            }
            set
            {
                ViewState["SelectedPackageNodeId"] = value;
            }
        }

        //#region UAT-1214
        // UAT 1545: WB: Change to order package selection screen
        //public String RequiredPackageLabel
        //{
        //    set
        //    {
        //        spnRequiredPkgLabel.InnerText = value;
        //    }
        //}

        //public String OptionalPackageLabel
        //{
        //    set
        //    {
        //        spnOptionalPkgLabel.InnerText = value;
        //    }
        //}

        /// <summary>
        /// Label for Immnuization Package Section
        /// </summary>
        String IPendingOrderView.ImmnuizationPackageLabel
        {
            set
            {
                spnImmPkgLabel.InnerText = value;
            }
        }

        /// <summary>
        /// Label for Administrative Package Section
        /// </summary>
        String IPendingOrderView.AdministrativePackageLabel
        {
            set
            {
                spnAdmnPkgLabel.InnerText = value;
            }
        }
        #region UAT-3601
        /// <summary>
        /// Label for Screening package Header
        /// </summary>
        String IPendingOrderView.ScreeningHeaderLabel
        {
            set
            {
                divScreeningHeder.InnerText = value;
            }
        }

        public List<LookupContract> lstCBIUniqueIds
        {
            get
            {
                if (ViewState["lstCBIUniqueIds"] == null)
                {
                    ViewState["lstCBIUniqueIds"] = new List<LookupContract>();
                }
                return (List<LookupContract>)ViewState["lstCBIUniqueIds"];
            }
            set
            {
                ViewState["lstCBIUniqueIds"] = value;
            }
        }

        #endregion
        #region UAT-1560:WB: We should be able to add documents that need to be signed to the order process
        public Boolean IsAdditionalDocumentExist { get; set; }
        #endregion

        Dictionary<Int32, Int32> IPendingOrderView.lstLocationHierarchy
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["lstLocationHierarchy"])))
                    return (Dictionary<Int32, Int32>)ViewState["lstLocationHierarchy"];
                return new Dictionary<Int32, Int32>();
            }
            set
            {
                ViewState["lstLocationHierarchy"] = value;
            }
        }

        FingerPrintAppointmentContract IPendingOrderView.FingerPrintData
        {
            get
            {
                if (!SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_LOCATION_CART).IsNullOrEmpty())
                    return (FingerPrintAppointmentContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_LOCATION_CART);
                return new FingerPrintAppointmentContract();

            }
        }
        //#region CBI CABS
        //Boolean IPendingOrderView.IsLocationServiceTenant
        //{
        //    get
        //    {
        //        if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IsLocationServiceTenant"])))
        //            return (Boolean)ViewState["IsLocationServiceTenant"];
        //        return false;
        //    }
        //    set
        //    {
        //        ViewState["IsLocationServiceTenant"] = value;
        //    }
        //}
        //#endregion

        #endregion

        #region UAT-729 WB: As an applicant, if I have an active Compliance package, that package should not appear as an option in the order process.

        /// <summary>
        /// Property that used to set and get the already purchased package name
        /// </summary>
        public String AlreadyPurchasedPackages { get; set; }


        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>
        public String OrderType
        {
            get
            {
                if (!ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE].IsNullOrEmpty())
                    return Convert.ToString(ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE]);

                return String.Empty;
            }
            set
            {
                ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE] = value;
            }
        }


        private ViewCompliancePackage getCompliancePackage()
        {
            Dictionary<string, ViewCompliancePackage> cp = (Dictionary<string, ViewCompliancePackage>)ViewState["CompliancePackages"];
            if (cp == null)
                cp = new Dictionary<string, ViewCompliancePackage>();

            if (cp.Count == 0)
            {
                cp.Add(string.IsNullOrEmpty(CurrentCompliancePackageType) ? string.Empty : CurrentCompliancePackageType, new ViewCompliancePackage());
                cp.Values.First().DeptProgramPackage = new Entity.ClientEntity.DeptProgramPackage();
                CompliancePackages = cp;
                return cp.Values.First();
            }
            if (string.IsNullOrEmpty(CurrentCompliancePackageType))
                return cp.Values.First();

            if (cp.Keys.Contains(CurrentCompliancePackageType))
                return cp[CurrentCompliancePackageType];
            else
            {
                cp.Add(CurrentCompliancePackageType, new ViewCompliancePackage());
                cp[CurrentCompliancePackageType].DeptProgramPackage = new Entity.ClientEntity.DeptProgramPackage();
                CompliancePackages = cp;
                return cp[CurrentCompliancePackageType];
            }

        }

        public List<string> AvailableComplaincePackageTypes
        {
            get
            {
                if (ViewState["AvailableComplaincePackageTypes"] != null)
                    return (List<string>)ViewState["AvailableComplaincePackageTypes"];
                return new List<string>();
            }
            set
            {
                ViewState["AvailableComplaincePackageTypes"] = value;
            }
        }

        public String CurrentCompliancePackageType
        {
            get
            {
                if (ViewState["CurrentCompliancePackageType"] != null)
                    return ViewState["CurrentCompliancePackageType"].ToString();
                return null;
            }
            set
            {
                ViewState["CurrentCompliancePackageType"] = value;
            }
        }

        string controlSuffix { get { return (CurrentCompliancePackageType.Equals(CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue()) || string.IsNullOrEmpty(CurrentCompliancePackageType) ? "" : "_" + CurrentCompliancePackageType); } }

        public Dictionary<string, ViewCompliancePackage> CompliancePackages
        {
            get
            {
                if (ViewState["CompliancePackages"] != null)
                    return (Dictionary<string, ViewCompliancePackage>)ViewState["CompliancePackages"];
                return null;
            }
            set
            {
                ViewState["CompliancePackages"] = value;
            }
        }

        public DeptProgramPackage DeptProgramPackage
        {
            get
            {
                ViewCompliancePackage cp = getCompliancePackage();
                return cp.DeptProgramPackage;
            }
            set
            {
                ViewCompliancePackage cp = getCompliancePackage();
                cp.DeptProgramPackage = value;
            }
        }

        public Int32? ProgramDuration
        {
            get
            {
                ViewCompliancePackage cp = getCompliancePackage();
                return cp.ProgramDuration;
            }
            set
            {
                ViewCompliancePackage cp = getCompliancePackage();
                cp.ProgramDuration = value;
            }
        }

        public Int32? DPP_Id
        {
            get
            {
                ViewCompliancePackage cp = getCompliancePackage();
                return cp.DPP_Id;
            }
            set
            {
                ViewCompliancePackage cp = getCompliancePackage();
                cp.DPP_Id = value;
            }
        }
        public Int32 PreviousOrderId
        {
            get
            {
                return ViewState["PreviousOrderId"].IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ViewState["PreviousOrderId"]);
            }
            set
            {
                ViewState["PreviousOrderId"] = value;
            }

        }

        public Decimal SettlementPrice
        {
            get
            {
                return ViewState["SettlementPrice"].IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ViewState["SettlementPrice"]);
            }
            set
            {
                ViewState["SettlementPrice"] = value;
            }

        }

        List<PackageBundle> IPendingOrderView.lstPackageBundle
        {
            get;
            set;
        }

        //Commented in UAT-3283
        //Int32 IPendingOrderView.SelectedPkgBundleId
        //{
        //    get;
        //    set;
        //    ////get
        //    ////{
        //    ////    if (!cmbPackageBundle.SelectedValue.IsNullOrEmpty())
        //    ////        return Convert.ToInt32(cmbPackageBundle.SelectedValue);
        //    ////    return AppConsts.NONE;
        //    ////}

        //    ////set
        //    ////{
        //    ////    cmbPackageBundle.SelectedValue = value.ToString();
        //    ////}
        //}

        List<Int32> IPendingOrderView.lstSelectedBundlePkgId
        {
            get;
            set;
        }
        //END UAT-3283

        /// <summary>
        /// List of Department Program Packages in a Bundle
        /// </summary>
        List<PackageBundleNodePackage> IPendingOrderView.lstBundleDeptProgramPackages
        {
            get;
            set;
        }

        /// <summary>
        /// List of Background Packages in a Bundle
        /// </summary>
        List<PackageBundleNodePackage> IPendingOrderView.lstBundleBkgPackages
        {
            get;
            set;
        }

        #endregion

        public string ServiceDescription
        {
            get;
            set;
        }
        #region Language Translation
        public String LanguageCode
        {
            get
            {
                LanguageContract langContract = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!langContract.IsNullOrEmpty())
                {
                    return langContract.LanguageCode;
                }
                return Languages.ENGLISH.GetStringValue();
            }
        }
        #endregion

        public KeyValuePair<string, string> CBIBillingCode
        {
            get
            {
                if (ViewState["CBIBillingCode"] == null)
                {
                    ViewState["CBIBillingCode"] = new KeyValuePair<string, string>("", "");
                }
                return (KeyValuePair<string, string>)ViewState["CBIBillingCode"];
            }
            set
            {
                ViewState["CBIBillingCode"] = value;
            }
        }

        string IPendingOrderView.BillingCode
        {
            get
            {
                return txtCBIBillingCode.Text.Trim();
            }
            set
            {
                txtCBIBillingCode.Text = value;
            }
        }

        Decimal IPendingOrderView.BillingCodeAmount
        {
            get
            {
                CBIBillingStatu cbiBillingStatusData = new CBIBillingStatu();
                cbiBillingStatusData = Presenter.GetCBIBillingStatusData();
                if (!cbiBillingStatusData.IsNullOrEmpty())
                    return !cbiBillingStatusData.CBS_Amount.IsNullOrEmpty() && cbiBillingStatusData.CBS_Amount > AppConsts.NONE ? Convert.ToDecimal(cbiBillingStatusData.CBS_Amount) : AppConsts.NONE;
                return AppConsts.NONE;
            }
        }


        public List<BackgroundPackagesContract> lstBackgroundPackages
        {
            get
            {
                if (ViewState["lstBackgroundPackages"] != null)
                    return (List<BackgroundPackagesContract>)ViewState["lstBackgroundPackages"];
                return new List<BackgroundPackagesContract>();
            }
            set
            {
                ViewState["lstBackgroundPackages"] = value;
            }
        }


        //public List<string> AvailableComplaincePackageTypes
        //{
        //    get
        //    {
        //        if (ViewState["AvailableComplaincePackageTypes"] != null)
        //            return (List<string>)ViewState["AvailableComplaincePackageTypes"];
        //        return new List<string>();
        //    }
        //    set
        //    {
        //        ViewState["AvailableComplaincePackageTypes"] = value;
        //    }
        //}

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = Resources.Language.ORDER;
                //base.SetPageTitle("Order");
                CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault basePage = base.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault;
                basePage.SetModuleTitle(Resources.Language.CREATODR);
                hdnLanguageCode.Value = CurrentViewContext.LanguageCode;

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
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];            
            if (!this.IsPostBack)
            {
                //EnableLoadPackages(false);
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);

                CurrentViewContext.TenantId = Presenter.GetTenant();
                lblInstitutionName.Text = CurrentViewContext.InstitutionName;

                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                {
                    Presenter.GetAdditionalServiceFeeOption();
                }

                if (applicantOrderCart != null && (!CurrentViewContext.FingerPrintData.IsLocationServiceTenant || applicantOrderCart.SelectedHierarchyNodeID > AppConsts.NONE))
                {

                    //UAT 3573
                    if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant && !applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.FingerPrintData.IsNullOrEmpty() && CurrentViewContext.FingerPrintData.LocationId > AppConsts.NONE && CurrentViewContext.FingerPrintData.LocationId != applicantOrderCart.FingerPrintData.LocationId)
                    {
                        applicantOrderCart = new ApplicantOrderCart();
                    }

                    RedirectIfIncorrectOrderStage(applicantOrderCart);
                    CurrentViewContext.OrderType = applicantOrderCart.OrderRequestType;

                    if (Convert.ToString(applicantOrderCart.OrderRequestType) == OrderRequestType.ChangeSubscription.GetStringValue())
                    {
                        CurrentViewContext.PreviousOrderId = applicantOrderCart.PrevOrderId;
                        CurrentViewContext.SettlementPrice = applicantOrderCart.SettleAmount;
                        _presenter.GetChangeSubscriptionSourceNodeId();
                        this.IsChangeSubscriptionRequest = true;
                    }
                    IfInvoiceOnlyPymnOptn = applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel;
                }
                else
                {                    
                    ShowHideSubscriptionContinueButton(false);
                    btnGo.Enabled = true;
                }
                _presenter.OnViewInitialized();

                if (Request.QueryString["DPP_Id"] != null)
                    DPP_Id = Convert.ToInt32(Request.QueryString["DPP_Id"]);

                BindHierarchyNode(cmbLevel1, litLevel1, divNode1Literal, 1, rfvLevel1);
                if (!IsFreshOrder(applicantOrderCart)) // This condition is satisfied when user navigates from package details as well as from changen order
                {
                    /*CurrentViewContext.NodeId = applicantOrderCart.NodeId;*/
                    if (!CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                    {
                        ReBindHierarchy();
                    }
                    SetSelectedHierarchyData();
                    //LoadPackages();

                    // If package bundle is not selected then only bind packages
                    if (!this.IsChangeSubscriptionRequest)
                    {
                        BindBackgroundPackages();
                    }
                    // Bind Compliance package only if they were selected, when View Details was selected from Background packages
                    BindCompliancePackages();

                    BindPackageBundle();

                    //STart UAT-3283

                    //UAT-1200: if not change subscription request then bind bundles and packages on basis of bundle.
                    //if (!this.IsChangeSubscriptionRequest && applicantOrderCart.SelectedPkgBundleId.IsNotNull())
                    //{
                    //    CurrentViewContext.SelectedPkgBundleId = applicantOrderCart.SelectedPkgBundleId.Value;
                    //    ////cmbPackageBundle_SelectedIndexChanged(cmbPackageBundle, null);
                    //}
                    //Commented Under UAT-3283

                    if (!this.IsChangeSubscriptionRequest && applicantOrderCart.lstSelectedPkgBundleId.IsNotNull())
                    {
                        CurrentViewContext.lstSelectedBundlePkgId = applicantOrderCart.lstSelectedPkgBundleId;
                        ////cmbPackageBundle_SelectedIndexChanged(cmbPackageBundle, null);
                    }

                    //END UAT-3283

                    ////if (AvailableComplaincePackageTypes.IsNotNull())
                    ////{
                    ////    foreach (string cptype in AvailableComplaincePackageTypes)
                    ////    {
                    ////        CurrentCompliancePackageType = cptype;
                    ////        if (applicantOrderCart.IsNotNull())
                    ////            applicantOrderCart.CurrentCompliancePackageType = cptype;
                    ////        if (!applicantOrderCart.DPP_Id.IsNullOrEmpty() && applicantOrderCart.IsCompliancePackageSelected)
                    ////        {
                    ////            WclButton ctrlBtnViewDetails = (WclButton)FindControl("btnViewDetails" + controlSuffix);
                    ////            if (ctrlBtnViewDetails.IsNotNull())
                    ////                ctrlBtnViewDetails.Visible = true;
                    ////        }
                    ////        SetSelectedPackage();
                    ////    }
                    ////}
                    ShowHideSubscriptionContinueButton(true);
                    if (!this.IsChangeSubscriptionRequest)
                    {
                        dvOrderTotal.Visible = true;
                    }

                    RepeaterItem selectedBundle = null;
                    RepeaterItem selectedExclusivBundle = null;
                    if (divBundles.Visible)
                    {
                        foreach (RepeaterItem repeaterItem in rptBundles.Items)
                        {
                            var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfBundleId") as HiddenField).Value);

                            // If the Bundle is Selected and ID is equal to the one added in Cart, before calling this function.
                            if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked && !applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty() && applicantOrderCart.lstSelectedPkgBundleId.Contains(_bundleId)) //UAT-3283
                            //if ((repeaterItem.FindControl("rbtnBundle") as RadioButton).Checked && _bundleId == applicantOrderCart.SelectedPkgBundleId)
                            {
                                selectedBundle = repeaterItem;

                            }

                        }
                        //UAT 3775 Ability to make Bundle packages exclusive (like screening packages)
                        foreach (RepeaterItem repeaterItem in rptBundlesExclusive.Items)
                        {
                            var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfExcBundleId") as HiddenField).Value);

                            //If the Bundle is Selected and ID is equal to the one added in Cart, before calling this function.
                            //if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked && !applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty() && applicantOrderCart.lstSelectedPkgBundleId.Contains(_bundleId)) //UAT-3283
                            if ((repeaterItem.FindControl("rbtnExclusiveBundle") as RadioButton).Checked && !applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty() && applicantOrderCart.lstSelectedPkgBundleId.Contains(_bundleId))
                            {
                                selectedExclusivBundle = repeaterItem;
                            }

                        }
                        if (selectedExclusivBundle.IsNotNull())
                        {
                            Tuple<decimal, Boolean> bundleCost = CalculateExclusiveBundleCost(selectedExclusivBundle);
                            Tuple<decimal, Boolean> trackingCost = CalculateTrackingCost();
                            Tuple<decimal, Boolean> screeningCost = CalculateScreeningCost(); ;
                            DisplayOrderCost(bundleCost.Item1, trackingCost.Item1, screeningCost.Item1, bundleCost.Item2, trackingCost.Item2, screeningCost.Item2);
                        }

                    }
                    if (selectedBundle.IsNotNull())
                    {
                        Tuple<decimal, Boolean> bundleCost = CalculateBundleCost(selectedBundle);
                        Tuple<decimal, Boolean> trackingCost = CalculateTrackingCost();
                        Tuple<decimal, Boolean> screeningCost = CalculateScreeningCost(); ;
                        DisplayOrderCost(bundleCost.Item1, trackingCost.Item1, screeningCost.Item1, bundleCost.Item2, trackingCost.Item2, screeningCost.Item2);
                    }
                    else
                    {

                        Tuple<decimal, Boolean> trackingCost = CalculateTrackingCost();
                        Tuple<decimal, Boolean> screeningCost = CalculateScreeningCost(); ;
                        DisplayOrderCost(AppConsts.NONE, trackingCost.Item1, screeningCost.Item1, false, trackingCost.Item2, screeningCost.Item2);
                    }
                    if (!this.IsChangeSubscriptionRequest)
                    {
                        dvTrackingTotal.Visible = true;
                    }
                }
                else
                {
                    ShowHidePackages(false);
                }
                //hHelpText.Visible = false; //UAT-3253
                CheckIsOrderFlowMessageSettingEnable();  //UAT - 2802

                //UAT 3521
                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                {
                    Presenter.GetLocationHierarchy();
                    Presenter.GetServiceDescription();
                    CaptureQueryString();
                    if (CurrentViewContext.lstLocationHierarchy.Count > AppConsts.NONE)
                    {
                        cmbLevel1.SelectedValue = CurrentViewContext.lstLocationHierarchy[1].ToString();
                        cmbLevel1_SelectedIndexChanged(null, null);
                        cmbLevel1.Enabled = false;
                        //btnLoadPackages_Click(null, null);
                        ShowHidePackageBundle(false);
                        LoadPackages();
                        //if (CurrentViewContext.FingerPrintData.IsLocationType)
                        //{
                        //    GetPreviousOrderHistory();
                        //}
                        cmbLevel1.Enabled = false;
                        cmbLevel2.Enabled = false;
                        cmbLevel3.Enabled = false;
                        fsucCmdBar1.ExtraButton.Enabled = false;
                        btnGo.Visible = false;
                        //h1.InnerText = "If you are not sure which service to order, please contact your employer or requesting agency for assistance.";
                        h1.InnerText = Resources.Language.CONTACTEMPLOYEE;

                        dvCBIUnique.Visible = (!CurrentViewContext.FingerPrintData.IsLocationServiceTenant || (CurrentViewContext.FingerPrintData.IsOutOfState || CurrentViewContext.FingerPrintData.IsEventCode));
                        
                        if(applicantOrderCart.IsNotNull() &&
                           applicantOrderCart.lstApplicantOrder.IsNotNull() &&
                           applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNotNull() &&
                           applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(package=>package.ServiceCode == "AAAA"))
                        {
                            if (!CurrentViewContext.FingerPrintData.CBIUniqueID.IsNullOrEmpty())
                            {
                                txtCBIUniqueID.Text = CurrentViewContext.FingerPrintData.CBIUniqueID;
                                if (!txtCBIUniqueID.Text.IsNullOrEmpty() && CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                                {
                                    dvCBIUnique.Visible = true;
                                }
                            }
                            else
                            {
                                if (applicantOrderCart.IsNotNull() && !applicantOrderCart.FingerPrintData.IsNullOrEmpty())
                                {
                                    txtCBIUniqueID.Text = applicantOrderCart.FingerPrintData.CBIUniqueID;

                                }
                            }
                        }
                        if (lstBackgroundPackages.IsNotNull() && lstBackgroundPackages.Count() == 1 && lstBackgroundPackages.Any(x => x.ServiceCode == BkgServiceType.SIMPLE.GetStringValue()))
                        {
                            dvCBIUnique.Visible = true;
                        }
                        if (!CurrentViewContext.FingerPrintData.IsConsent.IsNullOrEmpty())
                        {
                            chkIsConsent.Checked = CurrentViewContext.FingerPrintData.IsConsent;
                        }
                        else
                        {
                            if (applicantOrderCart.IsNotNull() && !applicantOrderCart.FingerPrintData.IsNullOrEmpty())
                            {
                                chkIsConsent.Checked = applicantOrderCart.FingerPrintData.IsConsent;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(CurrentViewContext.FingerPrintData.BillingCode))
                        {
                            dvCBIBillingCode.Visible = true;
                            CurrentViewContext.BillingCode = CurrentViewContext.FingerPrintData.BillingCode;
                        }
                        //if (!CurrentViewContext.FingerPrintData.AcctNameOrAcctNumber.IsNullOrEmpty())
                        //{
                        //    txtAcctNameOrAcctNumber.Text = CurrentViewContext.FingerPrintData.AcctNameOrAcctNumber;
                        //    if (lstCBIUniqueIds.IsNullOrEmpty())
                        //    {
                        //        lstCBIUniqueIds = Presenter.GetCBIUniqueIdByAcctNameOrNumber(txtAcctNameOrAcctNumber.Text);
                        //    }
                        //    cmbCbiUniqueIds.DataSource = CurrentViewContext.FingerPrintData.lstCBIUniqueIds;
                        //    cmbCbiUniqueIds.DataBind();
                        //    cmbCbiUniqueIds.SelectedValue = CurrentViewContext.FingerPrintData.SelectedCBIUniqueId;
                        //    if (CurrentViewContext.FingerPrintData.CBIUniqueID == CurrentViewContext.FingerPrintData.SelectedCBIUniqueId)
                        //    {
                        //        cmbCbiUniqueIds.Text = CurrentViewContext.FingerPrintData.CBIUniqueID;
                        //    }
                        //}
                    }
                    else
                    {
                        base.ShowInfoMessage("No Hierarchy is mapped with selected Location");
                    }                    
                }

            }
            _presenter.OnViewLoaded();
            /*ShowHideCompliancePackage();*/

            //UAT 3521
            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {               
                if (CurrentViewContext.FingerPrintData.IsEventCode
                    || CurrentViewContext.FingerPrintData.IsOutOfState || CurrentViewContext.FingerPrintData.IsFromArchivedOrderScreen)
                    base.SetPageTitle("(" + Resources.Language.STEP + " 2)"); //// 4331 : change schedule appointment to step 2 of order flow
                else
                    base.SetPageTitle("(" + Resources.Language.STEP + " 3)"); //// 4331 : change schedule appointment to step 2 of order flow
                //divScreeningHeder.InnerText = "Order Selections";
            }
            else
            {
                base.SetPageTitle(" (" + Resources.Language.STEP + " 1)");
                //divScreeningHeder.InnerText = "Screening";
            }            

            CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault basePage = base.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault;
            basePage.SetModuleTitle(Resources.Language.CREATODR);
            //Hide the rush order service check box on the basis of client setting for Rush Order.
            ////if (ShowRushOrder)
            ////{
            ////    dvRushOrder.Visible = false;
            ////}
            ////else
            ////{
            ////    dvRushOrder.Visible = false;
            ////}

            //cmdBar.SaveButton.ToolTip = "View detailed requirements associated with this subscription package";
            //cmdBar.SubmitButton.ToolTip = "Continue to the next step";
            cmdBar.SubmitButton.ToolTip = Resources.Language.CONTINUENXTSTP;
            SetButtonText();
            //(this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle("Orders");
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RegisterbtnSubscriptionsImmSelectedIndexChangedEvent();", true);
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Loads the Packages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLoadPackages_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    ShowHidePackageBundle(false);
                    LoadPackages();
                    Session["NonExclusiveBundleCost"] = null;
                    Session["ExclusiveBundleCost"] = null;
                    // System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RegisterbtnSubscriptionsImmSelectedIndexChangedEvent();", true);
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
        /// Load the Compliance Packages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBarBackgroundPackages_SubmitClick(object sender, EventArgs e)
        {
            Boolean _isExclusiveSelected = false;
            foreach (var repeaterItem in rptExclusive.Items)
            {
                RadioButton _rbtn = ((repeaterItem as RepeaterItem).FindControl("rbtnExclusive") as RadioButton);
                if (_rbtn.Checked)
                {
                    _isExclusiveSelected = true;
                    break;
                }
            }
            if (_isExclusiveSelected || !pnlExclusiveBkgPackages.Visible)
            {
                // Start order, if subscription of Compliance Package is not displayed selected
                // LoadCompliancePackage();
                Boolean _isCompliancePackageAvailable = BindCompliancePackages();
                //if (!divCompliancePackage.Visible)
                if (!_isCompliancePackageAvailable)
                {
                    // Take to next step
                    StartOrder();
                }
                else
                {
                    foreach (string cpType in AvailableComplaincePackageTypes)
                    {
                        CurrentCompliancePackageType = cpType;
                        BindPackageDetails();
                    }
                    cmdBarBackgroundPackages.SubmitButton.Enabled = false;
                    ShowHideSubscriptionContinueButton(true);
                }
            }
            else
                base.ShowInfoMessage("Please select an Exclusive Package from the listing.");
        }

        /// <summary>
        /// Start generating the order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBarStartOrder_Click(object sender, EventArgs e)
        {
            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                CurrentViewContext.FingerPrintData.IsConsent = chkIsConsent.Checked;
                CurrentViewContext.FingerPrintData.CBIUniqueID = txtCBIUniqueID.Text.Trim().ToUpper();
                CurrentViewContext.FingerPrintData.BillingCode = "";
                Boolean IsValidCBIUniqueID = CurrentViewContext.CBIBillingCode.Key == CurrentViewContext.FingerPrintData.CBIUniqueID
                    || Presenter.ValidateCBIUniqueID();

                foreach (RepeaterItem item in rptNonExclusive.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        Label lblServiceCode = item.FindControl("lblServiceCode") as Label;
                        CheckBox _rChkNonExclusive = (item.FindControl("chkNonExc") as CheckBox);
                        TextBox txtNoOfCopies = (item.FindControl("txtNoOfCopies") as TextBox);
                        
                        if (lblServiceCode.Text == BkgServiceType.SIMPLE.GetStringValue() && _rChkNonExclusive.Checked)
                        {
                            if (!IsValidCBIUniqueID)
                            {
                                //base.ShowInfoMessage("Invalid CBI Unique ID. Please contact your administrator.");
                                base.ShowInfoMessage(Resources.Language.INVALIDCBIID);
                                return;
                            }
                        }

                    }
                }               

                if (!string.IsNullOrWhiteSpace(CBIBillingCode.Value)
                    && !dvCBIBillingCode.Visible)
                {
                    dvCBIBillingCode.Visible = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(CBIBillingCode.Value))
                {
                    if (dvCBIBillingCode.Visible && !string.IsNullOrWhiteSpace(txtCBIBillingCode.Text))
                    {
                        base.ShowInfoMessage("This CBI Unique ID doesn't support Billing Code. Click \"Next\" to continue with this CBI Unique ID.");
                        HideBillingCodeSection();
                        return;
                    }
                    HideBillingCodeSection();
                }

                if (CurrentViewContext.FingerPrintData.CBIUniqueID == CBIBillingCode.Key
                    && !(string.IsNullOrWhiteSpace(CurrentViewContext.BillingCode)
                    || CurrentViewContext.BillingCode.ToLowerInvariant() == CBIBillingCode.Value.ToLowerInvariant()))
                {
                    //base.ShowInfoMessage("Invalid Billing Code. Please contact your administrator.");
                    base.ShowInfoMessage(Resources.Language.INVALIDBILLINGCODE);
                    //base.ShowInfoMessage(Resources.Language.INVALIDCBIID);
                    return;
                }
                CurrentViewContext.FingerPrintData.BillingCode = CurrentViewContext.BillingCode;
                //UAT-3850
                CurrentViewContext.FingerPrintData.BillingCodeAmount = CurrentViewContext.BillingCodeAmount;
                //CurrentViewContext.FingerPrintData.AcctNameOrAcctNumber = txtAcctNameOrAcctNumber.Text;
                //CurrentViewContext.FingerPrintData.lstCBIUniqueIds = lstCBIUniqueIds;
                CurrentViewContext.FingerPrintData.SelectedCBIUniqueId = cmbCbiUniqueIds.SelectedValue;
            }
            //UAT=2802
            if (!this.IsChangeSubscriptionRequest)
            {
                if (hdfPackageOrderConfirmationsttings.Value == "True")
                {
                    SetSelectedHierarchyData();
                    var nodeIdsCount = CurrentViewContext.SelectedHierarchyNodeIds.Count;

                    Boolean IsExistingSelect = _presenter.IsExistingNodeSelected(CurrentViewContext.SelectedHierarchyNodeIds[nodeIdsCount - 1]);
                    if (!IsExistingSelect)
                    {
                        applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                        if (applicantOrderCart == null)
                        {
                            applicantOrderCart = new ApplicantOrderCart();
                            applicantOrderCart.GetApplicantOrder();
                        }
                        if (hdfIsConfirm.Value == "1" || applicantOrderCart.IsOrderFlowConfirmation)
                        {
                            if (hdfIsMessageAcknowledged.Value == "1")
                            {
                                applicantOrderCart.IsOrderFlowConfirmation = true;
                                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
                                StartOrder();
                            }
                            else
                            {
                                AcknowledgeMessagePopUpBind();
                            }
                        }
                        else
                        {
                            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                            {
                                hdfIsConfirm.Value = "1";
                                cmdBarStartOrder_Click(sender, e);
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CheckPackageConfirmationSettings();", true);
                                return;
                            }

                        }
                    }
                    else
                    {
                        if (hdfIsMessageAcknowledged.Value == "1")
                        {
                            StartOrder();
                            hdfIsConfirm.Value = "0";
                            hdfIsMessageAcknowledged.Value = "0";
                        }
                        else
                        {
                            AcknowledgeMessagePopUpBind();
                        }
                    }
                }
                else
                {
                    if (hdfIsMessageAcknowledged.Value == "1")
                    {
                        StartOrder();
                        hdfIsConfirm.Value = "0";
                        hdfIsMessageAcknowledged.Value = "0";
                    }
                    else
                    {
                        AcknowledgeMessagePopUpBind();
                    }
                }
            }
            else
            {
                if (hdfIsMessageAcknowledged.Value == "1")
                {
                    StartOrder();
                    hdfIsMessageAcknowledged.Value = "0";
                }
                else
                {
                    AcknowledgeMessagePopUpBind();
                }
            }
        }

        private void HideBillingCodeSection()
        {
            CurrentViewContext.BillingCode = "";
            dvCBIBillingCode.Visible = false;
        }

        /// <summary>
        /// View Details of Compliance Package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            SetCurentComplianceTypeByControlID(((WclButton)sender).ID);
            RedirectToCompliancePackageDetails(DeptProgramPackage.DPP_CompliancePackageID);
        }

        /// <summary>
        /// Represents the Cancel event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                FingerPrintAppointmentContract fingerPrintData = new FingerPrintAppointmentContract();
                if (!applicantOrderCart.IsNullOrEmpty())
                {
                    if (!applicantOrderCart.IsLocationServiceTenant || applicantOrderCart.OrderRequestType != OrderRequestType.NewOrder.GetStringValue())
                    {
                        fingerPrintData = applicantOrderCart.FingerPrintData;
                        Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                    }
                    if (!applicantOrderCart.FingerPrintData.IsNullOrEmpty() && (applicantOrderCart.FingerPrintData.IsOutOfState || applicantOrderCart.FingerPrintData.IsEventCode))
                    {
                        Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                    }
                }

                Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
                Session.Remove(AppConsts.DISCLOSURE_ACCEPTED);
                //UAT-1560
                Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
                Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
                Dictionary<String, String> queryString;

                var _navigationFrom = String.Empty;

                // If its a new order OR applicant landed from the Change subscription page
                if (applicantOrderCart.IsNullOrEmpty() || applicantOrderCart.PendingOrderNavigationFrom.IsNullOrEmpty())
                    _navigationFrom = GetNavigationFrom();
                else
                    _navigationFrom = applicantOrderCart.PendingOrderNavigationFrom;

                if (applicantOrderCart.IsNotNull() && Convert.ToString(applicantOrderCart.OrderRequestType) == OrderRequestType.ChangeSubscription.GetStringValue()
                    && _navigationFrom == PendingOrderNavigationFrom.ApplicantChangeSubscription.GetStringValue())
                {
                    //change done for UAt-827 Applicant Dashboard Redesign.
                    if (applicantOrderCart.ParentControlType == AppConsts.DASHBOARD)
                    {
                        Response.Redirect(AppConsts.DASHBOARD_URL);
                    }
                    else
                    {
                        queryString = new Dictionary<String, String> { { AppConsts.CHILD, ChildControls.PackageSubscription } };
                        Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                    }
                }
                else if (_navigationFrom == PendingOrderNavigationFrom.ApplicantLandingPage.GetStringValue())
                {
                    queryString = new Dictionary<String, String> { { AppConsts.CHILD, AppConsts.APPLICANT_LANDING_PAGE_CONTROL_NAME } };
                    Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
                else if (_navigationFrom == PendingOrderNavigationFrom.ApplicantDashboard.GetStringValue())
                {
                    Response.Redirect("~/Main/Default.aspx");
                }
                else if (_navigationFrom == PendingOrderNavigationFrom.FingerPrintDataControl.GetStringValue())
                {
                    queryString = new Dictionary<String, String> { { AppConsts.CHILD, AppConsts.FINGER_PRINTDATA_CONTROL },
                                                                   { "OrderTypeCode",OrderRequestType.NewOrder.GetStringValue() },
                                                                   {"TenantId",TenantId.ToString()}
                                                                 };
                    Response.Redirect(String.Format("~/FingerPrintSetUp/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
                else if (_navigationFrom == PendingOrderNavigationFrom.ScheduleApplicantAppointment.GetStringValue())
                {
                    if (!fingerPrintData.IsNullOrEmpty() && applicantOrderCart.OrderRequestType != OrderRequestType.NewOrder.GetStringValue()) //// 4331 : change schedule appointment to step 2 of order flow
                    {
                        var appTempOrderData = new ApplicantOrderCart();
                        appTempOrderData.FingerPrintData = new FingerPrintAppointmentContract();
                        appTempOrderData.FingerPrintData = fingerPrintData;
                        SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, appTempOrderData);
                    }


                    //queryString = new Dictionary<String, String> { { AppConsts.CHILD, AppConsts.FINGER_PRINTDATA_CONTROL }, //// 4331 : change schedule appointment to step 2 of order flow
                    queryString = new Dictionary<String, String> { { AppConsts.CHILD, ChildControls.APPLICANT_APPOINTMENT_SCHEDULE },//// 4331 : change schedule appointment to step 2 of order flow
                                                                       {"OrderTypeCode",OrderRequestType.NewOrder.GetStringValue() }, //// 4331 : change schedule appointment to step 2 of order flow
                                                                       {"TenantId",TenantId.ToString()}
                                                                 };
                    //{"SelectedLocationID",applicantOrderCart.FingerPrintData.LocationId.ToString()}};
                    Response.Redirect(String.Format("~/FingerPrintSetUp/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
                else if (_navigationFrom == PendingOrderNavigationFrom.ArchivedOrderForm.GetStringValue())
                {
                    queryString = new Dictionary<String, String> { { AppConsts.CHILD, ChildControls.ArchivedOrder },
                                                                   {"TenantId",TenantId.ToString()}
                                                                 };
                    Response.Redirect(String.Format("~/FingerPrintSetUp/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
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
        /// UAT-4271
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLookUp1_Click(Object sender, EventArgs e)
        {
            //List<String> lstCBIUniqueIds = new List<String>();        
            lstCBIUniqueIds = new List<LookupContract>();
            cmbCbiUniqueIds.Items.Clear();
            cmbCbiUniqueIds.Text = String.Empty;
            //txtCBIUniqueID.Text = String.Empty;
            if (txtAcctNameOrAcctNumber.Text != String.Empty)
            {
                lstCBIUniqueIds = Presenter.GetCBIUniqueIdByAcctNameOrNumber(txtAcctNameOrAcctNumber.Text);
                cmbCbiUniqueIds.DataSource = lstCBIUniqueIds;
                cmbCbiUniqueIds.DataBind();
                //cmbCbiUniqueIds.AddFirstEmptyItem();
            }
            else
            {
                RegularExpressionValidator1.IsValid = false;


                //RegularExpressionValidator1.ErrorMessage = Resources.Language.MINFOURCHAR;
            }
        }

        #endregion

        #region DropDown Events

        //protected void ddlSubscription_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        //{
        //    if (SelectedDeptProgramPackageSubscription != null)
        //    {
        //        SetPackageDetails(SelectedDeptProgramPackageSubscription, true);
        //        divSubscriptions.Visible = true;
        //    }
        //}

        protected void ddlSubscriptions_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ////SetCurentComplianceTypeByControlID(((WclComboBox)sender).ID);
            ////if (SelectedDeptProgramPackageSubscription != null)
            ////{
            ////    SetPackageDetails(SelectedDeptProgramPackageSubscription, true);
            ////    divSubscriptions.Visible = true;
            ////}
        }



        ///// <summary>
        ///// Package Selection List 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ddlDeptprogramPkg_SelectedIndexChange(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        //{
        //    SetSelectedHierarchyData();
        //    _presenter.GetDeptProgramPackage();

        //    ClearPackageSelectionDataFromCart();
        //    SetSelectedPackage();

        //    if (!String.IsNullOrEmpty(ddlDeptprogramPkg.SelectedValue) && ddlDeptprogramPkg.SelectedValue != AppConsts.ZERO)
        //    {
        //        if (CurrentViewContext.DeptProgramPackages.Where(cond => cond.DPP_ID == Convert.ToInt32(ddlDeptprogramPkg.SelectedValue))
        //            .Select(x => x.CompliancePackage.IsViewDetailsInOrderEnabled).FirstOrDefault())
        //        {
        //            btnViewDetails.Visible = true;
        //        }
        //        else
        //        {
        //            btnViewDetails.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        btnViewDetails.Visible = false;
        //    }
        //}


        protected void ddlDeptprogramPkg_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            SetCurentComplianceTypeByControlID(((WclComboBox)sender).ID);

            SetSelectedHierarchyData();
            _presenter.GetDeptProgramPackage();

            DisplayPackageDetails();
        }

        protected void cmbLevel1_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            NodeToManage(1);
            if (cmbLevel1.SelectedValue != AppConsts.ZERO && !String.IsNullOrEmpty(cmbLevel1.SelectedValue))
            {
                CurrentViewContext.SelectedNodeId = Convert.ToInt32(cmbLevel1.SelectedValue);
                Presenter.GetHierarchyNodes(false);
                BindHierarchyNode(cmbLevel2, litLevel2, divNode2Literal, 2, rfvLevel2);
            }
            btnGo.Enabled = true;
            ShowHidePackages(false);

            //UAT 3521
            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant && !CurrentViewContext.lstHierarchy.IsNullOrEmpty() && CurrentViewContext.lstLocationHierarchy.Count > AppConsts.NONE)
            {
                cmbLevel2.SelectedValue = CurrentViewContext.lstLocationHierarchy[2].ToString();
                cmbLevel2_SelectedIndexChanged(null, null);
                cmbLevel2.Enabled = false;
            }

            //hHelpText.Visible = false; //UAT-3253
        }
        protected void cmbLevel2_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            NodeToManage(2);
            if (cmbLevel2.SelectedValue != AppConsts.ZERO && !String.IsNullOrEmpty(cmbLevel2.SelectedValue))
            {
                CurrentViewContext.SelectedNodeId = Convert.ToInt32(cmbLevel2.SelectedValue);
                Presenter.GetHierarchyNodes(false);
                BindHierarchyNode(cmbLevel3, litLevel3, divNode3Literal, 3, rfvLevel3);

                //UAT 3521
                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant && !CurrentViewContext.lstHierarchy.IsNullOrEmpty() && CurrentViewContext.lstLocationHierarchy.Count > AppConsts.NONE)
                {
                    cmbLevel3.SelectedValue = CurrentViewContext.lstLocationHierarchy[3].ToString();
                    cmbLevel3_SelectedIndexChanged(null, null);
                    cmbLevel3.Enabled = false;
                }
            }
            btnGo.Enabled = true;
            ShowHidePackages(false);
            //hHelpText.Visible = false; //UAT-3253


        }
        protected void cmbLevel3_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            NodeToManage(3);
            if (cmbLevel3.SelectedValue != AppConsts.ZERO && !String.IsNullOrEmpty(cmbLevel3.SelectedValue))
            {
                CurrentViewContext.SelectedNodeId = Convert.ToInt32(cmbLevel3.SelectedValue);
                Presenter.GetHierarchyNodes(false);
                pnl2.Visible = true;
                BindHierarchyNode(cmbLevel4, litLevel4, divNode4Literal, 4, rfvLevel4);

                //UAT 3521
                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant && !CurrentViewContext.lstHierarchy.IsNullOrEmpty() && CurrentViewContext.lstLocationHierarchy.Count > AppConsts.NONE)
                {
                    cmbLevel4.SelectedValue = CurrentViewContext.lstLocationHierarchy[4].ToString();
                    cmbLevel4_SelectedIndexChanged(null, null);
                    cmbLevel4.Enabled = false;
                }
            }
            btnGo.Enabled = true;
            ShowHidePackages(false);
            // hHelpText.Visible = false; //UAT-3253



        }
        protected void cmbLevel4_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            NodeToManage(4);
            if (cmbLevel4.SelectedValue != AppConsts.ZERO && !String.IsNullOrEmpty(cmbLevel4.SelectedValue))
            {
                CurrentViewContext.SelectedNodeId = Convert.ToInt32(cmbLevel4.SelectedValue);
                Presenter.GetHierarchyNodes(false);
                BindHierarchyNode(cmbLevel5, litLevel5, divNode5Literal, 5, rfvLevel5);

                //UAT 3521
                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant && !CurrentViewContext.lstHierarchy.IsNullOrEmpty() && CurrentViewContext.lstLocationHierarchy.Count > AppConsts.NONE)
                {
                    cmbLevel5.SelectedValue = CurrentViewContext.lstLocationHierarchy[5].ToString();
                    cmbLevel5_SelectedIndexChanged(null, null);
                    cmbLevel5.Enabled = false;
                }
            }
            btnGo.Enabled = true;
            ShowHidePackages(false);
            //  hHelpText.Visible = false; //UAT-3253


        }
        protected void cmbLevel5_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            NodeToManage(5);
            if (cmbLevel5.SelectedValue != AppConsts.ZERO && !String.IsNullOrEmpty(cmbLevel5.SelectedValue))
            {
                CurrentViewContext.SelectedNodeId = Convert.ToInt32(cmbLevel5.SelectedValue);
                Presenter.GetHierarchyNodes(false);
                BindHierarchyNode(cmbLevel6, litLevel6, divNode6Literal, 6, rfvLevel6);

                //UAT 3521
                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant && !CurrentViewContext.lstHierarchy.IsNullOrEmpty() && CurrentViewContext.lstLocationHierarchy.Count > AppConsts.NONE)
                {
                    cmbLevel6.SelectedValue = CurrentViewContext.lstLocationHierarchy[6].ToString();
                    cmbLevel6_SelectedIndexChanged(null, null);
                    cmbLevel6.Enabled = false;
                }
            }
            btnGo.Enabled = true;
            ShowHidePackages(false);
            //  hHelpText.Visible = false; //UAT-3253


        }
        protected void cmbLevel6_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            NodeToManage(6);
            if (cmbLevel6.SelectedValue != AppConsts.ZERO && !String.IsNullOrEmpty(cmbLevel6.SelectedValue))
            {
                CurrentViewContext.SelectedNodeId = Convert.ToInt32(cmbLevel6.SelectedValue);
                Presenter.GetHierarchyNodes(false);
                BindHierarchyNode(cmbLevel7, litLevel7, divNode7Literal, 7, rfvLevel7);
                pnl3.Visible = true;

                //UAT 3521
                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant && !CurrentViewContext.lstHierarchy.IsNullOrEmpty() && CurrentViewContext.lstLocationHierarchy.Count > AppConsts.NONE)
                {
                    cmbLevel7.SelectedValue = CurrentViewContext.lstLocationHierarchy[7].ToString();
                    cmbLevel7_SelectedIndexChanged(null, null);
                    cmbLevel7.Enabled = false;
                }
            }
            btnGo.Enabled = true;
            ShowHidePackages(false);
            // hHelpText.Visible = false; //UAT-3253


        }
        protected void cmbLevel7_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            NodeToManage(7);
            if (cmbLevel7.SelectedValue != AppConsts.ZERO && !String.IsNullOrEmpty(cmbLevel7.SelectedValue))
            {
                CurrentViewContext.SelectedNodeId = Convert.ToInt32(cmbLevel7.SelectedValue);
                Presenter.GetHierarchyNodes(false);
                BindHierarchyNode(cmbLevel8, litLevel8, divNode8Literal, 8, rfvLevel8);

                //UAT 3521
                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant && !CurrentViewContext.lstHierarchy.IsNullOrEmpty() && CurrentViewContext.lstLocationHierarchy.Count > AppConsts.NONE)
                {
                    cmbLevel8.SelectedValue = CurrentViewContext.lstLocationHierarchy[8].ToString();
                    cmbLevel8_SelectedIndexChanged(null, null);
                    cmbLevel8.Enabled = false;
                }
            }
            btnGo.Enabled = true;
            ShowHidePackages(false);
            //  hHelpText.Visible = false; //UAT-3253


        }

        protected void cmbLevel8_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            NodeToManage(8);
            if (cmbLevel8.SelectedValue != AppConsts.ZERO && !String.IsNullOrEmpty(cmbLevel8.SelectedValue))
            {
                CurrentViewContext.SelectedNodeId = Convert.ToInt32(cmbLevel8.SelectedValue);
                Presenter.GetHierarchyNodes(false);
                BindHierarchyNode(cmbLevel9, litLevel9, divNode9Literal, 9, rfvLevel9);

                //UAT 3521
                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant && !CurrentViewContext.lstHierarchy.IsNullOrEmpty() && CurrentViewContext.lstLocationHierarchy.Count > AppConsts.NONE)
                {
                    cmbLevel9.SelectedValue = CurrentViewContext.lstLocationHierarchy[9].ToString();
                    cmbLevel9_SelectedIndexChanged(null, null);
                    cmbLevel9.Enabled = false;
                }
            }
            btnGo.Enabled = true;
            ShowHidePackages(false);
            //hHelpText.Visible = false; //UAT-3253


        }

        protected void cmbLevel9_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            NodeToManage(9);
            if (cmbLevel9.SelectedValue != AppConsts.ZERO && !String.IsNullOrEmpty(cmbLevel9.SelectedValue))
            {
                CurrentViewContext.SelectedNodeId = Convert.ToInt32(cmbLevel9.SelectedValue);
                Presenter.GetHierarchyNodes(false);
                BindHierarchyNode(cmbLevel10, litLevel10, divNode10Literal, 10, rfvLevel10);
                pnl4.Visible = true;

                //UAT 3521
                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant && !CurrentViewContext.lstHierarchy.IsNullOrEmpty() && CurrentViewContext.lstLocationHierarchy.Count > AppConsts.NONE)
                {
                    cmbLevel10.SelectedValue = CurrentViewContext.lstLocationHierarchy[9].ToString();
                    cmbLevel10_SelectedIndexChanged(null, null);
                    cmbLevel10.Enabled = false;
                }
            }
            btnGo.Enabled = true;
            ShowHidePackages(false);
            //hHelpText.Visible = false; //UAT-3253


        }

        protected void cmbLevel10_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            btnGo.Enabled = true;
            ShowHidePackages(false);
            //hHelpText.Visible = false;
        }


        protected void cmbPackageBundle_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ////if (cmbPackageBundle.SelectedValue == String.Empty)
                ////{
                ////    LoadPackages();
                ////    DisplayPkgBundleNotes();
                ////}
                ////else
                ////{
                ////    DisplayPkgBundleNotes();
                ////    //BindPackagesWithSelectedBundle();
                ////}
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

        protected void cmbCbiUniqueIds_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            txtCBIUniqueID.Text = cmbCbiUniqueIds.SelectedValue.ToString();
        }

        protected void cmbPackageBundle_DataBound(object sender, EventArgs e)
        {
            ////try
            ////{
            ////    cmbPackageBundle.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT));
            ////}
            ////catch (SysXException ex)
            ////{
            ////    base.LogError(ex);
            ////    base.ShowErrorMessage(ex.Message);
            ////}
            ////catch (System.Exception ex)
            ////{
            ////    base.LogError(ex);
            ////    base.ShowErrorMessage(ex.Message);
            ////}

        }

        #endregion

        #region Repeater Events

        protected void rptExclusivePackages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var _packageId = (e.Item.FindControl("hdnExcBPAId") as HiddenField).Value;
            RedirectToBackgroundPackageDetails(Convert.ToInt32(_packageId));
        }

        protected void rptNonExclusive_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var _packageId = (e.Item.FindControl("hdnNonExcBPAId") as HiddenField).Value;
            RedirectToBackgroundPackageDetails(Convert.ToInt32(_packageId));
        }

        protected void rptExclusive_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //for hiding the price if invoice is only payment option available.
            //if (applicantOrderCart.IsNotNull())
            //{
            //    if (applicantOrderCart.ifInvoiceIsOnlyPaymentOptionAvailable)
            //    {
            //        System.Web.UI.HtmlControls.HtmlGenericControl dvElement = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvElement");
            //        dvElement.Style.Add("display", "none");
            //    }
            //}
            //else
            //{
            //    if (IfInvoiceOnlyPymnOptn)
            //    {
            //        System.Web.UI.HtmlControls.HtmlGenericControl dvElement = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvElement");
            //        dvElement.Style.Add("display", "none");
            //    }
            //}
            System.Web.UI.HtmlControls.HtmlGenericControl dvElement = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvElement");
            //Boolean? isInvoiceOnlyAtPackageLevel = Convert.ToBoolean((e.Item.FindControl("hdnExcIsInvoiceOnlyAtPackageLevel") as HiddenField).Value);
            Boolean? isInvoiceOnlyAtPackageLevel = (e.Item.DataItem as BackgroundPackagesContract).IsInvoiceOnlyAtPackageLevel;
            Boolean _displayPrice = true;

            if (dvElement.IsNotNull())
            {
                if (isInvoiceOnlyAtPackageLevel.IsNotNull() && isInvoiceOnlyAtPackageLevel.Value)
                {
                    dvElement.Style.Add("display", "none");
                    _displayPrice = false;
                }
                else if (isInvoiceOnlyAtPackageLevel.IsNull())
                {
                    if (applicantOrderCart.IsNotNull() && applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel)
                    {

                        dvElement.Style.Add("display", "none");
                        _displayPrice = false;
                    }
                    else if (applicantOrderCart.IsNull() && IfInvoiceOnlyPymnOptn)
                    {
                        dvElement.Style.Add("display", "none");
                        _displayPrice = false;
                    }
                }

                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                {
                    dvElement.Style.Add("display", "none");
                    _displayPrice = false;
                }
            }




            RadioButton _rBtnExclusive = (e.Item.FindControl("rbtnExclusive") as RadioButton);
            Label lblExclusive = (e.Item.FindControl("lblExclusive") as Label);
            Label lblServiceDesc = (e.Item.FindControl("lblServiceDesc") as Label);
            //UAT-1676 related changes.
            var basePrice = Convert.ToDecimal((e.Item.FindControl("hdfExcBasePrice") as HiddenField).Value);
            TextBox txtNoOfCopies = (e.Item.FindControl("txtNoOfCopies") as TextBox);
            Label lblServiceCode = (e.Item.FindControl("lblServiceCode") as Label);
            Label ExclusiveDesc = (e.Item.FindControl("ExclusiveDesc") as Label);
            Panel pnl02 = (e.Item.FindControl("pnl02") as Panel);
            Panel pnlCABS02 = (e.Item.FindControl("pnlCABS02") as Panel);
            System.Web.UI.HtmlControls.HtmlGenericControl ExclusivePkgDetails = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("ExclusivePkgDetails");
            TextBox txtNoOfPPCopies = (e.Item.FindControl("txtNoOfPPCopies") as TextBox);
            Label lblPPPhotoSet = (e.Item.FindControl("lblPPPhotoSet") as Label);
            TextBox wclPPPrice = (e.Item.FindControl("wclPPPrice") as TextBox);
            Label lblPerPPPhotoSet = (e.Item.FindControl("lblPerPPPhotoSet") as Label);
            RequiredFieldValidator rfvPPPhotoSet = (e.Item.FindControl("rfvNoOfPPCopies") as RequiredFieldValidator);
            RangeValidator rvPPPhotoSet = (e.Item.FindControl("rvNoOfPPCopies") as RangeValidator);                
            HtmlGenericControl exclusivePackagesDetails = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("exclusivePackagesDetails");
            HtmlTableCell cellAdditionalPkgDetails = (e.Item.FindControl("cellAdditionalPkgDetails") as HtmlTableCell);
            HtmlTableCell cellAdditionalPkgDetails01 = (e.Item.FindControl("cellAdditionalPkgDetails01") as HtmlTableCell);
            HtmlTableCell cellAdditionalPkgDetails02 = (e.Item.FindControl("cellAdditionalPkgDetails02") as HtmlTableCell);
            HtmlTableRow rowAdditionalPkgDetails01 = (e.Item.FindControl("rowAdditionalPkgDetails01") as HtmlTableRow);
            RequiredFieldValidator rfvNoOfCopies = (e.Item.FindControl("rfvNoOfCopies") as RequiredFieldValidator);
            RangeValidator rvNoOfCopies = (e.Item.FindControl("rvNoOfCopies") as RangeValidator);
            WclTextBox txtPasscode = (e.Item.FindControl("txtPackagePasscode") as WclTextBox);

            //UAT 3573
            if (!CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                lblExclusive.Text = _displayPrice ? String.Format("{0}{1}", lblExclusive.Text, " ($" + basePrice.ToString("0.00") + ")")
                                                                                : String.Format("{0}", lblExclusive.Text);
                _rBtnExclusive.Text = _displayPrice ? String.Format("{0}{1}", _rBtnExclusive.Text, " ($" + basePrice.ToString("0.00") + ")")
                                                                                : String.Format("{0}", _rBtnExclusive.Text);
            }

            (e.Item.FindControl("hdfIsDisplayPrice") as HiddenField).Value = _displayPrice.ToString();

            if (!CurrentViewContext.lstSelectedBundlePkgId.IsNullOrEmpty() && CurrentViewContext.lstSelectedBundlePkgId.Count > AppConsts.NONE)  //UAT-3283
            // if (CurrentViewContext.SelectedPkgBundleId.IsNotNull() && CurrentViewContext.SelectedPkgBundleId > AppConsts.NONE)
            {
                _rBtnExclusive.Checked = true;
                _rBtnExclusive.Visible = false;
                lblExclusive.Visible = true;
            }
            else
            {
                _rBtnExclusive.Visible = true;
                lblExclusive.Visible = false;
            }
            if (applicantOrderCart.IsNotNull()
                && applicantOrderCart.lstApplicantOrder.IsNotNull()
                && applicantOrderCart.lstApplicantOrder.Any()
                && !applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                if (_rBtnExclusive.IsNotNull())
                {
                    Int32 _packageId = Convert.ToInt32((e.Item.FindControl("hdnExcBPAId") as HiddenField).Value);

                    // Set this package as selected only if Bundle was not selected. 
                    // Else if Bundle has the same package, then the package 
                    // will be marked as selected here also
                    if (applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(pkg => pkg.BPAId == _packageId) &&
                       (applicantOrderCart.lstSelectedPkgBundleId.IsNull() || applicantOrderCart.lstSelectedPkgBundleId.Count == AppConsts.NONE))    //UAT-3283
                    //if (applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(pkg => pkg.BPAId == _packageId) &&
                    //   (applicantOrderCart.SelectedPkgBundleId.IsNull() || applicantOrderCart.SelectedPkgBundleId == AppConsts.NONE))
                    {
                        _rBtnExclusive.Checked = true;

                        txtPasscode.Text = applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(pkg => pkg.BPAId == _packageId).FirstOrDefault().PackagePasscode;
                        if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                        {
                            txtNoOfCopies.Text = applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(pkg => pkg.BPAId == _packageId).FirstOrDefault().CopiesCount.ToString();
                            txtNoOfPPCopies.Text = applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(pkg => pkg.BPAId == _packageId).FirstOrDefault().PPCopiesCount.ToString();
                        }
                    }
                }
            }
            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                Label lblPrice = (e.Item.FindControl("lblServiceDescNonEx") as Label);
                TextBox wclPrice = (e.Item.FindControl("wclPrice") as TextBox);
                Label lblPerCopy = (e.Item.FindControl("lblPerCopy") as Label);
                Label Copies = (e.Item.FindControl("lblCopies") as Label);
                //if (((IEnumerable)rptExclusive.DataSource).Cast<object>().Count() == 1)
                //{
                //    _rBtnExclusive.Checked = true;
                //}
                
                string cssClass = exclusivePackagesDetails.Attributes["Class"] + " colWid";
                exclusivePackagesDetails.Attributes.Add("Class", cssClass);
                cellAdditionalPkgDetails.Style.Add("font-weight", "bold");
                cellAdditionalPkgDetails01.Visible = true;
                cellAdditionalPkgDetails02.Visible = true;
                rowAdditionalPkgDetails01.Visible = true;

                lblServiceDesc.Visible = true;
                //lblServiceDesc.Text = "(" + CurrentViewContext.ServiceDescription + ")";
                
                if (!(CurrentViewContext.FingerPrintData.IsEventCode || CurrentViewContext.FingerPrintData.IsOutOfState))
                {
                    if ((e.Item.DataItem as BackgroundPackagesContract).ServiceCode == BkgServiceType.FingerPrint_Card.GetStringValue() || (e.Item.DataItem as BackgroundPackagesContract).ServiceCode == BkgServiceType.Passport_Photo.GetStringValue())
                    {

                        //btnViewImages.Visible = false;
                        txtNoOfCopies.Visible = true;
                        lblPrice.Visible = true;
                        wclPrice.Visible = true;
                        lblPerCopy.Visible = true;
                        Copies.Visible = true;
                        wclPrice.ReadOnly = true;
                        wclPrice.Text = basePrice.ToString("0.00");
                        //dvOrderHistoryDetails.Visible = _rChkNonExclusive.Checked;

                        //if (((e.Item.DataItem as BackgroundPackagesContract).ServiceCode == BkgServiceType.Passport_Photo.GetStringValue()) && !CurrentViewContext.FingerPrintData.IsFromArchivedOrderScreen)
                        //{
                        //    //btnViewImages.Visible = true;
                        //    txtNoOfCopies.Enabled = _rBtnExclusive.Enabled = !(applicantOrderCart.IsNull() || applicantOrderCart.lstApplicantOrder.IsNull() || applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty());
                        //}
                    }
                }

                if (lblServiceCode.Text == "AAAA")
                {
                    ExclusivePkgDetails.Style.Add("display", "none");
                    pnl02.Visible = false;
                    pnlCABS02.Visible = true;
                    ExclusiveDesc.Text = Resources.Language.CABSDESC;
                }

                if (lblServiceCode.Text == "AAAQ")
                {
                    ExclusivePkgDetails.Style.Add("display", "none");
                    pnl02.Visible = false;               
                    pnlCABS02.Visible = true;
                    ExclusiveDesc.Text = Resources.Language.FCDESC;
                    wclPrice.Text = CurrentViewContext.lstAdditionalServiceFeeOption.Where(x => x.FieldValue == "AAAA").Select(x => x.SIFR_Amount).FirstOrDefault().ToString();
                    rfvNoOfCopies.Enabled = true;
                    rvNoOfCopies.Enabled = true;
                }
                if (lblServiceCode.Text == "AAAR")
                {
                    ExclusivePkgDetails.Style.Add("display", "none");
                    pnl02.Visible = false;
                    pnlCABS02.Visible = true;
                    ExclusiveDesc.Text = Resources.Language.FCANDPPDESC;
                    lblPPPhotoSet.Visible = true;
                    txtNoOfPPCopies.Visible = true;
                    wclPPPrice.Visible = true;
                    lblPerPPPhotoSet.Visible = true;
                    rfvPPPhotoSet.Enabled = true;
                    rvNoOfCopies.Enabled = true;
                    rfvNoOfCopies.Enabled = true;
                    rvPPPhotoSet.Enabled = true;
                    wclPrice.Text = CurrentViewContext.lstAdditionalServiceFeeOption.Where(x => x.FieldValue == "AAAA" && Convert.ToInt32(x.FieldID) == 1).Select(x => x.SIFR_Amount).FirstOrDefault().ToString();
                    wclPPPrice.Text = CurrentViewContext.lstAdditionalServiceFeeOption.Where(x => x.FieldValue == "AAAB" && Convert.ToInt32(x.FieldID) == 2).Select(x => x.SIFR_Amount).FirstOrDefault().ToString();
                }
                else
                {
                    lblPPPhotoSet.Text = "1";
                }
            }
            else
            {
                lblServiceDesc.Visible = false;
            }
        }

        protected void rptNonExclusive_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ////for hiding the price if invoice is only payment option available.
            //if (applicantOrderCart.IsNotNull())
            //{
            //    if (applicantOrderCart.ifInvoiceIsOnlyPaymentOptionAvailable)
            //    {
            //        System.Web.UI.HtmlControls.HtmlGenericControl dvElement = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvElement1");
            //        dvElement.Style.Add("display", "none");
            //    }
            //}
            //else
            //{
            //    if (IfInvoiceOnlyPymnOptn)
            //    {
            //        System.Web.UI.HtmlControls.HtmlGenericControl dvElement = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvElement1");
            //        dvElement.Style.Add("display", "none");
            //    }
            //}
            Boolean _displayPrice = true;
            System.Web.UI.HtmlControls.HtmlGenericControl dvElement = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvElement1");
            Boolean? isInvoiceOnlyAtPackageLevel = (e.Item.DataItem as BackgroundPackagesContract).IsInvoiceOnlyAtPackageLevel; //Convert.ToBoolean((e.Item.FindControl("hdnNonExcIsInvoiceOnlyAtPackageLevel") as HiddenField).Value);
            if (dvElement.IsNotNull())
            {
                if (isInvoiceOnlyAtPackageLevel.IsNotNull() && isInvoiceOnlyAtPackageLevel.Value)
                {
                    dvElement.Style.Add("display", "none");
                    _displayPrice = false;
                }
                else if (isInvoiceOnlyAtPackageLevel.IsNull())
                {
                    if (applicantOrderCart.IsNotNull() && applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel)
                    {

                        dvElement.Style.Add("display", "none");
                        _displayPrice = false;
                    }
                    else if (applicantOrderCart.IsNull() && IfInvoiceOnlyPymnOptn)
                    {
                        dvElement.Style.Add("display", "none");
                        _displayPrice = false;
                    }
                }
            }

            //UAT 3573
            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                dvElement.Style.Add("display", "none");
                _displayPrice = false;
            }            

            CheckBox _rChkNonExclusive = (e.Item.FindControl("chkNonExc") as CheckBox);
            Label lblNonExc = (e.Item.FindControl("lblNonExc") as Label);
            Label lblServiceDescNonEx = (e.Item.FindControl("lblServiceDescNonEx") as Label);
            Label lblServiceCode = (e.Item.FindControl("lblServiceCode") as Label);
            Label NonExclusiveDesc = (e.Item.FindControl("NonExclusiveDesc") as Label);
            //Panel dvOrderHistoryDetails = (e.Item.FindControl("dvOrderHistoryDetails") as Panel);

            WclTextBox txtPasscode = (e.Item.FindControl("txtNonExcPackagePasscode") as WclTextBox);            
            //CheckBox chkIsOrderHistory = (e.Item.FindControl("chkIsOrderHistory") as CheckBox);
            TextBox txtNoOfCopies = (e.Item.FindControl("txtNoOfCopies") as TextBox);
            Panel pnl01 = (e.Item.FindControl("pnl01") as Panel);
            Panel pnlCABS01 = (e.Item.FindControl("pnlCABS01") as Panel);
            System.Web.UI.HtmlControls.HtmlGenericControl NonExclusivePkgDetails = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("NonExclusivePkgDetails");
            HtmlGenericControl nonExclusivePackagesDetails = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("nonExclusivePackagesDetails");
            HtmlTableCell cellAdditionalPkgDetailsNonExc = (e.Item.FindControl("cellAdditionalPkgDetailsNonExc") as HtmlTableCell);

            //UAT-1676 related changes.
            var basePrice = Convert.ToDecimal((e.Item.FindControl("hdfNonExcBasePrice") as HiddenField).Value);

            lblNonExc.Text = _displayPrice ? String.Format("{0}{1}", lblNonExc.Text, " ($" + basePrice.ToString("0.00") + ")")
                                                                            : String.Format("{0}", lblNonExc.Text);
            _rChkNonExclusive.Text = _displayPrice ? String.Format("{0}{1}", _rChkNonExclusive.Text, " ($" + basePrice.ToString("0.00") + ")")
                                                                            : String.Format("{0}", _rChkNonExclusive.Text);

            (e.Item.FindControl("hdfIsDisplayPrice") as HiddenField).Value = _displayPrice.ToString();

            if (CurrentViewContext.lstSelectedBundlePkgId.IsNotNull() && CurrentViewContext.lstSelectedBundlePkgId.Count > AppConsts.NONE) //UAT-3283

            //if (CurrentViewContext.SelectedPkgBundleId.IsNotNull() && CurrentViewContext.SelectedPkgBundleId > AppConsts.NONE)
            {
                _rChkNonExclusive.Checked = true;
                _rChkNonExclusive.Visible = false;
                lblNonExc.Visible = true;
            }

            else
            {
                _rChkNonExclusive.Visible = true;
                lblNonExc.Visible = false;
            }
            if (applicantOrderCart.IsNotNull() && applicantOrderCart.lstApplicantOrder.IsNotNull() && !applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                if (_rChkNonExclusive.IsNotNull() && applicantOrderCart.IsNotNull())
                {
                    Int32 _packageId = Convert.ToInt32((e.Item.FindControl("hdnNonExcBPAId") as HiddenField).Value);

                    // Set this package as selected only if Bundle was not selected. 
                    // Else if Bundle has the same package, then the package 
                    // will be marked as selected here also
                    if (applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(pkg => pkg.BPAId == _packageId) &&
                      (applicantOrderCart.lstSelectedPkgBundleId.IsNull() || applicantOrderCart.lstSelectedPkgBundleId.Count == AppConsts.NONE)) //UAT-3283
                    //if (applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(pkg => pkg.BPAId == _packageId) &&
                    //   (applicantOrderCart.SelectedPkgBundleId.IsNull() || applicantOrderCart.SelectedPkgBundleId == AppConsts.NONE))
                    {
                        _rChkNonExclusive.Checked = true;
                        txtPasscode.Text = applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(pkg => pkg.BPAId == _packageId).FirstOrDefault().PackagePasscode;
                        if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                        {

                            txtNoOfCopies.Text = applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(pkg => pkg.BPAId == _packageId).FirstOrDefault().CopiesCount.ToString();
                            //chkIsOrderHistory.Checked = applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(pkg => pkg.BPAId == _packageId).FirstOrDefault().IsOrderHistory;
                            _rChkNonExclusive.Enabled = txtNoOfCopies.Enabled = _rChkNonExclusive.Checked;
                        }
                    }
                    if (applicantOrderCart.lstApplicantOrder[0].lstPackages.Any() && CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                    {
                        _rChkNonExclusive.Enabled = txtNoOfCopies.Enabled = true;
                    }
                }
            }
            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                //Label lblHistoryDetails = (e.Item.FindControl("lblHistoryDetails") as Label);
                Label lblPrice = (e.Item.FindControl("lblServiceDescNonEx") as Label);
                TextBox wclPrice = (e.Item.FindControl("wclPrice") as TextBox);
                Label lblPerCopy = (e.Item.FindControl("lblPerCopy") as Label);
                Label Copies = (e.Item.FindControl("lblCopies") as Label);
                //RadButton btnViewImages = (e.Item.FindControl("btnViewImages") as RadButton);
                //if (((IEnumerable)rptNonExclusive.DataSource).Cast<object>().Count() == 1)
                //{
                //    _rChkNonExclusive.Checked = true;
                //    if ((e.Item.DataItem as BackgroundPackagesContract).ServiceCode == BkgServiceType.SIMPLE.GetStringValue())
                //        dvCBIUnique.Visible = true;
                //}
                lblServiceDescNonEx.Visible = true;

                string cssClass = nonExclusivePackagesDetails.Attributes["Class"] + " colWid";
                nonExclusivePackagesDetails.Attributes.Add("Class", cssClass);
                cellAdditionalPkgDetailsNonExc.Style.Add("font-weight", "bold");

                //lblServiceDescNonEx.Text = (e.Item.DataItem as BackgroundPackagesContract).ServiceDescription.IsNullOrEmpty() ? "(" + CurrentViewContext.ServiceDescription + ")" : "(" + (e.Item.DataItem as BackgroundPackagesContract).ServiceDescription + ")";
                if (!(CurrentViewContext.FingerPrintData.IsEventCode || CurrentViewContext.FingerPrintData.IsOutOfState))
                {
                    //divIsConsent.Visible = !CurrentViewContext.FingerPrintData.IsFromArchivedOrderScreen;

                    if ((e.Item.DataItem as BackgroundPackagesContract).ServiceCode == BkgServiceType.FingerPrint_Card.GetStringValue() || (e.Item.DataItem as BackgroundPackagesContract).ServiceCode == BkgServiceType.Passport_Photo.GetStringValue())
                    {

                        //btnViewImages.Visible = false;
                        txtNoOfCopies.Visible = true;
                        lblPrice.Visible = true;
                        wclPrice.Visible = true;
                        lblPerCopy.Visible = true;
                        Copies.Visible = true;
                        wclPrice.ReadOnly = true;
                        wclPrice.Text = basePrice.ToString("0.00");
                        //dvOrderHistoryDetails.Visible = _rChkNonExclusive.Checked;

                        if (((e.Item.DataItem as BackgroundPackagesContract).ServiceCode == BkgServiceType.Passport_Photo.GetStringValue()) && !CurrentViewContext.FingerPrintData.IsFromArchivedOrderScreen)
                        {
                            //btnViewImages.Visible = true;
                            txtNoOfCopies.Enabled = _rChkNonExclusive.Enabled = !(applicantOrderCart.IsNull() || applicantOrderCart.lstApplicantOrder.IsNull() || applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty());
                        }
                    }
                }

                if (lblServiceCode.Text=="AAAA")
                {
                    NonExclusivePkgDetails.Style.Add("display", "none");
                    pnl01.Visible = false;
                    pnlCABS01.Visible = true;
                    NonExclusiveDesc.Text = Resources.Language.CABSDESC;
                }               
            }
            else
            {
                lblServiceDescNonEx.Visible = false;
            }            
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        //UAT - 2802
        private void CheckIsOrderFlowMessageSettingEnable()
        {
            hdfPackageOrderConfirmationsttings.Value = _presenter.IsOrderFlowSettingsEnable().ToString(
                );
        }

        private void SetCurentComplianceTypeByControlID(string controlID)
        {
            CurrentCompliancePackageType = CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue();
            if (AvailableComplaincePackageTypes.IsNotNull())
            {
                foreach (string cptype in AvailableComplaincePackageTypes)
                {
                    if (controlID.Contains("_" + cptype))
                    {
                        CurrentCompliancePackageType = cptype;
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// Load Compliance Package if 
        /// 1. There is no Background Packages OR 
        /// 2. If there are Compliance packages and user clicks NEXT
        /// </summary>
        //private void LoadCompliancePackage()
        //{
        //    SetSelectedHierarchyData();
        //    BindComnpliancePackages();
        //    BindPackageDetails();
        //}

        /// <summary>
        /// Start user order 
        /// 1. On click of start order 
        /// 2. There is no compliance package to select, for the applicant
        /// </summary>
        private void StartOrder()
        {
            applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            IsExecPasscodeMatched = true;
            IsNonExecPasscodeMatched = true;
            if (applicantOrderCart == null)
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.GetApplicantOrder();
            }

            if (applicantOrderCart.IsNotNull() && (CurrentViewContext.FingerPrintData.IsLocationServiceTenant))
            {
                if (applicantOrderCart.FingerPrintData.IsNull())
                    applicantOrderCart.FingerPrintData = new FingerPrintAppointmentContract();
                applicantOrderCart.FingerPrintData.LocationId = CurrentViewContext.FingerPrintData.LocationId;
                applicantOrderCart.FingerPrintData.LocationName = CurrentViewContext.FingerPrintData.LocationName;
                applicantOrderCart.FingerPrintData.LocationAddress = CurrentViewContext.FingerPrintData.LocationAddress;
                applicantOrderCart.FingerPrintData.LocationDescription = CurrentViewContext.FingerPrintData.LocationDescription;
                applicantOrderCart.FingerPrintData.IsEventCode = CurrentViewContext.FingerPrintData.IsEventCode;
                applicantOrderCart.FingerPrintData.StartTime = CurrentViewContext.FingerPrintData.StartTime;
                applicantOrderCart.FingerPrintData.EndTime = CurrentViewContext.FingerPrintData.EndTime;
                applicantOrderCart.FingerPrintData.ReserverSlotID = CurrentViewContext.FingerPrintData.ReserverSlotID;
                applicantOrderCart.FingerPrintData.SlotID = CurrentViewContext.FingerPrintData.SlotID;
                applicantOrderCart.FingerPrintData.SlotDate = CurrentViewContext.FingerPrintData.SlotDate;
                applicantOrderCart.FingerPrintData.EventName = CurrentViewContext.FingerPrintData.EventName;
                applicantOrderCart.FingerPrintData.EventDescription = CurrentViewContext.FingerPrintData.EventDescription;
                applicantOrderCart.IsLocationServiceTenant = true;
                applicantOrderCart.FingerPrintData.CBIUniqueID = CurrentViewContext.FingerPrintData.CBIUniqueID.Trim();
                applicantOrderCart.FingerPrintData.IsSSNRequired = CurrentViewContext.FingerPrintData.IsSSNRequired;
                applicantOrderCart.FingerPrintData.lstAutoFilledAttributes = CurrentViewContext.FingerPrintData.lstAutoFilledAttributes;
                applicantOrderCart.FingerPrintData.BillingCode = CurrentViewContext.FingerPrintData.BillingCode;
                applicantOrderCart.FingerPrintData.IsLegalNameChange = CurrentViewContext.FingerPrintData.IsLegalNameChange;
                //UAT-3850
                applicantOrderCart.FingerPrintData.BillingCodeAmount = CurrentViewContext.FingerPrintData.BillingCodeAmount;
                applicantOrderCart.FingerPrintData.IsConsent = CurrentViewContext.FingerPrintData.IsConsent;
                applicantOrderCart.FingerPrintData.IsMailingOnly = CurrentViewContext.FingerPrintData.IsMailingOnly;
                applicantOrderCart.FingerPrintData.IsFromArchivedOrderScreen = CurrentViewContext.FingerPrintData.IsFromArchivedOrderScreen;
                applicantOrderCart.IncrementOrderStepCount();
            }

            if (!IsSamePkgInMultipleSelectedBundles())
            {

                base.ShowInfoMessage("The order can't proceed because the selected bundles have common package(s).");
                return;
            }

            // This check is required as when user navigates back from the Applicant Profile screen, 
            // then query string will be NULL and GetNavigationFrom() will set this property to ApplicantDashBoard,
            // even if the user had started from the ApplicantLanding page
            if (String.IsNullOrEmpty(applicantOrderCart.PendingOrderNavigationFrom))
            {
                applicantOrderCart.PendingOrderNavigationFrom = GetNavigationFrom();
            }

            if (!String.IsNullOrEmpty(applicantOrderCart.EDrugScreeningRegistrationId))
            {
                applicantOrderCart.EDrugScreeningRegistrationId = null;
            }

            SetSelectedHierarchyData();

            //UAT-3283

            //var _isCompliancePkgInMultipleSelectedBundles = IsCompliancePkgInMultipleSelectedBundles();

            var _isBundleSelected = false;
            //var _selectedBundleId = AppConsts.NONE;
            List<Int32> _selectedBundleIds = new List<Int32>();
            IsBundleSelected(ref _isBundleSelected, ref _selectedBundleIds);

            if (applicantOrderCart.IsNotNull() && applicantOrderCart.OrderRequestType != OrderRequestType.ChangeSubscription.GetStringValue() && !_isBundleSelected)
            {
                #region Validate the selection of an exclusive package - Code commented corresponding to UAT - 741

                //Boolean _isExclusiveSelected = false;
                //Boolean _isNonExclusiveSelected = false;
                //foreach (var repeaterItem in rptExclusive.Items)
                //{
                //    RadioButton _rbtn = ((repeaterItem as RepeaterItem).FindControl("rbtnExclusive") as RadioButton);
                //    if (_rbtn.Checked)
                //    {
                //        _isExclusiveSelected = true;
                //        break;
                //    }
                //}
                //foreach (var rptItem in rptNonExclusive.Items)
                //{
                //    CheckBox _chkNonExclusive = ((rptItem as RepeaterItem).FindControl("chkNonExc") as CheckBox);
                //    if (_chkNonExclusive.IsNotNull() && _chkNonExclusive.Checked)
                //    {

                //        _isNonExclusiveSelected = true;
                //        break;
                //    }
                //}

                //if (!_isExclusiveSelected && pnlExclusiveBkgPackages.Visible)
                //{
                //    if (_isNonExclusiveSelected && pnlNonExclusiveBkgPackages.Visible)
                //    {
                //        base.ShowInfoMessage("Please select an Exclusive Package from the listing.");
                //        return;
                //    }
                //    else if (divCompliancePackage.Visible && divSubscriptions.Visible
                //        && (ddlDeptprogramPkg.SelectedValue == AppConsts.ZERO || String.IsNullOrEmpty(ddlDeptprogramPkg.SelectedValue)))
                //    {
                //        base.ShowInfoMessage("Please select an Exclusive Package from the listing.");
                //        return;
                //    }
                //}

                #endregion

                #region Add Background Packages to Session object
                //// UAT 3771 Passcode required functionality for background check packages
                //if (IsEnteredPassCodeMatchedForBkgPackage())
                //{
                AddBackgroundPackageDataToSession();
                //}
                //else
                //{
                //    base.ShowInfoMessage("The order can't proceed because entered passcode doesn't match.");
                //    return;
                //}
                #endregion
            }

            if (!_isBundleSelected && (!IsExecPasscodeMatched || !IsNonExecPasscodeMatched))
            {
                base.ShowInfoMessage("Passcode does not match. Please provide valid passcode.");
                hdfIsMessageAcknowledged.Value = "0";
                return;
            }

            #region Add Compliance Packages to Session object

            AddCompliancePackageDataToSession(_isBundleSelected);

            #endregion

            applicantOrderCart.AddOrderStageTrackID(OrderStages.PendingOrder);

            // If No package is selected, then stop navigation
            if (!applicantOrderCart.IsCompliancePackageSelected && applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {

                //base.ShowInfoMessage("Please select any Package to continue.");
                base.ShowInfoMessage(Resources.Language.PLSSLCTPCKGS);
                return;
            }

            //BS
            Int32 _customFormSteps = AppConsts.NONE;

            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                _customFormSteps = GetTotalCustomForms(applicantOrderCart.lstApplicantOrder[0].lstPackages.ToList());
                if (CurrentViewContext.FingerPrintData.IsLocationType && CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                {
                    applicantOrderCart.FingerPrintData.IsFingerPrintAndPassPhotoService = !applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(x => x.ServiceCode == BkgServiceType.SIMPLE.GetStringValue());
                    //bool IsAllOrderHistoryChecked = applicantOrderCart.FingerPrintData.IsFingerPrintAndPassPhotoService == true ? applicantOrderCart.lstApplicantOrder[0].lstPackages.All(x => x.IsOrderHistory == true) : false;
                    if (applicantOrderCart.FingerPrintData.IsFingerPrintAndPassPhotoService)
                    {
                        applicantOrderCart.FingerPrintData.IsMailingOnly = true;
                    }
                }
            }

            //UAT 3521 Add two steps in existing flow because appointment and location screens are added
            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                if (CurrentViewContext.FingerPrintData.IsEventCode)
                {
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.SIX + _customFormSteps);
                }
                else if (CurrentViewContext.FingerPrintData.IsOutOfState)
                {
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.SIX + _customFormSteps);
                }
                else
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.SEVEN + _customFormSteps);
            }
            else
            {
                // For placing 'Rush Order for existing order' and 'Renew subscription', different screens are used
                if (!applicantOrderCart.IsCompliancePackageSelected)
                {
                    //Set Total Order steps to Seven because a new Required Documentation screen is added in order flow [UAT-1560]
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.SEVEN + _customFormSteps);

                }
                else
                {
                    //Set Total Order steps to Eight because a new Required Documentation screen is added in order flow [UAT-1560] 
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.SEVEN + _customFormSteps);
                }
            }

            applicantOrderCart.IncrementOrderStepCount();

            var selectedHierarchyNodeId = GetSelectedHierarchyNodeId();
            applicantOrderCart.NodeId = CurrentViewContext.NodeId = Presenter.GetLastNodeInstitutionId(Convert.ToInt32(selectedHierarchyNodeId));
            applicantOrderCart.SelectedHierarchyNodeID = selectedHierarchyNodeId != String.Empty ? Convert.ToInt32(selectedHierarchyNodeId) : AppConsts.NONE;

            Int32 selectedNodeId;
            if (applicantOrderCart.IsCompliancePackageSelected)
            {
                selectedNodeId = applicantOrderCart.DPMId;
            }
            else
            {
                selectedNodeId = Convert.ToInt32(selectedHierarchyNodeId);
            }

            IfInvoiceOnlyPymnOptn = Presenter.IfInvoiceIsOnlyPaymentOptions(selectedNodeId);
            applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel = IfInvoiceOnlyPymnOptn;

            #region UAT-1560: WB: We should be able to add documents that need to be signed to the order process
            Presenter.GetAdditionalDocuments(applicantOrderCart.lstApplicantOrder[0].lstPackages, applicantOrderCart.SelectedHierarchyNodeID.Value,
                                             applicantOrderCart.CompliancePackages, applicantOrderCart.IsCompliancePackageSelected);
            applicantOrderCart.IsAdditionalDocumentExist = IsAdditionalDocumentExist;
            if (IsAdditionalDocumentExist && !CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                if (!applicantOrderCart.IsCompliancePackageSelected)
                {
                    //Set Total Order steps to Seven because a new Required Documentation screen is added in order flow [UAT-1560]

                    //UAT-3234
                    //applicantOrderCart.SetTotalOrderSteps(AppConsts.SEVEN + _customFormSteps);
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.EIGHT + _customFormSteps);
                }
                else
                {
                    //Set Total Order steps to Eight because a new Required Documentation screen is added in order flow [UAT-1560] 
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.EIGHT + _customFormSteps);
                }
            }

            //UAT 3521
            if (IsAdditionalDocumentExist && CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                if (CurrentViewContext.FingerPrintData.IsEventCode)
                {
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.EIGHT + _customFormSteps);
                }
                else if (CurrentViewContext.FingerPrintData.IsOutOfState)
                {
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.EIGHT + _customFormSteps);
                }
                else
                {
                    applicantOrderCart.SetTotalOrderSteps(AppConsts.NINE + _customFormSteps);
                }
            }
            if (CurrentViewContext.FingerPrintData.IsOutOfState)
            {
                applicantOrderCart.FingerPrintData.IsOutOfState = true;
            }
            #endregion
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);

            Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.ApplicantProfile}
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }

        /// <summary>
        /// Returns if Bundle was selected
        /// </summary>
        /// <param name="_isBundleSelected"></param>
        /// <param name="_selectedBundleId"></param>
        private void IsBundleSelected(ref bool _isBundleSelected, ref List<int> _selectedBundleIds)
        {
            foreach (RepeaterItem bundleItem in rptBundles.Items)
            {
                //if ((bundleItem.FindControl("rbtnBundle") as RadioButton).Checked)
                if ((bundleItem.FindControl("chkBundles") as CheckBox).Checked)
                {
                    Int32 selectedBundleId = Convert.ToInt32((bundleItem.FindControl("hdfBundleId") as HiddenField).Value);
                    _selectedBundleIds.Add(selectedBundleId);  //UAT-3283
                    _isBundleSelected = true;
                    // break;
                }
            }
            //UAT 3775 Ability to make Bundle packages exclusive (like screening packages)
            foreach (RepeaterItem bundleItem in rptBundlesExclusive.Items)
            {
                if ((bundleItem.FindControl("rbtnExclusiveBundle") as RadioButton).Checked)
                // if ((bundleItem.FindControl("chkBundles") as CheckBox).Checked)
                {
                    Int32 selectedBundleId = Convert.ToInt32((bundleItem.FindControl("hdfExcBundleId") as HiddenField).Value);
                    _selectedBundleIds.Add(selectedBundleId);  //UAT-3283
                    _isBundleSelected = true;
                    // break;
                }
            }

            //Implemented in UAT-3283 to add multiple bundle packages in an order.
            if (!_selectedBundleIds.IsNullOrEmpty() && _isBundleSelected)
            {
                applicantOrderCart.lstSelectedPkgBundleId = new List<Int32>();
                applicantOrderCart.lstSelectedPkgBundleId = CurrentViewContext.lstSelectedBundlePkgId = _selectedBundleIds;
            }
            else
            {
                applicantOrderCart.lstSelectedPkgBundleId = null;
                //applicantOrderCart.lstSelectedPkgBundleId = new List<Int32>();
            }

            //Below code is commented in UAT-3283
            //if (_isBundleSelected)
            //{
            //    applicantOrderCart.SelectedPkgBundleId = CurrentViewContext.lstSelectedBundlePkgId = _selectedBundleIds;
            //}
            //else
            //{
            //    applicantOrderCart.SelectedPkgBundleId = null;
            //}
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
                return _presenter.GetCustomFormsCount(_packageIds);

            return AppConsts.NONE;
        }

        /// <summary>
        /// Bind the packages when user selects Load Packages or Navigates back from the View Details screen
        /// </summary>
        private void LoadPackages()
        {
            Boolean _isBackgroundPackageAvailable = false;

            Boolean _isCompliancePackageAvailable = BindCompliancePackages();

            // Load Background packages only if it is not Change Subscription
            if (!this.IsChangeSubscriptionRequest)
            {
                _isBackgroundPackageAvailable = BindBackgroundPackages();
            }
            //Changes related to issue Subscription Continue button is not visible if  only bundles are available at node. 
            Boolean _isanyBundleAvailbleForOrder = BindPackageBundle();

            if (!CurrentViewContext.AlreadyPurchasedPackages.IsNullOrEmpty())
            {
                String purchasedPackageMsg = "You have already purchased package(s) " + CurrentViewContext.AlreadyPurchasedPackages + ".";
                base.ShowInfoMessage(purchasedPackageMsg);
                if (_isBackgroundPackageAvailable || _isCompliancePackageAvailable || _isanyBundleAvailbleForOrder)
                {
                    ShowHideSubscriptionContinueButton(true);
                }
                // hHelpText.Visible = true; //UAT-3253
            }

            // If No Package is available, 
            // 1. Display message of Already purchased packages, if applicable
            // 2. Hide the HELP text
            if (!_isBackgroundPackageAvailable && !_isCompliancePackageAvailable && !_isanyBundleAvailbleForOrder)
            {
                // Override the message set by 'if' above.
                if (!CurrentViewContext.AlreadyPurchasedPackages.IsNullOrEmpty())
                {
                    base.ShowInfoMessage("You have already purchased package(s) " + CurrentViewContext.AlreadyPurchasedPackages
                        + ". Also, No package found for the criteria you have selected.");
                }
                else
                {
                    base.ShowInfoMessage("No package for the criteria you have selected.");
                }
                //To handle the scenario, when initially there was a package available,
                // but admin settings were changed to No package and then 'Load Pacakges' is clicked.
                ShowHideSubscriptionContinueButton(false);
                // hHelpText.Visible = false; //UAT-3253
            }
            else
            {
                //Hide help text, if Only Bundles are available for purchase.
                if (CurrentViewContext.PreviousOrderId > 0 || (!_isBackgroundPackageAvailable && !_isCompliancePackageAvailable))
                {
                    // hHelpText.Visible = false; // Hide for change subscription //UAT-3253
                }
                else
                {
                    // hHelpText.Visible = true; // Show for new order  //UAT-3253
                }
                ShowHideSubscriptionContinueButton(true);
            }
            if ((!this.IsChangeSubscriptionRequest) && (!CurrentViewContext.FingerPrintData.IsLocationServiceTenant))
            {
                dvOrderTotal.Visible = true;
                divFeeVideo.Visible = true;

            }
            if (!_isBackgroundPackageAvailable && !_isCompliancePackageAvailable && !_isanyBundleAvailbleForOrder)
            {
                dvOrderTotal.Visible = false;
            }
            DisplayOrderCost(AppConsts.NONE, AppConsts.NONE, AppConsts.NONE, false, false, false);
        }

        /// <summary>
        /// Check the 'Invoice Only' payment Options at the Last Selected Level.
        /// </summary>
        /// <param name="isCompliancePackageAvailable"></param>
        private void CheckIfInvoiceIsOnlyPaymentOption(Boolean isCompliancePackageAvailable)
        {
            SelectedHierarchyNodeId = Convert.ToInt32(GetSelectedHierarchyNodeId());
            IfInvoiceOnlyPymnOptn = Presenter.IfInvoiceIsOnlyPaymentOptions(SelectedHierarchyNodeId);
            //if (isCompliancePackageAvailable)
            //{
            //    //SelectedHierarchyNodeId = CurrentViewContext.DeptProgramPackages.FirstOrDefault().DPP_DeptProgramMappingID; commented for UAT-916
            //    SelectedHierarchyNodeId = CurrentViewContext.DeptProgramPackages.LastOrDefault().DPP_DeptProgramMappingID;
            //    IfInvoiceOnlyPymnOptn = Presenter.IfInvoiceIsOnlyPaymentOptions(SelectedHierarchyNodeId);
            //    if (IfInvoiceOnlyPymnOptn)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        var selectedHierarchyNodeId = GetSelectedHierarchyNodeId();
            //        SelectedHierarchyNodeId = Convert.ToInt32(selectedHierarchyNodeId);
            //        IfInvoiceOnlyPymnOptn = Presenter.IfInvoiceIsOnlyPaymentOptions(SelectedHierarchyNodeId);
            //    }
            //}
            //else
            //{
            //   }
        }


        /// <summary>
        /// Add the Compliance Package data to the Session object when Normal package selection and
        /// Add Compliance as well as Background package data to session when Bundle Selected.
        /// </summary>
        private void AddCompliancePackageDataToSession(Boolean isBundleSelected)
        {
            SetSelectedHierarchyData();
            if (!isBundleSelected)
                Presenter.GetDeptProgramPackage();
            else
            {
                Presenter.GetPackageBundlesAvailableForOrder();
                Presenter.GetPackagelistAvailableUnderBundle();
            }
            if (applicantOrderCart.IsNotNull())
            {
                applicantOrderCart.CompliancePackages = null;
            }
            applicantOrderCart.IsCompliancePackageSelected = false;

            // Normal Package selection and No Compliance package selected.
            if (!isBundleSelected && AvailableComplaincePackageTypes.IsNullOrEmpty())
            {
                return;
            }

            if (isBundleSelected)
            {
                #region Add Bundle Data to Session, for Both Compliance and Background.
                if (!applicantOrderCart.lstApplicantOrder[0].IsNullOrEmpty())
                    applicantOrderCart.lstApplicantOrder[0].lstPackages = new List<BackgroundPackagesContract>(); //UAT-3283
                foreach (RepeaterItem repeaterItem in rptBundles.Items)
                {
                    var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfBundleId") as HiddenField).Value);

                    // If the Bundle is Selected and ID is equal to the one added in Cart, before calling this function.
                    if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked && !applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty() && applicantOrderCart.lstSelectedPkgBundleId.Contains(_bundleId)) //UAT-3283
                    //if ((repeaterItem.FindControl("rbtnBundle") as RadioButton).Checked && _bundleId == applicantOrderCart.SelectedPkgBundleId)
                    {
                        RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptBundlePackages") as Repeater).Items;
                        //applicantOrderCart.lstApplicantOrder[0].lstPackages = new List<BackgroundPackagesContract>();

                        foreach (RepeaterItem rptItem in _rptItems)
                        {
                            var _masterTypeCode = (rptItem.FindControl("hdfMasterPackageTypeCode") as HiddenField).Value;

                            if (_masterTypeCode == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
                            {
                                #region Add Compliance Data to Session for Bundle

                                applicantOrderCart.BundleInContext = _bundleId;  //UAT-3283
                                applicantOrderCart.CurrentCompliancePackageTypeInContext = CurrentCompliancePackageType = (rptItem.FindControl("hdfCompliancePackageTypeCode") as HiddenField).Value;
                                SetOrderCartData(rptItem);

                                #endregion
                            }
                            else
                            {
                                #region Add Background Package to Session for Bundles

                                Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfPackageId") as HiddenField).Value);
                                Int32 _packageHierarchyMappingId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnBPHMId") as HiddenField).Value);
                                Int32 _packageMaxNumberOfYearforResidence = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnMaxResidence") as HiddenField).Value);
                                Decimal _packageBasePrice = Convert.ToDecimal(((rptItem as RepeaterItem).FindControl("hdfBkgPackageBasePrice") as HiddenField).Value);
                                //UAT-3268
                                Boolean _isReqToQualifyInRotation = Convert.ToBoolean(((rptItem as RepeaterItem).FindControl("hdnIsReqToQualifyInRotation") as HiddenField).Value);
                                HiddenField hdnAdditionalPrice = ((rptItem as RepeaterItem).FindControl("hdnAdditionalPrice") as HiddenField);
                                Decimal _additionalPrice = new Decimal();
                                if (!hdnAdditionalPrice.Value.IsNullOrEmpty())
                                    _additionalPrice = Convert.ToDecimal(hdnAdditionalPrice.Value);
                                BackgroundPackagesContract _bkgPackage = new BackgroundPackagesContract();
                                _bkgPackage.BPAId = _packageId;
                                _bkgPackage.IsExclusive = false;
                                _bkgPackage.BPHMId = _packageHierarchyMappingId;
                                _bkgPackage.BasePrice = _packageBasePrice;
                                _bkgPackage.MaxNumberOfYearforResidence = _packageMaxNumberOfYearforResidence;
                                _bkgPackage.IsReqToQualifyInRotation = _isReqToQualifyInRotation; //UAT-3268
                                _bkgPackage.AdditionalPrice = _additionalPrice;

                                if (applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
                                {
                                    applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                                    ApplicantOrder _applicantOrder = new ApplicantOrder();

                                    if (_applicantOrder.lstPackages.IsNullOrEmpty())
                                    {
                                        _applicantOrder.lstPackages = new List<BackgroundPackagesContract>();
                                    }

                                    _applicantOrder.lstPackages.Add(_bkgPackage);
                                    applicantOrderCart.lstApplicantOrder.Add(_applicantOrder);
                                }
                                else
                                {

                                    applicantOrderCart.lstApplicantOrder[0].lstPackages.Add(_bkgPackage);
                                }

                                if (!_bkgPackage.IsNullOrEmpty())
                                {
                                    applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();
                                }

                                #endregion
                            }
                        }
                        // break;
                    }
                }
                //UAT 3775 Ability to make Bundle packages exclusive (like screening packages)
                foreach (RepeaterItem repeaterItem in rptBundlesExclusive.Items)
                {
                    var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfExcBundleId") as HiddenField).Value);

                    // If the Bundle is Selected and ID is equal to the one added in Cart, before calling this function.
                    // if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked && !applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty() && applicantOrderCart.lstSelectedPkgBundleId.Contains(_bundleId)) //UAT-3283
                    if ((repeaterItem.FindControl("rbtnExclusiveBundle") as RadioButton).Checked && !applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty() && applicantOrderCart.lstSelectedPkgBundleId.Contains(_bundleId))
                    {
                        RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptExcBundlePackages") as Repeater).Items;
                        //applicantOrderCart.lstApplicantOrder[0].lstPackages = new List<BackgroundPackagesContract>();

                        foreach (RepeaterItem rptItem in _rptItems)
                        {
                            var _masterTypeCode = (rptItem.FindControl("hdfExcMasterPackageTypeCode") as HiddenField).Value;

                            if (_masterTypeCode == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
                            {
                                #region Add Compliance Data to Session for Bundle
                                applicantOrderCart.BundleInContext = _bundleId;  //UAT-3283
                                applicantOrderCart.CurrentCompliancePackageTypeInContext = CurrentCompliancePackageType = (rptItem.FindControl("hdfExcCompliancePackageTypeCode") as HiddenField).Value;
                                SetOrderCartDataForBundleExclusive(rptItem);

                                #endregion
                            }
                            else
                            {
                                #region Add Background Package to Session for Bundles

                                Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfExcPackageId") as HiddenField).Value);
                                Int32 _packageHierarchyMappingId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnExcBPHMId") as HiddenField).Value);
                                Int32 _packageMaxNumberOfYearforResidence = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnExcMaxResidence") as HiddenField).Value);
                                Decimal _packageBasePrice = Convert.ToDecimal(((rptItem as RepeaterItem).FindControl("hdfExcBkgPackageBasePrice") as HiddenField).Value);
                                //UAT-3268
                                Boolean _isReqToQualifyInRotation = Convert.ToBoolean(((rptItem as RepeaterItem).FindControl("hdnExcIsReqToQualifyInRotation") as HiddenField).Value);
                                HiddenField hdnAdditionalPrice = ((rptItem as RepeaterItem).FindControl("hdnExcAdditionalPrice") as HiddenField);
                                Decimal _additionalPrice = new Decimal();
                                if (!hdnAdditionalPrice.Value.IsNullOrEmpty())
                                    _additionalPrice = Convert.ToDecimal(hdnAdditionalPrice.Value);
                                BackgroundPackagesContract _bkgPackage = new BackgroundPackagesContract();
                                _bkgPackage.BPAId = _packageId;
                                _bkgPackage.IsExclusive = false;
                                _bkgPackage.BPHMId = _packageHierarchyMappingId;
                                _bkgPackage.BasePrice = _packageBasePrice;
                                _bkgPackage.MaxNumberOfYearforResidence = _packageMaxNumberOfYearforResidence;
                                _bkgPackage.IsReqToQualifyInRotation = _isReqToQualifyInRotation; //UAT-3268
                                _bkgPackage.AdditionalPrice = _additionalPrice;

                                if (applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
                                {
                                    applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                                    ApplicantOrder _applicantOrder = new ApplicantOrder();

                                    if (_applicantOrder.lstPackages.IsNullOrEmpty())
                                    {
                                        _applicantOrder.lstPackages = new List<BackgroundPackagesContract>();
                                    }

                                    _applicantOrder.lstPackages.Add(_bkgPackage);
                                    applicantOrderCart.lstApplicantOrder.Add(_applicantOrder);
                                }
                                else
                                {

                                    applicantOrderCart.lstApplicantOrder[0].lstPackages.Add(_bkgPackage);
                                }

                                if (!_bkgPackage.IsNullOrEmpty())
                                {
                                    applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();
                                }

                                #endregion
                            }
                        }
                        // break;
                    }
                }
                #endregion
            }
            else
            {
                var _isAnyPkgSelected = false;

                #region Add Compliance Data to Session for normal package selection

                foreach (RepeaterItem rptItem in rptImmunizationPackages.Items)
                {
                    if ((rptItem.FindControl("rbtnPkg") as RadioButton).Checked)
                    {
                        var _pkgTypeCode = (rptItem.FindControl("hdfPackageTypeCode") as HiddenField).Value;
                        applicantOrderCart.CurrentCompliancePackageTypeInContext = CurrentCompliancePackageType = _pkgTypeCode;
                        applicantOrderCart.PrevOrderId = CurrentViewContext.PreviousOrderId;

                        _isAnyPkgSelected = true;
                        SetOrderCartData(rptItem);
                        break;
                    }
                }

                foreach (RepeaterItem rptItem in rptAdminstrativePackages.Items)
                {
                    if ((rptItem.FindControl("rbtnPkg") as RadioButton).Checked)
                    {
                        var _pkgTypeCode = (rptItem.FindControl("hdfPackageTypeCode") as HiddenField).Value;
                        applicantOrderCart.CurrentCompliancePackageTypeInContext = CurrentCompliancePackageType = _pkgTypeCode;
                        applicantOrderCart.PrevOrderId = CurrentViewContext.PreviousOrderId;

                        _isAnyPkgSelected = true;
                        SetOrderCartData(rptItem);
                        break;
                    }
                }

                #endregion

                // If this is Change Subscription and No Package was selected 
                // and 'View Details' was clicked, then again set the PreviousOrderId in the Session
                // as it got cleared by 'applicantOrderCart.CompliancePackages = null;', ate start of the function
                // and it results in crash when the screen again loads, when coming back from 'View Details'
                if (CurrentViewContext.OrderType == OrderRequestType.ChangeSubscription.GetStringValue() && !_isAnyPkgSelected)
                {
                    applicantOrderCart.CurrentCompliancePackageTypeInContext = this.CompliancePackageType;
                    applicantOrderCart.PrevOrderId = CurrentViewContext.PreviousOrderId;
                }

                applicantOrderCart.SettleAmount = CurrentViewContext.SettlementPrice;
            }
        }

        private void SetOrderCartData(RepeaterItem rptItem)
        {
            var _rbtnListSubscriptions = (rptItem.FindControl("rbtnSubscriptions") as RadioButtonList);

            Int32 _dppsId = Convert.ToInt32(_rbtnListSubscriptions.SelectedValue);
            var _dppId = Convert.ToInt32((rptItem.FindControl("hdfDPPId") as HiddenField).Value);
            var _selectedDeptProgramPackage = CurrentViewContext.DeptProgramPackages.Where(dpp => dpp.DPP_ID == _dppId).First();
            //var _dpmId = Convert.ToInt32((rptItem.FindControl("hdfDPMId") as HiddenField).Value);

            applicantOrderCart.DPMId = _selectedDeptProgramPackage.DeptProgramMapping.DPM_ID;
            applicantOrderCart.lstDepProgramMappingId = CurrentViewContext.SelectedProgramIds;

            Int32? _duration = AppConsts.NONE;

            if (_selectedDeptProgramPackage.DeptProgramMapping.InstitutionNode.IsNull())
            {
                var _dpp = Presenter.GetDeptProgramPackageById(_dppId, TenantId);
                _duration = _dpp.DeptProgramMapping.InstitutionNode.IN_Duration;
            }
            else
            {
                _duration = _selectedDeptProgramPackage.DeptProgramMapping.InstitutionNode.IN_Duration;
            }

            applicantOrderCart.ProgramDuration = _duration;
            applicantOrderCart.IsCompliancePackageSelected = true;
            applicantOrderCart.DPP_Id = _dppId;
            applicantOrderCart.CompliancePackageID = _selectedDeptProgramPackage.DPP_CompliancePackageID;

            Decimal _actualPrice = 0;
            Decimal _netPrice = 0;

            DeptProgramPackageSubscription _selectedDpps = _selectedDeptProgramPackage.DeptProgramPackageSubscriptions.Where(dpps => dpps.DPPS_ID == _dppsId).First();
            GetPricing(_selectedDpps, out _netPrice, out _actualPrice);

            if (CurrentViewContext.PreviousOrderId > 0 && applicantOrderCart.OrderRequestType.IsNotNull()
                && applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue())
            {
                applicantOrderCart.Amount = Convert.ToString(_netPrice);
                applicantOrderCart.GrandTotal = _netPrice;
            }
            else
            {
                applicantOrderCart.Amount = Convert.ToString(_actualPrice);
                applicantOrderCart.GrandTotal = _netPrice;

                // In case of Change Program, it will be filled from that screen 
                applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();
            }

            applicantOrderCart.CurrentPackagePrice = _actualPrice;

            //UAT-360:WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
            //Label ctrlLblRushOrderPrice = (Label)FindControl("lblRushOrderPrice" + controlSuffix);
            //applicantOrderCart.RushOrderPrice = ctrlLblRushOrderPrice.Text.IsNullOrEmpty() ? null : ctrlLblRushOrderPrice.Text.Substring(1);

            applicantOrderCart.DPPS_ID = _dppsId;
        }

        private void SetOrderCartDataForBundleExclusive(RepeaterItem rptItem)
        {

            var _rbtnListSubscriptions = (rptItem.FindControl("rbtnExcSubscriptions") as RadioButtonList);

            Int32 _dppsId = Convert.ToInt32(_rbtnListSubscriptions.SelectedValue);
            var _dppId = Convert.ToInt32((rptItem.FindControl("hdfExcDPPId") as HiddenField).Value);
            var _selectedDeptProgramPackage = CurrentViewContext.DeptProgramPackages.Where(dpp => dpp.DPP_ID == _dppId).First();
            //var _dpmId = Convert.ToInt32((rptItem.FindControl("hdfDPMId") as HiddenField).Value);

            applicantOrderCart.DPMId = _selectedDeptProgramPackage.DeptProgramMapping.DPM_ID;
            applicantOrderCart.lstDepProgramMappingId = CurrentViewContext.SelectedProgramIds;

            Int32? _duration = AppConsts.NONE;

            if (_selectedDeptProgramPackage.DeptProgramMapping.InstitutionNode.IsNull())
            {
                var _dpp = Presenter.GetDeptProgramPackageById(_dppId, TenantId);
                _duration = _dpp.DeptProgramMapping.InstitutionNode.IN_Duration;
            }
            else
            {
                _duration = _selectedDeptProgramPackage.DeptProgramMapping.InstitutionNode.IN_Duration;
            }

            applicantOrderCart.ProgramDuration = _duration;
            applicantOrderCart.IsCompliancePackageSelected = true;
            applicantOrderCart.DPP_Id = _dppId;
            applicantOrderCart.CompliancePackageID = _selectedDeptProgramPackage.DPP_CompliancePackageID;

            Decimal _actualPrice = 0;
            Decimal _netPrice = 0;

            DeptProgramPackageSubscription _selectedDpps = _selectedDeptProgramPackage.DeptProgramPackageSubscriptions.Where(dpps => dpps.DPPS_ID == _dppsId).First();
            GetPricing(_selectedDpps, out _netPrice, out _actualPrice);

            if (CurrentViewContext.PreviousOrderId > 0 && applicantOrderCart.OrderRequestType.IsNotNull()
                && applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue())
            {
                applicantOrderCart.Amount = Convert.ToString(_netPrice);
                applicantOrderCart.GrandTotal = _netPrice;
            }
            else
            {
                applicantOrderCart.Amount = Convert.ToString(_actualPrice);
                applicantOrderCart.GrandTotal = _netPrice;

                // In case of Change Program, it will be filled from that screen 
                applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();
            }

            applicantOrderCart.CurrentPackagePrice = _actualPrice;

            //UAT-360:WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
            //Label ctrlLblRushOrderPrice = (Label)FindControl("lblRushOrderPrice" + controlSuffix);
            //applicantOrderCart.RushOrderPrice = ctrlLblRushOrderPrice.Text.IsNullOrEmpty() ? null : ctrlLblRushOrderPrice.Text.Substring(1);

            applicantOrderCart.DPPS_ID = _dppsId;
        }


        /// <summary>
        /// Add the data of Background Packages to the Session cart, for nlrmal package selection
        /// </summary>
        private void AddBackgroundPackageDataToSession()
        {

            if (!divBackgroundPackages.Visible)
            {
                if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
                {
                    applicantOrderCart.lstApplicantOrder[0].lstPackages = null;
                }
                return;
            }

            List<BackgroundPackagesContract> _lstBackgroundPackages = new List<BackgroundPackagesContract>();
            String BkgServiceCode = string.Empty;
            #region Generate Exclusive and Non Exclusive Packages' List

            //// TEMPORARY
            //if (applicantOrderCart.GrandTotal.IsNullOrEmpty())
            //    applicantOrderCart.GrandTotal = 0;           
            foreach (var rptItem in rptNonExclusive.Items)
            {
                Int32 CopiesCount = 0;
                //bool IsOrderHistory = false;

                //Int32? ReferencedOrderID = 0;
                CheckBox _chkNonExclusive = ((rptItem as RepeaterItem).FindControl("chkNonExc") as CheckBox);
                if (_chkNonExclusive.IsNotNull() && _chkNonExclusive.Checked)
                {
                    Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnNonExcBPAId") as HiddenField).Value);
                    Int32 _packageHierarchyMappingId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnNonExcBPHMId") as HiddenField).Value);
                    Int32 _packageMaxNumberOfYearforResidence = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnMaxResidence") as HiddenField).Value);
                    Decimal _packageBasePrice = Convert.ToDecimal(((rptItem as RepeaterItem).FindControl("hdfNonExcBasePrice") as HiddenField).Value);
                    //UAT-3268
                    Boolean _isReqToQualifyInRotation = Convert.ToBoolean(((rptItem as RepeaterItem).FindControl("hdnIsReqToQualifyInRotation") as HiddenField).Value);
                    HiddenField hdnAdditionalPrice = ((rptItem as RepeaterItem).FindControl("hdnAdditionalPrice") as HiddenField);
                    //hdnPackagePasscode
                    String txtPackagePasscode = ((rptItem as RepeaterItem).FindControl("txtNonExcPackagePasscode") as WclTextBox).Text; //UAT-3771
                    if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                    {
                        CopiesCount = ((rptItem as RepeaterItem).FindControl("txtNoOfCopies") as TextBox).Text.IsNullOrEmpty() ? 0 : Convert.ToInt32(((rptItem as RepeaterItem).FindControl("txtNoOfCopies") as TextBox).Text);
                        //IsOrderHistory = ((rptItem as RepeaterItem).FindControl("chkIsOrderHistory") as CheckBox).Checked.IsNullOrEmpty() ? false : ((rptItem as RepeaterItem).FindControl("chkIsOrderHistory") as CheckBox).Checked;
                        BkgServiceCode = ((rptItem as RepeaterItem).FindControl("lblServiceCode") as Label).Text;
                        // ReferencedOrderID = ((rptItem as RepeaterItem).FindControl("btnViewImages") as RadButton).Attributes["OrderId"].IsNotNull() ? Convert.ToInt32(((rptItem as RepeaterItem).FindControl("btnViewImages") as RadButton).Attributes["OrderId"]) : 0;
                    }
                    IsNonExecPasscodeMatched = CheckPackagePasscode(_packageId, txtPackagePasscode, false);
                    //if (!IsNonExecPasscodeMatched)
                    //{
                    //    // base.ShowInfoMessage("Passcode does not match. Please provide valid passcode.");
                    //    break;
                    //}
                    //else
                    //{
                    Decimal _additionalPrice = new Decimal();
                    if (!hdnAdditionalPrice.Value.IsNullOrEmpty())
                        _additionalPrice = Convert.ToDecimal(hdnAdditionalPrice.Value);
                    _lstBackgroundPackages.Add(new BackgroundPackagesContract
                    {
                        BPAId = _packageId,
                        IsExclusive = false,
                        BPHMId = _packageHierarchyMappingId,
                        BasePrice = _packageBasePrice,
                        MaxNumberOfYearforResidence = _packageMaxNumberOfYearforResidence,
                        IsReqToQualifyInRotation = _isReqToQualifyInRotation,
                        AdditionalPrice = _additionalPrice,
                        PackagePasscode = txtPackagePasscode,
                        CopiesCount = CurrentViewContext.FingerPrintData.IsLocationServiceTenant ? CopiesCount : 0,
                        //IsOrderHistory = CurrentViewContext.FingerPrintData.IsLocationServiceTenant ? IsOrderHistory : false,
                        ServiceCode = CurrentViewContext.FingerPrintData.IsLocationServiceTenant ? BkgServiceCode : String.Empty,
                        //Needs to be changed 
                        ReferencedOrderID = CurrentViewContext.FingerPrintData.ArchivedOrderID,
                    });
                    // }
                    // applicantOrderCart.GrandTotal += _packageBasePrice;
                }
            }

            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                Presenter.GetAdditionalServiceFeeOption();
            }

            foreach (var rptItem in rptExclusive.Items)
            {
                Int32 CopiesCount = 0;
                Int32 PPCopiesCount = 0;
                Decimal? _fCAdditionalPrice = 0;
                Decimal? _pPAdditionalPrice = 0;
                RadioButton _rBtnExclusive = ((rptItem as RepeaterItem).FindControl("rbtnExclusive") as RadioButton);
                if (_rBtnExclusive.IsNotNull() && _rBtnExclusive.Checked)
                {
                    Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnExcBPAId") as HiddenField).Value);
                    Int32 _packageHierarchyMappingId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnExcBPHMId") as HiddenField).Value);
                    Int32 _packageMaxNumberOfYearforResidence = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnMaxResidence") as HiddenField).Value);

                    Decimal _packageBasePrice = Convert.ToDecimal(((rptItem as RepeaterItem).FindControl("hdfExcBasePrice") as HiddenField).Value);
                    //UAT-3268
                    Boolean _isReqToQualifyInRotation = Convert.ToBoolean(((rptItem as RepeaterItem).FindControl("hdnIsReqToQualifyInRotation") as HiddenField).Value);
                    HiddenField hdnAdditionalPrice = ((rptItem as RepeaterItem).FindControl("hdnAdditionalPrice") as HiddenField);

                    String txtPackagePasscode = ((rptItem as RepeaterItem).FindControl("txtPackagePasscode") as WclTextBox).Text; //UAT-3771
                    if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                    {
                        CopiesCount = ((rptItem as RepeaterItem).FindControl("txtNoOfCopies") as TextBox).Text.IsNullOrEmpty() ? 0 : Convert.ToInt32(((rptItem as RepeaterItem).FindControl("txtNoOfCopies") as TextBox).Text);
                        BkgServiceCode = ((rptItem as RepeaterItem).FindControl("lblServiceCodeExc") as Label).Text;
                        if(BkgServiceCode == "AAAQ")
                        {
                            _fCAdditionalPrice = CurrentViewContext.lstAdditionalServiceFeeOption.Where(x => x.FieldValue == "AAAA").Select(x => x.SIFR_Amount).FirstOrDefault();
                        }
                        if (BkgServiceCode == "AAAR")
                        {
                            PPCopiesCount = ((rptItem as RepeaterItem).FindControl("txtNoOfPPCopies") as TextBox).Text.IsNullOrEmpty() ? 0 : Convert.ToInt32(((rptItem as RepeaterItem).FindControl("txtNoOfPPCopies") as TextBox).Text);
                            _fCAdditionalPrice = CurrentViewContext.lstAdditionalServiceFeeOption.Where(x => x.FieldValue == "AAAA").Select(x => x.SIFR_Amount).FirstOrDefault();
                            _pPAdditionalPrice = CurrentViewContext.lstAdditionalServiceFeeOption.Where(x => x.FieldValue == "AAAB").Select(x => x.SIFR_Amount).FirstOrDefault();
                        }
                    }

                    

                    IsExecPasscodeMatched = CheckPackagePasscode(_packageId, txtPackagePasscode, true);

                    //if (!IsExecPasscodeMatched)
                    //{
                    //    //  base.ShowInfoMessage("Passcode does not match. Please provide valid passcode.");
                    //    break;
                    //}
                    //else
                    //{
                    Decimal _additionalPrice = new Decimal();
                    if (!hdnAdditionalPrice.Value.IsNullOrEmpty())
                        _additionalPrice = Convert.ToDecimal(hdnAdditionalPrice.Value);

                    _lstBackgroundPackages.Add(new BackgroundPackagesContract
                    {
                        BPAId = _packageId,
                        IsExclusive = true,
                        BPHMId = _packageHierarchyMappingId,
                        BasePrice = _packageBasePrice,
                        MaxNumberOfYearforResidence = _packageMaxNumberOfYearforResidence,
                        IsReqToQualifyInRotation = _isReqToQualifyInRotation,
                        AdditionalPrice = _additionalPrice,
                        PackagePasscode = txtPackagePasscode, //UAT-3771
                        ServiceCode = CurrentViewContext.FingerPrintData.IsLocationServiceTenant ? BkgServiceCode : String.Empty,
                        CopiesCount = CurrentViewContext.FingerPrintData.IsLocationServiceTenant ? CopiesCount : 0,
                        FCAdditionalPrice = _fCAdditionalPrice,
                        PPAdditionalPrice = _pPAdditionalPrice,
                        PPCopiesCount = PPCopiesCount,
                    });

                    //applicantOrderCart.GrandTotal += _packageBasePrice;
                    if (CurrentViewContext.lstSelectedBundlePkgId.IsNullOrEmpty() || CurrentViewContext.lstSelectedBundlePkgId.Count == AppConsts.NONE)//UAT-3283
                                                                                                                                                       //if (CurrentViewContext.SelectedPkgBundleId == AppConsts.NONE)
                        break;
                    //}

                }
            }

            #endregion

            //if (!IsExecPasscodeMatched && !IsNonExecPasscodeMatched) //UAT: 4644
            //    return;

            ApplicantOrder _applicantOrder = new ApplicantOrder();
            if (applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
            {
                applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                applicantOrderCart.lstApplicantOrder.Add(new ApplicantOrder
                {
                    lstPackages = _lstBackgroundPackages,
                });
            }
            else
            {
                if (applicantOrderCart.IsLocationServiceTenant 
                    && applicantOrderCart.lstApplicantOrder[0].lstPackages != null
                    && applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > 0
                    && _lstBackgroundPackages != null
                    && _lstBackgroundPackages.Count> 0 )
                {
                    IsCabsFreshOrder(applicantOrderCart.lstApplicantOrder[0].lstPackages, _lstBackgroundPackages);
                }
                applicantOrderCart.lstApplicantOrder[0].lstPackages = _lstBackgroundPackages;
            }

            if (_lstBackgroundPackages.Count() > 0)
            {
                applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();
            }
        }

        private void IsCabsFreshOrder(List<BackgroundPackagesContract> OldBackgroundPackages, 
            List<BackgroundPackagesContract> NewBackgroundPackages)
        {
            int CabsAvaliableInNew = NewBackgroundPackages.Where(x => x.ServiceCode == BkgServiceType.SIMPLE.GetStringValue()).Count();
            int CabsAvaliableInOld = OldBackgroundPackages.Where(x => x.ServiceCode == BkgServiceType.SIMPLE.GetStringValue()).Count();
            if (CabsAvaliableInNew > 0 && CabsAvaliableInOld==0)
            {
                applicantOrderCart.IscabsFreshSelected = true;
            }

            else
            {
                applicantOrderCart.IscabsFreshSelected = false;
            }

        }
        #region UAT-3771
        private Boolean IsPackagePasscodeMatched(Int32 packageId, String packagePasscode, Boolean IsExclusive)
        {
            Boolean result = false;
            if (!packagePasscode.IsNullOrEmpty())
            {
                var lstPackages = lstBackgroundPackages.Where(x => x.BPAId == packageId && x.PackagePasscode == packagePasscode && x.IsExclusive == IsExclusive).FirstOrDefault();
                if (!lstPackages.IsNullOrEmpty())
                {
                    result = lstPackages.PackagePasscode.IsNullOrEmpty() ? false : true;
                }
            }
            else
            {
                var lstPackages = lstBackgroundPackages.Where(x => x.BPAId == packageId).FirstOrDefault();
                if (!lstPackages.IsNullOrEmpty())
                {
                    result = lstPackages.PackagePasscode.IsNullOrEmpty() ? false : true;
                }
            }
            return result;
        }


        private Boolean CheckPackagePasscode(Int32 packageId, String packagePasscode, Boolean IsExclusive)
        {
            Boolean result = false;
            if (!packagePasscode.IsNullOrEmpty())
            {
                var lstPackages = lstBackgroundPackages.Where(x => x.BPAId == packageId && x.PackagePasscode == packagePasscode && x.IsExclusive == IsExclusive).FirstOrDefault();
                if (!lstPackages.IsNullOrEmpty())
                {
                    result = lstPackages.PackagePasscode.IsNullOrEmpty() ? false : true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                var lstPackages = lstBackgroundPackages.Where(x => x.BPAId == packageId).FirstOrDefault();
                if (!lstPackages.IsNullOrEmpty())
                {
                    String passcode = lstPackages.PackagePasscode;
                    if (packagePasscode.IsNullOrEmpty() && passcode.IsNullOrEmpty())
                    {
                        result = true;
                    }
                    else
                    {
                        result = passcode.IsNullOrEmpty() ? true : false;
                    }
                }

            }
            return result;
        }

        #endregion

        public Boolean IsFreshOrder(ApplicantOrderCart applicantOrderCart)
        {
            Boolean result = true;

            if (applicantOrderCart.IsNotNull() && applicantOrderCart.alNodeIds.IsNotNull())
            {
                result = false;
            }
            else if (applicantOrderCart.IsNotNull() && applicantOrderCart.DefaultNodeId.IsNotNull())
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Returns true if any packages is available for the selected node
        /// </summary>
        /// <param name="isViewDetail"></param>
        /// <returns></returns>
        //private void BindPackageDetails(Boolean isViewDetail = false)
        private void BindPackageDetails()
        {
            WclComboBox cntrl = (WclComboBox)FindControl("ddlDeptprogramPkg" + controlSuffix);
            if (CurrentViewContext.DeptProgramPackage.IsNotNull() && cntrl.SelectedValue != AppConsts.ZERO)
            {
                HtmlGenericControl ctrlDivSubscriptions = (HtmlGenericControl)FindControl("divSubscriptions" + controlSuffix);
                if (ctrlDivSubscriptions.IsNotNull())
                    ctrlDivSubscriptions.Visible = true;
                /*ShowHideCompliancePackage();*/
                Label ctrlLblPackageName = (Label)FindControl("lblPackageName" + controlSuffix);
                if (ctrlLblPackageName.IsNotNull())
                    ctrlLblPackageName.Text = String.IsNullOrEmpty(CurrentViewContext.DeptProgramPackage.CompliancePackage.PackageLabel) ?
                        CurrentViewContext.DeptProgramPackage.CompliancePackage.PackageName : CurrentViewContext.DeptProgramPackage.CompliancePackage.PackageLabel;

                Label ctrlLblDescription = (Label)FindControl("lblDescription" + controlSuffix);
                if (ctrlLblDescription.IsNotNull())
                    ctrlLblDescription.Text = CurrentViewContext.DeptProgramPackage.CompliancePackage.Description.HtmlEncode();

                Literal ctrlLitCompPackageDetail = (Literal)FindControl("litCompPackageDetail" + controlSuffix);
                if (ctrlLitCompPackageDetail.IsNotNull())
                    ctrlLitCompPackageDetail.Text = (String.IsNullOrEmpty(CurrentViewContext.DeptProgramPackage.CompliancePackage.PackageDetail) ?
                        String.Empty : CurrentViewContext.DeptProgramPackage.CompliancePackage.PackageDetail);



                Guid subscriptionOptionCode = new Guid(SubscriptionOptions.CustomMonthly.GetStringValue());
                List<DeptProgramPackageSubscription> deptProgramPackageSubscriptions = CurrentViewContext.DeptProgramPackage.DeptProgramPackageSubscriptions.
                    Where(cond => cond.SubscriptionOption.IsSystem == false && cond.SubscriptionOption.Code != subscriptionOptionCode &&
                        cond.DPPS_TotalPrice != null && !cond.DPPS_IsDeleted).ToList(); // && cond.DPPS_TotalPrice > 0 Include 0 price packages [UAT-360]: WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00

                //Set ddlSubscription DataSource when view Detail button is not clicked
                //if (!isViewDetail)
                //{
                WclComboBox cntrlDdlSubscriptions = (WclComboBox)FindControl("ddlSubscriptions" + controlSuffix);
                cntrlDdlSubscriptions.DataSource = deptProgramPackageSubscriptions.Select(x => x.SubscriptionOption).ToList();
                cntrlDdlSubscriptions.DataBind();
                //}
                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);

                if (applicantOrderCart != null
                    && applicantOrderCart.lstApplicantOrder != null
                    && applicantOrderCart.lstApplicantOrder.Count > 0
                    && applicantOrderCart.lstApplicantOrder[0].DPPS_Id != null
                    && applicantOrderCart.lstApplicantOrder[0].DPPS_Id.Count > 0
                    && deptProgramPackageSubscriptions != null && applicantOrderCart.lstApplicantOrder[0].OrderId == 0)
                {
                    //Update Cart if clicked on View Detail button
                    //if (isViewDetail)
                    //{
                    //    applicantOrderCart.lstApplicantOrder[0].DPPS_Id[0] = Convert.ToInt32(ddlSubscription.SelectedValue);
                    //}
                    DeptProgramPackageSubscription deptProgramPackageSubscription =
                        deptProgramPackageSubscriptions.FirstOrDefault(x => x.DPPS_ID == applicantOrderCart.DPPS_ID);//applicantOrderCart.lstApplicantOrder[0].DPPS_Id[0]
                    if (deptProgramPackageSubscription != null)
                    {
                        SetPackageDetails(deptProgramPackageSubscription);
                        //SetDepartmentControl(false);
                    }
                    cmdBar.SubmitButtonText = "Continue Order";
                }
                //else if (deptProgramPackageSubscriptions != null && deptProgramPackageSubscriptions.Count > 0 && isViewDetail == false)
                else if (deptProgramPackageSubscriptions != null && deptProgramPackageSubscriptions.Count > 0)
                {
                    SetPackageDetails(deptProgramPackageSubscriptions[0]);
                    //SetDepartmentControl(false);
                }
                lblPendingOrder.Visible = false;
                //return true;
            }
            else
            {
                HtmlGenericControl cntrlDivSubscriptions = (HtmlGenericControl)FindControl("divSubscriptions" + controlSuffix);
                if (cntrlDivSubscriptions.IsNotNull())
                    cntrlDivSubscriptions.Visible = false;
            }
            //else if (IsPackageSubscribe)
            //{
            //    ShowHideSubscriptionContinueButton(false);

            //    if (applicantOrderCart.IsNull())
            //    {
            //        base.ShowInfoMessage("An active subscription has been found for the criteria you have selected.");
            //        btnGo.Enabled = true;
            //    }
            //    //return true;
            //}
            //else
            //{
            //    ShowHideSubscriptionContinueButton(false);
            //    if (applicantOrderCart.IsNull())
            //    {
            //        base.ShowInfoMessage("No Subscription for the criteria you have selected.");
            //        btnGo.Enabled = true;
            //    }
            //}
        }

        private void SetPackageDetails(DeptProgramPackageSubscription deptProgramPackageSubscription, Boolean IsSubscriptionChanged = false)
        {
            Label cntrlLblSubDescription = (Label)FindControl("lblSubDescription" + controlSuffix);
            if (cntrlLblSubDescription.IsNotNull())
                cntrlLblSubDescription.Text = deptProgramPackageSubscription.SubscriptionOption.Description.HtmlEncode();

            Decimal totalPrice = deptProgramPackageSubscription.DPPS_TotalPrice == null ? 0 : deptProgramPackageSubscription.DPPS_TotalPrice.Value;
            Decimal rushOrderPrice = deptProgramPackageSubscription.DPPS_RushOrderAdditionalPrice == null ? 0 : deptProgramPackageSubscription.DPPS_RushOrderAdditionalPrice.Value;

            Label cntrlLblPrice = (Label)FindControl("lblPrice" + controlSuffix);
            if (cntrlLblSubDescription.IsNotNull())
                cntrlLblPrice.Text = String.Format("${0}", totalPrice.ToString("0.00"));
            //UAT-360:WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
            if (deptProgramPackageSubscription.DPPS_RushOrderAdditionalPrice.IsNotNull())
            {
                Label cntrlLblRushOrderPrice = (Label)FindControl("lblRushOrderPrice" + controlSuffix);
                if (cntrlLblSubDescription.IsNotNull())
                    cntrlLblRushOrderPrice.Text = String.Format("${0}", rushOrderPrice.ToString("0.00"));
            }

            //If ddlSubscription selected index is changed then no need to set ddlSubscription selected value
            WclComboBox cntrlDdlSubscriptions = (WclComboBox)FindControl("ddlSubscriptions" + controlSuffix);
            if (IsSubscriptionChanged == false)
            {
                cntrlDdlSubscriptions.SelectedValue = deptProgramPackageSubscription.DPPS_SubscriptionID.ToString();
            }

            Label cntrlLblNetPrice = (Label)FindControl("lblNetPrice" + controlSuffix);
            if (CurrentViewContext.PreviousOrderId != 0 && CurrentViewContext.SettlementPrice != 0)
            {
                Decimal _netPrice = totalPrice - CurrentViewContext.SettlementPrice;
                if (_netPrice <= 0)
                {
                    //UAT 264
                    //_netPrice = 1;
                    _netPrice = 0;
                }

                Panel cntrlPnlChangeOrder = (Panel)FindControl("pnlChangeOrder" + controlSuffix);
                if (cntrlPnlChangeOrder.IsNotNull())
                    cntrlPnlChangeOrder.Visible = true;

                HtmlGenericControl cntrlDvChngSubsPriceSum = (HtmlGenericControl)FindControl("dvChngSubsPriceSum" + controlSuffix);
                if (cntrlDvChngSubsPriceSum.IsNotNull())
                    cntrlDvChngSubsPriceSum.Visible = false;

                if (cntrlLblNetPrice.IsNotNull())
                    cntrlLblNetPrice.Text = String.Format("${0}", _netPrice.ToString("0.00"));

                Label cntrlLblSetlementPrice = (Label)FindControl("lblSetlementPrice" + controlSuffix);
                if (cntrlLblSetlementPrice.IsNotNull())
                    cntrlLblSetlementPrice.Text = String.Format("${0}", CurrentViewContext.SettlementPrice.ToString("0.00"));
            }
            else
            {
                if (cntrlLblNetPrice.IsNotNull())
                    cntrlLblNetPrice.Text = String.Format("${0}", totalPrice.ToString("0.00"));
            }

            ////List<DeptProgramPackagePaymentOption> lstDeptProgramPackagePaymentOptions = !deptProgramPackageSubscription.DeptProgramPackage.IsNullOrEmpty() ?
            ////                                           deptProgramPackageSubscription.DeptProgramPackage.DeptProgramPackagePaymentOptions.Where(cond => !cond.DPPPO_IsDeleted && !cond.DeptProgramPackage.DPP_IsDeleted).ToList()
            ////                                           : new List<DeptProgramPackagePaymentOption>();


            ////List<lkpPaymentOption> lstPaymentOptions = !lstDeptProgramPackagePaymentOptions.IsNullOrEmpty() ?
            ////                                           lstDeptProgramPackagePaymentOptions.Select(col => col.lkpPaymentOption).ToList()
            ////                                           : new List<lkpPaymentOption>();
            ////if (lstPaymentOptions.IsNotNull() && lstPaymentOptions.Count > 0)
            ////{
            ////    if (lstPaymentOptions.Count == 1)
            ////    {
            ////        if (lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
            ////        {
            ////            pnlChangeOrder.Visible = false;
            ////            dvChngSubsPriceSum.Visible = false;
            ////        }
            ////        else
            ////        {
            ////            ShowHideNetPrice();
            ////        }
            ////    }
            ////    else if (lstPaymentOptions.Count == 2)
            ////    {
            ////        if (lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
            ////        {
            ////            pnlChangeOrder.Visible = false;
            ////            dvChngSubsPriceSum.Visible = false;
            ////        }
            ////        else
            ////        {
            ////            ShowHideNetPrice();
            ////        }
            ////    }
            ////    else
            ////    {
            ////        ShowHideNetPrice();
            ////    }
            ////}
            ////else if (IfInvoiceOnlyPymnOptn)
            ////{
            ////    pnlChangeOrder.Visible = false;
            ////    dvChngSubsPriceSum.Visible = false;
            ////}
            ////else
            ////{
            ////    ShowHideNetPrice();
            ////}

        }

        /// <summary>
        /// Method to Show/Hide Net price section according to Subscription (New/change)
        /// </summary>
        private void ShowHideNetPrice()
        {
            ////if (CurrentViewContext.PreviousOrderId != 0 && CurrentViewContext.SettlementPrice != 0)
            ////{
            ////    pnlChangeOrder.Visible = true;
            ////}
            ////else
            ////{
            ////    dvChngSubsPriceSum.Visible = true;
            ////}
        }

        /// <summary>
        /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void RedirectIfIncorrectOrderStage(ApplicantOrderCart applicantOrderCart)
        {
            if (IsFreshOrder(applicantOrderCart) == false)
            {
                Presenter.GetNextPagePathByOrderStageID(applicantOrderCart);

                //Redirect to next page path if Order Status track is not correct.
                if (CurrentViewContext.NextPagePath.IsNotNull())
                {
                    Response.Redirect(CurrentViewContext.NextPagePath);
                }

                else if (applicantOrderCart.SettleAmount == 0 && applicantOrderCart.PrevOrderId == 0)
                {
                    applicantOrderCart.AddOrderStageTrackID(OrderStages.PendingOrder);
                }
            }
        }

        private void BindHierarchyNode(WclComboBox targetCombobox, Literal targetLiteral, System.Web.UI.HtmlControls.HtmlGenericControl targetDiv
                                        , Int32 hierarchyLevel, RequiredFieldValidator rfvLevel)
        {
            targetCombobox.Items.Clear();

            if (CurrentViewContext.lstHierarchy.IsNotNull() && CurrentViewContext.lstHierarchy.Count() > 0)
            {
                foreach (var node in CurrentViewContext.lstHierarchy)
                {
                    //if (node.InstitutionNode.InstitutionNodeType.INT_Code.ToLower() == NodeType.Program.GetStringValue().ToLower() && !_isProgram)
                    //    _isProgram = false;

                    RadComboBoxItem _comboItem = new RadComboBoxItem();
                    if (node.InstitutionNode.IN_Label.IsNullOrEmpty())
                    {
                        _comboItem.Text = node.InstitutionNode.IN_Name;
                    }
                    else
                    {
                        _comboItem.Text = node.InstitutionNode.IN_Label;
                    }
                    _comboItem.Value = Convert.ToString(node.DPM_ID);
                    targetCombobox.Items.Add(_comboItem);
                    //targetLiteral.Text = "Select " + node.InstitutionNode.InstitutionNodeType.INT_Name;
                    targetLiteral.Text = Resources.Language.SELECT + " " + node.InstitutionNode.InstitutionNodeType.INT_Name.HtmlEncode();
                    //rfvLevel.ErrorMessage = "Please select " + node.InstitutionNode.InstitutionNodeType.INT_Name;
                    rfvLevel.ErrorMessage = Resources.Language.PLEASESELECT + " " + node.InstitutionNode.InstitutionNodeType.INT_Name;

                    rfvLevel.Enabled = true;
                    //If targetDiv is Null then this is first dropdown else others
                    //if (targetDiv.IsNull())
                    //{
                    //    divNode1Literal.Attributes.Add("title", "Select the " + node.InstitutionNode.InstitutionNodeType.INT_Name.ToLower() + " for which you need a package");
                    //}
                    //else                    
                    //{
                    //    targetDiv.Attributes.Add("title", "Select the " + node.InstitutionNode.InstitutionNodeType.INT_Name.ToLower() + " for which you need a package");
                    //}

                    if (targetDiv.IsNull())
                    {
                        divNode1Literal.Attributes.Add("title", Resources.Language.SELECTTHE + " " + node.InstitutionNode.InstitutionNodeType.INT_Name.ToLower() + " " + Resources.Language.FORWHICHYOUNEEDPKG);
                    }
                    else
                    {
                        targetDiv.Attributes.Add("title", Resources.Language.SELECTTHE + " " + node.InstitutionNode.InstitutionNodeType.INT_Name.ToLower() + " " + Resources.Language.FORWHICHYOUNEEDPKG);
                    }
                }

                targetCombobox.Items.Insert(0, new RadComboBoxItem
                {
                    Text = AppConsts.COMBOBOX_ITEM_SELECT,
                    Value = AppConsts.ZERO,
                });
                //EnableLoadPackages(false);
                ManageControls(targetCombobox, targetLiteral, targetDiv, true);
            }
            else
            {
                //EnableLoadPackages(true);
                ManageControls(targetCombobox, targetLiteral, targetDiv, false);
                ManageValidations(hierarchyLevel);
            }

            if (!String.IsNullOrEmpty(CurrentViewContext.ChangeSubscriptionTargetNodeId) && CurrentViewContext.ChangeSubscriptionTargetNodeId != AppConsts.ZERO)
            {
                targetCombobox.SelectedValue = CurrentViewContext.ChangeSubscriptionTargetNodeId;
                CallComboboxEvent(hierarchyLevel);
            }
        }

        /// <summary>
        /// Reset the Validation of the Controls which are ahead of the control for which SelectedValue == 0
        /// </summary>
        /// <param name="levelId"></param>
        private void ManageValidations(Int32 levelId)
        {
            switch (levelId)
            {
                case 2:
                    {
                        ClearValidation(rfvLevel2);
                        ClearValidation(rfvLevel3);
                        ClearValidation(rfvLevel4);
                        ClearValidation(rfvLevel5);
                        ClearValidation(rfvLevel6);
                        ClearValidation(rfvLevel7);
                        ClearValidation(rfvLevel8);
                        ClearValidation(rfvLevel9);
                        ClearValidation(rfvLevel10);
                    }
                    break;
                case 3:
                    {
                        ClearValidation(rfvLevel3);
                        ClearValidation(rfvLevel4);
                        ClearValidation(rfvLevel5);
                        ClearValidation(rfvLevel6);
                        ClearValidation(rfvLevel7);
                        ClearValidation(rfvLevel8);
                        ClearValidation(rfvLevel9);
                        ClearValidation(rfvLevel10);
                    }
                    break;
                case 4:
                    {
                        ClearValidation(rfvLevel4);
                        ClearValidation(rfvLevel5);
                        ClearValidation(rfvLevel6);
                        ClearValidation(rfvLevel7);
                        ClearValidation(rfvLevel8);
                        ClearValidation(rfvLevel9);
                        ClearValidation(rfvLevel10);
                    }
                    break;
                case 5:
                    {
                        ClearValidation(rfvLevel5);
                        ClearValidation(rfvLevel6);
                        ClearValidation(rfvLevel7);
                        ClearValidation(rfvLevel8);
                        ClearValidation(rfvLevel9);
                        ClearValidation(rfvLevel10);
                    }
                    break;
                case 6:
                    {
                        ClearValidation(rfvLevel6);
                        ClearValidation(rfvLevel7);
                        ClearValidation(rfvLevel8);
                        ClearValidation(rfvLevel9);
                        ClearValidation(rfvLevel10);
                    }
                    break;
                case 7:
                    {
                        ClearValidation(rfvLevel7);
                        ClearValidation(rfvLevel8);
                        ClearValidation(rfvLevel9);
                        ClearValidation(rfvLevel10);
                    }
                    break;
                case 8:
                    {
                        ClearValidation(rfvLevel8);
                        ClearValidation(rfvLevel9);
                        ClearValidation(rfvLevel10);
                    }
                    break;
                case 9:
                    {
                        ClearValidation(rfvLevel9);
                        ClearValidation(rfvLevel10);
                    }
                    break;
                case 10:
                    {
                        ClearValidation(rfvLevel10);
                    }
                    break;
            }
        }

        private void ClearValidation(RequiredFieldValidator rfValidator)
        {
            rfValidator.Enabled = false;
            rfValidator.ErrorMessage = String.Empty;
        }

        /// <summary>
        /// Show hide the dropdowns
        /// </summary>
        /// <param name="targetCombobox"></param>
        /// <param name="targetLiteral"></param>
        /// <param name="targetDiv"></param>
        /// <param name="visibility"></param>
        private void ManageControls(WclComboBox targetCombobox, Literal targetLiteral, HtmlGenericControl targetDiv, Boolean visibility)
        {
            if (targetDiv.IsNotNull())
                targetDiv.Visible = visibility;

            targetLiteral.Visible = targetCombobox.Visible = visibility;

            // if (targetCombobox.ID != cmbLevel10.ID)
            targetCombobox.AutoPostBack = visibility;
            //   else
            //     targetCombobox.AutoPostBack = false;

            targetCombobox.CausesValidation = visibility;

            if (!visibility && targetCombobox.CheckBoxes)
                hdfPackageCombo.Value = String.Empty;
        }

        /// <summary>
        /// Manage the 'Enable/Disable' of the 'Load Packages' button
        /// UAT 1202 - As an applicant, I should only be able to load packages and order from a bottom-level node. 
        /// </summary>
        /// <param name="isEnabled"></param>
        private void EnableLoadPackages(Boolean isEnabled)
        {
            btnGo.Enabled = isEnabled;
        }

        /// <summary>
        /// Clears package selection data from cart. useful in case of edit subscription
        /// </summary>
        private void ClearPackageSelectionDataFromCart(string CompliancePackageType)
        {
            var orderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            if (orderCart.IsNotNull())
            {
                orderCart.ClearPackageSelectionData(CompliancePackageType);
            }
        }
        private void ClearPackageSelectionDataFromCart()
        {
            var orderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            if (orderCart.IsNotNull())
            {
                orderCart.ClearPackageSelectionData();
                if (!orderCart.FingerPrintData.IsNullOrEmpty() && !orderCart.FingerPrintData.IsEventCode && !orderCart.FingerPrintData.IsEventCode && orderCart.lstApplicantOrder.Count > AppConsts.NONE)
                {
                    orderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.TWO;
                }
            }

        }

        /// <summary>
        /// Set the ids of the selected combobox values, including the Program checkboxes, in the properties
        /// </summary>
        /// <param name="targetComboBox"></param>
        private void SetSelectedNodeData(WclComboBox targetComboBox, Int32 nodeLevel)
        {
            if (CurrentViewContext.SelectedHierarchyNodeIds.IsNull())
                CurrentViewContext.SelectedHierarchyNodeIds = new Dictionary<Int32, Int32>();

            if (_defaultNodeId == 0)
            {
                //Get the default node id from the default Institution.  
                _defaultNodeId = Presenter.GetDefaultNodeId(TenantId);
            }
            if (_defaultNodeId.IsNotNull() && !CurrentViewContext.SelectedHierarchyNodeIds.Values.Contains(_defaultNodeId))
            {
                //Set the default node id in SelectedHierarchyNodeIds
                CurrentViewContext.SelectedHierarchyNodeIds.Add(AppConsts.NONE, _defaultNodeId);
                if (applicantOrderCart.IsNotNull())
                {
                    applicantOrderCart.DefaultNodeId = _defaultNodeId;
                }
            }

            if (targetComboBox.Visible)
            {
                #region Normal Comboboxes
                if (!targetComboBox.SelectedValue.IsNullOrEmpty() && targetComboBox.SelectedValue != AppConsts.ZERO
                    && !(CurrentViewContext.SelectedHierarchyNodeIds.Keys.Contains(nodeLevel)))
                    CurrentViewContext.SelectedHierarchyNodeIds.Add(nodeLevel, Convert.ToInt32(targetComboBox.SelectedValue));

                #endregion

                #region Set Selected hierarchy node id in Session

                if (applicantOrderCart.IsNotNull())
                {
                    if (applicantOrderCart.alNodeIds.IsNull())
                        applicantOrderCart.alNodeIds = new ArrayList();

                    if (!applicantOrderCart.alNodeIds.Contains(Convert.ToString(targetComboBox.SelectedValue)))
                        applicantOrderCart.alNodeIds.Add(Convert.ToString(targetComboBox.SelectedValue));

                }

                #endregion

            }
        }

        /// <summary>
        /// Show hide the controls based on the current node level
        /// </summary>
        /// <param name="nodeLevel"></param>
        private void NodeToManage(Int32 nodeLevel)
        {
            if (HierarchyNodesRebound.Value == "1")
                ClearPackageSelectionDataFromCart();

            switch (nodeLevel)
            {
                case 1:
                    {
                        ManageControls(cmbLevel2, litLevel2, divNode2Literal, false);
                        ManageControls(cmbLevel3, litLevel3, divNode3Literal, false);
                        ManageControls(cmbLevel4, litLevel4, divNode4Literal, false);
                        ManageControls(cmbLevel5, litLevel5, divNode5Literal, false);
                        ManageControls(cmbLevel6, litLevel6, divNode6Literal, false);
                        ManageControls(cmbLevel7, litLevel7, divNode7Literal, false);
                        ManageControls(cmbLevel8, litLevel8, divNode8Literal, false);
                        ManageControls(cmbLevel9, litLevel9, divNode9Literal, false);
                        ManageControls(cmbLevel10, litLevel10, divNode10Literal, false);
                    }
                    break;
                case 2:
                    {
                        ManageControls(cmbLevel3, litLevel3, divNode3Literal, false);
                        ManageControls(cmbLevel4, litLevel4, divNode4Literal, false);
                        ManageControls(cmbLevel5, litLevel5, divNode5Literal, false);
                        ManageControls(cmbLevel6, litLevel6, divNode6Literal, false);
                        ManageControls(cmbLevel7, litLevel7, divNode7Literal, false);
                        ManageControls(cmbLevel8, litLevel8, divNode8Literal, false);
                        ManageControls(cmbLevel9, litLevel9, divNode9Literal, false);
                        ManageControls(cmbLevel10, litLevel10, divNode10Literal, false);
                    }
                    break;
                case 3:
                    {
                        ManageControls(cmbLevel4, litLevel4, divNode4Literal, false);
                        ManageControls(cmbLevel5, litLevel5, divNode5Literal, false);
                        ManageControls(cmbLevel6, litLevel6, divNode6Literal, false);
                        ManageControls(cmbLevel7, litLevel7, divNode7Literal, false);
                        ManageControls(cmbLevel8, litLevel8, divNode8Literal, false);
                        ManageControls(cmbLevel9, litLevel9, divNode9Literal, false);
                        ManageControls(cmbLevel10, litLevel10, divNode10Literal, false);
                    }
                    break;
                case 4:
                    {
                        ManageControls(cmbLevel5, litLevel5, divNode5Literal, false);
                        ManageControls(cmbLevel6, litLevel6, divNode6Literal, false);
                        ManageControls(cmbLevel7, litLevel7, divNode7Literal, false);
                        ManageControls(cmbLevel8, litLevel8, divNode8Literal, false);
                        ManageControls(cmbLevel9, litLevel9, divNode9Literal, false);
                        ManageControls(cmbLevel10, litLevel10, divNode10Literal, false);
                    }
                    break;
                case 5:
                    {
                        ManageControls(cmbLevel6, litLevel6, divNode6Literal, false);
                        ManageControls(cmbLevel7, litLevel7, divNode7Literal, false);
                        ManageControls(cmbLevel8, litLevel8, divNode8Literal, false);
                        ManageControls(cmbLevel9, litLevel9, divNode9Literal, false);
                        ManageControls(cmbLevel10, litLevel10, divNode10Literal, false);
                    }
                    break;
                case 6:
                    {
                        ManageControls(cmbLevel7, litLevel7, divNode7Literal, false);
                        ManageControls(cmbLevel8, litLevel8, divNode8Literal, false);
                        ManageControls(cmbLevel9, litLevel9, divNode9Literal, false);
                        ManageControls(cmbLevel10, litLevel10, divNode10Literal, false);
                    }
                    break;
                case 7:
                    {
                        ManageControls(cmbLevel8, litLevel8, divNode8Literal, false);
                        ManageControls(cmbLevel9, litLevel9, divNode9Literal, false);
                        ManageControls(cmbLevel10, litLevel10, divNode10Literal, false);
                    }
                    break;
                case 8:
                    {
                        ManageControls(cmbLevel9, litLevel9, divNode9Literal, false);
                        ManageControls(cmbLevel10, litLevel10, divNode10Literal, false);
                    }
                    break;
                case 9:
                    ManageControls(cmbLevel10, litLevel10, divNode10Literal, false);
                    break;
            }
        }

        /// <summary>
        /// Set the data in properties for the entire selected hierarchy
        /// </summary>
        private void SetSelectedHierarchyData()
        {
            ClearHierarchyData();

            SetSelectedNodeData(cmbLevel1, AppConsts.ONE);
            SetSelectedNodeData(cmbLevel2, AppConsts.TWO);
            SetSelectedNodeData(cmbLevel3, AppConsts.THREE);
            SetSelectedNodeData(cmbLevel4, AppConsts.FOUR);
            SetSelectedNodeData(cmbLevel5, AppConsts.FIVE);
            SetSelectedNodeData(cmbLevel6, AppConsts.SIX);
            SetSelectedNodeData(cmbLevel7, AppConsts.SEVEN);
            SetSelectedNodeData(cmbLevel8, AppConsts.EIGHT);
            SetSelectedNodeData(cmbLevel9, AppConsts.NINE);
            SetSelectedNodeData(cmbLevel10, AppConsts.TEN);
        }

        /// <summary>
        /// Resets hierarchy nodes from order cart.
        /// </summary>
        private void ClearHierarchyData()
        {
            if (applicantOrderCart.IsNotNull())
            {
                applicantOrderCart.alNodeIds = null;
                applicantOrderCart.DefaultNodeId = null;
            }
        }

        /// <summary>
        /// Rebind hierarchy when user comes back from Package details
        /// </summary>
        private void ReBindHierarchy()
        {
            if (applicantOrderCart.alNodeIds.IsNotNull())
            {
                for (int i = 0; i < applicantOrderCart.alNodeIds.Count; i++)
                {
                    SetCombobox(Convert.ToString(applicantOrderCart.alNodeIds[i]), i + 1);
                }
            }
            this.HierarchyNodesRebound.Value = "1";
        }

        /// <summary>
        /// Set the hierarhcy node values when user comes back from package details
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <param name="level"></param>
        private void SetCombobox(String nodeIds, Int32 level)
        {
            WclComboBox targetCombobox = new WclComboBox();

            switch (level)
            {
                case 1:
                    targetCombobox = cmbLevel1;
                    break;
                case 2:
                    targetCombobox = cmbLevel2;
                    break;
                case 3:
                    targetCombobox = cmbLevel3;
                    break;
                case 4:
                    targetCombobox = cmbLevel4;
                    break;
                case 5:
                    targetCombobox = cmbLevel5;
                    break;
                case 6:
                    targetCombobox = cmbLevel6;
                    break;
                case 7:
                    targetCombobox = cmbLevel7;
                    break;
                case 8:
                    targetCombobox = cmbLevel8;
                    break;
                case 9:
                    targetCombobox = cmbLevel9;
                    break;
                case 10:
                    targetCombobox = cmbLevel10;

                    break;
            }


            if (!String.IsNullOrEmpty(nodeIds))
            {
                String[] arrNodeIds = nodeIds.Split(',');
                if (arrNodeIds.Count() == 1)
                    targetCombobox.SelectedValue = arrNodeIds[0];
                //else if (arrNodeIds.Count() >= 1 && nodeIds.Contains(','))
                //{
                //    foreach (RadComboBoxItem comboItem in targetCombobox.Items)
                //    {
                //        if (arrNodeIds.Contains(comboItem.Value))
                //            comboItem.Checked = true;
                //    }
                //}
                CallComboboxEvent(level);
                //targetCombobox.Enabled = false;
                btnGo.Enabled = false;
            }
        }

        private void CallComboboxEvent(Int32 level)
        {
            switch (level)
            {
                case 1:
                    cmbLevel1_SelectedIndexChanged(cmbLevel1, null);
                    break;
                case 2:
                    cmbLevel2_SelectedIndexChanged(cmbLevel2, null);
                    break;
                case 3:
                    cmbLevel3_SelectedIndexChanged(cmbLevel3, null);
                    break;
                case 4:
                    cmbLevel4_SelectedIndexChanged(cmbLevel4, null);
                    break;
                case 5:
                    cmbLevel5_SelectedIndexChanged(cmbLevel5, null);
                    break;
                case 6:
                    cmbLevel6_SelectedIndexChanged(cmbLevel6, null);
                    break;
                case 7:
                    cmbLevel7_SelectedIndexChanged(cmbLevel7, null);
                    break;
                case 8:
                    cmbLevel8_SelectedIndexChanged(cmbLevel8, null);
                    break;
                case 9:
                    cmbLevel9_SelectedIndexChanged(cmbLevel9, null);
                    break;
            }
        }

        /// <summary>
        /// Set the package selection 
        /// </summary>
        // private void SetSelectedPackage(Boolean isViewDetail = false)
        private void SetSelectedPackage()
        {
            try
            {
                WclComboBox cntrl = (WclComboBox)FindControl("ddlDeptprogramPkg" + controlSuffix);
                if (!String.IsNullOrEmpty(cntrl.SelectedValue))
                {
                    Int32 deptProgramPackageId = Convert.ToInt32(cntrl.SelectedValue);

                    //List<MobilityNodePackages> _lstMobilityNodePackages;
                    ////DeptProgramPackage departmentProgramPackage = Presenter.GetDeptProgramPackageById(deptProgramPackageId, TenantId, out _lstMobilityNodePackages);
                    DeptProgramPackage departmentProgramPackage = Presenter.GetDeptProgramPackageById(deptProgramPackageId, TenantId);

                    /*if (departmentProgramPackage.IsNotNull())
                        BindMobilityNodePackages(departmentProgramPackage.DPP_DeptProgramMappingID, departmentProgramPackage.DPP_ID);*/

                    if (departmentProgramPackage.IsNotNull())
                        CurrentViewContext.DeptProgramPackage = departmentProgramPackage;
                    CurrentViewContext.ProgramDuration = departmentProgramPackage.IsNull() ? null : departmentProgramPackage.DeptProgramMapping.InstitutionNode.IN_Duration;
                    /*CurrentViewContext.NodeId = departmentProgramPackage.DeptProgramMapping.DPM_InstitutionNodeID;*/
                    //ShowHidePackages(true);

                    //ShowHideCompliancePackage(true);
                    //BindPackageDetails(false);
                    BindPackageDetails();
                    if (applicantOrderCart.IsNotNull())
                        applicantOrderCart.DPP_Id = deptProgramPackageId;
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

        private String GetSelectedHierarchyNodeId()
        {
            if (cmbLevel10.Visible == true && Convert.ToInt32(cmbLevel10.SelectedValue) > AppConsts.NONE)
            {
                return cmbLevel10.SelectedValue;
            }
            else if (cmbLevel9.Visible == true && Convert.ToInt32(cmbLevel9.SelectedValue) > AppConsts.NONE)
            {
                return cmbLevel9.SelectedValue;
            }
            else if (cmbLevel8.Visible == true && Convert.ToInt32(cmbLevel8.SelectedValue) > AppConsts.NONE)
            {
                return cmbLevel8.SelectedValue;
            }
            else if (cmbLevel7.Visible == true && Convert.ToInt32(cmbLevel7.SelectedValue) > AppConsts.NONE)
            {
                return cmbLevel7.SelectedValue;
            }
            else if (cmbLevel6.Visible == true && Convert.ToInt32(cmbLevel6.SelectedValue) > AppConsts.NONE)
            {
                return cmbLevel6.SelectedValue;
            }
            else if (cmbLevel5.Visible == true && Convert.ToInt32(cmbLevel5.SelectedValue) > AppConsts.NONE)
            {
                return cmbLevel5.SelectedValue;
            }
            else if (cmbLevel4.Visible == true && Convert.ToInt32(cmbLevel4.SelectedValue) > AppConsts.NONE)
            {
                return cmbLevel4.SelectedValue;
            }
            else if (cmbLevel3.Visible == true && Convert.ToInt32(cmbLevel3.SelectedValue) > AppConsts.NONE)
            {
                return cmbLevel3.SelectedValue;
            }
            else if (cmbLevel2.Visible == true && Convert.ToInt32(cmbLevel2.SelectedValue) > AppConsts.NONE)
            {
                return cmbLevel2.SelectedValue;
            }
            else if (cmbLevel1.Visible == true && Convert.ToInt32(cmbLevel1.SelectedValue) > AppConsts.NONE)
            {
                return cmbLevel1.SelectedValue;
            }
            return Presenter.GetInstitutionDPMID();
        }


        /// <summary>
        /// Bind background Packages
        /// </summary>
        /// <returns></returns>
        private Boolean BindBackgroundPackages()
        {
            List<BackgroundPackagesContract> _lstAll = _presenter.GetBackgroundPackages(CurrentViewContext.SelectedHierarchyNodeIds,
                                                        CurrentViewContext.CurrentLoggedInUserId, CurrentViewContext.TenantId, CurrentViewContext.FingerPrintData.IsLocationServiceTenant);
            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                if (CurrentViewContext.FingerPrintData.IsEventCode || CurrentViewContext.FingerPrintData.IsOutOfState)
                    _lstAll = _lstAll.FindAll(x => x.ServiceCode != BkgServiceType.FingerPrint_Card.GetStringValue() && x.ServiceCode != BkgServiceType.Passport_Photo.GetStringValue());
                else if (CurrentViewContext.FingerPrintData.IsLocationType)
                {
                    if (CurrentViewContext.FingerPrintData.IsFromArchivedOrderScreen)
                    {
                        _lstAll = _lstAll.FindAll(x => x.ServiceCode != BkgServiceType.SIMPLE.GetStringValue());
                    }
                    else
                    {
                        if (!CurrentViewContext.FingerPrintData.IsPassportPhotoAvailable)
                        {
                            _lstAll = _lstAll.FindAll(x => x.ServiceCode != BkgServiceType.Passport_Photo.GetStringValue());
                        }
                    }
                }
            }


            lstBackgroundPackages = _lstAll;

            if (_lstAll.IsNullOrEmpty())
                return false;

            DisplayBackGroundPackages(_lstAll);
            return true;
        }

        private void DisplayBackGroundPackages(List<BackgroundPackagesContract> _lstAll)
        {

            foreach (BackgroundPackagesContract backgroundPackagesContract in _lstAll)
            {
                if (backgroundPackagesContract.CustomPriceText.IsNullOrEmpty())
                {
                    //UAT-1676.
                    //backgroundPackagesContract.CustomPriceText = "This package costs $" + Decimal.Round(Convert.ToDecimal(backgroundPackagesContract.BasePrice), 2) + " and additional fees may apply.";
                    backgroundPackagesContract.CustomPriceText = "*Additional fees may apply.";


                }
            }

            List<BackgroundPackagesContract> _lstExclusive = _lstAll.Where(bp => bp.IsExclusive).ToList();


            if (!_lstExclusive.IsNullOrEmpty())
            {
                rptExclusive.DataSource = _lstExclusive;
                rptExclusive.Visible = true;
                // pnlExclusiveBkgPackages.Visible = true;
            }
            else
            {
                rptExclusive.DataSource = null;
                rptExclusive.Visible = false;
                // pnlExclusiveBkgPackages.Visible = false;
            }
            rptExclusive.DataBind();

            List<BackgroundPackagesContract> _lstNonExclusive = _lstAll.Where(bp => !bp.IsExclusive).ToList();

            if (!_lstNonExclusive.IsNullOrEmpty())
            {
                rptNonExclusive.DataSource = _lstNonExclusive;
                rptNonExclusive.Visible = true;
                //pnlNonExclusiveBkgPackages.Visible = true;
            }
            else
            {
                rptNonExclusive.DataSource = null;
                rptNonExclusive.Visible = false;
                //pnlNonExclusiveBkgPackages.Visible = false;
            }

            if (_lstExclusive.IsNullOrEmpty() && _lstNonExclusive.IsNullOrEmpty())
            {
                pnlExclusiveBkgPackages.Visible = false;
            }
            else
            {
                pnlExclusiveBkgPackages.Visible = true;
            }

            rptNonExclusive.DataBind();
            ShowHideBackgroundPackages(true);
            ShowHideViewDetailsLink(rptExclusive, _lstExclusive, "hdnExcBPAId");
            ShowHideViewDetailsLink(rptNonExclusive, _lstNonExclusive, "hdnNonExcBPAId");
            //UAT-3268 
            List<BackgroundPackagesContract> lstBkpPkgsToQualifyRot = new List<BackgroundPackagesContract>();
            if (!_lstAll.IsNullOrEmpty())
            {
                lstBkpPkgsToQualifyRot = _lstAll.Where(cond => cond.IsReqToQualifyInRotation).ToList();
                RotationQualifyBkgPKgDisplay(lstBkpPkgsToQualifyRot);
            }
        }

        /// <summary>
        /// Bind Compliance related Packages
        /// </summary>
        /// <returns></returns>
        private Boolean BindCompliancePackages()
        {
            SetSelectedHierarchyData();
            _presenter.GetDeptProgramPackage();
            // Check the Node Level settings and set the 'IfInvoiceOnlyPymnOptn' so that it can be used by Compliance Repeaters
            CheckIfInvoiceIsOnlyPaymentOption(CurrentViewContext.DeptProgramPackages.IsNotNull() && CurrentViewContext.DeptProgramPackages.Count() > 0);
            return DisplayCompliancePackages();
        }

        /// <summary>
        /// Bind the Compliance packages of both types from DeptProgramPackges
        /// </summary>
        /// <returns></returns>
        private bool DisplayCompliancePackages()
        {

            if (DeptProgramPackages.IsNotNull() && DeptProgramPackages.Count() > 0)
            {
                if (!this.IsChangeSubscriptionRequest)
                {
                    dvTrackingTotal.Visible = true;
                }
                #region Commented Code - UAT 1545

                //AddEmptyDropDownItem(ddlDeptprogramPkg);

                ////DeptProgramPackage _deptProgramPackage = new DeptProgramPackage();
                ////_deptProgramPackage.DPP_ID = AppConsts.NONE;
                ////_deptProgramPackage.CompliancePackage = new CompliancePackage();
                ////_deptProgramPackage.CompliancePackage.PackageName = AppConsts.COMBOBOX_ITEM_SELECT;
                ////_deptProgramPackage.CompliancePackage.PackageLabel = AppConsts.COMBOBOX_ITEM_SELECT;
                ////_deptProgramPackage.CompliancePackage.lkpCompliancePackageType = new lkpCompliancePackageType();
                ////_deptProgramPackage.CompliancePackage.lkpCompliancePackageType.CPT_Code = string.Empty;

                ////DeptProgramPackages.Insert(0, _deptProgramPackage);

                #endregion

                rptImmunizationPackages.DataSource = GetPackageDataByType(CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue());
                rptImmunizationPackages.DataBind();

                rptAdminstrativePackages.DataSource = GetPackageDataByType(CompliancePackageTypes.ADMINISTRATIVE_COMPLIANCE_PACKAGE.GetStringValue());
                rptAdminstrativePackages.DataBind();

                divTrackingPackages.Visible = rptImmunizationPackages.Items.Count > 0 ? true : false;
                divTrackingPackages_ADMN.Visible = rptAdminstrativePackages.Items.Count > 0 ? true : false;

                divTracking.Visible = true;

                #region Commented Code - UAT 1545

                //foreach (string compliancePackageType in AvailableComplaincePackageTypes)
                //{
                //    CurrentCompliancePackageType = compliancePackageType;
                //    ShowHideTrackingDiv(true);
                //}

                //foreach (string compliancePackageType in AvailableComplaincePackageTypes)
                //{
                //    CurrentCompliancePackageType = compliancePackageType;
                //    if (applicantOrderCart.IsNotNull())
                //        applicantOrderCart.CurrentCompliancePackageType = compliancePackageType;

                //    WclComboBox cntrl = (WclComboBox)FindControl("ddlDeptprogramPkg" + controlSuffix);

                //    cntrl.DataSource = DeptProgramPackages.Where(dpp => dpp.CompliancePackage.lkpCompliancePackageType.CPT_Code.Equals(compliancePackageType) || string.IsNullOrEmpty(dpp.CompliancePackage.lkpCompliancePackageType.CPT_Code)).Select(item => new
                //    {
                //        PackageMappingId = item.DPP_ID,
                //        PackageName = String.IsNullOrEmpty(item.CompliancePackage.PackageLabel) ? item.CompliancePackage.PackageName : item.CompliancePackage.PackageLabel
                //    });

                //    cntrl.SelectedValue = AppConsts.ZERO;
                //    //cntrl.DataBind();
                //    cntrl.Enabled = true;

                //    if (applicantOrderCart.IsNotNull() && applicantOrderCart.DPP_Id.IsNotNull())
                //    {
                //        cntrl.SelectedValue = applicantOrderCart.DPP_Id.ToString();
                //    }

                //    ShowHideCompliancePackage(true);

                //    HtmlGenericControl divControl = (HtmlGenericControl)FindControl("divSubscriptions" + controlSuffix);

                //    if (divControl.IsNotNull())
                //    {
                //        if (!string.IsNullOrEmpty(cntrl.SelectedValue) && cntrl.SelectedValue != AppConsts.ZERO)
                //        {
                //            divControl.Visible = true;
                //        }
                //        else
                //        {
                //            divControl.Visible = false;
                //            WclComboBox cntrlDdlSubscriptions = (WclComboBox)FindControl("ddlSubscriptions" + controlSuffix);
                //            if (cntrlDdlSubscriptions.IsNotNull())
                //            {
                //                cntrlDdlSubscriptions.DataSource = null;
                //                cntrlDdlSubscriptions.DataBind();
                //            }
                //        }
                //    }
                //} 

                #endregion

                return true;
            }
            else
            {
                #region Commented Code - UAT 1545

                //1545
                //ddlDeptprogramPkg.DataSource = "";
                //ddlDeptprogramPkg.DataBind();
                //ddlDeptprogramPkg_ADMN.DataSource = "";
                //ddlDeptprogramPkg_ADMN.DataBind();

                #endregion

                rptImmunizationPackages.DataSource = null;
                rptImmunizationPackages.DataBind();

                rptAdminstrativePackages.DataSource = null;
                rptAdminstrativePackages.DataBind();

                divTracking.Visible = false;
                return false;
            }

        }

        /// <summary>
        /// Show/hide BOTH TYPE OF PACKAGES, based on the visibility, when 
        /// 1. User changes the Dropdown selection
        /// 2. It is a fresh order
        /// </summary>
        /// <param name="visibility"></param>
        private void ShowHidePackages(Boolean visibility)
        {
            divTracking.Visible = false;
            divBundles.Visible = false;
            //[SS]:[03/08/2017]:fix issue in case of bundle package 
            rptBundles.DataSource = new List<BundleData>();
            rptBundles.DataBind();
            rptBundlesExclusive.DataSource = new List<BundleData>();
            rptBundlesExclusive.DataBind();
            divBackgroundPackages.Visible = false;
            dvOrderTotal.Visible = false;
            ShowHideSubscriptionContinueButton(visibility);
        }

        /// <summary>
        /// Add empty or Select item in the specified dropdown
        /// </summary>
        /// <param name="targetDropDown"></param>
        private void AddEmptyDropDownItem(WclDropDownList targetDropDown)
        {
            targetDropDown.Items.Insert(0, new DropDownListItem
            {
                Text = AppConsts.COMBOBOX_ITEM_SELECT,
                Value = AppConsts.ZERO
            });
        }

        /// <summary>
        ///  Manage the visibility of the Compliance Packages
        /// </summary>
        /// <param name="visibility"></param>
        private void ShowHideCompliancePackage(Boolean visibility)
        {
            HtmlGenericControl ctrl = (HtmlGenericControl)FindControl("divCompliancePackage" + controlSuffix);
            if (ctrl.IsNotNull())
                ctrl.Visible = visibility;
            if (!visibility)
            {
                WclComboBox cmbCtrl = (WclComboBox)FindControl("ddlDeptprogramPkg" + controlSuffix);
                if (cmbCtrl.IsNotNull())
                {
                    cmbCtrl.Items.Clear(); // Clear items when div is hidden 
                    cmbCtrl.SelectedValue = "";
                }
                CurrentViewContext.DeptProgramPackage = null;
            }
            //else
            //    ShowHideSubscriptionContinueButton(true);
        }

        /// <summary>
        /// Manage the display of Background Packages, depending on the parameter
        /// </summary>
        /// <param name="visibility"></param>
        private void ShowHideBackgroundPackages(Boolean visibility)
        {
            divBackgroundPackages.Visible = visibility;
            cmdBarBackgroundPackages.Visible = false; // Not in use anymore. So always false

            if (!visibility)
            {
                rptExclusive.DataSource = null;
                rptNonExclusive.DataSource = null;
            }
        }

        /// <summary>
        /// Manage the visibility of Subscriptions' and Continue Order Button
        /// </summary>
        /// <param name="visibility"></param>
        private void ShowHideSubscriptionContinueButton(Boolean visibility)
        {
            //divSubscriptions.Visible = visibility;
            cmdBar.Visible = visibility;
        }

        /// <summary>
        /// Redirect to View Details of the selected Background Package
        /// </summary>
        /// <param name="packageId"></param>
        private void RedirectToBackgroundPackageDetails(Int32 packageId)
        {
            applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);

            if (applicantOrderCart == null)
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.GetApplicantOrder();
            }

            SetSelectedHierarchyData();

            var _isBundleSelected = false;
            //var _selectedBundleId = AppConsts.NONE;
            List<Int32> _selectedBundleIds = new List<Int32>();
            IsBundleSelected(ref _isBundleSelected, ref _selectedBundleIds);

            AddCompliancePackageDataToSession(_isBundleSelected);

            //UAT-1200: Added pkgBundle id into session.
            ////if (dvPackageBundle.Visible && CurrentViewContext.SelectedPkgBundleId > AppConsts.NONE)
            ////{
            ////    applicantOrderCart.SelectedPkgBundleId = CurrentViewContext.SelectedPkgBundleId;
            ////}
            ////else
            ////{
            ////    applicantOrderCart.SelectedPkgBundleId = null;
            ////}

            ////if (divCompliancePackage.Visible && divSubscriptions.Visible)
            ////if (divTracking.Visible && (divTrackingPackages.Visible || divTrackingPackages_ADMN.Visible))
            ////{
            ////    AddCompliancePackageDataToSession();
            ////}

            // For bundle selection, Bkg package data is added inside the 'AddCompliancePackageDataToSession' method
            if (!_isBundleSelected)
            {
                AddBackgroundPackageDataToSession();
            }

            applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel = IfInvoiceOnlyPymnOptn;
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);

            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>{
                                                                    { "Child", ChildControls.ViewBackroundPackageDetail},
                                                                    { "TenantId", TenantId.ToString()},
                                                                    { "PackageId",Convert.ToString( packageId)}
                                                         };
            Response.Redirect(String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString()), true);
        }

        /// <summary>
        /// Set the data for maintaing on coming back, after click on view details for a package
        /// </summary>
        /// <param name="packageId"></param>
        private void RedirectToCompliancePackageDetails(Int32 packageId)
        {
            applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);

            if (applicantOrderCart == null)
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.GetApplicantOrder();
            }

            SetSelectedHierarchyData();

            var _isBundleSelected = false;
            //var _selectedBundleId = AppConsts.NONE;
            List<Int32> _selectedBundleIds = new List<Int32>();
            IsBundleSelected(ref _isBundleSelected, ref _selectedBundleIds);

            //UAT-1200: Added pkgBundle id into session.
            ////if (dvPackageBundle.Visible && CurrentViewContext.SelectedPkgBundleId > AppConsts.NONE)
            ////{
            ////    applicantOrderCart.SelectedPkgBundleId = CurrentViewContext.SelectedPkgBundleId;
            ////}
            ////else
            ////{
            ////    applicantOrderCart.SelectedPkgBundleId = null;
            ////}

            //SetSelectedPackage(isViewDetail);
            AddCompliancePackageDataToSession(_isBundleSelected);

            // For bundle selection, Bkg package data is added inside the 'AddCompliancePackageDataToSession' method
            if (!_isBundleSelected)
            {
                AddBackgroundPackageDataToSession();
            }
            //if (applicantOrderCart.IsCompliancePackageSelected)
            //    applicantOrderCart.AddDeptProgramPackageSubscriptionId(SelectedDeptProgramPackageSubscription.DPPS_ID);

            applicantOrderCart.lstDepProgramMappingId = CurrentViewContext.SelectedHierarchyNodeIds.Values.ToList();
            applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel = IfInvoiceOnlyPymnOptn;

            String _orderRequestType = String.Empty;
            //if (CurrentViewContext.PreviousOrderId > 0 && CurrentViewContext.SettlementPrice > 0)
            if (CurrentViewContext.PreviousOrderId > 0)
            {
                applicantOrderCart.OrderRequestType = _orderRequestType = OrderRequestType.ChangeSubscription.GetStringValue();
            }
            else
            {
                applicantOrderCart.OrderRequestType = _orderRequestType = OrderRequestType.NewOrder.GetStringValue();
            }
            /*applicantOrderCart.NodeId = CurrentViewContext.NodeId;*/
            //applicantOrderCart.PrevOrderId = CurrentViewContext.PreviousOrderId;
            //applicantOrderCart.SettleAmount = CurrentViewContext.SettlementPrice;

            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.ViewPackageDetail},
                                                                    { "TenantId", TenantId.ToString()},
                                                                    { "PackageId",Convert.ToString( packageId)},
                                                                    { "OrderRequestType", _orderRequestType }
                                                                 };
            Response.Redirect(String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString()));
        }

        /// <summary>
        /// Method to SHow/Hide View Details link
        /// </summary>
        /// <param name="repeater"></param>
        /// <param name="lstBackgroundPackagesContract"></param>
        /// <param name="hdnBPAId"></param>
        private void ShowHideViewDetailsLink(Repeater repeater, List<BackgroundPackagesContract> lstBackgroundPackagesContract, String hdnBPAId)
        {
            foreach (var RepeaterItem in repeater.Items)
            {
                Int32 packageId = Convert.ToInt32(((RepeaterItem as RepeaterItem).FindControl(hdnBPAId) as HiddenField).Value);
                WclButton lnkBtn = ((RepeaterItem as RepeaterItem).FindControl("btnViewDetails") as WclButton);
                if (lstBackgroundPackagesContract.Where(cond => cond.BPAId == packageId).Select(x => x.BPAViewDetails).FirstOrDefault() && !CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                {
                    lnkBtn.Visible = true;
                }
                else
                {
                    lnkBtn.Visible = false;
                }
            }
        }

        /// <summary>
        /// Returns the enum Code of the screen, from which has user has navigated from. 
        /// If it is empty, then defaults to the Dashboard
        /// </summary>
        /// <returns></returns>
        private String GetNavigationFrom()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                if (args.ContainsKey(AppConsts.PENDING_ORDER_NAVIGATION_FROM))
                    return Convert.ToString(args[AppConsts.PENDING_ORDER_NAVIGATION_FROM]);


                return PendingOrderNavigationFrom.ApplicantDashboard.GetStringValue();
            }
            return PendingOrderNavigationFrom.ApplicantDashboard.GetStringValue();
        }

        /// <summary>
        /// Set the button Text for 'Previous', 'Next' or 'Restart' etc, based on the type of Order
        /// </summary>
        private void SetButtonText()
        {
            // Case when the Order has not started yet i.e. applicant has landed first time on the page
            if (String.IsNullOrEmpty(CurrentViewContext.OrderType) || (!String.IsNullOrEmpty(CurrentViewContext.OrderType)
                && CurrentViewContext.OrderType == OrderRequestType.NewOrder.GetStringValue()))
            {
                //cmdBar.SubmitButtonText = AppConsts.NEXT_BUTTON_TEXT;
                cmdBar.SubmitButtonText = Resources.Language.NEXT;
                if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
                {
                    cmdBar.SubmitButton.ValidationGroup = "cbiUniqueID";
                }
                //cmdBar.SaveButtonText = AppConsts.PREVIOUS_BUTTON_TEXT;
                cmdBar.SaveButtonText = Resources.Language.PREVIOUS;
                cmdBar.SaveButtonIconClass = "rbPrevious";
                cmdBar.ButtonSkin = "silk";
            }
            else
            {
                cmdBar.SubmitButtonText = AppConsts.NEXT_BUTTON_TEXT;
                cmdBar.SaveButtonText = "Cancel";
                cmdBar.SaveButton.CssClass = "margin-2 cancelposition";
                cmdBar.SaveButtonIconClass = "rbCancel";
            }
        }


        /// <summary>
        ///  Manage the visibility of the Compliance Packages
        /// </summary>
        /// <param name="visibility"></param>
        private void ShowHidePackageBundle(Boolean visibility)
        {
            ////dvPackageBundle.Visible = visibility;
            ////if (!visibility)
            ////{
            ////    cmbPackageBundle.Items.Clear(); // Clear items when div is hidden 
            ////    cmbPackageBundle.SelectedValue = "";
            ////    CurrentViewContext.DeptProgramPackage = null;
            ////    DisplayPkgBundleNotes();
            ////}
        }

        //private void BindPackagesWithSelectedBundle()
        //{
        //    Boolean _isBackgroundPackageAvailable = false;
        //    Boolean _isCompliancePackageAvailable = false;
        //    foreach (string cptype in AvailableComplaincePackageTypes)
        //    {
        //        CurrentCompliancePackageType = cptype;
        //        ShowHideCompliancePackage(false);
        //    }
        //    List<BackgroundPackagesContract> _lstAll = Presenter.GetPackagelistAvailableUnderBundle();

        //    if (DeptProgramPackages.IsNotNull() && DeptProgramPackages.Count() > 0)
        //    {
        //        _isCompliancePackageAvailable = DisplayCompliancePackages();
        //        foreach (string cptype in AvailableComplaincePackageTypes)
        //        {
        //            CurrentCompliancePackageType = cptype;
        //            WclComboBox cmbCtrl = (WclComboBox)FindControl("ddlDeptprogramPkg" + controlSuffix);
        //            if (cmbCtrl.IsNotNull())
        //            {
        //                if (cmbCtrl.Items.Count > AppConsts.NONE)
        //                {
        //                    cmbCtrl.Items[1].Selected = true;
        //                    cmbCtrl.Enabled = false;
        //                    ShowHideCompliancePackage(true);
        //                    DisplayPackageDetails();
        //                }
        //                else
        //                {
        //                    ShowHideCompliancePackage(false);
        //                }
        //            }
        //        }
        //    }

        //    if (_lstAll.Count > AppConsts.NONE)
        //    {
        //        _isBackgroundPackageAvailable = true;
        //        DisplayBackGroundPackages(_lstAll);
        //    }

        //    CheckIfInvoiceIsOnlyPaymentOption(_isCompliancePackageAvailable);
        //    if (!_isBackgroundPackageAvailable)
        //    {
        //        ShowHideBackgroundPackages(false);
        //    }
        //    if (_isBackgroundPackageAvailable || _isCompliancePackageAvailable)
        //        ShowHideSubscriptionContinueButton(true);
        //    else
        //        ShowHideSubscriptionContinueButton(false);
        //}


        private void DisplayPackageDetails()
        {
            ClearPackageSelectionDataFromCart(CurrentCompliancePackageType);
            SetSelectedPackage();

            WclComboBox cntrl = (WclComboBox)FindControl("ddlDeptprogramPkg" + controlSuffix);
            WclButton ctrlBtnViewDetails = (WclButton)FindControl("btnViewDetails" + controlSuffix);

            if (!String.IsNullOrEmpty(cntrl.SelectedValue) && cntrl.SelectedValue != AppConsts.ZERO)
            {
                if (CurrentViewContext.DeptProgramPackages.Where(cond => cond.DPP_ID == Convert.ToInt32(cntrl.SelectedValue))
                    .Select(x => x.CompliancePackage.IsViewDetailsInOrderEnabled).FirstOrDefault())
                {
                    ctrlBtnViewDetails.Visible = true;
                }
                else
                {
                    ctrlBtnViewDetails.Visible = false;
                }
            }
            else
            {
                ctrlBtnViewDetails.Visible = false;
            }
        }

        private void DisplayPkgBundleNotes()
        {
            ////if (CurrentViewContext.SelectedPkgBundleId > AppConsts.NONE)
            ////{
            ////    PackageBundle selectedPackageBundle = CurrentViewContext.lstPackageBundle.FirstOrDefault(cond => cond.PBU_ID == CurrentViewContext.SelectedPkgBundleId);
            ////    if (!selectedPackageBundle.PBU_ExplanatoryNotes.IsNullOrEmpty())
            ////    {
            ////        litEexplanatoryNotes.Text = selectedPackageBundle.PBU_ExplanatoryNotes;
            ////        dvBundleNotes.Visible = true;
            ////    }
            ////    else
            ////    {
            ////        litEexplanatoryNotes.Text = String.Empty;
            ////        dvBundleNotes.Visible = false;
            ////    }
            ////}
            ////else
            ////{
            ////    litEexplanatoryNotes.Text = String.Empty;
            ////    dvBundleNotes.Visible = false;
            ////}
        }
        //private Boolean GetPreviousOrderHistory()
        //{
        //    BackroundOrderContract _orderHistory = _presenter.GetPreviousOrderHistory(CurrentViewContext.CurrentLoggedInUserId, CurrentViewContext.TenantId);
        //    if (_orderHistory.IsNullOrEmpty())
        //    {
        //        //foreach (RepeaterItem item in rptNonExclusive.Items)
        //        //{
        //        //    Panel dvOrderHistoryDetails = (item.FindControl("dvOrderHistoryDetails") as Panel);
        //        //    dvOrderHistoryDetails.Visible = false;
        //        //}
        //            return false;
        //    }
        //    else
        //    {
        //        foreach (RepeaterItem item in rptNonExclusive.Items)
        //        {
        //            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
        //            {
        //                Label lblServiceCode = item.FindControl("lblServiceCode") as Label;
        //                if (lblServiceCode.Text == BkgServiceType.FingerPrint_Card.GetStringValue() || lblServiceCode.Text == BkgServiceType.Passport_Photo.GetStringValue())
        //                {
        //                    //Panel dvOrderHistoryDetails = (item.FindControl("dvOrderHistoryDetails") as Panel);
        //                    Label lblHistoryDetails = (item.FindControl("lblHistoryDetails") as Label);
        //                    RadButton lblOrderId = (item.FindControl("btnViewImages") as RadButton);
        //                    CheckBox chkNonExc = (item.FindControl("chkNonExc") as CheckBox);
        //                    lblOrderId.Attributes.Add("OrderId", Convert.ToString(_orderHistory.OrderID));
        //                    if (lblServiceCode.Text == BkgServiceType.FingerPrint_Card.GetStringValue())
        //                    {
        //                        lblHistoryDetails.Text = Resources.Language.LFPTKNON.Replace("#11111", _orderHistory.OrderNumber).Replace("DDYYYMMMM", Convert.ToDateTime(_orderHistory.OrderCreatedDate).ToString("MM/dd/yyyy"));
        //                    }
        //                    else if (lblServiceCode.Text == BkgServiceType.Passport_Photo.GetStringValue())
        //                    {
        //                        lblOrderId.Attributes.Add("ApplicantFPImgId", Convert.ToString(_orderHistory.ApplicantFPImgId));
        //                        lblOrderId.Attributes.Add("ApplicantFPImgPath", Convert.ToString(_orderHistory.ApplicantFPImgPath));
        //                        lblOrderId.Attributes.Add("ApplicantFPImgName", Convert.ToString(_orderHistory.ApplicantFPImgName));
        //                        lblHistoryDetails.Text = Resources.Language.LPPTKNON.Replace("#11111", _orderHistory.OrderNumber).Replace("DDYYYMMMM", Convert.ToDateTime(_orderHistory.OrderCreatedDate).ToString("MM/dd/yyyy"));
        //                    }
        //                    //if(lblServiceCode.Text != BkgServiceType.SIMPLE.GetStringValue() && chkNonExc.Checked)
        //                    //{
        //                    //    dvOrderHistoryDetails.Visible = true;
        //                    //}
        //                }
        //            }
        //        }
        //    }
        //    return true;
        //}
        #endregion

        #endregion

        #region UAT 1545

        #region Immumization Package Repeater Events

        protected void rptImmunizationPackages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // During Rebind from Back navigation throuhg 'previous' or 'Edit' or Back from Details page, 
            // Change CurrentCompliancePackageType in cart so that it get's corresonding details using 'getCompliancePackage()' 
            // in each property of cart.
            if (applicantOrderCart.IsNotNull())
            {
                applicantOrderCart.CurrentCompliancePackageTypeInContext = CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue();
            }

            BindSubscriptions(e);
        }

        protected void rptImmunizationPackages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                var _pkgId = (e.Item.FindControl("hdfImmPackageId") as HiddenField).Value;
                this.CompliancePackageType = CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue();
                RedirectToCompliancePackageDetails(Convert.ToInt32(_pkgId));
            }
        }

        #endregion

        #region Adminstrative Package Repeater Events

        protected void rptAdminstrativePackages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // During Rebind from Back navigation throuhg 'previous' or 'Edit' or Back from Details page, 
            // Change CurrentCompliancePackageType in cart so that it get's corresonding details using 'getCompliancePackage()' 
            // in each property of cart.
            if (applicantOrderCart.IsNotNull())
            {
                applicantOrderCart.CurrentCompliancePackageTypeInContext = CompliancePackageTypes.ADMINISTRATIVE_COMPLIANCE_PACKAGE.GetStringValue();
            }

            BindSubscriptions(e);
        }

        protected void rptAdminstrativePackages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                var _pkgId = (e.Item.FindControl("hdfImmPackageId") as HiddenField).Value;
                this.CompliancePackageType = CompliancePackageTypes.ADMINISTRATIVE_COMPLIANCE_PACKAGE.GetStringValue();
                RedirectToCompliancePackageDetails(Convert.ToInt32(_pkgId));
            }
        }

        #endregion

        #region Bundle Repeater Events

        protected void rptBundles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var _crntBundleId = Convert.ToInt32((e.Item.FindControl("hdfBundleId") as HiddenField).Value);
            var lstBundlePackages = new List<OrderPackageSelection>();

            var cmpliancelstPkg = lstNonExclusiveBundlePackageData.Where(bnd => bnd.BundleId == _crntBundleId).FirstOrDefault();
            var bkgListPkgs = lstNonExclusiveBundlePackageData.Where(bnd => bnd.BundleId == _crntBundleId).FirstOrDefault();

            if (!cmpliancelstPkg.IsNullOrEmpty())
                lstBundlePackages.AddRange(cmpliancelstPkg.lstCompliancePkgs.ToList());

            if (!bkgListPkgs.IsNullOrEmpty())
                lstBundlePackages.AddRange(bkgListPkgs.lstBkgPkgs.ToList());

            if (applicantOrderCart.IsNotNull() && !applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty() && applicantOrderCart.lstSelectedPkgBundleId.Count > AppConsts.NONE && applicantOrderCart.lstSelectedPkgBundleId.Contains(_crntBundleId))//UAT-3283
            //if (applicantOrderCart.IsNotNull() && applicantOrderCart.SelectedPkgBundleId > AppConsts.NONE && applicantOrderCart.SelectedPkgBundleId == _crntBundleId)
            {
                (e.Item.FindControl("chkBundles") as CheckBox).Checked = true;//UAT-3283
                //(e.Item.FindControl("rbtnBundle") as RadioButton).Checked = true;//UAT-3283
            }
            var _repeater = (e.Item.FindControl("rptBundlePackages") as Repeater);
            if (lstBundlePackages.Any())
            {
                _repeater.DataSource = lstBundlePackages;
                _repeater.DataBind();
            }
        }

        /// <summary>
        /// Databound for Repeater to display PAckages in a Bundle. Bind the Subscription Options for the Packages in a Bundle in this event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptBundlePackages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var _pkgTypeCode = (e.Item.FindControl("hdfMasterPackageTypeCode") as HiddenField).Value;
            var _bundleId = Convert.ToInt32((e.Item.FindControl("hdfNestedBundleId") as HiddenField).Value);

            // Bind the Subscription Options for the Tracking Packages only.
            if (_pkgTypeCode == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
            {
                Guid subscriptionOptionCode = new Guid(SubscriptionOptions.CustomMonthly.GetStringValue());
                var _rbtnList = (e.Item.FindControl("rbtnSubscriptions") as RadioButtonList);
                var _dppId = Convert.ToInt32((e.Item.FindControl("hdfDPPId") as HiddenField).Value);

                var _compliancePackageTypeCode = (e.Item.FindControl("hdfCompliancePackageTypeCode") as HiddenField).Value;

                var _dpp = CurrentViewContext.lstBundleDeptProgramPackages.Where(bnd => bnd.PBNP_PackageBundleID == _bundleId
                                                                              && bnd.PBNP_BkgPackageHierarchyMappingID == null
                                                                              && bnd.PBNP_DeptProgramPackageID == _dppId)
                                                                          .First().DeptProgramPackage;


                var _lstDPPS = _dpp.DeptProgramPackageSubscriptions.Where(dpps => dpps.SubscriptionOption.IsSystem == false
                                                                       && dpps.SubscriptionOption.Code != subscriptionOptionCode
                                                                       && dpps.DPPS_TotalPrice != null && !dpps.DPPS_IsDeleted).ToList();


                Dictionary<DeptProgramPackageSubscription, Int32?> dicDpps = new Dictionary<DeptProgramPackageSubscription, Int32?>();
                foreach (var item in _lstDPPS)
                {
                    Int32? totalDays = (item.SubscriptionOption.Year.IsNullOrEmpty() ? AppConsts.NONE : (item.SubscriptionOption.Year * 12 * 30))
                                        + (item.SubscriptionOption.Month.IsNullOrEmpty() ? AppConsts.NONE : (item.SubscriptionOption.Month * 30))
                                            + (item.SubscriptionOption.Day.IsNullOrEmpty() ? AppConsts.NONE : (item.SubscriptionOption.Day));
                    dicDpps.Add(item, totalDays);

                }



                if (!dicDpps.IsNullOrEmpty())
                {
                    dicDpps = dicDpps.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    _lstDPPS = dicDpps.Keys.ToList();
                }


                foreach (var dpps in _lstDPPS)
                {
                    Decimal _actualPrice = 0;
                    Decimal _netPrice = 0;

                    GetPricing(dpps, out _netPrice, out _actualPrice);
                    Boolean _displayPrice = ManagePriceDisplay(dpps);

                    ListItem _rbtnListItem = new ListItem
                    {
                        Text = _displayPrice ? String.Format("{0}{1}", dpps.SubscriptionOption.Label, " ($" + _netPrice.ToString("0.00") + ")") : String.Format("{0}", dpps.SubscriptionOption.Label),
                        Value = Convert.ToString(dpps.DPPS_ID)
                    };

                    if (applicantOrderCart.IsNotNull())
                    {
                        applicantOrderCart.BundleInContext = _bundleId;
                        applicantOrderCart.CurrentCompliancePackageTypeInContext = _compliancePackageTypeCode;
                        if (dpps.DPPS_ID.IsNotNull() && dpps.DPPS_ID == applicantOrderCart.DPPS_ID && !applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty() && applicantOrderCart.lstSelectedPkgBundleId.Contains(_bundleId)) //UAT-3283
                        //if (dpps.DPPS_ID.IsNotNull() && dpps.DPPS_ID == applicantOrderCart.DPPS_ID && applicantOrderCart.SelectedPkgBundleId == _bundleId)
                        {
                            _rbtnListItem.Selected = true;

                            // Enable subscriptions when re-binded so that they can be changed by applicant.
                            _rbtnList.Enabled = true;
                        }
                    }
                    _rbtnList.Items.Add(_rbtnListItem);
                }
                _rbtnList.Visible = true;
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlGenericControl divElement = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvElement1");

                var _bphmId = Convert.ToInt32((e.Item.FindControl("hdnBPHMId") as HiddenField).Value);
                var _bphm = CurrentViewContext.lstBundleBkgPackages.Where(bnd => bnd.PBNP_PackageBundleID == _bundleId
                                                                     && bnd.PBNP_BkgPackageHierarchyMappingID != null
                                                                     && bnd.PBNP_BkgPackageHierarchyMappingID == _bphmId)
                                                                 .First().BkgPackageHierarchyMapping;


                List<lkpPaymentOption> lstPaymentOptions = !_bphm.IsNullOrEmpty()
                                                            ? _bphm.BkgPackagePaymentOptions.Where(bppo => !bppo.BPPO_IsDeleted).Select(bppo => bppo.lkpPaymentOption).ToList()
                                                            : new List<lkpPaymentOption>();


                //UAt-1676 related changes.
                Boolean _displayPrice = DisplayPrice(lstPaymentOptions);
                (e.Item.FindControl("hdfIsDisplayPrice") as HiddenField).Value = _displayPrice.ToString();
                var basePrice = Convert.ToDecimal((e.Item.FindControl("hdfBkgPackageBasePrice") as HiddenField).Value);
                Literal literalControl = e.Item.FindControl("litPackageName") as Literal;
                literalControl.Text = _displayPrice ? String.Format("{0}{1}", literalControl.Text, " ($" + basePrice.ToString("0.00") + ")")
                                                                                : String.Format("{0}", literalControl.Text);
                divElement.Visible = _displayPrice;
            }
        }

        /// <summary>
        /// Item Command for Repeater for Packages
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptBundlePackages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                var _masterPkgTypeCode = (e.Item.FindControl("hdfMasterPackageTypeCode") as HiddenField).Value;
                var _packageId = (e.Item.FindControl("hdfPackageId") as HiddenField).Value;

                if (_masterPkgTypeCode == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
                {
                    this.CompliancePackageType = CurrentCompliancePackageType = (e.Item.FindControl("hdfCompliancePackageTypeCode") as HiddenField).Value;
                    RedirectToCompliancePackageDetails(Convert.ToInt32(_packageId));
                }
                else
                {
                    RedirectToBackgroundPackageDetails(Convert.ToInt32(_packageId));
                }
            }
        }

        /// <summary>
        /// Exclusive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void rptBundlesExclusive_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var _crntBundleId = Convert.ToInt32((e.Item.FindControl("hdfExcBundleId") as HiddenField).Value);
            var lstBundlePackages = new List<OrderPackageSelection>();


            var cmpliancelstPkg = lstExclusiveBundlePackageData.Where(bnd => bnd.BundleId == _crntBundleId).FirstOrDefault();
            var bkgListPkgs = lstExclusiveBundlePackageData.Where(bnd => bnd.BundleId == _crntBundleId).FirstOrDefault();

            if (!cmpliancelstPkg.IsNullOrEmpty())
                lstBundlePackages.AddRange(cmpliancelstPkg.lstCompliancePkgs.ToList());

            if (!bkgListPkgs.IsNullOrEmpty())
                lstBundlePackages.AddRange(bkgListPkgs.lstBkgPkgs.ToList());

            if (applicantOrderCart.IsNotNull() && !applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty() && applicantOrderCart.lstSelectedPkgBundleId.Count > AppConsts.NONE && applicantOrderCart.lstSelectedPkgBundleId.Contains(_crntBundleId))//UAT-3283
            //if (applicantOrderCart.IsNotNull() && applicantOrderCart.SelectedPkgBundleId > AppConsts.NONE && applicantOrderCart.SelectedPkgBundleId == _crntBundleId)
            {
                //(e.Item.FindControl("chkBundles") as CheckBox).Checked = true;//UAT-3283
                (e.Item.FindControl("rbtnExclusiveBundle") as RadioButton).Checked = true;//UAT-3283
            }
            var _repeater = (e.Item.FindControl("rptExcBundlePackages") as Repeater);

            if (lstBundlePackages.Any())
            {
                _repeater.DataSource = lstBundlePackages;
                _repeater.DataBind();
            }

        }

        /// <summary>
        /// Databound for Repeater to display PAckages in a Bundle. Bind the Subscription Options for the Packages in a Bundle in this event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptBundlePackagesExclusive_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var _pkgTypeCode = (e.Item.FindControl("hdfExcMasterPackageTypeCode") as HiddenField).Value;
            var _bundleId = Convert.ToInt32((e.Item.FindControl("hdfExcNestedBundleId") as HiddenField).Value);

            // Bind the Subscription Options for the Tracking Packages only.
            if (_pkgTypeCode == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
            {
                Guid subscriptionOptionCode = new Guid(SubscriptionOptions.CustomMonthly.GetStringValue());
                var _rbtnList = (e.Item.FindControl("rbtnExcSubscriptions") as RadioButtonList);
                var _dppId = Convert.ToInt32((e.Item.FindControl("hdfExcDPPId") as HiddenField).Value);

                var _compliancePackageTypeCode = (e.Item.FindControl("hdfExcCompliancePackageTypeCode") as HiddenField).Value;

                var _dpp = CurrentViewContext.lstBundleDeptProgramPackages.Where(bnd => bnd.PBNP_PackageBundleID == _bundleId
                                                                              && bnd.PBNP_BkgPackageHierarchyMappingID == null
                                                                              && bnd.PBNP_DeptProgramPackageID == _dppId)
                                                                          .First().DeptProgramPackage;


                var _lstDPPS = _dpp.DeptProgramPackageSubscriptions.Where(dpps => dpps.SubscriptionOption.IsSystem == false
                                                                       && dpps.SubscriptionOption.Code != subscriptionOptionCode
                                                                       && dpps.DPPS_TotalPrice != null && !dpps.DPPS_IsDeleted).ToList();


                Dictionary<DeptProgramPackageSubscription, Int32?> dicDpps = new Dictionary<DeptProgramPackageSubscription, Int32?>();
                foreach (var item in _lstDPPS)
                {
                    Int32? totalDays = (item.SubscriptionOption.Year.IsNullOrEmpty() ? AppConsts.NONE : (item.SubscriptionOption.Year * 12 * 30))
                                        + (item.SubscriptionOption.Month.IsNullOrEmpty() ? AppConsts.NONE : (item.SubscriptionOption.Month * 30))
                                            + (item.SubscriptionOption.Day.IsNullOrEmpty() ? AppConsts.NONE : (item.SubscriptionOption.Day));
                    dicDpps.Add(item, totalDays);

                }



                if (!dicDpps.IsNullOrEmpty())
                {
                    dicDpps = dicDpps.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    _lstDPPS = dicDpps.Keys.ToList();
                }


                foreach (var dpps in _lstDPPS)
                {
                    Decimal _actualPrice = 0;
                    Decimal _netPrice = 0;

                    GetPricing(dpps, out _netPrice, out _actualPrice);
                    Boolean _displayPrice = ManagePriceDisplay(dpps);

                    ListItem _rbtnListItem = new ListItem
                    {
                        Text = _displayPrice ? String.Format("{0}{1}", dpps.SubscriptionOption.Label, " ($" + _netPrice.ToString("0.00") + ")") : String.Format("{0}", dpps.SubscriptionOption.Label),
                        Value = Convert.ToString(dpps.DPPS_ID)
                    };

                    if (applicantOrderCart.IsNotNull())
                    {
                        applicantOrderCart.BundleInContext = _bundleId;
                        applicantOrderCart.CurrentCompliancePackageTypeInContext = _compliancePackageTypeCode;
                        if (dpps.DPPS_ID.IsNotNull() && dpps.DPPS_ID == applicantOrderCart.DPPS_ID && !applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty() && applicantOrderCart.lstSelectedPkgBundleId.Contains(_bundleId)) //UAT-3283
                        //if (dpps.DPPS_ID.IsNotNull() && dpps.DPPS_ID == applicantOrderCart.DPPS_ID && applicantOrderCart.SelectedPkgBundleId == _bundleId)
                        {
                            _rbtnListItem.Selected = true;

                            // Enable subscriptions when re-binded so that they can be changed by applicant.
                            _rbtnList.Enabled = true;
                        }
                    }
                    _rbtnList.Items.Add(_rbtnListItem);
                }
                _rbtnList.Visible = true;
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlGenericControl divElement = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvExcElement1");

                var _bphmId = Convert.ToInt32((e.Item.FindControl("hdnExcBPHMId") as HiddenField).Value);
                var _bphm = CurrentViewContext.lstBundleBkgPackages.Where(bnd => bnd.PBNP_PackageBundleID == _bundleId
                                                                     && bnd.PBNP_BkgPackageHierarchyMappingID != null
                                                                     && bnd.PBNP_BkgPackageHierarchyMappingID == _bphmId)
                                                                 .First().BkgPackageHierarchyMapping;


                List<lkpPaymentOption> lstPaymentOptions = !_bphm.IsNullOrEmpty()
                                                            ? _bphm.BkgPackagePaymentOptions.Where(bppo => !bppo.BPPO_IsDeleted).Select(bppo => bppo.lkpPaymentOption).ToList()
                                                            : new List<lkpPaymentOption>();


                //UAt-1676 related changes.
                Boolean _displayPrice = DisplayPrice(lstPaymentOptions);
                (e.Item.FindControl("hdfExcIsDisplayPrice") as HiddenField).Value = _displayPrice.ToString();
                var basePrice = Convert.ToDecimal((e.Item.FindControl("hdfExcBkgPackageBasePrice") as HiddenField).Value);
                Literal literalControl = e.Item.FindControl("litExcPackageName") as Literal;
                literalControl.Text = _displayPrice ? String.Format("{0}{1}", literalControl.Text, " ($" + basePrice.ToString("0.00") + ")")
                                                                                : String.Format("{0}", literalControl.Text);
                divElement.Visible = _displayPrice;
            }
        }

        /// <summary>
        /// Item Command for Repeater for Packages
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptBundlePackagesExclusive_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetailsExc")
            {
                var _masterPkgTypeCode = (e.Item.FindControl("hdfExcMasterPackageTypeCode") as HiddenField).Value;
                var _packageId = (e.Item.FindControl("hdfExcPackageId") as HiddenField).Value;

                if (_masterPkgTypeCode == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
                {
                    this.CompliancePackageType = CurrentCompliancePackageType = (e.Item.FindControl("hdfExcCompliancePackageTypeCode") as HiddenField).Value;
                    RedirectToCompliancePackageDetails(Convert.ToInt32(_packageId));
                }
                else
                {
                    RedirectToBackgroundPackageDetails(Convert.ToInt32(_packageId));
                }
            }
        }

        #endregion

        #region Check Change Events to manage package selection & de-selection

        /// <summary>
        /// Clear the selected Loose Compliance and Background Packages selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnBundle_CheckedChanged(object sender, EventArgs e)
        {
            #region Clear any Loose Compliance or Backgreound Packages selected.

            foreach (var exclusiveItem in rptExclusive.Items)
            {
                RadioButton rbtnExclusive = ((exclusiveItem as RepeaterItem).FindControl("rbtnExclusive") as RadioButton);
                rbtnExclusive.Checked = false;
            }
            foreach (var nonExclusiveItem in rptNonExclusive.Items)
            {
                CheckBox chkNonExc = ((nonExclusiveItem as RepeaterItem).FindControl("chkNonExc") as CheckBox);
                chkNonExc.Checked = false;
            }

            ClearComplianceSelection(rptImmunizationPackages.Items);
            ClearComplianceSelection(rptAdminstrativePackages.Items);

            #endregion

            //UAT-3283
            Repeater _crntItemRepeater = (sender as CheckBox).NamingContainer.Parent as Repeater;
            //Repeater _crntItemRepeater = (sender as RadioButton).NamingContainer.Parent as Repeater;

            foreach (RepeaterItem rptItem in _crntItemRepeater.Items)
            {
                //var _crntRadioBtn = rptItem.FindControl("rbtnBundle") as RadioButton;
                //var _isCrntRbtnSelected = false;

                // If the checked Bundle is Same as current being iterated then 
                // Set first subscription as selected for each package
                // else clear selection of the bundle

                //if (_crntRadioBtn.Attributes["bid"] == (sender as RadioButton).Attributes["bid"])
                //{
                //    _crntRadioBtn.Checked = true;
                //    _isCrntRbtnSelected = true;
                //}
                //else
                //{
                //    _crntRadioBtn.Checked = false;
                //}

                CheckBox _crntRadioBtn = rptItem.FindControl("chkBundles") as CheckBox;
                var _isCrntRbtnSelected = false;
                if (_crntRadioBtn.Checked)
                {
                    _isCrntRbtnSelected = true;
                }

                foreach (RepeaterItem rptSubscription in (rptItem.FindControl("rptBundlePackages") as Repeater).Items)
                {
                    var _rbtnListSubscriptions = rptSubscription.FindControl("rbtnSubscriptions") as RadioButtonList;
                    if (_rbtnListSubscriptions.IsNotNull() && _rbtnListSubscriptions.Visible)
                    {
                        if (_isCrntRbtnSelected)
                        {
                            _rbtnListSubscriptions.Enabled = true;
                            if (_rbtnListSubscriptions.SelectedIndex == -1)
                            {
                                _rbtnListSubscriptions.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            _rbtnListSubscriptions.Enabled = false;
                            _rbtnListSubscriptions.ClearSelection();
                        }
                    }
                }
            }
            //RepeaterItem selectdBundle = (sender as RadioButton).NamingContainer as RepeaterItem;
            RepeaterItem selectdBundle = (sender as CheckBox).NamingContainer as RepeaterItem;

            SetSelectedHierarchyData();
            Presenter.GetPackageBundlesAvailableForOrder();
            Presenter.GetPackagelistAvailableUnderBundle();
            Tuple<decimal, Boolean> bundleCost = CalculateBundleCost(selectdBundle);
            Tuple<decimal, Boolean> screeningCost = CalculateScreeningCost();
            Session["NonExclusiveBundleCost"] = bundleCost.Item1;
            decimal totalprice = 0;
            if (!Session["ExclusiveBundleCost"].IsNullOrEmpty())
                totalprice = Convert.ToDecimal(bundleCost.Item1) + Convert.ToDecimal(Session["ExclusiveBundleCost"]);
            else
                totalprice = Convert.ToDecimal(bundleCost.Item1);
            if (totalprice > 0)
                DisplayOrderCost(totalprice, AppConsts.NONE, screeningCost.Item1, true, false, screeningCost.Item2);
            else
                DisplayOrderCost(totalprice, AppConsts.NONE, screeningCost.Item1, bundleCost.Item2, false, screeningCost.Item2);
            // Session["ExclusiveBundleCost"] = null;
        }

        /// <summary>
        /// Checked Change event for Immunization and Administrative Pacakge selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnPkg_CheckedChanged(object sender, EventArgs e)
        {
            // Clear the bundle selections and disable subscription options
            ClearBundleSelection();

            Repeater _crntItemRepeater = (sender as RadioButton).NamingContainer.Parent as Repeater;

            foreach (RepeaterItem rptItem in _crntItemRepeater.Items)
            {
                var _crntRadioBtn = rptItem.FindControl("rbtnPkg") as RadioButton;
                var _isCrntRbtnSelected = false;

                // If the checked Radiobutton is Same as current being iterated then 
                // Set first subscription as selected
                // else clear selection of the subscriptions
                if (_crntRadioBtn.Attributes["pid"] == (sender as RadioButton).Attributes["pid"])
                {
                    _crntRadioBtn.Checked = true;
                    _isCrntRbtnSelected = true;
                }
                else
                {
                    _crntRadioBtn.Checked = false;
                }

                var _rbtnListSubscriptions = rptItem.FindControl("rbtnSubscriptions") as RadioButtonList;
                if (_rbtnListSubscriptions.IsNotNull() && _rbtnListSubscriptions.Visible)
                {
                    if (_isCrntRbtnSelected)
                    {
                        // _rbtnListSubscriptions.Enabled = true;
                        //  _rbtnListSubscriptions.SelectedIndex = 0;
                        if (_rbtnListSubscriptions.SelectedIndex == -1)
                        {
                            _rbtnListSubscriptions.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        //  _rbtnListSubscriptions.Enabled = false;
                        _rbtnListSubscriptions.ClearSelection();
                    }
                }
            }

            SetSelectedHierarchyData();
            Presenter.GetDeptProgramPackage();
            Tuple<decimal, Boolean> trackingCost = CalculateTrackingCost();
            Tuple<decimal, Boolean> screeningCost = CalculateScreeningCost();
            DisplayOrderCost(AppConsts.NONE, trackingCost.Item1, screeningCost.Item1, false, trackingCost.Item2, screeningCost.Item2);
        }

        protected void chkNonExc_CheckedChanged(object sender, EventArgs e)
        {
            ClearBundleSelection();
            SetSelectedHierarchyData();
            Presenter.GetDeptProgramPackage();
            Tuple<decimal, Boolean> trackingCost = CalculateTrackingCost();
            Tuple<decimal, Boolean> screeningCost = CalculateScreeningCost();
            DisplayOrderCost(AppConsts.NONE, trackingCost.Item1, screeningCost.Item1, false, trackingCost.Item2, screeningCost.Item2);
            if (CurrentViewContext.FingerPrintData.IsLocationServiceTenant)
            {
                Int16 count = 0, countCBIdisplay = 0;

                foreach (RepeaterItem item in rptNonExclusive.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        Label lblServiceCode = item.FindControl("lblServiceCode") as Label;
                        CheckBox _rChkNonExclusive = (item.FindControl("chkNonExc") as CheckBox);
                        TextBox txtNoOfCopies = (item.FindControl("txtNoOfCopies") as TextBox);
                        //CheckBox chkIsOrderHistory = (item.FindControl("chkIsOrderHistory") as CheckBox);
                        //RadButton btnViewImages = (item.FindControl("btnViewImages") as RadButton);
                        //Panel dvOrderHistoryDetails = (item.FindControl("dvOrderHistoryDetails") as Panel);
                        //Label lblOrderHistorytext = item.FindControl("lblHistoryDetails") as Label;
                        //dvOrderHistoryDetails.Visible = false;
                        if (lblServiceCode.Text == BkgServiceType.SIMPLE.GetStringValue())
                        {
                            if (_rChkNonExclusive.Checked)
                            {
                                countCBIdisplay++;
                                dvCBIUnique.Visible = true;
                            }
                            else if (!_rChkNonExclusive.Checked && countCBIdisplay == 0)
                            {
                                dvCBIUnique.Visible = false;
                                txtCBIUniqueID.Text = String.Empty;
                                CurrentViewContext.FingerPrintData.CBIUniqueID = string.Empty;
                                dvCBIBillingCode.Visible = false;
                                txtCBIBillingCode.Text = string.Empty;
                                CurrentViewContext.FingerPrintData.BillingCode = string.Empty;
                                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                                if (applicantOrderCart.IsNotNull() &&
                                    applicantOrderCart.lstApplicantOrder.IsNotNull() &&
                                    applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNotNull() &&
                                    applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(package => package.ServiceCode == "AAAA"))
                                {
                                    int packageToRemove=applicantOrderCart.lstApplicantOrder[0].lstPackages.FindIndex(package => package.ServiceCode == "AAAA");
                                    applicantOrderCart.lstApplicantOrder[0].lstPackages.RemoveAt(packageToRemove);
                                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
                                }
                                if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.FingerPrintData.IsNullOrEmpty())
                                {
                                    applicantOrderCart.FingerPrintData.CBIUniqueID = string.Empty;
                                    applicantOrderCart.FingerPrintData.BillingCode = string.Empty;
                                }
                            }
                        }

                        if (lblServiceCode.Text != BkgServiceType.Passport_Photo.GetStringValue() && _rChkNonExclusive.Checked)
                        {
                            count++;
                        }

                        if (lblServiceCode.Text == BkgServiceType.Passport_Photo.GetStringValue() && !CurrentViewContext.FingerPrintData.IsFromArchivedOrderScreen)
                        {
                            if ((sender as CheckBox).Checked)
                            {
                                _rChkNonExclusive.Enabled = true;
                                txtNoOfCopies.Enabled = true;
                                // chkIsOrderHistory.Enabled = true;
                                //btnViewImages.Enabled = true;

                            }
                            else if (!(sender as CheckBox).Checked && count == 0)
                            {
                                _rChkNonExclusive.Enabled = false;
                                _rChkNonExclusive.Checked = false;
                                txtNoOfCopies.Enabled = false;
                                //chkIsOrderHistory.Enabled = false;
                                //chkIsOrderHistory.Checked = false;
                                txtNoOfCopies.Text = "1";
                            }
                        }
                        //if (lblServiceCode.Text != BkgServiceType.SIMPLE.GetStringValue() && _rChkNonExclusive.Checked && !lblOrderHistorytext.Text.IsNullOrEmpty())
                        //{
                        //        dvOrderHistoryDetails.Visible = true;
                        //}
                        //else
                        //{
                        //    dvOrderHistoryDetails.Visible = false;
                        //    chkIsOrderHistory.Checked = false;
                        //}
                    }
                }
            }
        }

        protected void rbtnExclusive_CheckedChanged(object sender, EventArgs e)
        {
            ClearBundleSelection();// UAT-3283
            SetSelectedHierarchyData();
            Presenter.GetDeptProgramPackage();
            Tuple<decimal, Boolean> trackingCost = CalculateTrackingCost();
            Tuple<decimal, Boolean> screeningCost = CalculateScreeningCost();
            DisplayOrderCost(AppConsts.NONE, trackingCost.Item1, screeningCost.Item1, false, trackingCost.Item2, screeningCost.Item2);

            //  System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RegisterbtnSubscriptionsImmSelectedIndexChangedEvent();", true);
        }

        protected void rbtnSubscriptionsImm_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ShowExistingOrderInformation
            //  System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowExistingOrderInformation(0);", true);
            //Session["rbtnSubscriptionsImm_SelectedIndexChanged"] = sender;
            SetSelection(sender, false);
            //  var _selectedList = (sender as RadioButtonList);
            //  var _repeaterRow = ((sender as RadioButtonList).NamingContainer);
            //  RadioButton rbtnPkg = (_repeaterRow.FindControl("rbtnPkg") as RadioButton);

            //   Session["SelectedCompliancePackage"] = rbtnPkg.Text;

        }

        protected void rbtnSubscriptionsAdmn_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSelection(sender, true);
        }

        protected void rbtnSubscriptionsBundle_SelectedIndexChanged(object sender, EventArgs e)
        {
            Repeater crntRptr = (sender as RadioButtonList).NamingContainer.Parent as Repeater;
            RepeaterItem selectdBundle = null;
            foreach (RepeaterItem rptItem in rptBundles.Items)
            {
                //var _crntRadioBtn = rptItem.FindControl("rbtnBundle") as RadioButton;
                var _crntRadioBtn = rptItem.FindControl("chkBundles") as CheckBox;  //UAT-3283
                if (_crntRadioBtn.Checked)
                {
                    selectdBundle = rptItem;
                    //break;
                }
            }

            SetSelectedHierarchyData();
            Presenter.GetPackageBundlesAvailableForOrder();
            Presenter.GetPackagelistAvailableUnderBundle();
            Tuple<decimal, Boolean> bundleCost = CalculateBundleCost(selectdBundle);
            DisplayOrderCost(bundleCost.Item1, AppConsts.NONE, AppConsts.NONE, bundleCost.Item2, false, false);
        }

        #endregion

        #region Check Change Events For Exclusive Bundle Packages to manage selection & de-selection

        /// <summary>
        /// Clear the selected Loose Compliance and Background Packages selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnExclusiveBundle_CheckedChanged(object sender, EventArgs e)
        {
            #region Clear any Loose Compliance or Backgreound Packages selected.

            foreach (var exclusiveItem in rptExclusive.Items)
            {
                RadioButton rbtnExclusive = ((exclusiveItem as RepeaterItem).FindControl("rbtnExclusive") as RadioButton);
                rbtnExclusive.Checked = false;
            }
            foreach (var nonExclusiveItem in rptNonExclusive.Items)
            {
                CheckBox chkNonExc = ((nonExclusiveItem as RepeaterItem).FindControl("chkNonExc") as CheckBox);
                chkNonExc.Checked = false;
            }

            ClearComplianceSelection(rptImmunizationPackages.Items);
            ClearComplianceSelection(rptAdminstrativePackages.Items);

            #endregion

            //UAT-3283
            //Repeater _crntItemRepeater = (sender as RadioButtonList).NamingContainer.Parent as Repeater;
            //Repeater _crntItemRepeater = (sender as RadioButton).NamingContainer.Parent as Repeater;

            foreach (RepeaterItem rptItem in rptBundlesExclusive.Items)
            {
                var _crntRadioBtn = rptItem.FindControl("rbtnExclusiveBundle") as RadioButton;
                var _isCrntRbtnSelected = false;

                //If the checked Bundle is Same as current being iterated then
                //Set first subscription as selected for each package
                // else clear selection of the bundle

                if (_crntRadioBtn.Attributes["bid"] == (sender as RadioButton).Attributes["bid"])
                {
                    _crntRadioBtn.Checked = true;
                    _isCrntRbtnSelected = true;
                }
                else
                {
                    _crntRadioBtn.Checked = false;
                }

                //RadioButtonList _crntRadioBtn = rptItem.FindControl("rbtnExclusiveBundle") as RadioButtonList;
                //var _isCrntRbtnSelected = false;
                //if (_crntRadioBtn.)
                //{
                //    _isCrntRbtnSelected = true;
                //}

                foreach (RepeaterItem rptSubscription in (rptItem.FindControl("rptExcBundlePackages") as Repeater).Items)
                {
                    var _rbtnListSubscriptions = rptSubscription.FindControl("rbtnExcSubscriptions") as RadioButtonList;
                    if (_rbtnListSubscriptions.IsNotNull() && _rbtnListSubscriptions.Visible)
                    {
                        if (_isCrntRbtnSelected)
                        {
                            _rbtnListSubscriptions.Enabled = true;
                            if (_rbtnListSubscriptions.SelectedIndex == -1)
                            {
                                _rbtnListSubscriptions.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            _rbtnListSubscriptions.Enabled = false;
                            _rbtnListSubscriptions.ClearSelection();
                        }
                    }
                }
            }
            RepeaterItem selectdBundle = (sender as RadioButton).NamingContainer as RepeaterItem;
            //RepeaterItem selectdBundle = (sender as CheckBox).NamingContainer as RepeaterItem;

            SetSelectedHierarchyData();
            Presenter.GetPackageBundlesAvailableForOrder();
            Presenter.GetPackagelistAvailableUnderBundle();
            Tuple<decimal, Boolean> bundleCost = CalculateExclusiveBundleCost(selectdBundle);
            Tuple<decimal, Boolean> screeningCost = CalculateScreeningCost();
            Session["ExclusiveBundleCost"] = bundleCost.Item1;

            decimal totalprice = 0;
            if (!Session["NonExclusiveBundleCost"].IsNullOrEmpty())
                totalprice = bundleCost.Item1 + Convert.ToDecimal(Session["NonExclusiveBundleCost"]);
            else
                totalprice = Convert.ToDecimal(bundleCost.Item1);
            DisplayOrderCost(totalprice, AppConsts.NONE, screeningCost.Item1, bundleCost.Item2, false, screeningCost.Item2);
            //Session["NonExclusiveBundleCost"] = null;
        }

        protected void rbtnExcSubscriptionsBundle_SelectedIndexChanged(object sender, EventArgs e)
        {
            Repeater crntRptr = (sender as RadioButtonList).NamingContainer.Parent as Repeater;
            RepeaterItem selectdBundle = null;
            foreach (RepeaterItem rptItem in rptBundlesExclusive.Items) // UAT-4753
            {
                var _crntRadioBtn = rptItem.FindControl("rbtnExclusiveBundle") as RadioButton;
                //  var _crntRadioBtn = rptItem.FindControl("chkBundles") as CheckBox;  //UAT-3283
                if (_crntRadioBtn.Checked)
                {
                    selectdBundle = rptItem;
                    //break;
                }
            }

            SetSelectedHierarchyData();
            Presenter.GetPackageBundlesAvailableForOrder();
            Presenter.GetPackagelistAvailableUnderBundle();
            Tuple<decimal, Boolean> bundleCost = CalculateExclusiveBundleCost(selectdBundle);
            DisplayOrderCost(bundleCost.Item1, AppConsts.NONE, AppConsts.NONE, bundleCost.Item2, false, false);
        }

        #endregion
        #region Private Methods

        /// <summary>
        /// Clear Selected Bundle and Its packages (including subscriptions for Compliance Packages)
        /// </summary>
        private void ClearBundleSelection()
        {
            foreach (var bundleItem in rptBundles.Items)
            {
                CheckBox rbtnBundle = ((bundleItem as RepeaterItem).FindControl("chkBundles") as CheckBox);  //UAT-3283
                rbtnBundle.Checked = false;
                Repeater rptBundlePackages = ((bundleItem as RepeaterItem).FindControl("rptBundlePackages") as Repeater);
                foreach (var bundlePkgitem in rptBundlePackages.Items)
                {
                    RadioButtonList rbtnSubscriptions = ((bundlePkgitem as RepeaterItem).FindControl("rbtnSubscriptions") as RadioButtonList);
                    rbtnSubscriptions.ClearSelection();
                    rbtnSubscriptions.Enabled = false;
                }
            }
            //UAT 3775 Ability to make Bundle packages exclusive (like screening packages)
            foreach (var bundleItem in rptBundlesExclusive.Items)
            {
                RadioButton rbtnExclusiveBundle = ((bundleItem as RepeaterItem).FindControl("rbtnExclusiveBundle") as RadioButton);  //UAT-3283
                rbtnExclusiveBundle.Checked = false;
                Repeater rptExcBundlePackages = ((bundleItem as RepeaterItem).FindControl("rptExcBundlePackages") as Repeater);
                foreach (var bundlePkgitem in rptExcBundlePackages.Items)
                {
                    RadioButtonList rbtnExcSubscriptions = ((bundlePkgitem as RepeaterItem).FindControl("rbtnExcSubscriptions") as RadioButtonList);
                    rbtnExcSubscriptions.ClearSelection();
                    rbtnExcSubscriptions.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Reset the selection of individual Compliance Packages (of both types) and their selected subscriptions.
        /// </summary>
        private void ClearComplianceSelection(RepeaterItemCollection rptItems)
        {
            foreach (var rptItem in rptItems)
            {
                RadioButton rbtnPkg = ((rptItem as RepeaterItem).FindControl("rbtnPkg") as RadioButton);
                rbtnPkg.Checked = false;

                RadioButtonList rbtnSubscriptions = ((rptItem as RepeaterItem).FindControl("rbtnSubscriptions") as RadioButtonList);
                rbtnSubscriptions.ClearSelection();
                // rbtnSubscriptions.Enabled = false;
            }
        }

        /// <summary>
        /// Filter Compliance packae type by Immunization or Administrative
        /// </summary>
        /// <param name="pkgType"></param>
        /// <returns></returns>
        private List<OrderPackageSelection> GetPackageDataByType(String pkgType)
        {
            var _lstPackages = DeptProgramPackages.Where(dpp => dpp.CompliancePackage.lkpCompliancePackageType.CPT_Code.Equals(pkgType) || string.IsNullOrEmpty(dpp.CompliancePackage.lkpCompliancePackageType.CPT_Code)).ToList();

            return _lstPackages.Select(pkg => new OrderPackageSelection
            {
                PackageMappingId = pkg.DPP_ID,
                PackageName = String.IsNullOrEmpty(pkg.CompliancePackage.PackageLabel) ? pkg.CompliancePackage.PackageName : pkg.CompliancePackage.PackageLabel,
                PackageId = pkg.DPP_CompliancePackageID,
                CompliancePackageTypeCode = pkgType
            }).ToList();
        }

        /// <summary>
        ///  Manage the visibility of the Tracking Package Div
        /// </summary>
        /// <param name="visibility"></param>
        private void ShowHideTrackingDiv(Boolean visibility)
        {
            HtmlGenericControl ctrl = (HtmlGenericControl)FindControl("divTrackingPackages" + controlSuffix);
            if (ctrl.IsNotNull())
            {
                ctrl.Visible = visibility;
            }
            if (!visibility)
            {
                rptImmunizationPackages.DataSource = null;
                rptImmunizationPackages.DataBind();
                CurrentViewContext.DeptProgramPackage = null;
            }
        }

        private void BindSubscriptions(RepeaterItemEventArgs e)
        {
            var _hdfDppId = e.Item.FindControl("hdfDPPId") as HiddenField;
            var _btnViewDetails = e.Item.FindControl("btnViewDetails") as WclButton;
            var _litPackageDetails = e.Item.FindControl("litPackageDetails") as Literal;
            var _litPackageDetailsAbove = e.Item.FindControl("litPackageDetailsAbove") as Literal;
            var _rbtnListSubscriptions = e.Item.FindControl("rbtnSubscriptions") as RadioButtonList;
            Guid subscriptionOptionCode = new Guid(SubscriptionOptions.CustomMonthly.GetStringValue());

            // Only if the Bundle is not selected. Else, if same packages are in the Bundle, this will set it as checked for loose packages also
            if (applicantOrderCart.IsNotNull() && applicantOrderCart.DPP_Id.IsNotNull() && applicantOrderCart.DPP_Id == Convert.ToInt32(_hdfDppId.Value)
                && (applicantOrderCart.lstSelectedPkgBundleId.IsNull() || applicantOrderCart.lstSelectedPkgBundleId.Count == AppConsts.NONE))  //UAT-3283

            //if (applicantOrderCart.IsNotNull() && applicantOrderCart.DPP_Id.IsNotNull() && applicantOrderCart.DPP_Id == Convert.ToInt32(_hdfDppId.Value)
            //    && (applicantOrderCart.SelectedPkgBundleId.IsNull() || applicantOrderCart.SelectedPkgBundleId == AppConsts.NONE))
            {
                (e.Item.FindControl("rbtnPkg") as RadioButton).Checked = true;// = applicantOrderCart.DPP_Id.ToString();
            }

            // Get the DPP for Each Compliance package in Grid
            var _deptProgramPackage = CurrentViewContext.DeptProgramPackages.Where(dpp => dpp.DPP_ID == Convert.ToInt32(_hdfDppId.Value)).First();

            var _lstDPPS = _deptProgramPackage.DeptProgramPackageSubscriptions.
                                                  Where(dpps => dpps.SubscriptionOption.IsSystem == false
                                                     && dpps.SubscriptionOption.Code != subscriptionOptionCode
                                                     && dpps.DPPS_TotalPrice != null && !dpps.DPPS_IsDeleted).ToList();

            Dictionary<DeptProgramPackageSubscription, Int32?> dicDpps = new Dictionary<DeptProgramPackageSubscription, Int32?>();
            foreach (var item in _lstDPPS)
            {
                Int32? totalDays = (item.SubscriptionOption.Year.IsNullOrEmpty() ? AppConsts.NONE : (item.SubscriptionOption.Year * 12 * 30))
                                    + (item.SubscriptionOption.Month.IsNullOrEmpty() ? AppConsts.NONE : (item.SubscriptionOption.Month * 30))
                                        + (item.SubscriptionOption.Day.IsNullOrEmpty() ? AppConsts.NONE : (item.SubscriptionOption.Day));
                dicDpps.Add(item, totalDays);

            }



            if (!dicDpps.IsNullOrEmpty())
            {
                dicDpps = dicDpps.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                _lstDPPS = dicDpps.Keys.ToList();
            }


            if (applicantOrderCart != null
               && !applicantOrderCart.lstApplicantOrder.IsNullOrEmpty()
               && !applicantOrderCart.lstApplicantOrder[0].DPPS_Id.IsNullOrEmpty()
               && !_lstDPPS.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].OrderId == 0)
            {
                cmdBar.SubmitButtonText = "Continue Order";
            }
            foreach (var dpps in _lstDPPS)
            {
                Decimal _actualPrice = 0;
                Decimal _netPrice = 0;

                GetPricing(dpps, out _netPrice, out _actualPrice);
                Boolean _displayPrice = ManagePriceDisplay(dpps);

                ListItem _rbtnListItem = new ListItem
                {
                    Text = _displayPrice ? String.Format("{0}{1}", dpps.SubscriptionOption.Label, " ($" + _netPrice.ToString("0.00") + ")") : String.Format("{0}", dpps.SubscriptionOption.Label),
                    Value = Convert.ToString(dpps.DPPS_ID)
                };

                // Only if the Bundle is not selected. Else, if same packages are in the Bundle, this will set it as checked for loose packages also
                if (applicantOrderCart.IsNotNull() && dpps.DPPS_ID.IsNotNull() && dpps.DPPS_ID == applicantOrderCart.DPPS_ID
                     && (applicantOrderCart.lstSelectedPkgBundleId.IsNull() || applicantOrderCart.lstSelectedPkgBundleId.Count == AppConsts.NONE))
                //if (applicantOrderCart.IsNotNull() && dpps.DPPS_ID.IsNotNull() && dpps.DPPS_ID == applicantOrderCart.DPPS_ID
                //     && (applicantOrderCart.SelectedPkgBundleId.IsNull() || applicantOrderCart.SelectedPkgBundleId == AppConsts.NONE))
                {
                    _rbtnListItem.Selected = true;

                    // UAT-1599
                    // Enable subscriptions when re-binded so that they can be changed by applicant.
                    //_rbtnListSubscriptions.Enabled = true;
                }
                _rbtnListSubscriptions.Items.Add(_rbtnListItem);
            }

            if (_deptProgramPackage.CompliancePackage.IsViewDetailsInOrderEnabled)
            {
                _btnViewDetails.Visible = true;
            }

            if (_deptProgramPackage.CompliancePackage.lkpPackageNotesPosition.IsNull()
                || _deptProgramPackage.CompliancePackage.lkpPackageNotesPosition.PNP_Code == PkgNotesDisplayPosition.DISPLAY_BELOW.GetStringValue())
            {
                _litPackageDetails.Text = _deptProgramPackage.CompliancePackage.PackageDetail;
                _litPackageDetailsAbove.Visible = false;
            }
            else
            {
                _litPackageDetailsAbove.Text = _deptProgramPackage.CompliancePackage.PackageDetail.HtmlEncode();
                _litPackageDetails.Visible = false;
            }

        }

        /// <summary>
        /// Returns whether the Price is to be displayed or not
        /// </summary>
        /// <param name="deptProgramPackageSubscription"></param> 
        /// <returns>Whether the price should be displayed or not, based on Payment OPtions available.</returns>
        private Boolean ManagePriceDisplay(DeptProgramPackageSubscription deptProgramPackageSubscription)
        {
            List<DeptProgramPackagePaymentOption> lstDeptProgramPackagePaymentOptions = !deptProgramPackageSubscription.DeptProgramPackage.IsNullOrEmpty()
                                                                       ? deptProgramPackageSubscription.DeptProgramPackage.DeptProgramPackagePaymentOptions
                                                                                                       .Where(cond => !cond.DPPPO_IsDeleted && !cond.DeptProgramPackage.DPP_IsDeleted)
                                                                                                       .ToList()
                                                                       : new List<DeptProgramPackagePaymentOption>();

            List<lkpPaymentOption> lstPaymentOptions = !lstDeptProgramPackagePaymentOptions.IsNullOrEmpty()
                                                        ? lstDeptProgramPackagePaymentOptions.Select(col => col.lkpPaymentOption).ToList()
                                                        : new List<lkpPaymentOption>();

            return DisplayPrice(lstPaymentOptions);
        }

        private void GetPricing(DeptProgramPackageSubscription deptProgramPackageSubscription, out Decimal netPrice, out Decimal actualPrice)
        {
            actualPrice = deptProgramPackageSubscription.DPPS_TotalPrice == null ? 0 : deptProgramPackageSubscription.DPPS_TotalPrice.Value;

            if (CurrentViewContext.PreviousOrderId != 0 && CurrentViewContext.SettlementPrice != 0)
            {
                netPrice = actualPrice - CurrentViewContext.SettlementPrice;
                if (netPrice <= 0)
                {
                    //UAT 264
                    //_netPrice = 1;
                    netPrice = 0;
                }
            }
            else
            {
                // For normal order, Net price == Actual Price
                netPrice = actualPrice;
            }
        }

        /// <summary>
        /// Bind the Packages in the Bundle
        /// </summary>
        /// 



        private Boolean BindPackageBundle()
        {
            Boolean isanyBundleAvailbleForOrder = false;
            if (!this.IsChangeSubscriptionRequest)
            {
                //UAT-2754
                Boolean IsNeedToShowPckgsNotes = Presenter.IsNeedToShowBundlePackageNotes();

                Presenter.GetPackageBundlesAvailableForOrder();
                var _lstBundleData = Presenter.GetPackagelistAvailableUnderBundle();

                if (CurrentViewContext.lstPackageBundle.IsNullOrEmpty())
                {
                    return isanyBundleAvailbleForOrder;
                }

                foreach (var bundleId in _lstBundleData.DistinctBy(bnd => bnd.PBNP_PackageBundleID).Select(bnd => bnd.PBNP_PackageBundleID).ToList())
                {
                    var _lstCurrentBundleData = _lstBundleData.Where(bnd => bnd.PBNP_PackageBundleID == bundleId).ToList();

                    BundleData _bndData = new BundleData();
                    _bndData.BundleId = bundleId;
                    _bndData.BundleName = _lstCurrentBundleData.First().PackageBundle.PBU_Name;

                    var IsExclusive = _lstCurrentBundleData.Where(g => g.PBNP_PackageBundleID == bundleId).First().PackageBundle.PackageBundleNodeMappings.Where(x => x.PBNM_IsExclusive == true).FirstOrDefault();

                    _bndData.IsExclusive = IsExclusive.IsNullOrEmpty() ? false : Convert.ToBoolean(IsExclusive.PBNM_IsExclusive);
                    _bndData.lstCompliancePkgs = new List<OrderPackageSelection>();
                    _bndData.lstBkgPkgs = new List<OrderPackageSelection>();
                    _bndData.ExplanatoryNotes = _lstCurrentBundleData.First().PackageBundle.PBU_ExplanatoryNotes;

                    var _lstCrntBndCompliancePkgs = _lstCurrentBundleData
                                                    .Where(bnd => bnd.PBNP_BkgPackageHierarchyMappingID == null && bnd.DeptProgramPackage.DPP_IsDeleted == false)
                                                    .Select(bnd => bnd.DeptProgramPackage)
                                                    .ToList();

                    foreach (var cp in _lstCrntBndCompliancePkgs)
                    {
                        _bndData.lstCompliancePkgs.Add(new OrderPackageSelection
                        {
                            PackageId = cp.DPP_CompliancePackageID,
                            PackageName = cp.CompliancePackage.PackageLabel.IsNullOrEmpty() ? cp.CompliancePackage.PackageName : cp.CompliancePackage.PackageLabel,
                            MasterPackageTypeCode = SystemPackageTypes.COMPLIANCE_PKG.GetStringValue(),
                            CompliancePackageTypeCode = cp.CompliancePackage.lkpCompliancePackageType.CPT_Code,
                            PackageMappingId = cp.DPP_ID,
                            BundleId = bundleId,
                            IsViewDetailsVisible = cp.CompliancePackage.IsViewDetailsInOrderEnabled,
                            PackageDetail = cp.CompliancePackage.PackageDetail,
                            DPMId = cp.DeptProgramMapping.DPM_ID,
                            DisplayNotesAbove = cp.CompliancePackage.lkpPackageNotesPosition.PNP_Code == PkgNotesDisplayPosition.DISPLAY_ABOVE.GetStringValue() ? true : false,
                            DisplayNotesBelow = cp.CompliancePackage.lkpPackageNotesPosition.PNP_Code == PkgNotesDisplayPosition.DISPLAY_BELOW.GetStringValue() ? true : false,
                        });
                    }

                    var _lstCrntBndBkgPkgs = _lstCurrentBundleData
                                            .Where(_bnd => _bnd.BkgPackageHierarchyMapping != null && _bnd.BkgPackageHierarchyMapping.BackgroundPackage.BPA_IsAvailableForApplicantOrder
                                                    && _bnd.PBNP_DeptProgramPackageID == null).ToList();


                    foreach (var bp in _lstCrntBndBkgPkgs)
                    {
                        _bndData.lstBkgPkgs.Add(new OrderPackageSelection
                        {
                            PackageName = bp.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label.IsNullOrEmpty()
                                                 ? bp.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name
                                                 : bp.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label,

                            PackageId = bp.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID,
                            MasterPackageTypeCode = SystemPackageTypes.BACKGROUND_PKG.GetStringValue(),
                            IsViewDetailsVisible = bp.BkgPackageHierarchyMapping.BackgroundPackage.BPA_IsViewDetailsInOrderEnabled,
                            PackageDetail = bp.BkgPackageHierarchyMapping.BackgroundPackage.BPA_PackageDetail,
                            IsExclusiveBkgPackage = bp.BkgPackageHierarchyMapping.BPHM_IsExclusive,
                            //UAT-1676.
                            //CustomPriceText = String.IsNullOrEmpty(bp.BkgPackageHierarchyMapping.BPHM_CustomPriceText)
                            //                         ? "This package costs $" + Decimal.Round(Convert.ToDecimal(bp.BkgPackageHierarchyMapping.BPHM_PackageBasePrice.Value), 2) + " and additional fees may apply."
                            //                         : bp.BkgPackageHierarchyMapping.BPHM_CustomPriceText,

                            CustomPriceText = String.IsNullOrEmpty(bp.BkgPackageHierarchyMapping.BPHM_CustomPriceText)
                                                         ? "*Additional fees may apply."
                                                         : bp.BkgPackageHierarchyMapping.BPHM_CustomPriceText,
                            BkgPackageBasePrice = bp.BkgPackageHierarchyMapping.BPHM_PackageBasePrice.Value,
                            MaxNumberOfYearforResidence = bp.BkgPackageHierarchyMapping.BPHM_MaxNumberOfYearforResidence.HasValue ?
                                                                 bp.BkgPackageHierarchyMapping.BPHM_MaxNumberOfYearforResidence.Value : -1,
                            BPHMId = bp.BkgPackageHierarchyMapping.BPHM_ID,
                            IsInvoiceOnlyAtPackageLevel = bp.BkgPackageHierarchyMapping.BkgPackagePaymentOptions.Any(),
                            BundleId = bundleId,
                            DisplayNotesAbove = bp.BkgPackageHierarchyMapping.BackgroundPackage.lkpPackageNotesPosition.PNP_Code == PkgNotesDisplayPosition.DISPLAY_ABOVE.GetStringValue() && IsNeedToShowPckgsNotes ? true : false,
                            DisplayNotesBelow = bp.BkgPackageHierarchyMapping.BackgroundPackage.lkpPackageNotesPosition.PNP_Code == PkgNotesDisplayPosition.DISPLAY_BELOW.GetStringValue() && IsNeedToShowPckgsNotes ? true : false,
                            //UAT-3268
                            IsReqToQualifyInRotation = bp.BkgPackageHierarchyMapping.BackgroundPackage.BPA_IsReqToQualifyInRotation,
                            AdditionalPrice = bp.BkgPackageHierarchyMapping.BPHM_AdditionalPrice,
                        });
                    }
                    _lstBundlePackageData.Add(_bndData);

                    List<BundleData> _lstNonExclusiveBundlePackage = _lstBundlePackageData.Where(bp => !bp.IsExclusive).ToList();
                    if (_bndData.IsExclusive)
                    {
                        lstExclusiveBundlePackageData.Add(_bndData);
                    }
                    else
                    {
                        lstNonExclusiveBundlePackageData.Add(_bndData);
                    }
                    //UAT-3268
                    RotQualifyBkgPKgInBndlDisplay(_lstBundlePackageData);
                }

                if (lstExclusiveBundlePackageData.IsNullOrEmpty() && lstNonExclusiveBundlePackageData.IsNullOrEmpty())
                {
                    pnlBundlesPackages.Visible = false;
                }
                else
                {
                    pnlBundlesPackages.Visible = true;
                }

                if (!lstNonExclusiveBundlePackageData.IsNullOrEmpty())
                {
                    rptBundles.Visible = true;
                    rptBundles.DataSource = lstNonExclusiveBundlePackageData.Where(cond => cond.IsExclusive == false && cond.lstBkgPkgs.Any() || cond.lstCompliancePkgs.Any()).Any() ? lstNonExclusiveBundlePackageData.Where(cond => cond.lstBkgPkgs.Any() || cond.lstCompliancePkgs.Any()) : new List<BundleData>();
                }
                else
                {
                    rptBundles.DataSource = null;
                    rptBundles.Visible = false;

                }
                rptBundles.DataBind();

                List<BundleData> _lstExclusiveBundlePackage = _lstBundlePackageData.Where(bp => bp.IsExclusive).ToList();


                if (!lstExclusiveBundlePackageData.IsNullOrEmpty())
                {
                    rptBundlesExclusive.Visible = true;
                    rptBundlesExclusive.DataSource = lstExclusiveBundlePackageData.Where(cond => cond.IsExclusive == true && cond.lstBkgPkgs.Any() || cond.lstCompliancePkgs.Any()).Any() ? lstExclusiveBundlePackageData.Where(cond => cond.lstBkgPkgs.Any() || cond.lstCompliancePkgs.Any()) : new List<BundleData>();
                }
                else
                {
                    rptBundlesExclusive.DataSource = null;
                    rptBundlesExclusive.Visible = false;
                }
                rptBundlesExclusive.DataBind();


                divBundles.Visible = true;
                isanyBundleAvailbleForOrder = true;
            }
            return isanyBundleAvailbleForOrder;
        }

        /// <summary>
        /// Returns whether the price should be displayed or not, bsaed on the Payment options available.
        /// </summary>
        /// <param name="lstPaymentOptions"></param>
        /// <returns></returns>
        private Boolean DisplayPrice(List<lkpPaymentOption> lstPaymentOptions)
        {
            Boolean _displayPrice = true;

            if (!lstPaymentOptions.IsNullOrEmpty())
            {
                // Hide Price 
                // if there is only 1 Payment option and it is of type InvoiceWithApproval
                // if there are 2 Payment option and of type InvoiceWithApproval && InvoiceWithOutApproval
                if (
                    (lstPaymentOptions.Count == 1 && (lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()
                        || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue())))
                        ||
                     (lstPaymentOptions.Count == 2 && (lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue())
                                                   && lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))))
                {
                    _displayPrice = false;
                }
            }
            else if (IfInvoiceOnlyPymnOptn)
            {
                _displayPrice = false;
            }
            return _displayPrice;
        }

        #endregion



        /// <summary>
        /// On selection of the Subscription, clear all the Immn/Admn. Package selection and 
        /// set the currently selected subscription as selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isAdministrative"></param>
        private void SetSelection(object sender, Boolean isAdministrative)
        {
            var _selectedList = (sender as RadioButtonList);
            var _selectedValue = _selectedList.SelectedValue;

            // Clear the bundle selections and disable subscription options
            ClearBundleSelection();

            if (isAdministrative)
            {
                ClearComplianceSelection(rptAdminstrativePackages.Items);
            }
            else
            {
                ClearComplianceSelection(rptImmunizationPackages.Items);
            }
            var _repeaterRow = ((sender as RadioButtonList).NamingContainer);

            (_repeaterRow.FindControl("rbtnPkg") as RadioButton).Checked = true;
            _selectedList.SelectedValue = _selectedValue;

            SetSelectedHierarchyData();
            Presenter.GetDeptProgramPackage();
            Tuple<decimal, Boolean> trackingCost = CalculateTrackingCost();
            Tuple<decimal, Boolean> screeningCost = CalculateScreeningCost();
            DisplayOrderCost(AppConsts.NONE, trackingCost.Item1, screeningCost.Item1, false, trackingCost.Item2, screeningCost.Item2);
            // System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RegisterbtnSubscriptionsImmSelectedIndexChangedEvent();", true);
        }


        private Tuple<decimal, Boolean> CalculateBundleCost(RepeaterItem selectedrepeaterItem)
        {
            decimal totalBundlePrice = AppConsts.NONE;

            Boolean ifAnyBundleSelected = false;

            Repeater _crntItemRepeater = selectedrepeaterItem.Parent as Repeater;
            if (!_crntItemRepeater.IsNullOrEmpty())
            {
                foreach (RepeaterItem repeaterItem in _crntItemRepeater.Items)
                {
                    var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfBundleId") as HiddenField).Value);
                    // If the Bundle is Selected and ID is equal to the one added in Cart, before calling this function.
                    //if ((repeaterItem.FindControl("rbtnBundle") as RadioButton).Checked)
                    if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked)
                    {
                        ifAnyBundleSelected = true;
                        RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptBundlePackages") as Repeater).Items;

                        foreach (RepeaterItem rptItem in _rptItems)
                        {
                            var _masterTypeCode = (rptItem.FindControl("hdfMasterPackageTypeCode") as HiddenField).Value;

                            if (_masterTypeCode == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
                            {
                                totalBundlePrice = CalculatePriceOfTrackingPkg(totalBundlePrice, rptItem);
                            }
                            else
                            {
                                Boolean _displayPrice = Convert.ToBoolean(((rptItem as RepeaterItem).FindControl("hdfIsDisplayPrice") as HiddenField).Value);
                                if (_displayPrice)
                                {
                                    Decimal _packageBasePrice = Convert.ToDecimal(((rptItem as RepeaterItem).FindControl("hdfBkgPackageBasePrice") as HiddenField).Value);
                                    totalBundlePrice = totalBundlePrice + _packageBasePrice;
                                }
                            }
                        }
                    }
                }
            }
            return new Tuple<decimal, bool>(totalBundlePrice, ifAnyBundleSelected);
        }

        private Tuple<decimal, Boolean> CalculateExclusiveBundleCost(RepeaterItem selectedrepeaterItem)
        {
            decimal totalBundlePrice = AppConsts.NONE;

            Boolean ifAnyBundleSelected = false;

            Repeater _crntItemRepeater = selectedrepeaterItem.Parent as Repeater;
            if (!_crntItemRepeater.IsNullOrEmpty())
            {
                foreach (RepeaterItem repeaterItem in _crntItemRepeater.Items)
                {
                    var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfExcBundleId") as HiddenField).Value);
                    // If the Bundle is Selected and ID is equal to the one added in Cart, before calling this function.
                    if ((repeaterItem.FindControl("rbtnExclusiveBundle") as RadioButton).Checked)
                    //  if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked)
                    {
                        ifAnyBundleSelected = true;
                        RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptExcBundlePackages") as Repeater).Items;

                        foreach (RepeaterItem rptItem in _rptItems)
                        {
                            var _masterTypeCode = (rptItem.FindControl("hdfExcMasterPackageTypeCode") as HiddenField).Value;

                            if (_masterTypeCode == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
                            {
                                totalBundlePrice = CalculatePriceOfTrackingPkg(totalBundlePrice, rptItem);
                            }
                            else
                            {
                                Boolean _displayPrice = Convert.ToBoolean(((rptItem as RepeaterItem).FindControl("hdfExcIsDisplayPrice") as HiddenField).Value);
                                if (_displayPrice)
                                {
                                    Decimal _packageBasePrice = Convert.ToDecimal(((rptItem as RepeaterItem).FindControl("hdfExcBkgPackageBasePrice") as HiddenField).Value);
                                    totalBundlePrice = totalBundlePrice + _packageBasePrice;
                                }
                            }
                        }
                    }
                }
            }
            return new Tuple<decimal, bool>(totalBundlePrice, ifAnyBundleSelected);
        }
        private Tuple<decimal, Boolean> CalculateTrackingCost()
        {
            decimal totalTrackingCost = AppConsts.NONE;
            Boolean ifAnyTrackingPkgSelected = false;
            if (divTracking.Visible)
            {
                foreach (RepeaterItem rptItem in rptImmunizationPackages.Items)
                {
                    if ((rptItem.FindControl("rbtnPkg") as RadioButton).Checked)
                    {
                        ifAnyTrackingPkgSelected = true;
                        totalTrackingCost = CalculatePriceOfTrackingPkg(totalTrackingCost, rptItem);
                        break;
                    }
                }

                foreach (RepeaterItem rptItem in rptAdminstrativePackages.Items)
                {
                    if ((rptItem.FindControl("rbtnPkg") as RadioButton).Checked)
                    {
                        ifAnyTrackingPkgSelected = true;
                        totalTrackingCost = CalculatePriceOfTrackingPkg(totalTrackingCost, rptItem);
                        break;
                    }
                }
            }
            return new Tuple<decimal, bool>(totalTrackingCost, ifAnyTrackingPkgSelected);
        }

        private Tuple<decimal, Boolean> CalculateScreeningCost()
        {
            decimal totalScreeningCost = AppConsts.NONE;
            Boolean ifAnyScreeningPkgSelected = false;
            if (divBackgroundPackages.Visible)
            {
                foreach (var rptItem in rptNonExclusive.Items)
                {
                    CheckBox _chkNonExclusive = ((rptItem as RepeaterItem).FindControl("chkNonExc") as CheckBox);
                    if (_chkNonExclusive.IsNotNull() && _chkNonExclusive.Checked)
                    {
                        ifAnyScreeningPkgSelected = true;
                        PackagePasscodeId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnNonExcBPAId") as HiddenField).Value);
                        System.Web.UI.HtmlControls.HtmlGenericControl htmlDivControl = (System.Web.UI.HtmlControls.HtmlGenericControl)(rptItem as RepeaterItem).FindControl("divNonExcPackagePasscode");
                        Boolean _displayPrice = Convert.ToBoolean(((rptItem as RepeaterItem).FindControl("hdfIsDisplayPrice") as HiddenField).Value);

                        if (_displayPrice)
                        {
                            Decimal _packageBasePrice = Convert.ToDecimal(((rptItem as RepeaterItem).FindControl("hdfNonExcBasePrice") as HiddenField).Value);
                            totalScreeningCost = totalScreeningCost + _packageBasePrice;
                        }
                        if (IsPackagePasscodeMatched(PackagePasscodeId, String.Empty, false))
                        {
                            htmlDivControl.Attributes.Add("style", "display: block;");
                        }
                        else
                        {
                            htmlDivControl.Attributes.Add("style", "display: none;");
                        }
                    }
                    else if (!_chkNonExclusive.Checked)
                    {
                        System.Web.UI.HtmlControls.HtmlGenericControl htmlDivControl = (System.Web.UI.HtmlControls.HtmlGenericControl)(rptItem as RepeaterItem).FindControl("divNonExcPackagePasscode");
                        htmlDivControl.Attributes.Add("style", "display: none;");
                    }
                }

                foreach (var rptItem in rptExclusive.Items)
                {
                    RadioButton _rBtnExclusive = ((rptItem as RepeaterItem).FindControl("rbtnExclusive") as RadioButton);
                    if (_rBtnExclusive.IsNotNull() && _rBtnExclusive.Checked)
                    {
                        ifAnyScreeningPkgSelected = true;
                        PackagePasscodeId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnExcBPAId") as HiddenField).Value);
                        Boolean _displayPrice = Convert.ToBoolean(((rptItem as RepeaterItem).FindControl("hdfIsDisplayPrice") as HiddenField).Value);
                        System.Web.UI.HtmlControls.HtmlGenericControl htmlDivControl = (System.Web.UI.HtmlControls.HtmlGenericControl)(rptItem as RepeaterItem).FindControl("divExcPackagePasscode"); //("dvElement1");                            
                        if (_displayPrice)
                        {
                            Decimal _packageBasePrice = Convert.ToDecimal(((rptItem as RepeaterItem).FindControl("hdfExcBasePrice") as HiddenField).Value);
                            totalScreeningCost = totalScreeningCost + _packageBasePrice;
                        }
                        if (IsPackagePasscodeMatched(PackagePasscodeId, String.Empty, false))
                        {
                            htmlDivControl.Attributes.Add("style", "display: block;");
                        }
                    }
                    else if (!_rBtnExclusive.Checked)
                    {
                        System.Web.UI.HtmlControls.HtmlGenericControl htmlDivControl = (System.Web.UI.HtmlControls.HtmlGenericControl)(rptItem as RepeaterItem).FindControl("divExcPackagePasscode"); //("dvElement1");   
                        htmlDivControl.Attributes.Add("style", "display: none;");
                    }

                }
            }
            return new Tuple<decimal, bool>(totalScreeningCost, ifAnyScreeningPkgSelected);
        }

        private decimal CalculatePriceOfTrackingPkg(decimal totalPrice, RepeaterItem rptItem)
        {
            var _rbtnListSubscriptions = (rptItem.FindControl("rbtnSubscriptions") as RadioButtonList);

            var _rbtnListExecSubscription = (rptItem.FindControl("rbtnExcSubscriptions") as RadioButtonList);

            if (!_rbtnListSubscriptions.IsNullOrEmpty())
            {

                if (_rbtnListSubscriptions.SelectedIndex == -1)
                {
                    _rbtnListSubscriptions.SelectedIndex = 0;
                }
                Int32 _dppsId = Convert.ToInt32(_rbtnListSubscriptions.SelectedValue);
                var _dppId = Convert.ToInt32((rptItem.FindControl("hdfDPPId") as HiddenField).Value);
                var _selectedDeptProgramPackage = CurrentViewContext.DeptProgramPackages.Where(dpp => dpp.DPP_ID == _dppId).First();
                DeptProgramPackageSubscription _selectedDpps = _selectedDeptProgramPackage.DeptProgramPackageSubscriptions.Where(dpps => dpps.DPPS_ID == _dppsId).First();

                Boolean displayPrice = ManagePriceDisplay(_selectedDpps);
                if (displayPrice)
                {
                    var actualPrice = _selectedDpps.DPPS_TotalPrice == null ? 0 : _selectedDpps.DPPS_TotalPrice.Value;
                    totalPrice = totalPrice + actualPrice;
                }
            }
            else if (!_rbtnListExecSubscription.IsNullOrEmpty())
            {
                if (_rbtnListExecSubscription.SelectedIndex == -1)
                {
                    _rbtnListExecSubscription.SelectedIndex = 0;
                }
                Int32 _dppsId = Convert.ToInt32(_rbtnListExecSubscription.SelectedValue);
                var _dppId = Convert.ToInt32((rptItem.FindControl("hdfExcDPPId") as HiddenField).Value);
                var _selectedDeptProgramPackage = CurrentViewContext.DeptProgramPackages.Where(dpp => dpp.DPP_ID == _dppId).First();
                DeptProgramPackageSubscription _selectedDpps = _selectedDeptProgramPackage.DeptProgramPackageSubscriptions.Where(dpps => dpps.DPPS_ID == _dppsId).First();

                Boolean displayPrice = ManagePriceDisplay(_selectedDpps);
                if (displayPrice)
                {
                    var actualPrice = _selectedDpps.DPPS_TotalPrice == null ? 0 : _selectedDpps.DPPS_TotalPrice.Value;
                    totalPrice = totalPrice + actualPrice;
                }
            }
            return totalPrice;
        }

        private void DisplayOrderCost(Decimal bundleCost, Decimal trackingCost, Decimal screeningCost,
                                                          Boolean ifAnyBundleSelected, Boolean ifAnytrackingSelected, Boolean ifAnyscreeningSelected)
        {
            lblBundleCost.Text = ifAnyBundleSelected ? "$ " + Convert.ToString(Decimal.Round(bundleCost, 2)) : String.Empty;
            lblScreeningCost.Text = ifAnyscreeningSelected ? "$ " + Convert.ToString(Decimal.Round(screeningCost, 2)) : String.Empty;
            lblTrackingCost.Text = ifAnytrackingSelected ? "$ " + Convert.ToString(Decimal.Round(trackingCost, 2)) : String.Empty;

            decimal totalCost = AppConsts.NONE;
            if (ifAnyBundleSelected || ifAnytrackingSelected || ifAnyscreeningSelected)
            {
                totalCost = bundleCost + trackingCost + screeningCost;
                lblTotalCost.Text = "$ " + Convert.ToString(Decimal.Round(totalCost, 2));
            }
            else
            {
                lblTotalCost.Text = String.Empty;
            }

        }

        #endregion

        #region UAT-2587:-Popup prompts for screening orders that will have multiple emails sent

        private void AcknowledgeMessagePopUpBind()
        {

            List<BackgroundPackagesContract> _bkgPackage = new List<BackgroundPackagesContract>();
            List<BackroundServicesContract> lstBkgServices = new List<BackroundServicesContract>();

            foreach (RepeaterItem repeaterItem in rptBundles.Items)
            {
                var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfBundleId") as HiddenField).Value);

                //if ((repeaterItem.FindControl("rbtnBundle") as RadioButton).Checked)
                if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked)
                {
                    RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptBundlePackages") as Repeater).Items;
                    foreach (RepeaterItem rptItem in _rptItems)
                    {
                        var _masterTypeCode = (rptItem.FindControl("hdfMasterPackageTypeCode") as HiddenField).Value;
                        if (_masterTypeCode == SystemPackageTypes.BACKGROUND_PKG.GetStringValue())
                        {

                            Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfPackageId") as HiddenField).Value);

                            _bkgPackage.Add(new BackgroundPackagesContract
                            {
                                BPAId = _packageId,
                            });
                        }
                    }
                    // break;
                }
            }

            foreach (var rptItem in rptNonExclusive.Items)
            {
                CheckBox _chkNonExclusive = ((rptItem as RepeaterItem).FindControl("chkNonExc") as CheckBox);
                if (_chkNonExclusive.IsNotNull() && _chkNonExclusive.Checked)
                {
                    Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnNonExcBPAId") as HiddenField).Value);

                    _bkgPackage.Add(new BackgroundPackagesContract
                    {
                        BPAId = _packageId,
                    });
                }
            }

            foreach (var rptItem in rptExclusive.Items)
            {
                RadioButton _rBtnExclusive = ((rptItem as RepeaterItem).FindControl("rbtnExclusive") as RadioButton);
                if (_rBtnExclusive.IsNotNull() && _rBtnExclusive.Checked)
                {
                    Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnExcBPAId") as HiddenField).Value);

                    _bkgPackage.Add(new BackgroundPackagesContract
                    {
                        BPAId = _packageId,
                    });

                    //if (CurrentViewContext.SelectedPkgBundleId == AppConsts.NONE)
                    //    break;
                }
            }
            //UAT 3775 Ability to make Bundle packages exclusive (like screening packages)
            foreach (RepeaterItem repeaterItem in rptBundlesExclusive.Items)
            {
                var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfExcBundleId") as HiddenField).Value);

                if ((repeaterItem.FindControl("rbtnExclusiveBundle") as RadioButton).Checked)
                //if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked)
                {
                    RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptExcBundlePackages") as Repeater).Items;
                    foreach (RepeaterItem rptItem in _rptItems)
                    {
                        var _masterTypeCode = (rptItem.FindControl("hdfExcMasterPackageTypeCode") as HiddenField).Value;
                        if (_masterTypeCode == SystemPackageTypes.BACKGROUND_PKG.GetStringValue())
                        {

                            Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfExcPackageId") as HiddenField).Value);

                            _bkgPackage.Add(new BackgroundPackagesContract
                            {
                                BPAId = _packageId,
                            });
                        }
                    }
                    // break;
                }
            }

            if (!_bkgPackage.IsNullOrEmpty() || _bkgPackage.Select(Sel => Sel.BPAId).Count() > AppConsts.NONE)
            {
                List<Int32> lstBPAIds = _bkgPackage.Select(Sel => Sel.BPAId).ToList();
                String bkgPkgIds = String.Join(",", lstBPAIds);
                var SelectedNodeId = GetSelectedHierarchyNodeId();
                lstBkgServices = Presenter.AcknowledgeMessagePopUpContent(bkgPkgIds, Convert.ToInt32(SelectedNodeId));
            }

            if (!lstBkgServices.IsNullOrEmpty())
            {
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Style.Add("float", "left");
                HtmlGenericControl ul = new HtmlGenericControl("ul");

                lstBkgServices.ForEach(x =>
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    li.InnerText = x.ServicreName;
                    li.Style["list-style"] = "disc";
                    ul.Controls.Add(li);
                });
                ul.Style["padding-left"] = "30px";
                div.Controls.Add(ul);
                pnlServices.Controls.Add(div);

                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "AcknowledgeMessage();", true);
                return;
            }
            else
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);

                if (!applicantOrderCart.IsNullOrEmpty() && (hdfIsConfirm.Value == "1" || applicantOrderCart.IsOrderFlowConfirmation))
                {
                    applicantOrderCart.IsOrderFlowConfirmation = true;
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
                }
                StartOrder();
            }
        }
        #endregion

        #region UAT-3268

        public void RotationQualifyBkgPKgDisplay(List<BackgroundPackagesContract> lstBkpPkgsToQualifyRot)
        {
            foreach (RepeaterItem repeaterItem in rptBundles.Items)
            {
                var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfBundleId") as HiddenField).Value);

                //if ((repeaterItem.FindControl("rbtnBundle") as RadioButton).Checked)
                if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked)
                {
                    RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptBundlePackages") as Repeater).Items;
                    foreach (RepeaterItem rptItem in _rptItems)
                    {
                        var _masterTypeCode = (rptItem.FindControl("hdfMasterPackageTypeCode") as HiddenField).Value;
                        if (_masterTypeCode == SystemPackageTypes.BACKGROUND_PKG.GetStringValue())
                        {
                            Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfPackageId") as HiddenField).Value);
                            WclButton lnkBtn = ((rptItem as RepeaterItem).FindControl("btnViewBundlePackage") as WclButton);
                            if (lstBkpPkgsToQualifyRot.Where(cond => cond.BPAId == _packageId).Any())
                            {
                                if (_presenter.IsNoServicesExistInPackage(_packageId, CurrentViewContext.TenantId))//UAT-3706
                                {
                                    lnkBtn.Enabled = false;
                                    lnkBtn.ToolTip = "This package does not have any service(s).";
                                }
                            }
                        }
                    }
                    break;
                }
            }

            foreach (var rptItem in rptNonExclusive.Items)
            {
                CheckBox _chkNonExclusive = ((rptItem as RepeaterItem).FindControl("chkNonExc") as CheckBox);
                if (_chkNonExclusive.IsNotNull())
                {
                    Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnNonExcBPAId") as HiddenField).Value);
                    WclButton lnkBtn = ((rptItem as RepeaterItem).FindControl("btnViewDetails") as WclButton);
                    Label lblNonExc = (rptItem as RepeaterItem).FindControl("lblNonExc") as Label;
                    if (lstBkpPkgsToQualifyRot.Where(cond => cond.BPAId == _packageId).Any())
                    {
                        if (_presenter.IsNoServicesExistInPackage(_packageId, CurrentViewContext.TenantId))//UAT-3706
                        {
                            lblNonExc.Visible = false;
                            lnkBtn.Enabled = false;
                            lnkBtn.ToolTip = "This package does not have any service(s).";
                        }
                    }
                }
            }

            foreach (var rptItem in rptExclusive.Items)
            {
                RadioButton _rBtnExclusive = ((rptItem as RepeaterItem).FindControl("rbtnExclusive") as RadioButton);
                if (_rBtnExclusive.IsNotNull())
                {
                    Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnExcBPAId") as HiddenField).Value);
                    WclButton lnkBtn = ((rptItem as RepeaterItem).FindControl("btnViewDetails") as WclButton);
                    Label lblExclusive = (rptItem as RepeaterItem).FindControl("lblExclusive") as Label;
                    //RadioButton rBtnExclusive = ((rptItem as RepeaterItem).FindControl("rbtnExclusive") as RadioButton);
                    var basePrice = Convert.ToDecimal(((rptItem as RepeaterItem).FindControl("hdfExcBasePrice") as HiddenField).Value);
                    if (lstBkpPkgsToQualifyRot.Where(cond => cond.BPAId == _packageId).Any())
                    {
                        if (_presenter.IsNoServicesExistInPackage(_packageId, CurrentViewContext.TenantId))//UAT-3706
                        {
                            //lblExclusive.Text = lblExclusive.Text.Replace(" ($" + basePrice.ToString("0.00") + ")", "");
                            lnkBtn.Enabled = false;
                            lnkBtn.ToolTip = "This package does not have any service(s).";
                        }
                    }
                }
            }
            //UAT 3775 Ability to make Bundle packages exclusive (like screening packages)
            foreach (RepeaterItem repeaterItem in rptBundlesExclusive.Items)
            {
                var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfExcBundleId") as HiddenField).Value);

                if ((repeaterItem.FindControl("rbtnExclusiveBundle") as RadioButton).Checked)
                // if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked)
                {
                    RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptExcBundlePackages") as Repeater).Items;
                    foreach (RepeaterItem rptItem in _rptItems)
                    {
                        var _masterTypeCode = (rptItem.FindControl("hdfExcMasterPackageTypeCode") as HiddenField).Value;
                        if (_masterTypeCode == SystemPackageTypes.BACKGROUND_PKG.GetStringValue())
                        {
                            Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfExcPackageId") as HiddenField).Value);
                            WclButton lnkBtn = ((rptItem as RepeaterItem).FindControl("btnExcViewBundlePackage") as WclButton);
                            if (lstBkpPkgsToQualifyRot.Where(cond => cond.BPAId == _packageId).Any())
                            {
                                if (_presenter.IsNoServicesExistInPackage(_packageId, CurrentViewContext.TenantId))//UAT-3706
                                {
                                    lnkBtn.Enabled = false;
                                    lnkBtn.ToolTip = "This package does not have any service(s).";
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }

        public void RotQualifyBkgPKgInBndlDisplay(List<BundleData> lstBundlePackageData)
        {
            foreach (RepeaterItem repeaterItem in rptBundles.Items)
            {
                var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfBundleId") as HiddenField).Value);
                RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptBundlePackages") as Repeater).Items;
                foreach (RepeaterItem rptItem in _rptItems)
                {
                    var _masterTypeCode = (rptItem.FindControl("hdfMasterPackageTypeCode") as HiddenField).Value;
                    if (_masterTypeCode == SystemPackageTypes.BACKGROUND_PKG.GetStringValue())
                    {

                        Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfPackageId") as HiddenField).Value);
                        WclButton lnkBtn = ((rptItem as RepeaterItem).FindControl("btnViewBundlePackage") as WclButton);
                        Label lblExclusive = (rptItem as RepeaterItem).FindControl("lblExclusive") as Label;
                        var res = lstBundlePackageData.Where(cond => true).Any();
                        if (lstBundlePackageData.Where(cond => cond.lstBkgPkgs.Where(bkg => bkg.IsReqToQualifyInRotation && bkg.PackageId == _packageId).Any()).Any())
                        {
                            if (_presenter.IsNoServicesExistInPackage(_packageId, CurrentViewContext.TenantId))//UAT-3706
                            {
                                lnkBtn.Enabled = false;
                                lnkBtn.ToolTip = "This package does not have any service(s).";
                            }
                        }
                    }
                }
                break;
            }
            foreach (RepeaterItem repeaterItem in rptBundlesExclusive.Items)
            {
                var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfExcBundleId") as HiddenField).Value);

                // if ((repeaterItem.FindControl("rbtnExclusiveBundle") as RadioButton).Checked)
                // if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked)
                //{
                RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptExcBundlePackages") as Repeater).Items;
                foreach (RepeaterItem rptItem in _rptItems)
                {
                    var _masterTypeCode = (rptItem.FindControl("hdfExcMasterPackageTypeCode") as HiddenField).Value;
                    if (_masterTypeCode == SystemPackageTypes.BACKGROUND_PKG.GetStringValue())
                    {
                        Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfExcPackageId") as HiddenField).Value);
                        WclButton lnkBtn = ((rptItem as RepeaterItem).FindControl("btnExcViewBundlePackage") as WclButton);
                        // Label lblExclusive = (rptItem as RepeaterItem).FindControl("lblExclusive") as Label;
                        var res = lstBundlePackageData.Where(cond => true).Any();
                        if (lstBundlePackageData.Where(cond => cond.lstBkgPkgs.Where(bkg => bkg.IsReqToQualifyInRotation && bkg.PackageId == _packageId).Any()).Any())
                        {
                            if (_presenter.IsNoServicesExistInPackage(_packageId, CurrentViewContext.TenantId))//UAT-3706
                            {
                                lnkBtn.Enabled = false;
                                lnkBtn.ToolTip = "This package does not have any service(s).";
                            }
                        }
                    }
                }
                // break;
                // }
            }
        }
        #endregion

        #region UAT-3283

        private Boolean IsSamePkgInMultipleSelectedBundles()
        {
            List<Int32> lstCompliancePkgsIds = new List<Int32>();

            List<Int32> lstBkgPkgsIds = new List<Int32>();

            foreach (RepeaterItem repeaterItem in rptBundles.Items)
            {
                var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfBundleId") as HiddenField).Value);

                //if ((repeaterItem.FindControl("rbtnBundle") as RadioButton).Checked)
                if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked)
                {
                    RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptBundlePackages") as Repeater).Items;

                    foreach (RepeaterItem rptItem in _rptItems)
                    {
                        var _masterTypeCode = (rptItem.FindControl("hdfMasterPackageTypeCode") as HiddenField).Value;

                        if (_masterTypeCode == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
                        {
                            Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfPackageId") as HiddenField).Value);
                            if (!lstCompliancePkgsIds.Contains(_packageId))
                            {
                                lstCompliancePkgsIds.Add(_packageId);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        if (_masterTypeCode == SystemPackageTypes.BACKGROUND_PKG.GetStringValue())
                        {
                            Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfPackageId") as HiddenField).Value);
                            if (!lstBkgPkgsIds.Contains(_packageId))
                            {
                                lstBkgPkgsIds.Add(_packageId);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            //UAT 3775 Ability to make Bundle packages exclusive (like screening packages)
            foreach (RepeaterItem repeaterItem in rptBundlesExclusive.Items)
            {
                var _bundleId = Convert.ToInt32((repeaterItem.FindControl("hdfExcBundleId") as HiddenField).Value);

                if ((repeaterItem.FindControl("rbtnExclusiveBundle") as RadioButton).Checked)
                // if ((repeaterItem.FindControl("chkBundles") as CheckBox).Checked)
                {
                    RepeaterItemCollection _rptItems = (repeaterItem.FindControl("rptExcBundlePackages") as Repeater).Items;

                    foreach (RepeaterItem rptItem in _rptItems)
                    {
                        var _masterTypeCode = (rptItem.FindControl("hdfExcMasterPackageTypeCode") as HiddenField).Value;

                        if (_masterTypeCode == SystemPackageTypes.COMPLIANCE_PKG.GetStringValue())
                        {
                            Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfExcPackageId") as HiddenField).Value);
                            if (!lstCompliancePkgsIds.Contains(_packageId))
                            {
                                lstCompliancePkgsIds.Add(_packageId);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        if (_masterTypeCode == SystemPackageTypes.BACKGROUND_PKG.GetStringValue())
                        {
                            Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdfExcPackageId") as HiddenField).Value);
                            if (!lstBkgPkgsIds.Contains(_packageId))
                            {
                                lstBkgPkgsIds.Add(_packageId);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// IsEnteredPassCodeMatched
        /// </summary>
        //private Boolean IsEnteredPassCodeMatchedForBkgPackage()
        //{
        //    Boolean IsEnteredBkgPackagePassCodeMatched = true;
        //    if (!divBackgroundPackages.Visible)
        //    {
        //        if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
        //        {
        //            applicantOrderCart.lstApplicantOrder[0].lstPackages = null;
        //        }
        //        return false;
        //    }

        //    List<BackgroundPackagesContract> _lstAll = _presenter.GetBackgroundPackages(CurrentViewContext.SelectedHierarchyNodeIds,
        //                                                 CurrentViewContext.CurrentLoggedInUserId, CurrentViewContext.TenantId);

        //    foreach (var rptItem in rptNonExclusive.Items)
        //    {
        //        CheckBox _chkNonExclusive = ((rptItem as RepeaterItem).FindControl("chkNonExc") as CheckBox);
        //        if (_chkNonExclusive.IsNotNull() && _chkNonExclusive.Checked)
        //        {
        //            Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnNonExcBPAId") as HiddenField).Value);
        //            TextBox _txtNonExcPassCode = ((rptItem as RepeaterItem).FindControl("txtNonExcPassCode") as TextBox);
        //            if (_txtNonExcPassCode.Visible)
        //            {
        //                if (_txtNonExcPassCode.Text.IsNullOrEmpty())
        //                {
        //                    IsEnteredBkgPackagePassCodeMatched = false;
        //                    break;
        //                }
        //                else if (_lstAll.IsNotNull() && _lstAll.Count >0 && _lstAll.Any(x => x.BPAId == _packageId && x.PassCode.ToLower() != _txtNonExcPassCode.Text.ToLower()))
        //                {
        //                    IsEnteredBkgPackagePassCodeMatched = false;
        //                    break;
        //                }
        //            }                    

        //        }
        //    }

        //    foreach (var rptItem in rptExclusive.Items)
        //    {
        //        RadioButton _rBtnExclusive = ((rptItem as RepeaterItem).FindControl("rbtnExclusive") as RadioButton);
        //        if (_rBtnExclusive.IsNotNull() && _rBtnExclusive.Checked)
        //        {
        //            Int32 _packageId = Convert.ToInt32(((rptItem as RepeaterItem).FindControl("hdnExcBPAId") as HiddenField).Value);

        //            TextBox _txtExcPassCodee = ((rptItem as RepeaterItem).FindControl("txtExcPassCode") as TextBox);

        //            if (_txtExcPassCodee.Visible)
        //            {
        //                if (_txtExcPassCodee.Text.IsNullOrEmpty())
        //                {
        //                    IsEnteredBkgPackagePassCodeMatched = false;
        //                    break;
        //                }
        //                else if (_lstAll.IsNotNull() && _lstAll.Count > 0 && _lstAll.Any(x => x.BPAId == _packageId && x.PassCode.ToLower() != _txtExcPassCodee.Text.ToLower()))
        //                {
        //                    IsEnteredBkgPackagePassCodeMatched = false;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    return IsEnteredBkgPackagePassCodeMatched;

        //}
        protected void btnExistingOrderDoPostBack_Click(object sender, EventArgs e)
        {



        }

        private void CaptureQueryString()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("TenantId"))
                {
                    CurrentViewContext.TenantId = Convert.ToInt32(args["TenantId"]);
                }
                if (args.ContainsKey("ReferenceOrderID"))
                {
                    CurrentViewContext.FingerPrintData.ArchivedOrderID = Convert.ToInt32(args["ReferenceOrderID"]);
                }
                if (args.ContainsKey("IsFromArchivedOrderScreen"))
                {
                    CurrentViewContext.FingerPrintData.IsFromArchivedOrderScreen = Convert.ToBoolean(args["IsFromArchivedOrderScreen"]);
                }
            }
        }
    }
}


