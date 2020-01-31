using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.IntsofSecurityModel;

namespace CoreWeb.Search.Views
{
    public partial class ApplicantRequirementRotations : BaseUserControl, IApplicantRequirementRotationsView
    {
        #region Private Variables

        private String _viewType;

        private ApplicantRequirementRotationsPresenter _presenter = new ApplicantRequirementRotationsPresenter();
        private List<ClinicalRotationDetailContract> _lstRotations = new List<ClinicalRotationDetailContract>();
        private Int32 tenantId = 0;

        #endregion

        #region Properties

        #region Private Properties

        private ApplicantRequirementRotationsPresenter Presenter
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

        #endregion

        #region Public Properties

        /// <summary>
        /// Represents the Current View Context
        /// </summary>
        public IApplicantRequirementRotationsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        public Int32 OrganizationUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }
        }

        public Int32 TenantId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("TenantId"))
                    {
                        return (Convert.ToInt32(args["TenantId"]));
                    }
                }
                return 0;
            }
        }

        public Int32 LoggedInUserTenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
        }

        /// <summary>
        /// Represents the list of Rotations to which the applicant belongs to.
        /// </summary>
        public List<ClinicalRotationDetailContract> lstApplicantRotations
        {
            set
            {
                _lstRotations = value;
            }
            get
            {
                return _lstRotations;
            }
        }

        public String QueueType
        {
            get
            {
                if (!ViewState["QueueTypeRotation"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["QueueTypeRotation"]);
                }
                return null;
            }
            set
            {
                ViewState["QueueTypeRotation"] = value;
            }
        }

        public String QueueTypeChild
        {
            get
            {
                if (!ViewState["QueueTypeChild"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["QueueTypeChild"]);
                }
                return null;
            }
            set
            {
                ViewState["QueueTypeChild"] = value;
            }
        }

        public String UserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("UserId"))
                    {
                        return (Convert.ToString(args["UserId"]));
                    }
                }
                return String.Empty;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

        }

        #endregion

        #region Grid Events

        protected void grdRequirementRotations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetClinicalRotations();
                grdRequirementRotations.DataSource = CurrentViewContext.lstApplicantRotations;
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

        protected void grdRequirementRotations_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String rotationID = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationId"]);
                    String ReqPkgSubscriptionId = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PkgSubscriptionId"]);
                    //string queueType = "";

                    //ApplicantPortfolioDetails parentControl = (ApplicantPortfolioDetails)this.Parent;
                    //queueType = parentControl.QueueType;
                    if (CurrentViewContext.QueueType == WorkQueueType.SupportPortalDetail.ToString() && CurrentViewContext.QueueTypeChild == WorkQueueType.SupportPortalDetail.ToString())
                    {
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId, ReqPkgSubscriptionId },
                                                                    { ProfileSharingQryString.RotationId, rotationID }, 
                                                                    { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.OrganizationUserId) },
                                                                    { ProfileSharingQryString.ControlUseType,AppConsts.SUPPORT_PORTAL_DETAIL_USE_TYPE_CODE },
                                                                    {"PageType",CurrentViewContext.QueueType},
                                                                    {"PageTypeChild",CurrentViewContext.QueueTypeChild},
                                                                    {"UserId",UserId}
                                                                 };
                    }
                    else if (CurrentViewContext.QueueType == WorkQueueType.SupportPortalDetail.ToString() && CurrentViewContext.QueueTypeChild == WorkQueueType.ApplicantPortFolioDetail.ToString())
                    {
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId, ReqPkgSubscriptionId },
                                                                    { ProfileSharingQryString.RotationId, rotationID }, 
                                                                    { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.OrganizationUserId) },
                                                                    { ProfileSharingQryString.ControlUseType,AppConsts.ROTATION_PORTFOLIO_SEARCH_USE_TYPE_CODE },
                                                                    {"PageType",CurrentViewContext.QueueType},
                                                                    {"PageTypeChild",CurrentViewContext.QueueTypeChild},
                                                                    {"UserId",UserId}
                                                                 };
                    }
                    else
                    {
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId, ReqPkgSubscriptionId },
                                                                    { ProfileSharingQryString.RotationId, rotationID }, 
                                                                    { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.OrganizationUserId) },
                                                                    { ProfileSharingQryString.ControlUseType,AppConsts.ROTATION_PORTFOLIO_SEARCH_USE_TYPE_CODE },
                                                                    {"PageType",CurrentViewContext.QueueType},
                                                                 };
                    }
                    string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
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

        #region Private Methods


        #endregion



    }
}