using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceInterface.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Core;
using INTSOF.Utils.Enums;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;

namespace INTSOF.ServiceProxy.Modules.AgencyHierarchy
{
    public class AgencyHierarchyProxy : BaseServiceProxy<IAgencyHierarchy>
    {
        IAgencyHierarchy _agencyHierarchyServiceChannel;

        public AgencyHierarchyProxy()
            : base(ServiceUrlEnum.AgencyHierarchySvcUrl.GetStringValue())
        {
            _agencyHierarchyServiceChannel = base.ServiceChannel;
        }

        #region AgencyNodeMapping
        public ServiceResponse<Int32> SaveAgencyHierarchyMapping(ServiceRequest<AgencyNodeMappingContract, Int32> data)
        {
            return _agencyHierarchyServiceChannel.SaveAgencyHierarchyMapping(data);
        }
        #endregion

        #region Common Control
        public ServiceResponse<List<AgencyHierarchyContract>> GetAgencyHierarchy(ServiceRequest<Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchy(data);
        }

        public ServiceResponse<List<AgencyHierarchyContract>> GetTreeDataByRootNodeID(ServiceRequest<Int32, Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetTreeDataByRootNodeID(data);
        }
        public ServiceResponse<AgencyHierarchyContract> GetAgencyDetailByNodeId(ServiceRequest<Int32, Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyDetailByNodeId(data);
        }
        public ServiceResponse<Boolean> CheckForLeafNode(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.CheckForLeafNode(data);
        }
        public ServiceResponse<Boolean> DeleteAgencyNodeMapping(ServiceRequest<Int32, Int32> data)
        {
            return _agencyHierarchyServiceChannel.DeleteAgencyNodeMapping(data);
        }
        public ServiceResponse<AgencyNodeMappingContract> GetAgencyHierarchyAgencyMapping(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyAgencyMapping(data);
        }
        public ServiceResponse<List<AgencyHierarchyContract>> GetAgencyHierarchyByRootNodeIds(ServiceRequest<Int32, String, String> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyByRootNodeIds(data);
        }
        public ServiceResponse<List<AgencyHierarchyContract>> GetAgencyDetailByMultipleNodeIds(ServiceRequest<Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyDetailByMultipleNodeIds(data);
        }
        #endregion

        public ServiceResponse<List<AgencyHierarchyContract>> GetTreeDataByRootNodeIDForPopUp(ServiceRequest<Int32, AgencyHierarchPopUpParameter> data)
        {
            return _agencyHierarchyServiceChannel.GetTreeDataByRootNodeIDForPopUp(data);
        }
        public ServiceResponse<String> GetAgencyDetailByMultipleNodeID(ServiceRequest<Int32, AgencyHierarchMultiSelectParameter> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyDetailByMultipleNodeID(data);
        }

        public ServiceResponse<List<String>> GetAgencyHierarchyAgencyByMultipleNodeIds(ServiceRequest<Int32, AgencyHierarchMultiSelectParameter> data)//UAT-2926
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyAgencyByMultipleNodeIds(data);
        }
        public ServiceResponse<String> GetAgencyHierarchyLabel(ServiceRequest<Int32, Int32, Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyLabel(data);
        }

        public ServiceResponse<String> GetAgencyHierarchyParent(ServiceRequest<Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyParent(data);
        }
        public ServiceResponse<String> GetAgencyHierarchyLabelForMultipleSelection(ServiceRequest<Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyLabelForMultipleSelection(data);
        }

        #region UAT-2630:Agency hierarchy mapping: Agency Hierarchies grid
        public ServiceResponse<List<AgencyHierarchyDataContract>> GetRootAgencyHierarchyData(ServiceRequest<CustomPagingArgsContract> data)
        {
            return _agencyHierarchyServiceChannel.GetRootAgencyHierarchyData(data);
        }

        public ServiceResponse<Boolean> DeleteRootAgencyHierarchy(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.DeleteRootAgencyHierarchy(data);
        }
        #endregion

        #region UAT-2634 : Agency hierarchy mapping: Map Package with hierarchy Node
        public ServiceResponse<List<RequirementPackageContract>> GetRequirementPackages()
        {
            return _agencyHierarchyServiceChannel.GetRequirementPackages();
        }
        public ServiceResponse<List<RequirementPackageContract>> GetAgencyHierarchyPackages(ServiceRequest<CustomPagingArgsContract, Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyPackages(data);
        }
        public ServiceResponse<Boolean> SaveAgencyHierarchyPackageMapping(ServiceRequest<AgencyHierarchyPackageContract> data)
        {
            return _agencyHierarchyServiceChannel.SaveAgencyHierarchyPackageMapping(data);
        }
        public ServiceResponse<Boolean> DeleteAgencyHierarchyPackageMapping(ServiceRequest<AgencyHierarchyPackageContract> data)
        {
            return _agencyHierarchyServiceChannel.DeleteAgencyHierarchyPackageMapping(data);
        }
        #endregion

