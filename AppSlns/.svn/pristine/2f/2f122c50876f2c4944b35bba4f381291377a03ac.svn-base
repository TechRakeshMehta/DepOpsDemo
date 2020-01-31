using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;

namespace CoreWeb.ClinicalRotation.UserControl
{
    public partial class RequirementDetails : BaseUserControl
    {
        delegate void LoadDataItemPanel(Dictionary<string, string> args);

        public Int32 SelectedReqCategoryId_Global
        {
            get { return Convert.ToInt32(ViewState["SelectedReqCategoryId_Global"] ?? "0"); }
            set { ViewState["SelectedReqCategoryId_Global"] = value; }
        }
        public Int32 SelectedReqPackageSubscriptionID_Global
        {
            get
            {
                if (ViewState["SelectedReqPackageSubscriptionID_Global"].IsNotNull())
                    return (Int32)(ViewState["SelectedReqPackageSubscriptionID_Global"]);
                else
                    return 0;
            }

            set { ViewState["SelectedReqPackageSubscriptionID_Global"] = value; }
        }



        public Int32 CurrentTenantId_Global
        {
            get
            {
                if (ViewState["CurrentTenantId"].IsNotNull())
                {
                    return (Int32)(ViewState["CurrentTenantId"]);
                }
                else
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        ViewState["CurrentTenantId"] = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }


                return (Int32)(ViewState["CurrentTenantId"]);
            }
            set { ViewState["CurrentTenantId"] = value; }
        }
        /// <summary>
        /// OnInit event
        /// </summary>
        /// <param name="e"></param>
        /// 
        protected override void OnInit(EventArgs e)
        {
            try
            {
                LoadDataItemPanel _reload = new RequirementDetails.LoadDataItemPanel(LoadItemsPanel);

                base.OnInit(e);
                base.Title = "Verification Details";
                BasePage basePage = base.Page as BasePage;
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
                Int32 tenantID = 0;
                String useTypeCode = String.Empty;
                Int32 applicantId = 0;
                String PageType = String.Empty;
                String UserId = String.Empty;
                Int32 rotationId = 0;
                Int32 agencyId = 0;
                Int32 requirementPackageTypeId = 0;
                Int32 ClientContactId = 0;

                Boolean childHighlightRotationFieldUpdatedByAgencies = false;


                CaptureQueryStringParameters(ref tenantID, ref useTypeCode, ref applicantId, ref PageType, ref UserId, ref rotationId, ref agencyId, ref requirementPackageTypeId, ref childHighlightRotationFieldUpdatedByAgencies, ref ClientContactId);

                ucItemControl.CurrentTenantId_Global = this.CurrentTenantId_Global;

                if (requirementPackageTypeId == 2)
                    btnSendMsg.Visible = false;
                else
                    btnSendMsg.Visible = true;

                var queryString = new Dictionary<String, String>();
                if (useTypeCode.ToLower().Trim() == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower().Trim())
                {
                    queryString = new Dictionary<String, String>
                                  {
                                     { ProfileSharingQryString.SelectedTenantId,  Convert.ToString(tenantID) },
                                     { "Child",  @"~\ClinicalRotation\RotationMemberSearch.ascx"}
                                  };
                }
                else if (useTypeCode.ToLower().Trim() == AppConsts.ROTATION_PORTFOLIO_SEARCH_USE_TYPE_CODE.ToLower().Trim())
                {
                    hdnPageType.Value = PageType;
                    queryString = new Dictionary<String, String>
                                  {
                                     { "TenantId",  Convert.ToString(tenantID) },
                                     { "OrganizationUserId",  Convert.ToString(applicantId) },
                                     //{"PageType", WorkQueueType.ApplicantPortFolioSearch.ToString()},
                                     {"PageType", PageType},
                                     { "Child",  @"~\SearchUI\UserControl\ApplicantPortfolioDetails.ascx"},
                                     {"UserId",UserId},
                                     {"PageTypeChild",WorkQueueType.ApplicantPortFolioDetail.ToString()}
                                  };
                    lnkGoBack.InnerText = "Back to Portfolio Details";
                    lnkGoBack.HRef = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                    return;
                }
                else if (useTypeCode.ToLower().Trim() == AppConsts.SUPPORT_PORTAL_DETAIL_USE_TYPE_CODE.ToLower().Trim())
                {
                    queryString = new Dictionary<String, String>
                                  {
                                     { "TenantId",  Convert.ToString(tenantID) },
                                     { "Child",ChildControls.SupportPortalDetails },
                                    { "OrganizationUserId",Convert.ToString(applicantId)},
                                    {"PageType",PageType},
                                    {"UserId",UserId},
                                    {"PageTypeChild",WorkQueueType.SupportPortalDetail.ToString()}
                                  };
                    lnkGoBack.InnerText = "Back to Support Portal Details";
                    lnkGoBack.HRef = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                    return;
                }
                else if (useTypeCode.ToLower().Trim() == AppConsts.SUPPORT_PORTAL_DETAIL_INSTRUCTOR_USE_TYPE_CODE.ToLower().Trim())
                {
                    queryString = new Dictionary<String, String>
                                  {
                                    { "TenantId",  Convert.ToString(tenantID) },
                                    { "Child",ChildControls.InstructorSupportPortalDetails },
                                    { "OrganizationUserId",Convert.ToString(applicantId)},
                                    { "PageType",PageType},
                                    { "UserId",UserId},
                                    { "ClientContactId",Convert.ToString(ClientContactId)},
                                    {"PageTypeChild",WorkQueueType.SupportPortalDetail.ToString()}
                                  };
                    lnkGoBack.InnerText = "Back to Support Portal Details";
                    lnkGoBack.HRef = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                    return;
                }
                else if (useTypeCode.ToLower().Trim() == AppConsts.ASSIGN_ROTATION_VERIFICATION_QUEUE_TYPE_CODE.ToLower().Trim())
                {
                    queryString = new Dictionary<String, String>
                    {
                       { ProfileSharingQryString.SelectedTenantId, Convert.ToString(tenantID) },
                       { "Child",  @"~\ClinicalRotation\AssignRotationVerificationRecords.ascx"}
                    };

                    lnkGoBack.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                }
                else if (useTypeCode.ToLower().Trim() == AppConsts.ROTATION_VERIFICATION_USER_WORK__QUEUE_TYPE_CODE.ToLower().Trim())
                {
                    queryString = new Dictionary<String, String>
                    {
                       { ProfileSharingQryString.SelectedTenantId, Convert.ToString(tenantID) },
                       { "Child",  @"~\ClinicalRotation\RotationVerificationUserWorkQueue.ascx"}
                    };

                    lnkGoBack.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                }
                //START OF UAT-3049
                else if (useTypeCode.ToLower().Trim() == AppConsts.ROTATION_DETAIL_USE_TYPE_CODE.ToLower().Trim())
                {
                    queryString = new Dictionary<String, String>
                                  {
                                     { ProfileSharingQryString.SelectedTenantId, Convert.ToString(tenantID) },
                                     {"ID", Convert.ToString(rotationId)},
                                     {ProfileSharingQryString.AgencyId,Convert.ToString(agencyId)},
                                     { "Child",  @"~\ClinicalRotation\UserControl\RotationDetailForm.ascx"},
                                     {AppConsts.HIGHLIGHT_ROTATION_FIELD_UPDATED_BY_AGENCIES,Convert.ToString(childHighlightRotationFieldUpdatedByAgencies)},
                                   };

                    lnkGoBack.InnerText = "Back to Rotation Details";
                    lnkGoBack.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                }
                //END OF UAT:-3049
                else
                {
                    queryString = new Dictionary<String, String>
                                  {
                                     { ProfileSharingQryString.SelectedTenantId, Convert.ToString(tenantID) },
                                     { "Child",  @"~\ClinicalRotation\RequirementVerificationQueue.ascx"}
                                  };
                }
                lnkGoBack.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            }
            catch (Exception ex)
            {
                base.ShowInfoMessage(ex.Message);
                base.LogError(ex);
            }
        }



