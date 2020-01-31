using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System;
using Entity.ClientEntity;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.Utils;
using iTextSharp.text.pdf;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using INTSOF.UI.Contract.Templates;
using INTSOF.UI.Contract.SystemSetUp;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;

namespace Business.RepoManagers
{
    public static class AgencyHierarchyManager
    {
        #region Agency Node Mapping
        public static Int32 SaveAgencyHierarchyMapping(AgencyNodeMappingContract agencyNodeMappingContract, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).SaveAgencyHierarchyMapping(agencyNodeMappingContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean CheckForLeafNode(Int32 HierarchyNodeId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).CheckForLeafNode(HierarchyNodeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean DeleteAgencyNodeMapping(Int32 HierarchyNodeId, Int32 AgencyId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).DeleteAgencyNodeMapping(HierarchyNodeId, AgencyId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static AgencyNodeMappingContract GetAgencyHierarchyAgencyMapping(Int32 HierarchyNodeId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyAgencyMapping(HierarchyNodeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Common Control
        public static List<AgencyHierarchyContract> GetAgencyHierarchy(Int32 tenantId, String agencyHierarchyNodeIds)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetAgencyHierarchy(agencyHierarchyNodeIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AgencyHierarchyContract> GetTreeDataByRootNodeID(Int32 tenantId, Int32 rootNodeId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetTreeDataByRootNodeID(rootNodeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static AgencyHierarchyContract GetAgencyDetailByNodeId(Int32 tenantId, Int32 nodeId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetAgencyDetailByNodeId(nodeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<AgencyHierarchyContract> GetAgencyHierarchyByRootNodeIds(Int32 tenantId, String rootNodeIds, String agencyHierarchyIds)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetAgencyHierarchyByRootNodeIds(rootNodeIds, agencyHierarchyIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static String GetAgencyDetailByMultipleNodeID(Int32 tenantId, AgencyHierarchMultiSelectParameter parm)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetAgencyDetailByMultipleNodeID(parm);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<String> GetAgencyHierarchyAgencyByMultipleNodeIds(Int32 tenantId, AgencyHierarchMultiSelectParameter parm) //UAT-2926
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetAgencyHierarchyAgencyByMultipleNodeIds(parm);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AgencyHierarchyContract> GetAgencyDetailByMultipleNodeIds(Int32 tenantId, String nodeIds)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetAgencyDetailByMultipleNodeIds(nodeIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<AgencyHierarchyContract> GetTreeDataByRootNodeIDForPopUp(Int32 tenantId, AgencyHierarchPopUpParameter agencyHierarchPopUpParameter)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetTreeDataByRootNodeIDForPopUp(agencyHierarchPopUpParameter);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetAgencyHierarchyLabel(Int32 tenantId, Int32 agencyId, Int32 agencyHierarchyId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetAgencyHierarchyLabel(agencyId, agencyHierarchyId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetAgencyHierarchyParent(Int32 tenantId, String agencyHierarchyIDs)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetAgencyHierarchyParent(agencyHierarchyIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetAgencyHierarchyLabelForMultipleSelection(Int32 tenantId, String agencyHierarchyNodeIds)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetAgencyHierarchyLabelForMultipleSelection(agencyHierarchyNodeIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-2630:Agency hierarchy mapping: Agency Hierarchies grid
        public static List<AgencyHierarchyDataContract> GetRootAgencyHierarchyData(CustomPagingArgsContract customPagingContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetRootAgencyHierarchyData(customPagingContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteRootAgencyHierarchy(Int32 agencyHierarchyId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).DeleteRootAgencyHierarchy(agencyHierarchyId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        #region UAT-2634:- Agency Hierarchy Package Mapping
        public static List<RequirementPackageContract> GetRequirementPackages()
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetRequirementPackages();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementPackageContract> GetAgencyHierarchyPackages(CustomPagingArgsContract customPagingContract, Int32 agencyHierarchyID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyPackages(customPagingContract, agencyHierarchyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveAgencyHierarchyPackageMapping(AgencyHierarchyPackageContract agencyHierarchyPackageContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).SaveAgencyHierarchyPackageMapping(agencyHierarchyPackageContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean DeleteAgencyHierarchyPackageMapping(AgencyHierarchyPackageContract agencyHierarchyPackageContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).DeleteAgencyHierarchyPackageMapping(agencyHierarchyPackageContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2632:Agency hierarchy mapping: Map Node
        public static List<AgencyHierarchyDataContract> GetMappedNodesByNodeID(Int32 parentNodeID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetMappedNodesByNodeID(parentNodeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteNodeMapping(Int32 nodeId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).DeleteNodeMapping(nodeId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsAgencyMappedOnNode(Int32 nodeId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).IsAgencyMappedOnNode(nodeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Tuple<Boolean, Int32> SaveNodeMapping(Int32 parentNodeId, Int32 agencyNodeId, String nodeLabel, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).SaveNodeMapping(parentNodeId, agencyNodeId, nodeLabel, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 GetAgencyNodeIDByAgencyHierarchyID(Int32 agencyHierarchyId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyNodeIDByAgencyHierarchyID(agencyHierarchyId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2629

        /// <summary>
        /// Get list of node to bind the grid.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<AgencyNodeContract> GetAgencyNodeList()
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyNodeList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //UAT-3652
        public static List<AgencyNodeContract> GetAgencyNodeRootList(CustomPagingArgsContract customPagingArgsContract, String agencyNodeName, String agencyNodeDesc)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyNodeRootList(customPagingArgsContract, agencyNodeName, agencyNodeDesc);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsNodeExist(String nodeName, Int32? nodeId = null)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).IsNodeExist(nodeName, nodeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Save/Update and delete the details of a node.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="nodeDetail"></param>
        /// <returns></returns>
        public static Boolean SaveNodeDetail(AgencyNodeContract agencyNodeContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).SaveNodeDetail(agencyNodeContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Check whether node in use or not.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public static Boolean IsNodeMapped(Int32 nodeId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).IsNodeMapped(nodeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-2636 : Agency hierarchy mapping: Map users with hierarchy node
        public static List<AgencyHierarchyUserContract> GetAgencyUsers(Int32 agencyHierarchyID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyUsers(agencyHierarchyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<AgencyHierarchyUserContract> GetAgencyHierarchyUsers(Int32 agencyHierarchyID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyUsers(agencyHierarchyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SaveAgencyHierarchyUserMapping(AgencyHierarchyUserContract agencyHierarchyUserContract)
        {
            try
            {
                //UAT-3715
                if (BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).SaveAgencyHierarchyUserMapping(agencyHierarchyUserContract))
                {
                    if (agencyHierarchyUserContract.IsUpdateFlag && agencyHierarchyUserContract.AGU_ID > AppConsts.NONE && agencyHierarchyUserContract.CurrentLoggedInUser > AppConsts.NONE)
                    {
                        ProfileSharingManager.UpdateDocMappingForInvAttestation(agencyHierarchyUserContract.AGU_ID, null, agencyHierarchyUserContract.CurrentLoggedInUser);
                    }
                    return true;
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean DeleteAgencyHierarchyUserMapping(AgencyHierarchyUserContract agencyHierarchyUserContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).DeleteAgencyHierarchyUserMapping(agencyHierarchyUserContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void CallDigestionStoreProcedureFunctionForAgencyHierarchy(Dictionary<String, Object> param)
        {
            try
            {
                BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).CallDigestionStoreProcedureFunctionForAgencyHierarchy(param);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region [UAT-2635]

        public static List<SchoolNodeAssociationDataContract> GetSchoolNodeAssociationByAgencyHierarchyID(Int32 tenantId, Int32 agencyHierarchyID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetSchoolNodeAssociationByAgencyHierarchyID(agencyHierarchyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsSchoolNodeAssociationExists(Int32 tenantId, Int32 agencyHierarchyInstitutionNodeID, Int32 agencyHierarchyID, Int32 DPM_ID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).IsSchoolNodeAssociationExists(agencyHierarchyInstitutionNodeID, agencyHierarchyID, DPM_ID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveUpdateSchoolNodeAssociation(Int32 tenantId, SchoolNodeAssociationContract schoolNodeAssociationContract)
        {
            try
            {
                Boolean result = BALUtils.GetAgencyHierarchyRepoInstance(tenantId).SaveUpdateSchoolNodeAssociation(schoolNodeAssociationContract);

                //Adding Agency Hierarchy Tenant Mapping If Not Exists
                BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).AddRemoveAgencyHierarchyTenantMapping(tenantId, schoolNodeAssociationContract.AgencyHierarchyID, schoolNodeAssociationContract.CurrentLoggedInUserID, false, schoolNodeAssociationContract.IsAdminShare, schoolNodeAssociationContract.IsStudentShare);

                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean RemoveSchoolNodeAssociation(Int32 tenantId, SchoolNodeAssociationContract schoolNodeAssociationContract)
        {
            try
            {
                Boolean result = BALUtils.GetAgencyHierarchyRepoInstance(tenantId).RemoveSchoolNodeAssociation(schoolNodeAssociationContract);

                //Validating - Is School Node Association Exists for AgencyHierarchyID
                //If Not - then remove AgencyHierarchyID from AgencyHierarchyTenantMapping
                if (!BALUtils.GetAgencyHierarchyRepoInstance(tenantId).IsSchoolNodeAssociationExists(schoolNodeAssociationContract.AgencyHierarchyID))
                {
                    BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).AddRemoveAgencyHierarchyTenantMapping(tenantId, schoolNodeAssociationContract.AgencyHierarchyID, schoolNodeAssociationContract.CurrentLoggedInUserID, true, schoolNodeAssociationContract.IsAdminShare, schoolNodeAssociationContract.IsStudentShare);
                }
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region [Agency Hierarchy Profile Share Permission]

        public static List<AgencyHierarchyProfileSharePermissionDataContract> GetProfileSharePermissionByAgencyHierarchyID(Int32 tenantId, Int32 agencyHierarchyID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetProfileSharePermissionByAgencyHierarchyID(agencyHierarchyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveUpdateProfileSharePermission(Int32 tenantId, AgencyHierarchyProfileSharePermissionDataContract agencyHierarchyProfileSharePermissionDataContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).SaveUpdateSchoolNodeAssociation(agencyHierarchyProfileSharePermissionDataContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean RemoveProfileSharePermission(Int32 tenantId, AgencyHierarchyProfileSharePermissionDataContract agencyHierarchyProfileSharePermissionDataContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).RemoveProfileSharePermission(agencyHierarchyProfileSharePermissionDataContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #endregion
        #region UAT-2641
        public static List<Int32> GetAgencyHierarchyIdsByOrgUserID(Int32 OrgUserId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyIdsByOrgUserID(OrgUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2633
        public static List<AgencyNodeMappingContract> GetAgencies(Int32 AgencyHierarchyID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencies(AgencyHierarchyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AgencyNodeMappingContract> GetAgencyHierarchyAgencies(Int32 AgencyHierarchyID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyAgencies(AgencyHierarchyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveAgencyHierarchyAgencyMapping(AgencyNodeMappingContract agencyNodeMappingContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).SaveAgencyHierarchyAgencyMapping(agencyNodeMappingContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteAgencyHierarchyAgencyMapping(AgencyNodeMappingContract agencyNodeMappingContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).DeleteAgencyHierarchyAgencyMapping(agencyNodeMappingContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsAgencyHierarchyLeafNode(Int32 AgencyHierarchyID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).IsAgencyHierarchyLeafNode(AgencyHierarchyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region [UAT-2653]

        public static List<Int32> GetAgencyHiearchyIdsByDeptProgMappingID(Int32 tenantId, String DPM_Id)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(tenantId).GetAgencyHiearchyIdsByDeptProgMappingID(DPM_Id);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-2647

        public static List<Int32> GetAgencyHierarchyIdsByTenantID(Int32 TenantId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyIdsByTenantID(TenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-3245

        public static List<Int32> GetAgencyHierarchyIdsByLstTenantIDs(List<Int32> lstTenantIds)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyIdsByLstTenantIDs(lstTenantIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Digestion Process

        public static void CallDigestionProcess(String agencyhierarchyID, String changeType, Int32 CurrentLoggedInUserID)
        {
            try
            {
                var LoggerService = (HttpContext.Current.ApplicationInstance as INTSOF.Contracts.IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as INTSOF.Contracts.IWebApplication).ExceptionService;

                Dictionary<String, Object> param = new Dictionary<String, Object>();
                param.Add("AgencyHierarchyId", agencyhierarchyID);
                param.Add("ChangeType", changeType);
                param.Add("CurrentUserId", CurrentLoggedInUserID);
                INTSOF.ServiceUtil.ParallelTaskContext.PerformParallelTask(ExecuteDigestionProcess, param, LoggerService, ExceptiomService);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void ExecuteDigestionProcess(Dictionary<String, Object> param)
        {
            BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).CallDigestionProcedure(param);
        }

        #endregion

        public static String GetAgencyHiearchyIdsByTenantID(Int32 TenantId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(TenantId).GetAgencyHiearchyIdsByTenantID(TenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-2548
        public static Boolean SaveUpdateAgencyHierarchyTenantAccessMapping(Int32 AgencyHierarchyId, List<Int32> lstTenantIds, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).SaveUpdateAgencyHierarchyTenantAccessMapping(AgencyHierarchyId, lstTenantIds, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<Int32> GetAgencyHierarchyTenantAccessDetails(Int32 AgencyHierarchyId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyTenantAccessDetails(AgencyHierarchyId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2712
        //SaveAgencyHierarchyRotationFieldOptionSetting,GetAgencyHierarchyRotationFieldOptionSetting
        public static AgencyHierarchyRotationFieldOptionContract GetAgencyHierarchyRotationFieldOptionSetting(Int32 agencyHierarchyID)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyRotationFieldOptionSetting(agencyHierarchyID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SaveAgencyHierarchyRotationFieldOptionSetting(AgencyHierarchyRotationFieldOptionContract agencyHierarchyRotationFieldOptionContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).SaveAgencyHierarchyRotationFieldOptionSetting(agencyHierarchyRotationFieldOptionContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2784
        public static AgencyHierarchySettingContract GetAgencyHierarchySetting(Int32 agencyHierarchyID, String settingTypeCode)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchySetting(agencyHierarchyID, settingTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SaveAgencyHierarchySetting(AgencyHierarchySettingContract agencyHierarchyContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).SaveAgencyHierarchySetting(agencyHierarchyContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        /// <summary>
        /// UAT 2821 Agency formatted attestations (uploaded document rather than our attestation)
        /// </summary>
        /// <param name="agencyID"></param>
        /// <param name="currentLoggedInUserID"></param>
        /// <param name="SettingType"></param>
        /// <returns></returns>
        public static Boolean DeleteAgencyHierarchySetting(Int32 agencyID, Int32 currentLoggedInUserID, String SettingType)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).DeleteAgencyHierarchySetting(agencyID, currentLoggedInUserID, SettingType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //UAT-2982:- Delete mappings when hierarchy deleted 

        public static void DeletingAgencyHierarchyMappings(Int32 agencyHierarchyId, Int32 currentLoggedInUserID)
        {
            try
            {
                BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).DeletingAgencyHierarchyMappings(agencyHierarchyId, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-3237
        public static Boolean UpdateNodeDisplayOrder(List<AgencyHierarchyDataContract> lstAgencyHierarchy, Int32? destinationIndex, Int32 currentLoggedInuser)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).UpdateNodeDisplayOrder(lstAgencyHierarchy, destinationIndex, currentLoggedInuser);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        #region UAT-3241
        public static List<String> GetAgencyNamesByIds(Int32 tenantId, List<Int32> lstAgencyIds)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyNamesByIds(lstAgencyIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-3662
        public static AgencySetting GetInstPrecpReqdSetting(Int32 agencyId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetInstPrecpReqdSetting(agencyId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static Boolean SaveAgencyHierarchyRootNodeSetting(AgencyHierarchyRootNodeSettingContract agencyHierarchyContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).SaveAgencyHierarchyRootNodeSetting(agencyHierarchyContract.AgencyHierarchyID, agencyHierarchyContract.SettingTypeCode, agencyHierarchyContract.SettingValue, agencyHierarchyContract.CurrentLoggedInUser);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AgencyHierarchyRootNodeSettingContract> GetAgencyHierarchyRootNodeMapping(Int32 agencyHierarchyRootNodeId, String agencyHierarchySettingTypeCode)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyRootNodeMapping(agencyHierarchyRootNodeId, agencyHierarchySettingTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveUpdateAgencyHierarchyRootNodeMapping(AgencyHierarchyRootNodeSettingContract agencyHierarchyRootNodeSettingContract)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).SaveUpdateAgencyHierarchyRootNodeMapping(agencyHierarchyRootNodeSettingContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsAgencyHierarchyRootNodeSettingExist(Int32 agencyHierarchyRootNodeId, String agencyHierarchySettingTypeCode)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).IsAgencyHierarchyRootNodeSettingExist(agencyHierarchyRootNodeId, agencyHierarchySettingTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.SharedDataEntity.AgencyHierarchy> GetAgencyHierarchyRootNodes()
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyRootNodes();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-4257
        public static List<AgencyHierarchyContract> GetAgencyHierarchyRootNodesByTenantIDs(List<Int32> tenantIDs)
        {
            try
            {

                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetAgencyHierarchyRootNodesByTenantIDs(tenantIDs);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-4402
        public static List<RequirementCategoryContract> GetRequirementCategory(Int32 packageId)
        {
            try
            {
                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).GetRequirementCategoryByPackageID(packageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
        //UAT-4657
        public static String IsPackageVersionInProgress(Int32 PkgId)
        {
            try
            {
                List<lkpRequirementPkgVersioningStatu> lstlkpRequirementPkgVersioningStatu = LookupManager.GetSharedDBLookUpData<lkpRequirementPkgVersioningStatu>()
                                                                                                            .Where(cond => !cond.LRPVS_IsDeleted).ToList();

                String requirementPkgVersioningStatus_DueCode = lkpRequirementPkgVersioningStatus.DUE.GetStringValue();
                Int32 requirementPkgVersioningStatus_DueId = lstlkpRequirementPkgVersioningStatu.Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_DueCode)
                                                                                            .Select(col => col.LRPVS_ID).FirstOrDefault();

                String requirementPkgVersioningStatus_InProgressCode = lkpRequirementPkgVersioningStatus.IN_PROGRESS.GetStringValue();

                Int32 requirementPkgVersioningStatus_InProgressId = lstlkpRequirementPkgVersioningStatu.Where(cond => cond.LRPVS_Code == requirementPkgVersioningStatus_InProgressCode)
                                                                                            .Select(col => col.LRPVS_ID).FirstOrDefault();

                return BALUtils.GetAgencyHierarchyRepoInstance(AppConsts.NONE).IsPackageVersionInProgress(PkgId, requirementPkgVersioningStatus_DueId, requirementPkgVersioningStatus_InProgressId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

    }
}

