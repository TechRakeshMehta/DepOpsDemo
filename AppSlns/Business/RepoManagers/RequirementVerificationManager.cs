using System;
using System.Collections.Generic;
using System.Linq;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using Entity.SharedDataEntity;
using System.Data;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.Templates;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using System.Xml.Linq;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace Business.RepoManagers
{
    public class RequirementVerificationManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static RequirementVerificationManager()
        {
            BALUtils.ClassModule = "Requirement Verification Manager";
        }

        #endregion

        #region Common Methods

        /// <summary>
        /// Method to get requirement package types
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<RequirementPackageTypeContract> GetRequirementPackageTypes(Int32 tenantID)
        {
            try
            {
                List<Entity.ClientEntity.lkpRequirementPackageType> dataTypeList = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementPackageType>(tenantID).Where(x => !x.RPT_IsDeleted).ToList();
                return dataTypeList.Select(con => new RequirementPackageTypeContract
                {
                    ID = con.RPT_ID,
                    Code = con.RPT_Code,
                    Name = con.RPT_Name
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


        /// <summary>
        /// Method to get all possible data types of rotation fields
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<RequirementItemStatusContract> GetRequirementItemStatusTypes(Int32 tenantId)
        {
            try
            {
                var _lstReqItemStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementItemStatu>(tenantId);
                return _lstReqItemStatus.Select(ris => new RequirementItemStatusContract
                {
                    ReqItemstatusId = ris.RIS_ID,
                    ReqItemstatusName = ris.RIS_Name,
                    ReqItemstatusCode = ris.RIS_Code
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


        #endregion

        #region Requirement Verification Queue

        /// <summary>
        /// Get requirement verification queue search data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="searchDataContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        public static List<RequirementVerificationQueueContract> GetRequirementVerificationQueueSearch(Int32 tenantId, RequirementVerificationQueueContract searchDataContract, CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).GetRequirementVerificationQueueSearch(searchDataContract, customPagingArgsContract);
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

        public static List<ReqPkgSubscriptionIDList> GetReqPkgSubscriptionIdListForRotationVerification(RequirementVerificationQueueContract requirementVerificationQueueContract, Int32 CurrentReqPkgSubscriptionID, Int32 ApplicantRequirementItemID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).GetReqPkgSubscriptionIdListForRotationVerification(requirementVerificationQueueContract, CurrentReqPkgSubscriptionID, ApplicantRequirementItemID);
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

        #region Requirement Verification Details

        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        /// <param name="reqPkgSubId"></param>
        /// <returns></returns>
        public static List<RequirementVerificationDetailContract> GetVerificationDetailData(Int32 reqPkgSubId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).GetVerificationDetailData(reqPkgSubId);
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
        /// UAT 1626- Get the Requirement Verification Details by Category, including the data entered by Applicant.
        /// </summary>
        /// <param name="reqPkgSubId"></param>
        /// <returns></returns>
        public static List<RequirementVerificationDetailContract> GetRequirementItemsByCategoryId(Int32 reqPkgSubId, List<Int32> reqCatId, Int32 tenantId, Int32 rotationId)
        {
            try
            {
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).GetRequirementItemsByCategoryId(reqPkgSubId, reqCatId, rotationId);
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
        /// Save/Update the data of the Verification Details screen.
        /// </summary>
        /// <param name="reqVerificationDataToSave"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Dictionary<Int32, String> SaveVerificationData(RequirementVerificationData reqVerificationDataToSave, Int32 currentUserId, Int32 tenantId, ref Boolean isNewPackage)
        {
            try
            {
                var _lstReqCatStatus = LookupManager.GetLookUpData<lkpRequirementCategoryStatu>(tenantId).Where(rcs => !rcs.RCS_IsDeleted).ToList();
                var _lstReqItemStatus = LookupManager.GetLookUpData<lkpRequirementItemStatu>(tenantId).Where(ris => !ris.RIS_IsDeleted).ToList();

                List<Entity.ClientEntity.lkpObjectType> lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).SaveVerificationData(reqVerificationDataToSave, _lstReqCatStatus, _lstReqItemStatus, currentUserId, lkpObjectType, ref isNewPackage);
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


        public static Dictionary<Boolean, String> ValidateDocumentRules(RequirementVerificationCategoryData applicantReqCatData
                                                                   , Int32 packageSubscriptionId, Int32 tenantId)
        {
            try
            {
                List<Entity.ClientEntity.lkpObjectType> lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId).Where(x => x.OT_IsDeleted == false).ToList();
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).ValidateDocumentRules(applicantReqCatData, lkpObjectType, packageSubscriptionId);
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
        /// Get applicant documents by requirement package subscription id and category Id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="reqPkgSubId"></param>
        /// <param name="reqCatId"></param>
        /// <returns></returns>
        public static List<ApplicantFieldDocumentMappingContract> GetRequirementApplicantDocumentsByCategoryId(Int32 tenantId, Int32 reqPkgSubId, Int32 reqCatId)
        {
            try
            {
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).GetRequirementApplicantDocumentsByCategoryId(reqPkgSubId, reqCatId);
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
        #region UAT-1626 : update rotation package verification to mimic tracking verification
        /// <summary>
        /// Get Package and Category Details for Compliance Verification detail screen
        /// on basis of ReqPkgSubscriptionID
        /// </summary>
        /// <param name="ReqPkgSubscriptionID"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public static List<RequirementVerificationDetailContract> GetRequirementPackageCategoryData(Int32 ReqPkgSubscriptionID, Int32 TenantID, Int32 rotationId)
        {
            try
            {
                return BALUtils.GetRequirementVerificationRepoInstance(TenantID).GetRequirementPackageCategoryData(ReqPkgSubscriptionID, rotationId);
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


        public static Tuple<List<Int32>,String, Dictionary<Boolean, String>> RotationSubscriptionApproveAllPendingItems(Int32 tenantId, Int32 reqPkgSubsId, Int32 currentLoogedInUserId,Boolean isAdmin, ref Int32 affectedItemsCount)
        {
            try
            {
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).RotationSubscriptionApproveAllPendingItems(reqPkgSubsId, currentLoogedInUserId, isAdmin, ref  affectedItemsCount);
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

        //UAT 2371
        public static SystemEntityUserPermission GetSystemEntityUserPermission(Int32 clientOrganisationUserID, int tenantId)
        {
            try
            {
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).GetSystemEntityUserPermission(clientOrganisationUserID);
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


        #region UAT-2975: ADB Admin All Client Rotation Assignment and User work Queues.

        public static Boolean SyncRequirementVerificationToFlatData(String packageSubscriptionIds, Int32 tenantId, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).SyncRequirementVerificationToFlatData(packageSubscriptionIds, currentUserId);
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

        #region UAT-3957
        public static List<RequirementItemRejectionContract> GetRequirementRejectedItemDetailsForMail(String rejectedItemIds, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRequirementVerificationRepoInstance(tenantId).GetRequirementRejectedItemDetailsForMail(rejectedItemIds);
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
