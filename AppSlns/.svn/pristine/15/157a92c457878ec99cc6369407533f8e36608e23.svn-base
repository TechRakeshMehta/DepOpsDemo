using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Linq;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Core;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using INTSOF.ServiceUtil;
using INTSOF.Contracts;
using System.Web;
using System.Web.Configuration;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RotationDetailForm : BaseUserControl, IRotationDetailFormView
    {
        #region Variables

        private RotationDetailFormPresenter _presenter = new RotationDetailFormPresenter();
        private String _viewType;
        private Int32 _tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract _gridSearchContract = null;
        private Boolean _isAssignPkgFullAccess = true;

        #endregion;

        #region Properties

        public RotationDetailFormPresenter Presenter
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

        public IRotationDetailFormView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IRotationDetailFormView.ClinicalRotationID
        {
            get
            {
                if (!ViewState["ClinicalRotationID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["ClinicalRotationID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ClinicalRotationID"] = value;
            }
        }
        //UAT-2313       
        String IRotationDetailFormView.SourceScreen
        {
            get
            {

                return (Session["SourceScreen"]).ToString();

            }
            set
            {
                Session["SourceScreen"] = value;
            }
        }

        ClinicalRotationDetailContract IRotationDetailFormView.ClinicalRotationDetails
        {
            get
            {
                if (!Session["ClinicalRotationDetails"].IsNull())
                {
                    return Session["ClinicalRotationDetails"] as ClinicalRotationDetailContract;
                }
                return new ClinicalRotationDetailContract();
            }
            set
            {
                Session["ClinicalRotationDetails"] = value;
            }
        }

        List<ApplicantDataListContract> IRotationDetailFormView.ApplicantSearchData
        {
            get;
            set;
        }

        Int32 IRotationDetailFormView.SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        Int32 IRotationDetailFormView.TenantId
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

        //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
        Boolean IRotationDetailFormView.IsDisplaySuccessMessage
        {
            get
            {
                if (!ViewState["IsDisplaySuccessMessage"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsDisplaySuccessMessage"]);
                }

                return false;
            }
            set
            {
                ViewState["IsDisplaySuccessMessage"] = value;
            }
        }

        Int32 IRotationDetailFormView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        String IRotationDetailFormView.ApplicantFirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        String IRotationDetailFormView.ApplicantLastName
        {
            get
            {
                return txtLastName.Text;
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        String IRotationDetailFormView.EmailAddress
        {
            get
            {
                return txtEmail.Text;
            }
            set
            {
                txtEmail.Text = value;
            }
        }

        String IRotationDetailFormView.SSN
        {
            get;
            set;
        }

        DateTime? IRotationDetailFormView.DateOfBirth
        {
            get
            {
                return dpkrDOB.SelectedDate;
            }
            set
            {
                dpkrDOB.SelectedDate = value;
            }
        }

        List<UserGroupContract> IRotationDetailFormView.lstUserGroup
        {
            get;
            set;
        }

        List<RequirementPackageContract> IRotationDetailFormView.lstTenantRequirementPackage
        {
            get;
            set;
        }

        List<RequirementPackageContract> IRotationDetailFormView.lstSharedRequirementPackage
        {
            get;
            set;
        }

        List<RequirementPackageContract> IRotationDetailFormView.lstCombinedRequirementPackage
        {
            get
            {
                if (!ViewState["lstCombinedRequirementPackage"].IsNull())
                {
                    return ViewState["lstCombinedRequirementPackage"] as List<RequirementPackageContract>;
                }

                return new List<RequirementPackageContract>();
            }
            set
            {
                ViewState["lstCombinedRequirementPackage"] = value;
            }
        }

        List<RequirementPackageContract> IRotationDetailFormView.lstSharedInstructorRequirementPackages
        {
            get;
            set;
        }


        List<RequirementPackageContract> IRotationDetailFormView.lstCombinedInstructorRequirementPackages
        {
            get
            {
                if (!ViewState["lstCombinedInstructorRequirementPackages"].IsNull())
                {
                    return ViewState["lstCombinedInstructorRequirementPackages"] as List<RequirementPackageContract>;
                }

                return new List<RequirementPackageContract>();
            }
            set
            {
                ViewState["lstCombinedInstructorRequirementPackages"] = value;
            }
        }

        ClinicalRotationRequirementPackageContract IRotationDetailFormView.RotationRequirementPackage
        {
            get;
            set;
        }

        Int32 IRotationDetailFormView.RequirementPackageID
        {
            get
            {
                if (cmbPackage.SelectedIndex > AppConsts.NONE)
                {
                    return Convert.ToInt32(cmbPackage.SelectedValue);
                }
                return AppConsts.NONE;
            }
            set
            {
                //cmbPackage.SelectedValue = value.ToString(); --UAT 1347
                List<RadComboBoxItem> items = cmbPackage.Items.Where(cond => cond.Value == value.ToString()).ToList();

                foreach (RadComboBoxItem item in items)
                {

                    if (!Convert.ToBoolean(item.Attributes["IsShared"]))
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }

        Int32 IRotationDetailFormView.ReturnedRequirementPackageID
        {
            get;
            set;
        }

        Int32 IRotationDetailFormView.FilterUserGroupId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlUserGroup.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
                else
                {
                    ddlUserGroup.SelectedIndex = value;
                }
            }
        }

        Int32 IRotationDetailFormView.MatchUserGroupId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlUserGroup.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
                else
                {
                    ddlUserGroup.SelectedIndex = value;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, Boolean> IRotationDetailFormView.AssignOrganizationUserIds
        {
            get
            {
                if (!ViewState["SelectedApplicants"].IsNull())
                {
                    return ViewState["SelectedApplicants"] as Dictionary<Int32, Boolean>;
                }

                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["SelectedApplicants"] = value;
            }
        }

        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, Tuple<Boolean, Int32>> IRotationDetailFormView.RemovedClinicalRotationMemberIds
        {
            get
            {
                if (!ViewState["RemovedApplicants"].IsNull())
                {
                    return ViewState["RemovedApplicants"] as Dictionary<Int32, Tuple<Boolean, Int32>>;
                }
                return new Dictionary<Int32, Tuple<Boolean, Int32>>();
            }
            set
            {
                ViewState["RemovedApplicants"] = value;
            }
        }

        Dictionary<Int32, Boolean> IRotationDetailFormView.CustomMessageOrgUserIds
        {
            get
            {
                if (!ViewState["CustomMessageOrgUserIds"].IsNull())
                {
                    return ViewState["CustomMessageOrgUserIds"] as Dictionary<Int32, Boolean>;
                }
                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["CustomMessageOrgUserIds"] = value;
            }
        }
        String IRotationDetailFormView.ErrorMessage
        {
            get;
            set;
        }

        String IRotationDetailFormView.SuccessMessage
        {
            get;
            set;
        }

        String IRotationDetailFormView.InfoMessage
        {
            get;
            set;
        }

        //List<RotationMemberDetailContract> IRotationDetailFormView.RotationMemberDetailList
        //{
        //    get
        //    {
        //        if (!ViewState["RotationMemberDetailList"].IsNull())
        //        {
        //            return ViewState["RotationMemberDetailList"] as List<RotationMemberDetailContract>;
        //        }

        //        return new List<RotationMemberDetailContract>();
        //    }
        //    set
        //    {
        //        ViewState["RotationMemberDetailList"] = value;
        //    }
        //}

        List<RotationMemberDetailContract> IRotationDetailFormView.RotationMemberDetailList
        {
            get;
            set;
        }

        String IRotationDetailFormView.SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }

        Boolean IRotationDetailFormView.IsSearchClicked
        {
            get
            {
                if (!ViewState["IsSearchClicked"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsSearchClicked"]);
                }
                return false;
            }
            set
            {
                ViewState["IsSearchClicked"] = value;
            }
        }

        Boolean IRotationDetailFormView.IsRotationPackageEligibleForSharing
        {
            get;
            set;
        }

        List<RotationAndTrackingPkgStatusContract> IRotationDetailFormView.LstStatusMessages
        {
            get;
            set;
        }

        /// <summary>
        /// AgencyID for the Current Rotation
        /// </summary>
        Int32 IRotationDetailFormView.AgencyId
        {
            get
            {
                if (!ViewState["AgencyId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["AgencyId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["AgencyId"] = value;
            }
        }

        Boolean IRotationDetailFormView.IsFromPackageWizard
        {
            get;
            set;
        }

        Boolean IRotationDetailFormView.IsEditMode
        {
            get;
            set;
        }

        List<RequirementPackageContract> IRotationDetailFormView.lstInstructorRequirementPackage
        {
            get;
            set;
        }

        ClinicalRotationRequirementPackageContract IRotationDetailFormView.MappedInstructorRequirementPackage
        {
            get;
            set;
        }

        Int32 IRotationDetailFormView.InstructorRequirementPackageID
        {
            get
            {
                if (cmbInstPackage.SelectedIndex > AppConsts.NONE)
                {
                    return Convert.ToInt32(cmbInstPackage.SelectedValue);
                }
                return AppConsts.NONE;
            }
            set
            {
                //cmbInstPackage.SelectedValue = value.ToString();

                List<RadComboBoxItem> items = cmbInstPackage.Items.Where(cond => cond.Value == value.ToString()).ToList();
                if (CurrentViewContext.InstPercepRequirementPackageID > AppConsts.NONE) //UAT-3702
                {
                    foreach (RadComboBoxItem item in items)
                    {
                        //if (!Convert.ToBoolean(item.Attributes["IsShared"]))
                        //{
                        item.Selected = true;
                        break;
                        //}

                    }
                }

            }
        }

        Int32 IRotationDetailFormView.ReturnedInstRequirementPackageID
        {
            get;
            set;
        }
        //UAT-3702
        Int32 IRotationDetailFormView.InstPercepRequirementPackageID
        {
            get;
            set;
        }

        Int32 IRotationDetailFormView.OldInstRequirementPackageID
        {
            get
            {
                if (!ViewState["OldInstRequirementPackageID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["OldInstRequirementPackageID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["OldInstRequirementPackageID"] = value;
            }
        }

        Boolean IRotationDetailFormView.HighlightRotationFieldUpdatedByAgencies
        {
            get
            {
                if (ViewState["HighlightRotationFieldUpdated"].IsNotNull())
                    return Convert.ToBoolean(ViewState["HighlightRotationFieldUpdated"]);
                else
                    return false;
            }
            set
            {
                ViewState["HighlightRotationFieldUpdated"] = value;
            }
        }

        public string lstSelectedUserIDs
        {
            get;
            set;
        }


        #region UAT-2544:
        /// <summary>
        /// Gets or Sets the value for approved rotation memebers.
        /// </summary>
        Dictionary<Int32, Boolean> IRotationDetailFormView.ApprovedClinicalRotationMemberIdsToRemove
        {
            get
            {
                if (!ViewState["ApprovedClinicalRotationMemberIdsToRemove"].IsNull())
                {
                    return ViewState["ApprovedClinicalRotationMemberIdsToRemove"] as Dictionary<Int32, Boolean>;
                }
                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["ApprovedClinicalRotationMemberIdsToRemove"] = value;
            }
        }

        //UAT-4460
        Dictionary<Int32, Boolean> IRotationDetailFormView.ClinicalRotationMemberIdsToDrop
        {
            get
            {
                if (!ViewState["ClinicalRotationMemberIdsToDrop"].IsNull())
                {
                    return ViewState["ClinicalRotationMemberIdsToDrop"] as Dictionary<Int32, Boolean>;
                }
                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["ClinicalRotationMemberIdsToDrop"] = value;
            }
        }

        /// <summary>
        /// Gets or Sets the value for Rotation Start Date.
        /// </summary>
        DateTime? IRotationDetailFormView.RotationStartDate
        {
            get
            {
                if (!ViewState["RotationStartDate"].IsNull())
                {
                    return Convert.ToDateTime(ViewState["RotationStartDate"]);
                }
                return null;
            }
            set
            {
                ViewState["RotationStartDate"] = value;
            }
        }

        /// <summary>
        /// Gets or Sets the value for Is Rotation Start.
        /// </summary>
        Boolean IRotationDetailFormView.IsRotationStart
        {
            get
            {
                if (!ViewState["IsRotationStart"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsRotationStart"]);
                }
                return false;
            }
            set
            {
                ViewState["IsRotationStart"] = value;
            }
        }
        #endregion

        #region Custom Paging

        /// <summary>
        /// Current Page Index</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdAddToRotation.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdAddToRotation.MasterTableView.CurrentPageIndex > 0)
                {
                    grdAddToRotation.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        /// <summary>
        /// Page Size</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                return grdAddToRotation.PageSize;
            }
            set
            {
                grdAddToRotation.PageSize = value;
            }
        }

        /// <summary>
        /// Virtual Page Count</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualRecordCount
        {
            get
            {
                if (ViewState["ItemCount"] != null)
                    return Convert.ToInt32(ViewState["ItemCount"]);
                else
                    return 0;
            }
            set
            {
                ViewState["ItemCount"] = value;
                grdAddToRotation.VirtualItemCount = value;
                grdAddToRotation.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["GridCustomPaging"] == null)
                {
                    ViewState["GridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["GridCustomPaging"];
            }
            set
            {
                ViewState["GridCustomPaging"] = value;
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        #endregion

        //UAT-1629 : As a client admin, I should not be able to edit rotation packages
        Boolean IRotationDetailFormView.IsAdminLoggedIn
        {
            get;
            set;
        }

        /// <summary>
        /// Granular Permissions of the logged-in user.
        /// </summary>
        Dictionary<String, String> IRotationDetailFormView.dicGranularPermissions
        {
            get;
            set;
        }

        String IRotationDetailFormView.SelectedDPMIds
        {
            get
            {
                return ucCustomAttributeLoaderSearch.DPM_ID;
            }
        }

        //UAT-2090 : Complete Question 4 (C5) from UAT-2052
        String IRotationDetailFormView.Notes
        {
            get;
            set;
        }

        Dictionary<Int32, String> IRotationDetailFormView.lstSelectedOrgUserIDs
        {
            get;
            set;
        }


        String IRotationDetailFormView.RotationAgencyIds
        {
            get
            {
                if (!ViewState["RotationAgencyIds"].IsNull())
                {
                    return Convert.ToString(ViewState["RotationAgencyIds"]);
                }
                return string.Empty;
            }
            set
            {
                ViewState["RotationAgencyIds"] = value;
            }
        }

        //UAT 3041

        Boolean IRotationDetailFormView.IsEditableByClientAdmin
        {
            get
            {
                if (ViewState["IsEditableByClientAdmin"].IsNotNull())
                {
                    return Convert.ToBoolean(ViewState["IsEditableByClientAdmin"]);
                }
                return true;
            }
            set
            {
                ViewState["IsEditableByClientAdmin"] = value;
            }
        }

        Boolean IRotationDetailFormView.IsEditableByAgencyUser
        {
            get
            {
                if (ViewState["IsEditableByAgencyUser"].IsNotNull())
                {
                    return Convert.ToBoolean(ViewState["IsEditableByAgencyUser"]);
                }
                return true;
            }
            set
            {
                ViewState["IsEditableByAgencyUser"] = value;
            }
        }

        #region UAT-3121

        Boolean IRotationDetailFormView.IsApplicantPkgNotAssignedThroughCloning
        {
            get
            {
                if (!ViewState["IsApplicantPkgNotAssignedThroughCloning"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsApplicantPkgNotAssignedThroughCloning"]);
                }
                return false;
            }
            set
            {
                ViewState["IsApplicantPkgNotAssignedThroughCloning"] = value;
            }
        }
        Boolean IRotationDetailFormView.IsInstructorPkgNotAssignedThroughCloning
        {
            get
            {
                if (!ViewState["IsInstructorPkgNotAssignedThroughCloning"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsInstructorPkgNotAssignedThroughCloning"]);
                }
                return false;
            }
            set
            {
                ViewState["IsInstructorPkgNotAssignedThroughCloning"] = value;
            }
        }

        #endregion

        List<Entity.SharedDataEntity.AgencyUser> IRotationDetailFormView.LstAgencyUserByAgency
        {
            get;
            set;
        }

        List<RotationMemberDetailContract> IRotationDetailFormView.lstSelectedRotationMembers
        {
            get;
            set;
        }

        #endregion

        #region Page Events

        /// <summary>
        /// OnInit event to set page titles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.SetPageTitle("Rotation Detail");
                base.Title = "Rotation Detail";
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.SSN = txtSSN.TextWithPrompt;

                #region  "UAT - 2999"
                //Get Granular permissions only if not Admin
                //if (!CurrentViewContext.IsAdminLoggedIn)
                //{
                //    Presenter.GetGranularPermissions();
                //}
                #endregion
                if (!IsPostBack)
                {
                    //Set MinDate and MaxDate for DOB
                    dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                    dpkrDOB.MaxDate = DateTime.Now;
                    //Capture Querystring parameters
                    CaptureQuerystringParameters();
                    //Bind controls

                    BindControls();
                    ApplySSNMask();
                    GetSessionValues();
                    //Bind and show/hide requirement package controls
                    Presenter.BindRotationDetail();
                    ShowHidePackageControls(true);
                    ShowHideInstructorPackageControls(true);
                    //Set Properties of Rotation Details usercontrol to bind
                    (ucRotationDetails as CoreWeb.CommonControls.Views.IRotationDetails).TenantId = CurrentViewContext.SelectedTenantId;
                    (ucRotationDetails as CoreWeb.CommonControls.Views.IRotationDetails).ClinicalRotationId = CurrentViewContext.ClinicalRotationID;
                    //UAT-2544

                    if (!CurrentViewContext.ClinicalRotationDetails.IsNullOrEmpty() && !CurrentViewContext.ClinicalRotationDetails.StartDate.IsNullOrEmpty())
                    {
                        CurrentViewContext.RotationStartDate = CurrentViewContext.ClinicalRotationDetails.StartDate;
                        if (CurrentViewContext.RotationStartDate.Value <= DateTime.Now)
                        {
                            CurrentViewContext.IsRotationStart = true;
                        }
                    }
                    if (!CurrentViewContext.IsAdminLoggedIn && !CurrentViewContext.ClinicalRotationDetails.IsNullOrEmpty() && !CurrentViewContext.ClinicalRotationDetails.EndDate.IsNullOrEmpty() && CurrentViewContext.ClinicalRotationDetails.EndDate < DateTime.Now)
                    {
                        fsucCmdBarButtons.SaveButton.Enabled = false; //UAT-4460
                    }
                    if (!Session["RotationMemberIds"].IsNullOrEmpty())
                    {
                        Session.Remove("RotationMemberIds");
                    }
                    //UAT-3977
                    if (!Session["InstructorPreceptorOrgUserIds"].IsNullOrEmpty())
                    {
                        Session.Remove("InstructorPreceptorOrgUserIds");
                    }
                    //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
                    if (CurrentViewContext.IsDisplaySuccessMessage)
                    {
                        String noteSuccessMessage = String.Empty;
                        //UAT-3121
                        if (CurrentViewContext.IsApplicantPkgNotAssignedThroughCloning && CurrentViewContext.IsInstructorPkgNotAssignedThroughCloning)
                        {
                            noteSuccessMessage = "Previously assigned applicant rotation package as well as instructor rotation package are not available now. Please assign rotation package(s) manually.";
                        }
                        else if (CurrentViewContext.IsApplicantPkgNotAssignedThroughCloning)
                        {
                            noteSuccessMessage = "Previously assigned applicant rotation package is not available now. Please assign rotation package manually.";
                        }
                        else if (CurrentViewContext.IsInstructorPkgNotAssignedThroughCloning)
                        {
                            noteSuccessMessage = "Previously assigned instructor rotation package is not available now. Please assign rotation package manually.";
                        }

                        String DisplaySuccessMessage = "Clinical Rotation saved successfully";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + DisplaySuccessMessage + "','" + noteSuccessMessage + "','sucs');", true);

                        //base.ShowSuccessMessage("testing");
                    }


                }
                if (CurrentViewContext.IsRotationStart)
                {
                    fsucCmdBarButton_AssignRotation.ClearButton.Style.Add("visibility", "hidden");
                    fsucCmdBarButton.ClearButton.Style.Add("visibility", "hidden");
                }
                ShowHideControlOntheBasisOfPermission(); //UAT 3041
                //HideShowControlsForGranularPermission(); //UAT-2999
                ifrExportDocument.Src = String.Empty; //UAT 3049
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

        /// <summary>
        /// Grid NeedDataSource event to bind grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAddToRotation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                //CurrentViewContext.GridCustomPaging = GridCustomPaging;
                Presenter.PerformSearch();
                grdAddToRotation.DataSource = CurrentViewContext.ApplicantSearchData;
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
        /// Grid ItemCommand event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAddToRotation_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                #region Export functionality
                // Implemented the export functionlaity for exporting custom attribute and SSN columns accordingly
                if (e.CommandName.IsNullOrEmpty())
                {
                    if (e.Item is GridCommandItem)
                    {
                        WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                        if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                        {
                            //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                            // and displayed the masked column on Export instead of actual column.
                            grdAddToRotation.MasterTableView.GetColumn("_SSN").Display = true;
                        }
                        else
                        {
                            //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                            // and displayed the masked column on Export instead of actual column.
                            grdAddToRotation.MasterTableView.GetColumn("_SSN").Display = false;
                        }
                    }
                }
                if (e.CommandName == "Cancel")
                {
                    grdAddToRotation.MasterTableView.GetColumn("_SSN").Display = false;
                }
                #endregion
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
        ///  Grid SortCommand event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAddToRotation_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
                    CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
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
        ///  Grid ItemDataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAddToRotation_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                    // and displayed the masked column on Export instead of actual column.
                    dataItem["_SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["_SSN"].Text));

                    //UAT-806 Creation of granular permissions for Client Admin users
                    if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                    {
                        dataItem["SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                    else
                    {
                        ///Formatting SSN
                        dataItem["SSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                }

                //To select checkboxes
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    if (Convert.ToInt32(itemDataId) != 0)
                    {
                        Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.AssignOrganizationUserIds;
                        if (selectedItems.IsNotNull())
                        {
                            if (selectedItems.ContainsKey(Convert.ToInt32(itemDataId)))
                            {
                                CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                checkBox.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                        checkBox.Enabled = false;
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdAddToRotation.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdAddToRotation.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdAddToRotation.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAll"));
                            checkBox.Checked = true;
                        }
                    }
                }
                #region "UAT-2887: Add Select all records to assign to rotation screen"

                String[] checkedOrderIDs = null;
                if (!hdnOrganizationUserId.Value.IsNullOrEmpty())
                {
                    checkedOrderIDs = hdnOrganizationUserId.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    if (checkedOrderIDs.IsNotNull())
                    {
                        if ((e.Item as GridDataItem) != null)
                        {
                            String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();

                            if (!String.IsNullOrEmpty(itemDataId))
                            {
                                if (checkedOrderIDs.Any(cond => cond == itemDataId))
                                {
                                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                    checkBox.Checked = true;
                                }
                            }
                        }
                    }
                }
                #endregion
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
        /// Grid NeedDataSource event to bind grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRotationMembers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetClinicalRotationMembers();
                grdRotationMembers.DataSource = CurrentViewContext.RotationMemberDetailList;
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
        /// Grid ItemCommand event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRotationMembers_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                #region Export functionality
                // Implemented the export functionlaity for exporting custom attribute and SSN columns accordingly
                if (e.CommandName.IsNullOrEmpty())
                {
                    if (e.Item is GridCommandItem)
                    {
                        WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                        if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                        {
                            grdRotationMembers.MasterTableView.GetColumn("CustomAttributesTemp").Display = true;
                            grdRotationMembers.MasterTableView.GetColumn("SchoolComplianceTemp").Display = true;
                            grdRotationMembers.MasterTableView.GetColumn("AgencyComplianceTemp").Display = true;                            
                            //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                            // and displayed the masked column on Export instead of actual column.
                            grdRotationMembers.MasterTableView.GetColumn("_SSN").Display = true;
                        }
                        else
                        {
                            grdRotationMembers.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                            grdRotationMembers.MasterTableView.GetColumn("SchoolComplianceTemp").Display = false;
                            grdRotationMembers.MasterTableView.GetColumn("AgencyComplianceTemp").Display = false;
                            //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                            // and displayed the masked column on Export instead of actual column.
                            grdRotationMembers.MasterTableView.GetColumn("_SSN").Display = false;
                        }
                    }
                }
                if (e.CommandName == "Cancel")
                {
                    grdRotationMembers.MasterTableView.GetColumn("CustomAttributesTemp").Display = false;
                    grdRotationMembers.MasterTableView.GetColumn("_SSN").Display = false;
                    grdRotationMembers.MasterTableView.GetColumn("SchoolComplianceTemp").Display = false; ;
                    grdRotationMembers.MasterTableView.GetColumn("AgencyComplianceTemp").Display = false;
                }
                #endregion

                #region UAT-3049:- Export Documents of rotation member.

                if (e.CommandName == "Detail")
                {

                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String rotationID = CurrentViewContext.ClinicalRotationID.ToString();//(e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"].ToString();
                    String organizationUserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationMemberDetail.OrganizationUserId"].ToString();
                    String ReqPkgSubscriptionId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementSubscriptionId"].ToString();

                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    //{ "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL_CONTROL},
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId, ReqPkgSubscriptionId },
                                                                    //{ ProfileSharingQryString.RotationId, rotationID }, 
                                                                    {"RotationId" , rotationID }, 
                                                                    {"AgencyId",CurrentViewContext.AgencyId.ToString()},
                                                                    { ProfileSharingQryString.ApplicantId , organizationUserID },
                                                                    {ProfileSharingQryString.ControlUseType,AppConsts.ROTATION_DETAIL_USE_TYPE_CODE},
                      
                                 { "ChildHighlightRotationFieldUpdatedByAgencies", Convert.ToString(CurrentViewContext.HighlightRotationFieldUpdatedByAgencies) }
                                                                 };
                    string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }


                if (e.CommandName == "DocumentView")
                {
                    List<RotationMemberSearchDetailContract> lstApplicantRotationToExport = new List<RotationMemberSearchDetailContract>();

                    Int32 rotationMemberID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationMemberDetail.OrganizationUserId"]);
                    lstApplicantRotationToExport.Add(new RotationMemberSearchDetailContract
                                                        {
                                                            RotationID = CurrentViewContext.ClinicalRotationID,
                                                            OrganizationUserID = rotationMemberID
                                                        });

                    if (!lstApplicantRotationToExport.IsNullOrEmpty())
                    {
                        List<ApplicantDocumentContract> lstRotationMemberDocuments = new List<ApplicantDocumentContract>();

                        lstRotationMemberDocuments = Presenter.GetRotationMemberDocuments(lstApplicantRotationToExport);

                        Int32 folderFileCount = AppConsts.NONE;
                        if (lstRotationMemberDocuments.IsNotNull() && lstRotationMemberDocuments.Count > 0)
                        {
                            Int16 fileCount = AppConsts.ONE;
                            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                            String folderName = String.Empty;
                            if (tempFilePath.IsNullOrEmpty())
                            {
                                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                                return;
                            }
                            if (!tempFilePath.EndsWith(@"\"))
                            {
                                tempFilePath += @"\";
                            }
                            folderName = "Tenant_" + CurrentViewContext.SelectedTenantId.ToString() + "_Applicant_Requirement_Doc_Zip_" + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";
                            tempFilePath += folderName;

                            if (!Directory.Exists(tempFilePath))
                                Directory.CreateDirectory(tempFilePath);
                            DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);
                            try
                            {
                                foreach (ApplicantDocumentContract applicantDocumentToExport in lstRotationMemberDocuments.DistinctBy(docId => docId.ApplicantDocumentId))
                                {
                                    var FileNames = lstRotationMemberDocuments.Where(cond => cond.ApplicantDocumentId == applicantDocumentToExport.ApplicantDocumentId)
                                                                             .DistinctBy(dst => dst.FileName).ToList();
                                    var docFileName = GetDocumentFileName(FileNames.Select(col => col.FileName).ToList(), fileCount
                                        , Path.GetExtension(applicantDocumentToExport.DocumentPath));

                                    String newTempFilePath = Path.Combine(tempFilePath, docFileName);
                                    byte[] fileBytes = null;
                                    fileBytes = CommonFileManager.RetrieveDocument(applicantDocumentToExport.DocumentPath, FileType.ApplicantFileLocation.GetStringValue());

                                    if (fileBytes.IsNotNull())
                                    {
                                        try
                                        {
                                            File.WriteAllBytes(newTempFilePath, fileBytes);
                                        }
                                        catch (Exception ex)
                                        {
                                            base.LogError("Error found in bytes write for DocumentID: " + applicantDocumentToExport.ApplicantDocumentId.ToString(), ex);
                                        }
                                    }
                                    fileCount++;
                                }
                                folderFileCount = Directory.GetFiles(tempFilePath).Count();
                                if (folderFileCount > AppConsts.NONE)
                                {
                                    ifrExportDocument.Src = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?zipFilePath=" + tempFilePath + "&IsMultipleFileDownloadInZip=" + "True";
                                }
                                else
                                {
                                    base.ShowInfoMessage("No document(s) found to export.");
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
                        else
                        {
                            base.ShowInfoMessage("No document(s) found to export.");
                        }
                    }

                }
                #endregion

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
        /// Grid ItemDataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRotationMembers_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    RadButton btnView = e.Item.FindControl("btnView") as RadButton;
                    RadButton btnDetail = e.Item.FindControl("btnDetail") as RadButton;
                    Int32 requirementSubscriptionId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementSubscriptionId"]);

                    //UAT-3350 
                    if (!((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsInstructor"].IsNullOrEmpty()))
                    {
                        Boolean IsInstructor = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsInstructor"]);
                        if (IsInstructor)
                        {
                            e.Item.CssClass = e.Item.ItemType.Equals(GridItemType.Item) ? "instructorPreseptorRow rgRow" : "instructorPreseptorRow rgAltRow";
                            if (!btnView.IsNullOrEmpty())
                            {
                                btnView.Visible = true;
                            }
                            if (!btnDetail.IsNullOrEmpty() && requirementSubscriptionId > AppConsts.NONE)
                                btnDetail.Visible = true;
                            else
                                btnDetail.Visible = false;
                            //dataItem["Detail"].Controls[0].Visible = true;
                            //dataItem["DocumentView"].Controls[0].Visible = true;
                        }
                        else
                        {
                            if (!btnView.IsNullOrEmpty())
                            {
                                btnView.Visible = true;
                            }
                            if (!btnDetail.IsNullOrEmpty())
                            {
                                btnDetail.Visible = true;
                            }
                            //dataItem["Detail"].Controls[1].Visible = true;
                            //dataItem["DocumentView"].Controls[1].Visible = true;
                        }
                    }

                    //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                    // and displayed the masked column on Export instead of actual column.
                    dataItem["_SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["_SSN"].Text));

                    //UAT-806 Creation of granular permissions for Client Admin users
                    if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                    {
                        dataItem["SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                    else
                    {
                        ///Formatting SSN
                        dataItem["SSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }

                    //LinkButton ManageDocuments = dataItem["ManageDocuments"].Controls[0] as LinkButton;
                    //ManageDocuments.ToolTip = "Click to go to an admin view of the applicant's Upload Documents screen";
                }

                //To select checkboxes
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {

                    GridDataItem dataItem = (GridDataItem)e.Item;
                    if (Convert.ToString(dataItem["CustomAttributes"].Text).Length > 80)
                    {
                        dataItem["CustomAttributes"].ToolTip = dataItem["CustomAttributes"].Text;
                        dataItem["CustomAttributes"].Text = (dataItem["CustomAttributes"].Text).ToString().Substring(0, 80) + "...";
                    }

                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClinicalRotationMemberId"].ToString();
                    if (Convert.ToInt32(itemDataId) != 0)
                    {
                        Dictionary<Int32, Tuple<Boolean, Int32>> selectedItems = CurrentViewContext.RemovedClinicalRotationMemberIds;
                        if (selectedItems.IsNotNull())
                        {
                            if (selectedItems.ContainsKey(Convert.ToInt32(itemDataId)))
                            {
                                CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkRemoveItem"));
                                checkBox.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkRemoveItem"));
                        // checkBox.Enabled = false;
                    }

                    String schoolCompliance = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SchoolCompliance"].ToString();
                    String agencyCompliance = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyCompliance"].ToString();

                    HtmlAnchor lnkSchoolCompliance = ((HtmlAnchor)e.Item.FindControl("lnkSchoolCompliance"));
                    HtmlAnchor lnkAgencyCompliance = ((HtmlAnchor)e.Item.FindControl("lnkAgencyCompliance"));


                    if (!string.IsNullOrEmpty(schoolCompliance))
                    {
                        //Adding encrypted query string to lnkSchoolCompliance
                        Dictionary<String, String> queryString = new Dictionary<String, String>();

                        String schoolCompliancePackageID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SchoolCompliancePackageID"].ToString();
                        String schoolPackageSubscriptionID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SchoolPackageSubscriptionID"].ToString();
                        String applicantId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationMemberDetail.OrganizationUserId"].ToString();

                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"TenantId", CurrentViewContext.SelectedTenantId.ToString() },
                                                                    {"PackageId",Convert.ToString(schoolCompliancePackageID)},
                                                                    {"PackageSubscriptionId",Convert.ToString(schoolPackageSubscriptionID)} ,
                                                                    {"ApplicantId",Convert.ToString(applicantId)},
                                                                 };


                        lnkSchoolCompliance.Attributes.Add("args", queryString.ToEncryptedQueryString());
                        lnkSchoolCompliance.InnerText = schoolCompliance;
                    }
                    else
                    {
                        lnkSchoolCompliance.Visible = false;
                    }


                    if (!string.IsNullOrEmpty(agencyCompliance))
                    {
                        lnkAgencyCompliance.InnerText = agencyCompliance;
                        String requirementSubscriptionId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementSubscriptionId"].ToString();
                        String applicantId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationMemberDetail.OrganizationUserId"].ToString();
                        Boolean IsInstructor = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsInstructor"]);//UAT-3737
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    {ProfileSharingQryString.SelectedTenantId, CurrentViewContext.SelectedTenantId.ToString() },
                                                                    {ProfileSharingQryString.RotationId, CurrentViewContext.ClinicalRotationID.ToString()},
                                                                    {ProfileSharingQryString.ReqPkgSubscriptionId,requirementSubscriptionId.ToString() },
                                                                    {ProfileSharingQryString.Visibility,"true"},
                                                                    {ProfileSharingQryString.ControlUseType,AppConsts.SHARED_ROTATION_CONTROL_USE_TYPE_CODE},
                                                                    {ProfileSharingQryString.IsOpenInReadOnlyMode,"True"},
                                                                    {"ApplicantId",Convert.ToString(applicantId)},
                                                                    {"IsIntructorPreceptorPkg",Convert.ToString(IsInstructor)},//UAT-3737
                                                                 };
                        string url = String.Format("~/ApplicantRotationRequirement/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        lnkAgencyCompliance.Attributes.Add("args", queryString.ToEncryptedQueryString());
                    }
                    else
                    {
                        lnkAgencyCompliance.Visible = false;
                    }

                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdRotationMembers.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdRotationMembers.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkRemoveItem"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdRotationMembers.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkRemoveAll"));
                            checkBox.Checked = true;
                        }
                    }
                }

                //UAT-3049:- Show the view and detail link in rotation member grid when the package is assign to rotation.
                //RadButton btnView = e.Item.FindControl("btnView") as RadButton;
                //RadButton btnDetail = e.Item.FindControl("btnDetail") as RadButton;
                GetRotationRequirementPackage();

                //UAT-3350 commented this code.
                //if (CurrentViewContext.RotationRequirementPackage.RequirementPackageID > AppConsts.NONE && !CurrentViewContext.RotationRequirementPackage.RequirementPackageID.IsNullOrEmpty())
                //{
                //    if (!btnView.IsNullOrEmpty())
                //    {
                //        btnView.Visible = true;
                //    }
                //    if (!btnDetail.IsNullOrEmpty())
                //    {
                //        btnDetail.Visible = true;
                //    }
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
        #endregion

        #region Button Events

        /// <summary>
        /// Search click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsSearchClicked = true;

                if (ViewState["SelectedApplicants"] != null)
                {
                    ViewState["SelectedApplicants"] = null;
                }
                ViewState["IsBind"] = null;
                //To reset grid filters 
                ResetAddToRotationGridFilters();

                //if (grdAddToRotation.Items.Count > 0)
                //{
                //    fsucCmdBarButton.ClearButton.Style.Clear();
                //}
                //else
                //{
                //    fsucCmdBarButton.ClearButton.Style.Add("display", "none");
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

        /// <summary>
        /// Reset search filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsSearchClicked = false;
                ResetControls();
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
        /// Cancel click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                if (CurrentViewContext.SourceScreen == RotationDetailsScreenSource.MANAGE_ROTATION_BY_AGENCY.GetStringValue())
                {
                    queryString = new Dictionary<String, String>
                                                            {
                                                               { "Child",  AppConsts.MANAGE_ROTATION_BY_AGENCY_CONTROL},
                                                               { "ID", CurrentViewContext.ClinicalRotationID.ToString() },
                                                               {"SelectedTenantId",CurrentViewContext.SelectedTenantId.ToString()},
                                                               {"RebindGrid",AppConsts.YES}
                                                            };


                    String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    if (!Session["SourceScreen"].IsNullOrEmpty())
                    {
                        Session["SourceScreen"] = null;
                    }
                    Response.Redirect(url, true);
                }
                else
                {
                    queryString = new Dictionary<String, String>
                                                            {
                                                               { "Child",  AppConsts.MANAGE_ROTATION_CONTROL},
                                                               { "ID", CurrentViewContext.ClinicalRotationID.ToString() },
                                                               {"SelectedTenantId",CurrentViewContext.SelectedTenantId.ToString()}
                                                            };
                    String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
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

        /// <summary>
        /// Add to Rotation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_AddToRotationClick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.AssignOrganizationUserIds.IsNull()
                     || CurrentViewContext.AssignOrganizationUserIds.Count == AppConsts.NONE)
                {
                    base.ShowInfoMessage("Please select atleast one applicant.");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RemoveDisplayNoneFrompnlErrorDiv();", true);
                }
                else
                {
                    string rorationID = Convert.ToString(CurrentViewContext.ClinicalRotationID);
                    string selectedClientContactIDs = string.Empty;
                    List<ClinicalRotationMembersContract> lstClinicalRotationDetailContract = Presenter.IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(rorationID, CurrentViewContext.SelectedTenantId, String.Join(", ", CurrentViewContext.AssignOrganizationUserIds.Select(x => x.Key)), selectedClientContactIDs);
                    if (lstClinicalRotationDetailContract.IsNullOrEmpty())
                    {
                        WclNumericTextBox ucRotationDetailsTxtStudents = (WclNumericTextBox)this.ucRotationDetails.FindControl("txtStudents");
                        Presenter.GetClinicalRotationMembers();
                        List<RotationMemberDetailContract> lstRotationMembers = CurrentViewContext.RotationMemberDetailList;
                        Int32 cntExistingRotationApplicants = lstRotationMembers.Where(x => x.IsInstructor == false).Count();

                        if (Convert.ToString(ucRotationDetailsTxtStudents.Text).IsNullOrEmpty())
                        {
                            AddApplicantToRotation();
                        }
                        else 
                        {
                            if ((cntExistingRotationApplicants + CurrentViewContext.AssignOrganizationUserIds.Count) <= Convert.ToInt32(ucRotationDetailsTxtStudents.Text))
                            {
                                AddApplicantToRotation();
                            }
                            else 
                            {
                                lblNoOfStudents.Text = ucRotationDetailsTxtStudents.Text;
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowRotationApplicantLimitViolationNotification();", true);
                                return;
                            }
                        }
                    }
                    else // UAT-4147
                    {
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Style.Add("float", "left");
                        HtmlGenericControl ul = new HtmlGenericControl("ul");
                        if (lstClinicalRotationDetailContract.Any(x => x.IsApplicant == false))
                        {
                            lstClinicalRotationDetailContract.ForEach(x =>
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");
                                if (x.IsApplicant == false)
                                {
                                    li.InnerText = "Rotation with Complio ID " + x.ComplioID + " already has " + x.UserName + " as Instructor/Preceptor.";
                                    li.Style["list-style"] = "disc";
                                    ul.Controls.Add(li);
                                }
                            });
                            ul.Style["padding-left"] = "30px";
                            div.Controls.Add(ul);
                            pnlExistingRotationMembers.Controls.Add(div);

                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowExistingRotationMembers();", true);
                            return;
                        }
                        else 
                        {
                            //Presenter.AddApplicantsToRotation();
                            //ResetRotationMembersGridFilters();
                            //ResetAddToRotationGridFilters();
                            //if (ViewState["SelectedApplicants"] != null)
                            //{
                            //    ViewState["SelectedApplicants"] = null;
                            //}
                            //base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                            //hdnOrganizationUserId.Value = "";
                            //ShowHidePackageControls();

                            AddApplicantToRotation();
                        }
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

        private void AddApplicantToRotation()
        {
            Presenter.AddApplicantsToRotation();
            ResetRotationMembersGridFilters();
            ResetAddToRotationGridFilters();
            if (ViewState["SelectedApplicants"] != null)
            {
                ViewState["SelectedApplicants"] = null;
            }
            base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
            hdnOrganizationUserId.Value = "";
            ShowHidePackageControls();
        }

        /// <summary>
        /// Remove Clinical Rotation members
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButtons_RemoveMembersClick(object sender, EventArgs e)
        {
            try
            {
                Boolean IsInstructorPreceptorExist = false;

                if (CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => d.Key <= AppConsts.NONE).Any())
                {
                    var IPlist = CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => d.Key <= AppConsts.NONE).Select(r => r.Value.Item2).ToList();
                    List<Int32> lstOrgUserIds = CurrentViewContext.CustomMessageOrgUserIds.Keys.Select(x => Convert.ToInt32(x)).ToList();
                    if (IPlist.Where(d => lstOrgUserIds.Contains(d)).Any())
                        IsInstructorPreceptorExist = true;
                    // CurrentViewContext.RemovedClinicalRotationMemberIds.Remove(0);
                    //IsInstructorPreceptorExist = true;
                }
                if (IsInstructorPreceptorExist)
                {
                    base.ShowAlertMessage("Please select applicant user(s) only to remove.", MessageType.Information);
                    return;
                }
                //if (CurrentViewContext.RemovedClinicalRotationMemberIds.IsNull()
                //    || CurrentViewContext.RemovedClinicalRotationMemberIds.Count == AppConsts.NONE)
                if (CurrentViewContext.RemovedClinicalRotationMemberIds.IsNull() || CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => !(d.Key <= AppConsts.NONE)).ToList().Count == AppConsts.NONE)
                {
                    base.ShowInfoMessage("Please select atleast one rotation member to remove.");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RemoveDisplayNoneFrompnlErrorDiv();", true);
                }
                else
                {
                    Presenter.RemoveApplicantsFromRotation();
                    ResetRotationMembersGridFilters();
                    ResetAddToRotationGridFilters();
                    if (ViewState["RemovedApplicants"] != null)
                    {
                        ViewState["RemovedApplicants"] = null;
                    }
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    ShowHidePackageControls(true);
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
        /// Profile Share
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButtons_ProfileShareClick(object sender, EventArgs e)
        {
            string selectedRotationMemberIds = string.Empty;

            try
            {
                //UAT 2477
                Int32 rorationID = CurrentViewContext.ClinicalRotationID;
                List<ClinicalRotationDetailContract> lstClinicalRotationDetailContract = Presenter.GetRotationPackageAndAgencyData(rorationID, CurrentViewContext.SelectedTenantId);
                ClinicalRotationDetailContract clinicalRotationDetailContractForApplicantPkg = new ClinicalRotationDetailContract();
                ClinicalRotationDetailContract clinicalRotationDetailContractForInstructorPkg = new ClinicalRotationDetailContract();
                if (!lstClinicalRotationDetailContract.IsNullOrEmpty())
                {
                    String applicantRequirementPackageTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
                    clinicalRotationDetailContractForApplicantPkg = lstClinicalRotationDetailContract.Where(con => con.RequirementPackageTypeCode == applicantRequirementPackageTypeCode).FirstOrDefault();

                    String instructorRequirementPackageTypeCode = RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue();
                    clinicalRotationDetailContractForInstructorPkg = lstClinicalRotationDetailContract.Where(con => con.RequirementPackageTypeCode == instructorRequirementPackageTypeCode).FirstOrDefault();
                }
                Int32 requirementPackageID = AppConsts.NONE;
                String isComplianceRequiredforRotation = String.Empty;
                if (!clinicalRotationDetailContractForApplicantPkg.IsNullOrEmpty() && clinicalRotationDetailContractForApplicantPkg.RequirementPackageID > AppConsts.NONE)
                {
                    requirementPackageID = clinicalRotationDetailContractForApplicantPkg.RequirementPackageID;
                    isComplianceRequiredforRotation = clinicalRotationDetailContractForApplicantPkg.IsComplianceRequiredforRotation;
                }
                Int32 requirementPkgInstructorTypeId = AppConsts.NONE;
                if (!clinicalRotationDetailContractForInstructorPkg.IsNullOrEmpty() && clinicalRotationDetailContractForInstructorPkg.RequirementPackageID > AppConsts.NONE)
                {
                    requirementPkgInstructorTypeId = clinicalRotationDetailContractForInstructorPkg.RequirementPackageID;
                }






                //Int32 requirementPackageID = CurrentViewContext.ClinicalRotationDetails.RequirementPackageID;
                //String isComplianceRequiredforRotation = CurrentViewContext.ClinicalRotationDetails.IsComplianceRequiredforRotation;


                Boolean IsInstructorPreceptorExist = false;
                if (CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => d.Key <= AppConsts.NONE).Any())
                {
                    var IPlist = CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => d.Key <= AppConsts.NONE).Select(r => r.Value.Item2).ToList();
                    List<Int32> lstOrgUserIds = CurrentViewContext.CustomMessageOrgUserIds.Keys.Select(x => Convert.ToInt32(x)).ToList();
                    if (IPlist.Where(d => lstOrgUserIds.Contains(d)).Any())
                        IsInstructorPreceptorExist = true;
                    //  CurrentViewContext.RemovedClinicalRotationMemberIds.Remove(0);
                    // IsInstructorPreceptorExist = true;
                }
                //if (IsInstructorPreceptorExist)
                //{
                //    base.ShowAlertMessage("Please select applicant user(s) only for profile sharing.", MessageType.Information);
                //    return;
                //}
                if ((CurrentViewContext.RemovedClinicalRotationMemberIds.IsNull() || CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => !(d.Key <= AppConsts.NONE)).ToList().Count == AppConsts.NONE)
                    && !IsInstructorPreceptorExist)
                {
                    //base.ShowInfoMessage("Please select atleast one rotation member for profile sharing.");
                    base.ShowInfoMessage("Please select atleast one student/instructor for profile sharing.");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RemoveDisplayNoneFrompnlErrorDiv();", true);
                }
                else if (requirementPackageID == AppConsts.NONE
                    && (CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => !(d.Key <= AppConsts.NONE)).ToList().Count != AppConsts.NONE &&
                    (lstClinicalRotationDetailContract[0].IsComplianceRequiredforRotation == null || lstClinicalRotationDetailContract[0].IsComplianceRequiredforRotation.ToLower() == "yes")))
                {
                    base.ShowInfoMessage("This Agency requires that all students be compliant for their agency specific requirements, please assign an agency package to this rotation.");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RemoveDisplayNoneFrompnlErrorDiv();", true);

                }
                else if (IsInstructorPreceptorExist && requirementPkgInstructorTypeId == AppConsts.NONE)
                {
                    base.ShowInfoMessage("Please assign an Instructor/Preceptor Rotation package before sharing an Instructor/Preceptor.");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RemoveDisplayNoneFrompnlErrorDiv();", true);
                }
                else if (IsInstructorPreceptorExist && requirementPkgInstructorTypeId == AppConsts.NONE && (lstClinicalRotationDetailContract[0].IsComplianceRequiredforInstructorPreceptorRotationPkgs == null || lstClinicalRotationDetailContract[0].IsComplianceRequiredforInstructorPreceptorRotationPkgs.ToLower() == "yes"))
                {
                    base.ShowInfoMessage("This Agency requires that all instructors must be compliant for their agency specific requirements, please assign an instructor package to this rotation.");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RemoveDisplayNoneFrompnlErrorDiv();", true);
                }
                ////Implemented regarding UAT-2544
                //else if (Presenter.IsApprovedStudentProfileSharing())
                //{
                //    base.ShowInfoMessage("Profile of approved applicant cannot be shared.");
                //    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RemoveDisplayNoneFrompnlErrorDiv();", true);

                //}
                //UAT-4130
                else if (Presenter.IsApprovedStudentProfileSharing())
                {
                    base.ShowInfoMessage("Profile of approved applicant cannot be shared.");
                    //base.ShowInfoMessage("Profile of approved applicant cannot be shared without Instructor/preceptor.");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RemoveDisplayNoneFrompnlErrorDiv();", true);

                }
                else
                {
                    //UAT-1799: Addition of check "Ïf Rotation package exists for rotation and any 1 student is Non compliant then do not allow sharing
                    selectedRotationMemberIds = String.Join(",", CurrentViewContext.RemovedClinicalRotationMemberIds.Keys.ToArray());
                    //Presenter.GetRequirementPackageEligibility(selectedRotationMemberIds);
                    Dictionary<Int32, String> AgencysRequirementPkgCompliantReqd = Presenter.IsRequirementPkgCompliantReqd(CurrentViewContext.RotationAgencyIds);
                    if (AgencysRequirementPkgCompliantReqd.Count > 0) //to do 2071
                    {
                        Presenter.GetComplianceStatusOfImmunizationAndRotationPackages(selectedRotationMemberIds);
                    }

                    if (!CurrentViewContext.LstStatusMessages.IsNullOrEmpty())
                    {
                        String message = String.Join("<br/>", CurrentViewContext.LstStatusMessages.Select(x => x.ErrorMessage).ToList());
                        ShowMessage(message, MessageType.Information);
                        return;
                    }
                    //UAT-3977
                    String instructorPreceptorOrgUserIds = String.Empty;// String.Join(",", CurrentViewContext.RemovedClinicalRotationMemberIds.Keys.ToArray());
                    List<Int32> lstInstructorOrgUserIds = new List<Int32>();

                    foreach (Int32 item in CurrentViewContext.RemovedClinicalRotationMemberIds.Keys)
                    {
                        if (item <= AppConsts.NONE)
                        {
                            Tuple<Boolean, Int32> tuple = CurrentViewContext.RemovedClinicalRotationMemberIds[item];
                            if (tuple.Item2 > AppConsts.NONE)
                                lstInstructorOrgUserIds.Add(tuple.Item2);
                        }
                    }
                    if (lstInstructorOrgUserIds.Count > AppConsts.NONE)
                        instructorPreceptorOrgUserIds = String.Join(",", lstInstructorOrgUserIds);

                    if (!instructorPreceptorOrgUserIds.IsNullOrEmpty())
                    {

                        Dictionary<Int32, String> agnecysInstructorPreceptorRequiredPkgCompliantReqd = Presenter.InstructorPreceptorRequiredPkgCompliantReqd(CurrentViewContext.RotationAgencyIds);
                        if (agnecysInstructorPreceptorRequiredPkgCompliantReqd.Count > 0)
                        {
                            Presenter.GetComplianceStatusOfInstructorRotationPackages(instructorPreceptorOrgUserIds);
                        }

                        if (!CurrentViewContext.LstStatusMessages.IsNullOrEmpty())
                        {
                            String message = String.Join("<br/>", CurrentViewContext.LstStatusMessages.Select(x => x.ErrorMessage).ToList());
                            ShowMessage(message, MessageType.Information);
                            return;
                        }
                    }


                    Presenter.FilterApplicantHavingOnlyNonActiveOrExpireOrders(selectedRotationMemberIds);

                    if (!CurrentViewContext.LstStatusMessages.IsNullOrEmpty())
                    {
                        String message = String.Join("<br/>", CurrentViewContext.LstStatusMessages.Select(x => x.ErrorMessage).ToList());
                        ShowMessage(message, MessageType.Information);
                        return;
                    }

                    //if (CurrentViewContext.IsRotationPackageEligibleForSharing == false)
                    //{
                    //    base.ShowInfoMessage("The rotation has non-compliant student(s) associated. So it can not be shared.");
                    //}
                    else
                    {
                        SetSessionValues();
                        Session["RotationMemberIds"] = String.Join(",", CurrentViewContext.RemovedClinicalRotationMemberIds.Keys.ToArray());
                        //UAT-3977
                        Session["InstructorPreceptorOrgUserIds"] = instructorPreceptorOrgUserIds;

                        var _dicQryString = new Dictionary<String, String>();
                        _dicQryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.MANAGE_AGENCY_SHARING_CONTROL},
                                                                    { ProfileSharingQryString.SelectedTenantId , Convert.ToString( CurrentViewContext.SelectedTenantId)},
                                                                    { ProfileSharingQryString.RotationId , Convert.ToString(CurrentViewContext.ClinicalRotationID) },
                                                                    { ProfileSharingQryString.SourceScreen, ProfileSharingScreenSource.ROTATION_DETAILS.GetStringValue()},
                                                                    { ProfileSharingQryString.AgencyId, Convert.ToString(CurrentViewContext.AgencyId) },
                                                                    { ProfileSharingQryString.RotationAgencyIds, Convert.ToString(CurrentViewContext.RotationAgencyIds) },
                                                                    {ProfileSharingQryString.SendNotificationToAdminOnAppReqApproveRejection, Convert.ToString(chkSendNotificationToSchoolAdmin.Checked)},
                                                                    { "ChildHighlightRotationFieldUpdatedByAgencies", Convert.ToString(CurrentViewContext.HighlightRotationFieldUpdatedByAgencies) }
                                                                 };

                        var url = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, _dicQryString.ToEncryptedQueryString());
                        Response.Redirect(url, true);
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

        /// <summary>
        /// Back to Rotation queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGoBack_click(object sender, EventArgs e)
        {
            try
            {

                if (CurrentViewContext.SourceScreen == RotationDetailsScreenSource.MANAGE_ROTATION_BY_AGENCY.GetStringValue())
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                                            {
                                                               { "Child",  AppConsts.MANAGE_ROTATION_BY_AGENCY_CONTROL},
                                                               { "ID", CurrentViewContext.ClinicalRotationID.ToString() },
                                                               {"SelectedTenantId",CurrentViewContext.SelectedTenantId.ToString()},
                                                               {"RebindGrid",AppConsts.YES}
                                                            };
                    String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    if (!Session["SourceScreen"].IsNullOrEmpty())
                    {
                        Session["SourceScreen"] = null;
                    }

                    Response.Redirect(url, true);

                }
                else
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                                            {
                                                               { "Child",  AppConsts.MANAGE_ROTATION_CONTROL},
                                                               { "ID", CurrentViewContext.ClinicalRotationID.ToString() },
                                                               {"SelectedTenantId",CurrentViewContext.SelectedTenantId.ToString()}
                                                            };
                    String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

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

        /// <summary>
        /// Assign requirement package to rotation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAssignPackage_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPackage.SelectedIndex == AppConsts.NONE)
                {
                    base.ShowInfoMessage("Please select a package.");
                }
                else
                {
                    //UAT-2514 :Passed IsNewPackage to AssignPackageToRotation Method, so that different flow runs for New and Old Packages
                    Int32 assignedPackageID = Presenter.AssignPackageToRotation(Convert.ToBoolean(cmbPackage.SelectedItem.Attributes["IsShared"]), Convert.ToBoolean(cmbPackage.SelectedItem.Attributes["IsNewPackage"]));
                    if (assignedPackageID != CurrentViewContext.RequirementPackageID)
                    {
                        GetRotationRequirementPackage();
                        BindRequirementPackages();
                        CurrentViewContext.RequirementPackageID = assignedPackageID;
                        ShowHidePackageControls(false, false);
                    }
                    else
                    {
                        ShowHidePackageControls();
                    }
                    grdRotationMembers.Rebind();
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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
        /// Add new requirement package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAddNewPackage_click(object sender, EventArgs e)
        {
            try
            {
                SetSessionValues();
                var _dicQryString = new Dictionary<String, String>();

                _dicQryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.ADD_REQUIREMENT_PACKAGE_CONTROL },
                                                                    { "SelectedTenantID", Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    { ProfileSharingQryString.RotationAgencyIds,Convert.ToString(CurrentViewContext.RotationAgencyIds)},
                                                                    { "IsFromRotationScreen", "true" },
                                                                     {ProfileSharingQryString.ReqPkgTypeCode,RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue()}
                                                                 };

                var url = String.Format("~/RotationPackages/Default.aspx?ucid={0}&args={1}", _viewType, _dicQryString.ToEncryptedQueryString());
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

        /// <summary>
        /// Edit rotation requirement package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditPackage_click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.RequirementPackageID == AppConsts.NONE)
                {
                    base.ShowInfoMessage("Please select a package to edit.");
                }
                else
                {
                    SetSessionValues(true);
                    var _dicQryString = new Dictionary<String, String>();
                    _dicQryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.MANAGE_REQUIREMENT_PACKAGE_CONTROL },
                                                                    { "SelectedTenantID", Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    { ProfileSharingQryString.RequirementPackageID , Convert.ToString(CurrentViewContext.RequirementPackageID) },
                                                                    { "IsFromRotationScreen", "true" },
                                                                    {ProfileSharingQryString.ReqPkgTypeCode,RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue()}
                                                                 };

                    var url = String.Format("~/RotationPackages/Default.aspx?ucid={0}&args={1}", _viewType, _dicQryString.ToEncryptedQueryString());
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

        protected void btnAssignInstPkg_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbInstPackage.SelectedIndex == AppConsts.NONE)
                {
                    base.ShowInfoMessage("Please select a Instructor/Preceptor package.");
                }
                else
                {

                    Int32 assignedPackageID = Presenter.AssignInstructorPackageToRotation(Convert.ToBoolean(cmbInstPackage.SelectedItem.Attributes["IsShared"]), Convert.ToBoolean(cmbInstPackage.SelectedItem.Attributes["IsNewPackage"]));
                    List<Int32> contactIds = new List<int>();//as no contact ids are present on page.
                    Presenter.CreateRotationSubscrptnForClientContacts(contactIds, assignedPackageID);


                    if (assignedPackageID != CurrentViewContext.InstructorRequirementPackageID)
                    {
                        Presenter.GetMappedInstructorRequirementPackage();
                        BindInstructorRequirementPackages();
                        CurrentViewContext.InstructorRequirementPackageID = assignedPackageID;
                        ShowHideInstructorPackageControls(false, false);
                    }
                    else
                    {
                        ShowHideInstructorPackageControls();
                    }

                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    ShowHideInstructorPackageControls();
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

        protected void btnEditInstPkg_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.InstructorRequirementPackageID == AppConsts.NONE)
                {
                    base.ShowInfoMessage("Please select a package to edit.");
                }
                else
                {
                    SetSessionValues(true);
                    var _dicQryString = new Dictionary<String, String>();
                    _dicQryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.MANAGE_REQUIREMENT_PACKAGE_CONTROL },
                                                                    { "SelectedTenantID", Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    { ProfileSharingQryString.RequirementPackageID , Convert.ToString(CurrentViewContext.InstructorRequirementPackageID) },
                                                                    { "IsFromRotationScreen", "true" },
                                                                    {ProfileSharingQryString.ReqPkgTypeCode,RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue()}
                                                                 };

                    var url = String.Format("~/RotationPackages/Default.aspx?ucid={0}&args={1}", _viewType, _dicQryString.ToEncryptedQueryString());
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

        protected void btnAddInstPkg_Click(object sender, EventArgs e)
        {
            try
            {
                SetSessionValues();
                var _dicQryString = new Dictionary<String, String>();
                _dicQryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.ADD_REQUIREMENT_PACKAGE_CONTROL },
                                                                    { "SelectedTenantID", Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    { ProfileSharingQryString.RotationAgencyIds, CurrentViewContext.RotationAgencyIds },
                                                                    { "IsFromRotationScreen", "true" },
                                                                     {ProfileSharingQryString.ReqPkgTypeCode,RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue()}
                                                                 };

                var url = String.Format("~/RotationPackages/Default.aspx?ucid={0}&args={1}", _viewType, _dicQryString.ToEncryptedQueryString());
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
        #endregion

        #region Dropdown Events
        /// <summary>
        /// Dropdown DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserGroup_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlUserGroup.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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
        /// Dropdown DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbPackage_DataBound(object sender, EventArgs e)
        {
            try
            {
                cmbPackage.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        protected void cmbPackage_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            try
            {
                RequirementPackageContract dataItem = (RequirementPackageContract)e.Item.DataItem;
                e.Item.Attributes.Add("IsShared", dataItem.IsSharedUserPackage.ToString());
                e.Item.Attributes.Add("IsCopied", dataItem.IsCopied.ToString());
                if (dataItem.IsSharedUserPackage || dataItem.IsCopied)
                {
                    e.Item.Style["color"] = "#006E00";
                    e.Item.Style["font-weight"] = "Bold";
                }
                //UAT-2514:Added IsNewPackage Property to Applicant Package Dropdown
                e.Item.Attributes.Add("IsNewPackage", dataItem.IsNewPackage.ToString());
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

        protected void cmbInstPackage_DataBound(object sender, EventArgs e)
        {
            try
            {
                cmbInstPackage.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        protected void cmbInstPackage_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            try
            {
                RequirementPackageContract dataItem = (RequirementPackageContract)e.Item.DataItem;
                e.Item.Attributes.Add("IsShared", dataItem.IsSharedUserPackage.ToString());
                e.Item.Attributes.Add("IsCopied", dataItem.IsCopied.ToString());
                if (dataItem.IsSharedUserPackage || dataItem.IsCopied)
                {
                    e.Item.Style["color"] = "#006E00";
                    e.Item.Style["font-weight"] = "Bold";
                }
                //UAT-2514:Added IsNewPackage Property to Instructor Perceptor Package Dropdown
                e.Item.Attributes.Add("IsNewPackage", dataItem.IsNewPackage.ToString());
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

        protected void cmbInstPackage_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!cmbInstPackage.SelectedValue.IsNullOrEmpty())
                {
                    if (cmbInstPackage.SelectedIndex == AppConsts.NONE || Convert.ToBoolean(cmbInstPackage.SelectedItem.Attributes["IsShared"])
                                                || Convert.ToBoolean(cmbInstPackage.SelectedItem.Attributes["IsCopied"]))
                    {
                        dvEditInstPackage.Visible = false;
                    }
                    else
                    {
                        //UAT-1629 : As a client admin, I should not be able to edit rotation packages - NOT APPLICABLE as per UAT-1784
                        Presenter.IsAdminLoggedIn();

                        #region "UAT-2999"
                        //if (!CurrentViewContext.IsAdminLoggedIn)  
                        //{
                        //    //btnAddInstPkg.Visible = false;
                        //   // dvEditInstPackage.Visible = false;
                        //    ApplyManagePkgGranularPermissions(false);
                        //    ApplyAssignPkgGranularPermissions(false);
                        //}
                        //else
                        //{
                        //    dvEditInstPackage.Visible = true;
                        //}

                        if (!CurrentViewContext.IsAdminLoggedIn)
                        {
                            btnAddInstPkg.Visible = false;
                            dvEditInstPackage.Visible = false;
                        }
                        else
                        {
                            dvEditInstPackage.Visible = true;
                            btnAddInstPkg.Visible = true; //UAT-2999
                        }
                        #endregion
                    }
                    #region UAT-2514
                    Int32 RequirementPackageID = Convert.ToInt32(cmbInstPackage.SelectedValue);
                    String packageName = cmbInstPackage.Text;
                    if (cmbInstPackage.Text != cmbInstPackage.SelectedItem.Text)
                    {
                        RadComboBoxItem item = cmbInstPackage.FindItemByText(packageName);
                        item.Selected = true;
                    }
                    var pkgDetails = CurrentViewContext.lstCombinedInstructorRequirementPackages.Where(f => f.RequirementPackageID == RequirementPackageID).FirstOrDefault();
                    Presenter.IsAdminLoggedIn();
                    if (!pkgDetails.IsNewPackage)
                    {
                        /*UAT-2999*/
                        if (CurrentViewContext.IsAdminLoggedIn)
                        {
                            dvEditInstPackage.Visible = true;
                            btnAddInstPkg.Visible = true;
                        }
                        else
                        {
                            dvEditInstPackage.Visible = false;
                            btnAddInstPkg.Visible = false;
                        }
                        /*End UAT-2999*/
                    }
                    else
                    {
                        if (CurrentViewContext.IsAdminLoggedIn)
                        {
                            dvEditInstPackage.Visible = true;
                            btnAddInstPkg.Visible = true;
                        }
                        else
                        {
                            dvEditInstPackage.Visible = false;
                            btnAddInstPkg.Visible = false;
                        }
                    }
                    #endregion
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

        protected void cmbPackage_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Presenter.IsAdminLoggedIn();
                if (!cmbPackage.SelectedValue.IsNullOrEmpty())
                {
                    if (cmbPackage.SelectedIndex == AppConsts.NONE || Convert.ToBoolean(cmbPackage.SelectedItem.Attributes["IsShared"])
                                                || Convert.ToBoolean(cmbPackage.SelectedItem.Attributes["IsCopied"]))
                    {
                        dvEditPackage.Visible = false;
                        if (CurrentViewContext.IsAdminLoggedIn)
                        {
                            lnkAddNewPackage.Visible = true;
                        }
                        else
                        {
                            lnkAddNewPackage.Visible = false;
                        }
                    }
                    else
                    {
                        if (CurrentViewContext.IsAdminLoggedIn)
                        {
                            dvEditPackage.Visible = true;
                            lnkAddNewPackage.Visible = true;
                        }
                        else
                        {
                            dvEditInstPackage.Visible = false;
                            lnkAddNewPackage.Visible = false;
                        }
                    }
                    #region UAT-2514
                    Int32 RequirementPackageID = Convert.ToInt32(cmbPackage.SelectedValue);
                    String packageName = cmbPackage.Text;
                    if (cmbPackage.Text != cmbPackage.SelectedItem.Text)
                    {
                        RadComboBoxItem item = cmbPackage.FindItemByText(packageName);
                        item.Selected = true;
                    }
                    var pkgDetails = CurrentViewContext.lstCombinedRequirementPackage.Where(f => f.RequirementPackageID == RequirementPackageID && f.RequirementPackageName.Equals(packageName)).FirstOrDefault();
                    if (!pkgDetails.IsNewPackage)
                    {
                        /*UAT-2999 Remove school admin ability to edit agency packages*/
                        if (CurrentViewContext.IsAdminLoggedIn)
                        {
                            dvEditPackage.Visible = true;
                            lnkAddNewPackage.Visible = true;
                        }
                        else
                        {
                            dvEditPackage.Visible = false;
                            lnkAddNewPackage.Visible = false;
                        }
                        /*UAT-2999*/
                    }
                    else
                    {
                        dvEditPackage.Visible = false;
                        lnkAddNewPackage.Visible = false;
                    }
                    #endregion

                    #region "Commented for UAT-2999"
                    //UAT-1629 : As a client admin, I should not be able to edit rotation packages - NOT APPLICABLE as per UAT-1784
                    //Presenter.IsAdminLoggedIn();
                    //if (!CurrentViewContext.IsAdminLoggedIn)
                    //{
                    //    dvEditPackage.Visible = false;
                    //    lnkAddNewPackage.Visible = false;
                    // 
                    //    ApplyManagePkgGranularPermissions(true);
                    //    ApplyAssignPkgGranularPermissions(true);
                    //}
                    #endregion
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

        #region Checkbox Events
        /// <summary>
        /// To add/remove selected items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSelectItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.AssignOrganizationUserIds
                    ;
                Int32 orgUserID = (Int32)dataItem.GetDataKeyValue("OrganizationUserId");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;

                if (isChecked)
                {
                    if (!selectedItems.ContainsKey(orgUserID))
                        selectedItems.Add(orgUserID, isChecked);
                }
                else
                {
                    if (selectedItems != null && selectedItems.ContainsKey(orgUserID))
                        selectedItems.Remove(orgUserID);
                }
                CurrentViewContext.AssignOrganizationUserIds = selectedItems;
                hdnOrganizationUserId.Value = String.Join(", ", selectedItems.Select(x => x.Key));
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
        /// To add/remove selected items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRemoveItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                Dictionary<Int32, Boolean> customMessageUserList = CurrentViewContext.CustomMessageOrgUserIds;
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Dictionary<Int32, Tuple<Boolean, Int32>> selectedItems = CurrentViewContext.RemovedClinicalRotationMemberIds;
                //UAT-2544
                Dictionary<Int32, Boolean> approvedStudentList = CurrentViewContext.ApprovedClinicalRotationMemberIdsToRemove;
                Dictionary<Int32, Boolean> RotationMemberIdsToDrop = CurrentViewContext.ClinicalRotationMemberIdsToDrop;
                Int32 clinicalRotationMemberId = (Int32)dataItem.GetDataKeyValue("ClinicalRotationMemberId");
                //UAT-3977 -- Start
                Int32 rotationMemberRowIndex = (Int32)dataItem.GetDataKeyValue("RotationMemberRowIndex");
                Boolean isInstructor = Convert.ToBoolean(dataItem.GetDataKeyValue("IsInstructor"));
                //End
                Int32 orgUserId = (Int32)dataItem.GetDataKeyValue("RotationMemberDetail.OrganizationUserId");
                //Boolean isStudentApproved = dataItem.GetDataKeyValue("InvitationReviewStatus").IsNullOrEmpty() ? false : (Boolean)dataItem.GetDataKeyValue("InvitationReviewStatus");
                String invitationReviewStatus = dataItem["InvitationReviewStatus"].IsNullOrEmpty() ? String.Empty : dataItem["InvitationReviewStatus"].Text;
                Boolean isStudentShared = false;
                isStudentShared = !dataItem["ShareStatus"].IsNullOrEmpty() && dataItem["ShareStatus"].Text.Equals("Shared") ? true : false;
                Boolean isStudentApproved = false;
                if (!invitationReviewStatus.IsNullOrEmpty() && Convert.ToInt32(invitationReviewStatus) == AppConsts.ONE)
                {
                    isStudentApproved = true;
                }

                isChecked = ((CheckBox)dataItem.FindControl("chkRemoveItem")).Checked;

                //New COde implemented in UAT-3977//
                if (isChecked)
                {

                    selectedItems.Add(rotationMemberRowIndex, new Tuple<Boolean, Int32>(isChecked, orgUserId));
                    //UAT-2544
                    if (!approvedStudentList.ContainsKey(rotationMemberRowIndex))
                        approvedStudentList.Add(rotationMemberRowIndex, isStudentApproved);
                    if (!RotationMemberIdsToDrop.ContainsKey(rotationMemberRowIndex)) //UAT-4460
                        RotationMemberIdsToDrop.Add(rotationMemberRowIndex, isStudentShared); //UAT-4460
                    if (!customMessageUserList.ContainsKey(orgUserId))
                        customMessageUserList.Add(orgUserId, true);
                }
                else
                {
                    if (selectedItems != null && selectedItems.ContainsKey(rotationMemberRowIndex) && rotationMemberRowIndex != AppConsts.NONE)
                        selectedItems.Remove(rotationMemberRowIndex);
                    //UAT-2544
                    if (approvedStudentList != null && approvedStudentList.ContainsKey(rotationMemberRowIndex) && rotationMemberRowIndex != AppConsts.NONE)
                        approvedStudentList.Remove(rotationMemberRowIndex);

                    if (RotationMemberIdsToDrop.IsNullOrEmpty() && RotationMemberIdsToDrop.ContainsKey(rotationMemberRowIndex)) //UAT-4460
                        RotationMemberIdsToDrop.Remove(rotationMemberRowIndex); //UAT-4460

                    if (customMessageUserList != null && customMessageUserList.ContainsKey(orgUserId))
                        customMessageUserList.Remove(orgUserId);
                }
                CurrentViewContext.RemovedClinicalRotationMemberIds = selectedItems;
                CurrentViewContext.ApprovedClinicalRotationMemberIdsToRemove = approvedStudentList;
                CurrentViewContext.CustomMessageOrgUserIds = customMessageUserList;
                CurrentViewContext.ClinicalRotationMemberIdsToDrop = RotationMemberIdsToDrop; //UAT-4460
            }


                //Below code is commented in UAT-3977//

            //    if (isChecked)
            //    {
            //        if (!selectedItems.ContainsKey(clinicalRotationMemberId) || clinicalRotationMemberId <= AppConsts.NONE)
            //        {
            //            if (!selectedItems.ContainsKey(clinicalRotationMemberId))
            //            {
            //                Int32 IPcount = Convert.ToInt32("-" + (selectedItems.Keys.Count() + 1));
            //                if (clinicalRotationMemberId <= AppConsts.NONE)
            //                {
            //                    if (selectedItems.Where(d => d.Key == IPcount).Any())
            //                    {
            //                    Start:
            //                        IPcount = IPcount - 1;
            //                        if (selectedItems.Where(d => d.Key == IPcount).Any())
            //                        {
            //                            goto Start;
            //                        }
            //                    }
            //                    selectedItems.Add(IPcount, new Tuple<Boolean, Int32>(isChecked, orgUserId));
            //                }
            //                else
            //                {
            //                    selectedItems.Add(clinicalRotationMemberId, new Tuple<Boolean, Int32>(isChecked, orgUserId));
            //                }
            //            }
            //        }
            //        //UAT-2544
            //        if (!approvedStudentList.ContainsKey(clinicalRotationMemberId))
            //            approvedStudentList.Add(clinicalRotationMemberId, isStudentApproved);

            //        if (!customMessageUserList.ContainsKey(orgUserId))
            //            customMessageUserList.Add(orgUserId, true);
            //    }
            //    else
            //    {
            //        if (selectedItems != null && selectedItems.ContainsKey(clinicalRotationMemberId) && clinicalRotationMemberId != AppConsts.NONE)
            //            selectedItems.Remove(clinicalRotationMemberId);
            //        //UAT-2544
            //        if (approvedStudentList != null && approvedStudentList.ContainsKey(clinicalRotationMemberId) && clinicalRotationMemberId != AppConsts.NONE)
            //            approvedStudentList.Remove(clinicalRotationMemberId);

            //        if (customMessageUserList != null && customMessageUserList.ContainsKey(orgUserId))
            //            customMessageUserList.Remove(orgUserId);
            //    }
            //    CurrentViewContext.RemovedClinicalRotationMemberIds = selectedItems;
            //    CurrentViewContext.ApprovedClinicalRotationMemberIdsToRemove = approvedStudentList;
            //    CurrentViewContext.CustomMessageOrgUserIds = customMessageUserList;
            //}
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

        #region Private Methods

        private void ShowMessage(String strMessage, MessageType msgType)
        {
            String msgClass = "info";
            switch (msgType)
            {
                case MessageType.Error:
                    msgClass = "error";
                    break;
                case MessageType.Information:
                    msgClass = "info";
                    break;
                case MessageType.SuccessMessage:
                    msgClass = "sucs";
                    break;
                default:
                    msgClass = "info";
                    break;
            }
            hdnErrorMessage.Value = strMessage.ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlertMessageWithTitle"
                                                , "$page.showAlertMessageWithTitle('" + String.Empty + "','" + msgClass + "',true);", true);
        }

        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            BindUserGroups();
            //ShowHidePackageControls(true);
        }

        /// <summary>
        /// To bind User Groups dropdown
        /// </summary>
        private void BindUserGroups()
        {
            Presenter.GetAllUserGroups();
            ddlUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            ddlUserGroup.DataBind();
        }

        /// <summary>
        /// Show/Hide package controls
        /// </summary>
        private void ShowHidePackageControls(Boolean bindPackages = false, Boolean getRotationRequirementPackage = true)
        {
            fsucCmdBarButtons.CancelButton.Enabled = false;
            fsucCmdBarButton.CancelButton.ToolTip = "Click here to go to the Rotation Document screen in order to download the rotation document(s) of rotation members.";

            Presenter.IsAdminLoggedIn();
            // SetAssignPkgGranularPermissions(); //UAT-2999
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            var _userType = GetUserType(user);

            if (getRotationRequirementPackage)
            {
                GetRotationRequirementPackage();
            }

            //if clinical rotation members exist for rotation then show the package only, no buttons
            if (Presenter.IsClinicalRotationMembersExistForRotation()
                && CurrentViewContext.RotationRequirementPackage.RequirementPackageID > AppConsts.NONE)
            {
                //dvShowPackage.Visible = true;
                dvAddUpdatePackage.Visible = false;
                //txtPackageName.Text = CurrentViewContext.RotationRequirementPackage.RequirementPackageName;
                hdnRequirementPackageID.Value = Convert.ToString(CurrentViewContext.RotationRequirementPackage.RequirementPackageID);
                if (_userType == UserType.SUPERADMIN)
                {
                    lblAssignedRotationPkg.Text = CurrentViewContext.RotationRequirementPackage.RequirementPackageName.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationRequirementPackage.RequirementPackageName.HtmlEncode();

                }
                if (_userType == UserType.CLIENTADMIN)
                {
                    lblAssignedRotationPkg.Text = CurrentViewContext.RotationRequirementPackage.RequirementPackageLabel.IsNullOrEmpty() ?
                                                        CurrentViewContext.RotationRequirementPackage.RequirementPackageName.HtmlEncode()
                                                        : CurrentViewContext.RotationRequirementPackage.RequirementPackageLabel.HtmlEncode();
                }
                txtAssignedRotationPkg.Visible = false;
                fsucCmdBarButtons.CancelButton.Enabled = true;
                return;
            }

            dvShowPackage.Visible = false;
            dvAddUpdatePackage.Visible = true;

            //Bind requirement packages conditionally
            if (bindPackages)
                BindRequirementPackages();

            //If Requirement Package is mapped to clinical rotation
            if (CurrentViewContext.RotationRequirementPackage.ClinicalRotationRequirementPackageID > AppConsts.NONE)
            {

                //Show the Add to Rotation button
                fsucCmdBarButton.ClearButton.Style.Clear();
                //UAT 1332 Toshi: Add additional "Assign to Rotation" below the grid and centered. 
                fsucCmdBarButton_AssignRotation.Visible = true;
                if (CurrentViewContext.IsRotationStart)
                {
                    fsucCmdBarButton_AssignRotation.ClearButton.Style.Add("visibility", "hidden");
                    fsucCmdBarButton.ClearButton.Style.Add("visibility", "hidden");
                }

                if (CurrentViewContext.IsEditMode && CurrentViewContext.ReturnedRequirementPackageID > AppConsts.NONE &&
                    CurrentViewContext.ReturnedRequirementPackageID != CurrentViewContext.RotationRequirementPackage.RequirementPackageID)
                {
                    //Save data if Requirement Package ID of new version is created
                    CurrentViewContext.RequirementPackageID = CurrentViewContext.ReturnedRequirementPackageID;
                    if (CurrentViewContext.IsAdminLoggedIn || (!CurrentViewContext.IsAdminLoggedIn && _isAssignPkgFullAccess))
                    {
                        //Need to analyze it, Temp Check in 
                        Presenter.AssignPackageToRotation(false, false);
                        GetRotationRequirementPackage();
                    }
                }
                //UAT-1621
                else if (CurrentViewContext.ReturnedRequirementPackageID > AppConsts.NONE &&
                    CurrentViewContext.ReturnedRequirementPackageID != CurrentViewContext.RotationRequirementPackage.RequirementPackageID)
                {
                    CurrentViewContext.RequirementPackageID = CurrentViewContext.ReturnedRequirementPackageID;
                }
                else
                {
                    CurrentViewContext.RequirementPackageID = CurrentViewContext.RotationRequirementPackage.RequirementPackageID;
                }

                if (Convert.ToBoolean(cmbPackage.SelectedItem.Attributes["IsShared"])
                    || !CurrentViewContext.RotationRequirementPackage.IsActive || Convert.ToBoolean(cmbPackage.SelectedItem.Attributes["IsCopied"]))
                {
                    dvEditPackage.Visible = false;
                    /*UAT-2999 Remove school admin ability to edit agency packages*/
                    if (CurrentViewContext.IsAdminLoggedIn)
                    {
                        lnkAddNewPackage.Visible = true;
                    }
                    else
                    {
                        lnkAddNewPackage.Visible = false;
                    }
                }
                else
                {
                    /*UAT-2999 Remove school admin ability to edit agency packages*/
                    if (CurrentViewContext.IsAdminLoggedIn)
                    {
                        dvEditPackage.Visible = true;
                        lnkAddNewPackage.Visible = true;
                    }
                    else
                    {
                        dvEditPackage.Visible = false;
                        lnkAddNewPackage.Visible = false;
                    }
                    /*UAT-2999*/
                }

                if (CurrentViewContext.RotationRequirementPackage.IsArchived)
                {
                    dvEditPackage.Visible = false;
                    lnkAddNewPackage.Visible = false;
                }
            }
            else
            {
                //else if Requirement Package is not mapped to clinical rotation
                dvEditPackage.Visible = false;
                lnkAddNewPackage.Visible = false;
                ////Hide the Add to Rotation button
                //fsucCmdBarButton.ClearButton.Style.Add("display", "none");
                ////UAT 1332 Toshi: Add additional "Assign to Rotation" below the grid and centered. 
                //fsucCmdBarButton_AssignRotation.Visible = false;

                //UAT-1621
                if (CurrentViewContext.ReturnedRequirementPackageID > AppConsts.NONE &&
                    CurrentViewContext.ReturnedRequirementPackageID != CurrentViewContext.RotationRequirementPackage.RequirementPackageID)
                {
                    CurrentViewContext.RequirementPackageID = CurrentViewContext.ReturnedRequirementPackageID;
                }
            }

            if (CurrentViewContext.RotationRequirementPackage.RequirementPackageID > AppConsts.NONE)
            {
                if (_userType == UserType.SUPERADMIN)
                {
                    txtAssignedRotationPkg.Text = CurrentViewContext.RotationRequirementPackage.RequirementPackageName.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationRequirementPackage.RequirementPackageName.HtmlEncode();

                }
                if (_userType == UserType.CLIENTADMIN)
                {
                    txtAssignedRotationPkg.Text = CurrentViewContext.RotationRequirementPackage.RequirementPackageLabel.IsNullOrEmpty() ?
                                                        CurrentViewContext.RotationRequirementPackage.RequirementPackageName.HtmlEncode()
                                                        : CurrentViewContext.RotationRequirementPackage.RequirementPackageLabel.HtmlEncode();
                }
            }
            else
            {
                txtAssignedRotationPkg.Text = "N/A";
            }

            #region "Commented for UAT-2999"
            //UAT-1629 : As a client admin, I should not be able to edit rotation packages - NOT APPLICABLE as per UAT-1784 
            //if (!CurrentViewContext.IsAdminLoggedIn)
            //{
            //    lnkAddNewPackage.Visible = false;
            //    dvEditPackage.Visible = false;
            //    ApplyManagePkgGranularPermissions(true);
            //    ApplyAssignPkgGranularPermissions(true);
            //}
            #endregion
        }

        /// <summary>
        /// To bind Requirement Package dropdown
        /// </summary>
        private void BindRequirementPackages()
        {
            Presenter.GetRequirementPackages();
            if (CurrentViewContext.RotationRequirementPackage.RequirementPackageID > AppConsts.NONE
              && !CurrentViewContext.RotationRequirementPackage.IsActive)
            {
                var lst = CurrentViewContext.lstCombinedRequirementPackage;
                lst.Add(new RequirementPackageContract
                {
                    RequirementPackageID = CurrentViewContext.RotationRequirementPackage.RequirementPackageID,
                    RequirementPackageName = CurrentViewContext.RotationRequirementPackage.RequirementPackageLabel.IsNullOrEmpty() ?
                                            CurrentViewContext.RotationRequirementPackage.RequirementPackageName
                                            : CurrentViewContext.RotationRequirementPackage.RequirementPackageLabel,
                    IsCopied = CurrentViewContext.RotationRequirementPackage.IsCopied
                });
                CurrentViewContext.lstCombinedRequirementPackage = lst;
            }
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            var _userType = GetUserType(user);
            if (_userType == UserType.SUPERADMIN)
            {
                cmbPackage.DataTextField = "RequirementPackageName";
                cmbPackage.DataValueField = "RequirementPackageID";
                cmbPackage.DataSource = CurrentViewContext.lstCombinedRequirementPackage.OrderBy(col => col.RequirementPackageName);
                cmbPackage.DataBind();
            }
            if (_userType == UserType.CLIENTADMIN)
            {
                cmbInstPackage.DataTextField = "RequirementPackageLabel";
                cmbPackage.DataValueField = "RequirementPackageID";
                cmbPackage.DataSource = CurrentViewContext.lstCombinedRequirementPackage.OrderBy(col => col.RequirementPackageName);
                cmbPackage.DataBind();
            }
        }

        //UAT-3538
        public UserType GetUserType(SysXMembershipUser user)
        {
            if (user.IsApplicant.IsNotNull() && user.IsApplicant)
                return UserType.APPLICANT;
            else if (!user.IsApplicant && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                return UserType.CLIENTADMIN;
            else if (!user.IsApplicant && (user.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()))
                return UserType.THIRDPARTYADMIN;
            else if (user.IsSharedUser.IsNotNull() && user.IsSharedUser)
                return UserType.SHAREDUSER;
            else
                return UserType.SUPERADMIN;
        }

        /// <summary>
        /// Get Clinical Rotation requirement package
        /// </summary>
        private void GetRotationRequirementPackage()
        {
            Presenter.GetRotationRequirementPackage();
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetAddToRotationGridFilters()
        {
            grdAddToRotation.MasterTableView.SortExpressions.Clear();
            grdAddToRotation.CurrentPageIndex = 0;
            grdAddToRotation.MasterTableView.CurrentPageIndex = 0;
            chkSelectAllResults.Checked = false; //UAT-2887
            hdnOrganizationUserId.Value = "";
            CurrentViewContext.lstSelectedUserIDs = string.Empty;
            CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, Boolean>();
            grdAddToRotation.Rebind();
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetRotationMembersGridFilters()
        {
            grdRotationMembers.MasterTableView.SortExpressions.Clear();
            grdRotationMembers.CurrentPageIndex = 0;
            grdRotationMembers.MasterTableView.CurrentPageIndex = 0;
            grdRotationMembers.Rebind();
        }

        /// <summary>
        /// To reset search controls
        /// </summary>
        private void ResetControls()
        {
            CurrentViewContext.VirtualRecordCount = 0;
            ViewState["IsBind"] = null;
            if (ViewState["SelectedApplicants"] != null)
            {
                ViewState["SelectedApplicants"] = null;
            }
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            txtEmail.Text = String.Empty;
            txtSSN.Text = String.Empty;
            CurrentViewContext.SSN = null;
            ddlUserGroup.SelectedIndex = -1;
            ResetAddToRotationGridFilters();
            ucCustomAttributeLoaderSearch.Reset();
            ucCustomAttributeLoaderSearch.TenantId = CurrentViewContext.SelectedTenantId;
            ucCustomAttributeLoaderSearch.ScreenType = "CommonScreen";
            chkSelectAllResults.Checked = false;  //UAT-2887
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues(Boolean isEditMode = false)
        {
            ClinicalRotationSearchContract searchDataContract = new ClinicalRotationSearchContract();
            searchDataContract.IsBackToSearch = CurrentViewContext.IsSearchClicked;
            searchDataContract.ApplicantFirstName = CurrentViewContext.ApplicantFirstName;
            searchDataContract.ApplicantLastName = CurrentViewContext.ApplicantLastName;
            searchDataContract.EmailAddress = CurrentViewContext.EmailAddress;
            searchDataContract.DateOfBirth = CurrentViewContext.DateOfBirth;
            searchDataContract.ApplicantSSN = CurrentViewContext.SSN;
            searchDataContract.FilterUserGroupID = CurrentViewContext.FilterUserGroupId;
            searchDataContract.MatchUserGroupID = CurrentViewContext.MatchUserGroupId;
            searchDataContract.LoggedInUserId = CurrentViewContext.CurrentLoggedInUserId;
            searchDataContract.LoggedInUserTenantId = CurrentViewContext.TenantId;
            searchDataContract.GridCustomPagingArguments = CurrentViewContext.GridCustomPaging;
            searchDataContract.TenantID = CurrentViewContext.SelectedTenantId;
            searchDataContract.ClinicalRotationID = CurrentViewContext.ClinicalRotationID;
            searchDataContract.AgencyID = CurrentViewContext.AgencyId;
            searchDataContract.IsEditMode = isEditMode;
            searchDataContract.SelectedDPMIds = CurrentViewContext.SelectedDPMIds;
            searchDataContract.SelectedNodeLabels = ucCustomAttributeLoaderSearch.nodeLable;
            //UAT-2544
            searchDataContract.IsRotationStart = CurrentViewContext.IsRotationStart;
            searchDataContract.IsEditableByClientAdmin = CurrentViewContext.IsEditableByClientAdmin;
            CurrentViewContext.IsEditableByAgencyUser = searchDataContract.IsEditableByAgencyUser;
            //Session for maintaining control values
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ROTATION_DETAIL_SESSION_KEY, searchDataContract);
        }

        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            ClinicalRotationSearchContract searchDataContract = new ClinicalRotationSearchContract();
            if (Session[AppConsts.ROTATION_DETAIL_SESSION_KEY].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ROTATION_DETAIL_SESSION_KEY) as ClinicalRotationSearchContract;
                CurrentViewContext.IsSearchClicked = searchDataContract.IsBackToSearch;
                CurrentViewContext.ApplicantFirstName = searchDataContract.ApplicantFirstName;
                CurrentViewContext.ApplicantLastName = searchDataContract.ApplicantLastName;
                CurrentViewContext.EmailAddress = searchDataContract.EmailAddress;
                CurrentViewContext.DateOfBirth = searchDataContract.DateOfBirth;
                if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
                {
                    if (!searchDataContract.ApplicantSSN.IsNullOrEmpty())
                    {
                        CurrentViewContext.SSN = searchDataContract.ApplicantSSN.Substring(searchDataContract.ApplicantSSN.Length - AppConsts.FOUR);
                        txtSSN.Text = CurrentViewContext.SSN;
                    }
                }
                else
                {
                    CurrentViewContext.SSN = searchDataContract.ApplicantSSN;
                    txtSSN.Text = CurrentViewContext.SSN;
                }
                CurrentViewContext.FilterUserGroupId = searchDataContract.FilterUserGroupID ?? 0;
                CurrentViewContext.MatchUserGroupId = searchDataContract.MatchUserGroupID ?? 0;
                CurrentViewContext.TenantId = searchDataContract.LoggedInUserTenantId ?? 0;
                CurrentViewContext.GridCustomPaging = searchDataContract.GridCustomPagingArguments;

                //Get and use the following properties if redirected back from requirement package screens
                CurrentViewContext.ClinicalRotationID = searchDataContract.ClinicalRotationID ?? 0;
                CurrentViewContext.AgencyId = searchDataContract.AgencyID ?? 0;
                CurrentViewContext.IsEditMode = searchDataContract.IsEditMode;

                //if (CurrentViewContext.IsFromPackageWizard)
                //{
                //    //CurrentViewContext.SelectedTenantId = searchDataContract.TenantID;
                //    CurrentViewContext.ClinicalRotationID = searchDataContract.ClinicalRotationID ?? 0;
                //    CurrentViewContext.AgencyId = searchDataContract.AgencyID ?? 0;
                //    CurrentViewContext.IsEditMode = searchDataContract.IsEditMode;
                //}

                ucCustomAttributeLoaderSearch.TenantId = CurrentViewContext.SelectedTenantId;
                ucCustomAttributeLoaderSearch.DPM_ID = searchDataContract.SelectedDPMIds;
                ucCustomAttributeLoaderSearch.nodeLable = searchDataContract.SelectedNodeLabels;

                //UAT-2544:
                CurrentViewContext.IsRotationStart = searchDataContract.IsRotationStart;
                CurrentViewContext.IsEditableByClientAdmin = searchDataContract.IsEditableByClientAdmin;
                CurrentViewContext.IsEditableByAgencyUser = searchDataContract.IsEditableByAgencyUser;
                //Rebind grids
                grdRotationMembers.Rebind();
                grdAddToRotation.Rebind();
                //Reset session
                //Session[AppConsts.ROTATION_DETAIL_SESSION_KEY] = null;
            }
        }

        /// <summary>
        /// Sets the properties from the arguments recieved through querystring.
        /// </summary>
        /// <param name="args"></param>
        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
            {
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);
                ucCustomAttributeLoaderSearch.TenantId = CurrentViewContext.SelectedTenantId;
                ucCustomAttributeLoaderSearch.ScreenType = "CommonScreen";
            }
            if (args.ContainsKey("ID"))
            {
                CurrentViewContext.ClinicalRotationID = Convert.ToInt32(args["ID"]);
            }
            if (args.ContainsKey(ProfileSharingQryString.AgencyId))
            {
                CurrentViewContext.AgencyId = Convert.ToInt32(args[ProfileSharingQryString.AgencyId]);
            }
            if (args.ContainsKey(ProfileSharingQryString.RequirementPackageID))
            {
                if (Convert.ToString(args[ProfileSharingQryString.ReqPkgTypeCode]) == RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue())
                    CurrentViewContext.ReturnedRequirementPackageID = Convert.ToInt32(args[ProfileSharingQryString.RequirementPackageID]);
                else
                    CurrentViewContext.ReturnedInstRequirementPackageID = Convert.ToInt32(args[ProfileSharingQryString.RequirementPackageID]);
            }
            if (args.ContainsKey("IsFromPackageWizard"))
            {
                CurrentViewContext.IsFromPackageWizard = Convert.ToBoolean(args["IsFromPackageWizard"]);
            }
            if (Session["SourceScreen"].IsNullOrEmpty())
            {
                if (args.ContainsKey(ProfileSharingQryString.SourceScreen))
                {
                    CurrentViewContext.SourceScreen = Convert.ToString(args[ProfileSharingQryString.SourceScreen]);
                }
                else
                {
                    CurrentViewContext.SourceScreen = string.Empty;
                }
            }
            if (args.ContainsKey(AppConsts.HIGHLIGHT_ROTATION_FIELD_UPDATED_BY_AGENCIES))
            {
                CurrentViewContext.HighlightRotationFieldUpdatedByAgencies = Convert.ToBoolean(args[AppConsts.HIGHLIGHT_ROTATION_FIELD_UPDATED_BY_AGENCIES]);
            }
            //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
            if (args.ContainsKey("IsDisplaySuccessMessage"))
            {
                CurrentViewContext.IsDisplaySuccessMessage = Convert.ToBoolean(args["IsDisplaySuccessMessage"]);
            }

            //UAT:3041
            if (args.ContainsKey(AppConsts.IS_EDITABLE_BY_CLIENT_ADMIN))
            {
                CurrentViewContext.IsEditableByClientAdmin = Convert.ToBoolean(args[AppConsts.IS_EDITABLE_BY_CLIENT_ADMIN]);
            }

            if (args.ContainsKey(AppConsts.IS_EDITABLE_BY_AGENCY_USER))
            {
                CurrentViewContext.IsEditableByAgencyUser = Convert.ToBoolean(args[AppConsts.IS_EDITABLE_BY_AGENCY_USER]);
            }

            //UAT-3121
            if (args.ContainsKey("IsApplicantPkgNotAssignedThroughCloning"))
            {
                CurrentViewContext.IsApplicantPkgNotAssignedThroughCloning = Convert.ToBoolean(args["IsApplicantPkgNotAssignedThroughCloning"]);
            }
            if (args.ContainsKey("IsInstructorPkgNotAssignedThroughCloning"))
            {
                CurrentViewContext.IsInstructorPkgNotAssignedThroughCloning = Convert.ToBoolean(args["IsInstructorPkgNotAssignedThroughCloning"]);
            }
        }

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                grdAddToRotation.MasterTableView.GetColumn("SSN").Visible = false;
                grdRotationMembers.MasterTableView.GetColumn("SSN").Visible = false;
                //Hide Masked column if user does not have permission to view SSN Column.
                grdAddToRotation.MasterTableView.GetColumn("_SSN").Visible = false;
                grdRotationMembers.MasterTableView.GetColumn("_SSN").Visible = false;
            }
        }

        /// <summary>
        /// Apply SSN masking
        /// </summary>
        private void ApplySSNMask()
        {
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            {
                //txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
                txtSSN.Mask = AppConsts.SSN_MASK_FORMAT_ALPHANUMERIC;
            }
        }

        /// <summary>
        /// To bind Instructor/preceptor Requirement Package dropdown
        /// </summary>
        private void BindInstructorRequirementPackages()
        {
            Presenter.GetInstructorRequirementPackages();
            if (CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID > AppConsts.NONE
               && !CurrentViewContext.MappedInstructorRequirementPackage.IsActive)
            {
                var lst = CurrentViewContext.lstCombinedInstructorRequirementPackages;
                lst.Add(new RequirementPackageContract
                {
                    RequirementPackageID = CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID,
                    RequirementPackageName = CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageLabel.IsNullOrEmpty() ?
                                            CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageName
                                            : CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageLabel,
                    IsCopied = CurrentViewContext.MappedInstructorRequirementPackage.IsCopied
                });
                CurrentViewContext.lstCombinedInstructorRequirementPackages = lst;
            }

            if (CurrentViewContext.lstSharedRequirementPackage.Count == AppConsts.ONE) //UAT-3702
            {
                CurrentViewContext.InstPercepRequirementPackageID = CurrentViewContext.lstSharedRequirementPackage.FirstOrDefault().RequirementPackageID;
            }
            cmbInstPackage.DataSource = CurrentViewContext.lstCombinedInstructorRequirementPackages.OrderBy(col => col.RequirementPackageName);
            cmbInstPackage.DataBind();

        }

        /// <summary>
        /// Show/Hide package controls
        /// </summary>
        private void ShowHideInstructorPackageControls(Boolean bindPackages = false, Boolean getRotationRequirementPackage = true)
        {
            Presenter.IsAdminLoggedIn();
            // SetAssignPkgGranularPermissions();
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            var _userType = GetUserType(user);

            if (getRotationRequirementPackage)
            {
                Presenter.GetMappedInstructorRequirementPackage();
            }
            Boolean ifAnyContactIsMapped = Presenter.IfAnyContactIsMappedToRotation();
            if (!ifAnyContactIsMapped)
            {
                dvInstrüctorPkg.Visible = false;
                dv_InstructorPreceptorPkgDetail.Visible = false;
                return;
            }
            ////if clinical rotation members exist for rotation then show the package only, no buttons
            if (CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID > AppConsts.NONE
                && Presenter.IfAnyContactHasEnteredDataForRotation(CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID))
            {
                //dvShowInstPackage.Visible = true;
                dvAddUpdateInstPackage.Visible = false;
                //txtInstPackage.Text = CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageName;

                if (!CurrentViewContext.RotationRequirementPackage.IsNullOrEmpty())
                    hdnInstRequirementPackageID.Value = Convert.ToString(CurrentViewContext.RotationRequirementPackage.RequirementPackageID);

                if (_userType == UserType.SUPERADMIN)
                {
                    lblAssignedInstructorpkg.Text = CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageName.IsNullOrEmpty() ? "-" : CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageName;

                }
                if (_userType == UserType.CLIENTADMIN)
                {
                    lblAssignedInstructorpkg.Text = CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageLabel.IsNullOrEmpty() ?
                                                        CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageName
                                                        : CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageLabel;
                }
                txtAssignedInstructorpkg.Visible = false;
                return;
            }

            dvShowInstPackage.Visible = false;
            dvAddUpdateInstPackage.Visible = true;

            //Bind requirement packages conditionally
            if (bindPackages)
                BindInstructorRequirementPackages();


            //If Requirement Package is mapped to clinical rotation
            if (CurrentViewContext.MappedInstructorRequirementPackage.ClinicalRotationRequirementPackageID > AppConsts.NONE)
            {
                CurrentViewContext.OldInstRequirementPackageID = CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID;
                if (CurrentViewContext.MappedInstructorRequirementPackage.IsActive)
                {
                    if (CurrentViewContext.IsAdminLoggedIn)
                    {
                        dvEditInstPackage.Visible = true;
                        btnAddInstPkg.Visible = true;
                    }
                    else
                    {
                        dvEditInstPackage.Visible = false; //UAT-2999
                        btnAddInstPkg.Visible = false;
                    }
                }
                else
                {
                    dvEditInstPackage.Visible = false;
                }
                if (CurrentViewContext.IsEditMode && CurrentViewContext.ReturnedInstRequirementPackageID > AppConsts.NONE &&
                    CurrentViewContext.ReturnedInstRequirementPackageID != CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID)
                {
                    //Save data if Requirement Package ID of new version is created
                    CurrentViewContext.InstructorRequirementPackageID = CurrentViewContext.ReturnedInstRequirementPackageID;
                    if (CurrentViewContext.IsAdminLoggedIn || (!CurrentViewContext.IsAdminLoggedIn && _isAssignPkgFullAccess))
                    {
                        Presenter.AssignInstructorPackageToRotation(false, false);
                        Presenter.GetMappedInstructorRequirementPackage();
                    }
                }

                //UAT-1621
                else if (CurrentViewContext.ReturnedInstRequirementPackageID > AppConsts.NONE &&
                         CurrentViewContext.ReturnedInstRequirementPackageID != CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID)
                {
                    CurrentViewContext.InstructorRequirementPackageID = CurrentViewContext.ReturnedInstRequirementPackageID;
                }
                else
                {
                    CurrentViewContext.InstPercepRequirementPackageID = CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID; //UAT-3702
                    CurrentViewContext.InstructorRequirementPackageID = CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID;

                }

                if (Convert.ToBoolean(cmbInstPackage.SelectedItem.Attributes["IsShared"])
                 || !CurrentViewContext.MappedInstructorRequirementPackage.IsActive || Convert.ToBoolean(cmbInstPackage.SelectedItem.Attributes["IsCopied"]))
                {
                    dvEditInstPackage.Visible = false;
                }
                else
                {
                    if (CurrentViewContext.IsAdminLoggedIn) //UAT-2999
                    {
                        dvEditInstPackage.Visible = true;
                        btnAddInstPkg.Visible = true;
                    }
                    else
                    {
                        dvEditInstPackage.Visible = false;
                        btnAddInstPkg.Visible = false;
                    }
                }

                if (CurrentViewContext.MappedInstructorRequirementPackage.IsArchived)
                {
                    dvEditInstPackage.Visible = false;
                    btnAddInstPkg.Visible = false;
                }

            }
            else
            {
                //else if Requirement Package is not mapped to clinical rotation
                dvEditInstPackage.Visible = false;
                btnAddInstPkg.Visible = false;
                //UAT-1621
                if (CurrentViewContext.ReturnedInstRequirementPackageID > AppConsts.NONE &&
                         CurrentViewContext.ReturnedInstRequirementPackageID != CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID)
                {
                    CurrentViewContext.InstructorRequirementPackageID = CurrentViewContext.ReturnedInstRequirementPackageID;
                }
            }
            if (CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageID > AppConsts.NONE)
            {
                if (_userType == UserType.SUPERADMIN)
                {
                    txtAssignedInstructorpkg.Text = CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageName.IsNullOrEmpty() ? "-" : CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageName;

                }
                if (_userType == UserType.CLIENTADMIN)
                {
                    txtAssignedInstructorpkg.Text = CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageLabel.IsNullOrEmpty() ?
                                                        CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageName
                                                        : CurrentViewContext.MappedInstructorRequirementPackage.RequirementPackageLabel;
                }
            }
            else
            {
                if (CurrentViewContext.InstPercepRequirementPackageID > AppConsts.NONE) //UAT-3702
                {
                    CurrentViewContext.InstructorRequirementPackageID = CurrentViewContext.InstPercepRequirementPackageID;
                    var requirementPackage = CurrentViewContext.lstCombinedInstructorRequirementPackages.ToList()
                                                    .Where(x => x.RequirementPackageID == CurrentViewContext.InstPercepRequirementPackageID)
                                                    .FirstOrDefault();
                    txtAssignedInstructorpkg.Text = requirementPackage.RequirementPackageLabel.IsNullOrEmpty() ?
                                                        requirementPackage.RequirementPackageName
                                                        : requirementPackage.RequirementPackageLabel;

                    if (CurrentViewContext.IsAdminLoggedIn || (!CurrentViewContext.IsAdminLoggedIn && _isAssignPkgFullAccess))
                    {
                        Int32 assignedPackageID = Presenter.AssignInstructorPackageToRotation(Convert.ToBoolean(cmbInstPackage.SelectedItem.Attributes["IsShared"]), Convert.ToBoolean(cmbInstPackage.SelectedItem.Attributes["IsNewPackage"]));
                        List<Int32> contactIds = new List<int>();//as no contact ids are present on page.
                        Presenter.CreateRotationSubscrptnForClientContacts(contactIds, assignedPackageID);
                    }
                }
                else
                {
                    txtAssignedInstructorpkg.Text = "N/A";
                }
            }

            /*UAT-2999: Remove school admin ability to edit agency packages*/

            //UAT-1629 : As a client admin, I should not be able to edit rotation packages - NOT APPLICABLE as per UAT-1784
            //if (!CurrentViewContext.IsAdminLoggedIn)
            //{
            //    /*UAT-2999*/
            //    dvEditInstPackage.Visible = false;
            //    btnAddInstPkg.Visible = false; 
            //    /*End UAt-2999*/
            //    ApplyManagePkgGranularPermissions(false);
            //    ApplyAssignPkgGranularPermissions(false);
            //}
        }

        /// <summary>
        /// Manage the display of the buttons, based on Granular Permissions of user.
        /// </summary>
        private void ApplyManagePkgGranularPermissions(Boolean isStudentType)
        {
            var _managePkgPermissions = CurrentViewContext.dicGranularPermissions.Where(gp => gp.Key == EnumSystemEntity.MANAGE_ROTATION_PACKAGE.GetStringValue()).FirstOrDefault();

            if (_managePkgPermissions.IsNotNull())
            {
                if (_managePkgPermissions.Value == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue())
                {
                    if (isStudentType)
                    {
                        dvEditPackage.Visible = false;
                        lnkAddNewPackage.Visible = false;
                    }
                    else
                    {
                        dvEditInstPackage.Visible = false;
                        btnAddInstPkg.Visible = false;
                    }
                }
                else if (_managePkgPermissions.Value == EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue())
                {
                    if (isStudentType)
                    {
                        if (CurrentViewContext.IsAdminLoggedIn)//UAT-2999
                        {
                            lnkAddNewPackage.Visible = true;
                        }
                        else
                        {
                            lnkAddNewPackage.Visible = false;
                        }
                    }
                    else
                    {
                        if (CurrentViewContext.IsAdminLoggedIn) //UAT-2999
                        {
                            btnAddInstPkg.Visible = true;
                        }
                        else
                        {
                            btnAddInstPkg.Visible = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Manage the display of the buttons, based on Granular Permissions of user.
        /// </summary>
        private void ApplyAssignPkgGranularPermissions(Boolean isStudentType)
        {
            if (!_isAssignPkgFullAccess)
            {
                if (isStudentType)
                {
                    btnAssignPackage.Visible = false;
                }
                else
                {
                    btnAssignInstPkg.Visible = false;
                }
            }
        }
        /// <summary>
        /// Set the Assign Package permission.
        /// </summary>
        private void SetAssignPkgGranularPermissions()
        {
            var _managePkgPermissions = CurrentViewContext.dicGranularPermissions.Where(gp => gp.Key == EnumSystemEntity.ASSIGN_ROTATION_PACKAGE.GetStringValue()).FirstOrDefault();

            if (_managePkgPermissions.IsNotNull() && _managePkgPermissions.Key.IsNotNull() && _managePkgPermissions.Value.IsNotNull())
            {
                if (_managePkgPermissions.Value == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue())
                {
                    _isAssignPkgFullAccess = false;
                }
            }
        }

        private void ShowHideControlOntheBasisOfPermission()
        {
            Presenter.IsAdminLoggedIn();
            if (!CurrentViewContext.IsAdminLoggedIn && !CurrentViewContext.IsEditableByClientAdmin)
            {
                dvAssignToRotation.Attributes.Add("style", "Display:none");
                fsucCmdBarButtons.SaveButton.Visible = false;
                fsucCmdBarButtons.DisplayButtons = CommandBarButtons.Submit;
                dvAddUpdatePackage.Visible = false;
                dvAddUpdateInstPackage.Visible = false;
            }
        }

        #endregion

        protected void btnScheduleRequirements_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean IsInstructorPreceptorExist = false;
                if (CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => d.Key <= AppConsts.NONE).Any())
                {
                    var IPlist = CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => d.Key <= AppConsts.NONE).Select(r => r.Value.Item2).ToList();
                    List<Int32> lstOrgUserIds = CurrentViewContext.CustomMessageOrgUserIds.Keys.Select(x => Convert.ToInt32(x)).ToList();
                    if (IPlist.Where(d => lstOrgUserIds.Contains(d)).Any())
                        IsInstructorPreceptorExist = true;
                    // CurrentViewContext.RemovedClinicalRotationMemberIds.Remove(0);
                    //IsInstructorPreceptorExist = true;
                }
                if (IsInstructorPreceptorExist)
                {
                    base.ShowAlertMessage("Please select applicant user(s) only to send message.", MessageType.Information);
                    return;
                }
                if (CurrentViewContext.RemovedClinicalRotationMemberIds.IsNotNull() && !CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => !(d.Key <= AppConsts.NONE)).Any())
                {
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to send message.", MessageType.Information);
                    // base.ShowErrorInfoMessage("Please select user(s) to send message.");
                }
                else if (CurrentViewContext.RotationStartDate <= DateTime.Now)
                {
                    base.ShowAlertMessage("Unable to send Notification  because rotation date has been reached.", MessageType.Information);
                }
                else
                {
                    Dictionary<String, Object> data = new Dictionary<String, Object>();
                    data.Add("orgUserIds", String.Join(",", CurrentViewContext.RemovedClinicalRotationMemberIds.Keys));
                    data.Add("tenantId", CurrentViewContext.SelectedTenantId);
                    data.Add("CurentLoggedInUserId", CurrentViewContext.CurrentLoggedInUserId);

                    var loggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var exceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

                    ParallelTaskContext.PerformParallelTask(Presenter.SendScheduleRequirementsNotification, data, loggerService, exceptiomService);

                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Message delivered successfully for selected user(s).", MessageType.SuccessMessage);
                    //base.ShowSuccessMessage("Message delivered successfully for selected user(s).");

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

        protected void btnScheduleRotation_Click(object sender, EventArgs e)
        {
            try
            {

                Boolean IsInstructorPreceptorExist = false;
                if (CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => d.Key <= AppConsts.NONE).Any())
                {
                    var IPlist = CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => d.Key <= AppConsts.NONE).Select(r => r.Value.Item2).ToList();
                    List<Int32> lstOrgUserIds = CurrentViewContext.CustomMessageOrgUserIds.Keys.Select(x => Convert.ToInt32(x)).ToList();
                    if (IPlist.Where(d => lstOrgUserIds.Contains(d)).Any())
                        IsInstructorPreceptorExist = true;
                    // CurrentViewContext.RemovedClinicalRotationMemberIds.Remove(0);
                    //IsInstructorPreceptorExist = true;
                }
                if (IsInstructorPreceptorExist)
                {
                    base.ShowAlertMessage("Please select applicant user(s) only to send message.", MessageType.Information);
                    return;
                }

                if (CurrentViewContext.RemovedClinicalRotationMemberIds.IsNotNull() && !CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => !(d.Key <= AppConsts.NONE)).Any())
                {
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to send message.", MessageType.Information);
                    // base.ShowErrorInfoMessage("Please select user(s) to send message.");
                }
                else if (CurrentViewContext.RotationStartDate <= DateTime.Now)
                {
                    base.ShowAlertMessage("Unable to send Notification  because rotation date has been reached.", MessageType.Information);
                }
                else
                {
                    Dictionary<String, Object> data = new Dictionary<String, Object>();
                    data.Add("orgUserIds", String.Join(",", CurrentViewContext.RemovedClinicalRotationMemberIds.Keys));
                    data.Add("tenantId", CurrentViewContext.SelectedTenantId);
                    data.Add("CurentLoggedInUserId", CurrentViewContext.CurrentLoggedInUserId);

                    var loggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var exceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

                    ParallelTaskContext.PerformParallelTask(Presenter.SendScheduleRotationNotification, data, loggerService, exceptiomService);

                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Message delivered successfully for selected user(s).", MessageType.SuccessMessage);
                    //base.ShowSuccessMessage("Message delivered successfully for selected user(s).");

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

        protected void btnRotationDetailsNotification_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.RemovedClinicalRotationMemberIds.IsNotNull() && CurrentViewContext.RemovedClinicalRotationMemberIds.Count == 0)
                {
                    base.ShowAlertMessage("Please select user(s) to send message.", MessageType.Information);
                }
                else
                {
                    Presenter.GetClinicalRotationMembers();
                    var rotationMemberIDList = CurrentViewContext.RemovedClinicalRotationMemberIds.Select(r => r.Value.Item2).ToList();
                    CurrentViewContext.lstSelectedRotationMembers = CurrentViewContext.RotationMemberDetailList.Where(x => rotationMemberIDList.Contains(x.RotationMemberDetail.OrganizationUserId)).ToList();

                    Dictionary<String, Object> data = new Dictionary<String, Object>();
                    data.Add("rotationDetails", CurrentViewContext.ClinicalRotationDetails);
                    data.Add("tenantId", CurrentViewContext.SelectedTenantId);
                    data.Add("CurentLoggedInUserId", CurrentViewContext.CurrentLoggedInUserId);

                    var loggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var exceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

                    ParallelTaskContext.PerformParallelTask(Presenter.SendRotationDetailsNotificationToAgencyUsers, data, loggerService, exceptiomService);

                    base.ShowAlertMessage("Notification sent to agency user(s) successfully.", MessageType.SuccessMessage);
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

        protected void btnCustomMessage_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.CustomMessageOrgUserIds.IsNotNull() && !CurrentViewContext.CustomMessageOrgUserIds.Any())
                {
                    //UAT-2052 : C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to send message.", MessageType.Information);
                    //base.ShowErrorInfoMessage("Please select user(s) to send message.");
                }
                else
                {
                    Presenter.GetSelectedOrganizatioUserIDs();
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    Session["OrgUsersToList"] = CurrentViewContext.lstSelectedOrgUserIDs;

                    #region UAT-3463
                    Boolean IsInstructorPreceptorExist = false;
                    if (CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => d.Key <= AppConsts.NONE).Any())
                    {
                        var IPlist = CurrentViewContext.RemovedClinicalRotationMemberIds.Where(d => d.Key <= AppConsts.NONE).Select(r => r.Value.Item2).ToList();
                        List<Int32> lstOrgUserIds = CurrentViewContext.CustomMessageOrgUserIds.Keys.Select(x => Convert.ToInt32(x)).ToList();
                        if (IPlist.Where(d => lstOrgUserIds.Contains(d)).Any())
                            IsInstructorPreceptorExist = true;
                    }
                    if (IsInstructorPreceptorExist)
                    {
                        Session["HideCommunicationModeForIP"] = true;
                    }
                    #endregion

                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPopup();", true);
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
        /// UAT-2423: As a client admin, I should be able to see and print the explanatory notes of the categories within a package from the rotation details screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPkgDetail_Click(object sender, EventArgs e)
        {

            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenRequirementPackagePopup('" + CurrentViewContext.ClinicalRotationID + "','" + CurrentViewContext.SelectedTenantId + "','" + true + "');", true);
        }

        /// <summary>
        /// UAT-2887: Add Select all records to assign to rotation screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void chkSelectAllResults_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentViewContext.VirtualRecordCount > 0)
            {
                bool needToCheckboxChecked = false;

                if (((CheckBox)sender).Checked)
                {
                    Presenter.GetAllUserIds();
                    needToCheckboxChecked = true;
                    hdnOrganizationUserId.Value = CurrentViewContext.lstSelectedUserIDs;
                    CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, Boolean>();
                    string[] arrySelectdUsrIds = hdnOrganizationUserId.Value.Split(',');

                    foreach (var UsrId in arrySelectdUsrIds)
                    {
                        CurrentViewContext.AssignOrganizationUserIds.Add(Convert.ToInt32(UsrId), true);
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "AddStudentIdsInArray();", true);
                }
                else
                {
                    CurrentViewContext.lstSelectedUserIDs = string.Empty;
                    hdnOrganizationUserId.Value = string.Empty;
                    CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, Boolean>();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetSelectedUsers();", true);
                }

                foreach (GridDataItem item in grdAddToRotation.Items)
                {
                    CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                    checkBox.Checked = needToCheckboxChecked;
                }

                GridHeaderItem headerItem = grdAddToRotation.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                CheckBox headerCheckBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
                headerCheckBox.Checked = needToCheckboxChecked;
            }

            chkSelectAllResults.Focus();
        }

        #region UAT-3049:- Addition of Document link on the Rotation Member grid

        private String GetDocumentFileName(List<String> categoryNames, Int16 fileCount, String fileExtension)
        {
            StringBuilder sb = new StringBuilder();
            categoryNames.ForEach(catName =>
            {
                String categoryName = ReplaceSpecialCharacters(catName).Trim();
                sb.Append(categoryName);
                sb.Append("_");
            });
            //sb.Append(fileCount);
            if (!sb.IsNullOrEmpty() && sb.Length > 100)
            {
                sb = new StringBuilder();
                sb.Append("MultipleCategories_" + fileCount);
            }
            else if (!sb.IsNullOrEmpty())
            {
                sb.Append(fileCount);
            }
            else
            {
                sb = new StringBuilder();
                sb.Append("Categories_" + fileCount);
            }

            sb.Append(fileExtension);
            return sb.ToString();
        }

        private String ReplaceSpecialCharacters(String strName)
        {
            return Regex.Replace(strName, "[^a-zA-Z0-9]+", " ");
            //Regex.Replace(strName, @"[^\w\d]", "_");
        }

        #endregion

        protected void fsucCmdBarButtons_CancelClick(object sender, EventArgs e)
        {
            try
            {
                SetSessionValues();
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                String rotationID = CurrentViewContext.ClinicalRotationID.ToString();


                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    { "Child",  AppConsts.ROTATION_DOCUMENTS_CONTROL},
                                                                    {"RotationId" , rotationID }, 
                                                                    {ProfileSharingQryString.ControlUseType,AppConsts.ROTATION_DETAIL_USE_TYPE_CODE},
                                                                    {ProfileSharingQryString.AgencyId, CurrentViewContext.AgencyId.ToString()},                                                                    
                                                                    {AppConsts.IS_EDITABLE_BY_CLIENT_ADMIN,Convert.ToString(CurrentViewContext.IsEditableByClientAdmin)},
                                                                    {AppConsts.IS_EDITABLE_BY_AGENCY_USER,Convert.ToString(CurrentViewContext.IsEditableByAgencyUser)},
                                                                    {"HighlightRotationFieldUpdatedByAgencies",AppConsts.TRUE},
                                                                    {"IsApplicantPkgNotAssignedThroughCloning",Convert.ToString(CurrentViewContext.IsApplicantPkgNotAssignedThroughCloning)},//UAT-3121
                                                                    {"IsInstructorPkgNotAssignedThroughCloning",Convert.ToString(CurrentViewContext.IsInstructorPkgNotAssignedThroughCloning)}//UAT-3121
                                                                 };
                string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
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
        /// <summary>
        /// UAT-3351: Add "Rotation Requirement Package Detail" button for Assigned Instructor Packages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_InstructorPreceptorPkgDetail_Click(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenRequirementPackagePopup('" + CurrentViewContext.ClinicalRotationID + "','" + CurrentViewContext.SelectedTenantId + "','" + false + "');", true);
        }

    }
}