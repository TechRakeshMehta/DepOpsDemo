using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using Entity.ClientEntity;
using System.Collections.Generic;
using CoreWeb.Shell;
using CoreWeb.Shell.MasterPages;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;

namespace CoreWeb.BkgSetup.Views
{
    public partial class PackageBundleBkg : BaseWebPage, IPackageBundleBkgView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private PackageBundleBkgPresenter _presenter = new PackageBundleBkgPresenter();

        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public PackageBundleBkgPresenter Presenter
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


        public IPackageBundleBkgView CurrentViewContext
        {
            get { return this; }
        }

        Boolean IPackageBundleBkgView.IsBundleExclusive
        {
            get
            {
                return Convert.ToBoolean(rbtnExclusive.SelectedValue);
            }
            set
            {
                rbtnExclusive.SelectedValue = value.ToString();
            }
        }

        Int32 IPackageBundleBkgView.TenantId
        {
            get { return (Int32)(ViewState["TenantId"]); }
            set { ViewState["TenantId"] = value; }
        }


        /// <summary>
        /// ListDeptProgramPackagePriceAdjustment
        /// </summary>
        List<INTSOF.UI.Contract.ComplianceOperation.PackageBundleContract> IPackageBundleBkgView.ListPackageBundle
        {
            get;
            set;
        }



        Int32 IPackageBundleBkgView.ID
        {
            get
            {
                return Convert.ToInt32(ViewState["ID"]);
            }
            set
            {
                ViewState["ID"] = value;
            }
        }
        Int32 IPackageBundleBkgView.ParentID
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentID"]);
            }
            set
            {
                ViewState["ParentID"] = value;
            }
        }
        Int32 IPackageBundleBkgView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }



        #endregion

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                if (!Request.QueryString["SelectedTenantId"].IsNullOrEmpty())
                {
                    CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                }
                if (!Request.QueryString["Id"].IsNullOrEmpty())
                {
                    CurrentViewContext.ID = Convert.ToInt32(Request.QueryString["Id"]);
                }
                if (!Request.QueryString["ParentID"].IsNullOrEmpty())
                {
                    CurrentViewContext.ParentID = Convert.ToInt32(Request.QueryString["ParentID"]);
                }
            }
        }

        protected void grdPackageBundleBkg_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetPackageBundleNodeMapping();
                Presenter.GetBkgPackageIncludedInBundle();
                grdPackageBundleBkg.DataSource = CurrentViewContext.ListPackageBundle;
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

        protected void CmdBarSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.UpdatePackageBundleNodeMapping())
                {
                    base.ShowSuccessMessage("Package Details updated successfully.");
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
    }
}