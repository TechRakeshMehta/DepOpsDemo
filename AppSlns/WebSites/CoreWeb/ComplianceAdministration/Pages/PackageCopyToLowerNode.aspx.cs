using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class PackageCopyToLowerNode : BaseWebPage, IPackageCopyToLowerNode
    {
        #region Private Variables

        private PackageCopyToLowerNodePresenter _presenter = new PackageCopyToLowerNodePresenter();
        #endregion
        #region Properties

        public PackageCopyToLowerNodePresenter Presenter
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

        Int32 IPackageCopyToLowerNode.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IPackageCopyToLowerNode CurrentViewContext
        {
            get { return this; }
        }

        Int32 IPackageCopyToLowerNode.TenantId
        {
            get
            {
                if (ViewState["TenantId"] != null)
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                return AppConsts.NONE;
            }
            set { ViewState["TenantId"] = value; }
        }

        Int32 IPackageCopyToLowerNode.CompliancePackageID
        {
            get
            {
                if (ViewState["CompliancePackageID"] != null)
                {
                  return  Convert.ToInt32(ViewState["CompliancePackageID"]);
                }
                return AppConsts.NONE;

            }
            set
            {
                ViewState["CompliancePackageID"] = value;
            }
        }

        String IPackageCopyToLowerNode.CompliancePackageName
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

        Int32 IPackageCopyToLowerNode.NodeID
        {
            get
            {
                return Convert.ToInt32(ViewState["NodeID"]);
            }
            set
            {
                ViewState["NodeID"] = value;
            }
        }

        Int32 IPackageCopyToLowerNode.SelectedNodeID
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedNodeID"]);
            }
            set
            {
                ViewState["SelectedNodeID"] = value;
            }
        }

        List<DeptProgramMapping> IPackageCopyToLowerNode.lstDepartmentProgramMapping
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        #endregion

        #region Page events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantID"]);
                CurrentViewContext.CompliancePackageID = Convert.ToInt32(Request.QueryString["CompliancePackageID"]);
                CurrentViewContext.NodeID = Convert.ToInt32(Request.QueryString["NodeID"]);

                if (!CurrentViewContext.TenantId.IsNullOrEmpty() && !CurrentViewContext.NodeID.IsNullOrEmpty())
                {
                    Presenter.GetNodeList();
                    cmbNodes.DataSource = CurrentViewContext.lstDepartmentProgramMapping;
                    cmbNodes.DataBind();
                }
            }
        }
        #endregion

        #region Button Events

        protected void cmdBarCopy_SaveClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.SelectedNodeID = Convert.ToInt32(cmbNodes.SelectedValue);
                CurrentViewContext.CompliancePackageName = txtPackageName.Text;
                Presenter.CopyPackageStructure();
                if (CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    base.ShowSuccessMessage("Package copy created successfully.");
                    txtPackageName.Text = String.Empty;
                    cmbNodes.ClearSelection();
                }
                else
                {
                    //fsucCmdBarPrice.SaveButton.Enabled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "Page.hideProgress();", true);
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

    }
}