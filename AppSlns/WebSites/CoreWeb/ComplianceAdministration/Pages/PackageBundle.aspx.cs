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

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class PackageBundle : BaseWebPage, IPackageBundleView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private PackageBundlePresenter _presenter = new PackageBundlePresenter();

        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public PackageBundlePresenter Presenter
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


        public IPackageBundleView CurrentViewContext
        {
            get { return this; }
        }

        Int32 IPackageBundleView.TenantId
        {
            get { return (Int32)(ViewState["TenantId"]); }
            set { ViewState["TenantId"] = value; }
        }


        /// <summary>
        /// ListDeptProgramPackagePriceAdjustment
        /// </summary>
        List<INTSOF.UI.Contract.ComplianceOperation.PackageBundleContract> IPackageBundleView.ListPackageBundle
        {
            get;
            set;
        }



        Int32 IPackageBundleView.ID
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
        Int32 IPackageBundleView.ParentID
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
  

        Boolean IPackageBundleView.IsBundleExclusive
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

        Int32 IPackageBundleView.CurrentLoggedInUserId
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
                if (!Request.QueryString["TenantId"].IsNullOrEmpty())
                {
                    CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
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

        protected void grdPackageBundle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetPackageBundleNodeMapping();
                Presenter.GetPackageIncludedInBundle();
                grdPackageBundle.DataSource = CurrentViewContext.ListPackageBundle;
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