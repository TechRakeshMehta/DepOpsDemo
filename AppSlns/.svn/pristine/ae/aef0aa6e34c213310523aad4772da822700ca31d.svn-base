using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class SetupUniversalMapping : BaseWebPage, ISetupUniversalMappingView
    {
        #region Variables

        #region Private Variables

        private SetupUniversalMappingPresenter _presenter = new SetupUniversalMappingPresenter();

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public SetupUniversalMappingPresenter Presenter
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

        public ISetupUniversalMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 ISetupUniversalMappingView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        List<UniversalMappingContract> ISetupUniversalMappingView.lstTreeData { get; set; }

        #endregion

        #endregion

        #region Events

        #region Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindHierarchyTree(true);
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenDefaultScreen();", true);
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
                base.Title = "Universal Mapping";
                base.SetModuleTitle("Universal Mapping Setup");
                base.SetPageTitle("Universal Mapping");
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

        #region Tree Event
        protected void treeUniversalMapping_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            try
            {
                //Gets the detail of a node.
                UniversalMappingContract item = (UniversalMappingContract)e.Node.DataItem;

                //Sets the navigation URL, Color Code and Font Code as per node type.
                ComplianceTreeUIContract complianceTreeUIContract = Presenter.GetTreeNodeDetails(item.UICode, item.DataID, item.ParentDataID, item.ParentNodeID, item.NodeID);

                e.Node.NavigateUrl = complianceTreeUIContract.NavigateURL;
                e.Node.Value = item.NodeID;
                e.Node.Target = "childpageframe";
                e.Node.Expanded = complianceTreeUIContract.IsExpand;


                Boolean IfChildNodeExist = CurrentViewContext.lstTreeData.Any(x => x.ParentNodeID == item.NodeID);
                if (IfChildNodeExist)
                {
                    e.Node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                }
                //By default selects the Pcakage Label as selected.
                //if (item.UICode == RuleSetTreeNodeType.PackageLabel)
                //{
                //    e.Node.Selected = true;
                //}
                if (item.Level == AppConsts.NONE)
                {
                    if (!this.IsPostBack)
                    {
                        e.Node.Selected = true;
                        hdnSelectedNodeScreen.Value = hdnDefaultScreen.Value = e.Node.NavigateUrl;
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenDefaultScreen();", true);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenSelectedNodeScreen();", true);
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

        #endregion

        #region Methods

        #region Public Methdod

        #endregion

        #region Private Method

        private void BindShotSeriesTree()
        {
            treeUniversalMapping.DataTextField = "Value";
            treeUniversalMapping.DataFieldID = "NodeID";
            treeUniversalMapping.DataFieldParentID = "ParentNodeID";
            treeUniversalMapping.DataBind();
        }

        private void ReloadTreeData()
        {
            Presenter.GetTreeData();
            treeUniversalMapping.DataSource = CurrentViewContext.lstTreeData;
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
                var parentnode = treeUniversalMapping.FindNodeByValue(hdnSelectedNode.Value);
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