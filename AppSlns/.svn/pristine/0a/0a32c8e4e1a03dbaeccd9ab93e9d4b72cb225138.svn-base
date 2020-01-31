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
using INTSOF.UI.Contract.BkgSetup;
using System.Web.UI.WebControls;


#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManagePackageServiceHierarchy : BaseWebPage, IManagePackageServiceHierarchyView
    {
        #region Variables

        #region Private variables
        private ManagePackageServiceHierarchyPresenter _presenter = new ManagePackageServiceHierarchyPresenter();
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _tenantid;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #region Public Variables

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
                //this.treePackages.NodeCollapse += new RadTreeViewEventHandler(this.treePackages_NodeCollapse);
                base.Title = "Screening Package Setup";
                base.SetModuleTitle("Screening Package Setup");
                base.SetPageTitle("Screening Package Setup");
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
                //HandleComplianceNodeMovedCase();

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindTenant();
                    /*UAT-3157*/
                    if (IsAdminLoggedIn == true)
                        GetPreferredSelectedTenant();
                    BindPackageDropDown();
                    /*END UAT-3157*/

                    //SetDefaultSelectedTenantId();
                    ApplyActionLevelPermission(ActionCollection, "Manage Package Service SetUp");

                    /* Comment Belo Code Package DropDown Changes :Release_68
                    * if (SelectedTenantId > 0)
                    {
                        Presenter.GetTreeData();
                        lstCurrentTreeData = lstTreeData.Where(x => x.Code.Equals("LPKG") || x.Code.Equals("PKG")).Select(y => y).ToList();
                    }*/
                }
                Boolean asynchronousRequest = System.Web.UI.ScriptManager.GetCurrent(this).IsInAsyncPostBack;
                Boolean IfDragEventIsFired = hdnIfDragEventIsFired.Value != String.Empty ? Convert.ToBoolean(hdnIfDragEventIsFired.Value) : false;
                hdnIfDragEventIsFired.Value = String.Empty;
                Presenter.OnViewLoaded();

                ////Hiding top page
                CoreWeb.Shell.MasterPages.DefaultMaster masterPage = this.Master as CoreWeb.Shell.MasterPages.DefaultMaster;
                if (masterPage != null)
                {
                    masterPage.HideTitleBars(true);
                }
                /* Commented Code for Package DropDown Changes :Release_68
                if (SelectedTenantId > 0)
                {
                    BindTree(asynchronousRequest);
                }
                else
                {
                    treePackages.DataSource = new List<PkgSvcSetupContract>();
                    treePackages.DataBind();
                }*/

                #region Package DropDown Changes :Release_68

                if (!hdnIsSearchClicked.Value.IsNullOrEmpty() && Convert.ToInt32(hdnIsSearchClicked.Value) == AppConsts.ONE && SelectedTenantId > AppConsts.NONE
                    && CurrentViewContext.SelectedBkgPackageIdList.Count > AppConsts.NONE)
                {
                    BindTree(asynchronousRequest);
                }
                else
                {
                    treePackages.DataSource = new List<PkgSvcSetupContract>();
                    treePackages.DataBind();
                }
                #endregion
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

        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {
            ddlTenant.Items.Insert(0, new RadComboBoxItem("--SELECT--"));
            //   ddlTenant.SelectedValue = "0";
        }

        /// <summary>
        /// Binds the attributes as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                BindPackageDropDown();
                treePackages.DataSource = new List<PkgSvcSetupContract>();
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

        #endregion

        #region Tree events

        protected void treePackages_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            PkgSvcSetupContract item = (PkgSvcSetupContract)e.Node.DataItem;
            e.Node.NavigateUrl = Presenter.GetNodeDetails(item);
            e.Node.Value = item.NodeId;
            e.Node.Target = "childpageframe";
            e.Node.Font.Bold = true;
            e.Node.ForeColor = System.Drawing.Color.Black;
            if (item.ParentNodeId.IsNull() || (!nodesTobeExpanded.IsNullOrEmpty() && nodesTobeExpanded.Contains(item.NodeId)))
            {
                e.Node.Expanded = true;
            }
            Boolean IfChildNodeExist = lstTreeData.Any(x => x.ParentNodeId == item.NodeId);
            if (IfChildNodeExist)
            {
                e.Node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
            }
            //if (!String.IsNullOrEmpty(item.ColorCode))
            //{
            //    e.Node.ForeColor = System.Drawing.Color.FromName(item.ColorCode);
            //}
            if (item.NodeId.Equals(hdnSelectedNode.Value))
            {
                e.Node.Selected = true;
                Boolean IsAttributeExistForAttGrp = lstTreeData.Any(x => x.ParentNodeId == SelectedAttributeNode);
                if (!IsAttributeExistForAttGrp)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "NavigateToSelectedNode('" + e.Node.NavigateUrl + "');", true);
                }
            }

            if (item.Code == "LPKG")
                e.Node.ImageUrl = "~/App_Themes/Default/images/icons/pkgs.gif";
            else if (item.Code == "PKG")
                e.Node.ImageUrl = "~/App_Themes/Default/images/icons/Package.png";
            else if (item.Code == "SVCG")
                e.Node.ImageUrl = "~/App_Themes/Default/images/icons/cats.gif";
            else if (item.Code == "SVC" || item.Code == "SVCC")
                e.Node.ImageUrl = "~/App_Themes/Default/images/icons/cat.gif";
            else if (item.Code == "ATTG")
                e.Node.ImageUrl = "~/App_Themes/Default/images/icons/attrs.gif";
            else if (item.Code == "ATT")
                e.Node.ImageUrl = "~/App_Themes/Default/images/icons/attr.gif";
        }

        protected void treePackages_NodeExpand(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            List<PkgSvcSetupContract> currentTreeNodeList = lstCurrentTreeData;
            List<PkgSvcSetupContract> childNodeList = new List<PkgSvcSetupContract>();

            String currentNodeValue = e.Node.Value;
            String nodeId = String.Empty;
            String uiCode = String.Empty;

            nodeId = currentNodeValue;
            PkgSvcSetupContract currentNode = null;
            currentNode = lstTreeData.FirstOrDefault(x => x.NodeId == nodeId);
            childNodeList = lstTreeData.Where(x => x.ParentNodeId == currentNode.NodeId).ToList();
            List<String> childNodeIds = childNodeList.Select(x => x.NodeId).ToList();
            if (childNodeList.Count > AppConsts.NONE)
            {
                List<PkgSvcSetupContract> subChildNodeList = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeId)).ToList();
                AddNodesToCurrentTreeData(currentTreeNodeList, subChildNodeList);
            }
            lstCurrentTreeData = currentTreeNodeList;

        }

        protected void treePackages_NodeCollapse(object sender, RadTreeNodeEventArgs e)
        {
            List<PkgSvcSetupContract> currentTreeNodeList = lstCurrentTreeData;
            List<PkgSvcSetupContract> childNodeList = new List<PkgSvcSetupContract>();
            String nodeId = String.Empty;
            String uiCode = String.Empty;
            string navigateUrl = e.Node.NavigateUrl;
            nodeId = navigateUrl.Substring(navigateUrl.IndexOf("=") + 1, (navigateUrl.IndexOf("&") - navigateUrl.IndexOf("=") - 1));
            //uiCode = navigateUrl.Substring(navigateUrl.IndexOf("_") + 1, navigateUrl.Remove(0, navigateUrl.IndexOf("_") + 1).IndexOf("_"));
            if (uiCode.StartsWith("L"))
            {
                PkgSvcSetupContract currentNode = lstTreeData.FirstOrDefault(x => x.NodeId == nodeId);
                childNodeList = lstTreeData.Where(x => x.ParentNodeId == currentNode.NodeId).ToList();
                List<String> childNodeIds = childNodeList.Select(x => x.NodeId).ToList();
                if (childNodeList.Count > AppConsts.NONE)
                {
                    List<PkgSvcSetupContract> childLabelNodeList = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeId)).ToList();
                    foreach (PkgSvcSetupContract childLabel in childLabelNodeList)
                    {
                        currentTreeNodeList.Remove(childLabel);
                    }
                }
                lstCurrentTreeData = currentTreeNodeList;
                BindTreePackages();
            }
            else
            {
                PkgSvcSetupContract currentNode = lstTreeData.FirstOrDefault(x => x.NodeId == nodeId);
                childNodeList = lstTreeData.Where(x => x.ParentNodeId == currentNode.NodeId).ToList();
                List<String> childNodeIds = childNodeList.Select(x => x.NodeId).ToList();
                if (childNodeList.Count > AppConsts.NONE)
                {
                    List<PkgSvcSetupContract> subChildNodeList = lstTreeData.Where(x => childNodeIds.Contains(x.ParentNodeId)).ToList();
                    foreach (PkgSvcSetupContract childLabel in subChildNodeList)
                    {
                        currentTreeNodeList.Remove(childLabel);
                    }
                }
                lstCurrentTreeData = currentTreeNodeList;
            }

        }
        #endregion

        #endregion

        #region Properties

        #region Public Properties
        public ManagePackageServiceHierarchyPresenter Presenter
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

        public IManagePackageServiceHierarchyView CurrentViewContext
        {
            get
            {
                return this;
            }
        }


        public List<PkgSvcSetupContract> lstTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("PkgSvcSetupContract") != null)
                    return (List<PkgSvcSetupContract>)(SysXWebSiteUtils.SessionService.GetCustomData("PkgSvcSetupContract"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("PkgSvcSetupContract", value);
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

        public List<String> nodesTobeExpanded { get; set; }

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
                if (ddlTenant.SelectedValue.IsNullOrEmpty())
                    return 0;
                _selectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
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

        public List<PkgSvcSetupContract> lstCurrentTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("PkgSvcSetupContractCurrentData") != null)
                    return (List<PkgSvcSetupContract>)(SysXWebSiteUtils.SessionService.GetCustomData("PkgSvcSetupContractCurrentData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("PkgSvcSetupContractCurrentData", value);
            }
        }

        public String SelectedAttributeNode
        {
            get;
            set;
        }
        #region Package DropDown Changes :UAT-1116
        List<BackgroundPackage> IManagePackageServiceHierarchyView.lstBackgroundPackage
        {
            set;
            get;
        }

        List<Int32> IManagePackageServiceHierarchyView.SelectedBkgPackageIdList
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < chkBkgPackages.Items.Count; i++)
                {
                    if (chkBkgPackages.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(chkBkgPackages.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < chkBkgPackages.Items.Count; i++)
                {
                    chkBkgPackages.Items[i].Checked = value.Contains(Convert.ToInt32(chkBkgPackages.Items[i].Value));
                }

            }
        }
        #endregion

        //START UAT-3157
        Int32 IManagePackageServiceHierarchyView.PreferredSelectedTenantID
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
        //END UAT-3157

        #endregion

        #region Private Properties

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
                paneTenant.Visible = false;
            }
        }


        private void BindTree(Boolean getFefreshData)
        {
            if (getFefreshData)
            {
                //Package setup Changes UAT-1116 :Package selection combo box on package screens
                if (!hdnIsSearchClicked.Value.IsNullOrEmpty() && Convert.ToInt32(hdnIsSearchClicked.Value) == AppConsts.ONE)
                {
                    hdnAsynchronousRequest.Value = "1";
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ReloadPackages();", true);
                }
                ReloadTreeData();
            }
            treePackages.DataSource = lstTreeData;
            treePackages.DataTextField = "Name";
            treePackages.DataFieldID = "NodeId";
            treePackages.DataFieldParentID = "ParentNodeId";
            //treePackages.
            treePackages.DataBind();
        }


        private void BindTreePackages()
        {
            treePackages.DataSource = lstCurrentTreeData;
            //var jj = lstTreeData.WhereSelect(x => x.UICode == "LPAK");
            treePackages.DataSource = lstTreeData;
            treePackages.DataTextField = "Name";
            treePackages.DataFieldID = "NodeId";
            treePackages.DataFieldParentID = "ParentNodeId";
            treePackages.DataBind();
        }


        private void ReloadTreeData()
        {
            Presenter.GetTreeData();
            lstCurrentTreeData = lstTreeData.Where(x => x.Code.Equals("LPKG") || x.Code.Equals("PKG")).Select(y => y).ToList();
            String selectedNode = hdnSelectedNode.Value;
            List<String> lstnodesTobeExpanded = new List<String>();
            if (selectedNode != String.Empty)
            {
                List<PkgSvcSetupContract> currentTreeNodeList = lstCurrentTreeData;
                List<PkgSvcSetupContract> parentNodeList = new List<PkgSvcSetupContract>();
                List<PkgSvcSetupContract> nodeListToBeAdded = new List<PkgSvcSetupContract>();
                PkgSvcSetupContract currentNode = lstTreeData.FirstOrDefault(x => x.NodeId == selectedNode);
                if (currentNode.IsNullOrEmpty())
                {
                    SelectedAttributeNode = selectedNode;
                    String[] separator = { "_" };
                    String[] selectedNodeSplit = selectedNode.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (selectedNodeSplit[0] == "ATTG")
                    {
                        if (selectedNodeSplit[4] == "NA")
                            selectedNodeSplit[0] = "SVC";
                        else
                            selectedNodeSplit[0] = "SVCC";
                        selectedNodeSplit = selectedNodeSplit.Where((val, idx) => idx != selectedNodeSplit.Length - 1).ToArray();
                        selectedNode = string.Join("_", selectedNodeSplit);
                        currentNode = lstTreeData.FirstOrDefault(x => x.NodeId == selectedNode);
                        hdnSelectedNode.Value = selectedNode;
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", strClientScript);
                    }

                }
                PkgSvcSetupContract parentNode = null;
                parentNode = lstTreeData.FirstOrDefault(x => x.NodeId == currentNode.ParentNodeId);
                lstnodesTobeExpanded.Add(selectedNode);
                if (parentNode != null)
                {
                    lstnodesTobeExpanded.Add(parentNode.NodeId);
                    parentNodeList.Add(parentNode);
                    while (parentNode.ParentNodeId != null)
                    {
                        parentNode = lstTreeData.FirstOrDefault(x => x.NodeId == parentNode.ParentNodeId);
                        parentNodeList.Add(parentNode);
                        lstnodesTobeExpanded.Add(parentNode.NodeId);
                    }
                    List<String> childNodeIdsIds = null;
                    childNodeIdsIds = parentNodeList.Select(x => x.NodeId).ToList();
                    List<PkgSvcSetupContract> childNodeList = null;
                    childNodeList = lstTreeData.Where(x => childNodeIdsIds.Contains(x.ParentNodeId)).ToList();
                    childNodeIdsIds = childNodeList.Select(x => x.NodeId).ToList();
                    nodeListToBeAdded = lstTreeData.Where(x => childNodeIdsIds.Contains(x.ParentNodeId)).ToList();
                    AddNodesToCurrentTreeData(currentTreeNodeList, nodeListToBeAdded);
                }
                lstCurrentTreeData = currentTreeNodeList;
            }
            nodesTobeExpanded = lstnodesTobeExpanded;
        }

        /// <summary>
        /// Used for getting treedata on demand i.e fetch only data which is required.
        /// </summary>

        private static void AddNodesToCurrentTreeData(List<PkgSvcSetupContract> currentTreeNodeList, List<PkgSvcSetupContract> nodeListToBeAdded)
        {
            foreach (PkgSvcSetupContract nodeToBeAdded in nodeListToBeAdded)
            {
                if (!currentTreeNodeList.Any(cond => cond.NodeId == nodeToBeAdded.NodeId && cond.ParentNodeId == nodeToBeAdded.ParentNodeId))
                    currentTreeNodeList.Add(nodeToBeAdded);
            }
        }
        #region UAT-1116:Package selection combo box on package screens

        private void BindPackageDropDown()
        {
            Presenter.GetBkgPackages();
            chkBkgPackages.DataSource = CurrentViewContext.lstBackgroundPackage;
            chkBkgPackages.DataTextField = "BPA_Name";
            chkBkgPackages.DataValueField = "BPA_ID";
            chkBkgPackages.DataBind();
            if (CurrentViewContext.lstBackgroundPackage.Count >= 10)
            {
                chkBkgPackages.Height = Unit.Pixel(200);
            }
            if (CurrentViewContext.lstBackgroundPackage.Count == AppConsts.NONE)
            {
                chkBkgPackages.EnableCheckAllItemsCheckBox = false;
            }
            else
            {
                chkBkgPackages.EnableCheckAllItemsCheckBox = true;
            }
        }

        private void ResetControl()
        {
            treePackages.DataSource = new List<PkgSvcSetupContract>();
            treePackages.DataBind();
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefreshTree();", true);
        }
        #endregion

        #region UAT-3157:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenantId.IsNullOrEmpty() || CurrentViewContext.SelectedTenantId == AppConsts.NONE)
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

        #region Apply Permissions


        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                            {
                                if (x.FeatureAction.CustomActionId == "Show Institution")
                                {
                                    ddlTenant.Enabled = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Show Institution")
                                {
                                    ddlTenant.Visible = false;
                                }
                                break;
                            }
                    }

                });
            }
        }

        #endregion




    }
}