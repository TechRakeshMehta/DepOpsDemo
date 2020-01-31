using CoreWeb.AgencyHierarchy.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using Telerik.Web.UI;

namespace CoreWeb.AgencyHierarchy.Pages
{
    public partial class AgencyHierarchy : BaseWebPage, IAgencyHierarchyView
    {

        #region [Variables / Properties]

        #region [Private Variables]

        private AgencyHierarchyPresenter _presenter = new AgencyHierarchyPresenter();
        private Int32 _tenantId;
        String _viewType;

        #endregion

        public IAgencyHierarchyView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public AgencyHierarchyPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
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

        List<AgencyHierarchyContract> IAgencyHierarchyView.lstAgencyHierarchyRootNodes
        {
            get;
            set;
        }

        List<AgencyHierarchyContract> IAgencyHierarchyView.lstAgencyHierarchyTreeData
        {
            get;
            set;
        }

        Int32 IAgencyHierarchyView.SelectedRootNodeID
        {
            get
            {
                if (!ddlAgencyHierarchy.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(ddlAgencyHierarchy.SelectedValue);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ddlAgencyHierarchy.SelectedValue = value.ToString();
            }
        }

        public Int32 SelectedAgencyHierarchyNodeID
        {
            get
            {
                if (!string.IsNullOrEmpty(hdnSelectedNode.Value))
                {
                    return Convert.ToInt32(hdnSelectedNode.Value);
                }
                else
                {
                    return AppConsts.NONE;
                }
            }
        }

        #endregion

        #region [Page Events]

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                var updatePanel = this.Master.FindControl("UpdatePanel1") as System.Web.UI.UpdatePanel;
                updatePanel.UpdateMode = System.Web.UI.UpdatePanelUpdateMode.Conditional;
                updatePanel.ChildrenAsTriggers = false;

                base.Title = "Agency Hierarchy";
                base.SetPageTitle("Agency Hierarchy");
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
            try
            {
                SetBackwardNavigation();
                //Hiding top page
                CoreWeb.Shell.MasterPages.DefaultMaster masterPage = this.Master as CoreWeb.Shell.MasterPages.DefaultMaster;
                //if (masterPage != null)
                //    masterPage.HideTitleBars(true);

                if (masterPage != null)
                {
                    masterPage.HideTitleBars(true);
                }

                base.Title = "Agency Hierarchy";
                base.SetPageTitle("Agency Hierarchy");

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindDropDowns();

                    //Getting SelectedRootNodeID from Query String
                    if (!Request.QueryString["SelectedRootNodeID"].IsNullOrEmpty())
                    {
                        int _selectedRootNodeID = 0;
                        Int32.TryParse(Request.QueryString["SelectedRootNodeID"], out _selectedRootNodeID);
                        CurrentViewContext.SelectedRootNodeID = _selectedRootNodeID;
                    }
                    if (!Request.QueryString["SelectedHierarchyID"].IsNullOrEmpty())
                    {
                        int _selectedHierarchyID = 0;
                        Int32.TryParse(Request.QueryString["SelectedHierarchyID"], out _selectedHierarchyID);
                        if (_selectedHierarchyID > AppConsts.NONE)
                        {
                            hdnSelectedNode.Value = _selectedHierarchyID.ToString();
                        }
                        BindAgencyHierarchyTree(true, false);
                    }


                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenDefaultScreen();", true);
                }
                else
                {
                    BindDataAfterRootNodeAdded();
                    //for getting if page load is called by tree itself or right panel
                    Boolean asynchronousRequest = System.Web.UI.ScriptManager.GetCurrent(this).IsInAsyncPostBack;
                    if (ddlAgencyHierarchy.SelectedValue != String.Empty)
                    {
                        CurrentViewContext.SelectedRootNodeID = Convert.ToInt32(ddlAgencyHierarchy.SelectedValue);
                    }
                    Boolean ifRootNodeChanged = false; ;
                    if (!hdnIsRootNodeChanged.Value.IsNullOrEmpty())
                        ifRootNodeChanged = Convert.ToBoolean(hdnIsRootNodeChanged.Value);

                    WclSplitBar1.Visible = true;
                    BindAgencyHierarchyTree(asynchronousRequest, false);

                    if (ifRootNodeChanged || String.Compare(hdnIsNewRootNodeAdded.Value, "true", true) == AppConsts.NONE)
                    {
                        hdnIsNewRootNodeAdded.Value = "false";
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenDefaultScreen();", true);
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

        #region [Tree Events]

        protected void treeAgencyHierarchy_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            try
            {
                AgencyHierarchyContract item = (AgencyHierarchyContract)e.Node.DataItem;
                e.Node.NavigateUrl = "AgencyHierarchyControls.aspx?" + "SelectedAgencyHierarchyNodeID=" + item.NodeID.ToString() + "&SelectedRootNodeID=" + CurrentViewContext.SelectedRootNodeID;
                e.Node.ForeColor = System.Drawing.Color.FromName("DarkBlue");
                e.Node.Font.Bold = true;
                e.Node.Target = "childpageframe";
                e.Node.Expanded = true;

                if (!string.IsNullOrEmpty(hdnSelectedNode.Value))
                {
                    if (Convert.ToInt32(hdnSelectedNode.Value) == item.NodeID)
                    {
                        e.Node.Selected = true;
                        hdnDefaultScreen.Value = e.Node.NavigateUrl;
                    }
                }
                else
                {
                    if (CurrentViewContext.SelectedRootNodeID == item.NodeID)
                    {
                        e.Node.Selected = true;
                        hdnDefaultScreen.Value = e.Node.NavigateUrl;
                    }
                }
                Boolean IfChildNodeExist = CurrentViewContext.lstAgencyHierarchyTreeData.Any(x => x.ParentNodeID == item.NodeID);
                if (IfChildNodeExist)
                {
                    e.Node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
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

        //protected void treeAgencyHierarchy_NodeClick(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Node.IsNotNull())
        //        {
        //            //hdnNodeId.Value = e.Node.Value;
        //            e.Node.Selected = true;
        //            //NodeID = Convert.ToInt32(e.Node.Value);
        //            //Presenter.GetAgencyDetailByNodeId();
        //        }
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}

        #endregion

        #region [Private Methods]

        private void BindDropDowns()
        {
            Presenter.GetRootNodes();
            ddlAgencyHierarchy.DataSource = CurrentViewContext.lstAgencyHierarchyRootNodes;
            ddlAgencyHierarchy.DataBind();
            ddlAgencyHierarchy.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", AppConsts.ZERO));
        }

        private void BindAgencyHierarchyTree(Boolean getFreshData, Boolean ifNotPostBack)
        {
            if (getFreshData)
            {
                ReloadTreeData();
            }
            BindTree();

            if (getFreshData && !ifNotPostBack)
            {
                ExpandParentNodes();
            }
        }

        private void ReloadTreeData()
        {
            Presenter.GetTreeDataByRootNodeID();

            //lstCurrentTreeData = lstTreeData.Where(x => x.Level == 1 || x.Level == 2).ToList();
            //String selectedNode = hdnSelectedNode.Value;
            //if (selectedNode != String.Empty)
            //{
            //    List<GetDepartmentTree> currentTreeNodeList = lstCurrentTreeData;
            //    List<GetDepartmentTree> parentNodeList = new List<GetDepartmentTree>();
            //    List<GetDepartmentTree> nodeListToBeAdded = new List<GetDepartmentTree>();
            //    String nodeId = String.Empty;
            //    String uiCode = String.Empty;
            //    string nodeValue = String.Empty;
            //    string[] nodes = selectedNode.Split('#');
            //    if (!nodes.IsNullOrEmpty())
            //    {
            //        nodeValue = nodes[0];
            //        nodeId = nodes[1];
            //        uiCode = selectedNode.Substring(selectedNode.IndexOf("_") + 1, selectedNode.Remove(0, selectedNode.IndexOf("_") + 1).IndexOf("_"));
            //        GetDepartmentTree currentNode = lstTreeData.FirstOrDefault(x => x.NodeID == nodeId && x.UICode == uiCode);
            //        var uicode = lstTreeData.FirstOrDefault(x => x.UICode == uiCode);
            //        var uicode2 = lstTreeData.FirstOrDefault(x => x.NodeID == nodeId);
            //        GetDepartmentTree parentNodes = null;
            //        if (currentNode.IsNotNull())
            //        {
            //            parentNodes = lstTreeData.FirstOrDefault(x => x.NodeID == currentNode.ParentNodeID);
            //        }

            //        if (parentNodes != null)
            //        {
            //            //get All the Parents Node of current Node
            //            parentNodeList.Add(parentNodes);
            //            while (parentNodes.ParentDataID != null && parentNodes.Level != 1)
            //            {
            //                parentNodes = lstTreeData.FirstOrDefault(x => x.NodeID == parentNodes.ParentNodeID);
            //                parentNodeList.Add(parentNodes);
            //            }
            //            List<String> parentNodeIds = null;
            //            parentNodeIds = parentNodeList.Select(x => x.NodeID).ToList();

            //            //get all the immidiate child of parents
            //            List<GetDepartmentTree> childNodeList = null;
            //            childNodeList = lstTreeData.Where(x => parentNodeIds.Contains(x.ParentNodeID)).ToList();
            //            List<String> childNodeIds = null;
            //            childNodeIds = childNodeList.Select(x => x.NodeID).ToList();
            //            nodeListToBeAdded = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeID)).ToList();
            //            AddNodesToCurrentTreeData(currentTreeNodeList, nodeListToBeAdded);
            //        }
            //        lstCurrentTreeData = currentTreeNodeList;
            //    }
            //}
        }

        private void BindTree()
        {
            treeAgencyHierarchy.DataSource = CurrentViewContext.lstAgencyHierarchyTreeData.OrderBy(o=>o.DisplayOrder);
            treeAgencyHierarchy.DataTextField = "Value";
            treeAgencyHierarchy.DataFieldID = "NodeID";
            treeAgencyHierarchy.DataFieldParentID = "ParentNodeID";
            treeAgencyHierarchy.DataValueField = "NodeID";
            treeAgencyHierarchy.DataBind();
        }

        private void ExpandParentNodes()
        {
            if (SelectedAgencyHierarchyNodeID != 0)
            {
                var parentnode = treeAgencyHierarchy.FindNodeByValue(SelectedAgencyHierarchyNodeID.ToString()).ParentNode;
                while (parentnode != null)
                {
                    parentnode.Expanded = true;
                    parentnode = parentnode.ParentNode;
                }
            }
        }

        private void SetBackwardNavigation()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>
            {
                    {"Child", @"~\AgencyHierarchy\ManageAgencyHierarchy.ascx"},
                    {"CancelClicked", "CancelClicked"}                    
            };
            lnkGoBack.HRef = String.Format("~/AgencyHierarchy/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
        }

        #endregion

        private void BindDataAfterRootNodeAdded()
        {
            if (!hdnIsNewRootNodeAdded.Value.IsNullOrEmpty() && String.Compare(hdnIsNewRootNodeAdded.Value, "true", true) == AppConsts.NONE)
            {
                BindDropDowns();
                ddlAgencyHierarchy.SelectedValue = hdnSelectedNode.Value = hdnAddedRootNodeId.Value;
                hdnAddedRootNodeId.Value = AppConsts.ZERO;
            }
        }
    }
}