        #region UAT-2632:Agency hierarchy mapping: Map Node
        public ServiceResponse<List<AgencyHierarchyDataContract>> GetMappedNodesByNodeID(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetMappedNodesByNodeID(data);
        }

        public ServiceResponse<Boolean> DeleteNodeMapping(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.DeleteNodeMapping(data);
        }

        public ServiceResponse<List<AgencyNodeContract>> GetAgencyNodeListForMapping()
        {
            return _agencyHierarchyServiceChannel.GetAgencyNodeListForMapping();
        }

        public ServiceResponse<Tuple<Boolean, Int32>> SaveNodeMapping(ServiceRequest<Int32, Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.SaveNodeMapping(data);
        }

        public ServiceResponse<Boolean> IsAgencyMappedWithNode(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.IsAgencyMappedWithNode(data);
        }

        public ServiceResponse<Int32> GetAgencyNodeIDByAgencyHierarchyID(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyNodeIDByAgencyHierarchyID(data);
        }
        #endregion

        #region UAT-2629 :

        public ServiceResponse<Boolean> SaveNodeDetail(ServiceRequest<AgencyNodeContract> data)
        {
            return _agencyHierarchyServiceChannel.SaveNodeDetail(data);
        }

        public ServiceResponse<List<AgencyNodeContract>> GetAgencyNodeList()
        {
            return _agencyHierarchyServiceChannel.GetAgencyNodeList();
        }

        //UAT-3652
        public ServiceResponse<List<AgencyNodeContract>> GetAgencyNodeRootList(ServiceRequest<CustomPagingArgsContract, String, String> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyNodeRootList(data); //UAT-3652
        }
        public Boolean IsNodeExist(String nodeName, Int32? nodeId = null)
        {
            return _agencyHierarchyServiceChannel.IsNodeExist(nodeName, nodeId);
        }

        #endregion

        #region UAT-2636: Agency hierarchy mapping: Map users with hierarchy node
        public ServiceResponse<List<AgencyHierarchyUserContract>> GetAgencyUsers(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyUsers(data);
        }
        public ServiceResponse<List<AgencyHierarchyUserContract>> GetAgencyHierarchyUsers(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyUsers(data);
        }
        public ServiceResponse<Boolean> SaveUpdateAgencyHierarchyUserMapping(ServiceRequest<AgencyHierarchyUserContract> data)
        {
            return _agencyHierarchyServiceChannel.SaveAgencyHierarchyUserMapping(data);
        }
        public ServiceResponse<Boolean> DeleteAgencyHierarchyUserMapping(ServiceRequest<AgencyHierarchyUserContract> data)
        {
            return _agencyHierarchyServiceChannel.DeleteAgencyHierarchyUserMapping(data);
        }
        #endregion


        #region [UAT-2635]

        public ServiceResponse<List<SchoolNodeAssociationDataContract>> GetSchoolNodeAssociationByAgencyHierarchyID(ServiceRequest<Int32, Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetSchoolNodeAssociationByAgencyHierarchyID(data);
        }

        public ServiceResponse<Boolean> SaveUpdateSchoolNodeAssociation(ServiceRequest<Int32, SchoolNodeAssociationContract> data)
        {
            return _agencyHierarchyServiceChannel.SaveUpdateSchoolNodeAssociation(data);
        }

        public ServiceResponse<Boolean> RemoveSchoolNodeAssociation(ServiceRequest<Int32, SchoolNodeAssociationContract> data)
        {
            return _agencyHierarchyServiceChannel.RemoveSchoolNodeAssociation(data);
        }

        public ServiceResponse<Boolean> IsSchoolNodeAssociationExists(ServiceRequest<Int32, Int32, Int32, Int32> data)
        {
            return _agencyHierarchyServiceChannel.IsSchoolNodeAssociationExists(data);
        }

        #endregion

        #region [Agency Hierarchy Profile Share Permission]
        public ServiceResponse<List<AgencyHierarchyProfileSharePermissionDataContract>> GetProfileSharePermissionByAgencyHierarchyID(ServiceRequest<Int32, Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetProfileSharePermissionByAgencyHierarchyID(data);
        }

