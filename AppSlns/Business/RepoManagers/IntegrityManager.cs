using INTSOF.Utils;
using System;
using System.Data;
using System.Text;

namespace Business.RepoManagers
{
    public static class IntegrityManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static IntegrityManager()
        {
            BALUtils.ClassModule = "IntegrityManager";
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean IsDefaultTenant(Int32 tenantId)
        {
            try
            {
                return tenantId.Equals(SecurityManager.DefaultTenantID);
            }
            catch (SysXException ex)
            {
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
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean IsClientAdmin(Int32 tenantId)
        {
            try
            {
                return SecurityManager.IsTenantThirdPartyType(tenantId, TenantType.Institution.GetStringValue());
            }
            catch (SysXException ex)
            {
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
        /// <param name="packageId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfPackageCanBeDeleted(Int32 packageId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfPackageIsAssociated(packageId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot remove package {0} as it is associated with other objects.";
                    }
                }
                if (IsClientAdmin(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).ifPackageIsMapped(packageId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete package {0} as it is in use.";
                    }
                    else if (BALUtils.GetIntegrityRepoInstance(tenantId).IfPackageIsAssociated(packageId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot remove package {0} as it is associated with other objects.";
                    }
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

        public static IntegrityCheckResponse IfPackageCanBeUpdated(Int32 packageId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (IsDefaultTenant(tenantId))
                {

                }
                if (IsClientAdmin(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).ifPackageIsMapped(packageId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot update package {0} as it is in use.";
                    }
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
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfCategoryCanBeDeleted(Int32 categoryId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfCategoryIsAssociated(categoryId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete category {0} as it is associated with other objects.";
                    }
                }
                if (IsClientAdmin(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAnyPackageAssociatedWithCategoryIsMapped(categoryId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete category {0} as it is in use.";
                    }
                    else if (BALUtils.GetIntegrityRepoInstance(tenantId).IfCategoryIsAssociated(categoryId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete category {0} as it is associated with other objects.";
                    }
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

        public static IntegrityCheckResponse IfCategoryCanBeUpdated(Int32 categoryId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    //no condition for updation.
                }
                if (IsClientAdmin(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAnyPackageAssociatedWithCategoryIsMapped(categoryId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot update Category {0} as it is in use.";
                    }
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
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfItemCanBeDeleted(Int32 itemId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfItemIsAssociated(itemId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete item {0} as it is associated with other objects.";
                    }
                }
                if (IsClientAdmin(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAnyPackageAssociatedWithItemIsMapped(itemId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete item {0} as it is in use.";
                    }
                    else if (BALUtils.GetIntegrityRepoInstance(tenantId).IfItemIsAssociated(itemId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete item {0} as it is associated with other objects.";
                    }
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

        public static IntegrityCheckResponse IfItemCanBeUpdated(Int32 itemId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    //no condition for updation.
                }
                if (IsClientAdmin(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAnyPackageAssociatedWithItemIsMapped(itemId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot update item {0} as it is in use.";
                    }
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
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfAttributeCanBeDeleted(Int32 attributeId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAttributeIsAssociated(attributeId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete attribute {0} as it is associated with other objects.";
                    }
                }
                if (IsClientAdmin(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAnyPackageAssociatedWithAttributeIsMapped(attributeId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete attribute {0} as it is in use.";
                    }
                    else if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAttributeIsAssociated(attributeId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete attribute {0} as it is associated with other objects.";
                    }
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

        public static IntegrityCheckResponse IfAttributeCanBeUpdated(Int32 attributeId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    //no condition for updation.
                }
                if (IsClientAdmin(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAnyPackageAssociatedWithAttributeIsMapped(attributeId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot update attribute {0} as it is in use.";
                    }
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
        /// 
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
                //if (IsDefaultTenant(tenantId))
                //{
                if (BALUtils.GetIntegrityRepoInstance(tenantId).IfRuleTemplateIsAssociated(ruleTemplateSetId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = "You cannot delete {0} Template as it is associated with other objects.";
                }
                //}
                //if (IsClientAdmin(tenantId))
                //{
                //}

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
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="packageId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfPackageCategoryMappingCanBeDeleted(Int32 categoryId, Int32 packageId, Int32 tenantId, Int32 objectTypeIdForPackage, Int32 objectTypeIdForCategory)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                String tempResult = String.Empty;
                DataTable Result = new DataTable();
                StringBuilder tmpObjectNames = new StringBuilder();
                String seriesDetail = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    Result = BALUtils.GetIntegrityRepoInstance(tenantId).ifObjectIsUsedWithAnyRule(categoryId, objectTypeIdForCategory, packageId, objectTypeIdForPackage, out seriesDetail);
                }
                if (IsClientAdmin(tenantId))
                {
                    Result = BALUtils.GetIntegrityRepoInstance(tenantId).ifObjectIsUsedWithAnyRule(categoryId, objectTypeIdForCategory, packageId, objectTypeIdForPackage, out seriesDetail);
                }
                if (Result.Rows.Count > 0)
                {
                    //Object is mapped
                    response.CheckStatus = CheckStatus.True;
                    String objectMappingHierarchy = GetObjectMappingHierarchy(Result);
                    response.UIMessage = "You cannot remove category {0} as it is used in rules associated with " + objectMappingHierarchy;
                  
                }
                if (!seriesDetail.IsNullOrEmpty())
                {
                    response.CheckStatus = CheckStatus.True;
                    if (response.UIMessage == String.Empty)
                        response.UIMessage = "You cannot remove item {0} as it is mapped with following series: " + seriesDetail;
                    else
                        response.UIMessage = response.UIMessage + "and mapped with following series:" + seriesDetail;
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
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        ///  <param name="parentCategoryId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfCategoryItemMappingCanBeDeleted(Int32 itemId, Int32 parentCategoryId, Int32 tenantId, Int32 objectTypeIdForItem, Int32 objectTypeIdForCategory)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                String tempResult = String.Empty;
                DataTable Result = new DataTable();
                StringBuilder tmpObjectNames = new StringBuilder();
                String seriesDetail = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    Result = BALUtils.GetIntegrityRepoInstance(tenantId).ifObjectIsUsedWithAnyRule(itemId, objectTypeIdForItem, parentCategoryId, objectTypeIdForCategory, out seriesDetail);
                }
                if (IsClientAdmin(tenantId))
                {
                    Result = BALUtils.GetIntegrityRepoInstance(tenantId).ifObjectIsUsedWithAnyRule(itemId, objectTypeIdForItem, parentCategoryId, objectTypeIdForCategory, out seriesDetail);
                }
                if (Result.Rows.Count > 0)
                {
                    //Object is mapped
                    response.CheckStatus = CheckStatus.True;
                    String objectMappingHierarchy = GetObjectMappingHierarchy(Result);
                    response.UIMessage = "You cannot remove item {0} as it is used in rules associated with " + objectMappingHierarchy;
                }
                if (!seriesDetail.IsNullOrEmpty())
                {
                    response.CheckStatus = CheckStatus.True;
                    if (response.UIMessage == String.Empty)
                        response.UIMessage = "You cannot remove item {0} as it is mapped with following series: " + seriesDetail;
                    else
                        response.UIMessage = response.UIMessage + " " + "and mapped with following series:" + seriesDetail;
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


        private static String GetObjectMappingHierarchy(DataTable Result)
        {
            StringBuilder tmpObjectNames = new StringBuilder();

            foreach (DataRow row in Result.Rows)
            {
                tmpObjectNames.Append(row[0].ToString());
                tmpObjectNames.Append(", ");
            }
            tmpObjectNames.Length = tmpObjectNames.Length - 2;
            return tmpObjectNames.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="parentItemId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfItemAttributeMappingCanBeDeleted(Int32 attributeId, Int32 parentItemId, Int32 tenantId, Int32 objectTypeIdForAttr, Int32 objectTypeIdForItem)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                String tempResult = String.Empty;
                DataTable Result = new DataTable();
                StringBuilder tmpObjectNames = new StringBuilder();
                String seriesDetail = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    Result = BALUtils.GetIntegrityRepoInstance(tenantId).ifObjectIsUsedWithAnyRule(attributeId, objectTypeIdForAttr, parentItemId, objectTypeIdForItem, out seriesDetail);
                }
                if (IsClientAdmin(tenantId))
                {
                    Result = BALUtils.GetIntegrityRepoInstance(tenantId).ifObjectIsUsedWithAnyRule(attributeId, objectTypeIdForAttr, parentItemId, objectTypeIdForItem, out seriesDetail);
                }
                if (Result.Rows.Count > 0)
                {
                    //Object is mapped
                    response.CheckStatus = CheckStatus.True;
                    String objectMappingHierarchy = GetObjectMappingHierarchy(Result);
                    response.UIMessage = "You cannot remove attribute {0} as it is used in rules associated with " + objectMappingHierarchy;
                }
                if (!seriesDetail.IsNullOrEmpty())
                {
                    response.CheckStatus = CheckStatus.True;
                    if (response.UIMessage == String.Empty)
                        response.UIMessage = "You cannot remove item {0} as it is mapped with following series: " + seriesDetail;
                    else
                        response.UIMessage = response.UIMessage + " " + "and mapped with following series:" + seriesDetail;
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
        /// 
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
                if (IsDefaultTenant(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfobjectRuleSetMappingIsAssociated(ruleSetId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot remove ruleset {0} as it is associated with other objects.";
                    }
                }
                if (IsClientAdmin(tenantId))
                {
                    //if (BALUtils.GetIntegrityRepoInstance(tenantId).IfPackageAssociatedWithRuleSetIsMapped(ruleSetId))
                    //{
                    //    response.CheckStatus = CheckStatus.True;
                    //    response.UIMessage = "You cannot remove ruleset {0} as it is in use.";
                    //}
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfobjectRuleSetMappingIsAssociated(ruleSetId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot remove ruleset {0} as it is associated with other objects.";
                    }
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

        public static IntegrityCheckResponse IfRuleSetRuleMappingCanBeDeleted(Int32 ruleId, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (IsDefaultTenant(tenantId))
                {
                    //no condition for deletion
                }
                if (IsClientAdmin(tenantId))
                {
                    if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAnyPackageSubscriptionExistForRule(ruleId))
                    {
                        response.CheckStatus = CheckStatus.True;
                        response.UIMessage = "You cannot delete rule {0} as it is in use.";
                    }
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
        /// check whether package can be unassigned 
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="packageName"></param>
        /// <param name="tenantId"></param>
        /// <returns>IntegrityCheckResponse</returns>
        public static IntegrityCheckResponse IfPackageCanBeUnassigned(Int32 packageId, String packageName, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetIntegrityRepoInstance(tenantId).ifPackageIsMapped(packageId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = String.Format("You cannot unassign package {0} as it is in use.", packageName);
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
        ///  check whether category can be unassigned
        /// </summary>
        /// <param name="parentPackageId"></param>
        /// <param name="categoryName"></param>
        /// <param name="tenantId"></param>
        /// <returns>IntegrityCheckResponse</returns>
        public static IntegrityCheckResponse IfCategoryCanBeUnassigned(Int32 parentPackageId, String categoryName, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetIntegrityRepoInstance(tenantId).ifPackageIsMapped(parentPackageId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = String.Format("You cannot unassign category {0} as it is in use.", categoryName);
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
        ///  check whether item can be unassigned
        /// </summary>
        /// <param name="parentCategoryId"></param>
        /// <param name="itemName"></param>
        /// <param name="tenantId"></param>
        /// <returns>IntegrityCheckResponse</returns>
        public static IntegrityCheckResponse IfItemCanBeUnassigned(Int32 parentCategoryId, String itemName, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAnyPackageAssociatedWithCategoryIsMapped(parentCategoryId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = String.Format("You cannot unassign item {0} as it is in use.", itemName);
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
        ///  check whether attribute can be unassigned
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="attributeName"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IntegrityCheckResponse IfAttributeCanBeUnassigned(Int32 parentItemId, String attributeName, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;
                if (BALUtils.GetIntegrityRepoInstance(tenantId).IfAnyPackageAssociatedWithItemIsMapped(parentItemId))
                {
                    response.CheckStatus = CheckStatus.True;
                    response.UIMessage = String.Format("You cannot unassign attribute {0} as it is in use.", attributeName);
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

        //Boolean IsPackageHasOrder(Int32 DeptProgramPkgID)
        #region Hierarchy Package mapping
        public static IntegrityCheckResponse IsPackageHasOrder(Int32 deptProgramPackageID, Int32 tenantId)
        {
            try
            {
                IntegrityCheckResponse response;
                response.CheckStatus = CheckStatus.False;
                response.UIMessage = String.Empty;

                if (BALUtils.GetIntegrityRepoInstance(tenantId).IsPackageHasOrder(deptProgramPackageID))
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
    }
}
