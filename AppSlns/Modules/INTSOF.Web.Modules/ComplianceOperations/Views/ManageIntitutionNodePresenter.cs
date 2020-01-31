#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

#endregion

#region UserDefined

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageIntitutionNodePresenter : Presenter<IManageIntitutionNodeView>
    {

        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties
        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {
            View.ListTenants = ComplianceDataManager.getClientTenant();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Method to assign the Node List of selected Tenant in GetNodeList and LastCode properties.
        /// </summary>
        public void GetNodeList()
        {
            View.GetNodeList = ComplianceDataManager.GetInstitutionNodeList(View.SelectedTenantID);
            View.LastCode = ComplianceDataManager.GetLastCodeFromInstitutionNode(View.SelectedTenantID);
        }

        /// <summary>
        /// Method to assign the List of Custom Attribute and List of Institution Node Type
        /// </summary>
        public void GetNodeTypeList()
        {
            //Int32 nodetypeIdInstitute = (Int32)NodeType.Institution;
            List<InstitutionNodeType> lstInstitutionNodeType = ComplianceDataManager.GetInstitutionNodeTypeList(View.SelectedTenantID).ToList();
            lstInstitutionNodeType.Insert(0, new InstitutionNodeType { INT_ID = 0, INT_Name = "--SELECT--" });
            View.GetNodeTypeList = lstInstitutionNodeType;
            String useTypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
            View.GetCustomAttributeListTypeHierarchy = ComplianceDataManager.GetCustomAttributeListByType(View.SelectedTenantID, useTypeCode);
        }

        /// <summary>
        /// Method to get the list of mapping of Custom Attribute with Node.
        /// </summary>
        public void GetCustomAttributeMappedList()
        {
            View.GetCustomAttributeMappedList = ComplianceDataManager.GetNodeMappedCustomAttributeList(View.SelectedTenantID, View.NodeId).ToList();
            List<Int32> customAttribuiteMappingIds = View.GetCustomAttributeMappedList.Select(x => x.CAM_CustomAttributeMappingID).ToList();
            View.MappedIdsWithCustomAttributeValue = ComplianceDataManager.GetListOfIdMappedWithCustomAttrValue(View.SelectedTenantID, customAttribuiteMappingIds);
        } 

        /// <summary>
        /// Method to check the duplicate record of same name.
        /// </summary>
        /// <param name="nodeName">nodeName</param>
        /// <param name="nodeId">nodeId</param>
        /// <returns>Boolean</returns>
        public Boolean IsNodeExist(String nodeName, Int32? nodeId = null)
        {
            List<InstitutionNode> listNode = ComplianceDataManager.GetInstitutionNodeList(View.SelectedTenantID).ToList();
            if (nodeId != null)
            {
                if (listNode.Any(x => x.IN_Name.ToLower() == nodeName.ToLower() && x.IN_ID != nodeId))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (listNode.Any(x => x.IN_Name.ToLower() == nodeName.ToLower() && !x.IN_IsDeleted))
                {
                    return true;
                }
                return false;
            }
            //return true;
        }

        /// <summary>
        /// Method to save the Node Detail.
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean SaveNodeDetail()
        {
            if (!IsNodeExist(View.Name, null))
            {
                InstitutionNode institutionNode = new InstitutionNode();
                institutionNode.IN_Name = View.Name;
                institutionNode.IN_Label = View.Label;
                institutionNode.IN_Description = View.Description;
                institutionNode.IN_NodeTypeID = View.NodeTypeId;
                institutionNode.IN_Code = View.GeneratedCode;
                institutionNode.IN_Duration = View.Duration;
                institutionNode.IN_IsDeleted = false;
                institutionNode.IN_CreatedByID = View.CurrentUserId;
                institutionNode.IN_CreatedOn = DateTime.Now;
                if (ComplianceDataManager.SaveNodeDetail(View.SelectedTenantID, institutionNode,View.ListToAddCustomAttributeMapping))
                {
                    View.SuccessMessage = "Institution node saved successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred.Please try again.";
                    return false;
                }
            }
            else
            {
                View.InfoMessage = "Institution node already exist.";
                return false;
            }
        }

        /// <summary>
        /// Method to update the node detail.
        /// </summary>
        /// <returns></returns>
        public Boolean UpdateNodeDetail()
        {
            if (!IsNodeExist(View.Name, View.NodeId))
            {
                InstitutionNode institutionNode = ComplianceDataManager.GetNodeByNodeId(View.SelectedTenantID, View.NodeId);
                institutionNode.IN_Name = View.Name;
                institutionNode.IN_Label = View.Label;
                institutionNode.IN_Description = View.Description;
                institutionNode.IN_NodeTypeID = View.NodeTypeId;
                institutionNode.IN_Duration = View.Duration;
                institutionNode.IN_IsDeleted = false;
                institutionNode.IN_ModifiedByID = View.CurrentUserId;
                institutionNode.IN_ModifiedOn = DateTime.Now;
                if (ComplianceDataManager.UpdateNodeDetail(View.SelectedTenantID,View.NodeId,View.ListToAddCustomAttributeMapping,View.CurrentUserId))
                {
                    View.SuccessMessage = "Institution node updated successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred.Please try again.";
                    return false;
                }
            }
            else
            {
                View.InfoMessage = "Institution node already exist.";
                return false;
            }
            //return true;
        }

        /// <summary>
        /// Method to delete the node.
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteNode()
        {
            if (!ComplianceDataManager.IsNodeMapped(View.SelectedTenantID, View.NodeId))
            {
                InstitutionNode institutionNode = ComplianceDataManager.GetNodeByNodeId(View.SelectedTenantID, View.NodeId);
                institutionNode.IN_IsDeleted = true;
                institutionNode.IN_ModifiedByID = View.CurrentUserId;
                institutionNode.IN_ModifiedOn = DateTime.Now;
                //institutionNode.IN_Name = institutionNode.IN_Name + Guid.NewGuid();
                if (ComplianceDataManager.UpdateChanges(View.SelectedTenantID))
                {
                    View.SuccessMessage = "Institution node deleted successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred.Please try again.";
                    return false;
                }
            }
            else
            {
                View.InfoMessage = "This institution node is in used.";
                return false;
            }
            //return true;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            Int32 currentUserTenantId = GetTenantId();
            //Checked if logged user is admin or not.
            if (currentUserTenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                View.SelectedTenantID = currentUserTenantId;
                return false;
            }
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}




