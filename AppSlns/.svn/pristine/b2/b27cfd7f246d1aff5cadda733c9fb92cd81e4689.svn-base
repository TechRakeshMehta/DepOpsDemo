using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.IMAGE.MANAGER;
using INTSOF.Utils;
using System;
using System.Configuration;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ContentEditor : BaseUserControl, IContentEditorView
    {
        #region Private Variables

        private ContentEditorPresenter _presenter = new ContentEditorPresenter();
        #endregion

        #region properties
        public ContentEditorPresenter Presenter
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

        public IContentEditorView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Get Current loggedin UserId
        /// </summary>
        Int32 IContentEditorView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        string IContentEditorView.Content
        {
            get
            {
                if (!ViewState["Content"].IsNullOrEmpty())
                {
                    return ViewState["Content"].ToString();
                }
                return String.Empty;
            }
            set
            {
                ViewState["Content"] = value;
            }
        }
        Int32 IContentEditorView.TenantId
        {
            get
            {
                return (Int32)(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IContentEditorView.DeptProgramMappingID
        {
            get
            {
                return (Int32)(ViewState["DepProgramMappingID"]);
            }
            set
            {
                ViewState["DepProgramMappingID"] = value;
            }
        }

        PageContent IContentEditorView.ContentData
        {
            get
            {

                return (PageContent)(ViewState["ContentData"]);
            }
            set
            {
                ViewState["ContentData"] = value;
            }
        }

        public String SuccessMessage
        {
            get { return Convert.ToString(ViewState["SuccessMessage"]); }
            set { ViewState["SuccessMessage"] = value; }
        }
        public String ErrorMessage { get; set; }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                CurrentViewContext.DeptProgramMappingID = Convert.ToInt32(Request.QueryString["Id"]);
                Presenter.GetContentData();
                radContent.Content = CurrentViewContext.Content;
                SetImageEditorSource();
            }

        }

        protected void fsucCmdBar1_SaveClick(object sender, EventArgs e)
        {
            CurrentViewContext.Content = !String.IsNullOrEmpty(radContent.Content) ? radContent.Content : String.Empty;
            Presenter.SaveContentData();
            if (String.IsNullOrEmpty(ErrorMessage))
            {
                (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
            }
            else
            {
                 (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.ErrorMessage);
            }
        }

        protected void fsucCmdBar1_CancelClick(object sender, EventArgs e)
        {
            radContent.Content = CurrentViewContext.Content;
        }

        private void SetImageEditorSource()
        {
            String s3ImageManagerDirectory = ConfigurationManager.AppSettings["S3ImageManagerDirectory"];
            if (ConfigurationManager.AppSettings["FileManagerMode"] == "S3")
            {
                String[] viewImages = new String[] { s3ImageManagerDirectory };
                String[] uploadImages = new String[] { s3ImageManagerDirectory };
                String[] deleteImages = new String[] { s3ImageManagerDirectory };
                radContent.ImageManager.ViewPaths = viewImages;
                radContent.ImageManager.UploadPaths = uploadImages;
                radContent.ImageManager.DeletePaths = deleteImages;
                radContent.ImageManager.MaxUploadFileSize = 71000000;
                radContent.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;
            }
            else if (ConfigurationManager.AppSettings["FileManagerMode"] == "DB")
            {
                String[] viewImages = new String[] { "InstitutionImages/" };
                String[] uploadImages = new String[] { "InstitutionImages/" };
                String[] deleteImages = new String[] { "InstitutionImages/" };
                radContent.ImageManager.ViewPaths = viewImages;
                radContent.ImageManager.UploadPaths = uploadImages;
                radContent.ImageManager.DeletePaths = deleteImages;
                radContent.ImageManager.MaxUploadFileSize = 71000000;
                radContent.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
            }
            else
            {
                String[] viewImages = new String[] { "~/InstitutionImages" };
                String[] uploadImages = new String[] { "~/InstitutionImages" };
                String[] deleteImages = new String[] { "~/InstitutionImages" };
                radContent.ImageManager.ViewPaths = viewImages;
                radContent.ImageManager.UploadPaths = uploadImages;
                radContent.ImageManager.DeletePaths = deleteImages;
                radContent.ImageManager.MaxUploadFileSize = 71000000;
            }
        }
    }
}