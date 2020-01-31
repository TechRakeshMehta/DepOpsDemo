using CoreWeb.ClinicalRotation.UserControl;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RequirementApplicantDetailPanel : BaseUserControl, IRequirementApplicantDetailPanel
    {
        #region Variables

        private RequirementApplicantDetailPanelPresenter _presenter = new RequirementApplicantDetailPanelPresenter();
        private String _viewType;

        #endregion

        #region Properties

        public RequirementApplicantDetailPanelPresenter Presenter
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
        public IRequirementApplicantDetailPanel CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass> IRequirementApplicantDetailPanel.ApplicantComplianceCategoryDataList
        {
            get
            {
                if (ViewState["ApplicantComplianceCategoryDataList"] != null)
                    return (List<INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass>)ViewState["ApplicantComplianceCategoryDataList"];
                return null;
            }
            set
            {
                ViewState["ApplicantComplianceCategoryDataList"] = value;
            }
        }
        Int32 IRequirementApplicantDetailPanel.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        Int32 IRequirementApplicantDetailPanel.SelectedTenantID
        {
            get { return Convert.ToInt32(ViewState["SelectedTenantID"] ?? "0"); }
            set { ViewState["SelectedTenantID"] = value; }
        }
        public Int32 SubTotalPages
        {
            get
            {
                if (Session["SubTotalPages"] != null)
                    return Convert.ToInt32(Session["SubTotalPages"]);
                return 0;
            }
            set
            {
                Session["SubTotalPages"] = value;
            }
        }
        Int32 IRequirementApplicantDetailPanel.RequirementPackageTypeId
        {
            get { return Convert.ToInt32(ViewState["RequirementPackageTypeId"] ?? "0"); }
            set { ViewState["RequirementPackageTypeId"] = value; }
        }
        Int32 IRequirementApplicantDetailPanel.ClinicalRotationID
        {
            get { return Convert.ToInt32(Session["ClinicalRotationID"] ?? "0"); }
            set { Session["ClinicalRotationID"] = value; }
        }
        public Int32 RequirementItemId
        {
            get { return Convert.ToInt32(Session["RequirementItemId"] ?? "0"); }
            set { Session["RequirementItemId"] = value; }
        }
        public Int32 ApplicantRequirementItemId
        {
            get { return Convert.ToInt32(Session["ApplicantRequirementItemId"] ?? "0"); }
            set { Session["ApplicantRequirementItemId"] = value; }
        }

        public Int32 NextReqPackageSubscriptionID
        {
            get { return Convert.ToInt32(Session["NextReqPackageSubscriptionID"] ?? "0"); }
            set { Session["NextReqPackageSubscriptionID"] = value; }
        }
        public Int32 PrevReqPackageSubscriptionID
        {
            get { return Convert.ToInt32(Session["PrevReqPackageSubscriptionID"] ?? "0"); }
            set { Session["PrevReqPackageSubscriptionID"] = value; }
        }
        ClinicalRotationDetailContract IRequirementApplicantDetailPanel.RotationDeatils
        {
            get;
            set;
        }
        OrganizationUserContract IRequirementApplicantDetailPanel.ApplicantData
        {
            get;
            set;
        }
        Int32 IRequirementApplicantDetailPanel.CurrentApplicantID
        {
            get { return Convert.ToInt32(ViewState["CurrentApplicantID"] ?? "0"); }
            set { ViewState["CurrentApplicantID"] = value; }
        }
        Entity.OrganizationUser IRequirementApplicantDetailPanel.OrganizationUserData
        {
            get;
            set;
        }
        Boolean IRequirementApplicantDetailPanel.IsDOBDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDOBDisabled"] ?? false);
            }
            set
            {
                ViewState["IsDOBDisabled"] = value;
            }
        }
        String IRequirementApplicantDetailPanel.SSNPermissionCode
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
        Entity.Tenant IRequirementApplicantDetailPanel.LoggedInUser
        {
            get
            {
                if (ViewState["LoggedInUser"] == null)
                    ViewState["LoggedInUser"] = Presenter.GetTenant(CurrentViewContext.CurrentLoggedInUserId);
                return (Entity.Tenant)ViewState["LoggedInUser"];
            }
        }
        Int32 IRequirementApplicantDetailPanel.SelectedPackageSubscriptionID
        {
            get { return Convert.ToInt32(ViewState["SelectedPackageSubscriptionID"] ?? "0"); }
            set { ViewState["SelectedPackageSubscriptionID"] = value; }
        }
        Int32 IRequirementApplicantDetailPanel.ReqPkgSubsciptionID
        {
            get { return Convert.ToInt32(ViewState["ReqPkgSubsciptionID"] ?? "0"); }
            set { ViewState["ReqPkgSubsciptionID"] = value; }
        }

        Int32 IRequirementApplicantDetailPanel.TenantID
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    return user.TenantId.HasValue ? user.TenantId.Value : AppConsts.NONE;
                }
                return AppConsts.NONE;
            }
        }

        Int32 IRequirementApplicantDetailPanel.CategoryID
        {
            get;
            set;
        }

        List<RequirementVerificationDetailContract> IRequirementApplicantDetailPanel.lstReqPkgSubData
        {
            get;
            set;
        }

        List<ReqPkgSubscriptionIDList> IRequirementApplicantDetailPanel.lstReqPkgsubscriptionIdList
        {
            get;
            set;
        }

        String IRequirementApplicantDetailPanel.ControlUseType
        {
            get
            {
                return Convert.ToString(ViewState["ControlUseType"]);
            }
            set
            {
                ViewState["ControlUseType"] = value;
            }
        }

        Int32 IRequirementApplicantDetailPanel.AffectedItemsCount
        {
            get;
            set;
        }

        public string EntityPermissionName
        {
            get;
            set;
        }


        public Int32 ItemDataId_Global
        {
            get { return Convert.ToInt32(ViewState["ApplicantRequirementItemId"] ?? "0"); }
            set { ViewState["ApplicantRequirementItemId"] = value; }
        }
        public Int32 SubPageIndex
        {
            get
            {
                if (Session["SubPageIndex"] != null)
                    return Convert.ToInt32(Session["SubPageIndex"]);
                return 0;
            }
            set
            {
                Session["SubPageIndex"] = value;
            }
        }
        public Int32 SelectedPackageSubscriptionID_Global
        {
            get { return Convert.ToInt32(ViewState["ReqPkgSubsciptionID"] ?? "0"); }
            set { ViewState["ReqPkgSubsciptionID"] = value; }
        }

        public Int32 PrevAppReqItemID
        {
            get { return Convert.ToInt32(Session["PrevAppReqItemID"] ?? "0"); }
            set { Session["PrevAppReqItemID"] = value; }
        }

        public Int32 NextRotationID
        {
            get { return Convert.ToInt32(Session["NextRotationID"] ?? "0"); }
            set { Session["NextRotationID"] = value; }
        }
        public Int32 PrevRotationID
        {
            get { return Convert.ToInt32(Session["PrevRotationID"] ?? "0"); }
            set { Session["PrevRotationID"] = value; }
        }

        public Int32 NextTenantID
        {
            get { return Convert.ToInt32(Session["NextTenantID"] ?? "0"); }
            set { Session["NextTenantID"] = value; }
        }
        public Int32 PrevTenantID
        {
            get { return Convert.ToInt32(Session["PrevTenantID"] ?? "0"); }
            set { Session["PrevTenantID"] = value; }
        }

        public Int32 NextCategoryID
        {
            get { return Convert.ToInt32(Session["NextCategoryID"] ?? "0"); }
            set { Session["NextCategoryID"] = value; }
        }
        public Int32 PrevCategoryID
        {
            get { return Convert.ToInt32(Session["PrevCategoryID"] ?? "0"); }
            set { Session["PrevCategoryID"] = value; }
        }

        public Int32 NextApplicantID
        {
            get { return Convert.ToInt32(Session["NextApplicantID"] ?? "0"); }
            set { Session["NextApplicantID"] = value; }
        }
        public Int32 PrevApplicantID
        {
            get { return Convert.ToInt32(Session["PrevApplicantID"] ?? "0"); }
            set { Session["PrevApplicantID"] = value; }
        }
        public Int32 NextAppReqItemID
        {
            get { return Convert.ToInt32(Session["NextAppReqItemID"] ?? "0"); }
            set { Session["NextAppReqItemID"] = value; }
        }
        #region Private Properties

        #region UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
        private String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }
        public String PendingItemNames
        {
            get;
            set;
        }
        #endregion
        #endregion
        #endregion

        #region Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
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
            if (!this.IsPostBack)
            {
                Session["CurentPackageSubscriptionID"] = null;
                Session["CurrentSubscriptionIDList"] = null;
                CaptureQuerystringParameters();
                Presenter.OnViewInitialized();
                SetPageDataAndLayout();

                if (!Session[AppConsts.APPROVE_PENDING_REVIEW_ITEMS_SUCCESSMSG].IsNullOrEmpty())
                {
                    hdnSuccMsg.Value = Convert.ToString(Session[AppConsts.APPROVE_PENDING_REVIEW_ITEMS_SUCCESSMSG]);
                    Session[AppConsts.APPROVE_PENDING_REVIEW_ITEMS_SUCCESSMSG] = null;
                    if (!Presenter.IsAdminLoggedIn() && !Session[AppConsts.PENDING_ITEM_NAMES].IsNullOrEmpty())
                    {
                        hdnPendingItems.Value = "Item "+' ' + Convert.ToString(Session[AppConsts.PENDING_ITEM_NAMES]) + " status can not be marked as " + "'" + "Meets Requirement" + "'" + " as View Document attribute value in the item(s) is set to " + "'" + "No" + "'" + ". Please contact Admin for approving the Item(s).";
                        Session[AppConsts.PENDING_ITEM_NAMES] = null;
                    }
                }
                #region UAT 2371
                Presenter.GetSystemEntityUserPermission(CurrentViewContext.CurrentLoggedInUserId, CurrentViewContext.SelectedTenantID);
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                var _userType = GetUserType(user);
                if (_userType == UserType.CLIENTADMIN)
                {
                    if (!String.IsNullOrEmpty(CurrentViewContext.EntityPermissionName) && Convert.ToString(CurrentViewContext.EntityPermissionName).ToUpper() == "NONE")
                    {
                        btnApproveAllPendingReviewItems.Enabled = false;
                    }
                }
                else { btnApproveAllPendingReviewItems.Enabled = true; }

                #endregion
            }
        }

        #endregion

        #endregion

        #region Methods
        #region public methods
        private void BindAttributes()
        {
            Presenter.GetApplicantData();
            Presenter.GetRequirementPackageCategoryData();
            lblAddress.Text = CurrentViewContext.ApplicantData.Address1.HtmlEncode();
            lblAddress.Text += ", " + CurrentViewContext.ApplicantData.City.HtmlEncode();
            lblAddress.Text += ", " + CurrentViewContext.ApplicantData.State.HtmlEncode();
            lblAddress.Text += ", " + CurrentViewContext.ApplicantData.Country.HtmlEncode();
            lblAddress.Text += " " + CurrentViewContext.ApplicantData.ZipCode.HtmlEncode();
            if (CurrentViewContext.ApplicantData.Address1.IsNullOrEmpty() && CurrentViewContext.ApplicantData.City.IsNullOrEmpty() &&
                CurrentViewContext.ApplicantData.State.IsNullOrEmpty() && CurrentViewContext.ApplicantData.Country.IsNullOrEmpty()
                && CurrentViewContext.ApplicantData.ZipCode.IsNullOrEmpty())
            {
                divAddress.Visible = false;
            }
            //UAT-4308
            //if (CurrentViewContext.OrganizationUserData!=null &&  CurrentViewContext.OrganizationUserData.MiddleName.IsNullOrEmpty())
            //{
            //    lblApplicantName.Text = CurrentViewContext.OrganizationUserData.FirstName + " " + CurrentViewContext.OrganizationUserData.LastName;
            //}
            //else
            //{
            //    if (CurrentViewContext.OrganizationUserData != null)
            //    {
            //        lblApplicantName.Text = CurrentViewContext.OrganizationUserData.FirstName + " " + CurrentViewContext.OrganizationUserData.MiddleName + " " + CurrentViewContext.OrganizationUserData.LastName;
            //        divUser.Attributes.Add("style", "vertical-align:middle; height:90%; padding-top:4px;");
            //    }
            //}
            if (!CurrentViewContext.OrganizationUserData.FirstName.IsNullOrEmpty())
            {
                lblApplicantName.Text = CurrentViewContext.OrganizationUserData.FirstName.HtmlEncode();
            }
            if (!CurrentViewContext.OrganizationUserData.MiddleName.IsNullOrEmpty())
            {
                lblApplicantMiddleName.Text = CurrentViewContext.OrganizationUserData.MiddleName.HtmlEncode();
                divUser.Attributes.Add("style", "vertical-align:middle; height:90%; padding-top:4px;");
            }
            if (!CurrentViewContext.OrganizationUserData.LastName.IsNullOrEmpty())
            {
                lblApplicantLastName.Text = CurrentViewContext.OrganizationUserData.LastName.HtmlEncode();
            }
            //END UAT
            lblEmail.Text = CurrentViewContext.OrganizationUserData.PrimaryEmailAddress.HtmlEncode();
            if (!CurrentViewContext.OrganizationUserData.SecondaryEmailAddress.IsNullOrEmpty())
            {
                lblSecondaryEmail.Text = CurrentViewContext.OrganizationUserData.SecondaryEmailAddress.HtmlEncode();
                dvSecondaryEmail.Visible = true;
            }
            else
            {
                dvSecondaryEmail.Visible = false;
            }

            lblPhones.Text = (CurrentViewContext.OrganizationUserData.IsInternationalPhoneNumber ? CurrentViewContext.OrganizationUserData.PhoneNumber.HtmlEncode() : Presenter.GetFormattedPhoneNumber(CurrentViewContext.OrganizationUserData.PhoneNumber).HtmlEncode()) + ", " + (CurrentViewContext.OrganizationUserData.IsInternationalSecondaryPhone ? CurrentViewContext.OrganizationUserData.SecondaryPhone.HtmlEncode() : Presenter.GetFormattedPhoneNumber(CurrentViewContext.OrganizationUserData.SecondaryPhone).HtmlEncode());

            if (CurrentViewContext.OrganizationUserData.PhoneNumber.IsNullOrEmpty()
                && CurrentViewContext.OrganizationUserData.SecondaryPhone.IsNullOrEmpty())
            {
                divPhoneNumber.Visible = false;
            }

            //Creation of granular permissions for Client Admin users
            if (CurrentViewContext.IsDOBDisable)
            {
                lblApplicantDOB.Visible = false;
            }
            else
            {
                lblApplicantDOB.Visible = true;
            }
            lblApplicantDOB.Text = CurrentViewContext.OrganizationUserData.DOB.HasValue ? "(" + CurrentViewContext.OrganizationUserData.DOB.Value.ToShortDateString() + ")" : "";
            lblApplicantDOB.ToolTip = CurrentViewContext.OrganizationUserData.DOB.HasValue ? "DOB: " + CurrentViewContext.OrganizationUserData.DOB.Value.ToString("MMMM d, yyyy") : "";

            String unFromatedSSN = Presenter.GetApplicantSSN();
            if (!unFromatedSSN.IsNullOrEmpty())
            {
                if (Presenter.IsDefaultTenant || CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                {
                    lblSSN.Text = Presenter.GetMaskedSSN(unFromatedSSN).HtmlEncode();
                }
                else if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue())
                {
                    divSSN.Visible = false;
                }
                else
                {
                    lblSSN.Text = Presenter.GetFormattedSSN(unFromatedSSN).HtmlEncode();
                }
            }
            else
            {
                divSSN.Visible = false;
            }
            //Alias of the applicant
            if (CurrentViewContext.OrganizationUserData.PersonAlias.IsNotNull())
            {
                String AliasName = String.Empty;
                //Implemented Changes for middle name related to UAT-2212
                CurrentViewContext.OrganizationUserData.PersonAlias.Where(x => !x.PA_IsDeleted)
                                         .ForEach(x => AliasName += Convert.ToString(x.PA_FirstName + " " + (x.PA_MiddleName.IsNullOrEmpty() ? NoMiddleNameText : x.PA_MiddleName)
                                                                                                         + " " + x.PA_LastName) + ", ");

                if (AliasName.EndsWith(", "))
                    AliasName = AliasName.Substring(0, AliasName.Length - 2);

                if (CurrentViewContext.OrganizationUserData.PersonAlias.Count() > 0 && !AliasName.IsNullOrEmpty())
                {
                    dvAlias.Visible = true;
                    lblAlias.Text = AliasName.HtmlEncode();
                }
            }

            hdnApplicantId.Value = CurrentViewContext.CurrentApplicantID.ToString();
            hdnApplicantName.Value = CurrentViewContext.OrganizationUserData.FirstName + " " + CurrentViewContext.OrganizationUserData.LastName;

            if (!CurrentViewContext.OrganizationUserData.PhotoName.IsNullOrEmpty())
            {
                imgApplicantPhoto.ImageUrl = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?UserId={0}&DocumentType={1}", CurrentViewContext.OrganizationUserData.OrganizationUserID, "ProfilePicture");
            }
            //Setting Name initials
            if (!string.IsNullOrWhiteSpace(CurrentViewContext.OrganizationUserData.FirstName))
            {
                lblNameInitials.Text = CurrentViewContext.OrganizationUserData.FirstName.Substring(0, 1).HtmlEncode();
            }
            if (!string.IsNullOrWhiteSpace(CurrentViewContext.OrganizationUserData.LastName))
            {
                lblNameInitials.Text = lblNameInitials.Text + CurrentViewContext.OrganizationUserData.LastName.Substring(0, 1).HtmlEncode();
            }

            lblSchool.Text = Presenter.TenantName.IsNullOrEmpty() ? "-" : Presenter.TenantName.HtmlEncode();

            if (!CurrentViewContext.RotationDeatils.IsNullOrEmpty())
            {
                lblAgencyName.Text = CurrentViewContext.RotationDeatils.AgencyName.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationDeatils.AgencyName.HtmlEncode();

                lblRotationName.Text = CurrentViewContext.RotationDeatils.RotationName.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationDeatils.RotationName.HtmlEncode();

                lblDepatment.Text = CurrentViewContext.RotationDeatils.Department.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationDeatils.Department.HtmlEncode();

                lblProgram.Text = CurrentViewContext.RotationDeatils.Program.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationDeatils.Program.HtmlEncode();

                lblCourse.Text = CurrentViewContext.RotationDeatils.Course.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationDeatils.Course.HtmlEncode();

                lblTerm.Text = CurrentViewContext.RotationDeatils.Term.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationDeatils.Term.HtmlEncode();

                lblUnitFloor.Text = CurrentViewContext.RotationDeatils.UnitFloorLoc.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationDeatils.UnitFloorLoc.HtmlEncode();

                lblRecommendedHours.Text = CurrentViewContext.RotationDeatils.RecommendedHours.IsNull() ? "-" : Convert.ToString(CurrentViewContext.RotationDeatils.RecommendedHours).HtmlEncode();

                lblDays.Text = CurrentViewContext.RotationDeatils.DaysName.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationDeatils.DaysName.HtmlEncode();

                lblShift.Text = CurrentViewContext.RotationDeatils.Shift.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationDeatils.Shift.HtmlEncode();

                if (CurrentViewContext.RotationDeatils.StartTime.IsNullOrEmpty())
                {
                    lblStartTime.Text = "-";
                }
                else
                {
                    lblStartTime.Text = DateTime.Today.Add(CurrentViewContext.RotationDeatils.StartTime.Value).ToString("hh:mm tt");
                }

                if (CurrentViewContext.RotationDeatils.EndTime.IsNullOrEmpty())
                {
                    lblEndTime.Text = "-";
                }
                else
                {
                    lblEndTime.Text = DateTime.Today.Add(CurrentViewContext.RotationDeatils.EndTime.Value).ToString("hh:mm tt");
                }

                if (CurrentViewContext.RotationDeatils.StartDate.IsNullOrEmpty())
                {
                    lblStartDate.Text = "-";
                }
                else
                {
                    lblStartDate.Text = CurrentViewContext.RotationDeatils.StartDate.Value.ToShortDateString();
                }

                if (CurrentViewContext.RotationDeatils.EndDate.IsNullOrEmpty())
                {
                    lblEndDate.Text = "-";
                }
                else
                {
                    lblEndDate.Text = CurrentViewContext.RotationDeatils.EndDate.Value.ToShortDateString();
                }

                if (!CurrentViewContext.RotationDeatils.StartDate.IsNullOrEmpty()
                        && CurrentViewContext.RotationDeatils.StartDate.HasValue
                        && !CurrentViewContext.RotationDeatils.EndDate.IsNullOrEmpty()
                        && CurrentViewContext.RotationDeatils.EndDate.HasValue
                        && (CurrentViewContext.RotationDeatils.StartDate.Value.Date <= DateTime.Now.Date
                            && DateTime.Now.Date <= CurrentViewContext.RotationDeatils.EndDate.Value.Date
                            )
                    )
                    lblCurrentRotation.Visible = true;
                else
                    lblCurrentRotation.Visible = false;

                lblComplioID.Text = CurrentViewContext.RotationDeatils.ComplioID.IsNullOrEmpty() ? "-" : CurrentViewContext.RotationDeatils.ComplioID;

                var _pkgData = CurrentViewContext.lstReqPkgSubData.FirstOrDefault();
                if (!_pkgData.IsNullOrEmpty())
                {
                    lblStudents.Text = _pkgData.RotationMemberCount.IsNullOrEmpty() ? "-" : _pkgData.RotationMemberCount;
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    var _userType = GetUserType(user);
                    //UAT-3538
                    if (_userType == UserType.SUPERADMIN)
                    {
                        lblPackageName.Text = _pkgData.PkgName.IsNullOrEmpty() ? "-" : _pkgData.PkgName.HtmlEncode();
                    }
                    if (_userType == UserType.CLIENTADMIN)
                    {
                        lblPackageName.Text = _pkgData.PkgLabel.IsNullOrEmpty() ? "-" : _pkgData.PkgLabel.HtmlEncode();
                    }
                    lblRotationComplianceStatus.Text = _pkgData.PkgStatusName;
                    if (_pkgData.PkgStatusCode == RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue())
                    {
                        imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_NON_COMPLIANCE_IMAGE_URL;
                        lblRotationComplianceStatus.ForeColor = System.Drawing.Color.Red;
                        imgPackageComplianceStatus.ToolTip = lblRotationComplianceStatus.Text = _pkgData.PkgStatusName;
                    }
                    else
                    {
                        imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_COMPLIANCE_IMAGE_URL;
                        lblRotationComplianceStatus.ForeColor = System.Drawing.Color.Green;
                        imgPackageComplianceStatus.ToolTip = lblRotationComplianceStatus.Text = _pkgData.PkgStatusName;
                    }
                }
            }

            lstCategories.DataTextField = "CatName";
            lstCategories.DataValueField = "CatId";
            lstCategories.DataSource = CurrentViewContext.lstReqPkgSubData;
            lstCategories.DataBind();
            HiddenField hdnFirstCatagoryID = this.Parent.FindControl("hdnFirstCatagoryID") as HiddenField;
            HiddenField hdnPrevCatagoryID = this.Parent.FindControl("hdnPrevCatagoryID") as HiddenField;
            HiddenField hdnNextCatagoryID = this.Parent.FindControl("hdnNextCatagoryID") as HiddenField;
            HiddenField hdnApplicantReqItemID = this.Parent.FindControl("hdnApplicantReqItemID") as HiddenField; //UAT-4461
            if (hdnFirstCatagoryID.IsNotNull())
            {
                if (hdnFirstCatagoryID.Value == AppConsts.ZERO)
                    hdnFirstCatagoryID.Value = Convert.ToString(CurrentViewContext.lstReqPkgSubData.FirstOrDefault().CatId);

                if (CurrentViewContext.CategoryID > 0)
                    lstCategories.SelectedValue = Convert.ToString(CurrentViewContext.CategoryID);
                else
                    lstCategories.SelectedValue = hdnFirstCatagoryID.Value;

                if (hdnPrevCatagoryID.IsNotNull())
                    if (lstCategories.SelectedIndex > 0)
                        hdnPrevCatagoryID.Value = Convert.ToString(lstCategories.Items[lstCategories.SelectedIndex - 1].Value);

                if (hdnNextCatagoryID.IsNotNull())
                    if (lstCategories.SelectedIndex < (lstCategories.Items.Count - 1))
                        hdnNextCatagoryID.Value = Convert.ToString(lstCategories.Items[lstCategories.SelectedIndex + 1].Value);
                if (hdnApplicantReqItemID.IsNotNull() && hdnApplicantReqItemID.Value == AppConsts.ZERO)
                    hdnApplicantReqItemID.Value = Convert.ToString(CurrentViewContext.ApplicantRequirementItemId);
            }

        }
        private void ManageSubscriptionLinksNavigations()
        {
            RequirementVerificationQueueContract searchDataContract = new RequirementVerificationQueueContract();
            if (Session[AppConsts.ASSIGN_ROTATON_VERIFICATION_QUEUE_SESSION_KEY].IsNotNull() || Session[AppConsts.REQ_VERIFICATION_USER_WORK_QUEUE_SESSION_KEY].IsNotNull()
                || Session[AppConsts.REQ_VERIFICATION_QUEUE_SESSION_KEY].IsNotNull())//UAT-3528
            {

                Boolean isRedirectedFromRequirementVerification = false;
                //Int32 tenantId = AppConsts.NONE;

                if (Session[AppConsts.ASSIGN_ROTATON_VERIFICATION_QUEUE_SESSION_KEY].IsNotNull())
                {
                    CurrentViewContext.ControlUseType = AppConsts.ASSIGN_ROTATION_VERIFICATION_QUEUE_TYPE_CODE;
                    searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ASSIGN_ROTATON_VERIFICATION_QUEUE_SESSION_KEY) as RequirementVerificationQueueContract;

                }
                else if (Session[AppConsts.REQ_VERIFICATION_USER_WORK_QUEUE_SESSION_KEY].IsNotNull())
                {
                    CurrentViewContext.ControlUseType = AppConsts.ROTATION_VERIFICATION_USER_WORK__QUEUE_TYPE_CODE;
                    searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.REQ_VERIFICATION_USER_WORK_QUEUE_SESSION_KEY) as RequirementVerificationQueueContract;

                }
                else if (Session[AppConsts.REQ_VERIFICATION_QUEUE_SESSION_KEY].IsNotNull())
                {
                    isRedirectedFromRequirementVerification = true;
                    CurrentViewContext.ControlUseType = AppConsts.ROTATION_VERIFICATION_QUEUE;
                    searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.REQ_VERIFICATION_QUEUE_SESSION_KEY) as RequirementVerificationQueueContract;

                    //List<String> lstTenant = searchDataContract.SelectedTenantIDs.IsNotNull() ? searchDataContract.SelectedTenantIDs.Split(',').ToList() : new List<String>();
                    //tenantId = Convert.ToInt32(lstTenant[0]);
                }
                if (!searchDataContract.IsNullOrEmpty())
                {
                    searchDataContract.IsRotUserVericationItemDetail = true;
                    //Note:-Submission Date in Rotation Verification Queue is wrong.So, We may have ticket in future to fix - submission date issue then save and next and save and previous navigation should be test and updated as well.
                    SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.MANAGE_REQ_TRI_PANEL_NAVIGATION_SESSION_KEY, Presenter.GetReqPkgSubscriptionIdList(searchDataContract, CurrentViewContext.ReqPkgSubsciptionID, CurrentViewContext.ApplicantRequirementItemId, isRedirectedFromRequirementVerification));
                }

            }
            ManageReqPkgSubscriptionContract navigationData = new ManageReqPkgSubscriptionContract();
            if (Session[AppConsts.REQ_VERIFICATION_USER_WORK_QUEUE_SESSION_KEY].IsNotNull())
            {
                searchDataContract.IsRotUserVericationItemDetail = false;
                navigationData = Presenter.GetReqPkgSubscriptionIdList(searchDataContract, CurrentViewContext.ReqPkgSubsciptionID, CurrentViewContext.ApplicantRequirementItemId, false);
            }
            else
                navigationData = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.MANAGE_REQ_TRI_PANEL_NAVIGATION_SESSION_KEY) as ManageReqPkgSubscriptionContract;
            if (!navigationData.IsNullOrEmpty())
            {
                if (navigationData.CurrentSubscription.IsNullOrEmpty() || (!navigationData.CurrentSubscription.IsNullOrEmpty() && navigationData.CurrentSubscription.NextSubscriptionID == AppConsts.NONE))
                {
                    btnNextApp.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                    lnkBtnNextApp.Disabled = true;
                }
                else
                {
                    btnNextApp.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrow.png";
                    lnkBtnNextApp.Disabled = false;
                }
                if (navigationData.CurrentSubscription.IsNullOrEmpty() || (!navigationData.CurrentSubscription.IsNullOrEmpty() && navigationData.CurrentSubscription.PrevSubscriptionID == AppConsts.NONE))
                {
                    btnPrevApp.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                    lnkBtnPrevApp.Disabled = true;
                }
                else
                {
                    btnPrevApp.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrow-blue.png";
                    lnkBtnPrevApp.Disabled = false;
                }
                if (CurrentViewContext.ControlUseType == null)
                {
                    btnNextApp.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                    btnPrevApp.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                    lnkBtnNextApp.Disabled = true;
                    lnkBtnPrevApp.Disabled = true;
                }
                if (!CurrentViewContext.ControlUseType.IsNullOrEmpty() && !CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_USER_WORK__QUEUE_TYPE_CODE.ToLower())
                    && !CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ASSIGN_ROTATION_VERIFICATION_QUEUE_TYPE_CODE.ToLower()))
                {
                    btnNextApp.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                    btnPrevApp.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                    lnkBtnNextApp.Disabled = true;
                    lnkBtnPrevApp.Disabled = true;
                }
                if (!navigationData.CurrentSubscription.IsNullOrEmpty() && !navigationData.CurrentSubscription.PrevSubscriptionID.IsNullOrEmpty() && navigationData.CurrentSubscription.PrevSubscriptionID > AppConsts.NONE)
                {
                    Dictionary<String, String> prevQueryString = new Dictionary<String, String>();
                    String prevTenantID = navigationData.CurrentSubscription.PrevTenantID.ToString();
                    String prevrotationID = navigationData.CurrentSubscription.PrevClinicalRotationID.ToString();
                    String prevReqPkgSubscriptionId = navigationData.CurrentSubscription.PrevSubscriptionID.ToString();
                    String prevRequirementItemId = navigationData.CurrentSubscription.PrevRequirementItemId.ToString();
                    String prevApplicantRequirementItemId = navigationData.CurrentSubscription.PrevApplicantRequirementItemId.ToString();
                    String prevApplicantID = navigationData.CurrentSubscription.PrevOrganizationUserID.ToString();
                    String prevReqCategoryId = navigationData.CurrentSubscription.PrevRequirementCategoryID.ToString();
                    String prevRequirementPackageTypeId = CurrentViewContext.RequirementPackageTypeId.ToString();
                    String prevAgencyId = navigationData.CurrentSubscription.PrevAgencyId.ToString();
                    prevQueryString = new Dictionary<String, String>
                                                                 {
                                                                    { ProfileSharingQryString.SelectedTenantId, prevTenantID },
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId, prevReqPkgSubscriptionId },
                                                                    { ProfileSharingQryString.RotationId, prevrotationID },
                                                                    { ProfileSharingQryString.ApplicantId , prevApplicantID },
                                                                    { ProfileSharingQryString.ControlUseType,CurrentViewContext.ControlUseType},
                                                                    { ProfileSharingQryString.AgencyId, prevAgencyId},
                                                                    {"RequirementPackageTypeId" , prevRequirementPackageTypeId},
                                                                    {"RequirementItemId",prevRequirementItemId },
                                                                    {"ApplicantRequirementItemId", prevApplicantRequirementItemId },
                                                                    {"SelectedReqComplianceCategoryId", prevReqCategoryId},
                                                                 };




                    if (lnkBtnPrevApp.Disabled)
                        lnkBtnPrevApp.HRef = String.Empty;
                    else
                        lnkBtnPrevApp.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, prevQueryString.ToEncryptedQueryString());
                }

                //hdnPreviousSubscriptionURLApplicant.Value = lnkBtnPrevApp.HRef;

                //if (this.NextReqPackageSubscriptionID == AppConsts.MINUS_ONE)
                //{
                //    this.SubPageIndex++;
                //    this.SelectedPackageSubscriptionID_Global = AppConsts.MINUS_ONE;
                //}
                //else
                //{
                //    // this.SelectedPackageSubscriptionID_Global = this.NextReqPackageSubscriptionID;
                //    //  this.ItemDataId_Global = this.NextAppReqItemID;
                //}

                //if (CurrentViewContext.NextReqPackageSubscriptionID == AppConsts.NONE)
                //{
                //    lnkBtnNextApp.HRef = String.Empty;
                //}

                if (!navigationData.CurrentSubscription.IsNullOrEmpty() && !navigationData.CurrentSubscription.NextSubscriptionID.IsNullOrEmpty() && navigationData.CurrentSubscription.NextSubscriptionID > AppConsts.NONE)
                {
                    Dictionary<String, String> nextQueryString = new Dictionary<String, String>();
                    String nextTenantID = navigationData.CurrentSubscription.NextTenantID.ToString();
                    String nextrotationID = navigationData.CurrentSubscription.NextClinicalRotationID.ToString();
                    String nextReqPkgSubscriptionId = navigationData.CurrentSubscription.NextSubscriptionID.ToString();
                    String nextRequirementItemId = navigationData.CurrentSubscription.NextRequirementItemId.ToString();
                    String nextApplicantRequirementItemId = navigationData.CurrentSubscription.NextApplicantRequirementItemId.ToString();
                    String nextApplicantID = navigationData.CurrentSubscription.NextOrganizationUserID.ToString();
                    String nextReqCategoryId = navigationData.CurrentSubscription.NextRequirementCategoryID.ToString();
                    String nextRequirementPackageTypeId = CurrentViewContext.RequirementPackageTypeId.ToString();
                    String nextAgencyId = navigationData.CurrentSubscription.NextAgencyId.ToString();
                    nextQueryString = new Dictionary<String, String>
                                                                 {
                                                                    { ProfileSharingQryString.SelectedTenantId, nextTenantID },
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId,nextReqPkgSubscriptionId },
                                                                    { ProfileSharingQryString.RotationId, nextrotationID },
                                                                    { ProfileSharingQryString.ApplicantId , nextApplicantID },
                                                                    { ProfileSharingQryString.ControlUseType,CurrentViewContext.ControlUseType},
                                                                    {ProfileSharingQryString.AgencyId, nextAgencyId},
                                                                    { "PackageTypeId" , nextRequirementPackageTypeId},
                                                                    {"RequirementItemId", nextRequirementItemId },
                                                                    {"ApplicantRequirementItemId", nextApplicantRequirementItemId },
                                                                    {"SelectedReqComplianceCategoryId", nextReqCategoryId},

                                                                 };



                    if (lnkBtnNextApp.Disabled)
                        lnkBtnNextApp.HRef = String.Empty;
                    else
                        lnkBtnNextApp.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, nextQueryString.ToEncryptedQueryString());
                }

            }
            else
            {

                btnNextApp.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                btnPrevApp.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                lnkBtnNextApp.Disabled = true;
                lnkBtnPrevApp.Disabled = true;
                lnkBtnNextApp.HRef = String.Empty;
                lnkBtnPrevApp.HRef = String.Empty;
            }

            //hdnNextSubscriptionURLApplicant.Value = lnkBtnNextApp.HRef;

        }
        public void SetPageDataAndLayout()
        {
            BindAttributes();
            ManageSubscriptionLinksNavigations();
        }
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
        #endregion

        #region private method
        /// <summary>
        /// capture query string parameters
        /// </summary>
        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
                {
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);
                }
                if (args.ContainsKey(ProfileSharingQryString.RotationId))
                {
                    CurrentViewContext.ClinicalRotationID = Convert.ToInt32(args[ProfileSharingQryString.RotationId]);
                }
                if (args.ContainsKey(ProfileSharingQryString.ApplicantId))
                {
                    CurrentViewContext.CurrentApplicantID = Convert.ToInt32(args[ProfileSharingQryString.ApplicantId]);
                }
                if (args.ContainsKey(ProfileSharingQryString.ReqPkgSubscriptionId))
                {
                    CurrentViewContext.ReqPkgSubsciptionID = Convert.ToInt32(args[ProfileSharingQryString.ReqPkgSubscriptionId]);
                }
                if (args.ContainsKey("RequirementItemId"))
                {
                    CurrentViewContext.RequirementItemId = Convert.ToInt32(args["RequirementItemId"]);
                }
                if (args.ContainsKey("ApplicantRequirementItemId"))
                {
                    CurrentViewContext.ApplicantRequirementItemId = Convert.ToInt32(args["ApplicantRequirementItemId"]);
                }
                if (args.ContainsKey("CategoryID"))
                {
                    CurrentViewContext.CategoryID = Convert.ToInt32(args["CategoryID"]);
                }
                if (args.ContainsKey("PackageTypeId"))
                {
                    CurrentViewContext.RequirementPackageTypeId = Convert.ToInt32(args["PackageTypeId"]);
                }
                if (args.ContainsKey(ProfileSharingQryString.ControlUseType))
                {
                    CurrentViewContext.ControlUseType = Convert.ToString(args[ProfileSharingQryString.ControlUseType]);
                }
            }
        }

        protected string GetImageUrl(string status, Boolean isComplianceRequired, String CategoryRuleStatusID)
        {
            string url = "";
            if (status == RequirementCategoryStatus.APPROVED.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/yes16.png");
            }
            if (status == RequirementCategoryStatus.INCOMPLETE.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/no16.png");
            }
            if (status == RequirementCategoryStatus.PENDING_REVIEW.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/attn16.png");
            }
            //UAT-2165: Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            if (!isComplianceRequired)
            {
                //UAT 3106
                if (CategoryRuleStatusID.Trim() == AppConsts.STR_ONE)
                {
                    url = ResolveUrl("~/Resources/Mod/Compliance/icons/yes16.png");
                }
                else
                {
                    url = ResolveUrl("~/Resources/Mod/Compliance/icons/optional.png");
                }
            }
            return url;
        }
        //protected string GetStatus(string status, string categoryStatusName)
        //{
        //    string catStatus = categoryStatusName;
        //    if (status == RequirementCategoryStatus.INCOMPLETE.GetStringValue())
        //    {
        //        catStatus = "Pending Review";
        //    }
        //    return catStatus;
        //}
        #endregion
        #endregion

        protected void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenField hdnPrevCatagoryID = this.Parent.FindControl("hdnPrevCatagoryID") as HiddenField;
            HiddenField hdnNextCatagoryID = this.Parent.FindControl("hdnNextCatagoryID") as HiddenField;
            Int32 selectedCategoryID = Convert.ToInt32(lstCategories.SelectedValue);

            //To set the prev category for navigation buttons on middle panel
            if (lstCategories.SelectedIndex > 0)
            {
                if (hdnPrevCatagoryID.IsNotNull())
                {
                    hdnPrevCatagoryID.Value = Convert.ToString(lstCategories.Items[lstCategories.SelectedIndex - 1].Value);
                }
            }
            else
            {
                hdnPrevCatagoryID.Value = AppConsts.ZERO;
            }

            //To set the next category for navigation buttons on middle panel
            if (lstCategories.SelectedIndex < (lstCategories.Items.Count - 1))
            {
                if (hdnNextCatagoryID.IsNotNull())
                {
                    hdnNextCatagoryID.Value = Convert.ToString(lstCategories.Items[lstCategories.SelectedIndex + 1].Value);
                }
            }
            else
            {
                hdnNextCatagoryID.Value = AppConsts.ZERO;
            }
        }

        protected void lstCategories_DataBound(object sender, RadListBoxItemEventArgs e)
        {
            RadListBoxItem item = e.Item as RadListBoxItem;
            //INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass ccpc = (INTSOF.Utils.CommonPocoClasses.)item.DataItem;
            HtmlAnchor lnkCategoriesNavigation = (HtmlAnchor)e.Item.FindControl("lnkCategoriesNavigation");
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            HiddenField hdnPageType = this.Parent.FindControl("hdnPageType") as HiddenField;

            queryString = new Dictionary<String, String>
                        {
                            { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.SelectedTenantID) },
                            // { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL_CONTROL},
                            { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                            { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(CurrentViewContext.ReqPkgSubsciptionID) },
                            { ProfileSharingQryString.RotationId, Convert.ToString(CurrentViewContext.ClinicalRotationID) },
                            { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.OrganizationUserData.OrganizationUserID) },
                            {"CategoryID", Convert.ToString(item.Value) },
                            {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                            {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty}

                        };
            lnkCategoriesNavigation.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            lnkCategoriesNavigation.Attributes.Add("CategoryID", Convert.ToString(item.Value));
        }

        protected void btnApproveAllPendingReviewItems_Click(object sender, EventArgs e)
        {
            try
            {
                //UAt-2366
                Dictionary<Boolean, String> saveResponse = Presenter.ApproveAllPendingItems();
                Boolean isSuccess = !saveResponse.Keys.FirstOrDefault(); ;
                String errorMsg = saveResponse.Values.FirstOrDefault();
                if (isSuccess)
                {
                    String affectedCount = CurrentViewContext.AffectedItemsCount.IsNullOrEmpty() ? AppConsts.ZERO : Convert.ToString(CurrentViewContext.AffectedItemsCount);
                    Session[AppConsts.APPROVE_PENDING_REVIEW_ITEMS_SUCCESSMSG] = string.Concat(affectedCount, " pending item(s) marked as a meets requirements successfully.");
                    if (!Presenter.IsAdminLoggedIn() && !CurrentViewContext.PendingItemNames.IsNullOrEmpty())
                    {
                        Session[AppConsts.PENDING_ITEM_NAMES] = CurrentViewContext.PendingItemNames;
                    }
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    base.ShowSearchErrorMessage(errorMsg);
                    EnableDisableAllValidations();
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
        /// Enable/Disable all validations
        /// </summary>
        private void EnableDisableAllValidations()
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "EnableDisableAllValidations();", true);
        }
        protected string GetComplianceRequiredDateSet(bool isComplianceReq, String complianceStartDate, String complianceEndDate)
        {
            String complianceDateSet = String.Empty;
            String startDate = String.Empty;
            String endDate = String.Empty;
            String complianceReq = "(Compliance " + Convert.ToString(isComplianceReq == true ? "" : "not ") + "required: ";

            if (isComplianceReq)
            {
                if (!complianceStartDate.IsNullOrEmpty() && !complianceEndDate.IsNullOrEmpty())
                {
                    complianceReq = "(Compliance not required: ";
                    DateTime dtStart = Convert.ToDateTime(complianceEndDate).AddDays(1);
                    DateTime dtEnd = (Convert.ToDateTime(complianceStartDate)).AddDays(-1);

                    startDate = Convert.ToString(complianceStartDate) == String.Empty ? String.Empty : Convert.ToString(dtStart.Month + "/" + dtStart.Day);
                    endDate = Convert.ToString(complianceEndDate) == String.Empty ? String.Empty : Convert.ToString(dtEnd.Month + "/" + dtEnd.Day);
                }
            }
            else
            {
                startDate = Convert.ToString(complianceStartDate) == String.Empty ? String.Empty : Convert.ToString(Convert.ToDateTime(complianceStartDate).Month + "/" + Convert.ToDateTime(complianceStartDate).Day);
                endDate = Convert.ToString(complianceEndDate) == String.Empty ? String.Empty : Convert.ToString(Convert.ToDateTime(complianceEndDate).Month + "/" + Convert.ToDateTime(complianceEndDate).Day);
            }

            if (!startDate.IsNullOrEmpty() && !endDate.IsNullOrEmpty())
            {
                complianceDateSet = complianceReq + startDate + "-" + endDate + ")";
            }
            else
            {
                //complianceDateSet = complianceReq + ")";
                complianceDateSet = String.Empty;
            }

            return complianceDateSet;
        }


    }
}