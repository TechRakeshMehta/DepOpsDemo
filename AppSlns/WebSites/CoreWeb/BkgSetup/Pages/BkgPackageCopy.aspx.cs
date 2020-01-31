using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.BkgSetup.Views
{
    public partial class BkgPackageCopy : Page, IBkgPackageCopyView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private BkgPackageCopyPresenter _presenter = new BkgPackageCopyPresenter();
        private ISysXExceptionService _exceptionService = SysXWebSiteUtils.ExceptionService;
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public BkgPackageCopyPresenter Presenter
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

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IBkgPackageCopyView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 TenantId
        {
            get { return (Int32)(ViewState["TenantId"]); }
            set { ViewState["TenantId"] = value; }
        }

        public Int32 SourceHierarchyNodeId
        {
            get { return (Int32)(ViewState["SourceHierarchyNodeId"]); }
            set { ViewState["SourceHierarchyNodeId"] = value; }
        }

        public string BackGroundPackageName
        {
            get
            {
                return txtPackageName.Text.Trim();
            }
            set
            {
                txtPackageName.Text = value.ToString();
            }
        }

        public Int32 TargetHierarchyNodeId
        {
            get { return (Int32)(ViewState["TargetHierarchyNodeId"]); }
            set { ViewState["TargetHierarchyNodeId"] = value; }
        }

        public int BPHM_ID
        {
            get { return (Int32)(ViewState["BPHM_ID"]); }
            set { ViewState["BPHM_ID"] = value; }
        }

        public String ErrorMessage
        {
            get { return (String)(ViewState["ErrorMessage"]); }
            set { ViewState["ErrorMessage"] = value; }
        }

        #endregion

        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
                    CurrentViewContext.BPHM_ID = Convert.ToInt32(Request.QueryString["BPHM_ID"]);
                    CurrentViewContext.SourceHierarchyNodeId = Convert.ToInt32(Request.QueryString["HierarchyNodeId"]);
                    hdnTenantId.Value = CurrentViewContext.TenantId.ToString();
                   // hdnDepartmntPrgrmMppng.Value = CurrentViewContext.SourceHierarchyNodeId.ToString();
                }
                lblinstituteHierarchy.Text = hdnHierarchyLabel.Value;
                Presenter.OnViewLoaded();
                Title = "Copy Background Package";
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }

        }

        protected void fsucCmdBarCopyPkg_SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (hdnDepartmntPrgrmMppng.Value != String.Empty)
                    CurrentViewContext.TargetHierarchyNodeId = Convert.ToInt32(hdnDepartmntPrgrmMppng.Value);
                else
                {
                    lblMessage.ShowMessage("Please select target node.", MessageType.Information);
                    return;
                }
                Presenter.CopyPackageStructure();
                if (CurrentViewContext.ErrorMessage == String.Empty)
                {
                    lblMessage.ShowMessage("Package copy created successfully.", MessageType.SuccessMessage);
                    txtPackageName.Text = String.Empty;
                    lblinstituteHierarchy.Text = String.Empty;
                    hdnInstitutionNodeId.Value = String.Empty;
                    hdnHierarchyLabel.Value = String.Empty;
                }
                else
                {
                    lblMessage.ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Information);
                    txtPackageName.Text = String.Empty;
                }
                //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "Page.hideProgress();", true);
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="ex"></param>
        private void LogError(String errorMessage, System.Exception ex)
        {
            _exceptionService.HandleError(errorMessage, ex);
        }
        #endregion
    }
}