using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Data.Entity.Core.Objects;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class CustomAttributeLoaderSearchMultipleNodesPresenter : Presenter<ICustomAttributeLoaderSearchMultipleNodesView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceAdministrationController _controller;
        // public CustomAttributeLoaderSearchPresenter([CreateNew] IComplianceAdministrationController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Get the Custom Attributes for multiple Nodes selected - UAT 1055
        /// </summary>
        public void GetCustomAttributes(String mappingRecordId, String useTypeCode, Int32 tenantId, String screenName)
        {
            if (mappingRecordId.IsNullOrEmpty() && !tenantId.IsNullOrEmpty())
            {
                ObjectResult<GetDepartmentTree> objInstituteHierarchyTree = null;

                List<InstituteHierarchyNodesList> objInstituteHierarchyNodeTree = null; //UAT-3369


                ObjectResult<GetInstituteHierarchyOrderTree> lstOrderTreeData = null;
                if (screenName.IsNullOrEmpty())
                {
                    objInstituteHierarchyNodeTree = ComplianceSetupManager.GetInstituteHierarchyNodes(tenantId, IsDefaultTenant() ? (int?)null : View.CurrentLoggedInUserId).ToList();
                }
                else if (screenName == "CommonScreen")
                {
                    objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyTreeCommon(tenantId, IsDefaultTenant() ? (int?)null : View.CurrentLoggedInUserId, string.Empty);

                }
                else if (screenName == "OrderQueue")
                {
                    lstOrderTreeData = ComplianceSetupManager.GetInstituteHierarchyOrderTree(tenantId, IsDefaultTenant() ? (int?)null : View.CurrentLoggedInUserId);

                }
                else if (screenName == "BackgroundScreen")
                {
                    objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyTreeForBackgroundHierarchyPermissionType(tenantId, IsDefaultTenant() ? (int?)null : View.CurrentLoggedInUserId);

                }
                if (screenName == "OrderQueue")
                {
                    var data = lstOrderTreeData.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList();
                    var lstIds = data.Select(cond => cond.DataID);
                    mappingRecordId = String.Join(",", lstIds);
                }
                else
                {
                    if (objInstituteHierarchyNodeTree.IsNullOrEmpty()) //UAT-3369
                    {
                        var data = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList();
                        var lstIds = data.Select(cond => cond.DataID);
                        mappingRecordId = String.Join(",", lstIds);
                    }
                    else
                    {
                        var data1 = objInstituteHierarchyNodeTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList();
                        var lstIds = data1.Select(cond => cond.DataID);
                        mappingRecordId = String.Join(",", lstIds);
                    }

                }

            }
            //added this check to resolve issue: All custom attributes are displaying if Client admin does not have permission on any node.
            if (!mappingRecordId.IsNullOrEmpty())
            {
                if (useTypeCode == CustomAttributeUseTypeContext.Hierarchy.GetStringValue())
                {
                    View.lstTypeCustomAttributes = ComplianceDataManager.GetCustomAttributesNodeSearch(mappingRecordId, useTypeCode, tenantId).Where(x => x.DisplayInSearchFilter.HasValue && x.DisplayInSearchFilter.Value == true).ToList();
                }
                else
                {
                    View.lstTypeCustomAttributes = ComplianceDataManager.GetCustomAttributesNodeSearch(mappingRecordId, useTypeCode, tenantId);
                }
            }
            else
            {
                View.lstTypeCustomAttributes = new List<TypeCustomAttributesSearch>();
            }
        }

        /// <summary>
        /// Is Default Tenant.
        /// </summary>
        /// <returns></returns>
        public Boolean IsDefaultTenant()
        {
            Int32 tenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
            if (SecurityManager.DefaultTenantID == tenantId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}




