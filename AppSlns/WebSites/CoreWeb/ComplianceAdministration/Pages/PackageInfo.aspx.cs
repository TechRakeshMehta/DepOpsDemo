using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class PackageInfo : BaseWebPage, IPackageInfoView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private PackageInfoPresenter _presenter=new PackageInfoPresenter();
        private CompliancePackageContract _viewContract;
        private String _viewType;
        private Int32 _tenantid;

        #endregion
        #endregion

        #region Properties

        
        public PackageInfoPresenter Presenter
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

        public IPackageInfoView CurrentViewContext
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

        public CompliancePackage CompliancePackage
        {
            get;
            set;

        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
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

        public Int32 CurrentPackageId
        {
            get
            {
                return Convert.ToInt32(ViewState["currentPackageId"]);
            }
            set
            {
                ViewState["currentPackageId"] = value;
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

        public Int32 NotesPositionId
        {
            get;
            set;
        }

        Int32 IPackageInfoView.DefaultTenantId
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

        string IPackageInfoView.PackageBundleNodeHierarchy
        {
            get
            {
                if(ViewState["PackageBundleNodeHierarchy"] == null)
                {
                    ViewState["PackageBundleNodeHierarchy"] = string.Empty;
                }
                return ViewState["PackageBundleNodeHierarchy"].ToString();
            }
            set
            {
                ViewState["PackageBundleNodeHierarchy"] = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                CurrentPackageId = Convert.ToInt32(Request.QueryString["Id"]);
                SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                ViewContract.CompliancePackageId = CurrentPackageId;
                BindPackageInfo();
                ResetButtons(true);
                SetFormMode(false);
            }
            Presenter.OnViewLoaded();

            if (this.CurrentPackageId > 0)
                ViewContract.CompliancePackageId = CurrentPackageId;
            grdCategorylist.ParentPackageId = CurrentPackageId;
            grdCategorylist.SelectedTenantId = SelectedTenantId;
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarPackage_CancelClick(object sender, EventArgs e)
        {
            BindPackageInfo();
            ResetButtons(true);
            SetFormMode(false);
        }

        /// <summary>
        /// Submit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarPackage_SubmitClick(object sender, EventArgs e)
        {
            ResetButtons(false);
            SetFormMode(true);
        }

        /// <summary>
        /// Save button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarPackage_SaveClick(object sender, EventArgs e)
        {
            CurrentViewContext.ViewContract.PackageName = txtPackageName.Text.Trim();
            CurrentViewContext.ViewContract.PackageLabel = txtPackageLabel.Text.Trim();
            CurrentViewContext.ViewContract.ScreenLabel = txtScreenLabel.Text.Trim();
            CurrentViewContext.ViewContract.Description = txtPkgDescription.Text.Trim();
            CurrentViewContext.ViewContract.State = chkActive.Checked;
            CurrentViewContext.ViewContract.ViewDetails = chkViewdetails.Checked;
            ViewContract.CompliancePackageId = CurrentViewContext.CurrentPackageId;
            CurrentViewContext.ViewContract.ExceptionDescription = txtPkgExceptionDesc.Text.Trim();
            CurrentViewContext.ViewContract.ExplanatoryNotes = txtPkgNotes.Text.Trim();
            CurrentViewContext.ViewContract.PackageDetail = rdEditorPackageDetail.Content.Trim();
            CurrentViewContext.ViewContract.CompliancePackageTypeID = Convert.ToInt32(cmbCompliancePackageType.SelectedValue);
            CurrentViewContext.ViewContract.ChecklistDocumentURL = Convert.ToString(txtChkDocumentURL.Text);
            Presenter.GetPackageNotesPosition(rbtnDisplayPosition.SelectedValue); //UAT-2219
            CurrentViewContext.ViewContract.NotesDisplayPositionId = CurrentViewContext.NotesPositionId; //UAT-2219

            Presenter.UpdatePackageDetail();

            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Compliance Package updated successfully.");
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "parent.UpdatePackageDropDownPkg('" + Convert.ToString(CurrentViewContext.CurrentPackageId) + "','" + ViewContract.PackageName + "');", true);
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

        #region Public Methods

        /// <summary>
        /// To bind Package Info controls
        /// </summary>
        public void BindPackageInfo()
        {
            Presenter.getPackageInfo();
            //Commented for now as ticket is removed from the current sprint
            //UAT-3896 
            //Presenter.GetPackageBundleNodeHierarchy();
            Presenter.GetLargeContent();

            cmbCompliancePackageType.DataSource = Presenter.GetCompliancePackageTypes();
            cmbCompliancePackageType.DataBind();
            cmbCompliancePackageType.SelectedValue = CurrentViewContext.CompliancePackage.CompliancePackageTypeID.ToString();

            txtPackageName.Text = CurrentViewContext.CompliancePackage.PackageName;
            txtPackageLabel.Text = CurrentViewContext.CompliancePackage.PackageLabel;
            txtScreenLabel.Text = CurrentViewContext.CompliancePackage.ScreenLabel;
            txtPkgDescription.Text = CurrentViewContext.CompliancePackage.Description;
            chkActive.Checked = CurrentViewContext.CompliancePackage.IsActive;
            chkViewdetails.Checked = CurrentViewContext.CompliancePackage.IsViewDetailsInOrderEnabled;
            txtPkgExceptionDesc.Text = CurrentViewContext.ViewContract.ExceptionDescription;
            txtPkgNotes.Text = CurrentViewContext.ViewContract.ExplanatoryNotes;
            rdEditorPackageDetail.Content = CurrentViewContext.CompliancePackage.PackageDetail;
            txtChkDocumentURL.Text = CurrentViewContext.CompliancePackage.ChecklistURL;
            rbtnDisplayPosition.SelectedValue = CurrentViewContext.CompliancePackage.NotesDisplayPositionId == 1 ? "AAAA" : "AAAB";  //UAT-2129
            //UAT-2950
            if (CurrentViewContext.DefaultTenantId != SelectedTenantId)
            {
                dvMappingHierarchy.Style["display"] = "block";
                //dvBundleMappingHierarchy.Style["display"] = "block";
                lblMappedHierarchy.Text = (GetHierarchyText() + "," + Presenter.GetHierarchyTextForBundle()).HtmlEncode();
                
                //lblBundleMappedHierarchy.Text = CurrentViewContext.PackageBundleNodeHierarchy;
            }
            else
            {
                dvMappingHierarchy.Style["display"] = "none";
                //dvBundleMappingHierarchy.Style["display"] = "none";
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///  To show hide the buttons
        /// </summary>
        /// <param name="isReset"></param>
        private void ResetButtons(Boolean isReset)
        {
            fsucCmdBarPackage.SaveButton.Visible = !isReset;
            fsucCmdBarPackage.CancelButton.Visible = !isReset;
            fsucCmdBarPackage.SubmitButton.Visible = isReset;
            fsucCmdBarPackage.SubmitButton.Text = "Edit";
            //UAT-3716
            HideShowCopyItemDataButton(isReset);
        }

        /// <summary>
        /// To set Form Mode (Readonly true or false)
        /// </summary>
        /// <param name="isReadOnly"></param>
        private void SetFormMode(Boolean isEnabled)
        {
            txtPackageName.Enabled = isEnabled;
            txtPackageLabel.Enabled = isEnabled;
            txtScreenLabel.Enabled = isEnabled;
            chkActive.IsActiveEnable = isEnabled;
            chkViewdetails.Enabled = isEnabled;
            txtPkgDescription.Enabled = isEnabled;
            txtPkgNotes.Enabled = isEnabled;
            txtPkgExceptionDesc.Enabled = isEnabled;
            txtPkgNotes.Enabled = isEnabled;
            txtChkDocumentURL.Enabled = isEnabled;
            rbtnDisplayPosition.Enabled = isEnabled;
            if (isEnabled)
            {
                rdEditorPackageDetail.EditModes = EditModes.All;
            }
            else
            {
                rdEditorPackageDetail.EditModes = EditModes.Preview;
            }
            cmbCompliancePackageType.Enabled = isEnabled;
        }

        //UAT-2950
        private String GetHierarchyText()
        { 
            String HierarchyNodeText = String.Empty;
            if (!CurrentViewContext.CompliancePackage.DeptProgramPackages.IsNullOrEmpty())
            {
                foreach (var deptProgramPackage in CurrentViewContext.CompliancePackage.DeptProgramPackages)
                {
                    if (!deptProgramPackage.DPP_IsDeleted && !deptProgramPackage.DeptProgramMapping.DPM_IsDeleted)
                    {
                        HierarchyNodeText = HierarchyNodeText + ", " + deptProgramPackage.DeptProgramMapping.DPM_Label;
                    }
                }
                if (HierarchyNodeText.StartsWith(", "))
                    HierarchyNodeText = HierarchyNodeText.Substring(2, HierarchyNodeText.Length - 2);
            }
            return HierarchyNodeText;
        }

        #endregion



        #region UAT-3716: Additional Tracking to Rotation Mapping development and testing
        private void HideShowCopyItemDataButton(Boolean isReset)
        {

            if (!isReset)
                btnCopyPkgData.Visible = true;
            else
                btnCopyPkgData.Visible = false;
        }

        protected void btnCopyPkgData_Click(object sender, EventArgs e)
        {

            Presenter.AddApprovedPkgsToCopyDataQueue();

            base.ShowSuccessMessage("Data synchronization has been initiated. Compliance package data will be copied to rotation package in few minutes.");

            ResetButtons(true);
            SetFormMode(false);

        }

        #endregion UAT-3716

        #endregion
    }
}

