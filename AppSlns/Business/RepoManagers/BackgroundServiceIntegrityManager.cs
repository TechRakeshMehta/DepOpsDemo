using INTSOF.Utils;
using System;

namespace Business.RepoManagers
{
    public class BackgroundServiceIntegrityManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static BackgroundServiceIntegrityManager()
        {
            BALUtils.ClassModule = "BackgroundServiceIntegrityManager";
        }

        #endregion

        #region Methods

        #region Service Group Management

        public static IntegrityCheckResponse IfServiceGroupCanBeDeleted(Int32 svcGrpID, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfServiceGroupIsAssociated(svcGrpID))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete service group {0} as it is associated with other objects.";
                }
                return response;
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

        #region Service Attribute Management

        public static IntegrityCheckResponse IfServiceAttributeCanBeDeleted(Int32 attributeId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfServiceAttributeIsAssociated(attributeId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete attribute {0} as it is associated with other objects.";
                }

                return response;
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

        public static IntegrityCheckResponse IfServiceAttributeCanBeUpdated(Int32 attributeId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfServiceAttributeIsAssociated(attributeId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot update data type of attribute {0} as it is associated with other objects.";
                }

                return response;
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

        #region Service Attribute Group Management

        public static IntegrityCheckResponse IfServiceAttributeGroupCanBeDeleted(Int32 attributeGrpId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(SecurityManager.DefaultTenantID).IfServiceAttributeGroupIsAssociated(attributeGrpId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete attribute group {0} as it is associated with other objects.";
                }

                return response;
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

        public static IntegrityCheckResponse IfAttributeGroupMappingCanBeDeleted(Int32 attributeGrpMappingId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(SecurityManager.DefaultTenantID).IfAttributeGroupMappingIsAssociated(attributeGrpMappingId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete attribute group mapping {0} as it is associated with other objects.";
                }

                return response;
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

        #region Manage Custom Forms
        /// <summary>
        /// Checks if the CustomForm Service mapping exists in master DB.
        /// </summary>
        /// <param name="customFormId">customFormId</param>
        /// <returns>IntegrityCheckResponse</returns>
        public static IntegrityCheckResponse CheckIfBkgSvcCustomFormMappingExist(Int32 customFormId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(SecurityManager.DefaultTenantID).CheckIfBkgSvcCustomFormMappingExist(customFormId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot remove Attribute Group from Custom Form {0} as it is associated with other objects.";
                }

                return response;
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
        /// Checks if the Custom Forms mappings exist.
        /// </summary>
        /// <param name="customFormId">customFormId</param>
        /// <returns>IntegrityCheckResponse</returns>
        public static IntegrityCheckResponse CheckIfCustomFormCanBeDeleted(Int32 customFormId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(SecurityManager.DefaultTenantID).CheckIfCustomFormCanBeDeleted(customFormId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete custom form {0} as it is associated with other objects.";
                }

                return response;
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


        #region Manage Master service
        public static Boolean IsServiceMAppedToClient(Int32 bkgSvcMasterID)
        {
            try
            {
                return BALUtils.GetBackgroundServiceIntegrityRepoInstance(SecurityManager.DefaultTenantID).IsServiceMAppedToClient(bkgSvcMasterID);

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

        #region SetUp background Institution Hierarchy
        public static IntegrityCheckResponse CheckIfBkgNodeAssociated(Int32 DeptprogMappingId, Int32 SelectedTenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(SelectedTenantId).IsChildNodeExistForNode(DeptprogMappingId)
                    || BALUtils.GetBackgroundServiceIntegrityRepoInstance(SelectedTenantId).IsPackageMappedtoNode(DeptprogMappingId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete Node {0} as it is associated with other objects.";
                }

                return response;
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

        public static IntegrityCheckResponse CheckIfBkgPackageHasAnyOrder(Int32 bkgPackageHierarchyMappingID, Int32 SelectedTenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(SelectedTenantId).IsPackageHasOrder(bkgPackageHierarchyMappingID))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete Package {0} as it is associated with other objects.";
                }

                return response;
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

        #region Background Package Setup

        public static IntegrityCheckResponse IfPackageMappingCanBeDeleted(Int32 packageID, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfPackageMappingIsAssociated(packageID))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete this package as it is mapped with other objects.";
                }
                return response;
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

        public static IntegrityCheckResponse IfServiceGroupMappingCanBeDeleted(Int32 serviceGroupId, Int32 packageID, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfServiceGroupMappingIsAssociated(serviceGroupId, packageID))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete this service group as it is mapped with other objects.";
                }
                return response;
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

        public static IntegrityCheckResponse IfServiceMappingCanBeDeleted(Int32 serviceId, Int32 svcGrpID, Int32 packageId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfServiceMappingIsAssociated(serviceId, svcGrpID, packageId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete this service as it is mapped with other objects.";
                }
                return response;
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

        public static IntegrityCheckResponse IfAttributeMappingCanBeDeleted(Int32 packageId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfAttributeMappingIsAssociated(packageId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete this attribute as it is mapped with other objects.";
                }
                return response;
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

        #region Vendor Sevice Mapping

        /// <summary>
        /// Check if vendor service mapping can be deleted
        /// </summary>
        /// <param name="vendorSvcMappingId"></param>
        /// <returns>IntegrityCheckResponse</returns>
        public static IntegrityCheckResponse IfVendorServiceMappingCanBeDeleted(Int32 vendorSvcMappingId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(SecurityManager.DefaultTenantID).IfVendorServiceMappingIsAssociated(vendorSvcMappingId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete Vendor Service Mapping as it is associated with other objects.";
                }

                return response;
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

        #region Institution Hierarchy Vendor Account Mapping
        
        /// <summary>
        /// Check if Institution Hierarchy Vendor Account Mapping can be deleted
        /// </summary>
        /// <param name="nDPMId"></param>
        /// <param name="vendorId"></param>
        /// <returns>IntegrityCheckResponse</returns>
        public static IntegrityCheckResponse IfVendorAccountMappingIsAssociated(Int32 tenantId, Int32 nDPMId, Int32 vendorId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfVendorAccountMappingIsAssociated(nDPMId, vendorId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete Institution Hierarchy Vendor Account Mapping as it is associated with other objects.";
                }

                return response;
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

        #region Manage Rule Templates

        /// <summary>
        /// To check if Rule Template can be deleted
        /// </summary>
        /// <param name="ruleTemplateSetId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfRuleTemplateCanBeDeleted(Int32 ruleTemplateSetId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfRuleTemplateIsAssociated(ruleTemplateSetId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete {0} Template as it is associated with other objects.";
                }
                return response;
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

        #region Manage Ruleset and Rules

        /// <summary> 
        /// Check if Rule Set mapping is associated with any rule.
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfObjectRuleSetMappingCanBeDeleted(Int32 ruleSetId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfobjectRuleSetMappingIsAssociated(ruleSetId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot remove ruleset {0} as it is associated with other objects.";
                }
                return response;
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

        #region Order Status Colour Management

        /// <summary>
        /// Checks if the InstitutionOrderFlags mapping exist with BkgOrders.
        /// </summary>
        /// <param name="institutionOrderFlagId">institutionOrderFlagId</param>
        /// <returns>IntegrityCheckResponse</returns>
        public static IntegrityCheckResponse CheckIfInstitutionOrderFlagBkgOrderMappingExist(Int32 tenantId, Int32 institutionOrderFlagId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).CheckIfInstitutionOrderFlagBkgOrderMappingExist(institutionOrderFlagId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete Order Status Color/Flag {0} as it is associated with other objects.";
                }

                return response;
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

        #region Client Service Vendor
        /// <summary>
        /// check whether This backgroubd service is used in Client to map with the Package and to the Node
        /// </summary>
        /// <param name="masterBkgSvcID">masterBkgSvcID</param>
        /// <param name="selectedTenantID">selectedTenantID</param>
        /// <returns></returns>
        public static IntegrityCheckResponse IsClientSvcVendorStateMappingIsAssociated(Int32 masterBkgSvcID,Int32 selectedTenantID)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(selectedTenantID).IsClientSvcVendorStateMappingIsAssociated(masterBkgSvcID))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete Vendor Service Mapping as it is associated with other objects.";
                }

                return response;
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

        //Boolean IsClientSvcVendorStateMappingIsAssociated(Int32 masterBkgSvcID) 
        #endregion

        #region UAT-583 WB: AMS: Ability to delete attributes and attribute groups from the package setup screen (even after the attribute or attribute group is active)
        /// <summary>
        /// Method to check attribute group is associated with external services or not.
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="attributeGroupId">attributeGroupId</param>
        /// <param name="bkgServiceID">bkgServiceID</param>
        /// <param name="bkgPackageSvcId">bkgPackageSvcId</param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfAttributeGroupAssociatedWithExtSVC(Int32 tenantId, Int32 attributeGroupId, Int32 bkgServiceID, Int32 bkgPackageSvcId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfAttributeGroupAssociatedWithExtSVC(attributeGroupId, bkgServiceID, bkgPackageSvcId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete this attribute group as it is mapped with clear star services.";
                }
                return response;
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
        ///  Method to check attribute is associated with external services or not.
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="bkgPackageSvcAttributeMappingId">bkgPackageSvcAttributeMappingId</param>
        /// <param name="bkgServiceID">bkgServiceID</param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfAttributeAssociatedWithExtSVC(Int32 tenantId,Int32 bkgPackageSvcAttributeMappingId, Int32 bkgServiceID)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(tenantId).IfAttributeAssociatedWithExtSVC(bkgPackageSvcAttributeMappingId, bkgServiceID))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete this attribute as it is mapped with clear star services.";
                }
                return response;
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

        #endregion

        public static IntegrityCheckResponse IsReviewCriteriaMapped(int revwCriteriaID, int selectedTenantID)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetBackgroundServiceIntegrityRepoInstance(selectedTenantID).IsReviewCriteriaMapped(revwCriteriaID))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete this criteria as it is associated with other objects.";
                }
                return response;
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
