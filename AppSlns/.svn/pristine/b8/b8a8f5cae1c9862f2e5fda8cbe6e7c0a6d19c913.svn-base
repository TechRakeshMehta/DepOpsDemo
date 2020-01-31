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
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using CoreWeb.AgencyHierarchy.Views;
using Newtonsoft.Json;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
#endregion

#endregion
namespace CoreWeb.CommonOperations.Views
{
    public partial class InstituteHierarchyPackageList : BaseWebPage, IInstituteHierarchyPackageListView
    {
        #region Variables

        #region Private Variables

        private InstituteHierarchyPackageListPresenter _presenter = new InstituteHierarchyPackageListPresenter();
        private Int32 _tenantId;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public InstituteHierarchyPackageListPresenter Presenter
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
            get
            {
                if (!ViewState["lstTreeData"].IsNullOrEmpty())
                    return (ViewState["lstTreeData"] as List<GetDepartmentTree>);
                return new List<GetDepartmentTree>();
            }
            set
            {
                ViewState["lstTreeData"] = value;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 SelectedTenant
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenant"]);
            }
            set
            {
                ViewState["SelectedTenant"] = value;
            }
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IInstituteHierarchyPackageListView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String PackageNodeMappingID
        {
            get
            {
                if (!ViewState["PackageNodeMappingID"].IsNullOrEmpty())
                    return Convert.ToString(ViewState["PackageNodeMappingID"]);
                else
                    return String.Empty;
            }
            set
            {
                ViewState["PackageNodeMappingID"] = value;
            }
        }
        public String PackageName
        {
            get
            {
                if (!ViewState["PackageName"].IsNullOrEmpty())
                    return Convert.ToString(ViewState["PackageName"]);
                else
                    return String.Empty;
            }
            set
            {
                ViewState["PackageName"] = value;
            }
        }

        public String PackageID
        {
            get
            {
                if (!ViewState["PackageID"].IsNullOrEmpty())
                    return Convert.ToString(ViewState["PackageID"]);
                else
                    return String.Empty;
            }
            set
            {
                ViewState["PackageID"] = value;
            }
        }
        public String CompliancePackageTypeCode
        {
            get
            {
                return Convert.ToString(ViewState["CompliancePackageTypeCode"]);
            }
            set
            {
                ViewState["CompliancePackageTypeCode"] = value;
            }
        }

