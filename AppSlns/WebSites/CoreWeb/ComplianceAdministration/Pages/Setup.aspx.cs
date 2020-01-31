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
    public partial class Setup : BaseWebPage, ISetupView
    {
        #region Variables

        #region Private Variables

        private SetupPresenter _presenter = new SetupPresenter();
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


        public SetupPresenter Presenter
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

        public List<GetRuleSetTree> lstTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeData") != null)
                    return (List<GetRuleSetTree>)(SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeData"));
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

        /// <summary>
        ///  Gets and Sets list of compliance packages.
        /// </summary>
        //public List<LookupContract> ListCompliancePackages
        //{
        //    set;
        //    get;
        //}
        public List<CompliancePackage> ListCompliancePackages
        {
            set;
            get;
        }

        public List<Int32> SelectedPackageIDList
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < ddlPackage.Items.Count; i++)
                {
                    if (ddlPackage.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(ddlPackage.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < ddlPackage.Items.Count; i++)
                {
                    ddlPackage.Items[i].Checked = value.Contains(Convert.ToInt32(ddlPackage.Items[i].Value));
                }

            }
        }

        /*UAT - 3032*/

        public ISetupView CurrentViewContext
        {
            get { return this; }
        }

        Int32 ISetupView.PreferredSelectedTenantID
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
        /* END UAT - 3032*/
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
                this.treePackages.NodeExpand += new RadTreeViewEventHandler(this.treePackages_NodeExpand);
                this.treePackages.NodeCollapse += new RadTreeViewEventHandler(this.treePackages_NodeCollapse);
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
                HandleComplianceNodeMovedCase();

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindTenant();
                    /*UAT-3032*/
                    if (IsAdminLoggedIn == true)
                        GetPreferredSelectedTenant();
                    /*END UAT-3032*/
                    //UAT-1116: Package selection combo box on package screens
                    BindPackage();
                    SetDefaultSelectedTenantId();
                    //Presenter.GetTreeData();
                    //lstCurrentTreeData = lstTreeData.Where(x => (x.UICode == RuleSetTreeNodeType.PackageLabel || x.UICode == RuleSetTreeNodeType.Package || x.UICode == RuleSetTreeNodeType.CategoryLabel)).ToList();
                }
                Boolean asynchronousRequest = System.Web.UI.ScriptManager.GetCurrent(this).IsInAsyncPostBack;
                Boolean IfDragEventIsFired = hdnIfDragEventIsFired.Value != String.Empty ? Convert.ToBoolean(hdnIfDragEventIsFired.Value) : false;
                hdnIfDragEventIsFired.Value = String.Empty;

                //UAT-1116: Package selection combo box on package screens
                //Added if else condition for hdnIsSearchClicked field
                if (!String.IsNullOrEmpty(hdnIsSearchClicked.Value) && SelectedPackageIDList.Count > AppConsts.NONE)
                {
                    if (!IfDragEventIsFired)
                    {
                        BindComplianceTree(asynchronousRequest);
                    }
                    else
                    {
                        BindTreePackages();
                    }
                }
                else
                {
                    treePackages.DataSource = new List<GetRuleSetTree>();
                    treePackages.DataBind();
                }
                //End UAT-1116
                Presenter.OnViewLoaded();

                //Hiding top page
                CoreWeb.Shell.MasterPages.DefaultMaster masterPage = this.Master as CoreWeb.Shell.MasterPages.DefaultMaster;
                if (masterPage != null)
                {
                    masterPage.HideTitleBars(true);
                }

                base.Title = "Manage Mappings";
                base.SetModuleTitle("Compliance Setup");
                base.SetPageTitle("Manage Mappings");
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
                BindPackage();
                treePackages.DataSource = new List<GetRuleSetTree>();
                treePackages.DataBind();
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
        private void BindComplianceTree(Boolean GetFreshData)
        {
            if (GetFreshData)
            {
                ReloadTreeData();
            }
            BindTreePackages();
            if (GetFreshData)
            {
                expandParentNodes();
            }
        }


        #endregion

        #region TreeView Events

        /// <summary>
        /// Bounds the data to tree view (which includes list of packages, categories, items and attributes).
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void treePackages_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            try
            {
                //Gets the detail of a node.
                GetRuleSetTree item = (GetRuleSetTree)e.Node.DataItem;

                //Sets the navigation URL, Color Code and Font Code as per node type.
                ComplianceTreeUIContract complianceTreeUIContract = Presenter.GetTreeNodeDetails(item.UICode, item.DataID, item.ParentDataID, item.ParentNodeID, item.NodeID);

                e.Node.NavigateUrl = complianceTreeUIContract.NavigateURL;
                e.Node.Value = complianceTreeUIContract.Value + "_" + item.NodeID;
                //e.Node.ForeColor = System.Drawing.Color.FromName(complianceTreeUIContract.ColorCode);
                //e.Node.Font.Bold = complianceTreeUIContract.FontBold;
                e.Node.Target = "childpageframe";
                e.Node.Expanded = complianceTreeUIContract.IsExpand;
                e.Node.ForeColor = System.Drawing.Color.Black;

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
                if (item.UICode == RuleSetTreeNodeType.Category || item.UICode == RuleSetTreeNodeType.Item || item.UICode == RuleSetTreeNodeType.Attribute)
                {
                    e.Node.AllowDrag = true;
                    e.Node.AllowDrop = true;
                }
                else
                {
                    e.Node.AllowDrag = false;
                    e.Node.AllowDrop = false;
                }
                //Package 
                if (item.UICode.Equals(RuleSetTreeNodeType.Package))
                {
                    e.Node.ContextMenuID = "mnuTreePackage";
                }
                //Customize tree node
                BaseWebPage.CustomizeComplianceNode(e.Node, item, e.Node.Expanded);
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

        protected void treePackages_NodeDrop(object sender, Telerik.Web.UI.RadTreeNodeDragDropEventArgs e)
        {
            try
            {
                string sourceID = e.SourceDragNode.NavigateUrl.Substring(e.SourceDragNode.NavigateUrl.IndexOf("=") + 1, e.SourceDragNode.NavigateUrl.IndexOf("&") - e.SourceDragNode.NavigateUrl.IndexOf("=") - 1);
                string destinationID = e.DestDragNode.NavigateUrl.Substring(e.DestDragNode.NavigateUrl.IndexOf("=") + 1, e.DestDragNode.NavigateUrl.IndexOf("&") - e.DestDragNode.NavigateUrl.IndexOf("=") - 1);
                string parentID = e.SourceDragNode.NavigateUrl.Substring(e.SourceDragNode.NavigateUrl.LastIndexOf("=") + 1);
                Int32 sourceId = Convert.ToInt32(sourceID);
                Int32 destinationId = Convert.ToInt32(destinationID);
                Int32 displayOrder = 1;
                Int32 parentId = Convert.ToInt32(parentID);
                Int32 level = e.SourceDragNode.Level;
                switch (level)
                {
                    case AppConsts.THREE:

                        List<CompliancePackageCategory> packageCategoryList = Presenter.getCompliancePackageCategoryByDisplayOrder(parentId);

                        if (packageCategoryList != null)
                        {
                            hdnIfDragPositonAbove.Value = Convert.ToString(true);
                            foreach (CompliancePackageCategory packageCategory in packageCategoryList)
                            {
                                Int32 primaryID = packageCategory.CPC_CategoryID;
                                displayOrder = ApplyDisplayOrderLogic(primaryID, sourceId, destinationId, parentId, displayOrder, level);
                            }
                        }
                        break;

                    case AppConsts.FIVE:
                        List<ComplianceCategoryItem> categoryItemList = Presenter.getComplianceCategoryItemByDisplayOrder(parentId);
                        if (categoryItemList != null)
                        {
                            hdnIfDragPositonAbove.Value = Convert.ToString(true);
                            foreach (ComplianceCategoryItem categoryItem in categoryItemList)
                            {
                                displayOrder = ApplyDisplayOrderLogic(categoryItem.CCI_ItemID, sourceId, destinationId, parentId, displayOrder, level);
                            }
                        }
                        break;

                    case AppConsts.SEVEN:
                        List<ComplianceItemAttribute> itemAttributeList = Presenter.getComplianceItemAttributeByDisplayOrder(parentId);
                        if (itemAttributeList != null)
                        {
                            hdnIfDragPositonAbove.Value = Convert.ToString(true);
                            foreach (ComplianceItemAttribute itemAttribute in itemAttributeList)
                            {
                                displayOrder = ApplyDisplayOrderLogic(itemAttribute.CIA_AttributeID, sourceId, destinationId, parentId, displayOrder, level);
                            }
                        }
                        break;

                    default:
                        break;
                }

                BindComplianceTree(GetFreshData: true);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "NavigateToSelectedNode('" + e.SourceDragNode.NavigateUrl + "');", true);


            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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


        protected int ApplyDisplayOrderLogic(Int32 primaryID, Int32 sourceId, Int32 destinationId, Int32 parentId, Int32 displayOrder, Int32 level)
        {
            if (primaryID == sourceId)
            {
                hdnIfDragPositonAbove.Value = Convert.ToString(false);
                return displayOrder;
            }
            else if (primaryID == destinationId)
            {
                if (Convert.ToBoolean(hdnIfDragPositonAbove.Value) == true)
                {
                    Presenter.updateDisplayOrder(parentId, sourceId, displayOrder, level);
                    displayOrder++;
                    Presenter.updateDisplayOrder(parentId, destinationId, displayOrder, level);
                    displayOrder++;
                }
                else
                {
                    Presenter.updateDisplayOrder(parentId, destinationId, displayOrder, level);
                    displayOrder++;
                    Presenter.updateDisplayOrder(parentId, sourceId, displayOrder, level);
                    displayOrder++;
                }
                // hdnIfDragPositonAbove.Value = Convert.ToString(true);
            }
            else
            {
                Presenter.updateDisplayOrder(parentId, primaryID, displayOrder, level);
                displayOrder++;
            }
            return displayOrder;
        }

        protected void treePackages_NodeExpand(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            List<GetRuleSetTree> currentTreeNodeList = lstCurrentTreeData;
            List<GetRuleSetTree> childNodeList = new List<GetRuleSetTree>();

            String currentNodeValue = e.Node.Value;
            String nodeId = String.Empty;
            String uiCode = String.Empty;

            nodeId = currentNodeValue.Substring(currentNodeValue.LastIndexOf("_") + 1);
            uiCode = currentNodeValue.Substring(currentNodeValue.IndexOf("_") + 1, currentNodeValue.Remove(0, currentNodeValue.IndexOf("_") + 1).IndexOf("_"));
            GetRuleSetTree currentNode = null;
            currentNode = lstTreeData.FirstOrDefault(x => x.NodeID == nodeId && x.UICode == uiCode);
            childNodeList = lstTreeData.Where(x => x.ParentNodeID == currentNode.NodeID).ToList();
            List<String> childNodeIds = childNodeList.Select(x => x.NodeID).ToList();
            if (childNodeList.Count > AppConsts.NONE)
            {
                List<GetRuleSetTree> subChildNodeList = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeID)).ToList();
                AddNodesToCurrentTreeData(currentTreeNodeList, subChildNodeList);
            }
            lstCurrentTreeData = currentTreeNodeList;
        }

        protected void treePackages_NodeCollapse(object sender, RadTreeNodeEventArgs e)
        {
            List<GetRuleSetTree> currentTreeNodeList = lstCurrentTreeData;
            List<GetRuleSetTree> childNodeList = new List<GetRuleSetTree>();
            Int32 nodeDataId = 0;
            String uiCode = String.Empty;
            string navigateUrl = e.Node.NavigateUrl;
            nodeDataId = Convert.ToInt32(navigateUrl.Substring(navigateUrl.IndexOf("=") + 1, (navigateUrl.IndexOf("&") - navigateUrl.IndexOf("=") - 1)));
            uiCode = navigateUrl.Substring(navigateUrl.IndexOf("_") + 1, navigateUrl.Remove(0, navigateUrl.IndexOf("_") + 1).IndexOf("_"));
            if (uiCode.StartsWith("L"))
            {
                GetRuleSetTree currentNode = lstTreeData.FirstOrDefault(x => x.ParentDataID == nodeDataId && x.UICode == uiCode);
                childNodeList = lstTreeData.Where(x => x.ParentNodeID == currentNode.NodeID).ToList();
                List<String> childNodeIds = childNodeList.Select(x => x.NodeID).ToList();
                if (childNodeList.Count > AppConsts.NONE)
                {
                    List<GetRuleSetTree> childLabelNodeList = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeID)).ToList();
                    foreach (GetRuleSetTree childLabel in childLabelNodeList)
                    {
                        currentTreeNodeList.Remove(childLabel);
                    }
                }
                lstCurrentTreeData = currentTreeNodeList;
                BindTreePackages();
            }
            else
            {
                GetRuleSetTree currentNode = lstTreeData.FirstOrDefault(x => x.DataID == nodeDataId && x.UICode == uiCode);
                childNodeList = lstTreeData.Where(x => x.ParentNodeID == currentNode.NodeID).ToList();
                List<String> childNodeIds = childNodeList.Select(x => x.NodeID).ToList();
                if (childNodeList.Count > AppConsts.NONE)
                {
                    List<GetRuleSetTree> subChildNodeList = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeID)).ToList();
                    foreach (GetRuleSetTree childLabel in subChildNodeList)
                    {
                        currentTreeNodeList.Remove(childLabel);
                    }
                }
                lstCurrentTreeData = currentTreeNodeList;
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
                //UAT-1116: Package selection combo box on package screens
                //paneTenant.Visible = false;
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

        private void BindTreePackages()
        {
            treePackages.DataSource = lstCurrentTreeData;
            var jj = lstTreeData.WhereSelect(x => x.UICode == "LPAK");
            treePackages.DataTextField = "Value";
            treePackages.DataFieldID = "NodeID";
            treePackages.DataFieldParentID = "ParentNodeID";
            treePackages.DataBind();
        }

        /// <summary>
        /// Used for getting treedata on demand i.e fetch only data which is required.
        /// </summary>
        private void ReloadTreeData()
        {
            Presenter.GetTreeData();
            lstCurrentTreeData = lstTreeData.Where(x => (x.UICode == RuleSetTreeNodeType.PackageLabel || x.UICode == RuleSetTreeNodeType.Package || x.UICode == RuleSetTreeNodeType.CategoryLabel)).ToList();
            if (hdnStoredData.Value != String.Empty)
            {
                var dict = new JavaScriptSerializer().Deserialize<Dictionary<String, String>>(hdnStoredData.Value);
                var dataId = dict["DataId"];
                var parentDataId = dict["ParentDataId"];
                var uiCode = dict["UICode"];
                GetRuleSetTree selectedNodeData = new GetRuleSetTree();
                if (uiCode == RuleSetTreeNodeType.Rule)
                    selectedNodeData = lstTreeData.Find(x => x.DataID == Convert.ToInt32(dataId) && x.ParentDataID == Convert.ToInt32(parentDataId) && x.UICode == uiCode);
                else
                {
                    String hPath = String.Empty;
                    if (uiCode == RuleSetTreeNodeType.Category)
                    {
                        hPath = String.Format("1-{0}|2-{1}", parentDataId, dataId);
                    }
                    else if (uiCode == RuleSetTreeNodeType.Item)
                    {
                        var packageId = dict["PackageId"];
                        hPath = String.Format("1-{0}|2-{1}|3-{2}", packageId, parentDataId, dataId);
                    }
                    else if (uiCode == RuleSetTreeNodeType.Attribute)
                    {
                        var packageId = dict["PackageId"];
                        var categoryId = dict["CategoryId"];
                        hPath = String.Format("1-{0}|2-{1}|3-{2}|4-{3}", packageId, categoryId, parentDataId, dataId);
                    }
                    selectedNodeData = lstTreeData.Find(x => x.DataID == Convert.ToInt32(dataId) && x.ParentDataID == Convert.ToInt32(parentDataId) && x.UICode == uiCode && x.HID == hPath);
                }

                Dictionary<String, Int32> uniqueNodeData = Presenter.GetUniqueKeyPackageIDForNode(selectedNodeData.NodeID, Convert.ToInt32(dataId), uiCode);
                String uniqueNodeId = String.Empty;
                uniqueNodeId = uniqueNodeData.FirstOrDefault().Key;
                hdnStoredData.Value = uniqueNodeId + '_' + selectedNodeData.NodeID;
                hdnSelectedNode.Value = uniqueNodeId + '_' + selectedNodeData.NodeID;
            }
            String selectedNode = hdnSelectedNode.Value;
            if (selectedNode != String.Empty)
            {
                List<GetRuleSetTree> currentTreeNodeList = lstCurrentTreeData;
                List<GetRuleSetTree> parentNodeList = new List<GetRuleSetTree>();
                List<GetRuleSetTree> nodeListToBeAdded = new List<GetRuleSetTree>();
                String nodeId = String.Empty;
                String uiCode = String.Empty;

                nodeId = selectedNode.Substring(selectedNode.LastIndexOf("_") + 1);
                uiCode = selectedNode.Substring(selectedNode.IndexOf("_") + 1, selectedNode.Remove(0, selectedNode.IndexOf("_") + 1).IndexOf("_"));
                GetRuleSetTree currentNode = lstTreeData.FirstOrDefault(x => x.NodeID == nodeId && x.UICode == uiCode);

                GetRuleSetTree parentNode = null;
                parentNode = lstTreeData.FirstOrDefault(x => x.NodeID == currentNode.ParentNodeID);
                if (parentNode != null)
                {
                    parentNodeList.Add(parentNode);
                    while (parentNode.ParentDataID != null && parentNode.UICode != RuleSetTreeNodeType.PackageLabel)
                    {
                        parentNode = lstTreeData.FirstOrDefault(x => x.NodeID == parentNode.ParentNodeID);
                        parentNodeList.Add(parentNode);
                    }
                    List<String> childNodeIdsIds = null;
                    childNodeIdsIds = parentNodeList.Select(x => x.NodeID).ToList();
                    List<GetRuleSetTree> childNodeList = null;
                    childNodeList = lstTreeData.Where(x => childNodeIdsIds.Contains(x.ParentNodeID)).ToList();
                    childNodeIdsIds = childNodeList.Select(x => x.NodeID).ToList();
                    nodeListToBeAdded = lstTreeData.Where(x => childNodeIdsIds.Contains(x.ParentNodeID)).ToList();
                    AddNodesToCurrentTreeData(currentTreeNodeList, nodeListToBeAdded);
                }
                lstCurrentTreeData = currentTreeNodeList;
            }
        }

        private static void AddNodesToCurrentTreeData(List<GetRuleSetTree> currentTreeNodeList, List<GetRuleSetTree> nodeListToBeAdded)
        {
            foreach (GetRuleSetTree nodeToBeAdded in nodeListToBeAdded)
            {
                if (!currentTreeNodeList.Any(cond => cond.NodeID == nodeToBeAdded.NodeID && cond.ParentNodeID == nodeToBeAdded.ParentNodeID))
                    currentTreeNodeList.Add(nodeToBeAdded);
            }
        }

        private void HandleComplianceNodeMovedCase()
        {
            var ComplianceMappingNodeMoved = SysXWebSiteUtils.SessionService.GetCustomData("ComplianceMappingNodeMoved");
            if (ComplianceMappingNodeMoved == null || Convert.ToInt32(ComplianceMappingNodeMoved) != 1)
                return;
            SysXWebSiteUtils.SessionService.SetCustomData("ComplianceMappingNodeMoved", 0);
            String selectedTenantId = Request.QueryString["SelectedTenantId"];
            String navigationUrl = "";
            Dictionary<String, String> args = new Dictionary<String, String>();

            if (!Request.QueryString["NavigateUrl"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["NavigateUrl"]);
                args.TryGetValue("NavigateUrl", out navigationUrl);
            }
            String selectedNode = Request.QueryString["SelectedNodeId"];
            String scrollPosition = Request.QueryString["scrollPosition"];
            if (!selectedTenantId.IsNullOrEmpty())
            {
                SelectedTenantId = Convert.ToInt32(selectedTenantId);
            }
            if (!selectedNode.IsNullOrEmpty())
            {
                hdnSelectedNode.Value = selectedNode;
            }
            if (!navigationUrl.IsNullOrEmpty())
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "NavigateToSelectedNode('" + navigationUrl + "','" + scrollPosition + "');", true);
            }
            if (!scrollPosition.IsNullOrEmpty())
            {
                hdnScrollPosition.Value = scrollPosition;
            }
        }

        /// <summary>
        /// used for expanding parent nodes while binding tree load on demand.
        /// </summary>
        private void expandParentNodes()
        {
            if (hdnSelectedNode.Value != String.Empty)
            {
                var parentnode = treePackages.FindNodeByValue(hdnSelectedNode.Value).ParentNode;
                while (parentnode != null)
                {
                    parentnode.Expanded = true;
                    parentnode = parentnode.ParentNode;
                }
            }
        }

        //UAT-1116: Package selection combo box on package screens
        /// <summary>
        /// To bind package dropdown
        /// </summary>
        private void BindPackage()
        {
            Presenter.GetCompliancePackages();
            ddlPackage.DataSource = ListCompliancePackages;
            ddlPackage.DataBind();
            if (ListCompliancePackages.Count >= 10)
            {
                ddlPackage.Height = Unit.Pixel(200);
            }
            if (ListCompliancePackages.Count == AppConsts.NONE)
            {
                ddlPackage.EnableCheckAllItemsCheckBox = false;
            }
            else
            {
                ddlPackage.EnableCheckAllItemsCheckBox = true;
            }
        }

        #region UAT-3032:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenantId.IsNullOrEmpty() || CurrentViewContext.SelectedTenantId == AppConsts.ONE)
            {
               // Presenter.GetPreferredSelectedTenant();
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
                }
            }
        }
        #endregion
        #endregion

        #endregion

    }
}

