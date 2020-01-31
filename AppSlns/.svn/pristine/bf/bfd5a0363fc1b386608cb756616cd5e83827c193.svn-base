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
    public partial class ShotSeriesInfo : BaseWebPage, IShotSeriesInfoView
    {
        #region Variables

        private ShotSeriesInfoPresenter _presenter = new ShotSeriesInfoPresenter();

        #endregion

        #region Properties


        public ShotSeriesInfoPresenter Presenter
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
        public Int32 TenantId
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

        public IShotSeriesInfoView CurrentViewContext
        {
            get { return this; }
        }

        public ItemSery CurrentItemSeries
        {
            get;
            set;

        }

        public int CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public int CurrentSeriesID
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentSeriesID"]);
            }
            set
            {
                ViewState["CurrentSeriesID"] = value;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
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

        public Int32 CategoryID
        {
            get
            {
                return Convert.ToInt32(ViewState["CategoryID"]);
            }
            set
            {
                ViewState["CategoryID"] = value;
            }
        }

        public Int32 DefaultTenantId
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

        string IShotSeriesInfoView.SeriesName
        {
            get { return txtSeriesName.Text.Trim(); }
        }

        string IShotSeriesInfoView.SeriesLabel
        {
            get { return txtSeriesLabel.Text.Trim(); }
        }

        string IShotSeriesInfoView.SeriesDetails
        {
            get { return rdEditorDetails.Content; }
        }

        string IShotSeriesInfoView.SeriesDescription
        {
            get { return rdEditorDescription.Content; }
        }

        bool IShotSeriesInfoView.SeriesIsActive
        {
            get { return chkActive.Checked; }
        }

        bool IShotSeriesInfoView.IsAvailablePostApproval
        {
            get { return Convert.ToBoolean(Convert.ToInt32(rbtnAlloEntry.SelectedValue)); }
        }

        Int32 IShotSeriesInfoView.RuleExecutionOrder
        {
            get { return txtExecutionOrder.Text.IsNullOrEmpty() ? 1 : Convert.ToInt32(txtExecutionOrder.Text); }
        }
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            fsucCmdBarCat.DisplayButtons = CommandBarButtons.Save;
            fsucCmdBarCat.DisplayButtons = CommandBarButtons.Cancel;
            fsucCmdBarCat.DisplayButtons = CommandBarButtons.Submit;
            fsucCmdBarCat.DisplayButtons = CommandBarButtons.Extra;

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                CurrentViewContext.CurrentSeriesID = Convert.ToInt32(Request.QueryString["Id"]);
                SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                CategoryID = Convert.ToInt32(Request.QueryString["CatId"]);
                hdnSelectedTenantID.Value = SelectedTenantId.ToString();
                hdnCategoryID.Value = CategoryID.ToString();
                hdnSeriesID.Value = CurrentViewContext.CurrentSeriesID.ToString();
                BindShotSeriesInfo();
                ResetButtons(true);
                SetFormMode(false);
            }
            Presenter.OnViewLoaded();
            SetImageManagerDirectory();
        }

        protected void fsucCmdBarCat_CancelClick(object sender, EventArgs e)
        {
            BindShotSeriesInfo();
            ResetButtons(true);
            SetFormMode(false);
        }

        protected void fsucCmdBarCat_SubmitClick(object sender, EventArgs e)
        {
            ResetButtons(false);
            SetFormMode(true);
            fsucCmdBarCat.SaveButton.ValidationGroup = "grpFormSubmit";
        }

        protected void fsucCmdBarCat_SaveClick(object sender, EventArgs e)
        {
            Presenter.UpdateItemSeries();
            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Item Series updated successfully.");
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
            }
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
            ResetButtons(true);
            SetFormMode(false);
        }

        #endregion

        #region Methods

        public void BindShotSeriesInfo()
        {
            Presenter.GetCurrentItemSeriesInfo();
            txtSeriesName.Text = CurrentViewContext.CurrentItemSeries.IS_Name;
            rdEditorDescription.Content = CurrentViewContext.CurrentItemSeries.IS_Description;
            rdEditorDetails.Content = CurrentViewContext.CurrentItemSeries.IS_Details;
            txtSeriesLabel.Text = CurrentViewContext.CurrentItemSeries.IS_Label;
            chkActive.Checked = CurrentViewContext.CurrentItemSeries.IS_IsActive;
            rbtnAlloEntry.SelectedValue = CurrentViewContext.CurrentItemSeries.IS_IsAvailablePostApproval ? "1" : "0";
            txtExecutionOrder.Text = Convert.ToString( CurrentViewContext.CurrentItemSeries.IS_RuleExecutionOrder);
        }

        private void ResetButtons(Boolean isReset)
        {
            fsucCmdBarCat.SaveButton.Visible = !isReset;
            fsucCmdBarCat.CancelButton.Visible = !isReset;
            fsucCmdBarCat.SubmitButton.Visible = isReset;
            fsucCmdBarCat.ExtraButton.Visible = isReset;
            fsucCmdBarCat.SubmitButton.Text = "Edit";
            fsucCmdBarCat.ExtraButton.Text = "Test Shuffling";
        }

        private void SetFormMode(Boolean isEnabled)
        {
            txtSeriesName.Enabled = isEnabled;
            //txtCatDescription.Enabled = isEnabled;
            if (isEnabled)
            {
                rdEditorDescription.EditModes = EditModes.All;
                rdEditorDetails.EditModes = EditModes.All;
            }
            else
            {
                rdEditorDescription.EditModes = EditModes.Preview;
                rdEditorDetails.EditModes = EditModes.Preview;
            }
            txtSeriesLabel.Enabled = isEnabled;
            chkActive.IsActiveEnable = isEnabled;
            rbtnAlloEntry.Enabled = isEnabled;
            txtExecutionOrder.Enabled = isEnabled;
        }

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
                SetImageManagerSettingInEditor(rdEditorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdEditorDetails, viewImages, uploadImages, deleteImages);
                rdEditorDescription.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;
                rdEditorDetails.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;

            }
            else if (ConfigurationManager.AppSettings["FileManagerMode"] == "DB")
            {
                String[] viewImages = new String[] { "InstitutionImages/" };
                String[] uploadImages = new String[] { "InstitutionImages/" };
                String[] deleteImages = new String[] { "InstitutionImages/" };
                SetImageManagerSettingInEditor(rdEditorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdEditorDetails, viewImages, uploadImages, deleteImages);
                rdEditorDescription.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
                rdEditorDetails.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
            }
            else
            {
                String[] viewImages = new String[] { "~/InstitutionImages" };
                String[] uploadImages = new String[] { "~/InstitutionImages" };
                String[] deleteImages = new String[] { "~/InstitutionImages" };
                SetImageManagerSettingInEditor(rdEditorDescription, viewImages, uploadImages, deleteImages);
                SetImageManagerSettingInEditor(rdEditorDetails, viewImages, uploadImages, deleteImages);
            }
        }
        private void SetImageManagerSettingInEditor(RadEditor editor, String[] viewImages, String[] uploadImages, String[] deleteImages)
        {
            editor.ImageManager.ViewPaths = viewImages;
            editor.ImageManager.UploadPaths = uploadImages;
            editor.ImageManager.DeletePaths = deleteImages;
            editor.ImageManager.MaxUploadFileSize = 71000000;

        }
        #endregion

    }
}