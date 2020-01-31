using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class PlacementDashboard : BaseUserControl, IPlacementView
    {
        private PlacementViewPresenter _presenter = new PlacementViewPresenter();
        Int32 tenantId;
        public PlacementViewPresenter Presenter
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

        public IPlacementView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 AgencyID { get; set; }

        bool IPlacementView.IsAdminLoggedIn { get; set; }
        Guid IPlacementView.CurrentLoggedInUserID
        {
            get
            {
                return base.SysXMembershipUser.UserId;
            }
        }

        Int32 IPlacementView.TenantID
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.IsSysXAdmin ? AppConsts.NONE : user.TenantId.Value;
                    }
                }
                return tenantId;

            }
            set { tenantId = value; }
        }
        List<AgencyHierarchyContract> IPlacementView.LstAgencies
        {


            get
            {
                if (!ViewState["LstAgencies"].IsNullOrEmpty())
                    return (ViewState["LstAgencies"]) as List<AgencyHierarchyContract>;
                return new List<AgencyHierarchyContract>();
            }
            set
            {
                ViewState["LstAgencies"] = value;
            }
        }
        Boolean IPlacementView.IsGridView
        {
            get
            {
                if (!ViewState["IsGridView"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsGridView"]);
                return false;
            }
            set
            {
                ViewState["IsGridView"] = value;
            }
        }

        Int32 IPlacementView.AgencyHierarchyID
        {

            get
            {
                if (!ViewState["AgencyHierarchyID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["AgencyHierarchyID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["AgencyHierarchyID"] = value;
            }
        }
        String IPlacementView.StatusCode
        {

            get
            {
                if (!ViewState["StatusCode"].IsNullOrEmpty())
                    return Convert.ToString(ViewState["StatusCode"]);
                return String.Empty;
            }
            set
            {
                ViewState["StatusCode"] = value;
            }
        }
        Boolean IPlacementView.IsCalendarView
        {
            get
            {
                if (!ViewState["IsCalendarView"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsCalendarView"]);
                return false;
            }
            set
            {
                ViewState["IsCalendarView"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    dvGridView.Style.Add("Display", "none");
                    hdnSelectedStatus.Value = RequestStatusCodes.Requested.ToString();
                    hdnSelectedStatusCode.Value = RequestStatusCodes.Requested.GetStringValue();
                    BindStatusBar();
                    hdnStatusCount.Value = spnPendingRequestCount.InnerText;
                    Presenter.GetAgencyRootNodes();
                    ddAgency.DataSource = CurrentViewContext.LstAgencies;
                    ddAgency.DataBind();
                }
                CurrentViewContext.StatusCode = CalenderViewControl.StatusCode = GridViewControl.StatusCode = hdnSelectedStatusCode.Value;
                CurrentViewContext.AgencyHierarchyID = GridViewControl.AgencyHierarchyID = CalenderViewControl.AgencyHierarchyID = ddAgency.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ddAgency.SelectedValue);
                CalenderViewControl.eventHandleView += new CalenderViewControl.HandleView(HandleViews);
                GridViewControl.eventHandleView += new GridViewControl.HandleView(HandleViews);

                //ButtonStyle();
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

        protected void ddAgency_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                GridViewControl.AgencyHierarchyID = CalenderViewControl.AgencyHierarchyID = Convert.ToInt32(ddAgency.SelectedValue);
                BindStatusBar();
                BindPlacementStatusLabel();
                GridViewControl.BindGrid();
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


        private void HandleViews(Boolean isGridView)
        {
            if (isGridView)
            {
                dvCalenderView.Style.Add("Display", "none");
                dvGridView.Style.Add("Display", "block");
            }
            else
            {
                dvCalenderView.Style.Add("Display", "block");
                dvGridView.Style.Add("Display", "none");
                GridViewControl.BindGrid();
            }
        }

        protected void btnDoPostBack_Click(object sender, EventArgs e)
        {
            GridViewControl.BindGrid();
        }

        #region Private Methods
        private void BindStatusBar()
        {
            if (!ddAgency.SelectedValue.IsNullOrEmpty())
            {
                var lstofStatuses = Presenter.GetRequestStatusBarCounts(Convert.ToInt32(ddAgency.SelectedValue));
                SpnApprovedRequestCount.InnerText = lstofStatuses.Where(cond => cond.Code == RequestStatusCodes.Approved.GetStringValue()).Select(a => a.Count).FirstOrDefault().ToString();
                SpnArchivedRequestCount.InnerText = lstofStatuses.Where(cond => cond.Code == RequestStatusCodes.Archived.GetStringValue()).Select(a => a.Count).FirstOrDefault().ToString();
                SpnCancelledRequestCount.InnerText = lstofStatuses.Where(cond => cond.Code == RequestStatusCodes.Cancelled.GetStringValue()).Select(a => a.Count).FirstOrDefault().ToString();
                spnModifiedRequestCount.InnerText = lstofStatuses.Where(cond => cond.Code == RequestStatusCodes.Modified.GetStringValue()).Select(a => a.Count).FirstOrDefault().ToString();
                SpnRejectedRequestCount.InnerText = lstofStatuses.Where(cond => cond.Code == RequestStatusCodes.Rejected.GetStringValue()).Select(a => a.Count).FirstOrDefault().ToString();
                spnPendingRequestCount.InnerText = lstofStatuses.Where(cond => cond.Code == RequestStatusCodes.Requested.GetStringValue()).Select(a => a.Count).FirstOrDefault().ToString();

            }
            BindOnClickEvent();
        }

        private void BindOnClickEvent()
        {
            Requested.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Requested.ToString().ToUpper() + "','" + RequestStatusCodes.Requested.GetStringValue() + "','" + spnPendingRequestCount.InnerText + "');");
            Approved.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Approved.ToString().ToUpper() + "','" + RequestStatusCodes.Approved.GetStringValue() + "','" + SpnApprovedRequestCount.InnerText + "');");
            Modified.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Modified.ToString().ToUpper() + "','" + RequestStatusCodes.Modified.GetStringValue() + "','" + spnModifiedRequestCount.InnerText + "');");
            Cancelled.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Cancelled.ToString().ToUpper() + "','" + RequestStatusCodes.Cancelled.GetStringValue() + "','" + SpnCancelledRequestCount.InnerText + "');");
            Archived.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Archived.ToString().ToUpper() + "','" + RequestStatusCodes.Archived.GetStringValue() + "','" + SpnArchivedRequestCount.InnerText + "');");
            Rejected.Attributes.Add("onclick", "statusClick('" + RequestStatusCodes.Rejected.ToString().ToUpper() + "','" + RequestStatusCodes.Rejected.GetStringValue() + "','" + SpnRejectedRequestCount.InnerText + "');");
        }
        private void BindPlacementStatusLabel()
        {

            var statusCode = hdnSelectedStatusCode.Value;
            switch (statusCode)
            {
                case "AAAA":
                    {
                        hdnStatusCount.Value = spnPendingRequestCount.InnerText;
                        break;
                    }
                case "AAAB":
                    {
                        hdnStatusCount.Value = spnModifiedRequestCount.InnerText;
                        break;
                    }
                case "AAAC":
                    {
                        hdnStatusCount.Value = SpnApprovedRequestCount.InnerText;
                        break;
                    }
                case "AAAD":
                    {
                        hdnStatusCount.Value = SpnRejectedRequestCount.InnerText;
                        break;
                    }
                case "AAAE":
                    {
                        hdnStatusCount.Value = SpnRejectedRequestCount.InnerText;
                        break;
                    }
                case "AAAF":
                    {
                        hdnStatusCount.Value = SpnArchivedRequestCount.InnerText;
                        break;
                    }
                default:
                    {
                        hdnStatusCount.Value = spnPendingRequestCount.InnerText;
                        break;
                    }
            }
        }
        #endregion
    }
}