        private void CaptureQueryStringParameters(ref Int32 tenantID, ref String useTypeCode, ref Int32 applicantId, ref String PageType, ref String UserId, ref Int32 rotationId, ref Int32 agencyId, ref Int32 requirementPackageTypeId, ref Boolean childHighlightRotationFieldUpdatedByAgencies, ref int ClientContactId)
        {
            var args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
                {
                    tenantID = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);
                }
                if (args.ContainsKey(ProfileSharingQryString.ControlUseType))
                {
                    useTypeCode = Convert.ToString(args[ProfileSharingQryString.ControlUseType]);
                }
                if (args.ContainsKey(ProfileSharingQryString.ApplicantId))
                {
                    applicantId = Convert.ToInt32(args[ProfileSharingQryString.ApplicantId]);
                }
                if (args.ContainsKey("SelectedReqComplianceCategoryId"))
                {
                    this.SelectedReqCategoryId_Global = Convert.ToInt32(args["SelectedReqComplianceCategoryId"]);
                    hdnFirstCatagoryID.Value = Convert.ToString(args["SelectedReqComplianceCategoryId"]);
                }
                if (args.ContainsKey("SelectedReqPackageSubscriptionId"))
                {
                    this.SelectedReqPackageSubscriptionID_Global = Convert.ToInt32(args["SelectedReqPackageSubscriptionId"]);
                }
                if (args.ContainsKey(ProfileSharingQryString.PageType))
                {
                    if (args["PageType"] != null)
                    {
                        PageType = Convert.ToString(args["PageType"]);
                    }
                    else { PageType = ""; }
                }
                if (args.ContainsKey("UserId"))
                {
                    UserId = Convert.ToString(args["UserId"]);
                }
                //UAT-3049
                //if (args.ContainsKey(ProfileSharingQryString.RotationId))
                //{
                //    rotationId = Convert.ToInt32(args[ProfileSharingQryString.RotationId]);
                //}
                if (args.ContainsKey("RotationId"))
                {
                    rotationId = Convert.ToInt32(args["RotationId"]);
                }
                if (args.ContainsKey("AgencyId"))
                {
                    agencyId = Convert.ToInt32(args["AgencyId"]);
                }
                if (args.ContainsKey("PackageTypeId"))
                {
                    requirementPackageTypeId = Convert.ToInt32(args["PackageTypeId"]);
                }
                if (args.ContainsKey(AppConsts.CHILD_HIGHLIGHT_ROTATION_FIELD_UPDATED_BY_AGENCIES))
                {
                    childHighlightRotationFieldUpdatedByAgencies = Convert.ToBoolean(args[AppConsts.CHILD_HIGHLIGHT_ROTATION_FIELD_UPDATED_BY_AGENCIES]);
                }
                if (args.ContainsKey(ProfileSharingQryString.ClientContactId))
                {
                    ClientContactId = Convert.ToInt32(args["ClientContactId"]);
                }
            }
        }

        public void LoadItemsPanel(Dictionary<string, string> args)
        {
            this.CurrentTenantId_Global = (args.ContainsKey("CurrentTenantId_Global")) ? Convert.ToInt32(args["CurrentTenantId_Global"]) : this.CurrentTenantId_Global;
            ucItemControl.CurrentTenantId_Global = this.CurrentTenantId_Global;
        }
    }
}