        public ServiceResponse<Boolean> SaveUpdateProfileSharePermission(ServiceRequest<Int32, AgencyHierarchyProfileSharePermissionDataContract> data)
        {
            return _agencyHierarchyServiceChannel.SaveUpdateProfileSharePermission(data);
        }

        public ServiceResponse<Boolean> RemoveProfileSharePermission(ServiceRequest<Int32, AgencyHierarchyProfileSharePermissionDataContract> data)
        {
            return _agencyHierarchyServiceChannel.RemoveProfileSharePermission(data);
        }
        #endregion

        #region UAT-2641
        public ServiceResponse<List<Int32>> GetAgencyHierarchyIdsByOrgUserID(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyIdsByOrgUserID(data);
        }
        #endregion

        #region UAT-2633
        public ServiceResponse<List<AgencyNodeMappingContract>> GetAgencies(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencies(data);
        }
        public ServiceResponse<List<AgencyNodeMappingContract>> GetAgencyHierarchyAgencies(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyAgencies(data);
        }
        public ServiceResponse<Boolean> SaveAgencyHierarchyAgencyMapping(ServiceRequest<AgencyNodeMappingContract> data)
        {
            return _agencyHierarchyServiceChannel.SaveAgencyHierarchyAgencyMapping(data);
        }
        public ServiceResponse<Boolean> DeleteAgencyHierarchyAgencyMapping(ServiceRequest<AgencyNodeMappingContract> data)
        {
            return _agencyHierarchyServiceChannel.DeleteAgencyHierarchyAgencyMapping(data);
        }
        public ServiceResponse<Boolean> IsAgencyHierarchyLeafNode(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.IsAgencyHierarchyLeafNode(data);
        }
        #endregion


        #region [UAT-2653]

        public ServiceResponse<List<Int32>> GetAgencyHiearchyIdsByDeptProgMappingID(ServiceRequest<Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHiearchyIdsByDeptProgMappingID(data);
        }

        #endregion

        #region UAT-2647

        public ServiceResponse<List<Int32>> GetAgencyHierarchyIdsByTenantID(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyIdsByTenantID(data);
        }

        #endregion

        #region UAT-3245

        public ServiceResponse<List<Int32>> GetAgencyHierarchyIdsByLstTenantIDs(ServiceRequest<List<Int32>> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyIdsByLstTenantIDs(data);
        }

        #endregion

        public ServiceResponse<String> GetAgencyHiearchyIdsByTenantID(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHiearchyIdsByTenantID(data);
        }

        #region UAT-2548
        public ServiceResponse<List<TenantDetailContract>> GetTenants(ServiceRequest<Boolean, String> data)
        {
            return _agencyHierarchyServiceChannel.GetTenants(data);
        }
        public ServiceResponse<Boolean> SaveUpdateAgencyHierarchyTenantAccessMapping(ServiceRequest<Int32, List<Int32>, Int32> data)
        {
            return _agencyHierarchyServiceChannel.SaveUpdateAgencyHierarchyTenantAccessMapping(data);
        }
        public ServiceResponse<List<Int32>> GetAgencyHierarchyTenantAccessDetails(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyTenantAccessDetails(data);
        }
        #endregion

        #region UAT-2712
        public ServiceResponse<AgencyHierarchyRotationFieldOptionContract> GetAgencyHierarchyRotationFieldOptionSetting(ServiceRequest<Int32> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyRotationFieldOptionSetting(data);
        }


        public ServiceResponse<Boolean> SaveAgencyHierarchyRotationFieldOptionSetting(ServiceRequest<AgencyHierarchyRotationFieldOptionContract> data)
        {
            return _agencyHierarchyServiceChannel.SaveAgencyHierarchyRotationFieldOptionSetting(data);
        }
        #endregion

        #region [UAT-2427: Job Board]

        public ServiceResponse<List<AgencyJobContract>> GetAgencyJobTemplate(ServiceRequest<Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.GetAgencyJobTemplate(serviceRequest);
        }

        public ServiceResponse<Boolean> SaveAgencyJobTemplate(ServiceRequest<AgencyJobContract, Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.SaveAgencyJobTemplate(serviceRequest);
        }

        public ServiceResponse<Boolean> DeleteAgencyJobTemplate(ServiceRequest<AgencyJobContract, Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.DeleteAgencyJobTemplate(serviceRequest);
        }

        public ServiceResponse<Int32> GetAgencyHierarchyId(ServiceRequest<Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyId(serviceRequest);
        }

        public ServiceResponse<List<AgencyJobContract>> GetAgencyJobPosting(ServiceRequest<Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.GetAgencyJobPosting(serviceRequest);
        }

