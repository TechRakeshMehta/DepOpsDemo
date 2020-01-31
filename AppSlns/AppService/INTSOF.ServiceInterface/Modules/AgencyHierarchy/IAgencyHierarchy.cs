using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceInterface.Modules.AgencyHierarchy
{
    [ServiceContract]
    public interface IAgencyHierarchy
    {
        #region Common Control
        [OperationContract]
        ServiceResponse<List<AgencyHierarchyContract>> GetAgencyHierarchy(ServiceRequest<Int32, String> data);
        [OperationContract]
        ServiceResponse<List<AgencyHierarchyContract>> GetTreeDataByRootNodeID(ServiceRequest<Int32, Int32> data);
        [OperationContract]
        ServiceResponse<AgencyHierarchyContract> GetAgencyDetailByNodeId(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<List<AgencyHierarchyContract>> GetAgencyHierarchyByRootNodeIds(ServiceRequest<Int32, String, String> data);
        [OperationContract]
        ServiceResponse<List<AgencyHierarchyContract>> GetAgencyDetailByMultipleNodeIds(ServiceRequest<Int32, String> data);
        [OperationContract]
        ServiceResponse<List<AgencyHierarchyContract>> GetTreeDataByRootNodeIDForPopUp(ServiceRequest<Int32, AgencyHierarchPopUpParameter> data);
        [OperationContract]
        ServiceResponse<String> GetAgencyDetailByMultipleNodeID(ServiceRequest<Int32, AgencyHierarchMultiSelectParameter> data);
        [OperationContract]
        ServiceResponse<String> GetAgencyHierarchyLabel(ServiceRequest<Int32, Int32, Int32> data);
        [OperationContract]
        ServiceResponse<String> GetAgencyHierarchyParent(ServiceRequest<Int32, String> data);
        [OperationContract] 
        ServiceResponse<String> GetAgencyHierarchyLabelForMultipleSelection(ServiceRequest<Int32, String> data);
        [OperationContract]
        ServiceResponse<List<String>> GetAgencyHierarchyAgencyByMultipleNodeIds(ServiceRequest<Int32, AgencyHierarchMultiSelectParameter> data);

        #endregion

        #region UAT-2630:Agency hierarchy mapping: Agency Hierarchies grid
        [OperationContract]
        ServiceResponse<List<AgencyHierarchyDataContract>> GetRootAgencyHierarchyData(ServiceRequest<CustomPagingArgsContract> data);
        [OperationContract]
        ServiceResponse<Boolean> DeleteRootAgencyHierarchy(ServiceRequest<Int32> data);
        #endregion

        #region AgencyHierarchyMapping
        [OperationContract]
        ServiceResponse<Int32> SaveAgencyHierarchyMapping(ServiceRequest<AgencyNodeMappingContract, Int32> data);
        [OperationContract]
        ServiceResponse<Boolean> CheckForLeafNode(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<Boolean> DeleteAgencyNodeMapping(ServiceRequest<Int32, Int32> data);
        [OperationContract]
        ServiceResponse<AgencyNodeMappingContract> GetAgencyHierarchyAgencyMapping(ServiceRequest<Int32> data);
        #endregion

        #region UAT-2634 :- Agency Hierarchy Package Mapping
        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetRequirementPackages();
        [OperationContract]
        ServiceResponse<List<RequirementPackageContract>> GetAgencyHierarchyPackages(ServiceRequest<CustomPagingArgsContract, Int32> data);
        [OperationContract]
        ServiceResponse<Boolean> SaveAgencyHierarchyPackageMapping(ServiceRequest<AgencyHierarchyPackageContract> data);
        [OperationContract]
        ServiceResponse<Boolean> DeleteAgencyHierarchyPackageMapping(ServiceRequest<AgencyHierarchyPackageContract> data);
        #endregion

        #region UAT-2632:UAT-2632:Agency hierarchy mapping: Map Node
        [OperationContract]
        ServiceResponse<List<AgencyHierarchyDataContract>> GetMappedNodesByNodeID(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<Boolean> DeleteNodeMapping(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<List<AgencyNodeContract>> GetAgencyNodeListForMapping();
        [OperationContract]
        ServiceResponse<Tuple<Boolean, Int32>> SaveNodeMapping(ServiceRequest<Int32, Int32, String> data);
        [OperationContract]
        ServiceResponse<Boolean> IsAgencyMappedWithNode(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<Int32> GetAgencyNodeIDByAgencyHierarchyID(ServiceRequest<Int32> data);

        #endregion

        #region UAT-2629

        [OperationContract]
        ServiceResponse<Boolean> SaveNodeDetail(ServiceRequest<AgencyNodeContract> data);

        [OperationContract]
        ServiceResponse<List<AgencyNodeContract>> GetAgencyNodeList();

        #endregion

        //UAT-3652
        [OperationContract]
        ServiceResponse<List<AgencyNodeContract>> GetAgencyNodeRootList(ServiceRequest<CustomPagingArgsContract, String, String> data);

        [OperationContract]
        Boolean IsNodeExist(String nodeName, Int32? nodeId = null);

        #region UAT-2636: Agency hierarchy mapping: Map users with hierarchy node
        [OperationContract]
        ServiceResponse<List<AgencyHierarchyUserContract>> GetAgencyUsers(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<List<AgencyHierarchyUserContract>> GetAgencyHierarchyUsers(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<Boolean> SaveAgencyHierarchyUserMapping(ServiceRequest<AgencyHierarchyUserContract> data);
        [OperationContract]
        ServiceResponse<Boolean> DeleteAgencyHierarchyUserMapping(ServiceRequest<AgencyHierarchyUserContract> data);
        #endregion

        #region [UAT-2635]

        [OperationContract]
        ServiceResponse<List<SchoolNodeAssociationDataContract>> GetSchoolNodeAssociationByAgencyHierarchyID(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateSchoolNodeAssociation(ServiceRequest<Int32, SchoolNodeAssociationContract> data);

        [OperationContract]
        ServiceResponse<Boolean> RemoveSchoolNodeAssociation(ServiceRequest<Int32, SchoolNodeAssociationContract> data);

        [OperationContract]
        ServiceResponse<Boolean> IsSchoolNodeAssociationExists(ServiceRequest<Int32, Int32, Int32, Int32> data);

        #endregion

        #region [Agency Hierarchy Profile Share Permission]

        [OperationContract]
        ServiceResponse<List<AgencyHierarchyProfileSharePermissionDataContract>> GetProfileSharePermissionByAgencyHierarchyID(ServiceRequest<Int32, Int32> data);

        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateProfileSharePermission(ServiceRequest<Int32, AgencyHierarchyProfileSharePermissionDataContract> data);

        [OperationContract]
        ServiceResponse<Boolean> RemoveProfileSharePermission(ServiceRequest<Int32, AgencyHierarchyProfileSharePermissionDataContract> data);
        #endregion

        #region UAT-2641

        [OperationContract]
        ServiceResponse<List<Int32>> GetAgencyHierarchyIdsByOrgUserID(ServiceRequest<Int32> data);
        #endregion

        #region UAT-2633
        [OperationContract]
        ServiceResponse<Boolean> DeleteAgencyHierarchyAgencyMapping(ServiceRequest<AgencyNodeMappingContract> data);
        [OperationContract]
        ServiceResponse<List<AgencyNodeMappingContract>> GetAgencyHierarchyAgencies(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<List<AgencyNodeMappingContract>> GetAgencies(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<Boolean> SaveAgencyHierarchyAgencyMapping(ServiceRequest<AgencyNodeMappingContract> data);
        [OperationContract]
        ServiceResponse<Boolean> IsAgencyHierarchyLeafNode(ServiceRequest<Int32> data);
        #endregion

        #region [UAT-2653]

        [OperationContract]
        ServiceResponse<List<Int32>> GetAgencyHiearchyIdsByDeptProgMappingID(ServiceRequest<Int32, String> data);

        #endregion

        #region UAT-2647
        [OperationContract]
        ServiceResponse<List<Int32>> GetAgencyHierarchyIdsByTenantID(ServiceRequest<Int32> data);
        #endregion

        #region UAT-3245
        [OperationContract]
        ServiceResponse<List<Int32>> GetAgencyHierarchyIdsByLstTenantIDs(ServiceRequest<List<Int32>> data);
        #endregion

        [OperationContract]
        ServiceResponse<String> GetAgencyHiearchyIdsByTenantID(ServiceRequest<Int32> data);

        #region UAT-2548
        [OperationContract]
        ServiceResponse<List<TenantDetailContract>> GetTenants(ServiceRequest<Boolean, String> data);
        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateAgencyHierarchyTenantAccessMapping(ServiceRequest<Int32, List<Int32>, Int32> data);
        [OperationContract]
        ServiceResponse<List<Int32>> GetAgencyHierarchyTenantAccessDetails(ServiceRequest<Int32> data);
        #endregion

        #region UAT-2712
        [OperationContract]
        ServiceResponse<AgencyHierarchyRotationFieldOptionContract> GetAgencyHierarchyRotationFieldOptionSetting(ServiceRequest<Int32> data);
        [OperationContract]
        ServiceResponse<Boolean> SaveAgencyHierarchyRotationFieldOptionSetting(ServiceRequest<AgencyHierarchyRotationFieldOptionContract> data);
        #endregion


        #region [UAT-2427: Job Board]

        [OperationContract]
        ServiceResponse<List<AgencyJobContract>> GetAgencyJobTemplate(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> SaveAgencyJobTemplate(ServiceRequest<AgencyJobContract, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> DeleteAgencyJobTemplate(ServiceRequest<AgencyJobContract, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Int32> GetAgencyHierarchyId(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<List<AgencyJobContract>> GetAgencyJobPosting(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> SaveAgencyJobPosting(ServiceRequest<AgencyJobContract, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> DeleteAgencyJobPosting(ServiceRequest<AgencyJobContract, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<AgencyJobContract> GetTemplateDetailsByID(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<AgencyLogoContract> GetAgencyLogo(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateAgencyLogo(ServiceRequest<AgencyLogoContract, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> ArchiveJobPosts(ServiceRequest<List<Int32>, Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<AgencyJobContract> GetSelectedJobPostDetails(ServiceRequest<Int32> serviceRequest);

        [OperationContract]
        ServiceResponse<Boolean> ClearLogo(ServiceRequest<Int32, Int32> serviceRequest);

        #region Job Board
        [OperationContract]
        ServiceResponse<List<AgencyJobContract>> GetViewAgencyJobPosting(ServiceRequest<JobSearchContract, CustomPagingArgsContract> serviceRequest);
        #endregion
        #endregion

        #region UAT-3071
        [OperationContract]
        ServiceResponse<List<DefinedRequirementContract>> GetJobFieldType();
        
        #endregion

        #region UAT-2784
        [OperationContract]
        ServiceResponse<AgencyHierarchySettingContract> GetAgencyHierarchySetting(ServiceRequest<Int32, String> data);
        [OperationContract]
        ServiceResponse<Boolean> SaveAgencyHierarchySetting(ServiceRequest<AgencyHierarchySettingContract> data);
        #endregion

        #region [UAT-2723]

        [OperationContract]
        ServiceResponse<Boolean> SaveClientSystemDocument(ServiceRequest<Int32, List<RequirementApprovalNotificationDocumentContract>> data);

        [OperationContract]
        ServiceResponse<RequirementApprovalNotificationDocumentContract> GetClientSystemDocumentBasedOnDocumentType(ServiceRequest<Int32, String> data);

        [OperationContract]
        ServiceResponse<Boolean> DeleteClientSystemDocumentBasedOnDocType(ServiceRequest<Int32, Int32?, Int32, String> data);

        #endregion 

        //UAT 2821 
        [OperationContract]
        ServiceResponse<Boolean> DeleteAgencyHierarchySetting(ServiceRequest<Int32,Int32,String> data);

        [OperationContract]
        ServiceResponse<Boolean> UpdateNodeDisplayOrder(ServiceRequest<List<AgencyHierarchyDataContract>, Int32?,Int32> data); //UAT-3237
        [OperationContract]
        ServiceResponse<Boolean> SaveAgencyHierarchyRootNodeSetting(ServiceRequest<AgencyHierarchyRootNodeSettingContract> data);
        [OperationContract]
        ServiceResponse<List<AgencyHierarchyRootNodeSettingContract>> GetAgencyHierarchyRootNodeMapping(ServiceRequest<Int32, String> data);
        [OperationContract]
        ServiceResponse<Boolean> SaveUpdateAgencyHierarchyRootNodeMapping(ServiceRequest<AgencyHierarchyRootNodeSettingContract> data);
        [OperationContract]
        ServiceResponse<Boolean> IsAgencyHierarchyRootNodeSettingExist(ServiceRequest<Int32, String> data);
    }
}
