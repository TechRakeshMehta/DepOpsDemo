using INTSOF.Service.Core;
using INTSOF.ServiceInterface.Modules.ApplicantClinicalRotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NLog;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Data;

namespace INTSOF.Service.Modules.ApplicantClinicalRotation
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ApplicantRequirement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ApplicantRequirement.svc or ApplicantRequirement.svc.cs at the Solution Explorer and start debugging.
    public class ApplicantClinicalRotation : BaseService, IApplicantClinicalRotation
    {
        private static NLog.Logger logger;

        ServiceResponse<RequirementPackageSubscriptionContract, RequirementPackageContract> IApplicantClinicalRotation.GetRequirementPackageSubscriptionData(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<RequirementPackageSubscriptionContract, RequirementPackageContract> commonResponse = new ServiceResponse<RequirementPackageSubscriptionContract, RequirementPackageContract>();
            try
            {
                commonResponse.Result1 = ApplicantRequirementManager.GetRequirementPackageSubscription(data.Parameter1, data.Parameter2);
                commonResponse.Result2 = ApplicantRequirementManager.GetRequirementPackageDetail(commonResponse.Result1.RequirementPackageID, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<ClinicalRotationDetailContract>> IApplicantClinicalRotation.GetApplicantRotaions(ServiceRequest<ClinicalRotationDetailContract> data)
        {
            ServiceResponse<List<ClinicalRotationDetailContract>> commonResponse = new ServiceResponse<List<ClinicalRotationDetailContract>>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ApplicantClinicalRotationManager.GetApplicantClinicalRotations(activeUser.OrganizationUserId, data.Parameter, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<RequirementItemContract> IApplicantClinicalRotation.GetDataEntryRequirementItem(ServiceRequest<Int32,Int32> data)
        {
            ServiceResponse<RequirementItemContract> commonResponse = new ServiceResponse<RequirementItemContract>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetDataEntryRequirementItem(data.Parameter1, data.SelectedTenantId,data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<ApplicantDocumentContract>> IApplicantClinicalRotation.GetApplicantDocument(ServiceRequest<Int32> data)
        {
            ServiceResponse<List<ApplicantDocumentContract>> commonResponse = new ServiceResponse<List<ApplicantDocumentContract>>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetApplicantDocument(data.Parameter, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<ApplicantRequirementItemDataContract> IApplicantClinicalRotation.GetApplicantRequirementItemData(ServiceRequest<ApplicantRequirementParameterContract, Int32> data)
        {
            ServiceResponse<ApplicantRequirementItemDataContract> commonResponse = new ServiceResponse<ApplicantRequirementItemDataContract>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetApplicantRequirementItemData(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Dictionary<Boolean, String>> IApplicantClinicalRotation.SaveApplicantRequirementData(ServiceRequest<ApplicantRequirementParameterContract, Int32, Int32> data)
        {
            ServiceResponse<Dictionary<Boolean, String>> commonResponse = new ServiceResponse<Dictionary<Boolean, String>>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.SaveApplicantRequirementData(data.Parameter1, data.Parameter2, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementPackageSubscriptionStatusContract>> IApplicantClinicalRotation.GetPackageSubscriptionCategoryStatus(ServiceRequest<Int32, String> data)
        {
            ServiceResponse<List<RequirementPackageSubscriptionStatusContract>> commonResponse = new ServiceResponse<List<RequirementPackageSubscriptionStatusContract>>();
            try
            {
                var res = ApplicantRequirementManager.GetPackageSubscriptionCategoryStatus(data.Parameter1, data.Parameter2);
                List<RequirementPackageSubscriptionStatusContract> requirementPackageCategoryStatusList = new List<RequirementPackageSubscriptionStatusContract>();
                RequirementPackageSubscriptionStatusContract requirementPackageCategoryStatus;
                foreach (DataRow dr in res.Rows)
                {
                    requirementPackageCategoryStatus = new RequirementPackageSubscriptionStatusContract();
                    requirementPackageCategoryStatus.RequirementPackageSubscriptionID = Convert.ToInt32(dr["RequirementPackageSubscriptionID"]);
                    requirementPackageCategoryStatus.RequirementCategoryStatusCode = Convert.ToString(dr["RequirementCategoryStatusCode"]);
                    requirementPackageCategoryStatus.ApplicantName = dr["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                    requirementPackageCategoryStatus.RotationName = dr["RotationName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RotationName"]);
                    requirementPackageCategoryStatus.PackageName = dr["PackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PackageName"]);
                    requirementPackageCategoryStatus.OrganizationUserID = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                    requirementPackageCategoryStatus.Email = dr["Email"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Email"]);
                    requirementPackageCategoryStatus.UserName = dr["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UserName"]);
                    requirementPackageCategoryStatus.RotationID = Convert.ToInt32(dr["RotationID"]);//UAT-3364
                    requirementPackageCategoryStatusList.Add(requirementPackageCategoryStatus);
                }
                commonResponse.Result = requirementPackageCategoryStatusList;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<ApplicantDocumentContract>> IApplicantClinicalRotation.SaveApplicantUploadDocument(ServiceRequest<List<ApplicantDocumentContract>, Int32, Int32> data)
        {
            ServiceResponse<List<ApplicantDocumentContract>> commonResponse = new ServiceResponse<List<ApplicantDocumentContract>>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.SaveApplicantUploadDocument(data.Parameter1, data.Parameter2, data.SelectedTenantId, data.Parameter3);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IApplicantClinicalRotation.DeleteAppRequirementItemFieldData(ServiceRequest<Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.DeleteAppRequirementItemFieldData(data.Parameter1, data.Parameter2, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IApplicantClinicalRotation.IsDocumentAlreadyUploaded(ServiceRequest<String, Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                commonResponse.Result = ApplicantRequirementManager.IsDocumentAlreadyUploaded(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId,
                                                                                                      data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<RequirementFieldVideoData> IApplicantClinicalRotation.GetRequirementFieldVideoData(ServiceRequest<Int32> data)
        {
            ServiceResponse<RequirementFieldVideoData> commonResponse = new ServiceResponse<RequirementFieldVideoData>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetRequirementFieldVideoData(data.Parameter, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<RequirementObjectTreeContract> IApplicantClinicalRotation.GetObjectTreeProperty(ServiceRequest<Int32> data)
        {
            ServiceResponse<RequirementObjectTreeContract> commonResponse = new ServiceResponse<RequirementObjectTreeContract>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetObjectTreeProperty(data.Parameter, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<List<RequirementObjectTreeContract>> IApplicantClinicalRotation.GetAttributeObjectTreeProperties(ServiceRequest<ApplicantRequirementParameterContract, Int32> data)
        {
            ServiceResponse<List<RequirementObjectTreeContract>> commonResponse = new ServiceResponse<List<RequirementObjectTreeContract>>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetAttributeObjectTreeProperties(data.Parameter1, data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<ApplicantDocumentContract> IApplicantClinicalRotation.GetClientSystemDocument(ServiceRequest<Int32> data)
        {
            ServiceResponse<ApplicantDocumentContract> commonResponse = new ServiceResponse<ApplicantDocumentContract>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetClientSystemDocument(data.Parameter, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<ObjectAttributeContract> IApplicantClinicalRotation.GetObjectTreeProperties(ServiceRequest<Int32> data)
        {
            ServiceResponse<ObjectAttributeContract> commonResponse = new ServiceResponse<ObjectAttributeContract>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetObjectTreeProperties(data.Parameter, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<ViewDocumentContract> IApplicantClinicalRotation.GetViewDocumentData(ServiceRequest<Int32, Int32, Int32> data)
        {

            ServiceResponse<ViewDocumentContract> commonResponse = new ServiceResponse<ViewDocumentContract>();
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetViewDocumentData(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId, data.Parameter3, data.SelectedTenantId);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        #region Rule's Execution.
        ServiceResponse<Boolean> IApplicantClinicalRotation.ExecuteRequirementObjectBuisnessRules(ServiceRequest<List<RequirementRuleObject>, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                RequirementRuleManager.ExecuteRequirementObjectBuisnessRules(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId,
                                                                                                      data.SelectedTenantId);
                commonResponse.Result = true;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Boolean> IApplicantClinicalRotation.EvaluateRequirementPostSubmitRules(ServiceRequest<List<RequirementRuleObject>, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            try
            {
                UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
                RequirementRuleManager.EvaluateRequirementPostSubmitRules(data.Parameter1, data.Parameter2, activeUser.OrganizationUserId,
                                                                                                      data.SelectedTenantId);
                commonResponse.Result = true;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        #region GET EXPLANATORY NOTES
        ServiceResponse<String> IApplicantClinicalRotation.GetExplanatoryNotes(ServiceRequest<Int32, String, String> data)
        {
            ServiceResponse<String> commonResponse = new ServiceResponse<String>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetExplanatoryNotes(data.Parameter1, data.Parameter2, data.Parameter3,
                                                                                                        data.SelectedTenantId);

                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }
        #endregion

        //UAT-1523 Addition a notes box for each rotation for the student to input information
        ServiceResponse<Boolean> IApplicantClinicalRotation.UpdateRequirementPackageSubscriptionNotes(ServiceRequest<String, Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            commonResponse.Result = ApplicantRequirementManager.UpdateRequirementPackageSubscriptionNotes(data.Parameter1, data.Parameter2, data.SelectedTenantId, data.Parameter3); //UAT 1261
            return commonResponse;
        }

        /// <summary>
        /// IsApplicantDropped
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ServiceResponse<Boolean> IApplicantClinicalRotation.IsApplicantDropped(ServiceRequest<Int32, Int32, Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            commonResponse.Result = ApplicantClinicalRotationManager.IsApplicantDropped(data.Parameter1, data.Parameter2, data.Parameter3);
            return commonResponse;
        }

        #region UAT-2975:
        ServiceResponse<Boolean> IApplicantClinicalRotation.SyncRequirementVerificationToFlatData(ServiceRequest<Int32> data)
        {
            ServiceResponse<Boolean> commonResponse = new ServiceResponse<Boolean>();
            UserContext activeUser = OperationContext.Current.IsNotNull() ? OperationContext.Current.IncomingMessageHeaders.GetHeader<UserContext>("ActiveUser", "s"):ActiveUser;
            commonResponse.Result = RequirementVerificationManager.SyncRequirementVerificationToFlatData(data.Parameter.ToString(), data.SelectedTenantId, activeUser.OrganizationUserId); 
            return commonResponse;
        }
        #endregion

        
        ServiceResponse<DataTable> IApplicantClinicalRotation.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(ServiceRequest<Int32, string> data)
        {
            ServiceResponse<DataTable> commonResponse = new ServiceResponse<DataTable>();
            try
            {
                var res = ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(data.Parameter1, data.Parameter2);              
                commonResponse.Result = res;
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        ServiceResponse<Int32> IApplicantClinicalRotation.GetApplicantRequirementFieldData(ServiceRequest<Int32, Int32, String> data)
        {
            ServiceResponse<Int32> commonResponse = new ServiceResponse<Int32>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetApplicantRequirementFieldData(data.Parameter1, data.Parameter2,data.SelectedTenantId);

                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }


        #region UAT-4254

        ServiceResponse<List<RequirementCategoryDocUrl>> IApplicantClinicalRotation.GetRequirementCatDocUrls(ServiceRequest<Int32,Int32> data)
        {
            ServiceResponse<List<RequirementCategoryDocUrl>> commonResponse = new ServiceResponse<List<RequirementCategoryDocUrl>>();
            try
            {
                commonResponse.Result = ApplicantRequirementManager.GetRequirementCatDocUrls(data.Parameter1,data.Parameter2);
                return commonResponse;
            }
            catch (Exception ex)
            {
                base.LogApplicantClinicalRotationSvcError(ex);
                throw;
            }
        }

        #endregion
    }
}
