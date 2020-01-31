#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using System.Linq;
#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Web.Services;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using CoreWeb.AgencyHierarchy.Views;
using Newtonsoft.Json;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyHierarchyMultipleRootNodes : BaseWebPage, IAgencyHierarchyMultipleRootNodesView
    {
        #region Variables

        #region Private Variables

        private AgencyHierarchyMultipleRootNodesPresenter _presenter = new AgencyHierarchyMultipleRootNodesPresenter();
        private List<String> _lstCodeForColumnConfig = new List<String>(); //UAT-3952
        private Int32 _loggedInUserTenantId; // UAT-4443

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public AgencyHierarchyMultipleRootNodesPresenter Presenter
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

        public List<AgencyHierarchyContract> lstTreeData
        {
            get
            {
                if (!ViewState["lstTreeData"].IsNullOrEmpty())
                    return (ViewState["lstTreeData"] as List<AgencyHierarchyContract>);
                return new List<AgencyHierarchyContract>();
            }
            set
            {
                ViewState["lstTreeData"] = value;
            }
        }



        public Int32 TenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }
        public List<AgencyHierarchyContract> lstChildTreeData
        {
            get
            {
                if (!ViewState["lstChildTreeData"].IsNullOrEmpty())
                    return (ViewState["lstChildTreeData"] as List<AgencyHierarchyContract>);
                return new List<AgencyHierarchyContract>();
            }
            set
            {
                ViewState["lstChildTreeData"] = value;
            }
        }
        public String RootNodeIds
        {
            get
            {
                return Convert.ToString(ViewState["RootNodeIds"]);
            }
            set
            {
                ViewState["RootNodeIds"] = value;
            }
        }
        public String NodeIds
        {
            get
            {
                return Convert.ToString(ViewState["NodeIds"]);
            }
            set
            {
                ViewState["NodeIds"] = value;
            }
        }
        public String AgencyIds
        {
            get
            {
                return Convert.ToString(ViewState["AgencyIds"]);
            }
            set
            {
                ViewState["AgencyIds"] = value;
            }
        }
        public Boolean AgencyHierarchyNodeSelection
        {
            get
            {
                return Convert.ToBoolean(ViewState["AgencyHierarchyNodeSelection"]);
            }
            set
            {
                ViewState["AgencyHierarchyNodeSelection"] = value;
            }
        }
        public Boolean NodeHierarchySelection
        {
            get
            {
                return Convert.ToBoolean(ViewState["NodeHierarchySelection"]);
            }
            set
            {
                ViewState["NodeHierarchySelection"] = value;
            }
        }
        public List<AgencyHierarchyContract> AgencyList
        {
            get
            {
                if (!ViewState["AgencyHierarchyNodeIds"].IsNullOrEmpty())
                    return (ViewState["AgencyHierarchyNodeIds"] as List<AgencyHierarchyContract>);
                return new List<AgencyHierarchyContract>();
            }
            set
            {
                ViewState["AgencyHierarchyNodeIds"] = value;
            }
        }
        public Int32 CurrentOrgUserID
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentOrgUserID"]);
            }
            set
            {
                ViewState["CurrentOrgUserID"] = value;
            }
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IAgencyHierarchyMultipleRootNodesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public String AgencyHierarchyNodeIds
        {
            get
            {
                return Convert.ToString(ViewState["AgencyHierarchyNodeIds"]);
            }
            set
            {
                ViewState["AgencyHierarchyNodeIds"] = value;
            }
        }
        public String XMLResult { set; get; }

        public Boolean IsAllNodeDisabledMode
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAllNodeDisabledMode"]);
            }
            set
            {
                ViewState["IsAllNodeDisabledMode"] = value;
            }
        }

        public Boolean IsParentDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsParentDisable"]);
            }
            set
            {
                ViewState["IsParentDisable"] = value;
            }
        }
        public Int32 ParentRootNodeCount
        {
            get
            {
                if (!ViewState["ParentRootNodeCount"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["ParentRootNodeCount"]);
                else
                    return AppConsts.NONE;
            }
            set
            {
                ViewState["ParentRootNodeCount"] = value;
            }
        }
        public Boolean IsAgencyNodeCheckable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAgencyNodeCheckable"]);
            }
            set
            {
                ViewState["IsAgencyNodeCheckable"] = value;
            }
        }
        public String SelectedInstitutionNodeId
        {
            get;
            set;
        }

        #region UAT-3494
        public Boolean IsRotationPkgCopyFromAgencyHierarchy
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsRotationPkgCopyFromAgencyHierarchy"]);
            }
            set
            {
                ViewState["IsRotationPkgCopyFromAgencyHierarchy"] = value;
            }
        }
        #endregion

        //UAT-3952
        public Boolean isHierarchyCollapsed
        {
            set;
            get;
        }

        //UAT-4257
        public Boolean IsBackButtonDisabled
        {
            get;
            set;
        }

        public Int32 screenColumnID
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

        public List<int?> lstDosabledNodes = new List<int?>();

        #region UAT-4443
        
        public String IsRequestFromAddRotationScrn
        {
            get
            {
                return Convert.ToString(ViewState["IsRequestFromAddRotationScrn"]);
            }
            set
            {
                ViewState["IsRequestFromAddRotationScrn"] = value;
            }
        }

        Boolean IAgencyHierarchyMultipleRootNodesView.IsClientAdmin
        {
            get;
            set;
        }

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
        public Int32 LoggedInUserTenantId
        {
            get
            {
                if (_loggedInUserTenantId == 0)
                {
                    //_tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _loggedInUserTenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _loggedInUserTenantId;
            }
            set { _loggedInUserTenantId = value; }
        }
        #endregion

        #endregion
        #endregion

        #region Events

        #region Page Events
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (treeAgencyHierarchy.Visible)
            {
                fsucAgencyHierarchyMultipleNodes.SaveButton.Visible = false;
                fsucAgencyHierarchyMultipleNodes.ClearButton.Visible = false;
                fsucAgencyHierarchyMultipleNodes.ExtraButton.Visible = false;
                fsucAgencyHierarchyMultipleNodes.SubmitButton.Visible = false;
            }
            else if (treeChildAgencyHierarchy.Visible)
            {
                if (ParentRootNodeCount == AppConsts.ONE)
                {
                    fsucAgencyHierarchyMultipleNodes.SaveButton.Visible = true;
                    fsucAgencyHierarchyMultipleNodes.ClearButton.Visible = false;
                    fsucAgencyHierarchyMultipleNodes.ExtraButton.Visible = true;
                    fsucAgencyHierarchyMultipleNodes.SubmitButton.Visible = true;
                }
                else
                {
                    fsucAgencyHierarchyMultipleNodes.SaveButton.Visible = true;
                    fsucAgencyHierarchyMultipleNodes.ExtraButton.Visible = true;
                    fsucAgencyHierarchyMultipleNodes.SubmitButton.Visible = true;
                    if (IsRotationPkgCopyFromAgencyHierarchy) //UAT-3494
                    {
                        fsucAgencyHierarchyMultipleNodes.ClearButton.Visible = false;
                    }
                    else
                    {
                        fsucAgencyHierarchyMultipleNodes.ClearButton.Visible = true;
                    }
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Request.QueryString["TenantId"].IsNullOrEmpty())
                {
                    TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
                }
                else
                {
                    TenantId = AppConsts.NONE;
                }
                if (!Request.QueryString["CurrentOrgUserID"].IsNullOrEmpty())
                {
                    CurrentOrgUserID = Convert.ToInt32(Request.QueryString["CurrentOrgUserID"]);
                }
                else
                {
                    CurrentOrgUserID = AppConsts.NONE;
                }
                if (!Request.QueryString["AgencyHierarchyNodeSelection"].IsNullOrEmpty())
                {
                    AgencyHierarchyNodeSelection = Convert.ToBoolean(Request.QueryString["AgencyHierarchyNodeSelection"]);
                }
                else
                {
                    AgencyHierarchyNodeSelection = false;
                }

                if (!Request.QueryString["IsParentDisable"].IsNullOrEmpty())
                {
                    IsParentDisable = Convert.ToBoolean(Request.QueryString["IsParentDisable"]);
                }
                else
                {
                    IsParentDisable = false;
                }

                if (!Request.QueryString["NodeHierarchySelection"].IsNullOrEmpty())
                {
                    NodeHierarchySelection = Convert.ToBoolean(Request.QueryString["NodeHierarchySelection"]);
                }
                else
                {
                    NodeHierarchySelection = false;
                }

                if (!Request.QueryString["IsAllNodeDisabledMode"].IsNullOrEmpty())
                {
                    IsAllNodeDisabledMode = Convert.ToBoolean(Request.QueryString["IsAllNodeDisabledMode"]);
                }
                else
                {
                    IsAllNodeDisabledMode = false;
                }

                if (!Request.QueryString["IsAgencyNodeCheckable"].IsNullOrEmpty())
                {
                    IsAgencyNodeCheckable = Convert.ToBoolean(Request.QueryString["IsAgencyNodeCheckable"]);
                }
                else
                {
                    IsAgencyNodeCheckable = true;
                }

                if (!Request.QueryString["InstitutionNodeIds"].IsNullOrEmpty())
                    CurrentViewContext.SelectedInstitutionNodeId = Convert.ToString(Request.QueryString["InstitutionNodeIds"]);

                if (!CurrentViewContext.SelectedInstitutionNodeId.IsNullOrEmpty())
                {
                    Presenter.GetAgencyHiearchyIdsByDeptProgMappingID();
                }

                #region UAT-3494
                if (!Request.QueryString["IsRotationPkgCopyFromAgencyHierarchy"].IsNullOrEmpty())
                {
                    IsRotationPkgCopyFromAgencyHierarchy = Convert.ToBoolean(Request.QueryString["IsRotationPkgCopyFromAgencyHierarchy"]);
                }
                else
                {
                    IsRotationPkgCopyFromAgencyHierarchy = false;
                }
                #endregion

                #region UAT-4257

                if (!Request.QueryString["IsChildBackButtonDisabled"].IsNullOrEmpty())
                {
                    CurrentViewContext.IsBackButtonDisabled = Convert.ToBoolean(Request.QueryString["IsChildBackButtonDisabled"]);
                }
                else
                {
                    CurrentViewContext.IsBackButtonDisabled = false;
                }

                if (!CurrentViewContext.IsBackButtonDisabled.IsNullOrEmpty() && CurrentViewContext.IsBackButtonDisabled)
                {
                    fsucAgencyHierarchyMultipleNodes.ClearButton.Enabled = false;
                }
                else
                {
                    fsucAgencyHierarchyMultipleNodes.ClearButton.Enabled = true;
                }
                #endregion

                #region UAT-4443
                if (!Request.QueryString["IsRequestFromAddRotationScrn"].IsNullOrEmpty())
                {
                    IsRequestFromAddRotationScrn = Convert.ToString(Request.QueryString["IsRequestFromAddRotationScrn"]);                    
                }                
                else
                {
                    IsRequestFromAddRotationScrn = string.Empty;
                }
                Presenter.IsClientAdmin();
                #endregion

                SetPropertiesFromTree();

                //UAT-3952
                Presenter.BindPageControls();

                if (!this.IsPostBack)
                {
                    if (!Request.QueryString["SelectedAgecnyIds"].IsNullOrEmpty() && Request.QueryString["SelectedAgecnyIds"] != AppConsts.NONE.ToString())
                    {
                        AgencyIds = Convert.ToString(Request.QueryString["SelectedAgecnyIds"]);
                    }
                    else
                    {
                        AgencyIds = String.Empty;
                    }
                    if (!Request.QueryString["SelectedRootNodeId"].IsNullOrEmpty())
                    {
                        RootNodeIds = Convert.ToString(Request.QueryString["SelectedRootNodeId"]);
                        hdnSelectedRootNodeId.Value = RootNodeIds;
                    }
                    else
                    {
                        RootNodeIds = String.Empty;
                    }
                    if (!Request.QueryString["SelectedNodeIds"].IsNullOrEmpty() && Request.QueryString["SelectedNodeIds"] != AppConsts.NONE.ToString())
                    {
                        NodeIds = Convert.ToString(Request.QueryString["SelectedNodeIds"]);
                    }
                    else
                    {
                        NodeIds = String.Empty;
                    }
                    if (!String.IsNullOrEmpty(RootNodeIds))
                    {
                        treeAgencyHierarchy.Visible = false;
                        treeChildAgencyHierarchy.Visible = true;
                        Presenter.GetTreeData();
                        ParentRootNodeCount = (lstTreeData.IsNotNull() && lstTreeData.Count == AppConsts.ONE) ? lstTreeData.Count : AppConsts.NONE;
                        BindChildTree();
                    }
                    else
                    {
                        BindTree();
                    }

                    //UAT-3952
                    if (!CurrentViewContext.isHierarchyCollapsed.IsNullOrEmpty())
                    {
                        hdnIsHierarchyCollapsed.Value = CurrentViewContext.isHierarchyCollapsed.ToString();
                    }
                    else
                    {
                        hdnIsHierarchyCollapsed.Value = "False";
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

        #region Button Events
        protected void btnOk_Click(Object sender, EventArgs e)
        {
            try
            {
                if (NodeHierarchySelection)
                {
                    if (treeChildAgencyHierarchy.CheckedNodes.Count > 0)
                    {
                        // hdnIsChildTreeNodeChecked.Value = "true";
                        UnSelectParentNodes(treeChildAgencyHierarchy.CheckedNodes);
                    }

                    if (treeChildAgencyHierarchy.CheckedNodes.Count > 0 && treeChildAgencyHierarchy.CheckedNodes.Any(x => x.Checkable == true))
                        hdnIsChildTreeNodeChecked.Value = "true";
                    else
                        hdnIsChildTreeNodeChecked.Value = "false";
                }

                #region UAT-3952
                CurrentViewContext.isHierarchyCollapsed = Convert.ToBoolean(hdnIsHierarchyCollapsed.Value);
                Dictionary<Int32, Boolean> dctUserScreenColumnMpng = new Dictionary<Int32, Boolean>();
                dctUserScreenColumnMpng.Add(CurrentViewContext.screenColumnID, !CurrentViewContext.isHierarchyCollapsed);
                Presenter.SaveUserScreenColumnMapping(dctUserScreenColumnMpng);
                #endregion

                GetSelectedChildTreeData();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        protected void fsucAgencyHierarchyMultipleNodes_ClearClick(Object sender, EventArgs e)
        {
            treeAgencyHierarchy.Visible = true;
            treeChildAgencyHierarchy.Visible = false;
            treeAgencyHierarchy.UnselectAllNodes();
            hdnSelectedAgecnyIds.Value = String.Empty;
            hdnSelectedNodeIds.Value = String.Empty;
            hdnSelectedRootNodeId.Value = String.Empty;
            hdnIsHierarchyCollapsed.Value = CurrentViewContext.isHierarchyCollapsed.ToString();
            if (!String.IsNullOrEmpty(RootNodeIds))
            {
                BindTree();
            }
        }
        #endregion

        #region TreeView Events
        protected void TreeChildAgencyHierarchy_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            AgencyHierarchyContract item = (AgencyHierarchyContract)e.Node.DataItem;
            if (item.IsNotNull())
            {
                if (AgencyHierarchyNodeSelection && (!String.IsNullOrEmpty(item.NodeType) && item.NodeType.ToLower() == "agencynode" || item.NodeType.ToLower() == "leafnode"))
                {

                    if (!NodeIds.IsNullOrEmpty() && item.NodeType.ToLower() == "leafnode")
                    {
                        List<Int32> nodeIds = NodeIds.Split(',').Select(t => Int32.Parse(t)).ToList();

                        if (!IsAgencyNodeCheckable)
                        {
                            e.Node.Checkable = IsAgencyNodeCheckable;
                        }
                        else if (nodeIds.Contains(item.NodeID))
                        {
                            e.Node.Checked = true;
                        }
                    }
                    else if (item.NodeType.ToLower() == "agencynode")
                    {
                        if (!AgencyIds.IsNullOrEmpty())
                        {
                            List<Int32> agencyIds = AgencyIds.Split(',').Select(t => Int32.Parse(t)).ToList();

                            if (agencyIds.Count > 0 && agencyIds.Contains(item.NodeID))
                            {
                                e.Node.Checked = true;
                            }
                        }
                        e.Node.ForeColor = System.Drawing.Color.Green;
                    }

                    else
                    {
                        if (!IsAgencyNodeCheckable)
                        {
                            e.Node.Checkable = IsAgencyNodeCheckable;
                        }
                        else
                        {
                            e.Node.Enabled = true;
                        }
                    }
                }
                else if (NodeHierarchySelection && (!String.IsNullOrEmpty(item.NodeType) && item.NodeType.ToLower() == "leafnode"))
                {
                    if (!NodeIds.IsNullOrEmpty() && item.NodeType.ToLower() == "leafnode")
                    {
                        List<Int32> nodeIds = NodeIds.Split(',').Select(t => Int32.Parse(t)).ToList();

                        if (nodeIds.Count > 0 && nodeIds.Contains(item.NodeID))
                        {
                            e.Node.Checked = true;
                        }
                    }
                    else
                    {
                        e.Node.Enabled = true;
                    }
                    if (item.IsDisabled && IsParentDisable)
                    {
                        //UAT-2772 || Bug ID: 17095
                        // e.Node.Enabled = false;
                        e.Node.CssClass = "DisableNode";
                    }
                }
                else if ((!String.IsNullOrEmpty(item.NodeType) && item.NodeType.ToLower() == "parentnode"))
                {
                    if (!NodeIds.IsNullOrEmpty() && item.NodeType.ToLower() == "parentnode")
                    {
                        List<Int32> nodeIds = NodeIds.Split(',').Select(t => Int32.Parse(t)).ToList();

                        if (nodeIds.Count > 0 && nodeIds.Contains(item.NodeID))
                        {
                            e.Node.Checked = true;
                        }
                    }
                    if (!IsAgencyNodeCheckable)
                    {
                        e.Node.Checkable = IsAgencyNodeCheckable;
                    }
                    if (item.IsDisabled && IsParentDisable)
                    {
                        //UAT-2772 || Bug ID: 17095
                        //e.Node.Enabled = false;
                        e.Node.CssClass = "DisableNode";
                    }
                }
                else if ((!String.IsNullOrEmpty(item.NodeType) && item.NodeType.ToLower() == "agencynode"))
                {
                    e.Node.ForeColor = System.Drawing.Color.Green;
                    //UAT-2772 || Bug ID: 17095
                    //e.Node.Enabled = false;
                    if (IsRotationPkgCopyFromAgencyHierarchy)
                    {
                        e.Node.Checked = true;
                    }
                    else
                    {
                        e.Node.CssClass = "DisableNode";
                    }
                }
                else
                {
                    //UAT-2772 || Bug ID: 17095
                    // e.Node.Enabled = false;
                    e.Node.CssClass = "DisableNode";
                }
                if (IsAllNodeDisabledMode)
                {
                    //UAT-2772 || Bug ID: 17095
                    //e.Node.Enabled = false;
                    e.Node.CssClass = "DisableNode";
                }

                // UAT-4443
                if (IsRequestFromAddRotationScrn.Equals("Yes") && CurrentViewContext.IsClientAdmin)
                {
                    if (item.IsNodeAvailable == 1 && (!String.IsNullOrEmpty(item.NodeType) && item.NodeType.ToLower() != "agencynode"))
                    {
                        //e.Node.CssClass = "DisableNode";
                        e.Node.Visible = false;
                        lstDosabledNodes.Add(item.NodeID);
                    }
                    if (lstDosabledNodes.Contains(item.ParentNodeID)  && (!String.IsNullOrEmpty(item.NodeType) && item.NodeType.ToLower() == "leafnode"))
                    {
                        //e.Node.CssClass = "DisableNode";
                        e.Node.Visible = false;
                        lstDosabledNodes.Add(item.NodeID);
                        //e.Node.Checkable = false;
                    }
                    if (lstDosabledNodes.Contains(item.ParentNodeID) &&   (!String.IsNullOrEmpty(item.NodeType) && item.NodeType.ToLower() == "agencynode"))
                    {
                        e.Node.Visible = false;
                       // e.Node.CssClass = "DisableNode";
                        //e.Node.Checkable = false;
                    }
                }
                // UAT-4443

            }
        }

        protected void TreeAgencyHierarchy_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            AgencyHierarchyContract item = (AgencyHierarchyContract)e.Node.DataItem;
            if (item.IsNotNull())
            {
                if (!String.IsNullOrEmpty(RootNodeIds))
                {
                    if (Convert.ToInt32(RootNodeIds) != 0 && item.NodeID == Convert.ToInt32(RootNodeIds))
                    {
                        e.Node.Selected = true;
                    }
                }
                // UAT-4443
                if (IsRequestFromAddRotationScrn.Equals("Yes") && CurrentViewContext.IsClientAdmin)
                {
                    if (item.IsNodeAvailable == 1)
                    {
                        e.Node.Visible = false;
                    }
                }
                // UAT-4443
            }
        }
        protected void treeAgencyHierarchy_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                GetSelectedParentTreeData();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #endregion

        #region Methods

        #region Private Methods
        /// <summary>
        /// BindTree
        /// </summary>
        private void BindTree()
        {
            Presenter.GetTreeData();
            treeAgencyHierarchy.DataSource = lstTreeData;

            if (lstTreeData.IsNotNull() && lstTreeData.Count > 1)
            {
                treeAgencyHierarchy.DataTextField = "Value";
                treeAgencyHierarchy.DataFieldID = "NodeID";
                treeAgencyHierarchy.DataFieldParentID = "ParentNodeID";
                treeAgencyHierarchy.DataValueField = "NodeID";
                treeAgencyHierarchy.DataBind();
            }
            else if (lstTreeData.IsNotNull() && lstTreeData.Count == 1)
            {
                ParentRootNodeCount = lstTreeData.Count;
                RootNodeIds = (lstTreeData.FirstOrDefault()).NodeID.ToString();
                treeAgencyHierarchy.Visible = false;
                hdnSelectedRootNodeId.Value = RootNodeIds;
                treeChildAgencyHierarchy.Visible = true;
                BindChildTree();
            }
        }
        /// <summary>
        /// SetPropertiesFromTree
        /// </summary>
        private void SetPropertiesFromTree()
        {
            if (CurrentOrgUserID > AppConsts.NONE && (CurrentViewContext.SelectedInstitutionNodeId.IsNullOrEmpty()))
            {
                List<Int32> listAgencyHierarchyIds = Presenter.GetAgencyHierarchyIdsByOrgUserID();
                AgencyHierarchyNodeIds = String.Join(",", listAgencyHierarchyIds);
            }
            else if (TenantId > AppConsts.NONE && (CurrentViewContext.SelectedInstitutionNodeId.IsNullOrEmpty()))
            {
                List<Int32> listAgencyHierarchyIds = Presenter.GetAgencyHierarchyIdsByTenantID();
                AgencyHierarchyNodeIds = String.Join(",", listAgencyHierarchyIds);
            }
        }
        /// <summary>
        /// UnSelectParentNodes
        /// </summary>
        /// <param name="checkedNodes"></param>
        private void UnSelectParentNodes(IList<RadTreeNode> checkedNodes)
        {
            foreach (var node in treeChildAgencyHierarchy.CheckedNodes)
            {
                if (node.ParentNode.IsNotNull() && node.ParentNode.Checked)
                {
                    node.Checked = false;
                }
            }
        }
        /// <summary>
        /// GetSelectedChildTreeData
        /// </summary>
        private void GetSelectedChildTreeData()
        {
            List<String> nodeList = new List<String>();
            List<String> agencyList = new List<String>();
            List<String> listSelectedNodeIdForAgency = new List<String>();
            String jsonObj = String.Empty;
            String parentNode = String.Empty;
            String lowsetCommonAncestor = String.Empty;

            if (treeChildAgencyHierarchy.CheckedNodes.Count > 0)
            {
                foreach (var node in treeChildAgencyHierarchy.CheckedNodes)
                {

                    if (node.Nodes.Count == 0 && node.Value.IsNotNull() && Convert.ToInt32(node.Value) < 0)
                    {
                        agencyList.Add(node.Value.ToString());
                        listSelectedNodeIdForAgency.Add(node.TreeView.FindNodeByValue(node.Value).ParentNode.Value);
                    }
                    else
                    {
                        nodeList.Add(node.Value.ToString());
                    }
                }
                if (agencyList.IsNotNull() && agencyList.Count > 0)
                {
                    AgencyIds = String.Join(",", agencyList.ToArray());
                    hdnSelectedAgecnyIds.Value = AgencyIds;
                    if (AgencyHierarchyNodeSelection)
                    {
                        Session["AgencySelected"] = AgencyIds;
                    }
                }
                else if (AgencyHierarchyNodeSelection)
                {
                    Session["AgencySelected"] = null;
                    AgencyIds = String.Empty;
                }

                if (IsRotationPkgCopyFromAgencyHierarchy.IsNullOrEmpty() || !IsRotationPkgCopyFromAgencyHierarchy) //UAT-3494
                {
                    if (nodeList.IsNotNull() && nodeList.Count > 0)
                    {
                        NodeIds = String.Join(",", nodeList.ToArray());
                        hdnSelectedNodeIds.Value = NodeIds;
                        if (AgencyHierarchyNodeSelection)
                        {
                            Session["NodeSelected"] = NodeIds;
                        }
                    }
                    else if (AgencyHierarchyNodeSelection)
                    {
                        Session["NodeSelected"] = null;
                        NodeIds = String.Empty;
                    }
                }
                else
                {
                    NodeIds = String.Join(",", listSelectedNodeIdForAgency.Distinct());
                    hdnSelectedNodeIds.Value = NodeIds;
                    Session["NodeSelected"] = NodeIds;
                }

                //if (!IsRotationPkgCopyFromAgencyHierarchy.IsNullOrEmpty() && IsRotationPkgCopyFromAgencyHierarchy) //UAT-3494
                //{
                //    lowsetCommonAncestor = LowestCommonAncestor(listSelectedNodeIdForAgency, 0);
                //    hdnSelectedNodeIds.Value = lowsetCommonAncestor;
                //    Session["NodeSelected"] = lowsetCommonAncestor;
                //}                
                Presenter.GetAgencyDetailByMultipleNodeIds();
                if (!String.IsNullOrEmpty(XMLResult))
                {
                    System.Xml.Linq.XDocument input = System.Xml.Linq.XDocument.Parse(XMLResult);
                    jsonObj = JsonConvert.SerializeXNode(input, Newtonsoft.Json.Formatting.Indented, true);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParentFromAgencyHierarchy(" + jsonObj + ");", true);
            }
        }

        #region UAT-3494
        private String LowestCommonAncestor(List<String> lstselectedNodeIdsForAgency, Int32 parentId)
        {
            var treenode = treeChildAgencyHierarchy.GetAllNodes();
            String tempNodeId = lstselectedNodeIdsForAgency.First();
            foreach (String node in lstselectedNodeIdsForAgency)
            {
                tempNodeId = GetNodes(treeChildAgencyHierarchy, tempNodeId, node);
            }
            if (tempNodeId.IsNullOrEmpty())
                throw new Exception("Hierarchy node could not be found.");

            return tempNodeId;
        }
        private String GetNodes(WclTreeView nodes, String node1Id, String node2Id)
        {
            RadTreeNode node1 = nodes.FindNodeByValue(node1Id);
            RadTreeNode node2 = nodes.FindNodeByValue(node2Id);
            if (node1Id == node2Id)
                return node1Id;

            else if (!node1.ParentNode.IsNullOrEmpty())
            {
                return GetNodes(nodes, node2Id, node1.ParentNode.Value);
            }
            else if (!node2.ParentNode.IsNullOrEmpty())
            {
                return GetNodes(nodes, node1Id, node2.ParentNode.Value);
            }
            else
                return null;
        }
        #endregion

        /// <summary>
        /// GetSelectedParentTreeData
        /// </summary>
        private void GetSelectedParentTreeData()
        {
            List<String> rootNodes = new List<String>();
            if (!String.IsNullOrEmpty(treeAgencyHierarchy.SelectedNode.Value))
            {
                rootNodes.Add(treeAgencyHierarchy.SelectedNode.Value.ToString());
            }
            if (rootNodes.IsNotNull() && rootNodes.Count > 0)
            {
                RootNodeIds = String.Join(",", rootNodes.ToArray());
                hdnSelectedRootNodeId.Value = RootNodeIds;
                treeAgencyHierarchy.Visible = false;
                treeChildAgencyHierarchy.Visible = true;
                BindChildTree();
            }
        }
        /// <summary>
        /// BindChildTree
        /// </summary>
        private void BindChildTree()
        {
            Presenter.GetAgencyHierarchyByRootNodeIds();
            treeChildAgencyHierarchy.DataSource = lstChildTreeData;
            treeChildAgencyHierarchy.DataTextField = "Value";
            treeChildAgencyHierarchy.DataFieldID = "NodeID";
            treeChildAgencyHierarchy.DataFieldParentID = "ParentNodeID";
            treeChildAgencyHierarchy.DataValueField = "NodeID";
            treeChildAgencyHierarchy.DataBind();

            //UAT-3952
            Telerik.Web.UI.RadTreeNode node = this.treeChildAgencyHierarchy.Nodes[0];
            if (CurrentViewContext.isHierarchyCollapsed == true)
            {
                node.CollapseChildNodes();
                fsucAgencyHierarchyMultipleNodes.SubmitButtonText = "Expand";
            }
            else
            {
                node.ExpandChildNodes();
                fsucAgencyHierarchyMultipleNodes.SubmitButtonText = "Collapse";
            }
        }
        #endregion

        #endregion
    }
}