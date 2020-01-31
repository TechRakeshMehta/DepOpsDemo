#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Linq;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Web.Services;
using CoreWeb.IntsofSecurityModel;
using Telerik.Web.UI;


#endregion

#endregion

namespace CoreWeb.SystemSetUp.Views
{
    public partial class InstitutionConfigurationPage : BaseWebPage, IInstitutionConfigurationPageView
    {
        #region Variables

        #region Private Variables

        private InstitutionConfigurationPagePresenter _presenter = new InstitutionConfigurationPagePresenter();
        private Int32 _tenantId;

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public InstitutionConfigurationPagePresenter Presenter
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
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IInstitutionConfigurationPageView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<GetDepartmentTree> lstTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("InstitutionConfigurationData") != null)
                    return (List<GetDepartmentTree>)(SysXWebSiteUtils.SessionService.GetCustomData("InstitutionConfigurationData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("InstitutionConfigurationData", value);
            }
        }

        public List<GetDepartmentTree> lstCurrentTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("CurrentInstitutionConfigurationData") != null)
                    return (List<GetDepartmentTree>)(SysXWebSiteUtils.SessionService.GetCustomData("CurrentInstitutionConfigurationData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("CurrentInstitutionConfigurationData", value);
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 NodeId
        {
            get { return (Int32)(ViewState["NodeId"]); }
            set { ViewState["NodeId"] = value; }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    //_tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        public Int32 SelectedTenant
        {
            set;
            get;
        }

        #endregion

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {

                base.Title = "Institution Configuration Details";
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
        /// Loads the page
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

                    ddlTenant.DataSource = ListTenants;
                    ddlTenant.DataBind();

                    if (!Presenter.IsAdminLoggedIn())
                    {
                        ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.TenantId);
                        ddlTenant.Enabled = false;
                        SelectedTenant = Convert.ToInt32(ddlTenant.SelectedValue);
                        WclSplitBar1.Visible = true;
                        dvTenant.Visible = false;
                        BindHierarchyTree(true, true);
                    }
                }
                else
                {
                    //for getting if page load is called by tree itself or right panel
                    Boolean asynchronousRequest = System.Web.UI.ScriptManager.GetCurrent(this).IsInAsyncPostBack;
                    if (ddlTenant.SelectedValue != String.Empty)
                    {
                        SelectedTenant = Convert.ToInt32(ddlTenant.SelectedValue);
                    }
                    Boolean ifTenantIsChanged = false; ;
                    if (!hdnTenantIsChanged.Value.IsNullOrEmpty())
                        ifTenantIsChanged = Convert.ToBoolean(hdnTenantIsChanged.Value);
                    WclSplitBar1.Visible = true;
                    BindHierarchyTree(asynchronousRequest, false);
                    if (ifTenantIsChanged)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenDefaultScreen();", true);
                    }
                }

                Presenter.OnViewLoaded();

                //Hiding top page
                CoreWeb.Shell.MasterPages.DefaultMaster masterPage = this.Master as CoreWeb.Shell.MasterPages.DefaultMaster;
                if (masterPage != null)
                {
                     masterPage.HideTitleBars(true);
                }
                base.SetModuleTitle("Institution Configuration");
                base.Title = "Institution Configuration Details";
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
        /// Page_Init event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                //Do not set master page's update panel mode as Conditional if client/tenant dropdown changes
                if (ddlTenant.UniqueID != Page.Request.Params["__EVENTTARGET"])
                {
                    if (this.IsPostBack)
                    {
                        var updatePanel = this.Master.FindControl("UpdatePanel1") as System.Web.UI.UpdatePanel;
                        updatePanel.UpdateMode = System.Web.UI.UpdatePanelUpdateMode.Conditional;
                        updatePanel.ChildrenAsTriggers = false;
                    }
                    else
                    {
                        if (!Presenter.IsAdminLoggedIn())
                        {
                            var updatePanel = this.Master.FindControl("UpdatePanel1") as System.Web.UI.UpdatePanel;
                            updatePanel.UpdateMode = System.Web.UI.UpdatePanelUpdateMode.Conditional;
                            updatePanel.ChildrenAsTriggers = false;
                        }
                    }
                }
                this.treeDepartment.NodeExpand += new RadTreeViewEventHandler(this.treeDepartment_NodeExpand);
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
        /// Binds the tree data as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            WclSplitBar1.Visible = true;
            SelectedTenant = Convert.ToInt32(ddlTenant.SelectedValue);
            BindHierarchyTree(true, true);
        }

        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {
            ddlTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        /// <summary>
        /// Bounds the data to tree view (which includes list of packages, categories, items and attributes).
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void TreeDepartment_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            try
            {
                //Gets the detail of a node.
                GetDepartmentTree item = (GetDepartmentTree)e.Node.DataItem;

                ////Sets the navigation URL, Color Code and Font Code as per node type.
                ComplianceTreeUIContract complianceTreeUIContract = Presenter.GetTreeNodeDetails(item.UICode, item.DataID, item.ParentDataID, item.ParentNodeID, item.NodeID, item.MappingID, item.Value, item.EntityID, item.DPM_IsAvailableForOrder);

                e.Node.NavigateUrl = complianceTreeUIContract.NavigateURL + "&SelectedTenantId=" + SelectedTenant;
                e.Node.Value = complianceTreeUIContract.Value + "#" + item.NodeID;
                e.Node.Font.Bold = complianceTreeUIContract.FontBold;
                e.Node.Target = "childpageframe";
                e.Node.Expanded = complianceTreeUIContract.IsExpand;
                e.Node.ForeColor = System.Drawing.Color.FromName(complianceTreeUIContract.ColorCode);

                Boolean IfChildNodeExist = lstTreeData.Any(x => x.ParentNodeID == item.NodeID);
                if (IfChildNodeExist)
                {
                    e.Node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                }

                ////By default selects the Institute as selected.
                if (item.Level == AppConsts.ONE)
                {
                    if (!this.IsPostBack)
                    {
                        if (!Presenter.IsAdminLoggedIn())
                        {
                            e.Node.Selected = true;
                            hdnDefaultScreen.Value = e.Node.NavigateUrl;
                        }
                    }
                    else
                    {
                        if (Presenter.IsAdminLoggedIn())
                        {
                            e.Node.Selected = true;
                            hdnDefaultScreen.Value = e.Node.NavigateUrl;
                        }
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

        protected void treeDepartment_NodeExpand(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            try
            {
                List<GetDepartmentTree> currentTreeNodeList = lstCurrentTreeData;
                List<GetDepartmentTree> childNodeList = new List<GetDepartmentTree>();

                String currentNodeValue = e.Node.Value;
                String nodeId = String.Empty;
                String uiCode = String.Empty;
                string nodeValue = String.Empty;
                string[] nodes = currentNodeValue.Split('#');
                if (!nodes.IsNullOrEmpty())
                {
                    nodeValue = nodes[0];
                    nodeId = nodes[1];
                    uiCode = nodeValue.Substring(nodeValue.IndexOf("_") + 1, nodeValue.Remove(0, nodeValue.IndexOf("_") + 1).IndexOf("_"));
                    GetDepartmentTree currentNode = null;
                    currentNode = lstTreeData.FirstOrDefault(x => x.NodeID == nodeId && x.UICode == uiCode);
                    childNodeList = lstTreeData.Where(x => x.ParentNodeID == currentNode.NodeID).ToList();
                    List<String> childNodeIds = childNodeList.Select(x => x.NodeID).ToList();
                    if (childNodeList.Count > AppConsts.NONE)
                    {
                        List<GetDepartmentTree> subChildNodeList = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeID)).ToList();
                        AddNodesToCurrentTreeData(currentTreeNodeList, subChildNodeList);
                    }
                    lstCurrentTreeData = currentTreeNodeList;
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

        #region Methods

        /// <summary>
        /// To bind tree data
        /// </summary>
        private void BindTree()
        {
            treeDepartment.DataSource = lstCurrentTreeData;
            treeDepartment.DataTextField = "Value";
            treeDepartment.DataFieldID = "NodeID";
            treeDepartment.DataFieldParentID = "ParentNodeID";
            treeDepartment.DataBind();
        }

        /// <summary>
        /// bind tree and expand node for lazy loading
        /// </summary>
        /// <param name="GetFreshData"></param>
        private void BindHierarchyTree(Boolean GetFreshData, Boolean ifNotPostBack)
        {
            if (GetFreshData)
            {
                ReloadTreeData();
            }
            BindTree();
            if (GetFreshData && !ifNotPostBack)
            {
                expandParentNodes();
            }
        }

        /// <summary>
        /// Add sub child nodes to existing tree data list.
        /// </summary>
        /// <param name="currentTreeNodeList"></param>
        /// <param name="nodeListToBeAdded"></param>
        private static void AddNodesToCurrentTreeData(List<GetDepartmentTree> currentTreeNodeList, List<GetDepartmentTree> nodeListToBeAdded)
        {
            foreach (GetDepartmentTree nodeToBeAdded in nodeListToBeAdded)
            {
                if (!currentTreeNodeList.Any(cond => cond.NodeID == nodeToBeAdded.NodeID && cond.ParentNodeID == nodeToBeAdded.ParentNodeID))
                    currentTreeNodeList.Add(nodeToBeAdded);
            }
        }

        /// <summary>
        /// used for expanding parent nodes while binding tree load on demand.
        /// </summary>
        private void expandParentNodes()
        {
            if (hdnSelectedNode.Value != String.Empty)
            {
                var parentnode = treeDepartment.FindNodeByValue(hdnSelectedNode.Value).ParentNode;
                while (parentnode != null)
                {
                    parentnode.Expanded = true;
                    parentnode = parentnode.ParentNode;
                }
            }
        }

        /// <summary>
        /// Used for getting treedata on demand i.e fetch only data which is required.
        /// </summary>
        private void ReloadTreeData()
        {
            Presenter.GetTreeData();
            lstCurrentTreeData = lstTreeData.Where(x => x.Level == 1 || x.Level == 2).ToList();
            String selectedNode = hdnSelectedNode.Value;
            if (selectedNode != String.Empty)
            {
                List<GetDepartmentTree> currentTreeNodeList = lstCurrentTreeData;
                List<GetDepartmentTree> parentNodeList = new List<GetDepartmentTree>();
                List<GetDepartmentTree> nodeListToBeAdded = new List<GetDepartmentTree>();
                String nodeId = String.Empty;
                String uiCode = String.Empty;
                string nodeValue = String.Empty;
                string[] nodes = selectedNode.Split('#');
                if (!nodes.IsNullOrEmpty())
                {
                    nodeValue = nodes[0];
                    nodeId = nodes[1];
                    uiCode = selectedNode.Substring(selectedNode.IndexOf("_") + 1, selectedNode.Remove(0, selectedNode.IndexOf("_") + 1).IndexOf("_"));
                    GetDepartmentTree currentNode = lstTreeData.FirstOrDefault(x => x.NodeID == nodeId && x.UICode == uiCode);

                    GetDepartmentTree parentNodes = null;
                    parentNodes = lstTreeData.FirstOrDefault(x => x.NodeID == currentNode.ParentNodeID);
                    if (parentNodes != null)
                    {
                        //get All the Parents Node of current Node
                        parentNodeList.Add(parentNodes);
                        while (parentNodes.ParentDataID != null && parentNodes.Level != 1)
                        {
                            parentNodes = lstTreeData.FirstOrDefault(x => x.NodeID == parentNodes.ParentNodeID);
                            parentNodeList.Add(parentNodes);
                        }
                        List<String> parentNodeIds = null;
                        parentNodeIds = parentNodeList.Select(x => x.NodeID).ToList();

                        //get all the immidiate child of parents
                        List<GetDepartmentTree> childNodeList = null;
                        childNodeList = lstTreeData.Where(x => parentNodeIds.Contains(x.ParentNodeID)).ToList();
                        List<String> childNodeIds = null;
                        childNodeIds = childNodeList.Select(x => x.NodeID).ToList();
                        nodeListToBeAdded = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeID)).ToList();
                        AddNodesToCurrentTreeData(currentTreeNodeList, nodeListToBeAdded);
                    }
                    lstCurrentTreeData = currentTreeNodeList;
                }
            }
        }
        #endregion


    }
}

