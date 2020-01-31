using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.Repository
{
    public class IntegrityRepository : ClientBaseRepository, IIntegrityRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;

        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public IntegrityRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        /// <summary>
        /// check whether package is associated with a category
        /// </summary>
        /// <param name="packageId">Package Id</param>
        /// <returns></returns>
        public Boolean IfPackageIsAssociated(Int32 packageId)
        {
            return _ClientDBContext.CompliancePackageCategories.Any(condition => condition.CPC_PackageID == packageId && condition.CPC_IsDeleted == false);
        }

        /// <summary>
        /// check whether category is associated with a Package or Item
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Boolean IfCategoryIsAssociated(Int32 categoryId)
        {
            Boolean IfCategoryAssociatedWithPackge = IfCategoryPackageMappingExist(categoryId);
            Boolean ifCategoryAssociatedWithItem = IfICategoryItemMappingExist(categoryId);
            if (IfCategoryAssociatedWithPackge || ifCategoryAssociatedWithItem)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// check whether item is associated with a category or attribute
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Boolean IfItemIsAssociated(Int32 itemId)
        {
            Boolean ifItemAssociatedWithCategory = IfItemCategoryMappingExist(itemId);
            Boolean ifItemAssociatedWithAttribute = IfItemAttributeMappingExist(itemId);
            if (ifItemAssociatedWithCategory || ifItemAssociatedWithAttribute)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// check whether attribute is associated with a item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Boolean IfAttributeIsAssociated(Int32 attributeId)
        {
            return IfAttributeItemMappingExist(attributeId);
        }

        /// <summary>
        /// check whether Rule Template is associated with a Rule
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        public Boolean IfRuleTemplateIsAssociated(Int32 ruleTemplateId)
        {
            if (_ClientDBContext.RuleMappings.Any(condition => condition.RLM_RuleTemplateID == ruleTemplateId && condition.RLM_IsDeleted == false))
            {
                List<Int32> ruleSetIds = _ClientDBContext.RuleMappings.Where(condition => condition.RLM_RuleTemplateID == ruleTemplateId && condition.RLM_IsDeleted == false).Select(x => x.RLM_RuleSetID).ToList();
                return _ClientDBContext.RuleSets.Where(obj => ruleSetIds.Contains(obj.RLS_ID)
                                                        && obj.RLS_IsDeleted == false).Any();
            }
            return false;
        }

        /// <summary>
        /// Check whether an order has been placed against a package.
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public Boolean ifPackageIsMapped(Int32 packageId)
        {
            return _ClientDBContext.DeptProgramPackages.Any(condition => condition.DPP_CompliancePackageID == packageId
                                                                                  && condition.DPP_IsDeleted == false);
            //List<DeptProgramPackage> deptprogrammePackage = _ClientDBContext.DeptProgramPackages.Where(condition => condition.DPP_CompliancePackageID == packageId
            //                                                                        && condition.DPP_IsDeleted == false).ToList();
            //if (deptprogrammePackage.Count > 0)
            //{
            //    List<Int32> deptprogrammePackageIds = _ClientDBContext.DeptProgramPackages.Where(condition => condition.DPP_CompliancePackageID == packageId
            //                                                                        && condition.DPP_IsDeleted == false).Select(x => x.DPP_ID).ToList();
            //    if (deptprogrammePackageIds.Count > 0)
            //    {
            //        return _ClientDBContext.Orders.Where(obj => deptprogrammePackageIds.Contains(obj.DeptProgramPackageID) && obj.IsDeleted == false).Any();
            //    }
            //}
            //return false;
        }

        /// <summary>
        /// Check whether an order has been placed against any package associated with current category.
        /// </summary>
        /// <param name="categoryid"></param>
        /// <returns></returns>
        public Boolean IfAnyPackageAssociatedWithCategoryIsMapped(Int32 categoryId)
        {
            List<CompliancePackageCategory> associatedPackages = _ClientDBContext.CompliancePackageCategories.Where(condition => condition.CPC_CategoryID == categoryId
                                                                 && condition.CPC_IsDeleted == false).ToList();

            if (associatedPackages != null && associatedPackages.Count > 0)
            {
                return IfMappingExistForPackageList(associatedPackages.Select(x => x.CPC_PackageID).ToList());
            }
            return false;
        }

        /// <summary>
        /// Check whether an order has been placed against a Package associated with current Item.
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public Boolean IfAnyPackageAssociatedWithItemIsMapped(Int32 itemid)
        {
            List<Int32> associatedPackagesIds = (from compCatItem in _ClientDBContext.ComplianceCategoryItems
                                                 join compCat in _ClientDBContext.ComplianceCategories
                                                 on compCatItem.CCI_CategoryID equals compCat.ComplianceCategoryID
                                                 join compPakCat in _ClientDBContext.CompliancePackageCategories
                                                 on compCat.ComplianceCategoryID equals compPakCat.CPC_CategoryID
                                                 join compPak in _ClientDBContext.CompliancePackages
                                                 on compPakCat.CPC_PackageID equals compPak.CompliancePackageID
                                                 where compCatItem.CCI_ItemID == itemid && compCatItem.CCI_IsDeleted == false
                                                 && compPakCat.CPC_IsDeleted == false && compPak.IsDeleted == false
                                                 select compPak.CompliancePackageID).ToList();
            if (associatedPackagesIds.Count() > 0)
            {
                return IfMappingExistForPackageList(associatedPackagesIds);

            }
            return false;
        }

        /// <summary>
        /// Check whether an order has been placed against a Package associated with current attribute.
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public Boolean IfAnyPackageAssociatedWithAttributeIsMapped(Int32 attributeId)
        {
            List<Int32> associatedPackagesIds = (from compItmAttr in _ClientDBContext.ComplianceItemAttributes
                                                 join compItm in _ClientDBContext.ComplianceItems
                                                 on compItmAttr.CIA_ItemID equals compItm.ComplianceItemID
                                                 join compCatItem in _ClientDBContext.ComplianceCategoryItems
                                                 on compItm.ComplianceItemID equals compCatItem.CCI_ItemID
                                                 join compCat in _ClientDBContext.ComplianceCategories
                                                 on compCatItem.CCI_CategoryID equals compCat.ComplianceCategoryID
                                                 join compPakCat in _ClientDBContext.CompliancePackageCategories
                                                 on compCat.ComplianceCategoryID equals compPakCat.CPC_CategoryID
                                                 join compPak in _ClientDBContext.CompliancePackages
                                                 on compPakCat.CPC_PackageID equals compPak.CompliancePackageID
                                                 where compItmAttr.CIA_AttributeID == attributeId && compCatItem.CCI_IsDeleted == false
                                                 && compPakCat.CPC_IsDeleted == false && compPak.IsDeleted == false
                                                 select compPak.CompliancePackageID).ToList();
            if (associatedPackagesIds.Count() > 0)
            {
                return IfMappingExistForPackageList(associatedPackagesIds);
            }
            return false;
        }

        /// <summary>
        /// Check if RuleSetObject mapping is associated with any rule.
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        public Boolean IfobjectRuleSetMappingIsAssociated(Int32 ruleSetId)
        {
            return IfRuleSetRuleMappingExist(ruleSetId);
        }

        public Boolean IfAnyPackageSubscriptionExistForRule(Int32 ruleMappingId)
        {
            String code = LkpSubscriptionMobilityStatus.MobilitySwitched;
            Int32 subscriptionMobilityStatusID = ClientDBContext.lkpSubscriptionMobilityStatus.Where(item => !item.IsDeleted && item.Code.Equals(code)).FirstOrDefault().SubscriptionMobilityStatusID;
            List<PackageSubscriptionRule> activeSubscriptions = ClientDBContext.PackageSubscriptionRules.Where(x => x.PSR_RuleMappingID == ruleMappingId
                                                                                                        && !x.PSR_IsDeleted && !x.PackageSubscription.IsDeleted
                                                                                                        && (x.PackageSubscription.SubscriptionMobilityStatusID == null || x.PackageSubscription.SubscriptionMobilityStatusID != subscriptionMobilityStatusID)).ToList();
            if (activeSubscriptions.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// To check if object is used in any rule.
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <param name="ObjectTypeId"></param>
        /// <param name="ParentObjectId"></param>
        /// <param name="ParentObjectTypeId"></param>
        /// <returns></returns>
        public DataTable ifObjectIsUsedWithAnyRule(Int32 ObjectId, Int32 ObjectTypeId, Int32 ParentObjectId, Int32 ParentObjectTypeId, out String seriesDetail)
        {
            seriesDetail = String.Empty;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_IfObjectIsUsedInRule", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ParentObjectId", ParentObjectId);
                command.Parameters.AddWithValue("@ParentObjectTypeId", ParentObjectTypeId);
                command.Parameters.AddWithValue("@ObjectId", ObjectId);
                command.Parameters.AddWithValue("@ObjectTypeId", ObjectTypeId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > 1)
                    {
                        StringBuilder tmpObjectNames = new StringBuilder();

                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            tmpObjectNames.Append(row[0].ToString());
                            tmpObjectNames.Append(", ");
                        }
                        if (tmpObjectNames.Length > AppConsts.NONE)
                        {
                            tmpObjectNames.Length = tmpObjectNames.Length - 2;
                            seriesDetail = tmpObjectNames.ToString();
                        }
                    }
                    return ds.Tables[0]; //.Rows[0]["Result"].ToString();
                }
            }
            return new DataTable();
        }

        #region Common Methods

        /// <summary>
        /// Check whether an order has been placed against a package List.
        /// </summary>
        /// <param name="associatedPackages"></param>
        /// <returns></returns>
        public Boolean IfMappingExistForPackageList(List<Int32> associatedPackages)
        {
            return _ClientDBContext.DeptProgramPackages.Any(condition => associatedPackages.Contains(condition.DPP_CompliancePackageID)
                                                                                    && condition.DPP_IsDeleted == false);
        }

        /// <summary>
        /// Check whether for a given category any PackageCategory mapping exist.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Boolean IfCategoryPackageMappingExist(Int32 categoryId)
        {
            return ClientDBContext.CompliancePackageCategories.Any(condition => condition.CPC_CategoryID == categoryId && condition.CPC_IsDeleted == false);
        }

        /// <summary>
        /// Check whether for a given category any CategoryItem mapping exist.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Boolean IfICategoryItemMappingExist(Int32 categoryId)
        {
            return _ClientDBContext.ComplianceCategoryItems.Any(condition => condition.CCI_CategoryID == categoryId && condition.CCI_IsDeleted == false);
        }

        /// <summary>
        /// Check whether for a given item any CategoryItem mapping exist.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Boolean IfItemCategoryMappingExist(Int32 itemId)
        {
            return _ClientDBContext.ComplianceCategoryItems.Any(condition => condition.CCI_ItemID == itemId && condition.CCI_IsDeleted == false);
        }

        /// <summary>
        /// Check whether for a given item any ItemAttribute mapping exist.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Boolean IfItemAttributeMappingExist(Int32 itemId)
        {
            return _ClientDBContext.ComplianceItemAttributes.Any(condition => condition.CIA_ItemID == itemId && condition.CIA_IsDeleted == false);
        }

        /// <summary>
        /// Check whether for a given attrbute any ItemAttribute mapping exist.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Boolean IfAttributeItemMappingExist(Int32 attributeId)
        {
            return _ClientDBContext.ComplianceItemAttributes.Any(condition => condition.CIA_AttributeID == attributeId && condition.CIA_IsDeleted == false);
        }

        /// <summary>
        /// Check whether for a given ruleset any RuleSetRule mapping exist.
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        public Boolean IfRuleSetRuleMappingExist(Int32 ruleSetId)
        {
            return _ClientDBContext.RuleMappings.Any(condition => condition.RLM_RuleSetID == ruleSetId && condition.RLM_IsDeleted == false);
        }

        #endregion

        #region Hierarchy Package mapping
        public Boolean IsPackageHasOrder(Int32 DeptProgramPkgID)
        {
            //has order you can't delete 
            return ClientDBContext.Orders.Where(x => x.DeptProgramPackageID == DeptProgramPkgID && !x.IsDeleted).Any();
        }

        #endregion
    }
}
