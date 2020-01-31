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
using CoreWeb.ComplianceOperations.Pages;
using CoreWeb.CommonControls.Views;
using INTSOF.UI.Contract.CommonControls;
using System.Web.UI.WebControls;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class InstitutionNodeHierarchyList : BaseWebPage, IInstitutionNodeHierarchyListView
    {
        #region Variables

        #region Private Variables

        private InstitutionNodeHierarchyListPresenter _presenter = new InstitutionNodeHierarchyListPresenter();
        private Int32 _tenantId;

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties

        private List<String> _lstCodeForColumnConfig = new List<String>(); //UAT-3952

        #endregion

        #region Public Properties


        public InstitutionNodeHierarchyListPresenter Presenter
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

        public List<GetDepartmentTree> lstTreeData
        {
            set;
            get;
        }

        //UAT-3369
        public List<InstituteHierarchyNodesList> lstTreeHierarchyData
        {
            set;
            get;
        }

        public List<GetInstituteHierarchyOrderTree> lstOrderTreeData
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

        public Int32 DepartmentPrgMappingId
        {
            get;
            set;
        }

        public String DelemittedDepartmentPrgMappingIds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IInstitutionNodeHierarchyListView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String InstitutionNodeId { get; set; }

        public String HierarchyLabel { get; set; }

        //public Int32 TenantId
        //{
        //    get
        //    {
        //        if (_tenantId == 0)
        //            _tenantId = Presenter.GetTenantId();
        //        return _tenantId;
        //    }
        //    set { _tenantId = value; }
        //}

        //public List<Tenant> ListTenants
        //{
        //    set;
        //    get;
        //}

        public Int32 SelectedTenant
        {
            set;
            get;
        }

        public String ScreenName
        {
            set;
            get;
        }

        public string ScreenNameForPermission
        {
            set;
            get;
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

        public string IsRequestFromAddRotationScreen
        {
            set;
            get;
        }

        #endregion

        #endregion

        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Form.Attributes.Add("role", "application");
                //Checks if the TenantId is present in Query String.

                //if (!Request.QueryString["DepartmentProgramId"].IsNullOrEmpty())
                //{
                //    DepartmentPrgMappingId = Convert.ToInt32(Request.QueryString["DepartmentProgramId"]);
                //}

                //UAT-3952
                Presenter.BindPageControls();

                
                if (!Request.QueryString["DelemittedDeptPrgMapIds"].IsNullOrEmpty())
                {
                    DelemittedDepartmentPrgMappingIds = Convert.ToString(Request.QueryString["DelemittedDeptPrgMapIds"]);
                }
                //UAT-1181: Ability to restrict additional nodes to the order queue
                if (!Request.QueryString["ScreenName"].IsNullOrEmpty())
                {
                    CurrentViewContext.ScreenName = Convert.ToString(Request.QueryString["ScreenName"]);
                }

                if (!Request.QueryString["ScreenNameForPermission"].IsNullOrEmpty())
                {
                    CurrentViewContext.ScreenNameForPermission = Convert.ToString(Request.QueryString["ScreenNameForPermission"]);
                }

                if (!Request.QueryString["IsRequestFromAddRotationScreen"].IsNullOrEmpty())
                {
                    CurrentViewContext.IsRequestFromAddRotationScreen = Convert.ToString(Request.QueryString["IsRequestFromAddRotationScreen"]);
                }

                if (!Request.QueryString["TenantId"].IsNullOrEmpty())
                {
                    SelectedTenant = Convert.ToInt32(Request.QueryString["TenantId"]);
                    hdnTenantId.Value = SelectedTenant.ToString();
                    BindTree();
                }

                //SelectedTenant = 2;
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    //if (DepartmentPrgMappingId != 0)
                    //{
                    //    hdnSelectedNode.Value = DepartmentPrgMappingId.ToString();
                    //}

                    if (!DelemittedDepartmentPrgMappingIds.IsNullOrEmpty())
                    {
                        hdnSelectedNode.Value = DelemittedDepartmentPrgMappingIds + ",";
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

                Presenter.OnViewLoaded();
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

        #region TreeView List
        /// <summary>
        /// Bounds the data to tree view (which includes list of packages, categories, items and attributes).
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void TreeDepartment_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
          
            try
            {
                ComplianceTreeUIContract complianceTreeUIContract = new ComplianceTreeUIContract();
               
                //Gets the detail of a node.
                //if (CurrentViewContext.ScreenName.IsNullOrEmpty() || CurrentViewContext.ScreenName == "BackgroundScreen")

              
                if (CurrentViewContext.ScreenName != "OrderQueue")
                {
                    GetDepartmentTree item;
                    if (e.Node.DataItem is GetDepartmentTree)
                    {
                        item = e.Node.DataItem as GetDepartmentTree;

                    }
                    else if (e.Node.DataItem is InstituteHierarchyNodesList)
                    {
                        var temp = e.Node.DataItem as InstituteHierarchyNodesList;
                        item = new GetDepartmentTree();
                        item.UICode = temp.UICode;
                        item.DataID = temp.DataID;
                        item.ParentDataID = temp.ParentDataID;
                        item.ParentNodeID = temp.ParentNodeID;
                        item.NodeID = temp.NodeID;
                        item.MappingID = temp.MappingID;
                        item.Value = temp.Value;

                    }
                    else {
                        item = new GetDepartmentTree();
                    }

                    if (CurrentViewContext.ScreenNameForPermission == "ScreenNameForPermissionReadOnly")
                    {
                        if (!item.PermissionName.IsNullOrEmpty() && item.PermissionName == Permissions.NoAccess.ToString()) // Update condition for UAT-4471
                        {
                            e.Node.Enabled = false;
                        }
                    }


                   

                    //Sets the navigation URL, Color Code and Font Code as per node type.
                    complianceTreeUIContract = Presenter.GetTreeNodeDetails(item.UICode, item.DataID, item.ParentDataID, item.ParentNodeID, item.NodeID, item.MappingID, item.Value);

                    //e.Node.NavigateUrl = complianceTreeUIContract.NavigateURL + "&TenantId=" + SelectedTenant;
                    e.Node.Value = complianceTreeUIContract.Value;
                  
                    e.Node.ForeColor = System.Drawing.Color.FromName(complianceTreeUIContract.ColorCode);
                    e.Node.Font.Bold = complianceTreeUIContract.FontBold;
                    ////By default selects the Department as selected.
                    //if (DepartmentPrgMappingId != 0 && item.MappingID == DepartmentPrgMappingId)
                    //{
                    //    e.Node.Selected = true;
                    //}
                    if (!DelemittedDepartmentPrgMappingIds.IsNullOrEmpty())
                    {
                        String[] arrayDeptPrgMapIDs = DelemittedDepartmentPrgMappingIds.Split(',');
                        foreach (String DeptPrgMapID in arrayDeptPrgMapIDs)
                        {
                            if (Convert.ToInt32(DeptPrgMapID) != 0 && item.MappingID == Convert.ToInt32(DeptPrgMapID))
                            {
                                e.Node.Checked = true;
                            }
                        }
                    }
                }
                //UAT-1181: Ability to restrict additional nodes to the order queue
                else
                {
                    GetInstituteHierarchyOrderTree item = (GetInstituteHierarchyOrderTree)e.Node.DataItem;
                    //Sets the navigation URL, Color Code and Font Code as per node type.
                    complianceTreeUIContract = Presenter.GetTreeNodeDetails(item.UICode, item.DataID, item.ParentDataID, item.ParentNodeID, item.NodeID, item.MappingID, item.Value);

                    //e.Node.NavigateUrl = complianceTreeUIContract.NavigateURL + "&TenantId=" + SelectedTenant;
                    e.Node.Value = complianceTreeUIContract.Value;
                    e.Node.ForeColor = System.Drawing.Color.FromName(complianceTreeUIContract.ColorCode);
                    e.Node.Font.Bold = complianceTreeUIContract.FontBold;

                    if (!DelemittedDepartmentPrgMappingIds.IsNullOrEmpty())
                    {
                        String[] arrayDeptPrgMapIDs = DelemittedDepartmentPrgMappingIds.Split(',');
                        foreach (String DeptPrgMapID in arrayDeptPrgMapIDs)
                        {
                            if (Convert.ToInt32(DeptPrgMapID) != 0 && item.MappingID == Convert.ToInt32(DeptPrgMapID))
                            {
                                e.Node.Checked = true;
                            }
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

        #endregion

        #region Button Events

        protected void btnOk_Click(Object sender, EventArgs e)
        {
            try
            {
                if (!hdnSelectedNode.Value.IsNullOrEmpty())
                {
                    hdnSelectedNode.Value = hdnSelectedNode.Value.Remove(hdnSelectedNode.Value.Length - 1);
                    //  hdnLabel.Value = hdnLabel.Value.Remove(hdnLabel.Value.Length - 2);
                    Presenter.GetHierarchyLabel(hdnSelectedNode.Value);
                    hdnLabel.Value = HierarchyLabel.Remove(HierarchyLabel.Length - 2);
                    if (!InstitutionNodeId.IsNullOrEmpty())
                        hdnInstitutionNodeId.Value = InstitutionNodeId.Remove(InstitutionNodeId.Length - 1);
                }
                #region UAT-3952
                CurrentViewContext.isHierarchyCollapsed = Convert.ToBoolean(hdnIsHierarchyCollapsed.Value);
                Dictionary<Int32, Boolean> dctUserScreenColumnMpng = new Dictionary<Int32, Boolean>();
                dctUserScreenColumnMpng.Add(CurrentViewContext.screenColumnID, !CurrentViewContext.isHierarchyCollapsed);
                Presenter.SaveUserScreenColumnMapping(dctUserScreenColumnMpng);
                #endregion

                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
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

        /// <summary>
        /// To bind tree data
        /// </summary>
        private void BindTree()
        {
            Presenter.GetTreeData();
            if (CurrentViewContext.ScreenName == "OrderQueue")
            {
                treeDepartment.DataSource = lstOrderTreeData;
            }
            else
            {
                //UAT-3369
                if (!lstTreeData.IsNullOrEmpty())
                    treeDepartment.DataSource = lstTreeData;
                else if (!lstTreeHierarchyData.IsNullOrEmpty())
                    treeDepartment.DataSource = lstTreeHierarchyData;
                else
                    treeDepartment.DataSource = new List<InstituteHierarchyNodesList>();
            }
            treeDepartment.DataTextField = "Value";
            treeDepartment.DataFieldID = "NodeID";
            treeDepartment.DataFieldParentID = "ParentNodeID";
            treeDepartment.DataBind();
            treeDepartment.Focus();

            //UAT-3952
            Telerik.Web.UI.RadTreeNode node = this.treeDepartment.Nodes[0];
            if (CurrentViewContext.isHierarchyCollapsed == true)
            {
                node.CollapseChildNodes();
                fsucInstitutionHierarchyList.SubmitButtonText = "Expand";
            }
            else
            {
                node.ExpandChildNodes();
                fsucInstitutionHierarchyList.SubmitButtonText = "Collapse";
            }

        }

        #endregion

        protected void fsucInstitutionHierarchyList_SubmitClick(Object sender, EventArgs e)
        {

            Telerik.Web.UI.RadTreeNode node = this.treeDepartment.Nodes[0];
            node.CollapseChildNodes();

          

            // IList<Telerik.Web.UI.RadTreeNode> allNodes = treeDepartment.Nodes;

            //Telerik.Web.UI.RadTreeNodeCollection tnd = treeDepartment.Nodes.FindNodeByAttribute("Level", "0").Nodes;

            //foreach (Telerik.Web.UI.RadTreeNode tn in treeDepartment.Nodes)
            //{
            //    if (tn.Level == 0)
            //    {
            //        tn.CollapseChildNodes();
            //    }
            //}
        }


    }
}

