using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Contracts;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.Utils.CommonPocoClasses;
using CoreWeb.IntsofSecurityModel;
using System.Data;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public class RotationRequirementDataEntryPresenter : Presenter<IRotationRequirementDataEntryView>
    {
        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        private ApplicantClinicalRotationProxy _applicantClinicalRotationProxy
        {
            get
            {
                return new ApplicantClinicalRotationProxy();
            }
        }

        private Boolean IsApplicantLoggedIn(Int32 CurrentloggedInUserID)
        {
            Entity.OrganizationUser OrgUser = SecurityManager.GetOrganizationUser(CurrentloggedInUserID);
            if (!OrgUser.IsNullOrEmpty() && CurrentloggedInUserID == OrgUser.OrganizationUserID)
                return Convert.ToBoolean(OrgUser.IsApplicant);
            return false;
        }
        /// <summary>
        /// Get Clinical Rotation data to bind Rotation Detail section
        /// </summary>
        public void BindRotationDetail()
        {//UAT-2040
            if (View.IsDisplayMultipleRotationDetails)
            {
                ServiceRequest<String> serviceRequest = new ServiceRequest<String>();
                serviceRequest.SelectedTenantId = View.SelectedTenantId;
                serviceRequest.Parameter = View.ClinicalRotationIDs;
                var _serviceResponse = _clientRotationProxy.GetClinicalRotationByIds(serviceRequest);
                View.lstclinicalRotationDetailContract = _serviceResponse.Result;
            }
            else
            {
                ServiceRequest<Int32, Int32?> serviceRequest = new ServiceRequest<Int32, Int32?>();
                serviceRequest.SelectedTenantId = View.SelectedTenantId;
                serviceRequest.Parameter1 = View.ClinicalRotationID;
                serviceRequest.Parameter2 = null;
                var _serviceResponse = _clientRotationProxy.GetClinicalRotationById(serviceRequest);
                View.ClinicalRotationDetails = _serviceResponse.Result;
            }
        }

        public void GetSubscriptionDetail()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.RequirementPackageSubscriptionID;
            serviceRequest.Parameter2 = View.SelectedTenantId;
            var _serviceResponse = _applicantClinicalRotationProxy.GetRequirementPackageSubscriptionData(serviceRequest);
            View.RotationSubscriptionDetail = _serviceResponse.Result1;
            View.RotationPackageDetail = _serviceResponse.Result2;
            View.RequirementPackageId = View.RotationSubscriptionDetail.RequirementPackageID;
            //UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            GetComplianceRequiredCategories();
            GetRequirementItemsAboutToExpire();
        }
        #region UAT-3077
        public List<RequirementItemPaymentContract> GetItemPaymentDetail(Int32 subscriptionID)
        {
            List<RequirementItemPaymentContract> result = new List<RequirementItemPaymentContract>();
            var data = ComplianceDataManager.GetItemPaymentDetail(subscriptionID, View.SelectedTenantId, true);
            data.ForEach(s => result.Add(new RequirementItemPaymentContract()
            {

                PkgName = s.PkgName,
                CategoryName = s.CategoryName,
                ItemName = s.ItemName,
                PkgId = s.PkgId,
                CategoryID = s.CategoryID,
                ItemID = s.ItemID,
                PkgSubscriptionId = s.PkgSubscriptionId,
                IsRequirementPackage = true,
                TotalPrice = s.TotalPrice,
                PaidAmount = s.PaidAmount,
                ItemDataId = s.ItemDataId,
                OrderStatus = s.OrderStatus,
                OrderStatusCode = s.OrderStatusCode,
                orderID = s.orderID,
                OrderNumber = s.OrderNumber,
                invoiceNumber = s.invoiceNumber,
                OrganizationUserProfileID = s.OrganizationUserProfileID,
            }));
            return result;
        }
        #endregion
        public void GetItemsAvailableForDataEntry(List<Int32> expiringReqItemIds)
        {
            ApplicantRequirementCategoryDataContract alreadyEnteredCategoryData;
            List<Int32> alreadyEnteredItemIds = new List<Int32>();
            if (View.RotationSubscriptionDetail.ApplicantRequirementCategoryData.IsNotNull())
            {
                alreadyEnteredCategoryData = View.RotationSubscriptionDetail.ApplicantRequirementCategoryData.FirstOrDefault(cond => cond.RequirementCategoryID ==
                                                                                                                             View.RequirementCategoryId);
                if (alreadyEnteredCategoryData.IsNotNull() && alreadyEnteredCategoryData.ApplicantRequirementItemData.IsNotNull())
                {
                    alreadyEnteredItemIds = alreadyEnteredCategoryData.ApplicantRequirementItemData.Where(s => !expiringReqItemIds.Contains(s.RequirementItemID)).Select(cond => cond.RequirementItemID).ToList();
                }
            }
            if (View.RotationSubscriptionDetail.IsNotNull())
            {
                View.RequirementPackageId = View.RotationSubscriptionDetail.RequirementPackageID;
            }
            RequirementCategoryContract requirementCategory = View.RotationPackageDetail.LstRequirementCategory.FirstOrDefault(cond => cond.RequirementCategoryID == View.RequirementCategoryId);
            if (requirementCategory.IsNotNull())
            {
                View.lstAvailableItems = requirementCategory.LstRequirementItem.Where(sel => !alreadyEnteredItemIds.Contains(sel.RequirementItemID) && sel.IsEditableByApplicant == true).ToList();
            }
        }

        /// <summary>
        /// Save/Updfate dynamic applicant form data
        /// </summary>
        public Dictionary<Boolean, String> SaveAppRequirementFieldData()
        {
            //UAT-3805
            List<Int32> approvedCategoryIDs = new List<Int32>();
            List<ItemDocNotificationRequestDataContract> itemDocNotificationRequestData = new List<ItemDocNotificationRequestDataContract>();

            if (View.RequirementCategoryDataContract.RequirementCategoryDataID > 0)
            {
                approvedCategoryIDs.Add(View.RequirementCategoryDataContract.RequirementCategoryID);
                approvedCategoryIDs = ProfileSharingManager.GetApprovedCategorIDs(View.SelectedTenantId, View.RequirementPackageSubscriptionID
                                                                                      , approvedCategoryIDs, lkpUseTypeEnum.ROTATION.GetStringValue());
            }
            //Set properties in applicant Requirement Parameter contract to save the data.
            ApplicantRequirementParameterContract appParameterContract = new ApplicantRequirementParameterContract();
            appParameterContract.RequirementPkgSubscriptionId = View.RequirementPackageSubscriptionID;
            appParameterContract.TenantId = View.SelectedTenantId;
            appParameterContract.AppRequirementCategoryData = View.RequirementCategoryDataContract;
            appParameterContract.AppRequirementItemData = View.RequirementItemDataContract;
            appParameterContract.AppRequirementCategoryData = View.RequirementCategoryDataContract;
            appParameterContract.AppRequirementFieldDataList = View.ApplicantFieldDataContractList;
            appParameterContract.AppFieldDocuments = View.FieldDocuments;
            appParameterContract.SignedApplicantDocuments = View.AppSignedDocumentDic;
            //appParameterContract.IsUIValidationApplicable = View.IsUIValidationApplicable;
            appParameterContract.RequirementPackageId = View.RequirementPackageId;
            appParameterContract.IsNewPackage = View.RotationPackageDetail.IsNewPackage;
            //Start UAT-5062
            appParameterContract.IsUploadDocUpdated = View.IsUploadDocUpdated;
            //End UAT-5062

            ServiceRequest<ApplicantRequirementParameterContract, Int32, Int32> serviceRequest = new ServiceRequest<ApplicantRequirementParameterContract, Int32, Int32>();
            serviceRequest.Parameter1 = appParameterContract;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            serviceRequest.Parameter3 = View.OrgUsrID;


            //Getting Previous Package Status
            string previousPackageStatusCode = View.RotationSubscriptionDetail.RequirementPackageSubscriptionStatusCode;
            ServiceRequest<Int32, String> ptrevserviceRqst = new ServiceRequest<Int32, String>();
            ptrevserviceRqst.Parameter1 = View.SelectedTenantId;
            ptrevserviceRqst.Parameter2 = Convert.ToString(View.RequirementPackageSubscriptionID);
            var _ptrevserviceRspse = _applicantClinicalRotationProxy.GetPackageSubscriptionCategoryStatus(ptrevserviceRqst);
            var prevSubscriptionStatus = _ptrevserviceRspse.Result;

            Boolean IsApplicant = IsApplicantLoggedIn(View.CurrentLoggedInUserId);
            DataTable prevData = new DataTable();
            if (IsApplicant)
            {
                prevData = ApplicantRequirementManager.GetMailDataForItemSubmitted(Convert.ToString(View.RequirementPackageSubscriptionID), View.SelectedTenantId); //UAT-2905
            }

            var _serviceResponse = _applicantClinicalRotationProxy.SaveApplicantRequirementData(serviceRequest);
            Boolean isSuccess = !_serviceResponse.Result.Keys.FirstOrDefault();
            if (isSuccess)
            {
                View.IsAppDataSavedSuccessfully = true;

                if (!appParameterContract.IsNullOrEmpty() && !appParameterContract.SignedApplicantDocuments.IsNullOrEmpty() && appParameterContract.SignedApplicantDocuments.Count > AppConsts.NONE)
                {
                    #region UAT3532
                    if (View.ToSaveApplicantUploadedDocuments.IsNotNull() && View.ToSaveApplicantUploadedDocuments.Count() >= AppConsts.NONE)
                    {
                        List<String> documentIds = Convert.ToString(_serviceResponse.Result.Values.FirstOrDefault()).Split(',').ToList();
                        List<Int32> ViewDocumentFieldDocumentList = new List<Int32>();
                        if (documentIds.Count > AppConsts.NONE)
                        {
                            documentIds.ForEach(sel => ViewDocumentFieldDocumentList.Add(Convert.ToInt32(sel)));
                            View.ViewDocumentFieldDocumentList = ViewDocumentFieldDocumentList;
                        }
                        CallParallelTaskPdfConversion();
                    }

                    #endregion

                }

                #region  UAT-3273
                ServiceRequest<Int32, string> request = new ServiceRequest<Int32, string>();
                request.Parameter1 = View.SelectedTenantId;
                request.Parameter2 = View.RequirementPackageSubscriptionID.ToString();
                // var dataBeforeRuleExecution = _applicantClinicalRotationProxy.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(request);
                var dataBeforeRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(View.SelectedTenantId, View.RequirementPackageSubscriptionID.ToString());
                #endregion

                if (!View.RotationPackageDetail.IsNewPackage)
                {
                    EvaluateRequirementBuisnessRules(View.RequirementCategoryDataContract.RequirementCategoryID, View.RequirementItemDataContract.RequirementItemID);
                }
                else
                {
                    EvaluateRequirementDynamicBuisnessRules(View.RequirementCategoryDataContract.RequirementCategoryID, View.RequirementItemDataContract.RequirementItemID);
                }

                #region  UAT-3273
                //var dataAfterRuleExecution = _applicantClinicalRotationProxy.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(request);
                var dataAfterRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(View.SelectedTenantId, View.RequirementPackageSubscriptionID.ToString());
                ProfileSharingManager.CheckAndSendNotificationForNewlyApprovedRotations(dataBeforeRuleExecution, dataAfterRuleExecution, View.SelectedTenantId);

                #endregion
                //Changes related to bud ID:15048
                ComplianceDataManager.SendNotificationOnItemStatusChangedToReviewStatus(true, View.SelectedTenantId, View.RequirementPackageSubscriptionID, View.RequirementPackageId,
                                                                                        View.RequirementCategoryDataContract.RequirementCategoryID,
                                                                                        View.RequirementItemDataContract.RequirementItemID, View.CurrentLoggedInUserId,
                                                                                        View.RotationSubscriptionDetail.ApplicantOrgUserID,
                                                                                        View.ItemPreviousStatsCode);

                ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(View.SelectedTenantId, View.RequirementPackageSubscriptionID, View.CurrentLoggedInUserId, previousPackageStatusCode);

                //Changes related to UAT-2753
                ServiceRequest<Int32, String> serviceRqst = new ServiceRequest<Int32, String>();
                serviceRqst.Parameter1 = View.SelectedTenantId;
                serviceRqst.Parameter2 = Convert.ToString(View.RequirementPackageSubscriptionID);
                var _serviceRspse = _applicantClinicalRotationProxy.GetPackageSubscriptionCategoryStatus(serviceRqst);
                var SubscriptionStatus = _serviceRspse.Result;
                String prevCateCode = String.Empty, currCategoryCode = String.Empty;
                bool sendEmail = true;
                if ((prevSubscriptionStatus.Any() && prevSubscriptionStatus.Count == 1) && (SubscriptionStatus.Any() && SubscriptionStatus.Count == 1))
                {
                    if (prevSubscriptionStatus[0].RequirementCategoryStatusCode.Equals(SubscriptionStatus[0].RequirementCategoryStatusCode))
                    {
                        sendEmail = false;
                    }
                    else
                    {
                        sendEmail = true;
                    }
                }

                if (sendEmail && SubscriptionStatus.Any())
                {
                    if (SubscriptionStatus[0].RequirementPackageSubscriptionID.Equals(View.RequirementPackageSubscriptionID))
                    {
                        var status = String.Empty;
                        if (SubscriptionStatus[0].RequirementCategoryStatusCode.Equals(AppConsts.REQUIREMENT_PACKAGE_SUBSCRIPTION_APPROVED_CODE))
                        {
                            status = "Approved";
                        }
                        else if (SubscriptionStatus[0].RequirementCategoryStatusCode.Equals(AppConsts.REQUIREMENT_PACKAGE_SUBSCRIPTION_PENDING_REVIEW_CODE))
                        {
                            status = "Pending Review";
                        }

                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                        dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, SubscriptionStatus[0].ApplicantName);
                        dictMailData.Add(EmailFieldConstants.ROTATION_NAME, SubscriptionStatus[0].RotationName);
                        dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, SubscriptionStatus[0].PackageName);
                        dictMailData.Add(EmailFieldConstants.TENANT_ID, View.SelectedTenantId);
                        dictMailData.Add(EmailFieldConstants.STATUS, status);

                        Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();



                        //UAT-3160
                        String rotationHierarchyIDs = String.Empty;
                        if (View.RequirementPackageSubscriptionID > AppConsts.NONE)
                        {
                            rotationHierarchyIDs = ApplicantRequirementManager.GetRotationHierarchyIdsBasedOnSubscriptionID(View.SelectedTenantId, View.RequirementPackageSubscriptionID);
                        }
                        #region UAT-3364 - Granular permission for Rotation Creator
                        Boolean IsAllowed = ComplianceDataManager.CheckRotationCreatorGranularPermissionsByOrgUserIdForSendNotificationForCategoryApproved(SubscriptionStatus[0].OrganizationUserID);
                        #endregion
                        if (IsAllowed)
                        {
                            mockData.UserName = SubscriptionStatus[0].UserName;
                            mockData.EmailID = SubscriptionStatus[0].Email;
                            mockData.ReceiverOrganizationUserID = SubscriptionStatus[0].OrganizationUserID;
                        }
                        else
                        {
                            mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                            mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                            mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                        }
                        //Send mail
                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_ALL_ROTATION_CATEGORIES_APPROVED_OR_PENDING_REVIEW, dictMailData, mockData, View.SelectedTenantId, -1, null, null, true, false, null, rotationHierarchyIDs, SubscriptionStatus[0].RotationID);

                        //UAT-4015
                        ApplicantRequirementManager.SendMailToInstPrecpReqPKgCompliantStatus(View.SelectedTenantId, View.RequirementPackageSubscriptionID, View.CurrentLoggedInUserId);
                    }
                }

                #region UAT-2905 : Notification to admin when item is submitted.
                if (IsApplicant)
                {
                    ComplianceDataManager.SendNotificationToAdminForItemSubmitted(true, View.SelectedTenantId, Convert.ToString(View.RequirementPackageSubscriptionID), Convert.ToString(View.RequirementItemId), prevData, View.CurrentLoggedInUserId);
                }
                #endregion

                //UAT-3805
                CreateItemDocNotificationRequestData(approvedCategoryIDs, itemDocNotificationRequestData);


                //UAT-2975:
                SyncVerificationDataToFlat();

                #region UAT - 3164
                //Getting target Package Subscription Ids
                ServiceRequest<Int32, Int32> rotReqSubsServiceRequest = new ServiceRequest<Int32, Int32>();
                rotReqSubsServiceRequest.SelectedTenantId = View.SelectedTenantId;
                rotReqSubsServiceRequest.Parameter1 = View.RequirementPackageSubscriptionID;
                rotReqSubsServiceRequest.Parameter2 = View.RequirementCategoryDataContract.RequirementCategoryID;
                var _rotReqSubsserviceResponse = _clientRotationProxy.GetTargetReqPackageSubscriptionIDsForSync(rotReqSubsServiceRequest);
                IEnumerable<DataRow> ReqSubIDs = _rotReqSubsserviceResponse.Result.AsEnumerable();
                String lstRPSID = String.Join(",", ReqSubIDs.Select(col => col["TargetRPS_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TargetRPS_ID"])).Distinct().ToList());
                var prevSyncRotData = Business.RepoManagers.ApplicantRequirementManager.GetMailDataForItemSubmitted(String.Join(",", lstRPSID), View.SelectedTenantId);

                //Calling Rotation Data live Movement
                ServiceRequest<Int32, Int32, Int32> rotMovementServiceRequest = new ServiceRequest<Int32, Int32, Int32>();
                rotMovementServiceRequest.SelectedTenantId = View.SelectedTenantId;
                rotMovementServiceRequest.Parameter1 = View.RequirementPackageSubscriptionID;
                rotMovementServiceRequest.Parameter2 = View.RequirementCategoryDataContract.RequirementCategoryID;
                rotMovementServiceRequest.Parameter3 = View.CurrentLoggedInUserId;
                var _rotMovementserviceResponse = _clientRotationProxy.PerformRotationLiveDataMovement(rotMovementServiceRequest);
                IEnumerable<DataRow> rows = _rotMovementserviceResponse.Result.AsEnumerable();
                List<ApplicantRequirementParameterContract> rotData = new List<ApplicantRequirementParameterContract>();

                rotData.AddRange(rows.Select(col => new ApplicantRequirementParameterContract
                {
                    RequirementPkgSubscriptionId = col["RPSID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RPSID"]),
                    RequirementCategoryId = col["ReqCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqCategoryID"]),
                    prevCategoryStatusCode = col["OldCategoryStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["OldCategoryStatus"]),
                    NewCategoryStatusCode = col["NewCategoryStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["NewCategoryStatus"]),
                    AppRequirementItemDataID = col["ApplicantRequirementItemDataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ApplicantRequirementItemDataID"]),
                }).ToList());

                //Sending Notifications
                foreach (ApplicantRequirementParameterContract item in rotData.DistinctBy(cond => new { cond.RequirementPkgSubscriptionId }))
                {
                    //Sending Notification on Rotation pkg status change 
                    String previousSynPackageStatusCode = ReqSubIDs.Where(cond => Convert.ToInt32(cond["TargetRPS_ID"]) == item.RequirementPkgSubscriptionId).Select(sel => Convert.ToString(sel["OldPackageStatus"])).FirstOrDefault();
                    ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(View.SelectedTenantId, item.RequirementPkgSubscriptionId, View.CurrentLoggedInUserId, previousSynPackageStatusCode);

                    //Sending BadgeForm Notification
                    List<Int32> lstARID = rotData.Where(cond => cond.RequirementPkgSubscriptionId == item.RequirementPkgSubscriptionId).Select(s => s.AppRequirementItemDataID).Distinct().ToList();
                    if (!lstARID.IsNullOrEmpty())
                    {
                        string arid = string.Join(",", lstARID);
                        ComplianceDataManager.SaveBadgeFormNotificationData(View.SelectedTenantId, null, arid, null, View.CurrentLoggedInUserId);
                    }

                    //Sending PackageNotificationMail
                    if (!item.prevCategoryStatusCode.Equals(item.NewCategoryStatusCode))
                    {
                        ServiceRequest<Int32, String> rotServiceRqst = new ServiceRequest<Int32, String>();
                        rotServiceRqst.Parameter1 = View.SelectedTenantId;
                        rotServiceRqst.Parameter2 = Convert.ToString(item.RequirementPkgSubscriptionId);
                        var _rotServiceRspse = _applicantClinicalRotationProxy.GetPackageSubscriptionCategoryStatus(rotServiceRqst);
                        var rotSubscriptionStatus = _rotServiceRspse.Result;

                        if (rotSubscriptionStatus.Any())
                        {
                            var status = String.Empty;
                            if (rotSubscriptionStatus[0].RequirementCategoryStatusCode.Equals(AppConsts.REQUIREMENT_PACKAGE_SUBSCRIPTION_APPROVED_CODE))
                            {
                                status = "Approved";
                            }
                            else if (rotSubscriptionStatus[0].RequirementCategoryStatusCode.Equals(AppConsts.REQUIREMENT_PACKAGE_SUBSCRIPTION_PENDING_REVIEW_CODE))
                            {
                                status = "Pending Review";
                            }

                            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                            dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, rotSubscriptionStatus[0].ApplicantName);
                            dictMailData.Add(EmailFieldConstants.ROTATION_NAME, rotSubscriptionStatus[0].RotationName);
                            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, rotSubscriptionStatus[0].PackageName);
                            dictMailData.Add(EmailFieldConstants.TENANT_ID, View.SelectedTenantId);
                            dictMailData.Add(EmailFieldConstants.STATUS, status);

                            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();


                            //UAT-3160
                            String rotationHierarchyIDs = String.Empty;
                            if (item.RequirementPkgSubscriptionId > AppConsts.NONE)
                            {
                                rotationHierarchyIDs = ApplicantRequirementManager.GetRotationHierarchyIdsBasedOnSubscriptionID(View.SelectedTenantId, item.RequirementPkgSubscriptionId);
                            }

                            #region UAT-3364 - Granular permission for Rotation Creator
                            Boolean IsAllowed = ComplianceDataManager.CheckRotationCreatorGranularPermissionsByOrgUserIdForSendNotificationForCategoryApproved(rotSubscriptionStatus[0].OrganizationUserID);
                            #endregion
                            if (IsAllowed)
                            {
                                mockData.UserName = rotSubscriptionStatus[0].UserName;
                                mockData.EmailID = rotSubscriptionStatus[0].Email;
                                mockData.ReceiverOrganizationUserID = rotSubscriptionStatus[0].OrganizationUserID;
                            }
                            else
                            {
                                mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                                mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                            }
                            //Send mail
                            CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_ALL_ROTATION_CATEGORIES_APPROVED_OR_PENDING_REVIEW, dictMailData, mockData, View.SelectedTenantId, -1, null, null, true, false, null, rotationHierarchyIDs, rotSubscriptionStatus[0].RotationID);

                            //UAT-4015
                            ApplicantRequirementManager.SendMailToInstPrecpReqPKgCompliantStatus(View.SelectedTenantId, item.RequirementPkgSubscriptionId, View.CurrentLoggedInUserId);
                        }
                    }

                    // Sending Notification To Admin For ItemSubmitted 
                    if (IsApplicant)
                    {
                        ComplianceDataManager.SendNotificationToAdminForItemSubmitted(true, View.SelectedTenantId, item.RequirementPkgSubscriptionId.ToString(), String.Empty, prevSyncRotData, View.CurrentLoggedInUserId);
                    }

                    //UAT-3805

                    List<Int32> categoryIds = rotData.Where(cond => cond.RequirementPkgSubscriptionId == item.RequirementPkgSubscriptionId
                                                                     && cond.prevCategoryStatusCode != RequirementCategoryStatus.APPROVED.GetStringValue())
                                                      .Select(s => s.RequirementCategoryId).Distinct().ToList();

                    ItemDocNotificationRequestDataContract itemDocRequestData = new ItemDocNotificationRequestDataContract();

                    String categoryIDs = String.Join(",", categoryIds);

                    itemDocRequestData.TenantID = View.SelectedTenantId;
                    itemDocRequestData.CategoryIds = categoryIDs;
                    itemDocRequestData.ApplicantOrgUserID = View.ApplicantID;
                    itemDocRequestData.ApprovedCategoryIds = String.Empty;
                    itemDocRequestData.RequestTypeCode = lkpUseTypeEnum.ROTATION.GetStringValue();
                    itemDocRequestData.PackageSubscriptionID = null;
                    itemDocRequestData.RPS_ID = item.RequirementPkgSubscriptionId;
                    itemDocRequestData.CurrentLoggedInUserID = View.CurrentLoggedInUserId;
                    itemDocNotificationRequestData.Add(itemDocRequestData);
                }

                if (rotData.IsNotNull() && rotData.Count == AppConsts.NONE)
                {

                    if (View.RequirementItemDataContract.IsNotNull() && View.RequirementItemDataContract.RequirementItemDataID > AppConsts.NONE)
                    {
                        ApplicantRequirementItemData applicantRequirementItemData = ApplicantRequirementManager.GetApplicantRequirementItemDataByID(View.SelectedTenantId, View.RequirementItemDataContract.RequirementItemDataID);
                        if (applicantRequirementItemData != null)
                        {
                            if (applicantRequirementItemData.lkpRequirementItemStatu.RIS_Code == RequirementItemStatus.APPROVED.GetStringValue() && applicantRequirementItemData.RequirementItem.RI_IsPaymentType.HasValue && applicantRequirementItemData.RequirementItem.RI_IsPaymentType == true)
                            {
                                String ARID_ID = Convert.ToString(View.RequirementItemDataContract.RequirementItemDataID);
                                ComplianceDataManager.SaveBadgeFormNotificationData(View.SelectedTenantId, null, ARID_ID, null, View.CurrentLoggedInUserId);
                            }
                        }
                    }
                }
                //UAT-3805
                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                dicParam.Add("CategoryData", itemDocNotificationRequestData);
                ProfileSharingManager.RunParallelTaskItemDocNotificationOnCatApproval(dicParam, LoggerService, ExceptiomService);

                #endregion
            }
            return _serviceResponse.Result;
        }

        public void AddApplicantUploadedDocuments(String filePath)
        {
            View.SavedApplicantDocuments = new Dictionary<Int32, String>();
            ServiceRequest<List<ApplicantDocumentContract>, Int32, Int32> serviceRequest = new ServiceRequest<List<ApplicantDocumentContract>, Int32, Int32>();
            serviceRequest.Parameter1 = View.ToSaveApplicantUploadedDocuments;
            serviceRequest.Parameter2 = View.CurrentLoggedInUserId;
            serviceRequest.Parameter3 = View.OrgUsrID; //UAT 1261

            serviceRequest.SelectedTenantId = View.SelectedTenantId;

            var _serviceResponse = _applicantClinicalRotationProxy.SaveApplicantUploadDocument(serviceRequest);
            View.ToSaveApplicantUploadedDocuments = new List<ApplicantDocumentContract>();
            View.ToSaveApplicantUploadedDocuments = _serviceResponse.Result;
            View.ToSaveApplicantUploadedDocuments.ForEach(cnd =>
            {
                if (View.SavedApplicantDocuments.IsNull())
                {
                    View.SavedApplicantDocuments = new Dictionary<Int32, String>();
                }
                View.SavedApplicantDocuments.Add(cnd.ApplicantDocumentId, cnd.FileName);
            });


            //foreach (var applicantDocument in View.ToSaveApplicantUploadedDocuments)
            //{
            //    var Id = ComplianceDataManager.SaveApplicantDocument(applicantDocument, View.SelectedTenantId);
            //    View.SavedApplicantDocuments.Add(Id, applicantDocument.FileName);                //String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            //    //String newFileName = filePath + "UD_" + View.TenantID.ToString() + "_" + Id.ToString() + "_" + date + Path.GetExtension(applicantDocument.FileName);
            //    //String newFileName = applicantDocument.DocumentPath.Replace("xxx",Id.ToString());
            //    //ComplianceDataManager.UpdateDocumentPath(newFileName, applicantDocument.DocumentPath, Id, View.TenantID, applicantDocument.OrganizationUserID.Value);

            //}
        }

        /// <summary>
        /// To check if document is already uploaded
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="documentSize"></param>
        /// <param name="documentUploadedBytes"></param>
        /// <returns></returns>
        public String IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, byte[] documentUploadedBytes)
        {
            ServiceRequest<String, Int32, Int32> serviceRequestDocCheck = new ServiceRequest<String, Int32, Int32>();
            serviceRequestDocCheck.Parameter1 = documentName;
            serviceRequestDocCheck.Parameter2 = documentSize;
            serviceRequestDocCheck.SelectedTenantId = View.SelectedTenantId;

            ServiceRequest<Int32> serviceRequestToGetDoc = new ServiceRequest<Int32>();
            serviceRequestToGetDoc.Parameter = View.CurrentLoggedInUserId;
            serviceRequestToGetDoc.SelectedTenantId = View.SelectedTenantId;

            var _serviceResponseDocCheck = _applicantClinicalRotationProxy.IsDocumentAlreadyUploaded(serviceRequestDocCheck);
            if (_serviceResponseDocCheck.Result)
            {
                return documentName;
            }
            //Compare original document MD5Hash
            var _serviceResponseToGetDoc = _applicantClinicalRotationProxy.GetApplicantDocument(serviceRequestToGetDoc);
            List<ApplicantDocumentContract> applicantDocuments = _serviceResponseToGetDoc.Result;
            String md5Hash = CommonFileManager.GetMd5Hash(documentUploadedBytes);

            if (applicantDocuments.IsNotNull())
            {
                var applicantDocument = applicantDocuments.FirstOrDefault(x => x.OriginalDocMD5Hash == md5Hash);
                if (applicantDocument.IsNotNull())
                {
                    return applicantDocument.FileName;
                }
            }

            return null;
        }

        public void DeleteAppRequirementItemFieldData(Int32 appReqItemDataId, Int32 reqCatID, Int32 reqItemId)
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = appReqItemDataId;
            serviceRequest.Parameter2 = View.OrgUsrID; //View.CurrentLoggedInUserId;
            serviceRequest.SelectedTenantId = View.SelectedTenantId;

            var _serviceResponse = _applicantClinicalRotationProxy.DeleteAppRequirementItemFieldData(serviceRequest);

            if (_serviceResponse.Result)
            {
                if (!View.RotationPackageDetail.IsNewPackage)
                    EvaluateRequirementBuisnessRules(reqCatID, reqItemId);
                else
                    EvaluateRequirementDynamicBuisnessRules(reqCatID, reqItemId);
                //Send Mail 
                ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(View.SelectedTenantId, View.RequirementPackageSubscriptionID, View.CurrentLoggedInUserId, View.RotationSubscriptionDetail.RequirementPackageSubscriptionStatusCode);

                //UAT-2975:
                SyncVerificationDataToFlat();

                #region UAT - 3164
                //Getting target Package Subscription Ids
                ServiceRequest<Int32, Int32> rotReqSubsServiceRequest = new ServiceRequest<Int32, Int32>();
                rotReqSubsServiceRequest.SelectedTenantId = View.SelectedTenantId;
                rotReqSubsServiceRequest.Parameter1 = View.RequirementPackageSubscriptionID;
                rotReqSubsServiceRequest.Parameter2 = reqCatID;
                var _rotReqSubsserviceResponse = _clientRotationProxy.GetTargetReqPackageSubscriptionIDsForSync(rotReqSubsServiceRequest);
                IEnumerable<DataRow> ReqSubIDs = _rotReqSubsserviceResponse.Result.AsEnumerable();
                List<Int32> lstReqSubID = ReqSubIDs.Select(col => col["TargetRPS_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TargetRPS_ID"])).Distinct().ToList();

                //Calling Rotation Data live Movement
                ServiceRequest<Int32, Int32, Int32> rotMovementServiceRequest = new ServiceRequest<Int32, Int32, Int32>();
                rotMovementServiceRequest.SelectedTenantId = View.SelectedTenantId;
                rotMovementServiceRequest.Parameter1 = View.RequirementPackageSubscriptionID;
                rotMovementServiceRequest.Parameter2 = reqCatID;
                rotMovementServiceRequest.Parameter3 = View.CurrentLoggedInUserId;
                var _rotMovementserviceResponse = _clientRotationProxy.PerformRotationLiveDataMovement(rotMovementServiceRequest);

                //Sending Notification on Rotation pkg status change 
                foreach (var RequirementPackageSubscriptionID in lstReqSubID)
                {
                    String previousSynPackageStatusCode = ReqSubIDs.Where(cond => Convert.ToInt32(cond["TargetRPS_ID"]) == RequirementPackageSubscriptionID).Select(sel => Convert.ToString(sel["OldPackageStatus"])).FirstOrDefault();
                    ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(View.SelectedTenantId, RequirementPackageSubscriptionID, View.CurrentLoggedInUserId, previousSynPackageStatusCode);
                }
                #endregion
            }
        }

        private void EvaluateRequirementBuisnessRules(Int32 reqCategoryId, Int32 reqItemId)
        {
            List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();
            RequirementRuleObject ruleObjectMappingForPackage = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Package.GetStringValue(),
                RuleObjectId = Convert.ToString(View.RequirementPackageId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RequirementRuleObject ruleObjectMappingForCategory = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                RuleObjectId = Convert.ToString(reqCategoryId),
                RuleObjectParentId = Convert.ToString(View.RequirementPackageId)
            };

            RequirementRuleObject ruleObjectMappingForItem = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                RuleObjectId = Convert.ToString(reqItemId),
                RuleObjectParentId = Convert.ToString(reqCategoryId)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);

            ServiceRequest<List<RequirementRuleObject>, Int32> serviceRequest = new ServiceRequest<List<RequirementRuleObject>, Int32>();
            serviceRequest.Parameter1 = ruleObjectMappingList;
            serviceRequest.Parameter2 = View.RequirementPackageSubscriptionID;
            serviceRequest.SelectedTenantId = View.SelectedTenantId;

            _applicantClinicalRotationProxy.ExecuteRequirementObjectBuisnessRules(serviceRequest);
        }

        private void EvaluateRequirementDynamicBuisnessRules(Int32 reqCategoryId, Int32 reqItemId)
        {
            List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();

            RequirementRuleObject ruleObjectMappingForCategory = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                RuleObjectId = Convert.ToString(reqCategoryId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RequirementRuleObject ruleObjectMappingForItem = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                RuleObjectId = Convert.ToString(reqItemId),
                RuleObjectParentId = Convert.ToString(reqCategoryId)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);

            if (!View.ApplicantFieldDataContractList.IsNullOrEmpty())
            {
                foreach (ApplicantRequirementFieldDataContract attributeData in View.ApplicantFieldDataContractList)
                {
                    RequirementRuleObject ruleObjectMappingForAttribute = new RequirementRuleObject
                    {
                        RuleObjectTypeCode = ObjectType.Compliance_ATR.GetStringValue(),
                        RuleObjectId = Convert.ToString(attributeData.RequirementFieldID),
                        RuleObjectParentId = Convert.ToString(reqItemId)
                    };
                    ruleObjectMappingList.Add(ruleObjectMappingForAttribute);
                }
            }

            ServiceRequest<List<RequirementRuleObject>, Int32> serviceRequest = new ServiceRequest<List<RequirementRuleObject>, Int32>();
            serviceRequest.Parameter1 = ruleObjectMappingList;
            serviceRequest.Parameter2 = View.RequirementPackageSubscriptionID;
            serviceRequest.SelectedTenantId = View.SelectedTenantId;

            _applicantClinicalRotationProxy.EvaluateRequirementPostSubmitRules(serviceRequest);
        }

        /// <summary>
        /// This method is used to call the Parallel Task for Pdf conversion
        /// </summary>
        public void CallParallelTaskPdfConversion()
        {
            Dictionary<String, Object> conversionData = new Dictionary<String, Object>();
            //Use Poco class so that Entity will not get updated while running parallel tasks
            List<ApplicantDocumentPocoClass> lstApplicantDoc = new List<ApplicantDocumentPocoClass>();

            if ((!View.ToSaveApplicantUploadedDocuments.IsNullOrEmpty() && View.ToSaveApplicantUploadedDocuments.Count() >= AppConsts.NONE) || (!View.ViewDocumentFieldDocumentList.IsNullOrEmpty() && View.ViewDocumentFieldDocumentList.Count > AppConsts.NONE))
            {
                if (View.ToSaveApplicantUploadedDocuments.Where(d => d.ApplicantDocumentId != AppConsts.NONE).Any())
                {
                    foreach (var doc in View.ToSaveApplicantUploadedDocuments.Where(d => d.ApplicantDocumentId != AppConsts.NONE).ToList())
                    {
                        ApplicantDocumentPocoClass appDoc = new ApplicantDocumentPocoClass();
                        appDoc.ApplicantDocumentID = doc.ApplicantDocumentId;
                        appDoc.FileName = doc.FileName;
                        appDoc.DocumentPath = doc.DocumentPath;
                        appDoc.PdfDocPath = String.Empty;// doc.PdfDocPath;
                        appDoc.IsCompressed = false;//doc.IsCompressed;
                        appDoc.Size = doc.Size;
                        lstApplicantDoc.Add(appDoc);
                    }
                }
                #region UAT-3532
                var viewDocListAttr = View.ToSaveApplicantUploadedDocuments.Where(d => d.ApplicantDocumentId == AppConsts.NONE).ToList();
                if (viewDocListAttr.Count > AppConsts.NONE)
                {
                    int i = 0;
                    foreach (var doc in View.ViewDocumentFieldDocumentList)
                    {
                        var docDetails = viewDocListAttr[i];
                        ApplicantDocumentPocoClass appDoc = new ApplicantDocumentPocoClass();
                        appDoc.ApplicantDocumentID = doc;
                        appDoc.FileName = docDetails.FileName;
                        appDoc.DocumentPath = docDetails.DocumentPath;
                        appDoc.PdfDocPath = String.Empty;// doc.PdfDocPath;
                        appDoc.IsCompressed = false;//doc.IsCompressed;
                        appDoc.Size = docDetails.Size;
                        lstApplicantDoc.Add(appDoc);
                        i = i + 1;
                    }
                }
                #endregion
                //conversionData.Add("ApplicantUploadedDocuments", View.ToSaveApplicantUploadedDocuments);
                conversionData.Add("ApplicantUploadedDocuments", lstApplicantDoc);
            }
            else
            {
                conversionData.Add("ApplicantUploadedDocuments", null);
            }
            conversionData.Add("OrganizationUserId", View.OrganiztionUserID);
            conversionData.Add("CurrentLoggedUserID", View.OrgUsrID); //View.CurrentLoggedInUserId UAT 1261
            conversionData.Add("TenantID", View.SelectedTenantId);

            Dictionary<String, Object> mergingData = null;

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            if (!conversionData.IsNullOrEmpty() && conversionData.Count > AppConsts.NONE)
            {
                //ParallelTaskContext.ParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
                Business.RepoManagers.DocumentManager.RunParallelTaskPdfConversionMerging(ConvertDocumentsIntoPdf, conversionData, MergeDocIntoUnifiedPdf, mergingData, LoggerService, ExceptiomService);
            }
        }

        /// <summary>
        /// This method is used to convert the list of documents into pdf document
        /// </summary>
        /// <param name="conversionData">conversionData (Data dictionary that conatins the applicantdocument table object ,tenantId,currentLoggedUserID)</param>
        private void ConvertDocumentsIntoPdf(Dictionary<String, Object> conversionData)
        {
            if (!conversionData.IsNullOrEmpty() && conversionData.Count > AppConsts.NONE)
            {
                Int32 tenantId;
                Int32 CurrentLoggedUserID;
                List<ApplicantDocumentPocoClass> applicantDocument = conversionData.GetValue("ApplicantUploadedDocuments") as List<ApplicantDocumentPocoClass>;
                conversionData.TryGetValue("TenantID", out tenantId);
                conversionData.TryGetValue("CurrentLoggedUserID", out CurrentLoggedUserID);
                Business.RepoManagers.DocumentManager.ConvertApplicantDocumentToPDF(applicantDocument, tenantId, CurrentLoggedUserID);
            }
        }

        /// <summary>
        /// This method is used to merge converted pdf documents into unified pdf document
        /// </summary>
        /// <param name="mergingData">mergingData (Data dictionary that conatins the organizationUserId,tenantId,CurrentLoggedUserIDk)</param>
        private void MergeDocIntoUnifiedPdf(Dictionary<String, Object> mergingData)
        {
            if (mergingData.IsNotNull() && mergingData.Count > 0)
            {
            }
        }


        public String GetCategoryExplanatoryNotes()
        {
            View.RequirementCategoryName = String.Empty;
            if (View.RequirementCategoryId > AppConsts.NONE && View.SelectedTenantId > AppConsts.NONE)
            {
                if (View.RotationPackageDetail.IsNotNull() && View.RotationPackageDetail.LstRequirementCategory.IsNotNull())
                {
                    RequirementCategoryContract requirementCategory = View.RotationPackageDetail.LstRequirementCategory.FirstOrDefault(cond =>
                                                                                                        cond.RequirementCategoryID == View.RequirementCategoryId);

                    View.RequirementCategoryName = requirementCategory.IsNotNull() ? requirementCategory.RequirementCategoryName : String.Empty;
                    //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes).
                    View.RequirementCategoryURL = requirementCategory.IsNotNull() ? requirementCategory.RequirementDocumentLink : String.Empty;
                    View.RequirementCategoryURLLabel = requirementCategory.IsNotNull() ? requirementCategory.RequirementDocumentLinkLabel : String.Empty;

                }

                ServiceRequest<Int32, String, String> serviceRequest = new ServiceRequest<Int32, String, String>();
                serviceRequest.Parameter1 = View.RequirementCategoryId;
                serviceRequest.Parameter2 = LCObjectType.RequirementCategory.GetStringValue();
                serviceRequest.Parameter3 = LCContentType.ExplanatoryNotes.GetStringValue();
                serviceRequest.SelectedTenantId = View.SelectedTenantId;

                var _serviceResponse = _applicantClinicalRotationProxy.GetExplanatoryNotes(serviceRequest);
                return _serviceResponse.Result;
            }
            return String.Empty;
        }

        //UAT-1523: Addition a notes box for each rotation for the student to input information
        public Boolean UpdateRequirementPackageSubscriptionNotes(string notes)
        {
            ServiceRequest<String, Int32, Int32> serviceRequest = new ServiceRequest<String, Int32, Int32>();
            serviceRequest.Parameter1 = notes;
            serviceRequest.Parameter2 = View.RotationSubscriptionDetail.RequirementPackageSubscriptionID;
            serviceRequest.Parameter3 = View.OrgUsrID;
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            var _serviceResponse = _applicantClinicalRotationProxy.UpdateRequirementPackageSubscriptionNotes(serviceRequest);
            return _serviceResponse.Result;
        }

        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)

        private void GetComplianceRequiredCategories()
        {
            if (View.RequirementPackageId > AppConsts.NONE)
            {
                ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
                serviceRequest.SelectedTenantId = View.SelectedTenantId;
                serviceRequest.Parameter = View.RequirementPackageId;
                var _serviceResponse = _clientRotationProxy.GetComplianceRequiredRotCatForPackage(serviceRequest);
                View.LstComplianceRqdCategoryMapping = _serviceResponse.Result;
            }
            else
            {
                View.LstComplianceRqdCategoryMapping = new Dictionary<Int32, Boolean>();
            }
        }

        public bool IsApplicantDropped()
        {
            ServiceRequest<Int32, Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32, Int32>();
            serviceRequest.Parameter1 = View.SelectedTenantId;
            serviceRequest.Parameter2 = View.OrgUsrID;
            serviceRequest.Parameter3 = View.ClinicalRotationID;
            var _serviceResponse = _applicantClinicalRotationProxy.IsApplicantDropped(serviceRequest);
            return _serviceResponse.Result;
        }
        #endregion

        #region UAT-3458
        private void GetRequirementItemsAboutToExpire()
        {
            if (View.RequirementPackageSubscriptionID > AppConsts.NONE)
            {
                ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
                serviceRequest.Parameter1 = View.SelectedTenantId;
                serviceRequest.Parameter2 = View.RequirementPackageSubscriptionID;
                var _serviceResponse = _clientRotationProxy.GetRequirementItemsAboutToExpire(serviceRequest);
                View.lstRequirementExpiringItem = _serviceResponse.Result;
            }
            else
            {
                View.lstRequirementExpiringItem = new List<RequirementExpiringItemListContract>();
            }
        }
        #endregion

        /*UAT-2751*/
        public string GetApplicantNameByApplicantId(Int32 applicantId, Int32 tenantID)
        {
            return ComplianceDataManager.GetApplicantNameByApplicantId(applicantId, tenantID);
        }
        /*UAT-2751 End here*/

        #region UAT-2975

        public void SyncVerificationDataToFlat()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.SelectedTenantId = View.SelectedTenantId;
            serviceRequest.Parameter = View.RequirementPackageSubscriptionID;

            _applicantClinicalRotationProxy.SyncRequirementVerificationToFlatData(serviceRequest);
        }
        #endregion

        public Tuple<Boolean, Int32> CheckItemPayment(Int32 applicantReqItemID, Int32 reqItemId)
        {
            if (reqItemId > AppConsts.NONE)
            {
                return ComplianceDataManager.CheckItemPayment(View.SelectedTenantId, applicantReqItemID, reqItemId, true);
            }
            return new Tuple<bool, int>(true, 0);
        }

        //UAT 3106 
        public void GetClientSettingByCode()
        {
            ClientSetting _clientSettings = ComplianceDataManager.GetClientSetting(View.SelectedTenantId, Setting.EXECUTE_COMPLIANCE_RULE_WHEN_OPTIONAL_CATEGORY_COMPLIANCE_RULE_MET.GetStringValue());
            if (_clientSettings.IsNullOrEmpty())
            {
                View.IsOptionalCategoryClientSettingEnabled = false;
            }
            else
            {
                View.IsOptionalCategoryClientSettingEnabled = _clientSettings.CS_SettingValue == AppConsts.STR_ONE ? true : false;
            }
        }

        public void GetQuizConfigurationSetting()
        {
            String _key = "LockSubmittedQuiz";
            Entity.ClientEntity.AppConfiguration QuizSetting = ComplianceDataManager.GetAppConfiguration(_key, View.SelectedTenantId);
            if (QuizSetting.IsNullOrEmpty())
            {
                Entity.AppConfiguration MasterQuizSetting = SecurityManager.GetAppConfiguration(_key);
                View.QuizConfigSetting = MasterQuizSetting.AC_Value.ToString();
            }
            else
            {
                View.QuizConfigSetting = QuizSetting.AC_Value.ToString();
            }
        }

        public Boolean IsItemStatusApproved(Int32 reqItemId, Int32 reqCatId, Int32 reqItemDataID)
        {
            if (reqCatId > AppConsts.NONE && reqItemId > AppConsts.NONE && reqItemDataID > AppConsts.NONE)
            {
                var reqCatData = View.RotationSubscriptionDetail.ApplicantRequirementCategoryData.Where(g => g.RequirementCategoryID == reqCatId).FirstOrDefault();
                if (!reqCatData.IsNullOrEmpty())
                {
                    var reqItemData = reqCatData.ApplicantRequirementItemData.Where(d => d.RequirementItemID == reqItemId && d.RequirementItemDataID == reqItemDataID).FirstOrDefault();
                    if (!reqItemData.IsNullOrEmpty())
                    {
                        if (reqItemData.RequirementItemStatusCode.Equals(RequirementItemStatus.APPROVED.GetStringValue()))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #region UAT-3737

        public string GetInstructorNameByOrganizationUserId(Int32 organizationUserId)
        {
            return SecurityManager.GetInstructorNameByOrganizationUserId(organizationUserId);
        }
        #endregion

        #region UAT-3805
        private void CreateItemDocNotificationRequestData(List<Int32> approvedCategoryIDs, List<ItemDocNotificationRequestDataContract> itemDocNotificationRequestData)
        {
            String approvedCategoryIds = approvedCategoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", approvedCategoryIDs);
            ItemDocNotificationRequestDataContract itemDocRequestData = new ItemDocNotificationRequestDataContract();

            itemDocRequestData.TenantID = View.SelectedTenantId;
            itemDocRequestData.CategoryIds = Convert.ToString(View.RequirementCategoryDataContract.RequirementCategoryID);
            itemDocRequestData.ApplicantOrgUserID = View.ApplicantID;
            itemDocRequestData.ApprovedCategoryIds = approvedCategoryIds;
            itemDocRequestData.RequestTypeCode = lkpUseTypeEnum.ROTATION.GetStringValue();
            itemDocRequestData.PackageSubscriptionID = null;
            itemDocRequestData.RPS_ID = View.RequirementPackageSubscriptionID;
            itemDocRequestData.CurrentLoggedInUserID = View.CurrentLoggedInUserId;
            itemDocNotificationRequestData.Add(itemDocRequestData);

            //ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(View.SelectedTenantId, Convert.ToString(View.RequirementCategoryDataContract.RequirementCategoryID)
            //                                                               , View.ApplicantID, approvedCategoryIds, lkpUseTypeEnum.ROTATION.GetStringValue()
            //                                                               , null, View.RequirementPackageSubscriptionID);
        }
        #endregion

        public Int32 GetApplicantRequirementFieldData(Int32 RequirementItemDataID, Int32 RequirementFieldID)
        {
            if (RequirementItemDataID > AppConsts.NONE && RequirementFieldID > AppConsts.NONE && View.SelectedTenantId > AppConsts.NONE)
            {
                ServiceRequest<Int32, Int32, String> serviceRequest = new ServiceRequest<Int32, Int32, String>();
                serviceRequest.Parameter1 = RequirementItemDataID;
                serviceRequest.Parameter2 = RequirementFieldID;
                serviceRequest.SelectedTenantId = View.SelectedTenantId;

                var _serviceResponse = _applicantClinicalRotationProxy.GetApplicantRequirementFieldData(serviceRequest);
                return _serviceResponse.Result;
            }
            return AppConsts.NONE;
        }

        #region UAT-4254

        public void GetCurrentCategoryDocUrls()
        {
            View.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();

            if (!View.RequirementCategoryId.IsNullOrEmpty() && View.RequirementCategoryId > AppConsts.NONE)
            {
                ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
                serviceRequest.Parameter1 = View.RequirementCategoryId;
                serviceRequest.Parameter2 = View.SelectedTenantId;
                var _serviceResponse = _applicantClinicalRotationProxy.GetRequirementCatDocUrls(serviceRequest);

                if (!_serviceResponse.IsNullOrEmpty() && !_serviceResponse.Result.IsNullOrEmpty())
                {
                    View.lstReqCatDocUrls = _serviceResponse.Result;
                }
            }
        }

        #endregion
    }

}
