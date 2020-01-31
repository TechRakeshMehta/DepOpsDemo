using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Collections.Generic;
using System.Linq;
using INTSOF.UI.Contract.BkgOperations;
using INTERSOFT.WEB.UI.WebControls;
using Business.RepoManagers;
using System.Globalization;
using INTSOF.UI.Contract.Globalization;

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class AdminEntryPackageDetails : BaseUserControl, IAdminEntryPackageDetailsView
    {
        private AdminEntryPackageDetailsPresenter _presenter = new AdminEntryPackageDetailsPresenter();

        public Dictionary<string, Int32> DPPSIds { get; set; }

        public Dictionary<string, DeptProgramPackageSubscription> SelectedPackageDetails
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                var ApplicantOrder = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (!ApplicantOrder.IsNullOrEmpty())
                {
                    ViewState["TenantId"] = ApplicantOrder.TenantId;
                    return ApplicantOrder.TenantId;
                }
                return 0;


            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        public IAdminEntryPackageDetailsView CurrentViewContext
        {
            get { return this; }
        }
        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public String DetailViewType
        {
            get;
            set;
        }

        public List<BackgroundPackagesContract> lstExternalPackages
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Bkg package prices from the OrderReview screen
        /// </summary>
        public List<Package_PricingData> lstPackagePrices
        {
            get;
            set;
        }
        /// <summary>
        /// UAT-1867:Add breakdown of fees in total price on the order review screen (before submit).
        /// Gets the Backlground service line from the OrderReview Screen
        /// </summary>
        public List<BackroundOrderServiceLinePrice> lstBkgOrderSvcLineData
        {
            get;
            set;
        }
        public Boolean IfInvoiceOnlyPymntOptn
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

        public Boolean IsLocationServiceTenant
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IsLocationServiceTenant"])))
                    return (Boolean)ViewState["IsLocationServiceTenant"];
                return false;
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
            }
        }

        #region Admin Entry Portal
        public Boolean IsAdminEntryTenant
        {
            get
            {
                if (!ViewState["IsAdminEntryTenant"].IsNullOrEmpty())
                    return (Boolean)ViewState["IsAdminEntryTenant"];
                return false;
            }
            set
            {
                ViewState["IsAdminEntryTenant"] = value;
            }
        }
        #endregion
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
        #region UAT-3601
        String IAdminEntryPackageDetailsView.PackageDetailHeaderLabel
        {
            set
            {
                headerPkgDetails.InnerText = value;
            }
        }
        //String IAdminEntryPackageDetailsView.ChangePackageSelectionButtonLabel
        //{
        //    set
        //    {
        //        cmdbarEditPackage.ExtraButtonText = value;
        //    }
        //}
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            if (!IsPostBack)
            {

                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    divBkgSvcBreakdwnFees.Visible = false;
                    //headerPkgDetails.InnerText = "Order Selection Details";
                    //cmdbarEditPackage.ExtraButtonText = "Change Order Selection";
                }
                else
                {
                    //headerPkgDetails.InnerText = "Package Details";
                    //cmdbarEditPackage.ExtraButtonText = "Change Package Selection";
                }
                BindPackageDetails();
            }
            //#region UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
            //HideControlsForCompleteOrderMode();
            // #endregion
        }

        private void BindPackageDetails()
        {
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            IfInvoiceOnlyPymntOptn = applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel;
            Decimal complioPackageTotalPrice = 0;
            Decimal CompliancePackagesTotalPrice = 0;
            Decimal settlementAmount = 0; //UAT-4086
            Boolean isCompliancePackageOfInvoiceType = false;
            Boolean paymentOptionsExistForCompliancePackage = true;
            Boolean paymentOptionsExistForBkgPackages = true;
            //Show Compliance Package Details
            //CurrentViewContext.DPPSIds.IsNotNull() && CurrentViewContext.DPPSId > AppConsts.NONE
            BindCompliancePackages();//UAT-3283

            if (CurrentViewContext.DPPSIds.IsNotNull() && CurrentViewContext.DPPSIds.Count > 0)
            {
                foreach (string cptype in DPPSIds.Keys)
                {
                    //UAT-3283
                    String cpkgType = cptype;
                    if (applicantOrderCart.HasMultipleBundles)
                    {
                        int index = cptype.IndexOf("_");
                        if (index > AppConsts.NONE)
                        {
                            cpkgType = cptype.Substring(0, index);
                        }
                    }
                    //END UAT-3283
                    string controlSuffix = (cpkgType.Equals(CompliancePackageTypes.IMMUNIZATION_COMPLIANCE_PACKAGE.GetStringValue()) || string.IsNullOrEmpty(cpkgType) ? "" : "_" + cpkgType);
                    if (DPPSIds[cptype] > AppConsts.NONE)
                    {
                        //UAT-3283
                        foreach (RepeaterItem compPkgs in rptCompliancePkgs.Items)
                        {
                            var _packageId = Convert.ToInt32((compPkgs.FindControl("hdfPackageId") as HiddenField).Value);
                            var _dppId = Convert.ToInt32((compPkgs.FindControl("hdfDPPId") as HiddenField).Value);

                            if (CurrentViewContext.SelectedPackageDetails[cptype].DeptProgramPackage.CompliancePackage.CompliancePackageID == _packageId)//UAT-3283
                            {
                                HtmlGenericControl ctrlDivCompliancePackage = compPkgs.FindControl("divCompliancePackage" + controlSuffix) as HtmlGenericControl;// (HtmlGenericControl)FindControl("divCompliancePackage" + controlSuffix);
                                if (ctrlDivCompliancePackage.IsNotNull())
                                    ctrlDivCompliancePackage.Visible = true;

                                Label ctrlLblPackage = compPkgs.FindControl("lblPackage" + controlSuffix) as Label; //(Label)FindControl("lblPackage" + controlSuffix);
                                if (ctrlLblPackage.IsNotNull())
                                    ctrlLblPackage.Text = String.IsNullOrEmpty(CurrentViewContext.SelectedPackageDetails[cptype].DeptProgramPackage.CompliancePackage.PackageLabel)
                                        ? CurrentViewContext.SelectedPackageDetails[cptype].DeptProgramPackage.CompliancePackage.PackageName
                                        : CurrentViewContext.SelectedPackageDetails[cptype].DeptProgramPackage.CompliancePackage.PackageLabel;

                                WclNumericTextBox ctrTxtPrice = compPkgs.FindControl("txtPrice" + controlSuffix) as WclNumericTextBox; //(WclNumericTextBox)FindControl("txtPrice" + controlSuffix);
                                complioPackageTotalPrice = 0;
                                //UAT-1648
                                if (String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE)
                                {
                                    complioPackageTotalPrice = Convert.ToDecimal(applicantOrderCart.Amount);
                                    if (ctrTxtPrice.IsNotNull())
                                        ctrTxtPrice.Text = Convert.ToString(complioPackageTotalPrice);
                                    //UAT-4086
                                    settlementAmount = !applicantOrderCart.SettleAmount.IsNullOrEmpty() && applicantOrderCart.SettleAmount > AppConsts.NONE ? applicantOrderCart.SettleAmount : AppConsts.NONE;
                                }
                                else if (CurrentViewContext.SelectedPackageDetails[cptype].DPPS_TotalPrice != null)
                                {
                                    complioPackageTotalPrice = CurrentViewContext.SelectedPackageDetails[cptype].DPPS_TotalPrice.Value;
                                    if (ctrTxtPrice.IsNotNull())
                                        ctrTxtPrice.Text = Convert.ToString(complioPackageTotalPrice);
                                }
                                int sub = 0;
                                if (CurrentViewContext.SelectedPackageDetails[cptype].SubscriptionOption.Year != null)
                                {
                                    sub = (CurrentViewContext.SelectedPackageDetails[cptype].SubscriptionOption.Year ?? 0) * 12;
                                }
                                if (CurrentViewContext.SelectedPackageDetails[cptype].SubscriptionOption.Month != null)
                                {
                                    sub += (CurrentViewContext.SelectedPackageDetails[cptype].SubscriptionOption.Month ?? 0);
                                }
                                Label ctrlLblSubscription = compPkgs.FindControl("lblSubscription" + controlSuffix) as Label; //(Label)FindControl("lblSubscription" + controlSuffix);
                                if (ctrlLblSubscription.IsNotNull())
                                    ctrlLblSubscription.Text = sub.ToString() + " Month(s)";
                                if (applicantOrderCart.OrderRequestType == OrderRequestType.RenewalOrder.GetStringValue())
                                {
                                    if (ctrlLblSubscription.IsNotNull())
                                        ctrlLblSubscription.Text = applicantOrderCart.RenewalDuration.ToString() + " Month(s)";
                                    complioPackageTotalPrice = (applicantOrderCart.CompliancePackagesGrandTotal ?? 0);
                                    if (ctrTxtPrice.IsNotNull())
                                        ctrTxtPrice.Text = Convert.ToString(complioPackageTotalPrice);
                                }
                                //CompliancePackagesTotalPrice += complioPackageTotalPrice; //Commented in UAT-4086
                                //UAT -4086
                                CompliancePackagesTotalPrice = CompliancePackagesTotalPrice + complioPackageTotalPrice - settlementAmount;
                                List<DeptProgramPackagePaymentOption> lstDeptProgramPackagePaymentOptions = !CurrentViewContext.SelectedPackageDetails[cptype].DeptProgramPackage.IsNullOrEmpty() ?
                                                                       CurrentViewContext.SelectedPackageDetails[cptype].DeptProgramPackage.DeptProgramPackagePaymentOptions.Where(cond => !cond.DPPPO_IsDeleted && !cond.DeptProgramPackage.DPP_IsDeleted).ToList()
                                                                       : new List<DeptProgramPackagePaymentOption>();


                                List<lkpPaymentOption> lstPaymentOptions = !lstDeptProgramPackagePaymentOptions.IsNullOrEmpty() ?
                                                                           lstDeptProgramPackagePaymentOptions.Select(col => col.lkpPaymentOption).ToList()
                                                                           : new List<lkpPaymentOption>();

                                HtmlGenericControl ctrlDvCmplncPkgPrice = compPkgs.FindControl("dvCmplncPkgPrice" + controlSuffix) as HtmlGenericControl; //(HtmlGenericControl)FindControl("dvCmplncPkgPrice" + controlSuffix);

                                if (lstPaymentOptions.IsNotNull() && lstPaymentOptions.Count > 0)
                                {
                                    if (lstPaymentOptions.Count == 1)
                                    {
                                        if (lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                                        {
                                            if (ctrlDvCmplncPkgPrice.IsNotNull())
                                                ctrlDvCmplncPkgPrice.Style.Add("display", "none");
                                            isCompliancePackageOfInvoiceType = true;
                                        }
                                    }
                                    else if (lstPaymentOptions.Count == 2)
                                    {
                                        if (lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                                        {

                                            if (ctrlDvCmplncPkgPrice.IsNotNull())
                                                ctrlDvCmplncPkgPrice.Style.Add("display", "none");
                                            isCompliancePackageOfInvoiceType = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (IfInvoiceOnlyPymntOptn)
                                    {
                                        if (ctrlDvCmplncPkgPrice.IsNotNull())
                                            ctrlDvCmplncPkgPrice.Style.Add("display", "none");
                                    }
                                    paymentOptionsExistForCompliancePackage = false;
                                }
                            }
                        }
                    }
                }

            }

            //Show Background Package Data
            List<Int32> bkgHierarchyMappingIds = new List<int>();
            foreach (var applicantOrder in applicantOrderCart.lstApplicantOrder)
            {
                if (applicantOrder.lstPackages.IsNotNull() && applicantOrder.lstPackages.Count > AppConsts.NONE)
                {
                    bkgHierarchyMappingIds = applicantOrder.lstPackages.Select(condition => condition.BPHMId).ToList();
                }
                break;
            }
            Decimal bkgPackagesTotalPrice = 0;

            if (bkgHierarchyMappingIds.IsNotNull() && bkgHierarchyMappingIds.Count > AppConsts.NONE)
            {
                divBackgroundPackage.Style.Add("display", "block");

                //UAT-1648
                if (String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) != AppConsts.NONE)
                {
                    Presenter.GetOrderBkgPackageDetails(bkgHierarchyMappingIds);
                }

                if (!this.lstPackagePrices.IsNullOrEmpty())
                {
                    foreach (var pkg in CurrentViewContext.lstExternalPackages)
                    {
                        Package_PricingData _pkgPricingData = lstPackagePrices.Where(pkgPrice => pkgPrice.PackageId == pkg.BPAId).FirstOrDefault();
                        if (_pkgPricingData.IsNotNull())
                            //pkg.BasePrice = _pkgPricingData.TotalBkgPackagePrice.IsNullOrEmpty() ? AppConsts.NONE : _pkgPricingData.TotalBkgPackagePrice;
                            pkg.TotalBkgPackagePrice = _pkgPricingData.TotalBkgPackagePrice.IsNullOrEmpty() ? AppConsts.NONE : _pkgPricingData.TotalBkgPackagePrice;
                        //if (pkg.IsReqToQualifyInRotation && !pkg.AdditionalPrice.IsNullOrEmpty() && pkg.AdditionalPrice > AppConsts.NONE)
                        //    pkg.TotalBkgPackagePrice += Convert.ToDecimal(pkg.AdditionalPrice);
                        pkg.TotalLineItemPrice = _pkgPricingData.TotalBkgPackagePrice - pkg.BasePrice;
                        bkgPackagesTotalPrice += pkg.TotalBkgPackagePrice;
                    }
                }

                rptBackgroundPackages.DataSource = CurrentViewContext.lstExternalPackages;
                rptBackgroundPackages.DataBind();

                //UAT-1867:Add breakdown of fees in total price on the order review screen (before submit).
                //binding background line item price breakdown grid with the data coming from OrderReview screen
                if (!lstBkgOrderSvcLineData.IsNullOrEmpty())
                {
                    //divBkgSvcBreakdwnFees.Attributes.Add("style", "display:block");
                    grdOrderServiceLinePriceInfo.DataSource = lstBkgOrderSvcLineData;
                    grdOrderServiceLinePriceInfo.DataBind();
                }
            }

            //lblDepartment.Text = CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.DeptProgramMapping.Organization.OrganizationName; ;
            // lblProgram.Text = CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.DeptProgramMapping.AdminProgramStudy.ProgramStudy;

            //if (!CurrentViewContext.SelectedPackageDetails.IsNullOrEmpty()) // This implies that No Compliance Package was selected
            //    lblInstitutionHierarchy.Text = CurrentViewContext.SelectedPackageDetails.DeptProgramPackage.DeptProgramMapping.DPM_Label;
            //else
            //{
            // If no compliance package is selected, then load the hierarchy from the database
            //  if (applicantOrderCart.SelectedHierarchyNodeID != AppConsts.NONE)
            // UAT 1067 - Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on. 
            lblInstitutionHierarchy.Text = _presenter.GetInstituteHierarchyLabel(Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID));
            //}
            txtTotalPrice.Text = Convert.ToString((CompliancePackagesTotalPrice + bkgPackagesTotalPrice), CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));

            //UAT-3850
            if (!applicantOrderCart.FingerPrintData.IsNullOrEmpty() && !applicantOrderCart.FingerPrintData.IsNullOrEmpty()
                && !applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty() && applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE)
            {
                dvPaymentByInst.Style.Add("display", "block");
                dvBalanceAmount.Style.Add("display", "block");
                var _totalPrice = CompliancePackagesTotalPrice + bkgPackagesTotalPrice;
                if (_totalPrice < applicantOrderCart.FingerPrintData.BillingCodeAmount)
                {
                    txtPaymentByInst.Text = Convert.ToString(_totalPrice, CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                    txtBalanceAmount.Text = Convert.ToString(AppConsts.NONE, CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                }
                else
                {
                    txtPaymentByInst.Text = Convert.ToString(applicantOrderCart.FingerPrintData.BillingCodeAmount, CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                    txtBalanceAmount.Text = Convert.ToString((_totalPrice - applicantOrderCart.FingerPrintData.BillingCodeAmount), CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                }
            }

            if (CurrentViewContext.lstExternalPackages.IsNotNull() && CurrentViewContext.lstExternalPackages.Any(cond => cond.IsInvoiceOnlyAtPackageLevel == null))
            {
                paymentOptionsExistForBkgPackages = false;
            }


            if ((CurrentViewContext.lstExternalPackages.IsNotNull() && CurrentViewContext.lstExternalPackages.Any(cond => cond.IsInvoiceOnlyAtPackageLevel == true)) || isCompliancePackageOfInvoiceType)
            {
                dvTotalPrice.Style.Add("display", "none");
                //UAT-1867
                divBkgSvcBreakdwnFees.Attributes.Add("style", "display:none");
            }
            else if ((!paymentOptionsExistForBkgPackages || !paymentOptionsExistForCompliancePackage) && IfInvoiceOnlyPymntOptn)
            {
                dvTotalPrice.Style.Add("display", "none");
                //UAT-1867
                divBkgSvcBreakdwnFees.Attributes.Add("style", "display:none");
            }

            //else if (CurrentViewContext.lstExternalPackages.IsNull() && IfInvoiceOnlyPymntOptn
            //    CurrentViewContext.lstExternalPackages.IsNotNull() && !CurrentViewContext.lstExternalPackages.Any(cond => !cond.IsInvoiceOnlyAtPackageLevel == null)     IfInvoiceOnlyPymntOptn)
            //{
            //    dvTotalPrice.Style.Add("display", "none");
            //}
        }

        protected void btnEditPackage_Click(object sender, EventArgs e)
        {
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.NONE;
            applicantOrderCart.AddOrderStageTrackID(OrderStages.PendingOrder);
            Dictionary<String, String> queryString;
            if (applicantOrderCart.OrderRequestType == OrderRequestType.RenewalOrder.GetStringValue())
            {

                queryString = new Dictionary<String, String>()
                                                         {
                                                            {"OrderId",applicantOrderCart.PrevOrderId.ToString()},
                                                            { "Child",  ChildControls.RenewalOrder}
                                                         };
                //Response.Redirect(String.Format("~/Main/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

            }
            else
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",ChildControls.ApplicantPendingOrder}
                                                                 };
                String url = String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                Response.Redirect(url);
            }
        }


        public AdminEntryPackageDetailsPresenter Presenter
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

        protected void rptBackgroundPackages_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            try
            {
                Boolean? isInvoiceOnlyAtPackageLevel = (e.Item.DataItem as BackgroundPackagesContract).IsInvoiceOnlyAtPackageLevel;
                System.Web.UI.HtmlControls.HtmlGenericControl dvElement = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvElement");
                if ((isInvoiceOnlyAtPackageLevel.IsNotNull() && isInvoiceOnlyAtPackageLevel.Value) || CurrentViewContext.IsLocationServiceTenant)
                {
                    dvElement.Style.Add("display", "none");
                }
                else if (isInvoiceOnlyAtPackageLevel.IsNull() && IfInvoiceOnlyPymntOptn)
                {
                    dvElement.Style.Add("display", "none");
                }
                var lblOrderSelection = e.Item.FindControl("lblOrderSelection") as Label;
                if (IsLocationServiceTenant)
                {

                    //if(lblOrderSelection != null)
                    //{
                    // lblOrderSelection.Text = "Order Selection";
                    lblOrderSelection.Text = Resources.Language.ORDERSELECTION;
                    //}
                }
                else
                {
                    lblOrderSelection.Text = Resources.Language.BKGPKG;
                }

                //UAT-3268
                //Boolean isReqToQualifyInRotation = (e.Item.DataItem as BackgroundPackagesContract).IsReqToQualifyInRotation;
                //Decimal? additionalPrice = (e.Item.DataItem as BackgroundPackagesContract).AdditionalPrice;
                //System.Web.UI.HtmlControls.HtmlGenericControl dvAdditionalPrice = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvAdditionalPrice");
                //if (isReqToQualifyInRotation && !additionalPrice.IsNullOrEmpty() && additionalPrice > AppConsts.NONE)
                //{
                //    dvAdditionalPrice.Visible = true;
                //}
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

        #region UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
        //private void HideControlsForCompleteOrderMode()
        //{
        //    ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
        //    if (!applicantOrderCart.IsNullOrEmpty() && String.Compare(applicantOrderCart.OrderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE
        //        && applicantOrderCart.IsReadOnly)
        //    {
        //        cmdbarEditPackage.Visible = false;
        //    }
        //    //UAT-1834: NYU Migration 2 of 3: Applicant Complete Order Process
        //    if (!applicantOrderCart.IsNullOrEmpty() && applicantOrderCart.IsBulkOrder)
        //        cmdbarEditPackage.Visible = false;
        //    if (CurrentViewContext.IsAdminEntryTenant)
        //        cmdbarEditPackage.Visible = false;
        //}
        #endregion

        #region UAT-3283

        private void BindCompliancePackages()
        {
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            if (!applicantOrderCart.CompliancePackages.IsNullOrEmpty() && applicantOrderCart.CompliancePackages.Count > AppConsts.NONE)
            {
                rptCompliancePkgs.DataSource = applicantOrderCart.CompliancePackages.Values.ToList();
                rptCompliancePkgs.DataBind();
            }
        }

        #endregion
    }
}

