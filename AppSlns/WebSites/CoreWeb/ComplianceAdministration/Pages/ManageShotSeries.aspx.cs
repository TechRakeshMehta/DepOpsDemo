using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using Telerik.Web.UI;
using System.Configuration;
using INTSOF.IMAGE.MANAGER;
using System.Globalization;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageShotSeries : BaseWebPage, IManageShotSeriesView
    {
        #region Variables

        private ManageShotSeriesPresenter _presenter = new ManageShotSeriesPresenter();
        private ComplianceCategoryContract _viewContract;

        #endregion

        #region Properties


        public ManageShotSeriesPresenter Presenter
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

        private Int32 _tenantid;

        /// <summary>
        /// Get or Set the Tenant ID
        /// </summary>
        Int32 IManageShotSeriesView.TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        public IManageShotSeriesView CurrentViewContext
        {
            get { return this; }
        }

        ComplianceCategoryContract IManageShotSeriesView.ViewContract
        {
            get
            {
                if (_viewContract == null)
                {
                    _viewContract = new ComplianceCategoryContract();
                }
                return _viewContract;
            }
        }

        Int32 IManageShotSeriesView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IManageShotSeriesView.CurrentCategoryId
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentCategoryId"]);
            }
            set
            {
                ViewState["CurrentCategoryId"] = value;
            }

        }

        String IManageShotSeriesView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 IManageShotSeriesView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        ComplianceCategory IManageShotSeriesView.ComplianceCategory
        {
            get;
            set;
        }

        Int32 IManageShotSeriesView.DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        string IManageShotSeriesView.SeriesName
        {
            get { return txtSeriesName.Text.Trim(); }
        }

        string IManageShotSeriesView.SeriesLabel
        {
            get { return txtSeriesLabel.Text.Trim(); }
        }

        string IManageShotSeriesView.SeriesDetails
        {
            get { return rdSeriesEditorDetails.Content; }
        }

        string IManageShotSeriesView.SeriesDescription
        {
            get { return rdSeriesEditorDescription.Content; }
        }

        bool IManageShotSeriesView.SeriesIsActive
        {
            get { return IsSeriesActiveToggle.Checked; }
        }

        bool IManageShotSeriesView.IsAvailablePostApproval
        {
            get { return Convert.ToBoolean(Convert.ToInt32(rbtnAlloEntry.SelectedValue)); }
        }

        Int32 IManageShotSeriesView.RuleExecutionOrder
        {
            get { return txtExecutionOrder.Text.IsNullOrEmpty() ? 1 : Convert.ToInt32(txtExecutionOrder.Text); }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                CurrentViewContext.CurrentCategoryId = Convert.ToInt32(Request.QueryString["Id"]);
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                BindCategoryInfo();
            }
            SetImageManagerDirectory();
            Presenter.OnViewLoaded();
            ShotSeriesListing.SelectedTenantId = CurrentViewContext.SelectedTenantId;
            ShotSeriesListing.CategoryID = CurrentViewContext.CurrentCategoryId;
            CategoryPackageListing.SelectedTenantId = CurrentViewContext.SelectedTenantId;
            CategoryPackageListing.CategoryID = CurrentViewContext.CurrentCategoryId;
        }

        #endregion

        #region Methods

        public void BindCategoryInfo()
        {
            Presenter.getCurrentCategoryInfo();
            txtCategoryName.Text = CurrentViewContext.ComplianceCategory.CategoryName;
            rdEditorDescription.Content = CurrentViewContext.ComplianceCategory.Description;
            txtCategoryLabel.Text = CurrentViewContext.ComplianceCategory.CategoryLabel;
            txtScreenLabel.Text = CurrentViewContext.ComplianceCategory.ScreenLabel;
            chkActive.Checked = CurrentViewContext.ComplianceCategory.IsActive;
            CurrentViewContext.ViewContract.ComplianceCategoryId = CurrentViewContext.ComplianceCategory.ComplianceCategoryID;
            Presenter.GetLargeContent();
            rdEditorEcplanatoryNotes.Content = CurrentViewContext.ViewContract.ExplanatoryNotes;
        }

        #endregion

        #region Button Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Presenter.SaveSeriesInfo();

            if (String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
            {
                base.ShowSuccessMessage("Shot Series saved successfully.");
                ShotSeriesListing.RebindGrid();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                divAddForm.Visible = false;
            }
            else
            {
                base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                divAddForm.Visible = true;
            }
        }

        protected void btnCancel_click(object sender, EventArgs e)
        {
            divAddForm.Visible = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            divAddForm.Visible = true;
            ResetForm();
        }

        private void ResetForm()
        {
            txtSeriesName.Text = String.Empty;
            txtSeriesLabel.Text = String.Empty;
            rdSeriesEditorDescription.Content = String.Empty;
            rdSeriesEditorDetails.Content = String.Empty;
            IsSeriesActiveToggle.Checked = true;
            txtExecutionOrder.Text = AppConsts.ONE.ToString();
        }

        #endregion


        /// <summary>
        /// Method that set the Image Managere Directory in Content Editor.
        /// </summary>
        private void SetImageManagerDirectory()
        {
            String s3ImageManagerDirectory = ConfigurationManager.AppSettings["S3ImageManagerDirectory"];
            if (ConfigurationManager.AppSettings["FileManagerMode"] == "S3")
            {
                String[] viewImages = new String[] { s3ImageManagerDirectory };
                String[] uploadImages = new String[] { s3ImageManagerDirectory };
                String[] deleteImages = new String[] { s3ImageManagerDirectory };
                SetImageManagerSettingInEditor(rdSeriesEditorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdSeriesEditorDetails, viewImages, uploadImages, deleteImages);
                rdSeriesEditorDescription.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;
                rdSeriesEditorDetails.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;

            }
            else if (ConfigurationManager.AppSettings["FileManagerMode"] == "DB")
            {
                String[] viewImages = new String[] { "InstitutionImages/" };
                String[] uploadImages = new String[] { "InstitutionImages/" };
                String[] deleteImages = new String[] { "InstitutionImages/" };
                SetImageManagerSettingInEditor(rdSeriesEditorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdSeriesEditorDetails, viewImages, uploadImages, deleteImages);
                rdSeriesEditorDescription.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
                rdSeriesEditorDetails.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
            }
            else
            {
                String[] viewImages = new String[] { "~/InstitutionImages" };
                String[] uploadImages = new String[] { "~/InstitutionImages" };
                String[] deleteImages = new String[] { "~/InstitutionImages" };
                SetImageManagerSettingInEditor(rdSeriesEditorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdSeriesEditorDetails, viewImages, uploadImages, deleteImages);
            }
        }
        private void SetImageManagerSettingInEditor(RadEditor editor, String[] viewImages, String[] uploadImages, String[] deleteImages)
        {
            editor.ImageManager.ViewPaths = viewImages;
            editor.ImageManager.UploadPaths = uploadImages;
            editor.ImageManager.DeletePaths = deleteImages;
            editor.ImageManager.MaxUploadFileSize = 71000000;

        }

    }
}

