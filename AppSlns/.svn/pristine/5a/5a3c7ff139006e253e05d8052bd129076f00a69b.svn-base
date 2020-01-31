#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Web.Services;
using System.Threading;
using System.Web.Script.Serialization;
using System.Linq;
using Telerik.Web.UI;
using System.Web;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Web.UI.WebControls;


#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    /// <summary>
    /// Setup.aspx.cs class used to display hierarchy of packages, categories, items and attributes in a treeview.
    /// </summary>
    public partial class SetupShotSeries : BaseWebPage, ISetupShotSeriesView
    {
        #region Variables

        #region Private Variables

        private SetupShotSeriesPresenter _presenter = new SetupShotSeriesPresenter();
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _tenantid;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public SetupShotSeriesPresenter Presenter
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

        public List<GetShotSeriesTree> lstTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeData") != null)
                    return (List<GetShotSeriesTree>)(SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("ComplianceTreeData", value);
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        public List<Tenant> ListTenants
        {
            set;
            get;
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

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.Value;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(ddlTenant.SelectedValue, out _selectedTenantId);

                    if (_selectedTenantId == AppConsts.NONE)
                        _selectedTenantId = TenantId;
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
                //txtDefaultScreen.Text = "packagelist.aspx?SelectedTenantId=" + value;
            }
        }

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
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

        public List<GetRuleSetTree> lstCurrentTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeCurrentData") != null)
                    return (List<GetRuleSetTree>)(SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeCurrentData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("ComplianceTreeCurrentData", value);
            }
        }


        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                var updatePanel = this.Master.FindControl("UpdatePanel1") as System.Web.UI.UpdatePanel;
                updatePanel.UpdateMode = System.Web.UI.UpdatePanelUpdateMode.Conditional;
                updatePanel.ChildrenAsTriggers = false;
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
        /// Loads the page Setup.aspx.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindTenant();
                    SetDefaultSelectedTenantId();
                    BindHierarchyTree(true);

                    if (Session[AppConsts.SHOTSERIES_SESSION_LISTATTRIBUTESCONTRACT].IsNotNull() && Session[AppConsts.SHOTSERIES_SESSION_LISTITEMCONTRACT].IsNotNull())
                    {
                        Session.Remove(AppConsts.SHOTSERIES_SESSION_LISTATTRIBUTESCONTRACT);
                        Session.Remove(AppConsts.SHOTSERIES_SESSION_LISTITEMCONTRACT);
                    }
                }
                else
                {
                    //for getting if page load is called by tree itself or right panel
                    Boolean asynchronousRequest = System.Web.UI.ScriptManager.GetCurrent(this).IsInAsyncPostBack;
                    WclSplitBar1.Visible = true;
                    BindHierarchyTree(asynchronousRequest);
                }
                Presenter.OnViewLoaded();

                //Hiding top page
                CoreWeb.Shell.MasterPages.DefaultMaster masterPage = this.Master as CoreWeb.Shell.MasterPages.DefaultMaster;
                if (masterPage != null)
                {
                    masterPage.HideTitleBars(true);
                }

                base.Title = "Manage Shot Series";
                base.SetModuleTitle("ShotSeries Setup");
                base.SetPageTitle("Manage ShotSeries");
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

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindHierarchyTree(true);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "NavigateToSelectedNode('');", true);
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

        #region TreeView Events

        /// <summary>
        /// Bounds the data to tree view (which includes list of packages, categories, items and attributes).
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void treeShotSeries_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                //Gets the detail of a node.
                GetShotSeriesTree item = (GetShotSeriesTree)e.Node.DataItem;

                //Sets the navigation URL, Color Code and Font Code as per node type.
                ComplianceTreeUIContract complianceTreeUIContract = Presenter.GetTreeNodeDetails(item.UICode, item.DataID, item.ParentDataID, item.ParentNodeID, item.NodeID);

                e.Node.NavigateUrl = complianceTreeUIContract.NavigateURL;
                e.Node.Value = item.NodeID;
                e.Node.Target = "childpageframe";
                e.Node.Expanded = complianceTreeUIContract.IsExpand;


                Boolean IfChildNodeExist = lstTreeData.Any(x => x.ParentNodeID == item.NodeID);
                if (IfChildNodeExist)
                {
                    e.Node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                }
                //By default selects the Pcakage Label as selected.
                if (item.UICode == RuleSetTreeNodeType.PackageLabel)
                {
                    e.Node.Selected = true;
                }

                //Customize tree node
                //BaseWebPage.CustomizeComplianceNode(e.Node, item, e.Node.Expanded);
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

        #region DropDown Events

        #endregion

        #endregion

        #region Methods

        #region Public Methods



        #endregion

        #region Private Methods

        private void BindTenant()
        {
            if (IsAdminLoggedIn == true)
            {
                Presenter.GetTenants();
                ddlTenant.DataSource = ListTenants;
                ddlTenant.DataBind();
            }
            else
            {
                divTenant.Visible = false;
            }
        }

        private void SetDefaultSelectedTenantId()
        {
            if (ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                if (SelectedTenantId.IsNullOrEmpty())
                {
                    ddlTenant.SelectedValue = Convert.ToString(TenantId);
                    SelectedTenantId = TenantId;
                }
                else //to handle node drag case
                {
                    ddlTenant.SelectedValue = Convert.ToString(SelectedTenantId);
                }

            }
        }

        private void BindShotSeriesTree()
        {
            treeShotSeries.DataTextField = "Value";
            treeShotSeries.DataFieldID = "NodeID";
            treeShotSeries.DataFieldParentID = "ParentNodeID";
            treeShotSeries.DataBind();
        }

        private void ReloadTreeData()
        {
            Presenter.GetTreeData();
            treeShotSeries.DataSource = lstTreeData;
        }

        /// <summary>
        /// bind tree and expand node for lazy loading
        /// </summary>
        /// <param name="GetFreshData"></param>
        private void BindHierarchyTree(Boolean GetFreshData)
        {
            if (GetFreshData)
            {
                ReloadTreeData();
            }
            BindShotSeriesTree();
            if (GetFreshData)
            {
                ExpandParentNodes();
            }
        }

        /// <summary>
        /// used for expanding parent nodes while binding tree load on demand.
        /// </summary>
        private void ExpandParentNodes()
        {
            if (hdnSelectedNode.Value != String.Empty)
            {
                var parentnode = treeShotSeries.FindNodeByValue(hdnSelectedNode.Value).ParentNode;
                while (parentnode != null)
                {
                    parentnode.Expanded = true;
                    parentnode = parentnode.ParentNode;
                }
            }
        }

        #endregion
        #endregion
    }
}

