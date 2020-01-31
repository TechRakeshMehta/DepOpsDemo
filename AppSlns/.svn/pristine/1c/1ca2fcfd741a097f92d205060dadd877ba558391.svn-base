using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ReconcilicationApplicantPanel : BaseUserControl, IReconciliationApplicantPanelView
    {
        #region Variables

        private ReconciliationApplicantPanelPresenter _presenter = new ReconciliationApplicantPanelPresenter();
        private System.Delegate _ReLoadDataItemPanel;

        private String _viewType;
        private CustomPagingArgsContract _verificationGridCustomPaging = null;
        private CustomPagingArgsContract _exceptionGridCustomPaging = null;

        #endregion

        #region Properties
        public System.Delegate ReLoadDataItemPanel
        {
            set { _ReLoadDataItemPanel = value; }
        }

        public ReconciliationApplicantPanelPresenter Presenter
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

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>

        public IReconciliationApplicantPanelView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Get or Set the Tenant ID
        /// </summary>

        public Entity.Tenant Tenant
        {
            get
            {
                if (ViewState["Tenant"] == null)
                    ViewState["Tenant"] = Presenter.GetTenant(this.TenantId_Global);
                return (Entity.Tenant)ViewState["Tenant"];
            }
        }
        public Int32 CurrentLoggedInUserId
        {

            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        public Entity.Tenant LoggedInUser
        {
            get
            {
                if (ViewState["LoggedInUser"] == null)
                    ViewState["LoggedInUser"] = Presenter.GetTenant(this.CurrentLoggedInUserId);
                return (Entity.Tenant)ViewState["LoggedInUser"];
            }
        }

        public Entity.OrganizationUser OrganizationUserData
        {
            get
            {
                if (ViewState["OrganizationUserData"] != null)
                    return (Entity.OrganizationUser)ViewState["OrganizationUserData"];
                return null;
            }
            set
            {
                ViewState["OrganizationUserData"] = value;
            }
        }
        #region UAT-749:WB: Addition of "User Groups" to left panel of Verification Details screen
        public List<Entity.ClientEntity.UserGroup> UserGroupDataList
        {
            get
            {
                if (ViewState["UserGroupDataList"] != null)
                    return (List<Entity.ClientEntity.UserGroup>)ViewState["UserGroupDataList"];
                return null;
            }
            set
            {
                ViewState["UserGroupDataList"] = value;
            }
        }
        #endregion

        public String OrganizationUserName
        {
            get { return (String)(ViewState["OrganizationUserName"] ?? ""); }
            set { ViewState["OrganizationUserName"] = value; }
        }


        public string UIInputException { get; set; }

        public Int32 SelectedOrderId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedOrderId"] ?? "0");
            }
            set
            {
                ViewState["SelectedOrderId"] = value;
            }
        }
        /// <summary>
        /// set package id for return back to queue.
        /// </summary>
        public Int32 PackageId
        {
            get
            {
                return Convert.ToInt32(ViewState["PackageId"] ?? "0");
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }

        /// <summary>
        /// set Category id for return back to queue.
        /// </summary>
        public Int32 CategoryId
        {
            get
            {
                return Convert.ToInt32(ViewState["CategoryId"] ?? "0");
            }
            set
            {
                ViewState["CategoryId"] = value;
            }
        }

        public List<ApplicantDocuments> lstApplicantDocument
        {
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract VerificationGridCustomPaging
        {
            get
            {
                if (_verificationGridCustomPaging.IsNull())
                {
                    //var serializer = new XmlSerializer(typeof(CustomPagingArgsContract));
                    if (!String.IsNullOrEmpty(Convert.ToString(Session[AppConsts.VERIFICATION_QUEUE_SESSION_KEY])))
                    {
                        //TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.VERIFICATION_QUEUE_SESSION_KEY]));
                        _verificationGridCustomPaging = (CustomPagingArgsContract)(Session[AppConsts.VERIFICATION_QUEUE_SESSION_KEY]);
                    }
                }
                return _verificationGridCustomPaging;
            }
        }

        public String CurrentCompliancePackageStatus
        {
            get { return (String)(ViewState["CurrentCompliancePackageStatus"] ?? ""); }
            set { ViewState["CurrentCompliancePackageStatus"] = value; }
        }

        public String CurrentCompliancePackageStatusCode
        {
            get { return (String)(ViewState["CurrentCompliancePackageStatusCode"] ?? ""); }
            set { ViewState["CurrentCompliancePackageStatusCode"] = value; }
        }

        public String CurrentPackageBredCrum
        {
            get { return (String)(ViewState["CurrentPackageBredCrum"] ?? ""); }
            set { ViewState["CurrentPackageBredCrum"] = value; }
        }


        /// <summary>
        /// Data of the Applicant.
        /// </summary>
        OrganizationUserContract IReconciliationApplicantPanelView.ApplicantData
        {
            get;
            set;
        }


        DateTime? IReconciliationApplicantPanelView.OrderApprovalDate
        {
            get
            {
                return Convert.ToDateTime(ViewState["OrderApprovalDate"]);
            }
            set
            {
                ViewState["OrderApprovalDate"] = value;
            }
        }

        DateTime? IReconciliationApplicantPanelView.SubscriptionExpirationDate
        {
            get;
            set;
        }
        public Boolean IsRushOrder
        {
            get
            {
                if (!ViewState["IsRushOrder"].IsNull())
                {
                    return (Boolean)ViewState["IsRushOrder"];
                }
                return false;
            }
            set
            {
                ViewState["IsRushOrder"] = value;
            }
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
        #endregion
        #endregion
        #endregion

        #region "Global Properties"

        public Int32 TenantId_Global
        {
            get { return Convert.ToInt32(ViewState["TenantId"] ?? "0"); }
            set { ViewState["TenantId"] = value; }
        }

        /// <summary>
        ///get and  set Applicant id 
        /// </summary>
        public Int32 CurrentApplicantId_Global
        {
            get { return Convert.ToInt32(ViewState["CurrentApplicantId"] ?? "0"); }
            set { ViewState["CurrentApplicantId"] = value; }
        }

        /// <summary>
        ///get and  set Category id 
        /// </summary>
        public Int32 SelectedComplianceCategoryId_Global
        {
            get { return Convert.ToInt32(ViewState["SelectedComplianceCategoryId"] ?? "0"); }
            set { ViewState["SelectedComplianceCategoryId"] = value; }
        }

        public Int32 SelectedPackageSubscriptionID_Global
        {
            get { return Convert.ToInt32(ViewState["SelectedPackageSubscriptionID"] ?? "0"); }
            set { ViewState["SelectedPackageSubscriptionID"] = value; }
        }

        /// <summary>
        ///get and  set package id .
        /// </summary>
        public Int32 CurrentCompliancePackageId_Global
        {
            get { return Convert.ToInt32(ViewState["CurrentCompliancePackageId"] ?? "0"); }
            set { ViewState["CurrentCompliancePackageId"] = value; }
        }

        public Int32 ItemDataId_Global
        {
            get { return Convert.ToInt32(ViewState["ItemDataId"] ?? "0"); }
            set { ViewState["ItemDataId"] = value; }
        }

        public String packageName
        {
            get { return (String)(ViewState["PackageName"] ?? ""); }
            set { ViewState["PackageName"] = value; }
        }

        public String ActionType
        {
            get;
            set;
        }

        List<DataReconciliationQueueContract> IReconciliationApplicantPanelView.lstNextPrevReconiciliationItem
        {
            get { return (List<DataReconciliationQueueContract>)(Session["lstNextPrevReconiciliationItem"] ?? ""); }
            set { Session["lstNextPrevReconiciliationItem"] = value; }
        }
        Int32 IReconciliationApplicantPanelView.CurrentCompItemRecDataID
        {
            get { return (Int32)(ViewState["CurrentCompItemRecDataID"] ?? ""); }
            set { ViewState["CurrentCompItemRecDataID"] = value; }
        }
        String IReconciliationApplicantPanelView.SelectedInstitutionIds
        {
            get { return (String)(ViewState["SelectedInstitutionIds"] ?? ""); }
            set { ViewState["SelectedInstitutionIds"] = value; }
        }
        #endregion

        #region Events

        /// <summary>
        /// OnInit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {

                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Reconcilication Detail";
                BasePage basePage = base.Page as BasePage;
                if (basePage != null)
                {
                    basePage.HideTitleBars();
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

        public event DataSaved DataSavedClick;
        public delegate void DataSaved(object sender, EventArgs e);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                CaptureQuerystringParameters();
                Presenter.OnViewInitialized();
                SetPageDataAndLayout(!this.IsPostBack);
            }
            Presenter.OnViewLoaded();
        }

        #endregion

        #region Methods

        #region Private Methods

        private void BindAttributes()
        {
            Presenter.GetApplicantData();
            lblAddress.Text = CurrentViewContext.ApplicantData.Address1.HtmlEncode();
            lblAddress.Text += ", " + CurrentViewContext.ApplicantData.City.HtmlEncode();
            lblAddress.Text += ", " + CurrentViewContext.ApplicantData.State.HtmlEncode();
            lblAddress.Text += ", " + CurrentViewContext.ApplicantData.Country;
            lblAddress.Text += " " + CurrentViewContext.ApplicantData.ZipCode.HtmlEncode();

            if (CurrentViewContext.SubscriptionExpirationDate.IsNotNull())
            {
                lblExpirationDate.Text = CurrentViewContext.SubscriptionExpirationDate.Value.ToShortDateString();
            }

            //if (this.OrganizationUserData.MiddleName.IsNullOrEmpty())
            //{
            //    lblApplicantName.Text = this.OrganizationUserData.FirstName + " " + this.OrganizationUserData.LastName;
            //}
            //else
            //{
            //    lblApplicantName.Text = this.OrganizationUserData.FirstName + " " + this.OrganizationUserData.MiddleName + " " + this.OrganizationUserData.LastName;
            //    divUser.Attributes.Add("style", "vertical-align:middle; height:90%; padding-top:4px;");
            //}
            //START UAT-4308
            if (!this.OrganizationUserData.FirstName.IsNullOrEmpty())
            {
                lblApplicantName.Text = this.OrganizationUserData.FirstName.HtmlEncode();
            }
            if (!this.OrganizationUserData.MiddleName.IsNullOrEmpty())
            {
                lblApplicantMiddleName.Text = this.OrganizationUserData.MiddleName.HtmlEncode();
                divUser.Attributes.Add("style", "vertical-align:middle; height:90%; padding-top:4px;");
            }
            if (!this.OrganizationUserData.LastName.IsNullOrEmpty())
            {
                lblApplicantLastName.Text = this.OrganizationUserData.LastName.HtmlEncode();
            }
            //END UAT
            lblEmail.Text = this.OrganizationUserData.PrimaryEmailAddress.HtmlEncode();
            lblOrder.Text = this.SelectedOrderId.ToString();
            lblPhones.Text = ((this.OrganizationUserData.IsInternationalPhoneNumber ? this.OrganizationUserData.PhoneNumber : Presenter.GetFormattedPhoneNumber(this.OrganizationUserData.PhoneNumber)) + ", " + (this.OrganizationUserData.IsInternationalSecondaryPhone ? this.OrganizationUserData.SecondaryPhone : Presenter.GetFormattedPhoneNumber(this.OrganizationUserData.SecondaryPhone))).HtmlEncode();
            lblBredCrum.Text = (this.CurrentPackageBredCrum + " > " + this.packageName).HtmlEncode();
            lblOverComplianceStatus.Text = this.CurrentCompliancePackageStatus;

            if (CurrentViewContext.OrderApprovalDate.HasValue)
            {
                lblOrderApprovalDate.Text = CurrentViewContext.OrderApprovalDate.Value.ToShortDateString();
            }
            else
            {
                lblOrderApprovalDate.Text = "N/A";
            }

            //Date of birth of the applicant
            lblApplicantDOB.Text = this.OrganizationUserData.DOB.HasValue ? "(" + this.OrganizationUserData.DOB.Value.ToShortDateString() + ")" : "";
            lblApplicantDOB.ToolTip = this.OrganizationUserData.DOB.HasValue ? "DOB: " + this.OrganizationUserData.DOB.Value.ToString("MMMM d, yyyy") : "";

            String unformattedSSN = Presenter.GetApplicantSSN();

            lblSSN.Text = Presenter.GetMaskedSSN(unformattedSSN);

            //Alias of the applicant
            if (this.OrganizationUserData.PersonAlias.IsNotNull())
            {
                String AliasName = String.Empty;
                //Implemented Changes for middle name related to UAT-2212
                this.OrganizationUserData.PersonAlias.Where(x => !x.PA_IsDeleted)
                                         .ForEach(x => AliasName += Convert.ToString(x.PA_FirstName + " " + (x.PA_MiddleName.IsNullOrEmpty() ? NoMiddleNameText : x.PA_MiddleName)
                                                                                                         + " " + x.PA_LastName) + ", ");

                if (AliasName.EndsWith(", "))
                    AliasName = AliasName.Substring(0, AliasName.Length - 2);

                if (OrganizationUserData.PersonAlias.Count() > 0 && !AliasName.IsNullOrEmpty())
                {
                    dvAlias.Visible = true;
                    lblAlias.Text = AliasName.HtmlEncode();
                }
            }
            //Applicants User Groups
            if (this.UserGroupDataList.IsNotNull() && UserGroupDataList.Count > 0)
            {
                String userGroup = String.Empty;
                this.UserGroupDataList.ForEach(x => userGroup += Convert.ToString(x.UG_Name) + ", ");

                if (userGroup.EndsWith(", "))
                    userGroup = userGroup.Substring(0, userGroup.Length - 2);
                if (!userGroup.IsNullOrEmpty())
                {
                    dvUserGroup.Visible = true;
                    lblUserGroups.Text = userGroup.HtmlEncode();
                }
            }
            if (this.CurrentCompliancePackageStatusCode.ToLower() == ApplicantPackageComplianceStatus.Not_Compliant.GetStringValue().ToLower())
            {
                imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_NON_COMPLIANCE_IMAGE_URL;
                lblOverComplianceStatus.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_COMPLIANCE_IMAGE_URL;
                lblOverComplianceStatus.ForeColor = System.Drawing.Color.Green;
            }

            if (this.OrganizationUserData.PhotoName.IsNotNull())
                imgApplicantPhoto.ImageUrl = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?UserId={0}&DocumentType={1}", this.OrganizationUserData.OrganizationUserID, "ProfilePicture");

            hdnApplicantId.Value = this.CurrentApplicantId_Global.ToString();
            hdnApplicantName.Value = this.OrganizationUserData.FirstName + " " + this.OrganizationUserData.LastName;
            imgRushOrder.Visible = this.IsRushOrder;
            //Setting Name initials
            if (!string.IsNullOrWhiteSpace(this.OrganizationUserData.FirstName))
            {
                lblNameInitials.Text = this.OrganizationUserData.FirstName.Substring(0, 1);

            }
            if (!string.IsNullOrWhiteSpace(this.OrganizationUserData.LastName))
            {

                lblNameInitials.Text = lblNameInitials.Text + this.OrganizationUserData.LastName.Substring(0, 1);
            }

            var prvReconciliationData = CurrentViewContext.lstNextPrevReconiciliationItem.FirstOrDefault();
			if (!prvReconciliationData.IsNullOrEmpty())
			{
				if (prvReconciliationData.FlatComplianceItemReconciliationDataID == CurrentViewContext.CurrentCompItemRecDataID)
				{
					btnPrevApp.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
					lnkBtnPrevApp.Disabled = true;
				}
				else
				{
					lnkBtnPrevApp.HRef = RedirectToReconciliationDetailScreen(prvReconciliationData);
				}
			}

            HiddenField hdnNextItemURL = this.Parent.FindControl("hdnNextItemURL") as HiddenField;


            var nextReconciliationData = CurrentViewContext.lstNextPrevReconiciliationItem.LastOrDefault();
			if (!nextReconciliationData.IsNullOrEmpty())
			{
				if (nextReconciliationData.FlatComplianceItemReconciliationDataID == CurrentViewContext.CurrentCompItemRecDataID)
				{
					btnNextApp.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
					lnkBtnNextApp.Disabled = true;
					hdnNextItemURL.Value = string.Empty;
				}
				else
				{
					lnkBtnNextApp.HRef = RedirectToReconciliationDetailScreen(nextReconciliationData);
					hdnNextItemURL.Value = lnkBtnNextApp.HRef;
				}
			}
        }

        #endregion

        #region "Public Methods"

        public void SetPageDataAndLayout(Boolean GetFreshData)
        {
            if (GetFreshData)
            {
                BindAttributes();
            }

        }

        #endregion

        #endregion

        private String RedirectToReconciliationDetailScreen(DataReconciliationQueueContract ReconcilictionData)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                    { 
                                                        { "TenantId",Convert.ToString(ReconcilictionData.TenantId)},
                                                        { "Child", @"~\ComplianceOperations\UserControl\ReconciliationDetail.ascx"},
                                                        { "ItemDataId", Convert.ToString(ReconcilictionData.ApplicantComplianceItemId)},
                                                        {"PackageId",Convert.ToString(ReconcilictionData.PackageID)},
                                                        {"CategoryId",Convert.ToString(ReconcilictionData.CategoryID)}, 
                                                        {"SelectedComplianceCategoryId",Convert.ToString(ReconcilictionData.CategoryID)},
                                                        {"SelectedPackageSubscriptionId",Convert.ToString(ReconcilictionData.PackageSubscriptionID)},
                                                        {"ApplicantId",Convert.ToString(ReconcilictionData.ApplicantId)},
                                                        {"ComplianceItemReconciliationDataID",Convert.ToString(ReconcilictionData.FlatComplianceItemReconciliationDataID)},
                                                        //{"institutionIds",CurrentViewContext.SelectedInstitutionIds}
                                                    };
            return String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
        }

        /// <summary>
        /// capture query string parameters
        /// </summary>
        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey("institutionIds"))
                {
                    CurrentViewContext.SelectedInstitutionIds = Convert.ToString(args["institutionIds"]);
                }
                if (args.ContainsKey("ComplianceItemReconciliationDataID"))
                {
                    CurrentViewContext.CurrentCompItemRecDataID = Convert.ToInt32(args["ComplianceItemReconciliationDataID"]);
                }
            }

            DataReconciliationQueueContract dataReconciliationQueueFilters = (DataReconciliationQueueContract)SysXWebSiteUtils.SessionService.GetCustomData("Data_Reconciliation_Queue");
            if (!dataReconciliationQueueFilters.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedInstitutionIds = String.Join(",", dataReconciliationQueueFilters.selectedTenantIds);
            }
        }
    }
}