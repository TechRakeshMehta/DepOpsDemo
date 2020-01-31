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

namespace CoreWeb.BkgSetup.Views
{
    public partial class SetupInstitutionHierarchyBkg : BaseWebPage, ISetupInstitutionHierarchyBkgView
    {
        #region Variables

        #region Private Variables

        private SetupInstitutionHierarchyBkgPresenter _presenter = new SetupInstitutionHierarchyBkgPresenter();
        private Int32 _tenantId;

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public SetupInstitutionHierarchyBkgPresenter Presenter
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
        public ISetupInstitutionHierarchyBkgView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> lstTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeData") != null)
                    return (List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract>)(SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("ComplianceTreeData", value);
            }
        }

        public List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> lstCurrentTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeCurrentData") != null)
                    return (List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract>)(SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeCurrentData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("ComplianceTreeCurrentData", value);
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
        public Boolean IsAvailableforOrder
        {
            get { return (Boolean)(ViewState["IsAvailableforOrder"]); }
            set { ViewState["IsAvailableforOrder"] = value; }
        }

        //Start UAT - 3157/
        Int32 ISetupInstitutionHierarchyBkgView.PreferredSelectedTenantID
        {
            get
            {
                if (!ViewState["PreferredSelectedTenantID"].IsNull())
                {
                    return (Int32)ViewState["PreferredSelectedTenantID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PreferredSelectedTenantID"] = value;
            }
        }
        public Boolean IsPackageBundleAvailableforOrder
        {
            get { return (Boolean)(ViewState["IsPackageBundleAvailableforOrder"]); }
            set { ViewState["IsPackageBundleAvailableforOrder"] = value; }
        }
        //END UAT - 3157

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Loads the page
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!hdnIsAvailableforOrder.Value.IsNullOrEmpty())
                {
                    IsAvailableforOrder = Convert.ToBoolean(hdnIsAvailableforOrder.Value);
                }
                if (!hdnIsPackageBundleAvailableforOrder.Value.IsNullOrEmpty())
                {
                    IsPackageBundleAvailableforOrder = Convert.ToBoolean(hdnIsPackageBundleAvailableforOrder.Value);
                }

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();

                    ddlTenant.DataSource = ListTenants;
                    ddlTenant.DataBind();
                    //WclSplitBar1.Visible = false;
                    /*UAT-3157*/
                    GetPreferredSelectedTenant();
                    /*END UAT-3157*/

                    if (!Presenter.IsAdminLoggedIn())
                    {
                        ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.TenantId);
                        ddlTenant.Enabled = false;
                        SelectedTenant = Convert.ToInt32(ddlTenant.SelectedValue);
                        //WclSplitBar1.Visible = true;
                        paneTenant.Visible = false;
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
                    //WclSplitBar1.Visible = true;
                    if (ifTenantIsChanged)
                    {
                        IsAvailableforOrder = true;
                        hdnIsAvailableforOrder.Value = "true";
                        Session["IsPackagesNotAvailableForOrder"] = null;

                        IsPackageBundleAvailableforOrder = true;
                        hdnIsPackageBundleAvailableforOrder.Value = "true";
                        Session["IsPackageBundleAvailableForOrder"] = null;
                    }
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

                base.Title = "Institution Hierarchy";
                base.SetModuleTitle("BackGround Package Mapping");
                base.SetPageTitle("Institution Hierarchy");
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
            //WclSplitBar1.Visible = true;
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
                INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract item = (INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract)e.Node.DataItem;

                ////Sets the navigation URL, Color Code and Font Code as per node type.
                ComplianceTreeUIContract complianceTreeUIContract = Presenter.GetTreeNodeDetails(item.UICode, item.DataID, item.ParentDataID, item.ParentNodeID, item.NodeID, item.MappingID, item.Value, item.EntityID, item.IsPackageAvailableForOrder, item.IsPackageBundleAvailableForOrder,item.PackageColorCode);

                e.Node.NavigateUrl = complianceTreeUIContract.NavigateURL + "&SelectedTenantId=" + SelectedTenant;
                e.Node.Value = complianceTreeUIContract.Value + "#" + item.NodeID;

                e.Node.Font.Bold = complianceTreeUIContract.FontBold;
                e.Node.Target = "childpageframe";
                e.Node.Expanded = complianceTreeUIContract.IsExpand;

                if (!String.IsNullOrEmpty(complianceTreeUIContract.ColorCode))
                {
                    e.Node.ForeColor = System.Drawing.Color.FromName(complianceTreeUIContract.ColorCode);
                }
                if (item.IsPackageAvailableForOrder.HasValue && !item.IsPackageAvailableForOrder.Value)
                {
                    e.Node.ForeColor = System.Drawing.Color.FromName("Red");
                }
                
                if (item.UICode == PackageBundlePackageType.COMPLIANCEPACKAGEBUNDLE.GetStringValue() || item.UICode == PackageBundlePackageType.SCREENINGPACKAGEBUNDLE.GetStringValue())
                {
                    e.Node.EnableContextMenu = false;
                    e.Node.Enabled = false;
                    e.Node.CssClass = "DisableNode";
                    e.Node.ForeColor = (item.IsPackageAvailableForOrder.HasValue && item.IsPackageAvailableForOrder.Value == false) ? System.Drawing.Color.Red : System.Drawing.Color.DarkOrange;
                }
                if (item.UICode == RuleSetTreeNodeType.Contact)
                    e.Node.ImageUrl = "~/App_Themes/Default/images/icons/group.png";
                else if (item.UICode == RuleSetTreeNodeType.CompliancePackage)
                    e.Node.ImageUrl = "~/App_Themes/Default/images/icons/Package.png";
                else if (item.UICode == RuleSetTreeNodeType.Service)
                    e.Node.ImageUrl = "~/App_Themes/Default/images/icons/cat.gif";
                else if (item.UICode == RuleSetTreeNodeType.ServiceItem)
                    e.Node.ImageUrl = "~/App_Themes/Default/images/icons/itm.gif";
                else if (item.UICode == RuleSetTreeNodeType.RuleSet)
                    e.Node.ImageUrl = "~/App_Themes/Default/images/icons/rset.gif";
                else if (item.UICode == RuleSetTreeNodeType.Rule)
                    e.Node.ImageUrl = "~/App_Themes/Default/images/icons/rul.gif";

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
                            //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenDefaultScreen();", true);
                        }
                    }
                    else
                    {
                        if (Presenter.IsAdminLoggedIn())
                        {
                            e.Node.Selected = true;
                            hdnDefaultScreen.Value = e.Node.NavigateUrl;
                            //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenDefaultScreen();", true);
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
                List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> currentTreeNodeList = lstCurrentTreeData;
                if (IsAvailableforOrder)
                {
                    currentTreeNodeList = currentTreeNodeList.Where(x => x.IsPackageAvailableForOrder == true || x.IsPackageAvailableForOrder.IsNull()).ToList();
                }
                List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> childNodeList = new List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract>();

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
                    INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract currentNode = null;
                    currentNode = lstTreeData.FirstOrDefault(x => x.NodeID == nodeId && x.UICode == uiCode);
                    childNodeList = lstTreeData.Where(x => x.ParentNodeID == currentNode.NodeID).ToList();
                    if (IsAvailableforOrder)
                    {
                        childNodeList = childNodeList.Where(x => x.IsPackageAvailableForOrder == true || x.IsPackageAvailableForOrder.IsNull()).ToList();
                    }
                    if (!IsPackageBundleAvailableforOrder)
                    {
                        childNodeList.RemoveAll(x => x.IsPackageBundleAvailableForOrder == true);
                    }
                    List<String> childNodeIds = childNodeList.Select(x => x.NodeID).ToList();
                    if (childNodeList.Count > AppConsts.NONE)
                    {
                        List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> subChildNodeList = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeID)).ToList();
                        if (IsAvailableforOrder)
                        {
                            subChildNodeList = subChildNodeList.Where(x => x.IsPackageAvailableForOrder == true || x.IsPackageAvailableForOrder.IsNull()).ToList();
                        }
                        if (!IsPackageBundleAvailableforOrder)
                        {
                            subChildNodeList.RemoveAll(x => x.IsPackageBundleAvailableForOrder == true);
                        }

                        if (subChildNodeList.IsNotNull() && subChildNodeList.Count > 0)
                            subChildNodeList = subChildNodeList.OrderBy(x => x.Value).OrderBy(x => x.UICode).ToList();

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
        private static void AddNodesToCurrentTreeData(List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> currentTreeNodeList, List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> nodeListToBeAdded)
        {
            foreach (INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract nodeToBeAdded in nodeListToBeAdded)
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
            if (IsAvailableforOrder)
            {
                lstCurrentTreeData = lstCurrentTreeData.Where(x => x.IsPackageAvailableForOrder == true || x.IsPackageAvailableForOrder.IsNull()).ToList();
            }
            String selectedNode = hdnSelectedNode.Value;
            if (selectedNode != String.Empty)
            {
                List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> currentTreeNodeList = lstCurrentTreeData;
                if (IsAvailableforOrder)
                {
                    currentTreeNodeList = currentTreeNodeList.Where(x => x.IsPackageAvailableForOrder == true || x.IsPackageAvailableForOrder.IsNull()).ToList();
                }
                if (!IsPackageBundleAvailableforOrder)
                {
                    currentTreeNodeList.RemoveAll(x => x.IsPackageBundleAvailableForOrder == true);
                }

                List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> parentNodeList = new List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract>();
                List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> nodeListToBeAdded = new List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract>();
                String nodeId = String.Empty;
                String uiCode = String.Empty;
                string nodeValue = String.Empty;
                string[] nodes = selectedNode.Split('#');
                if (!nodes.IsNullOrEmpty())
                {
                    nodeValue = nodes[0];
                    nodeId = nodes[1];
                    uiCode = selectedNode.Substring(selectedNode.IndexOf("_") + 1, selectedNode.Remove(0, selectedNode.IndexOf("_") + 1).IndexOf("_"));
                    INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract currentNode = lstTreeData.FirstOrDefault(x => x.NodeID == nodeId && x.UICode == uiCode);

                    INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract parentNodes = null;
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
                        List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> childNodeList = null;
                        childNodeList = lstTreeData.Where(x => parentNodeIds.Contains(x.ParentNodeID)).ToList();
                        if (IsAvailableforOrder)
                        {
                            childNodeList = childNodeList.Where(x => x.IsPackageAvailableForOrder == true || x.IsPackageAvailableForOrder.IsNull()).ToList();
                        }
                        List<String> childNodeIds = null;
                        childNodeIds = childNodeList.Select(x => x.NodeID).ToList();
                        nodeListToBeAdded = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeID)).ToList();
                        if (IsAvailableforOrder)
                        {
                            nodeListToBeAdded = nodeListToBeAdded.Where(x => x.IsPackageAvailableForOrder == true || x.IsPackageAvailableForOrder.IsNull()).ToList();
                        }
                        AddNodesToCurrentTreeData(currentTreeNodeList, nodeListToBeAdded);
                    }
                    else
                    {
                        if (IsPackageBundleAvailableforOrder)
                        {
                            List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> childNodeList = null;
                            childNodeList = lstTreeData.Where(x => x.ParentNodeID == nodeId && x.IsPackageBundleAvailableForOrder == true).ToList();
                            if (childNodeList.IsNotNull() && childNodeList.Count > 0)
                            {
                                AddNodesToCurrentTreeData(currentTreeNodeList, childNodeList);
                            }
                        }
                    }
                    lstCurrentTreeData = currentTreeNodeList;
                }
            }
        }


        #region UAT-3157:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenant.IsNullOrEmpty() || CurrentViewContext.SelectedTenant == AppConsts.NONE)
            {
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    hdnIsDefaultPreferredTenant.Value = "true";
                    ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    CurrentViewContext.SelectedTenant = Convert.ToInt32(ddlTenant.SelectedValue);
                    //for getting if page load is called by tree itself or right panel
                    Boolean asynchronousRequest = true;
                    if (ddlTenant.SelectedValue != String.Empty)
                    {
                        SelectedTenant = Convert.ToInt32(ddlTenant.SelectedValue);
                    }
                    Boolean ifTenantIsChanged = true; ;
                    //WclSplitBar1.Visible = true;
                    if (ifTenantIsChanged)
                    {
                        IsAvailableforOrder = true;
                        hdnIsAvailableforOrder.Value = "true";
                        Session["IsPackagesNotAvailableForOrder"] = null;

                        IsPackageBundleAvailableforOrder = true;
                        hdnIsPackageBundleAvailableforOrder.Value = "true";
                        Session["IsPackageBundleAvailableForOrder"] = null;
                    }
                    BindHierarchyTree(asynchronousRequest, false);

                    if (ifTenantIsChanged)
                    {
                        var parentNodeData = lstCurrentTreeData.Where(cond => cond.ParentDataID.IsNullOrEmpty() && cond.ParentNodeID.IsNullOrEmpty()).FirstOrDefault();
                        ComplianceTreeUIContract complianceTreeUIContract = Presenter.GetTreeNodeDetails(parentNodeData.UICode, parentNodeData.DataID, parentNodeData.ParentDataID, parentNodeData.ParentNodeID, parentNodeData.NodeID, parentNodeData.MappingID, parentNodeData.Value, parentNodeData.EntityID, parentNodeData.IsPackageAvailableForOrder, parentNodeData.IsPackageBundleAvailableForOrder, parentNodeData.PackageColorCode);
                        hdnDefaultScreen.Value = complianceTreeUIContract.NavigateURL + "&SelectedTenantId=" + CurrentViewContext.SelectedTenant + "&PermissionCode=" + parentNodeData.PermissionCode;
                        //  hdnDefaultScreen.Value = "InstituteHierarchyNodePackage.aspx" + "?Id=1" + "&ParentID=" + "&NodeID=Start_IHN_1" + "&RecordTypeCode=AAAF" + "&TenantId=" + CurrentViewContext.SelectedTenant + "&PermissionCode= ";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenDefaultScreen();", true);
                    }
                }
            }
        }
        #endregion
        #endregion


    }
}

