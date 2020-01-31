using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.BkgOperations.Views
{
    public partial class ViewBkgPackageDetail : BaseUserControl, IViewBkgPackageDetailView
    {
        #region Private Variables

        private ViewBkgPackageDetailPresenter _presenter = new ViewBkgPackageDetailPresenter();
        //String _viewType = null;

        #endregion

        #region Public Properties

        public ViewBkgPackageDetailPresenter Presenter
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
        /// Gets the data of TreeListPackagesDetail.
        /// </summary>
        public List<Entity.ClientEntity.GetBkgPackageDetailTree> TreeListBkgPackagesDetail
        {
            get;
            set;
        }

        public Int32 ManageTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["ManageTenantId"]);
            }
            set
            {
                ViewState["ManageTenantId"] = value;
            }
        }

        public Int32 BackroundPackageID
        {
            get;
            set;
        }

        public IViewBkgPackageDetailView CurrentViewContext
        {
            get
            {
                return this;
            }
        } 
        #endregion

        #region Events

        #region Page Load Event

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //_viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                Dictionary<String, String> queryString = new Dictionary<String, String>();

                //Decrypt the TenantId from Query String.
                if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                {
                    queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                }

                //Checks if the TenantId is present in Query String.
                if (queryString.ContainsKey("TenantId"))
                {
                    //Assigns the TenantId to property ManageTenantId.
                    if (!queryString["TenantId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.ManageTenantId = Convert.ToInt32(queryString["TenantId"]);
                    }
                }

                //Checks if the PackageID is present in Query String.
                if (queryString.ContainsKey("PackageId"))
                {
                    //Assigns the TenantId to property ManageTenantId.
                    if (!queryString["PackageId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.BackroundPackageID = Convert.ToInt32(queryString["PackageId"]);
                    }
                }

                //Checks if page is PostBack or not.
                if (!this.IsPostBack)
                {
                    //Gets the assigned tree data from database and sets it in property AssignedTreeData.
                    Presenter.GetTreeData();
                    Presenter.OnViewInitialized();
                }
                Presenter.OnViewLoaded();
                base.SetPageTitle("Package Detail"); 
                CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault basePage = base.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault;
                basePage.SetModuleTitle("Background");

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

        #region Tree Events

        protected void treeListbkgPackageDetail_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                treeListbkgPackageDetail.DataSource = CurrentViewContext.TreeListBkgPackagesDetail;
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

        protected void treeListbkgPackageDetail_PreRender(object sender, EventArgs e)
        {
            try
            {
                treeListbkgPackageDetail.ExpandAllItems();
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

        protected void treeListbkgPackageDetail_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            try
            {
                // Below code disables the expand button.
                if (e.Item is TreeListDataItem)
                {
                    Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                    if (expandButton != null)
                    {
                        ((Button)expandButton).Enabled = false;
                        ((Button)expandButton).Visible = false;
                    }
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

        #endregion

        #region Button Event

        protected void fsucCmdBarBkgPackageDetail_SubmitClick(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString;
            try
            {
                queryString = new Dictionary<String, String>()
                                                         { 
                                                            { "Child",  @"~\ComplianceOperations\UserControl\PendingOrder.ascx"}
                                                         };
                Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
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

        #endregion
    }
}