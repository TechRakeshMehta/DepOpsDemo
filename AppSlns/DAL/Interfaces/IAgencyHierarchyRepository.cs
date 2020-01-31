using Entity.SharedDataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace DAL.Interfaces
{
    public interface IAgencyHierarchyRepository
    {

        #region Common Control
        List<AgencyHierarchyContract> GetAgencyHierarchy(String agencyHierarchyNodeIds);
        #region Agency Node Mapping
        Int32 SaveAgencyHierarchyMapping(AgencyNodeMappingContract agencyNodeMappingContract);
        Boolean CheckForLeafNode(Int32 HierarchyNodeId);
        Boolean DeleteAgencyNodeMapping(Int32 HierarchyNodeId, Int32 AgencyId);
        AgencyNodeMappingContract GetAgencyHierarchyAgencyMapping(Int32 HierarchyNodeId);
        #endregion

        List<AgencyHierarchyContract> GetTreeDataByRootNodeID(Int32 rootNodeId);
        AgencyHierarchyContract GetAgencyDetailByNodeId(Int32 nodeId);

        List<AgencyHierarchyContract> GetAgencyHierarchyByRootNodeIds(String rootNodeIds, String agencyHierarchyIds);
        String GetAgencyDetailByMultipleNodeID(AgencyHierarchMultiSelectParameter parm);
        List<String> GetAgencyHierarchyAgencyByMultipleNodeIds(AgencyHierarchMultiSelectParameter parm);//UAT-2926
        #endregion
        List<AgencyHierarchyContract> GetAgencyDetailByMultipleNodeIds(String nodeIds);
        List<AgencyHierarchyContract> GetTreeDataByRootNodeIDForPopUp(AgencyHierarchPopUpParameter agencyHierarchPopUpParameter);

        String GetAgencyHierarchyLabel(Int32 agencyId, Int32 agencyHierarchyId);
        String GetAgencyHierarchyParent(String agencyHierarchyIDs);


        String GetAgencyHierarchyLabelForMultipleSelection(String agencyHierarchyNodeIds);

        #region UAT-2630:Agency hierarchy mapping: Agency Hierarchies grid
        List<AgencyHierarchyDataContract> GetRootAgencyHierarchyData(CustomPagingArgsContract customPagingContract);
        Boolean DeleteRootAgencyHierarchy(Int32 agencyHierarchyId, Int32 currentLoggedInUserId);
        #endregion

        #region UAT-2634 :- Agency Hierarchy Package Mapping
        List<RequirementPackageContract> GetRequirementPackages();
        List<RequirementPackageContract> GetAgencyHierarchyPackages(CustomPagingArgsContract customPagingContract, Int32 agencyHierarchyID);
        Boolean SaveAgencyHierarchyPackageMapping(AgencyHierarchyPackageContract agencyHierarchyPackageContract);
        Boolean DeleteAgencyHierarchyPackageMapping(AgencyHierarchyPackageContract agencyHierarchyPackageContract);
        #endregion

        #region UAT-2632:Agency hierarchy mapping: Map Node
        List<AgencyHierarchyDataContract> GetMappedNodesByNodeID(Int32 parentNodeID);
        Boolean DeleteNodeMapping(Int32 nodeId, Int32 currentLoggedInUserId);
        Boolean IsAgencyMappedOnNode(Int32 nodeId);
        Tuple<Boolean, Int32> SaveNodeMapping(Int32 parentNodeId, Int32 agencyNodeId, String nodeLabel, Int32 currentLoggedInUserId);
        Int32 GetAgencyNodeIDByAgencyHierarchyID(Int32 agencyHierarchyId);

        #endregion

        #region UAT-2629

        Boolean SaveNodeDetail(AgencyNodeContract nodeDetail);

        List<AgencyNodeContract> GetAgencyNodeList();

        List<AgencyNodeContract> GetAgencyNodeRootList(CustomPagingArgsContract customPagingArgsContract, String agencyNodeName, String agencyNodeDesc); //UAT-3652
        Boolean IsNodeExist(String nodeName, Int32? nodeId = null); //UAT-3652

        Boolean IsNodeMapped(Int32 nodeId);

        #endregion

        #region UAT-2636 : Agency hierarchy mapping: Map users with hierarchy node
        List<AgencyHierarchyUserContract> GetAgencyUsers(Int32 agencyHierarchyID);
        List<AgencyHierarchyUserContract> GetAgencyHierarchyUsers(Int32 agencyHierarchyID);
        Boolean SaveAgencyHierarchyUserMapping(AgencyHierarchyUserContract agencyHierarchyUserContract);
        Boolean DeleteAgencyHierarchyUserMapping(AgencyHierarchyUserContract agencyHierarchyUserContract);
        void CallDigestionStoreProcedureFunctionForAgencyHierarchy(Dictionary<String, Object> param);
        #endregion

        #region [UAT-2635]

        List<SchoolNodeAssociationDataContract> GetSchoolNodeAssociationByAgencyHierarchyID(Int32 agencyHierarchyID);

        Boolean IsSchoolNodeAssociationExists(Int32 agencyHierarchyInstitutionNodeID, Int32 agencyHierarchyID, Int32 DPM_ID);

        Boolean SaveUpdateSchoolNodeAssociation(SchoolNodeAssociationContract schoolNodeAssociationContract);

        Boolean RemoveSchoolNodeAssociation(SchoolNodeAssociationContract schoolNodeAssociationContract);

        Boolean IsSchoolNodeAssociationExists(Int32 agencyHierarchyID);

        void AddRemoveAgencyHierarchyTenantMapping(Int32 tenantID, Int32 agencyHierarchyID, Int32 currentLoggedInUserID, Boolean needToRemove, Boolean IsAdminShare, Boolean IsStudentShare);

        #endregion

        List<AgencyHierarchyProfileSharePermissionDataContract> GetProfileSharePermissionByAgencyHierarchyID(Int32 agencyHierarchyID);

        Boolean SaveUpdateSchoolNodeAssociation(AgencyHierarchyProfileSharePermissionDataContract agencyHierarchyProfileSharePermissionDataContract);

        Boolean RemoveProfileSharePermission(AgencyHierarchyProfileSharePermissionDataContract agencyHierarchyProfileSharePermissionDataContract);

        #region UAT-2641
        List<Int32> GetAgencyHierarchyIdsByOrgUserID(Int32 OrgUserID);
        #endregion

        #region UAT-2633
        List<AgencyNodeMappingContract> GetAgencies(Int32 agencyHierarchyID);
        List<AgencyNodeMappingContract> GetAgencyHierarchyAgencies(Int32 agencyHierarchyID);
        Boolean IsAgencyHierarchyLeafNode(Int32 AgencyHierarchyID);
        Boolean DeleteAgencyHierarchyAgencyMapping(AgencyNodeMappingContract agencyNodeMappingContract);
        Boolean SaveAgencyHierarchyAgencyMapping(AgencyNodeMappingContract agencyNodeMappingContract);
        #endregion

        Boolean DeleteAgencyHierarchySetting(Int32 agencyID, Int32 currentLoggedInUserID, String SettingType); //UAT 2821

        #region [UAT-2653]

        List<Int32> GetAgencyHiearchyIdsByDeptProgMappingID(String DPM_ID);

        #endregion

        #region UAT-2647
        List<Int32> GetAgencyHierarchyIdsByTenantID(Int32 TenantId);
        #endregion

        #region UAT-3245
        List<Int32> GetAgencyHierarchyIdsByLstTenantIDs(List<Int32> lstTenantIds);
        #endregion

        void CallDigestionProcedure(Dictionary<String, Object> param);

        String GetAgencyHiearchyIdsByTenantID(Int32 TenantID);

        #region UAT-2712
        AgencyHierarchyRotationFieldOptionContract GetAgencyHierarchyRotationFieldOptionSetting(Int32 agencyHierarchyID);
        Boolean SaveAgencyHierarchyRotationFieldOptionSetting(AgencyHierarchyRotationFieldOptionContract agencyHierarchyRotationFieldOptionContract);
        //  Boolean SaveDefaultAgencyHierarchyRotationFieldOptionSetting(Int32 agencyHierarchyID, Int32 currentLoggedInUser);
        #endregion

        //UAT-2548
        Boolean SaveUpdateAgencyHierarchyTenantAccessMapping(Int32 AgencyHierarchyId, List<Int32> lstTenantIds, Int32 CurrentLoggedInUserId);
        List<Int32> GetAgencyHierarchyTenantAccessDetails(Int32 AgencyHierarchyId);


        #region UAT-2784
        AgencyHierarchySettingContract GetAgencyHierarchySetting(Int32 agencyHierarchyID, String settingTypeCode);
        Boolean SaveAgencyHierarchySetting(AgencyHierarchySettingContract agencyHierarchyContract);
        #endregion

        //UAT-2982
        void DeletingAgencyHierarchyMappings(Int32 agencyHierarchyId, Int32 currentLoggedInUserID);

        //UAT-3241
        List<String> GetAgencyNamesByIds(List<Int32> lstAgencyIds);

        Boolean UpdateNodeDisplayOrder(List<AgencyHierarchyDataContract> lstAgencyHierarchy, Int32? destinationIndex, Int32 currentLoggedInuser); //UAT 3237
        AgencySetting GetInstPrecpReqdSetting(Int32 agencyId); //UAT-3662

        Boolean SaveAgencyHierarchyRootNodeSetting(Int32 agencyHierarchyRootNodeId, String agencyHierarchySettingTypeCode, String agencyHierarchySettingValue, Int32 CurrentLoggedInUserID);
        List<AgencyHierarchyRootNodeSettingContract> GetAgencyHierarchyRootNodeMapping(Int32 agencyHierarchyRootNodeId, String agencyHierarchySettingTypeCode);
        Boolean SaveUpdateAgencyHierarchyRootNodeMapping(AgencyHierarchyRootNodeSettingContract agencyHierarchyRootNodeSettingContract);

        Boolean IsAgencyHierarchyRootNodeSettingExist(Int32 agencyHierarchyRootNodeId, String agencyHierarchySettingTypeCode);
        List<Entity.SharedDataEntity.AgencyHierarchy> GetAgencyHierarchyRootNodes(); //UAT-3704

        //UAT-4257
        List<AgencyHierarchyContract> GetAgencyHierarchyRootNodesByTenantIDs(List<Int32> tenantIDs);

        //UAT-4402
        List<RequirementCategoryContract> GetRequirementCategoryByPackageID(Int32 packageId);
        //UAT-4657
        String IsPackageVersionInProgress(Int32 PkgId, Int32 requirementPkgVersioningStatus_DueId, Int32 requirementPkgVersioningStatus_InProgressId);
    }
}
