using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class BackgroundRuleRepository : ClientBaseRepository, IBackgroundRuleRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;

        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public BackgroundRuleRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        #region Manage Ruleset

        /// <summary>
        /// Gets the list of rule set for the selected object.
        /// </summary>
        /// <param name="associationHierarchyId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <returns>List of RuleSet</returns>
        public List<BkgRuleSet> GetRuleSetForObject(Int32 objectId, Int32 objectTypeId)
        {
            return ClientDBContext.BkgRuleSets.Where(obj => obj.BRLS_ObjectTypeID == objectTypeId && obj.BRLS_ObjectID == objectId
                                                        && obj.BRLS_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Gets the rule set for the given Rule Set ID.
        /// </summary>
        /// <param name="ruleSetId">Rule Set Id</param>
        /// <returns>BkgRuleSet entity</returns>
        public BkgRuleSet GetRuleSetInfoByID(Int32 ruleSetId)
        {
            return ClientDBContext.BkgRuleSets.FirstOrDefault(obj => obj.BRLS_ID == ruleSetId && obj.BRLS_IsDeleted == false);
        }

        public BkgRuleSet SaveComplianceRuleSetDetail(BkgRuleSet ruleSet, Int32 currentUserId, Int32 tenantId)
        {
            ruleSet.BRLS_Code = ruleSet.BRLS_Code.Equals(Guid.Empty) ? Guid.NewGuid() : ruleSet.BRLS_Code;
            ruleSet.BRLS_IsDeleted = false;
            ruleSet.BRLS_CreatedByID = currentUserId;
            ruleSet.BRLS_CreatedOn = DateTime.Now;
            ClientDBContext.BkgRuleSets.AddObject(ruleSet);
            ClientDBContext.SaveChanges();
            return ruleSet;
        }

        /// <summary>
        /// Deletes the rule set and object mapping.
        /// </summary>
        /// <param name="ruleSetId">RuleSet Id</param>
        /// <param name="objectId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="currentUserId">Current User Id</param>
        public void DeleteRuleSet(Int32 ruleSetId, Int32 currentUserId)
        {
            BkgRuleSet existingRuleSet = _ClientDBContext.BkgRuleSets.Where(obj => obj.BRLS_ID == ruleSetId && obj.BRLS_IsDeleted == false).FirstOrDefault();
            existingRuleSet.BRLS_IsDeleted = true;
            existingRuleSet.BRLS_ModifiedByID = currentUserId;
            existingRuleSet.BRLS_ModifiedOn = DateTime.Now;
            _ClientDBContext.SaveChanges();
        }

        public BkgRuleSet UpdateComplianceRuleSetDetail(BkgRuleSet bkgRuleSet, Int32 currentUserId)
        {
            BkgRuleSet complianceRuleSet = _ClientDBContext.BkgRuleSets.FirstOrDefault(obj => obj.BRLS_ID == bkgRuleSet.BRLS_ID && obj.BRLS_IsDeleted == false);
            complianceRuleSet.BRLS_ID = bkgRuleSet.BRLS_ID;
            complianceRuleSet.BRLS_Name = bkgRuleSet.BRLS_Name;
            complianceRuleSet.BRLS_Description = bkgRuleSet.BRLS_Description;
            complianceRuleSet.BRLS_IsActive = bkgRuleSet.BRLS_IsActive;
            complianceRuleSet.BRLS_IsDeleted = false;
            complianceRuleSet.BRLS_ModifiedByID = currentUserId;
            complianceRuleSet.BRLS_ModifiedOn = DateTime.Now;
            _ClientDBContext.SaveChanges();
            return complianceRuleSet;
        }

        #endregion

        #region Manage Rules
        /// <summary>
        /// Gets all the rows from table RuleMappings.
        /// </summary>
        /// <returns>List of type RuleMapping</returns>
        public List<BkgRuleMapping> GetRuleMappings(Int32 ruleSetId)
        {
            return ClientDBContext.BkgRuleMappings.Where(rm => rm.BRLM_RuleSetID == ruleSetId && !rm.BRLM_IsDeleted).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleMappingId"></param>
        /// <param name="currentUserId"></param>
        public Boolean DeleteRuleMapping(Int32 ruleMappingId, Int32 currentUserId)
        {
            BkgRuleMapping ruleMapping = ClientDBContext.BkgRuleMappings.Where(rm => rm.BRLM_ID == ruleMappingId).FirstOrDefault();
            if (ruleMapping != null)
            {

                ruleMapping.BRLM_IsDeleted = true;
                ruleMapping.BRLM_ModifiedOn = DateTime.Now;
                ruleMapping.BRLM_ModifiedByID = currentUserId;
                foreach (BkgRuleMappingDetail mapping in ruleMapping.BkgRuleMappingDetails)
                {
                    mapping.BRLMD_IsDeleted = true;
                    mapping.BRLMD_ModifiedOn = DateTime.Now;
                    mapping.BRLMD_ModifiedByID = currentUserId;
                }
                ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        public BkgRuleMapping GetRuleMapping(Int32 ruleMappingId, Boolean getRuleMappingDetails)
        {
            if (getRuleMappingDetails)
            {
                return ClientDBContext.BkgRuleMappings
                     .Include("BkgRuleMappingDetails")
                     .Include("BkgRuleMappingDetails.lkpbkgObjectType")
                     .FirstOrDefault(x => x.BRLM_ID == ruleMappingId);
            }
            else
            {
                return ClientDBContext.BkgRuleMappings.FirstOrDefault(x => x.BRLM_ID == ruleMappingId);
            }
        }

        /// <summary>
        /// GetRuleTemplates
        /// </summary>
        /// <returns></returns>
        List<BkgRuleTemplate> IBackgroundRuleRepository.GetRuleTemplates()
        {
            return ClientDBContext.BkgRuleTemplates.Include("lkpBkgRuleResultType").Where(r => r.BRLT_IsDeleted == false).ToList();
        }

        public BkgRuleTemplate GetRuleTemplateDetails(Int32 ruleTemplateId)
        {
            return ClientDBContext.BkgRuleTemplates.Include("BkgRuleTemplateExpressions.BkgExpression").Where(r => r.BRLT_ID == ruleTemplateId).FirstOrDefault();
        }

        public Boolean AddRuleMapping(BkgRuleMapping ruleMapping, Int32 currentLoggedInUserId)
        {
            if (ruleMapping.BRLM_ID == AppConsts.NONE)
            {
                ruleMapping.BRLM_CreatedByID = currentLoggedInUserId;
                ruleMapping.BRLM_CreatedOn = DateTime.Now;
                ruleMapping.BRLM_Code = Guid.NewGuid();
                ClientDBContext.BkgRuleMappings.AddObject(ruleMapping);
                ClientDBContext.SaveChanges();
                return true;
            }
            else
            {
                BkgRuleMapping ruleMappingInDb = GetRuleMapping(ruleMapping.BRLM_ID, true);
                if (ruleMappingInDb != null)
                {
                    ruleMappingInDb.BRLM_RuleTemplateID = ruleMapping.BRLM_RuleTemplateID;
                    ruleMappingInDb.BRLM_Name = ruleMapping.BRLM_Name;
                    ruleMappingInDb.BRLM_ActionBlock = ruleMapping.BRLM_ActionBlock;
                    ruleMappingInDb.BRLM_UIExpression = ruleMapping.BRLM_UIExpression;
                    ruleMappingInDb.BRLM_SuccessMessage = ruleMapping.BRLM_SuccessMessage;
                    ruleMappingInDb.BRLM_ErrorMessage = ruleMapping.BRLM_ErrorMessage;
                    ruleMappingInDb.BRLM_ActionType = ruleMapping.BRLM_ActionType;
                    ruleMappingInDb.BRLM_RuleType = ruleMapping.BRLM_RuleType;
                    ruleMappingInDb.BRLM_IsActive = ruleMapping.BRLM_IsActive;
                    ruleMappingInDb.BRLM_IsDeleted = ruleMapping.BRLM_IsDeleted;
                    ruleMappingInDb.BRLM_ModifiedByID = currentLoggedInUserId;
                    ruleMappingInDb.BRLM_ModifiedOn = DateTime.Now;

                    ruleMappingInDb.BRLM_IsCurrent = ruleMapping.BRLM_IsCurrent;
                    foreach (BkgRuleMappingDetail mappingDetail in ruleMapping.BkgRuleMappingDetails)
                    {
                        BkgRuleMappingDetail existingMapping = ruleMappingInDb.BkgRuleMappingDetails.FirstOrDefault(obj => obj.BRLMD_PlaceHolderName == mappingDetail.BRLMD_PlaceHolderName);
                        if (existingMapping != null)
                        {
                            existingMapping.BRLMD_PlaceHolderName = mappingDetail.BRLMD_PlaceHolderName;
                            existingMapping.BRLMD_ObjectID = mappingDetail.BRLMD_ObjectID;
                            existingMapping.BRLMD_ObjectTypeID = mappingDetail.BRLMD_ObjectTypeID;
                            existingMapping.BRLMD_RuleObjectMappingTypeID = mappingDetail.BRLMD_RuleObjectMappingTypeID;
                            existingMapping.BRLMD_ConstantType = mappingDetail.BRLMD_ConstantType;
                            existingMapping.BRLMD_ConstantValue = mappingDetail.BRLMD_ConstantValue;
                            existingMapping.BRLMD_ModifiedByID = mappingDetail.BRLMD_ModifiedByID;
                            existingMapping.BRLMD_ModifiedOn = DateTime.Now;
                            existingMapping.BRLMD_IsDeleted = false;
                        }

                    }
                }
                ClientDBContext.SaveChanges();
                return true;
            }
        }

        public List<PackageService> getServiceListInPackage(Int32 packageId)
        {
            return ClientDBContext.BkgPackageSvcs.Include("BkgPackageSvcGroup").Include("BackgroundService").
                Where(x => (x.BkgPackageSvcGroup.BPSG_BackgroundPackageID == packageId && x.BPS_IsDeleted == false && x.BkgPackageSvcGroup.BPSG_IsDeleted == false && x.BackgroundService.BSE_IsDeleted == false)
                )
                 .Select(x => new PackageService
                 {
                     BkgPackageSrvcId = x.BPS_ID,
                     ServiceName = x.BackgroundService.BSE_Name
                 }).ToList();
        }

        public List<GetAttributeListByPackageId> getAttributeListByPackageId(Int32 packageId)
        {
            return ClientDBContext.usp_GetAttributeListByPackageId(packageId).ToList();
        }
        #endregion


    }
}
