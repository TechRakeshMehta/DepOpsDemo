using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using Entity.ClientEntity;
using System.Collections.Generic;
using CoreWeb.Shell;
using CoreWeb.Shell.MasterPages;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class PackageList : BaseWebPage, IPackageListView
    {

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private PackageListPresenter _presenter = new PackageListPresenter();
        private String _viewType;
        private CompliancePackageContract _viewContract;
        private Int32 _tenantid;

        #endregion
        #endregion

        #region properties

        #region Public Properties

        public PackageListPresenter Presenter
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
        /// CurrentUserID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for current user's id.
        /// </value>
        Int32 IPackageListView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IPackageListView CurrentViewContext
        {
            get { return this; }
        }

        public CompliancePackageContract ViewContract
        {
            get
            {
                if (_viewContract == null)
                {
                    _viewContract = new CompliancePackageContract();
                }

                return _viewContract;
            }

        }

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

        /// <summary>
        /// CompliancePackages
        /// </summary>
        /// <value>Gets or sets the list of all Compliance Packages.</value>
        /// <remarks></remarks>
        List<CompliancePackage> IPackageListView.CompliancePackages
        {
            get;
            set;

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

        //UAT-1116: Package selection combo box on package screens
        /// <summary>
        /// To set and get CompliancePackageID
        /// </summary>
        public Int32 CompliancePackageID
        {
            get;
            set;
        }

        public Int32 NotesPositionId
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdPackage.WclGridDataObject = new GridObjectDataContainer();
                ((GridObjectDataContainer)(grdPackage.WclGridDataObject)).ColumnsToSkipEncoding.Add("Description");
                base.OnInit(e);
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
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Request.QueryString["SelectedTenantId"] != null)
                {
                    Int32 TenantId = 0;
                    if (Int32.TryParse(Request.QueryString["SelectedTenantId"], out TenantId))
                        SelectedTenantId = TenantId;
                }
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }

        /// <summary>
        /// Add button Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            divAddForm.Visible = true;
            ResetControls();
        }

        /// <summary>
        /// To set DataSource of grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPackage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetCompliancePackages();

            if (CurrentViewContext.CompliancePackages.Count > 0)
            {
                grdPackage.Visible = true;
                lblTitle.Visible = true;
                grdPackage.DataSource = CurrentViewContext.CompliancePackages;
                grdPackage.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
            }
            else
            {
                grdPackage.Visible = false;
                lblTitle.Visible = false;
            }
        }

        /// <summary>
        /// Save button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarPackage_SaveClick(object sender, EventArgs e)
        {
            CurrentViewContext.ViewContract.PackageName = txtPackageName.Text.Trim();
            CurrentViewContext.ViewContract.Description = txtPkgDescription.Text.Trim();
            CurrentViewContext.ViewContract.PackageLabel = txtPackageLabel.Text.Trim();
            CurrentViewContext.ViewContract.ScreenLabel = txtScreenLabel.Text.Trim();
            CurrentViewContext.ViewContract.State = chkActive.Checked;
            CurrentViewContext.ViewContract.ViewDetails = chkViewdetails.Checked;
            CurrentViewContext.ViewContract.ExceptionDescription = txtPkgExceptionDesc.Text.Trim();
            CurrentViewContext.ViewContract.ExplanatoryNotes = txtPkgNotes.Text.Trim();
            CurrentViewContext.ViewContract.PackageDetail = rdEditorPackageDetail.Content.Trim();  //UAT 1006
            CurrentViewContext.ViewContract.CompliancePackageTypeID = Convert.ToInt32(cmbCompliancePackageType.SelectedValue);
            CurrentViewContext.ViewContract.ChecklistDocumentURL = Convert.ToString(txtChkDocumentURL.Text); //UAT 1337
            Presenter.GetPackageNotesPosition(rbtnDisplayPosition.SelectedValue); //UAT-2129
            CurrentViewContext.ViewContract.NotesDisplayPositionId = CurrentViewContext.NotesPositionId; //UAT-2129
            Presenter.SavePackagedetail();
            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Compliance Package saved successfully.");
                divAddForm.Visible = false;
                ResetControls();
                grdPackage.Rebind();

                //UAT-1116: Package selection combo box on package screens
                string packageName = CurrentViewContext.ViewContract.PackageName;
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "parent.AddNewPackage('" + Convert.ToString(CompliancePackageID) + "','" + packageName + "');", true);
                //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
                divAddForm.Visible = true;
            }

        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarPackage_CancelClick(object sender, EventArgs e)
        {
            divAddForm.Visible = false;
            ResetControls();
        }

        #endregion

        #region Methods

        /// <summary>
        /// To reset Controls
        /// </summary>
        private void ResetControls()
        {
            txtPackageName.Text = String.Empty;
            txtPkgDescription.Text = String.Empty;
            txtPackageLabel.Text = String.Empty;
            txtScreenLabel.Text = String.Empty;
            chkActive.Checked = true;
            txtPkgExceptionDesc.Text = String.Empty;
            txtPkgNotes.Text = String.Empty;
            txtChkDocumentURL.Text = String.Empty;
            cmbCompliancePackageType.DataSource = Presenter.GetCompliancePackageTypes();
            cmbCompliancePackageType.DataBind();

        }

        #endregion
    }
}

