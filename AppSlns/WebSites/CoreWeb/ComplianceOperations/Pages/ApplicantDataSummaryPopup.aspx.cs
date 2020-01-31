using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ApplicantDataSummaryPopup : BaseWebPage, IApplicantDataSummaryPopupView
    {
        #region Private Variables

        private ApplicantDataSummaryPopupPresenter _presenter = new ApplicantDataSummaryPopupPresenter();

        #endregion

        #region Properties
        public ApplicantDataSummaryPopupPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public IApplicantDataSummaryPopupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }


        Int32 IApplicantDataSummaryPopupView.CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        DateTime? IApplicantDataSummaryPopupView.UserLastLoginTime
        {
            get;
            set;
        }

        List<ApplicantDataSummaryContract> IApplicantDataSummaryPopupView.lstApplicantSummary { get; set; }

        List<ApplicantBackgroundSummaryContract> IApplicantDataSummaryPopupView.lstApplicantBackgroundSummary { get; set; }

        List<RequirementSharesDataContract> IApplicantDataSummaryPopupView.lstApprovedRotations { get; set; }

        List<UpcomingCategoryExpirationContract> IApplicantDataSummaryPopupView.lstUpcomingCategoryExpiration { get; set; }

        Int32 IApplicantDataSummaryPopupView.ApplicantTenantId
        {
            get
            {
                if (!ViewState["ApplicantTenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["ApplicantTenantId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ApplicantTenantId"] = value;
                hdnApplicantTenantId.Value = Convert.ToString(value);
            }
        }

        //public Int32 ClientSettingBeforeExpiry
        //{
        //    get
        //    {
        //        if (ViewState["BeforeExpirySettingValue"].IsNullOrEmpty())
        //        {
        //            Presenter.GetClientSettingDetails();
        //        }
        //        return Convert.ToInt32(ViewState["BeforeExpirySettingValue"]);
        //    }
        //    set
        //    {
        //        ViewState["BeforeExpirySettingValue"] = value;
        //    }
        //}

        public List<SubscriptionFrequency> lstSubscriptionFrequencies
        {
            get;set;
        }
        //UAT-2003: Add ability to extend/renew when clicking "place order"
        public List<Entity.ClientEntity.vwSubscription> ListSubscription { get; set; }

        #endregion

        #region EVENTS

        #region PAGE EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CaptureQueryStringData();
                    Presenter.OnViewInitialized();
                    Presenter.GetSubscriptionFrequencies();
                    BindDataSummary();
                    BindRenewSubscriptionSection();
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

        #endregion

        #region Methods

        #region Private Methods

        private void BindDataSummary()
        {
            try
            {
                Presenter.GetApplicantSummaryData();
                Presenter.GetApplicantApprovedRotations();
                GenerateUpcomingExpirationHtml();

                ApplicantDataSummaryContract applDataSummary = CurrentViewContext.lstApplicantSummary.FirstOrDefault();
                if (!applDataSummary.IsNullOrEmpty())
                {
                    lblMessage.Text = "Since your last login " + applDataSummary.ApprovedItemCount + " item(s) have been marked " + @"""meets requirements"", " + applDataSummary.RejectedItemCount + " item(s) have been marked " + @"""does not meet requirements"", and " + applDataSummary.PendingReviewItemCount + " are still pending review.";
                    GenerateCategoryHtml();
                }
                else
                {
                    dvCategoriesMain.Visible = false;
                    dvCategoriesParentDiv.Visible = false;
                }
                if (!CurrentViewContext.lstApplicantBackgroundSummary.IsNullOrEmpty())
                {
                    dvSvcGroupMain.Visible = true;
                    grdSvcGroup.DataSource = CurrentViewContext.lstApplicantBackgroundSummary;
                    grdSvcGroup.DataBind();
                    grdSvcGroup.MasterTableView.PageSize = CurrentViewContext.lstApplicantBackgroundSummary.Count();
                    Int32 recentlyCompletedOrderID = CurrentViewContext.lstApplicantBackgroundSummary.FirstOrDefault().RecentlyCompletedOrderID;
                    if (recentlyCompletedOrderID > AppConsts.NONE)
                    {
                        dvBkgOrderReport.Visible = true;
                        lbtnOrderCompletionReport.Text = String.Format(lbtnOrderCompletionReport.Text, recentlyCompletedOrderID);
                        hdnfOrderID.Value = recentlyCompletedOrderID.ToString();
                    }
                }
                if (CurrentViewContext.lstApplicantSummary.Where(x => x.IncompleteCategoryName != String.Empty).Count() == AppConsts.NONE
                    && CurrentViewContext.lstApplicantBackgroundSummary.IsNullOrEmpty() && CurrentViewContext.lstUpcomingCategoryExpiration.IsNullOrEmpty()
                    )
                {
                    dvMessage.Style["padding-top"] = "40px";
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "SetPopupHeight();", true);
                }

                if (!CurrentViewContext.lstApprovedRotations.IsNullOrEmpty() && CurrentViewContext.lstApprovedRotations.Count > 0)
                {
                    dvApprovedRotations.Visible = true;
                    grdApprovedRotations.DataSource = CurrentViewContext.lstApprovedRotations;
                    grdApprovedRotations.DataBind();
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

        private void CaptureQueryStringData()
        {
            if (!Request["ApplicantTenantId"].IsNullOrEmpty())
            {
                CurrentViewContext.ApplicantTenantId = Convert.ToInt32(Request["ApplicantTenantId"]);
            }

        }

        private void GenerateCategoryHtml()
        {
            if (CurrentViewContext.lstApplicantSummary.Where(x => x.IncompleteCategoryName != String.Empty).Count() == AppConsts.NONE)
            {
                dvCategoriesMain.Visible = false;
            }
            else
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append(@"<table>");
                CurrentViewContext.lstApplicantSummary.Where(x => x.IncompleteCategoryName != String.Empty).ToList().ForEach(gen =>
                {
                    strBuilder.AppendFormat("<tr><td class='td-one'>{0}</td></tr>", "<li>" + gen.IncompleteCategoryName + "</li>");
                });
                strBuilder.AppendFormat("</table>");
                divCategoryContainer.InnerHtml = strBuilder.ToString();
            }
        }

        //UAT-2924: Add upcoming expirations to Since You Been Gone popup as part of the not compliant categories
        private void GenerateUpcomingExpirationHtml()
        {
            Presenter.GetUpcomingExpirationcategoryByLoginId();
            if (CurrentViewContext.lstUpcomingCategoryExpiration.ToList().Count > 0)
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append(@"<table border='1'>");
                strBuilder.AppendFormat("<tr><th style='padding: 5px; border: 1px solid black;font-weight:bold;'>Category Name</th> <th style='padding: 5px; border: 1px solid black;font-weight:bold;'>Expiration Date</th><th style='padding: 5px; border: 1px solid black;font-weight:bold;'>Institution Hierarchy</th></tr>");
                CurrentViewContext.lstUpcomingCategoryExpiration.ForEach(gen =>
                {
                    strBuilder.AppendFormat("<tr><td class='td-one' style='padding: 5px; border: 1px solid black;'>{0}</td>", gen.Category);
                    strBuilder.AppendFormat("<td class='td-one' style='padding: 5px; border: 1px solid black;'>{0}</td>", Convert.ToDateTime(gen.CategoryComplianceExpiryDate).ToShortDateString());
                    strBuilder.AppendFormat("<td class='td-one' style='padding: 5px; border: 1px solid black;'>{0}</td></tr>", gen.InstitutionHierarchyLabel);
                });
                strBuilder.AppendFormat("</table>");
                divUpcomingExp.InnerHtml = strBuilder.ToString();
            }
            else
            {
                divUpcomingCatExp.Visible = false;
            }
        }

        private void BindRenewSubscriptionSection()
        {
            Boolean isAnyValidRenewSubscription = false;
            Presenter.GetSubscriptionList();
            List<Entity.ClientEntity.vwSubscription> packageSubscriptionList = new List<Entity.ClientEntity.vwSubscription>();
            packageSubscriptionList.AddRange(CurrentViewContext.ListSubscription);
            foreach (var packageSubscription in packageSubscriptionList)
            {
                Int32 remaingDays = 0;
                Int32? archivalGracePeriod = packageSubscription.ArchivalGracePeriod;
                Boolean isArchivedSubscription = false;
                Boolean isExpiredSubscription = false;
                SetSubscriptionExpiredAndArchiveStatus(packageSubscription.ExpiryDate.Value, packageSubscription.ArchiveStateCode, out isArchivedSubscription, out isExpiredSubscription);
                Boolean isOrderApproved = packageSubscription.IsOrderApproved ?? false;

                if (!packageSubscription.ExpiryDate.IsNullOrEmpty())
                {
                    DateTime ExpiryDate = packageSubscription.ExpiryDate.Value;
                    if (ExpiryDate.Date > DateTime.Now.Date)
                    {
                        remaingDays = (ExpiryDate.Date - DateTime.Now.Date).Days;
                    }

                    else if (archivalGracePeriod.IsNotNull())
                    {
                        remaingDays = (ExpiryDate.AddDays(archivalGracePeriod.Value).Date - DateTime.Now.Date).Days;
                    }
                }

                if (isOrderApproved == true && Presenter.IsCustomPriceSetAndSubsRenewalValid(packageSubscription.OrderID))
                {
                    if (((remaingDays <= CurrentViewContext.lstSubscriptionFrequencies.FirstOrDefault(a=>a.CompliancePackageID==packageSubscription.CompliancePackageID).DPM_SubscriptionBeforeExpFrequency && !isExpiredSubscription)
                        || ((archivalGracePeriod.IsNotNull() && remaingDays >= 0 && isExpiredSubscription) || (archivalGracePeriod.IsNull() && isExpiredSubscription)))
                        && !isArchivedSubscription)
                    {
                        isAnyValidRenewSubscription = true;
                    }
                }
                else
                {
                    isAnyValidRenewSubscription = false;
                }
                if (Presenter.IsPackageChangeSubscription(packageSubscription.OrderID)
                            || packageSubscription.SubscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.MobilitySwitched
                            || packageSubscription.SubscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.DataMovementDue)
                {
                    isAnyValidRenewSubscription = false;
                }
                DateTime PackageExpiryDate = packageSubscription.ExpiryDate.Value;
                if (DateTime.Now.Date > PackageExpiryDate.Date)
                {
                    isAnyValidRenewSubscription = false;
                }
                if (isAnyValidRenewSubscription)
                    break;
            }
            if (isAnyValidRenewSubscription)
            {
                //lnkPlaceOrder.Attributes.Add("href", "#");
                //lnkPlaceOrder.Attributes.Add("onMouseOver", "DisplayPlaceOrder('block')");
                //lnkPlaceOrder.Attributes.Add("onMouseOut", "DisplayPlaceOrder('none')");
                //lnkPlaceNewOrder.OnClientClick = "HandleDashboardCommandNavigation(" + "'" + plsOrderUrl + "');return false;";
                //lnkRenewOrder.OnClientClick = "OpenRenewOrderPopup();return false;";

                dvTrackingRenewSubscription.Visible = true;
            }
            else
            {
                dvTrackingRenewSubscription.Visible = false;
                //lnkPlaceOrder.OnClientClick = "HandleDashboardCommandNavigation(" + "'" + plsOrderUrl + "');return false;";
            }
        }
        /// <summary>
        /// Method to Set the Archive and Expired subscription properties
        /// </summary>
        /// <param name="expirydate">expiry date of subscription</param>
        /// <param name="ArchiveStatusCode">Archive Code of Subscription</param>
        private void SetSubscriptionExpiredAndArchiveStatus(DateTime expirydate, String archiveStatusCode, out Boolean isArchivedSubscription, out Boolean isExpiredSubscription)
        {
            isArchivedSubscription = archiveStatusCode.IsNullOrEmpty() ? false : archiveStatusCode.Equals(ArchiveState.Archived.GetStringValue()) ? true : false;
            isExpiredSubscription = expirydate.IsNullOrEmpty() ? false : expirydate.Date < DateTime.Now.Date ? true : false;
        }


        #endregion

        #endregion

        protected void lnkTrackingRenewSubscription_Click(object sender, EventArgs e)
        {
            String url = String.Format("~/ComplianceOperations/Pages/RenewOrderPopup.aspx?IsFromRenewOrderPopup=true");
            Response.Redirect(url, true);
        }
    }
}