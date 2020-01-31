using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Collections.Generic;
using Telerik.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Web.UI;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class CategoryListing : BaseUserControl, ICategoryListingView
    {

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private CategoryListingPresenter _presenter = new CategoryListingPresenter();
        private String _viewType;
        private ComplianceCategoryContract _viewContract;
        #endregion
        #endregion

        #region properties

        public int ParentPackageId { get; set; }

        public CategoryListingPresenter Presenter
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

        Int32 ICategoryListingView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 ICategoryListingView.TenantId
        {
            get;
            set;
        }

        public ICategoryListingView CurrentViewContext
        {
            get { return this; }
        }

        ComplianceCategoryContract ICategoryListingView.ViewContract
        {
            get
            {

                if (_viewContract == null)
                    _viewContract = new ComplianceCategoryContract();
                return _viewContract;
            }


        }

        List<ComplianceCategory> ICategoryListingView.complianceCategories
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

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
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
        #endregion

        protected override void OnInit(EventArgs e)
        {
            try
            {
                grdCategory.WclGridDataObject = new GridObjectDataContainer();
                ((GridObjectDataContainer)(grdCategory.WclGridDataObject)).ColumnsToSkipEncoding.Add("Description");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            if (SelectedTenantId == DefaultTenantId)
                ((GridButtonColumn)grdCategory.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Are you sure you want to delete this record?";
            else
                ((GridButtonColumn)grdCategory.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Deleting this record will impact existing orders. Are you sure you want to delete?";
        }

        protected void grdCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            CurrentViewContext.ViewContract.AssignToPackageId = ParentPackageId;
            Presenter.GetComplianceCategoriesByPackage();
            if (CurrentViewContext.complianceCategories.Count > 0)
            {
                grdCategory.Visible = true;
                lblTitle.Visible = true;
                grdCategory.DataSource = CurrentViewContext.complianceCategories;
                grdCategory.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
            }
            else
            {
                grdCategory.Visible = false;
                lblTitle.Visible = false;
            }
        }
        protected void grdCategory_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            CurrentViewContext.ViewContract.AssignToPackageId = ParentPackageId;
            CurrentViewContext.ViewContract.ComplianceCategoryId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ComplianceCategoryID"));
            if (Presenter.DeletePackageCategoryMapping())
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                (this.Page as BaseWebPage).ShowSuccessMessage("Compliance package category mapping deleted successfully.");
            }
            else
                (this.Page as BaseWebPage).ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
        }

    }
}
