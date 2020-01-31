using System;
using System.Data;

namespace DAL.Interfaces
{
    public interface IIntegrityRepository
    {
        /// <summary>
        /// check whether package is associated with a category
        /// </summary>
        /// <param name="packageId">Package Id</param>
        /// <returns></returns>
        Boolean IfPackageIsAssociated(Int32 packageId);

        /// <summary>
        /// check whether category is associated with a Package or Item
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Boolean IfCategoryIsAssociated(Int32 categoryId);

        /// <summary>
        /// check whether item is associated with a category or attribute
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Boolean IfItemIsAssociated(Int32 itemId);

        /// <summary>
        /// check whether attribute is associated with a item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Boolean IfAttributeIsAssociated(Int32 attributeId);

        /// <summary>
        /// check whether Rule Template is associated with a Rule
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        Boolean IfRuleTemplateIsAssociated(Int32 ruleTemplateId);


        /// <summary>
        /// Check whether an order has been placed against a package.
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        Boolean ifPackageIsMapped(Int32 packageId);


        /// <summary>
        /// Check whether an order has been placed against any package associated with current category.
        /// </summary>
        /// <param name="categoryid"></param>
        /// <returns></returns>
        Boolean IfAnyPackageAssociatedWithCategoryIsMapped(Int32 categoryId);

        /// <summary>
        /// Check whether an order has been placed against a Package associated with current Item.
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        Boolean IfAnyPackageAssociatedWithItemIsMapped(Int32 itemId);

        /// <summary>
        /// Check whether an order has been placed against a Package associated with current attribute.
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        Boolean IfAnyPackageAssociatedWithAttributeIsMapped(Int32 attributeId);


        /// <summary>
        /// Check if RuleSetObject mapping is associated with any rule.
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        Boolean IfobjectRuleSetMappingIsAssociated(Int32 ruleSetId);

        Boolean IfAnyPackageSubscriptionExistForRule(Int32 ruleMappingId);

        /// <summary>
        /// To check if object is used in any rule.
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <param name="ObjectTypeId"></param>
        /// <param name="ParentObjectId"></param>
        /// <param name="ParentObjectTypeId"></param>
        /// <returns></returns>
        DataTable ifObjectIsUsedWithAnyRule(Int32 ObjectId, Int32 ObjectTypeId, Int32 ParentObjectId, Int32 ParentObjectTypeId, out String seriesDetail);

        #region Hierarchy Package mapping
        Boolean IsPackageHasOrder(Int32 DeptProgramPkgID);
        #endregion
    }
}
