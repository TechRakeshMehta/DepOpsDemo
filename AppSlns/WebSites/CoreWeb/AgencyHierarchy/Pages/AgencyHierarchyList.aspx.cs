#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Data;
using System.Web.Services;
#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;

#endregion

#endregion

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyHierarchyList : BaseWebPage, IAgencyHierarchyListView
    {
        #region Variables

        #region Private Variables

        private AgencyHierarchyListPresenter _presenter = new AgencyHierarchyListPresenter();
        private List<String> _lstCodeForColumnConfig = new List<String>(); //UAT-3952
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties
        public AgencyHierarchyListPresenter Presenter
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
            set;
            get;
        }
        public List<AgencyHierarchyContract> lstChildTreeData
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

        public AgencyHierarchyContract agencyDetial
        {
            get;
            set;
        }
        public Int32 TenantId
        {
            get;
            set;
        }

        public Int32 RootNodeID
        {
            get;
            set;
        }

        public Int32 NodeID
        {
            get;
            set;
        }
        public String HierarchyLabel
        {
            get;
            set;
        }

        public String AgencyHierarchyNodeIds
        {
            get;
            set;
        }

        public String SelectedAgencyId
        {
            get;
            set;
        }

        public String SelectedRootNodeId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IAgencyHierarchyListView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 AgencyId
        {
            get;
            set;
        }

        public String SelectedInstitutionNodeId
        {
            get;
            set;
        }

        public String AgencyHierarchyNodeIdsToFilter
        {
            get;
            set;
        }

        public Boolean IsInstitutionHierarchyFilterApplied
        {
            get;
            set;
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

        //UAT-3245
        public List<Int32> lstTenantIds
        {
            get
            {
                if (!ViewState["lstTenantIds"].IsNullOrEmpty())
                    return (ViewState["lstTenantIds"] as List<Int32>);
                return new List<Int32>();
            }
            set
            {
                ViewState["lstTenantIds"] = value;
            }
        }

        //UAT-3952
        public Boolean isHierarchyCollapsed
        {
            set;
            get;
        }

        public Int32 screenColumnID
        {
            set;
            get;
        }

        #endregion

        #endregion

        #region Events

        #region Page Events
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (treeAgencyHierarchy.Visible)
            {
                fsucAgencyHierarchyList.ClearButton.Visible = false; ;
                fsucAgencyHierarchyList.ExtraButton.Visible = false;
                fsucAgencyHierarchyList.SubmitButton.Visible = false;
            }
            else if (treeChildAgencyHierarchy.Visible)
            {
                if (ParentRootNodeCount == AppConsts.ONE)
                {

                    fsucAgencyHierarchyList.ClearButton.Visible = false;
                    fsucAgencyHierarchyList.ExtraButton.Visible = true;
                    fsucAgencyHierarchyList.SubmitButton.Visible = true;
                }
                else
                {
                    fsucAgencyHierarchyList.ClearButton.Visible = true;
                    fsucAgencyHierarchyList.ExtraButton.Visible = true;
                    fsucAgencyHierarchyList.SubmitButton.Visible = true;
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

                if (!Request.QueryString["AgencyHierarchyNodeIds"].IsNullOrEmpty())
                {
                    AgencyHierarchyNodeIds = Convert.ToString(Request.QueryString["AgencyHierarchyNodeIds"]);
                }
                else
                {
                    AgencyHierarchyNodeIds = String.Empty;
                }

                if (!Request.QueryString["InstitutionNodeIds"].IsNullOrEmpty())
                    CurrentViewContext.SelectedInstitutionNodeId = Convert.ToString(Request.QueryString["InstitutionNodeIds"]);

                //UAT-3245
                if (!Request.QueryString["lstTenantIds"].IsNullOrEmpty())
                {
                    CurrentViewContext.lstTenantIds = (Request.QueryString["lstTenantIds"]).Split(',').ConvertIntoIntList();
                }
                else
                {
                    CurrentViewContext.lstTenantIds = null;
                }

                if (CurrentViewContext.lstTenantIds.IsNullOrEmpty())
                {
                    if (!CurrentViewContext.SelectedInstitutionNodeId.IsNullOrEmpty())
                    {
                        Presenter.GetAgencyHiearchyIdsByDeptProgMappingID();
                        IsInstitutionHierarchyFilterApplied = true;
                    }

                    if (!TenantId.IsNullOrEmpty() && TenantId > 0 && (CurrentViewContext.SelectedInstitutionNodeId.IsNullOrEmpty()))
                        Presenter.GetAgencyHiearchyIdsByTenantID();
                }
                //UAT-3245
                else if (!CurrentViewContext.lstTenantIds.IsNullOrEmpty())
                {
                    Presenter.GetAgencyHierarchyIdsByLstTenantIDs();
                }

                //UAT-3952
                Presenter.BindPageControls();

                if (!this.IsPostBack)
                {
                    if (!Request.QueryString["AgencyId"].IsNullOrEmpty())
                    {
                        SelectedAgencyId = Convert.ToString(Request.QueryString["AgencyId"]);
                    }
                    else
                    {
                        SelectedAgencyId = String.Empty;
                    }
                    if (!Request.QueryString["AgencyHierarchyRootNodeId"].IsNullOrEmpty())
                    {
                        hdnRootNodeId.Value = SelectedRootNodeId = Convert.ToString(Request.QueryString["AgencyHierarchyRootNodeId"]);
                    }
                    else
                    {
                        SelectedRootNodeId = String.Empty;
                    }

                    if (!String.IsNullOrEmpty(SelectedRootNodeId) && !String.IsNullOrEmpty(SelectedAgencyId)
                        && Convert.ToInt32(SelectedRootNodeId) > AppConsts.NONE && Convert.ToInt32(SelectedAgencyId) > AppConsts.NONE)
                    {
                        treeAgencyHierarchy.Visible = false;
                        RootNodeID = Convert.ToInt32(SelectedRootNodeId);
                        treeChildAgencyHierarchy.Visible = true;
                        Presenter.GetTreeData();
                        ParentRootNodeCount = (lstTreeData.IsNotNull() && lstTreeData.Count == AppConsts.ONE) ? lstTreeData.Count : AppConsts.NONE;
                        BindTreeByRootNodeID();
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

        #region TreeView Events
        protected void TreeAgency_NodeClick(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            if (e.Node.IsNotNull())
            {
                treeAgencyHierarchy.Visible = false;
                RootNodeID = Convert.ToInt32(e.Node.Value);
                hdnRootNodeId.Value = RootNodeID.ToString();
                treeChildAgencyHierarchy.Visible = true;
                BindTreeByRootNodeID();
            }
        }
        protected void TreeChildAgencyHierarchy_NodeClick(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            NodeID = Convert.ToInt32(e.Node.Attributes["NodeId"]);
            hdnNodeId.Value = NodeID.ToString();
            AgencyId = Convert.ToInt32(e.Node.Attributes["AgencyId"]);
            hdnLabel.Value = HierarchyLabel = Convert.ToString(e.Node.Attributes["HierarchyLabel"]);
            hdnAgencyId.Value = AgencyId.ToString();
            hdnAgencyName.Value = Convert.ToString(e.Node.Attributes["AgencyName"]);

            #region UAT-3952
            CurrentViewContext.isHierarchyCollapsed = Convert.ToBoolean(hdnIsHierarchyCollapsed.Value);
            Dictionary<Int32, Boolean> dctUserScreenColumnMpng = new Dictionary<Int32, Boolean>();
            dctUserScreenColumnMpng.Add(CurrentViewContext.screenColumnID, !CurrentViewContext.isHierarchyCollapsed);
            Presenter.SaveUserScreenColumnMapping(dctUserScreenColumnMpng);
            #endregion

            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
        }
        protected void TreeChildAgencyHierarchy_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            AgencyHierarchyContract item = (AgencyHierarchyContract)e.Node.DataItem;
            if (item.IsNotNull())
            {

                if (!String.IsNullOrEmpty(item.NodeType) && item.NodeType.ToLower() == "agencynode")
                {
                    e.Node.Enabled = true;
                    e.Node.Attributes["AgencyId"] = Convert.ToString(item.AgencyID);
                    e.Node.Attributes["HierarchyLabel"] = Convert.ToString(item.HierarchyLabel);
                    e.Node.Attributes["NodeId"] = Convert.ToString(item.ParentNodeID);
                    e.Node.Attributes["AgencyName"] = Convert.ToString(item.AgencyName);

                    if (!SelectedAgencyId.IsNullOrEmpty())
                    {
                        if (Convert.ToInt32(SelectedAgencyId) != 0 && item.AgencyID == Convert.ToInt32(SelectedAgencyId))
                        {
                            e.Node.Selected = true;

                        }
                    }
                    e.Node.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    //   e.Node.Enabled = false;
                }
            }
        }
        protected void TreeAgencyHierarchy_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            AgencyHierarchyContract item = (AgencyHierarchyContract)e.Node.DataItem;
            if (item.IsNotNull())
            {
                if (!String.IsNullOrEmpty(hdnRootNodeId.Value))
                {
                    if (Convert.ToInt32(hdnRootNodeId.Value) != 0 && item.NodeID == Convert.ToInt32(hdnRootNodeId.Value))
                    {
                        e.Node.Selected = true;
                    }
                }
            }
        }
        #endregion

        #region Button Events
        protected void fsucAgencyHierarchyList_ClearClick(Object sender, EventArgs e)
        {
            treeAgencyHierarchy.Visible = true;
            treeChildAgencyHierarchy.Visible = false;
            hdnAgencyId.Value = String.Empty;
            hdnIsHierarchyCollapsed.Value = CurrentViewContext.isHierarchyCollapsed.ToString();
            if (!String.IsNullOrEmpty(hdnRootNodeId.Value))
            {
                BindTree();
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
            if (lstTreeData.IsNotNull() && lstTreeData.Count > 1)
            {
                treeAgencyHierarchy.DataSource = lstTreeData;
                treeAgencyHierarchy.DataTextField = "Value";
                treeAgencyHierarchy.DataFieldID = "NodeID";
                treeAgencyHierarchy.DataFieldParentID = "ParentNodeID";
                treeAgencyHierarchy.DataValueField = "NodeID";
                treeAgencyHierarchy.DataBind();
            }
            else if (lstTreeData.IsNotNull() && lstTreeData.Count == 1)
            {
                ParentRootNodeCount = lstTreeData.Count;
                RootNodeID = lstTreeData.FirstOrNew().NodeID;
                treeAgencyHierarchy.Visible = false;
                hdnRootNodeId.Value = RootNodeID.ToString();
                treeChildAgencyHierarchy.Visible = true;
                BindTreeByRootNodeID();
            }
        }
        /// <summary>
        /// BindTreeByRootNodeID
        /// </summary>
        private void BindTreeByRootNodeID()
        {
            Presenter.GetTreeDataByRootNodeID();
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
                fsucAgencyHierarchyList.SubmitButtonText = "Expand";
            }
            else
            {
                node.ExpandChildNodes();
                fsucAgencyHierarchyList.SubmitButtonText = "Collapse";
            }
        }
        #endregion

        #endregion
    }
}