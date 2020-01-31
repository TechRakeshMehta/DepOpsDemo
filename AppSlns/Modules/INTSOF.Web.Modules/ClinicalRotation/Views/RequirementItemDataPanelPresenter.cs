using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using INTSOF.ServiceUtil;
using INTSOF.Contracts;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.ApplicantOperations;
using System.Data;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RequirementItemDataPanelPresenter : Presenter<IRequirementItemDataPanel>
    {
        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        //private Applicant _clientRotationProxy
        //{
        //    get
        //    {
        //        return new ClinicalRotationProxy();
        //    }
        //}

        private ApplicantClinicalRotationProxy _applicantClinicalRotationProxy
        {
            get
            {
                return new ApplicantClinicalRotationProxy();
            }
        }

        /// <summary>
        /// Get the data to Generate and bind the Items for selected Category.
        /// </summary>
        public void GetCategoryData()
        {
            ServiceRequest<Int32, List<Int32>, Int32, Int32> serviceRequest = new ServiceRequest<Int32, List<Int32>, Int32, Int32>();
            serviceRequest.Parameter1 = View.CurrentReqSubsciptionId;
            serviceRequest.Parameter2 = new List<Int32> { View.CurrentReqCategoryId };
            serviceRequest.Parameter3 = View.TenantId;
            serviceRequest.Parameter4 = View.CurrentRotationId;
            var _serviceResponse = _clientRotationProxy.GetRequirementItemsByCategoryId(serviceRequest);
            View.lstCategoryData = _serviceResponse.Result;
        }

        //UAT-4461
        public void GetApplicantDataByRPSid()
        {
            View.lstApplicantDataForNavigation = ComplianceDataManager.GetApplicantDataByRPSid(View.CurrentReqSubsciptionId, View.CurrentRotationId,View.TenantId);
        }

        /// <summary>
        /// Save the data of the Items
        /// </summary>
        public Dictionary<Int32, String> SaveData()
        {
            //UAT-3805
            List<Int32> approvedCategoryIDs = new List<Int32>();
            List<ItemDocNotificationRequestDataContract> itemDocNotificationRequestData = new List<ItemDocNotificationRequestDataContract>();
            if (View.DataToSave.RPSId > 0)
            {
                approvedCategoryIDs = View.DataToSave.lstData.Select(slct => slct.CatId).ToList();
                approvedCategoryIDs = ProfileSharingManager.GetApprovedCategorIDs(View.TenantId, View.DataToSave.RPSId
                                                                                      , approvedCategoryIDs, lkpUseTypeEnum.ROTATION.GetStringValue());
            }


            ServiceRequest<Int32, String> prevserviceRqst = new ServiceRequest<Int32, String>();
            prevserviceRqst.Parameter1 = View.TenantId;
            prevserviceRqst.Parameter2 = Convert.ToString(View.DataToSave.RPSId);
            var _prevserviceRspse = _applicantClinicalRotationProxy.GetPackageSubscriptionCategoryStatus(prevserviceRqst);
            var prevSubscriptionStatus = _prevserviceRspse.Result;

            ServiceRequest<RequirementVerificationData, Int32> serviceRequest = new ServiceRequest<RequirementVerificationData, Int32>();
            serviceRequest.Parameter1 = View.DataToSave;
            serviceRequest.Parameter2 = View.TenantId;

            Boolean isNewPackage = false;

            var prevData = ApplicantRequirementManager.GetMailDataForItemSubmitted(Convert.ToString(View.DataToSave.RPSId), View.TenantId); //UAT-2905

            //var _serviceResponse = _clientRotationProxy.SaveVerificationData(serviceRequest, ref isNewPackage);
            var _serviceResponse = RequirementVerificationManager.SaveVerificationData(View.DataToSave, View.CurrentLoggedInUserId, View.TenantId, ref isNewPackage);
            Boolean isSuccess = isNewPackage ? !_serviceResponse.Keys.Any() : !_serviceResponse.Keys.Any(k => k == AppConsts.MINUS_ONE);
            if (isSuccess)
            {
                #region  UAT-3273- Get status before rule execution
                ServiceRequest<Int32, string> request = new ServiceRequest<Int32, string>();
                request.Parameter1 = View.TenantId;
                request.Parameter2 = Convert.ToString(View.DataToSave.RPSId);
                // var dataBeforeRuleExecution = _applicantClinicalRotationProxy.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(request);
                var dataBeforeRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(View.TenantId, Convert.ToString(View.DataToSave.RPSId));
                #endregion

                if (!isNewPackage)
                {
                    EvaluateRequirementBuisnessRules();
                }
                else
                {
                    EvaluateRequirementDynamicBuisnessRules();

                    List<int> lstItemDataID = new List<int>();

                    foreach (var catItemData in View.DataToSave.lstData)
                    {
                        foreach (var itemData in catItemData.lstItemData)
                        {
                            lstItemDataID.Add(itemData.ApplicantItemDataId);
                        }
                    }

                    //if (!lstItemDataID.IsNullOrEmpty())
                    //{
                    //    string ids = string.Join(",", lstItemDataID);
                    //    //UAT-3112:-
                    //    ComplianceDataManager.SaveBadgeFormNotificationData(View.TenantId, null, ids, null, View.CurrentLoggedInUserId);
                    //}
                }

                #region  UAT-3273- Get status after rule execution
                //var dataAfterRuleExecution = _applicantClinicalRotationProxy.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(request);
                var dataAfterRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(View.TenantId, Convert.ToString(View.DataToSave.RPSId));
                ProfileSharingManager.CheckAndSendNotificationForNewlyApprovedRotations(dataBeforeRuleExecution, dataAfterRuleExecution, View.TenantId);
                #endregion


                #region UAT-3957
                //Step 1 we will filter records from  View.lstCategoryData as " View.lstCategoryData" contains old data to bind item data panel and "View.DataToSave.lstData" contains new data to save
                if (!View.lstCategoryData.IsNullOrEmpty() && !View.DataToSave.IsNullOrEmpty() && !View.DataToSave.lstData.IsNullOrEmpty())
                {
                    List<Int32> lstItemDataIdsToSendEmail = new List<Int32>();

                    if (!View.DataToSave.lstData[0].IsNullOrEmpty())
                    {
                        foreach (var item in View.DataToSave.lstData)
                        {
                            Int32 ApplicantCatDataId = item.ApplicantCatDataId;
                            List<RequirementVerificationDetailContract> oldData = View.lstCategoryData.Where(con => con.ApplReqCatDataId == ApplicantCatDataId).ToList();

                            foreach (var item111 in item.lstItemData)
                            {
                                Int32 applicantItemDataId = item111.ApplicantItemDataId;
                                RequirementVerificationDetailContract data = oldData.Where(con => con.ApplReqItemDataId == applicantItemDataId).FirstOrDefault();
                                if (!data.IsNullOrEmpty() && data.ItemStatusCode != item111.ItemStatusCode && item111.ItemStatusCode == RequirementItemStatus.NOT_APPROVED.GetStringValue())
                                {
                                    lstItemDataIdsToSendEmail.Add(applicantItemDataId);
                                }
                            }
                        }
                    }
                    if (!lstItemDataIdsToSendEmail.IsNullOrEmpty() && lstItemDataIdsToSendEmail.Count > AppConsts.NONE)
                    {
                        //SendMail after crosscheck in parallel task.
                        SendMailOnItemRejection(lstItemDataIdsToSendEmail);
                    }
                }

                #endregion


                SendNotificationsForItmAndPkgStatusChange();

                //UAT-3805
                CreateItemDocNotificationRequestData(approvedCategoryIDs, itemDocNotificationRequestData);

                ServiceRequest<Int32, String> serviceRqst = new ServiceRequest<Int32, String>();
                serviceRqst.Parameter1 = View.TenantId;
                serviceRqst.Parameter2 = Convert.ToString(View.DataToSave.RPSId);
                var _serviceRspse = _applicantClinicalRotationProxy.GetPackageSubscriptionCategoryStatus(serviceRqst);
                var SubscriptionStatus = _serviceRspse.Result;

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
                    if (SubscriptionStatus[0].RequirementPackageSubscriptionID.Equals(View.DataToSave.RPSId))
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
                        dictMailData.Add(EmailFieldConstants.TENANT_ID, View.TenantId);
                        dictMailData.Add(EmailFieldConstants.STATUS, status);

                        Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();


                        //UAT-3160
                        String rotationHierarchyIDs = String.Empty;
                        if (View.DataToSave.RPSId > AppConsts.NONE)
                        {
                            rotationHierarchyIDs = ApplicantRequirementManager.GetRotationHierarchyIdsBasedOnSubscriptionID(View.TenantId, View.DataToSave.RPSId);
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
                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_ALL_ROTATION_CATEGORIES_APPROVED_OR_PENDING_REVIEW, dictMailData, mockData, View.TenantId, -1, null, null, true, false, null, rotationHierarchyIDs, SubscriptionStatus[0].RotationID);

                        //UAT-4015
                        ApplicantRequirementManager.SendMailToInstPrecpReqPKgCompliantStatus(View.TenantId, View.DataToSave.RPSId, View.CurrentLoggedInUserId);


                    }
                }

                #region UAT-2905 : Notification to Client admin when item is submitted.

                List<Int32> lstItemIds = new List<Int32>();
                var _lstData = View.lstCategoryData;
                var _distinctCatIds = _lstData.Select(vdd => vdd.CatId).Distinct().ToList();
                foreach (var catId in _distinctCatIds)
                {
                    lstItemIds = _lstData.Where(vdd => vdd.CatId == catId).DistinctBy(cond => cond.ItemId).Select(sel => sel.ItemId).ToList();
                    //foreach (var _item in _distinctItems)
                    //{
                    //    var itemId = _item.ItemId;
                    //    lstItemIds.Add(itemId);
                    //}
                }

                // var prevData = ApplicantRequirementManager.GetMailDataForItemSubmitted(Convert.ToString(View.DataToSave.RPSId), View.TenantId, String.Join(",",lstItemIds));

                ComplianceDataManager.SendNotificationToAdminForItemSubmitted(true, View.TenantId, Convert.ToString(View.DataToSave.RPSId), String.Join(",", lstItemIds), prevData, View.CurrentLoggedInUserId);

                #endregion

                //UAT-2975
                SyncVerificationDataToFlat();
                #region UAT - 3164
                //Getting target Package Subscription Ids
                ServiceRequest<Int32, Int32> rotReqSubsServiceRequest = new ServiceRequest<Int32, Int32>();
                rotReqSubsServiceRequest.SelectedTenantId = View.TenantId;
                rotReqSubsServiceRequest.Parameter1 = View.CurrentReqSubsciptionId;
                rotReqSubsServiceRequest.Parameter2 = View.CurrentReqCategoryId;
                var _rotReqSubsserviceResponse = _clientRotationProxy.GetTargetReqPackageSubscriptionIDsForSync(rotReqSubsServiceRequest);
                IEnumerable<DataRow> ReqSubIDs = _rotReqSubsserviceResponse.Result.AsEnumerable();
                String lstRPSID = String.Join(",", ReqSubIDs.Select(col => col["TargetRPS_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TargetRPS_ID"])).Distinct().ToList());
                var prevSyncRotData = Business.RepoManagers.ApplicantRequirementManager.GetMailDataForItemSubmitted(String.Join(",", lstRPSID), View.TenantId);

                //Calling Rotation Data live Movement
                ServiceRequest<Int32, Int32, Int32> rotMovementServiceRequest = new ServiceRequest<Int32, Int32, Int32>();
                rotMovementServiceRequest.SelectedTenantId = View.TenantId;
                rotMovementServiceRequest.Parameter1 = View.CurrentReqSubsciptionId;
                rotMovementServiceRequest.Parameter2 = View.CurrentReqCategoryId;
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
                    RequirementItemId = col["ReqItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ReqItemID"]),
                }).ToList());

                //Sending Notifications
                foreach (ApplicantRequirementParameterContract item in rotData.DistinctBy(cond => new { cond.RequirementPkgSubscriptionId }))
                {
                    //Sending Notification on Rotation pkg status change 
                    String previousSynPackageStatusCode = ReqSubIDs.Where(cond => Convert.ToInt32(cond["TargetRPS_ID"]) == item.RequirementPkgSubscriptionId).Select(sel => Convert.ToString(sel["OldPackageStatus"])).FirstOrDefault();
                    ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(View.TenantId, item.RequirementPkgSubscriptionId, View.CurrentLoggedInUserId, previousSynPackageStatusCode);

                    //Sending BadgeForm Notification
                    List<Int32> lstARID = rotData.Where(cond => cond.RequirementPkgSubscriptionId == item.RequirementPkgSubscriptionId && cond.AppRequirementItemDataID != AppConsts.NONE).Select(s => s.AppRequirementItemDataID).Distinct().ToList();
                    if (!lstARID.IsNullOrEmpty())
                    {
                        string arid = string.Join(",", lstARID);
                        ComplianceDataManager.SaveBadgeFormNotificationData(View.TenantId, null, arid, null, View.CurrentLoggedInUserId);
                    }

                    //Sending PackageNotificationMail
                    if (!item.prevCategoryStatusCode.Equals(item.NewCategoryStatusCode))
                    {
                        ServiceRequest<Int32, String> rotServiceRqst = new ServiceRequest<Int32, String>();
                        rotServiceRqst.Parameter1 = View.TenantId;
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
                            dictMailData.Add(EmailFieldConstants.TENANT_ID, View.TenantId);
                            dictMailData.Add(EmailFieldConstants.STATUS, status);

                            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                            mockData.UserName = rotSubscriptionStatus[0].UserName;
                            mockData.EmailID = rotSubscriptionStatus[0].Email;
                            mockData.ReceiverOrganizationUserID = rotSubscriptionStatus[0].OrganizationUserID;

                            //UAT-3160
                            String rotationHierarchyIDs = String.Empty;
                            if (item.RequirementPkgSubscriptionId > AppConsts.NONE)
                            {
                                rotationHierarchyIDs = ApplicantRequirementManager.GetRotationHierarchyIdsBasedOnSubscriptionID(View.TenantId, item.RequirementPkgSubscriptionId);
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
                            CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_ALL_ROTATION_CATEGORIES_APPROVED_OR_PENDING_REVIEW, dictMailData, mockData, View.TenantId, -1, null, null, true, false, null, rotationHierarchyIDs, rotSubscriptionStatus[0].RotationID);

                            //UAT-4015
                            ApplicantRequirementManager.SendMailToInstPrecpReqPKgCompliantStatus(View.TenantId, item.RequirementPkgSubscriptionId, View.CurrentLoggedInUserId);

                        }
                    }

                    // Sending Notification To Admin For ItemSubmitted
                    ComplianceDataManager.SendNotificationToAdminForItemSubmitted(true, View.TenantId, item.RequirementPkgSubscriptionId.ToString(), String.Empty, prevSyncRotData, View.CurrentLoggedInUserId);

                    //UAT-3805

                    List<Int32> categoryIds = rotData.Where(cond => cond.RequirementPkgSubscriptionId == item.RequirementPkgSubscriptionId
                                                                     && cond.prevCategoryStatusCode != RequirementCategoryStatus.APPROVED.GetStringValue())
                                                      .Select(s => s.RequirementCategoryId).Distinct().ToList();

                    ItemDocNotificationRequestDataContract itemDocRequestData = new ItemDocNotificationRequestDataContract();

                    String categoryIDs = String.Join(",", categoryIds);

                    itemDocRequestData.TenantID = View.TenantId;
                    itemDocRequestData.CategoryIds = categoryIDs;
                    itemDocRequestData.ApplicantOrgUserID = View.ApplicantId;
                    itemDocRequestData.ApprovedCategoryIds = String.Empty;
                    itemDocRequestData.RequestTypeCode = lkpUseTypeEnum.ROTATION.GetStringValue();
                    itemDocRequestData.PackageSubscriptionID = null;
                    itemDocRequestData.RPS_ID = item.RequirementPkgSubscriptionId;
                    itemDocRequestData.CurrentLoggedInUserID = View.CurrentLoggedInUserId;
                    itemDocNotificationRequestData.Add(itemDocRequestData);
                }
                //UAT-3805
                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                dicParam.Add("CategoryData", itemDocNotificationRequestData);
                ProfileSharingManager.RunParallelTaskItemDocNotificationOnCatApproval(dicParam, LoggerService, ExceptiomService);

                #endregion

            }
            return _serviceResponse;
        }

        public void GetRequirementPackageCategoryData()
        {
            View.lstReqPkgSubData = RequirementVerificationManager.GetRequirementPackageCategoryData(View.CurrentReqSubsciptionId, View.TenantId, View.CurrentRotationId);
        }
        public void GetSubscriptionDetail()
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = View.DataToSave.RPSId;
            serviceRequest.Parameter2 = View.TenantId;
            var _serviceResponse = _applicantClinicalRotationProxy.GetRequirementPackageSubscriptionData(serviceRequest);
            View.RotationSubscriptionDetail = _serviceResponse.Result1;

        }


        /// <summary>
        /// Send Notifications for Itm and Pkg Status Change
        /// </summary>
        private void SendNotificationsForItmAndPkgStatusChange()
        {
            if (!View.lstCategoryData.IsNullOrEmpty())
            {
                var _lstData = View.lstCategoryData;

                var _distinctCatIds = _lstData.Select(vdd => vdd.CatId).Distinct().ToList();
                foreach (var catId in _distinctCatIds)
                {
                    var _distinctItems = _lstData.Where(vdd => vdd.CatId == catId).DistinctBy(cond => cond.ItemId).ToList();
                    foreach (var _item in _distinctItems)
                    {
                        //UAT-2273: Rotation three panel screen optimization
                        //ComplianceDataManager.SendNotificationOnItemStatusChangedToReviewStatus(true, View.TenantId, View.CurrentReqSubsciptionId, AppConsts.NONE, catId,
                        //                                                                        _item.ItemId, View.CurrentLoggedInUserId, View.ApplicantId, _item.ItemStatusCode);
                        CallParallelTaskForItemStatusChangedMail(catId, _item.ItemId, _item.ItemStatusCode);
                    }
                }

                //UAT-2273: Rotation three panel screen optimization
                //ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(View.TenantId, View.CurrentReqSubsciptionId, View.CurrentLoggedInUserId, View.lstCategoryData[0].PkgStatusCode);
                CallParallelTaskForPkgStatusChangedMail(View.lstCategoryData[0].PkgStatusCode);
            }
        }

        /// <summary>
        /// Execute the business rules after data is saved
        /// </summary>
        private void EvaluateRequirementBuisnessRules()
        {
            var _lstData = View.lstCategoryData;
            var _currentPkgId = _lstData.First().PkgId;

            List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();

            ruleObjectMappingList.Add(new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Package.GetStringValue(),
                RuleObjectId = Convert.ToString(_currentPkgId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            });

            var _distinctCatIds = _lstData.Select(vdd => vdd.CatId).Distinct().ToList();
            foreach (var catId in _distinctCatIds)
            {
                ruleObjectMappingList.Add(new RequirementRuleObject
                {
                    RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                    RuleObjectId = Convert.ToString(catId),
                    RuleObjectParentId = Convert.ToString(_currentPkgId)
                });

                var _distinctItemIds = _lstData.Where(vdd => vdd.CatId == catId).Select(vdd => vdd.ItemId).Distinct().ToList();
                foreach (var itemId in _distinctItemIds)
                {
                    ruleObjectMappingList.Add(new RequirementRuleObject
                    {
                        RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                        RuleObjectId = Convert.ToString(itemId),
                        RuleObjectParentId = Convert.ToString(catId)
                    });
                }
            }

            ServiceRequest<List<RequirementRuleObject>, Int32> serviceRequest = new ServiceRequest<List<RequirementRuleObject>, Int32>();
            serviceRequest.Parameter1 = ruleObjectMappingList;
            serviceRequest.Parameter2 = View.CurrentReqSubsciptionId;
            serviceRequest.SelectedTenantId = View.TenantId;

            _clientRotationProxy.ExecuteRequirementObjectBuisnessRules(serviceRequest);
        }

        private void EvaluateRequirementDynamicBuisnessRules()
        {
            var _lstData = View.lstCategoryData;
            var _currentPkgId = _lstData.First().PkgId;

            List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();

            var _distinctCatIds = _lstData.Select(vdd => vdd.CatId).Distinct().ToList();
            foreach (var catId in _distinctCatIds)
            {
                ruleObjectMappingList.Add(new RequirementRuleObject
                {
                    RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                    RuleObjectId = Convert.ToString(catId),
                    RuleObjectParentId = Convert.ToString(AppConsts.NONE)
                });

                var _distinctItemIds = _lstData.Where(vdd => vdd.CatId == catId).Select(vdd => vdd.ItemId).Distinct().ToList();
                foreach (var itemId in _distinctItemIds)
                {
                    ruleObjectMappingList.Add(new RequirementRuleObject
                    {
                        RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                        RuleObjectId = Convert.ToString(itemId),
                        RuleObjectParentId = Convert.ToString(catId)
                    });
                    var _distinctFieldIds = _lstData.Where(vdd => vdd.CatId == catId && vdd.ItemId == itemId).Select(vdd => vdd.FieldId).Distinct().ToList();
                    foreach (var fieldId in _distinctFieldIds)
                    {
                        ruleObjectMappingList.Add(new RequirementRuleObject
                        {
                            RuleObjectTypeCode = ObjectType.Compliance_ATR.GetStringValue(),
                            RuleObjectId = Convert.ToString(fieldId),
                            RuleObjectParentId = Convert.ToString(itemId)
                        });
                    }
                }
            }

            ServiceRequest<List<RequirementRuleObject>, Int32> serviceRequest = new ServiceRequest<List<RequirementRuleObject>, Int32>();
            serviceRequest.Parameter1 = ruleObjectMappingList;
            serviceRequest.Parameter2 = View.CurrentReqSubsciptionId;
            serviceRequest.SelectedTenantId = View.TenantId;

            _applicantClinicalRotationProxy.EvaluateRequirementPostSubmitRules(serviceRequest);
        }

        #region UAT-2273: Rotation three panel screen optimization

        /// <summary>
        /// Call Parallel Task for Item Status Changed Mail
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemStatusCode"></param>
        public void CallParallelTaskForItemStatusChangedMail(Int32 catId, Int32 itemId, String itemStatusCode)
        {
            Dictionary<String, Object> mailData = new Dictionary<String, Object>();
            mailData.Add("tenantID", View.TenantId);
            mailData.Add("currentReqSubsciptionId", View.CurrentReqSubsciptionId);
            mailData.Add("catId", catId);
            mailData.Add("itemId", itemId);
            mailData.Add("itemStatusCode", itemStatusCode);
            mailData.Add("currentLoggedInUserId", View.CurrentLoggedInUserId);
            mailData.Add("applicantId", View.ApplicantId);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(SendNotificationOnItemStatusChangedToReviewStatus, mailData, LoggerService, ExceptiomService);
        }

        /// <summary>
        /// Send Notification on Item Status Changed to Review Status
        /// </summary>
        /// <param name="data"></param>
        public void SendNotificationOnItemStatusChangedToReviewStatus(Dictionary<String, Object> data)
        {
            Int32 tenantID = Convert.ToInt32(data.GetValue("tenantID"));
            Int32 currentReqSubsciptionId = Convert.ToInt32(data.GetValue("currentReqSubsciptionId"));
            Int32 catId = Convert.ToInt32(data.GetValue("catId"));
            Int32 itemId = Convert.ToInt32(data.GetValue("itemId"));
            String itemStatusCode = Convert.ToString(data.GetValue("itemStatusCode"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("currentLoggedInUserId"));
            Int32 applicantId = Convert.ToInt32(data.GetValue("applicantId"));

            ComplianceDataManager.SendNotificationOnItemStatusChangedToReviewStatus(true, tenantID, currentReqSubsciptionId, AppConsts.NONE, catId,
                                                                                                itemId, currentLoggedInUserId, applicantId, itemStatusCode);
        }

        /// <summary>
        /// Call Parallel Task for Pkg Status Changed Mail
        /// </summary>
        /// <param name="pkgStatusCode"></param>
        public void CallParallelTaskForPkgStatusChangedMail(String pkgStatusCode)
        {
            Dictionary<String, Object> mailData = new Dictionary<String, Object>();
            mailData.Add("tenantID", View.TenantId);
            mailData.Add("currentReqSubsciptionId", View.CurrentReqSubsciptionId);
            mailData.Add("pkgStatusCode", pkgStatusCode);
            mailData.Add("currentLoggedInUserId", View.CurrentLoggedInUserId);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

            ParallelTaskContext.PerformParallelTask(SendNotificationOnRotationPkgStatusChangedFromCompToNC, mailData, LoggerService, ExceptiomService);
        }

        /// <summary>
        /// Send Notification on Rotation Pkg Status Changed from Comp to NC
        /// </summary>
        /// <param name="data"></param>
        public void SendNotificationOnRotationPkgStatusChangedFromCompToNC(Dictionary<String, Object> data)
        {
            Int32 tenantID = Convert.ToInt32(data.GetValue("tenantID"));
            Int32 currentReqSubsciptionId = Convert.ToInt32(data.GetValue("currentReqSubsciptionId"));
            String pkgStatusCode = Convert.ToString(data.GetValue("pkgStatusCode"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("currentLoggedInUserId"));

            ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(tenantID, currentReqSubsciptionId, currentLoggedInUserId, pkgStatusCode);
        }

        #endregion

        #region UAT 2371
        public void GetSystemEntityUserPermission(Int32 clientOrganisationUserID, int tenantId)
        {
            ServiceRequest<Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32>();
            serviceRequest.Parameter1 = clientOrganisationUserID;
            serviceRequest.Parameter2 = tenantId;
            var _serviceResponse = _clientRotationProxy.GetSystemEntityUserPermission(serviceRequest);
            View.EntityPermissionName = _serviceResponse.Result.SEP_PermissionName;
        }
        #endregion

        #region UAT-2975

        public void SyncVerificationDataToFlat()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.SelectedTenantId = View.TenantId;
            serviceRequest.Parameter = View.DataToSave.RPSId;

            _applicantClinicalRotationProxy.SyncRequirementVerificationToFlatData(serviceRequest);
        }
        #endregion

        #region UAT-3805
        private void CreateItemDocNotificationRequestData(List<Int32> approvedCategoryIDs, List<ItemDocNotificationRequestDataContract> itemDocNotificationRequestData)
        {
            String approvedCategoryIds = approvedCategoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", approvedCategoryIDs);
            String categoryIds = String.Join(",", View.DataToSave.lstData.Select(x => x.CatId).ToList());

            ItemDocNotificationRequestDataContract itemDocRequestData = new ItemDocNotificationRequestDataContract();

            itemDocRequestData.TenantID = View.TenantId;
            itemDocRequestData.CategoryIds = categoryIds;
            itemDocRequestData.ApplicantOrgUserID = View.ApplicantId;
            itemDocRequestData.ApprovedCategoryIds = approvedCategoryIds;
            itemDocRequestData.RequestTypeCode = lkpUseTypeEnum.ROTATION.GetStringValue();
            itemDocRequestData.PackageSubscriptionID = null;
            itemDocRequestData.RPS_ID = View.DataToSave.RPSId;
            itemDocRequestData.CurrentLoggedInUserID = View.CurrentLoggedInUserId;
            itemDocNotificationRequestData.Add(itemDocRequestData);


            //ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(View.TenantId, categoryIds
            //                                                               , View.ApplicantId, approvedCategoryIds, lkpUseTypeEnum.ROTATION.GetStringValue()
            //                                                               , null, View.DataToSave.RPSId);
        }
        #endregion

        #region UAT-3957
        private void SendMailOnItemRejection(List<Int32> lstRejectedItemDataIds)
        {
            String rejectedItemIds = String.Join(",", lstRejectedItemDataIds);
            ServiceRequest<String, Int32> serviceRequest = new ServiceRequest<String, Int32>();
            serviceRequest.Parameter1 = rejectedItemIds;
            serviceRequest.Parameter2 = View.TenantId;

            var _serviceResponse = _clientRotationProxy.GetRequirementRejectedItemDetailsForMail(serviceRequest);

            List<RequirementItemRejectionContract> lstRequirementItemRejectionContract = _serviceResponse.Result;

            if (!lstRequirementItemRejectionContract.IsNullOrEmpty() && lstRequirementItemRejectionContract.Count > AppConsts.NONE)
            {
                CommunicationManager.SendMailOnRequirementItemRejection(lstRequirementItemRejectionContract, View.TenantId);
            }
        }
        #endregion
    }
}
