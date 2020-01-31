using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.RepoManagers
{
    public static class BackgroungRuleManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static BackgroungRuleManager()
        {
            BALUtils.ClassModule = "BkgRuleManager";
        }

        #endregion

        #region Public Methods

        #region Manage Ruleset

        /// <summary>
        /// Gets the list of rule set for the selected object.
        /// </summary>
        /// <param name="associationHierarchyId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="tenantId">TenantId</param>
        /// <returns>List of RuleSet</returns>
        public static List<BkgRuleSet> GetRuleSetForObject(Int32 objectId, Int32 objectTypeId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).GetRuleSetForObject(objectId, objectTypeId);
            }
            catch (SysXException ex)
            {
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
        /// Gets the rule set for the given Rule Set ID.
        /// </summary>
        /// <param name="ruleSetId">Rule Set Id</param>
        /// <param name="tenantId">TenantId</param>
        /// <returns>BkgRuleSet entity</returns>
        public static BkgRuleSet GetRuleSetInfoByID(Int32 ruleSetId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).GetRuleSetInfoByID(ruleSetId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static BkgRuleSet SaveComplianceRuleSetDetail(BkgRuleSet ruleSet, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BkgRuleSet resultRuleSet = BALUtils.GetBackgroundRuleRepoInstance(ruleSet.BRLS_TenantID.Value).SaveComplianceRuleSetDetail(ruleSet, currentUserId, ruleSet.BRLS_TenantID.Value);
                return resultRuleSet;
            }
            catch (SysXException ex)
            {
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
        /// Deletes the rule set and object mapping.
        /// </summary>
        /// <param name="ruleSetId">RuleSet Id</param>
        /// <param name="objectId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="currentUserId">Current User Id</param>
        /// <param name="tenantId">TenantId</param>
        public static void DeleteRuleSet(Int32 ruleSetId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                BALUtils.GetBackgroundRuleRepoInstance(tenantId).DeleteRuleSet(ruleSetId, currentUserId);
            }
            catch (SysXException ex)
            {
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
        /// 
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static BkgRuleSet UpdateComplianceRuleSetDetail(BkgRuleSet ruleSet, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).UpdateComplianceRuleSetDetail(ruleSet, currentUserId);
            }
            catch (SysXException ex)
            {
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

        #region Manage Rules
        /// Gets all the rows from table RuleMappings.
        /// </summary>
        /// <returns>List of type RuleMapping</returns>
        public static List<BkgRuleMapping> GetRuleMappings(Int32 ruleSetId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).GetRuleMappings(ruleSetId);
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

        public static Boolean DeleteRuleMapping(Int32 ruleMappingId, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).DeleteRuleMapping(ruleMappingId, currentUserId);
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

        public static Boolean AddRuleMapping(BkgRuleMapping ruleMapping, Int32 tenantId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).AddRuleMapping(ruleMapping, currentLoggedInUserId);
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

        public static BkgRuleMapping GetRuleMapping(Int32 rlm_Id, Int32 tenantId, Boolean getRuleMappingDetails = false)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).GetRuleMapping(rlm_Id, getRuleMappingDetails);
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

        public static List<BkgRuleTemplate> GetRuleTemplateList(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).GetRuleTemplates();
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

        public static BkgRuleTemplate GetRuleTemplateDetails(Int32 ruleTemplateId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).GetRuleTemplateDetails(ruleTemplateId);
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
        /// Gets the list of all Rule Types.
        /// </summary>
        /// <returns>
        ///  List of lkpRuleType Objects.
        /// </returns>
        public static List<lkpBkgRuleType> GetRuleTypes(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpBkgRuleType>(tenantId).OrderBy(x=>x.BRLT_Description).ToList();
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
        /// Gets the list of all Rule Action Types.
        /// </summary>
        /// <returns>
        /// IQueryable List of lkpRuleActionType Objects.
        /// </returns>
        public static List<lkpBkgRuleActionType> GetRuleActionTypes(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpBkgRuleActionType>(tenantId).ToList();
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

        public static Int32 getObjectTypeIdByCode(String objectCode, Int32 tenantID)
        {
            try
            {
                lkpBkgObjectType objectType = LookupManager.GetLookUpData<lkpBkgObjectType>(tenantID).FirstOrDefault(ot => ot.BOT_Code == objectCode && !ot.BOT_IsDeleted);
                if (objectType != null)
                {
                    return objectType.BOT_ID;
                }
                return AppConsts.NONE;
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

        public static List<lkpBkgConstantType> getConstantType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpBkgConstantType>(tenantId).Where(obj => obj.BCT_IsDeleted == false).ToList();
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

        public static List<lkpBkgRuleObjectMappingType> getRuleObjectMappingType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpBkgRuleObjectMappingType>(tenantId).Where(obj => obj.BRMT_IsDeleted == false).ToList();
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

        public static List<lkpBkgObjectType> getBkgObjectType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpBkgObjectType>(tenantId).Where(obj => obj.BOT_IsDeleted == false).ToList();
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

        public static List<PackageService> getServiceListInPackage(Int32 packageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).getServiceListInPackage(packageId);
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

        public static List<GetAttributeListByPackageId> getAttributeListByPackageId(Int32 packageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundRuleRepoInstance(tenantId).getAttributeListByPackageId(packageId);
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

        public static Dictionary<String, String> getCountryList()
        {
            try
            {
                List<Entity.Country> allCountries = LookupManager.GetLookUpData<Entity.Country>().ToList();
                List<Entity.Country> selectedCountry = allCountries.Where(x => x.Alpha2Code == "US" || x.Alpha2Code == "CA").OrderByDescending(x => x.Alpha2Code).ToList();
                allCountries.RemoveAll(x => x.Alpha2Code == "US" || x.Alpha2Code == "CA");
                selectedCountry.AddRange(allCountries);
                return selectedCountry.ToDictionary(x => x.FullName, x => x.FullName);
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

        public static Dictionary<String, String> getStateList()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.State>().Where(obj => obj.IsActive && obj.StateID>AppConsts.NONE).OrderBy(x => x.StateName)
                    .ToDictionary(x => x.StateName, x => x.StateName);
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

        public static Dictionary<String, String> getCountyList()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.County>().OrderBy(x => x.CountyName).DistinctBy(x => x.CountyName)
                    .ToDictionary(x => x.CountyName, x => x.CountyName);
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

        public static Dictionary<String, String> getCityList()
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.City>().OrderBy(x => x.CityName).DistinctBy(x => x.CityName)
                    .ToDictionary(x => x.CityName, x => x.CityName);
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
