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
using INTSOF.UI.Contract.ComplianceOperation;
#endregion

#endregion


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class InstitutionNodeHierarchyWithPermissions : BaseWebPage, IInstitutionNodeHierarchyWithPermissionsView
    {

        #region Variables

        #region Private Variables

        private InstitutionNodeHierarchyWithPermissionsPresenter _presenter = new InstitutionNodeHierarchyWithPermissionsPresenter();
        private Int32 _tenantId;
        private List<String> _lstCodeForColumnConfig = new List<String>(); //UAT-3952

        #endregion

        #endregion

        #region Public Properties

        public InstitutionNodeHierarchyWithPermissionsPresenter Presenter
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
        public IInstitutionNodeHierarchyWithPermissionsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String InstitutionNodeId { get; set; }

        public String HierarchyLabel { get; set; }

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

        List<GetDepartmentTree> IInstitutionNodeHierarchyWithPermissionsView.lstTreeData
        {
            get;
            set;
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

        #region EVENTS

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //UAT-3952
                Presenter.BindPageControls();

                if (!Request.QueryString["DelemittedDeptPrgMapIds"].IsNullOrEmpty())
                {
                    DelemittedDepartmentPrgMappingIds = Convert.ToString(Request.QueryString["DelemittedDeptPrgMapIds"]);
                }
                if (!Request.QueryString["ScreenName"].IsNullOrEmpty())
                {
                    CurrentViewContext.ScreenName = Convert.ToString(Request.QueryString["ScreenName"]);
                }

                if (!Request.QueryString["TenantId"].IsNullOrEmpty())
                {
                    SelectedTenant = Convert.ToInt32(Request.QueryString["TenantId"]);
                    hdnTenantId.Value = SelectedTenant.ToString();
                    BindTree();
                }
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
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

        #region METHODS

        /// <summary>
        /// Method to bind tree data.
        /// </summary>
        private void BindTree()
        {
            Presenter.GetTreeData();
            treeDepartment.DataSource = CurrentViewContext.lstTreeData;
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

        /// <summary>
        /// Bounds the data to tree view (which includes list of packages, categories, items and attributes).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void treeDepartment_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            try
            {
                ComplianceTreeUIContract complianceTreeUIContract = new ComplianceTreeUIContract();

                if (CurrentViewContext.ScreenName == "CommonScreen")
                {
                    GetDepartmentTree item = (GetDepartmentTree)e.Node.DataItem;
                    //Sets the navigation URL, Color Code and Font Code as per node type

                    complianceTreeUIContract = Presenter.GetTreeNodeDetails(item.UICode, item.DataID, item.ParentDataID, item.ParentNodeID, item.NodeID, item.MappingID, item.Value, item.PermissionCode);
                    e.Node.Value = complianceTreeUIContract.Value;
                    e.Node.ForeColor = System.Drawing.Color.FromName(complianceTreeUIContract.ColorCode);
                    e.Node.Font.Bold = complianceTreeUIContract.FontBold;
                    //Disable the nodes which are not accessible or you can say on which permisiions are not assigned.
                    if (complianceTreeUIContract.PermissionCode == "AAAB")
                    {
                        e.Node.Enabled = false;
                    }
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
    }
}