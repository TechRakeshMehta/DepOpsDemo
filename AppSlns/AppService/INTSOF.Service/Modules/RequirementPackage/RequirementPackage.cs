using Business.RepoManagers;
using INTSOF.Service.Core;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceInterface.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace INTSOF.Service.Modules.RequirementPackage
{
    public class RequirementPackage : BaseService, IRequirementPackage
    {
        /// <summary>
        /// method used to return lkpRequirementFieldDataType values
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        ServiceResponse<List<RotationFieldDataTypeContract>> IRequirementPackage.GetRotationFieldDataTypes(ServiceRequest<Int32, Boolean> parameters)
        {
            ServiceResponse<List<RotationFieldDataTypeContract>> commonResponse = new ServiceResponse<List<RotationFieldDataTypeContract>>();
            try
            {
                //Call Business Manager Method 
                Boolean isSharedUser = parameters.Parameter2;
                if (isSharedUser)
                {
                    commonResponse.Result = SharedRequirementPackageManager.GetRotationFieldDataTypes();
                }
                else
                {
                    commonResponse.Result = RequirementPackageManager.GetRotationFieldDataTypes(parameters.Parameter1);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// method used to add entries in requirement package,category,item,field and all mapping tables
        /// </summary>
        /// <param name="requirementPackageParameters"></param>
        /// <returns></returns>
        ServiceResponse<Int32> IRequirementPackage.SaveRequirementPackage(ServiceRequest<RequirementPackageContract, Int32, Int32> requirementPackageParameters)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                Boolean isSharedUserLoggedIn = requirementPackageParameters.Parameter1.IsSharedUserLoggedIn;
                //Call Business Manager Method 
                if (isSharedUserLoggedIn)
                {
                    commonResponse.Result = SharedRequirementPackageManager.SaveRequirementPackage(requirementPackageParameters.Parameter1, requirementPackageParameters.Parameter3);
                }
                else
                {
                    commonResponse.Result = RequirementPackageManager.SaveRequirementPackage(requirementPackageParameters.Parameter1, requirementPackageParameters.Parameter2, requirementPackageParameters.Parameter3);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// method used to delete entries in requirement package,category,item,field and all mapping tables
        /// </summary>
        /// <param name="requirementPackageParameters"></param>
        /// <returns></returns>
        ServiceResponse<Int32> IRequirementPackage.DeleteRequirementPackage(ServiceRequest<RequirementPackageContract, Int32, Int32> requirementPackageParameters)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                Boolean isSharedUserLoggedIn = requirementPackageParameters.Parameter1.IsSharedUserLoggedIn;
                //Call Business Manager Method 
                if (isSharedUserLoggedIn)
                {
                    commonResponse.Result = SharedRequirementPackageManager.DeleteRequirementPackage(requirementPackageParameters.Parameter1, requirementPackageParameters.Parameter3);
                }
                else
                {
                    commonResponse.Result = RequirementPackageManager.DeleteRequirementPackage(requirementPackageParameters.Parameter1, requirementPackageParameters.Parameter2, requirementPackageParameters.Parameter3);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// get complete package details in hierarchal way
        /// </summary>
        /// <param name="requirementPackageDetailsParameters"></param>
        /// <returns></returns>
        ServiceResponse<List<RequirementPackageDetailsContract>> IRequirementPackage.GetRequirementPackageDetailsByPackageID(ServiceRequest<Int32, Int32> requirementPackageDetailsParameters)
        {
            ServiceResponse<List<RequirementPackageDetailsContract>> commonResponse = new ServiceResponse<List<RequirementPackageDetailsContract>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = RequirementPackageManager.GetRequirementPackageDetailsByPackageID(requirementPackageDetailsParameters.Parameter1, requirementPackageDetailsParameters.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// method used to return a single package details including package name,category name and item,field name based on reuirementPackageID
        /// </summary>
        /// <param name="requirementPackageDetailsParameters"></param>
        /// <returns></returns>
        ServiceResponse<RequirementPackageContract> IRequirementPackage.GetRequirementPackageHierarchalDetailsByPackageID(ServiceRequest<Int32, Int32, Boolean> requirementPackageDetailsParameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            ServiceResponse<RequirementPackageContract> commonResponse = new ServiceResponse<RequirementPackageContract>();
            try
            {

                Boolean isSharedUserLoggedIn = requirementPackageDetailsParameters.Parameter3;
                //Call Business Manager Method 
                if (isSharedUserLoggedIn)
                {
                    commonResponse.Result = SharedRequirementPackageManager.GetRequirementPackageHierarchalDetailsByPackageID(requirementPackageDetailsParameters.Parameter1, new Guid(activeUser.UserID));
                }
                else
                {
                    commonResponse.Result = RequirementPackageManager.GetRequirementPackageHierarchalDetailsByPackageID(requirementPackageDetailsParameters.Parameter1, requirementPackageDetailsParameters.Parameter2);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// used to get all requirement package details including package name and comma separated agencyNames with which they are mapped. It also returns unMapped packages too
        /// </summary>
        /// <returns></returns>
        ServiceResponse<List<RequirementPackageDetailsContract>> IRequirementPackage.GetRequirementPackageDetails(ServiceRequest<RequirementPackageDetailsContract, CustomPagingArgsContract> requirementPackageDetailsParameters)
        {
            ServiceResponse<List<RequirementPackageDetailsContract>> commonResponse = new ServiceResponse<List<RequirementPackageDetailsContract>>();
            try
            {
                Boolean isFetchFromSharedDB = requirementPackageDetailsParameters.Parameter1.IsSharedUserLoggedIn;
                //Call Business Manager Method 
                if (isFetchFromSharedDB)
                {
                    commonResponse.Result = SharedRequirementPackageManager.GetRequirementPackageDetails(requirementPackageDetailsParameters.Parameter1, requirementPackageDetailsParameters.Parameter2);
                }
                else
                {
                    commonResponse.Result = RequirementPackageManager.GetRequirementPackageDetails(requirementPackageDetailsParameters.Parameter1, requirementPackageDetailsParameters.Parameter2);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="data">AgencyId/SelectedTenantID</param>
        /// <returns></returns>
        ServiceResponse<List<RequirementPackageContract>> IRequirementPackage.GetRequirementPackages(ServiceRequest<String, Boolean> data)
        {
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();
            try
            {
                Boolean isSharedUser = data.Parameter2;
                if (isSharedUser)
                {
                    commonResponse.Result = SharedRequirementPackageManager.GetRequirementPackages(data.SelectedTenantId, data.Parameter1);
                }
                else
                {
                    commonResponse.Result = RequirementPackageManager.GetRequirementPackages(data.SelectedTenantId, data.Parameter1);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// method used to return lkpConstantType values
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        ServiceResponse<List<RulesConstantTypeContract>> IRequirementPackage.GetRulesConstantTypes(ServiceRequest<Int32, Boolean> parameters)
        {
            ServiceResponse<List<RulesConstantTypeContract>> commonResponse = new ServiceResponse<List<RulesConstantTypeContract>>();
            try
            {
                Boolean isSharedUserLoggedIn = parameters.Parameter2;
                //Call Business Manager Method 
                if (isSharedUserLoggedIn)
                {
                    commonResponse.Result = SharedRequirementPackageManager.GetRulesConstantTypes();
                }
                else
                {
                    commonResponse.Result = RequirementPackageManager.GetRulesConstantTypes(parameters.Parameter1);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        #region  UAT 1352 As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use.

        ServiceResponse<List<RequirementPackageTypeContract>> IRequirementPackage.GetRequirementPackageType(ServiceRequest<Int32, Boolean> parameters)
        {
            ServiceResponse<List<RequirementPackageTypeContract>> commonResponse = new ServiceResponse<List<RequirementPackageTypeContract>>();
            try
            {
                Boolean isGetFromMasterDB = parameters.Parameter2;
                //Call Business Manager Method 
                if (isGetFromMasterDB)
                {
                    commonResponse.Result = SharedRequirementPackageManager.GetRequirementPackageType();
                }
                else
                {
                    commonResponse.Result = RequirementPackageManager.GetRequirementPackageType(parameters.Parameter1);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }
        #endregion

        //ServiceResponse<Dictionary<Int32, List<Int32>>> IRequirementPackage.GetTenantIDsMappedForAgencyUser(ServiceRequest<Guid> data)
        //{
        //    ServiceResponse<Dictionary<Int32, List<Int32>>> commonResponse = new ServiceResponse<Dictionary<Int32, List<Int32>>>();
        //    try
        //    {
        //        commonResponse.Result = SharedRequirementPackageManager.GetTenantIDsMappedForAgencyUser(data.Parameter);
        //        return commonResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        base.LogClinicalRotationSvcError(ex);
        //        throw;
        //    }
        //}

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="data">AgencyId</param>
        /// <returns></returns>
        ServiceResponse<List<RequirementPackageContract>> IRequirementPackage.GetInstructorRequirementPackages(ServiceRequest<String, Boolean> data)
        {
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();
            try
            {
                Boolean isSharedUser = data.Parameter2;
                if (isSharedUser)
                {
                    commonResponse.Result = SharedRequirementPackageManager.GetInstructorRequirementPackages(data.SelectedTenantId, data.Parameter1);
                }
                else
                {
                    commonResponse.Result = RequirementPackageManager.GetInstructorRequirementPackages(data.SelectedTenantId, data.Parameter1);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }


        ServiceResponse<Int32> IRequirementPackage.CopySharedRqrmntPkgToClient(ServiceRequest<Int32, Int32> requirementPackageParameters)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;

                RequirementPackageContract sharedPackageContract = SharedRequirementPackageManager.GetRequirementPackageHierarchalDetailsByPackageID(requirementPackageParameters.Parameter1, Guid.Empty, true);
                if (!sharedPackageContract.IsUsed)
                {
                    SharedRequirementPackageManager.SetExistingPackageIsUsedToTrue(requirementPackageParameters.Parameter2, requirementPackageParameters.Parameter1);
                }
                commonResponse.Result = RequirementPackageManager.CopyPackageToClient(sharedPackageContract, requirementPackageParameters.SelectedTenantId, requirementPackageParameters.Parameter2);
                //UAT-2305:
                if (commonResponse.Result > AppConsts.NONE)
                {
                    UniversalMappingDataManager.CopySharedToTenantRequirementUniversalMapping(requirementPackageParameters.SelectedTenantId, requirementPackageParameters.Parameter1
                                                                                            , commonResponse.Result, activeUser.OrganizationUserId);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }



        ServiceResponse<Int32> IRequirementPackage.CopyClientRqrmntPkgToShared(ServiceRequest<RequirementPackageContract, Int32, Int32> requirementPackageParameters)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                Boolean isPackageMappedToRotation = false;
                if (!requirementPackageParameters.Parameter1.IsCopyWithInMaster)
                {
                    isPackageMappedToRotation = RequirementPackageManager.IsPackageMappedToRotation(requirementPackageParameters.Parameter1.RequirementPackageID, requirementPackageParameters.Parameter2);
                    if (isPackageMappedToRotation)
                    {
                        RequirementPackageManager.SetExistingPackageIsCopiedToTrue(requirementPackageParameters.Parameter2, requirementPackageParameters.Parameter3, requirementPackageParameters.Parameter1.RequirementPackageID);
                    }
                    else
                    {
                        RequirementPackageManager.SetExistingPackageIsDeletedToTrue(requirementPackageParameters.Parameter2, requirementPackageParameters.Parameter3, requirementPackageParameters.Parameter1.RequirementPackageID);
                    }
                }

                commonResponse.Result = RequirementPackageManager.CopyClientRqrmntPkgToShared(requirementPackageParameters.Parameter1, requirementPackageParameters.Parameter2, requirementPackageParameters.Parameter3, isPackageMappedToRotation);
                //UAT-2305
                if (requirementPackageParameters.Parameter1.IsCopyWithInMaster && commonResponse.Result > AppConsts.NONE)
                {
                    UniversalMappingDataManager.CopySharedToSharedReqUniversalMappingForPkg(requirementPackageParameters.Parameter2,
                                                                                                      requirementPackageParameters.Parameter1.RequirementPackageID,
                                                                                                      commonResponse.Result, requirementPackageParameters.Parameter3);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<RequirementPackageContract> IRequirementPackage.GetRequirementPackageDataByID(ServiceRequest<Int32> requirementPackageParameters)
        {
            ServiceResponse<RequirementPackageContract> commonResponse = new ServiceResponse<RequirementPackageContract>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementPackageDataByID(requirementPackageParameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Int32> IRequirementPackage.SaveRequirementPackageData(ServiceRequest<RequirementPackageContract, Int32> requirementPackageParameters)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveRequirementPackageData(requirementPackageParameters.Parameter1, requirementPackageParameters.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Int32> IRequirementPackage.CreateMasterPackageVersion(ServiceRequest<RequirementPackageContract, Int32> versionParameters)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveRequirementPackage(versionParameters.Parameter1
                                                , versionParameters.Parameter2, false, false, true);
                //UAT-2305
                if (commonResponse.Result > AppConsts.NONE)
                {
                    UniversalMappingDataManager.CopySharedToSharedReqUniversalMappingForPkg(AppConsts.ONE, versionParameters.Parameter1.RequirementPackageID,
                                                                                                      commonResponse.Result, versionParameters.Parameter2);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region UAT-1837:ADB Admin streamlined create and edit rotation packages
        ServiceResponse<Boolean> IRequirementPackage.SaveUpdateRequirementItemData(ServiceRequest<RequirementItemContract> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveUpdateRequirementItemData(parameters.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<RequirementItemContract> IRequirementPackage.GetRequirementItemDetail(ServiceRequest<Int32, Int32, Int32?> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<RequirementItemContract> commonResponse = new ServiceResponse<RequirementItemContract>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementItemDetailsByItemID(parameters.Parameter2, parameters.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementItemContract>> IRequirementPackage.GetRequirementItemsByCategoryID(ServiceRequest<Int32> parameters)
        {
            ServiceResponse<List<RequirementItemContract>> commonResponse = new ServiceResponse<List<RequirementItemContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementItemsByCategoryID(parameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<String> IRequirementPackage.DeleteReqCategoryItemMapping(ServiceRequest<Int32, Int32, Boolean> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.DeleteReqCategoryItemMapping(parameters.Parameter1, activeUser.OrganizationUserId, parameters.Parameter2,
                                                                                                     parameters.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementFieldContract>> IRequirementPackage.GetRequirementFieldsByItemID(ServiceRequest<Int32> parameters)
        {
            ServiceResponse<List<RequirementFieldContract>> commonResponse = new ServiceResponse<List<RequirementFieldContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementFieldsByItemID(parameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        //UAT-3342
        ServiceResponse<List<RequirementFieldContract>> IRequirementPackage.IsCalculatedAttribute(ServiceRequest<Int32> parameters)
        {
            ServiceResponse<List<RequirementFieldContract>> commonResponse = new ServiceResponse<List<RequirementFieldContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.IsCalculatedAttribute(parameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<String> IRequirementPackage.DeleteReqItemFieldMapping(ServiceRequest<Int32, String, Boolean, Int32> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.DeleteReqItemFieldMapping(parameters.Parameter1, parameters.Parameter2, activeUser.OrganizationUserId,
                                                                                                  parameters.Parameter3, parameters.Parameter4);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Int32> IRequirementPackage.SaveUpdateRequirementFieldData(ServiceRequest<RequirementFieldContract> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveUpdateRequirementFieldData(parameters.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<RequirementFieldContract> IRequirementPackage.GetRequirementFieldDataByID(ServiceRequest<Int32, Int32> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<RequirementFieldContract> commonResponse = new ServiceResponse<RequirementFieldContract>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementFieldDataByID(parameters.Parameter1, parameters.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        ServiceResponse<Boolean> IRequirementPackage.IsRequirementPackageUsed(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.IsPackageVersionNeedToCreate(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<RequirementCategoryContract> IRequirementPackage.GetRequirementCategoryDetail(ServiceRequest<Int32, Int32> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<RequirementCategoryContract> commonResponse = new ServiceResponse<RequirementCategoryContract>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementCategoryDetailByCategoryID(parameters.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementCategoryContract>> IRequirementPackage.GetRequirementCategoriesByPackageID(ServiceRequest<Int32> parameters)
        {
            ServiceResponse<List<RequirementCategoryContract>> commonResponse = new ServiceResponse<List<RequirementCategoryContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementCategoriesByPackageID(parameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.DeleteReqPackageCategoryMapping(ServiceRequest<Int32> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.DeleteReqPackageCategoryMapping(parameters.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.SaveRequirementCategoryDetails(ServiceRequest<RequirementCategoryContract> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveRequirementCategoryDetails(parameters.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region UAT-1837.
        ServiceResponse<List<RequirementTreeContract>> IRequirementPackage.GetRequirementTree(ServiceRequest<Int32> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<List<RequirementTreeContract>> commonResponse = new ServiceResponse<List<RequirementTreeContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementTree(parameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementRuleContract>> IRequirementPackage.GetRequirementRuleDetail(ServiceRequest<Int32> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<List<RequirementRuleContract>> commonResponse = new ServiceResponse<List<RequirementRuleContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementRuleDetail(parameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.SaveUpdateRequirementRule(ServiceRequest<List<RequirementRuleContract>> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveUpdateRequirementRule(parameters.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// method used to return a single package details including package name,category name and item,field name based on reuirementPackageID
        /// </summary>
        /// <param name="requirementPackageDetailsParameters"></param>
        /// <returns></returns>
        ServiceResponse<RequirementPackageContract> IRequirementPackage.GetRequirementPackageHierarchalDetailsByPackageIDForVersioning(ServiceRequest<Int32, Int32, Boolean> requirementPackageDetailsParameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<RequirementPackageContract> commonResponse = new ServiceResponse<RequirementPackageContract>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementPackageHierarchalDetailsByPackageID(requirementPackageDetailsParameters.Parameter1, Guid.Empty, true);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        #endregion

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="data">AgencyId/SelectedTenantID</param>
        /// <returns></returns>
        ServiceResponse<List<RequirementPackageContract>> IRequirementPackage.GetAllRequirementPackages(ServiceRequest<Int32> parameter)
        {
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();
            try
            {
                commonResponse.Result = RequirementPackageManager.GetAllRequirementPackages(parameter.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// To check Requirement Package completion status
        /// </summary>
        /// <param name="data">RequirementPackageID</param>
        /// <returns></returns>
        ServiceResponse<RequirementPackageCompletionContract> IRequirementPackage.CheckRequirementPackageCompletionStatus(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<RequirementPackageCompletionContract> commonResponse = new ServiceResponse<RequirementPackageCompletionContract>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.CheckRequirementPackageCompletionStatus(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #region UAT-2305 Master Rotation package
        ServiceResponse<List<UniversalCategoryContract>> IRequirementPackage.GetUniversalCategorys()
        {
            ServiceResponse<List<UniversalCategoryContract>> commonResponse = new ServiceResponse<List<UniversalCategoryContract>>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalCategorys();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<UniversalCategoryContract> IRequirementPackage.GetUniversalCategoryByReqCatID(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<UniversalCategoryContract> commonResponse = new ServiceResponse<UniversalCategoryContract>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalCategoryByReqCatID(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IRequirementPackage.DeleteUnversalCategoryMappings(ServiceRequest<Int32> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.DeleteUnversalCategoryMappings(serviceRequest.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<UniversalItemContract>> IRequirementPackage.GetUniversalItemsByUniReqCatID(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<UniversalItemContract>> commonResponse = new ServiceResponse<List<UniversalItemContract>>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalItemsByUniReqCatID(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<UniversalItemContract> IRequirementPackage.GetUniversalItemsByReqCatItmID(ServiceRequest<Int32, Int32> serviceRequest)
        {
            ServiceResponse<UniversalItemContract> commonResponse = new ServiceResponse<UniversalItemContract>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalItemsByUniReqCatItmID(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IRequirementPackage.DeleteUniversalReqItmMapping(ServiceRequest<Int32> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.DeleteUniversalReqItmMapping(serviceRequest.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<UniversalAttributeContract>> IRequirementPackage.GetUniversalAttributes(ServiceRequest<Int32, Int32> serviceRequest)
        {
            ServiceResponse<List<UniversalAttributeContract>> commonResponse = new ServiceResponse<List<UniversalAttributeContract>>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalAttributes(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<UniversalAttributeContract> IRequirementPackage.GetUniversalattributeDetails(ServiceRequest<Int32, Int32, Int32> serviceRequest)
        {
            ServiceResponse<UniversalAttributeContract> commonResponse = new ServiceResponse<UniversalAttributeContract>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalattributeDetails(serviceRequest.Parameter1, serviceRequest.Parameter2, serviceRequest.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<UniversalAttributeContract> IRequirementPackage.GetUniversalFieldAttributeDetails(ServiceRequest<Int32, Int32, Int32> serviceRequest)
        {
            ServiceResponse<UniversalAttributeContract> commonResponse = new ServiceResponse<UniversalAttributeContract>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalFieldAttributeDetails(serviceRequest.Parameter1, serviceRequest.Parameter2, serviceRequest.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<InputTypeComplianceAttributeServiceContract>> IRequirementPackage.GetAtrInputPriorityByID(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<InputTypeComplianceAttributeServiceContract>> commonResponse = new ServiceResponse<List<InputTypeComplianceAttributeServiceContract>>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetAtrInputPriorityByID(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<InputTypeComplianceAttributeServiceContract>> IRequirementPackage.GetUniversalFieldAtrInputPriorityByID(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<InputTypeComplianceAttributeServiceContract>> commonResponse = new ServiceResponse<List<InputTypeComplianceAttributeServiceContract>>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalFieldAtrInputPriorityByID(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IRequirementPackage.SaveUpdateAttributeInputPriority(ServiceRequest<UniversalAttributeContract> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.SaveUpdateAttributeInputPriority(serviceRequest.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Dictionary<Int32, String>> IRequirementPackage.GetUniversalAtrOptionData(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<Dictionary<Int32, String>> commonResponse = new ServiceResponse<Dictionary<Int32, String>>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalAtrOptionData(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Dictionary<Int32, String>> IRequirementPackage.GetUniversalFieldAtrOptionData(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<Dictionary<Int32, String>> commonResponse = new ServiceResponse<Dictionary<Int32, String>>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalFieldAtrOptionData(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<Int32>> IRequirementPackage.GetUniversalAtrOptionSelected(ServiceRequest<Int32, Int32> serviceRequest)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalAtrOptionSelected(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<Int32>> IRequirementPackage.GetUniversalFieldAtrOptionSelected(ServiceRequest<Int32, Int32> serviceRequest)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                commonResponse.Result = UniversalMappingDataManager.GetUniversalFieldAtrOptionSelected(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-2332
        ServiceResponse<List<DefinedRequirementContract>> IRequirementPackage.GetDefinedRequirement(ServiceRequest<Int32, Boolean> parameters)
        {
            ServiceResponse<List<DefinedRequirementContract>> commonResponse = new ServiceResponse<List<DefinedRequirementContract>>();
            try
            {
                Boolean isGetFromMasterDB = parameters.Parameter2;
                //Call Business Manager Method 
                if (isGetFromMasterDB)
                {
                    commonResponse.Result = SharedRequirementPackageManager.GetDefinedRequirement();
                }
                else
                {
                    commonResponse.Result = RequirementPackageManager.GetDefinedRequirement(parameters.Parameter1);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-2213
        ServiceResponse<List<RotationMappingContract>> IRequirementPackage.GetRotationMappingTreeData(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<RotationMappingContract>> commonResponse = new ServiceResponse<List<RotationMappingContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRotationMappingTreeData(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementPackageContract>> IRequirementPackage.GetMasterRequirementPackageDetails(ServiceRequest<RequirementPackageContract, CustomPagingArgsContract> serviceRequest)
        {
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetMasterRequirementPackageDetails(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region [UAT-2213]

        ServiceResponse<RequirementCategoryContract> IRequirementPackage.GetRequirementMasterCategoryDetailByCategoryID(ServiceRequest<Int32> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<RequirementCategoryContract> commonResponse = new ServiceResponse<RequirementCategoryContract>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementMasterCategoryDetailByCategoryID(parameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.SaveMasterRotationCategory(ServiceRequest<RequirementCategoryContract> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveMasterRotationCategory(parameters.Parameter, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IRequirementPackage.IsMasterCategoryNameExists(ServiceRequest<String> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.IsMasterCategoryNameExists(parameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementPackageContract>> IRequirementPackage.GetAllMasterRequirementPackages()
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetAllMasterRequirementPackages();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Int32> IRequirementPackage.CreateCategoryCopy(ServiceRequest<CreateCategoryCopyContract> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.CreateCategoryCopy(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementCategoryContract>> IRequirementPackage.GetMasterRequirementCategories(ServiceRequest<RequirementCategoryContract, CustomPagingArgsContract> serviceRequest)
        {
            ServiceResponse<List<RequirementCategoryContract>> commonResponse = new ServiceResponse<List<RequirementCategoryContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetMasterRequirementCategories(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementCategoryContract>> IRequirementPackage.GetRequirementCategories()
        {
            ServiceResponse<List<RequirementCategoryContract>> commonResponse = new ServiceResponse<List<RequirementCategoryContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementCategories();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<Int32>> IRequirementPackage.GetMappedPackageIdsWithCategory(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetMappedPackageIdsWithCategory(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<Int32>> IRequirementPackage.GetMappedCategoriesWithPackage(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetMappedCategoriesWithPackage(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        public ServiceResponse<string> DeleteRequirementCategory(ServiceRequest<int> serviceRequest)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.DeleteRequirementCategory(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }


        ServiceResponse<List<RequirementPackageContract>> IRequirementPackage.GetCategoryPackageMapping(ServiceRequest<CategoryPackageMappingContract, CustomPagingArgsContract> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();

            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetCategoryPackageMapping(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.SaveCategoryPackageMapping(ServiceRequest<Int32, Int32, String> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();

            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveCategoryPackageMapping(serviceRequest.Parameter1, serviceRequest.Parameter2, serviceRequest.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }


        ServiceResponse<Boolean> IRequirementPackage.UpdatePackageCategoryMappingDisplayOrder(ServiceRequest<List<RequirementCategoryContract>, Int32?, Int32, Int32> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();

            try
            {
                commonResponse.Result = SharedRequirementPackageManager.UpdatePackageCategoryMappingDisplayOrder(serviceRequest.Parameter1, serviceRequest.Parameter2, serviceRequest.Parameter3, serviceRequest.Parameter4);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }



        ServiceResponse<List<RequirementCategoryContract>> IRequirementPackage.GetPackageCategoryMapping(ServiceRequest<PackageCategoryMappingContract, CustomPagingArgsContract> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<List<RequirementCategoryContract>> commonResponse = new ServiceResponse<List<RequirementCategoryContract>>();

            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetPackageCategoryMapping(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.SavePackageCategoryMapping(ServiceRequest<Int32, Int32, String> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();

            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SavePackageCategoryMapping(serviceRequest.Parameter1, serviceRequest.Parameter2, serviceRequest.Parameter3, false, 0);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }


        ServiceResponse<int> IRequirementPackage.SaveMasterRequirementPackage(ServiceRequest<RequirementPackageContract, Int32, Boolean> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<int> commonResponse = new ServiceResponse<int>();

            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveMasterRequirementPackage(serviceRequest.Parameter1, serviceRequest.Parameter2, serviceRequest.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.ArchivePackage(ServiceRequest<Dictionary<Int32, Boolean>, Int32> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            commonResponse.Result = SharedRequirementPackageManager.ArchivePackage(serviceRequest.Parameter1, serviceRequest.Parameter2);
            return commonResponse;
        }

        //UAT-4054
        ServiceResponse<Boolean> IRequirementPackage.UnArchivePackage(ServiceRequest<Dictionary<Int32, Boolean>, Int32> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            commonResponse.Result = SharedRequirementPackageManager.UnArchivePackage(serviceRequest.Parameter1, serviceRequest.Parameter2);
            return commonResponse;
        }


        ServiceResponse<List<Int32>> IRequirementPackage.GetMappedPackageDetails(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            commonResponse.Result = SharedRequirementPackageManager.GetMappedPackageDetails(serviceRequest.Parameter);
            return commonResponse;
        }
        ServiceResponse<Int32> IRequirementPackage.GetRequirementObjectTreeIDByprntID(ServiceRequest<String> serviceRequest)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            commonResponse.Result = SharedRequirementPackageManager.GetRequirementObjectTreeIDByprntID(serviceRequest.Parameter);
            return commonResponse;
        }
        ServiceResponse<List<RequirementRuleContract>> IRequirementPackage.GetReqFixedRuleDetailByObjectTreeID(ServiceRequest<Int32> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<List<RequirementRuleContract>> commonResponse = new ServiceResponse<List<RequirementRuleContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetReqFixedRuleDetailByObjectTreeID(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IRequirementPackage.SaveUpdateNewRequirementFieldRule(ServiceRequest<List<RequirementRuleContract>, Int32, Int32> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveUpdateNewRequirementFieldRule(serviceRequest.Parameter1, serviceRequest.Parameter2, serviceRequest.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<List<Int32>> IRequirementPackage.GetRequirementObjectTreeIDByReqFieldID(ServiceRequest<Int32> serviceRequest)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementObjectTreeIDByReqFieldID(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion


        #region UAT-2213:Copy Package
        ServiceResponse<Int32> IRequirementPackage.CopySharedRequirementPackageToClientNew(ServiceRequest<Int32, Int32> requirementPackageParameters)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;

                RequirementPackageContract sharedPackageContract = SharedRequirementPackageManager.GetRequirementPackageHierarchalDetailsByPackageIDNew(requirementPackageParameters.Parameter1, Guid.Empty, true);
                if (!sharedPackageContract.IsUsed)
                {
                    SharedRequirementPackageManager.SetExistingPackageIsUsedToTrue(requirementPackageParameters.Parameter2, requirementPackageParameters.Parameter1);
                }
                commonResponse.Result = RequirementPackageManager.CopySharedPackageToClientNew(sharedPackageContract, requirementPackageParameters.SelectedTenantId, requirementPackageParameters.Parameter2);
                //UAT-2305:
                if (commonResponse.Result > AppConsts.NONE)
                {
                    UniversalMappingDataManager.CopySharedToTenantRequirementUniversalMapping(requirementPackageParameters.SelectedTenantId, requirementPackageParameters.Parameter1
                                                                                            , commonResponse.Result, activeUser.OrganizationUserId);
                }
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }


        #endregion

        #region Check Rot. Eff. Start Date & EndDate
        ServiceResponse<List<Int32>, List<String>> IRequirementPackage.CheckRotEffectiveDate(ServiceRequest<Int32, String, Int32> requirementPackageParameters)
        {
            ServiceResponse<List<Int32>, List<String>> commonResponse = new ServiceResponse<List<Int32>, List<String>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
                var Result = SharedRequirementPackageManager.CheckRotEffectiveDate(requirementPackageParameters.Parameter1, requirementPackageParameters.Parameter2, requirementPackageParameters.Parameter3);
                commonResponse.Result1 = Result.Item1;
                commonResponse.Result2 = Result.Item2;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<ClinicalRotationDetailContract> IRequirementPackage.GetRotationDetail(ServiceRequest<Int32, Int32> rotationParameters)
        {
            ServiceResponse<ClinicalRotationDetailContract> commonResponse = new ServiceResponse<ClinicalRotationDetailContract>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
                commonResponse.Result = SharedRequirementPackageManager.GetRotationDetail(rotationParameters.Parameter1, rotationParameters.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        //UAT-2533
        ServiceResponse<List<RequirementPackageContract>> IRequirementPackage.GetRequirementPackageDetail(ServiceRequest<String> serviceRequest)
        {
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementPackageDetail(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Boolean> IRequirementPackage.BulkPackageCopy(ServiceRequest<String, Int32> serviceRequest)
        {

            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.BulkPackageCopy(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// Get the AgencyHierarchyIds Associated with the Requirement Package. 
        /// </summary>
        /// <param name="requirementPackageParameters">RequirementPackageId,IsDeletedCheckNeededOrNot</param>
        /// <returns></returns>
        ServiceResponse<List<Int32>> IRequirementPackage.GetAgencyHierarchyIdsByRequirementPackageID(ServiceRequest<Int32> requirementPackageParameters)
        {
            ServiceResponse<List<Int32>> commonResponse = new ServiceResponse<List<Int32>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetAgencyHierarchyIdsByRequirementPackageID(requirementPackageParameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        /// <summary>
        /// UAT-2423 Get the Rotation Package Category name and ExplanatoryNotes using rotation ID 
        /// </summary>
        /// <param name="serviceRequest"></param>
        /// <returns></returns>
        ServiceResponse<List<RequirementCategoryContract>> IRequirementPackage.GetRotationPackageCategoryDetailByRotationID(ServiceRequest<Int32, Int32, Boolean> serviceRequest)
        {
            ServiceResponse<List<RequirementCategoryContract>> commonResponse = new ServiceResponse<List<RequirementCategoryContract>>();
            try
            {
                commonResponse.Result = RequirementPackageManager.GetRotationPackageCategoryDetailByRotationID(serviceRequest.Parameter1, serviceRequest.Parameter2, serviceRequest.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        #region UAT-2706
        ServiceResponse<List<RequirementItemContract>> IRequirementPackage.GetRequirementItemDetailsByCategoryId(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<RequirementItemContract>> commonResponse = new ServiceResponse<List<RequirementItemContract>>();
            try
            {
                commonResponse.Result = RequirementPackageManager.GetRequirementItemDetailsByCategoryId(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }
        ServiceResponse<Dictionary<Int32, String>> IRequirementPackage.GetSharedRequirementCategoryData(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<Dictionary<Int32, String>> commonResponse = new ServiceResponse<Dictionary<Int32, String>>();
            try
            {
                commonResponse.Result = RequirementPackageManager.GetRequirementCategoryDataBypackageId(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementPackageContract>> IRequirementPackage.GetRequirementPackagesByHierarcyIds(ServiceRequest<List<Int32>, Int32, CustomPagingArgsContract> serviceRequest)
        {
            ServiceResponse<List<RequirementPackageContract>> commonResponse = new ServiceResponse<List<RequirementPackageContract>>();
            try
            {
                commonResponse.Result = RequirementPackageManager.GetRequirementPackagesByHierarcyIds(serviceRequest.Parameter1, serviceRequest.Parameter2, serviceRequest.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        //UAT-2795
        ServiceResponse<String> IRequirementPackage.GetCategoryDocumentLink(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = RequirementPackageManager.GetCategoryDocumentLink(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        #endregion

        #region UAT-2788
        ServiceResponse<List<RequirementFieldType>> IRequirementPackage.GetAttributeType(ServiceRequest<Int32> parameters)
        {
            ServiceResponse<List<RequirementFieldType>> commonResponse = new ServiceResponse<List<RequirementFieldType>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = SharedRequirementPackageManager.GetAttributeType();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-3078
        ServiceResponse<Boolean> IRequirementPackage.updateRequirementItemDisplayOrder(ServiceRequest<Int32, Int32, Int32, Int32> parameters, Boolean isNewPackage)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = SharedRequirementPackageManager.updateRequirementItemDisplayOrder(parameters.Parameter1, parameters.Parameter2, parameters.Parameter3, parameters.Parameter4, isNewPackage);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.updateRequirementFieldDisplayOrder(ServiceRequest<Int32, Int32, Int32, Int32> parameters, Boolean isNewPackage)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = SharedRequirementPackageManager.updateRequirementFieldDisplayOrder(parameters.Parameter1, parameters.Parameter2, parameters.Parameter3, parameters.Parameter4, isNewPackage);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-3176
        ServiceResponse<List<RequirementAttributeGroups>> IRequirementPackage.GetRequirementAttributeGroups(ServiceRequest<Int32> parameters)
        {
            ServiceResponse<List<RequirementAttributeGroups>> commonResponse = new ServiceResponse<List<RequirementAttributeGroups>>();
            try
            {
                //Call Business Manager Method 
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementAttributeGroups();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }
        #endregion

        ServiceResponse<List<RequirementReviewByContract>> IRequirementPackage.GetRequirementReviewBy(ServiceRequest<Int32, Boolean> parameters)
        {
            ServiceResponse<List<RequirementReviewByContract>> commonResponse = new ServiceResponse<List<RequirementReviewByContract>>();
            try
            {
                Boolean isGetFromMasterDB = parameters.Parameter2;
                //Call Business Manager Method 
                if (isGetFromMasterDB)
                {
                    commonResponse.Result = SharedRequirementPackageManager.GetRequirementReviewBy();
                }
                //else
                //{
                //    commonResponse.Result = RequirementPackageManager.GetDefinedRequirement(parameters.Parameter1);
                //}
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.CloneRequirementItem(ServiceRequest<Int32, Int32, Int32> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.CloneRequirementItem(parameters.Parameter1, parameters.Parameter2, parameters.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region UAT-3296
        ServiceResponse<String> IRequirementPackage.GetCategoryExplanatoryNotes(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = RequirementPackageManager.GetCategoryExplanatoryNotes(serviceRequest.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-4001
        ServiceResponse<List<RequirementDocumentAcroFieldType>> IRequirementPackage.GetDocumentAcroFieldTypeData(ServiceRequest<Int32> serviceRequest)
        {
            ServiceResponse<List<RequirementDocumentAcroFieldType>> commonResponse = new ServiceResponse<List<RequirementDocumentAcroFieldType>>();
            try
            {
                commonResponse.Result = RequirementPackageManager.GetDocumentAcroFieldType();
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogRequirementPkgSvcError(ex);
                throw;
            }
        }
        #endregion

        #region UAT-4254 || Release - 181

        ServiceResponse<List<RequirementCategoryDocUrl>> IRequirementPackage.GetRequirementCatDocUrls(ServiceRequest<Int32> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<List<RequirementCategoryDocUrl>> commonResponse = new ServiceResponse<List<RequirementCategoryDocUrl>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetRequirementCatDocUrls(parameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion
        #region UAT-4657
       
        ServiceResponse<Dictionary<Int32, String>> IRequirementPackage.GetPackagesAssociatedWithCategory(ServiceRequest<Int32> parameters)
        {
            //UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Dictionary<Int32, String>> commonResponse = new ServiceResponse<Dictionary<Int32, String>>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.GetPackagesAssociatedWithCategory(parameters.Parameter);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.SaveCategoryDiassociationDetail(ServiceRequest<Int32,String> parameters)
        {
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s") : ActiveUser;
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.SaveCategoryDiassociationDetail(parameters.Parameter1, parameters.Parameter2, activeUser.OrganizationUserId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        ServiceResponse<String> IRequirementPackage.IsCategoryDisassociationInProgress(ServiceRequest<Int32, List<Int32>> serviceRequest)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.IsCategoryDisassociationInProgress(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IRequirementPackage.IsSyncAlreadyInProgress(ServiceRequest<Int32, Boolean> serviceRequest)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = SharedRequirementPackageManager.IsSyncAlreadyInProgress(serviceRequest.Parameter1, serviceRequest.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

    }
}
