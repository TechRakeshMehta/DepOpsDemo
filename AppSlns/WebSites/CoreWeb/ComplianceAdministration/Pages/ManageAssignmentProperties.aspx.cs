#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.Services;
using System.Linq;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;


#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageAssignmentProperties : BaseWebPage, IManageAssignmentPropertiesView
    {
        #region Variables

        #region Private Variables

        private ManageAssignmentPropertiesPresenter _presenter = new ManageAssignmentPropertiesPresenter();

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public ManageAssignmentPropertiesPresenter Presenter
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

        public List<Tenant> ListTenants
        {
            set;
            get;
        }
        public List<GetRuleSetTree> LstAssignmentPropertiesTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("LstAssignmentPropertiesTreeData") != null)
                    return (List<GetRuleSetTree>)(SysXWebSiteUtils.SessionService.GetCustomData("LstAssignmentPropertiesTreeData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("LstAssignmentPropertiesTreeData", value);
            }
        }

        public List<GetRuleSetTree> ListCurrentTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("AssignmentPropertiesTreeData") != null)
                    return (List<GetRuleSetTree>)(SysXWebSiteUtils.SessionService.GetCustomData("AssignmentPropertiesTreeData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("AssignmentPropertiesTreeData", value);
            }
        }

        public Int32 SelectedTenant
        {
            set;
            get;
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        //UAT-2717  
        public List<CompliancePackage> ListCompliancePackages
        {
            set;
            get;
        }

        public List<Int32> LstSelectedPackageIDs
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

        public IManageAssignmentPropertiesView CurrentViewContext
        {
            get { return this; }
        }

        Int32 IManageAssignmentPropertiesView.PreferredSelectedTenantID
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

        /// <summary>
        /// Loads the page ManageAssignmentProperties.aspx.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                if (Presenter.IsAdminLoggedIn())
                {

                    Presenter.OnViewLoaded();
                    ddlTenant.DataSource = ListTenants;
                    ddlTenant.DataBind();
                    /*UAT-3032*/
                    GetPreferredSelectedTenant();
                    /*END UAT-3032*/
                }
            }

            if (Presenter.IsAdminLoggedIn())
            {
                String selectedTenant = ddlTenant.SelectedValue;
                //spbTreeAndDetail.Visible = false;
                if (!selectedTenant.IsNullOrEmpty() && selectedTenant != AppConsts.ZERO)
                {
                    SelectedTenant = Convert.ToInt32(selectedTenant);
                    if (this.IsPostBack)
                    {
                        BindTreeAssignmentProperties();
                    }
                }
            }
            else
            {
                paneTenant.Visible = false;
                if (this.IsPostBack)
                {
                    BindTreeAssignmentProperties();
                }
                else
                {
                    Presenter.GetAssignmentPropertiesTreeData();
                    ListCurrentTreeData = LstAssignmentPropertiesTreeData.Where(x => (x.UICode == RuleSetTreeNodeType.Package)).ToList();
                    BindTreeAssignmentProperties();
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "createSplitter();", true);
            }
            //Hiding top page
            CoreWeb.Shell.MasterPages.DefaultMaster masterPage = this.Master as CoreWeb.Shell.MasterPages.DefaultMaster;
            if (masterPage != null)
            {
                masterPage.HideTitleBars(true);
            }
            base.Title = "Assignment Properties";
            base.SetModuleTitle("Compliance Setup");
            base.SetPageTitle("Assignment Properties");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //var updatePanel = this.Master.FindControl("UpdatePanel1") as System.Web.UI.UpdatePanel;
            //updatePanel.UpdateMode = System.Web.UI.UpdatePanelUpdateMode.Conditional;
            //updatePanel.ChildrenAsTriggers = false;
            this.treeAssignmentProperties.NodeExpand += new RadTreeViewEventHandler(this.treeAssignmentProperties_NodeExpand);
        }

        /// <summary>
        /// Binds the tree data as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                ListCurrentTreeData = new List<GetRuleSetTree>();
                BindPackage();
                BindTreeAssignmentProperties();
                //spbTreeAndDetail.Visible = true;
                //SelectedTenant = Convert.ToInt32(ddlTenant.SelectedValue);
                //Presenter.GetAssignmentPropertiesTreeData();
                //ListCurrentTreeData = LstAssignmentPropertiesTreeData.Where(x => (x.UICode == RuleSetTreeNodeType.Package)).ToList();
                //  rfvPackage.Enabled = true;
                // rfvPackage.Visible = true;
            }
        }

        /// <summary>
        /// Bounds the data to tree view (which includes list of packages, categories and items).
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void treeAssignmentProperties_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            //Gets the detail of a node.
            GetRuleSetTree item = (GetRuleSetTree)e.Node.DataItem;

            //Sets the navigation URL, Color Code and Font Code as per node type.
            ComplianceTreeUIContract complianceTreeUIContract = Presenter.GetTreeNodeDetails(item.UICode, item.DataID, item.ParentDataID, item.ParentNodeID);

            e.Node.NavigateUrl = complianceTreeUIContract.NavigateURL;
            e.Node.Target = "childpageframe";
            e.Node.Value = item.NodeID.ToString() + "_" + item.UICode;
            e.Node.Expanded = complianceTreeUIContract.IsExpand;
            Boolean IfChildNodeExist = LstAssignmentPropertiesTreeData.Any(x => x.ParentNodeID == item.NodeID);
            if (IfChildNodeExist)
            {
                e.Node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
            }

            //Customize Node UI
            BaseWebPage.CustomizeComplianceNode(e.Node, item);
        }

        protected void treeAssignmentProperties_NodeExpand(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            List<GetRuleSetTree> currentTreeNodeList = ListCurrentTreeData;
            List<GetRuleSetTree> childNodeList = new List<GetRuleSetTree>();
            String currentNodeValue = e.Node.Value;
            String nodeId = String.Empty;
            String uiCode = String.Empty;

            String[] idCodeList = currentNodeValue.Split('_');
            nodeId = idCodeList[0];
            uiCode = idCodeList[1];
            GetRuleSetTree currentNode = null;
            currentNode = LstAssignmentPropertiesTreeData.FirstOrDefault(x => x.NodeID == nodeId && x.UICode == uiCode);
            childNodeList = LstAssignmentPropertiesTreeData.Where(x => x.ParentNodeID == currentNode.NodeID).ToList();
            List<String> childNodeIds = childNodeList.Select(x => x.NodeID).ToList();
            if (childNodeList.Count > AppConsts.NONE)
            {
                foreach (GetRuleSetTree nodeToBeAdded in childNodeList)
                {
                    e.Node.Nodes.Add(AddNodesToCurrentTreeData(currentTreeNodeList, nodeToBeAdded));
                }
                e.Node.Expanded = true;
                RadTreeNode parentNode = GetTreeNodeTopParent(e.Node);
                if (parentNode.IsNotNull())
                {
                    parentNode.Expanded = true;
                }
            }
            ListCurrentTreeData = currentTreeNodeList;

        }
        #region UAT-2717 Button Event

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                //spbTreeAndDetail.Visible = true;
                SelectedTenant = Convert.ToInt32(ddlTenant.SelectedValue);
                Presenter.GetAssignmentPropertiesTreeData();
                ListCurrentTreeData = LstAssignmentPropertiesTreeData.Where(x => (x.UICode == RuleSetTreeNodeType.Package)).ToList();
                ListCurrentTreeData = ListCurrentTreeData.Where(cond => LstSelectedPackageIDs.Contains(cond.DataID)).ToList();
                BindTreeAssignmentProperties();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "createSplitter();", true);
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods

        private void BindTreeAssignmentProperties()
        {
            treeAssignmentProperties.DataSource = ListCurrentTreeData;
            treeAssignmentProperties.DataTextField = "Value";
            treeAssignmentProperties.DataFieldID = "NodeID";
            treeAssignmentProperties.DataFieldParentID = "ParentNodeID";
            treeAssignmentProperties.DataBind();
        }

        /// <summary>
        /// Method to add the node in Current assignment properties tree data list.
        /// </summary>
        /// <param name="currentTreeNodeList">currentTreeNodeList</param>
        /// <param name="nodeListToBeAdded">nodeListToBeAdded</param>
        private RadTreeNode AddNodesToCurrentTreeData(List<GetRuleSetTree> currentTreeNodeList, GetRuleSetTree nodeToBeAdded)
        {

            ComplianceTreeUIContract complianceTreeUIContract = Presenter.GetTreeNodeDetails(nodeToBeAdded.UICode, nodeToBeAdded.DataID, nodeToBeAdded.ParentDataID, nodeToBeAdded.ParentNodeID);
            Boolean IfChildNodeExist = LstAssignmentPropertiesTreeData.Any(x => x.ParentNodeID == nodeToBeAdded.NodeID);
            RadTreeNode node = new RadTreeNode();
            node.NavigateUrl = complianceTreeUIContract.NavigateURL;
            node.Text = nodeToBeAdded.Value.ToString();
            node.Target = "childpageframe";
            node.Value = nodeToBeAdded.NodeID.ToString() + "_" + nodeToBeAdded.UICode;
            if (IfChildNodeExist)
            {
                node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
            }
            BaseWebPage.CustomizeComplianceNode(node, nodeToBeAdded);
            if (!currentTreeNodeList.Any(cond => cond.NodeID == nodeToBeAdded.NodeID && cond.ParentNodeID == nodeToBeAdded.ParentNodeID))
                currentTreeNodeList.Add(nodeToBeAdded);
            return node;
        }

        public RadTreeNode GetTreeNodeTopParent(RadTreeNode nodeToFindTopParent)
        {
            var parentNode = nodeToFindTopParent.ParentNode;
            if (parentNode.IsNotNull())
            {
                if (parentNode.Level == 0)
                    return parentNode;
                parentNode = GetTreeNodeTopParent(parentNode);
                return parentNode;
            }
            return null;
        }
        #endregion

        #region UAT-2717

        private void BindPackage()
        {
            Presenter.GetCompliancePackages();
            ddlPackage.DataSource = ListCompliancePackages;
            ddlPackage.DataBind();
            if (ListCompliancePackages.Count >= 10)
            {
                ddlPackage.Height = System.Web.UI.WebControls.Unit.Pixel(200);
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
        #endregion

        #region UAT-3032:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenant.IsNullOrEmpty() || CurrentViewContext.SelectedTenant== AppConsts.ONE)
            {
                //Presenter.GetPreferredSelectedTenant();
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    CurrentViewContext.SelectedTenant= Convert.ToInt32(ddlTenant.SelectedValue);
                    BindPackage();
                }
            }
        }
        #endregion
        #endregion


    }
}

