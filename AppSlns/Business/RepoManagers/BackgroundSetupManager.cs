using Entity.ClientEntity;
using INTSOF.UI.Contract;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Xml;
using System.Text.RegularExpressions;

namespace Business.RepoManagers
{
    public static class BackgroundSetupManager
    {

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static BackgroundSetupManager()
        {
            BALUtils.ClassModule = "BackgroundSetupManager";
        }

        #endregion

        #region Manage Attribute Group
        public static List<AttributeSetupContract> GetMappedAttributeGroupList(Int32 tenantId, Int32 serviceId, Int32 bkgSvcGroupId, Int32 backgroundPackageId)
        {
            try
            {

                return NewAssignAttributeGroupToDataModel(BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetMappedAttributeGroupList(serviceId, bkgSvcGroupId, backgroundPackageId));
                //return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetMappedAttributeGroupList(serviceId).ToList();
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

        public static Boolean UpdateChanges(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateChanges();

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
        /// Assign the datatable record in AttributeSetupContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List<QueueAuditRecordContract></returns>
        private static List<AttributeSetupContract> NewAssignAttributeGroupToDataModel(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new AttributeSetupContract
                {
                    AttributeGroupID = x["AttributeGroupId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["AttributeGroupId"]),
                    AttributeGroupName = x["AttributeGroupName"].ToString(),
                    AttributeGroupDescription = x["AttributeGroupDescription"].ToString(),
                    IsSystemPreConfigured = Convert.ToBoolean(x["IsSystemPreConfigured"]) == true ? "True" : "False",
                    IsEditable = Convert.ToBoolean(x["IsSystemPreConfigured"]) == true ? "True" : "False",
                    BkgPackageSvcId = x["BkgPackageSvcId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["BkgPackageSvcId"]),

                }).ToList();
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

        public static List<AttributeSetupContract> GetMappedAttributeList(Int32 tenantId, Int32 serviceId, Int32 bkgSvcGroupId, Int32 attributeGroupId, Int32 backgroundPackageId)
        {
            try
            {

                return NewAssignAttributeToDataModel(BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetMappedAttributeList(serviceId, bkgSvcGroupId, attributeGroupId, backgroundPackageId));
                //return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetMappedAttributeGroupList(serviceId).ToList();
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
        /// Assign the datatable record in AttributeSetupContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List<QueueAuditRecordContract></returns>
        private static List<AttributeSetupContract> NewAssignAttributeToDataModel(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new AttributeSetupContract
                {
                    AttributeID = x["AttributeId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["AttributeId"]),
                    AttributeName = x["AttributeName"].ToString(),
                    AttributeDescription = x["AttributeDescription"].ToString(),
                    BkgPackageSvcAttributeMappingId = x["BkgPackageSvcAttributeMappingId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["BkgPackageSvcAttributeMappingId"]),
                    IsSystemPreConfigured = Convert.ToBoolean(x["IsSystemPreConfigured"]) == true ? "True" : "False",
                    IsEditable = Convert.ToBoolean(x["IsSystemPreConfigured"]) == true ? "True" : "False",
                    BkgPackageSvcId = x["BkgPackageSvcId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["BkgPackageSvcId"]),
                    BkgAttributeGroupMappingId = x["BkgAttributeGroupMappingId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["BkgAttributeGroupMappingId"]),
                    AttributeLabel = x["AttributeLabel"].ToString(),
                    AttributeDataType = x["AttributeDataType"].ToString(),
                    Required = Convert.ToBoolean(x["IsRequired"]),
                    IsDisplay = Convert.ToBoolean(x["IsDisplay"]),
                    Active = Convert.ToBoolean(x["Active"]),
                    AttributeGroupID = x["AttributeGroupId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["AttributeGroupId"]),
                    DisplayOrder = x["DisplayOrder"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["DisplayOrder"]),
                    AttributeGroupCode = Convert.ToString(table.Rows[0]["AttributeGroupCode"]),
                    IsHiddenFromUI = x["IsHiddenFromUI"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(x["IsHiddenFromUI"]),
                }).OrderBy(x => x.DisplayOrder).ToList();
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
        /// Get all attribute.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<AttributeDataSecurityClient> GetAllAttribute(Int32 tenantId, List<Int32> mappedSvcAttibuteIds, Int32 attributeGroupId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetAllAttribute(mappedSvcAttibuteIds, attributeGroupId);
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

        public static Boolean DeletedBkgSvcAttributeMapping(Int32 tenantId, Int32 bkgPackageSvcAttributeId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeletedBkgSvcAttributeMapping(bkgPackageSvcAttributeId, currentLoggedInUserId);

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

        public static String IsmappingOfThisTypeAllowed(Int32 tenantId, String attributeType, Int32 groupId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).IsmappingOfThisTypeAllowed(attributeType, groupId);

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
        /// Retrieve a list of institution Type tenant.
        /// </summary>
        /// <returns>list of institution Type tenant</returns>
        public static List<Tenant> getClientTenant()
        {
            try
            {
                Int32 defaultTenantId = SecurityManager.DefaultTenantID;
                //return BALUtils.GetComplianceDataRepoInstance(defaultTenantId).getClientTenant(defaultTenantId);
                String TenantTypeCodeForInstitution = TenantType.Institution.GetStringValue();
                Int32 tenantTypeId = LookupManager.GetLookUpData<lkpTenantType>(defaultTenantId).FirstOrDefault(condition => condition.TenantTypeCode == TenantTypeCodeForInstitution).TenantTypeID;
                //List<Tenant> tenant = LookupManager.GetLookUpData<Tenant>(defaultTenantId).OrderBy(o => o.TenantName).Where(condition => condition.IsActive && !condition.IsDeleted && condition.TenantTypeID == tenantTypeId).ToList();
                short businessChannelTypeID = AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE;
                if (BALUtils.SessionService.BusinessChannelType.IsNotNull())
                {
                    businessChannelTypeID = BALUtils.SessionService.BusinessChannelType.BusinessChannelTypeID;
                }
                // List<Tenant> tenant = BALUtils.GetSecurityRepoInstance().GetClientTenantsBasedOnBusinessChannelType(businessChannelTypeID);

                List<Tenant> tenant = LookupManager.GetLookUpData<Entity.vw_GetTenants>().Where(cond => cond.BCT.Value == businessChannelTypeID)
                                                                     .Select(col =>
                                                                                new Tenant
                                                                                {
                                                                                    TenantID = col.TenantID,
                                                                                    TenantName = col.TenantName,
                                                                                    TenantTypeID = col.TenantTypeID

                                                                                }).ToList();

                tenant = tenant.OrderBy(o => o.TenantName).Where(x => x.TenantTypeID == tenantTypeId).ToList();
                return tenant;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BackgroundPackage> GetPackageData(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetPackageData();
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

        public static List<BkgSvcGroup> GetServiceGroupGridData(Int32 tenantId, Int32 packageId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServiceGroupGridData(packageId);
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

        public static List<BackgroundService> GetServicesForGridData(Int32 tenantId, Int32 serviceGroupId, Int32 packageId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServicesForGridData(serviceGroupId, packageId);
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

        public static BkgPackageSvc GetCurrentBkgPkgService(Int32 tenantId, Int32 serviceGroupId, Int32 packageId, Int32 serviceId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetCurrentBkgPkgService(serviceGroupId, packageId, serviceId);
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

        public static List<BackgroundService> GetServicesForDropDown(Int32 tenantId, List<Int32> lstSvcToBeRemoved)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServicesForDropDown(lstSvcToBeRemoved);
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

        public static List<BkgSvcGroup> GetServiceGroupForDropDown(Int32 tenantId, List<Int32> lstSvcToBeRemoved)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServiceGroupForDropDown(lstSvcToBeRemoved);
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

        public static List<LocalAttributeGroupMappedToBkgPackage> GetAttributeGroupMappedToBkgPackage(Int32 tenantId, Int32 bkgPackageID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetAttributeGroupMappedToBkgPackage(bkgPackageID);
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

        public static BkgPkgAttributeGroupInstruction GetBkgPkgAttributeGroupInstructionText(Int32 tenantId, Int32 bkgPackageId, Int32 attrGrpId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBkgPkgAttributeGroupInstructionText(bkgPackageId, attrGrpId);
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

        public static Boolean SaveBkgPkgAttributeGroupInstruction(Int32 tenantId, BkgPkgAttributeGroupInstruction bkgPkgAttrGrpInstructionObj)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).SaveBkgPkgAttributeGroupInstruction(bkgPkgAttrGrpInstructionObj);
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


        public static BkgPackageSvcGroup GetServicesGroupForEdit(Int32 tenantId, Int32 serviceGroupId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServicesGroupForEdit(serviceGroupId);
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

        public static String MapServiceWithServiceGroup(Int32 tenantId, Int32 serviceId, Int32 serviceGroupId, Int32 packageId, Boolean isDelete, String displayName, String notes, Int32? pkgCount, Int32? minOccurrences, Int32? maxOccurrences, Int32? residentialDuration,
            Boolean sendDocsToStudent, Boolean isSupplemental, Boolean ignoreRHOnSupplement, Boolean isReportable, String _dataXML = "")
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).MapServiceWithServiceGroup(serviceId, serviceGroupId, packageId, isDelete, displayName, notes, pkgCount, minOccurrences, maxOccurrences, residentialDuration, sendDocsToStudent, isSupplemental, ignoreRHOnSupplement, isReportable, _dataXML);
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

        public static String SaveEditPackagedetail(Int32 tenantId, BackgroundPackage backgroundPackage, Int32 currentLoggedInUserID, List<Int32> targetPackageIds, Int32 months, Boolean isActive, Int32 packageId = 0, Boolean isEdit = false)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).SaveEditPackagedetail(backgroundPackage, packageId, isEdit, currentLoggedInUserID, targetPackageIds, months, isActive);
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

        public static Boolean DeleteServiceGroupMapping(Int32 tenantId, Int32 serviceGroupId, Int32 packageId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeleteServiceGroupMapping(serviceGroupId, packageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean DeletePackageMapping(Int32 tenantId, Int32 packageId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeletePackageMapping(packageId);
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

        public static BackgroundPackage GetPackageDetail(Int32 tenantId, Int32 packageId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetPackageDetail(packageId);
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

        public static String SaveEditServiceGroupDetail(Int32 tenantId, BkgPackageSvcGroup bkgPackageSvcGroup, Int32 serviceGroupId = 0, Boolean isEdit = false)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).SaveEditServiceGroupDetail(bkgPackageSvcGroup, serviceGroupId, isEdit);
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

        public static List<Int32> GetServicesByPackageId(Int32 tenantId, Int32 packageId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServicesByPackageId(packageId);
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

        #region Manage Attribute
        public static List<lkpSvcAttributeDataType> GetAttributeDataType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpSvcAttributeDataType>(tenantId).ToList();

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

        public static BkgSvcAttribute GetBkgSvcAttribute(Int32 tenantId, Int32 attributeId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBkgSvcAttribute(attributeId);

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

        public static BkgPackageSvcAttribute GetBkgPackageSvcAttribute(Int32 tenantId, Int32 serviceAttributeId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBkgPackageSvcAttribute(serviceAttributeId);

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

        public static ManageServiceAttributeData GetBkgSvcAttributeData(ServiceAttributeParameter serviceAttributeParameter, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBkgSvcAttributeData(serviceAttributeParameter, tenantId);

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

        public static Boolean SaveAttributeAndMapping(BkgSvcAttribute serviceAttribute, Int32 tenantId, Int32 attributeGroupId, Int32 bkgPackageSvcId, Int32 currentLoggedInUserId, Boolean isRequired, Boolean isDisplay, Boolean IsHiddenFromUI)
        {
            try
            {
                BkgSvcAttribute serviceAttributeClone = serviceAttribute.Clone();

                Entity.BkgSvcAttribute masterServiceAttribute = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveAttributeInMaster(serviceAttribute, attributeGroupId, currentLoggedInUserId, isRequired, isDisplay, tenantId, IsHiddenFromUI);
                //if (BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveAttributeAndMappingInMaster(serviceAttribute, attributeGroupId, currentLoggedInUserId,Boolean isRequired,Boolean isDisplay))
                //{
                //    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).CopyAttributeAndMappingInTenant(serviceAttributeClone, serviceAttribute, attributeGroupId, bkgPackageSvcId, currentLoggedInUserId);
                //}
                if (masterServiceAttribute.BSA_ID > 0)
                {
                    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).CopyAttributeAndMappingInTenant(serviceAttributeClone, masterServiceAttribute, attributeGroupId, bkgPackageSvcId, currentLoggedInUserId, isRequired, isDisplay, IsHiddenFromUI);
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
                throw ex;
            }
        }

        public static Boolean SaveExistingAttributeMapping(Int32 selectedAttributeId, Int32 tenantId, Int32 attributeGroupId, Int32 bkgPackageSvcId, Int32 currentLoggedInUserId, Boolean isRequired, Boolean isDisplay)
        {
            try
            {
                BkgAttributeGroupMapping mappedAttributeAndGroup = BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetAttributeMappingByAttributeGroupID(attributeGroupId, selectedAttributeId);
                if (mappedAttributeAndGroup.IsNullOrEmpty())
                {
                    Entity.BkgAttributeGroupMapping masterAttributeGroupMapping = BALUtils.GetBackgroundSetupRepoInstance(tenantId).CheckAddForSecurityAttribute(attributeGroupId, selectedAttributeId);
                    if (masterAttributeGroupMapping.IsNullOrEmpty())
                    {
                        BkgAttributeGroupMapping attributeMappingToAdd = GetTranslatedMappingObject(selectedAttributeId, attributeGroupId, currentLoggedInUserId);
                        masterAttributeGroupMapping = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveAttributeAndGroupMappingInMaster(attributeMappingToAdd, isRequired, isDisplay);
                    }
                    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).CopyAttributeAndGroupMappingInChild(masterAttributeGroupMapping, bkgPackageSvcId, isRequired, isDisplay);
                }
                if (!mappedAttributeAndGroup.IsNullOrEmpty())
                {
                    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).SavePackageSvcAttributeMapping(mappedAttributeAndGroup, bkgPackageSvcId, currentLoggedInUserId, isRequired, isDisplay);
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
                throw ex;
            }
        }

        public static BkgAttributeGroupMapping GetTranslatedMappingObject(Int32 selectedAttributeId, Int32 attributeGroupId, Int32 currentLoggedInUserId)
        {
            try
            {
                BkgAttributeGroupMapping bkgAttributeGroupMapping = new BkgAttributeGroupMapping();
                bkgAttributeGroupMapping.BAGM_BkgSvcAttributeGroupId = attributeGroupId;
                bkgAttributeGroupMapping.BAGM_BkgSvcAtributeID = selectedAttributeId;
                bkgAttributeGroupMapping.BAGM_Code = Guid.NewGuid();
                bkgAttributeGroupMapping.BAGM_CopiedFromCode = null;
                bkgAttributeGroupMapping.BAGM_CreatedOn = DateTime.Now;
                bkgAttributeGroupMapping.BAGM_CreatedBy = currentLoggedInUserId;
                bkgAttributeGroupMapping.BAGM_IsDeleted = false;
                bkgAttributeGroupMapping.BAGM_IsEditable = true;
                bkgAttributeGroupMapping.BAGM_IsSystemPreConfigured = false;
                return bkgAttributeGroupMapping;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Int32 GetBkgPackageSvcId(Int32 tenantId, Int32 serviceId, Int32 bkgSvcGroupId, Int32 backgroundPackageId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBkgPackageSvcId(serviceId, bkgSvcGroupId, backgroundPackageId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Updates an attribute along with its options in client and master db conditionally
        /// </summary>
        /// <param name="serviceAttributeContract"></param>
        /// <param name="tenantId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="_editLocally">Whether the change is local to client or local and master both</param>
        /// <returns>Success = true</returns>
        public static Boolean UpdateBkgSvcAttribute(ServiceAttributeContract serviceAttributeContract, Int32 tenantId, Int32 currentLoggedInUserId, Boolean _editLocally)
        {
            try
            {
                List<Int32> attributeOptionIdsToDelete = new List<Int32>();
                List<BkgSvcAttributeOption> needToAddAttributeOptions = new List<BkgSvcAttributeOption>();
                System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> attrOptionListMaster = null;
                BkgSvcAttribute serviceAttribute = serviceAttributeContract.TranslateToClientEntity();
                SegeregateAttributeOptions(tenantId, attributeOptionIdsToDelete, needToAddAttributeOptions, serviceAttribute);
                if (!_editLocally)
                {
                    attrOptionListMaster = UpdateBkgSvcAttributeMaster(serviceAttributeContract, tenantId, currentLoggedInUserId, serviceAttribute, attributeOptionIdsToDelete, needToAddAttributeOptions);
                }
                //save attribtue and options in client
                return UpdateBkgSvcAttributeClient(serviceAttributeContract, serviceAttribute, tenantId, currentLoggedInUserId, attrOptionListMaster, _editLocally);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        private static void SegeregateAttributeOptions(Int32 tenantId, List<Int32> attributeOptionIdsToDelete, List<BkgSvcAttributeOption> needToAddAttributeOptions, BkgSvcAttribute serviceAttribute)
        {
            BkgSvcAttribute serviceAttributeInDb = GetBkgSvcAttribute(tenantId, serviceAttribute.BSA_ID);
            if (serviceAttributeInDb != null)
            {
                //Add delete attribute option
                System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption> attributeOptions = serviceAttribute.BkgSvcAttributeOptions;

                IEnumerable<BkgSvcAttributeOption> attributeOptionsInDb = serviceAttributeInDb.BkgSvcAttributeOptions
                    .Where(x => x.EBSAO_IsActive && !x.EBSAO_IsDeleted);
                // Deletes attribute options
                foreach (BkgSvcAttributeOption attributeOptionIndb in attributeOptionsInDb)
                {
                    if (attributeOptions.Any(x => x.EBSAO_OptionText == attributeOptionIndb.EBSAO_OptionText
                        && x.EBSAO_OptionValue == attributeOptionIndb.EBSAO_OptionValue))
                        continue;
                    attributeOptionIdsToDelete.Add(attributeOptionIndb.EBSAO_ID);
                }


                // Adds attribute options
                foreach (BkgSvcAttributeOption attributeOption in attributeOptions)
                {
                    if (attributeOptionsInDb.Any(x => x.EBSAO_OptionText == attributeOption.EBSAO_OptionText
                        && x.EBSAO_OptionValue == attributeOption.EBSAO_OptionValue))
                        continue;

                    needToAddAttributeOptions.Add(attributeOption);
                }

            }
        }

        private static System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> UpdateBkgSvcAttributeMaster(ServiceAttributeContract serviceAttributeContract, Int32 tenantId, Int32 currentLoggedInUserId, BkgSvcAttribute serviceAttribute, List<Int32> attributeOptionIdsToDelete, List<BkgSvcAttributeOption> needToAddAttributeOptions)
        {
            //Save AttributeOption in Master database.
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> attrOptionListMaster
                = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID)
               .AddDeleteServiceAttributeOpt(attributeOptionIdsToDelete, needToAddAttributeOptions, currentLoggedInUserId, serviceAttribute.BSA_ID);
            //Save Attribute in Master database.
            BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateBkgSvcAttributeSecurity(serviceAttributeContract, currentLoggedInUserId);
            return attrOptionListMaster;
        }

        private static Boolean UpdateBkgSvcAttributeClient(ServiceAttributeContract serviceAttributeContract, BkgSvcAttribute serviceAttribute, Int32 tenantId, Int32 currentLoggedInUserId,
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> attrOptionListMaster, Boolean _editLocally)
        {
            try
            {
                //BkgSvcAttribute serviceAttribute = serviceAttributeContract.TranslateToClientEntity();
                BkgSvcAttribute serviceAttributeInDb = GetBkgSvcAttribute(tenantId, serviceAttribute.BSA_ID);
                BkgPackageSvcAttribute bkgPackageSvcAttributeInDB = GetBkgPackageSvcAttribute(tenantId, serviceAttributeContract.ServiceAttributeMappingId);

                if (bkgPackageSvcAttributeInDB != null)
                {
                    bkgPackageSvcAttributeInDB.BPSA_IsRequired = serviceAttributeContract.IsRequired;
                    bkgPackageSvcAttributeInDB.BPSA_IsDisplay = serviceAttributeContract.IsDisplay;
                    bkgPackageSvcAttributeInDB.BPSA_IsHiddenFromUI = serviceAttributeContract.IsHiddenFromUI;
                    bkgPackageSvcAttributeInDB.BPSA_ValidateExpression = serviceAttributeContract.ValidationExpression;
                    bkgPackageSvcAttributeInDB.BPSA_ValidationMessage = serviceAttributeContract.ValidationMessage;
                }

                if (serviceAttributeInDb != null)
                {
                    serviceAttributeInDb.BSA_Label = serviceAttribute.BSA_Label;
                    if (!_editLocally)// these properties are not available in "edit local"
                    {
                        serviceAttributeInDb.BSA_DataTypeID = serviceAttribute.BSA_DataTypeID;
                        serviceAttributeInDb.BSA_Name = serviceAttribute.BSA_Name;

                        serviceAttributeInDb.BSA_Description = serviceAttribute.BSA_Description;
                        serviceAttributeInDb.BSA_MaxDateValue = serviceAttribute.BSA_MaxDateValue;
                        serviceAttributeInDb.BSA_MaxIntValue = serviceAttribute.BSA_MaxIntValue;
                        serviceAttributeInDb.BSA_MaxLength = serviceAttribute.BSA_MaxLength;
                        serviceAttributeInDb.BSA_MinDateValue = serviceAttribute.BSA_MinDateValue;
                        serviceAttributeInDb.BSA_MinIntValue = serviceAttribute.BSA_MinIntValue;
                        serviceAttributeInDb.BSA_MinLength = serviceAttribute.BSA_MinLength;
                        serviceAttributeInDb.BSA_Active = serviceAttribute.BSA_Active;
                        serviceAttributeInDb.BSA_IsEditable = serviceAttribute.BSA_IsEditable;
                        //serviceAttributeInDb.BSA_IsSystemPreConfiguredq = serviceAttribute.BSA_IsSystemPreConfiguredq;
                        serviceAttributeInDb.BSA_IsRequired = serviceAttribute.BSA_IsRequired;
                        // serviceAttributeInDb.BSA_ReqValidationMessage = serviceAttribute.BSA_ReqValidationMessage;
                        //complianceAttributeInDb.CopiedFromCode = complianceAttribute.Code;
                        //Add delete attribute option
                        System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption> attributeOptions = serviceAttribute.BkgSvcAttributeOptions;
                        IEnumerable<BkgSvcAttributeOption> attributeOptionsInDb = serviceAttributeInDb.BkgSvcAttributeOptions
                            .Where(x => x.EBSAO_IsActive && !x.EBSAO_IsDeleted);

                        // Deletes attribute options
                        foreach (BkgSvcAttributeOption attributeOptionIndb in attributeOptionsInDb)
                        {
                            if (attributeOptions.Any(x => x.EBSAO_OptionText == attributeOptionIndb.EBSAO_OptionText
                                && x.EBSAO_OptionValue == attributeOptionIndb.EBSAO_OptionValue))
                                continue;
                            attributeOptionIndb.EBSAO_IsDeleted = true;
                            attributeOptionIndb.EBSAO_ModifiedOn = DateTime.Now;
                            attributeOptionIndb.EBSAO_ModifiedByID = currentLoggedInUserId;
                        }

                        BALUtils.GetBackgroundSetupRepoInstance(tenantId).SaveAttributeOptionInClient(attrOptionListMaster, currentLoggedInUserId);
                    }
                    serviceAttributeInDb.BSA_ModifiedBy = currentLoggedInUserId;
                    serviceAttributeInDb.BSA_ModifiedDate = DateTime.Now;

                    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateChanges();



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
                throw ex;
            }
        }

        #endregion

        #region Background Package Administration

        /// <summary>
        /// Returns all the service groups viewable to the current logged in user.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>List of Compliance Categories</returns>
        public static List<BkgSvcGroup> GetServiceGroups(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServiceGroups();
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
        /// Saves the Service Group in th MasterDB followed by ClientDB(in case of client).
        /// </summary>
        /// <param name="category">BkgSvcGroup Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <param name="tenantID">tenantID</param>
        public static Int32 SaveServiceGroup(BkgSvcGroup svcGroup, Int32 currentLoggedInUserId, Int32 tenantID)
        {
            try
            {
                BkgSvcGroup serviceGroup = BALUtils.GetBackgroundSetupRepoInstance(tenantID).SaveServiceGroupDetail(svcGroup, currentLoggedInUserId);
                return serviceGroup.BSG_ID;
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
        /// Checks if the service group name already exists.
        /// </summary>
        /// <param name="serviceGroupName">Service Group Name</param>
        public static Boolean CheckIfServiceGroupNameAlreadyExist(String serviceGroupName, Int32 svcGrpID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).CheckIfServiceGroupNameAlreadyExist(serviceGroupName, svcGrpID);

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

        public static void UpdateServiceGroupDetail(BkgSvcGroup svcGroup, Int32 svcGrpID, Int32 currentLoggedInUserId, Int32 tenantID)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(tenantID).UpdateServiceGroupDetail(svcGroup, svcGrpID, currentLoggedInUserId);

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

        public static BkgSvcGroup getCurrentServiceGroupInfo(Int32 svcGrpID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).getCurrentServiceGroupInfo(svcGrpID);
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

        public static Boolean DeleteServiceGroup(Int32 svcGrpID, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeleteServiceGroup(svcGrpID, currentUserId);

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

        #region SetUpBkgInstitutionhierarchy

        /// <summary>
        /// Get Institute Hierarchy Tree For Background data
        /// </summary>
        /// <param name="orgUserID">optional parameter in case of super admin pass null</param>
        /// <returns></returns>
        public static List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> GetBackgroundInstituteHierarchyTree(int? orgUserID, Int32 tenantId)
        {
            {
                try
                {
                    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBackgroundInstituteHierarchyTree(orgUserID);
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

        /// <summary>
        /// To get Program Packages by HierarchyMappingIdId
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        public static List<BkgPackageHierarchyMapping> GetProgramPackagesByHierarchyMappingId(Int32 deptProgramMappingID, Int32 tenantId)
        {
            {
                try
                {
                    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetProgramPackagesByHierarchyMappingId(deptProgramMappingID);
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

        /// <summary>
        /// To get not mapped Compliance Packages
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        public static List<BackgroundPackage> GetNotMappedBackGroungPackagesByMappingId(Int32 deptProgramMappingID, Int32 tenantId)
        {
            {
                try
                {
                    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetNotMappedBackGroungPackagesByMappingId(deptProgramMappingID);
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

        // <summary>
        /// To save Program Package Mapping
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="packageId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="IsCreatedByAdmin"></param>
        /// <returns></returns>
        public static Boolean SaveHierarchyNodePackageMapping(BkgPackageHierarchyMapping newMapping, Int32 currentUserId, List<Int32> lstPaymentOptionIds, Int32 tenantId)
        {
            {
                try
                {
                    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).SaveHierarchyNodePackageMapping(newMapping, lstPaymentOptionIds, currentUserId);
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

        /// <summary>
        /// To delete bkg Package HierarchyMapping by ID
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static Boolean DeleteHirarchyPackageMappingByID(Int32 bkgPackageHierarchyMappingID, Int32 currentUserId, Int32 tenantId)
        {
            {
                try
                {
                    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeleteHirarchyPackageMappingByID(bkgPackageHierarchyMappingID, currentUserId);
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
        public static Boolean UpdateBackgroundPackageSequence(IList<BkgPackageHierarchyMapping> hierarchyPackagesToMove, Int32? destinationIndex, Int32 currentUserId, Int32 tenantId)
        {
            {
                try
                {
                    return BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateBackgroundPackageSequence(hierarchyPackagesToMove, destinationIndex, currentUserId);
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
        public static List<lkpPackageSupplementalType> SupplementalTypeList(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpPackageSupplementalType>(tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        //SupplementalTypeList
        #endregion

        #region Communication

        /// <summary>
        /// Method is used to get the Contact list based on the institutionHierarchyNodeID
        /// </summary>
        /// <param name="institutionHierarchyNodeID"></param>
        /// <returns></returns>
        public static List<HierarchyContactMapping> GetInstitutionContactUserData(Int32 tenantId, Int32 institutionHierarchyNodeID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetInstitutionContactUserData(institutionHierarchyNodeID);
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
            return null;
        }

        /// <summary>
        /// Method is used to get the list of contact except selected node
        /// </summary>
        /// <param name="institutionHierarchyNodeID"></param>
        /// <returns></returns>
        public static List<InsContact> GetInstitutionContactList(Int32 tenantId, Int32 institutionHierarchyNodeID, Int32 contactID = AppConsts.MINUS_ONE)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetInstitutionContactList(institutionHierarchyNodeID, contactID);
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

            return null;


        }


        /// <summary>
        /// Method is used to get the InstitutionContact based on instutionContactID
        /// </summary>
        /// <param name="instutionContactID"></param>
        /// <returns></returns>
        public static InstitutionContact GetInstitutionContactList(Int32 tenantId, Int32 instutionContactID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetInstitutionContactList(instutionContactID);
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

            return null;


        }


        /// <summary>
        /// Method is used to Delete contact based on contactID
        /// </summary>
        /// <param name="instutionContactID"></param>
        /// <returns></returns>
        public static Boolean DeleteInstitutionContact(Int32 tenantId, Int32 instutionContactID, Int32 currentLoggedInUserId, Int32 nodeId)
        {
            try
            {
                List<Int32> hierarchyContactMappingIds = BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeleteInstitutionContact(instutionContactID, currentLoggedInUserId, nodeId);

                return BALUtils.GetCommunicationRepoInstance().DeleteCommunicationSubEventList(tenantId, hierarchyContactMappingIds, currentLoggedInUserId);

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

            return false;


        }

        /// <summary>
        /// To get HierarchyContactMapping by MappingIds
        /// </summary>
        /// <param name="hierarchyContactMappingIDs"></param>
        /// <returns></returns>
        public static List<HierarchyContactMapping> GetHierarchyContactMappingByMappingIds(Int32 tenantId, List<Int32> hierarchyContactMappingIDs)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetHierarchyContactMappingByMappingIds(hierarchyContactMappingIDs);
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

        #region Backgroung Package Hierarchy Mapping

        /// <summary>
        /// To get background packages by HierarchyMappingIds
        /// </summary>
        /// <param name="bkgHierarchyMappingIds"></param>
        /// <returns></returns>
        public static List<BackgroundPackagesContract> GetOrderBkgPackageDetails(Int32 tenantId, List<Int32> bkgHierarchyMappingIds, Int32? SelectedHierachyId = null)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetOrderBkgPackageDetails(bkgHierarchyMappingIds, SelectedHierachyId);
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

        #region Background Attributes

        public static List<BkgSvcAttribute> GetServiceAttributes(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServiceAttributes(tenantId, SecurityManager.DefaultTenantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static BkgSvcAttribute GetServiceAttributeBasedOnAttributeID(Int32 serviceAttributeID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServiceAttributeBasedOnAttributeID(serviceAttributeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Entity.BkgSvcAttribute AddServiceAttribute(Entity.BkgSvcAttribute serviceAttribute, Int32 tenantId)
        {
            try
            {
                Boolean result = false;
                Int32 defaultTenantId = SecurityManager.DefaultTenantID;
                result = BALUtils.GetBackgroundSetupRepoInstance(defaultTenantId).AddServiceAttribute(serviceAttribute, tenantId, defaultTenantId);
                if (tenantId != defaultTenantId && result)
                {
                    CopySvcAttributesToClient(serviceAttribute, tenantId);
                }
                return serviceAttribute;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Boolean UpdateServiceAttribute(Entity.BkgSvcAttribute serviceAttribute, Int32 tenantId)
        {
            try
            {
                Boolean result = false;
                List<BkgSvcAttributeOption> lstAddAttributeOptions = new List<BkgSvcAttributeOption>();

                if (tenantId == SecurityManager.DefaultTenantID)
                {
                    result = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateMasterServiceAttribute(serviceAttribute);
                }
                else
                {
                    //if change in option value
                    Entity.BkgSvcAttribute masterServiceAttributeInDb = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetMasterServiceAttributeBasedOnAttributeID(serviceAttribute.BSA_ID);
                    System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.BkgSvcAttributeOption> masterAttributeOptions = serviceAttribute.BkgSvcAttributeOptions;

                    IEnumerable<Entity.BkgSvcAttributeOption> masterAttributeOptionsInDb = masterServiceAttributeInDb.BkgSvcAttributeOptions
                        .Where(x => x.EBSAO_IsActive && !x.EBSAO_IsDeleted);

                    List<Entity.BkgSvcAttributeOption> needToAddAttributeOptions = new List<Entity.BkgSvcAttributeOption>();
                    // Adds attribute options
                    foreach (Entity.BkgSvcAttributeOption attributeOption in masterAttributeOptions)
                    {
                        if (masterAttributeOptionsInDb.Any(x => x.EBSAO_OptionText == attributeOption.EBSAO_OptionText
                            && x.EBSAO_OptionValue == attributeOption.EBSAO_OptionValue))
                            continue;

                        needToAddAttributeOptions.Add(attributeOption);
                    }

                    if (needToAddAttributeOptions.Count != AppConsts.NONE)
                    {
                        result = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).AddMasterAttributeOptions(needToAddAttributeOptions, masterServiceAttributeInDb);
                    }

                    List<BkgSvcAttributeOption> needToAddClientAttributeOptions = new List<BkgSvcAttributeOption>();


                    if (needToAddAttributeOptions.Count != AppConsts.NONE)
                    {
                        foreach (var attrOptns in needToAddAttributeOptions)
                        {
                            BkgSvcAttributeOption attrOptions = new BkgSvcAttributeOption()
                            {
                                EBSAO_ID = attrOptns.EBSAO_ID,
                                EBSAO_BkgSvcAttributeID = attrOptns.EBSAO_BkgSvcAttributeID,
                                EBSAO_CreatedByID = attrOptns.EBSAO_CreatedByID,
                                EBSAO_CreatedOn = attrOptns.EBSAO_CreatedOn,
                                EBSAO_IsActive = attrOptns.EBSAO_IsActive,
                                EBSAO_IsDeleted = attrOptns.EBSAO_IsDeleted,
                                EBSAO_ModifiedByID = attrOptns.EBSAO_ModifiedByID,
                                EBSAO_ModifiedOn = attrOptns.EBSAO_ModifiedOn,
                                EBSAO_OptionText = attrOptns.EBSAO_OptionText,
                                EBSAO_OptionValue = attrOptns.EBSAO_OptionValue
                            };
                            needToAddClientAttributeOptions.Add(attrOptions);
                        }
                    }
                    result = BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateServiceAttribute(serviceAttribute, needToAddClientAttributeOptions);
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
                throw ex;
            }
        }

        public static Boolean DeleteServiceAttribute(Int32 serviceAttributeID, Int32 modifiedByID, Int32 tenantId)
        {
            try
            {
                Int32 defaultTenantID = SecurityManager.DefaultTenantID;
                Boolean result = BALUtils.GetBackgroundSetupRepoInstance(defaultTenantID).DeleteServiceAttribute(serviceAttributeID, modifiedByID);
                if (result && tenantId != defaultTenantID)
                {
                    result = BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeleteServiceAttribute(serviceAttributeID, modifiedByID);
                }
                if (result)
                {
                    result = BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeleteServiceAttributeTenant(serviceAttributeID, modifiedByID);
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
                throw ex;
            }
        }

        public static Boolean checkIfMappingIsDefinedForSvcAttribute(Int32 attributeId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).checkIfSvcMappingIsDefinedForAttribute(attributeId, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<lkpSvcAttributeDataType> GetServiceAttributeDatatype(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpSvcAttributeDataType>(tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }


        public static Boolean CopySvcAttributesToClient(Entity.BkgSvcAttribute masterAttribute, Int32 tenantId)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption> lstClientSvcAttributeOption = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<BkgSvcAttributeOption>();
            foreach (var attrOptn in masterAttribute.BkgSvcAttributeOptions)
            {
                BkgSvcAttributeOption bkgSvcAttributeOptn = new BkgSvcAttributeOption()
                {
                    EBSAO_BkgSvcAttributeID = attrOptn.EBSAO_BkgSvcAttributeID,
                    EBSAO_CreatedByID = attrOptn.EBSAO_CreatedByID,
                    EBSAO_CreatedOn = attrOptn.EBSAO_CreatedOn,
                    EBSAO_ID = attrOptn.EBSAO_ID,
                    EBSAO_IsActive = attrOptn.EBSAO_IsActive,
                    EBSAO_IsDeleted = attrOptn.EBSAO_IsDeleted,
                    EBSAO_ModifiedByID = attrOptn.EBSAO_ModifiedByID,
                    EBSAO_ModifiedOn = attrOptn.EBSAO_ModifiedOn,
                    EBSAO_OptionText = attrOptn.EBSAO_OptionText,
                    EBSAO_OptionValue = attrOptn.EBSAO_OptionValue
                };
                lstClientSvcAttributeOption.Add(bkgSvcAttributeOptn);
            }


            BkgSvcAttribute clientSvcAttribute = new BkgSvcAttribute()
            {
                BSA_ID = masterAttribute.BSA_ID,
                BSA_DataTypeID = masterAttribute.BSA_DataTypeID,
                BSA_CopiedFromCode = masterAttribute.BSA_CopiedFromCode,
                BSA_Code = masterAttribute.BSA_Code,
                BSA_Name = masterAttribute.BSA_Name,
                BSA_Label = masterAttribute.BSA_Label,
                BSA_Description = masterAttribute.BSA_Description,
                BSA_MaxLength = masterAttribute.BSA_MaxLength,
                BSA_MinLength = masterAttribute.BSA_MinLength,
                BSA_MaxDateValue = masterAttribute.BSA_MaxDateValue,
                BSA_MinDateValue = masterAttribute.BSA_MinDateValue,
                BSA_MaxIntValue = masterAttribute.BSA_MaxIntValue,
                BSA_MinIntValue = masterAttribute.BSA_MinIntValue,
                BSA_IsDeleted = masterAttribute.BSA_IsDeleted,
                BSA_IsEditable = masterAttribute.BSA_IsEditable,
                //BSA_IsRequired = masterAttribute.BSA_IsRequired,
                //BSA_ReqValidationMessage = masterAttribute.BSA_ReqValidationMessage,
                BSA_IsSystemPreConfiguredq = masterAttribute.BSA_IsSystemPreConfiguredq,
                BSA_Active = masterAttribute.BSA_Active,
                BSA_CreatedById = masterAttribute.BSA_CreatedById,
                BSA_CreatedDate = masterAttribute.BSA_CreatedDate,
                BSA_ModifiedBy = masterAttribute.BSA_ModifiedBy,
                BSA_ModifiedDate = masterAttribute.BSA_ModifiedDate,
                BkgSvcAttributeOptions = lstClientSvcAttributeOption
            };

            return BALUtils.GetBackgroundSetupRepoInstance(tenantId).AddServiceAttributeToClient(clientSvcAttribute);

        }

        public static List<CascadingAttributeOptionsContract> GetCascadingAttributeOptions(Int32 tenantId, Int32 attributeId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetCascadingAttributeOptions(attributeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw;
            }
        }

        public static Boolean SaveCascadingAttributeOption(Int32 tenantId, CascadingAttributeOptionsContract cascadingAttributeOptionsContract, Int32 currentLoggedInUserId)
        {
            try
            {
                Boolean result = false;
                var masterEntity = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveMasterCascadingAttributeOption(cascadingAttributeOptionsContract, currentLoggedInUserId);
                result = masterEntity.IsNotNull();

                if (tenantId != SecurityManager.DefaultTenantID && result)
                {
                    CascadingAttributeOptionsContract contract = new CascadingAttributeOptionsContract
                    {
                        Id = masterEntity.CAO_ID,
                        AttributeId = masterEntity.CAO_AttributeID,
                        DisplaySequence = masterEntity.CAO_DisplaySequence ?? 0,
                        SourceValue = masterEntity.CAO_SourceValue,
                        Value = masterEntity.CAO_Value
                    };
                    var childEntity = BALUtils.GetBackgroundSetupRepoInstance(tenantId).SaveClientCascadingAttributeOption(contract, currentLoggedInUserId);
                    result = childEntity.IsNotNull();
                }

                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw;
            }
        }

        public static Boolean DeleteCascadingAttributeOption(Int32 tenantId, Int32 optionId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeleteCascadingAttributeOption(optionId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw;
            }
        }

        #endregion

        #region Contacts

        public static Boolean SaveContacts(InstitutionContact institutionContact, Int32 tenantID, Int32 heirarchyNodeID, Boolean isNew, Int32 contactID = 0, Int32 businessChannelType = AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE)
        {
            try
            {
                Int32 hierarchyContactMappingId = BALUtils.GetBackgroundSetupRepoInstance(tenantID).SaveContact(institutionContact, tenantID, heirarchyNodeID, isNew, contactID);

                //if (isNew && businessChannelType == AppConsts.AMS_BUSINESS_CHANNEL_TYPE)
                if (businessChannelType == AppConsts.AMS_BUSINESS_CHANNEL_TYPE)
                {
                    Entity.lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS.GetStringValue());

                    Entity.lkpCopyType lkpCopyType = CommunicationManager.GetlkpCopyType().Where(cond => cond.CT_Code == CopyType.CC.GetStringValue()).FirstOrDefault();
                    Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                    Entity.HierarchyNotificationMapping hierarchyNotificationMapping = new Entity.HierarchyNotificationMapping();
                    hierarchyNotificationMapping.HNM_SubEventID = subEvent.CommunicationSubEventID;
                    hierarchyNotificationMapping.HNM_CopyTypeID = lkpCopyType.CT_Id;
                    hierarchyNotificationMapping.HNM_TenantID = tenantID;
                    hierarchyNotificationMapping.HNM_HierarchyNodeID = heirarchyNodeID;
                    hierarchyNotificationMapping.HNM_HierarchyContactMappingID = hierarchyContactMappingId;
                    hierarchyNotificationMapping.HNM_IsCommunicationCenter = false;
                    hierarchyNotificationMapping.HNM_IsEmail = true;
                    hierarchyNotificationMapping.HNM_IsDeleted = false;
                    hierarchyNotificationMapping.HNM_CreatedOn = DateTime.Now;
                    hierarchyNotificationMapping.HNM_CreatedByID = institutionContact.ICO_CreatedByID;
                    return CommunicationManager.SaveCommunicationSubEvent(hierarchyNotificationMapping);

                }
                return true;

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


        public static Boolean UpdateContact(InstitutionContact institutionContact, Int32 contactID, Int32 tenantID, Int32 heirarchyNodeID, Boolean isContact = false)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).UpdateContact(institutionContact, contactID, tenantID, heirarchyNodeID, isContact);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static Boolean DeleteContact(Int32 contactID, Int32 tenantID, Int32 currentLoggedInUserId, Int32 nodeId)
        {
            try
            {
                List<Int32> hierarchyContactMappingIds = BALUtils.GetBackgroundSetupRepoInstance(tenantID).DeleteInstitutionContact(contactID, tenantID, nodeId);

                return BALUtils.GetCommunicationRepoInstance().DeleteCommunicationSubEventList(tenantID, hierarchyContactMappingIds, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        /// <summary>
        /// Method is used to check the contact with supplied email exists or not
        /// </summary>
        /// <param name="contactEmailAddress"></param>
        /// <returns></returns>
        public static Boolean IsContactExists(Int32 tenantID, String contactEmailAddress, Int32 contactID = AppConsts.NONE)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).IsContactExists(contactEmailAddress, contactID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;

        }


        #endregion

        #region Manage Service Item Setup
        //public static List<lkpServiceItemType> GetServiceItemTypeList(Int32 tenantId)
        //{
        //    try
        //    {
        //        return LookupManager.GetLookUpData<Entity.ClientEntity.lkpServiceItemType>(tenantId).ToList();

        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //public static List<PackageServiceItem> GetPackageServiceItemList(Int32 tenantId,Int32 BkgPackageSvcId)
        //{
        //    try
        //    {
        //        return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetPackageServiceItemList(BkgPackageSvcId);

        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //public static Int32 GetBkgPkgSvcItemUsageModeIDByCode(Int32 tenantId,String code)
        //{
        //    try
        //    {
        //        return LookupManager.GetLookUpData<Entity.ClientEntity.lkpPkgSvcItemUsageMode>(tenantId).FirstOrDefault(cond => cond.PSIUM_Code == code && cond.PSIUM_IsDeleted == false).PSIUM_ID;

        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //public static PackageServiceItem GetPackageServiceItemData(Int32 tenantId, Int32 PSI_ID)
        //{
        //    try
        //    {
        //        return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetPackageServiceItemData(PSI_ID);

        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //public static Boolean SavePackageServiceItemData(Int32 tenantId, PackageServiceItem packageServiceItemData)
        //{
        //    try
        //    {
        //        return BALUtils.GetBackgroundSetupRepoInstance(tenantId).SavePackageServiceItemData(packageServiceItemData);

        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //public static Boolean IsServiceItemExist(Int32 tenantId, String serviceItemName,Int32? PSI_ID=null)
        //{
        //    try
        //    {
        //        return BALUtils.GetBackgroundSetupRepoInstance(tenantId).IsServiceItemExist(serviceItemName, PSI_ID);

        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        //public static Boolean IsServiceItemMapped(Int32 tenantId, Int32 PSI_ID)
        //{
        //    try
        //    {
        //        return BALUtils.GetBackgroundSetupRepoInstance(tenantId).IsServiceItemMapped(PSI_ID);

        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        #endregion

        #region Manage Master Service Attribute Groups

        public static List<Entity.BkgSvcAttributeGroup> GetServiceAttributeGroups()
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetServiceAttributeGroups();
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
        /// Get Tenant specific Attribute Groups
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<Entity.ClientEntity.BkgSvcAttributeGroup> GetServiceAttributeGroupsByTenant(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServiceAttributeGroupsByTenant();
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

        public static Boolean CheckIfSvcAttrGrpNameAlreadyExist(String serviceAttrGrpName, Int32 svcAttrID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).CheckIfSvcAttrGrpNameAlreadyExist(serviceAttrGrpName, svcAttrID);

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

        public static Boolean SaveServiceAttributeGroup(Entity.BkgSvcAttributeGroup svcAttrGrp)
        {
            try
            {
                Boolean result = false;
                result = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveServiceAttributeGroup(svcAttrGrp);
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

        public static Boolean UpdateServiceAttributeGroup(Entity.BkgSvcAttributeGroup svcAttrGrp, Int32 svcAttrgrpId)
        {
            try
            {
                Boolean result = false;
                result = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateServiceAttributeGroup(svcAttrGrp, svcAttrgrpId);
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

        public static Boolean DeleteServiceAttributeGroup(Int32 serviceAttributeID, Int32 modifiedByID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteServiceAttributeGroup(serviceAttributeID, modifiedByID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        public static Entity.BkgSvcAttributeGroup GetServiceAttributeGroupBasedOnAttributeGrpID(Int32 serviceAttributeGrpID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetServiceAttributeGroupBasedOnAttributeGrpID(serviceAttributeGrpID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        #region Service Attribute Mapping

        public static List<MapServiceAttributeToGroupContract> GetMappedAttributes(Int32 serviceAttrGrpId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetMappedAttributes(serviceAttrGrpId);

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

        public static List<Entity.BkgSvcAttribute> GetUnmappedAttributes(Int32 attributegrpID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetUnmappedAttributes(attributegrpID, SecurityManager.DefaultTenantID);

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

        public static List<Entity.BkgSvcAttribute> GetSourceAttributes(Int32 currentAttributeId, Int32 attributeGroupId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetSourceAttributes(currentAttributeId, attributeGroupId, SecurityManager.DefaultTenantID);

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

        public static void SaveAttributeGroupMapping(List<Entity.BkgAttributeGroupMapping> lstBkgSvcAttributeGroupMapping, Int32 svcAttributeGrpId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveAttributeGroupMapping(lstBkgSvcAttributeGroupMapping, svcAttributeGrpId);

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

        public static Entity.BkgSvcAttribute GetMasterServiceAttributeBasedOnAttributeID(Int32 serviceAttributeID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetMasterServiceAttributeBasedOnAttributeID(serviceAttributeID);
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

        public static Boolean DeleteAttributeGroupMapping(Int32 attributeGrpMappingID, Int32 currentLoggedInUserId)
        {
            try
            {
                Boolean result = false;
                result = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteAttributeGroupMapping(attributeGrpMappingID, currentLoggedInUserId);
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

        public static Boolean UpdateAttributeSequence(IList<MapServiceAttributeToGroupContract> attributesToMove, Int32? destinationIndex, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateAttributeSequence(attributesToMove, destinationIndex, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean UpdateAttributeGroupMapping(Int32 attributeGrpMappingID, Int32 currentLoggedInUserId, Boolean IsRequired, Boolean IsDisplay, Int32? sourceAttributeId, Boolean IsHiddenFromUI)
        {
            try
            {
                Boolean result = false;
                result = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateAttributeGrpMapping(attributeGrpMappingID, currentLoggedInUserId, IsRequired, IsDisplay, sourceAttributeId, IsHiddenFromUI);
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

        #region Manage Custom Forms

        /// <summary>
        /// Returns all the CustomForms viewable to the current logged in user. 
        /// </summary>
        /// <returns>List of Custom Forms</returns>
        public static List<Entity.CustomForm> GetAllCustomForms()
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllCustomForms();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Saves the Entity.CustomForm.
        /// </summary>
        /// <param name="category">Entity.CustomForm Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>Entity.CustomForm Entity</returns>
        public static Entity.CustomForm SaveCustomFormDetail(Entity.CustomForm customForm, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveCustomFormDetail(customForm, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Checks if the custom Form Name already exists.
        /// </summary>
        /// <param name="customFormName">custom Form Name</param>
        /// <returns>True or false</returns>
        public static Boolean CheckIfCustomFormNameAlreadyExist(String customFormName)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).CheckIfCustomFormNameAlreadyExist(customFormName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Updates the custom Form.
        /// </summary>
        /// <param name="customForm">Entity.CustomForm Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public static Boolean UpdateCustomFormDetail(Entity.CustomForm customForm, Int32 customFormID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateCustomFormDetail(customForm, customFormID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets specific custom Form.
        /// </summary>
        /// <param name="customFormId">CustomFormID</param>
        public static Entity.CustomForm GetCurrentCustomFormInfo(Int32 customFormID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetCurrentCustomFormInfo(customFormID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Deletes the custom Form.
        /// </summary>
        /// <param name="customFormId">CustomFormID</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public static Boolean DeleteCustomForm(Int32 customFormID, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteCustomForm(customFormID, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Returns all the Entity.CustomFormAttributeGroup for specific Entity.CustomForm ID. 
        /// </summary>
        /// <returns>List of CustomFormAttributeGroups</returns>
        public static List<Entity.CustomFormAttributeGroup> GetCustomFormAttrGrpsByCustomFormId(Int32 customFormId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetCustomFormAttrGrpsByCustomFormId(customFormId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        ///Updates Custom Form Sequence.
        /// </summary>
        /// <param name="customFormsToMove">IList of Entity.CustomForm Entity</param>
        /// <param name="destinationIndex">Index</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public static Boolean UpdateCustomFormSequence(IList<Entity.CustomForm> customFormsToMove, Int32 destinationIndex, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateCustomFormSequence(customFormsToMove, destinationIndex, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// Returns all the BkgSvcAttributeGroup from master DB. 
        /// </summary>
        /// <returns>List of BkgSvcAttributeGroups</returns>
        public static List<Entity.BkgSvcAttributeGroup> GetAllBkgSvcAttributeGroup()
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllBkgSvcAttributeGroup();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Guid GetCodeForCurrentAttributeGroup(Int32? attributeGrpID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetCodeForCurrentAttributeGroup(attributeGrpID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        //Guid GetCodeForCurrentAttributeGroup(Int32 attributeGrpID)

        /// <summary>
        /// Saves the Entity.CustomFormAttributeGroup from master DB.
        /// </summary>
        /// <param name="category">Entity.CustomFormAttributeGroup Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>Entity.CustomFormAttributeGroup Entity</returns>
        public static Entity.CustomFormAttributeGroup SaveCustomFormAttributeGroupDetail(Entity.CustomFormAttributeGroup customFormAttrGrp, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveCustomFormAttributeGroupDetail(customFormAttrGrp, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Checks if the Entity.CustomFormAttributeGroup already exists from master DB.
        /// </summary>
        /// <param name="customFormId">customFormId</param>
        /// <param name="attrGrpId">AttributeGroupId</param>
        /// <returns>True or false</returns>
        public static Boolean CheckIfCustomFormAttrGrpMappingAlreadyExist(Int32 customFormId, Int32 attrGrpId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).CheckIfCustomFormAttrGrpMappingAlreadyExist(customFormId, attrGrpId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Updates the Entity.CustomFormAttributeGroup from master DB.
        /// </summary>
        /// <param name="customFormAttributeGroup">Entity.CustomFormAttributeGroup Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public static Boolean UpdateCustomFormAttributeGroupDetail(Entity.CustomFormAttributeGroup customFormAttributeGroup, Int32 customFormAttributeGroupID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateCustomFormAttributeGroupDetail(customFormAttributeGroup, customFormAttributeGroupID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        ///Updates Custom Form Attribute Group Mapping Sequence.
        /// </summary>
        /// <param name="customFormsToMove">IList of Entity.CustomFormAttributeGroup Entity</param>
        /// <param name="destinationIndex">Index</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public static Boolean UpdateCustomFormAttributeGroupSequence(IList<Entity.CustomFormAttributeGroup> customFormAttributeGroupsToMove, Int32 destinationIndex, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateCustomFormAttributeGroupSequence(customFormAttributeGroupsToMove, destinationIndex, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Deletes the Entity.CustomFormAttributeGroup from master DB.
        /// </summary>
        /// <param name="customFormAttributeGroupID">customFormAttributeGroupID</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public static Boolean DeleteCustomFormAttributeGroup(Int32 customFormAttributeGroupID, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteCustomFormAttributeGroup(customFormAttributeGroupID, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets specific Entity.CustomFormAttributeGroup from master DB.
        /// </summary>
        /// <param name="customFormAttributeGroupID">customFormAttributeGroupID</param>
        public static Entity.CustomFormAttributeGroup GetCurrentCustomFormAttributeGroup(Int32 customFormAttributeGroupID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetCurrentCustomFormAttributeGroup(customFormAttributeGroupID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<Entity.lkpCustomFormType> GetcustomFormType()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpCustomFormType>().Where(x => x.CFT_IsDeleted == false).ToList();

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
        //GetcustomFormType() 
        #endregion

        #region Map Master Services to Client

        #region Background Services
        //GetExistingBackgroundServices
        public static List<MapServicesToClientContract> GetBackgroundServices(Int32 defaultTenantId, Int32? SvcID = null, String SvcName = null, String ExtCode = null)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(defaultTenantId).GetBackgroundServices(SvcID, SvcName, ExtCode);
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

        #region Map Services To Client
        public static Boolean MapServicesToClient(Int32 SelectedTenantId, String SelectedServices, Int32 DefaultTenantId)
        {
            try
            {
                Boolean IsInserted = BALUtils.GetBackgroundSetupRepoInstance(SelectedTenantId).MapServicesToClient(SelectedServices, SelectedTenantId);
                if (IsInserted)
                {
                    return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantId).UpdateClientCount(SelectedServices, true);
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
        #endregion

        #region Existing Background Services of Client
        public static Int32[] GetExistingBackgroundServices(Int32 TenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(TenantId).GetExistingBackgroundServices();
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

        #region Deactivate Mapping
        public static Boolean DeactivateMapping(Int32 selectedTenantId, Int32 selectedServiceId, Int32 DefaultTenantId)
        {
            try
            {
                Boolean IsDeactivated = BALUtils.GetBackgroundSetupRepoInstance(selectedTenantId).DeactivateMapping(selectedServiceId, selectedTenantId);
                if (IsDeactivated)
                {
                    return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantId).UpdateClientCount(selectedServiceId.ToString(), false);
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
        #endregion

        #endregion

        #region Derived From Services

        public static List<Entity.BackgroundService> GetDerivedFromServiceList(Int32? currentServiceId = null)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetDerivedFromServiceList(currentServiceId);

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

        public static Boolean IsChildServiceExist(Int32 currentServiceId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).IsChildServiceExist(currentServiceId);

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

        #region Manage Master Services
        /// <summary>
        /// Get all the Service from Master
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<Entity.BackgroundService> GetMasterServices()
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetMasterServices();
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
        public static List<Entity.lkpBkgSvcType> GetBkgServiceType()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpBkgSvcType>().Where(x => x.BST_IsDeleted == false).OrderBy(con => con.BST_Name).ToList();

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
        //check if service already exist by service Name
        public static Boolean CheckIfServiceNameAlreadyExist(String serviceName, Int32 svcID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).CheckIfServiceNameAlreadyExist(serviceName, svcID);

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
        /// Save new service in Master 
        /// </summary>
        /// <param name="masterService"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Int32 SaveNewServiceDetail(Entity.BackgroundService masterService, Int32 currentLoggedInUserId)
        {
            try
            {
                Entity.BackgroundService BkgService = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveNewServiceDetail(masterService, currentLoggedInUserId);
                return BkgService.BSE_ID;
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
        /// update the Master
        /// </summary>
        /// <param name="masterService">masterService</param>
        /// <param name="svcMasterID">svcMasterID</param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="tenantId"></param>
        public static void UpdateServiceDetail(Entity.BackgroundService masterService, Int32 svcMasterID, Int32 currentLoggedInUserId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateServiceDetail(masterService, svcMasterID, currentLoggedInUserId);

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

        public static void DeletebackgroundService(Int32 bkgSvcMasterID, Int32 currentLoggedInUserId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeletebackgroundService(bkgSvcMasterID, currentLoggedInUserId);

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
        public static String BkgSrvName(Int32 bkgSvcMasterID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).BkgSrvName(bkgSvcMasterID);

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
        //BackgroundSetupManager.BkgSrvName(bkgSvcMasterID);
        #endregion

        #region Service Attribute Group Mapping

        public static List<ManageServiceAttributeGrpContract> GetAttributeGrps(Int32 serviceId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetAttributeGrps(serviceId);

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

        public static List<Entity.BkgSvcAttributeGroup> GetAllAttributeGroups(Int32 serviceID, Boolean isupdate, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllAttributeGroups(serviceID, isupdate);

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

        public static List<Entity.BkgSvcAttribute> GetAllAttributes(Int32 attributegrpID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllAttributes(attributegrpID);

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


        public static void SaveAttributeGrpMappings(Entity.BkgSvcAttributeGroupMapping newSvcAttributeGrpMapping, Int32 tenantId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveAttributeGrpMappings(newSvcAttributeGrpMapping);

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
        public static List<Int32> GetAllAttributesMappingIDs(List<Int32> attributesIDs, Int32 attributegrpID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllAttributesMappingIDs(attributesIDs, attributegrpID);

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

        public static List<Int32> GetAllAttributeIDsRelatedToAttributeGrpID(Int32 attributegrpID, Int32 serviceId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllAttributeIDsRelatedToAttributeGrpID(attributegrpID, serviceId);

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
        public static void UpdateAtttributeMappingLst(Int32 attributegrpID, Int32 serviceId, Int32 currentLoggedInUserId, List<Int32> updatedattributeIdLst, Int32 tenantId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateAtttributeMappingLst(attributegrpID, serviceId, currentLoggedInUserId, updatedattributeIdLst);

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

        public static void DeleteAttributeServiceMappingByAttributeId(Int32 attributegrpID, Int32 attributeId, Int32 serviceId, Int32 currentLoggedInUserId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteAttributeServiceMappingByAttributeId(attributegrpID, attributeId, serviceId, currentLoggedInUserId);

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
        public static void DeleteAttributMappingwithServicebyAttributeGroupid(Int32 attributegrpID, Int32 serviceId, Int32 currentLoggedInUserId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteAttributMappingwithServicebyAttributeGroupid(attributegrpID, serviceId, currentLoggedInUserId);

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
        //void DeleteAttributMappingwithServicebyAttributeGroupid(Int32 attributegrpID, Int32 serviceId, Int32 currentLoggedInUserId)
        #endregion

        #region Service CustomForm Mapping
        public static List<ManageServiceCustomFormContract> GetCustomFormsForService(Int32 serviceId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetCustomFormsForService(serviceId);

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
        public static List<Entity.CustomForm> GetAllCustomForm(Int32 serviceId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllCustomForm(serviceId);

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
        //void SaveSvcFormMapping(BkgSvcFormMapping newSvcFormMapping)
        public static void SaveSvcFormMapping(Entity.BkgSvcFormMapping newSvcFormMapping)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveSvcFormMapping(newSvcFormMapping);

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
        //void DeleteSvcFormMApping(Int32 svcFormMappingID, Int32 currentLoggedInUserId)
        public static void DeleteSvcFormMApping(Int32 svcFormMappingID, Int32 currentLoggedInUserId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteSvcFormMApping(svcFormMappingID, currentLoggedInUserId);

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

        public static String FetchExternalBkgServiceCodeByID(Int32 SelectedTenantId, Int32 ExtSvcID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SelectedTenantId).FetchExternalBkgServiceCodeByID(ExtSvcID);

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
        /// Get data to map the grid on the basis of TenantId
        /// </summary>
        /// <param name="SelectedTenantId">SelectedTenantId</param>
        /// <returns></returns>
        public static List<ClientServiceVendorContract> GetMappedBkgSvcExtSvcToState(Int32 SelectedTenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetMappedBkgSvcExtSvcToState(SelectedTenantId);

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
        ///  Get all Background Services(Excluding all mapped).
        /// </summary>
        /// <param name="_isupdate"></param>
        /// <param name="selectedTenantID"></param>
        /// <returns></returns>
        public static List<Entity.ClientEntity.BackgroundService> GetBkgService(Int32 selectedTenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(selectedTenantID).GetBkgService();

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
        /// Get List Of External Service Mapped with Bkgroung Service
        /// </summary>
        /// <param name="SelectedBkgSvcID">SelectedBkgSvcID</param>
        /// <returns></returns>
        public static List<Entity.ExternalBkgSvc> GetExtBkgSvcCorrespondsToBkgSvc(Int32 SelectedBkgSvcID, Int32 selectedTenantID, Boolean _isupdate)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetExtBkgSvcCorrespondsToBkgSvc(SelectedBkgSvcID, selectedTenantID, _isupdate);

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
        //Get List of states
        public static List<Entity.State> GetAllStates()
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetAllStates();

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
        /// Get the List of State that mapped with ExtService
        /// </summary>
        /// <param name="ExtSvcId"></param>
        /// <param name="selectedTenantId"></param>
        /// <returns></returns>
        public static List<Int32> GetMAppedStatesIdtoExtSvc(Int32 ExtSvcId, Int32 selectedTenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetMAppedStatesIdtoExtSvc(ExtSvcId, selectedTenantId);

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
        /// get the Mapping Id of External and Background Service mapping.
        /// </summary>
        /// <param name="bkgSvcId">bkgSvcId</param>
        /// <param name="extSvcId">extSvcId</param>
        /// <returns></returns>
        public static Int32 GetBkgSvcExtSvcMappedId(Int32 bkgSvcId, Int32 extSvcId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetBkgSvcExtSvcMappedId(bkgSvcId, extSvcId);

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
        /// Save the Mapped state with Service
        /// </summary>
        /// <param name="clientExtSvcVendorMapping">clientExtSvcVendorMapping</param>
        public static void SaveClientSvcvendormapping(Entity.ClientExtSvcVendorMapping clientExtSvcVendorMapping)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveClientSvcvendormapping(clientExtSvcVendorMapping);

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
        /// Update the mapping list of State with Backgroung/External Service
        /// </summary>
        /// <param name="updatedMappedStateIds">updatedMappedStateIds</param>
        /// <param name="selectedServiceID">selectedServiceID</param>
        /// <param name="selectedExternalServiceId">selectedExternalServiceId</param>
        /// <param name="selectedTenantID">selectedTenantID</param>
        /// <param name="currentLoggedInUserID">currentLoggedInUserID</param>
        public static void UpdateClientSvcVendorMapping(List<Int32> updatedMappedStateIds, Int32 selectedServiceID, Int32 selectedExternalServiceId, Int32 selectedTenantID, Int32 currentLoggedInUserID)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateClientSvcVendorMapping(updatedMappedStateIds, selectedServiceID, selectedExternalServiceId, selectedTenantID, currentLoggedInUserID);

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
        /// Delete the Mapping of State with Backgroung/External Service
        /// </summary>
        /// <param name="bkgSvcID">bkgSvcID</param>
        /// <param name="selectedTenantId">selectedTenantId</param>
        /// <param name="currentLoggedInUserID">currentLoggedInUserID</param>
        public static void DeleteClientSvcVendorMapping(Int32 bkgSvcID, Int32 ExtServiceID, Int32 selectedTenantId, Int32 currentLoggedInUserID)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteClientSvcVendorMapping(bkgSvcID, ExtServiceID, selectedTenantId, currentLoggedInUserID);

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
        /// Get Vendor Service Mapping
        /// </summary>
        /// <returns>List of BkgSvcExtSvcMapping</returns>
        public static List<Entity.BkgSvcExtSvcMapping> GetVendorServiceMapping()
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetVendorServiceMapping();
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
        /// Delete vendor service mapping
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <param name="modifiedByID"></param>
        /// <returns>True/False</returns>
        public static Boolean DeleteVendorServiceMapping(Int32 vendorServiceMappingID, Int32 modifiedByID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteVendorServiceMapping(vendorServiceMappingID, modifiedByID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get Vendors 
        /// </summary>
        /// <returns>List of ExternalVendor</returns>
        public static List<Entity.ExternalVendor> GetVendors()
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetVendors();
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
        /// Get External Service By VendorID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>List of ExternalBkgSvc</returns>
        public static List<Entity.ExternalBkgSvc> GetExternalBkgSvcByVendorID(Int32 vendorID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetExternalBkgSvcByVendorID(vendorID);
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
        /// Save Vendor service Mapping
        /// </summary>
        /// <param name="bkgSvcExtSvcMapping"></param>
        /// <returns>True/False</returns>
        public static Boolean SaveVendorServiceMapping(Entity.BkgSvcExtSvcMapping bkgSvcExtSvcMapping)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveVendorServiceMapping(bkgSvcExtSvcMapping);
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
        /// Update Vendor Service Mapping
        /// </summary>
        /// <param name="bkgSvcExtSvcMapping"></param>
        /// <returns>True/False</returns>
        public static Boolean UpdateVendorServiceMapping(Entity.BkgSvcExtSvcMapping bkgSvcExtSvcMapping)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateVendorServiceMapping(bkgSvcExtSvcMapping);
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
        /// Check If Vendor Service mapping already exists
        /// </summary>
        /// <param name="bkgSvcId"></param>
        /// <param name="extSvcId"></param>
        /// <param name="bsesmID"></param>
        /// <returns>True/False</returns>
        public static Boolean IfVendorServiceMappingExists(Int32 bkgSvcId, Int32 extSvcId, Int32? bsesmID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).IfVendorServiceMappingExists(bkgSvcId, extSvcId, bsesmID);
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
        /// Get Vendor Service Attribute Mapping By Vendor Service Mapping ID
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <returns>List of VendorServiceAttributeMappingContract</returns>
        public static List<VendorServiceAttributeMappingContract> GetVendorServiceAttributeMappingList(Int32 vendorServiceMappingID)
        {
            try
            {
                return AssignVendorServiceAttributeMappingToDataModel(BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetVendorServiceAttributeMappingList(vendorServiceMappingID));
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
        /// Assign the datatable record in VendorServiceAttributeMappingContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of VendorServiceAttributeMappingContract</returns>
        private static List<VendorServiceAttributeMappingContract> AssignVendorServiceAttributeMappingToDataModel(DataTable table)
        {
            try
            {
                List<VendorServiceAttributeMappingContract> vendorSvcAttMappingList = new List<VendorServiceAttributeMappingContract>();

                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows != null && rows.Count() > 0)
                {
                    Int32 previousFieldID = 0, rowCounter = -1;
                    foreach (var row in rows)
                    {
                        if (previousFieldID != Convert.ToInt32(row["EBSA_FieldID"]))
                        {
                            VendorServiceAttributeMappingContract vndSvcAttMapping = new VendorServiceAttributeMappingContract();

                            vndSvcAttMapping.ESAM_ID = Convert.ToInt32(row["ESAM_ID"]);
                            vndSvcAttMapping.ESAM_BkgSvcAttributeGroupMappingID = Convert.ToInt32(row["ESAM_BkgSvcAttributeGroupMappingID"]);
                            vndSvcAttMapping.ESAM_ExternalBkgSvcAttributeID = Convert.ToInt32(row["ESAM_ExternalBkgSvcAttributeID"]);
                            vndSvcAttMapping.ESAM_ServiceMappingId = Convert.ToInt32(row["ESAM_ServiceMappingId"]);
                            vndSvcAttMapping.ESAM_CreatedBy = Convert.ToInt32(row["ESAM_ServiceMappingId"]);
                            vndSvcAttMapping.ESAM_CreatedOn = Convert.ToDateTime(row["ESAM_CreatedOn"]);
                            vndSvcAttMapping.ESAM_FieldDelimiter = Convert.ToString(row["ESAM_FieldDelimiter"]);
                            vndSvcAttMapping.EBSA_FieldID = Convert.ToInt32(row["EBSA_FieldID"]);
                            vndSvcAttMapping.EBSA_Label = Convert.ToString(row["EBSA_Label"]);
                            vndSvcAttMapping.EBSA_LocationField = Convert.ToString(row["EBSA_LocationField"]);
                            vndSvcAttMapping.EBSA_DefaultValue = Convert.ToString(row["EBSA_DefaultValue"]);
                            vndSvcAttMapping.BSA_Name = Convert.ToString(row["BSA_Name"]);
                            vndSvcAttMapping.BSA_Description = Convert.ToString(row["BSA_Description"]);
                            vndSvcAttMapping.BSAD_Name = Convert.ToString(row["BSAD_Name"]);
                            vndSvcAttMapping.IsComplex = false;
                            List<Int32> extSvcAttIds = new List<int>();
                            extSvcAttIds.Add(Convert.ToInt32(row["ESAM_BkgSvcAttributeGroupMappingID"]));
                            vndSvcAttMapping.BkgSvcAttMappingIDs = extSvcAttIds;
                            vndSvcAttMapping.IsRequired = Convert.ToBoolean(row["BAGM_IsRequired"]);
                            vndSvcAttMapping.ExternalIsRequired = Convert.ToBoolean(row["EBSA_IsRequired"]);

                            vendorSvcAttMappingList.Add(vndSvcAttMapping);
                            rowCounter++;
                            previousFieldID = Convert.ToInt32(row["EBSA_FieldID"]);

                        }
                        else
                        {
                            vendorSvcAttMappingList[rowCounter].BkgSvcAttMappingIDs.Add(Convert.ToInt32(row["ESAM_BkgSvcAttributeGroupMappingID"]));
                            vendorSvcAttMappingList[rowCounter].IsComplex = true;
                            vendorSvcAttMappingList[rowCounter].BSA_Name = vendorSvcAttMappingList[rowCounter].BSA_Name
                                + vendorSvcAttMappingList[rowCounter].ESAM_FieldDelimiter + Convert.ToString(row["BSA_Name"]);
                            vendorSvcAttMappingList[rowCounter].ESAM_FieldDelimiter = Convert.ToString(row["ESAM_FieldDelimiter"]);
                        }
                    }
                }

                return vendorSvcAttMappingList;
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
        /// Delete Vendor Service Attribute Mapping
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <param name="vendorServiceFieldID"></param>
        /// <param name="modifiedByID"></param>
        /// <returns>True/False</returns>
        public static Boolean DeleteVendorServiceAttributeMapping(Int32 vendorServiceMappingID, Int32 vendorServiceFieldID, Int32 modifiedByID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteVendorServiceAttributeMapping(vendorServiceMappingID, vendorServiceFieldID, modifiedByID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get Background And External Service Atributes by Vendor Service MappingID and Vendor Service Field ID
        /// </summary>
        /// <param name="vendorServiceMappingID"></param>
        /// <param name="vndSvcFieldId"></param>
        /// <returns>ServiceAttributesContract</returns>
        public static ServiceAttributesContract GetBkgSvcExtSvcAttributes(Int32 vendorServiceMappingID, Int32? vndSvcFieldId)
        {
            try
            {
                return AssignServiceAttributesToDataModel(BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetBkgSvcExtSvcAttributes(vendorServiceMappingID, vndSvcFieldId));
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
        /// Assign the datatable record in ServiceAttributesContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>ServiceAttributesContract</returns>
        private static ServiceAttributesContract AssignServiceAttributesToDataModel(DataSet ds)
        {
            try
            {
                ServiceAttributesContract svcAttributeContract = new ServiceAttributesContract();
                if (ds.Tables.Count > 0)
                {
                    List<ExternalServiceAttribute> ExternalServiceAttributeList = new List<ExternalServiceAttribute>();
                    List<InternalServiceAttribute> InternalServiceAttributeList = new List<InternalServiceAttribute>();

                    IEnumerable<DataRow> extSvcAttributeRows = ds.Tables[0].AsEnumerable();
                    if (extSvcAttributeRows.Count() > 0)
                    {
                        ExternalServiceAttributeList = extSvcAttributeRows.Select(x => new ExternalServiceAttribute
                        {
                            ExtSvcAttributeID = Convert.ToInt32(x["ExtSvcAttributeID"]),
                            ExtSvcAttributeName = Convert.ToString(x["ExtSvcAttributeName"]),
                            ExtSvcAttributeMappingID = x["ExtSvcAttributeMappingID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["ExtSvcAttributeMappingID"]) : 0,
                            BkgSvcAttributeName = x["BkgSvcAttributeName"].GetType().Name != "DBNull" ? Convert.ToString(x["BkgSvcAttributeName"]) : String.Empty,
                            BkgSvcAttributeGroupMappingID = x["BkgSvcAttributeGroupMappingID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BkgSvcAttributeGroupMappingID"]) : 0,
                            FieldSequence = x["FieldSequence"].GetType().Name != "DBNull" ? Convert.ToInt32(x["FieldSequence"]) : 0,
                            FormatTypeID = x["FormatTypeID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["FormatTypeID"]) : 0,
                        }).ToList();
                    }

                    IEnumerable<DataRow> bkgSvcAttributeEditRows = ds.Tables[1].AsEnumerable();
                    if (bkgSvcAttributeEditRows.Count() > 0)
                    {
                        InternalServiceAttributeList = bkgSvcAttributeEditRows.Select(x => new InternalServiceAttribute
                        {
                            BSAGM_ID = Convert.ToInt32(x["BSAGM_ID"]),
                            BSA_Name = Convert.ToString(x["BSA_Name"]),
                        }).ToList();
                    }

                    svcAttributeContract.ExternalServiceAttributeList = ExternalServiceAttributeList;
                    svcAttributeContract.InternalServiceAttributeList = InternalServiceAttributeList;
                }

                return svcAttributeContract;
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
        /// Save Vendor Service Attribute Mapping
        /// </summary>
        /// <param name="extSvcAttMapping"></param>
        /// <returns>True/False</returns>
        public static Boolean SaveVendorServiceAttributeMapping(List<Entity.ExternalSvcAtributeMapping> extSvcAttMapping)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveVendorServiceAttributeMapping(extSvcAttMapping);
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

        public static List<Entity.lkpFormatType> GetFormatTypes()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpFormatType>().Where(x => x.FTY_IsDeleted == false).ToList();
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

        #region Manage Order Color Status

        /// <summary>
        /// Returns all OrderFlags which are not deleted 
        /// </summary>
        /// <returns>List of lkpOrderFlag</returns>
        public static List<lkpOrderFlag> GetAllOrderFlags(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetAllOrderFlags();
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
        /// Returns InstitutionOrderFlag for specific Tenant. 
        /// </summary>
        /// <returns>List of InstitutionOrderFlag</returns>
        public static List<InstitutionOrderFlag> GetInstituteOrderFlags(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetInstituteOrderFlags(tenantId);
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
        /// Saves the InstitutionOrderFlag details.
        /// </summary>
        /// <param name="institutionOrderFlag">institutionOrderFlag Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>institutionOrderFlag Entity</returns>
        public static InstitutionOrderFlag SaveInstitutionOrderFlagDetail(Int32 tenantId, InstitutionOrderFlag institutionOrderFlag, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).SaveInstitutionOrderFlagDetail(institutionOrderFlag, currentLoggedInUserId);
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
        /// Gets specific InstitutionOrderFlag.
        /// </summary>
        /// <param name="InstitutionOrderFlagID">InstitutionOrderFlagID</param>
        public static InstitutionOrderFlag GetCurrentInstitutionOrderFlag(Int32 tenantId, Int32 institutionOrderFlagID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetCurrentInstitutionOrderFlag(institutionOrderFlagID);
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
        /// Updates the InstitutionOrderFlag.
        /// </summary>
        /// <param name="InstitutionOrderFlag">InstitutionOrderFlag Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public static Boolean UpdateInstitutionOrderFlagDetail(Int32 tenantId, InstitutionOrderFlag institutionOrderFlag, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateInstitutionOrderFlagDetail(institutionOrderFlag, currentLoggedInUserId);
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
        /// Deletes the InstitutionOrderFlag.
        /// </summary>
        /// <param name="customFormId">institutionOrderFlagID</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public static Boolean DeleteInstitutionOrderFlag(Int32 tenantId, Int32 institutionOrderFlagID, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeleteInstitutionOrderFlag(institutionOrderFlagID, currentUserId);
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
        /// To get Rule Template List
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<BkgRuleTemplate> GetRuleTemplates(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetRuleTemplates();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// To get Rule Template by rule Template Id
        /// </summary>
        /// <param name="ruleTemplateId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static BkgRuleTemplate GetRuleTemplateByID(Int32 ruleTemplateId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetRuleTemplateByID(ruleTemplateId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// To delete Rule Template record
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        public static void DeleteRuleTemplate(Int32 ruleId, Int32 currentUserId, Int32 tenantId)
        {
            BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeleteRuleTemplate(ruleId, currentUserId);
        }

        /// <summary>
        /// Gets the list of all Rule Result Types.
        /// </summary>
        /// <returns>
        /// List of lkpBkgRuleResultType Objects.
        /// </returns>
        public static List<lkpBkgRuleResultType> GetRuleResultTypes(Int32? tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpBkgRuleResultType>(tenantId).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// To get Rule Template
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static Entity.ComplianceRuleTemplate GetRuleTemplate(Int32 ruleId, Int32 tenantId)
        {
            try
            {
                String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
                String delimiters = "|||";
                delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;
                BkgRuleTemplate rule = BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetRuleTemplateByID(ruleId);
                Entity.ComplianceRuleTemplate complianceRuleTemplate = new Entity.ComplianceRuleTemplate();

                complianceRuleTemplate.RuleGroupExpression = rule.BRLT_UIExpression;
                complianceRuleTemplate.RLT_ID = rule.BRLT_ID;
                complianceRuleTemplate.RLT_Name = rule.BRLT_Name;
                complianceRuleTemplate.RLT_Description = rule.BRLT_Description;
                complianceRuleTemplate.RLT_ResultType = rule.BRLT_ResultType;
                complianceRuleTemplate.RLT_ActionType = 0;
                complianceRuleTemplate.RLT_Type = 0;
                complianceRuleTemplate.RLT_Code = rule.BRLT_Code;
                complianceRuleTemplate.RLT_ObjectCount = rule.BRLT_ObjectCount;
                complianceRuleTemplate.RLT_Notes = rule.BRLT_Notes;
                complianceRuleTemplate.RLT_IsActive = rule.BRLT_IsActive;
                complianceRuleTemplate.RLT_IsDeleted = rule.BRLT_IsDeleted;
                complianceRuleTemplate.RLT_CreatedByID = rule.BRLT_CreatedByID;
                complianceRuleTemplate.RLT_CreatedOn = rule.BRLT_CreatedOn;
                complianceRuleTemplate.RLT_ModifiedByID = rule.BRLT_ModifiedByID;
                complianceRuleTemplate.RLT_ModifiedOn = rule.BRLT_ModifiedOn;

                //Set IsRuleTemplateAssociatedWithRule = true if RuleTemplate is associated with any Rule (to restricts the user to save any change).
                if (rule.BkgRuleMappings.IsNotNull() && rule.BkgRuleMappings.Count(x => x.BRLM_IsDeleted == false) > 0)
                {
                    complianceRuleTemplate.IsRuleTemplateAssociatedWithRule = true;
                }
                List<BkgRuleTemplateExpression> expressions = rule.BkgRuleTemplateExpressions.Where(x => x.BRLE_IsActive && x.BRLE_IsDeleted == false).OrderBy(x => x.BRLE_ExpressionOrder).ToList();
                foreach (BkgRuleTemplateExpression rExp in expressions)
                {
                    Entity.ComplianceRuleExpressionTemplate complianceRuleExpressionTemplate = new Entity.ComplianceRuleExpressionTemplate();
                    complianceRuleExpressionTemplate.EX_ID = rExp.BkgExpression.BEX_ID;
                    complianceRuleExpressionTemplate.EX_Name = rExp.BkgExpression.BEX_Name;
                    complianceRuleExpressionTemplate.EX_Description = rExp.BkgExpression.BEX_Description;
                    complianceRuleExpressionTemplate.EX_Expression = rExp.BkgExpression.BEX_Expression;
                    complianceRuleExpressionTemplate.EX_IsActive = rExp.BkgExpression.BEX_IsActive;
                    complianceRuleExpressionTemplate.EX_IsDeleted = rExp.BkgExpression.BEX_IsDeleted;
                    complianceRuleExpressionTemplate.EX_CreatedByID = rExp.BkgExpression.BEX_CreatedByID;
                    complianceRuleExpressionTemplate.EX_CreatedOn = rExp.BkgExpression.BEX_CreatedOn;
                    complianceRuleExpressionTemplate.EX_ModifiedByID = rExp.BkgExpression.BEX_ModifiedByID;
                    complianceRuleExpressionTemplate.EX_ModifiedOn = rExp.BkgExpression.BEX_ModifiedOn;
                    complianceRuleExpressionTemplate.EX_Code = rExp.BkgExpression.BEX_Code;
                    complianceRuleExpressionTemplate.ExpressionOrder = rExp.BRLE_ExpressionOrder;

                    // Set Expression Elements
                    String[] expElements = complianceRuleExpressionTemplate.EX_Expression.Split(new String[] { delimiterKey }, StringSplitOptions.None);

                    foreach (String expElement in expElements)
                    {
                        if (expElement != String.Empty)
                        {
                            Entity.ComplianceRuleExpressionElementType ruleExpressionElementType = Entity.ComplianceRuleExpressionElementType.Operator;

                            String equalOperator = "EQUAL";

                            //if (expElement.StartsWith("O", StringComparison.OrdinalIgnoreCase))
                            if (expElement.StartsWith("[", StringComparison.OrdinalIgnoreCase))
                            {
                                ruleExpressionElementType = Entity.ComplianceRuleExpressionElementType.Object;
                            }
                            else if (expElement.StartsWith("E", StringComparison.OrdinalIgnoreCase) && !(equalOperator.Equals(expElement.ToUpper())))
                            {
                                ruleExpressionElementType = Entity.ComplianceRuleExpressionElementType.Expression;
                            }

                            complianceRuleExpressionTemplate.RuleExpressionElements.Add(new Entity.ComplianceRuleExpressionElement
                            {
                                ElementValue = expElement,
                                ExpressionElementType = ruleExpressionElementType
                            });
                        }
                    }

                    complianceRuleTemplate.ComplianceRuleExpressionTemplates.Add(complianceRuleExpressionTemplate);
                }
                return complianceRuleTemplate;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// To get Expression Operators
        /// </summary>
        /// <returns></returns>
        public static List<lkpBkgExpressionOperator> GetExpressionOperators(int noOfObjects, Int32 tenantId)
        {
            try
            {
                List<lkpBkgExpressionOperator> lkpOperators = LookupManager.GetLookUpData<lkpBkgExpressionOperator>(tenantId).ToList();
                Int32 operatorCount = lkpOperators.Count;
                lkpOperators.Insert(operatorCount, new lkpBkgExpressionOperator() { BEO_ID = 100 + operatorCount, BEO_Name = "(", BEO_UILabel = "(", BEO_SQL = "(" });
                operatorCount += 1;
                lkpOperators.Insert(operatorCount, new lkpBkgExpressionOperator() { BEO_ID = 100 + operatorCount, BEO_Name = ")", BEO_UILabel = ")", BEO_SQL = ")" });

                for (int index = 1; index <= noOfObjects; index++)
                {
                    lkpOperators.Insert(index - 1, new lkpBkgExpressionOperator() { BEO_ID = 1000 + index, BEO_Name = "[Object" + index + "]", BEO_UILabel = "[Object" + index + "]", BEO_SQL = "[Object" + index + "]" });
                }
                return lkpOperators;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Add Rule Template
        /// </summary>
        /// <param name="complianceRuleTemplate"></param>
        /// <param name="tenantId"></param>
        public static void AddRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate, Int32 tenantId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(tenantId).AddRuleTemplate(complianceRuleTemplate);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Update Rule Template
        /// </summary>
        /// <param name="complianceRuleTemplate"></param>
        /// <param name="expressionIds"></param>
        /// <param name="tenantId"></param>
        public static void UpdateRuleTemplate(Entity.ComplianceRuleTemplate complianceRuleTemplate, List<Int32> expressionIds, Int32 tenantId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateRuleTemplate(complianceRuleTemplate, expressionIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Validate Rule Template
        /// </summary>
        /// <param name="ruleExpressionTemplates"></param>
        /// <param name="resultTypeCode"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static RuleProcessingResult ValidateRuleTemplate(List<Entity.ComplianceRuleExpressionTemplate> ruleExpressionTemplates, String resultTypeCode, Int32 tenantId)
        {
            try
            {
                if (!(ruleExpressionTemplates.Count > 0))
                {
                    return new RuleProcessingResult
                    {
                        Status = 1,
                        ErrorMessage = "No expression defined."
                    };
                }
                String xmlResult = String.Empty;
                XmlDocument doc = new XmlDocument();
                XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("Rule"));
                el.AppendChild(doc.CreateElement("ResultType")).InnerText = resultTypeCode;
                XmlNode expGroup = el.AppendChild(doc.CreateElement("Expressions"));

                String delimiterKey = WebConfigurationManager.AppSettings[AppConsts.DELIMITER];
                String delimiters = "$$$";
                delimiterKey = delimiterKey.IsNull() || delimiterKey == "" ? delimiters : delimiterKey;

                foreach (Entity.ComplianceRuleExpressionTemplate ruleExpressionTemplate in ruleExpressionTemplates)
                {
                    XmlNode exp = expGroup.AppendChild(doc.CreateElement("Expression"));

                    String expressionName = ruleExpressionTemplate.EX_Name == "(Group)" ? "GE" : ruleExpressionTemplate.EX_Name;
                    exp.AppendChild(doc.CreateElement("Name")).InnerText = expressionName;

                    String definition = String.Empty;
                    for (int index = 0; index < ruleExpressionTemplate.RuleExpressionElements.Count; index++)
                    {
                        String equalOperator = "EQUAL";
                        if (ruleExpressionTemplate.RuleExpressionElements[index].ElementValue.ToUpper().StartsWith("E", StringComparison.OrdinalIgnoreCase) && !(equalOperator.Equals(ruleExpressionTemplate.RuleExpressionElements[index].ElementValue.ToUpper())))
                        {
                            if (definition == String.Empty)
                            {
                                definition += ruleExpressionTemplate.RuleExpressionElements[index].ElementValue.ToUpper();
                            }
                            else
                            {
                                definition += delimiterKey + ruleExpressionTemplate.RuleExpressionElements[index].ElementValue.ToUpper();
                            }
                        }
                        else
                        {
                            if (definition == String.Empty)
                            {
                                definition += ruleExpressionTemplate.RuleExpressionElements[index].ElementOperator.ToUpper();
                            }
                            else
                            {
                                definition += delimiterKey + ruleExpressionTemplate.RuleExpressionElements[index].ElementOperator.ToUpper();
                            }
                        }
                    }
                    definition = definition.Trim();

                    //Implement CDATA object 
                    //exp.AppendChild(doc.CreateElement("Definition")).InnerText = SysXUtils.GetXmlDecodedString(definition.Trim());
                    exp.AppendChild(doc.CreateElement("Definition")).InnerText = "<![CDATA[" + definition + "]]>";
                }

                /* Implement Encoding 
                //Create an XML declaration. 
                XmlDeclaration xmldecl;
                xmldecl = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");

                //Add the new node to the document.
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmldecl, root);
                */

                xmlResult = BALUtils.GetBackgroundSetupRepoInstance(tenantId).ValidateRuleTemplate(doc.OuterXml.ToString().Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&"));
                return RuleManager.ParseValidationXml(xmlResult);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Manage Service Settings

        public static Entity.ApplicableServiceSetting GetServiceSetting(Int32 tenantId, Int32 backgroundServiceId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServiceSetting(backgroundServiceId);
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

        public static BkgPackageSvc GetCurrentBkgPkgServiceDetail(Int32 tenantId, Int32 backgroundPkgSrvcId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetCurrentBkgPkgServiceDetail(backgroundPkgSrvcId);
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

        //UAT-3109 --Add Service AMER# when clicking service name on hierarchy and package screens
        public static String GetCurrentBkgPkgServiceAMERDetail(Int32 backgroundServiceId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetCurrentBkgPkgServiceAMERDetail(backgroundServiceId);
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

        #region Order Client Status
        public static Boolean SaveOrderClientStatus(Int32 SelectedTenantId, String OrderClientStatusTypeName, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SelectedTenantId).SaveOrderClientStatus(SelectedTenantId, OrderClientStatusTypeName, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BkgOrderClientStatu> FetchOrderClientStatus(Int32 SelectedTenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SelectedTenantId).FetchOrderClientStatus();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateClientStatusSequence(Int32 SelectedTenantId, IList<BkgOrderClientStatu> statusToMove, Int32? destinationIndex, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SelectedTenantId).UpdateClientStatusSequence(statusToMove, destinationIndex, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteOrderClientStatus(Int32 SelectedTenantId, Int32 Id, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SelectedTenantId).DeleteOrderClientStatus(Id, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateOrderClientStatus(Int32 SelectedTenantId, Int32 OrderClientStatusId, String OrderClientStatusTypeName, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SelectedTenantId).UpdateOrderClientStatus(OrderClientStatusId, OrderClientStatusTypeName, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Manage Service Item Entity

        public static List<GetServiceItemEntityList> getServiceItemEntityList(Int32 serviceItemId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).getServiceItemEntityList(serviceItemId);
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

        public static List<GetAttributeListForServiceItemEntity> getAttribteListForServiceItemEntity(Int32 serviceItemId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).getAttribteListForServiceItemEntity(serviceItemId);
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

        public static Boolean SavePackageServiceItemEntity(List<PackageServiceItemEntity> newServiceItemEntityList, Int32 tenantId, Int32 currentloggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).SavePackageServiceItemEntity(newServiceItemEntityList, currentloggedInUserId);
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

        public static Boolean DeletePackageServiceItemEntityRecord(Int32 packageServiceItemEntityId, Int32 tenantId, Int32 currentloggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeletePackageServiceItemEntityRecord(packageServiceItemEntityId, currentloggedInUserId);
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

        #region Service Vendors

        public static IList<Entity.ExternalVendor> FetchServiceVendorsList(Int32 DefaultTenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantId).FetchServiceVendors();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static bool SaveServiceVendors(Int32 DefaultTenantId, VendorsDetailsContract vendorsDetails, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantId).SaveServiceVendors(vendorsDetails, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateServiceVendors(Int32 DefaultTenantId, VendorsDetailsContract vendorsDetails, Int32 serviceVendorsID, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantId).UpdateServiceVendors(vendorsDetails, serviceVendorsID, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteServiceVendors(Int32 DefaultTenantId, Int32 serviceVendorsID, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantId).DeleteServiceVendors(serviceVendorsID, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Institution Hierarchy Vendor Account Mapping
        /// <summary>
        /// Get the list of External Vendor Account and Institution Hierarchy Vendor Account Mapping for a given node
        /// </summary>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        public static List<ExternalVendorAccountMappingDetails> GetInstHierarchyVendorAcctMappingDetails(Int32 TenantId, Int32 DPMId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(TenantId).GetInstHierarchyVendorAcctMappingDetails(DPMId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get the list of Filtered (yet not selected) External Vendor Account for a given node
        /// </summary>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        public static List<Entity.ExternalVendorAccount> GetExternalVendorAccountsNotMapped(Int32 TenantId, Int32 DPMId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(TenantId).GetExternalVendorAccountsNotMapped(DPMId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<MappedResidentialHistoryAttributeGroupsWithPkg> GetMappedResidentialHistoryAttributeGroupsWithPkg(Int32 TenantId, Int32 pkgId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(TenantId).GetMappedResidentialHistoryAttributeGroupsWithPkg(pkgId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Save the External Vendor Account mapping for a given node
        /// </summary>
        /// <param name="objInstHierarchyVendorAcctMapping">object InstHierarchyVendorAcctMapping</param>
        /// <returns></returns>
        public static Boolean SaveInstHierarchyVendorAcctMapping(Int32 TenantId, InstHierarchyVendorAcctMapping objInstHierarchyVendorAcctMapping)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(TenantId).SaveInstHierarchyVendorAcctMapping(objInstHierarchyVendorAcctMapping);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get the  External Vendor Account mapping for a given node
        /// </summary>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        public static InstHierarchyVendorAcctMapping GetInstHierarchyVendorAcctMappingByID(Int32 TenantId, Int32 instHierarchyVendorAcctMappingID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(TenantId).GetInstHierarchyVendorAcctMappingByID(instHierarchyVendorAcctMappingID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Update Tenant after modifying record.
        /// </summary>
        /// <returns>Boolean</returns>
        public static Boolean UpdateTenantChanges(Int32 TenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(TenantId).UpdateTenantChanges();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Map Regulatory Entity

        public static List<Entity.lkpRegulatoryEntityType> FetchRegulatoryEntityTypeNotMapped(Int32 tenantId, Int32 nDPMID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).FetchRegulatoryEntityTypeNotMapped(nDPMID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveInstHierarchyRegulatoryEntityMapping(Int32 tenantId, InstHierarchyRegulatoryEntityMapping instHierarchyRegEntity)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).SaveInstHierarchyRegulatoryEntityMapping(instHierarchyRegEntity);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<InstHierarchyRegulatoryEntityMappingDetails> GetInstHierarchyRegulatoryEntityMappingDetails(Int32 tenantId, Int32 nDPMId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetInstHierarchyRegulatoryEntityMappingDetails(nDPMId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static InstHierarchyRegulatoryEntityMapping GetInstHierarchyRegEntityMappingByID(Int32 tenantId, Int32 mappingID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetInstHierarchyRegEntityMappingByID(mappingID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region BackgroundPackageDetails
        public static BkgPackageHierarchyMapping GetBackgroundPackageDetail(Int32 BkgPackageNodeMappingId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBackgroundPackageDetail(BkgPackageNodeMappingId);
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
        public static Boolean UpdatePackageHirarchyDetails(BkgPackageHierarchyMapping bkgPackageHierarchyMapping, Int32 bkgPackageHierarchyMappingId, Int32 currentLoggedInID, List<Int32> lstSelectedOptionIds, Int32 tenantId, List<Int32> targetPackageIds, Int32 months, Boolean isActive)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdatePackageHirarchyDetails(bkgPackageHierarchyMapping, bkgPackageHierarchyMappingId, currentLoggedInID, lstSelectedOptionIds, targetPackageIds, months, isActive);
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
        //UpdatePackageHirarchyDetails(bkgPackageHierarchyMapping,View.TenantId)
        #endregion

        #region Vendor Account Settings

        public static IList<Entity.ExternalVendorAccount> FetchVendorsAccountDetail(Int32 DefaultTenantId, Int32 VendorId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantId).FetchVendorsAccountDetail(VendorId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String SaveVendorsAccountDetail(Int32 DefaultTenantId, Int32 VendorId, String AccountNumber, String AccountName, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantId).SaveVendorsAccountDetail(VendorId, AccountNumber, AccountName, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String UpdateVendorsAccountDetail(Int32 DefaultTenantId, String AccountNumber, String AccountName, Int32 EvaId, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantId).UpdateVendorsAccountDetail(AccountNumber, AccountName, EvaId, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteVendorsAccountDetail(Int32 DefaultTenantId, Int32 EvaId, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantId).DeleteVendorsAccountDetail(EvaId, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Import ClearStar Services

        public static IList<Entity.ExternalBkgSvc> FetchExternalBkgServices(Int32 DefaultTenantID, Int32 VendorID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantID).FetchExternalBkgServices(VendorID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static IList<Entity.ExternalBkgSvcAttribute> FetchExternalBkgServiceAttributes(Int32 DefaultTenantID, Int32 EBS_ID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantID).FetchExternalBkgServiceAttributes(EBS_ID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static IList<Entity.ClearStarService> FetchClearstarServices(Int32 DefaultTenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantID).FetchClearstarServices();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean ImportClearStarServices(Int32 DefaultTenantID, Int32[] SelectedCssIds, Int32 VendorID, Int32 CurrentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantID).ImportClearStarServices(SelectedCssIds, VendorID, CurrentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static IEnumerable<Entity.ClearStarService> FetchAllClearstarServices(Int32 DefaultTenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(DefaultTenantID).FetchAllClearstarServices();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveClearStarSevices(List<Entity.ClearStarService> lstClearStarService, Int32 defaultTenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(defaultTenantId).SaveClearStarSevices(lstClearStarService);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Manage D & R Documents

        public static Boolean SaveDisclosureTemplateDocument(List<Entity.SystemDocument> lstDisclosureDocuments)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).SaveDisclosureTemplateDocument(lstDisclosureDocuments);
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

        public static Int32 GetDocumentStatusIDByCode(String code)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpDocumentStatu>().Where(x => x.DS_Code == code).FirstOrDefault().DS_ID;
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

        public static Int32 GetDocumentTypeIDByCode(String code)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.lkpDocumentType>().Where(x => x.DT_Code == code).FirstOrDefault().DT_ID;
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

        public static List<Entity.SystemDocument> GetDisclosureTemplateDocuments(Int32 docTypeID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetDisclosureTemplateDocuments(docTypeID);
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

        public static Boolean DeleteDisclosureTemplateDocument(Int32 systemDocumentID, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).DeleteDisclosureTemplateDocument(systemDocumentID, currentUserId);
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

        public static Boolean UpdateDisclosureTemplateDocument(Entity.SystemDocument disclosureDocument, Int32 selectedBkgSvcId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateDisclosureTemplateDocument(disclosureDocument, selectedBkgSvcId);
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

        #region Manage Service Item Custom Forms Mappings

        public static List<Entity.CustomForm> GetSupplCustomFrmsNotMappedToSvcItem(Int32 tenantId, Int32 pkgSvcItemID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetSupplCustomFrmsNotMappedToSvcItem(pkgSvcItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveBkgSvcItemFormMapping(Int32 tenantId, BkgSvcItemFormMapping objBkgSvcItemFormMapping)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).SaveBkgSvcItemFormMapping(objBkgSvcItemFormMapping);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get the list of Pkg Service Item Custom Form Mapping Details for a given service item
        /// </summary>
        /// <param name="pkgServiceItemId"></param>
        /// <returns></returns>
        public static List<PkgServiceItemCustomFormMappingDetails> GetPkgServiceItemCustomFormMappingDetails(Int32 tenantId, Int32 pkgServiceItemId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetPkgServiceItemCustomFormMappingDetails(pkgServiceItemId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static BkgSvcItemFormMapping GetBkgSvcItemFormMappingById(Int32 tenantId, Int32 bkgSvcItemFormMappingId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBkgSvcItemFormMappingById(bkgSvcItemFormMappingId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region D And R AttributeGroup Mapping

        public static List<DAndRAttributeGroupMappingContract> GetDAndRAttributeGroupMapping(Int32 systemDocumentId)
        {
            try
            {
                List<DAndRAttributeGroupMappingContract> lstDAndRAttributeGroupMappingContract = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID)
                                                                            .GetDAndRAttributeGroupMapping(systemDocumentId);
                foreach (DAndRAttributeGroupMappingContract dAndRContract in lstDAndRAttributeGroupMappingContract)
                {
                    if (dAndRContract.TenantID > AppConsts.NONE && dAndRContract.CustomAttributeID > AppConsts.NONE)
                    {
                        CustomAttribute customAttribute = ComplianceDataManager.GetCustomAttributes(dAndRContract.TenantID, dAndRContract.CustomAttributeID);
                        if (!customAttribute.IsNullOrEmpty())
                        {
                            dAndRContract.CustomAttributeName = customAttribute.CA_AttributeLabel.IsNullOrEmpty()
                                                                    ? customAttribute.CA_AttributeName : customAttribute.CA_AttributeLabel;
                        }
                    }
                }
                return lstDAndRAttributeGroupMappingContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static IQueryable<Entity.BkgSvcAttributeGroup> GetServiceAttributeGroup()
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetServiceAttributeGroup();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static IQueryable<Entity.BkgSvcAttribute> GetServiceAttributeByServiceGroupID(Int32 BkgSvcAGID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetServiceAttributeByServiceGroupID(BkgSvcAGID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //public static Boolean UpdateMapping(Int32 systemDocumentID, Int32 bkgSvcAttributeGroupID, Int32 bkgSvcAttributeID, Int32 currentLoggedInUserID, Int32 specialFieldType_ID, Boolean rbApplicantAttr)
        public static Boolean UpdateMapping(DAndRAttributeGroupMappingContract dAndRContract, Int32 currentLoggedInUserID)
        {
            try
            {//DAndRAttributeGroupMappingContract dAndRContract
                //return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateMapping(systemDocumentID, bkgSvcAttributeGroupID, bkgSvcAttributeID, currentLoggedInUserID, specialFieldType_ID, rbApplicantAttr);
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).UpdateMapping(dAndRContract, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        #endregion

        #region Disclosure and Release

        public static List<Entity.SystemDocument> GetDisclosureAndReleaseDocuments(List<String> Countries, List<String> States, Int32? HierarchyNodeID, String RegulatoryNodeIDs,
                                                                                   String BkgServiceIds, Int32 TenantId, String disclosureDocAgeGroupType)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetDAndRDocuments(Countries, States, HierarchyNodeID, RegulatoryNodeIDs,
                                                                                                                  BkgServiceIds, TenantId, disclosureDocAgeGroupType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Int32> GetServicesIds(Int32 TenantID, List<Int32> BkgPackages)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(TenantID).GetServicesIds(BkgPackages);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<SysDocumentFieldMappingContract> GetFieldNames(Dictionary<Int32, String> dictionary, List<Entity.SystemDocument> DocumentList, List<TypeCustomAttributes> lstCustomAttributes, Int32 tenantID)
        {
            //GetFieldNames(Dictionary<Int32, String> dictionary, List<Entity.SystemDocument> DocumentList)
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetFieldNames(dictionary, DocumentList, lstCustomAttributes, tenantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Fill Data In Pdf Form
        public static byte[] FillDataInPdfForm(String documentPath, List<SysDocumentFieldMappingContract> formData, Int32 TenantID, String fullName = null)
        {
            try
            {
                //fetch current tenant name
                String tenantName = ComplianceDataManager.getClientTenant().Where(x => x.TenantID == TenantID).Select(x => x.TenantName).FirstOrDefault();
                byte[] buffer = CommonFileManager.RetrieveDocument(documentPath, FileType.SystemDocumentLocation.GetStringValue());
                byte[] updatedDocument = null;
                if (formData.IsNotNull() && formData.Count > 0)
                {
                    PdfReader reader = new PdfReader(buffer);
                    MemoryStream ms = new MemoryStream();
                    PdfStamper stamper = new PdfStamper(reader, ms);


                    //Fill-in the form values
                    AcroFields af = stamper.AcroFields;
                    foreach (var item in formData)
                    {
                        if (af.GetField(item.FieldName).IsNotNull())
                        {
                            if (item.SpecialFieldTypeCode.IsNotNull())
                            {
                                switch (item.SpecialFieldTypeCode)
                                {
                                    case "AAAA":
                                        af.SetField(item.FieldName, tenantName);
                                        af.SetFieldProperty(item.FieldName,
                                                        "setfflags",
                                                         PdfFormField.FF_READ_ONLY,
                                                         null);
                                        break;
                                    case "AAAB":
                                        af.SetField(item.FieldName, "Yes");
                                        af.SetFieldProperty(item.FieldName,
                                                        "setfflags",
                                                         PdfFormField.FF_READ_ONLY,
                                                         null);
                                        break;
                                    case "AAAD":
                                        af.SetFieldProperty(item.FieldName,
                                                        "setfflags",
                                                         PdfFormField.FF_READ_ONLY,
                                                         null);
                                        break;
                                    case "AAAE":
                                        af.SetField(item.FieldName, DateTime.Now.ToShortDateString());
                                        af.SetFieldProperty(item.FieldName,
                                                        "setfflags",
                                                         PdfFormField.FF_READ_ONLY,
                                                         null);
                                        break;
                                    case "AAAF":
                                        af.SetField(item.FieldName, fullName);
                                        af.SetFieldProperty(item.FieldName,
                                                        "setfflags",
                                                         PdfFormField.FF_READ_ONLY,
                                                         null);
                                        break;
                                }
                            }
                            else
                            {
                                af.SetField(item.FieldName, item.FieldValue);
                                af.SetFieldProperty(item.FieldName,
                                                         "setfflags",
                                                          PdfFormField.FF_READ_ONLY,
                                                          null);
                            }
                        }
                    }
                    //stamper.FormFlattening = true;
                    stamper.Close();
                    updatedDocument = ms.ToArray();
                    ms.Close();
                    //Recompress final document to further shrink.
                    //updatedDocument = CompressPDFDocument(updatedDocument);
                    reader.Close();
                }
                if (updatedDocument.IsNotNull())
                    return updatedDocument;
                return buffer;

            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
        }

        public static byte[] CompressPDFDocument(byte[] document)
        {
            try
            {
                PdfReader compressionReader = new PdfReader(document);
                MemoryStream compressionsMS = new MemoryStream();
                PdfStamper compressionStamper = new PdfStamper(compressionReader, compressionsMS);
                compressionStamper.FormFlattening = true;
                compressionStamper.SetFullCompression();
                compressionStamper.Close();
                document = compressionsMS.ToArray();
                compressionsMS.Close();
                compressionReader.Close();
                return document;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return null;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return null;
            }
        }
        #endregion

        public static Boolean CheckIfPackageNameAlreadyExist(String packageName, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).CheckIfPackageNameAlreadyExist(packageName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String CopyBackgroundPackage(Int32 sourceNodeId, Int32 targetNodeId, Int32 sourceBPHMId, String targetPackageName, Int32 currentLoggedInUserId, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).CopyBackgroundPackage(sourceNodeId, targetNodeId, sourceBPHMId, targetPackageName, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #region Special Field Type for D & R
        public static List<Entity.lkpDisclosureDocumentSpecialFieldType> GetSpecialFieldType()
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetSpecialFieldType();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static Boolean CheckIfVendorNameAlreadyExist(String vendorName, Int32 vendorId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).CheckIfVendorNameAlreadyExist(vendorName, vendorId);

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
        /// Check if the PackageServiceItem is Quanity group of any another ServiceItem
        /// </summary>
        /// <param name="pkgSvcItemId"></param>
        /// <returns></returns>
        public static Boolean IsPackageServiceItemEditable(Int32 pkgSvcItemId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).IsPackageServiceItemEditable(pkgSvcItemId);
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

        #region Manage Payment Option Instruction
        /// <summary>
        /// Get lkpPaymentOption from Security
        /// </summary>
        /// <returns>Security Payment Options</returns>
        public static List<Entity.lkpPaymentOption> GetSecurityPaymentOptions(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetSecurityPaymentOptions();
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
        /// Get lkpPaymentOption from Security by Id
        /// </summary>
        /// <param name="paymentOptionId"></param>
        /// <returns>Security Payment Option Id</returns>
        public static Entity.lkpPaymentOption GetSecurityPaymentOptionById(Int32 tenantId, Int32 paymentOptionId)
        {

            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetSecurityPaymentOptionById(paymentOptionId);
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
        /// Update Security after modifying record.
        /// </summary>
        /// <returns>Boolean</returns>
        public static Boolean UpdateSecurityChanges(Int32 tenantId)
        {

            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateSecurityChanges();
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

        #region Manual Service Forms

        /// <summary>
        /// Get all the Service from tenant
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<BackgroundService> GetTenantServices(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetTenantServices();
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

        public static Boolean UpdateOrderServiceServiceFormStatus(Int32 orderServiceFormId, Int32 statusId, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            try
            {
                Boolean result = false;
                result = BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateOrderServiceServiceFormStatus(orderServiceFormId, statusId, currentLoggedInUserId);
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

        public static Boolean CheckIfOrderClientStatusIsUsed(Int32 selectedTenantId, Int32 orderClientStatusId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(selectedTenantId).CheckIfOrderClientStatusIsUsed(orderClientStatusId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #region BkgCompl Package Data Mapping
        public static List<BackgroundPackage> GetPermittedBackgroundPackagesByUserID(int clientId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).GetPermittedBackgroundPackagesByUserID();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.ClientEntity.lkpBkgDataPointType> GetDataPoints(Int32 clientId)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgDataPointType>(clientId).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<LookupContract> GetServiceGroupsForPackage(Int32 clientId, Int32 selectedPackageId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).GetServiceGroupsForPackage(selectedPackageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<LookupContract> GetServicesForSvcGroup(int clientId, int selectedSvcGroup, int selectedBkgPkgID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).GetServicesForSvcGroup(selectedSvcGroup, selectedBkgPkgID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<LookupContract> GetServiceItemsForSvc(int clientId, int selectedService)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).GetServiceItemsForSvc(selectedService);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<LookupContract> GetComplianceCatagories(int clientId, int selectedComplPkgID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).GetComplianceCatagories(selectedComplPkgID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<LookupContract> GetCatagoryItems(int clientId, int selectedCatagoryID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).GetCatagoryItems(selectedCatagoryID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<LookupContract> GetComplianceItemAttributes(int clientId, int selectedItemID, string dataPointCode)
        {
            try
            {
                String ComplianceAttributeDataTypeCode = String.Empty;
                String ScreeningDocAttributeDataTypeCode = String.Empty;
                List<ComplianceItemAttribute> tmpLst = BALUtils.GetBackgroundSetupRepoInstance(clientId).GetComplianceItemAttributes(selectedItemID);

                if (dataPointCode == "AAAA" || dataPointCode == "AAAB")
                {
                    ComplianceAttributeDataTypeCode = "ADTDAT";
                }

                if (dataPointCode == "AAAC" || dataPointCode == "AAAD" || dataPointCode == "AAAG")
                {
                    ComplianceAttributeDataTypeCode = "ADTOPT";
                }

                if (dataPointCode == "AAAE" || dataPointCode == "AAAF")
                {
                    ComplianceAttributeDataTypeCode = "ADTFUP";
                    ScreeningDocAttributeDataTypeCode = ComplianceAttributeDatatypes.Screening_Document.GetStringValue();
                }
                //Commented below codeUAT-1738:Create new attribute type for data-synced documents and update data sync procedure
                //Int32 ComplianceAttributeDataTypeID = BALUtils.GetBackgroundSetupRepoInstance(clientId).GetComplianceAttributeDataTypeID(ComplianceAttributeDataTypeCode);

                #region UAT-1738:Create new attribute type for data-synced documents and update data sync procedure
                List<Int32> lstCompAttributeDataTypeID = new List<Int32>();
                List<Entity.ClientEntity.lkpComplianceAttributeDatatype> lstCompAttributeDataType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpComplianceAttributeDatatype>(clientId).ToList();
                lstCompAttributeDataTypeID = lstCompAttributeDataType.Where(x => x.Code == ComplianceAttributeDataTypeCode || x.Code == ScreeningDocAttributeDataTypeCode)
                                                                      .Select(slct => slct.ComplianceAttributeDatatypeID).ToList();

                #endregion
                //Commented below codeUAT-1738:Create new attribute type for data-synced documents and update data sync procedure
                //List<ComplianceItemAttribute> filteredLst = tmpLst.Where(cond => cond.ComplianceAttribute.ComplianceAttributeDatatypeID == ComplianceAttributeDataTypeID).ToList();

                //UAT-1738:Create new attribute type for data-synced documents and update data sync procedure
                List<ComplianceItemAttribute> filteredLst = tmpLst.Where(cond => lstCompAttributeDataTypeID.Contains(cond.ComplianceAttribute.ComplianceAttributeDatatypeID)).ToList();

                List<LookupContract> catagoryItemsAttributeList = new List<LookupContract>();
                foreach (ComplianceItemAttribute item in filteredLst)
                {
                    LookupContract serviceGroup = new LookupContract();
                    serviceGroup.ID = item.ComplianceAttribute.ComplianceAttributeID;
                    serviceGroup.Name = item.ComplianceAttribute.Name;
                    catagoryItemsAttributeList.Add(serviceGroup);
                }
                return catagoryItemsAttributeList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BkgCompliancePackageMappingSearchData> FetchBkgCompliancePackageMapping(int clientId, Int32 bkgPackageId, Int32 compPackageId, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return AssignValuesToBkgCompliancePackageMappingSearchData(BALUtils.GetBackgroundSetupRepoInstance(clientId).FetchBkgCompliancePackageMapping(bkgPackageId, compPackageId, gridCustomPaging));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.ClientEntity.ComplianceAttributeOption> GetComplianceAttributeOption(int clientId, Int32 attributeId)
        {
            try
            {
                //return LookupManager.GetLookUpData<Entity.ClientEntity.ComplianceAttributeOption>(clientId).Where(x => !x.IsDeleted && x.IsActive).ToList();
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).GetComplianceAttributeOption(attributeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveBkgComplPkgDataMapping(int clientId, BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).SaveBkgComplPkgDataMapping(bkgComplPkgDataMappingContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static string DeleteBkgComplPkgDataMapping(int clientId, int BCPM_ID, int currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).DeleteBkgComplPkgDataMapping(BCPM_ID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static string UpdateBkgComplPkgDataMapping(int clientId, int BCPM_ID, int currentLoggedInUserId, BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).UpdateBkgComplPkgDataMapping(BCPM_ID, currentLoggedInUserId, bkgComplPkgDataMappingContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static List<BkgCompliancePkgMappingAttrOption> FetchBkgCompliancePkgMappingAttrOptions(int clientId, int BCPM_ID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).FetchBkgCompliancePkgMappingAttrOptions(BCPM_ID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-583 WB: AMS: Ability to delete attributes and attribute groups from the package setup screen (even after the attribute or attribute group is active)
        public static Boolean DeletedBkgSvcAttributeGroupMapping(Int32 tenantId, Int32 bkgAttributeGroupId, Int32 bkgPackageSvcId, Int32 currentloggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).DeletedBkgSvcAttributeGroupMapping(bkgAttributeGroupId, bkgPackageSvcId, currentloggedInUserId);

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

        #region UAT 779: As an application admin, I should be able to update the display order and label of attributes at the tenant level.

        public static Boolean UpdateAttributeDisplaySequence(int tenantId, IList<AttributeSetupContract> statusToMove, Int32? destinationIndex, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateAttributeDisplaySequence(statusToMove, destinationIndex, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<AttributeSetupContract> GetBkgAttributesBasedOnGroup(int tenantId, int bkgSvcGroupId)
        {
            try
            {
                return NewAssignAttributeToDataModel(BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBkgAttributesBasedOnGroup(bkgSvcGroupId));
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

        #region UAT-800 Build all missing services into Complio based on spreadsheet of services for College System

        /// <summary>
        /// To Get Service Form Mapping for All and Specific Institute.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="serviceFormID"></param>
        /// <param name="serviceID"></param>
        /// <param name="mappingTypeID"></param>
        /// <param name="dpmID"></param>
        /// <returns>List of ServiceFormTenantMappingContract</returns>
        public static List<ServiceFormInstitutionMappingContract> GetServiceFormMappingAllandSpecificInstitution(Int32 tenantID, Int32? serviceFormID, Int32? serviceID, Int32? mappingTypeID, Int32? dpmID, Int32? selectedTenantID)
        {
            try
            {
                return AssignValuesToServiceFormTenantMappingContract(BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetServiceFormMappingAllandSpecificInstitution(serviceFormID, serviceID, mappingTypeID, dpmID, selectedTenantID));
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
        /// Assign the datatable record in ServiceFormTenantMappingContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of ServiceFormTenantMappingContract</returns>
        private static List<ServiceFormInstitutionMappingContract> AssignValuesToServiceFormTenantMappingContract(DataTable table)
        {
            try
            {
                List<ServiceFormInstitutionMappingContract> lstServiceFormTenantMapping = new List<ServiceFormInstitutionMappingContract>();
                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows.Count() > 0)
                {
                    lstServiceFormTenantMapping = rows.Select(x => new ServiceFormInstitutionMappingContract
                    {
                        SFM_ID = x["SFM_ID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["SFM_ID"]) : 0,
                        SF_ID = x["SF_ID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["SF_ID"]) : 0,
                        BSE_ID = x["BSE_ID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BSE_ID"]) : 0,
                        SAFHM_ID = x["SAFHM_ID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["SAFHM_ID"]) : (int?)null,
                        DPM_ID = x["DPM_ID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["DPM_ID"]) : (int?)null,
                        ServiceFormName = x["ServiceFormName"].GetType().Name != "DBNull" ? Convert.ToString(x["ServiceFormName"]) : String.Empty,
                        ServiceName = x["ServiceName"].GetType().Name != "DBNull" ? Convert.ToString(x["ServiceName"])
                        + (x["ExternalCode"].GetType().Name != "DBNull" ? "(" + Convert.ToString(x["ExternalCode"]) + ")" : "") : String.Empty,
                        DPM_Label = x["DPM_Label"].GetType().Name != "DBNull" ? Convert.ToString(x["DPM_Label"]) : String.Empty,
                        EnforceManual = x["EnforceManual"].GetType().Name != "DBNull" ? Convert.ToBoolean(x["EnforceManual"]) : (Boolean?)null,
                        IsAutomatic = x["IsAutomatic"].GetType().Name != "DBNull" ? Convert.ToBoolean(x["IsAutomatic"]) : false,
                    }).ToList();
                }

                return lstServiceFormTenantMapping;
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
        /// Get Background Service with Mapping
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns>List of BackgroundServiceMapping</returns>
        public static List<BackgroundServiceMapping> GetBackgroundServiceMapping(Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetBackgroundServiceMapping();
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
        /// Get Service Attached Forms
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns>List of ServiceForm</returns>
        public static List<ServiceForm> GetServiceForm(Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetServiceForm();
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
        /// Get Mapping Types
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns>List of SvcFormMappingType</returns>
        public static List<SvcFormMappingType> GetMappingType(Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetMappingType();
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
        /// Delete Service Form Institution Mapping
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="serviceFormMappingID"></param>
        /// <param name="serviceFormHierarchyMappingID"></param>
        /// <param name="mappingTypeID"></param>
        /// <param name="currentUserId"></param>
        /// <returns>True/False</returns>
        public static Boolean DeleteServiceFormInstitutionMapping(Int32 tenantID, Int32 serviceFormMappingID, Int32? serviceFormHierarchyMappingID, Int32 mappingTypeID, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).DeleteServiceFormInstitutionMapping(serviceFormMappingID, serviceFormHierarchyMappingID, mappingTypeID, currentUserId);
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
        /// Save Service Form Institution Mapping
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="svcFormInstitutionMappingContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns>True/False</returns>
        public static String SaveServiceFormInstitutionMapping(Int32 tenantID, ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).SaveServiceFormInstitutionMapping(svcFormInstitutionMappingContract, currentUserId);
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
        /// Update Service Form Institution Mapping
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="svcFormInstitutionMappingContract"></param>
        /// <param name="currentUserId"></param>
        /// <returns>True/False</returns>
        public static String UpdateServiceFormInstitutionMapping(Int32 tenantID, ServiceFormInstitutionMappingContract svcFormInstitutionMappingContract, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).UpdateServiceFormInstitutionMapping(svcFormInstitutionMappingContract, currentUserId);
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
        /// Get Service Ids by Service Form ID 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="serviceFormID"></param>
        /// <returns>List of ServiceIDs</returns>
        public static List<Int32> GetServiceIdsByServiceForm(Int32 tenantID, Int32 serviceFormID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetServiceIdsByServiceForm(serviceFormID);
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


        #region Service Attached Form

        public static List<ServiceAttachedFormContract> GetServoceAttachedFormList(Int32 clientId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).GetServoceAttachedFormList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.ServiceAttachedForm> GetParentServiceattachedForm(int clientId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).GetParentServiceattachedForm();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveServiceAttachedForm(Int32 tenantID, Entity.ServiceAttachedForm serviceAttachedForm)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).SaveServiceAttachedForm(serviceAttachedForm);

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

        public static Entity.ServiceAttachedForm GetServiceAttachedFormByID(Int32 tenantID, Int32 SF_ID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetServiceAttachedFormByID(SF_ID);

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

        //UAT-2480
        public static Boolean IsServiceAttachedFormVersionsDeleted(Int32 tenantID, Int32 SF_ID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).IsServiceAttachedFormVersionsDeleted(SF_ID);

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

        public static Boolean UpdateServiceAttachedForm(Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).UpdateServiceAttachedForm();
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

        public static Boolean CheckIfServiceAttachedFormNameAlreadyExist(Int32 tenantId, String serviceFormName, Int32 serviceFormID, Boolean isUpdate)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).CheckIfServiceAttachedFormNameAlreadyExist(serviceFormName, serviceFormID, isUpdate);

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

        public static IEnumerable<Entity.BkgServiceAttachedFormMapping> GetBkgServiceAttachedFormMappingByServiceFormID(Int32 tenantId, Int32 serviceFormID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBkgServiceAttachedFormMappingByServiceFormID(serviceFormID);

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

        #region UAT-803 - BACKGROUND PACKAGE STATE SEARCH CRITERIA
        public static Boolean SaveBkgPkgStateSearchCriteria(Int32 tenantID, List<BkgPackageStateSearchContract> bkgPkgStateSearchContract, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).SaveBkgPkgStateSearchCriteria(bkgPkgStateSearchContract, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.ClientEntity.BkgPkgStateSearch> GetBkgPkgStateSearchCriteria(Int32 bkgPkgID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetBkgPkgStateSearchCriteria(bkgPkgID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static Boolean SaveMasterStateSearchCriteria(Int32 tenantID, List<BkgPackageStateSearchContract> bkgPkgStateSearchContract, Int32 currentLoggedInUserID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).SaveMasterStateSearchCriteria(bkgPkgStateSearchContract, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.BkgMasterStateSearch> GetMasterStateSearchCriteria(Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetMasterStateSearchCriteria();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BkgPkgStateSearch> UpdateStateSearchSettingsFromMaster(Int32 tenantID, Int32 currentLoggedInUserID, Int32 bkgPackageID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).UpdateStateSearchSettingsFromMaster(currentLoggedInUserID, bkgPackageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsStateSearchRuleExists(Int32 pkgServiceItemID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).IsStateSearchRuleExists(pkgServiceItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region  Service Forms

        /// <summary>
        ///Get the Service forms associated with a Background Service, along with their
        ///Dispatch type either Manual or Electronic, at the Root(Form) level
        ///Service level and Package Service Mapping level
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="bkgSvcId"></param>
        /// <param name="bpsId"></param>
        /// <returns></returns> 
        public static List<ServiceFormsDispatchTypesContract> GetServiceFormDispatchType(Int32 packageId, Int32 bkgSvcId, Int32 bpsId, Int32 tenantId)
        {
            try
            {
                DataTable _dt = BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetServiceFormDispatchType(packageId, bkgSvcId, bpsId);
                List<ServiceFormsDispatchTypesContract> _lst = new List<ServiceFormsDispatchTypesContract>();

                if (_dt.Rows.Count == 0)
                    return _lst;

                var _bpsoIdColumnName = "BPSOId";
                var _svcFormColumnName = "ServiceFormName";
                var _isRootLevelAutoColumnName = "IsRootLevelAuto";
                var _svcAttachedFormMappingIdColumnName = "ServiceAttachedFormMappingId";
                var _mappingTypeCodeColumnName = "MappingTypeCode";
                var _enforceManualColumnName = "EnforceManual";
                var _isPackageLevelAutomatic = "IsPackageLevelAutomatic";
                var _hideSvcFormColumnName = "HideServiceForm";

                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    _lst.Add(new ServiceFormsDispatchTypesContract
                    {
                        ServiceFormName = Convert.ToString(_dt.Rows[i][_svcFormColumnName]),
                        IsRootLevelAuto = Convert.ToBoolean(_dt.Rows[i][_isRootLevelAutoColumnName]),
                        ServiceAttachedFormMappingId = Convert.ToInt32(_dt.Rows[i][_svcAttachedFormMappingIdColumnName]),
                        MappingTypeCode = Convert.ToString(_dt.Rows[i][_mappingTypeCodeColumnName]),
                        EnforceManual = string.IsNullOrEmpty(Convert.ToString(_dt.Rows[i][_enforceManualColumnName])) ? (Boolean?)null : Convert.ToBoolean(_dt.Rows[i][_enforceManualColumnName]),
                        IsPackageLevelAutomatic = string.IsNullOrEmpty(Convert.ToString(_dt.Rows[i][_isPackageLevelAutomatic])) ? (Boolean?)null : Convert.ToBoolean(_dt.Rows[i][_isPackageLevelAutomatic]),
                        HideServiceForm = string.IsNullOrEmpty(Convert.ToString(_dt.Rows[i][_hideSvcFormColumnName])) ? false : Convert.ToBoolean(_dt.Rows[i][_hideSvcFormColumnName]),
                        BPSOId = Convert.ToInt32(_dt.Rows[i][_bpsoIdColumnName])
                    });
                }

                return _lst;
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
        /// Update the Overriding data when the PacageService mapping is updated
        /// </summary>
        /// <param name="_lstBkgPackageSvcFormOverride"></param>
        /// <param name="bpsId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean UpdateBkgPackageSvcFormOverride(List<BkgPackageSvcFormOverride> _lstBkgPackageSvcFormOverride, Int32 bpsId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateBkgPackageSvcFormOverride(_lstBkgPackageSvcFormOverride, bpsId);
                return true;
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

        public static List<BkgReviewCriteria> FetchMasterReviewCriteria(Int32 selectedTenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(selectedTenantID).FetchMasterReviewCriteria();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool SaveReviewCriteria(Int32 selectedTenantID, BkgReviewCriteria reviewCriteria)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(selectedTenantID).SaveReviewCriteria(reviewCriteria);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static bool UpdateReviewCriteria(Int32 selectedTenantID, BkgReviewCriteria reviewCriteria, Boolean isDeleteMode)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(selectedTenantID).UpdateReviewCriteria(reviewCriteria, isDeleteMode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #region Mapped Review Criteria [UAT-844: Order Review enhancements]
        public static List<BkgReviewCriteriaHierarchyMapping> GetMappedReviewCriteriaList(Int32 instHierarchyNodeId, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetMappedReviewCriteriaList(instHierarchyNodeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveReviewCriteriaMapping(List<Int32> reviewCriteriaIdsToMap, Int32 currentloggedInUserId, Int32 instHierarchyMappingId, Int32 tenantID)
        {
            try
            {
                List<BkgReviewCriteriaHierarchyMapping> reviewCriteriaListToMap = new List<BkgReviewCriteriaHierarchyMapping>();
                foreach (Int32 id in reviewCriteriaIdsToMap)
                {
                    BkgReviewCriteriaHierarchyMapping tempReviewCriteriamapping = new BkgReviewCriteriaHierarchyMapping();
                    tempReviewCriteriamapping.BRCHM_IsDeleted = false;
                    tempReviewCriteriamapping.BRCHM_InstitutionHierarchyNodeID = instHierarchyMappingId;
                    tempReviewCriteriamapping.BRCHM_CreatedBy = currentloggedInUserId;
                    tempReviewCriteriamapping.BRCHM_CreatedOn = DateTime.Now;
                    tempReviewCriteriamapping.BRCHM_BkgReviewCriteriaID = id;
                    reviewCriteriaListToMap.Add(tempReviewCriteriamapping);
                }
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).SaveReviewCriteriaMapping(reviewCriteriaListToMap);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteReviewCriteriaMapping(Int32 currentloggedInUserId, Int32 BRCHM_ID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).DeleteReviewCriteriaMapping(currentloggedInUserId, BRCHM_ID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-844 - ORDER REVIEW ENHANCEMENT

        // UPDATE PACKAGE SERVICE GROUP
        public static Boolean UpdatePackageServiceGroup(Int32 tenantID, BkgPackageSvcGroup bkgPackageSvcGroup, Int32 bkgPackageID, Int32 bkgSvcGroupID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).UpdatePackageServiceGroup(bkgPackageSvcGroup, bkgPackageID, bkgSvcGroupID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //GET PACKAGE SERVICE GROUP DETAILS BY BKG PACKAGEID AND SERVICEGROUPID
        public static BkgPackageSvcGroup GetPkgServiceGroupDetail(Int32 tenantID, Int32 serviceGroupID, Int32 packageID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetPkgServiceGroupDetail(serviceGroupID, packageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-1451:
        /// <summary>
        /// Assign the datatable record in BkgCompliancePackageMappingSearchData 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of BkgCompliancePackageMappingSearchData</returns>
        private static List<BkgCompliancePackageMappingSearchData> AssignValuesToBkgCompliancePackageMappingSearchData(DataTable table)
        {
            try
            {
                List<BkgCompliancePackageMappingSearchData> lstBkgCompPackageMapping = new List<BkgCompliancePackageMappingSearchData>();
                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows.Count() > 0)
                {
                    lstBkgCompPackageMapping = rows.Select(x => new BkgCompliancePackageMappingSearchData
                    {
                        BCPM_ID = x["BCPM_ID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BCPM_ID"]) : 0,
                        BPA_Name = x["BPA_Name"].GetType().Name != "DBNull" ? Convert.ToString(x["BPA_Name"]) : String.Empty,
                        BSG_Name = x["BSG_Name"].GetType().Name != "DBNull" ? Convert.ToString(x["BSG_Name"]) : String.Empty,
                        BSE_Name = x["BSE_Name"].GetType().Name != "DBNull" ? Convert.ToString(x["BSE_Name"]) : String.Empty,
                        BDPT_Name = x["BDPT_Name"].GetType().Name != "DBNull" ? Convert.ToString(x["BDPT_Name"]) : String.Empty,
                        BDPT_Code = x["BDPT_Code"].GetType().Name != "DBNull" ? Convert.ToString(x["BDPT_Code"]) : String.Empty,
                        PackageName = x["PackageName"].GetType().Name != "DBNull" ? Convert.ToString(x["PackageName"]) : String.Empty,
                        CategoryName = x["CategoryName"].GetType().Name != "DBNull" ? Convert.ToString(x["CategoryName"]) : String.Empty,
                        ComplianceItemName = x["ComplianceItemName"].GetType().Name != "DBNull" ? Convert.ToString(x["ComplianceItemName"]) : String.Empty,
                        ComplianceAttributeName = x["ComplianceAttributeName"].GetType().Name != "DBNull" ? Convert.ToString(x["ComplianceAttributeName"]) : String.Empty,
                        BCPM_BkgPackageID = x["BCPM_BkgPackageID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BCPM_BkgPackageID"]) : 0,
                        BCPM_BkgDataPointTypeID = x["BCPM_BkgDataPointTypeID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BCPM_BkgDataPointTypeID"]) : 0,
                        BCPM_BkgSvcGroupID = x["BCPM_BkgSvcGroupID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BCPM_BkgSvcGroupID"]) : 0,
                        BCPM_BkgSvcID = x["BCPM_BkgSvcID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BCPM_BkgSvcID"]) : 0,
                        BCPM_PackageSvcItemID = x["BCPM_PackageSvcItemID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BCPM_PackageSvcItemID"]) : 0,
                        BCPM_CompliancePkgID = x["BCPM_CompliancePkgID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BCPM_CompliancePkgID"]) : 0,
                        BCPM_ComplianceCategoryID = x["BCPM_ComplianceCategoryID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BCPM_ComplianceCategoryID"]) : 0,
                        BCPM_ComplianceItemID = x["BCPM_ComplianceItemID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BCPM_ComplianceItemID"]) : 0,
                        BCPM_ComplianceAttributeID = x["BCPM_ComplianceAttributeID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["BCPM_ComplianceAttributeID"]) : 0,
                        BCPM_CreatedOn = x["BCPM_CreatedOn"].GetType().Name != "DBNull" ? Convert.ToDateTime(x["BCPM_CreatedOn"]) : DateTime.UtcNow, //UAT 3582
                        //TotalCount = x["TotalCount"].GetType().Name != "DBNull" ? Convert.ToInt32(x["TotalCount"]) : 0,

                    }).ToList();
                }

                return lstBkgCompPackageMapping;
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

        #region UAT-1451:Data synch mapping screen is almost unusable as of now (UI updates)
        public static Boolean IsBkgCompDataPointMappingExist(Int32 clientId, Int32? BCPM_ID, BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(clientId).IsBkgCompDataPointMappingExist(BCPM_ID, bkgComplPkgDataMappingContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT 1560 WB: We should be able to add documents that need to be signed to the order process.
        public static List<Entity.SystemDocument> GetBothUploadedDisclosureDocuments(List<Int32> lstDocStatusIDs)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetBothUploadedDisclosureDocuments(lstDocStatusIDs);
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
        public static List<Entity.SystemDocument> GetAdditionalDocuments(List<Int32> backgroundPackageIds, List<Int32> compliancePackageIds, Int32? HierarchyNodeID, Int32 TenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(TenantId).GetAdditionalDocuments(backgroundPackageIds, compliancePackageIds, HierarchyNodeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-1744:Forms filled out at the time of order should be able to pull in data from custom forms within the order.
        /// <summary>
        /// Return the final attributes data from custom form and applicant personal data for filling the form.  
        /// </summary>
        /// <param name="dictionaryApplicantData">Contains the attribute data of'Personal information','Alias','Residential history'</param>
        /// <param name="dictionaryCustomFormData">Containd the data of custom forms</param>
        /// <returns></returns>
        public static Dictionary<Int32, String> GetDandAFormAttributeDataDictionary(Dictionary<Int32, String> dictionaryApplicantData,
                                                                                    Dictionary<Int32, String> dictionaryCustomFormData)
        {
            try
            {
                Dictionary<Int32, String> finalDandAFormData = new Dictionary<Int32, String>();


                dictionaryApplicantData = dictionaryApplicantData.IsNullOrEmpty() ? new Dictionary<Int32, String>() : dictionaryApplicantData;
                dictionaryCustomFormData = dictionaryCustomFormData.IsNullOrEmpty() ? new Dictionary<Int32, String>() : dictionaryCustomFormData;
                //This will add the common data from custom from if 'Attribute Group Mapping ID' exist in both 'dictionaryApplicantData' and 'dictionaryCustomFormData'
                /*For Example:
                 * IF
                 * dictionaryApplicantDat= <1,'ABC'>,<2,'ADD1'>
                 * dictionaryCustomFormData=<1,'DD'>,<3,'ADD2'>
                 * then 
                 * finalDandAFormData=<1,'DD'>
                 */
                finalDandAFormData.AddRange(dictionaryCustomFormData.Where(x => dictionaryApplicantData.ContainsKey(x.Key)));
                //This will add the all the from 'dictionaryApplicantData' except that already added in 'finalDandAFormData'
                /*Now,
                 * finalDandAFormData=<1,'DD'>,<2,'ADD1'>
                 */
                finalDandAFormData.AddRange(dictionaryApplicantData.Where(cnd => !finalDandAFormData.ContainsKey(cnd.Key)));
                //This will add the all the from 'dictionaryCustomFormData' except that already added in 'finalDandAFormData'
                /*Now,
                 * finalDandAFormData=<1,'DD'>,<2,'ADD1'>,<3,'ADD2'>
                 */
                finalDandAFormData.AddRange(dictionaryCustomFormData.Where(cnd => !finalDandAFormData.ContainsKey(cnd.Key)));

                return finalDandAFormData;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<SysDocumentFieldMappingContract> FetchDandRAttributes(ApplicantOrderCart applicantOrderCart, Int32 tenantID,
                                                                                 List<Entity.SystemDocument> DandRDocuments, String applicantDataXml)
        {
            try
            {
                Dictionary<Int32, String> DictAttributeGroupIDs = new Dictionary<Int32, String>();

                DictAttributeGroupIDs = StoredProcedureManagers.GetPricingDataDictionary(tenantID, applicantDataXml);

                //UAT-2326:Change SSN on D&A and Additional Documents to be masked.
                //here we are getting mapping ID of SSN  and if id exists in the list than for that id value has been masked(###-##-0000) and list is updated.
                Int32 BkgAttributeGroupMappingIDforSSN = BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetBkgAttributeGroupMappingIDforSSN();
                if (DictAttributeGroupIDs.Keys.Contains(BkgAttributeGroupMappingIDforSSN))
                {
                    Int32 KeyIndex = DictAttributeGroupIDs.Where(cond => cond.Key == BkgAttributeGroupMappingIDforSSN).Select(sel => sel.Key).FirstOrDefault();
                    String value = GetMaskedSSN(DictAttributeGroupIDs[KeyIndex]);
                    DictAttributeGroupIDs[KeyIndex] = value;
                }


                //MVR Fields
                if (!applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID > 0)
                {
                    Int32 MVRDvrLicenseNumberID = applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID;
                    String LicenceNumber = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.GetValue(MVRDvrLicenseNumberID);
                    DictAttributeGroupIDs.Add(MVRDvrLicenseNumberID, LicenceNumber);
                }
                if (!applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID > 0)
                {
                    Int32 MVRDvrLicenseNumberStateID = applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID;
                    String StateName = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData[0].CustomFormData.GetValue(MVRDvrLicenseNumberStateID);
                    DictAttributeGroupIDs.Add(MVRDvrLicenseNumberStateID, StateName);
                }

                //UAT-1744:Forms filled out at the time of order should be able to pull in data from custom forms within the order.
                if (!applicantOrderCart.lstApplicantOrder[0].IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty())
                {
                    Dictionary<Int32, String> dicCustomFormData = new Dictionary<Int32, String>();

                    //Get Custom Form Data to add in documents.
                    var customFormDataList = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.Where(cnd => cnd.InstanceId == 1);

                    customFormDataList.ForEach(cstFormData =>
                    {
                        dicCustomFormData.AddRange(cstFormData.CustomFormData.Where(cnd => !dicCustomFormData.ContainsKey(cnd.Key)));
                    });

                    DictAttributeGroupIDs = BackgroundSetupManager.GetDandAFormAttributeDataDictionary(DictAttributeGroupIDs, dicCustomFormData);
                }

                List<TypeCustomAttributes> lstCustomAttributes = applicantOrderCart.GetCustomAttributeValues();

                // View.DocumentAttributeMappingList = BackgroundSetupManager.GetFieldNames(View.DictAttributeGroupIDs, View.DandRDocuments);
                List<SysDocumentFieldMappingContract> tempDocumentAttributeMappingList = BackgroundSetupManager.GetFieldNames(DictAttributeGroupIDs, DandRDocuments, lstCustomAttributes, tenantID);
                if (tempDocumentAttributeMappingList.IsNotNull() && tempDocumentAttributeMappingList.Count > AppConsts.NONE)
                {
                    tempDocumentAttributeMappingList.ForEach(cond =>
                    {
                        if (cond.FieldName.ToLower() == AttributeMappingFieldName.DateofBirth.GetStringValue()) cond.FieldValue = !cond.FieldValue.IsNullOrEmpty() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
                        if (cond.FieldName.ToLower() == AttributeMappingFieldName.DigitallySigned.GetStringValue()) cond.FieldValue = !cond.FieldValue.IsNullOrEmpty() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
                        if (cond.FieldName.ToLower() == AttributeMappingFieldName.DateDigitallySigned.GetStringValue()) cond.FieldValue = !cond.FieldValue.IsNullOrEmpty() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
                        if (cond.FieldName.ToLower() == AttributeMappingFieldName.DateSigned.GetStringValue()) cond.FieldValue = !cond.FieldValue.IsNullOrEmpty() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
                        if (cond.FieldName.ToLower() == AttributeMappingFieldName.DatesAtCurrentResidency.GetStringValue()) cond.FieldValue = !cond.FieldValue.IsNullOrEmpty() ? Convert.ToDateTime(cond.FieldValue).ToShortDateString() : cond.FieldValue;
                    });
                }
                return tempDocumentAttributeMappingList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        ///UAT-2326:Change SSN on D&A and Additional Documents to be masked.
        /// <summary>
        /// Method to Masked SSN as ###-##-0000
        /// </summary>
        /// <param name="unMaskedSSN"></param>
        /// <returns></returns>
        public static String GetMaskedSSN(String unMaskedSSN)
        {
            try
            {
                if (unMaskedSSN == null)
                {
                    return String.Empty;
                }
                string value = unMaskedSSN;
                Regex re = new Regex(@"(\d\d\d)(\d\d)(\d\d\d\d)");
                if (re.IsMatch(unMaskedSSN))
                {
                    Match match = re.Match(unMaskedSSN);
                    value = "###-##-" + match.Groups[3];
                }
                return value;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-1834: NYU Migration 2 of 3: Applicant Complete Order Process

        /// <summary>
        /// Get Background Package by Background Package ID and Hierarchy Node ID
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="bkgPackageID"></param>
        /// <param name="orderNodeID"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <returns></returns>
        public static BackgroundPackagesContract GetBackgroundPackageByPkgIDAndNodeID(Int32 tenantId, Int32 bkgPackageID, Int32 orderNodeID, Int32 hierarchyNodeID)
        {

            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetBackgroundPackageByPkgIDAndNodeID(bkgPackageID, orderNodeID, hierarchyNodeID);
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
        ///  Update Order ID and Status in BulkOrderUpload table
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <param name="bulkOrderUploadID"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public static Boolean UpdateBulkOrder(Int32 tenantId, Int32 orderID, Int32 bulkOrderUploadID, Int32 currentUserID)
        {
            try
            {
                var bulkOrderStatus = BulkOrderStatus.OrderPlaced.GetStringValue();
                Int32 bulkOrderStatusID = LookupManager.GetLookUpData<lkpBulkOrderStatu>(tenantId).FirstOrDefault(con => con.BOS_Code == bulkOrderStatus
                                                        && !con.BOS_IsDeleted).BOS_ID;
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateBulkOrder(orderID, bulkOrderUploadID, bulkOrderStatusID, currentUserID);
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


        public static Boolean UpdateLastOrderPlacedDate(Int32 tenantId, Int32 bulkOrderUploadID, Int32 currentUserID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).UpdateLastOrderPlacedDate(bulkOrderUploadID, currentUserID);
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

        public static lkpPackageNotesPosition GetPackageNotesPosition(Int32 tenantId, String selectedPositionCode)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpPackageNotesPosition>(tenantId).Where(x => x.PNP_Code == selectedPositionCode).FirstOrDefault();
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

        #region UAT-2304: Random review of auto completed supplements

        /// <summary>
        /// Get Current Supplement Automation Configuration
        /// </summary>
        /// <returns></returns>
        public static Entity.SupplementAutomationConfiguration GetCurrentSupplementAutomationConfiguration()
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().GetCurrentSupplementAutomationConfiguration();
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
        /// Save/Update Supplement Automation Configuration
        /// </summary>
        /// <param name="supplementAutomationConfiguration"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public static Boolean SaveUpdateSupplementAutomationConfiguration(Entity.SupplementAutomationConfiguration supplementAutomationConfiguration, Int32 currentUserID)
        {
            try
            {
                return BALUtils.GetSecurityRepoInstance().SaveUpdateSupplementAutomationConfiguration(supplementAutomationConfiguration, currentUserID);
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

        #region UAT-2388
        public static List<BackgroundPackage> GetAutomaticInvitationBackgroundPackages(Int32 tenantID, Int32 packageID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetAutomaticInvitationBackgroundPackages(packageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean GetAutomaticPackageInvitationSetting(Int32 tenantID, Int32 packageID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetAutomaticPackageInvitationSetting(packageID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-3268
        public static Boolean GetRotationQualifyingSetting(Int32 tenantId, Int32 packageId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetRotationQualifyingSetting(packageId);
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

        #region UAT-3525
        public static List<BkgPackageType> GetAllBkgPackageTypes(Int32 tenantId, String packageTypeName = "", String packageTypeCode = "", Int32 bkgPackageTypeId = 0, String bkgPackageColorCode = "")
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetAllBkgPackageTypes(packageTypeName, packageTypeCode, bkgPackageTypeId, bkgPackageColorCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static string DeletePackageType(Int32 tenantID, Int32 bkgPackageTypeId, Int32 loggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).DeletePackageType(bkgPackageTypeId, loggedInUserId);
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
        public static Boolean SaveUpdatePackageType(Int32 tenantID, BkgPackageTypeContract _packageTypeContract, Int32 LoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).SaveUpdatePackageType(_packageTypeContract, LoggedInUserId);
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

        public static String GetPackageTypeCode(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).GetPackageTypeCode();
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

        public static Boolean IsPackageMapped(Int32 tenantID, Int32 bkgPackageTypeId, Int32 loggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).IsPackageMapped(bkgPackageTypeId, loggedInUserId);
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

        public static Boolean IsPackageTypeCodeAlreadyExists(Int32 tenantId, String packageTypeCode, Int32 bkgPackageTypeId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).IsPackageTypeCodeAlreadyExists(packageTypeCode, bkgPackageTypeId);
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


        public static Boolean IsPackageTypeNameAlreadyExists(Int32 tenantId, String packageTypeName, Int32 bkgPackageTypeId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantId).IsPackageTypeNameAlreadyExists(packageTypeName, bkgPackageTypeId);
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

        #region UAT-3745
        public static List<Entity.ExternalBkgSvc> GetExternalBkgSvc()
        {
            try
            {
                List<Entity.ExternalVendor> lstExtVendor = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetVendors();
                Int32 vendorID = lstExtVendor.Where(cond => cond.EVE_Code == "AAAA").FirstOrDefault().EVE_ID;
                if (!vendorID.IsNullOrEmpty() && vendorID > AppConsts.NONE)
                    return GetExternalBkgSvcByVendorID(vendorID);
                return new List<Entity.ExternalBkgSvc>();
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

        public static List<SystemDocBkgSvcMapping> GetAddtionalDocBkgSvcMapping(List<Int32> bkgPackagesIds, String additionalDocIds, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(tenantID).GetAddtionalDocBkgSvcMapping(bkgPackagesIds, additionalDocIds);
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

        #region UAT-4004

        public static Entity.ExternalVendorAccount GetExternalVendorAccount(Int32 selectedVendorId, Int32 tenantId, Int32 orderId)
        {
            try
            {
                Order order = ComplianceDataManager.GetOrderDetailsByOrderId(tenantId, orderId);
                Int32 selectedNodeID = !order.IsNullOrEmpty() && order.SelectedNodeID.HasValue ? order.SelectedNodeID.Value : AppConsts.NONE;
                List<DeptProgramMapping> lstDeptProgramMappings = ComplianceDataManager.GetDeptProgramMappingList(tenantId);
                List<Entity.ExternalVendorAccount> lstExternalVendorAccount = BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetExternalVendorAccount(selectedVendorId);
                return GetExternalVendorMapped(selectedVendorId, lstDeptProgramMappings, lstExternalVendorAccount, selectedNodeID);
                //return BALUtils.GetBackgroundSetupRepoInstance(SecurityManager.DefaultTenantID).GetExternalVendorAccount(selectedVendorId);
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
        #region UAT-4775
        public static Boolean SaveContentData(Int32 selectedTenantID, PageContent objPageContent)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(selectedTenantID).SaveContentData(objPageContent);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static int GetContentType(Int32 selectedTenantID, String contentTypeCode)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(selectedTenantID).GetContentType(contentTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static int GetContentRecordType(Int32 selectedTenantID, String contentRecordTypeCode)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(selectedTenantID).GetContentRecordType(contentRecordTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static PageContent GetContentData(Int32 selectedTenantID, Int32 dpmId)
        {
            try
            {
                return BALUtils.GetBackgroundSetupRepoInstance(selectedTenantID).GetContentData(dpmId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Admin Entry Portal (Release-180)

        public static AdminEntryNodeTemplate GetTemplate(Int32 tenantId, Int32 dpmId, String subEventCode)
        {
            try
            {

                AdminEntryNodeTemplate adminEntryNodeTemplate = new AdminEntryNodeTemplate();
                Int32 templateId = AppConsts.NONE;
                Int32 subEventId = AppConsts.NONE;
                Entity.CommunicationTemplate template = new Entity.CommunicationTemplate();
                //Step 1:- Check if Any Template Exists for the given Node Id
                //Get All Templates with given sub eventid/
                //create a method to get list of SystemEventSettings, then check if SystemEventSettings have Any Record with the given Node Id.
                //If yes get the TemplateId and get the data.
                //Else get the templateId which is not in the List of SystemEventSetting templateIds and get data corressponding to it.

                //List<Entity.lkpCommunicationSubEvent> lstSubEvents = LookupManager.GetMessagingLookUpData<Entity.lkpCommunicationSubEvent>().ToList();
                //if (!lstSubEvents.IsNullOrEmpty())
                //    subEventId = lstSubEvents.Where(con => con.IsDeleted != true && con.Code == subEventCode).Select(sel => sel.CommunicationSubEventID).FirstOrDefault();

                subEventId = TemplatesManager.GetSubEventIdByCode(subEventCode);

                if (subEventId > AppConsts.NONE)
                {
                    List<Entity.CommunicationTemplate> lstTemplates = TemplatesManager.GetTemplates(subEventId);
                    List<Entity.SystemEventSetting> lstSystemEventSettings = TemplatesManager.GetSystemEventSettings(subEventId);

                    if (!lstSystemEventSettings.IsNullOrEmpty() && lstSystemEventSettings.Count > AppConsts.NONE)
                    {
                        templateId = lstSystemEventSettings.Where(con => con.InstHierarchyNodeID.HasValue && con.InstHierarchyNodeID == dpmId
                                                                         && con.TenantID.HasValue && con.TenantID == tenantId).Select(sel => sel.CommunicationTemplateID).FirstOrDefault();
                    }

                    //if (templateId <= AppConsts.NONE && !lstTemplates.IsNullOrEmpty() && lstTemplates.Count > AppConsts.NONE)
                    //{
                    //    templateId = lstTemplates.Where(con => !lstSystemEventSettings.Select(sel => sel.CommunicationTemplateID).ToList().Contains(con.CommunicationTemplateID))
                    //                    .Select(Sel => Sel.CommunicationTemplateID).FirstOrDefault();
                    //}

                    if (!lstTemplates.IsNullOrEmpty() && lstTemplates.Count > AppConsts.NONE && templateId > AppConsts.NONE)
                        template = lstTemplates.Where(con => con.CommunicationTemplateID == templateId).FirstOrDefault();
                }

                if (!template.IsNullOrEmpty() && !template.CommunicationTemplateID.IsNullOrEmpty() && template.CommunicationTemplateID > AppConsts.NONE)
                {
                    adminEntryNodeTemplate.TemplateId = template.CommunicationTemplateID;
                    adminEntryNodeTemplate.Subject = template.Subject;
                    adminEntryNodeTemplate.Content = template.Content;
                    adminEntryNodeTemplate.TemplateName = template.Name;
                }

                return adminEntryNodeTemplate;
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

        public static Boolean SaveUpdateAdminEntryNodeTemplate(Int32 tenantId, Int32 dpmId, String subEventCode, Int32 currentLoggedInUserID, AdminEntryNodeTemplate adminEntryNodeTemplate)
        {
            try
            {
                //Step 1 check if isDefaulttemplate // Create a method to check this
                //If Yes 
                //Then Add new CommunicationTemplate and SystemEventSetting
                //Else
                // Update the existing CommunicationTemplate and SystemEventSetting

                Boolean isDefaultTemplate = false;
                Int32 subEventId = AppConsts.NONE;

                //List<Entity.lkpCommunicationSubEvent> lstSubEvents = LookupManager.GetMessagingLookUpData<Entity.lkpCommunicationSubEvent>().ToList();
                //if (!lstSubEvents.IsNullOrEmpty())
                //    subEventId = lstSubEvents.Where(con => con.IsDeleted != true && con.Code == subEventCode).Select(sel => sel.CommunicationSubEventID).FirstOrDefault();

                subEventId = TemplatesManager.GetSubEventIdByCode(subEventCode);

                if (subEventId > AppConsts.NONE)
                {
                    List<Entity.SystemEventSetting> lstSystemEventSettings = TemplatesManager.GetSystemEventSettings(subEventId);

                    isDefaultTemplate = !lstSystemEventSettings.Any(c => c.IsDeleted != true && c.TenantID.HasValue && c.TenantID == tenantId && c.InstHierarchyNodeID.HasValue
                                                     && c.InstHierarchyNodeID == dpmId && c.CommunicationTemplateID == adminEntryNodeTemplate.TemplateId);


                    //update existing 
                    if (!isDefaultTemplate && adminEntryNodeTemplate.TemplateId > AppConsts.NONE)
                    {
                        Entity.CommunicationTemplate communicationTemplate = TemplatesManager.GetTemplates(subEventId).Where(c => c.IsDeleted != true
                                                                                     && c.CommunicationTemplateID == adminEntryNodeTemplate.TemplateId).FirstOrDefault();

                        communicationTemplate.CommunicationSubEventID = subEventId;
                        communicationTemplate.Name = adminEntryNodeTemplate.TemplateName;
                        communicationTemplate.Subject = adminEntryNodeTemplate.Subject;
                        communicationTemplate.Content = adminEntryNodeTemplate.Content;
                        communicationTemplate.ModifiedOn = DateTime.Now;
                        communicationTemplate.ModifiedByID = currentLoggedInUserID;

                        if (TemplatesManager.SaveUpdateTemplate(communicationTemplate))
                        {
                            Entity.SystemEventSetting systemEventSetting = lstSystemEventSettings.Where(c => c.IsDeleted != true && c.TenantID.HasValue && c.TenantID == tenantId && c.InstHierarchyNodeID.HasValue
                                                                             && c.InstHierarchyNodeID == dpmId && c.CommunicationTemplateID == adminEntryNodeTemplate.TemplateId).FirstOrDefault();

                            systemEventSetting.ModifiedBy = currentLoggedInUserID;
                            systemEventSetting.ModifiedOn = DateTime.Now;

                            return TemplatesManager.SaveUpdateSystemEventSetting(systemEventSetting);
                        }

                    }
                    //save new
                    else
                    {
                        Entity.CommunicationTemplate communicationTemplate = new Entity.CommunicationTemplate();
                        String languageCode = Languages.ENGLISH.GetStringValue();
                        Int32 languageId = LookupManager.GetLookUpData<Entity.lkpLanguage>().ToList().Where(con => con.LAN_Code == languageCode && !con.LAN_IsDeleted).Select(sel => sel.LAN_ID).FirstOrDefault();

                        communicationTemplate.CommunicationSubEventID = subEventId;
                        communicationTemplate.Name = adminEntryNodeTemplate.TemplateName;
                        communicationTemplate.Subject = adminEntryNodeTemplate.Subject;
                        //communicationTemplate.Description = null;
                        communicationTemplate.Content = adminEntryNodeTemplate.Content;
                        communicationTemplate.IsDeleted = false;
                        communicationTemplate.CreatedOn = DateTime.Now;
                        communicationTemplate.CreatedByID = currentLoggedInUserID;
                        communicationTemplate.LanguageId = languageId;

                        //Call save/update template
                        if (TemplatesManager.SaveUpdateTemplate(communicationTemplate))
                        {
                            Entity.SystemEventSetting systemEventSetting = new Entity.SystemEventSetting();

                            systemEventSetting.TenantID = tenantId;
                            systemEventSetting.CommunicationTemplateID = communicationTemplate.CommunicationTemplateID;
                            systemEventSetting.CommunicationSubEventID = subEventId;
                            systemEventSetting.IsDeleted = false;
                            systemEventSetting.CreatedBy = currentLoggedInUserID;
                            systemEventSetting.CreatedOn = DateTime.Now;
                            systemEventSetting.IsNotificationBlocked = false;
                            systemEventSetting.InstHierarchyNodeID = dpmId;

                            return TemplatesManager.SaveUpdateSystemEventSetting(systemEventSetting);
                        }
                        //Call save/update systemEventSetting
                    }
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

        #endregion

        #region UAT-4736 :- Link clearstar orders
        private static Entity.ExternalVendorAccount GetExternalVendorMapped(Int32 vendorId, List<DeptProgramMapping> lstDeptProgramMappings, List<Entity.ExternalVendorAccount> lstExternalVendorAccount, Int32 selectedNodeID)
        {
            try
            {
                Entity.ExternalVendorAccount externalVendorAccount = new Entity.ExternalVendorAccount();
                if (!lstDeptProgramMappings.IsNullOrEmpty() && !lstExternalVendorAccount.IsNullOrEmpty() && selectedNodeID > AppConsts.NONE)
                {
                    DeptProgramMapping deptProgramMapping = lstDeptProgramMappings.Where(c => c.DPM_ID == selectedNodeID).FirstOrDefault();

                    if (!deptProgramMapping.InstHierarchyVendorAcctMappings.Where(c => !c.DPMEVAM_IsDeleted).IsNullOrEmpty())
                    {
                        List<Int32> lstExtVendorAcctId = deptProgramMapping.InstHierarchyVendorAcctMappings.Where(c => !c.DPMEVAM_IsDeleted)
                                    .Select(sel => sel.DPMEVAM_ExternalVendorAccountID).ToList();

                        externalVendorAccount = lstExternalVendorAccount.Where(c => lstExtVendorAcctId.Contains(c.EVA_ID) && c.EVA_VendorID == vendorId).FirstOrDefault();
                    }
                    else
                    {
                        //call same method
                        if (deptProgramMapping.DPM_ParentNodeID.HasValue)
                            externalVendorAccount = GetExternalVendorMapped(vendorId, lstDeptProgramMappings, lstExternalVendorAccount, deptProgramMapping.DPM_ParentNodeID.Value);
                    }
                }
                return externalVendorAccount;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion
    }
}

