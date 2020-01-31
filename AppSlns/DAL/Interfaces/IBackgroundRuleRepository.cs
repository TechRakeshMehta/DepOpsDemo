using Entity.ClientEntity;
using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IBackgroundRuleRepository
    {
        #region Manage Ruleset and Rules

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="objectTypeId"></param>
        /// <returns></returns>
        List<BkgRuleSet> GetRuleSetForObject(Int32 objectId, Int32 objectTypeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        BkgRuleSet GetRuleSetInfoByID(Int32 ruleSetId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleSet"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        BkgRuleSet SaveComplianceRuleSetDetail(BkgRuleSet ruleSet, Int32 currentUserId, Int32 tenantId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <param name="currentUserId"></param>
        void DeleteRuleSet(Int32 ruleSetId, Int32 currentUserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bkgRuleSet"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        BkgRuleSet UpdateComplianceRuleSetDetail(BkgRuleSet bkgRuleSet, Int32 currentUserId);

        /// <summary>
        /// Gets all the rows from table RuleMappings.
        /// </summary>
        /// <returns>List of type RuleMapping</returns>
        List<BkgRuleMapping> GetRuleMappings(Int32 ruleSetId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleMappingId"></param>
        /// <param name="currentUserId"></param>
        Boolean DeleteRuleMapping(Int32 ruleMappingId, Int32 currentUserId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rlm_Id"></param>
        /// <param name="getRuleMappingDetails"></param>
        /// <returns></returns>
        BkgRuleMapping GetRuleMapping(Int32 rlm_Id, Boolean getRuleMappingDetails);

        /// <summary>
        /// GetRuleTemplates
        /// </summary>
        /// <returns></returns>
        List<BkgRuleTemplate> GetRuleTemplates();
        
        BkgRuleTemplate GetRuleTemplateDetails(Int32 ruleTemplateId);

        Boolean AddRuleMapping(BkgRuleMapping ruleMapping, Int32 currentLoggedInUserId);

        List<PackageService> getServiceListInPackage(Int32 packageId);

        List<GetAttributeListByPackageId> getAttributeListByPackageId(Int32 packageId);
        #endregion
    }
}
