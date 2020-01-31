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


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class InstitutionHierarchyList : BaseWebPage, IInstitutionHierarchyListView
    {
        #region Variables

        #region Private Variables

        private InstitutionHierarchyListPresenter _presenter=new InstitutionHierarchyListPresenter();
        private Int32 _tenantId;
        private List<String> _lstCodeForColumnConfig = new List<String>(); //UAT-3952

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        
        public InstitutionHierarchyListPresenter Presenter
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

        //public List<GetDepartmentTree> lstTreeData
        //{
        //    set;
        //    get;
        //}

        //UAT-3369
        public List<InstituteHierarchyNodesList> lstTreeHierarchyData
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

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IInstitutionHierarchyListView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 InstitutionNodeId { get; set; }

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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Checks if the TenantId is present in Query String.
                //UAT-3952
                Presenter.BindPageControls();
              
                if (!Request.QueryString["DepartmentProgramId"].IsNullOrEmpty())
                {
                    DepartmentPrgMappingId = Convert.ToInt32(Request.QueryString["DepartmentProgramId"]);
                }

                if (!Request.QueryString["TenantId"].IsNullOrEmpty())
                {
                    SelectedTenant = Convert.ToInt32(Request.QueryString["TenantId"]);
                    BindTree();
                }

                //SelectedTenant = 2;
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    if (DepartmentPrgMappingId != 0)
                    {
                        hdnSelectedNode.Value = DepartmentPrgMappingId.ToString();
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
                //Gets the detail of a node.
                InstituteHierarchyNodesList item = (InstituteHierarchyNodesList)e.Node.DataItem;

                //Sets the navigation URL, Color Code and Font Code as per node type.
                ComplianceTreeUIContract complianceTreeUIContract = Presenter.GetTreeNodeDetails(item.UICode, item.DataID, item.ParentDataID, item.ParentNodeID, item.NodeID, item.MappingID, item.Value);

                //e.Node.NavigateUrl = complianceTreeUIContract.NavigateURL + "&TenantId=" + SelectedTenant;
                e.Node.Value = complianceTreeUIContract.Value;
                e.Node.ForeColor = System.Drawing.Color.FromName(complianceTreeUIContract.ColorCode);
                e.Node.Font.Bold = complianceTreeUIContract.FontBold;
                ////By default selects the Department as selected.
                if (DepartmentPrgMappingId != 0 && item.MappingID == DepartmentPrgMappingId)
                {
                    e.Node.Selected = true;

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
                    Int32 depPrgMappingId;
                    if (hdnSelectedNode.Value.Contains("_"))
                    {
                        var selectedNode = hdnSelectedNode.Value.Split('_');
                        depPrgMappingId = Convert.ToInt32(selectedNode[2]);
                    }
                    else
                    {
                        depPrgMappingId = Convert.ToInt32(hdnSelectedNode.Value);
                    }
                    Presenter.GetHierarchyLabel(depPrgMappingId);
                    hdnLabel.Value = HierarchyLabel;
                    if (InstitutionNodeId != 0)
                        hdnInstitutionNodeId.Value = InstitutionNodeId.ToString();
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
            //treeDepartment.DataSource = lstTreeData; //UAT-3369
            treeDepartment.DataSource = lstTreeHierarchyData;
            treeDepartment.DataTextField = "Value";
            treeDepartment.DataFieldID = "NodeID";
            treeDepartment.DataFieldParentID = "ParentNodeID";
            treeDepartment.DataBind();

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
    }
}

