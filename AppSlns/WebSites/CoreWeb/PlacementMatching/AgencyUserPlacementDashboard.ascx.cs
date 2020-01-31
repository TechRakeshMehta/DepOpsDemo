using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Business.RepoManagers;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class AgencyUserPlacementDashboard : BaseUserControl, IAgencyUserPlacementDashboardView
    {
        #region Variables

        #region Private Variables
        private AgencyUserPlacementDashboardPresenter _presenter = new AgencyUserPlacementDashboardPresenter();
        private String _defaultStatusCode = "AAAA"; // Status code for pending review requests.
        #endregion

        #region Public Variables

        #endregion

        #endregion


        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public AgencyUserPlacementDashboardPresenter Presenter
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

        public IAgencyUserPlacementDashboardView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Guid IAgencyUserPlacementDashboardView.UserId
        {
            get
            {
                return base.SysXMembershipUser.UserId;
            }
        }

        String IAgencyUserPlacementDashboardView.SelectedStatusCode
        {
            get
            {
                if (!ViewState["SelectedStatusCode"].IsNullOrEmpty())
                    return ViewState["SelectedStatusCode"].ToString();
                return String.Empty;
            }
            set
            {
                ViewState["SelectedStatusCode"] = value;
            }
        }

        List<RequestDetailContract> IAgencyUserPlacementDashboardView.lstRequests
        {
            get
            {
                if (!ViewState["lstRequests"].IsNullOrEmpty())
                    return (List<RequestDetailContract>)ViewState["lstRequests"];
                return new List<RequestDetailContract>();
            }
            set
            {
                ViewState["lstRequests"] = value;
            }
        }

        List<RequestDetailContract> IAgencyUserPlacementDashboardView.lstAllRequests
        {
            get
            {
                if (!ViewState["lstAllRequests"].IsNullOrEmpty())
                    return (List<RequestDetailContract>)ViewState["lstAllRequests"];
                return new List<RequestDetailContract>();
            }
            set
            {
                ViewState["lstAllRequests"] = value;
            }
        }

        RequestDetailContract IAgencyUserPlacementDashboardView.SearchRequestContract
        {
            get
            {
                if (!ViewState["SearchRequestContract"].IsNullOrEmpty())
                    return (RequestDetailContract)ViewState["SearchRequestContract"];
                return new RequestDetailContract();
            }
            set
            {
                ViewState["SearchRequestContract"] = value;
            }
        }

        List<RequestStatusContract> IAgencyUserPlacementDashboardView.lstRequestStatus
        {
            get
            {
                if (!ViewState["lstRequestStatus"].IsNullOrEmpty())
                {
                    return ViewState["lstRequestStatus"] as List<RequestStatusContract>;
                }
                return new List<RequestStatusContract>();
            }
            set
            {
                ViewState["lstRequestStatus"] = value;
            }
        }

        Int32 IAgencyUserPlacementDashboardView.SelectedStatusID
        {
            get
            {
                if (!ViewState["SelectedStatusID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedStatusID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedStatusID"] = value;
            }
        }


        Int32 IAgencyUserPlacementDashboardView.AgencyHierarchyRootNodeID
        {
            get
            {
                if (!ViewState["AgencyHierarchyRootNodeID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["AgencyHierarchyRootNodeID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["AgencyHierarchyRootNodeID"] = value;
            }
        }

        DateTime? IAgencyUserPlacementDashboardView.FromDate { get; set; }
        DateTime? IAgencyUserPlacementDashboardView.ToDate { get; set; }

        List<InstitutionRequestPieChartContract> IAgencyUserPlacementDashboardView.lstInstitutionRequestsApproved
        {
            get
            {
                if (!ViewState["lstInstitutionRequestsApproved"].IsNullOrEmpty())
                    return (List<InstitutionRequestPieChartContract>)(ViewState["lstInstitutionRequestsApproved"]);
                return new List<InstitutionRequestPieChartContract>();
            }
            set
            {
                ViewState["lstInstitutionRequestsApproved"] = value;
            }
        }
        #endregion

        #endregion


        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Placement Dashboard";
                base.SetPageTitle("Placement Dashboard");
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
                if (!this.IsPostBack)
                {
                    Presenter.GetRequestStatuses();
                    hdnSelectedStatusCode.Value = _defaultStatusCode;
                    BindPieChartFilters();
                }
                Presenter.GetAgencyRootNode();

                if (!hdnSelectedStatusCode.Value.IsNullOrEmpty())
                    CurrentViewContext.SelectedStatusCode = hdnSelectedStatusCode.Value;


                BindAnchorClicks();
                //BindPieChartData();
                grdPlacementRequest.Rebind();
                BindStatusRecordsCount();
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

        protected void grdPlacementRequest_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                // BindSearchContract();
                CurrentViewContext.SelectedStatusID = CurrentViewContext.lstRequestStatus.Where(cond => cond.Code == CurrentViewContext.SelectedStatusCode).FirstOrDefault().StatusID;
                Presenter.GetSelectedStatusTypeRequests();
                grdPlacementRequest.DataSource = CurrentViewContext.lstRequests;
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

        protected void grdPlacementRequest_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetails")
                {
                    //Redirect to Request details popup
                    String requestStatusCode = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StatusCode"]);
                    Int32 requestID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequestId"]);
                    Int32 opportunityID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OpportunityID"]);
                    hdnRequestId.Value = requestID.ToString();
                    hdnPageRequested.Value = RequestDetails.REQUESTDETAILS.GetStringValue();
                    hdnRequestStatusCode.Value = requestStatusCode;
                    hdnOpportunityID.Value = opportunityID.ToString();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RequestDetailsPop();", true);
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

        protected void grdPlacementRequest_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    RadButton btnDetails = ((RadButton)e.Item.FindControl("btnDetails"));
                    String requestStatusCode = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StatusCode"].IsNullOrEmpty() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StatusCode"].ToString();

                    if (!requestStatusCode.IsNullOrEmpty() && requestStatusCode == RequestStatusCodes.Approved.GetStringValue())
                    {
                        btnDetails.ForeColor = System.Drawing.Color.LightGreen;
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

        #region Calendar Events

        protected void clndrPlacement_DayRender(object sender, Telerik.Web.UI.Calendar.DayRenderEventArgs e)
        {
            try
            {
                DateTime calenderDate = e.Day.Date;

                foreach (var item in CurrentViewContext.lstRequests)
                {
                    DateTime a = Convert.ToDateTime(item.RequestSubmittedDate);
                    if (Convert.ToDateTime(item.RequestSubmittedDate).Date == calenderDate)
                    {
                        e.Cell.Text = e.Cell.Text.IsNullOrEmpty()?(e.Cell.Text + item.InstitutionName):(e.Cell.Text +", "+item.InstitutionName);
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

        #region Dropdown Events

        protected void ddlPieChartFilters_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ShowHideDatePickers();
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

        protected void btnGo_RangeFilter_Click(object sender, EventArgs e)
        {
            try
            {
                //need to implement the code here /// TODO
                BindPieChartData();
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

        protected void btnDoPostBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (!hdnRequestSavedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestSavedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request saved successfully.");
                        grdPlacementRequest.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnRequestCancelledSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestCancelledSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request cancelled successfully.");
                        grdPlacementRequest.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnRequestRejectedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestRejectedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request rejected successfully.");
                        grdPlacementRequest.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnRequestArchivedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestArchivedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request archived successfully.");
                        grdPlacementRequest.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnRequestApprovedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestApprovedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request approved successfully.");
                        grdPlacementRequest.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnRequestPublishedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestPublishedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request published successfully.");
                        grdPlacementRequest.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
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

        #endregion

        #region Methods

        #region Private Methods

        private void BindPieChartFilters()
        {
            List<Entity.SharedDataEntity.lkpDurationOption> lstDurationOptions = Presenter.GetDurationOptions();
            if (!lstDurationOptions.IsNullOrEmpty())
            {
                ddlPieChartFilters.DataTextField = "DO_Name";
                ddlPieChartFilters.DataValueField = "DO_Code";
                ddlPieChartFilters.DataSource = lstDurationOptions;
                ddlPieChartFilters.DataBind();

                ddlPieChartFilters.SelectedValue = LKPDurationOptions.LAST_YEAR.GetStringValue();
                ShowHideDatePickers();
            }
        }

        private void ShowHideDatePickers()
        {
            if (string.Compare(ddlPieChartFilters.SelectedValue, LKPDurationOptions.DATE_RANGE.GetStringValue()) == 0)
            {
                dvRangeFilters.Visible = true;
                dpFromDate.Clear();
                dpToDate.Clear();
            }
            else
            {
                dvRangeFilters.Visible = false;
                BindPieChartData();
            }
        }

        private void BindAnchorClicks()
        {
            ancPendingReview.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Requested.GetStringValue() + "')");
            ancModified.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Modified.GetStringValue() + "')");
            ancApproved.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Approved.GetStringValue() + "')");
            ancRejected.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Rejected.GetStringValue() + "')");
            ancArchived.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Archived.GetStringValue() + "')");
            ancCancelled.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Cancelled.GetStringValue() + "')");
            ancConflicts.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Conflicts.GetStringValue() + "')");
        }

        private void BindPieChartData()
        {
            Dictionary<String, Int32> dicPieChartData = new Dictionary<String, Int32>();

            //Method to get data to bind pie chart. // TODO
            if (string.Compare(ddlPieChartFilters.SelectedValue, LKPDurationOptions.LAST_WEEK.GetStringValue()) == 0)
            {
                int dayOfWeek = (int)DateTime.Now.DayOfWeek;

                DateTime lastMonday = DateTime.Now.AddDays(-(dayOfWeek + 6));
                CurrentViewContext.FromDate = lastMonday.Date;
                CurrentViewContext.ToDate = lastMonday.Date.AddDays(6);
            }
            else if (string.Compare(ddlPieChartFilters.SelectedValue, LKPDurationOptions.LAST_MONTH.GetStringValue()) == 0)
            {
                DateTime firstOfMonth = DateTime.Now.AddDays(-1 * DateTime.Now.Day + 1);
                CurrentViewContext.FromDate = firstOfMonth.AddMonths(-1);
                CurrentViewContext.ToDate = firstOfMonth.AddDays(-1);
            }
            else if (string.Compare(ddlPieChartFilters.SelectedValue, LKPDurationOptions.LAST_YEAR.GetStringValue()) == 0)
            {
                //DateTime dt = DateTime.Now.AddYears(-1).AddDays(1);
                CurrentViewContext.FromDate = DateTime.Now.AddYears(-1).AddDays(1);
                CurrentViewContext.ToDate = DateTime.Now;
            }
            else if (string.Compare(ddlPieChartFilters.SelectedValue, LKPDurationOptions.DATE_RANGE.GetStringValue()) == 0)
            {
                CurrentViewContext.FromDate = dpFromDate.SelectedDate.HasValue ? dpFromDate.SelectedDate : new DateTime(1980, 1, 1);
                CurrentViewContext.ToDate = dpToDate.SelectedDate.HasValue ? dpToDate.SelectedDate : DateTime.Now;
            }


            Presenter.GetIntitutionsRequestsApproved();

            List<InstitutionRequestPieChartContract> lst = (List<InstitutionRequestPieChartContract>)ViewState["PieChartDataSource"];
            RdPieChartInstitutionProfile.DataSource = lst;
            RdPieChartInstitutionProfile.DataBind();
            ViewState["PieChartDataSource"] = lst;
            spnPieInformation.InnerText = string.Concat("Below pie chart displaying information from ", CurrentViewContext.FromDate.Value.ToString("dd MMM yyyy"), " - ", CurrentViewContext.ToDate.Value.ToString("dd MMM yyyy"), " against ", ddlPieChartFilters.SelectedItem.Text, " option selection.");
            // }

            if (CurrentViewContext.lstInstitutionRequestsApproved != null && CurrentViewContext.lstInstitutionRequestsApproved.Count > 0)
            {
                //Assigning Pie chart data into viewstate
                RdPieChartInstitutionProfile.DataSource = CurrentViewContext.lstInstitutionRequestsApproved;
                RdPieChartInstitutionProfile.DataBind();
                ViewState["PieChartDataSource"] = CurrentViewContext.lstInstitutionRequestsApproved;
                spnPieInformation.InnerText = string.Concat("Below pie chart displaying information from ", CurrentViewContext.FromDate.Value.ToString("dd MMM yyyy"), " - ", CurrentViewContext.ToDate.Value.ToString("dd MMM yyyy"), " against ", ddlPieChartFilters.SelectedItem.Text, " option selection.");
            }
            else
            {
                ViewState["PieChartDataSource"] = new List<InstitutionRequestPieChartContract>();
                RdPieChartInstitutionProfile.DataSource = new List<InstitutionRequestPieChartContract>();
                RdPieChartInstitutionProfile.DataBind();
                if (string.Compare(ddlPieChartFilters.SelectedValue, LKPDurationOptions.DATE_RANGE.GetStringValue()) == 0)
                    spnPieInformation.InnerText = "There are no records that match your selected date range, please update the date range and try again.";
                else
                    spnPieInformation.InnerText = string.Concat("There are no records found against ", ddlPieChartFilters.SelectedItem.Text, " option selection.");
            }

        }

        //private void BindSearchContract()
        //{
        //    CurrentViewContext.SearchRequestContract = new RequestDetailContract();
        //    CurrentViewContext.SearchRequestContract.InventoryAvailabilityTypeCode = InstitutionAvailabilityType.AllInstitutions.GetStringValue();
        //    CurrentViewContext.SearchRequestContract.StatusCode = CurrentViewContext.SelectedStatusCode;
        //    CurrentViewContext.SearchRequestContract.StatusID = CurrentViewContext.lstRequestStatus.Where(cond => cond.Code == CurrentViewContext.SelectedStatusCode).FirstOrDefault().StatusID;
        //}

        private void BindStatusRecordsCount()
        {
            emPendingReview.InnerHtml = CurrentViewContext.lstAllRequests.Where(cond => cond.StatusCode == RequestStatusCodes.Requested.GetStringValue()).Count().ToString();
            emModified.InnerHtml = CurrentViewContext.lstAllRequests.Where(cond => cond.StatusCode == RequestStatusCodes.Modified.GetStringValue()).Count().ToString();
            emApproved.InnerHtml = CurrentViewContext.lstAllRequests.Where(cond => cond.StatusCode == RequestStatusCodes.Approved.GetStringValue()).Count().ToString();
            emRejected.InnerHtml = CurrentViewContext.lstAllRequests.Where(cond => cond.StatusCode == RequestStatusCodes.Rejected.GetStringValue()).Count().ToString();
            emArchived.InnerHtml = CurrentViewContext.lstAllRequests.Where(cond => cond.StatusCode == RequestStatusCodes.Archived.GetStringValue()).Count().ToString();
            emCancelled.InnerHtml = CurrentViewContext.lstAllRequests.Where(cond => cond.StatusCode == RequestStatusCodes.Cancelled.GetStringValue()).Count().ToString();
            emConflicts.InnerHtml = CurrentViewContext.lstAllRequests.Where(cond => cond.StatusCode == RequestStatusCodes.Conflicts.GetStringValue()).Count().ToString();
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion

    }
}