using System;
using Microsoft.Practices.ObjectBuilder;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class CategoryPackageListing : BaseUserControl, ICategoryPackageListingView
    {
        private CategoryPackageListingPresenter _presenter = new CategoryPackageListingPresenter();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }


        public CategoryPackageListingPresenter Presenter
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

        Int32 ICategoryPackageListingView.DefaultTenantId
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

        String ICategoryPackageListingView.ErrorMessage
        {
            get;
            set;
        }

        public Int32 CategoryID
        {
            get;
            set;
        }

        List<CompliancePackage> ICategoryPackageListingView.LstPackages
        {
            get;
            set;
        }

        public ICategoryPackageListingView CurrentViewContext
        {
            get { return this; }
        }


        Int32 ICategoryPackageListingView.TenantId
        {
            get;
            set;
        }

        Int32 ICategoryPackageListingView.CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }

        }

        protected void grdPackage_NeedDataSource1(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetPackagesRelatedToCategory();
            if (!CurrentViewContext.LstPackages.IsNullOrEmpty())
            {
                grdPackage.Visible = true;
                this.Parent.FindControl("lblTitlePackage").Visible = true;
                grdPackage.DataSource = CurrentViewContext.LstPackages;
            }
            else
            {
                grdPackage.Visible = false;
                this.Parent.FindControl("lblTitlePackage").Visible = false;
            }
        }
    }
}

