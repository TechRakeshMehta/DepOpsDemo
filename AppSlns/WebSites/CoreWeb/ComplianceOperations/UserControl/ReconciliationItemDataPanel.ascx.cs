using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using System.Linq;
using INTSOF.Utils;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;
using System.Data.Entity.Core.Objects;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.ComplianceOperations.Views;
using Business.RepoManagers;
using System.Web.UI.WebControls;
using System.Text;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ReconciliationItemDataPanel : BaseUserControl, IReconciliationItemDataPanelView
    {
        public delegate void CategoryChanged(object sender, EventArgs e);
        public event CategoryChanged CategoryNextClick;
        public event CategoryChanged CategoryPreviousClick;

        public delegate void PendingReviewCategoryChanged(object sender, EventArgs e);
        public event PendingReviewCategoryChanged PendingReviewCategoryNextClick;
        public event PendingReviewCategoryChanged PendingReviewCategoryPreviousClick;

        #region Variables

        #region Private Variables

        private ReconciliationItemDataPanelPresenter _presenter = new ReconciliationItemDataPanelPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public ReconciliationItemDataPanelPresenter Presenter
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

        public IReconciliationItemDataPanelView CurrentViewContext
        {
            get { return this; }
        }

        public List<ApplicantItemVerificationData> lst
        {
            get;
            set;
        }

        public List<ReconciliationDetailsDataContract> lstReconciliationDetailsData { get; set; }

        public List<ApplicantDocuments> lstApplicantDocument
        {
            get;
            set;
        }

        public String viewType
        {
            get
            {
                if (ViewState["viewType"] != null)
                    return (String)ViewState["viewType"];
                return null;
            }
            set
            {
                ViewState["viewType"] = value;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        public Int32 CurrentTenantId_Global
        {
            get
            {
                if (!ViewState["CurrentTenantId"].IsNullOrEmpty())
                    return (Int32)(ViewState["CurrentTenantId"]);
                else
                    return 0;
            }
            set
            {
                ViewState["CurrentTenantId"] = value;
            }
        }

        public Boolean IsEscalationRecords
        {
            get
            {
                if (ViewState["IsEscalationRecords"] != null)
                    return Convert.ToBoolean(ViewState["IsEscalationRecords"]);
                return false;
            }
            set
            {
                ViewState["IsEscalationRecords"] = value;
            }
        }

        public Int32 SelectedTenantId_Global
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedTenantId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedTenantId"].IsNullOrEmpty())
                    ViewState["SelectedTenantId"] = value;
            }
        }

        public Int32 SelectedCompliancePackageId_Global
        {
            get
            {
                if (!ViewState["SelectedCompliancePackageId"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedCompliancePackageId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedCompliancePackageId"].IsNullOrEmpty())
                    ViewState["SelectedCompliancePackageId"] = value;
            }
        }

        public Int32 SelectedComplianceCategoryId_Global
        {
            get
            {
                if (!ViewState["SelectedComplianceCategoryId"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedComplianceCategoryId"]);
                else
                    return 0;
            }
            set
            {
                ViewState["SelectedComplianceCategoryId"] = value;
            }
        }

        public Int32 SelectedApplicantId_Global
        {
            get
            {
                if (!ViewState["SelectedApplicantId"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedApplicantId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedApplicantId"].IsNullOrEmpty())
                    ViewState["SelectedApplicantId"] = value;
            }
        }

        public String CurrentLoggedInUserName_Global
        {
            get
            {
                if (!ViewState["CurrentLoggedInUserName"].IsNullOrEmpty())
                    return (String)(ViewState["CurrentLoggedInUserName"]);
                else
                    return String.Empty;
            }
            set
            {
                if (ViewState["CurrentLoggedInUserName"].IsNullOrEmpty())
                    ViewState["CurrentLoggedInUserName"] = value;
            }
        }

        public Int32 AssignedToVerUser
        {
            get
            {
                if (!ViewState["AssignedToVerUser"].IsNullOrEmpty())
                    return (Int32)(ViewState["AssignedToVerUser"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["AssignedToVerUser"].IsNullOrEmpty())
                    ViewState["AssignedToVerUser"] = value;
            }
        }

        public Int32 CurrentPackageSubscriptionID_Global
        {
            get { return (Int32)(ViewState["CurrentPackageSubscriptionID"]); }
            set { ViewState["CurrentPackageSubscriptionID"] = value; }
        }

        public Boolean IsException
        {
            get { return (Boolean)(ViewState["IsException_Data"]); }
            set { ViewState["IsException_Data"] = value; }
        }

        public Boolean IncludeIncompleteItems_Global
        {
            get
            {
                if (!ViewState["IncludeIncompleteItems"].IsNullOrEmpty())
                    return (Boolean)(ViewState["IncludeIncompleteItems"]);
                else
                    return false;
            }
            set
            {
                if (ViewState["IncludeIncompleteItems"].IsNullOrEmpty())
                    ViewState["IncludeIncompleteItems"] = value;
            }
        }

        public Boolean ShowOnlyRushOrders
        {
            get
            {
                if (!ViewState["ShowOnlyRushOrders"].IsNull())
                {
                    return (Boolean)ViewState["ShowOnlyRushOrders"];
                }
                return false;
            }
            set
            {
                ViewState["ShowOnlyRushOrders"] = value;
            }
        }

        public Int32 ItemDataId_Global
        {
            get
            {
                if (!ViewState["ItemDataId"].IsNullOrEmpty())
                    return (Int32)(ViewState["ItemDataId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["ItemDataId"].IsNullOrEmpty())
                    ViewState["ItemDataId"] = value;
            }
        }

        public Int32 PackageId_Global
        {
            get
            {
                if (!ViewState["PackageId"].IsNullOrEmpty())
                    return (Int32)(ViewState["PackageId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["PackageId"].IsNullOrEmpty())
                    ViewState["PackageId"] = value;
            }
        }

        public Int32 CategoryId_Global
        {
            get
            {
                if (!ViewState["CategoryId"].IsNullOrEmpty())
                    return (Int32)(ViewState["CategoryId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["CategoryId"].IsNullOrEmpty())
                    ViewState["CategoryId"] = value;
            }
        }

        /// <summary>
        /// set user group id for return back to queue.
        /// </summary>
        public Int32 UserGroupId
        {
            get
            {
                return Convert.ToInt32(ViewState["UserGroupId"] ?? "0");
            }
            set
            {
                ViewState["UserGroupId"] = value;
            }
        }

        public String LoggedInUserInitials_Global
        {
            get
            {
                if (!ViewState["LoggedInUserInitials"].IsNullOrEmpty())
                    return (String)(ViewState["LoggedInUserInitials"]);
                else
                    return String.Empty;
            }
            set
            {
                if (ViewState["LoggedInUserInitials"].IsNullOrEmpty())
                    ViewState["LoggedInUserInitials"] = value;
            }
        }

        //UAT 537 Verification Details Screen "go to Next Pending for Review Category" should save data and button text change.
        public Boolean IsDataSavedSuccessfully
        {
            get;
            set;
        }

        //UAT-613
        public String UserId
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user != null)
                {
                    return Convert.ToString(user.UserId);
                }
                return String.Empty;
            }
        }

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                HiddenField hdnNextItemURL = this.Parent.FindControl("hdnNextItemURL") as HiddenField;

                if (!hdnNextItemURL.Value.IsNullOrEmpty())
                    btnSave.SubmitButton.Enabled = true;
                else
                    btnSave.SubmitButton.Enabled = false;

                Presenter.OnViewInitialized();
                GetCategoryData();
                #region UAT:719 Check Exceptions turned off for a Category/Item

                Boolean isExceptionAlloed = Presenter.IsAllowExceptionOnCategory();
                if (isExceptionAlloed)
                {
                    imageExceptionOff.Visible = true;
                }

                #endregion
                //UAT-3599
                Boolean isSDEdisabled = Presenter.IsStudentDataEntryEnable();
                if (isSDEdisabled)
                {
                    imageSDEdisabled.Visible = true;
                }
                hdnExplanatoryNoteState.Value = Presenter.GetExplanatoryNoteState(UserId); //Set hidden field with explanatory note state value-UAT-613.
                hdnUserId.Value = UserId;
            }
            Presenter.OnViewLoaded();

            viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            Dictionary<String, String> args = new Dictionary<string, string>();
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("UserMessage") && !this.IsPostBack)
            {
                String _message = Convert.ToString(args["UserMessage"]);
                if (!String.IsNullOrEmpty(_message))
                {
                    if (_message.LastIndexOf(',') > 0) // Errors are received as CSV from the DataLoader.cs section
                    {
                        lblMessage.Text = _message.Substring(0, _message.LastIndexOf(','));
                        lblMessage.CssClass = "error";
                    }
                    else // No "," in the success message received from the DataLoader.cs section
                    {
                        lblMessage.Text = _message;
                        lblMessage.CssClass = "sucs";
                    }
                }
            }
            else
            {
                lblMessage.Text = String.Empty;
                LoadItems();
                hiddenuploader.MaxFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
            }
        }

        private void GetCategoryData()
        {
            ComplianceCategory cmpCategory = Presenter.GetCategoryData();

            litAdminExplanation.Text = cmpCategory.Description;
            litApplicantExplanation.Text = (cmpCategory.ExpNotes + GenerateExplanatoryNotesLinkList(cmpCategory));
            lblSelectedCategoryName.Text = cmpCategory.CategoryName.HtmlEncode();

            //UAT-1519: Add the ability to include the Category Explanatory note in the Item rejected email notification.
            hdnCatExplanatoryNotes.Value = cmpCategory.ExpNotes;
            hdnCategoryMoreInfoURL.Value = cmpCategory.SampleDocFormURL.IsNullOrEmpty() ? String.Empty : cmpCategory.SampleDocFormURL;
        }

        private String GenerateExplanatoryNotesLinkList(ComplianceCategory cmpCategory)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var docUrl in cmpCategory.ComplianceCategoryDocUrls)
            {
                if (!docUrl.SampleDocFormURL.IsNullOrEmpty())
                    sb.Append(GenerateExplanatoryNotesLink(docUrl.SampleDocFormURL,docUrl.SampleDocFormURLLabel));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Genetate the link field for category explanatory notes - UAT 837
        /// </summary>
        /// <param name="sampleDocUrl"></param>
        /// <returns></returns>
        private String GenerateExplanatoryNotesLink(String sampleDocUrl,String sampleDocUrlLabel)
        {
            if (!String.IsNullOrEmpty(sampleDocUrl))
            {
                String linkText = sampleDocUrlLabel.IsNullOrEmpty() ? AppConsts.CATEGORY_EXP_NOTES_LINK_TEXT : sampleDocUrlLabel;
                return "<br /><a href=\"" + sampleDocUrl + "\" onclick=\"\" target=\"_blank\");'>" + linkText + "</a>";

            }
            return String.Empty;
        }

        private void ucVerificationItemDataPanel_DataSavedClick(object sender, EventArgs e)
        {
            //LoadItems();
        }

        protected void btnSaveCategoryNotes_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.UpdateApplicantCategoryNotes(txtCategoryNotes.Text, Convert.ToInt32(hdfApplicantComplianceCategoryId.Value));
                lblMessage.Text = "Category notes updated successfully.";
                lblMessage.CssClass = "error";
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblMessage.CssClass = "error";
                lblMessage.Text = ex.ToString();
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblMessage.CssClass = "error";
                lblMessage.Text = ex.ToString();
            }
        }

        #endregion

        #region Methods

        #region Private Methods

        #endregion

        #region Public Methods

        protected void Save(object sender, EventArgs e)
        {
            SaveCurrentCatagoryData();
        }

        private void SaveCurrentCatagoryData()
        {
            Int32 _controlCount = pnlLoader.Controls.Count;

            for (int i = 0; i < _controlCount; i++)
            {
                if (pnlLoader.Controls[i] is ReconciliationItemDataLoader)
                {
                    ReconciliationItemDataLoader _ReconciliationItemDataLoader = pnlLoader.Controls[i] as ReconciliationItemDataLoader;
                    _ReconciliationItemDataLoader.Save(hdnCatExplanatoryNotes.Value, hdnCategoryMoreInfoURL.Value);
                    IsDataSavedSuccessfully = _ReconciliationItemDataLoader.IsDataSavedSuccessfully;
                }
            }

            // to set focus on "ucVerificationItemDataPanel" middle pane
            String key = "CategoryPanel";
            if (HttpContext.Current.Items[key].IsNotNull())
            {
                INTERSOFT.WEB.UI.WebControls.WclSplitter categoryPanel = (INTERSOFT.WEB.UI.WebControls.WclSplitter)HttpContext.Current.Items[key];
                if (!categoryPanel.IsNullOrEmpty())
                    Page.FindControl(categoryPanel.UniqueID).Focus();
                else
                    this.Parent.Parent.Focus();
            }
        }

        public void LoadItems()
        {
            Control itemLoader;
            Presenter.GetComplianceItemData();
            pnlLoader.Controls.Clear();
            itemLoader = Page.LoadControl("~/ComplianceOperations/UserControl/ReconciliationItemDataLoader.ascx");

            (itemLoader as ReconciliationItemDataLoader).lst = CurrentViewContext.lst;
            (itemLoader as ReconciliationItemDataLoader).lstReconciliationDetailsData = CurrentViewContext.lstReconciliationDetailsData;
            (itemLoader as ReconciliationItemDataLoader).SelectedCompliancePackageId_Global = CurrentViewContext.SelectedCompliancePackageId_Global;
            (itemLoader as ReconciliationItemDataLoader).SelectedComplianceCategoryId_Global = CurrentViewContext.SelectedComplianceCategoryId_Global;
            (itemLoader as ReconciliationItemDataLoader).SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
            (itemLoader as ReconciliationItemDataLoader).CurrentLoggedInUserName_Global = CurrentViewContext.CurrentLoggedInUserName_Global;
            (itemLoader as ReconciliationItemDataLoader).CurrentTenantId_Global = CurrentViewContext.CurrentTenantId_Global;
            (itemLoader as ReconciliationItemDataLoader).CurrentPackageSubscriptionID_Global = CurrentViewContext.CurrentPackageSubscriptionID_Global;
            (itemLoader as ReconciliationItemDataLoader).IncludeIncompleteItems_Global = CurrentViewContext.IncludeIncompleteItems_Global;
            (itemLoader as ReconciliationItemDataLoader).ItemDataId_Global = CurrentViewContext.ItemDataId_Global;
            (itemLoader as ReconciliationItemDataLoader).SelectedApplicantId_Global = CurrentViewContext.SelectedApplicantId_Global;
            (itemLoader as ReconciliationItemDataLoader).PackageId_Global = CurrentViewContext.PackageId_Global;
            (itemLoader as ReconciliationItemDataLoader).CategoryId_Global = CurrentViewContext.CategoryId_Global;
            (itemLoader as ReconciliationItemDataLoader).ShowOnlyRushOrders = CurrentViewContext.ShowOnlyRushOrders;
            (itemLoader as ReconciliationItemDataLoader).IsException = IsException;
            (itemLoader as ReconciliationItemDataLoader).UserGroupId = CurrentViewContext.UserGroupId;
            (itemLoader as ReconciliationItemDataLoader).lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
            (itemLoader as ReconciliationItemDataLoader).btnSave = this.btnSave;
            (itemLoader as ReconciliationItemDataLoader).LoggedInUserInitials_Global = CurrentViewContext.LoggedInUserInitials_Global;
            if (lst.IsNotNull() && lst.Count() > 0)
            {
                litCategoryStatus.Text = lst[0].CatComplianceStatus;

                if (!String.IsNullOrEmpty(Convert.ToString(lst[0].ApplicantCompCatId)) && !IsPostBack)
                {
                    txtCategoryNotes.Text = lst[0].ApplicantCatNotes;
                    hdfApplicantComplianceCategoryId.Value = Convert.ToString(lst[0].ApplicantCompCatId);
                }
            }

            cbarCategory.Visible = pnlCategoryLevel.Visible = false;

            (itemLoader as ReconciliationItemDataLoader).DataSavedClick -= new ReconciliationItemDataLoader.DataSaved(ucVerificationItemDataPanel_DataSavedClick);
            (itemLoader as ReconciliationItemDataLoader).DataSavedClick += new ReconciliationItemDataLoader.DataSaved(ucVerificationItemDataPanel_DataSavedClick);
            pnlLoader.Controls.Add(itemLoader);
        }



        #endregion

        protected void btnSave_SubmitClick(object sender, EventArgs e)
        {
            SaveCurrentCatagoryData();

            if (IsDataSavedSuccessfully)
            {
                HiddenField hdnNextItemURL = this.Parent.FindControl("hdnNextItemURL") as HiddenField;

                if (!hdnNextItemURL.Value.IsNullOrEmpty())
                    Response.Redirect(hdnNextItemURL.Value);
            }
        }

        #endregion
    }
}