        public ServiceResponse<Boolean> SaveAgencyJobPosting(ServiceRequest<AgencyJobContract, Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.SaveAgencyJobPosting(serviceRequest);
        }

        public ServiceResponse<Boolean> DeleteAgencyJobPosting(ServiceRequest<AgencyJobContract, Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.DeleteAgencyJobPosting(serviceRequest);
        }

        public ServiceResponse<AgencyJobContract> GetTemplateDetailsByID(ServiceRequest<Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.GetTemplateDetailsByID(serviceRequest);
        }

        public ServiceResponse<AgencyLogoContract> GetAgencyLogo(ServiceRequest<Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.GetAgencyLogo(serviceRequest);
        }

        public ServiceResponse<Boolean> SaveUpdateAgencyLogo(ServiceRequest<AgencyLogoContract, Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.SaveUpdateAgencyLogo(serviceRequest);
        }

        public ServiceResponse<Boolean> ArchiveJobPosts(ServiceRequest<List<Int32>, Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.ArchiveJobPosts(serviceRequest);
        }

        public ServiceResponse<AgencyJobContract> GetSelectedJobPostDetails(ServiceRequest<Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.GetSelectedJobPostDetails(serviceRequest);
        }

        public ServiceResponse<Boolean> ClearLogo(ServiceRequest<Int32, Int32> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.ClearLogo(serviceRequest);
        }

        public ServiceResponse<List<AgencyJobContract>> GetViewAgencyJobPosting(ServiceRequest<JobSearchContract, CustomPagingArgsContract> serviceRequest)
        {
            return _agencyHierarchyServiceChannel.GetViewAgencyJobPosting(serviceRequest);
        }

        #endregion


        #region UAT-3071
        public ServiceResponse<List<DefinedRequirementContract>> GetJobFieldType()
        {
            return _agencyHierarchyServiceChannel.GetJobFieldType();
        }
        #endregion

        #region UAT-2784
        public ServiceResponse<AgencyHierarchySettingContract> GetAgencyHierarchySetting(ServiceRequest<Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchySetting(data);
        }


        public ServiceResponse<Boolean> SaveAgencyHierarchySetting(ServiceRequest<AgencyHierarchySettingContract> data)
        {
            return _agencyHierarchyServiceChannel.SaveAgencyHierarchySetting(data);
        }
        #endregion

        #region [UAT-2723]

        public ServiceResponse<Boolean> SaveClientSystemDocument(ServiceRequest<Int32, List<RequirementApprovalNotificationDocumentContract>> data)
        {
            return _agencyHierarchyServiceChannel.SaveClientSystemDocument(data);
        }

        public ServiceResponse<RequirementApprovalNotificationDocumentContract> GetClientSystemDocumentBasedOnDocumentType(ServiceRequest<Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.GetClientSystemDocumentBasedOnDocumentType(data);
        }

        public ServiceResponse<Boolean> DeleteClientSystemDocumentBasedOnDocType(ServiceRequest<Int32, Int32?, Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.DeleteClientSystemDocumentBasedOnDocType(data);
        }

        #endregion

        //UAT 2821
        public ServiceResponse<Boolean> DeleteAgencyHierarchySetting(ServiceRequest<Int32, Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.DeleteAgencyHierarchySetting(data);
        }

        #region UAT-3237
        public ServiceResponse<Boolean> UpdateNodeDisplayOrder(ServiceRequest<List<AgencyHierarchyDataContract>, Int32?, Int32> data)
        {
            return _agencyHierarchyServiceChannel.UpdateNodeDisplayOrder(data);
        }
        #endregion

        public ServiceResponse<Boolean> SaveAgencyHierarchyRootNodeSetting(ServiceRequest<AgencyHierarchyRootNodeSettingContract> data)
        {
            return _agencyHierarchyServiceChannel.SaveAgencyHierarchyRootNodeSetting(data);
        }
        public ServiceResponse<List<AgencyHierarchyRootNodeSettingContract>> GetAgencyHierarchyRootNodeMapping(ServiceRequest<Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.GetAgencyHierarchyRootNodeMapping(data);
        }

        public ServiceResponse<Boolean> SaveUpdateAgencyHierarchyRootNodeMapping(ServiceRequest<AgencyHierarchyRootNodeSettingContract> data)
        {
            return _agencyHierarchyServiceChannel.SaveUpdateAgencyHierarchyRootNodeMapping(data);
        }
        public ServiceResponse<Boolean> IsAgencyHierarchyRootNodeSettingExist(ServiceRequest<Int32, String> data)
        {
            return _agencyHierarchyServiceChannel.IsAgencyHierarchyRootNodeSettingExist(data);
        }
    }
}
