using CoreWeb.PlacementMatching.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using INTSOF.Utils;
using INTSOF.UI.Contract.PlacementMatching;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class CreateRequestPopup : BaseWebPage, IRequestView
    {

        private Int32 _tenantId;

        public IRequestView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IRequestView.OpportunityId { get; set; }
        Int32 IRequestView.RequestId { get; set; }
        String IRequestView.PageRequested { get; set; }
        String IRequestView.RequestStatusCode { get; set; }
        PlacementMatchingContract IRequestView.OpportunityDetails
        {
            get;
            set;
        }
        Int32 IRequestView.CurrentLoggedInUser
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        RequestDetailContract IRequestView.RequestDetail
        {
            get;
            set;
        }
        Int32 IRequestView.SelectedTenantID
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        Boolean IRequestView.IsSharedUser
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull() && user.IsSharedUser.IsNotNull() && user.IsSharedUser)
                {
                    return true;
                }
                return false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    CaptureQueryStringParameters();
                    ucCreateDraftControl.OpportunityId = CurrentViewContext.OpportunityId;
                    ucCreateDraftControl.PageRequested = CurrentViewContext.PageRequested;
                    ucCreateDraftControl.RequestId = CurrentViewContext.RequestId;
                    ucCreateDraftControl.RequestStatusCode = CurrentViewContext.RequestStatusCode;
                    ucCreateDraftControl.SelectedTenantID = CurrentViewContext.SelectedTenantID;
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

        private void CaptureQueryStringParameters()
        {
            if (!Request.QueryString["OpportunityId"].IsNullOrEmpty())
            {
                CurrentViewContext.OpportunityId = Convert.ToInt32(Request.QueryString["OpportunityId"]);
            }
            if (!Request.QueryString["RequestedPage"].IsNullOrEmpty())
            {
                CurrentViewContext.PageRequested = Request.QueryString["RequestedPage"];
            }
            if (!Request.QueryString["RequestId"].IsNullOrEmpty())
            {
                CurrentViewContext.RequestId = Convert.ToInt32(Request.QueryString["RequestId"]);
            }
            if (!Request.QueryString["RequestStatusCode"].IsNullOrEmpty())
            {
                CurrentViewContext.RequestStatusCode = Convert.ToString(Request.QueryString["RequestStatusCode"]);
            }
            if (!Request.QueryString["SelectedTenantID"].IsNullOrEmpty())
            {
                CurrentViewContext.SelectedTenantID = Convert.ToInt32(Request.QueryString["SelectedTenantID"]);
            }
        }
    }
}