        public Boolean IsCompliancePackage
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsCompliancePackage"]);
            }
            set
            {
                ViewState["IsCompliancePackage"] = value;
            }
        }

        public String SelectedPackageID
        {
            get
            {
                if (!ViewState["SelectedPackageID"].IsNullOrEmpty())
                    return Convert.ToString(ViewState["SelectedPackageID"]);
                else
                    return String.Empty;
            }
            set
            {
                ViewState["SelectedPackageID"] = value;
            }
        }


        public String InstitutionHierarchyNodeID
        {
            get
            {
                if (!ViewState["InstitutionHierarchyNodeID"].IsNullOrEmpty())
                    return Convert.ToString(ViewState["InstitutionHierarchyNodeID"]);
                else
                    return String.Empty;
            }
            set
            {
                ViewState["InstitutionHierarchyNodeID"] = value;
            }
        }
        public String SelectedPackageNodeMappingID
        {
            get
            {
                if (!ViewState["SelectedPackageNodeMappingID"].IsNullOrEmpty())
                    return Convert.ToString(ViewState["SelectedPackageNodeMappingID"]);
                else
                    return String.Empty;
            }
            set
            {
                ViewState["SelectedPackageNodeMappingID"] = value;
            }
        }
        public String SelectedInstitutionHierarchyNodeID
        {
            get
            {
                if (!ViewState["SelectedInstitutionHierarchyNodeID"].IsNullOrEmpty())
                    return ViewState["SelectedInstitutionHierarchyNodeID"].ToString();
                else
                    return String.Empty;
            }
            set
            {
                ViewState["SelectedInstitutionHierarchyNodeID"] = value;
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
        public List<GetDepartmentTree> lstCurrentTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeCurrentData") != null)
                    return (List<GetDepartmentTree>)(SysXWebSiteUtils.SessionService.GetCustomData("ComplianceTreeCurrentData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("ComplianceTreeCurrentData", value);
            }
        }

        #endregion
        #endregion

        #region Events

        #region Page
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsCompliancePackage)
            {
                fsucInstituteHierarchyPackageList.SaveButton.Visible = false;
            }
            else
            {
                fsucInstituteHierarchyPackageList.SaveButton.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Request.QueryString["TenantId"].IsNullOrEmpty())
                {
                    SelectedTenant = Convert.ToInt32(Request.QueryString["TenantId"]);
                }
                else
                {
                    SelectedTenant = AppConsts.NONE;
                }
                if (!Request.QueryString["CompliancePackageTypeCode"].IsNullOrEmpty())
                {
                    CompliancePackageTypeCode = Convert.ToString(Request.QueryString["CompliancePackageTypeCode"]);
                }
                else
                {
                    CompliancePackageTypeCode = String.Empty;
                }

                if (!Request.QueryString["IsCompliancePackage"].IsNullOrEmpty())
                {
                    IsCompliancePackage = Convert.ToBoolean(Request.QueryString["IsCompliancePackage"]);
                }

                if (!this.IsPostBack)
                {
                    if (!Request.QueryString["PackageNodeMappingID"].IsNullOrEmpty())
                    {
                        SelectedPackageNodeMappingID = Convert.ToString(Request.QueryString["PackageNodeMappingID"]);
                    }
                    if (!Request.QueryString["PackageId"].IsNullOrEmpty())
                    {
                        SelectedPackageID = Convert.ToString(Request.QueryString["PackageId"]);
                    }
                    if (!Request.QueryString["InstitutionHierarchyNodeID"].IsNullOrEmpty())
                    {
                        SelectedInstitutionHierarchyNodeID = Convert.ToString(Request.QueryString["InstitutionHierarchyNodeID"]);
                    }                  

                    BindHierarchyTree();
                    if (IsCompliancePackage == false)
                    {
                        treeInstituteHierarchyPackage.CheckBoxes = true;
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

        #region TreeView
        protected void TreeInstituteHierarchyPackage_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            try
            {
                GetDepartmentTree item = (GetDepartmentTree)e.Node.DataItem;
                e.Node.Font.Bold = true;   
                if (!String.IsNullOrEmpty(item.UICode) && item.UICode.ToUpper() == "PKG")
                {
                    e.Node.Attributes["PackageNodeMappingID"] = Convert.ToString(item.DataID);
                    e.Node.Attributes["PackageName"] = Convert.ToString(item.Value);
                    e.Node.Attributes["PackageID"] = Convert.ToString(item.EntityID);
                    e.Node.Attributes["UICode"] = Convert.ToString(item.UICode);
                    e.Node.Attributes["IsCompliancePackage"] = Convert.ToString(IsCompliancePackage);
                    e.Node.Attributes["InstitutionHierarchyNodeID"] = Convert.ToString(item.MappingID);
                    e.Node.Checkable = true;
                    if (!String.IsNullOrEmpty(SelectedPackageID) && !String.IsNullOrEmpty(SelectedPackageNodeMappingID) && !String.IsNullOrEmpty(SelectedInstitutionHierarchyNodeID))
                    {
                        //ONBD-18798
                        List<Int32> lstSelectedPackageNodeMappingIDs = SelectedPackageNodeMappingID.Split(',').Select(Int32.Parse).ToList();
                        List<Int32> lstSelectedPackageIDs = SelectedPackageID.Split(',').Select(Int32.Parse).ToList();
                        List<Int32> lstSelectedInstitutionHierarchyNodeIDs = SelectedInstitutionHierarchyNodeID.Split(',').Select(Int32.Parse).ToList();

                        //if (SelectedPackageNodeMappingID.Contains(item.DataID.ToString()) && SelectedPackageID.Contains(item.EntityID.ToString()) && SelectedInstitutionHierarchyNodeID.Contains(item.MappingID.ToString()))
                        if (lstSelectedPackageNodeMappingIDs.Contains(item.DataID) && lstSelectedPackageIDs.Contains(item.EntityID) && lstSelectedInstitutionHierarchyNodeIDs.Contains(item.MappingID))
                        {
                            if (IsCompliancePackage)
                            {
                                e.Node.Selected = true;
                            }
                            else
                            {
                                e.Node.Checked = true;                               
                            }
                        }
                    }
                    e.Node.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    e.Node.Checkable = false;
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
        protected void TreeInstituteHierarchyPackage_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            try
            {
                PackageNodeMappingID = Convert.ToString(e.Node.Attributes["PackageNodeMappingID"]);
                PackageName = Convert.ToString(e.Node.Attributes["PackageName"]);
                PackageID = Convert.ToString(e.Node.Attributes["PackageID"]);

                InstitutionHierarchyNodeID = Convert.ToString(e.Node.Attributes["InstitutionHierarchyNodeID"]);
                hdnInstitutionHierarchyNodeID.Value = InstitutionHierarchyNodeID;
                hdnPackageNodeMappingID.Value = PackageNodeMappingID;
                hdnPackageName.Value = PackageName;
                hdnPackageID.Value = PackageID.ToString();
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

        #region Private
        private void BindTree()
        {

            treeInstituteHierarchyPackage.DataSource = lstCurrentTreeData;
            treeInstituteHierarchyPackage.DataFieldID = "NodeID";
            treeInstituteHierarchyPackage.DataFieldParentID = "ParentNodeID";
            treeInstituteHierarchyPackage.DataTextField = "Value";
            treeInstituteHierarchyPackage.DataValueField = "NodeID";
            treeInstituteHierarchyPackage.DataBind();

        }
        private void BindHierarchyTree()
        {
            Presenter.GetTreeData();
            lstCurrentTreeData = lstTreeData;
            BindTree();
        }

    
        private void ReloadTreeData()
        {
    
        }
        #endregion

        #region Protected
        protected void btnOk_Click(Object sender, EventArgs e)
        {
            try
            {
                GetSelectedTreeNodes();
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

        private void GetSelectedTreeNodes()
        {
            List<String> PackageNodeMappingIDs = new List<String>();
            List<String> PackageNames = new List<String>();
            List<String> PackageIds = new List<String>();
            List<String> InstitutionHierarchyNodeIDs = new List<String>();

            String jsonObj = String.Empty;
            if (treeInstituteHierarchyPackage.CheckedNodes.Count > 0)
            {
                foreach (RadTreeNode node in treeInstituteHierarchyPackage.CheckedNodes)
                {
                    PackageNodeMappingIDs.Add(node.Attributes["PackageNodeMappingID"]);
                    PackageNames.Add(node.Attributes["PackageName"]);
                    PackageIds.Add(node.Attributes["PackageID"]);
                    InstitutionHierarchyNodeIDs.Add(node.Attributes["InstitutionHierarchyNodeID"]);
                }

                if (PackageNodeMappingIDs.IsNotNull() && PackageNodeMappingIDs.Count > 0)
                {
                    PackageNodeMappingID = String.Join(",", PackageNodeMappingIDs.ToArray());
                    hdnPackageNodeMappingID.Value = PackageNodeMappingID;
                }

                if (PackageNames.IsNotNull() && PackageNames.Count > 0)
                {
                    PackageName = String.Join(",", PackageNames.ToArray().Distinct());
                    hdnPackageName.Value = PackageName;
                }
                if (PackageIds.IsNotNull() && PackageIds.Count > 0)
                {
                    PackageID = String.Join(",", PackageIds.ToArray().Distinct());
                    hdnPackageID.Value = PackageID.ToString();
                }

                if (InstitutionHierarchyNodeIDs.IsNotNull() && InstitutionHierarchyNodeIDs.Count > 0)
                {
                    InstitutionHierarchyNodeID = String.Join(",", InstitutionHierarchyNodeIDs.ToArray().Distinct());
                    hdnInstitutionHierarchyNodeID.Value = InstitutionHierarchyNodeID.ToString();
                }
            }
            else
            {
                InstitutionHierarchyNodeID = hdnInstitutionHierarchyNodeID.Value = String.Empty;
                PackageID = hdnPackageID.Value = String.Empty;
                PackageName = hdnPackageName.Value = String.Empty;
                PackageNodeMappingID = hdnPackageName.Value = String.Empty;
            }

            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
        }
    }
    #endregion

    #endregion
}
