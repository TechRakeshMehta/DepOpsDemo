using INTSOF.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using INTSOF.ServiceInterface.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System.ServiceModel;
using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;

namespace INTSOF.Service.Modules.AgencyHierarchy
{
    public class AgencyHierarchy : BaseService, IAgencyHierarchy
    {
        ServiceResponse<Int32> IAgencyHierarchy.SaveAgencyHierarchyMapping(ServiceRequest<AgencyNodeMappingContract, Int32> data)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.SaveAgencyHierarchyMapping(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region Common Control
        ServiceResponse<List<AgencyHierarchyContract>> IAgencyHierarchy.GetAgencyHierarchy(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<List<AgencyHierarchyContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyContract>>();
            try
            {
                List<AgencyHierarchyContract> agencyHierarchyTreeData = new List<AgencyHierarchyContract>();
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                agencyHierarchyTreeData = AgencyHierarchyManager.GetAgencyHierarchy(data.Parameter1, data.Parameter2);
                commonResponse.Result = agencyHierarchyTreeData;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<AgencyHierarchyContract>> IAgencyHierarchy.GetTreeDataByRootNodeID(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<List<AgencyHierarchyContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyContract>>();
            try
            {
                List<AgencyHierarchyContract> agencyChildHierarchyTreeData = new List<AgencyHierarchyContract>();
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                agencyChildHierarchyTreeData = AgencyHierarchyManager.GetTreeDataByRootNodeID(data.Parameter1, data.Parameter2);
                commonResponse.Result = agencyChildHierarchyTreeData;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<AgencyHierarchyContract> IAgencyHierarchy.GetAgencyDetailByNodeId(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<AgencyHierarchyContract> commonResponse = new ServiceResponse<AgencyHierarchyContract>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                AgencyHierarchyContract agencyContract = AgencyHierarchyManager.GetAgencyDetailByNodeId(data.Parameter1, data.Parameter2);
                commonResponse.Result = agencyContract;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<AgencyHierarchyContract>> IAgencyHierarchy.GetAgencyHierarchyByRootNodeIds(ServiceRequest<Int32, String, String> data)
        {
            ServiceResponse<List<AgencyHierarchyContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyContract>>();
            try
            {
                List<AgencyHierarchyContract> agencyHierarchyTreeData = new List<AgencyHierarchyContract>();
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                agencyHierarchyTreeData = AgencyHierarchyManager.GetAgencyHierarchyByRootNodeIds(data.Parameter1, data.Parameter2, data.Parameter3);
                commonResponse.Result = agencyHierarchyTreeData;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<String> IAgencyHierarchy.GetAgencyDetailByMultipleNodeID(ServiceRequest<Int32, AgencyHierarchMultiSelectParameter> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {

                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                String agencyHierarchyTreeData = AgencyHierarchyManager.GetAgencyDetailByMultipleNodeID(data.Parameter1, data.Parameter2);
                commonResponse.Result = agencyHierarchyTreeData;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<String>> IAgencyHierarchy.GetAgencyHierarchyAgencyByMultipleNodeIds(ServiceRequest<Int32, AgencyHierarchMultiSelectParameter> data) //UAT-2926
        {
            ServiceResponse<List<String>> commonResponse = new ServiceResponse<List<String>>();
            try
            {

                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                List<String> agencyHierarchyTreeData = AgencyHierarchyManager.GetAgencyHierarchyAgencyByMultipleNodeIds(data.Parameter1, data.Parameter2);
                commonResponse.Result = agencyHierarchyTreeData;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<AgencyHierarchyContract>> IAgencyHierarchy.GetAgencyDetailByMultipleNodeIds(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<List<AgencyHierarchyContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyContract>>();
            try
            {
                List<AgencyHierarchyContract> agencyHierarchyTreeData = new List<AgencyHierarchyContract>();
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                agencyHierarchyTreeData = AgencyHierarchyManager.GetAgencyDetailByMultipleNodeIds(data.Parameter1, data.Parameter2);
                commonResponse.Result = agencyHierarchyTreeData;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }


        ServiceResponse<List<AgencyHierarchyContract>> IAgencyHierarchy.GetTreeDataByRootNodeIDForPopUp(ServiceRequest<Int32, AgencyHierarchPopUpParameter> data)
        {
            ServiceResponse<List<AgencyHierarchyContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyContract>>();
            try
            {
                List<AgencyHierarchyContract> agencyHierarchyTreeData = new List<AgencyHierarchyContract>();
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                agencyHierarchyTreeData = AgencyHierarchyManager.GetTreeDataByRootNodeIDForPopUp(data.Parameter1, data.Parameter2);
                commonResponse.Result = agencyHierarchyTreeData;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<String> IAgencyHierarchy.GetAgencyHierarchyLabel(ServiceRequest<Int32, Int32, Int32> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {

                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                String hierarchyLabel = AgencyHierarchyManager.GetAgencyHierarchyLabel(data.Parameter1, data.Parameter2, data.Parameter3);
                commonResponse.Result = hierarchyLabel;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<String> IAgencyHierarchy.GetAgencyHierarchyParent(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {

                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                String hierarchyLabel = AgencyHierarchyManager.GetAgencyHierarchyParent(data.Parameter1, data.Parameter2);
                commonResponse.Result = hierarchyLabel;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<String> IAgencyHierarchy.GetAgencyHierarchyLabelForMultipleSelection(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {

                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                String hierarchyLabel = AgencyHierarchyManager.GetAgencyHierarchyLabelForMultipleSelection(data.Parameter1, data.Parameter2);
                commonResponse.Result = hierarchyLabel;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-2630:Agency hierarchy mapping: Agency Hierarchies grid
        ServiceResponse<List<AgencyHierarchyDataContract>> IAgencyHierarchy.GetRootAgencyHierarchyData(ServiceRequest<CustomPagingArgsContract> data)
        {
            ServiceResponse<List<AgencyHierarchyDataContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyDataContract>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetRootAgencyHierarchyData(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.DeleteRootAgencyHierarchy(ServiceRequest<Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.DeleteRootAgencyHierarchy(data.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region AgencyNodeMapping
        ServiceResponse<Boolean> IAgencyHierarchy.CheckForLeafNode(ServiceRequest<Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.CheckForLeafNode(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IAgencyHierarchy.DeleteAgencyNodeMapping(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                AgencyHierarchyManager.DeleteAgencyNodeMapping(data.Parameter1, data.Parameter2);
                commonResponse.Result = true;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<AgencyNodeMappingContract> IAgencyHierarchy.GetAgencyHierarchyAgencyMapping(ServiceRequest<Int32> data)
        {
            ServiceResponse<AgencyNodeMappingContract> commonResponse = new ServiceResponse<AgencyNodeMappingContract>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchyAgencyMapping(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        #endregion




        #region UAT-2634:- Agency Hierarchy Package Mapping

        ServiceResponse<List<RequirementPackageContract>> IAgencyHierarchy.GetRequirementPackages()
        {
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetRequirementPackages();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<RequirementPackageContract>> IAgencyHierarchy.GetAgencyHierarchyPackages(ServiceRequest<CustomPagingArgsContract, Int32> data)
        {
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchyPackages(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IAgencyHierarchy.SaveAgencyHierarchyPackageMapping(ServiceRequest<AgencyHierarchyPackageContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.SaveAgencyHierarchyPackageMapping(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.DeleteAgencyHierarchyPackageMapping(ServiceRequest<AgencyHierarchyPackageContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.DeleteAgencyHierarchyPackageMapping(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2632:UAT-2632:Agency hierarchy mapping: Map Node
        ServiceResponse<List<AgencyHierarchyDataContract>> IAgencyHierarchy.GetMappedNodesByNodeID(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<AgencyHierarchyDataContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyDataContract>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetMappedNodesByNodeID(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.DeleteNodeMapping(ServiceRequest<Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.DeleteNodeMapping(data.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<AgencyNodeContract>> IAgencyHierarchy.GetAgencyNodeListForMapping()
        {
            ServiceResponse<List<AgencyNodeContract>> commonResponse = new ServiceResponse<List<AgencyNodeContract>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetAgencyNodeList();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Tuple<Boolean, Int32>> IAgencyHierarchy.SaveNodeMapping(ServiceRequest<Int32, Int32, String> data)
        {
            ServiceResponse<Tuple<Boolean, Int32>> commonResponse = new ServiceResponse<Tuple<Boolean, Int32>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.SaveNodeMapping(data.Parameter1, data.Parameter2, data.Parameter3, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.IsAgencyMappedWithNode(ServiceRequest<Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.IsAgencyMappedOnNode(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Int32> IAgencyHierarchy.GetAgencyNodeIDByAgencyHierarchyID(ServiceRequest<Int32> data)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetAgencyNodeIDByAgencyHierarchyID(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion


        #region UAT-2629

        ServiceResponse<Boolean> IAgencyHierarchy.SaveNodeDetail(ServiceRequest<AgencyNodeContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                AgencyHierarchyManager.SaveNodeDetail(data.Parameter);
                commonResponse.Result = true;
                return commonResponse;
            }

            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }


        ServiceResponse<List<AgencyNodeContract>> IAgencyHierarchy.GetAgencyNodeList()
        {
            ServiceResponse<List<AgencyNodeContract>> commonResponse = new ServiceResponse<List<AgencyNodeContract>>();
            try
            {
                List<AgencyNodeContract> _lst = AgencyHierarchyManager.GetAgencyNodeList();
                commonResponse.Result = _lst;
                return commonResponse;

            }

            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        //UAT-3652
        ServiceResponse<List<AgencyNodeContract>> IAgencyHierarchy.GetAgencyNodeRootList(ServiceRequest<CustomPagingArgsContract, String, String> data)
        {
            ServiceResponse<List<AgencyNodeContract>> commonResponse = new ServiceResponse<List<AgencyNodeContract>>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencyNodeRootList(data.Parameter1,data.Parameter2,data.Parameter3);
                return commonResponse;
            }

            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        Boolean IAgencyHierarchy.IsNodeExist(String nodeName, Int32? nodeId = null)
        {
            try
            {
                return AgencyHierarchyManager.IsNodeExist(nodeName, nodeId);
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }


        #endregion

        #region UAT-2636 : Agency hierarchy mapping: Map users with hierarchy node
        ServiceResponse<List<AgencyHierarchyUserContract>> IAgencyHierarchy.GetAgencyUsers(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<AgencyHierarchyUserContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyUserContract>>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencyUsers(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<AgencyHierarchyUserContract>> IAgencyHierarchy.GetAgencyHierarchyUsers(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<AgencyHierarchyUserContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyUserContract>>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchyUsers(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IAgencyHierarchy.SaveAgencyHierarchyUserMapping(ServiceRequest<AgencyHierarchyUserContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.SaveAgencyHierarchyUserMapping(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.DeleteAgencyHierarchyUserMapping(ServiceRequest<AgencyHierarchyUserContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.DeleteAgencyHierarchyUserMapping(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        #endregion

        ServiceResponse<List<SchoolNodeAssociationDataContract>> IAgencyHierarchy.GetSchoolNodeAssociationByAgencyHierarchyID(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<List<SchoolNodeAssociationDataContract>> commonResponse = new ServiceResponse<List<SchoolNodeAssociationDataContract>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetSchoolNodeAssociationByAgencyHierarchyID(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.IsSchoolNodeAssociationExists(ServiceRequest<Int32, Int32, Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.IsSchoolNodeAssociationExists(data.Parameter1, data.Parameter2, data.Parameter3, data.Parameter4);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.SaveUpdateSchoolNodeAssociation(ServiceRequest<Int32, SchoolNodeAssociationContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.SaveUpdateSchoolNodeAssociation(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.RemoveSchoolNodeAssociation(ServiceRequest<Int32, SchoolNodeAssociationContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.RemoveSchoolNodeAssociation(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region [Agency Hierarchy Profile Share Permission]
        ServiceResponse<List<AgencyHierarchyProfileSharePermissionDataContract>> IAgencyHierarchy.GetProfileSharePermissionByAgencyHierarchyID(ServiceRequest<Int32, Int32> data)
        {

            ServiceResponse<List<AgencyHierarchyProfileSharePermissionDataContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyProfileSharePermissionDataContract>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetProfileSharePermissionByAgencyHierarchyID(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IAgencyHierarchy.SaveUpdateProfileSharePermission(ServiceRequest<Int32, AgencyHierarchyProfileSharePermissionDataContract> data)
        {

            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.SaveUpdateProfileSharePermission(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.RemoveProfileSharePermission(ServiceRequest<Int32, AgencyHierarchyProfileSharePermissionDataContract> data)
        {

            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.RemoveProfileSharePermission(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-2641
        ServiceResponse<List<Int32>> IAgencyHierarchy.GetAgencyHierarchyIdsByOrgUserID(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchyIdsByOrgUserID(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2633
        ServiceResponse<List<AgencyNodeMappingContract>> IAgencyHierarchy.GetAgencies(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<AgencyNodeMappingContract>> commonResponse = new ServiceResponse<List<AgencyNodeMappingContract>>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencies(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<AgencyNodeMappingContract>> IAgencyHierarchy.GetAgencyHierarchyAgencies(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<AgencyNodeMappingContract>> commonResponse = new ServiceResponse<List<AgencyNodeMappingContract>>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchyAgencies(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.SaveAgencyHierarchyAgencyMapping(ServiceRequest<AgencyNodeMappingContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.SaveAgencyHierarchyAgencyMapping(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.DeleteAgencyHierarchyAgencyMapping(ServiceRequest<AgencyNodeMappingContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.DeleteAgencyHierarchyAgencyMapping(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.IsAgencyHierarchyLeafNode(ServiceRequest<Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.IsAgencyHierarchyLeafNode(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        #endregion

        #region [UAT-2653]

        ServiceResponse<List<Int32>> IAgencyHierarchy.GetAgencyHiearchyIdsByDeptProgMappingID(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHiearchyIdsByDeptProgMappingID(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-2647

        ServiceResponse<List<Int32>> IAgencyHierarchy.GetAgencyHierarchyIdsByTenantID(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchyIdsByTenantID(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }


        #endregion

        #region UAT-3245

        ServiceResponse<List<Int32>> IAgencyHierarchy.GetAgencyHierarchyIdsByLstTenantIDs(ServiceRequest<List<Int32>> data)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchyIdsByLstTenantIDs(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        ServiceResponse<String> IAgencyHierarchy.GetAgencyHiearchyIdsByTenantID(ServiceRequest<Int32> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHiearchyIdsByTenantID(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region UAT-2548
        ServiceResponse<List<TenantDetailContract>> IAgencyHierarchy.GetTenants(ServiceRequest<Boolean, String> data)
        {
            ServiceResponse<List<TenantDetailContract>> commonResponse = new ServiceResponse<List<TenantDetailContract>>();
            try
            {
                commonResponse.Result = ClinicalRotationManager.GetTenants(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IAgencyHierarchy.SaveUpdateAgencyHierarchyTenantAccessMapping(ServiceRequest<Int32, List<Int32>, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.SaveUpdateAgencyHierarchyTenantAccessMapping(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<Int32>> IAgencyHierarchy.GetAgencyHierarchyTenantAccessDetails(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchyTenantAccessDetails(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2712
        ServiceResponse<AgencyHierarchyRotationFieldOptionContract> IAgencyHierarchy.GetAgencyHierarchyRotationFieldOptionSetting(ServiceRequest<Int32> data)
        {
            ServiceResponse<AgencyHierarchyRotationFieldOptionContract> commonResponse = new ServiceResponse<AgencyHierarchyRotationFieldOptionContract>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchyRotationFieldOptionSetting(data.Parameter);
                return commonResponse;
            }


            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IAgencyHierarchy.SaveAgencyHierarchyRotationFieldOptionSetting(ServiceRequest<AgencyHierarchyRotationFieldOptionContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.SaveAgencyHierarchyRotationFieldOptionSetting(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        #endregion

        #region [UAT-2427: Job Board]

        ServiceResponse<List<AgencyJobContract>> IAgencyHierarchy.GetAgencyJobTemplate(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<AgencyJobContract>> commonResponse = new ServiceResponse<List<AgencyJobContract>>();
            try
            {
                List<AgencyJobContract> lstAgencyJobs = new List<AgencyJobContract>();
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                lstAgencyJobs = AgencyJobBoardManager.GetAgencyJobTemplate(serviceRequest.Parameter);
                commonResponse.Result = lstAgencyJobs;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<DefinedRequirementContract>> IAgencyHierarchy.GetJobFieldType()
        {
            ServiceResponse<List<DefinedRequirementContract>> commonResponse = new ServiceResponse<List<DefinedRequirementContract>>();
            try
            {
                List<DefinedRequirementContract> lstJobFieldType = new List<DefinedRequirementContract>();
                lstJobFieldType = AgencyJobBoardManager.GetJobFieldType();
                commonResponse.Result = lstJobFieldType;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }


        ServiceResponse<Boolean> IAgencyHierarchy.SaveAgencyJobTemplate(ServiceRequest<AgencyJobContract, Int32> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                Boolean _result = new Boolean(); ;
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                _result = AgencyJobBoardManager.SaveAgencyJobTemplate(serviceRequest.Parameter1, serviceRequest.Parameter2);
                commonResponse.Result = _result;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.DeleteAgencyJobTemplate(ServiceRequest<AgencyJobContract, Int32> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                Boolean _result = new Boolean(); ;
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                _result = AgencyJobBoardManager.DeleteAgencyJobTemplate(serviceRequest.Parameter1, serviceRequest.Parameter2);
                commonResponse.Result = _result;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Int32> IAgencyHierarchy.GetAgencyHierarchyId(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                Int32 _result;
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                _result = AgencyJobBoardManager.GetAgencyHierarchyId(serviceRequest.Parameter);
                commonResponse.Result = _result;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<AgencyJobContract>> IAgencyHierarchy.GetAgencyJobPosting(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<AgencyJobContract>> commonResponse = new ServiceResponse<List<AgencyJobContract>>();
            try
            {
                List<AgencyJobContract> lstAgencyJobs = new List<AgencyJobContract>();
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                lstAgencyJobs = AgencyJobBoardManager.GetAgencyJobPosting(serviceRequest.Parameter);
                commonResponse.Result = lstAgencyJobs;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.SaveAgencyJobPosting(ServiceRequest<AgencyJobContract, Int32> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                Boolean _result = new Boolean(); ;
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                _result = AgencyJobBoardManager.SaveAgencyJobPosting(serviceRequest.Parameter1, serviceRequest.Parameter2);
                commonResponse.Result = _result;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.DeleteAgencyJobPosting(ServiceRequest<AgencyJobContract, Int32> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                Boolean _result = new Boolean(); ;
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                _result = AgencyJobBoardManager.DeleteAgencyJobPosting(serviceRequest.Parameter1, serviceRequest.Parameter2);
                commonResponse.Result = _result;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<AgencyJobContract> IAgencyHierarchy.GetTemplateDetailsByID(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<AgencyJobContract> commonResponse = new ServiceResponse<AgencyJobContract>();
            try
            {
                AgencyJobContract _result = new AgencyJobContract(); ;
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                _result = AgencyJobBoardManager.GetTemplateDetailsByID(serviceRequest.Parameter);
                commonResponse.Result = _result;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<AgencyLogoContract> IAgencyHierarchy.GetAgencyLogo(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<AgencyLogoContract> commonResponse = new ServiceResponse<AgencyLogoContract>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyJobBoardManager.GetAgencyLogo(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.SaveUpdateAgencyLogo(ServiceRequest<AgencyLogoContract, Int32> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyJobBoardManager.SaveUpdateAgencyLogo(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.ArchiveJobPosts(ServiceRequest<List<Int32>, Int32> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyJobBoardManager.ArchiveJobPosts(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.ClearLogo(ServiceRequest<Int32, Int32> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyJobBoardManager.ClearLogo(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        #region Job Board
        ServiceResponse<List<AgencyJobContract>> IAgencyHierarchy.GetViewAgencyJobPosting(ServiceRequest<JobSearchContract, CustomPagingArgsContract> serviceRequest)
        {
            ServiceResponse<List<AgencyJobContract>> commonResponse = new ServiceResponse<List<AgencyJobContract>>();
            try
            {
                commonResponse.Result = AgencyJobBoardManager.GetViewAgencyJobPosting(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        #endregion

        ServiceResponse<AgencyJobContract> IAgencyHierarchy.GetSelectedJobPostDetails(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<AgencyJobContract> commonResponse = new ServiceResponse<AgencyJobContract>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyJobBoardManager.GetSelectedJobPostDetails(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-2784
        ServiceResponse<AgencyHierarchySettingContract> IAgencyHierarchy.GetAgencyHierarchySetting(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<AgencyHierarchySettingContract> commonResponse = new ServiceResponse<AgencyHierarchySettingContract>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchySetting(data.Parameter1, data.Parameter2);
                return commonResponse;
            }


            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IAgencyHierarchy.SaveAgencyHierarchySetting(ServiceRequest<AgencyHierarchySettingContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.SaveAgencyHierarchySetting(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        #endregion

        #region [UAT-2723]

        ServiceResponse<Boolean> IAgencyHierarchy.SaveClientSystemDocument(ServiceRequest<Int32, List<RequirementApprovalNotificationDocumentContract>> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyJobBoardManager.SaveClientSystemDocument(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IAgencyHierarchy.DeleteClientSystemDocumentBasedOnDocType(ServiceRequest<Int32, Int32?, Int32, String> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyJobBoardManager.DeleteClientSystemDocumentBasedOnDocType(serviceRequest.Parameter1, serviceRequest.Parameter2, serviceRequest.Parameter3, serviceRequest.Parameter4);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        ServiceResponse<RequirementApprovalNotificationDocumentContract> IAgencyHierarchy.GetClientSystemDocumentBasedOnDocumentType(ServiceRequest<Int32, String> serviceRequest)
        {
            ServiceResponse<RequirementApprovalNotificationDocumentContract> commonResponse = new ServiceResponse<RequirementApprovalNotificationDocumentContract>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = AgencyJobBoardManager.GetClientSystemDocumentBasedOnDocumentType(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT 2821

        ServiceResponse<Boolean> IAgencyHierarchy.DeleteAgencyHierarchySetting(ServiceRequest<Int32, Int32, String> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.DeleteAgencyHierarchySetting(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-3237
        ServiceResponse<Boolean> IAgencyHierarchy.UpdateNodeDisplayOrder(ServiceRequest<List<AgencyHierarchyDataContract>, Int32?, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.UpdateNodeDisplayOrder(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        #endregion

        ServiceResponse<Boolean> IAgencyHierarchy.SaveAgencyHierarchyRootNodeSetting(ServiceRequest<AgencyHierarchyRootNodeSettingContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.SaveAgencyHierarchyRootNodeSetting(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<AgencyHierarchyRootNodeSettingContract>> IAgencyHierarchy.GetAgencyHierarchyRootNodeMapping(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<List<AgencyHierarchyRootNodeSettingContract>> commonResponse = new ServiceResponse<List<AgencyHierarchyRootNodeSettingContract>>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.GetAgencyHierarchyRootNodeMapping(data.Parameter1,data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IAgencyHierarchy.SaveUpdateAgencyHierarchyRootNodeMapping(ServiceRequest<AgencyHierarchyRootNodeSettingContract> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.SaveUpdateAgencyHierarchyRootNodeMapping(data.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IAgencyHierarchy.IsAgencyHierarchyRootNodeSettingExist(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = AgencyHierarchyManager.IsAgencyHierarchyRootNodeSettingExist(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogAgencyHierarchySvcError(ex);
                throw;
            }
        }
  
    }
}